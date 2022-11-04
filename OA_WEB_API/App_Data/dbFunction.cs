using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

using OA_WEB_API.Core.Exceptions;

/// <summary> 
/// 資料庫存取類別 
/// </summary> 
public class dbFunction
{
    #region - 宣告 -

    public string strConn;
    private SqlConnection Conn;

    logFunction logFun = new logFunction();

    #endregion

    #region - 初始化 -

    public dbFunction()
    {
        this.strConn = ConfigurationManager.ConnectionStrings["OADBConnectionString"].ConnectionString;

        this.Conn = new SqlConnection(this.strConn);
    }

    public dbFunction(string connName)
    {
        switch (connName)
        {
            case GlobalParameters.sqlConnOADB:
                this.strConn = ConfigurationManager.ConnectionStrings["OADBConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMProDevHo:
                this.strConn = ConfigurationManager.ConnectionStrings["BPMProDevHoConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMProDev:
                this.strConn = ConfigurationManager.ConnectionStrings["BPMProDevConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMProTest:
                this.strConn = ConfigurationManager.ConnectionStrings["BPMProTestConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMPro:
                this.strConn = ConfigurationManager.ConnectionStrings["BPMProConnectionString"].ConnectionString;
                break;
        }

        this.Conn = new SqlConnection(this.strConn);
    }

    #endregion

    #region - 參數式 -

    /// <summary>
    /// 是否要除錯？
    /// </summary>
    private bool _IsDebug = false;

    /// <summary>
    /// 執行 SQL Command
    /// </summary>
    /// <param name="strSQL">T-SQL字串</param>
    /// <param name="listParam">SqlParameter</param>
    public DataTable DoQuery(string strSQL, List<SqlParameter> listParam = null)
    {
        using (SqlConnection Conn = new SqlConnection(strConn))
        {
            using (SqlCommand Cmd = new SqlCommand(strSQL, Conn))
            {
                DataTable dt = new DataTable();

                try
                {
                    Conn.Open();

                    Cmd.CommandType = CommandType.Text;

                    //Cmd.Prepare();

                    SqlDataAdapter da = new SqlDataAdapter(strSQL, Conn);

                    if (listParam != null && listParam.Count > 0)
                    {
                        foreach (SqlParameter param in listParam)
                        {
                            da.SelectCommand.Parameters.Add(new SqlParameter(param.ParameterName, param.Value));
                        }
                    }

                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    CommLib.Logger.Error("DoQuery：" + ex.Message);
                    throw;
                }
                finally
                {
                    if (_IsDebug)
                    {
                        CommLib.Logger.Debug(GetCommandText(strSQL, listParam));
                    }
                }

                return dt;
            }
        }
    }

    /// <summary>
    /// 執行不回傳的 SQL Command
    /// </summary>
    /// <param name="strSQL">T-SQL字串</param>
    /// <param name="listParam">SqlParameter</param>
    public void DoTran(string strSQL, List<SqlParameter> listParam = null)
    {
        using (SqlConnection Conn = new SqlConnection(strConn))
        {
            using (SqlCommand Cmd = Conn.CreateCommand())
            {
                try
                {
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = strSQL;

                    if (listParam != null && listParam.Count > 0)
                    {
                        //CommLib.Logger.Info(strSQL);

                        foreach (SqlParameter param in listParam)
                        {
                            //CommLib.Logger.Info(param.ParameterName + "：" + param.Value);

                            Cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
                        }
                    }

                    Conn.Open();
                    Cmd.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    CommLib.Logger.Error("DoTran：" + ex.Message);
                    throw;
                }
                finally
                {
                    if (_IsDebug)
                    {
                        CommLib.Logger.Debug(GetCommandText(strSQL, listParam));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 僅用在 SP 執行上
    /// </summary>
    /// <param name="strSQL">T-SQL字串</param>
    public void DoExec(string strSQL, List<SqlParameter> listParam = null)
    {
        using (SqlConnection Conn = new SqlConnection(strConn))
        {
            using (SqlCommand Cmd = new SqlCommand(strSQL, Conn))
            {
                try
                {
                    Cmd.CommandType = CommandType.StoredProcedure;

                    if (listParam != null && listParam.Count > 0)
                    {
                        foreach (SqlParameter param in listParam)
                        {
                            Cmd.Parameters.AddWithValue(param.ParameterName, param.Value);
                        }
                    }

                    var dt = new DataTable();
                    var da = new SqlDataAdapter(Cmd);

                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    CommLib.Logger.Error("DoExec：" + ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// 動態設定SQL參數
    /// </summary>
    /// <param name="sqlParamList">List of SqlParameters</param>
    /// <param name="Values">參數值</param>
    /// <param name="paramNamePrefix">參數名稱</param>
    /// <returns>主要使用在 WHERE IN 查詢，產生 @UID0、@UID1、@UID2</returns>
    public String SetSqlParameterList(ref List<SqlParameter> sqlParamList, string Values, String paramNamePrefix)
        {
            string sql_Params = String.Empty;

            string[] aryValues = Values.Split(',');

            try
            {
                for (int i = 0; i < aryValues.Length; i++)
                {
                    string paramName = String.Format("@{0}{1}", paramNamePrefix, i.ToString());
                    sqlParamList.Add(new SqlParameter(paramName, aryValues[i]));
                    sql_Params += paramName + ",";
                }

                sql_Params = sql_Params.TrimEnd(',');

                return sql_Params;
            }
            catch (Exception ex)
            {
                CommLib.Logger.Error("SetSqlParameterList：" + ex.Message);
                throw;
            }
        }

    /// <summary>
    /// 參數化查詢，取得 SQL Command
    /// </summary>
    /// <param name="strSQL">T-SQL字串</param>
    /// <param name="listParam">SqlParameter</param>
    private static string GetCommandText(string strSQL, List<SqlParameter> listParam = null)
    {
        if (listParam != null && listParam.Count > 0)
        {
            foreach (SqlParameter param in listParam)
            {
                //strSQL = strSQL.Replace(param.ParameterName, param.Value is String ? "'" + param.Value.ToString() + "'" : param.ParameterName);  //
                strSQL = strSQL.Replace(param.ParameterName, "'" + param.Value.ToString() + "'");

                //CommLib.Logger.Info(param.ParameterName + "：" + param.Value);
            }
        }

        return strSQL;
    }

    /// <summary>
    /// 參數化查詢，取得 SQL Connection Strings
    /// </summary>
    public static string GetConnectionStrings(string connName)
    {
        var connString = String.Empty;

        switch (connName)
        {
            case GlobalParameters.sqlConnOADB:
                connString = ConfigurationManager.ConnectionStrings["OADBConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMProDevHo:
                connString = ConfigurationManager.ConnectionStrings["BPMProDevHoConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMProDev:
                connString = ConfigurationManager.ConnectionStrings["BPMProDevConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMProTest:
                connString = ConfigurationManager.ConnectionStrings["BPMProTestConnectionString"].ConnectionString;
                break;

            case GlobalParameters.sqlConnBPMPro:
                connString = ConfigurationManager.ConnectionStrings["BPMProConnectionString"].ConnectionString;
                break;
        }

        return connString;
    }

    #endregion
}