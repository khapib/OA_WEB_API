﻿using System;
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

        #region - 費用申請單 審核資訊_回傳ERP -

        /// <summary>
        /// 費用申請單 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostExpensesReimburseInfoSingle")]
        [HttpPost]
        public ExpensesReimburseInfoRequest PostExpensesReimburseInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
        
            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostExpensesReimburseInfoSingle(query);
        }

        #endregion

        #region - 行政採購類_回傳ERP資訊 -

        #region - 行政採購申請單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購申請單 審核資訊_回傳ERP
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

        #region - 行政採購點驗收單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購點驗收單 審核資訊_回傳ERP
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

        #region - 行政採購請款單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購請款單 審核資訊_回傳ERP
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

        #region - 行政採購退貨折讓單 審核資訊_回傳ERP -

        /// <summary>
        /// 行政採購退貨折讓單 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostGeneralOrderReturnRefundInfoSingle")]
        [HttpPost]
        public GeneralOrderReturnRefundInfoRequest PostGeneralOrderReturnRefundInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostGeneralOrderReturnRefundInfoSingle(query);
        }

        #endregion

        #endregion

        #region -內容評估表_回傳ERP資訊-

        #region - 內容評估表 審核資訊_回傳ERP -

        /// <summary>
        /// 內容評估表 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostEvaluateContentInfoSingle")]
        [HttpPost]
        public EvaluateContentInfoRequest PostEvaluateContentInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostEvaluateContentInfoSingle(query);
        }

        #endregion

        #region - 內容評估表_補充意見 審核資訊_回傳ERP -

        /// <summary>
        /// 內容評估表_補充意見 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostEvaluateContentReplenishInfoSingle")]
        [HttpPost]
        public EvaluateContentInfoRequest PostEvaluateContentReplenishInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostEvaluateContentReplenishInfoSingle(query);
        }


        #endregion

        #endregion

        #region - 版權採購類_回傳ERP資訊 -

        #region - 版權採購申請單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購申請單 審核資訊_回傳ERP
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

        #region - 版權採購交片單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購交片單 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostMediaAcceptanceInfoSingle")]
        [HttpPost]
        public MediaAcceptanceInfoRequest PostMediaAcceptanceInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostMediaAcceptanceInfoSingle(query);
        }

        #endregion

        #region - 版權採購請款單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購請款單 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostMediaInvoiceInfoSingle")]
        [HttpPost]
        public MediaInvoiceInfoRequest PostMediaInvoiceInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostMediaInvoiceInfoSingle(query);
        }

        #endregion

        #region - 版權採購退貨折讓單 審核資訊_回傳ERP -

        /// <summary>
        /// 版權採購退貨折讓單 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostMediaOrderReturnRefundInfoSingle")]
        [HttpPost]
        public MediaOrderReturnRefundInfoRequest PostMediaOrderReturnRefundInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostMediaOrderReturnRefundInfoSingle(query);
        }

        #endregion

        #endregion

        #region -四方四隅_內容評估表_回傳ERP資訊-

        #region - 四方四隅_內容評估表 審核資訊_回傳ERP -

        /// <summary>
        /// 四方四隅_內容評估表 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostGPI_EvaluateContentInfoSingle")]
        [HttpPost]
        public EvaluateContentInfoRequest PostGPI_EvaluateContentInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostGPI_EvaluateContentInfoSingle(query);
        }

        #endregion

        #region - 四方四隅_內容評估表_補充意見 審核資訊_回傳ERP -

        /// <summary>
        /// 四方四隅_內容評估表_補充意見 審核資訊_回傳ERP
        /// </summary>    
        [Route("api/PostGPI_EvaluateContentReplenishInfoSingle")]
        [HttpPost]
        public EvaluateContentInfoRequest PostGPI_EvaluateContentReplenishInfoSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new RequestQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                REQUEST_FLG = bool.Parse(request["RequestFlg"])
            };

            return responseInfoRepository.PostGPI_EvaluateContentReplenishInfoSingle(query);
        }


        #endregion

        #endregion

        #endregion
    }
}
