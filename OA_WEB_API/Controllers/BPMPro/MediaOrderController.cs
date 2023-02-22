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
    /// 會簽管理系統 - 版權採購申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaOrder")]
    public class MediaOrderController : ApiController
    {
        #region - 宣告 -

        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 版權採購申請單(查詢)
        /// </summary>    
        [Route("PostMediaOrderSingle")]
        [HttpPost]
        public MediaOrderViewModel PostMediaOrderSingle([FromBody] MediaOrderQueryModel query)
        {
            return mediaOrderRepository.PostMediaOrderSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutMediaOrderRefill")]
        //[HttpPost]
        //public bool PuttMediaOrderRefill([FromBody] MediaOrderQueryModel query)
        //{
        //    return mediaOrderRepository.PutMediaOrderRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權採購申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaOrderSingle")]
        [HttpPost]
        public bool PutMediaOrderSingle([FromBody] MediaOrderViewModel model)
        {
            return mediaOrderRepository.PutMediaOrderSingle(model);
        }

        /// <summary>
        /// 版權採購申請單(原表單匯入子表單)
        /// </summary>
        [Route("PutMediaOrderImportSingle")]
        [HttpPost]
        public MediaOrderViewModel PutMediaOrderImportSingle([FromBody] MediaOrderViewModel model)
        {
            return mediaOrderRepository.PutMediaOrderImportSingle(model);
        }


        #endregion
    }
}
