using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;

using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 合作夥伴審核單
    /// </summary>
    public class SupplierReviewRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();

        #endregion

        #region 功能

        JavaScriptSerializer jss = new JavaScriptSerializer();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 合作夥伴審核單(查詢)
        /// </summary>
        public SupplierReviewViewModel PostSupplierReviewSingle(SupplierReviewQueryModel query)
        {
            #region - 申請人資訊 -

            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 合作夥伴審核單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [Approve] AS [APPROVE], ";
            strSQL += "     [SupNo] AS [SUP_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var supplierReviewTitle = dbFun.DoQuery(strSQL, parameterA).ToList<SupplierReviewTitle>().FirstOrDefault();

            #endregion

            #region - 合作夥伴審核單 基本資料 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     M2.[SupName] AS [SUP_NAME], ";
            strSQL += "     M2.[RegisterKind] AS [REG_KIND], ";
            strSQL += "     M2.[RegisterNo] AS [REG_NO], ";
            strSQL += "     M2.[IS_SUP_Partner] AS [IS_SUP_PARTNER], ";
            strSQL += "     M2.[IS_CUST_Partner] AS [IS_CUST_PARTNER],";
            strSQL += "     M2.[IS_AD_Partner] AS [IS_AD_PARTNER], ";
            strSQL += "     M2.[CountryName] AS [COUNTRY_NAME], ";
            strSQL += "     M2.[RegisterYear] AS [REG_YEAR], ";
            strSQL += "     M2.[RegisterCapital] AS [REG_CAPITAL], ";
            strSQL += "     M2.[NoOfEmployee] AS [NO_OF_EMPLOYEE], ";
            strSQL += "     M2.[OwnerName] AS [OWNER_NAME], ";
            strSQL += "     M2.[OwnerTEL] AS [OWNER_TEL], ";
            strSQL += "     M2.[RegisterTEL] AS [REG_TEL], ";
            strSQL += "     M2.[RegisterWeb] AS [REG_WEB], ";
            strSQL += "     M2.[RegisterAddress] AS [REG_ADDRESS], ";
            strSQL += "     M2.[CurrentAddress] AS [CURRENT_ADDRESS], ";
            strSQL += "     M.[SupplierReviewDiff] AS [SUPPLIER_REVIEW_DIFF] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_M2] AS M2 ";
            strSQL += "     INNER JOIN [BPMPro].[dbo].[FM7T_SupplierReview_M] AS M ON M.[RequisitionID]=M2.[RequisitionID] ";
            strSQL += "WHERE M2.[RequisitionID]=@REQUISITION_ID ";

            var supplierReviewDataSetModel = dbFun.DoQuery(strSQL, parameterA).ToList<SupplierReviewDataSetModel>().FirstOrDefault();
            var supplierReviewDifference = new SupplierReviewDifference();
            if (supplierReviewDataSetModel != null)
            {
                //差異資訊
                strJson = jsonFunction.ObjectToJSON(supplierReviewDataSetModel);
                supplierReviewDifference = jsonFunction.JsonToObject<SupplierReviewDifference>(strJson);
                if (supplierReviewDataSetModel.SUPPLIER_REVIEW_DIFF != null)
                {
                    var DiffJson = jsonFunction.JsonToObject<IList<DifferenceInfo>>(supplierReviewDataSetModel.SUPPLIER_REVIEW_DIFF);
                    supplierReviewDifference.DIFF = DiffJson;
                }
            }

            #endregion

            #region - 合作夥伴審核 銀行往來資訊 -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     D2.[StatusFlg] AS [STATUS_FLG], ";
            strSQL += "     D2.[SupTXTempId] AS [SUP_TX_TEMP_ID], ";
            strSQL += "     D2.[SupTXId] AS [SUP_TX_ID], ";
            strSQL += "     D2.[TX_Category] AS [TX_CATEGORY], ";
            strSQL += "     D2.[BFCY_AccountNo] AS [BFCY_AC_NO], ";
            strSQL += "     D2.[BFCY_AccountName] AS [BFCY_AC_NAME], ";
            strSQL += "     D2.[BFCY_BankNo] AS [BFCY_BK_NO], ";
            strSQL += "     D2.[BFCY_BankName] AS [BFCY_BK_NAME], ";
            strSQL += "     D2.[BFCY_BanKBranchNo] AS [BFCY_BK_BRANCH_NO], ";
            strSQL += "     D2.[BFCY_BanKBranchName] AS [BFCY_BK_BRANCH_NAME], ";
            strSQL += "     D2.[BFCY_BankSWIFT] AS [BFCY_BK_SWIFT], ";
            strSQL += "     D2.[BFCY_BankAddress] AS [BFCY_BK_ADDRESS],";
            strSQL += "     D2.[BFCY_BankCountryAndCity] AS [BFCY_BK_COUNTRY_AND_CITY],";
            strSQL += "     D2.[BFCY_BankIBAN] AS [BFCY_BK_IBAN],";
            strSQL += "     D2.[CurrencyName] AS [CURRENCY_NAME], ";
            strSQL += "     D2.[BFCY_Name] AS [BFCY_NAME],";
            strSQL += "     D2.[BFCY_TEL] AS [BFCY_TEL],";
            strSQL += "     D2.[BFCY_Email] AS [BFCY_EMAIL],";
            strSQL += "     D.[RemitDiff] AS [REMIT_DIFF]";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_D2] AS D2 ";
            strSQL += "     LEFT JOIN [BPMPro].[dbo].[FM7T_SupplierReview_D] AS D ON D.[RequisitionID]=D2.[RequisitionID] AND D.[SupTXId]=D2.[SupTXId] ";
            strSQL += "WHERE D2.[RequisitionID]=@REQUISITION_ID";

            var supplierReviewRemitDataSetList = dbFun.DoQuery(strSQL, parameterB).ToList<SupplierReviewRemitDataSetModel>();
            var supplierReviewRemitDifference = new List<SupplierReviewRemitDifference>();

            if (supplierReviewRemitDataSetList != null)
            {
                strJson = jsonFunction.ObjectToJSON(supplierReviewRemitDataSetList);
                supplierReviewRemitDifference = jsonFunction.JsonToObject<List<SupplierReviewRemitDifference>>(strJson);

                foreach (var dbRemit in supplierReviewRemitDataSetList)
                {
                    supplierReviewRemitDifference.ForEach(view =>
                    {
                        switch (view.TX_CATEGORY)
                        {
                            case "DT":
                                view.TX_CATEGORY = "國內臺幣";
                                break;
                            case "DF":
                                view.TX_CATEGORY = "國內外幣";
                                break;
                            case "DD":
                                view.TX_CATEGORY = "開票";
                                break;
                            case "FF":
                                view.TX_CATEGORY = "國外電匯";
                                break;
                            default:
                                break;
                        }

                        if (dbRemit.REMIT_DIFF != null)
                        {
                            //差異資訊
                            var DiffJson = jsonFunction.JsonToObject<IList<DifferenceInfo>>(dbRemit.REMIT_DIFF);
                            var TempRemitobject = supplierReviewRemitDifference
                                                    .Where(v => v.SUP_TX_ID == dbRemit.SUP_TX_ID)
                                                    .FirstOrDefault();
                            TempRemitobject.DIFF = DiffJson;
                        }
                    });
                }
            }
            #endregion

            #region - 合作夥伴審核 附件 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var attachment = commonRepository.PostAttachment(formQueryModel);

            #endregion

            var supplierReviewView = new SupplierReviewViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                SUPPLIER_REVIEW_TITLE = supplierReviewTitle,
                SUPPLIER_REVIEW_CONFIG = supplierReviewDifference,
                SUPPLIER_REVIEW_REMIT_CONFIG = supplierReviewRemitDifference,
                ATTACHMENT_CONFIG = attachment
            };

            return supplierReviewView;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 合作夥伴審核單(依此單內容重送)(僅外部起單使用)
        ///// </summary>
        //public bool PutSupplierReviewRefill(SupplierReviewQueryModel query)
        //{



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("跑馬申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 合作夥伴審核單(新增/修改/草稿)
        /// </summary>
        public bool PutSupplierReviewSingle(SupplierReviewDataViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                Dictionary<string, string> InfoDictionary = null;
                Dictionary<string, string> TempInfoDictionary = null;
                Dictionary<string, string> RemitInfoDictionary = null;
                Dictionary<string, string> TempRemitInfoDictionary = null;

                #region - 初始審核單性質 -

                if (String.IsNullOrEmpty(model.SUPPLIER_REVIEW_TITLE.APPROVE) || String.IsNullOrWhiteSpace(model.SUPPLIER_REVIEW_TITLE.APPROVE))
                {
                    //沒有值；則會寫入(新增)
                    model.SUPPLIER_REVIEW_TITLE.APPROVE = "新增";
                }

                #endregion

                //主旨
                FM7Subject = model.SUPPLIER_REVIEW_TITLE.APPROVE + "-" + model.SUPPLIER_REVIEW_TEMP_CONFIG.SUP_NAME;

                #endregion

                #region - 合作夥伴審核 表頭資訊：SupplierReview_M -

                var parameterTitle = new List<SqlParameter>()
                {
                    //申請人資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = model.APPLICANT_INFO.PRIORITY },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = model.APPLICANT_INFO.DRAFT_FLAG },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = model.APPLICANT_INFO.FLOW_ACTIVATED },
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //合作夥伴審核單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.SUPPLIER_REVIEW_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@APPROVE", SqlDbType.NVarChar) { Size = 4, Value = (object)model.SUPPLIER_REVIEW_TITLE.APPROVE ?? DBNull.Value },
                    new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)model.SUPPLIER_REVIEW_TITLE.SUP_NO ?? DBNull.Value },
                };

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_SupplierReview_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "    [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "    [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "    [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "    [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "    [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "    [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "    [FillerID]=@FILLER_ID, ";
                    strSQL += "    [FillerName]=@FILLER_NAME, ";
                    strSQL += "    [Priority]=@PRIORITY, ";
                    strSQL += "    [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "    [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "    [Approve]=@APPROVE,";
                    strSQL += "    [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "    [FormNo]=@FORM_NO, ";
                    strSQL += "    [SupNo]=@SUP_NO ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_SupplierReview_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[FillerID],[FillerName],[ApplicantPhone],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[Approve],[FormNo],[SupNo]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@FILLER_ID,@FILLER_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@APPROVE,@FORM_NO,@SUP_NO) ";
                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                var parameterInfo = new List<SqlParameter>()
                {
                    //合作夥伴審核單(基本資料)
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_SUP_PARTNER", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_CUST_PARTNER", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_AD_PARTNER", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COUNTRY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_YEAR", SqlDbType.NVarChar) { Size = 10, Value =(object) DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_CAPITAL", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NO_OF_EMPLOYEE", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_TEL", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_TEL", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_WEB", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REG_ADDRESS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CURRENT_ADDRESS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region - 合作夥伴審核 基本資料(已審核)：SupplierReview_M -

                if (model.SUPPLIER_REVIEW_CONFIG != null)
                {
                    //寫入：合作夥伴審核 基本資料parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.SUPPLIER_REVIEW_CONFIG);
                    InfoDictionary = jss.Deserialize<Dictionary<string, string>>(strJson);
                    InfoDictionary.Remove("DIFF");
                    strJson = jsonFunction.ObjectToJSON(InfoDictionary);

                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_SupplierReview_M] ";
                    strSQL += "SET [SupName]=@SUP_NAME, ";
                    strSQL += "    [RegisterKind]=@REG_KIND, ";
                    strSQL += "    [RegisterNo]=@REG_NO, ";
                    strSQL += "    [IS_SUP_Partner]=@IS_SUP_PARTNER, ";
                    strSQL += "    [IS_CUST_Partner]=@IS_CUST_PARTNER, ";
                    strSQL += "    [IS_AD_Partner]=@IS_AD_PARTNER, ";
                    strSQL += "    [CountryName]=@COUNTRY_NAME, ";
                    strSQL += "    [RegisterYear]=@REG_YEAR, ";
                    strSQL += "    [RegisterCapital]=@REG_CAPITAL, ";
                    strSQL += "    [NoOfEmployee]=@NO_OF_EMPLOYEE, ";
                    strSQL += "    [OwnerName]=@OWNER_NAME, ";
                    strSQL += "    [OwnerTEL]=@OWNER_TEL, ";
                    strSQL += "    [RegisterTEL]=@REG_TEL, ";
                    strSQL += "    [RegisterWeb]=@REG_WEB, ";
                    strSQL += "    [RegisterAddress]=@REG_ADDRESS, ";
                    strSQL += "    [CurrentAddress]=@CURRENT_ADDRESS ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);
                }

                #endregion

                #region - 合作夥伴審核 基本資料(修改後/新增)：SupplierReview_M2 -

                //寫入：合作夥伴審核 基本資料parameter                        
                strJson = jsonFunction.ObjectToJSON(model.SUPPLIER_REVIEW_TEMP_CONFIG);
                TempInfoDictionary = jss.Deserialize<Dictionary<string, string>>(strJson);
                strJson = jsonFunction.ObjectToJSON(TempInfoDictionary);

                GlobalParameters.Infoparameter(strJson, parameterInfo);

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_M2] ";
                strSQL += "WHERE 1=1";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterInfo);

                #endregion

                #region 再新增資料

                strSQL = "";
                strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_SupplierReview_M2]([RequisitionID],[SupName],[RegisterKind],[RegisterNo],[IS_SUP_Partner],[IS_CUST_Partner],[IS_AD_Partner],[CountryName],[RegisterYear],[RegisterCapital],[NoOfEmployee],[OwnerName],[OwnerTEL],[RegisterTEL],[RegisterWeb],[RegisterAddress],[CurrentAddress]) ";
                strSQL += "VALUES(@REQUISITION_ID,@SUP_NAME,@REG_KIND,@REG_NO,@IS_SUP_PARTNER,@IS_CUST_PARTNER,@IS_AD_PARTNER,@COUNTRY_NAME,@REG_YEAR,@REG_CAPITAL,@NO_OF_EMPLOYEE,@OWNER_NAME,@OWNER_TEL,@REG_TEL,@REG_WEB,@REG_ADDRESS,@CURRENT_ADDRESS) ";

                dbFun.DoTran(strSQL, parameterInfo);

                #endregion

                #endregion

                #region - 合作夥伴審核 基本資料(差異) -

                if (!model.SUPPLIER_REVIEW_TITLE.APPROVE.Contains("新增"))
                {
                    if (model.SUPPLIER_REVIEW_CONFIG != null)
                    {
                        var parameterDiff = new List<SqlParameter>()
                        {
                            //合作夥伴審核單(異動)註記
                            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                            new SqlParameter("@SUPPLIER_REVIEW_DIFF", SqlDbType.NVarChar) { Size = 1000, Value = (object)DBNull.Value ?? DBNull.Value },
                        };

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_SupplierReview_M] ";
                        strSQL += "SET [SupplierReviewDiff] = @SUPPLIER_REVIEW_DIFF ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                        //先清除欄位
                        dbFun.DoTran(strSQL, parameterDiff);

                        #region 差異欄位

                        //取出後寫入差異列表
                        var DifferenceInfoKey = TempInfoDictionary
                                                    .Where(t => !InfoDictionary.ContainsValue(t.Value))
                                                    .Select(t => t.Key)
                                                    .ToList();
                        var DifferenceInfoValue = InfoDictionary
                                                    .Where(o => DifferenceInfoKey.Contains(o.Key))
                                                    .Select(o => o.Value)
                                                    .ToList();

                        var DifferenceInfolist = new List<DifferenceInfo>();
                        for (var i = 0; i < DifferenceInfoKey.Count; i++)
                        {
                            DifferenceInfolist.Add(new DifferenceInfo() { KEY = DifferenceInfoKey[i], ORIGINAL = DifferenceInfoValue[i] });
                        }
                        var DifferenceInfolistJson = jsonFunction.ObjectToJSON(DifferenceInfolist);

                        parameterDiff[1].Value = DifferenceInfolistJson;
                        dbFun.DoTran(strSQL, parameterDiff);

                        #endregion
                    }
                }

                #endregion

                var parameterRemit = new List<SqlParameter>()
                {
                    //合作夥伴審核單(銀行往來資訊)
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@STATUS_FLG", SqlDbType.NVarChar) { Size = 1000, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@SUP_TX_TEMP_ID", SqlDbType.NVarChar) { Size = 1000, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@SUP_TX_ID", SqlDbType.NVarChar) { Size = 1000, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@TX_CATEGORY", SqlDbType.VarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_AC_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_AC_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_NO", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_BRANCH_NO", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_BRANCH_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_SWIFT", SqlDbType.NVarChar) { Size = 300, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_ADDRESS", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_COUNTRY_AND_CITY", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_BK_IBAN", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CURRENCY_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_TEL", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@BFCY_EMAIL", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                #region - 合作夥伴審核 銀行往來資訊(已審核)：SupplierReview_D -

                if (model.SUPPLIER_REVIEW_REMIT_CONFIG != null && model.SUPPLIER_REVIEW_REMIT_CONFIG.Count > 0)
                {
                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_D] ";
                    strSQL += "WHERE 1=1";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterRemit);

                    #endregion

                    #region 再新增資料

                    foreach (var item in model.SUPPLIER_REVIEW_REMIT_CONFIG)
                    {
                        //寫入：合作夥伴審核 銀行往來資訊parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        RemitInfoDictionary = jss.Deserialize<Dictionary<string, string>>(strJson);
                        RemitInfoDictionary.Remove("DIFF");
                        strJson = jsonFunction.ObjectToJSON(RemitInfoDictionary);
                        GlobalParameters.Infoparameter(strJson, parameterRemit);

                        strSQL = "";
                        strSQL += "INSERT INTO [dbo].[FM7T_SupplierReview_D]([RequisitionID],[StatusFlg],[SupTXTempId],[SupTXId],[TX_Category],[BFCY_AccountNo],[BFCY_AccountName],[BFCY_BankNo],[BFCY_BankName],[BFCY_BanKBranchNo],[BFCY_BanKBranchName],[BFCY_BankSWIFT],[BFCY_BankAddress],[BFCY_BankCountryAndCity],[BFCY_BankIBAN],[CurrencyName],[BFCY_Name],[BFCY_TEL],[BFCY_Email]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@STATUS_FLG,@SUP_TX_TEMP_ID,@SUP_TX_ID,@TX_CATEGORY,@BFCY_AC_NO,@BFCY_AC_NAME,@BFCY_BK_NO,@BFCY_BK_NAME,@BFCY_BK_BRANCH_NO,@BFCY_BK_BRANCH_NAME,@BFCY_BK_SWIFT,@BFCY_BK_ADDRESS,@BFCY_BK_COUNTRY_AND_CITY,@BFCY_BK_IBAN,@CURRENCY_NAME,@BFCY_NAME,@BFCY_TEL,@BFCY_EMAIL) ";

                        dbFun.DoTran(strSQL, parameterRemit);
                    }

                    #endregion
                }

                #endregion

                #region - 合作夥伴審核 銀行往來資訊(修改後/新增)：SupplierReview_D2 -

                if (model.SUPPLIER_REVIEW_TEMP_REMIT_CONFIG != null && model.SUPPLIER_REVIEW_TEMP_REMIT_CONFIG.Count > 0)
                {
                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_SupplierReview_D2] ";
                    strSQL += "WHERE 1=1";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterRemit);

                    #endregion

                    #region 再新增資料

                    foreach (var item in model.SUPPLIER_REVIEW_TEMP_REMIT_CONFIG)
                    {
                        //寫入：合作夥伴審核 銀行往來資訊parameter                      
                        strJson = jsonFunction.ObjectToJSON(item);
                        TempRemitInfoDictionary = jss.Deserialize<Dictionary<string, string>>(strJson);
                        strJson = jsonFunction.ObjectToJSON(TempRemitInfoDictionary);
                        GlobalParameters.Infoparameter(strJson, parameterRemit);

                        strSQL = "";
                        strSQL += "INSERT INTO [dbo].[FM7T_SupplierReview_D2]([RequisitionID],[StatusFlg],[SupTXTempId],[SupTXId],[TX_Category],[BFCY_AccountNo],[BFCY_AccountName],[BFCY_BankNo],[BFCY_BankName],[BFCY_BanKBranchNo],[BFCY_BanKBranchName],[BFCY_BankSWIFT],[BFCY_BankAddress],[BFCY_BankCountryAndCity],[BFCY_BankIBAN],[CurrencyName],[BFCY_Name],[BFCY_TEL],[BFCY_Email]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@STATUS_FLG,@SUP_TX_TEMP_ID,@SUP_TX_ID,@TX_CATEGORY,@BFCY_AC_NO,@BFCY_AC_NAME,@BFCY_BK_NO,@BFCY_BK_NAME,@BFCY_BK_BRANCH_NO,@BFCY_BK_BRANCH_NAME,@BFCY_BK_SWIFT,@BFCY_BK_ADDRESS,@BFCY_BK_COUNTRY_AND_CITY,@BFCY_BK_IBAN,@CURRENCY_NAME,@BFCY_NAME,@BFCY_TEL,@BFCY_EMAIL) ";

                        dbFun.DoTran(strSQL, parameterRemit);
                    }

                    #endregion
                }

                #endregion

                #region - 合作夥伴審核 銀行往來資訊(差異) -

                if (model.SUPPLIER_REVIEW_REMIT_CONFIG != null && model.SUPPLIER_REVIEW_REMIT_CONFIG.Count > 0)
                {
                    foreach (var item in model.SUPPLIER_REVIEW_REMIT_CONFIG)
                    {
                        var parameterDiff = new List<SqlParameter>()
                        {
                            //合作夥伴審核單(異動)註記
                            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                            new SqlParameter("@SUP_TX_ID", SqlDbType.NVarChar) { Size = 1000, Value = (object)item.SUP_TX_ID ?? DBNull.Value },
                            new SqlParameter("@REMIT_DIFF", SqlDbType.NVarChar) { Size = 1000, Value = (object)DBNull.Value ?? DBNull.Value },
                        };

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_SupplierReview_D] ";
                        strSQL += "SET [RemitDiff] = @REMIT_DIFF ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [RequisitionID]=@REQUISITION_ID";
                        strSQL += "         AND [SupTXId]=@SUP_TX_ID";

                        //先清除欄位
                        dbFun.DoTran(strSQL, parameterDiff);

                        #region 差異欄位

                        //銀行往來資訊:修改
                        var ReviseRemit = model.SUPPLIER_REVIEW_TEMP_REMIT_CONFIG
                                        .Where(rri => rri.STATUS_FLG == "Z002" && rri.SUP_TX_ID == item.SUP_TX_ID)
                                        .Select(rri => new SupplierReviewRemitConfig
                                        {
                                            STATUS_FLG = rri.STATUS_FLG,
                                            SUP_TX_TEMP_ID = rri.SUP_TX_TEMP_ID,
                                            SUP_TX_ID = rri.SUP_TX_ID,
                                            TX_CATEGORY = rri.TX_CATEGORY,
                                            BFCY_AC_NO = rri.BFCY_AC_NO,
                                            BFCY_AC_NAME = rri.BFCY_AC_NAME,
                                            BFCY_BK_NO = rri.BFCY_BK_NO,
                                            BFCY_BK_NAME = rri.BFCY_BK_NAME,
                                            BFCY_BK_BRANCH_NO = rri.BFCY_BK_BRANCH_NO,
                                            BFCY_BK_BRANCH_NAME = rri.BFCY_BK_BRANCH_NAME,
                                            BFCY_BK_SWIFT = rri.BFCY_BK_SWIFT,
                                            BFCY_BK_ADDRESS = rri.BFCY_BK_ADDRESS,
                                            BFCY_BK_COUNTRY_AND_CITY = rri.BFCY_BK_COUNTRY_AND_CITY,
                                            BFCY_BK_IBAN = rri.BFCY_BK_IBAN,
                                            CURRENCY_NAME = rri.CURRENCY_NAME,
                                            BFCY_NAME = rri.BFCY_NAME,
                                            BFCY_TEL = rri.BFCY_TEL,
                                            BFCY_EMAIL = rri.BFCY_EMAIL
                                        }).FirstOrDefault();
                        //銀行往來資訊:修改
                        var strReviseRemitDiffJson = jsonFunction.ObjectToJSON(ReviseRemit);
                        if (ReviseRemit != null)
                        {
                            var ReviseRemitDictionary = jss.Deserialize<Dictionary<string, string>>(strReviseRemitDiffJson);


                            //銀行往來資訊:已審核
                            var strRemitInfoDiffJson = jsonFunction.ObjectToJSON(item);
                            RemitInfoDictionary = jss.Deserialize<Dictionary<string, string>>(strRemitInfoDiffJson);

                            //取出後寫入差異列表
                            var DifferenceInfoRemitKey = ReviseRemitDictionary
                                                            .Where(t => !RemitInfoDictionary.Contains(t))
                                                            .Select(t => t.Key)
                                                            .ToList();
                            var DifferenceInfoRemitValue = RemitInfoDictionary
                                                            .Where(o => DifferenceInfoRemitKey.Contains(o.Key))
                                                            .Select(o => o.Value)
                                                            .ToList();

                            var DifferenceInfoRemitlist = new List<DifferenceInfo>();
                            for (var i = 0; i < DifferenceInfoRemitKey.Count; i++)
                            {
                                DifferenceInfoRemitlist.Add(new DifferenceInfo() { KEY = DifferenceInfoRemitKey[i], ORIGINAL = DifferenceInfoRemitValue[i] });
                            }
                            var DifferenceInfolistJson = jsonFunction.ObjectToJSON(DifferenceInfoRemitlist);

                            parameterDiff[2].Value = DifferenceInfolistJson;

                            dbFun.DoTran(strSQL, parameterDiff);
                        }

                        #endregion
                    }
                }

                #endregion

                #region - 合作夥伴審核 附件：Attachment -

                if (model.ATTACHMENT_CONFIG != null && model.ATTACHMENT_CONFIG.Count > 0)
                {
                    var attachmentMain = new AttachmentMain()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = IDENTIFY,
                        ATTACHMENT = model.ATTACHMENT_CONFIG
                    };

                    commonRepository.PutAttachment(attachmentMain);
                }

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

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
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
                CommLib.Logger.Error("合作夥伴審核單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "SupplierReview";

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