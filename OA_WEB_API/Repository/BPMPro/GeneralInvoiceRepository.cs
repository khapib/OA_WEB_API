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
    /// 會簽管理系統 - 行政採購請款單
    /// </summary>
    public class GeneralInvoiceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

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
            strSQL += "     [GeneralOrderPath] AS [GENERAL_ORDER_PATH], ";
            strSQL += "     [GeneralOrderDTL_OrderTotal_TWD] AS [GENERAL_ORDER_DTL_ORDER_TOTAL_TWD], ";
            strSQL += "     [GeneralAcceptanceRequisitionID] AS [GENERAL_ACCEPTANCE_REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [PredictRate] AS [PRE_RATE], ";
            strSQL += "     [PricingMethod] AS [PRICING_METHOD], ";
            strSQL += "     [Tax] AS [TAX], ";
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
            strSQL += "     [INV_AmountTotal] AS [INV_AMOUNT_TOTAL], ";
            strSQL += "     [ActualPayAmount] AS [ACTUAL_PAY_AMOUNT] ";
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
            strSQL += "     DTL.[DTL_ProjectFormNo] AS [DTL_PROJECT_FORM_NO], ";
            strSQL += "     DTL.[DTL_ProjectName] AS [DTL_PROJECT_NAME], ";
            strSQL += "     DTL.[DTL_ProjectNickname] AS [DTL_PROJECT_NICKNAME], ";
            strSQL += "     DTL.[DTL_ProjectUseYear] AS [DTL_PROJECT_USE_YEAR], ";
            strSQL += "     DTL.[DTL_Note] AS [DTL_NOTE], ";
            strSQL += "     ACPT.[Period] AS [PERIOD], ";
            strSQL += "     ACPT.[PA_SupProdANo] AS [PA_SUP_PROD_A_NO], ";
            strSQL += "     ACPT.[PA_ItemName] AS [PA_ITEM_NAME], ";
            strSQL += "     ACPT.[PA_Model] AS [PA_MODEL], ";
            strSQL += "     ACPT.[PA_Specifications] AS [PA_SPECIFICATIONS], ";
            strSQL += "     ACPT.[PA_Quantity] AS [PA_QUANTITY], ";
            strSQL += "     ACPT.[PA_Unit] AS [PA_UNIT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_ACPT] AS ACPT ";
            strSQL += "	    INNER JOIN [BPMPro].[dbo].[FM7T_GeneralOrder_DTL] AS DTL ON ACPT.[RequisitionID]=DTL.[RequisitionID] AND ACPT.[PA_SupProdANo]=DTL.[DTL_SupProdANo] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND ACPT.[RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND ACPT.[Period]=@PERIOD ";

            var generalInvoiceAcceptancesConfig = dbFun.DoQuery(strSQL, generalOrderparameter).ToList<GeneralInvoiceAcceptancesConfig>();

            #endregion

            #region - 行政採購請款單 付款辦法 -

            //View的「付款辦法」是 行政採購申請單 的「付款辦法」

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [PYMT_Project] AS [PYMT_PROJECT], ";
            strSQL += "     [PYMT_Terms] AS [PYMT_TERMS], ";
            strSQL += "     [PYMT_MethodID] AS [PYMT_METHOD_ID], ";
            strSQL += "     [PYMT_Tax] AS [PYMT_TAX], ";
            strSQL += "     [PYMT_Net] AS [PYMT_NET], ";
            strSQL += "     [PYMT_Gross] AS [PYMT_GROSS], ";
            strSQL += "     [PYMT_PredictRate] AS [PYMT_PRE_RATE], ";
            strSQL += "     [PYMT_Gross_CONV] AS [PYMT_GROSS_CONV], ";
            strSQL += "     [PYMT_UseBudget] AS [PYMT_USE_BUDGET], ";
            strSQL += "     [ACCT_Category] AS [ACCT_CATEGORY] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_PYMT] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND [Period]=@PERIOD ";
            strSQL += "ORDER BY [AutoCounter] ";

            var generalInvoicePaymentsConfig = dbFun.DoQuery(strSQL, generalOrderparameter).ToList<GeneralInvoicePaymentsConfig>();

            #endregion

            #region - 行政採購申請單 使用預算 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [BUDG_FormNo] AS [BUDG_FORM_NO], ";
            strSQL += "     [BUDG_CreateYear] AS [BUDG_CREATE_YEAR], ";
            strSQL += "     [BUDG_Name] AS [BUDG_NAME], ";
            strSQL += "     [BUDG_OwnerDept] AS [BUDG_OWNER_DEPT], ";
            strSQL += "     [BUDG_Total] AS [BUDG_TOTAL], ";
            strSQL += "     [BUDG_AvailableBudgetAmount] AS [BUDG_AVAILABLE_BUDGET_AMOUNT], ";
            strSQL += "     [BUDG_UseBudgetAmount] AS [BUDG_USE_BUDGET_AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_BUDG] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND [Period]=@PERIOD ";
            strSQL += "ORDER BY [AutoCounter] ";

            var generalInvoiceBudgetsConfig = dbFun.DoQuery(strSQL, generalOrderparameter).ToList<GeneralInvoiceBudgetsConfig>();

            #endregion

            #region - 行政採購請款單 發票明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [INV_Num] AS [INV_NUM], ";
            strSQL += "     [INV_Date] AS [INV_DATE], ";
            strSQL += "     [INV_Amount] AS [INV_AMOUNT], ";
            strSQL += "     [INV_Note] AS [INV_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_INV] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var generalInvoiceDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<GeneralInvoiceDetailsConfig>();

            #endregion

            #region - 行政採購請款單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var GeneralInvoiceViewModel = new GeneralInvoiceViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                GENERAL_INVOICE_TITLE = generalInvoiceTitle,
                GENERAL_INVOICE_CONFIG = generalInvoiceConfig,
                GENERAL_INVOICE_ACCEPTANCES_CONFIG = generalInvoiceAcceptancesConfig,
                GENERAL_INVOICE_PAYMENTS_CONFIG = generalInvoicePaymentsConfig,
                GENERAL_INVOICE_BUDGETS_CONFIG= generalInvoiceBudgetsConfig,
                GENERAL_INVOICE_DETAILS_CONFIG = generalInvoiceDetailsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            return GeneralInvoiceViewModel;
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
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  model.APPLICANT_INFO.REQUISITION_ID},
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
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //行政採購申請 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

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
                    strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";
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
                    model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_BPM_FORM_NO = GeneralOrderformData.SERIAL_ID;
                    model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_SUBJECT = GeneralOrderformData.FORM_SUBJECT;
                    model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_PATH = GlobalParameters.FormContentPath(model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID, GeneralOrderformData.IDENTIFY, GeneralOrderformData.DIAGRAM_NAME);

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //行政採購請款單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                        new SqlParameter("@GENERAL_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ORDER_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GENERAL_ACCEPTANCE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@PRE_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@PRICING_METHOD", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@TAX", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMBURSEMENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
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
                        //new SqlParameter("@PYMT_CURRENT_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        //new SqlParameter("@INV_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACTUAL_PAY_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：行政採購請款單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.GENERAL_INVOICE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
                    strSQL += "SET [GeneralOrderRequisitionID]=@GENERAL_ORDER_REQUISITION_ID, ";
                    strSQL += "     [GeneralOrderBPMFormNo]=@GENERAL_ORDER_BPM_FORM_NO, ";
                    strSQL += "     [GeneralOrderERPFormNo]=@GENERAL_ORDER_ERP_FORM_NO, ";
                    strSQL += "     [GeneralOrderSubject]=@GENERAL_ORDER_SUBJECT, ";
                    strSQL += "     [GeneralOrderPath]=@GENERAL_ORDER_PATH, ";
                    strSQL += "     [GeneralOrderDTL_OrderTotal_TWD]=MAIN.[DTL_ORDER_TOTAL_TWD], ";
                    strSQL += "     [GeneralAcceptanceRequisitionID]=@GENERAL_ACCEPTANCE_REQUISITION_ID, ";
                    strSQL += "     [Currency]=MAIN.[CURRENCY], ";
                    strSQL += "     [PredictRate]=MAIN.[PRE_RATE], ";
                    strSQL += "     [PricingMethod]=MAIN.[PRICING_METHOD], ";
                    strSQL += "     [Tax]=MAIN.[TAX], ";
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
                    strSQL += "     [ActualPayAmount]=@ACTUAL_PAY_AMOUNT ";
                    strSQL += "     FROM ( ";
                    strSQL += "             SELECT ";
                    strSQL += "                 [Currency] AS [CURRENCY], ";
                    strSQL += "                 [PredictRate] AS [PRE_RATE], ";
                    strSQL += "                 [PricingMethod] AS [PRICING_METHOD], ";
                    strSQL += "                 [Tax] AS [TAX], ";
                    strSQL += "                 [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
                    strSQL += "                 [SupNo] AS [SUP_NO], ";
                    strSQL += "                 [SupName] AS [SUP_NAME], ";
                    strSQL += "                 [RegisterKind] AS [REG_KIND], ";
                    strSQL += "                 [RegisterNo] AS [REG_NO] ";
                    strSQL += "             FROM [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
                    strSQL += "             WHERE [RequisitionID] = @GENERAL_ORDER_REQUISITION_ID ";
                    strSQL += "     ) AS MAIN ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
                    strSQL += "SET [PYMT_CurrentTotal]=MAIN.[PYMT_CURRENT_TOTAL] ";
                    strSQL += "     FROM ( ";
                    strSQL += "             SELECT ";
                    strSQL += "                 SUM([PYMT_Gross_CONV]) AS [PYMT_CURRENT_TOTAL] ";
                    strSQL += "             FROM [BPMPro].[dbo].[FM7T_GeneralOrder_PYMT] ";
                    strSQL += "             WHERE [RequisitionID] = @GENERAL_ORDER_REQUISITION_ID ";
                    strSQL += "               AND [Period]=@PERIOD ";
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

                #region - 行政採購請款單 發票明細：GeneralInvoice_INV -

                var parameterDetails = new List<SqlParameter>()
                {
                    //行政採購請款單 發票明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@GENERAL_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@GENERAL_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_ERP_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@GENERAL_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)GeneralOrderformData.SERIAL_ID ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NUM", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_DATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_INV] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.GENERAL_INVOICE_DETAILS_CONFIG != null && model.GENERAL_INVOICE_DETAILS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.GENERAL_INVOICE_DETAILS_CONFIG)
                    {
                        //寫入：行政採購申請 發票明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GeneralInvoice_INV]([RequisitionID],[Period],[GeneralOrderRequisitionID],[GeneralOrderBPMFormNo],[GeneralOrderERPFormNo],[INV_Num],[INV_Date],[INV_Amount],[INV_Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@GENERAL_ORDER_REQUISITION_ID,@GENERAL_ORDER_BPM_FORM_NO,@GENERAL_ORDER_ERP_FORM_NO,@INV_NUM,@INV_DATE,@INV_AMOUNT,@INV_NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion

                    #region -  -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralInvoice_M] ";
                    strSQL += "SET [INV_AmountTotal]=MAIN.[INV_AMOUNT_TOTAL] ";
                    strSQL += "     FROM ( ";
                    strSQL += "             SELECT ";
                    strSQL += "                 SUM([INV_AMOUNT]) AS [INV_AMOUNT_TOTAL] ";
                    strSQL += "             FROM [BPMPro].[dbo].[FM7T_GeneralInvoice_INV] ";
                    strSQL += "             WHERE [RequisitionID] = @REQUISITION_ID ";
                    strSQL += "               AND [Period]=@PERIOD ";
                    strSQL += "     ) AS MAIN ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterDetails);

                    #endregion
                }

                #endregion

                #region - 行政採購請款單 表單關聯：AssociatedForm -

                //關聯表:匯入【行政採購申請單】的「關聯表單」
                var importAssociatedForm = commonRepository.PostAssociatedForm(GeneralOrderformQueryModel);

                #region 關聯表:加上【行政採購申請單】

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

                #region 關聯表:加上【行政採購點驗收單】

                if(!String.IsNullOrEmpty(model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID))
                {
                    var GeneralAcceptanceformQueryModel = new FormQueryModel()
                    {
                        REQUISITION_ID = model.GENERAL_INVOICE_CONFIG.GENERAL_ACCEPTANCE_REQUISITION_ID
                    };
                    var GeneralAcceptanceformData = formRepository.PostFormData(GeneralAcceptanceformQueryModel);

                    importAssociatedForm.Add(new AssociatedFormConfig()
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

                var associatedFormConfig = model.ASSOCIATED_FORM_CONFIG;
                if (associatedFormConfig == null || associatedFormConfig.Count <= 0)                
                {
                    associatedFormConfig = importAssociatedForm;
                }

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfig
                };

                //寫入「關聯表單」
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
                CommLib.Logger.Error("點驗收單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "GeneralInvoice";

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