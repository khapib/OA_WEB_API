using System.Collections.Generic;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 表單及簽核流程
    /// </summary>
    [RoutePrefix("api/BPMPro/Active")]
    public class ActiveController : ApiController
    {
        #region - 宣告 -

        TokenManager tokenManager = new TokenManager();
        FormRepository formRepository = new FormRepository();
        FlowRepository flowRepository = new FlowRepository();

        #endregion

        #region - 方法 -

        #region - 表單共用 -

        /// <summary>
        /// 單位主管審核(查詢)
        /// </summary>
        [HttpPost]
        [Route("PutFormAutoStart")]
        public string PutFormAutoStart([FromBody] FormAutoStart model)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? formRepository.PutFormAutoStart(model) : null;
        }

        /// <summary>
        /// 表單基本資料(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostFormData")]
        public FormData PostFormData([FromBody] FormQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? formRepository.PostFormData(query) : null;
        }

        /// <summary>
        /// 表單主旨(查詢)
        /// </summary>
        [HttpPost]
        [Route("GetFormSubject")]
        public string GetFormSubject([FromBody] FormQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? formRepository.GetFormSubject(query) : null;
        }

        #endregion

        #region - 簽核流程 -

        /// <summary>
        /// 單位主管審核(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostUnitApprover")]
        public IList<UnitApproverModel> PostUnitApprover([FromBody] FlowQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? flowRepository.PostUnitApprover(query) : null;
        }

        /// <summary>
        /// 上一級主管(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostSupervisor")]
        public IList<SupervisorModel> PostSupervisor([FromBody] FlowQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? flowRepository.PostSupervisor(query) : null;
        }

        /// <summary>
        /// 判斷(待簽核人)有無設定(代理人)
        /// </summary>
        [HttpPost]
        [Route("PostAgent")]
        public IList<AgentModel> PostAgent([FromBody] FlowQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? flowRepository.PostAgent(query) : null;
        }

        /// <summary>
        /// 簽核流程查詢(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostFormSignOff")]
        public IList<FormSignOff> PostFormSignOff([FromBody] FormQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? flowRepository.PostFormSignOff(query) : null;
        }

        /// <summary>
        /// 表單(待簽核)列表(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostNextApproverByPending")]
        public IList<FormNextApprover> PostNextApproverByPending([FromBody] FormQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? flowRepository.PostNextApproverByPending(query) : null;
        }

        /// <summary>
        /// 表單(已逾時)列表(查詢)
        /// </summary>
        [HttpPost]
        [Route("PostNextApproverByOverTime")]
        public IList<FormNextApprover> PostNextApproverByOverTime([FromBody] FormQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            return (tokenManager.IsAuthenticated(token)) ? flowRepository.PostNextApproverByOverTime(query) : null;
        }

        /// <summary>
        /// 表單(結束)執行
        /// </summary>
        [HttpPost]
        [Route("PutFormClose")]
        public void PutFormClose([FromBody] FormQueryModel query)
        {
            var token = HttpContext.Current.Request.Headers["Authoriaztion"].ToString();

            if (tokenManager.IsAuthenticated(token))
            {
                flowRepository.PutFormClose(query);
            }
        }

        /// <summary>
        /// (測試)表單(結束)執行
        /// </summary>
        [HttpPost]
        [Route("PutLogNextApprover")]
        public void PutLogNextApprover()
        {
            var guidList = new List<string>() 
            {
                "1ed7e06b-a6ce-4163-aed9-d6da95a6f814",
                "f644cd74-90d5-430b-9f6c-3db164d858db",
                "6f276d32-7bad-4110-b097-6e4e5191afc3",
                "ffd6568c-a68d-4533-ad92-1f138e3b1ed3",
                "645664b3-2a79-4118-9770-1da277bed397",
                "f55da61d-a538-43d3-9d3e-76bd7df28baf",
                "b57287e4-5b8c-405b-8501-07640c511831",
                "b1ed5072-555c-46f0-a5c1-d9d5f5885fde"
            };

            flowRepository.PutLogNextApprover(guidList);
        }

        #endregion

        #endregion
    }
}