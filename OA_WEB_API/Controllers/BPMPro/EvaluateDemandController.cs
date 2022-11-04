using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 需求評估單
    /// </summary>
    [RoutePrefix("api/BPMPro/EvaluateDemand")]
    public class EvaluateDemandController : ApiController
    {
        #region - 宣告 -

        EvaluateDemandRepository evaluateDemandRepository = new EvaluateDemandRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 需求評估單(查詢)
        /// </summary>    
        [Route("PostEvaluateDemandSingle")]
        [HttpPost]
        public EvaluateDemandViewModel PostEvaluateDemandSingle([FromBody] EvaluateDemandQueryModel query)
        {
            return evaluateDemandRepository.PostEvaluateDemandSingle(query);
        }

        /// <summary>
        /// 需求評估單(依此單內容重送)
        /// </summary>
        [Route("PutEvaluateDemandRefill")]
        [HttpPost]
        public bool PutEvaluateDemandRefill([FromBody] EvaluateDemandQueryModel query)
        {
            return evaluateDemandRepository.PutEvaluateDemandRefill(query);
        }

        /// <summary>
        /// 需求評估單(新增/修改/草稿)
        /// </summary>
        [Route("PutEvaluateDemandSingle")]
        [HttpPost]
        public bool PutEvaluateDemandSingle([FromBody] EvaluateDemandViewModel model)
        {
            return evaluateDemandRepository.PutEvaluateDemandSingle(model);
        }

        #endregion
    }
}
