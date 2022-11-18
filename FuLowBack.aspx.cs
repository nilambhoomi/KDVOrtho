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

public partial class FuLowBack : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Lowback";
    ILog log = log4net.LogManager.GetLogger(typeof(FuLowBack));


    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Lowback";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (Session["patientFUId"] == null || Session["patientFUId"] == "")
        {
            Response.Redirect("AddFu.aspx");
        }
        if (!IsPostBack)
        {

            if (Session["PatientIE_ID2"] != null && Session["patientFUId"] != null)
            {
                BindROM();
                bindDropdown();
                _CurIEid = Session["PatientIE_ID2"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpLowBack WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpLowBack WHERE PatientIE_ID= " + _CurIEid + "");
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

        Logger.Info(Session["uname"].ToString() + "- Visited in  FuLowBack for -" + Convert.ToString(Session["LastNameFU"]) + Convert.ToString(Session["FirstNameFU"]) + "-" + DateTime.Now);

    }
    public string SaveUI(string FuID, string ieMode, bool bpChecked)
    {
        long _FuID = Convert.ToInt64(FuID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpLowBack WHERE PatientFU_ID = " + FuID;
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
            TblRow["PatientFU_ID"] = _FuID;

           

            //TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            //TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty);
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();

            TblRow["ISFirst"] = true;
            TblRow["CCvalue"] = hdCCvalue.Value;

            TblRow["PEvalue"] = hdPEvalue.Value;
            TblRow["PEvalueoriginal"] = hdPEvalueoriginal.Value;
            TblRow["PESidesText"] = hdPESidesText.Value;
            TblRow["PESides"] = hdPESides.Value;

            TblRow["NameTest"] = hdNameTest.Value;
            TblRow["LeftTest"] = hdLeftTest.Value;
            TblRow["RightTest"] = hdRightTest.Value;
            TblRow["TextVal"] = hdTextVal.Value;


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
            return "LowBack has been added...";
        else if (_ieMode == "Update")
            return "LowBack has been updated...";
        else if (_ieMode == "Delete")
            return "LowBack has been deleted...";
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

    public void PopulateUI(string fuID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblFubpLowBack WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];


           

            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);

            //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

            CF.InnerHtml = TblRow["CCvalue"].ToString();


            divPE.InnerHtml = TblRow["PEvalue"].ToString();

            hdorgvalPE.Value = TblRow["PEvalueoriginal"].ToString();

            int val = checkTP();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ");bindSideval('" + TblRow["PESides"].ToString() + "','" + TblRow["PESidesText"].ToString() + "')", true);

            _fldPop = false;
        }

        sqlTbl1.Dispose();
        sqlCmdBuilder1.Dispose();
        sqlAdapt1.Dispose();
        oSQLConn.Close();


    }
    public void PopulateIEUI(string IEID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblbpLowBack WHERE PatientIE_ID = " + IEID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];


         

            //txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            //txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/LowBack");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;


           
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty) : node.SelectSingleNode("FreeFormA").InnerText.Trim().Replace("      ", string.Empty);
          
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
            // SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' Order By BodyPart,Heading";
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
                ids += Session["PatientIE_ID2"].ToString() + ",";
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


    private void Page_Loaded_1(object sender, EventArgs e) //RoutedEventArgs 
    {
        PopulateStrightFwd();
    }

    //protected void btnReset_Click(object sender, EventArgs e)// RoutedEventArgs
    //{
    //    chkSideLeft1.Checked = false;
    //    chkButtockLeft1.Checked = false;
    //    chkGroinLeft1.Checked = false;
    //    chkHipLeft1.Checked = false;
    //    chkThighLeft1.Checked = false;
    //    chkLegLeft1.Checked = false;
    //    chkKneeLeft1.Checked = false;
    //    chkAnkleLeft1.Checked = false;
    //    chkAnkleRight1.Checked = false;
    //    chkFeetLeft1.Checked = false;
    //    chkToeLeft1.Checked = false;
    //    chkSideRight1.Checked = false;
    //    chkButtockRight1.Checked = false;
    //    chkGroinRight1.Checked = false;
    //    chkHipRight1.Checked = false;
    //    chkThighRight1.Checked = false;
    //    chkLegRight1.Checked = false;
    //    chkSideBilateral1.Checked = false;
    //    chkButtockBilateral1.Checked = false;
    //    chkGroinBilateral1.Checked = false;
    //    chkHipBilateral1.Checked = false;
    //    chkThighBilateral1.Checked = false;
    //    chkLegBilateral1.Checked = false;
    //    chkKneeBilateral1.Checked = false;
    //    chkAnkleBilateral1.Checked = false;
    //    chkFeetBilateral1.Checked = false;
    //    chkToeBilateral1.Checked = false;
    //    chkSide1None.Checked = false;
    //    chkButtock1None.Checked = false;
    //    chkGroin1None.Checked = false;
    //    chkHip1None.Checked = false;
    //    chkThigh1None.Checked = false;
    //    chkLeg1None.Checked = false;
    //    chkKnee1None.Checked = false;
    //    chkAnkle1None.Checked = false;
    //    chkFeet1None.Checked = false;
    //    chkToe1None.Checked = false;
    //}

    //protected void btnReset1_Click(object sender, EventArgs e)//RoutedEventArgs 
    //{
    //    chkSideLeft2.Checked = false;
    //    chkButtockLeft2.Checked = false;
    //    chkGroinLeft2.Checked = false;
    //    chkHipLeft2.Checked = false;
    //    chkThighLeft2.Checked = false;
    //    chkLegLeft2.Checked = false;
    //    chkKneeLeft2.Checked = false;
    //    chkAnkleLeft2.Checked = false;
    //    chkFeetLeft2.Checked = false;
    //    chkToeLeft2.Checked = false;
    //    chkSideRight2.Checked = false;
    //    chkButtockRight2.Checked = false;
    //    chkGroinRight2.Checked = false;
    //    chkHipRight2.Checked = false;
    //    chkThighRight2.Checked = false;
    //    chkLegRight2.Checked = false;
    //    chkSideBilateral2.Checked = false;
    //    chkButtockBilateral2.Checked = false;
    //    chkGroinBilateral2.Checked = false;
    //    chkHipBilateral2.Checked = false;
    //    chkThighBilateral2.Checked = false;
    //    chkLegBilateral2.Checked = false;
    //    chkKneeBilateral2.Checked = false;
    //    chkAnkleBilateral2.Checked = false;
    //    chkFeetBilateral2.Checked = false;
    //    chkToeBilateral2.Checked = false;
    //    chkSide2None.Checked = false;
    //    chkButtock2None.Checked = false;
    //    chkGroin2None.Checked = false;
    //    chkHip2None.Checked = false;
    //    chkThigh2None.Checked = false;
    //    chkLeg2None.Checked = false;
    //    chkKnee2None.Checked = false;
    //    chkAnkle2None.Checked = false;
    //    chkFeet2None.Checked = false;
    //    chkToe2None.Checked = false;
    //}

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

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        SaveDiagnosis(Session["PatientIE_ID2"].ToString());
        SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        SaveStandards(Session["PatientIE_ID2"].ToString());
        PopulateUI(Session["patientFUId"].ToString());

        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        Session["refresh_count"] = Convert.ToInt64(Session["refresh_count"]) + 1;
        _CurIEid = Session["PatientIE_ID2"].ToString();
        //  SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        // SaveStandards(Session["PatientIE_ID2"].ToString());
        _FuId = Session["patientFUId"].ToString();
        bindgridPoup();
        // Response.Redirect("AddFuDiagnosis.aspx");
    }

    public void bindDropdown()
    {
        //XmlDocument doc = new XmlDocument();
        //doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        //foreach (XmlNode node in doc.SelectNodes("//HSM/Levels/Level"))
        //{
        //    cboLevels.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        //}

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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/LowBack");
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
        SqlStr = "Select * from tblFUbpLowback WHERE PatientFU_ID = " + _FuId;
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
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Lowback.xml"));
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