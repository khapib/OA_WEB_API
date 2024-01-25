using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 行政採購請款單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 行政採購請款單(查詢條件)
    /// </summary>
    public class GeneralInvoiceQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 行政採購請款單
    /// </summary>
    public class GeneralInvoiceViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>行政採購請款單 表頭資訊</summary>
        public GeneralInvoiceTitle GENERAL_INVOICE_TITLE { get; set; }

        /// <summary>行政採購請款單 表單內容 設定</summary>
        public GeneralInvoiceConfig GENERAL_INVOICE_CONFIG { get; set; }

        /// <summary>行政採購請款單 驗收明細 設定</summary>
        public IList<GeneralInvoiceAcceptancesConfig> GENERAL_INVOICE_ACCEPTANCES_CONFIG { get; set; }

        /// <summary>行政採購請款單 付款辦法 設定</summary>
        public IList<GeneralInvoicePaymentsConfig> GENERAL_INVOICE_PAYMENTS_CONFIG { get; set; }

        /// <summary>行政採購申請 使用預算 設定</summary>
        public IList<GeneralInvoiceBudgetsConfig> GENERAL_INVOICE_BUDGETS_CONFIG { get; set; }

        /// <summary>行政採購申請 憑證明細 設定</summary>
        public List<GeneralInvoiceInvoicesConfig> GENERAL_INVOICE_INVS_CONFIG { get; set; }

        /// <summary>行政採購申請 憑證細項 設定</summary>
        public List<GeneralInvoiceInvoiceDetailsConfig> GENERAL_INVOICE_INV_DTLS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }
    
    /// <summary>
    /// 行政採購請款單 表頭資訊
    /// </summary>
    public class GeneralInvoiceTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }

        /// <summary>是否經由財務協理簽核</summary>
        public string IS_CFO { get; set; }
    }

    /// <summary>
    /// 行政採購請款單 表單內容 設定
    /// </summary>
    public class GeneralInvoiceConfig : COMM_Bank
    {
        /// <summary>行政採購 系統編號</summary>
        public string GENERAL_ORDER_REQUISITION_ID { get; set; }

        /// <summary>行政採購 主旨</summary>
        public string GENERAL_ORDER_SUBJECT { get; set; }

        /// <summary>行政採購 BPM 表單單號</summary>
        public string GENERAL_ORDER_BPM_FORM_NO { get; set; }

        /// <summary>行政採購 ERP 表單唯一碼</summary>
        public string GENERAL_ORDER_ERP_FORM_NO { get; set; }

        /// <summary>行政採購 交易類型</summary>
        public string GENERAL_ORDER_TXN_TYPE { get; set; }

        /// <summary>行政採購 路徑</summary>
        public string GENERAL_ORDER_PATH { get; set; }

        /// <summary>行政採購 採購單 付款辦法總額</summary>
        public double GENERAL_ORDER_PYMT_GROSS_TOTAL { get; set; }

        /// <summary>行政採購 採購單 付款辦法總額_台幣(換算)</summary>
        public int GENERAL_ORDER_PYMT_GROSS_TOTAL_CONV { get; set; }

        /// <summary>行政採購點驗收單 系統編號</summary>
        public string GENERAL_ACCEPTANCE_REQUISITION_ID { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>預計匯率</summary>
        public double PRE_RATE { get; set; }

        /// <summary>計價方式</summary>
        public string PRICING_METHOD { get; set; }

        /// <summary>營業稅/[稅率]租稅協定</summary>
        public double TAX_RATE { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>員工代墊</summary>
        public string REIMBURSEMENT { get; set; }

        /// <summary>代墊員工部門編號</summary>
        public string REIMB_STAFF_DEPT_ID { get; set; }

        /// <summary>代墊員工部門名稱</summary>
        public string REIMB_STAFF_DEPT_NAME { get; set; }

        /// <summary>代墊員工編號</summary>
        public string REIMB_STAFF_ID { get; set; }

        /// <summary>代墊員工姓名</summary>
        public string REIMB_STAFF_NAME { get; set; }

        /// <summary>
        /// 支付方式：
        /// CS.現金
        /// PT_AC.薪轉帳戶
        /// D_AC.指定帳戶
        /// SUP_AC.合作夥伴帳戶
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }

        /// <summary>廠商名稱</summary>
        public string SUP_NAME { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>銀行往來編號</summary>
        public string SUP_TX_ID { get; set; }

        /// <summary>
        /// 憑證類型：
        /// GUI.統一發票、
        /// Invoice. Invoice、
        /// RECPT.收據
        /// </summary>
        public string INVOICE_TYPE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>付款辦法 本期付款總額</summary>
        public double PYMT_CURRENT_TOTAL { get; set; }

        /// <summary>付款辦法 本期付款總額_台幣</summary>
        public int PYMT_CURRENT_TOTAL_TWD { get; set; }

        /// <summary>憑證總稅額</summary>
        public double INV_TAX_TOTAL { get; set; }

        /// <summary>憑證總稅額_台幣</summary>
        public int INV_TAX_TOTAL_TWD { get; set; }

        /// <summary>憑證明細 合計</summary>
        public double INV_AMOUNT_TOTAL { get; set; }

        /// <summary>憑證明細 合計_台幣</summary>
        public int INV_AMOUNT_TOTAL_TWD { get; set; }

        /// <summary>實際支付金額</summary>
        public int ACTUAL_PAY_AMOUNT { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }

    }

    /// <summary>
    /// 行政採購請款單 驗收明細 設定
    /// </summary>
    public class GeneralInvoiceAcceptancesConfig : GeneralOrderAcceptancesConfig
    {
        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案起案年度</summary>
        public string PROJECT_USE_YEAR { get; set; }
    }

    /// <summary>
    /// 行政採購請款單 付款辦法 設定
    /// </summary>
    public class GeneralInvoicePaymentsConfig : GeneralOrderPaymentsConfig
    {
        /// <summary>會計類別</summary>
        public string ACCT_CATEGORY { get; set; }
    }

    /// <summary>
    /// 行政採購請款單 使用預算 設定
    /// </summary>
    public class GeneralInvoiceBudgetsConfig : GeneralOrderBudgetsConfig
    {

    }

    /// <summary>
    /// 行政採購請款單 憑證明細 設定
    /// </summary>
    public class GeneralInvoiceInvoicesConfig : InvoiceConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>憑證行數編號</summary>
        public int INV_ROW_NO { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 行政採購申請 憑證細項 設定
    /// </summary>
    public class GeneralInvoiceInvoiceDetailsConfig : InvoiceDetailConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>憑證行數編號</summary>
        public int INV_ROW_NO { get; set; }

    }
}