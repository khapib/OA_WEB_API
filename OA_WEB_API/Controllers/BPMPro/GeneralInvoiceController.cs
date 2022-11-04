using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 行政採購請款單
    /// </summary>
    [RoutePrefix("api/BPMPro/GeneralInvoice")]
    public class GeneralInvoiceController : ApiController
    {
        #region - 宣告 -

        GeneralInvoiceRepository generalInvoiceRepository = new GeneralInvoiceRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 行政採購請款單(查詢)
        /// </summary>    
        [Route("PostGeneralInvoiceSingle")]
        [HttpPost]
        public GeneralInvoiceViewModel PostGeneralInvoiceSingle([FromBody] GeneralInvoiceQueryModel query)
        {
            return generalInvoiceRepository.PostGeneralInvoiceSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 行政採購請款單(依此單內容重送)
        ///// </summary>
        //[Route("PutGeneralInvoiceRefill")]
        //[HttpPost]
        //public bool PuttGeneralInvoiceRefill([FromBody] GeneralInvoiceQueryModel query)
        //{
        //    return GeneralInvoiceRepository.PutGeneralInvoiceRefill(query);
        //}

        #endregion

        /// <summary>
        /// 行政採購請款單(新增/修改/草稿)
        /// </summary>
        [Route("PutGeneralInvoiceSingle")]
        [HttpPost]
        public bool PutGeneralInvoiceSingle([FromBody] GeneralInvoiceViewModel model)
        {
            return generalInvoiceRepository.PutGeneralInvoiceSingle(model);
        }

        #endregion
    }
}
