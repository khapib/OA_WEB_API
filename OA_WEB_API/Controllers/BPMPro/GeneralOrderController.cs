using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 行政採購申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/GeneralOrder")]
    public class GeneralOrderController : ApiController
    {
        #region - 宣告 -

        GeneralOrderRepository generalOrderRepository = new GeneralOrderRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 行政採購申請單(查詢)
        /// </summary>    
        [Route("PostGeneralOrderSingle")]
        [HttpPost]
        public GeneralOrderViewModel PostGeneralOrderSingle([FromBody] GeneralOrderQueryModel query)
        {
            return generalOrderRepository.PostGeneralOrderSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 行政採購申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutGeneralOrderRefill")]
        //[HttpPost]
        //public bool PutGeneralOrderRefill([FromBody] GeneralOrderQueryModel query)
        //{
        //    return generalOrderRepository.PutGeneralOrderRefill(query);
        //}

        #endregion

        /// <summary>
        /// 行政採購申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutGeneralOrderSingle")]
        [HttpPost]
        public bool PutGeneralOrderSingle([FromBody] GeneralOrderViewModel model)
        {
            return generalOrderRepository.PutGeneralOrderSingle(model);
        }

        /// <summary>
        /// 行政採購申請單(原表單匯入子表單)
        /// </summary>
        [Route("PutGeneralOrderImportSingle")]
        [HttpPost]
        public GeneralOrderViewModel PutGeneralOrderImportSingle([FromBody] GeneralOrderViewModel model)
        {
            return generalOrderRepository.PutGeneralOrderImportSingle(model);
        }


        #endregion

    }
}
