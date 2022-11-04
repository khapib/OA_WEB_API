using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 會簽管理系統 - 跑馬申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 跑馬申請單(查詢條件)
    /// </summary>
    public class RollCardQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 跑馬申請單
    /// </summary>
    public class RollCardViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>跑馬設定</summary>
        public RollCardConfig ROLLCARD_CONFIG { get; set; }

        /// <summary>跑馬時段清單</summary>
        public IList<RollCardRunTime> ROLLCARD_RUN_TIME { get; set; }
    }

    /// <summary>
    /// 跑馬設定
    /// </summary>
    public class RollCardConfig
    {
        #region - 申請頻道 -

        /// <summary>頻道1</summary>
        public string CH01 { get; set; }

        /// <summary>頻道2</summary>
        public string CH02 { get; set; }

        /// <summary>頻道3</summary>
        public string CH03 { get; set; }

        /// <summary>頻道4</summary>
        public string CH04 { get; set; }

        /// <summary>頻道5</summary>
        public string CH05 { get; set; }

        /// <summary>頻道6</summary>
        public string CH06 { get; set; }

        #endregion

        #region - 申請等級 -

        /// <summary>申請等級</summary>
        public string RUN_LEVEL { get; set; }

        /// <summary>每幾分鐘內</summary>
        public string CLASS_B_MINUTES { get; set; }

        /// <summary>跑幾次</summary>
        public string CLASS_B_FREQUENCY { get; set; }

        /// <summary>備註</summary>
        public string CLASS_E_NOTE { get; set; }

        #endregion

        #region - 執行日期 -

        /// <summary>開始日期</summary>
        public string START_DATE { get; set; }

        /// <summary>結束日期</summary>
        public string END_DATE { get; set; }

        #endregion

        #region - 每週週期 -

        /// <summary>執行方式</summary>
        public string RUN_MODE { get; set; }

        /// <summary>星期一</summary>
        public string EVERY_WEEK_MON { get; set; }

        /// <summary>星期二</summary>
        public string EVERY_WEEK_TUE { get; set; }

        /// <summary>星期三</summary>
        public string EVERY_WEEK_WED { get; set; }

        /// <summary>星期四</summary>
        public string EVERY_WEEK_THU { get; set; }

        /// <summary>星期五</summary>
        public string EVERY_WEEK_FRI { get; set; }

        /// <summary>星期六</summary>
        public string EVERY_WEEK_SAT { get; set; }

        /// <summary>星期日</summary>
        public string EVERY_WEEK_SUN { get; set; }

        #endregion

        #region - 跑馬內容 -

        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        /// <summary>內容</summary>
        public string ROLL_CONTENT { get; set; }

        /// <summary>備註</summary>
        public string REMARK { get; set; }

        #endregion
    }

    /// <summary>
    /// 跑馬時段
    /// </summary>
    public class RollCardRunTime
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>開始時間</summary>
        public string START_TIME { get; set; }

        /// <summary>結束時間</summary>
        public string END_TIME { get; set; }
    }
}