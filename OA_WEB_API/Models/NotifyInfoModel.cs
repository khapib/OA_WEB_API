using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA_WEB_API.Models
{
    /// <summary>
    /// 簡訊
    /// </summary>
    public class SmsModel
    {
        /// <summary>
        /// 簡訊內容
        /// </summary>
        public string BODY { get; set; }
    }

    /// <summary>
    /// 信件
    /// </summary>
    public class EmailModel
    {
        /// <summary>寄件人</summary>
        public string FROM { get; set; }
        
        /// <summary>收件人 正本(以;分隔)</summary>
        public string TO { get; set; }

        /// <summary>收件人 副本(以;分隔)</summary>
        public string CC { get; set; }

        /// <summary>收件人 密件(以;分隔)</summary>
        public string BCC { get; set; }

        /// <summary>信件主旨</summary>
        public string SUBJECT { get; set; }

        /// <summary>信件本文</summary>
        public string BODY { get; set; }

        /// <summary>信件附件</summary>
        public string ATTACHMENTS { get; set; }

        /// <summary>寄件時間</summary>
        public string SEND_TIME = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

        /// <summary>訊息備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 信件內容分段處理
    /// </summary>
    public class EmailBodyModel
    {
        /// <summary>維護工作編號</summary>
        public int JOB_NUM { get; set; }

        /// <summary>信件本文</summary>
        public string BODY_SEGMENT { get; set; }
    }

    /// <summary>
    /// 信件內容分段處理
    /// </summary>
    public class EmailRecipientModel
    {
        /// <summary>維護工作編號</summary>
        public int JOB_NUM { get; set; }

        /// <summary>收件人</summary>
        public string RECIPIENT { get; set; }
    }
}
