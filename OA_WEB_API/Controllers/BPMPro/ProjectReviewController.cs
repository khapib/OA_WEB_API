using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 專案建立審核單
    /// </summary>
    [RoutePrefix("api/BPMPro/ProjectReview")]
    public class ProjectReviewController : ApiController
    {
        #region - 宣告 -

        ProjectReviewRepository projectReviewRepository = new ProjectReviewRepository();

        #endregion

        #region  - 方法 - 
        /// <summary>
        /// 專案建立審核單(查詢)
        /// </summary>    
        [Route("PostProjectReviewSingle")]
        [HttpPost]        
        public ProjectReviewViewModel PostProjectReviewSingle([FromBody] ProjectReviewQueryModel query)
        {
            return projectReviewRepository.PostProjectReviewSingle(query);
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 專案建立審核單(依此單內容重送)
        /// </summary>
        //[Route("PutProjectReviewRefill")]
        //[HttpPost]
        //public bool PutProjectReviewRefill([FromBody] ProjectReviewQueryModel query)
        //{
        //    return projectReviewRepository.PutProjectReviewRefill(query);
        //}

        #endregion

        /// <summary>
        /// 專案建立審核單(新增/修改/草稿)
        /// </summary>
        [Route("PutProjectReviewSingle")]
        [HttpPost]
        public bool PutProjectReviewSingle([FromBody] ProjectReviewViewModel model)
        {
            return projectReviewRepository.PutProjectReviewSingle(model);
        }
        #endregion
    }
}
