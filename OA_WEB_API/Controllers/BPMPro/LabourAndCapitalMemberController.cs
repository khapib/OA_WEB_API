using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 勞資委員投票
    /// </summary>
    [RoutePrefix("api/BPMPro/LabourAndCapitalMember")]
    public class LabourAndCapitalMemberController : ApiController
    {
        #region - 宣告 -

        LabourAndCapitalMemberRepository labourAndCapitalMemberRepository = new LabourAndCapitalMemberRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 勞資委員投票(登入者資訊)
        /// </summary>
        [Route("PostLabourAndCapitalMemberVoterInfoSingle")]
        [HttpPost]
        public LabourAndCapitalMemberVoterInfoConfig PostLabourAndCapitalMemberVoterInfoSingle(LabourAndCapitalMemberQueryModel query)
        {
            return labourAndCapitalMemberRepository.PostLabourAndCapitalMemberVoterInfoSingle(query);
        }

        /// <summary>
        /// 勞資委員投票(部門查詢)
        /// </summary>
        [Route("PostLabourAndCapitalMemberVoterDeptsSingle")]
        [HttpPost]
        public List<LabourAndCapitalMemberVoterDeptsConfig> PostLabourAndCapitalMemberVoterDeptsSingle(LabourAndCapitalMemberQueryModel query)
        {
            return labourAndCapitalMemberRepository.PostLabourAndCapitalMemberVoterDeptsSingle(query);
        }

        /// <summary>
        /// 勞資委員投票(查詢)
        /// </summary>
        [Route("PostLabourAndCapitalMemberSingle")]
        [HttpPost]
        public LabourAndCapitalMemberViewModel PostLabourAndCapitalMemberSingle(LabourAndCapitalMemberQueryModel query)
        {
            return labourAndCapitalMemberRepository.PostLabourAndCapitalMemberSingle(query);
        }

        /// <summary>
        /// 勞資委員投票(新建/調整)
        /// </summary>
        [Route("PutLabourAndCapitalMemberSingle")]
        [HttpPost]
        public bool PutLabourAndCapitalMemberSingle(LabourAndCapitalMemberViewModel model)
        {
            return labourAndCapitalMemberRepository.PutLabourAndCapitalMemberSingle(model);
        }

        /// <summary>
        /// 勞資委員投票(投票)
        /// </summary>
        [Route("PutLabourAndCapitalMemberVoteSingle")]
        [HttpPost]
        public bool PutLabourAndCapitalMemberVoteSingle(LabourAndCapitalMemberVoteConfig model)
        {
            return labourAndCapitalMemberRepository.PutLabourAndCapitalMemberVoteSingle(model);
        }

        /// <summary>
        /// 勞資委員投票(清除主部門票箱)
        /// </summary>
        [Route("PutLabourAndCapitalMemberClearMainDeptVoteSingle")]
        [HttpPost]
        public bool PutLabourAndCapitalMemberClearMainDeptVoteSingle(LabourAndCapitalMemberClearMainDeptVoteConfig model)
        {
            return labourAndCapitalMemberRepository.PutLabourAndCapitalMemberClearMainDeptVoteSingle(model);
        }

        /// <summary>
        /// 勞資委員投票(附件:新增F表)
        /// </summary>      
        [Route("PutLabourAndCapitalMemberFilesSingle")]
        [HttpPost]
        public List<LabourAndCapitalMemberFilesConfig> PutLabourAndCapitalMemberFilesSingle()
        {
            #region - 檔案上傳接收 -

            HttpRequest request = HttpContext.Current.Request;

            var model = new LabourAndCapitalMemberFilesConfig()
            {
                VOTE_YEAR = request.Form.Get("VOTE_YEAR"),
                ACCOUNT_ID = request.Form.Get("ACCOUNT_ID"),
                REMARK = request.Form.Get("REMARK")
            };

            #endregion

            return labourAndCapitalMemberRepository.PutLabourAndCapitalMemberFilesSingle(model);
        }

        /// <summary>
        /// 勞資委員投票(當選註記)
        /// </summary>
        [Route("PutLabourAndCapitalMemberMarkSingle")]
        [HttpPost]
        public bool PutLabourAndCapitalMemberMarkSingle(LabourAndCapitalMemberMarkConfig model)
        {
            return labourAndCapitalMemberRepository.PutLabourAndCapitalMemberMarkSingle(model);
        }


        #endregion
    }
}
