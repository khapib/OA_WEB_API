using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Reflection;
using System.Web.Http.Results;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.ERP;

using Dapper;
using Microsoft.Ajax.Utilities;
using Docker.DotNet.Models;
using OA_WEB_API.Repository.OA;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 系統共通功能邏輯
    /// </summary>
    public class CommonRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Model

        NoticeMode noticeMode = new NoticeMode();

        #endregion

        #region Repository

        FormRepository formRepository = new FormRepository();
        UserRepository userRepository = new UserRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        StepFlowRepository stepFlowRepository = new StepFlowRepository();

        #endregion

        #endregion

        #region - 方法 -

        #region - 測試 -

        #region - 刪除表單 -

        /// <summary>
        /// 刪除表單
        /// </summary>        
        public void PostFormDelete(FormRemoveQueryModel query)
        {
            var parameter = new List<SqlParameter>()
            {
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID},
            };
            strSQL = "";
            strSQL += "IF EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [NAME]='FM7T_" + query.IDENTIFY + "_M') ";
            strSQL += "	    DELETE [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_M] WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "IF EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [NAME]='FM7T_" + query.IDENTIFY + "_M2') ";
            strSQL += "     DELETE [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_M2] WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "IF EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [NAME]='FM7T_" + query.IDENTIFY + "_D') ";
            strSQL += "     DELETE [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_D] WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "IF EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [NAME]='FM7T_" + query.IDENTIFY + "_D2') ";
            strSQL += "     DELETE [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_D2] WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "IF EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [NAME]='GTV_Attachment')";
            strSQL += "     DELETE [BPMPro].[dbo].[GTV_Attachment] WHERE [RequisitionID]=@REQUISITION_ID";

            dbFun.DoTran(strSQL, parameter);
        }

        #endregion

        #endregion

        #region - 取得《會簽管理系統》網站根目錄 -

        /// <summary>
        /// 取得《會簽管理系統》網站根目錄
        /// </summary>
        public string GetWebBPM()
        {
            try
            {
                #region - 查詢 -

                strSQL = "";
                strSQL += "SELECT [Base_IP] AS [WEB_ROOT] ";
                strSQL += "FROM [NUP].[dbo].[NUP_Sys_System] ";
                strSQL += "WHERE [System]='WebBPM' ";

                #endregion

                return dbFun.DoQuery(strSQL).Rows[0]["WEB_ROOT"].ToString();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("取得《會簽管理系統》網站根目錄錯誤，原因：" + ex.Message);
                throw new Exception("取得《會簽管理系統》網站根目錄錯誤，原因：" + ex.Message);
            }
        }

        #endregion

        #region - 確認是否已起單且簽核中 -

        /// <summary>
        /// 確認是否已起單且簽核中
        /// </summary>                 
        public GTVApproveProgressResponse PostGTVInApproveProgress(GTVInApproveProgress query)
        {
            bool vResult = false;
            var BPMStatus = "";

            try
            {
                if (!String.IsNullOrEmpty(query.FORM_NO) || !String.IsNullOrWhiteSpace(query.FORM_NO))
                {
                    var formDistinguish = PostFormDistinguish(query.IDENTIFY);

                    var parameter = new List<SqlParameter>()
                    {
                        new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = query.FORM_NO }
                    };

                    strSQL = "";
                    strSQL += "SELECT TOP 1 ";
                    strSQL += "     [RequisitionID] AS [REQUISITION_ID] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_M] ";
                    strSQL += "WHERE [FormNo] = @FORM_NO ";
                    strSQL += "ORDER BY [ApplicantDateTime] DESC";

                    var dt = dbFun.DoQuery(strSQL, parameter);

                    var queryInfos = jsonFunction.JsonToObject<List<StepFlowQueryModel>>(jsonFunction.DataTableToJsonList(dt));
                    foreach (var s in queryInfos)
                    {
                        strREQ = s.REQUISITION_ID;
                        if (!String.IsNullOrEmpty(strREQ) || !String.IsNullOrWhiteSpace(strREQ)) break;
                    }

                    #region 確認是否有「成功起單」;【有】就判斷是否【簽核中或在草稿中】，【沒有】就從M表刪除起單失敗的單後「重新啟單」

                    if ((!String.IsNullOrEmpty(strREQ) || !String.IsNullOrWhiteSpace(strREQ)))
                    {
                        var formQueryModel = new FormQueryModel
                        {
                            REQUISITION_ID = strREQ
                        };

                        if (CommonRepository.PostDataHaveForm(formQueryModel))
                        {
                            #region 判斷是否簽核中或在草稿中【是】就不能起單

                            var stepFlowQueryModel = new StepFlowQueryModel
                            {
                                REQUISITION_ID = strREQ,
                            };
                            BPMStatus = stepFlowRepository.PostStepFlowSingle(stepFlowQueryModel);

                            //表單資料
                            var formData = formRepository.PostFormData(formQueryModel);

                            if (int.Parse(BPMSysStatus.PROGRESS) == formData.FORM_STATUS)
                            {
                                vResult = true;
                            }
                            else
                            {
                                if (BPMStatus == BPMStatusCode.PROGRESS || BPMStatus == BPMStatusCode.DRAFT)
                                {
                                    vResult = true;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            //沒有成功的表單就直接在資料表刪除
                            var formRemoveQueryModel = new FormRemoveQueryModel
                            {
                                IDENTIFY = query.IDENTIFY,
                                REQUISITION_ID = strREQ
                            };
                            //刪除表單
                            //PostFormDelete(formRemoveQueryModel);
                        }
                    }

                    #endregion

                }

                var response = new GTVApproveProgressResponse()
                {
                    REQUISITION_ID = strREQ,
                    vResult = vResult,
                    BPMStatus = BPMStatus
                };

                return response;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("確認是否已起單且簽核中，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 表單列表 -

        /// <summary>
        /// 表單列表
        /// </summary>        
        public IList<FormMainTree> PostGTVBPMFormTree(FormFilter filter)
        {
            try
            {
                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [FolderID] AS [FOLDER_ID], ";
                strSQL += "     [FolderName] AS [FOLDER_NAME], ";
                strSQL += "     [IDENTIFY], ";
                strSQL += "     [FormName] AS [FORM_NAME] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_FormDistinguish] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "ORDER BY [FormType] ";
                var formMainData = dbFun.DoQuery(strSQL).ToList<FormMainData>();

                #region - 表單篩選 -

                if (filter != null && filter.IDENTIFY != null)
                {
                    if (filter.IDENTIFY.Count > 0)
                    {
                        formMainData = formMainData
                                        .Where(D => filter.IDENTIFY.Contains(D.IDENTIFY))
                                        .Select(D => D)
                                        .ToList();
                    }
                }

                #endregion

                strJson = jsonFunction.ObjectToJSON(formMainData);
                var formMainTree = jsonFunction.JsonToObject<List<FormMainTree>>(strJson);

                //篩選掉重複的 表單資料夾名稱
                formMainTree = formMainTree
                                .GroupBy(M => M.FOLDER_NAME)
                                .Select(g => g.First())
                                .ToList();

                foreach (var Data in formMainData)
                {
                    var queryMain = formMainTree
                                        .Where(M => M.FOLDER_NAME == Data.FOLDER_NAME)
                                        .Select(M => M)
                                        .ToList();
                    queryMain.ForEach(Main =>
                    {
                        Main.FORM_TREE = formMainData
                                            .Where(D => D.FOLDER_ID == Data.FOLDER_ID)
                                            .Select(D => new FormTree()
                                            {
                                                FORM_NAME = D.FORM_NAME,
                                                IDENTIFY = D.IDENTIFY
                                            }).ToList();
                    });
                }

                return formMainTree;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單列表呈現失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 審核單列表 -
        /// <summary>
        /// 審核單列表
        /// </summary>
        public IList<ApproveFormsConfig> PostApproveForms(ApproveFormQuery query)
        {
            try
            {
                var HaveCold = String.Empty;
                if (query.PARENT)
                {
                    if (!String.IsNullOrEmpty(query.IDENTIFY) || !String.IsNullOrWhiteSpace(query.IDENTIFY))
                    {
                        //是否為子表單
                        strSQL = "";
                        strSQL += "SELECT COL_LENGTH('[BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_M]','GroupID') AS COL_LEN";
                        var HaveColdt = dbFun.DoQuery(strSQL);
                        HaveCold = HaveColdt.Rows[0][0].ToString();
                    }
                }

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [REQUISITION_ID], ";
                strSQL += "     [SERIAL_ID] AS [BPM_FROM_NO], ";
                strSQL += "     [FORM_SUBJECT] AS [FM7_SUBJECT], ";
                strSQL += "     [FORM_STATUS] AS [STATUS], ";
                strSQL += "     [IDENTIFY], ";
                strSQL += "     [APPLICANT_ID], ";
                strSQL += "     [APPLICANT_NAME], ";
                strSQL += "     [APPLICANT_DATETIME] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_View_FormData] ";
                strSQL += "WHERE 1=1 ";
                if (query.PARENT)
                {
                    if (!String.IsNullOrEmpty(HaveCold) || !String.IsNullOrWhiteSpace(HaveCold))
                    {
                        strSQL += "         AND [REQUISITION_ID] in (SELECT RequisitionID FROM [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_M] WHERE [GroupID] IS NOT NULL AND [GroupID]<>'')";
                    }
                    else
                    {
                        strSQL += "         AND [REQUISITION_ID] in (SELECT RequisitionID FROM [BPMPro].[dbo].[FM7T_" + query.IDENTIFY + "_M] WHERE [GroupID] IS NULL OR [GroupID]='')";
                    }
                }

                if (query.RECENT)
                {
                    strSQL += "ORDER BY [APPLICANT_DATETIME] DESC ";
                }
                else
                {
                    strSQL += "ORDER BY [SERIAL_ID],[IDENTIFY] DESC ";
                }

                var Approveforms = dbFun.DoQuery(strSQL).ToList<ApproveFormsConfig>();

                #region - 搜尋:REQUISITION_ID 系統編號 -

                if (query.REQUISITION_ID != null)
                {
                    Approveforms = Approveforms
                                    .Where(F => F.REQUISITION_ID == query.REQUISITION_ID)
                                    .Select(F => F)
                                    .ToList();
                }

                #endregion

                #region - 搜尋:REQUISITION_ID 系統編號 -

                if (query.BPM_FORM_NO != null)
                {
                    Approveforms = Approveforms
                                    .Where(F => F.BPM_FROM_NO.Contains(query.BPM_FORM_NO))
                                    .Select(F => F)
                                    .ToList();
                }

                #endregion

                #region - 篩選:審核狀態 -

                if (query.STATUS != null)
                {
                    Approveforms = Approveforms
                                    .Where(F => F.STATUS == query.STATUS)
                                    .Select(F => F)
                                    .ToList();
                }

                #endregion

                #region - 篩選:表單代號 -

                if (!String.IsNullOrEmpty(query.IDENTIFY) || !String.IsNullOrWhiteSpace(query.IDENTIFY))
                {
                    Approveforms = Approveforms
                                    .Where(F => F.IDENTIFY == query.IDENTIFY)
                                    .Select(F => F)
                                    .ToList();
                }

                #endregion

                #region - 篩選:申請人 -

                if (!String.IsNullOrEmpty(query.APPLICANT) || !String.IsNullOrWhiteSpace(query.APPLICANT))
                {
                    Approveforms = Approveforms
                                    .Where(F => F.APPLICANT_ID.Contains(query.APPLICANT) || F.APPLICANT_NAME.Contains(query.APPLICANT))
                                    .Select(F => F)
                                    .ToList();
                }

                #endregion

                #region - 篩選:天數限制內的單 -

                if (query.DATEDIFF != null)
                {

                    Approveforms = Approveforms
                                    .Where(F => (DateTime.Now - F.APPLICANT_DATETIME).Days <= query.DATEDIFF)
                                    .Select(F => F)
                                    .ToList();
                }

                #endregion

                return Approveforms;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("審核單列表失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - ERP附件 -

        /// <summary>
        /// ERP附件(查詢)
        /// </summary>  
        public IList<AttachmentConfig> PostAttachment(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     [FilePath] AS [FILE_PATH], ";
                strSQL += "     [Identify] AS [IDENTIFY], ";
                strSQL += "     [FileRename] AS [FILE_RENAME], ";
                strSQL += "     [FileName] AS [FILE_NAME], ";
                strSQL += "     [FileExtension] AS [FILE_EXTENSION], ";
                strSQL += "     [FileSize] AS [FILE_SIZE], ";
                strSQL += "     [CreateBy] AS [CREATE_BY], ";
                strSQL += "     [CreateDate] AS [CREATE_DATE], ";
                strSQL += "     [Description] AS [DESCRIPTION] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Attachment] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                strSQL += "ORDER BY [AutoCounter] ";

                return dbFun.DoQuery(strSQL, parameter).ToList<AttachmentConfig>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("附件上傳(查詢)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// ERP附件(新增)
        /// </summary>
        public bool PutAttachment(AttachmentMain model)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@IDENTIFY", SqlDbType.VarChar) { Size = 100, Value = model.IDENTIFY },
                    new SqlParameter("@FILE_RENAME", SqlDbType.VarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FILE_PATH", SqlDbType.VarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FILE_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FILE_EXTENSION", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FILE_SIZE", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CREATE_BY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CREATE_DATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 300, Value = (object)DBNull.Value ?? DBNull.Value },

                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Attachment] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameter);

                #endregion

                #region 再新增資料

                foreach (var item in model.ATTACHMENT)
                {
                    //寫入：附件parameter
                    strJson = jsonFunction.ObjectToJSON(item);
                    GlobalParameters.Infoparameter(strJson, parameter);

                    strSQL = "";
                    strSQL += "INSERT INTO [dbo].[GTV_Attachment]([RequisitionID],[Identify],[FileRename],[FilePath],[FileName],[FileExtension],[FileSize],[CreateBy],[CreateDate],[Description]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@IDENTIFY,@FILE_RENAME,@FILE_PATH,@FILE_NAME,@FILE_EXTENSION,@FILE_SIZE,@CREATE_BY,@CREATE_DATE,@DESCRIPTION)";

                    dbFun.DoTran(strSQL, parameter);

                }

                #endregion

                vResult = true;

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("附件上傳(新增)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        #endregion

        #region - 關聯表單 -

        /// <summary>
        /// 關聯表單(搜詢)(只關聯 "同意結束" 的表單)；
        /// 可查到以【使用者編號】申請的、簽核過的、代他人申請的、被知會的 表單。
        /// </summary>        
        public AssociatedFormViewModel PostAssociatedFormSearch(AssociatedFormQuery query)
        {
            try
            {
                #region - 宣告 -

                //Base64加密內容
                var Base64Code = String.Empty;

                //每頁顯示的筆數
                var PageSize = 5;

                //頁碼:0也就是第一條 
                if (query.PAGE == null || query.PAGE <= 0)
                {
                    query.PAGE = 0;
                }

                #endregion

                #region - 搜尋 -

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 64, Value = query.USER_ID },
                     new SqlParameter("@IDENTIFY", SqlDbType.NVarChar) { Size = 50, Value = query.IDENTIFY }
                };

                strSQL = "";
                strSQL += "EXEC [BPMPro].[dbo].[GTV_Sp_AssociatedFormSearch] @USER_ID,@IDENTIFY ";

                var associatedFormDataViewModel = dbFun.DoQuery(strSQL, parameter).ToList<AssociatedFormDataViewModel>();

                #region - 搜尋:系統編號 -

                if (!String.IsNullOrEmpty(query.REQUISITION_ID) || !String.IsNullOrWhiteSpace(query.REQUISITION_ID))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.ASSOCIATED_REQUISITION_ID.Contains(query.REQUISITION_ID))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #region - 搜尋:表單主旨/(簽呈)收文編號 -

                if (!String.IsNullOrEmpty(query.QUERY) || !String.IsNullOrWhiteSpace(query.QUERY))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.FM7_SUBJECT.Contains(query.QUERY))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #region - 搜尋:表單編號 -

                if (!String.IsNullOrEmpty(query.BPM_FORM_NO) || !String.IsNullOrWhiteSpace(query.BPM_FORM_NO))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.BPM_FORM_NO.Contains(query.BPM_FORM_NO))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #region - 搜尋:申請部門 -

                if (!String.IsNullOrEmpty(query.APPLICANT_DEPT) || !String.IsNullOrWhiteSpace(query.APPLICANT_DEPT))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.APPLICANT_DEPT.Contains(query.APPLICANT_DEPT))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #region - 搜尋:申請人 -


                if (!String.IsNullOrEmpty(query.APPLICANT) || !String.IsNullOrWhiteSpace(query.APPLICANT))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.APPLICANT_ID.Contains(query.APPLICANT) || D.APPLICANT_NAME.Contains(query.APPLICANT))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #region - 搜尋:起始日期 -

                if (!String.IsNullOrEmpty(query.START_DATE) || !String.IsNullOrWhiteSpace(query.START_DATE))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.APPLICANT_DATE_TIME >= DateTime.Parse(query.START_DATE))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #region - 搜尋:結束日期 -

                if (!String.IsNullOrEmpty(query.END_DATE) || !String.IsNullOrWhiteSpace(query.END_DATE))
                {
                    associatedFormDataViewModel = associatedFormDataViewModel
                                                    .Where(D => D.TIME_LAST_ACTION <= DateTime.Parse(query.END_DATE))
                                                    .Select(D => D).ToList();
                }

                #endregion

                #endregion

                #region - 分頁 -

                //分頁
                var associatedFormPage = GlobalParameters.Pagination(int.Parse(query.PAGE.ToString()), PageSize, associatedFormDataViewModel);
                strJson = jsonFunction.ObjectToJSON(associatedFormPage);
                var associatedFormConfigList = jsonFunction.JsonToObject<List<AssociatedFormConfig>>(strJson);

                #endregion

                #region - 總筆數 & 總頁數 -

                //總筆數
                var Total = associatedFormDataViewModel.Count();
                //總頁數
                var TotalPages = Total / PageSize;
                var PageRemainder = Total % PageSize;
                if (PageRemainder != 0)
                {
                    TotalPages = int.Parse(TotalPages.ToString()) + 1;
                }

                #endregion

                #region - 呈現 -

                var formQueryModel = new FormQueryModel();
                associatedFormConfigList.ForEach(ConfigList =>
                {
                    ConfigList.IDENTIFY = query.IDENTIFY;
                    ConfigList.FORM_NAME = PostFormDistinguish(query.IDENTIFY).FORM_NAME;
                    formQueryModel.REQUISITION_ID = ConfigList.ASSOCIATED_REQUISITION_ID;
                    var formData = formRepository.PostFormData(formQueryModel);
                    ConfigList.APPLICANT_DATE_TIME = DateTime.Parse(ConfigList.APPLICANT_DATE_TIME).ToString("yyyy/MM/dd HH:mm:ss");
                    ConfigList.FORM_PATH = GlobalParameters.FormContentPath(formData.REQUISITION_ID, formData.IDENTIFY, formData.DIAGRAM_NAME);
                });

                #endregion

                var associatedFormViewModel = new AssociatedFormViewModel()
                {
                    TOTAL = Total,
                    TOTAL_PAGES = TotalPages,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfigList
                };
                return associatedFormViewModel;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單關聯(搜尋)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 關聯表單(查詢)
        /// </summary>  
        public IList<AssociatedFormConfig> PostAssociatedForm(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [Identify] AS [IDENTIFY], ";
                strSQL += "      '' AS [FORM_NAME], ";
                strSQL += "      [AssociatedRequisitionID] AS [ASSOCIATED_REQUISITION_ID], ";
                strSQL += "      [BPMFormNo] AS [BPM_FORM_NO], ";
                strSQL += "      [FM7Subject] AS [FM7_SUBJECT], ";
                strSQL += "      [ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
                strSQL += "      [ApplicantName] AS [APPLICANT_NAME], ";
                strSQL += "      [ApplicantDateTime] AS [APPLICANT_DATE_TIME], ";
                strSQL += "      [State] AS [STATE], ";
                strSQL += "      [FormPath] AS [FORM_PATH] ";
                strSQL += "FROM [dbo].[GTV_AssociatedForm] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                strSQL += "ORDER BY [AutoCounter] ";

                var associatedForm = dbFun.DoQuery(strSQL, parameter).ToList<AssociatedFormConfig>();
                foreach (var Form in associatedForm)
                {
                    Form.FORM_NAME = PostFormDistinguish(Form.IDENTIFY).FORM_NAME;
                }
                return associatedForm;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("關聯表單(查詢)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 關聯表單(新增)
        /// </summary>
        public bool PutAssociatedForm(AssociatedFormModel model)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@IDENTIFY", SqlDbType.VarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ASSOCIATED_REQUISITION_ID", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BPM_FORM_NO", SqlDbType.VarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLICANT_DATE_TIME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FORM_PATH", SqlDbType.NVarChar) { Size = 1000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@STATE", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_AssociatedForm] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameter);

                #endregion

                if (model.ASSOCIATED_FORM_CONFIG != null && model.ASSOCIATED_FORM_CONFIG.Count > 0)
                {
                    #region 新增資料

                    foreach (var item in model.ASSOCIATED_FORM_CONFIG)
                    {
                        //寫入：表單關聯parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameter);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[GTV_AssociatedForm] ([RequisitionID],[Identify],[AssociatedRequisitionID],[BPMFormNo],[FM7Subject],[ApplicantDeptName],[ApplicantName],[ApplicantDateTime],[State],[FormPath]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@IDENTIFY,@ASSOCIATED_REQUISITION_ID,@BPM_FORM_NO,@FM7_SUBJECT,@APPLICANT_DEPT_NAME,@APPLICANT_NAME,@APPLICANT_DATE_TIME,@STATE,@FORM_PATH) ";

                        dbFun.DoTran(strSQL, parameter);

                    }

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("關聯表單(新增)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        /// <summary>
        /// 關聯表單(知會)
        /// </summary>
        public bool PutAssociatedFormNotify(AssociatedFormNotifyModel model)
        {
            bool vResult = false;
            try
            {
                var query = new FormQueryModel
                {
                    REQUISITION_ID = model.REQUISITION_ID
                };
                var AssociatedForms = PostAssociatedForm(query);
                if (AssociatedForms != null)
                {
                    if (AssociatedForms.Count > 0)
                    {
                        var AssociatedFormsRequisitionID = new List<String>();

                        AssociatedForms.ForEach(AF =>
                        {
                            AssociatedFormsRequisitionID.Add(AF.ASSOCIATED_REQUISITION_ID);
                        });

                        var groupInformNotifyModel = new GroupInformNotifyModel()
                        {
                            REQUISITION_ID = AssociatedFormsRequisitionID,
                            NOTIFY_BY = model.NOTIFY_BY,
                            ROLE_ID = model.ROLE_ID,
                        };

                        vResult = notifyRepository.ByGroupInformNotify(groupInformNotifyModel);
                    }
                    else
                    {
                        CommLib.Logger.Debug(model.REQUISITION_ID + "：此表單無 關聯表單，可(知會)通知(2)。");
                    }
                }
                else
                {
                    CommLib.Logger.Debug(model.REQUISITION_ID + "：此表單無 關聯表單，可(知會)通知(1)。");
                }

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("關聯表單(知會)通知失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - BPM表單機能 -

        /// <summary>
        /// BPM表單機能
        /// </summary>
        public bool PostBPMFormFunction(BPMFormFunction model)
        {
            bool vResult = false;

            try
            {
                var parameter = new List<SqlParameter>()
                {
                    //BPM表單機能
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };
                //寫入：版權採購交片單 表單內容parameter                        
                strJson = jsonFunction.ObjectToJSON(model);
                GlobalParameters.Infoparameter(strJson, parameter);

                #region - 表單送出後將【附件】轉為啟用 -

                strSQL = "";
                strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_F] ";
                strSQL += "SET [DraftFlag]=@DRAFT_FLAG ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameter);

                #endregion

                #region - 表單送出後將【關聯表單】、【外部連結】轉為啟用 -

                strSQL = "";
                strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_U] ";
                strSQL += "SET [DraftFlag]=@DRAFT_FLAG ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameter);

                #endregion

                vResult = true;

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【表單機能】啟用失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 表單共用模組 -

        #region - 申請人資訊(查詢) -

        /// <summary>
        /// 申請人資訊(查詢)
        /// </summary>
        public T PostApplicantInfoFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     [ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     [ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     [ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     [ApplicantPhone] AS [APPLICANT_PHONE], ";
            strSQL += "     [ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     [FillerID] AS [FILLER_ID], ";
            strSQL += "     [FillerName] AS [FILLER_NAME], ";
            strSQL += "     [Priority] AS [PRIORITY], ";
            strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     [FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            return (T)(Object)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<ApplicantInfo>().FirstOrDefault();
        }

        #endregion

        #region - 會簽簽核人員 -

        /// <summary>
        /// 會簽簽核人員_共用模組(查詢)
        /// </summary>
        public List<T> PostApproverFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ApproverCompanyID] AS [APPROVER_COMPANY_ID], ";
            strSQL += "     [ApproverDeptMainID] AS [APPROVER_DEPT_MAIN_ID], ";
            strSQL += "     [ApproverDeptID] AS [APPROVER_DEPT_ID], ";
            strSQL += "     [ApproverID] AS [APPROVER_ID], ";
            strSQL += "     [ApproverName] AS [APPROVER_NAME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";
            switch (Common.IDENTIFY)
            {
                case "GPI_Countersign": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GPI_CountersignApproversConfig>();
                case "OfficialStamp": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<OfficialStampApproversConfig>();
                default: return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<ApproversConfig>();
            }
        }


        /// <summary>
        /// 會簽簽核人員_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutApproverFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                strTable = Common.IDENTIFY + "_" + Common.EXT;

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.PARAMETER);

                #endregion

                if (Common.MODEL != null && Common.MODEL.Count > 0)
                {
                    #region 再新增資料

                    Common.MODEL.ForEach(item =>
                    {
                        //寫入：會簽簽核人員parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.PARAMETER);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "]([RequisitionID],[ApproverCompanyID],[ApproverDeptMainID],[ApproverDeptID],[ApproverID],[ApproverName]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@APPROVER_COMPANY_ID,@APPROVER_DEPT_MAIN_ID,@APPROVER_DEPT_ID,@APPROVER_ID,@APPROVER_NAME) ";

                        dbFun.DoTran(strSQL, Common.PARAMETER);

                    });

                    #endregion

                    vResult = true;
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【會簽簽核人員_共用模組】執行失敗，原因：" + ex.Message);
                throw;
            }

            return vResult;
        }

        #endregion

        #region - 憑證明細 -

        /// <summary>
        /// 憑證明細_共用模組(查詢)
        /// </summary>
        public List<T> PostInvoiceFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            switch (Common.IDENTIFY)
            {
                case "GeneralInvoice":
                case "GeneralOrderReturnRefund":
                case "MediaInvoice":
                case "MediaOrderReturnRefund":
                    strField_V = "[Period] AS [PERIOD], ";
                    strField_V += "[InvoiceRowNo] AS [INV_ROW_NO], ";
                    strField_V += "[Note] AS [NOTE], ";
                    break;
                case "ExpensesReimburse":
                    strField_V = "[RowNo] AS [ROW_NO], ";
                    strField_V += "[Name] AS [NAME], ";
                    strField_V += "[Type] AS [TYPE], ";
                    strField_V += "[InvoiceType] AS [INV_TYPE], ";
                    strField_V += "[ExchangeRate] AS [EXCH_RATE], ";
                    strField_V += "[Amount_CONV] AS [AMOUNT_CONV], ";
                    strField_V += "[Currency] AS [CURRENCY], ";
                    strField_V += "[ACCT_Category] AS [ACCT_CATEGORY], ";
                    strField_V += "[ProjectFormNo] AS [PROJECT_FORM_NO], ";
                    strField_V += "[ProjectName] AS [PROJECT_NAME], ";
                    strField_V += "[ProjectNickname] AS [PROJECT_NICKNAME], ";
                    strField_V += "[ProjectUseYear] AS [PROJECT_USE_YEAR], ";
                    break;
                default:
                    strField_V = "[RowNo] AS [ROW_NO], ";
                    strTable = Common.IDENTIFY;
                    break;
            }

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += strField_V;
            strSQL += "     [Num] AS [NUM], ";
            strSQL += "     [Date] AS [DATE], ";
            strSQL += "     [Excl] AS [EXCL], ";
            strSQL += "     [Excl_TWD] AS [EXCL_TWD], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Net_TWD] AS [NET_TWD], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [Gross_TWD] AS [GROSS_TWD], ";
            strSQL += "     [Amount] AS [AMOUNT], ";
            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            if (Common.IDENTIFY.Contains("General") || Common.IDENTIFY.Contains("Media"))
            {
                strSQL += "         AND [Period]=@PERIOD ";
            }
            strSQL += "ORDER BY [AutoCounter] ";

            switch (Common.IDENTIFY)
            {
                case "GeneralInvoice": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GeneralInvoiceInvoicesConfig>();
                case "GeneralOrderReturnRefund": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GeneralInvoiceInvoicesConfig>();
                case "MediaInvoice": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<MediaInvoiceInvoicesConfig>();
                case "MediaOrderReturnRefund": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<MediaInvoiceInvoicesConfig>();
                case "ExpensesReimburse": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<ExpensesReimburseDetailsConfig>();
                default: return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<InvoiceConfig>();
            }
        }

        /// <summary>
        /// 憑證明細_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutInvoiceFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                strTable = Common.IDENTIFY + "_" + Common.EXT;

                switch (Common.IDENTIFY)
                {
                    case "GeneralInvoice":
                    case "GeneralOrderReturnRefund":
                        strField_F = "[GeneralOrderRequisitionID],[GeneralOrderBPMFormNo],[GeneralOrderERPFormNo],[Period],[InvoiceRowNo],[Note],";
                        strField_V = "@GENERAL_ORDER_REQUISITION_ID,@GENERAL_ORDER_BPM_FORM_NO,@GENERAL_ORDER_ERP_FORM_NO,@PERIOD,@INV_ROW_NO,@NOTE,";
                        break;
                    case "MediaInvoice":
                    case "MediaOrderReturnRefund":
                        strField_F = "[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo],[Period],[InvoiceRowNo],[Note],";
                        strField_V = "@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO,@PERIOD,@INV_ROW_NO,@NOTE,";
                        break;
                    case "ExpensesReimburse":
                        strField_F = "[RowNo],";
                        strField_V = "@ROW_NO,";
                        break;
                    default:
                        strField_F = null;
                        strField_V = null;
                        strTable = Common.IDENTIFY;
                        break;
                }

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.PARAMETER);

                #endregion

                if (Common.MODEL != null && Common.MODEL.Count > 0)
                {
                    #region 再新增資料

                    Common.MODEL.ForEach(item =>
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購請款 憑證明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.PARAMETER);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "]([RequisitionID]," + strField_F + "[Num],[Date],[Excl],[Excl_TWD],[Tax],[Tax_TWD],[Net],[Net_TWD],[Gross],[Gross_TWD],[Amount],[Amount_TWD]) ";
                        strSQL += "VALUES(@REQUISITION_ID," + strField_V + "@NUM,@DATE,@EXCL,@EXCL_TWD,@TAX,@TAX_TWD,@NET,@NET_TWD,@GROSS,@GROSS_TWD,@AMOUNT,@AMOUNT_TWD) ";

                        dbFun.DoTran(strSQL, Common.PARAMETER);

                    });

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【憑證明細_共用模組】執行失敗，原因：" + ex.Message);
                throw;
            }

            return vResult;
        }

        #endregion

        #region - 憑證細項 -

        /// <summary>
        /// 憑證細項_共用模組(查詢)
        /// </summary>
        public List<T> PostInvoiceDetailFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            switch (Common.IDENTIFY)
            {
                case "GeneralInvoice":
                case "GeneralOrderReturnRefund":
                case "MediaInvoice":
                case "MediaOrderReturnRefund":
                    strField_V = "[Period] AS [PERIOD], [InvoiceRowNo] AS [INV_ROW_NO],";

                    break;
                case "ExpensesReimburse":
                    strField_V = null;
                    break;
                default:
                    strField_V = null;
                    strTable = Common.IDENTIFY;
                    break;
            }

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += strField_V;
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [Num] AS [NUM], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [Quantity] AS [QUANTITY], ";
            strSQL += "     [Amount] AS [AMOUNT], ";
            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD], ";
            strSQL += "     [RemainingQuantity] AS [R_QUANTITY], ";
            strSQL += "     [RemainingAmount] AS [R_AMOUNT], ";
            strSQL += "     [RemainingAmount_TWD] AS [R_AMOUNT_TWD], ";
            strSQL += "     [IsExcl] AS [IS_EXCL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            if (Common.IDENTIFY.Contains("General") || Common.IDENTIFY.Contains("Media"))
            {
                strSQL += "         AND [Period]=@PERIOD ";
            }
            strSQL += "ORDER BY [AutoCounter] ";

            switch (Common.IDENTIFY)
            {
                case "GeneralInvoice": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GeneralInvoiceInvoiceDetailsConfig>();
                case "GeneralOrderReturnRefund": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GeneralInvoiceInvoiceDetailsConfig>();
                case "MediaInvoice": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<MediaInvoiceInvoiceDetailsConfig>();
                case "MediaOrderReturnRefund": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<MediaInvoiceInvoiceDetailsConfig>();
                case "ExpensesReimburse": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<InvoiceDetailConfig>();
                default: return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<InvoiceDetailConfig>();
            }
        }

        /// <summary>
        /// 憑證細項_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutInvoiceDetailFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                strTable = Common.IDENTIFY + "_" + Common.EXT;

                switch (Common.IDENTIFY)
                {
                    case "GeneralInvoice":
                    case "GeneralOrderReturnRefund":
                        strField_F = "[GeneralOrderRequisitionID],[GeneralOrderBPMFormNo],[GeneralOrderERPFormNo],[Period],[InvoiceRowNo],";
                        strField_V = "@GENERAL_ORDER_REQUISITION_ID,@GENERAL_ORDER_BPM_FORM_NO,@GENERAL_ORDER_ERP_FORM_NO,@PERIOD,@INV_ROW_NO,";
                        break;
                    case "MediaInvoice":
                    case "MediaOrderReturnRefund":
                        strField_F = "[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo],[Period],[InvoiceRowNo],";
                        strField_V = "@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO,@PERIOD,@INV_ROW_NO,";
                        break;
                    case "ExpensesReimburse":
                        strField_F = null;
                        strField_V = null;
                        break;
                    default:
                        strField_F = null;
                        strField_V = null;
                        strTable = Common.IDENTIFY;
                        break;
                }

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.PARAMETER);

                #endregion

                if (Common.MODEL != null && Common.MODEL.Count > 0)
                {
                    #region 再新增資料

                    Common.MODEL.ForEach(item =>
                    {
                        switch (Common.IDENTIFY)
                        {
                            case "GeneralOrderReturnRefund":
                            case "MediaOrderReturnRefund":
                                Common.PARAMETER.Add(new SqlParameter("@R_QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value });
                                Common.PARAMETER.Add(new SqlParameter("@R_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value });
                                Common.PARAMETER.Add(new SqlParameter("@R_AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value });
                                break;
                            default:
                                break;
                        }

                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：憑證細項parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.PARAMETER);

                        switch (Common.IDENTIFY)
                        {
                            case "GeneralInvoice":
                            case "MediaInvoice":
                            case "ExpensesReimburse":
                                Common.PARAMETER.Add(new SqlParameter("@R_QUANTITY", SqlDbType.Int) { Value = Common.PARAMETER.Where(SP => SP.ParameterName.Contains("@QUANTITY")).FirstOrDefault().Value });
                                Common.PARAMETER.Add(new SqlParameter("@R_AMOUNT", SqlDbType.Int) { Value = Common.PARAMETER.Where(SP => SP.ParameterName.Contains("@AMOUNT")).FirstOrDefault().Value });
                                Common.PARAMETER.Add(new SqlParameter("@R_AMOUNT_TWD", SqlDbType.Int) { Value = Common.PARAMETER.Where(SP => SP.ParameterName.Contains("@AMOUNT_TWD")).FirstOrDefault().Value });
                                break;
                            default:
                                break;
                        }

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "]([RequisitionID]," + strField_F + "[RowNo],[Num],[Name],[Quantity],[Amount],[Amount_TWD],[RemainingQuantity],[RemainingAmount],[RemainingAmount_TWD],[IsExcl]) ";
                        strSQL += "VALUES(@REQUISITION_ID," + strField_V + "@ROW_NO,@NUM,@NAME,@QUANTITY,@AMOUNT,@AMOUNT_TWD,@R_QUANTITY,@R_AMOUNT,@R_AMOUNT_TWD,@IS_EXCL) ";

                        dbFun.DoTran(strSQL, Common.PARAMETER);

                        switch (Common.IDENTIFY)
                        {
                            case "GeneralInvoice":
                            case "GeneralOrderReturnRefund":
                            case "MediaInvoice":
                            case "MediaOrderReturnRefund":
                            case "ExpensesReimburse":
                                Common.PARAMETER.Remove(Common.PARAMETER.Where(SP => SP.ParameterName.Contains("@R_QUANTITY")).FirstOrDefault());
                                Common.PARAMETER.Remove(Common.PARAMETER.Where(SP => SP.ParameterName.Contains("@R_AMOUNT")).FirstOrDefault());
                                Common.PARAMETER.Remove(Common.PARAMETER.Where(SP => SP.ParameterName.Contains("@R_AMOUNT_TWD")).FirstOrDefault());
                                break;
                            default:
                                break;
                        }
                    });

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【憑證細項_共用模組】執行失敗，原因：" + ex.Message);
                throw;
            }

            return vResult;
        }

        #endregion

        #region - 預算 -

        /// <summary>
        /// 預算_共用模組(查詢)
        /// </summary>
        public List<T> PostBudgetFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            switch (Common.IDENTIFY)
            {
                case "GeneralOrder":
                //case "GeneralInvoice":
                //case "GeneralOrderReturnRefund":
                //由GeneralOrder(查詢)後LINQ篩選PERIOD。
                case "MediaOrder":
                    //case "MediaInvoice": 
                    //case "MediaOrderReturnRefund":
                    //由MediaOrder(查詢)後LINQ篩選PERIOD。
                    strField_V = "[Period] AS [PERIOD], ";
                    break;
                case "ExpensesReimburse":
                    strField_V = "[RowNo] AS[ROW_NO], ";
                    break;
                default:
                    strField_V = null;
                    strTable = Common.IDENTIFY;
                    break;
            }

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += strField_V;
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [CreateYear] AS [CREATE_YEAR], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [OwnerDept] AS [OWNER_DEPT], ";
            strSQL += "     [Total] AS [TOTAL], ";
            strSQL += "     [AvailableBudgetAmount] AS [AVAILABLE_BUDGET_AMOUNT], ";
            strSQL += "     [UseBudgetAmount] AS [USE_BUDGET_AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            switch (Common.IDENTIFY)
            {
                case "GeneralOrder": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GeneralOrderBudgetsConfig>();
                case "MediaOrder": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<MediaOrderBudgetsConfig>();
                case "ExpensesReimburse": return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<ExpensesReimburseBudgetsConfig>();
                default: return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<BudgetConfig>();
            }

        }

        /// <summary>
        /// 預算_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutBudgetFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                strTable = Common.IDENTIFY + "_" + Common.EXT;

                switch (Common.IDENTIFY)
                {
                    case "GeneralOrder":
                    case "MediaOrder":
                        strField_F = "[Period], ";
                        strField_V = "@PERIOD, ";
                        break;
                    case "ExpensesReimburse":
                        strField_F = "[RowNo], ";
                        strField_V = "@ROW_NO, ";
                        break;
                    default:
                        strField_F = null;
                        strField_V = null;
                        strTable = Common.IDENTIFY;
                        break;
                }

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.PARAMETER);

                #endregion

                if (Common.MODEL != null && Common.MODEL.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in Common.MODEL)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：預算parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.PARAMETER);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "]([RequisitionID]," + strField_F + "[FormNo],[CreateYear],[Name],[OwnerDept],[Total],[AvailableBudgetAmount],[UseBudgetAmount]) ";
                        strSQL += "VALUES(@REQUISITION_ID," + strField_V + "@FORM_NO,@CREATE_YEAR,@NAME,@OWNER_DEPT,@TOTAL,@AVAILABLE_BUDGET_AMOUNT,@USE_BUDGET_AMOUNT) ";

                        dbFun.DoTran(strSQL, Common.PARAMETER);

                    }

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【憑證明細_共用模組】執行失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        #endregion

        #region - (已退貨/退貨)商品 -

        #region - (已退貨/退貨)行政商品 -

        /// <summary>
        /// (已退貨/退貨)行政商品_共用模組(查詢)
        /// </summary>
        public List<T> PostGeneralCommodityFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [Model] AS [MODEL], ";
            strSQL += "     [Specifications] AS [SPECIFICATIONS], ";
            strSQL += "     [Quantity] AS [QUANTITY], ";
            strSQL += "     [Unit] AS [UNIT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<GeneralCommodityConfig>();

        }

        /// <summary>
        /// (已退貨/退貨)行政商品_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutGeneralCommodityFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                strTable = Common.IDENTIFY + "_" + Common.EXT;

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.PARAMETER);

                #endregion

                if (Common.MODEL != null && Common.MODEL.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in Common.MODEL)
                    {
                        //寫入：商品parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.PARAMETER);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "]([RequisitionID],[OrderRowNo],[SupProdANo],[ItemName],[Model],[Specifications],[Quantity],[Unit]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ORDER_ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@MODEL,@SPECIFICATIONS,@QUANTITY,@UNIT) ";

                        dbFun.DoTran(strSQL, Common.PARAMETER);
                    }

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【版權商品_共用模組】執行失敗，原因：" + ex.Message);
                throw;
            }

            return vResult;
        }

        #endregion

        #region - (已退貨/退貨)版權商品 -

        /// <summary>
        /// (已退貨/退貨)版權商品_共用模組(查詢)
        /// </summary>
        public List<T> PostMediaCommodityFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            strTable = Common.IDENTIFY + "_" + Common.EXT;

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaSpec] AS [MEDIA_SPEC], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [OrderEpisode] AS [ORDER_EPISODE], ";
            strSQL += "     [ACPT_Episode] AS [ACPT_EPISODE], ";
            strSQL += "     [EpisodeTime] AS [EPISODE_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";


            return (List<T>)dbFun.DoQuery(strSQL, Common.PARAMETER).ToList<MediaCommodityConfig>();
        }

        /// <summary>
        /// (已退貨/退貨)版權商品_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutMediaCommodityFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                strTable = Common.IDENTIFY + "_" + Common.EXT;

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.PARAMETER);

                #endregion

                if (Common.MODEL != null && Common.MODEL.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in Common.MODEL)
                    {
                        //寫入：商品parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.PARAMETER);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "]([RequisitionID],[OrderRowNo],[SupProdANo],[ItemName],[MediaSpec],[MediaType],[StartEpisode],[EndEpisode],[OrderEpisode],[ACPT_Episode],[EpisodeTime]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ORDER_ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_SPEC,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@ORDER_EPISODE,@ACPT_EPISODE,@EPISODE_TIME) ";

                        dbFun.DoTran(strSQL, Common.PARAMETER);
                    }

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("【版權商品_共用模組】執行失敗，原因：" + ex.Message);
                throw;
            }

            return vResult;
        }

        #endregion

        #endregion

        #endregion

        #region - 擴充方法 -

        #region - (擴充方法)_角色列表 -

        /// <summary>
        /// (擴充方法)_角色列表
        /// </summary>
        public static IList<RolesModel> GetRoles()
        {
            //擴充方法使用dbFunction需要參考物件
            CommonRepository commonRepository = new CommonRepository();

            try
            {
                var strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     S.[RoleID] AS [ROLE_ID], ";
                strSQL += "     I.[DisplayName] AS [ROLE_NAME], ";
                strSQL += "     S.[AtomID] AS [USER_ID] ";
                strSQL += "FROM [BPMPro].[dbo].[FSe7en_Org_RoleStruct] AS S ";
                strSQL += "INNER JOIN [BPMPro].[dbo].[FSe7en_Org_RoleInfo] AS I on S.RoleID=I.RoleID ";
                var RolesData = commonRepository.dbFun.DoQuery(strSQL).ToList<RolesModel>();

                return RolesData;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("角色列表呈現失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - (擴充方法)_BPM系統申請單總表 -

        /// <summary>
        /// (擴充方法)_BPM系統申請單總表
        /// </summary>
        public static IList<FSe7enSysRequisitionField> PostFSe7enSysRequisition(FormData model)
        {
            //擴充方法使用dbFunction需要參考物件
            CommonRepository commonRepository = new CommonRepository();

            var parameter = new List<SqlParameter>();

            if (!String.IsNullOrEmpty(model.REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.REQUISITION_ID)) parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID });

            try
            {
                var strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     R.[AutoCounter] AS [AUTO_COUNTER], ";
                strSQL += "     R.[ParentRequisition] AS [PARENT_REQUISITION], ";
                strSQL += "     R.[RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     R.[SerialID] AS [BPM_FORM_NO], ";
                strSQL += "     D.[Identify] AS [IDENTIFY], ";
                strSQL += "     R.[DiagramID] AS [DIAGRAM_ID], ";
                strSQL += "     R.[DeptID] AS [APPLICANT_DEPT], ";
                strSQL += "     R.[ApplicantID] AS [APPLICANT_ID], ";
                strSQL += "     R.[Status] AS [STATUS], ";
                strSQL += "     ( ";
                strSQL += "             CASE ";
                strSQL += "                 WHEN R.[Status] = 0 THEN '進行中' ";
                strSQL += "                 WHEN R.[Status] = 1 THEN '同意結束' ";
                strSQL += "                 WHEN R.[Status] = 2 THEN '駁回結束' ";
                strSQL += "                 WHEN R.[Status] = 3 THEN '逾期結束' ";
                strSQL += "                 WHEN R.[Status] = 4 THEN '表單撤回' ";
                strSQL += "                 WHEN R.[Status] = 5 THEN '異常表單' ";
                strSQL += "             END ";
                strSQL += "     ) AS [STATUS_NAME], ";
                strSQL += "     R.[TimeStart] AS [TIME_START], ";
                strSQL += "     R.[TimeLastAction] AS [TIME_LAST_ACTION] ";
                strSQL += "FROM [BPMPro].[dbo].[FSe7en_Sys_Requisition] AS R ";
                strSQL += "     INNER JOIN [BPMPro].[dbo].[FSe7en_Sys_DiagramList] AS D on D.[DiagramID]=R.[DiagramID] ";

                if (!String.IsNullOrEmpty(model.REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.REQUISITION_ID)) strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                strSQL += "ORDER BY R.[AutoCounter] DESC ";

                if (!String.IsNullOrEmpty(model.REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.REQUISITION_ID)) return commonRepository.dbFun.DoQuery(strSQL, parameter).ToList<FSe7enSysRequisitionField>();
                else return commonRepository.dbFun.DoQuery(strSQL).ToList<FSe7enSysRequisitionField>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("BPM系統申請單總表呈現失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - (擴充方法)_確認BPM表單(FormData)是否存在 -

        /// <summary>
        /// (擴充方法)_確認BPM表單是否存在
        /// </summary>
        public static bool PostDataHaveForm(FormQueryModel query)
        {
            //擴充方法使用dbFunction需要參考物件
            CommonRepository commonRepository = new CommonRepository();

            bool vResult = false;

            var parameter = new List<SqlParameter>();

            var strSQL = "";
            strSQL += "SELECT * ";
            strSQL += "FROM [BPMPro].[dbo].[GTV_View_FormData] ";
            strSQL += "WHERE 1=1 ";

            if (!String.IsNullOrEmpty(query.REQUISITION_ID))
            {
                strSQL += "AND [REQUISITION_ID]=@REQUISITION_ID ";
                parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID });
            }
            var dt = commonRepository.dbFun.DoQuery(strSQL, parameter);

            if (dt.Rows.Count > 0)
            {
                vResult = true;
            }
            else
            {
                vResult = false;
            }
            return vResult;
        }

        #endregion

        #region - (擴充方法)_共同表單區分 -

        /// <summary>
        /// (擴充方法)_共同表單區分
        /// </summary>
        public static FormDistinguishResponse PostFormDistinguish(string IDENTIFY)
        {
            //擴充方法使用dbFunction需要參考物件
            CommonRepository commonRepository = new CommonRepository();

            var parameter = new List<SqlParameter>()
            {
                new SqlParameter("@IDENTIFY", SqlDbType.NVarChar) { Size = 64, Value = IDENTIFY }
            };

            var strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [IDENTIFY], ";
            strSQL += "     [FormName] AS [FORM_NAME], ";
            strSQL += "     [EndProcessID] AS [END_PROCESS_ID], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[GTV_FormDistinguish] ";
            strSQL += "WHERE [IDENTIFY]=@IDENTIFY ";
            var response = commonRepository.dbFun.DoQuery(strSQL, parameter).ToList<FormDistinguishResponse>().FirstOrDefault();

            return response;
        }

        #endregion

        #region - (擴充方法)_確認檔案複製路徑 -

        /// <summary>
        /// (擴充方法)_確認檔案複製路徑
        /// </summary>
        public static string PostUploadFilePath(UploadFilePathModel model)
        {
            var ProjectField = String.Empty;

            try
            {
                switch (model.LOCATION)
                {
                    case "BPMProDev":
                        ProjectField = "OA_WEB_API_DEV";
                        break;
                    case "BPMProTest":
                        ProjectField = "OA_WEB_API_TEST";
                        break;
                    case "BPMPro":
                        ProjectField = "OA_WEB_API";
                        break;
                    default:
                        ProjectField = "OA_WEB_API_DEV_HO";
                        break;
                }

                model.PATH = model.PATH + ProjectField + "\\Attach\\" + model.IDENTIFY + "\\";

                if (!Directory.Exists(model.PATH))
                {
                    //確認是否有資料夾沒有的話就自動新增
                    //新增資料夾
                    Directory.CreateDirectory(model.PATH);
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("位置錯誤，原因：" + ex.Message);
                throw new Exception("位置錯誤，原因：" + ex.Message);
            }

            return model.PATH;
        }

        #endregion

        #endregion

        #region - base64圖片上傳 -

        #region - 單筆上傳 >>> 單筆輸出 -

        /// <summary>
        /// base64圖片上傳
        /// </summary>
        public bool PostBase64ImgSingletoSingle(Base64ImgSingletoSingleModel model)
        {
            bool vResult = false;

            try
            {
                string imgPath = null;
                if (model.IMG_SIZE != null) imgPath = Path.Combine(model.FILE_PATH, model.IMG_NAME);
                else imgPath = Path.Combine(model.FILE_PATH, model.PRO_IMG_NAME);
                byte[] imageBytes = Convert.FromBase64String(model.PHOTO);
                File.WriteAllBytes(imgPath, imageBytes);

                #region - 調整圖片大小 -

                //調整大小
                //if (model.IMG_SIZE != null)
                //{
                //    Image image = Image.FromFile(model.FILE_PATH + model.IMG_NAME);
                //    //取得影像的格式
                //    ImageFormat thisFormat = image.RawFormat;

                //    int fixWidth = 0;
                //    int fixHeight = 0;
                //    if (image.Width > image.Height)
                //    {
                //        if (image.Width < model.IMG_SIZE && image.Height < model.IMG_SIZE)
                //        {
                //            //圖片沒有超過設定值，不執行縮圖
                //            fixHeight = int.Parse(model.IMG_SIZE.ToString());
                //            fixWidth = int.Parse(model.IMG_SIZE.ToString());

                //        }
                //        else
                //        {
                //            //設定修改後的圖寬
                //            fixWidth = int.Parse(model.IMG_SIZE.ToString());
                //            //設定修改後的圖高
                //            fixHeight = Convert.ToInt32((Convert.ToDouble(fixWidth) / Convert.ToDouble(image.Width)) * Convert.ToDouble(image.Height));
                //        }
                //    }
                //    else
                //    {
                //        if (image.Width < int.Parse(model.IMG_SIZE.ToString()) && image.Height < int.Parse(model.IMG_SIZE.ToString()))
                //        {
                //            //圖片沒有超過設定值，不執行縮圖
                //            fixHeight = int.Parse(model.IMG_SIZE.ToString());
                //            fixWidth = int.Parse(model.IMG_SIZE.ToString());
                //        }
                //        else
                //        {
                //            //設定修改後的圖高
                //            fixHeight = int.Parse(model.IMG_SIZE.ToString());
                //            //設定修改後的圖寬
                //            fixWidth = Convert.ToInt32((Convert.ToDouble(fixHeight) / Convert.ToDouble(image.Height)) * Convert.ToDouble(image.Width));
                //        }
                //    }

                //    //輸出一個新圖(就是修改過的圖)
                //    Bitmap imageOutput = new Bitmap(image, fixWidth, fixHeight);
                //    //將修改過的圖存於設定的位子
                //    imageOutput.Save(string.Concat(model.FILE_PATH + model.PRO_IMG_NAME), thisFormat);
                //    //docfiles.Add(filePath + Nfile);
                //    //釋放記憶體
                //    imageOutput.Dispose();
                //    //釋放掉圖檔 
                //    image.Dispose();

                //    ///刪除原始檔案
                //    File.Delete(model.FILE_PATH + model.IMG_NAME);
                //}

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("圖片上傳失敗，原因：" + ex.Message);
                throw new Exception("圖片上傳失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        #endregion

        /// <summary>
        /// base64圖片輸出設定
        /// </summary>
        public string PostBase64ImgOut(Base64ImgModel model)
        {
            try
            {
                string[] ExtStrArray = model.FILE_EXTENSION.Split('/');
                byte[] imageArray = File.ReadAllBytes(model.FILE_PATH + "." + ExtStrArray[1]);
                return Convert.ToBase64String(imageArray);
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("圖片輸出失敗，原因：" + ex.Message);
                throw new Exception("圖片輸出失敗，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 整理圖片檔案及子表單資料
        /// </summary>
        public bool PostOrganizeImg(OrganizeImgModel model)
        {
            bool vResult = false;

            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [FileRename], ";
                strSQL += "     [FileExtension] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_" + model.EXT + "] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";
                var dtImg = dbFun.DoQuery(strSQL, parameter);
                if (dtImg.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtImg.Rows)
                    {
                        string[] ExtStrArray = dr["FileExtension"].ToString().Split('/');
                        File.Delete(model.FILE_PATH + dr["FileRename"].ToString() + "." + ExtStrArray[1]);
                    }

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_D] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameter);

                    vResult = true;
                }
                else vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("圖片刪除失敗，原因：" + ex.Message);
                throw new Exception("圖片刪除失敗，原因：" + ex.Message);
            }
            return vResult;
        }

        #endregion

        #region - 檔案、資料整理 -

        /// <summary>
        /// 檔案、資料整理
        /// </summary>        
        public bool PostInformationOrganize(OrganizeImgModel model)
        {
            bool vResult = false;
            try
            {
                #region - 確認檔案路徑 -

                var uploadFilePathModel = new UploadFilePathModel()
                {
                    LOCATION = GlobalParameters.sqlConnBPMProDevHo,
                    PATH = model.FILE_PATH,
                    IDENTIFY = model.IDENTIFY
                };

                var ImgPath = CommonRepository.PostUploadFilePath(uploadFilePathModel);

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_" + model.EXT + "] ";
                strSQL += "GROUP BY [RequisitionID] ";
                strSQL += "EXCEPT ";
                strSQL += "SELECT ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_M] ";
                strSQL += "GROUP BY [RequisitionID] ";
                var dtExcept = dbFun.DoQuery(strSQL);
                if (dtExcept.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtExcept.Rows)
                    {
                        #region 整理檔案及資料

                        model = new OrganizeImgModel()
                        {
                            EXT = model.EXT,
                            FILE_PATH = ImgPath,
                            IDENTIFY = model.IDENTIFY,
                            REQUISITION_ID = dr["REQUISITION_ID"].ToString()
                        };

                        vResult = PostOrganizeImg(model);

                        #endregion
                    }
                }
                else vResult = true;

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("檔案、資料調整失敗，原因：" + ex.Message);
                throw new Exception("檔案、資料調整失敗，原因：" + ex.Message);
            }
            return vResult;
        }

        #endregion

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        /// <summary>
        /// T-SQL Table
        /// </summary>
        private string strTable;

        /// <summary>
        /// T-SQL 欄位
        /// </summary>
        private string strField_F;
        /// <summary>
        /// T-SQL 欄位值
        /// </summary>
        private string strField_V;

        /// <summary>
        /// Json字串
        /// </summary>
        private string strJson;

        /// <summary>
        /// 系統編號
        /// </summary>
        private string strREQ;

        #endregion
    }
}