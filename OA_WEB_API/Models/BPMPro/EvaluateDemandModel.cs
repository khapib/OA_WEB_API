using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 需求評估單(查詢條件)
    /// </summary>
    public class EvaluateDemandQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 需求評估單
    /// </summary>
    public class EvaluateDemandViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>需求評估設定</summary>
        public EvaluateDemandConfig EVALUATE_DEMAND_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 需求評估設定
    /// </summary>
    public class EvaluateDemandConfig : HeaderTitle
    {
        /// <summary>概要</summary>
        public string COMPENDIUM { get; set; }

        /// <summary>窗口</summary>
        public string CONTACT_PERSON { get; set; }

        /// <summary>是否跳關</summary>
        public Nullable<Boolean> IS_JUMP { get; set; }

        /// <summary>評估</summary>
        public string EVALUATE { get; set; }

        /// <summary>方案</summary>
        public string SCHEME { get; set; }

        /// <summary>處理結果</summary>
        public string PROCESS_RESULT { get; set; }

        /// <summary>審核迴圈數</summary>
        public int APPROVE_LOOP { get; set; }
    }

}