using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.ERP;

namespace OA_WEB_API.Repository.ERP
{
    public class GTVStructureRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #endregion

        #region  - 方法 -

        /// <summary>
        /// 提供外部系統(員工結構)(查詢)
        /// </summary>
        public IList<GTVStaffModel> PostGTVStaffSingle()
        {
            #region - 查詢 -

            #region  - GTV人員資料表 -

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
            strSQL += "     M.[JOB_STATUS], ";
            strSQL += "     M.[USER_TITLE],";
            strSQL += "     REPLACE(M.[USER_FLOW],'>','-') AS [USER_FLOW], ";
            strSQL += "     M.[USER_FLOW_LEVEL], ";
            strSQL += "     REPLACE(REPLACE(M.[DEPT_FLOW],'>','-'),'八大電視 - ','') AS [DEPT_FLOW], ";
            strSQL += "     M.[DEPT_FLOW_LEVEL], ";
            strSQL += "     M.[CREATE_DATE], ";
            strSQL += "     M.[MODIFY_DATE] ";            
            strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] AS M ";
            strSQL += "INNER JOIN [NUP].[dbo].[GTV_Org_DeptStruct] AS D ON M.[DEPT_ID]=D.[TheEndDeptID] ";
            strSQL += "INNER JOIN [NUP].[dbo].[FSe7en_Org_MemberStruct] AS S on (S.[AccountID]=M.[USER_ID] AND S.[DeptID]=M.[DEPT_ID]) AND (M.[COMPANY_ID]<>'GPI' AND M.[TITLE_ID]<>'AD') AND S.[IsMainJob]=1 ";
            strSQL += "ORDER BY M.[SEQ_ID],[SORT_ORDER] ASC ";

            var gTVStaffModel = dbFun.DoQuery(strSQL, parameterA).ToList<GTVStaffModel>();

            #endregion

            return gTVStaffModel;

            #endregion
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