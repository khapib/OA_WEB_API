using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 外部起單資訊
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    #region - 回傳ERP狀態 -

    public class GetExternalData
    {
        /// <summary>BPM 表單唯一碼</summary>
        public string BPM_REQ_ID { get; set; }

        /// <summary>ERP 表單唯一碼</summary>
        public string ERP_FORM_NO { get; set; }

        /// <summary>狀態</summary>
        public string STATE { get; set; }
    }

    #endregion

    #region - 共用抬頭(外部起單) -

    public class InfoTitle
    {
        /// <summary>BPM表 單唯一碼</summary>
        public string BPM_REQ_ID { get; set; }

        /// <summary>ERP 表單唯一碼</summary>
        public string ERP_FORM_NO { get; set; }

        /// <summary>ERP送審人</summary>
        public string CREATE_BY { get; set; }

    }

    #endregion

    #region - 專案建立審核單(外部起單) -

    /// <summary>
    /// 專案建立審核單(外部起單)ERP資料
    /// </summary>
    public class ProjectReviewERPInfo : InfoTitle
    {
        /// <summary>專案群組名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>專案名稱</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>執行年分</summary>
        public string USE_YEAR { get; set; }

        /// <summary>ERP起案人</summary>
        public string START_ID { get; set; }

        /// <summary>建檔日期</summary>
        public string CREATE_DATE { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

    }

    #endregion

    #region - 合作夥伴審核單(外部起單) -

    /// <summary>
    /// 合作夥伴審核單(外部起單)ERP資料
    /// </summary>
    public class SupplierReviewERPInfo
    {
        /// <summary>合作夥伴審核單(表頭內容)</summary>
        public SupplierReviewInfoTitle TITLE { get; set; }

        /// <summary>合作夥伴審核單(基本資料)(已審核)</summary>
        public SupplierReviewConfig INFO { get; set; }

        /// <summary>合作夥伴審核單(基本資料)(修改後/新增)</summary>
        public SupplierReviewConfig TEMP_INFO { get; set; }

        /// <summary>合作夥伴審核單(銀行往來資訊)(已審核)</summary>
        public IList<SupplierReviewRemitConfig> REMIT_INFO { get; set; }

        /// <summary>合作夥伴審核單(銀行往來資訊)(修改後/新增)</summary>
        public IList<SupplierReviewRemitConfig> TEMP_REMIT_INFO { get; set; }

        /// <summary>合作夥伴審核單(附件)</summary>
        public IList<AttachmentConfig> ATTACHMENT { get; set; }
    }

    /// <summary>
    /// 合作夥伴審核單(表頭內容)
    /// </summary>
    public class SupplierReviewInfoTitle : InfoTitle
    {
        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }
    }

    #endregion

    #region - 費用申請單(外部起單) -

    /// <summary>
    /// 費用申請單(外部起單)ERP資料
    /// </summary>
    public class ExpensesReimburseERPInfo : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    #endregion

    #region - 行政採購類_(外部起單) -

    #region - 行政採購申請單(外部起單) -

    /// <summary>
    /// 行政採購申請單(外部起單)ERP資料
    /// </summary>
    public class GeneralOrderERPInfo : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    #endregion

    #region - 行政採購異動申請單(外部起單) -

    /// <summary>
    /// 行政採購異動申請單(外部起單)ERP資料
    /// </summary>
    public class GeneralOrderChangeERPInfo : InfoTitle
    {
        /// <summary>行政採購:新單ERP 表單唯一碼</summary>
        public string ERP_MODIFY_FORM_NO { get; set; }

        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }

        /// <summary>原異動單BPM 表單唯一碼</summary>
        public string GROUP_ID { get; set; }

        /// <summary>異動次數</summary>
        public int MODIFY_NO { get; set; }

        /// <summary>不可異動標住(付款辦法)</summary>
        public string LOCK_PERIOD { get; set; }
    }

    #endregion

    #region - 行政採購點驗收單(外部起單) -

    /// <summary>
    /// 行政採購點驗收單(外部起單)
    /// </summary>
    public class GeneralAcceptanceERPInfo
    {
        /// <summary>行政採購點驗收單(表頭內容)</summary>
        public GeneralAcceptanceInfoTitle TITLE { get; set; }

        /// <summary>行政採購點驗收單(表單內容)</summary>
        public GeneralAcceptanceConfig INFO { get; set; }

        /// <summary>行政採購點驗收單(驗收明細)</summary>
        public IList<GeneralAcceptanceDetailsConfig> DTL { get; set; }
    }

    /// <summary>
    /// 行政採購點驗收單(表頭內容)
    /// </summary>
    public class GeneralAcceptanceInfoTitle : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    #endregion

    #region - 行政採購請購單(外部起單) -

    /// <summary>
    /// 行政採購請購單(外部起單)
    /// </summary>
    public class GeneralInvoiceERPInfo
    {
        /// <summary>行政採購點驗收單(表頭內容)</summary>
        public GeneralInvoiceInfoTitle TITLE { get; set; }

        /// <summary>行政採購請購單(表單內容)</summary>
        public GeneralInvoiceInfoConfig INFO { get; set; }
    }

    /// <summary>
    /// 行政採購請購單(表頭內容)
    /// </summary>
    public class GeneralInvoiceInfoTitle : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 行政採購請購單(表單內容)
    /// </summary>
    public class GeneralInvoiceInfoConfig
    {
        /// <summary>行政採購 系統編號</summary>
        public string GENERAL_ORDER_REQUISITION_ID { get; set; }

        /// <summary>行政採購 ERP 表單唯一碼</summary>
        public string GENERAL_ORDER_ERP_FORM_NO { get; set; }

        /// <summary>行政採購點驗收單 系統編號</summary>
        public string GENERAL_ACCEPTANCE_REQUISITION_ID { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }
    }

    #endregion

    #endregion

    #region - 版權採購類_(外部起單) -

    #region - 版權採購申請單(外部起單) -

    /// <summary>
    /// 版權採購申請單(外部起單)ERP資料
    /// </summary>
    public class MediaOrderERPInfo : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    #endregion

    #region - 版權採購異動申請單(外部起單) -

    /// <summary>
    /// 版權採購異動申請單(外部起單)ERP資料
    /// </summary>
    public class MediaOrderChangeERPInfo : InfoTitle
    {
        /// <summary>行政採購:新單ERP 表單唯一碼</summary>
        public string ERP_MODIFY_FORM_NO { get; set; }

        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }

        /// <summary>原異動單BPM 表單唯一碼</summary>
        public string GROUP_ID { get; set; }

        /// <summary>異動次數</summary>
        public int MODIFY_NO { get; set; }

        /// <summary>不可異動標住(付款辦法)</summary>
        public string LOCK_PERIOD { get; set; }
    }

    #endregion

    #region - 版權採購交片單(外部起單) -

    /// <summary>
    /// 版權採購交片單(外部起單)
    /// </summary>
    public class MediaAcceptanceERPInfo
    {
        /// <summary>行政採購點驗收單(表頭內容)</summary>
        public MediaAcceptanceInfoTitle TITLE { get; set; }

        /// <summary>行政採購點驗收單(表單內容)</summary>
        public MediaAcceptanceConfig INFO { get; set; }

        /// <summary>行政採購點驗收單(驗收明細)</summary>
        public IList<MediaAcceptanceDetailsConfig> DTL { get; set; }
    }

    public class MediaAcceptanceInfoTitle : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }

    }

    #endregion

    #region - 版權採購請購單(外部起單) -

    /// <summary>
    /// 版權採購請購單(外部起單)
    /// </summary>
    public class MediaInvoiceERPInfo
    {
        /// <summary>行政採購點驗收單(表頭內容)</summary>
        public MediaInvoiceInfoTitle TITLE { get; set; }

        /// <summary>行政採購請購單(表單內容)</summary>
        public MediaInvoiceInfoConfig INFO { get; set; }
    }

    /// <summary>
    /// 版權採購請購單(表頭內容)
    /// </summary>
    public class MediaInvoiceInfoTitle : InfoTitle
    {
        /// <summary>ERP 工作流程名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 版權採購請購單(表單內容)
    /// </summary>
    public class MediaInvoiceInfoConfig
    {
        /// <summary>版權採購 系統編號</summary>
        public string MEDIA_ORDER_REQUISITION_ID { get; set; }

        /// <summary>版權採購 ERP 表單唯一碼</summary>
        public string MEDIA_ORDER_ERP_FORM_NO { get; set; }

        /// <summary>版權採購點驗收單 系統編號</summary>
        public string MEDIA_ACCEPTANCE_REQUISITION_ID { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }
    }

    #endregion

    #endregion
}