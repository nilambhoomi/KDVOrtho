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

public partial class EditFuHip : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Hip";
    string Position = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();

    ILog log = log4net.LogManager.GetLogger(typeof(EditFuHip));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];

        Session["PageName"] = "Hip";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            BindROM();
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {
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
                            chkOberLeft.Enabled = true;
                            chkFaberLeft.Enabled = true;
                            chkTrendelenburgLeft.Enabled = true;
                            //Left checkbox
                            chkOberRight.Enabled = false;
                            chkFaberRight.Enabled = false;
                            chkTrendelenburgRight.Enabled = false;

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
                            chkOberLeft.Enabled = false;
                            chkFaberLeft.Enabled = false;
                            chkTrendelenburgLeft.Enabled = false;
                            //Left checkbox
                            chkOberRight.Enabled = true;
                            chkFaberRight.Enabled = true;
                            chkTrendelenburgRight.Enabled = true;
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
                            chkOberLeft.Enabled = true;
                            chkFaberLeft.Enabled = true;
                            chkTrendelenburgLeft.Enabled = true;
                            //Left checkbox
                            chkOberRight.Enabled = true;
                            chkFaberRight.Enabled = true;
                            chkTrendelenburgRight.Enabled = true;
                            break;
                    }
                }

                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpHip WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpHip WHERE PatientIE_ID= " + _CurIEid + "");
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
                Response.Redirect("EditFU.aspx");
            }
        }
       
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuHip for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }

    public string SaveUI(string fuID, string ieMode, bool bpIsChecked)
    {
        long _fuID = Convert.ToInt64(fuID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpHip WHERE PatientFU_ID = " + _fuID;
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

            TblRow["OberRight"] = chkOberRight.Checked;
            TblRow["FaberRight"] = chkFaberRight.Checked;
            TblRow["TrendelenburgRight"] = chkTrendelenburgRight.Checked;
            TblRow["OberLeft"] = chkOberLeft.Checked;
            TblRow["FaberLeft"] = chkFaberLeft.Checked;
            TblRow["TrendelenburgLeft"] = chkTrendelenburgLeft.Checked;

            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["CCvalue"] = hdCCvalue.Value;
           
            TblRow["PEvalue"] = hdPEvalue.Value;
            


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
            //TblRow["WorseActivitiesLeft"] = chkWorseActivitiesLeft.Checked;
            //TblRow["PainScaleRight"] = txtPainScaleRight.Text.ToString().ToString();
            //TblRow["SharpRight"] = chkSharpRight.Checked;
            //TblRow["ElectricRight"] = chkElectricRight.Checked;
            //TblRow["ShootingRight"] = chkShootingRight.Checked;
            //TblRow["ThrobblingRight"] = chkThrobblingRight.Checked;
            //TblRow["PulsatingRight"] = chkPulsatingRight.Checked;
            //TblRow["DullRight"] = chkDullRight.Checked;
            //TblRow["AchyRight"] = chkAchyRight.Checked;
            //TblRow["WorseMovementRight"] = chkWorseMovementRight.Checked;
            //TblRow["WorseActivitiesRight"] = chkWorseActivitiesRight.Checked;

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

            //TblRow["SprainStrainSide"] = cboSprainStrainSide.Text.ToString();
            //TblRow["SprainStrain"] = Convert.ToBoolean(chkSprainStrain.Checked);
            //TblRow["IntDerangementSide"] = cboIntDerangementSide.Text.ToString();
            //TblRow["IntDerangement"] = Convert.ToBoolean(chkIntDerangement.Checked);
            //TblRow["Scan"] = Convert.ToBoolean(chkScan.Checked);
            //TblRow["ScanType"] = cboScanType.Text.ToString();
            //TblRow["ScanSide"] = cboScanSide.Text.ToString();

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
            return "Hip has been added...";
        else if (_ieMode == "Update")
            return "Hip has been updated...";
        else if (_ieMode == "Delete")
            return "Hip has been deleted...";
        else
            return "";
    }
    public void PopulateUI(string fuID)
    {
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpHip WHERE PatientFU_ID = " + fuID;
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
           

            chkOberRight.Checked = CommonConvert.ToBoolean(TblRow["OberRight"].ToString());
            chkFaberRight.Checked = CommonConvert.ToBoolean(TblRow["FaberRight"].ToString());
            chkTrendelenburgRight.Checked = CommonConvert.ToBoolean(TblRow["TrendelenburgRight"].ToString());
            chkOberLeft.Checked = CommonConvert.ToBoolean(TblRow["OberLeft"].ToString());
            chkFaberLeft.Checked = CommonConvert.ToBoolean(TblRow["FaberLeft"].ToString());
            chkTrendelenburgLeft.Checked = CommonConvert.ToBoolean(TblRow["TrendelenburgLeft"].ToString());
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();


            if (!string.IsNullOrEmpty(sqlTbl.Rows[0]["CCvalue"].ToString())
                && sqlTbl.Rows[0]["CCvalue"].ToString() != "\r\n                ")
            {
                CF.InnerHtml = sqlTbl.Rows[0]["CCvalue"].ToString();

                //hdorgval.Value = sqlTbl.Rows[0]["CCvalueoriginal"].ToString();
            }
            else
            {
                bindCC(Position.ToUpper());
            }
            if (!string.IsNullOrEmpty(sqlTbl.Rows[0]["PEvalue"].ToString())
                && sqlTbl.Rows[0]["PEvalue"].ToString() != "\r\n                ")
            {
                divPE.InnerHtml = sqlTbl.Rows[0]["PEvalue"].ToString();

                //hdorgvalPE.Value = sqlTbl.Rows[0]["PEvalueoriginal"].ToString();
            }
            else
            {
                bindPE(Position.ToUpper());
            }




            string pos = Request.QueryString["P"];

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP('" + pos + "');", true);

          
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

            //chkContentLeft.Checked = CommonConvert.ToBoolean(TblRow["ConstantLeft"].ToString());
            //chkIntermittentLeft.Checked = CommonConvert.ToBoolean(TblRow["IntermittentLeft"].ToString());
            //chkContentRight.Checked = CommonConvert.ToBoolean(TblRow["ConstantRight"].ToString());
            //chkIntermittentRight.Checked = CommonConvert.ToBoolean(TblRow["IntermittentRight"].ToString());
            //chkWorseSittingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseSittingLeft"].ToString());
            //chkWorseStandingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseStandingLeft"].ToString());
            //chkWorseMovementLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementLeft"].ToString());
            //chkWorseActivitiesLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesLeft"].ToString());
            //txtWorseOtherLeft.Text = TblRow["WorseOtherLeft"].ToString().Trim();
            //chkWorseSittingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseSittingRight"].ToString());
            //chkWorseStandingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseStandingRight"].ToString());
            //chkWorseMovementRight.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementRight"].ToString());
            //chkWorseActivitiesRight.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesRight"].ToString());
            //txtWorseOtherRight.Text = TblRow["WorseOtherRight"].ToString().Trim();
            //chkGreaterTrochanterLeft.Checked = CommonConvert.ToBoolean(TblRow["GreaterTrochanterLeft"].ToString());
            //chkPosteriorLeft.Checked = CommonConvert.ToBoolean(TblRow["PosteriorLeft"].ToString());
            //chkIliotibialLeft.Checked = CommonConvert.ToBoolean(TblRow["IliotibialLeft"].ToString());
            //chkGreaterTrochanterRight.Checked = CommonConvert.ToBoolean(TblRow["GreaterTrochanterRight"].ToString());
            //chkPosteriorRight.Checked = CommonConvert.ToBoolean(TblRow["PosteriorRight"].ToString());
            //chkIliotibialRight.Checked = CommonConvert.ToBoolean(TblRow["IliotibialRight"].ToString());
            txtFlexRight.Text = TblRow["FlexRight"].ToString().Trim();
            txtIntRotationRight.Text = TblRow["IntRotationRight"].ToString().Trim();
            txtExtRotationRight.Text = TblRow["ExtRotationRight"].ToString().Trim();
            txtFlexLeft.Text = TblRow["FlexLeft"].ToString().Trim();
            txtIntRotationLeft.Text = TblRow["IntRotationLeft"].ToString().Trim();
            txtExtRotationLeft.Text = TblRow["ExtRotationLeft"].ToString().Trim();
            //txtFlexRightWas.Text = TblRow["FlexRight"].ToString().Trim();
            //txtIntRotationRightWas.Text = TblRow["IntRotationRight"].ToString().Trim();
            //txtExtRotationRightWas.Text = TblRow["ExtRotationRight"].ToString().Trim();
            //txtFlexLeftWas.Text = TblRow["FlexLeft"].ToString().Trim();
            //txtIntRotationLeftWas.Text = TblRow["IntRotationLeft"].ToString().Trim();
            //txtExtRotationLeftWas.Text = TblRow["ExtRotationLeft"].ToString().Trim();

            chkOberRight.Checked = CommonConvert.ToBoolean(TblRow["OberRight"].ToString());
            chkFaberRight.Checked = CommonConvert.ToBoolean(TblRow["FaberRight"].ToString());
            chkTrendelenburgRight.Checked = CommonConvert.ToBoolean(TblRow["TrendelenburgRight"].ToString());
            chkOberLeft.Checked = CommonConvert.ToBoolean(TblRow["OberLeft"].ToString());
            chkFaberLeft.Checked = CommonConvert.ToBoolean(TblRow["FaberLeft"].ToString());
            chkTrendelenburgLeft.Checked = CommonConvert.ToBoolean(TblRow["TrendelenburgLeft"].ToString());
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

            //txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString().Trim();
            //chkSharpLeft.Checked = CommonConvert.ToBoolean(TblRow["SharpLeft"].ToString());
            //chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            //chkShootingLeft.Checked = CommonConvert.ToBoolean(TblRow["ShootingLeft"].ToString());
            //chkThrobblingLeft.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingLeft"].ToString());
            //chkPulsatingLeft.Checked = CommonConvert.ToBoolean(TblRow["PulsatingLeft"].ToString());
            //chkDullLeft.Checked = CommonConvert.ToBoolean(TblRow["DullLeft"].ToString());
            //chkAchyLeft.Checked = CommonConvert.ToBoolean(TblRow["AchyLeft"].ToString());
            //chkWorseMovementLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementLeft"].ToString());
            //chkWorseActivitiesLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesLeft"].ToString());
            //txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString().Trim();
            //chkSharpRight.Checked = CommonConvert.ToBoolean(TblRow["SharpRight"].ToString());
            //chkElectricRight.Checked = CommonConvert.ToBoolean(TblRow["ElectricRight"].ToString());
            //chkShootingRight.Checked = CommonConvert.ToBoolean(TblRow["ShootingRight"].ToString());
            //chkThrobblingRight.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingRight"].ToString());
            //chkPulsatingRight.Checked = CommonConvert.ToBoolean(TblRow["PulsatingRight"].ToString());
            //chkDullRight.Checked = CommonConvert.ToBoolean(TblRow["DullRight"].ToString());
            //chkAchyRight.Checked = CommonConvert.ToBoolean(TblRow["AchyRight"].ToString());
            //chkWorseMovementRight.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementRight"].ToString());
            //chkWorseActivitiesRight.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesRight"].ToString());
            //if (TblRow["SprainStrainSide"] != null)
            //    cboSprainStrainSide.Text = TblRow["SprainStrainSide"].ToString().Trim();
            //chkSprainStrain.Checked = CommonConvert.ToBoolean(TblRow["SprainStrain"];
            //if (TblRow["IntDerangementSide"] != null)
            //    cboIntDerangementSide.Text = TblRow["IntDerangementSide"].ToString().Trim();
            //chkIntDerangement.Checked = CommonConvert.ToBoolean(TblRow["IntDerangement"];
            //chkScan.Checked = CommonConvert.ToBoolean(TblRow["Scan"];
            //if (TblRow["ScanType"] != null)
            //    cboScanType.Text = TblRow["ScanType"].ToString().Trim();
            //if (TblRow["ScanSide"] != null)
            //    cboScanSide.Text = TblRow["ScanSide"].ToString().Trim();
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
            //txtFlexRightWas.Text = node.SelectSingleNode("FlexRight") == null ? txtFlexRightWas.Text.ToString().Trim() : node.SelectSingleNode("FlexRight").InnerText;
            //txtIntRotationRightWas.Text = node.SelectSingleNode("IntRotationRight") == null ? txtIntRotationRightWas.ToString().Trim() : node.SelectSingleNode("IntRotationRight").InnerText;
            //txtExtRotationRightWas.Text = node.SelectSingleNode("ExtRotationRight") == null ? txtExtRotationRightWas.ToString().Trim() : node.SelectSingleNode("ExtRotationRight").InnerText;
            //txtFlexLeftWas.Text = node.SelectSingleNode("FlexLeft") == null ? txtFlexLeftWas.Text.ToString().Trim() : node.SelectSingleNode("FlexLeft").InnerText;
            //txtIntRotationLeftWas.Text = node.SelectSingleNode("IntRotationLeft") == null ? txtIntRotationLeftWas.ToString().Trim() : node.SelectSingleNode("IntRotationLeft").InnerText;
            //txtExtRotationLeftWas.Text = node.SelectSingleNode("ExtRotationLeft") == null ? txtExtRotationLeftWas.ToString().Trim() : node.SelectSingleNode("ExtRotationLeft").InnerText;
            chkOberRight.Checked = node.SelectSingleNode("OberRight") == null ? chkOberRight.Checked : Convert.ToBoolean(node.SelectSingleNode("OberRight").InnerText);
            chkFaberRight.Checked = node.SelectSingleNode("FaberRight") == null ? chkFaberRight.Checked : Convert.ToBoolean(node.SelectSingleNode("FaberRight").InnerText);
            chkTrendelenburgRight.Checked = node.SelectSingleNode("TrendelenburgRight") == null ? chkTrendelenburgRight.Checked : Convert.ToBoolean(node.SelectSingleNode("TrendelenburgRight").InnerText);
            chkOberLeft.Checked = node.SelectSingleNode("OberLeft") == null ? chkOberLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("OberLeft").InnerText);
            chkFaberLeft.Checked = node.SelectSingleNode("FaberLeft") == null ? chkFaberLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("FaberLeft").InnerText);
            chkTrendelenburgLeft.Checked = node.SelectSingleNode("TrendelenburgLeft") == null ? chkTrendelenburgLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("TrendelenburgLeft").InnerText);
            txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
            //cboSprainStrainSide.Text = node.SelectSingleNode("SprainStrainSide") == null ? cboSprainStrainSide.Text.ToString().Trim() : node.SelectSingleNode("SprainStrainSide").InnerText;
            //chkSprainStrain.Checked = node.SelectSingleNode("SprainStrain") == null ? chkSprainStrain.Checked : Convert.ToBoolean(node.SelectSingleNode("SprainStrain").InnerText);
            //cboIntDerangementSide.Text = node.SelectSingleNode("IntDerangementSide") == null ? cboIntDerangementSide.Text.ToString().Trim() : node.SelectSingleNode("IntDerangementSide").InnerText;
            //chkIntDerangement.Checked = node.SelectSingleNode("IntDerangement") == null ? chkIntDerangement.Checked : Convert.ToBoolean(node.SelectSingleNode("IntDerangement").InnerText);
            //chkScan.Checked = node.SelectSingleNode("Scan") == null ? chkScan.Checked : Convert.ToBoolean(node.SelectSingleNode("Scan").InnerText);
            //cboScanType.Text = node.SelectSingleNode("ScanType") == null ? cboScanType.Text.ToString().Trim() : node.SelectSingleNode("ScanType").InnerText;
            //cboScanSide.Text = node.SelectSingleNode("ScanSide") == null ? cboScanSide.Text.ToString().Trim() : node.SelectSingleNode("ScanSide").InnerText;
            _fldPop = false;
           
        }
        bindCC(Position.ToUpper());
        bindPE(Position.ToUpper());
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
        // long _StdID = Convert.ToInt64(iStdID);
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
        //SaveUI(Session["PatientFUID"].ToString(), ieMode, true);
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
        SaveUI(Session["PatientFUID"].ToString(), ieMode, true);
        _FuId = Session["patientFUId"].ToString();
        _CurIEid = Session["PatientIE_ID"].ToString();
        SaveDiagnosis(_CurIEid);
        // SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["PatientFUID"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
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
            SqlStr = "Select * from tblFUbpHip WHERE PatientFU_ID = " + _FuId;
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

    public void bindPE(string p)
    {
        string path = Server.MapPath("~/Template/HipPE.html");
        string body = File.ReadAllText(path);

        if (p == "L")
            body = body.Replace("#rigthtdiv", "style='display:none'");
        else if (p == "R")
            body = body.Replace("#leftdiv", "style='display:none'");


        divPE.InnerHtml = body;

    }

}