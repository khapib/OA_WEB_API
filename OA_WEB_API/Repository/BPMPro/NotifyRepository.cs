using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using Dapper;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;

namespace OA_WEB_API.Repository.BPMPro
{
    /// <summary>
    /// 會簽管理系統 - 訊息通知
    /// </summary>
    public class NotifyRepository
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnBPMProTest);

        FormRepository formRepository = new FormRepository();
        FlowRepository flowRepository = new FlowRepository();
        UserRepository userRepository = new UserRepository();

        #endregion

        #region - 方法 -

        #region - 接收事件 -

        /// <summary>
        /// (待簽核)事件
        /// </summary>
        public void ByPending(FormQueryModel query)
        {
            #region STEP1：取得(待簽核)名單

            var nextApprover = flowRepository.PostNextApproverByPending(query);

            #endregion

            if (nextApprover != null && nextApprover.Count > 0)
            {
                #region STEP2：組裝收件人資料

                var emailList = new List<string>();
                var nameList = new List<string>();
                var emailOriginList = new List<string>();
                var nameOriginList = new List<string>();
                var guidList = new List<string>();

                foreach (var item in nextApprover)
                {
                    if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_NAME))
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailList.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_EMAIL))
                        {
                            emailOriginList.Add(String.Format("{0}<{1}>", item.ORIGIN_APPROVER_NAME, item.ORIGIN_APPROVER_EMAIL));
                        }

                        nameList.Add(String.Format("{0}(代理 {1})", item.APPROVER_NAME, item.ORIGIN_APPROVER_NAME));
                        nameOriginList.Add(item.ORIGIN_APPROVER_NAME);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailList.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        nameList.Add(item.APPROVER_NAME);
                    }

                    guidList.Add(item.APPROVER_GUID);
                }

                var emailListExcept = emailList.Except(emailOriginList).ToList();
                var nameListExcept = nameList.Except(nameOriginList).ToList();

                #endregion

                #region STEP3：發送(待處理)信件

                var modelA = new EmailModel()
                {
                    FROM_LIST = _FORM,
                    TO_LIST = String.Join(";", emailListExcept),
                    CC_LIST = String.Empty,
                    BCC_LIST = String.Empty,
                    SUBJECT = String.Format(formRepository.GetFormSubject(query), "待處理"),
                    CONTENT = GetEmailBody(query, String.Join("、", nameListExcept), enumProcess.PENDING),
                    FW3_TO_NAME = String.Join(";", nameListExcept)
                };

                SendEmail(modelA);

                #endregion

                #region STEP4：發送(通知)信件

                if (emailOriginList.Count > 0)
                {
                    var modelB = new EmailModel()
                    {
                        FROM_LIST = _FORM,
                        TO_LIST = String.Join(";", emailOriginList),
                        CC_LIST = String.Empty,
                        BCC_LIST = String.Empty,
                        SUBJECT = String.Format(formRepository.GetFormSubject(query), "通知"),
                        CONTENT = GetEmailBody(query, String.Join("、", nameOriginList), enumProcess.NOTIFY),
                        FW3_TO_NAME = String.Join(";", nameOriginList)
                    };

                    SendEmail(modelB);
                }

                #endregion

                #region STEP5：發送(待處理)簡訊

                if (query.IS_ENABLE_SMS == true)
                {
                    var phoneBookList = new List<PhoneBook>();

                    foreach (var item in nextApprover)
                    {
                        var phonebook = new PhoneBook()
                        {
                            TO_LIST = item.APPROVER_NAME,
                            PHONE_NO = item.APPROVER_PHONE
                        };

                        phoneBookList.Add(phonebook);
                    }

                    var modelC = new SmsModel()
                    {
                        REQUISITION_ID = query.REQUISITION_ID,
                        PHONE_BOOK_LIST = phoneBookList,
                        CONTENT = String.Format(formRepository.GetFormSubject(query), "待處理")
                    };

                    SendSMS(modelC);
                }

                #endregion

                #region STEP6：發送記錄更新：(次數+1、時間更新)

                flowRepository.PutLogNextApprover(guidList);

                #endregion
            }
        }

        /// <summary>
        /// (同意)事件
        /// </summary>
        public void ByAgree(FormQueryModel query)
        {
        }

        /// <summary>
        /// (不同意)事件
        /// </summary>
        public void ByDisagree(FormQueryModel query)
        {
        }

        /// <summary>
        /// (已逾時)事件
        /// </summary>
        public void ByOverTime(FormQueryModel query)
        {
            #region STEP1：取得(已逾時)名單

            var nextApprover = flowRepository.PostNextApproverByOverTime(query);

            #endregion

            if (nextApprover != null && nextApprover.Count > 0)
            {
                #region STEP2：組裝收件人資料

                var approverList = new List<string>();
                var emailList = new List<string>();
                var emailCCList = new List<string>();
                var emailCCTemp = new List<string>();
                var nameList = new List<string>();
                var nameTemp = new List<string>();
                var guidList = new List<string>();
                var sentCount = 0;
                var titleInfo = "已逾期";
                var company = String.Empty;

                foreach (var item in nextApprover)
                {
                    if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_NAME))
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailList.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_EMAIL))
                        {
                            emailList.Add(String.Format("{0}<{1}>", item.ORIGIN_APPROVER_NAME, item.ORIGIN_APPROVER_EMAIL));
                        }

                        nameList.Add(String.Format("{0}(代理 {1})", item.APPROVER_NAME, item.ORIGIN_APPROVER_NAME));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailList.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        nameList.Add(item.APPROVER_NAME);
                    }

                    approverList.Add(item.APPROVER_ID);
                    company = item.COMPANY_ID;
                    sentCount = item.SENT_COUNT;
                    guidList.Add(item.APPROVER_GUID);
                    titleInfo = String.Format("第{0}次逾期通知", sentCount);
                }

                switch (sentCount)
                {
                    case 1:  //第一次逾時：待簽核人+ 代理人

                        break;

                    case 2: //第二次逾時：待簽核人+ 代理人 + 上一級主管 + 02044

                        foreach (var approver in approverList)
                        {
                            var manager = flowRepository.PostSupervisor(new FlowQueryModel()
                            {
                                COMPANY_ID = company,
                                USER_ID = approver
                            }
                            ).FirstOrDefault();

                            emailCCTemp.Add(String.Format("{0}<{1}>", manager.USER_NAME, manager.EMAIL));
                            nameTemp.Add(manager.USER_NAME);
                        }

                        emailCCList.Add("何聖文<james@gtv.com.tw>;藍永利<leon@gtv.com.tw>");
                        emailCCTemp.Add("孫慶偉<top@gtv.com.tw>"); /*測試機在加經理*/

                        emailCCList = emailCCTemp.Distinct().ToList();

                        break;

                    case 3: //第三次逾時：系統管理者

                        emailList.Clear();
                        emailCCList.Clear();
                        emailCCList.Add("何聖文<james@gtv.com.tw>;藍永利<leon@gtv.com.tw>");

                        break;
                }

                var nameListUnion = nameList.Union(nameTemp.Distinct().ToList());

                #endregion

                #region STEP3：發送(已逾期)信件

                var model = new EmailModel()
                {
                    FROM_LIST = _FORM,
                    TO_LIST = String.Join(";", emailList),
                    CC_LIST = String.Join(";", emailCCList),
                    BCC_LIST = String.Empty,
                    SUBJECT = String.Format(formRepository.GetFormSubject(query), titleInfo),
                    CONTENT = GetEmailBody(query, String.Join("、", nameListUnion), enumProcess.OVER_TIME),
                    FW3_TO_NAME = String.Join(";", nameListUnion)
                };

                SendEmail(model);

                #endregion

                #region STEP4：發送記錄更新：(次數+1、時間更新)

                flowRepository.PutLogNextApprover(guidList);

                #endregion
            }
        }

        /// <summary>
        /// (結案)事件
        /// </summary>
        public void ByClose(FormQueryModel query)
        {
            #region STEP1：取得(結案)通知名單

            var finalApprover = flowRepository.PostNextApproverByClose(query);

            #endregion

            if (finalApprover != null && finalApprover.Count > 0)
            {
                #region STEP2：組裝收件人資料

                var emailList = new List<string>();
                var emailListTemp = new List<string>();
                var nameList = new List<string>();
                var nameListTemp = new List<string>();

                foreach (var item in finalApprover)
                {
                    if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_NAME))
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.ORIGIN_APPROVER_NAME, item.ORIGIN_APPROVER_EMAIL));
                        }

                        nameListTemp.Add(String.Format("{0}(代理 {1})", item.APPROVER_NAME, item.ORIGIN_APPROVER_NAME));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        nameListTemp.Add(item.APPROVER_NAME);
                    }
                }

                emailList = emailListTemp.Distinct().ToList();
                nameList = nameListTemp.Distinct().ToList();

                #endregion

                #region STEP3：發送(處理完畢)信件

                var model = new EmailModel()
                {
                    FROM_LIST = _FORM,
                    TO_LIST = String.Join(";", emailList),
                    CC_LIST = String.Empty,
                    BCC_LIST = String.Empty,
                    SUBJECT = String.Format(formRepository.GetFormSubject(query), "處理完畢通知"),
                    CONTENT = GetEmailBody(query, String.Join("、", nameList), enumProcess.CLOSE),
                    FW3_TO_NAME = String.Join(";", nameList)
                };

                SendEmail(model);

                #endregion

                #region STEP4：發送記錄：刪除該表單的發送記錄

                flowRepository.PutFormClose(query);

                #endregion
            }
        }

        /// <summary>
        /// (駁回結束)事件
        /// </summary>
        public void ByDisagreeClose(FormQueryModel query)
        {
            #region STEP1：取得(結案)通知名單

            var finalApprover = flowRepository.PostNextApproverByDisagreeClose(query);

            #endregion

            if (finalApprover != null && finalApprover.Count > 0)
            {
                #region STEP2：組裝收件人資料

                var emailList = new List<string>();
                var emailListTemp = new List<string>();
                var nameList = new List<string>();
                var nameListTemp = new List<string>();

                foreach (var item in finalApprover)
                {
                    if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_NAME))
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.ORIGIN_APPROVER_NAME, item.ORIGIN_APPROVER_EMAIL));
                        }

                        nameListTemp.Add(String.Format("{0}(代理 {1})", item.APPROVER_NAME, item.ORIGIN_APPROVER_NAME));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        nameListTemp.Add(item.APPROVER_NAME);
                    }
                }

                emailList = emailListTemp.Distinct().ToList();
                nameList = nameListTemp.Distinct().ToList();

                #endregion

                #region STEP3：發送(處理完畢)信件

                var model = new EmailModel()
                {
                    FROM_LIST = _FORM,
                    TO_LIST = String.Join(";", emailList),
                    CC_LIST = String.Empty,
                    BCC_LIST = String.Empty,
                    SUBJECT = String.Format(formRepository.GetFormSubject(query), "駁回結束通知"),
                    CONTENT = GetEmailBody(query, String.Join("、", nameList), enumProcess.DISAGREE_CLOSE),
                    FW3_TO_NAME = String.Join(";", nameList)
                };

                SendEmail(model);

                #endregion

                #region STEP4：發送記錄：刪除該表單的發送記錄

                flowRepository.PutFormClose(query);

                #endregion
            }
        }

        /// <summary>
        /// (特定人員)事件
        /// </summary>
        public void ByNotice(FormQueryModel query)
        {
            if (!String.IsNullOrEmpty(query.RECEIVER_ID))
            {
                #region STEP1：取得(特定人員)通知名單

                var receiverList = new List<ReceiverModel>();

                foreach (var userInfo in query.RECEIVER_ID.Split(';'))
                {
                    if (!String.IsNullOrEmpty(userInfo))
                    {
                        var item = userInfo.Split('@');
                        var userQuery = new UserQueryModel
                        {
                            DEPT_ID = item[1],
                            USER_ID = item[0]
                        };
                        var user = userRepository.PostUsers(userQuery).SingleOrDefault();
                        var receiver = new ReceiverModel()
                        {
                            DEPT_ID = user.DEPT_ID,
                            DEPT_NAME = user.DEPT_NAME,
                            USER_ID = user.USER_ID,
                            USER_NAME = user.USER_NAME,
                            EMAIL = user.EMAIL,
                            MOBILE = user.MOBILE
                        };

                        receiverList.Add(receiver);
                    }
                }

                #endregion

                if (receiverList != null && receiverList.Count > 0)
                {
                    #region STEP2：組裝收件人資料

                    var emailList = new List<string>();
                    var emailListTemp = new List<string>();
                    var nameList = new List<string>();
                    var nameListTemp = new List<string>();

                    foreach (var item in receiverList)
                    {
                        emailListTemp.Add(String.Format("{0}<{1}>", item.USER_NAME, item.EMAIL));
                        nameListTemp.Add(item.USER_NAME);
                    }

                    emailList = emailListTemp.Distinct().ToList();
                    nameList = nameListTemp.Distinct().ToList();

                    #endregion

                    #region STEP3：發送(特定通知)信件

                    var email = new EmailModel()
                    {
                        FROM_LIST = _FORM,
                        TO_LIST = String.Join(";", emailList),
                        CC_LIST = String.Empty,
                        BCC_LIST = String.Empty,
                        SUBJECT = String.Format(formRepository.GetFormSubject(query), "特定通知"),
                        CONTENT = GetEmailBody(query, String.Join("、", nameList), enumProcess.NOTIFY),
                        FW3_TO_NAME = String.Join(";", nameList)
                    };

                    SendEmail(email);

                    #endregion

                    #region STEP4：發送(特定通知)簡訊

                    if (query.IS_ENABLE_SMS == true)
                    {
                        var phoneBookList = new List<PhoneBook>();

                        foreach (var item in receiverList)
                        {
                            var phonebook = new PhoneBook()
                            {
                                TO_LIST = item.USER_NAME,
                                PHONE_NO = item.MOBILE
                            };

                            phoneBookList.Add(phonebook);
                        }

                        var model = new SmsModel()
                        {
                            REQUISITION_ID = query.REQUISITION_ID,
                            PHONE_BOOK_LIST = phoneBookList,
                            CONTENT = String.Format(formRepository.GetFormSubject(query), "特定通知")
                        };

                        SendSMS(model);
                    }

                    #endregion

                    #region STEP5：新增(知會通知)

                    foreach (var item in receiverList)
                    {
                        var notice = new NoticeMode()
                        {
                            REQUISITION_ID = query.REQUISITION_ID,
                            RECEIVER_ID = item.USER_ID,
                            RECEIVER_NAME = item.USER_NAME,
                            SUBJECT = String.Format(formRepository.GetFormSubject(query), "特定通知")
                        };

                        SetNotice(notice);
                    }

                    #endregion
                }
            }
        }

        #region - 2022/11/08 Leon: 接收流程引擎(特定知會通知)通知觸發事件 -

        /// <summary>
        /// (特定知會通知)事件
        /// </summary>
        public void ByInformNotify(InformNotifyModel inform)
        {
            var groupInformNotifyModel = new GroupInformNotifyModel
            {
                REQUISITION_ID = new List<string>
                {
                    inform.REQUISITION_ID
                },
                NOTIFY_BY = new List<string>
                {
                    inform.NOTIFY_BY
                },
                ROLE_ID = new List<string>
                {
                    inform.ROLE_ID
                }
            };
            ByGroupInformNotify(groupInformNotifyModel);
        }

        #endregion

        #region - 2022/11/08 Leon: (群體知會通知)通知觸發事件 -

        /// <summary>
        /// 群體知會通知
        /// </summary>
        public bool ByGroupInformNotify(GroupInformNotifyModel model)
        {
            bool vResult = false;
            try
            {
                ReceiverID = "";

                foreach (var requisitionID in model.REQUISITION_ID)
                {
                    #region - 被知會特定人員 -

                    if (String.IsNullOrWhiteSpace(ReceiverID))
                    {
                        #region - 被知會特定角色 -

                        foreach (var role in model.ROLE_ID)
                        {
                            if (role != null)
                            {
                                var RolesUserID = CommonRepository.GetRoles()
                                                            .Where(R => R.ROLE_ID.Contains(role))
                                                            .Select(R => R).ToList();
                                RolesUserID.ForEach(roleuser =>
                                {
                                    model.NOTIFY_BY.Add(roleuser.USER_ID);
                                });
                            }
                        }

                        #endregion

                        #region - 排除重複人員 -

                        model.NOTIFY_BY = model.NOTIFY_BY.GroupBy(N => N)
                                                        .Select(g => g.First()).ToList();

                        #endregion

                        #region - 排除 NOTIFY_BY List 是 null -

                        model.NOTIFY_BY = model.NOTIFY_BY.Where(N => N != null)
                                                            .Select(R => R)
                                                            .ToList();

                        #endregion

                        foreach (var notify in model.NOTIFY_BY)
                        {
                            var UserIDmodel = new LogonModel()
                            {
                                USER_ID = notify
                            };

                            foreach (var userInfo in userRepository.PostUserSingle(UserIDmodel).USER_MODEL)
                            {
                                ReceiverID += userInfo.USER_ID + "@" + userInfo.DEPT_ID + ";";
                            }
                        }
                        ReceiverID = ReceiverID.Substring(0, ReceiverID.Length - 1);
                    }

                    #endregion

                    var formQueryModel = new FormQueryModel()
                    {
                        REQUISITION_ID = requisitionID
                    };

                    if (CommonRepository.PostDataHaveForm(formQueryModel))
                    {
                        //表單資訊
                        var formData = formRepository.PostFormData(formQueryModel);

                        formQueryModel = new FormQueryModel()
                        {
                            IS_ENABLE_SMS = false,
                            RECEIVER_ID = ReceiverID,
                            REQUISITION_ID = requisitionID,
                            SERIAL_ID = formData.SERIAL_ID
                        };
                        //發特定人員通知及Email
                        ByNotice(formQueryModel);

                        vResult = true;
                    }
                    else
                    {
                        //無此表單資料
                        vResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("群體知會通知 通知失敗，原因：" + ex.Message);
                throw;
            }
            return vResult;
        }

        #endregion

        #region - 2022/12/19 Leon: (欄位確認後知會通知)通知觸發事件 -

        /// <summary>
        /// 欄位確認後知會通知
        /// </summary>
        /// <param name="Identify">識別編號(運用在M表)</param>
        /// <param name="IsImplement">要確認的欄位名稱</param>
        /// <returns></returns>
        public bool ByCheckNotify(CheckNotifyModel model)
        {
            bool vResult = false;

            try
            {
                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "     [" + model.IS_IMPLEMENT + "] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + model.IDENTIFY + "_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtSubject = dbFun.DoQuery(strSQL, parameter);
                var IsImplement = dtSubject.Rows[0][0].ToString();

                if (!String.IsNullOrEmpty(IsImplement) || !String.IsNullOrWhiteSpace(IsImplement))
                {
                    if (Boolean.Parse(IsImplement))
                    {
                        var groupInformNotifyModel = new GroupInformNotifyModel
                        {
                            REQUISITION_ID = new List<string>
                            {
                                model.REQUISITION_ID
                            },
                            NOTIFY_BY = new List<string>
                            {
                                model.NOTIFY_BY
                            },
                            ROLE_ID = new List<string>
                            {
                                model.ROLE_ID
                            }
                        };
                        ByGroupInformNotify(groupInformNotifyModel);

                    }
                    else CommLib.Logger.Debug("確認知會通知，" + model.IDENTIFY + " 無須知會通知。");
                }
                else CommLib.Logger.Debug("確認知會通知，" + model.IDENTIFY + " 通知為 '空值'。");

                vResult = true;

                return vResult;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("確認知會通知 通知失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// (結案打包)事件，包含 簽呈、會簽單、四方四隅簽呈
        /// </summary>
        public void ByArchive(FormQueryModel query)
        {
            #region STEP1：取得(結案)通知名單

            var finalApprover = flowRepository.PostNextApproverByClose(query);

            #endregion

            if (finalApprover != null && finalApprover.Count > 0)
            {
                #region STEP2：組裝收件人資料

                var emailList = new List<string>();
                var emailListTemp = new List<string>();
                var nameList = new List<string>();
                var nameListTemp = new List<string>();

                foreach (var item in finalApprover)
                {
                    if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_NAME))
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        if (!String.IsNullOrEmpty(item.ORIGIN_APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.ORIGIN_APPROVER_NAME, item.ORIGIN_APPROVER_EMAIL));
                        }

                        nameListTemp.Add(String.Format("{0}(代理 {1})", item.APPROVER_NAME, item.ORIGIN_APPROVER_NAME));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.APPROVER_EMAIL))
                        {
                            emailListTemp.Add(String.Format("{0}<{1}>", item.APPROVER_NAME, item.APPROVER_EMAIL));
                        }

                        nameListTemp.Add(item.APPROVER_NAME);
                    }
                }

                emailList = emailListTemp.Distinct().ToList();
                nameList = nameListTemp.Distinct().ToList();

                #endregion

                #region STEP3：發送(處理完畢)信件

                var modelA = new EmailModel()
                {
                    FROM_LIST = _FORM,
                    TO_LIST = "何聖文<james@gtv.com.tw>;藍永利<leon@gtv.com.tw>",
                    CC_LIST = String.Empty,
                    BCC_LIST = String.Empty,
                    SUBJECT = String.Format(formRepository.GetFormSubject(query), "處理完畢通知"),
                    CONTENT = GetEmailBody(query, String.Join("、", nameList), enumProcess.CLOSE),
                    FW3_TO_NAME = String.Join(";", nameList)
                };

                SendEmail(modelA);

                #endregion

                #region STEP4：發送(處理完畢)監控

                var formData = formRepository.PostFormData(query);

                var formMessage = String.Format("【{0}】結案打包通知\r\n表單編號：{1}\r\n申請人：{2}\r\n主旨：{3}\r\n核准日：{4}\r\n電腦 IP：{5}",
                    formData.DIAGRAM_NAME,
                    formData.SERIAL_ID,
                    formData.APPLICANT_NAME,
                    formData.FORM_SUBJECT,
                    DateTime.Now.ToString(),
                    System.Web.HttpContext.Current.Request.UserHostAddress
                );

                var modelB = new MessageModel()
                {
                    CONTENT = formMessage
                };

                SendTelegram(modelB);

                #endregion
            }
        }



        /// <summary>
        /// 申請(同意後)觸發事件 寫入BPM表單單號: M表 [BPMFormNo]
        /// </summary>
        public void ByInsertBPMFormNo(FormQueryModel query)
        {
            try
            {
                var formData = formRepository.PostFormData(query);

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID },
                     new SqlParameter("@IDENTIFY", SqlDbType.NVarChar) { Size = 50, Value = formData.IDENTIFY }
                };

                strSQL = "";
                strSQL += "EXEC [BPMPro].[dbo].[GTV_Sp_InsertBPMFormNo] @REQUISITION_ID, @IDENTIFY ";

                dbFun.DoTran(strSQL, parameter);
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("(流程事件)_寫入BPM表單單號 失敗，原因：" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 申請(同意後)觸發事件 新單 更新暫時主旨: M表 [FM7Subject]。
        /// 主旨如果跟BPM表單單號有關的需要跑此事件
        /// </summary>        
        public void ByUpdateFM7Subject(FormQueryModel query)
        {
            try
            {
                var formData = formRepository.PostFormData(query);

                var parameter = new List<SqlParameter>()
                {
                     new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = query.REQUISITION_ID }
                };

                strSQL = "";
                strSQL += "SELECT ";
                strSQL += "      [BPMFormNo] AS [BPM_FORM_NO], ";
                strSQL += "      [FM7Subject] AS [FM7_SUBJECT], ";
                strSQL += "      [GroupID] AS [GROUP_ID] ";
                strSQL += "FROM [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_M] ";
                strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                var dtSubject = dbFun.DoQuery(strSQL, parameter);
                var BPMFormNo = dtSubject.Rows[0][0].ToString();
                var FM7Subject = dtSubject.Rows[0][1].ToString();
                var GroupID = dtSubject.Rows[0][2].ToString();

                if (!String.IsNullOrEmpty(GroupID) || !String.IsNullOrWhiteSpace(GroupID))
                {
                    if (FM7Subject.Substring(1, 3) == "待填寫")
                    {
                        FM7Subject += BPMFormNo;

                        //表單主旨：FormHeader
                        FormHeader header = new FormHeader();
                        header.REQUISITION_ID = query.REQUISITION_ID;
                        header.ITEM_NAME = "Subject";
                        header.ITEM_VALUE = FM7Subject;

                        formRepository.PutFormHeader(header);

                        #region - 修改主旨 -

                        parameter.Add(new SqlParameter("@FM7_SUBJECT", SqlDbType.NVarChar) { Size = 64, Value = FM7Subject });

                        strSQL = "";
                        strSQL += "UPDATE [BPMPro].[dbo].[FM7T_" + formData.IDENTIFY + "_M] ";
                        strSQL += "SET [FM7Subject]=@FM7_SUBJECT ";
                        strSQL += "WHERE [RequisitionID]=@REQUISITION_ID ";

                        dbFun.DoTran(strSQL, parameter);

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("(流程事件)_新單 更新暫時主旨 失敗，原因：" + ex.Message);
                throw;
            }
        }

        #endregion

        #region - 基本資料準備 -

        /// <summary>
        /// 信件內文HTML(表單資料、簽核歷程)
        /// </summary>
        private string GetEmailBody(FormQueryModel query, string nameList, Enum process)
        {
            #region - 宣告 -

            var commRepository = new CommonRepository();
            var formRepository = new FormRepository();
            var flowRepository = new FlowRepository();

            var formData = formRepository.PostFormData(query);
            var flowData = flowRepository.PostFormSignOff(query);

            var _HTML = String.Empty;

            #endregion

            #region - 信件內文 -

            GetEmailBackgroundColor(process, out string processInfo, out string color1, out string color2, out string color3);

            _HTML += "";
            _HTML += "<style type='text/css'>";
            _HTML += "table { line-height: 12pt;font-size:15px; font-family: 微軟正黑體;width: 100%;border: 1px solid #fff;border-collapse:collapse; }";
            _HTML += "th { color:#fff;background-color: " + color1 + ";border: 2px solid #fff; padding: 5px; }";
            _HTML += "td { border: 2px solid #fff;padding: 5px; } ";
            _HTML += ".info { line-height: 23pt;font-weight:bold;font-size:18px;font-family: 微軟正黑體; }";
            _HTML += ".font01 { text-align:right; color:#fff; background-color: " + color1 + "; width: 15%; }";
            _HTML += ".font02 { background-color: " + color2 + ";width: 85%;}";
            _HTML += "</style>";
            _HTML += "<div class='info'>※ 信件通知 ※</div>";
            _HTML += "<table>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>收件人：</td>";
            _HTML += "        <td class='font02'>" + nameList + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>表單編號：</td>";
            _HTML += "        <td class='font02'>" + formData.SERIAL_ID + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>表單名稱：</td>";
            _HTML += "        <td class='font02'>" + formData.DIAGRAM_NAME + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>重要性：</td>";
            _HTML += "        <td class='font02'>" + formData.PRIORITY_TEXT + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>主旨：</td>";
            _HTML += "        <td class='font02'>" + formData.FORM_SUBJECT + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>申請人：</td>";
            _HTML += "        <td class='font02'>" + formData.APPLICANT_NAME + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>申請時間：</td>";
            _HTML += "        <td class='font02'>" + formData.APPLICANT_DATETIME + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>表單狀態：</td>";
            _HTML += "        <td class='font02'>" + processInfo + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>收件時間：</td>";
            _HTML += "        <td class='font02'>" + DateTime.Now.ToString() + "</td>";
            _HTML += "    </tr>";
            _HTML += "    <tr>";
            _HTML += "        <td class='font01'>查看內容：</td>";
            _HTML += "        <td class='font02'><a href='" + commRepository.GetWebBPM() + "FM7_MailApprove.aspx?RequisitionID=" + formData.REQUISITION_ID + "' target='_blank'>(請按此處) </a></td>";
            _HTML += "      </tr>";
            _HTML += "</table>";
            _HTML += "<div class='info'>※ 簽核流程 ※</div>";
            _HTML += "<table>";
            _HTML += "     <tr>";
            _HTML += "        <th style='width:15%;'>關卡流程</th>";
            _HTML += "        <th style='width:10%;'>部門</th>";
            _HTML += "        <th style='width:7%;'>姓名</th>";
            _HTML += "        <th style='width:20%;'>處理時間</th>";
            _HTML += "        <th style='width:13%;'>簽核</th>";
            _HTML += "        <th style='width:35%;'>簽核意見</th>";
            _HTML += "     </tr>";

            int i = 0;

            foreach (var item in flowData)
            {
                var rowColor = (i % 2 == 0) ? color3 : color2;

                _HTML += "<tr style='background-color: " + rowColor + "'>";
                _HTML += "   <td>" + item.PROCESS_NAME + "</td>";
                _HTML += "   <td>" + item.DEPT_NAME + "</td>";
                _HTML += "   <td align='center'>" + item.APPROVER_NAME + "</td>";
                _HTML += "   <td align='center'>" + item.APPROVER_TIME + "</td>";
                _HTML += "   <td align='center'>" + item.RESULT_PROMPT + "</td>";
                _HTML += "   <td>" + item.COMMENT + "</td>";
                _HTML += "</tr>";

                i++;
            }

            _HTML += "</table>";

            #endregion

            return _HTML;
        }

        /// <summary>
        /// 信件內文(背景顏色)
        /// </summary>
        private void GetEmailBackgroundColor(Enum process, out string processInfo, out string color1, out string color2, out string color3)
        {
            switch (process)
            {
                case enumProcess.PENDING:
                    processInfo = "進行中";
                    color1 = "#4D80E6";
                    color2 = "#EAFBFF";
                    color3 = "#ADD8E6";
                    break;

                case enumProcess.OVER_TIME:
                    processInfo = "已逾時";
                    color1 = "#FF7166";
                    color2 = "#FFF3D9";
                    color3 = "#FFD0B0";
                    break;

                case enumProcess.NOTIFY:
                    processInfo = "通知";
                    color1 = "#385623";
                    color2 = "#E2F4C9";
                    color3 = "#C6EA93";
                    break;

                case enumProcess.CLOSE:
                    //同意結束當下，接收到系統事件，但表單狀態還沒改變
                    processInfo = "處理完畢";
                    color1 = "#595959";
                    color2 = "#F2F2F2";
                    color3 = "#D9D9D9";
                    break;
                case enumProcess.DISAGREE_CLOSE:
                    processInfo = "駁回結束";
                    color1 = "#804040";
                    color2 = "#EBD6D6";
                    color3 = "#C48888";
                    break;
                default:
                    processInfo = "進行中";
                    color1 = "#4D80E6";
                    color2 = "#EAFBFF";
                    color3 = "#ADD8E6";
                    break;
            }
        }

        #endregion

        #region - 信件通知 -

        /// <summary>
        /// 傳送信件
        /// </summary>
        public void SendEmail(EmailModel model)
        {
            var maxSerialNo = GetMaxSerialNo();
            var parameter = new List<SqlParameter>
            {
                new SqlParameter("@AUTO_COUNTER", SqlDbType.BigInt) { Value = maxSerialNo.AUTO_COUNTER },
                new SqlParameter("@SUBJECT", SqlDbType.NVarChar) { Size = 200, Value = model.SUBJECT },
                new SqlParameter("@CONTENT", SqlDbType.NVarChar) { Size = -1, Value = model.CONTENT },
                new SqlParameter("@FROM_LIST", SqlDbType.NVarChar) { Size = 50, Value = model.FROM_LIST },
                new SqlParameter("@TO_LIST", SqlDbType.NVarChar) { Size = 500, Value = model.TO_LIST },
                new SqlParameter("@CC_LIST", SqlDbType.NVarChar) { Size = 500, Value = model.CC_LIST },
                //new SqlParameter("@BCC_LIST", SqlDbType.NVarChar) { Size = 500, Value = model.BCC_LIST },
                new SqlParameter("@BCC_LIST", SqlDbType.NVarChar) { Size = 500, Value = _BCC_LIST },
                new SqlParameter("@MAIL_TIME", SqlDbType.DateTime) { Size = 200, Value = DateTime.Now},
                new SqlParameter("@HASH_CODE", SqlDbType.Int) { Value = maxSerialNo.HASH_CODE },
                //new SqlParameter("@FW3_TO_LIST", SqlDbType.NVarChar) { Value = DBNull.Value },
                new SqlParameter("@FW3_TO_NAME", SqlDbType.NVarChar) { Value = model.FW3_TO_NAME },
                new SqlParameter("@PRIORITY", SqlDbType.SmallInt) { Value = 3 }
            };

            strSQL = "";
            strSQL += "INSERT INTO [BPMPro].[dbo].[FSe7en_EMail_Bank]([AutoCounter],[Subject],[Content],[FromList],[ToList],[CcList],[BccList],[MailTime],[HashCode],[FW3ToName],[Priority]) ";
            strSQL += "VALUES(@AUTO_COUNTER,@SUBJECT,@CONTENT,@FROM_LIST,@TO_LIST,@CC_LIST,@BCC_LIST,GETDATE(),@HASH_CODE,@FW3_TO_NAME,@PRIORITY)";

            dbFun.DoTran(strSQL, parameter);
        }

        /// <summary>
        /// 取得最大編號(AutoCounter)(HashCode)(0-255)
        /// </summary>
        private MaxSerialNo GetMaxSerialNo()
        {
            strSQL = "";
            strSQL += "SELECT TOP 1 (ISNULL([AutoCounter],0) + 1) AS [AUTO_COUNTER], (ISNULL([HashCode],0) + 1) AS [HASH_CODE] ";
            strSQL += "FROM [BPMPro].[dbo].[FSe7en_EMail_Bank] ";
            strSQL += "ORDER BY [AutoCounter] DESC ";

            var serialNo = dbFun.DoQuery(strSQL).ToList<MaxSerialNo>().FirstOrDefault();

            var maxNum = new MaxSerialNo()
            {
                AUTO_COUNTER = serialNo.AUTO_COUNTER,
                HASH_CODE = (serialNo.HASH_CODE > 255) ? 0 : serialNo.HASH_CODE
            };

            return maxNum;
        }

        #endregion

        #region - 知會通知 -

        /// <summary>
        /// 設定知會通知 
        /// </summary>
        public void SetNotice(NoticeMode model)
        {
            var maxSerialNo = GetMaxUniqueID();
            var parameter = new List<SqlParameter>
            {
                new SqlParameter("@UNIQUE_ID", SqlDbType.BigInt) { Value = maxSerialNo.UNIQUE_ID },
                new SqlParameter("@SENDER_ID", SqlDbType.NVarChar) { Size = 50, Value = "#SYSTEM!" },
                new SqlParameter("@SENDER_NAME", SqlDbType.NVarChar) { Size = 50, Value = "FlowSe7en System" },
                new SqlParameter("@MAIN_TYPE", SqlDbType.TinyInt) { Value = 1 },
                new SqlParameter("@SUB_TYPE", SqlDbType.TinyInt) { Value = 1 },
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) {  Size = 64, Value = model.REQUISITION_ID },
                new SqlParameter("@RECEIVER_ID", SqlDbType.NVarChar) { Size = 50, Value = model.RECEIVER_ID },
                new SqlParameter("@RECEIVER_NAME", SqlDbType.NVarChar) { Size = 50, Value = model.RECEIVER_NAME },
                new SqlParameter("@SUBJECT", SqlDbType.NVarChar) { Size = 255, Value = model.SUBJECT },
                new SqlParameter("@INFORMCONTENT", SqlDbType.NVarChar) { Size = -1, Value = DBNull.Value },
                new SqlParameter("@SUBMIT_DATETIME", SqlDbType.DateTime) { Value = DateTime.Now},
                new SqlParameter("@READ_FLAG", SqlDbType.TinyInt) { Value = 0 }
            };

            strSQL = "";
            strSQL += "INSERT INTO [BPMPro].[dbo].[FSe7en_Tep_Inform]([UniqueID],[SenderID],[SenderName],[MainType],[SubType],[RequisitionID],[ReceiverID],[ReceiverName],[Subject],[InformContent],[SubmitDateTime],[ReadFlag]) ";
            strSQL += "VALUES(@UNIQUE_ID,@SENDER_ID,@SENDER_NAME,@MAIN_TYPE,@SUB_TYPE,@REQUISITION_ID,@RECEIVER_ID,@RECEIVER_NAME,@SUBJECT,@INFORMCONTENT,@SUBMIT_DATETIME,@READ_FLAG)";

            dbFun.DoTran(strSQL, parameter);
        }

        /// <summary>
        /// 取得最大編號(UniqueID)(知會通知)
        /// </summary>
        private MaxUniqueID GetMaxUniqueID()
        {
            strSQL = "";
            strSQL += "SELECT ISNULL(MAX([UniqueID]) +1, 0) AS [UNIQUE_ID] ";
            strSQL += "FROM [BPMPro].[dbo].[FSe7en_Tep_Inform] ";

            var serialNo = dbFun.DoQuery(strSQL).ToList<MaxUniqueID>().FirstOrDefault();

            var maxNum = new MaxUniqueID()
            {
                UNIQUE_ID = serialNo.UNIQUE_ID
            };

            return maxNum;
        }

        #endregion

        #region - 簡訊通知 -

        /// <summary>
        /// 傳送簡訊(多筆)
        /// </summary>
        public void SendSMS(SmsModel model)
        {
            var smsFun = new smsFunction();
            smsFun.SendSMS(model);

            LogToSmsBank(model);
        }

        /// <summary>
        /// 簡訊發送記錄(單筆輸入)
        /// </summary>
        private void LogToSmsBank(SmsModel model)
        {
            var parameter = new List<SqlParameter>
             {
                new SqlParameter("@REQUISITION_ID", SqlDbType.NVarChar) { Size = 64, Value = model.REQUISITION_ID },
                new SqlParameter("@TO_LIST", SqlDbType.NVarChar) { Size = 20, Value = String.Empty },
                new SqlParameter("@PHONE_NO", SqlDbType.VarChar) { Size = 20, Value = String.Empty },
                new SqlParameter("@CONTENT", SqlDbType.Int) { Value = model.CONTENT }
            };

            foreach (var item in model.PHONE_BOOK_LIST)
            {
                parameter[1].Value = item.TO_LIST;
                parameter[2].Value = item.PHONE_NO;

                strSQL = "";
                strSQL += "INSERT INTO [BPMPro].[dbo].[FSe7en_FET_SMS_Bank]([RequisitionID],[Subject],[ToList],[PhoneNumber]) ";
                strSQL += "VALUES(@REQUISITION_ID,@CONTENT,@TO_LIST,@PHONE_NO)";

                dbFun.DoTran(strSQL, parameter);
            }
        }

        #endregion

        #region - 監控通知 -

        /// <summary>
        /// 傳送訊息(Telegram)
        /// </summary>
        public void SendTelegram(MessageModel model)
        {
            try
            {
                botFunction.PushMessageAsync(model.CONTENT);
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error(String.Format("傳送訊息失敗，原因：{0}", ex.Message));
            }
        }

        #endregion

        #endregion

        #region - 欄位和屬性 -

        /// <summary>T-SQL</summary>
        private string strSQL;

        /// <summary>寄件人</summary>
        private string _FORM = "會簽管理系統<Newtype@gtv.com.tw>";

        /// <summary>收件人</summary>
        //private string _TO_LIST = "何聖文<james@gtv.com.tw>;藍永利<leon@gtv.com.tw>";
        private string _TO_LIST = String.Empty;

        /// <summary>副本收件人</summary>
        private string _CC_LIST = String.Empty;

        /// <summary>密件收件人</summary>
        private string _BCC_LIST = "何聖文<james@gtv.com.tw>;藍永利<leon@gtv.com.tw>";

        /// <summary>特定人員</summary>
        private string ReceiverID;

        /// <summary>列舉 表單狀態</summary>
        private enum enumProcess
        {
            /// <summary>待簽核</summary>
            PENDING = 1,
            /// <summary>已逾時</summary>
            OVER_TIME = 2,
            /// <summary>通知</summary>
            NOTIFY = 5,
            /// <summary>結案</summary>
            CLOSE = 9,
            /// <summary>駁回結束</summary>
            DISAGREE_CLOSE = 4
        }

        #endregion
    }
}