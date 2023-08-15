using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 行政採購請款單
    /// </summary>
    public class GeneralInvoiceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #region FormRepository

        /// <summary>行政採購申請單</summary>
        GeneralOrderRepository generalOrderRepository = new GeneralOrderRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 行政採購請款單(查詢)
        /// </summary>
        public GeneralInvoiceViewModel PostGeneralInvoiceSingle(GeneralInvoiceQueryModel query)
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

            #region - 行政採購請款單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var generalInvoiceTitle = dbFun.DoQuery(strSQL, parameter).ToList<GeneralInvoiceTitle>().FirstOrDefault();

            #endregion

            #region - 行政採購請款單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [GeneralOrderRequisitionID] AS [GENERAL_ORDER_REQUISITION_ID], ";
            strSQL += "     [GeneralOrderSubject] AS [GENERAL_ORDER_SUBJECT], ";
            strSQL += "     [GeneralOrderBPMFormNo] AS [GENERAL_ORDER_BPM_FORM_NO], ";
            strSQL += "     [GeneralOrderERPFormNo] AS [GENERAL_ORDER_ERP_FORM_NO], ";
            strSQL += "     [GeneralOrderTXN_Type] AS [GENERAL_ORDER_TXN_TYPE], ";
            strSQL += "     [GeneralOrderPath] AS [GENERAL_ORDER_PATH], ";
            strSQL += "     [GeneralOrderPYMT_GrossTotal] AS [GENERAL_ORDER_PYMT_GROSS_TOTAL], ";
            strSQL += "     [GeneralOrderPYMT_GrossTotal_CONV] AS [GENERAL_ORDER_PYMT_GROSS_TOTAL_CONV], ";
            strSQL += "     [GeneralAcceptanceRequisitionID] AS [GENERAL_ACCEPTANCE_REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [PredictRate] AS [PRE_RATE], ";
            strSQL += "     [PricingMethod] AS [PRICING_METHOD], ";
            strSQL += "     [TaxRate] AS [TAX_RATE], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [Reimbursement] AS [REIMBURSEMENT], ";
            strSQL += "     [REIMB_StaffDeptID] AS [REIMB_STAFF_DEPT_ID], ";
            strSQL += "     [REIMB_StaffDeptName] AS [REIMB_STAFF_DEPT_NAME], ";
            strSQL += "     [REIMB_StaffID] AS [REIMB_STAFF_ID], ";
            strSQL += "     [REIMB_StaffName] AS [REIMB_STAFF_NAME], ";
            strSQL += "     [PayMethod] AS [PAY_METHOD], ";
            strSQL += "     [SupNo] AS [SUP_NO], ";
            strSQL += "     [SupName] AS [SUP_NAME], ";
            strSQL += "     [RegisterKind] AS [REG_KIND], ";
            strSQL += "     [RegisterNo] AS [REG_NO], ";
            strSQL += "     [SupTXId] AS [SUP_TX_ID], ";
            strSQL += "     [TX_Category] AS [TX_CATEGORY], ";
            strSQL += "     [BFCY_AccountNo] AS [BFCY_ACCOUNT_NO], ";
            strSQL += "     [BFCY_AccountName] AS [BFCY_ACCOUNT_NAME], ";
            strSQL += "     [BFCY_BankNo] AS [BFCY_BANK_NO], ";
            strSQL += "     [BFCY_BankName] AS [BFCY_BANK_NAME], ";
            strSQL += "     [BFCY_BanKBranchNo] AS [BFCY_BANK_BRANCH_NO], ";
            strSQL += "     [BFCY_BanKBranchName] AS [BFCY_BANK_BRANCH_NAME], ";
            strSQL += "     [BFCY_BankSWIFT] AS [BFCY_BANK_SWIFT], ";
            strSQL += "     [BFCY_BankAddress] AS [BFCY_BANK_ADDRESS], ";
            strSQL += "     [BFCY_BankCountryAndCity] AS [BFCY_BANK_COUNTRY_AND_CITY], ";
            strSQL += "     [BFCY_BankIBAN] AS [BFCY_BANK_IBAN], ";
            strSQL += "     [CurrencyName] AS [CURRENCY_NAME], ";
            strSQL += "     [BFCY_Name] AS [BFCY_NAME], ";
            strSQL += "     [BFCY_TEL] AS [BFCY_TEL], ";
            strSQL += "     [BFCY_Email] AS [BFCY_EMAIL], ";
            strSQL += "     [InvoiceType] AS [INVOICE_TYPE], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [PYMT_CurrentTotal] AS [PYMT_CURRENT_TOTAL], ";
            strSQL += "     [PYMT_CurrentTotal_TWD] AS [PYMT_CURRENT_TOTAL_TWD], ";
            strSQL += "     [INV_TaxTotal] AS [INV_TAX_TOTAL], ";
            strSQL += "     [INV_TaxTotal_TWD] AS [INV_TAX_TOTAL_TWD], ";
            strSQL += "     [INV_AmountTotal] AS [INV_AMOUNT_TOTAL], ";
            strSQL += "     [INV_AmountTotal_TWD] AS [INV_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [ActualPayAmount] AS [ACTUAL_PAY_AMOUNT], ";
            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var generalInvoiceConfig = dbFun.DoQuery(strSQL, parameter).ToList<GeneralInvoiceConfig>().FirstOrDefault();

            #endregion

            var generalOrderparameter = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = generalInvoiceConfig.GENERAL_ORDER_REQUISITION_ID },
                 new SqlParameter("@PERIOD", SqlDbType.Int) { Value = generalInvoiceConfig.PERIOD }
            };

            #region - 行政採購請款單 驗收明細 -

            //View的「驗收明細」是 行政採購申請單 的「驗收明細」加上 「採購明細」的所屬專案及備註

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     DTL.[OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     DTL.[ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     DTL.[ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     DTL.[ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     DTL.[ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     DTL.[Note] AS [NOTE], ";
            strSQL += "     ACPT.[Period] AS [PERIOD], ";
            strSQL += "     ACPT.[SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     ACPT.[ItemName] AS [ITEM_NAME], ";
            strSQL += "     ACPT.[Model] AS [MODEL], ";
            strSQL += "     ACPT.[Specifications] AS [SPECIFICATIONS], ";
            strSQL += "     ACPT.[Quantity] AS [QUANTITY], ";
            strSQL += "     ACPT.[Unit] AS [UNIT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_ACPT] AS ACPT ";
            strSQL += "	    INNER JOIN [BPMPro].[dbo].[FM7T_GeneralOrder_DTL] AS DTL ON ACPT.[RequisitionID]=DTL.[RequisitionID] AND ACPT.[SupProdANo]=DTL.[SupProdANo] AND ACPT.[OrderRowNo]=DTL.[OrderRowNo] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND ACPT.[RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND ACPT.[Period]=@PERIOD ";

            var generalInvoiceAcceptancesConfig = dbFun.DoQuery(strSQL, generalOrderparameter).ToList<GeneralInvoiceAcceptancesConfig>();

            #endregion

            #region - 行政採購單 資訊 -

            var generalOrderQueryModel = new GeneralOrderQueryModel
            {
                REQUISITION_ID = generalInvoiceConfig.GENERAL_ORDER_REQUISITION_ID
            };

            var generalOrderContent = generalOrderRepository.PostGeneralOrderSingle(generalOrderQueryModel);

            #endregion

            #region - 行政採購請款單 付款辦法 -
            //View的「付款辦法」是 行政採購申請單 的「付款辦法」

            strJson = jsonFunction.ObjectToJSON(generalOrderContent.GENERAL_ORDER_PAYMENTS_CONFIG.Where(PYMT => PYMT.PERIOD == generalInvoiceConfig.PERIOD).Select(PYMT => PYMT));
            var generalInvoicePaymentsConfig = JsonConvert.DeserializeObject<List<GeneralInvoicePaymentsConfig>>(strJson);

            #endregion

            #region - 行政採購申請單 使用預算 -
            //View的「使用預算」是 行政採購申請單 的「使用預算」

            strJson = jsonFunction.ObjectToJSON(generalOrderContent.GENERAL_ORDER_BUDGETS_CONFIG.Where(BUDG => BUDG.PERIOD == generalInvoiceConfig.PERIOD).Select(BUDG => BUDG));
            var generalInvoiceBudgetsConfig = JsonConvert.DeserializeObject<List<GeneralInvoiceBudgetsConfig>>(strJson);

            #endregion

            parameter.Add(new SqlParameter("@PERIOD", SqlDbType.Int) { Value = generalInvoiceConfig.PERIOD });

            #region - 行政採購請款單 憑證明細 -

            var CommonINV = new BPMCommonModel<GeneralInvoiceInvoicesConfig>()
            {
                EXT = "INV",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceFunction(CommonINV));
            var generalInvoiceInvoicsConfig = jsonFunction.JsonToObject<List<GeneralInvoiceInvoicesConfig>>(strJson);

            #endregion

            #region - 行政採購請款單 憑證細項 -

            var CommonINV_DTL = new BPMCommonModel<GeneralInvoiceInvoiceDetailsConfig>()
            {
                EXT = "INV_DTL",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceDetailFunction(CommonINV_DTL));
            var generalInvoiceInvoiceDetailsConfig = jsonFunction.JsonToObject<List<GeneralInvoiceInvoiceDetailsConfig>>(strJson);

            #endregion

            #region - 行政採購請款單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var generalInvoiceViewModel = new GeneralInvoiceViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                GENERAL_INVOICE_TITLE = generalInvoiceTitle,
                GENERAL_INVOICE_CONFIG = generalInvoiceConfig,
                GENERAL_INVOICE_ACCEPTANCES_CONFIG = generalInvoiceAcceptancesConfig,
                GENERAL_INVOICE_PAYMENTS_CONFIG = generalInvoicePaymentsConfig,
                GENERAL_INVOICE_BUDGETS_CONFIG = generalInvoiceBudgetsConfig,
                GENERAL_INVOICE_INVS_CONFIG = generalInvoiceInvoicsConfig,
                GENERAL_INVOICE_INV_DTLS_CONFIG = generalInvoiceInvoiceDetailsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };            

            #region - 確認表單 -

            if (generalInvoiceViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                #region - 確認BPM表單是否正常起單到系統中 -

                //保留原有資料
                strJson = jsonFunction.ObjectToJSON(generalInvoiceViewModel);

                var BpmSystemOrder = new BPMSystemOrder()
                {
                    REQUISITION_ID = query.REQUISITION_ID,
                    IDENTIFY = IDENTIFY,
                    EXTS = new List<string>()
                    {
                        "M",
                        "INV",
                        "INV_DTL"
                    },
                };
                if (generalInvoiceViewModel.ASSOCIATED_FORM_CONFIG != null && generalInvoiceViewModel.ASSOCIATED_FORM_CONFIG.Count > 0) BpmSystemOrder.IS_ASSOCIATED_FORM = true;
                else BpmSystemOrder.IS_ASSOCIATED_FORM = false;
                //確認是否有正常到系統起單；清除失敗表單資料並重新送單值行
                if (commonRepository.PostBPMSystemOrder(BpmSystemOrder)) PutGeneralInvoiceSingle(jsonFunction.JsonToObject<GeneralInvoiceViewModel>(strJson));

                #endregion

                #region - 確認M表BPM表單單號 -

                //避免儲存後送出表單BPM表單單號沒寫入的情形
                var formQuery = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                notifyRepository.ByInsertBPMFormNo(formQuery);

                if (String.IsNullOrEmpty(generalInvoiceViewModel.GENERAL_INVOICE_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(generalInvoiceViewModel.GENERAL_INVOICE_TITLE.BPM_FORM_NO))
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                    var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                    if (dtBpmFormNo.Rows.Count > 0) generalInvoiceViewModel.GENERAL_INVOICE_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                }

                #endregion
            }

            #endregion

            return generalInvoiceViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 行政採購請款單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutGeneralInvoiceRefill(GeneralInvoiceQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("專案建立審核單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 行政採購請款單(新增/修改/草稿)
        /// </summary>
        public bool PutGeneralInvoiceSingle(GeneralInvoiceViewModel model)
        {
            bool vResult = false;
            try
            {
                var GeneralOrderformQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID
                };
                var GeneralOrderformData = formRepository.PostFormData(GeneralOrderformQueryModel);

                #region - 宣告 -

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                FM7Subject = model.GENERAL_INVOICE_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    FM7Subject = "【請款】第" + model.GENERAL_INVOICE_CONFIG.PERIOD + "期-" + GeneralOrderformData.FORM_SUBJECT;
                }

                #endregion

                #region - 預設設定 -

                if (String.IsNullOrEmpty(model.GENERAL_INVOICE_CONFIG.REIMBURSEMENT) || String.IsNullOrWhiteSpace(model.GENERAL_INVOICE_CONFIG.REIMBURSEMENT))
                {
                    //員工代墊
                    model.GENERAL_INVOICE_CONFIG.REIMBURSEMENT = "false";
                    //支付方式 
                    model.GENERAL_INVOICE_CONFIG.PAY_METHOD = "SUP_A/C";
                }

                #endregion

                #endregion

                #region - 行政採購請款單 表頭資訊：GeneralInvoice_M -

                var parameterTitle = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  strREQ},
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value =  model.APPLICANT_INFO.PRIORITY},
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
                    //行政採購請款單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
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
                    strSQL += "     [FlowName]=@FLOW_NAME, ";
                    strSQL += "     [FormNo]=@FORM_NO, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GeneralInvoice_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 行政採購請款單 表單內容：GeneralInvoice_M -

                if (model.GENERAL_INVOICE_CONFIG != null)
                {
                    #region - 【行政採購申請單】資訊 -

                    model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_BPM_FORM_NO = GeneralOrderformData.SERIAL_ID;
                    model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_SUBJECT = GeneralOrderformData.FORM_SUBJECT;
                    model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_PATH = GlobalParameters.FormContentPath(model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID, GeneralOrderformData.IDENTIFY, GeneralOrderformData.DIAGRAM_NAME);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //行政採購請款單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@GENERAL_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ACCEPTANCE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMBURSEMENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_TX_ID", SqlDbType.NVarChar) { Size = 1000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TX_CATEGORY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_SWIFT", SqlDbType.NVarChar) { Size = 300, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_ADDRESS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_COUNTRY_AND_CITY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_IBAN", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_EMAIL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INVOICE_TYPE", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_CURRENT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_CURRENT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACTUAL_PAY_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：行政採購請款單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.GENERAL_INVOICE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
                    strSQL += "SET [GeneralOrderRequisitionID]=@GENERAL_ORDER_REQUISITION_ID, ";
                    strSQL += "     [GeneralOrderBPMFormNo]=@GENERAL_ORDER_BPM_FORM_NO, ";
                    strSQL += "     [GeneralOrderERPFormNo]=@GENERAL_ORDER_ERP_FORM_NO, ";
                    strSQL += "     [GeneralOrderTXN_Type]=MAIN.[TXN_TYPE], ";
                    strSQL += "     [GeneralOrderSubject]=@GENERAL_ORDER_SUBJECT, ";
                    strSQL += "     [GeneralOrderPath]=@GENERAL_ORDER_PATH, ";
                    strSQL += "     [GeneralOrderPYMT_GrossTotal]=MAIN.[PYMT_GROSS_TOTAL], ";
                    strSQL += "     [GeneralOrderPYMT_GrossTotal_CONV]=MAIN.[PYMT_GROSS_TOTAL_CONV], ";
                    strSQL += "     [GeneralAcceptanceRequisitionID]=@GENERAL_ACCEPTANCE_REQUISITION_ID, ";
                    strSQL += "     [Currency]=MAIN.[CURRENCY], ";
                    strSQL += "     [PredictRate]=MAIN.[PRE_RATE], ";
                    strSQL += "     [PricingMethod]=MAIN.[PRICING_METHOD], ";
                    strSQL += "     [TaxRate]=MAIN.[TAX_RATE], ";
                    strSQL += "     [Period]=@PERIOD, ";
                    strSQL += "     [Reimbursement]=@REIMBURSEMENT, ";
                    strSQL += "     [REIMB_StaffDeptID]=@REIMB_STAFF_DEPT_ID, ";
                    strSQL += "     [REIMB_StaffDeptName]=@REIMB_STAFF_DEPT_NAME, ";
                    strSQL += "     [REIMB_StaffID]=@REIMB_STAFF_ID, ";
                    strSQL += "     [REIMB_StaffName]=@REIMB_STAFF_NAME, ";
                    strSQL += "     [PayMethod]=@PAY_METHOD, ";
                    strSQL += "     [SupNo]=MAIN.[SUP_NO], ";
                    strSQL += "     [SupName]=MAIN.[SUP_NAME], ";
                    strSQL += "     [RegisterKind]=MAIN.[REG_KIND], ";
                    strSQL += "     [RegisterNo]=MAIN.[REG_NO], ";
                    strSQL += "     [SupTXId]=@SUP_TX_ID, ";
                    strSQL += "     [TX_Category]=@TX_CATEGORY, ";
                    strSQL += "     [BFCY_AccountNo]=@BFCY_ACCOUNT_NO, ";
                    strSQL += "     [BFCY_AccountName]=@BFCY_ACCOUNT_NAME, ";
                    strSQL += "     [BFCY_BankNo]=@BFCY_BANK_NO, ";
                    strSQL += "     [BFCY_BankName]=@BFCY_BANK_NAME, ";
                    strSQL += "     [BFCY_BanKBranchNo]=@BFCY_BANK_BRANCH_NO, ";
                    strSQL += "     [BFCY_BanKBranchName]=@BFCY_BANK_BRANCH_NAME, ";
                    strSQL += "     [BFCY_BankSWIFT]=@BFCY_BANK_SWIFT, ";
                    strSQL += "     [BFCY_BankAddress]=@BFCY_BANK_ADDRESS, ";
                    strSQL += "     [BFCY_BankCountryAndCity]=@BFCY_BANK_COUNTRY_AND_CITY, ";
                    strSQL += "     [BFCY_BankIBAN]=@BFCY_BANK_IBAN, ";
                    strSQL += "     [CurrencyName]=@CURRENCY_NAME, ";
                    strSQL += "     [BFCY_Name]=@BFCY_NAME, ";
                    strSQL += "     [BFCY_TEL]=@BFCY_TEL, ";
                    strSQL += "     [BFCY_Email]=@BFCY_EMAIL, ";
                    strSQL += "     [InvoiceType]=@INVOICE_TYPE, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [PYMT_CurrentTotal]=@PYMT_CURRENT_TOTAL, ";
                    strSQL += "     [PYMT_CurrentTotal_TWD]=@PYMT_CURRENT_TOTAL_TWD, ";
                    strSQL += "     [INV_TaxTotal]=@INV_TAX_TOTAL, ";
                    strSQL += "     [INV_TaxTotal_TWD]=@INV_TAX_TOTAL_TWD, ";
                    strSQL += "     [INV_AmountTotal]=@INV_AMOUNT_TOTAL, ";
                    strSQL += "     [INV_AmountTotal_TWD]=@INV_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [ActualPayAmount]=@ACTUAL_PAY_AMOUNT, ";
                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2 ";
                    strSQL += "     FROM ( ";
                    strSQL += "             SELECT ";
                    strSQL += "                 [TXN_Type] AS [TXN_TYPE], ";
                    strSQL += "                 [Currency] AS [CURRENCY], ";
                    strSQL += "                 [PredictRate] AS [PRE_RATE], ";
                    strSQL += "                 [PricingMethod] AS [PRICING_METHOD], ";
                    strSQL += "                 [TaxRate] AS [TAX_RATE], ";
                    strSQL += "                 [PYMT_GrossTotal] AS [PYMT_GROSS_TOTAL], ";
                    strSQL += "                 [PYMT_GrossTotal_CONV] AS [PYMT_GROSS_TOTAL_CONV], ";
                    strSQL += "                 [SupNo] AS [SUP_NO], ";
                    strSQL += "                 [SupName] AS [SUP_NAME], ";
                    strSQL += "                 [RegisterKind] AS [REG_KIND], ";
                    strSQL += "                 [RegisterNo] AS [REG_NO] ";
                    strSQL += "             FROM [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
                    strSQL += "             WHERE [RequisitionID] = @GENERAL_ORDER_REQUISITION_ID ";
                    strSQL += "     ) AS MAIN ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);
                }

                #endregion

                #region - 行政採購請款單 驗收明細: GeneralOrder_ACPT -

                //View 是執行
                //行政採購的 驗收項目(GeneralOrder_ACPT)及 採購項目(GeneralOrder_DTL) 內容。

                #endregion

                #region - 行政採購請款單 付款辦法: GeneralOrder_PYMT -

                //View 是執行
                //行政採購的 付款辦法(GeneralOrder_PYMT)
                //只有在「財務部簽核」會需要更新 會計類別(ACCT_Category) 欄位



                var parameterPayments = new List<SqlParameter>()
                {
                    //行政採購申請 付款辦法 更新:會計類別
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACCT_CATEGORY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                if (model.GENERAL_INVOICE_PAYMENTS_CONFIG != null && model.GENERAL_INVOICE_PAYMENTS_CONFIG.Count > 0)
                {
                    #region 修改資料

                    foreach (var item in model.GENERAL_INVOICE_PAYMENTS_CONFIG)
                    {
                        //寫入：行政採購申請 付款辦法 更新:會計類別parameter

                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterPayments);

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralOrder_PYMT] ";
                        strSQL += "SET [ACCT_Category]=@ACCT_CATEGORY ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                        strSQL += "         AND [Period]=@PERIOD ";

                        dbFun.DoTran(strSQL, parameterPayments);
                    }

                    #endregion
                }

                #endregion

                #region - 行政採購請款單 使用預算: GeneralOrder_BUDG -

                //View 是執行
                //行政採購的 使用預算(GeneralOrder_BUDG) 內容。

                #endregion

                #region - 行政採購請款單 憑證明細：GeneralInvoice_INV -

                if (model.GENERAL_INVOICE_INVS_CONFIG != null && model.GENERAL_INVOICE_INVS_CONFIG.Count > 0)
                {
                    var parameterInvoices = new List<SqlParameter>()
                    {
                        //行政採購請款單 憑證明細
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@GENERAL_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_BPM_FORM_NO ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_ERP_FORM_NO ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXCL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXCL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    var CommonINV = new BPMCommonModel<GeneralInvoiceInvoicesConfig>()
                    {
                        EXT = "INV",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterInvoices,
                        MODEL = model.GENERAL_INVOICE_INVS_CONFIG
                    };
                    commonRepository.PutInvoiceFunction(CommonINV);
                }

                #endregion

                #region - 行政採購請款單 憑證細項：GeneralInvoice_INV_DTL -

                if (model.GENERAL_INVOICE_INV_DTLS_CONFIG != null && model.GENERAL_INVOICE_INV_DTLS_CONFIG.Count > 0)
                {
                    var parameterInvoiceDetails = new List<SqlParameter>()
                    {
                        //行政採購請款單 憑證明細
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@GENERAL_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_BPM_FORM_NO ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_ERP_FORM_NO ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = model.GENERAL_INVOICE_CONFIG.PERIOD },
                        new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    var CommonINV_DTL = new BPMCommonModel<GeneralInvoiceInvoiceDetailsConfig>()
                    {
                        EXT="INV_DTL",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterInvoiceDetails,
                        MODEL = model.GENERAL_INVOICE_INV_DTLS_CONFIG
                    };
                    commonRepository.PutInvoiceDetailFunction(CommonINV_DTL);
                }

                #endregion

                #region - 行政採購請款單 表單關聯：AssociatedForm -

                //關聯表:匯入【行政採購申請單】的「關聯表單」
                var importAssociatedForm = commonRepository.PostAssociatedForm(GeneralOrderformQueryModel);

                #region 關聯:【行政採購申請單】

                importAssociatedForm.Add(new AssociatedFormConfig()
                {
                    IDENTIFY = GeneralOrderformData.IDENTIFY,
                    ASSOCIATED_REQUISITION_ID = model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID,
                    BPM_FORM_NO = GeneralOrderformData.SERIAL_ID,
                    FM7_SUBJECT = GeneralOrderformData.FORM_SUBJECT,
                    APPLICANT_DEPT_NAME = GeneralOrderformData.APPLICANT_DEPT_NAME,
                    APPLICANT_NAME = GeneralOrderformData.APPLICANT_NAME,
                    APPLICANT_DATE_TIME = GeneralOrderformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                    FORM_PATH = GlobalParameters.FormContentPath(model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID, GeneralOrderformData.IDENTIFY, GeneralOrderformData.DIAGRAM_NAME),
                    STATE = BPMStatusCode.CLOSE
                });

                #endregion                

                var associatedFormConfig = model.ASSOCIATED_FORM_CONFIG;
                if (associatedFormConfig == null || associatedFormConfig.Count <= 0)
                {
                    associatedFormConfig = importAssociatedForm;
                }

                if (!String.IsNullOrEmpty(model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID))
                {
                    #region 關聯:【行政採購點驗收單】

                    if (!associatedFormConfig.Where(AF => AF.ASSOCIATED_REQUISITION_ID.Contains(model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID)).Any())
                    {
                        var GeneralAcceptanceformQueryModel = new FormQueryModel()
                        {
                            REQUISITION_ID = model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID
                        };
                        var GeneralAcceptanceformData = formRepository.PostFormData(GeneralAcceptanceformQueryModel);

                        associatedFormConfig.Add(new AssociatedFormConfig()
                        {
                            IDENTIFY = GeneralAcceptanceformData.IDENTIFY,
                            ASSOCIATED_REQUISITION_ID = model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID,
                            BPM_FORM_NO = GeneralAcceptanceformData.SERIAL_ID,
                            FM7_SUBJECT = GeneralAcceptanceformData.FORM_SUBJECT,
                            APPLICANT_DEPT_NAME = GeneralAcceptanceformData.APPLICANT_DEPT_NAME,
                            APPLICANT_NAME = GeneralAcceptanceformData.APPLICANT_NAME,
                            APPLICANT_DATE_TIME = GeneralAcceptanceformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                            FORM_PATH = GlobalParameters.FormContentPath(model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID, GeneralAcceptanceformData.IDENTIFY, GeneralAcceptanceformData.DIAGRAM_NAME),
                            STATE = BPMStatusCode.CLOSE
                        });
                    }

                    #endregion
                }

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = strREQ,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfig
                };

                //寫入「關聯表單」
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
                CommLib.Logger.Error("行政採購請款單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "GeneralInvoice";

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