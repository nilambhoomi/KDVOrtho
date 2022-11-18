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

public partial class FuMidBack : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Midback";
    ILog log = log4net.LogManager.GetLogger(typeof(FuMidBack));

    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Midback";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (Session["patientFUId"] == null || Session["patientFUId"] == "")
        {
            Response.Redirect("AddFu.aspx");
        }
        if (!IsPostBack)
        {
            bindDropdown();
            if (Session["PatientIE_ID2"] != null && Session["patientFUId"] != null)
            {
                _CurIEid = Session["PatientIE_ID2"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpMidBack WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpMidBack WHERE PatientIE_ID= " + _CurIEid + "");
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

            }
            else
            {
                Response.Redirect("AddFU.aspx");
            }
            Session["refresh_count"] = 0;
        }


        Logger.Info(Session["uname"].ToString() + "- Visited in  FuMidBack for -" + Convert.ToString(Session["LastNameFU"]) + Convert.ToString(Session["FirstNameFU"]) + "-" + DateTime.Now);
    }
    public string SaveUI(string ieID, string ieMode, bool bpChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpMidBack WHERE PatientFU_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && bpChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpChecked == false)
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
            TblRow["PatientFU_ID"] = _ieID;
            //TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            //TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["CCvalue"] = hdCCvalue.Value;


            TblRow["PEvalue"] = hdPEvalue.Value;
            TblRow["PEvalueoriginal"] = hdPEvalueoriginal.Value;
            TblRow["PESidesText"] = hdPESidesText.Value;
            TblRow["PESides"] = hdPESides.Value;


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
            return "MidBack has been added...";
        else if (_ieMode == "Update")
            return "MidBack has been updated...";
        else if (_ieMode == "Delete")
            return "MidBack has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string fuID)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpMidBack WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];

            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            //txtPalpationAt.Text = Convert.ToString(TblRow["PalpationAt"]);

            CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();


            divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();

            hdorgvalPE.Value = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();

            int val = checkTP();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ");bindSidesVal('" + sqlTbl.Rows[0]["PESides"].ToString() + "','" + sqlTbl.Rows[0]["PESidesText"].ToString() + "');", true);
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
        SqlStr = "Select * from tblbpMidBack WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            //txtPainScale.Text = TblRow["PainScale"].ToString().Trim();
            //chkSharp.Checked = Convert.ToBoolean(TblRow["Sharp"]);

            //chkContent.Checked = Convert.ToBoolean(TblRow["constant"]);
            //chkIntermittent.Checked = Convert.ToBoolean(TblRow["intermittent"]);

            //chkelectric.Checked = Convert.ToBoolean(TblRow["Electric"]);
            //chkshooting.Checked = Convert.ToBoolean(TblRow["Shooting"]);
            //chkthrobbing.Checked = Convert.ToBoolean(TblRow["Throbbling"]);
            //chkpulsating.Checked = Convert.ToBoolean(TblRow["Pulsating"]);
            //chkdull.Checked = Convert.ToBoolean(TblRow["Dull"]);
            //chkachy.Checked = Convert.ToBoolean(TblRow["Achy"]);
            //cboRadiates.Text = TblRow["Radiates"].ToString().Trim();
            //chkWorseSitting.Checked = Convert.ToBoolean(TblRow["WorseSitting"]);
            //chkWorseStanding.Checked = Convert.ToBoolean(TblRow["WorseStanding"]);
            //chkWorseLyingDown.Checked = Convert.ToBoolean(TblRow["WorseLyingDown"]);
            //chkWorseMovement.Checked = Convert.ToBoolean(TblRow["WorseMovement"]);
            //chkWorseBending.Checked = Convert.ToBoolean(TblRow["WorseBending"]);
            //chkWorseLifting.Checked = Convert.ToBoolean(TblRow["WorseLifting"]);
            //chkWorseSeatingtoStandingUp.Checked = Convert.ToBoolean(TblRow["WorseSeatingtoStandingUp"]);
            //chkWorseWalking.Checked = Convert.ToBoolean(TblRow["WorseWalking"]);
            //chkWorseClimbingStairs.Checked = Convert.ToBoolean(TblRow["WorseClimbingStairs"]);
            //chkWorseDescendingStairs.Checked = Convert.ToBoolean(TblRow["WorseDescendingStairs"]);
            //chkWorseDriving.Checked = Convert.ToBoolean(TblRow["WorseDriving"]);
            //chkWorseWorking.Checked = Convert.ToBoolean(TblRow["WorseWorking"]);
            //txtWorseOtherText.Text = TblRow["WorseOtherText"].ToString().Trim();
            //chkImprovedResting.Checked = Convert.ToBoolean(TblRow["ImprovedResting"]);
            //chkImprovedMedication.Checked = Convert.ToBoolean(TblRow["ImprovedMedication"]);
            //chkImprovedTherapy.Checked = Convert.ToBoolean(TblRow["ImprovedTherapy"]);
            //chkImprovedSleeping.Checked = Convert.ToBoolean(TblRow["ImprovedSleeping"]);
            //chkImprovedMovement.Checked = Convert.ToBoolean(TblRow["ImprovedMovement"]);
            //txtPalpationAt.Text = TblRow["PalpationAt"].ToString().Trim();
            //cboLevels.Text = TblRow["Levels"].ToString().Trim();
            //cboROM.Text = TblRow["ROM"].ToString().Trim();
            //cboTPSide1.Text = TblRow["TPSide1"].ToString().Trim();
            //txtTPText1.Text = TblRow["TPText1"].ToString().Trim();
            //cboTPSide2.Text = TblRow["TPSide2"].ToString().Trim();
            //txtTPText2.Text = TblRow["TPText2"].ToString().Trim();
            //cboTPSide3.Text = TblRow["TPSide3"].ToString().Trim();
            //txtTPText3.Text = TblRow["TPText3"].ToString().Trim();
            //cboTPSide4.Text = TblRow["TPSide4"].ToString().Trim();
            //txtTPText4.Text = TblRow["TPText4"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            if (Session["refresh_count"] != null)
                if (Session["refresh_count"] != "0")
                {
                    //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
                    //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/MidBack");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;

            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            //if (isTP == false)
            //    divTP.Attributes.Add("style", "display:none");
            //else
            //    divTP.Attributes.Add("style", "display:block");

            //chkContent.Checked = node.SelectSingleNode("constant") == null ? chkSharp.Checked : Convert.ToBoolean(node.SelectSingleNode("constant").InnerText);
            //chkIntermittent.Checked = node.SelectSingleNode("intermittent") == null ? chkSharp.Checked : Convert.ToBoolean(node.SelectSingleNode("intermittent").InnerText);
            //txtPainScale.Text = node.SelectSingleNode("PainScale") == null ? txtPainScale.Text.ToString().Trim() : node.SelectSingleNode("PainScale").InnerText;
            //chkSharp.Checked = node.SelectSingleNode("Sharp") == null ? chkSharp.Checked : Convert.ToBoolean(node.SelectSingleNode("Sharp").InnerText);
            //chkelectric.Checked = node.SelectSingleNode("Electric") == null ? chkelectric.Checked : Convert.ToBoolean(node.SelectSingleNode("Electric").InnerText);
            //chkshooting.Checked = node.SelectSingleNode("Shooting") == null ? chkshooting.Checked : Convert.ToBoolean(node.SelectSingleNode("Shooting").InnerText);
            //chkthrobbing.Checked = node.SelectSingleNode("Throbbling") == null ? chkthrobbing.Checked : Convert.ToBoolean(node.SelectSingleNode("Throbbling").InnerText);
            //chkpulsating.Checked = node.SelectSingleNode("Pulsating") == null ? chkpulsating.Checked : Convert.ToBoolean(node.SelectSingleNode("Pulsating").InnerText);
            //chkdull.Checked = node.SelectSingleNode("Dull") == null ? chkdull.Checked : Convert.ToBoolean(node.SelectSingleNode("Dull").InnerText);
            //chkachy.Checked = node.SelectSingleNode("Achy") == null ? chkachy.Checked : Convert.ToBoolean(node.SelectSingleNode("Achy").InnerText);
            //cboRadiates.Text = node.SelectSingleNode("Radiates") == null ? cboRadiates.Text.ToString().Trim() : node.SelectSingleNode("Radiates").InnerText;
            //chkWorseSitting.Checked = node.SelectSingleNode("WorseSitting") == null ? chkWorseSitting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSitting").InnerText);
            //chkWorseStanding.Checked = node.SelectSingleNode("WorseStanding") == null ? chkWorseStanding.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseStanding").InnerText);
            //chkWorseLyingDown.Checked = node.SelectSingleNode("WorseLyingDown") == null ? chkWorseLyingDown.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingDown").InnerText);
            //chkWorseMovement.Checked = node.SelectSingleNode("WorseMovement") == null ? chkWorseMovement.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovement").InnerText);
            //chkWorseBending.Checked = node.SelectSingleNode("WorseBending") == null ? chkWorseBending.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseBending").InnerText);
            //chkWorseLifting.Checked = node.SelectSingleNode("WorseLifting") == null ? chkWorseLifting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLifting").InnerText);
            //chkWorseSeatingtoStandingUp.Checked = node.SelectSingleNode("WorseSeatingtoStandingUp") == null ? chkWorseSeatingtoStandingUp.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSeatingtoStandingUp").InnerText);
            //chkWorseWalking.Checked = node.SelectSingleNode("WorseWalking") == null ? chkWorseWalking.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWalking").InnerText);
            //chkWorseClimbingStairs.Checked = node.SelectSingleNode("WorseClimbingStairs") == null ? chkWorseClimbingStairs.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseClimbingStairs").InnerText);
            //chkWorseDescendingStairs.Checked = node.SelectSingleNode("WorseDescendingStairs") == null ? chkWorseDescendingStairs.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseDescendingStairs").InnerText);
            //chkWorseDriving.Checked = node.SelectSingleNode("WorseDriving") == null ? chkWorseDriving.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseDriving").InnerText);
            //chkWorseWorking.Checked = node.SelectSingleNode("WorseWorking") == null ? chkWorseWorking.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorking").InnerText);
            //txtWorseOtherText.Text = node.SelectSingleNode("WorseOtherText") == null ? txtWorseOtherText.Text.ToString().Trim() : node.SelectSingleNode("WorseOtherText").InnerText;
            //chkImprovedResting.Checked = node.SelectSingleNode("ImprovedResting") == null ? chkImprovedResting.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedResting").InnerText);
            //chkImprovedMedication.Checked = node.SelectSingleNode("ImprovedMedication") == null ? chkImprovedMedication.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedication").InnerText);
            //chkImprovedTherapy.Checked = node.SelectSingleNode("ImprovedTherapy") == null ? chkImprovedTherapy.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapy").InnerText);
            //chkImprovedSleeping.Checked = node.SelectSingleNode("ImprovedSleeping") == null ? chkImprovedSleeping.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleeping").InnerText);
            //chkImprovedMovement.Checked = node.SelectSingleNode("ImprovedMovement") == null ? chkImprovedMovement.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMovement").InnerText);
            //if (txtPalpationAt.Text == "") txtPalpationAt.Text = node.SelectSingleNode("PalpationAt") == null ? txtPalpationAt.Text.ToString().Trim() : node.SelectSingleNode("PalpationAt").InnerText;
            //cboLevels.Text = node.SelectSingleNode("Levels") == null ? cboLevels.Text.ToString().Trim() : node.SelectSingleNode("Levels").InnerText;
            //cboROM.Text = node.SelectSingleNode("ROM") == null ? cboROM.Text.ToString().Trim() : node.SelectSingleNode("ROM").InnerText;
            //cboTPSide1.Text = node.SelectSingleNode("TPSide1") == null ? cboTPSide1.Text.ToString().Trim() : node.SelectSingleNode("TPSide1").InnerText;
            //txtTPText1.Text = node.SelectSingleNode("TPText1") == null ? txtTPText1.Text.ToString().Trim() : node.SelectSingleNode("TPText1").InnerText;
            //cboTPSide2.Text = node.SelectSingleNode("TPSide2") == null ? cboTPSide2.Text.ToString().Trim() : node.SelectSingleNode("TPSide2").InnerText;
            //txtTPText2.Text = node.SelectSingleNode("TPText2") == null ? txtTPText2.Text.ToString().Trim() : node.SelectSingleNode("TPText2").InnerText;
            //cboTPSide3.Text = node.SelectSingleNode("TPSide3") == null ? cboTPSide3.Text.ToString().Trim() : node.SelectSingleNode("TPSide3").InnerText;
            //txtTPText3.Text = node.SelectSingleNode("TPText3") == null ? txtTPText3.Text.ToString().Trim() : node.SelectSingleNode("TPText3").InnerText;
            //cboTPSide4.Text = node.SelectSingleNode("TPSide4") == null ? cboTPSide4.Text.ToString().Trim() : node.SelectSingleNode("TPSide4").InnerText;
            //txtTPText4.Text = node.SelectSingleNode("TPText4") == null ? txtTPText4.Text.ToString().Trim() : node.SelectSingleNode("TPText4").InnerText;
            // txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            // txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            // txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
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
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID + " AND PatientFu_ID=" + Session["patientFUId"].ToString();
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
        _FuId = Session["patientFUId"].ToString();
        if (_FuId == "" || _FuId == "0")
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

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        SaveDiagnosis(Session["patientFUId"].ToString());
        SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["patientFUId"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }
    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        bindgridPoup();
        Session["refresh_count"] = Convert.ToInt64(Session["refresh_count"]) + 1;
        //SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
        //Response.Redirect("AddFuDiagnosis.aspx");
    }
    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        //foreach (XmlNode node in doc.SelectNodes("//HSM/Radiates/Radiate"))
        //{
        //    cboRadiates.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
        //foreach (XmlNode node in doc.SelectNodes("//HSM/Levels/Level"))
        //{
        //    cboLevels.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}
        //foreach (XmlNode node in doc.SelectNodes("//HSM/ROMs/ROM"))
        //{
        //    cboROM.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}



        //ListItemCollection collection = new ListItemCollection();
        //collection.Add(new ListItem("on the left"));
        //collection.Add(new ListItem("on the right"));
        //collection.Add(new ListItem("bilaterally"));
        //collection.Add(new ListItem("left greater than right"));
        //collection.Add(new ListItem("right greater than left"));

        //ddlLevels.DataSource = collection;
        //ddlLevels.DataBind();

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/MidBack");
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
}