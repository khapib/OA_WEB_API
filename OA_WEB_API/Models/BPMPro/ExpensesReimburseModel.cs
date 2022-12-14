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
        public IList<ExpensesReimburseDetailsConfig> EXPENSES_REIMBURSE_DTLS_CONFIG { get; set; }

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
    public class ExpensesReimburseConfig
    {
        /// <summary>代墊員工部門編號</summary>
        public string REIMB_STAFF_DEPT_ID { get; set; }

        /// <summary>代墊員工部門名稱</summary>
        public string REIMB_STAFF_DEPT_NAME { get; set; }

        /// <summary>代墊員工編號</summary>
        public string REIMB_STAFF_ID { get; set; }

        /// <summary>代墊員工姓名</summary>
        public string REIMB_STAFF_NAME { get; set; }

        /// <summary>支付方式</summary>
        public string PAY_METHOD { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>是否過副總</summary>
        public string IS_VICE_PRESIDENT { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }

        /// <summary>發票金額_換算台幣 合計</summary>
        public int AMOUNT_CONV_TOTAL { get; set; }
    }

    /// <summary>
    /// 費用申請單 明細 設定
    /// </summary>
    public class ExpensesReimburseDetailsConfig
    {
        /// <summary>行數編號</summary>
        public int DTL_ROW_NO { get; set; }

        /// <summary>
        /// 憑證類型：
        /// GUI.統一發票、
        /// Invoice. Invoice、
        /// RECPT.收據
        /// </summary>
        public string INV_TYPE { get; set; }

        /// <summary>憑證號碼</summary>
        public string INV_NUM { get; set; }

        /// <summary>憑證日期</summary>
        public string INV_DATE { get; set; }

        /// <summary>項目名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>項目類別</summary>
        public string ITEM_TYPE { get; set; }

        /// <summary>申請源由</summary>
        public string REASON { get; set; }

        /// <summary>發票金額</summary>
        public double INV_AMOUNT { get; set; }

        /// <summary>匯率</summary>
        public double EXCH_RATE { get; set; }

        /// <summary>發票金額_換算台幣</summary>
        public int AMOUNT_CONV { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>會計類別</summary>
        public string ACCT_CATEGORY { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案年分</summary>
        public string PROJECT_USE_YEAR { get; set; }
    }
}