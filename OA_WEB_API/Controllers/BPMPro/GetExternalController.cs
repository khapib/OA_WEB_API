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
    /// 外部接收
    /// </summary>
    public class GetExternalController : ApiController
    {
        #region - 宣告 -

        GetExternalRepository getExternalRepository = new GetExternalRepository();

        #endregion

        #region - 方法 -

        #region - 專案建立審核單(外部接收) -

        /// <summary>
        /// 專案建立審核單(外部接收)
        /// </summary>
        [Route("api/PutProjectReviewGetExternal")]
        [HttpPost]
        public GetExternalData PutProjectReviewGetExternal(ProjectReviewERPInfo model)
        {
            return getExternalRepository.PutProjectReviewGetExternal(model);
        }

        #endregion

        #region - 合作夥伴審核單(外部接收) -

        /// <summary>
        /// 合作夥伴審核單(外部接收)
        /// </summary>
        [Route("api/PutSupplierReviewGetExternal")]
        [HttpPost]
        public GetExternalData PutSupplierReviewGetExternal(SupplierReviewERPInfo model)
        {
            return getExternalRepository.PutSupplierReviewGetExternal(model);
        }

        #endregion

        #region - 行政採購申請單(外部接收) -

        /// <summary>
        /// 行政採購申請單(外部接收)
        /// </summary>
        [Route("api/PutGeneralOrderGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralOrderGetExternal(GeneralOrderERPInfo model)
        {
            return getExternalRepository.PutGeneralOrderGetExternal(model);
        }

        #endregion

        #region - 行政採購異動申請單(外部接收) -

        /// <summary>
        /// 行政採購異動申請單(外部接收)
        /// </summary>
        [Route("api/PutGeneralOrderChangeGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralOrderChangeGetExternal(GeneralOrderChangeERPInfo model)
        {
            return getExternalRepository.PutGeneralOrderChangeGetExternal(model);
        }

        #endregion

        #region - 行政採購點驗收單(外部接收) -

        /// <summary>
        /// 行政採購點驗收單(外部接收)
        /// </summary>
        [Route("api/PutGeneralAcceptanceGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralAcceptanceGetExternal(GeneralAcceptanceERPInfo model)
        {
            return getExternalRepository.PutGeneralAcceptanceGetExternal(model);
        }

        #endregion

        #region - 行政採購請款單(外部接收) -

        /// <summary>
        /// 行政採購請款單(外部接收)
        /// </summary>
        [Route("api/PutGeneralInvoiceGetExternal")]
        [HttpPost]
        public GetExternalData PutGeneralInvoiceGetExternal(GeneralInvoiceERPInfo model)
        {
            return getExternalRepository.PutGeneralInvoiceGetExternal(model);
        }

        #endregion

        #endregion
    }
}
