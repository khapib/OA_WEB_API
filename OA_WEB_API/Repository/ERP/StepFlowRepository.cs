using System;
using System.Web.Script.Serialization;
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
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.BPMPro;

using Newtonsoft.Json;

namespace OA_WEB_API.Repository.ERP
{
    /// <summary>
    /// BPM簽核狀況[ERP]
    /// </summary>
    public class StepFlowRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDev);

        #region Model

        StepFlowConfig stepFlowConfig = new StepFlowConfig();
        ErpResponseState erpResponseState = new ErpResponseState();

        #endregion

        #region Repository

        FormRepository formRepository = new FormRepository();

        #endregion

        #endregion

        #region - 方法 -

        #region - BPM表單狀態細項 -

        /// <summary>
        /// BPM表單狀態細項
        /// </summary>
        public StepFlowConfig StepFlowInfo(FormData formData, List<SqlParameter> parameter)
        {

            strSQL = "";
            strSQL += " SELECT TOP 1 ";
            strSQL += "     M.[FormNo] AS [ERP_FORM_NO], ";
            strSQL += "     M.[RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     M.[BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     M.[FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     M.[ApplicantDept] AS [APPLICANT_DEPT], ";
            strSQL += "     M.[ApplicantID] AS [APPLICANT_ID], ";
            strSQL += "     M.[ApplicantName] AS[APPLICANT_NAME], ";
            strSQL += "     M.[ApplicantDateTime] AS [APPLICANT_DATE_TIME], ";
            strSQL += "     M.[DraftFlag] AS [DRAFT_FLAG], ";
            strSQL += "     S.[ProcessID] AS [PROCESS_ID], ";
            strSQL += "     S.[ApproverDept] AS [APPROVER_DEPT], ";
            strSQL += "     S.[ApproverID] AS [APPROVER_ID], ";
            strSQL += "     S.[ApproverName] AS [APPROVER_NAME], ";
            strSQL += "     S.[ApproveTime] AS [APPROVE_TIME], ";
            strSQL += "     S.[Result] AS [RESULT], ";
            strSQL += "     S.[ResultPrompt] AS [RESULT_PROMPT], ";
            strSQL += "     S.[Comment] AS [COMMENT] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_M] AS M ";
            strSQL += "LEFT JOIN [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_S] AS S ON M.[RequisitionID]=S.[RequisitionID] AND S.[ApproverName] IS NOT NULL ";
            strSQL += "WHERE 1=1 ";
            strSQL += "         AND [M].[RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [S].[ApproveTime] DESC ";

            stepFlowConfig = dbFun.DoQuery(strSQL, parameter).ToList<StepFlowConfig>().FirstOrDefault();

            return stepFlowConfig;
        }

        #endregion

        /// <summary>
        /// [手動同步]BPM表單狀態(查詢)
        /// </summary>
        /// <param name="query"></param>
        /// <returns>
        /// 
        /// 狀態回傳
        /// （
        /// 新建：NewCreate；
        /// 已簽完：Close；
        /// 不同意結束：DisagreeClose； 
        /// 簽核中：Progress；
        /// 草稿：Draft
        /// ）
        /// 
        /// </returns>
        public string PostStepFlowSingle(StepFlowQueryModel query)
        {
            //預設回傳狀態: 新建
            var State = BPMStatusCode.NEW_CREATE;

            try
            {
                #region  - 查詢 - 

                #region - 表單資訊 -

                var formQueryModel = new FormQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostDataHaveForm(formQueryModel))
                {
                    var formData = formRepository.PostFormData(formQueryModel);

                    #region  - BPM簽核狀況查詢 - 

                    var parameter = new List<SqlParameter>()
                    {
                         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    };

                    stepFlowConfig = StepFlowInfo(formData, parameter);

                    #endregion

                    #region - BPM簽核狀況設定回應 -

                    if (!String.IsNullOrEmpty(stepFlowConfig.REQUISITION_ID) || !String.IsNullOrWhiteSpace(stepFlowConfig.REQUISITION_ID))
                    {
                        // 有資料
                        if (stepFlowConfig.DRAFT_FLAG == 1)
                        {
                            // 表單儲存在草稿中
                            State = BPMStatusCode.DRAFT;
                        }
                        else
                        {
                            #region 如果「是否表單已完結」欄位是空的；則會先查詢表單狀態

                            string StateEND = null;
                            if (String.IsNullOrWhiteSpace(query.STATE_END) || String.IsNullOrEmpty(query.STATE_END))
                            {
                                switch (formData.FORM_STATUS.ToString())
                                {
                                    case BPMSysStatus.CLOSE:
                                        StateEND = true.ToString();
                                        break;
                                    case BPMSysStatus.DISAGREE_CLOSE:
                                    case BPMSysStatus.WITHDRAWAL:
                                    case BPMSysStatus.EXCEPTION:
                                        StateEND = false.ToString();
                                        break;
                                    default:
                                        StateEND = null;
                                        break;
                                }
                            }
                            else
                            {
                                StateEND = query.STATE_END;
                            }

                            #endregion

                            if (String.IsNullOrEmpty(StateEND) || String.IsNullOrWhiteSpace(StateEND))
                            {
                                // 表單進行中
                                State = BPMStatusCode.PROGRESS;
                            }
                            else if (bool.Parse(StateEND) == false)
                            {
                                // 表單不同意結束
                                State = BPMStatusCode.DISAGREE_CLOSE;
                            }
                            else if (bool.Parse(StateEND) == true)
                            {
                                // 表單同意結束
                                State = BPMStatusCode.CLOSE;
                            }

                        }
                    }
                    else
                    {
                        // 無資料
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion
                }
                else
                {
                    // 錯誤
                    State = BPMStatusCode.FAIL;
                }
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("[ERP]BPM簽核狀況(查詢)失敗，原因：" + ex.Message);
                throw;
            }

            return State;
        }

        /// <summary>
        /// 更新ERP表單狀態(內容)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public StepSignResponse PostStepSignContentSingle(StepFlowQueryModel query)
        {
            try
            {
                StepSignResponse stepSignResponse = null;

                #region  - 查詢及執行 - 

                FormQueryModel formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostDataHaveForm(formQueryModel))
                {
                    //表單資料
                    var formData = formRepository.PostFormData(formQueryModel);

                    #region  - BPM簽核狀況查詢 - 

                    var parameter = new List<SqlParameter>()
                    {
                         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    };

                    stepFlowConfig = StepFlowInfo(formData, parameter);

                    #endregion

                    #region - BPM簽核狀況 -

                    //BPM FromContent頁-表單內容檢視頁路徑
                    var ViewPath = GlobalParameters.FormContentPath(formData.REQUISITION_ID, formData.IDENTIFY, formData.DIAGRAM_NAME);

                    stepSignResponse = new StepSignResponse()
                    {
                        ERP_FormNo=stepFlowConfig.ERP_FORM_NO,
                        RequisitionID = stepFlowConfig.REQUISITION_ID,
                        BPM_FormNo = formData.SERIAL_ID,
                        FM7Subject = formData.FORM_SUBJECT,
                        State = PostStepFlowSingle(query),
                        LoginId = stepFlowConfig.APPROVER_ID,
                        LoginName = stepFlowConfig.APPROVER_NAME,
                        ViewPath = ViewPath
                    };

                    #endregion
                }

                #endregion

                return stepSignResponse;

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("回傳簽核資訊失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 更新ERP表單狀態
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string PostStepSignSingle(StepFlowQueryModel query)
        {
            try
            {
                StepSignResponse stepSignResponse = null;

                #region  - 查詢及執行 - 

                FormQueryModel formQueryModel = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostDataHaveForm(formQueryModel))
                {
                    //表單資料
                    var formData = formRepository.PostFormData(formQueryModel);

                    #region  - BPM簽核狀況查詢 - 

                    var parameter = new List<SqlParameter>()
                    {
                         new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    };

                    stepFlowConfig = StepFlowInfo(formData, parameter);

                    #endregion

                    #region - BPM簽核狀況 -

                    //BPM FromContent頁-表單內容檢視頁路徑
                    var ViewPath = GlobalParameters.FormContentPath(formData.REQUISITION_ID, formData.IDENTIFY, formData.DIAGRAM_NAME);

                    stepSignResponse = new StepSignResponse()
                    {
                        ERP_FormNo=stepFlowConfig.ERP_FORM_NO,
                        RequisitionID = stepFlowConfig.REQUISITION_ID,
                        BPM_FormNo = formData.SERIAL_ID,
                        FM7Subject = formData.FORM_SUBJECT,
                        State = PostStepFlowSingle(query),
                        LoginId = stepFlowConfig.APPROVER_ID,
                        LoginName = stepFlowConfig.APPROVER_NAME,
                        ViewPath = ViewPath
                    };
                    strRequestJson = JsonConvert.SerializeObject(stepSignResponse);

                    #endregion
                }

                #region - BPM寫Log -

                //CommLib.Logger.Debug("表單編號 : " + query.REQUISITION_ID + "，目前BPM執行狀態 : " + stepSignResponse.State);
                CommLib.Logger.Debug("表單編號 : " + query.REQUISITION_ID + "，BPM傳輸內容 : " + strRequestJson.ToString());

                #endregion

                #region - 回傳ERP - 

                ApiUrl = GlobalParameters.ERPSystemAPI(GlobalParameters.sqlConnBPMProDev) + "BPM/UpdateFormStatus";
                Method = "POST";
                strResponseJson = GlobalParameters.RequestInfoWebServers(ApiUrl, Method, strRequestJson);

                erpResponseState = jsonFunction.JsonToObject<ErpResponseState>(strResponseJson);

                #region - ERP寫Log -

                CommLib.Logger.Debug("表單編號 : " + query.REQUISITION_ID + "，ERP回傳狀態 : " + erpResponseState.data + "," + erpResponseState.code + "," + erpResponseState.data + "。");                

                #endregion

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("回傳簽核資訊失敗，原因：" + ex.Message);
                throw;
            }

            return erpResponseState.msg;
        }

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

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