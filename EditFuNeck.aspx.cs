using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class EditFuNeck : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Neck";
    ILog log = log4net.LogManager.GetLogger(typeof(EditFuNeck));


    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Neck";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            checkTP();
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {
                BindROM();
                bindDropdown();
                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpNeck WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpNeck WHERE PatientIE_ID= " + _CurIEid + "");
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
                    PopulateUIDefaults();
                    BindDataGrid();
                }
            }
            else
            {
                Response.Redirect("EditFU.aspx");
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuNeck for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }
    [System.Web.Services.WebMethod]
    public string SaveUI_Ajax()
    {
        string ieMode = "New";
        string ieID = Session["PatientIE_ID"].ToString();
        bool bpChecked = true;
        // string result=  SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpNeck WHERE PatientIE_ID = " + ieID;
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
            TblRow["PatientIE_ID"] = _ieID;
          
            //TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            //TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty);
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["Xrays"] = false;//TBD
            TblRow["TPEpidular"] = false;//TBD
            TblRow["NoOfTPInjections"] = "";// txtPainScale;
            TblRow["ScheduleEpidural"] = false;//txtPainScale;
            TblRow["NewMBB"] = false;// txtPainScale;
            TblRow["SPTPMBB"] = false;//txtPainScale;
            TblRow["ISFirst"] = true;

          

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
            return "Neck has been added...";
        else if (_ieMode == "Update")
            return "Neck has been updated...";
        else if (_ieMode == "Delete")
            return "Neck has been deleted...";
        else
            return "";
    }

    public string SaveUI(string ieID, string fuID, string fuMode, bool bpIsChecked)
    {
        long _fuID = Convert.ToInt64(fuID);
        string _fuMode = "";

        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpNeck WHERE PatientFU_ID = " + _fuID;
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
           
            //TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            //TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty);
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["Xrays"] = false;//TBD
            TblRow["TPEpidular"] = false;//TBD
            TblRow["NoOfTPInjections"] = "";// txtPainScale;
            TblRow["ScheduleEpidural"] = false;//txtPainScale;
            TblRow["NewMBB"] = false;// txtPainScale;
            TblRow["SPTPMBB"] = false;//txtPainScale;

            TblRow["CCvalue"] = hdCCvalue.Value;
          
            TblRow["PEvalue"] = hdPEvalue.Value;
            TblRow["PEvalueoriginal"] = hdPEvalueoriginal.Value;
           
            TblRow["PESides"] = hdPESides.Value;
            TblRow["PESidesText"] = hdPESidesText.Value;

            string strname = "", strleft = "", strright = "", strnormal = "", strcname = "", strcrom = "", strcnormal = "";

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


            for (int i = 0; i < repROMCervical.Items.Count; i++)
            {
                Label lblname = repROMCervical.Items[i].FindControl("lblname") as Label;
                TextBox txtrom = repROMCervical.Items[i].FindControl("txtrom") as TextBox;
                TextBox txtnormal = repROMCervical.Items[i].FindControl("txtnormal") as TextBox;

                strcname = strcname + "," + lblname.Text;
                strcrom = strcrom + "," + txtrom.Text;
                strcnormal = strcnormal + "," + txtnormal.Text;
            }

            if (!string.IsNullOrEmpty(strcname))
                TblRow["CNameROM"] = strcname.Substring(1);
            if (!string.IsNullOrEmpty(strcrom))
                TblRow["CROM"] = strcrom.Substring(1);
            if (!string.IsNullOrEmpty(strcnormal))
                TblRow["CNormalROM"] = strcnormal.Substring(1);





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
            return "Neck has been added...";
        else if (_fuMode == "Update")
            return "Neck has been updated...";
        else if (_fuMode == "Delete")
            return "Neck has been deleted...";
        else
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

    public void PopulateUI(string fuid)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpNeck WHERE PatientFU_ID = " + fuid;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
           
            //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            // txtWorseOtherText.Text = TblRow["WorseOther"].ToString().Trim();

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

    public void PopulateIEUI(string ieid)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpNeck WHERE PatientIE_ID = " + ieid;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
           
            //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            // txtWorseOtherText.Text = TblRow["WorseOther"].ToString().Trim();
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

        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Neck");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
         
            //if (txtFreeForm.Text == "") txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            //if (txtFreeFormCC.Text == "") txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            if (txtFreeFormA.Text == "") txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty) : node.SelectSingleNode("FreeFormA").InnerText.Trim().Replace("      ", string.Empty);
            if (txtFreeFormP.Text == "") txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
            //  if (txtWorseOtherText.Text == "") txtWorseOtherText.Text = node.SelectSingleNode("WorseOther") == null ? txtWorseOtherText.Text.ToString().Trim() : node.SelectSingleNode("WorseOther").InnerText;
            _fldPop = false;
        }
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

    //protected void btnReset_Click(object sender, EventArgs e)// RoutedEventArgs
    //{
    //    chkShoulderLeft1.Checked = false;
    //    chkScapulaLeft1.Checked = false;
    //    chkArmLeft1.Checked = false;
    //    chkForearmLeft1.Checked = false;
    //    chkHandLeft1.Checked = false;
    //    chkWristLeft1.Checked = false;
    //    chk1stDigitLeft1.Checked = false;
    //    chk2ndDigitLeft1.Checked = false;
    //    chk3rdDigitLeft1.Checked = false;
    //    chk4thDigitLeft1.Checked = false;
    //    chk5thDigitLeft1.Checked = false;
    //    chkShoulderRight1.Checked = false;
    //    chkScapulaRight1.Checked = false;
    //    chkArmRight1.Checked = false;
    //    chkForearmRight1.Checked = false;
    //    chkHandRight1.Checked = false;
    //    chkWristRight1.Checked = false;
    //    chk1stDigitRight1.Checked = false;
    //    chk2ndDigitRight1.Checked = false;
    //    chk3rdDigitRight1.Checked = false;
    //    chk4thDigitRight1.Checked = false;
    //    chk5thDigitRight1.Checked = false;
    //    chkShoulderBilateral1.Checked = false;
    //    chkScapulaBilateral1.Checked = false;
    //    chkArmBilateral1.Checked = false;
    //    chkForearmBilateral1.Checked = false;
    //    chkHandBilateral1.Checked = false;
    //    chkWristBilateral1.Checked = false;
    //    chk1stDigitBilateral1.Checked = false;
    //    chk2ndDigitBilateral1.Checked = false;
    //    chk3rdDigitBilateral1.Checked = false;
    //    chk4thDigitBilateral1.Checked = false;
    //    chk5thDigitBilateral1.Checked = false;
    //    chkShoulderNone1.Checked = false;
    //    chkScapulaNone1.Checked = false;
    //    chkArmNone1.Checked = false;
    //    chkForearmNone1.Checked = false;
    //    chkHandNone1.Checked = false;
    //    chkWristNone1.Checked = false;
    //    chk1stDigitNone1.Checked = false;
    //    chk2ndDigitNone1.Checked = false;
    //    chk3rdDigitNone1.Checked = false;
    //    chk4thDigitNone1.Checked = false;
    //    chk5thDigitNone1.Checked = false;
    //}

    //protected void btnReset1_Click(object sender, EventArgs e)//RoutedEventArgs 
    //{
    //    chkShoulderLeft2.Checked = false;
    //    chkScapulaLeft2.Checked = false;
    //    chkArmLeft2.Checked = false;
    //    chkForearmLeft2.Checked = false;
    //    chkHandLeft2.Checked = false;
    //    chkWristLeft2.Checked = false;
    //    chk1stDigitLeft2.Checked = false;
    //    chk2ndDigitLeft2.Checked = false;
    //    chk3rdDigitLeft2.Checked = false;
    //    chk4thDigitLeft2.Checked = false;
    //    chk5thDigitLeft2.Checked = false;
    //    chkShoulderRight2.Checked = false;
    //    chkScapulaRight2.Checked = false;
    //    chkArmRight2.Checked = false;
    //    chkForearmRight2.Checked = false;
    //    chkHandRight2.Checked = false;
    //    chkWristRight2.Checked = false;
    //    chk1stDigitRight2.Checked = false;
    //    chk2ndDigitRight2.Checked = false;
    //    chk3rdDigitRight2.Checked = false;
    //    chk4thDigitRight2.Checked = false;
    //    chk5thDigitRight2.Checked = false;
    //    chkShoulderBilateral2.Checked = false;
    //    chkScapulaBilateral2.Checked = false;
    //    chkArmBilateral2.Checked = false;
    //    chkForearmBilateral2.Checked = false;
    //    chkHandBilateral2.Checked = false;
    //    chkWristBilateral2.Checked = false;
    //    chk1stDigitBilateral2.Checked = false;
    //    chk2ndDigitBilateral2.Checked = false;
    //    chk3rdDigitBilateral2.Checked = false;
    //    chk4thDigitBilateral2.Checked = false;
    //    chk5thDigitBilateral2.Checked = false;
    //    chkShoulderNone2.Checked = false;
    //    chkScapulaNone2.Checked = false;
    //    chkArmNone2.Checked = false;
    //    chkForearmNone2.Checked = false;
    //    chkHandNone2.Checked = false;
    //    chkWristNone2.Checked = false;
    //    chk1stDigitNone2.Checked = false;
    //    chk2ndDigitNone2.Checked = false;
    //    chk3rdDigitNone2.Checked = false;
    //    chk4thDigitNone2.Checked = false;
    //    chk5thDigitNone2.Checked = false;
    //}

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

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "Update";
        SaveDiagnosis(_CurIEid);
        SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["patientFUId"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }

    protected void AddStd_Click1(object sender, ImageClickEventArgs e)
    {
        string ieMode = "New";
        SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        Response.Redirect("AddStandards.aspx");
        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + "AddStandards.aspx" + "','_blank')", true);
    }

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        bindgridPoup();
        //SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
        // Response.Redirect("AddDiagnosis.aspx");
    }

    public void bindDropdown()
    {
        //XmlDocument doc = new XmlDocument();
        //doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        //foreach (XmlNode node in doc.SelectNodes("//HSM/Results/Result"))
        //{
        //    cboSpurlings.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //    cboDistraction.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Neck");
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


        _FuId = Session["patientFUId"].ToString();
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpNeck WHERE PatientFU_ID = " + _FuId;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        oSQLConn.Close();
        if (sqlTbl.Rows.Count > 0)
        {
            string[] strname, strrom, strnormal, strleftrom, strrightrom;
            // Create the Table
            DataTable ROMTable = new DataTable("ROM");
            // Build the Orders schema
            ROMTable.Columns.Add("name", Type.GetType("System.String"));
            ROMTable.Columns.Add("rom", Type.GetType("System.String"));
            ROMTable.Columns.Add("normal", Type.GetType("System.String"));
            ROMTable.Columns.Add("left", Type.GetType("System.String"));
            ROMTable.Columns.Add("right", Type.GetType("System.String"));

            if (string.IsNullOrEmpty(sqlTbl.Rows[0]["CNameROM"].ToString()) == false)
            {
                strname = sqlTbl.Rows[0]["CNameROM"].ToString().Split(',');
                strrom = sqlTbl.Rows[0]["CROM"].ToString().Split(',');
                strnormal = sqlTbl.Rows[0]["CNormalROM"].ToString().Split(',');


                DataRow workRow;

                for (int i = 0; i < strname.Length; i++)
                {
                    workRow = ROMTable.NewRow();
                    workRow[0] = strname[i];
                    workRow[1] = strrom[i];
                    workRow[2] = strnormal[i];
                    ROMTable.Rows.Add(workRow);
                }

                if (ROMTable.Rows.Count != 0)
                {
                    repROMCervical.DataSource = ROMTable;
                    repROMCervical.DataBind();
                }

                ROMTable.Rows.Clear();
            }
            else
                getXMLROMvalue(true, false);

            if (string.IsNullOrEmpty(sqlTbl.Rows[0]["NameROM"].ToString()) == false)
            {
                strname = sqlTbl.Rows[0]["NameROM"].ToString().Split(',');
                strleftrom = sqlTbl.Rows[0]["LeftROM"].ToString().Split(',');
                strrightrom = sqlTbl.Rows[0]["RightROM"].ToString().Split(',');
                strnormal = sqlTbl.Rows[0]["NormalROM"].ToString().Split(',');


                DataRow workRow;

                for (int i = 0; i < strname.Length; i++)
                {
                    workRow = ROMTable.NewRow();
                    workRow[0] = strname[i];
                    workRow[2] = strnormal[i];
                    workRow[3] = strleftrom[i];
                    workRow[4] = strrightrom[i];
                    ROMTable.Rows.Add(workRow);
                }

                if (ROMTable.Rows.Count != 0)
                {
                    repROM.DataSource = ROMTable;
                    repROM.DataBind();
                }

                ROMTable.Rows.Clear();
            }
            else
                getXMLROMvalue(false, true);
        }
        else
        {
            getXMLROMvalue(true, true);
        }
    }

    private void getXMLROMvalue(bool rom, bool crom)
    {
        //open the tender xml file  
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Neck.xml"));
        //reading the xml data  
        DataSet ds = new DataSet();
        ds.ReadXml(xmlreader);
        xmlreader.Close();
        //if ds is not empty  
        if (ds.Tables.Count != 0)
        {
            if (rom)
            {
                repROM.DataSource = ds.Tables[0];
                repROM.DataBind();
            }

            if (crom)
            {
                repROMCervical.DataSource = ds.Tables[1];
                repROMCervical.DataBind();
            }
        }
    }
}