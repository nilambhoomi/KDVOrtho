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

public partial class Wrist : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Wrist";
    string Position = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();


    ILog log = log4net.LogManager.GetLogger(typeof(Wrist));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        Session["PageName"] = "Wrist";
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
                string query = ("select count(*) as count1 FROM tblbpWrist WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
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
                            //Bilaterally checkbox
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
                            //Left checkbox
                            //chkTinelLeft.Enabled = true;
                            //chkPhalenLeft.Enabled = true;
                            //chkFinkelsteinleft.Enabled = true;
                            //chkPainUponUlnarDeviationLeft.Enabled = true;
                            //chkPainUponRadialLeft.Enabled = true;
                            //chkPainUponDorsiFlexionLeft.Enabled = true;
                            //Bilaterally checkbox
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
                Response.Redirect("Page1.aspx");
            }
        }
        BindDCDataGrid();
        Logger.Info(Session["uname"].ToString() + "- Visited in  Wrist for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }
    public string SaveUI(string ieID, string ieMode, bool bpIsChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpWrist WHERE PatientIE_ID = " + ieID;
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

            TblRow["FlexionRangeNormal"] = txtFlexionRange.Text.Trim();
            TblRow["ExtensionRangeNormal"] = txtExtensionRange.Text.Trim();
            TblRow["RadialdeviationRangeNormal"] = txtRadialdeviationRange.Text.Trim();
            TblRow["UlnarDeviationRangeNormal"] = txtUlnarDeviationRange.Text.Trim();



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
            //TblRow["PainUponDorsiFlexionBilaterally"] = chkPainUponDorsiFlexionBilaterally.Checked;
            //TblRow["RangeOfMotionRight"] = cboRangeOfMotionRight.Text.ToString();
            //TblRow["RangeOfMotionLeft"] = cboRangeOfMotionLeft.Text.ToString();
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
            return "Wrist has been added...";
        else if (_ieMode == "Update")
            return "Wrist has been updated...";
        else if (_ieMode == "Delete")
            return "Wrist has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string ieID)
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

            //txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString();
            //chkContentLeft.Checked = CommonConvert.ToBoolean(TblRow["ConstantLeft"].ToString());
            //chkIntermittentLeft.Checked = CommonConvert.ToBoolean(TblRow["IntermittentLeft"].ToString());
            //chkSharpLeft.Checked = CommonConvert.ToBoolean(TblRow["SharpLeft"].ToString());
            //chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());

            //chkShootingLeft.Checked = CommonConvert.ToBoolean(TblRow["ShootingLeft"].ToString());
            //chkThrobblingLeft.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingLeft"].ToString());
            //chkPulsatingLeft.Checked = CommonConvert.ToBoolean(TblRow["PulsatingLeft"].ToString());
            //chkDullLeft.Checked = CommonConvert.ToBoolean(TblRow["DullLeft"].ToString());
            //chkAchyLeft.Checked = CommonConvert.ToBoolean(TblRow["AchyLeft"].ToString());
            //chkulnarLeft.Checked = CommonConvert.ToBoolean(TblRow["UlnarLeft"].ToString());
            //chkradiusLeft.Checked = CommonConvert.ToBoolean(TblRow["RadialLeft"].ToString());
            //chkDorsalLeft.Checked = CommonConvert.ToBoolean(TblRow["DorsalLeft"].ToString());
            //chkPalmarLeft.Checked = CommonConvert.ToBoolean(TblRow["PalmarLeft"].ToString());
            //chkLiftingObjectLeft.Checked = CommonConvert.ToBoolean(TblRow["LiftingObjectLeft"].ToString());
            //chkWorkingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorkingLeft"].ToString());
            //chkMovementLeft.Checked = CommonConvert.ToBoolean(TblRow["MovementLeft"].ToString());
            //chkRotationLeft.Checked = CommonConvert.ToBoolean(TblRow["RotationLeft"].ToString());

            //txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString().Trim();
            //chkContentRight.Checked = CommonConvert.ToBoolean(TblRow["ConstantRight"].ToString());
            //chkIntermittentRight.Checked = CommonConvert.ToBoolean(TblRow["IntermittentRight"].ToString());
            //chkSharpRight.Checked = CommonConvert.ToBoolean(TblRow["SharpRight"].ToString());
            //chkElectricRight.Checked = CommonConvert.ToBoolean(TblRow["ElectricRight"].ToString());
            //chkShootingRight.Checked = CommonConvert.ToBoolean(TblRow["ShootingRight"].ToString());
            //chkThrobblingRight.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingRight"].ToString());
            //chkPulsatingRight.Checked = CommonConvert.ToBoolean(TblRow["PulsatingRight"].ToString());
            //chkDullRight.Checked = CommonConvert.ToBoolean(TblRow["DullRight"].ToString());
            //chkAchyRight.Checked = CommonConvert.ToBoolean(TblRow["AchyRight"].ToString());
            //chkulnarRight.Checked = CommonConvert.ToBoolean(TblRow["UlnarRight"].ToString());
            //chkradiusRight.Checked = CommonConvert.ToBoolean(TblRow["RadialRight"].ToString());
            //chkDorsalRight.Checked = CommonConvert.ToBoolean(TblRow["DorsalRight"].ToString());
            //chkPalmarRight.Checked = CommonConvert.ToBoolean(TblRow["PalmarRight"].ToString());
            //chkLiftingObjectRight.Checked = CommonConvert.ToBoolean(TblRow["LiftingObjectRight"].ToString());
            //chkRotationRight.Checked = CommonConvert.ToBoolean(TblRow["RotationRight"].ToString());
            //chkWorkingRight.Checked = CommonConvert.ToBoolean(TblRow["WorkingRight"].ToString());
            //chkMovementRight.Checked = CommonConvert.ToBoolean(TblRow["MovementRight"].ToString());

            txtFlexionLeft.Text = TblRow["FlexionROMLeft"].ToString();
            txtFlexionRight.Text = TblRow["FlexionROMRight"].ToString();
            txtExtensionRight.Text = TblRow["ExtensionROMRight"].ToString();
            txtExtensionLeft.Text = TblRow["ExtensionROMLeft"].ToString();
            txtRadialdeviationLeft.Text = TblRow["RadialDeviationROMLeft"].ToString();
            txtRadialdeviationRight.Text = TblRow["RadialDeviationROMRight"].ToString();
            txtUlnarDeviationRight.Text = TblRow["UlnarDeviationROMRight"].ToString();
            txtUlnarDeviationLeft.Text = TblRow["UlnarDeviationROMLeft"].ToString();


            txtFlexionRange.Text = Convert.ToString(TblRow["FlexionRangeNormal"]);
            txtExtensionRange.Text = Convert.ToString(TblRow["ExtensionRangeNormal"]);
            txtRadialdeviationRange.Text = Convert.ToString(TblRow["RadialdeviationRangeNormal"]);
            txtUlnarDeviationRange.Text = Convert.ToString(TblRow["UlnarDeviationRangeNormal"]);


            //chkPalpationUlnarLeft.Checked = CommonConvert.ToBoolean(TblRow["PalpationUlnarLeft"].ToString());
            //chkPalpationUlnarRight.Checked = CommonConvert.ToBoolean(TblRow["PalpationUlnarRight"].ToString());
            //chkPalpationRadialLeft.Checked = CommonConvert.ToBoolean(TblRow["PalpationRadialLeft"].ToString());
            //chkPalpationRadialRight.Checked = CommonConvert.ToBoolean(TblRow["PalpationRadialRight"].ToString());
            //chkPalpationDorsalLeft.Checked = CommonConvert.ToBoolean(TblRow["PalpationDorsalLeft"].ToString());
            //chkPalpationDorsalRight.Checked = CommonConvert.ToBoolean(TblRow["PalpationDorsalRight"].ToString());
            //chkPalpationPalmarLeft.Checked = CommonConvert.ToBoolean(TblRow["PalpationPalmarLeft"].ToString());
            //chkPalpationPalmarRight.Checked = CommonConvert.ToBoolean(TblRow["PalpationPalmarRight"].ToString());
            //chkTinelLeft.Checked = CommonConvert.ToBoolean(TblRow["TinelLeft"].ToString());
            //chkTinelRight.Checked = CommonConvert.ToBoolean(TblRow["TinelRight"].ToString());
            //chkTinelBilaterally.Checked = CommonConvert.ToBoolean(TblRow["TinelBilaterally"].ToString());
            //chkPhalenLeft.Checked = CommonConvert.ToBoolean(TblRow["PhalenLeft"].ToString());
            //chkPhalenRight.Checked = CommonConvert.ToBoolean(TblRow["PhalenRight"].ToString());
            //chkPhalenBilaterally.Checked = CommonConvert.ToBoolean(TblRow["PhalenBilaterally"].ToString());
            //chkFinkelsteinleft.Checked = CommonConvert.ToBoolean(TblRow["FinkelsteinLeft"].ToString());
            //chkFinkelsteinRight.Checked = CommonConvert.ToBoolean(TblRow["FinkelsteinRight"].ToString());
            //chkFinkelsteinBilaterally.Checked = CommonConvert.ToBoolean(TblRow["FinkelsteinBilaterally"].ToString());
            //chkPainUponUlnarDeviationLeft.Checked = CommonConvert.ToBoolean(TblRow["PainUponUlnarDeviationLeft"].ToString());
            //chkPainUponUlnarDeviationRight.Checked = CommonConvert.ToBoolean(TblRow["PainUponUlnarDeviationRight"].ToString());
            //chkPainUponUlnarDeviationBilaterally.Checked = CommonConvert.ToBoolean(TblRow["PainUponUlnarDeviationBilaterally"].ToString());
            //chkPainUponRadialLeft.Checked = CommonConvert.ToBoolean(TblRow["PainUponRadialLeft"].ToString());
            //chkPainUponRadialRight.Checked = CommonConvert.ToBoolean(TblRow["PainUponRadialRight"].ToString());
            //chkPainUponRadialBilaterally.Checked = CommonConvert.ToBoolean(TblRow["PainUponRadialBilaterally"].ToString());
            //chkPainUponDorsiFlexionLeft.Checked = CommonConvert.ToBoolean(TblRow["PainUponDorsiFlexionLeft"].ToString());
            //chkPainUponDorsiFlexionRight.Checked = CommonConvert.ToBoolean(TblRow["PainUponDorsiFlexionRight"].ToString());
            //chkPainUponDorsiFlexionBilaterally.Checked = CommonConvert.ToBoolean(TblRow["PainUponDorsiFlexionBilaterally"].ToString());
            //if (!string.IsNullOrEmpty(TblRow["RangeOfMotionRight"].ToString()))
            //{ cboRangeOfMotionRight.Text = TblRow["RangeOfMotionRight"].ToString(); }

            //if (!string.IsNullOrEmpty(TblRow["RangeOfMotionLeft"].ToString()))
            //{ cboRangeOfMotionLeft.Text = TblRow["RangeOfMotionLeft"].ToString(); }

            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            CF.InnerHtml = TblRow["CCvalue"].ToString();


            string orgval = TblRow["PEvalueoriginal"].ToString();
            string editval = TblRow["PEvalue"].ToString();

            divPE.InnerHtml = editval;


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP('" + Request.QueryString["P"] + "');", true);

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Wrist");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            //txtPainScaleLeft.Text = node.SelectSingleNode("PainScaleLeft") == null ? txtPainScaleLeft.Text.ToString().Trim() : node.SelectSingleNode("PainScaleLeft").InnerText;
            //chkSharpLeft.Checked = node.SelectSingleNode("SharpLeft") == null ? chkSharpLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SharpLeft").InnerText);
            //chkElectricLeft.Checked = node.SelectSingleNode("ElectricLeft") == null ? chkElectricLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricLeft").InnerText);
            //chkShootingLeft.Checked = node.SelectSingleNode("ShootingLeft") == null ? chkShootingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ShootingLeft").InnerText);
            //chkThrobblingLeft.Checked = node.SelectSingleNode("ThrobblingLeft") == null ? chkThrobblingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ThrobblingLeft").InnerText);
            //chkPulsatingLeft.Checked = node.SelectSingleNode("PulsatingLeft") == null ? chkPulsatingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("PulsatingLeft").InnerText);
            //chkDullLeft.Checked = node.SelectSingleNode("DullLeft") == null ? chkDullLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DullLeft").InnerText);
            //chkAchyLeft.Checked = node.SelectSingleNode("AchyLeft") == null ? chkAchyLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("AchyLeft").InnerText);
            //chkMovementLeft.Checked = node.SelectSingleNode("WorseMovementLeft") == null ? chkMovementLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovementLeft").InnerText);
            //chkWorkingLeft.Checked = node.SelectSingleNode("WorseWalkingLeft") == null ? chkWorkingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWalkingLeft").InnerText);
            // chkWorseStairsLeft.Checked = node.SelectSingleNode("WorseStairsLeft") == null ? chkWorseStairsLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseStairsLeft").InnerText);
            // chkWorseSquattingLeft.Checked = node.SelectSingleNode("WorseSquattingLeft") == null ? chkWorseSquattingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSquattingLeft").InnerText);
            // chkWorseActivitiesLeft.Checked = node.SelectSingleNode("WorseActivitiesLeft") == null ? chkWorseActivitiesLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseActivitiesLeft").InnerText);
            // chkWorseOtherLeft.Checked = node.SelectSingleNode("WorseOtherLeft") == null ? chkWorseOtherLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseOtherLeft").InnerText);
            // txtWorseOtherTextLeft.Text = node.SelectSingleNode("WorseOtherTextLeft") == null ? txtWorseOtherTextLeft.Text.ToString().Trim() : node.SelectSingleNode("WorseOtherTextLeft").InnerText;
            // chkImprovedRestingLeft.Checked = node.SelectSingleNode("ImprovedRestingLeft") == null ? chkImprovedRestingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedRestingLeft").InnerText);
            // chkImprovedMedicationLeft.Checked = node.SelectSingleNode("ImprovedMedicationLeft") == null ? chkImprovedMedicationLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
            // chkImprovedTherapyLeft.Checked = node.SelectSingleNode("ImprovedTherapyLeft") == null ? chkImprovedTherapyLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapyLeft").InnerText);
            // chkImprovedSleepingLeft.Checked = node.SelectSingleNode("ImprovedSleepingLeft") == null ? chkImprovedSleepingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleepingLeft").InnerText);
            //txtPainScaleRight.Text = node.SelectSingleNode("PainScaleRight") == null ? txtPainScaleRight.Text.ToString().Trim() : node.SelectSingleNode("PainScaleRight").InnerText;
            //chkSharpRight.Checked = node.SelectSingleNode("SharpRight") == null ? chkSharpRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SharpRight").InnerText);
            //chkElectricRight.Checked = node.SelectSingleNode("ElectricRight") == null ? chkElectricRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricRight").InnerText);
            //chkShootingRight.Checked = node.SelectSingleNode("ShootingRight") == null ? chkShootingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ShootingRight").InnerText);
            //chkThrobblingRight.Checked = node.SelectSingleNode("ThrobblingRight") == null ? chkThrobblingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ThrobblingRight").InnerText);
            //chkPulsatingRight.Checked = node.SelectSingleNode("PulsatingRight") == null ? chkPulsatingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("PulsatingRight").InnerText);
            //chkDullRight.Checked = node.SelectSingleNode("DullRight") == null ? chkDullRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DullRight").InnerText);
            //chkAchyRight.Checked = node.SelectSingleNode("AchyRight") == null ? chkAchyRight.Checked : Convert.ToBoolean(node.SelectSingleNode("AchyRight").InnerText);
            //chkWorseMovementRight.Checked = node.SelectSingleNode("WorseMovementRight") == null ? chkWorseMovementRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovementRight").InnerText);
            //chkWorseWalkingRight.Checked = node.SelectSingleNode("WorseWalkingRight") == null ? chkWorseWalkingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWalkingRight").InnerText);
            //chkWorseStairsRight.Checked = node.SelectSingleNode("WorseStairsRight") == null ? chkWorseStairsRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseStairsRight").InnerText);
            //chkWorseSquattingRight.Checked = node.SelectSingleNode("WorseSquattingRight") == null ? chkWorseSquattingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSquattingRight").InnerText);
            //chkWorseActivitiesRight.Checked = node.SelectSingleNode("WorseActivitiesRight") == null ? chkWorseActivitiesRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseActivitiesRight").InnerText);
            //chkWorseOtherRight.Checked = node.SelectSingleNode("WorseOtherRight") == null ? chkWorseOtherRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseOtherRight").InnerText);
            //txtWorseOtherTextRight.Text = node.SelectSingleNode("WorseOtherTextRight") == null ? txtWorseOtherTextRight.Text.ToString().Trim() : node.SelectSingleNode("WorseOtherTextRight").InnerText;
            //chkImprovedRestingRight.Checked = node.SelectSingleNode("ImprovedRestingRight") == null ? chkImprovedRestingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedRestingRight").InnerText);
            //chkImprovedMedicationRight.Checked = node.SelectSingleNode("ImprovedMedicationRight") == null ? chkImprovedMedicationRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedicationRight").InnerText);
            //chkImprovedTherapyRight.Checked = node.SelectSingleNode("ImprovedTherapyRight") == null ? chkImprovedTherapyRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapyRight").InnerText);
            //chkImprovedSleepingRight.Checked = node.SelectSingleNode("ImprovedSleepingRight") == null ? chkImprovedSleepingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleepingRight").InnerText);
            txtExtensionLeft.Text = node.SelectSingleNode("LEExtensionRight") == null ? txtExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionRight").InnerText;
            txtFlexionRight.Text = node.SelectSingleNode("LEFlexionRight") == null ? txtFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionRight").InnerText;
            txtExtensionLeft.Text = node.SelectSingleNode("LEExtensionLeft") == null ? txtExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionLeft").InnerText;
            txtFlexionLeft.Text = node.SelectSingleNode("LEFlexionLeft") == null ? txtFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionLeft").InnerText;
            txtExtensionRight.Text = node.SelectSingleNode("LEExtensionRight") == null ? txtExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionRight").InnerText;


            txtFlexionRange.Text = node.SelectSingleNode("FlexionRangeNormal") == null ? txtFlexionRange.Text.ToString().Trim() : node.SelectSingleNode("FlexionRangeNormal").InnerText;
            txtExtensionRange.Text = node.SelectSingleNode("ExtensionRangeNormal") == null ? txtExtensionRange.Text.ToString().Trim() : node.SelectSingleNode("ExtensionRangeNormal").InnerText;
            txtRadialdeviationRange.Text = node.SelectSingleNode("RadialdeviationRangeNormal") == null ? txtRadialdeviationRange.Text.ToString().Trim() : node.SelectSingleNode("RadialdeviationRangeNormal").InnerText;
            txtUlnarDeviationRange.Text = node.SelectSingleNode("UlnarDeviationRangeNormal") == null ? txtUlnarDeviationRange.Text.ToString().Trim() : node.SelectSingleNode("UlnarDeviationRangeNormal").InnerText;


            //txtPalpationText1Left.Text = node.SelectSingleNode("PalpationText1Left") == null ? txtPalpationText1Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Left").InnerText;
            //txtPalpationText2Left.Text = node.SelectSingleNode("PalpationText2Left") == null ? txtPalpationText2Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Left").InnerText;
            //chkMedialLeft.Checked = node.SelectSingleNode("MedialLeft") == null ? chkMedialLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("MedialLeft").InnerText);
            //chkLateralLeft.Checked = node.SelectSingleNode("LateralLeft") == null ? chkLateralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("LateralLeft").InnerText);
            //chkSuperiorLeft.Checked = node.SelectSingleNode("SuperiorLeft") == null ? chkSuperiorLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SuperiorLeft").InnerText);
            //chkInferiorLeft.Checked = node.SelectSingleNode("InferiorLeft") == null ? chkInferiorLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("InferiorLeft").InnerText);
            //chkSupermedialLeft.Checked = node.SelectSingleNode("SupermedialLeft") == null ? chkSupermedialLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SupermedialLeft").InnerText);
            //chkSuperoLateralLeft.Checked = node.SelectSingleNode("SuperoLateralLeft") == null ? chkSuperoLateralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SuperoLateralLeft").InnerText);
            //chkInferomedialLeft.Checked = node.SelectSingleNode("InferomedialLeft") == null ? chkInferomedialLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("InferomedialLeft").InnerText);
            //chkInferoLateralLeft.Checked = node.SelectSingleNode("InferoLateralLeft") == null ? chkInferoLateralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("InferoLateralLeft").InnerText);
            //chkPeripatellarLeft.Checked = node.SelectSingleNode("PeripatellarLeft") == null ? chkPeripatellarLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("PeripatellarLeft").InnerText);
            //chkPeripatellarRight.Checked = node.SelectSingleNode("PeripatellarRight") == null ? chkPeripatellarRight.Checked : Convert.ToBoolean(node.SelectSingleNode("PeripatellarRight").InnerText);
            //chkMcMurrayLeft.Checked = node.SelectSingleNode("McMurrayLeft") == null ? chkMcMurrayLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("McMurrayLeft").InnerText);
            //chkLachmanLeft.Checked = node.SelectSingleNode("LachmanLeft") == null ? chkLachmanLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("LachmanLeft").InnerText);
            //chkAnteriorLeft.Checked = node.SelectSingleNode("AnteriorLeft") == null ? chkAnteriorLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("AnteriorLeft").InnerText);
            //chkPosteriorLeft.Checked = node.SelectSingleNode("PosteriorLeft") == null ? chkPosteriorLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("PosteriorLeft").InnerText);
            //chkVarusLeft.Checked = node.SelectSingleNode("VarusLeft") == null ? chkVarusLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("VarusLeft").InnerText);
            //chkValgusLeft.Checked = node.SelectSingleNode("ValgusLeft") == null ? chkValgusLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ValgusLeft").InnerText);
            //chkMcMurrayRight.Checked = node.SelectSingleNode("McMurrayRight") == null ? chkMcMurrayRight.Checked : Convert.ToBoolean(node.SelectSingleNode("McMurrayRight").InnerText);
            //chkLachmanRight.Checked = node.SelectSingleNode("LachmanRight") == null ? chkLachmanRight.Checked : Convert.ToBoolean(node.SelectSingleNode("LachmanRight").InnerText);
            //chkAnteriorRight.Checked = node.SelectSingleNode("AnteriorRight") == null ? chkAnteriorRight.Checked : Convert.ToBoolean(node.SelectSingleNode("AnteriorRight").InnerText);
            //chkPosteriorRight.Checked = node.SelectSingleNode("PosteriorRight") == null ? chkPosteriorRight.Checked : Convert.ToBoolean(node.SelectSingleNode("PosteriorRight").InnerText);
            //chkVarusRight.Checked = node.SelectSingleNode("VarusRight") == null ? chkVarusRight.Checked : Convert.ToBoolean(node.SelectSingleNode("VarusRight").InnerText);
            //chkValgusRight.Checked = node.SelectSingleNode("ValgusRight") == null ? chkValgusRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ValgusRight").InnerText);
            //   txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            //  txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;

            _fldPop = false;
        }
    }
    public void PopulateStrightFwd(bool bL, bool bR)
    {
        //bool bLeft = bL;
        //bool bRight = bR;
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLEFlexionRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLEExtensionRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLEFlexionLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLEExtensionLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal.Visibility = System.Windows.Visibility.Visible;
        //txtExtensionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFwdFlexNormal.Visibility = System.Windows.Visibility.Visible;

        //wrpLeft1.IsEnabled =
        //wrpLeft2.IsEnabled =
        //wrpLeft3.IsEnabled =
        //wrpLeft4.IsEnabled = bLeft;

        //wrpRight1.IsEnabled =
        //wrpRight3.IsEnabled =
        //wrpRight4.IsEnabled = bRight;

        //txtLEFlexionLeft.IsEnabled = bLeft;
        //txtLEExtensionLeft.IsEnabled = bLeft;
        //txtLEFlexionLeftWas.IsEnabled = bLeft;
        //txtLEExtensionLeftWas.IsEnabled = bLeft;

        //txtLEFlexionRight.IsEnabled = bRight;
        //txtLEExtensionRight.IsEnabled = bRight;
        //txtLEFlexionRightWas.IsEnabled = bRight;
        //txtLEExtensionRightWas.IsEnabled = bRight;

        //chkMcMurrayLeft.IsEnabled = bLeft;
        //chkLachmanLeft.IsEnabled = bLeft;
        //chkAnteriorLeft.IsEnabled = bLeft;
        //chkPosteriorLeft.IsEnabled = bLeft;
        //chkVarusLeft.IsEnabled = bLeft;
        //chkValgusLeft.IsEnabled = bLeft;

        //chkMcMurrayRight.IsEnabled = bRight;
        //chkLachmanRight.IsEnabled = bRight;
        //chkAnteriorRight.IsEnabled = bRight;
        //chkPosteriorRight.IsEnabled = bRight;
        //chkVarusRight.IsEnabled = bRight;
        //chkValgusRight.IsEnabled = bRight;

        //if (bLeft && bRight)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = 3;
        //else if (bLeft)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = 1;
        //else
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = 2;
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
        //SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
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

    private void LoadDV_Click(object sender, ImageClickEventArgs e)
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


        long _ieID = Convert.ToInt64(Session["PatientIE_ID"]);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpWrist WHERE PatientIE_ID = " + _ieID;
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

            DataTable newTable = new DataTable();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (Request.QueryString["P"].ToLower() == "r")
                    newTable = ds.Tables[0].Select(" Description like '%right%' ").CopyToDataTable();

                if (Request.QueryString["P"].ToLower() == "l")
                    newTable = ds.Tables[0].Select(" Description like '%left%' ").CopyToDataTable();

                if (Request.QueryString["P"].ToLower() == "b")
                    newTable = ds.Tables[0];


                DataView dv = newTable.DefaultView;
                dv.Sort = "Description asc";
                DataTable sortedDT = dv.ToTable();

                dgvDiagCodesPopup.DataSource = sortedDT;
                dgvDiagCodesPopup.DataBind();
            }

            if (newTable != null && newTable.Rows.Count > 0)
            {
                dgvDiagCodesPopup.DataSource = newTable;
                dgvDiagCodesPopup.DataBind();
            }
            else
            {
                dgvDiagCodesPopup.DataSource = ds;
                dgvDiagCodesPopup.DataBind();
            }
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
        string path = Server.MapPath("~/Template/WristCC.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");

        DefaultCCPEModel model = new DefaultCCPEModel();
        model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Left");
        body = body.Replace("#LCC", model.CC);
        model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Right");
        body = body.Replace("#RCC", model.CC);

        CF.InnerHtml = body;

    }

    public void bindPE(string p)
    {
        string path = Server.MapPath("~/Template/WristPE.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");


        DefaultCCPEModel model = new DefaultCCPEModel();
        model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Right");
        body = body.Replace("#RPE", model.PE);
        model = gDbhelperobj.getDefaultCCPEValues("Wrist", "Left");
        body = body.Replace("#LPE", model.PE);

        divPE.InnerHtml = body;


    }

}