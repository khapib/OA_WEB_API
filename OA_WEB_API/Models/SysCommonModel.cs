using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

/// <summary>
/// 系統共通資訊
/// </summary>
namespace OA_WEB_API.Models
{
    #region - 測試 -

    #region - 會簽管理系統 - BPM刪除及結束表單 -

    /// <summary>
    /// 會簽管理系統 - BPM刪除及結束表單
    /// </summary>
    public class FormRemoveQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }
    }

    #endregion

    #endregion

    #region - 部門列表 -

    /// <summary>
    /// 部門列表DataViewModel
    /// </summary>
    public class DeptTree
    {
        /// <summary>上級部門編號</summary>
        public string PARENT_DEPT_ID { get; set; }

        /// <summary>上級部門名稱</summary>
        public string PARENT_DEPT_NAME { get; set; }

        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }
        
        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

    }

    #endregion

    #region - 主要部門及使用者資料 -

    /// <summary>
    /// 主要部門及使用者資料(查詢)
    /// </summary>
    public class UserInfoMainDeptModel
    {
        /// <summary>使用者員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>使用者部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>公司別</summary>
        public string COMPANY_ID { get; set; }
    }

    /// <summary>
    /// 主要部門及使用者資料
    /// </summary>
    public class UserInfoMainDeptViewModel
    {
        /// <summary>主要部門</summary>
        public DeptTree MAIN_DEPT { get; set; }

        /// <summary>使用者資料</summary>
        public UserInfoConfig USER_MODEL { get; set; }
    }

    public class UserInfoConfig : UserModel
    {
        public bool? IS_MAIN_JOB { get; set; }
    }

    #endregion

    #region - BPM表單狀態(查詢條件) -

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
    /// BPM表單狀態_回傳(查詢條件)
    /// </summary>
    public class StepFlowQueryRequestModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>是否表單已完結</summary>
        public string STATE_END { get; set; }

        /// <summary>確認是否要回傳</summary>
        public string REQUEST_FLG { get; set; }
    }

    #endregion
}