using OA_WEB_API.Models.OA;
using OA_WEB_API.Models;
using OA_WEB_API.Repository.BPMPro;
using OA_WEB_API.Repository.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OA_WEB_API.Controllers.OA
{
    /// <summary>
    /// BPM簽核狀況[OA]
    /// </summary>
    public class FormStateContentController : ApiController
    {
        #region - 宣告 -

        FormStateContentRepository formStateContentRepository = new FormStateContentRepository();

        #endregion

        #region - 方法 -

        #region - [OA]BPM表單狀態(查詢) -

        /// <summary>
        /// [OA]BPM表單狀態(查詢)
        /// </summary>    
        [Route("api/PostFormStateSingle")]
        [HttpPost]
        public string PostFormStateSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new StepFlowQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                STATE_END = request["StateEND"]
            };

            return formStateContentRepository.PostFormStateSingle(query);
        }
        #endregion

        #endregion
    }
}
