using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 勞資委員投票
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 勞資委員投票(查詢條件)
    /// </summary>
    public class LabourAndCapitalMemberQueryModel
    {
        /// <summary>投票年度</summary>
        public string VOTE_YEAR { get; set; }

        /// <summary>登入者</summary>
        public string LOGIN_ID { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 登入者資訊
    /// </summary>
    public class LabourAndCapitalMemberVoterInfoConfig
    {
        /// <summary>
        /// 是否已投過票：
        /// true.尚未投過
        /// false.已投過
        /// </summary>
        public bool IS_VOTE { get; set; }

        /// <summary>主要部門</summary>
        public string MAIN_DEPT_ID { get; set; }

        /// <summary>投票年度</summary>
        public List<string> VOTE_YEARS { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 部門查詢
    /// </summary>
    public class LabourAndCapitalMemberVoterDeptsConfig
    {
        /// <summary>當年度_主要部門編號</summary>
        public string MAIN_DEPT_ID { get; set; }

        /// <summary>當年度_主要部門名稱</summary>
        public string MAIN_DEPT_NAME { get; set; }
    }

    /// <summary>
    /// 勞資委員投票
    /// </summary>
    public class LabourAndCapitalMemberViewModel
    {
        /// <summary>勞資委員投票 抬頭資訊</summary>
        public TitleInfo TITLE_INFO { get; set; }

        /// <summary>勞資委員投票 內容 設定</summary>
        public LabourAndCapitalMemberConfig LABOUR_AND_CAPITAL_MEMBER_CONFIG { get; set; }

        /// <summary>勞資委員投票 勞方代表 設定</summary>
        public IList<LabourAndCapitalMemberLaboursConfig> LABOUR_AND_CAPITAL_MEMBER_LABOURS_CONFIG { get; set; }

        /// <summary>勞資委員投票 附件 設定</summary>
        public IList<LabourAndCapitalMemberFilesConfig> LABOUR_AND_CAPITAL_MEMBER_FILES_CONFIG { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 抬頭資訊
    /// </summary>
    public class TitleInfo : ApplicantInfo
    {
        /// <summary>異動或調整過時間紀錄</summary>
        public Nullable<DateTime> MODIFY_DATETIME { get; set; }

        /// <summary>投票年度</summary>
        public string VOTE_YEAR { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 內容資訊 設定
    /// </summary>
    public class LabourAndCapitalMemberConfig
    {
        /// <summary>開始日期</summary>
        public Nullable<DateTime> START_DATE_TIME { get; set; }

        /// <summary>結束日期</summary>
        public Nullable<DateTime> END_DATE_TIME { get; set; }

        /// <summary>總投票數</summary>
        public int VOTE_NUM_TOTAL { get; set; }

        /// <summary>投票結果公布日期</summary>
        public Nullable<DateTime> VOTE_RESULT_DATE_TIME { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 勞方代表 設定
    /// </summary> 
    public class LabourAndCapitalMemberLaboursConfig
    {
        /// <summary>勞方代表部門實際投票人數</summary>
        public int? MAIN_DEPT_ACTUAL_VOTE_NUM { get; set; }

        /// <summary>是否為勞方代表當選人</summary>
        public string IS_LABOUR { get; set; }

        /// <summary>勞方代表主要部門</summary>
        public string MAIN_DEPT_ID { get; set; }

        /// <summary>勞方代表主要部門名稱</summary>
        public string MAIN_DEPT_NAME { get; set; }

        /// <summary>勞方代表部門編號</summary>
        public string MEMBER_DEPT_ID { get; set; }

        /// <summary>勞方代表部門名稱</summary>
        public string MEMBER_DEPT_NAME { get; set; }

        /// <summary>勞方代表員工編號</summary>
        public string MEMBER_ID { get; set; }

        /// <summary>勞方代表姓名</summary>
        public string MEMBER_NAME { get; set; }

        /// <summary>得票數</summary>
        public int? VOTE_NUM { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 投票 設定
    /// </summary>
    public class LabourAndCapitalMemberVoteConfig
    {
        /// <summary>投票年度</summary>
        public string VOTE_YEAR { get; set; }

        /// <summary>投票人主要部門</summary>
        public string MAIN_DEPT_ID { get; set; }

        /// <summary>投票人部門</summary>
        public string DEPT_ID { get; set; }

        /// <summary>投票人員工編號</summary>
        public string LOGIN_ID { get; set; }

        /// <summary>第一票(候選人員工編號)</summary>
        public string MEMBER_ID_1 { get; set; }

        /// <summary>第二票(候選人員工編號)</summary>
        public string MEMBER_ID_2 { get; set; }

        /// <summary>投票時間</summary>
        public Nullable<DateTime> VOTE_DATE { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 清除主部門票箱 設定
    /// </summary>
    public class LabourAndCapitalMemberClearMainDeptVoteConfig
    {
        /// <summary>投票年度</summary>
        public string VOTE_YEAR { get; set; }

        /// <summary>主要部門編號</summary>
        public string MAIN_DEPT_ID { get; set; }
    }

    /// <summary>
    /// 勞資委員投票 附件 設定
    /// </summary>
    public class LabourAndCapitalMemberFilesConfig : FilesConfig
    {
        /// <summary>附件位置</summary>
        public string FILE_PATH { get; set; }

        /// <summary>投票年度</summary>
        public string VOTE_YEAR { get; set; }

        /// <summary>上傳時間</summary>
        public Nullable<DateTime> UPLOD_TIME { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

    }

    /// <summary>
    /// 勞資委員投票 當選註記 設定
    /// </summary>
    public class LabourAndCapitalMemberMarkConfig : LabourAndCapitalMemberLaboursConfig
    {
        /// <summary>投票年度</summary>
        public string VOTE_YEAR { get; set; }
    }


}