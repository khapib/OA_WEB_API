using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 系統共通功能資訊
    /// </summary>
    public class CommonController : ApiController
    {
        #region - 宣告 -

        CommonRepository commonRepository = new CommonRepository();

        #endregion

        #region - 方法 -

        #region - 測試 -

        /// <summary>
        /// 取得時間
        /// </summary>
        [Route("api/GetDateTime")]
        [HttpGet]
        public string GetDateTime()
        {
            return "現在時間：" + DateTime.Now.ToString();
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        [Route("api/SendMessage")]
        [HttpPost]
        public bool SendMessage()
        {
            try
            {
                var userIP = System.Web.HttpContext.Current.Request.UserHostAddress;

                botFunction.PushMessageAsync(String.Format("【{0}】結案打包歸檔通知\r\n表單編號：{1}\r\n申請人：{2}\r\n主旨：{3}\r\n核准日：{4}\r\n電腦 IP：{5}",
                    "簽呈",
                    "110節字第0099號",
                    "劉菁菁",
                    "節目部 娛樂百分百簽約藝人艾莉兒與雨婷外部邀約合作案",
                    "2021/12/07 14:00:00",
                    userIP
                ));

                return true;
            }
            catch (Exception ex)
            {
                botFunction.PushMessageAsync(String.Format("發送訊失敗，原因：{0}", ex.Message));

                return false;
            }
        }

        #endregion

        #region - 確認是否簽核中 -

        /// <summary>
        /// 確認是否簽核中
        /// </summary>        
        [Route("api/PostGTVInApproveProgress")]
        [HttpPost]
        public GTVApproveProgressResponse PostGTVInApproveProgress([FromBody] GTVInApproveProgress query)
        {
            return commonRepository.PostGTVInApproveProgress(query);
        }

        #endregion

        #region - 角色列表 -

        /// <summary>
        /// 角色列表
        /// </summary>
        [Route("api/GetRoles")]
        [HttpGet]
        public IList<RolesModel> GetRoles()
        {
            return CommonRepository.GetRoles();
        }

        #endregion

        #region - 表單列表 -

        /// <summary>
        /// BPM表單列表
        /// </summary>        
        [Route("api/PostGTVBPMFormTree")]
        [HttpPost]
        public IList<FormMainTree> PostGTVBPMFormTree([FromBody]FormFilter filter)
        {
            return commonRepository.PostGTVBPMFormTree(filter);
        }

        #endregion

        #region - 審核單列表 -

        /// <summary>
        /// 審核單列表
        /// </summary>        
        [Route("api/PostApproveForms")]
        [HttpPost]
        public IList<ApproveFormsConfig> PostApproveForms([FromBody] ApproveFormQuery query)
        {
            return commonRepository.PostApproveForms(query);
        }

        #endregion

        #region - 附件上傳 -

        /// <summary>
        /// 附件上傳(查詢)
        /// </summary>        
        [Route("api/PostAttachment")]
        [HttpPost]
        public IList<AttachmentConfig> PostAttachment([FromBody] FormQueryModel query)
        {
            return commonRepository.PostAttachment(query);
        }

        /// <summary>
        /// 附件上傳(新增)
        /// </summary>
        [Route("api/PutAttachment")]
        [HttpPost]
        public bool PutAttachment([FromBody] AttachmentMain model)
        {
            return commonRepository.PutAttachment(model);
        }

        #endregion

        #region - 關聯表單 -

        /// <summary>
        /// 關聯表單(搜詢)
        /// </summary>
        [Route("api/PostAssociatedFormSearch")]
        [HttpPost]
        public AssociatedFormViewModel PostAssociatedFormSearch([FromBody] AssociatedFormQuery query)
        {
            return commonRepository.PostAssociatedFormSearch(query);
        }

        /// <summary>
        /// 關聯表單(查詢)
        /// </summary>        
        [Route("api/PostAssociatedForm")]
        [HttpPost]
        public IList<AssociatedFormConfig> PostAssociatedForm([FromBody] FormQueryModel query)
        {
            return commonRepository.PostAssociatedForm(query);
        }

        /// <summary>
        /// 關聯表單(新增)
        /// </summary>
        [Route("api/PutAssociatedForm")]
        [HttpPost]
        public bool PutAssociatedForm([FromBody] AssociatedFormModel model)
        {
            return commonRepository.PutAssociatedForm(model);
        }

        /// <summary>
        /// 關聯表單(知會)
        /// </summary>
        [Route("api/PutAssociatedFormNotify")]
        [HttpPost]
        public bool PutAssociatedFormNotify([FromBody] AssociatedFormNotifyModel model)
        {
            return commonRepository.PutAssociatedFormNotify(model);
        }

        #endregion

        #region - BPM表單機能 -

        /// <summary>
        /// BPM表單機能
        /// </summary>
        [Route("api/PostBPMFormFunction")]
        [HttpPost]
        public bool PostBPMFormFunction([FromBody] BPMFormFunction model)
        {
            return commonRepository.PostBPMFormFunction(model);
        }


        #endregion

        #endregion
    }
}