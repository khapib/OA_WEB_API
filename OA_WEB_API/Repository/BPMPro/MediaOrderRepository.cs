using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;

using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購申請單
    /// </summary>
    public class MediaOrderRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購申請單(查詢)
        /// </summary>
        public MediaOrderViewModel PostMediaOrderSingle(MediaOrderQueryModel query)
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

            #region - 版權採購申請單 表頭資訊 -

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaOrderTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderTitle>().FirstOrDefault();

            #endregion

            #region - 版權採購申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [IsVicePresident] AS [IS_VICE_PRESIDENT], ";
            strSQL += "     [TXN_Type] AS [TXN_TYPE], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [PredictRate] AS [PRE_RATE], ";
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
            strSQL += "     [DiscountPrice] AS [DISCOUNT_PRICE], ";
            strSQL += "     [DTL_MaterialTotal] AS [DTL_MATERIAL_TOTAL], ";
            strSQL += "     [DTL_MaterialTotal_TWD] AS [DTL_MATERIAL_TOTAL_TWD], ";
            strSQL += "     [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
            strSQL += "     [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
            strSQL += "     [EX_AmountTotal] AS [EX_AMOUNT_TOTAL], ";
            strSQL += "     [EX_AmountTotal_TWD] AS [EX_AMOUNT_TOTAL_TWD], ";
            strSQL += "     [EX_TaxTotal] AS [EX_TAX_TOTAL], ";
            strSQL += "     [EX_TaxTotal_TWD] AS [EX_TAX_TOTAL_TWD], ";
            strSQL += "     [PYMT_LockPeriod] AS [PYMT_LOCK_PERIOD], ";
            strSQL += "     [PYMT_TaxTotal] AS [PYMT_TAX_TOTAL], ";
            strSQL += "     [PYMT_NetTotal] AS [PYMT_NET_TOTAL], ";
            strSQL += "     [PYMT_GrossTotal] AS [PYMT_GROSS_TOTAL], ";
            strSQL += "     [PYMT_MaterialTotal] AS [PYMT_MATERIAL_TOTAL], ";
            strSQL += "     [PYMT_OrderTotal] AS [PYMT_ORDER_TOTAL], ";
            strSQL += "     [PYMT_OrderTotal_CONV] AS [PYMT_ORDER_TOTAL_CONV], ";
            strSQL += "     [PYMT_UseBudgetTotal] AS [PYMT_USE_BUDGET_TOTAL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaOrderConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderConfig>().FirstOrDefault();

            #endregion

            #region - 版權採購申請單 採購明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaSpec] AS [MEDIA_SPEC], ";
            strSQL += "     [AUTH_All] AS [AUTH_ALL], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [OrderEpisode] AS [ORDER_EPISODE], ";
            strSQL += "     [ACPT_Episode] AS [ACPT_EPISODE], ";
            strSQL += "     [EpisodeTime] AS [EPISODE_TIME], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Net_TWD] AS [NET_TWD], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [Gross_TWD] AS [GROSS_TWD], ";
            strSQL += "     [NetSum] AS [NET_SUM], ";
            strSQL += "     [NetSum_TWD] AS [NET_SUM_TWD], ";
            strSQL += "     [GrossSum] AS [GROSS_SUM], ";
            strSQL += "     [GrossSum_TWD] AS [GROSS_SUM_TWD], ";
            strSQL += "     [Material] AS [MATERIAL], ";
            strSQL += "     [ItemSum] AS [ITEM_SUM], ";
            strSQL += "     [ItemSum_TWD] AS [ITEM_SUM_TWD], ";
            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderDetailsConfig>();

            #endregion

            #region - 版權採購申請單 授權權利 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_AUTH] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderAuthorizesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderAuthorizesConfig>();

            #endregion

            #region - 版權採購申請單 額外項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [Amount] AS [AMOUNT], ";
            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Tax_TWD] AS [TAX_TWD], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_EX] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderExtrasConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderExtrasConfig>();

            #endregion

            #region - 版權採購申請單 付款辦法 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [Project] AS [PROJECT], ";
            strSQL += "     [Terms] AS [TERMS], ";
            strSQL += "     [MethodID] AS [METHOD_ID], ";
            strSQL += "     [Tax] AS [TAX], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [PredictRate] AS [PRE_RATE], ";
            strSQL += "     [Material] AS [MATERIAL], ";
            strSQL += "     [OrderSum] AS [ORDER_SUM], ";
            strSQL += "     [OrderSum_CONV] AS [ORDER_SUM_CONV], ";
            strSQL += "     [UseBudget] AS [USE_BUDGET] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_PYMT] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderPaymentsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderPaymentsConfig>();

            #endregion

            #region - 版權採購申請單 使用預算 -

            var CommonBUDG = new BPMCommonModel<MediaOrderBudgetsConfig>()
            {
                EXT = "BUDG",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostBudgetFunction(CommonBUDG));
            var mediaOrderBudgetsConfig = jsonFunction.JsonToObject<List<MediaOrderBudgetsConfig>>(strJson);

            #endregion

            #region - 版權採購申請單 驗收項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [OrderEpisode] AS [ORDER_EPISODE], ";
            strSQL += "     [ACPT_Episode] AS [ACPT_EPISODE], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaOrderAcceptancesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderAcceptancesConfig>();

            #endregion

            #region - 版權採購申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var mediaOrderViewModel = new MediaOrderViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_ORDER_TITLE = mediaOrderTitle,
                MEDIA_ORDER_CONFIG = mediaOrderConfig,
                MEDIA_ORDER_DTLS_CONFIG = mediaOrderDetailsConfig,
                MEDIA_ORDER_AUTHS_CONFIG = mediaOrderAuthorizesConfig,
                MEDIA_ORDER_EXS_CONFIG = mediaOrderExtrasConfig,
                MEDIA_ORDER_PYMTS_CONFIG = mediaOrderPaymentsConfig,
                MEDIA_ORDER_BUDGS_CONFIG = mediaOrderBudgetsConfig,
                MEDIA_ORDER_ACPTS_CONFIG = mediaOrderAcceptancesConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認表單 -

            if (mediaOrderViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                if (!CommonRepository.GetFSe7enSysRequisition().Any(R => R.REQUISITION_ID == query.REQUISITION_ID))
                {
                    mediaOrderViewModel = new MediaOrderViewModel();
                    CommLib.Logger.Error("版權採購申請單(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(mediaOrderViewModel.MEDIA_ORDER_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(mediaOrderViewModel.MEDIA_ORDER_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) mediaOrderViewModel.MEDIA_ORDER_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            #region 確認是否匯入【子表單】

            if (mediaOrderTitle != null)
            {
                if (!String.IsNullOrEmpty(mediaOrderTitle.GROUP_ID) || !String.IsNullOrWhiteSpace(mediaOrderTitle.GROUP_ID))
                {
                    try
                    {
                        #region 判斷 編輯註記及匯入【子表單】

                        //是空null就給false預設值
                        mediaOrderTitle.EDIT_FLAG = mediaOrderTitle.EDIT_FLAG ?? "false";

                        if (!Boolean.Parse(mediaOrderTitle.EDIT_FLAG))
                        {
                            //匯入【子表單】
                            mediaOrderViewModel = PutMediaOrderImportSingle(mediaOrderViewModel);
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

            return mediaOrderViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaOrderRefill(MediaOrderQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權採購申請單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權採購申請單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaOrderSingle(MediaOrderViewModel model)
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

                if (String.IsNullOrEmpty(model.MEDIA_ORDER_TITLE.GROUP_ID) || String.IsNullOrWhiteSpace(model.MEDIA_ORDER_TITLE.GROUP_ID))
                {
                    model.MEDIA_ORDER_TITLE.FORM_ACTION = "申請";
                }

                #region - 主旨 -

                if (String.IsNullOrEmpty(model.MEDIA_ORDER_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.MEDIA_ORDER_TITLE.FM7_SUBJECT))
                {
                    // 單號由流程事件做寫入
                    FM7Subject = "(待填寫)" + model.MEDIA_ORDER_TITLE.FLOW_NAME + "  ";
                }
                else
                {
                    FM7Subject = model.MEDIA_ORDER_TITLE.FM7_SUBJECT;

                    if (!String.IsNullOrEmpty(model.MEDIA_ORDER_TITLE.GROUP_ID) || !String.IsNullOrWhiteSpace(model.MEDIA_ORDER_TITLE.GROUP_ID))
                    {
                        if (FM7Subject.Substring(1, 2) != "異動")
                        {
                            FM7Subject = "【異動】" + FM7Subject;
                        }
                    }

                    #region 零稅率

                    if (model.MEDIA_ORDER_CONFIG != null)
                    {
                        if (model.MEDIA_ORDER_CONFIG.CURRENCY == "台幣" && model.MEDIA_ORDER_CONFIG.TAX_RATE == 0.0)
                        {
                            FM7Subject += "  (零稅率)";
                        }
                    }

                    #endregion
                }

                #endregion

                //編輯註記
                model.MEDIA_ORDER_TITLE.EDIT_FLAG = "true";

                #endregion

                #region - 版權採購申請單 表頭資訊：MediaOrder_M -

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
                    //版權採購申請單 表頭
                    new SqlParameter("@EDIT_FLAG", SqlDbType.NVarChar) { Size = 5, Value = (object)model.MEDIA_ORDER_TITLE.EDIT_FLAG ?? DBNull.Value },
                    new SqlParameter("@GROUP_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_TITLE.GROUP_ID ?? DBNull.Value },
                    new SqlParameter("@GROUP_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_TITLE.GROUP_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@GROUP_PATH", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_TITLE.GROUP_PATH ?? DBNull.Value },
                    new SqlParameter("@PARENT_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_TITLE.PARENT_ID ?? DBNull.Value },
                    new SqlParameter("@PARENT_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_TITLE.PARENT_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FORM_ACTION", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_ORDER_TITLE.FORM_ACTION ?? String.Empty },
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_TITLE.FORM_NO ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[EditFlag],[GroupID],[GroupBPMFormNo],[GroupPath],[ParentID],[ParentBPMFormNo],[FormAction],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@EDIT_FLAG,@GROUP_ID,@GROUP_BPM_FORM_NO,@GROUP_PATH,@PARENT_ID,@PARENT_BPM_FORM_NO,@FORM_ACTION,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 版權採購申請單 表單內容：MediaOrder_M -

                if (model.MEDIA_ORDER_CONFIG != null)
                {
                    #region - 確認小數點後第二位 -

                    model.MEDIA_ORDER_CONFIG.PRE_RATE = Math.Round(model.MEDIA_ORDER_CONFIG.PRE_RATE, 2);
                    model.MEDIA_ORDER_CONFIG.TAX_RATE = Math.Round(model.MEDIA_ORDER_CONFIG.TAX_RATE, 2);
                    model.MEDIA_ORDER_CONFIG.DTL_NET_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.DTL_NET_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.DTL_TAX_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.DTL_TAX_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.DTL_GROSS_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.DTL_GROSS_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.DTL_MATERIAL_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.DTL_MATERIAL_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.DTL_ORDER_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.DTL_ORDER_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.EX_AMOUNT_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.EX_AMOUNT_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.EX_TAX_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.EX_TAX_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.PYMT_TAX_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.PYMT_TAX_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.PYMT_NET_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.PYMT_NET_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.PYMT_GROSS_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.PYMT_GROSS_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.PYMT_MATERIAL_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.PYMT_MATERIAL_TOTAL, 2);
                    model.MEDIA_ORDER_CONFIG.PYMT_ORDER_TOTAL = Math.Round(model.MEDIA_ORDER_CONFIG.PYMT_ORDER_TOTAL, 2);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權採購申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TXN_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_VICE_PRESIDENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRE_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
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
                        new SqlParameter("@DTL_MATERIAL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_MATERIAL_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_AMOUNT_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EX_TAX_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_LOCK_PERIOD", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_MATERIAL_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_ORDER_TOTAL_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_USE_BUDGET_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：版權採購申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_ORDER_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrder_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [IsVicePresident]=@IS_VICE_PRESIDENT, ";
                    strSQL += "     [TXN_Type]=@TXN_TYPE, ";
                    strSQL += "     [Currency]=@CURRENCY, ";
                    strSQL += "     [PredictRate]=@PRE_RATE, ";
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
                    strSQL += "     [DiscountPrice]=@DISCOUNT_PRICE, ";
                    strSQL += "     [DTL_MaterialTotal]=@DTL_MATERIAL_TOTAL, ";
                    strSQL += "     [DTL_MaterialTotal_TWD]=@DTL_MATERIAL_TOTAL_TWD, ";
                    strSQL += "     [DTL_OrderTotal]=@DTL_ORDER_TOTAL, ";
                    strSQL += "     [DTL_OrderTotal_TWD]=@DTL_ORDER_TOTAL_TWD, ";
                    strSQL += "     [EX_AmountTotal]=@EX_AMOUNT_TOTAL, ";
                    strSQL += "     [EX_AmountTotal_TWD]=@EX_AMOUNT_TOTAL_TWD, ";
                    strSQL += "     [EX_TaxTotal]=@EX_TAX_TOTAL, ";
                    strSQL += "     [EX_TaxTotal_TWD]=@EX_TAX_TOTAL_TWD, ";
                    strSQL += "     [PYMT_LockPeriod]=@PYMT_LOCK_PERIOD, ";
                    strSQL += "     [PYMT_TaxTotal]=@PYMT_TAX_TOTAL, ";
                    strSQL += "     [PYMT_NetTotal]=@PYMT_NET_TOTAL, ";
                    strSQL += "     [PYMT_GrossTotal]=@PYMT_GROSS_TOTAL, ";
                    strSQL += "     [PYMT_MaterialTotal]=@PYMT_MATERIAL_TOTAL, ";
                    strSQL += "     [PYMT_OrderTotal]=@PYMT_ORDER_TOTAL, ";
                    strSQL += "     [PYMT_OrderTotal_CONV]=@PYMT_ORDER_TOTAL_CONV, ";
                    strSQL += "     [PYMT_UseBudgetTotal]=@PYMT_USE_BUDGET_TOTAL ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權採購申請單 採購明細：MediaOrder_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //版權採購申請單 採購明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AUTH_ALL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_SUM_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_SUM_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MATERIAL", SqlDbType.Float) { Value =(object) DBNull.Value ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_ORDER_DTLS_CONFIG != null && model.MEDIA_ORDER_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_DTLS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購申請單 採購明細parameter
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_DTL]([RequisitionID],[OrderRowNo],[SupProdANo],[ItemName],[MediaSpec],[AUTH_All],[MediaType],[StartEpisode],[EndEpisode],[OrderEpisode],[ACPT_Episode],[EpisodeTime],[Net],[Net_TWD],[Tax],[Tax_TWD],[Gross],[Gross_TWD],[NetSum],[NetSum_TWD],[GrossSum],[GrossSum_TWD],[Material],[ItemSum],[ItemSum_TWD],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ORDER_ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_SPEC,@AUTH_ALL,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@ORDER_EPISODE,@ACPT_EPISODE,@EPISODE_TIME,@NET,@NET_TWD,@TAX,@TAX_TWD,@GROSS,@GROSS_TWD,@NET_SUM,@NET_SUM_TWD,@GROSS_SUM,@GROSS_SUM_TWD,@MATERIAL,@ITEM_SUM,@ITEM_SUM_TWD,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購申請單 授權權利：MediaOrder_AUTH -

                var parameterAuthorizes = new List<SqlParameter>()
                {
                    //版權採購申請單 授權權利
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_AUTH] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterAuthorizes);

                #endregion

                if (model.MEDIA_ORDER_AUTHS_CONFIG != null && model.MEDIA_ORDER_AUTHS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_AUTHS_CONFIG)
                    {

                        //寫入：版權採購申請單 授權權利parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterAuthorizes);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_AUTH]([RequisitionID],[OrderRowNo],[SupProdANo],[ItemName],[Continent],[Country],[PlayPlatform],[Play],[Sell],[EditToPlay],[EditToSell],[AllotedTimeType],[StartDate],[EndDate],[FrequencyType],[PlayFrequency],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ORDER_ROW_NO,@SUP_PROD_A_NO,@ITEM_NAME,@CONTINENT,@COUNTRY,@PLAY_PLATFORM,@PLAY,@SELL,@EDIT_TO_PLAY,@EDIT_TO_SELL,@ALLOTED_TIME_TYPE,@START_DATE,@END_DATE,@FREQUENCY_TYPE,@PLAY_FREQUENCY,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterAuthorizes);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購申請單 額外項目：MediaOrder_EX -

                var parameterExtras = new List<SqlParameter>()
                {
                    //版權採購申請單 額外項目
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_EX] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterAuthorizes);

                #endregion

                if (model.MEDIA_ORDER_EXS_CONFIG != null && model.MEDIA_ORDER_EXS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_EXS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購申請單 授權權利parameter
                        GlobalParameters.Infoparameter(strJson, parameterExtras);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_EX]([RequisitionID],[Name],[Amount],[Amount_TWD],[Tax],[Tax_TWD],[Period],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@NAME,@AMOUNT,@AMOUNT_TWD,@TAX,@TAX_TWD,@PERIOD,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterExtras);
                    }

                    #endregion

                }

                #endregion

                #region - 版權採購申請單 付款辦法：MediaOrder_PYMT -

                var parameterPayments = new List<SqlParameter>()
                {
                    //版權採購申請單 付款辦法
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TERMS", SqlDbType.NVarChar) { Size = 25, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@METHOD_ID", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PRE_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MATERIAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_SUM_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USE_BUDGET", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_PYMT] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterPayments);

                #endregion

                if (model.MEDIA_ORDER_PYMTS_CONFIG != null && model.MEDIA_ORDER_PYMTS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_PYMTS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        #region - 確認小數點後第二位 -

                        GlobalParameters.IsDouble(strJson);

                        #endregion

                        //寫入：版權採購申請單 付款辦法parameter

                        GlobalParameters.Infoparameter(strJson, parameterPayments);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_PYMT]([RequisitionID],[Period],[Project],[Terms],[MethodID],[Tax],[Net],[Gross],[PredictRate],[Material],[OrderSum],[OrderSum_CONV],[UseBudget]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@PROJECT,@TERMS,@METHOD_ID,@TAX,@NET,@GROSS,@PRE_RATE,@MATERIAL,@ORDER_SUM,@ORDER_SUM_CONV,@USE_BUDGET) ";

                        dbFun.DoTran(strSQL, parameterPayments);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購申請單 使用預算：MediaOrder_BUDG -

                var parameterBudgets = new List<SqlParameter>()
                {
                    //版權採購申請單 使用預算
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CREATE_YEAR", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEPT", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AVAILABLE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.MEDIA_ORDER_BUDGS_CONFIG != null && model.MEDIA_ORDER_BUDGS_CONFIG.Count > 0)
                {
                    var CommonBUDG = new BPMCommonModel<MediaOrderBudgetsConfig>()
                    {
                        EXT = "BUDG",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterBudgets,
                        MODEL = model.MEDIA_ORDER_BUDGS_CONFIG
                    };
                    commonRepository.PutBudgetFunction(CommonBUDG);
                }

                #endregion

                #region - 版權採購申請單 驗收項目：MediaOrder_ACPT -

                var parameterAcceptance = new List<SqlParameter>()
                {
                    //版權採購申請單 驗收項目
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_ACPT] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterAcceptance);

                #endregion

                if (model.MEDIA_ORDER_ACPTS_CONFIG != null && model.MEDIA_ORDER_ACPTS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ORDER_ACPTS_CONFIG)
                    {
                        //寫入：版權採購申請單 驗收項目parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterAcceptance);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrder_ACPT]([RequisitionID],[OrderRowNo],[Period],[SupProdANo],[ItemName],[MediaType],[StartEpisode],[EndEpisode],[OrderEpisode],[ACPT_Episode],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ORDER_ROW_NO,@PERIOD,@SUP_PROD_A_NO,@ITEM_NAME,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@ORDER_EPISODE,@ACPT_EPISODE,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterAcceptance);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購申請單 表單關聯：AssociatedForm -

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
                CommLib.Logger.Error("版權採購申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 版權採購申請單(原表單匯入子表單)
        /// </summary>
        public MediaOrderViewModel PutMediaOrderImportSingle(MediaOrderViewModel model)
        {
            try
            {
                #region 原表單_版權採購申請單(查詢)

                var groupQuery = new MediaOrderQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_ORDER_TITLE.GROUP_ID
                };
                //原表單_版權採購申請單(查詢)
                var postMediaOrderGroupSingle = PostMediaOrderSingle(groupQuery);

                #endregion

                #region 組裝要匯入寫回【子表單】的內容

                #region 保留【子表單】所需欄位

                var ChildFormAction = model.MEDIA_ORDER_TITLE.FORM_ACTION;
                var ChildPYMT_LockPeriod = model.MEDIA_ORDER_CONFIG.PYMT_LOCK_PERIOD;

                #endregion

                //版權採購申請單 表頭資訊:ERP 工作流程標題名稱
                model.MEDIA_ORDER_TITLE.FLOW_NAME = postMediaOrderGroupSingle.MEDIA_ORDER_TITLE.FLOW_NAME;
                //版權採購申請單 設定
                model.MEDIA_ORDER_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_CONFIG;
                //版權採購申請單 採購明細 設定
                model.MEDIA_ORDER_DTLS_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_DTLS_CONFIG;
                //版權採購申請單 授權權利 設定
                model.MEDIA_ORDER_AUTHS_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_AUTHS_CONFIG;
                //版權採購申請單 額外項目 設定
                model.MEDIA_ORDER_EXS_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_EXS_CONFIG;
                //版權採購申請單 付款辦法 設定
                model.MEDIA_ORDER_PYMTS_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_PYMTS_CONFIG;
                //版權採購申請單 使用預算 設定
                model.MEDIA_ORDER_BUDGS_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_BUDGS_CONFIG;
                //版權採購申請單 驗收項目 設定
                model.MEDIA_ORDER_ACPTS_CONFIG = postMediaOrderGroupSingle.MEDIA_ORDER_ACPTS_CONFIG;
                //表單關聯
                model.ASSOCIATED_FORM_CONFIG = postMediaOrderGroupSingle.ASSOCIATED_FORM_CONFIG;

                #region 寫回【子表單】不可被覆蓋內容

                //版權採購申請 表頭資訊: 避免 子表單的 表單操作 被原單覆蓋
                model.MEDIA_ORDER_TITLE.FORM_ACTION = ChildFormAction;
                //版權採購申請 設定:避免 子表單的 不可異動標住 被原單覆蓋
                model.MEDIA_ORDER_CONFIG.PYMT_LOCK_PERIOD = ChildPYMT_LockPeriod;

                #endregion

                #endregion

                strJson = jsonFunction.ObjectToJSON(model);
                CommLib.Logger.Debug("版權採購申請單(原表單匯入子表單)Json：" + strJson);

                #region 執行 版權採購申請單 原表單匯入子表單(新增/修改/草稿)

                //執行 版權採購申請單 原表單匯入子表單(新增/修改/草稿)
                if (!PutMediaOrderSingle(model))
                {
                    CommLib.Logger.Error("版權採購申請單 原表單匯入子表單(新增/修改/草稿)失敗。");
                    throw new Exception("版權採購申請單 原表單匯入子表單(新增/修改/草稿)失敗。");
                }

                #endregion

                return model;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購申請單(原表單匯入子表單)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaOrder";

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