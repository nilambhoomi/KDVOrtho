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
using System.Globalization;
using log4net;

public partial class Page5 : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Page5";
    ILog log = log4net.LogManager.GetLogger(typeof(Page5));

    DBHelperClass gDbhelperobj = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Page5";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            _CurIEid = Session["PatientIE_ID"].ToString();

            if (Session["PatientIE_ID"] != null)
            {
                BindDataGrid();

                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblPatientIEDetailPage3 WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString() + "");
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
                }
                else
                {
                    PopulateUI(_CurIEid);

                }
            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  Page4 for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    public string SaveUI(string ieID, string ieMode, bool bpChecked)
    {
        try
        {
            DateTime result;

            long _ieID = Convert.ToInt64(ieID);
            string _ieMode = "";
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + ieID;
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count == 0)
                _ieMode = "New";
            else if (sqlTbl.Rows.Count > 0)
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
                TblRow["PatientIE_ID"] = _ieID;
                //TblRow["GAIT"] = cboGAIT.Text.ToString();
                //TblRow["Ambulates"] = cboAmbulates.Text.ToString();
                //TblRow["Footslap"] = chkFootslap.Checked;
                //TblRow["Kneehyperextension"] = false;
                //TblRow["Unabletohealwalk"] = chkUnabletohealwalk.Checked;
                //TblRow["Unabletotoewalk"] = chkUnabletotoewalk.Checked;
                //TblRow["Other"] = txtOther.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagCervialBulgeDate.Text))
                { TblRow["DiagCervialBulgeDate"] = DateTime.ParseExact(dtpDiagCervialBulgeDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagCervialBulgeDate"] = DBNull.Value;
                TblRow["DiagCervialBulgeStudy"] = cboDiagCervialBulgeStudy.Text.ToString();
                TblRow["DiagCervialBulge"] = chkDiagCervialBulge.Checked;
                TblRow["DiagCervialBulgeText"] = txtDiagCervialBulgeText.Text.ToString();
                TblRow["DiagCervialBulgeHNP1"] = txtDiagCervialBulgeHNP1.Text.ToString();
                TblRow["DiagCervialBulgeHNP2"] = txtDiagCervialBulgeHNP2.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagThoracicBulgeDate.Text))
                { TblRow["DiagThoracicBulgeDate"] = DateTime.ParseExact(dtpDiagThoracicBulgeDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagThoracicBulgeDate"] = DBNull.Value;
                TblRow["DiagThoracicBulgeStudy"] = cboDiagThoracicBulgeStudy.Text.ToString();
                TblRow["DiagThoracicBulge"] = chkDiagThoracicBulge.Checked;
                TblRow["DiagThoracicBulgeText"] = txtDiagThoracicBulgeText.Text.ToString();
                TblRow["DiagThoracicBulgeHNP1"] = txtDiagThoracicBulgeHNP1.Text.ToString();
                TblRow["DiagThoracicBulgeHNP2"] = txtDiagThoracicBulgeHNP2.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagLumberBulgeDate.Text))
                { TblRow["DiagLumberBulgeDate"] = DateTime.ParseExact(dtpDiagLumberBulgeDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagLumberBulgeDate"] = DBNull.Value;
                TblRow["DiagLumberBulgeStudy"] = cboDiagLumberBulgeStudy.Text.ToString();
                TblRow["DiagLumberBulge"] = chkDiagLumberBulge.Checked;
                TblRow["DiagLumberBulgeText"] = txtDiagLumberBulgeText.Text.ToString();
                TblRow["DiagLumberBulgeHNP1"] = txtDiagLumberBulgeHNP1.Text.ToString();
                TblRow["DiagLumberBulgeHNP2"] = txtDiagLumberBulgeHNP2.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagLeftShoulderDate.Text))
                { TblRow["DiagLeftShoulderDate"] = DateTime.ParseExact(dtpDiagLeftShoulderDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagLeftShoulderDate"] = DBNull.Value;
                TblRow["DiagLeftShoulderStudy"] = cboDiagLeftShoulderStudy.Text.ToString();
                TblRow["DiagLeftShoulder"] = chkDiagLeftShoulder.Checked;
                TblRow["DiagLeftShoulderText"] = txtDiagLeftShoulderText.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagRightShoulderDate.Text))
                { TblRow["DiagRightShoulderDate"] = DateTime.ParseExact(dtpDiagRightShoulderDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagRightShoulderDate"] = DBNull.Value;
                TblRow["DiagRightShoulderStudy"] = cboDiagRightShoulderStudy.Text.ToString();
                TblRow["DiagRightShoulder"] = chkDiagRightShoulder.Checked;
                TblRow["DiagRightShoulderText"] = txtDiagRightShoulderText.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagLeftKneeDate.Text))
                { TblRow["DiagLeftKneeDate"] = DateTime.ParseExact(dtpDiagLeftKneeDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagLeftKneeDate"] = DBNull.Value;
                TblRow["DiagLeftKneeStudy"] = cboDiagLeftKneeStudy.Text.ToString();
                TblRow["DiagLeftKnee"] = chkDiagLeftKnee.Checked;
                TblRow["DiagLeftKneeText"] = txtDiagLeftKneeText.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpDiagRightKneeDate.Text))
                { TblRow["DiagRightKneeDate"] = DateTime.ParseExact(dtpDiagRightKneeDate.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["DiagRightKneeDate"] = DBNull.Value;
                TblRow["DiagRightKneeStudy"] = cboDiagRightKneeStudy.Text.ToString();
                TblRow["DiagRightKnee"] = chkDiagRightKnee.Checked;
                TblRow["DiagRightKneeText"] = txtDiagRightKneeText.Text.ToString();
                //if (dtpDiagBrainDate.Text != null)
                //    TblRow["DiagBrainDate"] = Convert.ToDateTime(dtpDiagBrainDate.Text);
                //else
                //    TblRow["DiagBrainDate"] = DBNull.Value;
                //TblRow["DiagBrainStudy"] = cboDiagBrainStudy.Text.ToString();
                //TblRow["DiagBrain"] = chkDiagBrain.Checked;
                //  TblRow["DiagBrainText"] = txtDiagBrainText.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther1Date.Text))
                { TblRow["Other1Date"] = Convert.ToDateTime(dtpOther1Date.Text); }
                else
                    TblRow["Other1Date"] = DBNull.Value;
                TblRow["Other1Study"] = cboOther1Study.Text.ToString();
                TblRow["Other1Text"] = txtOther1Text.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther2Date.Text))
                { TblRow["Other2Date"] = DateTime.ParseExact(dtpOther2Date.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["Other2Date"] = DBNull.Value;
                TblRow["Other2Study"] = cboOther2Study.Text.ToString();
                TblRow["Other2Text"] = txtOther2Text.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther3Date.Text))
                { TblRow["Other3Date"] = DateTime.ParseExact(dtpOther3Date.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["Other3Date"] = DBNull.Value;
                TblRow["Other3Study"] = cboOther3Study.Text.ToString();
                TblRow["Other3Text"] = txtOther3Text.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther4Date.Text))
                { TblRow["Other4Date"] = DateTime.ParseExact(dtpOther4Date.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["Other4Date"] = DBNull.Value;
                TblRow["Other4Study"] = cboOther4Study.Text.ToString();
                TblRow["Other4Text"] = txtOther4Text.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther5Date.Text))
                { TblRow["Other5Date"] = DateTime.ParseExact(dtpOther5Date.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["Other5Date"] = DBNull.Value;
                TblRow["Other5Study"] = cboOther5Study.Text.ToString();
                TblRow["Other5Text"] = txtOther5Text.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther6Date.Text))
                { TblRow["Other6Date"] = DateTime.ParseExact(dtpOther6Date.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["Other6Date"] = DBNull.Value;
                TblRow["Other6Study"] = cboOther6Study.Text.ToString();
                TblRow["Other6Text"] = txtOther6Text.Text.ToString();
                if (!string.IsNullOrWhiteSpace(dtpOther7Date.Text))
                { TblRow["Other7Date"] = DateTime.ParseExact(dtpOther7Date.Text, "MM/dd/yyyy", null); }
                else
                    TblRow["Other7Date"] = DBNull.Value;
                TblRow["Other7Study"] = cboOther7Study.Text.ToString();
                TblRow["Other7Text"] = txtOther7Text.Text.ToString();
                TblRow["Procedures"] = chkProcedures.Checked;
                TblRow["Acupuncture"] = chkAcupuncture.Checked;
                TblRow["Chiropratic"] = chkChiropratic.Checked;
                TblRow["PhysicalTherapy"] = chkPhysicalTherapy.Checked;
                TblRow["AvoidHeavyLifting"] = chkAvoidHeavyLifting.Checked;
                TblRow["Carrying"] = chkCarrying.Checked;
                TblRow["ExcessiveBend"] = chkExcessiveBend.Checked;
                TblRow["ProlongedSitStand"] = chkProlongedSitStand.Checked;
                TblRow["CareOther"] = txtCareOther.Text.ToString();
                TblRow["Cardiac"] = chkCardiac.Checked;
                TblRow["WeightBearing"] = chkWeightBearing.Checked;
                TblRow["EducationProvided"] = chkEducationProvided.Checked;
                TblRow["ViaPhysician"] = chkViaPhysician.Checked;
                TblRow["ViaPrintedMaterial"] = chkViaPrintedMaterial.Checked;
                TblRow["ViaWebsite"] = chkViaWebsite.Checked;
                TblRow["ViaVideo"] = txtViaVideo.Text.ToString();
                TblRow["IsViaVedio"] = chkIsViaVideo.Checked;
                TblRow["ConsultNeuro"] = chkConsultNeuro.Checked;
                TblRow["ConsultOrtho"] = chkConsultOrtho.Checked;
                TblRow["ConsultPsych"] = chkConsultPsych.Checked;
                TblRow["ConsultPodiatrist"] = chkConsultPodiatrist.Checked;
                TblRow["ConsultOther"] = txtConsultOther.Text.ToString();
                TblRow["FollowUpIn"] = txtFollowUpIn.Text.ToString();
                if (!string.IsNullOrWhiteSpace(txtFollowUpInDate.Text))
                    TblRow["FollowUpInDate"] = DateTime.ParseExact(txtFollowUpInDate.Text, "MM/dd/yyyy", null);
                else
                    TblRow["FollowUpInDate"] = DBNull.Value;
                TblRow["Precautions"] = txtPrecautions.Text.ToString();
                TblRow["OtherMedicine"] = txtOtherMedicine.Text.ToString();
                if (_ieMode == "New")
                {
                    TblRow["CreatedBy"] = "Admin";
                    TblRow["CreatedDate"] = DateTime.Now;
                    sqlTbl.Rows.Add(TblRow);
                }
                sqlAdapt.Update(sqlTbl);
            }
            if (TblRow != null)
                TblRow.Table.Dispose();
            sqlTbl.Dispose();
            sqlCmdBuilder.Dispose();
            sqlAdapt.Dispose();
            oSQLConn.Close();
            if (_ieMode == "New")
                return "LowBack has been added...";
            else if (_ieMode == "Update")
                return "LowBack has been updated...";
            else if (_ieMode == "Delete")
                return "LowBack has been deleted...";
            else
                return "";

        }
        catch (Exception ex)
        {
            gDbhelperobj.LogError(ex);
            oSQLConn.Close();
        }
        return "";
    }

    public void PopulateStrightFwd()
    {
        //tbRomIs.Text = "ROM";
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtensionWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFwdFlexWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLateralFlexLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLateralFlexRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal1.Visibility = System.Windows.Visibility.Visible;
        //tbNormal2.Visibility = System.Windows.Visibility.Visible;
        //txtExtensionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFwdFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txtLateralFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txttxtRotationNormal.Visibility = System.Windows.Visibility.Visible;
    }

    public void PopulateUI(string ieID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];
            //cboGAIT.Text = TblRow["GAIT"].ToString().Trim();
            //cboAmbulates.Text = TblRow["Ambulates"].ToString().Trim();
            //chkFootslap.Checked = CommonConvert.ToBoolean(TblRow["Footslap"].ToString());
            //chkKneehyperextension.Checked = CommonConvert.ToBoolean(TblRow["Kneehyperextension"].ToString());
            //chkUnabletohealwalk.Checked = CommonConvert.ToBoolean(TblRow["Unabletohealwalk"].ToString());
            //chkUnabletotoewalk.Checked = CommonConvert.ToBoolean(TblRow["Unabletotoewalk"].ToString());
            //txtOther.Text = TblRow["Other"].ToString().Trim();
            dtpDiagCervialBulgeDate.Text = TblRow["DiagCervialBulgeDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagCervialBulgeDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagCervialBulgeStudy.Text = TblRow["DiagCervialBulgeStudy"].ToString().Trim();
            chkDiagCervialBulge.Checked = CommonConvert.ToBoolean(TblRow["DiagCervialBulge"].ToString());
            txtDiagCervialBulgeText.Text = TblRow["DiagCervialBulgeText"].ToString().Trim();
            txtDiagCervialBulgeHNP1.Text = TblRow["DiagCervialBulgeHNP1"].ToString().Trim();
            txtDiagCervialBulgeHNP2.Text = TblRow["DiagCervialBulgeHNP2"].ToString().Trim();
            dtpDiagThoracicBulgeDate.Text = TblRow["DiagThoracicBulgeDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagThoracicBulgeDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagThoracicBulgeStudy.Text = TblRow["DiagThoracicBulgeStudy"].ToString().Trim();
            chkDiagThoracicBulge.Checked = CommonConvert.ToBoolean(TblRow["DiagThoracicBulge"].ToString());
            txtDiagThoracicBulgeText.Text = TblRow["DiagThoracicBulgeText"].ToString().Trim();
            txtDiagThoracicBulgeHNP1.Text = TblRow["DiagThoracicBulgeHNP1"].ToString().Trim();
            txtDiagThoracicBulgeHNP2.Text = TblRow["DiagThoracicBulgeHNP2"].ToString().Trim();
            dtpDiagLumberBulgeDate.Text = TblRow["DiagLumberBulgeDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagLumberBulgeDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagLumberBulgeStudy.Text = TblRow["DiagLumberBulgeStudy"].ToString().Trim();
            chkDiagLumberBulge.Checked = CommonConvert.ToBoolean(TblRow["DiagLumberBulge"].ToString());
            txtDiagLumberBulgeText.Text = TblRow["DiagLumberBulgeText"].ToString().Trim();
            txtDiagLumberBulgeHNP1.Text = TblRow["DiagLumberBulgeHNP1"].ToString().Trim();
            txtDiagLumberBulgeHNP2.Text = TblRow["DiagLumberBulgeHNP2"].ToString().Trim();
            dtpDiagLeftShoulderDate.Text = TblRow["DiagLeftShoulderDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagLeftShoulderDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagLeftShoulderStudy.Text = TblRow["DiagLeftShoulderStudy"].ToString().Trim();
            chkDiagLeftShoulder.Checked = CommonConvert.ToBoolean(TblRow["DiagLeftShoulder"].ToString());
            txtDiagLeftShoulderText.Text = TblRow["DiagLeftShoulderText"].ToString().Trim();
            dtpDiagRightShoulderDate.Text = TblRow["DiagRightShoulderDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagRightShoulderDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagRightShoulderStudy.Text = TblRow["DiagRightShoulderStudy"].ToString().Trim();
            chkDiagRightShoulder.Checked = CommonConvert.ToBoolean(TblRow["DiagRightShoulder"].ToString());
            txtDiagRightShoulderText.Text = TblRow["DiagRightShoulderText"].ToString().Trim();
            dtpDiagLeftKneeDate.Text = TblRow["DiagLeftKneeDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagLeftKneeDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagLeftKneeStudy.Text = TblRow["DiagLeftKneeStudy"].ToString().Trim();
            chkDiagLeftKnee.Checked = CommonConvert.ToBoolean(TblRow["DiagLeftKnee"].ToString());
            txtDiagLeftKneeText.Text = TblRow["DiagLeftKneeText"].ToString().Trim();
            dtpDiagRightKneeDate.Text = TblRow["DiagRightKneeDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagRightKneeDate"]).ToString("MM/dd/yyyy") : "";
            cboDiagRightKneeStudy.Text = TblRow["DiagRightKneeStudy"].ToString().Trim();
            chkDiagRightKnee.Checked = CommonConvert.ToBoolean(TblRow["DiagRightKnee"].ToString());
            txtDiagRightKneeText.Text = TblRow["DiagRightKneeText"].ToString().Trim();
            //dtpDiagBrainDate.Text = TblRow["diagbraindate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["diagbraindate"]).ToString("MM/dd/yyyy").Trim() : "";
            //cboDiagBrainStudy.Text = TblRow["diagbrainstudy"].ToString().Trim();
            //chkDiagBrain.Checked = CommonConvert.ToBoolean(TblRow["diagbrain"]);
            dtpOther1Date.Text = TblRow["Other1Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other1Date"]).ToString("MM/dd/yyyy") : "";
            cboOther1Study.Text = TblRow["Other1Study"].ToString().Trim();
            txtOther1Text.Text = TblRow["Other1Text"].ToString().Trim();
            dtpOther2Date.Text = TblRow["Other2Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other2Date"]).ToString("MM/dd/yyyy") : "";
            cboOther2Study.Text = TblRow["Other2Study"].ToString().Trim();
            txtOther2Text.Text = TblRow["Other2Text"].ToString().Trim();
            dtpOther3Date.Text = TblRow["Other3Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other3Date"]).ToString("MM/dd/yyyy") : "";
            cboOther3Study.Text = TblRow["Other3Study"].ToString().Trim();
            txtOther3Text.Text = TblRow["Other3Text"].ToString().Trim();
            dtpOther4Date.Text = TblRow["Other4Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other4Date"]).ToString("MM/dd/yyyy") : "";
            cboOther4Study.Text = TblRow["Other4Study"].ToString().Trim();
            txtOther4Text.Text = TblRow["Other4Text"].ToString().Trim();
            dtpOther5Date.Text = TblRow["Other5Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other5Date"]).ToString("MM/dd/yyyy") : "";
            cboOther5Study.Text = TblRow["Other5Study"].ToString().Trim();
            txtOther5Text.Text = TblRow["Other5Text"].ToString().Trim();
            dtpOther6Date.Text = TblRow["Other6Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other6Date"]).ToString("MM/dd/yyyy") : "";
            cboOther6Study.Text = TblRow["Other6Study"].ToString().Trim();
            txtOther6Text.Text = TblRow["Other6Text"].ToString().Trim();
            dtpOther7Date.Text = TblRow["Other7Date"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["Other7Date"]).ToString("MM/dd/yyyy") : "";
            cboOther7Study.Text = TblRow["Other7Study"].ToString().Trim();
            txtOther7Text.Text = TblRow["Other7Text"].ToString().Trim();
            //dtpDiagBrainDate.Text = TblRow["DiagBrainDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["DiagBrainDate"]).ToString("MM/dd/yyyy").Trim() : "";
            //chkDiagBrain.Checked = CommonConvert.ToBoolean(TblRow["DiagBrain"]);
            //txtDiagBrainText.Text = TblRow["DiagBrainText"].ToString().Trim();

            chkProcedures.Checked = CommonConvert.ToBoolean(TblRow["Procedures"].ToString());
            chkAcupuncture.Checked = CommonConvert.ToBoolean(TblRow["Acupuncture"].ToString());
            chkChiropratic.Checked = CommonConvert.ToBoolean(TblRow["Chiropratic"].ToString());
            chkPhysicalTherapy.Checked = CommonConvert.ToBoolean(TblRow["PhysicalTherapy"].ToString());
            chkAvoidHeavyLifting.Checked = CommonConvert.ToBoolean(TblRow["AvoidHeavyLifting"].ToString());
            chkCarrying.Checked = CommonConvert.ToBoolean(TblRow["Carrying"].ToString());
            chkExcessiveBend.Checked = CommonConvert.ToBoolean(TblRow["ExcessiveBend"].ToString());
            chkProlongedSitStand.Checked = CommonConvert.ToBoolean(TblRow["ProlongedSitStand"].ToString());
            txtCareOther.Text = TblRow["CareOther"].ToString().Trim();
            chkCardiac.Checked = CommonConvert.ToBoolean(TblRow["Cardiac"].ToString());
            chkWeightBearing.Checked = CommonConvert.ToBoolean(TblRow["WeightBearing"].ToString());
            chkEducationProvided.Checked = CommonConvert.ToBoolean(TblRow["EducationProvided"].ToString());
            chkViaPhysician.Checked = CommonConvert.ToBoolean(TblRow["ViaPhysician"].ToString());
            chkViaPrintedMaterial.Checked = CommonConvert.ToBoolean(TblRow["ViaPrintedMaterial"].ToString());
            chkViaWebsite.Checked = CommonConvert.ToBoolean(TblRow["ViaWebsite"].ToString());
            txtViaVideo.Text = TblRow["ViaVideo"].ToString().Trim();
            chkIsViaVideo.Checked = CommonConvert.ToBoolean(TblRow["IsViaVedio"].ToString());
            chkConsultNeuro.Checked = CommonConvert.ToBoolean(TblRow["ConsultNeuro"].ToString());
            chkConsultOrtho.Checked = CommonConvert.ToBoolean(TblRow["ConsultOrtho"].ToString());
            chkConsultPsych.Checked = CommonConvert.ToBoolean(TblRow["ConsultPsych"].ToString());
            chkConsultPodiatrist.Checked = CommonConvert.ToBoolean(TblRow["ConsultPodiatrist"].ToString());
            txtConsultOther.Text = TblRow["ConsultOther"].ToString().Trim();
            txtFollowUpIn.Text = TblRow["FollowUpIn"].ToString().Trim();
            txtFollowUpInDate.Text = TblRow["FollowUpInDate"].ToString().Trim() != "" ? Convert.ToDateTime(TblRow["FollowUpInDate"]).ToString("MM/dd/yyyy") : "";
            txtPrecautions.Text = TblRow["Precautions"].ToString().Trim();
            txtOtherMedicine.Text = TblRow["OtherMedicine"].ToString().Trim();

            if ((cboOther1Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther1Date.Text) || !string.IsNullOrEmpty(txtOther1Text.Text)) ||
                 (cboOther2Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther2Date.Text) || !string.IsNullOrEmpty(txtOther2Text.Text)) ||
                 (cboOther3Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther3Date.Text) || !string.IsNullOrEmpty(txtOther3Text.Text)) ||
                 (cboOther4Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther4Date.Text) || !string.IsNullOrEmpty(txtOther4Text.Text)) ||
                 (cboOther5Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther5Date.Text) || !string.IsNullOrEmpty(txtOther5Text.Text)) ||
                 (cboOther6Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther6Date.Text) || !string.IsNullOrEmpty(txtOther6Text.Text)) ||
                 (cboOther7Study.Text.Equals("UE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther7Date.Text) || !string.IsNullOrEmpty(txtOther7Text.Text)))
            { Session["neckdate"] = DateTime.Now; }

            if ((cboOther1Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther1Date.Text) || !string.IsNullOrEmpty(txtOther1Text.Text)) ||
                (cboOther2Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther2Date.Text) || !string.IsNullOrEmpty(txtOther2Text.Text)) ||
                (cboOther3Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther3Date.Text) || !string.IsNullOrEmpty(txtOther3Text.Text)) ||
                (cboOther4Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther4Date.Text) || !string.IsNullOrEmpty(txtOther4Text.Text)) ||
                (cboOther5Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther5Date.Text) || !string.IsNullOrEmpty(txtOther5Text.Text)) ||
                (cboOther6Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther6Date.Text) || !string.IsNullOrEmpty(txtOther6Text.Text)) ||
                (cboOther7Study.Text.Equals("LE NCV/EMG") && !string.IsNullOrWhiteSpace(dtpOther7Date.Text) || !string.IsNullOrEmpty(txtOther7Text.Text)))
            { Session["Lowbackdate"] = DateTime.Now; }

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage3");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;

            //if (txtDiagBrainText.Text == "") txtDiagBrainText.Text = node.SelectSingleNode("DiagBrainStudy") == null ? txtDiagBrainText.Text.ToString().Trim() : node.SelectSingleNode("DiagBrainStudy").InnerText;
            //if (cboGAIT.Text == "") cboGAIT.Text = node.SelectSingleNode("GAIT") == null ? cboGAIT.Text.ToString().Trim() : node.SelectSingleNode("GAIT").InnerText;
            //if (cboAmbulates.Text == "") cboAmbulates.Text = node.SelectSingleNode("Ambulates") == null ? cboAmbulates.Text.ToString().Trim() : node.SelectSingleNode("Ambulates").InnerText;
            //chkFootslap.Checked = node.SelectSingleNode("Footslap") == null ? chkFootslap.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Footslap").InnerText);
            //chkKneehyperextension.Checked = node.SelectSingleNode("Kneehyperextension") == null ? chkKneehyperextension.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Kneehyperextension").InnerText);
            //chkUnabletohealwalk.Checked = node.SelectSingleNode("Unabletohealwalk") == null ? chkUnabletohealwalk.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Unabletohealwalk").InnerText);
            //chkUnabletotoewalk.Checked = node.SelectSingleNode("Unabletotoewalk") == null ? chkUnabletotoewalk.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Unabletotoewalk").InnerText);
            //if (txtOther.Text == "") txtOther.Text = node.SelectSingleNode("Other") == null ? txtOther.Text.ToString().Trim() : node.SelectSingleNode("Other").InnerText;
            if (dtpDiagCervialBulgeDate.Text == "") dtpDiagCervialBulgeDate.Text = node.SelectSingleNode("DiagCervialBulgeDate") == null ? dtpDiagCervialBulgeDate.Text.ToString().Trim() : node.SelectSingleNode("DiagCervialBulgeDate").InnerText;
            if (cboDiagCervialBulgeStudy.Text == "") cboDiagCervialBulgeStudy.Text = node.SelectSingleNode("DiagCervialBulgeStudy") == null ? cboDiagCervialBulgeStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagCervialBulgeStudy").InnerText;
            chkDiagCervialBulge.Checked = node.SelectSingleNode("DiagCervialBulge") == null ? chkDiagCervialBulge.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagCervialBulge").InnerText);
            if (txtDiagCervialBulgeText.Text == "") txtDiagCervialBulgeText.Text = node.SelectSingleNode("DiagCervialBulgeText") == null ? txtDiagCervialBulgeText.Text.ToString().Trim() : node.SelectSingleNode("DiagCervialBulgeText").InnerText;
            if (txtDiagCervialBulgeHNP1.Text == "") txtDiagCervialBulgeHNP1.Text = node.SelectSingleNode("DiagCervialBulgeHNP1") == null ? txtDiagCervialBulgeHNP1.Text.ToString().Trim() : node.SelectSingleNode("DiagCervialBulgeHNP1").InnerText;
            if (txtDiagCervialBulgeHNP2.Text == "") txtDiagCervialBulgeHNP2.Text = node.SelectSingleNode("DiagCervialBulgeHNP2") == null ? txtDiagCervialBulgeHNP2.Text.ToString().Trim() : node.SelectSingleNode("DiagCervialBulgeHNP2").InnerText;
            if (dtpDiagThoracicBulgeDate.Text == "") dtpDiagThoracicBulgeDate.Text = node.SelectSingleNode("DiagThoracicBulgeDate") == null ? dtpDiagThoracicBulgeDate.Text.ToString().Trim() : node.SelectSingleNode("DiagThoracicBulgeDate").InnerText;
            if (cboDiagThoracicBulgeStudy.Text == "") cboDiagThoracicBulgeStudy.Text = node.SelectSingleNode("DiagThoracicBulgeStudy") == null ? cboDiagThoracicBulgeStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagThoracicBulgeStudy").InnerText;
            chkDiagThoracicBulge.Checked = node.SelectSingleNode("DiagThoracicBulge") == null ? chkDiagThoracicBulge.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagThoracicBulge").InnerText);
            if (txtDiagThoracicBulgeText.Text == "") txtDiagThoracicBulgeText.Text = node.SelectSingleNode("DiagThoracicBulgeText") == null ? txtDiagThoracicBulgeText.Text.ToString().Trim() : node.SelectSingleNode("DiagThoracicBulgeText").InnerText;
            if (txtDiagThoracicBulgeHNP1.Text == "") txtDiagThoracicBulgeHNP1.Text = node.SelectSingleNode("DiagThoracicBulgeHNP1") == null ? txtDiagThoracicBulgeHNP1.Text.ToString().Trim() : node.SelectSingleNode("DiagThoracicBulgeHNP1").InnerText;
            if (txtDiagThoracicBulgeHNP2.Text == "") txtDiagThoracicBulgeHNP2.Text = node.SelectSingleNode("DiagThoracicBulgeHNP2") == null ? txtDiagThoracicBulgeHNP2.Text.ToString().Trim() : node.SelectSingleNode("DiagThoracicBulgeHNP2").InnerText;
            if (dtpDiagLumberBulgeDate.Text == "") dtpDiagLumberBulgeDate.Text = node.SelectSingleNode("DiagLumberBulgeDate") == null ? dtpDiagLumberBulgeDate.Text.ToString().Trim() : node.SelectSingleNode("DiagLumberBulgeDate").InnerText;
            if (cboDiagLumberBulgeStudy.Text == "") cboDiagLumberBulgeStudy.Text = node.SelectSingleNode("DiagLumberBulgeStudy") == null ? cboDiagLumberBulgeStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagLumberBulgeStudy").InnerText;
            chkDiagLumberBulge.Checked = node.SelectSingleNode("DiagLumberBulge") == null ? chkDiagLumberBulge.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagLumberBulge").InnerText);
            if (txtDiagLumberBulgeText.Text == "") txtDiagLumberBulgeText.Text = node.SelectSingleNode("DiagLumberBulgeText") == null ? txtDiagLumberBulgeText.Text.ToString().Trim() : node.SelectSingleNode("DiagLumberBulgeText").InnerText;
            if (txtDiagLumberBulgeHNP1.Text == "") txtDiagLumberBulgeHNP1.Text = node.SelectSingleNode("DiagLumberBulgeHNP1") == null ? txtDiagLumberBulgeHNP1.Text.ToString().Trim() : node.SelectSingleNode("DiagLumberBulgeHNP1").InnerText;
            if (txtDiagLumberBulgeHNP2.Text == "") txtDiagLumberBulgeHNP2.Text = node.SelectSingleNode("DiagLumberBulgeHNP2") == null ? txtDiagLumberBulgeHNP2.Text.ToString().Trim() : node.SelectSingleNode("DiagLumberBulgeHNP2").InnerText;
            if (dtpDiagLeftShoulderDate.Text == "") dtpDiagLeftShoulderDate.Text = node.SelectSingleNode("DiagLeftShoulderDate") == null ? dtpDiagLeftShoulderDate.Text.ToString().Trim() : node.SelectSingleNode("DiagLeftShoulderDate").InnerText;
            if (cboDiagLeftShoulderStudy.Text == "") cboDiagLeftShoulderStudy.Text = node.SelectSingleNode("DiagLeftShoulderStudy") == null ? cboDiagLeftShoulderStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagLeftShoulderStudy").InnerText;
            chkDiagLeftShoulder.Checked = node.SelectSingleNode("DiagLeftShoulder") == null ? chkDiagLeftShoulder.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagLeftShoulder").InnerText);
            if (txtDiagLeftShoulderText.Text == "") txtDiagLeftShoulderText.Text = node.SelectSingleNode("DiagLeftShoulderText") == null ? txtDiagLeftShoulderText.Text.ToString().Trim() : node.SelectSingleNode("DiagLeftShoulderText").InnerText;
            if (dtpDiagRightShoulderDate.Text == "") dtpDiagRightShoulderDate.Text = node.SelectSingleNode("DiagRightShoulderDate") == null ? dtpDiagRightShoulderDate.Text.ToString().Trim() : node.SelectSingleNode("DiagRightShoulderDate").InnerText;
            if (cboDiagRightShoulderStudy.Text == "") cboDiagRightShoulderStudy.Text = node.SelectSingleNode("DiagRightShoulderStudy") == null ? cboDiagRightShoulderStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagRightShoulderStudy").InnerText;
            chkDiagRightShoulder.Checked = node.SelectSingleNode("DiagRightShoulder") == null ? chkDiagRightShoulder.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagRightShoulder").InnerText);
            if (txtDiagRightShoulderText.Text == "") txtDiagRightShoulderText.Text = node.SelectSingleNode("DiagRightShoulderText") == null ? txtDiagRightShoulderText.Text.ToString().Trim() : node.SelectSingleNode("DiagRightShoulderText").InnerText;
            if (dtpDiagLeftKneeDate.Text == "") dtpDiagLeftKneeDate.Text = node.SelectSingleNode("DiagLeftKneeDate") == null ? dtpDiagLeftKneeDate.Text.ToString().Trim() : node.SelectSingleNode("DiagLeftKneeDate").InnerText;
            if (cboDiagLeftKneeStudy.Text == "") cboDiagLeftKneeStudy.Text = node.SelectSingleNode("DiagLeftKneeStudy") == null ? cboDiagLeftKneeStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagLeftKneeStudy").InnerText;
            chkDiagLeftKnee.Checked = node.SelectSingleNode("DiagLeftKnee") == null ? chkDiagLeftKnee.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagLeftKnee").InnerText);
            if (txtDiagLeftKneeText.Text == "") txtDiagLeftKneeText.Text = node.SelectSingleNode("DiagLeftKneeText") == null ? txtDiagLeftKneeText.Text.ToString().Trim() : node.SelectSingleNode("DiagLeftKneeText").InnerText;
            if (dtpDiagRightKneeDate.Text == "") dtpDiagRightKneeDate.Text = node.SelectSingleNode("DiagRightKneeDate") == null ? dtpDiagRightKneeDate.Text.ToString().Trim() : node.SelectSingleNode("DiagRightKneeDate").InnerText;
            if (cboDiagRightKneeStudy.Text == "") cboDiagRightKneeStudy.Text = node.SelectSingleNode("DiagRightKneeStudy") == null ? cboDiagRightKneeStudy.Text.ToString().Trim() : node.SelectSingleNode("DiagRightKneeStudy").InnerText;
            chkDiagRightKnee.Checked = node.SelectSingleNode("DiagRightKnee") == null ? chkDiagRightKnee.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("DiagRightKnee").InnerText);
            if (txtDiagRightKneeText.Text == "") txtDiagRightKneeText.Text = node.SelectSingleNode("DiagRightKneeText") == null ? txtDiagRightKneeText.Text.ToString().Trim() : node.SelectSingleNode("DiagRightKneeText").InnerText;
            if (dtpOther1Date.Text == "") dtpOther1Date.Text = node.SelectSingleNode("Other1Date") == null ? dtpOther1Date.Text.ToString().Trim() : node.SelectSingleNode("Other1Date").InnerText;
            if (cboOther1Study.Text == "") cboOther1Study.Text = node.SelectSingleNode("Other1Study") == null ? cboOther1Study.Text.ToString().Trim() : node.SelectSingleNode("Other1Study").InnerText;
            if (txtOther1Text.Text == "") txtOther1Text.Text = node.SelectSingleNode("Other1Text") == null ? txtOther1Text.Text.ToString().Trim() : node.SelectSingleNode("Other1Text").InnerText;
            if (dtpOther2Date.Text == "") dtpOther2Date.Text = node.SelectSingleNode("Other2Date") == null ? dtpOther2Date.Text.ToString().Trim() : node.SelectSingleNode("Other2Date").InnerText;
            if (cboOther2Study.Text == "") cboOther2Study.Text = node.SelectSingleNode("Other2Study") == null ? cboOther2Study.Text.ToString().Trim() : node.SelectSingleNode("Other2Study").InnerText;
            if (txtOther2Text.Text == "") txtOther2Text.Text = node.SelectSingleNode("Other2Text") == null ? txtOther2Text.Text.ToString().Trim() : node.SelectSingleNode("Other2Text").InnerText;
            if (dtpOther3Date.Text == "") dtpOther3Date.Text = node.SelectSingleNode("Other3Date") == null ? dtpOther3Date.Text.ToString().Trim() : node.SelectSingleNode("Other3Date").InnerText;
            if (cboOther3Study.Text == "") cboOther3Study.Text = node.SelectSingleNode("Other3Study") == null ? cboOther3Study.Text.ToString().Trim() : node.SelectSingleNode("Other3Study").InnerText;
            if (txtOther3Text.Text == "") txtOther3Text.Text = node.SelectSingleNode("Other3Text") == null ? txtOther3Text.Text.ToString().Trim() : node.SelectSingleNode("Other3Text").InnerText;
            if (dtpOther4Date.Text == "") dtpOther4Date.Text = node.SelectSingleNode("Other4Date") == null ? dtpOther4Date.Text.ToString().Trim() : node.SelectSingleNode("Other4Date").InnerText;
            if (cboOther4Study.Text == "") cboOther4Study.Text = node.SelectSingleNode("Other4Study") == null ? cboOther4Study.Text.ToString().Trim() : node.SelectSingleNode("Other4Study").InnerText;
            if (txtOther4Text.Text == "") txtOther4Text.Text = node.SelectSingleNode("Other4Text") == null ? txtOther4Text.Text.ToString().Trim() : node.SelectSingleNode("Other4Text").InnerText;
            if (dtpOther5Date.Text == "") dtpOther5Date.Text = node.SelectSingleNode("Other5Date") == null ? dtpOther5Date.Text.ToString().Trim() : node.SelectSingleNode("Other5Date").InnerText;
            if (cboOther5Study.Text == "") cboOther5Study.Text = node.SelectSingleNode("Other5Study") == null ? cboOther5Study.Text.ToString().Trim() : node.SelectSingleNode("Other5Study").InnerText;
            if (txtOther5Text.Text == "") txtOther5Text.Text = node.SelectSingleNode("Other5Text") == null ? txtOther5Text.Text.ToString().Trim() : node.SelectSingleNode("Other5Text").InnerText;
            if (dtpOther6Date.Text == "") dtpOther6Date.Text = node.SelectSingleNode("Other6Date") == null ? dtpOther6Date.Text.ToString().Trim() : node.SelectSingleNode("Other6Date").InnerText;
            if (cboOther6Study.Text == "") cboOther6Study.Text = node.SelectSingleNode("Other6Study") == null ? cboOther6Study.Text.ToString().Trim() : node.SelectSingleNode("Other6Study").InnerText;
            if (txtOther6Text.Text == "") txtOther6Text.Text = node.SelectSingleNode("Other6Text") == null ? txtOther6Text.Text.ToString().Trim() : node.SelectSingleNode("Other6Text").InnerText;
            if (dtpOther7Date.Text == "") dtpOther7Date.Text = node.SelectSingleNode("Other7Date") == null ? dtpOther7Date.Text.ToString().Trim() : node.SelectSingleNode("Other7Date").InnerText;
            if (cboOther7Study.Text == "") cboOther7Study.Text = node.SelectSingleNode("Other7Study") == null ? cboOther7Study.Text.ToString().Trim() : node.SelectSingleNode("Other7Study").InnerText;
            if (txtOther7Text.Text == "") txtOther7Text.Text = node.SelectSingleNode("Other7Text") == null ? txtOther7Text.Text.ToString().Trim() : node.SelectSingleNode("Other7Text").InnerText;
            chkProcedures.Checked = node.SelectSingleNode("Procedures") == null ? chkProcedures.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Procedures").InnerText);
            chkAcupuncture.Checked = node.SelectSingleNode("Acupuncture") == null ? chkAcupuncture.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Acupuncture").InnerText);
            chkChiropratic.Checked = node.SelectSingleNode("Chiropratic") == null ? chkChiropratic.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Chiropratic").InnerText);
            chkPhysicalTherapy.Checked = node.SelectSingleNode("PhysicalTherapy") == null ? chkPhysicalTherapy.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("PhysicalTherapy").InnerText);
            chkAvoidHeavyLifting.Checked = node.SelectSingleNode("AvoidHeavyLifting") == null ? chkAvoidHeavyLifting.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("AvoidHeavyLifting").InnerText);
            chkCarrying.Checked = node.SelectSingleNode("Carrying") == null ? chkCarrying.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Carrying").InnerText);
            chkExcessiveBend.Checked = node.SelectSingleNode("ExcessiveBend") == null ? chkExcessiveBend.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ExcessiveBend").InnerText);
            chkProlongedSitStand.Checked = node.SelectSingleNode("ProlongedSitStand") == null ? chkProlongedSitStand.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ProlongedSitStand").InnerText);
            if (txtCareOther.Text == "") txtCareOther.Text = node.SelectSingleNode("CareOther") == null ? txtCareOther.Text.ToString().Trim() : node.SelectSingleNode("CareOther").InnerText;
            chkCardiac.Checked = node.SelectSingleNode("Cardiac") == null ? chkCardiac.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Cardiac").InnerText);
            chkWeightBearing.Checked = node.SelectSingleNode("WeightBearing") == null ? chkWeightBearing.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("WeightBearing").InnerText);
            chkEducationProvided.Checked = node.SelectSingleNode("EducationProvided") == null ? chkEducationProvided.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("EducationProvided").InnerText);
            chkViaPhysician.Checked = node.SelectSingleNode("ViaPhysician") == null ? chkViaPhysician.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ViaPhysician").InnerText);
            chkViaPrintedMaterial.Checked = node.SelectSingleNode("ViaPrintedMaterial") == null ? chkViaPrintedMaterial.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ViaPrintedMaterial").InnerText);
            chkViaWebsite.Checked = node.SelectSingleNode("ViaWebsite") == null ? chkViaWebsite.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ViaWebsite").InnerText);
            if (txtViaVideo.Text == "") txtViaVideo.Text = node.SelectSingleNode("ViaVideo") == null ? txtViaVideo.Text.ToString().Trim() : node.SelectSingleNode("ViaVideo").InnerText;
            chkIsViaVideo.Checked = node.SelectSingleNode("IsViaVedio") == null ? chkIsViaVideo.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("IsViaVedio").InnerText);
            chkConsultNeuro.Checked = node.SelectSingleNode("ConsultNeuro") == null ? chkConsultNeuro.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ConsultNeuro").InnerText);
            chkConsultOrtho.Checked = node.SelectSingleNode("ConsultOrtho") == null ? chkConsultOrtho.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ConsultOrtho").InnerText);
            chkConsultPsych.Checked = node.SelectSingleNode("ConsultPsych") == null ? chkConsultPsych.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ConsultPsych").InnerText);
            chkConsultPodiatrist.Checked = node.SelectSingleNode("ConsultPodiatrist") == null ? chkConsultPodiatrist.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("ConsultPodiatrist").InnerText);
            if (txtConsultOther.Text == "") txtConsultOther.Text = node.SelectSingleNode("ConsultOther") == null ? txtConsultOther.Text.ToString().Trim() : node.SelectSingleNode("ConsultOther").InnerText;
            if (txtFollowUpIn.Text == "") txtFollowUpIn.Text = node.SelectSingleNode("FollowUpIn") == null ? txtFollowUpIn.Text.ToString().Trim() : node.SelectSingleNode("FollowUpIn").InnerText;
            if (txtPrecautions.Text == "") txtPrecautions.Text = node.SelectSingleNode("Precautions") == null ? txtPrecautions.Text.ToString().Trim() : node.SelectSingleNode("Precautions").InnerText;
            if (txtOtherMedicine.Text == "") txtOtherMedicine.Text = node.SelectSingleNode("OtherMedicine") == null ? txtPrecautions.Text.ToString().Trim() : node.SelectSingleNode("OtherMedicine").InnerText;
            _fldPop = false;
        }
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
            if (!IsPostBack)
            {
                SqlDataAdapter oSQLAdpr;
                DataTable Medicine = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblMedicationRx WHERE PatientIE_ID = " + _CurIEid + " Order By Medicine";
                oSQLCmd.Connection = oSQLConn;
                oSQLCmd.CommandText = SqlStr;
                oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
                oSQLAdpr.Fill(Medicine);
                dgvMedi.DataSource = "";
                dgvMedi.DataSource = Medicine.DefaultView;
                dgvMedi.DataBind();
                oSQLAdpr.Dispose();
                oSQLConn.Close();
            }
            else
            {
                if (ViewState["DiagnosisList"] != null)
                {
                    List<AddDrug> objList = (List<AddDrug>)ViewState["DiagnosisList"];

                    dgvMedi.DataSource = objList;
                    dgvMedi.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public string SaveMedicines(string ieID)
    {
        string ids = string.Empty;
        try
        {
            RemoveDrugDetail(ieID);
            foreach (GridViewRow row in dgvMedi.Rows)
            {
                string iMedID, med;
                iMedID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                med = row.Cells[0].Controls.OfType<TextBox>().FirstOrDefault().Text;
                ids += Session["PatientIE_ID"].ToString() + ",";

                bool isChecked = row.Cells[1].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    SaveMediUI(ieID, iMedID, true, med);
                }
            }
        }
        catch (Exception ex)
        {

        }
        if (ids != string.Empty)
            return "Medicine(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }

    public void SaveMediUI(string ieID, string iMedID, bool MedIsChecked, string med)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _MedID = Convert.ToInt64(iMedID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblMedicationRx WHERE PatientIE_ID = " + ieID + " AND MedicationID = " + _MedID;
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
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["MedicationID"] = _MedID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["Medicine"] = med.ToString().Trim();

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDateTime"] = DateTime.Now;
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

    private void Page_Loaded_1(object sender, EventArgs e) //RoutedEventArgs 
    {
        PopulateStrightFwd();
    }

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string ieMode = "New";
            SaveMedicines(Session["PatientIE_ID"].ToString());
            SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
            PopulateUI(Session["PatientIE_ID"].ToString());
            Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Create IE - Page5 " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);

        }
        catch (Exception ex)
        {
            gDbhelperobj.LogError(ex);
        }
        finally
        {
            lblMessage.InnerHtml = "Additional Complaints have been Updated successfuly.";
            lblMessage.Attributes.Add("style", "color:green");
            upMessage.Update();
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);

        }
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }

    protected void AddDrug_Click(object sender, ImageClickEventArgs e)
    {
        string ieMode = "New";
        bindgridPoup();
        //SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        //Response.Redirect("AddDrugs.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindgridPoup();
    }

    protected void btnDaigSave_Click(object sender, EventArgs e)
    {
        SaveStandardsPopup(Session["PatientIE_ID"].ToString());
        BindDataGrid();
        txDesc.Text = string.Empty;
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Test", "closeModelPopup()", true);
        upMedicine.Update();
    }

    private void bindgridPoup()
    {
        try
        {
            string _CurBodyPart = _CurBP;
            string _SKey = "WHERE tblMedicines.Medicine LIKE '%" + txDesc.Text.Trim() + "%'";
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";

            _CurIEid = Session["PatientIE_ID"].ToString();

            if (!string.IsNullOrEmpty(_CurIEid))
                SqlStr = "Select tblMedicines.*, dbo.MEDEXISTS(" + _CurIEid + ", Medicine_ID) as IsChkd FROM tblMedicines " + _SKey + " Order By Medicine";
            else
                SqlStr = "Select tblMedicines.*, dbo.MEDEXISTS('0', Medicine_ID) as IsChkd FROM tblMedicines " + _SKey + " Order By Medicine";
            ds = gDbhelperobj.selectData(SqlStr);

            dgvMedicenPopup.DataSource = ds;
            dgvMedicenPopup.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    public string SaveStandardsPopup(string ieID)
    {
        List<AddDrug> objList = new List<AddDrug>();
        AddDrug obj = new AddDrug();
        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvMedicenPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    obj = new AddDrug();

                    obj.Medicine_ID = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.MedicationID = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Medicine = row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.IsChkd = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;

                    if (obj.IsChkd)
                    {
                        ids += obj.Medicine_ID + ",";
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

    protected void RemoveDrugDetail(string PatientIE_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblMedicationRx WHERE PatientIE_ID=" + PatientIE_ID;
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }
}