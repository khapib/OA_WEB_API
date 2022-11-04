using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 權杖資料
/// </summary>
namespace OA_WEB_API.Models
{
    /// <summary>
    /// 登入
    /// </summary>
    public class LogonModel
    {
        /// <summary>帳號</summary>
        public string USER_ID { get; set; }

        /// <summary>密碼</summary>
        public string USER_PW { get; set; }
    }

    /// <summary>
    /// 權杖
    /// </summary>
    public class TokenModel
    {
        /// <summary>Token</summary>
        public string ACCESS_TOKEN { get; set; }

        /// <summary>Refresh Token</summary>
        public string REFRESH_TOKEN { get; set; }

        /// <summary>幾秒過期</summary>
        public double EXPIRES_IN { get; set; }
    }

    /// <summary>
    /// 權杖儲存
    /// </summary>
    public class RefreshTokenModel
    {
        /// <summary>臉書編號</summary>
        public string FB_USER_ID { get; set; }

        /// <summary>重取 Token</summary>
        public string REFRESH_TOKEN { get; set; }

        /// <summary>過期日期</summary>
        public DateTime EXPIRY_DATE { get; set; }
    }

    /// <summary>
    /// 承載資料
    /// </summary>
    public class PayloadModel
    {
        /// <summary>使用者資訊</summary>
        public Object USER { get; set; }

        /// <summary>過期時間(秒)</summary>
        public int EXPIRED { get; set; }
    }
}