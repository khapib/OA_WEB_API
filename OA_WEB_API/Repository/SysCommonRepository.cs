using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
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
        UserRepository userRepository = new UserRepository();

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

        #region - 公司列表 -

        /// <summary>
        /// 公司列表
        /// </summary>
        public IList<CompanyViewModel> GetCompanyList()
        {
            try
            {
                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [AutoCounter] AS [AUTO_COUNTER], ";
                strSQL += "     [CompanyID] AS [COMPANY_ID], ";
                strSQL += "     [CompanyName] AS [COMPANY_NAME] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Company] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "ORDER BY [AutoCounter] ";
                var getCompanyList = dbFun.DoQuery(strSQL).ToList<CompanyViewModel>();

                return getCompanyList;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("公司列表，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 部門列表 -

        /// <summary>
        /// 八大電視_部門列表
        /// </summary>
        public IList<DeptTree> GetGTVDeptTree()
        {
            try
            {
                //抓取八大電視的所有部門
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

        /// <summary>
        /// 四方四隅_部門列表
        /// </summary>
        public IList<DeptTree> GetGPIDeptTree()
        {
            try
            {
                //抓取四方四隅的所有部門
                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [PARENT_DEPT_ID], ";
                strSQL += "     [DEPT_ID], ";
                strSQL += "     [PARENT_DEPT_NAME], ";
                strSQL += "     [DEPT_NAME] ";
                strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "         AND [COMPANY_ID]='GPI' ";
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

        #region - 主要部門及使用者資訊 -

        /// <summary>
        /// 主要部門及使用者資訊
        /// </summary>        
        public UserInfoMainDeptViewModel PostUserInfoMainDept(UserInfoMainDeptModel model)
        {
            try
            {
                var DeptMainID = String.Empty;
                var ParentDeptID = String.Empty;
                var MainDept = new List<DeptTree>();

                switch (model.COMPANY_ID)
                {
                    case "RootCompany":
                        ParentDeptID = "H01";
                        MainDept = GetGTVDeptTree().Where(D => D.PARENT_DEPT_ID == ParentDeptID).Select(D => D).ToList();
                        break;
                    case "GPI":
                        ParentDeptID = "H02";
                        MainDept = GetGPIDeptTree().ToList();
                        break;
                    default: break;
                }

                var userInfoMainDeptViewModel = new UserInfoMainDeptViewModel();

                if(!String.IsNullOrEmpty(model.USER_ID) || !String.IsNullOrWhiteSpace(model.USER_ID))
                {
                    var logonModel = new LogonModel()
                    {
                        USER_ID = model.USER_ID
                    };

                    userInfoMainDeptViewModel.USER_MODEL = jsonFunction.JsonToObject<UserInfoConfig>(jsonFunction.ObjectToJSON(userRepository.PostUserSingle(logonModel).USER_MODEL
                                                                                                                                .Where(U => U.DEPT_ID == model.DEPT_ID)
                                                                                                                                .Select(U => U)
                                                                                                                                .FirstOrDefault()));
                    if (userInfoMainDeptViewModel.USER_MODEL != null && userInfoMainDeptViewModel.USER_MODEL.JOB_STATUS != 0)
                    {
                        var parameter = new List<SqlParameter>()
                        {
                            new SqlParameter("@ACCOUNT_ID", SqlDbType.NVarChar) { Size = 10, Value = userInfoMainDeptViewModel.USER_MODEL.USER_ID },
                            new SqlParameter("@DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = userInfoMainDeptViewModel.USER_MODEL.DEPT_ID },
                        };

                        strSQL = "";
                        strSQL += "Select ";
                        strSQL += "      [IsMainJob] ";
                        strSQL += "FROM [NUP].[dbo].[FSe7en_Org_MemberStruct] ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "         AND [AccountID]=@ACCOUNT_ID ";
                        strSQL += "         AND [DeptID]=@DEPT_ID ";
                        var dt = dbFun.DoQuery(strSQL, parameter);

                        if (dt.Rows.Count > 0)
                        {
                            switch (dt.Rows[0][0].ToString())
                            {
                                case "0":
                                    userInfoMainDeptViewModel.USER_MODEL.IS_MAIN_JOB = false;
                                    break;
                                case "1":
                                    userInfoMainDeptViewModel.USER_MODEL.IS_MAIN_JOB = true;
                                    break;
                                default:
                                    userInfoMainDeptViewModel.USER_MODEL.IS_MAIN_JOB = null;
                                    break;
                            }

                            if (userInfoMainDeptViewModel.USER_MODEL.DEPT_FLOW == null)
                            {
                                switch (model.COMPANY_ID)
                                {
                                    case "RootCompany":
                                        userInfoMainDeptViewModel.MAIN_DEPT = MainDept.Where(D => D.DEPT_ID == "D01").Select(D => D).FirstOrDefault();
                                        break;
                                    case "GPI":
                                        userInfoMainDeptViewModel.MAIN_DEPT = MainDept.Where(D => D.DEPT_ID == "H02").Select(D => D).FirstOrDefault();
                                        break;
                                    default: break;
                                }
                            }
                            else
                            {
                                userInfoMainDeptViewModel.MAIN_DEPT = MainDept.Where(D => userInfoMainDeptViewModel.USER_MODEL.DEPT_FLOW.Contains(D.DEPT_NAME)).Select(D => D).FirstOrDefault();
                            }

                        }
                    }
                }
                else
                {
                    userInfoMainDeptViewModel.USER_MODEL = null;

                    if(String.IsNullOrEmpty(model.DEPT_ID) || String.IsNullOrWhiteSpace(model.DEPT_ID))
                    {
                        switch (model.COMPANY_ID)
                        {
                            case "RootCompany":
                                userInfoMainDeptViewModel.MAIN_DEPT = MainDept.Where(D => D.DEPT_ID == "D01").Select(D => D).FirstOrDefault();
                                break;
                            case "GPI":
                                userInfoMainDeptViewModel.MAIN_DEPT = MainDept.Where(D => D.DEPT_ID == "H02").Select(D => D).FirstOrDefault();
                                break;
                            default: break;
                        }
                    }
                    else
                    {    
                        switch (model.COMPANY_ID)
                        {
                            case "RootCompany":
                                ParentDeptID = "H01";
                                MainDept = GetGTVDeptTree().Where(D => D.DEPT_ID == model.DEPT_ID).Select(D => D).ToList();
                                while(MainDept.Select(D=>D.PARENT_DEPT_ID).FirstOrDefault()!= ParentDeptID)
                                {
                                    MainDept= GetGTVDeptTree().Where(D=>D.DEPT_ID== MainDept.Select(P => P.PARENT_DEPT_ID).FirstOrDefault()).ToList();
                                }
                                break;
                            case "GPI":
                                ParentDeptID = "H02";
                                MainDept = GetGPIDeptTree().Where(D => D.DEPT_ID == model.DEPT_ID).Select(D => D).ToList();
                                while (MainDept.Select(D => D.PARENT_DEPT_ID).FirstOrDefault() != ParentDeptID)
                                {
                                    MainDept = GetGPIDeptTree().Where(D => D.DEPT_ID == MainDept.Select(P => P.PARENT_DEPT_ID).FirstOrDefault()).ToList();
                                }
                                break;
                            default: break;
                        }
                        userInfoMainDeptViewModel.MAIN_DEPT = MainDept.FirstOrDefault();
                    }  
                }

                return userInfoMainDeptViewModel;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("主要部門及使用者資訊 失敗，原因：" + ex.Message);
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