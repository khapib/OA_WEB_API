using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 行政採購點驗收單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 行政採購點驗收單(查詢條件)
    /// </summary>
    public class GeneralAcceptanceQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 行政採購點驗收單
    /// </summary>
    public class GeneralAcceptanceViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>行政採購點驗收單 表頭資訊</summary>
        public GeneralAcceptanceTitle GENERAL_ACCEPTANCE_TITLE { get; set; }

        /// <summary>行政採購點驗收單 表單內容 設定</summary>
        public GeneralAcceptanceConfig GENERAL_ACCEPTANCE_CONFIG { get; set; }

        /// <summary>行政採購點驗收單 驗收明細 設定</summary>
        public IList<GeneralAcceptanceDetailsConfig> GENERAL_ACCEPTANCE_DETAILS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 行政採購點驗收單 表頭資訊
    /// </summary>
    public class GeneralAcceptanceTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 行政採購點驗收單 表單內容 設定
    /// </summary>
    public class GeneralAcceptanceConfig
    {
        /// <summary>行政採購 系統編號</summary>
        public string GENERAL_ORDER_REQUISITION_ID { get; set; }

        /// <summary>行政採購 主旨</summary>
        public string GENERAL_ORDER_SUBJECT { get; set; }

        /// <summary>行政採購 BPM 表單單號</summary>
        public string GENERAL_ORDER_BPM_FORM_NO { get; set; }

        /// <summary>行政採購 ERP 表單唯一碼</summary>
        public string GENERAL_ORDER_ERP_FORM_NO { get; set; }

        /// <summary>行政採購 路徑</summary>
        public string GENERAL_ORDER_PATH { get; set; }

        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }

        /// <summary>廠商名稱</summary>
        public string SUP_NAME { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>負責人</summary>
        public string OWNER_NAME { get; set; }

        /// <summary>負責人電話</summary>
        public string OWNER_TEL { get; set; }
    }

    /// <summary>
    /// 行政採購點驗收單 驗收明細 設定
    /// </summary>
    public class GeneralAcceptanceDetailsConfig
    {
        /// <summary>商品代碼</summary>
        public string DTL_SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string DTL_ITEM_NAME { get; set; }

        /// <summary>型號</summary>
        public string DTL_MODEL { get; set; }

        /// <summary>規格</summary>
        public string DTL_SPECIFICATIONS { get; set; }

        /// <summary>驗收量</summary>
        public int DTL_ACPT_QUANTITY { get; set; }

        /// <summary>總採購量</summary>
        public int DTL_QUANTITY { get; set; }

        /// <summary>單位</summary>
        public string DTL_UNIT { get; set; }

        /// <summary>驗收負責人主要部門</summary>
        public string DTL_OWNER_DEPT_MAIN_ID { get; set; }

        /// <summary>驗收負責人部門</summary>
        public string DTL_OWNER_DEPT_ID { get; set; }

        /// <summary>驗收負責人編號</summary>
        public string DTL_OWNER_ID { get; set; }

        /// <summary>驗收負責人姓名</summary>
        public string DTL_OWNER_NAME { get; set; }

        /// <summary>驗收備註</summary>
        public string DTL_ACPT_NOTE { get; set; }

        /// <summary>驗收結果</summary>
        public string DTL_STATUS { get; set; }

        /// <summary>商品備註</summary>
        public string DTL_NOTE { get; set; }

        /// <summary>是否為原始列</summary>
        public string IS_ORIGINAL { get; set; }

        /// <summary>原始列編碼</summary>
        public int? ORIGIN_NUM { get; set;}
    }

}