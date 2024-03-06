using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using OA_WEB_API.Models;
using System.Runtime.InteropServices;
using OA_WEB_API.Models.ERP;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 企業乘車對帳單
    /// </summary>
    public class EnterpriseTaxiReviewRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();
        SysCommonRepository sysCommonRepository = new SysCommonRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 企業乘車對帳單(主單查詢)
        /// </summary>
        public EnterpriseTaxiReviewViewModel PostEnterpriseTaxiReviewSingle(EnterpriseTaxiReviewQueryModel query)
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

            #region - 企業乘車對帳單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var enterpriseTaxiReviewTitle = dbFun.DoQuery(strSQL, parameter).ToList<EnterpriseTaxiReviewTitle>().FirstOrDefault();

            #endregion

            #region - 企業乘車對帳單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [AccountingDateStart] AS [ACCOUNTING_DATE_START], ";
            strSQL += "     [AccountingDateEnd] AS [ACCOUNTING_DATE_END], ";
            strSQL += "     [BillPlan] AS [BILL_PLAN], ";
            strSQL += "     [PayOffPeriod] AS [PAY_OFF_PERIOD], ";
            strSQL += "     [AddExpense] AS [ADD_EXPENSE], ";
            strSQL += "     [AccountsPayable] AS [ACCOUNTS_PAYABLE], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [Total] AS [TOTAL] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var enterpriseTaxiReviewConfig = dbFun.DoQuery(strSQL, parameter).ToList<EnterpriseTaxiReviewConfig>().FirstOrDefault();

            #endregion

            #region - 企業乘車對帳單 乘車明細、使用預算 -

            //企業乘車對帳單 乘車明細、使用預算 檢視，由另一隻API篩選設定後View。

            #endregion            

            var enterpriseTaxiReviewViewModel = new EnterpriseTaxiReviewViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                ENTERPRISE_TAXI_REVIEW_TITLE = enterpriseTaxiReviewTitle,
                ENTERPRISE_TAXI_REVIEW_CONFIG = enterpriseTaxiReviewConfig,
                ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG = null,
                ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG = null,
            };

            #region - 確認表單 -

            if (enterpriseTaxiReviewViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    enterpriseTaxiReviewViewModel = new EnterpriseTaxiReviewViewModel();
                    CommLib.Logger.Error("企業乘車對帳單(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(enterpriseTaxiReviewViewModel.ENTERPRISE_TAXI_REVIEW_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(enterpriseTaxiReviewViewModel.ENTERPRISE_TAXI_REVIEW_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) enterpriseTaxiReviewViewModel.ENTERPRISE_TAXI_REVIEW_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return enterpriseTaxiReviewViewModel;
        }

        /// <summary>
        /// 企業乘車對帳單(明細查詢)
        /// </summary>
        public EnterpriseTaxiReviewDetailsViewModel PostEnterpriseTaxiReviewDetailsSingle(EnterpriseTaxiReviewDetailsQueryModel query)
        {
            #region - 宣告 -

            var strSetSQL = String.Empty;

            var parameter = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            if (!String.IsNullOrEmpty(query.USER_ID) || !String.IsNullOrWhiteSpace(query.USER_ID))
            {
                if (query.IS_ALL)
                {
                    if (CommonRepository.GetRoles().Any(R => R.ROLE_ID == "GV_ENTERPRISE_TAXI_REVIEW_VIEW_ALL" && R.USER_ID == query.USER_ID)) query.IS_ALL = true;
                    else query.IS_ALL = false;
                }
            }
            else query.IS_ALL = true;

            #endregion

            #region - 企業乘車對帳單 乘車明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [Ticket] AS [TICKET], ";
            strSQL += "     [GetOnDate] AS [GET_ON_DATE], ";
            strSQL += "     [GetOffDate] AS [GET_OFF_DATE], ";
            strSQL += "     [GetOnTime] AS [GET_ON_TIME], ";
            strSQL += "     [GetOffTime] AS [GET_OFF_TIME], ";
            strSQL += "     [GetOnPlace] AS [GET_ON_PLACE], ";
            strSQL += "     [GetOffPlace] AS [GET_OFF_PLACE], ";
            strSQL += "     [ComplementGetOnPlace] AS [COMPLEMENT_GET_ON_PLACE], ";
            strSQL += "     [ComplementGetOffPlace] AS [COMPLEMENT_GET_OFF_PLACE], ";
            strSQL += "     [TravelByPurpose] AS [TRAVEL_BY_PURPOSE], ";
            strSQL += "     [TaxiExpenses] AS [TAXI_EXPENSES], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [AccountID] AS [ACCOUNT_ID], ";
            strSQL += "     [DeptID] AS [DEPT_ID], ";
            strSQL += "     [DeptName] AS [DEPT_NAME], ";
            strSQL += "     [OfficeID] AS [OFFICE_ID], ";
            strSQL += "     [OfficeName] AS [OFFICE_NAME], ";
            strSQL += "     [GroupID] AS [GROUP_ID], ";
            strSQL += "     [GroupName] AS [GROUP_NAME], ";
            strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR], ";
            strSQL += "     [Flag] AS [FLAG] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var enterpriseTaxiReviewDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<EnterpriseTaxiReviewDetailsConfig>().ToList();

            #endregion

            if (!query.IS_ALL)
            {
                var setUserApproverQuery = new SetUserApproverQueryModel()
                {
                    APPROVER_ID = query.USER_ID,
                    IDENTIFY = IDENTIFY
                };

                var SetMember = userRepository.PostSetUserApproverSingle(setUserApproverQuery);

                enterpriseTaxiReviewDetailsConfig = enterpriseTaxiReviewDetailsConfig.Where(DTL => SetMember.Select(S => S.USER_ID).Contains(DTL.ACCOUNT_ID) || DTL.ACCOUNT_ID == query.USER_ID).ToList();

            }

            var ArrayRowNo = enterpriseTaxiReviewDetailsConfig.Select(DTL => DTL.ROW_NO).ToArray();

            #region - 企業乘車對帳單 使用預算 -

            var CommonBUDG = new BPMCommonModel<EnterpriseTaxiReviewBudgetsConfig>()
            {
                EXT = "BUDG",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostBudgetFunction(CommonBUDG));
            var enterpriseTaxiReviewBudgetsConfig = jsonFunction.JsonToObject<List<EnterpriseTaxiReviewBudgetsConfig>>(strJson);

            if (!query.IS_ALL)
            {
                enterpriseTaxiReviewBudgetsConfig = enterpriseTaxiReviewBudgetsConfig.Where(BUDG => ArrayRowNo.Any(R => R == BUDG.ROW_NO)).ToList();
            }

            #endregion

            var enterpriseTaxiReviewDetailsViewModel = new EnterpriseTaxiReviewDetailsViewModel()
            {
                ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG = enterpriseTaxiReviewDetailsConfig,
                ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG = enterpriseTaxiReviewBudgetsConfig,
                REQUISITION_ID=strREQ
            };

            return enterpriseTaxiReviewDetailsViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 企業乘車對帳單(依此單內容重送)(僅外部起單使用)
        ///// </summary>
        //public bool PutEnterpriseTaxiReviewRefill(EnterpriseTaxiReviewQueryModel query)
        //{
        //    bool vResult = false;

        //    try
        //    {
        //        #region - 宣告 -

        //        var original = PostEnterpriseTaxiReviewSingle(query);
        //        strJson = jsonFunction.ObjectToJSON(original);

        //        var enterpriseTaxiReviewViewModel = new EnterpriseTaxiReviewViewModel();

        //        var requisitionID = Guid.NewGuid().ToString();

        //        #endregion

        //        #region - 重送內容 -

        //        enterpriseTaxiReviewViewModel = jsonFunction.JsonToObject<EnterpriseTaxiReviewViewModel>(strJson);

        //        #region - 申請人資訊 調整 -

        //        enterpriseTaxiReviewViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
        //        enterpriseTaxiReviewViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
        //        enterpriseTaxiReviewViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

        //        #endregion

        //        #region - 載入明細、預算 -

        //        var detailsQuery = new EnterpriseTaxiReviewDetailsQueryModel()
        //        {
        //            USER_ID = enterpriseTaxiReviewViewModel.APPLICANT_INFO.APPLICANT_ID,
        //            IS_ALL = true,
        //            REQUISITION_ID = requisitionID;
        //        };

        //        var originalDetails = PostEnterpriseTaxiReviewSingle(detailsQuery);
        //        enterpriseTaxiReviewViewModel.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG = originalDetails.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG;
        //        enterpriseTaxiReviewViewModel.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG = originalDetails.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG;

        //        #endregion

        //        #endregion

        //        #region - 送出 執行(新增/修改/草稿) -

        //        PutEnterpriseTaxiReviewSingle(enterpriseTaxiReviewViewModel);

        //        #endregion

        //        vResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("用印申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 企業乘車對帳單(新增/修改/草稿)
        /// </summary>
        public bool PutEnterpriseTaxiReviewSingle(EnterpriseTaxiReviewViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                var TaxiExpensesSum = 0;

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                if (String.IsNullOrEmpty(model.ENTERPRISE_TAXI_REVIEW_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.ENTERPRISE_TAXI_REVIEW_TITLE.FM7_SUBJECT))
                {
                    // 單號由流程事件做寫入
                    FM7Subject = DateTime.Parse(model.ENTERPRISE_TAXI_REVIEW_CONFIG.ACCOUNTING_DATE_END.ToString()).Year + "年度" + DateTime.Parse(model.ENTERPRISE_TAXI_REVIEW_CONFIG.ACCOUNTING_DATE_END.ToString()).Month + "月企業乘車審核單";
                }
                else
                {
                    FM7Subject = model.ENTERPRISE_TAXI_REVIEW_TITLE.FM7_SUBJECT;
                }

                #endregion

                #endregion

                #region - 企業乘車對帳單 表頭資訊：EnterpriseTaxiReview_M -

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
                    //企業乘車對帳單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.ENTERPRISE_TAXI_REVIEW_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.ENTERPRISE_TAXI_REVIEW_TITLE.FORM_NO ?? DBNull.Value },
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

                #region - 企業乘車對帳單 表單內容：EnterpriseTaxiReview_M -

                if (model.ENTERPRISE_TAXI_REVIEW_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //企業乘車對帳單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@ACCOUNTING_DATE_START", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACCOUNTING_DATE_END", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BILL_PLAN", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PAY_OFF_PERIOD", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ADD_EXPENSE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ACCOUNTS_PAYABLE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    };


                    if (model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG != null && model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG.Count > 0) TaxiExpensesSum = model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG.Sum(DTL => DTL.TAXI_EXPENSES);

                    if (TaxiExpensesSum > 0)
                    {
                        if (model.ENTERPRISE_TAXI_REVIEW_CONFIG.ADD_EXPENSE == 0) model.ENTERPRISE_TAXI_REVIEW_CONFIG.ADD_EXPENSE = TaxiExpensesSum;
                        if (model.ENTERPRISE_TAXI_REVIEW_CONFIG.ACCOUNTS_PAYABLE == 0) model.ENTERPRISE_TAXI_REVIEW_CONFIG.ACCOUNTS_PAYABLE = TaxiExpensesSum;
                        if (model.ENTERPRISE_TAXI_REVIEW_CONFIG.TOTAL == 0) model.ENTERPRISE_TAXI_REVIEW_CONFIG.TOTAL = TaxiExpensesSum;
                    }

                    //寫入：企業乘車對帳單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.ENTERPRISE_TAXI_REVIEW_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [AccountingDateStart]=@ACCOUNTING_DATE_START, ";
                    strSQL += "     [AccountingDateEnd]=@ACCOUNTING_DATE_END, ";
                    strSQL += "     [BillPlan]=@BILL_PLAN, ";
                    strSQL += "     [PayOffPeriod]=@PAY_OFF_PERIOD, ";
                    strSQL += "     [AddExpense]=@ADD_EXPENSE, ";
                    strSQL += "     [AccountsPayable]=@ACCOUNTS_PAYABLE, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [Total]=@TOTAL ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 企業乘車對帳單 乘車明細: EnterpriseTaxiReview_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //企業乘車對帳單 乘車明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TICKET", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_ON_DATE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_OFF_DATE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_ON_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_OFF_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_ON_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_OFF_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COMPLEMENT_GET_ON_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COMPLEMENT_GET_OFF_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TRAVEL_BY_PURPOSE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@TAXI_EXPENSES", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACCOUNT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OFFICE_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OFFICE_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROUP_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GROUP_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@Flag", SqlDbType.TinyInt) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                //企業乘車對帳單 乘車明細
                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG != null && model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG.Count > 0)
                {
                    var enterpriseTaxiReviewApproversConfigs = new List<EnterpriseTaxiReviewApproversConfig>();

                    #region 再新增資料

                    var i = 1;
                    model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG.ForEach(DTL =>
                    {
                        DTL.ROW_NO = i;

                        #region - 標註是否可以調整 -

                        DTL.FLAG = 0;
                        if (String.IsNullOrEmpty(DTL.GET_ON_DATE) || String.IsNullOrWhiteSpace(DTL.GET_ON_DATE)) DTL.FLAG = 1;
                        else if (String.IsNullOrEmpty(DTL.GET_OFF_DATE) || String.IsNullOrWhiteSpace(DTL.GET_OFF_DATE)) DTL.FLAG = 1;
                        else if (String.IsNullOrEmpty(DTL.GET_ON_TIME) || String.IsNullOrWhiteSpace(DTL.GET_ON_TIME)) DTL.FLAG = 1;
                        else if (String.IsNullOrEmpty(DTL.GET_OFF_TIME) || String.IsNullOrWhiteSpace(DTL.GET_OFF_TIME)) DTL.FLAG = 1;
                        else if (String.IsNullOrEmpty(DTL.GET_ON_PLACE) || String.IsNullOrWhiteSpace(DTL.GET_ON_PLACE)) DTL.FLAG = 1;
                        else if (String.IsNullOrEmpty(DTL.GET_OFF_PLACE) || String.IsNullOrWhiteSpace(DTL.GET_OFF_PLACE)) DTL.FLAG = 1;

                        #endregion

                        #region - 部門彙整 -
                        var UserInfo = userRepository.GetUsersStructure().Where(U => U.COMPANY_ID == "GTV" && U.IS_MAIN_JOB == 1 && U.USER_ID == DTL.ACCOUNT_ID).Select(U => U).FirstOrDefault();

                        if (UserInfo != null)
                        {
                            DTL.NAME = UserInfo.USER_NAME;
                            DTL.DEPT_ID = UserInfo.DEPT_ID;
                            DTL.DEPT_NAME = UserInfo.DEPT_NAME;
                            DTL.OFFICE_ID = UserInfo.OFFICE_ID;
                            DTL.OFFICE_NAME = UserInfo.OFFICE_NAME;
                            DTL.GROUP_ID = UserInfo.GROUP_ID;
                            DTL.GROUP_NAME = UserInfo.GROUP_NAME;
                        }

                        #endregion

                        //寫入：企業乘車對帳單 乘車明細parameter
                        strJson = jsonFunction.ObjectToJSON(DTL);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL]([RequisitionID],[RowNo],[Ticket],[GetOnDate],[GetOffDate],[GetOnTime],[GetOffTime],[GetOnPlace],[GetOffPlace],[ComplementGetOnPlace],[ComplementGetOffPlace],[TravelByPurpose],[TaxiExpenses],[Name],[AccountID],[DeptID],[DeptName],[OfficeID],[OfficeName],[GroupID],[GroupName],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear],[Flag]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ROW_NO,@TICKET,@GET_ON_DATE,@GET_OFF_DATE,@GET_ON_TIME,@GET_OFF_TIME,@GET_ON_PLACE,@GET_OFF_PLACE,@COMPLEMENT_GET_ON_PLACE,@COMPLEMENT_GET_OFF_PLACE,@TRAVEL_BY_PURPOSE,@TAXI_EXPENSES,@NAME,@ACCOUNT_ID,@DEPT_ID,@DEPT_NAME,@OFFICE_ID,@OFFICE_NAME,@GROUP_ID,@GROUP_NAME,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR,@FLAG) ";

                        dbFun.DoTran(strSQL, parameterDetails);

                        #region - 乘車會簽 彙整 -

                        var approversConfig = new EnterpriseTaxiReviewApproversConfig();
                        var userInfoMainDeptModel = new UserInfoMainDeptModel();

                        approversConfig.APPROVER_DEPT_ID = DTL.DEPT_ID;
                        if (!String.IsNullOrEmpty(DTL.OFFICE_ID) || !String.IsNullOrWhiteSpace(DTL.OFFICE_ID)) approversConfig.APPROVER_DEPT_ID = DTL.OFFICE_ID;
                        if (!String.IsNullOrEmpty(DTL.GROUP_ID) || !String.IsNullOrWhiteSpace(DTL.GROUP_ID)) approversConfig.APPROVER_DEPT_ID = DTL.GROUP_ID;

                        if (UserInfo != null || UserInfo.JOB_STATUS == 1)
                        {
                            var query = new LogonModel()
                            {
                                USER_ID = DTL.ACCOUNT_ID
                            };
                            var UserModel = userRepository.PostUserSingle(query).USER_MODEL;
                            approversConfig.APPROVER_ID = DTL.ACCOUNT_ID;
                            approversConfig.APPROVER_COMPANY_ID = UserModel.Where(U => U.DEPT_ID == approversConfig.APPROVER_DEPT_ID).Select(U => U.COMPANY_ID).FirstOrDefault();
                            approversConfig.APPROVER_NAME = UserModel.Where(U => U.USER_ID == approversConfig.APPROVER_ID).Select(U => U.USER_NAME).FirstOrDefault();
                            strJson = jsonFunction.ObjectToJSON(approversConfig);
                            strJson = strJson.Replace("APPROVER_ID", "USER_ID");
                            strJson = strJson.Replace("APPROVER_DEPT_ID", "DEPT_ID");
                            strJson = strJson.Replace("APPROVER_COMPANY_ID", "COMPANY_ID");
                            userInfoMainDeptModel = jsonFunction.JsonToObject<UserInfoMainDeptModel>(strJson);
                        }
                        else
                        {
                            var query = new SetUserApproverQueryModel()
                            {
                                USER_ID = DTL.ACCOUNT_ID,
                                IDENTIFY = IDENTIFY
                            };
                            var SetUserApproverInfo = userRepository.PostSetUserApproverSingle(query).Select(U => U).FirstOrDefault();
                            if (SetUserApproverInfo != null)
                            {
                                approversConfig.APPROVER_ID = null;
                                if (sysCommonRepository.GetGPIDeptTree().Where(T => T.DEPT_ID == approversConfig.APPROVER_COMPANY_ID).Count() > 0) approversConfig.APPROVER_COMPANY_ID = "GPI";
                                else approversConfig.APPROVER_COMPANY_ID = "RootCompany";
                                approversConfig.APPROVER_NAME = SetUserApproverInfo.USER_NAME;
                                strJson = jsonFunction.ObjectToJSON(approversConfig);
                                strJson = strJson.Replace("APPROVER_ID", "USER_ID");
                                strJson = strJson.Replace("APPROVER_DEPT_ID", "DEPT_ID");
                                strJson = strJson.Replace("APPROVER_COMPANY_ID", "COMPANY_ID");
                                userInfoMainDeptModel = jsonFunction.JsonToObject<UserInfoMainDeptModel>(strJson);
                                approversConfig.APPROVER_ID = DTL.ACCOUNT_ID;
                            }
                        }

                        approversConfig.APPROVER_DEPT_MAIN_ID = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).MAIN_DEPT.DEPT_ID;

                        enterpriseTaxiReviewApproversConfigs.Add(approversConfig);

                        #endregion

                        i++;
                    });

                    #endregion

                    #region - 企業乘車對帳單 乘車人員: EnterpriseTaxiReview_D 執行 -

                    var parameterApprovers = new List<SqlParameter>()
                    {
                        //企業乘車對帳單 乘車人員
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@APPROVER_COMPANY_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVER_DEPT_MAIN_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    var CommonApprovers = new BPMCommonModel<EnterpriseTaxiReviewApproversConfig>()
                    {
                        EXT = "D",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterApprovers,
                        MODEL = enterpriseTaxiReviewApproversConfigs
                    };
                    commonRepository.PutApproverFunction(CommonApprovers);

                    #endregion

                }

                #endregion

                #region - 企業乘車對帳單 使用預算：EnterpriseTaxiReview_BUDG -

                var parameterBudgets = new List<SqlParameter>()
                {
                    //企業乘車對帳單 使用預算
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
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (model.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG != null && model.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG.Count > 0)
                {
                    var CommonBUDG = new BPMCommonModel<EnterpriseTaxiReviewBudgetsConfig>()
                    {
                        EXT = "BUDG",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterBudgets,
                        MODEL = model.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG
                    };
                    commonRepository.PutBudgetFunction(CommonBUDG);
                }

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
                CommLib.Logger.Error("企業乘車對帳單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 企業乘車對帳單(審核)
        /// </summary>        
        public bool PutEnterpriseTaxiReviewApproveSingle(EnterpriseTaxiReviewDetailsViewModel model)
        {
            bool vResult = false;
            try
            {
                strREQ = model.REQUISITION_ID;

                if (!String.IsNullOrEmpty(strREQ) || !String.IsNullOrWhiteSpace(strREQ))
                {
                    #region - 企業乘車對帳單 乘車明細: EnterpriseTaxiReview_DTL -

                    var parameterDetails = new List<SqlParameter>()
                    {
                        //企業乘車對帳單 乘車明細
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GET_ON_DATE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GET_OFF_DATE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GET_ON_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GET_OFF_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GET_ON_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@GET_OFF_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COMPLEMENT_GET_ON_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COMPLEMENT_GET_OFF_PLACE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TRAVEL_BY_PURPOSE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    if (model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG != null && model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG.Count > 0)
                    {
                        model.ENTERPRISE_TAXI_REVIEW_DTLS_CONFIG.ForEach(DTL =>
                        {
                            strJson = jsonFunction.ObjectToJSON(DTL);
                            GlobalParameters.Infoparameter(strJson, parameterDetails);

                            strSQL = "";
                            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_DTL] ";
                            strSQL += "SET [GetOnDate]=@GET_ON_DATE, ";
                            strSQL += "     [GetOffDate]=@GET_OFF_DATE, ";
                            strSQL += "     [GetOnTime]=@GET_ON_TIME, ";
                            strSQL += "     [GetOffTime]=@GET_OFF_TIME, ";
                            strSQL += "     [GetOnPlace]=@GET_ON_PLACE, ";
                            strSQL += "     [GetOffPlace]=@GET_OFF_PLACE, ";
                            strSQL += "     [ComplementGetOnPlace]=@COMPLEMENT_GET_ON_PLACE, ";
                            strSQL += "     [ComplementGetOffPlace]=@COMPLEMENT_GET_OFF_PLACE, ";
                            strSQL += "     [TravelByPurpose]=@TRAVEL_BY_PURPOSE, ";
                            strSQL += "     [ProjectFormNo]=@PROJECT_FORM_NO, ";
                            strSQL += "     [ProjectName]=@PROJECT_NAME, ";
                            strSQL += "     [ProjectNickname]=@PROJECT_NICKNAME, ";
                            strSQL += "     [ProjectUseYear]=@PROJECT_USE_YEAR ";
                            strSQL += "WHERE 1=1 ";
                            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                            strSQL += "         AND [RowNo]=@ROW_NO ";
                            dbFun.DoTran(strSQL, parameterDetails);
                        });
                    }
                    #endregion

                    #region - 企業乘車對帳單 使用預算：EnterpriseTaxiReview_BUDG -

                    var parameterBudgets = new List<SqlParameter>()
                    {
                        //企業乘車對帳單 使用預算
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
                        new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    if (model.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG != null && model.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG.Count > 0)
                    {
                        var CommonBUDG = new BPMCommonModel<EnterpriseTaxiReviewBudgetsConfig>()
                        {
                            EXT = "BUDG",
                            IDENTIFY = IDENTIFY,
                            PARAMETER = parameterBudgets,
                            MODEL = model.ENTERPRISE_TAXI_REVIEW_BUDGS_CONFIG
                        };
                        commonRepository.PutBudgetFunction(CommonBUDG);
                    }

                    #endregion

                    vResult = true;

                }
                else
                {
                    vResult = false;
                    CommLib.Logger.Error("企業乘車對帳單(審核)失敗，原因：未帶入【系統編號】");
                }
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("企業乘車對帳單(審核)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "EnterpriseTaxiReview";

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