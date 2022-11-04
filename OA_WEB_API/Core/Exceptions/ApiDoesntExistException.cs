using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace OA_WEB_API.Core.Exceptions
{
    /// <summary>
    /// 此Api名稱不存在
    /// </summary>
    public class ApiDoesntExistException : ApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDoesntExistException"/> class.
        /// </summary>
        public ApiDoesntExistException() : base("API 不存在")
        {
            ErrorId = "api_doesnt_exist";
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}