using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Controllers
{
    /// <summary>
    /// 會簽管理系統 - 測試申請單
    /// </summary>    
    public class TestController : ApiController
    {
        #region - 宣告 -

        TestRepository testRepository = new TestRepository();

        #endregion

        #region - 方法 -

        #region - TEST_01 -

        /// <summary>
        /// 測試01申請單(查詢) 
        /// </summary>
        [Route("api/PostTest01Single")]
        [HttpPost]
        public Test01ViewModel PostTest01Single([FromBody] Test01QueryModel query)
        {
            return testRepository.PostTest01Single(query);
        }

        /// <summary>
        /// 測試01申請單(依此單內容重送)
        /// </summary>
        [Route("api/PutTest01Refill")]
        [HttpPost]
        public bool PutTest01Refill([FromBody] Test01QueryModel query)
        {
            return testRepository.PutTest01Refill(query);
        }

        /// <summary>
        /// 測試01申請單(新增/修改/草稿)
        /// </summary>
        [Route("api/PutTest01Single")]
        [HttpPost]
        public bool PutTest01Single([FromBody] Test01ViewModel model)
        {
            //CommLib.Logger.Info("REQUISITION_ID=" + model.APPLICANT_INFO.REQUISITION_ID);
            //CommLib.Logger.Info("DIAGRAM_ID=" + model.APPLICANT_INFO.DIAGRAM_ID);
            //CommLib.Logger.Info("PRIORITY=" + model.APPLICANT_INFO.PRIORITY);
            //CommLib.Logger.Info("DRAFT_FLAG=" + model.APPLICANT_INFO.DRAFT_FLAG);
            //CommLib.Logger.Info("FLOW_ACTIVATED=" + model.APPLICANT_INFO.FLOW_ACTIVATED);
            //CommLib.Logger.Info("APPLICANT_DEPT=" + model.APPLICANT_INFO.APPLICANT_DEPT);
            //CommLib.Logger.Info("APPLICANT_DEPT_NAME=" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME);
            //CommLib.Logger.Info("APPLICANT_ID=" + model.APPLICANT_INFO.APPLICANT_ID);

            return testRepository.PutTest01Single(model);
        }

        #endregion

        #region  - TEST_02 - 

        /// <summary>
        /// 測試02申請單(查詢)
        /// </summary>
        [Route("api/PostTest02Single")]
        [HttpPost]
        public Test02ViewModel PostTest02Single([FromBody] Test02QueryModel query)
        {
            return testRepository.PostTest02Single(query);
        }

        /// <summary>
        /// 測試02申請單(依此單內容重送)
        /// </summary>
        [Route("api/PutTest02Refill")]
        [HttpPost]
        public bool PutTest02Refill([FromBody] Test02QueryModel query)
        {
            return testRepository.PutTest02Refill(query);
        }

        /// <summary>
        /// 測試02申請單(新增/修改/草稿)
        /// </summary>
        [Route("api/PutTest02Single")]
        [HttpPost]
        public bool PutTest02Single([FromBody] Test02ViewModel model)
        {
            return testRepository.PutTest02Single(model);
        }

        #endregion

        #region  - TEST_03 - 

        /// <summary>
        /// 測試03申請單(查詢)
        /// </summary>        
        [Route("api/PostTest03Single")]
        [HttpPost]
        public Test03ViewModel PostTest03Single([FromBody] Test03QueryModel query)
        {
            return testRepository.PostTest03Single(query);
        }

        /// <summary>
        /// 測試03申請單(新增/修改/草稿)
        /// </summary>
        [Route("api/PutTest03Single")]
        [HttpPost]
        public bool PutTest03Single([FromBody] Test03ViewModel model)
        {
            return testRepository.PutTest03Single(model);
        }

        #endregion

        #region  - TEST_05 - 

        /// <summary>
        /// 測試05申請單(查詢)
        /// </summary>        
        [Route("api/PostTest05Single")]
        [HttpPost]        
        public Test05ViewModel PostTest05Single([FromBody] Test05QueryModel query)
        {
            return testRepository.PostTest05Single(query);
        }

        /// <summary>
        /// 測試05申請單(依此單內容重送)
        /// </summary>
        [Route("api/PutTest05Refill")]
        [HttpPost]
        public bool PutTest05Refill([FromBody] Test05QueryModel query)
        {
            return testRepository.PutTest05Refill(query);
        }

        /// <summary>
        /// 測試05申請單(新增/修改/草稿)
        /// </summary>
        [Route("api/PutTest05Single")]
        [HttpPost]
        public bool PutTest05Single([FromBody] Test05ViewModel model)
        {
            return testRepository.PutTest05Single(model);
        }

        #endregion

        #region - TEST_外部程序 -

        /// <summary>
        /// 測試外部程序(匯入)
        /// </summary>        
        [Route("api/PutTestImportSingle")]
        [HttpPost]
        public bool PutTestImportSingle([FromBody] Test02QueryModel query)
        {
            return testRepository.PutTestImportSingle(query);
        }

        #endregion



        #region - TEST_回傳ERP -

        /// <summary>
        /// 測試外部程序(匯入)
        /// </summary>        
        [Route("api/PutTestReurnERPSingle")]
        [HttpPost]
        public bool PutTestReurnERPSingle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            var query = new StepFlowQueryModel()
            {
                REQUISITION_ID = request["RequisitionID"],
                STATE_END = request["StateEND"]
            };

            return testRepository.PutTestReurnERPSingle(query);
        }

        #endregion

        /// <summary>
        /// 用戶端SSL憑證
        /// </summary>
        [HttpGet]
        [Route("api/GetClientCertificate")]
        public string GetClientCertificate()
        {
            X509Certificate2 cert = Request.GetClientCertificate();

            return String.Format("憑證授權單位：{0}、憑證辨別名稱：{1}", cert.Issuer, cert.Subject);
        }

        /// <summary>
        /// 用戶端使用者IP
        /// </summary>
        [HttpGet]
        [Route("api/GetUserIP")]
        public string GetUserIP()
        {
            HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string userIP = String.Empty;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');

                if (addresses.Length != 0)
                {
                    userIP = addresses[0];
                }
            }

            userIP = context.Request.ServerVariables["REMOTE_ADDR"];

            return String.Format("現在時間：{0}、使用者位置：{1}", DateTime.Now.ToString(), userIP);
        }

        #endregion
    }
}