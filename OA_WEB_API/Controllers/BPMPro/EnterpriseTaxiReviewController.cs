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
    /// 會簽管理系統 - 企業乘車審核單
    /// </summary>
    [RoutePrefix("api/BPMPro/EnterpriseTaxiReview")]
    public class EnterpriseTaxiReviewController : ApiController
    {
        #region - 宣告 -

        EnterpriseTaxiReviewRepository enterpriseTaxiReviewRepository = new EnterpriseTaxiReviewRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 企業乘車審核單(主單查詢)
        /// </summary>    
        [Route("PostEnterpriseTaxiReviewSingle")]
        [HttpPost]
        public EnterpriseTaxiReviewViewModel PostEnterpriseTaxiReviewSingle([FromBody] EnterpriseTaxiReviewQueryModel query)
        {
            return enterpriseTaxiReviewRepository.PostEnterpriseTaxiReviewSingle(query);
        }

        /// <summary>
        /// 企業乘車審核單(明細查詢)
        /// </summary>    
        [Route("PostEnterpriseTaxiReviewDetailsSingle")]
        [HttpPost]
        public EnterpriseTaxiReviewDetailsViewModel PostEnterpriseTaxiReviewDetailsSingle([FromBody] EnterpriseTaxiReviewDetailsQueryModel query)
        {
            return enterpriseTaxiReviewRepository.PostEnterpriseTaxiReviewDetailsSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 企業乘車審核單(依此單內容重送)
        ///// </summary>
        //[Route("PutEnterpriseTaxiReviewRefill")]
        //[HttpPost]
        //public bool PutEnterpriseTaxiReviewRefill([FromBody] EnterpriseTaxiReviewQueryModel query)
        //{
        //    return enterpriseTaxiReviewRepository.PutEnterpriseTaxiReviewRefill(query);
        //}

        #endregion

        /// <summary>
        /// 企業乘車審核單(新增/修改/草稿)
        /// </summary>
        [Route("PutEnterpriseTaxiReviewSingle")]
        [HttpPost]
        public bool PutEnterpriseTaxiReviewSingle([FromBody] EnterpriseTaxiReviewViewModel model)
        {
            return enterpriseTaxiReviewRepository.PutEnterpriseTaxiReviewSingle(model);
        }

        /// <summary>
        /// 企業乘車審核單(審核)
        /// </summary>
        [Route("PutEnterpriseTaxiReviewApproveSingle")]
        [HttpPost]
        public bool PutEnterpriseTaxiReviewApproveSingle([FromBody] EnterpriseTaxiReviewDetailsViewModel model)
        {
            return enterpriseTaxiReviewRepository.PutEnterpriseTaxiReviewApproveSingle(model);
        }

        #endregion
    }
}
