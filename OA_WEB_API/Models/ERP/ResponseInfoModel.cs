using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

/// <summary>
/// 回傳ERP資訊
/// </summary>
namespace OA_WEB_API.Models.ERP
{
    #region 回傳ERP BPM內容資訊

    /// <summary>
    /// 回傳ERP BPM內容資訊
    /// </summary>
    public class RequestQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>確認是否要回傳</summary>
        public bool REQUEST_FLG { get; set; }
    }

    #endregion

    #region - 專案建立審核單 財務審核資訊_回傳ERP -

    /// <summary>
    /// 專案建立審核單 財務審核資訊_回傳ERP 設定
    /// </summary>
    public class ProjectReviewFinanceConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>專案類型</summary>
        public string ACC_CATEGORY { get; set; }
    }

    /// <summary>
    /// 專案建立審核單 財務審核資訊_回傳ERP 
    /// </summary>
    public class ProjectReviewFinanceRequest
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>系統編號</summary>
        public string RequisitionID { get; set; }

        /// <summary>專案類型</summary>
        public string AccCategory { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }

    }

    #endregion

    #region - 行政採購類_回傳ERP資訊 -

    #region - 行政採購申請單 申請審核資訊_回傳ERP -

    /// <summary>
    /// 行政採購申請單 申請審核資訊_回傳ERP 
    /// </summary>
    public class GeneralOrderInfoRequest : GeneralOrderQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>行政採購申請 表頭資訊</summary>
        public GeneralOrderTitle GENERAL_ORDER_TITLE { get; set; }

        /// <summary>行政採購申請 設定</summary>
        public GeneralOrderConfig GENERAL_ORDER_CONFIG { get; set; }

        /// <summary>行政採購申請 採購明細 設定</summary>
        public IList<GeneralOrderDetailsConfig> GENERAL_ORDER_DETAILS_CONFIG { get; set; }

        /// <summary>行政採購申請 付款辦法 設定</summary>
        public IList<GeneralOrderPaymentsConfig> GENERAL_ORDER_PAYMENTS_CONFIG { get; set; }

        /// <summary>行政採購申請 使用預算 設定</summary>
        public IList<GeneralOrderBudgetsConfig> GENERAL_ORDER_BUDGETS_CONFIG { get; set; }

        /// <summary>行政採購申請 驗收項目 設定</summary>
        public IList<GeneralOrderAcceptancesConfig> GENERAL_ORDER_ACPT_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }

    }

    #endregion
        
    #region - 行政採購點驗收單 驗收審核資訊_回傳ERP -

    /// <summary>
    /// 行政採購點驗收單 驗收審核資訊_回傳ERP 
    /// </summary>
    public class GeneralAcceptanceInfoRequest : GeneralAcceptanceQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>行政採購點驗收單 表頭資訊</summary>
        public GeneralAcceptanceTitle GENERAL_ACCEPTANCE_TITLE { get; set; }

        /// <summary>行政採購點驗收單 驗收明細 設定</summary>
        public IList<GeneralAcceptanceDetailsConfig> GENERAL_ACCEPTANCE_DETAILS_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }
    }

    #endregion

    #region - 行政採購請款單 財務審核資訊_回傳ERP -

    /// <summary>
    /// 行政採購請款單 財務審核資訊_回傳ERP
    /// </summary>
    public class GeneralInvoiceInfoRequest : GeneralInvoiceQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>行政採購請款單 表頭資訊</summary>
        public GeneralInvoiceTitle GENERAL_INVOICE_TITLE { get; set; }

        /// <summary>行政採購請款單 表單內容 設定</summary>
        public GeneralInvoiceConfig GENERAL_INVOICE_CONFIG { get; set; }

        /// <summary>行政採購請款單 付款辦法 設定</summary>
        public IList<GeneralInvoicePaymentsConfig> GENERAL_INVOICE_PAYMENTS_CONFIG { get; set; }

        /// <summary>行政採購申請 發票明細 設定</summary>
        public IList<GeneralInvoiceDetailsConfig> GENERAL_INVOICE_DETAILS_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }
    }

    #endregion

    #endregion

    #region - 版權採購類_回傳ERP資訊 -

    #region - 版權採購申請單 申請審核資訊_回傳ERP -

    /// <summary>
    /// 版權採購申請單 申請審核資訊_回傳ERP 
    /// </summary>
    public class MediaOrderInfoRequest : MediaOrderQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>版權採購申請 表頭資訊</summary>
        public MediaOrderTitle MEDIA_ORDER_TITLE { get; set; }

        /// <summary>版權採購申請 設定</summary>
        public MediaOrderConfig MEDIA_ORDER_CONFIG { get; set; }

        /// <summary>版權採購申請單 採購明細 設定</summary>
        public IList<MediaOrderDetailsConfig> MEDIA_ORDER_DTLS_CONFIG { get; set; }

        /// <summary>版權採購申請單 授權權利 設定</summary>
        public IList<MediaOrderAuthorizesConfig> MEDIA_ORDER_AUTHS_CONFIG { get; set; }

        /// <summary>版權採購申請單 額外項目 設定</summary>
        public IList<MediaOrderExtrasConfig> MEDIA_ORDER_EXS_CONFIG { get; set; }

        /// <summary>版權採購申請單 付款辦法 設定</summary>
        public IList<MediaOrderPaymentsConfig> MEDIA_ORDER_PYMTS_CONFIG { get; set; }

        /// <summary>版權採購申請單 使用預算 設定</summary>
        public IList<MediaOrderBudgetsConfig> MEDIA_ORDER_BUDGS_CONFIG { get; set; }

        /// <summary>版權採購申請單 驗收項目 設定</summary>
        public IList<MediaOrderAcceptancesConfig> MEDIA_ORDER_ACPTS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }

    }

    #endregion

    #endregion

}