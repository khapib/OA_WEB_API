using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 四方四隅_內容評估表(查詢條件)
    /// </summary>
    public class GPI_EvaluateContentQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 四方四隅_內容評估表
    /// </summary>
    public class GPI_EvaluateContentViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>四方四隅_內容評估表 表頭資訊</summary>
        public GPI_EvaluateContentTitle GPI_EVALUATE_CONTENT_TITLE { get; set; }

        /// <summary>四方四隅_內容評估表 表單內容 設定</summary>
        public GPI_EvaluateContentConfig GPI_EVALUATE_CONTENT_CONFIG { get; set; }

        /// <summary>四方四隅_內容評估表 評估人員 設定</summary>
        public IList<GPI_EvaluateContentUsersConfig> GPI_EVALUATE_CONTENT_USERS_CONFIG { get; set; }

        /// <summary>四方四隅_內容評估表 評估意見彙整 設定</summary>
        public IList<GPI_EvaluateContentEvaluatesConfig> GPI_EVALUATE_CONTENT_EVAS_CONFIG { get; set; }

        /// <summary>四方四隅_內容評估表 決策意見彙整 設定</summary>
        public IList<GPI_EvaluateContentDecisionsConfig> GPI_EVALUATE_CONTENT_DECS_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }

    }

    /// <summary>
    /// 四方四隅_內容評估表 表頭資訊
    /// </summary>
    public class GPI_EvaluateContentTitle : ImplementHeader
    {
        /// <summary>
        /// 評估類別：
        /// MADE.自製
        /// PUR.外購
        /// </summary>
        public string EVALUATE_CATEGORY { get; set; }

        /// <summary>評估編號</summary>
        public string SORT_NO { get; set; }
    }

    /// <summary>
    /// 四方四隅_內容評估表 表單內容 設定
    /// </summary>
    public class GPI_EvaluateContentConfig : EvaluateContentBasicInformation
    {
        /// <summary>繼續上呈[標記]</summary>
        public string IS_SUBMIT { get; set; }

        /// <summary>繼續上呈董事長</summary>
        public string IS_PRESIDENT { get; set; }

        /// <summary>建議評估期限</summary>
        public Nullable<DateTime> EVALUATE_DATE { get; set; }

        /// <summary>指定劇種負責人編號</summary>
        public string PRINCIPAL_ID { get; set; }

        /// <summary>指定劇種負責人姓名</summary>
        public string PRINCIPAL_NAME { get; set; }

    }

    /// <summary>
    /// 四方四隅_內容評估表 評估人員 設定
    /// </summary>
    public class GPI_EvaluateContentUsersConfig
    {
        /// <summary>評估人員主要部門</summary>
        public string USER_DEPT_MAIN_ID { get; set; }

        /// <summary>評估人員部門</summary>
        public string USER_DEPT_ID { get; set; }

        /// <summary>評估人員編號</summary>
        public string USER_ID { get; set; }

        /// <summary>評估人員姓名</summary>
        public string USER_NAME { get; set; }
    }

    /// <summary>
    /// 四方四隅_內容評估表 評估意見彙整 設定
    /// </summary>
    public class GPI_EvaluateContentEvaluatesConfig : GPI_EvaluateContentDecisionsConfig
    {
        /// <summary>優點</summary>
        public string ADVANTAGE { get; set; }

        /// <summary>缺點</summary>
        public string DEFECT { get; set; }

    }

    /// <summary>
    /// 四方四隅_內容評估表 決策意見彙整 設定
    /// </summary>
    public class GPI_EvaluateContentDecisionsConfig
    {
        /// <summary>評估人員編號</summary>
        public string USER_ID { get; set; }

        /// <summary>評估人員姓名</summary>
        public string USER_NAME { get; set; }

        /// <summary>建議類型：
        /// PUR.外購：
        /// ADV.建議採購
        /// N_ADV.不建議採購
        /// AG.需再次評估
        /// N_I.沒意見(決策意見無這一項)
        /// 
        /// MADE.自製：
        /// RSV.保留企劃
        /// N_RSV.不受理企劃
        /// </summary>
        public string ADVISE_TYPE { get; set; }

        /// <summary>建議原因</summary>
        public string REASON { get; set; }

        /// <summary>建議時間</summary>
        public Nullable<DateTime> OPINION_DATE_TIME { get; set; }
    }

    #region - 四方四隅_內容評估表 填寫 -

    /// <summary>
    /// 四方四隅_內容評估表 填寫 設定
    /// </summary>
    public class GPI_EvaluateContentFillinConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>四方四隅_內容評估表 表單內容 設定</summary>
        public GPI_EvaluateContentConfig GPI_EVALUATE_CONTENT_CONFIG { get; set; }

        /// <summary>四方四隅_內容評估表 評估人員 設定</summary>
        public IList<GPI_EvaluateContentUsersConfig> GPI_EVALUATE_CONTENT_USERS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    #endregion

    #region - 四方四隅_內容評估表 評估意見 -

    /// <summary>
    /// 四方四隅_內容評估表 評估意見 設定
    /// </summary>
    public class GPI_EvaluateContentOpinionConfig : GPI_EvaluateContentEvaluatesConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>
        /// 意見類型：
        /// EVA. 補充意見
        /// DEC.決策意見
        /// </summary>
        public string OPINION_TYPE { get; set; }
    }

    #endregion
}