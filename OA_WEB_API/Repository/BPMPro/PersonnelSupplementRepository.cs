using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models;
using System.Collections;
using System.Drawing;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 人員增補單
    /// </summary>
    public class PersonnelSupplementRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();
        SysCommonRepository sysCommonRepository = new SysCommonRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 人員增補單(查詢)
        /// </summary>
        public PersonnelSupplementViewModel PostPersonnelSupplementSingle(PersonnelSupplementQueryModel query)
        {
            var parameter = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            #region - 申請人資訊 -

            var CommonApplicantInfo = new BPMCommonModel<ApplicantInfo>()
            {
                EXT = "M",
                IDENTIFY = IDENTIFY,
                PARAMETER = parameter,
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostApplicantInfoFunction(CommonApplicantInfo));
            var applicantInfo = jsonFunction.JsonToObject<ApplicantInfo>(strJson);

            #endregion

            #region - 人員增補單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var personnelSupplementTitle = dbFun.DoQuery(strSQL, parameter).ToList<PersonnelSupplementTitle>().FirstOrDefault();

            #endregion

            #region - 人員增補單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Occupation] AS [OCCUPATION], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [ReasonNote] AS [REASON_NOTE], ";
            strSQL += "     [BraidNum] AS [BRAID_NUM], ";
            strSQL += "     [NowNum] AS [NOW_NUM], ";
            strSQL += "     [DemandNum] AS [DEMAND_NUM], ";
            strSQL += "     CAST([IsPartTime] as bit) AS [IS_PART_TIME], ";
            strSQL += "     [SalaryMin] AS [SALARY_MIN], ";
            strSQL += "     [SalaryMax] AS [SALARY_MAX], ";
            strSQL += "     [EducationRequirement] AS [EDUCATION_REQUIREMENT], ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [ApplicationMethod] AS [APPLICATION_METHOD], ";
            strSQL += "     [ContactEmail] AS [CONTACT_EMAIL], ";
            strSQL += "     [ComputerSkills] AS [COMPUTER_SKILLS], ";
            strSQL += "     [JobSkill] AS [JOB_SKILL], ";
            strSQL += "     [ApprovalNo] AS [APPROVAL_NO], ";
            strSQL += "     [ImplementDate] AS [IMPLEMENT_DATE], ";
            strSQL += "     [CloseDate] AS [CLOSE_DATE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var personnelSupplementConfig = dbFun.DoQuery(strSQL, parameter).ToList<PersonnelSupplementConfig>().FirstOrDefault();

            #endregion

            #region - 人員增補單 增補明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [Name] AS [NAME], ";
            strSQL += "     [Date] AS [DATE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_PersonnelSupplement_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var personnelSupplementDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<PersonnelSupplementDetailsConfig>();

            #endregion

            var personnelSupplementViewModel = new PersonnelSupplementViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                PERSONNEL_SUPPLEMENT_TITLE = personnelSupplementTitle,
                PERSONNEL_SUPPLEMENT_CONFIG = personnelSupplementConfig,
                PERSONNEL_SUPPLEMENT_DTLS_CONFIG = personnelSupplementDetailsConfig,
            };

            #region - 確認表單 -

            if (personnelSupplementViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    personnelSupplementViewModel = new PersonnelSupplementViewModel();
                    CommLib.Logger.Error("人員增補單(查詢)失敗，原因：系統無正常起單。");
                }
                else
                {
                    #region - 確認M表BPM表單單號 -

                    //避免儲存後送出表單BPM表單單號沒寫入的情形
                    var formQuery = new FormQueryModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID
                    };
                    notifyRepository.ByInsertBPMFormNo(formQuery);

                    if (String.IsNullOrEmpty(personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return personnelSupplementViewModel;
        }

        /// <summary>
        /// 人員增補單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutPersonnelSupplementRefill(PersonnelSupplementQueryModel query)
        {
            bool vResult = false;

            try
            {
                if (!String.IsNullOrEmpty(query.REQUISITION_ID) || !String.IsNullOrWhiteSpace(query.REQUISITION_ID))
                {
                    #region - 宣告 -

                    var original = PostPersonnelSupplementSingle(query);
                    strJson = jsonFunction.ObjectToJSON(original);

                    var personnelSupplementViewModel = new PersonnelSupplementViewModel();

                    var requisitionID = Guid.NewGuid().ToString();

                    #endregion

                    #region - 重送內容 -

                    personnelSupplementViewModel = jsonFunction.JsonToObject<PersonnelSupplementViewModel>(strJson);

                    #region - 申請人資訊 調整 -

                    personnelSupplementViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
                    personnelSupplementViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
                    personnelSupplementViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

                    #endregion

                    #region - 人員增補單 表單內容 調整 -

                    personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_CONFIG.APPROVAL_NO = null;
                    personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_CONFIG.IMPLEMENT_DATE = null;
                    personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_CONFIG.CLOSE_DATE = null;

                    #endregion

                    #region - 人員增補單 增補明細 清空 -

                    personnelSupplementViewModel.PERSONNEL_SUPPLEMENT_DTLS_CONFIG = null;

                    #endregion

                    #endregion

                    #region - 送出 執行(新增/修改/草稿) -

                    PutPersonnelSupplementSingle(personnelSupplementViewModel);

                    #endregion

                    vResult = true;
                }
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("人員增補單(依此單內容重送)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 人員增補單(新增/修改/草稿)
        /// </summary>
        public bool PutPersonnelSupplementSingle(PersonnelSupplementViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                FM7Subject = model.PERSONNEL_SUPPLEMENT_TITLE.FM7_SUBJECT;

                var ParentDeptName = sysCommonRepository.GetGTVDeptTree().Where(GTV => GTV.DEPT_ID.Contains(model.APPLICANT_INFO.APPLICANT_DEPT)).Select(GTV => GTV.PARENT_DEPT_NAME).FirstOrDefault();
                if (String.IsNullOrEmpty(ParentDeptName) || String.IsNullOrWhiteSpace(ParentDeptName)) sysCommonRepository.GetGPIDeptTree().Where(GPI => GPI.DEPT_ID.Contains(model.APPLICANT_INFO.APPLICANT_DEPT)).Select(GPI => GPI.PARENT_DEPT_NAME).FirstOrDefault();
                var DeptName = sysCommonRepository.GetGTVDeptTree().Where(GTV => GTV.DEPT_ID.Contains(model.APPLICANT_INFO.APPLICANT_DEPT)).Select(GTV => GTV.DEPT_NAME).FirstOrDefault();
                if (String.IsNullOrEmpty(DeptName) || String.IsNullOrWhiteSpace(DeptName)) sysCommonRepository.GetGPIDeptTree().Where(GPI => GPI.DEPT_ID.Contains(model.APPLICANT_INFO.APPLICANT_DEPT)).Select(GPI => GPI.DEPT_NAME).FirstOrDefault();

                if (String.IsNullOrEmpty(FM7Subject) || String.IsNullOrWhiteSpace(FM7Subject))
                {
                    FM7Subject = ParentDeptName + "_" + DeptName + "_人員增補_職別：" + model.PERSONNEL_SUPPLEMENT_CONFIG.OCCUPATION + "，需求人數為：" + model.PERSONNEL_SUPPLEMENT_CONFIG.DEMAND_NUM + "人";
                }

                #endregion

                #endregion

                #region - 人員增補單 表頭資訊：PersonnelSupplement_M -

                var parameterTitle = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  strREQ},
                    new SqlParameter("@DIAGRAM_ID", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.DIAGRAM_ID },
                    new SqlParameter("@PRIORITY", SqlDbType.Int) { Value =  model.APPLICANT_INFO.PRIORITY},
                    new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value =  model.APPLICANT_INFO.DRAFT_FLAG},
                    new SqlParameter("@FLOW_ACTIVATED", SqlDbType.Int) { Value =  model.APPLICANT_INFO.FLOW_ACTIVATED},
                    //(申請人/起案人)資訊
                    new SqlParameter("@APPLICANT_DEPT", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT },
                    new SqlParameter("@APPLICANT_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_DEPT_NAME },
                    new SqlParameter("@APPLICANT_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_ID },
                    new SqlParameter("@APPLICANT_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.APPLICANT_NAME },
                    new SqlParameter("@APPLICANT_PHONE", SqlDbType.NVarChar) { Size = 50, Value = model.APPLICANT_INFO.APPLICANT_PHONE ?? String.Empty },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //人員增補單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    var formData = new FormData()
                    {
                        REQUISITION_ID = strREQ
                    };

                    if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                    {
                        parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });
                        IsADD = true;
                    }
                }
                else parameterTitle.Add(new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) });

                #endregion

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";

                    if (IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

                    strSQL += "     [FillerID]=@FILLER_ID, ";
                    strSQL += "     [FillerName]=@FILLER_NAME, ";
                    strSQL += "     [Priority]=@PRIORITY, ";
                    strSQL += "     [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "     [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 人員增補單 表單內容：PersonnelSupplement_M -

                if (model.PERSONNEL_SUPPLEMENT_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //人員增補單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@OCCUPATION", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REASON_NOTE", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BRAID_NUM", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOW_NUM", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DEMAND_NUM", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_PART_TIME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SALARY_MIN", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SALARY_MAX", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EDUCATION_REQUIREMENT", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPLICATION_METHOD", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACT_EMAIL", SqlDbType.NVarChar) { Size = 255, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COMPUTER_SKILLS", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@JOB_SKILL", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVAL_NO", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IMPLEMENT_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CLOSE_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    #region - 確認新單人資主管填的值就要清空 -

                    var formData = new FormData()
                    {
                        REQUISITION_ID = strREQ
                    };

                    if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                    {
                        model.PERSONNEL_SUPPLEMENT_CONFIG.APPROVAL_NO = null;
                        model.PERSONNEL_SUPPLEMENT_CONFIG.IMPLEMENT_DATE = null;
                        model.PERSONNEL_SUPPLEMENT_CONFIG.CLOSE_DATE = null;
                    }

                    #endregion

                    //寫入：人員增補單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.PERSONNEL_SUPPLEMENT_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [Occupation]=@OCCUPATION, ";
                    strSQL += "     [Reason]=@REASON, ";
                    strSQL += "     [ReasonNote]=@REASON_NOTE, ";
                    strSQL += "     [BraidNum]=@BRAID_NUM, ";
                    strSQL += "     [NowNum]=@NOW_NUM, ";
                    strSQL += "     [DemandNum]=@DEMAND_NUM, ";
                    strSQL += "     [IsPartTime]=@IS_PART_TIME, ";
                    strSQL += "     [SalaryMin]=@SALARY_MIN, ";
                    strSQL += "     [SalaryMax]=@SALARY_MAX, ";
                    strSQL += "     [EducationRequirement]=@EDUCATION_REQUIREMENT, ";
                    strSQL += "     [Description]=@DESCRIPTION, ";
                    strSQL += "     [ApplicationMethod]=@APPLICATION_METHOD, ";
                    strSQL += "     [ContactEmail]=@CONTACT_EMAIL, ";
                    strSQL += "     [ComputerSkills]=@COMPUTER_SKILLS, ";
                    strSQL += "     [JobSkill]=@JOB_SKILL, ";
                    strSQL += "     [ApprovalNo]=@APPROVAL_NO, ";
                    strSQL += "     [ImplementDate]=@IMPLEMENT_DATE, ";
                    strSQL += "     [CloseDate]=@CLOSE_DATE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 人員增補單 增補明細：PersonnelSupplement_D -

                var parameterDetails = new List<SqlParameter>()
                {
                    //人員增補單 增補明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_PersonnelSupplement_D] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.PERSONNEL_SUPPLEMENT_DTLS_CONFIG != null && model.PERSONNEL_SUPPLEMENT_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.PERSONNEL_SUPPLEMENT_DTLS_CONFIG)
                    {
                        if (!String.IsNullOrEmpty(item.NAME) || !String.IsNullOrWhiteSpace(item.NAME))
                        {
                            //寫入：人員增補單 增補明細parameter
                            strJson = jsonFunction.ObjectToJSON(item);
                            GlobalParameters.Infoparameter(strJson, parameterDetails);

                            strSQL = "";
                            strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_PersonnelSupplement_D]([RequisitionID],[Name],[Date]) ";
                            strSQL += "VALUES(@REQUISITION_ID,@NAME,@DATE) ";

                            dbFun.DoTran(strSQL, parameterDetails);
                        }
                    }

                    #endregion
                }

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = strREQ;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = strREQ;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region - 送出表單：FormAutoStart -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = strREQ;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = strREQ;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = strREQ,
                    IDENTIFY = IDENTIFY,
                    DRAFT_FLAG = 0
                };
                commonRepository.PostBPMFormFunction(BPM_FormFunction);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("人員增補單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
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
        private string IDENTIFY = "PersonnelSupplement";

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

        #endregion
    }
}