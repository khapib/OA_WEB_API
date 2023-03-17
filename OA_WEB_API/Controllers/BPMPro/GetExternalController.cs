using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.BPMPro;


namespace OA_WEB_API.Controllers.BPMPro
{
    /// <summary>
    /// 外部起單
    /// </summary>
    public class GetExternalController : ApiController
    {
        #region - 宣告 -

        GetExternalRepository getExternalRepository = new GetExternalRepository();

        #endregion

        #region - 方法 -

        #region - 專案建立審核單(外部起單) -

        /// <summary>
        /// 專案建立審核單(外部起單)
        /// </summary>
        [Route("api/PutProjectReviewGetExternal")]
        [HttpPost]
        public GetExternalData PutProjectReviewGetExternal(ProjectReviewERPInfo model)
        {
            return getExternalRepository.PutProjectReviewGetExternal(model);
        }

        #endregion

        #region - 合作夥伴審核單(外部起單) -

        /// <summary>
        /// 合作夥伴審核單(外部起單)
        /// </summary>
        [Route("api/PutSupplierReviewGetExternal")]
        [HttpPost]
        public GetExternalData PutSupplierReviewGetExternal(SupplierReviewERPInfo model)
        {
            return getExternalRepository.PutSupplierReviewGetExternal(model);
        }

        #endregion

        #region - 費用申請單(外部起單) -

        /// <summary>
        /// 費用申請單(外部起單)
        /// </summary>
        [Route("api/PutExpensesReimburseGetExternal")]
        [HttpPost]
        public GetExternalData PutExpensesReimburseGetExternal(ExpensesReimburseERPInfo model)
        {
            return getExternalRepository.PutExpensesReimburseGetExternal(model);
        }

        #endregion

        #region - 行政採購類_(外部起單) -

        #region - 行政採購申請單(外部起單) -

        /// <summary>
        /// 行政採購申請單(外部起單)
        /// </summary>
        [Route("api/PutGeneralOrderGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralOrderGetExternal(GeneralOrderERPInfo model)
        {
            return getExternalRepository.PutGeneralOrderGetExternal(model);
        }

        #endregion

        #region - 行政採購異動申請單(外部起單) -

        /// <summary>
        /// 行政採購異動申請單(外部起單)
        /// </summary>
        [Route("api/PutGeneralOrderChangeGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralOrderChangeGetExternal(GeneralOrderChangeERPInfo model)
        {
            return getExternalRepository.PutGeneralOrderChangeGetExternal(model);
        }

        #endregion

        #region - 行政採購點驗收單(外部起單) -

        /// <summary>
        /// 行政採購點驗收單(外部起單)
        /// </summary>
        [Route("api/PutGeneralAcceptanceGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralAcceptanceGetExternal(GeneralAcceptanceERPInfo model)
        {
            return getExternalRepository.PutGeneralAcceptanceGetExternal(model);
        }

        #endregion

        #region - 行政採購請款單(外部起單) -

        /// <summary>
        /// 行政採購請款單(外部起單)
        /// </summary>
        [Route("api/PutGeneralInvoiceGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralInvoiceGetExternal(GeneralInvoiceERPInfo model)
        {
            return getExternalRepository.PutGeneralInvoiceGetExternal(model);
        }

        #endregion

        #endregion

        #region - 內容評估表(外部起單) -

        #region - 內容評估表(外部起單) -

        /// <summary>
        /// 內容評估表(外部起單)
        /// </summary>
        [Route("api/PutEvaluateContentGetExternal")]
        [HttpPost]
        public GetExternalData PutEvaluateContentGetExternal(EvaluateContentERPInfo model)
        {
            return getExternalRepository.PutEvaluateContentGetExternal(model);
        }


        #endregion

        #region - 內容評估表_補充意見(外部起單) -

        /// <summary>
        /// 內容評估表_補充意見(外部起單)
        /// </summary>
        [Route("api/PutEvaluateContentReplenishGetExternal")]
        [HttpPost]
        public GetExternalData PutEvaluateContentReplenishGetExternal(EvaluateContentReplenishERPInfo model)
        {
            return getExternalRepository.PutEvaluateContentReplenishGetExternal(model);
        }

        #endregion

        #endregion

        #region - 版權採購類_(外部起單) -

        #region - 版權採購申請單(外部起單) -

        /// <summary>
        /// 版權採購申請單(外部起單)
        /// </summary>
        [Route("api/PutMediaOrderGetExternal")]
        [HttpPost]
        public GetExternalData PutMediaOrderGetExternal(MediaOrderERPInfo model)
        {
            return getExternalRepository.PutMediaOrderGetExternal(model);
        }

        #endregion

        #region - 版權採購異動申請單(外部起單) -

        /// <summary>
        /// 版權採購異動申請單(外部起單)
        /// </summary>
        [Route("api/PutMediaOrderChangeGetExternal")]
        [HttpPost]
        public GetExternalData PutMediaOrderChangeGetExternal(MediaOrderChangeERPInfo model)
        {
            return getExternalRepository.PutMediaOrderChangeGetExternal(model);
        }

        #endregion

        #region - 版權採購交片單(外部起單) -

        /// <summary>
        /// 版權採購交片單(外部起單)
        /// </summary>
        [Route("api/PutMediaAcceptanceGetExternal")]
        [HttpPost]
        public GetExternalData PutMediaAcceptanceGetExternal(MediaAcceptanceERPInfo model)
        {
            return getExternalRepository.PutMediaAcceptanceGetExternal(model);
        }

        #endregion

        #region - 版權採購請款單(外部起單) -

        /// <summary>
        /// 版權採購交片單(外部起單)
        /// </summary>
        [Route("api/PutMediaInvoiceGetExternal")]
        [HttpPost]
        public GetExternalData PutMediaInvoiceGetExternal(MediaInvoiceERPInfo model)
        {
            return getExternalRepository.PutMediaInvoiceGetExternal(model);
        }

        #endregion

        #endregion

        #region - 四方四隅_內容評估表(外部起單) -

        #region - 四方四隅_內容評估表(外部起單) -

        /// <summary>
        /// 四方四隅_內容評估表(外部起單)
        /// </summary>
        [Route("api/PutGPI_EvaluateContentGetExternal")]
        [HttpPost]
        public GetExternalData PutGPI_EvaluateContentGetExternal(GPI_EvaluateContentERPInfo model)
        {
            return getExternalRepository.PutGPI_EvaluateContentGetExternal(model);
        }


        #endregion

        #region - 四方四隅_內容評估表_補充意見(外部起單) -

        /// <summary>
        /// 四方四隅_內容評估表_補充意見(外部起單)
        /// </summary>
        [Route("api/PutGPI_EvaluateContentReplenishGetExternal")]
        [HttpPost]
        public GetExternalData PutGPI_EvaluateContentReplenishGetExternal(GPI_EvaluateContentReplenishERPInfo model)
        {
            return getExternalRepository.PutGPI_EvaluateContentReplenishGetExternal(model);
        }

        #endregion

        #endregion

        #endregion
    }
}
