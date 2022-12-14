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
    /// 會簽管理系統 - 費用申請單
    /// </summary>
    [RoutePrefix("api/BPMPro/ExpensesReimburse")]

    public class ExpensesReimburseController : ApiController
    {
        #region - 宣告 -

        ExpensesReimburseRepository expensesReimburseRepository = new ExpensesReimburseRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 費用申請單(查詢)
        /// </summary>    
        [Route("PostExpensesReimburseSingle")]
        [HttpPost]
        public ExpensesReimburseViewModel PostExpensesReimburseSingle([FromBody] ExpensesReimburseQueryModel query)
        {
            return expensesReimburseRepository.PostExpensesReimburseSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 費用申請單(依此單內容重送)
        ///// </summary>
        //[Route("PutExpensesReimburseRefill")]
        //[HttpPost]
        //public bool PutExpensesReimburseRefill([FromBody] ExpensesReimburseQueryModel query)
        //{
        //    return expensesReimburseRepository.PutExpensesReimburseRefill(query);
        //}

        #endregion

        /// <summary>
        /// 費用申請單(新增/修改/草稿)
        /// </summary>
        [Route("PutExpensesReimburseSingle")]
        [HttpPost]
        public bool PutExpensesReimburseSingle([FromBody] ExpensesReimburseViewModel model)
        {
            return expensesReimburseRepository.PutExpensesReimburseSingle(model);
        }

        #endregion
    }
}
