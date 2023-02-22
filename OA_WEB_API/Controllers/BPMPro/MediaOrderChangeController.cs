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
    /// 會簽管理系統 - 版權採購異動申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaOrderChange")]
    public class MediaOrderChangeController : ApiController
    {
        #region - 宣告 -

        MediaOrderChangeRepository mediaOrderChangeRepository = new MediaOrderChangeRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 版權採購異動申請單(查詢)
        /// </summary>    
        [Route("PostMediaOrderChangeSingle")]
        [HttpPost]
        public MediaOrderChangeViewModel PostMediaOrderChangeSingle([FromBody] MediaOrderChangeQueryModel query)
        {
            return mediaOrderChangeRepository.PostMediaOrderChangeSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購異動申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutMediaOrderChangeRefill")]
        //[HttpPost]
        //public bool PutMediaOrderChangeRefill([FromBody] MediaOrderChangeQueryModel query)
        //{
        //    return mediaOrderChangeRepository.PutMediaOrderChangeRefill(query);
        //}

        #endregion

        /// <summary>
        /// 版權採購異動申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaOrderChangeSingle")]
        [HttpPost]
        public bool PutMediaOrderChangeSingle([FromBody] MediaOrderChangeViewModel model)
        {
            return mediaOrderChangeRepository.PutMediaOrderChangeSingle(model);
        }

        #endregion

    }
}
