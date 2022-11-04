using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 會簽管理系統 - 測試表單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    #region - TEST_01 -

    /// <summary>
    /// 測試表單01(查詢條件)
    /// </summary>
    public class Test01QueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 測試表單01(完整表單)
    /// </summary>
    public class Test01ViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>表單內容</summary>
        public Test01Content TEST_01_CONTENT { get; set; }
    }

    /// <summary>
    /// 測試表單01(內容)
    /// </summary>
    public class Test01Content
    {
        /// <summary>備註</summary>
        public string REMARK { get; set; }
    }

    #endregion

    #region  - TEST_02 - 

    /// <summary>
    /// 測試02(查詢條件)
    /// </summary>
    public class Test02QueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 測試02(完整表單內容)
    /// </summary>
    public class Test02ViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>測試表單02設定</summary>
        public Test02Config TEST_02_CONFIG { get; set; }

        /// <summary>測試表單02列表清單</summary>
        public IList<Test02List> TEST_02_LIST { get; set; }
    }

    /// <summary>
    /// 測試02設定
    /// </summary>
    public class Test02Config
    {
        #region  - 測試表單02內容 - 
        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }
        /// <summary>備註</summary>
        public string REMARK { get; set; }
        #endregion
    }

    /// <summary>
    /// 測試02列表清單
    /// </summary>
    public class Test02List
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>執行年份</summary>
        public string IMPLPEMENTATION_YEAR { get; set; }

        /// <summary>描述</summary>
        public string NARRATIVE { get; set; }
    }
    #endregion

    #region  - TEST_03 - 

    /// <summary>
    /// 測試05(查詢條件)
    /// </summary>
    public class Test03QueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 測試05(完整表單內容)
    /// </summary>
    public class Test03ViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>測試表單05設定</summary>
        public Test03Config TEST_03_CONFIG { get; set; }

        /// <summary>測試表單05列表清單</summary>
        public IList<Test03List> TEST_03_LIST { get; set; }
    }

    /// <summary>
    /// 測試05設定
    /// </summary>
    public class Test03Config
    {
        #region  - 測試表單03內容 - 
        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }
        /// <summary>備註</summary>
        public string REMARK { get; set; }
        #endregion
    }

    /// <summary>
    /// 測試05列表清單
    /// </summary>
    public class Test03List
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>驗收部門</summary>
        public string ACPT_OWNER_DEPT { get; set; }

        /// <summary>驗收負責人編號</summary>
        public string ACPT_OWNER_ID { get; set; }
    }
    #endregion

    #region  - TEST_05 - 

    /// <summary>
    /// 測試05(查詢條件)
    /// </summary>
    public class Test05QueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 測試05(完整表單內容)
    /// </summary>
    public class Test05ViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>測試表單05設定</summary>
        public Test05Config TEST_05_CONFIG { get; set; }

        /// <summary>測試表單05列表清單</summary>
        public IList<Test05List> TEST_05_LIST { get; set; }
    }

    /// <summary>
    /// 測試05設定
    /// </summary>
    public class Test05Config
    {
        #region  - 測試表單05內容 - 
        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }
        /// <summary>備註</summary>
        public string REMARK { get; set; }
        #endregion
    }

    /// <summary>
    /// 測試05列表清單
    /// </summary>
    public class Test05List
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>執行年份</summary>
        public string IMPLPEMENTATION_YEAR { get; set; }

        /// <summary>描述</summary>
        public string NARRATIVE { get; set; }
    }
    #endregion
}