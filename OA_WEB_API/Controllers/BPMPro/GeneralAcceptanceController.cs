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
    /// 會簽管理系統 - 點驗收單
    /// </summary>
    [RoutePrefix("api/BPMPro/GeneralAcceptance")]
    public class GeneralAcceptanceController : ApiController
    {
        #region - 宣告 -

        GeneralAcceptanceRepository GeneralAcceptanceRepository = new GeneralAcceptanceRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 點驗收單(查詢)
        /// </summary>    
        [Route("PostGeneralAcceptanceSingle")]
        [HttpPost]
        public GeneralAcceptanceViewModel PostGeneralAcceptanceSingle([FromBody] GeneralAcceptanceQueryModel query)
        {
            return GeneralAcceptanceRepository.PostGeneralAcceptanceSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 點驗收單(依此單內容重送)
        ///// </summary>
        //[Route("PutGeneralAcceptanceRefill")]
        //[HttpPost]
        //public bool PutGeneralAcceptanceRefill([FromBody] GeneralAcceptanceQueryModel query)
        //{
        //    return GeneralAcceptanceRepository.PutGeneralAcceptanceRefill(query);
        //}

        #endregion

        /// <summary>
        /// 點驗收單(新增/修改/草稿)
        /// </summary>
        [Route("PutGeneralAcceptanceSingle")]
        [HttpPost]
        public bool PutGeneralAcceptanceSingle([FromBody] GeneralAcceptanceViewModel model)
        {
            return GeneralAcceptanceRepository.PutGeneralAcceptanceSingle(model);
        }

        /// <summary>
        /// 點驗收單(驗收簽核)
        /// </summary>
        [Route("PutGeneralAcceptanceApproveSingle")]
        [HttpPost]
        public bool PutGeneralAcceptanceApproveSingle([FromBody] GeneralAcceptanceApproveViewModel model)
        {
            return GeneralAcceptanceRepository.PutGeneralAcceptanceApproveSingle(model);
        }

        #endregion

    }
}
