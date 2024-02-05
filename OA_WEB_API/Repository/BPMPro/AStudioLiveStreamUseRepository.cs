using Microsoft.Ajax.Utilities;
using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using OA_WEB_API.Models;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - A攝影棚直播使用申請單
    /// </summary>
    public class AStudioLiveStreamUseRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

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
        /// A攝影棚直播使用申請單(查詢)
        /// </summary>
        public AStudioLiveStreamUseViewModel PostAStudioLiveStreamUseSingle(AStudioLiveStreamUseQueryModel query)
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

            #region - A攝影棚直播使用申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var aStudioLiveStreamUseTitle = dbFun.DoQuery(strSQL, parameter).ToList<AStudioLiveStreamUseTitle>().FirstOrDefault();

            #endregion

            #region - A攝影棚直播使用申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [LiveStreamDate] AS [LIVE_STREAM_DATE], ";
            strSQL += "     [StartTime] AS [START_TIME], ";
            strSQL += "     [EndTime] AS [END_TIME], ";
            strSQL += "     [EquipmentUse] AS [EQUIPMENT_USE], ";
            strSQL += "     [MicCount] AS [MIC_COUNT], ";
            strSQL += "     [Light] AS [LIGHT], ";
            strSQL += "     CAST([IsLiveStreamEquipment] as bit) AS [IS_LIVE_STREAM_EQUIPMENT], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [ApplicationContent] AS [APPLICATION_CONTENT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var aStudioLiveStreamUseConfig = dbFun.DoQuery(strSQL, parameter).ToList<AStudioLiveStreamUseConfig>().FirstOrDefault();

            #endregion

            #region - A攝影棚直播使用申請單 會簽簽核人員 -

            var CommonApprovers = new BPMCommonModel<AStudioLiveStreamUseApproversConfig>()
            {
                EXT = "D",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApproverFunction(CommonApprovers));
            var aStudioLiveStreamUseApproversConfig = jsonFunction.JsonToObject<List<AStudioLiveStreamUseApproversConfig>>(strJson);

            #endregion

            var aStudioLiveStreamUseViewModel = new AStudioLiveStreamUseViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                A_STUDIO_LIVE_STREAM_USE_TITLE = aStudioLiveStreamUseTitle,
                A_STUDIO_LIVE_STREAM_USE_CONFIG = aStudioLiveStreamUseConfig,
                A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG = aStudioLiveStreamUseApproversConfig,
            };

            #region - 確認表單 -

            if (aStudioLiveStreamUseViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    aStudioLiveStreamUseViewModel = new AStudioLiveStreamUseViewModel();
                    CommLib.Logger.Error("A攝影棚直播使用申請單(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(aStudioLiveStreamUseViewModel.A_STUDIO_LIVE_STREAM_USE_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(aStudioLiveStreamUseViewModel.A_STUDIO_LIVE_STREAM_USE_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) aStudioLiveStreamUseViewModel.A_STUDIO_LIVE_STREAM_USE_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return aStudioLiveStreamUseViewModel;
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// A攝影棚直播使用申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>        
        //public bool PutAStudioLiveStreamUseRefill(AStudioLiveStreamUseQueryModel query)
        //{
        //    bool vResult = false;

        //    try
        //    {
        //        if (!String.IsNullOrEmpty(query.REQUISITION_ID) || !String.IsNullOrWhiteSpace(query.REQUISITION_ID))
        //        {
        //            #region - 宣告 -

        //            var original = PostAStudioLiveStreamUseSingle(query);
        //            strJson = jsonFunction.ObjectToJSON(original);

        //            var aStudioLiveStreamUseViewModel = new AStudioLiveStreamUseViewModel();

        //            var requisitionID = Guid.NewGuid().ToString();

        //            #endregion

        //            #region - 重送內容 -

        //            aStudioLiveStreamUseViewModel = jsonFunction.JsonToObject<AStudioLiveStreamUseViewModel>(strJson);

        //            #region - 申請人資訊 調整 -

        //            aStudioLiveStreamUseViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
        //            aStudioLiveStreamUseViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
        //            aStudioLiveStreamUseViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

        //            #endregion

        //            #region - A攝影棚直播使用申請單 表頭資訊 調整 -

        //            aStudioLiveStreamUseViewModel.A_STUDIO_LIVE_STREAM_USE_TITLE = null;

        //            #endregion

        //            #region - A攝影棚直播使用申請單 表頭內容 調整 -

        //            aStudioLiveStreamUseViewModel.A_STUDIO_LIVE_STREAM_USE_CONFIG = null;

        //            #endregion

        //            #region - 四方四隅_會簽單 會簽簽核人員 清除 -

        //            aStudioLiveStreamUseViewModel.A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG = null;

        //            #endregion

        //            #endregion

        //            #region - 送出 執行(新增/修改/草稿) -

        //            PutAStudioLiveStreamUseSingle(aStudioLiveStreamUseViewModel);

        //            #endregion
        //        }

        //        vResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("A攝影棚直播使用申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// A攝影棚直播使用申請單(新增/修改/草稿)
        /// </summary>
        public bool PutAStudioLiveStreamUseSingle(AStudioLiveStreamUseViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                var LiveStreamActivityDateTime = String.Empty;

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                FM7Subject = model.A_STUDIO_LIVE_STREAM_USE_TITLE.FM7_SUBJECT;

                if (String.IsNullOrEmpty(model.A_STUDIO_LIVE_STREAM_USE_TITLE.FM7_SUBJECT) || String.IsNullOrWhiteSpace(model.A_STUDIO_LIVE_STREAM_USE_TITLE.FM7_SUBJECT))
                {
                    if (model.APPLICANT_INFO.DRAFT_FLAG == 1)
                    {
                        if ((String.IsNullOrEmpty(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE) || String.IsNullOrWhiteSpace(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE)) && (String.IsNullOrEmpty(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME) || String.IsNullOrWhiteSpace(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME)))
                            LiveStreamActivityDateTime = "_____";
                        else if (String.IsNullOrEmpty(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE) || String.IsNullOrWhiteSpace(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE))
                            LiveStreamActivityDateTime = "___ " + model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME;
                        else if (String.IsNullOrEmpty(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME) || String.IsNullOrWhiteSpace(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME))
                            LiveStreamActivityDateTime = model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE + " ___";
                        else LiveStreamActivityDateTime = model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE + " " + model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME;

                    }
                    else LiveStreamActivityDateTime = model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE + " " + model.A_STUDIO_LIVE_STREAM_USE_CONFIG.START_TIME;

                    FM7Subject = "A攝影棚" + LiveStreamActivityDateTime + "直播使用；" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                }

                #endregion

                #endregion

                #region - A攝影棚直播使用申請單 表頭資訊：AStudioLiveStreamUse_M -

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
                    //四方四隅_會簽單 表頭
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
                        model.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Parse(DateTime.Parse(parameterTitle.Where(P => P.ParameterName == "@APPLICANT_DATETIME").FirstOrDefault().Value.ToString()).ToShortDateString());

                        #region - 表單防呆 -

                        if (DateTime.Parse(model.A_STUDIO_LIVE_STREAM_USE_CONFIG.LIVE_STREAM_DATE) < model.APPLICANT_INFO.APPLICANT_DATETIME)
                        {
                            CommLib.Logger.Error("A攝影棚直播使用申請單(新增/修改/草稿)失敗，原因：直播日期異常申請。");
                            return false;
                        }

                        #endregion

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
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - A攝影棚直播使用申請單 表單內容：AStudioLiveStreamUse_M -

                if (model.A_STUDIO_LIVE_STREAM_USE_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //A攝影棚直播使用申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@LIVE_STREAM_DATE", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@START_TIME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@END_TIME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EQUIPMENT_USE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MIC_COUNT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@LIGHT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_LIVE_STREAM_EQUIPMENT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPLICATION_CONTENT", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    //寫入：A攝影棚直播使用申請單 表單內容parameter
                                        
                    if (model.A_STUDIO_LIVE_STREAM_USE_CONFIG.IS_LIVE_STREAM_EQUIPMENT == null) model.A_STUDIO_LIVE_STREAM_USE_CONFIG.IS_LIVE_STREAM_EQUIPMENT = true;

                    strJson = jsonFunction.ObjectToJSON(model.A_STUDIO_LIVE_STREAM_USE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [LiveStreamDate]=@LIVE_STREAM_DATE, ";
                    strSQL += "     [StartTime]=@START_TIME, ";
                    strSQL += "     [EndTime]=@END_TIME, ";
                    strSQL += "     [EquipmentUse]=@EQUIPMENT_USE, ";
                    strSQL += "     [MicCount]=@MIC_COUNT, ";
                    strSQL += "     [Light]=@LIGHT, ";
                    strSQL += "     [IsLiveStreamEquipment]=UPPER(@IS_LIVE_STREAM_EQUIPMENT), ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [ApplicationContent]=@APPLICATION_CONTENT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - A攝影棚直播使用申請單 會簽簽核人員：AStudioLiveStreamUse_D -

                var parameterApprovers = new List<SqlParameter>()
                {
                    //A攝影棚直播使用申請單 會簽簽核人員
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@APPROVER_COMPANY_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_MAIN_ID", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPROVER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                if (model.A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG != null && model.A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG.Count > 0)
                {
                    model.A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG.ForEach(A =>
                    {
                        var logonModel = new LogonModel()
                        {
                            USER_ID = A.APPROVER_ID
                        };
                        var userModel = userRepository.PostUserSingle(logonModel).USER_MODEL;

                        if (userModel != null && userModel.Count > 0 && userModel.Any(U => U.JOB_STATUS != 0))
                        {
                            var userInfoMainDeptModel = new UserInfoMainDeptModel()
                            {
                                USER_ID = A.APPROVER_ID,
                                DEPT_ID = A.APPROVER_DEPT_ID,
                                COMPANY_ID = userModel.Where(U => U.DEPT_ID == A.APPROVER_DEPT_ID).Select(U => U.COMPANY_ID).FirstOrDefault(),
                            };
                            A.APPROVER_DEPT_MAIN_ID = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).MAIN_DEPT.DEPT_ID;
                        }
                    });

                    var CommonApprovers = new BPMCommonModel<AStudioLiveStreamUseApproversConfig>()
                    {
                        EXT = "D",
                        IDENTIFY = IDENTIFY,
                        PARAMETER = parameterApprovers,
                        MODEL = model.A_STUDIO_LIVE_STREAM_USE_APPROVERS_CONFIG
                    };
                    commonRepository.PutApproverFunction(CommonApprovers);
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
                CommLib.Logger.Error("A攝影棚直播使用申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "AStudioLiveStreamUse";

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