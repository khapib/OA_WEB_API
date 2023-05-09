using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 尚未播出檔拷貝申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 尚未播出檔拷貝申請單(查詢條件)
    /// </summary>
    public class MediaWarehouseNotAiredCopyQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 尚未播出檔拷貝申請單
    /// </summary>
    public class MediaWarehouseNotAiredCopyViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>尚未播出檔拷貝申請單 表頭資訊</summary>
        public MediaWarehouseNotAiredCopyTitle MEDIA_WAREHOUSE_NOT_AIRED_COPY_TITLE { get; set; }

        /// <summary>尚未播出檔拷貝申請單 表單內容 設定</summary>
        public MediaWarehouseNotAiredCopyConfig MEDIA_WAREHOUSE_NOT_AIRED_COPY_CONFIG { get; set; }

        /// <summary>尚未播出檔拷貝申請單 拷貝明細 設定</summary>
        public IList<MediaWarehouseNotAiredCopyDetailsConfig> MEDIA_WAREHOUSE_NOT_AIRED_COPY_DTLS_CONFIG { get; set; }
    }

    /// <summary>
    /// 尚未播出檔拷貝申請單 表頭資訊
    /// </summary>
    public class MediaWarehouseNotAiredCopyTitle : HeaderTitle
    {

    }

    /// <summary>
    /// 尚未播出檔拷貝申請單 表單內容 設定
    /// </summary>
    public class MediaWarehouseNotAiredCopyConfig
    {
        /// <summary>拷貝說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 尚未播出檔拷貝申請單 拷貝明細 設定
    /// </summary>
    public class MediaWarehouseNotAiredCopyDetailsConfig
    {
        /// <summary>節目名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>集數</summary>
        public string EPISODE { get; set; }

        /// <summary>用途</summary>
        public string APPLICATION { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }
}