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
    /// 會簽管理系統 - 用印申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/OfficialStamp")]
    public class OfficialStampController : ApiController
    {
        #region - 宣告 -

        OfficialStampRepository officialStampRepository = new OfficialStampRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 用印申請單(查詢)
        /// </summary>    
        [Route("PostOfficialStampSingle")]
        [HttpPost]
        public OfficialStampViewModel PostOfficialStampSingle([FromBody] OfficialStampQueryModel query)
        {
            return officialStampRepository.PostOfficialStampSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 用印申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutOfficialStampRefill")]
        //[HttpPost]
        //public bool PutOfficialStampRefill([FromBody] OfficialStampQueryModel query)
        //{
        //    return officialStampRepository.PutOfficialStampRefill(query);
        //}

        #endregion

        /// <summary>
        /// 用印申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutOfficialStampSingle")]
        [HttpPost]
        public bool PutOfficialStampSingle([FromBody] OfficialStampViewModel model)
        {
            return officialStampRepository.PutOfficialStampSingle(model);
        }

        #endregion
    }
}
