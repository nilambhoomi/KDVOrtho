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

public partial class EditFuAnkle : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Ankle";
    string Position = "";

    ILog log = log4net.LogManager.GetLogger(typeof(EditFuAnkle));

    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        if (!IsPostBack)
        {
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {

                bindDropdown();
                BindROM();

                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFubpAnkle WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpAnkle WHERE PatientIE_ID= " + _CurIEid + "");
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
                    // row exists
                    // PopulateUIDefaults();
                    //BindDataGrid();
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
                    //switch (Position)
                    //{
                    //    case "L":
                    //        //first div
                    //        //wrpLeft1.Visible = true;
                    //        //wrpRight1.Visible = false;
                    //        //Second div
                    //        wrpLeft2.Visible = true;
                    //        wrpRight2.Visible = false;

                    //        break;
                    //    case "R":
                    //        //first div
                    //        //wrpLeft1.Visible = false;
                    //        //wrpRight1.Visible = true;
                    //        //Second div
                    //        wrpLeft2.Visible = false;
                    //        wrpRight2.Visible = true;

                    //        break;
                    //    case "B":
                    //        //first div
                    //        //wrpLeft1.Visible = true;
                    //        //wrpRight1.Visible = true;
                    //        //Second div
                    //        wrpLeft2.Visible = true;
                    //        wrpRight2.Visible = true;

                    //        break;
                    //}
                }

            }
            else
            {
                Response.Redirect("EditFU.aspx");
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in EditFuAnkle for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);

    }
    public string SaveUI(string fuID, string ieMode, bool bpIsChecked)
    {
        long _fuID = Convert.ToInt64(fuID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpAnkle WHERE PatientFU_ID = " + _fuID;
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
            //TblRow["PainScaleRight"] = txtPainScaleRight.Text;
            //TblRow["PainScaleLeft"] = txtPainScaleLeft.Text;
            //TblRow["ConstantLeft"] = chkContentLeft.Checked;
            //TblRow["IntermittentLeft"] = chkIntermittentLeft.Checked;
            //TblRow["SharpLeft"] = chksharpLeft.Checked;
            //TblRow["ElectricLeft"] = chkelectricLeft.Checked;
            //TblRow["ShootingLeft"] = chkshootingLeft.Checked;
            //TblRow["ThrobblingLeft"] = chkthrobbingLeft.Checked;
            //TblRow["PulsatingLeft"] = chkpulsatingLeft.Checked;
            //TblRow["DullLeft"] = chkdullLeft.Checked;
            //TblRow["AchyLeft"] = chkachyLeft.Checked;
            //TblRow["MedMalleolusLeft"] = chkMedMalleolusLeft.Checked;
            //TblRow["LatMalleolusLeft"] = chkLatMalleolusLeft.Checked;
            //TblRow["AchillesLeft"] = chkAchillesLeft.Checked;
            //TblRow["ConstantRight"] = chkContentRight.Checked;
            //TblRow["IntermittentRight"] = chkIntermittentRight.Checked;
            //TblRow["SharpRight"] = chksharpRight.Checked;
            //TblRow["ElectricRight"] = chkelectricRight.Checked;
            //TblRow["ShootingRight"] = chkshootingRight.Checked;
            //TblRow["ThrobblingRight"] = chkthrobbingRight.Checked;
            //TblRow["PulsatingRight"] = chkpulsatingRight.Checked;
            //TblRow["DullRight"] = chkdullRight.Checked;
            //TblRow["AchyRight"] = chkachyRight.Checked;
            //TblRow["MedMalleolusRight"] = chkMedMalleolusRight.Checked;
            //TblRow["LatMalleolusRight"] = chkLatMalleolusRight.Checked;
            //TblRow["AchillesRight"] = chkAchillesRight.Checked;
            //TblRow["PalpationMedMalleolusLeft"] = chkPalpationMedMalleolusLeft.Checked;
            //TblRow["PalpationLatMalleolusLeft"] = chkPalpationLatMalleolusLeft.Checked;
            //TblRow["PalpationAchillesLeft"] = chkPalpationAchillesLeft.Checked;
            //TblRow["WorsePlantarLeft"] = chkWorsePlantarLeft.Checked;
            //TblRow["WorseDorsiLeft"] = chkWorseDorsiLeft.Checked;
            //TblRow["WorseEversionLeft"] = chkWorseEversionLeft.Checked;
            //TblRow["WorseInversionLeft"] = chkWorseInversionLeft.Checked;
            //TblRow["WorseExtensionLeft"] = chkWorseExtensionLeft.Checked;
            //TblRow["WorseAmbulationLeft"] = chkWorseAmbulationLeft.Checked;
            //TblRow["EdemaLeft"] = chkEdemaLeft.Checked;
            //TblRow["EcchymosisLeft"] = chkEcchymosisLeft.Checked;
            //TblRow["PalpationMedMalleolusRight"] = chkPalpationMedMalleolusRight.Checked;
            //TblRow["PalpationLatMalleolusRight"] = chkPalpationLatMalleolusRight.Checked;
            //TblRow["PalpationAchillesRight"] = chkPalpationAchillesRight.Checked;
            //TblRow["WorsePlantarRight"] = chkWorsePlantarRight.Checked;
            //TblRow["WorseDorsiRight"] = chkWorseDorsiRight.Checked;
            //TblRow["WorseEversionRight"] = chkWorseEversionRight.Checked;
            //TblRow["WorseInversionRight"] = chkWorseInversionRight.Checked;
            //TblRow["WorseExtensionRight"] = chkWorseExtensionRight.Checked;
            //TblRow["WorseAmbulationRight"] = chkWorseAmbulationRight.Checked;
            //TblRow["RangeOfMotionRight"] = cboRangeOfMotionRight.Text.ToString();
            //TblRow["RangeOfMotionLeft"] = cboRangeOfMotionLeft.Text.ToString();
            //TblRow["EdemaRight"] = chkEdemaRight.Checked;
            //TblRow["EcchymosisRight"] = chkEcchymosisRight.Checked;
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
            return "Ankle has been added...";
        else if (_ieMode == "Update")
            return "Ankle has been updated...";
        else if (_ieMode == "Delete")
            return "Ankle has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string fuID)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFubpAnkle WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];

            if (!string.IsNullOrEmpty(sqlTbl.Rows[0]["CCvalue"].ToString()) && sqlTbl.Rows[0]["CCvalue"].ToString() != "\r\n                ")
            {
                CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();

                //hdorgval.Value = sqlTbl.Rows[0]["CCvalueoriginal"].ToString();
            }
            else
            {
                bindCC(Position.ToUpper());
            }
            if (!string.IsNullOrEmpty(sqlTbl.Rows[0]["PEvalue"].ToString()) && sqlTbl.Rows[0]["PEvalue"].ToString() != "\r\n                ")
            {
                divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();

                //hdorgvalPE.Value = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();
            }
            else
            {
                bindPE(Position.ToUpper());
            }

            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

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
        SqlStr = "Select * from tblbpAnkle WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];

            //txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString().Trim();
            //txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString().Trim();

            //if (!string.IsNullOrEmpty(TblRow["ConstantRight"].ToString()))
            //{ chkContentRight.Checked = Convert.ToBoolean(TblRow["ConstantRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["IntermittentRight"].ToString()))
            //{ chkIntermittentRight.Checked = Convert.ToBoolean(TblRow["IntermittentRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["SharpRight"].ToString()))
            //{ chksharpRight.Checked = Convert.ToBoolean(TblRow["SharpRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["ElectricRight"].ToString()))
            //{ chkelectricRight.Checked = Convert.ToBoolean(TblRow["ElectricRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["ShootingRight"].ToString()))
            //{ chkshootingRight.Checked = Convert.ToBoolean(TblRow["ShootingRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["ThrobblingRight"].ToString()))
            //{ chkthrobbingRight.Checked = Convert.ToBoolean(TblRow["ThrobblingRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["PulsatingRight"].ToString()))
            //{ chkpulsatingRight.Checked = Convert.ToBoolean(TblRow["PulsatingRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["DullRight"].ToString()))
            //{ chkdullRight.Checked = Convert.ToBoolean(TblRow["DullRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["AchyRight"].ToString()))
            //{ chkachyRight.Checked = Convert.ToBoolean(TblRow["AchyRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["ConstantLeft"].ToString()))
            //{ chkContentLeft.Checked = Convert.ToBoolean(TblRow["ConstantLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["IntermittentLeft"].ToString()))
            //{ chkIntermittentLeft.Checked = Convert.ToBoolean(TblRow["IntermittentLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["SharpLeft"].ToString()))
            //{ chksharpLeft.Checked = Convert.ToBoolean(TblRow["SharpLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["ElectricLeft"].ToString()))
            //{ chkelectricLeft.Checked = Convert.ToBoolean(TblRow["ElectricLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["ShootingLeft"].ToString()))
            //{ chkshootingLeft.Checked = Convert.ToBoolean(TblRow["ShootingLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["ThrobblingLeft"].ToString()))
            //{ chkthrobbingLeft.Checked = Convert.ToBoolean(TblRow["ThrobblingLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["PulsatingLeft"].ToString()))
            //{ chkpulsatingLeft.Checked = Convert.ToBoolean(TblRow["PulsatingLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["DullLeft"].ToString()))
            //{ chkdullLeft.Checked = Convert.ToBoolean(TblRow["DullLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["AchyLeft"].ToString()))
            //{ chkachyLeft.Checked = Convert.ToBoolean(TblRow["AchyLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["MedMalleolusLeft"].ToString()))
            //{ chkMedMalleolusLeft.Checked = Convert.ToBoolean(TblRow["MedMalleolusLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["LatMalleolusLeft"].ToString()))
            //{ chkLatMalleolusLeft.Checked = Convert.ToBoolean(TblRow["LatMalleolusLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["AchillesLeft"].ToString()))
            //{ chkAchillesLeft.Checked = Convert.ToBoolean(TblRow["AchillesLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["MedMalleolusRight"].ToString()))
            //{ chkMedMalleolusRight.Checked = Convert.ToBoolean(TblRow["MedMalleolusRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["LatMalleolusRight"].ToString()))
            //{ chkLatMalleolusRight.Checked = Convert.ToBoolean(TblRow["LatMalleolusRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["AchillesRight"].ToString()))
            //{ chkAchillesRight.Checked = Convert.ToBoolean(TblRow["AchillesRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["PalpationMedMalleolusLeft"].ToString()))
            //{ chkPalpationMedMalleolusLeft.Checked = Convert.ToBoolean(TblRow["PalpationMedMalleolusLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["PalpationLatMalleolusLeft"].ToString()))
            //{ chkPalpationLatMalleolusLeft.Checked = Convert.ToBoolean(TblRow["PalpationLatMalleolusLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["PalpationAchillesLeft"].ToString()))
            //{ chkPalpationAchillesLeft.Checked = Convert.ToBoolean(TblRow["PalpationAchillesLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorsePlantarLeft"].ToString()))
            //{ chkWorsePlantarLeft.Checked = Convert.ToBoolean(TblRow["WorsePlantarLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseDorsiLeft"].ToString()))
            //{ chkWorseDorsiLeft.Checked = Convert.ToBoolean(TblRow["WorseDorsiLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseEversionLeft"].ToString()))
            //{ chkWorseEversionLeft.Checked = Convert.ToBoolean(TblRow["WorseEversionLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseInversionLeft"].ToString()))
            //{ chkWorseInversionLeft.Checked = Convert.ToBoolean(TblRow["WorseInversionLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseExtensionLeft"].ToString()))
            //{ chkWorseExtensionLeft.Checked = Convert.ToBoolean(TblRow["WorseExtensionLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseAmbulationLeft"].ToString()))
            //{ chkWorseAmbulationLeft.Checked = Convert.ToBoolean(TblRow["WorseAmbulationLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["EdemaLeft"].ToString()))
            //{ chkEdemaLeft.Checked = Convert.ToBoolean(TblRow["EdemaLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["EcchymosisLeft"].ToString()))
            //{ chkEcchymosisLeft.Checked = Convert.ToBoolean(TblRow["EcchymosisLeft"]); }

            //if (!string.IsNullOrEmpty(TblRow["PalpationMedMalleolusRight"].ToString()))
            //{ chkPalpationMedMalleolusRight.Checked = Convert.ToBoolean(TblRow["PalpationMedMalleolusRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["PalpationLatMalleolusRight"].ToString()))
            //{ chkPalpationLatMalleolusRight.Checked = Convert.ToBoolean(TblRow["PalpationLatMalleolusRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["PalpationAchillesRight"].ToString()))
            //{ chkPalpationAchillesRight.Checked = Convert.ToBoolean(TblRow["PalpationAchillesRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorsePlantarRight"].ToString()))
            //{ chkWorsePlantarRight.Checked = Convert.ToBoolean(TblRow["WorsePlantarRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseDorsiRight"].ToString()))
            //{ chkWorseDorsiRight.Checked = Convert.ToBoolean(TblRow["WorseDorsiRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseEversionRight"].ToString()))
            //{ chkWorseEversionRight.Checked = Convert.ToBoolean(TblRow["WorseEversionRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseInversionRight"].ToString()))
            //{ chkWorseInversionRight.Checked = Convert.ToBoolean(TblRow["WorseInversionRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseExtensionRight"].ToString()))
            //{ chkWorseExtensionRight.Checked = Convert.ToBoolean(TblRow["WorseExtensionRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["WorseAmbulationRight"].ToString()))
            //{ chkWorseAmbulationRight.Checked = Convert.ToBoolean(TblRow["WorseAmbulationRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["EdemaRight"].ToString()))
            //{ chkEdemaRight.Checked = Convert.ToBoolean(TblRow["EdemaRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["EcchymosisRight"].ToString()))
            //{ chkEcchymosisRight.Checked = Convert.ToBoolean(TblRow["EcchymosisRight"]); }

            //if (!string.IsNullOrEmpty(TblRow["RangeOfMotionRight"].ToString()))
            //{ cboRangeOfMotionRight.Text = TblRow["RangeOfMotionRight"].ToString(); }

            //if (!string.IsNullOrEmpty(TblRow["RangeOfMotionLeft"].ToString()))
            //{ cboRangeOfMotionLeft.Text = TblRow["RangeOfMotionLeft"].ToString(); }


            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

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

        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Ankle");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
                     txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text : node.SelectSingleNode("FreeForm").InnerText;
            txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text : node.SelectSingleNode("FreeFormP").InnerText;
            bindCC(Position.ToUpper());
            bindPE(Position.ToUpper());
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
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' and IsConsidered=0 Order By BodyPart,Heading";
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
        bindgridPoup();
        //SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        ////SaveStandards(Session["PatientIE_ID"].ToString());
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
            ieID = Session["PatientIE_ID"].ToString();
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
                        ids += DiagCode_ID + ",";
                        SaveDiagUI(ieID, DiagCode_ID, true, _CurBP, Description, DiagCode);
                    }

                }
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
            TblRow["PatientFU_ID"] = Session["patientFUId"].ToString();
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
                if (_FuId == "" || _FuId == "0")
                    return;
                string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
                string SqlStr = "";

                SqlDataAdapter oSQLAdpr;
                DataTable Diagnosis = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientFU_ID = " + _FuId + " AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
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
        _CurIEid = Session["PatientIE_ID"].ToString();
        _FuId = Session["patientFUId"].ToString();
        SaveDiagnosis(_CurIEid);
        SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["patientFUId"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }


    protected void btnDaigSave_Click(object sender, EventArgs e)
    {
        SaveStandardsPopup(Session["PatientIE_ID"].ToString());
        BindDCDataGrid();
        txDesc.Text = string.Empty;
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "TestFU", "closeModelPopup()", true);
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
    
    private void bindgridPoup()
    {
        try
        {
            _FuId = Session["patientFUId"].ToString();
            string _CurBodyPart = _CurBP;
            string _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";
            if (_FuId != "")
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTSFU(" + _FuId + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            else
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTSFU('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            ds = gDbhelperobj.selectData(SqlStr);

            dgvDiagCodesPopup.DataSource = ds;
            dgvDiagCodesPopup.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

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
            SqlStr = "Select * from tblFUbpAnkle WHERE PatientFU_ID = " + _FuId;
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
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Ankle.xml"));
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

    public void bindCC(string p)
    {
        string path = Server.MapPath("~/Template/AnkleCC.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");

        DefaultCCPEModel model = new DefaultCCPEModel();
        model = gDbhelperobj.getDefaultCCPEValues("Ankle", "Left");
        body = body.Replace("#LCC", model.CC);
        model = gDbhelperobj.getDefaultCCPEValues("Ankle", "Right");
        body = body.Replace("#RCC", model.CC);

        CF.InnerHtml = body;

    }

    public void bindPE(string p)
    {
        string path = Server.MapPath("~/Template/AnklePE.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");

        DefaultCCPEModel model = new DefaultCCPEModel();
        model = gDbhelperobj.getDefaultCCPEValues("Ankle", "Right");
        body = body.Replace("#RPE", model.PE);
        model = gDbhelperobj.getDefaultCCPEValues("Ankle", "Left");
        body = body.Replace("#LPE", model.PE);


        divPE.InnerHtml = body;

    }
}