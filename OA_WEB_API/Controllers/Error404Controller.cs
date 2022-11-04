using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OA_WEB_API.Controllers
{
    public class Error404Controller : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        public object Get()
        {
            throw new Exception("找不到API...");
        }
    }
}