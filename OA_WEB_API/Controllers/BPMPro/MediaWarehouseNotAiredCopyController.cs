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
    /// 會簽管理系統 - 尚未播出檔拷貝申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/MediaWarehouseNotAiredCopy")]
    public class MediaWarehouseNotAiredCopyController : ApiController
    {
        #region - 宣告 -

        MediaWarehouseNotAiredCopyRepository mediaWarehouseNotAiredCopyRepository = new MediaWarehouseNotAiredCopyRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 尚未播出檔拷貝申請單(查詢)
        /// </summary>    
        [Route("PostMediaWarehouseNotAiredCopySingle")]
        [HttpPost]
        public MediaWarehouseNotAiredCopyViewModel PostMediaWarehouseNotAiredCopySingle([FromBody] MediaWarehouseNotAiredCopyQueryModel query)
        {
            return mediaWarehouseNotAiredCopyRepository.PostMediaWarehouseNotAiredCopySingle(query);
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 尚未播出檔拷貝申請單(依此單內容重送)
        /// </summary>
        [Route("PutMediaWarehouseNotAiredCopyRefill")]
        [HttpPost]
        public bool PutMediaWarehouseNotAiredCopyRefill([FromBody] MediaWarehouseNotAiredCopyQueryModel query)
        {
            return mediaWarehouseNotAiredCopyRepository.PutMediaWarehouseNotAiredCopyRefill(query);
        }

        #endregion

        /// <summary>
        /// 版權採購申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutMediaWarehouseNotAiredCopySingle")]
        [HttpPost]
        public bool PutMediaWarehouseNotAiredCopySingle([FromBody] MediaWarehouseNotAiredCopyViewModel model)
        {
            return mediaWarehouseNotAiredCopyRepository.PutMediaWarehouseNotAiredCopySingle(model);
        }

        #endregion
    }
}
