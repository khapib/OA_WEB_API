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
    /// 會簽管理系統 - 停車證申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/ParkingPermit")]
    public class ParkingPermitController : ApiController
    {
        #region - 宣告 -

        ParkingPermitRepository parkingPermitRepository = new ParkingPermitRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 停車證申請單(查詢)
        /// </summary>    
        [Route("PostParkingPermitSingle")]
        [HttpPost]
        public ParkingPermitViewModel PostParkingPermitSingle([FromBody] ParkingPermitQueryModel query)
        {
            return parkingPermitRepository.PostParkingPermitSingle(query);
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 停車證申請單(依此單內容重送)
        /// </summary>
        [Route("PutParkingPermitRefill")]
        [HttpPost]
        public bool PutParkingPermitRefill([FromBody] ParkingPermitQueryModel query)
        {
            return parkingPermitRepository.PutParkingPermitRefill(query);
        }

        #endregion

        /// <summary>
        /// 停車證申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutParkingPermitSingle")]
        [HttpPost]
        public bool PutParkingPermitSingle([FromBody] ParkingPermitViewModel model)
        {
            return parkingPermitRepository.PutParkingPermitSingle(model);
        }

        #endregion
    }
}
