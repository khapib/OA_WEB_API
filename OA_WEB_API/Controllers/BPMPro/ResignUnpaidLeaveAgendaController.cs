﻿using OA_WEB_API.Models.BPMPro;
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
    /// 離職、留職停薪_手續表
    /// </summary>
    [RoutePrefix("api/BPMPro/ResignUnpaidLeaveAgenda")]
    public class ResignUnpaidLeaveAgendaController : ApiController
    {
        #region - 宣告 -

        ResignUnpaidLeaveAgendRepository resignUnpaidLeaveAgendRepository = new ResignUnpaidLeaveAgendRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 離職、留職停薪_手續表(查詢)
        /// </summary>    
        [Route("PostResignUnpaidLeaveAgendaSingle")]
        [HttpPost]
        public ResignUnpaidLeaveAgendaViewModel PostResignUnpaidLeaveAgendaSingle([FromBody] ResignUnpaidLeaveAgendaQueryModel query)
        {
            return resignUnpaidLeaveAgendRepository.PostResignUnpaidLeaveAgendaSingle(query);
        }

        /// <summary>
        /// 離職、留職停薪_手續表(新增/修改/草稿)
        /// </summary>
        [Route("PutResignUnpaidLeaveAgendaSingle")]
        [HttpPost]
        public bool PutResignUnpaidLeaveAgendaSingle([FromBody] ResignUnpaidLeaveAgendaViewModel model)
        {
            return resignUnpaidLeaveAgendRepository.PutResignUnpaidLeaveAgendaSingle(model);
        }

        #endregion
    }
}
