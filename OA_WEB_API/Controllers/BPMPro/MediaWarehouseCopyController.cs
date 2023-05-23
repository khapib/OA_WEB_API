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
    /// 會簽管理系統 - 拷貝申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaWarehouseCopy")]
    public class MediaWarehouseCopyController : ApiController
    {
        #region - 宣告 -

        MediaWarehouseCopyRepository mediaWarehouseCopyRepository = new MediaWarehouseCopyRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 拷貝申請單(查詢)
        /// </summary>    
        [Route("PostMediaWarehouseCopySingle")]
        [HttpPost]
        public MediaWarehouseCopyViewModel PostMediaWarehouseCopySingle([FromBody] MediaWarehouseCopyQueryModel query)
        {
            return mediaWarehouseCopyRepository.PostMediaWarehouseCopySingle(query);
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 拷貝申請單(依此單內容重送)
        /// </summary>
        [Route("PutMediaWarehouseCopyRefill")]
        [HttpPost]
        public bool PutMediaWarehouseCopyRefill([FromBody] MediaWarehouseCopyQueryModel query)
        {
            return mediaWarehouseCopyRepository.PutMediaWarehouseCopyRefill(query);
        }

        #endregion

        /// <summary>
        /// 拷貝申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaWarehouseCopySingle")]
        [HttpPost]
        public bool PutMediaWarehouseCopySingle([FromBody] MediaWarehouseCopyViewModel model)
        {
            return mediaWarehouseCopyRepository.PutMediaWarehouseCopySingle(model);
        }

        #endregion

    }
}
