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
    /// 會簽管理系統 - 版權採購退貨折讓單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaOrderReturnRefund")]
    public class MediaOrderReturnRefundController : ApiController
    {
        #region - 宣告 -

        MediaOrderReturnRefundRepository mediaOrderReturnRefundRepository = new MediaOrderReturnRefundRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 版權採購退貨折讓單(查詢)
        /// </summary>    
        [Route("PostMediaOrderReturnRefundSingle")]
        [HttpPost]
        public MediaOrderReturnRefundViewModel PostMediaOrderReturnRefundSingle([FromBody] MediaOrderReturnRefundQueryModel query)
        {
            return mediaOrderReturnRefundRepository.PostMediaOrderReturnRefundSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購退貨折讓單(依此單內容重送)
        ///// </summary>
        //[Route("PutMediaOrderReturnRefundRefill")]
        //[HttpPost]
        //public bool PutMediaOrderReturnRefundRefill([FromBody] MediaOrderReturnRefundQueryModel query)
        //{
        //    return mediaOrderReturnRefundRepository.PutMediaOrderReturnRefundRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權採購退貨折讓單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaOrderReturnRefundSingle")]
        [HttpPost]
        public bool PutMediaOrderReturnRefundSingle([FromBody] MediaOrderReturnRefundViewModel model)
        {
            return mediaOrderReturnRefundRepository.PutMediaOrderReturnRefundSingle(model);
        }

        #endregion
    }
}
