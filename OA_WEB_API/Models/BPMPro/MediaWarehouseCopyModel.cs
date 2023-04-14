using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 拷貝申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 拷貝申請單(查詢條件)
    /// </summary>
    public class MediaWarehouseCopyQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 拷貝申請單
    /// </summary>
    public class MediaWarehouseCopyViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>拷貝申請單 表頭資訊</summary>
        public MediaWarehouseCopyTitle MEDIA_WAREHOUSE_COPY_TITLE { get; set; }

        /// <summary>拷貝申請單 表單內容 設定</summary>
        public MediaWarehouseCopyConfig MEDIA_WAREHOUSE_COPY_CONFIG { get; set; }

        /// <summary>拷貝申請單 拷貝明細 設定</summary>
        public IList<MediaWarehouseCopyDetailsConfig> MEDIA_WAREHOUSE_COPY_DTLS_CONFIG { get; set; }

        /// <summary>拷貝申請單 音軌規格 設定</summary>
        public IList<MediaWarehouseCopyChaptersConfig> MEDIA_WAREHOUSE_COPY_CHS_CONFIG { get; set; }
    }

    /// <summary>
    /// 拷貝申請單 表頭資訊
    /// </summary>
    public class MediaWarehouseCopyTitle : HeaderTitle
    {

    }

    /// <summary>
    /// 拷貝申請單 表單內容 設定
    /// </summary>
    public class MediaWarehouseCopyConfig
    {
        /// <summary>拷貝說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>往來對象</summary>
        public string CONTACT { get; set; }

        /// <summary>公文字號</summary>
        public string APPROVAL_NO { get; set; }

        /// <summary>期望交付日期</summary>
        public Nullable<DateTime> EXPECTED_DATE { get; set; }

        /// <summary>處理人員</summary>
        public string CONTACT_PERSON { get; set; }

        #region 拷貝類型

        /// <summary>素材類別[複選]</summary>
        public string METERIAL_TYPE { get; set; }

        /// <summary>素材類別_其他</summary>
        public string METERIAL_TYPE_OTHERS { get; set; }

        /// <summary>用途說明</summary>
        public string APPLY_TYPE { get; set; }

        /// <summary>用途說明_其他</summary>
        public string APPLY_TYPE_OTHERS { get; set; }

        /// <summary>長度版本</summary>
        public string LENGTH { get; set; }

        /// <summary>長度版本_其他</summary>
        public string LENGTH_OTHERS { get; set; }

        /// <summary>拷貝格式</summary>
        public string COPY_TYPE { get; set; }

        /// <summary>拷貝格式_其他</summary>
        public string COPY_TYPE_OTHERS { get; set; }

        /// <summary>儲存型態[複選]</summary>
        public string SAVE_TYPE { get; set; }

        /// <summary>儲存型態_其他</summary>
        public string SAVE_TYPE_OTHERS { get; set; }

        /// <summary>指定路徑</summary>
        public string SAVE_PATH { get; set; }

        #endregion

        #region 拷貝需求

        /// <summary>畫面左上LOGO[標記]</summary>
        public string UPPER_LEFT_LOGO { get; set; }

        /// <summary>字幕[標記]</summary>
        public string SUBTITLES { get; set; }

        /// <summary>破口[標記]</summary>
        public string BREACH { get; set; }

        /// <summary>畫面動畫效果字[標記]</summary>
        public string EFFECT { get; set; }

        /// <summary>Time Code[標記]</summary>
        public string TIME_CODE { get; set; }

        /// <summary>版權所有嚴禁公開場所播放字樣[標記]</summary>
        public string MEDIA_POSSESSION { get; set; }

        /// <summary>片頭/尾(演職員表) [標記]</summary>
        public string CAST_CREW { get; set; }

        /// <summary>片頭/尾(歌詞) [標記]</summary>
        public string LYRICS { get; set; }

        /// <summary>片頭曲LOGO[標記]</summary>
        public string OPENING_LOGO { get; set; }

        /// <summary>預告字樣(畫面左上) [標記]</summary>
        public string NOTICE { get; set; }

        /// <summary>END卡[標記]</summary>
        public string END_CARD { get; set; }

        /// <summary>拷貝需求 其他</summary>
        public string COPY_DEMAND_OTHER { get; set; }

        #endregion

        #region 音軌規格

        /// <summary>立體聲[標記]</summary>
        public string STEREO { get; set; }

        /// <summary>響度</summary>
        public string LOUDNESS { get; set; }

        /// <summary>音軌規格 註記</summary>
        public string CHAPTER_NOTE { get; set; }

        #endregion

        #region 轉檔規格

        /// <summary>視訊轉檔封包</summary>
        public string VIDEO_CODEC { get; set; }

        /// <summary>NTSC/PAL</summary>
        public string FRANE_RATE { get; set; }

        /// <summary>解析大小</summary>
        public string BIRATE { get; set; }

        /// <summary>畫面尺寸</summary>
        public string RESOLUTION { get; set; }

        /// <summary>聲音封包</summary>
        public string AUDIO_CODEO { get; set; }

        /// <summary>轉檔規格 其他</summary>
        public string CONVERSION_NOTE { get; set; }

        #endregion

    }

    /// <summary>
    /// 拷貝申請單 拷貝明細 設定
    /// </summary>
    public class MediaWarehouseCopyDetailsConfig
    {
        /// <summary>節目名稱</summary>
        public string PROGRAM_NAME { get; set; }

        /// <summary>集數</summary>
        public string VOLUME { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 拷貝申請單 音軌規格 設定
    /// </summary>
    public class MediaWarehouseCopyChaptersConfig
    {
        /// <summary>音軌編號</summary>
        public int CHAPTER_NO { get; set; }

        /// <summary>音軌</summary>
        public string CHAPTER { get; set; }
    }
}