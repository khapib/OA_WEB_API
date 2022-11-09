using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OA_WEB_API.Models;
using OA_WEB_API.Repository;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 系統共通功能
    /// </summary>
    public class SysCommonController : ApiController
    {
        #region - 宣告 -

        SysCommonRepository sysCommonRepository = new SysCommonRepository();

        #endregion

        #region - 方法 -

        #region - 測試 -

        #region - 會簽管理系統 - BPM執行表單駁回結束 -

        /// <summary>
        /// 會簽管理系統 - BPM刪除及結束表單
        /// </summary>
        [Route("api/PostFormRemove")]
        [HttpPost]
        public string PostFormRemove([FromBody]FormRemoveQueryModel query)
        {
            return sysCommonRepository.PostFormRemove(query);
        }

        #endregion

        #endregion

        #region - 部門列表 -

        /// <summary>
        /// 部門列表
        /// </summary>
        [Route("api/GetGTVDeptTree")]
        [HttpGet]
        public IList<DeptTree> GetGTVDeptTree()
        {
            return sysCommonRepository.GetGTVDeptTree();
        }

        #endregion

        #region - 喚醒程式 -

        [HttpGet]
        [Route("api/GetWakeUp")]
        public HttpStatusCode GetWakeUp()
        {
            return HttpStatusCode.OK;
        }

        #endregion

        #endregion
    }
}
