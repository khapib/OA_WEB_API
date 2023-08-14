using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;

using Dapper;

/// <summary>
/// 會簽管理系統 - 表單及簽核流程
/// </summary>
namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 表單共用
    /// </summary>
    public class FormRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #endregion

        #region - 方法 -

        /// <summary>
        /// 表單主旨
        /// </summary>
        public void PutFormHeader(FormHeader model)
        {
            var parameter = new List<SqlParameter>()
            {
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 10, Value = "Subject" },
                new SqlParameter("@ITEM_VALUE", SqlDbType.NVarChar) { Size = 60, Value = model.ITEM_VALUE }
            };

            #region 先刪除舊資料

            strSQL = "";
            strSQL += "DELETE ";
            strSQL += "FROM [BPMPro].[dbo].[FSe7en_Tep_FormHeader] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            dbFun.DoTran(strSQL, parameter);

            #endregion

            #region 再新增資料

            strSQL = "";
            strSQL += "INSERT INTO [BPMPro].[dbo].[FSe7en_Tep_FormHeader]([RequisitionID],[ItemName],[Value]) ";
            strSQL += "VALUES(@REQUISITION_ID,@ITEM_NAME,@ITEM_VALUE) ";

            dbFun.DoTran(strSQL, parameter);

            #endregion
        }

        /// <summary>
        /// 儲存草稿列表
        /// </summary>
        public void PutFormDraftList(FormDraftList model, bool IsAdd)
        {
            var parameter = new List<SqlParameter>()
            {
                new SqlParameter("@UNIQUE_ID", SqlDbType.Int) { Value = 0 },
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                new SqlParameter("@IDENTIFY", SqlDbType.NVarChar) { Size = 50, Value = model.IDENTIFY },
                new SqlParameter("@DISPLAY_NAME", SqlDbType.NVarChar) { Size = 60, Value = String.Empty },
                new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 50, Value = model.FILLER_ID },
                new SqlParameter("@SAVED_TIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                new SqlParameter("@REFILL", SqlDbType.Int) { Value = 0 },
                new SqlParameter("@ORIGINAL_REQUISITION_ID", SqlDbType.NVarChar) { Size =64, Value = String.Empty }
            };

            #region 先刪除舊資料

            strSQL = "";
            strSQL += "DELETE ";
            strSQL += "FROM [FSe7en_Tep_DraftList] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "            AND [RequisitionID]=@REQUISITION_ID ";
            strSQL += "            AND [Identify]=@IDENTIFY ";
            strSQL += "            AND [FillerID]=@FILLER_ID ";

            dbFun.DoQuery(strSQL, parameter);

            #endregion

            #region 再新增資料

            if (IsAdd)
            {
                strSQL = "";
                strSQL += "SELECT ISNULL(MAX([UniqueID]), 0) +1 AS [UniqueMaxID] ";
                strSQL += "FROM [BPMPro].[dbo].[FSe7en_Tep_DraftList] ";

                parameter[0].Value = int.Parse(dbFun.DoQuery(strSQL).Rows[0]["UniqueMaxID"].ToString());

                strSQL = "";
                strSQL += "INSERT INTO [BPMPro].[dbo].[FSe7en_Tep_DraftList]([UniqueID],[RequisitionID],[Identify],[DisplayName],[FillerID],[SavedTime],[refill],[OriginalRequisitionID]) ";
                strSQL += "VALUES(@UNIQUE_ID,@REQUISITION_ID,@IDENTIFY,@DISPLAY_NAME,@FILLER_ID,@SAVED_TIME,@REFILL,@ORIGINAL_REQUISITION_ID) ";

                dbFun.DoTran(strSQL, parameter);
            }

            #endregion
        }

        /// <summary>
        /// 送出表單
        /// </summary>
        public string PutFormAutoStart(FormAutoStart model)
        {
            try
            {
                //HttpWebRequest request = HttpWebRequest.Create("http://192.168.1.84:81/BPMPro/AutoStart.aspx") as HttpWebRequest;        //正試機
                //HttpWebRequest request = HttpWebRequest.Create("http://192.168.1.217:81/BPMPro/AutoStart.aspx") as HttpWebRequest;   //測試機
                HttpWebRequest request = HttpWebRequest.Create("http://192.168.1.219:82/BPMPro/AutoStart.aspx") as HttpWebRequest;   //共用測試機
                //HttpWebRequest request = HttpWebRequest.Create("http://song-vm-pc:82/BPMPro/AutoStart.aspx") as HttpWebRequest;     //個人測試機

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0";
                request.Timeout = 3000;

                NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
                postParams.Add("r", model.REQUISITION_ID);
                postParams.Add("d", model.DIAGRAM_ID);
                postParams.Add("a", model.APPLICANT_ID);
                postParams.Add("t", model.APPLICANT_DEPT);

                byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }

                string responseStr = "";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseStr = reader.ReadToEnd();
                }

                return responseStr;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error(" 啟動流程：申請人送出失敗，原因：" + ex.Message);

                return ex.Message;
            }
        }

        /// <summary>
        /// 表單基本資料(查詢)
        /// </summary>
        public FormData PostFormData(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>();

                strSQL = "";
                strSQL += "SELECT [REQUISITION_ID],[SERIAL_ID],[IDENTIFY],[DIAGRAM_ID],[DIAGRAM_NAME],[COMPANY_ID],[APPLICANT_DEPT_ID],[APPLICANT_DEPT_NAME],[APPLICANT_ID],[APPLICANT_NAME],[APPLICANT_DATETIME],[FORM_STATUS],[FORM_STATUS_NAME],[FORM_SUBJECT],NULL AS [PRIORITY_TEXT] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_View_FormData] ";
                strSQL += "WHERE 1=1 ";

                if (!String.IsNullOrEmpty(query.REQUISITION_ID))
                {
                    strSQL += "AND [REQUISITION_ID]=@REQUISITION_ID ";
                    parameter.Add(new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID });
                }

                if (!String.IsNullOrEmpty(query.SERIAL_ID))
                {
                    strSQL += "AND [SERIAL_ID]=@SERIAL_ID ";
                    parameter.Add(new SqlParameter("@SERIAL_ID", SqlDbType.NVarChar) { Size = 50, Value = query.SERIAL_ID });
                }

                var formData = dbFun.DoQuery(strSQL, parameter).ToList<FormData>().SingleOrDefault();
                formData.PRIORITY_TEXT = PostFormPriority(formData);

                return formData;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單基本資料(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("表單基本資料(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單重要性(查詢)
        /// </summary>
        private string PostFormPriority(FormData model)
        {
            try
            {
                #region - 查詢 -

                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "  CASE ";
                strSQL += "      WHEN [Priority]='1' THEN '低' ";
                strSQL += "      WHEN [Priority]='2' THEN '中' ";
                strSQL += "      WHEN [Priority]='3' THEN '高' ";
                strSQL += "  END [PRIORITY_TEXT] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_"+ model.IDENTIFY +"_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                #endregion

                return dbFun.DoQuery(strSQL, parameter).Rows[0]["PRIORITY_TEXT"].ToString();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單重要性(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("表單重要性(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 信件主旨(狀態依照情境後補)
        /// </summary>
        public string GetFormSubject(FormQueryModel query)
        {
            #region - 宣告 -

            var formRepository = new FormRepository();
            var formData = formRepository.PostFormData(query);

            #endregion

            //格式：(已逾期)【會簽單】蔡佳珍 申請【主旨】呈大陸劇『如意芳霏』第一期款50%請款，申請簽核。

            return String.Format("({0})【{1}】{2} 申請【主旨】{3}", "{0}", formData.DIAGRAM_NAME, formData.APPLICANT_NAME, formData.FORM_SUBJECT);
        }

        /// <summary>
        /// 訊息通知(狀態依照情境後補)
        /// </summary>
        public MessageModel GetFormSubjectToMessage(FormQueryModel query)
        {
            #region - 宣告 -

            var formRepository = new FormRepository();
            var formData = formRepository.PostFormData(query);

            #endregion

            var model = new MessageModel()
            {
                //【{0}】打包歸檔通知

                //申請人：{2}

                //SUBJECT = String.Format(formRepository.GetFormSubject(query), "處理完畢通知"),   //主旨
            };

            return model;
        }

        //允許代填單人(暫時不需要，前端可以自己抓到)

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        #endregion
    }

    /// <summary>
    /// 簽核流程
    /// </summary>
    public class FlowRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        FormRepository formRepository = new FormRepository();
        UserRepository userRepository = new UserRepository();

        #endregion

        #region - 方法 -

        /// <summary>
        /// 單位主管審核(查詢)
        /// </summary>
        public IList<UnitApproverModel> PostUnitApprover(FlowQueryModel query)
        {
            try
            {
                #region - 查詢 -

                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@COMPANY_ID", SqlDbType.NVarChar) { Size = 20, Value = query.COMPANY_ID },
                    new SqlParameter("@DEP_NAME", SqlDbType.NVarChar) { Size = 20, Value = query.DEPT_NAME },
                    new SqlParameter("@JOB_GRADE", SqlDbType.Int) { Value = query.JOB_GRADE }
                };

                strSQL = "";
                strSQL += "SELECT [USER_TITLE],[USER_ID] ";
                strSQL += "FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "WHERE 1=1  ";
                strSQL += "           AND [USER_ID]<>'administrator' ";
                strSQL += "           AND [COMPANY_ID]=@COMPANY_ID ";
                strSQL += "           AND [USER_TITLE] LIKE '%' + @DEP_NAME + '%' ";
                strSQL += "           AND [JOB_GRADE] <= @JOB_GRADE ";
                strSQL += "           AND [IS_MANAGER]='1' ";
                strSQL += "ORDER BY [COMPANY_ID],[SORT_ORDER],[JOB_GRADE] DESC ";

                #endregion

                return dbFun.DoQuery(strSQL, parameter).ToList<UnitApproverModel>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("單位主管審核(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("單位主管審核(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 上一級主管(查詢)
        /// </summary>
        public IList<SupervisorModel> PostSupervisor(FlowQueryModel query)
        {
            try
            {
                #region - 查詢 -

                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@COMPANY_ID", SqlDbType.NVarChar) { Size = 20, Value = query.COMPANY_ID },
                    new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 20, Value = query.USER_ID }
                };

                strSQL = "";
                strSQL += "WITH [GTV_USER_FLOW_CTE] ([PARENT_DEPT_ID],[DEPT_ID],[MANAGER_ID],[USER_ID],[PARENT_DEPT_NAME],[DEPT_NAME],[MANAGER_NAME],[USER_NAME],[SORT_ORDER],[JOB_GRADE]) AS ";
                strSQL += "( ";
                strSQL += "    SELECT [PARENT_DEPT_ID],[DEPT_ID],[MANAGER_ID],[USER_ID],[PARENT_DEPT_NAME],[DEPT_NAME],[MANAGER_NAME],[USER_NAME],[SORT_ORDER],[JOB_GRADE] ";
                strSQL += "    FROM [NUP].[dbo].[GTV_View_OrgRelationMember] ";
                strSQL += "    WHERE [COMPANY_ID]=@COMPANY_ID AND [USER_ID]=@USER_ID ";
                strSQL += "    UNION ALL ";
                strSQL += "    SELECT A.[PARENT_DEPT_ID],A.[DEPT_ID],A.[MANAGER_ID],A.[USER_ID],A.[PARENT_DEPT_NAME],A.[DEPT_NAME],A.[MANAGER_NAME],A.[USER_NAME],A.[SORT_ORDER],A.[JOB_GRADE] ";
                strSQL += "    FROM [NUP].[dbo].[GTV_View_OrgRelationMember] A ";
                strSQL += "             INNER JOIN [GTV_USER_FLOW_CTE] B ON A.[USER_ID]=B.[MANAGER_ID] ";
                strSQL += "    WHERE A.[COMPANY_ID]=@COMPANY_ID ";
                strSQL += ") ";
                strSQL += "SELECT [DEPT_ID],[USER_ID],[DEPT_NAME],[USER_NAME],[EMAIL],[MOBILE] ";
                strSQL += "FROM ( ";
                strSQL += "           SELECT C.[PARENT_DEPT_ID],C.[DEPT_ID],C.[MANAGER_ID],C.[USER_ID],C.[PARENT_DEPT_NAME],C.[DEPT_NAME],C.[MANAGER_NAME],C.[USER_NAME],M.[EMAIL],M.[MOBILE],C.[SORT_ORDER],C.[JOB_GRADE],ROW_NUMBER() OVER (ORDER BY C.[JOB_GRADE]) - 1 AS [SORT_ID] ";
                strSQL += "           FROM [GTV_USER_FLOW_CTE] C ";
                strSQL += "                    INNER JOIN [NUP].[dbo].[GTV_View_OrgRelationMember] M ON M.[USER_ID]=C.[USER_ID] ";
                strSQL += "         ) M ";
                strSQL += "WHERE M.[SORT_ID]='1' ";

                if (query.JOB_GRADE != null)
                {
                    strSQL += "AND M.[JOB_GRADE] <= @JOB_GRADE ";
                    parameter.Add(new SqlParameter("@JOB_GRADE", SqlDbType.Int) { Value = query.JOB_GRADE });
                }

                strSQL += "ORDER BY M.[SORT_ID] ";

                #endregion

                return dbFun.DoQuery(strSQL, parameter).ToList<SupervisorModel>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("上一級主管(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("上一級主管(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 判斷(待簽核人)有無設定(代理人)(兼職會有多筆)
        /// </summary>
        public IList<AgentModel> PostAgent(FlowQueryModel query)
        {
            try
            {
                #region - 查詢 -

                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 20, Value = query.USER_ID }
                };

                strSQL = "";
                strSQL += "SELECT A.[AccountID] AS [USER_ID],B.[AgentID] AS [AGENT_ID],B.[DeptID] AS [DEPT_ID],A.[TimeStart] AS [TIME_START],A.[TimeEnd] AS [TIME_END] ";
                strSQL += "FROM [BPMPro].[dbo].[FSe7en_Sys_AbsentInfo] A ";
                strSQL += "         INNER JOIN [BPMPro].[dbo].[FSe7en_Sys_AbsentAgent] B ON A.[AbsentID]=B.[AbsentID] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "           AND A.[AccountID]=@USER_ID ";
                strSQL += "           AND A.[Status]='1' ";
                strSQL += "           AND A.[TimeStart]<=GETDATE() ";
                strSQL += "           AND A.[TimeEnd]>=GETDATE() ";

                #endregion

                return dbFun.DoQuery(strSQL, parameter).ToList<AgentModel>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("代理人(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("代理人(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 簽核流程查詢(查詢)
        /// </summary>
        public IList<FormSignOff> PostFormSignOff(FormQueryModel query)
        {
            try
            {
                #region - 查詢 -

                var formData = formRepository.PostFormData(query);
         
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = formData.REQUISITION_ID },
                    new SqlParameter("@COMPANY_ID", SqlDbType.NVarChar) { Size = 64, Value = formData.COMPANY_ID },
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 64, Value = formData.DIAGRAM_ID }
                };

                strSQL = "";
                #region Original Code /**兼職部門會有重複顯示問題 20230530 Leon**/ 

                //strSQL += "WITH [CTE_SIGNATURE]([AUTO_COUNTER],[REQUISITION_ID],[PROCESS_ID],[APPROVER_DEPT],[APPROVER_ID],[APPROVER_NAME],[APPROVER_TIME],[RESULT_PROMPT],[COMMENT]) AS ";
                //strSQL += "( ";
                //strSQL += "     SELECT S.[AutoCounter],S.[RequisitionID],S.[ProcessID],S.[ApproverDept],S.[ApproverID],S.[ApproverName],S.[ApproveTime],S.[ResultPrompt],S.[Comment] ";
                //strSQL += "     FROM [FM7T_" + formData.IDENTIFY + "_S] AS S ";
                //strSQL += "     WHERE S.[RequisitionID]=@REQUISITION_ID ";
                //strSQL += "     UNION ALL ";
                //strSQL += "     SELECT '0',[RequisitionID],NULL,[ApplicantDept],[ApplicantID],[ApplicantName],[ApplicantDateTime],'提交申請',NULL ";
                //strSQL += "     FROM [FM7T_" + formData.IDENTIFY + "_M] ";
                //strSQL += "     WHERE [RequisitionID]=@REQUISITION_ID ";
                //strSQL += ") ";
                //strSQL += "SELECT S.[REQUISITION_ID],ISNULL(P.[DisplayName],'申請人') AS [PROCESS_NAME],A.[DEPT_NAME],A.[TITLE_NAME],S.[APPROVER_NAME],S.[APPROVER_TIME],S.[RESULT_PROMPT],S.[COMMENT] ";
                //strSQL += "FROM [CTE_SIGNATURE] AS S ";
                //strSQL += "         INNER JOIN [NUP].[dbo].[GTV_View_OrgRelationMember] AS A ON A.[COMPANY_ID]=@COMPANY_ID AND A.[USER_ID]=S.[APPROVER_ID] ";
                //strSQL += "         LEFT JOIN [FSe7en_Sys_ProcessName] AS P ON P.[DiagramID]=@DIAGRAM_ID AND P.[ProcessID]=S.[PROCESS_ID] ";
                //strSQL += "WHERE S.[APPROVER_NAME] IS NOT NULL ";
                //strSQL += "ORDER BY S.[AUTO_COUNTER] DESC ";

                #endregion

                #region /**代理簽核會不顯示問題 20230814 Leon**/ 

                //strSQL += "WITH [CTE_SIGNATURE]([AUTO_COUNTER],[REQUISITION_ID],[PROCESS_ID],[APPROVER_DEPT],[APPROVER_ID],[APPROVER_NAME],[APPROVER_TIME],[RESULT_PROMPT],[COMMENT]) AS ";
                //strSQL += "( ";
                //strSQL += "     SELECT S.[AutoCounter],S.[RequisitionID],S.[ProcessID],S.[ApproverDept],S.[ApproverID],S.[ApproverName],S.[ApproveTime],S.[ResultPrompt],S.[Comment] ";
                //strSQL += "     FROM [FM7T_" + formData.IDENTIFY + "_S] AS S ";
                //strSQL += "     WHERE S.[RequisitionID]=@REQUISITION_ID ";
                //strSQL += "     UNION ALL ";
                //strSQL += "     SELECT '0',[REQUISITION_ID],NULL,[APPLICANT_DEPT_ID],[APPLICANT_ID],[APPLICANT_NAME],[APPLICANT_DATETIME],'提交申請',NULL ";
                //strSQL += "     FROM [BPMPro].[dbo].[GTV_View_FormData] ";
                //strSQL += "     WHERE [REQUISITION_ID]=@REQUISITION_ID ";
                //strSQL += ") ";
                //strSQL += "SELECT S.[REQUISITION_ID],ISNULL(P.[DisplayName],'申請人') AS [PROCESS_NAME],A.[DEPT_NAME],A.[TITLE_NAME],S.[APPROVER_NAME],S.[APPROVER_TIME],S.[RESULT_PROMPT],S.[COMMENT] ";
                //strSQL += "FROM [CTE_SIGNATURE] AS S ";
                //strSQL += "         INNER JOIN [NUP].[dbo].[GTV_View_OrgRelationMember] AS A ON A.[COMPANY_ID]=@COMPANY_ID AND A.[USER_ID]=S.[APPROVER_ID] AND A.[DEPT_ID]=S.[APPROVER_DEPT] ";
                //strSQL += "         LEFT JOIN [FSe7en_Sys_ProcessName] AS P ON P.[DiagramID]=@DIAGRAM_ID AND P.[ProcessID]=S.[PROCESS_ID] ";
                //strSQL += "WHERE S.[APPROVER_NAME] IS NOT NULL ";
                //strSQL += "ORDER BY S.[AUTO_COUNTER] DESC ";

                #endregion

                strSQL += "WITH [CTE_SIGNATURE]([AUTO_COUNTER],[REQUISITION_ID],[PROCESS_ID],[APPROVER_DEPT],[DEPT_NAME],[TITLE_NAME],[APPROVER_ID],[APPROVER_NAME],[APPROVER_TIME],[RESULT_PROMPT],[COMMENT]) AS ";
                strSQL += "(";
                strSQL += "     SELECT ";
                strSQL += "         S.[AutoCounter] AS [AUTO_COUNTER], ";
                strSQL += "         S.[RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "         S.[ProcessID] AS [PROCESS_ID], ";
                strSQL += "         S.[ApproverDept] AS [APPROVER_DEPT], ";
                strSQL += "         ( ";
                strSQL += "                 SELECT ";
                strSQL += "                     [DEPT_NAME] ";
                strSQL += "                 FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "                 WHERE [DEPT_ID]=S.[ApproverDept] ";
                strSQL += "                 GROUP BY [DEPT_NAME] ";
                strSQL += "         ) AS [DEPT_NAME], ";
                strSQL += "         ( ";
                strSQL += "                 SELECT ";
                strSQL += "                     [TITLE_NAME] ";
                strSQL += "                 FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "                 WHERE  [USER_ID]=S.[ApproverID] AND [COMPANY_ID]=@COMPANY_ID ";
                strSQL += "                 GROUP BY [TITLE_NAME] ";
                strSQL += "         )AS [TITLE_NAME], ";
                strSQL += "         S.[ApproverID] AS [APPROVER_ID], ";
                strSQL += "         ( ";
                strSQL += "                 SELECT ";
                strSQL += "                     [USER_NAME] ";
                strSQL += "                 FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "                 WHERE [USER_ID]=S.[ApproverID] ";
                strSQL += "                 GROUP BY [USER_NAME] ";
                strSQL += "         )AS [APPROVER_NAME], ";
                strSQL += "         S.[ApproveTime] AS [APPROVE_TIME], ";
                strSQL += "         S.[ResultPrompt] AS [RESULT_PROMPT], ";
                strSQL += "         S.[Comment] AS [COMMENT] ";
                strSQL += "     FROM [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_S] AS S ";
                strSQL += "     WHERE S.[RequisitionID]=@REQUISITION_ID ";
                strSQL += "     UNION ALL ";
                strSQL += "     SELECT ";
                strSQL += "         '0', ";
                strSQL += "         [REQUISITION_ID], ";
                strSQL += "         NULL, ";
                strSQL += "         [APPLICANT_DEPT_ID], ";
                strSQL += "         [APPLICANT_DEPT_NAME], ";
                strSQL += "         ( ";
                strSQL += "                 SELECT ";
                strSQL += "                     [TITLE_NAME] ";
                strSQL += "                 FROM [NUP].[dbo].[GTV_Org_Relation_Member] ";
                strSQL += "                 WHERE  [USER_ID]=[APPLICANT_ID] AND [COMPANY_ID]=@COMPANY_ID ";
                strSQL += "                 GROUP BY [TITLE_NAME] ";
                strSQL += "         )AS [TITLE_NAME], ";
                strSQL += "     [APPLICANT_ID], ";
                strSQL += "     [APPLICANT_NAME], ";
                strSQL += "     [APPLICANT_DATETIME], ";
                strSQL += "     '提交申請', ";
                strSQL += "     NULL ";
                strSQL += "     FROM [BPMPro].[dbo].[GTV_View_FormData] ";
                strSQL += "     WHERE [REQUISITION_ID]=@REQUISITION_ID ";
                strSQL += ")";
                strSQL += "SELECT S.[REQUISITION_ID],ISNULL(P.[DisplayName],'申請人') AS [PROCESS_NAME],S.[DEPT_NAME],S.[TITLE_NAME],S.[APPROVER_NAME],S.[APPROVER_TIME],S.[RESULT_PROMPT],S.[COMMENT] ";
                strSQL += "FROM [CTE_SIGNATURE] AS S ";
                strSQL += "     LEFT JOIN [NUP].[dbo].[GTV_View_OrgRelationMember] AS A ON A.[COMPANY_ID]=@COMPANY_ID AND A.[USER_ID]=S.[APPROVER_ID] AND A.[DEPT_ID]=S.[APPROVER_DEPT] ";
                strSQL += "     LEFT JOIN [BPMPro].[dbo].[FSe7en_Sys_ProcessName] AS P ON P.[DiagramID]=@DIAGRAM_ID AND P.[ProcessID]=S.[PROCESS_ID] ";
                strSQL += "ORDER BY S.[AUTO_COUNTER] DESC ";

                #endregion

                return dbFun.DoQuery(strSQL, parameter).ToList<FormSignOff>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("簽核流程查詢(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("簽核流程查詢(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單(待簽核)列表(查詢)
        /// </summary>
        public IList<FormNextApprover> PostNextApproverByPending(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    new SqlParameter("@PROCESS_ID", SqlDbType.NVarChar) { Size = 50, Value = query.PROCESS_ID }
                };

                #region STEP1：同步更新簽核記錄表

                  SyncLogNextApprover(query);

                #endregion

                #region STEP2：取得(IDENTIFY)間接查詢M表的(重要性)欄位

                strSQL = "";
                strSQL += "SELECT [IDENTIFY] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_View_NextApprover] ";
                strSQL += "WHERE [REQUISITION_ID]=@REQUISITION_ID ";
                strSQL += "GROUP BY [IDENTIFY] ";

                var tabalName = dbFun.DoQuery(strSQL, parameter).Rows[0]["IDENTIFY"].ToString();

                #endregion

                #region STEP3：更新待簽核記錄表的(重要性)(1：低 Low、2：中 Normal、3：高 Heigh)

                strSQL = "";
                strSQL += "UPDATE [A] ";
                strSQL += "SET A.[PRIORITY]=B.[Priority] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Log_NextApprover] A ";
                strSQL += "         INNER JOIN [BPMPro].[dbo].[FM7T_"+ tabalName + "_M] B ON B.[RequisitionID]=A.[REQUISITION_ID] ";
                strSQL += "WHERE A.[REQUISITION_ID]=@REQUISITION_ID  ";

                dbFun.DoTran(strSQL, parameter);

                #endregion
                
                #region STEP4：查詢待簽核列表(未寄件)

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "    B.[COMPANY_ID],A.[REQUISITION_ID],A.[PROCESS_ID],A.[IDENTIFY],A.[APPROVER_GUID],A.[PRIORITY],A.[HANDLE_MINUTE],A.[SENT_COUNT],A.[SENT_LAST_TIME], ";
                strSQL += "    B.[PROCESS_NAME],B.[FORM_SUBJECT],B.[APPROVER_ID],B.[APPROVER_NAME],B.[APPROVER_EMAIL],B.[APPROVER_PHONE],B.[TIME_START],B.[IS_AGENT],B.[IS_AGENT_TEXT],B.[ORIGIN_APPROVER],B.[ORIGIN_APPROVER_NAME],B.[ORIGIN_APPROVER_EMAIL] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Log_NextApprover] A ";
                strSQL += "         RIGHT JOIN [BPMPro].[dbo].[GTV_View_NextApprover] B ON A.[APPROVER_GUID]=B.[APPROVER_GUID] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND A.[REQUISITION_ID]=@REQUISITION_ID ";
                strSQL += "          AND A.[PROCESS_ID]=@PROCESS_ID ";
                strSQL += "          AND A.[SENT_LAST_TIME] IS NULL "; //只針對未寄件的

                return dbFun.DoQuery(strSQL, parameter).ToList<FormNextApprover>();

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(待簽核)列表(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("表單(待簽核)列表(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單(同意)執行
        /// </summary>
        public void PutNextApproverByAgree(FormQueryModel query)
        {
            try
            {
                SyncLogNextApprover(query);
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(同意)執行錯誤，原因：" + ex.Message);
                throw new Exception("表單(同意)執行錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單(不同意)觸發
        /// </summary>
        public void PutNextApproverByDisagree(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    new SqlParameter("@PROCESS_ID", SqlDbType.NVarChar) { Size = 50, Value = query.PROCESS_ID }
                };

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FSe7en_Sys_NextApprover] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "           AND [RequisitionID]=@REQUISITION_ID ";
                strSQL += "           AND [ProcessID]<>@PROCESS_ID ";
                strSQL += "           AND [ProcessID]<>'AplSlf01' ";

                dbFun.DoTran(strSQL, parameter);

                SyncLogNextApprover(query);
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(不同意)觸發錯誤，原因：" + ex.Message);
                throw new Exception("表單(不同意)觸發錯誤，原因：" + ex.Message);
            }
        }

        #region 2022/09/28 Leon: 表單-條件分支(不同意)觸發

        /// <summary>
        /// 表單-條件分支(不同意)觸發
        /// </summary>
        public void PutActiveProcessByDisagree(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    new SqlParameter("@PROCESS_ID", SqlDbType.NVarChar) { Size = 50, Value = query.PROCESS_ID }
                };

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FSe7en_Sys_ActiveProcess] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "           AND [RequisitionID]=@REQUISITION_ID ";
                strSQL += "           AND [ProcessID]<>@PROCESS_ID ";
                strSQL += "           AND [ProcessID]<>'AplSlf01' ";

                dbFun.DoTran(strSQL, parameter);

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單-條件分支(不同意)觸發錯誤，原因：" + ex.Message);
                throw new Exception("表單-條件分支(不同意)觸發錯誤，原因：" + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 表單(已逾時)列表(查詢)
        /// </summary>
        public IList<FormNextApprover> PostNextApproverByOverTime(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    new SqlParameter("@PROCESS_ID", SqlDbType.NVarChar) { Size = 50, Value = query.PROCESS_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "    B.[COMPANY_ID],A.[REQUISITION_ID],A.[PROCESS_ID],A.[IDENTIFY],A.[APPROVER_GUID],A.[PRIORITY],A.[HANDLE_MINUTE],A.[SENT_COUNT],A.[SENT_LAST_TIME], ";
                strSQL += "    B.[PROCESS_NAME],B.[FORM_SUBJECT],B.[APPROVER_ID],B.[APPROVER_NAME],B.[APPROVER_EMAIL],B.[APPROVER_PHONE],B.[TIME_START],B.[IS_AGENT],B.[IS_AGENT_TEXT],B.[ORIGIN_APPROVER],B.[ORIGIN_APPROVER_NAME],B.[ORIGIN_APPROVER_EMAIL] ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Log_NextApprover] A ";
                strSQL += "         RIGHT JOIN [BPMPro].[dbo].[GTV_View_NextApprover] B ON A.[APPROVER_GUID]=B.[APPROVER_GUID] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "           AND B.[APPROVER_ID] NOT IN ";
                strSQL += "                  ( ";
                strSQL += "                     SELECT [ORIGIN_APPROVER] ";
                strSQL += "                     FROM [BPMPro].[dbo].[GTV_View_NextApprover] ";
                strSQL += "                     WHERE 1=1  ";
                strSQL += "                                 AND [REQUISITION_ID]=@REQUISITION_ID ";
                strSQL += "                                 AND [PROCESS_ID]=@PROCESS_ID ";
                strSQL += "                                 AND [ORIGIN_APPROVER]<>'' ";
                strSQL += "                  ) ";
                strSQL += "           AND A.[APPROVER_GUID] IN ";
                strSQL += "                  ( ";
                strSQL += "                     SELECT C.[APPROVER_GUID] ";
                strSQL += "                     FROM [BPMPro].[dbo].[GTV_Log_NextApprover] C ";
                strSQL += "                              INNER JOIN [BPMPro].[dbo].[GTV_FormOverTimeSetting] D ON C.[IDENTIFY]=D.[IDENTIFY] AND C.[PRIORITY]=D.[PRIORITY] AND (C.[SENT_COUNT]+1)=D.[OVER_TIME_COUNT] ";
                strSQL += "                     WHERE 1=1  ";
                strSQL += "                                AND C.[REQUISITION_ID]=@REQUISITION_ID ";
                strSQL += "                                AND C.[PROCESS_ID]=@PROCESS_ID ";
                strSQL += "                                AND C.[HANDLE_MINUTE] >= (D.[LIMIT_HOUR]*60) ";  //換算為分鐘
                strSQL += "	               )";

                return dbFun.DoQuery(strSQL, parameter).ToList<FormNextApprover>();
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(逾時)列表(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("表單(逾時)列表(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單(結案)列表(查詢)
        /// </summary>
        public IList<FormFinalApprover> PostNextApproverByClose(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                #region STEP1：取得(COMPANY_ID、IDENTIFY)

                strSQL = "";
                strSQL += "SELECT B.[CompanyID],A.[Identify] ";
                strSQL += "FROM [BPMPro].[dbo].[F7Tep_View_Requisition] A ";
                strSQL += "         INNER JOIN [NUP].[dbo].[FSe7en_Org_DeptStruct] B ON B.[DeptID]=A.[DeptID] ";
                strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

                var dt = dbFun.DoQuery(strSQL, parameter);

                var compName = dt.Rows[0]["CompanyID"].ToString();
                var tableName = dt.Rows[0]["Identify"].ToString();

                parameter.Add(new SqlParameter("@COMPANY_ID", SqlDbType.NVarChar) { Size = 20, Value = compName });

                #endregion

                #region STEP2：查詢(FM7T_{?}_S)表的簽核歷程

                #region - 原查詢(FM7T_{?}_S)表的簽核歷程 -
                //[NUP].[dbo].[NUP_View_ALL_MemberData]的View有些人原資訊會抓不到導致；收件人顯示變成：	、廖佳瑩、王克捷的問題。

                //strSQL = "";
                //strSQL += "SELECT ";
                //strSQL += "     S.[RequisitionID] AS [REQUISITION_ID], ";
                //strSQL += "     S.[ApproverID] AS [APPROVER_ID], ";
                //strSQL += "     CASE ";
                //strSQL += "        WHEN S.[OriginalApprover] IS NULL THEN M1.[MemberName] ";
                //strSQL += "        WHEN S.[OriginalApprover] IS NOT NULL THEN M1.[MemberName] ";
                //strSQL += "     END [APPROVER_NAME], ";
                //strSQL += "     M1.[EMail] AS [APPROVER_EMAIL], ";
                //strSQL += "     M1.[Mobile] AS [APPROVER_PHONE], ";
                //strSQL += "     S.[OriginalApprover] AS [ORIGIN_APPROVER], ";
                //strSQL += "     M2.[MemberName] AS [ORIGIN_APPROVER_NAME], ";
                //strSQL += "     M2.[EMail] AS [ORIGIN_APPROVER_EMAIL], ";
                //strSQL += "     M2.[Mobile] AS [ORIGIN_APPROVER_PHONE] ";
                //strSQL += "FROM [BPMPro].[dbo].[FM7T_" + tableName + "_S] S ";
                //strSQL += "         LEFT JOIN [NUP].[dbo].[NUP_View_ALL_MemberData] M1 ON M1.[CompanyID]=@COMPANY_ID AND M1.[AccountID]=S.[ApproverID] ";
                //strSQL += "         LEFT JOIN [NUP].[dbo].[NUP_View_ALL_MemberData] M2 ON M2.[CompanyID]=@COMPANY_ID AND M2.[AccountID]=S.[OriginalApprover] AND S.[OriginalApprover] IS NOT NULL ";
                //strSQL += "WHERE S.[RequisitionID]=@REQUISITION_ID ";
                //strSQL += "UNION ALL ";
                //strSQL += "SELECT ";
                //strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                //strSQL += "     [ApplicantID] AS [APPROVER_ID], ";
                //strSQL += "     [ApplicantName] AS [APPROVER_NAME], ";
                //strSQL += "     B.[EMail] AS [APPROVER_EMAIL], ";
                //strSQL += "     B.[Mobile] AS [ORIGIN_APPROVER_PHONE], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER_NAME], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER_EMAIL], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER_PHONE] ";
                //strSQL += "FROM [FM7T_" + tableName + "_M] A ";
                //strSQL += "         LEFT JOIN [NUP].[dbo].[NUP_View_ALL_MemberData] B ON B.[CompanyID]=@COMPANY_ID AND B.[AccountID]=A.[ApplicantID] ";
                //strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     S.[RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     S.[ApproverID] AS [APPROVER_ID], ";
                strSQL += "     CASE ";
                strSQL += "        WHEN S.[OriginalApprover] IS NULL THEN M1.[USER_NAME] ";
                strSQL += "        WHEN S.[OriginalApprover] IS NOT NULL THEN M1.[USER_NAME] ";
                strSQL += "     END [APPROVER_NAME], ";
                strSQL += "     M1.[EMail] AS [APPROVER_EMAIL], ";
                strSQL += "     M1.[Mobile] AS [APPROVER_PHONE], ";
                strSQL += "     S.[OriginalApprover] AS [ORIGIN_APPROVER], ";
                strSQL += "     M2.[USER_NAME] AS [ORIGIN_APPROVER_NAME], ";
                strSQL += "     M2.[EMail] AS [ORIGIN_APPROVER_EMAIL], ";
                strSQL += "     M2.[Mobile] AS [ORIGIN_APPROVER_PHONE] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + tableName + "_S] S ";
                strSQL += "         LEFT JOIN [NUP].[dbo].[GTV_Org_Relation_Member] M1 ON M1.[COMPANY_ID]=@COMPANY_ID AND M1.[USER_ID]=S.[ApproverID] ";
                strSQL += "         LEFT JOIN [NUP].[dbo].[GTV_Org_Relation_Member] M2 ON M2.[COMPANY_ID]=@COMPANY_ID AND M2.[USER_ID]=S.[OriginalApprover] AND S.[OriginalApprover] IS NOT NULL ";
                strSQL += "WHERE S.[RequisitionID]=@REQUISITION_ID ";
                strSQL += "UNION ALL ";
                strSQL += "SELECT ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     [ApplicantID] AS [APPROVER_ID], ";
                strSQL += "     [ApplicantName] AS [APPROVER_NAME], ";
                strSQL += "     B.[EMail] AS [APPROVER_EMAIL], ";
                strSQL += "     B.[Mobile] AS [ORIGIN_APPROVER_PHONE], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER_NAME], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER_EMAIL], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER_PHONE] ";
                strSQL += "FROM [FM7T_" + tableName + "_M] A ";
                strSQL += "         LEFT JOIN [NUP].[dbo].[GTV_Org_Relation_Member] B ON B.[COMPANY_ID]=@COMPANY_ID AND B.[USER_ID]=A.[ApplicantID] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                return dbFun.DoQuery(strSQL, parameter).ToList<FormFinalApprover>();

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(結案)列表(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("表單(結案)列表(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單(駁回結束)列表(查詢)
        /// </summary>
        public IList<FormFinalApprover> PostNextApproverByDisagreeClose(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                #region STEP1：取得(COMPANY_ID、IDENTIFY)

                strSQL = "";
                strSQL += "SELECT B.[CompanyID],A.[Identify] ";
                strSQL += "FROM [BPMPro].[dbo].[F7Tep_View_Requisition] A ";
                strSQL += "         INNER JOIN [NUP].[dbo].[FSe7en_Org_DeptStruct] B ON B.[DeptID]=A.[DeptID] ";
                strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

                var dt = dbFun.DoQuery(strSQL, parameter);

                var compName = dt.Rows[0]["CompanyID"].ToString();
                var tableName = dt.Rows[0]["Identify"].ToString();

                parameter.Add(new SqlParameter("@COMPANY_ID", SqlDbType.NVarChar) { Size = 20, Value = compName });

                #endregion

                #region STEP2：查詢(FM7T_{?}_S)表的簽核歷程

                #region - 原查詢(FM7T_{?}_S)表的簽核歷程 -
                //[NUP].[dbo].[NUP_View_ALL_MemberData]的View有些人原資訊會抓不到導致；收件人顯示變成：孫慶偉、薛鳳、、廖佳瑩、何聖文的問題。

                //strSQL = "";
                //strSQL += "SELECT ";
                //strSQL += "     S.[RequisitionID] AS [REQUISITION_ID], ";
                //strSQL += "     S.[ApproverID] AS [APPROVER_ID], ";
                //strSQL += "     CASE ";
                //strSQL += "        WHEN S.[OriginalApprover] IS NULL THEN M1.[MemberName] ";
                //strSQL += "        WHEN S.[OriginalApprover] IS NOT NULL THEN M1.[MemberName] ";
                //strSQL += "     END [APPROVER_NAME], ";
                //strSQL += "     M1.[EMail] AS [APPROVER_EMAIL], ";
                //strSQL += "     M1.[Mobile] AS [APPROVER_PHONE], ";
                //strSQL += "     S.[OriginalApprover] AS [ORIGIN_APPROVER], ";
                //strSQL += "     M2.[MemberName] AS [ORIGIN_APPROVER_NAME], ";
                //strSQL += "     M2.[EMail] AS [ORIGIN_APPROVER_EMAIL], ";
                //strSQL += "     M2.[Mobile] AS [ORIGIN_APPROVER_PHONE] ";
                //strSQL += "FROM [BPMPro].[dbo].[FM7T_" + tableName + "_S] S ";
                //strSQL += "         LEFT JOIN [NUP].[dbo].[NUP_View_ALL_MemberData] M1 ON M1.[CompanyID]=@COMPANY_ID AND M1.[AccountID]=S.[ApproverID] ";
                //strSQL += "         LEFT JOIN [NUP].[dbo].[NUP_View_ALL_MemberData] M2 ON M2.[CompanyID]=@COMPANY_ID AND M2.[AccountID]=S.[OriginalApprover] AND S.[OriginalApprover] IS NOT NULL ";
                //strSQL += "WHERE S.[RequisitionID]=@REQUISITION_ID ";
                //strSQL += "UNION ALL ";
                //strSQL += "SELECT ";
                //strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                //strSQL += "     [ApplicantID] AS [APPROVER_ID], ";
                //strSQL += "     [ApplicantName] AS [APPROVER_NAME], ";
                //strSQL += "     B.[EMail] AS [APPROVER_EMAIL], ";
                //strSQL += "     B.[Mobile] AS [ORIGIN_APPROVER_PHONE], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER_NAME], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER_EMAIL], ";
                //strSQL += "     NULL AS [ORIGIN_APPROVER_PHONE] ";
                //strSQL += "FROM [FM7T_" + tableName + "_M] A ";
                //strSQL += "         LEFT JOIN [NUP].[dbo].[NUP_View_ALL_MemberData] B ON B.[CompanyID]=@COMPANY_ID AND B.[AccountID]=A.[ApplicantID] ";
                //strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     S.[RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     S.[ApproverID] AS [APPROVER_ID], ";
                strSQL += "     CASE ";
                strSQL += "        WHEN S.[OriginalApprover] IS NULL THEN M1.[USER_NAME] ";
                strSQL += "        WHEN S.[OriginalApprover] IS NOT NULL THEN M1.[USER_NAME] ";
                strSQL += "     END [APPROVER_NAME], ";
                strSQL += "     M1.[EMail] AS [APPROVER_EMAIL], ";
                strSQL += "     M1.[Mobile] AS [APPROVER_PHONE], ";
                strSQL += "     S.[OriginalApprover] AS [ORIGIN_APPROVER], ";
                strSQL += "     M2.[USER_NAME] AS [ORIGIN_APPROVER_NAME], ";
                strSQL += "     M2.[EMail] AS [ORIGIN_APPROVER_EMAIL], ";
                strSQL += "     M2.[Mobile] AS [ORIGIN_APPROVER_PHONE] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + tableName + "_S] S ";
                strSQL += "         LEFT JOIN [NUP].[dbo].[GTV_Org_Relation_Member] M1 ON M1.[COMPANY_ID]=@COMPANY_ID AND M1.[USER_ID]=S.[ApproverID] ";
                strSQL += "         LEFT JOIN [NUP].[dbo].[GTV_Org_Relation_Member] M2 ON M2.[COMPANY_ID]=@COMPANY_ID AND M2.[USER_ID]=S.[OriginalApprover] AND S.[OriginalApprover] IS NOT NULL ";
                strSQL += "WHERE S.[RequisitionID]=@REQUISITION_ID ";
                strSQL += "UNION ALL ";
                strSQL += "SELECT ";
                strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
                strSQL += "     [ApplicantID] AS [APPROVER_ID], ";
                strSQL += "     [ApplicantName] AS [APPROVER_NAME], ";
                strSQL += "     B.[EMail] AS [APPROVER_EMAIL], ";
                strSQL += "     B.[Mobile] AS [ORIGIN_APPROVER_PHONE], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER_NAME], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER_EMAIL], ";
                strSQL += "     NULL AS [ORIGIN_APPROVER_PHONE] ";
                strSQL += "FROM [FM7T_" + tableName + "_M] A ";
                strSQL += "         LEFT JOIN [NUP].[dbo].[GTV_Org_Relation_Member] B ON B.[COMPANY_ID]=@COMPANY_ID AND B.[USER_ID]=A.[ApplicantID] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                return dbFun.DoQuery(strSQL, parameter).ToList<FormFinalApprover>();

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(結案)列表(查詢)錯誤，原因：" + ex.Message);
                throw new Exception("表單(結案)列表(查詢)錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 表單(結束)執行
        /// </summary>
        public void PutFormClose(FormQueryModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[GTV_Log_NextApprover] ";
                strSQL += "WHERE [REQUISITION_ID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameter);
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("表單(結束)執行錯誤，原因：" + ex.Message);
                throw new Exception("表單(結束)執行錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 更新(待簽核)記錄表：(次數+1、時間更新)
        /// </summary>
        public void PutLogNextApprover(List<string> guidList)
        {
            try
            {
                var approversGUID = String.Join(",", guidList);
                var parameter = new List<SqlParameter>();

                if (!String.IsNullOrEmpty(approversGUID))
                {
                    string strFilmPurposes = dbFun.SetSqlParameterList(ref parameter, approversGUID, "GUID_");

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[GTV_Log_NextApprover] ";
                    strSQL += "SET [SENT_COUNT]=[SENT_COUNT]+1 , ";
                    strSQL += "       [SENT_LAST_TIME]=GETDATE() ";
                    strSQL += "WHERE [APPROVER_GUID] IN (" + strFilmPurposes + ") ";

                    dbFun.DoTran(strSQL, parameter);
                } 
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("更新(待簽核)記錄表錯誤，原因：" + ex.Message);
                throw new Exception("更新(待簽核)記錄表錯誤，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 同步更新簽核記錄表
        /// </summary>
        public void SyncLogNextApprover(FormQueryModel query) 
        {
            var parameter = new List<SqlParameter>()
            {
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            strSQL = "";
            strSQL += "MERGE [BPMPro].[dbo].[GTV_Log_NextApprover] AS T ";
            strSQL += "USING [BPMPro].[dbo].[GTV_View_NextApprover] AS S ";
            strSQL += "ON (T.[APPROVER_GUID]=S.[APPROVER_GUID]) ";
            strSQL += "WHEN NOT MATCHED BY TARGET AND S.[REQUISITION_ID]=@REQUISITION_ID THEN ";
            strSQL += "      INSERT([REQUISITION_ID],[PROCESS_ID],[IDENTIFY],[APPROVER_GUID]) ";
            strSQL += "      VALUES(S.[REQUISITION_ID], S.[PROCESS_ID], S.[IDENTIFY], S.[APPROVER_GUID]) ";
            strSQL += "WHEN NOT MATCHED BY SOURCE AND T.[REQUISITION_ID]=@REQUISITION_ID THEN ";
            strSQL += "      DELETE; ";

            dbFun.DoTran(strSQL, parameter);
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