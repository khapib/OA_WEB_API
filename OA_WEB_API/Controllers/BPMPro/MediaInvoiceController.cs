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
    /// 會簽管理系統 - 版權採購請款單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaInvoice")]
    public class MediaInvoiceController : ApiController
    {
        #region - 宣告 -

        MediaInvoiceRepository mediaInvoiceRepository = new MediaInvoiceRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購請款單(查詢)
        /// </summary>    
        [Route("PostMediaInvoiceSingle")]
        [HttpPost]
        public MediaInvoiceViewModel PostMediaInvoiceSingle([FromBody] MediaInvoiceQueryModel query)
        {
            return mediaInvoiceRepository.PostMediaInvoiceSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購請款單(依此單內容重送)
        ///// </summary>
        //[Route("PutMediaInvoiceRefill")]
        //[HttpPost]
        //public bool PutMediaInvoiceRefill([FromBody] MediaInvoiceQueryModel query)
        //{
        //    return mediaInvoiceRepository.PutMediaInvoiceRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權採購請款單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaInvoiceSingle")]
        [HttpPost]
        public bool PutMediaInvoiceSingle([FromBody] MediaInvoiceViewModel model)
        {
            return mediaInvoiceRepository.PutMediaInvoiceSingle(model);
        }

        /// <summary>
        /// 版權採購請款單(財務審核關卡-關聯表單(知會))
        /// </summary>
        [Route("PutMediaInvoiceNotifySingle")]
        [HttpPost]
        public bool PutMediaInvoiceNotifySingle([FromBody] MediaInvoiceQueryModel query)
        {
            return mediaInvoiceRepository.PutMediaInvoiceNotifySingle(query);
        }

        #endregion

    }
}
