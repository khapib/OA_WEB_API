﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Repository.ERP;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 外部起單
    /// </summary>
    public class GetExternalRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMPro);

        #region Model

        LogonModel UserIDmodel = new LogonModel();

        #endregion

        #region Repository

        UserRepository userRepository = new UserRepository();
        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        StepFlowRepository stepFlowRepository = new StepFlowRepository();

        #endregion

        #region FormRepository

        /// <summary>專案建立審核單</summary>
        ProjectReviewRepository projectReviewRepository = new ProjectReviewRepository();
        /// <summary>合作夥伴審核單</summary>
        SupplierReviewRepository supplierReviewRepository = new SupplierReviewRepository();
        /// <summary>費用申請單</summary>
        ExpensesReimburseRepository expensesReimburseRepository = new ExpensesReimburseRepository();

        #region 行政採購類

        /// <summary>行政採購申請單</summary>
        GeneralOrderRepository generalOrderRepository = new GeneralOrderRepository();
        /// <summary>行政採購異動申請單</summary>
        GeneralOrderChangeRepository generalOrderChangeRepository = new GeneralOrderChangeRepository();
        /// <summary>行政採購點驗收單</summary>
        GeneralAcceptanceRepository generalAcceptanceRepository = new GeneralAcceptanceRepository();
        /// <summary>行政採購請款單</summary>
        GeneralInvoiceRepository generalInvoiceRepository = new GeneralInvoiceRepository();
        /// <summary>行政採購退貨折讓單</summary>
        GeneralOrderReturnRefundRepository generalOrderReturnRefundRepository = new GeneralOrderReturnRefundRepository();

        #endregion

        #region 內容評估表

        #region 內容評估表

        /// <summary>內容評估表</summary>
        EvaluateContentRepository evaluateContentRepository = new EvaluateContentRepository();

        #endregion

        #region 內容評估表_補充意見

        /// <summary>內容評估表_補充意見</summary>
        EvaluateContentReplenishRepository evaluateContentReplenishRepository = new EvaluateContentReplenishRepository();

        #endregion

        #endregion

        #region 版權採購類
        /// <summary>版權採購申請單</summary>
        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();
        /// <summary>版權採購異動申請單</summary>
        MediaOrderChangeRepository mediaOrderChangeRepository = new MediaOrderChangeRepository();
        /// <summary>版權採購交片單</summary>
        MediaAcceptanceRepository mediaAcceptanceRepository = new MediaAcceptanceRepository();
        /// <summary>版權採購請款單</summary>
        MediaInvoiceRepository mediaInvoiceRepository = new MediaInvoiceRepository();
        /// <summary>版權採購退貨折讓單</summary>
        MediaOrderReturnRefundRepository mediaOrderReturnRefundRepository = new MediaOrderReturnRefundRepository();

        #endregion

        #region 四方四隅

        /// <summary>四方四隅_內容評估表</summary>
        GPI_EvaluateContentRepository GPI_evaluateContentRepository = new GPI_EvaluateContentRepository();
        /// <summary>四方四隅_內容評估表_補充意見</summary>
        GPI_EvaluateContentReplenishRepository GPI_evaluateContentReplenishRepository = new GPI_EvaluateContentReplenishRepository();

        #endregion

        #endregion

        #endregion

        #region - 方法 -

        #region - 專案建立審核單(外部起單) -

        /// <summary>
        /// 專案建立審核單(外部起單)
        /// </summary>
        public GetExternalData PutProjectReviewGetExternal(ProjectReviewERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "ProjectReview";

                strFormNo = model.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中
                if (!ApproveProgress.vResult)
                {
                    //沒有才起新單
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.START_ID
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    strJson = jsonFunction.ObjectToJSON(model);

                    #region - ERP專案建立審核單設定:ProjectReviewERPConfig -

                    //ERP專案建立審核單設定                

                    var projectReviewERPConfig = jsonFunction.JsonToObject<ProjectReviewErpConfig>(strJson);
                    projectReviewERPConfig.FORM_NO = model.ERP_FORM_NO;
                    projectReviewERPConfig.OWNER_DEP = applicantInfo.APPLICANT_DEPT_NAME;
                    projectReviewERPConfig.START_ID = applicantInfo.APPLICANT_NAME;

                    #endregion

                    #region - BPM專案建立審核單設定:ProjectReviewBPMConfig -

                    //ERP專案建立審核單設定
                    var projectReviewBpmConfig = new ProjectReviewBpmConfig()
                    {
                        FM7_SUBJECT = model.PROJECT_NAME + "-" + model.PROJECT_NICKNAME,
                        GAD_REVIEW = null,
                        ACC_CATEGORY = null
                    };

                    #endregion

                    #region - 送單 -

                    //送單
                    var projectReviewViewModel = new ProjectReviewViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        PROJECT_REVIEW_ERP_CONFIG = projectReviewERPConfig,
                        PROJECT_REVIEW_BPM_CONFIG = projectReviewBpmConfig
                    };

                    if (projectReviewRepository.PutProjectReviewSingle(projectReviewViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("專案建立審核單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 合作夥伴審核單(外部起單) -

        /// <summary>
        /// 合作夥伴審核單(外部起單)
        /// </summary>
        public GetExternalData PutSupplierReviewGetExternal(SupplierReviewERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "SupplierReview";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 合作夥伴審核單(表頭內容):SupplierReviewTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var supplierReviewTitle = jsonFunction.JsonToObject<SupplierReviewTitle>(strJson);
                    supplierReviewTitle.FORM_NO = strFormNo;
                    if (model.INFO == null)
                    {
                        supplierReviewTitle.APPROVE = "新增";
                    }
                    else
                    {
                        supplierReviewTitle.APPROVE = "修改";
                    }

                    #endregion

                    #region - 合作夥伴審核單設定(基本資料)(已審核):SupplierReviewConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var supplierReviewConfig = jsonFunction.JsonToObject<SupplierReviewDifference>(strJson);

                    #endregion

                    #region - 合作夥伴審核單設定(基本資料)(修改後/新增):SupplierReviewTempConfig -

                    strJson = jsonFunction.ObjectToJSON(model.TEMP_INFO);
                    var supplierReviewTempConfig = jsonFunction.JsonToObject<SupplierReviewConfig>(strJson);

                    #endregion

                    #region - 合作夥伴審核(銀行往來資訊)(已審核):SupplierReviewRemitInfo -

                    strJson = jsonFunction.ObjectToJSON(model.REMIT_INFO);
                    var supplierReviewRemitConfig = jsonFunction.JsonToObject<IList<SupplierReviewRemitDifference>>(strJson);

                    #endregion

                    #region - 合作夥伴審核(銀行往來資訊)(修改後/新增):SupplierReviewRemitTempInfo -

                    strJson = jsonFunction.ObjectToJSON(model.TEMP_REMIT_INFO);
                    var supplierReviewRemitTempConfig = jsonFunction.JsonToObject<IList<SupplierReviewRemitConfig>>(strJson);

                    #endregion

                    #region - 合作夥伴審核(附件)上傳:AttachmentConfig -

                    //附件的設定
                    strJson = jsonFunction.ObjectToJSON(model.ATTACHMENT);
                    var attachmentConfig = jsonFunction.JsonToObject<List<AttachmentConfig>>(strJson);
                    attachmentConfig.ForEach(Attachment =>
                    {
                        Attachment.CREATE_DATE = DateTime.Parse(Attachment.CREATE_DATE).ToString("yyyy/MM/dd HH:mm:ss");
                    });


                    #endregion

                    #region - 送單 -

                    //送單
                    var supplierReviewViewModel = new SupplierReviewDataViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        SUPPLIER_REVIEW_TITLE = supplierReviewTitle,
                        SUPPLIER_REVIEW_CONFIG = supplierReviewConfig,
                        SUPPLIER_REVIEW_TEMP_CONFIG = supplierReviewTempConfig,
                        SUPPLIER_REVIEW_REMIT_CONFIG = supplierReviewRemitConfig,
                        SUPPLIER_REVIEW_TEMP_REMIT_CONFIG = supplierReviewRemitTempConfig,
                        ATTACHMENT_CONFIG = attachmentConfig
                    };

                    if (supplierReviewRepository.PutSupplierReviewSingle(supplierReviewViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("合作夥伴審核單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 費用申請單_(外部起單) -

        /// <summary>
        /// 費用申請單(外部起單)
        /// </summary>
        public GetExternalData PutExpensesReimburseGetExternal(ExpensesReimburseERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "ExpensesReimburse";

                strFormNo = model.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 費用申請單 表頭資訊:MediaOrderTitle -

                    strJson = jsonFunction.ObjectToJSON(model);
                    var expensesReimburseTitle = jsonFunction.JsonToObject<ExpensesReimburseTitle>(strJson);
                    expensesReimburseTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 送單 -

                    //送單
                    var expensesReimburseViewModel = new ExpensesReimburseViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        EXPENSES_REIMBURSE_TITLE = expensesReimburseTitle,
                    };

                    if (expensesReimburseRepository.PutExpensesReimburseSingle(expensesReimburseViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("費用申請單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }



        #endregion

        #region - 行政採購類_(外部起單) -

        #region - 行政採購申請單(外部起單) -

        /// <summary>
        /// 行政採購申請單(外部起單)
        /// </summary>
        public GetExternalData PutGeneralOrderGetExternal(GeneralOrderERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GeneralOrder";

                strFormNo = model.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 行政採購申請單 表頭資訊:GeneralOrderTitle -

                    strJson = jsonFunction.ObjectToJSON(model);
                    var generalOrderTitle = jsonFunction.JsonToObject<GeneralOrderTitle>(strJson);
                    generalOrderTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 送單 -

                    //送單
                    var generalOrderViewModel = new GeneralOrderViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GENERAL_ORDER_TITLE = generalOrderTitle
                    };

                    if (generalOrderRepository.PutGeneralOrderSingle(generalOrderViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購申請單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購異動申請單(外部起單) -

        /// <summary>
        /// 行政採購異動申請單(外部起單)
        /// </summary>
        public GetExternalData PutGeneralOrderChangeGetExternal(GeneralOrderChangeERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GeneralOrderChange";

                strFormNo = model.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 查詢原單內容 -

                    ApproveFormQuery approveFormQuery = new ApproveFormQuery()
                    {
                        REQUISITION_ID = model.GROUP_ID
                    };
                    var ApproveForms = commonRepository.PostApproveForms(approveFormQuery);

                    var GroupSubject = String.Empty;
                    var GroupBPMFromNo = String.Empty;
                    foreach (var item in ApproveForms)
                    {
                        GroupSubject = item.FM7_SUBJECT;
                        GroupBPMFromNo = item.BPM_FROM_NO;
                    }

                    #endregion

                    #region - 行政採購異動申請單 表頭資訊:GeneralOrderChangeTitle -

                    strJson = jsonFunction.ObjectToJSON(model);
                    var generalOrderChangeConfig = jsonFunction.JsonToObject<GeneralOrderChangeConfig>(strJson);
                    generalOrderChangeConfig.FORM_NO = strFormNo;
                    generalOrderChangeConfig.MODIFY_FORM_NO = model.ERP_MODIFY_FORM_NO;

                    #region - 主旨(【異動X】-XXXX-XXXXXX-XXXX 問題排除) -

                    if (GroupSubject.Contains("【異動"))
                    {
                        GroupSubject = GroupSubject.Substring(GroupSubject.IndexOf("-", GroupSubject.IndexOf("-", GroupSubject.IndexOf("-") + 1) + 1));
                        GroupSubject = GroupSubject.Remove(0, 1);
                    }

                    #endregion

                    generalOrderChangeConfig.FM7_SUBJECT = "【異動" + model.MODIFY_NO + "】" + GroupBPMFromNo + "-" + GroupSubject;
                    generalOrderChangeConfig.GROUP_BPM_FORM_NO = GroupBPMFromNo;
                    generalOrderChangeConfig.FORM_ACTION = "修改";
                    generalOrderChangeConfig.PYMT_LOCK_PERIOD = model.LOCK_PERIOD;

                    #endregion

                    #region - 送單 -

                    //送單
                    var generalOrderChangeViewModel = new GeneralOrderChangeViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GENERAL_ORDER_CHANGE_CONFIG = generalOrderChangeConfig
                    };

                    if (generalOrderChangeRepository.PutGeneralOrderChangeSingle(generalOrderChangeViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購異動申請單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購點驗收單(外部起單) -

        /// <summary>
        /// 行政採購點驗收單(外部起單)
        /// </summary>
        public GetExternalData PutGeneralAcceptanceGetExternal(GeneralAcceptanceERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GeneralAcceptance";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 行政採購點驗收單(表頭內容):GeneralAcceptanceTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var generalAcceptanceTitle = jsonFunction.JsonToObject<GeneralAcceptanceTitle>(strJson);
                    generalAcceptanceTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 行政採購點驗收單(基本資料):GeneralAcceptanceConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var generalAcceptanceConfig = jsonFunction.JsonToObject<GeneralAcceptanceConfig>(strJson);

                    #endregion

                    #region - 行政採購點驗收單(驗收明細):GeneralAcceptanceDetailsConfig -

                    strJson = jsonFunction.ObjectToJSON(model.DTL);
                    var generalAcceptanceDetailsConfig = jsonFunction.JsonToObject<IList<GeneralAcceptanceDetailsConfig>>(strJson);

                    #endregion

                    #region - 送單 -

                    //送單
                    var generalAcceptanceViewModel = new GeneralAcceptanceViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GENERAL_ACCEPTANCE_TITLE = generalAcceptanceTitle,
                        GENERAL_ACCEPTANCE_CONFIG = generalAcceptanceConfig,
                        GENERAL_ACCEPTANCE_DETAILS_CONFIG = generalAcceptanceDetailsConfig
                    };

                    if (generalAcceptanceRepository.PutGeneralAcceptanceSingle(generalAcceptanceViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購點驗收單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購請款單(外部起單) -

        /// <summary>
        /// 行政採購請款單(外部起單)
        /// </summary>
        public GetExternalData PutGeneralInvoiceGetExternal(GeneralInvoiceERPInfo model)
        {

            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GeneralInvoice";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P2",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 行政採購請款單(表頭內容):GeneralInvoiceTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var generalInvoiceTitle = jsonFunction.JsonToObject<GeneralInvoiceTitle>(strJson);
                    generalInvoiceTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 行政採購請款單(表單內容):GeneralInvoiceConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var generalInvoiceConfig = jsonFunction.JsonToObject<GeneralInvoiceConfig>(strJson);

                    #endregion

                    #region - 送單 -

                    //送單
                    var generalInvoiceViewModel = new GeneralInvoiceViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GENERAL_INVOICE_TITLE = generalInvoiceTitle,
                        GENERAL_INVOICE_CONFIG = generalInvoiceConfig,

                    };

                    if (generalInvoiceRepository.PutGeneralInvoiceSingle(generalInvoiceViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購請款單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 行政採購退貨折讓單(外部起單) -

        /// <summary>
        /// 行政採購退貨折讓單(外部起單)
        /// </summary>
        public GetExternalData PutGeneralOrderReturnRefundGetExternal(GeneralOrderReturnRefundERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GeneralOrderReturnRefund";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P2",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 行政採購退貨折讓單(表頭內容):GeneralOrderReturnRefundInfoTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var generalOrderReturnRefundTitle = jsonFunction.JsonToObject<GeneralOrderReturnRefundTitle>(strJson);
                    generalOrderReturnRefundTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 行政採購退貨折讓單(表單內容):GeneralOrderReturnRefundInfoConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var generalOrderReturnRefundConfig = jsonFunction.JsonToObject<GeneralOrderReturnRefundConfig>(strJson);

                    #region - 行政採購請款單(查詢) 資訊 -

                    var generalInvoiceQueryModel = new GeneralInvoiceQueryModel()
                    {
                        REQUISITION_ID = generalOrderReturnRefundConfig.GENERAL_INVOICE_REQUISITION_ID
                    };
                    var generalInvoiceInfo = generalInvoiceRepository.PostGeneralInvoiceSingle(generalInvoiceQueryModel);

                    #endregion

                    generalOrderReturnRefundConfig.PERIOD = generalInvoiceInfo.GENERAL_INVOICE_CONFIG.PERIOD;
                    generalOrderReturnRefundConfig.FINANC_AUDIT_ID_1 = generalInvoiceInfo.GENERAL_INVOICE_CONFIG.FINANC_AUDIT_ID_1;
                    generalOrderReturnRefundConfig.FINANC_AUDIT_NAME_1 = generalInvoiceInfo.GENERAL_INVOICE_CONFIG.FINANC_AUDIT_NAME_1;
                    generalOrderReturnRefundConfig.FINANC_AUDIT_ID_2 = generalInvoiceInfo.GENERAL_INVOICE_CONFIG.FINANC_AUDIT_ID_2;
                    generalOrderReturnRefundConfig.FINANC_AUDIT_NAME_2 = generalInvoiceInfo.GENERAL_INVOICE_CONFIG.FINANC_AUDIT_NAME_2;

                    #endregion

                    #region - 行種採購退貨折讓單 憑證退款明細：GeneralOrderReturnRefundInvoicesConfig -

                    strJson = jsonFunction.ObjectToJSON(generalInvoiceInfo.GENERAL_INVOICE_INVS_CONFIG);
                    var generalOrderReturnRefundInvoicesConfig = jsonFunction.JsonToObject<List<GeneralOrderReturnRefundInvoicesConfig>>(strJson);
                    generalOrderReturnRefundInvoicesConfig.ForEach(INV =>
                    {
                        INV.EXCL = 0;
                        INV.EXCL_TWD = 0;
                        INV.TAX = 0;
                        INV.TAX_TWD = 0;
                        INV.NET = 0;
                        INV.NET_TWD = 0;
                        INV.GROSS = 0;
                        INV.GROSS_TWD = 0;
                        INV.AMOUNT = 0;
                        INV.AMOUNT_TWD = 0;
                        INV.NOTE = null;
                    });

                    #endregion

                    #region - 送單 -

                    //送單
                    var generalOrderReturnRefundViewModel = new GeneralOrderReturnRefundViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GENERAL_ORDER_RETURN_REFUND_TITLE = generalOrderReturnRefundTitle,
                        GENERAL_ORDER_RETURN_REFUND_CONFIG = generalOrderReturnRefundConfig,
                        GENERAL_ORDER_RETURN_REFUND_INVS_CONFIG = generalOrderReturnRefundInvoicesConfig,
                    };

                    if (generalOrderReturnRefundRepository.PutGeneralOrderReturnRefundSingle(generalOrderReturnRefundViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;

                        if (model.ALDY_RF_COMM != null)
                        {
                            #region - 行政採購退貨折讓單 已退貨商品明細: GeneralOrderReturnRefund_ALDY_RF_COMM -

                            var parameterAlreadyRefundCommoditys = new List<SqlParameter>()
                            {
                                //行政採購退貨折讓單 已退貨商品明細
                                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                                new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object) DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@MODEL", SqlDbType.NVarChar) { Size = 100, Value = (object) DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@SPECIFICATIONS", SqlDbType.NVarChar) { Size = 500, Value = (object) DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@UNIT", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                            };

                            var CommonALDY_RF_COMM = new BPMCommonModel<GeneralCommodityConfig>()
                            {
                                EXT = "ALDY_RF_COMM",
                                IDENTIFY = IDENTIFY,
                                PARAMETER = parameterAlreadyRefundCommoditys,
                                MODEL = model.ALDY_RF_COMM
                            };
                            commonRepository.PutGeneralCommodityFunction(CommonALDY_RF_COMM);

                            #endregion
                        }

                        #region - 行政採購退貨折讓單 憑證已退款細項：GeneralOrderReturnRefund_ALDY_INV_DTL -

                        var parameterInvoiceDetails = new List<SqlParameter>()
                        {
                            //行政採購退貨折讓單 憑證細項
                            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                            new SqlParameter("@GENERAL_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)generalInvoiceInfo.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_REQUISITION_ID ?? DBNull.Value },
                            new SqlParameter("@GENERAL_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)generalInvoiceInfo.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_BPM_FORM_NO ?? DBNull.Value },
                            new SqlParameter("@GENERAL_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)generalInvoiceInfo.GENERAL_INVOICE_CONFIG.GENERAL_ORDER_ERP_FORM_NO ?? DBNull.Value },
                            new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                        };

                        var CommonALDY_INV_DTL = new BPMCommonModel<GeneralOrderReturnRefundAlreadyInvoiceDetailsConfig>
                        {
                            EXT = "ALDY_INV_DTL",
                            IDENTIFY = IDENTIFY,
                            PARAMETER = parameterInvoiceDetails
                        };

                        if (model.ALDY_INV_DTL != null && model.ALDY_INV_DTL.Count > 0)
                        {
                            CommonALDY_INV_DTL.MODEL = model.ALDY_INV_DTL;
                        }
                        else
                        {
                            //如果沒有傳ALDY_INV_DTL就代表第一次起退貨折讓單，ALDY_INV_DTL就會抓請款單的INV_DTL。
                            CommonALDY_INV_DTL.MODEL = jsonFunction.JsonToObject<List<GeneralOrderReturnRefundAlreadyInvoiceDetailsConfig>>(jsonFunction.ObjectToJSON(generalInvoiceInfo.GENERAL_INVOICE_INV_DTLS_CONFIG));
                        }

                        commonRepository.PutInvoiceDetailFunction(CommonALDY_INV_DTL);

                        #endregion

                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("行政採購退貨折讓單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 內容評估表(外部起單) -

        #region - 內容評估表(外部起單) -

        /// <summary>
        /// 內容評估表(外部起單)
        /// </summary>
        public GetExternalData PutEvaluateContentGetExternal(EvaluateContentERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "EvaluateContent";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 內容評估表(表頭內容):EvaluateContentTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var evaluateContentTitle = jsonFunction.JsonToObject<EvaluateContentTitle>(strJson);
                    evaluateContentTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 內容評估表_外購(表單內容):EvaluateContentConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var evaluateContentConfig = jsonFunction.JsonToObject<EvaluateContentConfig>(strJson);

                    #endregion

                    #region - 內容評估表_外購(附件)上傳:AttachmentConfig -

                    //附件的設定
                    strJson = jsonFunction.ObjectToJSON(model.ATTACHMENT);
                    var attachmentConfig = jsonFunction.JsonToObject<List<AttachmentConfig>>(strJson);
                    attachmentConfig.ForEach(Attachment =>
                    {
                        Attachment.CREATE_DATE = DateTime.Parse(Attachment.CREATE_DATE).ToString("yyyy/MM/dd HH:mm:ss");
                    });


                    #endregion

                    #region - 送單 -

                    //送單
                    var evaluateContentViewModel = new EvaluateContentViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        EVALUATE_CONTENT_TITLE = evaluateContentTitle,
                        EVALUATE_CONTENT_CONFIG = evaluateContentConfig,
                        ATTACHMENT_CONFIG = attachmentConfig
                    };

                    if (evaluateContentRepository.PutEvaluateContentSingle(evaluateContentViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("內容評估表(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 內容評估表_補充意見(外部起單) -

        /// <summary>
        /// 內容評估表_補充意見(外部起單)
        /// </summary>
        public GetExternalData PutEvaluateContentReplenishGetExternal(EvaluateContentReplenishERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "EvaluateContentReplenish";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 內容評估表_補充意見(表頭內容):EvaluateContentReplenishTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var evaluateContentReplenishTitle = jsonFunction.JsonToObject<EvaluateContentReplenishTitle>(strJson);
                    evaluateContentReplenishTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 內容評估表_補充意見(表單內容):EvaluateContentReplenishConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var evaluateContentReplenishConfig = jsonFunction.JsonToObject<EvaluateContentReplenishConfig>(strJson);

                    #endregion

                    #region - 內容評估表_補充意見(附件)上傳:AttachmentConfig -

                    //附件的設定
                    strJson = jsonFunction.ObjectToJSON(model.ATTACHMENT);
                    var attachmentConfig = jsonFunction.JsonToObject<List<AttachmentConfig>>(strJson);
                    attachmentConfig.ForEach(Attachment =>
                    {
                        Attachment.CREATE_DATE = DateTime.Parse(Attachment.CREATE_DATE).ToString("yyyy/MM/dd HH:mm:ss");
                    });


                    #endregion

                    #region - 內容評估表_補充意見(關聯表單):AttachmentConfig -

                    //關聯表單的設定
                    strJson = jsonFunction.ObjectToJSON(model.ASSOCIATED_FORM_CONFIG);
                    var associatedFormConfig = jsonFunction.JsonToObject<List<AssociatedFormConfig>>(strJson);

                    #endregion

                    #region - 送單 -

                    //送單
                    var evaluateContentReplenishViewModel = new EvaluateContentReplenishViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        EVALUATE_CONTENT_REPLENISH_TITLE = evaluateContentReplenishTitle,
                        EVALUATE_CONTENT_REPLENISH_CONFIG = evaluateContentReplenishConfig,
                        ATTACHMENT_CONFIG = attachmentConfig,
                        ASSOCIATED_FORM_CONFIG = associatedFormConfig,
                    };

                    if (evaluateContentReplenishRepository.PutEvaluateContentReplenishSingle(evaluateContentReplenishViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("內容評估表_外購(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 版權採購類_(外部起單) -

        #region - 版權採購申請單(外部起單) -

        /// <summary>
        /// 版權採購申請單(外部起單)
        /// </summary>
        public GetExternalData PutMediaOrderGetExternal(MediaOrderERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "MediaOrder";

                strFormNo = model.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 版權採購申請單 表頭資訊:MediaOrderTitle -

                    strJson = jsonFunction.ObjectToJSON(model);
                    var mediaOrderTitle = jsonFunction.JsonToObject<MediaOrderTitle>(strJson);
                    mediaOrderTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 送單 -

                    //送單
                    var mediaOrderViewModel = new MediaOrderViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        MEDIA_ORDER_TITLE = mediaOrderTitle,
                    };

                    if (mediaOrderRepository.PutMediaOrderSingle(mediaOrderViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購申請單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 版權採購異動申請單(外部起單) -

        /// <summary>
        /// 版權採購異動申請單(外部起單)
        /// </summary>
        public GetExternalData PutMediaOrderChangeGetExternal(MediaOrderChangeERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "MediaOrderChange";

                strFormNo = model.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 查詢原單內容 -

                    ApproveFormQuery approveFormQuery = new ApproveFormQuery()
                    {
                        REQUISITION_ID = model.GROUP_ID
                    };
                    var ApproveForms = commonRepository.PostApproveForms(approveFormQuery);

                    var GroupSubject = String.Empty;
                    var GroupBPMFromNo = String.Empty;
                    foreach (var item in ApproveForms)
                    {
                        GroupSubject = item.FM7_SUBJECT;
                        GroupBPMFromNo = item.BPM_FROM_NO;
                    }

                    #endregion

                    #region - 版權採購異動申請單 表頭資訊:MediaOrderChangeTitle -

                    strJson = jsonFunction.ObjectToJSON(model);
                    var mediaOrderChangeConfig = jsonFunction.JsonToObject<MediaOrderChangeConfig>(strJson);
                    mediaOrderChangeConfig.FORM_NO = strFormNo;
                    mediaOrderChangeConfig.MODIFY_FORM_NO = model.ERP_MODIFY_FORM_NO;

                    #region - 主旨(【異動X】-XXXX-XXXXXX-XXXX 問題排除) -

                    if (GroupSubject.Contains("【異動"))
                    {
                        GroupSubject = GroupSubject.Substring(GroupSubject.IndexOf("-", GroupSubject.IndexOf("-", GroupSubject.IndexOf("-") + 1) + 1));
                        GroupSubject = GroupSubject.Remove(0, 1);
                    }

                    #endregion

                    mediaOrderChangeConfig.FM7_SUBJECT = "【異動" + model.MODIFY_NO + "】" + GroupBPMFromNo + "-" + GroupSubject;
                    mediaOrderChangeConfig.GROUP_BPM_FORM_NO = GroupBPMFromNo;
                    mediaOrderChangeConfig.FORM_ACTION = "修改";
                    mediaOrderChangeConfig.PYMT_LOCK_PERIOD = model.LOCK_PERIOD;

                    #endregion

                    #region - 送單 -

                    //送單
                    var mediaOrderChangeViewModel = new MediaOrderChangeViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        MEDIA_ORDER_CHANGE_CONFIG = mediaOrderChangeConfig
                    };

                    if (mediaOrderChangeRepository.PutMediaOrderChangeSingle(mediaOrderChangeViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購異動申請單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 版權採購交片單(外部起單) -

        /// <summary>
        /// 版權採購交片單(外部起單)
        /// </summary>
        public GetExternalData PutMediaAcceptanceGetExternal(MediaAcceptanceERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "MediaAcceptance";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 版權採購交片單(表頭內容):MediaAcceptanceTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var mediaAcceptanceTitle = jsonFunction.JsonToObject<MediaAcceptanceTitle>(strJson);
                    mediaAcceptanceTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 版權採購交片單(基本資料):MediaAcceptanceConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var mediaAcceptanceConfig = jsonFunction.JsonToObject<MediaAcceptanceConfig>(strJson);

                    #endregion

                    #region - 版權採購交片單(驗收明細):MediaAcceptanceDetailsConfig -

                    strJson = jsonFunction.ObjectToJSON(model.DTL);
                    var mediaAcceptanceDetailsConfig = jsonFunction.JsonToObject<List<MediaAcceptanceDetailsConfig>>(strJson);

                    #region - 預設 母帶受領日為 -

                    mediaAcceptanceDetailsConfig.ForEach(ACPT =>
                    {
                        ACPT.GET_MASTERING_DATE = DateTime.Today;
                    });

                    #endregion

                    #endregion

                    #region - 送單 -

                    //送單
                    var mediaAcceptanceViewModel = new MediaAcceptanceViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        MEDIA_ACCEPTANCE_TITLE = mediaAcceptanceTitle,
                        MEDIA_ACCEPTANCE_CONFIG = mediaAcceptanceConfig,
                        MEDIA_ACCEPTANCE_DTLS_CONFIG = mediaAcceptanceDetailsConfig,
                    };

                    if (mediaAcceptanceRepository.PutMediaAcceptanceSingle(mediaAcceptanceViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;


                        if (model.ALDY_RF_COMM != null)
                        {
                            #region - 版權採購交片單 已退貨商品明細: MediaAcceptance_ALDY_RF_COMM -

                            var parameterAlreadyRefundCommoditys = new List<SqlParameter>()
                            {
                                //版權採購退貨折讓單 已退貨商品明細
                                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                                new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                            };

                            var CommonALDY_RF_COMM = new BPMCommonModel<MediaCommodityConfig>()
                            {
                                EXT = "ALDY_RF_COMM",
                                IDENTIFY = IDENTIFY,
                                PARAMETER = parameterAlreadyRefundCommoditys,
                                MODEL = model.ALDY_RF_COMM
                            };
                            commonRepository.PutMediaCommodityFunction(CommonALDY_RF_COMM);

                            #endregion
                        }

                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購交片單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 版權採購請款單(外部起單) -

        /// <summary>
        /// 版權採購請款單(外部起單)
        /// </summary>
        public GetExternalData PutMediaInvoiceGetExternal(MediaInvoiceERPInfo model)
        {

            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "MediaInvoice";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P2",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 版權採購請款單(表頭內容):MediaInvoiceTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var mediaInvoiceTitle = jsonFunction.JsonToObject<MediaInvoiceTitle>(strJson);
                    mediaInvoiceTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 版權採購請款單(表單內容):MediaInvoiceConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var mediaInvoiceConfig = jsonFunction.JsonToObject<MediaInvoiceConfig>(strJson);

                    #endregion

                    #region - 送單 -

                    //送單
                    var mediaInvoiceViewModel = new MediaInvoiceViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        MEDIA_INVOICE_TITLE = mediaInvoiceTitle,
                        MEDIA_INVOICE_CONFIG = mediaInvoiceConfig,

                    };

                    if (mediaInvoiceRepository.PutMediaInvoiceSingle(mediaInvoiceViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion

            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購請款單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 版權採購退貨折讓單(外部起單) -

        /// <summary>
        /// 版權採購退貨折讓單(外部起單)
        /// </summary>
        public GetExternalData PutMediaOrderReturnRefundGetExternal(MediaOrderReturnRefundERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "MediaOrderReturnRefund";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P2",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 版權採購退貨折讓單(表頭內容):MediaOrderReturnRefundInfoTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var mediaOrderReturnRefundTitle = jsonFunction.JsonToObject<MediaOrderReturnRefundTitle>(strJson);
                    mediaOrderReturnRefundTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 版權採購退貨折讓單(表單內容):MediaOrderReturnRefundInfoConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var mediaOrderReturnRefundConfig = jsonFunction.JsonToObject<MediaOrderReturnRefundConfig>(strJson);

                    #region - 版權採購請款單(查詢) 資訊 -

                    var mediaInvoiceQueryModel = new MediaInvoiceQueryModel()
                    {
                        REQUISITION_ID = mediaOrderReturnRefundConfig.MEDIA_INVOICE_REQUISITION_ID
                    };
                    var mediaInvoiceInfo = mediaInvoiceRepository.PostMediaInvoiceSingle(mediaInvoiceQueryModel);

                    #endregion

                    mediaOrderReturnRefundConfig.PERIOD = mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.PERIOD;
                    mediaOrderReturnRefundConfig.FINANC_AUDIT_ID_1 = mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.FINANC_AUDIT_ID_1;
                    mediaOrderReturnRefundConfig.FINANC_AUDIT_NAME_1 = mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.FINANC_AUDIT_NAME_1;
                    mediaOrderReturnRefundConfig.FINANC_AUDIT_ID_2 = mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.FINANC_AUDIT_ID_2;
                    mediaOrderReturnRefundConfig.FINANC_AUDIT_NAME_2 = mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.FINANC_AUDIT_NAME_2;

                    #endregion

                    #region - 版權採購退貨折讓單 憑證退款明細：MediaOrderReturnRefundInvoicesConfig -

                    strJson = jsonFunction.ObjectToJSON(mediaInvoiceInfo.MEDIA_INVOICE_INVS_CONFIG);
                    var mediaOrderReturnRefundInvoicesConfig = jsonFunction.JsonToObject<List<MediaOrderReturnRefundInvoicesConfig>>(strJson);
                    mediaOrderReturnRefundInvoicesConfig.ForEach(INV =>
                    {
                        INV.EXCL = 0;
                        INV.EXCL_TWD = 0;
                        INV.TAX = 0;
                        INV.TAX_TWD = 0;
                        INV.NET = 0;
                        INV.NET_TWD = 0;
                        INV.GROSS = 0;
                        INV.GROSS_TWD = 0;
                        INV.AMOUNT = 0;
                        INV.AMOUNT_TWD = 0;
                        INV.NOTE = null;
                    });

                    #endregion

                    #region - 送單 -

                    //送單
                    var mediaOrderReturnRefundViewModel = new MediaOrderReturnRefundViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        MEDIA_ORDER_RETURN_REFUND_TITLE = mediaOrderReturnRefundTitle,
                        MEDIA_ORDER_RETURN_REFUND_CONFIG = mediaOrderReturnRefundConfig,
                        MEDIA_ORDER_RETURN_REFUND_INVS_CONFIG = mediaOrderReturnRefundInvoicesConfig,
                    };

                    if (mediaOrderReturnRefundRepository.PutMediaOrderReturnRefundSingle(mediaOrderReturnRefundViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;

                        if (model.ALDY_RF_COMM != null)
                        {
                            #region - 版權採購退貨折讓單 已退貨商品明細: MediaOrderReturnRefund_ALDY_RF_COMM -

                            var parameterAlreadyRefundCommoditys = new List<SqlParameter>()
                            {
                                //版權採購退貨折讓單 已退貨商品明細
                                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                                new SqlParameter("@ORDER_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@SUP_PROD_A_NO", SqlDbType.NVarChar) { Size = 500, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ITEM_NAME", SqlDbType.NVarChar) { Size = 100, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@MEDIA_SPEC", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@MEDIA_TYPE", SqlDbType.NVarChar) { Size = 64, Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@START_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@END_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ORDER_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@ACPT_EPISODE", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                                new SqlParameter("@EPISODE_TIME", SqlDbType.Int) { Value = (object) DBNull.Value ?? DBNull.Value },
                            };

                            var CommonALDY_RF_COMM = new BPMCommonModel<MediaCommodityConfig>()
                            {
                                EXT = "ALDY_RF_COMM",
                                IDENTIFY = IDENTIFY,
                                PARAMETER = parameterAlreadyRefundCommoditys,
                                MODEL = model.ALDY_RF_COMM
                            };
                            commonRepository.PutMediaCommodityFunction(CommonALDY_RF_COMM);

                            #endregion
                        }

                        #region - 版權採購退貨折讓單 憑證已退款細項：MediaOrderReturnRefund_ALDY_INV_DTL -

                        var parameterInvoiceDetails = new List<SqlParameter>()
                        {
                            //版權採購退貨折讓單 憑證明細
                            new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                            new SqlParameter("@MEDIA_ORDER_REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = (object)mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_REQUISITION_ID ?? DBNull.Value },
                            new SqlParameter("@MEDIA_ORDER_BPM_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_BPM_FORM_NO ?? DBNull.Value },
                            new SqlParameter("@MEDIA_ORDER_ERP_FORM_NO", SqlDbType.NVarChar) { Size = 20, Value = (object)mediaInvoiceInfo.MEDIA_INVOICE_CONFIG.MEDIA_ORDER_ERP_FORM_NO ?? DBNull.Value },
                            new SqlParameter("@PERIOD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@INV_ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@ROW_NO", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@NUM", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@NAME", SqlDbType.NVarChar) { Size = 50 , Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@QUANTITY", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@AMOUNT", SqlDbType.Float) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@AMOUNT_TWD", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value },
                            new SqlParameter("@IS_EXCL", SqlDbType.NVarChar) { Size = 5 , Value = (object)DBNull.Value ?? DBNull.Value },
                        };

                        var CommonALDY_INV_DTL = new BPMCommonModel<MediaOrderReturnRefundAlreadyInvoiceDetailsConfig>
                        {
                            EXT = "ALDY_INV_DTL",
                            IDENTIFY = IDENTIFY,
                            PARAMETER = parameterInvoiceDetails
                        };

                        if (model.ALDY_INV_DTL != null && model.ALDY_INV_DTL.Count > 0)
                        {
                            CommonALDY_INV_DTL.MODEL = model.ALDY_INV_DTL;
                        }
                        else
                        {
                            //如果沒有傳ALDY_INV_DTL就代表第一次起退貨折讓單，ALDY_INV_DTL就會抓請款單的INV_DTL。
                            CommonALDY_INV_DTL.MODEL = jsonFunction.JsonToObject<List<MediaOrderReturnRefundAlreadyInvoiceDetailsConfig>>(jsonFunction.ObjectToJSON(mediaInvoiceInfo.MEDIA_INVOICE_INV_DTLS_CONFIG));
                        }
                        commonRepository.PutInvoiceDetailFunction(CommonALDY_INV_DTL);

                        #endregion

                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("版權採購退貨折讓單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region - 四方四隅(外部起單) -

        #region - 四方四隅_內容評估表(外部起單) -

        /// <summary>
        /// 四方四隅_內容評估表(外部起單)
        /// </summary>
        public GetExternalData PutGPI_EvaluateContentGetExternal(GPI_EvaluateContentERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GPI_EvaluateContent";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 四方四隅_內容評估表(表頭內容):GPI_EvaluateContentTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var GPI_evaluateContentTitle = jsonFunction.JsonToObject<GPI_EvaluateContentTitle>(strJson);
                    GPI_evaluateContentTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 四方四隅_內容評估表_外購(表單內容):EvaluateContentConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var GPI_evaluateContentConfig = jsonFunction.JsonToObject<GPI_EvaluateContentConfig>(strJson);

                    #endregion

                    #region - 四方四隅_內容評估表_外購(附件)上傳:AttachmentConfig -

                    //附件的設定
                    strJson = jsonFunction.ObjectToJSON(model.ATTACHMENT);
                    var attachmentConfig = jsonFunction.JsonToObject<List<AttachmentConfig>>(strJson);
                    attachmentConfig.ForEach(Attachment =>
                    {
                        Attachment.CREATE_DATE = DateTime.Parse(Attachment.CREATE_DATE).ToString("yyyy/MM/dd HH:mm:ss");
                    });


                    #endregion

                    #region - 送單 -

                    //送單
                    var GPI_evaluateContentViewModel = new GPI_EvaluateContentViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GPI_EVALUATE_CONTENT_TITLE = GPI_evaluateContentTitle,
                        GPI_EVALUATE_CONTENT_CONFIG = GPI_evaluateContentConfig,
                        ATTACHMENT_CONFIG = attachmentConfig
                    };

                    if (GPI_evaluateContentRepository.PutGPI_EvaluateContentSingle(GPI_evaluateContentViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("四方四隅_內容評估表(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 四方四隅_內容評估表_補充意見(外部起單) -

        /// <summary>
        /// 四方四隅_內容評估表_補充意見(外部起單)
        /// </summary>
        public GetExternalData PutGPI_EvaluateContentReplenishGetExternal(GPI_EvaluateContentReplenishERPInfo model)
        {
            try
            {
                #region - 初始化宣告 -

                //表單ID
                IDENTIFY = "GPI_EvaluateContentReplenish";

                strFormNo = model.TITLE.ERP_FORM_NO;
                var request = new GTVInApproveProgress()
                {
                    FORM_NO = strFormNo,
                    IDENTIFY = IDENTIFY
                };

                //BPM 系統編號
                if (model.TITLE.BPM_REQ_ID == null)
                {
                    strREQ = Guid.NewGuid().ToString();

                }
                else
                {
                    strREQ = model.TITLE.BPM_REQ_ID;
                }

                #endregion

                #region 確認是否已起單且簽核中

                var ApproveProgress = commonRepository.PostGTVInApproveProgress(request);

                //確認是否已起單且簽核中或草稿中
                if (!ApproveProgress.vResult)
                {
                    #region - 起單 -

                    #region - 申請人資訊:ApplicantInfo -

                    //表單資訊
                    var applicantInfo = new ApplicantInfo()
                    {
                        REQUISITION_ID = strREQ,
                        DIAGRAM_ID = IDENTIFY + "_P1",
                        PRIORITY = 2,
                        DRAFT_FLAG = 0,
                        FLOW_ACTIVATED = 1
                    };

                    //申請人資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.APPLICANT_DEPT = item.DEPT_ID;
                        applicantInfo.APPLICANT_DEPT_NAME = item.DEPT_NAME;
                        applicantInfo.APPLICANT_ID = item.USER_ID;
                        applicantInfo.APPLICANT_NAME = item.USER_NAME;
                        applicantInfo.APPLICANT_PHONE = item.MOBILE;
                    }

                    //(填單人/代填單人)資訊
                    UserIDmodel = new LogonModel()
                    {
                        USER_ID = model.TITLE.CREATE_BY
                    };

                    foreach (UserModel item in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                    {
                        applicantInfo.FILLER_ID = item.USER_ID;
                        applicantInfo.FILLER_NAME = item.USER_NAME;
                    }

                    #endregion

                    #region - 四方四隅_內容評估表_補充意見(表頭內容):GPI_EvaluateContentReplenishTitle -

                    strJson = jsonFunction.ObjectToJSON(model.TITLE);
                    var GPI_evaluateContentReplenishTitle = jsonFunction.JsonToObject<GPI_EvaluateContentReplenishTitle>(strJson);
                    GPI_evaluateContentReplenishTitle.FORM_NO = strFormNo;

                    #endregion

                    #region - 四方四隅_內容評估表_補充意見(表單內容):GPI_EvaluateContentReplenishConfig -

                    strJson = jsonFunction.ObjectToJSON(model.INFO);
                    var GPI_evaluateContentReplenishConfig = jsonFunction.JsonToObject<GPI_EvaluateContentReplenishConfig>(strJson);

                    #endregion

                    #region - 四方四隅_內容評估表_補充意見(附件)上傳:AttachmentConfig -

                    //附件的設定
                    strJson = jsonFunction.ObjectToJSON(model.ATTACHMENT);
                    var attachmentConfig = jsonFunction.JsonToObject<List<AttachmentConfig>>(strJson);
                    attachmentConfig.ForEach(Attachment =>
                    {
                        Attachment.CREATE_DATE = DateTime.Parse(Attachment.CREATE_DATE).ToString("yyyy/MM/dd HH:mm:ss");
                    });


                    #endregion

                    #region - 四方四隅_內容評估表_補充意見(關聯表單):AttachmentConfig -

                    //關聯表單的設定
                    strJson = jsonFunction.ObjectToJSON(model.ASSOCIATED_FORM_CONFIG);
                    var associatedFormConfig = jsonFunction.JsonToObject<List<AssociatedFormConfig>>(strJson);

                    #endregion

                    #region - 送單 -

                    //送單
                    var GPI_evaluateContentReplenishViewModel = new GPI_EvaluateContentReplenishViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        GPI_EVALUATE_CONTENT_REPLENISH_TITLE = GPI_evaluateContentReplenishTitle,
                        GPI_EVALUATE_CONTENT_REPLENISH_CONFIG = GPI_evaluateContentReplenishConfig,
                        ATTACHMENT_CONFIG = attachmentConfig,
                        ASSOCIATED_FORM_CONFIG = associatedFormConfig,
                    };

                    if (GPI_evaluateContentReplenishRepository.PutGPI_EvaluateContentReplenishSingle(GPI_evaluateContentReplenishViewModel))
                    {
                        //起單成功
                        State = BPMStatusCode.PROGRESS;
                    }
                    else
                    {
                        //起單失敗
                        State = BPMStatusCode.FAIL;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    strREQ = ApproveProgress.REQUISITION_ID;
                    State = ApproveProgress.BPMStatus;
                }

                #endregion

                #region - 回傳狀態資訊 -

                var getExternalData = new GetExternalData()
                {
                    BPM_REQ_ID = strREQ,
                    ERP_FORM_NO = strFormNo,
                    STATE = State
                };

                return getExternalData;

                #endregion
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("內容評估表_外購(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #endregion

        #region - 欄位和屬性 -

        /// <summary>
        /// T-SQL
        /// </summary>
        private string strSQL;

        /// <summary>
        /// 表單代號
        /// </summary>
        private string IDENTIFY;

        /// <summary>
        /// Json字串
        /// </summary>
        private string strJson;

        #region - 回傳狀態 -

        private string requisitionID = Guid.NewGuid().ToString();

        /// <summary>
        /// 回傳狀態: 表單唯一碼
        /// </summary>
        private string strREQ;

        /// <summary>
        /// 回傳狀態: 表單唯一碼
        /// </summary>
        private string strFormNo;

        /// <summary>
        /// 回傳狀態: 新建 預設值
        /// </summary>
        private string State = BPMStatusCode.NEW_CREATE;

        #endregion

        #endregion

    }
}