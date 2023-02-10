using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 版權採購交片單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 版權採購交片單(查詢條件)
    /// </summary>
    public class MediaAcceptanceQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 版權採購交片單
    /// </summary>
    public class MediaAcceptanceViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>版權採購交片單 表頭資訊</summary>
        public MediaAcceptanceTitle MEDIA_ACCEPTANCE_TITLE { get; set; }

        /// <summary>版權採購交片單 表單內容 設定</summary>
        public MediaAcceptanceConfig MEDIA_ACCEPTANCE_CONFIG { get; set; }

        /// <summary>版權採購交片單 驗收明細 設定</summary>
        public IList<MediaAcceptanceDetailsConfig> MEDIA_ACCEPTANCE_DTLS_CONFIG { get; set; }

        /// <summary>版權採購申請單 授權權利 設定</summary>
        public IList<MediaAcceptanceAuthorizesConfig> MEDIA_ACCEPTANCE_AUTHS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 版權採購交片單 表頭資訊
    /// </summary>
    public class MediaAcceptanceTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 版權採購交片單 表單內容 設定
    /// </summary>
    public class MediaAcceptanceConfig
    {
        /// <summary>行政採購 系統編號</summary>
        public string MEDIA_ORDER_REQUISITION_ID { get; set; }

        /// <summary>行政採購 主旨</summary>
        public string MEDIA_ORDER_SUBJECT { get; set; }

        /// <summary>行政採購 BPM 表單單號</summary>
        public string MEDIA_ORDER_BPM_FORM_NO { get; set; }

        /// <summary>行政採購 ERP 表單唯一碼</summary>
        public string MEDIA_ORDER_ERP_FORM_NO { get; set; }

        /// <summary>行政採購 路徑</summary>
        public string MEDIA_ORDER_PATH { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

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

        /// <summary>是否通知片庫[標記]</summary>
        public string IS_FILM_STORAGE { get; set; }
    }

    /// <summary>
    /// 版權採購交片單 驗收明細 設定
    /// </summary>
    public class MediaAcceptanceDetailsConfig
    {
        /// <summary>商品代碼</summary>
        public string DTL_SUP_PROD_A_NO { get; set; }

        /// <summary>行數編號</summary>
        public int DTL_ROW_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string DTL_ITEM_NAME { get; set; }

        /// <summary>影帶規格</summary>
        public string DTL_MEDIA_SPEC { get; set; }

        /// <summary>影片類型</summary>
        public string DTL_MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int DTL_START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int DTL_END_EPISODE { get; set; }

        /// <summary>總採購集數</summary>
        public int DTL_ORDER_EPISODE { get; set; }

        /// <summary>驗收集數</summary>
        public int DTL_ACPT_EPISODE { get; set; }

        /// <summary>拆分集數</summary>
        public int DTL_DISMANTLE_EPISODE { get; set; }

        /// <summary>每集長度</summary>
        public int DTL_EPISODE_TIME { get; set; }

        /// <summary>母帶受領日期</summary>
        public Nullable<DateTime> DTL_GET_MASTERING_DATE { get; set; }

        /// <summary>驗收負責人主要部門</summary>
        public string DTL_OWNER_DEPT_MAIN_ID { get; set; }

        /// <summary>驗收負責人部門</summary>
        public string DTL_OWNER_DEPT_ID { get; set; }

        /// <summary>驗收負責人編號</summary>
        public string DTL_OWNER_ID { get; set; }

        /// <summary>驗收負責人姓名</summary>
        public string DTL_OWNER_NAME { get; set; }

        /// <summary>驗收備註</summary>
        public string DTL_ACPT_NOTE { get; set; }

        /// <summary>驗收結果</summary>
        public string DTL_STATUS { get; set; }

        /// <summary>商品備註</summary>
        public string DTL_NOTE { get; set; }

        /// <summary>是否為原始列</summary>
        public string IS_ORIGINAL { get; set; }

        /// <summary>原始列編碼</summary>
        public int ORIGIN_NUM { get; set; }
    }

    /// <summary>
    /// 版權採購申請單 授權權利 設定
    /// </summary>
    public class MediaAcceptanceAuthorizesConfig : MediaOrderAuthorizesConfig
    {

    }

    #region - 版權採購交片單 驗收簽核 -

    /// <summary>
    /// 版權採購交片單 驗收簽核
    /// </summary>
    public class MediaAcceptanceApproveViewModel : MediaAcceptanceQueryModel
    {
        /// <summary>版權採購交片單 驗收簽核 設定</summary>
        public IList<MediaAcceptanceApprovesConfig> MEDIA_ACCEPTANCE_APPROVES_CONFIG { get; set; }
    }

    /// <summary>
    /// 版權採購交片單 驗收簽核 設定
    /// </summary>
    public class MediaAcceptanceApprovesConfig
    {
        /// <summary>行數編號</summary>
        public int DTL_ROW_NO { get; set; }

        /// <summary>驗收結果</summary>
        public string DTL_STATUS { get; set; }

        /// <summary>驗收備註</summary>
        public string DTL_ACPT_NOTE { get; set; }
    }

    #endregion
}