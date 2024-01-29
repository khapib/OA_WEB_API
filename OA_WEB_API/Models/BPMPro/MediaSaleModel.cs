using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

/// <summary>
/// 會簽管理系統 - 版權銷售申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 版權銷售申請單(查詢條件)
    /// </summary>
    public class MediaSaleQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單(查詢)
    /// </summary>
    public class MediaSaleViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>版權銷售申請單 表頭資訊</summary>
        public MediaSaleTitle MEDIA_SALE_TITLE { get; set; }

        /// <summary>版權銷售申請單 表單內容 設定</summary>
        public MediaSaleConfig MEDIA_SALE_CONFIG { get; set; }

        /// <summary>版權銷售申請單 稅率結構 設定</summary>
        public IList<MediaSaleTaxRateStructuresConfig> MEDIA_SALE_TRSS_CONFIG { get; set; }

        /// <summary>版權銷售申請單 採購明細 設定</summary>
        public IList<MediaSaleDetailsConfig> MEDIA_SALE_DTLS_CONFIG { get; set; }

        /// <summary>版權銷售申請單 授權權利 設定</summary>
        public IList<MediaSaleAuthorizesConfig> MEDIA_SALE_AUTHS_CONFIG { get; set; }

        /// <summary>版權銷售申請單 額外項目 設定</summary>
        public IList<MediaSaleExtrasConfig> MEDIA_SALE_EXS_CONFIG { get; set; }

        /// <summary>版權銷售申請單 收款辦法 設定</summary>
        public IList<MediaSaleCollectionsConfig> MEDIA_SALE_COLLS_CONFIG { get; set; }

        /// <summary>版權銷售申請單 使用預算 設定</summary>
        public List<MediaSaleBudgetsConfig> MEDIA_SALE_BUDGS_CONFIG { get; set; }

        /// <summary>版權銷售申請單 交付項目 設定</summary>
        public IList<MediaSaleDeliverysConfig> MEDIA_SALE_DELYS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 表頭資訊
    /// </summary>
    public class MediaSaleTitle : ImplementHeader
    {
        /// <summary>編輯註記</summary>
        public string EDIT_FLAG { get; set; }

        /// <summary>異動原單系統編號</summary>
        public string GROUP_ID { get; set; }

        /// <summary>異動原單BPM 表單單號</summary>
        public string GROUP_BPM_FORM_NO { get; set; }

        /// <summary>異動原單BPM 表單路徑</summary>
        public string GROUP_PATH { get; set; }

        /// <summary>母表單系統編號</summary>
        public string PARENT_ID { get; set; }

        /// <summary>母表單BPM 表單單號</summary>
        public string PARENT_BPM_FORM_NO { get; set; }

        /// <summary>表單操作</summary>
        public string FORM_ACTION { get; set; }

        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 表單內容 設定
    /// </summary>
    public class MediaSaleConfig
    {
        /// <summary>說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>是否過副總</summary>
        public string IS_VICE_PRESIDENT { get; set; }

        /// <summary>交易類型</summary>
        public string TXN_TYPE { get; set; }

        /// <summary>作業日期</summary>
        public Nullable<DateTime> EXEC_DATE { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>匯率</summary>
        public double EXCH_RATE { get; set; }

        /// <summary>提供稅單[標記]</summary>
        public string IS_TAX_BILL { get; set; }

        /// <summary>稅單備註</summary>
        public string TAX_NOTE { get; set; }

        /// <summary>計價方式</summary>
        public string PRICING_METHOD { get; set; }

        /// <summary>已知GROSS價[標記]</summary>
        public string KNOW_GROSS { get; set; }

        /// <summary>營業稅/[稅率]租稅協定</summary>
        public double TAX_RATE { get; set; }

        /// <summary>合計稅率(稅率結構)</summary>
        public double TRS_RATE_TOTAL { get; set; }

        /// <summary>合計稅額(稅率結構)</summary>
        public double TRS_TAX_TOTAL { get; set; }

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

        /// <summary>總收款期數</summary>
        public int COLLECTION_PERIOD_TOTAL { get; set; }

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

        /// <summary>額外項目總額(額外項目)</summary>
        public double EX_AMOUNT_TOTAL { get; set; }

        /// <summary>額外項目總額_台幣(額外項目)</summary>
        public int EX_AMOUNT_TOTAL_TWD { get; set; }

        /// <summary>不可異動標住(收款辦法)</summary>
        public string COLL_LOCK_PERIOD { get; set; }

        /// <summary>合計未稅金額(收款辦法)/NET總額(收款辦法)</summary>
        public double COLL_NET_TOTAL { get; set; }

        /// <summary>合計含稅總額(收款辦法)/GROSS總額(收款辦法)</summary>
        public double COLL_GROSS_TOTAL { get; set; }

        /// <summary>材料費總價(收款辦法)</summary>
        public double COLL_MATERIAL_TOTAL { get; set; }

        /// <summary>額外項目總額(收款辦法)</summary>
        public double COLL_EX_AMOUNT_TOTAL { get; set; }        

        /// <summary>合計(收款辦法)</summary>
        public double COLL_ORDER_TOTAL { get; set; }

        /// <summary>合計(換算)(收款辦法)</summary>
        public int COLL_ORDER_TOTAL_CONV { get; set; }

        /// <summary>合計使用預算(所需預算)(收款辦法)</summary>
        public int COLL_USE_BUDGET_TOTAL { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 稅率結構 設定
    /// </summary>
    public class MediaSaleTaxRateStructuresConfig
    {
        /// <summary>名目</summary>
        public string NAME { get; set; }

        /// <summary>稅率[占比]</summary>
        public double RATE { get; set; }

        /// <summary>稅額</summary>
        public double TAX { get; set; }

        /// <summary>稅額_台幣</summary>
        public int TAX_TWD { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 採購明細 設定
    /// </summary>
    public class MediaSaleDetailsConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>影帶類別</summary>
        public string MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int END_EPISODE { get; set; }

        /// <summary>交付集數</summary>
        public int DELY_EPISODE { get; set; }

        /// <summary>每集長度</summary>
        public int EPISODE_TIME { get; set; }

        /// <summary>影帶規格</summary>
        public string MEDIA_SPEC { get; set; }

        /// <summary>未稅單價/NET單價</summary>
        public double NET { get; set; }

        /// <summary>未稅單價_台幣/NET單價_台幣</summary>
        public int NET_TWD { get; set; }

        /// <summary>稅額/預估保留稅額(銷售明細)</summary>
        public double TAX { get; set; }

        /// <summary>稅額/預估保留稅額_台幣(銷售明細)</summary>
        public int TAX_TWD { get; set; }

        /// <summary>含稅單價/GROSS單價</summary>
        public double GROSS { get; set; }

        /// <summary>含稅單價_台幣/GROSS單價_台幣</summary>
        public int GROSS_TWD { get; set; }

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

        /// <summary>所屬專案年分</summary>
        public string PROJECT_USE_YEAR { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 授權權利 設定
    /// </summary>
    public class MediaSaleAuthorizesConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>洲別</summary>
        public string CONTINENT { get; set; }

        /// <summary>國家</summary>
        public string COUNTRY { get; set; }

        /// <summary>播放平台</summary>
        public string PLAY_PLATFORM { get; set; }

        /// <summary>播放[標記]</summary>
        public string PLAY { get; set; }

        /// <summary>販售[標記]</summary>
        public string SELL { get; set; }

        /// <summary>剪後播[標記]</summary>
        public string EDIT_TO_PLAY { get; set; }

        /// <summary>剪後售[標記]</summary>
        public string EDIT_TO_SELL { get; set; }

        /// <summary>授權時間類型</summary>
        public string ALLOTED_TIME_TYPE { get; set; }

        /// <summary>授權開始時間(授權時間)</summary>
        public Nullable<DateTime> START_DATE { get; set; }

        /// <summary>授權結束時間(授權時間)</summary>
        public Nullable<DateTime> END_DATE { get; set; }

        /// <summary>授權方式類型</summary>
        public string FREQUENCY_TYPE { get; set; }

        /// <summary>授權播放次數(授權方式)</summary>
        public int PLAY_FREQUENCY { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 額外項目 設定
    /// </summary>
    public class MediaSaleExtrasConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>名稱</summary>
        public string NAME { get; set; }

        /// <summary>含稅價</summary>
        public double TAX_INCL { get; set; }

        /// <summary>含稅價_台幣</summary>
        public int TAX_INCL_TWD { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱 </summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案年分</summary>
        public string PROJECT_USE_YEAR { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 收款辦法 設定
    /// </summary>
    public class MediaSaleCollectionsConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>收款項目</summary>
        public string COLL_PROJECT { get; set; }

        /// <summary>收款條件</summary>
        public string COLL_TERMS { get; set; }

        /// <summary>
        /// 收款方式編號：
        /// DD.票據
        /// FF.電匯
        /// CS.現金
        /// </summary>
        public string COLL_METHOD_ID { get; set; }

        /// <summary>未稅金額/NET單價</summary>
        public double NET { get; set; }

        /// <summary>含稅總額/GROSS單價</summary>
        public double GROSS { get; set; }

        /// <summary>材料費</summary>
        public double MATERIAL { get; set; }

        /// <summary>額外採購項目金額</summary>
        public double EX_AMOUNT { get; set; }

        /// <summary>小計</summary>
        public double ORDER_SUM { get; set; }

        /// <summary>小計(換算)</summary>
        public int ORDER_SUM_CONV { get; set; }

        /// <summary>使用預算金額</summary>
        public int USE_BUDGET { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 使用預算 設定
    /// </summary>
    public class MediaSaleBudgetsConfig : BudgetConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }
    }

    /// <summary>
    /// 版權銷售申請單 交付項目 設定
    /// </summary>
    public class MediaSaleDeliverysConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>影片類型</summary>
        public string MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int END_EPISODE { get; set; }

        /// <summary>交付集數</summary>
        public int DELY_EPISODE { get; set; }
    }
}