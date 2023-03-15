﻿using System;
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

    #region - 費用申請單 審核資訊_回傳ERP -

    /// <summary>
    /// 費用申請單 審核資訊_回傳ERP 
    /// </summary>
    public class ExpensesReimburseInfoRequest : ExpensesReimburseQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>費用申請單 表頭資訊</summary>
        public ExpensesReimburseTitle EXPENSES_REIMBURSE_TITLE { get; set; }

        /// <summary>費用申請單 表單內容 設定</summary>
        public ExpensesReimburseConfig EXPENSES_REIMBURSE_CONFIG { get; set; }

        /// <summary>費用申請單 費用明細 設定</summary>
        public IList<ExpensesReimburseDetailsConfig> EXPENSES_REIMBURSE_DTLS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }

    }

    #endregion

    #region - 行政採購類_回傳ERP資訊 -

    #region - 行政採購申請單 審核資訊_回傳ERP -

    /// <summary>
    /// 行政採購申請單 審核資訊_回傳ERP 
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

    #region - 行政採購點驗收單 審核資訊_回傳ERP -

    /// <summary>
    /// 行政採購點驗收單 審核資訊_回傳ERP 
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

    #region - 行政採購請款單 審核資訊_回傳ERP -

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

    #region - 內容評估表_回傳ERP資訊 -

    #region - 內容評估表 審核資訊_回傳ERP -

    public class EvaluateContentInfoRequest: EvaluateContentQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>內容評估表_外購 表頭資訊</summary>
        public EvaluateContentTitle EVALUATE_CONTENT_TITLE { get; set; }

        /// <summary>內容評估表_外購 表單內容 設定</summary>
        public EvaluateContentConfig EVALUATE_CONTENT_CONFIG { get; set; }

        /// <summary>內容評估表_外購 評估人員 設定</summary>
        public IList<EvaluateContentUsersConfig> EVALUATE_CONTENT_USERS_CONFIG { get; set; }

        /// <summary>內容評估表_外購 評估意見彙整 設定</summary>
        public IList<EvaluateContentEvaluatesConfig> EVALUATE_CONTENT_EVAS_CONFIG { get; set; }

        /// <summary>內容評估表_外購 決策意見彙整 設定</summary>
        public IList<EvaluateContentDecisionsConfig> EVALUATE_CONTENT_DECS_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }
    }

    #endregion

    #region - 內容評估表_補充意見 審核資訊_回傳ERP -

    /// <summary>
    /// 內容評估表_補充意見 審核資訊_回傳ERP
    /// </summary>
    public class EvaluateContent_ReplenishInfoRequest: EvaluateContent_ReplenishQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>內容評估表_外購 表頭資訊</summary>
        public EvaluateContent_ReplenishTitle EVALUATE_CONTENT_REPLENISH_TITLE { get; set; }

        /// <summary>內容評估表_外購 表單內容 設定</summary>
        public EvaluateContent_ReplenishConfig EVALUATE_CONTENT_REPLENISH_CONFIG { get; set; }

        /// <summary>內容評估表_外購 評估意見彙整 設定</summary>
        public IList<EvaluateContent_ReplenishEvaluatesConfig> EVALUATE_CONTENT_REPLENISH_EVAS_CONFIG { get; set; }

        /// <summary>內容評估表_外購 決策意見彙整 設定</summary>
        public IList<EvaluateContent_ReplenishDecisionsConfig> EVALUATE_CONTENT_REPLENISH_DECS_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }

    }

    #endregion

    #endregion

    #region - 版權採購類_回傳ERP資訊 -

    #region - 版權採購申請單 審核資訊_回傳ERP -

    /// <summary>
    /// 版權採購申請單 審核資訊_回傳ERP 
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

    #region - 版權採購交片單 審核資訊_回傳ERP -

    /// <summary>
    /// 版權採購交片單 審核資訊_回傳ERP
    /// </summary>
    public class MediaAcceptanceInfoRequest : MediaAcceptanceQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>版權採購交片單 表頭資訊</summary>
        public MediaAcceptanceTitle MEDIA_ACCEPTANCE_TITLE { get; set; }

        /// <summary>版權採購交片單 表單內容 設定</summary>
        public MediaAcceptanceConfig MEDIA_ACCEPTANCE_CONFIG { get; set; }

        /// <summary>版權採購交片單 驗收明細 設定</summary>
        public IList<MediaAcceptanceDetailsConfig> MEDIA_ACCEPTANCE_DTLS_CONFIG { get; set; }

        /// <summary>版權採購申請單 授權權利 設定</summary>
        public IList<MediaAcceptanceAuthorizesConfig> MEDIA_ACCEPTANCE_AUTHS_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }
    }

    #endregion

    #region - 版權採購請款單 審核資訊_回傳ERP -

    /// <summary>
    /// 版權採購請款單 審核資訊_回傳ERP
    /// </summary>
    public class MediaInvoiceInfoRequest : MediaInvoiceQueryModel
    {
        /// <summary>接收ERP回傳狀態</summary>
        public ErpResponseState ERP_RESPONSE_STATE { get; set; }

        /// <summary>版權採購請款單 表頭資訊</summary>
        public MediaInvoiceTitle MEDIA_INVOICE_TITLE { get; set; }

        /// <summary>版權採購請款單 表單內容 設定</summary>
        public MediaInvoiceConfig MEDIA_INVOICE_CONFIG { get; set; }

        /// <summary>版權採購請款單 驗收明細 設定</summary>
        public IList<MediaInvoiceAcceptancesConfig> MEDIA_INVOICE_ACPTS_CONFIG { get; set; }

        /// <summary>版權採購請款單 授權權利 設定</summary>
        public IList<MediaInvoiceAuthorizesConfig> MEDIA_INVOICE_AUTHS_CONFIG { get; set; }

        /// <summary>版權採購申請單 額外項目 設定</summary>
        public IList<MediaInvoiceExtrasConfig> MEDIA_INVOICE_EXS_CONFIG { get; set; }

        /// <summary>版權採購請款單 付款辦法 設定</summary>
        public IList<MediaInvoicePaymentsConfig> MEDIA_INVOICE_PYMTS_CONFIG { get; set; }

        /// <summary>版權採購請款單 使用預算 設定</summary>
        public IList<MediaInvoiceBudgetsConfig> MEDIA_INVOICE_BUDGS_CONFIG { get; set; }

        /// <summary>版權採購請款單 發票明細 設定</summary>
        public IList<MediaInvoiceDetailsConfig> MEDIA_INVOICE_DTLS_CONFIG { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }
    }

    #endregion

    #endregion

}