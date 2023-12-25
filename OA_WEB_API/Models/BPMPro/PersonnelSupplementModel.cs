using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 人員增補單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 人員增補單(查詢條件)
    /// </summary>
    public class PersonnelSupplementQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 人員增補單
    /// </summary>
    public class PersonnelSupplementViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>人員增補單 表頭資訊</summary>
        public PersonnelSupplementTitle PERSONNEL_SUPPLEMENT_TITLE { get; set; }

        /// <summary>人員增補單 表單內容 設定</summary>
        public PersonnelSupplementConfig PERSONNEL_SUPPLEMENT_CONFIG { get; set; }

        /// <summary>人員增補單 增補明細 設定</summary>
        public IList<PersonnelSupplementDetailsConfig> PERSONNEL_SUPPLEMENT_DTLS_CONFIG { get; set; }
    }

    /// <summary>
    /// 人員增補單 表頭資訊
    /// </summary>
    public class PersonnelSupplementTitle : HeaderTitle
    {

    }

    /// <summary>
    /// 人員增補單 表單內容 設定
    /// </summary>
    public class PersonnelSupplementConfig
    {
        /// <summary>增補職別</summary>
        public string OCCUPATION { get; set; }

        /// <summary>增補原因</summary>
        public string REASON { get; set; }

        /// <summary>增補原因_備註</summary>
        public string REASON_NOTE { get; set; }

        /// <summary>編制人數</summary>
        public int BRAID_NUM { get; set; }

        /// <summary>現有人數</summary>
        public int NOW_NUM { get; set; }

        /// <summary>需求人數</summary>
        public int DEMAND_NUM { get; set; }

        /// <summary>是否為工讀生</summary>
        public Nullable<Boolean> IS_PART_TIME { get; set; }

        /// <summary>薪資(起)</summary>
        public int SALARY_MIN { get; set; }

        /// <summary>薪資(迄)</summary>
        public int SALARY_MAX { get; set; }

        /// <summary>學歷要求</summary>
        public string EDUCATION_REQUIREMENT { get; set; }

        /// <summary>工作內容說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>應徵方式</summary>
        public string APPLICATION_METHOD { get; set; }

        /// <summary>聯絡人Email</summary>
        public string CONTACT_EMAIL { get; set; }

        /// <summary>本職所需電腦技能</summary>
        public string COMPUTER_SKILLS { get; set; }

        /// <summary>其他工作條件限制</summary>
        public string JOB_SKILL { get; set; }

        /// <summary>文件編號</summary>
        public string APPROVAL_NO { get; set; }

        /// <summary>執行日期</summary>
        public Nullable<DateTime> IMPLEMENT_DATE { get; set; }

        /// <summary>關閉日期</summary>
        public Nullable<DateTime> CLOSE_DATE { get; set; }
    }

    /// <summary>
    /// 人員增補單 增補明細 設定
    /// </summary>
    public class PersonnelSupplementDetailsConfig
    {
        /// <summary>姓名</summary>
        public string NAME { get; set; }

        /// <summary>日期</summary>
        public Nullable<DateTime> DATE { get; set; }

        /// <summary>離職/增補 註記</summary>
        public string FLAG { get; set; }
    }
}