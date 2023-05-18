using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System.Collections;

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

        #endregion

        #region FormRepsitory

        /// <summary>版權採購申請單</summary>
        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();
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

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     [ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     [ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     [ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     [ApplicantPhone] AS [APPLICANT_PHONE], ";
            strSQL += "     [ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     [FillerID] AS [FILLER_ID], ";
            strSQL += "     [FillerName] AS [FILLER_NAME], ";
            strSQL += "     [Priority] AS [PRIORITY], ";
            strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     [FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

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

            #region - 版權採購退貨折讓單 退貨商品明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [INV_Num] AS [INV_NUM], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaSpec] AS [MEDIA_SPEC], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [OrderEpisode] AS [ORDER_EPISODE], ";
            strSQL += "     [ACPT_Episode] AS [ACPT_EPISODE], ";
            strSQL += "     [EpisodeTime] AS [EPISODE_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_RF_COMM] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaOrderReturnRefundRefundCommoditysConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundRefundCommoditysConfig>();

            #endregion

            #region - 版權採購退貨折讓單 憑證退款明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [InvoiceRowNo] AS [INV_ROW_NO], ";
            strSQL += "     [Num] AS [NUM], ";
            strSQL += "     [Date] AS [DATE], ";
            strSQL += "     [Excl] AS [EXCL], ";
            strSQL += "     [Excl_TWD] AS [EXCL_TWD], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Net_TWD] AS [NET_TWD], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [Gross_TWD] AS [GROSS_TWD], ";
            strSQL += "     [Amount] AS [AMOUNT], ";
            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [IsExcl] AS [IS_EXCL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderReturnRefundInvoicesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundInvoicesConfig>();

            #endregion

            #region - 版權採購退貨折讓單 憑證退款細項 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [InvoiceRowNo] AS [INV_ROW_NO], ";
            strSQL += "     [Num] AS [NUM], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [Quantity] AS [QUANTITY], ";
            strSQL += "     [Amount] AS [AMOUNT], ";
            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD], ";
            strSQL += "     [IsExcl] AS [IS_EXCL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV_DTL] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderReturnRefundInvoiceDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundInvoiceDetailsConfig>();

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
                MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG = mediaOrderReturnRefundRefundCommoditysConfig,
                MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG = mediaOrderReturnRefundInvoicesConfig,
                MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG = mediaOrderReturnRefundInvoiceDetailsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

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
                model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_PATH = GlobalParameters.FormContentPath(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID, mediaInvoiceformData.IDENTIFY, mediaInvoiceformData.DIAGRAM_NAME);
                
                #endregion

                #region - 宣告 -

                #region - 主旨 -

                FM7Subject = model.MEDIA_ORDER_RETURN_REFUND_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    FM7Subject = "【退貨折讓】第" + model.MEDIA_ORDER_RETURN_REFUND_CONFIG.PERIOD + "期-" + strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_SUBJECT;
                }

                #endregion

                #endregion

                #region - 版權採購退貨折讓單 表頭資訊：MediaOrderReturnRefund_M -

                var parameterTitle = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  model.APPLICANT_INFO.REQUISITION_ID},
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
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //版權採購退貨折讓單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_RETURN_REFUND_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_RETURN_REFUND_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

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
                    strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";
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
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                        new SqlParameter("@MEDIA_INVOICE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_INVOICE_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
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
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@INV_NUM", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = model.APPLICANT_INFO.REQUISITION_ID },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_RF_COMM] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterRefundCommoditys);

                #endregion

                if (model.MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_RETURN_REFUND_RF_COMMS_CONFIG)
                    {
                        //寫入：版權採購交片單 退貨商品明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterRefundCommoditys);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_RF_COMM]([RequisitionID],[INV_Num],[OrderRowNo],[SupProdANo],[ItemName],[MediaSpec],[MediaType],[StartEpisode],[EndEpisode],[OrderEpisode],[ACPT_Episode],[EpisodeTime]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@INV_NUM,@ORDER_ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_SPEC,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@ORDER_EPISODE,@ACPT_EPISODE,@EPISODE_TIME) ";

                        dbFun.DoTran(strSQL, parameterRefundCommoditys);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購退貨折讓單 憑證退款明細：MediaOrderReturnRefund_INV -

                var parameterInvoices = new List<SqlParameter>()
                {
                    //版權採購退貨折讓單 憑證
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
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

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterInvoices);

                #endregion

                if (model.MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購請款 發票明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterInvoices);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV]([RequisitionID],[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo],[Period],[InvoiceRowNo],[Num],[Date],[Excl],[Excl_TWD],[Tax],[Tax_TWD],[Net],[Net_TWD],[Gross],[Gross_TWD],[Amount],[Amount_TWD],[Note],[IsExcl]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO,@PERIOD,@INV_ROW_NO,@NUM,@DATE,@EXCL,@EXCL_TWD,@TAX,@TAX_TWD,@NET,@NET_TWD,@GROSS,@GROSS_TWD,@AMOUNT,@AMOUNT_TWD,@NOTE,@IS_EXCL) ";

                        dbFun.DoTran(strSQL, parameterInvoices);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購退貨折讓單 憑證退款細項：MediaOrderReturnRefund_INV_DTL -

                var parameterInvoiceDetails = new List<SqlParameter>()
                {
                    //版權採購退貨折讓單 憑證明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)strmediaInvoiceQuery.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_ERP_FORM_NO ?? DBNull.Value },                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterInvoiceDetails);

                #endregion

                if (model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG != null && model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_RETURN_REFUND_INV_DTLS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購請款 憑證細項parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterInvoiceDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV_DTL]([RequisitionID],[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo],[Period],[InvoiceRowNo],[Num],[Name],[Quantity],[Amount],[Amount_TWD],[IsExcl]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO,@PERIOD,@INV_ROW_NO,@NUM,@NAME,@QUANTITY,@AMOUNT,@AMOUNT_TWD,@IS_EXCL) ";

                        dbFun.DoTran(strSQL, parameterInvoiceDetails);
                    }

                    #endregion
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

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfig
                };

                //寫入「關聯表單」
                commonRepository.PutAssociatedForm(associatedFormModel);

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
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
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
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

        #endregion
    }
}