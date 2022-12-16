using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 訊息通知
    /// </summary>
    [RoutePrefix("api/BPMPro/Notify")]
    public class NotifyController : ApiController
    {
        #region - 宣告 -

        FlowRepository flowRepository = new FlowRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 取得流程引擎事件
        /// </summary>
        [Route("EventLog")]
        [HttpPost]
        public void NotifyEvent()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            CommLib.Logger.Debug(
                "【REQUISITION_ID】：" + request["RequisitionID"] +
                "【PROCESS_ID】：" + request["ProcessID"] +
                "【EVENT_ID】：" + request["EventID"]
                );
        }

        /// <summary>
        /// 接收流程引擎(待簽核)通知事件
        /// </summary>
        [Route("ByPending")]
        [HttpPost]
        public void ByPending()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                PROCESS_ID = request["ProcessID"],
                IS_ENABLE_SMS = bool.Parse(request["IsEnableSMS"])
            };

            notifyRepository.ByPending(query);
        }

        /// <summary>
        /// 接收流程引擎(同意後)觸發事件
        /// </summary>
        [Route("ByAgree")]
        [HttpPost]
        public void ByAgree()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                PROCESS_ID = request["ProcessID"]
            };

            //notifyRepository.ByAgree(query);
            flowRepository.PutNextApproverByAgree(query);
        }

        /// <summary>
        /// 接收流程引擎(不同意後)觸發事件
        /// </summary>
        [Route("ByDisagree")]
        [HttpPost]
        public void ByDisagree()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                PROCESS_ID = request["ProcessID"]
            };

            //notifyRepository.ByDisagree(query);
            flowRepository.PutNextApproverByDisagree(query);
        }

        #region 2022/09/28 Leon: 接收流程引擎-條件分支(不同意後)觸發事件

        /// <summary>
        /// 接收流程引擎-條件分支(不同意後)觸發事件
        /// </summary>
        [Route("ActiveProcessByDisagree")]
        [HttpPost]
        public void ActiveProcessByDisagree()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                PROCESS_ID = request["ProcessID"]
            };
                        
            flowRepository.PutActiveProcessByDisagree(query);
        }

        #endregion

        /// <summary>
        /// 接收流程引擎(已逾時)通知事件
        /// </summary>
        [Route("ByOverTime")]
        [HttpPost]
        public void ByOverTime()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                PROCESS_ID = request["ProcessID"]
            };

            notifyRepository.ByOverTime(query);
        }

        /// <summary>
        /// 接收流程引擎(結案)通知事件
        /// </summary>
        [Route("ByClose")]
        [HttpPost]
        public void ByClose()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"]
            };

            notifyRepository.ByClose(query);
        }

        /// <summary>
        /// 接收流程引擎(駁回結束)通知事件
        /// </summary>
        [Route("ByDisagreeClose")]
        [HttpPost]
        public void ByDisagreeClose()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"]
            };

            notifyRepository.ByDisagreeClose(query);
        }

        /// <summary>
        /// 接收流程引擎(特定人員)通知
        /// </summary>
        [Route("ByNotice")]
        [HttpPost]
        public void ByNotice()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                PROCESS_ID = request["ProcessID"],
                RECEIVER_ID = request["ReceiverID"],
                IS_ENABLE_SMS = bool.Parse(request["IsEnableSMS"])
            };

            notifyRepository.ByNotice(query);
        }

        #region - 2022/11/08 Leon: 接收流程引擎(特定知會通知)通知觸發事件 -

        /// <summary>
        /// 接收流程引擎(特定知會通知)通知
        /// </summary>
        [Route("ByInformNotify")]
        [HttpPost]
        public void ByInformNotify()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var inform = new InformNotifyModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                NOTIFY_BY = request["NotifyBy"],
                ROLE_ID = request["RoleID"]
            };

            notifyRepository.ByInformNotify(inform);
        }

        #endregion

        #region - 群體知會通知 -

        /// <summary>
        /// 群體知會通知
        /// </summary>
        [Route("ByGroupInformNotify")]
        [HttpPost]
        public bool ByGroupInformNotify([FromBody] GroupInformNotifyModel model)
        {
            return notifyRepository.ByGroupInformNotify(model);
        }

        #endregion

        /// <summary>
        /// 接收流程引擎(結案打包)通知，包含 簽呈、會簽單、四方四隅簽呈
        /// </summary>
        [Route("ByArchive")]
        [HttpPost]
        public void ByArchive()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"]
            };

            notifyRepository.ByArchive(query);
        }

        /// <summary>
        /// 寫入BPM表單單號: M表 [BPMFormNo] 申請(同意後)觸發事件
        /// </summary>    
        [Route("ByInsertBPMFormNo")]
        [HttpPost]
        public void PostInsertBPMFormNoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
            };

            notifyRepository.ByInsertBPMFormNo(query);
        }

        /// <summary>
        /// 新單 更新暫時主旨: M表 [FM7Subject] 申請(同意後)觸發事件
        /// </summary>    
        [Route("ByUpdateFM7Subject")]
        [HttpPost]
        public void ByUpdateFM7Subject()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new FormQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
            };

            notifyRepository.ByUpdateFM7Subject(query);
        }

        #endregion
    }
}