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
    /// 會簽管理系統 - A攝影棚直播使用申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/AStudioLiveStreamUse")]
    public class AStudioLiveStreamUseController : ApiController
    {
        #region - 宣告 -

        AStudioLiveStreamUseRepository aStudioLiveStreamUseRepository = new AStudioLiveStreamUseRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// A攝影棚直播使用申請單(查詢)
        /// </summary>    
        [Route("PostAStudioLiveStreamUseSingle")]
        [HttpPost]
        public AStudioLiveStreamUseViewModel PostAStudioLiveStreamUseSingle([FromBody] AStudioLiveStreamUseQueryModel query)
        {
            return aStudioLiveStreamUseRepository.PostAStudioLiveStreamUseSingle(query);
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// A攝影棚直播使用申請單(依此單內容重送)
        /// </summary>
        //[Route("PutAStudioLiveStreamUseRefill")]
        //[HttpPost]
        //public bool PutAStudioLiveStreamUseRefill([FromBody] AStudioLiveStreamUseQueryModel query)
        //{
        //    return aStudioLiveStreamUseRepository.PutAStudioLiveStreamUseRefill(query);
        //}

        #endregion

        /// <summary>
        /// A攝影棚直播使用申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutAStudioLiveStreamUseSingle")]
        [HttpPost]
        public bool PutAStudioLiveStreamUseSingle([FromBody] AStudioLiveStreamUseViewModel model)
        {
            return aStudioLiveStreamUseRepository.PutAStudioLiveStreamUseSingle(model);
        }

        #endregion
    }
}
