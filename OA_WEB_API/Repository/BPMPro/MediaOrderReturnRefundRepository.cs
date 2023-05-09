﻿using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購退貨折讓單
    /// </summary>
    public class MediaOrderReturnRefundRepository
    {
        //#region - 宣告 -

        //dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        //#region Repository

        //FormRepository formRepository = new FormRepository();
        //CommonRepository commonRepository = new CommonRepository();
        //FlowRepository flowRepository = new FlowRepository();

        //#endregion

        //#endregion

        //#region - 方法 -

        ///// <summary>
        ///// 版權採購退貨折讓單(查詢)
        ///// </summary>
        //public MediaOrderReturnRefundViewModel PostMediaOrderReturnRefundSingle(MediaOrderReturnRefundQueryModel query)
        //{
        //    var parameter = new List<SqlParameter>()
        //    {
        //         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
        //    };

        //    #region - 申請人資訊 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
        //    strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
        //    strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
        //    strSQL += "     [ApplicantDept] AS [APPLICANT_DEPT], ";
        //    strSQL += "     [ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
        //    strSQL += "     [ApplicantID] AS [APPLICANT_ID], ";
        //    strSQL += "     [ApplicantName] AS [APPLICANT_NAME], ";
        //    strSQL += "     [ApplicantPhone] AS [APPLICANT_PHONE], ";
        //    strSQL += "     [ApplicantDateTime] AS [APPLICANT_DATETIME], ";
        //    strSQL += "     [FillerID] AS [FILLER_ID], ";
        //    strSQL += "     [FillerName] AS [FILLER_NAME], ";
        //    strSQL += "     [Priority] AS [PRIORITY], ";
        //    strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
        //    strSQL += "     [FlowActivated] AS [FLOW_ACTIVATED] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

        //    #endregion

        //    #region - 版權採購退貨折讓單 表頭資訊 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [FlowName] AS [FLOW_NAME], ";
        //    strSQL += "     [FormNo] AS [FORM_NO], ";
        //    strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
        //    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var mediaOrderReturnRefundTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundTitle>().FirstOrDefault();

        //    #endregion

        //    #region - 版權採購退貨折讓單 表單內容 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [MediaInvoiceRequisitionID] AS [MEDIA_INVOICE_REQUISITION_ID], ";
        //    strSQL += "     [MediaInvoiceSubject] AS [MEDIA_INVOICE_SUBJECT], ";
        //    strSQL += "     [MediaInvoiceBPMFormNo] AS [MEDIA_INVOICE_BPM_FORM_NO], ";
        //    strSQL += "     [MediaInvoiceERPFormNo] AS [MEDIA_INVOICE_ERP_FORM_NO], ";
        //    strSQL += "     [MediaInvoicePath] AS [MEDIA_INVOICE_PATH], ";
        //    strSQL += "     [MediaOrderDTL_OrderTotal] AS [MEDIA_ORDER_DTL_ORDER_TOTAL], ";
        //    strSQL += "     [MediaOrderDTL_OrderTotal_TWD] AS [MEDIA_ORDER_DTL_ORDER_TOTAL_TWD], ";
        //    strSQL += "     [TXN_Type] AS [TXN_TYPE], ";
        //    strSQL += "     [Period] AS [PERIOD], ";
        //    strSQL += "     [TaxTate] AS [TAX_TATE], ";
        //    strSQL += "     [RefundAmount] AS [REFUND_AMOUNT], ";
        //    strSQL += "     [RefundAmount_TWD] AS [REFUND_AMOUNT_TWD], ";
        //    strSQL += "     [TaxInclTotal] AS [TAX_INCL_TOTAL], ";
        //    strSQL += "     [TaxInclTotal_TWD] AS [TAX_INCL_TOTAL_TWD], ";
        //    strSQL += "     [Currency] AS [CURRENCY], ";
        //    strSQL += "     [ExchangeRate] AS [EXCH_RATE], ";
        //    strSQL += "     [Note] AS [NOTE], ";
        //    strSQL += "     [SupNo] AS [SUP_NO], ";
        //    strSQL += "     [SupName] AS [SUP_NAME], ";
        //    strSQL += "     [RegisterKind] AS [REG_KIND], ";
        //    strSQL += "     [RegisterNo] AS [REG_NO], ";
        //    strSQL += "     [OwnerName] AS [OWNER_NAME], ";
        //    strSQL += "     [OwnerTEL] AS [OWNER_TEL], ";
        //    strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
        //    strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
        //    strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
        //    strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2], ";
        //    strSQL += "     [DTL_NetTotal] AS [DTL_NET_TOTAL], ";
        //    strSQL += "     [DTL_NetTotal_TWD] AS [DTL_NET_TOTAL_TWD], ";
        //    strSQL += "     [DTL_TaxTotal] AS [DTL_TAX_TOTAL], ";
        //    strSQL += "     [DTL_TaxTotal_TWD] AS [DTL_TAX_TOTAL_TWD], ";
        //    strSQL += "     [DTL_GrossTotal] AS [DTL_GROSS_TOTAL], ";
        //    strSQL += "     [DTL_GrossTotal_TWD] AS [DTL_GROSS_TOTAL_TWD], ";
        //    strSQL += "     [DTL_MaterialTotal] AS [DTL_MATERIAL_TOTAL], ";
        //    strSQL += "     [DTL_MaterialTotal_TWD] AS [DTL_MATERIAL_TOTAL_TWD], ";
        //    strSQL += "     [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
        //    strSQL += "     [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
        //    strSQL += "     [EX_AmountTotal] AS [EX_AMOUNT_TOTAL], ";
        //    strSQL += "     [EX_AmountTotal_TWD] AS [EX_AMOUNT_TOTAL_TWD], ";
        //    strSQL += "     [EX_TaxTotal] AS [EX_TAX_TOTAL], ";
        //    strSQL += "     [EX_TaxTotal_TWD] AS [EX_TAX_TOTAL_TWD], ";
        //    strSQL += "     [PYMT_CurrentTotal] AS [PYMT_CURRENT_TOTAL], ";
        //    strSQL += "     [PYMT_CurrentTotal_TWD] AS [PYMT_CURRENT_TOTAL_TWD], ";
        //    strSQL += "     [PYMT_EX_TaxTotal] AS [PYMT_EX_TAX_TOTAL], ";
        //    strSQL += "     [PYMT_EX_TaxTotal_TWD] AS [PYMT_EX_TAX_TOTAL_TWD], ";
        //    strSQL += "     [INV_AmountTotal] AS [INV_AMOUNT_TOTAL], ";
        //    strSQL += "     [INV_AmountTotal_TWD] AS [INV_AMOUNT_TOTAL_TWD], ";
        //    strSQL += "     [INV_TaxTotal] AS [INV_TAX_TOTAL], ";
        //    strSQL += "     [INV_TaxTotal_TWD] AS [INV_TAX_TOTAL_TWD], ";
        //    strSQL += "     [ProcessMethod] AS [PROCESS_METHOD], ";
        //    strSQL += "     [FinancNote] AS [FINANC_NOTE] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var mediaOrderReturnRefundConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundConfig>().FirstOrDefault();

        //    #endregion

        //    #region - 採購單 系統編號 -

        //    var Invoiceparameter = new List<SqlParameter>()
        //    {
        //         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = mediaOrderReturnRefundConfig.MEDIA_INVOICE_REQUISITION_ID }
        //    };

        //    var formQueryModel = new FormQueryModel
        //    {
        //        REQUISITION_ID = mediaOrderReturnRefundConfig.MEDIA_INVOICE_REQUISITION_ID
        //    };

        //    var formData = formRepository.PostFormData(formQueryModel);

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "      [MediaOrderRequisitionID] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var dtOrder = dbFun.DoQuery(strSQL, parameter);
        //    var OrderRequisitionID = dtOrder.Rows[0][0].ToString();

        //    #endregion

        //    var Orderparameter = new List<SqlParameter>()
        //    {
        //         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = OrderRequisitionID },
        //         new SqlParameter("@PERIOD", SqlDbType.Int) { Value = mediaOrderReturnRefundConfig.PERIOD }
        //    };

        //    #region - 版權採購退貨折讓單 驗收明細 -

        //    //View的「驗收明細」是 版權採購退貨折讓單 的「驗收明細」加上 「採購明細」的所屬專案、金額及備註。

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     DTL.[DTL_RowNo] AS [ROW_NO], ";
        //    strSQL += "     DTL.[DTL_EpisodeTime] AS [EPISODE_TIME], ";
        //    strSQL += "     DTL.[DTL_MediaSpec] AS [MEDIA_SPEC], ";
        //    strSQL += "     DTL.[DTL_Net] AS [NET], ";
        //    strSQL += "     DTL.[DTL_Net_TWD] AS [NET_TWD], ";
        //    strSQL += "     DTL.[DTL_Tax] AS [TAX], ";
        //    strSQL += "     DTL.[DTL_Tax_TWD] AS [TAX_TWD], ";
        //    strSQL += "     DTL.[DTL_Gross] AS [GROSS], ";
        //    strSQL += "     DTL.[DTL_Gross_TWD] AS [GROSS_TWD], ";
        //    strSQL += "     DTL.[DTL_NetSum] AS [NET_SUM], ";
        //    strSQL += "     DTL.[DTL_NetSum_TWD] AS [NET_SUM_TWD], ";
        //    strSQL += "     DTL.[DTL_GrossSum] AS [GROSS_SUM], ";
        //    strSQL += "     DTL.[DTL_GrossSum_TWD] AS [GROSS_SUM_TWD], ";
        //    strSQL += "     DTL.[DTL_Material] AS [MATERIAL], ";
        //    strSQL += "     DTL.[DTL_ItemSum] AS [ITEM_SUM], ";
        //    strSQL += "     DTL.[DTL_ItemSum_TWD] AS [ITEM_SUM_TWD], ";
        //    strSQL += "     DTL.[DTL_ProjectFormNo] AS [PROJECT_FORM_NO], ";
        //    strSQL += "     DTL.[DTL_ProjectName] AS [PROJECT_NAME], ";
        //    strSQL += "     DTL.[DTL_ProjectNickname] AS [PROJECT_NICKNAME], ";
        //    strSQL += "     DTL.[DTL_ProjectUseYear] AS [PROJECT_USE_YEAR], ";
        //    strSQL += "     ACPT.[PA_RowNo] AS [PA_ROW_NO], ";
        //    strSQL += "     ACPT.[Period] AS [PERIOD], ";
        //    strSQL += "     ACPT.[PA_SupProdANo] AS [SUP_PROD_A_NO], ";
        //    strSQL += "     ACPT.[PA_ItemName] AS [ITEM_NAME], ";
        //    strSQL += "     ACPT.[PA_MediaType] AS [MEDIA_TYPE], ";
        //    strSQL += "     ACPT.[PA_StartEpisode] AS [START_EPISODE], ";
        //    strSQL += "     ACPT.[PA_EndEpisode] AS [END_EPISODE], ";
        //    strSQL += "     ACPT.[PA_ACPT_Episode] AS [ACPT_EPISODE], ";
        //    strSQL += "     ACPT.[PA_Note] AS [PA_NOTE] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] AS ACPT ";
        //    strSQL += "	    INNER JOIN [BPMPro].[dbo].[FM7T_MediaOrder_DTL] AS DTL ON ACPT.[RequisitionID]=DTL.[RequisitionID] AND ACPT.[PA_SupProdANo]=DTL.[DTL_SupProdANo] AND ACPT.[PA_RowNo]=DTL.[DTL_RowNo] ";
        //    strSQL += "WHERE 1=1 ";
        //    strSQL += "         AND ACPT.[RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "         AND ACPT.[Period]=@PERIOD ";

        //    var mediaOrderReturnRefundAcceptancesConfig = dbFun.DoQuery(strSQL, Orderparameter).ToList<MediaOrderReturnRefundAcceptancesConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 授權權利 -

        //    //View的「授權權利」是 版權採購申請單 的「授權權利」

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     AUTH.[RequisitionID] AS [REQUISITION_ID], ";
        //    strSQL += "     [AUTH_RowNo] AS [ROW_NO], ";
        //    strSQL += "     [AUTH_SupProdANo] AS [SUP_PROD_A_NO], ";
        //    strSQL += "     [AUTH_ItemName] AS [ITEM_NAME], ";
        //    strSQL += "     [AUTH_Continent] AS [CONTINENT], ";
        //    strSQL += "     [AUTH_Country] AS [COUNTRY], ";
        //    strSQL += "     [AUTH_PlayPlatform] AS [PLAY_PLATFORM],";
        //    strSQL += "     [AUTH_Play] AS [PLAY], ";
        //    strSQL += "     [AUTH_Sell] AS [SELL], ";
        //    strSQL += "     [AUTH_EditToPlay] AS [EDIT_TO_PLAY], ";
        //    strSQL += "     [AUTH_EditToSell] AS [EDIT_TO_SELL], ";
        //    strSQL += "     [AUTH_AllotedTimeType] AS [ALLOTED_TIME_TYPE], ";
        //    strSQL += "     [AUTH_StartDate] AS [START_DATE], ";
        //    strSQL += "     [AUTH_EndDate] AS [END_DATE], ";
        //    strSQL += "     [AUTH_FrequencyType] AS [FREQUENCY_TYPE], ";
        //    strSQL += "     [AUTH_PlayFrequency] AS [PLAY_FREQUENCY], ";
        //    strSQL += "     [AUTH_Note] AS [NOTE] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_AUTH] AS AUTH ";
        //    strSQL += "     INNER JOIN [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] AS ACPT ON AUTH.[RequisitionID]=ACPT.[RequisitionID] AND AUTH.[AUTH_RowNo]=ACPT.[PA_RowNo] ";
        //    strSQL += "WHERE 1=1 ";
        //    strSQL += "         AND AUTH.[RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "         AND ACPT.[Period]=@PERIOD ";
        //    strSQL += "ORDER BY AUTH.[AutoCounter] ";

        //    var mediaOrderReturnRefundAuthorizesConfig = dbFun.DoQuery(strSQL, Orderparameter).ToList<MediaOrderReturnRefundAuthorizesConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 額外項目 -

        //    //View的「額外項目」是 版權採購申請單 的「額外項目」

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
        //    strSQL += "     [EX_RowNo] AS [ROW_NO], ";
        //    strSQL += "     [EX_Name] AS [NAME], ";
        //    strSQL += "     [EX_Amount] AS [AMOUNT], ";
        //    strSQL += "     [EX_Amount_TWD] AS [AMOUNT_TWD], ";
        //    strSQL += "     [EX_Tax] AS [TAX], ";
        //    strSQL += "     [EX_Tax_TWD] AS [TAX_TWD], ";
        //    strSQL += "     [Period] AS [PERIOD], ";
        //    strSQL += "     [EX_ProjectFormNo] AS [PROJECT_FORM_NO], ";
        //    strSQL += "     [EX_ProjectName] AS [PROJECT_NAME], ";
        //    strSQL += "     [EX_ProjectNickname] AS [PROJECT_NICKNAME], ";
        //    strSQL += "     [EX_ProjectUseYear] AS [PROJECT_USE_YEAR], ";
        //    strSQL += "     [EX_Note] AS [NOTE] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_EX] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "ORDER BY [AutoCounter] ";

        //    var mediaOrderReturnRefundExtrasConfig = dbFun.DoQuery(strSQL, Orderparameter).ToList<MediaOrderReturnRefundExtrasConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 付款辦法 -

        //    //View的「付款辦法」是 版權採購退貨折讓單 的「付款辦法」

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
        //    strSQL += "     [PYMT_Project] AS [PYMT_PROJECT], ";
        //    strSQL += "     [PYMT_Terms] AS [PYMT_TERMS], ";
        //    strSQL += "     [PYMT_MethodID] AS [PYMT_METHOD_ID], ";
        //    strSQL += "     [PYMT_Tax] AS [TAX], ";
        //    strSQL += "     [PYMT_Net] AS [NET], ";
        //    strSQL += "     [PYMT_Gross] AS [GROSS], ";
        //    strSQL += "     [PYMT_Material] AS [MATERIAL], ";
        //    strSQL += "     [PYMT_EX_Amount] AS [EX_AMOUNT], ";
        //    strSQL += "     [PYMT_EX_Tax] AS [EX_TAX], ";
        //    strSQL += "     [PYMT_OrderSum] AS [ORDER_SUM], ";
        //    strSQL += "     [PYMT_OrderSum_CONV] AS [ORDER_SUM_CONV], ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_PYMT] ";
        //    strSQL += "WHERE 1=1 ";
        //    strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "         AND [Period]=@PERIOD ";
        //    strSQL += "ORDER BY [AutoCounter] ";

        //    var mediaOrderReturnRefundPaymentsConfig = dbFun.DoQuery(strSQL, Orderparameter).ToList<MediaOrderReturnRefundPaymentsConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 使用預算 -

        //    //View的「使用預算」是 版權採購退貨折讓單 的「使用預算」

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
        //    strSQL += "     [Period] AS [PERIOD], ";
        //    strSQL += "     [BUDG_FormNo] AS [BUDG_FORM_NO], ";
        //    strSQL += "     [BUDG_CreateYear] AS [BUDG_CREATE_YEAR], ";
        //    strSQL += "     [BUDG_Name] AS [BUDG_NAME], ";
        //    strSQL += "     [BUDG_OwnerDept] AS [OWNER_DEPT], ";
        //    strSQL += "     [BUDG_Total] AS [BUDG_TOTAL], ";
        //    strSQL += "     [BUDG_AvailableBudgetAmount] AS [BUDG_AVAILABLE_BUDGET_AMOUNT], ";
        //    strSQL += "     [BUDG_UseBudgetAmount] AS [BUDG_USE_BUDGET_AMOUNT] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_BUDG] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "ORDER BY [AutoCounter] ";

        //    var mediaOrderReturnRefundBudgetsConfig = dbFun.DoQuery(strSQL, Orderparameter).ToList<MediaOrderReturnRefundBudgetsConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 憑證明細 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [Period] AS [PERIOD], ";
        //    strSQL += "     [RowNo] AS [ROW_NO], ";
        //    strSQL += "     [INV_Num] AS [INV_NUM], ";
        //    strSQL += "     [INV_Date] AS [INV_DATE], ";
        //    strSQL += "     [Amount] AS [AMOUNT], ";
        //    strSQL += "     [Amount_TWD] AS [AMOUNT_TWD], ";
        //    strSQL += "     [Net] AS [NET], ";
        //    strSQL += "     [Net_TWD] AS [NET_TWD], ";
        //    strSQL += "     [Gross] AS [GROSS], ";
        //    strSQL += "     [Gross_TWD] AS [GROSS_TWD], ";
        //    strSQL += "     [Tax] AS [TAX], ";
        //    strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
        //    strSQL += "     [Note] AS [NOTE], ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_INV] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "ORDER BY [AutoCounter] ";

        //    var mediaOrderReturnRefundInvoicesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundInvoicesConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 退貨商品明細 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [RowNo] AS [ROW_NO], ";
        //    strSQL += "     [INV_Num] AS [INV_NUM], ";
        //    strSQL += "     [ItemName] AS [ITEM_NAME], ";
        //    strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
        //    strSQL += "     [StartEpisode] AS [START_EPISODE], ";
        //    strSQL += "     [EndEpisode] AS [END_EPISODE], ";
        //    strSQL += "     [OrderEpisode] AS [ORDER_EPISODE], ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_COMM] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
        //    strSQL += "ORDER BY [AutoCounter] ";

        //    var mediaOrderReturnRefundCommoditysConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderReturnRefundCommoditysConfig>();

        //    #endregion

        //    #region - 版權採購退貨折讓單 表單關聯 -

        //    formQueryModel = new FormQueryModel()
        //    {
        //        REQUISITION_ID = query.REQUISITION_ID
        //    };
        //    var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

        //    #endregion

        //    var mediaOrderReturnRefundViewModel = new MediaOrderReturnRefundViewModel()
        //    {
        //        APPLICANT_INFO = applicantInfo,
        //        MEDIA_ORDER_RETURN_REFUND_TITLE = mediaOrderReturnRefundTitle,
        //        MEDIA_ORDER_RETURN_REFUND_CONFIG = mediaOrderReturnRefundConfig,
        //        MEDIA_ORDER_RETURN_REFUND_ACPTS_CONFIG = mediaOrderReturnRefundAcceptancesConfig,
        //        MEDIA_ORDER_RETURN_REFUND_AUTHS_CONFIG = mediaOrderReturnRefundAuthorizesConfig,
        //        MEDIA_ORDER_RETURN_REFUND_EXS_CONFIG = mediaOrderReturnRefundExtrasConfig,
        //        MEDIA_ORDER_RETURN_REFUND_PYMTS_CONFIG = mediaOrderReturnRefundPaymentsConfig,
        //        MEDIA_ORDER_RETURN_REFUND_BUDGS_CONFIG = mediaOrderReturnRefundBudgetsConfig,
        //        MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG = mediaOrderReturnRefundInvoicesConfig,
        //        MEDIA_ORDER_RETURN_REFUND_COMMS_CONFIG = mediaOrderReturnRefundCommoditysConfig
        //    };

        //    return mediaOrderReturnRefundViewModel;
        //}

        //#region - 依此單內容重送 -

        /////// <summary>
        /////// 版權採購退貨折讓單(依此單內容重送)(僅外部起單使用)
        /////// </summary>        
        ////public bool PutMediaOrderReturnRefundRefill(MediaOrderReturnRefundQueryModel query)
        ////{
        ////    bool vResult = false;
        ////    try
        ////    {



        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        vResult = false;
        ////        CommLib.Logger.Error("版權採購退貨折讓單(依此單內容重送)失敗，原因" + ex.Message);
        ////    }
        ////    return vResult;
        ////}

        //#endregion


        ///// <summary>
        ///// 版權採購退貨折讓單(新增/修改/草稿)
        ///// </summary>
        //public bool PutMediaOrderReturnRefundSingle(MediaOrderReturnRefundViewModel model)
        //{
        //    bool vResult = false;
        //    try
        //    {
        //        var InvoiceformQueryModel = new FormQueryModel()
        //        {
        //            REQUISITION_ID = model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID
        //        };
        //        var InvoiceformData = formRepository.PostFormData(InvoiceformQueryModel);

        //        #region - 宣告 -

        //        #region - 主旨 -

        //        FM7Subject = model.MEDIA_ORDER_RETURN_REFUND_TITLE.FM7_SUBJECT;

        //        if (FM7Subject == null)
        //        {
        //            FM7Subject = "【退貨折讓】第" + model.MEDIA_ORDER_RETURN_REFUND_CONFIG.PERIOD + "期-" + InvoiceformData.FORM_SUBJECT;
        //        }

        //        #endregion

        //        #endregion

        //        #region - 版權採購退貨折讓單 表頭資訊：MediaOrderReturnRefund_M -

        //        var parameterTitle = new List<SqlParameter>()
        //        {
        //            //表單資訊
        //            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  model.APPLICANT_INFO.REQUISITION_ID},
        //            new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
        //            new SqlParameter("@PRIORITY", SqlDbType.Int) { Value =  model.APPLICANT_INFO.PRIORITY},
        //            new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value =  model.APPLICANT_INFO.DRAFT_FLAG},
        //            new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value =  model.APPLICANT_INFO.FLOW_ACTIVATED},
        //            //(申請人/起案人)資訊
        //            new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
        //            new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
        //            new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
        //            new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
        //            new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
        //            new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
        //            //(填單人/代填單人)資訊
        //            new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
        //            new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
        //            //版權採購退貨折讓單 表頭
        //            new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_RETURN_REFUND_TITLE.FLOW_NAME ?? DBNull.Value },
        //            new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_RETURN_REFUND_TITLE.FORM_NO ?? DBNull.Value },
        //            new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
        //        };

        //        strSQL = "";
        //        strSQL += "SELECT ";
        //        strSQL += "      [RequisitionID] ";
        //        strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
        //        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //        var dtA = dbFun.DoQuery(strSQL, parameterTitle);

        //        if (dtA.Rows.Count > 0)
        //        {
        //            #region - 修改 -

        //            strSQL = "";
        //            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
        //            strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
        //            strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
        //            strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
        //            strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
        //            strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
        //            strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";
        //            strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";
        //            strSQL += "     [FillerID]=@FILLER_ID, ";
        //            strSQL += "     [FillerName]=@FILLER_NAME, ";
        //            strSQL += "     [Priority]=@PRIORITY, ";
        //            strSQL += "     [DraftFlag]=@DRAFT_FLAG, ";
        //            strSQL += "     [FlowActivated]=@FLOW_ACTIVATED, ";
        //            strSQL += "     [FlowName]=@FLOW_NAME, ";
        //            strSQL += "     [FormNo]=@FORM_NO, ";
        //            strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
        //            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //            dbFun.DoTran(strSQL, parameterTitle);

        //            #endregion
        //        }
        //        else
        //        {
        //            #region - 新增 -

        //            strSQL = "";
        //            strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
        //            strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

        //            dbFun.DoTran(strSQL, parameterTitle);

        //            #endregion
        //        }

        //        #endregion

        //        #region - 版權採購退貨折讓單 表單內容：MediaOrderReturnRefund_M -

        //        if (model.MEDIA_ORDER_RETURN_REFUND_CONFIG != null)
        //        {
        //            #region - 【版權採購退貨折讓單】資訊 -

        //            model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_BPM_FORM_NO = InvoiceformData.SERIAL_ID;
        //            model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_SUBJECT = InvoiceformData.FORM_SUBJECT;
        //            model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_PATH = GlobalParameters.FormContentPath(model.MEDIA_ORDER_RETURN_REFUND_CONFIG.MEDIA_INVOICE_REQUISITION_ID, InvoiceformData.IDENTIFY, InvoiceformData.DIAGRAM_NAME);

        //            #endregion

        //            var parameterInfo = new List<SqlParameter>()
        //            {
        //                //版權採購請款單 表單內容
        //                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
        //                new SqlParameter("@MEDIA_INVOICE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@MEDIA_INVOICE_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@MEDIA_INVOICE_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@MEDIA_INVOICE_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@MEDIA_INVOICE_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@MEDIA_ORDER_DTL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@MEDIA_ORDER_DTL_ORDER_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@TXN_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@TAX_TATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REFUND_AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REFUND_AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@TAX_INCL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@TAX_INCL_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@EXCH_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@OWNER_TEL", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_NET_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_GROSS_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_MATERIAL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_MATERIAL_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@DTL_ORDER_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@EX_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PYMT_CURRENT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PYMT_CURRENT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PYMT_EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PYMT_EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@INV_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@INV_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@INV_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@INV_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PROCESS_METHOD", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@FINANC_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },                        
        //            };

        //            strJson = jsonFunction.ObjectToJSON(model.MEDIA_ORDER_RETURN_REFUND_CONFIG);

        //            #region - 確認小數點後第二位 -

        //            GlobalParameters.IsDouble(strJson);

        //            #endregion

        //            GlobalParameters.Infoparameter(strJson, parameterInfo);

        //            strSQL = "";
        //            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrderReturnRefund_M] ";
        //            strSQL += "SET []=@, ";
        //            strSQL += "     []=@, ";

        //            strSQL += "     FROM ( ";
        //            strSQL += "             select ";
        //            strSQL += "                 [TXN_Type] AS [TXN_TYPE], ";
        //            strSQL += "                 [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
        //            strSQL += "                 [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
        //            strSQL += "                 [Currency] AS [CURRENCY], ";
        //            strSQL += "                 [PredictRate] AS [PRE_RATE], ";
        //            strSQL += "                 [PricingMethod] AS [PRICING_METHOD], ";
        //            strSQL += "                 [TaxRate] AS [TAX_RATE], ";
        //            strSQL += "                 [SupNo] AS [SUP_NO], ";
        //            strSQL += "                 [SupName] AS [SUP_NAME], ";
        //            strSQL += "                 [RegisterKind] AS [REG_KIND], ";
        //            strSQL += "                 [RegisterNo] AS [REG_NO] ";
        //            strSQL += "             FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
        //            strSQL += "             WHERE [RequisitionID] = @MEDIA_ORDER_REQUISITION_ID ";
        //            strSQL += "     ) AS MAIN ";
        //            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //            dbFun.DoTran(strSQL, parameterInfo);

        //        }

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權採購退貨折讓單 (新增/修改/草稿)失敗，原因：" + ex.Message);
        //    }
        //    return vResult;

        //}

        //#endregion

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