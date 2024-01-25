using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
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
    public class FormMainData : FormTree
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

    #region - BPM附件 -

    /// <summary>BPM附件</summary>
    public class FilesConfig
    {
        /// <summary>上傳者員工編號</summary>
        public string ACCOUNT_ID { get; set; }

        /// <summary>上傳者名字</summary>
        public string MEMBER_NAME { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>流程圖編號</summary>
        public string DIAGRAM_ID { get; set; }

        /// <summary>流程圖：關卡編號</summary>
        public string PROCESS_ID { get; set; }

        /// <summary>流程圖：關卡名稱</summary>
        public string PROCESS_NAME { get; set; }

        /// <summary>專案資料夾/附件編碼名稱</summary>
        public string N_FILE_NAME { get; set; }

        /// <summary>附件原本名稱</summary>
        public string O_FILE_NAME { get; set; }

        /// <summary>檔案大小</summary>
        public int FILE_SIZE { get; set; }

        /// <summary>正式：0；草稿：1</summary>
        public int? DRAFT_FLAG { get; set; }

        /// <summary>備註</summary>
        public string REMARK { get; set; }

        
    }

    #endregion

    #region - ERP附件 -

    /// <summary>
    /// ERP附件內容
    /// </summary>
    public class AttachmentMain
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>ERP附件</summary>
        public IList<AttachmentConfig> ATTACHMENT { get; set; }
    }

    /// <summary>
    /// ERP附件
    /// </summary>
    public class AttachmentConfig
    {
        /// <summary>附件檔名</summary>
        public string FILE_RENAME { get; set; }

        /// <summary>連結網址</summary>
        public string FILE_PATH { get; set; }

        /// <summary>附件原檔名</summary>
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

    #region - 關聯表單 -

    #region - 關聯表單(搜詢) -

    /// <summary>
    /// 關聯表單(搜詢條件)
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
    /// 關聯表單(搜詢)DataViewModel
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
    /// 關聯表單(搜詢)(完整內容)
    /// </summary>
    public class AssociatedFormViewModel
    {
        /// <summary>總筆數</summary>
        public int TOTAL { get; set; }

        /// <summary>總頁數</summary>
        public int TOTAL_PAGES { get; set; }

        /// <summary>關聯表單</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    #endregion

    /// <summary>
    /// 關聯表單內容
    /// </summary>
    public class AssociatedFormModel
    {
        /// <summary>主要表單:系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>關聯表單</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 關聯表單
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

    #region - 關聯表單(知會) -

    /// <summary>
    /// 關聯表單(知會)
    /// </summary>
    public class AssociatedFormNotifyModel
    {
        /// <summary>主要表單:系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>被知會人</summary>
        public IList<String> NOTIFY_BY { get; set; }

        /// <summary>被知會角色</summary>
        public IList<String> ROLE_ID { get; set; }
    }

    #endregion

    #endregion

    #region - BPM表單機能 -

    /// <summary>
    /// BPM表單機能
    /// </summary>
    public class BPMFormFunction
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>是否為草稿：
        /// 0.啟用
        /// 1.草稿
        /// </summary>
        public int DRAFT_FLAG { get; set; }
    }

    #endregion

    #region - 表單共用模組 -

    #region (BPM API共用)_ERP起單共用抬頭

    /// <summary>
    /// (BPM API共用)_ERP起單共用抬頭
    /// </summary>
    public class ImplementHeader : HeaderTitle
    {
        /// <summary>ERP 表單唯一碼</summary>
        public string FORM_NO { get; set; }
    }

    /// <summary>
    /// (BPM API共用)_BPM表單抬頭
    /// </summary>
    public class HeaderTitle
    {
        /// <summary>主旨</summary>
        public string FM7_SUBJECT { get; set; }

        /// <summary>BPM 表單單號</summary>
        public string BPM_FORM_NO { get; set; }
    }

    #endregion

    /// <summary>
    /// BPM_表單共用模組
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class BPMCommonModel<T>
    {
        /// <summary>表單資料表子名稱</summary>
        public string EXT { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>SqlParameter</summary>
        public List<SqlParameter> PARAMETER { get; set; }

        /// <summary>Models</summary>
        public List<T> MODEL { get; set; }
    }

    #region - 費用流程 -

    /// <summary>
    /// 費用流程_主表_後半部 設定
    /// </summary>
    public class ExpensesReimburseProcessLatterHalfConfig
    {
        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>金額</summary>
        public double AMOUNT { get; set; }
    }

    #endregion

    #region - 會簽簽核人員 -

    /// <summary>
    /// 會簽簽核人員
    /// </summary>
    public class ApproversConfig
    {
        /// <summary>簽核人員公司別編號</summary>
        public string APPROVER_COMPANY_ID { get; set; }

        /// <summary>簽核人員主要部門</summary>
        public string APPROVER_DEPT_MAIN_ID { get; set; }

        /// <summary>簽核人員部門</summary>
        public string APPROVER_DEPT_ID { get; set; }

        /// <summary>簽核人員編號</summary>
        public string APPROVER_ID { get; set; }

        /// <summary>簽核人員姓名</summary>
        public string APPROVER_NAME { get; set; }

    }

    #endregion

    #region - 銀行 -

    /// <summary>
    /// 銀行(國內/國外)_台幣/外幣/開票
    /// </summary>
    public class COMM_Bank : DOM_TWD_Bank
    {
        /// <summary>受款分行代碼</summary>
        public string BFCY_BANK_BRANCH_NO { get; set; }

        /// <summary>受款分行名稱</summary>
        public string BFCY_BANK_BRANCH_NAME { get; set; }

        /// <summary>SWIFT</summary>
        public string BFCY_BANK_SWIFT { get; set; }

        /// <summary>受款銀行地址</summary>
        public string BFCY_BANK_ADDRESS { get; set; }

        /// <summary>受款銀行國家</summary>
        public string BFCY_BANK_COUNTRY_AND_CITY { get; set; }

        /// <summary>中間銀行</summary>
        public string BFCY_BANK_IBAN { get; set; }
    }

    /// <summary>
    /// 銀行(國內)_台幣
    /// </summary>
    public class DOM_TWD_Bank
    {
        /// <summary>
        /// 匯款類型：
        /// DT.國內電匯(台幣)、
        /// DF.國內電匯(外幣)、
        /// FF.國外電匯、
        /// DD.票匯、
        /// CS.現金、
        /// OR.其他
        /// </summary>
        public string TX_CATEGORY { get; set; }

        /// <summary>受款帳號</summary>
        public string BFCY_ACCOUNT_NO { get; set; }

        /// <summary>受款帳號名稱/票據抬頭</summary>
        public string BFCY_ACCOUNT_NAME { get; set; }

        /// <summary>受款銀行代碼</summary>
        public string BFCY_BANK_NO { get; set; }

        /// <summary>受款銀行名稱</summary>
        public string BFCY_BANK_NAME { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY_NAME { get; set; }

        /// <summary>帳款聯絡人</summary>
        public string BFCY_NAME { get; set; }

        /// <summary>聯絡電話</summary>
        public string BFCY_TEL { get; set; }

        /// <summary>聯絡Email</summary>
        public string BFCY_EMAIL { get; set; }

    }

    #endregion

    #region - 憑證 -

    #region - 憑證明細 -

    /// <summary>
    /// 憑證明細
    /// </summary>
    public class InvoiceConfig
    {
        /// <summary>憑證號碼</summary>
        public string NUM { get; set; }

        /// <summary>憑證日期</summary>
        public string DATE { get; set; }

        /// <summary>憑證免稅額</summary>
        public double EXCL { get; set; }

        /// <summary>憑證免稅額_台幣</summary>
        public int EXCL_TWD { get; set; }

        /// <summary>憑證稅額</summary>
        public double TAX { get; set; }

        /// <summary>憑證稅額_台幣</summary>
        public int TAX_TWD { get; set; }

        /// <summary>憑證未稅金額</summary>
        public double NET { get; set; }

        /// <summary>憑證未稅金額_台幣</summary>
        public int NET_TWD { get; set; }

        /// <summary>憑證含稅金額</summary>
        public double GROSS { get; set; }

        /// <summary>憑證含稅金額_台幣</summary>
        public int GROSS_TWD { get; set; }

        /// <summary>憑證金額</summary>
        public double AMOUNT { get; set; }

        /// <summary>憑證金額</summary>
        public int AMOUNT_TWD { get; set; }
    }

    #endregion

    #region - 憑證細項 -

    /// <summary>
    /// 憑證細項
    /// </summary>
    public class InvoiceDetailConfig
    {
        /// <summary>行數編號</summary>
        public int ROW_NO { get; set; }

        /// <summary>憑證號碼</summary>
        public string NUM { get; set; }

        /// <summary>名稱</summary>
        public string NAME { get; set; }

        /// <summary>數量</summary>
        public int QUANTITY { get; set; }

        /// <summary>金額</summary>
        public double AMOUNT { get; set; }

        /// <summary>金額_台幣</summary>
        public int AMOUNT_TWD { get; set; }

        /// <summary>剩餘數量</summary>
        public int R_QUANTITY { get; set; }

        /// <summary>剩餘金額</summary>
        public double R_AMOUNT { get; set; }

        /// <summary>剩餘金額_台幣</summary>
        public int R_AMOUNT_TWD { get; set; }

        /// <summary>是否免稅[註記]</summary>
        public string IS_EXCL { get; set; }
    }

    #endregion

    #endregion

    #region - 商品 -

    #region - 行政商品 -

    /// <summary>
    /// 行政商品
    /// </summary>
    public class GeneralCommodityConfig
    {
        /// <summary>訂單行數編號</summary>
        public int ORDER_ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>型號</summary>
        public string MODEL { get; set; }

        /// <summary>規格</summary>
        public string SPECIFICATIONS { get; set; }

        /// <summary>數量</summary>
        public int QUANTITY { get; set; }

        /// <summary>單位</summary>
        public string UNIT { get; set; }
    }

    #endregion

    #region - 版權商品 -

    /// <summary>
    /// 版權商品
    /// </summary>
    public class MediaCommodityConfig
    {
        /// <summary>訂單行數編號</summary>
        public int ORDER_ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>影帶規格</summary>
        public string MEDIA_SPEC { get; set; }

        /// <summary>影片類型</summary>
        public string MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int END_EPISODE { get; set; }

        /// <summary>總集數</summary>
        public int ORDER_EPISODE { get; set; }

        /// <summary>驗收集數</summary>
        public int ACPT_EPISODE { get; set; }

        /// <summary>每集長度</summary>
        public int EPISODE_TIME { get; set; }
    }

    #endregion

    #endregion

    #region - 預算 -

    /// <summary>
    /// 預算
    /// </summary>
    public class BudgetConfig
    {
        /// <summary>預算 ERP唯一碼</summary>        
        public string FORM_NO { get; set; }

        /// <summary>預算編列年度</summary>
        public string CREATE_YEAR { get; set; }

        /// <summary>預算名稱</summary>
        public string NAME { get; set; }

        /// <summary>所屬部門</summary>
        public string OWNER_DEPT { get; set; }

        /// <summary>預算總額</summary>
        public int TOTAL { get; set; }

        /// <summary>可用預算金額</summary>
        public int AVAILABLE_BUDGET_AMOUNT { get; set; }

        /// <summary>使用預算金額</summary>
        public int USE_BUDGET_AMOUNT { get; set; }
    }

    #endregion

    #endregion

    #region - base64圖片 -

    /// <summary>
    /// base64圖片上傳設定
    /// </summary>
    public class Base64ImgSingletoSingleModel
    {
        /// <summary>暫存名稱_[調整圖片大小]</summary>
        public string IMG_NAME { get; set; }

        /// <summary>base64圖片碼</summary>
        public string PHOTO { get; set; }

        /// <summary>輸出名稱</summary>
        public string PRO_IMG_NAME { get; set; }

        /// <summary>檔案路徑</summary>
        public string FILE_PATH { get; set; }

        /// <summary>圖片設定值(大小)_[調整圖片大小]</summary>
        public int? IMG_SIZE { get; set; }
    }

    /// <summary>
    /// base64圖片輸出設定
    /// </summary>
    public class Base64ImgModel
    {
        /// <summary>路徑</summary>
        public string FILE_PATH { get; set; }

        /// <summary>附件副檔名</summary>
        public string FILE_EXTENSION { get; set; }
    }

    #endregion

    /// <summary>
    /// 整理檔案及資料
    /// </summary>
    public class OrganizeImgModel
    {
        /// <summary>表單資料表子名稱</summary>
        public string EXT { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>檔案路徑</summary>
        public string FILE_PATH { get; set; }
    }


    #region - 擴充方法 -

    #region - (擴充方法)_角色列表 -

    /// <summary>
    /// (擴充方法)_角色列表
    /// </summary>
    public class RolesModel
    {
        /// <summary>角色代碼</summary>
        public string ROLE_ID { get; set; }

        /// <summary>角色名稱</summary>
        public string ROLE_NAME { get; set; }

        /// <summary>員工編號</summary>
        public string USER_ID { get; set; }

        /// <summary>在職狀態(1：在職、2：離職)</summary>
        public int JOB_STATUS { get; set; }
    }

    #endregion

    #region - (擴充方法)_BPM系統申請單總表 -

    /// <summary>
    /// (擴充方法)_BPM系統申請單總表
    /// </summary>
    public class FSe7enSysRequisitionField
    {
        /// <summary>流水號</summary>
        public Int64 AUTO_COUNTER { get; set; }

        /// <summary>母表單系統編號</summary>
        public string PARENT_REQUISITION { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>BPM 表單單號</summary>
        public string BPM_FORM_NO { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }

        /// <summary>流程圖編號</summary>
        public string DIAGRAM_ID { get; set; }

        /// <summary>申請部門編號</summary>
        public string APPLICANT_DEPT { get; set; }

        /// <summary>申請人編號</summary>
        public string APPLICANT_ID { get; set; }

        /// <summary>表單狀態編號</summary>
        public Int16 STATUS { get; set; }

        /// <summary>表單狀態</summary>
        public string STATUS_NAME { get; set; }

        /// <summary>開始時間</summary>
        public Nullable<DateTime> TIME_START { get; set; }

        /// <summary>最後執行時間</summary>
        public Nullable<DateTime> TIME_LAST_ACTION { get; set; }
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

    #region - (擴充方法)_確認檔案複製路徑 -

    /// <summary>
    /// (擴充方法)_確認檔案複製路徑
    /// </summary>
    public class UploadFilePathModel
    {
        /// <summary>位置</summary>
        public string LOCATION { get; set; }

        /// <summary>路徑</summary>
        public string PATH { get; set; }

        /// <summary>表單代號</summary>
        public string IDENTIFY { get; set; }
    }

    #endregion

    #endregion
}