using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 差旅費用報支單(查詢條件)
    /// </summary>
    public class StaffTravellingExpensesQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 差旅費用報支單
    /// </summary>
    public class StaffTravellingExpensesViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>差旅費用報支單 表頭資訊</summary>
        public StaffTravellingExpensesTitle STAFF_TRAVELLING_EXPENSES_TITLE { get; set; }

        /// <summary>差旅費用報支單 表單內容 設定</summary>
        public StaffTravellingExpensesConfig STAFF_TRAVELLING_EXPENSES_CONFIG { get; set; }

        /// <summary>差旅費用報支單 差旅明細 設定</summary>
        public List<StaffTravellingExpensesDetailsConfig> STAFF_TRAVELLING_EXPENSES_DTLS_CONFIG { get; set; }

        /// <summary>差旅費用報支單 憑證明細 設定</summary>
        public List<StaffTravellingExpensesInvoiceDetailsConfig> STAFF_TRAVELLING_EXPENSES_INV_DTLS_CONFIG { get; set; }

        /// <summary>差旅費用報支單 使用預算 設定</summary>
        public List<StaffTravellingExpensesBudgetsConfig> STAFF_TRAVELLING_EXPENSES_BUDGS_CONFIG { get; set; }

        /// <summary>差旅費用報支單 小計 設定</summary>
        public List<StaffTravellingExpensesSumsConfig> STAFF_TRAVELLING_EXPENSES_SUMS_CONFIG { get; set; }

        /// <summary>差旅費用報支單 已預支 設定</summary>
        public List<StaffTravellingExpensesAdvancesConfig> STAFF_TRAVELLING_EXPENSES_ADVS_CONFIG { get; set; }

        /// <summary>差旅費用報支單 應退(退還財務) 設定</summary>
        public List<StaffTravellingExpensesFinancAmountsConfig> STAFF_TRAVELLING_EXPENSES_FAS_CONFIG { get; set; }

        /// <summary>差旅費用報支單 應付(付給使用者) 設定</summary>
        public List<StaffTravellingExpensesUserAmountsConfig> STAFF_TRAVELLING_EXPENSES_UAS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 差旅費用報支單 表頭資訊
    /// </summary>
    public class StaffTravellingExpensesTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 差旅費用報支單 表單內容 設定
    /// </summary>
    public class StaffTravellingExpensesConfig
    {
        /// <summary>是否過協理</summary>
        public string IS_CFO { get; set; }

        /// <summary>出差人員</summary>
        public string TRAVELLING_STAFFS { get; set; }

        /// <summary>申請源由</summary>
        public string REASON { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>
        /// 退款方式：
        /// CS.現金
        /// PT_AC.薪轉帳戶
        /// OR.其他[X]
        /// DT.其他帳戶(需自行負擔手續費)[?]
        /// </summary>
        public string REFUND_MODE { get; set; }

        /// <summary>換算台幣 合計</summary>
        public int AMOUNT_CONV_TOTAL { get; set; }

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
    /// 差旅費用報支單 差旅明細 設定
    /// </summary>
    public class StaffTravellingExpensesDetailsConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>行程日期</summary>
        public Nullable<DateTime> TRAVELLING_DATE { get; set; }

        /// <summary>起訖地點</summary>
        public string PLACE { get; set; }

        /// <summary>付款人</summary>
        public string PAYER { get; set; }

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

        /// <summary>憑證行數編號</summary>
        public int INV_ROW_NO { get; set; }

        /// <summary>
        /// 憑證類型：
        /// INV_TW.統一發票、
        /// INV_F. Invoice、
        /// RECPT.收據
        /// </summary>
        public string INV_TYPE { get; set; }

        /// <summary>憑證號碼</summary>
        public string NUM { get; set; }

        /// <summary>憑證日期</summary>
        public Nullable<DateTime> DATE { get; set; }

        /// <summary>項目金額</summary>
        public double AMOUNT { get; set; }

        /// <summary>換算台幣</summary>
        public int AMOUNT_CONV { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>匯率</summary>
        public double EXCH_RATE { get; set; }

        /// <summary>憑證附件</summary>
        public string INV_NOTE { get; set; }
    }

    /// <summary>
    /// 差旅費用報支單 憑證細項 設定
    /// </summary>
    public class StaffTravellingExpensesInvoiceDetailsConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>憑證行數編號</summary>
        public int INV_ROW_NO { get; set; }

        /// <summary>憑證號碼</summary>
        public string NUM { get; set; }

        /// <summary>項目類型</summary>
        public string TYPE { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>金額</summary>
        public double AMOUNT { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

    }

    /// <summary>
    /// 差旅費用報支單 使用預算 設定
    /// </summary>
    public class StaffTravellingExpensesBudgetsConfig : BudgetConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }
    }

    /// <summary>
    /// 差旅費用報支單 小計 設定
    /// </summary>
    public class StaffTravellingExpensesSumsConfig : FinanceFieldConfig
    {

    }

    /// <summary>
    /// 差旅費用報支單 已預支 設定
    /// </summary>
    public class StaffTravellingExpensesAdvancesConfig : FinanceFieldExchangeRateConfig
    {

    }

    /// <summary>
    /// 差旅費用報支單 應退(退還財務) 設定
    /// </summary>
    public class StaffTravellingExpensesFinancAmountsConfig : FinanceFieldExchangeRateConfig
    {

    }

    /// <summary>
    /// 差旅費用報支單 應付(付給使用者) 設定
    /// </summary>
    public class StaffTravellingExpensesUserAmountsConfig : FinanceFieldExchangeRateConfig
    {

    }
}