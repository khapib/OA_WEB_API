using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 跑馬申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/RollCard")]
    public class RollCardController : ApiController
    {
        #region - 宣告 -

        RollCardRepository rollCardRepository = new RollCardRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 跑馬申請單(查詢) 
        /// </summary>
        [Route("PostRollCardSingle")]
        [HttpPost]
        public RollCardViewModel PostRollCardSingle([FromBody] RollCardQueryModel query)
        {
            return rollCardRepository.PostRollCardSingle(query);
        }

        /// <summary>
        /// 跑馬申請單(依此單內容重送)
        /// </summary>
        [Route("PutRollCardRefill")]
        [HttpPost]
        public bool PutRollCardRefill([FromBody] RollCardQueryModel query)
        {
            return rollCardRepository.PutRollCardRefill(query);
        }

        /// <summary>
        /// 跑馬申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutRollCardSingle")]
        [HttpPost]
        public bool PutRollCardSingle([FromBody] RollCardViewModel model)
        {
            //CommLib.Logger.Info("REQUISITION_ID=" + model.APPLICANT_INFO.REQUISITION_ID);
            //CommLib.Logger.Info("DIAGRAM_ID=" + model.APPLICANT_INFO.DIAGRAM_ID);
            //CommLib.Logger.Info("PRIORITY=" + model.APPLICANT_INFO.PRIORITY);
            //CommLib.Logger.Info("DRAFT_FLAG=" + model.APPLICANT_INFO.DRAFT_FLAG);
            //CommLib.Logger.Info("FLOW_ACTIVATED=" + model.APPLICANT_INFO.FLOW_ACTIVATED);
            //CommLib.Logger.Info("APPLICANT_DEPT=" + model.APPLICANT_INFO.APPLICANT_DEPT);
            //CommLib.Logger.Info("APPLICANT_DEPT_NAME=" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME);
            //CommLib.Logger.Info("APPLICANT_ID=" + model.APPLICANT_INFO.APPLICANT_ID);

            return rollCardRepository.PutRollCardSingle(model);
        }

        /// <summary>
        /// 跑馬申請單(審核通過，同步至OA及字幕機)
        /// </summary>
        [Route("SetRollCardToCG")]
        [HttpPost]
        public bool SetRollCardToCG()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RollCardQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"]
            };

            return rollCardRepository.SetRollCardToCG(query);
        }

        #endregion
    }
}