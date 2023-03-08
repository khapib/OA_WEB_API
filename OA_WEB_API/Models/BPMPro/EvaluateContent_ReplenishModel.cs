using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 內容評估表_補充意見(查詢條件)
    /// </summary>
    public class EvaluateContent_ReplenishQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見
    /// </summary>
    public class EvaluateContent_ReplenishViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>內容評估表_補充意見 表頭資訊</summary>
        public EvaluateContent_ReplenishTitle EVALUATE_CONTENT_REPLENISH_TITLE { get; set; }

        /// <summary>內容評估表_補充意見 表單內容 設定</summary>
        public EvaluateContent_ReplenishConfig EVALUATE_CONTENT_REPLENISH_CONFIG { get; set; }

        /// <summary>內容評估表_補充意見 補充意見彙整 設定</summary>
        public IList<EvaluateContent_ReplenishEvaluatesConfig> EVALUATE_CONTENT_REPLENISH_EVAS_CONFIG { get; set; }

        /// <summary>內容評估表_補充意見 決策意見彙整 設定</summary>
        public IList<EvaluateContent_ReplenishDecisionsConfig> EVALUATE_CONTENT_REPLENISH_DECS_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 內容評估表_補充意見 表頭資訊
    /// </summary>
    public class EvaluateContent_ReplenishTitle : ImplementHeader
    {
        /// <summary>補充意見編號</summary>
        public string SORT_NO { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見 表單內容 設定
    /// </summary>
    public class EvaluateContent_ReplenishConfig
    {
        /// <summary>原始片名</summary>
        public string ORIGINAL_TITLE { get; set; }

        /// <summary>播出片名</summary>
        public string USUALLY_TITLE { get; set; }

        /// <summary>譯名</summary>
        public string TRANSLATE_TITLE { get; set; }

        /// <summary>國別</summary>
        public string COUNTRY_NAME { get; set; }

        /// <summary>影片類型</summary>
        public string MEDIA_TYPE { get; set; }

        /// <summary>影片屬性</summary>
        public string MEDIA_ATTRIBUTE { get; set; }

        /// <summary>類別</summary>
        public string CATEGORY { get; set; }

        /// <summary>影片長度</summary>
        public int MEDIA_LENGTH { get; set; }

        /// <summary>評估集數</summary>
        public string EVALUATE_EPISODE { get; set; }

        /// <summary>總集數</summary>
        public int EPISODE_TOTAL { get; set; }

        /// <summary>提供單位</summary>
        public string PROVIDE_UNIT { get; set; }

        /// <summary>提供日期</summary>
        public Nullable<DateTime> PROVIDE_DATE { get; set; }

        /// <summary>製作單位</summary>
        public string PRODUCER_UNIT { get; set; }

        /// <summary>製作年份</summary>
        public string PRODUCER_YEAR { get; set; }

        /// <summary>字幕</summary>
        public string SUBTITLE { get; set; }

        /// <summary>語言</summary>
        public string MEDIA_LANG { get; set; }

        /// <summary>雙語播出[標記]</summary>
        public string IS_BILINGUAL { get; set; }

        /// <summary>收視年齡層</summary>
        public string AGE_CATEGORY { get; set; }

        /// <summary>收視群組</summary>
        public string VIEW_GROUP { get; set; }

        /// <summary>導演</summary>
        public string DIRECTOR { get; set; }

        /// <summary>編劇</summary>
        public string SCREENWRITER { get; set; }

        /// <summary>製作人</summary>
        public string PRODUCER { get; set; }

        /// <summary></summary>
        public string E_PRODUCER { get; set; }

        /// <summary>主持人</summary>
        public string EMCEE { get; set; }

        /// <summary>主要演員或來賓</summary>
        public string ARTISTS { get; set; }

        /// <summary>內容大綱</summary>
        public string CONTENT { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>指定劇種負責人編號</summary>
        public string PRINCIPAL_ID { get; set; }

        /// <summary>指定劇種負責人姓名</summary>
        public string PRINCIPAL_NAME { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見 補充意見彙整 設定
    /// </summary>
    public class EvaluateContent_ReplenishEvaluatesConfig
    {
        /// <summary>評估人員編號</summary>
        public string USER_ID { get; set; }

        /// <summary>評估人員姓名</summary>
        public string USER_NAME { get; set; }
        
        /// <summary>建議原因</summary>
        public string REASON { get; set; }

        /// <summary>建議時間</summary>
        public Nullable<DateTime> OPINION_DATE_TIME { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見 決策意見彙整 設定
    /// </summary>
    public class EvaluateContent_ReplenishDecisionsConfig: EvaluateContent_ReplenishEvaluatesConfig
    {
        /// <summary>建議類型：
        /// ADV.建議採購
        /// N_ADV.不建議採購
        /// AG.需再次評估
        /// N_I.沒意見(決策意見無這一項)
        /// </summary>
        public string ADVISE_TYPE { get; set; }
    }

    #region - 內容評估表_補充意見 填寫 -

    /// <summary>
    /// 內容評估表_補充意見 填寫 設定
    /// </summary>
    public class EvaluateContent_ReplenishFillinConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>指定劇種負責人編號</summary>
        public string PRINCIPAL_ID { get; set; }

        /// <summary>指定劇種負責人姓名</summary>
        public string PRINCIPAL_NAME { get; set; }
    }

    #endregion

    #region - 內容評估表_補充意見 評估意見 -

    /// <summary>
    /// 內容評估表_補充意見 評估意見 設定
    /// </summary>
    public class EvaluateContent_ReplenishOpinionConfig : EvaluateContent_ReplenishDecisionsConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>意見類型</summary>
        public string OPINION_TYPE { get; set; }

    }

    #endregion
}