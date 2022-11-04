﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using OA_WEB_API.Core;
using OA_WEB_API.Core.Exceptions;

namespace OA_WEB_API.Utils
{
    public class ExceptionUtils
    {
        public static ApiResponse ConvertToApiResponse(Exception exception)
        {
            var errorApiResponseResult = new ApiResponse();

            if (exception is ApiException)
            {
                var apiException = exception as ApiException;
                errorApiResponseResult.StatusCode = apiException.StatusCode;
                errorApiResponseResult.Error = new
                {
                    ErrorId = apiException.ErrorId,
                    Message = apiException.Message
                };
            }
            else
            {
                errorApiResponseResult.StatusCode = HttpStatusCode.BadRequest;
                errorApiResponseResult.Error = new
                {
                    ErrorId = "",
                    Message = exception.Message
                };
            }

            return errorApiResponseResult;
        }
    }
}