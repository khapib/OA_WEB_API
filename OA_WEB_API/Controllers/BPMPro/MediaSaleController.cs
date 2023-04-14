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
    /// 會簽管理系統 - 版權銷售申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaSale")]
    public class MediaSaleController : ApiController
    {
        #region - 宣告 -

        MediaSaleRepository mediaSaleRepository = new MediaSaleRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 版權銷售申請單(查詢)
        /// </summary>    
        [Route("PostMediaSaleSingle")]
        [HttpPost]
        public MediaSaleViewModel PostMediaSaleSingle([FromBody] MediaSaleQueryModel query)
        {
            return mediaSaleRepository.PostMediaSaleSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權銷售申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutMediaSaleRefill")]
        //[HttpPost]
        //public bool PutMediaSaleRefill([FromBody] MediaSaleQueryModel query)
        //{
        //    return mediaSaleRepository.PutMediaSaleRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權銷售申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaSaleSingle")]
        [HttpPost]
        public bool PutMediaSaleSingle([FromBody] MediaSaleViewModel model)
        {
            return mediaSaleRepository.PutMediaSaleSingle(model);
        }

        #endregion

    }
}
