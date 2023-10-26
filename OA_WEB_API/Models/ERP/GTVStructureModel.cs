using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.ERP
{
    /// <summary>
    /// 提供外部系統(員工結構)
    /// </summary>
    public class GTVStaffModel
    {
        #region - 提供外部系統 API 查詢_GTV人員資料表內容 -

        /// <summary>序列</summary>
        public int SEQ_ID { get; set; }

        /// <summary>公司編號</summary>
        public string COMPANY_ID { get; set; }

        /// <summary>上級部門編號</summary>
        public string PARENT_DEPT_ID { get; set; }

        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>局處中心編號</summary>
        public string OFFICE_ID { get; set; }

        /// <summary>組編號</summary>
        public string GROUP_ID { get; set; }

        /// <summary>部門層級編號</summary>
        public string GRADE_ID { get; set; }

        /// <summary>職位編號</summary>
        public string TITLE_ID { get; set; }

        /// <summary>上級部門名稱</summary>
        public string PARENT_DEPT_NAME { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>局處中心名稱</summary>
        public string OFFICE_NAME { get; set; }

        /// <summary>組名稱</summary>
        public string GROUP_NAME { get; set; }

        /// <summary>部門層級名稱</summary>
        public string GRADE_NAME { get; set; }

        /// <summary>職位名稱</summary>
        public string TITLE_NAME { get; set; }

        /// <summary>部門層級</summary>
        public short GRADE_NUM { get; set; }

        /// <summary>排序</summary>
        public short SORT_ORDER { get; set; }

        /// <summary>主管部門編號</summary>
        public string MANAGER_DEPT_ID { get; set; }

        /// <summary>主管員工編號</summary>
        public string MANAGER_ID { get; set; }

        /// <summary>主管姓名</summary>
        public string MANAGER_NAME { get; set; }

        /// <summary>使用者員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>使用者姓名</summary>
        public string USER_NAME { get; set; }

        /// <summary>是否為管理人員</summary>
        public byte IS_MANAGER { get; set; }

        /// <summary>信箱</summary>
        public string EMAIL { get; set; }

        /// <summary>手機號碼</summary>
        public string MOBILE { get; set; }

        /// <summary>職級</summary>
        public int JOB_GRADE { get; set; }

        /// <summary>工作狀態</summary>
        public int JOB_STATUS { get; set; }

        /// <summary>使用者標題</summary>
        public string USER_TITLE { get; set; }

        /// <summary>使用者簽核流程</summary>
        public string USER_FLOW { get; set; }

        /// <summary>使用者簽核流程關卡</summary>
        public byte USER_FLOW_LEVEL { get; set; }

        /// <summary>部門簽核流程</summary>
        public string DEPT_FLOW { get; set; }

        /// <summary>部門簽核流程關卡</summary>
        public byte DEPT_FLOW_LEVEL { get; set; }

        /// <summary>建立日期</summary>
        public DateTime CREATE_DATE { get; set; }

        /// <summary>修改日期</summary>
        public DateTime MODIFY_DATE { get; set; }

        #endregion
    }
}