using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

using OA_WEB_API.Models.BPMPro;

using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 跑馬申請單
    /// </summary>
    public class RollCardRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 跑馬申請單(查詢)
        /// </summary>
        public RollCardViewModel PostRollCardSingle(RollCardQueryModel query) 
        {
            #region - 查詢 -

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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_RollCard_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 跑馬設定及內容 -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Ch01] AS [CH01], ";
            strSQL += "     [Ch02] AS [CH02], ";
            strSQL += "     [Ch03] AS [CH03], ";
            strSQL += "     [Ch04] AS [CH04], ";
            strSQL += "     [Ch05] AS [CH05], ";
            strSQL += "     [Ch06] AS [CH06], ";
            strSQL += "     [RunLevel] AS [RUN_LEVEL], ";
            strSQL += "     [ClassB_Minutes] AS [CLASS_B_MINUTES], ";
            strSQL += "     [ClassB_Frequency] AS [CLASS_B_FREQUENCY], ";
            strSQL += "     [ClassE_Note] AS [CLASS_E_NOTE], ";
            strSQL += "     [StartDate] AS [START_DATE], ";
            strSQL += "     [EndDate] AS [END_DATE], ";
            strSQL += "     [RunMode] AS [RUN_MODE], ";
            strSQL += "     [EveryWeek_Mon] AS [EVERY_WEEK_MON], ";
            strSQL += "     [EveryWeek_Tue] AS [EVERY_WEEK_TUE], ";
            strSQL += "     [EveryWeek_Wed] AS [EVERY_WEEK_WED], ";
            strSQL += "     [EveryWeek_Thu] AS [EVERY_WEEK_THU], ";
            strSQL += "     [EveryWeek_Fri] AS [EVERY_WEEK_FRI], ";
            strSQL += "     [EveryWeek_Sat] AS [EVERY_WEEK_SAT], ";
            strSQL += "     [EveryWeek_Sun] AS [EVERY_WEEK_SUN], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [RollContent] AS [ROLL_CONTENT], ";
            strSQL += "     [Remark] AS [REMARK] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_RollCard_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var rollCardConfig = dbFun.DoQuery(strSQL, parameterB).ToList<RollCardConfig>().FirstOrDefault();

            #endregion

            #region - 跑馬時段 -

            var parameterC = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [StartTime] AS [START_TIME], ";
            strSQL += "     [EndTime] AS [END_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_RollCard_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [StartTime] ";

           var rollCardRunTimeList =  dbFun.DoQuery(strSQL, parameterC).ToList<RollCardRunTime>();

            #endregion

            #endregion

            var rollCard = new RollCardViewModel() 
            {
                APPLICANT_INFO = applicantInfo,
                ROLLCARD_CONFIG = rollCardConfig,
                ROLLCARD_RUN_TIME = rollCardRunTimeList
            };

            return rollCard;
        }

        /// <summary>
        /// 跑馬申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutRollCardRefill(RollCardQueryModel query)
        {
            bool vResult = false;

            try
            {
                #region - 宣告 -

                var original = PostRollCardSingle(query);

                var rollCard = new RollCardViewModel();
                var applicantInfo = new ApplicantInfo();
                var rollCardConfig = new RollCardConfig();
                var rollCardRunTimeList = new List<RollCardRunTime>();

                var requisitionID = Guid.NewGuid().ToString();

                #endregion

                #region - 申請人資訊 -

                applicantInfo.REQUISITION_ID = requisitionID;
                applicantInfo.DIAGRAM_ID = original.APPLICANT_INFO.DIAGRAM_ID;
                applicantInfo.PRIORITY = original.APPLICANT_INFO.PRIORITY;
                applicantInfo.DRAFT_FLAG = 1;
                applicantInfo.FLOW_ACTIVATED = original.APPLICANT_INFO.FLOW_ACTIVATED;
                applicantInfo.APPLICANT_DEPT = original.APPLICANT_INFO.APPLICANT_DEPT;
                applicantInfo.APPLICANT_DEPT_NAME = original.APPLICANT_INFO.APPLICANT_DEPT_NAME;
                applicantInfo.APPLICANT_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.APPLICANT_NAME = original.APPLICANT_INFO.APPLICANT_NAME;
                applicantInfo.APPLICANT_PHONE = original.APPLICANT_INFO.APPLICANT_PHONE;
                applicantInfo.APPLICANT_DATETIME = DateTime.Now;
                applicantInfo.FILLER_ID = original.APPLICANT_INFO.APPLICANT_ID;
                applicantInfo.FILLER_NAME = original.APPLICANT_INFO.APPLICANT_NAME;

                #endregion

                #region - 跑馬設定 -

                rollCardConfig.CH01 = original.ROLLCARD_CONFIG.CH01;
                rollCardConfig.CH02 = original.ROLLCARD_CONFIG.CH02;
                rollCardConfig.CH03 = original.ROLLCARD_CONFIG.CH03;
                rollCardConfig.CH04 = original.ROLLCARD_CONFIG.CH04;
                rollCardConfig.CH05 = original.ROLLCARD_CONFIG.CH05;
                rollCardConfig.CH06 = original.ROLLCARD_CONFIG.CH06;

                rollCardConfig.RUN_LEVEL = original.ROLLCARD_CONFIG.RUN_LEVEL;
                rollCardConfig.CLASS_B_MINUTES = original.ROLLCARD_CONFIG.CLASS_B_MINUTES;
                rollCardConfig.CLASS_B_FREQUENCY = original.ROLLCARD_CONFIG.CLASS_B_FREQUENCY;
                rollCardConfig.CLASS_E_NOTE = original.ROLLCARD_CONFIG.CLASS_E_NOTE;

                rollCardConfig.START_DATE = original.ROLLCARD_CONFIG.START_DATE;
                rollCardConfig.END_DATE = original.ROLLCARD_CONFIG.END_DATE;

                rollCardConfig.RUN_MODE = original.ROLLCARD_CONFIG.RUN_MODE;
                rollCardConfig.EVERY_WEEK_MON = original.ROLLCARD_CONFIG.EVERY_WEEK_MON;
                rollCardConfig.EVERY_WEEK_TUE = original.ROLLCARD_CONFIG.EVERY_WEEK_TUE;
                rollCardConfig.EVERY_WEEK_WED = original.ROLLCARD_CONFIG.EVERY_WEEK_WED;
                rollCardConfig.EVERY_WEEK_THU = original.ROLLCARD_CONFIG.EVERY_WEEK_THU;
                rollCardConfig.EVERY_WEEK_FRI = original.ROLLCARD_CONFIG.EVERY_WEEK_FRI;
                rollCardConfig.EVERY_WEEK_SAT = original.ROLLCARD_CONFIG.EVERY_WEEK_SAT;
                rollCardConfig.EVERY_WEEK_SUN = original.ROLLCARD_CONFIG.EVERY_WEEK_SUN;

                rollCardConfig.FM7_SUBJECT = "(依此單內容重上)" + original.ROLLCARD_CONFIG.FM7_SUBJECT;
                rollCardConfig.ROLL_CONTENT = original.ROLLCARD_CONFIG.ROLL_CONTENT;
                rollCardConfig.REMARK = original.ROLLCARD_CONFIG.REMARK;

                #endregion

                #region - 跑馬時段 -

                foreach (var item in original.ROLLCARD_RUN_TIME)
                {
                    rollCardRunTimeList.Add(new RollCardRunTime 
                    { 
                        REQUISITION_ID = requisitionID, 
                        START_TIME = item.START_TIME, 
                        END_TIME = item.END_TIME
                    });
                }

                #endregion

                rollCard.APPLICANT_INFO = applicantInfo;
                rollCard.ROLLCARD_CONFIG = rollCardConfig;
                rollCard.ROLLCARD_RUN_TIME = rollCardRunTimeList;

                PutRollCardSingle(rollCard);

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("跑馬申請單(依此單內容重送)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 跑馬申請單(新增/修改/草稿)
        /// </summary>
        public bool PutRollCardSingle(RollCardViewModel model)
        {
            bool vResult = false;

            try
            {
                #region 跑馬主表：RollCard_M

                var parameterA = new List<SqlParameter>()
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
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //跑馬設定
                    new SqlParameter("@CH01", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.CH01 ?? String.Empty },
                    new SqlParameter("@CH02", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.CH02 ?? String.Empty },
                    new SqlParameter("@CH03", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.CH03 ?? String.Empty },
                    new SqlParameter("@CH04", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.CH04 ?? String.Empty },
                    new SqlParameter("@CH05", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.CH05 ?? String.Empty },
                    new SqlParameter("@CH06", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.CH06 ?? String.Empty },
                    new SqlParameter("@RUN_LEVEL", SqlDbType.NVarChar) { Size = 10, Value = model.ROLLCARD_CONFIG.RUN_LEVEL ?? String.Empty },
                    new SqlParameter("@CLASS_B_MINUTES", SqlDbType.NVarChar) { Size = 50, Value = model.ROLLCARD_CONFIG.CLASS_B_MINUTES ?? String.Empty },
                    new SqlParameter("@CLASS_B_FREQUENCY", SqlDbType.NVarChar) { Size = 50, Value = model.ROLLCARD_CONFIG.CLASS_B_FREQUENCY ?? String.Empty }, 
                    new SqlParameter("@CLASS_E_NOTE", SqlDbType.NVarChar) { Size = 50, Value = model.ROLLCARD_CONFIG.CLASS_E_NOTE ?? String.Empty },   
                    new SqlParameter("@START_DATE", SqlDbType.NVarChar) { Size = 10, Value = model.ROLLCARD_CONFIG.START_DATE ?? String.Empty },
                    new SqlParameter("@END_DATE", SqlDbType.NVarChar) { Size = 10, Value = model.ROLLCARD_CONFIG.END_DATE ?? String.Empty },
                    new SqlParameter("@RUN_MODE", SqlDbType.NVarChar) { Size = 50, Value = model.ROLLCARD_CONFIG.RUN_MODE ?? String.Empty },
                    new SqlParameter("@EVERY_WEEK_MON", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_MON ?? String.Empty },
                    new SqlParameter("@EVERY_WEEK_TUE", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_TUE ?? String.Empty },
                    new SqlParameter("@EVERY_WEEK_WED", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_WED ?? String.Empty },
                    new SqlParameter("@EVERY_WEEK_THU", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_THU ?? String.Empty },
                    new SqlParameter("@EVERY_WEEK_FRI", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_FRI ?? String.Empty },
                    new SqlParameter("@EVERY_WEEK_SAT", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_SAT  ?? String.Empty},
                    new SqlParameter("@EVERY_WEEK_SUN", SqlDbType.NVarChar) { Size = 5, Value = model.ROLLCARD_CONFIG.EVERY_WEEK_SUN ?? String.Empty },
                    //跑馬內容
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = model.ROLLCARD_CONFIG.FM7_SUBJECT ?? String.Empty },
                    new SqlParameter("@ROLL_CONTENT", SqlDbType.NVarChar) { Size = 400, Value = model.ROLLCARD_CONFIG.ROLL_CONTENT ?? String.Empty }, 
                    new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size = 100, Value = model.ROLLCARD_CONFIG.REMARK ?? String.Empty }
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    var formData = new FormData()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID
                    };

                    if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                    {
                        parameterA.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });
                        IsADD = true;
                    }
                }
                else parameterA.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });

                #endregion

                strSQL = "";
                strSQL += "SELECT [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_RollCard_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_RollCard_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "      [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "      [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "      [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "      [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "      [ApplicantPhone]=@APPLICANT_PHONE, ";

                    if (IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

                    strSQL += "      [FillerID]=@FILLER_ID, ";
                    strSQL += "      [FillerName]=@FILLER_NAME, ";
                    strSQL += "      [Priority]=@PRIORITY, ";
                    strSQL += "      [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "      [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "      [Ch01]=@CH01, ";
                    strSQL += "      [Ch02]=@CH02, ";
                    strSQL += "      [Ch03]=@CH03, ";
                    strSQL += "      [Ch04]=@CH04, ";
                    strSQL += "      [Ch05]=@CH05, ";
                    strSQL += "      [Ch06]=@CH06, ";
                    strSQL += "      [RunLevel]=@RUN_LEVEL, ";
                    strSQL += "      [ClassB_Minutes]=@CLASS_B_MINUTES, ";
                    strSQL += "      [ClassB_Frequency]=@CLASS_B_FREQUENCY, ";
                    strSQL += "      [ClassE_Note]=@CLASS_E_NOTE, ";
                    strSQL += "      [StartDate]=@START_DATE, ";
                    strSQL += "      [EndDate]=@END_DATE, ";
                    strSQL += "      [RunMode]=@RUN_MODE, ";
                    strSQL += "      [EveryWeek_Mon]=@EVERY_WEEK_MON, ";
                    strSQL += "      [EveryWeek_Tue]=@EVERY_WEEK_TUE, ";
                    strSQL += "      [EveryWeek_Wed]=@EVERY_WEEK_WED, ";
                    strSQL += "      [EveryWeek_Thu]=@EVERY_WEEK_THU, ";
                    strSQL += "      [EveryWeek_Fri]=@EVERY_WEEK_FRI, ";
                    strSQL += "      [EveryWeek_Sat]=@EVERY_WEEK_SAT, ";
                    strSQL += "      [EveryWeek_Sun]=@EVERY_WEEK_SUN, ";
                    strSQL += "      [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "      [RollContent]=@ROLL_CONTENT, ";
                    strSQL += "      [Remark]=@REMARK ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_RollCard_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[FillerID],[FillerName],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[Ch01],[Ch02],[Ch03],[Ch04],[Ch05],[Ch06],[RunLevel],[ClassB_Minutes],[ClassB_Frequency],[ClassE_Note],[StartDate],[EndDate],[RunMode],[EveryWeek_Mon],[EveryWeek_Tue],[EveryWeek_Wed],[EveryWeek_Thu],[EveryWeek_Fri],[EveryWeek_Sat],[EveryWeek_Sun],[FM7Subject],[RollContent],[Remark]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@FILLER_ID,@FILLER_NAME,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@CH01,@CH02,@CH03,@CH04,@CH05,@CH06,@RUN_LEVEL,@CLASS_B_MINUTES,@CLASS_B_FREQUENCY,@CLASS_E_NOTE,@START_DATE,@END_DATE,@RUN_MODE,@EVERY_WEEK_MON,@EVERY_WEEK_TUE,@EVERY_WEEK_WED,@EVERY_WEEK_THU,@EVERY_WEEK_FRI,@EVERY_WEEK_SAT,@EVERY_WEEK_SUN,@FM7_SUBJECT,@ROLL_CONTENT,@REMARK) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region 跑馬時段：RollCard_D

                var parameterB = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@START_TIME", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty },
                    new SqlParameter("@END_TIME", SqlDbType.VarChar) { Size = 10, Value = String.Empty ?? String.Empty }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_RollCard_D] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoQuery(strSQL, parameterB);

                #endregion

                #region 再新增資料

                foreach (var item in model.ROLLCARD_RUN_TIME)
                {
                    parameterB[1].Value = item.START_TIME;
                    parameterB[2].Value = item.END_TIME;

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_RollCard_D]([RequisitionID],[StartTime],[EndTime]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@START_TIME,@END_TIME) ";

                    dbFun.DoTran(strSQL, parameterB);
                }

                #endregion

                #endregion

                #region 表單主旨：FormHeader

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = model.ROLLCARD_CONFIG.FM7_SUBJECT;

                formRepository.PutFormHeader(header);

                #endregion

                #region 儲存草稿：FormDraftList

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = "RollCard";
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
                    draftList.IDENTIFY = "RollCard";
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
                CommLib.Logger.Error("跑馬申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 跑馬申請單(審核通過，同步至OA及字幕機)
        /// </summary>
        public bool SetRollCardToCG(RollCardQueryModel query)
        {
            bool vResult = false;

            try
            {
                #region 方法一

                //var strConn = ConfigurationManager.ConnectionStrings["OADBConnectionString"].ConnectionString;

                //using (SqlConnection Conn = new SqlConnection(strConn))
                //{
                //    using (SqlCommand Cmd = new SqlCommand("sp_SetRollCardToCG_NewType", Conn))
                //    {
                //        Conn.Open();

                //        Cmd.CommandType = CommandType.StoredProcedure;
                //        Cmd.Parameters.Add("@RequisitionID", SqlDbType.NVarChar).Value = query.REQUISITION_ID;
                //        Cmd.ExecuteNonQuery();

                //        Conn.Close();
                //    }
                //}

                //CommLib.Logger.Debug("strConn：" + strConn);
                //CommLib.Logger.Debug("SP Name：sp_SetRollCardToCG_NewType");
                //CommLib.Logger.Debug("@RequisitionID：" + query.REQUISITION_ID);

                #endregion

                #region 方法二(同意結束，外部串接不要判斷Status=1，因為可能當下還沒是1)

                dbFunction dbFunOA = new dbFunction(GlobalParameters.sqlConnOADB);

                var parameter = new List<SqlParameter>
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "EXEC [OADB].[dbo].[sp_SetRollCardToCG_NewType] @REQUISITION_ID ";

                CommLib.Logger.Info(String.Format("EXEC [OADB].[dbo].[sp_SetRollCardToCG_NewType] {0}", query.REQUISITION_ID));

                dbFunOA.DoTran(strSQL, parameter);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;

                CommLib.Logger.Error("跑馬申請單(審核通過，同步至OA及字幕機)失敗，原因：" + ex.Message);
                throw new Exception("跑馬申請單(審核通過，同步至OA及字幕機)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "RollCard";
        #endregion
    }
}