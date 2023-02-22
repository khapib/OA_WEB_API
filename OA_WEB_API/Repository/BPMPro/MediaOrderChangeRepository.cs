using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購異動申請單
    /// </summary>
    public class MediaOrderChangeRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();

        #endregion        

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購異動申請單(查詢)
        /// </summary> 
        public MediaOrderChangeViewModel PostMediaOrderChangeSingle(MediaOrderChangeQueryModel query)
        {
            var parameter = new List<SqlParameter>()
            {
                 new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
            };

            #region - 申請人資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [DiagramID] AS [DIAGRAM_ID], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     [ApplicantDeptName] AS [APPLICANT_DEPT_NAME], ";
            strSQL += "     [ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     [ApplicantName] AS [APPLICANT_NAME], ";
            strSQL += "     [ApplicantPhone] AS [APPLICANT_PHONE], ";
            strSQL += "     [ApplicantDateTime] AS [APPLICANT_DATETIME], ";
            strSQL += "     [FillerID] AS [FILLER_ID], ";
            strSQL += "     [FillerName] AS [FILLER_NAME], ";
            strSQL += "     [Priority] AS [PRIORITY], ";
            strSQL += "     [DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     [FlowActivated] AS [FLOW_ACTIVATED] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderChange_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 版權購異動申請單 表頭資訊及設定內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [PYMT_LockPeriod] AS [PYMT_LOCK_PERIOD], ";
            strSQL += "     [GroupID] AS [GROUP_ID], ";
            strSQL += "     [GroupBPMFormNo] AS [GROUP_BPM_FORM_NO], ";
            strSQL += "     [GroupPath] AS [GROUP_PATH], ";
            strSQL += "     [FormAction] AS [FORM_ACTION], ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [ModifyFormNo] AS [MODIFY_FORM_NO], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [ChangeDescription] AS [CHANGE_DESCRIPTION] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderChange_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaOrderChangeConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaOrderChangeConfig>().FirstOrDefault();

            #endregion

            #region - 版權購異動申請單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var mediaOrderChangeViewModel = new MediaOrderChangeViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_ORDER_CHANGE_CONFIG = mediaOrderChangeConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            return mediaOrderChangeViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權購異動申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaOrderChangeRefill(MediaOrderChangeQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權購異動申請單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權採購異動申請單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaOrderChangeSingle(MediaOrderChangeViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                #region - 主旨 -

                FM7Subject = model.MEDIA_ORDER_CHANGE_CONFIG.FM7_SUBJECT;

                if (FM7Subject.Substring(1, 2) != "異動")
                {
                    FM7Subject = "【異動】" + FM7Subject;
                }

                #endregion

                #region - 表單路徑 -

                if (String.IsNullOrEmpty(model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_PATH) || String.IsNullOrWhiteSpace(model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_PATH))
                {
                    var mediaOrderformQueryModel = new FormQueryModel()
                    {
                        REQUISITION_ID = model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_ID
                    };
                    var mediaOrderformData = formRepository.PostFormData(mediaOrderformQueryModel);

                    //表單路徑
                    model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_PATH = GlobalParameters.FormContentPath(mediaOrderformData.REQUISITION_ID, mediaOrderformData.IDENTIFY, mediaOrderformData.DIAGRAM_NAME);

                }

                #endregion

                #endregion

                #region - 版權採購異動申請單 表頭資訊及設定內容：MediaOrderChange_M -

                var parameterTitle = new List<SqlParameter>()
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
                    //版權購異動申請單 表頭資訊及設定內容
                    new SqlParameter("@PYMT_LOCK_PERIOD", SqlDbType.NVarChar) { Size = 4000, Value = model.MEDIA_ORDER_CHANGE_CONFIG.PYMT_LOCK_PERIOD ?? String.Empty },
                    new SqlParameter("@GROUP_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_ID ?? DBNull.Value },
                    new SqlParameter("@GROUP_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@GROUP_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)model.MEDIA_ORDER_CHANGE_CONFIG.GROUP_PATH ?? DBNull.Value },
                    new SqlParameter("@FORM_ACTION", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_ORDER_CHANGE_CONFIG.FORM_ACTION ?? String.Empty },
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_CHANGE_CONFIG.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@MODIFY_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_CHANGE_CONFIG.MODIFY_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ORDER_CHANGE_CONFIG.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@CHANGE_DESCRIPTION", SqlDbType.NVarChar) { Size = 64, Value = model.MEDIA_ORDER_CHANGE_CONFIG.CHANGE_DESCRIPTION ?? String.Empty },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaOrderChange_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaOrderChange_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";
                    strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";
                    strSQL += "     [FillerID]=@FILLER_ID, ";
                    strSQL += "     [FillerName]=@FILLER_NAME, ";
                    strSQL += "     [Priority]=@PRIORITY, ";
                    strSQL += "     [DraftFlag]=@DRAFT_FLAG, ";
                    strSQL += "     [FlowActivated]=@FLOW_ACTIVATED, ";
                    strSQL += "     [PYMT_LockPeriod]=@PYMT_LOCK_PERIOD, ";
                    strSQL += "     [GroupID]=@GROUP_ID, ";
                    strSQL += "     [GroupBPMFormNo]=@GROUP_BPM_FORM_NO, ";
                    strSQL += "     [GroupPath]=@GROUP_PATH, ";
                    strSQL += "     [FormAction]=@FORM_ACTION, ";
                    strSQL += "     [FlowName]=@FLOW_NAME, ";
                    strSQL += "     [ModifyFormNo]=@MODIFY_FORM_NO, ";
                    strSQL += "     [FormNo]=@FORM_NO, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "     [ChangeDescription]=@CHANGE_DESCRIPTION ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaOrderChange_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[PYMT_LockPeriod],[GroupID],[GroupBPMFormNo],[GroupPath],[FormAction],[FlowName],[ModifyFormNo],[FormNo],[FM7Subject],[ChangeDescription]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@PYMT_LOCK_PERIOD,@GROUP_ID,@GROUP_BPM_FORM_NO,@GROUP_PATH,@FORM_ACTION,@FLOW_NAME,@MODIFY_FORM_NO,@FORM_NO,@FM7_SUBJECT,@CHANGE_DESCRIPTION) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 版權採購異動申請單 表單關聯：AssociatedForm -

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ASSOCIATED_FORM_CONFIG = model.ASSOCIATED_FORM_CONFIG
                };

                commonRepository.PutAssociatedForm(associatedFormModel);

                #endregion

                #region - 表單主旨：FormHeader -

                FormHeader header = new FormHeader();
                header.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                header.ITEM_NAME = "Subject";
                header.ITEM_VALUE = FM7Subject;

                formRepository.PutFormHeader(header);

                #endregion

                #region - 儲存草稿：FormDraftList -

                if (model.APPLICANT_INFO.DRAFT_FLAG.Equals(1))
                {
                    FormDraftList draftList = new FormDraftList();
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
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
                    draftList.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    draftList.IDENTIFY = IDENTIFY;
                    draftList.FILLER_ID = model.APPLICANT_INFO.APPLICANT_ID;

                    formRepository.PutFormDraftList(draftList, false);

                    #endregion

                    FormAutoStart autoStart = new FormAutoStart();
                    autoStart.REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID;
                    autoStart.DIAGRAM_ID = model.APPLICANT_INFO.DIAGRAM_ID;
                    autoStart.APPLICANT_ID = model.APPLICANT_INFO.APPLICANT_ID;
                    autoStart.APPLICANT_DEPT = model.APPLICANT_INFO.APPLICANT_DEPT;

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
                CommLib.Logger.Error("版權採購異動申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaOrderChange";

        /// <summary>
        /// 表單主旨
        /// </summary>
        private string FM7Subject;

        /// <summary>
        /// Json字串
        /// </summary>
        private string strJson;

        #endregion

    }
}