using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 內容評估表_補充意見
    /// </summary>
    [RoutePrefix("api/BPMPro/EvaluateContent_Replenish")]
    public class EvaluateContent_ReplenishController : ApiController
    {
        #region - 宣告 -

        EvaluateContent_ReplenishRepository evaluateContent_ReplenishRepository = new EvaluateContent_ReplenishRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 內容評估表_補充意見(查詢)
        /// </summary>    
        [Route("PostEvaluateContent_ReplenishSingle")]
        [HttpPost]
        public EvaluateContent_ReplenishViewModel PostEvaluateContent_ReplenishSingle([FromBody] EvaluateContent_ReplenishQueryModel query)
        {
            return evaluateContent_ReplenishRepository.PostEvaluateContent_ReplenishSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 內容評估表_補充意見(依此單內容重送)
        ///// </summary>
        //[Route("PutEvaluateContent_ReplenishRefill")]
        //[HttpPost]
        //public bool PutEvaluateContent_ReplenishRefill([FromBody] EvaluateContent_ReplenishQueryModel query)
        //{
        //    return evaluateContent_ReplenishRepository.PutEvaluateContent_ReplenishRefill(query);
        //}

        #endregion

        /// <summary>
        /// 內容評估表_補充意見(新增/修改/草稿)
        /// </summary>
        [Route("PutEvaluateContent_ReplenishSingle")]
        [HttpPost]
        public bool PutEvaluateContent_ReplenishSingle([FromBody] EvaluateContent_ReplenishViewModel model)
        {
            return evaluateContent_ReplenishRepository.PutEvaluateContent_ReplenishSingle(model);
        }

        /// <summary>
        /// 內容評估表_外購(填寫)
        /// </summary>
        [Route("PutEvaluateContent_ReplenishFillinSingle")]
        [HttpPost]
        public bool PutEvaluateContent_ReplenishFillinSingle([FromBody] EvaluateContent_ReplenishFillinConfig model)
        {
            return evaluateContent_ReplenishRepository.PutEvaluateContent_ReplenishFillinSingle(model);
        }

        /// <summary>
        /// 內容評估表_外購(意見)
        /// </summary>
        [Route("PutEvaluateContent_ReplenishOpinionSingle")]
        [HttpPost]
        public bool PutEvaluateContent_ReplenishOpinionSingle([FromBody] EvaluateContent_ReplenishOpinionConfig model)
        {
            return evaluateContent_ReplenishRepository.PutEvaluateContent_ReplenishOpinionSingle(model);
        }

        #endregion
    }
}
