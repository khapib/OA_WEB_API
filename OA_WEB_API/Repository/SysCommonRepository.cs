using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.BPMPro;


namespace OA_WEB_API.Repository
{
    /// <summary>
    /// 系統共通功能
    /// </summary>
    public class SysCommonRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        CommonRepository commonRepository = new CommonRepository();
        FormRepository formRepository = new FormRepository();
        #endregion

        #region - 方法 -

        #region - 測試 -

        #region - 會簽管理系統 - BPM刪除及結束表單 -

        /// <summary>
        /// 會簽管理系統 - BPM刪除及結束表單
        /// </summary>
        public string PostFormRemove(FormRemoveQueryModel query)
        {
            var ResponseMsg = "";
            try
            {
                var formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostDataHaveForm(formQueryModel))
                {
                    //表單資料
                    var formData = formRepository.PostFormData(formQueryModel);

                    #region - 執行表單 駁回結束 -                   

                    var parameter = new List<SqlParameter>()
                    {
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID},
                        new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = formData.DIAGRAM_ID }
                    };

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FSe7en_Sys_Requisition] ";
                    strSQL += "SET [Status]=5 ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "     AND [DiagramID]=@DIAGRAM_ID ";
                    strSQL += "     AND [RequisitionID]=@REQUISITION_ID ";

                    //調整為撤單
                    dbFun.DoTran(strSQL, parameter);

                    #endregion
                }

                #region - 刪除表單 -

                //刪除表單
                commonRepository.PostFormDelete(query);

                #endregion

                ResponseMsg = "已「結束及刪除」表單 REQUISITION_ID：" + query.REQUISITION_ID + "。";

                return ResponseMsg;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("會簽管理系統 - BPM結束及刪除表單 寫入失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 部門列表 -

        /// <summary>
        /// 部門列表
        /// </summary>
        public IList<DeptTree> GetGTVDeptTree()
        {
            try
            {
                //抓取八大的所有部門
                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [PARENT_DEPT_ID], ";
                strSQL += "     [DEPT_ID], ";
                strSQL += "     [PARENT_DEPT_NAME], ";
                strSQL += "     [DEPT_NAME] ";
                strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [COMPANY_ID]='RootCompany' ";
                strSQL += "         AND [TITLE_ID]<>'AD' ";
                strSQL += "ORDER BY [SEQ_ID] ";
                var deptTree = dbFun.DoQuery(strSQL).ToList<DeptTree>();
                deptTree = deptTree.GroupBy(D => D.DEPT_ID)
                                    .Select(g => g.First()).ToList();
                return deptTree;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("部門列表呈現失敗，原因：" + ex.Message);
                throw;
            }
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