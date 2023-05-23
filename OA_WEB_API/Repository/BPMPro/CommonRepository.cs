﻿using System;
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


using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.ERP;
using OA_WEB_API.Models;

using Dapper;
using Microsoft.Ajax.Utilities;
using System.Reflection;
using System.Web.Http.Results;
using System.Runtime.InteropServices;
using Docker.DotNet.Models;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 系統共通功能邏輯
    /// </summary>
    public class CommonRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

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
                    var formDistinguish = FormDistinguish(query.IDENTIFY);

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
                            PostFormDelete(formRemoveQueryModel);
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

        #region - 附件上傳 -

        /// <summary>
        /// 附件上傳(查詢)
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
        /// 附件上傳(新增)
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
                    strSQL += "INSERT INTO [dbo].[GTV_Attachment]([RequisitionID],[Identify],[FilePath],[FileName],[FileExtension],[FileSize],[CreateBy],[CreateDate],[Description]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@IDENTIFY,@FILE_PATH,@FILE_NAME,@FILE_EXTENSION,@FILE_SIZE,@CREATE_BY,@CREATE_DATE,@DESCRIPTION)";

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
                    ConfigList.FORM_NAME = FormDistinguish(query.IDENTIFY).FORM_NAME;
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
                    Form.FORM_NAME = FormDistinguish(Form.IDENTIFY).FORM_NAME;
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



        #region - 角色列表 -

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
        public static FormDistinguishResponse FormDistinguish(string IDENTIFY)
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

        #region - BPM表單共用模組 -

        #region - 憑證明細 -

        /// <summary>
        /// 憑證明細_共用模組(查詢)
        /// </summary>
        public List<T> PostInvoiceFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            if (Common.IsALDY) strTable = Common.IDENTIFY + "_ALDY";
            else strTable = Common.IDENTIFY;

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [InvoiceRowNo] AS [INV_ROW_NO], ";
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
            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [IsExcl] AS [IS_EXCL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_"+ strTable + "_INV] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND [Period]=@PERIOD ";
            strSQL += "ORDER BY [AutoCounter] ";


            return (List<T>)dbFun.DoQuery(strSQL, Common.parameter).ToList<InvoiceConfig>();
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

                if (Common.IsALDY) strTable = Common.IDENTIFY + "_ALDY";
                else strTable = Common.IDENTIFY;

                if (Common.IDENTIFY.Contains("General"))
                {
                    strField_F = "[GeneralOrderRequisitionID],[GeneralOrderBPMFormNo],[GeneralOrderERPFormNo]";
                    strField_V = "@GENERAL_ORDER_REQUISITION_ID,@GENERAL_ORDER_BPM_FORM_NO,@GENERAL_ORDER_ERP_FORM_NO";
                }
                else
                {
                    strField_F = "[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo]";
                    strField_V = "@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO";
                }

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "_INV] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.parameter);

                #endregion

                if (Common.Model != null && Common.Model.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in Common.Model)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購請款 發票明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.parameter);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "_INV]([RequisitionID]," + strField_F + ",[Period],[InvoiceRowNo],[Num],[Date],[Excl],[Excl_TWD],[Tax],[Tax_TWD],[Net],[Net_TWD],[Gross],[Gross_TWD],[Amount],[Amount_TWD],[Note],[IsExcl]) ";
                        strSQL += "VALUES(@REQUISITION_ID," + strField_V + ",@PERIOD,@INV_ROW_NO,@NUM,@DATE,@EXCL,@EXCL_TWD,@TAX,@TAX_TWD,@NET,@NET_TWD,@GROSS,@GROSS_TWD,@AMOUNT,@AMOUNT_TWD,@NOTE,@IS_EXCL) ";

                        dbFun.DoTran(strSQL, Common.parameter);
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

        #region - 憑證細項 -

        /// <summary>
        /// 憑證細項_共用模組(查詢)
        /// </summary>
        public List<T> PostInvoiceDetailFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            if (Common.IsALDY) strTable = Common.IDENTIFY + "_ALDY";
            else strTable = Common.IDENTIFY;

            #endregion

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [InvoiceRowNo] AS [INV_ROW_NO], ";
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "_INV_DTL] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND [Period]=@PERIOD ";
            strSQL += "ORDER BY [AutoCounter] ";

            return (List<T>)dbFun.DoQuery(strSQL, Common.parameter).ToList<InvoiceDetailConfig>();
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

                if (Common.IsALDY) strTable = Common.IDENTIFY + "_ALDY";
                else strTable = Common.IDENTIFY;

                if (Common.IDENTIFY.Contains("General"))
                {
                    strField_F = "[GeneralOrderRequisitionID],[GeneralOrderBPMFormNo],[GeneralOrderERPFormNo]";
                    strField_V = "@GENERAL_ORDER_REQUISITION_ID,@GENERAL_ORDER_BPM_FORM_NO,@GENERAL_ORDER_ERP_FORM_NO";
                }
                else
                {
                    strField_F = "[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo]";
                    strField_V = "@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO";
                }

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "_INV_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.parameter);

                #endregion

                if (Common.Model != null && Common.Model.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in Common.Model)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：憑證細項parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.parameter);

                        if (Common.IDENTIFY == "GeneralInvoice" || Common.IDENTIFY == "MediaInvoice")
                        {
                            Common.parameter.Add(new SqlParameter("@R_QUANTITY", SqlDbType.Int) { Value = Common.parameter.Where(SP => SP.ParameterName.Contains("@QUANTITY")).FirstOrDefault().Value });
                            Common.parameter.Add(new SqlParameter("@R_AMOUNT", SqlDbType.Int) { Value = Common.parameter.Where(SP => SP.ParameterName.Contains("@AMOUNT")).FirstOrDefault().Value });
                            Common.parameter.Add(new SqlParameter("@R_AMOUNT_TWD", SqlDbType.Int) { Value = Common.parameter.Where(SP => SP.ParameterName.Contains("@AMOUNT_TWD")).FirstOrDefault().Value });
                        }

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "_INV_DTL]([RequisitionID]," + strField_F + ",[Period],[InvoiceRowNo],[RowNo],[Num],[Name],[Quantity],[Amount],[Amount_TWD],[RemainingQuantity],[RemainingAmount],[RemainingAmount_TWD],[IsExcl]) ";
                        strSQL += "VALUES(@REQUISITION_ID," + strField_V + ",@PERIOD,@INV_ROW_NO,@ROW_NO,@NUM,@NAME,@QUANTITY,@AMOUNT,@AMOUNT_TWD,@R_QUANTITY,@R_AMOUNT,@R_AMOUNT_TWD,@IS_EXCL) ";

                        dbFun.DoTran(strSQL, Common.parameter);

                        if (Common.IDENTIFY == "GeneralInvoice" || Common.IDENTIFY == "MediaInvoice")
                        {
                            Common.parameter.Remove(Common.parameter.Where(SP => SP.ParameterName.Contains("@R_QUANTITY")).FirstOrDefault());
                            Common.parameter.Remove(Common.parameter.Where(SP => SP.ParameterName.Contains("@R_AMOUNT")).FirstOrDefault());
                            Common.parameter.Remove(Common.parameter.Where(SP => SP.ParameterName.Contains("@R_AMOUNT_TWD")).FirstOrDefault());
                        }

                    }

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

        #region - 版權商品 -

        /// <summary>
        /// 版權商品_共用模組(查詢)
        /// </summary>
        public List<T> PostMediaCommodityFunction<T>(BPMCommonModel<T> Common)
        {
            #region - 宣告 -

            if (Common.IsALDY) strTable = Common.IDENTIFY + "_ALDY";
            else strTable = Common.IDENTIFY;

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "_RF_COMM] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            return (List<T>)dbFun.DoQuery(strSQL, Common.parameter).ToList<MediaCommodityConfig>();
        }

        /// <summary>
        /// 版權商品_共用模組(新增/修改/草稿)
        /// </summary>
        public bool PutMediaCommodityFunction<T>(BPMCommonModel<T> Common)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                if (Common.IsALDY) strTable = Common.IDENTIFY + "_ALDY";
                else strTable = Common.IDENTIFY;

                #endregion

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + strTable + "_RF_COMM] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, Common.parameter);

                #endregion

                if (Common.Model != null && Common.Model.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in Common.Model)
                    {
                        //寫入：商品parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, Common.parameter);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + strTable + "_RF_COMM]([RequisitionID],[OrderRowNo],[SupProdANo],[ItemName],[MediaSpec],[MediaType],[StartEpisode],[EndEpisode],[OrderEpisode],[ACPT_Episode],[EpisodeTime]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ORDER_ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_SPEC,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@ORDER_EPISODE,@ACPT_EPISODE,@EPISODE_TIME) ";

                        dbFun.DoTran(strSQL, Common.parameter);
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