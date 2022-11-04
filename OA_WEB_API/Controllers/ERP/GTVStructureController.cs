using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.ERP;

namespace OA_WEB_API.Controllers.ERP
{
    public class GTVStructureController : ApiController
    {
        #region - 宣告 -

        GTVStructureRepository gTVStructureRepository = new GTVStructureRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 提供外部系統(員工結構)(查詢)
        /// </summary>    
        [Route("api/PostGTVStaffSingle")]
        [HttpGet]
        public IList<GTVStaffModel> PostGTVStaffSingle()
        {
            return gTVStructureRepository.PostGTVStaffSingle();
        }

        #endregion
    }
}
