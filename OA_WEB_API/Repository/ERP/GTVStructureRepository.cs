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

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

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
            strSQL += "     [SEQ_ID], ";
            strSQL += "     REPLACE([COMPANY_ID],'RootCompany','GTV') AS [COMPANY_ID], ";
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
            strSQL += "     [JOB_STATUS], ";
            strSQL += "     [USER_TITLE], ";
            strSQL += "     REPLACE([USER_FLOW],'>','-') AS [USER_FLOW], ";
            strSQL += "     [USER_FLOW_LEVEL], ";
            strSQL += "     REPLACE(REPLACE([DEPT_FLOW],'>','-'),'八大電視 - ','') AS [DEPT_FLOW], ";
            strSQL += "     [DEPT_FLOW_LEVEL], ";
            strSQL += "     [CREATE_DATE], ";
            strSQL += "     [MODIFY_DATE] ";
            strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "          AND (";
            strSQL += "                  [COMPANY_ID]<>'GPI' AND [TITLE_ID]<>'AD' ";
            strSQL += "              )";
            strSQL += "order by [SEQ_ID],[SORT_ORDER] ASC ";

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