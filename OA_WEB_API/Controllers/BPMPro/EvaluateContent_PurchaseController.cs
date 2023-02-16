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
    /// 會簽管理系統 - 內容評估表_外購
    /// </summary>
    [RoutePrefix("api/BPMPro/EvaluateContent_Purchase")]
    public class EvaluateContent_PurchaseController : ApiController
    {
        #region - 宣告 -

        EvaluateContent_PurchaseRepository evaluateContent_PurchaseRepository = new EvaluateContent_PurchaseRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 內容評估表_外購(查詢)
        /// </summary>    
        [Route("PostEvaluateContent_PurchaseSingle")]
        [HttpPost]
        public EvaluateContent_PurchaseViewModel PostEvaluateContent_PurchaseSingle([FromBody] EvaluateContent_PurchaseQueryModel query)
        {
            return evaluateContent_PurchaseRepository.PostEvaluateContent_PurchaseSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 內容評估表_外購(依此單內容重送)
        ///// </summary>
        //[Route("PutEvaluateContent_PurchaseRefill")]
        //[HttpPost]
        //public bool PutEvaluateContent_PurchaseRefill([FromBody] EvaluateContent_PurchaseQueryModel query)
        //{
        //    return evaluateContent_PurchaseRepository.PutEvaluateContent_PurchaseRefill(query);
        //}

        #endregion

        /// <summary>
        /// 內容評估表_外購(新增/修改/草稿)
        /// </summary>
        [Route("PutEvaluateContent_PurchaseSingle")]
        [HttpPost]
        public bool PutEvaluateContent_PurchaseSingle([FromBody] EvaluateContent_PurchaseViewModel model)
        {
            return evaluateContent_PurchaseRepository.PutEvaluateContent_PurchaseSingle(model);
        }

        /// <summary>
        /// 內容評估表_外購(評估意見)
        /// </summary>
        [Route("PutEvaluateContent_PurchaseSingle")]
        [HttpPost]
        public bool PutEvaluateContent_PurchaseOpinionSingle([FromBody] EvaluateContent_PurchaseOpinionConfig model)
        {
            return evaluateContent_PurchaseRepository.PutEvaluateContent_PurchaseOpinionSingle(model);
        }

        #endregion
    }
}
