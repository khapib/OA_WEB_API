using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

/// <summary> 
/// 機器人通報記錄類別
/// </summary> 
public class botFunction
{
    /// <summary>
    /// 推播訊息(非同步)(直發)
    /// </summary>
    public static void PushMessageAsync(string message)
    {
        try
        {
            if (GlobalParameters.IsTelegram)
            {
                int TelegramChatID = 1137783381;
                string TelegramTokenID = "1117202849:AAEgpIfENQf2zdMbeBQMIcG7tLrgrY7DC0Y";
                string TelegramURLTemp = String.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}", TelegramTokenID, TelegramChatID);
                string TelegramURL = TelegramURLTemp + "&text={0}";

                string webAPI = String.Format(TelegramURL, message);

                Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        await client.GetStringAsync(webAPI);
                    }
                }).Wait();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// 推播訊息(非同步)(內部轉發)
    /// </summary>
    public static void PushMessage(string message)
    {
        try
        {
            if (GlobalParameters.IsTelegram)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://192.168.1.99:10400/api/PushMessage");
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/json";

                var postData = new TLModel() { MESSAGE = message };
                var postBody = JsonConvert.SerializeObject(postData);
                var byteArray = Encoding.UTF8.GetBytes(postBody);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var responseStr = reader.ReadToEnd();
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}

/// <summary>
/// 推播訊息
/// </summary>
public class TLModel
{
    /// <summary>訊息</summary>
    public string MESSAGE { get; set; }
}