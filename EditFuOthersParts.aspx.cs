using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using IntakeSheet;
using System.Configuration;
using System.IO;
using IntakeSheet.BLL;

public partial class EditFuOthersParts : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Other";

    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "OthersParts";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (Session["patientFUId"] == null || Session["patientFUId"] == "")
        {
            Response.Redirect("EditFu.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {
                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpOtherPart WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpOtherPart WHERE PatientIE_ID= " + _CurIEid + "");
                SqlCommand cm1 = new SqlCommand(query1, cn);
                SqlDataAdapter IEda = new SqlDataAdapter(cm1);
                cn.Open();
                DataSet IEds = new DataSet();
                IEda.Fill(IEds);
                cn.Close();
                DataRow FUrw = FUds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("FuCount") == 0);
                DataRow IErw = IEds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("IECount") == 0);


                if (FUrw == null)
                {

                    PopulateUI(_FuId);
                    BindDCDataGrid();
                    // BindDataGrid();

                }
                else if (IErw == null)
                {
                    PopulateIEUI(_CurIEid);
                    BindDCDataGrid();
                    //  BindDataGrid();
                }
                else
                {

                    //_CurIEid = Session["PatientIE_ID"].ToString();
                    //patientID.Value = Session["PatientIE_ID"].ToString();
                    PopulateUIDefaults();
                    BindTreatmentDeafultValues();
                    BindPONDeafultValues();
                    bindTeratMentPrintvalue();
                }
            }
            else
            {
                Response.Redirect("EditFu.aspx");
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuOthersParts for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }

    public string SaveUI(string ieID, string fuID, string fuMode, bool bpIsChecked)
    {
        long _fuID = Convert.ToInt64(fuID);
        string _fuMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpOtherPart WHERE PatientFU_ID = " + _fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && bpIsChecked == true)
            _fuMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpIsChecked == false)
            _fuMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpIsChecked == false)
            _fuMode = "Delete";
        else
            _fuMode = "Update";

        if (_fuMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_fuMode == "Update" || _fuMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_fuMode == "Update" || _fuMode == "New")
        {
            TblRow["PatientFU_ID"] = _fuID;
            TblRow["OthersCC"] = txtOthersCC.Text.ToString();
            TblRow["OthersPE"] = txtOthersPE.Text.ToString();
            TblRow["OthersA"] = txtOthersA.Text.ToString();
            TblRow["OthersP"] = txtOthersP.Text.ToString();
            TblRow["RecommandationDetails"] = Request.Form[txtTreatmentParagraph.UniqueID];
            TblRow["RecommandationDelimit"] = bindTeratMentPrintvalue();
            TblRow["PONDetails"] = Request.Form[txtPONDetails.UniqueID];
            TblRow["PONDelimit"] = bindPONPrintvalue();
            TblRow["PONPrint"] = ViewState["PONPrint"].ToString();
            TblRow["CR"] = txtCR.Text;


            if (_fuMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_fuMode == "Delete")
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

        if (_fuMode == "New")
            return "Other parts has been added...";
        else if (_fuMode == "Update")
            return "Other parts has been updated...";
        else if (_fuMode == "Delete")
            return "Other parts has been deleted...";
        else
            return "";
    }

    public void PopulateUI(string fuID)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpOtherPart WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtOthersCC.Text = TblRow["OthersCC"].ToString().Trim();
            txtOthersPE.Text = TblRow["OthersPE"].ToString().Trim();
            txtOthersA.Text = TblRow["OthersA"].ToString().Trim();
            txtOthersP.Text = TblRow["OthersP"].ToString().Trim();
            txtTreatmentParagraph.Text = TblRow["RecommandationDetails"].ToString().Trim();
            txtPONDetails.Text = TblRow["PONDetails"].ToString().Trim();


            if (!string.IsNullOrEmpty(TblRow["CR"].ToString()))
                txtCR.Text = TblRow["CR"].ToString();
            else
                bindCR();

            BindTreatmentEditValues(TblRow["RecommandationDelimit"].ToString().Trim());
            BindPONEditValues(TblRow["PONDelimit"].ToString().Trim());
            ViewState["PONPrint"] = TblRow["PONPrint"].ToString().Trim();
            _fldPop = false;
        }
        else
        {
            bindCR();
        }

        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }

    public void PopulateIEUI(string ieID)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpOtherPart WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtOthersCC.Text = TblRow["OthersCC"].ToString().Trim();
            txtOthersPE.Text = TblRow["OthersPE"].ToString().Trim();
            txtOthersA.Text = TblRow["OthersA"].ToString().Trim();
            txtOthersP.Text = TblRow["OthersP"].ToString().Trim();
            txtTreatmentParagraph.Text = TblRow["TreatMentDetails"].ToString().Trim();
            BindTreatmentEditValues(TblRow["TreatMentDelimit"].ToString().Trim());

            _fldPop = false;
        }

        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }

    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Others");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            txtOthersCC.Text = node.SelectSingleNode("OthersCC") == null ? txtOthersCC.Text.ToString().Trim() : node.SelectSingleNode("OthersCC").InnerText;
            txtOthersCC.Text = node.SelectSingleNode("OthersPE") == null ? txtOthersPE.Text.ToString().Trim() : node.SelectSingleNode("OthersPE").InnerText;
            txtOthersCC.Text = node.SelectSingleNode("OthersA") == null ? txtOthersA.Text.ToString().Trim() : node.SelectSingleNode("OthersA").InnerText;
            txtOthersCC.Text = node.SelectSingleNode("OthersP") == null ? txtOthersP.Text.ToString().Trim() : node.SelectSingleNode("OthersP").InnerText;
            _fldPop = false;
        }
    }

    public void PopulateStrightFwd(bool bL, bool bR)
    {
        bool bLeft = bL;
        bool bRight = bR;

    }

    public void BindDataGrid()
    {
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        try
        {
            SqlDataAdapter oSQLAdpr;
            DataTable Standards = new DataTable();
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * from tblMacros WHERE PatientIE_ID = " + _CurIEid + " AND BodayParts = '" + _CurBP + "' Order By BodayParts,Heading";
            oSQLCmd.Connection = oSQLConn;
            oSQLCmd.CommandText = SqlStr;
            oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
            oSQLAdpr.Fill(Standards);
            dgvStandards.DataSource = "";
            dgvStandards.DataSource = Standards.DefaultView;
            dgvStandards.DataBind();
            oSQLAdpr.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
    }

    public string SaveStandards(string ieID)
    {
        string ids = string.Empty;
        try
        {
            foreach (GridViewRow row in dgvStandards.Rows)
            {


                string Procedure_ID, MCODE, BodyPart, Heading, CCDesc, PEDesc, ADesc, PDesc;

                Procedure_ID = dgvStandards.DataKeys[row.RowIndex].Value.ToString();
                MCODE = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                BodyPart = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                Heading = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                CCDesc = row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text;
                PEDesc = row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Text;
                ADesc = row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text;
                PDesc = row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text;
                bool CF = row.Cells[8].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                bool PN = row.Cells[9].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                ids += Session["Macro_Master_ID"].ToString() + ",";

                SaveStdUI(ieID, Procedure_ID, true, BodyPart, Heading, CCDesc, PEDesc, ADesc, PDesc, CF, PN);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Standard(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }

    public void SaveStdUI(string ieID, string iStdID, bool StdIsChecked, string bp, string shd, string scc, string spe, string sa, string sp, bool bcf, bool bpn)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _StdID = Convert.ToInt64(iStdID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblMacros WHERE PatientIE_ID = " + ieID + " AND Macro_Master_ID = " + _StdID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && StdIsChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && StdIsChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && StdIsChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

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
            TblRow["Macro_Master_ID"] = _StdID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["BodayParts"] = bp.ToString().Trim();
            TblRow["Heading"] = shd.ToString().Trim();
            TblRow["CCDesc"] = scc.ToString().Trim();
            TblRow["PEDesc"] = spe.ToString().Trim();
            TblRow["ADesc"] = sa.ToString().Trim();
            TblRow["PDesc"] = sp.ToString().Trim();
            TblRow["CF"] = bcf;
            TblRow["PN"] = bpn;

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
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
    }

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        BindDCDataGrid();
    }

    private void AddStd_Click(object sender, EventArgs e) //RoutedEventArgs e
    {

        BindDataGrid();

    }

    public string SaveDiagnosis(string ieID)
    {
        string ids = string.Empty;
        try
        {
            foreach (DataRowView dr in dgvDiagCodes.Rows)
            {
                ids += dr["Diag_Master_ID"].ToString() + ",";
                SaveDiagUI(ieID, dr["Diag_Master_ID"].ToString(), true, dr["BodyPart"].ToString(), dr["Description"].ToString(), dr["DiagCode"].ToString());
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Diagnosis Code(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }

    public void SaveDiagUI(string ieID, string iDiagID, bool DiagIsChecked, string bp, string dcd, string dc)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _DiagID = Convert.ToInt64(iDiagID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && DiagIsChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && DiagIsChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && DiagIsChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

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
            TblRow["Diag_Master_ID"] = _DiagID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["BodyPart"] = bp.ToString().Trim();
            TblRow["DiagCode"] = dc.ToString().Trim();
            TblRow["Description"] = dcd.ToString().Trim();

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
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
    }

    public void BindDCDataGrid()
    {
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        try
        {
            SqlDataAdapter oSQLAdpr;
            DataTable Diagnosis = new DataTable();
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * from tblDiagCodesDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
            oSQLCmd.Connection = oSQLConn;
            oSQLCmd.CommandText = SqlStr;
            oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
            oSQLAdpr.Fill(Diagnosis);
            dgvDiagCodes.DataSource = "";
            dgvDiagCodes.DataSource = Diagnosis.DefaultView;
            dgvDiagCodes.DataBind();
            oSQLAdpr.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
    }

    public void LoadDV_Click(object sender, ImageClickEventArgs e)
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        PopulateUI(Session["patientFUId"].ToString());

        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        bindTeratMentPrintvalue();
    }

    protected void txtTreatment_TextChanged(object sender, EventArgs e)
    {
        bindTeratMentPrintvalue();
    }

    private void BindTreatmentEditValues(string val)
    {
        if (!string.IsNullOrEmpty(val))
        {
            string[] str = val.Split('`');

            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("isChecked", typeof(string)),
                            new DataColumn("name", typeof(string)) });

            for (int i = 0; i < str.Length; i++)
            {
                dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'));
                //dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'));
            }

            repTreatMent.DataSource = dt;
            repTreatMent.DataBind();

            //  bindTeratMentPrintvalue();

        }
        else
        {
            BindTreatmentDeafultValues();
            bindTeratMentPrintvalue();
        }

    }


    private void BindPONEditValues(string val)
    {
        if (!string.IsNullOrEmpty(val))
        {
            string[] str = val.Split('`');

            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("isChecked", typeof(string)),
                            new DataColumn("name", typeof(string)) });

            for (int i = 0; i < str.Length; i++)
            {
                dt.Rows.Add(string.IsNullOrEmpty(str[i]) ? "False" : str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'));
                //dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", str[i].TrimStart('@'));
            }

            repPON.DataSource = dt;
            repPON.DataBind();

            //  bindTeratMentPrintvalue();

        }
        else
        {
            BindPONDeafultValues();
            bindPONPrintvalue();
        }

    }

    private string bindTeratMentPrintvalue()
    {
        string str = "";
        string strDelimit = "";
        for (int i = 0; i < repTreatMent.Items.Count; i++)
        {
            TextBox txt = repTreatMent.Items[i].FindControl("txtTreatment") as TextBox;
            CheckBox chk = repTreatMent.Items[i].FindControl("chk") as CheckBox;

            if (chk.Checked)
            {
                str = str + txt.Text;
                strDelimit = strDelimit + "`" + txt.Text;
            }
            else
            {

                str = !string.IsNullOrEmpty(txt.Text) ? str.Replace(txt.Text, "") : str;
                strDelimit = strDelimit + "`@" + txt.Text;
            }

        }
        strDelimit = strDelimit.TrimStart('`');
        txtTreatmentParagraph.Text = str;

        return strDelimit;

    }

    private void BindTreatmentDeafultValues()
    {
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Default_Treatment.xml"));
        //reading the xml data  
        DataSet ds = new DataSet();
        ds.ReadXml(xmlreader);
        xmlreader.Close();

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            repTreatMent.DataSource = ds.Tables[0];
            repTreatMent.DataBind();


        }
    }

    private string bindPONPrintvalue()
    {
        string str = "", strprint = "";
        string strDelimit = "";
        for (int i = 0; i < repPON.Items.Count; i++)
        {
            TextBox txt = repPON.Items[i].FindControl("txtPON") as TextBox;
            CheckBox chk = repPON.Items[i].FindControl("chkPON") as CheckBox;

            if (chk.Checked)
            {
                str = str + txt.Text;
                strprint = strprint == "" ? (txt.Text) : (strprint + "<br/>" + txt.Text);
                strDelimit = strDelimit + "`" + txt.Text;
            }
            else
            {

                str = !string.IsNullOrEmpty(txt.Text) ? str.Replace(txt.Text, "") : str;
                strDelimit = strDelimit + "`@" + txt.Text;
            }

        }
        strDelimit = strDelimit.TrimStart('`');
        ViewState["PONPrint"] = strprint;
        txtPONDetails.Text = str;

        return strDelimit;

    }

    private void BindPONDeafultValues()
    {
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Default_PON.xml"));
        //reading the xml data  
        DataSet ds = new DataSet();
        ds.ReadXml(xmlreader);
        xmlreader.Close();

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            repPON.DataSource = ds.Tables[0];
            repPON.DataBind();


        }
    }

    protected void chkPON_CheckedChanged(object sender, EventArgs e)
    {
        bindPONPrintvalue();
    }

    protected void txtPON_TextChanged(object sender, EventArgs e)
    {
        bindPONPrintvalue();
    }

    private void bindCR()
    {

        string str = "It is within a certain degree of medical certainty, that the history presented by the patient, the objective physical findings as well as the diagnosis rendered is causally related to the injury the patient incurred on the #bodyparts on #DOA. These current symptoms were nonexistent prior to the accident. Findings were discussed with the patient.";

        if (Session["dtDOA"] != null)
            str = str.Replace("#DOA", CommonConvert.FullDateFormat(Session["dtDOA"].ToString()));
        else
            str = str.Replace("#DOA", "___________");

        string strBodypart = getBodyParts(Convert.ToInt64(Session["patientFUId"].ToString()), "fu");

        str = str.Replace("#bodyparts", strBodypart);

        txtCR.Text = str;
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

}