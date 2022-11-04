using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 會簽管理系統 - 訊息通知
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    ///  簽核流程(查詢條件)
    /// </summary>
    public class NotifyQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>關卡編號</summary>
        public string PROCESS_ID { get; set; }

        /// <summary>簽核人編號</summary>
        public string APPROVER_ID { get; set; }
    }

    #region - 信件通知  -

    /// <summary>
    /// 信件
    /// </summary>
    public class EmailModel
    {
        /// <summary>流水號</summary>
        public string AUTO_COUNTER { get; set; }

        /// <summary>信件主旨</summary>
        public string SUBJECT { get; set; }

        /// <summary>信件內容</summary>
        public string CONTENT { get; set; }

        /// <summary>寄件人</summary>
        public string FROM_LIST { get; set; }

        /// <summary>收件人</summary>
        public string TO_LIST { get; set; }

        /// <summary>副本</summary>
        public string CC_LIST { get; set; }

        /// <summary>密件</summary>
        public string BCC_LIST { get; set; }

        /// <summary>寄件時間</summary>
        public string MAIL_TIME = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

        /// <summary>散列碼,</summary>
        public string HASH_CODE { get; set; }

        /// <summary>收件者信箱(Flow3 相容欄位)</summary>
        public string FW3_TO_LIST { get; set; }

        /// <summary>收件者署名(Flow3 相容欄位)</summary>
        public string FW3_TO_NAME { get; set; }

        /// <summary>重要性</summary>
        public string PRIORITY { get; set; }
    }

    /// <summary>
    /// 信件內文格式
    /// </summary>
    public class EmailBody
    {
        /// <summary>基本資料</summary>
        public string FORM_DATA { get; set; }

        /// <summary>簽核歷程</summary>
        public string FLOW_DATA { get; set; }
    }

    /// <summary>
    /// 信件(最大編號+1)
    /// </summary>
    public class MaxSerialNo
    {
        /// <summary>流水號</summary>
        public Int64 AUTO_COUNTER { get; set; }

        /// <summary>散列碼</summary>
        public int HASH_CODE { get; set; }
    }

    #endregion

    #region - 知會通知  -

    /// <summary>
    /// 知會
    /// </summary>
    public class NoticeMode
    {
        /// <summary>流水號</summary>
        public Int64 UNIQUE_ID { get; set; }

        ///<summary>發送者帳號</summary>
        public string SENDER_ID { get; set; }

        ///<summary>發送者姓名</summary>
        public string SENDER_NAME { get; set; }

        ///<summary>主識別碼</summary>
        public Byte MAIN_TYPE { get; set; }

        ///<summary>副識別碼</summary>
        public Byte SUB_TYPE { get; set; }

        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>收件者帳號</summary>
        public string RECEIVER_ID { get; set; }

        /// <summary>收件者姓名</summary>
        public string RECEIVER_NAME { get; set; }

        /// <summary>主旨</summary>
        public string SUBJECT { get; set; }

        /// <summary>知會內容</summary>
        public string INFORMCONTENT { get; set; }

        ///<summary>發送時間</summary>
        public string SUBMIT_DATETIME { get; set; }

        ///<summary>是否已讀取</summary>
        public Byte READ_FLAG { get; set; }

        ///// <summary>發送者帳號</summary>
        //public string SENDER_ID = "#SYSTEM!";

        ///// <summary>發送者姓名</summary>
        //public string SENDER_NAME = "FlowSe7en System";

        ///// <summary>發送時間</summary>
        //public string SUBMIT_DATETIME = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

        ///// <summary>主識別碼</summary>
        //public Byte MAIN_TYPE = 1;

        ///// <summary>副識別碼</summary>
        //public Byte SUB_TYPE = 1;

        ///// <summary>是否已讀取</summary>
        //public Byte READ_FLAG = 0;
    }

    /// <summary>
    /// 知會通知(最大編號+1)
    /// </summary>
    public class MaxUniqueID
    {
        /// <summary>流水號</summary>
        public Int64 UNIQUE_ID { get; set; }
    }

    #endregion

    #region - 簡訊通知  -

    /// <summary>
    /// 簡訊
    /// </summary>
    public class SmsModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }

        /// <summary>通訊錄</summary>
        public List<PhoneBook> PHONE_BOOK_LIST { get; set; }

        /// <summary>簡訊內容</summary>
        public string CONTENT { get; set; }
    }

    /// <summary>
    /// 手機通訊錄
    /// </summary>
    public class PhoneBook
    {
        /// <summary>收件人</summary>
        public string TO_LIST { get; set; }

        /// <summary>手機號碼</summary>
        public string PHONE_NO { get; set; }
    }

    #endregion

    #region - 監控通知  -

    /// <summary>
    /// 訊息
    /// </summary>
    public class MessageModel
    {
        /// <summary>訊息內容</summary>
        public string CONTENT { get; set; }
    }

    #endregion
}