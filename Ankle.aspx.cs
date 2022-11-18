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

public partial class Ankle : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Ankle";
    string Position = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();


    ILog log = log4net.LogManager.GetLogger(typeof(Ankle));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        Session["PageName"] = "Ankle";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["PatientIE_ID"] != null)
            {
                bindDropdown();
                BindROM();
                _CurIEid = Session["PatientIE_ID"].ToString();

                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblbpAnkle WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                DataRow rw = ds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                if (rw != null)
                {
                    // row exists
                    PopulateUIDefaults();
                    BindDataGrid();
                    bindCC(Position);
                    bindPE(Position);


                }
                else
                {

                    PopulateUI(_CurIEid);

                    BindDataGrid();

                }
                if (Position != "")
                {
                    switch (Position)
                    {
                        case "L":
                            //first div
                            //wrpLeft1.Visible = true;
                            //wrpRight1.Visible = false;
                            //Second div
                            //wrpLeft2.Visible = true;
                            //wrpRight2.Visible = false;

                            break;
                        case "R":
                            //first div
                            //wrpLeft1.Visible = false;
                            //wrpRight1.Visible = true;
                            //Second div
                            //wrpLeft2.Visible = false;
                            //wrpRight2.Visible = true;

                            break;
                        case "B":
                            //first div
                            //wrpLeft1.Visible = true;
                            //wrpRight1.Visible = true;
                            //Second div
                            //wrpLeft2.Visible = true;
                            //wrpRight2.Visible = true;

                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
        }
        BindDCDataGrid();
        Logger.Info(Session["uname"].ToString() + "- Visited in  Ankle for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }
    public string SaveUI(string ieID, string ieMode, bool bpIsChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";
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
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        if (_ieMode == "New")
            return "Ankle has been added...";
        else if (_ieMode == "Update")
            return "Ankle has been updated...";
        else if (_ieMode == "Delete")
            return "Ankle has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string ieID)
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
            CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();


            string orgval = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();
            string editval = sqlTbl.Rows[0]["PEvalue"].ToString();


            //Position = Request.QueryString["P"];


            //if (Position == "L")
            //{
            //    orgval = orgval.Replace("#rigthtdiv", "style='display:none'");
            //    editval = editval.Replace("#rigthtdiv", "style='display:none'");

            //    orgval = orgval.Replace("#leftdiv", "style='display:block'");
            //    editval = editval.Replace("#leftdiv", "style='display:block'");
            //}
            //else if (Position == "R")
            //{
            //    orgval = orgval.Replace("#leftdiv", "style='display:none'");
            //    editval = editval.Replace("#leftdiv", "style='display:none'");

            //    orgval = orgval.Replace("#rigthtdiv", "style='display:block'");
            //    editval = editval.Replace("#rigthtdiv", "style='display:block'");
            //}
            //else
            //{
            //    orgval = orgval.Replace("#leftdiv", "style='display:block'");
            //    editval = editval.Replace("#leftdiv", "style='display:block'");

            //    orgval = orgval.Replace("#rigthtdiv", "style='display:block'");
            //    editval = editval.Replace("#rigthtdiv", "style='display:block'");
            //}



            divPE.InnerHtml = editval;

            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "bindRadiobuttonValues('" + sqlTbl.Rows[0]["PERangeOfMotionRight"].ToString() + "','" + sqlTbl.Rows[0]["PERangeOfMotionLeft"].ToString() + "')", true);

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
            //txtPainScaleLeft.Text = node.SelectSingleNode("PainScaleLeft") == null ? txtPainScaleLeft.Text.ToString().Trim() : node.SelectSingleNode("PainScaleLeft").InnerText;
            //txtPainScaleRight.Text = node.SelectSingleNode("PainScaleRight") == null ? txtPainScaleRight.Text.ToString().Trim() : node.SelectSingleNode("PainScaleRight").InnerText;
            ////node.SelectSingleNode("FreeForm").InnerText = "arumwritetest";
            //chkMedMalleolusLeft.Checked = node.SelectSingleNode("MedMalleolusLeft") == null ? chkMedMalleolusLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("MedMalleolusLeft").InnerText);
            //chkLatMalleolusLeft.Checked = node.SelectSingleNode("LatMalleolusLeft") == null ? chkLatMalleolusLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("LatMalleolusLeft").InnerText);
            //chkAchillesLeft.Checked = node.SelectSingleNode("AchillesLeft") == null ? chkAchillesLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AchillesLeft").InnerText);
            //chkMedMalleolusRight.Checked = node.SelectSingleNode("MedMalleolusRight") == null ? chkMedMalleolusRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("MedMalleolusRight").InnerText);
            //chkLatMalleolusRight.Checked = node.SelectSingleNode("LatMalleolusRight") == null ? chkLatMalleolusRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("LatMalleolusRight").InnerText);
            //chkAchillesRight.Checked = node.SelectSingleNode("AchillesRight") == null ? chkAchillesRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AchillesRight").InnerText);
            ////chkPalpationMedMalleolusLeft.Checked = node.SelectSingleNode("PalpationMedMalleolusLeft") == null ? chkPalpationMedMalleolusLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PalpationMedMalleolusLeft").InnerText);
            //chkPalpationLatMalleolusLeft.Checked = node.SelectSingleNode("PalpationLatMalleolusLeft") == null ? chkPalpationLatMalleolusLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PalpationLatMalleolusLeft").InnerText);
            //chkPalpationAchillesLeft.Checked = node.SelectSingleNode("PalpationAchillesLeft") == null ? chkPalpationAchillesLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PalpationAchillesLeft").InnerText);
            //chkWorsePlantarLeft.Checked = node.SelectSingleNode("WorsePlantarLeft") == null ? chkWorsePlantarLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorsePlantarLeft").InnerText);
            //chkWorseDorsiLeft.Checked = node.SelectSingleNode("WorseDorsiLeft") == null ? chkWorseDorsiLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseDorsiLeft").InnerText);
            //chkWorseEversionLeft.Checked = node.SelectSingleNode("WorseEversionLeft") == null ? chkWorseEversionLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseEversionLeft").InnerText);
            //chkWorseInversionLeft.Checked = node.SelectSingleNode("WorseInversionLeft") == null ? chkWorseInversionLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseInversionLeft").InnerText);
            //chkWorseExtensionLeft.Checked = node.SelectSingleNode("WorseExtensionLeft") == null ? chkWorseExtensionLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseExtensionLeft").InnerText);
            //chkWorseAmbulationLeft.Checked = node.SelectSingleNode("WorseAmbulationLeft") == null ? chkWorseAmbulationLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseAmbulationLeft").InnerText);
            //chkEdemaLeft.Checked = node.SelectSingleNode("EdemaLeft") == null ? chkEdemaLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("EdemaLeft").InnerText);
            //chkEcchymosisLeft.Checked = node.SelectSingleNode("EcchymosisLeft") == null ? chkEcchymosisLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("EcchymosisLeft").InnerText);
            //chkPalpationMedMalleolusRight.Checked = node.SelectSingleNode("PalpationMedMalleolusRight") == null ? chkPalpationMedMalleolusRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PalpationMedMalleolusRight").InnerText);
            //chkPalpationLatMalleolusRight.Checked = node.SelectSingleNode("PalpationLatMalleolusRight") == null ? chkPalpationLatMalleolusRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PalpationLatMalleolusRight").InnerText);
            //chkPalpationAchillesRight.Checked = node.SelectSingleNode("PalpationAchillesRight") == null ? chkPalpationAchillesRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PalpationAchillesRight").InnerText);
            //chkWorsePlantarRight.Checked = node.SelectSingleNode("WorsePlantarRight") == null ? chkWorsePlantarRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorsePlantarRight").InnerText);
            //chkWorseDorsiRight.Checked = node.SelectSingleNode("WorseDorsiRight") == null ? chkWorseDorsiRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseDorsiRight").InnerText);
            //chkWorseEversionRight.Checked = node.SelectSingleNode("WorseEversionRight") == null ? chkWorseEversionRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseEversionRight").InnerText);
            //chkWorseInversionRight.Checked = node.SelectSingleNode("WorseInversionRight") == null ? chkWorseInversionRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseInversionRight").InnerText);
            //chkWorseExtensionRight.Checked = node.SelectSingleNode("WorseExtensionRight") == null ? chkWorseExtensionRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseExtensionRight").InnerText);
            //chkWorseAmbulationRight.Checked = node.SelectSingleNode("WorseAmbulationRight") == null ? chkWorseAmbulationRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseAmbulationRight").InnerText);
            //chkEdemaRight.Checked = node.SelectSingleNode("EdemaRight") == null ? chkEdemaRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("EdemaRight").InnerText);
            //chkEcchymosisRight.Checked = node.SelectSingleNode("EcchymosisRight") == null ? chkEcchymosisRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("EcchymosisRight").InnerText);
            //txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text : node.SelectSingleNode("FreeForm").InnerText;
            //txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text : node.SelectSingleNode("FreeFormP").InnerText;
            //cboRangeOfMotionRight.Text = node.SelectSingleNode("RangeOfMotionRight") == null ? cboRangeOfMotionRight.Text.ToString().Trim() : node.SelectSingleNode("RangeOfMotionRight").InnerText;
            //cboRangeOfMotionLeft.Text = node.SelectSingleNode("RangeOfMotionLeft") == null ? cboRangeOfMotionLeft.Text.ToString().Trim() : node.SelectSingleNode("RangeOfMotionLeft").InnerText;

            _fldPop = false;
        }
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
        //string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["dbPainTrax"].ConnectionString;
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
        SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["PatientIE_ID"].ToString());
    }

    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

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


        long _ieID = Convert.ToInt64(Session["PatientIE_ID"]);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpAnkle WHERE PatientIE_ID = " + _ieID;
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindgridPoup();
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