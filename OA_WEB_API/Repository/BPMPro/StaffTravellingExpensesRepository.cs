//using OA_WEB_API.Models.BPMPro;
//using OA_WEB_API.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.Linq;
//using System.Web;

//namespace OA_WEB_API.Repository.BPMPro
//{
//    /// <summary>
//    /// 會簽管理系統 - 差旅費用報支單
//    /// </summary>
//    public class StaffTravellingExpensesRepository
//    {
//        #region - 宣告 -

//        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

//        #region Repository

//        FormRepository formRepository = new FormRepository();
//        CommonRepository commonRepository = new CommonRepository();
//        NotifyRepository notifyRepository = new NotifyRepository();
//        UserRepository userRepository = new UserRepository();

//        #endregion

//        #endregion

//        #region - 方法 -

//        /// <summary>
//        /// 差旅費用報支單(查詢)
//        /// </summary>
//        public StaffTravellingExpensesViewModel PostStaffTravellingExpensesSingle(StaffTravellingExpensesQueryModel query)
//        {
//            var parameter = new List<SqlParameter>()
//            {
//                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
//            };

//            #region - 申請人資訊 -

//            var CommonApplicantInfo = new BPMCommonModel<ApplicantInfo>()
//            {
//                EXT = "M",
//                IDENTIFY = IDENTIFY,
//                PARAMETER = parameter,
//            };
//            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApplicantInfoFunction(CommonApplicantInfo));
//            var applicantInfo = jsonFunction.JsonToObject<ApplicantInfo>(strJson);

//            #endregion

//            #region - 差旅費用報支單 表頭資訊 -

//            strSQL = "";
//            strSQL += "SELECT ";
//            strSQL += "     [FlowName] AS [FLOW_NAME], ";
//            strSQL += "     [FormNo] AS [FORM_NO], ";
//            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
//            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
//            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
//            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

//            var staffTravellingExpensesTitle = dbFun.DoQuery(strSQL, parameter).ToList<StaffTravellingExpensesTitle>().FirstOrDefault();

//            #endregion

//            #region - 差旅費用報支單 表單內容 -

//            strSQL = "";
//            strSQL += "SELECT ";
//            strSQL += "     [IsCFO] AS [IS_CFO], ";
//            strSQL += "     [TravellingStaffs] AS [TRAVELLING_STAFFS], ";
//            strSQL += "     [Reason] AS [REASON], ";
//            strSQL += "     [Note] AS [NOTE], ";
//            strSQL += "     [RefundMode] AS [REFUND_MODE], ";
//            strSQL += "     [Amount_CONV_Total] AS [AMOUNT_CONV_TOTAL], ";
//            strSQL += "     [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
//            strSQL += "     [FinancAuditName_1] AS [FINANC_AUDIT_NAME_1], ";
//            strSQL += "     [FinancAuditID_2] AS [FINANC_AUDIT_ID_2], ";
//            strSQL += "     [FinancAuditName_2] AS [FINANC_AUDIT_NAME_2] ";
//            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
//            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

//            var staffTravellingExpensesConfig = dbFun.DoQuery(strSQL, parameter).ToList<StaffTravellingExpensesConfig>().FirstOrDefault();

//            #endregion

//            #region - 差旅費用報支單 差旅明細 -

//            strSQL = "";
//            strSQL += "SELECT ";
//            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
//            strSQL += "     [RowNo] AS [ROW_NO], ";
//            strSQL += "     [TravellingDate] AS [TRAVELLING_DATE], ";
//            strSQL += "     [Place] AS [PLACE], ";
//            strSQL += "     [Payer] AS [PAYER], ";
//            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
//            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
//            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
//            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
//            strSQL += "     [Note] AS [NOTE], ";
//            strSQL += "     [InvoiceRowNo] AS [INV_ROW_NO], ";
//            strSQL += "     [InvoiceType] AS [INV_TYPE], ";
//            strSQL += "     [Num] AS [NUM], ";
//            strSQL += "     [Date] AS [DATE], ";
//            strSQL += "     [Amount] AS [AMOUNT], ";
//            strSQL += "     [Amount_CONV] AS [AMOUNT_CONV], ";
//            strSQL += "     [Currency] AS [CURRENCY], ";
//            strSQL += "     [ExchangeRate] AS [EXCH_RATE], ";
//            strSQL += "     [InvoiceNote] AS [INV_NOTE] ";
//            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
//            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
//            strSQL += "ORDER BY [AutoCounter] ";

//            var staffTravellingExpensesDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<StaffTravellingExpensesDetailsConfig>().ToList();

//            #endregion

//            #region - 差旅費用報支單 憑證細項 -

//            strSQL = "";
//            strSQL += "SELECT ";
//            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
//            strSQL += "     [RowNo] AS [ROW_NO], ";
//            strSQL += "     [InvoiceRowNo] AS [INV_ROW_NO], ";
//            strSQL += "     [Num] AS [NUM], ";
//            strSQL += "     [Name] AS [NAME], ";
//            strSQL += "     [Amount] AS [AMOUNT], ";
//            strSQL += "     [Amount_TWD] AS [AMOUNT_TWD] ";
//            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_INV_DTL] ";
//            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
//            strSQL += "ORDER BY [AutoCounter] ";

//            var staffTravellingExpensesInvoiceDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<StaffTravellingExpensesInvoiceDetailsConfig>().ToList();

//            #endregion

//            #region - 差旅費用報支單 使用預算 -

//            var CommonBUDG = new BPMCommonModel<StaffTravellingExpensesBudgetsConfig>()
//            {
//                EXT = "BUDG",
//                IDENTIFY = IDENTIFY,
//                PARAMETER = parameter
//            };
//            strJson = jsonFunction.ObjectToJSON(commonRepository.PostBudgetFunction(CommonBUDG));
//            var staffTravellingExpensesBudgetsConfig = jsonFunction.JsonToObject<List<StaffTravellingExpensesBudgetsConfig>>(strJson);

//            #endregion

//            #region - 差旅費用報支單 小計 -

//            var CommonSUM = new BPMCommonModel<StaffTravellingExpensesSumsConfig>()
//            {
//                EXT = "SUM",
//                IDENTIFY = IDENTIFY,
//                PARAMETER = parameter
//            };
//            strJson = jsonFunction.ObjectToJSON(commonRepository.PostExpensesReimburseProcessLatterHalfFunction(CommonSUM));
//            var staffTravellingExpensesSumsConfig = jsonFunction.JsonToObject<List<StaffTravellingExpensesSumsConfig>>(strJson);

//            #endregion

//            #region - 差旅費用報支單 已預支 -

//            var CommonADV = new BPMCommonModel<StaffTravellingExpensesAdvancesConfig>()
//            {
//                EXT = "ADV",
//                IDENTIFY = IDENTIFY,
//                PARAMETER = parameter
//            };
//            strJson = jsonFunction.ObjectToJSON(commonRepository.PostExpensesReimburseProcessLatterHalfFunction(CommonADV));
//            var staffTravellingExpensesAdvancesConfig = jsonFunction.JsonToObject<List<StaffTravellingExpensesAdvancesConfig>>(strJson);

//            #endregion

//            #region - 差旅費用報支單 應退(退還財務) -

//            var CommonFA = new BPMCommonModel<StaffTravellingExpensesFinancAmountsConfig>()
//            {
//                EXT = "FA",
//                IDENTIFY = IDENTIFY,
//                PARAMETER = parameter
//            };
//            strJson = jsonFunction.ObjectToJSON(commonRepository.PostExpensesReimburseProcessLatterHalfFunction(CommonFA));
//            var staffTravellingExpensesFinancAmountsConfig = jsonFunction.JsonToObject<List<StaffTravellingExpensesFinancAmountsConfig>>(strJson);

//            #endregion

//            #region - 差旅費用報支單 應付(付給使用者) -

//            var CommonUA = new BPMCommonModel<StaffTravellingExpensesUserAmountsConfig>()
//            {
//                EXT = "UA",
//                IDENTIFY = IDENTIFY,
//                PARAMETER = parameter
//            };
//            strJson = jsonFunction.ObjectToJSON(commonRepository.PostExpensesReimburseProcessLatterHalfFunction(CommonUA));
//            var staffTravellingExpensesUserAmountsConfig = jsonFunction.JsonToObject<List<StaffTravellingExpensesUserAmountsConfig>>(strJson);

//            #endregion

//            #region - 費用申請單 表單關聯 -

//            var formQueryModel = new FormQueryModel()
//            {
//                REQUISITION_ID = query.REQUISITION_ID
//            };
//            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

//            #endregion

//            var staffTravellingExpensesViewModel = new StaffTravellingExpensesViewModel()
//            {
//                APPLICANT_INFO = applicantInfo,
//                STAFF_TRAVELLING_EXPENSES_TITLE = staffTravellingExpensesTitle,
//                STAFF_TRAVELLING_EXPENSES_CONFIG = staffTravellingExpensesConfig,
//                STAFF_TRAVELLING_EXPENSES_DTLS_CONFIG = staffTravellingExpensesDetailsConfig,
//                STAFF_TRAVELLING_EXPENSES_INV_DTLS_CONFIG = staffTravellingExpensesInvoiceDetailsConfig,
//                STAFF_TRAVELLING_EXPENSE_BUDGS_CONFIG = staffTravellingExpensesBudgetsConfig,
//                STAFF_TRAVELLING_EXPENSES_SUMS_CONFIG = staffTravellingExpensesSumsConfig,
//                STAFF_TRAVELLING_EXPENSES_ADVS_CONFIG = staffTravellingExpensesAdvancesConfig,
//                STAFF_TRAVELLING_EXPENSES_FAS_CONFIG = staffTravellingExpensesFinancAmountsConfig,
//                STAFF_TRAVELLING_EXPENSES_UAS_CONFIG = staffTravellingExpensesUserAmountsConfig,
//                ASSOCIATED_FORM_CONFIG = associatedForm
//            };

//            #region - 確認表單 -

//            if (staffTravellingExpensesViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
//            {
//                var formData = new FormData()
//                {
//                    REQUISITION_ID = query.REQUISITION_ID
//                };

//                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
//                {
//                    staffTravellingExpensesViewModel = new StaffTravellingExpensesViewModel();
//                    CommLib.Logger.Error("費用申請單(查詢)失敗，原因：系統無正常起單。");
//                }
//                else
//                {
//                    #region - 確認M表BPM表單單號 -

//                    //避免儲存後送出表單BPM表單單號沒寫入的情形
//                    var formQuery = new FormQueryModel()
//                    {
//                        REQUISITION_ID = query.REQUISITION_ID
//                    };
//                    notifyRepository.ByInsertBPMFormNo(formQuery);

//                    if (String.IsNullOrEmpty(staffTravellingExpensesViewModel.STAFF_TRAVELLING_EXPENSES_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(staffTravellingExpensesViewModel.STAFF_TRAVELLING_EXPENSES_TITLE.BPM_FORM_NO))
//                    {
//                        strSQL = "";
//                        strSQL += "SELECT ";
//                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
//                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
//                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
//                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
//                        if (dtBpmFormNo.Rows.Count > 0) staffTravellingExpensesViewModel.STAFF_TRAVELLING_EXPENSES_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
//                    }

//                    #endregion
//                }
//            }

//            #endregion

//            return staffTravellingExpensesViewModel;
//        }


//        /// <summary>
//        /// 差旅費用報支單(新增/修改/草稿)
//        /// </summary>
//        public bool PutStaffTravellingExpensesSingle(StaffTravellingExpensesViewModel model)
//        {
//            bool vResult = false;
//            try
//            {
//                #region - 宣告 -

//                #region - 系統編號 -

//                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
//                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
//                {
//                    strREQ = Guid.NewGuid().ToString();
//                }

//                #endregion

//                #region - 主旨 -

//                if (String.IsNullOrEmpty(model.STAFF_TRAVELLING_EXPENSES_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.STAFF_TRAVELLING_EXPENSES_TITLE.FM7_SUBJECT))
//                {
//                    // 單號由流程事件做寫入
//                    FM7Subject = "(待填寫)" + model.STAFF_TRAVELLING_EXPENSES_TITLE.FLOW_NAME + "_差旅費用報支單。";
//                }
//                else
//                {
//                    FM7Subject = model.STAFF_TRAVELLING_EXPENSES_TITLE.FM7_SUBJECT;
//                }

//                #endregion

//                #endregion

//                #region - 差旅費用報支單 表頭資訊：StaffTravellingExpenses_M -

//                var parameterTitle = new List<SqlParameter>()
//                {
//                    //表單資訊
//                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  strREQ},
//                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
//                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value =  model.APPLICANT_INFO.PRIORITY},
//                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value =  model.APPLICANT_INFO.DRAFT_FLAG},
//                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value =  model.APPLICANT_INFO.FLOW_ACTIVATED},
//                    //(申請人/起案人)資訊
//                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
//                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
//                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
//                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
//                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
//                    //(填單人/代填單人)資訊
//                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
//                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
//                    //差旅費用報支單 表頭
//                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.STAFF_TRAVELLING_EXPENSES_TITLE.FLOW_NAME ?? DBNull.Value },
//                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.STAFF_TRAVELLING_EXPENSES_TITLE.FORM_NO ?? DBNull.Value },
//                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
//                };

//                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

//                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
//                {
//                    var formData = new FormData()
//                    {
//                        REQUISITION_ID = strREQ
//                    };

//                    if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
//                    {
//                        parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });
//                        IsADD = true;
//                    }
//                }
//                else parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });

//                #endregion

//                strSQL = "";
//                strSQL += "SELECT ";
//                strSQL += "      [RequisitionID] ";
//                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
//                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

//                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

//                if (dtA.Rows.Count > 0)
//                {
//                    #region - 修改 -

//                    strSQL = "";
//                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
//                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
//                    strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
//                    strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
//                    strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
//                    strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
//                    strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";

//                    if (IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

//                    strSQL += "     [FillerID]=@FILLER_ID, ";
//                    strSQL += "     [FillerName]=@FILLER_NAME, ";
//                    strSQL += "     [Priority]=@PRIORITY, ";
//                    strSQL += "     [DraftFlag]=@DRAFT_FLAG, ";
//                    strSQL += "     [FlowActivated]=@FLOW_ACTIVATED, ";
//                    strSQL += "     [FlowName]=@FLOW_NAME, ";
//                    strSQL += "     [FormNo]=@FORM_NO, ";
//                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
//                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

//                    dbFun.DoTran(strSQL, parameterTitle);

//                    #endregion
//                }
//                else
//                {
//                    #region - 新增 -

//                    strSQL = "";
//                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
//                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

//                    dbFun.DoTran(strSQL, parameterTitle);

//                    #endregion
//                }

//                #endregion

//                #region - 差旅費用報支單 表單內容：StaffTravellingExpenses_M -

//                if (model.STAFF_TRAVELLING_EXPENSES_CONFIG != null)
//                {
//                    var parameterInfo = new List<SqlParameter>()
//                    {
//                        //差旅費用報支單 表單內容
//                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
//                        new SqlParameter("@IS_CFO", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@TRAVELLING_STAFFS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@REFUND_MODE", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@AMOUNT_CONV_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@FINANC_AUDIT_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@FINANC_AUDIT_NAME_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@FINANC_AUDIT_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                        new SqlParameter("@FINANC_AUDIT_NAME_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                    };

//                    var queryFinanc = new UserQueryModel()
//                    {
//                        COMPANY_ID = "RootCompany",
//                        DEPT_FLOW = "財務部"
//                    };
//                    var FinancUser = userRepository.PostUsers(queryFinanc);

//                    if (!String.IsNullOrEmpty(model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_ID_1) || !String.IsNullOrWhiteSpace(model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_ID_1))
//                        model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_NAME_1 = FinancUser.Where(U => U.USER_ID == model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_ID_1).Select(U => U.USER_NAME).FirstOrDefault();

//                    if (!String.IsNullOrEmpty(model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_ID_2) || !String.IsNullOrWhiteSpace(model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_ID_2))
//                        model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_NAME_2 = FinancUser.Where(U => U.USER_ID == model.STAFF_TRAVELLING_EXPENSES_CONFIG.FINANC_AUDIT_ID_2).Select(U => U.USER_NAME).FirstOrDefault();


//                    if (String.IsNullOrEmpty(model.STAFF_TRAVELLING_EXPENSES_CONFIG.IS_CFO) || String.IsNullOrWhiteSpace(model.STAFF_TRAVELLING_EXPENSES_CONFIG.IS_CFO))
//                    {
//                        if (model.STAFF_TRAVELLING_EXPENSES_CONFIG.AMOUNT_CONV_TOTAL >= 10000) model.STAFF_TRAVELLING_EXPENSES_CONFIG.IS_CFO = true.ToString().ToLower();
//                        else model.STAFF_TRAVELLING_EXPENSES_CONFIG.IS_CFO = false.ToString().ToLower();
//                    }

//                    //寫入：費用申請單 表單內容parameter                        
//                    strJson = jsonFunction.ObjectToJSON(model.STAFF_TRAVELLING_EXPENSES_CONFIG);
//                    GlobalParameters.Infoparameter(strJson, parameterInfo);

//                    strSQL = "";
//                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
//                    strSQL += "SET [IsCFO]=@IS_CFO, ";
//                    strSQL += "     [TravellingStaffs]=@TRAVELLING_STAFFS, ";
//                    strSQL += "     [Reason]=@REASON, ";
//                    strSQL += "     [Note]=@NOTE, ";
//                    strSQL += "     [RefundMode]=@REFUND_MODE, ";
//                    strSQL += "     [Amount_CONV_Total]=@AMOUNT_CONV_TOTAL, ";
//                    strSQL += "     [FinancAuditID_1]=@FINANC_AUDIT_ID_1, ";
//                    strSQL += "     [FinancAuditName_1]=@FINANC_AUDIT_NAME_1, ";
//                    strSQL += "     [FinancAuditID_2]=@FINANC_AUDIT_ID_2, ";
//                    strSQL += "     [FinancAuditName_2]=@FINANC_AUDIT_NAME_2 ";
//                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

//                    dbFun.DoTran(strSQL, parameterInfo);

//                }

//                #endregion

//                #region - 差旅費用報支單 差旅明細: StaffTravellingExpenses_DTL -

//                var parameterDetails = new List<SqlParameter>()
//                {
//                    //差旅費用報支單 差旅明細
//                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
//                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@TRAVELLING_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PLACE", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PAYER", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@INV_TYPE", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@AMOUNT_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@EXCH_RATE", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@INV_NOTE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value }
//                };

//                #region 先刪除舊資料

//                strSQL = "";
//                strSQL += "DELETE ";
//                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
//                strSQL += "WHERE 1=1 ";
//                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

//                dbFun.DoTran(strSQL, parameterDetails);

//                #endregion

//                if (model.STAFF_TRAVELLING_EXPENSES_DTLS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSES_DTLS_CONFIG.Count > 0)
//                {
//                    #region 再新增資料

//                    model.STAFF_TRAVELLING_EXPENSES_DTLS_CONFIG.ForEach(DTL =>
//                    {
//                        //寫入：差旅費用報支單 差旅明細parameter
//                        strJson = jsonFunction.ObjectToJSON(DTL);
//                        GlobalParameters.Infoparameter(strJson, parameterDetails);

//                        strSQL = "";
//                        strSQL += "INSERT INTO [BPMPro].[dbo].FM7T_" + IDENTIFY + "_DTL]([RequisitionID],[RowNo],[TravellingDate],[Place],[Payer],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Note],[InvoiceRowNo],[InvoiceType],[Num],[Date],[Amount],[Amount_CONV],[Currency],[ExchangeRate],[InvoiceNote]) ";
//                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@TRAVELLING_DATE,@PLACE,@PAYER,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@NOTE,@INV_ROW_NO,@INV_TYPE,@NUM,@DATE,@AMOUNT,@CURRENCY,@EXCH_RATE,@INV_NOTE)";

//                        dbFun.DoTran(strSQL, parameterDetails);
//                    });

//                    #endregion
//                }

//                #endregion

//                #region - 差旅費用報支單 憑證細項: StaffTravellingExpenses_INV_DTL -

//                var parameterInvoiceDetails = new List<SqlParameter>()
//                {
//                    //差旅費用報支單 憑證細項
//                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
//                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@NAME", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@AMOUNT_CONV", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
//                };

//                #region 先刪除舊資料

//                strSQL = "";
//                strSQL += "DELETE ";
//                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_INV_DTL] ";
//                strSQL += "WHERE 1=1 ";
//                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

//                dbFun.DoTran(strSQL, parameterDetails);

//                #endregion

//                if (model.STAFF_TRAVELLING_EXPENSES_INV_DTLS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSES_INV_DTLS_CONFIG.Count > 0)
//                {
//                    #region 再新增資料

//                    model.STAFF_TRAVELLING_EXPENSES_INV_DTLS_CONFIG.ForEach(INV_DTL =>
//                    {
//                        //寫入：差旅費用報支單 差旅明細parameter
//                        strJson = jsonFunction.ObjectToJSON(INV_DTL);
//                        GlobalParameters.Infoparameter(strJson, parameterInvoiceDetails);

//                        strSQL = "";
//                        strSQL += "INSERT INTO [BPMPro].[dbo].FM7T_" + IDENTIFY + "_INV_DTL]([RequisitionID],[RowNo],[InvoiceRowNo],[Num],[Name],[Amount],[Amount_TWD]) ";
//                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@INV_ROW_NO,@NUM,@NAME,@AMOUNT,@AMOUNT_TWD)";

//                        dbFun.DoTran(strSQL, parameterInvoiceDetails);
//                    });

//                    #endregion
//                }

//                #endregion

//                #region - 差旅費用報支單 使用預算：StaffTravellingExpenses_BUDG -

//                var parameterBudgets = new List<SqlParameter>()
//                {
//                    //差旅費用報支單 使用預算
//                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
//                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@PERIOD", SqlDbType.Int) { Size = 2, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@CREATE_YEAR", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@OWNER_DEPT", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@AVAILABLE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                    new SqlParameter("@USE_BUDGET_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
//                };

//                if (model.STAFF_TRAVELLING_EXPENSE_BUDGS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSE_BUDGS_CONFIG.Count > 0)
//                {
//                    var CommonBUDG = new BPMCommonModel<StaffTravellingExpensesBudgetsConfig>()
//                    {
//                        EXT = "BUDG",
//                        IDENTIFY = IDENTIFY,
//                        PARAMETER = parameterBudgets,
//                        MODEL = model.STAFF_TRAVELLING_EXPENSE_BUDGS_CONFIG
//                    };
//                    commonRepository.PutBudgetFunction(CommonBUDG);
//                }

//                #endregion

//                var parameterLatterHalf = new List<SqlParameter>()
//                {
//                    //費用申請單 小計、已預支、應退、應付
//                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
//                };

//                #region - 差旅費用報支單 小計：StaffTravellingExpenses_SUM -

//                if (model.STAFF_TRAVELLING_EXPENSES_SUMS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSES_SUMS_CONFIG.Count > 0)
//                {
//                    var CommonSUM = new BPMCommonModel<StaffTravellingExpensesSumsConfig>()
//                    {
//                        EXT = "SUM",
//                        IDENTIFY = IDENTIFY,
//                        PARAMETER = parameterLatterHalf,
//                        MODEL = model.STAFF_TRAVELLING_EXPENSES_SUMS_CONFIG
//                    };
//                    vResult = commonRepository.PutExpensesReimburseProcessLatterHalfFunction(CommonSUM);
//                }

//                #endregion

//                #region - 差旅費用報支單 已預支：StaffTravellingExpenses_ADV -

//                if (model.STAFF_TRAVELLING_EXPENSES_ADVS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSES_ADVS_CONFIG.Count > 0)
//                {
//                    var CommonSUM = new BPMCommonModel<StaffTravellingExpensesAdvancesConfig>()
//                    {
//                        EXT = "ADV",
//                        IDENTIFY = IDENTIFY,
//                        PARAMETER = parameterLatterHalf,
//                        MODEL = model.STAFF_TRAVELLING_EXPENSES_ADVS_CONFIG
//                    };
//                    if (vResult) vResult = commonRepository.PutExpensesReimburseProcessLatterHalfFunction(CommonSUM);
//                }

//                #endregion

//                #region - 差旅費用報支單 應退(退還財務)：StaffTravellingExpenses_FA -

//                if (model.STAFF_TRAVELLING_EXPENSES_FAS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSES_FAS_CONFIG.Count > 0)
//                {
//                    var CommonSUM = new BPMCommonModel<StaffTravellingExpensesFinancAmountsConfig>()
//                    {
//                        EXT = "FA",
//                        IDENTIFY = IDENTIFY,
//                        PARAMETER = parameterLatterHalf,
//                        MODEL = model.STAFF_TRAVELLING_EXPENSES_FAS_CONFIG
//                    };
//                    if (vResult) vResult = commonRepository.PutExpensesReimburseProcessLatterHalfFunction(CommonSUM);
//                }

//                #endregion

//                #region - 差旅費用報支單 應付(付給使用者)：StaffTravellingExpenses_UA -

//                if (model.STAFF_TRAVELLING_EXPENSES_UAS_CONFIG != null && model.STAFF_TRAVELLING_EXPENSES_UAS_CONFIG.Count > 0)
//                {
//                    var CommonSUM = new BPMCommonModel<StaffTravellingExpensesUserAmountsConfig>()
//                    {
//                        EXT = "UA",
//                        IDENTIFY = IDENTIFY,
//                        PARAMETER = parameterLatterHalf,
//                        MODEL = model.STAFF_TRAVELLING_EXPENSES_UAS_CONFIG
//                    };
//                    if (vResult) vResult = commonRepository.PutExpensesReimburseProcessLatterHalfFunction(CommonSUM);
//                }

//                #endregion                

//                #region - 差旅費用報支單 表單關聯：AssociatedForm -

//                var associatedFormModel = new AssociatedFormModel()
//                {
//                    REQUISITION_ID = strREQ,
//                    ASSOCIATED_FORM_CONFIG = model.ASSOCIATED_FORM_CONFIG
//                };

//                if (vResult) commonRepository.PutAssociatedForm(associatedFormModel);

//                #endregion

//                #region - 表單主旨：FormHeader -

//                FormHeader header = new FormHeader();
//                header.REQUISITION_ID = strREQ;
//                header.ITEM_NAME = "Subject";
//                header.ITEM_VALUE = FM7Subject;

//                formRepository.PutFormHeader(header);

//                #endregion

//                #region - 儲存草稿：FormDraftList -

//                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
//                {
//                    FormDraftList draftList = new FormDraftList();
//                    draftList.REQUISITION_ID = strREQ;
//                    draftList.IDENTIFY = IDENTIFY;
//                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

//                    formRepository.PutFormDraftList(draftList, true);
//                }

//                #endregion

//                #region - 送出表單：FormAutoStart -

//                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
//                {
//                    #region 送出表單前，先刪除草稿清單

//                    FormDraftList draftList = new FormDraftList();
//                    draftList.REQUISITION_ID = strREQ;
//                    draftList.IDENTIFY = IDENTIFY;
//                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

//                    formRepository.PutFormDraftList(draftList, false);

//                    #endregion

//                    FormAutoStart autoStart = new FormAutoStart();
//                    autoStart.REQUISITION_ID = strREQ;
//                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
//                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
//                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

//                    formRepository.PutFormAutoStart(autoStart);
//                }

//                #endregion

//                #region - 表單機能啟用：BPMFormFunction -

//                var BPM_FormFunction = new BPMFormFunction()
//                {
//                    REQUISITION_ID = strREQ,
//                    IDENTIFY = IDENTIFY,
//                    DRAFT_FLAG = 0
//                };
//                commonRepository.PostBPMFormFunction(BPM_FormFunction);

//                #endregion
//            }
//            catch (Exception ex)
//            {
//                vResult = false;
//                CommLib.Logger.Error("差旅費用報支單(新增/修改/草稿)失敗，原因：" + ex.Message);
//            }

//            return vResult;
//        }

//        #endregion

//        #region - 欄位和屬性 -

//        /// <summary>
//        /// T-SQL
//        /// </summary>
//        private string strSQL;

//        /// <summary>
//        /// 確認是否為新建的表單
//        /// </summary>
//        private bool IsADD = false;

//        /// <summary>
//        /// 表單代號
//        /// </summary>
//        private string IDENTIFY = "StaffTravellingExpenses";

//        /// <summary>
//        /// 表單主旨
//        /// </summary>
//        private string FM7Subject;

//        /// <summary>
//        /// Json字串
//        /// </summary>
//        private string strJson;

//        /// <summary>
//        /// 系統編號
//        /// </summary>
//        private string strREQ;

//        #endregion
//    }
//}