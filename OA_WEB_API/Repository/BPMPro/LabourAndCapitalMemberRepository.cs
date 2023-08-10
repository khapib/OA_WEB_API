using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Docker.DotNet.Models;
using System.Reflection;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Diagnostics;
using System.IO;
using System.Collections;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;

using Microsoft.Ajax.Utilities;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 勞資委員投票
    /// </summary>
    public class LabourAndCapitalMemberRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        SysCommonRepository sysCommonRepository = new SysCommonRepository();
        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        FlowRepository flowRepository = new FlowRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 勞資委員投票(登入者資訊)
        /// </summary>
        public LabourAndCapitalMemberVoterInfoConfig PostLabourAndCapitalMemberVoterInfoSingle(LabourAndCapitalMemberQueryModel query)
        {
            try
            {
                var labourAndCapitalMemberVoterInfoConfig = new LabourAndCapitalMemberVoterInfoConfig();

                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = query.VOTE_YEAR },
                    new SqlParameter("@LOGIN_ID", SqlDbType.NVarChar) { Size = 40, Value = query.LOGIN_ID },
                };

                #region - 是否已投過票 -

                strSQL = "";
                strSQL += "Select ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                strSQL += "         AND [LoginID]=@LOGIN_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameter);

                if (dtA.Rows.Count > 0) labourAndCapitalMemberVoterInfoConfig.IS_VOTE = false;
                else labourAndCapitalMemberVoterInfoConfig.IS_VOTE = true;

                #endregion

                #region - 當前登入者的主要部門 -

                var logonModel = new LogonModel()
                {
                    USER_ID = query.LOGIN_ID
                };
                var userModel = userRepository.PostUserSingle(logonModel).USER_MODEL;

                if (userModel != null && userModel.Count > 0 && userModel.Any(U => U.JOB_STATUS != 0))
                {
                    userModel.ForEach(U =>
                    {
                        var userInfoMainDeptModel = new UserInfoMainDeptModel()
                        {
                            USER_ID = query.LOGIN_ID,
                            DEPT_ID = U.DEPT_ID,
                            COMPANY_ID = userModel.Where(U2 => U2.DEPT_ID == U.DEPT_ID).Select(U2 => U2.COMPANY_ID).FirstOrDefault(),
                        };
                        if (sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).USER_MODEL.IS_MAIN_JOB != null)
                        {
                            if (bool.Parse(sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).USER_MODEL.IS_MAIN_JOB.ToString()))
                            {
                                labourAndCapitalMemberVoterInfoConfig.MAIN_DEPT_ID = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).MAIN_DEPT.DEPT_ID;
                            }
                        }
                    });
                }
                else labourAndCapitalMemberVoterInfoConfig.IS_VOTE = false;

                #endregion

                #region - 投票年分 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";
                var dt = dbFun.DoQuery(strSQL, parameter);
                if (dt.Rows.Count > 0)
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [VoteYear] AS [VOTE_YEARS] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "ORDER BY CAST([VoteYear] AS INTEGER) ASC ";
                    labourAndCapitalMemberVoterInfoConfig.VOTE_YEARS = dbFun.DoQuery(strSQL)
                                                                        .AsEnumerable()
                                                                        .Select(r => r.Field<string>("VOTE_YEARS"))
                                                                        .ToList();
                }

                #endregion

                return labourAndCapitalMemberVoterInfoConfig;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(登入者資訊)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 勞資委員投票(部門查詢)
        /// </summary>
        public List<LabourAndCapitalMemberVoterDeptsConfig> PostLabourAndCapitalMemberVoterDeptsSingle(LabourAndCapitalMemberQueryModel query)
        {
            var labourAndCapitalMemberVoterDeptsConfig = new List<LabourAndCapitalMemberVoterDeptsConfig>();

            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = query.VOTE_YEAR },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";
                var dt = dbFun.DoQuery(strSQL, parameter);
                if (dt.Rows.Count > 0)
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [MainDeptID] AS [MAIN_DEPT_ID], ";
                    strSQL += "     [MainDeptName] AS [MAIN_DEPT_NAME] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                    strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";
                    strSQL += "GROUP BY [MainDeptID],[MainDeptName] ";
                    labourAndCapitalMemberVoterDeptsConfig = dbFun.DoQuery(strSQL, parameter).ToList<LabourAndCapitalMemberVoterDeptsConfig>().ToList();
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(部門查詢)失敗，原因：" + ex.Message);
                throw;
            }

            return labourAndCapitalMemberVoterDeptsConfig;
        }

        /// <summary>
        /// 勞資委員投票(查詢)
        /// </summary>
        public LabourAndCapitalMemberViewModel PostLabourAndCapitalMemberSingle(LabourAndCapitalMemberQueryModel query)
        {
            try
            {
                var VoteResultDateTime = String.Empty;
                #region - 查詢 -

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 64, Value = query.VOTE_YEAR }
                };

                #region  - 勞資委員投票 抬頭資訊 - 

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
                strSQL += "     [ApplicantDept] AS [APPLICANT_DEPT], ";
                strSQL += "     [ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
                strSQL += "     [ApplicantID] AS [APPLICANT_ID], ";
                strSQL += "     [ApplicantName] AS [APPLICANT_NAME], ";
                strSQL += "     null AS [APPLICANT_PHONE], ";
                strSQL += "     [FillerID] AS [FILLER_ID], ";
                strSQL += "     [FillerName] AS [FILLER_NAME], ";
                strSQL += "     [ApplicantDateTime] AS [APPLICANT_DATETIME], ";
                strSQL += "     [Priority] AS [PRIORITY], ";
                strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
                strSQL += "     [FlowActivated] AS [FLOW_ACTIVATED], ";
                strSQL += "     null AS [FM7_SUBJECT], ";
                strSQL += "     [ProjectCode] AS [PROJECT_CODE], ";
                strSQL += "     [ModifyDateTime] AS [MODIFY_DATETIME], ";
                strSQL += "     [VoteYear] AS [VOTE_YEAR] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                var TitleInfo = dbFun.DoQuery(strSQL, parameter).ToList<TitleInfo>().FirstOrDefault();

                #endregion

                if (!String.IsNullOrEmpty(TitleInfo.VOTE_YEAR) || !String.IsNullOrWhiteSpace(TitleInfo.VOTE_YEAR))
                {
                    #region - 投票公佈日 -

                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [VoteResultDateTime] AS [VOTE_RESULT_DATE_TIME] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";
                    var dt = dbFun.DoQuery(strSQL, parameter);

                    if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()) || !String.IsNullOrWhiteSpace(dt.Rows[0][0].ToString()))
                    {
                        VoteResultDateTime = dt.Rows[0][0].ToString();
                        if (DateTime.Now >= DateTime.Parse(VoteResultDateTime))
                        {
                            #region - 計算總投票數 -

                            strSQL = "";
                            strSQL += "DECLARE @VoteNum INT ";
                            strSQL += "WITH [CountMemberCTE]([CountMember]) AS ";
                            strSQL += "( ";
                            strSQL += "     SELECT ";
                            strSQL += "         COUNT([MemberID1]) AS [CountMember] ";
                            strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                            strSQL += "     WHERE [MemberID1] IS NOT NULL ";
                            strSQL += "     UNION ALL ";
                            strSQL += "     SELECT ";
                            strSQL += "         COUNT([MemberID2]) AS [CountMember] ";
                            strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                            strSQL += "     WHERE [MemberID2] IS NOT NULL ";
                            strSQL += ") ";
                            strSQL += "SELECT ";
                            strSQL += "     @VoteNum=SUM([CountMember]) ";
                            strSQL += "FROM [CountMemberCTE] AS CTE; ";
                            strSQL += " ";
                            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                            strSQL += "SET [VoteNumTotal]=@VoteNum ";
                            strSQL += "WHERE 1=1 ";
                            strSQL += "         AND VoteYear=@VOTE_YEAR ";
                            dbFun.DoTran(strSQL, parameter);

                            #endregion                                                        
                        }
                    }

                    #endregion
                }

                #region - 勞資委員投票 內容資訊 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [StartDateTime] AS [START_DATE_TIME], ";
                strSQL += "     [EndDateTime] AS [END_DATE_TIME], ";
                strSQL += "     [VoteNumTotal] AS [VOTE_NUM_TOTAL], ";
                strSQL += "     [VoteResultDateTime] AS [VOTE_RESULT_DATE_TIME] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                var labourAndCapitalMemberConfig = dbFun.DoQuery(strSQL, parameter).ToList<LabourAndCapitalMemberConfig>().FirstOrDefault();

                #endregion

                #region - 勞資委員投票 勞方代表 -

                #region - 當前登入人員的主要職責主部門編號 -

                if (!String.IsNullOrEmpty(query.LOGIN_ID) || !String.IsNullOrWhiteSpace(query.LOGIN_ID))
                {
                    var labourAndCapitalMemberQueryModel = new LabourAndCapitalMemberQueryModel()
                    {
                        LOGIN_ID = query.LOGIN_ID,
                        VOTE_YEAR = query.VOTE_YEAR,
                    };
                    parameter.Add(new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = PostLabourAndCapitalMemberVoterInfoSingle(labourAndCapitalMemberQueryModel).MAIN_DEPT_ID });
                }

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [MainDeptActualVoteTurnout] AS [MAIN_DEPT_ACTUAL_VOTE_TURNOUT], ";
                strSQL += "     [MainDeptActualVoteNum] AS [MAIN_DEPT_ACTUAL_VOTE_NUM], ";
                strSQL += "     [IsLabour] AS [IS_LABOUR], ";
                strSQL += "     [MainDeptID] AS [MAIN_DEPT_ID], ";
                strSQL += "     [MainDeptName] AS [MAIN_DEPT_NAME], ";
                strSQL += "     [MemberDeptID] AS [MEMBER_DEPT_ID], ";
                strSQL += "     [MemberDeptName] AS [MEMBER_DEPT_NAME], ";
                strSQL += "     [MemberID] AS [MEMBER_ID], ";
                strSQL += "     [MemberName] [MEMBER_NAME], ";
                strSQL += "     [VoteNum] AS [VOTE_NUM], ";
                strSQL += "     [Note] AS [NOTE] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                if (!String.IsNullOrEmpty(query.LOGIN_ID) || !String.IsNullOrWhiteSpace(query.LOGIN_ID))
                {
                    strSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID ";
                }

                var labourAndCapitalMemberLaboursConfig = dbFun.DoQuery(strSQL, parameter).ToList<LabourAndCapitalMemberLaboursConfig>().OrderBy(L => L.MAIN_DEPT_ID).ToList();

                #region - 投票公布日 -

                if (!String.IsNullOrEmpty(VoteResultDateTime) || !String.IsNullOrWhiteSpace(VoteResultDateTime))
                {
                    if (DateTime.Now >= DateTime.Parse(VoteResultDateTime))
                    {
                        labourAndCapitalMemberLaboursConfig.ForEach(L =>
                        {
                            if (parameter.Any(SP => SP.ParameterName.Contains("@MAIN_DEPT_ID"))) parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@MAIN_DEPT_ID")).FirstOrDefault());

                            parameter.Add(new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = L.MAIN_DEPT_ID });

                            #region - 計算主要部門投票票數 -

                            strSQL = "";
                            strSQL += "DECLARE @VoteNum INT ";
                            strSQL += "WITH [MainDeptActualVoteNumCTE]([CountMainDeptActualVoteNum]) AS ";
                            strSQL += "( ";
                            strSQL += "     SELECT ";
                            strSQL += "         COUNT([MemberID1]) AS [CountMainDeptActualVoteNum] ";
                            strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                            strSQL += "     WHERE 1=1 ";
                            strSQL += "             AND [VoteYear]=@VOTE_YEAR ";
                            strSQL += "             AND [MainDeptID]=@MAIN_DEPT_ID ";
                            strSQL += "             AND [MemberID1] IS NOT NULL ";
                            strSQL += "             AND [MemberID1] <>'' ";
                            strSQL += "     UNION ALL ";
                            strSQL += "     SELECT ";
                            strSQL += "         COUNT([MemberID2]) AS [CountMainDeptActualVoteNum] ";
                            strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                            strSQL += "     WHERE 1=1 ";
                            strSQL += "             AND [VoteYear]=@VOTE_YEAR ";
                            strSQL += "             AND [MainDeptID]=@MAIN_DEPT_ID ";
                            strSQL += "             AND [MemberID2] IS NOT NULL ";
                            strSQL += "             AND [MemberID2] <>'' ";
                            strSQL += ") ";
                            strSQL += "SELECT ";
                            strSQL += "     @VoteNum=SUM([CountMainDeptActualVoteNum]) ";
                            strSQL += "FROM [MainDeptActualVoteNumCTE] AS CTE; ";
                            strSQL += " ";
                            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                            strSQL += "SET [MainDeptActualVoteNum]=@VoteNum ";
                            strSQL += "WHERE 1=1 ";
                            strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                            strSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID ";
                            dbFun.DoTran(strSQL, parameter);

                            #endregion

                            #region - 部門實際投票人數 -

                            strSQL = "";
                            strSQL += "UPDATE LABOUR ";
                            strSQL += "SET [MainDeptActualVoteTurnout]=(SELECT COUNT(LoginID) FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] AS VOTE WHERE LABOUR.[VoteYear]=VOTE.[VoteYear] AND LABOUR.[MainDeptID]=VOTE.[MainDeptID]) ";
                            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] AS LABOUR ";
                            strSQL += "WHERE 1=1 ";
                            strSQL += "         AND LABOUR.[VoteYear]=@VOTE_YEAR ";
                            strSQL += "         AND LABOUR.[MainDeptID]=@MAIN_DEPT_ID ";
                            dbFun.DoTran(strSQL, parameter);

                            #endregion

                        });
                    }
                }

                #endregion

                #endregion

                #region - 勞方委員投票 附件 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     null AS [FILE_PATH], ";
                strSQL += "     [VoteYear] AS [VOTE_YEAR], ";
                strSQL += "     [UplodTime] AS [UPLOD_TIME], ";
                strSQL += "     [Identify] AS [IDENTIFY], ";
                strSQL += "     [AccountID] AS [ACCOUNT_ID], ";
                strSQL += "     [MemberName] AS [MEMBER_NAME], ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
                strSQL += "     [ProcessID] AS [PROCESS_ID], ";
                strSQL += "     [ProcessName] AS [PROCESS_NAME], ";
                strSQL += "     [NFileName] AS [N_FILE_NAME], ";
                strSQL += "     [OFileName] AS [O_FILE_NAME], ";
                strSQL += "     [FileSize] AS [FILE_SIZE], ";
                strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
                strSQL += "     [Remark] AS [REMARK] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_F] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                var labourAndCapitalMemberFilesConfig = dbFun.DoQuery(strSQL, parameter).ToList<LabourAndCapitalMemberFilesConfig>();
                labourAndCapitalMemberFilesConfig.ForEach(F =>
                {
                    F.FILE_PATH = AttachfilePath + "\\" + F.N_FILE_NAME;
                });

                #endregion

                var labourAndCapitalMemberViewModel = new LabourAndCapitalMemberViewModel()
                {
                    TITLE_INFO = TitleInfo,
                    LABOUR_AND_CAPITAL_MEMBER_CONFIG = labourAndCapitalMemberConfig,
                    LABOUR_AND_CAPITAL_MEMBER_LABOURS_CONFIG = labourAndCapitalMemberLaboursConfig,
                    LABOUR_AND_CAPITAL_MEMBER_FILES_CONFIG = labourAndCapitalMemberFilesConfig,
                };

                return labourAndCapitalMemberViewModel;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(查詢)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 勞資委員投票(新建/調整)
        /// </summary>
        public bool PutLabourAndCapitalMemberSingle(LabourAndCapitalMemberViewModel model)
        {
            bool vResult = false;

            try
            {
                model.TITLE_INFO.APPLICANT_DEPT_NAME = sysCommonRepository.GetGTVDeptTree().Where(D => D.DEPT_ID == model.TITLE_INFO.APPLICANT_DEPT).Select(D => D.DEPT_NAME).FirstOrDefault();

                var parameterApplicant = new List<SqlParameter>()
                {
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.TITLE_INFO.VOTE_YEAR },
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.TITLE_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.TITLE_INFO.APPLICANT_DEPT_NAME },
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = "BPMSysteam" },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = "BPMSysteam" },
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = "BPMSysteam" },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = "BPMSysteam" },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                var dtA = dbFun.DoQuery(strSQL, parameterApplicant);

                #region - 勞資委員投票 抬頭資訊：LabourAndCapitalMember_M -

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strREQ = dtA.Rows[0][0].ToString();
                    parameterApplicant.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ });
                    parameterApplicant.Add(new SqlParameter("@MODIFY_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [ModifyDateTime] =@MODIFY_DATETIME ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strREQ = Guid.NewGuid().ToString();
                    parameterApplicant.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ });

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[VoteYear]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@VOTE_YEAR) ";

                    #endregion
                }
                dbFun.DoTran(strSQL, parameterApplicant);

                #endregion

                #region - 勞資委員投票 內容資訊：LabourAndCapitalMember_M -

                if (model.LABOUR_AND_CAPITAL_MEMBER_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //勞資委員投票 內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64 , Value = strREQ },
                        new SqlParameter("@START_DATE_TIME", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@END_DATE_TIME", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@VOTE_NUM_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@VOTE_RESULT_DATE_TIME", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    strJson = jsonFunction.ObjectToJSON(model.LABOUR_AND_CAPITAL_MEMBER_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [StartDateTime]=@START_DATE_TIME, ";
                    strSQL += "     [EndDateTime]=@END_DATE_TIME, ";
                    strSQL += "     [VoteNumTotal]=@VOTE_NUM_TOTAL,";
                    strSQL += "     [VoteResultDateTime]=@VOTE_RESULT_DATE_TIME ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);
                }

                #endregion

                #region - 勞資委員投票 勞方代表：LabourAndCapitalMember_LABOUR -

                if (model.LABOUR_AND_CAPITAL_MEMBER_LABOURS_CONFIG != null && model.LABOUR_AND_CAPITAL_MEMBER_LABOURS_CONFIG.Count > 0)
                {
                    var parameterLabour = new List<SqlParameter>()
                    {
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64 , Value = strREQ },
                        new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.TITLE_INFO.VOTE_YEAR },
                    };

                    #region 先刪除今年的參選人

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterLabour);

                    #endregion

                    foreach (var item in model.LABOUR_AND_CAPITAL_MEMBER_LABOURS_CONFIG)
                    {
                        var labourAndCapitalMemberStaffConfig = new LabourAndCapitalMemberStaffConfig()
                        {
                            VOTE_YEAR = model.TITLE_INFO.VOTE_YEAR,
                            MEMBER_DEPT_ID = item.MEMBER_DEPT_ID,
                            MEMBER_ID = item.MEMBER_ID,
                        };
                        PutLabourAndCapitalMemberAddStaffSingle(labourAndCapitalMemberStaffConfig);
                    }
                }

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(新建/調整)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        /// <summary>
        /// 勞資委員投票(投票)
        /// </summary>
        public bool PutLabourAndCapitalMemberVoteSingle(LabourAndCapitalMemberVoteConfig model)
        {
            bool vResult = false;
            try
            {
                var labourAndCapitalMemberQueryModel = new LabourAndCapitalMemberQueryModel()
                {
                    LOGIN_ID = model.LOGIN_ID,
                    VOTE_YEAR = model.VOTE_YEAR,
                };
                var VoterInfo = PostLabourAndCapitalMemberVoterInfoSingle(labourAndCapitalMemberQueryModel);

                if (VoterInfo.IS_VOTE)
                {
                    var logonModel = new LogonModel()
                    {
                        USER_ID = model.LOGIN_ID
                    };
                    var userModel = userRepository.PostUserSingle(logonModel);

                    if (userModel != null && userModel.USER_MODEL.Count > 0)
                    {
                        #region - 當前登入人員的主要職責部門編號 -

                        var LoginDeptIDs = new List<string>();
                        LoginDeptIDs = userModel.USER_MODEL.Where(U => U.USER_ID == model.LOGIN_ID).Select(U => U.DEPT_ID).ToList();
                        LoginDeptIDs.ForEach(L =>
                        {
                            var userInfoMainDeptModel = new UserInfoMainDeptModel()
                            {
                                USER_ID = model.LOGIN_ID,
                                DEPT_ID = L,
                                COMPANY_ID = userModel.USER_MODEL.Where(U => U.DEPT_ID == L).Select(U => U.COMPANY_ID).FirstOrDefault(),
                            };
                            var userInfoMainDept = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel);
                            if (userInfoMainDept.USER_MODEL.IS_MAIN_JOB != null)
                            {
                                if (bool.Parse(userInfoMainDept.USER_MODEL.IS_MAIN_JOB.ToString()))
                                {
                                    model.DEPT_ID = userInfoMainDept.USER_MODEL.DEPT_ID;
                                }
                            }
                        });

                        #endregion

                        var parameter = new List<SqlParameter>()
                        {
                            new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                            new SqlParameter("@DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@LOGIN_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@MEMBER_ID_1", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@MEMBER_ID_2", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        };

                        strJson = jsonFunction.ObjectToJSON(model);
                        GlobalParameters.Infoparameter(strJson, parameter);

                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "      [RequisitionID] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                        strSQL += "         AND [LoginID]=@LOGIN_ID ";

                        var dt = dbFun.DoQuery(strSQL, parameter);

                        if (dt.Rows.Count <= 0)
                        {
                            strSQL = "";
                            strSQL += "SELECT ";
                            strSQL += "      [RequisitionID] ";
                            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                            strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                            var dtA = dbFun.DoQuery(strSQL, parameter);
                            if (dtA.Rows.Count > 0)
                            {
                                strREQ = dtA.Rows[0][0].ToString();

                                parameter.Add(new SqlParameter("@VOTE_DATE", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });
                                parameter.Add(new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = VoterInfo.MAIN_DEPT_ID });
                                parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ });

                                #region - 投票 -

                                strSQL = "";
                                strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE]([RequisitionID],[VoteYear],[MainDeptID],[DeptID],[LoginID],[MemberID1],[MemberID2],[VoteDate]) ";
                                strSQL += "VALUES(@REQUISITION_ID,@VOTE_YEAR,@MAIN_DEPT_ID,@DEPT_ID,@LOGIN_ID,@MEMBER_ID_1,@MEMBER_ID_2,@VOTE_DATE) ";

                                dbFun.DoTran(strSQL, parameter);

                                #endregion

                                #region - 計算主要部門投票票數 -

                                strSQL = "";
                                strSQL += "DECLARE @VoteNum INT ";
                                strSQL += "WITH [MainDeptActualVoteNumCTE]([CountMainDeptActualVoteNum]) AS ";
                                strSQL += "( ";
                                strSQL += "     SELECT ";
                                strSQL += "         COUNT([MemberID1]) AS [CountMainDeptActualVoteNum] ";
                                strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                                strSQL += "     WHERE 1=1 ";
                                strSQL += "             AND [VoteYear]=@VOTE_YEAR ";
                                strSQL += "             AND [MainDeptID]=@MAIN_DEPT_ID ";
                                strSQL += "             AND [MemberID1] IS NOT NULL ";
                                strSQL += "             AND [MemberID1] <>'' ";
                                strSQL += "     UNION ALL ";
                                strSQL += "     SELECT ";
                                strSQL += "         COUNT([MemberID2]) AS [CountMainDeptActualVoteNum] ";
                                strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                                strSQL += "     WHERE 1=1 ";
                                strSQL += "             AND [VoteYear]=@VOTE_YEAR ";
                                strSQL += "             AND [MainDeptID]=@MAIN_DEPT_ID ";
                                strSQL += "             AND [MemberID2] IS NOT NULL ";
                                strSQL += "             AND [MemberID2] <>'' ";
                                strSQL += ") ";
                                strSQL += "SELECT ";
                                strSQL += "     @VoteNum=SUM([CountMainDeptActualVoteNum]) ";
                                strSQL += "FROM [MainDeptActualVoteNumCTE] AS CTE; ";
                                strSQL += " ";
                                strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                                strSQL += "SET [MainDeptActualVoteNum]=@VoteNum ";
                                strSQL += "WHERE 1=1 ";
                                strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                                strSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID ";
                                dbFun.DoTran(strSQL, parameter);

                                #endregion

                                #region - 候選人票數計算 -

                                var parameterMember = new List<SqlParameter>()
                                {
                                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                                };

                                var Members = new List<string>()
                                {
                                    model.MEMBER_ID_1,
                                    model.MEMBER_ID_2,
                                };

                                Members.ForEach(M =>
                                {
                                    if (!String.IsNullOrEmpty(M) || !String.IsNullOrWhiteSpace(M))
                                    {
                                        parameterMember.Add(new SqlParameter("@MEMBER_ID", SqlDbType.NVarChar) { Size = 40, Value = M });

                                        strSQL = "";
                                        strSQL = "";
                                        strSQL += "SELECT ";
                                        strSQL += "      [RequisitionID] ";
                                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                                        strSQL += "WHERE 1=1 ";
                                        strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                                        strSQL += "         AND [MemberID]=@MEMBER_ID ";

                                        var dtL = dbFun.DoQuery(strSQL, parameterMember);
                                        if (dtL.Rows.Count > 0)
                                        {
                                            strSQL = "";
                                            strSQL += "DECLARE @VoteNum INT ";
                                            strSQL += "WITH [CountVoteNumCTE]([CountVoteNum]) AS ";
                                            strSQL += "( ";
                                            strSQL += "     SELECT ";
                                            strSQL += "         COUNT([MemberID1]) AS [CountVoteNum] ";
                                            strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                                            strSQL += "     WHERE 1=1 ";
                                            strSQL += "             AND [VoteYear]=@VOTE_YEAR ";
                                            strSQL += "             AND [MemberID1]=@MEMBER_ID ";
                                            strSQL += "     UNION ALL ";
                                            strSQL += "     SELECT ";
                                            strSQL += "         COUNT([MemberID2]) AS [CountVoteNum] ";
                                            strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                                            strSQL += "     WHERE 1=1 ";
                                            strSQL += "             AND [VoteYear]=@VOTE_YEAR ";
                                            strSQL += "             AND [MemberID2]=@MEMBER_ID ";
                                            strSQL += ") ";
                                            strSQL += "SELECT ";
                                            strSQL += "     @VoteNum=SUM([CountVoteNum]) ";
                                            strSQL += "FROM [CountVoteNumCTE] AS CTE; ";
                                            strSQL += " ";
                                            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                                            strSQL += "SET [VoteNum]=@VoteNum ";
                                            strSQL += "WHERE 1=1 ";
                                            strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                                            strSQL += "         AND [MemberID]=@MEMBER_ID ";
                                            dbFun.DoTran(strSQL, parameterMember);
                                        }
                                        parameterMember.Remove(parameterMember.Where(SP => SP.ParameterName.Contains("@MEMBER_ID")).FirstOrDefault());
                                    }
                                });

                                #endregion

                                #region - 部門實際投票人數 -

                                strSQL = "";
                                strSQL += "UPDATE LABOUR ";
                                strSQL += "SET [MainDeptActualVoteTurnout]=(SELECT COUNT(LoginID) FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] AS VOTE WHERE LABOUR.[VoteYear]=VOTE.[VoteYear] AND LABOUR.[MainDeptID]=VOTE.[MainDeptID]) ";
                                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] AS LABOUR ";
                                strSQL += "WHERE 1=1 ";
                                strSQL += "         AND LABOUR.[VoteYear]=@VOTE_YEAR ";
                                strSQL += "         AND LABOUR.[MainDeptID]=@MAIN_DEPT_ID ";
                                dbFun.DoTran(strSQL, parameter);

                                #endregion

                                vResult = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(投票)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        /// <summary>
        /// 勞資委員投票(清除主部門票箱)
        /// </summary>
        public bool PutLabourAndCapitalMemberClearMainDeptVoteSingle(LabourAndCapitalMemberClearMainDeptVoteConfig model)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                    new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.MAIN_DEPT_ID },
                };

                #region - 清除主部門候選人票數 -

                strSQL = "";
                strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                strSQL += "SET [VoteNum] = null, ";
                strSQL += "     [MainDeptActualVoteNum] = null, ";
                strSQL += "     [IsLabour] = null ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                strSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID";
                dbFun.DoTran(strSQL, parameter);

                #endregion

                #region - 清除當年度主部門投票結果 -

                strSQL = "";
                strSQL += "DELETE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_VOTE] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                strSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID";
                dbFun.DoTran(strSQL, parameter);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(清除主部門票箱)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        /// <summary>
        /// 勞資委員投票(附件:新增F表)
        /// 檔案手動上傳到(C/D):\\NTWEB\\AutoWeb3\\Database\\Project\\BPM\\BPMPro\\object路徑
        /// </summary>
        public List<LabourAndCapitalMemberFilesConfig> PutLabourAndCapitalMemberFilesSingle(LabourAndCapitalMemberFilesConfig model)
        {
            bool vResult = false;
            try
            {
                #region - 檔案上傳:BPM與API會有跨機器、跨域問題，因此改由手動上傳檔案 -

                //var path = AttachfilePath + "\\" + IDENTIFY;

                //#region - 確認複製路徑 -

                //if (!Directory.Exists(path))
                //{
                //    //確認是否有資料夾沒有的話就自動新增
                //    //新增資料夾
                //    Directory.CreateDirectory(path);
                //}

                //#endregion

                //if (HttpContext.Current.Request.Files.Count > 0)
                //{
                //    var parameter = new List<SqlParameter>()
                //    {
                //        new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                //        new SqlParameter("@UPLOD_TIME", SqlDbType.DateTime) { Value = DateTime.Now },
                //        new SqlParameter("@IDENTIFY", SqlDbType.NVarChar) { Size = 100, Value = IDENTIFY },
                //        new SqlParameter("@ACCOUNT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)model.ACCOUNT_ID ?? "BPMSysteam" },
                //        new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                //        new SqlParameter("@PROCESS_ID", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                //        new SqlParameter("@PROCESS_NAME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                //        new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = 0 },
                //        new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size=255, Value= (object)model.REMARK ?? DBNull.Value },
                //    };

                //    strSQL = "";
                //    strSQL += "SELECT ";
                //    strSQL += "      [RequisitionID] ";
                //    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                //    strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                //    var dt = dbFun.DoQuery(strSQL, parameter);

                //    if (dt.Rows.Count > 0)
                //    {
                //        var MemberName = String.Empty;
                //        if (!String.IsNullOrEmpty(model.ACCOUNT_ID) || !String.IsNullOrWhiteSpace(model.ACCOUNT_ID))
                //        {
                //            var logonModel = new LogonModel()
                //            {
                //                USER_ID = model.ACCOUNT_ID
                //            };
                //            var userModel = userRepository.PostUserSingle(logonModel);

                //            MemberName = userModel.USER_MODEL.Where(U => U.USER_ID == model.ACCOUNT_ID).Select(U => U.USER_NAME).FirstOrDefault();

                //        }
                //        else MemberName = "BPMSysteam";
                //        parameter.Add(new SqlParameter("@MEMBER_NAME", SqlDbType.NVarChar) { Size = 40, Value = MemberName });
                //        parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = dt.Rows[0][0].ToString() });

                //        int i = 0;
                //        while (i <= (HttpContext.Current.Request.Files.Count - 1))
                //        {
                //            var fileResult = false;
                //            var File = HttpContext.Current.Request.Files[i];

                //            #region - 檔案大小不可大於 : 3MB -

                //            if (File.ContentLength > 0 || File.ContentLength <= 3145728)
                //            {
                //                fileResult = true;
                //            }

                //            #endregion

                //            #region - 限制上傳檔案種類 -

                //            switch (File.ContentType)
                //            {
                //                case "application/pdf":
                //                case "application/msword":
                //                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                //                    fileResult = true;
                //                    break;
                //                default: break;
                //            }

                //            #endregion

                //            if (fileResult)
                //            {
                //                var fileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
                //                //如果檔案大小、檔案種類都對才會儲存檔案    

                //                parameter.Add(new SqlParameter("@N_FILE_NAME", SqlDbType.NVarChar) { Size = 200, Value = IDENTIFY + "\\" + fileName });
                //                parameter.Add(new SqlParameter("@O_FILE_NAME", SqlDbType.NVarChar) { Size = 200, Value = File.FileName });
                //                parameter.Add(new SqlParameter("@FILE_SIZE", SqlDbType.Int) { Value = File.ContentLength });

                //                #region - 寫入附件F表 -

                //                strSQL = "";
                //                strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_LabourAndCapitalMember_F]([VoteYear],[UplodTime],[Identify],[AccountID],[MemberName],[RequisitionID],[DiagramID],[ProcessID],[ProcessName],[NFileName],[OFileName],[FileSize],[DraftFlag],[Remark]) ";
                //                strSQL += "VALUES(@VOTE_YEAR,@UPLOD_TIME,@IDENTIFY,@ACCOUNT_ID,@MEMBER_NAME,@REQUISITION_ID,@DIAGRAM_ID,@PROCESS_ID,@PROCESS_NAME,@N_FILE_NAME,@O_FILE_NAME,@FILE_SIZE,@DRAFT_FLAG,@REMARK) ";
                //                dbFun.DoTran(strSQL, parameter);

                //                #endregion

                //                #region - 儲存檔案 -

                //                File.SaveAs(path + "\\" + fileName);

                //                #endregion

                //                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@N_FILE_NAME")).FirstOrDefault());
                //                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@O_FILE_NAME")).FirstOrDefault());
                //                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@FILE_SIZE")).FirstOrDefault());
                //            }

                //            i++;
                //        }
                //        vResult = true;
                //    }
                //}

                #endregion

                var labourAndCapitalMemberFilesConfig = new List<LabourAndCapitalMemberFilesConfig>();

                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                    new SqlParameter("@UPLOD_TIME", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@IDENTIFY", SqlDbType.NVarChar) { Size = 100, Value = IDENTIFY },
                    new SqlParameter("@ACCOUNT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)model.ACCOUNT_ID ?? "BPMSysteam" },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROCESS_ID", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PROCESS_NAME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = 0 },
                    new SqlParameter("@REMARK", SqlDbType.NVarChar) { Size=255, Value= (object)model.REMARK ?? DBNull.Value },
                };

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "      [RequisitionID] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                    var dt = dbFun.DoQuery(strSQL, parameter);

                    if (dt.Rows.Count > 0)
                    {
                        var MemberName = String.Empty;
                        if (!String.IsNullOrEmpty(model.ACCOUNT_ID) || !String.IsNullOrWhiteSpace(model.ACCOUNT_ID))
                        {
                            var logonModel = new LogonModel()
                            {
                                USER_ID = model.ACCOUNT_ID
                            };
                            var userModel = userRepository.PostUserSingle(logonModel);

                            MemberName = userModel.USER_MODEL.Where(U => U.USER_ID == model.ACCOUNT_ID).Select(U => U.USER_NAME).FirstOrDefault();

                        }
                        else MemberName = "BPMSysteam";
                        parameter.Add(new SqlParameter("@MEMBER_NAME", SqlDbType.NVarChar) { Size = 40, Value = MemberName });
                        parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = dt.Rows[0][0].ToString() });

                        int i = 0;
                        while (i <= (HttpContext.Current.Request.Files.Count - 1))
                        {
                            var fileResult = true;
                            var File = HttpContext.Current.Request.Files[i];

                            if (fileResult)
                            {
                                var fileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
                                //如果檔案大小、檔案種類都對才會儲存檔案    

                                parameter.Add(new SqlParameter("@N_FILE_NAME", SqlDbType.NVarChar) { Size = 200, Value = IDENTIFY + "\\" + fileName });
                                parameter.Add(new SqlParameter("@O_FILE_NAME", SqlDbType.NVarChar) { Size = 200, Value = File.FileName });
                                parameter.Add(new SqlParameter("@FILE_SIZE", SqlDbType.Int) { Value = File.ContentLength });

                                #region - 寫入附件F表 -

                                strSQL = "";
                                strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_F]([VoteYear],[UplodTime],[Identify],[AccountID],[MemberName],[RequisitionID],[DiagramID],[ProcessID],[ProcessName],[NFileName],[OFileName],[FileSize],[DraftFlag],[Remark]) ";
                                strSQL += "VALUES(@VOTE_YEAR,@UPLOD_TIME,@IDENTIFY,@ACCOUNT_ID,@MEMBER_NAME,@REQUISITION_ID,@DIAGRAM_ID,@PROCESS_ID,@PROCESS_NAME,@N_FILE_NAME,@O_FILE_NAME,@FILE_SIZE,@DRAFT_FLAG,@REMARK) ";
                                dbFun.DoTran(strSQL, parameter);

                                #endregion

                                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@N_FILE_NAME")).FirstOrDefault());
                                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@O_FILE_NAME")).FirstOrDefault());
                                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@FILE_SIZE")).FirstOrDefault());
                            }

                            i++;
                        }
                        vResult = true;
                    }
                }

                #region - 檢視上傳資訊 -

                if (vResult)
                {
                    //方便更改要上傳的檔案檔名
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     null AS [FILE_PATH], ";
                    strSQL += "     [VoteYear] AS [VOTE_YEAR], ";
                    strSQL += "     [UplodTime] AS [UPLOD_TIME], ";
                    strSQL += "     [Identify] AS [IDENTIFY], ";
                    strSQL += "     [AccountID] AS [ACCOUNT_ID], ";
                    strSQL += "     [MemberName] AS [MEMBER_NAME], ";
                    strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                    strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
                    strSQL += "     [ProcessID] AS [PROCESS_ID], ";
                    strSQL += "     [ProcessName] AS [PROCESS_NAME], ";
                    strSQL += "     [NFileName] AS [N_FILE_NAME], ";
                    strSQL += "     [OFileName] AS [O_FILE_NAME], ";
                    strSQL += "     [FileSize] AS [FILE_SIZE], ";
                    strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
                    strSQL += "     [Remark] AS [REMARK] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_F] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                    labourAndCapitalMemberFilesConfig = dbFun.DoQuery(strSQL, parameter).ToList<LabourAndCapitalMemberFilesConfig>().ToList();
                    labourAndCapitalMemberFilesConfig.ForEach(F =>
                    {
                        F.FILE_PATH = AttachfilePath + "\\" + F.N_FILE_NAME;
                    });
                }

                #endregion

                return labourAndCapitalMemberFilesConfig;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(附件上傳)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 勞資委員投票(當選註記)
        /// </summary>
        public bool PutLabourAndCapitalMemberMarkSingle(LabourAndCapitalMemberStaffConfig model)
        {
            bool vResult = false;

            #region - 宣告 -

            var strDmarkSQL = "";
            var strWhereSQL = "";
            var strSecondSQL = "";
            var strMarkSQL = "";
            var MemberResult = false;

            #endregion

            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";
                var dt = dbFun.DoQuery(strSQL, parameter);
                if (dt.Rows.Count > 0)
                {
                    #region 常用的WHERE

                    strWhereSQL += "AND [VoteYear]=@VOTE_YEAR ";
                    strWhereSQL += "AND MainDeptID=@MAIN_DEPT_ID ";

                    #endregion

                    strDmarkSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                    strDmarkSQL += "SET [IsLabour]=null ";
                    strDmarkSQL += "WHERE 1=1 ";
                    strDmarkSQL += strWhereSQL;

                    if (String.IsNullOrEmpty(model.MEMBER_ID) || String.IsNullOrWhiteSpace(model.MEMBER_ID))
                    {
                        #region - 註記當選人與備取 -

                        #region 註記B.備取人員

                        strSecondSQL += "UPDATE BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR ";
                        strSecondSQL += "SET IsLabour='B' ";
                        strSecondSQL += "WHERE 1=1 ";
                        strSecondSQL += strWhereSQL;
                        strSecondSQL += "AND ISNULL([VoteNum],0)=@SecondNum ";

                        #endregion

                        #region - 當選註記_strSQL -

                        strSQL = "";
                        strSQL += "DECLARE @MaxNum INT ";
                        strSQL += "DECLARE @SecondNum INT ";
                        //檢視當前部門最高票
                        strSQL += "SELECT ";
                        strSQL += "     @MaxNum=MAX(ISNULL([VoteNum],0)) ";
                        strSQL += "FROM BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += strWhereSQL;
                        strSQL += "IF @MaxNum <>0 ";
                        strSQL += "     BEGIN ";
                        //註記A.當選人
                        strSQL += "         UPDATE BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR ";
                        strSQL += "         SET IsLabour='A' ";
                        strSQL += "         WHERE 1=1 ";
                        strSQL += strWhereSQL;
                        strSQL += "                 AND ISNULL([VoteNum],0)=@MaxNum ";
                        //檢視當前部門第二高票
                        strSQL += "         SELECT ";
                        strSQL += "             @SecondNum=MAX(ISNULL([VoteNum],0)) ";
                        strSQL += "         FROM BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR ";
                        strSQL += "         WHERE 1=1 ";
                        strSQL += strWhereSQL;
                        strSQL += "                 AND ISNULL([VoteNum],0)<@MaxNum ";
                        //檢視當年參選部門人數而定備取人員
                        strSQL += "         IF (SELECT COUNT(MemberDeptID) FROM BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR WHERE 1=1 " + strWhereSQL + ")>2 ";
                        strSQL += "             BEGIN ";
                        strSQL += "                 IF @SecondNum <>0 ";
                        strSQL += "                     BEGIN ";
                        //註記B.備取人員
                        strSQL += strSecondSQL;
                        strSQL += "                     END ";
                        strSQL += "             END ";
                        strSQL += "         ELSE ";
                        strSQL += "             BEGIN ";
                        //註記B.備取人員
                        strSQL += strSecondSQL;
                        strSQL += "             END ";
                        strSQL += "     END ";
                        strMarkSQL = strSQL;

                        #endregion

                        if (String.IsNullOrEmpty(model.MAIN_DEPT_ID) || String.IsNullOrWhiteSpace(model.MAIN_DEPT_ID))
                        {
                            var labourAndCapitalMemberQueryModel = new LabourAndCapitalMemberQueryModel()
                            {
                                VOTE_YEAR = model.VOTE_YEAR
                            };

                            PostLabourAndCapitalMemberVoterDeptsSingle(labourAndCapitalMemberQueryModel).ForEach(D =>
                            {
                                parameter.Add(new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = D.MAIN_DEPT_ID });

                                //清除註記資料
                                dbFun.DoTran(strDmarkSQL, parameter);
                                //進行新的註記
                                dbFun.DoTran(strMarkSQL, parameter);

                                parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@MAIN_DEPT_ID")).FirstOrDefault());
                            });

                            vResult = true;
                        }
                        else
                        {
                            parameter.Add(new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.MAIN_DEPT_ID });

                            //對部門：
                            //清除註記資料
                            dbFun.DoTran(strDmarkSQL, parameter);
                            //進行新的註記
                            dbFun.DoTran(strMarkSQL, parameter);

                            parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@MAIN_DEPT_ID")).FirstOrDefault());

                            vResult = true;
                        }

                        #endregion
                    }
                    else
                    {

                        #region - 勞方代表的主要部門 -

                        var logonModel = new LogonModel()
                        {
                            USER_ID = model.MEMBER_ID
                        };
                        var UserModel = userRepository.PostUserSingle(logonModel).USER_MODEL;

                        if (UserModel != null && UserModel.Count > 0)
                        {
                            UserModel.ForEach(U =>
                            {
                                var userInfoMainDeptModel = new UserInfoMainDeptModel()
                                {
                                    USER_ID = model.MEMBER_ID,
                                    DEPT_ID = U.DEPT_ID,
                                    COMPANY_ID = UserModel.Where(U2 => U2.DEPT_ID == U.DEPT_ID).Select(U2 => U2.COMPANY_ID).FirstOrDefault(),
                                };
                                if (sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).USER_MODEL.IS_MAIN_JOB != null)
                                {
                                    if (bool.Parse(sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).USER_MODEL.IS_MAIN_JOB.ToString()))
                                    {
                                        model.MAIN_DEPT_ID = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel).MAIN_DEPT.DEPT_ID;
                                    }
                                }
                            });
                        }

                        #endregion

                        parameter.Add(new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.MAIN_DEPT_ID });
                        parameter.Add(new SqlParameter("@MEMBER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.MEMBER_ID });
                        parameter.Add(new SqlParameter("@IS_LABOUR", SqlDbType.NVarChar) { Size = 5, Value = (object)model.IS_LABOUR ?? DBNull.Value });

                        if (model.IS_LABOUR == "A")
                        {
                            //IS_LABOUR是A表示，勞資委員異動須先清空部門的勞資委員註記再，手動新增；備取人員。
                            strDmarkSQL += "        AND [MainDeptID]=@MAIN_DEPT_ID ";
                            //對部門：
                            //清除註記資料
                            dbFun.DoTran(strDmarkSQL, parameter);

                            MemberResult = true;
                        }
                        else if (model.IS_LABOUR == "B")
                        {
                            //如果勞資委員註記沒有當選的勞資委員，則不會記錄備取人員。
                            strSQL = "";
                            strSQL += "SELECT ";
                            strSQL += "     [MemberID] ";
                            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                            strSQL += "WHERE 1=1 ";
                            strSQL += "        AND [VoteYear]=@VOTE_YEAR ";
                            strSQL += "        AND [MainDeptID]=@MAIN_DEPT_ID ";
                            strSQL += "        AND [IsLabour]='A' ";
                            var dtB = dbFun.DoQuery(strSQL, parameter);
                            if (dtB.Rows.Count > 0) MemberResult = true;
                        }
                        else if (String.IsNullOrEmpty(model.IS_LABOUR) || String.IsNullOrWhiteSpace(model.IS_LABOUR)) MemberResult = true;
                        else MemberResult = false;

                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                        strSQL += "SET [IsLabour]=@IS_LABOUR, ";
                        strSQL += "     [Note]=@NOTE ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "        AND [VoteYear]=@VOTE_YEAR ";
                        strSQL += "        AND [MainDeptID]=@MAIN_DEPT_ID ";
                        strSQL += "        AND [MemberID]=@MEMBER_ID ";

                        //進行新的註記
                        if (MemberResult)
                        {
                            parameter.Add(new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 255, Value = (object)model.NOTE ?? DBNull.Value });
                            dbFun.DoTran(strSQL, parameter);
                            vResult = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(當選註記)失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        /// <summary>
        /// 勞資委員投票(新增人員)
        /// </summary>        
        public bool PutLabourAndCapitalMemberAddStaffSingle(LabourAndCapitalMemberStaffConfig model)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    //勞資委員投票 勞方代表
                    new SqlParameter("@VOTE_YEAR", SqlDbType.NVarChar) { Size = 10, Value = model.VOTE_YEAR },
                    new SqlParameter("@MAIN_DEPT_ACTUAL_VOTE_TURNOUT", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MAIN_DEPT_ACTUAL_VOTE_NUM", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_LABOUR", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MAIN_DEPT_ID", SqlDbType.NVarChar) { Size = 40 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MAIN_DEPT_NAME", SqlDbType.NVarChar) { Size = 40 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEMBER_DEPT_ID", SqlDbType.NVarChar) { Size = 40 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEMBER_DEPT_NAME", SqlDbType.NVarChar) { Size = 40 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEMBER_ID", SqlDbType.NVarChar) { Size = 40 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEMBER_NAME", SqlDbType.NVarChar) { Size = 40 , Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@VOTE_NUM", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                var logonModel = new LogonModel()
                {
                    USER_ID = model.MEMBER_ID
                };

                var userInfoMainDeptModel = new UserInfoMainDeptModel()
                {
                    USER_ID = model.MEMBER_ID,
                    DEPT_ID = model.MEMBER_DEPT_ID,
                    COMPANY_ID = userRepository.PostUserSingle(logonModel).USER_MODEL.Where(U => U.DEPT_ID == model.MEMBER_DEPT_ID).Select(U => U.COMPANY_ID).FirstOrDefault(),
                };

                var UserInfoMainDept = sysCommonRepository.PostUserInfoMainDept(userInfoMainDeptModel);

                model.MAIN_DEPT_ID = UserInfoMainDept.MAIN_DEPT.DEPT_ID;
                model.MAIN_DEPT_NAME = UserInfoMainDept.MAIN_DEPT.DEPT_NAME;
                model.MEMBER_DEPT_NAME = UserInfoMainDept.USER_MODEL.DEPT_NAME;
                model.MEMBER_NAME = UserInfoMainDept.USER_MODEL.USER_NAME;

                //寫入：勞資委員投票 勞方代表parameter
                strJson = jsonFunction.ObjectToJSON(model);
                GlobalParameters.Infoparameter(strJson, parameter);

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [VoteYear]=@VOTE_YEAR ";

                var dt = dbFun.DoQuery(strSQL, parameter);

                if (dt.Rows.Count > 0)
                {
                    parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)dt.Rows[0][0].ToString() ?? DBNull.Value });

                    var labourAndCapitalMemberQueryModel = new LabourAndCapitalMemberQueryModel()
                    {
                        VOTE_YEAR = model.VOTE_YEAR,
                    };

                    var strMainDeptActualVoteSQL = "";
                    strMainDeptActualVoteSQL += "DECLARE ";
                    strMainDeptActualVoteSQL += "     @MainDeptActualVoteTurnout Int, ";
                    strMainDeptActualVoteSQL += "     @MainDeptActualVoteNum Int; ";
                    strMainDeptActualVoteSQL += "SELECT ";
                    strMainDeptActualVoteSQL += "     @MainDeptActualVoteTurnout=MainDeptActualVoteTurnout ";
                    strMainDeptActualVoteSQL += "FROM BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR ";
                    strMainDeptActualVoteSQL += "WHERE 1=1 ";
                    strMainDeptActualVoteSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                    strMainDeptActualVoteSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID ";
                    strMainDeptActualVoteSQL += "GROUP BY MainDeptActualVoteTurnout ";
                    strMainDeptActualVoteSQL += "SELECT ";
                    strMainDeptActualVoteSQL += "     @MainDeptActualVoteNum=MainDeptActualVoteNum ";
                    strMainDeptActualVoteSQL += "FROM BPMPro.dbo.FM7T_" + IDENTIFY + "_LABOUR ";
                    strMainDeptActualVoteSQL += "WHERE 1=1 ";
                    strMainDeptActualVoteSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                    strMainDeptActualVoteSQL += "         AND [MainDeptID]=@MAIN_DEPT_ID ";
                    strMainDeptActualVoteSQL += "GROUP BY MainDeptActualVoteNum ";

                    if (!PostLabourAndCapitalMemberSingle(labourAndCapitalMemberQueryModel).LABOUR_AND_CAPITAL_MEMBER_LABOURS_CONFIG.Any(L => L.MEMBER_ID.Contains(model.MEMBER_ID) && L.MEMBER_DEPT_ID.Contains(model.MEMBER_DEPT_ID)))
                    {
                        #region 新增參選人資料

                        strSQL = "";
                        strSQL += strMainDeptActualVoteSQL;
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR]([RequisitionID],[VoteYear],[MainDeptActualVoteTurnout],[MainDeptActualVoteNum],[IsLabour],[MainDeptID],[MainDeptName],[MemberDeptID],[MemberDeptName],[MemberID],[MemberName],[VoteNum],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@VOTE_YEAR,@MainDeptActualVoteTurnout,@MainDeptActualVoteNum,@IS_LABOUR,@MAIN_DEPT_ID,@MAIN_DEPT_NAME,@MEMBER_DEPT_ID,@MEMBER_DEPT_NAME,@MEMBER_ID,@MEMBER_NAME,null,@NOTE) ";

                        dbFun.DoTran(strSQL, parameter);

                        #endregion
                    }
                    else
                    {
                        #region 更新參選人備註

                        strSQL = "";
                        strSQL += strMainDeptActualVoteSQL;
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_LABOUR] ";
                        strSQL += "SET [Note]=@NOTE ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [VoteYear]=@VOTE_YEAR ";
                        strSQL += "         AND [MemberID]=@MEMBER_ID ";
                        strSQL += "         AND [MemberDeptID]=@MEMBER_DEPT_ID ";

                        dbFun.DoTran(strSQL, parameter);

                        #endregion
                    }

                    parameter.Remove(parameter.Where(SP => SP.ParameterName.Contains("@REQUISITION_ID")).FirstOrDefault());

                    vResult = true;
                }

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("勞資委員投票(新增人員)失敗，原因：" + ex.Message);
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
        /// 確認是否為新建的表單
        /// </summary>
        private bool IsADD = false;

        /// <summary>
        /// 表單代號
        /// </summary>
        private string IDENTIFY = "LabourAndCapitalMember";

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

        /// <summary>
        /// 佈版檔案路徑：IIS須設定Attach虛擬目錄
        /// </summary>
        private string AttachfilePath = GlobalParameters.attachFilePathBPMProDev + "Attach";

        /// <summary>
        /// 本機測試檔案路徑
        /// </summary>
        //private string localfilePath = "D:\\NTWEB\\AutoWeb3\\Database\\Project\\BPM\\BPMPro\\object";

        #endregion
    }
}