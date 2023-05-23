using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using OA_WEB_API.Models.ERP;
using Newtonsoft.Json;
using System.Reflection;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購交片單
    /// </summary>
    public class MediaAcceptanceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #region FormRepository

        /// <summary>版權採購申請單</summary>
        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 版權採購交片單(查詢)
        /// </summary>
        public MediaAcceptanceViewModel PostMediaAcceptanceSingle(MediaAcceptanceQueryModel query)
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
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaAcceptance_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var applicantInfo = dbFun.DoQuery(strSQL, parameter).ToList<ApplicantInfo>().FirstOrDefault();

            #endregion

            #region - M表寫入BPM表單單號 -

            //避免儲存後送出表單BPM表單單號沒寫入的情形
            var formQuery = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            if (applicantInfo.DRAFT_FLAG == 0) notifyRepository.ByInsertBPMFormNo(formQuery);

            #endregion

            #region - 版權採購交片單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FlowName] AS [FLOW_NAME], ";
            strSQL += "     [FormNo] AS [FORM_NO], ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaAcceptance_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaAcceptanceTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaAcceptanceTitle>().FirstOrDefault();

            #endregion

            #region - 版權採購交片單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [MediaOrderRequisitionID] AS [MEDIA_ORDER_REQUISITION_ID], ";
            strSQL += "     [MediaOrderSubject] AS [MEDIA_ORDER_SUBJECT], ";
            strSQL += "     [MediaOrderBPMFormNo] AS [MEDIA_ORDER_BPM_FORM_NO], ";
            strSQL += "     [MediaOrderERPFormNo] AS [MEDIA_ORDER_ERP_FORM_NO], ";
            strSQL += "     [MediaOrderPath] AS [MEDIA_ORDER_PATH], ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [SupNo] AS [SUP_NO], ";
            strSQL += "     [SupName] AS [SUP_NAME], ";
            strSQL += "     [RegisterKind] AS [REG_KIND], ";
            strSQL += "     [RegisterNo] AS [REG_NO], ";
            strSQL += "     [OwnerName] AS [OWNER_NAME], ";
            strSQL += "     [OwnerTEL] AS [OWNER_TEL], ";
            strSQL += "     [IsFilmStorage] AS [IS_FILM_STORAGE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaAcceptance_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaAcceptanceConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaAcceptanceConfig>().FirstOrDefault();

            #endregion

            #region - 版權採購交片單 驗收明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Period] AS [PERIOD], ";
            strSQL += "     [SupProdANo] AS [SUP_PROD_A_NO], ";
            strSQL += "     [RowNo] AS [ROW_NO], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [MediaSpec] AS [MEDIA_SPEC], ";
            strSQL += "     [MediaType] AS [MEDIA_TYPE], ";
            strSQL += "     [StartEpisode] AS [START_EPISODE], ";
            strSQL += "     [EndEpisode] AS [END_EPISODE], ";
            strSQL += "     [OrderEpisode] AS [ORDER_EPISODE], ";
            strSQL += "     [ACPT_Episode] AS [ACPT_EPISODE], ";
            strSQL += "     [DismantleEpisode] AS [DISMANTLE_EPISODE], ";
            strSQL += "     [EpisodeTime] AS [EPISODE_TIME], ";
            strSQL += "     [GetMasteringDate] AS [GET_MASTERING_DATE], ";
            strSQL += "     [OwnerDeptMainID] AS [OWNER_DEPT_MAIN_ID], ";
            strSQL += "     [OwnerDeptID] AS [OWNER_DEPT_ID], ";
            strSQL += "     [OwnerID] AS [OWNER_ID], ";
            strSQL += "     [OwnerName] AS [OWNER_NAME], ";
            strSQL += "     [AcceptanceNote] AS [ACPT_NOTE], ";
            strSQL += "     [Status] AS [STATUS], ";
            strSQL += "     [Note] AS [NOTE], ";
            strSQL += "     [IsOriginal] AS [IS_ORIGINAL], ";
            strSQL += "     [OrderRowNo] AS [ORDER_ROW_NO], ";
            strSQL += "     [IsReturn] AS [IS_RETURN] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaAcceptance_DTL] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaAcceptanceDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaAcceptanceDetailsConfig>();

            #endregion

            #region - 版權採購單 資訊 -

            var mediaOrderQueryModel = new MediaOrderQueryModel
            {
                REQUISITION_ID = mediaAcceptanceConfig.MEDIA_ORDER_REQUISITION_ID
            };

            var mediaOrderContent = mediaOrderRepository.PostMediaOrderSingle(mediaOrderQueryModel);

            #endregion

            #region - 版權採購交片單 授權權利 -

            List<MediaAcceptanceAuthorizesConfig> mediaAcceptanceAuthorizesConfig = new List<MediaAcceptanceAuthorizesConfig>();
            foreach (var item in mediaAcceptanceDetailsConfig)
            {
                strJson = jsonFunction.ObjectToJSON(mediaOrderContent.MEDIA_ORDER_AUTHS_CONFIG.Where(AUTH => AUTH.ORDER_ROW_NO == item.ORDER_ROW_NO).Select(AUTH => AUTH));
                mediaAcceptanceAuthorizesConfig.AddRange(JsonConvert.DeserializeObject<List<MediaAcceptanceAuthorizesConfig>>(strJson));
            }

            #endregion

            #region - 版權採購交片單 已退貨商品明細 -

            var CommonALDY_COMM = new BPMCommonModel<MediaCommodityConfig>()
            {
                IsALDY = true,
                IDENTIFY = IDENTIFY,
                parameter = parameter
            };
            strJson = jsonFunction.ObjectToJSON(commonRepository.PostMediaCommodityFunction(CommonALDY_COMM));
            var mediaAcceptanceAlreadyRefundCommoditysConfigConfig = jsonFunction.JsonToObject<List<MediaAcceptanceAlreadyRefundCommoditysConfigConfig>>(strJson);

            #endregion

            #region - 版權採購交片單 表單關聯 -

            var formQueryModel = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };
            var associatedForm = commonRepository.PostAssociatedForm(formQueryModel);

            #endregion

            var mediaAcceptanceViewModel = new MediaAcceptanceViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_ACCEPTANCE_TITLE = mediaAcceptanceTitle,
                MEDIA_ACCEPTANCE_CONFIG = mediaAcceptanceConfig,
                MEDIA_ACCEPTANCE_DTLS_CONFIG = mediaAcceptanceDetailsConfig,
                MEDIA_ACCEPTANCE_AUTHS_CONFIG = mediaAcceptanceAuthorizesConfig,
                MEDIA_ACCEPTANCE_ALDY_RF_COMMS_CONFIG = mediaAcceptanceAlreadyRefundCommoditysConfigConfig,
                ASSOCIATED_FORM_CONFIG = associatedForm
            };

            return mediaAcceptanceViewModel;
        }


        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購交片單(依此單內容重送)(僅外部起單使用)
        ///// </summary>        
        //public bool PutMediaAcceptanceRefill(MediaAcceptanceQueryModel query)
        //{
        //    bool vResult = false;
        //    try
        //    {



        //    }
        //    catch (Exception ex)
        //    {
        //        vResult = false;
        //        CommLib.Logger.Error("版權採購交片單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion

        /// <summary>
        /// 版權採購交片單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaAcceptanceSingle(MediaAcceptanceViewModel model)
        {
            bool vResult = false;
            try
            {
                var mediaOrderformQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_REQUISITION_ID
                };
                var mediaOrderformData = formRepository.PostFormData(mediaOrderformQueryModel);

                #region - 宣告 -

                #region - 主旨 -

                FM7Subject = model.MEDIA_ACCEPTANCE_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    FM7Subject = "【驗收】第" + model.MEDIA_ACCEPTANCE_CONFIG.PERIOD + "期-" + mediaOrderformData.FORM_SUBJECT;
                }

                #endregion

                #endregion

                #region - 版權採購交片單 表頭資訊：MediaAcceptance_M -

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
                    //版權採購交片 表頭
                    new SqlParameter("@FLOW_NAME", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ACCEPTANCE_TITLE.FLOW_NAME ?? DBNull.Value },
                    new SqlParameter("@FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ACCEPTANCE_TITLE.FORM_NO ?? DBNull.Value },
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [RequisitionID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaAcceptance_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaAcceptance_M] ";
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
                    strSQL += "     [FlowName]=@FLOW_NAME, ";
                    strSQL += "     [FormNo]=@FORM_NO, ";
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaAcceptance_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FlowName],[FormNo],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FLOW_NAME,@FORM_NO,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 版權採購交片單 表單內容：MediaAcceptance_M -

                model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_BPM_FORM_NO = mediaOrderformData.SERIAL_ID;
                model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_SUBJECT = mediaOrderformData.FORM_SUBJECT;
                model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_PATH = GlobalParameters.FormContentPath(model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_REQUISITION_ID, mediaOrderformData.IDENTIFY, mediaOrderformData.DIAGRAM_NAME);

                if (model.MEDIA_ACCEPTANCE_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //版權採購交片單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                        new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@MEDIA_ORDER_PATH", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NO", SqlDbType.NVarChar) { Size = 16, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SUP_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_KIND", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@REG_NO", SqlDbType.NVarChar) { Size = 15, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@OWNER_TEL", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_FILM_STORAGE", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：版權採購交片單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_ACCEPTANCE_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaAcceptance_M] ";
                    strSQL += "SET [MediaOrderRequisitionID]=@MEDIA_ORDER_REQUISITION_ID, ";
                    strSQL += "     [MediaOrderSubject]=@MEDIA_ORDER_SUBJECT, ";
                    strSQL += "     [MediaOrderBPMFormNo]=@MEDIA_ORDER_BPM_FORM_NO, ";
                    strSQL += "     [MediaOrderERPFormNo]=@MEDIA_ORDER_ERP_FORM_NO, ";
                    strSQL += "     [MediaOrderPath]=@MEDIA_ORDER_PATH, ";
                    strSQL += "     [Period]=@PERIOD, ";
                    strSQL += "     [SupNo]=@SUP_NO, ";
                    strSQL += "     [SupName]=@SUP_NAME, ";
                    strSQL += "     [RegisterKind]=@REG_KIND, ";
                    strSQL += "     [RegisterNo]=@REG_NO, ";
                    strSQL += "     [OwnerName]=@OWNER_NAME, ";
                    strSQL += "     [OwnerTEL]=@OWNER_TEL, ";
                    strSQL += "     [IsFilmStorage]=@IS_FILM_STORAGE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 版權採購交片單 驗收明細: MediaAcceptance_DTL -

                var parameterDetails = new List<SqlParameter>()
                {
                    //版權採購交片單 驗收明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.APPLICANT_INFO.REQUISITION_ID },
                    new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                    new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                    new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)model.MEDIA_ACCEPTANCE_CONFIG.PERIOD ?? DBNull.Value },
                    new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@DISMANTLE_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@GET_MASTERING_DATE", SqlDbType.Date) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEPT_MAIN_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_DEPT_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@OWNER_NAME", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACPT_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@STATUS", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_ORIGINAL", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@IS_RETURN", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaAcceptance_DTL] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_ACCEPTANCE_DTLS_CONFIG != null && model.MEDIA_ACCEPTANCE_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_ACCEPTANCE_DTLS_CONFIG)
                    {
                        //寫入：版權採購交片單 驗收明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaAcceptance_DTL]([RequisitionID],[MediaOrderRequisitionID],[MediaOrderBPMFormNo],[Period],[SupProdANo],[RowNo],[ItemName],[MediaSpec],[MediaType],[StartEpisode],[EndEpisode],[OrderEpisode],[ACPT_Episode],[DismantleEpisode],[EpisodeTime],[GetMasteringDate],[OwnerDeptMainID],[OwnerDeptID],[OwnerID],[OwnerName],[AcceptanceNote],[Status],[Note],[IsOriginal],[OrderRowNo],[IsReturn]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@MEDIA_ORDER_REQUISITION_ID,@MEDIA_ORDER_BPM_FORM_NO,@PERIOD,@SUP_PROD_A_NO,@ROW_NO,@ITEM_NAME,@MEDIA_SPEC,@MEDIA_TYPE,@START_EPISODE,@END_EPISODE,@ORDER_EPISODE,@ACPT_EPISODE,@DISMANTLE_EPISODE,@EPISODE_TIME,@GET_MASTERING_DATE,@OWNER_DEPT_MAIN_ID,@OWNER_DEPT_ID,@OWNER_ID,@OWNER_NAME,@ACPT_NOTE,@STATUS,@NOTE,@IS_ORIGINAL,@ORDER_ROW_NO,@IS_RETURN) ";

                        dbFun.DoTran(strSQL, parameterDetails);
                    }

                    #endregion
                }

                #endregion

                #region - 版權採購交片單 授權權利: MediaAcceptance_AUTH -

                //View 是執行
                //版權採購申請單 授權權利(MediaOrder_AUTHS) 內容。

                #endregion
                                
                #region - 版權採購交片單 表單關聯：AssociatedForm -

                //關聯表:匯入【版權採購交片單】的「關聯表單」
                var importAssociatedForm = commonRepository.PostAssociatedForm(mediaOrderformQueryModel);

                #region 關聯表:加上【行政採購申請單】

                importAssociatedForm.Add(new AssociatedFormConfig()
                {
                    IDENTIFY = mediaOrderformData.IDENTIFY,
                    ASSOCIATED_REQUISITION_ID = model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_REQUISITION_ID,
                    BPM_FORM_NO = mediaOrderformData.SERIAL_ID,
                    FM7_SUBJECT = mediaOrderformData.FORM_SUBJECT,
                    APPLICANT_DEPT_NAME = mediaOrderformData.APPLICANT_DEPT_NAME,
                    APPLICANT_NAME = mediaOrderformData.APPLICANT_NAME,
                    APPLICANT_DATE_TIME = mediaOrderformData.APPLICANT_DATETIME.ToString("yyyy/MM/dd HH:mm:ss"),
                    FORM_PATH = GlobalParameters.FormContentPath(model.MEDIA_ACCEPTANCE_CONFIG.MEDIA_ORDER_REQUISITION_ID, mediaOrderformData.IDENTIFY, mediaOrderformData.DIAGRAM_NAME),
                    STATE = BPMStatusCode.CLOSE
                });

                #endregion

                var associatedFormConfig = model.ASSOCIATED_FORM_CONFIG;
                if (associatedFormConfig == null || associatedFormConfig.Count <= 0)
                {
                    associatedFormConfig = importAssociatedForm;
                }

                var associatedFormModel = new AssociatedFormModel()
                {
                    REQUISITION_ID = model.APPLICANT_INFO.REQUISITION_ID,
                    ASSOCIATED_FORM_CONFIG = associatedFormConfig
                };

                //寫入「關聯表單」
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
                CommLib.Logger.Error("版權採購交片單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 版權採購交片單(驗收簽核)
        /// </summary>
        public bool PutMediaAcceptanceApproveSingle(MediaAcceptanceApproveViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 版權採購交片單 驗收簽核: GeneralAcceptanceApprove -

                var parameterApprove = new List<SqlParameter>()
                {
                    //版權採購交片單 驗收簽核
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                    new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@ACPT_NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@STATUS", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                };

                if (!String.IsNullOrEmpty(model.REQUISITION_ID) || !String.IsNullOrWhiteSpace(model.REQUISITION_ID))
                {
                    #region 調整資料

                    foreach (var item in model.MEDIA_ACCEPTANCE_APPROVES_CONFIG)
                    {
                        //寫入：版權採購交片單 驗收明細parameter
                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterApprove);

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaAcceptance_DTL] ";
                        strSQL += "SET  [AcceptanceNote]=@ACPT_NOTE, ";
                        strSQL += "     [Status]=@STATUS ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "     AND [RequisitionID]=@REQUISITION_ID ";
                        strSQL += "     AND [RowNo]=@ROW_NO ";

                        dbFun.DoTran(strSQL, parameterApprove);
                    }

                    #endregion
                }

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("版權採購交片單(驗收簽核)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaAcceptance";

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