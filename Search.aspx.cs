using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Search : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetPatients(string prefix, bool IsPatientPage = false)
    {
        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_SearchByName";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Parameters.AddWithValue("@IsPatient", IsPatientPage);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString() + "_" + sdr["Patient_ID"].ToString());
                    }
                }
                conn.Close();
            }
            return _patients.ToArray();
        }
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string[] GetLocations(string prefix)
    {
        DBHelperClass bHelperClass = new DBHelperClass();
        List<string> _locations = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {

                string query = " select Location_Id,Location from tbllocations where 1=1";


                if (!string.IsNullOrEmpty(prefix))
                {
                    query = query + " and Location like '%" + prefix + "%' or NameOfPractice like '%"+ prefix +"%'";
                }

                DataSet ds = bHelperClass.selectData(query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _locations.Add(ds.Tables[0].Rows[i]["Location_Id"].ToString() + "_" + ds.Tables[0].Rows[i]["Location"].ToString());
                    }
                }
            }
            return _locations.ToArray();
        }
    }
}