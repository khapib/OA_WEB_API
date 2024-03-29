﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;
using System.Collections;
using OA_WEB_API.Models;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 預支費用申請單
    /// </summary>
    public class AdvanceExpenseRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 預支費用申請單(查詢)
        /// </summary>
        public AdvanceExpenseViewModel PostAdvanceExpenseSingle(AdvanceExpenseQueryModel query)
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

            #region - 預支費用申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var advanceExpenseTitle = dbFun.DoQuery(strSQL, parameter).ToList<AdvanceExpenseTitle>().FirstOrDefault();

            #endregion

            #region - 預支費用申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [IsCFO] AS [IS_CFO], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [Amount_CONV_Total] AS [AMOUNT_CONV_TOTAL], ";
            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var advanceExpenseConfig = dbFun.DoQuery(strSQL, parameter).ToList<AdvanceExpenseConfig>().FirstOrDefault();

            #endregion

            #region - 預支費用申請單 預知明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [AdvanceCurrencyName] AS [ADVANCE_CURRENCY_NAME], ";
            strSQL += "     [ExchangeRate] AS [EXCH_RATE], ";
            strSQL += "     [AdvanceAmount] AS [ADVANCE_AMOUNT], ";
            strSQL += "     [Amount_CONV] AS [AMOUNT_CONV], ";
            strSQL += "     [AdvanceDate] AS [ADVANCE_DATE], ";
            strSQL += "     [RepaymentDate] AS [REPAYMENT_DATE], ";
            strSQL += "     [RepaymentType] AS [REPAYMENT_TYPE], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [PayMethod] AS [PAY_METHOD], ";
            strSQL += "     [AccountCategory] AS [ACCOUNT_CATEGORY], ";
            strSQL += "     [PaymentObject] AS [PAYMENT_OBJECT], ";
            strSQL += "     [TX_Category] AS [TX_CATEGORY], ";
            strSQL += "     [BFCY_AccountNo] AS [BFCY_ACCOUNT_NO], ";
            strSQL += "     [BFCY_AccountName] AS [BFCY_ACCOUNT_NAME], ";
            strSQL += "     [BFCY_BankNo] AS [BFCY_BANK_NO], ";
            strSQL += "     [BFCY_BankName] AS [BFCY_BANK_NAME], ";
            strSQL += "     [CurrencyName] AS [CURRENCY_NAME], ";
            strSQL += "     [BFCY_Name] AS [BFCY_NAME], ";
            strSQL += "     [BFCY_TEL] AS [BFCY_TEL], ";
            strSQL += "     [BFCY_Email] AS [BFCY_EMAIL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var advanceExpenseDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<AdvanceExpenseDetailsConfig>();

            #endregion

            #region - 預支費用申請單 小計 -

            var CommonSUM = new BPMCommonModel<AdvanceExpenseSumsConfig>()
            {
                EXT = "SUM",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostFinanceFieldFunction(CommonSUM));
            var advanceExpenseSumsConfig = jsonFunction.JsonToObject<List<AdvanceExpenseSumsConfig>>(strJson);

            #endregion

            #region - 預支費用申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var advanceExpenseViewModel = new AdvanceExpenseViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                ADVANCE_EXPENSE_TITLE = advanceExpenseTitle,
                ADVANCE_EXPENSE_CONFIG = advanceExpenseConfig,
                ADVANCE_EXPENSE_DTLS_CONFIG = advanceExpenseDetailsConfig,
                ADVANCE_EXPENSE_SUMS_CONFIG=advanceExpenseSumsConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認表單 -

            if (advanceExpenseViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    advanceExpenseViewModel = new AdvanceExpenseViewModel();
                    CommLib.Logger.Error("預支費用申請單(查詢)失敗，原因：系統無正常起單。");
                }
                else
                {
                    #region - 確認M表BPM表單單號 -

                    //避免儲存後送出表單BPM表單單號沒寫入的情形
                    notifyRepository.ByInsertBPMFormNo(formQuery);

                    if (String.IsNullOrEmpty(advanceExpenseViewModel.ADVANCE_EXPENSE_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(advanceExpenseViewModel.ADVANCE_EXPENSE_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) advanceExpenseViewModel.ADVANCE_EXPENSE_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return advanceExpenseViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 預支費用申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutAdvanceExpenseRefill(AdvanceExpenseeQueryModel query)
        //{
        //    bool vResult = false;

        //    try
        //    {
        //        #region - 宣告 -

        //        var original = PostAdvanceExpenseSingle(query);
        //        strJson = jsonFunction.ObjectToJSON(original);

        //        var advanceExpenseViewModel = new AdvanceExpenseViewModel();

        //        var requisitionID = Guid.NewGuid().ToString();

        //        #endregion

        //        #region - 重送內容 -

        //        advanceExpenseViewModel = jsonFunction.JsonToObject<AdvanceExpenseViewModel>(strJson);

        //        #region - 申請人資訊 調整 -

        //        advanceExpenseViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
        //        advanceExpenseViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
        //        advanceExpenseViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

        //        #endregion

        //        #endregion

        //        #region - 送出 執行(新增/修改/草稿) -

        //        PutExpensesReimburseSingle(expensesReimburseViewModel);

        //        #endregion

        //        vResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("預支費用申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 預支費用申請單(新增/修改/草稿)
        /// </summary>
        public bool PutAdvanceExpenseSingle(AdvanceExpenseViewModel model)
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

                #region - 主旨 -

                if (String.IsNullOrEmpty(model.ADVANCE_EXPENSE_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.ADVANCE_EXPENSE_TITLE.FM7_SUBJECT))
                {
                    // 單號由流程事件做寫入
                    FM7Subject = "(待填寫)" + model.ADVANCE_EXPENSE_TITLE.FLOW_NAME + "_預知費用申請。";
                }
                else
                {
                    FM7Subject = model.ADVANCE_EXPENSE_TITLE.FM7_SUBJECT;
                }

                #endregion

                #endregion

                #region - 預支費用申請單 表頭資訊：AdvanceExpense_M -

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
                    //預支費用申請單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.ADVANCE_EXPENSE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.ADVANCE_EXPENSE_TITLE.FORM_NO ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 預支費用申請單 表單內容：AdvanceExpense_M -

                if (model.ADVANCE_EXPENSE_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //預支費用申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@IS_CFO", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000 , Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AMOUNT_CONV_TOTAL", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    var queryFinanc = new UserQueryModel()
                    {
                        COMPANY_ID = "RootCompany",
                        DEPT_FLOW = "財務部"
                    };
                    var FinancUser = userRepository.PostUsers(queryFinanc);

                    if (!String.IsNullOrEmpty(model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_ID_1) || !String.IsNullOrWhiteSpace(model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_ID_1))
                        model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_NAME_1 = FinancUser.Where(U => U.USER_ID == model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_ID_1).Select(U => U.USER_NAME).FirstOrDefault();

                    if (!String.IsNullOrEmpty(model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_ID_2) || !String.IsNullOrWhiteSpace(model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_ID_2))
                        model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_NAME_2 = FinancUser.Where(U => U.USER_ID == model.ADVANCE_EXPENSE_CONFIG.FINANC_AUDIT_ID_2).Select(U => U.USER_NAME).FirstOrDefault();


                    //寫入：預支費用申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.ADVANCE_EXPENSE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [IsCFO]=@IS_CFO, ";
                    strSQL += "     [Reason]=@REASON, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [Amount_CONV_Total]=@AMOUNT_CONV_TOTAL, ";
                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2 ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 預支費用申請單 預知明細: AdvanceExpense_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //預支費用申請單 預知明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ADVANCE_CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EXCH_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ADVANCE_AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ADVANCE_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REPAYMENT_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REPAYMENT_TYPE", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACCOUNT_CATEGORY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PAYMENT_OBJECT", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TX_CATEGORY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_ACCOUNT_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_ACCOUNT_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BANK_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BANK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_EMAIL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.ADVANCE_EXPENSE_DTLS_CONFIG != null && model.ADVANCE_EXPENSE_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.ADVANCE_EXPENSE_DTLS_CONFIG)
                    {
                        //寫入：預支費用申請單 預知明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].FM7T_" + IDENTIFY + "_DTL([RowNo],[RequisitionID],[AdvanceCurrencyName],[ExchangeRate],[AdvanceAmount],[Amount_CONV],[AdvanceDate],[RepaymentDate],[RepaymentType],[Note],[PayMethod],[AccountCategory],[PaymentObject],[TX_Category],[BFCY_AccountNo],[BFCY_AccountName],[BFCY_BankNo],[BFCY_BankName],[CurrencyName],[BFCY_Name],[BFCY_TEL],[BFCY_Email]) ";
                        strSQL += "VALUES(@ROW_NO,@REQUISITION_ID,@ADVANCE_CURRENCY_NAME,@EXCH_RATE,@ADVANCE_AMOUNT,@AMOUNT_CONV,@ADVANCE_DATE,@REPAYMENT_DATE,@REPAYMENT_TYPE,@NOTE,@PAY_METHOD,@ACCOUNT_CATEGORY,@PAYMENT_OBJECT,@TX_CATEGORY,@BFCY_ACCOUNT_NO,@BFCY_ACCOUNT_NAME,@BFCY_BANK_NO,@BFCY_BANK_NAME,@CURRENCY_NAME,@BFCY_NAME,@BFCY_TEL,@BFCY_EMAIL)";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                var parameterLatterHalf = new List<SqlParameter>()
                {
                    //預支費用申請單 小計
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@CURRENCY", SqlDbType.VarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value }
                };

                #region - 預支費用申請單 小計：AdvanceExpense_SUM -

                if (model.ADVANCE_EXPENSE_SUMS_CONFIG != null && model.ADVANCE_EXPENSE_SUMS_CONFIG.Count > 0)
                {
                    var CommonSUM = new BPMCommonModel<AdvanceExpenseSumsConfig>()
                    {
                        EXT = "SUM",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterLatterHalf,
                        MODEL = model.ADVANCE_EXPENSE_SUMS_CONFIG
                    };
                    vResult = commonRepository.PutFinanceFieldFunction(CommonSUM);
                }

                #endregion

                #region - 預支費用申請單 表單關聯：AssociatedForm -

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
                CommLib.Logger.Error("預支費用申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "AdvanceExpense";

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