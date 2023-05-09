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
        ///// <summary>申請人資訊</summary>
        //public ApplicantInfo APPLICANT_INFO { get; set; }

        ///// <summary>版權採購退貨折讓單 表頭資訊</summary>
        //public MediaOrderReturnRefundTitle MEDIA_ORDER_RETURN_REFUND_TITLE { get; set; }

        ///// <summary>版權採購退貨折讓單 表單內容 設定</summary>
        //public MediaOrderReturnRefundConfig MEDIA_ORDER_RETURN_REFUND_CONFIG { get; set; }

        ///// <summary>版權採購退貨折讓單 驗收明細 設定</summary>
        //public IList<MediaOrderReturnRefundAcceptancesConfig> MEDIA_ORDER_RETURN_REFUND_ACPTS_CONFIG { get; set; }

        ///// <summary>版權採購退貨折讓單 授權權利 設定</summary>
        //public IList<MediaOrderReturnRefundAuthorizesConfig> MEDIA_ORDER_RETURN_REFUND_AUTHS_CONFIG { get; set; }

        ///// <summary>版權採購申請單 額外項目 設定</summary>
        //public IList<MediaOrderReturnRefundExtrasConfig> MEDIA_ORDER_RETURN_REFUND_EXS_CONFIG { get; set; }

        ///// <summary>版權採購退貨折讓單 付款辦法 設定</summary>
        //public IList<MediaOrderReturnRefundPaymentsConfig> MEDIA_ORDER_RETURN_REFUND_PYMTS_CONFIG { get; set; }

        ///// <summary>版權採購退貨折讓單 使用預算 設定</summary>
        //public IList<MediaOrderReturnRefundBudgetsConfig> MEDIA_ORDER_RETURN_REFUND_BUDGS_CONFIG { get; set; }

        ///// <summary>版權採購退貨折讓單 憑證明細 設定</summary>
        //public IList<MediaOrderReturnRefundInvoicesConfig> MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG { get; set; }

        ///// <summary>版權採購退貨折讓單 退貨商品明細 設定</summary>
        //public IList<MediaOrderReturnRefundCommoditysConfig> MEDIA_ORDER_RETURN_REFUND_COMMS_CONFIG { get; set; }

        ///// <summary>表單關聯</summary>
        //public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

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

        /// <summary>版權採購 總金額</summary>
        public double MEDIA_ORDER_DTL_ORDER_TOTAL { get; set; }

        /// <summary>版權採購 總金額_台幣</summary>
        public int MEDIA_ORDER_DTL_ORDER_TOTAL_TWD { get; set; }

        /// <summary>交易類型</summary>
        public string TXN_TYPE { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>營業稅/[稅率]租稅協定</summary>
        public double TAX_TATE { get; set; }

        /// <summary>退款金額</summary>
        public double REFUND_AMOUNT { get; set; }

        /// <summary>退款金額_台幣</summary>
        public int REFUND_AMOUNT_TWD { get; set; }

        /// <summary>含稅總額</summary>
        public double TAX_INCL_TOTAL { get; set; }

        /// <summary>含稅總額_台幣</summary>
        public int TAX_INCL_TOTAL_TWD { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>匯率</summary>
        public double EXCH_RATE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }

        /// <summary>廠商名稱</summary>
        public string SUP_NAME { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>負責人</summary>
        public string OWNER_NAME { get; set; }

        /// <summary>負責人電話</summary>
        public string OWNER_TEL { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }

        /// <summary>合計未稅金額(銷售明細)/NET總額(銷售明細)</summary>
        public double DTL_NET_TOTAL { get; set; }

        /// <summary>合計未稅金額_台幣(銷售明細)/NET總額_台幣(銷售明細)</summary>
        public int DTL_NET_TOTAL_TWD { get; set; }

        /// <summary>總稅額/總預估保留稅額(銷售明細)</summary>
        public double DTL_TAX_TOTAL { get; set; }

        /// <summary>總稅額/總預估保留稅額_台幣(銷售明細)</summary>
        public int DTL_TAX_TOTAL_TWD { get; set; }

        /// <summary>合計含稅總額(銷售明細)/GROSS總額(銷售明細)</summary>
        public double DTL_GROSS_TOTAL { get; set; }

        /// <summary>合計含稅總額_台幣(銷售明細)/GROSS總額_台幣(銷售明細)</summary>
        public int DTL_GROSS_TOTAL_TWD { get; set; }

        /// <summary>材料費總價(採購明細)</summary>
        public double DTL_MATERIAL_TOTAL { get; set; }

        /// <summary>材料費總價_台幣(採購明細)</summary>
        public int DTL_MATERIAL_TOTAL_TWD { get; set; }

        /// <summary>合計(採購明細)</summary>
        public double DTL_ORDER_TOTAL { get; set; }

        /// <summary>合計_台幣(採購明細)</summary>
        public int DTL_ORDER_TOTAL_TWD { get; set; }

        /// <summary>額外採購項目 總額(額外採購項目)</summary>
        public double EX_AMOUNT_TOTAL { get; set; }

        /// <summary>額外採購項目 總額_台幣(額外採購項目)</summary>
        public int EX_AMOUNT_TOTAL_TWD { get; set; }

        /// <summary>額外採購項目 總稅額/總預估保留稅額(額外採購項目)</summary>
        public double EX_TAX_TOTAL { get; set; }

        /// <summary>額外採購項目 總稅額/總預估保留稅額_台幣(額外採購項目)</summary>
        public int EX_TAX_TOTAL_TWD { get; set; }

        /// <summary>本期付款總額(付款辦法)</summary>
        public double PYMT_CURRENT_TOTAL { get; set; }

        /// <summary>本期付款總額_台幣(付款辦法)</summary>
        public int PYMT_CURRENT_TOTAL_TWD { get; set; }

        /// <summary>額外採購項目 總稅額/總預估保留稅額(付款辦法)</summary>
        public double PYMT_EX_TAX_TOTAL { get; set; }

        /// <summary>額外採購項目 總稅額/總預估保留稅額_台幣(付款辦法)</summary>
        public int PYMT_EX_TAX_TOTAL_TWD { get; set; }

        /// <summary>退款總額</summary>
        public double INV_AMOUNT_TOTAL { get; set; }

        /// <summary>退款總額_台幣</summary>
        public int INV_AMOUNT_TOTAL_TWD { get; set; }

        /// <summary>退款總預估保留稅</summary>
        public double INV_TAX_TOTAL { get; set; }

        /// <summary>退款總預估保留稅_台幣</summary>
        public int INV_TAX_TOTAL_TWD { get; set; }

        /// <summary>處理方式</summary>
        public string PROCESS_METHOD { get; set; }

        /// <summary>其他類別</summary>
        public string FINANC_NOTE { get; set; }
    }

    ///// <summary>
    ///// 版權採購退貨折讓單 驗收明細 設定
    ///// </summary>
    //public class MediaOrderReturnRefundAcceptancesConfig:MediaInvoiceAcceptancesConfig
    //{

    //}

    ///// <summary>
    ///// 版權採購退貨折讓單 授權權利 設定
    ///// </summary>
    //public class MediaOrderReturnRefundAuthorizesConfig: MediaInvoiceAuthorizesConfig
    //{

    //}

    ///// <summary>
    ///// 版權採購退貨折讓單 額外項目 設定
    ///// </summary>
    //public class MediaOrderReturnRefundExtrasConfig: MediaInvoiceExtrasConfig
    //{
    //    /// <summary>行數編號</summary>
    //    public int ROW_NO { get; set; }

    //    /// <summary>名稱</summary>
    //    public string NAME { get; set; }

    //    /// <summary>金額</summary>
    //    public double AMOUNT { get; set; }

    //    /// <summary>金額_台幣</summary>
    //    public int AMOUNT_TWD { get; set; }

    //    /// <summary>稅額/預估保留稅額</summary>
    //    public double TAX { get; set; }

    //    /// <summary>稅額/預估保留稅額_台幣</summary>
    //    public int TAX_TWD { get; set; }

    //    /// <summary>期別</summary>
    //    public int PERIOD { get; set; }

    //    /// <summary>所屬專案 ERP 單號</summary>
    //    public string PROJECT_FORM_NO { get; set; }

    //    /// <summary>所屬專案名稱</summary>
    //    public string PROJECT_NAME { get; set; }

    //    /// <summary>所屬專案描述</summary>
    //    public string PROJECT_NICKNAME { get; set; }

    //    /// <summary>所屬專案年分</summary>
    //    public string PROJECT_USE_YEAR { get; set; }

    //    /// <summary>備註</summary>
    //    public string NOTE { get; set; }
    //}

    ///// <summary>
    ///// 版權採購退貨折讓單 付款辦法 設定
    ///// </summary>
    //public class MediaOrderReturnRefundPaymentsConfig: MediaOrderPaymentsConfig
    //{

    //}

    ///// <summary>
    ///// 版權採購退貨折讓單 使用預算 設定
    ///// </summary>
    //public class MediaOrderReturnRefundBudgetsConfig: MediaInvoiceBudgetsConfig
    //{
    //    /// <summary>期別</summary>
    //    public int PERIOD { get; set; }

    //    /// <summary>預算 ERP唯一碼</summary>
    //    public string BUDG_FORM_NO { get; set; }

    //    /// <summary>預算編列年度</summary>
    //    public string BUDG_CREATE_YEAR { get; set; }

    //    /// <summary>預算名稱</summary>
    //    public string BUDG_NAME { get; set; }

    //    /// <summary>所屬部門</summary>
    //    public string OWNER_DEPT { get; set; }

    //    /// <summary>預算總額</summary>
    //    public int BUDG_TOTAL { get; set; }

    //    /// <summary>可用預算金額</summary>
    //    public int BUDG_AVAILABLE_BUDGET_AMOUNT { get; set; }

    //    /// <summary>使用預算金額</summary>
    //    public int BUDG_USE_BUDGET_AMOUNT { get; set; }
    //}

    ///// <summary>
    ///// 版權採購退貨折讓單 憑證明細 設定
    ///// </summary>

    //public class MediaOrderReturnRefundInvoicesConfig
    //{
    //    /// <summary>行數編號</summary>
    //    public int ROW_NO { get; set; }

    //    /// <summary>憑證號碼</summary>
    //    public string INV_NUM { get; set; }

    //    /// <summary>憑證日期</summary>
    //    public string INV_DATE { get; set; }

    //    /// <summary>金額</summary>
    //    public double AMOUNT { get; set; }

    //    /// <summary>金額_台幣</summary>
    //    public int AMOUNT_TWD { get; set; }

    //    /// <summary>退款未稅金額</summary>
    //    public double NET { get; set; }

    //    /// <summary>退款未稅金額_台幣</summary>
    //    public int NET_TWD { get; set; }

    //    /// <summary>退款含稅金額</summary>
    //    public double GROSS { get; set; }

    //    /// <summary>退款含稅金額_台幣</summary>
    //    public int GROSS_TWD { get; set; }

    //    /// <summary>退款稅額</summary>
    //    public double TAX { get; set; }

    //    /// <summary>退款稅額_台幣</summary>
    //    public int TAX_TWD { get; set; }

    //    /// <summary>備註</summary>
    //    public string NOTE { get; set; }
    //}

    /// <summary>
    /// 版權採購退貨折讓單 退貨商品明細 設定
    /// </summary>
    public class MediaOrderReturnRefundCommoditysConfig
    {
        /// <summary>憑證號碼</summary>
        public string INV_NUM { get; set; }

        /// <summary>訂單行數編號</summary>
        public int ORDER_ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>影帶規格</summary>
        public string MEDIA_SPEC { get; set; }

        /// <summary>影片類型</summary>
        public string MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int END_EPISODE { get; set; }

        /// <summary>總集數</summary>
        public int ORDER_EPISODE { get; set; }

        /// <summary>驗收集數</summary>
        public int ACPT_EPISODE { get; set; }

        /// <summary>每集長度</summary>
        public int EPISODE_TIME { get; set; }
    }
}