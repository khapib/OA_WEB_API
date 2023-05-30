using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 版權採購退貨折讓單
/// </summary>

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 版權採購退貨折讓單(查詢條件)
    /// </summary>
    public class MediaOrderReturnRefundQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 版權採購退貨折讓單
    /// </summary>
    public class MediaOrderReturnRefundViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>版權採購退貨折讓單 表頭資訊</summary>
        public MediaOrderReturnRefundTitle MEDIA_ORDER_RETURN_REFUND_TITLE { get; set; }

        /// <summary>版權採購退貨折讓單 表單內容 設定</summary>
        public MediaOrderReturnRefundConfig MEDIA_ORDER_RETURN_REFUND_CONFIG { get; set; }

        /// <summary>版權採購退貨折讓單 已退貨商品明細 設定</summary>
        public List<MediaOrderReturnRefundAlreadyRefundCommoditysConfig> MEDIA_ORDER_RETURN_REFUND_ALDY_RF_COMMS_CONFIG { get; set; }

        /// <summary>版權採購退貨折讓單 退貨商品明細 設定</summary>
        public List<MediaOrderReturnRefundRefundCommoditysConfig> MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG { get; set; }

        /// <summary>版權採購退貨折讓單 憑證退款明細 設定</summary>
        public List<MediaOrderReturnRefundInvoicesConfig> MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG { get; set; }

        /// <summary>版權採購退貨折讓單 憑證退款細項 設定</summary>
        public List<MediaOrderReturnRefundAlreadyInvoiceDetailsConfig> MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG { get; set; }

        /// <summary>版權採購退貨折讓單 憑證退款細項 設定</summary>
        public List<MediaOrderReturnRefundInvoiceDetailsConfig> MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 版權採購退貨折讓單 表頭資訊
    /// </summary>
    public class MediaOrderReturnRefundTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 版權採購退貨折讓單 表單內容 設定
    /// </summary>
    public class MediaOrderReturnRefundConfig
    {
        /// <summary>版權請款 系統編號</summary>
        public string MEDIA_INVOICE_REQUISITION_ID { get; set; }

        /// <summary>版權請款 主旨</summary>
        public string MEDIA_INVOICE_SUBJECT { get; set; }

        /// <summary>版權請款 BPM 表單單號</summary>
        public string MEDIA_INVOICE_BPM_FORM_NO { get; set; }

        /// <summary>版權請款 ERP 表單唯一碼</summary>
        public string MEDIA_INVOICE_ERP_FORM_NO { get; set; }

        /// <summary>版權請款 路徑</summary>
        public string MEDIA_INVOICE_PATH { get; set; }

        /// <summary>版權採購 採購單 付款辦法總額</summary>
        public double MEDIA_ORDER_PYMT_ORDER_TOTAL { get; set; }

        /// <summary>版權採購 採購單 付款辦法總額_台幣(換算)</summary>
        public int MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV { get; set; }

        /// <summary>版權請款 期別</summary>
        public int PERIOD { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>免稅總額(退貨明細)</summary>
        public double RF_EXCL_TOTAL { get; set; }

        /// <summary>免稅總額_台幣(退貨明細)</summary>
        public int RF_EXCL_TOTAL_TWD { get; set; }

        /// <summary>總稅額(退貨明細)</summary>
        public double RF_TAX_TOTAL { get; set; }

        /// <summary>總稅額_台幣(退貨明細)</summary>
        public int RF_TAX_TOTAL_TWD { get; set; }

        /// <summary>未稅總額(退貨明細)</summary>
        public double RF_NET_TOTAL { get; set; }

        /// <summary>未稅總額_台幣(退貨明細)</summary>
        public int RF_NET_TOTAL_TWD { get; set; }

        /// <summary>含稅總額(退貨明細)</summary>
        public double RF_GROSS_TOTAL { get; set; }

        /// <summary>含稅總額_台幣(退貨明細)</summary>
        public int RF_GROSS_TOTAL_TWD { get; set; }

        /// <summary>合計金額(退貨明細)</summary>
        public double RF_AMOUNT_TOTAL { get; set; }

        /// <summary>合計金額_台幣(退貨明細)</summary>
        public int RF_AMOUNT_TOTAL_TWD { get; set; }

        /// <summary>處理方式</summary>
        public string PROCESS_METHOD { get; set; }

        /// <summary>其他類別</summary>
        public string FINANC_NOTE { get; set; }
    }

    /// <summary>
    /// 版權採購退貨折讓單 已退貨商品明細 設定
    /// </summary>
    public class MediaOrderReturnRefundAlreadyRefundCommoditysConfig : MediaOrderReturnRefundRefundCommoditysConfig
    {

    }

    /// <summary>
    /// 版權採購退貨折讓單 退貨商品明細 設定
    /// </summary>
    public class MediaOrderReturnRefundRefundCommoditysConfig: MediaCommodityConfig
    {

    }

    /// <summary>
    /// 版權採購退貨折讓單 憑證退款明細 設定
    /// </summary>
    public class MediaOrderReturnRefundInvoicesConfig : InvoiceConfig
    {

    }

    /// <summary>
    /// 版權採購退貨折讓單 憑證已退款細項 設定
    /// </summary>
    public class MediaOrderReturnRefundAlreadyInvoiceDetailsConfig : InvoiceDetailConfig
    {

    }

    /// <summary>
    /// 版權採購退貨折讓單 憑證退款細項 設定
    /// </summary>
    public class MediaOrderReturnRefundInvoiceDetailsConfig : InvoiceDetailConfig
    {

    }

}