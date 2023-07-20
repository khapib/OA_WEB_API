using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;

using OA_WEB_API.Models.BPMPro;

using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購退貨折讓單
    /// </summary>
    public class MediaOrderReturnRefundRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        FlowRepository flowRepository = new FlowRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #region FormRepsitory

        /// <summary>版權採購請款單</summary>
        MediaInvoiceRepository mediaInvoiceRepository = new MediaInvoiceRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購退貨折讓單(查詢)
        /// </summary>
        public MediaOrderReturnRefundViewModel PostMediaOrderReturnRefundSingle(MediaOrderReturnRefundQueryModel query)
        {
            var parameter = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            #region - 申請人資訊 -

            var CommonApplicantInfo = new BPMCommonModel<ApplicantInfo>()
            {
                EXT = "M",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter,
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApplicantInfoFunction(CommonApplicantInfo));
            var applicantInfo = jsonFunction.JsonToObject<ApplicantInfo>(strJson);

            #endregion

            #region - 版權採購退貨折讓單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaOrderReturnRefundTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundTitle>().FirstOrDefault();

            #endregion

            #region - 版權採購退貨折讓單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [MediaInvoiceRequisitionID] AS [MEDIA_INVOICE_REQUISITION_ID], ";
            strSQL += "     [MediaInvoiceSubject] AS [MEDIA_INVOICE_SUBJECT], ";
            strSQL += "     [MediaInvoiceBPMFormNo] AS [MEDIA_INVOICE_BPM_FORM_NO], ";
            strSQL += "     [MediaInvoiceERPFormNo] AS [MEDIA_INVOICE_ERP_FORM_NO], ";
            strSQL += "     [MediaInvoicePath] AS [MEDIA_INVOICE_PATH], ";
            strSQL += "     [MediaOrderPYMT_OrderTotal] AS [MEDIA_ORDER_PYMT_ORDER_TOTAL], ";
            strSQL += "     [MediaOrderPYMT_OrderTotal_CONV] AS [MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV], ";
            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [RF_ExclTotal] AS [RF_EXCL_TOTAL], ";
            strSQL += "     [RF_ExclTotal_TWD] AS [RF_EXCL_TOTAL_TWD], ";
            strSQL += "     [RF_TaxTotal] AS [RF_TAX_TOTAL], ";
            strSQL += "     [RF_TaxTotal_TWD] AS [RF_TAX_TOTAL_TWD], ";
            strSQL += "     [RF_NetTotal] AS [RF_NET_TOTAL], ";
            strSQL += "     [RF_NetTotal_TWD] AS [RF_NET_TOTAL_TWD], ";
            strSQL += "     [RF_GrossTotal] AS [RF_GROSS_TOTAL], ";
            strSQL += "     [RF_GrossTotal_TWD] AS [RF_GROSS_TOTAL_TWD], ";
            strSQL += "     [RF_AmountTotal] AS [RF_AMOUNT_TOTAL], ";
            strSQL += "     [RF_AmountTotal_TWD] AS [RF_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [ProcessMethod] AS [PROCESS_METHOD], ";
            strSQL += "     [FinancNote] AS [FINANC_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaOrderReturnRefundConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundConfig>().FirstOrDefault();

            #endregion

            #region - 版權採購退貨折讓單 已退貨商品明細 -

            var CommonALDY_RF_COMM = new BPMCommonModel<MediaCommodityConfig>()
            {
                EXT = "ALDY_RF_COMM",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostMediaCommodityFunction(CommonALDY_RF_COMM));
            var mediaOrderReturnRefundAlreadyRefundCommoditysConfig = jsonFunction.JsonToObject<List<MediaOrderReturnRefundAlreadyRefundCommoditysConfig>>(strJson);

            #endregion

            parameter.Add(new SqlParameter("@PERIOD", SqlDbType.Int) { Value = mediaOrderReturnRefundConfig.PERIOD });

            #region - 版權採購退貨折讓單 退貨商品明細 -

            var CommonRF_COMM = new BPMCommonModel<MediaCommodityConfig>()
            {
                EXT = "RF_COMM",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostMediaCommodityFunction(CommonRF_COMM));
            var mediaOrderReturnRefundRefundCommoditysConfig = jsonFunction.JsonToObject<List<MediaOrderReturnRefundRefundCommoditysConfig>>(strJson);

            #endregion

            #region - 版權採購退貨折讓單 憑證退款明細 -

            var CommonINV = new BPMCommonModel<MediaInvoiceInvoicesConfig>()
            {
                EXT = "INV",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceFunction(CommonINV));
            var mediaOrderReturnRefundInvoicesConfig = jsonFunction.JsonToObject<List<MediaOrderReturnRefundInvoicesConfig>>(strJson);

            #endregion

            #region - 版權採購退貨折讓單 憑證已退款細項 -

            var CommonALDY_INV_DTL = new BPMCommonModel<MediaInvoiceInvoiceDetailsConfig>()
            {
                EXT = "ALDY_INV_DTL",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceDetailFunction(CommonALDY_INV_DTL));
            var mediaOrderReturnRefundAlreadyInvoiceDetailsConfig = jsonFunction.JsonToObject<List<MediaOrderReturnRefundAlreadyInvoiceDetailsConfig>>(strJson);

            #endregion            

            #region - 版權採購退貨折讓單 憑證退款細項 -

            var CommonINV_DTL = new BPMCommonModel<MediaInvoiceInvoiceDetailsConfig>()
            {
                EXT = "INV_DTL",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceDetailFunction(CommonINV_DTL));
            var mediaOrderReturnRefundInvoiceDetailsConfig = jsonFunction.JsonToObject<List<MediaOrderReturnRefundInvoiceDetailsConfig>>(strJson);

            #endregion            

            #region - 版權採購退貨折讓單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var mediaOrderReturnRefundViewModel = new MediaOrderReturnRefundViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_ORDER_RETURN_REFUND_TITLE = mediaOrderReturnRefundTitle,
                MEDIA_ORDER_RETURN_REFUND_CONFIG = mediaOrderReturnRefundConfig,
                MEDIA_ORDER_RETURN_REFUND_ALDY_RF_COMMS_CONFIG = mediaOrderReturnRefundAlreadyRefundCommoditysConfig,
                MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG = mediaOrderReturnRefundRefundCommoditysConfig,
                MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG = mediaOrderReturnRefundInvoicesConfig,
                MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG = mediaOrderReturnRefundAlreadyInvoiceDetailsConfig,
                MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG = mediaOrderReturnRefundInvoiceDetailsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認BPM表單是否正常起單到系統中 -

            //保留原有資料
            strJson = jsonFunction.ObjectToJSON(mediaOrderReturnRefundViewModel);

            var BpmSystemOrder = new BPMSystemOrder()
            {
                REQUISITION_ID = query.REQUISITION_ID,
                IDENTIFY = IDENTIFY,
                EXTS = new List<string>()
                {
                    "M",
                    "RF_COMM",
                    "ALDY_RF_COMM",
                    "INV",
                    "ALDY_INV_DTL",
                    "INV_DTL"
                },
            };
            if (mediaOrderReturnRefundViewModel.ASSOCIATED_FORM_CONFIG != null && mediaOrderReturnRefundViewModel.ASSOCIATED_FORM_CONFIG.Count > 0) BpmSystemOrder.IS_ASSOCIATED_FORM = true;
            else BpmSystemOrder.IS_ASSOCIATED_FORM = false;
            //確認是否有正常到系統起單；清除失敗表單資料並重新送單值行
            if (commonRepository.PostBPMSystemOrder(BpmSystemOrder)) PutMediaOrderReturnRefundSingle(jsonFunction.JsonToObject<MediaOrderReturnRefundViewModel>(strJson));

            #endregion

            #region - 確認M表BPM表單單號 -

            //避免儲存後送出表單BPM表單單號沒寫入的情形
            var formQuery = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            if (mediaOrderReturnRefundViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                notifyRepository.ByInsertBPMFormNo(formQuery);

                if (String.IsNullOrEmpty(mediaOrderReturnRefundViewModel.MEDIA_ORDER_RETURN_REFUND_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(mediaOrderReturnRefundViewModel.MEDIA_ORDER_RETURN_REFUND_TITLE.BPM_FORM_NO))
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                    var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                    if (dtBpmFormNo.Rows.Count > 0) mediaOrderReturnRefundViewModel.MEDIA_ORDER_RETURN_REFUND_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                }
            }

            #endregion

            return mediaOrderReturnRefundViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購退貨折讓單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaOrderReturnRefundRefill(MediaOrderReturnRefundQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權採購退貨折讓單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權採購退貨折讓單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaOrderReturnRefundSingle(MediaOrderReturnRefundViewModel model)
        {
            bool vResult = false;
            try
            {
                var mediaInvoiceformQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID
                };
                var mediaInvoiceformData = formRepository.PostFormData(mediaInvoiceformQueryModel);

                #region - 【版權採購請款單】資訊 -

                var mediaInvoiceQueryModel = new MediaInvoiceQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID
                };

                var strmediaInvoiceQuery = mediaInvoiceRepository.PostMediaInvoiceSingle(mediaInvoiceQueryModel);

                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID = mediaInvoiceformData.REQUISITION_ID;
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_BPM_FORM_NO = strmediaInvoiceQuery.MEDIA_INVOICE_TITLE.BPM_FORM_NO;
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_ERP_FORM_NO = strmediaInvoiceQuery.MEDIA_INVOICE_TITLE.FORM_NO;
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_SUBJECT = mediaInvoiceformData.FORM_SUBJECT;
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_ORDER_PYMT_ORDER_TOTAL = strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_PYMT_ORDER_TOTAL;
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV = strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV;
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_PATH = GlobalParameters.FormContentPath(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID, mediaInvoiceformData.IDENTIFY, mediaInvoiceformData.DIAGRAM_NAME);

                #endregion

                #region - 宣告 -

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                FM7Subject = model.MEDIA_ORDER_RETURN_REFUND_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    FM7Subject = "【退貨折讓】第" + model.MEDIA_ORDER_RETURN_REFUND_CONFIG.PERIOD + "期-" + strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_SUBJECT;

                    //var parameter = new List<SqlParameter>()
                    //{
                    //    new SqlParameter("@MEDIA_INVOICE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID }
                    //};

                    //strSQL = "";
                    //strSQL += "SELECT ";
                    //strSQL += "      R.[RequisitionID] ";
                    //strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] AS M ";
                    //strSQL += "LEFT JOIN [BPMPro].[dbo].[FSe7en_Sys_Requisition] AS R ON R.RequisitionID=M.RequisitionID ";
                    //strSQL += "WHERE [MediaInvoiceRequisitionID]=@MEDIA_INVOICE_REQUISITION_ID ";

                    //var frequency = int.Parse((dbFun.DoQuery(strSQL, parameter).Rows.Count).ToString()) + 1;

                    //FM7Subject = "【退貨折讓_第" + frequency + "次】第" + model.MEDIA_ORDER_RETURN_REFUND_CONFIG.PERIOD + "期-" + strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_SUBJECT;

                }

                #endregion

                #endregion

                #region - 版權採購退貨折讓單 表頭資訊：MediaOrderReturnRefund_M -

                var parameterTitle = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  strREQ},
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value =  model.APPLICANT_INFO.PRIORITY},
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value =  model.APPLICANT_INFO.DRAFT_FLAG},
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value =  model.APPLICANT_INFO.FLOW_ACTIVATED},
                    //(申請人/起案人)資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //版權採購退貨折讓單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_RETURN_REFUND_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_RETURN_REFUND_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "      [RequisitionID] ";
                    strSQL += "FROM [BPMPro].[dbo].[FSe7en_Sys_Requisition] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    var dtReq = dbFun.DoQuery(strSQL, parameterTitle);
                    if (dtReq.Rows.Count <= 0)
                    {
                        parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });
                        IsADD = true;
                    }

                }
                else parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";
 
                    if (IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

                    strSQL += "     [FillerID]=@FILLER_ID, ";
                    strSQL += "     [FillerName]=@FILLER_NAME, ";
                    strSQL += "     [Priority]=@PRIORITY, ";
                    strSQL += "     [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "     [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "     [FlowName]=@FLOW_NAME, ";
                    strSQL += "     [FormNo]=@FORM_NO, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion             

                #region - 版權採購退貨折讓單 表單內容：MediaOrderReturnRefund_M -

                if (model.MEDIA_ORDER_RETURN_REFUND_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權採購退貨折讓單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@MEDIA_INVOICE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_PYMT_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.PERIOD },
                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_EXCL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_EXCL_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_NET_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_GROSS_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RF_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROCESS_METHOD", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_ORDER_RETURN_REFUND_CONFIG);

                    #region - 確認小數點後第二位 -

                    GlobalParameters.IsDouble(strJson);

                    #endregion

                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
                    strSQL += "SET [MediaInvoiceRequisitionID]=@MEDIA_INVOICE_REQUISITION_ID, ";
                    strSQL += "     [MediaInvoiceSubject]=@MEDIA_INVOICE_SUBJECT, ";
                    strSQL += "     [MediaInvoiceBPMFormNo]=@MEDIA_INVOICE_BPM_FORM_NO, ";
                    strSQL += "     [MediaInvoiceERPFormNo]=@MEDIA_INVOICE_ERP_FORM_NO, ";
                    strSQL += "     [MediaInvoicePath]=@MEDIA_INVOICE_PATH, ";
                    strSQL += "     [MediaOrderPYMT_OrderTotal]=@MEDIA_ORDER_PYMT_ORDER_TOTAL, ";
                    strSQL += "     [MediaOrderPYMT_OrderTotal_CONV]=@MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV, ";
                    strSQL += "     [Period]=@PERIOD, ";
                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [RF_ExclTotal]=@RF_EXCL_TOTAL, ";
                    strSQL += "     [RF_ExclTotal_TWD]=@RF_EXCL_TOTAL_TWD, ";
                    strSQL += "     [RF_TaxTotal]=@RF_TAX_TOTAL, ";
                    strSQL += "     [RF_TaxTotal_TWD]=@RF_TAX_TOTAL_TWD, ";
                    strSQL += "     [RF_NetTotal]=@RF_NET_TOTAL, ";
                    strSQL += "     [RF_NetTotal_TWD]=@RF_NET_TOTAL_TWD, ";
                    strSQL += "     [RF_GrossTotal]=@RF_GROSS_TOTAL, ";
                    strSQL += "     [RF_GrossTotal_TWD]=@RF_GROSS_TOTAL_TWD, ";
                    strSQL += "     [RF_AmountTotal]=@RF_AMOUNT_TOTAL, ";
                    strSQL += "     [RF_AmountTotal_TWD]=@RF_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [ProcessMethod]=@PROCESS_METHOD, ";
                    strSQL += "     [FinancNote]=@FINANC_NOTE ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權採購退貨折讓單 退貨商品明細: MediaOrderReturnRefund_RF_COMM -

                var parameterRefundCommoditys = new List<SqlParameter>()
                {
                    //版權採購退貨折讓單 退貨商品明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                };

                if (model.MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG.Count > 0)
                {
                    var CommonRF_COMM = new BPMCommonModel<MediaOrderReturnRefundRefundCommoditysConfig>()
                    {
                        EXT = "RF_COMM",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterRefundCommoditys,
                        MODEL = model.MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG
                    };
                    commonRepository.PutMediaCommodityFunction(CommonRF_COMM);
                }

                #endregion

                #region - 版權採購退貨折讓單 憑證退款明細：MediaOrderReturnRefund_INV -

                var parameterInvoices = new List<SqlParameter>()
                {
                    //版權採購退貨折讓單 憑證
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_ERP_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.PERIOD },
                    new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EXCL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EXCL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG.Count > 0)
                {
                    var CommonINV = new BPMCommonModel<MediaOrderReturnRefundInvoicesConfig>()
                    {
                        EXT = "INV",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterInvoices,
                        MODEL = model.MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG
                    };
                    commonRepository.PutInvoiceFunction(CommonINV);
                }

                #endregion

                #region - 版權採購退貨折讓單 憑證退款細項：MediaOrderReturnRefund_INV_DTL -

                var parameterInvoiceDetails = new List<SqlParameter>()
                {
                    //版權採購退貨折讓單 憑證明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_ERP_FORM_NO ?? DBNull.Value },                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG.Count > 0)
                {

                    #region - 計算剩餘數量及金額 -

                    if (model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG.Count > 0)
                    {
                        model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG.ForEach(INV_DTL =>
                        {
                            if (model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG.Any(ALDY_INV_DTL => ALDY_INV_DTL.ROW_NO == INV_DTL.ROW_NO))
                            {
                                INV_DTL.R_QUANTITY = model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG.Where(ALDY_INV_DTL => ALDY_INV_DTL.ROW_NO == INV_DTL.ROW_NO).FirstOrDefault().R_QUANTITY - INV_DTL.QUANTITY;

                                if (model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG.Any(ALDY_INV_DTL => ALDY_INV_DTL.R_QUANTITY == INV_DTL.QUANTITY))
                                {
                                    INV_DTL.R_AMOUNT = 0;
                                    INV_DTL.R_AMOUNT_TWD = 0;

                                }
                                else
                                {
                                    INV_DTL.R_AMOUNT = model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG.Where(ALDY_INV_DTL => ALDY_INV_DTL.ROW_NO == INV_DTL.ROW_NO).FirstOrDefault().R_AMOUNT - INV_DTL.AMOUNT;
                                    INV_DTL.R_AMOUNT_TWD = model.MEDIA_ORDER_RETURN_REFUND_ALDY_INV_DTLS_CONFIG.Where(ALDY_INV_DTL => ALDY_INV_DTL.ROW_NO == INV_DTL.ROW_NO).FirstOrDefault().R_AMOUNT_TWD - INV_DTL.AMOUNT_TWD;
                                }

                            }
                        });
                    }

                    #endregion

                    var CommonINV_DTL = new BPMCommonModel<MediaOrderReturnRefundInvoiceDetailsConfig>()
                    {
                        EXT = "INV_DTL",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterInvoiceDetails,
                        MODEL = model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG
                    };
                    commonRepository.PutInvoiceDetailFunction(CommonINV_DTL);
                }


                #endregion

                #region - 版權採購退貨折讓單 表單關聯：AssociatedForm -
                //關聯表:匯入【版權採購請款單】的「關聯表單」

                var importAssociatedForm = commonRepository.PostAssociatedForm(mediaInvoiceformQueryModel);

                var associatedFormConfig = model.ASSOCIATED_FORM_CONFIG;
                if (associatedFormConfig == null || associatedFormConfig.Count <= 0)
                {
                    associatedFormConfig = importAssociatedForm;
                }

                if (!String.IsNullOrEmpty(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID))
                {
                    #region 關聯:【版權採購請款單】

                    if (!associatedFormConfig.Where(AF => AF.ASSOCIATED_REQUISITION_ID.Contains(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID)).Any())
                    {
                        var MediaInvoiceformQueryModel = new FormQueryModel()
                        {
                            REQUISITION_ID = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID
                        };
                        var MediaInvoiceformData = formRepository.PostFormData(MediaInvoiceformQueryModel);

                        associatedFormConfig.Add(new AssociatedFormConfig()
                        {
                            IDENTIFY = MediaInvoiceformData.IDENTIFY,
                            ASSOCIATED_REQUISITION_ID = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID,
                            BPM_FORM_NO = MediaInvoiceformData.SERIAL_ID,
                            FM7_SUBJECT = MediaInvoiceformData.FORM_SUBJECT,
                            APPLICANT_DEPT_NAME = MediaInvoiceformData.APPLICANT_DEPT_NAME,
                            APPLICANT_NAME = MediaInvoiceformData.APPLICANT_NAME,
                            APPLICANT_DATE_TIME = MediaInvoiceformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                            FORM_PATH = GlobalParameters.FormContentPath(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID, MediaInvoiceformData.IDENTIFY, MediaInvoiceformData.DIAGRAM_NAME),
                            STATE = BPMStatusCode.CLOSE
                        });
                    }

                    #endregion
                }

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = strREQ,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfig
                };

                //寫入「關聯表單」
                commonRepository.PutAssociatedForm(associatedFormModel);

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = strREQ;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = strREQ;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region - 送出表單：FormAutoStart -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = strREQ;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = strREQ;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = strREQ,
                    IDENTIFY = IDENTIFY,
                    DRAFT_FLAG = 0
                };
                commonRepository.PostBPMFormFunction(BPM_FormFunction);

                #endregion

                vResult = true;

            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("版權採購退貨折讓單 (新增/修改/草稿)失敗，原因：" + ex.Message);
            }
            return vResult;

        }

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        /// <summary>
        /// 確認是否為新建的表單
        /// </summary>
        private bool IsADD = false;

        /// <summary>
        /// 表單代號
        /// </summary>
        private string IDENTIFY = "MediaOrderReturnRefund";

        /// <summary>
        /// 表單主旨
        /// </summary>
        private string FM7Subject;

        /// <summary>
        /// Json字串
        /// </summary>
        private string strJson;

        /// <summary>
        /// 系統編號
        /// </summary>
        private string strREQ;

        #endregion
    }
}