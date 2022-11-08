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

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        CommonRepository commonRepository = new CommonRepository();
        FormRepository formRepository = new FormRepository();
        UserRepository userRepository = new UserRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        NoticeMode noticeMode = new NoticeMode();

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

        #region - 表單知會 -

        /// <summary>
        /// 表單知會
        /// </summary>
        public bool PutFormNotify(FormNotifyViewModel model)
        {
            bool vResult = false;
            try
            {
                ReceiverID = "";

                foreach (var requisitionID in model.REQUISITION_ID)
                {
                    #region - 被知會特定人員 -

                    if (String.IsNullOrWhiteSpace(ReceiverID))
                    {
                        #region - 被知會特定角色 -

                        if (model.ROLE_ID != null)
                        {
                            foreach (var role in model.ROLE_ID)
                            {
                                var RolesUserID = commonRepository.GetRoles()
                                                                .Where(R => R.ROLE_ID.Contains(role))
                                                                .Select(R => R).ToList();
                                RolesUserID.ForEach(roleuser =>
                                {
                                    model.NOTIFY_BY.Add(roleuser.USER_ID);
                                });
                            }
                        }

                        #endregion

                        #region - 排除重複人員 -

                        model.NOTIFY_BY = model.NOTIFY_BY.GroupBy(N => N)
                                                        .Select(g => g.First()).ToList();

                        #endregion

                        foreach (var notify in model.NOTIFY_BY)
                        {
                            var UserIDmodel = new LogonModel()
                            {
                                USER_ID = notify
                            };

                            foreach (var userInfo in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                            {
                                ReceiverID += userInfo.USER_ID + "@" + userInfo.DEPT_ID + ";";
                            }
                        }
                        ReceiverID = ReceiverID.Substring(0, ReceiverID.Length - 1);
                    }

                    #endregion

                    var formQueryModel = new FormQueryModel()
                    {
                        REQUISITION_ID = requisitionID
                    };

                    if (CommonRepository.PostDataHaveForm(formQueryModel))
                    {
                        //表單資訊
                        var formData = formRepository.PostFormData(formQueryModel);

                        formQueryModel = new FormQueryModel()
                        {
                            IS_ENABLE_SMS = false,
                            RECEIVER_ID = ReceiverID,
                            REQUISITION_ID = requisitionID,
                            SERIAL_ID = formData.SERIAL_ID
                        };
                        //發特定人員通知及Email
                        notifyRepository.ByNotice(formQueryModel);

                        vResult = true;
                    }
                    else
                    {
                        //無此表單資料
                        vResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單知會寫入失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
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

        /// <summary>
        /// 特定人員
        /// </summary>
        private string ReceiverID;

        #endregion
    }
}