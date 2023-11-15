using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 停車證申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 停車證申請單(查詢條件)
    /// </summary>
    public class ParkingPermitQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 停車證申請單(查詢)
    /// </summary>
    public class ParkingPermitViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>停車證申請單 表頭資訊</summary>
        public ParkingPermitTitle PARKING_PERMIT_TITLE { get; set; }

        /// <summary>停車證申請單 表單內容 設定</summary>
        public ParkingPermitConfig PARKING_PERMIT_CONFIG { get; set; }

        /// <summary>停車證申請單 圖片上傳 設定</summary>
        public IList<ParkingPermitImagesConfig> PARKING_PERMIT_IMGS_CONFIG { get; set; }
    }

    /// <summary>
    /// 停車證申請單 表頭資訊
    /// </summary>
    public class ParkingPermitTitle : HeaderTitle
    {
        /// <summary>手機</summary>
        public string MOBILE { get; set; }

        /// <summary>公司名稱(申請人/窗口)</summary>
        public string COMPANY_NAME { get; set; }

        /// <summary>部門編號(自動帶入)(申請人/窗口)</summary>
        public string DEPT_ID { get; set; }

        /// <summary>局處中心編號(自動帶入)(申請人/窗口)</summary>
        public string OFFICE_ID { get; set; }

        /// <summary>組別編號(自動帶入)(申請人/窗口)</summary>
        public string GROUP_ID { get; set; }

        /// <summary>部門名稱(自動帶入)(申請人/窗口)</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>局處中心名稱(自動帶入)(申請人/窗口)</summary>
        public string OFFICE_NAME { get; set; }

        /// <summary>組別名稱(自動帶入)(申請人/窗口)</summary>
        public string GROUP_NAME { get; set; }

        /// <summary>年度(民國年)</summary>
        public int? ROC_YEAR { get; set; }

        /// <summary>
        /// 申請類別：員工/非員工
        /// </summary>
        public string APPLICATION_CATEGORY { get; set; }

        /// <summary>姓名(非員工)</summary>
        public string GUEST_NAME { get; set; }

        /// <summary>公司名稱(非員工)</summary>
        public string GUEST_COMPANY_NAME { get; set; }

        /// <summary>部門(非員工)</summary>
        public string GUEST_DEPT_NAME { get; set; }

        /// <summary>手機(非員工)</summary>
        public string GUEST_MOBILE { get; set; }
    }

    /// <summary>
    /// 停車證申請單 表單內容 設定
    /// </summary>
    public class ParkingPermitConfig
    {
        /// <summary>
        /// 車種類別：汽車/機車
        /// </summary>
        public string VEHICLE_CATEGORY { get; set; }

        /// <summary>車牌號碼</summary>
        public string LICENSE_PLATE_NUMBER { get; set; }

        /// <summary>
        /// 是否為異動：
        /// 給前端Bool，
        /// 存進DB記得轉String
        /// </summary>
        public Nullable<Boolean> IS_CHANGE { get; set; }

        /// <summary>異動的車牌</summary>
        public string CHANGE_LICENSE_PLATE_NUMBER { get; set; }

        /// <summary>車主關係</summary>
        public string CAR_OWNER_RELATIONSHIP { get; set; }
    }

    /// <summary>
    /// 停車證申請單 圖片上傳 設定
    /// </summary>
    public class ParkingPermitImagesConfig
    {
        /// <summary>
        /// 申請類別：員工/廠商
        /// </summary>
        public string APPLICATION_CATEGORY { get; set; }

        /// <summary>
        /// 車種類別：汽車/機車
        /// </summary>
        public string VEHICLE_CATEGORY { get; set; }

        /// <summary>車牌號碼</summary>
        public string LICENSE_PLATE_NUMBER { get; set; }

        /// <summary>圖片識別：行照/身分證</summary>
        public string IMG_IDENTIFY { get; set; }

        /// <summary>base64圖片碼</summary>
        public string PHOTO { get; set; }

        /// <summary>附件編碼名稱</summary>
        public string FILE_RENAME { get; set; }

        /// <summary>附件原本名稱</summary>
        public string FILE_NAME { get; set; }

        /// <summary>附件副檔名</summary>
        public string FILE_EXTENSION { get; set; }

        /// <summary>檔案大小</summary>
        public long? FILE_SIZE { get; set; }

        /// <summary>上傳者員工編號</summary>
        public string UPLOADER_ID { get; set; }

        /// <summary>上傳時間</summary>
        public Nullable<DateTime> UPLOAD_DATETIME { get; set; }

        /// <summary>
        /// 正式：0；草稿：1
        /// </summary>
        public int DRAFT_FLAG { get; set; }
    }
}