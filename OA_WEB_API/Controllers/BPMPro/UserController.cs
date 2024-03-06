using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models;
using OA_WEB_API.Repository;
using OA_WEB_API.Repository.ERP;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 使用者資料
    /// </summary>
    [RoutePrefix("api/BPMPro/User")]
    public class UserController : ApiController
    {
        #region - 宣告 -

        TokenManager tokenManager = new TokenManager();
        UserRepository userRepository = new UserRepository();

        #endregion

        /// <summary>
        ///  登入成功，取得存取權杖
        /// </summary>
        [HttpPost]
        [Route("SignIn")]
        public TokenModel SignIn([FromBody] LogonModel model)
        {
            return userRepository.SignIn(model);
        }

        /// <summary>
        /// 使用者資料(單一使用者查詢；兼職會有多筆)
        /// </summary>
        [HttpPost]
        [Route("PostUserSingle")]
        public UserInfo PostUserSingle([FromBody] LogonModel model)
        {
            //var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();
            //return (tokenManager.IsAuthenticated(token)) ? userRepository.PostUserSingle(model) : null;

            return userRepository.PostUserSingle(model);
        }

        /// <summary>
        /// 使用者資料列表(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostUsers")]
        public IList<UserModel> PostUsers([FromBody] UserQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? userRepository.PostUsers(query) : null;
        }

        /// <summary>
        /// 使用者特殊簽核路徑
        /// </summary>
        [HttpPost]
        [Route("PostSetUserApproverSingle")]
        public IList<SetUserApproverModel> PostSetUserApproverSingle([FromBody] SetUserApproverQueryModel query)
        {
            return userRepository.PostSetUserApproverSingle(query);
        }

        /// <summary>
        /// 使用者員工結構資料(檢視)
        /// </summary>    
        [Route("GetUsersStructure")]
        [HttpGet]
        public IList<UsersStructure> GetUsersStructure()
        {
            return userRepository.GetUsersStructure();
        }
    }
}