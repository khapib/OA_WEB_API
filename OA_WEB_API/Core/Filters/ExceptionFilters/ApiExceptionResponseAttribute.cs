﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

using OA_WEB_API.Core.Exceptions;
using OA_WEB_API.Utils;

namespace OA_WEB_API.Core.Filters.ExceptionFilters
{
    /// <summary>
    /// WebApi錯誤處理用的Exception Filter
    /// </summary>
    public class ApiExceptionResponseAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 引發例外狀況事件。
        /// </summary>
        /// <param name="actionExecutedContext">動作的內容。</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            // 將錯誤訊息轉成要回傳的ApiResponseResult
            var errorApiResponseResult = ExceptionUtils.ConvertToApiResponse(actionExecutedContext.Exception);

            // 重新打包回傳的訊息
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(errorApiResponseResult.StatusCode, errorApiResponseResult);
        }
    }
}