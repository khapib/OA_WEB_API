using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.SqlServer.Server;
using Microsoft.Ajax.Utilities;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購請款單
    /// </summary>
    public class MediaInvoiceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        FlowRepository flowRepository = new FlowRepository();

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

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



            #endregion

            #region - 版權採購請款單 驗收明細 -



            #endregion

            #region - 版權採購請款單 授權權利 -

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_AUTH] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoiceAuthorizesConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceAuthorizesConfig>();

            #endregion

            #region - 版權採購申請單 額外項目 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [EX_RowNo] AS [EX_ROW_NO], ";
            strSQL += "     [EX_Name] AS [EX_NAME], ";
            strSQL += "     [EX_Amount] AS [EX_AMOUNT], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [EX_ProjectFormNo] AS [EX_PROJECT_FORM_NO], ";
            strSQL += "     [EX_ProjectName] AS [EX_PROJECT_NAME], ";
            strSQL += "     [EX_ProjectNickname] AS [EX_PROJECT_NICKNAME], ";
            strSQL += "     [EX_ProjectUseYear] AS [EX_PROJECT_USE_YEAR], ";
            strSQL += "     [EX_Note] AS [EX_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_EX] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoiceExtrasConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceExtrasConfig>();

            #endregion

            #region - 版權採購請款單 付款辦法 -



            #endregion

            #region - 版權採購請款單 使用預算 -

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrder_BUDG] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaInvoiceBudgetsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaInvoiceBudgetsConfig>();

            #endregion

            #region - 版權採購請款單 發票明細 -



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

            };

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
                    //版權採購請款單 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

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
                    strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";
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



                #endregion

                #region - 版權採購請款單 驗收明細: MediaInvoice_ACPT -

                //View 是執行
                //版權採購的 驗收明細(MediaInvoice_ACPT)

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
                    //行政採購申請 付款辦法 更新:會計類別
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
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaInvoice_PYMT] ";
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

                #region - 版權採購請款單 發票明細：MediaInvoice_INV -

                var parameterDetails = new List<SqlParameter>()
                {
                    //版權採購請款單 發票明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)medialOrderformData.SERIAL_ID ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NUM", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_DATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@INV_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_INV] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_INVOICE_DTLS_CONFIG != null && model.MEDIA_INVOICE_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_INVOICE_DTLS_CONFIG)
                    {
                        #region - 確認小數點後第二位 -

                        item.INV_AMOUNT = Math.Round(item.INV_AMOUNT, 2);

                        #endregion

                        //寫入：版權採購請款 發票明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaInvoice_INV]([RequisitionID],[Period],[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[MediaOrderERPFormNo],[INV_Num],[INV_Date],[INV_Amount],[INV_Amount_TWD],[INV_Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PERIOD,@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@MEDIA_ORDER_ERP_FORM_NO,@INV_NUM,@INV_DATE,@INV_AMOUNT,@INV_AMOUNT_TWD,@INV_NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
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

                #region 關聯表:加上【版權採購點驗收單】

                if (!String.IsNullOrEmpty(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID))
                {
                    var medialAcceptanceformQueryModel = new FormQueryModel()
                    {
                        REQUISITION_ID = model.MEDIA_INVOICE_CONFIG.MEDIA_ACCEPTANCE_REQUISITION_ID
                    };
                    var medialAcceptanceformData = formRepository.PostFormData(medialAcceptanceformQueryModel);

                    importAssociatedForm.Add(new AssociatedFormConfig()
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

                var associatedFormConfig = model.ASSOCIATED_FORM_CONFIG;
                if (associatedFormConfig == null || associatedFormConfig.Count <= 0)
                {
                    associatedFormConfig = importAssociatedForm;
                }

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfig
                };

                //寫入「關聯表單」
                commonRepository.PutAssociatedForm(associatedFormModel);

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
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
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

                    formRepository.PutFormAutoStart(autoStart);
                }

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
                strSQL += "      [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
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

        #endregion
    }
}