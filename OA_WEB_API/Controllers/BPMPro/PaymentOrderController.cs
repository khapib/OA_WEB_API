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
    /// 會簽管理系統 - 繳款單
    /// </summary>
    [RoutePrefix("api/BPMPro/PaymentOrder")]
    public class PaymentOrderController : ApiController
    {
        #region - 宣告 -

        PaymentOrderRepository paymentOrderRepository = new PaymentOrderRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 繳款單(查詢)
        /// </summary>    
        [Route("PostPaymentOrderSingle")]
        [HttpPost]
        public PaymentOrderViewModel PostPaymentOrderSingle([FromBody] PaymentOrderQueryModel query)
        {
            return paymentOrderRepository.PostPaymentOrderSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 繳款單(依此單內容重送)
        ///// </summary>
        //[Route("PutPaymentOrderRefill")]
        //[HttpPost]
        //public bool PutPaymentOrderRefill([FromBody] AdvanceExpenseQPaymentOrderQueryModelueryModel query)
        //{
        //    return paymentOrderRepository.PutPaymentOrderRefill(query);
        //}

        #endregion

        /// <summary>
        /// 繳款單(新增/修改/草稿)
        /// </summary>
        [Route("PutPaymentOrderSingle")]
        [HttpPost]
        public bool PutPaymentOrderSingle([FromBody] PaymentOrderViewModel model)
        {
            return paymentOrderRepository.PutPaymentOrderSingle(model);
        }

        #endregion
    }
}
