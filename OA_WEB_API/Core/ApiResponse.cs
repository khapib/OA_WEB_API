using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace OA_WEB_API.Core
{
    /// <summary>
    /// Api回應結果
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public object Result { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public object Error { get; set; }
    }
}