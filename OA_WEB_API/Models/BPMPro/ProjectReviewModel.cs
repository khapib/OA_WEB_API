using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 專案建立審核單(查詢條件)
    /// </summary>
    public class ProjectReviewQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 專案建立審核單
    /// </summary>
    public class ProjectReviewViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>專案建立審核 ERP 設定</summary>
        public ProjectReviewErpConfig PROJECT_REVIEW_ERP_CONFIG { get; set; }

        /// <summary>專案建立審核 BPM 設定</summary>
        public ProjectReviewBpmConfig PROJECT_REVIEW_BPM_CONFIG { get; set; }
    }

    /// <summary>
    /// 專案建立審核 ERP 設定
    /// </summary>
    public class ProjectReviewErpConfig
    {
        #region  - ERP專案建立審核單內容 - 

        /// <summary>專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>ERP 表單唯一碼</summary>
        public string FORM_NO { get; set; }

        /// <summary>專案簡述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>起案年度</summary>
        public string USE_YEAR { get; set; }

        /// <summary>起案部門</summary>
        public string OWNER_DEP { get; set; }

        /// <summary>ERP起案人</summary>
        public string START_ID { get; set; }

        /// <summary>建檔日期</summary>
        public string CREATE_DATE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        #endregion
    }

    /// <summary>
    /// 專案建立審核 BPM 設定
    /// </summary>
    public class ProjectReviewBpmConfig
    {
        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        #region - 稽核組簽核關卡 - 

        /// <summary>財務簽核</summary>
        public string GAD_REVIEW { get; set; }

        #endregion

        #region - 財務簽核關卡 -

        /// <summary>專案類型</summary>
        public string ACC_CATEGORY { get; set; }

        #endregion
    }
}