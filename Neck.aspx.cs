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
using System.Text.RegularExpressions;
using log4net;
using System.Web.Services;

public partial class Neck : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Neck";
    ILog log = log4net.LogManager.GetLogger(typeof(Neck));


    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Neck";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {

            checkTP();

            if (Session["PatientIE_ID"] != null)
            {
                bindDropdown();

                BindROM();
                _CurIEid = Session["PatientIE_ID"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();

                //change on 14-8-2017-Moulick
                // string query = ("select count(*) as count1 FROM tblbpNeck WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                string query = ("select ISFirst FROM tblbpNeck WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");

                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                cn.Close();

                //change on 14-8-2017-Moulick

                //DataRow rw = ds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                //if (rw != null)
                //{
                //    // row exists
                //    PopulateUIDefaults();
                //    BindDataGrid();
                //}
                //else
                //{
                //    _CurIEid = Session["PatientIE_ID"].ToString();
                //    patientID.Value = Session["PatientIE_ID"].ToString();
                //    PopulateUI(_CurIEid);

                //    BindDataGrid();
                //}

                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ISFirst"] != DBNull.Value ? !Convert.ToBoolean(ds.Tables[0].Rows[0]["ISFirst"].ToString()) : true)
                {  // row exists
                    PopulateUIDefaults();
                    BindDataGrid();
                    bindCF();
                    bindPE();
                }

                else
                {
                    _CurIEid = Session["PatientIE_ID"].ToString();
                    patientID.Value = Session["PatientIE_ID"].ToString();
                    PopulateUI(_CurIEid);
                    BindDataGrid();
                }

            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
            bindgridPoup();
            BindDCDataGrid();
        }


        Logger.Info(Session["uname"].ToString() + "- Visited in  Neck for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
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

            //TblRow["WorseOther"] = txtWorseOtherText.Text.ToString();

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

    public string SaveUI(string ieID, string ieMode, bool bpChecked, string val = "", string orgval = "", string painradiation = "", string painradiation1 = "")
    {
        try
        {
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

                if (!string.IsNullOrEmpty(strleft))
                    TblRow["LeftROM"] = strleft.Substring(1);
                if (!string.IsNullOrEmpty(strright))
                    TblRow["RightROM"] = strright.Substring(1);
                if (!string.IsNullOrEmpty(strnormal))
                    TblRow["NormalROM"] = strnormal.Substring(1);
                if (!string.IsNullOrEmpty(strname))
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
                return "Neck has been added...";
            else if (_ieMode == "Update")
                return "Neck has been updated...";
            else if (_ieMode == "Delete")
                return "Neck has been deleted...";
            else
                return "";

        }
        catch (Exception ex)
        {
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

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];

            //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("    ", string.Empty);


            CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();

            divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();

            hdorgvalPE.Value = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();

            int val = checkTP();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ",1);bindSidesVal('" + sqlTbl.Rows[0]["PESides"].ToString() + "','" + sqlTbl.Rows[0]["PESidesText"].ToString() + "');", true);

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

        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Neck");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            //if (txtFreeForm.Text == "") txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            //if (txtFreeFormCC.Text == "") txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            if (txtFreeFormA.Text == "") txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty) : node.SelectSingleNode("FreeFormA").InnerText.Trim().Replace("      ", string.Empty);

            //if (txtFreeFormA.Text == "") txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            //string RemText = "Strain of muscle, fascia, tendons (cervical): S16.1xxA, S16.1xxD";
            //if (Session["neckdate"] != null)
            //{ txtFreeFormA.Text = txtFreeFormA.Text.Replace(RemText+System.Environment.NewLine,string.Empty); }

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
            // SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' and IsConsidered=0  Order By BodyPart,Heading";
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

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        SaveDiagnosis(Session["PatientIE_ID"].ToString());
        SaveStandards(Session["PatientIE_ID"].ToString());
        SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);

        PopulateUI(Session["PatientIE_ID"].ToString());
    }
    protected void AddStd_Click1(object sender, ImageClickEventArgs e)
    {
        string ieMode = "New";
        SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        Response.Redirect("AddStandards.aspx");
        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + "AddStandards.aspx" + "','_blank')", true);
    }

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        //SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        // bindgridPoup();
        //SaveStandards(Session["PatientIE_ID"].ToString());
        //Response.Redirect("AddDiagnosis.aspx");
    }

    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

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

        //foreach (XmlNode node in doc.SelectNodes("//HSM/Levels/Level"))
        //{
        //    ddlLevels.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}

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


    public void bindCF()
    {
        string path = Server.MapPath("~/Template/NeckCC.html");
        string body = File.ReadAllText(path);

        CF.InnerHtml = body;
        hdorgval.Value = body;
    }

    public void bindPE()
    {
        string path = Server.MapPath("~/Template/NeckPE.html");
        string body = File.ReadAllText(path);

        divPE.InnerHtml = body;
        hdorgvalPE.Value = body;

        int val = checkTP();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ",0)", true);
    }


    protected void BindROM()
    {


        long _ieID = Convert.ToInt64(Session["PatientIE_ID"]);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpNeck WHERE PatientIE_ID = " + _ieID;
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
                getXMLROMvalue(false, false);
        }
        else
        {
            getXMLROMvalue(true, false);
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