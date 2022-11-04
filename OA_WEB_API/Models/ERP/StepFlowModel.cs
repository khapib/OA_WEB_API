using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.ERP
{
    /// <summary>
    /// BPM表單狀態(查詢條件)
    /// </summary>
    public class StepFlowQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>是否表單已完結</summary>
        public string STATE_END { get; set; }

    }

    /// <summary>
    /// BPM表單狀態細項 設定
    /// </summary>
    public class StepFlowConfig
    {
        /// <summary>ERP 表單唯一碼</summary>
        public string ERP_FORM_NO { get; set; }

        /// <summary>BPM 表單唯一碼</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>BPM 表單單號</summary>
        public string BPM_FORM_NO { get; set; }

        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        /// <summary>申請人部門</summary>
        public string APPLICANT_DEPT { get; set; }

        /// <summary>申請人員工編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>申請人姓名</summary>
        public string APPLICANT_NAME { get; set; }

        /// <summary>申請時間</summary>
        public DateTime APPLICANT_DATE_TIME { get; set; }

        /// <summary>正式：0；草稿：1</summary>
        public int DRAFT_FLAG { get; set; }

        /// <summary>目前關卡流程ID</summary>
        public string PROCESS_ID { get; set; }

        /// <summary>簽核人部門</summary>
        public string APPROVER_DEPT { get; set; }

        /// <summary>簽核人員工編號</summary>
        public string APPROVER_ID { get; set; }

        /// <summary>簽核人姓名</summary>
        public string APPROVER_NAME { get; set; }

        /// <summary>簽核時間</summary>
        public DateTime APPROVE_TIME { get; set; }

        /// <summary>關卡簽核結果</summary>
        public int RESULT { get; set; }

        /// <summary>關卡簽核結果(提示)</summary>
        public string RESULT_PROMPT { get; set; }

        /// <summary>關卡簽核結果(回覆)</summary>
        public string COMMENT { get; set; }
    }

    /// <summary>
    /// BPM更新ERP表單狀態
    /// </summary>
    public class StepSignResponse
    {
        /// <summary>ERP 表單唯一碼</summary>
        public string ERP_FormNo { get; set; }

        /// <summary>BPM表單唯一碼</summary>
        public string RequisitionID { get; set; }
        
        /// <summary>BPM表單單號</summary>
        public string BPM_FormNo { get; set; }

        /// <summary>主旨</summary>
        public string FM7Subject { get; set; }
        
        /// <summary>
        /// 狀態回傳
        /// （
        /// 新建：NewCreate；
        /// 已簽完：Close；
        /// 不同意結束：DisagreeClose； 
        /// 簽核中：Progress；
        /// 草稿：Draft
        /// ）
        /// </summary>
        public string State { get; set; }

        /// <summary>最後簽核人員工編號</summary>
        public string LoginId { get; set; }

        /// <summary>最後簽核人姓名</summary>
        public string LoginName { get; set; }

        /// <summary>表單內容檢視頁路徑</summary>
        public string ViewPath { get; set; }
    }
}