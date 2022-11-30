using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.ERP;

namespace OA_WEB_API.Controllers.ERP
{
    /// <summary>
    /// 回傳ERP資訊
    /// </summary>
    public class ResponseInfoController : ApiController
    {
        #region - 宣告 -

        ResponseInfoRepository responseInfoRepository = new ResponseInfoRepository();

        #endregion

        #region - 方法 -

        #region - 專案建立審核單 財務審核資訊_回傳ERP -

        /// <summary>
        /// 專案建立審核單 財務審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostProjectReviewFinanceSingle")]
        [HttpPost]
        public ProjectReviewFinanceRequest PostProjectReviewFinanceSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostProjectReviewFinanceSingle(query);
        }

        #endregion

        #region - 行政採購類_回傳ERP資訊 -

        #region - 行政採購申請單 申請審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購申請單 申請審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostGeneralOrderInfoSingle")]
        [HttpPost]
        public GeneralOrderInfoRequest PostGeneralOrderInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostGeneralOrderInfoSingle(query);
        }

        #endregion

        #region - 行政採購異動申請單 異動申請資訊_回傳ERP(暫時不需要) -

        ///// <summary>
        ///// 行政採購異動申請單 異動申請資訊_回傳ERP
        ///// </summary>    
        //[Route("api/PostGeneralOrderChangeInfoSingle")]
        //[HttpPost]
        //public GeneralOrderChangeInfoRequest PostGeneralOrderChangeInfoSingle()
        //{
        //    HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
        //    HttpRequestBase request = context.Request;

        //    var query = new RequestQueryModel()
        //    {
        //        REQUISITION_ID = request["RequisitionID"],
        //        REQUEST_FLG = bool.Parse(request["RequestFlg"])
        //    };

        //    return responseInfoRepository.PostGeneralOrderChangeInfoSingle(query);
        //}

        #endregion

        #region - 行政採購點驗收單 驗收審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購點驗收單 驗收審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostGeneralAcceptanceInfoSingle")]
        [HttpPost]
        public GeneralAcceptanceInfoRequest PostGeneralAcceptanceInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostGeneralAcceptanceInfoSingle(query);
        }

        #endregion

        #region - 行政採購請款單 財務審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購請款單 財務審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostGeneralInvoiceInfoSingle")]
        [HttpPost]
        public GeneralInvoiceInfoRequest PostGeneralInvoiceInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostGeneralInvoiceInfoSingle(query);
        }

        #endregion

        #endregion

        #region - 版權採購類_回傳ERP資訊 -

        #region - 行政採購申請單 申請審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購申請單 申請審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostMediaOrderInfoSingle")]
        [HttpPost]
        public MediaOrderInfoRequest PostMediaOrderInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostMediaOrderInfoSingle(query);
        }

        #endregion

        #endregion

        #endregion
    }
}
