using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using OA_WEB_API.Models;
using OA_WEB_API.Models.BPMPro;

///   <summary> 
///   簡訊發送類別
///   </summary> 
public class smsFunction
{
    /// <summary>
     /// 傳送簡訊(多筆：用逗號分開)
    /// </summary>
    public void SendSMS(SmsModel model)
    {
        #region - 宣告 -

        dbFunction dbFun = new dbFunction(GlobalParameters.sqlConnOADB);

        #endregion

        var phoneNo = new List<String>();

        foreach (var item in model.PHONE_BOOK_LIST)
        {
            phoneNo.Add(item.PHONE_NO);
        }

        var parameter = new List<SqlParameter>
        {
            new SqlParameter("@PHONE_NO", SqlDbType.VarChar) { Size = 500, Value = String.Join(",", phoneNo) },
            new SqlParameter("@DELIVER_DATE_TIME", SqlDbType.Char) { Size = 14, Value = DateTime.Now.ToString("yyyyMMddHHmmss") },
            new SqlParameter("@CONTENT", SqlDbType.VarChar) { Size = 200, Value = model.CONTENT }
        };

        strSQL = "";
        strSQL += "EXEC [OADB].[dbo].[sp_SendSMS] @PHONE_NO, @DELIVER_DATE_TIME, @CONTENT";

        dbFun.DoTran(strSQL, parameter);
    }

    #region - 欄位和屬性 -

    /// <summary>
    /// T-SQL
    /// </summary>
    private string strSQL;

    #endregion
}