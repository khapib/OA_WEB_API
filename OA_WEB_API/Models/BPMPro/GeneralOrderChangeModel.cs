using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 行政採購異動申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 行政採購異動申請單(查詢條件)
    /// </summary>
    public class GeneralOrderChangeQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 行政採購異動申請單
    /// </summary>
    public class GeneralOrderChangeViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>行政採購異動申請單 表頭資訊及表單內容 設定</summary>
        public GeneralOrderChangeConfig GENERAL_ORDER_CHANGE_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 行政採購異動申請 表頭資訊
    /// </summary>
    public class GeneralOrderChangeTitle : ImplementHeader
    {
        /// <summary>不可異動標住(付款辦法)</summary>
        public string PYMT_LOCK_PERIOD { get; set; }

        /// <summary>異動原單系統編號</summary>
        public string GROUP_ID { get; set; }

        /// <summary>異動原單BPM 表單單號</summary>
        public string GROUP_BPM_FORM_NO { get; set; }

        /// <summary>異動原單BPM 表單路徑</summary>
        public string GROUP_PATH { get; set; }

        /// <summary>表單操作</summary>
        public string FORM_ACTION { get; set; }    

        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }

        /// <summary> 新表單ERP 表單唯一碼</summary>
        public string MODIFY_FORM_NO { get; set; }

        ///// <summary>預計匯率</summary>
        //public double PRE_RATE { get; set; }
    }

    /// <summary>
    /// 行政採購異動申請單 表單內容 設定
    /// </summary>
    public class GeneralOrderChangeConfig : GeneralOrderChangeTitle
    {
        /// <summary>異動說明</summary>
        public string CHANGE_DESCRIPTION { get; set; }
    }

}