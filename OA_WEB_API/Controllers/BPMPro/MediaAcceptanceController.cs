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
    /// 會簽管理系統 - 版權採購交片單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaAcceptance")]
    public class MediaAcceptanceController : ApiController
    {
        #region - 宣告 -

        MediaAcceptanceRepository mediaAcceptanceRepository = new MediaAcceptanceRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購交片單(查詢)
        /// </summary>    
        [Route("PostMediaAcceptanceSingle")]
        [HttpPost]
        public MediaAcceptanceViewModel PostMediaAcceptanceSingle([FromBody] MediaAcceptanceQueryModel query)
        {
            return mediaAcceptanceRepository.PostMediaAcceptanceSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購交片單(依此單內容重送)
        ///// </summary>
        //[Route("PutMediaAcceptanceRefill")]
        //[HttpPost]
        //public bool PutMediaAcceptanceRefill([FromBody] MediaAcceptanceQueryModel query)
        //{
        //    return mediaAcceptanceRepository.PutMediaAcceptanceRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權採購交片單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaAcceptanceSingle")]
        [HttpPost]
        public bool PutMediaAcceptanceSingle([FromBody] MediaAcceptanceViewModel model)
        {
            return mediaAcceptanceRepository.PutMediaAcceptanceSingle(model);
        }

        /// <summary>
        /// 版權採購交片單(驗收簽核)
        /// </summary>
        [Route("PutMediaAcceptanceApproveSingle")]
        [HttpPost]
        public bool PutMediaAcceptanceApproveSingle([FromBody] MediaAcceptanceApproveViewModel model)
        {
            return mediaAcceptanceRepository.PutMediaAcceptanceApproveSingle(model);
        }

        #endregion
    }
}
