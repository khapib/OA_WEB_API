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
    /// 會簽管理系統 - 預支單
    /// </summary>
    [RoutePrefix("api/BPMPro/AdvanceExpense")]
    public class AdvanceExpenseController : ApiController
    {
        #region - 宣告 -

        AdvanceExpenseRepository advanceExpenseRepository = new AdvanceExpenseRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 預支單(查詢)
        /// </summary>    
        [Route("PostAdvanceExpenseSingle")]
        [HttpPost]
        public AdvanceExpenseViewModel PostAdvanceExpenseSingle([FromBody] AdvanceExpenseQueryModel query)
        {
            return advanceExpenseRepository.PostAdvanceExpenseSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 預支單(依此單內容重送)
        ///// </summary>
        //[Route("PutAdvanceExpenseRefill")]
        //[HttpPost]
        //public bool PutAdvanceExpenseRefill([FromBody] AdvanceExpenseQueryModel query)
        //{
        //    return advanceExpenseRepository.PutAdvanceExpenseRefill(query);
        //}

        #endregion

        /// <summary>
        /// 預支單(新增/修改/草稿)
        /// </summary>
        [Route("PutAdvanceExpenseSingle")]
        [HttpPost]
        public bool PutAdvanceExpenseSingle([FromBody] AdvanceExpenseViewModel model)
        {
            return advanceExpenseRepository.PutAdvanceExpenseSingle(model);
        }

        #endregion
    }
}
