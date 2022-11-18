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
using log4net;
using IntakeSheet.BLL;

public partial class OthersParts : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Other";
    ILog log = log4net.LogManager.GetLogger(typeof(OthersParts));

    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "OthersParts";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {

            if (Session["PatientIE_ID"] != null)
            {
                bindRecording();
                hidIE.Value = Session["PatientIE_ID"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblbpOtherPart WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                DataRow rw = ds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                BindTreatmentDeafultValues();
                if (rw != null)
                {
                    // row exists
                    PopulateUIDefaults();
                    bindCR();

                }
                else
                {

                    _CurIEid = Session["PatientIE_ID"].ToString();


                    PopulateUI(_CurIEid);
                    BindDCDataGrid();
                    BindDataGrid();

                }
                bindgridPoup();
            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  OtherParts for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    public string SaveUI(string ieID, string ieMode, bool bpIsChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";
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

        if (sqlTbl.Rows.Count == 0 && bpIsChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpIsChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpIsChecked == false)
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
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["OthersCC"] = txtOthersCC.Text.ToString();
            TblRow["OthersPE"] = txtOthersPE.Text.ToString();
            TblRow["OthersA"] = txtOthersA.Text.ToString();
            TblRow["OthersP"] = txtOthersP.Text.ToString();
            TblRow["TreatMentDetails"] = Request.Form[txtTreatmentParagraph.UniqueID];
            TblRow["TreatMentDelimit"] = bindTeratMentPrintvalue();
            TblRow["CR"] = txtCR.Text;
            TblRow["IMPRating"] = txtIMPRating.Text;


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
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        if (_ieMode == "New")
            return "Hip has been added...";
        else if (_ieMode == "Update")
            return "Hip has been updated...";
        else if (_ieMode == "Delete")
            return "Hip has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string ieID)
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
            txtCR.Text = TblRow["CR"].ToString().Trim();
            txtIMPRating.Text = TblRow["IMPRating"].ToString().Trim();
            txtTreatmentParagraph.Text = !string.IsNullOrEmpty(TblRow["TreatMentDetails"].ToString().Trim()) ? TblRow["TreatMentDetails"].ToString().Trim() : "";
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
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
        foreach (XmlNode node in nodeList)
        {
            bool isTreatment = node.SelectSingleNode("displayTreatment") != null ? Convert.ToBoolean(node.SelectSingleNode("displayTreatment").InnerText) : true;

            if (isTreatment)
                divTreatment.Attributes.Add("style", "display:block");
            else
                divTreatment.Attributes.Add("style", "display:none");
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
        //  BindDCDataGrid();
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
            RemoveDiagCodesDetail(ieID);
            foreach (GridViewRow row in dgvDiagCodes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string Description, DiagCode, DiagCode_ID;

                    DiagCode_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                    //        DiagCodeDetail_ID = row.Cells[2].Controls.OfType<HiddenField>().FirstOrDefault().Value;

                    Description = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    DiagCode = row.Cells[0].Controls.OfType<TextBox>().FirstOrDefault().Text;

                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        //ids += DiagCode_ID + ",";
                        SaveDiagUI(ieID, DiagCode_ID, true, _CurBP, Description, DiagCode);
                    }
                }
            }
            BindDCDataGrid();
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
        _CurIEid = Session["PatientIE_ID"].ToString();
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        try
        {
            if (!IsPostBack)
            {
                SqlDataAdapter oSQLAdpr;
                DataTable Diagnosis = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientIE_ID = " + _CurIEid + " and PatientFU_ID is null AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
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
            else
            {
                if (ViewState["DiagnosisList"] != null)
                {
                    List<Adddiagnosis> objList = (List<Adddiagnosis>)ViewState["DiagnosisList"];

                    dgvDiagCodes.DataSource = objList;
                    dgvDiagCodes.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void LoadDV_Click(object sender, ImageClickEventArgs e)
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        SaveDiagnosis(Session["PatientIE_ID"].ToString());
        SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);

        PopulateUI(Session["PatientIE_ID"].ToString());
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
            //DataView dv = ds.Tables[0].DefaultView;
            //dv.RowFilter = "name like '%shoulder%'";
            repTreatMent.DataSource = ds;
            repTreatMent.DataBind();

            bindTeratMentPrintvalue();


        }
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
                // dt.Rows.Add(str[i].Substring(0, 1) == "@" ? "False" : "True", string.IsNullOrEmpty(str[i]) ? str[i] : str[i].TrimStart('@'));
            }

            repTreatMent.DataSource = dt;
            repTreatMent.DataBind();

            //  bindTeratMentPrintvalue();

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

    private void bindgridPoup()
    {
        try
        {
            string _CurBodyPart = _CurBP;
            string _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";
            if (_CurIEid != "")
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS(" + _CurIEid + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            else
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            ds = gDbhelperobj.selectData(SqlStr);

            dgvDiagCodesPopup.DataSource = ds;
            dgvDiagCodesPopup.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    protected void btnDaigSave_Click(object sender, EventArgs e)
    {
        SaveStandardsPopup(Session["PatientIE_ID"].ToString());
        BindDCDataGrid();
        txDesc.Text = string.Empty;
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Test", "closeModelPopup()", true);
    }

    public string SaveStandardsPopup(string ieID)
    {
        List<Adddiagnosis> objList = new List<Adddiagnosis>();
        Adddiagnosis obj = new Adddiagnosis();
        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvDiagCodesPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    obj = new Adddiagnosis();
                    obj.Diag_Master_ID = dgvDiagCodesPopup.DataKeys[row.RowIndex].Value.ToString();
                    obj.BodyPart = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.DiagCode = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Description = row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.PN = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        ids += obj.DiagCode_ID + ",";
                        //  SaveStdUI(ieID, obj.DiagCode_ID, true, obj.BodyPart, obj.Description, obj.DiagCode);
                        objList.Add(obj);
                    }
                    //else
                    //{ SaveStdUI(ieID, obj.DiagCode_ID, false, obj.BodyPart, obj.Description, obj.DiagCode); }

                }
            }
            ViewState["DiagnosisList"] = objList;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    protected void RemoveDiagCodesDetail(string PatientIE_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblDiagCodesDetail WHERE PatientIE_ID=" + PatientIE_ID + " and BodyPart like '%" + _CurBP + "%'";
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }

    private void bindRecording()
    {
        try
        {
            DataSet ds = gDbhelperobj.selectData("select * from tblbpOtherPartRecording where PatientIE_ID=" + Session["PatientIE_ID"]);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersCC"].ToString()))
                {
                    ccAudio.Src = "~/RecordingFiles/OtherParts/CC/" + ds.Tables[0].Rows[0]["OthersCC"].ToString();
                    ccAudio.Attributes.Add("style", "display:block");
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersPE"].ToString()))
                {
                    peAudio.Src = "~/RecordingFiles/OtherParts/PE/" + ds.Tables[0].Rows[0]["OthersPE"].ToString();
                    peAudio.Attributes.Add("style", "display:block");
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersA"].ToString()))
                {
                    aAudio.Src = "~/RecordingFiles/OtherParts/ASSESMENT/" + ds.Tables[0].Rows[0]["OthersA"].ToString();
                    aAudio.Attributes.Add("style", "display:block");
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OthersP"].ToString()))
                {
                    pAudio.Src = "~/RecordingFiles/OtherParts/PLAN/" + ds.Tables[0].Rows[0]["OthersP"].ToString();
                    pAudio.Attributes.Add("style", "display:block");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void bindCR()
    {

        //string str = "It is within a certain degree of medical certainty, that the history presented by the patient, the objective physical findings as well as the diagnosis rendered is causally related to the injury the patient incurred on the #bodyparts on #DOA. These current symptoms were nonexistent prior to the accident. Findings were discussed with the patient.";
        string str = "It is within a certain degree of medical certainty, that the history presented by the patient, the objective physical findings as well as the diagnoses rendered are causally related to the injuries the patient incurred on the #bodyparts on #DOA. These current symptoms were nonexistent prior to the accident. Findings were discussed with the patient.";

        if (Session["dtDOA"] != null)
            str = str.Replace("#DOA", CommonConvert.FullDateFormat(Session["dtDOA"].ToString()));
        else
            str = str.Replace("#DOA", "___________");

        string strBodypart = getBodyParts(Convert.ToInt64(Session["PatientIE_ID"].ToString()), "ie");

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