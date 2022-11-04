<%@ Page Language="C#" Debug="true" ResponseEncoding="utf-8" %>
<%@ Import Namespace="Microsoft.Win32" %>
<%@ Import Namespace="NAWXDBCINFOIOLib" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Newtonsoft.Json.Linq" %>
<%@ Import Namespace="NewType.FlowSe7en.FlowEngine" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data.SqlClient" %>


<script runat="server">
protected void Page_Load(object sender, EventArgs e){
	string connectionString_ERP_MSSQL = LoadCmdStr("\\\\Database\\\\Project\\\\BPM\\\\BPMPro\\\\Connection\\\\ERP.xdbc.xmf", 1);
	string connectionString_BPM_MSSQL = LoadCmdStr("\\\\Database\\\\Project\\\\BPM\\\\BPMPro\\\\Connection\\\\FM7.xdbc.xmf", 1);
	string connectionString_BPM_OleDB = LoadCmdStr("\\\\Database\\\\Project\\\\BPM\\\\BPMPro\\\\Connection\\\\FM7.xdbc.xmf", 0);
	
	string table = "TEMPTABLE";//<SQL 資料表名稱>
	string code = "S";//<狀態碼：S：已送簽、 1：簽核中、 2：退件、 3：已核准、 5：作廢>
	string sCommand = "";
	
	string returnMsg = "";//<起單錯誤回傳資訊>
	
	//<Error Message Table>
	DataTable ErrMsgDT = new DataTable();
	ErrMsgDT.Columns.Add("ID", typeof(String));
	ErrMsgDT.Columns.Add("ErrorMsg", typeof(String));
	
	//<撈中介表資料>
	sCommand = @"select * from " + table + @" where code = @code";
	Dictionary<string, string> dic = new Dictionary<string, string>();
	dic.Add("@code", code);//<狀態碼>
	DataTable MiddleData = ExecSqlQueryParameters(connectionString_ERP_MSSQL, sCommand, dic);
	
	foreach (DataRow r in MiddleData.Rows) {
		DataTable dtItem_M = new DataTable();
		DataTable dtItem_D = new DataTable();
		
		//<撈申請人資訊>
		sCommand = @"select DeptID, DeptName, AccountID, MemberName from F7Organ_View_CurrMember where Description=@ERP_Account and IsMainJob='1'";
		Dictionary<string, string> dicApplicant = new Dictionary<string, string>();
		dicApplicant.Add("@ERP_Account", r["creator"].ToString());//<狀態碼>
		DataTable CreatorData = ExecSqlQueryParameters(connectionString_BPM_MSSQL, sCommand, dicApplicant);

		if (CreatorData.Rows.Count > 0) {
			string Identify = r["menuid"].ToString();
			string RequisitionID = r["id"].ToString();
			string ApplicantDateTime = r["createdate"].ToString();

			string DeptID = CreatorData.Rows[0]["DeptID"].ToString();
			string DeptName = CreatorData.Rows[0]["DeptName"].ToString();
			string AccountID = CreatorData.Rows[0]["AccountID"].ToString();
			string MemberName = CreatorData.Rows[0]["MemberName"].ToString();
			
			DataRow AutoRow = ErrMsgDT.NewRow();
			
			if(Identify=="PURI05") {
				var Object = JObject.Parse(r["bodycont"].ToString());
				dtItem_M = JsonConvert.DeserializeObject<DataTable>("[" + Object["objA"].ToString() + "]");//<Json 字串轉 DataTable M表>
				dtItem_D = JsonConvert.DeserializeObject<DataTable>(Object["objB"].ToString());//<Json 字串轉 DataTable D表>

				if(dtItem_M.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_M, "M", Identify, RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert M表>
				}
				if(dtItem_D.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_D, "D", Identify, RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert D表>
				}
				returnMsg = AutoStart(connectionString_ERP_MSSQL, connectionString_BPM_MSSQL, table, RequisitionID, Identify + "_P0", AccountID, DeptID);

				AutoRow["ID"] = r["id"].ToString();
				AutoRow["ErrorMsg"] = returnMsg;
				ErrMsgDT.Rows.Add(AutoRow);
			}
			else if(Identify=="PURMI07" || Identify=="PURI07") {
				var Object = JObject.Parse(r["bodycont"].ToString());
				dtItem_M = JsonConvert.DeserializeObject<DataTable>("[" + Object["objA"].ToString() + "]");//<Json 字串轉 DataTable M表>
				dtItem_D = JsonConvert.DeserializeObject<DataTable>(Object["objB"].ToString());//<Json 字串轉 DataTable D表>
				
				if(dtItem_M.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_M, "M", "PURMI07", RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert M表>
				}
				if(dtItem_D.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_D, "D", "PURMI07", RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert D表>
				}
				returnMsg = AutoStart(connectionString_ERP_MSSQL, connectionString_BPM_MSSQL, table, RequisitionID, "PURMI07" + "_P0", AccountID, DeptID);
				
				AutoRow["ID"] = r["id"].ToString();
				AutoRow["ErrorMsg"] = returnMsg;
				ErrMsgDT.Rows.Add(AutoRow);
			}
			else if(Identify=="PURI08") {
				var Object = JObject.Parse(r["bodycont"].ToString());
				dtItem_M = JsonConvert.DeserializeObject<DataTable>("[" + Object["objA"].ToString() + "]");//<Json 字串轉 DataTable M表>
				dtItem_D = JsonConvert.DeserializeObject<DataTable>(Object["objB"].ToString());//<Json 字串轉 DataTable D表>

				if(dtItem_M.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_M, "M", Identify, RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert M表>
				}
				if(dtItem_D.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_D, "D", Identify, RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert D表>
				}
				returnMsg = AutoStart(connectionString_ERP_MSSQL, connectionString_BPM_MSSQL, table, RequisitionID, Identify + "_P0", AccountID, DeptID);

				AutoRow["ID"] = r["id"].ToString();
				AutoRow["ErrorMsg"] = returnMsg;
				ErrMsgDT.Rows.Add(AutoRow);
			}
			else if(Identify=="COPMI06" || Identify=="COPI06") {
				var Object = JObject.Parse(r["bodycont"].ToString());
				dtItem_M = JsonConvert.DeserializeObject<DataTable>("[" + Object["objA"].ToString() + "]");//<Json 字串轉 DataTable M表>
				dtItem_D = JsonConvert.DeserializeObject<DataTable>(Object["objB"].ToString());//<Json 字串轉 DataTable D表>

				if(dtItem_M.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_M, "M", "COPMI06", RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert M表>
				}
				if(dtItem_D.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_D, "D", "COPMI06", RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert D表>
				}
				returnMsg = AutoStart(connectionString_ERP_MSSQL, connectionString_BPM_MSSQL, table, RequisitionID, "COPMI06" + "_P0", AccountID, DeptID);

				AutoRow["ID"] = r["id"].ToString();
				AutoRow["ErrorMsg"] = returnMsg;
				ErrMsgDT.Rows.Add(AutoRow);
			}
			else if(Identify=="COPI07") {
				var Object = JObject.Parse(r["bodycont"].ToString());
				dtItem_M = JsonConvert.DeserializeObject<DataTable>("[" + Object["objA"].ToString() + "]");//<Json 字串轉 DataTable M表>
				dtItem_D = JsonConvert.DeserializeObject<DataTable>(Object["objB"].ToString());//<Json 字串轉 DataTable D表>

				if(dtItem_M.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_M, "M", Identify, RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert M表>
				}
				if(dtItem_D.Rows.Count >0) {
					InsertDB(connectionString_BPM_OleDB, dtItem_D, "D", Identify, RequisitionID, DeptID, DeptName, AccountID, MemberName, ApplicantDateTime);//<insert D表>
				}
				returnMsg = AutoStart(connectionString_ERP_MSSQL, connectionString_BPM_MSSQL, table, RequisitionID, Identify + "_P0", AccountID, DeptID);
				
				AutoRow["ID"] = r["id"].ToString();
				AutoRow["ErrorMsg"] = returnMsg;
				ErrMsgDT.Rows.Add(AutoRow);
			}
			else {
				DataRow row = ErrMsgDT.NewRow();
				row["ID"] = r["id"].ToString();
				row["ErrorMsg"] = "BPM查無此單";
				ErrMsgDT.Rows.Add(AutoRow);
			}
		}
		else {
			DataRow row = ErrMsgDT.NewRow();
			row["ID"] = r["id"].ToString();
			row["ErrorMsg"] = "BPM無此申請人帳號";
			ErrMsgDT.Rows.Add(row);
		}
	}
	
	Response.Write(JsonConvert.SerializeObject(ErrMsgDT, Formatting.Indented));
	Response.End();
}

private string AutoStart(string cnnString_ERP, string cnnString_BPM,string ERP_table, string RequisitionID, string DiagramID, string ApplicantID, string ApplicantDept) {
	string ErrorMsg = "";
	Connector FM7Engine = new Connector();
	DiagramGuid sDiagramGuid = new DiagramGuid(DiagramID);
	
	if (!FM7Engine.Initialize("BPM_BPMPro", ref ErrorMsg)) {//<呼叫實質型別的無參數建構函式，初始化陣列的每個項目> 
		return ErrorMsg;
	}

	using (SqlConnection InterfaceConnection = new SqlConnection(cnnString_BPM)) {
		InterfaceConnection.Open();
		if (FM7Engine.Start(InterfaceConnection, RequisitionID, sDiagramGuid, ApplicantID, ApplicantDept, ref ErrorMsg) == FlowReturn.OK) {
			//<更新中介資料>
			string sCommand = @"update " + ERP_table + " set code = @code where id = @RequisitionID";
			Dictionary<string, string> dic = new Dictionary<string, string>();
			dic.Add("@code", "1");//<狀態碼>
			dic.Add("@RequisitionID", RequisitionID);
			ExecSqlQueryParameters(cnnString_ERP, sCommand, dic);
			
			//Response.Write("{\"State\":\"AutoStartSuccess\", \"ID\":\""+RequisitionID+"\"}");
			return "起單成功";
		}
		else {
			Response.Write("{\"State\":\"AutoStartError\", \"ID\":\""+RequisitionID+"\"}");
			return "起單失敗";
		}
		InterfaceConnection.Close();
	}
	return ErrorMsg;
}

private void InsertDB(string cnnString, DataTable dt, string table, string Identify, string RequisitionID, string DeptID, string DeptName, string AccountID, string MemberName, string ApplicantDateTime) {
	string sField = "";
	string sFieldParam = "";
	string sCommand = "";

	foreach (DataColumn column in dt.Columns) {
		sField += column.ColumnName + ", ";
		sFieldParam += "?, ";
	}

	sField = sField.Substring(0, sField.Length - 2);
	sFieldParam = sFieldParam.Substring(0, sFieldParam.Length - 2);

	if (table == "M") {
		sCommand = @"INSERT INTO [FM7T_" + Identify + "_M]" +
						"(RequisitionID, DiagramID, ApplicantDept, ApplicantDeptName, ApplicantID, ApplicantName, FillerID, FillerName, ApplicantDateTime, Priority, DraftFlag, " + sField + ") " +
						"VALUES('" + RequisitionID + "','" + Identify + "_P0" + "','" + DeptID + "','" + DeptName + "','" + AccountID + "','" + MemberName + "','" + AccountID + "','" + MemberName + "','" + 
						ApplicantDateTime + "','" + "2" + "','" + "0" + "'," + sFieldParam + ")";
	}
	else if (table == "D") {
		sCommand = @"INSERT INTO [FM7T_" + Identify + "_D](AutoCounter, RequisitionID, " + sField + ") " +
						"VALUES((SELECT ISNULL(MAX(AutoCounter),0)+1 FROM [FM7T_" + Identify + "_D]), '" + RequisitionID + "', " + sFieldParam + ")";
	}
	//Response.Write(sCommand+"<br><br>");

	using (OleDbConnection cnn = new OleDbConnection(cnnString)) {
		cnn.Open();
		OleDbTransaction trans = cnn.BeginTransaction();
		using (OleDbCommand cmd = new OleDbCommand()) {
			cmd.Connection = cnn;
			cmd.Transaction = trans;
			cmd.CommandText = sCommand;

			foreach (DataRow rowItem in dt.Rows) {
				cmd.Parameters.Clear();
				foreach (DataColumn column in dt.Columns) {
					cmd.Parameters.AddWithValue("@" + column.ColumnName, (object)rowItem[column.ColumnName] ?? DBNull.Value);
				}
				try {
					cmd.ExecuteNonQuery();
				}
				catch (Exception error) {
					Response.Write("{\"error\":\"" + error + "\"}");
				}
			}
		}
		trans.Commit();
		cnn.Close();
	}
}

private DataTable ExecSqlQueryParameters(string connString, string CommandText, Dictionary<string, string> Map) {
	DataTable dt = new DataTable();
	using (SqlConnection Conn = new SqlConnection(connString)) {
		Conn.Open();
		SqlCommand cmd = new SqlCommand();
		cmd.Connection = Conn;
		cmd.CommandText = CommandText;
		foreach (KeyValuePair<string, string> kvp in Map) {
			SqlParameter parameter = new SqlParameter(kvp.Key, SqlDbType.NVarChar, 255);
			parameter.Value = kvp.Value;
			cmd.Parameters.Add(parameter);
		}
		SqlDataAdapter dAdpter = new SqlDataAdapter(cmd);
		dAdpter.Fill(dt);
		Conn.Close();
	}
	return dt;
}

string LoadCmdStr(string xdbcPath, int DBConType) {
	string FileName = "";
	string connectionString = "";
	const string userRoot = "HKEY_LOCAL_MACHINE";
	const string subkey = "Software\\NewType\\AutoWeb.Net";
	const string keyName = userRoot + "\\" + subkey;
	string Path = (string)Registry.GetValue(keyName, "Root", -1);
	Path = Path.Replace("\\", "\\\\");
	XdbcInfoIO objXdbc = new XdbcInfoIO();
	FileName = Path + xdbcPath;
	objXdbc.LoadFile(FileName, "");
	if (DBConType == 1) {
		connectionString = objXdbc.XdbcConnection.sMsSqlConnectString;
	}
	else {
		connectionString = objXdbc.XdbcConnection.sOleDBConnectString;
	}
	return connectionString;
}
</script>