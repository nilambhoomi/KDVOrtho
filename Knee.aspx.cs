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

public partial class Knee : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Knee";
    string Position = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();

    ILog log = log4net.LogManager.GetLogger(typeof(Knee));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        Session["PageName"] = "Knee";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            BindROM();
            if (Session["PatientIE_ID"] != null)
            {
                _CurIEid = Session["PatientIE_ID"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblbpKnee WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
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
                    bindPE();
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
                            //second div
                            //wrpPELeft.Visible = true;
                            //wrpPERight.Visible = false;
                            //Right
                            //txtLEFlexionLeft.ReadOnly = false;
                            //txtLEExtensionLeft.ReadOnly = false;
                            //txtLEFlexionRight.ReadOnly = true;
                            //txtLEExtensionRight.ReadOnly = true;
                            //Left checkbox
                            //chkMcMurrayLeft.Enabled = true;
                            //chkLachmanLeft.Enabled = true;
                            //chkAnteriorLeft.Enabled = true;
                            //chkPosteriorLeft.Enabled = true;
                            //chkVarusLeft.Enabled = true;
                            //chkValgusLeft.Enabled = true;
                            ////Right checkbox
                            //chkMcMurrayRight.Enabled = false;
                            //chkLachmanRight.Enabled = false;
                            //chkAnteriorRight.Enabled = false;
                            //chkPosteriorRight.Enabled = false;
                            //chkVarusRight.Enabled = false;
                            //chkValgusRight.Enabled = false;
                            break;
                        case "R":
                            //first div
                            //wrpRight.Visible = true;
                            //WrapLeft.Visible = false;
                            //second div
                            //wrpPELeft.Visible = false;
                            //wrpPERight.Visible = true;

                            //txtLEFlexionLeft.ReadOnly = true;
                            //txtLEExtensionLeft.ReadOnly = true;
                            //txtLEFlexionRight.ReadOnly = false;
                            //txtLEExtensionRight.ReadOnly = false;
                            //Left checkbox
                            //chkMcMurrayLeft.Enabled = false;
                            //chkLachmanLeft.Enabled = false;
                            //chkAnteriorLeft.Enabled = false;
                            //chkPosteriorLeft.Enabled = false;
                            //chkVarusLeft.Enabled = false;
                            //chkValgusLeft.Enabled = false;
                            ////Right checkbox
                            //chkMcMurrayRight.Enabled = true;
                            //chkLachmanRight.Enabled = true;
                            //chkAnteriorRight.Enabled = true;
                            //chkPosteriorRight.Enabled = true;
                            //chkVarusRight.Enabled = true;
                            //chkValgusRight.Enabled = true;
                            break;
                        case "B":
                            //first div
                            //wrpRight.Visible = true;
                            //WrapLeft.Visible = true;
                            //second div
                            //wrpPELeft.Visible = true;
                            //wrpPERight.Visible = true;
                            //txtLEFlexionLeft.ReadOnly = false;
                            //txtLEExtensionLeft.ReadOnly = false;
                            //txtLEFlexionRight.ReadOnly = false;
                            //txtLEExtensionRight.ReadOnly = false;
                            //Left checkbox
                            //chkMcMurrayLeft.Enabled = true;
                            //chkLachmanLeft.Enabled = true;
                            //chkAnteriorLeft.Enabled = true;
                            //chkPosteriorLeft.Enabled = true;
                            //chkVarusLeft.Enabled = true;
                            //chkValgusLeft.Enabled = true;
                            ////Right checkbox
                            //chkMcMurrayRight.Enabled = true;
                            //chkLachmanRight.Enabled = true;
                            //chkAnteriorRight.Enabled = true;
                            //chkPosteriorRight.Enabled = true;
                            //chkVarusRight.Enabled = true;
                            //chkValgusRight.Enabled = true;
                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
            bindgridPoup();
            BindDCDataGrid();
        }


        Logger.Info(Session["uname"].ToString() + "- Visited in  Knee for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    public string SaveUI(string ieID, string ieMode, bool bpIsChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpKnee WHERE PatientIE_ID = " + ieID;
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
            //TblRow["PainScaleLeft"] = txtPainScaleLeft.Text.ToString().ToString();
            //TblRow["ConstantRight"] = chkContentRight.Checked;
            //TblRow["ConstantLeft"] = chkContentLeft.Checked;
            //TblRow["IntermittentLeft"] = chkIntermittentLeft.Checked;
            //TblRow["IntermittentRight"] = chkIntermittentRight.Checked;
            //TblRow["SharpLeft"] = chkSharpLeft.Checked;
            //TblRow["ElectricLeft"] = chkElectricLeft.Checked;
            //TblRow["ShootingLeft"] = chkShootingLeft.Checked;
            //TblRow["ThrobblingLeft"] = chkThrobblingLeft.Checked;
            //TblRow["PulsatingLeft"] = chkPulsatingLeft.Checked;
            //TblRow["DullLeft"] = chkDullLeft.Checked;
            //TblRow["AchyLeft"] = chkAchyLeft.Checked;
            //TblRow["WorseMovementLeft"] = chkWorseMovementLeft.Checked;
            //TblRow["WorseWalkingLeft"] = chkWorseWalkingLeft.Checked;
            //TblRow["WorseStairsLeft"] = chkWorseStairsLeft.Checked;
            //TblRow["WorseSquattingLeft"] = chkWorseSquattingLeft.Checked;
            //TblRow["WorseActivitiesLeft"] = chkWorseActivitiesLeft.Checked;
            //TblRow["WorseOtherLeft"] = chkWorseOtherLeft.Checked;
            //TblRow["WorseOtherTextLeft"] = txtWorseOtherTextLeft.Text.ToString().ToString();
            //TblRow["ImprovedRestingLeft"] = chkImprovedRestingLeft.Checked;
            //TblRow["ImprovedMedicationLeft"] = chkImprovedMedicationLeft.Checked;
            //TblRow["ImprovedTherapyLeft"] = chkImprovedTherapyLeft.Checked;
            //TblRow["ImprovedSleepingLeft"] = chkImprovedSleepingLeft.Checked;
            //TblRow["PainScaleRight"] = txtPainScaleRight.Text.ToString().ToString();
            //TblRow["SharpRight"] = chkSharpRight.Checked;
            //TblRow["ElectricRight"] = chkElectricRight.Checked;
            //TblRow["ShootingRight"] = chkShootingRight.Checked;
            //TblRow["ThrobblingRight"] = chkThrobblingRight.Checked;
            //TblRow["PulsatingRight"] = chkPulsatingRight.Checked;
            //TblRow["DullRight"] = chkDullRight.Checked;
            //TblRow["AchyRight"] = chkAchyRight.Checked;
            //TblRow["WorseMovementRight"] = chkWorseMovementRight.Checked;
            //TblRow["WorseWalkingRight"] = chkWorseWalkingRight.Checked;
            //TblRow["WorseStairsRight"] = chkWorseStairsRight.Checked;
            //TblRow["WorseSquattingRight"] = chkWorseSquattingRight.Checked;
            //TblRow["WorseActivitiesRight"] = chkWorseActivitiesRight.Checked;
            //TblRow["WorseOtherRight"] = chkWorseOtherRight.Checked;
            //TblRow["WorseOtherTextRight"] = txtWorseOtherTextRight.Text.ToString().ToString();
            //TblRow["ImprovedRestingRight"] = chkImprovedRestingRight.Checked;
            //TblRow["ImprovedMedicationRight"] = chkImprovedMedicationRight.Checked;
            //TblRow["ImprovedTherapyRight"] = chkImprovedTherapyRight.Checked;
            //TblRow["ImprovedSleepingRight"] = chkImprovedSleepingRight.Checked;
            //TblRow["LEExtensionRight"] = txtLEExtensionRight.Text.ToString().ToString();
            //TblRow["LEFlexionRight"] = txtLEFlexionRight.Text.ToString().ToString();

            //TblRow["FlexNormal"] = txtFwdFlexNormal.Text.ToString().ToString();
            //TblRow["ExtensionNormal"] = txtExtensionNormal.Text.ToString().ToString();

            //TblRow["LEExtensionLeft"] = txtLEExtensionLeft.Text.ToString().ToString();
            //TblRow["LEFlexionLeft"] = txtLEFlexionLeft.Text.ToString().ToString();
            //TblRow["PalpationText1Left"] = txtPalpationText1Left.Text.ToString().ToString();
            //TblRow["PalpationText2Left"] = txtPalpationText2Left.Text.ToString().ToString();
            //TblRow["MedialLeft"] = chkMedialLeft.Checked;
            //TblRow["LateralLeft"] = chkLateralLeft.Checked;
            //TblRow["SuperiorLeft"] = chkSuperiorLeft.Checked;
            //TblRow["InferiorLeft"] = chkInferiorLeft.Checked;
            //TblRow["SupermedialLeft"] = chkSupermedialLeft.Checked;
            //TblRow["SuperoLateralLeft"] = chkSuperoLateralLeft.Checked;
            //TblRow["InferomedialLeft"] = chkInferomedialLeft.Checked;
            //TblRow["InferoLateralLeft"] = chkInferoLateralLeft.Checked;
            //TblRow["PeripatellarLeft"] = chkPeripatellarLeft.Checked;
            //TblRow["PalpationText1Right"] = txtPalpationText1Right.Text.ToString().ToString();
            //TblRow["PalpationText2Right"] = txtPalpationText2Right.Text.ToString().ToString();
            //TblRow["MedialRight"] = chkMedialRight.Checked;
            //TblRow["LateralRight"] = chkLateralRight.Checked;
            //TblRow["SuperiorRight"] = chkSuperiorRight.Checked;
            //TblRow["InferiorRight"] = chkInferiorRight.Checked;
            //TblRow["SupermedialRight"] = chkSupermedialRight.Checked;
            //TblRow["SuperoLateralRight"] = chkSuperoLateralRight.Checked;
            //TblRow["InferomedialRight"] = chkInferomedialRight.Checked;
            //TblRow["InferoLateralRight"] = chkInferoLateralRight.Checked;
            //TblRow["PeripatellarRight"] = chkPeripatellarRight.Checked;
            //TblRow["McMurrayLeft"] = chkMcMurrayLeft.Checked;
            //TblRow["LachmanLeft"] = chkLachmanLeft.Checked;
            //TblRow["AnteriorLeft"] = chkAnteriorLeft.Checked;
            //TblRow["PosteriorLeft"] = chkPosteriorLeft.Checked;
            //TblRow["VarusLeft"] = chkVarusLeft.Checked;
            //TblRow["ValgusLeft"] = chkValgusLeft.Checked;
            //TblRow["McMurrayRight"] = chkMcMurrayRight.Checked;
            //TblRow["LachmanRight"] = chkLachmanRight.Checked;
            //TblRow["AnteriorRight"] = chkAnteriorRight.Checked;
            //TblRow["PosteriorRight"] = chkPosteriorRight.Checked;
            //TblRow["VarusRight"] = chkVarusRight.Checked;
            //TblRow["ValgusRight"] = chkValgusRight.Checked;
            TblRow["FreeForm"] = txtFreeForm.Text.ToString().ToString();
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
            return "Knee has been added...";
        else if (_ieMode == "Update")
            return "Knee has been updated...";
        else if (_ieMode == "Delete")
            return "Knee has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string ieID)
    {


        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpKnee WHERE PatientIE_ID = " + ieID;
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
            //chkContentLeft.Checked = CommonConvert.ToBoolean(TblRow["ConstantLeft"].ToString());
            //chkIntermittentLeft.Checked = CommonConvert.ToBoolean(TblRow["IntermittentLeft"].ToString());
            //chkIntermittentRight.Checked = CommonConvert.ToBoolean(TblRow["IntermittentRight"].ToString());
            //txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString().Trim();
            //chkSharpLeft.Checked = CommonConvert.ToBoolean(TblRow["SharpLeft"].ToString());
            //chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            //chkShootingLeft.Checked = CommonConvert.ToBoolean(TblRow["ShootingLeft"].ToString());
            //chkThrobblingLeft.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingLeft"].ToString());
            //chkPulsatingLeft.Checked = CommonConvert.ToBoolean(TblRow["PulsatingLeft"].ToString());
            //chkDullLeft.Checked = CommonConvert.ToBoolean(TblRow["DullLeft"].ToString());
            //chkAchyLeft.Checked = CommonConvert.ToBoolean(TblRow["AchyLeft"].ToString());
            //chkWorseMovementLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementLeft"].ToString());
            //chkWorseWalkingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseWalkingLeft"].ToString());
            //chkWorseStairsLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseStairsLeft"].ToString());
            //chkWorseSquattingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseSquattingLeft"].ToString());
            //chkWorseActivitiesLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesLeft"].ToString());
            //chkWorseOtherLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseOtherLeft"].ToString());
            //txtWorseOtherTextLeft.Text = TblRow["WorseOtherTextLeft"].ToString().Trim();
            //chkImprovedRestingLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedRestingLeft"].ToString());
            //chkImprovedMedicationLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedicationLeft"].ToString());
            //chkImprovedTherapyLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapyLeft"].ToString());
            //chkImprovedSleepingLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleepingLeft"].ToString());
            //txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString().Trim();
            //chkSharpRight.Checked = CommonConvert.ToBoolean(TblRow["SharpRight"].ToString());
            //chkElectricRight.Checked = CommonConvert.ToBoolean(TblRow["ElectricRight"].ToString());
            //chkShootingRight.Checked = CommonConvert.ToBoolean(TblRow["ShootingRight"].ToString());
            //chkThrobblingRight.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingRight"].ToString());
            //chkPulsatingRight.Checked = CommonConvert.ToBoolean(TblRow["PulsatingRight"].ToString());
            //chkDullRight.Checked = CommonConvert.ToBoolean(TblRow["DullRight"].ToString());
            //chkAchyRight.Checked = CommonConvert.ToBoolean(TblRow["AchyRight"].ToString());
            //chkWorseMovementRight.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementRight"].ToString());
            //chkWorseWalkingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseWalkingRight"].ToString());
            //chkWorseStairsRight.Checked = CommonConvert.ToBoolean(TblRow["WorseStairsRight"].ToString());
            //chkWorseSquattingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseSquattingRight"].ToString());
            //chkWorseActivitiesRight.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesRight"].ToString());
            //chkWorseOtherRight.Checked = CommonConvert.ToBoolean(TblRow["WorseOtherRight"].ToString());
            //txtWorseOtherTextRight.Text = TblRow["WorseOtherTextRight"].ToString().Trim();
            //chkImprovedRestingRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedRestingRight"].ToString());
            //chkImprovedMedicationRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedicationRight"].ToString());
            //chkImprovedTherapyRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapyRight"].ToString());
            //chkImprovedSleepingRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleepingRight"].ToString());
            //txtLEExtensionRight.Text = TblRow["LEExtensionRight"].ToString().Trim();
            //txtLEFlexionRight.Text = TblRow["LEFlexionRight"].ToString().Trim();
            //txtLEExtensionLeft.Text = TblRow["LEExtensionLeft"].ToString().Trim();
            //txtLEFlexionLeft.Text = TblRow["LEFlexionLeft"].ToString().Trim();

            //txtFwdFlexNormal.Text = TblRow["FlexNormal"].ToString().Trim();
            //txtExtensionNormal.Text = TblRow["ExtensionNormal"].ToString().Trim();

            //txtLEExtensionRightWas.Text = TblRow["LEExtensionRight"].ToString().Trim();
            //txtLEFlexionRightWas.Text = TblRow["LEFlexionRight"].ToString().Trim();
            //txtLEExtensionLeftWas.Text = TblRow["LEExtensionLeft"].ToString().Trim();
            //txtLEFlexionLeftWas.Text = TblRow["LEFlexionLeft"].ToString().Trim();
            //txtPalpationText1Left.Text = TblRow["PalpationText1Left"].ToString().Trim();
            //txtPalpationText2Left.Text = TblRow["PalpationText2Left"].ToString().Trim();
            //chkMedialLeft.Checked = CommonConvert.ToBoolean(TblRow["MedialLeft"].ToString());
            //chkLateralLeft.Checked = CommonConvert.ToBoolean(TblRow["LateralLeft"].ToString());
            //chkSuperiorLeft.Checked = CommonConvert.ToBoolean(TblRow["SuperiorLeft"].ToString());
            //chkInferiorLeft.Checked = CommonConvert.ToBoolean(TblRow["InferiorLeft"].ToString());
            //chkSupermedialLeft.Checked = CommonConvert.ToBoolean(TblRow["SupermedialLeft"].ToString());
            //chkSuperoLateralLeft.Checked = CommonConvert.ToBoolean(TblRow["SuperoLateralLeft"].ToString());
            //chkInferomedialLeft.Checked = CommonConvert.ToBoolean(TblRow["InferomedialLeft"].ToString());
            //chkInferoLateralLeft.Checked = CommonConvert.ToBoolean(TblRow["InferoLateralLeft"].ToString());
            //chkPeripatellarLeft.Checked = CommonConvert.ToBoolean(TblRow["PeripatellarLeft"].ToString());
            //txtPalpationText1Right.Text = TblRow["PalpationText1Right"].ToString().Trim();
            //txtPalpationText2Right.Text = TblRow["PalpationText2Right"].ToString().Trim();
            //chkMedialRight.Checked = CommonConvert.ToBoolean(TblRow["MedialRight"].ToString());
            //chkLateralRight.Checked = CommonConvert.ToBoolean(TblRow["LateralRight"].ToString());
            //chkSuperiorRight.Checked = CommonConvert.ToBoolean(TblRow["SuperiorRight"].ToString());
            //chkInferiorRight.Checked = CommonConvert.ToBoolean(TblRow["InferiorRight"].ToString());
            //chkSupermedialRight.Checked = CommonConvert.ToBoolean(TblRow["SupermedialRight"].ToString());
            //chkSuperoLateralRight.Checked = CommonConvert.ToBoolean(TblRow["SuperoLateralRight"].ToString());
            //chkInferomedialRight.Checked = CommonConvert.ToBoolean(TblRow["InferomedialRight"].ToString());
            //chkInferoLateralRight.Checked = CommonConvert.ToBoolean(TblRow["InferoLateralRight"].ToString());
            //chkPeripatellarRight.Checked = CommonConvert.ToBoolean(TblRow["PeripatellarRight"].ToString());
            //chkMcMurrayLeft.Checked = CommonConvert.ToBoolean(TblRow["McMurrayLeft"].ToString());
            //chkLachmanLeft.Checked = CommonConvert.ToBoolean(TblRow["LachmanLeft"].ToString());
            //chkAnteriorLeft.Checked = CommonConvert.ToBoolean(TblRow["AnteriorLeft"].ToString());
            //chkPosteriorLeft.Checked = CommonConvert.ToBoolean(TblRow["PosteriorLeft"].ToString());
            //chkVarusLeft.Checked = CommonConvert.ToBoolean(TblRow["VarusLeft"].ToString());
            //chkValgusLeft.Checked = CommonConvert.ToBoolean(TblRow["ValgusLeft"].ToString());
            //chkMcMurrayRight.Checked = CommonConvert.ToBoolean(TblRow["McMurrayRight"].ToString());
            //chkLachmanRight.Checked = CommonConvert.ToBoolean(TblRow["LachmanRight"].ToString());
            //chkAnteriorRight.Checked = CommonConvert.ToBoolean(TblRow["AnteriorRight"].ToString());
            //chkPosteriorRight.Checked = CommonConvert.ToBoolean(TblRow["PosteriorRight"].ToString());
            //chkVarusRight.Checked = CommonConvert.ToBoolean(TblRow["VarusRight"].ToString());
            //chkValgusRight.Checked = CommonConvert.ToBoolean(TblRow["ValgusRight"].ToString());
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

            CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();



            divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();




            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ",'" + pos + "')", true);

            string pos = Request.QueryString["P"];

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP('" + pos + "');", true);

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Knee");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            //txtPainScaleLeft.Text = node.SelectSingleNode("PainScaleLeft") == null ? txtPainScaleLeft.Text.ToString().Trim() : node.SelectSingleNode("PainScaleLeft").InnerText;
            //chkSharpLeft.Checked = node.SelectSingleNode("SharpLeft") == null ? chkSharpLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SharpLeft").InnerText);
            //chkElectricLeft.Checked = node.SelectSingleNode("ElectricLeft") == null ? chkElectricLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ElectricLeft").InnerText);
            //chkShootingLeft.Checked = node.SelectSingleNode("ShootingLeft") == null ? chkShootingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ShootingLeft").InnerText);
            //chkThrobblingLeft.Checked = node.SelectSingleNode("ThrobblingLeft") == null ? chkThrobblingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ThrobblingLeft").InnerText);
            //chkPulsatingLeft.Checked = node.SelectSingleNode("PulsatingLeft") == null ? chkPulsatingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PulsatingLeft").InnerText);
            //chkDullLeft.Checked = node.SelectSingleNode("DullLeft") == null ? chkDullLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DullLeft").InnerText);
            //chkAchyLeft.Checked = node.SelectSingleNode("AchyLeft") == null ? chkAchyLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AchyLeft").InnerText);
            //chkWorseMovementLeft.Checked = node.SelectSingleNode("WorseMovementLeft") == null ? chkWorseMovementLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseMovementLeft").InnerText);
            //chkWorseWalkingLeft.Checked = node.SelectSingleNode("WorseWalkingLeft") == null ? chkWorseWalkingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseWalkingLeft").InnerText);
            //chkWorseStairsLeft.Checked = node.SelectSingleNode("WorseStairsLeft") == null ? chkWorseStairsLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseStairsLeft").InnerText);
            //chkWorseSquattingLeft.Checked = node.SelectSingleNode("WorseSquattingLeft") == null ? chkWorseSquattingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseSquattingLeft").InnerText);
            //chkWorseActivitiesLeft.Checked = node.SelectSingleNode("WorseActivitiesLeft") == null ? chkWorseActivitiesLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseActivitiesLeft").InnerText);
            //chkWorseOtherLeft.Checked = node.SelectSingleNode("WorseOtherLeft") == null ? chkWorseOtherLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseOtherLeft").InnerText);
            //txtWorseOtherTextLeft.Text = node.SelectSingleNode("WorseOtherTextLeft") == null ? txtWorseOtherTextLeft.Text.ToString().Trim() : node.SelectSingleNode("WorseOtherTextLeft").InnerText;
            //chkImprovedRestingLeft.Checked = node.SelectSingleNode("ImprovedRestingLeft") == null ? chkImprovedRestingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedRestingLeft").InnerText);
            //chkImprovedMedicationLeft.Checked = node.SelectSingleNode("ImprovedMedicationLeft") == null ? chkImprovedMedicationLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
            //chkImprovedTherapyLeft.Checked = node.SelectSingleNode("ImprovedTherapyLeft") == null ? chkImprovedTherapyLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedTherapyLeft").InnerText);
            //chkImprovedSleepingLeft.Checked = node.SelectSingleNode("ImprovedSleepingLeft") == null ? chkImprovedSleepingLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedSleepingLeft").InnerText);
            //txtPainScaleRight.Text = node.SelectSingleNode("PainScaleRight") == null ? txtPainScaleRight.Text.ToString().Trim() : node.SelectSingleNode("PainScaleRight").InnerText;
            //chkSharpRight.Checked = node.SelectSingleNode("SharpRight") == null ? chkSharpRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SharpRight").InnerText);
            //chkElectricRight.Checked = node.SelectSingleNode("ElectricRight") == null ? chkElectricRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ElectricRight").InnerText);
            //chkShootingRight.Checked = node.SelectSingleNode("ShootingRight") == null ? chkShootingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ShootingRight").InnerText);
            //chkThrobblingRight.Checked = node.SelectSingleNode("ThrobblingRight") == null ? chkThrobblingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ThrobblingRight").InnerText);
            //chkPulsatingRight.Checked = node.SelectSingleNode("PulsatingRight") == null ? chkPulsatingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PulsatingRight").InnerText);
            //chkDullRight.Checked = node.SelectSingleNode("DullRight") == null ? chkDullRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DullRight").InnerText);
            //chkAchyRight.Checked = node.SelectSingleNode("AchyRight") == null ? chkAchyRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AchyRight").InnerText);
            //chkWorseMovementRight.Checked = node.SelectSingleNode("WorseMovementRight") == null ? chkWorseMovementRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseMovementRight").InnerText);
            //chkWorseWalkingRight.Checked = node.SelectSingleNode("WorseWalkingRight") == null ? chkWorseWalkingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseWalkingRight").InnerText);
            //chkWorseStairsRight.Checked = node.SelectSingleNode("WorseStairsRight") == null ? chkWorseStairsRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseStairsRight").InnerText);
            //chkWorseSquattingRight.Checked = node.SelectSingleNode("WorseSquattingRight") == null ? chkWorseSquattingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseSquattingRight").InnerText);
            //chkWorseActivitiesRight.Checked = node.SelectSingleNode("WorseActivitiesRight") == null ? chkWorseActivitiesRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseActivitiesRight").InnerText);
            //chkWorseOtherRight.Checked = node.SelectSingleNode("WorseOtherRight") == null ? chkWorseOtherRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WorseOtherRight").InnerText);
            //txtWorseOtherTextRight.Text = node.SelectSingleNode("WorseOtherTextRight") == null ? txtWorseOtherTextRight.Text.ToString().Trim() : node.SelectSingleNode("WorseOtherTextRight").InnerText;
            //chkImprovedRestingRight.Checked = node.SelectSingleNode("ImprovedRestingRight") == null ? chkImprovedRestingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedRestingRight").InnerText);
            //chkImprovedMedicationRight.Checked = node.SelectSingleNode("ImprovedMedicationRight") == null ? chkImprovedMedicationRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedMedicationRight").InnerText);
            //chkImprovedTherapyRight.Checked = node.SelectSingleNode("ImprovedTherapyRight") == null ? chkImprovedTherapyRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedTherapyRight").InnerText);
            //chkImprovedSleepingRight.Checked = node.SelectSingleNode("ImprovedSleepingRight") == null ? chkImprovedSleepingRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ImprovedSleepingRight").InnerText);

            //txtFwdFlexNormal.Text = node.SelectSingleNode("KneeFlexNormal") == null ? txtFwdFlexNormal.Text.ToString().Trim() : node.SelectSingleNode("KneeFlexNormal").InnerText;

            //txtExtensionNormal.Text = node.SelectSingleNode("KneeExtNormal") == null ? txtExtensionNormal.Text.ToString().Trim() : node.SelectSingleNode("KneeExtNormal").InnerText;

            //txtLEExtensionRight.Text = node.SelectSingleNode("LEExtensionRight") == null ? txtLEExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionRight").InnerText;
            //txtLEFlexionRight.Text = node.SelectSingleNode("LEFlexionRight") == null ? txtLEFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionRight").InnerText;
            //txtLEExtensionLeft.Text = node.SelectSingleNode("LEExtensionLeft") == null ? txtLEExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionLeft").InnerText;
            //txtLEFlexionLeft.Text = node.SelectSingleNode("LEFlexionLeft") == null ? txtLEFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionLeft").InnerText;
            //txtLEExtensionRightWas.Text = node.SelectSingleNode("LEExtensionRight") == null ? txtLEExtensionRightWas.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionRight").InnerText;
            //txtLEFlexionRightWas.Text = node.SelectSingleNode("LEFlexionRight") == null ? txtLEFlexionRightWas.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionRight").InnerText;
            //txtLEExtensionLeftWas.Text = node.SelectSingleNode("LEExtensionLeft") == null ? txtLEExtensionLeftWas.Text.ToString().Trim() : node.SelectSingleNode("LEExtensionLeft").InnerText;
            //txtLEFlexionLeftWas.Text = node.SelectSingleNode("LEFlexionLeft") == null ? txtLEFlexionLeftWas.Text.ToString().Trim() : node.SelectSingleNode("LEFlexionLeft").InnerText;
            //txtPalpationText1Left.Text = node.SelectSingleNode("PalpationText1Left") == null ? txtPalpationText1Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Left").InnerText;
            //txtPalpationText2Left.Text = node.SelectSingleNode("PalpationText2Left") == null ? txtPalpationText2Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Left").InnerText;
            //chkMedialLeft.Checked = node.SelectSingleNode("MedialLeft") == null ? chkMedialLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("MedialLeft").InnerText);
            //chkLateralLeft.Checked = node.SelectSingleNode("LateralLeft") == null ? chkLateralLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("LateralLeft").InnerText);
            //chkSuperiorLeft.Checked = node.SelectSingleNode("SuperiorLeft") == null ? chkSuperiorLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SuperiorLeft").InnerText);
            //chkInferiorLeft.Checked = node.SelectSingleNode("InferiorLeft") == null ? chkInferiorLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("InferiorLeft").InnerText);
            //chkSupermedialLeft.Checked = node.SelectSingleNode("SupermedialLeft") == null ? chkSupermedialLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SupermedialLeft").InnerText);
            //chkSuperoLateralLeft.Checked = node.SelectSingleNode("SuperoLateralLeft") == null ? chkSuperoLateralLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SuperoLateralLeft").InnerText);
            //chkInferomedialLeft.Checked = node.SelectSingleNode("InferomedialLeft") == null ? chkInferomedialLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("InferomedialLeft").InnerText);
            //chkInferoLateralLeft.Checked = node.SelectSingleNode("InferoLateralLeft") == null ? chkInferoLateralLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("InferoLateralLeft").InnerText);
            //chkPeripatellarLeft.Checked = node.SelectSingleNode("PeripatellarLeft") == null ? chkPeripatellarLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PeripatellarLeft").InnerText);
            //txtPalpationText1Right.Text = node.SelectSingleNode("PalpationText1Right") == null ? txtPalpationText1Right.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Right").InnerText;
            //txtPalpationText2Right.Text = node.SelectSingleNode("PalpationText2Right") == null ? txtPalpationText2Right.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Right").InnerText;
            //chkMedialRight.Checked = node.SelectSingleNode("MedialRight") == null ? chkMedialRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("MedialRight").InnerText);
            //chkLateralRight.Checked = node.SelectSingleNode("LateralRight") == null ? chkLateralRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("LateralRight").InnerText);
            //chkSuperiorRight.Checked = node.SelectSingleNode("SuperiorRight") == null ? chkSuperiorRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SuperiorRight").InnerText);
            //chkInferiorRight.Checked = node.SelectSingleNode("InferiorRight") == null ? chkInferiorRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("InferiorRight").InnerText);
            //chkSupermedialRight.Checked = node.SelectSingleNode("SupermedialRight") == null ? chkSupermedialRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SupermedialRight").InnerText);
            //chkSuperoLateralRight.Checked = node.SelectSingleNode("SuperoLateralRight") == null ? chkSuperoLateralRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("SuperoLateralRight").InnerText);
            //chkInferomedialRight.Checked = node.SelectSingleNode("InferomedialRight") == null ? chkInferomedialRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("InferomedialRight").InnerText);
            //chkInferoLateralRight.Checked = node.SelectSingleNode("InferoLateralRight") == null ? chkInferoLateralRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("InferoLateralRight").InnerText);
            //chkPeripatellarRight.Checked = node.SelectSingleNode("PeripatellarRight") == null ? chkPeripatellarRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PeripatellarRight").InnerText);
            //chkMcMurrayLeft.Checked = node.SelectSingleNode("McMurrayLeft") == null ? chkMcMurrayLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("McMurrayLeft").InnerText);
            //chkLachmanLeft.Checked = node.SelectSingleNode("LachmanLeft") == null ? chkLachmanLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("LachmanLeft").InnerText);
            //chkAnteriorLeft.Checked = node.SelectSingleNode("AnteriorLeft") == null ? chkAnteriorLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AnteriorLeft").InnerText);
            //chkPosteriorLeft.Checked = node.SelectSingleNode("PosteriorLeft") == null ? chkPosteriorLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PosteriorLeft").InnerText);
            //chkVarusLeft.Checked = node.SelectSingleNode("VarusLeft") == null ? chkVarusLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("VarusLeft").InnerText);
            //chkValgusLeft.Checked = node.SelectSingleNode("ValgusLeft") == null ? chkValgusLeft.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ValgusLeft").InnerText);
            //chkMcMurrayRight.Checked = node.SelectSingleNode("McMurrayRight") == null ? chkMcMurrayRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("McMurrayRight").InnerText);
            //chkLachmanRight.Checked = node.SelectSingleNode("LachmanRight") == null ? chkLachmanRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("LachmanRight").InnerText);
            //chkAnteriorRight.Checked = node.SelectSingleNode("AnteriorRight") == null ? chkAnteriorRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AnteriorRight").InnerText);
            //chkPosteriorRight.Checked = node.SelectSingleNode("PosteriorRight") == null ? chkPosteriorRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PosteriorRight").InnerText);
            //chkVarusRight.Checked = node.SelectSingleNode("VarusRight") == null ? chkVarusRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("VarusRight").InnerText);
            //chkValgusRight.Checked = node.SelectSingleNode("ValgusRight") == null ? chkValgusRight.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ValgusRight").InnerText);
            txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
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
        _CurIEid = Session["PatientIE_ID"].ToString();
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
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientIE_ID = " + _CurIEid + " and PatientFU_ID is null AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart";
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

    private void bindgridPoup()
    {
        try
        {
            _CurIEid = Session["PatientIE_ID"].ToString();
            string _CurBodyPart = _CurBP;
            string _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";
            if (_CurIEid != "")
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS(" + _CurIEid + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By TRIM(Description)";
            else
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By TRIM(Description)";
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

    public void SaveStdUI(string ieID, string iDiagID, bool DiagIsChecked, string bp, string dcd, string dc)
    {
        try
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
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindgridPoup();
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
        string path = Server.MapPath("~/Template/KneeCC.html");
        string body = File.ReadAllText(path);

        DefaultCCPEModel model = new DefaultCCPEModel();

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display: none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display: none'");

        var gender = Session["Gender"].ToString().ToLower() == "mr." ? "He" : "She";

        body = body.Replace("#gender", gender);

        model = gDbhelperobj.getDefaultCCPEValues("Knee", "Left");
        body = body.Replace("#LCC", model.CC);
        model = gDbhelperobj.getDefaultCCPEValues("Knee", "Right");
        body = body.Replace("#RCC", model.CC);


        CF.InnerHtml = body;

    }

    public void bindPE()
    {
        string path = Server.MapPath("~/Template/KneePE.html");
        string body = File.ReadAllText(path);
        DefaultCCPEModel model = new DefaultCCPEModel();
        string p = Request.QueryString["P"];

        if (p == "L")
        {
            body = body.Replace("#rigthtdiv", "style='display:none'");

        }
        else if (p == "R")
        {
            body = body.Replace("#leftdiv", "style='display:none'");

        }

        model = gDbhelperobj.getDefaultCCPEValues("Knee", "Right");
        body = body.Replace("#RPE", model.PE);
        model = gDbhelperobj.getDefaultCCPEValues("Knee", "Left");
        body = body.Replace("#LPE", model.PE);

        divPE.InnerHtml = body;

       

    }

    public int checkTP()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        int val = 0;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        // cboTPSide1.DataBind();
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Knee");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            if (isTP == false)
                val = 0;
            else
                val = 1;

        }

        return val;

    }

    protected void BindROM()
    {


        long _ieID = Convert.ToInt64(Session["PatientIE_ID"]);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpKnee WHERE PatientIE_ID = " + _ieID;
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
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Knee.xml"));
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
}