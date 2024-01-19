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
    /// 會簽管理系統 - 費用申請單
    /// </summary>
    public class ExpensesReimburseRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 費用申請單(查詢)
        /// </summary>
        public ExpensesReimburseViewModel PostExpensesReimburseSingle(ExpensesReimburseQueryModel query)
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

            #region - 費用申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var expensesReimburseTitle = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseTitle>().FirstOrDefault();

            #endregion

            #region - 費用申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [IsCFO] AS [IS_CFO], ";
            strSQL += "     [IsVicePresident] AS [IS_VICE_PRESIDENT], ";
            strSQL += "     [REIMB_StaffDeptID] AS [REIMB_STAFF_DEPT_ID], ";
            strSQL += "     [REIMB_StaffDeptName] AS [REIMB_STAFF_DEPT_NAME], ";
            strSQL += "     [REIMB_StaffID] AS [REIMB_STAFF_ID], ";
            strSQL += "     [REIMB_StaffName] AS [REIMB_STAFF_NAME], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2], ";
            strSQL += "     [Amount_CONV_Total] AS [AMOUNT_CONV_TOTAL], ";
            strSQL += "     [PayMethod] AS [PAY_METHOD], ";
            strSQL += "     [AccountCategory] AS [ACCOUNT_CATEGORY], ";
            strSQL += "     [PaymentObject] AS [PAYMENT_OBJECT], ";
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var expensesReimburseConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseConfig>().FirstOrDefault();

            #endregion

            #region - 費用申請單 費用明細 -

            var CommonDTL = new BPMCommonModel<ExpensesReimburseDetailsConfig>()
            {
                EXT = "DTL",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostInvoiceFunction(CommonDTL));
            var expensesReimburseDetailsConfig = jsonFunction.JsonToObject<List<ExpensesReimburseDetailsConfig>>(strJson);

            #endregion

            #region - 費用申請單 使用預算 -

            var CommonBUDG = new BPMCommonModel<ExpensesReimburseBudgetsConfig>()
            {
                EXT = "BUDG",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostBudgetFunction(CommonBUDG));
            var expensesReimburseBudgetsConfig = jsonFunction.JsonToObject<List<ExpensesReimburseBudgetsConfig>>(strJson);

            #endregion

            #region - 費用申請單 小計 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [Amount] AS [AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_SUM] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var expensesReimburseSumsConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseSumsConfig>().ToList();

            #endregion

            #region - 費用申請單 已預支 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [Amount] AS [AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_ADV] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var expensesReimburseAdvancesConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseAdvancesConfig>().ToList();

            #endregion

            #region - 費用申請單 應退 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [Amount] AS [AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_AR] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var expensesReimburseRefundablesConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseRefundablesConfig>().ToList();

            #endregion

            #region - 費用申請單 應付 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Currency] AS [CURRENCY], ";
            strSQL += "     [Amount] AS [AMOUNT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_AP] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var expensesReimbursePayablesConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimbursePayablesConfig>().ToList();

            #endregion

            #region - 費用申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var expensesReimburseViewModel = new ExpensesReimburseViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                EXPENSES_REIMBURSE_TITLE = expensesReimburseTitle,
                EXPENSES_REIMBURSE_CONFIG = expensesReimburseConfig,
                EXPENSES_REIMBURSE_DTLS_CONFIG = expensesReimburseDetailsConfig,
                EXPENSES_REIMBURSE_BUDGS_CONFIG = expensesReimburseBudgetsConfig,
                EXPENSES_REIMBURSE_SUMS_CONFIG = expensesReimburseSumsConfig,
                EXPENSES_REIMBURSE_ADVS_CONFIG= expensesReimburseAdvancesConfig,
                EXPENSES_REIMBURSE_ARS_CONFIG=expensesReimburseRefundablesConfig,
                EXPENSES_REIMBURSE_APS_CONFIG=expensesReimbursePayablesConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            #region - 確認表單 -

            if (expensesReimburseViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    expensesReimburseViewModel = new ExpensesReimburseViewModel();
                    CommLib.Logger.Error("費用申請單(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(expensesReimburseViewModel.EXPENSES_REIMBURSE_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(expensesReimburseViewModel.EXPENSES_REIMBURSE_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) expensesReimburseViewModel.EXPENSES_REIMBURSE_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return expensesReimburseViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 費用申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutExpensesReimburseRefill(ExpensesReimburseQueryModel query)
        //{
        //    bool vResult = false;

        //    try
        //    {
        //        #region - 宣告 -

        //        var original = PostExpensesReimburseSingle(query);
        //        strJson = jsonFunction.ObjectToJSON(original);

        //        var expensesReimburseViewModel = new ExpensesReimburseViewModel();

        //        var requisitionID = Guid.NewGuid().ToString();

        //        #endregion

        //        #region - 重送內容 -

        //        expensesReimburseViewModel = jsonFunction.JsonToObject<ExpensesReimburseViewModel>(strJson);

        //        #region - 申請人資訊 調整 -

        //        expensesReimburseViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
        //        expensesReimburseViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
        //        expensesReimburseViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

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
        //        CommLib.Logger.Error("費用申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 費用申請單(新增/修改/草稿)
        /// </summary>
        public bool PutExpensesReimburseSingle(ExpensesReimburseViewModel model)
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

                if (String.IsNullOrEmpty(model.EXPENSES_REIMBURSE_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.EXPENSES_REIMBURSE_TITLE.FM7_SUBJECT))
                {
                    // 單號由流程事件做寫入
                    FM7Subject = "(待填寫)" + model.EXPENSES_REIMBURSE_TITLE.FLOW_NAME + "_費用申請。";
                }
                else
                {
                    FM7Subject = model.EXPENSES_REIMBURSE_TITLE.FM7_SUBJECT;
                }

                #endregion

                #endregion

                #region - 費用申請單 表頭資訊：ExpensesReimburse_M -

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
                    //費用申請單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.EXPENSES_REIMBURSE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.EXPENSES_REIMBURSE_TITLE.FORM_NO ?? DBNull.Value },
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

                #region - 費用申請單 表單內容：ExpensesReimburse_M -

                if (model.EXPENSES_REIMBURSE_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //費用申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@IS_CFO", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_VICE_PRESIDENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REIMB_STAFF_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000 , Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AMOUNT_CONV_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACCOUNT_CATEGORY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAYMENT_OBJECT", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TX_CATEGORY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_ACCOUNT_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_BRANCH_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_SWIFT", SqlDbType.NVarChar) { Size = 300, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_ADDRESS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_COUNTRY_AND_CITY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_BANK_IBAN", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BFCY_EMAIL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    if (String.IsNullOrEmpty(model.EXPENSES_REIMBURSE_CONFIG.IS_VICE_PRESIDENT) || String.IsNullOrWhiteSpace(model.EXPENSES_REIMBURSE_CONFIG.IS_VICE_PRESIDENT))
                    {
                        if (model.EXPENSES_REIMBURSE_CONFIG.AMOUNT_CONV_TOTAL >= 30000) model.EXPENSES_REIMBURSE_CONFIG.IS_VICE_PRESIDENT = true.ToString().ToLower();
                        else model.EXPENSES_REIMBURSE_CONFIG.IS_VICE_PRESIDENT = false.ToString().ToLower();
                    }

                    //寫入：費用申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.EXPENSES_REIMBURSE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [IsCFO]=@IS_CFO, ";
                    strSQL += "     [IsVicePresident]=@IS_VICE_PRESIDENT, ";
                    strSQL += "     [REIMB_StaffDeptID]=@REIMB_STAFF_DEPT_ID, ";
                    strSQL += "     [REIMB_StaffDeptName]=@REIMB_STAFF_DEPT_NAME, ";
                    strSQL += "     [REIMB_StaffID]=@REIMB_STAFF_ID, ";
                    strSQL += "     [REIMB_StaffName]=@REIMB_STAFF_NAME, ";
                    strSQL += "     [Reason]=@REASON, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2, ";
                    strSQL += "     [Amount_CONV_Total]=@AMOUNT_CONV_TOTAL, ";
                    strSQL += "     [PayMethod]=@PAY_METHOD, ";
                    strSQL += "     [AccountCategory]=@ACCOUNT_CATEGORY,";
                    strSQL += "     [PaymentObject]=@PAYMENT_OBJECT,";
                    strSQL += "     [TX_Category]=@TX_CATEGORY, ";
                    strSQL += "     [BFCY_AccountNo]=@BFCY_ACCOUNT_NO, ";
                    strSQL += "     [BFCY_AccountName]=@BFCY_ACCOUNT_NAME, ";
                    strSQL += "     [BFCY_BankNo]=@BFCY_BANK_NO, ";
                    strSQL += "     [BFCY_BankName]=@BFCY_BANK_NAME, ";
                    strSQL += "     [BFCY_BanKBranchNo]=@BFCY_BANK_BRANCH_NO, ";
                    strSQL += "     [BFCY_BanKBranchName]=@BFCY_BANK_BRANCH_NAME, ";
                    strSQL += "     [BFCY_BankSWIFT]=@BFCY_BANK_SWIFT, ";
                    strSQL += "     [BFCY_BankAddress]=@BFCY_BANK_ADDRESS, ";
                    strSQL += "     [BFCY_BankCountryAndCity]=@BFCY_BANK_COUNTRY_AND_CITY, ";
                    strSQL += "     [BFCY_BankIBAN]=@BFCY_BANK_IBAN, ";
                    strSQL += "     [CurrencyName]=@CURRENCY_NAME, ";
                    strSQL += "     [BFCY_Name]=@BFCY_NAME, ";
                    strSQL += "     [BFCY_TEL]=@BFCY_TEL, ";
                    strSQL += "     [BFCY_Email]=@BFCY_EMAIL ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 費用申請單 費用明細: ExpensesReimburse_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //費用申請單 費用明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 100 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TYPE", SqlDbType.NVarChar) { Size = 100 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_TYPE", SqlDbType.NVarChar) { Size = 20 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EXCH_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AMOUNT_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10 , Value = (object)DBNull.Value ?? DBNull.Value },
                    //new SqlParameter("@ACCT_CATEGORY", SqlDbType.NVarChar) { Size = 10 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DATE", SqlDbType.NVarChar) { Size = 64 , Value = (object)DBNull.Value ?? DBNull.Value },
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
                    new SqlParameter("@NOTE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },

                };

                if (model.EXPENSES_REIMBURSE_DTLS_CONFIG != null && model.EXPENSES_REIMBURSE_DTLS_CONFIG.Count > 0)
                {
                    var CommonDTL = new BPMCommonModel<ExpensesReimburseDetailsConfig>()
                    {
                        EXT = "DTL",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterDetails,
                        MODEL = model.EXPENSES_REIMBURSE_DTLS_CONFIG
                    };
                    commonRepository.PutInvoiceFunction(CommonDTL);

                    model.EXPENSES_REIMBURSE_DTLS_CONFIG.ForEach(DTL =>
                    {
                        //寫入：費用申請單 費用明細parameter
                        strJson = jsonFunction.ObjectToJSON(DTL);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
                        strSQL += "SET [Name] =@NAME, ";
                        strSQL += "     [Type]=@TYPE, ";
                        strSQL += "     [InvoiceType]=@INV_TYPE, ";
                        strSQL += "     [ExchangeRate]=@EXCH_RATE, ";
                        strSQL += "     [Amount_CONV]=@AMOUNT_CONV, ";
                        strSQL += "     [Currency]=@CURRENCY, ";
                        //strSQL += "     [ACCT_Category]=@ACCT_CATEGORY, ";
                        strSQL += "     [ProjectFormNo]=@PROJECT_FORM_NO, ";
                        strSQL += "     [ProjectName]=@PROJECT_NAME, ";
                        strSQL += "     [ProjectNickname]=@PROJECT_NICKNAME, ";
                        strSQL += "     [ProjectUseYear]=@PROJECT_USE_YEAR, ";
                        strSQL += "     [Note]=@NOTE ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                        strSQL += "         AND [RowNo]=@ROW_NO ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    });
                }

                #endregion

                #region - 費用申請單 使用預算：ExpensesReimburse_BUDG -

                var parameterBudgets = new List<SqlParameter>()
                {
                    //費用申請單 使用預算
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CREATE_YEAR", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEPT", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@AVAILABLE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.EXPENSES_REIMBURSE_BUDGS_CONFIG != null && model.EXPENSES_REIMBURSE_BUDGS_CONFIG.Count > 0)
                {
                    var CommonBUDG = new BPMCommonModel<ExpensesReimburseBudgetsConfig>()
                    {
                        EXT = "BUDG",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterBudgets,
                        MODEL = model.EXPENSES_REIMBURSE_BUDGS_CONFIG
                    };
                    commonRepository.PutBudgetFunction(CommonBUDG);
                }

                #endregion

                var parameterLatterHalf = new List<SqlParameter>()
                {
                    //費用申請單 小計、已預支、應退、應付
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                };

                #region - 費用申請單 小計：ExpensesReimburse_SUM -

                if (model.EXPENSES_REIMBURSE_SUMS_CONFIG != null && model.EXPENSES_REIMBURSE_SUMS_CONFIG.Count > 0)
                {
                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_SUM] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterLatterHalf);

                    #endregion

                    model.EXPENSES_REIMBURSE_SUMS_CONFIG.ForEach(SUM =>
                    {
                        parameterLatterHalf.Add(new SqlParameter("@CURRENCY", SqlDbType.VarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value });
                        parameterLatterHalf.Add(new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value });

                        //寫入：費用申請單 小計parameter
                        strJson = jsonFunction.ObjectToJSON(SUM);
                        GlobalParameters.Infoparameter(strJson, parameterLatterHalf);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_SUM]([RequisitionID],[Currency],[Amount]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@CURRENCY,@AMOUNT) ";

                        dbFun.DoTran(strSQL, parameterLatterHalf);

                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@CURRENCY")).FirstOrDefault());
                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@AMOUNT")).FirstOrDefault());
                    });
                }

                #endregion

                #region - 費用申請單 已預支：ExpensesReimburse_ADV -

                if (model.EXPENSES_REIMBURSE_ADVS_CONFIG != null && model.EXPENSES_REIMBURSE_ADVS_CONFIG.Count > 0)
                {
                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_ADV] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterLatterHalf);

                    #endregion

                    model.EXPENSES_REIMBURSE_ADVS_CONFIG.ForEach(ADV =>
                    {
                        parameterLatterHalf.Add(new SqlParameter("@CURRENCY", SqlDbType.VarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value });
                        parameterLatterHalf.Add(new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value });

                        //寫入：費用申請單 已預支parameter
                        strJson = jsonFunction.ObjectToJSON(ADV);
                        GlobalParameters.Infoparameter(strJson, parameterLatterHalf);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_ADV]([RequisitionID],[Currency],[Amount]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@CURRENCY,@AMOUNT) ";

                        dbFun.DoTran(strSQL, parameterLatterHalf);

                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@CURRENCY")).FirstOrDefault());
                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@AMOUNT")).FirstOrDefault());
                    });
                }

                #endregion

                #region - 費用申請單 應退：ExpensesReimburse_AR -

                if (model.EXPENSES_REIMBURSE_ARS_CONFIG != null && model.EXPENSES_REIMBURSE_ARS_CONFIG.Count > 0)
                {
                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_AR] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterLatterHalf);

                    #endregion

                    model.EXPENSES_REIMBURSE_ARS_CONFIG.ForEach(AR =>
                    {
                        parameterLatterHalf.Add(new SqlParameter("@CURRENCY", SqlDbType.VarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value });
                        parameterLatterHalf.Add(new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value });

                        //寫入：費用申請單 應退parameter
                        strJson = jsonFunction.ObjectToJSON(AR);
                        GlobalParameters.Infoparameter(strJson, parameterLatterHalf);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_AR]([RequisitionID],[Currency],[Amount]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@CURRENCY,@AMOUNT) ";

                        dbFun.DoTran(strSQL, parameterLatterHalf);

                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@CURRENCY")).FirstOrDefault());
                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@AMOUNT")).FirstOrDefault());
                    });
                }

                #endregion

                #region - 費用申請單 應付：ExpensesReimburse_AP -

                if (model.EXPENSES_REIMBURSE_APS_CONFIG != null && model.EXPENSES_REIMBURSE_APS_CONFIG.Count > 0)
                {
                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_AP] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterLatterHalf);

                    #endregion

                    model.EXPENSES_REIMBURSE_APS_CONFIG.ForEach(AP =>
                    {
                        parameterLatterHalf.Add(new SqlParameter("@CURRENCY", SqlDbType.VarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value });
                        parameterLatterHalf.Add(new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value });

                        //寫入：費用申請單 應退parameter
                        strJson = jsonFunction.ObjectToJSON(AP);
                        GlobalParameters.Infoparameter(strJson, parameterLatterHalf);

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_AP] ";
                        strSQL += "SET [Currency] =@CURRENCY, ";
                        strSQL += "     [Amount]=@AMOUNT ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";

                        dbFun.DoTran(strSQL, parameterLatterHalf);

                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@CURRENCY")).FirstOrDefault());
                        parameterLatterHalf.Remove(parameterLatterHalf.Where(SP => SP.ParameterName.Contains("@AMOUNT")).FirstOrDefault());
                    });
                }

                #endregion

                #region - 費用申請單 表單關聯：AssociatedForm -

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
                CommLib.Logger.Error("費用申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "ExpensesReimburse";

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