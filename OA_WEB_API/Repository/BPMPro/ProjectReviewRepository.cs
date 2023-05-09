using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 專案建立審核單
    /// </summary>
    public class ProjectReviewRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #endregion

        #region  - 方法 - 

        /// <summary>
        /// 專案建立審核單(查詢)
        /// </summary>
        public ProjectReviewViewModel PostProjectReviewSingle(ProjectReviewQueryModel query)
        {
            #region  - 查詢 - 

            #region  - 申請人資訊 - 
            var parameterA = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }           
            };

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     A.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     A.[DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     B.[Value] AS [FM7_SUBJECT], ";
            strSQL += "     A.[ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     A.[ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     A.[ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     A.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     NULL AS [APPLICANT_PHONE], ";
            strSQL += "     A.[ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     A.[FillerID] AS [FILLER_ID], ";
            strSQL += "     A.[FillerName] AS [FILLER_NAME], ";
            strSQL += "     A.[Priority] AS [PRIORITY], ";
            strSQL += "     A.[DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     A.[FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_ProjectReview_M] A";
            strSQL += "         INNER JOIN [BPMPro].[dbo].[FSe7en_Tep_FormHeader] B ON A.[RequisitionID]=B.[RequisitionID]";
            strSQL += "WHERE A.[RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameterA).ToList<ApplicantInfo>().FirstOrDefault();
            #endregion

            #region - M表寫入BPM表單單號 -

            //避免儲存後送出表單BPM表單單號沒寫入的情形
            var formQuery = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            if (applicantInfo.DRAFT_FLAG == 0) notifyRepository.ByInsertBPMFormNo(formQuery);

            #endregion

            #region - 專案建立審核設定及內容 -

            var parameterB = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }                 
            };

            strSQL = "";
            strSQL += "SELECT ";                          
            strSQL += "     [ProjectName] AS [PROJECT_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [ProjectNickname] AS [PROJECT_NICKNAME], ";
            strSQL += "     [UseYear] AS [USE_YEAR], ";
            strSQL += "     [OwnerDep] AS [OWNER_DEP], ";            
            strSQL += "     [StartID] AS [START_ID], ";
            strSQL += "     [CreateDate] AS [CREATE_DATE], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_ProjectReview_M] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
            
            var projectReviewERPConfig = dbFun.DoQuery(strSQL, parameterB).ToList<ProjectReviewErpConfig>().FirstOrDefault();

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [AccCategory] AS [ACC_CATEGORY], ";
            strSQL += "     [GADReview] AS [GAD_REVIEW] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_ProjectReview_M] ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";

            var projectReviewBPMConfig = dbFun.DoQuery(strSQL, parameterB).ToList<ProjectReviewBpmConfig>().FirstOrDefault();

            #endregion                      

            #endregion

            var projectReview = new ProjectReviewViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                PROJECT_REVIEW_ERP_CONFIG = projectReviewERPConfig,
                PROJECT_REVIEW_BPM_CONFIG = projectReviewBPMConfig
            };
            return projectReview;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 專案建立審核單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutProjectReviewRefill(ProjectReviewQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("專案建立審核單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}
        
        #endregion

        /// <summary>
        /// 專案建立審核單(新增/修改/草稿)
        /// </summary>
        public bool PutProjectReviewSingle(ProjectReviewViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告主旨 -

                FM7Subject = model.PROJECT_REVIEW_ERP_CONFIG.PROJECT_NAME + "-" + model.PROJECT_REVIEW_ERP_CONFIG.PROJECT_NICKNAME;
                
                #endregion

                #region - 專案建立審核主表：ProjectReview_M -

                var parameterA = new List<SqlParameter>()
                {
                    //表單資訊
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value =  model.APPLICANT_INFO.REQUISITION_ID},
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
                    new SqlParameter("@APPLICANT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) },
                    //(填單人/代填單人)資訊
                    new SqlParameter("@FILLER_ID", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_ID },
                    new SqlParameter("@FILLER_NAME", SqlDbType.NVarChar) { Size = 40, Value = model.APPLICANT_INFO.FILLER_NAME },
                    //表單內容_ERP                                        
                    new SqlParameter("@PROJECT_NAME", SqlDbType.NVarChar) { Size = 50, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.PROJECT_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@PROJECT_NICKNAME", SqlDbType.NVarChar) { Size = 500, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.PROJECT_NICKNAME ?? DBNull.Value },
                    new SqlParameter("@USE_YEAR", SqlDbType.NVarChar) { Size = 50, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.USE_YEAR ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEP", SqlDbType.NVarChar) { Size = 50, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.OWNER_DEP ?? DBNull.Value },
                    new SqlParameter("@START_ID", SqlDbType.NVarChar) { Size = 50, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.START_ID ?? DBNull.Value },
                    new SqlParameter("@CREATE_DATE", SqlDbType.NVarChar) { Size = 50, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.CREATE_DATE ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 500, Value = (object)model.PROJECT_REVIEW_ERP_CONFIG.NOTE ?? DBNull.Value },
                    //表單內容_BPM
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@GAD_REVIEW", SqlDbType.NVarChar) { Size = 10, Value = (object)model.PROJECT_REVIEW_BPM_CONFIG.GAD_REVIEW ?? DBNull.Value },
                    new SqlParameter("@ACC_CATEGORY", SqlDbType.NVarChar) { Size = 50, Value = (object)model.PROJECT_REVIEW_BPM_CONFIG.ACC_CATEGORY ?? DBNull.Value },
                };
                
                
                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_ProjectReview_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterA);

                if (dtA.Rows.Count > 0)
                {
                    #region 修改

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_ProjectReview_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "      [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "      [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "      [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "      [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "      [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "      [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "      [FillerID]=@FILLER_ID, ";
                    strSQL += "      [FillerName]=@FILLER_NAME, ";
                    strSQL += "      [Priority]=@PRIORITY, ";
                    strSQL += "      [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "      [FlowActivated]=@FLOW_ACTIVATED, ";                    
                    strSQL += "      [ProjectName]=@PROJECT_NAME, ";
                    strSQL += "      [FormNo]=@FORM_NO, ";
                    strSQL += "      [ProjectNickname]=@PROJECT_NICKNAME, ";
                    strSQL += "      [UseYear]=@USE_YEAR, ";
                    strSQL += "      [OwnerDep]=@OWNER_DEP, ";                    
                    strSQL += "      [StartID]=@START_ID, ";
                    strSQL += "      [CreateDate]=@CREATE_DATE, ";                    
                    strSQL += "      [Note]=@NOTE, ";
                    strSQL += "      [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "      [AccCategory]=@ACC_CATEGORY, ";
                    strSQL += "      [GADReview]=@GAD_REVIEW ";                
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }
                else
                {
                    #region 新增

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_ProjectReview_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[FillerID],[FillerName],[ApplicantDateTime],[Priority],[DraftFlag],[FlowActivated],[ProjectName],[FormNo],[ProjectNickname],[UseYear],[OwnerDep],[StartID],[CreateDate],[Note],[FM7Subject],[AccCategory],[GADReview]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@FILLER_ID,@FILLER_NAME,@APPLICANT_DATETIME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@PROJECT_NAME,@FORM_NO,@PROJECT_NICKNAME,@USE_YEAR,@OWNER_DEP,@START_ID,@CREATE_DATE,@NOTE,@FM7_SUBJECT,@ACC_CATEGORY,@GAD_REVIEW) ";

                    dbFun.DoTran(strSQL, parameterA);

                    #endregion
                }

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ITEM_NAME = "Subject",
                    ITEM_VALUE = FM7Subject
                };

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = IDENTIFY,
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };

                    formRepository.PutFormDraftList(draftList, true);
                }

                #endregion

                #region - 送出表單：FormAutoStart -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(0))
                {
                    #region 送出表單前，先刪除草稿清單

                    //刪除草稿清單
                    FormDraftList draftList = new FormDraftList()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        IDENTIFY = IDENTIFY,
                        FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID
                    };
                    //PutFormDraftList(參數, 是否再新增)
                    formRepository.PutFormDraftList(draftList, false);

                    #endregion
                    //送出表單
                    FormAutoStart autoStart = new FormAutoStart()
                    {
                        REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                        DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID,
                        APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID,
                        APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT
                    };

                    formRepository.PutFormAutoStart(autoStart);
                }

                #endregion

                #region - 表單機能啟用：BPMFormFunction -

                var BPM_FormFunction = new BPMFormFunction()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
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
                CommLib.Logger.Error("專案建立審核單(新增/修改/草稿)失敗，原因：" + ex.Message);
                throw;
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
        /// 表單代號
        /// </summary>
        private string IDENTIFY = "ProjectReview";

        /// <summary>
        /// 表單主旨
        /// </summary>
        private string FM7Subject;

        #endregion
    }
}