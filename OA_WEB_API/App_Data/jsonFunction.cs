using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//using System.Net.Http;
//using System.Net;

/// <summary> 
/// JSON 轉換類別
/// </summary> 
public class jsonFunction
{
    #region - 共用方法 -

    #region DataTable

    /// <summary>
    /// Json 轉 DataTable
    /// </summary>
    public static DataTable JsonToDataTable(string strJSON)
    {
        DataTable dtJSON = JsonConvert.DeserializeObject<DataTable>(strJSON.Trim());

        return dtJSON;
    }

    /// <summary>
    /// DataTable 轉 ListJson
    /// </summary>
    public static string DataTableToJsonList(DataTable dtJSON)
    {
        string strJSON = JsonConvert.SerializeObject(dtJSON, Formatting.Indented);

        return strJSON;
    }

    #endregion

    #region Object

    /// <summary>
    /// Json 轉 物件
    /// </summary>
    /// <typeparam name="T">物件名稱</typeparam>
    /// <param name="strJSON">JSON</param>
    /// <returns>物件序列化</returns>
    public static List<T> ToList<T>(string strJSON)
    {
        List<T> obj = JsonConvert.DeserializeObject<List<T>>(strJSON);

        return obj;
    }

    /// <summary>
    /// 物件 轉 Json
    /// </summary>
    /// <param name="ogj">物件名稱</param>
    /// <returns>JSON</returns>
    public static string ObjectToJSON(object ogj)
    {
        string strJSON = JsonConvert.SerializeObject(ogj, Formatting.Indented);

        return strJSON;
    }

    #endregion

    //#region External JSON

    ///// <summary>
    ///// 外部 JSON
    ///// </summary>
    ///// <param name="URL">網址</param>
    ///// <returns></returns>
    //public string GetExternalJson(string URL)
    //{
    //    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
    //    System.Net.Http.HttpResponseMessage response = client.GetAsync(URL).Result;

    //    string strJSON = response.Content.ReadAsStringAsync().Result.ToString();

    //    return strJSON;
    //}

    //#endregion

    /// <summary>
    /// Json 轉 物件
    /// </summary>        
    public static T JsonToObject<T>(string JsonStr)
    {
        T ResultObj = default(T);
        JsonSerializerSettings mySetting = new JsonSerializerSettings();
        ResultObj = (T)JsonConvert.DeserializeObject(JsonStr, typeof(T), mySetting);

        return ResultObj;
    }
    #endregion
}