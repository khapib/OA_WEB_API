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
    /// 會簽管理系統 - 差旅費用報支單
    /// </summary>
    [RoutePrefix("api/BPMPro/StaffTravellingExpenses")]
    public class StaffTravellingExpensesController : ApiController
    {
        #region - 宣告 -

        StaffTravellingExpensesRepository staffTravellingExpensesRepository = new StaffTravellingExpensesRepository();

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 差旅費用報支單(查詢)
        /// </summary>    
        [Route("PostStaffTravellingExpensesSingle")]
        [HttpPost]
        public StaffTravellingExpensesViewModel PostStaffTravellingExpensesSingle([FromBody] StaffTravellingExpensesQueryModel query)
        {
            return staffTravellingExpensesRepository.PostStaffTravellingExpensesSingle(query);
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 差旅費用報支單(依此單內容重送)
        ///// </summary>
        //[Route("PutStaffTravellingExpensesRefill")]
        //[HttpPost]
        //public bool PutStaffTravellingExpensesRefill([FromBody] StaffTravellingExpensesQueryModel query)
        //{
        //    return staffTravellingExpensesRepository.PutStaffTravellingExpensesRefill(query);
        //}

        #endregion

        /// <summary>
        /// 差旅費用報支單(新增/修改/草稿)
        /// </summary>
        [Route("PutStaffTravellingExpensesSingle")]
        [HttpPost]
        public bool PutStaffTravellingExpensesSingle([FromBody] StaffTravellingExpensesViewModel model)
        {
            return staffTravellingExpensesRepository.PutStaffTravellingExpensesSingle(model);
        }

        #endregion
    }
}
