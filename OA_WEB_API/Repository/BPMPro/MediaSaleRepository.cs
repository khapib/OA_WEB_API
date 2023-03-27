using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權銷售申請單
    /// </summary>
    public class MediaSaleRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權銷售申請單(查詢)
        /// </summary>
        public MediaSaleViewModel PostMediaSaleSingle(MediaSaleQueryModel query)
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 版權銷售申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [EditFlag] AS [EDIT_FLAG], ";
            strSQL += "     [GroupID] AS [GROUP_ID], ";
            strSQL += "     [GroupBPMFormNo] AS [GROUP_BPM_FORM_NO], ";
            strSQL += "     [GroupPath] AS [GROUP_PATH], ";
            strSQL += "     [ParentID] AS [PARENT_ID], ";
            strSQL += "     [ParentBPMFormNo] AS [PARENT_BPM_FORM_NO], ";
            strSQL += "     [FormAction] AS [FORM_ACTION], ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaSaleTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleTitle>().FirstOrDefault();

            #endregion

            #region - 版權銷售申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [TXN_Type] AS [TXN_TYPE], ";
            strSQL += "     [ExecuteDate] AS [EXEC_DATE], ";
            strSQL += "     [TaxNote] AS [TAX_NOTE], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [ExchangeRate] AS [EXCH_RATE], ";
            strSQL += "     [PricingMethod] AS [PRICING_METHOD], ";
            strSQL += "     [TaxRate] AS [TAX_RATE], ";
            strSQL += "     [SupNo] AS [SUP_NO], ";
            strSQL += "     [SupName] AS [SUP_NAME], ";
            strSQL += "     [RegisterKind] AS [REG_KIND], ";
            strSQL += "     [RegisterNo] AS [REG_NO], ";
            strSQL += "     [OwnerName] AS [OWNER_NAME], ";
            strSQL += "     [OwnerTEL] AS [OWNER_TEL], ";
            strSQL += "     [PaymentPeriodTotal] AS [PAYMENT_PERIOD_TOTAL], ";
            strSQL += "     [DTL_NetTotal] AS [DTL_NET_TOTAL], ";
            strSQL += "     [DTL_NetTotal_TWD] AS [DTL_NET_TOTAL_TWD], ";
            strSQL += "     [DTL_TaxTotal] AS [DTL_TAX_TOTAL], ";
            strSQL += "     [DTL_TaxTotal_TWD] AS [DTL_TAX_TOTAL_TWD], ";
            strSQL += "     [DTL_GrossTotal] AS [DTL_GROSS_TOTAL], ";
            strSQL += "     [DTL_GrossTotal_TWD] AS [DTL_GROSS_TOTAL_TWD], ";
            strSQL += "     [EX_AmountTotal] AS [EX_AMOUNT_TOTAL], ";
            strSQL += "     [EX_AmountTotal_TWD] AS [EX_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [EX_TaxTotal] AS [EX_TAX_TOTAL], ";
            strSQL += "     [EX_TaxTotal_TWD] AS [EX_TAX_TOTAL_TWD], ";
            strSQL += "     [COLL_LockPeriod] AS [COLL_LOCK_PERIOD], ";
            strSQL += "     [COLL_TaxTotal] AS [COLL_TAX_TOTAL], ";
            strSQL += "     [COLL_NetTotal] AS [COLL_NET_TOTAL], ";
            strSQL += "     [COLL_GrossTotal] AS [COLL_GROSS_TOTAL], ";
            strSQL += "     [COLL_EX_AmountTotal] AS [COLL_EX_AMOUNT_TOTAL], ";
            strSQL += "     [COLL_EX_TaxTotal] AS [COLL_EX_TAX_TOTAL], ";
            strSQL += "     [COLL_OrderTotal] AS [COLL_ORDER_TOTAL], ";
            strSQL += "     [COLL_OrderTotal_CONV] AS [COLL_ORDER_TOTAL_CONV], ";
            strSQL += "     [COLL_UseBudgetTotal] AS [COLL_USE_BUDGET_TOTAL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaSaleConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleConfig>().FirstOrDefault();

            #endregion

            #region - 版權銷售申請單 銷售明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [DTL_RowNo] AS [DTL_ROW_NO], ";
            strSQL += "     [DTL_SupProdANo] AS [DTL_SUP_PROD_A_NO], ";
            strSQL += "     [DTL_ItemName] AS [DTL_ITEM_NAME], ";
            strSQL += "     [DTL_ItemType] AS [DTL_ITEM_TYPE], ";
            strSQL += "     [DTL_MediaType] AS [DTL_MEDIA_TYPE], ";
            strSQL += "     [DTL_StartEpisode] AS [DTL_START_EPISODE], ";
            strSQL += "     [DTL_EndEpisode] AS [DTL_END_EPISODE], ";
            strSQL += "     [DTL_ACPT_Episode] AS [DTL_ACPT_EPISODE], ";
            strSQL += "     [DTL_EpisodeTime] AS [DTL_EPISODE_TIME], ";
            strSQL += "     [DTL_Net] AS [DTL_NET], ";
            strSQL += "     [DTL_Net_TWD] AS [DTL_NET_TWD], ";
            strSQL += "     [DTL_Tax] AS [DTL_TAX], ";
            strSQL += "     [DTL_Tax_TWD] AS [DTL_TAX_TWD], ";
            strSQL += "     [DTL_Gross] AS [DTL_GROSS], ";
            strSQL += "     [DTL_Gross_TWD] AS [DTL_GROSS_TWD], ";
            strSQL += "     [DTL_NetSum] AS [DTL_NET_SUM], ";
            strSQL += "     [DTL_NetSum_TWD] AS [DTL_NET_SUM_TWD], ";
            strSQL += "     [DTL_GrossSum] AS [DTL_GROSS_SUM], ";
            strSQL += "     [DTL_GrossSum_TWD] AS [DTL_GROSS_SUM_TWD], ";
            strSQL += "     [DTL_ItemSum] AS [DTL_ITEM_SUM], ";
            strSQL += "     [DTL_ItemSum_TWD] AS [DTL_ITEM_SUM_TWD], ";
            strSQL += "     [DTL_ProjectFormNo] AS [DTL_PROJECT_FORM_NO], ";
            strSQL += "     [DTL_ProjectName] AS [DTL_PROJECT_NAME], ";
            strSQL += "     [DTL_ProjectNickname] AS [DTL_PROJECT_NICKNAME], ";
            strSQL += "     [DTL_ProjectUseYear] AS [DTL_PROJECT_USE_YEAR], ";
            strSQL += "     [DTL_Note] AS [DTL_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleDetailsConfig>();

            #endregion

            #region - 版權銷售申請單 授權權利 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [AUTH_RowNo] AS [AUTH_ROW_NO], ";
            strSQL += "     [AUTH_SupProdANo] AS [AUTH_SUP_PROD_A_NO], ";
            strSQL += "     [AUTH_ItemName] AS [AUTH_ITEM_NAME], ";
            strSQL += "     [AUTH_Continent] AS [AUTH_CONTINENT], ";
            strSQL += "     [AUTH_Country] AS [AUTH_COUNTRY], ";
            strSQL += "     [AUTH_PlayPlatform] AS [AUTH_PLAY_PLATFORM],";
            strSQL += "     [AUTH_Play] AS [AUTH_PLAY], ";
            strSQL += "     [AUTH_Sell] AS [AUTH_SELL], ";
            strSQL += "     [AUTH_EditToPlay] AS [AUTH_EDIT_TO_PLAY], ";
            strSQL += "     [AUTH_EditToSell] AS [AUTH_EDIT_TO_SELL], ";
            strSQL += "     [AUTH_AllotedTimeType] AS [AUTH_ALLOTED_TIME_TYPE], ";
            strSQL += "     [AUTH_StartDate] AS [AUTH_START_DATE], ";
            strSQL += "     [AUTH_EndDate] AS [AUTH_END_DATE], ";
            strSQL += "     [AUTH_FrequencyType] AS [AUTH_FREQUENCY_TYPE], ";
            strSQL += "     [AUTH_PlayFrequency] AS [AUTH_PLAY_FREQUENCY], ";
            strSQL += "     [AUTH_Note] AS [AUTH_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_AUTH] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleAuthorizesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleAuthorizesConfig>();

            #endregion

            #region - 版權銷售申請單 額外項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [EX_RowNo] AS [EX_ROW_NO], ";
            strSQL += "     [EX_Name] AS [EX_NAME], ";
            strSQL += "     [EX_Amount] AS [EX_AMOUNT], ";
            strSQL += "     [EX_Amount_TWD] AS [EX_AMOUNT_TWD], ";
            strSQL += "     [EX_Tax] AS [EX_TAX], ";
            strSQL += "     [EX_Tax_TWD] AS [EX_TAX_TWD], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [EX_ProjectFormNo] AS [EX_PROJECT_FORM_NO], ";
            strSQL += "     [EX_ProjectName] AS [EX_PROJECT_NAME], ";
            strSQL += "     [EX_ProjectNickname] AS [EX_PROJECT_NICKNAME], ";
            strSQL += "     [EX_ProjectUseYear] AS [EX_PROJECT_USE_YEAR], ";
            strSQL += "     [EX_Note] AS [EX_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_EX] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleExtrasConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleExtrasConfig>();

            #endregion

            #region - 版權銷售申請單 收款辦法 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [COLL_RowNo] AS [COLL_ROW_NO], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [COLL_Project] AS [COLL_PROJECT], ";
            strSQL += "     [COLL_Terms] AS [COLL_TERMS], ";
            strSQL += "     [COLL_MethodID] AS [COLL_METHOD_ID], ";
            strSQL += "     [COLL_Tax] AS [COLL_TAX], ";
            strSQL += "     [COLL_Net] AS [COLL_NET], ";
            strSQL += "     [COLL_Gross] AS [COLL_GROSS], ";
            strSQL += "     [COLL_ExchangeRate] AS [COLL_EXCH_RATE], ";
            strSQL += "     [COLL_EX_Amount] AS [COLL_EX_AMOUNT], ";
            strSQL += "     [COLL_EX_Tax] AS [COLL_EX_TAX], ";
            strSQL += "     [COLL_OrderSum] AS [COLL_ORDER_SUM], ";
            strSQL += "     [COLL_OrderSum_CONV] AS [COLL_ORDER_SUM_CONV], ";
            strSQL += "     [COLL_UseBudget] AS [COLL_USE_BUDGET] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_COLL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleCollectionsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleCollectionsConfig>();

            #endregion

            #region - 版權銷售申請單 使用預算 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [BUDG_RowNo] AS [BUDG_ROW_NO], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [BUDG_FormNo] AS [BUDG_FORM_NO], ";
            strSQL += "     [BUDG_CreateYear] AS [BUDG_CREATE_YEAR], ";
            strSQL += "     [BUDG_Name] AS [BUDG_NAME], ";
            strSQL += "     [BUDG_OwnerDept] AS [BUDG_OWNER_DEPT], ";
            strSQL += "     [BUDG_Total] AS [BUDG_TOTAL], ";
            strSQL += "     [BUDG_AvailableBudgetAmount] AS [BUDG_AVAILABLE_BUDGET_AMOUNT], ";
            strSQL += "     [BUDG_UseBudgetAmount] AS [BUDG_USE_BUDGET_AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_BUDG] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleBudgetsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleBudgetsConfig>();

            #endregion

            #region - 版權銷售申請單 交付項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [DELY_RowNo] AS [DELY_ROW_NO], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [DELY_SupProdANo] AS [DELY_SUP_PROD_A_NO], ";
            strSQL += "     [DELY_ItemName] AS [DELY_ITEM_NAME], ";
            strSQL += "     [DELY_MediaType] AS [DELY_MEDIA_TYPE], ";
            strSQL += "     [DELY_StartEpisode] AS [DELY_START_EPISODE], ";
            strSQL += "     [DELY_EndEpisode] AS [DELY_END_EPISODE], ";
            strSQL += "     [DELY_ACPT_Episode] AS [DELY_ACPT_EPISODE], ";
            strSQL += "     [DELY_Note] AS [DELY_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_DELY] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleDeliverysConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleDeliverysConfig>();

            #endregion

            #region - 版權銷售申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            MediaSaleViewModel mediaSaleViewModel = new MediaSaleViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_SALE_TITLE = mediaSaleTitle,
                MEDIA_SALE_CONFIG = mediaSaleConfig,
                MEDIA_SALE_DTLS_CONFIG = mediaSaleDetailsConfig,
                MEDIA_SALE_AUTHS_CONFIG = mediaSaleAuthorizesConfig,
                MEDIA_SALE_EXS_CONFIG = mediaSaleExtrasConfig,
                MEDIA_SALE_COLLS_CONFIG = mediaSaleCollectionsConfig,
                MEDIA_SALE_BUDGS_CONFIG = mediaSaleBudgetsConfig,
                MEDIA_SALE_DELYS_CONFIG = mediaSaleDeliverysConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region 確認是否匯入【子表單】

            if (mediaSaleTitle != null)
            {
                if (!String.IsNullOrEmpty(mediaSaleTitle.GROUP_ID) || !String.IsNullOrWhiteSpace(mediaSaleTitle.GROUP_ID))
                {
                    try
                    {
                        #region 判斷 編輯註記及匯入【子表單】

                        //是空null就給false預設值
                        mediaSaleTitle.EDIT_FLAG = mediaSaleTitle.EDIT_FLAG ?? "false";

                        if (!Boolean.Parse(mediaSaleTitle.EDIT_FLAG))
                        {
                            //匯入【子表單】
                            //mediaSaleViewModel = PutMediaSaleImportSingle(mediaSaleViewModel);
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("版權採購申請單(原表單匯入子表單)失敗；請確認原表單資訊是否正確。原因：" + ex.Message);
                    }

                }
            }

            #endregion

            return mediaSaleViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權銷售申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaSaleRefill(MediaSaleQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權銷售申請單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權銷售申請單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaSaleSingle(MediaSaleViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                if (String.IsNullOrEmpty(model.MEDIA_SALE_TITLE.GROUP_ID) || String.IsNullOrWhiteSpace(model.MEDIA_SALE_TITLE.GROUP_ID))
                {
                    model.MEDIA_SALE_TITLE.FORM_ACTION = "申請";
                }

                #region - 主旨 -

                if (String.IsNullOrEmpty(model.MEDIA_SALE_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.MEDIA_SALE_TITLE.FM7_SUBJECT))
                {
                    // 單號由流程事件做寫入
                    FM7Subject = "(待填寫)" + model.MEDIA_SALE_TITLE.FLOW_NAME + "  ";
                }
                else
                {
                    FM7Subject = model.MEDIA_SALE_TITLE.FM7_SUBJECT;

                    if (!String.IsNullOrEmpty(model.MEDIA_SALE_TITLE.GROUP_ID) || !String.IsNullOrWhiteSpace(model.MEDIA_SALE_TITLE.GROUP_ID))
                    {
                        if (FM7Subject.Substring(1, 2) != "異動")
                        {
                            FM7Subject = "【異動】" + FM7Subject;
                        }
                    }

                    #region 零稅率

                    if (model.MEDIA_SALE_CONFIG != null)
                    {
                        if (model.MEDIA_SALE_CONFIG.CURRENCY == "台幣" && model.MEDIA_SALE_CONFIG.TAX_RATE == 0.0)
                        {
                            FM7Subject += "  (零稅率)";
                        }
                    }

                    #endregion
                }

                #endregion

                //編輯註記
                model.MEDIA_SALE_TITLE.EDIT_FLAG = "true";

                #endregion

                #region - 版權銷售申請單 表頭資訊：MediaSale_M -

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
                    //版權銷售申請單 表頭
                    new SqlParameter("@EDIT_FLAG", SqlDbType.NVarChar) { Size = 5, Value = (object)model.MEDIA_SALE_TITLE.EDIT_FLAG ?? DBNull.Value },
                    new SqlParameter("@GROUP_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_SALE_TITLE.GROUP_ID ?? DBNull.Value },
                    new SqlParameter("@GROUP_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_SALE_TITLE.GROUP_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@GROUP_PATH", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_SALE_TITLE.GROUP_PATH ?? DBNull.Value },
                    new SqlParameter("@PARENT_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_SALE_TITLE.PARENT_ID ?? DBNull.Value },
                    new SqlParameter("@PARENT_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_SALE_TITLE.PARENT_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FORM_ACTION", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_SALE_TITLE.FORM_ACTION ?? String.Empty },
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_SALE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_SALE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaSale_M] ";
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
                    strSQL += "     [EditFlag]=@EDIT_FLAG, ";
                    strSQL += "     [GroupID]=@GROUP_ID, ";
                    strSQL += "     [GroupBPMFormNo]=@GROUP_BPM_FORM_NO, ";
                    strSQL += "     [GroupPath]=@GROUP_PATH, ";
                    strSQL += "     [ParentID]=@PARENT_ID, ";
                    strSQL += "     [ParentBPMFormNo]=@PARENT_BPM_FORM_NO, ";
                    strSQL += "     [FormAction]=@FORM_ACTION, ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[EditFlag],[GroupID],[GroupBPMFormNo],[GroupPath],[ParentID],[ParentBPMFormNo],[FormAction],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@EDIT_FLAG,@GROUP_ID,@GROUP_BPM_FORM_NO,@GROUP_PATH,@PARENT_ID,@PARENT_BPM_FORM_NO,@FORM_ACTION,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 表單內容：MediaSale_M -

                if (model.MEDIA_SALE_CONFIG != null)
                {
                    #region - 確認小數點後第二位 -

                    model.MEDIA_SALE_CONFIG.EXCH_RATE = Math.Round(model.MEDIA_SALE_CONFIG.EXCH_RATE, 2);
                    model.MEDIA_SALE_CONFIG.TAX_RATE = Math.Round(model.MEDIA_SALE_CONFIG.TAX_RATE, 2);
                    model.MEDIA_SALE_CONFIG.DTL_NET_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.DTL_NET_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.DTL_TAX_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.DTL_TAX_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.DTL_GROSS_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.DTL_GROSS_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.EX_AMOUNT_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.EX_AMOUNT_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.EX_TAX_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.EX_TAX_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.COLL_TAX_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.COLL_TAX_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.COLL_NET_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.COLL_NET_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.COLL_GROSS_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.COLL_GROSS_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.COLL_EX_AMOUNT_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.COLL_EX_AMOUNT_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.COLL_EX_TAX_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.COLL_EX_TAX_TOTAL, 2);
                    model.MEDIA_SALE_CONFIG.COLL_ORDER_TOTAL = Math.Round(model.MEDIA_SALE_CONFIG.COLL_ORDER_TOTAL, 2);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權銷售申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TXN_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXEC_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXCH_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRICING_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64,  Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_TEL", SqlDbType.NVarChar) { Size = 20,  Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAYMENT_PERIOD_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DISCOUNT_PRICE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_LOCK_PERIOD", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_ORDER_TOTAL_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_USE_BUDGET_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：版權銷售申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_SALE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaSale_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [TXN_Type]=@TXN_TYPE, ";
                    strSQL += "     [ExecuteDate]=@EXEC_DATE, ";
                    strSQL += "     [TaxNote]=@TAX_NOTE, ";
                    strSQL += "     [Currency]=@CURRENCY, ";
                    strSQL += "     [ExchangeRate]=@EXCH_RATE, ";
                    strSQL += "     [PricingMethod]=@PRICING_METHOD, ";
                    strSQL += "     [TaxRate]=@TAX_RATE, ";
                    strSQL += "     [SupNo]=@SUP_NO, ";
                    strSQL += "     [SupName]=@SUP_NAME, ";
                    strSQL += "     [RegisterKind]=@REG_KIND, ";
                    strSQL += "     [RegisterNo]=@REG_NO, ";
                    strSQL += "     [OwnerName]=@OWNER_NAME, ";
                    strSQL += "     [OwnerTEL]=@OWNER_TEL, ";
                    strSQL += "     [PaymentPeriodTotal]=@PAYMENT_PERIOD_TOTAL, ";
                    strSQL += "     [DTL_NetTotal]=@DTL_NET_TOTAL, ";
                    strSQL += "     [DTL_NetTotal_TWD]=@DTL_NET_TOTAL_TWD, ";
                    strSQL += "     [DTL_TaxTotal]=@DTL_TAX_TOTAL, ";
                    strSQL += "     [DTL_TaxTotal_TWD]=@DTL_TAX_TOTAL_TWD, ";
                    strSQL += "     [DTL_GrossTotal]=@DTL_GROSS_TOTAL, ";
                    strSQL += "     [DTL_GrossTotal_TWD]=@DTL_GROSS_TOTAL_TWD, ";
                    strSQL += "     [EX_AmountTotal]=@EX_AMOUNT_TOTAL, ";
                    strSQL += "     [EX_AmountTotal_TWD]=@EX_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [EX_TaxTotal]=@EX_TAX_TOTAL, ";
                    strSQL += "     [EX_TaxTotal_TWD]=@EX_TAX_TOTAL_TWD, ";
                    strSQL += "     [COLL_LockPeriod]=@COLL_LOCK_PERIOD, ";
                    strSQL += "     [COLL_TaxTotal]=@COLL_TAX_TOTAL, ";
                    strSQL += "     [COLL_NetTotal]=@COLL_NET_TOTAL, ";
                    strSQL += "     [COLL_GrossTotal]=@COLL_GROSS_TOTAL, ";
                    strSQL += "     [COLL_EX_AmountTotal]=@COLL_EX_AMOUNT_TOTAL, ";
                    strSQL += "     [COLL_EX_TaxTotal]=@COLL_EX_TAX_TOTAL, ";
                    strSQL += "     [COLL_OrderTotal]=@COLL_ORDER_TOTAL, ";
                    strSQL += "     [COLL_OrderTotal_CONV]=@COLL_ORDER_TOTAL_CONV, ";
                    strSQL += "     [COLL_UseBudgetTotal]=@COLL_USE_BUDGET_TOTAL ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權銷售申請單 銷售明細：MediaSale_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //版權銷售申請單 銷售明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DTL_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_ITEM_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_ACPT_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_EPISODE_TIME", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_NET_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_NET_SUM_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_GROSS_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_GROSS_SUM_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_ITEM_SUM", SqlDbType.Float) { Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_ITEM_SUM_TWD", SqlDbType.Int) { Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_PROJECT_NAME", SqlDbType.NVarChar) { Size = 500, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DTL_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_SALE_DTLS_CONFIG != null && model.MEDIA_SALE_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_DTLS_CONFIG)
                    {
                        #region - 確認小數點後第二位 -

                        item.DTL_NET = Math.Round(item.DTL_NET, 2);
                        item.DTL_GROSS = Math.Round(item.DTL_GROSS, 2);
                        item.DTL_NET_SUM = Math.Round(item.DTL_NET_SUM, 2);
                        item.DTL_TAX = Math.Round(item.DTL_TAX, 2);
                        item.DTL_GROSS_SUM = Math.Round(item.DTL_GROSS_SUM, 2);
                        item.DTL_ITEM_SUM = Math.Round(item.DTL_ITEM_SUM, 2);

                        #endregion

                        //寫入：版權銷售申請單 銷售明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_DTL]([RequisitionID],[DTL_RowNo],[DTL_SupProdANo],[DTL_ItemName],[DTL_ItemType],[DTL_MediaType],[DTL_StartEpisode],[DTL_EndEpisode],[DTL_OrderEpisode],[DTL_ACPT_Episode],[DTL_EpisodeTime],[DTL_Net],[DTL_Net_TWD],[DTL_Tax],[DTL_Tax_TWD],[DTL_Gross],[DTL_Gross_TWD],[DTL_NetSum],[DTL_NetSum_TWD],[DTL_GrossSum],[DTL_GrossSum_TWD],[DTL_ItemSum],[DTL_ItemSum_TWD],[DTL_ProjectFormNo],[DTL_ProjectName],[DTL_ProjectNickname],[DTL_ProjectUseYear],[DTL_Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@DTL_ROW_NO,@DTL_SUP_PROD_A_NO,@DTL_ITEM_NAME,@DTL_ITEM_TYPE,@DTL_MEDIA_TYPE,@DTL_START_EPISODE,@DTL_END_EPISODE,@DTL_ORDER_EPISODE,@DTL_ACPT_EPISODE,@DTL_EPISODE_TIME,@DTL_NET,@DTL_NET_TWD,@DTL_TAX,@DTL_TAX_TWD,@DTL_GROSS,@DTL_GROSS_TWD,@DTL_NET_SUM,@DTL_NET_SUM_TWD,@DTL_GROSS_SUM,@DTL_GROSS_SUM_TWD,@DTL_ITEM_SUM,@DTL_ITEM_SUM_TWD,@DTL_PROJECT_FORM_NO,@DTL_PROJECT_NAME,@DTL_PROJECT_NICKNAME,@DTL_PROJECT_USE_YEAR,@DTL_NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 授權權利：MediaSale_AUTH -

                var parameterAuthorizes = new List<SqlParameter>()
                {
                    //版權銷售申請單 授權權利
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@AUTH_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_CONTINENT", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_COUNTRY", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_PLAY_PLATFORM", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_PLAY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_SELL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_EDIT_TO_PLAY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_EDIT_TO_SELL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_ALLOTED_TIME_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_START_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_END_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_FREQUENCY_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_PLAY_FREQUENCY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_AUTH] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterAuthorizes);

                #endregion

                if (model.MEDIA_SALE_AUTHS_CONFIG != null && model.MEDIA_SALE_AUTHS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_AUTHS_CONFIG)
                    {

                        //寫入：版權銷售申請單 授權權利parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterAuthorizes);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_AUTH]([RequisitionID],[AUTH_RowNo],[AUTH_SupProdANo],[AUTH_ItemName],[AUTH_Continent],[AUTH_Country],[AUTH_PlayPlatform],[AUTH_Play],[AUTH_Sell],[AUTH_EditToPlay],[AUTH_EditToSell],[AUTH_AllotedTimeType],[AUTH_StartDate],[AUTH_EndDate],[AUTH_FrequencyType],[AUTH_PlayFrequency],[AUTH_Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@AUTH_ROW_NO,@AUTH_SUP_PROD_A_NO,@AUTH_ITEM_NAME,@AUTH_CONTINENT,@AUTH_COUNTRY,@AUTH_PLAY_PLATFORM,@AUTH_PLAY,@AUTH_SELL,@AUTH_EDIT_TO_PLAY,@AUTH_EDIT_TO_SELL,@AUTH_ALLOTED_TIME_TYPE,@AUTH_START_DATE,@AUTH_END_DATE,@AUTH_FREQUENCY_TYPE,@AUTH_PLAY_FREQUENCY,@AUTH_NOTE) ";

                        dbFun.DoTran(strSQL, parameterAuthorizes);
                    }

                    #endregion
                }

                #endregion

                

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("版權銷售申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaSale";

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