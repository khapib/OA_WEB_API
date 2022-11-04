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
    /// 會簽管理系統 - 合作夥伴審核單
    /// </summary>
    [RoutePrefix("api/BPMPro/SupplierReview")]
    public class SupplierReviewController : ApiController
    {
        #region - 宣告 -

        SupplierReviewRepository supplierCorrectRepository = new SupplierReviewRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 合作夥伴審核單(查詢)
        /// </summary>    
        [Route("PostSupplierReviewSingle")]
        [HttpPost]
        public SupplierReviewViewModel PostSupplierReviewSingle([FromBody] SupplierReviewQueryModel query)
        {
            return supplierCorrectRepository.PostSupplierReviewSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 合作夥伴審核單(依此單內容重送)
        ///// </summary>
        //[Route("PutSupplierReviewRefill")]
        //[HttpPost]
        //public bool PutSupplierReviewRefill([FromBody] SupplierReviewQueryModel query)
        //{
        //    return supplierCorrectRepository.PutSupplierReviewRefill(query);
        //}

        #endregion

        /// <summary>
        /// 合作夥伴審核單(新增/修改/草稿)
        /// </summary>
        [Route("PutSupplierReviewSingle")]
        [HttpPost]
        public bool PutSupplierReviewSingle([FromBody] SupplierReviewDataViewModel model)
        {
            return supplierCorrectRepository.PutSupplierReviewSingle(model);
        }

        #endregion

    }
}
