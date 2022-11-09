using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 會簽管理系統 - 表單及簽核流程
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    #region - 表單共用 -

    /// <summary>
    /// 申請人資訊
    /// </summary>
    public class ApplicantInfo
    {
        #region - 表單資訊 -

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>流程圖編號</summary>
        public string DIAGRAM_ID { get; set; }

        /// <summary>重要性(高：中：低：)</summary>
        public int? PRIORITY { get; set; }

        /// <summary>正式：0；草稿：1</summary>
        public int? DRAFT_FLAG { get; set; }

        /// <summary>流程啟用</summary>
        public int? FLOW_ACTIVATED { get; set; }

        #endregion

        #region - 表單主旨 -

        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        #endregion

        #region - 申請人資訊 -

        /// <summary>(申請/起案)部門編號</summary>
        public string APPLICANT_DEPT { get; set; }

        /// <summary>(申請/起案)部門名稱</summary>
        public string APPLICANT_DEPT_NAME { get; set; }

        /// <summary>申請人編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>申請人姓名</summary>
        public string APPLICANT_NAME { get; set; }

        /// <summary>申請人電話</summary>
        public string APPLICANT_PHONE { get; set; }

        /// <summary>申請時間</summary>
        public DateTime APPLICANT_DATETIME { get; set; }

        #endregion

        #region - 填單人資訊 -

        /// <summary>(填單人/代填單人)編號</summary>
        public string FILLER_ID { get; set; }

        /// <summary>(填單人/代填單人)姓名</summary>
        public string FILLER_NAME { get; set; }

        #endregion
    }

    /// <summary>
    /// 表單主旨
    /// </summary>
    public class FormHeader
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>項目名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>項目值</summary>
        public string ITEM_VALUE { get; set; }
    }

    /// <summary>
    /// 表單儲存至草稿列表
    /// </summary>
    public class FormDraftList
    {
        /// <summary>流水號</summary>
        public string UNIQUE_ID { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>識別編號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>顯示名稱</summary>
        public string DISPLAY_NAME { get; set; }

        /// <summary>填單人編號</summary>
        public string FILLER_ID { get; set; }

        /// <summary>儲存時間</summary>
        public DateTime SAVED_TIME { get; set; }

        /// <summary>重新填單</summary>
        public int REFILL { get; set; }

        /// <summary>原始系統編號</summary>
        public string ORIGINAL_REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 啟動流程：申請人送出
    /// </summary>
    public class FormAutoStart
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>流程圖編號</summary>
        public string DIAGRAM_ID { get; set; }

        /// <summary>申請人編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>申請部門編號</summary>
        public string APPLICANT_DEPT { get; set; }
    }

    /// <summary>
    /// 表單同意送出後訊息
    /// </summary>
    public class FormToaster
    {
        /// <summary>發信人編號</summary>
        public string RECIPIENT_ID { get; set; }

        /// <summary>訊息種類</summary>
        public string TOASTER_TYPE { get; set; }

        /// <summary>標題</summary>
        public string TITLE { get; set; }

        /// <summary>訊息</summary>
        public string MESSAGE { get; set; }

        /// <summary>修改人</summary>
        public string WHO_CHANGED { get; set; }

        /// <summary>修改時間</summary>
        public DateTime WHEN_CHANGED { get; set; }

        /// <summary>使用時間</summary>
        public DateTime WHEN_USED { get; set; }
    }

    /// <summary>
    /// 表單資料(查詢條件)
    /// </summary>
    public class FormQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單編號</summary>
        public string SERIAL_ID { get; set; }

        /// <summary>關卡編號</summary>
        public string PROCESS_ID { get; set; }

        /// <summary>特定人員</summary>
        public string RECEIVER_ID { get; set; }

        /// <summary>啟用簡訊通知</summary>
        public bool? IS_ENABLE_SMS { get; set; }
    }

    /// <summary>
    /// 表單資料
    /// </summary>
    public class FormData
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單編號</summary>
        public string SERIAL_ID { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>流程圖編號</summary>
        public string DIAGRAM_ID { get; set; }

        /// <summary>流程圖名稱</summary>
        public string DIAGRAM_NAME { get; set; }

        /// <summary>公司編號</summary>
        public string COMPANY_ID { get; set; }

        /// <summary>部門編號</summary>
        public string APPLICANT_DEPT_ID { get; set; }

        /// <summary>部門名稱</summary>
        public string APPLICANT_DEPT_NAME { get; set; }

        /// <summary>申請人編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>申請人</summary>
        public string APPLICANT_NAME { get; set; }

        /// <summary>申請日期</summary>
        public DateTime APPLICANT_DATETIME { get; set; }

        /// <summary>表單狀態編號</summary>
        public int FORM_STATUS { get; set; }

        /// <summary>表單狀態</summary>
        public string FORM_STATUS_NAME { get; set; }

        /// <summary>表單主旨</summary>
        public string FORM_SUBJECT { get; set; }

        /// <summary>重要性</summary>
        public string PRIORITY_TEXT { get; set; }
    }

    //允許代填單人

    #endregion

    #region - 簽核流程 -

    /// <summary>
    ///  簽核流程(查詢條件)
    /// </summary>
    public class FlowQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>公司編號</summary>
        public string COMPANY_ID { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>職稱權重</summary>
        public int? JOB_GRADE { get; set; }
    }

    /// <summary>
    /// 單位主管審核
    /// </summary>
    public class UnitApproverModel
    {
        /// <summary>完整職稱</summary>
        public string USER_TITLE { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }
    }

    /// <summary>
    /// 上一級主管
    /// </summary>
    public class SupervisorModel
    {
        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>員工姓名</summary>
        public string USER_NAME { get; set; }

        /// <summary>電子信箱</summary>
        public string EMAIL { get; set; }

        /// <summary>手機</summary>
        public string MOBILE { get; set; }
    }

    /// <summary>
    /// 代理人
    /// </summary>
    public class AgentModel
    {
        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>部門編號</summary>
        public string AGENT_ID { get; set; }

        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>開始時間</summary>
        public DateTime TIME_START { get; set; }

        /// <summary>結束時間</summary>
        public DateTime TIME_END { get; set; }
    }

    /// <summary>
    /// 特定人員知會
    /// </summary>
    public class ReceiverModel
    {
        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>員工姓名</summary>
        public string USER_NAME { get; set; }

        /// <summary>電子信箱</summary>
        public string EMAIL { get; set; }

        /// <summary>手機</summary>
        public string MOBILE { get; set; }
    }

    /// <summary>
    /// 特定知會通知
    /// </summary>
    public class InformNotifyModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>被通知人</summary>
        public string NOTIFY_BY { get; set; }

        /// <summary>角色群組編號</summary>
        public string ROLE_ID { get; set; }
    }

    #region - 群體知會通知 -

    /// <summary>
    /// 群體知會通知
    /// </summary>
    public class GroupInformNotifyModel
    {
        /// <summary>系統編號</summary>
        public IList<String> REQUISITION_ID { get; set; }

        /// <summary>被知會人</summary>
        public IList<String> NOTIFY_BY { get; set; }

        /// <summary>被知會角色</summary>
        public IList<String> ROLE_ID { get; set; }
    }

    #endregion

    /// <summary>
    /// 表單簽核歷程
    /// </summary>
    public class FormSignOff
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>關卡名稱</summary>
        public string PROCESS_NAME { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>職稱名稱</summary>
        public string TITLE_NAME { get; set; }

        /// <summary>簽核人</summary>
        public string APPROVER_NAME { get; set; }

        /// <summary>簽核時間</summary>
        public DateTime APPROVER_TIME { get; set; }

        /// <summary>簽核結果</summary>
        public string RESULT_PROMPT { get; set; }

        /// <summary>簽何意見</summary>
        public string COMMENT { get; set; }
    }

    /// <summary>
    /// 表單(待簽核)(已逾時)列表(查詢)
    /// </summary>
    public class FormNextApprover
    {
        /// <summary>公司編號</summary>
        public string COMPANY_ID { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>關卡編號</summary>
        public string PROCESS_ID { get; set; }

        /// <summary>關卡名稱</summary>
        public string PROCESS_NAME { get; set; }

        /// <summary>表單編號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>代簽核人GUID</summary>
        public string APPROVER_GUID { get; set; }

        /// <summary>重要性</summary>
        public int PRIORITY { get; set; }

        /// <summary>處理時間(分鐘)</summary>
        public int HANDLE_MINUTE { get; set; }

        /// <summary>發送次數</summary>
        public Byte SENT_COUNT { get; set; }

        /// <summary>最後發信時間</summary>
        public DateTime SENT_LAST_TIME { get; set; }

        /// <summary>表單主旨</summary>
        public string FORM_SUBJECT { get; set; }

        /// <summary>收件時間</summary>
        public DateTime TIME_START { get; set; }

        /// <summary>簽核人編號</summary>
        public string APPROVER_ID { get; set; }

        /// <summary>簽核人</summary>
        public string APPROVER_NAME { get; set; }

        /// <summary>簽核人信箱</summary>
        public string APPROVER_EMAIL { get; set; }

        /// <summary>簽核人手機</summary>
        public string APPROVER_PHONE { get; set; }

        /// <summary>是否代理</summary>
        public Byte IS_AGENT { get; set; }

        /// <summary>是否代理文字</summary>
        public string IS_AGENT_TEXT { get; set; }

        /// <summary>被代理人</summary>
        public string ORIGIN_APPROVER { get; set; }

        /// <summary>被代理人姓名</summary>
        public string ORIGIN_APPROVER_NAME { get; set; }

        /// <summary>被代理人信箱</summary>
        public string ORIGIN_APPROVER_EMAIL { get; set; }
    }

    /// <summary>
    /// 表單(結案)列表(查詢)
    /// </summary>
    public class FormFinalApprover
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>簽核人編號</summary>
        public string APPROVER_ID { get; set; }

        /// <summary>簽核人編號</summary>
        public string APPROVER_NAME { get; set; }

        /// <summary>簽核人信箱</summary>
        public string APPROVER_EMAIL { get; set; }

        /// <summary>簽核人手機</summary>
        public string APPROVER_PHONE { get; set; }

        /// <summary>(原)簽核人編號</summary>
        public string ORIGIN_APPROVER { get; set; }

        /// <summary>(原)簽核人編號</summary>
        public string ORIGIN_APPROVER_NAME { get; set; }

        /// <summary>(原)簽核人信箱</summary>
        public string ORIGIN_APPROVER_EMAIL { get; set; }

        /// <summary>(原)簽核人手機</summary>
        public string ORIGIN_APPROVER_PHONE { get; set; }
    }

    #endregion
}