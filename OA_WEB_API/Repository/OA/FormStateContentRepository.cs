using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

using OA_WEB_API.Models;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.BPMPro;
using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.OA
{
    /// <summary>
    /// BPM簽核狀況[OA]
    /// </summary>
    public class FormStateContentRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Model


        #endregion

        #region Repository

        FormRepository formRepository = new FormRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// [OA]BPM表單狀態(查詢)
        /// </summary>
        public string PostFormStateSingle(StepFlowQueryModel query)
        {
            //預設回傳狀態: 新建
            var State = OAStatusCode.NEW_CREATE;
            
            try
            {
                #region - 查詢 -

                var formQueryModel = new FormQueryModel
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (BPMPro.CommonRepository.PostDataHaveForm(formQueryModel))
                {
                    var formData = formRepository.PostFormData(formQueryModel);

                    var parameter = new List<SqlParameter>()
                    {
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                    };

                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "      [RequisitionID] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    var dtA = dbFun.DoQuery(strSQL, parameter);

                    if (dtA.Rows.Count > 0)
                    {
                        #region 如果「是否表單已完結」欄位是空的；則會先查詢表單狀態

                        string StateEND = null;
                        if (String.IsNullOrWhiteSpace(query.STATE_END) || String.IsNullOrEmpty(query.STATE_END))
                        {
                            switch (formData.FORM_STATUS.ToString())
                            {
                                case OAStatusCode.CLOSE:
                                    StateEND = true.ToString();
                                    break;
                                case OAStatusCode.DISAGREE_CLOSE:
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
                            //確認新建還是修改的狀態

                            strSQL = "";
                            strSQL += "SELECT ";
                            strSQL += "      [OA_MasterNo] ";
                            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_M] ";
                            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                            var dtOA = dbFun.DoQuery(strSQL, parameter);
                            var OA_MasterNo = dtOA.Rows[0][0].ToString();

                            if (!String.IsNullOrEmpty(OA_MasterNo) || !String.IsNullOrWhiteSpace(OA_MasterNo))
                            {
                                // 表單修改
                                State = OAStatusCode.MODIFY;
                            }
                            else
                            {
                                // 表單新建
                                State = OAStatusCode.NEW_CREATE;
                            }
                        }
                        else if (bool.Parse(StateEND) == false)
                        {
                            // 表單不同意結束
                            State = OAStatusCode.DISAGREE_CLOSE;
                        }
                        else if (bool.Parse(StateEND) == true)
                        {
                            // 表單同意結束
                            State = OAStatusCode.CLOSE;
                        }
                    }
                    else
                    {
                        // 錯誤
                        State = BPMStatusCode.FAIL;
                    }
                }
                else
                {
                    // 錯誤
                    State = BPMStatusCode.FAIL;
                }

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("[OA]BPM表單狀態(查詢)失敗，原因：" + ex.Message);
                throw;
            }

            return State;
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