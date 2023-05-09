using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GPI_Countersign_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - M表寫入BPM表單單號 -

            //避免儲存後送出表單BPM表單單號沒寫入的情形
            var formQuery = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            if (applicantInfo.DRAFT_FLAG == 0) notifyRepository.ByInsertBPMFormNo(formQuery);

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

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ApproverCompanyID] AS [APPROVER_COMPANY_ID], ";
            strSQL += "     [ApproverDeptMainID] AS [APPROVER_DEPT_MAIN_ID], ";
            strSQL += "     [ApproverDeptID] AS [APPROVER_DEPT_ID], ";
            strSQL += "     [ApproverID] AS [APPROVER_ID], ";
            strSQL += "     [ApproverName] AS [APPROVER_NAME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GPI_Countersign_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var GPI_countersignApproversConfig = dbFun.DoQuery(strSQL, parameter).ToList<GPI_CountersignApproversConfig>();

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

            return GPI_countersignViewModel;
        }

        /// <summary>
        /// 四方四隅_會簽單(依此單內容重送)(僅外部起單使用)
        /// </summary>        
        public bool PutGPI_CountersignRefill(GPI_CountersignQueryModel query)
        {
            bool vResult = false;

            try
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

                #endregion

                #region - 送出 執行(新增/修改/草稿) -

                PutGPI_CountersignSingle(GPI_countersignViewModel);

                #endregion
            
                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("四方四隅_會簽單(依此單內容重送)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 四方四隅_會簽單(新增/修改/草稿)
        /// </summary>
        public bool PutGPI_CountersignSingle(GPI_CountersignViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

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
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  model.APPLICANT_INFO.REQUISITION_ID},
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
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //四方四隅_會簽單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@LEVEL_TYPE", SqlDbType.NVarChar) { Size = 10, Value = (object)model.GPI_COUNTERSIGN_TITLE.LEVEL_TYPE ?? DBNull.Value },
                };

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
                    strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";
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
                        //行政採購申請 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
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
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@APPROVER_COMPANY_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_MAIN_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GPI_Countersign_D] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterApprovers);

                #endregion


                if (model.GPI_COUNTERSIGN_APPROVERS_CONFIG != null && model.GPI_COUNTERSIGN_APPROVERS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.GPI_COUNTERSIGN_APPROVERS_CONFIG)
                    {
                        //寫入：四方四隅_會簽單 會簽簽核人員parameter

                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterApprovers);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GPI_Countersign_D]([RequisitionID],[ApproverCompanyID],[ApproverDeptMainID],[ApproverDeptID],[ApproverID],[ApproverName]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@APPROVER_COMPANY_ID,@APPROVER_DEPT_MAIN_ID,@APPROVER_DEPT_ID,@APPROVER_ID,@APPROVER_NAME) ";

                        dbFun.DoTran(strSQL, parameterApprovers);
                    }

                    #endregion
                }

                #endregion

                #region - 四方四隅_會簽單 表單關聯：AssociatedForm -

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ASSOCIATED_FORM_CONFIG = model.ASSOCIATED_FORM_CONFIG
                };

                commonRepository.PutAssociatedForm(associatedFormModel);

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
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
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
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

        #endregion
    }
}