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

public partial class FuWrist : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Wrist";
    string Position = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();

    ILog log = log4net.LogManager.GetLogger(typeof(FuWrist));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        Session["PageName"] = "Wrist";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (Session["patientFUId"] == null || Session["patientFUId"] == "")
        {
            Response.Redirect("AddFu.aspx");
        }
        if (!IsPostBack)
        {
            BindROM();
            if (Session["PatientIE_ID"] != null)
            {
                if (Session["PatientIE_ID2"] != null && Session["patientFUId"] != null)
                {
                    bindDropdown();
                    _CurIEid = Session["PatientIE_ID2"].ToString();
                    _FuId = Session["patientFUId"].ToString();
                    SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                    DBHelperClass db = new DBHelperClass();
                    string query = ("select count(*) as FuCount FROM tblFUbpWrist WHERE PatientFU_ID = " + _FuId + "");
                    SqlCommand cm = new SqlCommand(query, cn);
                    SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                    cn.Open();
                    DataSet FUds = new DataSet();
                    Fuda.Fill(FUds);
                    cn.Close();
                    string query1 = ("select count(*) as IECount FROM tblbpWrist WHERE PatientIE_ID= " + _CurIEid + "");
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
                        BindDataGrid();

                    }
                    else if (IErw == null)
                    {
                        PopulateIEUI(_CurIEid);
                        BindDCDataGrid();
                        BindDataGrid();
                    }
                    else
                    {

                        //_CurIEid = Session["PatientIE_ID"].ToString();
                        //patientID.Value = Session["PatientIE_ID"].ToString();
                        PopulateUIDefaults();
                        BindDataGrid();
                        //PopulateUI(_CurIEid);
                        //BindDCDataGrid();
                        //BindDataGrid();
                    }
                    if (Position != "")
                    {
                        switch (Position)
                        {
                            case "L":
                                //first div
                                //WrapLeft.Visible = true;
                                //wrpRight.Visible = false;
                                //Second div
                                //wrpPELeft.Visible = true;
                                //wrpPERight.Visible = false;
                                //Left textbox
                                txtFlexionLeft.ReadOnly = false;
                                txtExtensionLeft.ReadOnly = false;
                                txtRadialdeviationLeft.ReadOnly = false;
                                txtUlnarDeviationLeft.ReadOnly = false;
                                //Left textbox
                                txtFlexionRight.ReadOnly = true;
                                txtExtensionRight.ReadOnly = true;
                                txtRadialdeviationRight.ReadOnly = true;
                                txtUlnarDeviationRight.ReadOnly = true;
                                //Right Checkbox
                                //chkTinelRight.Enabled = false;
                                //chkPhalenRight.Enabled = false;
                                //chkPainUponDorsiFlexionRight.Enabled = false;
                                //chkFinkelsteinRight.Enabled = false;
                                //chkPainUponUlnarDeviationRight.Enabled = false;
                                //chkPainUponRadialRight.Enabled = false;
                                //Left checkbox
                                //chkTinelLeft.Enabled = true;
                                //chkPhalenLeft.Enabled = true;
                                //chkFinkelsteinleft.Enabled = true;
                                //chkPainUponUlnarDeviationLeft.Enabled = true;
                                //chkPainUponRadialLeft.Enabled = true;
                                //chkPainUponDorsiFlexionLeft.Enabled = true;
                                ////Bilaterally checkbox
                                //chkTinelBilaterally.Enabled = false;
                                //chkFinkelsteinBilaterally.Enabled = false;
                                //chkPhalenBilaterally.Enabled = false;
                                //chkPainUponDorsiFlexionBilaterally.Enabled = false;
                                //chkPainUponRadialBilaterally.Enabled = false;
                                //chkPainUponUlnarDeviationBilaterally.Enabled = false;
                                break;
                            case "R":
                                //first div
                                //wrpRight.Visible = true;
                                //WrapLeft.Visible = false;
                                //second div
                                //wrpPELeft.Visible = false;
                                //wrpPERight.Visible = true;
                                //Left textbox
                                txtFlexionLeft.ReadOnly = true;
                                txtExtensionLeft.ReadOnly = true;
                                txtRadialdeviationLeft.ReadOnly = true;
                                txtUlnarDeviationLeft.ReadOnly = true;
                                //Left textbox
                                txtFlexionRight.ReadOnly = false;
                                txtExtensionRight.ReadOnly = false;
                                txtRadialdeviationRight.ReadOnly = false;
                                txtUlnarDeviationRight.ReadOnly = false;
                                //Right Checkbox
                                //chkTinelRight.Enabled = true;
                                //chkPhalenRight.Enabled = true;
                                //chkPainUponDorsiFlexionRight.Enabled = true;
                                //chkFinkelsteinRight.Enabled = true;
                                //chkPainUponUlnarDeviationRight.Enabled = true;
                                //chkPainUponRadialRight.Enabled = true;
                                //Left checkbox
                                //chkTinelLeft.Enabled = false;
                                //chkPhalenLeft.Enabled = false;
                                //chkFinkelsteinleft.Enabled = false;
                                //chkPainUponUlnarDeviationLeft.Enabled = false;
                                //chkPainUponRadialLeft.Enabled = false;
                                //chkPainUponDorsiFlexionLeft.Enabled = false;
                                //Bilaterally checkbox
                                //chkTinelBilaterally.Enabled = false;
                                //chkFinkelsteinBilaterally.Enabled = false;
                                //chkPhalenBilaterally.Enabled = false;
                                //chkPainUponDorsiFlexionBilaterally.Enabled = false;
                                //chkPainUponRadialBilaterally.Enabled = false;
                                //chkPainUponUlnarDeviationBilaterally.Enabled = false;
                                break;
                            case "B":
                                //first div
                                //wrpRight.Visible = true;
                                //WrapLeft.Visible = true;
                                //second div
                                //wrpPELeft.Visible = true;
                                //wrpPERight.Visible = true;
                                //Left textbox
                                txtFlexionLeft.ReadOnly = false;
                                txtExtensionLeft.ReadOnly = false;
                                txtRadialdeviationLeft.ReadOnly = false;
                                txtUlnarDeviationLeft.ReadOnly = false;
                                //Left textbox
                                txtFlexionRight.ReadOnly = false;
                                txtExtensionRight.ReadOnly = false;
                                txtRadialdeviationRight.ReadOnly = false;
                                txtUlnarDeviationRight.ReadOnly = false;
                                //Right Checkbox
                                //chkTinelRight.Enabled = true;
                                //chkPhalenRight.Enabled = true;
                                //chkPainUponDorsiFlexionRight.Enabled = true;
                                //chkFinkelsteinRight.Enabled = true;
                                //chkPainUponUlnarDeviationRight.Enabled = true;
                                //chkPainUponRadialRight.Enabled = true;
                                ////Left checkbox
                                //chkTinelLeft.Enabled = true;
                                //chkPhalenLeft.Enabled = true;
                                //chkFinkelsteinleft.Enabled = true;
                                //chkPainUponUlnarDeviationLeft.Enabled = true;
                                //chkPainUponRadialLeft.Enabled = true;
                                //chkPainUponDorsiFlexionLeft.Enabled = true;
                                ////Bilaterally checkbox
                                //chkTinelBilaterally.Enabled = true;
                                //chkFinkelsteinBilaterally.Enabled = true;
                                //chkPhalenBilaterally.Enabled = true;
                                //chkPainUponDorsiFlexionBilaterally.Enabled = true;
                                //chkPainUponRadialBilaterally.Enabled = true;
                                //chkPainUponUlnarDeviationBilaterally.Enabled = true;
                                break;
                        }
                    }
                }
                else
                {
                    Response.Redirect("AddFU.aspx");
                }
                Session["refresh_count"] = 0;
            }
        }

        Logger.Info(Session["uname"].ToString() + "- Visited in  FuWrist for -" + Convert.ToString(Session["LastNameFU"]) + Convert.ToString(Session["FirstNameFU"]) + "-" + DateTime.Now);
    }
    public string SaveUI(string ieID, string fuID, string ieMode, bool bpIsChecked)
    {
        _CurIEid = Session["PatientIE_ID2"].ToString();
        _FuId = Session["patientFUId"].ToString();
        long _fuID = Convert.ToInt64(_FuId);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpWrist WHERE PatientFU_ID = " + _fuID;
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
            TblRow["PatientFU_ID"] = _fuID;
            //TblRow["PainScaleLeft"] = txtPainScaleLeft.Text.Trim();
            //TblRow["ConstantLeft"] = chkContentLeft.Checked;
            //TblRow["IntermittentLeft"] = chkIntermittentLeft.Checked;
            //TblRow["SharpLeft"] = chkSharpLeft.Checked;
            //TblRow["ElectricLeft"] = chkElectricLeft.Checked;
            //TblRow["ShootingLeft"] = chkShootingLeft.Checked;
            //TblRow["ThrobblingLeft"] = chkThrobblingLeft.Checked;
            //TblRow["PulsatingLeft"] = chkPulsatingLeft.Checked;
            //TblRow["DullLeft"] = chkDullLeft.Checked;
            //TblRow["AchyLeft"] = chkAchyLeft.Checked;
            //TblRow["UlnarLeft"] = chkulnarLeft.Checked;
            //TblRow["RadialLeft"] = chkradiusLeft.Checked;
            //TblRow["DorsalLeft"] = chkDorsalLeft.Checked;
            //TblRow["PalmarLeft"] = chkPalmarLeft.Checked;
            //TblRow["LiftingObjectLeft"] = chkLiftingObjectLeft.Checked;
            //TblRow["WorkingLeft"] = chkWorkingLeft.Checked;
            //TblRow["MovementLeft"] = chkMovementLeft.Checked;
            //TblRow["RotationLeft"] = chkRotationLeft.Checked;

            //TblRow["PainScaleRight"] = txtPainScaleRight.Text.Trim();
            //TblRow["ConstantRight"] = chkContentRight.Checked;
            //TblRow["IntermittentRight"] = chkIntermittentRight.Checked;
            //TblRow["SharpRight"] = chkSharpRight.Checked;
            //TblRow["ElectricRight"] = chkElectricRight.Checked;
            //TblRow["ShootingRight"] = chkShootingRight.Checked;
            //TblRow["ThrobblingRight"] = chkThrobblingRight.Checked;
            //TblRow["PulsatingRight"] = chkPulsatingRight.Checked;
            //TblRow["DullRight"] = chkDullRight.Checked;
            //TblRow["AchyRight"] = chkAchyRight.Checked;
            //TblRow["UlnarRight"] = chkulnarRight.Checked;
            //TblRow["RadialRight"] = chkradiusRight.Checked;
            //TblRow["DorsalRight"] = chkDorsalRight.Checked;
            //TblRow["PalmarRight"] = chkPalmarRight.Checked;
            //TblRow["LiftingObjectRight"] = chkLiftingObjectRight.Checked;
            //TblRow["RotationRight"] = chkRotationRight.Checked;
            //TblRow["WorkingRight"] = chkWorkingRight.Checked;
            //TblRow["MovementRight"] = chkMovementRight.Checked;

            TblRow["FlexionROMLeft"] = txtFlexionLeft.Text.Trim();
            TblRow["FlexionROMRight"] = txtFlexionRight.Text.Trim();
            TblRow["ExtensionROMRight"] = txtExtensionRight.Text.Trim();
            TblRow["ExtensionROMLeft"] = txtExtensionLeft.Text.Trim();
            TblRow["RadialDeviationROMLeft"] = txtRadialdeviationLeft.Text.Trim();
            TblRow["RadialDeviationROMRight"] = txtRadialdeviationRight.Text.Trim();
            TblRow["UlnarDeviationROMRight"] = txtUlnarDeviationRight.Text.Trim();
            TblRow["UlnarDeviationROMLeft"] = txtUlnarDeviationLeft.Text.Trim();

            //TblRow["PalpationUlnarLeft"] = chkPalpationUlnarLeft.Checked;
            //TblRow["PalpationUlnarRight"] = chkPalpationUlnarRight.Checked;

            //TblRow["PalpationRadialLeft"] = chkPalpationRadialLeft.Checked;
            //TblRow["PalpationRadialRight"] = chkPalpationRadialRight.Checked;

            //TblRow["PalpationDorsalLeft"] = chkPalpationDorsalLeft.Checked;
            //TblRow["PalpationDorsalRight"] = chkPalpationDorsalRight.Checked;

            //TblRow["PalpationPalmarLeft"] = chkPalpationPalmarLeft.Checked;
            //TblRow["PalpationPalmarRight"] = chkPalpationPalmarRight.Checked;

            //TblRow["TinelLeft"] = chkTinelLeft.Checked;
            //TblRow["TinelRight"] = chkTinelRight.Checked;
            //TblRow["TinelBilaterally"] = chkTinelBilaterally.Checked;

            //TblRow["PhalenLeft"] = chkPhalenLeft.Checked;
            //TblRow["PhalenRight"] = chkPhalenRight.Checked;
            //TblRow["PhalenBilaterally"] = chkPhalenBilaterally.Checked;

            //TblRow["FinkelsteinLeft"] = chkFinkelsteinleft.Checked;
            //TblRow["FinkelsteinRight"] = chkFinkelsteinRight.Checked;
            //TblRow["FinkelsteinBilaterally"] = chkFinkelsteinBilaterally.Checked;

            //TblRow["PainUponUlnarDeviationLeft"] = chkPainUponUlnarDeviationLeft.Checked;
            //TblRow["PainUponUlnarDeviationRight"] = chkPainUponUlnarDeviationRight.Checked;
            //TblRow["PainUponUlnarDeviationBilaterally"] = chkPainUponUlnarDeviationBilaterally.Checked;

            //TblRow["PainUponRadialLeft"] = chkPainUponRadialLeft.Checked;
            //TblRow["PainUponRadialRight"] = chkPainUponRadialRight.Checked;
            //TblRow["PainUponRadialBilaterally"] = chkPainUponRadialBilaterally.Checked;

            //TblRow["PainUponDorsiFlexionLeft"] = chkPainUponDorsiFlexionLeft.Checked;
            //TblRow["PainUponDorsiFlexionRight"] = chkPainUponDorsiFlexionRight.Checked;
            //TblRow["PainUponDorsiFlexionBilaterally"] = chkPainUponDorsiFlexionLeft.Checked;
            //TblRow["RangeOfMotionRight"] = cboRangeOfMotionRight.Text.ToString();
            //TblRow["RangeOfMotionLeft"] = cboRangeOfMotionLeft.Text.ToString();
            // TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            // TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();

            TblRow["CCvalue"] = hdCCvalue.Value;

            TblRow["PEvalue"] = hdPEvalue.Value;



            string strname = "", strleft = "", strright = "", strnormal = "";

            for (int i = 0; i < repROM.Items.Count; i++)
            {
                Label lblname = repROM.Items[i].FindControl("lblname") as Label;
                TextBox txtleft = repROM.Items[i].FindControl("txtleft") as TextBox;
                TextBox txtright = repROM.Items[i].FindControl("txtright") as TextBox;
                TextBox txtnormal = repROM.Items[i].FindControl("txtnormal") as TextBox;

                strname = strname + "," + lblname.Text;
                strleft = strleft + "," + txtleft.Text;
                strright = strright + "," + txtright.Text;
                strnormal = strnormal + "," + txtnormal.Text;
            }

            TblRow["LeftROM"] = strleft.Substring(1);
            TblRow["RightROM"] = strright.Substring(1);
            TblRow["NormalROM"] = strnormal.Substring(1);
            TblRow["NameROM"] = strname.Substring(1);

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
            return "Wrist has been added...";
        else if (_ieMode == "Update")
            return "Wrist has been updated...";
        else if (_ieMode == "Delete")
            return "Wrist has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string fuID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblFUbpWrist WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];



            txtFlexionLeft.Text = TblRow["FlexionROMLeft"].ToString();
            txtFlexionRight.Text = TblRow["FlexionROMRight"].ToString();
            txtExtensionRight.Text = TblRow["ExtensionROMRight"].ToString();
            txtExtensionLeft.Text = TblRow["ExtensionROMLeft"].ToString();
            txtRadialdeviationLeft.Text = TblRow["RadialDeviationROMLeft"].ToString();
            txtRadialdeviationRight.Text = TblRow["RadialDeviationROMRight"].ToString();
            txtUlnarDeviationRight.Text = TblRow["UlnarDeviationROMRight"].ToString();
            txtUlnarDeviationLeft.Text = TblRow["UlnarDeviationROMLeft"].ToString();


         
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

            if (!string.IsNullOrEmpty(TblRow["CCvalue"].ToString()))
            {
                CF.InnerHtml = TblRow["CCvalue"].ToString();

                //hdorgval.Value = sqlTbl.Rows[0]["CCvalueoriginal"].ToString();
            }
            else
            {
                bindCC(Position.ToUpper());
            }
            if (!string.IsNullOrEmpty(TblRow["PEvalue"].ToString()))
            {
                divPE.InnerHtml = TblRow["PEvalue"].ToString();

                //hdorgvalPE.Value = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();
            }
            else
            {
                bindPE(Position.ToUpper());
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP('" + Request.QueryString["P"] + "');", true);

            _fldPop = false;
        }

        sqlTbl1.Dispose();
        sqlCmdBuilder1.Dispose();
        sqlAdapt1.Dispose();
        oSQLConn.Close();


    }

    public void PopulateIEUI(string ieID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblbpWrist WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];



            txtFlexionLeft.Text = TblRow["FlexionROMLeft"].ToString();
            txtFlexionRight.Text = TblRow["FlexionROMRight"].ToString();
            txtExtensionRight.Text = TblRow["ExtensionROMRight"].ToString();
            txtExtensionLeft.Text = TblRow["ExtensionROMLeft"].ToString();
            txtRadialdeviationLeft.Text = TblRow["RadialDeviationROMLeft"].ToString();
            txtRadialdeviationRight.Text = TblRow["RadialDeviationROMRight"].ToString();
            txtUlnarDeviationRight.Text = TblRow["UlnarDeviationROMRight"].ToString();
            txtUlnarDeviationLeft.Text = TblRow["UlnarDeviationROMLeft"].ToString();


            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();



            _fldPop = false;
        }

        sqlTbl1.Dispose();
        sqlCmdBuilder1.Dispose();
        sqlAdapt1.Dispose();
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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Knee");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            txtExtensionLeft.Text = node.SelectSingleNode("LEExtensionRight") == null ? txtExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionRight").InnerText;
            txtFlexionRight.Text = node.SelectSingleNode("LEFlexionRight") == null ? txtFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionRight").InnerText;
            txtExtensionLeft.Text = node.SelectSingleNode("LEExtensionLeft") == null ? txtExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionLeft").InnerText;
            txtFlexionLeft.Text = node.SelectSingleNode("LEFlexionLeft") == null ? txtFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionLeft").InnerText;
            txtExtensionRight.Text = node.SelectSingleNode("LEExtensionRight") == null ? txtExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionRight").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;

            _fldPop = false;
        }

        bindCC(Position.ToUpper());
        bindPE(Position.ToUpper());
    }

    public void BindDataGrid()
    {
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        try
        {
            SqlDataAdapter oSQLAdpr;
            DataTable Standards = new DataTable();
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            //SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' Order By BodyPart,Heading";
            SqlStr = @"Select 
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
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "'  and IsConsidered=0 Order By BodyPart,Heading";
            oSQLCmd.Connection = oSQLConn;
            oSQLCmd.CommandText = SqlStr;
            oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
            oSQLAdpr.Fill(Standards);
            dgvStandards.DataSource = "";
            // dgvStandards.DataSource = Standards.DefaultView;
            dgvStandards.DataBind();
            oSQLAdpr.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
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

                Procedure_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                Heading = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                PDesc = row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text;
                ids += Session["PatientIE_ID2"].ToString() + ",";
                SaveStdUI(ieID, Procedure_ID, Heading, PDesc);
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
    public void SaveStdUI(string ieID, string iStdID, string heading, string pdesc)
    {
        string[] _Type = iStdID.Split('_');
        int _StdID = Convert.ToInt32(_Type[0]);
        string Part = Convert.ToString(_Type[1]);

        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        // long _StdID = Convert.ToInt64(iStdID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + ieID + " AND ProcedureDetail_ID = " + _StdID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        //if (sqlTbl.Rows.Count == 0 && StdChecked == true)
        //    _ieMode = "New";
        //else if (sqlTbl.Rows.Count == 0 && StdChecked == false)
        //    _ieMode = "None";
        //else if (sqlTbl.Rows.Count > 0 && StdChecked == false)
        //    _ieMode = "Delete";
        //else
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
            TblRow["ProcedureDetail_ID"] = _StdID;
            TblRow["PatientIE_ID"] = _ieID;

            if (Part.Equals("R"))
            {
                TblRow["Heading"] = heading.ToString().Trim();
                TblRow["PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("S"))
            {
                TblRow["S_Heading"] = heading.ToString().Trim();
                TblRow["S_PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("E"))
            {
                TblRow["E_Heading"] = heading.ToString().Trim();
                TblRow["E_PDesc"] = pdesc.ToString().Trim();
            }

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
        string ieMode = "New";
        Session["refresh_count"] = Convert.ToInt64(Session["refresh_count"]) + 1;
        _CurIEid = Session["PatientIE_ID2"].ToString();
        _FuId = Session["patientFUId"].ToString();
        bindgridPoup();
        //SaveUI(_CurIEid, _FuId, ieMode, true);
        //SaveStandards(Session["PatientIE_ID2"].ToString());
        //Response.Redirect("AddDiagnosis.aspx");
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
            RemoveDiagCodesDetail(Session["patientFUId"].ToString());
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
    public void SaveDiagUI(string ieID, string iDiagID, bool DiagChecked, string bp, string dcd, string dc)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _DiagID = Convert.ToInt64(iDiagID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID + " AND PatientFu_ID=" + Session["patientFUId"].ToString() + " and BodyPart like '%" + _CurBP + "%' ";
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && DiagChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && DiagChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && DiagChecked == false)
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
            TblRow["PatientFu_ID"] = Session["patientFUId"].ToString();
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

        try
        {
            if (!IsPostBack)
            {

                string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
                string SqlStr = "";
                SqlDataAdapter oSQLAdpr;
                DataTable Diagnosis = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientFU_ID = " + Session["patientFUId"].ToString() + " AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
                oSQLCmd = new SqlCommand(SqlStr, oSQLConn);

                oSQLAdpr = new SqlDataAdapter(oSQLCmd);
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

    private void LoadDV_Click(object sender, ImageClickEventArgs e)
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        _CurIEid = Session["PatientIE_ID2"].ToString();
        _FuId = Session["patientFUId"].ToString();
        SaveDiagnosis(_CurIEid);
        SaveUI(_CurIEid, _FuId, ieMode, true);
        SaveStandards(Session["PatientIE_ID2"].ToString());
        PopulateUI(Session["patientFUId"].ToString());
    }
    public void bindDropdown()
    {
        //XmlDocument doc = new XmlDocument();
        //doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        //foreach (XmlNode node in doc.SelectNodes("//HSM/ROMs/ROM"))
        //{
        //    cboRangeOfMotionLeft.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
        //foreach (XmlNode node in doc.SelectNodes("//HSM/ROMs/ROM"))
        //{
        //    cboRangeOfMotionRight.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
    }

    protected void BindROM()
    {

        try
        {
            _FuId = Session["patientFUId"].ToString();
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;

            if (oSQLConn.State == ConnectionState.Closed)
                oSQLConn.Open();
            SqlStr = "Select * from tblFUbpWrist WHERE PatientFU_ID = " + _FuId;
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            oSQLConn.Close();
            if (sqlTbl.Rows.Count > 0)
            {
                string[] strname, strleft, strright, strnormal;
                if (string.IsNullOrEmpty(sqlTbl.Rows[0]["NameROM"].ToString()) == false)
                {
                    strname = sqlTbl.Rows[0]["NameROM"].ToString().Split(',');
                    strleft = sqlTbl.Rows[0]["LeftROM"].ToString().Split(',');
                    strright = sqlTbl.Rows[0]["RightROM"].ToString().Split(',');
                    strnormal = sqlTbl.Rows[0]["NormalROM"].ToString().Split(',');


                    // Create the Table
                    DataTable OrdersTable = new DataTable("ROM");
                    // Build the Orders schema
                    OrdersTable.Columns.Add("name", Type.GetType("System.String"));
                    OrdersTable.Columns.Add("left", Type.GetType("System.String"));
                    OrdersTable.Columns.Add("right", Type.GetType("System.String"));
                    OrdersTable.Columns.Add("normal", Type.GetType("System.String"));

                    DataRow workRow;

                    for (int i = 0; i < strname.Length; i++)
                    {

                        workRow = OrdersTable.NewRow();
                        workRow[0] = strname[i];
                        workRow[1] = strleft[i];
                        workRow[2] = strright[i];
                        workRow[3] = strnormal[i];
                        OrdersTable.Rows.Add(workRow);
                    }

                    if (OrdersTable.Rows.Count != 0)
                    {
                        repROM.DataSource = OrdersTable;
                        repROM.DataBind();
                    }
                }
                else
                    getXMLROMvalue();
            }
            else
            {
                getXMLROMvalue();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void getXMLROMvalue()
    {
        //open the tender xml file  
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Wrist.xml"));
        //reading the xml data  
        DataSet ds = new DataSet();
        ds.ReadXml(xmlreader);
        xmlreader.Close();
        //if ds is not empty  
        if (ds.Tables.Count != 0)
        {
            repROM.DataSource = ds;
            repROM.DataBind();
        }
    }

    protected void repROM_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (Request["P"] != null)
            {
                if (Request["P"] == "R")
                {
                    TextBox txtleft = e.Item.FindControl("txtleft") as TextBox;
                    txtleft.ReadOnly = true;
                }
                else if (Request["P"] == "L")
                {
                    TextBox txtright = e.Item.FindControl("txtright") as TextBox;
                    txtright.ReadOnly = true;
                }
            }
        }
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
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "TestFU", "closeModelPopup()", true);
    }

    protected void RemoveDiagCodesDetail(string PatientFU_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblDiagCodesDetail WHERE PatientFU_ID=" + PatientFU_ID + " and BodyPart like '%" + _CurBP + "%'";
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
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


    public void bindCC(string p)
    {


        int PatientIE_Id = 0, PatientFU_Id = 0;
        string body = "";
        DefaultCCPEModel model = new DefaultCCPEModel();


        if (Session["UserId"].ToString() == "10")
        {
            string query = "SELECT  top 1 LAG(t.PatientFU_ID) OVER (ORDER BY t.PatientFU_ID) as PreviousValue from tblFUbpWrist t WHERE PatientFU_ID in (select PatientFU_ID from tblFUPatient where PatientIE_ID=" + Session["PatientIE_ID"].ToString() + ")  order by PatientFU_ID desc";

            DataSet ds = gDbhelperobj.selectData(query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PreviousValue"].ToString()))
                {
                    PatientIE_Id = gDbhelperobj.geIEfromFUID(Session["patientFUId"].ToString());

                    query = "select CCvalue,PEvalue from tblbpWrist WHERE PatientIE_ID =" + PatientIE_Id;
                    var result = gDbhelperobj.getCarryForwardValues(query);

                    if (result != null)
                    {
                        body = result.CC;
                    }
                }
                else
                {

                    query = "select CCvalue,PEvalue from tblFUbpWrist WHERE PatientFU_ID =" + ds.Tables[0].Rows[0]["PreviousValue"].ToString();
                    var result = gDbhelperobj.getCarryForwardValues(query);

                    if (result != null)
                    {
                        body = result.CC;
                    }
                }
            }
        }
        else
        {
            string path = Server.MapPath("~/Template/WristCC.html");
            body = File.ReadAllText(path);


            model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Left");
            body = body.Replace("#LCC", model.CC);
            model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Right");
            body = body.Replace("#RCC", model.CC);

        }


        if (p == "left")
        {
            body = body.Replace("#rigthtdiv", "style='display:none'");

        }
        else if (p == "right")
        {
            body = body.Replace("#leftdiv", "style='display:none'");

        }


        CF.InnerHtml = body;

    }

    public void bindPE(string p)
    {
        int PatientIE_Id = 0, PatientFU_Id = 0;
        string body = "";
        DefaultCCPEModel model = new DefaultCCPEModel();



        if (Session["UserId"].ToString() == "10")
        {


            string query = "SELECT  top 1 LAG(t.PatientFU_ID) OVER (ORDER BY t.PatientFU_ID) as PreviousValue from tblFUbpWrist t WHERE PatientFU_ID in (select PatientFU_ID from tblFUPatient where PatientIE_ID=" + Session["PatientIE_ID"].ToString() + ")  order by PatientFU_ID desc";

            DataSet ds = gDbhelperobj.selectData(query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PreviousValue"].ToString()))
                {
                    PatientIE_Id = gDbhelperobj.geIEfromFUID(Session["patientFUId"].ToString());

                    query = "select CCvalue,PEvalue from tblbpWrist WHERE PatientIE_ID =" + PatientIE_Id;
                    var result = gDbhelperobj.getCarryForwardValues(query);

                    if (result != null)
                    {
                        body = result.PE;
                    }
                }
                else
                {
                    query = "select CCvalue,PEvalue from tblFUbpWrist WHERE PatientFU_ID =" + ds.Tables[0].Rows[0]["PreviousValue"].ToString();
                    var result = gDbhelperobj.getCarryForwardValues(query);

                    if (result != null)
                    {
                        body = result.PE;
                    }
                }
            }
        }
        else
        {
            string path = Server.MapPath("~/Template/WristPE.html");
            body = File.ReadAllText(path);
            model = new DefaultCCPEModel();


            model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Right");
            body = body.Replace("#RPE", model.PE);
            model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Left");
            body = body.Replace("#LPE", model.PE);

        }
        if (p == "left")
        {
            body = body.Replace("#rigthtdiv", "style='display:none'");

        }
        else if (p == "right")
        {
            body = body.Replace("#leftdiv", "style='display:none'");

        }

        divPE.InnerHtml = body;

    }
}