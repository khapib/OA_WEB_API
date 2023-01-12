using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;
using OA_WEB_API.Models.ERP;
using OA_WEB_API.Repository.ERP;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 外部起單
    /// </summary>
    public class GetExternalRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProDevHo);

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

        #endregion

        #region 版權採購類
        /// <summary>版權採購申請單</summary>
        MediaOrderRepository mediaOrderRepository = new MediaOrderRepository();
        /// <summary>版權採購異動申請單</summary>
        MediaOrderChangeRepository mediaOrderChangeRepository = new MediaOrderChangeRepository();
        /// <summary>版權採購交片單</summary>
        MediaAcceptanceRepository mediaAcceptanceRepository = new MediaAcceptanceRepository();
        /// <summary>版權採購請款單</summary>
        MediaInvoiceRepository mediaInvoiceRepository =new MediaInvoiceRepository();

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
                CommLib.Logger.Error("版權採購申請單(外部起單)失敗，原因：" + ex.Message);
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
                        MEDIA_ORDER_TITLE= mediaOrderTitle,
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

                    #region - 版權採購交片單(驗收明細):mediaAcceptanceDetailsConfig -

                    strJson = jsonFunction.ObjectToJSON(model.DTL);
                    var mediaAcceptanceDetailsConfig = jsonFunction.JsonToObject<IList<MediaAcceptanceDetailsConfig>>(strJson);

                    #endregion

                    #region - 送單 -

                    //送單
                    var mediaAcceptanceViewModel = new MediaAcceptanceViewModel()
                    {
                        APPLICANT_INFO = applicantInfo,
                        MEDIA_ACCEPTANCE_TITLE = mediaAcceptanceTitle,
                        MEDIA_ACCEPTANCE_CONFIG = mediaAcceptanceConfig,
                        MEDIA_ACCEPTANCE_DTLS_CONFIG = mediaAcceptanceDetailsConfig
                    };

                    if (mediaAcceptanceRepository.PutMediaAcceptanceSingle(mediaAcceptanceViewModel))
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
                CommLib.Logger.Error("行政採購請款單(外部起單)失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #endregion

        #region - 欄位和屬性 -

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