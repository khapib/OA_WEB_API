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
    /// 會簽管理系統 - 人員增補單
    /// </summary>
    [RoutePrefix("api/BPMPro/PersonnelSupplement")]
    public class PersonnelSupplementController : ApiController
    {
        #region - 宣告 -

        PersonnelSupplementRepository personnelSupplementRepository = new PersonnelSupplementRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 離職、人員增補單(查詢)
        /// </summary>    
        [Route("PostPersonnelSupplementSingle")]
        [HttpPost]
        public PersonnelSupplementViewModel PostPersonnelSupplementSingle([FromBody] PersonnelSupplementQueryModel query)
        {
            return personnelSupplementRepository.PostPersonnelSupplementSingle(query);
        }

        /// <summary>
        /// 人員增補單(依此單內容重送)
        /// </summary>
        [Route("PutPersonnelSupplementRefill")]
        [HttpPost]
        public bool PutPersonnelSupplementRefill([FromBody] PersonnelSupplementQueryModel query)
        {
            return personnelSupplementRepository.PutPersonnelSupplementRefill(query);
        }

        /// <summary>
        /// 人員增補單(新增/修改/草稿)
        /// </summary>
        [Route("PutPersonnelSupplementSingle")]
        [HttpPost]
        public bool PutPersonnelSupplementSingle([FromBody] PersonnelSupplementViewModel model)
        {
            return personnelSupplementRepository.PutPersonnelSupplementSingle(model);
        }

        #endregion

    }
}
