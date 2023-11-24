using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;

using OA_WEB_API.Controllers;
using OA_WEB_API.Models.BPMPro;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Microsoft.Ajax.Utilities;
using OA_WEB_API.Repository.OA;
using System.Reflection;
using System.Web.Http.Results;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 停車證申請單
    /// </summary>
    public class ParkingPermitRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        #region Repository

        SysCommonController sysCommonController = new SysCommonController();
        FormRepository formRepository = new FormRepository();
        CommonRepository commonRepository = new CommonRepository();
        NotifyRepository notifyRepository = new NotifyRepository();
        UserRepository userRepository = new UserRepository();

        #endregion

        #endregion

        #region  - 方法 -

        /// <summary>
        /// 停車證申請單(查詢)
        /// </summary>
        public ParkingPermitViewModel PostParkingPermitSingle(ParkingPermitQueryModel query)
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

            #region - M表寫入BPM表單單號 -

            //避免儲存後送出表單BPM表單單號沒寫入的情形
            var formQuery = new FormQueryModel()
            {
                REQUISITION_ID = query.REQUISITION_ID
            };

            if (applicantInfo.DRAFT_FLAG == 0) notifyRepository.ByInsertBPMFormNo(formQuery);

            #endregion

            #region - 停車證申請單 表頭資訊 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [FM7Subject] AS [FM7_SUBJECT], ";
            strSQL += "     [BPMFormNo] AS [BPM_FORM_NO], ";
            strSQL += "     [Mobile] AS [MOBILE], ";
            strSQL += "     [CompanyName] AS [COMPANY_NAME], ";
            strSQL += "     [DeptID] AS [DEPT_ID], ";
            strSQL += "     [OfficeID] AS [OFFICE_ID], ";
            strSQL += "     [GroupID] AS [GROUP_ID], ";
            strSQL += "     [DeptName] AS [DEPT_NAME], ";
            strSQL += "     [OfficeName] AS [OFFICE_NAME], ";
            strSQL += "     [GroupName] AS [GROUP_NAME], ";
            strSQL += "     [RocYear] AS [ROC_YEAR], ";
            strSQL += "     [ApplicationCategory] AS [APPLICATION_CATEGORY], ";
            strSQL += "     [GuestName] AS [GUEST_NAME], ";
            strSQL += "     [GuestCompanyName] AS [GUEST_COMPANY_NAME], ";
            strSQL += "     [GuestDeptName] AS [GUEST_DEPT_NAME], ";
            strSQL += "     [GuestMobile] AS [GUEST_MOBILE] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var parkingPermitTitle = dbFun.DoQuery(strSQL, parameter).ToList<ParkingPermitTitle>().FirstOrDefault();

            #endregion

            #region - 停車證申請單 表單內容 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [VehicleCategory] AS [VEHICLE_CATEGORY], ";
            strSQL += "     [LicensePlateNumber] AS [LICENSE_PLATE_NUMBER], ";
            strSQL += "     CAST([IsChange] as bit) AS [IS_CHANGE], ";
            strSQL += "     [ChangeLicensePlateNumber] AS [CHANGE_LICENSE_PLATE_NUMBER], ";
            strSQL += "     [CarOwnerRelationship] AS [CAR_OWNER_RELATIONSHIP] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

            var parkingPermitConfig = dbFun.DoQuery(strSQL, parameter).ToList<ParkingPermitConfig>().FirstOrDefault();

            #endregion

            #region - 停車證申請單 圖片 -

            strSQL = "";
            strSQL += "SELECT ";
            strSQL += "     [ApplicationCategory] AS [APPLICATION_CATEGORY], ";
            strSQL += "     [VehicleCategory] AS [VEHICLE_CATEGORY], ";
            strSQL += "     [LicensePlateNumber] AS [LICENSE_PLATE_NUMBER], ";
            strSQL += "     [ImgIdentify] AS [IMG_IDENTIFY], ";
            strSQL += "     null AS [PHOTO] , ";
            strSQL += "     [FileRename] AS [FILE_RENAME], ";
            strSQL += "     [FileName] AS [FILE_NAME], ";
            strSQL += "     [FileExtension] AS [FILE_EXTENSION], ";
            strSQL += "     [FileSize] AS [FILE_SIZE], ";
            strSQL += "     [UploaderID] AS [UPLOADER_ID], ";
            strSQL += "     [UploadDateTime] AS [UPLOAD_DATETIME], ";
            strSQL += "     [DraftFlag] AS [DRAFT_FLAG] ";
            strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D] ";
            strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
            strSQL += "ORDER BY [AutoCounter] ";

            var parkingPermitImgsConfig = dbFun.DoQuery(strSQL, parameter).ToList<ParkingPermitImagesConfig>();

            #region - 確認檔案複製路徑 -

            var uploadFilePathModel = new UploadFilePathModel()
            {
                LOCATION = GlobalParameters.sqlConnBPMProTest,
                PATH = Path,
                IDENTIFY = IDENTIFY
            };

            var ImgPath = CommonRepository.PostUploadFilePath(uploadFilePathModel);

            #endregion

            parkingPermitImgsConfig.ForEach(IMG =>
            {
                var base64ImgModel = new Base64ImgModel()
                {
                    FILE_PATH = ImgPath + "\\" + IMG.FILE_RENAME,
                    FILE_EXTENSION = IMG.FILE_EXTENSION
                };
                IMG.PHOTO = commonRepository.PostBase64ImgOut(base64ImgModel);
            });

            #endregion

            var parkingPermitViewModel = new ParkingPermitViewModel()
            {
                APPLICANT_INFO = applicantInfo,
                PARKING_PERMIT_TITLE = parkingPermitTitle,
                PARKING_PERMIT_CONFIG = parkingPermitConfig,
                PARKING_PERMIT_IMGS_CONFIG = parkingPermitImgsConfig
            };

            #region - 確認表單 -

            if (parkingPermitViewModel.APPLICANT_INFO.DRAFT_FLAG == 0)
            {
                var formData = new FormData()
                {
                    REQUISITION_ID = query.REQUISITION_ID
                };

                if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
                {
                    parkingPermitViewModel = new ParkingPermitViewModel();
                    CommLib.Logger.Error("停車證申請單(查詢)失敗，原因：系統無正常起單。");
                }
                else
                {
                    #region - 確認M表BPM表單單號 -

                    //避免儲存後送出表單BPM表單單號沒寫入的情形
                    notifyRepository.ByInsertBPMFormNo(formQuery);

                    if (String.IsNullOrEmpty(parkingPermitViewModel.PARKING_PERMIT_TITLE.BPM_FORM_NO) || String.IsNullOrWhiteSpace(parkingPermitViewModel.PARKING_PERMIT_TITLE.BPM_FORM_NO))
                    {
                        strSQL = "";
                        strSQL += "SELECT ";
                        strSQL += "     [BPMFormNo] AS [BPM_FORM_NO] ";
                        strSQL += "FROM [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";
                        var dtBpmFormNo = dbFun.DoQuery(strSQL, parameter);
                        if (dtBpmFormNo.Rows.Count > 0) parkingPermitViewModel.PARKING_PERMIT_TITLE.BPM_FORM_NO = dtBpmFormNo.Rows[0][0].ToString();
                    }

                    #endregion
                }
            }

            #endregion

            return parkingPermitViewModel;
        }

        #region - 依此單內容重送 -

        /// <summary>
        /// 停車證申請單(依此單內容重送)(僅外部起單使用)
        /// </summary>        
        public bool PutParkingPermitRefill(ParkingPermitQueryModel query)
        {
            bool vResult = false;
            try
            {
                if (!String.IsNullOrEmpty(query.REQUISITION_ID) || !String.IsNullOrWhiteSpace(query.REQUISITION_ID))
                {
                    #region - 宣告 -

                    var original = PostParkingPermitSingle(query);
                    strJson = jsonFunction.ObjectToJSON(original);

                    var parkingPermitViewModel = new ParkingPermitViewModel();

                    var requisitionID = Guid.NewGuid().ToString();

                    #endregion

                    #region - 重送內容 -

                    parkingPermitViewModel = jsonFunction.JsonToObject<ParkingPermitViewModel>(strJson);

                    #region - 申請人資訊 調整 -

                    parkingPermitViewModel.APPLICANT_INFO.REQUISITION_ID = requisitionID;
                    parkingPermitViewModel.APPLICANT_INFO.DRAFT_FLAG = 1;
                    parkingPermitViewModel.APPLICANT_INFO.APPLICANT_DATETIME = DateTime.Now;

                    #endregion

                    #region - 停車證申請單 表頭資訊 調整 -

                    parkingPermitViewModel.PARKING_PERMIT_TITLE.FM7_SUBJECT = null;
                    parkingPermitViewModel.PARKING_PERMIT_TITLE.ROC_YEAR = null;
                    parkingPermitViewModel.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY = null;
                    parkingPermitViewModel.PARKING_PERMIT_TITLE.GROUP_NAME = null;
                    parkingPermitViewModel.PARKING_PERMIT_TITLE.GUEST_MOBILE = null;

                    #endregion

                    #region - 停車證申請單 表單內容 調整 -

                    parkingPermitViewModel.PARKING_PERMIT_CONFIG.VEHICLE_CATEGORY = null;
                    parkingPermitViewModel.PARKING_PERMIT_CONFIG.LICENSE_PLATE_NUMBER = null;
                    parkingPermitViewModel.PARKING_PERMIT_CONFIG.IS_CHANGE = false;
                    parkingPermitViewModel.PARKING_PERMIT_CONFIG.CHANGE_LICENSE_PLATE_NUMBER = null;
                    parkingPermitViewModel.PARKING_PERMIT_CONFIG.CAR_OWNER_RELATIONSHIP = null;

                    #endregion

                    #region - 停車證申請單 圖片 調整 -

                    parkingPermitViewModel.PARKING_PERMIT_IMGS_CONFIG = null;

                    #endregion

                    #endregion

                    #region - 送出 執行(新增/修改/草稿) -

                    PutParkingPermitSingle(parkingPermitViewModel);

                    #endregion

                    vResult = true;
                }
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("停車證申請單(依此單內容重送)失敗，原因" + ex.Message);
            }
            return vResult;
        }

        #endregion

        /// <summary>
        /// 停車證申請單(新增/修改/草稿)
        /// </summary>
        public bool PutParkingPermitSingle(ParkingPermitViewModel model)
        {
            bool vResult = false;
            try
            {
                #region - 宣告 -

                var ApplicationCategoryTW = String.Empty;
                var VehicleCategoryTW = String.Empty;
                var ChangeCategoryTW = String.Empty;
                var CompanyID = String.Empty;
                var ConcatenationDept = String.Empty;
                var ImgIdentifyTW = String.Empty;

                #region - 系統編號 -

                strREQ = model.APPLICANT_INFO.REQUISITION_ID;
                if (String.IsNullOrEmpty(strREQ) || String.IsNullOrWhiteSpace(strREQ))
                {
                    strREQ = Guid.NewGuid().ToString();
                }

                #endregion

                #region - 申請類別 -

                if (model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY == "A") ApplicationCategoryTW = "員工";
                else if (model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY == "B") ApplicationCategoryTW = "廠商";

                #endregion

                #region - 車種類別 -

                if (model.PARKING_PERMIT_CONFIG.VEHICLE_CATEGORY == "A") VehicleCategoryTW = "汽車";
                else if (model.PARKING_PERMIT_CONFIG.VEHICLE_CATEGORY == "B") VehicleCategoryTW = "機車";

                #endregion

                #region - 是否為異動 -

                if (String.IsNullOrEmpty(model.PARKING_PERMIT_CONFIG.IS_CHANGE.ToString()) || String.IsNullOrWhiteSpace(model.PARKING_PERMIT_CONFIG.IS_CHANGE.ToString())) model.PARKING_PERMIT_CONFIG.IS_CHANGE = false;

                if (!Boolean.Parse(model.PARKING_PERMIT_CONFIG.IS_CHANGE.ToString())) ChangeCategoryTW = "新辦";
                else ChangeCategoryTW = "異動";

                #endregion

                #region - 確認公司別 -

                CompanyID = sysCommonController.GetCompanyList().Where(C => C.COMPANY_NAME == model.PARKING_PERMIT_TITLE.COMPANY_NAME).Select(C => C.COMPANY_ID).FirstOrDefault();

                #endregion

                #region  - 申請人部門資訊 -

                var userStructure = userRepository.GetUsersStructure().Where(U => U.COMPANY_ID == CompanyID && U.USER_ID == model.APPLICANT_INFO.APPLICANT_ID && U.IS_MAIN_JOB==1).Select(U => U).FirstOrDefault();
                model.PARKING_PERMIT_TITLE.DEPT_ID = userStructure.DEPT_ID;
                model.PARKING_PERMIT_TITLE.OFFICE_ID = userStructure.OFFICE_ID;
                model.PARKING_PERMIT_TITLE.GROUP_ID = userStructure.GROUP_ID;
                model.PARKING_PERMIT_TITLE.DEPT_NAME = userStructure.DEPT_NAME;
                model.PARKING_PERMIT_TITLE.OFFICE_NAME = userStructure.OFFICE_NAME;
                model.PARKING_PERMIT_TITLE.GROUP_NAME = userStructure.GROUP_NAME;

                #endregion

                #region - 主旨 -

                FM7Subject = model.PARKING_PERMIT_TITLE.FM7_SUBJECT;

                if (FM7Subject == null)
                {
                    if (String.IsNullOrEmpty(model.PARKING_PERMIT_TITLE.OFFICE_NAME) || String.IsNullOrWhiteSpace(model.PARKING_PERMIT_TITLE.OFFICE_NAME)) ConcatenationDept = userStructure.DEPT_NAME + "_" + userStructure.GROUP_NAME;
                    else if (String.IsNullOrEmpty(model.PARKING_PERMIT_TITLE.GROUP_NAME) || String.IsNullOrWhiteSpace(model.PARKING_PERMIT_TITLE.GROUP_NAME)) ConcatenationDept = userStructure.DEPT_NAME + "_" + userStructure.OFFICE_NAME;
                    else ConcatenationDept = userStructure.DEPT_NAME + "_" + userStructure.OFFICE_NAME + "-" + userStructure.GROUP_NAME;

                    if ((String.IsNullOrEmpty(model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY) || String.IsNullOrWhiteSpace(model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY)) && (String.IsNullOrEmpty(model.PARKING_PERMIT_CONFIG.VEHICLE_CATEGORY) || String.IsNullOrWhiteSpace(model.PARKING_PERMIT_CONFIG.VEHICLE_CATEGORY)))
                    {
                        FM7Subject = "(汽車/機車)申請，車牌號碼：_____；" + ConcatenationDept + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                    }
                    else FM7Subject = ApplicationCategoryTW + VehicleCategoryTW + ChangeCategoryTW + "申請，車牌號碼：" + model.PARKING_PERMIT_CONFIG.LICENSE_PLATE_NUMBER.ToUpper() + "；" + ConcatenationDept + "_" + model.APPLICANT_INFO.APPLICANT_NAME;
                }



                #endregion

                #endregion

                #region - 停車證申請單 圖片上傳：ParkingPermit_D -

                if (model.PARKING_PERMIT_IMGS_CONFIG != null)
                {
                    #region - 確認檔案複製路徑 -

                    var uploadFilePathModel = new UploadFilePathModel()
                    {
                        LOCATION = GlobalParameters.sqlConnBPMProTest,
                        PATH = Path,
                        IDENTIFY = IDENTIFY
                    };

                    var ImgPath = CommonRepository.PostUploadFilePath(uploadFilePathModel);

                    #endregion

                    var parameterImages = new List<SqlParameter>()
                    {
                        //版權銷售申請單 銷售明細
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@APPLICATION_CATEGORY", SqlDbType.NVarChar) { Size = 5,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@VEHICLE_CATEGORY", SqlDbType.NVarChar) { Size = 5,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@LICENSE_PLATE_NUMBER", SqlDbType.NVarChar) { Size = 20,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IMG_IDENTIFY", SqlDbType.NVarChar) { Size = 5,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FILE_RENAME", SqlDbType.NVarChar) { Size = 200,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FILE_NAME", SqlDbType.NVarChar) { Size = 200,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FILE_EXTENSION", SqlDbType.NVarChar) { Size = 10,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@FILE_SIZE", SqlDbType.BigInt) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@UPLOADER_ID", SqlDbType.NVarChar) { Size = 40,Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@UPLOAD_DATETIME", SqlDbType.DateTime) { Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@DRAFT_FLAG", SqlDbType.Int) { Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    #region 先刪除舊檔案及資料

                    var organizeImgModel = new OrganizeImgModel()
                    {
                        EXT = "D",
                        FILE_PATH = ImgPath,
                        IDENTIFY = IDENTIFY,
                        REQUISITION_ID = strREQ
                    };

                    #endregion

                    if (commonRepository.PostOrganizeImg(organizeImgModel))
                    {
                        model.PARKING_PERMIT_IMGS_CONFIG.ToList().ForEach(IMG =>
                        {
                            #region - 子表宣告 -

                            IMG.APPLICATION_CATEGORY = model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY;
                            IMG.VEHICLE_CATEGORY = model.PARKING_PERMIT_CONFIG.VEHICLE_CATEGORY;
                            IMG.LICENSE_PLATE_NUMBER = model.PARKING_PERMIT_CONFIG.LICENSE_PLATE_NUMBER;
                            if (IMG.IMG_IDENTIFY == "A") ImgIdentifyTW = "行照";
                            else if (IMG.IMG_IDENTIFY == "B") ImgIdentifyTW = "身分證";
                            string[] ExtStrArray = IMG.FILE_EXTENSION.Split('/');
                            IMG.FILE_RENAME = Guid.NewGuid().ToString();
                            if (model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY == "A") IMG.FILE_NAME = ApplicationCategoryTW + "_" + model.APPLICANT_INFO.APPLICANT_NAME + "_" + VehicleCategoryTW + ":" + model.PARKING_PERMIT_CONFIG.LICENSE_PLATE_NUMBER + "_" + ImgIdentifyTW;
                            else if (model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY == "B") IMG.FILE_NAME = ApplicationCategoryTW + "_" + model.PARKING_PERMIT_TITLE.GUEST_NAME + "_" + VehicleCategoryTW + ":" + model.PARKING_PERMIT_CONFIG.LICENSE_PLATE_NUMBER + "_" + ImgIdentifyTW;
                            IMG.UPLOADER_ID = model.APPLICANT_INFO.APPLICANT_ID;
                            IMG.UPLOAD_DATETIME = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            IMG.DRAFT_FLAG = int.Parse(model.APPLICANT_INFO.DRAFT_FLAG.ToString());

                            #endregion

                            #region 新增檔案

                            var base64model = new Base64ImgSingletoSingleModel()
                            {
                                IMG_NAME = null,
                                PHOTO = IMG.PHOTO,
                                PRO_IMG_NAME = IMG.FILE_RENAME + "." + ExtStrArray[1],
                                FILE_PATH = ImgPath,
                                IMG_SIZE = null
                            };

                            commonRepository.PostBase64ImgSingletoSingle(base64model);

                            #endregion

                            #region 新增資料

                            IMG.PHOTO = String.Empty;
                            IMG.FILE_SIZE = new FileInfo(ImgPath + IMG.FILE_RENAME + "." + ExtStrArray[1]).Length;

                            strJson = jsonFunction.ObjectToJSON(IMG);

                            //寫入：停車證申請單 圖片上傳parameter
                            GlobalParameters.Infoparameter(strJson, parameterImages);

                            strSQL = "";
                            strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_D]([RequisitionID],[ApplicationCategory],[VehicleCategory],[LicensePlateNumber],[ImgIdentify],[FileRename],[FileName],[FileExtension],[FileSize],[UploaderID],[UploadDateTime],[DraftFlag]) ";
                            strSQL += "VALUES(@REQUISITION_ID,@APPLICATION_CATEGORY,@VEHICLE_CATEGORY,@LICENSE_PLATE_NUMBER,@IMG_IDENTIFY,@FILE_RENAME,@FILE_NAME,@FILE_EXTENSION,@FILE_SIZE,@UPLOADER_ID,@UPLOAD_DATETIME,@DRAFT_FLAG) ";
                            dbFun.DoTran(strSQL, parameterImages);

                            #endregion
                        });
                    }
                }

                #endregion

                #region - 停車證申請單 表頭資訊：ParkingPermit_M -

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
                    //停車證申請單 表頭
                    new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = FM7Subject ?? String.Empty },
                    new SqlParameter("@MOBILE", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.MOBILE ?? DBNull.Value },
                    new SqlParameter("@COMPANY_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.COMPANY_NAME ?? DBNull.Value },
                    new SqlParameter("@DEPT_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.DEPT_ID ?? DBNull.Value },
                    new SqlParameter("@OFFICE_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.OFFICE_ID ?? DBNull.Value },
                    new SqlParameter("@GROUP_ID", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.GROUP_ID ?? DBNull.Value },
                    new SqlParameter("@DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.DEPT_NAME ?? DBNull.Value },
                    new SqlParameter("@OFFICE_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.OFFICE_NAME ?? DBNull.Value },
                    new SqlParameter("@GROUP_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.GROUP_NAME ?? DBNull.Value },
                    new SqlParameter("@ROC_YEAR", SqlDbType.Int) { Value = (object)model.PARKING_PERMIT_TITLE.ROC_YEAR ?? DBNull.Value },
                    new SqlParameter("@APPLICATION_CATEGORY", SqlDbType.NVarChar) { Size = 5, Value = (object)model.PARKING_PERMIT_TITLE.APPLICATION_CATEGORY ?? DBNull.Value },
                    new SqlParameter("@GUEST_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.GUEST_NAME ?? DBNull.Value },
                    new SqlParameter("@GUEST_COMPANY_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.GUEST_COMPANY_NAME ?? DBNull.Value },
                    new SqlParameter("@GUEST_DEPT_NAME", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.GUEST_DEPT_NAME ?? DBNull.Value },
                    new SqlParameter("@GUEST_MOBILE", SqlDbType.NVarChar) { Size = 40, Value = (object)model.PARKING_PERMIT_TITLE.GUEST_MOBILE ?? DBNull.Value },

                };

                #region - 正常起單後 申請時間(APPLICANT_DATETIME) 不可覆蓋 -

                if (model.APPLICANT_INFO.DRAFT_FLAG == 0)
                {
                    var formData = new FormData()
                    {
                        REQUISITION_ID = strREQ
                    };

                    if (CommonRepository.PostFSe7enSysRequisition(formData).Count <= 0)
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
                    strSQL += "     [FM7Subject]=@FM7_SUBJECT, ";
                    strSQL += "     [Mobile]=@MOBILE, ";
                    strSQL += "     [CompanyName]=@COMPANY_NAME, ";
                    strSQL += "     [DeptID]=@DEPT_ID, ";
                    strSQL += "     [OfficeID]=@OFFICE_ID, ";
                    strSQL += "     [GroupID]=@GROUP_ID, ";
                    strSQL += "     [DeptName]=@DEPT_NAME, ";
                    strSQL += "     [OfficeName]=@OFFICE_NAME, ";
                    strSQL += "     [GroupName]=@GROUP_NAME, ";
                    strSQL += "     [RocYear]=@ROC_YEAR, ";
                    strSQL += "     [ApplicationCategory]=@APPLICATION_CATEGORY, ";
                    strSQL += "     [GuestName]=@GUEST_NAME, ";
                    strSQL += "     [GuestCompanyName]=@GUEST_COMPANY_NAME, ";
                    strSQL += "     [GuestDeptName]=@GUEST_DEPT_NAME, ";
                    strSQL += "     [GuestMobile]=@GUEST_MOBILE ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }
                else
                {
                    #region - 新增 -

                    strSQL = "";
                    strSQL += "INSERT INTO [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M]([RequisitionID],[DiagramID],[ApplicantDept],[ApplicantDeptName],[ApplicantID],[ApplicantName],[ApplicantPhone],[ApplicantDateTime],[FillerID],[FillerName],[Priority],[DraftFlag],[FlowActivated],[FM7Subject],[Mobile],[CompanyName],[DeptID],[OfficeID],[GroupID],[DeptName],[OfficeName],[GroupName],[RocYear],[ApplicationCategory],[GuestName],[GuestCompanyName],[GuestDeptName],[GuestMobile]) ";
                    strSQL += "VALUES(@REQUISITION_ID,@DIAGRAM_ID,@APPLICANT_DEPT,@APPLICANT_DEPT_NAME,@APPLICANT_ID,@APPLICANT_NAME,@APPLICANT_PHONE,@APPLICANT_DATETIME,@FILLER_ID,@FILLER_NAME,@PRIORITY,@DRAFT_FLAG,@FLOW_ACTIVATED,@FM7_SUBJECT,@MOBILE,@COMPANY_NAME,@DEPT_ID,@OFFICE_ID,@GROUP_ID,@DEPT_NAME,@OFFICE_NAME,@GROUP_NAME,@ROC_YEAR,@APPLICATION_CATEGORY,@GUEST_NAME,@GUEST_COMPANY_NAME,@GUEST_DEPT_NAME,@GUEST_MOBILE) ";

                    dbFun.DoTran(strSQL, parameterTitle);

                    #endregion
                }

                #endregion

                #region - 停車證申請單 表單內容：ParkingPermit_M -

                if (model.PARKING_PERMIT_CONFIG != null)
                {
                    strJson = jsonFunction.ObjectToJSON(model.PARKING_PERMIT_CONFIG);

                    var parameterInfo = new List<SqlParameter>()
                    {
                        //停車證申請單 表單內容
                        new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = strREQ },
                        new SqlParameter("@VEHICLE_CATEGORY", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@LICENSE_PLATE_NUMBER", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@IS_CHANGE", SqlDbType.NVarChar) { Size = 5, Value = (object)false.ToString() ?? DBNull.Value },
                        new SqlParameter("@CHANGE_LICENSE_PLATE_NUMBER", SqlDbType.NVarChar) { Size = 20, Value = (object)DBNull.Value ?? DBNull.Value },
                        new SqlParameter("@CAR_OWNER_RELATIONSHIP", SqlDbType.NVarChar) { Size = 5, Value = (object)DBNull.Value ?? DBNull.Value }
                    };

                    //寫入：停車證申請單 表單內容parameter                    
                    GlobalParameters.Infoparameter(strJson, parameterInfo);

                    strSQL = "";
                    strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + IDENTIFY + "_M] ";
                    strSQL += "SET [VehicleCategory]=@VEHICLE_CATEGORY, ";
                    strSQL += "     [LicensePlateNumber]=UPPER(@LICENSE_PLATE_NUMBER), ";
                    strSQL += "     [IsChange]=UPPER(@IS_CHANGE), ";
                    strSQL += "     [ChangeLicensePlateNumber]=@CHANGE_LICENSE_PLATE_NUMBER, ";
                    strSQL += "     [CarOwnerRelationship]=@CAR_OWNER_RELATIONSHIP ";
                    strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                    dbFun.DoTran(strSQL, parameterInfo);

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
                CommLib.Logger.Error("停車證申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
            }

            return vResult;
        }

        /// <summary>
        /// 停車證申請單(檔案、資料整理)
        /// </summary>
        public bool GetParkingPermitOrganize()
        {
            bool vResult = false;
            try
            {
                var organizeImgModel = new OrganizeImgModel()
                {
                    EXT = "D",
                    FILE_PATH = Path,
                    IDENTIFY = IDENTIFY,
                };

                vResult = commonRepository.PostInformationOrganize(organizeImgModel);
            }
            catch (Exception ex)
            {
                vResult = false;
                CommLib.Logger.Error("停車證申請單(新增/修改/草稿)失敗，原因：" + ex.Message);
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
        private string IDENTIFY = "ParkingPermit";

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

        /// <summary>
        /// 檔案路徑
        /// </summary>
        private string Path = "D:\\WebSites\\";

        #endregion
    }
}