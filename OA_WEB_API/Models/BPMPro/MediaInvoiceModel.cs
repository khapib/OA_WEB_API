using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 版權採購請款單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 版權採購請款單(查詢條件)
    /// </summary>
    public class MediaInvoiceQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 版權採購請款單
    /// </summary>
    public class MediaInvoiceViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>版權採購請款單 表頭資訊</summary>
        public MediaInvoiceTitle MEDIA_INVOICE_TITLE { get; set; }

        /// <summary>版權採購請款單 表單內容 設定</summary>
        public MediaInvoiceConfig MEDIA_INVOICE_CONFIG { get; set; }

        /// <summary>版權採購請款單 驗收明細 設定</summary>
        public IList<MediaInvoiceAcceptancesConfig> MEDIA_INVOICE_ACPTS_CONFIG { get; set; }

        /// <summary>版權採購請款單 授權權利 設定</summary>
        public IList<MediaInvoiceAuthorizesConfig> MEDIA_INVOICE_AUTHS_CONFIG { get; set; }

        /// <summary>版權採購請款單 額外項目 設定</summary>
        public IList<MediaInvoiceExtrasConfig> MEDIA_INVOICE_EXS_CONFIG { get; set; }

        /// <summary>版權採購請款單 付款辦法 設定</summary>
        public IList<MediaInvoicePaymentsConfig> MEDIA_INVOICE_PYMTS_CONFIG { get; set; }

        /// <summary>版權採購請款單 使用預算 設定</summary>
        public IList<MediaInvoiceBudgetsConfig> MEDIA_INVOICE_BUDGS_CONFIG { get; set; }

        /// <summary>版權採購請款單 憑證明細 設定</summary>
        public List<MediaInvoiceInvoicesConfig> MEDIA_INVOICE_INVS_CONFIG { get; set; }

        /// <summary>版權採購請款單 憑證細項 設定</summary>
        public List<MediaInvoiceInvoiceDetailsConfig> MEDIA_INVOICE_INV_DTLS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 版權採購請款單 表頭資訊
    /// </summary>
    public class MediaInvoiceTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 版權採購請款單 表單內容 設定
    /// </summary>
    public class MediaInvoiceConfig : COMM_Bank
    {
        /// <summary>版權採購 系統編號</summary>
        public string MEDIA_ORDER_REQUISITION_ID { get; set; }

        /// <summary>版權採購 主旨</summary>
        public string MEDIA_ORDER_SUBJECT { get; set; }

        /// <summary>版權採購 BPM 表單單號</summary>
        public string MEDIA_ORDER_BPM_FORM_NO { get; set; }

        /// <summary>版權採購 ERP 表單唯一碼</summary>
        public string MEDIA_ORDER_ERP_FORM_NO { get; set; }

        /// <summary>版權採購 路徑</summary>
        public string MEDIA_ORDER_PATH { get; set; }

        /// <summary>版權採購 交易類型</summary>
        public string MEDIA_ORDER_TXN_TYPE { get; set; }

        /// <summary>版權採購 採購單 付款辦法總額</summary>
        public double MEDIA_ORDER_PYMT_ORDER_TOTAL { get; set; }

        /// <summary>版權採購 採購單 付款辦法總額_台幣(換算)</summary>
        public int MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV { get; set; }

        /// <summary>版權採購點驗收單 系統編號</summary>
        public string MEDIA_ACCEPTANCE_REQUISITION_ID { get; set; }
        
        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>預計匯率</summary>
        public double PRE_RATE { get; set; }

        /// <summary>計價方式</summary>
        public string PRICING_METHOD { get; set; }

        /// <summary>營業稅/[稅率]租稅協定</summary>
        public double TAX_RATE { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>員工代墊</summary>
        public string REIMBURSEMENT { get; set; }

        /// <summary>代墊員工部門編號</summary>
        public string REIMB_STAFF_DEPT_ID { get; set; }

        /// <summary>代墊員工部門名稱</summary>
        public string REIMB_STAFF_DEPT_NAME { get; set; }

        /// <summary>代墊員工編號</summary>
        public string REIMB_STAFF_ID { get; set; }

        /// <summary>代墊員工姓名</summary>
        public string REIMB_STAFF_NAME { get; set; }

        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }

        /// <summary>廠商名稱</summary>
        public string SUP_NAME { get; set; }

        /// <summary>
        /// 支付方式：
        /// CS.現金
        /// PT_AC.薪轉帳戶
        /// D_AC.指定帳戶
        /// SUP_AC.合作夥伴帳戶
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>銀行往來編號</summary>
        public string SUP_TX_ID { get; set; }

        /// <summary>
        /// 憑證類型：
        /// GUI.統一發票、
        /// Invoice. Invoice、
        /// RECPT.收據
        /// </summary>
        public string INVOICE_TYPE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>合計未稅金額(採購明細)/NET總額(採購明細)</summary>
        public double DTL_NET_TOTAL { get; set; }

        /// <summary>合計未稅金額_台幣(採購明細)/NET總額_台幣(採購明細)</summary>
        public int DTL_NET_TOTAL_TWD { get; set; }

        /// <summary>總稅額/總預估保留稅額(採購明細)</summary>
        public double DTL_TAX_TOTAL { get; set; }

        /// <summary>總稅額/總預估保留稅額_台幣(採購明細)</summary>
        public int DTL_TAX_TOTAL_TWD { get; set; }

        /// <summary>合計含稅總額(採購明細)/GROSS總額(採購明細)</summary>
        public double DTL_GROSS_TOTAL { get; set; }

        /// <summary>合計含稅總額_台幣(採購明細)/GROSS總額_台幣(採購明細)</summary>
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

        /// <summary>付款辦法 本期付款總額(付款辦法)</summary>
        public double PYMT_CURRENT_TOTAL { get; set; }

        /// <summary>付款辦法 本期付款總額_台幣(付款辦法)</summary>
        public int PYMT_CURRENT_TOTAL_TWD { get; set; }

        /// <summary>憑證明細 合計</summary>
        public double INV_AMOUNT_TOTAL { get; set; }

        /// <summary>憑證明細 合計_台幣</summary>
        public int INV_AMOUNT_TOTAL_TWD { get; set; }

        /// <summary>憑證明細 總稅額/總預估保留稅額(憑證明細)</summary>
        public double INV_TAX_TOTAL { get; set; }

        /// <summary>憑證明細 總稅額/總預估保留稅額_台幣(憑證明細)</summary>
        public int INV_TAX_TOTAL_TWD { get; set; }

        /// <summary>實際支付金額</summary>
        public int ACTUAL_PAY_AMOUNT { get; set; }

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
    /// 版權採購請款單 驗收明細 設定
    /// </summary>
    public class MediaInvoiceAcceptancesConfig : MediaOrderAcceptancesConfig
    {
        /// <summary>每集長度</summary>
        public int EPISODE_TIME { get; set; }

        /// <summary>影帶規格</summary>
        public string MEDIA_SPEC { get; set; }

        /// <summary>未稅單價/NET單價</summary>
        public double NET { get; set; }

        /// <summary>未稅單價_台幣/NET單價_台幣</summary>
        public int NET_TWD { get; set; }

        /// <summary>稅額/總預估保留稅額</summary>
        public double TAX { get; set; }

        /// <summary>稅額/總預估保留稅額_台幣</summary>
        public int TAX_TWD { get; set; }

        /// <summary>含稅單價/GROSS單價</summary>
        public double GROSS { get; set; }

        /// <summary>含稅單價_台幣/GROSS單價_台幣</summary>
        public int GROSS_TWD { get; set; }

        /// <summary>未稅小計/NET小計</summary>
        public double NET_SUM { get; set; }

        /// <summary>未稅小計_台幣/NET小計_台幣</summary>
        public int NET_SUM_TWD { get; set; }

        /// <summary>含稅小計/GROSS小計</summary>
        public double GROSS_SUM { get; set; }

        /// <summary>含稅小計_台幣/GROSS小計_台幣</summary>
        public int GROSS_SUM_TWD { get; set; }

        /// <summary>單集材料費</summary>
        public double MATERIAL { get; set; }

        /// <summary>明細單項小記</summary>
        public double ITEM_SUM { get; set; }

        /// <summary>明細單項小記_台幣</summary>
        public int ITEM_SUM_TWD { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案起案年度</summary>
        public string PROJECT_USE_YEAR { get; set; }
    }

    /// <summary>
    /// 版權採購請款單 授權權利 設定
    /// </summary>
    public class MediaInvoiceAuthorizesConfig : MediaOrderAuthorizesConfig
    {

    }

    /// <summary>
    /// 版權採購請款單 額外項目 設定
    /// </summary>
    public class MediaInvoiceExtrasConfig: MediaOrderExtrasConfig
    {

    }

    /// <summary>
    /// 版權採購請款單 付款辦法 設定
    /// </summary>
    public class MediaInvoicePaymentsConfig : MediaOrderPaymentsConfig
    {
        /// <summary>會計類別</summary>
        public string ACCT_CATEGORY { get; set; }
    }

    /// <summary>
    /// 版權採購請款單 使用預算 設定
    /// </summary>
    public class MediaInvoiceBudgetsConfig : MediaOrderBudgetsConfig
    {

    }

    /// <summary>
    /// 版權採購請款單 憑證明細 設定
    /// </summary>
    public class MediaInvoiceInvoicesConfig : InvoiceConfig
    {

    }

    /// <summary>
    /// 版權採購請款單 憑證細項 設定
    /// </summary>
    public class MediaInvoiceInvoiceDetailsConfig : InvoiceDetailConfig
    {

    }
}