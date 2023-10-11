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
    [RoutePrefix("api/BPMPro/GeneralOrderReturnRefund")]
    public class GeneralOrderReturnRefundController : ApiController
    {
        #region - 宣告 -

        GeneralOrderReturnRefundRepository generalOrderReturnRefundRepository = new GeneralOrderReturnRefundRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 版權採購退貨折讓單(查詢)
        /// </summary>    
        [Route("PostGeneralOrderReturnRefundSingle")]
        [HttpPost]
        public GeneralOrderReturnRefundViewModel PostGeneralOrderReturnRefundSingle([FromBody] GeneralOrderReturnRefundQueryModel query)
        {
            return generalOrderReturnRefundRepository.PostGeneralOrderReturnRefundSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購退貨折讓單(依此單內容重送)
        ///// </summary>
        //[Route("PutGeneralOrderReturnRefundRefill")]
        //[HttpPost]
        //public bool PutGeneralOrderReturnRefundRefill([FromBody] GeneralOrderReturnRefundQueryModel query)
        //{
        //    return GeneralOrderReturnRefundRepository.PutGeneralOrderReturnRefundRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權採購退貨折讓單(新增/修改/草稿)
        /// </summary>
        [Route("PutGeneralOrderReturnRefundSingle")]
        [HttpPost]
        public bool PutGeneralOrderReturnRefundSingle([FromBody] GeneralOrderReturnRefundViewModel model)
        {
            return generalOrderReturnRefundRepository.PutGeneralOrderReturnRefundSingle(model);
        }

        #endregion
    }
}
