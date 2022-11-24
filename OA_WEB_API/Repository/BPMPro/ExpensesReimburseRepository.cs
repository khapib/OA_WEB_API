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
    public class ExpensesReimburseRepository
    {
        //#region - 宣告 -

        //dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        //#region Repository

        //FormRepository formRepository = new FormRepository();
        //CommonRepository commonRepository = new CommonRepository();

        //#endregion

        //#endregion

        //#region - 方法 -

        ///// <summary>
        ///// 費用申請單(查詢)
        ///// </summary>
        //public ExpensesReimburseViewModel PostExpensesReimburseSingle(ExpensesReimburseQueryModel query)
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
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_ExpensesReimburse_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

        //    #endregion

        //    #region - 費用申請單 表頭資訊 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [FlowName] AS [FLOW_NAME], ";
        //    strSQL += "     [FormNo] AS [FORM_NO], ";
        //    strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
        //    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_ExpensesReimburse_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var expensesReimburseTitle = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseTitle>().FirstOrDefault();

        //    #endregion

        //    #region - 費用申請單 表單內容 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [REIMB_StaffDeptID] AS [REIMB_STAFF_DEPT_ID], ";
        //    strSQL += "     [REIMB_StaffDeptName] AS [REIMB_STAFF_DEPT_NAME], ";
        //    strSQL += "     [REIMB_StaffID] AS [REIMB_STAFF_ID], ";
        //    strSQL += "     [REIMB_StaffName] AS [REIMB_STAFF_NAME], ";
        //    strSQL += "     [PayMethod] AS [PAY_METHOD], ";
        //    strSQL += "     [BFCY_AccountNo] AS [BFCY_ACCOUNT_NO], ";
        //    strSQL += "     [BFCY_AccountName] AS [BFCY_ACCOUNT_NAME], ";
        //    strSQL += "     [BFCY_BankNo] AS [BFCY_BANK_NO], ";
        //    strSQL += "     [BFCY_BankName] AS [BFCY_BANK_NAME], ";
        //    strSQL += "     [CurrencyName] AS [CURRENCY_NAME], ";
        //    strSQL += "     [BFCY_Name] AS [BFCY_NAME], ";
        //    strSQL += "     [BFCY_TEL] AS [BFCY_TEL] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_ExpensesReimburse_M] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //    var expensesReimburseConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseConfig>().FirstOrDefault();

        //    #endregion

        //    #region - 費用申請單 費用明細 -

        //    strSQL = "";
        //    strSQL += "SELECT ";
        //    strSQL += "     [INV_Num] AS [INV_NUM], ";
        //    strSQL += "     [ItemName] AS [ITEM_NAME], ";
        //    strSQL += "     [ItemType] AS [ITEM_TYPE], ";
        //    strSQL += "     [Reason] AS [REASON], ";
        //    strSQL += "     [INV_Amount] AS [INV_AMOUNT], ";
        //    strSQL += "     [Currency] AS [CURRENCY], ";
        //    strSQL += "     [ProjectFormNo] AS [PROJECT_FORM_NO], ";
        //    strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
        //    strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
        //    strSQL += "     [ProjectUseYear] AS [PROJECT_USE_YEAR] ";
        //    strSQL += "FROM [BPMPro].[dbo].[FM7T_ExpensesReimburse_D] ";
        //    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
        //    var expensesReimburseDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<ExpensesReimburseDetailsConfig>();


        //    #endregion

        //    var expensesReimburseViewModel = new ExpensesReimburseViewModel()
        //    {
        //        APPLICANT_INFO = applicantInfo,
        //        EXPENSES_REIMBURSE_TITLE= expensesReimburseTitle,
        //        EXPENSES_REIMBURSE_CONFIG= expensesReimburseConfig,
        //        EXPENSES_REIMBURSE_DETAILS_CONFIG= expensesReimburseDetailsConfig,
        //    };

        //    return expensesReimburseViewModel;
        //}

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

        ///// <summary>
        ///// 費用申請單(新增/修改/草稿)
        ///// </summary>
        //public bool PutExpensesReimburseSingle(ExpensesReimburseViewModel model)
        //{
        //    bool vResult = false;
        //    try
        //    {
        //        #region - 宣告主旨 -

        //        FM7Subject = model.EXPENSES_REIMBURSE_TITLE.FM7_SUBJECT;

        //        #endregion

        //        #region - 費用申請單 表頭資訊：ExpensesReimburse_M -

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
        //            //費用申請單 表頭
        //            new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.EXPENSES_REIMBURSE_TITLE.FLOW_NAME ?? DBNull.Value },
        //            new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.EXPENSES_REIMBURSE_TITLE.FORM_NO ?? DBNull.Value },
        //            new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },                };

        //        strSQL = "";
        //        strSQL += "SELECT ";
        //        strSQL += "      [RequisitionID] ";
        //        strSQL += "FROM [BPMPro].[dbo].[FM7T_ExpensesReimburse_M] ";
        //        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //        var dtA = dbFun.DoQuery(strSQL, parameterTitle);

        //        if (dtA.Rows.Count > 0)
        //        {
        //            #region - 修改 -

        //            strSQL = "";
        //            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_ExpensesReimburse_M] ";
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
        //            strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_ExpensesReimburse_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
        //            strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

        //            dbFun.DoTran(strSQL, parameterTitle);

        //            #endregion
        //        }

        //        #endregion

        //        #region - 費用申請單 表單內容：ExpensesReimburse_M -

        //        if (model.EXPENSES_REIMBURSE_CONFIG != null)
        //        {
        //            var parameterInfo = new List<SqlParameter>()
        //            {
        //                //費用申請單 表單內容
        //                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
        //                new SqlParameter("@REIMB_STAFF_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REIMB_STAFF_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REIMB_STAFF_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@REIMB_STAFF_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@PAY_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@BFCY_ACCOUNT_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@BFCY_ACCOUNT_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@BFCY_BANK_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@BFCY_BANK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
        //                new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
        //            };

        //            //寫入：費用申請單 表單內容parameter                        
        //            strJson = jsonFunction.ObjectToJSON(model.EXPENSES_REIMBURSE_CONFIG);
        //            GlobalParameters.Infoparameter(strJson, parameterInfo);

        //            strSQL = "";
        //            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_ExpensesReimburse_M] ";
        //            strSQL += "SET [REIMB_StaffDeptID]=@REIMB_STAFF_DEPT_ID, ";
        //            strSQL += "     [REIMB_StaffDeptName]=@REIMB_STAFF_DEPT_NAME, ";
        //            strSQL += "     [REIMB_StaffID]=@REIMB_STAFF_ID, ";
        //            strSQL += "     [REIMB_StaffName]=@REIMB_STAFF_NAME, ";
        //            strSQL += "     [PayMethod]=@PAY_METHOD, ";
        //            strSQL += "     [BFCY_AccountNo]=@BFCY_ACCOUNT_NO, ";
        //            strSQL += "     [BFCY_AccountName]=@BFCY_ACCOUNT_NAME, ";
        //            strSQL += "     [BFCY_BankNo]=@BFCY_BANK_NO, ";
        //            strSQL += "     [BFCY_BankName]=@BFCY_BANK_NAME, ";
        //            strSQL += "     [CurrencyName]=@CURRENCY_NAME, ";
        //            strSQL += "     [BFCY_Name]=@BFCY_NAME, ";
        //            strSQL += "     [BFCY_TEL]=@BFCY_TEL ";
        //            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

        //            dbFun.DoTran(strSQL, parameterInfo);

        //        }

        //        #endregion

        //        #region - 費用申請單 費用明細: ExpensesReimburse_D -

        //        var parameterDetails = new List<SqlParameter>()
        //        {
        //            //費用申請單 費用明細
        //            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
        //            new SqlParameter("@INV_NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@ITEM_TYPE", SqlDbType.NVarChar) { Size = 100 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@INV_AMOUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@CURRENCY", SqlDbType.NVarChar) { Size = 10 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@PROJECT_FORM_NO", SqlDbType.NVarChar) { Size = 20 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 500 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 4000 , Value = (object)DBNull.Value ?? DBNull.Value },
        //            new SqlParameter("@PROJECT_USE_YEAR", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
        //        };

        //        #region 先刪除舊資料

        //        strSQL = "";
        //        strSQL += "DELETE ";
        //        strSQL += "FROM [BPMPro].[dbo].[FM7T_ExpensesReimburse_D] ";
        //        strSQL += "WHERE 1=1 ";
        //        strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

        //        dbFun.DoTran(strSQL, parameterDetails);

        //        #endregion

        //        if (model.EXPENSES_REIMBURSE_DETAILS_CONFIG != null && model.EXPENSES_REIMBURSE_DETAILS_CONFIG.Count > 0)
        //        {
        //            #region 再新增資料

        //            foreach (var item in model.EXPENSES_REIMBURSE_DETAILS_CONFIG)
        //            {
        //                //寫入：費用申請單 費用明細parameter
        //                strJson = jsonFunction.ObjectToJSON(item);
        //                GlobalParameters.Infoparameter(strJson, parameterDetails);

        //                strSQL = "";
        //                strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_ExpensesReimburse_D]([RequisitionID],[INV_Num],[ItemName],[ItemType],[Reason],[INV_Amount],[Currency],[ProjectFormNo],[ProjectName],[ProjectNickname],[ProjectUseYear]) ";
        //                strSQL += "VALUES(@REQUISITION_ID,@INV_NUM,@ITEM_NAME,@ITEM_TYPE,@REASON,@INV_AMOUNT,@CURRENCY,@PROJECT_FORM_NO,@PROJECT_NAME,@PROJECT_NICKNAME,@PROJECT_USE_YEAR) ";

        //                dbFun.DoTran(strSQL, parameterDetails);
        //            }

        //            #endregion
        //        }

        //        #endregion

        //        #region - 表單主旨：FormHeader -

        //        FormHeader header = new FormHeader();
        //        header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
        //        header.ITEM_NAME = "Subject";
        //        header.ITEM_VALUE = FM7Subject;

        //        formRepository.PutFormHeader(header);

        //        #endregion

        //        #region - 儲存草稿：FormDraftList -

        //        if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
        //        {
        //            FormDraftList draftList = new FormDraftList();
        //            draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
        //            draftList.IDENTIFY = IDENTIFY;
        //            draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

        //            formRepository.PutFormDraftList(draftList, true);
        //        }

        //        #endregion

        //        #region - 送出表單：FormAutoStart -

        //        if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
        //        {
        //            #region 送出表單前，先刪除草稿清單

        //            FormDraftList draftList = new FormDraftList();
        //            draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
        //            draftList.IDENTIFY = IDENTIFY;
        //            draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

        //            formRepository.PutFormDraftList(draftList, false);

        //            #endregion

        //            FormAutoStart autoStart = new FormAutoStart();
        //            autoStart.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
        //            autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
        //            autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
        //            autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

        //            formRepository.PutFormAutoStart(autoStart);
        //        }

        //        #endregion

        //        vResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("費用申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        //#endregion

        //#region - 欄位和屬性 -

        ///// <summary>
        ///// T-SQL
        ///// </summary>
        //private string strSQL;

        ///// <summary>
        ///// 表單代號
        ///// </summary>
        //private string IDENTIFY = "ExpensesReimburse";

        ///// <summary>
        ///// 表單主旨
        ///// </summary>
        //private string FM7Subject;

        ///// <summary>
        ///// Json字串
        ///// </summary>
        //private string strJson;

        //#endregion
    }
}