using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.BPMPro;

using Newtonsoft.Json;


namespace OA_WEB_API.Repository.ERP
{
    /// <summary>
    /// 回傳ERP資訊
    /// </summary>
    public class ResponseInfoRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Model

        ErpResponseState erpResponseState = new ErpResponseState();

        #endregion

        #region Repository

        FormRepository formRepository = new FormRepository();
        StepFlowRepository stepFlowRepository = new StepFlowRepository();

        #endregion

        #region FormRepository

        /// <summary>行政採購申請單</summary>
        GeneralOrderRepository generalOrderRepository = new GeneralOrderRepository();
        /// <summary>行政採購異動申請單</summary>
        GeneralOrderChangeRepository generalOrderChangeRepository = new GeneralOrderChangeRepository();
        /// <summary>行政採購點驗收單</summary>
        GeneralAcceptanceRepository generalAcceptanceRepository = new GeneralAcceptanceRepository();
        /// <summary>行政採購請款單</summary>
        GeneralInvoiceRepository generalInvoiceRepository = new GeneralInvoiceRepository();

        #endregion

        #endregion

        #region  - 方法 -

        #region - 專案建立審核單 財務審核資訊回傳ERP -

        /// <summary>
        /// 專案建立審核單 財務審核資訊回傳ERP
        /// </summary>
        public ProjectReviewFinanceRequest PostProjectReviewFinanceSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 專案建立審核單 財務審核資訊查詢 -

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [M].[RequisitionID] AS [REQUISITION_ID],";
                strSQL += "     [M].[AccCategory] AS [ACC_CATEGORY] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_ProjectReview_M] AS [M] ";
                strSQL += "LEFT JOIN [BPMPro].[dbo].[FM7T_ProjectReview_S] AS [S] ON [M].[RequisitionID]=[S].[RequisitionID] AND [S].[ApproverName] IS NOT NULL ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [M].[RequisitionID]=@REQUISITION_ID ";
                strSQL += "ORDER BY [S].[ApproveTime] DESC ";
                                
                var projectReviewFinanceConfig = dbFun.DoQuery(strSQL, parameter).ToList<ProjectReviewFinanceConfig>().FirstOrDefault();

                #endregion

                #region 表單簽核狀態

                //表單資料
                var formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                var formData = formRepository.PostFormData(formQueryModel);
                var stepFlowConfig = stepFlowRepository.StepFlowInfo(formData, parameter);

                #endregion

                #region - 回傳ERP - 

                var projectReviewFinanceRequest = new ProjectReviewFinanceRequest()
                {
                    RequisitionID = projectReviewFinanceConfig.REQUISITION_ID,
                    AccCategory = projectReviewFinanceConfig.ACC_CATEGORY,
                    LoginId = stepFlowConfig.APPROVER_ID,
                    LoginName = stepFlowConfig.APPROVER_NAME
                };

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI + "BPM/UpdatePrjListInfoFromBPM";
                    Method = "POST";
                    strRequestJson = JsonConvert.SerializeObject(projectReviewFinanceRequest);
                    strResponseJson = GlobalParameters.RequestInfoWebServers(ApiUrl, Method, strRequestJson);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("專案建立審核單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    projectReviewFinanceRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(projectReviewFinanceRequest);
                CommLib.Logger.Debug("專案建立審核單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return projectReviewFinanceRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("專案建立審核單:" + query.REQUISITION_ID + " 財務審核資訊 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購申請單 申請審核資訊回傳ERP -

        /// <summary>
        /// 行政採購申請單 申請審核資訊回傳ERP
        /// </summary>
        public GeneralOrderInfoRequest PostGeneralOrderInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購申請單 申請審核資訊查詢 -

                #region 回傳表單內容

                GeneralOrderQueryModel generalOrderquery = new GeneralOrderQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                GeneralOrderInfoRequest generalOrderInfoRequest = new GeneralOrderInfoRequest();
                var generalOrderContent = generalOrderRepository.PostGeneralOrderSingle(generalOrderquery);
                //Join 行政採購申請單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(generalOrderContent);
                //給予需要回傳ERP的資訊
                generalOrderInfoRequest = jsonFunction.JsonToObject<GeneralOrderInfoRequest>(strJson);
                generalOrderInfoRequest.REQUISITION_ID = generalOrderContent.APPLICANT_INFO.REQUISITION_ID;

                #endregion

                #region 表單簽核狀態

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                };
                //表單資料
                var formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                var formData = formRepository.PostFormData(formQueryModel);
                var stepFlowConfig = stepFlowRepository.StepFlowInfo(formData, parameter);

                #endregion

                #endregion

                #region - 回傳ERP - 

                generalOrderInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                generalOrderInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI + "BPM/UpdatePO_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, generalOrderInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("行政採購申請單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    generalOrderInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(generalOrderInfoRequest);
                CommLib.Logger.Debug("行政採購申請單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return generalOrderInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購申請單:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購異動申請單 異動申請資訊回傳ERP -

        /// <summary>
        /// 行政採購異動申請單 異動申請資訊回傳ERP
        /// </summary>
        public GeneralOrderChangeInfoRequest PostGeneralOrderChangeInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購異動申請單 異動申請資訊查詢 -

                #region 回傳表單內容

                GeneralOrderChangeQueryModel generalOrderChangeQueryModel = new GeneralOrderChangeQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                GeneralOrderChangeInfoRequest generalOrderChangeInfoRequest = new GeneralOrderChangeInfoRequest();
                var generalOrderChangeContent = generalOrderChangeRepository.PostGeneralOrderChangeSingle(generalOrderChangeQueryModel);
                //Join 行政採購異動申請單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(generalOrderChangeContent);
                //給予需要回傳ERP的資訊
                generalOrderChangeInfoRequest.GENERAL_ORDER_INFO_CONFIG = jsonFunction.JsonToObject<GeneralOrderChangeInfoConfig>(strJson);
                generalOrderChangeInfoRequest.REQUISITION_ID = generalOrderChangeContent.APPLICANT_INFO.REQUISITION_ID;

                #endregion

                #region 表單簽核狀態

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                };
                //表單資料
                var formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                var formData = formRepository.PostFormData(formQueryModel);
                var stepFlowConfig = stepFlowRepository.StepFlowInfo(formData, parameter);

                #endregion

                #endregion

                #region - 回傳ERP - 

                generalOrderChangeInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                generalOrderChangeInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI + "BPM/";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, generalOrderChangeInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("行政採購異動申請單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    generalOrderChangeInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(generalOrderChangeInfoRequest);
                CommLib.Logger.Debug("行政採購異動申請單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return generalOrderChangeInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購異動申請單:" + query.REQUISITION_ID + " 異動申請資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購點驗收單 驗收明細回傳ERP -

        /// <summary>
        /// 行政採購點驗收單 驗收明細回傳ERP
        /// </summary>
        public GeneralAcceptanceInfoRequest PostGeneralAcceptanceInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購點驗收單 內容資訊查詢 -

                #region 回傳表單內容

                GeneralAcceptanceQueryModel generalAcceptanceQueryModel = new GeneralAcceptanceQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                GeneralAcceptanceInfoRequest generalAcceptanceInfoRequest = new GeneralAcceptanceInfoRequest();
                var generalAcceptanceContent = generalAcceptanceRepository.PostGeneralAcceptanceSingle(generalAcceptanceQueryModel);
                //Join 行政採購點驗收單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(generalAcceptanceContent);
                //給予需要回傳ERP的資訊
                generalAcceptanceInfoRequest = jsonFunction.JsonToObject<GeneralAcceptanceInfoRequest>(strJson);
                generalAcceptanceInfoRequest.REQUISITION_ID = generalAcceptanceContent.APPLICANT_INFO.REQUISITION_ID;

                #endregion

                #region 表單簽核狀態

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                };
                //表單資料
                var formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                var formData = formRepository.PostFormData(formQueryModel);
                var stepFlowConfig = stepFlowRepository.StepFlowInfo(formData, parameter);

                #endregion

                #endregion

                #region - 回傳ERP - 

                generalAcceptanceInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                generalAcceptanceInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;
                
                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI + "BPM/UpdateAcpt_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, generalAcceptanceInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("行政採購點驗收單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    generalAcceptanceInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(generalAcceptanceInfoRequest);
                CommLib.Logger.Debug("行政採購點驗收單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return generalAcceptanceInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購點驗收單:" + query.REQUISITION_ID + " 驗收明細回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購請款單 財務簽核資訊回傳ERP -

        /// <summary>
        /// 行政採購請款單 財務簽核資訊回傳ERP
        /// </summary>
        public GeneralInvoiceInfoRequest PostGeneralInvoiceInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購點驗收單 內容資訊查詢 -

                #region 回傳表單內容

                GeneralInvoiceQueryModel generalInvoiceQueryModel = new GeneralInvoiceQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                GeneralInvoiceInfoRequest generalInvoiceInfoRequest = new GeneralInvoiceInfoRequest();
                var generalInvoiceContent = generalInvoiceRepository.PostGeneralInvoiceSingle(generalInvoiceQueryModel);
                //Join 行政採購點驗收單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(generalInvoiceContent);
                //給予需要回傳ERP的資訊
                generalInvoiceInfoRequest = jsonFunction.JsonToObject<GeneralInvoiceInfoRequest>(strJson);
                generalInvoiceInfoRequest.REQUISITION_ID = generalInvoiceContent.APPLICANT_INFO.REQUISITION_ID;

                #endregion

                #region 表單簽核狀態

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                };
                //表單資料
                var formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                var formData = formRepository.PostFormData(formQueryModel);
                var stepFlowConfig = stepFlowRepository.StepFlowInfo(formData, parameter);

                #endregion

                #endregion

                #region - 回傳ERP - 

                generalInvoiceInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                generalInvoiceInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI + "BPM/UpdateSI_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, generalInvoiceInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("行政採購請款單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    generalInvoiceInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(generalInvoiceInfoRequest);
                CommLib.Logger.Debug("行政採購請款單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return generalInvoiceInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購請款單:" + query.REQUISITION_ID + " 財務簽核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }           
        }

        #endregion

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        /// <summary>
        /// Json字串
        /// </summary>
        private string strJson;

        #region - 請求信息欄位 -

        /// <summary>
        /// 請求信息-Api路徑
        /// </summary>
        private string ApiUrl;

        /// <summary>
        /// 請求信息-取得或要求的方法
        /// </summary>
        private string Method;

        /// <summary>
        /// 請求信息-輸入值
        /// </summary>
        private string strRequestJson;

        /// <summary>
        /// 請求信息-接收回傳值
        /// </summary>
        private string strResponseJson;

        #endregion

        #endregion
    }
}