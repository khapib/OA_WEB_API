using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 預支費用申請單(查詢條件)
    /// </summary>
    public class AdvanceExpenseQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 預支費用申請單
    /// </summary>
    public class AdvanceExpenseViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>預支費用申請單 表頭資訊</summary>
        public AdvanceExpenseTitle ADVANCE_EXPENSE_TITLE { get; set; }

        /// <summary>預支費用申請單 表單內容 設定</summary>
        public AdvanceExpenseConfig ADVANCE_EXPENSE_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 預支費用申請單 表頭資訊
    /// </summary>
    public class AdvanceExpenseTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 費用申請單 表單內容 設定
    /// </summary>
    public class AdvanceExpenseConfig : DOM_TWD_Bank
    {
        /// <summary>預支員工部門編號</summary>
        public string ADV_STAFF_DEPT_ID { get; set; }

        /// <summary>預支員工部門名稱</summary>
        public string ADV_STAFF_DEPT_NAME { get; set; }

        /// <summary>預支員工編號</summary>
        public string ADV_STAFF_ID { get; set; }

        /// <summary>預支員工姓名</summary>
        public string ADV_STAFF_NAME { get; set; }

        /// <summary>
        /// 支付方式：
        /// CS.現金
        /// PT_AC.薪轉帳戶
        /// OR.其他
        /// DT.(國內)_台幣其他帳戶(需自行負擔手續費)
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>申請源由</summary>
        public string REASON { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>預支金額</summary>
        public int ADVANCE_AMOUNT { get; set; }

        /// <summary>預支日期</summary>
        public Nullable<DateTime> ADVANCE_DATE { get; set; }

        /// <summary>預計還款沖銷日</summary>
        public Nullable<DateTime> REPYMT_DATE { get; set; }

        /// <summary>
        /// 還款方式：
        /// CS.現金
        /// TM.轉帳
        /// </summary>
        public string REPYMT_TYPE { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }
    }


}