using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

using Microsoft.International.Formatters;
using System.Collections;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 內容評估表
    /// </summary>
    public class EvaluateContentRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 內容評估表(查詢)
        /// </summary>
        public EvaluateContentViewModel PostEvaluateContentSingle(EvaluateContentQueryModel query)
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

            #region - 內容評估表 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [EvaluateCategory] AS [EVALUATE_CATEGORY], ";
            strSQL += "     [SortNo] AS [SORT_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentTitle = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentTitle>().FirstOrDefault();

            #endregion

            #region - 內容評估表 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [OriginalTitle] AS [ORIGINAL_TITLE], ";
            strSQL += "     [UsuallyTitle] AS [USUALLY_TITLE], ";
            strSQL += "     [TranslateTitle] AS [TRANSLATE_TITLE], ";
            strSQL += "     [CountryName] AS [COUNTRY_NAME], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [MediaAttribute] AS [MEDIA_ATTRIBUTE], ";
            strSQL += "     [Category] AS [CATEGORY], ";
            strSQL += "     [MediaLength] AS [MEDIA_LENGTH], ";
            strSQL += "     [EvaluateEpisode] AS [EVALUATE_EPISODE], ";
            strSQL += "     [EpisodeTotal] AS [EPISODE_TOTAL], ";
            strSQL += "     [ProvideUnit] AS [PROVIDE_UNIT], ";
            strSQL += "     [ProvideDate] AS [PROVIDE_DATE], ";
            strSQL += "     [ProducerUnit] AS [PRODUCER_UNIT], ";
            strSQL += "     [ProducerYear] AS [PRODUCER_YEAR], ";
            strSQL += "     [Subtitle] AS [SUBTITLE], ";
            strSQL += "     [MediaLang] AS [MEDIA_LANG], ";
            strSQL += "     [IsBilingual] AS [IS_BILINGUAL], ";
            strSQL += "     [AgeCategory] AS [AGE_CATEGORY], ";
            strSQL += "     [ViewGroup] AS [VIEW_GROUP], ";
            strSQL += "     [Director] AS [DIRECTOR], ";
            strSQL += "     [Screenwriter] AS [SCREENWRITER], ";
            strSQL += "     [Producer] AS [PRODUCER], ";
            strSQL += "     [E_Producer] AS [E_PRODUCER], ";
            strSQL += "     [Emcee] AS [EMCEE], ";
            strSQL += "     [Artists] AS [ARTISTS], ";
            strSQL += "     [Content] AS [CONTENT], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [IsSubmit] AS [IS_SUBMIT], ";
            strSQL += "     [EvaluateDate] AS [EVALUATE_DATE], ";
            strSQL += "     [PrincipalID] AS [PRINCIPAL_ID], ";
            strSQL += "     [PrincipalName] AS [PRINCIPAL_NAME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentConfig>().FirstOrDefault();

            #endregion

            #region - 內容評估表 評估人員 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [UserDeptMainID] AS [USER_DEPT_MAIN_ID], ";
            strSQL += "     [UserDeptID] AS [USER_DEPT_ID], ";
            strSQL += "     [UserID] AS [USER_ID], ";
            strSQL += "     [UserName] AS [USER_NAME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentUsersConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentUsersConfig>();

            #endregion

            #region - 內容評估表 評估意見彙整 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Advantage] AS [ADVANTAGE], ";
            strSQL += "     [Defect] AS [DEFECT], ";
            strSQL += "     [UserID] AS [USER_ID], ";
            strSQL += "     [UserName] AS [USER_NAME], ";
            strSQL += "     [AdviseType] AS [ADVISE_TYPE], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [OpinionDateTime] AS [OPINION_DATE_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_EVA] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentEvaluatesConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentEvaluatesConfig>();

            #endregion

            #region - 內容評估表 決策意見彙整 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [UserID] AS [USER_ID], ";
            strSQL += "     [UserName] AS [USER_NAME], ";
            strSQL += "     [AdviseType] AS [ADVISE_TYPE], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [OpinionDateTime] AS [OPINION_DATE_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_DEC] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentDecisionsConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentDecisionsConfig>();

            #endregion

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            #region - 內容評估表 附件 -

            var attachment = commonRepository.PostAttachment(formQueryModel);

            #endregion

            var evaluateContentViewModel = new EvaluateContentViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                EVALUATE_CONTENT_TITLE = evaluateContentTitle,
                EVALUATE_CONTENT_CONFIG = evaluateContentConfig,
                EVALUATE_CONTENT_USERS_CONFIG = evaluateContentUsersConfig,
                EVALUATE_CONTENT_EVAS_CONFIG = evaluateContentEvaluatesConfig,
                EVALUATE_CONTENT_DECS_CONFIG = evaluateContentDecisionsConfig,
                ATTACHMENT_CONFIG = attachment
            };

            #region - 確認表單 -

            if (evaluateContentViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    evaluateContentViewModel = new EvaluateContentViewModel();
                    CommLib.Logger.Error("內容評估表(查詢)失敗，原因：系統無正常起單。");
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

                    if (String.IsNullOrEmpty(evaluateContentViewModel.EVALUATE_CONTENT_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(evaluateContentViewModel.EVALUATE_CONTENT_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) evaluateContentViewModel.EVALUATE_CONTENT_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }                
            }

            #endregion

            return evaluateContentViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 內容評估表(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutEvaluateContentRefill(EvaluateContentQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("內容評估表(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 內容評估表(新增/修改/草稿)
        /// </summary>
        public bool PutEvaluateContentSingle(EvaluateContentViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                string strSortNo = null;
                int SortNo;

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 主旨 -

                FM7Subject = model.EVALUATE_CONTENT_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    if (!String.IsNullOrEmpty(model.EVALUATE_CONTENT_TITLE.SORT_NO) || !String.IsNullOrWhiteSpace(model.EVALUATE_CONTENT_TITLE.SORT_NO))
                    {

                        if (int.TryParse(model.EVALUATE_CONTENT_TITLE.SORT_NO, out SortNo))
                        {
                            if (SortNo != 0)
                            {
                                strSortNo = EastAsiaNumericFormatter.FormatWithCulture("Ln", SortNo, null, new CultureInfo("zh-TW"));

                                if (SortNo == 1)
                                {
                                    strSortNo = "初";
                                }
                                strSortNo += "評";

                                FM7Subject = "【" + model.EVALUATE_CONTENT_CONFIG.ORIGINAL_TITLE + "】：" + strSortNo;
                            }
                        }
                    }
                }
                else
                {
                    strSortNo = model.EVALUATE_CONTENT_TITLE.SORT_NO;
                }

                #endregion

                #endregion

                #region - 內容評估表 表頭資訊：EvaluateContent_M -

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
                    //內容評估表 表頭
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.EVALUATE_CONTENT_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@EVALUATE_CATEGORY", SqlDbType.NVarChar) { Size = 5, Value = (object)model.EVALUATE_CONTENT_TITLE.EVALUATE_CATEGORY ?? DBNull.Value },
                    new SqlParameter("@SORT_NO", SqlDbType.NVarChar) { Size = 50, Value = (object)strSortNo ?? DBNull.Value },
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
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
                    strSQL += "     [FormNo]=@FORM_NO, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "     [EvaluateCategory]=@EVALUATE_CATEGORY, ";
                    strSQL += "     [SortNo]=@SORT_NO ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_EvaluateContent_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FormNo],[FM7Subject],[EvaluateCategory],[SortNo]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FORM_NO,@FM7_SUBJECT,@EVALUATE_CATEGORY,@SORT_NO) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 內容評估表 表單內容：EvaluateContent_M -

                if (model.EVALUATE_CONTENT_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //內容評估表 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@ORIGINAL_TITLE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@USUALLY_TITLE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TRANSLATE_TITLE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COUNTRY_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ATTRIBUTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CATEGORY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_LENGTH", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EVALUATE_EPISODE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EPISODE_TOTAL", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROVIDE_UNIT", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PROVIDE_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRODUCER_UNIT", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRODUCER_YEAR", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUBTITLE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_LANG", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_BILINGUAL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AGE_CATEGORY", SqlDbType.NVarChar) { Size = 80, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@VIEW_GROUP", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DIRECTOR", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SCREENWRITER", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRODUCER", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@E_PRODUCER", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EMCEE", SqlDbType.NVarChar) { Size = 80, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ARTISTS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTENT", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    //寫入：內容評估表 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.EVALUATE_CONTENT_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
                    strSQL += "SET [OriginalTitle]=@ORIGINAL_TITLE, ";
                    strSQL += "     [UsuallyTitle]=@USUALLY_TITLE, ";
                    strSQL += "     [TranslateTitle]=@TRANSLATE_TITLE, ";
                    strSQL += "     [CountryName]=@COUNTRY_NAME, ";
                    strSQL += "     [MediaType]=@MEDIA_TYPE, ";
                    strSQL += "     [MediaAttribute]=@MEDIA_ATTRIBUTE, ";
                    strSQL += "     [Category]=@CATEGORY, ";
                    strSQL += "     [MediaLength]=@MEDIA_LENGTH, ";
                    strSQL += "     [EvaluateEpisode]=@EVALUATE_EPISODE, ";
                    strSQL += "     [EpisodeTotal]=@EPISODE_TOTAL, ";
                    strSQL += "     [ProvideUnit]=@PROVIDE_UNIT, ";
                    strSQL += "     [ProvideDate]=@PROVIDE_DATE, ";
                    strSQL += "     [ProducerUnit]=@PRODUCER_UNIT, ";
                    strSQL += "     [ProducerYear]=@PRODUCER_YEAR, ";
                    strSQL += "     [Subtitle]=@SUBTITLE, ";
                    strSQL += "     [MediaLang]=@MEDIA_LANG, ";
                    strSQL += "     [IsBilingual]=@IS_BILINGUAL, ";
                    strSQL += "     [AgeCategory]=@AGE_CATEGORY, ";
                    strSQL += "     [ViewGroup]=@VIEW_GROUP, ";
                    strSQL += "     [Director]=@DIRECTOR, ";
                    strSQL += "     [Screenwriter]=@SCREENWRITER, ";
                    strSQL += "     [Producer]=@PRODUCER, ";
                    strSQL += "     [E_Producer]=@E_PRODUCER, ";
                    strSQL += "     [Emcee]=@EMCEE, ";
                    strSQL += "     [Artists]=@ARTISTS, ";
                    strSQL += "     [Content]=@CONTENT, ";
                    strSQL += "     [Note]=@NOTE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 內容評估表 附件：Attachment -

                if (model.ATTACHMENT_CONFIG != null && model.ATTACHMENT_CONFIG.Count > 0)
                {
                    var attachmentMain = new AttachmentMain()
                    {
                        REQUISITION_ID = strREQ,
                        IDENTIFY = IDENTIFY,
                        ATTACHMENT = model.ATTACHMENT_CONFIG
                    };

                    commonRepository.PutAttachment(attachmentMain);
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
                CommLib.Logger.Error("內容評估表(新增/修改/草稿)失敗，原因：" + ex.Message);

            }

            return vResult;
        }

        /// <summary>
        /// 內容評估表(填寫)
        /// </summary>
        public bool PutEvaluateContentFillinSingle(EvaluateContentFillinConfig model)
        {
            bool vResult = false;
            try
            {
                #region - 內容評估表 表單內容：EvaluateContent_M -

                if (model.EVALUATE_CONTENT_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //內容評估表 填寫
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                        new SqlParameter("@IS_SUBMIT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EVALUATE_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRINCIPAL_ID", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PRINCIPAL_NAME", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    //寫入：內容評估表 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.EVALUATE_CONTENT_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);


                    strSQL = "";
                    strSQL += "SELECT [EvaluateCategory] AS [EVALUATE_CATEGORY] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    var dt = dbFun.DoQuery(strSQL, parameterInfo);

                    var EvaluateCategory = dt.Rows[0]["EVALUATE_CATEGORY"].ToString();

                    //是否外購
                    if (EvaluateCategory == "PUR")
                    {
                        //是的話就將【繼續上呈】強制Null。
                        parameterInfo.Where(I => I.ParameterName == "@IS_SUBMIT").FirstOrDefault().Value = (object)DBNull.Value;
                    }

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateContent_M] ";
                    strSQL += "SET [EvaluateDate]=@EVALUATE_DATE, ";
                    strSQL += "     [IsSubmit]=@IS_SUBMIT, ";
                    strSQL += "     [PrincipalID]=@PRINCIPAL_ID, ";
                    strSQL += "     [PrincipalName]=@PRINCIPAL_NAME ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 內容評估表 評估人員: EvaluateContent_D -

                var parameterUsers = new List<SqlParameter>()
                {
                    //內容評估表 評估人員
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@USER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_D] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterUsers);

                #endregion

                if (model.EVALUATE_CONTENT_USERS_CONFIG != null && model.EVALUATE_CONTENT_USERS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.EVALUATE_CONTENT_USERS_CONFIG)
                    {
                        //寫入：內容評估表 評估人員parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterUsers);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_EvaluateContent_D]([RequisitionID],[UserDeptID],[UserID],[UserName]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@USER_DEPT_ID,@User_ID,@USER_NAME) ";

                        dbFun.DoTran(strSQL, parameterUsers);
                    }

                    #endregion
                }

                #endregion

                vResult = true;

            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("內容評估表(評估意見)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 內容評估表(評估意見)
        /// </summary>
        public bool PutEvaluateContentOpinionSingle(EvaluateContentOpinionConfig model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                string insertOpinionKey = null;
                string insertOpinionValue = null;

                #endregion

                #region - 內容評估表 評估意見:EvaluateContentOpinion -

                var parameterOpinion = new List<SqlParameter>()
                {
                    //內容評估表 評估意見
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@USER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ADVANTAGE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DEFECT", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ADVISE_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (!String.IsNullOrEmpty(model.REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.REQUISITION_ID))
                {
                    //寫入：內容評估表 評估意見parameter
                    strJson = jsonFunction.ObjectToJSON(model);
                    GlobalParameters.Infoparameter(strJson, parameterOpinion);

                    #region 先刪除舊資料

                    strSQL = "";
                    strSQL += "DELETE ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContent_" + model.OPINION_TYPE + "] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "          AND [UserID]=@USER_ID ";
                    strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterOpinion);

                    #endregion

                    #region - 添加意見資料 -

                    if (model.OPINION_TYPE == "EVA")
                    {
                        insertOpinionKey = "[Advantage],[Defect],";
                        insertOpinionValue = "@ADVANTAGE,@DEFECT,";
                    }

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_EvaluateContent_" + model.OPINION_TYPE + "]([RequisitionID],[UserID],[UserName]," + insertOpinionKey + "[AdviseType],[Reason],[OpinionDateTime]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@USER_ID,@USER_NAME," + insertOpinionValue + "@ADVISE_TYPE,@REASON,GETDATE()) ";

                    dbFun.DoTran(strSQL, parameterOpinion);

                    #endregion
                }

                #endregion

                vResult = true;

            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("內容評估表(評估意見)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 內容評估表(清除評估人員)
        /// </summary>
        public bool PutEvaluateContentRemoveCountersignSingle(EvaluateContentQueryModel query)
        {
            bool vResult = false;
            try
            {
                #region - 內容評估表 評估意見:EvaluateContentRemoveCountersign -

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                //清空  評估人員列。
                strSQL = "";
                strSQL += "DELETE [BPMPro].[dbo].[FM7T_EvaluateContent_D] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "			AND [RequisitionID]=@REQUISITION_ID ";
                dbFun.DoTran(strSQL, parameter);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("內容評估表(清除評估人員)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "EvaluateContent";

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