using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Models.OA;
using OA_WEB_API.Repository.ERP;
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
    /// 回傳OA資訊
    /// </summary>
    public class ResponseOAInfoController : ApiController
    {
        #region - 宣告 -

        ResponseOAInfoRepository responseOAInfoRepository = new ResponseOAInfoRepository();

        #endregion

        #region - 方法 -

        #region - 拷貝申請單 回傳OA -

        /// <summary>
        /// 拷貝申請單 回傳OA
        /// </summary>    
        [Route("api/PostMediaWarehouseCopyResponseOASingle")]
        [HttpPost]
        public MediaWarehouseCopyResponseOA PostMediaWarehouseCopyResponseOASingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new StepFlowQueryRequestModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = request["RequestFlg"],
                STATE_END = request["StateEND"],
            };

            return responseOAInfoRepository.PostMediaWarehouseCopyResponseOASingle(query);
        }

        #endregion

        #endregion
    }
}
