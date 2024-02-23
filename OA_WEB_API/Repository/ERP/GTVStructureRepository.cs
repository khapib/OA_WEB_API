using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Repository.BPMPro;

namespace OA_WEB_API.Repository.ERP
{
    public class GTVStructureRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        UserRepository userRepository = new UserRepository();

        #endregion

        #endregion

        #region  - 方法 -

        /// <summary>
        /// 提供外部系統(員工結構)(查詢)
        /// </summary>
        public IList<UsersStructure> PostGTVStaffSingle()
        {
            #region - 查詢 -

            #region - 員工結構資料 -

            var usersStructure = userRepository.GetUsersStructure();

            #endregion

            #region  - GTV人員資料表 -
            //篩選掉四方四隅及只顯示非兼職的正職部門給ERP
            var gTVStaffModel = usersStructure.Where(S => S.COMPANY_ID != "GPI").Select(S => S).ToList();
            gTVStaffModel.ForEach(S =>
            {
                if (!String.IsNullOrEmpty(S.USER_FLOW) || !String.IsNullOrWhiteSpace(S.USER_FLOW)) S.USER_FLOW = S.USER_FLOW.Replace(">", "-");
                if (!String.IsNullOrEmpty(S.DEPT_FLOW) || !String.IsNullOrWhiteSpace(S.DEPT_FLOW)) S.DEPT_FLOW = (S.DEPT_FLOW.Replace("八大電視 - ", "")).Replace(">", "-");
            });

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