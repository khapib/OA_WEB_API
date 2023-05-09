using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 四方四隅_內容評估表_補充意見
    /// </summary>
    [RoutePrefix("api/BPMPro/GPI_EvaluateContentReplenish")]
    public class GPI_EvaluateContentReplenishController : ApiController
    {
        #region - 宣告 -

        GPI_EvaluateContentReplenishRepository GPI_evaluateContentReplenishRepository = new GPI_EvaluateContentReplenishRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 四方四隅_內容評估表_補充意見(查詢)
        /// </summary>    
        [Route("PostGPI_EvaluateContentReplenishSingle")]
        [HttpPost]
        public GPI_EvaluateContentReplenishViewModel PostGPI_EvaluateContentReplenishSingle([FromBody] GPI_EvaluateContentReplenishQueryModel query)
        {
            return GPI_evaluateContentReplenishRepository.PostGPI_EvaluateContentReplenishSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 四方四隅_內容評估表_補充意見(依此單內容重送)
        ///// </summary>
        //[Route("PutGPI_EvaluateContentReplenishRefill")]
        //[HttpPost]
        //public bool PutGPI_EvaluateContentReplenishRefill([FromBody] GPI_EvaluateContentReplenishQueryModel query)
        //{
        //    return GPI_evaluateContentReplenishRepository.PutGPI_EvaluateContentReplenishRefill(query);
        //}

        #endregion

        /// <summary>
        /// 四方四隅_內容評估表_補充意見(新增/修改/草稿)
        /// </summary>
        [Route("PutGPI_EvaluateContentReplenishSingle")]
        [HttpPost]
        public bool PutGPI_EvaluateContentReplenishSingle([FromBody] GPI_EvaluateContentReplenishViewModel model)
        {
            return GPI_evaluateContentReplenishRepository.PutGPI_EvaluateContentReplenishSingle(model);
        }

        /// <summary>
        /// 四方四隅_內容評估表_補充意見(填寫)
        /// </summary>
        [Route("PutGPI_EvaluateContentReplenishFillinSingle")]
        [HttpPost]
        public bool PutGPI_EvaluateContentReplenishFillinSingle([FromBody] GPI_EvaluateContentReplenishFillinConfig model)
        {
            return GPI_evaluateContentReplenishRepository.PutGPI_EvaluateContentReplenishFillinSingle(model);
        }

        /// <summary>
        /// 四方四隅_內容評估表_補充意見(意見)
        /// </summary>
        [Route("PutGPI_EvaluateContentReplenishOpinionSingle")]
        [HttpPost]
        public bool PutGPI_EvaluateContentReplenishOpinionSingle([FromBody] GPI_EvaluateContentReplenishOpinionConfig model)
        {
            return GPI_evaluateContentReplenishRepository.PutGPI_EvaluateContentReplenishOpinionSingle(model);
        }

        #endregion

    }
}
