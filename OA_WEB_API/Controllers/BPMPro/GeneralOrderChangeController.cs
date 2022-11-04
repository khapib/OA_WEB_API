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
    /// 會簽管理系統 - 行政採購異動申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/GeneralOrderChange")]
    public class GeneralOrderChangeController : ApiController
    {
        #region - 宣告 -

        GeneralOrderChangeRepository generalOrderChangeRepository = new GeneralOrderChangeRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 行政採購異動申請單(查詢)
        /// </summary>    
        [Route("PostGeneralOrderChangeSingle")]
        [HttpPost]
        public GeneralOrderChangeViewModel PostGeneralOrderChangeSingle([FromBody] GeneralOrderChangeQueryModel query)
        {
            return generalOrderChangeRepository.PostGeneralOrderChangeSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 行政採購異動申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutGeneralOrderChangeRefill")]
        //[HttpPost]
        //public bool PutGeneralOrderChangeRefill([FromBody] GeneralOrderChangeQueryModel query)
        //{
        //    return generalOrderChangeRepository.PutGeneralOrderChangeRefill(query);
        //}

        #endregion

        /// <summary>
        /// 行政採購異動申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutGeneralOrderChangeSingle")]
        [HttpPost]
        public bool PutGeneralOrderChangeSingle([FromBody] GeneralOrderChangeViewModel model)
        {
            return generalOrderChangeRepository.PutGeneralOrderChangeSingle(model);
        }

        #endregion
    }
}
