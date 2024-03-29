﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.BPMPro;

using Newtonsoft.Json;
using System.Drawing;
using Microsoft.Ajax.Utilities;

namespace OA_WEB_API.Repository.ERP
{
    /// <summary>
    /// 回傳ERP資訊
    /// </summary>
    public class ResponseInfoRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Model

        ErpResponseState erpResponseState = new ErpResponseState();

        #endregion

        #region Repository

        FormRepository formRepository = new FormRepository();
        StepFlowRepository stepFlowRepository = new StepFlowRepository();
        UserRepository userRepository = new UserRepository();

        #endregion

        #region FormRepository

        /// <summary>費用申請單</summary>
        ExpensesReimburseRepository expensesReimburseRepository = new ExpensesReimburseRepository();

        #region 行政採購類

        /// <summary>行政採購申請單</summary>
        GeneralOrderRepository generalOrderRepository = new GeneralOrderRepository();
        /// <summary>行政採購點驗收單</summary>
        GeneralAcceptanceRepository generalAcceptanceRepository = new GeneralAcceptanceRepository();
        /// <summary>行政採購請款單</summary>
        GeneralInvoiceRepository generalInvoiceRepository = new GeneralInvoiceRepository();
        /// <summary>行政採購退貨折讓單</summary>
        GeneralOrderReturnRefundRepository generalOrderReturnRefundRepository = new GeneralOrderReturnRefundRepository();

        #endregion

        #region 內容評估表

        /// <summary>內容評估表</summary>
        EvaluateContentRepository evaluateContentRepository = new EvaluateContentRepository();
        /// <summary>內容評估表_補充意見</summary>
        EvaluateContentReplenishRepository evaluateContentReplenishRepository = new EvaluateContentReplenishRepository();

        #endregion

        #region 版權採購類

        /// <summary>版權採購申請單</summary>
        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();
        /// <summary>版權採購交片單</summary>
        MediaAcceptanceRepository mediaAcceptanceRepository = new MediaAcceptanceRepository();
        /// <summary>版權採購請款單</summary>
        MediaInvoiceRepository mediaInvoiceRepository = new MediaInvoiceRepository();
        /// <summary>版權採購退貨折讓單</summary>
        MediaOrderReturnRefundRepository mediaOrderReturnRefundRepository = new MediaOrderReturnRefundRepository();

        #endregion

        #region 四方四隅

        /// <summary>四方四隅_內容評估表</summary>
        GPI_EvaluateContentRepository GPI_evaluateContentRepository = new GPI_EvaluateContentRepository();
        /// <summary>四方四隅_內容評估表_補充意見</summary>
        GPI_EvaluateContentReplenishRepository GPI_evaluateContentReplenishRepository = new GPI_EvaluateContentReplenishRepository();

        #endregion

        #endregion

        #endregion

        #region  - 方法 -

        #region - 專案建立審核單 財務審核資訊_回傳ERP -

        /// <summary>
        /// 專案建立審核單 財務審核資訊_回傳ERP
        /// </summary>
        public ProjectReviewFinanceRequest PostProjectReviewFinanceSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 專案建立審核單 財務審核資訊 -

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
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdatePrjListInfoFromBPM";
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

        #region - 費用申請單 審核資訊_回傳ERP -

        /// <summary>
        /// 費用申請單 審核資訊_回傳ERP
        /// </summary>
        public ExpensesReimburseInfoRequest PostExpensesReimburseInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 費用申請單 申請審核資訊 -

                #region 回傳表單內容

                ExpensesReimburseQueryModel expensesReimburseQueryModel = new ExpensesReimburseQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                ExpensesReimburseInfoRequest expensesReimburseInfoRequest = new ExpensesReimburseInfoRequest();
                var expensesReimburseContent = expensesReimburseRepository.PostExpensesReimburseSingle(expensesReimburseQueryModel);
                //Join 費用申請單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(expensesReimburseContent);
                //給予需要回傳ERP的資訊
                expensesReimburseInfoRequest = jsonFunction.JsonToObject<ExpensesReimburseInfoRequest>(strJson);
                expensesReimburseInfoRequest.REQUISITION_ID = expensesReimburseContent.APPLICANT_INFO.REQUISITION_ID;

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

                expensesReimburseInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                expensesReimburseInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateER_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, expensesReimburseInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("費用申請單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    expensesReimburseInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(expensesReimburseInfoRequest);
                CommLib.Logger.Debug("費用申請單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return expensesReimburseInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("費用申請單:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購類_回傳ERP資訊 -

        #region - 行政採購申請單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購申請單 審核資訊_回傳ERP
        /// </summary>
        public GeneralOrderInfoRequest PostGeneralOrderInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購申請單 申請審核資訊 -

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
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdatePO_DetailContent";
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

        #region - 行政採購點驗收單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購點驗收單 驗收審核資訊_回傳ERP
        /// </summary>
        public GeneralAcceptanceInfoRequest PostGeneralAcceptanceInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購點驗收單 驗收審核資訊 -

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
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateAcpt_DetailContent";
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

        #region - 行政採購請款單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購請款單 審核資訊_回傳ERP
        /// </summary>
        public GeneralInvoiceInfoRequest PostGeneralInvoiceInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購請款單 財務審核資訊 -

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
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateSI_DetailContent";
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

        #region - 行政採購退貨折讓單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購退貨折讓單 審核資訊_回傳ERP
        /// </summary>
        public GeneralOrderReturnRefundInfoRequest PostGeneralOrderReturnRefundInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 行政採購退貨折讓單 財務審核資訊 -

                #region 回傳表單內容

                GeneralOrderReturnRefundQueryModel generalOrderReturnRefundQueryModel = new GeneralOrderReturnRefundQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                GeneralOrderReturnRefundInfoRequest generalOrderReturnRefundInfoRequest = new GeneralOrderReturnRefundInfoRequest();
                var generalOrderReturnRefundContent = generalOrderReturnRefundRepository.PostGeneralOrderReturnRefundSingle(generalOrderReturnRefundQueryModel);
                generalOrderReturnRefundInfoRequest.GENERAL_ORDER_RETURN_REFUND_VIEW = generalOrderReturnRefundContent;

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

                generalOrderReturnRefundInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                generalOrderReturnRefundInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateDN_A_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, generalOrderReturnRefundInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("行政採購退貨折讓單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    generalOrderReturnRefundInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(generalOrderReturnRefundInfoRequest);
                CommLib.Logger.Debug("行政採購請款單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return generalOrderReturnRefundInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購退貨折讓單:" + query.REQUISITION_ID + " 財務簽核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 內容評估表_回傳ERP資訊  -

        #region - 內容評估表 審核資訊_回傳ERP -

        /// <summary>
        /// 內容評估表 審核資訊_回傳ERP
        /// </summary>
        public EvaluateContentInfoRequest PostEvaluateContentInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 內容評估表 申請審核資訊 -

                #region 回傳表單內容

                var evaluateContentQueryModel = new EvaluateContentQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                EvaluateContentInfoRequest evaluateContentInfoRequest = new EvaluateContentInfoRequest();
                var evaluateContentContent = evaluateContentRepository.PostEvaluateContentSingle(evaluateContentQueryModel);
                //Join 內容評估表(查詢)Function
                strJson = jsonFunction.ObjectToJSON(evaluateContentContent);
                //給予需要回傳ERP的資訊
                evaluateContentInfoRequest = jsonFunction.JsonToObject<EvaluateContentInfoRequest>(strJson);
                evaluateContentInfoRequest.REQUISITION_ID = evaluateContentContent.APPLICANT_INFO.REQUISITION_ID;               

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

                evaluateContentInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                evaluateContentInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if(!String.IsNullOrEmpty(evaluateContentInfoRequest.LoginId) || !String.IsNullOrWhiteSpace(evaluateContentInfoRequest.LoginId))
                {
                    evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE = evaluateContentInfoRequest.EVALUATE_CONTENT_DECS_CONFIG.Where(D => D.USER_ID == evaluateContentInfoRequest.LoginId).Select(D => D.ADVISE_TYPE).LastOrDefault();
                    if (String.IsNullOrEmpty(evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE) || String.IsNullOrWhiteSpace(evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE))
                    {
                        evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE = evaluateContentInfoRequest.EVALUATE_CONTENT_EVAS_CONFIG.Where(E => E.USER_ID == evaluateContentInfoRequest.LoginId).Select(E => E.ADVISE_TYPE).LastOrDefault();
                    }
                }

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateEval_M_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, evaluateContentInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("內容評估表:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    evaluateContentInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(evaluateContentInfoRequest);
                CommLib.Logger.Debug("內容評估表:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return evaluateContentInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("內容評估表:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 內容評估表_補充意見 審核資訊_回傳ERP -

        /// <summary>
        /// 內容評估表_補充意見 審核資訊_回傳ERP
        /// </summary>
        public EvaluateContentInfoRequest PostEvaluateContentReplenishInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 內容評估表_補充意見 申請審核資訊 -

                #region 回傳表單內容

                var evaluateContentReplenishQueryModel = new EvaluateContentReplenishQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                EvaluateContentInfoRequest evaluateContentReplenishInfoRequest = new EvaluateContentInfoRequest();
                var evaluateContentReplenishContent = evaluateContentReplenishRepository.PostEvaluateContentReplenishSingle(evaluateContentReplenishQueryModel);
                //Join 內容評估表_補充意見(查詢)Function
                strJson = jsonFunction.ObjectToJSON(evaluateContentReplenishContent);
                strJson = strJson.Replace("EVALUATE_CONTENT_REPLENISH_TITLE", "EVALUATE_CONTENT_TITLE");
                strJson = strJson.Replace("EVALUATE_CONTENT_REPLENISH_CONFIG", "EVALUATE_CONTENT_CONFIG");
                //strJson = strJson.Replace("", "EVALUATE_CONTENT_USERS_CONFIG");
                strJson = strJson.Replace("EVALUATE_CONTENT_REPLENISH_EVAS_CONFIG", "EVALUATE_CONTENT_EVAS_CONFIG");
                strJson = strJson.Replace("EVALUATE_CONTENT_REPLENISH_DECS_CONFIG", "EVALUATE_CONTENT_DECS_CONFIG");
                //給予需要回傳ERP的資訊
                evaluateContentReplenishInfoRequest = jsonFunction.JsonToObject<EvaluateContentInfoRequest>(strJson);
                evaluateContentReplenishInfoRequest.REQUISITION_ID = evaluateContentReplenishContent.APPLICANT_INFO.REQUISITION_ID;
                evaluateContentReplenishInfoRequest.EVALUATE_CONTENT_USERS_CONFIG = new List<EvaluateContentUsersConfig>();

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

                evaluateContentReplenishInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                evaluateContentReplenishInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (!String.IsNullOrEmpty(evaluateContentReplenishInfoRequest.LoginId) || !String.IsNullOrWhiteSpace(evaluateContentReplenishInfoRequest.LoginId))
                {
                    evaluateContentReplenishInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE = evaluateContentReplenishInfoRequest.EVALUATE_CONTENT_DECS_CONFIG.Where(D => D.USER_ID == evaluateContentReplenishInfoRequest.LoginId).Select(D => D.ADVISE_TYPE).LastOrDefault();
                }
                
                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateEval_M_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, evaluateContentReplenishInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("內容評估表_補充意見:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    evaluateContentReplenishInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(evaluateContentReplenishInfoRequest);
                CommLib.Logger.Debug("內容評估表_補充意見:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return evaluateContentReplenishInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("內容評估表_補充意見:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 版權採購類_回傳ERP資訊 -

        #region - 版權採購申請單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購申請單 審核資訊_回傳ERP
        /// </summary>
        public MediaOrderInfoRequest PostMediaOrderInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 版權採購申請單 申請審核資訊 -

                #region 回傳表單內容

                var mediaOrderQueryModel = new MediaOrderQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                MediaOrderInfoRequest mediaOrderInfoRequest = new MediaOrderInfoRequest();
                var mediaOrderContent = mediaOrderRepository.PostMediaOrderSingle(mediaOrderQueryModel);
                //Join 版權採購申請單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(mediaOrderContent);
                //給予需要回傳ERP的資訊
                mediaOrderInfoRequest = jsonFunction.JsonToObject<MediaOrderInfoRequest>(strJson);
                mediaOrderInfoRequest.REQUISITION_ID = mediaOrderContent.APPLICANT_INFO.REQUISITION_ID;

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

                mediaOrderInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                mediaOrderInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateMO_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, mediaOrderInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("版權採購申請單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    mediaOrderInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(mediaOrderInfoRequest);
                CommLib.Logger.Debug("版權採購申請單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return mediaOrderInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購申請單:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 版權採購交片單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購交片單 審核資訊_回傳ERP
        /// </summary>
        public MediaAcceptanceInfoRequest PostMediaAcceptanceInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 版權採購交片單 驗收審核資訊 -

                #region 回傳表單內容

                MediaAcceptanceQueryModel mediaAcceptanceQueryModel = new MediaAcceptanceQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                MediaAcceptanceInfoRequest mediaAcceptanceInfoRequest = new MediaAcceptanceInfoRequest();
                var mediaAcceptanceContent = mediaAcceptanceRepository.PostMediaAcceptanceSingle(mediaAcceptanceQueryModel);
                //Join 版權採購交片單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(mediaAcceptanceContent);
                //給予需要回傳ERP的資訊
                mediaAcceptanceInfoRequest = jsonFunction.JsonToObject<MediaAcceptanceInfoRequest>(strJson);
                mediaAcceptanceInfoRequest.REQUISITION_ID = mediaAcceptanceContent.APPLICANT_INFO.REQUISITION_ID;

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

                mediaAcceptanceInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                mediaAcceptanceInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateMR_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, mediaAcceptanceInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("版權採購交片單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    mediaAcceptanceInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(mediaAcceptanceInfoRequest);
                CommLib.Logger.Debug("版權採購交片單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return mediaAcceptanceInfoRequest;

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購交片單:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }


        #endregion

        #region - 版權採購請款單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購請款單 審核資訊_回傳ERP
        /// </summary>
        public MediaInvoiceInfoRequest PostMediaInvoiceInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 版權採購請款單 財務審核資訊 -

                #region 回傳表單內容

                MediaInvoiceQueryModel mediaInvoiceQueryModel = new MediaInvoiceQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                MediaInvoiceInfoRequest mediaInvoiceInfoRequest = new MediaInvoiceInfoRequest();
                var mediaInvoiceContent = mediaInvoiceRepository.PostMediaInvoiceSingle(mediaInvoiceQueryModel);
                //Join 版權採購請款單(查詢)Function
                strJson = jsonFunction.ObjectToJSON(mediaInvoiceContent);
                //給予需要回傳ERP的資訊
                mediaInvoiceInfoRequest = jsonFunction.JsonToObject<MediaInvoiceInfoRequest>(strJson);
                mediaInvoiceInfoRequest.REQUISITION_ID = mediaInvoiceContent.APPLICANT_INFO.REQUISITION_ID;

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

                mediaInvoiceInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                mediaInvoiceInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateSI_M_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, mediaInvoiceInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("版權採購請款單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    mediaInvoiceInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(mediaInvoiceInfoRequest);
                CommLib.Logger.Debug("版權採購請款單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return mediaInvoiceInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購請款單:" + query.REQUISITION_ID + " 財務簽核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 版權採購退貨折讓單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購退貨折讓單 審核資訊_回傳ERP
        /// </summary>
        public MediaOrderReturnRefundInfoRequest PostMediaOrderReturnRefundInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 版權採購退貨折讓單 財務審核資訊 -

                #region 回傳表單內容

                MediaOrderReturnRefundQueryModel mediaOrderReturnRefundQueryModel = new MediaOrderReturnRefundQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                MediaOrderReturnRefundInfoRequest mediaOrderReturnRefundInfoRequest = new MediaOrderReturnRefundInfoRequest();
                var mediaOrderReturnRefundContent = mediaOrderReturnRefundRepository.PostMediaOrderReturnRefundSingle(mediaOrderReturnRefundQueryModel);
                mediaOrderReturnRefundInfoRequest.MEDIA_ORDER_RETURN_REFUND_VIEW = mediaOrderReturnRefundContent;

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

                mediaOrderReturnRefundInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                mediaOrderReturnRefundInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateDN_M_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, mediaOrderReturnRefundInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("版權採購退貨折讓單:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    mediaOrderReturnRefundInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(mediaOrderReturnRefundInfoRequest);
                CommLib.Logger.Debug("版權採購退貨折讓單:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return mediaOrderReturnRefundInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購退貨折讓單:" + query.REQUISITION_ID + " 簽核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 四方四隅_回傳ERP資訊  -

        #region - 四方四隅_內容評估表 審核資訊_回傳ERP -

        /// <summary>
        /// 四方四隅_內容評估表 審核資訊_回傳ERP
        /// </summary>
        public EvaluateContentInfoRequest PostGPI_EvaluateContentInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 四方四隅_內容評估表 申請審核資訊 -

                #region 回傳表單內容

                var GPI_evaluateContentQueryModel = new GPI_EvaluateContentQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                EvaluateContentInfoRequest GPI_evaluateContentInfoRequest = new EvaluateContentInfoRequest();
                var GPI_evaluateContentContent = GPI_evaluateContentRepository.PostGPI_EvaluateContentSingle(GPI_evaluateContentQueryModel);
                //Join 內容評估表(查詢)Function
                strJson = jsonFunction.ObjectToJSON(GPI_evaluateContentContent);
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_TITLE", "EVALUATE_CONTENT_TITLE");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_CONFIG", "EVALUATE_CONTENT_CONFIG");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_USERS_CONFIG", "EVALUATE_CONTENT_USERS_CONFIG");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_EVAS_CONFIG", "EVALUATE_CONTENT_EVAS_CONFIG");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_DECS_CONFIG", "EVALUATE_CONTENT_DECS_CONFIG");

                //給予需要回傳ERP的資訊
                GPI_evaluateContentInfoRequest = jsonFunction.JsonToObject<EvaluateContentInfoRequest>(strJson);
                GPI_evaluateContentInfoRequest.REQUISITION_ID = GPI_evaluateContentContent.APPLICANT_INFO.REQUISITION_ID;

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

                GPI_evaluateContentInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                GPI_evaluateContentInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (!String.IsNullOrEmpty(GPI_evaluateContentInfoRequest.LoginId) || !String.IsNullOrWhiteSpace(GPI_evaluateContentInfoRequest.LoginId))
                {
                    GPI_evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE = GPI_evaluateContentInfoRequest.EVALUATE_CONTENT_DECS_CONFIG.Where(D => D.USER_ID == GPI_evaluateContentInfoRequest.LoginId).Select(D => D.ADVISE_TYPE).LastOrDefault();
                    if (String.IsNullOrEmpty(GPI_evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE) || String.IsNullOrWhiteSpace(GPI_evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE))
                    {
                        GPI_evaluateContentInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE = GPI_evaluateContentInfoRequest.EVALUATE_CONTENT_EVAS_CONFIG.Where(E => E.USER_ID == GPI_evaluateContentInfoRequest.LoginId).Select(E => E.ADVISE_TYPE).LastOrDefault();
                    }
                }

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateEval_M_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, GPI_evaluateContentInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("四方四隅_內容評估表:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    GPI_evaluateContentInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(GPI_evaluateContentInfoRequest);
                CommLib.Logger.Debug("四方四隅_內容評估表:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return GPI_evaluateContentInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("四方四隅_內容評估表:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 四方四隅_內容評估表_補充意見 審核資訊_回傳ERP -

        /// <summary>
        /// 四方四隅_內容評估表_補充意見 審核資訊_回傳ERP
        /// </summary>
        public EvaluateContentInfoRequest PostGPI_EvaluateContentReplenishInfoSingle(RequestQueryModel query)
        {
            try
            {
                #region - 查詢及執行 -

                #region - 四方四隅_內容評估表_補充意見 申請審核資訊 -

                #region 回傳表單內容

                var GPI_evaluateContentReplenishQueryModel = new GPI_EvaluateContentReplenishQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                EvaluateContentInfoRequest GPI_evaluateContentReplenishInfoRequest = new EvaluateContentInfoRequest();
                var GPI_evaluateContentReplenishContent = GPI_evaluateContentReplenishRepository.PostGPI_EvaluateContentReplenishSingle(GPI_evaluateContentReplenishQueryModel);
                //Join 四方四隅_內容評估表_補充意見(查詢)Function
                strJson = jsonFunction.ObjectToJSON(GPI_evaluateContentReplenishContent);
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_REPLENISH_TITLE", "EVALUATE_CONTENT_TITLE");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_REPLENISH_CONFIG", "EVALUATE_CONTENT_CONFIG");
                //strJson = strJson.Replace("", "EVALUATE_CONTENT_USERS_CONFIG");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_REPLENISH_EVAS_CONFIG", "EVALUATE_CONTENT_EVAS_CONFIG");
                strJson = strJson.Replace("GPI_EVALUATE_CONTENT_REPLENISH_DECS_CONFIG", "EVALUATE_CONTENT_DECS_CONFIG");

                //給予需要回傳ERP的資訊
                GPI_evaluateContentReplenishInfoRequest = jsonFunction.JsonToObject<EvaluateContentInfoRequest>(strJson);
                GPI_evaluateContentReplenishInfoRequest.REQUISITION_ID = GPI_evaluateContentReplenishContent.APPLICANT_INFO.REQUISITION_ID;
                GPI_evaluateContentReplenishInfoRequest.EVALUATE_CONTENT_USERS_CONFIG = new List<EvaluateContentUsersConfig>();

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

                GPI_evaluateContentReplenishInfoRequest.LoginId = stepFlowConfig.APPROVER_ID;
                GPI_evaluateContentReplenishInfoRequest.LoginName = stepFlowConfig.APPROVER_NAME;

                if (!String.IsNullOrEmpty(GPI_evaluateContentReplenishInfoRequest.LoginId) || !String.IsNullOrWhiteSpace(GPI_evaluateContentReplenishInfoRequest.LoginId))
                {
                    GPI_evaluateContentReplenishInfoRequest.EVALUATE_CONTENT_TITLE.FINAL_ADVISE = GPI_evaluateContentReplenishInfoRequest.EVALUATE_CONTENT_DECS_CONFIG.Where(D => D.USER_ID == GPI_evaluateContentReplenishInfoRequest.LoginId).Select(D => D.ADVISE_TYPE).LastOrDefault();
                }

                if (query.REQUEST_FLG)
                {
                    ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMPro) + "BPM/UpdateEval_M_DetailContent";
                    Method = "POST";
                    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, GPI_evaluateContentReplenishInfoRequest);

                    erpResponseState = JsonConvert.DeserializeObject<ErpResponseState>(strResponseJson);
                    CommLib.Logger.Debug("四方四隅_內容評估表_補充意見:" + query.REQUISITION_ID + " ERP訊息回傳：" + erpResponseState.msg);
                    GPI_evaluateContentReplenishInfoRequest.ERP_RESPONSE_STATE = erpResponseState;
                }

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(GPI_evaluateContentReplenishInfoRequest);
                CommLib.Logger.Debug("四方四隅_內容評估表_補充意見:" + query.REQUISITION_ID + " BPM回傳內容：" + strJson);
                return GPI_evaluateContentReplenishInfoRequest;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("四方四隅_內容評估表_補充意見:" + query.REQUISITION_ID + " 申請審核資訊回傳ERP 失敗，原因：" + ex.Message);
                throw;
            }
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