using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;


using log4net;

/// <summary> 
/// 共用參數設定
/// </summary> 
public class GlobalParameters
{
    #region - 檔案路徑 -

    public static string LogPath = @"C:\OA2_WEB_API_LOG\";

    #endregion

    #region - 參數設定 -

    /// <summary>軟體版本</summary>
    public static string DateBC = GetTaiwanDate();

    /// <summary>IP位置</summary>
    public static string NetUseIP = @"192.168.60.122";

    /// <summary>OA資料庫</summary>
    public const string sqlConnOADB = "OADB";

    /// <summary>新人類簽核資料庫(個人開發機)</summary>
    public const string sqlConnBPMProDevHo = "BPMProDevHo";
    /// <summary>新人類簽核附件路徑(個人開發機)</summary>
    public const string attachFilePathBPMProDevHo = @"http://song-vm-pc:82/";

    /// <summary>新人類簽核資料庫(公用開發機)</summary>
    public const string sqlConnBPMProDev = "BPMProDev";
    /// <summary>新人類簽核附件路徑(公用開發機)</summary>
    public const string attachFilePathBPMProDev = @"http://oa-web-dev01.gtv.com.tw:82/";

    /// <summary>新人類簽核資料庫(測試機)</summary>
    public const string sqlConnBPMProTest = "BPMProTest";
    /// <summary>新人類簽核附件路徑(測試機)</summary>
    public const string attachFilePathBPMProTest = @"http://oa-web-test02.gtv.com.tw:81/";

    /// <summary>新人類簽核資料庫(正式機)</summary>
    public const string sqlConnBPMPro = "BPMPro";
    /// <summary>新人類簽核附件路徑(正式機)</summary>
    public const string attachFilePathBPMPro = @"http://oa-web04.gtv.com.tw:81/Attachment/";

    /// <summary>
    /// 新人類簽核網址路徑
    /// </summary>
    public static string WebPathBPMPro(string connName)
    {
        var response = String.Empty;
        try
        {
            switch (connName)
            {
                case sqlConnOADB:
                    //OADB
                    break;

                case sqlConnBPMProDevHo:
                    response = "http://song-vm-pc:82/";
                    break;

                case sqlConnBPMProDev:
                    //response = "http://oa-web-dev01.gtv.com.tw:82/";
                    response = "http://192.168.1.219:82/";
                    break;

                case sqlConnBPMProTest:
                    response = "http://oa-web-test02.gtv.com.tw:81/";
                    break;

                case sqlConnBPMPro:
                    response = "http://oa-web04.gtv.com.tw:81/";
                    break;
            }
        }
        catch (Exception ex)
        {
            CommLib.Logger.Error("網址路徑比對失敗，原因：" + ex.Message);
            throw;
        }
        return response;
    }

    /// <summary>私鑰(64)</summary>
    public static string PrivateKey = @"6W5P8WZ97R32SK6N3SK5PBWMKYWJC96F4BGHELXEZ5CUQZSLH89L3LWNZFCXQUMR";
    /// <summary>過期時間(秒)</summary>
    public static double Expired = 3600;

    /// <summary>是否要發監控訊息</summary>
    public static bool IsTelegram = true;

    /// <summary>ERP API路徑</summary>
    public static string ERPSystemAPI = @"http://192.168.1.49:15500/ERP_API/";

    #endregion

    #region - 郵件伺服器 -

    /// <summary>郵件伺服器</summary>
    public static string MailServer = "gtvex01.gtv.com.tw";

    /// <summary>伺服器登入帳號</summary>
    public static string UID = @"GTV\app.pushmail";

    /// <summary>伺服器登入密碼</summary>
    public static string PWD = "8dgtv";

    #endregion

    #region - 共用方法 -

    /// <summary>
    /// 取得 民國年
    /// </summary>
    /// <returns>1030714</returns>
    public static string GetTaiwanDate()
    {
        CultureInfo cui = new CultureInfo("zh-TW", true);
        cui.DateTimeFormat.Calendar = new TaiwanCalendar();

        return DateTime.Now.ToString("yyMMdd", cui);
    }

    /// <summary>
    /// 取得 民國年
    /// </summary>
    /// <param name="TwDate"></param>
    /// <returns>1030714</returns>
    public static string GetTaiwanDate(DateTime TwDate)
    {
        CultureInfo cui = new CultureInfo("zh-TW", true);
        cui.DateTimeFormat.Calendar = new TaiwanCalendar();

        return TwDate.ToString("yyMMdd", cui);
    }

    /// <summary>
    /// 取得 民國年 轉 西元年
    /// </summary>
    /// <param name="TwDate">103/07/14</param>
    /// <returns>2014/07/14</returns>
    public static string GetUnTaiwanDate(string TwDate)
    {
        TwDate = TwDate.Substring(0, 3) + "/" + TwDate.Substring(3, 2) + "/" + TwDate.Substring(5, 2);

        CultureInfo cui = new CultureInfo("zh-TW");
        cui.DateTimeFormat.Calendar = new TaiwanCalendar();

        string CeDate = DateTime.Parse(TwDate, cui).Date.ToString("yyyy/MM/dd");

        return CeDate;
    }

    /// <summary>
    /// 取得 星期幾
    /// </summary>
    /// <param name="EnDate">西元年</param>
    public static string GetChtWeek(DateTime EnDate)
    {
        switch (EnDate.DayOfWeek.ToString())
        {
            case "Monday": return "(一)";
            case "Tuesday": return "(二)";
            case "Wednesday": return "(三)";
            case "Thursday": return "(四)";
            case "Friday": return "(五)";
            case "Saturday": return "(六)";
            case "Sunday": return "(日)";
            default: return "？";
        }
    }

    /// <summary>
    /// 布林 轉 int
    /// </summary>
    /// <returns>0 or 1</returns>
    public static int BoolToInt(bool TrueOrFalse)
    {
        int i = TrueOrFalse ? 1 : 0;

        return i;
    }

    /// <summary>
    ///  int 轉 布林
    /// </summary>
    /// <returns>true or false</returns>
    public static bool IntToBool(int num)
    {
        bool b = Convert.ToBoolean(num);

        return b;
    }

    /// <summary>
    /// 日期 NULL 轉 空值
    /// </summary>
    /// <param name="dateTime">日期</param>
    public static string DateTimeNullToEmpty(DateTime dateTime)
    {
        if (DateTime.Compare(dateTime, DateTime.MinValue) == 0)
        {
            return "";
        }
        else
        {
            return dateTime.ToString();
        }
    }

    /// <summary>
    /// 字串 NULL 轉 空值
    /// </summary>
    /// <param name="stringText">空字串</param>
    public static string StringNullToEmpty(String stringText)
    {
        return String.IsNullOrEmpty(stringText) ? "" : stringText;
    }

    /// <summary>
    /// 取得 User IP(外部)
    /// </summary>
    public static string GetUserIP()
    {
        #region 方法一

        /*
        HttpWebRequest request = HttpWebRequest.Create("http://api.ipify.org/?format=json") as HttpWebRequest;
        request.Method = "GET";
        request.ContentType = "application/x-www-form-urlencoded";
        request.UserAgent = "Mozilla/5.0";

        string _IP = String.Empty;
        string _JSON = String.Empty;

        WebResponse response = request.GetResponse();

        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            _JSON = reader.ReadToEnd();
        }

        JObject obj = JObject.Parse(_JSON);

        _IP = obj["ip"].ToString();

        return _IP;
       */

        #endregion

        #region 方法二

        HttpContext context = System.Web.HttpContext.Current;
        string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        string userIP = String.Empty;

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');

            if (addresses.Length != 0)
            {
                userIP = addresses[0];
            }
        }

        userIP = context.Request.ServerVariables["REMOTE_ADDR"];

        return userIP;

        #endregion
    }

    /// <summary>
    /// 取得 MD5 編碼驗證
    /// </summary>
    public static string HashToMD5(string str)
    {
        using (var cryptoMD5 = MD5.Create())
        {
            //將字串編碼成 UTF8 位元組陣列
            var bytes = Encoding.UTF8.GetBytes(str);

            //取得雜湊值位元組陣列
            var hash = cryptoMD5.ComputeHash(bytes);

            //取得 MD5
            var md5 = BitConverter.ToString(hash)
                                              .Replace("-", String.Empty)
                                              .ToUpper();

            return md5;
        }
    }

    /// <summary>
    /// 請求信息【WebServers作法】
    /// </summary>    
    public static string RequestInfoWebServers(string ApiUrl, string Method, string RequestJson)
    {
        try
        {
            var request = WebRequest.Create(ApiUrl) as HttpWebRequest;
            request.Method = Method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0";
            request.Timeout = 3000;

            NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
            var jss = new JavaScriptSerializer();
            var dictionary = jss.Deserialize<Dictionary<string, string>>(RequestJson);
            if (dictionary != null)
            {
                foreach (var k in dictionary)
                {
                    postParams.Add(k.Key, k.Value);
                }
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }

            var response = request.GetResponse() as HttpWebResponse;

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            CommLib.Logger.Error("請求信息失敗，原因：" + ex.Message);
             throw;
        }
    }

    /// <summary>
    /// 請求信息【WebAPI作法】
    /// </summary> 
    public static string RequestInfoWebAPI(string ApiUrl, string Method, Object RequestJson)
    {
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(ApiUrl);            
            request.Method = Method;
            request.ContentType = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.UserAgent = "Mozilla/5.0";
            request.Timeout = 3000; 

            //將匿名物件序列化為json字串
            string postBody = jsonFunction.ObjectToJSON(RequestJson);

            byte[] byteArray = Encoding.UTF8.GetBytes(postBody);

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }

            //發出Request            
            using (WebResponse response = request.GetResponse())
            {

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }

            }

        }
        catch (Exception ex)
        {
            CommLib.Logger.Error("請求信息失敗，原因：" + ex.Message);
            throw;
        }
    }

    /// <summary>
    /// 寫入parameter值
    /// </summary>
    public static void Infoparameter(string strJson, List<SqlParameter> parameters)
    {
        try
        {
            var jss = new JavaScriptSerializer();
            var dictionary = jss.Deserialize<Dictionary<string, string>>(strJson);

            foreach (var k in dictionary)
            {
                var SqlParameter = parameters.Where(P => P.ParameterName.Contains(k.Key)).FirstOrDefault();

                if (SqlParameter != null)
                {
                    SqlParameter.Value = k.Value;
                    
                    if(k.Value == null)
                    {
                        SqlParameter.Value = (object)DBNull.Value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommLib.Logger.Error("寫入parameter值失敗，原因：" + ex.Message);
            throw;
        }
    }

    /// <summary>
    /// 分頁
    /// </summary>
    public static IEnumerable<T> Pagination<T>(int Page, int PageSize, IList<T> list) where T : new()
    {
        //頁碼:0也就是第一條         
        if (Page <= 0)
        {
            Page = 0;
        }
        else
        {
            Page = Page - 1;
        }
        return list.Skip(Page * PageSize).Take(PageSize);
    }

    /// <summary>
    /// 表單內容檢視頁路徑
    /// </summary>    
    public static string FormContentPath(string RequisitionID, string Identify, string DiagramName)
    {
        var Base64Code = "RequisitionID=" + RequisitionID + "&Identify=" + Identify + "&DiagramName=" + HttpUtility.UrlEncode(DiagramName).ToUpper();

        return WebPathBPMPro(sqlConnBPMPro) + "BPMPro/FM7_FormContent_Redirect.aspx?EinB64=" + Convert.ToBase64String(Encoding.Default.GetBytes(Base64Code));
    }

    #endregion
}

/// <summary>
/// BPM簽核狀態
/// </summary>
public class BPMStatusCode
{
    /// <summary>新建</summary>
    public const string NEW_CREATE = "NewCreate";

    /// <summary>已簽完</summary>
    public const string CLOSE = "Close";

    /// <summary>不同意結束</summary>
    public const string DISAGREE_CLOSE = "DisagreeClose";

    /// <summary>簽核中</summary>
    public const string PROGRESS = "Progress";

    /// <summary>草稿</summary>
    public const string DRAFT = "Draft";

    /// <summary>失敗</summary>
    public const string FAIL = "Fail";

}

/// <summary>
/// BPM(系統)表單狀態代碼
/// </summary>
public class BPMSysStatus
{
    /// <summary>進行中</summary>
    public const string PROGRESS = "0";

    /// <summary>同意結束</summary>
    public const string CLOSE = "1";

    /// <summary>駁回結束</summary>
    public const string DISAGREE_CLOSE = "2";

    /// <summary>表單撤回</summary>
    public const string WITHDRAWAL = "4";

    /// <summary>異常表單</summary>
    public const string EXCEPTION = "5";

}

/// <summary> 
/// 靜態參數設定(log4)
/// </summary> 
public static class CommLib
{
    #region - log4 -

    public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    //public static readonly ILog LoggerWeb = LogManager.GetLogger("Web");

    #endregion
}

/// <summary>
/// DataTable 互轉 List(T) 物件
/// </summary>
public static class DataTableExtensions
{
    public static IList<T> ToList<T>(this DataTable table) where T : new()
    {
        IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
        IList<T> result = new List<T>();

        foreach (var row in table.Rows)
        {
            var item = CreateItemFromRow<T>((DataRow)row, properties);
            result.Add(item);
        }

        return result;
    }

    public static IList<T> ToList<T>(this DataTable table, Dictionary<string, string> mappings) where T : new()
    {
        IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
        IList<T> result = new List<T>();

        foreach (var row in table.Rows)
        {
            var item = CreateItemFromRow<T>((DataRow)row, properties, mappings);
            result.Add(item);
        }

        return result;
    }

    private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
    {
        T item = new T();

        try
        {
            foreach (var property in properties)
            {
                var propertyValue = row[property.Name];

                if (propertyValue != DBNull.Value)
                {
                    property.SetValue(item, row[property.Name], null);
                }
            }
        }
        catch (Exception ex)
        {
            CommLib.Logger.Error("DataTable 互轉 List(T) 物件錯誤，原因：" + ex.Message);
            throw;
        }

        return item;
    }

    private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties, Dictionary<string, string> mappings) where T : new()
    {
        T item = new T();

        foreach (var property in properties)
        {
            if (mappings.ContainsKey(property.Name))
            {
                property.SetValue(item, row[mappings[property.Name]], null);
            }
        }

        return item;
    }

    public static DataTable ToDataTable<T>(this IEnumerable<T> list)
    {
        List<PropertyInfo> pList = new List<PropertyInfo>();
        Type type = typeof(T);
        DataTable dt = new DataTable();

        Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });

        foreach (var item in list)
        {
            DataRow row = dt.NewRow();
            pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
            dt.Rows.Add(row);
        }

        return dt;
    }
}