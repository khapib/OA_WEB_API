using System;
using System.IO;
using System.Data;
using System.Net.Mail;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 信件發送
/// </summary>
public class mailFunction
{
    #region - 宣告 -

    #endregion

    #region  - 方法  -

    /// <summary>
    /// 發送電子郵件
    /// </summary>
    public void SendEmail()
    {
        try
        {
            SmtpClient SMTP = new SmtpClient(GlobalParameters.MailServer);

            SMTP.UseDefaultCredentials = false;
            SMTP.Credentials = new System.Net.NetworkCredential(GlobalParameters.UID, GlobalParameters.PWD);
            SMTP.DeliveryMethod = SmtpDeliveryMethod.Network;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_From);

                #region - 收件人(一般) -

                string address = String.Empty;
                string displayName = String.Empty;
                string[] mailNames = (_To).Split(';');

                foreach (string name in mailNames)
                {
                    if (name != String.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = String.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }

                        mail.To.Add(new MailAddress(address, displayName));
                    }
                }

                #endregion

                #region - 收件人(副本) -

                address = String.Empty;
                displayName = String.Empty;
                mailNames = (_CC).Split(';');

                foreach (string name in mailNames)
                {
                    if (name != String.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = String.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }

                        mail.CC.Add(new MailAddress(address, displayName));
                    }
                }

                #endregion

                #region - 收件人(密件) -

                address = String.Empty;
                displayName = String.Empty;
                mailNames = (_BCC).Split(';');

                foreach (string name in mailNames)
                {
                    if (name != String.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = String.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }

                        mail.Bcc.Add(new MailAddress(address, displayName));

                        //mail.Bcc.Add(new MailAddress("digimiracle@gmail.com", "<原廠監控>"));
                    }
                }

                #endregion

                mail.Subject = Subject;
                mail.Body = Body;

                #region - 附件 -

                if (_Attachments != null && _Attachments != "")
                {
                    Attachment attachment = new Attachment(_Attachments);
                    mail.Attachments.Add(attachment);
                }

                #endregion

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                SMTP.Send(mail);

                //釋放資源
                mail.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommLib.Logger.Error("發送電子郵件失敗，原因：" + ex.Message);
        }
    }

    #endregion

    #region - 欄位和屬性 -

    /// <summary>
    /// 寄件人
    /// </summary>
    public string From
    {
        get { return _From; }
        set { _From = value; }
    }
    private string _From = String.Empty;

    /// <summary>
    /// 收件人 正本(以;分隔)
    /// </summary>
    public string To
    {
        get { return _To; }
        set { _To = value; }
    }
    private string _To = String.Empty;

    /// <summary>
    /// 收件人 副本(以;分隔)
    /// </summary>
    public string CC
    {
        get { return _CC; }
        set { _CC = value; }
    }
    private string _CC = String.Empty;

    /// <summary>
    /// 收件人 密件(以;分隔)
    /// </summary>
    public string BCC
    {
        get { return _BCC; }
        set { _BCC = value; }
    }
    private string _BCC = String.Empty;

    /// <summary>
    /// 信件主旨
    /// </summary>
    public string Subject
    {
        get { return _Subject; }
        set { _Subject = value; }
    }
    private string _Subject = String.Empty;

    /// <summary>
    /// 信件本文
    /// </summary>
    public string Body
    {
        get { return _Body; }
        set { _Body = value; }
    }
    private string _Body = String.Empty;

    /// <summary>
    /// 信件附件
    /// </summary>
    public string Attachments
    {
        get { return _Attachments; }
        set { _Attachments = value; }
    }
    private string _Attachments = String.Empty;

    /// <summary>
    /// 信件時間
    /// </summary>
    private string _SendTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

    /// <summary>
    /// 訊息
    /// </summary>
    public string Msg
    {
        get { return _Msg; }
        set { _Msg = value; }
    }
    private string _Msg;

    #endregion
}