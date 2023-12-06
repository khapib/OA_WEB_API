using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Ajax.Utilities;
using OA_WEB_API.Models;

namespace OA_WEB_API.Repository
{
    /// <summary>
    /// 會簽管理系統 - 使用者資料
    /// </summary>
    public class UserRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #endregion

        #region - 方法 -

        /// <summary>
        /// 登入成功，取得存取權杖
        /// </summary>
        public TokenModel SignIn(LogonModel model)
        {
            var tokenManager = new TokenManager();

            if (IsVerifyUser(model))
            {
                return tokenManager.Create(PostUserSingle(model));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 使用者資料(單一使用者查詢；兼職會有多筆)
        /// </summary>
        public UserInfo PostUserSingle(LogonModel model)
        {
            var parameter = new List<SqlParameter>()
            {
                new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 64, Value = model.USER_ID }
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     M.[COMPANY_ID], ";
            strSQL += "     M.[PARENT_DEPT_ID], ";
            strSQL += "     M.[DEPT_ID], ";
            strSQL += "     M.[GRADE_ID], ";
            strSQL += "     M.[TITLE_ID], ";
            strSQL += "     M.[PARENT_DEPT_NAME], ";
            strSQL += "     M.[DEPT_NAME], ";
            strSQL += "     M.[GRADE_NAME], ";
            strSQL += "     M.[TITLE_NAME], ";
            strSQL += "     M.[GRADE_NUM], ";
            strSQL += "     M.[SORT_ORDER], ";
            strSQL += "     M.[MANAGER_DEPT_ID], ";
            strSQL += "     M.[MANAGER_ID], ";
            strSQL += "     M.[MANAGER_NAME], ";
            strSQL += "     M.[USER_ID], ";
            strSQL += "     M.[USER_NAME], ";
            strSQL += "     M.[IS_MANAGER], ";
            strSQL += "     M.[EMAIL], ";
            strSQL += "     M.[MOBILE], ";
            strSQL += "     M.[JOB_GRADE], ";
            strSQL += "     CASE (SELECT COUNT(*) FROM [NUP].[dbo].[FSe7en_Org_MemberInfo] WHERE [AccountID]=M.[USER_ID] AND [Terminated] = 1) ";
            strSQL += "         WHEN 0 THEN  M.[JOB_STATUS] ";
            strSQL += "         ELSE null ";
            strSQL += "     END ";
            strSQL += "     AS [JOB_STATUS], ";
            strSQL += "     S.[IsMainJob] AS [IS_MAIN_JOB], ";
            strSQL += "     M.[USER_TITLE], ";
            strSQL += "     M.[USER_FLOW], ";
            strSQL += "     M.[DEPT_FLOW] ";
            strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] AS M ";
            strSQL += "INNER JOIN [NUP].[dbo].[FSe7en_Org_MemberStruct] AS S ON (S.[AccountID]=M.[USER_ID] AND S.[DeptID]=M.[DEPT_ID]) ";
            strSQL += "WHERE [USER_ID]=@USER_ID ";
            strSQL += "ORDER BY [COMPANY_ID],[SORT_ORDER],[JOB_GRADE] DESC";

            var userModel = dbFun.DoQuery(strSQL, parameter).ToList<UserInfoModel>();                        

            #region - 角色 -

            var userRole = BPMPro.CommonRepository.GetRoles().Where(R=>R.USER_ID== model.USER_ID).Select(R=>R.ROLE_ID).ToList();

            #endregion

            var userInfo = new UserInfo()
            {
                USER_MODEL = userModel,
                USER_ROLE = userRole
            };

            return userInfo;
        }

        /// <summary>
        /// 使用者資料列表(查詢)
        /// </summary>
        public IList<UserModel> PostUsers(UserQueryModel query)
        {
            var parameter = new List<SqlParameter>();

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [COMPANY_ID], ";
            strSQL += "     [PARENT_DEPT_ID], ";
            strSQL += "     [DEPT_ID], ";
            strSQL += "     [GRADE_ID], ";
            strSQL += "     [TITLE_ID], ";
            strSQL += "     [PARENT_DEPT_NAME], ";
            strSQL += "     [DEPT_NAME], ";
            strSQL += "     [GRADE_NAME], ";
            strSQL += "     [TITLE_NAME], ";
            strSQL += "     [GRADE_NUM], ";
            strSQL += "     [SORT_ORDER], ";
            strSQL += "     [MANAGER_DEPT_ID], ";
            strSQL += "     [MANAGER_ID], ";
            strSQL += "     [MANAGER_NAME], ";
            strSQL += "     [USER_ID], ";
            strSQL += "     [USER_NAME], ";
            strSQL += "     [IS_MANAGER], ";
            strSQL += "     [EMAIL], ";
            strSQL += "     [MOBILE], ";
            strSQL += "     [JOB_GRADE], ";
            strSQL += "     CASE (SELECT COUNT(*) FROM [NUP].[dbo].[FSe7en_Org_MemberInfo] WHERE [AccountID]=[USER_ID] AND [Terminated] = 1) ";
            strSQL += "         WHEN 0 THEN [JOB_STATUS] ";
            strSQL += "         ELSE null ";
            strSQL += "     END ";
            strSQL += "     AS [JOB_STATUS], ";
            strSQL += "     [USER_TITLE], ";
            strSQL += "     [USER_FLOW], ";
            strSQL += "     [DEPT_FLOW] ";
            strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
            strSQL += "WHERE 1=1 ";

            if (!String.IsNullOrEmpty(query.COMPANY_ID))
            {
                strSQL += "AND [COMPANY_ID]=@COMPANY_ID ";
                parameter.Add(new SqlParameter("@COMPANY_ID", SqlDbType.NVarChar) { Size = 20, Value = query.COMPANY_ID });
            }

            if (!String.IsNullOrEmpty(query.DEPT_ID))
            {
                strSQL += "AND [DEPT_ID]=@DEPT_ID ";
                parameter.Add(new SqlParameter("@DEPT_ID", SqlDbType.NVarChar) { Size = 20, Value = query.DEPT_ID });
            }

            if (!String.IsNullOrEmpty(query.DEPT_NAME))
            {
                strSQL += "AND [DEPT_NAME]=@DEPT_NAME ";
                parameter.Add(new SqlParameter("@DEPT_NAME", SqlDbType.VarChar) { Size = 20, Value = query.DEPT_NAME });
            }

            if (!String.IsNullOrEmpty(query.MANAGER_ID))
            {
                strSQL += "AND [MANAGER_ID]=@MANAGER_ID ";
                parameter.Add(new SqlParameter("@MANAGER_ID", SqlDbType.NVarChar) { Size = 20, Value = query.MANAGER_ID });
            }

            if (!String.IsNullOrEmpty(query.MANAGER_NAME))
            {
                strSQL += "AND [MANAGER_NAME] LIKE '%' + @MANAGER_NAME + '%' ";
                parameter.Add(new SqlParameter("@MANAGER_NAME", SqlDbType.NVarChar) { Size = 20, Value = query.MANAGER_NAME });
            }

            if (!String.IsNullOrEmpty(query.USER_ID))
            {
                strSQL += "AND [USER_ID]=@USER_ID ";
                parameter.Add(new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 20, Value = query.USER_ID });
            }

            if (!String.IsNullOrEmpty(query.USER_NAME))
            {
                strSQL += "AND [USER_NAME] LIKE '%' + @USER_NAME + '%' ";
                parameter.Add(new SqlParameter("@USER_NAME", SqlDbType.NVarChar) { Size = 20, Value = query.USER_NAME });
            }

            if (query.IS_MANAGER != null)
            {
                strSQL += "AND [IS_MANAGER]=@IS_MANAGER  ";
                parameter.Add(new SqlParameter("@IS_MANAGER", SqlDbType.Bit) { Value = query.IS_MANAGER });
            }

            if (!String.IsNullOrEmpty(query.EMAIL))
            {
                strSQL += "AND [EMAIL]=@EMAIL ";
                parameter.Add(new SqlParameter("@USEEMAILR_ID", SqlDbType.NVarChar) { Size = 50, Value = query.EMAIL });
            }

            if (!String.IsNullOrEmpty(query.MOBILE))
            {
                strSQL += "AND [MOBILE]=@MOBILE ";
                parameter.Add(new SqlParameter("@MOBILE", SqlDbType.NVarChar) { Size = 20, Value = query.MOBILE });
            }

            if (query.JOB_GRADE != null)
            {
                strSQL += "AND [JOB_GRADE]=@JOB_GRADE ";
                parameter.Add(new SqlParameter("@JOB_GRADE", SqlDbType.Int) { Value = query.JOB_GRADE });
            }

            int JobGrade_Start, JobGrade_End;

            if (int.TryParse(query.JOB_GRADE_START.ToString(), out JobGrade_Start) && !int.TryParse(query.JOB_GRADE_END.ToString(), out JobGrade_End))
            {
                strSQL += "AND [JOB_GRADE] >= @JOB_GRADE_START ";
                parameter.Add(new SqlParameter("@JOB_GRADE_START", SqlDbType.Int) { Value = JobGrade_Start });
            }

            if (!int.TryParse(query.JOB_GRADE_START.ToString(), out JobGrade_Start) && int.TryParse(query.JOB_GRADE_END.ToString(), out JobGrade_End))
            {
                strSQL += "AND [JOB_GRADE] <= @JOB_GRADE_END ";
                parameter.Add(new SqlParameter("@JOB_GRADE_END", SqlDbType.Int) { Value = JobGrade_End });
            }

            if (int.TryParse(query.JOB_GRADE_START.ToString(), out JobGrade_Start) && int.TryParse(query.JOB_GRADE_END.ToString(), out JobGrade_End))
            {
                strSQL += "AND [JOB_GRADE] BETWEEN @JOB_GRADE_START AND @JOB_GRADE_END ";
                parameter.Add(new SqlParameter("@JOB_GRADE_START", SqlDbType.Int) { Value = JobGrade_Start });
                parameter.Add(new SqlParameter("@JOB_GRADE_END", SqlDbType.Int) { Value = JobGrade_End });
            }

            if (!String.IsNullOrEmpty(query.USER_FLOW))
            {
                strSQL += "AND [USER_FLOW] LIKE '%' + @USER_FLOW + '%' ";
                parameter.Add(new SqlParameter("@USER_FLOW", SqlDbType.NVarChar) { Size = 200, Value = query.USER_FLOW });
            }

            if (!String.IsNullOrEmpty(query.DEPT_FLOW))
            {
                strSQL += "AND [DEPT_FLOW] LIKE '%' + @DEPT_FLOW + '%' ";
                parameter.Add(new SqlParameter("@DEPT_FLOW", SqlDbType.NVarChar) { Size = 200, Value = query.DEPT_FLOW });
            }

            strSQL += "ORDER BY [COMPANY_ID],[SORT_ORDER],[JOB_GRADE] DESC ";

            var usersModel = dbFun.DoQuery(strSQL, parameter).ToList<UserModel>();                        

            return usersModel;
        }

        /// <summary>
        /// 使用者員工結構資料(檢視)
        /// </summary>
        public IList<UsersStructure> GetUsersStructure()
        {
            #region - 查詢 -

            var parameterA = new List<SqlParameter>();

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     M.[SEQ_ID], ";
            strSQL += "     REPLACE(M.[COMPANY_ID],'RootCompany','GTV') AS [COMPANY_ID], ";
            strSQL += "     M.[PARENT_DEPT_ID], ";
            strSQL += "     D.[DeptID] AS [DEPT_ID], ";
            strSQL += "     D.[OfficeID] AS [OFFICE_ID], ";
            strSQL += "     D.[GroupID] AS [GROUP_ID], ";
            strSQL += "     M.[GRADE_ID], ";
            strSQL += "     M.[TITLE_ID], ";
            strSQL += "     M.[PARENT_DEPT_NAME],";
            strSQL += "     D.[DeptName] AS [DEPT_NAME], ";
            strSQL += "     D.[OfficeName] AS [OFFICE_NAME], ";
            strSQL += "     D.[GroupName] AS [GROUP_NAME], ";
            strSQL += "     M.[GRADE_NAME], ";
            strSQL += "     M.[TITLE_NAME], ";
            strSQL += "     M.[GRADE_NUM], ";
            strSQL += "     M.[SORT_ORDER], ";
            strSQL += "     M.[MANAGER_DEPT_ID], ";
            strSQL += "     M.[MANAGER_ID], ";
            strSQL += "     M.[MANAGER_NAME], ";
            strSQL += "     M.[USER_ID], ";
            strSQL += "     M.[USER_NAME], ";
            strSQL += "     M.[IS_MANAGER], ";
            strSQL += "     M.[EMAIL],";
            strSQL += "     M.[MOBILE], ";
            strSQL += "     M.[JOB_GRADE], ";
            strSQL += "     CASE (SELECT COUNT(*) FROM [NUP].[dbo].[FSe7en_Org_MemberInfo] WHERE [AccountID]=M.[USER_ID] AND [Terminated] = 1) ";
            strSQL += "         WHEN 0 THEN M.[JOB_STATUS] ";
            strSQL += "         ELSE null ";
            strSQL += "     END ";
            strSQL += "     AS [JOB_STATUS], ";
            strSQL += "     S.[IsMainJob] AS [IS_MAIN_JOB], ";
            strSQL += "     M.[USER_TITLE],";
            strSQL += "     M.[USER_FLOW], ";
            strSQL += "     M.[USER_FLOW_LEVEL], ";
            strSQL += "     M.[DEPT_FLOW], ";
            strSQL += "     M.[DEPT_FLOW_LEVEL], ";
            strSQL += "     M.[CREATE_DATE], ";
            strSQL += "     M.[MODIFY_DATE] ";
            strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] AS M ";
            strSQL += "INNER JOIN [NUP].[dbo].[GTV_Org_DeptStruct] AS D ON M.[DEPT_ID]=D.[TheEndDeptID] ";
            strSQL += "INNER JOIN [NUP].[dbo].[FSe7en_Org_MemberStruct] AS S ON (S.[AccountID]=M.[USER_ID] AND S.[DeptID]=M.[DEPT_ID]) AND M.[TITLE_ID]<>'AD' ";
            strSQL += "ORDER BY M.[SEQ_ID],[SORT_ORDER] ASC ";

            var usersStructure = dbFun.DoQuery(strSQL, parameterA).ToList<UsersStructure>();

            return usersStructure;

            #endregion
        }

        /// <summary>
        /// 驗證使用者是否有效
        /// </summary>
        public bool IsVerifyUser(LogonModel model)
        {
            var vResult = false;

            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 64, Value = model.USER_ID },
                    new SqlParameter("@USER_PW", SqlDbType.NVarChar) { Size = 128, Value = GlobalParameters.HashToMD5(model.USER_PW) }
                };

                strSQL = "";
                strSQL += "SELECT [AccountID] ";
                strSQL += "FROM [NUP].[dbo].[FSe7en_Org_MemberInfo] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "            AND [AccountID]=@USER_ID ";
                strSQL += "            AND [Password]=@USER_PW ";

                var dt = dbFun.DoQuery(strSQL, parameter);

                if (dt.Rows.Count > 0)
                {
                    vResult = true;
                }
                else
                {
                    throw new Exception("帳號密碼錯誤或查無此帳號");
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("驗證使用者是否有效失敗，原因：" + ex.Message);
                throw new Exception(ex.Message);
            }

            return vResult;
        }

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        #endregion
    }
}