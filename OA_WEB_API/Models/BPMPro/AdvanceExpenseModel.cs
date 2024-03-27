using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 預支單(查詢條件)
    /// </summary>
    public class AdvanceExpenseQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 預支單
    /// </summary>
    public class AdvanceExpenseViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>預支單 表頭資訊</summary>
        public AdvanceExpenseTitle ADVANCE_EXPENSE_TITLE { get; set; }

        /// <summary>預支單 表單內容 設定</summary>
        public AdvanceExpenseConfig ADVANCE_EXPENSE_CONFIG { get; set; }

        /// <summary>預支單 預知明細 設定</summary>
        public IList<AdvanceExpenseDetailsConfig> ADVANCE_EXPENSE_DTLS_CONFIG { get; set; }

        /// <summary>預支單 小計 設定</summary>
        public List<AdvanceExpenseSumsConfig> ADVANCE_EXPENSE_SUMS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 預支單 表頭資訊
    /// </summary>
    public class AdvanceExpenseTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 預支單 表單內容 設定
    /// </summary>
    public class AdvanceExpenseConfig
    {
        /// <summary>是否過協理</summary>
        public string IS_CFO { get; set; }

        /// <summary>申請源由</summary>
        public string REASON { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

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
    /// 預支單 預支明細 設定
    /// </summary>
    public class AdvanceExpenseDetailsConfig : DOM_TWD_Bank
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>預知幣別</summary>
        public string ADVANCE_CURRENCY_NAME { get; set; }

        /// <summary>匯率</summary>
        public double EXCH_RATE { get; set; }

        /// <summary>預支金額</summary>
        public double ADVANCE_AMOUNT { get; set; }

        /// <summary>換算台幣</summary>
        public int AMOUNT_CONV { get; set; }

        /// <summary>預支日期</summary>
        public DateTime ADVANCE_DATE { get; set; }

        /// <summary>預計還款沖銷日</summary>
        public DateTime REPAYMENT_DATE { get; set; }

        /// <summary>
        /// 還款方式：
        /// CS.現金、
        /// TM.轉帳
        /// </summary>
        public string REPAYMENT_TYPE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

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
    /// 預支單 小計 設定
    /// </summary>
    public class AdvanceExpenseSumsConfig : FinanceFieldConfig
    {

    }
}