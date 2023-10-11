using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.ERP;

namespace OA_WEB_API.Controllers.ERP
{
    /// <summary>
    /// BPM簽核狀況
    /// </summary>
    public class StepFlowController : ApiController
    {
        #region - 宣告 -

        StepFlowRepository stepFlowRepository = new StepFlowRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 手動同步BPM表單狀態
        /// </summary>    
        [Route("api/PostStepFlowSingle")]
        [HttpPost]
        public string PostStepFlowSingle([FromBody] StepFlowQueryModel query)
        {
            return stepFlowRepository.PostStepFlowSingle(query);
        }

        /// <summary>
        /// BPM更新ERP表單狀態(內容)
        /// </summary>    
        [Route("api/PostStepSignContentSingle")]
        [HttpPost]
        public StepSignResponse PostStepSignContentSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new StepFlowQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                STATE_END = request["StateEND"]
            };

            return stepFlowRepository.PostStepSignContentSingle(query);
        }

        /// <summary>
        /// BPM更新ERP表單狀態
        /// </summary>    
        [Route("api/PostStepSignSingle")]
        [HttpPost]
        public string PostStepSignSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new StepFlowQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                STATE_END = request["StateEND"]
            };
                        
            return stepFlowRepository.PostStepSignSingle(query);
        }

        #endregion

    }
}
