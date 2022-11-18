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

public partial class Hip : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Hip";
    string Position = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();


    ILog log = log4net.LogManager.GetLogger(typeof(Hip));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        Session["PageName"] = "Hip";
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
                string query = ("select count(*) as count1 FROM tblbpHip WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
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
                            //wrpLeft.Visible = true;
                            //wrpRight.Visible = false;
                            //Second div
                            //wrpLeft2.Visible = true;
                            //wrpRight2.Visible = false;
                            //Left textbox
                            txtFlexLeft.ReadOnly = false;
                            txtIntRotationLeft.ReadOnly = false;
                            txtExtRotationLeft.ReadOnly = false;
                            //Left textbox
                            txtFlexRight.ReadOnly = true;
                            txtIntRotationRight.ReadOnly = true;
                            txtExtRotationRight.ReadOnly = true;
                            //Right Checkbox
                            //chkOberLeft.Enabled = true;
                            //chkFaberLeft.Enabled = true;
                            //chkTrendelenburgLeft.Enabled = true;
                            //Left checkbox
                            //chkOberRight.Enabled = false;
                            //chkFaberRight.Enabled = false;
                            //chkTrendelenburgRight.Enabled = false;

                            break;
                        case "R":
                            //first div
                            //wrpLeft.Visible = false;
                            //wrpRight.Visible = true;
                            //Second div
                            //wrpLeft2.Visible = false;
                            //wrpRight2.Visible = true;
                            //Left textbox
                            txtFlexLeft.ReadOnly = true;
                            txtIntRotationLeft.ReadOnly = true;
                            txtExtRotationLeft.ReadOnly = true;
                            //Right textbox
                            txtFlexRight.ReadOnly = false;
                            txtIntRotationRight.ReadOnly = false;
                            txtExtRotationRight.ReadOnly = false;
                            //Right Checkbox
                            //chkOberLeft.Enabled = false;
                            //chkFaberLeft.Enabled = false;
                            //chkTrendelenburgLeft.Enabled = false;
                            //Left checkbox
                            //chkOberRight.Enabled = true;
                            //chkFaberRight.Enabled = true;
                            //chkTrendelenburgRight.Enabled = true;
                            break;
                        case "B":
                            //first div
                            //wrpLeft.Visible = true;
                            //wrpRight.Visible = true;
                            //Second div
                            //wrpLeft2.Visible = true;
                            //wrpRight2.Visible = true;
                            //Left textbox
                            txtFlexLeft.ReadOnly = false;
                            txtIntRotationLeft.ReadOnly = false;
                            txtExtRotationLeft.ReadOnly = false;
                            //Left textbox
                            txtFlexRight.ReadOnly = false;
                            txtIntRotationRight.ReadOnly = false;
                            txtExtRotationRight.ReadOnly = false;
                            //Right Checkbox
                            //chkOberLeft.Enabled = true;
                            //chkFaberLeft.Enabled = true;
                            //chkTrendelenburgLeft.Enabled = true;
                            //Left checkbox
                            //chkOberRight.Enabled = true;
                            //chkFaberRight.Enabled = true;
                            //chkTrendelenburgRight.Enabled = true;
                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
            bindgridPoup();
        }
        BindDCDataGrid();
        Logger.Info(Session["uname"].ToString() + "- Visited in  Hip for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    public string SaveUI(string ieID, string ieMode, bool bpIsChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpHip WHERE PatientIE_ID = " + ieID;
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


            //TblRow["ConstantLeft"] = chkContentLeft.Checked;
            //TblRow["IntermittentLeft"] = chkIntermittentLeft.Checked;
            //TblRow["ConstantRight"] = chkContentRight.Checked;
            //TblRow["IntermittentRight"] = chkIntermittentRight.Checked;

            //TblRow["WorseSittingLeft"] = chkWorseSittingLeft.Checked;
            //TblRow["WorseStandingLeft"] = chkWorseStandingLeft.Checked;
            //TblRow["WorseMovementLeft"] = chkWorseMovementLeft.Checked;
            //TblRow["WorseActivitiesLeft"] = chkWorseActivitiesLeft.Checked;
            //TblRow["WorseOtherLeft"] = txtWorseOtherLeft.Text.ToString();
            //TblRow["WorseSittingRight"] = chkWorseSittingRight.Checked;
            //TblRow["WorseStandingRight"] = chkWorseStandingRight.Checked;
            //TblRow["WorseMovementRight"] = chkWorseMovementRight.Checked;
            //TblRow["WorseActivitiesRight"] = chkWorseActivitiesRight.Checked;
            //TblRow["WorseOtherRight"] = txtWorseOtherRight.Text.ToString();

            //TblRow["GreaterTrochanterLeft"] = chkGreaterTrochanterLeft.Checked;
            //TblRow["PosteriorLeft"] = chkPosteriorLeft.Checked;
            //TblRow["IliotibialLeft"] = chkIliotibialLeft.Checked;
            //TblRow["GreaterTrochanterRight"] = chkGreaterTrochanterRight.Checked;
            //TblRow["PosteriorRight"] = chkPosteriorRight.Checked;
            //TblRow["IliotibialRight"] = chkIliotibialRight.Checked;

            TblRow["FlexRight"] = txtFlexRight.Text.ToString();
            TblRow["IntRotationRight"] = txtIntRotationRight.Text.ToString();
            TblRow["ExtRotationRight"] = txtExtRotationRight.Text.ToString();

            TblRow["FlexLeft"] = txtFlexLeft.Text.ToString();
            TblRow["IntRotationLeft"] = txtIntRotationLeft.Text.ToString();
            TblRow["ExtRotationLeft"] = txtExtRotationLeft.Text.ToString();

            TblRow["FlexNormal"] = txtFlexNormal.Text.ToString();
            TblRow["IntRotationNormal"] = txtIntRotationNormal.Text.ToString();
            TblRow["ExtRotationNormal"] = txtExtRotationNormal.Text.ToString();

           
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();

            TblRow["CCvalue"] = hdCCvalue.Value;
         


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
            TblRow["PEvalue"] = hdPEvalue.Value;
           

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
            return "Hip has been added...";
        else if (_ieMode == "Update")
            return "Hip has been updated...";
        else if (_ieMode == "Delete")
            return "Hip has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string ieID)
    {

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpHip WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];

           
            txtFlexRight.Text = TblRow["FlexRight"].ToString().Trim();
            txtIntRotationRight.Text = TblRow["IntRotationRight"].ToString().Trim();
            txtExtRotationRight.Text = TblRow["ExtRotationRight"].ToString().Trim();
            txtFlexLeft.Text = TblRow["FlexLeft"].ToString().Trim();
            txtIntRotationLeft.Text = TblRow["IntRotationLeft"].ToString().Trim();
            txtExtRotationLeft.Text = TblRow["ExtRotationLeft"].ToString().Trim();

            txtFlexNormal.Text = Convert.ToString(TblRow["FlexNormal"]);
            txtIntRotationNormal.Text = Convert.ToString(TblRow["IntRotationNormal"]);
            txtExtRotationNormal.Text = Convert.ToString(TblRow["ExtRotationNormal"]);

            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();

            CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();
         

            divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();

          
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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Hip");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
          
            txtFlexNormal.Text = node.SelectSingleNode("HipFlexNormal") == null ? txtFlexNormal.Text.ToString().Trim() : node.SelectSingleNode("HipFlexNormal").InnerText;
            txtIntRotationNormal.Text = node.SelectSingleNode("HipIRNormal") == null ? txtIntRotationNormal.Text.ToString().Trim() : node.SelectSingleNode("HipIRNormal").InnerText;
            txtExtRotationNormal.Text = node.SelectSingleNode("HipERNormal") == null ? txtExtRotationNormal.Text.ToString().Trim() : node.SelectSingleNode("HipERNormal").InnerText;

            txtFlexRight.Text = node.SelectSingleNode("FlexRight") == null ? txtFlexRight.Text.ToString().Trim() : node.SelectSingleNode("FlexRight").InnerText;
            txtIntRotationRight.Text = node.SelectSingleNode("IntRotationRight") == null ? txtIntRotationRight.Text.ToString().Trim() : node.SelectSingleNode("IntRotationRight").InnerText;
            txtExtRotationRight.Text = node.SelectSingleNode("ExtRotationRight") == null ? txtExtRotationRight.Text.ToString().Trim() : node.SelectSingleNode("ExtRotationRight").InnerText;
            txtFlexLeft.Text = node.SelectSingleNode("FlexLeft") == null ? txtFlexLeft.Text.ToString().Trim() : node.SelectSingleNode("FlexLeft").InnerText;
            txtIntRotationLeft.Text = node.SelectSingleNode("IntRotationLeft") == null ? txtIntRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("IntRotationLeft").InnerText;
            txtExtRotationLeft.Text = node.SelectSingleNode("ExtRotationLeft") == null ? txtExtRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("ExtRotationLeft").InnerText;
           
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
           
            _fldPop = false;
        }
    }
    public void PopulateStrightFwd(bool bL, bool bR)
    {
        bool bLeft = bL;
        bool bRight = bR;
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFlexRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFlexLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtIntRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtIntRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal.Visibility = System.Windows.Visibility.Visible;
        //txtExtRotationNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txtIntRotationNormal.Visibility = System.Windows.Visibility.Visible;

        //wrpLeft1.IsEnabled =
        //wrpLeft2.IsEnabled = bLeft;

        //wrpRight1.IsEnabled =
        //wrpRight2.IsEnabled = bRight;

        //txtFlexRight.IsEnabled = bRight;
        //txtFlexLeft.IsEnabled = bLeft;
        //txtIntRotationRight.IsEnabled = bRight;
        //txtIntRotationLeft.IsEnabled = bLeft;
        //txtExtRotationRight.IsEnabled = bRight;
        //txtExtRotationLeft.IsEnabled = bLeft;
        //txtFlexRightWas.IsEnabled = bRight;
        //txtFlexLeftWas.IsEnabled = bLeft;
        //txtIntRotationRightWas.IsEnabled = bRight;
        //txtIntRotationLeftWas.IsEnabled = bLeft;
        //txtExtRotationRightWas.IsEnabled = bRight;
        //txtExtRotationLeftWas.IsEnabled = bLeft;

        //chkOberRight.IsEnabled = bRight;
        //chkOberLeft.IsEnabled = bLeft;
        //chkFaberRight.IsEnabled = bRight;
        //chkFaberLeft.IsEnabled = bLeft;
        //chkTrendelenburgRight.IsEnabled = bRight;
        //chkTrendelenburgLeft.IsEnabled = bLeft;

        //if (bLeft && bRight)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex = cboIntDerangementSide.SelectedIndex = 3;
        //else if (bLeft)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex = cboIntDerangementSide.SelectedIndex = 1;
        //else
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex = cboIntDerangementSide.SelectedIndex = 2;
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

    protected void BindROM()
    {


        long _ieID = Convert.ToInt64(Session["PatientIE_ID"]);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpHip WHERE PatientIE_ID = " + _ieID;
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
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Hip.xml"));
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
        string path = Server.MapPath("~/Template/HipCC.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");


        CF.InnerHtml = body;
     
    }

    public void bindPE()
    {
        string path = Server.MapPath("~/Template/HipPE.html");
        string body = File.ReadAllText(path);

        string p = Request.QueryString["P"];

        if (p == "L")
        {
            body = body.Replace("#rigthtdiv", "style='display:none'");

        }
        else if (p == "R")
        {
            body = body.Replace("#leftdiv", "style='display:none'");

        }
        else
            p = "B";

        divPE.InnerHtml = body;
      

        //int val = checkTP();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP('" + p + "')", true);
    }


}