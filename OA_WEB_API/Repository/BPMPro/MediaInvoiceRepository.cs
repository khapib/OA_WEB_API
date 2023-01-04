using OA_WEB_API.Models.BPMPro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.SqlServer.Server;
using Microsoft.Ajax.Utilities;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 版權採購請款單
    /// </summary>
    public class MediaInvoiceRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        FlowRepository flowRepository = new FlowRepository();

        #endregion

        #endregion

        #region - 方法 -



        #region - 依此單內容重送 -

        ///// <summary>
        ///// 版權採購請款單(依此單內容重送)(僅外部起單使用)
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
        //        CommLib.Logger.Error("版權採購請款單(依此單內容重送)失敗，原因" + ex.Message);
        //    }
        //    return vResult;
        //}

        #endregion



        /// <summary>
        /// 版權採購請款單(財務審核關卡-關聯表單(知會))：
        /// 【財務審核關卡】"版權採購請款"單關聯表單列表知會；
        /// 確認是否有代理人，
        /// 並知會給代理人。
        /// </summary>
        public bool PutMediaInvoiceNotifySingle(MediaInvoiceQueryModel query)
        {
            bool vResult = false;
            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                #region - 財務審核人 -

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [FinancAuditID_1] AS [FINANC_AUDIT_ID_1], ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_MediaInvoice_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                var dtFinancAudit1 = dbFun.DoQuery(strSQL, parameter);
                var FinancAudit1 = dtFinancAudit1.Rows[0][0].ToString();

                if (!string.IsNullOrEmpty(FinancAudit1) || !string.IsNullOrWhiteSpace(FinancAudit1))
                {
                    var flowQueryModel = new FlowQueryModel()
                    {
                        USER_ID = FinancAudit1
                    };
                    //被知會通知人
                    var NotifyBys = new List<String>()
                    {
                        FinancAudit1
                    };

                    #region - 代理人 -

                    var Agents = flowRepository.PostAgent(flowQueryModel);

                    if (Agents != null)
                    {
                        if (Agents.Count > 0)
                        {
                            Agents.ForEach(A =>
                            {
                                NotifyBys.Add(A.AGENT_ID);
                            });
                        }
                        else
                        {
                            CommLib.Logger.Debug(query.REQUISITION_ID + "：" + FinancAudit1 + "目前(" + DateTime.Now + ")尚無設定 「代理人」(2)。");
                        }
                    }
                    else
                    {
                        CommLib.Logger.Debug(query.REQUISITION_ID + "：" + FinancAudit1 + "目前(" + DateTime.Now + ")尚無設定 「代理人」(1)。");
                    }

                    #endregion

                    #region - 關聯表單(知會) -

                    var associatedFormNotifyModel = new AssociatedFormNotifyModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID,
                        NOTIFY_BY = NotifyBys
                    };
                    vResult = commonRepository.PutAssociatedFormNotify(associatedFormNotifyModel);

                    #endregion
                }
                else
                {
                    CommLib.Logger.Debug(query.REQUISITION_ID + "：此表單「尚未決定」財務審核人，可(知會)通知。");
                }

                #endregion

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購請款單(財務審核關卡-關聯表單(知會))通知失敗，原因：" + ex.Message);
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
        /// 表單代號
        /// </summary>
        private string IDENTIFY = "MediaInvoice";

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