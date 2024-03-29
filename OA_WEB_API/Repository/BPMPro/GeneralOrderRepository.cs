﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

using Newtonsoft.Json;
using System.Drawing;
using System.Collections;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 行政採購申請單
    /// </summary>
    public class GeneralOrderRepository
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
        /// 行政採購申請單(查詢)
        /// </summary>
        public GeneralOrderViewModel PostGeneralOrderSingle(GeneralOrderQueryModel query)
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

            #region - 行政採購申請單 表頭資訊 -

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var generalOrderTitle = dbFun.DoQuery(strSQL, parameter).ToList<GeneralOrderTitle>().FirstOrDefault();

            #endregion

            #region - 行政採購申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [IsVicePresident] AS [IS_VICE_PRESIDENT], ";
            strSQL += "     [IsAssest] AS [IS_ASSEST], ";
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
            strSQL += "     [DTL_GrossTotal] AS [DTL_GROSS_TOTAL], ";
            strSQL += "     [DTL_GrossTotal_TWD] AS [DTL_GROSS_TOTAL_TWD], ";
            strSQL += "     [DiscountPrice] AS [DISCOUNT_PRICE], ";
            strSQL += "     [DTL_OrderTotal] AS [DTL_ORDER_TOTAL], ";
            strSQL += "     [DTL_OrderTotal_TWD] AS [DTL_ORDER_TOTAL_TWD], ";
            strSQL += "     [PYMT_LockPeriod] AS [PYMT_LOCK_PERIOD], ";
            strSQL += "     [PYMT_TaxTotal] AS [PYMT_TAX_TOTAL], ";
            strSQL += "     [PYMT_NetTotal] AS [PYMT_NET_TOTAL], ";
            strSQL += "     [PYMT_GrossTotal] AS [PYMT_GROSS_TOTAL], ";
            strSQL += "     [PYMT_GrossTotal_CONV] AS [PYMT_GROSS_TOTAL_CONV], ";
            strSQL += "     [PYMT_UseBudgetTotal] AS [PYMT_USE_BUDGET_TOTAL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var generalOrderConfig = dbFun.DoQuery(strSQL, parameter).ToList<GeneralOrderConfig>().FirstOrDefault();

            #endregion

            #region - 行政採購申請單 採購明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [Model] AS [MODEL], ";
            strSQL += "     [Specifications] AS [SPECIFICATIONS], ";
            strSQL += "     [Quantity] AS [QUANTITY], ";
            strSQL += "     [Unit] AS [UNIT], ";
            strSQL += "     [Net] AS [NET], ";
            strSQL += "     [Net_TWD] AS [NET_TWD], ";
            strSQL += "     [Gross] AS [GROSS], ";
            strSQL += "     [Gross_TWD] AS [GROSS_TWD], ";
            strSQL += "     [NetSum] AS [NET_SUM], ";
            strSQL += "     [NetSum_TWD] AS [NET_SUM_TWD], ";
            strSQL += "     [GrossSum] AS [GROSS_SUM], ";
            strSQL += "     [GrossSum_TWD] AS [GROSS_SUM_TWD], ";
            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var generalOrderDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<GeneralOrderDetailsConfig>();

            #endregion

            #region - 行政採購申請單 付款辦法 -

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
            strSQL += "     [Gross_CONV] AS [GROSS_CONV], ";
            strSQL += "     [UseBudget] AS [USE_BUDGET] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_PYMT] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var generalOrderPaymentsConfig = dbFun.DoQuery(strSQL, parameter).ToList<GeneralOrderPaymentsConfig>();

            #endregion

            #region - 行政採購申請單 使用預算 -

            var CommonBUDG = new BPMCommonModel<GeneralOrderBudgetsConfig>()
            {
                EXT = "BUDG",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostBudgetFunction(CommonBUDG));
            var generalOrderBudgetsConfig = jsonFunction.JsonToObject<List<GeneralOrderBudgetsConfig>>(strJson);


            #endregion

            #region - 行政採購申請單 驗收項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [Model] AS [MODEL], ";
            strSQL += "     [Specifications] AS [SPECIFICATIONS], ";
            strSQL += "     [Quantity] AS [QUANTITY], ";
            strSQL += "     [Unit] AS [UNIT], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_ACPT] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var generalOrderProjectsConfig = dbFun.DoQuery(strSQL, parameter).ToList<GeneralOrderAcceptancesConfig>();

            #endregion

            #region - 行政採購申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var generalOrderViewModel = new GeneralOrderViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                GENERAL_ORDER_TITLE = generalOrderTitle,
                GENERAL_ORDER_CONFIG = generalOrderConfig,
                GENERAL_ORDER_DETAILS_CONFIG = generalOrderDetailsConfig,
                GENERAL_ORDER_PAYMENTS_CONFIG = generalOrderPaymentsConfig,
                GENERAL_ORDER_BUDGETS_CONFIG = generalOrderBudgetsConfig,
                GENERAL_ORDER_ACPT_CONFIG = generalOrderProjectsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認表單 -

            if (generalOrderViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    generalOrderViewModel = new GeneralOrderViewModel();
                    CommLib.Logger.Error("行政採購申請單(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(generalOrderViewModel.GENERAL_ORDER_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(generalOrderViewModel.GENERAL_ORDER_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) generalOrderViewModel.GENERAL_ORDER_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            #region 確認是否匯入【子表單】

            if (generalOrderTitle != null)
            {
                if (!String.IsNullOrEmpty(generalOrderTitle.GROUP_ID) || !String.IsNullOrWhiteSpace(generalOrderTitle.GROUP_ID))
                {
                    try
                    {
                        #region 判斷 編輯註記及匯入【子表單】

                        //是空null就給false預設值
                        generalOrderTitle.EDIT_FLAG = generalOrderTitle.EDIT_FLAG ?? "false";

                        if (!Boolean.Parse(generalOrderTitle.EDIT_FLAG))
                        {
                            //匯入【子表單】
                            generalOrderViewModel = PutGeneralOrderImportSingle(generalOrderViewModel);
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("行政採購申請單(原表單匯入子表單)失敗；請確認原表單資訊是否正確。原因：" + ex.Message);
                    }

                }
            }

            #endregion

            return generalOrderViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 行政採購申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutGeneralOrderRefill(GeneralOrderQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("行政採購申請單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 行政採購申請單(新增/修改/草稿)
        /// </summary>
        public bool PutGeneralOrderSingle(GeneralOrderViewModel model)
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

                if (String.IsNullOrEmpty(model.GENERAL_ORDER_TITLE.GROUP_ID) || String.IsNullOrWhiteSpace(model.GENERAL_ORDER_TITLE.GROUP_ID))
                {
                    model.GENERAL_ORDER_TITLE.FORM_ACTION = "申請";
                }

                #region - 主旨 -

                if (String.IsNullOrEmpty(model.GENERAL_ORDER_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.GENERAL_ORDER_TITLE.FM7_SUBJECT))
                {
                    // 單號由流程事件做寫入
                    FM7Subject = "(待填寫)" + model.GENERAL_ORDER_TITLE.FLOW_NAME + "  ";
                }
                else
                {
                    FM7Subject = model.GENERAL_ORDER_TITLE.FM7_SUBJECT;

                    if (!String.IsNullOrEmpty(model.GENERAL_ORDER_TITLE.GROUP_ID) || !String.IsNullOrWhiteSpace(model.GENERAL_ORDER_TITLE.GROUP_ID))
                    {
                        if (FM7Subject.Substring(1, 2) != "異動")
                        {
                            FM7Subject = "【異動】" + FM7Subject;
                        }
                    }

                    #region 零稅率

                    if (model.GENERAL_ORDER_CONFIG != null)
                    {
                        if (model.GENERAL_ORDER_CONFIG.CURRENCY == "台幣" && model.GENERAL_ORDER_CONFIG.TAX_RATE == 0.0)
                        {
                            FM7Subject += "  (零稅率)";
                        }
                    }

                    #endregion
                }

                #endregion

                //編輯註記
                model.GENERAL_ORDER_TITLE.EDIT_FLAG = "true";

                #endregion

                #region - 行政採購申請 表頭資訊：GeneralOrder_M -

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
                    //行政採購申請 表頭
                    new SqlParameter("@EDIT_FLAG", SqlDbType.NVarChar) { Size = 5, Value = (object)model.GENERAL_ORDER_TITLE.EDIT_FLAG ?? DBNull.Value },
                    new SqlParameter("@GROUP_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_ORDER_TITLE.GROUP_ID ?? DBNull.Value },
                    new SqlParameter("@GROUP_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_ORDER_TITLE.GROUP_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@GROUP_PATH", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_ORDER_TITLE.GROUP_PATH ?? DBNull.Value },
                    new SqlParameter("@PARENT_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_ORDER_TITLE.PARENT_ID ?? DBNull.Value },
                    new SqlParameter("@PARENT_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.GENERAL_ORDER_TITLE.PARENT_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FORM_ACTION", SqlDbType.NVarChar) { Size = 64, Value = model.GENERAL_ORDER_TITLE.FORM_ACTION ?? String.Empty },
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_ORDER_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.GENERAL_ORDER_TITLE.FORM_NO ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GeneralOrder_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[EditFlag],[GroupID],[GroupBPMFormNo],[GroupPath],[ParentID],[ParentBPMFormNo],[FormAction],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@EDIT_FLAG,@GROUP_ID,@GROUP_BPM_FORM_NO,@GROUP_PATH,@PARENT_ID,@PARENT_BPM_FORM_NO,@FORM_ACTION,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 行政採購申請 表單內容：GeneralOrder_M -

                if (model.GENERAL_ORDER_CONFIG != null)
                {
                    #region - 確認小數點後第二位 -

                    model.GENERAL_ORDER_CONFIG.PRE_RATE = Math.Round(model.GENERAL_ORDER_CONFIG.PRE_RATE, 2);
                    model.GENERAL_ORDER_CONFIG.TAX_RATE = Math.Round(model.GENERAL_ORDER_CONFIG.TAX_RATE, 2);
                    model.GENERAL_ORDER_CONFIG.DTL_NET_TOTAL = Math.Round(model.GENERAL_ORDER_CONFIG.DTL_NET_TOTAL, 2);
                    model.GENERAL_ORDER_CONFIG.DTL_GROSS_TOTAL = Math.Round(model.GENERAL_ORDER_CONFIG.DTL_GROSS_TOTAL, 2);
                    model.GENERAL_ORDER_CONFIG.PYMT_TAX_TOTAL = Math.Round(model.GENERAL_ORDER_CONFIG.PYMT_TAX_TOTAL, 2);
                    model.GENERAL_ORDER_CONFIG.PYMT_NET_TOTAL = Math.Round(model.GENERAL_ORDER_CONFIG.PYMT_NET_TOTAL, 2);
                    model.GENERAL_ORDER_CONFIG.PYMT_GROSS_TOTAL = Math.Round(model.GENERAL_ORDER_CONFIG.PYMT_GROSS_TOTAL, 2);

                    #endregion

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //行政採購申請 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_VICE_PRESIDENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_ASSEST", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TXN_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRE_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRICING_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TAX_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64,  Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_TEL", SqlDbType.NVarChar) { Size = 20,  Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAYMENT_PERIOD_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_NET_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_GROSS_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DISCOUNT_PRICE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DTL_ORDER_TOTAL_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_LOCK_PERIOD", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_TAX_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_NET_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_GROSS_TOTAL", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_GROSS_TOTAL_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PYMT_USE_BUDGET_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：行政採購申請 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.GENERAL_ORDER_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_GeneralOrder_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [IsVicePresident]=@IS_VICE_PRESIDENT, ";
                    strSQL += "     [IsAssest]=@IS_ASSEST, ";
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
                    strSQL += "     [DTL_GrossTotal]=@DTL_GROSS_TOTAL, ";
                    strSQL += "     [DTL_GrossTotal_TWD]=@DTL_GROSS_TOTAL_TWD, ";
                    strSQL += "     [DiscountPrice]=@DISCOUNT_PRICE, ";
                    strSQL += "     [DTL_OrderTotal]=@DTL_ORDER_TOTAL, ";
                    strSQL += "     [DTL_OrderTotal_TWD]=@DTL_ORDER_TOTAL_TWD, ";
                    strSQL += "     [PYMT_LockPeriod]=@PYMT_LOCK_PERIOD, ";
                    strSQL += "     [PYMT_TaxTotal]=@PYMT_TAX_TOTAL, ";
                    strSQL += "     [PYMT_NetTotal]=@PYMT_NET_TOTAL, ";
                    strSQL += "     [PYMT_GrossTotal]=@PYMT_GROSS_TOTAL, ";
                    strSQL += "     [PYMT_GrossTotal_CONV]=@PYMT_GROSS_TOTAL_CONV, ";
                    strSQL += "     [PYMT_UseBudgetTotal]=@PYMT_USE_BUDGET_TOTAL ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 行政採購申請 採購明細：GeneralOrder_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //行政採購申請 採購明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MODEL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SPECIFICATIONS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@UNIT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET_SUM_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_SUM", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_SUM_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.GENERAL_ORDER_DETAILS_CONFIG != null && model.GENERAL_ORDER_DETAILS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.GENERAL_ORDER_DETAILS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        //寫入：行政採購申請 採購明細parameter
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GeneralOrder_DTL]([RequisitionID],[SupProdANo],[OrderRowNo],[ItemName],[Model],[Specifications],[Quantity],[Unit],[Net],[Net_TWD],[Gross],[Gross_TWD],[NetSum],[NetSum_TWD],[GrossSum],[GrossSum_TWD],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@SUP_PROD_A_NO,@ORDER_ROW_NO,@ITEM_NAME,@MODEL,@SPECIFICATIONS,@QUANTITY,@UNIT,@NET,@NET_TWD,@GROSS,@GROSS_TWD,@NET_SUM,@NET_SUM_TWD,@GROSS_SUM,@GROSS_SUM_TWD,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 行政採購申請 付款辦法：GeneralOrder_PYMT -

                var parameterPayments = new List<SqlParameter>()
                {
                    //行政採購申請 付款辦法
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TERMS", SqlDbType.NVarChar) { Size = 25, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@METHOD_ID", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAX", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NET", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PRE_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROSS_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USE_BUDGET", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_PYMT] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterPayments);

                #endregion


                if (model.GENERAL_ORDER_PAYMENTS_CONFIG != null && model.GENERAL_ORDER_PAYMENTS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.GENERAL_ORDER_PAYMENTS_CONFIG)
                    {
                        strJson = jsonFunction.ObjectToJSON(item);

                        //寫入：行政採購申請 付款辦法parameter

                        GlobalParameters.Infoparameter(strJson, parameterPayments);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GeneralOrder_PYMT]([RequisitionID],[Period],[Project],[Terms],[MethodID],[Tax],[Net],[Gross],[PredictRate],[Gross_CONV],[UseBudget]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@PROJECT,@TERMS,@METHOD_ID,@TAX,@NET,@GROSS,@PRE_RATE,@GROSS_CONV,@USE_BUDGET) ";

                        dbFun.DoTran(strSQL, parameterPayments);
                    }

                    #endregion
                }

                #endregion

                #region - 行政採購申請 使用預算：GeneralOrder_BUDG -

                var parameterBudgets = new List<SqlParameter>()
                {
                    //行政採購申請 使用預算
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CREATE_YEAR", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEPT", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AVAILABLE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.GENERAL_ORDER_BUDGETS_CONFIG != null && model.GENERAL_ORDER_BUDGETS_CONFIG.Count > 0)
                {
                    var CommonBUDG = new BPMCommonModel<GeneralOrderBudgetsConfig>()
                    {
                        EXT = "BUDG",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterBudgets,
                        MODEL = model.GENERAL_ORDER_BUDGETS_CONFIG
                    };
                    commonRepository.PutBudgetFunction(CommonBUDG);
                }

                #endregion

                #region - 行政採購申請 驗收項目：GeneralOrder_ACPT -

                var parameterAcceptance = new List<SqlParameter>()
                {
                    //行政採購申請 驗收項目
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MODEL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@SPECIFICATIONS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@UNIT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_GeneralOrder_ACPT] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterAcceptance);

                #endregion

                if (model.GENERAL_ORDER_ACPT_CONFIG != null && model.GENERAL_ORDER_ACPT_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.GENERAL_ORDER_ACPT_CONFIG)
                    {
                        //寫入：行政採購申請 驗收項目parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterAcceptance);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_GeneralOrder_ACPT]([RequisitionID],[Period],[SupProdANo],[OrderRowNo],[ItemName],[Model],[Specifications],[Quantity],[Unit],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@SUP_PROD_A_NO,@ORDER_ROW_NO,@ITEM_NAME,@MODEL,@SPECIFICATIONS,@QUANTITY,@UNIT,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterAcceptance);
                    }

                    #endregion
                }

                #endregion

                #region - 行政採購申請 表單關聯：AssociatedForm -

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
                CommLib.Logger.Error("行政採購申請單(新增/修改/草稿)失敗，原因：" + ex.Message);

            }

            return vResult;
        }

        /// <summary>
        /// 行政採購申請單(原表單匯入子表單)
        /// </summary>
        public GeneralOrderViewModel PutGeneralOrderImportSingle(GeneralOrderViewModel model)
        {
            try
            {
                #region 原表單_行政採購申請單(查詢)

                var groupQuery = new GeneralOrderQueryModel()
                {
                    REQUISITION_ID = model.GENERAL_ORDER_TITLE.GROUP_ID
                };
                //原表單_行政採購申請單(查詢)
                var postGeneralOrderGroupSingle = PostGeneralOrderSingle(groupQuery);

                #endregion

                #region 組裝要匯入寫回【子表單】的內容

                #region 保留【子表單】所需欄位

                var ChildFormAction = model.GENERAL_ORDER_TITLE.FORM_ACTION;
                var ChildPYMT_LockPeriod = model.GENERAL_ORDER_CONFIG.PYMT_LOCK_PERIOD;

                #endregion

                //行政採購申請 表頭資訊:ERP 工作流程標題名稱
                model.GENERAL_ORDER_TITLE.FLOW_NAME = postGeneralOrderGroupSingle.GENERAL_ORDER_TITLE.FLOW_NAME;
                //行政採購申請 設定
                model.GENERAL_ORDER_CONFIG = postGeneralOrderGroupSingle.GENERAL_ORDER_CONFIG;
                //行政採購申請 採購明細 設定
                model.GENERAL_ORDER_DETAILS_CONFIG = postGeneralOrderGroupSingle.GENERAL_ORDER_DETAILS_CONFIG;
                //行政採購申請 付款辦法 設定
                model.GENERAL_ORDER_PAYMENTS_CONFIG = postGeneralOrderGroupSingle.GENERAL_ORDER_PAYMENTS_CONFIG;
                //行政採購申請 使用預算 設定
                model.GENERAL_ORDER_BUDGETS_CONFIG = postGeneralOrderGroupSingle.GENERAL_ORDER_BUDGETS_CONFIG;
                //行政採購申請 驗收項目 設定
                model.GENERAL_ORDER_ACPT_CONFIG = postGeneralOrderGroupSingle.GENERAL_ORDER_ACPT_CONFIG;
                //表單關聯
                model.ASSOCIATED_FORM_CONFIG = postGeneralOrderGroupSingle.ASSOCIATED_FORM_CONFIG;

                #region 寫回【子表單】不可被覆蓋內容

                //行政採購申請 表頭資訊: 避免 子表單的 表單操作 被原單覆蓋
                model.GENERAL_ORDER_TITLE.FORM_ACTION = ChildFormAction;
                //行政採購申請 設定:避免 子表單的 不可異動標住 被原單覆蓋
                model.GENERAL_ORDER_CONFIG.PYMT_LOCK_PERIOD = ChildPYMT_LockPeriod;

                #endregion

                #endregion
                strJson = jsonFunction.ObjectToJSON(model);
                CommLib.Logger.Debug("行政採購申請單(原表單匯入子表單)Json：" + strJson);

                #region 執行 行政採購申請單 原表單匯入子表單(新增/修改/草稿)

                //執行 行政採購申請單 原表單匯入子表單(新增/修改/草稿)
                if (!PutGeneralOrderSingle(model))
                {
                    CommLib.Logger.Error("行政採購申請單 原表單匯入子表單(新增/修改/草稿)失敗。");
                    throw new Exception("行政採購申請單 原表單匯入子表單(新增/修改/草稿)失敗。");
                }

                #endregion

                return model;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購申請單(原表單匯入子表單)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "GeneralOrder";

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