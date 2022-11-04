using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 會簽管理系統 - 系統共通資訊
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    #region - 訊息通知 -

    //信件通知

    //簡訊通知

    //監控通知

    #endregion

    #region (BPM API共用)_ERP共用抬頭

        /// <summary>
        /// (BPM API共用)_ERP共用抬頭
        /// </summary>
        public class HeaderTitle
        {
            /// <summary>ERP 表單唯一碼</summary>
            public string FORM_NO { get; set; }

            /// <summary>主旨</summary>
            public string FM7_SUBJECT { get; set; }

            /// <summary>BPM 表單單號</summary>
            public string BPM_FORM_NO { get; set; }
        }

    #endregion



    #region - 確認是否已起單且簽核中或草稿中 -

    /// <summary>
    /// 確認是否已起單且簽核中或草稿中
    /// </summary>
    public class GTVInApproveProgress
    {
        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>ERP 表單唯一碼</summary>
        public string FORM_NO { get; set; }
    }

    /// <summary>
    /// 確認是否已起單且簽核中或草稿中(回傳)
    /// </summary>
    public class GTVApproveProgressResponse
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>是否有執行的單在送審中</summary>
        public bool vResult { get; set; }

        /// <summary>BPM表單狀態</summary>
        public string BPMStatus { get; set; }
    }

    #endregion

    #region - 表單列表 -

    /// <summary>
    /// 表單列表(篩選條件)
    /// </summary>
    public class FormFilter
    {
        /// <summary>表單代號</summary>
        public List<string> IDENTIFY { get; set; }
    }

    /// <summary>
    /// 表單資料夾分類DataViewModel
    /// </summary>
    public class FormMainData: FormTree
    {
        /// <summary>表單資料夾名稱</summary>
        public string FOLDER_NAME { get; set; }

        /// <summary>表單資料夾編號</summary>
        public string FOLDER_ID { get; set; }

    }

    /// <summary>
    /// 表單列表(完整內容)
    /// </summary>
    public class FormMainTree
    {
        /// <summary>表單資料夾名稱</summary>
        public string FOLDER_NAME { get; set; }

        /// <summary>表單列表內容</summary>
        public IList<FormTree> FORM_TREE { get; set; }
    }

    /// <summary>
    /// 表單列表內容
    /// </summary>
    public class FormTree
    {
        /// <summary>表單名稱</summary>
        public string FORM_NAME { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }
    }

    #endregion

    #region  - 審核單列表 - 

    /// <summary>
    /// 審核單列表(查詢條件)
    /// </summary>
    public class ApproveFormQuery
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>BPM 表單單號</summary>
        public string BPM_FORM_NO { get; set; }

        /// <summary>是否開啟子母表單篩選功能</summary>
        public bool PARENT { get; set; }
        
        /// <summary>審核狀態</summary>
        public int? STATUS { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }
        
        /// <summary>
        /// 申請人-可查：
        ///     姓名、
        ///     員編
        /// </summary>
        public string APPLICANT { get; set; }

        /// <summary>天數限制</summary>
        public int? DATEDIFF { get; set; }

        /// <summary>查看由 [新] 到 [舊] 的表單</summary>
        public bool RECENT { get; set; }
    }

    /// <summary>
    /// 審核單列表
    /// </summary>
    public class ApproveFormsConfig
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>BPM表單編號</summary>
        public string BPM_FROM_NO { get; set; }

        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        /// <summary>審核狀態</summary>
        public int STATUS { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>申請人員工編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>申請人員工姓名</summary>
        public string APPLICANT_NAME { get; set; }

        /// <summary>申請時間</summary>
        public DateTime APPLICANT_DATETIME { get; set; }
    }

    #endregion

    #region - 附件上傳 -

    /// <summary>
    /// 附件上傳內容
    /// </summary>
    public class AttachmentMain
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        public IList<AttachmentConfig> ATTACHMENT { get; set; }
    }

    /// <summary>
    /// 附件
    /// </summary>
    public class AttachmentConfig
    {
        /// <summary>連結網址</summary>
        public string FILE_PATH { get; set; }

        /// <summary>附件檔名</summary>
        public string FILE_NAME { get; set; }

        /// <summary>副檔名</summary>
        public string FILE_EXTENSION { get; set; }

        /// <summary>檔案大小</summary>
        public string FILE_SIZE { get; set; }

        /// <summary>建檔人</summary>
        public string CREATE_BY { get; set; }

        /// <summary>建檔時間</summary>
        public string CREATE_DATE { get; set; }

        /// <summary>附件描述</summary>
        public string DESCRIPTION { get; set; }
    }

    #endregion

    #region - 表單關聯 -

    #region - 表單關聯(搜詢) -

    /// <summary>
    /// 表單關聯(搜詢條件)
    /// </summary>
    public class AssociatedFormQuery
    {
        /// <summary>頁碼</summary>
        public int? PAGE { get; set; }

        /// <summary>使用者編號</summary>
        public string USER_ID { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>BPM 表單編號</summary>
        public string BPM_FORM_NO { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>表單主旨/(簽呈)收文編號</summary>
        public string QUERY { get; set; }

        /// <summary>申請部門</summary>
        public string APPLICANT_DEPT { get; set; }

        /// <summary>
        /// 申請人-可查：
        ///     姓名、
        ///     員編
        /// </summary>
        public string APPLICANT { get; set; }

        /// <summary>起始日期</summary>
        public string START_DATE { get; set; }

        /// <summary>結束日期</summary>
        public string END_DATE { get; set; }        
    }

    /// <summary>
    /// 表單關聯(搜詢)DataViewModel
    /// </summary>
    public class AssociatedFormDataViewModel
   {
        /// <summary>關聯表單:系統編號</summary>
        public string ASSOCIATED_REQUISITION_ID { get; set; }

        /// <summary>BPM 表單編號</summary>
        public string BPM_FORM_NO { get; set; }

        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        /// <summary>(申請/起案)部門編號</summary>
        public string APPLICANT_DEPT { get; set; }

        /// <summary>(申請/起案)部門名稱</summary>
        public string APPLICANT_DEPT_NAME { get; set; }

        /// <summary>申請人編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>申請人姓名</summary>
        public string APPLICANT_NAME { get; set; }

        /// <summary>填單人編號</summary>
        public string FILLER_ID { get; set; }

        /// <summary>簽核結果</summary>
        public string STATE { get; set; }

        /// <summary>申請時間</summary>
        public DateTime APPLICANT_DATE_TIME { get; set; }

        /// <summary>最近一次操作時間</summary>
        public DateTime TIME_LAST_ACTION { get; set; }
    }

    /// <summary>
    /// 表單關聯(搜詢)(完整內容)
    /// </summary>
    public class AssociatedFormViewModel
    {
        /// <summary>總筆數</summary>
        public int TOTAL { get; set; }

        /// <summary>總頁數</summary>
        public int TOTAL_PAGES { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    #endregion
    
    /// <summary>
    /// 表單關聯內容
    /// </summary>
    public class AssociatedFormModel
    {
        /// <summary>主要表單:系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 表單關聯
    /// </summary>
    public class AssociatedFormConfig
    {
        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>表單名稱</summary>
        public string FORM_NAME { get; set; }

        /// <summary>關聯表單:系統編號</summary>
        public string ASSOCIATED_REQUISITION_ID { get; set; }

        /// <summary>BPM 表單編號</summary>
        public string BPM_FORM_NO { get; set; }
        
        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        /// <summary>(申請/起案)部門名稱</summary>
        public string APPLICANT_DEPT_NAME { get; set; }

        /// <summary>申請人姓名</summary>
        public string APPLICANT_NAME { get; set; }

        /// <summary>申請時間</summary>
        public string APPLICANT_DATE_TIME { get; set; }

        /// <summary>表單路徑</summary>
        public string FORM_PATH { get; set; }

        /// <summary>狀態</summary>
        public string STATE { get; set; }
    }

    #endregion



    #region - (擴充方法)_共同表單區分 -

    /// <summary>
    /// (擴充方法)_共同表單區分
    /// </summary>
    public class FormDistinguishResponse
    {
        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>表單名稱</summary>
        public string FORM_NAME { get; set; }

        /// <summary>最終關卡編號</summary>
        public string END_PROCESS_ID { get; set; }
    }

    #endregion

    
}