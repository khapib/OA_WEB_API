using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 用印申請單
    /// </summary>
    public class OfficialStampRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();
        SysCommonRepository sysCommonRepository = new SysCommonRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 用印申請單(查詢)
        /// </summary>
        public OfficialStampViewModel PostOfficialStampSingle(OfficialStampQueryModel query)
        {
            var parameter = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            #region - 申請人資訊 -

            var CommonApplicantInfo = new BPMCommonModel<ApplicantInfo>()
            {
                EXT = "M",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter,
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApplicantInfoFunction(CommonApplicantInfo));
            var applicantInfo = jsonFunction.JsonToObject<ApplicantInfo>(strJson);

            #endregion

            #region - 用印申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [CompanyName] AS [COMPANY_NAME], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [LevelType] AS [LEVEL_TYPE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var officialStampTitle = dbFun.DoQuery(strSQL, parameter).ToList<OfficialStampTitle>().FirstOrDefault();

            #endregion

            #region - 用印申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [Contact] AS [CONTACT], ";
            strSQL += "     [ApprovalNo] AS [APPROVAL_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var officialStampConfig = dbFun.DoQuery(strSQL, parameter).ToList<OfficialStampConfig>().FirstOrDefault();

            #endregion

            #region - 用印申請單 用印項目明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [Servings] AS [SERVINGS], ";
            strSQL += "     [ApplyStampType] AS [APPLY_STAMP_TYPE], ";
            strSQL += "     [ApplyStampTypeOthers] AS [APPLY_STAMP_TYPE_OTHERS] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DOC] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var officialStampDocumentsConfig = dbFun.DoQuery(strSQL, parameter).ToList<OfficialStampDocumentsConfig>();

            #endregion

            #region - 用印申請單 會簽簽核人員 -

            var CommonApprovers = new BPMCommonModel<OfficialStampApproversConfig>()
            {
                EXT = "D",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApproverFunction(CommonApprovers));
            var officialStampApproversConfig = jsonFunction.JsonToObject<List<OfficialStampApproversConfig>>(strJson);

            #endregion

            var officialStampViewModel = new OfficialStampViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                OFFICIAL_STAMP_TITLE = officialStampTitle,
                OFFICIAL_STAMP_CONFIG = officialStampConfig,
                OFFICIAL_STAMP_DOCS_CONFIG = officialStampDocumentsConfig,
                OFFICIAL_STAMP_APPROVERS_CONFIG = officialStampApproversConfig,
            };

            #region - 確認表單 -

            if (officialStampViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                if (!CommonRepository.GetFSe7enSysRequisition().Any(R => R.REQUISITION_ID == query.REQUISITION_ID))
                {
                    officialStampViewModel = new OfficialStampViewModel();
                    CommLib.Logger.Error("用印申請單(查詢)失敗，原因：系統無正常起單。");
                }
                else
                {
                    #region - 確認M表BPM表單單號 -

                    //避免儲存後送出表單BPM表單單號沒寫入的情形
                    var formQuery = new FormQueryModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID
                    };
                    notifyRepository.ByInsertBPMFormNo(formQuery);

                    if (String.IsNullOrEmpty(officialStampViewModel.OFFICIAL_STAMP_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(officialStampViewModel.OFFICIAL_STAMP_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) officialStampViewModel.OFFICIAL_STAMP_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return officialStampViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 用印申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>
        //public bool PutOfficialStampRefill(OfficialStampQueryModel query)
        //{
        //    bool vResult = false;

        //    try
        //    {
        //        #region - 宣告 -

        //        var original = PostOfficialStampSingle(query);
        //        strJson = jsonFunction.ObjectToJSON(original);

        //        var officialStampViewModel = new OfficialStampViewModel();

        //        var requisitionID = Guid.NewGuid().ToString();

        //        #endregion

        //        #region - 重送內容 -

        //        officialStampViewModel = jsonFunction.JsonToObject<OfficialStampViewModel>(strJson);

        //        #region - 申請人資訊 調整 -

        //        officialStampViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
        //        officialStampViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
        //        officialStampViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

        //        #endregion

        //        #endregion

        //        #region - 送出 執行(新增/修改/草稿) -

        //        PutOfficialStampSingle(officialStampViewModel);

        //        #endregion

        //        vResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("拷貝申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 用印申請單(新增/修改/草稿)
        /// </summary>
        public bool PutOfficialStampSingle(OfficialStampViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                //表單重要性
                var PRIORITY = model.APPLICANT_INFO.PRIORITY;

                switch (model.OFFICIAL_STAMP_TITLE.LEVEL_TYPE)
                {
                    case "特急件":
                        PRIORITY = 3;
                        break;
                    case "急件":
                        PRIORITY = 2;
                        break;
                    default:
                        //普通件
                        PRIORITY = 1;
                        break;
                }

                #region - 主旨 -

                if (model.OFFICIAL_STAMP_DOCS_CONFIG != null && model.OFFICIAL_STAMP_DOCS_CONFIG.Count > 0)
                {
                    if (String.IsNullOrEmpty(model.OFFICIAL_STAMP_CONFIG.CONTACT) || String.IsNullOrWhiteSpace(model.OFFICIAL_STAMP_CONFIG.CONTACT)) FM7Subject = "用印申請單_" + model.OFFICIAL_STAMP_DOCS_CONFIG.Select(D => D.ITEM_NAME).FirstOrDefault() + "_" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                    else FM7Subject = "用印申請單_" + model.OFFICIAL_STAMP_DOCS_CONFIG.Select(D => D.ITEM_NAME).FirstOrDefault() + "_" + model.OFFICIAL_STAMP_CONFIG.CONTACT + "_" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                }
                else
                {
                    if (String.IsNullOrEmpty(model.OFFICIAL_STAMP_CONFIG.CONTACT) || String.IsNullOrWhiteSpace(model.OFFICIAL_STAMP_CONFIG.CONTACT)) FM7Subject = "用印申請單_" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                    else FM7Subject = "用印申請單_" + model.OFFICIAL_STAMP_CONFIG.CONTACT + "_" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                }


                #endregion

                #endregion

                #region - 用印申請單 表頭資訊：OfficialStamp_M -

                var parameterTitle = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  strREQ},
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value =  PRIORITY},
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value =  model.APPLICANT_INFO.DRAFT_FLAG},
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value =  model.APPLICANT_INFO.FLOW_ACTIVATED},
                    //(申請人/起案人)資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //用印申請單 表頭
                    new SqlParameter("@COMPANY_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.OFFICIAL_STAMP_TITLE.COMPANY_NAME ?? String.Empty },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@LEVEL_TYPE", SqlDbType.NVarChar) { Size = 10, Value = (object)model.OFFICIAL_STAMP_TITLE.LEVEL_TYPE ?? DBNull.Value },
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    if (CommonRepository.GetFSe7enSysRequisition().Where(R => R.REQUISITION_ID == strREQ).Count() <= 0)
                    {
                        parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });
                        IsADD = true;
                    }
                }
                else parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";

                    if (IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

                    strSQL += "     [FillerID]=@FILLER_ID, ";
                    strSQL += "     [FillerName]=@FILLER_NAME, ";
                    strSQL += "     [Priority]=@PRIORITY, ";
                    strSQL += "     [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "     [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "     [CompanyName]=@COMPANY_NAME, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "     [LevelType]=@LEVEL_TYPE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[CompanyName],[FM7Subject],[LevelType]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@COMPANY_NAME,@FM7_SUBJECT,@LEVEL_TYPE) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 用印申請單 表單內容：OfficialStamp_M -

                if (model.OFFICIAL_STAMP_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //用印申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACT", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVAL_NO", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：用印申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.OFFICIAL_STAMP_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [Reason] = @REASON, ";
                    strSQL += "     [Note] = @NOTE, ";
                    strSQL += "     [Contact] = @CONTACT, ";
                    strSQL += "     [ApprovalNo] = @APPROVAL_NO ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 用印申請單 用印項目明細：OfficialStamp_DOC -

                var parameterDocuments = new List<SqlParameter>()
                {
                    //用印申請單 用印項目明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SERVINGS", SqlDbType.Int) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLY_STAMP_TYPE", SqlDbType.Int) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLY_STAMP_TYPE_OTHERS", SqlDbType.Int) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DOC] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDocuments);

                #endregion

                if (model.OFFICIAL_STAMP_DOCS_CONFIG != null && model.OFFICIAL_STAMP_DOCS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.OFFICIAL_STAMP_DOCS_CONFIG)
                    {
                        //寫入：用印申請單 用印項目明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDocuments);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DOC]([RequisitionID],[ItemName],[Servings],[ApplyStampType],[ApplyStampTypeOthers]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ITEM_NAME,@SERVINGS,@APPLY_STAMP_TYPE,@APPLY_STAMP_TYPE_OTHERS) ";

                        dbFun.DoTran(strSQL, parameterDocuments);
                    }

                    #endregion
                }

                #endregion

                #region - 用印申請單 會簽簽核人員：OfficialStamp_D -

                var parameterApprovers = new List<SqlParameter>()
                {
                    //用印申請單 會簽簽核人員
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@APPROVER_COMPANY_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_MAIN_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                if (model.OFFICIAL_STAMP_APPROVERS_CONFIG != null && model.OFFICIAL_STAMP_APPROVERS_CONFIG.Count > 0)
                {
                    model.OFFICIAL_STAMP_APPROVERS_CONFIG.ForEach(A =>
                    {
                        var logonModel = new LogonModel()
                        {
                            USER_ID = A.APPROVER_ID
                        };
                        var UserModel = userRepository.PostUserSingle(logonModel).USER_MODEL;

                        A.APPROVER_COMPANY_ID = UserModel.Where(U => U.DEPT_ID == A.APPROVER_DEPT_ID).Select(U => U.COMPANY_ID).FirstOrDefault();
                        A.APPROVER_NAME = UserModel.Where(U => U.USER_ID == A.APPROVER_ID).Select(U => U.USER_NAME).FirstOrDefault();

                        var userInfoMainDeptModel = new UserInfoMainDeptModel()
                        {
                            USER_ID = A.APPROVER_ID,
                            DEPT_ID = A.APPROVER_DEPT_ID,
                            COMPANY_ID = A.APPROVER_COMPANY_ID,
                        };
                        A.APPROVER_DEPT_MAIN_ID = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).MAIN_DEPT.DEPT_ID;
                    });

                    var CommonApprovers = new BPMCommonModel<OfficialStampApproversConfig>()
                    {
                        EXT = "D",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterApprovers,
                        MODEL = model.OFFICIAL_STAMP_APPROVERS_CONFIG
                    };
                    commonRepository.PutApproverFunction(CommonApprovers);
                }

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = strREQ;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = strREQ;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region - 送出表單：FormAutoStart -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = strREQ;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = strREQ;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = strREQ,
                    IDENTIFY = IDENTIFY,
                    DRAFT_FLAG = 0
                };
                commonRepository.PostBPMFormFunction(BPM_FormFunction);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("用印申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        /// <summary>
        /// 確認是否為新建的表單
        /// </summary>
        private bool IsADD = false;

        /// <summary>
        /// 表單代號
        /// </summary>
        private string IDENTIFY = "OfficialStamp";

        /// <summary>
        /// 表單主旨
        /// </summary>
        private string FM7Subject;

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