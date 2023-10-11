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
    /// 會簽管理系統 - 內容評估表
    /// </summary>
    [RoutePrefix("api/BPMPro/EvaluateContent")]
    public class EvaluateContentController : ApiController
    {
        #region - 宣告 -

        EvaluateContentRepository evaluateContentRepository = new EvaluateContentRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 內容評估表(查詢)
        /// </summary>    
        [Route("PostEvaluateContentSingle")]
        [HttpPost]
        public EvaluateContentViewModel PostEvaluateContentSingle([FromBody] EvaluateContentQueryModel query)
        {
            return evaluateContentRepository.PostEvaluateContentSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 內容評估表(依此單內容重送)
        ///// </summary>
        //[Route("PutEvaluateContentRefill")]
        //[HttpPost]
        //public bool PutEvaluateContentRefill([FromBody] EvaluateContentQueryModel query)
        //{
        //    return EvaluateContentRepository.PutEvaluateContentRefill(query);
        //}

        #endregion

        /// <summary>
        /// 內容評估表(新增/修改/草稿)
        /// </summary>
        [Route("PutEvaluateContentSingle")]
        [HttpPost]
        public bool PutEvaluateContentSingle([FromBody] EvaluateContentViewModel model)
        {
            return evaluateContentRepository.PutEvaluateContentSingle(model);
        }

        /// <summary>
        /// 內容評估表(填寫)
        /// </summary>
        [Route("PutEvaluateContentFillinSingle")]
        [HttpPost]
        public bool PutEvaluateContentFillinSingle([FromBody] EvaluateContentFillinConfig model)
        {
            return evaluateContentRepository.PutEvaluateContentFillinSingle(model);
        }

        /// <summary>
        /// 內容評估表(評估意見)
        /// </summary>
        [Route("PutEvaluateContentOpinionSingle")]
        [HttpPost]
        public bool PutEvaluateContentOpinionSingle([FromBody] EvaluateContentOpinionConfig model)
        {
            return evaluateContentRepository.PutEvaluateContentOpinionSingle(model);
        }

        /// <summary>
        /// 內容評估表(清除評估人員)
        /// </summary>
        [Route("PutEvaluateContentRemoveCountersignSingle")]
        [HttpPost]
        public void PutEvaluateContentRemoveCountersignSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new EvaluateContentQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
            };

            evaluateContentRepository.PutEvaluateContentRemoveCountersignSingle(query);
        }

        #endregion

    }
}
