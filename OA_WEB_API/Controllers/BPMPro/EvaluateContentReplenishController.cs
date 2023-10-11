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
    /// 會簽管理系統 - 內容評估表_補充意見
    /// </summary>
    [RoutePrefix("api/BPMPro/EvaluateContentReplenish")]
    public class EvaluateContentReplenishController : ApiController
    {
        #region - 宣告 -

        EvaluateContentReplenishRepository evaluateContentReplenishRepository = new EvaluateContentReplenishRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 內容評估表_補充意見(查詢)
        /// </summary>    
        [Route("PostEvaluateContentReplenishSingle")]
        [HttpPost]
        public EvaluateContentReplenishViewModel PostEvaluateContentReplenishSingle([FromBody] EvaluateContentReplenishQueryModel query)
        {
            return evaluateContentReplenishRepository.PostEvaluateContentReplenishSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 內容評估表_補充意見(依此單內容重送)
        ///// </summary>
        //[Route("PutEvaluateContentReplenishRefill")]
        //[HttpPost]
        //public bool PutEvaluateContentReplenishRefill([FromBody] EvaluateContentReplenishQueryModel query)
        //{
        //    return evaluateContentReplenishRepository.PutEvaluateContentReplenishRefill(query);
        //}

        #endregion

        /// <summary>
        /// 內容評估表_補充意見(新增/修改/草稿)
        /// </summary>
        [Route("PutEvaluateContentReplenishSingle")]
        [HttpPost]
        public bool PutEvaluateContentReplenishSingle([FromBody] EvaluateContentReplenishViewModel model)
        {
            return evaluateContentReplenishRepository.PutEvaluateContentReplenishSingle(model);
        }

        /// <summary>
        /// 內容評估表_補充意見(填寫)
        /// </summary>
        [Route("PutEvaluateContentReplenishFillinSingle")]
        [HttpPost]
        public bool PutEvaluateContentReplenishFillinSingle([FromBody] EvaluateContentReplenishFillinConfig model)
        {
            return evaluateContentReplenishRepository.PutEvaluateContentReplenishFillinSingle(model);
        }

        /// <summary>
        /// 內容評估表_補充意見(意見)
        /// </summary>
        [Route("PutEvaluateContentReplenishOpinionSingle")]
        [HttpPost]
        public bool PutEvaluateContentReplenishOpinionSingle([FromBody] EvaluateContentReplenishOpinionConfig model)
        {
            return evaluateContentReplenishRepository.PutEvaluateContentReplenishOpinionSingle(model);
        }

        #endregion

    }
}
