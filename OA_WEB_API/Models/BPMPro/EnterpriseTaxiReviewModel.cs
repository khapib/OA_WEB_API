using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 企業乘車審核單(查詢條件)
    /// </summary>
    public class EnterpriseTaxiReviewQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 企業乘車審核單
    /// </summary>
    public class EnterpriseTaxiReviewViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>企業乘車審核單 表頭資訊</summary>
        public EnterpriseTaxiReviewTitle ENTERPRISE_TAXI_REVIEW_TITLE { get; set; }

        /// <summary>企業乘車審核單 表單內容 設定</summary>
        public EnterpriseTaxiReviewConfig ENTERPRISE_TAXI_REVIEW_CONFIG { get; set; }

        /// <summary>企業乘車審核單 乘車明細 設定</summary>
        public List<EnterpriseTaxiReviewDetailsConfig> ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG { get; set; }

        /// <summary>企業乘車審核單 使用預算 設定</summary>
        public List<EnterpriseTaxiReviewBudgetsConfig> ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG { get; set; }

    }

    /// <summary>
    /// 企業乘車審核單 表頭資訊
    /// </summary>
    public class EnterpriseTaxiReviewTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 企業乘車審核單 表單內容 設定
    /// </summary>
    public class EnterpriseTaxiReviewConfig
    {
        /// <summary>列帳期間起</summary>
        public Nullable<DateTime> ACCOUNTING_DATE_START { get; set; }

        /// <summary>列帳期間迄</summary>
        public Nullable<DateTime> ACCOUNTING_DATE_END { get; set; }

        /// <summary>結款方案</summary>
        public string BILL_PLAN { get; set; }

        /// <summary>還款期間</summary>
        public Nullable<DateTime> PAY_OFF_PERIOD { get; set; }

        /// <summary>新增費用</summary>
        public int ADD_EXPENSE { get; set; }

        /// <summary>應繳帳款</summary>
        public int ACCOUNTS_PAYABLE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>合計</summary>
        public int TOTAL { get; set; }
    }

    /// <summary>
    /// 企業乘車審核單 乘車明細 設定
    /// </summary>
    public class EnterpriseTaxiReviewDetailsConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>乘車券</summary>
        public string TICKET { get; set; }

        /// <summary>上車日期</summary>
        public string GET_ON_DATE { get; set; }

        /// <summary>下車日期</summary>
        public string GET_OFF_DATE { get; set; }

        /// <summary>上車時間</summary>
        public string GET_ON_TIME { get; set; }

        /// <summary>下車時間</summary>
        public string GET_OFF_TIME { get; set; }

        /// <summary>上車地點</summary>
        public string GET_ON_PLACE { get; set; }

        /// <summary>下車地點</summary>
        public string GET_OFF_PLACE { get; set; }

        /// <summary>補充上車地點</summary>
        public string COMPLEMENT_GET_ON_PLACE { get; set; }

        /// <summary>補充下車地點</summary>
        public string COMPLEMENT_GET_OFF_PLACE { get; set; }

        /// <summary>乘車目的</summary>
        public string TRAVEL_BY_PURPOSE { get; set; }

        /// <summary>車資</summary>
        public int TAXI_EXPENSES { get; set; }

        /// <summary>員工姓名</summary>
        public string NAME { get; set; }

        /// <summary>員工編號</summary>
        public string ACCOUNT_ID { get; set; }

        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>局處中心編號</summary>
        public string OFFICE_ID { get; set; }

        /// <summary>局處中心名稱</summary>
        public string OFFICE_NAME { get; set; }

        /// <summary>組別編號</summary>
        public string GROUP_ID { get; set; }

        /// <summary>組別名稱</summary>
        public string GROUP_NAME { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案年分</summary>
        public string PROJECT_USE_YEAR { get; set; }

        /// <summary>標記[空值標記：1]</summary>
        public int FLAG { get; set; }
    }

    /// <summary>
    /// 企業乘車審核單 會簽乘車人員 設定
    /// </summary>
    public class EnterpriseTaxiReviewApproversConfig : ApproversConfig
    {

    }

    /// <summary>
    /// 企業乘車審核單 使用預算 設定
    /// </summary>
    public class EnterpriseTaxiReviewBudgetsConfig : BudgetConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }
    }

    #region - 企業乘車審核單(審核/明細) -

    /// <summary>
    /// 企業乘車審核單(明細)(查詢條件)
    /// </summary>
    public class EnterpriseTaxiReviewDetailsQueryModel : EnterpriseTaxiReviewQueryModel
    {
        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>是否全部檢視</summary>
        public bool IS_ALL { get; set; }
    }

    /// <summary>
    /// 企業乘車審核單(審核/明細) 設定
    /// </summary>
    public class EnterpriseTaxiReviewDetailsViewModel: EnterpriseTaxiReviewQueryModel
    {
        /// <summary>企業乘車審核單 乘車明細 設定</summary>
        public List<EnterpriseTaxiReviewDetailsConfig> ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG { get; set; }

        /// <summary>企業乘車審核單 使用預算 設定</summary>
        public List<EnterpriseTaxiReviewBudgetsConfig> ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG { get; set; }
    }

    #endregion    
}