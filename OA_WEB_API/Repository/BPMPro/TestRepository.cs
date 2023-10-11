using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

using OA_WEB_API.Repository.ERP;
using OA_WEB_API.Models;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Models.BPMPro;

using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 測試表單
    /// </summary>
    public class TestRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Repository

        FormRepository formRepository = new FormRepository();
        GeneralOrderRepository generalOrderRepository = new GeneralOrderRepository();
        StepFlowRepository stepFlowRepository = new StepFlowRepository();
        ResponseInfoRepository responseInfoRepository = new ResponseInfoRepository();

        #endregion

        #endregion

        #region - 方法 -

        #region - TEST_01 -

        /// <summary>
        /// 測試01申請單(查詢)
        /// </summary>
        public Test01ViewModel PostTest01Single(Test01QueryModel query) 
        {
            #region - 查詢 -

            #region - 申請人資訊 -

            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     A.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     A.[DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     B.[Value] AS [FM7_SUBJECT], ";
            strSQL += "     A.[ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     A.[ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     A.[ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     NULL AS [APPLICANT_PHONE], ";
            strSQL += "     A.[ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     A.[FillerID] AS [FILLER_ID], ";
            strSQL += "     A.[FillerName] AS [FILLER_NAME], ";
            strSQL += "     A.[Priority] AS [PRIORITY], ";
            strSQL += "     A.[DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     A.[FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_01_M] A";
            strSQL += "         INNER JOIN [BPMPro].[dbo].[FSe7en_Tep_FormHeader] B ON A.[RequisitionID]=B.[RequisitionID]";
            strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 表單內容 -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Remark] AS [REMARK] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_01_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var test01Content = dbFun.DoQuery(strSQL, parameterB).ToList<Test01Content>().FirstOrDefault();

            #endregion

            #endregion

            var test01 = new Test01ViewModel();

            test01.APPLICANT_INFO = applicantInfo;
            test01.TEST_01_CONTENT = test01Content;

            return test01;
        }

        /// <summary>
        /// 測試01申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutTest01Refill(Test01QueryModel query)
        {
            bool vResult = false;

            try
            {
                #region - 宣告 -

                var original = PostTest01Single(query);

                var test01 = new Test01ViewModel();
                var applicantInfo = new ApplicantInfo();
                var test01Content = new Test01Content();
               
                var requisitionID = Guid.NewGuid().ToString();

                #endregion

                #region - 申請人資訊 -

                //表單資訊
                applicantInfo.REQUISITION_ID = requisitionID;
                applicantInfo.DIAGRAM_ID = original.APPLICANT_INFO.DIAGRAM_ID;
                applicantInfo.PRIORITY = original.APPLICANT_INFO.PRIORITY;
                applicantInfo.DRAFT_FLAG = 1;
                applicantInfo.FLOW_ACTIVATED = original.APPLICANT_INFO.FLOW_ACTIVATED;
                //表單主旨
                applicantInfo.FM7_SUBJECT = original.APPLICANT_INFO.FM7_SUBJECT;
                //申請人資訊
                applicantInfo.APPLICANT_DEPT = original.APPLICANT_INFO.APPLICANT_DEPT;
                applicantInfo.APPLICANT_DEPT_NAME = original.APPLICANT_INFO.APPLICANT_DEPT_NAME;
                applicantInfo.APPLICANT_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.APPLICANT_NAME = original.APPLICANT_INFO.APPLICANT_NAME;
                applicantInfo.APPLICANT_PHONE = String.Empty;  //自訂欄位
                applicantInfo.APPLICANT_DATETIME = DateTime.Now;
                applicantInfo.FILLER_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.FILLER_NAME = original.APPLICANT_INFO.APPLICANT_NAME;

                #endregion

                #region - 申請單內容 -

                //表單內容
                test01Content.REMARK = original.TEST_01_CONTENT.REMARK;

                #endregion

                test01.APPLICANT_INFO = applicantInfo;
                test01.TEST_01_CONTENT = test01Content;

                //PutTest01Single(test01);

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("測試01申請單(依此單內容重送)失敗，原因：" + ex.Message);

            }

            return vResult;
        }

        /// <summary>
        /// 測試01申請單(新增/修改/草稿)
        /// </summary>
        public bool PutTest01Single(Test01ViewModel model)
        {
            bool vResult = false;

            try
            {
                #region 主表：TEST_01_M

                var parameterA = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = model.APPLICANT_INFO.PRIORITY },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = model.APPLICANT_INFO.DRAFT_FLAG },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = model.APPLICANT_INFO.FLOW_ACTIVATED },
                    //表單主旨
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.FM7_SUBJECT },
                    //申請人資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME  ?? String.Empty },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = String.Empty },  //自訂欄位
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //填單人資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //表單內容
                    new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size = 200, Value = model.TEST_01_CONTENT.REMARK ?? String.Empty } //自訂欄位
                };

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_01_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_TEST_01_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "      [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "      [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "      [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "      [ApplicantName]=@APPLICANT_NAME, ";
                    //strSQL += "      [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "      [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "      [FillerID]=@FILLER_ID, ";
                    strSQL += "      [FillerName]=@FILLER_NAME, ";
                    strSQL += "      [Priority]=@PRIORITY, ";
                    strSQL += "      [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "      [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "      [Remark]=@REMARK ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_01_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[FillerID],[FillerName],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[Remark]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@FILLER_ID,@FILLER_NAME,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@REMARK) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region 表單主旨：FormHeader

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = model.APPLICANT_INFO.FM7_SUBJECT;

                formRepository.PutFormHeader(header);

                #endregion

                #region 儲存草稿：FormDraftList

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = "TEST_01";
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region 送出表單：FormAutoStart

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = "TEST_01";
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

                //commonRepository.PutFormToaster();

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("跑馬申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        #endregion

        #region  - TEST_02 - 
        
        /// <summary>
        /// 測試02申請單(查詢)
        /// </summary>
        public Test02ViewModel PostTest02Single(Test02QueryModel query)
        {
            #region  - 查詢 - 

            #region  - 申請人資訊 - 
            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     A.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     A.[DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     B.[Value] AS [FM7_SUBJECT], ";
            strSQL += "     A.[ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     A.[ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     A.[ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     NULL AS [APPLICANT_PHONE], ";
            strSQL += "     A.[ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     A.[FillerID] AS [FILLER_ID], ";
            strSQL += "     A.[FillerName] AS [FILLER_NAME], ";
            strSQL += "     A.[Priority] AS [PRIORITY], ";
            strSQL += "     A.[DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     A.[FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_02_M] A";
            strSQL += "         INNER JOIN [BPMPro].[dbo].[FSe7en_Tep_FormHeader] B ON A.[RequisitionID]=B.[RequisitionID]";
            strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();
            #endregion

            #region - 測試02設定(申請單內容) -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [Remark] AS [REMARK] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_02_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var test02Config = dbFun.DoQuery(strSQL, parameterB).ToList<Test02Config>().FirstOrDefault();

            #endregion

            #region - 測試02列表 -

            var parameterC = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ImplementationYear] AS [IMPLPEMENTATION_YEAR], ";
            strSQL += "     [Narrative] AS [NARRATIVE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_02_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var test02List = dbFun.DoQuery(strSQL, parameterC).ToList<Test02List>();

            #endregion

            #endregion

            var test02 = new Test02ViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                TEST_02_CONFIG = test02Config,
                TEST_02_LIST = test02List
            };
            return test02;
        }

        /// <summary>
        /// 測試02申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutTest02Refill(Test02QueryModel query)
        {
            bool vResult = false;
            try
            {
                #region  - 宣告 - 
                var original = PostTest02Single(query);

                var test02 = new Test02ViewModel();
                var applicantInfo = new ApplicantInfo();
                var test02Config = new Test02Config();
                var test02List = new List<Test02List>();

                var requisitionID = Guid.NewGuid().ToString();
                #endregion

                #region - 申請人資訊 -

                //表單資訊
                applicantInfo.REQUISITION_ID = requisitionID;
                applicantInfo.DIAGRAM_ID = original.APPLICANT_INFO.DIAGRAM_ID;
                applicantInfo.PRIORITY = original.APPLICANT_INFO.PRIORITY;
                applicantInfo.DRAFT_FLAG = 1;
                applicantInfo.FLOW_ACTIVATED = original.APPLICANT_INFO.FLOW_ACTIVATED;
                //申請人資訊
                applicantInfo.APPLICANT_DEPT = original.APPLICANT_INFO.APPLICANT_DEPT;
                applicantInfo.APPLICANT_DEPT_NAME = original.APPLICANT_INFO.APPLICANT_DEPT_NAME;
                applicantInfo.APPLICANT_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.APPLICANT_NAME = original.APPLICANT_INFO.APPLICANT_NAME;
                applicantInfo.APPLICANT_PHONE = String.Empty;  //自訂欄位
                applicantInfo.APPLICANT_DATETIME = DateTime.Now;
                applicantInfo.FILLER_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.FILLER_NAME = original.APPLICANT_INFO.APPLICANT_NAME;

                #endregion

                #region - 測試02(申請單內容) -

                //表單內容
                test02Config.FM7_SUBJECT = "(依此單內容重上)" + original.TEST_02_CONFIG.FM7_SUBJECT;
                test02Config.REMARK = original.TEST_02_CONFIG.REMARK;

                #endregion

                #region - 測試02列表 -

                foreach (var item in original.TEST_02_LIST)
                {
                    test02List.Add(new Test02List
                    {
                        REQUISITION_ID = requisitionID,
                        IMPLPEMENTATION_YEAR = item.IMPLPEMENTATION_YEAR,
                        NARRATIVE = item.NARRATIVE
                    });
                }

                #endregion

                test02.APPLICANT_INFO = applicantInfo;
                test02.TEST_02_CONFIG = test02Config;
                test02.TEST_02_LIST = test02List;

                PutTest02Single(test02);

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("測試02申請單(依此單內容重送)失敗，原因" + ex.Message);
            }
            return vResult;
        }

        /// <summary>
        /// 測試02申請單(新增/修改/草稿)
        /// </summary>
        public bool PutTest02Single(Test02ViewModel model)
        {
            bool vResult = false;
            try
            {
                #region 測試02主表：TEST_02_M

                var parameterA = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = model.APPLICANT_INFO.PRIORITY },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = model.APPLICANT_INFO.DRAFT_FLAG },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = model.APPLICANT_INFO.FLOW_ACTIVATED },
                    //申請人資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME  ?? String.Empty },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = String.Empty },  //自訂欄位
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //填單人資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //表單內容                    
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = model.TEST_02_CONFIG.FM7_SUBJECT ?? String.Empty }, //自訂欄位
                    new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size = 200, Value = model.TEST_02_CONFIG.REMARK ?? String.Empty } //自訂欄位
                };

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_02_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_TEST_02_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "      [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "      [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "      [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "      [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "      [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "      [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "      [FillerID]=@FILLER_ID, ";
                    strSQL += "      [FillerName]=@FILLER_NAME, ";
                    strSQL += "      [Priority]=@PRIORITY, ";
                    strSQL += "      [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "      [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "      [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "      [Remark]=@REMARK ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_02_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[FillerID],[FillerName],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[Remark]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@FILLER_ID,@FILLER_NAME,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@REMARK) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region 測試02列表：TEST_02_D

                var parameterB = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@IMPLPEMENTATION_YEAR", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@NARRATIVE", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_02_D] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoQuery(strSQL, parameterB);

                #endregion

                #region 再新增資料

                foreach (var item in model.TEST_02_LIST)
                {
                    parameterB[1].Value = item.IMPLPEMENTATION_YEAR;
                    parameterB[2].Value = item.NARRATIVE;

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_02_D]([RequisitionID],[ImplementationYear],[Narrative]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@IMPLPEMENTATION_YEAR,@NARRATIVE) ";

                    dbFun.DoTran(strSQL, parameterB);
                }

                #endregion

                #endregion

                #region 表單主旨：FormHeader

                FormHeader header = new FormHeader
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ITEM_NAME = "Subject",
                    ITEM_VALUE = model.TEST_02_CONFIG.FM7_SUBJECT
                };

                formRepository.PutFormHeader(header);

                #endregion

                #region 儲存草稿：FormDraftList

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = "TEST_02",
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region 送出表單：FormAutoStart

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    //刪除草稿清單
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = "TEST_02",
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };
                    //PutFormDraftList(參數, 是否再新增)
                    formRepository.PutFormDraftList(draftList, false);

                    #endregion
                    //送出表單
                    FormAutoStart autoStart = new FormAutoStart()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID,
                        APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID,
                        APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT
                    };

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("測試02申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }
            return vResult;
        }
        #endregion

        #region  - TEST_03 - 

        /// <summary>
        /// 測試03申請單(查詢)
        /// </summary>
        public Test03ViewModel PostTest03Single(Test03QueryModel query)
        {
            #region  - 查詢 - 

            #region  - 申請人資訊 - 
            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     A.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     A.[DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     B.[Value] AS [FM7_SUBJECT], ";
            strSQL += "     A.[ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     A.[ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     A.[ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     NULL AS [APPLICANT_PHONE], ";
            strSQL += "     A.[ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     A.[FillerID] AS [FILLER_ID], ";
            strSQL += "     A.[FillerName] AS [FILLER_NAME], ";
            strSQL += "     A.[Priority] AS [PRIORITY], ";
            strSQL += "     A.[DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     A.[FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_03_M] A";
            strSQL += "         INNER JOIN [BPMPro].[dbo].[FSe7en_Tep_FormHeader] B ON A.[RequisitionID]=B.[RequisitionID]";
            strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();
            #endregion

            #region - 測試03設定(申請單內容) -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [Remark] AS [REMARK] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_03_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var Test03Config = dbFun.DoQuery(strSQL, parameterB).ToList<Test03Config>().FirstOrDefault();

            #endregion

            #region - 測試03列表 -

            var parameterC = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ACPT_OwnerDept] AS [ACPT_OWNER_DEPT], ";
            strSQL += "     [ACPT_OwnerID] AS [ACPT_OWNER_ID] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_03_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var Test03List = dbFun.DoQuery(strSQL, parameterC).ToList<Test03List>();

            #endregion

            #endregion

            var Test03 = new Test03ViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                TEST_03_CONFIG = Test03Config,
                TEST_03_LIST = Test03List
            };
            return Test03;
        }

        /// <summary>
        /// 測試03申請單(新增/修改/草稿)
        /// </summary>
        public bool PutTest03Single(Test03ViewModel model)
        {
            bool vResult = false;
            try
            {
                #region 測試03主表：TEST_03_M

                var parameterA = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = model.APPLICANT_INFO.PRIORITY },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = model.APPLICANT_INFO.DRAFT_FLAG },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = model.APPLICANT_INFO.FLOW_ACTIVATED },
                    //申請人資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME  ?? String.Empty },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = String.Empty },  //自訂欄位
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //填單人資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //表單內容                    
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = model.TEST_03_CONFIG.FM7_SUBJECT ?? String.Empty }, //自訂欄位
                    new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size = 200, Value = model.TEST_03_CONFIG.REMARK ?? String.Empty } //自訂欄位
                };

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_03_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_TEST_03_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "      [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "      [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "      [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "      [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "      [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "      [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "      [FillerID]=@FILLER_ID, ";
                    strSQL += "      [FillerName]=@FILLER_NAME, ";
                    strSQL += "      [Priority]=@PRIORITY, ";
                    strSQL += "      [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "      [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "      [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "      [Remark]=@REMARK ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_03_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[FillerID],[FillerName],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[Remark]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@FILLER_ID,@FILLER_NAME,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@REMARK) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region 測試03列表：TEST_03_D

                var parameterB = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@ACPT_OWNER_DEPT", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@ACPT_OWNER_ID", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_03_D] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoQuery(strSQL, parameterB);

                #endregion

                #region 再新增資料

                foreach (var item in model.TEST_03_LIST)
                {
                    parameterB[1].Value = item.ACPT_OWNER_DEPT;
                    parameterB[2].Value = item.ACPT_OWNER_ID;

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_03_D]([RequisitionID],[ACPT_OwnerDept],[ACPT_OwnerID]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@ACPT_OWNER_DEPT,@ACPT_OWNER_ID) ";

                    dbFun.DoTran(strSQL, parameterB);
                }

                #endregion

                #endregion

                #region 表單主旨：FormHeader

                FormHeader header = new FormHeader
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ITEM_NAME = "Subject",
                    ITEM_VALUE = model.TEST_03_CONFIG.FM7_SUBJECT
                };

                formRepository.PutFormHeader(header);

                #endregion

                #region 儲存草稿：FormDraftList

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = "TEST_03",
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region 送出表單：FormAutoStart

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    //刪除草稿清單
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = "TEST_03",
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };
                    //PutFormDraftList(參數, 是否再新增)
                    formRepository.PutFormDraftList(draftList, false);

                    #endregion
                    //送出表單
                    FormAutoStart autoStart = new FormAutoStart()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID,
                        APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID,
                        APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT
                    };

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("測試03申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }
            return vResult;
        }
        #endregion

        #region  - TEST_05 - 

        /// <summary>
        /// 測試05申請單(查詢)
        /// </summary>
        public Test05ViewModel PostTest05Single(Test05QueryModel query)
        {
            #region  - 查詢 - 

            #region  - 申請人資訊 - 
            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     A.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     A.[DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     B.[Value] AS [FM7_SUBJECT], ";
            strSQL += "     A.[ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     A.[ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     A.[ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     NULL AS [APPLICANT_PHONE], ";
            strSQL += "     A.[ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     A.[FillerID] AS [FILLER_ID], ";
            strSQL += "     A.[FillerName] AS [FILLER_NAME], ";
            strSQL += "     A.[Priority] AS [PRIORITY], ";
            strSQL += "     A.[DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     A.[FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_05_M] A";
            strSQL += "         INNER JOIN [BPMPro].[dbo].[FSe7en_Tep_FormHeader] B ON A.[RequisitionID]=B.[RequisitionID]";
            strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();
            #endregion

            #region - 測試05設定(申請單內容) -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [Remark] AS [REMARK] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_05_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var test05Config = dbFun.DoQuery(strSQL, parameterB).ToList<Test05Config>().FirstOrDefault();

            #endregion

            #region - 測試05列表 -

            var parameterC = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ImplementationYear] AS [IMPLPEMENTATION_YEAR], ";
            strSQL += "     [Narrative] AS [NARRATIVE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_05_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var test05List = dbFun.DoQuery(strSQL, parameterC).ToList<Test05List>();

            #endregion

            #endregion

            var test05 = new Test05ViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                TEST_05_CONFIG = test05Config,
                TEST_05_LIST = test05List
            };
            return test05;
        }

        /// <summary>
        /// 測試05申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutTest05Refill(Test05QueryModel query)
        {
            bool vResult = false;
            try
            {
                #region  - 宣告 - 
                var original = PostTest05Single(query);

                var test05 = new Test05ViewModel();
                var applicantInfo = new ApplicantInfo();
                var test05Config = new Test05Config();
                var test05List = new List<Test05List>();

                var requisitionID = Guid.NewGuid().ToString();
                #endregion

                #region - 申請人資訊 -

                //表單資訊
                applicantInfo.REQUISITION_ID = requisitionID;
                applicantInfo.DIAGRAM_ID = original.APPLICANT_INFO.DIAGRAM_ID;
                applicantInfo.PRIORITY = original.APPLICANT_INFO.PRIORITY;
                applicantInfo.DRAFT_FLAG = 1;
                applicantInfo.FLOW_ACTIVATED = original.APPLICANT_INFO.FLOW_ACTIVATED;
                //申請人資訊
                applicantInfo.APPLICANT_DEPT = original.APPLICANT_INFO.APPLICANT_DEPT;
                applicantInfo.APPLICANT_DEPT_NAME = original.APPLICANT_INFO.APPLICANT_DEPT_NAME;
                applicantInfo.APPLICANT_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.APPLICANT_NAME = original.APPLICANT_INFO.APPLICANT_NAME;
                applicantInfo.APPLICANT_PHONE = String.Empty;  //自訂欄位
                applicantInfo.APPLICANT_DATETIME = DateTime.Now;
                applicantInfo.FILLER_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.FILLER_NAME = original.APPLICANT_INFO.APPLICANT_NAME;

                #endregion

                #region - 測試05(申請單內容) -

                //表單內容
                test05Config.FM7_SUBJECT = "(依此單內容重上)" + original.TEST_05_CONFIG.FM7_SUBJECT;
                test05Config.REMARK = original.TEST_05_CONFIG.REMARK;

                #endregion

                #region - 測試05列表 -

                foreach (var item in original.TEST_05_LIST)
                {
                    test05List.Add(new Test05List
                    {
                        REQUISITION_ID = requisitionID,
                        IMPLPEMENTATION_YEAR = item.IMPLPEMENTATION_YEAR,
                        NARRATIVE = item.NARRATIVE
                    });
                }

                #endregion

                test05.APPLICANT_INFO = applicantInfo;
                test05.TEST_05_CONFIG = test05Config;
                test05.TEST_05_LIST = test05List;

                PutTest05Single(test05);

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("測試05申請單(依此單內容重送)失敗，原因" + ex.Message);
            }
            return vResult;
        }

        /// <summary>
        /// 測試05申請單(新增/修改/草稿)
        /// </summary>
        public bool PutTest05Single(Test05ViewModel model)
        {
            bool vResult = false;
            try
            {
                #region 測試05主表：TEST_05_M

                var parameterA = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = model.APPLICANT_INFO.PRIORITY },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = model.APPLICANT_INFO.DRAFT_FLAG },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = model.APPLICANT_INFO.FLOW_ACTIVATED },
                    //申請人資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME  ?? String.Empty },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = String.Empty },  //自訂欄位
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //填單人資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //表單內容                    
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = model.TEST_05_CONFIG.FM7_SUBJECT ?? String.Empty }, //自訂欄位
                    new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size = 200, Value = model.TEST_05_CONFIG.REMARK ?? String.Empty } //自訂欄位
                };

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_05_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_TEST_05_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "      [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "      [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "      [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "      [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "      [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "      [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "      [FillerID]=@FILLER_ID, ";
                    strSQL += "      [FillerName]=@FILLER_NAME, ";
                    strSQL += "      [Priority]=@PRIORITY, ";
                    strSQL += "      [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "      [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "      [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "      [Remark]=@REMARK ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_05_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[FillerID],[FillerName],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[Remark]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@FILLER_ID,@FILLER_NAME,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@REMARK) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region 測試05列表：TEST_05_D

                var parameterB = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@IMPLPEMENTATION_YEAR", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@NARRATIVE", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_TEST_05_D] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoQuery(strSQL, parameterB);

                #endregion

                #region 再新增資料

                foreach (var item in model.TEST_05_LIST)
                {
                    parameterB[1].Value = item.IMPLPEMENTATION_YEAR;
                    parameterB[2].Value = item.NARRATIVE;

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_TEST_05_D]([RequisitionID],[ImplementationYear],[Narrative]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@IMPLPEMENTATION_YEAR,@NARRATIVE) ";

                    dbFun.DoTran(strSQL, parameterB);
                }

                #endregion

                #endregion

                #region 表單主旨：FormHeader

                FormHeader header = new FormHeader
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ITEM_NAME = "Subject",
                    ITEM_VALUE = model.TEST_05_CONFIG.FM7_SUBJECT
                };

                formRepository.PutFormHeader(header);

                #endregion

                #region 儲存草稿：FormDraftList

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = "TEST_05",
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region 送出表單：FormAutoStart

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    //刪除草稿清單
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = "TEST_05",
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };
                    //PutFormDraftList(參數, 是否再新增)
                    formRepository.PutFormDraftList(draftList, false);

                    #endregion
                    //送出表單
                    FormAutoStart autoStart = new FormAutoStart()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID,
                        APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID,
                        APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT
                    };

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("測試05申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }
            return vResult;
        }
        #endregion

        #region - TEST_外部程序 -

        /// <summary>
        /// 測試外部程序(匯入)
        /// </summary>        
        public bool PutTestImportSingle(Test02QueryModel query)
        {
            bool vResult = false;
            try
            {
                //子表單_行政採購申請單(查詢)
                var postTest02Single = PostTest02Single(query);
                postTest02Single.TEST_02_CONFIG.REMARK = "TEST-123";

                strJson = jsonFunction.ObjectToJSON(postTest02Single);
                CommLib.Logger.Debug("測試外部程序(匯入)Json：" + strJson);

                //行政採購申請單 原表單匯入子表單(新增/修改/草稿)
                PutTest02Single(postTest02Single);
                vResult = true; 
            }
            catch (Exception ex)
            {                
                CommLib.Logger.Error("行政採購申請單(匯入)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        #endregion



        #region - TEST_回傳ERP -

        public bool PutTestReurnERPSingle(StepFlowQueryModel query)
        {
            bool vResult = false;
            try
            {
                stepFlowRepository.PostStepSignSingle(query);
                var requestQueryModel = new RequestQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID,
                    REQUEST_FLG = false
                };
                responseInfoRepository.PostGeneralOrderInfoSingle(requestQueryModel);
                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購申請單(回傳ERP)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        #endregion

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        /// <summary>
        /// Json字串
        /// </summary>
        private string strJson;

        #endregion
    }
}