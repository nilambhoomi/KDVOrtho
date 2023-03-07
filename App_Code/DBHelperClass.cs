using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Xml;

/// <summary>
/// Summary description for DBHelperClass
/// </summary>
public class DBHelperClass
{
    SqlConnection cn = null;
    public DBHelperClass()
    {
        cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
    }
    public DataSet test(string id, string day)
    {
        SqlCommand cm = new SqlCommand("test", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@LID", id);
        cm.Parameters.AddWithValue("@day", id);
        cn.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public void logDetail(string LogName, string LogLocation, string LogIp, string logDescription, string logIntime, string logOutTime, string log_id)
    {
        DateTime logIntime1;
        DateTime logoutTime1;
        SqlCommand cm = new SqlCommand("usp_logDetail", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@name", LogName);
        cm.Parameters.AddWithValue("@location", LogLocation);
        cm.Parameters.AddWithValue("@ip_address", LogIp);
        cm.Parameters.AddWithValue("@description", logDescription);
        if (string.IsNullOrWhiteSpace(logIntime))
        {
            cm.Parameters.Add("@login_time", DBNull.Value);
        }
        else
        {
            logIntime1 = Convert.ToDateTime(logIntime);
            cm.Parameters.AddWithValue("@login_time", logIntime1);
        }
        if (string.IsNullOrWhiteSpace(logOutTime))
        {
            cm.Parameters.Add("@logout_time", DBNull.Value);
        }
        else
        {
            logoutTime1 = Convert.ToDateTime(logOutTime);
            cm.Parameters.AddWithValue("@logout_time", logoutTime1);
        }
        if (string.IsNullOrWhiteSpace(log_id))
        {
            cm.Parameters.AddWithValue("@log_id", DBNull.Value);
        }
        else
        {
            cm.Parameters.AddWithValue("@log_id", Convert.ToInt32(log_id));
        }
        cn.Open();
        var val = cm.ExecuteScalar();
        HttpContext.Current.Session["log_id"] = val;
        cn.Close();

    }
    public void logDetailtbl(int log_id, string description, string log_time)
    {

        DateTime logtime;
        SqlCommand cm = new SqlCommand("usp_logdetails_insert", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@log_id", log_id);
        cm.Parameters.AddWithValue("@description", description);
        if (string.IsNullOrWhiteSpace(log_time))
        {
            cm.Parameters.Add("@log_time", DBNull.Value);
        }
        else
        {
            logtime = Convert.ToDateTime(log_time);
            cm.Parameters.Add("@log_time", logtime);
        }
        cn.Close();
        cn.Open();
        cm.ExecuteNonQuery();
        cn.Close();
    }

    public DataSet logDetail(string start_date, string end_date, string id, string location)
    {
        SqlCommand cm = new SqlCommand("usp_select_log", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        if (id == "ALL")
        {
            cm.Parameters.AddWithValue("@name", DBNull.Value);
        }
        else
        {
            cm.Parameters.AddWithValue("@name", id);
        }
        if (location == "ALL")
        {
            cm.Parameters.AddWithValue("@location", DBNull.Value);
        }
        else
        {
            cm.Parameters.AddWithValue("@location", location);
        }
        cm.Parameters.AddWithValue("@start_date", Convert.ToDateTime(start_date));
        cm.Parameters.AddWithValue("@end_date", Convert.ToDateTime(end_date));
        cn.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);
        cn.Close();
        return ds;
    }


    public DataSet PatientSP(string date, int id)
    {
        SqlCommand cm = new SqlCommand("usp_patient", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;

        cm.Parameters.AddWithValue("@UsrDate", date);
        cm.Parameters.AddWithValue("@LocationId", id);
        cn.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public DataTable GetGriddata(string name)
    {
        SqlCommand cm = new SqlCommand("usp_getdatagrid", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@name", name);
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;

    }
    public DataTable GetGrid(int id)
    {
        SqlCommand cm = new SqlCommand("usp_GetGrid", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@ID", id);
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public DataTable GetProcDetails(int id, string bodyPart)
    {
        SqlCommand cm = new SqlCommand("GetProcedureDetails", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@PatientIE_id", id);
        cm.Parameters.AddWithValue("@BodyPart", bodyPart);
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public DataTable GetAllProcDetails(string bodyPart, int id, string Position)
    {
        SqlCommand cm = new SqlCommand("GetAllProceduressnew", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@PatientIEId", id);
        cm.Parameters.AddWithValue("@BodyPart", bodyPart);
        cm.Parameters.AddWithValue("@Position", Position);
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public DataTable GetAllConsider(int patientIEID)
    {
        SqlCommand cm = new SqlCommand("nusp_GetConsider", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@ProcedureIEID", patientIEID);
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public DataTable GetAllProcDetail(string bodyPart, int id, string Position)
    {
        SqlCommand cm = new SqlCommand("GetAllProcedurecnew", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@PatientIEId", id);
        cm.Parameters.AddWithValue("@BodyPart", bodyPart);
        cm.Parameters.AddWithValue("@Position", Position);
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;
    }
    public DataTable GetallBodyparts()
    {
        SqlCommand cm = new SqlCommand("GetallBodyparts", cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cm.CommandType = CommandType.StoredProcedure;
        cn.Close();
        cn.Open();
        DataTable ds = new DataTable();
        da.Fill(ds);
        cn.Close();
        return ds;
    }

    public int executeQuery(string query)
    {
        SqlCommand cm = new SqlCommand(query, cn);
        cn.Open();
        cm.ExecuteNonQuery();
        cn.Close();
        return 1;
    }
    public int executeSP(string sp, SqlParameter[] param)
    {
        int val = 0;
        try
        {
            SqlCommand cm = new SqlCommand(sp, cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddRange(param.ToArray());
            cn.Open();
            val = Convert.ToInt32(cm.ExecuteScalar());
            cn.Close();
            return val;
        }
        catch (Exception e)
        {
            LogError(e);
        }

        return val;
    }

    public DataSet executeSelectSP(string sp, SqlParameter[] param)
    {
        DataSet val = new DataSet();
        try
        {
            SqlCommand cm = new SqlCommand(sp, cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddRange(param.ToArray());
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(val);

            cn.Close();
            return val;
        }
        catch (Exception e)
        {
            LogError(e);
        }

        return val;
    }
    public void LogError(Exception ex)
    {
        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", ex.Message);
        message += Environment.NewLine;
        message += string.Format("StackTrace: {0}", ex.StackTrace);
        message += Environment.NewLine;
        message += string.Format("Source: {0}", ex.Source);
        message += Environment.NewLine;
        message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        string path = HttpContext.Current.Server.MapPath("~/ErrorLog.txt");
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(message);
            writer.Close();
        }
    }
    public DataTable selectDatatable(string query)
    {
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        DataTable ds = new DataTable();
        da.Fill(ds);
        return ds;
    }
    public DataSet selectData(string query)
    {
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public void Insertupdate(string pid, string @patientIE_ID, string MCODE, string Request, string scheduled, string Executed, string BodyPart)
    {
        DateTime Request1;
        DateTime scheduled1;
        DateTime Executed1;
        SqlCommand cm = new SqlCommand("usp_insertupdate", cn);
        cm.CommandType = CommandType.StoredProcedure;
        cm.Parameters.AddWithValue("@Pid", pid);
        cm.Parameters.AddWithValue("@PatientIE_ID", @patientIE_ID);
        cm.Parameters.AddWithValue("@MCODE", MCODE);
        if (string.IsNullOrWhiteSpace(Request))
        {
            cm.Parameters.Add("@Request", DBNull.Value);
        }
        else
        {
            Request1 = Convert.ToDateTime(Request);
            cm.Parameters.AddWithValue("@Request", Request1);
        }

        if (string.IsNullOrWhiteSpace(scheduled))
        {
            cm.Parameters.Add("@Scheduled", DBNull.Value);
        }
        else
        {
            scheduled1 = Convert.ToDateTime(scheduled);
            cm.Parameters.AddWithValue("@Scheduled", scheduled1);
        }
        if (string.IsNullOrWhiteSpace(Executed))
        {
            cm.Parameters.Add("@Executed", DBNull.Value);
        }
        else
        {
            Executed1 = Convert.ToDateTime(Executed);
            cm.Parameters.AddWithValue("@Executed", Executed1);
        }
        cm.Parameters.AddWithValue("@BodyPart", BodyPart);
        cn.Open();
        cm.ExecuteNonQuery();
        cn.Close();
    }
    public DataSet PatientIE_getAll(string query, int pageindex, int pagesize, out int totalrecords)
    {
        DataSet ds = new DataSet();
        totalrecords = 0;
        try
        {
            string sql_SP = "usp_select_PatientIE";
            using (SqlCommand cmd = new SqlCommand(sql_SP, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageindex);
                cmd.Parameters.AddWithValue("@cnd", query);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
                totalrecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                cn.Close();
            }


        }
        catch (Exception Ex)
        {
            ds = null;
        }
        return ds;
    }

    public DataSet Patientmaster_autocomplete(string fname, string lname)
    {
        DataSet ds = new DataSet();

        try
        {
            string sql_SP = "usp_select_PatientMaster";


            using (SqlCommand cmd = new SqlCommand(sql_SP, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fname", fname);
                cmd.Parameters.AddWithValue("@lname", lname);


                cn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
                cn.Close();
            }


        }
        catch (Exception Ex)
        {

            ds = null;

        }
        return ds;
    }

    public SettingModel getSettingValues()
    {
        SettingModel settingModel = new SettingModel();
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Template/Default_Admin.xml"));
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
            foreach (XmlNode node in nodeList)
            {
                settingModel.forwardCC = node.SelectSingleNode("forwardCC") == null ? false : Convert.ToBoolean(node.SelectSingleNode("forwardCC").InnerText);
                settingModel.forwardPE = node.SelectSingleNode("forwardPE") == null ? false : Convert.ToBoolean(node.SelectSingleNode("forwardPE").InnerText);
                settingModel.forwardROM = node.SelectSingleNode("forwardROM") == null ? false : Convert.ToBoolean(node.SelectSingleNode("forwardROM").InnerText);
                settingModel.is_no_diagnostudy_show = node.SelectSingleNode("is_no_diagnostudy_show") == null ? false : Convert.ToBoolean(node.SelectSingleNode("is_no_diagnostudy_show").InnerText);
                settingModel.is_no_diagnostudy_msg = node.SelectSingleNode("is_no_diagnostudy_msg").InnerText;
            }

            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings/POCReports");

            foreach (XmlNode node in nodeList)
            {
                settingModel.displaySex = node.SelectSingleNode("Sex") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Sex").InnerText);
                settingModel.displayFName = node.SelectSingleNode("FName") == null ? false : Convert.ToBoolean(node.SelectSingleNode("FName").InnerText);
                settingModel.displayLName = node.SelectSingleNode("LName") == null ? false : Convert.ToBoolean(node.SelectSingleNode("LName").InnerText);
                settingModel.displayMC = node.SelectSingleNode("MC") == null ? false : Convert.ToBoolean(node.SelectSingleNode("MC").InnerText);
                settingModel.displayAttorny = node.SelectSingleNode("Attorny") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Attorny").InnerText);
                settingModel.displayCase = node.SelectSingleNode("Case") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Case").InnerText);
                settingModel.displayDOB = node.SelectSingleNode("DOB") == null ? false : Convert.ToBoolean(node.SelectSingleNode("DOB").InnerText);
                settingModel.displayNote = node.SelectSingleNode("Note") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Note").InnerText);
                settingModel.displayDOA = node.SelectSingleNode("DOA") == null ? false : Convert.ToBoolean(node.SelectSingleNode("DOA").InnerText);
                settingModel.displayMCode = node.SelectSingleNode("MCode") == null ? false : Convert.ToBoolean(node.SelectSingleNode("MCode").InnerText);
                settingModel.displaySides = node.SelectSingleNode("Sides") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Sides").InnerText);
                settingModel.displayLevel = node.SelectSingleNode("Level") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Level").InnerText);
                settingModel.displayPhone = node.SelectSingleNode("Phone") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Phone").InnerText);
                settingModel.displayLocation = node.SelectSingleNode("Location") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Location").InnerText);
                settingModel.displayPolicy_No = node.SelectSingleNode("Policy_No") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Policy_No").InnerText);
                settingModel.displayClaim_No = node.SelectSingleNode("ClaimNumber") == null ? false : Convert.ToBoolean(node.SelectSingleNode("ClaimNumber").InnerText);
                settingModel.displayInsurance = node.SelectSingleNode("Insurance") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Insurance").InnerText);

                settingModel.displayConsider = node.SelectSingleNode("Consider") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Consider").InnerText);
                settingModel.displayVaccinated = node.SelectSingleNode("Vaccinated") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Vaccinated").InnerText);
                settingModel.displayRequested = node.SelectSingleNode("Requested") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Requested").InnerText);
                settingModel.displayScheduled = node.SelectSingleNode("Scheduled") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Scheduled").InnerText);
                settingModel.displayExecuted = node.SelectSingleNode("Executed") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Executed").InnerText);
                settingModel.displayProvider = node.SelectSingleNode("Provider") == null ? false : Convert.ToBoolean(node.SelectSingleNode("Provider").InnerText);
                settingModel.displayDOE = node.SelectSingleNode("DOE") == null ? false : Convert.ToBoolean(node.SelectSingleNode("DOE").InnerText);

            }
        }
        catch (Exception ex)
        {
        }

        return settingModel;

    }

    public DefaultCCPEModel getDefaultCCPEValues(string bodyPart, string pos = "")
    {
        DefaultCCPEModel defaultModel = new DefaultCCPEModel();
        try
        {
            string query = "select CC,PE from tblDefaultCCPE where bodyPart='" + bodyPart + "' ";
            if (!string.IsNullOrEmpty(pos))
                query = query + " and Position='" + pos + "' ";

            DataSet ds = this.selectData(query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                defaultModel.CC = ds.Tables[0].Rows[0]["CC"].ToString();
                defaultModel.PE = ds.Tables[0].Rows[0]["PE"].ToString();
            }
        }
        catch (Exception ex)
        {
        }

        return defaultModel;

    }

    public int getLatestFUID(string PatientIE_Id)
    {

        string query = "select top 1 PatientFU_ID from tblFUPatient where PatientIE_ID=" + PatientIE_Id + " order by PatientFU_ID desc";
        var dt = this.selectDatatable(query);
        int id = 0;

        if (dt != null && dt.Rows.Count > 0)
        {
            id = Convert.ToInt32(dt.Rows[0][0].ToString());
        }

        return id;
      
    }

    public int geIEfromFUID(string PatientFU_Id)
    {

        string query = "select PatientIE_ID from tblFUPatient where PatientFU_ID=" + PatientFU_Id;
        var dt = this.selectDatatable(query);
        int id = 0;

        if (dt != null && dt.Rows.Count > 0)
        {
            id = Convert.ToInt32(dt.Rows[0][0].ToString());
        }

        return id;

    }


    public DefaultCCPEModel getCarryForwardValues(string query)
    {
        DefaultCCPEModel _obj = new DefaultCCPEModel(); 

        var dt = this.selectDatatable(query);
        int id = 0;

        if (dt != null && dt.Rows.Count > 0)
        {
            _obj.CC = dt.Rows[0]["CCvalue"].ToString();
            _obj.PE = dt.Rows[0]["PEvalue"].ToString();
          
        }

        return _obj;

    }
}

