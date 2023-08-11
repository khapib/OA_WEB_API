using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 尚未播出檔拷貝申請單
    /// </summary>
    public class MediaWarehouseNotAiredCopyRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 尚未播出檔拷貝申請單(查詢)
        /// </summary>
        public MediaWarehouseNotAiredCopyViewModel PostMediaWarehouseNotAiredCopySingle(MediaWarehouseNotAiredCopyQueryModel query)
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

            #region - 尚未播出檔拷貝申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaWarehouseNotAiredCopyTitle = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseNotAiredCopyTitle>().FirstOrDefault();

            #endregion           

            #region - 尚未播出檔拷貝申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var mediaWarehouseNotAiredCopyConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseNotAiredCopyConfig>().FirstOrDefault();

            #endregion

            #region - 尚未播出檔拷貝申請單 拷貝明細 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     [Episode] AS [EPISODE], ";
            strSQL += "     [Application] AS [APPLICATION], ";
            strSQL += "     [Note] AS [NOTE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var mediaWarehouseNotAiredCopyDetailsConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseNotAiredCopyDetailsConfig>();

            #endregion

            var mediaWarehouseNotAiredCopyViewModel = new MediaWarehouseNotAiredCopyViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                MEDIA_WAREHOUSE_NOT_AIRED_COPY_TITLE = mediaWarehouseNotAiredCopyTitle,
                MEDIA_WAREHOUSE_NOT_AIRED_COPY_CONFIG = mediaWarehouseNotAiredCopyConfig,
                MEDIA_WAREHOUSE_NOT_AIRED_COPY_DTLS_CONFIG = mediaWarehouseNotAiredCopyDetailsConfig,
            };

            #region - 確認表單 -

            if (mediaWarehouseNotAiredCopyViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                #region - 確認BPM表單是否正常起單到系統中 -

                //保留原有資料
                strJson = jsonFunction.ObjectToJSON(mediaWarehouseNotAiredCopyViewModel);

                var BpmSystemOrder = new BPMSystemOrder()
                {
                    REQUISITION_ID = query.REQUISITION_ID,
                    IDENTIFY = IDENTIFY,
                    EXTS = new List<string>()
                    {
                        "M",
                        "D"
                    },
                    IS_ASSOCIATED_FORM = false
                };
                //確認是否有正常到系統起單；清除失敗表單資料並重新送單值行
                if (commonRepository.PostBPMSystemOrder(BpmSystemOrder)) PutMediaWarehouseNotAiredCopySingle(jsonFunction.JsonToObject<MediaWarehouseNotAiredCopyViewModel>(strJson));

                #endregion

                #region - 確認M表BPM表單單號 -

                //避免儲存後送出表單BPM表單單號沒寫入的情形
                var formQuery = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };
                notifyRepository.ByInsertBPMFormNo(formQuery);

                if (String.IsNullOrEmpty(mediaWarehouseNotAiredCopyViewModel.MEDIA_WAREHOUSE_NOT_AIRED_COPY_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(mediaWarehouseNotAiredCopyViewModel.MEDIA_WAREHOUSE_NOT_AIRED_COPY_TITLE.BPM_FORM_NO))
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                    var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                    if (dtBpmFormNo.Rows.Count > 0) mediaWarehouseNotAiredCopyViewModel.MEDIA_WAREHOUSE_NOT_AIRED_COPY_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                }

                #endregion                
            }

            #endregion

            return mediaWarehouseNotAiredCopyViewModel;
        }

        /// <summary>
        /// 尚未播出檔拷貝申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>
        public bool PutMediaWarehouseNotAiredCopyRefill(MediaWarehouseNotAiredCopyQueryModel query)
        {
            bool vResult = false;

            try
            {
                #region - 宣告 -

                var original = PostMediaWarehouseNotAiredCopySingle(query);
                strJson = jsonFunction.ObjectToJSON(original);

                var MediaWarehouseNotAiredCopyViewModel = new MediaWarehouseNotAiredCopyViewModel();

                var requisitionID = Guid.NewGuid().ToString();

                #endregion

                #region - 重送內容 -

                MediaWarehouseNotAiredCopyViewModel = jsonFunction.JsonToObject<MediaWarehouseNotAiredCopyViewModel>(strJson);

                #region - 申請人資訊 調整 -

                MediaWarehouseNotAiredCopyViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
                MediaWarehouseNotAiredCopyViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
                MediaWarehouseNotAiredCopyViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

                #endregion

                #endregion

                #region - 送出 執行(新增/修改/草稿) -

                PutMediaWarehouseNotAiredCopySingle(MediaWarehouseNotAiredCopyViewModel);

                #endregion

                vResult = true;
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("尚未播出檔拷貝申請單(依此單內容重送)失敗，原因：" + ex.Message);
            }

            return vResult;
        }


        /// <summary>
        /// 尚未播出檔拷貝申請單(新增/修改/草稿)
        /// </summary>
        public bool PutMediaWarehouseNotAiredCopySingle(MediaWarehouseNotAiredCopyViewModel model)
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

                FM7Subject = model.MEDIA_WAREHOUSE_NOT_AIRED_COPY_TITLE.FM7_SUBJECT;

                #endregion

                #endregion

                #region - 尚未播出檔拷貝申請單 表頭資訊：MediaWarehouseNotAiredCopy_M -

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
                    //尚未播出檔拷貝申請單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "      [RequisitionID] ";
                    strSQL += "FROM [BPMPro].[dbo].[FSe7en_Sys_Requisition] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    var dtReq = dbFun.DoQuery(strSQL, parameterTitle);
                    if (dtReq.Rows.Count <= 0)
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_M] ";
                    strSQL += "SET [DiagramID] =@DIAGRAM_ID, ";
                    strSQL += "     [ApplicantDept]=@APPLICANT_DEPT, ";
                    strSQL += "     [ApplicantDeptName]=@APPLICANT_DEPT_NAME, ";
                    strSQL += "     [ApplicantID]=@APPLICANT_ID, ";
                    strSQL += "     [ApplicantName]=@APPLICANT_NAME, ";
                    strSQL += "     [ApplicantPhone]=@APPLICANT_PHONE, ";

                    if(IsADD) strSQL += "     [ApplicantDateTime]=@APPLICANT_DATETIME, ";

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
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 尚未播出檔拷貝申請單 表單內容：MediaWarehouseNotAiredCopy_M -

                if (model.MEDIA_WAREHOUSE_NOT_AIRED_COPY_CONFIG != null)
                {                    
                    var parameterInfo = new List<SqlParameter>()
                    {
                        //尚未播出檔拷貝申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：尚未播出檔拷貝申請單 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.MEDIA_WAREHOUSE_NOT_AIRED_COPY_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_M] ";
                    strSQL += "SET [Description]=@DESCRIPTION, ";
                    strSQL += "     [Note]=@NOTE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 尚未播出檔拷貝申請單 拷貝明細：MediaWarehouseNotAiredCopy_D -

                var parameterDetails = new List<SqlParameter>()
                {
                    //尚未播出檔拷貝申請單 拷貝明細
                    new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                    new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 200, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@EPISODE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@APPLICATION", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value },
                    new SqlParameter("@NOTE", SqlDbType.NVarChar) { Size = 4000, Value = (object)DBNull.Value ?? DBNull.Value }
                };

                #region 先刪除舊資料

                strSQL = "";
                strSQL += "DELETE ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_D] ";
                strSQL += "WHERE 1=1 ";
                strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                dbFun.DoTran(strSQL, parameterDetails);

                #endregion

                if (model.MEDIA_WAREHOUSE_NOT_AIRED_COPY_DTLS_CONFIG != null && model.MEDIA_WAREHOUSE_NOT_AIRED_COPY_DTLS_CONFIG.Count > 0)
                {
                    #region 再新增資料

                    foreach (var item in model.MEDIA_WAREHOUSE_NOT_AIRED_COPY_DTLS_CONFIG)
                    {
                        //寫入：尚未播出檔拷貝申請單 拷貝明細parameter

                        strJson = jsonFunction.ObjectToJSON(item);
                        GlobalParameters.Infoparameter(strJson, parameterDetails);

                        strSQL = "";
                        strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_MediaWarehouseNotAiredCopy_D]([RequisitionID],[ItemName],[Episode],[Application],[Note]) ";
                        strSQL += "VALUES(@REQUISITION_ID,@ITEM_NAME,@EPISODE,@APPLICATION,@NOTE) ";

                        dbFun.DoTran(strSQL, parameterDetails);
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
                CommLib.Logger.Error("尚未播出檔拷貝申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "MediaWarehouseNotAiredCopy";

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