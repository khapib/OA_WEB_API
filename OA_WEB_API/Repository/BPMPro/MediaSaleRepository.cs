using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;

using OA_WEB_API.Models.BPMPro;
using System.Collections;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權銷售申請單
    /// </summary>
    public class MediaSaleRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

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

            var CommonApplicantInfo = new BPMCommonModel<ApplicantInfo>()
            {
                EXT = "M",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter,
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApplicantInfoFunction(CommonApplicantInfo));
            var applicantInfo = jsonFunction.JsonToObject<ApplicantInfo>(strJson);

            #endregion

            #region - M表寫入BPM表單單號 -

            //避免儲存後送出表單BPM表單單號沒寫入的情形
            var formQuery = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            if (applicantInfo.DRAFT_FLAG == 0) notifyRepository.ByInsertBPMFormNo(formQuery);

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
            strSQL += "     [IsVicePresident] AS [IS_VICE_PRESIDENT], ";
            strSQL += "     [TXN_Type] AS [TXN_TYPE], ";
            strSQL += "     [ExecuteDate] AS [EXEC_DATE], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [ExchangeRate] AS [EXCH_RATE], ";
            strSQL += "     [IsTaxBill] AS [IS_TAX_BILL], ";
            strSQL += "     [TaxNote] AS [TAX_NOTE], ";
            strSQL += "     [PricingMethod] AS [PRICING_METHOD], ";
            strSQL += "     [Know_GROSS] AS [KNOW_GROSS], ";
            strSQL += "     [TaxRate] AS [TAX_RATE], ";
            strSQL += "     [TRS_RateTotal] AS [TRS_RATE_TOTAL], ";
            strSQL += "     [TRS_TaxTotal] AS [TRS_TAX_TOTAL], ";
            strSQL += "     [SupNo] AS [SUP_NO], ";
            strSQL += "     [SupName] AS [SUP_NAME], ";
            strSQL += "     [RegisterKind] AS [REG_KIND], ";
            strSQL += "     [RegisterNo] AS [REG_NO], ";
            strSQL += "     [OwnerName] AS [OWNER_NAME], ";
            strSQL += "     [OwnerTEL] AS [OWNER_TEL], ";
            strSQL += "     [CollectionPeriodTotal] AS [COLLECTION_PERIOD_TOTAL], ";
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
            strSQL += "     [COLL_LockPeriod] AS [COLL_LOCK_PERIOD], ";
            strSQL += "     [COLL_NetTotal] AS [COLL_NET_TOTAL], ";
            strSQL += "     [COLL_GrossTotal] AS [COLL_GROSS_TOTAL], ";
            strSQL += "     [COLL_MaterialTotal] AS [COLL_MATERIAL_TOTAL], ";
            strSQL += "     [COLL_EX_AmountTotal] AS [COLL_EX_AMOUNT_TOTAL], ";
            strSQL += "     [COLL_OrderTotal] AS [COLL_ORDER_TOTAL], ";
            strSQL += "     [COLL_OrderTotal_CONV] AS [COLL_ORDER_TOTAL_CONV], ";
            strSQL += "     [COLL_UseBudgetTotal] AS [COLL_USE_BUDGET_TOTAL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaSaleConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleConfig>().FirstOrDefault();

            #endregion

            #region - 版權銷售申請單 稅率結構 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [Rate] AS [RATE], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_TRS] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleTaxRateStructuresConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleTaxRateStructuresConfig>();

            #endregion

            #region - 版權銷售申請單 銷售明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [DELY_Episode] AS [DELY_EPISODE], ";
            strSQL += "     [EpisodeTime] AS [EPISODE_TIME], ";
            strSQL += "     [Media_Spec] AS [MEDIA_SPEC], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Net_TWD] AS [NET_TWD], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [Gross_TWD] AS [GROSS_TWD], ";
            strSQL += "     [Material] AS [MATERIAL], ";
            strSQL += "     [ItemSum] AS [ITEM_SUM], ";
            strSQL += "     [ItemSum_TWD] AS [ITEM_SUM_TWD], ";
            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleDetailsConfig>();

            #endregion

            #region - 版權銷售申請單 授權權利 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [Continent] AS [CONTINENT], ";
            strSQL += "     [Country] AS [COUNTRY], ";
            strSQL += "     [PlayPlatform] AS [PLAY_PLATFORM],";
            strSQL += "     [Play] AS [PLAY], ";
            strSQL += "     [Sell] AS [SELL], ";
            strSQL += "     [EditToPlay] AS [EDIT_TO_PLAY], ";
            strSQL += "     [EditToSell] AS [EDIT_TO_SELL], ";
            strSQL += "     [AllotedTimeType] AS [ALLOTED_TIME_TYPE], ";
            strSQL += "     [StartDate] AS [START_DATE], ";
            strSQL += "     [EndDate] AS [END_DATE], ";
            strSQL += "     [FrequencyType] AS [FREQUENCY_TYPE], ";
            strSQL += "     [PlayFrequency] AS [PLAY_FREQUENCY], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_AUTH] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleAuthorizesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleAuthorizesConfig>();

            #endregion

            #region - 版權銷售申請單 額外項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [TaxIncl] AS [TAX_INCL], ";
            strSQL += "     [TaxIncl_TWD] AS [TAX_INCL_TWD], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_EX] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleExtrasConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleExtrasConfig>();

            #endregion

            #region - 版權銷售申請單 收款辦法 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [COLL_Project] AS [COLL_PROJECT], ";
            strSQL += "     [COLL_Terms] AS [COLL_TERMS], ";
            strSQL += "     [COLL_MethodID] AS [COLL_METHOD_ID], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [Material] AS [MATERIAL], ";
            strSQL += "     [EX_Amount] AS [EX_AMOUNT], ";
            strSQL += "     [OrderSum] AS [ORDER_SUM], ";
            strSQL += "     [OrderSum_CONV] AS [ORDER_SUM_CONV], ";
            strSQL += "     [UseBudget] AS [USE_BUDGET] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_COLL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaSaleCollectionsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaSaleCollectionsConfig>();

            #endregion

            #region - 版權銷售申請單 使用預算 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [BUDG_FormNo] AS [BUDG_FORM_NO], ";
            strSQL += "     [BUDG_CreateYear] AS [BUDG_CREATE_YEAR], ";
            strSQL += "     [BUDG_Name] AS [BUDG_NAME], ";
            strSQL += "     [OwnerDept] AS [OWNER_DEPT], ";
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
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [DELY_Episode] AS [DELY_EPISODE] ";
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

            var mediaSaleViewModel = new MediaSaleViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_SALE_TITLE = mediaSaleTitle,
                MEDIA_SALE_CONFIG = mediaSaleConfig,
                MEDIA_SALE_TRSS_CONFIG=mediaSaleTaxRateStructuresConfig,
                MEDIA_SALE_DTLS_CONFIG = mediaSaleDetailsConfig,
                MEDIA_SALE_AUTHS_CONFIG = mediaSaleAuthorizesConfig,
                MEDIA_SALE_EXS_CONFIG = mediaSaleExtrasConfig,
                MEDIA_SALE_COLLS_CONFIG = mediaSaleCollectionsConfig,
                MEDIA_SALE_BUDGS_CONFIG = mediaSaleBudgetsConfig,
                MEDIA_SALE_DELYS_CONFIG = mediaSaleDeliverysConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認表單 -

            if (mediaSaleViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    mediaSaleViewModel = new MediaSaleViewModel();
                    CommLib.Logger.Error("版權銷售申請單(查詢)失敗，原因：系統無正常起單。");
                }
                else
                {
                    #region - 確認M表BPM表單單號 -

                    //避免儲存後送出表單BPM表單單號沒寫入的情形
                    notifyRepository.ByInsertBPMFormNo(formQuery);

                    if (String.IsNullOrEmpty(mediaSaleViewModel.MEDIA_SALE_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(mediaSaleViewModel.MEDIA_SALE_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) mediaSaleViewModel.MEDIA_SALE_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

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
                            mediaSaleViewModel = PutMediaSaleImportSingle(mediaSaleViewModel);
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

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

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

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    var formData = new FormData()
                    {
                        REQUISITION_ID = strREQ
                    };

                    if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
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

                    if (IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

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
                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_SALE_CONFIG);

                    #region - 確認小數點後第二位 -

                    GlobalParameters.IsDouble(strJson);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權銷售申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_VICE_PRESIDENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TXN_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXEC_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXCH_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_TAX_BILL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRICING_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@KNOW_GROSS", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TRS_RATE_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TRS_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64,  Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_TEL", SqlDbType.NVarChar) { Size = 20,  Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLLECTION_PERIOD_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
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
                        new SqlParameter("@COLL_LOCK_PERIOD", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_MATERIAL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_ORDER_TOTAL_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COLL_USE_BUDGET_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：版權銷售申請單 表單內容parameter                    
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaSale_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [IsVicePresident]=@IS_VICE_PRESIDENT, ";
                    strSQL += "     [TXN_Type]=@TXN_TYPE, ";
                    strSQL += "     [ExecuteDate]=@EXEC_DATE, ";
                    strSQL += "     [TaxNote]=@TAX_NOTE, ";
                    strSQL += "     [Currency]=@CURRENCY, ";
                    strSQL += "     [ExchangeRate]=@EXCH_RATE, ";
                    strSQL += "     [PricingMethod]=@PRICING_METHOD, ";
                    strSQL += "     [Know_GROSS]=@KNOW_GROSS, ";
                    strSQL += "     [TaxRate]=@TAX_RATE, ";
                    strSQL += "     [TRS_RateTotal]=@TRS_RATE_TOTAL, ";
                    strSQL += "     [TRS_TaxTotal]=@TRS_TAX_TOTAL, ";
                    strSQL += "     [SupNo]=@SUP_NO, ";
                    strSQL += "     [SupName]=@SUP_NAME, ";
                    strSQL += "     [RegisterKind]=@REG_KIND, ";
                    strSQL += "     [RegisterNo]=@REG_NO, ";
                    strSQL += "     [OwnerName]=@OWNER_NAME, ";
                    strSQL += "     [OwnerTEL]=@OWNER_TEL, ";
                    strSQL += "     [CollectionPeriodTotal]=@COLLECTION_PERIOD_TOTAL, ";
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
                    strSQL += "     [COLL_LockPeriod]=@COLL_LOCK_PERIOD, ";
                    strSQL += "     [COLL_NetTotal]=@COLL_NET_TOTAL, ";
                    strSQL += "     [COLL_GrossTotal]=@COLL_GROSS_TOTAL, ";
                    strSQL += "     [COLL_MaterialTotal]=@COLL_MATERIAL_TOTAL, ";
                    strSQL += "     [COLL_EX_AmountTotal]=@COLL_EX_AMOUNT_TOTAL, ";
                    strSQL += "     [COLL_OrderTotal]=@COLL_ORDER_TOTAL, ";
                    strSQL += "     [COLL_OrderTotal_CONV]=@COLL_ORDER_TOTAL_CONV, ";
                    strSQL += "     [COLL_UseBudgetTotal]=@COLL_USE_BUDGET_TOTAL ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權銷售申請單 稅率結構：MediaSale_TRS -

                var parameterTaxRateStructures = new List<SqlParameter>()
                {
                    //版權銷售申請單 稅率結構
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@NAME", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_TRS] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterTaxRateStructures);

                #endregion

                if (model.MEDIA_SALE_TRSS_CONFIG != null && model.MEDIA_SALE_TRSS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_TRSS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權銷售申請單 稅率結構parameter
                        GlobalParameters.Infoparameter(strJson, parameterTaxRateStructures);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_TRS]([RequisitionID],[Name],[Rate],[Tax],[Tax_TWD],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@NAME,@RATE,@TAX,@TAX_TWD,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterTaxRateStructures);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 銷售明細：MediaSale_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //版權銷售申請單 銷售明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DELY_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MATERIAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_SUM", SqlDbType.Float) { Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_SUM_TWD", SqlDbType.Int) { Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
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
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權銷售申請單 銷售明細parameter
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_DTL]([RequisitionID],[RowNo],[SupProdANo],[ItemName],[MediaType],[StartEpisode],[EndEpisode],[DELY_Episode],[EpisodeTime],[Media_Spec],[Net],[Net_TWD],[Tax],[Tax_TWD],[Gross],[Gross_TWD],[Material],[ItemSum],[ItemSum_TWD],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@DELY_EPISODE,@EPISODE_TIME,@MEDIA_SPEC,@NET,@NET_TWD,@TAX,@TAX_TWD,@GROSS,@GROSS_TWD,@MATERIAL,@ITEM_SUM,@ITEM_SUM_TWD,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 授權權利：MediaSale_AUTH -

                var parameterAuthorizes = new List<SqlParameter>()
                {
                    //版權銷售申請單 授權權利
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CONTINENT", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COUNTRY", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PLAY_PLATFORM", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PLAY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SELL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EDIT_TO_PLAY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EDIT_TO_SELL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ALLOTED_TIME_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FREQUENCY_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PLAY_FREQUENCY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
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
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_AUTH]([RequisitionID],[RowNo],[SupProdANo],[ItemName],[Continent],[Country],[PlayPlatform],[Play],[Sell],[EditToPlay],[EditToSell],[AllotedTimeType],[StartDate],[EndDate],[FrequencyType],[PlayFrequency],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@CONTINENT,@COUNTRY,@PLAY_PLATFORM,@PLAY,@SELL,@EDIT_TO_PLAY,@EDIT_TO_SELL,@ALLOTED_TIME_TYPE,@START_DATE,@END_DATE,@FREQUENCY_TYPE,@PLAY_FREQUENCY,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterAuthorizes);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 額外項目：MediaSale_EX -

                var parameterExtras = new List<SqlParameter>()
                {
                    //版權銷售申請單 額外項目
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_INCL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_INCL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_EX] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterAuthorizes);

                #endregion

                if (model.MEDIA_SALE_EXS_CONFIG != null && model.MEDIA_SALE_EXS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_EXS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權銷售申請單 授權權利parameter                        
                        GlobalParameters.Infoparameter(strJson, parameterExtras);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_EX]([RequisitionID],[RowNo],[Name],[TaxIncl],[TaxIncl_TWD],[Period],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@NAME,@TAX_INCL,@TAX_INCL_TWD,@PERIOD,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterExtras);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 收款辦法：MediaSale_COLL -

                var parameterCollections = new List<SqlParameter>()
                {
                    //版權銷售申請單 收款辦法
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COLL_PROJECT", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COLL_TERMS", SqlDbType.NVarChar) { Size = 25, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COLL_METHOD_ID", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MATERIAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EX_AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_SUM_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USE_BUDGET", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_COLL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterCollections);

                #endregion

                if (model.MEDIA_SALE_COLLS_CONFIG != null && model.MEDIA_SALE_COLLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_COLLS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權銷售申請單 收款辦法parameter                                               
                        GlobalParameters.Infoparameter(strJson, parameterCollections);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_COLL]([RequisitionID],[Period],[COLL_Project],[COLL_Terms],[COLL_MethodID],[Net],[Gross],[Material],[EX_Amount],[OrderSum],[OrderSum_CONV],[UseBudget]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@COLL_PROJECT,@COLL_TERMS,@COLL_METHOD_ID,@NET,@GROSS,@MATERIAL,@EX_AMOUNT,@ORDER_SUM,@ORDER_SUM_CONV,@USE_BUDGET) ";

                        dbFun.DoTran(strSQL, parameterCollections);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 使用預算：MediaSale_BUDG -

                var parameterBudgets = new List<SqlParameter>()
                {
                    //版權銷售申請單 使用預算
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BUDG_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BUDG_CREATE_YEAR", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BUDG_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEPT", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BUDG_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BUDG_AVAILABLE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BUDG_USE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_BUDG] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoQuery(strSQL, parameterBudgets);

                #endregion

                if (model.MEDIA_SALE_BUDGS_CONFIG != null && model.MEDIA_SALE_BUDGS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_BUDGS_CONFIG)
                    {
                        //寫入：版權銷售申請單 使用預算parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterBudgets);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_BUDG]([RequisitionID],[Period],[BUDG_FormNo],[BUDG_CreateYear],[BUDG_Name],[OwnerDept],[BUDG_Total],[BUDG_AvailableBudgetAmount],[BUDG_UseBudgetAmount]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@BUDG_FORM_NO,@BUDG_CREATE_YEAR,@BUDG_NAME,@OWNER_DEPT,@BUDG_TOTAL,@BUDG_AVAILABLE_BUDGET_AMOUNT,@BUDG_USE_BUDGET_AMOUNT) ";

                        dbFun.DoTran(strSQL, parameterBudgets);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 交付項目：MediaSale_DELY -

                var parameterDeliverys = new List<SqlParameter>()
                {
                    //版權銷售申請單 交付項目
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DELY_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaSale_DELY] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDeliverys);

                #endregion

                if (model.MEDIA_SALE_DELYS_CONFIG != null && model.MEDIA_SALE_DELYS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_SALE_DELYS_CONFIG)
                    {
                        //寫入：版權銷售申請單 交付項目parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDeliverys);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaSale_DELY]([RequisitionID],[RowNo],[Period],[SupProdANo],[ItemName],[MediaType],[StartEpisode],[EndEpisode],[DELY_Episode]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@PERIOD,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@DELY_EPISODE) ";

                        dbFun.DoTran(strSQL, parameterDeliverys);
                    }

                    #endregion
                }

                #endregion

                #region - 版權銷售申請單 表單關聯：AssociatedForm -

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = strREQ,
                    ASSOCIATED_FORM_CONFIG = model.ASSOCIATED_FORM_CONFIG
                };

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
                CommLib.Logger.Error("版權銷售申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 版權銷售申請單(原表單匯入子表單)
        /// </summary>
        public MediaSaleViewModel PutMediaSaleImportSingle(MediaSaleViewModel model)
        {
            try
            {
                #region 原表單_版權銷售申請單(查詢)

                var groupQuery = new MediaSaleQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_SALE_TITLE.GROUP_ID
                };
                //原表單_版權銷售申請單(查詢)
                var postMediaSaleGroupSingle = PostMediaSaleSingle(groupQuery);

                #endregion

                #region 組裝要匯入寫回【子表單】的內容

                #region 保留【子表單】所需欄位

                var ChildFormAction = model.MEDIA_SALE_TITLE.FORM_ACTION;
                var ChildPYMT_LockPeriod = model.MEDIA_SALE_CONFIG.COLL_LOCK_PERIOD;

                #endregion

                //版權銷售申請單 表頭資訊:ERP 工作流程標題名稱
                model.MEDIA_SALE_TITLE.FLOW_NAME = postMediaSaleGroupSingle.MEDIA_SALE_TITLE.FLOW_NAME;
                //版權銷售申請單 設定
                model.MEDIA_SALE_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_CONFIG;
                //版權銷售申請單 稅率結構 設定
                model.MEDIA_SALE_TRSS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_TRSS_CONFIG;
                //版權銷售申請單 採購明細 設定
                model.MEDIA_SALE_DTLS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_DTLS_CONFIG;
                //版權銷售申請單 授權權利 設定
                model.MEDIA_SALE_AUTHS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_AUTHS_CONFIG;
                //版權銷售申請單 額外項目 設定
                model.MEDIA_SALE_EXS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_EXS_CONFIG;
                //版權銷售申請單 付款辦法 設定
                model.MEDIA_SALE_COLLS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_COLLS_CONFIG;
                //版權銷售申請單 使用預算 設定
                model.MEDIA_SALE_BUDGS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_BUDGS_CONFIG;
                //版權銷售申請單 驗收項目 設定
                model.MEDIA_SALE_DELYS_CONFIG = postMediaSaleGroupSingle.MEDIA_SALE_DELYS_CONFIG;
                //表單關聯
                model.ASSOCIATED_FORM_CONFIG = postMediaSaleGroupSingle.ASSOCIATED_FORM_CONFIG;

                #region 寫回【子表單】不可被覆蓋內容

                //版權銷售申請 表頭資訊: 避免 子表單的 表單操作 被原單覆蓋
                model.MEDIA_SALE_TITLE.FORM_ACTION = ChildFormAction;
                //版權銷售申請 設定:避免 子表單的 不可異動標住 被原單覆蓋
                model.MEDIA_SALE_CONFIG.COLL_LOCK_PERIOD = ChildPYMT_LockPeriod;

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(model);
                CommLib.Logger.Debug("版權銷售申請單(原表單匯入子表單)Json：" + strJson);

                #region 執行 版權銷售申請單 原表單匯入子表單(新增/修改/草稿)

                //執行 版權銷售申請單 原表單匯入子表單(新增/修改/草稿)
                if (!PutMediaSaleSingle(model))
                {
                    CommLib.Logger.Error("版權銷售申請單 原表單匯入子表單(新增/修改/草稿)失敗。");
                    throw new Exception("版權銷售申請單 原表單匯入子表單(新增/修改/草稿)失敗。");
                }

                #endregion

                return model;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權銷售申請單(原表單匯入子表單)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaSale";

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