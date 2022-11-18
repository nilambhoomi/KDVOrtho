using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DemoTest : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();

    protected void Page_Load(object sender, EventArgs e)


    {
        if (!IsPostBack)
        {
            //string path = Server.MapPath("~/Template/NeckCF.html");
            //string body = File.ReadAllText(path);
            //body = body.Replace("#runat", "runat='server'");
            //CF.InnerHtml = body;
            //hdorgval.Value = body;
            //bindValue("23");
            printDemo();


        }
    }

    [WebMethod]
    public static void funSave(string value, string id, string orgvalue, string painradiation, string painradiation1)
    {
        BusinessLogic _bl = new BusinessLogic();
        _bl.testCF(id, value, orgvalue, painradiation.TrimStart(','), painradiation1.TrimStart(','));
    }

    public void bindValue(string id)
    {

        string _ieMode = "";

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblTest WHERE id=" + id;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);

        if (sqlTbl.Rows.Count > 0)
        {
            CF.InnerHtml = sqlTbl.Rows[0]["value"].ToString();
            hdId.Value = id;
            hdorgval.Value = sqlTbl.Rows[0]["valueoriginal"].ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "bindRadiobuttonValues('" + sqlTbl.Rows[0]["painradiation"].ToString() + "','" + sqlTbl.Rows[0]["painradiation1"].ToString() + "')", true);
        }
    }


    public void printDemo()
    {
        String strfile = File.ReadAllText(Server.MapPath("~/Template/test1.txt"));
        String[] str = new String[2];
        String pattern = @"(<input\s*(.+?)\s*>|<label.*>\s*(.+?)\s*</label>|<select.*>\s*(.+?)\s*</select>)";
        RegexOptions regexOptions = RegexOptions.Multiline;
        Regex regex = new Regex(pattern, regexOptions);
        StringBuilder sb = new StringBuilder();
        Dictionary<string, string> d = new Dictionary<string, string>();


        foreach (Match match in regex.Matches(strfile))
        {
            if (match.Success)
            {
                //            for (int j = 0; j < match.Groups.Count - 1; j++)
                //          {
                // Response.Write("Match [" + j.ToString() + "]: " + match.Groups[j].Value + "<br>");
                //        }
                if (match.Groups[2].Value.Length != 0)
                {
                    String tagvalue = match.Groups[2].Value;
                    String[] attrib = tagvalue.Split(' ');
                    string id = "", value = "", type = "";
                    foreach (string att in attrib)
                    {

                        if (att.ToLower().StartsWith("id"))
                        {
                            String[] attele = att.Split('=');
                            id = attele[1].ToString().Trim('"');

                        }
                        if (att.ToLower().StartsWith("value"))
                        {
                            String[] test = att.Split('=');
                            int startindex = tagvalue.IndexOf("value=") + 7;
                            int endindex = tagvalue.IndexOf("\"", startindex);
                            value = tagvalue.Substring(startindex, endindex - startindex);
                        }
                        if (att.ToLower().StartsWith("type"))
                        {
                            String[] test = att.Split('=');
                            type = test[1].ToLower();
                            Regex rx = new Regex(@"^\s*""?|""?\s*$");

                            type = rx.Replace(type, "");
                            //      Response.Write("type="+type+"<br>");
                        }
                        if (type == "checkbox" || type == "radio")
                        {
                            if (att.ToLower().StartsWith("checked=\"checked\""))
                                value = "true";
                            else
                                value = "false";
                        }

                    }
                    if (id.Trim() != "")
                    {

                        d.Add(id, value);
                    }

                }
            }
        }
        foreach (string key in d.Keys)
        {
            Response.Write(key + "=");
            Response.Write(d[key] + "<br>");
        }
    }
}