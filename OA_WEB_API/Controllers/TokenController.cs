using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Dapper;
using Newtonsoft.Json;

using OA_WEB_API.Models;
using OA_WEB_API.Repository;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 權杖資料
    /// </summary>
    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {
        #region - 宣告 -

        TokenManager tokenManager = new TokenManager();

        #endregion

        /// <summary>
        /// 測試是否通過驗證
        /// </summary>
        [HttpPost]
        [Route("IsAuthenticated")]
        public bool IsAuthenticated()
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return tokenManager.IsAuthenticated(token);
        }
    }
}