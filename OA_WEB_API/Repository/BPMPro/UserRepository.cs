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

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

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
            strSQL += "SELECT [COMPANY_ID],[PARENT_DEPT_ID],[DEPT_ID],[GRADE_ID],[TITLE_ID],[PARENT_DEPT_NAME],[DEPT_NAME],[GRADE_NAME],[TITLE_NAME],[GRADE_NUM],[SORT_ORDER],[MANAGER_ID],[MANAGER_NAME],[USER_ID],[USER_NAME],[IS_MANAGER],[EMAIL],[MOBILE],[JOB_GRADE],[JOB_STATUS],[USER_TITLE],[USER_FLOW],[DEPT_FLOW] ";
            strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
            strSQL += "WHERE [USER_ID]=@USER_ID ";
            strSQL += "ORDER BY [COMPANY_ID],[SORT_ORDER],[JOB_GRADE] DESC ";

            var userModel = dbFun.DoQuery(strSQL, parameter).ToList<UserModel>();

            userModel.ForEach(U =>
            {
                if (Sysusers().Any(SU => SU.USER_ID.Contains(U.USER_ID))) U.JOB_STATUS = 0;
            });

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
            strSQL += "SELECT [COMPANY_ID],[PARENT_DEPT_ID],[DEPT_ID],[GRADE_ID],[TITLE_ID],[PARENT_DEPT_NAME],[DEPT_NAME],[GRADE_NAME],[TITLE_NAME],[GRADE_NUM],[SORT_ORDER],[MANAGER_ID],[MANAGER_NAME],[USER_ID],[USER_NAME],[IS_MANAGER],[EMAIL],[MOBILE],[JOB_GRADE],[JOB_STATUS],[USER_TITLE],[USER_FLOW],[DEPT_FLOW] ";
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

            #region - 人員在職確認 -

            //以系統[NUP].[dbo].[FSe7en_Org_MemberInfo]與[NUP].[dbo].[GTV_Org_Relation_Member]確認，
            //如果[NUP].[dbo].[GTV_Org_Relation_Member]有資料[NUP].[dbo].[FSe7en_Org_MemberInfo]沒有及
            //[NUP].[dbo].[FSe7en_Org_MemberInfo].[Terminated]狀態=1
            //則JOB_STATUS就會改為0(離職)

            usersModel.ForEach(U =>
            {
                if (Sysusers().Any(SU => SU.USER_ID.Contains(U.USER_ID))) U.JOB_STATUS = 0;
            });

            #endregion

            return usersModel;
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


        public IList<UserModel> Sysusers()
        {
            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     null AS [COMPANY_ID], ";
            strSQL += "     null AS [PARENT_DEPT_ID], ";
            strSQL += "     null AS [DEPT_ID], ";
            strSQL += "     null AS [GRADE_ID], ";
            strSQL += "     null AS [TITLE_ID], ";
            strSQL += "     null AS [PARENT_DEPT_NAME], ";
            strSQL += "     null AS [DEPT_NAME], ";
            strSQL += "     null AS [GRADE_NAME], ";
            strSQL += "     null AS [TITLE_NAME], ";
            strSQL += "     null AS [GRADE_NUM], ";
            strSQL += "     null AS [SORT_ORDER], ";
            strSQL += "     null AS [MANAGER_ID], ";
            strSQL += "     null AS [MANAGER_NAME], ";
            strSQL += "     [AccountID] AS [USER_ID], ";
            strSQL += "     null AS [USER_NAME], ";
            strSQL += "     null AS [IS_MANAGER], ";
            strSQL += "     null AS [EMAIL], ";
            strSQL += "     null AS [MOBILE], ";
            strSQL += "     null AS [JOB_GRADE], ";
            strSQL += "     null AS [JOB_STATUS], ";
            strSQL += "     null AS [USER_TITLE], ";
            strSQL += "     null AS [USER_FLOW], ";
            strSQL += "     null AS [DEPT_FLOW] ";
            strSQL += "FROM [NUP].[dbo].[FSe7en_Org_MemberInfo] ";
            strSQL += "WHERE [Terminated] = 1";
            return dbFun.DoQuery(strSQL).ToList<UserModel>();
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