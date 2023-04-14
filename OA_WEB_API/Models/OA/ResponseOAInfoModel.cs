using OA_WEB_API.Models.ERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 回傳OA資訊
/// </summary>
namespace OA_WEB_API.Models.OA
{
    #region - 拷貝申請單 回傳OA資訊 -

    /// <summary>
    /// 拷貝申請單 回傳OA資訊
    /// </summary>
    public class MediaWarehouseCopyResponseOA
    {
        /// <summary>接收OA回傳狀態</summary>
        public OAResponseState OA_RESPONSE_STATE { get; set; }

        /// <summary>拷貝申請單 回傳OA 內容</summary>
        public MediaWarehouseCopyResponseOAInfoConfig MEDIA_WAREHOUSE_COPY_RESPONSE_OA_INFO_CONFIG { get; set; }

        /// <summary>拷貝申請單 回傳OA 節目資訊</summary>
        public IList<MediaWarehouseCopyResponseOAProgramInfoConfig> MEDIA_WAREHOUSE_COPY_RESPONSE_OA_PROGRAM_INFO_CONFIG { get; set; }

    }

    /// <summary>
    /// 拷貝申請單 回傳OA 內容
    /// </summary>
    public class MediaWarehouseCopyResponseOAInfoConfig
    {
        /// <summary>案件等級</summary>
        public string CASE_LEVEL { get; set; }

        /// <summary>BPM 表單單號</summary>
        public string BPM_FORM_NO { get; set; }

        /// <summary>主旨</summary>
        public string SUBJECT { get; set; }

        /// <summary>申請日期</summary>
        public Nullable<DateTime> APPLY_DATE { get; set; }

        /// <summary>申請人</summary>
        public string APPLY_PERSON { get; set; }

        /// <summary>期望取件日期</summary>
        public Nullable<DateTime> EXPECTED_DATE { get; set; }

        /// <summary>申請用途</summary>
        public string APPLY_TYPE { get; set; }

        /// <summary>申請用途_其他</summary>
        public string APPLY_TYPE_OTHERS { get; set; }

        /// <summary>往來對象</summary>
        public string CONTACT { get; set; }

        /// <summary>賣片文號</summary>
        public string SELL_DOC_NO { get; set; }

        /// <summary>協力單位</summary>
        public string MODIFY_PERSON { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>最後交付日期</summary>
        public Nullable<DateTime> DELIVER_DATE { get; set; }

        /// <summary>處理人員</summary>
        public string CREATE_PERSON { get; set; }
    }

    /// <summary>
    /// 拷貝申請單 回傳OA 節目資訊
    /// </summary>
    public class MediaWarehouseCopyResponseOAProgramInfoConfig
    {
        /// <summary>節目名稱</summary>
        public string PROGRAM_NAME { get; set; }

        /// <summary>集數</summary>
        public string VOLUME { get; set; }

        /// <summary>素材類別</summary>
        public string METERIAL_TYPE { get; set; }

        /// <summary>拷貝格式</summary>
        public string COPY_TYPE { get; set; }

        /// <summary>長度版本</summary>
        public string LENGTH { get; set; }
    }

    #endregion
}