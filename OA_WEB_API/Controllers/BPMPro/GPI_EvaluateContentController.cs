using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 四方四隅_內容評估表
    /// </summary>
    [RoutePrefix("api/BPMPro/GPI_EvaluateContent")]
    public class GPI_EvaluateContentController : ApiController
    {
        #region - 宣告 -

        GPI_EvaluateContentRepository GPI_evaluateContentRepository = new GPI_EvaluateContentRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 四方四隅_內容評估表(查詢)
        /// </summary>    
        [Route("PostGPI_EvaluateContentSingle")]
        [HttpPost]
        public GPI_EvaluateContentViewModel PostGPI_EvaluateContentSingle([FromBody] GPI_EvaluateContentQueryModel query)
        {
            return GPI_evaluateContentRepository.PostGPI_EvaluateContentSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 四方四隅_內容評估表(依此單內容重送)
        ///// </summary>
        //[Route("PutGPI_EvaluateContentRefill")]
        //[HttpPost]
        //public bool PutGPI_EvaluateContentRefill([FromBody] GPI_EvaluateContentQueryModel query)
        //{
        //    return GPI_evaluateContentRepository.PutGPI_EvaluateContentRefill(query);
        //}

        #endregion

        /// <summary>
        /// 四方四隅_內容評估表(新增/修改/草稿)
        /// </summary>
        [Route("PutGPI_EvaluateContentSingle")]
        [HttpPost]
        public bool PutGPI_EvaluateContentSingle([FromBody] GPI_EvaluateContentViewModel model)
        {
            return GPI_evaluateContentRepository.PutGPI_EvaluateContentSingle(model);
        }

        /// <summary>
        /// 四方四隅_內容評估表(填寫)
        /// </summary>
        [Route("PutGPI_EvaluateContentFillinSingle")]
        [HttpPost]
        public bool PutGPI_EvaluateContentFillinSingle([FromBody] GPI_EvaluateContentFillinConfig model)
        {
            return GPI_evaluateContentRepository.PutGPI_EvaluateContentFillinSingle(model);
        }

        /// <summary>
        /// 四方四隅_內容評估表(評估意見)
        /// </summary>
        [Route("PutGPI_EvaluateContentOpinionSingle")]
        [HttpPost]
        public bool PutGPI_EvaluateContentOpinionSingle([FromBody] GPI_EvaluateContentOpinionConfig model)
        {
            return GPI_evaluateContentRepository.PutGPI_EvaluateContentOpinionSingle(model);
        }

        /// <summary>
        /// 四方四隅_內容評估表(清除評估人員)
        /// </summary>
        [Route("PutGPI_EvaluateContentRemoveCountersignSingle")]
        [HttpPost]
        public void PutGPI_EvaluateContentRemoveCountersignSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new GPI_EvaluateContentQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
            };

            GPI_evaluateContentRepository.PutGPI_EvaluateContentRemoveCountersignSingle(query);
        }

        #endregion
    }
}
