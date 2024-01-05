using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Http.Results;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.OA;
using OA_WEB_API.Repository.BPMPro;
using OA_WEB_API.Repository.ERP;

using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

namespace OA_WEB_API.Repository.OA
{
    /// <summary>
    /// 回傳OA資訊
    /// </summary>
    public class ResponseOAInfoRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Model

        OAResponseState OA_ResponseState = new OAResponseState();

        #endregion

        #region Repository

        FormRepository formRepository = new FormRepository();
        StepFlowRepository stepFlowRepository = new StepFlowRepository();
        FormStateContentRepository formStateContentRepository = new FormStateContentRepository();

        #endregion

        #region FormRepository

        /// <summary>拷貝申請單</summary>
        MediaWarehouseCopyRepository mediaWarehouseCopyRepository = new MediaWarehouseCopyRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 拷貝申請單 回傳OA
        /// </summary>
        public MediaWarehouseCopyResponseOA PostMediaWarehouseCopyResponseOASingle(StepFlowQueryRequestModel query)
        {
            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                };

                #region - 宣告 -

                var mediaWarehouseCopyResponseOA = new MediaWarehouseCopyResponseOA();
                var Priority = String.Empty;
                var ContactPerson = String.Empty;

                #endregion

                #region - 查詢及執行 -

                #region - 拷貝申請單 回傳OA 內容 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     null AS [BPM_FORM_PATH], ";
                strSQL += "     null AS [BPM_FORM_ACTION], ";
                strSQL += "     [OA_MasterNo] AS [OA_MASTER_NO], ";
                strSQL += "     [OA_FormNo] AS [OA_FORM_NO], ";
                strSQL += "     null AS [CASE_LEVEL], ";
                strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
                strSQL += "     [FM7Subject] AS [SUBJECT], ";
                strSQL += "     [ApplicantDateTime] AS [APPLY_DATE], ";
                strSQL += "     [ApplicantID] AS [APPLY_PERSON], ";
                strSQL += "     [ExpectedDate] AS [EXPECTED_DATE], ";
                strSQL += "     [ApplyType] AS [APPLY_TYPE], ";
                strSQL += "     [ApplyTypeOthers] AS [APPLY_TYPE_OTHERS], ";
                strSQL += "     [Contact] AS [CONTACT], ";
                strSQL += "     [ApprovalNo] AS [SELL_DOC_NO], ";
                strSQL += "     null AS [MODIFY_PERSON], ";
                strSQL += "     [Note] AS [NOTE], ";
                strSQL += "     null AS [DELIVER_DATE], ";
                strSQL += "     [ContactPerson] AS [CREATE_PERSON] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var mediaWarehouseCopyResponseOAInfoConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseCopyResponseOAInfoConfig>().FirstOrDefault();

                #endregion

                if(mediaWarehouseCopyResponseOAInfoConfig != null)
                {
                    #region - BPM表單狀態 -

                    strJson = jsonFunction.ObjectToJSON(query);
                    var stepFlowQuery = jsonFunction.JsonToObject<StepFlowQueryModel>(strJson);
                    mediaWarehouseCopyResponseOAInfoConfig.BPM_FORM_ACTION = formStateContentRepository.PostFormStateSingle(stepFlowQuery);
                    if (mediaWarehouseCopyResponseOAInfoConfig.BPM_FORM_ACTION == OAStatusCode.MODIFY || mediaWarehouseCopyResponseOAInfoConfig.BPM_FORM_ACTION == OAStatusCode.NEW_CREATE)
                    {
                        //拷貝處裡確認後回傳OA「待領取」
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [Result] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_S] ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "          AND RequisitionID=@REQUISITION_ID ";
                        strSQL += "          AND ProcessID='SpcRol02' ";

                        dt = dbFun.DoQuery(strSQL, parameter);

                        if (dt.Rows.Count > 0)
                        {
                            var strResult = dt.Rows[0][0].ToString();
                            if (int.Parse(strResult) == 1) mediaWarehouseCopyResponseOAInfoConfig.BPM_FORM_ACTION = "PickUp";
                        }
                    }

                    #endregion

                    #region - 案件等級 -

                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [Priority] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dt = dbFun.DoQuery(strSQL, parameter);
                    if (dt.Rows.Count > 0)
                    {
                        Priority = dt.Rows[0][0].ToString();
                        mediaWarehouseCopyResponseOAInfoConfig.CASE_LEVEL = CommonRepository.OA_CaseLevel(Priority);
                    }

                    #endregion

                    #region - 協力單位 -

                    ContactPerson = mediaWarehouseCopyResponseOAInfoConfig.CREATE_PERSON;

                    switch (ContactPerson)
                    {
                        case "G021":
                            ContactPerson = "GTV_MEDIA_WAREHOUSE_COPY_SIGNAL";
                            break;
                        case "G023":
                            ContactPerson = "GTV_MEDIA_WAREHOUSE_COPY_MEDIA_WAREHOUSE";
                            break;
                        case "G022":
                            ContactPerson = "GTV_MEDIA_WAREHOUSE_COPY_MEDIA_WAREHOUSE";
                            break;
                        default:
                            break;
                    }

                    var AtomID=BPMPro.CommonRepository.GetRoles().Where(R=>R.ROLE_ID== ContactPerson).Select(R=>R.USER_ID).First();

                    var parameterB = new List<SqlParameter>()
                    {
                         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                         new SqlParameter("@AtomID", SqlDbType.NVarChar) { Size = 64, Value = AtomID },
                    };

                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [DynamicApproverID] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_S] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "AND [RequisitionID]=@REQUISITION_ID ";
                    strSQL += "AND [ApproverID]=@AtomID ";

                    dt = dbFun.DoQuery(strSQL, parameterB);
                    if (dt.Rows.Count > 0)
                    {
                        var DynamicApproverID = dt.Rows[0][0].ToString();

                        if (!String.IsNullOrEmpty(DynamicApproverID) || !String.IsNullOrWhiteSpace(DynamicApproverID))
                        {
                            string[] DynamicApproverIDArray = DynamicApproverID.Split('@');
                            mediaWarehouseCopyResponseOAInfoConfig.MODIFY_PERSON = DynamicApproverIDArray[0];
                        }
                    }

                    #endregion

                    #region - 處理人員 -

                    mediaWarehouseCopyResponseOAInfoConfig.CREATE_PERSON = AtomID;

                    #endregion

                    #region - 拷貝申請單 回傳OA 節目資訊 -

                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     D.[RequisitionID] AS [REQUISITION_ID], ";
                    strSQL += "     D.[ProgramName] AS [PROGRAM_NAME], ";
                    strSQL += "     D.[Volume] AS [VOLUME], ";
                    strSQL += "     D.[MeterialType] AS [METERIAL_TYPE], ";
                    strSQL += "     D.[MeterialTypeOthers] AS [METERIAL_TYPE_OTHERS], ";
                    strSQL += "     M.[SaveType] AS [COPY_TYPE], ";
                    strSQL += "     M.[SaveTypeOthers] AS [COPY_TYPE_OTHERS], ";
                    strSQL += "     D.[Length] AS [LENGTH], ";
                    strSQL += "     D.[LengthOthers] AS [LENGTH_OTHERS] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_D] AS D ";
                    strSQL += "     LEFT JOIN [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] AS M ON D.RequisitionID=M.RequisitionID ";
                    strSQL += "WHERE D.[RequisitionID]=@REQUISITION_ID ";
                    strSQL += "ORDER BY [AutoCounter] ";

                    var mediaWarehouseCopyResponseOAProgramInfoConfig = dbFun.DoQuery(strSQL, parameter).ToList<MediaWarehouseCopyResponseOAProgramInfoConfig>();

                    #endregion

                    #region - BPM表單連結 -

                    if (mediaWarehouseCopyResponseOAInfoConfig.BPM_FORM_ACTION != OAStatusCode.FAIL)
                    {
                        var formQueryModel = new FormQueryModel
                        {
                            REQUISITION_ID = query.REQUISITION_ID
                        };

                        var formData = formRepository.PostFormData(formQueryModel);
                        mediaWarehouseCopyResponseOAInfoConfig.BPM_FORM_PATH = GlobalParameters.FormContentPath(query.REQUISITION_ID, formData.IDENTIFY, formData.DIAGRAM_NAME);
                    }

                    #endregion

                    mediaWarehouseCopyResponseOA = new MediaWarehouseCopyResponseOA()
                    {
                        MEDIA_WAREHOUSE_COPY_RESPONSE_OA_INFO_CONFIG = mediaWarehouseCopyResponseOAInfoConfig,
                        MEDIA_WAREHOUSE_COPY_RESPONSE_OA_PROGRAM_INFO_CONFIG = mediaWarehouseCopyResponseOAProgramInfoConfig,
                    };
                }

                #endregion

                #region - 回傳OA - 

                /*正式機要加回來*/
                //if (bool.Parse(query.REQUEST_FLG))
                //{
                //    ApiUrl = "http://oa.gtv.com.tw/OA2/FilmWareHouseCopyWork/FilmWareHouseCopyWork_BpmOaSync.ashx";
                //    Method = "POST";
                //    strResponseJson = GlobalParameters.RequestInfoWebAPI(ApiUrl, Method, mediaWarehouseCopyResponseOA);
                //    OA_ResponseState = JsonConvert.DeserializeObject<OAResponseState>(strResponseJson);
                //    CommLib.Logger.Debug("拷貝申請單:" + query.REQUISITION_ID + " OA狀態：" + OA_ResponseState.OA_FORM_ACTION);
                //    mediaWarehouseCopyResponseOA.OA_RESPONSE_STATE = OA_ResponseState;

                //    if (!String.IsNullOrEmpty(OA_ResponseState.OA_MASTER_NO) || !String.IsNullOrWhiteSpace(OA_ResponseState.OA_MASTER_NO))
                //    {
                //        var parameterState = new List<SqlParameter>()
                //        {
                //            //OA狀態資訊
                //            new SqlParameter("@OA_MASTER_NO", SqlDbType.NVarChar) { Size = 64, Value = OA_ResponseState.OA_MASTER_NO  },
                //            new SqlParameter("@OA_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = OA_ResponseState.OA_FORM_NO },
                //            new SqlParameter("@BPM_FORM_NO", SqlDbType.NVarChar) { Size = 64, Value = mediaWarehouseCopyResponseOA.MEDIA_WAREHOUSE_COPY_RESPONSE_OA_INFO_CONFIG.BPM_FORM_NO },
                //        };

                //        strSQL = "";
                //        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_MediaWarehouseCopy_M] ";
                //        strSQL += "SET  [OA_MasterNo] =@OA_MASTER_NO, ";
                //        strSQL += "     [OA_FormNo]=@OA_FORM_NO ";
                //        strSQL += "WHERE [BPMFormNo]=@BPM_FORM_NO ";

                //        dbFun.DoTran(strSQL, parameterState);
                //    }
                //}

                #endregion

                return mediaWarehouseCopyResponseOA;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("拷貝申請單(回傳OA)失敗，原因：" + ex.Message);
                throw;
            }
        }

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
        /// Data Table
        /// </summary>
        private DataTable dt = null;

        #region - 請求信息欄位 -

        /// <summary>
        /// 請求信息-Api路徑
        /// </summary>
        private string ApiUrl;

        /// <summary>
        /// 請求信息-取得或要求的方法
        /// </summary>
        private string Method;

        /// <summary>
        /// 請求信息-輸入值
        /// </summary>
        private string strRequestJson;

        /// <summary>
        /// 請求信息-接收回傳值
        /// </summary>
        private string strResponseJson;

        #endregion

        #endregion
    }
}