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
    /// 會簽管理系統 - 拷貝申請單
    /// </summary>
    public class MediaWarehouseCopyRepository
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
        /// 拷貝申請單(查詢)
        /// </summary>
        public MediaWarehouseCopyViewModel PostMediaWarehouseCopySingle(MediaWarehouseCopyQueryModel query)
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - 拷貝申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [OA_MasterNo] AS [OA_MASTER_NO], ";
            strSQL += "     [OA_FormNo] AS [OA_FORM_NO], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaWarehouseCopyTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseCopyTitle>().FirstOrDefault();

            #endregion

            #region - 拷貝申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [Contact] AS [CONTACT], ";
            strSQL += "     [ApprovalNo] AS [APPROVAL_NO], ";
            strSQL += "     [ExpectedDate] AS [EXPECTED_DATE], ";
            strSQL += "     [ContactPerson] AS [CONTACT_PERSON], ";
            strSQL += "     [ApplyType] AS [APPLY_TYPE], ";
            strSQL += "     [ApplyTypeOthers] AS [APPLY_TYPE_OTHERS], ";
            strSQL += "     [SaveType] AS [SAVE_TYPE], ";
            strSQL += "     [SaveTypeOthers] AS [SAVE_TYPE_OTHERS], ";
            strSQL += "     [SavePath] AS [SAVE_PATH], ";
            strSQL += "     [UpperLeftLogo] AS [UPPER_LEFT_LOGO], ";
            strSQL += "     [Subtitles] AS [SUBTITLES], ";
            strSQL += "     [Breach] AS [BREACH], ";
            strSQL += "     [Effect] AS [EFFECT], ";
            strSQL += "     [TimeCode] AS [TIME_CODE], ";
            strSQL += "     [MediaPossession] AS [MEDIA_POSSESSION], ";
            strSQL += "     [CastCrew] AS [CAST_CREW], ";
            strSQL += "     [Lyrics] AS [LYRICS], ";
            strSQL += "     [OpeningLogo] AS [OPENING_LOGO], ";
            strSQL += "     [Notice] AS [NOTICE], ";
            strSQL += "     [EndCard] AS [END_CARD], ";
            strSQL += "     [CopyDemandOther] AS [COPY_DEMAND_OTHER], ";
            strSQL += "     [Stereo] AS [STEREO], ";
            strSQL += "     [Loudness] AS [LOUDNESS], ";
            strSQL += "     [ChapterNote] AS [CHAPTER_NOTE], ";
            strSQL += "     [VideoCodec] AS [VIDEO_CODEC], ";
            strSQL += "     [FraneRate] AS [FRANE_RATE], ";
            strSQL += "     [Birate] AS [BIRATE], ";
            strSQL += "     [Resolution] AS [RESOLUTION], ";
            strSQL += "     [AudioCodeo] AS [AUDIO_CODEO], ";
            strSQL += "     [ConversionNote] AS [CONVERSION_NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaWarehouseCopyConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseCopyConfig>().FirstOrDefault();

            #endregion

            #region - 拷貝申請單 拷貝明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ProgramName] AS [PROGRAM_NAME], ";
            strSQL += "     [Volume] AS [VOLUME], ";
            strSQL += "     [MeterialType] AS [METERIAL_TYPE], ";
            strSQL += "     [MeterialTypeOthers] AS [METERIAL_TYPE_OTHERS], ";
            strSQL += "     [Length] AS [LENGTH], ";
            strSQL += "     [LengthOthers] AS [LENGTH_OTHERS], ";
            strSQL += "     [CopyType] AS [COPY_TYPE], ";
            strSQL += "     [CopyTypeOthers] AS [COPY_TYPE_OTHERS], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaWarehouseCopyDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseCopyDetailsConfig>();

            #endregion

            #region - 拷貝申請單 音軌規格 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ChapterNo] AS [CHAPTER_NO], ";
            strSQL += "     [Chapter] AS [CHAPTER] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_CH] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaWarehouseCopyChaptersConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseCopyChaptersConfig>();

            #endregion

            MediaWarehouseCopyViewModel mediaWarehouseCopyViewModel = new MediaWarehouseCopyViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_WAREHOUSE_COPY_TITLE = mediaWarehouseCopyTitle,
                MEDIA_WAREHOUSE_COPY_CONFIG = mediaWarehouseCopyConfig,
                MEDIA_WAREHOUSE_COPY_DTLS_CONFIG = mediaWarehouseCopyDetailsConfig,
                MEDIA_WAREHOUSE_COPY_CHS_CONFIG = mediaWarehouseCopyChaptersConfig,
            };

            return mediaWarehouseCopyViewModel;
        }

        #region - 依此單內容重送 -

        ///// <summary>
        ///// 拷貝申請單(依此單內容重送)(僅外部起單使用)
        ///// </summary>
        //public bool PutMediaWarehouseCopyRefill(MediaWarehouseCopyQueryModel query)
        //{
        //    bool vResult = false;

        //    try
        //    {
        //        #region - 宣告 -

        //        var original = PostMediaWarehouseCopySingle(query);
        //        strJson = jsonFunction.ObjectToJSON(original);

        //        var MediaWarehouseCopyViewModel = new MediaWarehouseCopyViewModel();

        //        var requisitionID = Guid.NewGuid().ToString();

        //        #endregion

        //        #region - 重送內容 -

        //        MediaWarehouseCopyViewModel = jsonFunction.JsonToObject<MediaWarehouseCopyViewModel>(strJson);

        //        #region - 申請人資訊 調整 -

        //        MediaWarehouseCopyViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
        //        MediaWarehouseCopyViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
        //        MediaWarehouseCopyViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

        //        #endregion

        //        #endregion

        //        #region - 送出 執行(新增/修改/草稿) -

        //        PutMediaWarehouseCopySingle(MediaWarehouseCopyViewModel);

        //        #endregion

        //        vResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("拷貝申請單(依此單內容重送)失敗，原因：" + ex.Message);
        //    }

        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 拷貝申請單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaWarehouseCopySingle(MediaWarehouseCopyViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                #region - 主旨 -

                FM7Subject = model.MEDIA_WAREHOUSE_COPY_TITLE.FM7_SUBJECT;

                #endregion

                #endregion

                #region - 拷貝申請單 表頭資訊：MediaWarehouseCopy_M -

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
                    //拷貝申請單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
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
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 拷貝申請單 表單內容：MediaWarehouseCopy_M -

                if (model.MEDIA_WAREHOUSE_COPY_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //拷貝申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACT", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPROVAL_NO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EXPECTED_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACT_PERSON", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        //拷貝申請單 拷貝類型
                        new SqlParameter("@APPLY_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@APPLY_TYPE_OTHERS", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SAVE_TYPE", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SAVE_TYPE_OTHERS", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SAVE_PATH", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        //拷貝申請單 拷貝需求
                        new SqlParameter("@UPPER_LEFT_LOGO", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUBTITLES", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BREACH", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@EFFECT", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@TIME_CODE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_POSSESSION", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CAST_CREW", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@LYRICS", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OPENING_LOGO", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTICE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@END_CARD", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@COPY_DEMAND_OTHER", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        //拷貝申請單 音軌規格
                        new SqlParameter("@STEREO", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@LOUDNESS", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CHAPTER_NOTE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        //拷貝申請單 轉檔規格
                        new SqlParameter("@VIDEO_CODEC", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FRANE_RATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@BIRATE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@RESOLUTION", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@AUDIO_CODEO", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONVERSION_NOTE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    //寫入：拷貝申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_WAREHOUSE_COPY_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [Note]=@NOTE, ";
                    strSQL += "     [Contact]=@CONTACT, ";
                    strSQL += "     [ApprovalNo]=@APPROVAL_NO, ";
                    strSQL += "     [ExpectedDate]=@EXPECTED_DATE, ";
                    strSQL += "     [ContactPerson]=@CONTACT_PERSON, ";
                    strSQL += "     [ApplyType]=@APPLY_TYPE, ";
                    strSQL += "     [ApplyTypeOthers]=@APPLY_TYPE_OTHERS, ";
                    strSQL += "     [SaveType]=@SAVE_TYPE, ";
                    strSQL += "     [SaveTypeOthers]=@SAVE_TYPE_OTHERS, ";
                    strSQL += "     [SavePath]=@SAVE_PATH, ";
                    strSQL += "     [UpperLeftLogo]=@UPPER_LEFT_LOGO, ";
                    strSQL += "     [Subtitles]=@SUBTITLES, ";
                    strSQL += "     [Breach]=@BREACH, ";
                    strSQL += "     [Effect]=@EFFECT, ";
                    strSQL += "     [TimeCode]=@TIME_CODE, ";
                    strSQL += "     [MediaPossession]=@MEDIA_POSSESSION, ";
                    strSQL += "     [CastCrew]=@CAST_CREW, ";
                    strSQL += "     [Lyrics]=@LYRICS, ";
                    strSQL += "     [OpeningLogo]=@OPENING_LOGO, ";
                    strSQL += "     [Notice]=@NOTICE, ";
                    strSQL += "     [EndCard]=@END_CARD, ";
                    strSQL += "     [CopyDemandOther]=@COPY_DEMAND_OTHER, ";
                    strSQL += "     [Stereo]=@STEREO, ";
                    strSQL += "     [Loudness]=@LOUDNESS, ";
                    strSQL += "     [ChapterNote]=@CHAPTER_NOTE, ";
                    strSQL += "     [VideoCodec]=@VIDEO_CODEC, ";
                    strSQL += "     [FraneRate]=@FRANE_RATE, ";
                    strSQL += "     [Birate]=@BIRATE, ";
                    strSQL += "     [Resolution]=@RESOLUTION, ";
                    strSQL += "     [AudioCodeo]=@AUDIO_CODEO, ";
                    strSQL += "     [ConversionNote]=@CONVERSION_NOTE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 拷貝申請單 拷貝明細：MediaWarehouseCopy_D -

                var parameterDetails = new List<SqlParameter>()
                {
                    //拷貝申請單 拷貝明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@PROGRAM_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@VOLUME", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@METERIAL_TYPE", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@METERIAL_TYPE_OTHERS", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLY_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLY_TYPE_OTHERS", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@LENGTH", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@LENGTH_OTHERS", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COPY_TYPE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@COPY_TYPE_OTHERS", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_D] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_WAREHOUSE_COPY_DTLS_CONFIG != null && model.MEDIA_WAREHOUSE_COPY_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_WAREHOUSE_COPY_DTLS_CONFIG)
                    {
                        //寫入：拷貝申請單 拷貝明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_D]([RequisitionID],[ProgramName],[Volume],[MeterialType],[MeterialTypeOthers],[Length],[LengthOthers],[CopyType],[CopyTypeOthers],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@PROGRAM_NAME,@VOLUME,@METERIAL_TYPE,@METERIAL_TYPE_OTHERS,@LENGTH,@LENGTH_OTHERS,@COPY_TYPE,@COPY_TYPE_OTHERS,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 拷貝申請單 音軌規格：MediaWarehouseCopy_CH -

                var parameterChapters = new List<SqlParameter>()
                {
                    //拷貝申請單 拷貝明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@CHAPTER_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@CHAPTER", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_CH] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_WAREHOUSE_COPY_CHS_CONFIG != null && model.MEDIA_WAREHOUSE_COPY_CHS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_WAREHOUSE_COPY_CHS_CONFIG)
                    {
                        //寫入：拷貝申請單 音軌規格parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterChapters);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_CH]([RequisitionID],[ChapterNo],[Chapter]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@CHAPTER_NO,@CHAPTER) ";

                        dbFun.DoTran(strSQL, parameterChapters);
                    }

                    #endregion
                }

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
                CommLib.Logger.Error("拷貝申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaWarehouseCopy";

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