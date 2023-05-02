using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.SqlServer.Server;
using Microsoft.Ajax.Utilities;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購請款單
    /// </summary>
    public class MediaInvoiceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        FlowRepository flowRepository = new FlowRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購請款單(查詢)
        /// </summary>
        public MediaInvoiceViewModel PostMediaInvoiceSingle(MediaInvoiceQueryModel query)
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
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

            #region - 版權採購請款單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaInvoiceTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceTitle>().FirstOrDefault();

            #endregion

            #region - 版權採購請款單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [MediaOrderRequisitionID] AS [MEDIA_ORDER_REQUISITION_ID], ";
            strSQL += "     [MediaOrderSubject] AS [MEDIA_ORDER_SUBJECT], ";
            strSQL += "     [MediaOrderBPMFormNo] AS [MEDIA_ORDER_BPM_FORM_NO], ";
            strSQL += "     [MediaOrderERPFormNo] AS [MEDIA_ORDER_ERP_FORM_NO], ";
            strSQL += "     [MediaOrderPath] AS [MEDIA_ORDER_PATH], ";
            strSQL += "     [MediaOrderTXN_Type] AS [MEDIA_ORDER_TXN_TYPE], ";
            strSQL += "     [MediaOrderDTL_OrderTotal] AS [MEDIA_ORDER_DTL_ORDER_TOTAL], ";
            strSQL += "     [MediaOrderDTL_OrderTotal_TWD] AS [MEDIA_ORDER_DTL_ORDER_TOTAL_TWD], ";
            strSQL += "     [MediaAcceptanceRequisitionID] AS [MEDIA_ACCEPTANCE_REQUISITION_ID], ";
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
            strSQL += "     [SupNo] AS [SUP_NO], ";
            strSQL += "     [SupName] AS [SUP_NAME], ";
            strSQL += "     [PayMethod] AS [PAY_METHOD], ";
            strSQL += "     [RegisterKind] AS [REG_KIND], ";
            strSQL += "     [RegisterNo] AS [REG_NO], ";
            strSQL += "     [SupTXId] AS [SUP_TX_ID], ";
            strSQL += "     [InvoiceType] AS [INVOICE_TYPE], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [DTL_NetTotal] AS [DTL_NET_TOTAL], ";
            strSQL += "     [DTL_NetTotal_TWD] AS [DTL_NET_TOTAL_TWD], ";
            strSQL += "     [DTL_TaxTotal] AS [DTL_TAX_TOTAL], ";
            strSQL += "     [DTL_TaxTotal_TWD] AS [DTL_TAX_TOTAL_TWD], ";
            strSQL += "     [DTL_GrossTotal] AS [DTL_GROSS_TOTAL], ";
            strSQL += "     [DTL_GrossTotal_TWD] AS [DTL_GROSS_TOTAL_TWD], ";
            strSQL += "     [DTL_MaterialTotal] AS [DTL_MATERIAL_TOTAL], ";
            strSQL += "     [DTL_MaterialTotal_TWD] AS [DTL_MATERIAL_TOTAL_TWD], ";
            strSQL += "     [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
            strSQL += "     [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
            strSQL += "     [EX_AmountTotal] AS [EX_AMOUNT_TOTAL], ";
            strSQL += "     [EX_AmountTotal_TWD] AS [EX_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [EX_TaxTotal] AS [EX_TAX_TOTAL], ";
            strSQL += "     [EX_TaxTotal_TWD] AS [EX_TAX_TOTAL_TWD], ";
            strSQL += "     [PYMT_CurrentTotal] AS [PYMT_CURRENT_TOTAL], ";
            strSQL += "     [PYMT_CurrentTotal_TWD] AS [PYMT_CURRENT_TOTAL_TWD], ";
            strSQL += "     [PYMT_EX_TaxTotal] AS [PYMT_EX_TAX_TOTAL], ";
            strSQL += "     [PYMT_EX_TaxTotal_TWD] AS [PYMT_EX_TAX_TOTAL_TWD], ";
            strSQL += "     [INV_AmountTotal] AS [INV_AMOUNT_TOTAL], ";
            strSQL += "     [INV_AmountTotal_TWD] AS [INV_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [INV_TaxTotal] AS [INV_TAX_TOTAL], ";
            strSQL += "     [INV_TaxTotal_TWD] AS [INV_TAX_TOTAL_TWD], ";
            strSQL += "     [ActualPayAmount] AS [ACTUAL_PAY_AMOUNT], ";
            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2], ";
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
            strSQL += "     [BFCY_Email] AS [BFCY_EMAIL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaInvoiceConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceConfig>().FirstOrDefault();

            #endregion

            var mediaOrderparameter = new List<SqlParameter>()
            {
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = mediaInvoiceConfig.MEDIA_ORDER_REQUISITION_ID },
                new SqlParameter("@PERIOD", SqlDbType.Int) { Value = mediaInvoiceConfig.PERIOD }
            };

            #region - 版權採購請款單 驗收明細 -

            //View的「驗收明細」是 版權採購申請單 的「驗收明細」加上 「採購明細」的所屬專案、金額及備註。

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     DTL.[DTL_RowNo] AS [DTL_ROW_NO], ";
            strSQL += "     DTL.[DTL_EpisodeTime] AS [DTL_EPISODE_TIME], ";
            strSQL += "     DTL.[DTL_MediaSpec] AS [DTL_MEDIA_SPEC], ";
            strSQL += "     DTL.[DTL_Net] AS [DTL_NET], ";
            strSQL += "     DTL.[DTL_Net_TWD] AS [DTL_NET_TWD], ";
            strSQL += "     DTL.[DTL_Tax] AS [DTL_TAX], ";
            strSQL += "     DTL.[DTL_Tax_TWD] AS [DTL_TAX_TWD], ";
            strSQL += "     DTL.[DTL_Gross] AS [DTL_GROSS], ";
            strSQL += "     DTL.[DTL_Gross_TWD] AS [DTL_GROSS_TWD], ";
            strSQL += "     DTL.[DTL_NetSum] AS [DTL_NET_SUM], ";
            strSQL += "     DTL.[DTL_NetSum_TWD] AS [DTL_NET_SUM_TWD], ";
            strSQL += "     DTL.[DTL_GrossSum] AS [DTL_GROSS_SUM], ";
            strSQL += "     DTL.[DTL_GrossSum_TWD] AS [DTL_GROSS_SUM_TWD], ";
            strSQL += "     DTL.[DTL_Material] AS [DTL_MATERIAL], ";
            strSQL += "     DTL.[DTL_ItemSum] AS [DTL_ITEM_SUM], ";
            strSQL += "     DTL.[DTL_ItemSum_TWD] AS [DTL_ITEM_SUM_TWD], ";
            strSQL += "     DTL.[DTL_ProjectFormNo] AS [DTL_PROJECT_FORM_NO], ";
            strSQL += "     DTL.[DTL_ProjectName] AS [DTL_PROJECT_NAME], ";
            strSQL += "     DTL.[DTL_ProjectNickname] AS [DTL_PROJECT_NICKNAME], ";
            strSQL += "     DTL.[DTL_ProjectUseYear] AS [DTL_PROJECT_USE_YEAR], ";
            strSQL += "     DTL.[DTL_Note] AS [DTL_NOTE], ";
            strSQL += "     ACPT.[PA_RowNo] AS [PA_ROW_NO], ";
            strSQL += "     ACPT.[Period] AS [PERIOD], ";
            strSQL += "     ACPT.[PA_SupProdANo] AS [PA_SUP_PROD_A_NO], ";
            strSQL += "     ACPT.[PA_ItemName] AS [PA_ITEM_NAME], ";
            strSQL += "     ACPT.[PA_MediaType] AS [PA_MEDIA_TYPE], ";
            strSQL += "     ACPT.[PA_StartEpisode] AS [PA_START_EPISODE], ";
            strSQL += "     ACPT.[PA_EndEpisode] AS [PA_END_EPISODE], ";
            strSQL += "     ACPT.[PA_ACPT_Episode] AS [PA_ACPT_EPISODE], ";
            strSQL += "     ACPT.[PA_OrderEpisode] AS [PA_ORDER_EPISODE], ";
            strSQL += "     ACPT.[PA_Note] AS [PA_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] AS ACPT ";
            strSQL += "	    INNER JOIN [BPMPro].[dbo].[FM7T_MediaOrder_DTL] AS DTL ON ACPT.[RequisitionID]=DTL.[RequisitionID] AND ACPT.[PA_SupProdANo]=DTL.[DTL_SupProdANo] AND ACPT.[PA_RowNo]=DTL.[DTL_RowNo] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND ACPT.[RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND ACPT.[Period]=@PERIOD ";

            var mediaInvoiceAcceptancesConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoiceAcceptancesConfig>();

            #endregion

            #region - 版權採購請款單 授權權利 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     AUTH.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [AUTH_RowNo] AS [AUTH_ROW_NO], ";
            strSQL += "     [AUTH_SupProdANo] AS [AUTH_SUP_PROD_A_NO], ";
            strSQL += "     [AUTH_ItemName] AS [AUTH_ITEM_NAME], ";
            strSQL += "     [AUTH_Continent] AS [AUTH_CONTINENT], ";
            strSQL += "     [AUTH_Country] AS [AUTH_COUNTRY], ";
            strSQL += "     [AUTH_PlayPlatform] AS [AUTH_PLAY_PLATFORM],";
            strSQL += "     [AUTH_Play] AS [AUTH_PLAY], ";
            strSQL += "     [AUTH_Sell] AS [AUTH_SELL], ";
            strSQL += "     [AUTH_EditToPlay] AS [AUTH_EDIT_TO_PLAY], ";
            strSQL += "     [AUTH_EditToSell] AS [AUTH_EDIT_TO_SELL], ";
            strSQL += "     [AUTH_AllotedTimeType] AS [AUTH_ALLOTED_TIME_TYPE], ";
            strSQL += "     [AUTH_StartDate] AS [AUTH_START_DATE], ";
            strSQL += "     [AUTH_EndDate] AS [AUTH_END_DATE], ";
            strSQL += "     [AUTH_FrequencyType] AS [AUTH_FREQUENCY_TYPE], ";
            strSQL += "     [AUTH_PlayFrequency] AS [AUTH_PLAY_FREQUENCY], ";
            strSQL += "     [AUTH_Note] AS [AUTH_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_AUTH] AS AUTH ";
            strSQL += "     INNER JOIN [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] AS ACPT ON AUTH.[RequisitionID]=ACPT.[RequisitionID] AND AUTH.[AUTH_RowNo]=ACPT.[PA_RowNo] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND AUTH.[RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND ACPT.[Period]=@PERIOD ";
            strSQL += "ORDER BY AUTH.[AutoCounter] ";

            var mediaInvoiceAuthorizesConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoiceAuthorizesConfig>();

            #endregion

            #region - 版權採購申請單 額外項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [EX_RowNo] AS [EX_ROW_NO], ";
            strSQL += "     [EX_Name] AS [EX_NAME], ";
            strSQL += "     [EX_Amount] AS [EX_AMOUNT], ";
            strSQL += "     [EX_Amount_TWD] AS [EX_AMOUNT_TWD], ";
            strSQL += "     [EX_Tax] AS [EX_TAX], ";
            strSQL += "     [EX_Tax_TWD] AS [EX_TAX_TWD], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [EX_ProjectFormNo] AS [EX_PROJECT_FORM_NO], ";
            strSQL += "     [EX_ProjectName] AS [EX_PROJECT_NAME], ";
            strSQL += "     [EX_ProjectNickname] AS [EX_PROJECT_NICKNAME], ";
            strSQL += "     [EX_ProjectUseYear] AS [EX_PROJECT_USE_YEAR], ";
            strSQL += "     [EX_Note] AS [EX_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_EX] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoiceExtrasConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoiceExtrasConfig>();

            #endregion

            #region - 版權採購請款單 付款辦法 -

            //View的「付款辦法」是 版權採購申請單 的「付款辦法」

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [PYMT_RowNo] AS [PYMT_ROW_NO], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [PYMT_Project] AS [PYMT_PROJECT], ";
            strSQL += "     [PYMT_Terms] AS [PYMT_TERMS], ";
            strSQL += "     [PYMT_MethodID] AS [PYMT_METHOD_ID], ";
            strSQL += "     [PYMT_Tax] AS [PYMT_TAX], ";
            strSQL += "     [PYMT_Net] AS [PYMT_NET], ";
            strSQL += "     [PYMT_Gross] AS [PYMT_GROSS], ";
            strSQL += "     [PYMT_PredictRate] AS [PYMT_PRE_RATE], ";
            strSQL += "     [PYMT_Material] AS [PYMT_MATERIAL], ";
            strSQL += "     [PYMT_EX_Amount] AS [PYMT_EX_AMOUNT], ";
            strSQL += "     [PYMT_EX_Tax] AS [PYMT_EX_TAX], ";
            strSQL += "     [PYMT_OrderSum] AS [PYMT_ORDER_SUM], ";
            strSQL += "     [PYMT_OrderSum_CONV] AS [PYMT_ORDER_SUM_CONV], ";
            strSQL += "     [PYMT_UseBudget] AS [PYMT_USE_BUDGET], ";
            strSQL += "     [ACCT_Category] AS [ACCT_CATEGORY] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_PYMT] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND [Period]=@PERIOD ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoicePaymentsConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoicePaymentsConfig>();

            #endregion

            #region - 版權採購請款單 使用預算 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [BUDG_RowNo] AS [BUDG_ROW_NO], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [BUDG_FormNo] AS [BUDG_FORM_NO], ";
            strSQL += "     [BUDG_CreateYear] AS [BUDG_CREATE_YEAR], ";
            strSQL += "     [BUDG_Name] AS [BUDG_NAME], ";
            strSQL += "     [BUDG_OwnerDept] AS [BUDG_OWNER_DEPT], ";
            strSQL += "     [BUDG_Total] AS [BUDG_TOTAL], ";
            strSQL += "     [BUDG_AvailableBudgetAmount] AS [BUDG_AVAILABLE_BUDGET_AMOUNT], ";
            strSQL += "     [BUDG_UseBudgetAmount] AS [BUDG_USE_BUDGET_AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_BUDG] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoiceBudgetsConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoiceBudgetsConfig>();

            #endregion

            #region - 版權採購請款單 憑證明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [INV_Num] AS [INV_NUM], ";
            strSQL += "     [INV_Date] AS [INV_DATE], ";
            strSQL += "     [INV_Excl] AS [INV_EXCL], ";
            strSQL += "     [INV_Excl_TWD] AS [INV_EXCL_TWD], ";
            strSQL += "     [INV_Tax] AS [INV_TAX], ";
            strSQL += "     [INV_Tax_TWD] AS [INV_TAX_TWD], ";
            strSQL += "     [INV_Net] AS [INV_NET], ";
            strSQL += "     [INV_Net_TWD] AS [INV_NET_TWD], ";
            strSQL += "     [INV_Gross] AS [INV_GROSS], ";
            strSQL += "     [INV_Gross_TWD] AS [INV_GROSS_TWD], ";
            strSQL += "     [INV_Amount] AS [INV_AMOUNT], ";
            strSQL += "     [INV_Amount_TWD] AS [INV_AMOUNT_TWD], ";
            strSQL += "     [INV_Note] AS [INV_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_INV] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoiceDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceDetailsConfig>();

            #endregion

            #region - 版權採購申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var mediaInvoiceViewModel = new MediaInvoiceViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_INVOICE_TITLE = mediaInvoiceTitle,
                MEDIA_INVOICE_CONFIG = mediaInvoiceConfig,
                MEDIA_INVOICE_ACPTS_CONFIG = mediaInvoiceAcceptancesConfig,
                MEDIA_INVOICE_AUTHS_CONFIG = mediaInvoiceAuthorizesConfig,
                MEDIA_INVOICE_EXS_CONFIG = mediaInvoiceExtrasConfig,
                MEDIA_INVOICE_PYMTS_CONFIG = mediaInvoicePaymentsConfig,
                MEDIA_INVOICE_BUDGS_CONFIG = mediaInvoiceBudgetsConfig,
                MEDIA_INVOICE_DTLS_CONFIG = mediaInvoiceDetailsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            return mediaInvoiceViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購請款單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaAcceptanceRefill(MediaAcceptanceQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權採購請款單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權採購請款單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaInvoiceSingle(MediaInvoiceViewModel model)
        {
            bool vResult = false;
            try
            {
                var medialOrderformQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID
                };
                var medialOrderformData = formRepository.PostFormData(medialOrderformQueryModel);

                #region - 宣告 -

                #region - 主旨 -

                FM7Subject = model.MEDIA_INVOICE_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    FM7Subject = "【請款】第" + model.MEDIA_INVOICE_CONFIG.PERIOD + "期-" + medialOrderformData.FORM_SUBJECT;
                }

                #endregion

                #region - 預設設定 -

                if (String.IsNullOrEmpty(model.MEDIA_INVOICE_CONFIG.REIMBURSEMENT) || String.IsNullOrWhiteSpace(model.MEDIA_INVOICE_CONFIG.REIMBURSEMENT))
                {
                    //員工代墊
                    model.MEDIA_INVOICE_CONFIG.REIMBURSEMENT = "false";
                    //支付方式 
                    model.MEDIA_INVOICE_CONFIG.PAY_METHOD = "SUP_A/C";
                }

                #endregion

                #endregion

                #region - 版權採購請款單 表頭資訊：MediaInvoice_M -

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
                    //版權採購請款單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaInvoice_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 版權採購請款單 表單內容：MediaInvoice_M -

                if(model.MEDIA_INVOICE_CONFIG != null)
                {
                    #region - 【版權採購申請單】資訊 -

                    model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO = medialOrderformData.SERIAL_ID;
                    model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_SUBJECT = medialOrderformData.FORM_SUBJECT;
                    model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_PATH = GlobalParameters.FormContentPath(model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID, medialOrderformData.IDENTIFY, medialOrderformData.DIAGRAM_NAME);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權採購請款單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                        new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ACCEPTANCE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMBURSEMENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_TX_ID", SqlDbType.NVarChar) { Size = 1000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INVOICE_TYPE", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_MATERIAL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_MATERIAL_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_CURRENT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_CURRENT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACTUAL_PAY_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_SWIFT", SqlDbType.NVarChar) { Size = 300, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_ADDRESS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_COUNTRY_AND_CITY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_IBAN", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TX_CATEGORY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_EMAIL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    #region - 確認小數點後第二位 -
                                      
                    model.MEDIA_INVOICE_CONFIG.DTL_NET_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_NET_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_TAX_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_GROSS_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_GROSS_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_ORDER_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_ORDER_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.EX_AMOUNT_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.EX_AMOUNT_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.EX_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.EX_TAX_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.PYMT_EX_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.PYMT_EX_TAX_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.PYMT_CURRENT_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.PYMT_CURRENT_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.INV_AMOUNT_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.INV_AMOUNT_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.INV_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.INV_TAX_TOTAL, 2);

                    #endregion

                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_INVOICE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                    strSQL += "SET [MediaOrderRequisitionID]=@MEDIA_ORDER_REQUISITION_ID, ";
                    strSQL += "     [MediaOrderSubject]=@MEDIA_ORDER_SUBJECT, ";
                    strSQL += "     [MediaOrderBPMFormNo]=@MEDIA_ORDER_BPM_FORM_NO, ";
                    strSQL += "     [MediaOrderERPFormNo]=@MEDIA_ORDER_ERP_FORM_NO, ";
                    strSQL += "     [MediaOrderPath]=@MEDIA_ORDER_PATH, ";
                    strSQL += "     [MediaOrderTXN_Type]=MAIN.[TXN_TYPE], ";
                    strSQL += "     [MediaOrderDTL_OrderTotal]=MAIN.[DTL_ORDER_TOTAL], ";
                    strSQL += "     [MediaOrderDTL_OrderTotal_TWD]=MAIN.[DTL_ORDER_TOTAL_TWD], ";
                    strSQL += "     [MediaAcceptanceRequisitionID]=@MEDIA_ACCEPTANCE_REQUISITION_ID, ";
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
                    strSQL += "     [SupNo]=MAIN.[SUP_NO], ";
                    strSQL += "     [SupName]=MAIN.[SUP_NAME], ";
                    strSQL += "     [PayMethod]=@PAY_METHOD, ";
                    strSQL += "     [RegisterKind]=MAIN.[REG_KIND], ";
                    strSQL += "     [RegisterNo]=MAIN.[REG_NO], ";
                    strSQL += "     [SupTXId]=@SUP_TX_ID, ";
                    strSQL += "     [InvoiceType]=@INVOICE_TYPE, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [DTL_NetTotal]=@DTL_NET_TOTAL, ";
                    strSQL += "     [DTL_NetTotal_TWD]=@DTL_NET_TOTAL_TWD, ";
                    strSQL += "     [DTL_TaxTotal]=@DTL_TAX_TOTAL, ";
                    strSQL += "     [DTL_TaxTotal_TWD]=@DTL_TAX_TOTAL_TWD, ";
                    strSQL += "     [DTL_GrossTotal]=@DTL_GROSS_TOTAL, ";
                    strSQL += "     [DTL_GrossTotal_TWD]=@DTL_GROSS_TOTAL_TWD, ";
                    strSQL += "     [DTL_MaterialTotal]=@DTL_MATERIAL_TOTAL, ";
                    strSQL += "     [DTL_MaterialTotal_TWD]=@DTL_MATERIAL_TOTAL_TWD, ";
                    strSQL += "     [DTL_OrderTotal]=@DTL_ORDER_TOTAL, ";
                    strSQL += "     [DTL_OrderTotal_TWD]=@DTL_ORDER_TOTAL_TWD, ";
                    strSQL += "     [EX_AmountTotal]=@EX_AMOUNT_TOTAL, ";
                    strSQL += "     [EX_AmountTotal_TWD]=@EX_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [EX_TaxTotal]=@EX_TAX_TOTAL, ";
                    strSQL += "     [EX_TaxTotal_TWD]=@EX_TAX_TOTAL_TWD, ";
                    strSQL += "     [PYMT_CurrentTotal]=@PYMT_CURRENT_TOTAL, ";
                    strSQL += "     [PYMT_CurrentTotal_TWD]=@PYMT_CURRENT_TOTAL_TWD, ";
                    strSQL += "     [PYMT_EX_TaxTotal]=@PYMT_EX_TAX_TOTAL, ";
                    strSQL += "     [PYMT_EX_TaxTotal_TWD]=@PYMT_EX_TAX_TOTAL_TWD, ";
                    strSQL += "     [INV_AmountTotal]=@INV_AMOUNT_TOTAL, ";
                    strSQL += "     [INV_AmountTotal_TWD]=@INV_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [INV_TaxTotal]=@INV_TAX_TOTAL, ";
                    strSQL += "     [INV_TaxTotal_TWD]=@INV_TAX_TOTAL_TWD, ";
                    strSQL += "     [ActualPayAmount]=@ACTUAL_PAY_AMOUNT, ";
                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2, ";
                    strSQL += "     [BFCY_BanKBranchNo]=@BFCY_BANK_BRANCH_NO, ";
                    strSQL += "     [BFCY_BanKBranchName]=@BFCY_BANK_BRANCH_NAME, ";
                    strSQL += "     [BFCY_BankSWIFT]=@BFCY_BANK_SWIFT, ";
                    strSQL += "     [BFCY_BankAddress]=@BFCY_BANK_ADDRESS, ";
                    strSQL += "     [BFCY_BankCountryAndCity]=@BFCY_BANK_COUNTRY_AND_CITY, ";
                    strSQL += "     [BFCY_BankIBAN]=@BFCY_BANK_IBAN, ";
                    strSQL += "     [TX_Category]=@TX_CATEGORY, ";
                    strSQL += "     [BFCY_AccountNo]=@BFCY_ACCOUNT_NO, ";
                    strSQL += "     [BFCY_AccountName]=@BFCY_ACCOUNT_NAME, ";
                    strSQL += "     [BFCY_BankNo]=@BFCY_BANK_NO, ";
                    strSQL += "     [BFCY_BankName]=@BFCY_BANK_NAME, ";
                    strSQL += "     [CurrencyName]=@CURRENCY_NAME, ";
                    strSQL += "     [BFCY_Name]=@BFCY_NAME, ";
                    strSQL += "     [BFCY_TEL]=@BFCY_TEL, ";
                    strSQL += "     [BFCY_Email]=@BFCY_EMAIL ";
                    strSQL += "     FROM ( ";
                    strSQL += "             select ";
                    strSQL += "                 [TXN_Type] AS [TXN_TYPE], ";
                    strSQL += "                 [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
                    strSQL += "                 [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
                    strSQL += "                 [Currency] AS [CURRENCY], ";
                    strSQL += "                 [PredictRate] AS [PRE_RATE], ";
                    strSQL += "                 [PricingMethod] AS [PRICING_METHOD], ";
                    strSQL += "                 [TaxRate] AS [TAX_RATE], ";
                    strSQL += "                 [SupNo] AS [SUP_NO], ";
                    strSQL += "                 [SupName] AS [SUP_NAME], ";
                    strSQL += "                 [RegisterKind] AS [REG_KIND], ";
                    strSQL += "                 [RegisterNo] AS [REG_NO] ";
                    strSQL += "             FROM [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
                    strSQL += "             WHERE [RequisitionID] = @MEDIA_ORDER_REQUISITION_ID ";
                    strSQL += "     ) AS MAIN ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權採購請款單 驗收明細: MediaInvoice_ACPT -

                //View 是執行
                //版權採購的 採購明細(MediaInvoice_DTL)及驗收明細(MediaInvoice_ACPT)

                #endregion

                #region - 版權採購請款單 授權權利: MediaInvoice_AUTH -

                //View 是執行
                //版權採購的 授權權利(MediaInvoice_AUTH)

                #endregion

                #region - 版權採購申請單 額外項目: MediaInvoice_EX -

                //View 是執行
                //版權採購的 額外項目(MediaInvoice_EX)

                #endregion

                #region - 版權採購請款單 付款辦法: MediaInvoice_PYMT -

                //View 是執行
                //版權採購的 付款辦法(MediaInvoice_PYMT)
                //只有在「財務部簽核」會需要更新 會計類別(ACCT_Category) 欄位



                var parameterPayments = new List<SqlParameter>()
                {
                    //版權採購請款 付款辦法 更新:會計類別
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACCT_CATEGORY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                if (model.MEDIA_INVOICE_PYMTS_CONFIG != null && model.MEDIA_INVOICE_PYMTS_CONFIG.Count > 0)
                {
                    #region 修改資料

                    foreach (var item in model.MEDIA_INVOICE_PYMTS_CONFIG)
                    {
                        //寫入：版權採購申請 付款辦法 更新:會計類別parameter

                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterPayments);

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrder_PYMT] ";
                        strSQL += "SET [ACCT_Category]=@ACCT_CATEGORY ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                        strSQL += "         AND [Period]=@PERIOD ";

                        dbFun.DoTran(strSQL, parameterPayments);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購請款單 使用預算: MediaInvoice_BUDG -

                //View 是執行
                //版權採購的 使用預算(MediaInvoice_BUDG) 內容。

                #endregion

                #region - 版權採購請款單 憑證明細：MediaInvoice_INV -

                var parameterDetails = new List<SqlParameter>()
                {
                    //版權採購請款單 憑證明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)medialOrderformData.SERIAL_ID ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NUM", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_DATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_EXCL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_EXCL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_INV] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_INVOICE_DTLS_CONFIG != null && model.MEDIA_INVOICE_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_INVOICE_DTLS_CONFIG)
                    {
                        #region - 確認小數點後第二位 -

                        item.INV_EXCL = Math.Round(item.INV_EXCL, 2);
                        item.INV_TAX = Math.Round(item.INV_TAX, 2);
                        item.INV_NET = Math.Round(item.INV_NET, 2);
                        item.INV_GROSS = Math.Round(item.INV_GROSS, 2);
                        item.INV_AMOUNT = Math.Round(item.INV_AMOUNT, 2);

                        #endregion

                        //寫入：版權採購請款 發票明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaInvoice_INV]([RequisitionID],[Period],[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo],[INV_Num],[INV_Date],[INV_Excl],[INV_Excl_TWD],[INV_Tax],[INV_Tax_TWD],[INV_Net],[INV_Net_TWD],[INV_Gross],[INV_Gross_TWD],[INV_Amount],[INV_Amount_TWD],[INV_Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO,@INV_NUM,@INV_DATE,@INV_EXCL,@INV_EXCL_TWD,@INV_TAX,@INV_TAX_TWD,@INV_NET,@INV_NET_TWD,@INV_GROSS,@INV_GROSS_TWD,@INV_AMOUNT,@INV_AMOUNT_TWD,@INV_NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購請款單 表單關聯：AssociatedForm -

                //關聯表:匯入【版權採購申請單】的「關聯表單」
                var importAssociatedForm = commonRepository.PostAssociatedForm(medialOrderformQueryModel);

                #region 關聯表:加上【版權採購申請單】

                importAssociatedForm.Add(new AssociatedFormConfig()
                {
                    IDENTIFY = medialOrderformData.IDENTIFY,
                    ASSOCIATED_REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID,
                    BPM_FORM_NO = medialOrderformData.SERIAL_ID,
                    FM7_SUBJECT = medialOrderformData.FORM_SUBJECT,
                    APPLICANT_DEPT_NAME = medialOrderformData.APPLICANT_DEPT_NAME,
                    APPLICANT_NAME = medialOrderformData.APPLICANT_NAME,
                    APPLICANT_DATE_TIME = medialOrderformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                    FORM_PATH = GlobalParameters.FormContentPath(model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID, medialOrderformData.IDENTIFY, medialOrderformData.DIAGRAM_NAME),
                    STATE = BPMStatusCode.CLOSE
                });

                #endregion

                #region 關聯表:加上【版權採購點驗收單】

                if (!String.IsNullOrEmpty(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID))
                {
                    var medialAcceptanceformQueryModel = new FormQueryModel()
                    {
                        REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID
                    };
                    var medialAcceptanceformData = formRepository.PostFormData(medialAcceptanceformQueryModel);

                    importAssociatedForm.Add(new AssociatedFormConfig()
                    {
                        IDENTIFY = medialAcceptanceformData.IDENTIFY,
                        ASSOCIATED_REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID,
                        BPM_FORM_NO = medialAcceptanceformData.SERIAL_ID,
                        FM7_SUBJECT = medialAcceptanceformData.FORM_SUBJECT,
                        APPLICANT_DEPT_NAME = medialAcceptanceformData.APPLICANT_DEPT_NAME,
                        APPLICANT_NAME = medialAcceptanceformData.APPLICANT_NAME,
                        APPLICANT_DATE_TIME = medialAcceptanceformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                        FORM_PATH = GlobalParameters.FormContentPath(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID, medialAcceptanceformData.IDENTIFY, medialAcceptanceformData.DIAGRAM_NAME),
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
                CommLib.Logger.Error("版權採購請款單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }
            return vResult;
        }


        /// <summary>
        /// 版權採購請款單(財務審核關卡-關聯表單(知會))：
        /// 【財務審核關卡】"版權採購請款"單關聯表單列表知會；
        /// 確認是否有代理人，
        /// 並知會給代理人。
        /// </summary>
        public bool PutMediaInvoiceNotifySingle(MediaInvoiceQueryModel query)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                #region - 財務審核人 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [FinancAuditID_1] AS [FINANC_AUDIT_ID_1] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                var dtFinancAudit1 = dbFun.DoQuery(strSQL, parameter);
                var FinancAudit1 = dtFinancAudit1.Rows[0][0].ToString();

                if (!string.IsNullOrEmpty(FinancAudit1) || !string.IsNullOrWhiteSpace(FinancAudit1))
                {
                    var flowQueryModel = new FlowQueryModel()
                    {
                        USER_ID = FinancAudit1
                    };
                    //被知會通知人
                    var NotifyBys = new List<String>()
                    {
                        FinancAudit1
                    };

                    #region - 代理人 -

                    var Agents = flowRepository.PostAgent(flowQueryModel);

                    if (Agents != null)
                    {
                        if (Agents.Count > 0)
                        {
                            Agents.ForEach(A =>
                            {
                                NotifyBys.Add(A.AGENT_ID);
                            });
                        }
                        else
                        {
                            CommLib.Logger.Debug(query.REQUISITION_ID + "：" + FinancAudit1 + "目前(" + DateTime.Now + ")尚無設定 「代理人」(2)。");
                        }
                    }
                    else
                    {
                        CommLib.Logger.Debug(query.REQUISITION_ID + "：" + FinancAudit1 + "目前(" + DateTime.Now + ")尚無設定 「代理人」(1)。");
                    }

                    #endregion

                    #region - 關聯表單(知會) -

                    var associatedFormNotifyModel = new AssociatedFormNotifyModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID,
                        NOTIFY_BY = NotifyBys
                    };
                    vResult = commonRepository.PutAssociatedFormNotify(associatedFormNotifyModel);

                    #endregion
                }
                else
                {
                    CommLib.Logger.Debug(query.REQUISITION_ID + "：此表單「尚未決定」財務審核人，可(知會)通知。");
                }

                #endregion

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購請款單(財務審核關卡-關聯表單(知會))通知失敗，原因：" + ex.Message);
                throw;
            }
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
        private string IDENTIFY = "MediaInvoice";

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