using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Comments : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["PatientIE_ID"] == null)
            {
                Session["bodyPartsList"] = null;
                Response.Redirect("Page1.aspx");

            }
            else if (Session["PatientIE_ID"] != null)
            {
                PopulateUI(Session["PatientIE_ID"].ToString(), "Open");
            }
            //PopulateUIDefaults();
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  Comments for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }
    public string SaveUI(string ieID, string ieMode)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);

        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblPatientIE WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0)
            _ieMode = "New";
        else
            _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["Comments"] = txtComment.Text.ToString().Trim();
            TblRow["commentextra"] = txtCommentextra.Text.ToString().Trim();
            TblRow["UpdateFlag"] = "NEW";
        }

        if (_ieMode == "New")
        {
            TblRow["CreatedBy"] = "Admin";
            TblRow["CreatedDate"] = DateTime.Now;
            sqlTbl.Rows.Add(TblRow);
        }

        sqlAdapt.Update(sqlTbl);

        TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        if (_ieMode == "New")
            return "Comments have been added...";
        else
            return "Comments have been updated...";
    }
    public void PopulateUI(string ieID, string ieMode)
    {
        if (ieMode == "Open")
        {
            string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select Comments,commentextra from tblPatientIE WHERE PatientIE_ID = " + ieID;
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count > 0)
            {
                _fldPop = true;
                TblRow = sqlTbl.Rows[0];
                txtComment.Text = TblRow["Comments"].ToString().Trim();
               txtCommentextra.Text=Convert.ToString(TblRow["commentextra"]);
                _fldPop = false;
            }
            else
                PopulateUIDefaults();

            sqlTbl.Dispose();
            sqlCmdBuilder.Dispose();
            sqlAdapt.Dispose();
            oSQLConn.Close();
        }
        else if (ieMode == "New")
        {
            string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["dbPainTrax"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select Comments,commentextra from tblPatientIE WHERE PatientIE_ID = " + ieID;
            
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count > 0)
            {
                _fldPop = true;
                TblRow = sqlTbl.Rows[0];
                txtComment.Text = TblRow["Comments"].ToString().Trim();
                txtCommentextra.Text = Convert.ToString(TblRow["commentextra"]);
                _fldPop = false;
            }
            sqlTbl.Dispose();
            sqlCmdBuilder.Dispose();
            sqlAdapt.Dispose();
            oSQLConn.Close();
        }
    }
    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Comments");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            if (txtComment.Text == "") txtComment.Text = node.SelectSingleNode("Comment") == null ? txtComment.Text.ToString().Trim() : node.SelectSingleNode("Comment").InnerText;
            _fldPop = false;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveUI(Session["PatientIE_ID"].ToString(), "");
    }
}