using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 費用申請單(查詢條件)
    /// </summary>
    public class ExpensesReimburseQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 費用申請單
    /// </summary>
    public class ExpensesReimburseViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>費用申請單 表頭資訊</summary>
        public ExpensesReimburseTitle EXPENSES_REIMBURSE_TITLE { get; set; }

        /// <summary>費用申請單 表單內容 設定</summary>
        public ExpensesReimburseConfig EXPENSES_REIMBURSE_CONFIG { get; set; }

        /// <summary>費用申請單 費用明細 設定</summary>
        public List<ExpensesReimburseDetailsConfig> EXPENSES_REIMBURSE_DTLS_CONFIG { get; set; }

        /// <summary>費用申請單 使用預算 設定</summary>
        public List<ExpensesReimburseBudgetsConfig> EXPENSES_REIMBURSE_BUDGS_CONFIG { get; set; }

        /// <summary>費用申請單 小計 設定</summary>
        public List<ExpensesReimburseSumsConfig> EXPENSES_REIMBURSE_SUMS_CONFIG { get; set; }

        /// <summary>費用申請單 已預支 設定</summary>
        public List<ExpensesReimburseAdvancesConfig> EXPENSES_REIMBURSE_ADVS_CONFIG { get; set; }

        /// <summary>費用申請單 申請人應繳 設定</summary>
        public List<ExpensesReimburseUserAmountsConfig> EXPENSES_REIMBURSE_UAS_CONFIG { get; set; }

        /// <summary>費用申請單 財務應退(合作夥伴廠商/合作夥伴個人/員工) 設定</summary>
        public List<ExpensesReimburseFinancAmountsConfig> EXPENSES_REIMBURSE_FAS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 費用申請單 表頭資訊
    /// </summary>
        public class ExpensesReimburseTitle : ImplementHeader
        {
            /// <summary>ERP 工作流程標題名稱</summary>
            public string FLOW_NAME { get; set; }
        }

    /// <summary>
    /// 費用申請單 表單內容 設定
    /// </summary>
    public class ExpensesReimburseConfig : COMM_Bank
    {
        /// <summary>是否過財務協理</summary>
        public string IS_CFO { get; set; }

        /// <summary>是否過副總</summary>
        public string IS_VICE_PRESIDENT { get; set; }

        /// <summary>代墊員工部門編號</summary>
        public string REIMB_STAFF_DEPT_ID { get; set; }

        /// <summary>代墊員工部門名稱</summary>
        public string REIMB_STAFF_DEPT_NAME { get; set; }

        /// <summary>代墊員工編號</summary>
        public string REIMB_STAFF_ID { get; set; }

        /// <summary>代墊員工姓名</summary>
        public string REIMB_STAFF_NAME { get; set; }

        /// <summary>申請源由</summary>
        public string REASON { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }

        /// <summary>憑證金額_換算台幣 合計</summary>
        public int AMOUNT_CONV_TOTAL { get; set; }

        /// <summary>
        /// 支付方式：
        /// CS.現金
        /// PT_AC.薪轉帳戶
        /// OR.其他[X]
        /// DT.其他帳戶(需自行負擔手續費)
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>
        /// 帳戶類別：
        /// A.廠商
        /// B.個人
        /// PayMethod：
        /// CS.(現金)、
        /// PT_AC.(薪轉帳戶)
        /// 帳戶類別 固定都會是B。
        /// </summary>
        public string ACCOUNT_CATEGORY { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>
        /// 支付對象：
        /// 員工編號、
        /// 合作夥伴的統編
        /// </summary>
        public string PAYMENT_OBJECT_NO { get; set; }

        /// <summary>
        /// 支付對象：
        /// 員工姓名、
        /// 合作夥伴的名稱
        /// </summary>
        public string PAYMENT_OBJECT_NAME { get; set; }
    }

    /// <summary>
    /// 費用申請單 費用明細 設定
    /// </summary>
    public class ExpensesReimburseDetailsConfig : InvoiceConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>項目名稱</summary>
        public string NAME { get; set; }

        /// <summary>項目類別</summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 憑證類型：
        /// INV_TW.統一發票、
        /// INV_F. Invoice、
        /// RECPT.收據
        /// </summary>
        public string INV_TYPE { get; set; }

        /// <summary>匯率</summary>
        public double EXCH_RATE { get; set; }

        /// <summary>憑證金額_換算台幣</summary>
        public int AMOUNT_CONV { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        ///// <summary>會計類別</summary>
        //public string ACCT_CATEGORY { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案年分</summary>
        public string PROJECT_USE_YEAR { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 費用申請單 使用預算 設定
    /// </summary>
    public class ExpensesReimburseBudgetsConfig : BudgetConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }
    }       

    /// <summary>
    /// 費用申請單 小計 設定
    /// </summary>
    public class ExpensesReimburseSumsConfig : FinanceFieldConfig
    {

    }

    /// <summary>
    /// 費用申請單 已預支 設定
    /// </summary>
    public class ExpensesReimburseAdvancesConfig : FinanceFieldExchangeRateConfig
    {

    }

    /// <summary>
    /// 費用申請單 申請人應繳 設定
    /// </summary>
    public class ExpensesReimburseUserAmountsConfig : FinanceFieldExchangeRateConfig
    {

    }

    /// <summary>
    /// 費用申請單 財務應退(合作夥伴廠商/合作夥伴個人/員工) 設定
    /// </summary>
    public class ExpensesReimburseFinancAmountsConfig : FinanceFieldExchangeRateConfig
    {

    }
}