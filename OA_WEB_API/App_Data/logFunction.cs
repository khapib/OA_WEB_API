using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

/// <summary> 
/// LOG 記錄類別
/// </summary> 
public class logFunction
{
    #region - 檔案記錄 -

    /// <summary>
    ///  Log 寫檔動作
    /// </summary>
    /// <param name="log">Log訊息內容</param>
    public static void SetLogWrite(string log)
    {
        try
        {
            string LogPath = GlobalParameters.LogPath;

            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            StreamWriter sw = new StreamWriter(LogPath + @"\Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", true, Encoding.Default);
            sw.WriteLine(String.Format("{0}∣{1}", DateTime.Now.ToString(), log));
            sw.Flush();
            sw.Close();
        }
        catch (Exception)
        {
        }
    }

    #endregion

    #region - 欄位和屬性 -

    /// <summary>
    /// Log 等級
    /// </summary>
    private enum enumLevel
    {
        DEBUG = 0, 
        INFO = 1, 
        WARN = 2, 
        ERROR = 3
    }

    #endregion
}