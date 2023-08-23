using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Diagnostics;

using OA_WEB_API.Models.BPMPro;

using Microsoft.SqlServer.Server;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購請款單
    /// </summary>
    public class MediaInvoiceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        FlowRepository flowRepository = new FlowRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #region FormRepository

        /// <summary>版權採購申請單</summary>
        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購請款單(查詢)
        /// </summary>
        public MediaInvoiceViewModel PostMediaInvoiceSingle(MediaInvoiceQueryModel query)
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

            #region - 版權採購請款單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaInvoiceTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceTitle>().FirstOrDefault();

            #endregion

            #region - 版權採購請款單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [MediaOrderRequisitionID] AS [MEDIA_ORDER_REQUISITION_ID], ";
            strSQL += "     [MediaOrderSubject] AS [MEDIA_ORDER_SUBJECT], ";
            strSQL += "     [MediaOrderBPMFormNo] AS [MEDIA_ORDER_BPM_FORM_NO], ";
            strSQL += "     [MediaOrderERPFormNo] AS [MEDIA_ORDER_ERP_FORM_NO], ";
            strSQL += "     [MediaOrderPath] AS [MEDIA_ORDER_PATH], ";
            strSQL += "     [MediaOrderTXN_Type] AS [MEDIA_ORDER_TXN_TYPE], ";
            strSQL += "     [MediaOrderPYMT_OrderTotal] AS [MEDIA_ORDER_PYMT_ORDER_TOTAL], ";
            strSQL += "     [MediaOrderPYMT_OrderTotal_CONV] AS [MEDIA_ORDER_PYMT_ORDER_TOTAL_CONV], ";
            strSQL += "     [MediaAcceptanceRequisitionID] AS [MEDIA_ACCEPTANCE_REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [PredictRate] AS [PRE_RATE], ";
            strSQL += "     [PricingMethod] AS [PRICING_METHOD], ";
            strSQL += "     [TaxRate] AS [TAX_RATE], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [Reimbursement] AS [REIMBURSEMENT], ";
            strSQL += "     [REIMB_StaffDeptID] AS [REIMB_STAFF_DEPT_ID], ";
            strSQL += "     [REIMB_StaffDeptName] AS [REIMB_STAFF_DEPT_NAME], ";
            strSQL += "     [REIMB_StaffID] AS [REIMB_STAFF_ID], ";
            strSQL += "     [REIMB_StaffName] AS [REIMB_STAFF_NAME], ";
            strSQL += "     [SupNo] AS [SUP_NO], ";
            strSQL += "     [SupName] AS [SUP_NAME], ";
            strSQL += "     [PayMethod] AS [PAY_METHOD], ";
            strSQL += "     [RegisterKind] AS [REG_KIND], ";
            strSQL += "     [RegisterNo] AS [REG_NO], ";
            strSQL += "     [SupTXId] AS [SUP_TX_ID], ";
            strSQL += "     [InvoiceType] AS [INVOICE_TYPE], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [DTL_NetTotal] AS [DTL_NET_TOTAL], ";
            strSQL += "     [DTL_NetTotal_TWD] AS [DTL_NET_TOTAL_TWD], ";
            strSQL += "     [DTL_TaxTotal] AS [DTL_TAX_TOTAL], ";
            strSQL += "     [DTL_TaxTotal_TWD] AS [DTL_TAX_TOTAL_TWD], ";
            strSQL += "     [DTL_GrossTotal] AS [DTL_GROSS_TOTAL], ";
            strSQL += "     [DTL_GrossTotal_TWD] AS [DTL_GROSS_TOTAL_TWD], ";
            strSQL += "     [DTL_MaterialTotal] AS [DTL_MATERIAL_TOTAL], ";
            strSQL += "     [DTL_MaterialTotal_TWD] AS [DTL_MATERIAL_TOTAL_TWD], ";
            strSQL += "     [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
            strSQL += "     [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
            strSQL += "     [EX_AmountTotal] AS [EX_AMOUNT_TOTAL], ";
            strSQL += "     [EX_AmountTotal_TWD] AS [EX_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [EX_TaxTotal] AS [EX_TAX_TOTAL], ";
            strSQL += "     [EX_TaxTotal_TWD] AS [EX_TAX_TOTAL_TWD], ";
            strSQL += "     [PYMT_CurrentTotal] AS [PYMT_CURRENT_TOTAL], ";
            strSQL += "     [PYMT_CurrentTotal_TWD] AS [PYMT_CURRENT_TOTAL_TWD], ";
            strSQL += "     [INV_AmountTotal] AS [INV_AMOUNT_TOTAL], ";
            strSQL += "     [INV_AmountTotal_TWD] AS [INV_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [INV_TaxTotal] AS [INV_TAX_TOTAL], ";
            strSQL += "     [INV_TaxTotal_TWD] AS [INV_TAX_TOTAL_TWD], ";
            strSQL += "     [ActualPayAmount] AS [ACTUAL_PAY_AMOUNT], ";
            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2], ";
            strSQL += "     [TX_Category] AS [TX_CATEGORY], ";
            strSQL += "     [BFCY_AccountNo] AS [BFCY_ACCOUNT_NO], ";
            strSQL += "     [BFCY_AccountName] AS [BFCY_ACCOUNT_NAME], ";
            strSQL += "     [BFCY_BankNo] AS [BFCY_BANK_NO], ";
            strSQL += "     [BFCY_BankName] AS [BFCY_BANK_NAME], ";
            strSQL += "     [BFCY_BanKBranchNo] AS [BFCY_BANK_BRANCH_NO], ";
            strSQL += "     [BFCY_BanKBranchName] AS [BFCY_BANK_BRANCH_NAME], ";
            strSQL += "     [BFCY_BankSWIFT] AS [BFCY_BANK_SWIFT], ";
            strSQL += "     [BFCY_BankAddress] AS [BFCY_BANK_ADDRESS], ";
            strSQL += "     [BFCY_BankCountryAndCity] AS [BFCY_BANK_COUNTRY_AND_CITY], ";
            strSQL += "     [BFCY_BankIBAN] AS [BFCY_BANK_IBAN], ";
            strSQL += "     [CurrencyName] AS [CURRENCY_NAME], ";
            strSQL += "     [BFCY_Name] AS [BFCY_NAME], ";
            strSQL += "     [BFCY_TEL] AS [BFCY_TEL], ";
            strSQL += "     [BFCY_Email] AS [BFCY_EMAIL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaInvoiceConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceConfig>().FirstOrDefault();

            #endregion

            var mediaOrderparameter = new List<SqlParameter>()
            {
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = mediaInvoiceConfig.MEDIA_ORDER_REQUISITION_ID },
                new SqlParameter("@PERIOD", SqlDbType.Int) { Value = mediaInvoiceConfig.PERIOD }
            };

            #region - 版權採購單 資訊 -

            var mediaOrderQueryModel = new MediaOrderQueryModel
            {
                REQUISITION_ID = mediaInvoiceConfig.MEDIA_ORDER_REQUISITION_ID
            };

            var mediaOrderContent = mediaOrderRepository.PostMediaOrderSingle(mediaOrderQueryModel);

            #endregion

            #region - 版權採購請款單 驗收明細 -

            //View的「驗收明細」是 版權採購申請單 的「驗收明細」加上 「採購明細」的所屬專案、金額及備註。

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     DTL.[EpisodeTime] AS [EPISODE_TIME], ";
            strSQL += "     DTL.[MediaSpec] AS [MEDIA_SPEC], ";
            strSQL += "     DTL.[Net] AS [NET], ";
            strSQL += "     DTL.[Net_TWD] AS [NET_TWD], ";
            strSQL += "     DTL.[Tax] AS [TAX], ";
            strSQL += "     DTL.[Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     DTL.[Gross] AS [GROSS], ";
            strSQL += "     DTL.[Gross_TWD] AS [GROSS_TWD], ";
            strSQL += "     DTL.[NetSum] AS [NET_SUM], ";
            strSQL += "     DTL.[NetSum_TWD] AS [NET_SUM_TWD], ";
            strSQL += "     DTL.[GrossSum] AS [GROSS_SUM], ";
            strSQL += "     DTL.[GrossSum_TWD] AS [GROSS_SUM_TWD], ";
            strSQL += "     DTL.[Material] AS [MATERIAL], ";
            strSQL += "     DTL.[ItemSum] AS [ITEM_SUM], ";
            strSQL += "     DTL.[ItemSum_TWD] AS [ITEM_SUM_TWD], ";
            strSQL += "     DTL.[ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     DTL.[ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     DTL.[ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     DTL.[ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     DTL.[Note] AS [NOTE], ";
            strSQL += "     DTL.[OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     ACPT.[Period] AS [PERIOD], ";
            strSQL += "     ACPT.[SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     ACPT.[ItemName] AS [ITEM_NAME], ";
            strSQL += "     ACPT.[MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     ACPT.[StartEpisode] AS [START_EPISODE], ";
            strSQL += "     ACPT.[EndEpisode] AS [END_EPISODE], ";
            strSQL += "     ACPT.[ACPT_Episode] AS [ACPT_EPISODE], ";
            strSQL += "     ACPT.[OrderEpisode] AS [ORDER_EPISODE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] AS ACPT ";
            strSQL += "	    INNER JOIN [BPMPro].[dbo].[FM7T_MediaOrder_DTL] AS DTL ON ACPT.[RequisitionID]=DTL.[RequisitionID] AND ACPT.[SupProdANo]=DTL.[SupProdANo] AND ACPT.[OrderRowNo]=DTL.[OrderRowNo] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND ACPT.[RequisitionID]=@REQUISITION_ID ";
            strSQL += "         AND ACPT.[Period]=@PERIOD ";

            var mediaInvoiceAcceptancesConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoiceAcceptancesConfig>();

            #endregion

            #region - 版權採購請款單 授權權利 -
            //View的「授權權利」是 版權採購申請單 的「授權權利」

            #region 授權權利 舊寫法
            //strSQL = "";
            //strSQL += "SELECT ";
            //strSQL += "     AUTH.[RequisitionID] AS [REQUISITION_ID], ";
            //strSQL += "     AUTH.[OrderRowNo] AS [ORDER_ROW_NO], ";
            //strSQL += "     AUTH.[SupProdANo] AS [SUP_PROD_A_NO], ";
            //strSQL += "     AUTH.[ItemName] AS [ITEM_NAME], ";
            //strSQL += "     AUTH.[Continent] AS [CONTINENT], ";
            //strSQL += "     AUTH.[Country] AS [COUNTRY], ";
            //strSQL += "     AUTH.[PlayPlatform] AS [PLAY_PLATFORM],";
            //strSQL += "     AUTH.[Play] AS [PLAY], ";
            //strSQL += "     AUTH.[Sell] AS [SELL], ";
            //strSQL += "     AUTH.[EditToPlay] AS [EDIT_TO_PLAY], ";
            //strSQL += "     AUTH.[EditToSell] AS [EDIT_TO_SELL], ";
            //strSQL += "     AUTH.[AllotedTimeType] AS [ALLOTED_TIME_TYPE], ";
            //strSQL += "     AUTH.[StartDate] AS [START_DATE], ";
            //strSQL += "     AUTH.[EndDate] AS [END_DATE], ";
            //strSQL += "     AUTH.[FrequencyType] AS [FREQUENCY_TYPE], ";
            //strSQL += "     AUTH.[PlayFrequency] AS [PLAY_FREQUENCY], ";
            //strSQL += "     AUTH.[Note] AS [NOTE] ";
            //strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_AUTH] AS AUTH ";
            //strSQL += "     INNER JOIN [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] AS ACPT ON AUTH.[RequisitionID]=ACPT.[RequisitionID] AND AUTH.[OrderRowNo]=ACPT.[OrderRowNo] ";
            //strSQL += "WHERE 1=1 ";
            //strSQL += "         AND AUTH.[RequisitionID]=@REQUISITION_ID ";
            //strSQL += "         AND ACPT.[Period]=@PERIOD ";
            //strSQL += "ORDER BY AUTH.[AutoCounter] ";

            //var mediaInvoiceAuthorizesConfig = dbFun.DoQuery(strSQL, mediaOrderparameter).ToList<MediaInvoiceAuthorizesConfig>();
            #endregion

            List<MediaInvoiceAuthorizesConfig> mediaInvoiceAuthorizesConfig = new List<MediaInvoiceAuthorizesConfig>();
            foreach (var item in mediaInvoiceAcceptancesConfig)
            {
                strJson = jsonFunction.ObjectToJSON(mediaOrderContent.MEDIA_ORDER_AUTHS_CONFIG.Where(AUTH => AUTH.SUP_PROD_A_NO == item.SUP_PROD_A_NO && item.PERIOD == mediaInvoiceConfig.PERIOD).Select(AUTH => AUTH));
                mediaInvoiceAuthorizesConfig.AddRange(JsonConvert.DeserializeObject<List<MediaInvoiceAuthorizesConfig>>(strJson));
            }
            mediaInvoiceAuthorizesConfig = mediaInvoiceAuthorizesConfig.GroupBy(AUTH => new { AUTH.SUP_PROD_A_NO, AUTH.PLAY_PLATFORM }).Select(g => g.First()).ToList();


            #endregion

            #region - 版權採購請款單 額外項目 -
            //View的「額外項目」是 版權採購申請單 的「額外項目」

            strJson = jsonFunction.ObjectToJSON(mediaOrderContent.MEDIA_ORDER_EXS_CONFIG.Where(EX => EX.PERIOD == mediaInvoiceConfig.PERIOD).Select(EX => EX));
            var mediaInvoiceExtrasConfig = JsonConvert.DeserializeObject<List<MediaInvoiceExtrasConfig>>(strJson);

            #endregion

            #region - 版權採購請款單 付款辦法 -
            //View的「付款辦法」是 版權採購申請單 的「付款辦法」

            strJson = jsonFunction.ObjectToJSON(mediaOrderContent.MEDIA_ORDER_PYMTS_CONFIG.Where(PYMT => PYMT.PERIOD == mediaInvoiceConfig.PERIOD).Select(PYMT => PYMT));
            var mediaInvoicePaymentsConfig = JsonConvert.DeserializeObject<List<MediaInvoicePaymentsConfig>>(strJson);


            #endregion

            #region - 版權採購請款單 使用預算 -
            //View的「使用預算」是 版權採購申請單 的「使用預算」

            strJson = jsonFunction.ObjectToJSON(mediaOrderContent.MEDIA_ORDER_BUDGS_CONFIG.Where(BUDG => BUDG.PERIOD == mediaInvoiceConfig.PERIOD).Select(BUDG => BUDG));
            var mediaInvoiceBudgetsConfig = JsonConvert.DeserializeObject<List<MediaInvoiceBudgetsConfig>>(strJson);

            #endregion

            parameter.Add(new SqlParameter("@PERIOD", SqlDbType.Int) { Value = mediaInvoiceConfig.PERIOD });

            #region - 版權採購請款單 憑證明細 -

            var CommonINV = new BPMCommonModel<MediaInvoiceInvoicesConfig>()
            {
                EXT = "INV",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceFunction(CommonINV));
            var mediaInvoiceInvoicesConfig = jsonFunction.JsonToObject<List<MediaInvoiceInvoicesConfig>>(strJson);

            #endregion

            #region - 版權採購請款單 憑證細項 -

            var CommonINV_DTL = new BPMCommonModel<MediaInvoiceInvoiceDetailsConfig>()
            {
                EXT = "INV_DTL",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceDetailFunction(CommonINV_DTL));
            var mediaInvoiceInvoiceDetailsConfig = jsonFunction.JsonToObject<List<MediaInvoiceInvoiceDetailsConfig>>(strJson);

            #endregion

            #region - 版權採購申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var mediaInvoiceViewModel = new MediaInvoiceViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_INVOICE_TITLE = mediaInvoiceTitle,
                MEDIA_INVOICE_CONFIG = mediaInvoiceConfig,
                MEDIA_INVOICE_ACPTS_CONFIG = mediaInvoiceAcceptancesConfig,
                MEDIA_INVOICE_AUTHS_CONFIG = mediaInvoiceAuthorizesConfig,
                MEDIA_INVOICE_EXS_CONFIG = mediaInvoiceExtrasConfig,
                MEDIA_INVOICE_PYMTS_CONFIG = mediaInvoicePaymentsConfig,
                MEDIA_INVOICE_BUDGS_CONFIG = mediaInvoiceBudgetsConfig,
                MEDIA_INVOICE_INVS_CONFIG = mediaInvoiceInvoicesConfig,
                MEDIA_INVOICE_INV_DTLS_CONFIG = mediaInvoiceInvoiceDetailsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認表單 -

            if (mediaInvoiceViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                if (!CommonRepository.GetFSe7enSysRequisition().Any(R => R.REQUISITION_ID == query.REQUISITION_ID))
                {
                    mediaInvoiceViewModel = new MediaInvoiceViewModel();
                    CommLib.Logger.Error("版權採購請款單(查詢)失敗，原因：系統無正常起單。");
                }
                else
                {
                    #region - 確認M表BPM表單單號 -

                    //避免儲存後送出表單BPM表單單號沒寫入的情形
                    var formQuery = new FormQueryModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID
                    };
                    notifyRepository.ByInsertBPMFormNo(formQuery);

                    if (String.IsNullOrEmpty(mediaInvoiceViewModel.MEDIA_INVOICE_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(mediaInvoiceViewModel.MEDIA_INVOICE_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) mediaInvoiceViewModel.MEDIA_INVOICE_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return mediaInvoiceViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購請款單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaAcceptanceRefill(MediaAcceptanceQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權採購請款單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權採購請款單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaInvoiceSingle(MediaInvoiceViewModel model)
        {
            bool vResult = false;
            try
            {
                var medialOrderformQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID
                };
                var medialOrderformData = formRepository.PostFormData(medialOrderformQueryModel);

                #region - 宣告 -

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                FM7Subject = model.MEDIA_INVOICE_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    FM7Subject = "【請款】第" + model.MEDIA_INVOICE_CONFIG.PERIOD + "期-" + medialOrderformData.FORM_SUBJECT;
                }

                #endregion

                #region - 預設設定 -

                if (String.IsNullOrEmpty(model.MEDIA_INVOICE_CONFIG.REIMBURSEMENT) || String.IsNullOrWhiteSpace(model.MEDIA_INVOICE_CONFIG.REIMBURSEMENT))
                {
                    //員工代墊
                    model.MEDIA_INVOICE_CONFIG.REIMBURSEMENT = "false";
                    //支付方式 
                    model.MEDIA_INVOICE_CONFIG.PAY_METHOD = "SUP_A/C";
                }

                #endregion

                #endregion

                #region - 版權採購請款單 表頭資訊：MediaInvoice_M -

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
                    //版權採購請款單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    if (CommonRepository.GetFSe7enSysRequisition().Where(R => R.REQUISITION_ID == strREQ).Count() <= 0)
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaInvoice_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 版權採購請款單 表單內容：MediaInvoice_M -

                if (model.MEDIA_INVOICE_CONFIG != null)
                {
                    #region - 版權採購單 資訊 -

                    var mediaOrderQueryModel = new MediaOrderQueryModel
                    {
                        REQUISITION_ID = medialOrderformData.REQUISITION_ID
                    };

                    var mediaOrderContent = mediaOrderRepository.PostMediaOrderSingle(mediaOrderQueryModel);

                    #endregion

                    #region - 版權採購請款單 驗收明細_金額總計 -

                    strJson = jsonFunction.ObjectToJSON(mediaOrderContent.MEDIA_ORDER_DTLS_CONFIG
                        .Join(mediaOrderContent.MEDIA_ORDER_ACPTS_CONFIG,
                        DTL => DTL.ORDER_ROW_NO,
                        ACPT => ACPT.ORDER_ROW_NO,
                        (DTL, ACPT) => new
                        {
                            DTL.ORDER_ROW_NO,
                            DTL.SUP_PROD_A_NO,
                            DTL.ITEM_NAME,
                            DTL.MEDIA_SPEC,
                            DTL.AUTH_ALL,
                            DTL.MEDIA_TYPE,
                            DTL.START_EPISODE,
                            DTL.END_EPISODE,
                            DTL.ORDER_EPISODE,
                            DTL.EPISODE_TIME,
                            DTL.NET,
                            DTL.NET_TWD,
                            DTL.TAX,
                            DTL.TAX_TWD,
                            DTL.GROSS,
                            DTL.GROSS_TWD,
                            DTL.NET_SUM,
                            DTL.NET_SUM_TWD,
                            DTL.GROSS_SUM,
                            DTL.GROSS_SUM_TWD,
                            DTL.MATERIAL,
                            DTL.ITEM_SUM,
                            DTL.ITEM_SUM_TWD,
                            DTL.PROJECT_FORM_NO,
                            DTL.PROJECT_NAME,
                            DTL.PROJECT_NICKNAME,
                            DTL.PROJECT_USE_YEAR,
                            DTL.NOTE,
                            ACPT
                        })
                        .OrderBy(ACPT_DTL => ACPT_DTL.ORDER_ROW_NO)
                        .Where(ACPT_DTL => ACPT_DTL.ACPT.PERIOD == model.MEDIA_INVOICE_CONFIG.PERIOD));
                    var mediaOrderDetailsConfig = JsonConvert.DeserializeObject<List<MediaOrderDetailsConfig>>(strJson);
                    if (mediaOrderDetailsConfig != null)
                    {
                        mediaOrderDetailsConfig.ForEach(DTL =>
                        {
                            model.MEDIA_INVOICE_CONFIG.DTL_NET_TOTAL += DTL.NET_SUM;
                            model.MEDIA_INVOICE_CONFIG.DTL_NET_TOTAL_TWD += DTL.NET_SUM_TWD;
                            model.MEDIA_INVOICE_CONFIG.DTL_GROSS_TOTAL += DTL.GROSS_SUM;
                            model.MEDIA_INVOICE_CONFIG.DTL_GROSS_TOTAL_TWD += DTL.GROSS_SUM_TWD;
                            model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL += DTL.MATERIAL;
                            model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL_TWD = int.Parse((model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL * model.MEDIA_INVOICE_CONFIG.PRE_RATE).ToString());
                            model.MEDIA_INVOICE_CONFIG.DTL_ORDER_TOTAL += DTL.ITEM_SUM;
                            model.MEDIA_INVOICE_CONFIG.DTL_ORDER_TOTAL_TWD += DTL.ITEM_SUM_TWD;
                            model.MEDIA_INVOICE_CONFIG.DTL_TAX_TOTAL += DTL.TAX;
                            model.MEDIA_INVOICE_CONFIG.DTL_TAX_TOTAL_TWD += DTL.TAX_TWD;
                        });
                    }

                    #endregion

                    #region - 版權採購請款單 額外項目_金額總計 -

                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_INVOICE_EXS_CONFIG);
                    var mediaOrderExtrasConfig = JsonConvert.DeserializeObject<List<MediaOrderExtrasConfig>>(strJson);
                    if (mediaOrderExtrasConfig != null)
                    {
                        mediaOrderExtrasConfig.ForEach(EX =>
                        {
                            model.MEDIA_INVOICE_CONFIG.EX_AMOUNT_TOTAL += EX.AMOUNT;
                            model.MEDIA_INVOICE_CONFIG.EX_AMOUNT_TOTAL_TWD += EX.AMOUNT_TWD;
                            model.MEDIA_INVOICE_CONFIG.EX_TAX_TOTAL += EX.TAX;
                            model.MEDIA_INVOICE_CONFIG.EX_TAX_TOTAL_TWD += EX.TAX_TWD;
                        });
                    }

                    #endregion

                    #region - 【版權採購申請單】資訊 -

                    model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO = medialOrderformData.SERIAL_ID;
                    model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_SUBJECT = medialOrderformData.FORM_SUBJECT;
                    model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_PATH = GlobalParameters.FormContentPath(model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID, medialOrderformData.IDENTIFY, medialOrderformData.DIAGRAM_NAME);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權採購請款單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ACCEPTANCE_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMBURSEMENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_TX_ID", SqlDbType.NVarChar) { Size = 1000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INVOICE_TYPE", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_MATERIAL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_MATERIAL_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_CURRENT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_CURRENT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@INV_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACTUAL_PAY_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_SWIFT", SqlDbType.NVarChar) { Size = 300, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_ADDRESS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_COUNTRY_AND_CITY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_IBAN", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TX_CATEGORY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_EMAIL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    #region - 確認小數點後第二位 -

                    model.MEDIA_INVOICE_CONFIG.DTL_NET_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_NET_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_TAX_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_GROSS_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_GROSS_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_MATERIAL_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.DTL_ORDER_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.DTL_ORDER_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.EX_AMOUNT_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.EX_AMOUNT_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.EX_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.EX_TAX_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.PYMT_CURRENT_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.PYMT_CURRENT_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.INV_AMOUNT_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.INV_AMOUNT_TOTAL, 2);
                    model.MEDIA_INVOICE_CONFIG.INV_TAX_TOTAL = Math.Round(model.MEDIA_INVOICE_CONFIG.INV_TAX_TOTAL, 2);

                    #endregion

                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_INVOICE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                    strSQL += "SET [MediaOrderRequisitionID]=@MEDIA_ORDER_REQUISITION_ID, ";
                    strSQL += "     [MediaOrderSubject]=@MEDIA_ORDER_SUBJECT, ";
                    strSQL += "     [MediaOrderBPMFormNo]=@MEDIA_ORDER_BPM_FORM_NO, ";
                    strSQL += "     [MediaOrderERPFormNo]=@MEDIA_ORDER_ERP_FORM_NO, ";
                    strSQL += "     [MediaOrderPath]=@MEDIA_ORDER_PATH, ";
                    strSQL += "     [MediaOrderTXN_Type]=MAIN.[TXN_TYPE], ";
                    strSQL += "     [MediaOrderPYMT_OrderTotal]=MAIN.[PYMT_ORDER_TOTAL], ";
                    strSQL += "     [MediaOrderPYMT_OrderTotal_CONV]=MAIN.[PYMT_ORDER_TOTAL_COMV], ";
                    strSQL += "     [MediaAcceptanceRequisitionID]=@MEDIA_ACCEPTANCE_REQUISITION_ID, ";
                    strSQL += "     [Currency]=MAIN.[CURRENCY], ";
                    strSQL += "     [PredictRate]=MAIN.[PRE_RATE], ";
                    strSQL += "     [PricingMethod]=MAIN.[PRICING_METHOD], ";
                    strSQL += "     [TaxRate]=MAIN.[TAX_RATE], ";
                    strSQL += "     [Period]=@PERIOD, ";
                    strSQL += "     [Reimbursement]=@REIMBURSEMENT, ";
                    strSQL += "     [REIMB_StaffDeptID]=@REIMB_STAFF_DEPT_ID, ";
                    strSQL += "     [REIMB_StaffDeptName]=@REIMB_STAFF_DEPT_NAME, ";
                    strSQL += "     [REIMB_StaffID]=@REIMB_STAFF_ID, ";
                    strSQL += "     [REIMB_StaffName]=@REIMB_STAFF_NAME, ";
                    strSQL += "     [SupNo]=MAIN.[SUP_NO], ";
                    strSQL += "     [SupName]=MAIN.[SUP_NAME], ";
                    strSQL += "     [PayMethod]=@PAY_METHOD, ";
                    strSQL += "     [RegisterKind]=MAIN.[REG_KIND], ";
                    strSQL += "     [RegisterNo]=MAIN.[REG_NO], ";
                    strSQL += "     [SupTXId]=@SUP_TX_ID, ";
                    strSQL += "     [InvoiceType]=@INVOICE_TYPE, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [DTL_NetTotal]=@DTL_NET_TOTAL, ";
                    strSQL += "     [DTL_NetTotal_TWD]=@DTL_NET_TOTAL_TWD, ";
                    strSQL += "     [DTL_TaxTotal]=@DTL_TAX_TOTAL, ";
                    strSQL += "     [DTL_TaxTotal_TWD]=@DTL_TAX_TOTAL_TWD, ";
                    strSQL += "     [DTL_GrossTotal]=@DTL_GROSS_TOTAL, ";
                    strSQL += "     [DTL_GrossTotal_TWD]=@DTL_GROSS_TOTAL_TWD, ";
                    strSQL += "     [DTL_MaterialTotal]=@DTL_MATERIAL_TOTAL, ";
                    strSQL += "     [DTL_MaterialTotal_TWD]=@DTL_MATERIAL_TOTAL_TWD, ";
                    strSQL += "     [DTL_OrderTotal]=@DTL_ORDER_TOTAL, ";
                    strSQL += "     [DTL_OrderTotal_TWD]=@DTL_ORDER_TOTAL_TWD, ";
                    strSQL += "     [EX_AmountTotal]=@EX_AMOUNT_TOTAL, ";
                    strSQL += "     [EX_AmountTotal_TWD]=@EX_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [EX_TaxTotal]=@EX_TAX_TOTAL, ";
                    strSQL += "     [EX_TaxTotal_TWD]=@EX_TAX_TOTAL_TWD, ";
                    strSQL += "     [PYMT_CurrentTotal]=@PYMT_CURRENT_TOTAL, ";
                    strSQL += "     [PYMT_CurrentTotal_TWD]=@PYMT_CURRENT_TOTAL_TWD, ";
                    strSQL += "     [INV_AmountTotal]=@INV_AMOUNT_TOTAL, ";
                    strSQL += "     [INV_AmountTotal_TWD]=@INV_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [INV_TaxTotal]=@INV_TAX_TOTAL, ";
                    strSQL += "     [INV_TaxTotal_TWD]=@INV_TAX_TOTAL_TWD, ";
                    strSQL += "     [ActualPayAmount]=@ACTUAL_PAY_AMOUNT, ";
                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2, ";
                    strSQL += "     [BFCY_BanKBranchNo]=@BFCY_BANK_BRANCH_NO, ";
                    strSQL += "     [BFCY_BanKBranchName]=@BFCY_BANK_BRANCH_NAME, ";
                    strSQL += "     [BFCY_BankSWIFT]=@BFCY_BANK_SWIFT, ";
                    strSQL += "     [BFCY_BankAddress]=@BFCY_BANK_ADDRESS, ";
                    strSQL += "     [BFCY_BankCountryAndCity]=@BFCY_BANK_COUNTRY_AND_CITY, ";
                    strSQL += "     [BFCY_BankIBAN]=@BFCY_BANK_IBAN, ";
                    strSQL += "     [TX_Category]=@TX_CATEGORY, ";
                    strSQL += "     [BFCY_AccountNo]=@BFCY_ACCOUNT_NO, ";
                    strSQL += "     [BFCY_AccountName]=@BFCY_ACCOUNT_NAME, ";
                    strSQL += "     [BFCY_BankNo]=@BFCY_BANK_NO, ";
                    strSQL += "     [BFCY_BankName]=@BFCY_BANK_NAME, ";
                    strSQL += "     [CurrencyName]=@CURRENCY_NAME, ";
                    strSQL += "     [BFCY_Name]=@BFCY_NAME, ";
                    strSQL += "     [BFCY_TEL]=@BFCY_TEL, ";
                    strSQL += "     [BFCY_Email]=@BFCY_EMAIL ";
                    strSQL += "     FROM ( ";
                    strSQL += "             select ";
                    strSQL += "                 [TXN_Type] AS [TXN_TYPE], ";
                    strSQL += "                 [PYMT_OrderTotal] AS [PYMT_ORDER_TOTAL], ";
                    strSQL += "                 [PYMT_OrderTotal_CONV] AS [PYMT_ORDER_TOTAL_COMV], ";
                    strSQL += "                 [Currency] AS [CURRENCY], ";
                    strSQL += "                 [PredictRate] AS [PRE_RATE], ";
                    strSQL += "                 [PricingMethod] AS [PRICING_METHOD], ";
                    strSQL += "                 [TaxRate] AS [TAX_RATE], ";
                    strSQL += "                 [SupNo] AS [SUP_NO], ";
                    strSQL += "                 [SupName] AS [SUP_NAME], ";
                    strSQL += "                 [RegisterKind] AS [REG_KIND], ";
                    strSQL += "                 [RegisterNo] AS [REG_NO] ";
                    strSQL += "             FROM [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
                    strSQL += "             WHERE [RequisitionID] = @MEDIA_ORDER_REQUISITION_ID ";
                    strSQL += "     ) AS MAIN ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權採購請款單 驗收明細: MediaInvoice_ACPT -

                //View 是執行
                //版權採購的 採購明細(MediaInvoice_DTL)及驗收明細(MediaInvoice_ACPT)

                #endregion

                #region - 版權採購請款單 授權權利: MediaInvoice_AUTH -

                //View 是執行
                //版權採購的 授權權利(MediaInvoice_AUTH)

                #endregion

                #region - 版權採購申請單 額外項目: MediaInvoice_EX -

                //View 是執行
                //版權採購的 額外項目(MediaInvoice_EX)

                #endregion

                #region - 版權採購請款單 付款辦法: MediaInvoice_PYMT -

                //View 是執行
                //版權採購的 付款辦法(MediaInvoice_PYMT)
                //只有在「財務部簽核」會需要更新 會計類別(ACCT_Category) 欄位



                var parameterPayments = new List<SqlParameter>()
                {
                    //版權採購請款 付款辦法 更新:會計類別
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACCT_CATEGORY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                if (model.MEDIA_INVOICE_PYMTS_CONFIG != null && model.MEDIA_INVOICE_PYMTS_CONFIG.Count > 0)
                {
                    #region 修改資料

                    foreach (var item in model.MEDIA_INVOICE_PYMTS_CONFIG)
                    {
                        //寫入：版權採購申請 付款辦法 更新:會計類別parameter

                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterPayments);

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrder_PYMT] ";
                        strSQL += "SET [ACCT_Category]=@ACCT_CATEGORY ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                        strSQL += "         AND [Period]=@PERIOD ";

                        dbFun.DoTran(strSQL, parameterPayments);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購請款單 使用預算: MediaInvoice_BUDG -

                //View 是執行
                //版權採購的 使用預算(MediaInvoice_BUDG) 內容。

                #endregion

                #region - 版權採購請款單 憑證明細：MediaInvoice_INV -

                var parameterInvoices = new List<SqlParameter>()
                {
                    //版權採購請款單 憑證
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_ERP_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
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

                if (model.MEDIA_INVOICE_INVS_CONFIG != null && model.MEDIA_INVOICE_INVS_CONFIG.Count > 0)
                {
                    var CommonINV = new BPMCommonModel<MediaInvoiceInvoicesConfig>()
                    {
                        EXT = "INV",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterInvoices,
                        MODEL = model.MEDIA_INVOICE_INVS_CONFIG
                    };
                    commonRepository.PutInvoiceFunction(CommonINV);
                }

                #endregion

                #region - 版權採購請款單 憑證細項：MediaInvoice_INV_DTL -

                var parameterInvoiceDetails = new List<SqlParameter>()
                {
                    //版權採購請款單 憑證細項
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_ERP_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.MEDIA_INVOICE_INV_DTLS_CONFIG != null && model.MEDIA_INVOICE_INV_DTLS_CONFIG.Count > 0)
                {
                    var CommonINV_DTL = new BPMCommonModel<MediaInvoiceInvoiceDetailsConfig>()
                    {
                        EXT = "INV_DTL",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterInvoiceDetails,
                        MODEL = model.MEDIA_INVOICE_INV_DTLS_CONFIG
                    };
                    commonRepository.PutInvoiceDetailFunction(CommonINV_DTL);

                }

                #endregion

                #region - 版權採購請款單 表單關聯：AssociatedForm -

                //關聯表:匯入【版權採購申請單】的「關聯表單」
                var importAssociatedForm = commonRepository.PostAssociatedForm(medialOrderformQueryModel);

                #region 關聯表:加上【版權採購申請單】

                importAssociatedForm.Add(new AssociatedFormConfig()
                {
                    IDENTIFY = medialOrderformData.IDENTIFY,
                    ASSOCIATED_REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID,
                    BPM_FORM_NO = medialOrderformData.SERIAL_ID,
                    FM7_SUBJECT = medialOrderformData.FORM_SUBJECT,
                    APPLICANT_DEPT_NAME = medialOrderformData.APPLICANT_DEPT_NAME,
                    APPLICANT_NAME = medialOrderformData.APPLICANT_NAME,
                    APPLICANT_DATE_TIME = medialOrderformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                    FORM_PATH = GlobalParameters.FormContentPath(model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID, medialOrderformData.IDENTIFY, medialOrderformData.DIAGRAM_NAME),
                    STATE = BPMStatusCode.CLOSE
                });

                #endregion

                var associatedFormConfig = model.ASSOCIATED_FORM_CONFIG;
                if (associatedFormConfig == null || associatedFormConfig.Count <= 0)
                {
                    associatedFormConfig = importAssociatedForm;
                }

                if (!String.IsNullOrEmpty(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID))
                {
                    #region 關聯表:加上【版權採購點驗收單】

                    if (!associatedFormConfig.Where(AF => AF.ASSOCIATED_REQUISITION_ID.Contains(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID)).Any())
                    {
                        var medialAcceptanceformQueryModel = new FormQueryModel()
                        {
                            REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID
                        };
                        var medialAcceptanceformData = formRepository.PostFormData(medialAcceptanceformQueryModel);

                        associatedFormConfig.Add(new AssociatedFormConfig()
                        {
                            IDENTIFY = medialAcceptanceformData.IDENTIFY,
                            ASSOCIATED_REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID,
                            BPM_FORM_NO = medialAcceptanceformData.SERIAL_ID,
                            FM7_SUBJECT = medialAcceptanceformData.FORM_SUBJECT,
                            APPLICANT_DEPT_NAME = medialAcceptanceformData.APPLICANT_DEPT_NAME,
                            APPLICANT_NAME = medialAcceptanceformData.APPLICANT_NAME,
                            APPLICANT_DATE_TIME = medialAcceptanceformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                            FORM_PATH = GlobalParameters.FormContentPath(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID, medialAcceptanceformData.IDENTIFY, medialAcceptanceformData.DIAGRAM_NAME),
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
                CommLib.Logger.Error("版權採購請款單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }
            return vResult;
        }

        /// <summary>
        /// 版權採購請款單(財務審核關卡-關聯表單(知會))：
        /// 【財務審核關卡】"版權採購請款"單關聯表單列表知會；
        /// 確認是否有代理人，
        /// 並知會給代理人。
        /// </summary>
        public bool PutMediaInvoiceNotifySingle(MediaInvoiceQueryModel query)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                #region - 財務審核人 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [FinancAuditID_1] AS [FINANC_AUDIT_ID_1] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                var dtFinancAudit1 = dbFun.DoQuery(strSQL, parameter);
                var FinancAudit1 = dtFinancAudit1.Rows[0][0].ToString();

                if (!string.IsNullOrEmpty(FinancAudit1) || !string.IsNullOrWhiteSpace(FinancAudit1))
                {
                    var flowQueryModel = new FlowQueryModel()
                    {
                        USER_ID = FinancAudit1
                    };
                    //被知會通知人
                    var NotifyBys = new List<String>()
                    {
                        FinancAudit1
                    };

                    #region - 代理人 -

                    var Agents = flowRepository.PostAgent(flowQueryModel);

                    if (Agents != null)
                    {
                        if (Agents.Count > 0)
                        {
                            Agents.ForEach(A =>
                            {
                                NotifyBys.Add(A.AGENT_ID);
                            });
                        }
                        else
                        {
                            CommLib.Logger.Debug(query.REQUISITION_ID + "：" + FinancAudit1 + "目前(" + DateTime.Now + ")尚無設定 「代理人」(2)。");
                        }
                    }
                    else
                    {
                        CommLib.Logger.Debug(query.REQUISITION_ID + "：" + FinancAudit1 + "目前(" + DateTime.Now + ")尚無設定 「代理人」(1)。");
                    }

                    #endregion

                    #region - 關聯表單(知會) -

                    var associatedFormNotifyModel = new AssociatedFormNotifyModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID,
                        NOTIFY_BY = NotifyBys
                    };
                    vResult = commonRepository.PutAssociatedFormNotify(associatedFormNotifyModel);

                    #endregion
                }
                else
                {
                    CommLib.Logger.Debug(query.REQUISITION_ID + "：此表單「尚未決定」財務審核人，可(知會)通知。");
                }

                #endregion

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購請款單(財務審核關卡-關聯表單(知會))通知失敗，原因：" + ex.Message);
                throw;
            }
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
        private string IDENTIFY = "MediaInvoice";

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