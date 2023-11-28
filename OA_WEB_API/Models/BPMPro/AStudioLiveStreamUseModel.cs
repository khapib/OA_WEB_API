using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - A攝影棚直播使用申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// A攝影棚直播使用申請單(查詢條件)
    /// </summary>
    public class AStudioLiveStreamUseQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// A攝影棚直播使用申請單
    /// </summary>
    public class AStudioLiveStreamUseViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>A攝影棚直播使用申請單 表頭資訊</summary>
        public AStudioLiveStreamUseTitle A_STUDIO_LIVE_STREAM_USE_TITLE { get; set; }

        /// <summary>A攝影棚直播使用申請單 表單內容 設定</summary>
        public AStudioLiveStreamUseConfig A_STUDIO_LIVE_STREAM_USE_CONFIG { get; set; }

        /// <summary>A攝影棚直播使用申請單 會簽簽核人員 設定</summary>
        public List<AStudioLiveStreamUseApproversConfig> A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG { get; set; }
    }

    /// <summary>
    /// A攝影棚直播使用申請單 表頭資訊
    /// </summary>
    public class AStudioLiveStreamUseTitle : HeaderTitle
    {

    }

    /// <summary>
    /// A攝影棚直播使用申請單 表單內容 設定
    /// </summary>
    public class AStudioLiveStreamUseConfig
    {
        /// <summary>直播內容</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>直播日期</summary>
        public string LIVE_STREAM_DATE { get; set; }

        /// <summary>預計活動開始時間</summary>
        public string START_TIME { get; set; }

        /// <summary>預計活動結束時間</summary>
        public string END_TIME { get; set; }

        /// <summary>
        /// 使用器材：單機/雙機/三機
        /// </summary>
        public string EQUIPMENT_USE { get; set; }

        /// <summary>
        /// 麥克風數量(使用器材勾選就會必填)
        /// </summary>
        public int MIC_COUNT { get; set; }

        /// <summary>
        /// Light：使用投影幕/網美拍照區
        /// </summary>
        public string LIGHT { get; set; }

        /// <summary>是否要直播機</summary>
        public Nullable<Boolean> IS_LIVE_STREAM_EQUIPMENT { get; set; }

        /// <summary>特殊需求</summary>
        public string NOTE { get; set; }

        /// <summary>美術組應用內容</summary>
        public string APPLICATION_CONTENT { get; set; }
    }

    /// <summary>
    /// A攝影棚直播使用申請單 會簽簽核人員 設定
    /// </summary>
    public class AStudioLiveStreamUseApproversConfig : ApproversConfig
    {

    }

}