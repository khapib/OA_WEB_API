using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models;

using Microsoft.Ajax.Utilities;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 離職、留職停薪_手續表
    /// </summary>
    public class ResignUnpaidLeaveAgendaRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Repository

        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();
        SysCommonRepository sysCommonRepository = new SysCommonRepository();

        #endregion

        #endregion

        #region - 方法 -

        /// <summary>
        /// 離職、留職停薪_手續表(查詢)
        /// </summary>
        public ResignUnpaidLeaveAgendaViewModel PostResignUnpaidLeaveAgendaSingle(ResignUnpaidLeaveAgendaQueryModel query)
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

            #region - 離職、留職停薪_手續表 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var resignUnpaidLeaveAgendaTitle = dbFun.DoQuery(strSQL, parameter).ToList<ResignUnpaidLeaveAgendaTitle>().FirstOrDefault();

            #endregion

            #region - 離職、留職停薪_手續表 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FormAction] AS [FORM_ACTION], ";
            strSQL += "     [ResignDate] AS [RESIGN_DATE], ";
            strSQL += "     [HandoverSupervisorDeptID] AS [HANDOVER_SUPERVISOR_DEPT_ID], ";
            strSQL += "     [HandoverSupervisorID] AS [HANDOVER_SUPERVISOR_ID], ";
            strSQL += "     [HandoverSupervisorName] AS [HANDOVER_SUPERVISOR_NAME], ";
            strSQL += "     [C01B_Date] AS [C01B_DATE], ";
            strSQL += "     [C01C_StrDateTime] AS [C01C_STR_DATE_TIME], ";
            strSQL += "     [C01F_StrDateTime] AS [C01F_STR_DATE_TIME], ";
            strSQL += "     [C01H_StrDateTime] AS [C01H_STR_DATE_TIME], ";
            strSQL += "     [C02_Others] AS [C02_OTHERS], ";
            strSQL += "     [C03_Others] AS [C03_OTHERS] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var resignUnpaidLeaveAgendaConfig = dbFun.DoQuery(strSQL, parameter).ToList<ResignUnpaidLeaveAgendaConfig>().FirstOrDefault();

            #endregion

            #region - 離職、留職停薪_手續表 事務清單 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [RequisitionID] AS [REQUISITION_ID], ";
            strSQL += "     [ItemID] AS [ITEM_ID], ";
            strSQL += "     [ItemName] AS [ITEM_NAME], ";
            strSQL += "     CAST([IsConsummation] as bit) AS [IS_CONSUMMATION], ";
            strSQL += "     [Description] AS [DESCRIPTION], ";
            strSQL += "     [ContacterDeptID] AS [CONTACTER_DEPT_ID], ";
            strSQL += "     [ContacterID] AS [CONTACTER_ID], ";
            strSQL += "     [ContacterName] AS [CONTACTER_NAME], ";
            strSQL += "     [SignDate] AS [SIGN_DATE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [ItemID],[AutoCounter] ";

            var resignUnpaidLeaveAgendaAffairsConfig = dbFun.DoQuery(strSQL, parameter).ToList<ResignUnpaidLeaveAgendaAffairsConfig>();

            #endregion

            var resignUnpaidLeaveAgendaViewModel = new ResignUnpaidLeaveAgendaViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                RESIGN_UNPAID_LEAVE_AGENDA_TITLE = resignUnpaidLeaveAgendaTitle,
                RESIGN_UNPAID_LEAVE_AGENDA_CONFIG = resignUnpaidLeaveAgendaConfig,
                RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG = resignUnpaidLeaveAgendaAffairsConfig
            };

            #region - 確認表單 -

            if (resignUnpaidLeaveAgendaViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                #region - 確認BPM表單是否正常起單到系統中 -

                //保留原有資料
                strJson = jsonFunction.ObjectToJSON(resignUnpaidLeaveAgendaViewModel);

                var BpmSystemOrder = new BPMSystemOrder()
                {
                    REQUISITION_ID = query.REQUISITION_ID,
                    IDENTIFY = IDENTIFY,
                    EXTS = new List<string>()
                    {
                        "M",
                        "D"
                    },
                    IS_ASSOCIATED_FORM = false,
                };
                //確認是否有正常到系統起單；清除失敗表單資料並重新送單值行
                if (commonRepository.PostBPMSystemOrder(BpmSystemOrder)) PutResignUnpaidLeaveAgendaSingle(jsonFunction.JsonToObject<ResignUnpaidLeaveAgendaViewModel>(strJson));

                #endregion

                #region - 確認M表BPM表單單號 -

                //避免儲存後送出表單BPM表單單號沒寫入的情形
                var formQuery = new FormQueryModel()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                notifyRepository.ByInsertBPMFormNo(formQuery);

                if (String.IsNullOrEmpty(resignUnpaidLeaveAgendaViewModel.RESIGN_UNPAID_LEAVE_AGENDA_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(resignUnpaidLeaveAgendaViewModel.RESIGN_UNPAID_LEAVE_AGENDA_TITLE.BPM_FORM_NO))
                {
                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                    var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                    if (dtBpmFormNo.Rows.Count > 0) resignUnpaidLeaveAgendaViewModel.RESIGN_UNPAID_LEAVE_AGENDA_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                }

                #endregion
            }

            #endregion

            if (resignUnpaidLeaveAgendaViewModel.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG.Count == 0)
            {
                ItemNameDic.ForEach(D =>
                {
                    resignUnpaidLeaveAgendaViewModel.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG.Add(new ResignUnpaidLeaveAgendaAffairsConfig()
                    {
                        ITEM_ID = D.Key,
                        ITEM_NAME = D.Value,
                        IS_CONSUMMATION = false,
                    });
                });
            }

            return resignUnpaidLeaveAgendaViewModel;
        }

        /// <summary>
        /// 離職、留職停薪_手續表(新增/修改/草稿)
        /// </summary>
        public bool PutResignUnpaidLeaveAgendaSingle(ResignUnpaidLeaveAgendaViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                var logonModel = new LogonModel();
                var ConstructionItemNameDic = new Dictionary<string, string>();

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                var FormAction = String.Empty;
                switch (model.RESIGN_UNPAID_LEAVE_AGENDA_CONFIG.FORM_ACTION)
                {
                    case "A": FormAction = "離職"; break;
                    case "B": FormAction = "留職停薪"; break;
                    default: break;
                }

                #region - 主旨 -                              

                var ParentDeptName = sysCommonRepository.GetGTVDeptTree().Where(GTV => GTV.DEPT_ID.Contains(model.APPLICANT_INFO.APPLICANT_DEPT)).Select(GTV => GTV.PARENT_DEPT_NAME).FirstOrDefault();
                if (String.IsNullOrEmpty(ParentDeptName) || String.IsNullOrWhiteSpace(ParentDeptName)) sysCommonRepository.GetGPIDeptTree().Where(GPI => GPI.DEPT_ID.Contains(model.APPLICANT_INFO.APPLICANT_DEPT)).Select(GPI => GPI.PARENT_DEPT_NAME).FirstOrDefault();

                FM7Subject = ParentDeptName + "_" + model.APPLICANT_INFO.APPLICANT_DEPT_NAME + "_" + model.APPLICANT_INFO.APPLICANT_ID + "_" + model.APPLICANT_INFO.APPLICANT_NAME + "_" + DateTime.Parse(model.RESIGN_UNPAID_LEAVE_AGENDA_CONFIG.RESIGN_DATE.ToString()).ToString("yyyy/MM/dd") + "_" + FormAction;

                #endregion

                #endregion

                #region - 離職、留職停薪_手續表 表頭資訊：ResignUnpaidLeaveAgenda_M -

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
                        //離職、留職停薪_手續表 表頭
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
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtA = dbFun.DoQuery(strSQL, parameterTitle);

                if (dtA.Rows.Count > 0)
                {
                    #region - 修改 -

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
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
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 離職、留職停薪_手續表 表單內容：ResignUnpaidLeaveAgenda_M -

                if (model.RESIGN_UNPAID_LEAVE_AGENDA_CONFIG != null)
                {
                    var parameterInfo = new List<SqlParameter>()
                        {
                            //離職、留職停薪_手續表 表單內容
                            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                            new SqlParameter("@FORM_ACTION", SqlDbType.NVarChar) { Size = 25, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@RESIGN_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@HANDOVER_SUPERVISOR_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@HANDOVER_SUPERVISOR_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@HANDOVER_SUPERVISOR_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@C01B_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@C01C_STR_DATE_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@C01F_STR_DATE_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@C01H_STR_DATE_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@C02_OTHERS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@C03_OTHERS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        };

                    logonModel.USER_ID = model.RESIGN_UNPAID_LEAVE_AGENDA_CONFIG.HANDOVER_SUPERVISOR_ID;
                    model.RESIGN_UNPAID_LEAVE_AGENDA_CONFIG.HANDOVER_SUPERVISOR_NAME = userRepository.PostUserSingle(logonModel).USER_MODEL.Select(U => U.USER_NAME).FirstOrDefault();

                    //寫入：離職、留職停薪_流程表 表單內容parameter                        
                    strJson = jsonFunction.ObjectToJSON(model.RESIGN_UNPAID_LEAVE_AGENDA_CONFIG);
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [FormAction]=@FORM_ACTION, ";
                    strSQL += "     [ResignDate]=@RESIGN_DATE, ";
                    strSQL += "     [HandoverSupervisorDeptID]=@HANDOVER_SUPERVISOR_DEPT_ID, ";
                    strSQL += "     [HandoverSupervisorID]=@HANDOVER_SUPERVISOR_ID, ";
                    strSQL += "     [HandoverSupervisorName]=@HANDOVER_SUPERVISOR_NAME, ";
                    strSQL += "     [C01B_Date]=@C01B_DATE, ";
                    strSQL += "     [C01C_StrDateTime]=@C01C_STR_DATE_TIME, ";
                    strSQL += "     [C01F_StrDateTime]=@C01F_STR_DATE_TIME, ";
                    strSQL += "     [C01H_StrDateTime]=@C01H_STR_DATE_TIME, ";
                    strSQL += "     [C02_Others]=@C02_OTHERS, ";
                    strSQL += "     [C03_Others]=@C03_OTHERS ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

                }

                #endregion

                #region - 離職、留職停薪_手續表 事務清單：ResignUnpaidLeaveAgenda_D -                              

                //IS_CONSUMMATION初始值都會是False
                var parameterAffairs = new List<SqlParameter>()
                    {
                        //離職、留職停薪_手續表 事務清單
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@ITEM_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_CONSUMMATION", SqlDbType.NVarChar) { Size = 10, Value = (object)false.ToString() ?? DBNull.Value },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACTER_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACTER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACTER_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SIGN_DATE", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                var strInsertSQL = "";
                strInsertSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D]([RequisitionID],[ItemID],[ItemName],[IsConsummation],[Description],[ContacterDeptID],[ContacterID],[ContacterName],[SignDate]) ";
                strInsertSQL += "VALUES(@REQUISITION_ID,@ITEM_ID,@ITEM_NAME,@IS_CONSUMMATION,@DESCRIPTION,@CONTACTER_DEPT_ID,@CONTACTER_ID,@CONTACTER_NAME,@SIGN_DATE) ";

                if (model.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG != null && model.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG.Count > 0)
                {
                    if (model.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG.Any(AFF => AFF.ITEM_ID.Contains("A") && AFF.CONTACTER_ID == model.APPLICANT_INFO.APPLICANT_ID))
                    {
                        CommLib.Logger.Error("離職、留職停薪_流程表(新增/修改/草稿)失敗，原因：所屬部門交接人不能是申請人。");
                        return false;                        
                    }
                    else if (model.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG.Where(AFF => !String.IsNullOrEmpty(AFF.CONTACTER_ID) || !String.IsNullOrWhiteSpace(AFF.CONTACTER_ID)).Count() > 0)
                    {
                        #region 先刪除舊資料

                        strSQL = "";
                        strSQL += "DELETE ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D] ";
                        strSQL += "WHERE 1=1 ";
                        strSQL += "          AND [RequisitionID]=@REQUISITION_ID ";

                        dbFun.DoTran(strSQL, parameterAffairs);

                        #endregion

                        model.RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG.Where(AFF => AFF.ITEM_ID.Contains("A")).ToList().ForEach(item =>
                        {
                            if (ItemNameDic.Keys.Contains(item.ITEM_ID))
                            {
                                logonModel.USER_ID = item.CONTACTER_ID;
                                item.ITEM_NAME = ItemNameDic.Where(Dic => Dic.Key.Contains(item.ITEM_ID)).Select(Dic => Dic.Value).FirstOrDefault();
                                item.CONTACTER_NAME = userRepository.PostUserSingle(logonModel).USER_MODEL.Select(U => U.USER_NAME).FirstOrDefault();
                                item.IS_CONSUMMATION = false;

                                //寫入：離職、留職停薪_手續表 事務清單parameter
                                strJson = jsonFunction.ObjectToJSON(item);
                                GlobalParameters.Infoparameter(strJson, parameterAffairs);

                                strSQL = "";
                                strSQL += strInsertSQL;

                                dbFun.DoTran(strSQL, parameterAffairs);

                                if (!ConstructionItemNameDic.Any(D => D.Key == item.ITEM_ID)) ConstructionItemNameDic.Add(item.ITEM_ID, item.ITEM_NAME);
                            }
                        });

                        #region - 清掉已新增的承辦事項 -

                        ConstructionItemNameDic.ForEach(DEL =>
                        {
                            ItemNameDic.Remove(DEL.Key);
                        });

                        #endregion

                        #region - 建立剩餘的事務清單 -

                        var newAffairsConfig = new NewAffairsConfig()
                        {
                            STR_SQL = strInsertSQL,
                            DICTIONARY = ItemNameDic,
                            PARAMETER = parameterAffairs
                        };
                        PutNewAffairsFunction(newAffairsConfig);

                        #endregion

                    }
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
                CommLib.Logger.Error("離職、留職停薪_流程表(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 離職、留職停薪_手續表(事項交接)
        /// </summary>
        public bool PutResignUnpaidLeaveAgendHandoverSingle(ResignUnpaidLeaveAgendHandoverSingle model)
        {
            var vResult = false;
            var HandoverToken = false;
            try
            {
                if (model.IS_CONSUMMATION == null) model.IS_CONSUMMATION = false;
                //開放C區通行
                if (model.ITEM_ID.Contains("C")) model.IS_CONSUMMATION = true;

                if (Boolean.Parse(model.IS_CONSUMMATION.ToString()))
                {
                    var parameter = new List<SqlParameter>()
                    {
                        //離職、留職停薪_手續表 事項交接
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                        new SqlParameter("@ITEM_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)model.ITEM_ID ?? DBNull.Value },
                        new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_CONSUMMATION", SqlDbType.NVarChar) { Size = 10, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DESCRIPTION", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACTER_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACTER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CONTACTER_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@SIGN_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@C01B_DATE", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@C01C_STR_DATE_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@C01F_STR_DATE_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@C01H_STR_DATE_TIME", SqlDbType.NVarChar) { Size = 50, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@C02_OTHERS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@C03_OTHERS", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                    };

                    strSQL = "";
                    strSQL += "SELECT ";
                    strSQL += "     [RequisitionID],";
                    strSQL += "     [ContacterID], ";
                    strSQL += "     [ContacterDeptID], ";
                    strSQL += "     [ContacterName], ";
                    strSQL += "     [SignDate] ";
                    strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D] ";
                    strSQL += "WHERE 1=1 ";
                    strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                    strSQL += "         AND [ItemID]=@ITEM_ID ";

                    var dt = dbFun.DoQuery(strSQL, parameter);

                    if (dt.Rows.Count > 0)
                    {
                        #region - 交接確認 -

                        if (model.ITEM_ID.Contains("A"))
                        {
                            //如果是A區塊(所屬單位：工作交接)的要比對交接人
                            if (dt.AsEnumerable().Any(R => R.Field<string>("ContacterID").Contains(model.CONTACTER_ID)))
                            {
                                HandoverToken = true;
                            }
                        }
                        else HandoverToken = true;

                        #endregion                        

                        if (HandoverToken)
                        {
                            var logonModel = new LogonModel()
                            {
                                USER_ID = model.CONTACTER_ID
                            };
                            model.CONTACTER_NAME = userRepository.PostUserSingle(logonModel).USER_MODEL.Where(U => U.DEPT_ID.Contains(model.CONTACTER_DEPT_ID)).Select(U => U.USER_NAME).FirstOrDefault();
                            model.SIGN_DATE = DateTime.Now.ToString("yyyy/MM/dd");

                            if (model.ITEM_ID.Contains("B05") || model.ITEM_ID.Contains("B06"))
                            {
                                if (!dt.AsEnumerable().Any(R => String.IsNullOrEmpty(R.Field<string>("ContacterID")) || String.IsNullOrWhiteSpace(R.Field<string>("ContacterID"))))
                                {
                                    var dtContacterID = dt.AsEnumerable().Select(R => R.Field<string>("ContacterID")).FirstOrDefault();
                                    var dtContacterName = dt.AsEnumerable().Select(R => R.Field<string>("ContacterName")).FirstOrDefault();
                                    var dtContacterDeptID = dt.AsEnumerable().Select(R => R.Field<string>("ContacterDeptID")).FirstOrDefault();
                                    var dtSignDate = dt.AsEnumerable().Select(R => R.Field<string>("SignDate")).FirstOrDefault();

                                    if (dt.AsEnumerable().Any(R => R.Field<string>("ContacterID").Contains(model.CONTACTER_ID) && dt.AsEnumerable().Any(R2 => R2.Field<string>("ContacterDeptID").Contains(model.CONTACTER_DEPT_ID))))
                                    {
                                        var ArrayContacterID = dtContacterID.Split(';');
                                        var ArraySignDate = dtSignDate.Split(';');
                                        var i = 0;
                                        while (ArrayContacterID.Count() > i)
                                        {
                                            if (ArrayContacterID[i].Contains(model.CONTACTER_ID)) break;
                                            i++;
                                        }
                                        dtContacterName = dtContacterName.Replace(model.CONTACTER_NAME, string.Empty).Replace(";", string.Empty);
                                        dtContacterDeptID = dtContacterDeptID.Replace(model.CONTACTER_DEPT_ID, string.Empty).Replace(";", string.Empty);
                                        dtContacterID = dtContacterID.Replace(model.CONTACTER_ID, string.Empty).Replace(";", string.Empty);
                                        dtSignDate = dtSignDate.Replace(ArraySignDate[i], string.Empty).Replace(";", string.Empty);
                                        //如果畫押日期是空的代表日期都一樣就把原本的日期寫回畫押日期
                                        if (String.IsNullOrEmpty(dtSignDate) || String.IsNullOrWhiteSpace(dtSignDate)) dtSignDate = ArraySignDate[i];
                                    }

                                    model.CONTACTER_NAME = dtContacterName + ";" + model.CONTACTER_NAME;
                                    model.CONTACTER_DEPT_ID = dtContacterDeptID + ";" + model.CONTACTER_DEPT_ID;
                                    model.CONTACTER_ID = dtContacterID + ";" + model.CONTACTER_ID;
                                    model.SIGN_DATE = dtSignDate + ";" + model.SIGN_DATE;
                                }
                            }

                            //寫入：離職、留職停薪_流程表 事項交接parameter                        
                            strJson = jsonFunction.ObjectToJSON(model);
                            GlobalParameters.Infoparameter(strJson, parameter);

                            strSQL = "";
                            strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D] ";
                            strSQL += "SET [IsConsummation]=@IS_CONSUMMATION, ";
                            strSQL += "     [Description]=@DESCRIPTION, ";
                            if (!model.ITEM_ID.Contains("A"))
                            {
                                strSQL += "     [ContacterDeptID]=@CONTACTER_DEPT_ID, ";
                                strSQL += "     [ContacterID]=@CONTACTER_ID, ";
                                strSQL += "     [ContacterName]=@CONTACTER_NAME, ";
                            }
                            strSQL += "     [SignDate]=@SIGN_DATE ";
                            strSQL += "WHERE 1=1 ";
                            strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                            strSQL += "         AND [ItemID]=@ITEM_ID ";
                            if (model.ITEM_ID.Contains("A")) strSQL += "         AND [ContacterID]=@CONTACTER_ID ";

                            if (model.ITEM_ID.Contains("C"))
                            {
                                strSQL += " ";
                                strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                                strSQL += "SET  ";
                                switch (model.ITEM_ID)
                                {
                                    case "C01":
                                        strSQL += "     [C01B_Date]=@C01B_DATE, ";
                                        strSQL += "     [C01C_StrDateTime]=@C01C_STR_DATE_TIME, ";
                                        strSQL += "     [C01F_StrDateTime]=@C01F_STR_DATE_TIME, ";
                                        strSQL += "     [C01H_StrDateTime]=@C01H_STR_DATE_TIME ";
                                        break;
                                    case "C02":
                                        strSQL += "     [C02_Others]=@C02_OTHERS ";
                                        break;
                                    case "C03":
                                        strSQL += "     [C03_Others]=@C03_OTHERS ";
                                        break;
                                    default: break;
                                }
                                strSQL += "WHERE 1=1 ";
                                strSQL += "         AND [RequisitionID]=@REQUISITION_ID ";
                            }

                            dbFun.DoTran(strSQL, parameter);

                            vResult = true;

                        }
                    }
                }

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("離職、留職停薪_手續表(事項交接)失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 建立新的事務清單
        /// </summary>
        public void PutNewAffairsFunction(NewAffairsConfig model)
        {
            try
            {
                var RemoveContacter = model.PARAMETER.Where(SP => SP.ParameterName.Contains("@CONTACTER")).ToList();
                RemoveContacter.ForEach(REMOVE =>
                {
                    model.PARAMETER.Remove(model.PARAMETER.Where(SP => SP.ParameterName.Contains(REMOVE.ParameterName)).FirstOrDefault());
                });
                model.PARAMETER.Add(new SqlParameter("@CONTACTER_DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value });
                model.PARAMETER.Add(new SqlParameter("@CONTACTER_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value });
                model.PARAMETER.Add(new SqlParameter("@CONTACTER_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)DBNull.Value ?? DBNull.Value });

                model.DICTIONARY.ForEach(D =>
                {
                    //定義parameter值
                    model.PARAMETER.Remove(model.PARAMETER.Where(SP => SP.ParameterName.Contains("@ITEM_ID")).FirstOrDefault());
                    model.PARAMETER.Remove(model.PARAMETER.Where(SP => SP.ParameterName.Contains("@ITEM_NAME")).FirstOrDefault());
                    model.PARAMETER.Add(new SqlParameter("@ITEM_ID", SqlDbType.NVarChar) { Size = 10, Value = (object)D.Key ?? DBNull.Value });
                    model.PARAMETER.Add(new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 10, Value = (object)D.Value ?? DBNull.Value });

                    strSQL = "";
                    strSQL += model.STR_SQL;

                    dbFun.DoTran(strSQL, model.PARAMETER);

                });
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("離職、留職停薪_流程表(建立新的事務清單)失敗，原因：" + ex.Message);
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
        /// 確認是否為新建的表單
        /// </summary>
        private bool IsADD = false;

        /// <summary>
        /// 表單代號
        /// </summary>
        private string IDENTIFY = "ResignUnpaidLeaveAgenda";

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

        #region - 承辦事項 -

        /// <summary>
        /// 承辦事項
        /// </summary>
        private Dictionary<string, string> ItemNameDic = new Dictionary<string, string>
        {
            { "A01", "工作交接" },
            { "A02", "文件/電子檔案移交" },
            { "A03", "鑰匙或其他辦公用品" },
            { "A04", "電腦(或密碼)移交" },
            { "B01", "片庫管理(新聞部-2F)" },
            { "B02", "片庫管理(工程部-4F)" },
            { "B03", "租借道具" },
            { "B04", "關閉帳號" },
            { "B05", "私人借款" },
            { "B06", "應收或其他帳款" },
            { "B07", "停車證" },
            { "B08", "其他固定資產" },
            { "B09", "出租和約" },
            { "B10", "印鑑保管人" },
            { "B11", "離職切結書" },
            { "B12", "員工識別證" },
            { "C01", "後續行政作業" },
            { "C02", "薪資發放狀況" },
            { "C03", "後續資訊作業" }
        };

        #endregion

        #endregion

    }
}