using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

using Microsoft.International.Formatters;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 內容評估表_補充意見
    /// </summary>
    public class EvaluateContentReplenishRepository
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
        /// 內容評估表_補充意見(查詢)
        /// </summary>
        public EvaluateContentReplenishViewModel PostEvaluateContentReplenishSingle(EvaluateContentReplenishQueryModel query)
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

            #region - 內容評估表_補充意見 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [EvaluateCategory] AS [EVALUATE_CATEGORY], ";
            strSQL += "     [SortNo] AS [SORT_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentReplenishTitle = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentReplenishTitle>().FirstOrDefault();

            #endregion

            #region - 內容評估表_補充意見 表單內容 -

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
            strSQL += "     [PrincipalID] AS [PRINCIPAL_ID], ";
            strSQL += "     [PrincipalName] AS [PRINCIPAL_NAME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentReplenishConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentReplenishConfig>().FirstOrDefault();

            #endregion

            #region - 內容評估表_補充意見 評估意見彙整 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [UserID] AS [USER_ID], ";
            strSQL += "     [UserName] AS [USER_NAME], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [OpinionDateTime] AS [OPINION_DATE_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_EVA] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentReplenishEvaluatesConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentReplenishEvaluatesConfig>();

            #endregion

            #region - 內容評估表_補充意見 決策意見彙整 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [UserID] AS [USER_ID], ";
            strSQL += "     [UserName] AS [USER_NAME], ";
            strSQL += "     [AdviseType] AS [ADVISE_TYPE], ";
            strSQL += "     [Reason] AS [REASON], ";
            strSQL += "     [OpinionDateTime] AS [OPINION_DATE_TIME] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_DEC] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var evaluateContentReplenishDecisionsConfig = dbFun.DoQuery(strSQL, parameter).ToList<EvaluateContentReplenishDecisionsConfig>();

            #endregion

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            #region - 內容評估表_補充意見 附件 -

            var attachment = commonRepository.PostAttachment(formQueryModel);

            #endregion

            #region - 內容評估表_補充意見 表單關聯 -

            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var evaluateContentReplenishViewModel = new EvaluateContentReplenishViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                EVALUATE_CONTENT_REPLENISH_TITLE = evaluateContentReplenishTitle,
                EVALUATE_CONTENT_REPLENISH_CONFIG = evaluateContentReplenishConfig,
                EVALUATE_CONTENT_REPLENISH_EVAS_CONFIG = evaluateContentReplenishEvaluatesConfig,
                EVALUATE_CONTENT_REPLENISH_DECS_CONFIG = evaluateContentReplenishDecisionsConfig,
                ATTACHMENT_CONFIG = attachment,
                ASSOCIATED_FORM_CONFIG = associatedForm,
            };

            #region - 確認表單 -

            if (evaluateContentReplenishViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                #region - 確認BPM表單是否正常起單到系統中 -

                //保留原有資料
                strJson = jsonFunction.ObjectToJSON(evaluateContentReplenishViewModel);

                var BpmSystemOrder = new BPMSystemOrder()
                {
                    REQUISITION_ID = query.REQUISITION_ID,
                    IDENTIFY = IDENTIFY,
                    EXTS = new List<string>()
                    {
                        "M",
                        "D",
                        "EVA",
                        "DEC"
                    },
                    IS_ASSOCIATED_FORM = false
                };
                //確認是否有正常到系統起單；清除失敗表單資料並重新送單值行
                if (commonRepository.PostBPMSystemOrder(BpmSystemOrder)) PutEvaluateContentReplenishSingle(jsonFunction.JsonToObject<EvaluateContentReplenishViewModel>(strJson));

                #endregion

                #region - 確認M表BPM表單單號 -

                //避免儲存後送出表單BPM表單單號沒寫入的情形
                var formQuery = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                notifyRepository.ByInsertBPMFormNo(formQuery);

                if (String.IsNullOrEmpty(evaluateContentReplenishViewModel.EVALUATE_CONTENT_REPLENISH_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(evaluateContentReplenishViewModel.EVALUATE_CONTENT_REPLENISH_TITLE.BPM_FORM_NO))
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                    var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                    if (dtBpmFormNo.Rows.Count > 0) evaluateContentReplenishViewModel.EVALUATE_CONTENT_REPLENISH_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                }

                #endregion
            }

            #endregion

            return evaluateContentReplenishViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 內容評估表_補充意見(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutEvaluateContentReplenishRefill(EvaluateContentReplenishQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("內容評估表_補充意見(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 內容評估表_補充意見(新增/修改/草稿)
        /// </summary>
        public bool PutEvaluateContentReplenishSingle(EvaluateContentReplenishViewModel model)
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

                string strSortNo = null;
                int SortNo;

                #region - 主旨 -

                FM7Subject = model.EVALUATE_CONTENT_REPLENISH_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    if (!String.IsNullOrEmpty(model.EVALUATE_CONTENT_REPLENISH_TITLE.SORT_NO) || !String.IsNullOrWhiteSpace(model.EVALUATE_CONTENT_REPLENISH_TITLE.SORT_NO))
                    {

                        if (int.TryParse(model.EVALUATE_CONTENT_REPLENISH_TITLE.SORT_NO, out SortNo))
                        {
                            if (SortNo != 0)
                            {
                                strSortNo = EastAsiaNumericFormatter.FormatWithCulture("Ln", SortNo, null, new CultureInfo("zh-TW"));

                                strSortNo = "補充意見" + strSortNo;

                                FM7Subject = "【" + model.EVALUATE_CONTENT_REPLENISH_CONFIG.ORIGINAL_TITLE + "】：" + strSortNo;
                            }
                        }
                    }
                }
                else
                {
                    strSortNo = model.EVALUATE_CONTENT_REPLENISH_TITLE.SORT_NO;
                }

                #endregion

                #endregion

                #region - 內容評估表_補充意見 表頭資訊：EvaluateContentReplenish_M -

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
                    //內容評估表_補充意見 表頭
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.EVALUATE_CONTENT_REPLENISH_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@EVALUATE_CATEGORY", SqlDbType.NVarChar) { Size = 200, Value = model.EVALUATE_CONTENT_REPLENISH_TITLE.EVALUATE_CATEGORY ?? String.Empty },
                    new SqlParameter("@SORT_NO", SqlDbType.NVarChar) { Size = 50, Value = (object)strSortNo ?? DBNull.Value },
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    if (CommonRepository.GetFSe7enSysRequisition().Where(R => R.REQUISITION_ID == strREQ).Count() <= 0)
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M] ";
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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FormNo],[FM7Subject],[EvaluateCategory],[SortNo]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FORM_NO,@FM7_SUBJECT,@EVALUATE_CATEGORY,@SORT_NO) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 內容評估表_補充意見 表單內容：EvaluateContentReplenish_M -

                if (model.EVALUATE_CONTENT_REPLENISH_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //內容評估表_補充意見 表單內容
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

                    //寫入：內容評估表_補充意見 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.EVALUATE_CONTENT_REPLENISH_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M] ";
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

                #region - 內容評估表_補充意見 附件：Attachment -

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

                #region - 內容評估表_補充意見 表單關聯：AssociatedForm -

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = strREQ,
                    ASSOCIATED_FORM_CONFIG = model.ASSOCIATED_FORM_CONFIG
                };

                commonRepository.PutAssociatedForm(associatedFormModel);

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
                CommLib.Logger.Error("內容評估表_補充意見(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;

        }

        /// <summary>
        /// 內容評估表_補充意見(填寫)
        /// </summary>
        public bool PutEvaluateContentReplenishFillinSingle(EvaluateContentReplenishFillinConfig model)
        {
            bool vResult = false;
            try
            {
                #region - 內容評估表_補充意見 表單內容：EvaluateContentReplenish_M -

                var parameterInfo = new List<SqlParameter>()
                {
                    //內容評估表_補充意見 填寫
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@PRINCIPAL_ID", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@PRINCIPAL_NAME", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                //寫入：內容評估表_補充意見 表單內容parameter                        
                strJson = jsonFunction.ObjectToJSON(model);
                GlobalParameters.Infoparameter(strJson, parameterInfo);

                strSQL = "";
                strSQL += "UPDATE [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_M] ";
                strSQL += "SET [PrincipalID]=@PRINCIPAL_ID, ";
                strSQL += "     [PrincipalName]=@PRINCIPAL_NAME ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterInfo);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("內容評估表_補充意見(填寫)失敗，原因：" + ex.Message);
            }

            return vResult;

        }

        /// <summary>
        /// 內容評估表_補充意見(意見)
        /// </summary>
        public bool PutEvaluateContentReplenishOpinionSingle(EvaluateContentReplenishOpinionConfig model)
        {
            bool vResult = false;
            try
            {
                #region - 判斷是否寫入意見 -

                bool OpinionToken = false;

                if (model.OPINION_TYPE == "DEC")
                {
                    OpinionToken = true;
                }
                else
                {
                    if (!String.IsNullOrEmpty(model.REASON) || !String.IsNullOrWhiteSpace(model.REASON))
                    {
                        OpinionToken = true;
                    }
                    else
                    {
                        OpinionToken = false;
                    }
                }

                #endregion

                if (OpinionToken)
                {
                    #region - 宣告 -

                    string insertOpinionKey = null;
                    string insertOpinionValue = null;

                    #endregion

                    #region - 內容評估表_補充意見 評估意見:EvaluateContentReplenishOpinion -

                    var parameterOpinion = new List<SqlParameter>()
                    {
                        //內容評估表_補充意見 評估意見
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                        new SqlParameter("@USER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@USER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ADVISE_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REASON", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    if (!String.IsNullOrEmpty(model.REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.REQUISITION_ID))
                    {
                        //寫入：內容評估表_補充意見 評估意見parameter
                        strJson = jsonFunction.ObjectToJSON(model);
                        GlobalParameters.Infoparameter(strJson, parameterOpinion);

                        #region 先刪除舊資料

                        strSQL = "";
                        strSQL += "DELETE ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_" + model.OPINION_TYPE + "] ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "          AND [UserID]=@USER_ID ";
                        strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                        dbFun.DoTran(strSQL, parameterOpinion);

                        #endregion

                        #region - 添加意見資料 -

                        if (model.OPINION_TYPE == "DEC")
                        {
                            insertOpinionKey = "[AdviseType],";
                            insertOpinionValue = "@ADVISE_TYPE,";
                        }

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_EvaluateContentReplenish_" + model.OPINION_TYPE + "]([RequisitionID],[UserID],[UserName]," + insertOpinionKey + "[Reason],[OpinionDateTime]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@USER_ID,@USER_NAME," + insertOpinionValue + "@REASON,GETDATE()) ";

                        dbFun.DoTran(strSQL, parameterOpinion);

                        #endregion
                    }

                    #endregion
                }

                vResult = true;

            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("內容評估表_補充意見(評估意見)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "EvaluateContentReplenish";

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