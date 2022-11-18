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

public partial class FuElbow : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Elbow";
    string Position = "";

    ILog log = log4net.LogManager.GetLogger(typeof(FuElbow));


    DBHelperClass gDbhelperobj = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        Session["PageName"] = "Elbow";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (Session["patientFUId"] == null || Session["patientFUId"] == "")
        {
            Response.Redirect("AddFu.aspx");
        }
        if (!IsPostBack)
        {
            BindROM();
            bindDropdown();
            if (Session["PatientIE_ID2"] != null && Session["patientFUId"] != null)
            {

                _CurIEid = Session["PatientIE_ID2"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpElbow WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpElbow WHERE PatientIE_ID= " + _CurIEid + "");
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
                            //second div
                            //wrpLeft2.Visible = true;
                            //wrpRight2.Visible = false;
                            //Left textbox
                            txtExtension1.ReadOnly = false;
                            txtFlex1.ReadOnly = false;
                            txtSupination1.ReadOnly = false;
                            txtPronation1.ReadOnly = false;
                            //Left textbox
                            txtExtension2.ReadOnly = true;
                            txtFlex2.ReadOnly = true;
                            txtSupination2.ReadOnly = true;
                            txtPronation2.ReadOnly = true;
                            break;
                        case "R":
                            //first div
                            //wrpRight.Visible = true;
                            //WrapLeft.Visible = false;
                            //second div
                            //wrpLeft2.Visible = false;
                            //wrpRight2.Visible = true;
                            //Left textbox
                            txtExtension1.ReadOnly = true;
                            txtFlex1.ReadOnly = true;
                            txtSupination1.ReadOnly = true;
                            txtPronation1.ReadOnly = true;
                            //Left textbox
                            txtExtension2.ReadOnly = false;
                            txtFlex2.ReadOnly = false;
                            txtSupination2.ReadOnly = false;
                            txtPronation2.ReadOnly = false;
                            break;
                        case "B":
                            //first div
                            //wrpRight.Visible = true;
                            //WrapLeft.Visible = true;
                            //second div
                            //wrpLeft2.Visible = true;
                            //wrpRight2.Visible = true;
                            //Left textbox
                            txtExtension1.ReadOnly = false;
                            txtFlex1.ReadOnly = false;
                            txtSupination1.ReadOnly = false;
                            txtPronation1.ReadOnly = false;
                            //Left textbox
                            txtExtension2.ReadOnly = false;
                            txtFlex2.ReadOnly = false;
                            txtSupination2.ReadOnly = false;
                            txtPronation2.ReadOnly = false;
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

        Logger.Info(Session["uname"].ToString() + "- Visited in  FuElbow for -" + Convert.ToString(Session["LastNameFU"]) + Convert.ToString(Session["FirstNameFU"]) + "-" + DateTime.Now);


    }

    public string SaveUI(string ieID, string fuid, string ieMode, bool bpIsChecked)
    {
        _CurIEid = Session["PatientIE_ID2"].ToString();
        _FuId = Session["patientFUId"].ToString();
        long _fuID = Convert.ToInt64(_FuId);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpElbow WHERE PatientFU_ID = " + _FuId;
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

            //TblRow["ConstantRight"] = chkContentRight.Checked;
            //TblRow["IntermittentRight"] = chkIntermittentRight.Checked;
            //TblRow["SharpRight"] = chksharpRight.Checked;
            //TblRow["ElectricRight"] = chkelectricRight.Checked;
            //TblRow["ShootingRight"] = chkshootingRight.Checked;
            //TblRow["ThrobblingRight"] = chkthrobbingRight.Checked;
            //TblRow["PulsatingRight"] = chkpulsatingRight.Checked;
            //TblRow["DullRight"] = chkdullRight.Checked;
            //TblRow["AchyRight"] = chkachyinnatureRight.Checked;
            //TblRow["ConstantLeft"] = chkContentLeft.Checked;
            //TblRow["IntermittentLeft"] = chkIntermittentLeft.Checked;
            //TblRow["SharpLeft"] = chksharpLeft.Checked;
            //TblRow["ElectricLeft"] = chkelectricLeft.Checked;
            //TblRow["ShootingLeft"] = chkshootingLeft.Checked;
            //TblRow["ThrobblingLeft"] = chkthrobbingLeft.Checked;
            //TblRow["PulsatingLeft"] = chkpulsatingLeft.Checked;
            //TblRow["DullLeft"] = chkdullLeft.Checked;
            //TblRow["AchyLeft"] = chkachyinnatureLeft.Checked;
            //TblRow["PainScaleLeft"] = txtPainScaleLeft.Text.Trim();
            //TblRow["PainScaleRight"] = txtPainScaleRight.Text.Trim();
            //TblRow["MovementRight"] = chkmovementRight.Checked;
            //TblRow["RaisingArmRight"] = chkraisingthearmRight.Checked;
            //TblRow["LiftingObjectRight"] = chkliftingobjectsRight.Checked;
            //TblRow["RotationRight"] = chkrotationRight.Checked;
            //TblRow["WorkingRight"] = chkworkingRight.Checked;
            //TblRow["NoteRight"] = txtNoteRight.Text.Trim();
            //TblRow["MovementLeft"] = chkmovementLeft.Checked;
            //TblRow["RaisingArmLeft"] = chkraisingthearmLeft.Checked;
            //TblRow["LiftingObjectLeft"] = chkliftingobjectsLeft.Checked;
            //TblRow["RotationLeft"] = chkrotationLeft.Checked;
            //TblRow["WorkingLeft"] = chkworkingLeft.Checked;
            //TblRow["NoteLeft"] = txtNoteLeft.Text.Trim();
            TblRow["ExtensionROM1"] = txtExtension1.Text.Trim();
            TblRow["ExtensionROM2"] = txtExtension2.Text.Trim();
            TblRow["FlexionROM1"] = txtFlex1.Text.Trim();
            TblRow["FlexionROM2"] = txtFlex2.Text.Trim();
            TblRow["SupinationROM1"] = txtSupination1.Text.Trim();
            TblRow["SupinationROM2"] = txtSupination2.Text.Trim();
            TblRow["PronationROM1"] = txtPronation1.Text.Trim();
            TblRow["PronationROM2"] = txtPronation2.Text.Trim();


            //TblRow["MedEpicondyleLeft"] = chkMedEpicondyleLeft.Checked;
            //TblRow["LatEpicondyleLeft"] = chkLatEpicondyleLeft.Checked;
            //TblRow["OlecranonLeft"] = chkOlecranonLeft.Checked;
            //TblRow["MedEpicondyleRight"] = chkMedEpicondyleRight.Checked;
            //TblRow["LatEpicondyleRight"] = chkLatEpicondyRight.Checked;
            //TblRow["OlecranonRight"] = chkOlecranonRight.Checked;
            //TblRow["RangeOfMotionLeft"] = cboRangeOfMotionLeft.Text.ToString();
            //TblRow["TinelLeft"] = cboTinelLeft.Text.ToString();
            //TblRow["RangeOfMotionRight"] = cboRangeOfMotionRight.Text.ToString();
            //TblRow["TinelRight"] = cboTinelRight.Text.ToString();
            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
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

        if (_ieMode == "New")
            return "Elbow has been added...";
        else if (_ieMode == "Update")
            return "Elbow has been updated...";
        else if (_ieMode == "Delete")
            return "Elbow has been deleted...";
        else
            return "";
    }

    public void PopulateUI(string fuID)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpElbow WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];

          
            txtExtension1.Text = TblRow["ExtensionROM1"].ToString();
            txtExtension2.Text = TblRow["ExtensionROM2"].ToString();
            txtFlex1.Text = TblRow["FlexionROM1"].ToString();
            txtFlex2.Text = TblRow["FlexionROM2"].ToString();
            txtSupination1.Text = TblRow["SupinationROM1"].ToString();
            txtSupination2.Text = TblRow["SupinationROM2"].ToString();
            txtPronation1.Text = TblRow["PronationROM1"].ToString();
            txtPronation2.Text = TblRow["PronationROM2"].ToString();
          
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();

            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();


            if (!string.IsNullOrEmpty(sqlTbl.Rows[0]["CCvalue"].ToString()))
            {
                CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();

                //hdorgval.Value = sqlTbl.Rows[0]["CCvalueoriginal"].ToString();
            }
            else
            {
                bindCC(Position.ToUpper());
            }
            if (!string.IsNullOrEmpty(sqlTbl.Rows[0]["PEvalue"].ToString()))
            {
                divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();

                //hdorgvalPE.Value = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();
            }
            else
            {
                bindPE(Position.ToUpper());
            }


            _fldPop = false;
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
        SqlStr = "Select * from tblbpElbow WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];



            //chkContentRight.Checked = CommonConvert.ToBoolean(TblRow["ConstantRight"].ToString());
            //chkIntermittentRight.Checked = CommonConvert.ToBoolean(TblRow["IntermittentRight"].ToString());
            //chksharpRight.Checked = CommonConvert.ToBoolean(TblRow["SharpRight"].ToString());
            //chkelectricRight.Checked = CommonConvert.ToBoolean(TblRow["ElectricRight"].ToString());
            //chkshootingRight.Checked = CommonConvert.ToBoolean(TblRow["ShootingRight"].ToString());
            //chkthrobbingRight.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingRight"].ToString());
            //chkpulsatingRight.Checked = CommonConvert.ToBoolean(TblRow["PulsatingRight"].ToString());
            //chkdullRight.Checked = CommonConvert.ToBoolean(TblRow["DullRight"].ToString());
            //chkachyinnatureRight.Checked = CommonConvert.ToBoolean(TblRow["AchyRight"].ToString());
            //chkContentLeft.Checked = CommonConvert.ToBoolean(TblRow["ConstantLeft"].ToString());
            //chkIntermittentLeft.Checked = CommonConvert.ToBoolean(TblRow["IntermittentLeft"].ToString());
            //chksharpLeft.Checked = CommonConvert.ToBoolean(TblRow["SharpLeft"].ToString());
            //chkelectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            //chkshootingLeft.Checked = CommonConvert.ToBoolean(TblRow["ShootingLeft"].ToString());
            //chkthrobbingLeft.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingLeft"].ToString());
            //chkpulsatingLeft.Checked = CommonConvert.ToBoolean(TblRow["PulsatingLeft"].ToString());
            //chkdullLeft.Checked = CommonConvert.ToBoolean(TblRow["DullLeft"].ToString());
            //chkachyinnatureLeft.Checked = CommonConvert.ToBoolean(TblRow["AchyLeft"].ToString());
            //txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString();
            //txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString();
            //chkmovementRight.Checked = CommonConvert.ToBoolean(TblRow["MovementRight"].ToString());
            //chkraisingthearmRight.Checked = CommonConvert.ToBoolean(TblRow["RaisingArmRight"].ToString());
            //chkliftingobjectsRight.Checked = CommonConvert.ToBoolean(TblRow["LiftingObjectRight"].ToString());
            //chkrotationRight.Checked = CommonConvert.ToBoolean(TblRow["RotationRight"].ToString());
            //chkworkingRight.Checked = CommonConvert.ToBoolean(TblRow["WorkingRight"].ToString());
            //txtNoteRight.Text = TblRow["NoteRight"].ToString();
            //chkmovementLeft.Checked = CommonConvert.ToBoolean(TblRow["MovementLeft"].ToString());
            //chkraisingthearmLeft.Checked = CommonConvert.ToBoolean(TblRow["RaisingArmLeft"].ToString());
            //chkliftingobjectsLeft.Checked = CommonConvert.ToBoolean(TblRow["LiftingObjectLeft"].ToString());
            //chkrotationLeft.Checked = CommonConvert.ToBoolean(TblRow["RotationLeft"].ToString());
            //chkworkingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorkingLeft"].ToString());
            //txtNoteLeft.Text = TblRow["NoteLeft"].ToString();
            txtExtension1.Text = TblRow["ExtensionROM1"].ToString();
            txtExtension2.Text = TblRow["ExtensionROM2"].ToString();
            txtFlex1.Text = TblRow["FlexionROM1"].ToString();
            txtFlex2.Text = TblRow["FlexionROM2"].ToString();
            txtSupination1.Text = TblRow["SupinationROM1"].ToString();
            txtSupination2.Text = TblRow["SupinationROM2"].ToString();
            txtPronation1.Text = TblRow["PronationROM1"].ToString();
            txtPronation2.Text = TblRow["PronationROM2"].ToString();
            //chkMedEpicondyleLeft.Checked = CommonConvert.ToBoolean(TblRow["MedEpicondyleLeft"].ToString());
            //chkLatEpicondyleLeft.Checked = CommonConvert.ToBoolean(TblRow["LatEpicondyleLeft"].ToString());
            //chkOlecranonLeft.Checked = CommonConvert.ToBoolean(TblRow["OlecranonLeft"].ToString());
            //chkMedEpicondyleRight.Checked = CommonConvert.ToBoolean(TblRow["MedEpicondyleRight"].ToString());
            //chkLatEpicondyRight.Checked = CommonConvert.ToBoolean(TblRow["LatEpicondyleRight"].ToString());
            //chkOlecranonRight.Checked = CommonConvert.ToBoolean(TblRow["OlecranonRight"].ToString());
            //cboRangeOfMotionLeft.Text = TblRow["RangeOfMotionLeft"].ToString().Trim();
            //cboTinelLeft.Text = TblRow["TinelLeft"].ToString().Trim();
            //cboRangeOfMotionRight.Text = TblRow["RangeOfMotionRight"].ToString().Trim();
            //cboTinelRight.Text = TblRow["TinelRight"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            if (Session["refresh_count"] != null)
                if (Session["refresh_count"] != "0")
                {
                    txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
                    txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
                    txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
                    txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
                }
                else
                {
                    txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
                }

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Elbow");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            //txtPainScaleLeft.Text = node.SelectSingleNode("PainScaleLeft") == null ? txtPainScaleLeft.Text.ToString().Trim() : node.SelectSingleNode("PainScaleLeft").InnerText;
            //txtPainScaleRight.Text = node.SelectSingleNode("PainScaleRight") == null ? txtPainScaleRight.Text.ToString().Trim() : node.SelectSingleNode("PainScaleRight").InnerText;

            txtExtension1.Text = node.SelectSingleNode("ExtensionLeft") == null ? txtExtension1.Text.ToString().Trim() : node.SelectSingleNode("ExtensionLeft").InnerText;
            txtExtension2.Text = node.SelectSingleNode("ExtensionRight") == null ? txtExtension2.Text.ToString().Trim() : node.SelectSingleNode("ExtensionRight").InnerText;
            txtFlex1.Text = node.SelectSingleNode("FlexLeft") == null ? txtFlex1.Text.ToString().Trim() : node.SelectSingleNode("FlexLeft").InnerText;
            txtFlex2.Text = node.SelectSingleNode("FlexRight") == null ? txtFlex2.Text.ToString().Trim() : node.SelectSingleNode("FlexRight").InnerText;
            txtSupination1.Text = node.SelectSingleNode("SupinationLeft") == null ? txtSupination1.Text.ToString().Trim() : node.SelectSingleNode("SupinationLeft").InnerText;
            txtSupination2.Text = node.SelectSingleNode("SupinationRight") == null ? txtSupination2.Text.ToString().Trim() : node.SelectSingleNode("SupinationRight").InnerText;
            txtPronation1.Text = node.SelectSingleNode("PronationLeft") == null ? txtPronation1.Text.ToString().Trim() : node.SelectSingleNode("PronationLeft").InnerText;
            txtPronation2.Text = node.SelectSingleNode("PronationRight") == null ? txtPronation2.Text.ToString().Trim() : node.SelectSingleNode("PronationRight").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            // txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;

            _fldPop = false;
        }

        bindCC(Position.ToUpper());
        bindPE(Position.ToUpper());
    }

    public void PopulateStrightFwd(bool bL, bool bR)
    {
        bool bLeft = bL;
        bool bRight = bR;

        //wrpLeft1.IsEnabled =
        //wrpLeft2.IsEnabled = bLeft;

        //wrpRight1.IsEnabled =
        //wrpRight2.IsEnabled = bRight;

        //if (bLeft && bRight)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex = cboLatEpiconSide.SelectedIndex =
        //    cbocontusionSide.SelectedIndex = cbofractureSide.SelectedIndex = cboMedEpiconSide.SelectedIndex = 3;
        //else if (bLeft)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex = cboLatEpiconSide.SelectedIndex =
        //    cbocontusionSide.SelectedIndex = cbofractureSide.SelectedIndex = cboMedEpiconSide.SelectedIndex = 1;
        //else
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex = cboLatEpiconSide.SelectedIndex =
        //    cbocontusionSide.SelectedIndex = cbofractureSide.SelectedIndex = cboMedEpiconSide.SelectedIndex = 2;
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
            //SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' Order By BodyPart,Heading";
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
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "'  and IsConsidered=0 Order By BodyPart,Heading";
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
                ids += Session["PatientIE_ID"].ToString() + ",";
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
        //long _StdID = Convert.ToInt64(iStdID);
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

    //private void LoadDV_Click(object sender, ImageClickEventArgs e)
    //{
    //    PopulateUIDefaults();
    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        _CurIEid = Session["PatientIE_ID2"].ToString();
        _FuId = Session["patientFUId"].ToString();
        SaveDiagnosis(_CurIEid);
        SaveUI(_CurIEid, _FuId, ieMode, true);
        SaveStandards(Session["PatientIE_ID2"].ToString());
        PopulateUI(Session["PatientIE_ID2"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }

    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        //foreach (XmlNode node in doc.SelectNodes("//HSM/ROMs/ROM"))
        //{
        //    cboRangeOfMotionLeft.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
        //foreach (XmlNode node in doc.SelectNodes("//HSM/Results/Result"))
        //{
        //    cboTinelLeft.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
        //foreach (XmlNode node in doc.SelectNodes("//HSM/ROMs/ROM"))
        //{
        //    cboRangeOfMotionRight.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
        //foreach (XmlNode node in doc.SelectNodes("//HSM/Results/Result"))
        //{
        //    cboTinelRight.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
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
            SqlStr = "Select * from tblFUbpElbow WHERE PatientFU_ID = " + _FuId;
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
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Elbow.xml"));
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
        string path = Server.MapPath("~/Template/ElbowCC.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");

        DefaultCCPEModel model = new DefaultCCPEModel();
        model = gDbhelperobj.getDefaultCCPEValues("Elbow", "Left");
        body = body.Replace("#LCC", model.CC);
        model = gDbhelperobj.getDefaultCCPEValues("Elbow", "Right");
        body = body.Replace("#RCC", model.CC);

        CF.InnerHtml = body;

    }

    public void bindPE(string p)
    {
        string path = Server.MapPath("~/Template/ElbowPE.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");

        DefaultCCPEModel model = new DefaultCCPEModel();
        model = gDbhelperobj.getDefaultCCPEValues("Elbow", "Right");
        body = body.Replace("#RPE", model.PE);
        model = gDbhelperobj.getDefaultCCPEValues("Elbow", "Left");
        body = body.Replace("#LPE", model.PE);

        divPE.InnerHtml = body;

    }
}