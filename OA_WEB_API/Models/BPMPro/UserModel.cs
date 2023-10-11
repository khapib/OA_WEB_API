using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 會簽管理系統 - 使用者資料
/// </summary>
namespace OA_WEB_API.Models
{
    /// <summary>
    /// 使用者資料(查詢)
    /// </summary>
    public class UserQueryModel
    {
        /// <summary>公司編號</summary>
        public string COMPANY_ID { get; set; }

        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>上一級主管編號</summary>
        public string MANAGER_ID { get; set; }

        /// <summary>上一級主管名稱</summary>
        public string MANAGER_NAME { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>員工姓名</summary>
        public string USER_NAME { get; set; }

        /// <summary>是否為管理者</summary>
        public bool? IS_MANAGER { get; set; }

        /// <summary>電子信箱</summary>
        public string EMAIL { get; set; }

        /// <summary>手機</summary>
        public string MOBILE { get; set; }

        /// <summary>職稱權重</summary>
        public int? JOB_GRADE { get; set; }

        /// <summary>職稱權重(開始)</summary>
        public int? JOB_GRADE_START { get; set; }

        /// <summary>職稱權重(結束)</summary>
        public int? JOB_GRADE_END { get; set; }

        /// <summary>員工流程</summary>
        public string USER_FLOW { get; set; }

        /// <summary>部門流程</summary>
        public string DEPT_FLOW { get; set; }
    }

    /// <summary>
    /// 使用者資訊
    /// </summary>
    public class UserInfo
    {
        /// <summary>使用者資料</summary>
        public IList<UserModel> USER_MODEL { get; set; }

        /// <summary>使用者角色</summary>
        public IList<string> USER_ROLE { get; set; }
    }

    /// <summary>
    /// 使用者資料
    /// </summary>
    public class UserModel
    {
        #region 公司及部門資訊

        /// <summary>公司編號</summary>
        public string COMPANY_ID { get; set; }

        /// <summary>部門編號(父節點)</summary>
        public string PARENT_DEPT_ID { get; set; }

        /// <summary>部門編號</summary>
        public string DEPT_ID { get; set; }

        /// <summary>組織層級編號(部級/處級/組級)</summary>
        public string GRADE_ID { get; set; }

        /// <summary>職稱編號</summary>
        public string TITLE_ID { get; set; }

        /// <summary>部門名稱(父節點)</summary>
        public string PARENT_DEPT_NAME { get; set; }

        /// <summary>部門名稱</summary>
        public string DEPT_NAME { get; set; }

        /// <summary>組織層級名稱(部級/處級/組級)</summary>
        public string GRADE_NAME { get; set; }

        /// <summary>職稱名稱</summary>
        public string TITLE_NAME { get; set; }

        /// <summary>組織層級權重</summary>
        public int GRADE_NUM { get; set; }

        /// <summary>部門排序</summary>
        public int SORT_ORDER { get; set; }

        #endregion

        #region 員工資料

        /// <summary>上一級主管編號</summary>
        public string MANAGER_ID { get; set; }

        /// <summary>上一級主管名稱</summary>
        public string MANAGER_NAME { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>員工姓名</summary>
        public string USER_NAME { get; set; }

        /// <summary>是否為管理者</summary>
        public Byte IS_MANAGER { get; set; }

        /// <summary>電子信箱</summary>
        public string EMAIL { get; set; }

        /// <summary>手機</summary>
        public string MOBILE { get; set; }

        /// <summary>在職狀態(1：在職、2：離職)</summary>
        public Byte JOB_STATUS { get; set; }

        /// <summary>職稱權重</summary>
        public int JOB_GRADE { get; set; }

        #endregion

        #region 辨識資料

        /// <summary>員工職稱</summary>
        public string USER_TITLE { get; set; }

        /// <summary>員工流程</summary>
        public string USER_FLOW { get; set; }

        /// <summary>部門流程</summary>
        public string DEPT_FLOW { get; set; }

        #endregion
                
    }
}