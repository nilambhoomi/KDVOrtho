using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO.Compression;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Xceed.Words.NET;
using System.Text;
using System.Net;

public partial class PatientIntakeList : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    public int iCounter = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            Session["patientFUId"] = "";
            BindPatientIEDetails();
            bindLocation();
            txtSearch.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
            txtFromDate.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
            txtEndDate.Attributes.Add("onkeydown", "funfordefautenterkey1(" + btnSearch.ClientID + ",event)");
        }
    }

    protected void BindPatientIEDetails(string patientId = null, string searchText = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetPatientIEDetails", con);

            if (!string.IsNullOrEmpty(patientId))
            {
                cmd.Parameters.AddWithValue("@Patient_Id", hfPatientId.Value);
            }
            else if (!string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(patientId))
            {
                string keyword = searchText.TrimStart(("Mrs. ").ToCharArray());
                cmd.Parameters.AddWithValue("@SearchText", keyword);
            }
            else
            {
                if (Session["Location"] != null)
                {
                    cmd.Parameters.AddWithValue("@LocationId", Convert.ToString(Session["Location"]));
                }
            }

            if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                cmd.Parameters.AddWithValue("@SDate", txtFromDate.Text);
                cmd.Parameters.AddWithValue("@EDate", txtEndDate.Text);

            }
            else if (!string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtEndDate.Text))
            {
                cmd.Parameters.AddWithValue("@SDate", txtFromDate.Text);
            }

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            string _query = "";
            DataRow row;
            if (rbllisttype.SelectedItem.Value == "0")
            {
                _query = " DOE is not null";

            }
            else
                _query = " DOE is null";

            if (ddl_location.SelectedIndex > 0)
            {
                if (string.IsNullOrEmpty(_query))
                {
                    _query = " Location_ID=" + ddl_location.SelectedItem.Value;
                }
                else
                    _query = _query + " and Location_ID=" + ddl_location.SelectedItem.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(_query))
                    _query = " Location_ID in (" + Session["Locations"].ToString() + ")";
                else
                    _query = _query + " and Location_ID in (" + Session["Locations"].ToString() + ")";
            }



            try
            {
                dt = dt.Select(_query).CopyToDataTable();
                DataView dv = dt.DefaultView;
                dv.Sort = "LastTestDate desc";
                dt = dv.ToTable();
            }
            catch (Exception ex)
            {
                dt = null;
            }


            con.Close();
            Session["iedata"] = dt;

            gvPatientDetails.DataSource = dt;
            gvPatientDetails.DataBind();
            hfPatientId.Value = null;
        }
    }

    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void gvPatientFUDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvPatientFUDetails = (sender as GridView);
        hfCurrentlyOpened.Value = gvPatientFUDetails.ToolTip;
        gvPatientFUDetails.PageIndex = e.NewPageIndex;
        bindFUDetails(gvPatientFUDetails);
    }

    protected void bindFUDetails(GridView gvPatientFUDetails)
    {
        BusinessLogic bl = new BusinessLogic();
        gvPatientFUDetails.DataSource = Session["dtfu"] = bl.GetFUDetails(Convert.ToInt32(gvPatientFUDetails.ToolTip));
        gvPatientFUDetails.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string patientIEId = gvPatientDetails.DataKeys[e.Row.RowIndex].Value.ToString();
            BusinessLogic bl = new BusinessLogic();
            GridView gvPatientFUDetails = e.Row.FindControl("gvPatientFUDetails") as GridView;

            System.Web.UI.WebControls.Image img = e.Row.FindControl("plusimg") as System.Web.UI.WebControls.Image;



            gvPatientFUDetails.ToolTip = patientIEId;
            gvPatientFUDetails.DataSource = bl.GetFUDetails(Convert.ToInt32(patientIEId));
            gvPatientFUDetails.DataBind();

            if (gvPatientFUDetails.Rows.Count == 0)
                img.Attributes.Add("style", "display:none");
            else
                img.Attributes.Add("style", "display:block");
        }
    }

    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvPatientDetails.PageIndex = e.NewPageIndex;
        BindPatientIEDetails(hfPatientId.Value, txtSearch.Text.Trim());
    }

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Session["PatientIE_ID"] = null;
        Response.Redirect("Page1.aspx");
    }

    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        Response.Redirect("Page1.aspx?id=" + btn.CommandArgument);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PatientIntakeList.aspx");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatus(string flag, Int64 id)
    {
        string tempFileName = DateTime.Now.ToString("yyyyMMdd_") + flag + "_" + id;
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        //if (filePaths.Length <= 0)
        //    return "";
        //using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        //{
        //    foreach (string filePath in filePaths)
        //    {
        //        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
        //        archive.CreateEntryFromFile(filePath, filename);
        //    }
        //}

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownload(string flag, Int64 id)
    {
        string tempFileName = DateTime.Now.ToString("yyyyMMdd_") + flag + "_" + id;
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatus";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string UpdatePrintStatusRod(string flag, Int64 id)
    {
        string tempFileName = "_" + id + "_" + flag + "_Rod";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString();
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");

        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            if (filePaths.Count() > 0)
            {
                foreach (var item in filePaths)
                {
                    File.Delete(item);
                }
            }
        }

        List<string> _patients = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusRoD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        _patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
            return "";
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string CheckDownloadRod(string flag, Int64 id)
    {
        string tempFileName = "_" + id + "_" + flag + "_Rod";
        string tempFilePath = ConfigurationSettings.AppSettings["downloadpath"].ToString();
        string fileGetPath = ConfigurationSettings.AppSettings["fileGetPath"].ToString() + "/ROD";
        string zipCreatePath = System.Web.Hosting.HostingEnvironment.MapPath(tempFilePath + "/" + tempFileName + ".zip");
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        if (File.Exists(zipCreatePath))
        {
            File.Delete(zipCreatePath);
            filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileGetPath), "*_" + id + "_*.*");
        }
        if (filePaths.Length <= 0)
            return "";
        using (ZipArchive archive = ZipFile.Open(zipCreatePath, ZipArchiveMode.Create))
        {
            foreach (string filePath in filePaths)
            {
                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                archive.CreateEntryFromFile(filePath, filename);
            }
        }
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "nusp_UpdatePrintStatusRoD";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flag", flag);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Isdownload", "1");
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //_patients.Add(sdr["RESULT"].ToString());
                    }
                }
                conn.Close();
            }
        }
        return tempFileName;
    }

    protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvPatientDetails.PageSize = Convert.ToInt16(ddlPage.SelectedItem.Value);
        BindPatientIEDetails();
    }

    private void BindRODDeafultValues(DataView dv, bool IsFromFU = false)
    {
        try
        {
            XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Default_Rod.xml"));
            DataSet ds = new DataSet();
            ds.ReadXml(xmlreader);
            xmlreader.Close();
            if (dv != null)
            {

                string clause = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/XML/Clause.xml"));
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Clauses");
                foreach (XmlNode node in nodeList)
                {
                    if (!IsFromFU)
                    {
                        clause = node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[7])) == null ? string.Empty : node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[7])).InnerText;
                    }
                    else
                    {
                        clause = node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[13])) == null ? string.Empty : node.SelectSingleNode(Convert.ToString(dv[0].Row.ItemArray[13])).InnerText;
                    }

                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Name"].ToString().Contains("##Name##"))
                        {
                            string temp = row["Name"].ToString();
                            if (!IsFromFU)
                            {
                                temp = temp.Replace("##Name##", Convert.ToString(dv[0].Row.ItemArray[1]) + " " + Convert.ToString(dv[0].Row.ItemArray[3]) + " " + Convert.ToString(dv[0].Row.ItemArray[2])).Replace("##IEdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[9]).ToString("MM/dd/yyyy"))).Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy"))).Replace("##cause##", clause).Replace("##FUVisitdate##", " ___(last DOS)___ ");
                            }
                            else
                            {
                                temp = temp.Replace("##Name##", Convert.ToString(dv[0].Row.ItemArray[6]) + " " + Convert.ToString(dv[0].Row.ItemArray[4]) + " " + Convert.ToString(dv[0].Row.ItemArray[5])).Replace("##FUVisitdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[7]).ToString("MM/dd/yyyy"))).Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy"))).Replace("##cause##", clause).Replace("##IEdate##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[12]).ToString("MM/dd/yyyy")));
                            }
                            row.SetField("Name", temp);
                        }
                        else if (row["Name"].ToString().Contains("##DOA##"))
                        {
                            string temp = row["Name"].ToString();
                            if (!IsFromFU)
                            {
                                temp = temp.Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[5]).ToString("MM/dd/yyyy")));
                            }
                            else
                            {
                                temp = temp.Replace("##DOA##", Convert.ToString(Convert.ToDateTime(dv[0].Row.ItemArray[11]).ToString("MM/dd/yyyy")));
                            }
                            row.SetField("Name", temp);
                        }
                    }

                    repRoD.DataSource = ds.Tables[0];
                    repRoD.DataBind();
                }
            }
        }
        catch (Exception)
        {

            throw;
        }


    }

    protected void btnrodsave_Click(object sender, EventArgs e)
    {
        //string id = hdnrodieid.Value;
        SqlConnection oSQLConn = new SqlConnection();
        SqlCommand oSQLCmd = new SqlCommand();
        string _ieID = Convert.ToString(hdnrodieid.Value);
        string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
        string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
        {
            SqlStr = "Select * from tblrod WHERE patietn_IE = " + _ieID;
        }
        else
        {
            SqlStr = "Select * from tblrod WHERE patietn_IE = " + Convert.ToInt64(_fuieid) + " and Patiend_FUID = " + Convert.ToInt64(_fufuid);
        }

        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0)
            _ieMode = "Update";
        else
            _ieMode = "Delete";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["patietn_IE"] = !string.IsNullOrEmpty(_ieID) ? _ieID : _fuieid;

            if (!string.IsNullOrEmpty(_fufuid))
            {
                TblRow["Patiend_FUID"] = _fufuid;
            }

            TblRow["Content"] = txtrodFulldetails.Text;
            TblRow["Contentdelimit"] = bindRodPrintvalue();
            TblRow["Bodypartdetails"] = hdbodyparts.Value;
            TblRow["Newlinedetails"] = hdnewline.Value;
            TblRow["Plandetails"] = "test";
            TblRow["Plandelimit"] = "test";
            TblRow["Clientnote"] = "test";
            TblRow["Signpath"] = "test";

            if (_ieMode == "New")
            {
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();





        if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
        {
            LinkButton btn = new LinkButton();
            btn.Text = "RoD";
            btn.CommandArgument = _ieID;
            lnkierod_Click(btn, e);

        }
        else
        {

            LinkButton btn = new LinkButton();
            btn.Text = "RoD";
            btn.CommandArgument = _fufuid + "-" + _fuieid;
            lnkfurod_Click(btn, e);
        }

    }

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        bindRodPrintvalue();
    }

    protected void txtRod_TextChanged(object sender, EventArgs e)
    {
        bindRodPrintvalue();
    }

    private string bindRodPrintvalue()
    {
        string str = "";
        string strDelimit = "";
        string bodypartselected = string.Empty;
        string bodypartUnselected = string.Empty;
        string planselected = string.Empty;
        string planunselected = string.Empty;
        string bodypart = string.Empty;
        string strbp = string.Empty, strnewline = string.Empty;
        for (int i = 0; i < repRoD.Items.Count; i++)
        {

            TextBox txt = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
            CheckBox chk = repRoD.Items[i].FindControl("chk") as CheckBox;
            HiddenField hdbodypart = repRoD.Items[i].FindControl("bodypart") as HiddenField;
            HiddenField hdisnewline = repRoD.Items[i].FindControl("isnewline") as HiddenField;
            if (chk.Checked)
            {
                if (hdisnewline.Value == "1")
                    str = str + @"\n" + txt.Text;
                else if (hdisnewline.Value == "2")
                    str = str + @"\n\n" + txt.Text;
                else
                    str = str + txt.Text;

                strDelimit = strDelimit + "^" + txt.Text;
                bodypart += hdbodypart.Value + ",";
                if (hdbodypart.Value.Split('-').Count() > 1)
                {
                    if (hdbodypart.Value.Split('-')[1].Equals("b"))
                    {
                        bodypartselected += hdbodypart.Value + ",";
                    }
                    else if (hdbodypart.Value.Split('-')[1].Equals("p"))
                    {
                        planselected += hdbodypart.Value + ",";
                    }
                }


            }
            else
            {

                // str = !string.IsNullOrEmpty(txt.Text) ? str.Replace(txt.Text, "") : str;
                strDelimit = strDelimit + "^@" + txt.Text;

                if (hdbodypart.Value.Split('-').Count() > 1)
                {
                    if (hdbodypart.Value.Split('-')[1].Equals("b"))
                    {
                        bodypartUnselected += hdbodypart.Value + ",";
                    }
                    else if (hdbodypart.Value.Split('-')[1].Equals("p"))
                    {
                        planunselected += hdbodypart.Value + ",";
                    }
                }
            }
            if (string.IsNullOrEmpty(strbp))
                strbp = hdbodypart.Value + ",";
            else
                strbp += hdbodypart.Value + ",";

            if (string.IsNullOrEmpty(strnewline))
                strnewline = hdisnewline.Value + ",";
            else
                strnewline += hdisnewline.Value + ",";

        }

        foreach (var item in bodypartselected.TrimEnd(',').Split(','))
        {
            for (int i = 0; i < repRoD.Items.Count; i++)
            {
                TextBox txt1 = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
                CheckBox chk1 = repRoD.Items[i].FindControl("chk") as CheckBox;
                HiddenField hdbodypart1 = repRoD.Items[i].FindControl("bodypart") as HiddenField;
                if (hdbodypart1.Value.Split('-').Count() > 1)
                {
                    if (item.Split('-')[0].Equals(hdbodypart1.Value.Split('-')[0]) && hdbodypart1.Value.Split('-')[1].Equals("p"))
                    {
                        chk1.Checked = true;
                    }
                    else if (hdbodypart1.Value.Split('-')[1].Equals("p") && chk1.Checked && !bodypartselected.Contains(hdbodypart1.Value.Split('-')[0]))
                    {
                        chk1.Checked = false;
                    }
                }
            }
        }

        foreach (var v in bodypart.Split(','))
        {
            for (int i = 0; i < repRoD.Items.Count; i++)
            {
                TextBox txt1 = i == 0 || i == 13 || i == 15 ? repRoD.Items[i].FindControl("txtRod") as TextBox : repRoD.Items[i].FindControl("txtRod1") as TextBox;
                CheckBox chk1 = repRoD.Items[i].FindControl("chk") as CheckBox;
                HiddenField hdbodypart1 = repRoD.Items[i].FindControl("bodypart") as HiddenField;

                if (v.Split('-')[0].Equals(Convert.ToString(hdbodypart1.Value).Split('-')[0]) && !chk1.Checked && v.Split('-').Count() > 1)
                {
                    if (v.Split('-')[1] != hdbodypart1.Value.Split('-')[1] && hdbodypart1.Value.Split('-')[1].Equals("p"))
                    {
                        chk1.Checked = true;
                    }
                    else
                    {
                        if (hdbodypart1.Value.Split('-')[1] == "b" && !chk1.Checked)
                        {
                            if (v.Split('-')[1] == "p")
                            {
                                chk1.Checked = false;
                            }
                        }
                    }

                }
                if (v.Equals(hdbodypart1.Value) && chk1.Checked)
                {
                    chk1.Checked = chk1.Checked;
                }
            }
        }





        txtrodFulldetails.Text = str;

        strDelimit = strDelimit.TrimStart('^');
        hdbodyparts.Value = strbp;
        hdnewline.Value = strnewline;

        return strDelimit;
    }

    protected void lnkierod_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)(sender);
            DataTable dt = (DataTable)(Session["iedata"]);
            hdnrodieid.Value = btn.CommandArgument;
            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientIE_ID=" + Convert.ToInt32(btn.CommandArgument); // query example = "id = 10"

            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblROD where patietn_IE= " + btn.CommandArgument + " and Patiend_FUID is null");
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string printStatus = "Print";
            string downloadStatus = "";



            if (ds.Tables[0].Rows.Count == 0)
            {
                BindRODDeafultValues(dv);
                btnRODDelete.Visible = false;

            }
            else
            {
                BindRODEditValues(ds.Tables[0].Rows[0]["Contentdelimit"].ToString(), ds.Tables[0].Rows[0]["Bodypartdetails"].ToString(), ds.Tables[0].Rows[0]["Newlinedetails"].ToString());
                printStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PrintStatus"].ToString()) ? "Print" : ds.Tables[0].Rows[0]["PrintStatus"].ToString();

                if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Download"))
                {
                    printStatus = "Print";
                    downloadStatus = "Download";
                }
                else if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Downloaded"))
                {
                    printStatus = "Print";
                    downloadStatus = "Downloaded";
                }
                btnRODDelete.Visible = true;
                ViewState["rodid"] = ds.Tables[0].Rows[0]["id"].ToString();
            }


            ltrprint.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='IE' data-id='" + Convert.ToString(btn.CommandArgument) + "'>" + printStatus + "</a> ";
            if (!string.IsNullOrEmpty(downloadStatus))
                ltrdownload.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='IE' data-id='" + Convert.ToString(btn.CommandArgument) + "'>" + downloadStatus + "</a>";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup();", true);
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void lnkfurod_Click(object sender, EventArgs e)
    {
        try
        {

            BusinessLogic bl = new BusinessLogic();
            LinkButton btn = (LinkButton)(sender);

            hdnrodeditedfuid.Value = btn.CommandArgument.Split('-')[0];
            hdnrodeditedfuieid.Value = btn.CommandArgument.Split('-')[1];

            DataTable dt = ToDataTable(bl.GetFUDetails(Convert.ToInt32(hdnrodeditedfuieid.Value)));
            DataView dv = new DataView(dt);
            dv.RowFilter = "PatientFUId=" + Convert.ToInt32(hdnrodeditedfuid.Value); // query example = "id = 10"
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
            DBHelperClass db = new DBHelperClass();
            string query = ("select * from tblROD where Patiend_FUID= " + hdnrodeditedfuid.Value + "");
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            cn.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            string printStatus = "Print";
            string downloadStatus = "";

            if (ds.Tables[0].Rows.Count == 0)
            {
                BindRODDeafultValues(dv, true);
                btnRODDelete.Visible = false;
            }
            else
            {
                BindRODEditValues(ds.Tables[0].Rows[0]["Contentdelimit"].ToString(), ds.Tables[0].Rows[0]["Bodypartdetails"].ToString(), ds.Tables[0].Rows[0]["Newlinedetails"].ToString());
                printStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PrintStatus"].ToString()) ? "Print" : ds.Tables[0].Rows[0]["PrintStatus"].ToString();

                if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Download"))
                {
                    printStatus = "Print";
                    downloadStatus = "Download";
                }
                else if (ds.Tables[0].Rows[0]["PrintStatus"].ToString().Equals("Downloaded"))
                {
                    printStatus = "Print";
                    downloadStatus = "Downloaded";
                }
                btnRODDelete.Visible = true;
                ViewState["rodid"] = ds.Tables[0].Rows[0]["id"].ToString();
            }


            ltrprint.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='FU' data-id='" + hdnrodeditedfuid.Value + "'>" + printStatus + "</a> ";
            if (!string.IsNullOrEmpty(downloadStatus))
                ltrdownload.Text = "<a class='btn btn-link PrintClickRod' data-FUIE='FU' data-id='" + hdnrodeditedfuid.Value + "'>" + downloadStatus + "</a>";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup();", true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Defining type of data column gives proper data table 
            var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name, type);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        //put a breakpoint here and check datatable
        return dataTable;
    }

    private void BindRODEditValues(string val, string bpstr, string newlinestr)
    {
        try
        {
            if (!string.IsNullOrEmpty(val))
            {
                string[] str = val.Split('^');
                string[] strbp = bpstr.Split(',');
                string[] strnl = newlinestr.Split(',');

                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("isChecked", typeof(string)),
                            new DataColumn("name", typeof(string)),
            new DataColumn("bodypart", typeof(string)),
              new DataColumn("isnewline", typeof(string))});

                for (int i = 0; i < str.Length; i++)
                {
                    dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'), strbp[i], strnl[i]);
                    // dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", string.IsNullOrEmpty(str[i]) ? str[i] : str[i].TrimStart('@'));
                }

                repRoD.DataSource = dt;
                repRoD.DataBind();

                //  bindTeratMentPrintvalue();

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnRODDelete_Click(object sender, EventArgs e)
    {
        DBHelperClass dB = new DBHelperClass();
        int val = dB.executeQuery("delete from tblROD where id=" + ViewState["rodid"].ToString());
        if (val > 0)
        {
            string _ieID = Convert.ToString(hdnrodieid.Value);
            string _fuieid = Convert.ToString(hdnrodeditedfuieid.Value);
            string _fufuid = Convert.ToString(hdnrodeditedfuid.Value);

            //if (string.IsNullOrEmpty(_fuieid) && string.IsNullOrEmpty(_fufuid))
            //{
            //    LinkButton btn = new LinkButton();
            //    btn.Text = "RoD";
            //    btn.CommandArgument = _ieID;
            //    lnkierod_Click(btn, e);

            //}
            //else
            //{

            //    LinkButton btn = new LinkButton();
            //    btn.Text = "RoD";
            //    btn.CommandArgument = _fufuid + "-" + _fuieid;
            //    lnkfurod_Click(btn, e);
            //}
        }
    }

    protected void lnkprint_Click(object sender, EventArgs e)
    {
        //try
        //{
        PrintDocumentHelper helper = new PrintDocumentHelper();

        String str = File.ReadAllText(Server.MapPath("~/Template/DocumentPrintIE.html"));

        string prstrCC = "", prstrPE = "", docname = "";

        LinkButton lnk = sender as LinkButton;
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        DBHelperClass db = new DBHelperClass();




        //page1 printing
        string query = ("select * from View_PatientIE where PatientIE_ID= " + lnk.CommandArgument + "");
        DataSet ds = db.selectData(query);


        docname = CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["LastName"].ToString()) + ", " + CommonConvert.UppercaseFirst(ds.Tables[0].Rows[0]["FirstName"].ToString()) + "_" + lnk.CommandArgument + "_IE_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOA"].ToString());

        string gender = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "He" : "She";
        string name = ds.Tables[0].Rows[0]["LastName"].ToString() + ", " + ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString();
        str = str.Replace("#patientname", name);
        str = str.Replace("#dob", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOB"].ToString()));
        str = str.Replace("#doa", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOA"].ToString()));
        str = str.Replace("#dos", CommonConvert.FullDateFormat(ds.Tables[0].Rows[0]["DOE"].ToString()));

        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ADLContent"].ToString()))
            str = str.Replace("#ADL", "<b>ADL CAPABILITIES: </b>" + (ds.Tables[0].Rows[0]["ADLContent"].ToString()) + "<br/>");
        else
            str = str.Replace("#ADL", "");

        string printpage1str = printPage1(lnk.CommandArgument);


        printpage1str = printpage1str.Replace("#gender", gender);

        Int64 idId = Convert.ToInt64(lnk.CommandArgument);
        string strBodypart = getBodyParts(idId, "ie");

        printpage1str = printpage1str.Replace("#bodyparts", strBodypart.ToLower());



        if (ds.Tables[0].Rows[0]["Compensation"].ToString().ToLower() == "wc")
            printpage1str = printpage1str.Replace("#accidenttype", "work related");
        else
            printpage1str = printpage1str.Replace("#accidenttype", "motor related");

        str = str.Replace("#history", printpage1str.Replace("..", "."));


        // header printing

        query = ("select * from tblLocations where Location_ID=" + ds.Tables[0].Rows[0]["Location_Id"]);
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            str = str.Replace("#location", ds.Tables[0].Rows[0]["NameOfPractice"].ToString());
            str = str.Replace("#phoneno", ds.Tables[0].Rows[0]["Telephone"].ToString());
        }


        //page1 priting
        query = ("select topSectionHTML,OtherValue from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        string other_val = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

            str = str.Replace("#pastmedicalhistory", string.IsNullOrEmpty(page1["PMH"]) ? "" : "<b>PAST MEDICAL HISTORY: </b> " + page1["txt_PMH"].TrimEnd('.') + ".<br /><br/>");
            str = str.Replace("#pastsurgicalhistory", string.IsNullOrEmpty(page1["PSH"]) ? "" : "<b>PAST SURGICAL HISTORY: </b> " + page1["PSH"].TrimEnd('.') + ".<br/><br/>");
            str = str.Replace("#pastmedications", string.IsNullOrEmpty(page1["Medication"]) ? "" : "<b>MEDICATIONS: </b> " + page1["Medication"].TrimEnd('.') + ".<br/><br/>");
            str = str.Replace("#allergies", string.IsNullOrEmpty(page1["Allergies"]) ? "" : "<b>DRUG ALLERGIES: </b> " + page1["Allergies"].TrimEnd('.').ToUpper() + ".<br/><br/>");
            //str = str.Replace("#familyhistory", string.IsNullOrEmpty(page1["FamilyHistory"]) ? "" : "<b>FAMILY HISTORY: </b><br/>" + page1["FamilyHistory"].TrimEnd('.') + ".<br/><br/>");
            str = str.Replace("#familyhistory", "");
            other_val = ds.Tables[0].Rows[0]["OtherValue"].ToString();

        }


        if (!string.IsNullOrEmpty(strBodypart))
        {
            StringBuilder sb = new StringBuilder(strBodypart.TrimStart(','));
            if (sb.ToString().LastIndexOf(",") >= 0)
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);

            str = str.Replace("#CC1", sb.ToString() + " pain." + other_val);
        }
        else
        {
            str = str.Replace("#CC1", "" + other_val);
        }

        query = ("select socialSectionHTML,historyHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string strstatus = new PrintDocumentHelper().getDocumentString(ds.Tables[0].Rows[0]["socialSectionHTML"].ToString());
            str = str.Replace("#socialhistory", string.IsNullOrEmpty(strstatus) ? "" : "<b>SOCIAL HISTORY: </b>" + strstatus.TrimEnd('.').Replace(" .", "") + "<br/><br/>");

            Dictionary<string, string> page1_history = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["historyHTML"].ToString());

            if (page1_history.ContainsKey("txtOverallPain"))
            {
                if (!string.IsNullOrEmpty(page1_history["txtOverallPain"]))
                {
                    string pain = "Overall pain score is " + page1_history["txtOverallPain"].Trim() + "/10.";
                    str = str.Replace("#painScore", pain + "<br/><br/>");
                }
                else
                {
                    str = str.Replace("#painScore", "");
                }
            }
            else
            {
                str = str.Replace("#painScore", "");
            }

        }
        else
        {
            str = str.Replace("#socialhistory", "");
            str = str.Replace("#painScore", "");
        }

        query = ("select accidentHTML from tblPage1HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page1_accident = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["accidentHTML"].ToString());

            string work_status = "", accidentdetails = "";

            if (page1_accident.ContainsKey("txt_details"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_details"]))
                    accidentdetails = accidentdetails + page1_accident["txt_details"].TrimEnd('.') + ". ";
            }

            //if (!string.IsNullOrEmpty(page1_accident["txt_accident_desc"]))
            //    accidentdetails = accidentdetails + gender + " " + page1_accident["txt_accident_desc"].TrimEnd('.') + ". ";

            //str = str.Replace("#accidentdetails", accidentdetails);

            if (page1_accident.ContainsKey("txt_vital"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_vital"]))
                    str = str.Replace("#vital", page1_accident["txt_vital"].Trim() + "<br/>");
                else
                    str = str.Replace("#vital", "");
            }
            else
                str = str.Replace("#vital", "");

            if (page1_accident.ContainsKey("txt_alert"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_alert"]))
                    str = str.Replace("#alert", page1_accident["txt_alert"].Trim() + "<br/><br/>");
                else
                    str = str.Replace("#alert", "");
            }
            else
                str = str.Replace("#alert", "");

            if (page1_accident.ContainsKey("txt_gait_desc"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_gait_desc"]))
                    str = str.Replace("#gait", "<br/><br/><b>GAIT</b>: The patient " + page1_accident["txt_gait_desc"].Trim() + ".");
            }
            else
                str = str.Replace("#gait", "");

            if (!string.IsNullOrEmpty(page1_accident["txt_work_status"]))
                work_status = work_status + page1_accident["txt_work_status"].TrimEnd('.') + ". ";

            if (page1_accident.ContainsKey("txt_imp_rating"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txt_imp_rating"]))
                    str = str.Replace("#IMPRating", "<b>IMPAIRMENT RATING: </b>" + page1_accident["txt_imp_rating"].Trim() + "<br/><br/>");
                else
                    str = str.Replace("#IMPRating", "");
            }
            else
                str = str.Replace("#IMPRating", "");

            //if (!string.IsNullOrEmpty(page1_accident["txtMissed"]))
            //    work_status = work_status + gender + " has missed " + page1_accident["txtMissed"] + " of work after the accident. ";

            //if (!string.IsNullOrEmpty(page1_accident["txtReturnedToWork"]))
            //    work_status = work_status + page1_accident["txtReturnedToWork"].TrimEnd('.') + ". ";

            str = str.Replace("#work_status", string.IsNullOrEmpty(work_status) ? "" : "<b>WORK HISTORY: </b>" + work_status + "<br/><br/>");

            string pastinjury = "";
            if (page1_accident["rdbinjuyes"] == "true")
            {
                pastinjury = gender + " had an  injury to " + page1_accident["txt_injur_past_bp"] + " because of a " + page1_accident["txt_injur_past_how"].TrimEnd('.') + ". ";
            }

            str = str.Replace("#pastinjury", string.IsNullOrEmpty(pastinjury) ? "" : "<b><u>PAST INJURY</u>: </b>" + pastinjury + "<br/><br/>");
            //if (page1_accident["rdbdocyes"] == "true")
            //{
            //    work_status = work_status + gender + " was seen by " + page1_accident["txt_docname"] + " for that injury. ";
            //}


        }

        //treatment priting
        query = ("Select TreatMentDetails,CR,TreatMentDelimit,IMPRating from tblbpOtherPart WHERE PatientIE_ID=" + lnk.CommandArgument + "");
        ds = db.selectData(query);

        string treatment = "", cr = "", IMPRating = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            treatment = this.getTreatment(ds.Tables[0].Rows[0]["TreatMentDelimit"].ToString());
            cr = ds.Tables[0].Rows[0]["CR"].ToString();
            IMPRating = ds.Tables[0].Rows[0]["IMPRating"].ToString();

        }



        if (!string.IsNullOrEmpty(treatment))
            str = str.Replace("#treatment", treatment + "<br/>");
        else
            str = str.Replace("#treatment", "");

        if (!string.IsNullOrEmpty(cr))
            str = str.Replace("#CR", "<b><u>CAUSAL RELATIONSHIP</u>: </b><br/>" + cr + "<br/>");
        else
            str = str.Replace("#CR", "");

        //if (!string.IsNullOrEmpty(IMPRating))
        str = str.Replace("#IMPRating", "<b>IMPAIRMENT RATING: </b>" + IMPRating + "%<br/>");
        //else
        //    str = str.Replace("#IMPRating", "");


        //page2 printing
        query = ("select * from tblPage2HTMLContent where PatientIE_ID= " + lnk.CommandArgument + "");
        ds = db.selectData(query);

        string strRos = "", strRosDenis = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            strRos = ds.Tables[0].Rows[0]["rosSectionNewHTML"].ToString();


            if (!string.IsNullOrEmpty(strRos))
            {
                strRos = strRos.TrimEnd();
                strRos = strRos.Replace("<br />", "");
            }

            //strRosDenis = helper.getDocumentStringDenies(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());
            //if (!string.IsNullOrEmpty(strRosDenis))
            //    strRosDenis = "The patient denies " + strRosDenis.TrimEnd() + ".";
        }
        if (!string.IsNullOrEmpty(strRos))
        {
            strRos = strRos.Replace(".", ". ");
            str = str.Replace("#ROS", "<b>REVIEW OF SYSTEMS:</b> " + strRos + strRosDenis + "<br/><br/>");
        }
        else
            str = str.Replace("#ROS", "<br/><br/>");



        //page4 printing
        query = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID=" + lnk.CommandArgument;
        ds = db.selectData(query);

        string strprocedures = "", note = "", strCare = "", strDaignosis = "", strshoulderrightmri = "", strshoulderleftmri = "", strkneerighttmri = "", strkneeleftmri = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()))
            {
                // strDaignosis = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Cervical spine, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString() + ",";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString().TrimEnd('.') + ".";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString().TrimEnd('.') + ".";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString() + " of the ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Thoracic spine, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString().TrimEnd('.') + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString() + " of the ";

                //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Lumbar spine, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString().TrimEnd('.') + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
                strDaignosis = strDaignosis + " right shoulder, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString()))
                strDaignosis = strDaignosis + " left shoulder, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString().TrimEnd('.') + ". ";



            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the ";

                //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
                strDaignosis = strDaignosis + " right knee, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString().TrimEnd('.') + ". ";

            }


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
                strDaignosis = strDaignosis + " left knee, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

            }


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other1Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other1Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other1Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other2Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other2Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other2Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other3Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other3Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other3Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other4Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other4Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other4Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other5Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other5Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other5Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other6Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other6Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other6Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other7Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other7Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other7sText"].ToString().TrimEnd('.') + ". ";
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OtherMedicine"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + ds.Tables[0].Rows[0]["OtherMedicine"].ToString();
            }

            query = "Select * from tblMedicationRx WHERE PatientIE_ID = " + lnk.CommandArgument + " Order By Medicine";
            DataSet dsDaig = db.selectData(query);

            if (dsDaig != null && dsDaig.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsDaig.Tables[0].Rows.Count; i++)
                {
                    strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + dsDaig.Tables[0].Rows[i]["Medicine"].ToString();
                }
            }


            if (!string.IsNullOrEmpty(strDaignosis))
                str = str.Replace("#diagnostic", "<b><u>DIAGNOSTIC STUDIES</u>:</b> " + strDaignosis + "<br/><br/>");
            else
                str = str.Replace("#diagnostic", "");


            //str = str.Replace("#goal", "<b>GOAL: </b>To increase range of motion, strength, flexibility, to decrease pain and to improve body biomechanics and activities of daily living and improve the functional status.<br/><br/>");
            str = str.Replace("#goal", "");

            strDaignosis = "";


            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Procedures"].ToString()))
                strprocedures = "If the patient continues to have tender palpable taut bands/trigger points with referral patterns as noted in the future on examination, I will consider doing trigger point injections. ";

            str = str.Replace("#procedures", string.IsNullOrEmpty(strprocedures) ? "" : "<b><u>PROCEDURES</u>: </b>" + strprocedures + "<br/><br/>");

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Acupuncture"].ToString()))
                strCare = strCare + ", Acupuncture";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Chiropratic"].ToString()))
                strCare = strCare + ", Chiropratic";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["PhysicalTherapy"].ToString()))
                strCare = strCare + ", PhysicalTherapy";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["AvoidHeavyLifting"].ToString()))
                strCare = strCare + ", AvoidHeavyLifting";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Carrying"].ToString()))
                strCare = strCare + ", Carrying";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ExcessiveBend"].ToString()))
                strCare = strCare + ", ExcessiveBend";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ProlongedSitStand"].ToString()))
                strCare = strCare + ", ProlongedSitStand";

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CareOther"].ToString()))
                strCare = strCare + ", " + ds.Tables[0].Rows[0]["CareOther"].ToString();

            strCare = strCare.TrimStart(',');

            StringBuilder sb = new StringBuilder();
            sb.Append(strCare);

            if (sb.ToString().LastIndexOf(",") > 0)
            {
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
            }

            str = str.Replace("#care", string.IsNullOrEmpty(strCare.TrimStart(',')) ? "" : "<b><u>CARE</u>: </b>" + sb.ToString().TrimEnd('.') + ".<br/><br/>");


            strprocedures = "Universal";
            string strproceduresTemp = "";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Cardiac"].ToString()))
                strproceduresTemp = strproceduresTemp + ", Cardiac";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["WeightBearing"].ToString()))
                strproceduresTemp = strproceduresTemp + ", Weight Bearing";


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ViaVideo"].ToString()))
                strproceduresTemp = strproceduresTemp + ", " + ds.Tables[0].Rows[0]["ViaVideo"].ToString();

            if (string.IsNullOrEmpty(strproceduresTemp))
                strprocedures = "";
            else
                strprocedures = strprocedures + ", " + strproceduresTemp;

            if (!string.IsNullOrEmpty(strprocedures))
            {
                sb = new StringBuilder();
                sb.Append(strprocedures);

                if (sb.ToString().LastIndexOf(",") > 0)
                {
                    sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                }

                strprocedures = sb.ToString() + ". ";
            }

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["EducationProvided"].ToString()))
                strprocedures = strprocedures + "Patient education provided via";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPhysician"].ToString()))
                strprocedures = strprocedures + ", physician ";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPrintedMaterial"].ToString()))
                strprocedures = strprocedures + ", printed material";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaPrintedMaterial"].ToString()))
                strprocedures = strprocedures + ", printed material";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ViaWebsite"].ToString()))
                strprocedures = strprocedures + ", online website references";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["IsViaVedio"].ToString()))
                strprocedures = strprocedures + ", video";



            if (!string.IsNullOrEmpty(strprocedures))
            {
                strprocedures = strprocedures + ".";

                if (strprocedures.IndexOf("and") == 0)
                {
                    sb = new StringBuilder();
                    sb.Append(strprocedures);

                    if (sb.ToString().LastIndexOf(",") > 0)
                    {
                        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                    }
                }

                str = str.Replace("#precautions", string.IsNullOrEmpty(sb.ToString().TrimStart(',')) ? "" : "<b><u>PRECAUTIONS</u>: </b>" + (sb.ToString().TrimStart(',').TrimEnd('.').Replace(",,", ",")) + ".<br/><br/>");
            }
            else
            {
                str = str.Replace("#precautions", "");
            }

            string strComplain = "";
            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultNeuro"].ToString()))
                strComplain = strComplain + ", Neurologist";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultOrtho"].ToString()))
                strComplain = strComplain + ", orthopedist";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultPsych"].ToString()))
                strComplain = strComplain + ", psychiatrist";

            if (CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["ConsultPodiatrist"].ToString()))
                strComplain = strComplain + ", podiatrist";


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ConsultOther"].ToString()))
                strComplain = strComplain + ", " + ds.Tables[0].Rows[0]["ConsultOther"].ToString();

            sb = new StringBuilder();
            sb.Append(strComplain);

            if (sb.ToString().LastIndexOf(",") > 0)
            {
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
            }


            str = str.Replace("#consultation", string.IsNullOrEmpty(sb.ToString().TrimStart(',')) ? "" : "<b><u>CONSULTATION</u>: </b>" + sb.ToString().ToLower().TrimStart(',') + ".<br/><br/> ");

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim()))
                str = str.Replace("#follow-up", "<b><u>FOLLOW-UP</u>: </b>" + ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim() + "<br/><br/>");
            else
                str = str.Replace("#follow-up", "");

            query = "Select * from tblMedicationRx WHERE PatientIE_ID=" + lnk.CommandArgument;
            ds = db.selectData(query);

            string strMedi = "";

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strMedi = strMedi + ds.Tables[0].Rows[i]["Medicine"].ToString() + "<br/>";
                }
            }

            str = str.Replace("#medications", string.IsNullOrEmpty(strMedi) ? "" : "<b><u>MEDICATIONS</u>: </b><br/>" + strMedi + "<br/><br/>");
        }
        else
        {
            str = str.Replace("#medications", "");
            str = str.Replace("#follow-up", "");
            str = str.Replace("#precautions", "");
            str = str.Replace("#care", "");
            str = str.Replace("#procedures", "");
            str = str.Replace("#diagnostic", "");
            str = str.Replace("#consultation", "");
        }

        //diagnoses printing for all body parts

        strDaignosis = "";

        strDaignosis = this.getDiagnosis("Neck", lnk.CommandArgument);
        strDaignosis = strDaignosis + this.getDiagnosis("Midback", lnk.CommandArgument);
        strDaignosis = strDaignosis + this.getDiagnosis("Lowback", lnk.CommandArgument);
        strDaignosis = strDaignosis + this.getDiagnosisRightLeft(lnk.CommandArgument);

        if (!string.IsNullOrEmpty(strDaignosis))
        {
            strDaignosis = strDaignosis.TrimStart('@');
            string[] strDaignosisFinal = strDaignosis.Split('@');

            strDaignosis = "<ol>";
            foreach (var s in strDaignosisFinal)
            {
                strDaignosis = strDaignosis + "<li>" + s + "</li>";
            }

            note = this.getDiagno("tblbpShoulder", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpKnee", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpWrist", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpHip", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpElbow", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpAnkle", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            strDaignosis = strDaignosis + "</ol>";

            str = str.Replace("#diagnoses", "<b>FINAL DIAGNOSES: </b>" + strDaignosis + "<br/>");
        }
        else
        {
            strDaignosis = "<ol>";
            note = this.getDiagno("tblbpShoulder", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpKnee", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpWrist", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpHip", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpElbow", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagno("tblbpAnkle", lnk.CommandArgument);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            strDaignosis = strDaignosis + "</ol>";

            if (string.IsNullOrEmpty(strDaignosis))
                str = str.Replace("#diagnoses", "");
            else
                str = str.Replace("#diagnoses", "<b>FINAL DIAGNOSES: </b>" + strDaignosis + "<br/>");
        }
        //plan printing for all body parts


        string strPlan = "";
        if (!string.IsNullOrEmpty(this.getPOC("Neck", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Neck", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpNeck", lnk.CommandArgument)) == false ? this.getPlan("tblbpNeck", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("MidBack", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("MidBack", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpMidback", lnk.CommandArgument)) == false ? this.getPlan("tblbpMidback", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("LowBack", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("LowBack", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpLowback", lnk.CommandArgument)) == false ? this.getPlan("tblbpLowback", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Shoulder", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Shoulder", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpShoulder", lnk.CommandArgument)) == false ? this.getPlan("tblbpShoulder", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Knee", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Knee", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpKnee", lnk.CommandArgument)) == false ? this.getPlan("tblbpKnee", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Elbow", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Elbow", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpElbow", lnk.CommandArgument)) == false ? this.getPlan("tblbpElbow", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Wrist", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Wrist", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpWrist", lnk.CommandArgument)) == false ? this.getPlan("tblbpWrist", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Hip", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Hip", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpHip", lnk.CommandArgument)) == false ? this.getPlan("tblbpHip", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("Ankle", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("Ankle", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpAnkle", lnk.CommandArgument)) == false ? this.getPlan("tblbpAnkle", lnk.CommandArgument) : "");

        if (!string.IsNullOrEmpty(this.getPOC("OtherPart", lnk.CommandArgument)))
            strPlan = strPlan + "<br/>";
        strPlan = strPlan + this.getPOC("OtherPart", lnk.CommandArgument);

        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlan("tblbpOtherPart", lnk.CommandArgument)) == false ? this.getPlan("tblbpOtherPart", lnk.CommandArgument) : "");


        str = str.Replace("#plan", string.IsNullOrEmpty(strPlan) ? "" : "<br/>" + strPlan + "<br/><br/>");


        //neck printing string
        query = ("select CCvalue from tblbpNeck where PatientIE_ID= " + lnk.CommandArgument + "");
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cn.Open();
        ds = new DataSet();
        da.Fill(ds);


        string neckCC = "", lowbackCC = "", shoudlerCC = "", kneeCC = "", elbowCC = "", wristCC = "", hipCC = "", ankleCC = "";



        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                neckCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());

                neckCC = formatString(neckCC);
                str = str.Replace("#neck", neckCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#neck", "");

            }
        }
        else
        {
            str = str.Replace("#neck", "");

        }

        //neck PE printing string
        query = ("select PEvalue from tblbpNeck where PatientIE_ID= " + lnk.CommandArgument + "");
        string neckPE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                neckCC = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                neckCC = formatString(neckCC);
                str = str.Replace("#PENeck", neckCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#PENeck", "");

            }

        }
        else
            str = str.Replace("#PENeck", neckPE);


        //lowback printing string
        query = ("select CCvalue from tblbpLowback where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                lowbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                lowbackCC = formatString(lowbackCC);
                str = str.Replace("#lowback", lowbackCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#lowback", lowbackCC);
        }
        else
            str = str.Replace("#lowback", lowbackCC);


        //lowback PE printing string
        query = ("select PEvalue  from tblbpLowback where PatientIE_ID= " + lnk.CommandArgument + "");
        string lowbackPE = "", lowbackTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                lowbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                lowbackCC = formatString(lowbackCC);
                str = str.Replace("#PELowback", lowbackCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#PELowback", lowbackCC);

        }
        else
            str = str.Replace("#PELowback", lowbackPE);

        //midback printing string
        string midbackCC = "";
        query = ("select CCvalue from tblbpMidback where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                midbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                midbackCC = formatString(midbackCC);
                str = str.Replace("#midback", midbackCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#midback", midbackCC);
        }
        else
            str = str.Replace("#midback", midbackCC);

        //midback PE printing string
        string midbackPE = "";
        query = ("select PEvalue from tblbpMidback where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                midbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                midbackPE = formatString(midbackPE);

                str = str.Replace("#PEMidback", "<b><u>Thoracic Spine Examination</u>: </b>" + midbackPE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEMidback", midbackPE);
        }
        else
            str = str.Replace("#PEMidback", midbackPE);

        //shoulder printing string
        query = ("select CCvalue from tblbpShoulder where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                shoudlerCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Shoulder");

                note = this.getNoteIE("tblbpShoulder", "FreeFormCC", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    shoudlerCC = shoudlerCC + "<br/>" + note;

                shoudlerCC = formatString(shoudlerCC);
                str = str.Replace("#shoulder", shoudlerCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#shoulder", shoudlerCC);
        }
        else
            str = str.Replace("#shoulder", shoudlerCC);

        //shoulder PE printing string
        query = ("select PEvalue from tblbpshoulder where PatientIE_ID= " + lnk.CommandArgument + "");
        string shoulderPE = "", shoulderTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                shoulderPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                shoulderPE = shoulderPE.Replace(",,", ",").Replace(" ,", ",");
            }


            if (!string.IsNullOrEmpty(shoulderPE))
            {

                note = this.getNoteIE("tblbpShoulder", "FreeForm", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    shoulderPE = shoulderPE + "<br/>" + note;

                str = str.Replace("#PEShoudler", shoulderPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEShoudler", "");

        }
        else
            str = str.Replace("#PEShoudler", shoulderPE);



        //knee printing string
        query = ("select CCvalue from tblbpKnee where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                kneeCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Knee");
                kneeCC = formatString(kneeCC);

                note = this.getNoteIE("tblbpKnee", "FreeFormCC", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    kneeCC = kneeCC + "<br/>" + note;

                str = str.Replace("#knee", kneeCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#knee", kneeCC);
        }
        else
            str = str.Replace("#knee", kneeCC);

        //knee PE printing string
        query = ("select PEvalue from tblbpKnee where PatientIE_ID= " + lnk.CommandArgument + "");
        string kneePE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                kneePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                kneePE = kneePE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(kneePE))
            {
                note = this.getNoteIE("tblbpKnee", "FreeForm", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    kneePE = kneePE + "<br/>" + note;

                str = str.Replace("#PEKnee", kneePE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEKnee", "");

        }
        else
            str = str.Replace("#PEKnee", kneePE);

        //elbow printing string
        query = ("select CCvalue from tblbpElbow where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                elbowCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Elbow");
                elbowCC = formatString(elbowCC);

                note = this.getNoteIE("tblbpElbow", "FreeFormCC", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    elbowCC = elbowCC + "<br/>" + note;

                str = str.Replace("#elbow", elbowCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#elbow", elbowCC);
        }
        else
            str = str.Replace("#elbow", elbowCC);

        //elbow PE printing string
        string elbowPE = "";
        query = ("select  PEvalue  from tblbpElbow where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                elbowPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                elbowPE = elbowPE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(elbowPE))
            {
                note = this.getNoteIE("tblbpElbow", "FreeForm", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    elbowPE = elbowPE + "<br/>" + note;

                elbowPE = formatString(elbowPE);
                str = str.Replace("#PEElbow", elbowPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEElbow", "");

        }
        else
            str = str.Replace("#PEElbow", elbowPE);

        //wrist printing string
        query = ("select CCvalue from tblbpWrist where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                wristCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Wrist");


                note = this.getNoteIE("tblbpWrist", "FreeFormCC", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    wristCC = wristCC + "<br/>" + note;


                wristCC = formatString(wristCC);
                str = str.Replace("#wrist", wristCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#wrist", wristCC);
        }
        else
            str = str.Replace("#wrist", wristCC);

        //hip printing string
        query = ("select CCvalue from tblbpHip where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                hipCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Hip");


                note = this.getNoteIE("tblbpHip", "FreeFormCC", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    hipCC = hipCC + "<br/>" + note;

                hipCC = formatString(hipCC);
                str = str.Replace("#hip", hipCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#hip", hipCC);
        }
        else
            str = str.Replace("#hip", hipCC);

        //hip PE printing string
        string hipPE = "";
        query = ("select PEvalue from tblbpHip where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                hipPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                hipPE = hipPE.Replace(",,", ",");
            }


            if (!string.IsNullOrEmpty(hipPE))
            {
                note = this.getNoteIE("tblbpHip", "FreeForm", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    hipPE = hipPE + "<br/>" + note;

                hipPE = formatString(hipPE);
                str = str.Replace("#PEHip", hipPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEHip", "");
        }
        else
            str = str.Replace("#PEHip", "");


        //ankle printing string
        query = ("select CCvalue from tblbpAnkle where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                ankleCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Ankle");

                note = this.getNoteIE("tblbpAnkle", "FreeFormCC", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    ankleCC = ankleCC + "<br/>" + note;


                ankleCC = formatString(ankleCC);
                str = str.Replace("#ankle", ankleCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#ankle", ankleCC);
        }
        else
            str = str.Replace("#ankle", ankleCC);


        //ankle PE printing string
        string anklePE = "";
        query = ("select PEvalue from tblbpAnkle where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                anklePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                anklePE = anklePE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(anklePE))
            {
                note = this.getNoteIE("tblbpAnkle", "FreeForm", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    anklePE = anklePE + "<br/>" + note;

                anklePE = formatString(anklePE);
                str = str.Replace("#PEAnkle", anklePE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEAnkle", "");

        }
        else
            str = str.Replace("#PEAnkle", anklePE);

        //wrist PE printing string
        string wristPE = "";
        query = ("select PEvalue from tblbpWrist where PatientIE_ID= " + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                wristPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                wristPE = wristPE.Replace(",,", ",");
            }

            if (!string.IsNullOrEmpty(wristPE))
            {
                note = this.getNoteIE("tblbpWrist", "FreeForm", lnk.CommandArgument);

                if (!string.IsNullOrEmpty(note))
                    wristPE = wristPE + "<br/>" + note;

                wristPE = formatString(wristPE);
                str = str.Replace("#PEWrist", wristPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEWrist", "");
        }
        else
            str = str.Replace("#PEWrist", "");

        query = ("Select * from tblbpOtherPart WHERE PatientIE_ID=" + lnk.CommandArgument + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = str.Replace("#otherCC", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersCC"].ToString() + "<br /><br />" : "");
            str = str.Replace("#otherPE", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersPE"].ToString()) ? ds.Tables[0].Rows[0]["OthersPE"].ToString() + "<br /><br />" : "");
            str = str.Replace("#causality", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CR"].ToString()) ? "<b><u> CAUSALITY :</u></b>" + ds.Tables[0].Rows[0]["CR"].ToString() + "<br /><br />" : "");
            str = str.Replace("#otherDiagnoses", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["othersA"].ToString()) ? ds.Tables[0].Rows[0]["othersA"].ToString() + "<br /><br />" : "");
        }
        else
        {
            str = str.Replace("#otherCC", "");
            str = str.Replace("#otherPE", "");
            str = str.Replace("#causality", "");
            str = str.Replace("#causality", "");
            str = str.Replace("#otherDiagnoses", "");
        }


        //print sign

        //string path = "http://aeiuat.dynns.com:82/V3_Test/sign/21.jpg";
        str = str.Replace("#signsrc", "");

        str = this.formatString(str);

        string printStr = str;

        divPrint.InnerHtml = printStr;

        printStr = prstrCC + "\n" + prstrPE;


        createWordDocument(str, docname, lnk.CommandArgument, "");

        string folderPath = Server.MapPath("~/Reports/" + lnk.CommandArgument);

        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available soon for download.')", true);

        //DownloadFiles(folderPath, "IE");

        savePrintRequest(lnk.CommandArgument, "0");

        BindPatientIEDetails();
        // ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available for download after 5 min.')", true);
        //}
        //catch (Exception ex)
        //{
        //}
    }

    private void savePrintRequest(string PatientIEID = "0", string PatientFUID = "0")
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (PatientFUID == "0")
            query = "delete from tblPrintRequestTime where PatientIE_Id=" + PatientIEID;
        else
            query = "delete from tblPrintRequestTime where PatientFU_Id=" + PatientFUID;
        db.executeQuery(query);

        query = "insert into tblPrintRequestTime values(" + PatientIEID + "," + PatientFUID + ",getdate())";

        db.executeQuery(query);
    }

    public bool downloadVisible(string PatientIEID = "0", string PatientFUID = "0")
    {

        string folderID = "";
        if (PatientFUID != "0")
            folderID = PatientIEID + "_" + PatientFUID;
        else
            folderID = PatientIEID;

        string path = Server.MapPath("~/Reports/Done/" + folderID);

        if (Directory.Exists(path))
            return true;
        else
            return false;

    }

    public string getTPString(string sides, string sidesText)
    {
        string str = "";


        if (!string.IsNullOrEmpty(sides))
        {
            string[] val = sides.Split(',');
            string[] valText = sidesText.Split(',');

            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] != "")
                {
                    str = str + "," + val[i] + " " + valText[i].ToString();
                }
            }
        }

        return str;
    }

    public string getROMString(string nameROM, string valROM, string normalROM, string side = "", string IEFU = "IE", string bodypart = "")
    {
        string str = "";


        if (!string.IsNullOrEmpty(nameROM))
        {
            string[] nameText = nameROM.Split(',');
            string[] valText = valROM.Split(',');
            string[] normalText = normalROM.Split(',');

            for (int i = 0; i < valText.Length; i++)
            {
                if (valText[i] != "")
                {
                    if (bodypart == "Neck" || bodypart == "Midback" || bodypart == "Lowback")
                    {
                        if (IEFU == "IE")
                        {
                            if (string.IsNullOrEmpty(side))
                                str = str + " " + nameText[i] + " is " + valText[i] + " degrees, normal is " + normalText[i] + " degrees;";
                            else
                                str = str + " " + side + " " + nameText[i] + " is " + valText[i] + " degrees, normal is " + normalText[i] + " degrees;";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(side))
                                str = str + " " + nameText[i] + " is " + valText[i] + " degrees;";
                            else
                                str = str + " " + side + " " + nameText[i] + " is " + valText[i] + " degrees;";
                        }
                    }
                    else
                    {
                        if (IEFU == "IE")
                        {
                            str = str + " " + nameText[i] + " is " + valText[i] + " degrees, normal is " + normalText[i] + " degrees;";
                        }
                        else
                        {

                            str = str + " " + nameText[i] + " is " + valText[i] + " degrees;";
                        }
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(str))
            return str.TrimEnd(';') + ".";
        else
            return str;
    }

    public string printPage1(string patientIE_ID)
    {

        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        DBHelperClass db = new DBHelperClass();

        string query = ("select accidentHTML,historyHTML,historyHTMLValue from tblPage1HTMLContent where PatientIE_ID= " + patientIE_ID + "");
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        cn.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);

        string str = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = ds.Tables[0].Rows[0]["historyHTMLValue"].ToString();

        }
        return str;
    }


    public string printPage1FU()
    {

        string _body = "This is a #age-year-old #handed dominant #gender who presents for follow-up evaluation of injuries sustained in a #wc incident on #DOA.";

        return _body;

    }

    public void createWordDocument(string strHTML, string docname, string patientIE_ID = "", string patientFU_ID = "")
    {
        try
        {

            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            System.Web.UI.HtmlControls.HtmlGenericControl createDiv =
   new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");

            createDiv.InnerHtml = strHTML;

            string strFileName = docname + ".doc";

            createDiv.DataBind();
            createDiv.RenderControl(hw);

            string strPath = "";
            strPath = Server.MapPath("~/Reports");

            //if (!string.IsNullOrEmpty(patientIE_ID))
            //    strPath = Server.MapPath("~/Reports/" + patientIE_ID + "/print");
            //else
            //    strPath = Server.MapPath("~/Reports/" + patientFU_ID + "/print");

            if (Directory.Exists(strPath) == false)
                Directory.CreateDirectory(strPath);


            StreamWriter sWriter = new StreamWriter(strPath + "/" + strFileName);
            sWriter.Write(sw.ToString());
            sWriter.Close();



            //downloadfile(strFileName);

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Charset = "";
            //HttpContext.Current.Response.ContentType = "application/msword";
            //string strFileName = docname + ".doc";
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
            //StringBuilder strHTMLContent = new StringBuilder();
            //strHTMLContent.Append("<html><body>" + strHTML + "</body></html>");


            //HttpContext.Current.Response.Write(strHTMLContent);
            //HttpContext.Current.Response.End();
            //HttpContext.Current.Response.Flush();

            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
        }

    }

    protected void lnkprintFU_Click(object sender, EventArgs e)
    {
        LinkButton lnkfu = sender as LinkButton;
        string val = lnkfu.CommandArgument;
        //try
        //{
        string PatientFU_ID = val.Split(',')[1];
        string PatientIE_ID = val.Split(',')[0];
        PrintDocumentHelper helper = new PrintDocumentHelper();

        String str = File.ReadAllText(Server.MapPath("~/Template/DocumentPrintFU.html"));

        string prstrCC = "", prstrPE = "", docname = "";

        LinkButton lnk = sender as LinkButton;
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
        DBHelperClass db = new DBHelperClass();


        //page1 printing
        string query = ("select * from View_PatientFU where PatientFU_ID= " + PatientFU_ID + "");
        DataSet ds = db.selectData(query);

        string IEDOE = CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["IEDOE"].ToString());

        // docname = ds.Tables[0].Rows[0]["FirstName"].ToString() + ", " + ds.Tables[0].Rows[0]["LastName"].ToString() + "_" + PatientFU_ID + "_FU_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["IEDOA"].ToString());
        docname = ds.Tables[0].Rows[0]["LastName"].ToString() + ", " + ds.Tables[0].Rows[0]["FirstName"].ToString() + "_" + PatientFU_ID + "_FU_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["DOE"].ToString()) + "_" + CommonConvert.DateFormatPrint(ds.Tables[0].Rows[0]["IEDOA"].ToString());

        string name = ds.Tables[0].Rows[0]["LastName"].ToString() + ", " + ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString();
        string gender = ds.Tables[0].Rows[0]["Sex"].ToString() == "Mr." ? "male" : "female";
        str = str.Replace("#patientname", name);
        str = str.Replace("#dos", CommonConvert.FullDateFormat(ds.Tables[0].Rows[0]["DOE"].ToString()));
        str = str.Replace("#dob", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["DOB"].ToString()));
        str = str.Replace("#doa", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["IEDOA"].ToString()));

        this.printPTPreport(PatientIE_ID, ds.Tables[0].Rows[0]["FirstName"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString(),
        ds.Tables[0].Rows[0]["DOB"].ToString(),
        ds.Tables[0].Rows[0]["DOE"].ToString(),
         PatientFU_ID);

        string fuNote = this.getFUNote(ds.Tables[0].Rows[0]["FreeForm"].ToString());


        string printpage1str = printPage1FU();


        printpage1str = printpage1str.Replace("#gender", gender);
        printpage1str = printpage1str.Replace("#age", ds.Tables[0].Rows[0]["age"].ToString());
        printpage1str = printpage1str.Replace("#handed", ds.Tables[0].Rows[0]["Handedness"].ToString());
        printpage1str = printpage1str.Replace("#DOA", CommonConvert.DateFormat(ds.Tables[0].Rows[0]["IEDOA"].ToString()));


        string strBodypart = getBodyParts(Convert.ToInt64(PatientFU_ID), "fu");

        printpage1str = printpage1str.Replace("#bodyparts", strBodypart.ToLower());



        if (ds.Tables[0].Rows[0]["Compensation"].ToString().ToLower() == "wc")
            printpage1str = printpage1str.Replace("#wc", "work related");
        else
            printpage1str = printpage1str.Replace("#wc", "motor related");

        if (!string.IsNullOrEmpty(fuNote))
            printpage1str = printpage1str + "<br/>" + fuNote;

        str = str.Replace("#history", printpage1str.Replace("..", "."));

        //header printing

        query = ("select * from tblLocations where Location_ID=" + ds.Tables[0].Rows[0]["Location_Id"]);
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            str = str.Replace("#location", ds.Tables[0].Rows[0]["NameOfPractice"].ToString());
            str = str.Replace("#phoneno", ds.Tables[0].Rows[0]["Telephone"].ToString());
        }
        query = ("select otherValue from tblPage1HTMLContent where PatientIE_ID= " + PatientIE_ID + "");
        ds = db.selectData(query);

        string other_val = "", note = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            //Dictionary<string, string> page1 = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["topSectionHTML"].ToString());

            //str = str.Replace("#pastmedicalhistory", string.IsNullOrEmpty(page1["PMH"]) ? "" : "<b>PAST MEDICAL HISTORY: </b> " + page1["txt_PMH"].TrimEnd('.') + ".<br /><br/>");
            //str = str.Replace("#pastsurgicalhistory", string.IsNullOrEmpty(page1["PSH"]) ? "" : "<b>PAST SURGICAL HISTORY: </b> " + page1["PSH"].TrimEnd('.') + ".<br/><br/>");
            //str = str.Replace("#pastmedications", string.IsNullOrEmpty(page1["Medication"]) ? "" : "<b>MEDICATIONS: </b> " + page1["Medication"].TrimEnd('.') + ".<br/><br/>");
            //str = str.Replace("#allergies", string.IsNullOrEmpty(page1["Allergies"]) ? "" : "<b>DRUG ALLERGIES: </b> " + page1["Allergies"].TrimEnd('.').ToUpper() + ".<br/><br/>");
            ////str = str.Replace("#familyhistory", string.IsNullOrEmpty(page1["FamilyHistory"]) ? "" : "<b>FAMILY HISTORY: </b><br/>" + page1["FamilyHistory"].TrimEnd('.') + ".<br/><br/>");
            //str = str.Replace("#familyhistory", "");
            other_val = ds.Tables[0].Rows[0]["OtherValue"].ToString();

        }


        if (!string.IsNullOrEmpty(strBodypart))
        {
            StringBuilder sb = new StringBuilder(strBodypart.TrimStart(','));
            if (sb.ToString().LastIndexOf(",") >= 0)
                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);

            str = str.Replace("#CC1", sb.ToString() + " pain." + other_val);
        }
        else
        {
            str = str.Replace("#CC1", "" + other_val);
        }

        query = ("select socialSectionHTML from tblPage1FUHTMLContent where PateintFU_ID= " + PatientFU_ID + "");
        ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    string strstatus = new PrintDocumentHelper().getDocumentString(ds.Tables[0].Rows[0]["socialSectionHTML"].ToString());
        //    str = str.Replace("#socialhistory", "<b>SOCIAL HISTORY: </b>" + strstatus + "<br/>");
        //}
        //else
        //{
        //    str = str.Replace("#socialhistory", "");
        //}

        query = ("select accidentHTML from tblPage1HTMLContent where PatientIE_ID= " + PatientIE_ID + "");
        ds = db.selectData(query);

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    Dictionary<string, string> page1_accident = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["accidentHTML"].ToString());

        //    string work_status = "";

        //    if (!string.IsNullOrEmpty(page1_accident["txt_work_status"]))
        //        work_status = work_status + page1_accident["txt_work_status"] + ". ";

        //    if (!string.IsNullOrEmpty(page1_accident["txtMissed"]))
        //        work_status = work_status + gender + " has missed " + page1_accident["txtMissed"] + " of work after the accident. ";

        //    if (!string.IsNullOrEmpty(page1_accident["txtReturnedToWork"]))
        //        work_status = work_status + page1_accident["txtReturnedToWork"] + ". ";



        //    str = str.Replace("#work_status", work_status);

        //}



        //ROS printing
        //query = ("select * from tblPage2HTMLContent where PatientIE_ID= " + PatientIE_ID + "");
        //ds = db.selectData(query);

        //string strRos = "", strRosDenis = "";

        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString()))
        //    {


        //        strRos = helper.getDocumentString(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());


        //        if (!string.IsNullOrEmpty(strRos))
        //            strRos = "The patient admits to " + strRos + ". ";

        //        strRosDenis = helper.getDocumentStringDenies(ds.Tables[0].Rows[0]["rosSectionHTML"].ToString());
        //        if (!string.IsNullOrEmpty(strRosDenis))
        //            strRosDenis = "The patient denies " + strRosDenis + ". ";
        //    }
        //}
        //str = str.Replace("#ROS", strRos + strRosDenis);

        query = "select degreeSectionHTML from tblPage1FUHTMLContent where PateintFU_ID=" + PatientFU_ID;
        ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Dictionary<string, string> page1_accident = new PrintDocumentHelper().getPage1String(ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString());

            if (page1_accident.ContainsKey("txtOtherRestrictions"))
            {
                if (!string.IsNullOrEmpty(page1_accident["txtOtherRestrictions"]))
                    str = str.Replace("#IMPRating", "<b>IMPAIRMENT RATING: </b>" + page1_accident["txtOtherRestrictions"].Trim() + "<br/><br/>");
                else
                    str = str.Replace("#IMPRating", "");
            }
            else
                str = str.Replace("#IMPRating", "");
        }



        //plan printing 

        string strPlan = "";

        strPlan = strPlan + this.getPOCFU("Neck", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpNeck", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpNeck", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("MidBack", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpMidback", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpMidback", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("LowBack", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpLowback", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpLowback", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Shoulder", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpShoulder", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpShoulder", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Knee", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpKnee", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpKnee", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Elbow", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpElbow", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpElbow", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Wrist", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpWrist", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpWrist", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Hip", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpHip", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpHip", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("Ankle", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpAnkle", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpAnkle", PatientFU_ID) : "");

        strPlan = strPlan + this.getPOCFU("OtherPart", PatientIE_ID, PatientFU_ID);
        strPlan = strPlan + (string.IsNullOrEmpty(this.getPlanFU("tblFUbpOtherPart", PatientFU_ID)) == false ? "<br />" + this.getPlanFU("tblFUbpOtherPart", PatientFU_ID) : "");


        str = str.Replace("#plan", string.IsNullOrEmpty(strPlan) ? strPlan : strPlan);


        //page4 printing
        query = "Select * from tblFUPatientFUDetailPage1 WHERE PatientFU_ID=" + PatientFU_ID;
        ds = db.selectData(query);

        string strDaignosis = "", strshoulderrightmri = "", strshoulderleftmri = "", strkneeleftmri = "", strkneerightmri = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()))
            {
                // strDaignosis = Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagCervialBulgeStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Cervical spine, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagCervialBulgeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagCervialBulgeText"].ToString() + ",";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP1"].ToString().TrimEnd('.') + ".";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagCervialBulgeHNP2"].ToString().TrimEnd('.') + ".";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagThoracicBulgeStudy"].ToString() + " of the ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Thoracic spine, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagThoracicBulgeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagThoracicBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP1"].ToString().TrimEnd('.') + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagThoracicBulgeHNP2"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLumberBulgeStudy"].ToString() + " of the ";

                //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString()))
                strDaignosis = strDaignosis + " Lumbar spine, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLumberBulgeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagLumberBulgeText"].ToString() + ", ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString()))
                    strDaignosis = strDaignosis + " HNP at " + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP1"].ToString().TrimEnd('.') + ". ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["DiagLumberBulgeHNP2"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightShoulderStudy"].ToString() + " of the ";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString()))
                strDaignosis = strDaignosis + " right shoulder, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightShoulderDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagRightShoulderText"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftShoulderStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString()))
                strDaignosis = strDaignosis + " left shoulder, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftShoulderDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagLeftShoulderText"].ToString().TrimEnd('.') + ". ";



            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagRightKneeStudy"].ToString() + " of the ";

                //  if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString()))
                strDaignosis = strDaignosis + " right knee, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagRightKneeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagRightKneeText"].ToString().TrimEnd('.') + ". ";

            }


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "");

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["DiagLeftKneeStudy"].ToString() + " of the ";

                // if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString()))
                strDaignosis = strDaignosis + " left knee, done on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DiagLeftKneeDate"].ToString()).ToString("MM/dd/yyyy") + ", " + ds.Tables[0].Rows[0]["DiagLeftKneeText"].ToString().TrimEnd('.') + ". ";

            }


            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other1Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other1Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other1Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other1Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other2Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other2Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other2Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other2Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other3Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other3Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other3Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other3Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other4Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other4Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other4Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other4Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other5Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other5Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other5Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other5Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other6Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other6Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other6Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other6Text"].ToString().TrimEnd('.') + ". ";

            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Date"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + Convert.ToDateTime(ds.Tables[0].Rows[0]["Other7Date"].ToString()).ToString("MM/dd/yyyy") + " - ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Study"].ToString()))
                    strDaignosis = strDaignosis + " " + ds.Tables[0].Rows[0]["Other7Study"].ToString() + " of the ";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Other7Text"].ToString()))
                    strDaignosis = strDaignosis + ds.Tables[0].Rows[0]["Other7sText"].ToString().TrimEnd('.') + ". ";
            }

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OtherMedicine"].ToString()))
            {
                strDaignosis = (!string.IsNullOrEmpty(strDaignosis) ? (strDaignosis + "<br/>") : "") + ds.Tables[0].Rows[0]["OtherMedicine"].ToString();
            }

            if (!string.IsNullOrEmpty(strDaignosis))
                str = str.Replace("#diagnostic", "<b><u>DIAGNOSTIC STUDIES</u>:</b><br/> " + strDaignosis + "<br/><br/>");
            else
                str = str.Replace("#diagnostic", "");




            str = str.Replace("#follow-up", ds.Tables[0].Rows[0]["FollowUpIn"].ToString().Trim());

            //query = "Select * from tblFUMedicationRx WHERE PatientFUid_ID=" + PatientFU_ID;
            //ds = db.selectData(query);

            //if (ds != null && ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        strMedi = strMedi + ds.Tables[0].Rows[0]["Medicine"].ToString() + "<br/>";
            //    }
            //}

            //str = str.Replace("#medications", strMedi);




        }
        else
        {
            str = str.Replace("#diagnostic", "");
        }

        //diagnoses printing for all body parts

        strDaignosis = "";

        strDaignosis = this.getDiagnosisFU("Neck", PatientFU_ID);
        strDaignosis = strDaignosis + this.getDiagnosisFU("Midback", PatientFU_ID);
        strDaignosis = strDaignosis + this.getDiagnosisFU("Lowback", PatientFU_ID);
        strDaignosis = strDaignosis + this.getDiagnosisRightLeftFU(PatientFU_ID);

        if (!string.IsNullOrEmpty(strDaignosis))
        {
            strDaignosis = strDaignosis.TrimStart('@');
            string[] strDaignosisFinal = strDaignosis.Split('@');

            strDaignosis = "<ol>";
            foreach (var s in strDaignosisFinal)
            {
                strDaignosis = strDaignosis + "<li>" + s + "</li>";
            }

            note = this.getDiagnoFU("tblFUbpShoulder", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpKnee", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpWrist", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpHip", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpElbow", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpAnkle", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            strDaignosis = strDaignosis + "</ol>";

            str = str.Replace("#diagnoses", "<b>FINAL DIAGNOSES: </b>" + strDaignosis + "<br/>");
        }
        else
        {
            strDaignosis = "<ol>";
            note = this.getDiagnoFU("tblFUbpShoulder", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpKnee", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpWrist", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpHip", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpElbow", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            note = this.getDiagnoFU("tblFUbpAnkle", PatientFU_ID);
            if (!string.IsNullOrEmpty(note))
                strDaignosis = strDaignosis + "<li>" + note + "</li>";

            strDaignosis = strDaignosis + "</ol>";

            if (string.IsNullOrEmpty(strDaignosis))
                str = str.Replace("#diagnoses", "");
            else
                str = str.Replace("#diagnoses", "<b>FINAL DIAGNOSES: </b>" + strDaignosis + "<br/>");
        }



        //treatment priting
        query = ("Select RecommandationDelimit from tblFUbpOtherPart WHERE PatientFU_ID=" + PatientFU_ID + "");
        ds = db.selectData(query);

        string treatment = "", cr = "", IMPRating = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            treatment = this.getTreatment(ds.Tables[0].Rows[0]["RecommandationDelimit"].ToString());
            //cr = ds.Tables[0].Rows[0]["CR"].ToString();
            //IMPRating = ds.Tables[0].Rows[0]["IMPRating"].ToString();

        }

        if (!string.IsNullOrEmpty(treatment))
            str = str.Replace("#treatment", treatment + "<br/>");
        else
            str = str.Replace("#treatment", "");


        //causality printing

        query = ("Select * from tblFUbpOtherPart WHERE PatientFU_ID=" + PatientFU_ID + "");
        SqlCommand cm = new SqlCommand(query, cn);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = str.Replace("#otherCC", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersCC"].ToString() + "<br /><br />" : "");
            str = str.Replace("#otherPE", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersPE"].ToString()) ? ds.Tables[0].Rows[0]["OthersPE"].ToString() + "<br /><br />" : "");
            str = str.Replace("#causality", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CR"].ToString()) ? "<b><u> CAUSALITY :</u></b>" + ds.Tables[0].Rows[0]["CR"].ToString() + "<br /><br />" : "");
            str = str.Replace("#otherDiagnoses", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersA"].ToString()) ? ds.Tables[0].Rows[0]["OthersA"].ToString() + "<br /><br />" : "");
        }
        else
        {
            str = str.Replace("#otherCC", "");
            str = str.Replace("#otherPE", "");
            str = str.Replace("#causality", "");
            str = str.Replace("#otherDiagnoses", "");
        }


        //neck printing string
        query = ("select CCvalue from tblFUbpNeck where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        cn.Open();
        ds = new DataSet();
        da.Fill(ds);


        string neckCC = "", lowbackCC = "", shoudlerCC = "", kneeCC = "", elbowCC = "", wristCC = "", hipCC = "", ankleCC = "";



        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                neckCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                neckCC = formatString(neckCC);
                str = str.Replace("#neck", neckCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
            {
                str = str.Replace("#neck", "");

            }
        }
        else
        {
            str = str.Replace("#neck", "");

        }





        //neck PE printing string
        query = ("select PEvalue from tblFUbpNeck where PatientFU_ID= " + PatientFU_ID + "");
        string neckPE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                neckPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());

            }


            if (!string.IsNullOrEmpty(neckPE))
            {
                neckPE = formatString(neckPE);
                str = str.Replace("#PENeck", "<b>Cervical Spine Examination: </b>" + neckPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PENeck", "");

        }
        else
            str = str.Replace("#PENeck", neckPE);



        //lowback printing string
        query = ("select CCvalue from tblFUbpLowback where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                lowbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                lowbackCC = formatString(lowbackCC);
                str = str.Replace("#lowback", lowbackCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#lowback", lowbackCC);
        }
        else
            str = str.Replace("#lowback", lowbackCC);


        //lowback PE printing string
        query = ("select PEvalue  from tblFUbpLowback where PatientFU_ID= " + PatientFU_ID + "");
        string lowbackPE = "", lowbackTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                lowbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
            }


            if (!string.IsNullOrEmpty(lowbackPE))
            {
                lowbackPE = formatString(lowbackPE);
                str = str.Replace("#PELowback", "<b>Lumbar Spine Examination: </b>" + lowbackPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PELowback", "");

        }
        else
            str = str.Replace("#PELowback", lowbackPE);

        //midback printing string
        string midbackCC = "";
        query = ("select CCvalue from tblFUbpMidback where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                midbackCC = helper.getDocumentString(ds.Tables[0].Rows[0]["CCvalue"].ToString());
                midbackCC = formatString(midbackCC);
                str = str.Replace("#midback", midbackCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#midback", midbackCC);
        }
        else
            str = str.Replace("#midback", midbackCC);

        //midback PE printing string
        string midbackPE = "", midbackTP = "";
        query = ("select PEvalue from tblFUbpMidback where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                midbackPE = helper.getDocumentString(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                midbackPE = midbackPE.Replace(",,", ",");


                midbackPE = formatString(midbackPE);

                str = str.Replace("#PEMidback", "<b><u>Thoracic Spine Examination</u>: </b>" + midbackPE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEMidback", midbackPE);
        }
        else
            str = str.Replace("#PEMidback", midbackPE);

        //shoulder printing string
        query = ("select CCvalue from tblFUbpShoulder where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                shoudlerCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Shoulder");


                note = this.getNoteFU("tblFUbpShoulder", "FreeFormCC", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    shoudlerCC = shoudlerCC + "<br/>" + note;

                shoudlerCC = formatString(shoudlerCC);
                str = str.Replace("#shoulder", shoudlerCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#shoulder", shoudlerCC);
        }
        else
            str = str.Replace("#shoulder", shoudlerCC);

        //shoulder PE printing string
        query = ("select PEvalue from tblFUbpshoulder where PatientFU_ID= " + PatientFU_ID + "");
        string shoulderPE = "", shoulderTP = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                shoulderPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                shoulderPE = shoulderPE.Replace(",,", ",").Replace(" ,", ",");

            }



            if (!string.IsNullOrEmpty(shoulderPE))
            {
                shoulderPE = shoulderPE.Replace("#rightshouldertitle", "Right Shoulder : ");
                // shoulderPE = shoulderPE.Replace("#shoulderrightrom", "");
                shoulderPE = shoulderPE.Replace("#shoulderrightmri", "");

                shoulderPE = shoulderPE.Replace("#leftshouldertitle", "Left Shoulder : ");
                //  shoulderPE = shoulderPE.Replace("#shoulderleftrom", "");
                shoulderPE = shoulderPE.Replace("#shoulderleftmri", "");

                note = this.getNoteFU("tblFUbpShoulder", "FreeForm", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    shoulderPE = shoulderPE + "<br/>" + note;

                str = str.Replace("#PEShoudler", shoulderPE + "<br/><br/>");


            }
            else
                str = str.Replace("#PEShoudler", "");

        }
        else
            str = str.Replace("#PEShoudler", shoulderPE);

        //knee printing string
        query = ("select CCvalue from tblFUbpKnee where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                kneeCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Knee");

                note = this.getNoteFU("tblFUbpKnee", "FreeFormCC", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    kneeCC = kneeCC + "<br/>" + note;

                kneeCC = formatString(kneeCC);
                str = str.Replace("#knee", kneeCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#knee", kneeCC);
        }
        else
            str = str.Replace("#knee", kneeCC);

        //knee PE printing string
        query = ("select PEvalue from tblFUbpKnee where PatientFU_ID= " + PatientFU_ID + "");
        string kneePE = "";
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {

                kneePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                kneePE = kneePE.Replace(",,", ",");

            }


            if (!string.IsNullOrEmpty(kneePE))
            {

                kneePE = kneePE.Replace("#rightkneetitle", "Right Knee : ");
                // kneePE = kneePE.Replace("#kneerightrom", "");
                kneePE = kneePE.Replace("#kneerightmri", "");

                kneePE = kneePE.Replace("#leftkneetitle", "Left Knee : ");
                //  kneePE = kneePE.Replace("#kneeleftrom", "");
                kneePE = kneePE.Replace("#kneeleftmri", "");

                note = this.getNoteFU("tblFUbpKnee", "FreeForm", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    kneePE = kneePE + "<br/>" + note;

                str = str.Replace("#PEKnee", kneePE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEKnee", "");

        }
        else
            str = str.Replace("#PEKnee", kneePE);


        //elbow printing string
        query = ("select CCvalue from tblFUbpElbow where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);


        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                elbowCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Elbow");


                note = this.getNoteFU("tblFUbpElbow", "FreeFormCC", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    elbowCC = elbowCC + "<br/>" + note;

                elbowCC = formatString(elbowCC);
                str = str.Replace("#elbow", elbowCC.Replace(" /", "/") + "<br/><br/>");
            }
            else
                str = str.Replace("#elbow", elbowCC);
        }
        else
            str = str.Replace("#elbow", elbowCC);

        //elbow PE printing string
        string elbowPE = "";
        query = ("select  PEvalue  from tblFUbpElbow where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                elbowPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                elbowPE = elbowPE.Replace(",,", ",");
            }



            if (!string.IsNullOrEmpty(elbowPE))
            {
                elbowPE = elbowPE.Replace("#rightelbowtitle", "Right Elbow : ");
                // elbowPE = kneePE.Replace("#kneerightrom", "");
                elbowPE = elbowPE.Replace("#elbowrightmri", "");

                elbowPE = elbowPE.Replace("#leftelbowtitle", "Left Elbow : ");
                //  elbowPE = kneePE.Replace("#kneeleftrom", "");
                elbowPE = elbowPE.Replace("#elbowleftmri", "");

                note = this.getNoteFU("tblFUbpElbow", "FreeForm", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    elbowPE = elbowPE + "<br/>" + note;

                elbowPE = formatString(elbowPE);
                str = str.Replace("#PEElbow", elbowPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEElbow", "");

        }
        else
            str = str.Replace("#PEElbow", elbowPE);

        //wrist printing string
        query = ("select CCvalue from tblFUbpWrist where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                wristCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Wrist");

                note = this.getNoteFU("tblFUbpWrist", "FreeFormCC", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    wristCC = wristCC + "<br/>" + note;

                wristCC = formatString(wristCC);
                str = str.Replace("#wrist", wristCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#wrist", wristCC);
        }
        else
            str = str.Replace("#wrist", wristCC);

        //hip printing string
        query = ("select CCvalue from tblFUbpHip where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                hipCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Hip");

                note = this.getNoteFU("tblFUbpHip", "FreeFormCC", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    hipCC = hipCC + "<br/>" + note;

                hipCC = formatString(hipCC);
                str = str.Replace("#hip", hipCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#hip", hipCC);
        }
        else
            str = str.Replace("#hip", hipCC);

        //hip PE printing string
        string hipPE = "";
        query = ("select PEvalue from tblFUbpHip where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                hipPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                hipPE = hipPE.Replace(",,", ",");
            }



            if (!string.IsNullOrEmpty(hipPE))
            {

                hipPE = hipPE.Replace("#rightHiptitle", "Right Hip : ");
                // elbowPE = kneePE.Replace("#kneerightrom", "");
                hipPE = hipPE.Replace("#hiprightmri", "");

                hipPE = hipPE.Replace("#lefthiptitle", "Left Hip : ");
                //  elbowPE = kneePE.Replace("#kneeleftrom", "");
                hipPE = hipPE.Replace("#hipleftmri", "");


                note = this.getNoteFU("tblFUbpHip", "FreeForm", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    hipPE = hipPE + "<br/>" + note;


                hipPE = formatString(hipPE);
                str = str.Replace("#PEHip", hipPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEHip", "");
        }
        else
            str = str.Replace("#PEHip", "");


        //ankle printing string
        query = ("select CCvalue from tblFUbpAnkle where PatientFU_ID=" + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CCvalue"].ToString()))
            {
                ankleCC = helper.getDocumentStringLeftRight(ds.Tables[0].Rows[0]["CCvalue"].ToString(), "Ankle");

                note = this.getNoteFU("tblFUbpAnkle", "FreeFormCC", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    ankleCC = ankleCC + "<br/>" + note;

                ankleCC = formatString(ankleCC);
                str = str.Replace("#ankle", ankleCC.Replace(" /", "/") + "<br/><br/>");

            }
            else
                str = str.Replace("#ankle", ankleCC);
        }
        else
            str = str.Replace("#ankle", ankleCC);


        //ankle PE printing string
        string anklePE = "";
        query = ("select PEvalue from tblFUbpAnkle where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                anklePE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                anklePE = anklePE.Replace(",,", ",");
            }



            if (!string.IsNullOrEmpty(anklePE))
            {
                anklePE = anklePE.Replace("#rightankletitle", "Right Ankle : ");
                // elbowPE = kneePE.Replace("#kneerightrom", "");
                anklePE = anklePE.Replace("#anklerightmri", "");

                anklePE = anklePE.Replace("#leftankletitle", "Left Ankle : ");
                //  elbowPE = kneePE.Replace("#kneeleftrom", "");
                anklePE = anklePE.Replace("#ankleleftmri", "");

                note = this.getNoteFU("tblFUbpAnkle", "FreeForm", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    anklePE = anklePE + "<br/>" + note;



                anklePE = formatString(anklePE);
                str = str.Replace("#PEAnkle", anklePE + "<br/><br/>");

            }
            else
                str = str.Replace("#PEAnkle", "");

        }
        else
            str = str.Replace("#PEAnkle", anklePE);



        //wrist PE printing string
        string wristPE = "";
        query = ("select PEvalue from tblFUbpWrist where PatientFU_ID= " + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PEvalue"].ToString()))
            {
                wristPE = helper.getDocumentStringLeftRightPE(ds.Tables[0].Rows[0]["PEvalue"].ToString());
                wristPE = wristPE.Replace(",,", ",");
            }


            if (!string.IsNullOrEmpty(wristPE))
            {

                wristPE = wristPE.Replace("#rightwristtitle", "Right Wrist : ");
                // elbowPE = kneePE.Replace("#kneerightrom", "");
                wristPE = wristPE.Replace("#wristrightmri", "");

                wristPE = wristPE.Replace("#leftwristtitle", "Left Wrist : ");
                //  elbowPE = kneePE.Replace("#kneeleftrom", "");
                wristPE = wristPE.Replace("#wristleftmri", "");


                note = this.getNoteFU("tblFUbpWrist", "FreeForm", PatientFU_ID);

                if (!string.IsNullOrEmpty(note))
                    wristPE = wristPE + "<br/>" + note;


                wristPE = formatString(wristPE);
                str = str.Replace("#PEWrist", wristPE + "<br/><br/>");
            }
            else
                str = str.Replace("#PEWrist", "");
        }
        else
            str = str.Replace("#PEWrist", "");


        


        query = ("Select* from tblFUbpOtherPart WHERE PatientFU_ID=" + PatientFU_ID + "");
        cm = new SqlCommand(query, cn);
        da = new SqlDataAdapter(cm);
        ds = new DataSet();
        da.Fill(ds);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            str = str.Replace("#otherCC", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersCC"].ToString() : "");
            str = str.Replace("#otherPE", !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()) ? ds.Tables[0].Rows[0]["OthersPE"].ToString() : "");

        }
        else
        {
            str = str.Replace("#otherCC", "");
            str = str.Replace("#otherPE", "");
        }

        bool isDisplayCC = displayCCinPrint();

        if (isDisplayCC)
        {
            str = str.Replace("#display", "block");
        }
        else
        {
            str = str.Replace("#display", "none");
        }

        string printStr = str;

        divPrint.InnerHtml = printStr;

        createWordDocument(str, docname, PatientFU_ID, "");

        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available soon for download.')", true);
        savePrintRequest("0", PatientFU_ID);

        // string folderPath = Server.MapPath("~/Reports/" + PatientFU_ID);

        // DownloadFiles(folderPath, "FU");

    }

    private string getDiagnosis(string bodypart, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "Select * from tblDiagCodesDetail WHERE PatientIE_ID = " + PatientIE_ID + " and PatientFU_ID is null AND BodyPart LIKE '%" + bodypart + "%' Order By BodyPart, Description";

        DataSet dsDaigCode = db.selectData(query);

        string strDaignosis = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            //strDaignosis = strDaignosis + "<br/><br/><b>" + bodypart + ":</b>";
            for (int i = 0; i < dsDaigCode.Tables[0].Rows.Count; i++)
            {
                //strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
                strDaignosis = strDaignosis + "@" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString();
            }
        }

        if (bodypart != "Other")
        {
            query = "select FreeFormA from tblbp" + bodypart + " where PatientIE_ID = " + PatientIE_ID;
        }
        else
        {
            query = "select OthersA from tblbpOtherPart where PatientIE_ID = " + PatientIE_ID;
        }
        dsDaigCode = null;
        dsDaigCode = db.selectData(query);
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strDaignosis = strDaignosis + "@" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }


        return strDaignosis;
    }

    private string getDiagnosisRightLeft(string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();

        SqlParameter[] parameters = new SqlParameter[1];
        parameters[0] = new SqlParameter("@PatientIE_ID", PatientIE_ID);

        DataSet dsDaigCode = db.executeSelectSP("nusp_getDiagnosis", parameters);

        string strDaignosis = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            //strDaignosis = strDaignosis + "<br/><br/><b>" + bodypart + ":</b>";
            for (int i = 0; i < dsDaigCode.Tables[0].Rows.Count; i++)
            {
                //strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
                if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[i]["Description"].ToString()))
                    strDaignosis = strDaignosis + "@" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
            }
        }

        return strDaignosis;
    }

    private string getDiagnosisFU(string bodypart, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "Select * from tblDiagCodesDetail WHERE PatientFU_ID = " + PatientFU_ID + "  AND BodyPart LIKE '%" + bodypart + "%' Order By BodyPart, Description";

        DataSet dsDaigCode = db.selectData(query);

        string strDaignosis = "";
        if (dsDaigCode != null)
        {
            //strDaignosis = strDaignosis + "<br/><br/><b>" + bodypart + ":</b>";
            for (int i = 0; i < dsDaigCode.Tables[0].Rows.Count; i++)
            {
                //strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
                strDaignosis = strDaignosis + "@" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString();
            }
        }

        if (bodypart != "Other")
        {
            query = "select FreeFormA from tblFUbp" + bodypart + " where PatientFU_ID = " + PatientFU_ID;
        }
        else
        {
            query = "select OthersA from tblFUbpOtherPart where PatientFU_ID = " + PatientFU_ID;
        }
        dsDaigCode = null;
        dsDaigCode = db.selectData(query);
        if (dsDaigCode != null)
        {
            if (dsDaigCode.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
                {
                    strDaignosis = strDaignosis + "@" + dsDaigCode.Tables[0].Rows[0][0].ToString();
                }
            }
        }


        return strDaignosis;
    }

    private string getDiagnosisRightLeftFU(string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();

        SqlParameter[] parameters = new SqlParameter[1];
        parameters[0] = new SqlParameter("@PatientFU_ID", PatientFU_ID);

        DataSet dsDaigCode = db.executeSelectSP("nusp_getFUDiagnosis", parameters);

        string strDaignosis = "";
        if (dsDaigCode != null)
        {
            //strDaignosis = strDaignosis + "<br/><br/><b>" + bodypart + ":</b>";
            for (int i = 0; i < dsDaigCode.Tables[0].Rows.Count; i++)
            {
                //strDaignosis = strDaignosis + "<br/>" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
                if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[i]["Description"].ToString()))
                    strDaignosis = strDaignosis + "@" + dsDaigCode.Tables[0].Rows[i]["Description"].ToString() + " - " + dsDaigCode.Tables[0].Rows[i]["DiagCode"].ToString();
            }
        }

        return strDaignosis;
    }



    private string getPlan(string tablename, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (tablename != "tblbpOtherPart")
            query = "Select FreeFormP from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;
        else
            query = "Select OthersP from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getPlanFU(string tablename, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (tablename != "tblFUbpOtherPart")
            query = "Select FreeFormP from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;
        else
            query = "Select OthersP from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getDiagno(string tablename, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (tablename != "tblbpOtherPart")
            query = "Select FreeFormA from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;
        else
            query = "Select OthersA from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getDiagnoFU(string tablename, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";
        if (tablename != "tblFUbpOtherPart")
            query = "Select FreeFormA from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;
        else
            query = "Select OthersA from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan +  dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getNoteFU(string tablename, string colName, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";


        if (tablename != "tblFUbpOtherPart")
            query = "Select " + colName + " from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;
        else
            query = "Select " + colName + " from " + tablename + " WHERE PatientFU_ID = " + PatientFU_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getNoteIE(string tablename, string colName, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";


        if (tablename != "tblFUbpOtherPart")
            query = "Select " + colName + " from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;
        else
            query = "Select " + colName + " from " + tablename + " WHERE PatientIE_ID = " + PatientIE_ID;

        DataSet dsDaigCode = db.selectData(query);

        string strPlan = "";
        if (dsDaigCode != null && dsDaigCode.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsDaigCode.Tables[0].Rows[0][0].ToString()))
            {
                strPlan = strPlan + "<br/>" + dsDaigCode.Tables[0].Rows[0][0].ToString();
            }
        }
        return strPlan;
    }

    private string getPOC(string bodypart, string PatientIE_ID)
    {
        DBHelperClass db = new DBHelperClass();


        string SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + PatientIE_ID + " AND BodyPart = '" + bodypart + "'  and IsConsidered=0 Order By BodyPart,Heading"; ;


        DataSet dsPOC = db.selectData(SqlStr);

        string strPoc = "";
        if (dsPOC != null && dsPOC.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dsPOC.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dsPOC.Tables[0].Rows[i]["Heading"].ToString()))
                {
                    //if (i != dsPOC.Tables[0].Rows.Count - 1)
                    //    strPoc = strPoc + "<b style='text-transform:uppercase'>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString().TrimEnd(':') + ": </b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString() + "<br/><br/>";
                    //else
                    strPoc = strPoc + "<b style='text-transform:uppercase'>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString().TrimEnd(':') + ": </b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString() + "<br/><br/>";
                }
            }
        }
        return strPoc;
    }

    private string getPOCFU(string bodypart, string PatientIE_ID, string PatientFU_ID)
    {
        DBHelperClass db = new DBHelperClass();
        string SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + PatientIE_ID + " AND BodyPart = '" + bodypart + "' AND PatientFU_ID = '" + PatientFU_ID + "'  and IsConsidered=0 Order By BodyPart,Heading"; ;


        DataSet dsPOC = db.selectData(SqlStr);

        string strPoc = "";
        if (dsPOC != null && dsPOC.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < dsPOC.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dsPOC.Tables[0].Rows[i]["Heading"].ToString()))
                {
                    if (i != dsPOC.Tables[0].Rows.Count - 1)
                        strPoc = strPoc + "<br/><br/><b>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString() + "</b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString() + "<br/><br/>";
                    else
                        strPoc = strPoc + "<br/><br/><b>" + dsPOC.Tables[0].Rows[i]["Heading"].ToString() + "</b>" + dsPOC.Tables[0].Rows[i]["PDesc"].ToString();
                }
            }
        }
        return strPoc;
    }

    private bool displayCCinPrint()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
        bool display = Convert.ToBoolean(nodeList[0].SelectSingleNode("displayCCprint").InnerText);
        return display;
    }

    private void printPTPreport(string PatientIE_ID, string fname, string lname, string dob, string doe, string PatientFU_ID = "0")
    {
        String str = File.ReadAllText(Server.MapPath("~/Template/PTP.html"));
        string query = "", docname = "";

        if (PatientFU_ID == "0")
        {
            query = "Select * from tblInjuredBodyParts Where PatientIE_ID =" + PatientIE_ID;
            docname = lname + "," + fname + "_" + PatientIE_ID + "_PTP_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);
        }
        else
        {
            query = "Select * from tblInjuredBodyParts Where PatientFU_ID =" + PatientFU_ID;
            docname = lname + "," + fname + "_" + PatientFU_ID + "_PTP_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);
        }

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData(query);

        str = str.Replace("#patient", fname + " " + lname);
        str = str.Replace("#date", CommonConvert.DateFormat(doe));
        str = str.Replace("#dob", CommonConvert.DateFormat(dob));

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["Neck"].ToString().ToLower() == "true")
            {
                str = str.Replace("#c01", "x");
                str = str.Replace("#c03", "x");
                str = str.Replace("#c04", "x");
            }
            else
            {

                str = str.Replace("#c01", " ");
                str = str.Replace("#c03", " ");
                str = str.Replace("#c04", " ");
            }
            str = str.Replace("#c02", " ");
            str = str.Replace("#c05", " ");
            str = str.Replace("#c06", " ");
            str = str.Replace("#c07", " ");
            str = str.Replace("#c08", " ");
            str = str.Replace("#c09", " ");
            str = str.Replace("#c10", " ");

            if (ds.Tables[0].Rows[0]["LowBack"].ToString().ToLower() == "true")
            {
                str = str.Replace("#l01", "x");
                str = str.Replace("#l03", "x");
                str = str.Replace("#l04", "x");
            }
            else
            {
                str = str.Replace("#l01", "");
                str = str.Replace("#l03", "");
                str = str.Replace("#l04", "");
            }

            str = str.Replace("#l02", " ");
            str = str.Replace("#l05", " ");
            str = str.Replace("#l06", " ");
            str = str.Replace("#l07", " ");
            str = str.Replace("#l08", " ");
            str = str.Replace("#l09", " ");
            str = str.Replace("#l10", " ");
            str = str.Replace("#l11", " ");
            str = str.Replace("#l12", " ");
            str = str.Replace("#l13", " ");
            str = str.Replace("#l14", " ");
            str = str.Replace("#l15", " ");
            str = str.Replace("#l16", " ");
            str = str.Replace("#l17", " ");
            str = str.Replace("#l18", " ");
            str = str.Replace("#l19", " ");
            str = str.Replace("#l20", " ");

            if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString().ToLower() == "true" && ds.Tables[0].Rows[0]["RightShoulder"].ToString().ToLower() == "true")
                str = str.Replace("#shoulderaspect", "Bilateral");
            else if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString().ToLower() == "true")
                str = str.Replace("#shoulderaspect", "Left");
            else if (ds.Tables[0].Rows[0]["RightShoulder"].ToString().ToLower() == "true")
                str = str.Replace("#shoulderaspect", "Right");
            else
                str = str.Replace("#shoulderaspect", "Shoulder");

            if (ds.Tables[0].Rows[0]["LeftShoulder"].ToString().ToLower() == "true" || ds.Tables[0].Rows[0]["RightShoulder"].ToString().ToLower() == "true")
            {
                str = str.Replace("#s01", "x");
                str = str.Replace("#s02", "x");
                str = str.Replace("#s03", "x");
                str = str.Replace("#s05", "x");
            }
            else
            {
                str = str.Replace("#s01", "");
                str = str.Replace("#s02", "");
                str = str.Replace("#s03", "");
                str = str.Replace("#s05", "");
            }

            str = str.Replace("#s04", "");
            str = str.Replace("#s06", "");
            str = str.Replace("#s07", "");
            str = str.Replace("#s08", "");
            str = str.Replace("#s09", "");
            str = str.Replace("#s10", "");

            if (ds.Tables[0].Rows[0]["LeftKnee"].ToString().ToLower() == "true" && ds.Tables[0].Rows[0]["RightKnee"].ToString().ToLower() == "true")
                str = str.Replace("#kneeaspect", "Bilateral");
            else if (ds.Tables[0].Rows[0]["LeftKnee"].ToString().ToLower() == "true")
                str = str.Replace("#kneeaspect", "Left");
            else if (ds.Tables[0].Rows[0]["RightKnee"].ToString().ToLower() == "true")
                str = str.Replace("#kneeaspect", "Right");
            else
                str = str.Replace("#kneeaspect", "Kneess");

            if (ds.Tables[0].Rows[0]["LeftKnee"].ToString().ToLower() == "true" || ds.Tables[0].Rows[0]["RightKnee"].ToString().ToLower() == "true")
            {
                str = str.Replace("#k01", "x");
                str = str.Replace("#k02", "x");
                str = str.Replace("#k03", "x");
                str = str.Replace("#k05", "x");
            }
            else
            {
                str = str.Replace("#k01", "");
                str = str.Replace("#k02", "");
                str = str.Replace("#k03", "");
                str = str.Replace("#k05", "");
            }

            str = str.Replace("#k04", "");
            str = str.Replace("#k06", "");
            str = str.Replace("#k07", "");
            str = str.Replace("#k08", "");
            str = str.Replace("#k09", "");
            str = str.Replace("#k10", "");

            str = str.Replace("#o11", "");
            str = str.Replace("#o12", "");
            str = str.Replace("#o13", "");
            str = str.Replace("#o14", "");
            str = str.Replace("#o15", "");
            str = str.Replace("#o16", "");
            str = str.Replace("#o17", "");
            str = str.Replace("#o18", "");
            str = str.Replace("#o19", "");
            str = str.Replace("#o20", "");


            if (PatientFU_ID == "0")
                createWordDocument(str, docname, PatientIE_ID);
            else
                createWordDocument(str, docname, "", PatientFU_ID);

        }

    }

    private void printCFreport(string PatientIE_ID, string fname, string lname, string dob, string doe, string location, string PatientFU_ID = "0")
    {
        String str = File.ReadAllText(Server.MapPath("~/Template/CF.html"));

        string sSQLCustomQuery = " Muscle, Sides, Level, CASE WHEN ISDATE(Requested) = 1 THEN ADesc ";
        sSQLCustomQuery = sSQLCustomQuery + "Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_ADesc Else ";
        sSQLCustomQuery = sSQLCustomQuery + "CASE WHEN ISDATE(Executed) = 1 THEN E_ADesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS ADesc,CASE WHEN ISDATE(Requested) = 1 THEN PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDesc,CASE WHEN ISDATE(Requested) = 1 THEN CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_CCDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS CCDesc,CASE WHEN ISDATE(Requested) = 1 THEN PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PEDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PEDesc, CASE WHEN ISDATE(Requested) = 1 THEN Requested ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDATE, CASE WHEN ISDATE(Requested) = 1 THEN Requested_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed_Position ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS Position, CASE WHEN ISDATE(Requested) = 1 THEN Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_Heading ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END As Heading ";

        string query = "", docname = "";
        if (PatientFU_ID != "0")
        {
            query = "SELECT " + sSQLCustomQuery + " FROM tblProceduresDetail WHERE IsConsidered<> 1 AND(CF = 'True') AND PatientFU_ID IS NULL AND(PatientIE_ID = " + PatientIE_ID + ")";
            docname = lname + "," + fname + "_" + PatientFU_ID + "_CF_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);
        }
        else
        {
            query = "SELECT " + sSQLCustomQuery + " FROM tblProceduresDetail WHERE IsConsidered<> 1 AND(CF = 'True') AND PatientFU_ID=" + PatientFU_ID + " AND(PatientIE_ID = " + PatientIE_ID + ")";
            docname = lname + "," + fname + "_" + PatientIE_ID + "_CF_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(dob);

        }


        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData(query);

        string strHeading = "";

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Heading"].ToString()))
                {
                    strHeading = strHeading + "," + ds.Tables[0].Rows[i]["Heading"].ToString();
                    strHeading = strHeading.Replace("Procedure:", "");
                    strHeading = strHeading.Replace("Request:", "");

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Sides"].ToString()))
                    {
                        strHeading = strHeading.Replace("(side)", ds.Tables[0].Rows[i]["Sides"].ToString());
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Level"].ToString()))
                    {
                        strHeading = strHeading.Replace("(level)", ds.Tables[0].Rows[i]["Level"].ToString());
                    }
                }
            }
        }



        StringBuilder sb = new StringBuilder(strHeading.TrimStart(','));

        if (sb.ToString().LastIndexOf(",") >= 0)
            sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);

        str = str.Replace("#patient", fname + " " + lname);
        str = str.Replace("#date", doe);
        str = str.Replace("#dob", dob);
        str = str.Replace("#location", location);
        str = str.Replace("#headings", sb.ToString());
        str = str.Replace("#dos", "<u>&nbsp;&nbsp;&nbsp;&nbsp;" + doe + "&nbsp;&nbsp;&nbsp;&nbsp;</u>");



        if (PatientFU_ID == "0")
            createWordDocument(str, docname, PatientIE_ID);
        else
            createWordDocument(str, docname, "", PatientFU_ID);

        //}

    }

    private void printPNreport(string PatientIE_ID, string fname, string lname, string doa, string doe, string location, string dob, string PatientFU_ID = "0")
    {

        string printtype = "";

        string sSQLCustomQuery = " Muscle, Sides, Level, CASE WHEN ISDATE(Requested) = 1 THEN ADesc ";
        sSQLCustomQuery = sSQLCustomQuery + "Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_ADesc Else ";
        sSQLCustomQuery = sSQLCustomQuery + "CASE WHEN ISDATE(Executed) = 1 THEN E_ADesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS ADesc,CASE WHEN ISDATE(Requested) = 1 THEN PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDesc,CASE WHEN ISDATE(Requested) = 1 THEN CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_CCDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_CCDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS CCDesc,CASE WHEN ISDATE(Requested) = 1 THEN PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_PEDesc ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_PEDesc ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PEDesc, CASE WHEN ISDATE(Requested) = 1 THEN Requested ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS PDATE, CASE WHEN ISDATE(Requested) = 1 THEN Requested_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN Scheduled_Position ";
        sSQLCustomQuery = sSQLCustomQuery + " Else  CASE WHEN ISDATE(Executed) = 1 THEN Executed_Position ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END AS Position, CASE WHEN ISDATE(Requested) = 1 THEN Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Scheduled) = 1 THEN S_Heading ";
        sSQLCustomQuery = sSQLCustomQuery + " Else CASE WHEN ISDATE(Executed) = 1 THEN E_Heading ELSE NULL END ";
        sSQLCustomQuery = sSQLCustomQuery + " End END As Heading ";

        string query = "", docname = "";
        if (PatientFU_ID == "0")
        {
            printtype = "IE";
            query = "SELECT ProcedureDetail_ID, " + sSQLCustomQuery + ", BodyPart, MCODE, DBO.GETPROCMED(MCODE,SubCode) as Medications FROM tblProceduresDetail WHERE IsConsidered <> 1 AND (PN = 'True') AND PatientFU_ID IS NULL AND (PatientIE_ID = (Select PatientIE_ID from tblInjuredBodyParts Where PatientIE_ID = " + PatientIE_ID + "))";
            //    docname = lname + "," + fname + "_" + PatientIE_ID + "_CF_" + Convert.ToDateTime(doe).ToString("mmddyyyy") + "_" + Convert.ToDateTime(dob).ToString("mmddyyyy");
        }
        else
        {
            printtype = "FU";
            query = "SELECT ProcedureDetail_ID, " + sSQLCustomQuery + ", BodyPart, MCODE, DBO.GETPROCMED(MCODE,SubCode) as Medications FROM tblProceduresDetail WHERE IsConsidered <> 1 AND (PN = 'True') AND PatientFU_ID=" + PatientFU_ID + " AND (PatientIE_ID = (Select PatientIE_ID from tblInjuredBodyParts Where PatientIE_ID = " + PatientIE_ID + "))";
            //  docname = lname + "," + fname + "_" + PatientFU_ID + "_CF_" + Convert.ToDateTime(doe).ToString("mmddyyyy") + "_" + Convert.ToDateTime(dob).ToString("mmddyyyy");
        }

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string docpath = Server.MapPath("~/Template/PN/" + ds.Tables[0].Rows[i]["MCODE"].ToString() + ".html");

                if (File.Exists(docpath))
                {
                    String str = File.ReadAllText(docpath);

                    str = str.Replace("#name", fname + " " + lname);
                    str = str.Replace("#date", CommonConvert.DateFormat(doe));
                    str = str.Replace("#dob", CommonConvert.DateFormat(dob));
                    str = str.Replace("#location", location);


                    docname = lname + ", " + fname + "_" + printtype + "_" + ds.Tables[0].Rows[i]["MCODE"].ToString() + "_PN_" + i + 1 + "_" + CommonConvert.DateFormatPrint(doe) + "_" + CommonConvert.DateFormatPrint(doa) + ".docx";


                    string sMuscle = "", sSide = "", sLevel = "", sMedications = "";

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Muscle"].ToString()))
                    {
                        sMuscle = ds.Tables[0].Rows[i]["Muscle"].ToString().Replace("~", " ");

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Sides"].ToString()))
                        {
                            sSide = ds.Tables[0].Rows[i]["Sides"].ToString();
                            sSide = sSide.ToString().Substring(0, 1);

                        }

                        sMuscle = sMuscle.Replace("_X_", "_" + sSide + "_");
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Level"].ToString()))
                    {
                        sLevel = ds.Tables[0].Rows[i]["Level"].ToString().Replace("~", " ");
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Medications"].ToString()))
                    {
                        sMedications = ds.Tables[0].Rows[i]["Medications"].ToString().Replace("~", " ");
                    }

                    if (!string.IsNullOrEmpty(sSide))
                    {
                        if (sSide == "L")
                            str = str.Replace("(side)", "left");
                        else if (sSide == "R")
                            str = str.Replace("(side)", "right");
                        else if (sSide == "B")
                            str = str.Replace("(side)", "bilaterals");
                    }



                    if (PatientFU_ID == "0")
                        createWordDocument(str, docname, PatientIE_ID);
                    else
                        createWordDocument(str, docname, "", PatientFU_ID);
                }


            }
        }


    }

    protected void DownloadFiles(string folderPath, string fullname, string IEFU)
    {

        if (Directory.Exists(folderPath))
        {

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.AlternateEncodingUsage = Ionic.Zip.ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");



                foreach (string file in Directory.EnumerateFiles(folderPath))
                {
                    string contents = file;
                    zip.AddFile(file, "Files");
                }

                //foreach (GridViewRow row in GridView1.Rows)   
                //{
                //    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                //    {
                //        string filePath = (row.FindControl("lblFilePath") as Label).Text;
                //        zip.AddFile(filePath, "Files");
                //    }
                //}


                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}_{1}_{2}.zip", fullname, IEFU, DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Documents will be available soon.')", true);
        }
    }

    protected void rbllisttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPatientIEDetails();
    }

    protected void lnkDownloadIE_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        string path = Server.MapPath("~/Reports/Done/" + lnk.CommandArgument.Split(',')[0]);
        string fullname = lnk.CommandArgument.Split(',')[2] + '_' + lnk.CommandArgument.Split(',')[1];
        DownloadFiles(path, fullname, "IE");
    }

    protected void lnkDownloadFU_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        string path = Server.MapPath("~/Reports/Done/" + lnk.CommandArgument.Split(',')[0]);
        string fullname = lnk.CommandArgument.Split(',')[2] + '_' + lnk.CommandArgument.Split(',')[1];
        DownloadFiles(path, fullname, "FU");
    }

    protected void btnSaveSign_Click(object sender, EventArgs e)
    {
        byte[] blob = null;
        if (string.IsNullOrEmpty(hidBlobServer.Value) == false)
        {
            try
            {
                string blobstring = hidBlobServer.Value.Split(',')[1];
                blob = Convert.FromBase64String(blobstring);


                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Server.MapPath("~/Sign/"));
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(patientIEIDServer.Value.ToString() + "*.*");
                string fullName = string.Empty;
                foreach (FileInfo foundFile in filesInDir)
                {
                    foundFile.Delete();
                }


                string path = HttpContext.Current.Server.MapPath("~/Sign/");
                string fname = patientIEIDServer.Value.ToString() + "_" + System.DateTime.Now.Millisecond.ToString() + ".jpg";

                string fullpath = path + fname;

                File.WriteAllBytes(fullpath, blob);

                DBHelperClass db = new DBHelperClass();
                string query = "";
                if (patientFUIDServer.Value == "0")
                    query = "delete from tblPatientIESign where PatientIE_ID=" + patientIEIDServer.Value;
                else if (patientIEIDServer.Value == "0")
                    query = "delete from tblPatientIESign where PatinetFU_ID=" + patientFUIDServer.Value;

                db.executeQuery(query);
                query = "insert into tblPatientIESign values(" + patientIEIDServer.Value + ",'" + fullpath + "'," + patientFUIDServer.Value + ",'" + hidBlobServer.Value + "',getdate(),1)";
                db.executeQuery(query);


                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "closeSignModelPopup();", true);

                string name = "";



            }
            catch (Exception ex)
            {
            }

        }
    }

    protected void lnkSignFU_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;
        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData("select * from tblPatientIESign where PatinetFU_ID=" + lnk.CommandArgument);
        bool flag = false;
        string filename = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
            filename = ds.Tables[0].Rows[0]["sign_path"].ToString();
        }

        ClientScript.RegisterStartupScript(this.GetType(), "PopupFU", "openSignModelPopup(0," + lnk.CommandArgument + ",'" + flag + "','" + filename + "');", true);
    }

    protected void lnkSignIE_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;

        string[] str = lnk.CommandArgument.Split(',');

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData("select * from tblPatientIESign where PatientIE_ID=" + str[0]);
        bool flag = false;
        string filename = "";
        string pname = str[2] + " " + str[3];
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
            filename = ds.Tables[0].Rows[0]["sign_path"].ToString();
        }
        bindSignHTML(str[1], pname);
        ClientScript.RegisterStartupScript(this.GetType(), "PopupIE", "openSignModelPopup(" + str[0] + ",0,'" + flag + "','" + filename + "');", true);
    }

    protected void lnkuploadsign_Click(object sender, EventArgs e)
    {
        LinkButton lnk = sender as LinkButton;

        DBHelperClass db = new DBHelperClass();
        DataSet ds = db.selectData("select * from tblPatientIESign where PatientIE_ID=" + lnk.CommandArgument);
        bool flag = false;
        string filename = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            flag = true;
            filename = ds.Tables[0].Rows[0]["sign_path"].ToString();
        }

        ClientScript.RegisterStartupScript(this.GetType(), "PopupIE", "opensignupload(" + lnk.CommandArgument + ",0,'" + flag + "','" + filename + "');", true);
    }

    protected void btnuploadimage_Click(object sender, EventArgs e)
    {
        try
        {

            string path = HttpContext.Current.Server.MapPath("~/Sign/");
            string fname = patientIEIDServer.Value.ToString() + "_" + System.DateTime.Now.Millisecond.ToString() + ".jpg";
            string fullpath = path + "//" + fname;
            //if (File.Exists(fullpath))
            //{
            //    File.Delete(fullpath);
            //}
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Server.MapPath("~/Sign/"));
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(patientIEIDServer.Value.ToString() + "*.*");
            string fullName = string.Empty;
            foreach (FileInfo foundFile in filesInDir)
            {
                foundFile.Delete();
            }

            if (fupuploadsign.HasFile)
            { fupuploadsign.SaveAs(fullpath); }

            string query = "";
            if (patientFUIDServer.Value == "0")
                query = "delete from tblPatientIESign where PatientIE_ID=" + patientIEIDServer.Value;
            else if (patientIEIDServer.Value == "0")
                query = "delete from tblPatientIESign where PatinetFU_ID=" + patientFUIDServer.Value;

            db.executeQuery(query);
            query = "insert into tblPatientIESign values(" + patientIEIDServer.Value + ",'" + fname + "'," + patientFUIDServer.Value + ",'" + hidBlobServer.Value + "',getdate(),0)";
            db.executeQuery(query);

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "closeSignuploadModalPopup();", true);
            string name = "";
        }
        catch (Exception ex)
        {
        }
    }

    private void bindLocation()
    {
        DataSet ds = new DataSet();

        string query = "select Location,Location_ID from tblLocations ";
        if (!string.IsNullOrEmpty(Session["Locations"].ToString()))
        {
            query = query + " where Location_ID in (" + Session["Locations"] + ")";
        }
        query = query + " Order By Location";

        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- All --", "0"));


        }

    }

    protected void lnkPONReport_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton linkButton = sender as LinkButton;
            string[] val = linkButton.CommandArgument.Split(',');
            string filename = val[3] + "," + val[4] + "_" + val[1] + "_PON_Report";
            DataSet ds = db.selectData("select PONPrint from tblFUbpOtherPart where PatientFU_ID=" + val[1]);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string body = File.ReadAllText(Server.MapPath("~/Template/PON.html"));

                body = body.Replace("#content", ds.Tables[0].Rows[0]["PONPrint"].ToString());
                body = body.Replace("#date", val[2].ToString());
                body = body.Replace("#pname", val[4].ToString() + " " + val[3].ToString());

                createWordDocument(body, filename, val[0]);
                downloadfile(filename + ".doc");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "AlertPopup", "javascript:alert('Please Add Report first from OtherPart menu.')", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void downloadfile(string filename)
    {
        string fname = "~/Reports/" + filename;
        if (fname != string.Empty)
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = fname;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            byte[] data = req.DownloadData(Server.MapPath(filePath));
            response.BinaryWrite(data);
            response.End();
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@patientIEID", lnkdel.CommandArgument);

            int val = db.executeSP("nusp_Delete_PatientIE", parameters);
            if (val > 0)
                BindPatientIEDetails();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    protected void lnkDelete_FU_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkdel = sender as LinkButton;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@patientFUID", lnkdel.CommandArgument);

            int val = db.executeSP("nusp_Delete_PatientFU", parameters);
            if (val > 0)
                BindPatientIEDetails();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    private string getBodyParts(Int64 ID, string type)
    {
        List<string> _injured = null;

        if (type == "ie")
            _injured = new BusinessLogic().getInjuredParts(ID).Distinct<string>().ToList<string>();
        else
            _injured = new BusinessLogic().getFUInjuredParts(ID).Distinct<string>().ToList<string>();

        string str = "";

        if (_injured.Contains("Neck"))
            str = str + ", neck";

        if (_injured.Contains("Midback"))
            str = str + ", midback";

        if (_injured.Contains("Lownback"))
            str = str + ", lownback";

        if (_injured.Contains("RightShoulder"))
            str = str + ", right shoulder";

        if (_injured.Contains("LeftShoulder"))
            str = str + ", left shoulder";





        if (_injured.Contains("RightKnee"))
            str = str + ", right knee";

        if (_injured.Contains("LeftKnee"))
            str = str + ", left knee";


        if (_injured.Contains("RightElbowt"))
            str = str + ", right elbow";

        if (_injured.Contains("LeftElbow"))
            str = str + ", left elbow";



        if (_injured.Contains("RightWrist"))
            str = str + ", right wrist";


        if (_injured.Contains("LeftWrist"))
            str = str + ", left wrist";

        if (_injured.Contains("RightHip"))
            str = str + ", right hip";


        if (_injured.Contains("LeftHip"))
            str = str + ", left hip";



        if (_injured.Contains("RightAnkle"))
            str = str + ", right ankle";

        if (_injured.Contains("LeftAnkle"))
            str = str + ", left ankle";



        if (!string.IsNullOrEmpty(str))
        {
            str = str.TrimStart(',');
            str = str.TrimStart();
            str = char.ToUpper(str[0]) + str.Substring(1);
        }
        return " " + str;
    }

    public void bindSignHTML(string type, string pname)
    {
        string path = "";

        if (type.ToLower() == "nf")
            path = Server.MapPath("~/Template/NFSignpade.html");
        else if (type.ToLower() == "wc")
            path = Server.MapPath("~/Template/WCSignpade.html");

        string body = File.ReadAllText(path);

        body = body.Replace("#date", System.DateTime.Now.ToString("MM/dd/yyyy"));
        body = body.Replace("#pname", pname);

        divSignHTML.InnerHtml = body;


    }

    private string getTreatment(string val)
    {
        string strVal = "";
        if (!string.IsNullOrEmpty(val))
        {
            string[] str = val.Split('`');


            for (int i = 0; i < str.Length; i++)
            {
                // dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'));
                // dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", string.IsNullOrEmpty(str[i]) ? str[i] : str[i].TrimStart('@'));

                if (!string.IsNullOrEmpty(str[i]))
                {
                    if (str[i].Substring(0, 1) == "@") { }
                    else
                    {
                        if (string.IsNullOrEmpty(strVal))
                            strVal = str[i].TrimStart('@');
                        else
                            strVal = strVal + "<br/>" + str[i].TrimStart('@');
                    }
                }
            }



        }
        return " " + strVal;

    }

    public string formatString(string str)
    {
        string _str = str.Replace(System.Environment.NewLine, string.Empty);
        _str = System.Text.RegularExpressions.Regex.Replace(_str, @"\s+", " ");
        _str = _str.Replace(" ,", ", ");
        _str = _str.Replace(",", ", ");
        _str = _str.Replace(". ;", ";");
        _str = _str.Replace(" to.", ".");
        _str = _str.Replace(" to to ", " to ");

        _str = _str.Replace(" /", "/");
        _str = _str.Replace(". . .", ".");
        _str = _str.Replace(".  .", ".");
        _str = _str.Replace(". .", ".");


        _str = _str.Replace(", .", ".");
        _str = _str.Replace(",.", ".");
        _str = _str.Replace(",  .", ".");
        _str = _str.Replace("..", ".");

        _str = _str.Replace(" and .", ".");
        _str = _str.Replace(".,", ".");
        _str = _str.Replace(" and.", ".");
        _str = _str.Replace(", and", "and");
        _str = _str.Replace(" and  .", ".");
        _str = _str.Replace(". and", ".");
        _str = _str.Replace(" .", ".");
        _str = _str.Replace(", ,", ", ");


        return _str;
    }

    public string getFUNote(string freeForm)
    {
        string note = "";
        if (!string.IsNullOrEmpty(freeForm))
        {
            string[] freeFormDetails = freeForm.Split('~');
            for (int i = 0; i < freeFormDetails.Length; i++)
            {
                if (freeFormDetails[i].Length > 0)
                {
                    string title = freeFormDetails[i].Split('^')[0].Trim();

                    if (title == "Notes")
                    {
                        note = freeFormDetails[i].Split('^')[1].Trim();
                    }

                }
            }
        }
        return note;
    }
}