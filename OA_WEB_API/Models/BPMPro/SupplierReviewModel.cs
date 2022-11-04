using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 合作夥伴審核單(查詢條件)
    /// </summary>
    public class SupplierReviewQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 合作夥伴審核單(查詢)
    /// </summary>
    public class SupplierReviewViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>合作夥伴審核 表頭內容</summary>
        public SupplierReviewTitle SUPPLIER_REVIEW_TITLE { get; set; }

        /// <summary>合作夥伴審核 基本資料 設定</summary>
        public SupplierReviewDifference SUPPLIER_REVIEW_CONFIG { get; set; }

        /// <summary>合作夥伴審核 銀行往來資訊</summary>
        public IList<SupplierReviewRemitDifference> SUPPLIER_REVIEW_REMIT_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }
    }

    /// <summary>
    /// 合作夥伴審核 表頭內容
    /// </summary>
    public class SupplierReviewTitle : HeaderTitle
    {
        /// <summary>審核單性質</summary>
        public string APPROVE { get; set; }

        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }
       
    }

    /// <summary>
    /// 合作夥伴審核 基本資料 設定
    /// </summary>
    public class SupplierReviewConfig
    {
        /// <summary>廠商名稱</summary>
        public string SUP_NAME { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>是否為廠商</summary>
        public string IS_SUP_PARTNER { get; set; }

        /// <summary>是否為客戶</summary>
        public string IS_CUST_PARTNER { get; set; }

        /// <summary>是否為廣告商</summary>
        public string IS_AD_PARTNER { get; set; }

        /// <summary>國別</summary>
        public string COUNTRY_NAME { get; set; }

        /// <summary>成立年度</summary>
        public string REG_YEAR { get; set; }

        /// <summary>實收資本額</summary>
        public string REG_CAPITAL { get; set; }

        /// <summary>員工人數</summary>
        public string NO_OF_EMPLOYEE { get; set; }

        /// <summary>負責人</summary>
        public string OWNER_NAME { get; set; }

        /// <summary>負責人電話</summary>
        public string OWNER_TEL { get; set; }

        /// <summary>公司聯絡電話</summary>
        public string REG_TEL { get; set; }

        /// <summary>公司網址</summary>
        public string REG_WEB { get; set; }

        /// <summary>公司登記地址</summary>
        public string REG_ADDRESS { get; set; }

        /// <summary>公司聯絡地址</summary>
        public string CURRENT_ADDRESS { get; set; }
    }

    /// <summary>
    /// 合作夥伴審核 銀行往來資訊 設定
    /// </summary>
    public class SupplierReviewRemitConfig
    {
        /// <summary>
        /// (銀行往來資訊)狀態
        /// Z001=新增
        /// Z002=修改
        /// </summary>
        public string STATUS_FLG { get; set; }

        /// <summary>銀行往來修改編號</summary>
        public string SUP_TX_TEMP_ID { get; set; }

        /// <summary>銀行往來編號</summary>
        public string SUP_TX_ID { get; set; }

        /// <summary>匯款類型</summary>
        public string TX_CATEGORY { get; set; }

        /// <summary>受款帳號</summary>
        public string BFCY_AC_NO { get; set; }

        /// <summary>受款帳號名稱/票據抬頭</summary>
        public string BFCY_AC_NAME { get; set; }

        /// <summary>受款銀行代碼</summary>
        public string BFCY_BK_NO { get; set; }

        /// <summary>受款銀行名稱</summary>
        public string BFCY_BK_NAME { get; set; }

        /// <summary>受款分行代碼</summary>
        public string BFCY_BK_BRANCH_NO { get; set; }

        /// <summary>受款分行名稱</summary>
        public string BFCY_BK_BRANCH_NAME { get; set; }

        /// <summary>SWIFT</summary>
        public string BFCY_BK_SWIFT { get; set; }

        /// <summary>受款銀行地址</summary>
        public string BFCY_BK_ADDRESS { get; set; }

        /// <summary>受款銀行國家</summary>
        public string BFCY_BK_COUNTRY_AND_CITY { get; set; }

        /// <summary>中間銀行</summary>
        public string BFCY_BK_IBAN { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY_NAME { get; set; }

        /// <summary>帳款聯絡人</summary>
        public string BFCY_NAME { get; set; }

        /// <summary>聯絡電話</summary>
        public string BFCY_TEL { get; set; }

        /// <summary>聯絡Email</summary>
        public string BFCY_EMAIL { get; set; }

    }

    /// <summary>
    /// 合作夥伴審核單(新增/修改/草稿)(資料表完整內容)
    /// </summary>
    public class SupplierReviewDataViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>合作夥伴審核 表頭資訊</summary>
        public SupplierReviewTitle SUPPLIER_REVIEW_TITLE { get; set; }

        /// <summary>合作夥伴審核 基本資料 設定(已審核)</summary>
        public SupplierReviewDifference SUPPLIER_REVIEW_CONFIG { get; set; }

        /// <summary>合作夥伴審核 基本資料 設定(修改後/新增)</summary>
        public SupplierReviewConfig SUPPLIER_REVIEW_TEMP_CONFIG { get; set; }

        /// <summary>合作夥伴審核 銀行往來資訊 設定(已審核)</summary>
        public IList<SupplierReviewRemitDifference> SUPPLIER_REVIEW_REMIT_CONFIG { get; set; }

        /// <summary>合作夥伴審核 銀行往來資訊 設定(修改後/新增)</summary>
        public IList<SupplierReviewRemitConfig> SUPPLIER_REVIEW_TEMP_REMIT_CONFIG { get; set; }

        /// <summary>附件</summary>
        public IList<AttachmentConfig> ATTACHMENT_CONFIG { get; set; }
    }

    #region - (查詢):Model -

    /// <summary>
    /// 合作夥伴審核單 基本資料 設定(資料表內容)
    /// </summary>
    public class SupplierReviewDataSetModel : SupplierReviewConfig
    {
        /// <summary>差異Json</summary>
        public string SUPPLIER_REVIEW_DIFF { get; set; }
    }

    /// <summary>
    /// 合作夥伴審核 銀行往來資訊 設定(資料表內容)
    /// </summary>
    public class SupplierReviewRemitDataSetModel : SupplierReviewRemitConfig
    {
        /// <summary>差異Json</summary>
        public string REMIT_DIFF { get; set; }
    }

    #endregion

    #region - 差異:Model -

    /// <summary>
    /// (差異)合作夥伴審核 基本資料 設定
    /// </summary>
    public class SupplierReviewDifference : SupplierReviewConfig
    {
        /// <summary>差異Json</summary>
        public IList<DifferenceInfo> DIFF { get; set; }
    }

    /// <summary>
    /// (差異)合作夥伴審核 銀行往來資訊 設定
    /// </summary>
    public class SupplierReviewRemitDifference : SupplierReviewRemitConfig
    {
        /// <summary>差異Json</summary>
        public IList<DifferenceInfo> DIFF { get; set; }

    }

    /// <summary>
    /// 差異
    /// </summary>
    public class DifferenceInfo
    {
        /// <summary>欄位名稱</summary>
        public string KEY { get; set; }

        /// <summary>差異資料</summary>
        public string ORIGINAL { get; set; }
    }

    #endregion

}