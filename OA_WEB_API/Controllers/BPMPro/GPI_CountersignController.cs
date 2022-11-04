using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 四方四隅_會簽單
    /// </summary>
    [RoutePrefix("api/BPMPro/GPI_Countersign")]
    public class GPI_CountersignController : ApiController
    {
        #region - 宣告 -

        GPI_CountersignRepository GPI_countersignRepository = new GPI_CountersignRepository();

        #endregion

        #region  - 方法 - 
        /// <summary>
        /// 四方四隅_會簽單(查詢)
        /// </summary>    
        [Route("PostGPI_CountersignSingle")]
        [HttpPost]
        public GPI_CountersignViewModel PostGPI_CountersignSingle([FromBody] GPI_CountersignQueryModel query)
        {
            return GPI_countersignRepository.PostGPI_CountersignSingle(query);
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 四方四隅_會簽單(依此單內容重送)
        /// </summary>
        [Route("PutGPI_CountersignRefill")]
        [HttpPost]
        public bool PutGPI_CountersignRefill([FromBody] GPI_CountersignQueryModel query)
        {
            return GPI_countersignRepository.PutGPI_CountersignRefill(query);
        }

        #endregion

        /// <summary>
        /// 四方四隅_會簽單(新增/修改/草稿)
        /// </summary>
        [Route("PutGPI_CountersignSingle")]
        [HttpPost]
        public bool PutGPI_CountersignSingle([FromBody] GPI_CountersignViewModel model)
        {
            return GPI_countersignRepository.PutGPI_CountersignSingle(model);
        }
        #endregion
    }
}
