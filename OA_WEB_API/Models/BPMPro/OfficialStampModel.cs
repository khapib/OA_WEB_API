using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 用印申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 用印申請單(查詢條件)
    /// </summary>
    public class OfficialStampQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 用印申請單
    /// </summary>
    public class OfficialStampViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>用印申請單 表頭資訊</summary>
        public OfficialStampTitle OFFICIAL_STAMP_TITLE { get; set; }

        /// <summary>用印申請單 表單內容 設定</summary>
        public OfficialStampConfig OFFICIAL_STAMP_CONFIG { get; set; }

        /// <summary>用印申請單 用印項目明細 設定</summary>
        public IList<OfficialStampDocumentsConfig> OFFICIAL_STAMP_DOCS_CONFIG { get; set; }

        /// <summary>用印申請單 會簽簽核人員 設定</summary>
        public List<OfficialStampApproversConfig> OFFICIAL_STAMP_APPROVERS_CONFIG { get; set; }
    }

    /// <summary>
    /// 用印申請單 表頭資訊
    /// </summary>
    public class OfficialStampTitle : HeaderTitle
    {
        /// <summary>公司名稱</summary>
        public string COMPANY_NAME { get; set; }

        /// <summary>
        /// 級別：
        /// 調整 ApplicantInfo(申請人資訊)的，表單PRIORITY(重要性)
        /// 特急件:3
        /// 急件:2
        /// 普通件:1
        /// </summary>
        public string LEVEL_TYPE { get; set; }
    }

    /// <summary>
    /// 用印申請單 表單內容 設定
    /// </summary>
    public class OfficialStampConfig
    {
        /// <summary>申請源由</summary>
        public string REASON { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>往來對象</summary>
        public string CONTACT { get; set; }

        /// <summary>公文字號</summary>
        public string APPROVAL_NO { get; set; }
    }

    /// <summary>
    /// 用印申請單 用印項目明細 設定
    /// </summary>
    public class OfficialStampDocumentsConfig
    {
        /// <summary>文件名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>份數</summary>
        public int SERVINGS { get; set; }

        /// <summary>請印種類</summary>
        public string APPLY_STAMP_TYPE { get; set; }

        /// <summary>請印種類_其他</summary>
        public string APPLY_STAMP_TYPE_OTHERS { get; set; }
    }

    /// <summary>
    /// 用印申請單 會簽簽核人員 設定
    /// </summary>
    public class OfficialStampApproversConfig : ApproversConfig
    {

    }
}