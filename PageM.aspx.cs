using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Globalization;

public partial class Page1 : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        //txt_DOA.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
        //txt_DOB.Text = "01/11/1990";
        if (!IsPostBack)
        {
            bindLocation();
            txt_DOV.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            if (Session["Location"] != null)
            {
                ddl_location.SelectedValue = Convert.ToString(Session["Location"]);
            }
            if (Request["id"] != null)
            {
                bindEditData(Request.QueryString["id"]);
            }
            else
            {
                if (Session["PatientIE_ID"] != null)
                    bindEditData(Session["PatientIE_ID"].ToString());
                else
                {
                    if (Session["Providers"] != null)
                    {
                        txtMAProviders.Text = Convert.ToString(Session["Providers"]);
                    }
                }
            }
            ddl_location.Focus();
        }
    }

    [WebMethod]
    public static string[] getFirstName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID,FirstName from tblPatientMaster where FirstName like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                fname = ds.Tables[0].Rows[i]["FirstName"].ToString();
                patient.Add(string.Format("{0}-{1}", fname, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }

        return patient.ToArray();
    }

    [WebMethod]
    public static string[] getLastName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID,LastName from tblPatientMaster where LastName like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                fname = ds.Tables[0].Rows[i]["LastName"].ToString();
                patient.Add(string.Format("{0}-{1}", fname, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }

        return patient.ToArray();
    }
    [WebMethod]
    public static string[] getInsComp(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> inscmp = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select InsCo_ID,InsCo from tblInsCos where InsCo like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string cmpname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmpname = ds.Tables[0].Rows[i]["InsCo"].ToString();
                inscmp.Add(string.Format("{0}-{1}", cmpname, ds.Tables[0].Rows[i]["InsCo_ID"].ToString()));
            }
        }

        return inscmp.ToArray();
    }
    [WebMethod]
    public static string[] getPharmacy(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> inscmp = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Pharmacy_ID,Pharmacy from tblPharmacy where Pharmacy like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string cmpname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                cmpname = ds.Tables[0].Rows[i]["Pharmacy"].ToString();
                inscmp.Add(string.Format("{0}-{1}", cmpname, ds.Tables[0].Rows[i]["Pharmacy_ID"].ToString()));
            }
        }

        return inscmp.ToArray();
    }
    [WebMethod]
    public static string[] getAttorney(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> inscmp = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Attorney_ID,Attorney from tblAttorneys where Attorney like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string attorney = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                attorney = ds.Tables[0].Rows[i]["Attorney"].ToString();
                inscmp.Add(string.Format("{0}-{1}", attorney, ds.Tables[0].Rows[i]["Attorney_ID"].ToString()));
            }
        }

        return inscmp.ToArray();
    }
    private void bindData()
    {

        DataSet ds = db.Patientmaster_autocomplete(txt_fname.Text, txt_lname.Text);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txt_mname.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
            txt_add.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
            txt_city.Text = ds.Tables[0].Rows[0]["City"].ToString();
            ddl_State.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
            txt_zip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
            txt_home_ph.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txt_mobile.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();

            ddlHandedness.ClearSelection();
            ddlHandedness.Items.FindByText(ds.Tables[0].Rows[0]["Handedness"].ToString()).Selected = true;

            txt_DOB.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
        }
    }
    private void bindEditData(string PatientIEid)
    {
        try
        {
            Session["PatientIE_ID"] = PatientIEid;
            string query = "select * from View_PatientIE VP left join tblAdjuster A on VP.Adjuster_Id=A.Adjuster_Id  where PatientIE_ID=" + PatientIEid;

            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_fname.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                txt_lname.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                txt_SSN.Text = ds.Tables[0].Rows[0]["SSN"].ToString();

                txt_attorney_ph.Text = ds.Tables[0].Rows[0]["Telephone"].ToString();
                txt_attorney.Text = ds.Tables[0].Rows[0]["Attorney"].ToString();

                txt_DOA.Text = ds.Tables[0].Rows[0]["DOA"] != DBNull.Value ? ds.Tables[0].Rows[0]["DOA"].ToString().Split(' ')[0] : "";
                txt_DOV.Text = ds.Tables[0].Rows[0]["DOE"] != DBNull.Value ? ds.Tables[0].Rows[0]["DOE"].ToString().Split(' ')[0] : "";

                txt_ins_co.Text = ds.Tables[0].Rows[0]["InsCo"].ToString();

                txt_claim.Text = ds.Tables[0].Rows[0]["ClaimNumber"].ToString();

                txtMAProviders.Text = ds.Tables[0].Rows[0]["MA_Providers"].ToString();

                ddl_location.ClearSelection();
                ddl_location.Items.FindByText(ds.Tables[0].Rows[0]["Location"].ToString()).Selected = true;

                ddl_gender.ClearSelection();
                ddl_gender.Items.FindByValue(ds.Tables[0].Rows[0]["Sex"].ToString()).Selected = true;

                ddl_casetype.ClearSelection();
                ddl_casetype.Items.FindByValue(ds.Tables[0].Rows[0]["Compensation"].ToString()).Selected = true;

                txt_mname.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                txt_add.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                txt_city.Text = ds.Tables[0].Rows[0]["City"].ToString();
                ddl_State.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                txt_zip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txt_policy.Text = ds.Tables[0].Rows[0]["policy_no"].ToString();
                txt_work_ph.Text = ds.Tables[0].Rows[0]["work_phone"].ToString();
                txt_mobile.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                txt_home_ph.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                txt_pharmacy_address.Text = ds.Tables[0].Rows[0]["PharmaAddress"].ToString();
                txt_pharmacy_name.Text = ds.Tables[0].Rows[0]["Pharmacy"].ToString();

                if (ds.Tables[0].Columns.Contains("Adjuster"))
                {
                    txt_adjuster.Text = ds.Tables[0].Rows[0]["Adjuster"].ToString();
                }
                txt_WCBGroup.Text = ds.Tables[0].Rows[0]["WCBGroup"].ToString();

                ddlHandedness.ClearSelection();
                ddlHandedness.Items.FindByText(ds.Tables[0].Rows[0]["Handedness"].ToString()).Selected = true;


                if (ds.Tables[0].Rows[0]["DOB"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString());
                    txt_DOB.Text = dob.ToString("MM/dd/yyyy");
                }


            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void txt_lname_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_fname.Text))
        {
            bindData();

        }
        txt_fname.Focus();
    }
    protected void txt_fname_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_lname.Text))
        {
            bindData();
            txt_mname.Focus();
        }
    }

    private void bindLocation()
    {
        DataSet ds = new DataSet();


        ds = db.selectData("select Location,Location_ID from tblLocations Order By Location");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- Location --", "0"));

            DataSet locds = new DataSet();
            locds.ReadXml(Server.MapPath("~/LocationXML.xml"));

            //if (locds.Tables[0].Rows[0][0].ToString() == System.DateTime.Now.ToString("MM-dd-yyyy"))
            //{
            ddl_location.ClearSelection();
            ddl_location.Items.FindByValue(locds.Tables[0].Rows[0][1].ToString()).Selected = true;
            //}

        }

        ds = db.selectData("SELECT DISTINCT Compensation FROM tblPatientIE WHERE (Compensation <> '') OR (Compensation <> NULL) Order By Compensation");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_casetype.DataValueField = "Compensation";
            ddl_casetype.DataTextField = "Compensation";

            ddl_casetype.DataSource = ds;
            ddl_casetype.DataBind();

            ddl_casetype.Items.Insert(0, new ListItem("-- Case Type --", "0"));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            string SP = "";
            SqlParameter[] param = null;

            if (Request["id"] != null || Session["PatientIE_ID"] != null)
            {

                param = new SqlParameter[32];
                SP = "usp_update_PatientIE";
            }
            else
            {
                param = new SqlParameter[31];
                SP = "usp_insert_PatientIE";
            }
            //   param[0] = new SqlParameter("@FirstName", node.SelectSingleNode("FirstName").InnerText);
            param[0] = new SqlParameter("@FirstName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase( txt_fname.Text.ToLower()));
            param[1] = new SqlParameter("@LastName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_lname.Text.ToLower()));
            param[2] = new SqlParameter("@MiddleName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_mname.Text.ToLower()));
            param[3] = new SqlParameter("@Sex", ddl_gender.SelectedItem.Value);
            param[4] = new SqlParameter("@Address", txt_add.Text);
            if (string.IsNullOrWhiteSpace(txt_DOB.Text))
            {
                param[5] = new SqlParameter("@DOB", DBNull.Value);
            }
            else
            {
                param[5] = new SqlParameter("@DOB", Convert.ToDateTime(txt_DOB.Text));
            }

            param[6] = new SqlParameter("@Phone", txt_home_ph.Text);
            param[7] = new SqlParameter("@City", txt_city.Text);
            param[8] = new SqlParameter("@State", ddl_State.SelectedValue.Trim());
            param[9] = new SqlParameter("@Zip", txt_zip.Text);
            param[10] = new SqlParameter("@Handedness", ddlHandedness.Text);
            param[11] = new SqlParameter("@Createdby", "");
            param[12] = new SqlParameter("@IncCo", txt_ins_co.Text);
            param[13] = new SqlParameter("@Attorney", txt_attorney.Text);
            param[14] = new SqlParameter("@Attorney_phone", txt_attorney_ph.Text);
            param[15] = new SqlParameter("@DOV", Convert.ToDateTime(txt_DOV.Text));

            if (string.IsNullOrWhiteSpace(txt_DOA.Text))
            {

                param[16] = new SqlParameter("@DOA", DBNull.Value);
            }
            else
            {
                param[16] = new SqlParameter("@DOA", Convert.ToDateTime(txt_DOA.Text));
            }

            param[17] = new SqlParameter("@Location", ddl_location.SelectedItem.Value);
            param[18] = new SqlParameter("@Claim", txt_claim.Text);
            param[19] = new SqlParameter("@Policy", txt_policy.Text);
            param[20] = new SqlParameter("@AGE", 20);
            param[21] = new SqlParameter("@SSN", txt_SSN.Text);
            param[22] = new SqlParameter("@Phone2", txt_mobile.Text);
            param[23] = new SqlParameter("@work_phone", txt_work_ph.Text);
            param[24] = new SqlParameter("@policy_no", txt_policy.Text);
            param[25] = new SqlParameter("@Compensation", ddl_casetype.SelectedItem.Value);
            param[26] = new SqlParameter("@Pharmacy", txt_pharmacy_name.Text.Trim());
            param[27] = new SqlParameter("@PharmaAddress", txt_pharmacy_address.Text.Trim());
            param[28] = new SqlParameter("@Adjuster", txt_adjuster.Text);
            param[29] = new SqlParameter("@WCBGroup", txt_WCBGroup.Text);
            param[30] = new SqlParameter("@MA_Providers", txtMAProviders.Text.Trim());
            string patientIE_ID = "";

            if (Request["id"] != null)
                //param[28] = new SqlParameter("@PatientIE_ID", Request.QueryString["id"]);
                patientIE_ID = Request.QueryString["id"];
            else if (Session["PatientIE_ID"] != null)
                //    param[28] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"]);
                patientIE_ID = Convert.ToString(Session["PatientIE_ID"]);


            if (!string.IsNullOrEmpty(patientIE_ID))
            {
                Session["PatientIE_ID"] = patientIE_ID;
                param[31] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"]);
            }

            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                if (Request["id"] == null)
                {
                    lblMessage.InnerHtml = "Patient IE Created successfuly.";
                    lblmess.Text = "Patient IE Created successfuly.";
                    lblmess.ForeColor = System.Drawing.Color.Green;
                    Session["PatientIE_ID"] = val;
                    defaultValues(Session["PatientIE_ID"].ToString());
                }
                else
                {
                    lblMessage.InnerHtml = "Patient IE Updated successfuly.";
                    lblmess.Text = "Patient IE Created successfuly.";
                    lblmess.ForeColor = System.Drawing.Color.Green;
                }
                lblMessage.Attributes.Add("style", "color:green");
                upMessage.Update();
                DataSet locds = new DataSet();
                locds.ReadXml(Server.MapPath("~/LocationXML.xml"));
                if (locds.Tables[0].Rows[0][0].ToString() != System.DateTime.Now.ToString("MM-dd-yyyy"))
                {
                    locds.Tables[0].Rows[0][0] = System.DateTime.Now.ToString("MM-dd-yyyy");
                    locds.Tables[0].Rows[0][1] = ddl_location.SelectedItem.Value;

                    locds.WriteXml(Server.MapPath("~/LocationXML.xml"));
                }


            }
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
        finally
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
            Response.Redirect("Page2.aspx");
        }
        //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
        //Response.Redirect("Page2.aspx");

    }
    public void defaultValues(string id)
    {
        try
        {
            string SP = "";
            SqlParameter[] param = null;

            //insert into the ankle table.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Ankle");
            //nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Elbow");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[48];
                SP = "usp_ankle_insert_DV";
                param[0] = new SqlParameter("@MedMalleolusLeft", node.SelectSingleNode("MedMalleolusLeft").InnerText);
                param[1] = new SqlParameter("@LatMalleolusLeft", node.SelectSingleNode("LatMalleolusLeft").InnerText);
                param[2] = new SqlParameter("@AchillesLeft", node.SelectSingleNode("AchillesLeft").InnerText);
                param[3] = new SqlParameter("@MedMalleolusRight", node.SelectSingleNode("MedMalleolusRight").InnerText);
                param[4] = new SqlParameter("@LatMalleolusRight", node.SelectSingleNode("LatMalleolusRight").InnerText);
                param[5] = new SqlParameter("@AchillesRight", node.SelectSingleNode("AchillesRight").InnerText);
                param[6] = new SqlParameter("@RangeOfMotionLeft", node.SelectSingleNode("RangeOfMotionLeft").InnerText);
                param[7] = new SqlParameter("@PalpationMedMalleolusLeft", node.SelectSingleNode("PalpationMedMalleolusLeft").InnerText);
                param[8] = new SqlParameter("@PalpationLatMalleolusLeft", node.SelectSingleNode("PalpationLatMalleolusLeft").InnerText);
                param[9] = new SqlParameter("@PalpationAchillesLeft", node.SelectSingleNode("PalpationAchillesLeft").InnerText);
                param[10] = new SqlParameter("@WorsePlantarLeft", node.SelectSingleNode("WorsePlantarLeft").InnerText);
                param[11] = new SqlParameter("@WorseDorsiLeft", node.SelectSingleNode("WorseDorsiLeft").InnerText);
                param[12] = new SqlParameter("@WorseEversionLeft", node.SelectSingleNode("WorseEversionLeft").InnerText);
                param[13] = new SqlParameter("@WorseInversionLeft", node.SelectSingleNode("WorseInversionLeft").InnerText);
                param[14] = new SqlParameter("@WorseExtensionLeft", node.SelectSingleNode("WorseExtensionLeft").InnerText);
                param[15] = new SqlParameter("@WorseAmbulationLeft", node.SelectSingleNode("WorseAmbulationLeft").InnerText);
                param[16] = new SqlParameter("@EdemaLeft", node.SelectSingleNode("EdemaLeft").InnerText);
                param[17] = new SqlParameter("@EcchymosisLeft", node.SelectSingleNode("EcchymosisLeft").InnerText);
                param[18] = new SqlParameter("@RangeOfMotionRight", node.SelectSingleNode("RangeOfMotionRight").InnerText);
                param[19] = new SqlParameter("@PalpationMedMalleolusRight", node.SelectSingleNode("PalpationMedMalleolusRight").InnerText);
                param[20] = new SqlParameter("@PalpationLatMalleolusRight", node.SelectSingleNode("PalpationLatMalleolusRight").InnerText);
                param[21] = new SqlParameter("@PalpationAchillesRight", node.SelectSingleNode("PalpationAchillesRight").InnerText);
                param[22] = new SqlParameter("@WorsePlantarRight", node.SelectSingleNode("WorsePlantarRight").InnerText);
                param[23] = new SqlParameter("@WorseDorsiRight", node.SelectSingleNode("WorseDorsiRight").InnerText);
                param[24] = new SqlParameter("@WorseEversionRight", node.SelectSingleNode("WorseEversionRight").InnerText);
                param[25] = new SqlParameter("@WorseInversionRight", node.SelectSingleNode("WorseInversionRight").InnerText);
                param[26] = new SqlParameter("@WorseExtensionRight", node.SelectSingleNode("WorseExtensionRight").InnerText);
                param[27] = new SqlParameter("@WorseAmbulationRight", node.SelectSingleNode("WorseAmbulationRight").InnerText);
                param[28] = new SqlParameter("@EdemaRight", node.SelectSingleNode("EdemaRight").InnerText);
                param[29] = new SqlParameter("@EcchymosisRight", node.SelectSingleNode("EcchymosisRight").InnerText);
                param[30] = new SqlParameter("@FreeForm ", node.SelectSingleNode("FreeForm ").InnerText);
                param[31] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[32] = new SqlParameter("@SprainStrainSide", node.SelectSingleNode("SprainStrainSide").InnerText);
                param[33] = new SqlParameter("@Contusion", node.SelectSingleNode("Contusion").InnerText);
                param[34] = new SqlParameter("@ContusionSide", node.SelectSingleNode("ContusionSide").InnerText);
                param[35] = new SqlParameter("@Fracture", node.SelectSingleNode("Fracture").InnerText);
                param[36] = new SqlParameter("@FractureSide", node.SelectSingleNode("FractureSide").InnerText);
                param[37] = new SqlParameter("@Ligamentous", node.SelectSingleNode("Ligamentous").InnerText);
                param[38] = new SqlParameter("@LigamentousSide", node.SelectSingleNode("LigamentousSide").InnerText);
                param[39] = new SqlParameter("@Scan", node.SelectSingleNode("Scan").InnerText);
                param[40] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[41] = new SqlParameter("@ScanSide", node.SelectSingleNode("ScanSide").InnerText);
                param[42] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[43] = new SqlParameter("@CreatedBy", "Default");
                param[44] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[45] = new SqlParameter("@FreeFormCC", "Default");
                param[46] = new SqlParameter("@FreeFormA", "Default");
                param[47] = new SqlParameter("@FreeFormP", "Default");
            }
            db.executeSP(SP, param);
            //insert into Elbow table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Elbow");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[32];
                SP = "usp_elbow_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("MedEpicondyleLeft", node.SelectSingleNode("MedEpicondyleLeft").InnerText);
                param[2] = new SqlParameter("LatEpicondyleLeft", node.SelectSingleNode("LatEpicondyleLeft").InnerText);
                param[3] = new SqlParameter("OlecranonLeft", node.SelectSingleNode("OlecranonLeft").InnerText);
                param[4] = new SqlParameter("MedEpicondyleRight", node.SelectSingleNode("MedEpicondyleRight").InnerText);
                param[5] = new SqlParameter("LatEpicondyleRight", node.SelectSingleNode("LatEpicondyleRight").InnerText);
                param[6] = new SqlParameter("OlecranonRight", node.SelectSingleNode("OlecranonRight").InnerText);
                param[7] = new SqlParameter("PalpationLeft", node.SelectSingleNode("PalpationLeft").InnerText);
                param[8] = new SqlParameter("RangeOfMotionLeft", node.SelectSingleNode("RangeOfMotionLeft").InnerText);
                param[9] = new SqlParameter("TinelLeft", node.SelectSingleNode("TinelLeft").InnerText);
                param[10] = new SqlParameter("PalpationRight", node.SelectSingleNode("PalpationRight").InnerText);
                param[11] = new SqlParameter("RangeOfMotionRight", node.SelectSingleNode("RangeOfMotionRight").InnerText);
                param[12] = new SqlParameter("TinelRight", node.SelectSingleNode("TinelRight").InnerText);
                param[13] = new SqlParameter("FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[14] = new SqlParameter("SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[15] = new SqlParameter("SprainStrainSide", node.SelectSingleNode("SprainStrainSide").InnerText);
                param[16] = new SqlParameter("contusion", node.SelectSingleNode("contusion").InnerText);
                param[17] = new SqlParameter("contusionSide", node.SelectSingleNode("contusionSide").InnerText);
                param[18] = new SqlParameter("fracture", node.SelectSingleNode("fracture").InnerText);
                param[19] = new SqlParameter("fractureSide", node.SelectSingleNode("fractureSide").InnerText);
                param[20] = new SqlParameter("LatEpicon", node.SelectSingleNode("LatEpicon").InnerText);
                param[21] = new SqlParameter("LatEpiconSide", node.SelectSingleNode("LatEpiconSide").InnerText);
                param[22] = new SqlParameter("MedEpicon", node.SelectSingleNode("MedEpicon").InnerText);
                param[23] = new SqlParameter("MedEpiconSide", node.SelectSingleNode("MedEpiconSide").InnerText);
                param[24] = new SqlParameter("Scan", node.SelectSingleNode("Scan").InnerText);
                param[25] = new SqlParameter("ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[26] = new SqlParameter("ScanSide", node.SelectSingleNode("ScanSide").InnerText);
                param[27] = new SqlParameter("CreatedBy", "Default");
                param[28] = new SqlParameter("CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[29] = new SqlParameter("FreeFormCC", "Default");
                param[30] = new SqlParameter("FreeFormA", "Default");
                param[31] = new SqlParameter("FreeFormP", "Default");
            }
            db.executeSP(SP, param);
            //insert into Hip table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Hip");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[42];
                SP = "usp_hip_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@WorseSittingLeft", node.SelectSingleNode("WorseSittingLeft").InnerText);
                param[2] = new SqlParameter("@WorseStandingLeft", node.SelectSingleNode("WorseStandingLeft").InnerText);
                param[3] = new SqlParameter("@WorseMovementLeft", node.SelectSingleNode("WorseMovementLeft").InnerText);
                param[4] = new SqlParameter("@WorseActivitiesLeft", node.SelectSingleNode("WorseActivitiesLeft").InnerText);
                param[5] = new SqlParameter("@WorseOtherLeft", node.SelectSingleNode("WorseOtherLeft").InnerText);
                param[6] = new SqlParameter("@WorseSittingRight", node.SelectSingleNode("WorseSittingRight").InnerText);
                param[7] = new SqlParameter("@WorseStandingRight", node.SelectSingleNode("WorseStandingRight").InnerText);
                param[8] = new SqlParameter("@WorseMovementRight", node.SelectSingleNode("WorseMovementRight").InnerText);
                param[9] = new SqlParameter("@WorseActivitiesRight", node.SelectSingleNode("WorseActivitiesRight").InnerText);
                param[10] = new SqlParameter("@WorseOtherRight", node.SelectSingleNode("WorseOtherRight").InnerText);
                param[11] = new SqlParameter("@GreaterTrochanterLeft", node.SelectSingleNode("GreaterTrochanterLeft").InnerText);
                param[12] = new SqlParameter("@PosteriorLeft", node.SelectSingleNode("PosteriorLeft").InnerText);
                param[13] = new SqlParameter("@IliotibialLeft", node.SelectSingleNode("IliotibialLeft").InnerText);
                param[14] = new SqlParameter("@GreaterTrochanterRight", node.SelectSingleNode("GreaterTrochanterRight").InnerText);
                param[15] = new SqlParameter("@PosteriorRight", node.SelectSingleNode("PosteriorRight").InnerText);
                param[16] = new SqlParameter("@IliotibialRight", node.SelectSingleNode("IliotibialRight").InnerText);
                param[17] = new SqlParameter("@FlexRight", node.SelectSingleNode("FlexRight").InnerText);
                param[18] = new SqlParameter("@FlexLeft", node.SelectSingleNode("FlexLeft").InnerText);
                param[19] = new SqlParameter("@IntRotationRight", node.SelectSingleNode("IntRotationRight").InnerText);
                param[20] = new SqlParameter("@IntRotationLeft", node.SelectSingleNode("IntRotationLeft").InnerText);
                param[21] = new SqlParameter("@ExtRotationRight", node.SelectSingleNode("ExtRotationRight").InnerText);
                param[22] = new SqlParameter("@ExtRotationLeft", node.SelectSingleNode("ExtRotationLeft").InnerText);
                param[23] = new SqlParameter("@OberRight", node.SelectSingleNode("OberRight").InnerText);
                param[24] = new SqlParameter("@FaberRight", node.SelectSingleNode("FaberRight").InnerText);
                param[25] = new SqlParameter("@TrendelenburgRight", node.SelectSingleNode("TrendelenburgRight").InnerText);
                param[26] = new SqlParameter("@OberLeft", node.SelectSingleNode("OberLeft").InnerText);
                param[27] = new SqlParameter("@FaberLeft", node.SelectSingleNode("FaberLeft").InnerText);
                param[28] = new SqlParameter("@TrendelenburgLeft", node.SelectSingleNode("TrendelenburgLeft").InnerText);
                param[29] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[30] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[31] = new SqlParameter("@SprainStrainSide", node.SelectSingleNode("SprainStrainSide").InnerText);
                param[32] = new SqlParameter("@IntDerangement", node.SelectSingleNode("IntDerangement").InnerText);
                param[33] = new SqlParameter("@IntDerangementSide", node.SelectSingleNode("IntDerangementSide").InnerText);
                param[34] = new SqlParameter("@Scan", node.SelectSingleNode("Scan").InnerText);
                param[35] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[36] = new SqlParameter("@ScanSide", node.SelectSingleNode("ScanSide").InnerText);
                param[37] = new SqlParameter("@CreatedBy", "Default");
                param[38] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[39] = new SqlParameter("@FreeFormCC", "Default");
                param[40] = new SqlParameter("@FreeFormA", "Default");
                param[41] = new SqlParameter("@FreeFormP", "Default");
            }
            db.executeSP(SP, param);

            //insert into Knee table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Knee");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[92];
                SP = "usp_Knee_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@PainScaleLeft", node.SelectSingleNode("PainScaleLeft").InnerText);
                param[2] = new SqlParameter("@SharpLeft", node.SelectSingleNode("SharpLeft").InnerText);
                param[3] = new SqlParameter("@ElectricLeft", node.SelectSingleNode("ElectricLeft").InnerText);
                param[4] = new SqlParameter("@ShootingLeft", node.SelectSingleNode("ShootingLeft").InnerText);
                param[5] = new SqlParameter("@ThrobblingLeft", node.SelectSingleNode("ThrobblingLeft").InnerText);
                param[6] = new SqlParameter("@PulsatingLeft", node.SelectSingleNode("PulsatingLeft").InnerText);
                param[7] = new SqlParameter("@DullLeft", node.SelectSingleNode("DullLeft").InnerText);
                param[8] = new SqlParameter("@AchyLeft", node.SelectSingleNode("AchyLeft").InnerText);
                param[9] = new SqlParameter("@WorseMovementLeft", node.SelectSingleNode("WorseMovementLeft").InnerText);
                param[10] = new SqlParameter("@WorseWalkingLeft", node.SelectSingleNode("WorseWalkingLeft").InnerText);
                param[11] = new SqlParameter("@WorseStairsLeft", node.SelectSingleNode("WorseStairsLeft").InnerText);
                param[12] = new SqlParameter("@WorseSquattingLeft", node.SelectSingleNode("WorseSquattingLeft").InnerText);
                param[13] = new SqlParameter("@WorseActivitiesLeft", node.SelectSingleNode("WorseActivitiesLeft").InnerText);
                param[14] = new SqlParameter("@WorseOtherLeft", node.SelectSingleNode("WorseOtherLeft").InnerText);
                param[15] = new SqlParameter("@WorseOtherTextLeft", node.SelectSingleNode("WorseOtherTextLeft").InnerText);
                param[16] = new SqlParameter("@ImprovedRestingLeft", node.SelectSingleNode("ImprovedRestingLeft").InnerText);
                param[17] = new SqlParameter("@ImprovedMedicationLeft", node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
                param[18] = new SqlParameter("@ImprovedTherapyLeft", node.SelectSingleNode("ImprovedTherapyLeft").InnerText);
                param[19] = new SqlParameter("@ImprovedSleepingLeft", node.SelectSingleNode("ImprovedSleepingLeft").InnerText);
                param[20] = new SqlParameter("@PainScaleRight", node.SelectSingleNode("PainScaleRight").InnerText);
                param[21] = new SqlParameter("@SharpRight", node.SelectSingleNode("SharpRight").InnerText);
                param[22] = new SqlParameter("@ElectricRight", node.SelectSingleNode("ElectricRight").InnerText);
                param[23] = new SqlParameter("@ShootingRight", node.SelectSingleNode("ShootingRight").InnerText);
                param[24] = new SqlParameter("@ThrobblingRight", node.SelectSingleNode("ThrobblingRight").InnerText);
                param[25] = new SqlParameter("@PulsatingRight", node.SelectSingleNode("PulsatingRight").InnerText);
                param[26] = new SqlParameter("@DullRight", node.SelectSingleNode("DullRight").InnerText);
                param[27] = new SqlParameter("@AchyRight", node.SelectSingleNode("AchyRight").InnerText);
                param[28] = new SqlParameter("@WorseMovementRight", node.SelectSingleNode("WorseMovementRight").InnerText);
                param[29] = new SqlParameter("@WorseWalkingRight", node.SelectSingleNode("WorseWalkingRight").InnerText);
                param[30] = new SqlParameter("@WorseStairsRight", node.SelectSingleNode("WorseStairsRight").InnerText);
                param[31] = new SqlParameter("@WorseSquattingRight", node.SelectSingleNode("WorseSquattingRight").InnerText);
                param[32] = new SqlParameter("@WorseActivitiesRight", node.SelectSingleNode("WorseActivitiesRight").InnerText);
                param[33] = new SqlParameter("@WorseOtherRight", node.SelectSingleNode("WorseOtherRight").InnerText);
                param[34] = new SqlParameter("@WorseOtherTextRight", node.SelectSingleNode("WorseOtherTextRight").InnerText);
                param[35] = new SqlParameter("@ImprovedRestingRight", node.SelectSingleNode("ImprovedRestingRight").InnerText);
                param[36] = new SqlParameter("@ImprovedMedicationRight", node.SelectSingleNode("ImprovedMedicationRight").InnerText);
                param[37] = new SqlParameter("@ImprovedTherapyRight", node.SelectSingleNode("ImprovedTherapyRight").InnerText);
                param[38] = new SqlParameter("@ImprovedSleepingRight", node.SelectSingleNode("ImprovedSleepingRight").InnerText);
                param[39] = new SqlParameter("@LEExtensionRight", node.SelectSingleNode("LEExtensionRight").InnerText);
                param[40] = new SqlParameter("@LEFlexionRight", node.SelectSingleNode("LEFlexionRight").InnerText);
                param[41] = new SqlParameter("@LEExtensionLeft", node.SelectSingleNode("LEExtensionLeft").InnerText);
                param[42] = new SqlParameter("@LEFlexionLeft", node.SelectSingleNode("LEFlexionLeft").InnerText);
                param[43] = new SqlParameter("@PalpationOfLeft", node.SelectSingleNode("PalpationOfLeft").InnerText);
                param[44] = new SqlParameter("@PalpationText1Left", node.SelectSingleNode("PalpationText1Left").InnerText);
                param[45] = new SqlParameter("@PalpationText2Left", node.SelectSingleNode("PalpationText2Left").InnerText);
                param[46] = new SqlParameter("@MedialLeft", node.SelectSingleNode("MedialLeft").InnerText);
                param[47] = new SqlParameter("@LateralLeft", node.SelectSingleNode("LateralLeft").InnerText);
                param[48] = new SqlParameter("@SuperiorLeft", node.SelectSingleNode("SuperiorLeft").InnerText);
                param[49] = new SqlParameter("@InferiorLeft", node.SelectSingleNode("InferiorLeft").InnerText);
                param[50] = new SqlParameter("@SupermedialLeft", node.SelectSingleNode("SupermedialLeft").InnerText);
                param[51] = new SqlParameter("@SuperoLateralLeft", node.SelectSingleNode("SuperoLateralLeft").InnerText);
                param[52] = new SqlParameter("@InferomedialLeft", node.SelectSingleNode("InferomedialLeft").InnerText);
                param[53] = new SqlParameter("@InferoLateralLeft", node.SelectSingleNode("InferoLateralLeft").InnerText);
                param[54] = new SqlParameter("@PeripatellarLeft", node.SelectSingleNode("PeripatellarLeft").InnerText);
                param[55] = new SqlParameter("@PalpationOfRight", node.SelectSingleNode("PalpationOfRight").InnerText);
                param[56] = new SqlParameter("@PalpationText1Right", node.SelectSingleNode("PalpationText1Right").InnerText);
                param[57] = new SqlParameter("@PalpationText2Right", node.SelectSingleNode("PalpationText2Right").InnerText);
                param[58] = new SqlParameter("@MedialRight", node.SelectSingleNode("MedialRight").InnerText);
                param[59] = new SqlParameter("@LateralRight", node.SelectSingleNode("LateralRight").InnerText);
                param[60] = new SqlParameter("@SuperiorRight", node.SelectSingleNode("SuperiorRight").InnerText);
                param[61] = new SqlParameter("@InferiorRight", node.SelectSingleNode("InferiorRight").InnerText);
                param[62] = new SqlParameter("@SupermedialRight", node.SelectSingleNode("SupermedialRight").InnerText);
                param[63] = new SqlParameter("@SuperoLateralRight", node.SelectSingleNode("SuperoLateralRight").InnerText);
                param[64] = new SqlParameter("@InferomedialRight", node.SelectSingleNode("InferomedialRight").InnerText);
                param[65] = new SqlParameter("@InferoLateralRight", node.SelectSingleNode("InferoLateralRight").InnerText);
                param[66] = new SqlParameter("@PeripatellarRight", node.SelectSingleNode("PeripatellarRight").InnerText);
                param[67] = new SqlParameter("@McMurrayRight", node.SelectSingleNode("McMurrayRight").InnerText);
                param[68] = new SqlParameter("@LachmanRight", node.SelectSingleNode("LachmanRight").InnerText);
                param[69] = new SqlParameter("@AnteriorRight", node.SelectSingleNode("AnteriorRight").InnerText);
                param[70] = new SqlParameter("@PosteriorRight", node.SelectSingleNode("PosteriorRight").InnerText);
                param[71] = new SqlParameter("@VarusRight", node.SelectSingleNode("VarusRight").InnerText);
                param[72] = new SqlParameter("@ValgusRight", node.SelectSingleNode("ValgusRight").InnerText);
                param[73] = new SqlParameter("@McMurrayLeft", node.SelectSingleNode("McMurrayLeft").InnerText);
                param[74] = new SqlParameter("@LachmanLeft", node.SelectSingleNode("LachmanLeft").InnerText);
                param[75] = new SqlParameter("@AnteriorLeft", node.SelectSingleNode("AnteriorLeft").InnerText);
                param[76] = new SqlParameter("@PosteriorLeft", node.SelectSingleNode("PosteriorLeft").InnerText);
                param[77] = new SqlParameter("@VarusLeft", node.SelectSingleNode("VarusLeft").InnerText);
                param[78] = new SqlParameter("@ValgusLeft", node.SelectSingleNode("ValgusLeft").InnerText);
                param[79] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[80] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[81] = new SqlParameter("@SprainStrainSide", node.SelectSingleNode("SprainStrainSide").InnerText);
                param[82] = new SqlParameter("@Derangment", node.SelectSingleNode("Derangment").InnerText);
                param[83] = new SqlParameter("@DerangmentSide", node.SelectSingleNode("DerangmentSide").InnerText);
                param[84] = new SqlParameter("@Plan", node.SelectSingleNode("Plan").InnerText);
                param[85] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[86] = new SqlParameter("@ScanSide", node.SelectSingleNode("ScanSide").InnerText);
                param[87] = new SqlParameter("@CreatedBy", "Default");
                param[88] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[89] = new SqlParameter("@FreeFormCC", "Default");
                param[90] = new SqlParameter("@FreeFormA", "Default");
                param[91] = new SqlParameter("@FreeFormP", "Default");
            }
            db.executeSP(SP, param);

            //insert into LowBack table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/LowBack");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[141];
                SP = "usp_LowBack_insert_DV";

                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@PainScale", node.SelectSingleNode("PainScale").InnerText);
                param[2] = new SqlParameter("@Sharp", node.SelectSingleNode("Sharp").InnerText);
                param[3] = new SqlParameter("@Electric", node.SelectSingleNode("Electric").InnerText);
                param[4] = new SqlParameter("@Shooting", node.SelectSingleNode("Shooting").InnerText);
                param[5] = new SqlParameter("@Throbbling", node.SelectSingleNode("Throbbling").InnerText);
                param[6] = new SqlParameter("@Pulsating", node.SelectSingleNode("Pulsating").InnerText);
                param[7] = new SqlParameter("@Dull", node.SelectSingleNode("Dull").InnerText);
                param[8] = new SqlParameter("@Achy", node.SelectSingleNode("Achy").InnerText);


                param[9] = new SqlParameter("@Numbness", node.SelectSingleNode("Numbness").InnerText);

                param[10] = new SqlParameter("@Tingling", false);
                param[11] = new SqlParameter("@Burning", node.SelectSingleNode("Burning").InnerText);

                param[12] = new SqlParameter("@SideLeft1", node.SelectSingleNode("SideLeft1").InnerText);
                param[13] = new SqlParameter("@SideRight1", node.SelectSingleNode("SideRight1").InnerText);
                param[14] = new SqlParameter("@SideBilateral1", node.SelectSingleNode("SideBilateral1").InnerText);
                param[15] = new SqlParameter("@ButtockLeft1", node.SelectSingleNode("ButtockLeft1").InnerText);

                param[16] = new SqlParameter("@ButtockRight1", node.SelectSingleNode("ButtockRight1").InnerText);
                param[17] = new SqlParameter("@ButtockBilateral1", node.SelectSingleNode("ButtockBilateral1").InnerText);
                param[18] = new SqlParameter("@GroinLeft1", node.SelectSingleNode("GroinLeft1").InnerText);
                param[19] = new SqlParameter("@GroinRight1", node.SelectSingleNode("GroinRight1").InnerText);
                param[20] = new SqlParameter("@GroinBilateral1", node.SelectSingleNode("GroinBilateral1").InnerText);
                param[21] = new SqlParameter("@HipLeft1", node.SelectSingleNode("HipLeft1").InnerText);
                param[22] = new SqlParameter("@HipRight1", node.SelectSingleNode("HipRight1").InnerText);
                param[23] = new SqlParameter("@HipBilateral1", node.SelectSingleNode("HipBilateral1").InnerText);
                param[24] = new SqlParameter("@ThighLeft1", node.SelectSingleNode("ThighLeft1").InnerText);
                param[25] = new SqlParameter("@ThighRight1", node.SelectSingleNode("ThighRight1").InnerText);
                param[26] = new SqlParameter("@ThighBilateral1", node.SelectSingleNode("ThighBilateral1").InnerText);
                param[27] = new SqlParameter("@LegLeft1", node.SelectSingleNode("LegLeft1").InnerText);
                param[28] = new SqlParameter("@LegRight1", node.SelectSingleNode("LegRight1").InnerText);
                param[29] = new SqlParameter("@LegBilateral1", false);
                param[30] = new SqlParameter("@KneeLeft1", node.SelectSingleNode("KneeLeft1").InnerText);
                param[31] = new SqlParameter("@KneeRight1", node.SelectSingleNode("KneeRight1").InnerText);
                param[32] = new SqlParameter("@KneeBilateral1", node.SelectSingleNode("KneeBilateral1").InnerText);
                param[33] = new SqlParameter("@AnkleLeft1", node.SelectSingleNode("AnkleLeft1").InnerText);
                param[34] = new SqlParameter("@AnkleRight1", node.SelectSingleNode("AnkleRight1").InnerText);
                param[35] = new SqlParameter("@AnkleBilateral1", node.SelectSingleNode("AnkleBilateral1").InnerText);
                param[36] = new SqlParameter("@FeetLeft1", node.SelectSingleNode("FeetLeft1").InnerText);

                param[37] = new SqlParameter("@FeetRight1", node.SelectSingleNode("FeetRight1").InnerText);
                param[38] = new SqlParameter("@FeetBilateral1", node.SelectSingleNode("FeetBilateral1").InnerText);
                param[39] = new SqlParameter("@ToeLeft1", node.SelectSingleNode("ToeLeft1").InnerText);
                param[40] = new SqlParameter("@ToeRight1", node.SelectSingleNode("ToeRight1").InnerText);
                param[41] = new SqlParameter("@ToeBilateral1", node.SelectSingleNode("ToeBilateral1").InnerText);
                param[42] = new SqlParameter("@SideLeft2", node.SelectSingleNode("SideLeft2").InnerText);
                param[43] = new SqlParameter("@SideRight2", node.SelectSingleNode("SideRight2").InnerText);
                param[44] = new SqlParameter("@SideBilateral2", node.SelectSingleNode("SideBilateral2").InnerText);
                param[45] = new SqlParameter("@ButtockLeft2", node.SelectSingleNode("ButtockLeft2").InnerText);
                param[46] = new SqlParameter("@ButtockRight2", node.SelectSingleNode("ButtockRight2").InnerText);
                param[47] = new SqlParameter("@ButtockBilateral2", node.SelectSingleNode("ButtockBilateral2").InnerText);
                param[48] = new SqlParameter("@GroinLeft2", node.SelectSingleNode("GroinLeft2").InnerText);
                param[49] = new SqlParameter("@GroinRight2", node.SelectSingleNode("GroinRight2").InnerText);
                param[50] = new SqlParameter("@GroinBilateral2", node.SelectSingleNode("GroinBilateral2").InnerText);

                param[51] = new SqlParameter("@HipLeft2", node.SelectSingleNode("HipLeft2").InnerText);
                param[52] = new SqlParameter("@HipRight2", node.SelectSingleNode("HipRight2").InnerText);
                param[53] = new SqlParameter("@HipBilateral2", node.SelectSingleNode("HipBilateral2").InnerText);
                param[54] = new SqlParameter("@ThighLeft2", node.SelectSingleNode("ThighLeft2").InnerText);
                param[55] = new SqlParameter("@ThighRight2", node.SelectSingleNode("ThighRight2").InnerText);
                param[56] = new SqlParameter("@ThighBilateral2", node.SelectSingleNode("ThighBilateral2").InnerText);
                param[57] = new SqlParameter("@LegLeft2", node.SelectSingleNode("LegLeft2").InnerText);
                param[58] = new SqlParameter("@LegRight2", node.SelectSingleNode("LegRight2").InnerText);
                param[59] = new SqlParameter("@LegBilateral2", false);
                param[60] = new SqlParameter("@KneeLeft2", node.SelectSingleNode("KneeLeft2").InnerText);
                param[61] = new SqlParameter("@KneeRight2", node.SelectSingleNode("KneeRight2").InnerText);
                param[62] = new SqlParameter("@KneeBilateral2", node.SelectSingleNode("KneeBilateral2").InnerText);
                param[63] = new SqlParameter("@AnkleLeft2", node.SelectSingleNode("AnkleLeft2").InnerText);
                param[64] = new SqlParameter("@AnkleRight2", node.SelectSingleNode("AnkleRight2").InnerText);
                param[65] = new SqlParameter("@AnkleBilateral2", node.SelectSingleNode("AnkleBilateral2").InnerText);
                param[66] = new SqlParameter("@FeetLeft2", node.SelectSingleNode("FeetLeft2").InnerText);
                param[67] = new SqlParameter("@FeetRight2", node.SelectSingleNode("FeetRight2").InnerText);
                param[68] = new SqlParameter("@FeetBilateral2", node.SelectSingleNode("FeetBilateral2").InnerText);
                param[69] = new SqlParameter("@ToeLeft2", node.SelectSingleNode("ToeLeft2").InnerText);
                param[70] = new SqlParameter("@ToeRight2", node.SelectSingleNode("ToeRight2").InnerText);
                param[71] = new SqlParameter("@ToeBilateral2", node.SelectSingleNode("ToeBilateral2").InnerText);


                param[72] = new SqlParameter("@WorseSitting", node.SelectSingleNode("WorseSitting").InnerText);
                param[73] = new SqlParameter("@WorseStanding", node.SelectSingleNode("WorseStanding").InnerText);
                param[74] = new SqlParameter("@WorseLyingDown", node.SelectSingleNode("WorseLyingDown").InnerText);
                param[75] = new SqlParameter("@WorseMovement", node.SelectSingleNode("WorseMovement").InnerText);
                param[76] = new SqlParameter("@WorseBending", node.SelectSingleNode("WorseBending").InnerText);
                param[77] = new SqlParameter("@WorseLifting", node.SelectSingleNode("WorseLifting").InnerText);
                param[78] = new SqlParameter("@WorseSeatingtoStandingUp", node.SelectSingleNode("WorseSeatingtoStandingUp").InnerText);
                param[79] = new SqlParameter("@WorseWalking", node.SelectSingleNode("WorseWalking").InnerText);
                param[80] = new SqlParameter("@WorseClimbingStairs", node.SelectSingleNode("WorseClimbingStairs").InnerText);
                param[81] = new SqlParameter("@WorseDescendingStairs", node.SelectSingleNode("WorseDescendingStairs").InnerText);
                param[82] = new SqlParameter("@WorseDriving", node.SelectSingleNode("WorseDriving").InnerText);
                param[83] = new SqlParameter("@WorseWorking", node.SelectSingleNode("WorseWorking").InnerText);
                param[84] = new SqlParameter("@WorseOther", node.SelectSingleNode("WorseOther").InnerText);

                param[85] = new SqlParameter("@ImprovedResting", node.SelectSingleNode("ImprovedResting").InnerText);
                param[86] = new SqlParameter("@ImprovedMedication", node.SelectSingleNode("ImprovedMedication").InnerText);
                param[87] = new SqlParameter("@ImprovedTherapy", node.SelectSingleNode("ImprovedTherapy").InnerText);
                param[88] = new SqlParameter("@ImprovedSleeping", node.SelectSingleNode("ImprovedSleeping").InnerText);
                param[89] = new SqlParameter("@ImprovedMovement", node.SelectSingleNode("ImprovedMovement").InnerText);
                param[90] = new SqlParameter("@FwdFlex", node.SelectSingleNode("FwdFlex").InnerText);
                param[91] = new SqlParameter("@Extension", node.SelectSingleNode("Extension").InnerText);
                param[92] = new SqlParameter("@RotationRight", node.SelectSingleNode("RotationRight").InnerText);
                param[93] = new SqlParameter("@RotationLeft", node.SelectSingleNode("RotationLeft").InnerText);

                param[94] = new SqlParameter("@LateralFlexRight", node.SelectSingleNode("LateralFlexRight").InnerText);
                param[95] = new SqlParameter("@LateralFlexLeft", node.SelectSingleNode("LateralFlexLeft").InnerText);


                param[96] = new SqlParameter("@LegRaisedExamLeft", node.SelectSingleNode("LegRaisedExamLeft").InnerText);
                param[97] = new SqlParameter("@LegRaisedExamRight", node.SelectSingleNode("LegRaisedExamRight").InnerText);
                param[98] = new SqlParameter("@LegRaisedExamBilateral", node.SelectSingleNode("LegRaisedExamBilateral").InnerText);
                param[99] = new SqlParameter("@BraggardLeft", node.SelectSingleNode("BraggardLeft").InnerText);
                param[100] = new SqlParameter("@BraggardRight", node.SelectSingleNode("BraggardRight").InnerText);
                param[101] = new SqlParameter("@BraggardBilateral", node.SelectSingleNode("BraggardBilateral").InnerText);
                param[102] = new SqlParameter("@KernigLeft", node.SelectSingleNode("KernigLeft").InnerText);
                param[103] = new SqlParameter("@KernigRight", node.SelectSingleNode("KernigRight").InnerText);
                param[104] = new SqlParameter("@KernigBilateral", node.SelectSingleNode("KernigBilateral").InnerText);
                param[105] = new SqlParameter("@BrudzinskiLeft", node.SelectSingleNode("BrudzinskiLeft").InnerText);
                param[106] = new SqlParameter("@BrudzinskiRight", node.SelectSingleNode("BrudzinskiRight").InnerText);
                param[107] = new SqlParameter("@BrudzinskiBilateral", node.SelectSingleNode("BrudzinskiBilateral").InnerText);
                param[108] = new SqlParameter("@SacroiliacLeft", node.SelectSingleNode("SacroiliacLeft").InnerText);

                param[109] = new SqlParameter("@SacroiliacRight", node.SelectSingleNode("SacroiliacRight").InnerText);
                param[110] = new SqlParameter("@SacroiliacBilateral", node.SelectSingleNode("SacroiliacBilateral").InnerText);
                param[111] = new SqlParameter("@SacralNotchLeft", node.SelectSingleNode("SacralNotchLeft").InnerText);
                param[112] = new SqlParameter("@SacralNotchRight", node.SelectSingleNode("SacralNotchRight").InnerText);
                param[113] = new SqlParameter("@SacralNotchBilateral", node.SelectSingleNode("SacralNotchBilateral").InnerText);
                param[114] = new SqlParameter("@OberLeft", node.SelectSingleNode("OberLeft").InnerText);
                param[115] = new SqlParameter("@OberRight", node.SelectSingleNode("OberRight").InnerText);
                param[116] = new SqlParameter("@OberBilateral", node.SelectSingleNode("OberBilateral").InnerText);
                param[117] = new SqlParameter("@TPSide", node.SelectSingleNode("TPSide").InnerText);
                param[118] = new SqlParameter("@TPText", node.SelectSingleNode("TPText").InnerText);

                param[119] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[120] = new SqlParameter("@Herniation", node.SelectSingleNode("Herniation").InnerText);
                param[121] = new SqlParameter("@Syndrome", node.SelectSingleNode("Syndrome").InnerText);
                param[122] = new SqlParameter("@Sacroilitis", node.SelectSingleNode("Sacroilitis").InnerText);
                param[123] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[124] = new SqlParameter("@LumberSpine", node.SelectSingleNode("LumberSpine").InnerText);
                param[125] = new SqlParameter("@EMG_NCV", node.SelectSingleNode("EMG_x002F_NCV").InnerText);

                param[126] = new SqlParameter("@Areas", node.SelectSingleNode("Areas").InnerText);
                param[127] = new SqlParameter("@CreatedBy", "Default");
                param[128] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[129] = new SqlParameter("@RuleOut", node.SelectSingleNode("RuleOut").InnerText);

                param[130] = new SqlParameter("@FreeFormA", node.SelectSingleNode("FreeFormA").InnerText);

                param[131] = new SqlParameter("@Radiates", node.SelectSingleNode("Radiates").InnerText);
                param[132] = new SqlParameter("@BurningTo", node.SelectSingleNode("BurningTo").InnerText);
                param[133] = new SqlParameter("@WeeknessIn", node.SelectSingleNode("WeeknessIn").InnerText);
                param[134] = new SqlParameter("@WorseOtherText", node.SelectSingleNode("WorseOtherText").InnerText);
                param[135] = new SqlParameter("@PalpationAt", "1");
                param[136] = new SqlParameter("@Levels", node.SelectSingleNode("Levels").InnerText);
                param[137] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[138] = new SqlParameter("@Modalities", node.SelectSingleNode("Modalities").InnerText);
                param[139] = new SqlParameter("@FreeFormCC", node.SelectSingleNode("FreeFormCC").InnerText);
                param[140] = new SqlParameter("@FreeFormP", node.SelectSingleNode("FreeFormP").InnerText);
            }
            var temp = db.executeSP(SP, param);

            //insert into MidBack table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/MidBack");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[49];
                SP = "usp_MidBack_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@PainScale", node.SelectSingleNode("PainScale").InnerText);
                param[2] = new SqlParameter("@Sharp", node.SelectSingleNode("Sharp").InnerText);
                param[3] = new SqlParameter("@Electric", node.SelectSingleNode("Electric").InnerText);
                param[4] = new SqlParameter("@Shooting", node.SelectSingleNode("Shooting").InnerText);
                param[5] = new SqlParameter("@Throbbling", node.SelectSingleNode("Throbbling").InnerText);
                param[6] = new SqlParameter("@Pulsating", node.SelectSingleNode("Pulsating").InnerText);
                param[7] = new SqlParameter("@Dull", node.SelectSingleNode("Dull").InnerText);
                param[8] = new SqlParameter("@Achy", node.SelectSingleNode("Achy").InnerText);
                param[9] = new SqlParameter("@Radiates", node.SelectSingleNode("Radiates").InnerText);
                param[10] = new SqlParameter("@WorseSitting", node.SelectSingleNode("WorseSitting").InnerText);
                param[11] = new SqlParameter("@WorseStanding", node.SelectSingleNode("WorseStanding").InnerText);
                param[12] = new SqlParameter("@WorseLyingDown", node.SelectSingleNode("WorseLyingDown").InnerText);
                param[13] = new SqlParameter("@WorseMovement", node.SelectSingleNode("WorseMovement").InnerText);
                param[14] = new SqlParameter("@WorseBending", node.SelectSingleNode("WorseBending").InnerText);
                param[15] = new SqlParameter("@WorseLifting", node.SelectSingleNode("WorseLifting").InnerText);

                param[16] = new SqlParameter("@WorseSeatingtoStandingUp", node.SelectSingleNode("WorseSeatingtoStandingUp").InnerText);
                param[17] = new SqlParameter("@WorseWalking", node.SelectSingleNode("WorseWalking").InnerText);
                param[18] = new SqlParameter("@WorseClimbingStairs", node.SelectSingleNode("WorseClimbingStairs").InnerText);
                param[19] = new SqlParameter("@WorseDescendingStairs", node.SelectSingleNode("WorseDescendingStairs").InnerText);
                param[20] = new SqlParameter("@WorseDriving", node.SelectSingleNode("WorseDriving").InnerText);
                param[21] = new SqlParameter("@WorseWorking", node.SelectSingleNode("WorseWorking").InnerText);
                param[22] = new SqlParameter("@WorseOther", node.SelectSingleNode("WorseOther").InnerText);
                param[23] = new SqlParameter("@WorseOtherText", node.SelectSingleNode("WorseOtherText").InnerText);
                param[24] = new SqlParameter("@ImprovedResting", node.SelectSingleNode("ImprovedResting").InnerText);
                param[25] = new SqlParameter("@ImprovedMedication", node.SelectSingleNode("ImprovedMedication").InnerText);
                param[26] = new SqlParameter("@ImprovedTherapy", node.SelectSingleNode("ImprovedTherapy").InnerText);
                param[27] = new SqlParameter("@ImprovedSleeping", node.SelectSingleNode("ImprovedSleeping").InnerText);
                param[28] = new SqlParameter("@ImprovedMovement", node.SelectSingleNode("ImprovedMovement").InnerText);
                param[29] = new SqlParameter("@Levels", node.SelectSingleNode("Levels").InnerText);
                param[30] = new SqlParameter("@ROM", node.SelectSingleNode("ROM").InnerText);
                param[31] = new SqlParameter("@TPSide1", node.SelectSingleNode("TPSide1").InnerText);
                param[32] = new SqlParameter("@TPText1", node.SelectSingleNode("TPText1").InnerText);
                param[33] = new SqlParameter("@TPSide2", node.SelectSingleNode("TPSide2").InnerText);

                param[34] = new SqlParameter("@TPText2", node.SelectSingleNode("TPText2").InnerText);
                param[35] = new SqlParameter("@TPSide3", node.SelectSingleNode("TPSide3").InnerText);
                param[36] = new SqlParameter("@TPText3", node.SelectSingleNode("TPText3").InnerText);
                param[37] = new SqlParameter("@TPSide4", node.SelectSingleNode("TPSide4").InnerText);
                param[38] = new SqlParameter("@TPText4", node.SelectSingleNode("TPText4").InnerText);
                param[39] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[40] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[41] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[42] = new SqlParameter("@ThoracicSpine", node.SelectSingleNode("ThoracicSpine").InnerText);
                param[43] = new SqlParameter("@CreatedBy", "Default");
                param[44] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[45] = new SqlParameter("@RuleOut", node.SelectSingleNode("RuleOut").InnerText);
                param[46] = new SqlParameter("@FreeFormCC", node.SelectSingleNode("FreeFormCC").InnerText);
                param[47] = new SqlParameter("@FreeFormA", node.SelectSingleNode("FreeFormA").InnerText);
                param[48] = new SqlParameter("@FreeFormP", node.SelectSingleNode("FreeFormP").InnerText);


            }
            db.executeSP(SP, param);

            //insert into Neck table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Neck");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[152];
                SP = "usp_tblbpNeck_insert_DV";

                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@PainScale", node.SelectSingleNode("PainScale").InnerText);
                param[2] = new SqlParameter("@Sharp", node.SelectSingleNode("Sharp").InnerText);

                param[3] = new SqlParameter("@Electric", node.SelectSingleNode("Electric").InnerText);
                param[4] = new SqlParameter("@Shooting", node.SelectSingleNode("Shooting").InnerText);
                param[5] = new SqlParameter("@Throbbling", node.SelectSingleNode("Throbbling").InnerText);
                param[6] = new SqlParameter("@Pulsating", node.SelectSingleNode("Pulsating").InnerText);
                param[7] = new SqlParameter("@Dull", node.SelectSingleNode("Dull").InnerText);
                param[8] = new SqlParameter("@Achy", node.SelectSingleNode("Achy").InnerText);
                param[9] = new SqlParameter("@Radiates", node.SelectSingleNode("Radiates").InnerText);
                param[10] = new SqlParameter("@Numbness", node.SelectSingleNode("Numbness").InnerText);
                param[11] = new SqlParameter("@Tingling", "false");
                param[12] = new SqlParameter("@Burning", node.SelectSingleNode("Burning").InnerText);
                param[13] = new SqlParameter("@BurningTo", node.SelectSingleNode("BurningTo").InnerText);
                param[14] = new SqlParameter("@ShoulderLeft1", node.SelectSingleNode("ShoulderLeft1").InnerText);
                param[15] = new SqlParameter("@ShoulderRight1", node.SelectSingleNode("ShoulderRight1").InnerText);

                param[16] = new SqlParameter("@ShoulderBilateral1", "false");
                param[17] = new SqlParameter("@ScapulaLeft1", node.SelectSingleNode("ScapulaLeft1").InnerText);
                param[18] = new SqlParameter("@ScapulaRight1", node.SelectSingleNode("ScapulaRight1").InnerText);
                param[19] = new SqlParameter("@ScapulaBilateral1", node.SelectSingleNode("ScapulaBilateral1").InnerText);
                param[20] = new SqlParameter("@ArmLeft1", node.SelectSingleNode("ArmLeft1").InnerText);
                param[21] = new SqlParameter("@ArmRight1", node.SelectSingleNode("ArmRight1").InnerText);
                param[22] = new SqlParameter("@ArmBilateral1", node.SelectSingleNode("ArmBilateral1").InnerText);
                param[23] = new SqlParameter("@ForearmLeft1", node.SelectSingleNode("ForearmLeft1").InnerText);
                param[24] = new SqlParameter("@ForearmRight1", node.SelectSingleNode("ForearmRight1").InnerText);
                param[25] = new SqlParameter("@ForearmBilateral1", node.SelectSingleNode("ForearmBilateral1").InnerText);
                param[26] = new SqlParameter("@HandLeft1", node.SelectSingleNode("HandLeft1").InnerText);
                param[27] = new SqlParameter("@HandRight1", node.SelectSingleNode("HandRight1").InnerText);
                param[28] = new SqlParameter("@HandBilateral1", node.SelectSingleNode("HandBilateral1").InnerText);
                param[29] = new SqlParameter("@WristLeft1", node.SelectSingleNode("WristLeft1").InnerText);
                param[30] = new SqlParameter("@WristRight1", node.SelectSingleNode("WristRight1").InnerText);

                param[31] = new SqlParameter("@WristBilateral1", node.SelectSingleNode("WristBilateral1").InnerText);
                param[32] = new SqlParameter("@C1stDigitLeft1", node.SelectSingleNode("C1stDigitLeft1").InnerText);
                param[33] = new SqlParameter("@C1stDigitRight1", node.SelectSingleNode("C1stDigitRight1").InnerText);
                param[34] = new SqlParameter("@C1stDigitBilateral1", node.SelectSingleNode("C1stDigitBilateral1").InnerText);
                param[35] = new SqlParameter("@C2ndDigitLeft1", node.SelectSingleNode("C2ndDigitLeft1").InnerText);
                param[36] = new SqlParameter("@C2ndDigitRight1", node.SelectSingleNode("C2ndDigitRight1").InnerText);
                param[37] = new SqlParameter("@C2ndDigitBilateral1", node.SelectSingleNode("C2ndDigitBilateral1").InnerText);
                param[38] = new SqlParameter("@C3rdDigitLeft1", node.SelectSingleNode("C3rdDigitLeft1").InnerText);
                param[39] = new SqlParameter("@C3rdDigitRight1", node.SelectSingleNode("C3rdDigitRight1").InnerText);
                param[40] = new SqlParameter("@C3rdDigitBilateral1", node.SelectSingleNode("C3rdDigitBilateral1").InnerText);
                param[41] = new SqlParameter("@C4thDigitLeft1", node.SelectSingleNode("C4thDigitLeft1").InnerText);
                param[42] = new SqlParameter("@C4thDigitRight1", node.SelectSingleNode("C4thDigitRight1").InnerText);
                param[43] = new SqlParameter("@C4thDigitBilateral1", node.SelectSingleNode("C4thDigitBilateral1").InnerText);
                param[44] = new SqlParameter("@C5thDigitLeft1", node.SelectSingleNode("C5thDigitLeft1").InnerText);
                param[45] = new SqlParameter("@C5thDigitRight1", node.SelectSingleNode("C5thDigitRight1").InnerText);
                param[46] = new SqlParameter("@C5thDigitBilateral1", node.SelectSingleNode("C5thDigitBilateral1").InnerText);
                param[47] = new SqlParameter("@ShoulderLeft2", node.SelectSingleNode("ShoulderLeft2").InnerText);

                param[48] = new SqlParameter("@ShoulderRight2", node.SelectSingleNode("ShoulderRight2").InnerText);
                param[49] = new SqlParameter("@ShoulderBilateral2", node.SelectSingleNode("ShoulderBilateral2").InnerText);
                param[50] = new SqlParameter("@ScapulaLeft2", node.SelectSingleNode("ScapulaLeft2").InnerText);
                param[51] = new SqlParameter("@ScapulaRight2", node.SelectSingleNode("ScapulaRight2").InnerText);
                param[52] = new SqlParameter("@ScapulaBilateral2", node.SelectSingleNode("ScapulaBilateral2").InnerText);
                param[53] = new SqlParameter("@ArmLeft2", node.SelectSingleNode("ArmLeft2").InnerText);
                param[54] = new SqlParameter("@ArmRight2", node.SelectSingleNode("ArmRight2").InnerText);
                param[55] = new SqlParameter("@ArmBilateral2", node.SelectSingleNode("ArmBilateral2").InnerText);
                param[56] = new SqlParameter("@ForearmLeft2", node.SelectSingleNode("ForearmLeft2").InnerText);
                param[57] = new SqlParameter("@ForearmRight2", node.SelectSingleNode("ForearmRight2").InnerText);
                param[58] = new SqlParameter("@ForearmBilateral2", node.SelectSingleNode("ForearmBilateral2").InnerText);
                param[59] = new SqlParameter("@HandLeft2", node.SelectSingleNode("HandLeft2").InnerText);
                param[60] = new SqlParameter("@HandRight2", node.SelectSingleNode("HandRight2").InnerText);
                param[61] = new SqlParameter("@HandBilateral2", node.SelectSingleNode("HandBilateral2").InnerText);
                param[62] = new SqlParameter("@WristLeft2", node.SelectSingleNode("WristLeft2").InnerText);
                param[63] = new SqlParameter("@WristRight2", node.SelectSingleNode("WristRight2").InnerText);
                param[64] = new SqlParameter("@WristBilateral2", node.SelectSingleNode("WristBilateral2").InnerText);
                param[65] = new SqlParameter("@C1stDigitLeft2", node.SelectSingleNode("C1stDigitLeft2").InnerText);

                param[66] = new SqlParameter("@C1stDigitRight2", node.SelectSingleNode("C1stDigitRight2").InnerText);
                param[67] = new SqlParameter("@C1stDigitBilateral2", node.SelectSingleNode("C1stDigitBilateral2").InnerText);
                param[68] = new SqlParameter("@C2ndDigitLeft2", node.SelectSingleNode("C2ndDigitLeft2").InnerText);
                param[69] = new SqlParameter("@C2ndDigitRight2", node.SelectSingleNode("C2ndDigitRight2").InnerText);
                param[70] = new SqlParameter("@C2ndDigitBilateral2", node.SelectSingleNode("C2ndDigitBilateral2").InnerText);
                param[71] = new SqlParameter("@C3rdDigitLeft2", node.SelectSingleNode("C3rdDigitLeft2").InnerText);
                param[72] = new SqlParameter("@C3rdDigitRight2", node.SelectSingleNode("C3rdDigitRight2").InnerText);
                param[73] = new SqlParameter("@C3rdDigitBilateral2", node.SelectSingleNode("C3rdDigitBilateral2").InnerText);
                param[74] = new SqlParameter("@C4thDigitLeft2", node.SelectSingleNode("C4thDigitLeft2").InnerText);
                param[75] = new SqlParameter("@C4thDigitRight2", node.SelectSingleNode("C4thDigitRight2").InnerText);
                param[76] = new SqlParameter("@C4thDigitBilateral2", node.SelectSingleNode("C4thDigitBilateral2").InnerText);
                param[77] = new SqlParameter("@C5thDigitLeft2", node.SelectSingleNode("C5thDigitLeft2").InnerText);
                param[78] = new SqlParameter("@C5thDigitRight2", node.SelectSingleNode("C5thDigitRight2").InnerText);
                param[79] = new SqlParameter("@C5thDigitBilateral2", node.SelectSingleNode("C5thDigitBilateral2").InnerText);
                param[80] = new SqlParameter("@WeeknessIn", node.SelectSingleNode("WeeknessIn").InnerText);
                param[81] = new SqlParameter("@WorseSitting", node.SelectSingleNode("WorseSitting").InnerText);
                param[82] = new SqlParameter("@WorseStanding", node.SelectSingleNode("WorseStanding").InnerText);
                param[83] = new SqlParameter("@WorseLyingDown", node.SelectSingleNode("WorseLyingDown").InnerText);
                param[84] = new SqlParameter("@WorseMovement", node.SelectSingleNode("WorseMovement").InnerText);
                param[85] = new SqlParameter("@WorseSeatingtoStandingUp", node.SelectSingleNode("WorseSeatingtoStandingUp").InnerText);

                param[86] = new SqlParameter("@WorseWalking", node.SelectSingleNode("WorseWalking").InnerText);
                param[87] = new SqlParameter("@WorseClimbingStairs", node.SelectSingleNode("WorseClimbingStairs").InnerText);
                param[88] = new SqlParameter("@WorseDescendingStairs", node.SelectSingleNode("WorseDescendingStairs").InnerText);
                param[89] = new SqlParameter("@WorseDriving", node.SelectSingleNode("WorseDriving").InnerText);
                param[90] = new SqlParameter("@WorseWorking", node.SelectSingleNode("WorseWorking").InnerText);
                param[91] = new SqlParameter("@WorseBending", node.SelectSingleNode("WorseBending").InnerText);
                param[92] = new SqlParameter("@WorseLifting", node.SelectSingleNode("WorseLifting").InnerText);
                param[93] = new SqlParameter("@WorseTwisting", node.SelectSingleNode("WorseTwisting").InnerText);
                param[94] = new SqlParameter("@ImprovedResting", node.SelectSingleNode("ImprovedResting").InnerText);
                param[95] = new SqlParameter("@ImprovedMedication", node.SelectSingleNode("ImprovedMedication").InnerText);
                param[96] = new SqlParameter("@ImprovedTherapy", node.SelectSingleNode("ImprovedTherapy").InnerText);
                param[97] = new SqlParameter("@ImprovedSleeping", node.SelectSingleNode("ImprovedSleeping").InnerText);
                param[98] = new SqlParameter("@ImprovedMovement", node.SelectSingleNode("ImprovedMovement").InnerText);
                param[99] = new SqlParameter("@FwdFlexRight", node.SelectSingleNode("FwdFlexRight").InnerText);
                param[100] = new SqlParameter("@FwdFlexLeft", node.SelectSingleNode("FwdFlexLeft").InnerText);
                param[101] = new SqlParameter("@ExtensionRight", node.SelectSingleNode("ExtensionRight").InnerText);
                param[102] = new SqlParameter("@ExtensionLeft", node.SelectSingleNode("ExtensionLeft").InnerText);
                param[103] = new SqlParameter("@RotationRight", node.SelectSingleNode("RotationRight").InnerText);
                param[104] = new SqlParameter("@RotationLeft", node.SelectSingleNode("RotationLeft").InnerText);
                param[105] = new SqlParameter("@LateralFlexRight", node.SelectSingleNode("LateralFlexRight").InnerText);
                param[106] = new SqlParameter("@LateralFlexLeft", node.SelectSingleNode("LateralFlexLeft").InnerText);
                param[107] = new SqlParameter("@PalpationAt", node.SelectSingleNode("PalpationAt").InnerText);
                param[108] = new SqlParameter("@Levels", node.SelectSingleNode("Levels").InnerText);
                param[109] = new SqlParameter("@Bilaterally", node.SelectSingleNode("Bilaterally").InnerText);
                param[110] = new SqlParameter("@CSRight", node.SelectSingleNode("CSRight").InnerText);
                param[111] = new SqlParameter("@CSLeft", node.SelectSingleNode("CSLeft").InnerText);
                param[112] = new SqlParameter("@RightGreaterLeft", node.SelectSingleNode("RightGreaterLeft").InnerText);
                param[113] = new SqlParameter("@LeftGreaterRight", node.SelectSingleNode("LeftGreaterRight").InnerText);
                param[114] = new SqlParameter("@Spurlings", node.SelectSingleNode("Spurlings").InnerText);

                param[115] = new SqlParameter("@Distraction", node.SelectSingleNode("Distraction").InnerText);
                param[116] = new SqlParameter("@TPSide1", node.SelectSingleNode("TPSide1").InnerText);
                param[117] = new SqlParameter("@TPText1", node.SelectSingleNode("TPText1").InnerText);
                param[118] = new SqlParameter("@TPSide2", node.SelectSingleNode("TPSide2").InnerText);
                param[119] = new SqlParameter("@TPText2", node.SelectSingleNode("TPText2").InnerText);
                param[120] = new SqlParameter("@TPSide3", node.SelectSingleNode("TPSide3").InnerText);

                param[121] = new SqlParameter("@TPText3", node.SelectSingleNode("TPText3").InnerText);
                param[122] = new SqlParameter("@TPside4", node.SelectSingleNode("TPside4").InnerText);
                param[123] = new SqlParameter("@TPText4", node.SelectSingleNode("TPText4").InnerText);
                param[124] = new SqlParameter("@TPSide5", node.SelectSingleNode("TPSide5").InnerText);
                param[125] = new SqlParameter("@TPText5", node.SelectSingleNode("TPText5").InnerText);
                param[126] = new SqlParameter("@TPSide6", node.SelectSingleNode("TPSide6").InnerText);
                param[127] = new SqlParameter("@TPText6", node.SelectSingleNode("TPText6").InnerText);
                param[128] = new SqlParameter("@TPSide7", node.SelectSingleNode("TPSide7").InnerText);
                param[129] = new SqlParameter("@TPText7", node.SelectSingleNode("TPText7").InnerText);
                param[130] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[131] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[132] = new SqlParameter("@Herniation", node.SelectSingleNode("Herniation").InnerText);
                param[133] = new SqlParameter("@Syndrome", node.SelectSingleNode("Syndrome").InnerText);
                param[134] = new SqlParameter("@Xrays", node.SelectSingleNode("Xrays").InnerText);
                param[135] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[136] = new SqlParameter("@CervicalSpine", node.SelectSingleNode("CervicalSpine").InnerText);
                param[137] = new SqlParameter("@ToRuleOut", node.SelectSingleNode("ToRuleOut").InnerText);
                param[138] = new SqlParameter("@TPEpidular", node.SelectSingleNode("TPEpidular").InnerText);
                param[139] = new SqlParameter("@NoOfTPInjections", node.SelectSingleNode("NoOfTPInjections").InnerText);
                param[140] = new SqlParameter("@ScheduleEpidural", node.SelectSingleNode("ScheduleEpidural").InnerText);
                param[141] = new SqlParameter("@NewMBB", node.SelectSingleNode("NewMBB").InnerText);
                param[142] = new SqlParameter("@SPTPMBB", node.SelectSingleNode("SPTPMBB").InnerText);
                param[143] = new SqlParameter("@EMGNCV", node.SelectSingleNode("EMGNCV").InnerText);
                param[144] = new SqlParameter("@ModalitiesAreas", node.SelectSingleNode("ModalitiesAreas").InnerText);
                param[145] = new SqlParameter("@AreasPositive", node.SelectSingleNode("AreasPositive").InnerText);
                param[146] = new SqlParameter("@CreatedBy", "Default");
                param[147] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[148] = new SqlParameter("@WorseOther", node.SelectSingleNode("WorseOther").InnerText);
                param[149] = new SqlParameter("@FreeFormCC", node.SelectSingleNode("FreeFormCC").InnerText);
                param[150] = new SqlParameter("@FreeFormA", node.SelectSingleNode("FreeFormA").InnerText);
                param[151] = new SqlParameter("@FreeFormP", node.SelectSingleNode("FreeFormP").InnerText);
            }
            db.executeSP(SP, param);

            //insert into shoulder table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Shoulder");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[114];
                SP = "usp_shoulder_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@PainScaleLeft", node.SelectSingleNode("PainScaleLeft").InnerText);
                param[2] = new SqlParameter("@SharpLeft", node.SelectSingleNode("SharpLeft").InnerText);
                param[3] = new SqlParameter("@ElectricLeft", node.SelectSingleNode("ElectricLeft").InnerText);
                param[4] = new SqlParameter("@ShootingLeft", node.SelectSingleNode("ShootingLeft").InnerText);
                param[5] = new SqlParameter("@ThrobblingLeft", node.SelectSingleNode("ThrobblingLeft").InnerText);
                param[6] = new SqlParameter("@PulsatingLeft", node.SelectSingleNode("PulsatingLeft").InnerText);
                param[7] = new SqlParameter("@DullLeft", node.SelectSingleNode("DullLeft").InnerText);
                param[8] = new SqlParameter("@AchyLeft", node.SelectSingleNode("AchyLeft").InnerText);
                param[9] = new SqlParameter("@WorseLyingLeft", node.SelectSingleNode("WorseLyingLeft").InnerText);
                param[10] = new SqlParameter("@WorseMovementLeft", node.SelectSingleNode("WorseMovementLeft").InnerText);
                param[11] = new SqlParameter("@WorseRaisingLeft", node.SelectSingleNode("WorseRaisingLeft").InnerText);
                param[12] = new SqlParameter("@WorseLiftingLeft", node.SelectSingleNode("WorseLiftingLeft").InnerText);
                param[13] = new SqlParameter("@WorseRotationLeft", node.SelectSingleNode("WorseRotationLeft").InnerText);
                param[14] = new SqlParameter("@WorseWorkingLeft", node.SelectSingleNode("WorseWorkingLeft").InnerText);
                param[15] = new SqlParameter("@WorseActivitiesLeft", node.SelectSingleNode("WorseActivitiesLeft").InnerText);
                param[16] = new SqlParameter("@ImprovedRestingLeft", node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
                param[17] = new SqlParameter("@ImprovedMedicationLeft", node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
                param[18] = new SqlParameter("@ImprovedTherapyLeft", node.SelectSingleNode("ImprovedTherapyLeft").InnerText);
                param[19] = new SqlParameter("@ImprovedSleepingLeft", node.SelectSingleNode("ImprovedSleepingLeft").InnerText);
                param[20] = new SqlParameter("@PainScaleRight", node.SelectSingleNode("PainScaleRight").InnerText);
                param[21] = new SqlParameter("@SharpRight", node.SelectSingleNode("SharpRight").InnerText);
                param[22] = new SqlParameter("@ElectricRight", node.SelectSingleNode("ElectricRight").InnerText);
                param[23] = new SqlParameter("@ShootingRight", node.SelectSingleNode("ShootingRight").InnerText);
                param[24] = new SqlParameter("@ThrobblingRight ", node.SelectSingleNode("ThrobblingRight").InnerText);
                param[25] = new SqlParameter("@PulsatingRight", node.SelectSingleNode("PulsatingRight").InnerText);
                param[26] = new SqlParameter("@DullRight", node.SelectSingleNode("DullRight ").InnerText);
                param[27] = new SqlParameter("@AchyRight", node.SelectSingleNode("AchyRight").InnerText);
                param[28] = new SqlParameter("@WorseLyingRight", node.SelectSingleNode("WorseLyingRight").InnerText);
                param[29] = new SqlParameter("@WorseMovementRight", node.SelectSingleNode("WorseMovementRight").InnerText);
                param[30] = new SqlParameter("@WorseRaisingRight", node.SelectSingleNode("WorseRaisingRight").InnerText);
                param[31] = new SqlParameter("@WorseLiftingRight", node.SelectSingleNode("WorseLiftingRight").InnerText);
                param[32] = new SqlParameter("@WorseRotationRight", node.SelectSingleNode("WorseRotationRight").InnerText);
                param[33] = new SqlParameter("@WorseWorkingRight", node.SelectSingleNode("WorseWorkingRight").InnerText);
                param[34] = new SqlParameter("@WorseActivitiesRight", node.SelectSingleNode("WorseActivitiesRight").InnerText);
                param[35] = new SqlParameter("@ImprovedRestingRight", node.SelectSingleNode("ImprovedRestingRight").InnerText);
                param[36] = new SqlParameter("@ImprovedMedicationRight", node.SelectSingleNode("ImprovedMedicationRight").InnerText);
                param[37] = new SqlParameter("@ImprovedTherapyRight", node.SelectSingleNode("ImprovedMedicationRight").InnerText);
                param[38] = new SqlParameter("@ImprovedSleepingRight", node.SelectSingleNode("ImprovedSleepingRight").InnerText);
                param[39] = new SqlParameter("@AbductionLeft", node.SelectSingleNode("AbductionLeft").InnerText);
                param[40] = new SqlParameter("@FlexionLeft", node.SelectSingleNode("FlexionLeft").InnerText);
                param[41] = new SqlParameter("@ExtRotationLeft", node.SelectSingleNode("ExtRotationLeft").InnerText);
                param[42] = new SqlParameter("@IntRotationLeft", node.SelectSingleNode("IntRotationLeft").InnerText);
                param[43] = new SqlParameter("@AbductionRight", node.SelectSingleNode("AbductionRight").InnerText);
                param[44] = new SqlParameter("@FlexionRight", node.SelectSingleNode("FlexionRight").InnerText);
                param[45] = new SqlParameter("@ExtRotationRight", node.SelectSingleNode("ExtRotationRight").InnerText);
                param[46] = new SqlParameter("@IntRotationRight", node.SelectSingleNode("IntRotationRight").InnerText);


                param[47] = new SqlParameter("@ACJointLeft", node.SelectSingleNode("ACJointLeft").InnerText);
                param[48] = new SqlParameter("@GlenohumeralLeft", node.SelectSingleNode("GlenohumeralLeft").InnerText);
                param[49] = new SqlParameter("@CorticoidLeft", node.SelectSingleNode("CorticoidLeft").InnerText);
                param[50] = new SqlParameter("@SupraspinatusLeft", node.SelectSingleNode("SupraspinatusLeft").InnerText);
                param[51] = new SqlParameter("@ScapularLeft", node.SelectSingleNode("ScapularLeft").InnerText);
                param[52] = new SqlParameter("@DeepLabralLeft", node.SelectSingleNode("DeepLabralLeft").InnerText);
                param[53] = new SqlParameter("@DeltoidLeft", node.SelectSingleNode("DeltoidLeft").InnerText);
                param[54] = new SqlParameter("@TrapeziusLeft", node.SelectSingleNode("TrapeziusLeft").InnerText);
                param[55] = new SqlParameter("@EccymosisLeft", node.SelectSingleNode("EccymosisLeft").InnerText);
                param[56] = new SqlParameter("@EdemaLeft", node.SelectSingleNode("EdemaLeft").InnerText);
                param[57] = new SqlParameter("@RangeOfMotionLeft", node.SelectSingleNode("RangeOfMotionLeft").InnerText);

                param[58] = new SqlParameter("@ACJointRight", node.SelectSingleNode("ACJointRight").InnerText);
                param[59] = new SqlParameter("@GlenohumeralRight", node.SelectSingleNode("GlenohumeralRight").InnerText);
                param[60] = new SqlParameter("@CorticoidRight", node.SelectSingleNode("CorticoidRight").InnerText);
                param[61] = new SqlParameter("@SupraspinatusRight", node.SelectSingleNode("SupraspinatusRight").InnerText);
                param[62] = new SqlParameter("@ScapularRight", node.SelectSingleNode("ScapularRight").InnerText);
                param[63] = new SqlParameter("@DeepLabralRight", node.SelectSingleNode("DeepLabralRight").InnerText);
                param[64] = new SqlParameter("@DeltoidRight", node.SelectSingleNode("DeltoidRight").InnerText);
                param[65] = new SqlParameter("@TrapeziusRight", node.SelectSingleNode("TrapeziusRight").InnerText);
                param[66] = new SqlParameter("@EccymosisRight", node.SelectSingleNode("EccymosisRight").InnerText);
                param[67] = new SqlParameter("@EdemaRight", node.SelectSingleNode("EdemaRight").InnerText);
                param[68] = new SqlParameter("@RangeOfMotionRight", node.SelectSingleNode("RangeOfMotionRight").InnerText);
                param[69] = new SqlParameter("@NeerLeft", node.SelectSingleNode("NeerLeft").InnerText);
                param[70] = new SqlParameter("@HawkinLeft", node.SelectSingleNode("HawkinLeft").InnerText);
                param[71] = new SqlParameter("@YergasonsLeft", node.SelectSingleNode("YergasonsLeft").InnerText);
                param[72] = new SqlParameter("@DropArmLeft", node.SelectSingleNode("DropArmLeft").InnerText);
                param[73] = new SqlParameter("@ReverseBeerLeft", node.SelectSingleNode("ReverseBeerLeft").InnerText);
                param[74] = new SqlParameter("@NeerRight", node.SelectSingleNode("NeerRight").InnerText);
                param[75] = new SqlParameter("@HawkinRight", node.SelectSingleNode("HawkinRight").InnerText);
                param[76] = new SqlParameter("@YergasonsRight", node.SelectSingleNode("YergasonsRight").InnerText);
                param[77] = new SqlParameter("@DropArmRight", node.SelectSingleNode("DropArmRight").InnerText);
                param[78] = new SqlParameter("@ReverseBeerRight", node.SelectSingleNode("ReverseBeerRight").InnerText);
                param[79] = new SqlParameter("@TPSide1", node.SelectSingleNode("TPSide1").InnerText);
                param[80] = new SqlParameter("@TPText1", node.SelectSingleNode("TPText1").InnerText);
                param[81] = new SqlParameter("@TPSide2", node.SelectSingleNode("TPSide2").InnerText);
                param[82] = new SqlParameter("@TPText2", node.SelectSingleNode("TPText2").InnerText);
                param[83] = new SqlParameter("@TPSide3", node.SelectSingleNode("TPSide3").InnerText);
                param[84] = new SqlParameter("@TPText3", node.SelectSingleNode("TPText3").InnerText);
                param[85] = new SqlParameter("@TPSide4", node.SelectSingleNode("TPSide4").InnerText);
                param[86] = new SqlParameter("@TPText4", node.SelectSingleNode("TPText4").InnerText);

                param[87] = new SqlParameter("@TPText5", node.SelectSingleNode("TPText5").InnerText);

                param[88] = new SqlParameter("@TPText6", node.SelectSingleNode("TPText6").InnerText);

                param[89] = new SqlParameter("@TPText7", node.SelectSingleNode("TPText7").InnerText);

                param[90] = new SqlParameter("@TPText8", node.SelectSingleNode("TPText8").InnerText);

                param[91] = new SqlParameter("@SprainStrainSide", node.SelectSingleNode("SprainStrainSide").InnerText);
                param[92] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[93] = new SqlParameter("@DerangmentSide", node.SelectSingleNode("DerangmentSide").InnerText);
                param[94] = new SqlParameter("@Derangment", node.SelectSingleNode("Derangment").InnerText);
                param[95] = new SqlParameter("@SyndromeSide", node.SelectSingleNode("SyndromeSide").InnerText);
                param[96] = new SqlParameter("@Syndrome", node.SelectSingleNode("Syndrome").InnerText);
                param[97] = new SqlParameter("@Plan", node.SelectSingleNode("Plan").InnerText);
                param[98] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[99] = new SqlParameter("@ScanSide", node.SelectSingleNode("ScanSide").InnerText);
                param[100] = new SqlParameter("@CreatedBy", "Default");
                param[101] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[102] = new SqlParameter("@FreeFormCC", "Default");
                param[103] = new SqlParameter("@FreeFormA", "Default");
                param[104] = new SqlParameter("@FreeFormP", "Default");




                param[105] = new SqlParameter("@PalpationText1Left", node.SelectSingleNode("PalpationText1Left").InnerText);
                param[106] = new SqlParameter("@PalpationText2Left", node.SelectSingleNode("PalpationText2Left").InnerText);
                param[107] = new SqlParameter("@PalpationText1Right", node.SelectSingleNode("PalpationText1Right").InnerText);
                param[108] = new SqlParameter("@PalpationText2Right", node.SelectSingleNode("PalpationText2Right").InnerText);
                param[109] = new SqlParameter("@TPSide5", node.SelectSingleNode("TPSide5").InnerText);
                param[110] = new SqlParameter("@TPSide6", node.SelectSingleNode("TPSide6").InnerText);
                param[111] = new SqlParameter("@TPSide7", node.SelectSingleNode("TPSide7").InnerText);
                param[112] = new SqlParameter("@TPSide8", node.SelectSingleNode("TPSide8").InnerText);
                param[113] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);

            }
            db.executeSP(SP, param);

            //insert into wrist table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Wrist");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[56];
                SP = "usp_wrist_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@UlnarLeft", node.SelectSingleNode("UlnarLeft").InnerText);
                param[2] = new SqlParameter("@RadialLeft", node.SelectSingleNode("RadialLeft").InnerText);
                param[3] = new SqlParameter("@DorsalLeft", node.SelectSingleNode("DorsalLeft").InnerText);
                param[4] = new SqlParameter("@PalmarLeft", node.SelectSingleNode("PalmarLeft").InnerText);
                param[5] = new SqlParameter("@UlnarRight", node.SelectSingleNode("UlnarRight").InnerText);
                param[6] = new SqlParameter("@RadialRight", node.SelectSingleNode("RadialRight").InnerText);
                param[7] = new SqlParameter("@DorsalRight", node.SelectSingleNode("DorsalRight").InnerText);
                param[8] = new SqlParameter("@PalmarRight", node.SelectSingleNode("PalmarRight").InnerText);
                param[9] = new SqlParameter("@RangeOfMotionLeft", node.SelectSingleNode("RangeOfMotionLeft").InnerText);
                param[10] = new SqlParameter("@PalpationUlnarLeft", node.SelectSingleNode("PalpationUlnarLeft").InnerText);
                param[11] = new SqlParameter("@PalpationRadialLeft", node.SelectSingleNode("PalpationRadialLeft").InnerText);
                param[12] = new SqlParameter("@PalpationDorsalLeft", node.SelectSingleNode("PalpationDorsalLeft").InnerText);
                param[13] = new SqlParameter("@PalpationPalmarLeft", node.SelectSingleNode("PalpationPalmarLeft").InnerText);
                param[14] = new SqlParameter("@IncreasesUlnarLeft", node.SelectSingleNode("IncreasesUlnarLeft").InnerText);
                param[15] = new SqlParameter("@IncreasesRadialLeft", node.SelectSingleNode("IncreasesRadialLeft").InnerText);
                param[16] = new SqlParameter("@IncreasesDorsalLeft", node.SelectSingleNode("IncreasesDorsalLeft").InnerText);
                param[17] = new SqlParameter("@IncreasesPalmarLeft", node.SelectSingleNode("IncreasesPalmarLeft").InnerText);
                param[18] = new SqlParameter("@RangeOfMotionRight", node.SelectSingleNode("RangeOfMotionRight").InnerText);
                param[19] = new SqlParameter("@PalpationUlnarRight", node.SelectSingleNode("PalpationUlnarRight").InnerText);
                param[20] = new SqlParameter("@PalpationRadialRight", node.SelectSingleNode("PalpationRadialRight").InnerText);
                param[21] = new SqlParameter("@PalpationDorsalRight", node.SelectSingleNode("PalpationDorsalRight").InnerText);
                param[22] = new SqlParameter("@PalpationPalmarRight", node.SelectSingleNode("PalpationPalmarRight").InnerText);
                param[23] = new SqlParameter("@IncreasesUlnarRight", node.SelectSingleNode("IncreasesUlnarRight").InnerText);
                param[24] = new SqlParameter("@IncreasesRadialRight", node.SelectSingleNode("IncreasesRadialRight").InnerText);
                param[25] = new SqlParameter("@IncreasesDorsalRight", node.SelectSingleNode("IncreasesDorsalRight").InnerText);
                param[26] = new SqlParameter("@IncreasesPalmarRight", node.SelectSingleNode("IncreasesPalmarRight").InnerText);
                param[27] = new SqlParameter("@TinelRight", node.SelectSingleNode("TinelRight").InnerText);
                param[28] = new SqlParameter("@PhalenRight", node.SelectSingleNode("PhalenRight").InnerText);
                param[29] = new SqlParameter("@FinkelsteinRight", node.SelectSingleNode("FinkelsteinRight").InnerText);
                param[30] = new SqlParameter("@UlnarDeviationRight", node.SelectSingleNode("UlnarDeviationRight").InnerText);
                param[31] = new SqlParameter("@RadialDeviationRight", node.SelectSingleNode("RadialDeviationRight").InnerText);
                param[32] = new SqlParameter("@DorsiFlexionRight", node.SelectSingleNode("DorsiFlexionRight").InnerText);
                param[33] = new SqlParameter("@PalmarFlexionRight", node.SelectSingleNode("PalmarFlexionRight").InnerText);
                param[34] = new SqlParameter("@TinelLeft", node.SelectSingleNode("TinelLeft").InnerText);
                param[35] = new SqlParameter("@PhalenLeft", node.SelectSingleNode("PhalenLeft").InnerText);
                param[36] = new SqlParameter("@FinkelsteinLeft", node.SelectSingleNode("FinkelsteinLeft").InnerText);
                param[37] = new SqlParameter("@UlnarDeviationLeft", node.SelectSingleNode("UlnarDeviationLeft").InnerText);
                param[38] = new SqlParameter("@RadialDeviationLeft", node.SelectSingleNode("RadialDeviationLeft").InnerText);
                param[39] = new SqlParameter("@DorsiFlexionLeft", node.SelectSingleNode("DorsiFlexionLeft").InnerText);
                param[40] = new SqlParameter("@PalmarFlexionLeft", node.SelectSingleNode("PalmarFlexionLeft").InnerText);
                param[41] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[42] = new SqlParameter("@SprainStrain", node.SelectSingleNode("SprainStrain").InnerText);
                param[43] = new SqlParameter("@SprainStrainSide", node.SelectSingleNode("SprainStrainSide").InnerText);
                param[44] = new SqlParameter("@Contusion", node.SelectSingleNode("Contusion").InnerText);
                param[45] = new SqlParameter("@ContusionSide", node.SelectSingleNode("ContusionSide").InnerText);
                param[46] = new SqlParameter("@Fracture", node.SelectSingleNode("Fracture").InnerText);
                param[47] = new SqlParameter("@FractureSide", node.SelectSingleNode("FractureSide").InnerText);
                param[48] = new SqlParameter("@Scan", node.SelectSingleNode("Scan").InnerText);
                param[49] = new SqlParameter("@ScanType", node.SelectSingleNode("ScanType").InnerText);
                param[50] = new SqlParameter("@ScanSide", node.SelectSingleNode("ScanSide").InnerText);
                param[51] = new SqlParameter("@CreatedBy", "Default");
                param[52] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[53] = new SqlParameter("@FreeFormCC", "Default");
                param[54] = new SqlParameter("@FreeFormA", "Default");
                param[55] = new SqlParameter("@FreeFormP", "Default");
            }
            db.executeSP(SP, param);

            //insert into IEPage1 table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage1");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[49];
                SP = "usp_tblPatientIEDetailPage1_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@Sustained", node.SelectSingleNode("Sustained").InnerText);
                param[2] = new SqlParameter("@Position", node.SelectSingleNode("Position").InnerText);
                param[3] = new SqlParameter("@InvolvedIn", node.SelectSingleNode("InvolvedIn").InnerText);
                param[4] = new SqlParameter("@InvolvedInOther", node.SelectSingleNode("InvolvedInOther").InnerText);
                param[5] = new SqlParameter("@EMSTeam", node.SelectSingleNode("EMSTeam").InnerText);
                param[6] = new SqlParameter("@CriteriaA", node.SelectSingleNode("CriteriaA").InnerText);
                param[7] = new SqlParameter("@WentTo", node.SelectSingleNode("WentTo").InnerText);
                param[8] = new SqlParameter("@Via", node.SelectSingleNode("Via").InnerText);
                param[9] = new SqlParameter("@SameDay", node.SelectSingleNode("SameDay").InnerText);
                param[10] = new SqlParameter("@DaysLater", node.SelectSingleNode("DaysLater").InnerText);
                param[11] = new SqlParameter("@HadXrayOf", node.SelectSingleNode("HadXrayOf").InnerText);
                param[12] = new SqlParameter("@HadCTScanOf", node.SelectSingleNode("HadCTScanOf").InnerText);
                param[13] = new SqlParameter("@DiagFindingRequested", node.SelectSingleNode("DiagFindingRequested").InnerText);
                param[14] = new SqlParameter("@PrescriptionFor", node.SelectSingleNode("PrescriptionFor").InnerText);
                param[15] = new SqlParameter("@RecommendedToSeeDoctor", node.SelectSingleNode("RecommendedToSeeDoctor").InnerText);
                param[16] = new SqlParameter("@RecommendedToGetPhyTherapy", node.SelectSingleNode("RecommendedToGetPhyTherapy").InnerText);
                param[17] = new SqlParameter("@AdmittedFor", node.SelectSingleNode("AdmittedFor").InnerText);

                param[18] = new SqlParameter("@Evaluated", node.SelectSingleNode("Evaluated").InnerText);
                param[19] = new SqlParameter("@CriteriaB", node.SelectSingleNode("CriteriaB").InnerText);
                param[20] = new SqlParameter("@FreeForm", node.SelectSingleNode("FreeForm").InnerText);
                param[21] = new SqlParameter("@InjuryToHead", node.SelectSingleNode("InjuryToHead").InnerText);
                param[22] = new SqlParameter("@LossOfConsciousnessFor", node.SelectSingleNode("LossOfConsciousnessFor").InnerText);
                param[23] = new SqlParameter("@ComplainingHeadeaches", node.SelectSingleNode("ComplainingHeadeaches").InnerText);
                param[24] = new SqlParameter("@Persistent", node.SelectSingleNode("Persistent").InnerText);
                param[25] = new SqlParameter("@HeadechesAssociated", "false");
                param[26] = new SqlParameter("@Frontal", node.SelectSingleNode("Frontal").InnerText);
                param[27] = new SqlParameter("@LeftParietal", node.SelectSingleNode("LeftParietal").InnerText);
                param[28] = new SqlParameter("@RightParietal", node.SelectSingleNode("RightParietal").InnerText);
                param[29] = new SqlParameter("@LeftTemporal", node.SelectSingleNode("LeftTemporal").InnerText);
                param[30] = new SqlParameter("@RightTemporal", node.SelectSingleNode("RightTemporal").InnerText);
                param[31] = new SqlParameter("@Occipital", node.SelectSingleNode("Occipital").InnerText);
                param[32] = new SqlParameter("@Global", node.SelectSingleNode("Global").InnerText);
                param[33] = new SqlParameter("@SevereAnxiety", node.SelectSingleNode("SevereAnxiety").InnerText);
                param[34] = new SqlParameter("@Nausea", node.SelectSingleNode("Nausea").InnerText);
                param[35] = new SqlParameter("@Dizziness", node.SelectSingleNode("Dizziness").InnerText);
                param[36] = new SqlParameter("@Vomiting", node.SelectSingleNode("Vomiting").InnerText);
                param[37] = new SqlParameter("@HadTreatmentFor", node.SelectSingleNode("HadTreatmentFor").InnerText);
                param[38] = new SqlParameter("@DetailInfo", node.SelectSingleNode("DetailInfo").InnerText);
                param[39] = new SqlParameter("@CreatedBy", "Default");
                param[40] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[41] = new SqlParameter("@HadMRIOf", node.SelectSingleNode("HadMRIOf").InnerText);
                param[42] = new SqlParameter("@LossOfConsciousness", node.SelectSingleNode("LossOfConsciousness").InnerText);
                param[43] = new SqlParameter("@OccurOn", node.SelectSingleNode("OccurOn").InnerText);
                param[44] = new SqlParameter("@CC1", node.SelectSingleNode("CC1").InnerText);
                param[45] = new SqlParameter("@CC2", node.SelectSingleNode("CC2").InnerText);
                param[46] = new SqlParameter("@DischargedOn", "");
                param[47] = new SqlParameter("@AccidentDetail", "");
                param[48] = new SqlParameter("@DoctorSeen", "");
                //   
            }
            db.executeSP(SP, param);

            //insert into IEPage2 table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage2");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[108];
                SP = "usp_tblPatientIEDetailPage2_insert_DV";
                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@Seizures", node.SelectSingleNode("Seizures").InnerText);
                param[2] = new SqlParameter("@ChestPain", node.SelectSingleNode("ChestPain").InnerText);
                param[3] = new SqlParameter("@ShortnessOfBreath", node.SelectSingleNode("ShortnessOfBreath").InnerText);
                param[4] = new SqlParameter("@Jawpain", node.SelectSingleNode("Jawpain").InnerText);
                param[5] = new SqlParameter("@AbdominalPain", node.SelectSingleNode("AbdominalPain").InnerText);
                param[6] = new SqlParameter("@Fevers", node.SelectSingleNode("Fevers").InnerText);
                param[7] = new SqlParameter("@NightSweats", node.SelectSingleNode("NightSweats").InnerText);
                param[8] = new SqlParameter("@Diarrhea", node.SelectSingleNode("Diarrhea").InnerText);
                param[9] = new SqlParameter("@DloodInUrine", node.SelectSingleNode("DloodInUrine").InnerText);
                param[10] = new SqlParameter("@Bowel", node.SelectSingleNode("Bowel").InnerText);
                param[11] = new SqlParameter("@DoubleVision", node.SelectSingleNode("DoubleVision").InnerText);
                param[12] = new SqlParameter("@HearingLoss", node.SelectSingleNode("HearingLoss").InnerText);
                param[13] = new SqlParameter("@RecentWeightloss", node.SelectSingleNode("RecentWeightloss").InnerText);
                param[14] = new SqlParameter("@Episodic", node.SelectSingleNode("Episodic").InnerText);
                param[15] = new SqlParameter("@Rashes", node.SelectSingleNode("Rashes").InnerText);
                param[16] = new SqlParameter("@PMH", node.SelectSingleNode("PMH").InnerText);
                param[17] = new SqlParameter("@PSH", node.SelectSingleNode("PSH").InnerText);
                param[18] = new SqlParameter("@Medications", node.SelectSingleNode("Medications").InnerText);
                param[19] = new SqlParameter("@Allergies", node.SelectSingleNode("Allergies").InnerText);
                param[20] = new SqlParameter("@DeniesSmoking", node.SelectSingleNode("DeniesSmoking").InnerText);
                param[21] = new SqlParameter("@DeniesDrinking", node.SelectSingleNode("DeniesDrinking").InnerText);
                param[22] = new SqlParameter("@DeniesDrugs", node.SelectSingleNode("DeniesDrugs").InnerText);
                param[23] = new SqlParameter("@DeniesSocialDrinking", node.SelectSingleNode("DeniesSocialDrinking").InnerText);
                param[24] = new SqlParameter("@WorksAt", node.SelectSingleNode("WorksAt").InnerText);
                param[25] = new SqlParameter("@Missed", node.SelectSingleNode("Missed").InnerText);
                param[26] = new SqlParameter("@ReturnToWork", node.SelectSingleNode("ReturnToWork").InnerText);
                param[27] = new SqlParameter("@intactexcept", node.SelectSingleNode("intactexcept").InnerText);
                param[28] = new SqlParameter("@DTRtricepsRight", node.SelectSingleNode("DTRtricepsRight").InnerText);
                param[29] = new SqlParameter("@DTRtricepsLeft", node.SelectSingleNode("DTRtricepsLeft").InnerText);
                param[30] = new SqlParameter("@DTRBicepsRight", node.SelectSingleNode("DTRBicepsRight").InnerText);
                param[31] = new SqlParameter("@DTRBicepsLeft", node.SelectSingleNode("DTRBicepsLeft").InnerText);
                param[32] = new SqlParameter("@DTRKneeRight", node.SelectSingleNode("DTRKneeRight").InnerText);
                param[33] = new SqlParameter("@DTRKneeLeft", node.SelectSingleNode("DTRKneeLeft").InnerText);
                param[34] = new SqlParameter("@DTRBrachioRight", node.SelectSingleNode("DTRBrachioRight").InnerText);
                param[35] = new SqlParameter("@DTRBrachioLeft", node.SelectSingleNode("DTRBrachioLeft").InnerText);
                param[36] = new SqlParameter("@Sensory", node.SelectSingleNode("Sensory").InnerText);
                param[37] = new SqlParameter("@Pinprick", node.SelectSingleNode("Pinprick").InnerText);
                param[38] = new SqlParameter("@Lighttouch", node.SelectSingleNode("Lighttouch").InnerText);
                param[39] = new SqlParameter("@UEC5Right", node.SelectSingleNode("UEC5Right").InnerText);
                param[40] = new SqlParameter("@UEC5Left", node.SelectSingleNode("UEC5Left").InnerText);
                param[41] = new SqlParameter("@UEC6Right", node.SelectSingleNode("UEC6Right").InnerText);
                param[42] = new SqlParameter("@UEC6Left", node.SelectSingleNode("UEC6Left").InnerText);
                param[43] = new SqlParameter("@UEC7Right", node.SelectSingleNode("UEC7Right").InnerText);
                param[44] = new SqlParameter("@UEC7Left", node.SelectSingleNode("UEC7Left").InnerText);
                param[45] = new SqlParameter("@UEC8Right", node.SelectSingleNode("UEC8Right").InnerText);
                param[46] = new SqlParameter("@UEC8Left", node.SelectSingleNode("UEC8Left").InnerText);
                param[47] = new SqlParameter("@UET1Right", node.SelectSingleNode("UET1Right").InnerText);
                param[48] = new SqlParameter("@UET1Left", node.SelectSingleNode("UET1Left").InnerText);
                param[49] = new SqlParameter("@UECervicalParaspinalsRight", node.SelectSingleNode("UECervicalParaspinalsRight").InnerText);
                param[50] = new SqlParameter("@UECervicalParaspinalsLeft", node.SelectSingleNode("UECervicalParaspinalsLeft").InnerText);
                param[51] = new SqlParameter("@LEL3Right", node.SelectSingleNode("LEL3Right").InnerText);
                param[52] = new SqlParameter("@LEL3Left", node.SelectSingleNode("LEL3Left").InnerText);
                param[53] = new SqlParameter("@LEL4Right", node.SelectSingleNode("LEL4Right").InnerText);
                param[54] = new SqlParameter("@LEL4Left", node.SelectSingleNode("LEL4Left").InnerText);
                param[55] = new SqlParameter("@LEL5Right", node.SelectSingleNode("LEL5Right").InnerText);
                param[56] = new SqlParameter("@LEL5Left", node.SelectSingleNode("LEL5Left").InnerText);
                param[57] = new SqlParameter("@LELumberParaspinalsRight", node.SelectSingleNode("LELumberParaspinalsRight").InnerText);
                param[58] = new SqlParameter("@LELumberParaspinalsLeft", node.SelectSingleNode("LELumberParaspinalsLeft").InnerText);
                param[59] = new SqlParameter("@HoffmanExam", node.SelectSingleNode("HoffmanExam").InnerText);
                param[60] = new SqlParameter("@Stocking", node.SelectSingleNode("Stocking").InnerText);
                param[61] = new SqlParameter("@Glove", node.SelectSingleNode("Glove").InnerText);
                param[62] = new SqlParameter("@UEShoulderAbductionRight", node.SelectSingleNode("UEShoulderAbductionRight").InnerText);
                param[63] = new SqlParameter("@UEShoulderAbductionLeft", node.SelectSingleNode("UEShoulderAbductionLeft").InnerText);
                param[64] = new SqlParameter("@UEShoulderFlexionRight", node.SelectSingleNode("UEShoulderFlexionRight").InnerText);
                param[65] = new SqlParameter("@UEShoulderFlexionLeft", node.SelectSingleNode("UEShoulderFlexionLeft").InnerText);
                param[66] = new SqlParameter("@UEElbowExtensionRight", node.SelectSingleNode("UEElbowExtensionRight").InnerText);
                param[67] = new SqlParameter("@UEElbowExtensionLeft", node.SelectSingleNode("UEElbowExtensionLeft").InnerText);
                param[68] = new SqlParameter("@UEElbowFlexionRight", node.SelectSingleNode("UEElbowFlexionRight").InnerText);
                param[69] = new SqlParameter("@UEElbowFlexionLeft", node.SelectSingleNode("UEElbowFlexionLeft").InnerText);
                param[70] = new SqlParameter("@UEElbowSupinationRight", node.SelectSingleNode("UEElbowSupinationRight").InnerText);
                param[71] = new SqlParameter("@UEElbowSupinationLeft", node.SelectSingleNode("UEElbowSupinationLeft").InnerText);
                param[72] = new SqlParameter("@UEElbowPronationRight", node.SelectSingleNode("UEElbowPronationRight").InnerText);
                param[73] = new SqlParameter("@UEElbowPronationLeft", node.SelectSingleNode("UEElbowPronationLeft").InnerText);
                param[74] = new SqlParameter("@UEWristFlexionRight", node.SelectSingleNode("UEWristFlexionRight").InnerText);
                param[75] = new SqlParameter("@UEWristFlexionLeft", node.SelectSingleNode("UEWristFlexionLeft").InnerText);
                param[76] = new SqlParameter("@UEWristExtensionRight", node.SelectSingleNode("UEWristExtensionRight").InnerText);
                param[77] = new SqlParameter("@UEWristExtensionLeft", node.SelectSingleNode("UEWristExtensionLeft").InnerText);
                param[78] = new SqlParameter("@UEHandGripStrengthRight", node.SelectSingleNode("UEHandGripStrengthRight").InnerText);
                param[79] = new SqlParameter("@UEHandGripStrengthLeft", node.SelectSingleNode("UEHandGripStrengthLeft").InnerText);
                param[80] = new SqlParameter("@UEHandFingerAbductorsRight", node.SelectSingleNode("UEHandFingerAbductorsRight").InnerText);
                param[81] = new SqlParameter("@UEHandFingerAbductorsLeft", node.SelectSingleNode("UEHandFingerAbductorsLeft").InnerText);
                param[82] = new SqlParameter("@LEHipFlexionRight", node.SelectSingleNode("LEHipFlexionRight").InnerText);
                param[83] = new SqlParameter("@LEHipFlexionLeft", node.SelectSingleNode("LEHipFlexionLeft").InnerText);
                param[84] = new SqlParameter("@LEHipAbductionRight", node.SelectSingleNode("LEHipAbductionRight").InnerText);
                param[85] = new SqlParameter("@LEHipAbductionLeft", node.SelectSingleNode("LEHipAbductionLeft").InnerText);
                param[86] = new SqlParameter("@LEKneeExtensionRight", node.SelectSingleNode("LEKneeExtensionRight").InnerText);
                param[87] = new SqlParameter("@LEKneeExtensionLeft", node.SelectSingleNode("LEKneeExtensionLeft").InnerText);
                param[88] = new SqlParameter("@LEKneeFlexionRight", node.SelectSingleNode("LEKneeFlexionRight").InnerText);
                param[89] = new SqlParameter("@LEKneeFlexionLeft", node.SelectSingleNode("LEKneeFlexionLeft").InnerText);
                param[90] = new SqlParameter("@LEAnkleDorsiRight", node.SelectSingleNode("LEAnkleDorsiRight").InnerText);
                param[91] = new SqlParameter("@LEAnkleDorsiLeft", node.SelectSingleNode("LEAnkleDorsiLeft").InnerText);
                param[92] = new SqlParameter("@LEAnklePlantarRight", node.SelectSingleNode("LEAnklePlantarRight").InnerText);
                param[93] = new SqlParameter("@LEAnklePlantarLeft", node.SelectSingleNode("LEAnklePlantarLeft").InnerText);
                param[94] = new SqlParameter("@LEAnkleExtensorRight", node.SelectSingleNode("LEAnkleExtensorRight").InnerText);
                param[95] = new SqlParameter("@LEAnkleExtensorLeft", node.SelectSingleNode("LEAnkleExtensorLeft").InnerText);
                param[96] = new SqlParameter("@CreatedBy", "Default");
                param[97] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[98] = new SqlParameter("@LES1Left", node.SelectSingleNode("LES1Left").InnerText);
                param[99] = new SqlParameter("@LES1Right", node.SelectSingleNode("LES1Right").InnerText);
                param[100] = new SqlParameter("@DTRAnkleRight", node.SelectSingleNode("DTRAnkleRight").InnerText);
                param[101] = new SqlParameter("@DTRAnkleLeft", node.SelectSingleNode("DTRAnkleLeft").InnerText);
                param[102] = new SqlParameter("@UEdtr", node.SelectSingleNode("UEdtr").InnerText);
                param[103] = new SqlParameter("@LEdtr", node.SelectSingleNode("LEdtr").InnerText);
                param[104] = new SqlParameter("@UEsen", node.SelectSingleNode("UEsen").InnerText);
                param[105] = new SqlParameter("@LEsen", node.SelectSingleNode("LEsen").InnerText);
                param[106] = new SqlParameter("@UEmmst", node.SelectSingleNode("UEmmst").InnerText);
                param[107] = new SqlParameter("@LEmmst", node.SelectSingleNode("LEmmst").InnerText);
            }
            db.executeSP(SP, param);


            //insert into IEPage3 table
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage3");
            foreach (XmlNode node in nodeList)
            {
                param = new SqlParameter[94];
                SP = "usp_tblPatientIEDetailPage3_insert_DV";

                param[0] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                param[1] = new SqlParameter("@GAIT", node.SelectSingleNode("GAIT").InnerText);
                param[2] = new SqlParameter("@Ambulates", node.SelectSingleNode("Ambulates").InnerText);
                param[3] = new SqlParameter("@Footslap", node.SelectSingleNode("Footslap").InnerText);
                param[4] = new SqlParameter("@Kneehyperextension", node.SelectSingleNode("Kneehyperextension").InnerText);
                param[5] = new SqlParameter("@Unabletohealwalk", node.SelectSingleNode("Unabletohealwalk").InnerText);
                param[6] = new SqlParameter("@Unabletotoewalk", node.SelectSingleNode("Unabletotoewalk").InnerText);
                param[7] = new SqlParameter("@Other", node.SelectSingleNode("Other").InnerText);
                param[8] = new SqlParameter("@DiagCervialBulgeDate", node.SelectSingleNode("DiagCervialBulgeDate").InnerText);
                param[9] = new SqlParameter("@DiagCervialBulgeStudy", node.SelectSingleNode("DiagCervialBulgeStudy").InnerText);
                param[10] = new SqlParameter("@DiagCervialBulge", node.SelectSingleNode("DiagCervialBulge").InnerText);
                param[11] = new SqlParameter("@DiagCervialBulgeText", node.SelectSingleNode("DiagCervialBulgeText").InnerText);
                param[12] = new SqlParameter("@DiagCervialBulgeHNP1", node.SelectSingleNode("DiagCervialBulgeHNP1").InnerText);
                param[13] = new SqlParameter("@DiagCervialBulgeHNP2", node.SelectSingleNode("DiagCervialBulgeHNP2").InnerText);
                param[14] = new SqlParameter("@DiagThoracicBulgeDate", DBNull.Value);
                param[15] = new SqlParameter("@DiagThoracicBulgeStudy", node.SelectSingleNode("DiagThoracicBulgeStudy").InnerText);
                param[16] = new SqlParameter("@DiagThoracicBulge", node.SelectSingleNode("DiagThoracicBulge").InnerText);
                param[17] = new SqlParameter("@DiagThoracicBulgeText", node.SelectSingleNode("DiagThoracicBulgeText").InnerText);
                param[18] = new SqlParameter("@DiagThoracicBulgeHNP1", node.SelectSingleNode("DiagThoracicBulgeHNP1").InnerText);
                param[19] = new SqlParameter("@DiagThoracicBulgeHNP2", node.SelectSingleNode("DiagThoracicBulgeHNP2").InnerText);
                param[20] = new SqlParameter("@DiagLumberBulgeDate", node.SelectSingleNode("DiagLumberBulgeDate").InnerText);
                param[21] = new SqlParameter("@DiagLumberBulgeStudy", node.SelectSingleNode("DiagLumberBulgeStudy").InnerText);
                param[22] = new SqlParameter("@DiagLumberBulge", node.SelectSingleNode("DiagLumberBulge").InnerText);
                param[23] = new SqlParameter("@DiagLumberBulgeText", node.SelectSingleNode("DiagLumberBulgeText").InnerText);
                param[24] = new SqlParameter("@DiagLumberBulgeHNP1", node.SelectSingleNode("DiagLumberBulgeHNP1").InnerText);
                param[25] = new SqlParameter("@DiagLumberBulgeHNP2", node.SelectSingleNode("DiagLumberBulgeHNP2").InnerText);
                param[26] = new SqlParameter("@DiagLeftShoulderDate", DBNull.Value);
                param[27] = new SqlParameter("@DiagLeftShoulderStudy", node.SelectSingleNode("DiagLeftShoulderStudy").InnerText);
                param[28] = new SqlParameter("@DiagLeftShoulder", node.SelectSingleNode("DiagLeftShoulder").InnerText);
                param[29] = new SqlParameter("@DiagLeftShoulderText", node.SelectSingleNode("DiagLeftShoulderText").InnerText);
                param[30] = new SqlParameter("@DiagRightShoulderDate", DBNull.Value);
                param[31] = new SqlParameter("@DiagRightShoulderStudy", node.SelectSingleNode("DiagRightShoulderStudy").InnerText);
                param[32] = new SqlParameter("@DiagRightShoulder", node.SelectSingleNode("DiagRightShoulder").InnerText);
                param[33] = new SqlParameter("@DiagRightShoulderText", node.SelectSingleNode("DiagRightShoulderText").InnerText);
                param[34] = new SqlParameter("@DiagLeftKneeDate", DBNull.Value);
                param[35] = new SqlParameter("@DiagLeftKneeStudy", node.SelectSingleNode("DiagLeftKneeStudy").InnerText);
                param[36] = new SqlParameter("@DiagLeftKnee", node.SelectSingleNode("DiagLeftKnee").InnerText);
                param[37] = new SqlParameter("@DiagLeftKneeText", node.SelectSingleNode("DiagLeftKneeText").InnerText);
                param[38] = new SqlParameter("@DiagRightKneeDate", DBNull.Value);
                param[39] = new SqlParameter("@DiagRightKneeStudy", node.SelectSingleNode("DiagRightKneeStudy").InnerText);
                param[40] = new SqlParameter("@DiagRightKnee", node.SelectSingleNode("DiagRightKnee").InnerText);
                param[41] = new SqlParameter("@DiagRightKneeText", node.SelectSingleNode("DiagRightKneeText").InnerText);
                param[42] = new SqlParameter("@DiagBrainDate", DBNull.Value);
                param[43] = new SqlParameter("@DiagBrainStudy", node.SelectSingleNode("DiagBrainStudy").InnerText);
                param[44] = new SqlParameter("@DiagBrain", node.SelectSingleNode("DiagBrain").InnerText);

                param[45] = new SqlParameter("@Other1Date", DBNull.Value);
                param[46] = new SqlParameter("@Other1Study", node.SelectSingleNode("Other1Study").InnerText);
                param[47] = new SqlParameter("@Other1Text", node.SelectSingleNode("Other1Text").InnerText);
                param[48] = new SqlParameter("@Other2Date", DBNull.Value);
                param[49] = new SqlParameter("@Other2Study", node.SelectSingleNode("Other2Study").InnerText);
                param[50] = new SqlParameter("@Other2Text", node.SelectSingleNode("Other2Text").InnerText);
                param[51] = new SqlParameter("@Other3Date", DBNull.Value);
                param[52] = new SqlParameter("@Other3Study", node.SelectSingleNode("Other3Study").InnerText);
                param[53] = new SqlParameter("@Other3Text", node.SelectSingleNode("Other3Text").InnerText);
                param[54] = new SqlParameter("@Other4Date", DBNull.Value);
                param[55] = new SqlParameter("@Other4Study", node.SelectSingleNode("Other4Study").InnerText);
                param[56] = new SqlParameter("@Other4Text", node.SelectSingleNode("Other4Text").InnerText);
                param[57] = new SqlParameter("@Other5Date", DBNull.Value);
                param[58] = new SqlParameter("@Other5Study", node.SelectSingleNode("Other5Study").InnerText);
                param[59] = new SqlParameter("@Other5Text", node.SelectSingleNode("Other5Text").InnerText);

                param[60] = new SqlParameter("@Other6Study", node.SelectSingleNode("Other6Study").InnerText);
                param[61] = new SqlParameter("@Other6Text", node.SelectSingleNode("Other6Text").InnerText);

                param[62] = new SqlParameter("@Other7Study", node.SelectSingleNode("Other7Study").InnerText);
                param[63] = new SqlParameter("@Other7Text", node.SelectSingleNode("Other7Text").InnerText);

                param[64] = new SqlParameter("@Procedures", node.SelectSingleNode("Procedures").InnerText);
                param[65] = new SqlParameter("@Acupuncture", node.SelectSingleNode("Acupuncture").InnerText);
                param[66] = new SqlParameter("@Chiropratic", node.SelectSingleNode("Chiropratic").InnerText);
                param[67] = new SqlParameter("@PhysicalTherapy", node.SelectSingleNode("PhysicalTherapy").InnerText);
                param[68] = new SqlParameter("@AvoidHeavyLifting", node.SelectSingleNode("AvoidHeavyLifting").InnerText);
                param[69] = new SqlParameter("@Carrying", node.SelectSingleNode("Carrying").InnerText);
                param[70] = new SqlParameter("@ExcessiveBend", node.SelectSingleNode("ExcessiveBend").InnerText);
                param[71] = new SqlParameter("@ProlongedSitStand", node.SelectSingleNode("ProlongedSitStand").InnerText);
                param[72] = new SqlParameter("@CareOther", node.SelectSingleNode("CareOther").InnerText);
                param[73] = new SqlParameter("@Cardiac", node.SelectSingleNode("Cardiac").InnerText);
                param[74] = new SqlParameter("@WeightBearing", node.SelectSingleNode("WeightBearing").InnerText);
                param[75] = new SqlParameter("@Precautions", node.SelectSingleNode("Precautions").InnerText);
                param[76] = new SqlParameter("@EducationProvided", node.SelectSingleNode("EducationProvided").InnerText);
                param[77] = new SqlParameter("@ViaPhysician", node.SelectSingleNode("ViaPhysician").InnerText);
                param[78] = new SqlParameter("@ViaPrintedMaterial", node.SelectSingleNode("ViaPrintedMaterial").InnerText);
                param[79] = new SqlParameter("@ViaWebsite", node.SelectSingleNode("ViaWebsite").InnerText);
                param[80] = new SqlParameter("@ViaVideo", node.SelectSingleNode("ViaVideo").InnerText);
                param[81] = new SqlParameter("@ConsultNeuro", node.SelectSingleNode("ConsultNeuro").InnerText);
                param[82] = new SqlParameter("@ConsultOrtho", node.SelectSingleNode("ConsultOrtho").InnerText);
                param[83] = new SqlParameter("@ConsultPsych", node.SelectSingleNode("ConsultPsych").InnerText);
                param[84] = new SqlParameter("@ConsultPodiatrist", node.SelectSingleNode("ConsultPodiatrist").InnerText);
                param[85] = new SqlParameter("@ConsultOther", node.SelectSingleNode("ConsultOther").InnerText);
                param[86] = new SqlParameter("@FollowUpIn", node.SelectSingleNode("FollowUpIn").InnerText);
                param[87] = new SqlParameter("@CreatedBy", "Default");
                param[88] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
                param[89] = new SqlParameter("@IsViaVedio", node.SelectSingleNode("IsViaVedio").InnerText);
                param[90] = new SqlParameter("@OtherMedicine", "");
                // param[91] = new SqlParameter("@DiagBrainText",System.Text);
                param[91] = new SqlParameter("@Other6Date", DBNull.Value);
                param[92] = new SqlParameter("@Other7Date", DBNull.Value);
                param[93] = new SqlParameter("@MedicationRx_ID", "");
            }
            db.executeSP(SP, param);

        }
        catch (Exception e)
        {

        }
    }
    protected void txt_attorney_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hattorney.Value) == false)
            if (string.IsNullOrWhiteSpace(txt_attorney.Text))
            {
                txt_attorney_ph.Text = string.Empty;
                txt_attorney_ph.Focus();
            }
            else
            {

                DataSet ds = db.selectData("select Telephone from tblAttorneys where Attorney_ID=" + hattorney.Value);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString().Trim() != "")
                {
                    txt_attorney_ph.Text = ds.Tables[0].Rows[0][0].ToString();
                    ddl_casetype.Focus();
                }
                else
                {
                    txt_attorney_ph.Text = string.Empty;
                    txt_attorney_ph.Focus();
                }
            }
    }
    protected void txt_pharmacy_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hpharmacy.Value) == false)
        {
            //DataSet ds = db.selectData("select ISNULL(Address1,'') + ISNULL(', ' + Address2,'') + ISNULL(', ' + City,'') + ISNULL(', ' + State,'') + ISNULL(', ' + Zip,'') as PhAddress from tblPharmacy where Pharmacy_ID=" + hpharmacy.Value);
            DataSet ds = db.selectData("select ISNULL(Address1,'') as PhAddress from tblPharmacy where Pharmacy_ID=" + hpharmacy.Value);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString().Trim() != "")
            {
                txt_pharmacy_address.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                txt_pharmacy_address.Text = string.Empty;
                txt_pharmacy_address.Focus();
            }
        }
    }

    protected void lbtnProcedureDetails_Click(object sender, EventArgs e)
    {
        if (Session["PatientIE_ID"] != null)
        {
            Response.Redirect("~/TimeSheet.aspx?PId=" + Convert.ToString(Session["PatientIE_ID"]));
        }
    }
}