using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 內容評估表_補充意見(查詢條件)
    /// </summary>
    public class EvaluateContentReplenishQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見
    /// </summary>
    public class EvaluateContentReplenishViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>內容評估表_補充意見 表頭資訊</summary>
        public EvaluateContentReplenishTitle EVALUATE_CONTENT_REPLENISH_TITLE { get; set; }

        /// <summary>內容評估表_補充意見 表單內容 設定</summary>
        public EvaluateContentReplenishConfig EVALUATE_CONTENT_REPLENISH_CONFIG { get; set; }

        /// <summary>內容評估表_補充意見 補充意見彙整 設定</summary>
        public IList<EvaluateContentReplenishEvaluatesConfig> EVALUATE_CONTENT_REPLENISH_EVAS_CONFIG { get; set; }

        /// <summary>內容評估表_補充意見 決策意見彙整 設定</summary>
        public IList<EvaluateContentReplenishDecisionsConfig> EVALUATE_CONTENT_REPLENISH_DECS_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 內容評估表_補充意見 表頭資訊
    /// </summary>
    public class EvaluateContentReplenishTitle : ImplementHeader
    {
        /// <summary>
        /// 評估類別：
        /// MADE.自製
        /// PUR.外購
        /// </summary>
        public string EVALUATE_CATEGORY { get; set; }

        /// <summary>補充意見編號</summary>
        public string SORT_NO { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見 表單內容 設定
    /// </summary>
    public class EvaluateContentReplenishConfig : EvaluateContentBasicInformation
    {
        /// <summary>指定劇種負責人編號</summary>
        public string PRINCIPAL_ID { get; set; }

        /// <summary>指定劇種負責人姓名</summary>
        public string PRINCIPAL_NAME { get; set; }
    }

    /// <summary>
    /// 內容評估表_補充意見 補充意見彙整 設定
    /// </summary>
    public class EvaluateContentReplenishEvaluatesConfig
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
    public class EvaluateContentReplenishDecisionsConfig : EvaluateContentReplenishEvaluatesConfig
    {
        /// <summary>建議類型：
        /// PUR.外購：
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
    public class EvaluateContentReplenishFillinConfig
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
    public class EvaluateContentReplenishOpinionConfig : EvaluateContentReplenishDecisionsConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>意見類型</summary>
        public string OPINION_TYPE { get; set; }

    }

    #endregion

}