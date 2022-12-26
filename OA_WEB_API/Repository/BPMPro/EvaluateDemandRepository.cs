using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 需求評估單
    /// </summary>
    public class EvaluateDemandRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 需求評估單(查詢)
        /// </summary>
        public EvaluateDemandViewModel PostEvaluateDemandSingle(EvaluateDemandQueryModel query)
        {
            #region - 查詢 -

            #region - 申請人資訊 -

            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateDemand_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 需求評估設定及內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [Compendium] AS [COMPENDIUM], ";
            strSQL += "     [ContactPerson] AS [CONTACT_PERSON], ";
            strSQL += "     [Evaluate] AS [EVALUATE], ";
            strSQL += "     [Scheme] AS [SCHEME], ";
            strSQL += "     [ProcessResult] AS [PROCESS_RESULT], ";
            strSQL += "     [ApproveLoop] AS [APPROVE_LOOP] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateDemand_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateDemandConfig = dbFun.DoQuery(strSQL, parameterA).ToList<EvaluateDemandConfig>().FirstOrDefault();

            #endregion

            #region - 需求評估 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            #endregion

            var evaluateDemand = new EvaluateDemandViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                EVALUATE_DEMAND_CONFIG = evaluateDemandConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            return evaluateDemand;
        }

        /// <summary>
        /// 需求評估單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutEvaluateDemandRefill(EvaluateDemandQueryModel query)
        {
            bool vResult = false;

            try
            {
                #region - 宣告 -

                var original = PostEvaluateDemandSingle(query);

                var evaluateDemand = new EvaluateDemandViewModel();
                var applicantInfo = new ApplicantInfo();
                var evaluateDemandConfig = new EvaluateDemandConfig();

                var requisitionID = Guid.NewGuid().ToString();

                #endregion

                #region - 申請人資訊 -

                applicantInfo.REQUISITION_ID = requisitionID;
                applicantInfo.DIAGRAM_ID = original.APPLICANT_INFO.DIAGRAM_ID;
                applicantInfo.PRIORITY = original.APPLICANT_INFO.PRIORITY;
                applicantInfo.DRAFT_FLAG = 1;
                applicantInfo.FLOW_ACTIVATED = original.APPLICANT_INFO.FLOW_ACTIVATED;
                applicantInfo.APPLICANT_DEPT = original.APPLICANT_INFO.APPLICANT_DEPT;
                applicantInfo.APPLICANT_DEPT_NAME = original.APPLICANT_INFO.APPLICANT_DEPT_NAME;
                applicantInfo.APPLICANT_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.APPLICANT_NAME = original.APPLICANT_INFO.APPLICANT_NAME;
                applicantInfo.APPLICANT_PHONE = original.APPLICANT_INFO.APPLICANT_PHONE;
                applicantInfo.APPLICANT_DATETIME = DateTime.Now;
                applicantInfo.FILLER_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.FILLER_NAME = original.APPLICANT_INFO.APPLICANT_NAME;

                #endregion

                #region - 需求評估單設定 -

                evaluateDemandConfig.FM7_SUBJECT = "(依此單內容重上)" + original.EVALUATE_DEMAND_CONFIG.FM7_SUBJECT;

                evaluateDemandConfig.COMPENDIUM = original.EVALUATE_DEMAND_CONFIG.COMPENDIUM;
                evaluateDemandConfig.CONTACT_PERSON = original.EVALUATE_DEMAND_CONFIG.CONTACT_PERSON;
                evaluateDemandConfig.EVALUATE = original.EVALUATE_DEMAND_CONFIG.EVALUATE;
                evaluateDemandConfig.SCHEME = original.EVALUATE_DEMAND_CONFIG.SCHEME;
                evaluateDemandConfig.PROCESS_RESULT = original.EVALUATE_DEMAND_CONFIG.PROCESS_RESULT;
                evaluateDemandConfig.APPROVE_LOOP = 0;

                #endregion

                evaluateDemand.APPLICANT_INFO = applicantInfo;
                evaluateDemand.EVALUATE_DEMAND_CONFIG = evaluateDemandConfig;
                evaluateDemand.ASSOCIATED_FORM_CONFIG = original.ASSOCIATED_FORM_CONFIG;

                PutEvaluateDemandSingle(evaluateDemand);

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("跑馬申請單(依此單內容重送)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 需求評估單(新增/修改/草稿)
        /// </summary>
        public bool PutEvaluateDemandSingle(EvaluateDemandViewModel model)
        {
            bool vResult = false;

            try
            {
                #region - 宣告主旨 -

                FM7Subject = model.EVALUATE_DEMAND_CONFIG.FM7_SUBJECT;

                #endregion

                #region - 需求評估主表：EvaluateDemand_M -

                var parameterA = new List<SqlParameter>()
                {
                    //申請人資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = model.APPLICANT_INFO.PRIORITY },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = model.APPLICANT_INFO.DRAFT_FLAG },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = model.APPLICANT_INFO.FLOW_ACTIVATED },
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //合作夥伴審核單設定
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@COMPENDIUM", SqlDbType.NVarChar) { Size = 4000, Value = (object)model.EVALUATE_DEMAND_CONFIG.COMPENDIUM ?? DBNull.Value },
                    new SqlParameter("@CONTACT_PERSON", SqlDbType.NVarChar) { Size = 10, Value = (object)model.EVALUATE_DEMAND_CONFIG.CONTACT_PERSON ?? DBNull.Value },
                    new SqlParameter("@EVALUATE", SqlDbType.NVarChar) { Size = 4000, Value = (object)model.EVALUATE_DEMAND_CONFIG.EVALUATE ?? DBNull.Value },
                    new SqlParameter("@SCHEME", SqlDbType.NVarChar) { Size = 25, Value = (object)model.EVALUATE_DEMAND_CONFIG.SCHEME ?? DBNull.Value },
                    new SqlParameter("@PROCESS_RESULT", SqlDbType.NVarChar) { Size = 4000, Value = (object)model.EVALUATE_DEMAND_CONFIG.PROCESS_RESULT ?? DBNull.Value },
                    new SqlParameter("@APPROVE_LOOP", SqlDbType.Int) { Value = (object)model.EVALUATE_DEMAND_CONFIG.APPROVE_LOOP ?? DBNull.Value },
                };

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateDemand_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateDemand_M] ";
                    strSQL += "SET  [DiagramID]=@DIAGRAM_ID, ";
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
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT,";
                    strSQL += "     [Compendium]=@COMPENDIUM, ";
                    strSQL += "     [ContactPerson]=@CONTACT_PERSON, ";
                    strSQL += "     [Evaluate]=@EVALUATE, ";
                    strSQL += "     [Scheme]=@SCHEME, ";
                    strSQL += "     [ProcessResult]=@PROCESS_RESULT, ";
                    strSQL += "     [ApproveLoop]=@APPROVE_LOOP ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_EvaluateDemand_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[FillerID],[FillerName],[ApplicantPhone],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[Compendium],[ContactPerson],[Evaluate],[Scheme],[ProcessResult],[ApproveLoop]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@FILLER_ID,@FILLER_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@COMPENDIUM,@CONTACT_PERSON,NULL,NULL,NULL,1) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region - 需求評估 表單關聯：AssociatedForm -

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
                
                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("合作夥伴審核單(新增/修改/草稿)失敗，原因：" + ex.Message);

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
        private string IDENTIFY = "EvaluateDemand";

        /// <summary>
        /// 表單主旨
        /// </summary>
        private string FM7Subject;

        #endregion

    }
}