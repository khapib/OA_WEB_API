using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 四方四隅_會簽單
    /// </summary>
    public class GPI_CountersignRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

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
        /// 四方四隅_會簽單(查詢)
        /// </summary>
        public GPI_CountersignViewModel PostGPI_CountersignSingle(GPI_CountersignQueryModel query)
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

            #region - 四方四隅_會簽單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [LevelType] AS [LEVEL_TYPE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GPI_Countersign_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var GPI_countersignTitle = dbFun.DoQuery(strSQL, parameter).ToList<GPI_CountersignTitle>().FirstOrDefault();

            #endregion

            #region - 四方四隅_會簽單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [IsVicePresident] AS [IS_VICE_PRESIDENT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GPI_Countersign_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var GPI_countersignConfig = dbFun.DoQuery(strSQL, parameter).ToList<GPI_CountersignConfig>().FirstOrDefault();

            #endregion

            #region - 四方四隅_會簽單 會簽簽核人員 -

            var CommonApprovers = new BPMCommonModel<GPI_CountersignApproversConfig>()
            {
                EXT = "D",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApproverFunction(CommonApprovers));
            var GPI_countersignApproversConfig = jsonFunction.JsonToObject<List<GPI_CountersignApproversConfig>>(strJson);

            #endregion

            #region - 四方四隅_會簽單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var GPI_countersignViewModel = new GPI_CountersignViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                GPI_COUNTERSIGN_TITLE = GPI_countersignTitle,
                GPI_COUNTERSIGN_CONFIG = GPI_countersignConfig,
                GPI_COUNTERSIGN_APPROVERS_CONFIG = GPI_countersignApproversConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };            

            #region - 確認表單 -

            if (GPI_countersignViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                if (!CommonRepository.GetFSe7enSysRequisition().Any(R => R.REQUISITION_ID == query.REQUISITION_ID))
                {
                    GPI_countersignViewModel = new GPI_CountersignViewModel();
                    CommLib.Logger.Error("四方四隅_會簽單(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(GPI_countersignViewModel.GPI_COUNTERSIGN_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(GPI_countersignViewModel.GPI_COUNTERSIGN_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) GPI_countersignViewModel.GPI_COUNTERSIGN_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return GPI_countersignViewModel;
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 四方四隅_會簽單(依此單內容重送)(僅外部起單使用)
        /// </summary>        
        public bool PutGPI_CountersignRefill(GPI_CountersignQueryModel query)
        {
            bool vResult = false;

            try
            {
                if (!String.IsNullOrEmpty(query.REQUISITION_ID) || !String.IsNullOrWhiteSpace(query.REQUISITION_ID))
                {
                    #region - 宣告 -

                    var original = PostGPI_CountersignSingle(query);
                    strJson = jsonFunction.ObjectToJSON(original);

                    var GPI_countersignViewModel = new GPI_CountersignViewModel();

                    var requisitionID = Guid.NewGuid().ToString();

                    #endregion

                    #region - 重送內容 -

                    GPI_countersignViewModel = jsonFunction.JsonToObject<GPI_CountersignViewModel>(strJson);

                    #region - 申請人資訊 調整 -

                    GPI_countersignViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
                    GPI_countersignViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
                    GPI_countersignViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

                    #endregion

                    #region - 四方四隅_會簽單 表頭資訊 調整 -

                    GPI_countersignViewModel.GPI_COUNTERSIGN_TITLE.FM7_SUBJECT = "(依此單內容重上)" + GPI_countersignViewModel.GPI_COUNTERSIGN_TITLE.FM7_SUBJECT;

                    #endregion

                    #endregion

                    #region - 送出 執行(新增/修改/草稿) -

                    PutGPI_CountersignSingle(GPI_countersignViewModel);

                    #endregion
                }

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("四方四隅_會簽單(依此單內容重送)失敗，原因：" + ex.Message);
            }

            return vResult;
        }


        #endregion

        /// <summary>
        /// 四方四隅_會簽單(新增/修改/草稿)
        /// </summary>
        public bool PutGPI_CountersignSingle(GPI_CountersignViewModel model)
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

                switch (model.GPI_COUNTERSIGN_TITLE.LEVEL_TYPE)
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

                FM7Subject = model.GPI_COUNTERSIGN_TITLE.FM7_SUBJECT;

                #endregion

                #endregion

                #region - 四方四隅_會簽單 表頭資訊：GPI_Countersign_M -

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
                    //四方四隅_會簽單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@LEVEL_TYPE", SqlDbType.NVarChar) { Size = 10, Value = (object)model.GPI_COUNTERSIGN_TITLE.LEVEL_TYPE ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GPI_Countersign_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GPI_Countersign_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GPI_Countersign_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[LevelType]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@LEVEL_TYPE) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 四方四隅_會簽單 表單內容：GPI_Countersign_M -

                if (model.GPI_COUNTERSIGN_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //四方四隅_會簽單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_VICE_PRESIDENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：四方四隅_會簽單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.GPI_COUNTERSIGN_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GPI_Countersign_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [IsVicePresident]=@IS_VICE_PRESIDENT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 四方四隅_會簽單 會簽簽核人員：GPI_Countersign_D -

                var parameterApprovers = new List<SqlParameter>()
                {
                    //四方四隅_會簽單 會簽簽核人員
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@APPROVER_COMPANY_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_MAIN_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                if (model.GPI_COUNTERSIGN_APPROVERS_CONFIG != null && model.GPI_COUNTERSIGN_APPROVERS_CONFIG.Count > 0)
                {
                    model.GPI_COUNTERSIGN_APPROVERS_CONFIG.ForEach(A =>
                    {
                        var logonModel = new LogonModel()
                        {
                            USER_ID = A.APPROVER_ID
                        };
                        var userModel = userRepository.PostUserSingle(logonModel).USER_MODEL;
                                                
                        if (userModel != null && userModel.Count > 0 && userModel.Any(U => U.JOB_STATUS != 0))
                        {
                            var userInfoMainDeptModel = new UserInfoMainDeptModel()
                            {
                                USER_ID = A.APPROVER_ID,
                                DEPT_ID = A.APPROVER_DEPT_ID,
                                COMPANY_ID = userModel.Where(U => U.DEPT_ID == A.APPROVER_DEPT_ID).Select(U => U.COMPANY_ID).FirstOrDefault(),
                            };
                            A.APPROVER_DEPT_MAIN_ID = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).MAIN_DEPT.DEPT_ID;
                        }                        
                    });

                    var CommonApprovers = new BPMCommonModel<GPI_CountersignApproversConfig>()
                    {
                        EXT = "D",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterApprovers,
                        MODEL = model.GPI_COUNTERSIGN_APPROVERS_CONFIG
                    };
                    commonRepository.PutApproverFunction(CommonApprovers);
                }

                #endregion

                #region - 四方四隅_會簽單 表單關聯：AssociatedForm -

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = strREQ,
                    ASSOCIATED_FORM_CONFIG = model.ASSOCIATED_FORM_CONFIG
                };

                commonRepository.PutAssociatedForm(associatedFormModel);

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
                CommLib.Logger.Error("四方四隅_會簽單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "GPI_Countersign";

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