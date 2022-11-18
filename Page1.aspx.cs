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

            if (Request["id"] != null)
            {
                bindEditData(Request.QueryString["id"]);
                Session["PID"] = Request.QueryString["PID"];

                PageMainMaster master = (PageMainMaster)this.Master;
                master.bindData(Request.QueryString["id"]);


                //txt_fname.ReadOnly = true;
                //txt_lname.ReadOnly = true;
                //txt_mname.ReadOnly = true;
            }
            else
            {
                //Session.Remove("bodyPartsList");
                if (Session["PatientIE_ID"] != null)
                {
                    Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Edit IE " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);
                    bindEditData(Session["PatientIE_ID"].ToString());
                    Session["bodyPartsList"] = null;
                    PageMainMaster master = (PageMainMaster)this.Master;
                    master.bindData(Session["PatientIE_ID"].ToString());


                }
                else
                {
                    if (Session["Providers"] != null)
                    {
                        txtMAProviders.Text = Convert.ToString(Session["Providers"]);
                    }
                    Session["bodyPartsList"] = null;
                    //s Session.Remove("bodyPartsList");
                }
            }

            ddl_location.Focus();
        }
        //PatientIntime.Text = DateTime.Now.ToString();
        Logger.Info(Session["uname"].ToString() + "- Visited in  PatientDetails for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    [WebMethod]
    public static string[] getFirstName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select distinct FirstName from tblPatientMaster where FirstName like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                fname = ds.Tables[0].Rows[i]["FirstName"].ToString();
                patient.Add(string.Format("{0}-{1}", fname, fname));
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

        DataSet ds = db.selectData("select distinct LastName from tblPatientMaster where LastName like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {

                fname = ds.Tables[0].Rows[i]["LastName"].ToString();
                patient.Add(string.Format("{0}-{1}", fname, fname));
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
            ddState.DataValueField = ds.Tables[0].Rows[0]["State"].ToString();
            txt_zip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
            txt_home_ph.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txt_mobile.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();



            txt_DOB.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
        }
    }

    private void bindEditData(string PatientIEid)
    {

        Session["PatientIE_ID"] = PatientIEid;
        string query = "select * from View_PatientIE VP left join tblAdjuster A on VP.Adjuster_Id=A.Adjuster_Id  where PatientIE_ID=" + PatientIEid;
        if (!string.IsNullOrEmpty(Convert.ToString(PatientIEid)) && File.Exists(Server.MapPath(Convert.ToString("~/Userimages/" + PatientIEid + ".jpeg"))))
        {
            ProfileImage.ImageUrl = Convert.ToString("~/Userimages/" + PatientIEid + ".jpeg");
        }
        else
        {
            ProfileImage.ImageUrl = "~/Userimages/default.png";
        }

        DataSet ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["FirstNameIE"] = txt_fname.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
            Session["LastNameIE"] = txt_lname.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
            txt_SSN.Text = ds.Tables[0].Rows[0]["SSN"].ToString();

            txtEmpPhone.Text = ds.Tables[0].Rows[0]["EmpPhone"].ToString();
            txtEmpAdd.Text = ds.Tables[0].Rows[0]["EmpAddress"].ToString();
            txtEmpFax.Text = ds.Tables[0].Rows[0]["EmpFax"].ToString();
            txtEmpName.Text = ds.Tables[0].Rows[0]["EmpName"].ToString();
            txt_attorney_ph.Text = ds.Tables[0].Rows[0]["AttorneyPhno"].ToString();
            txt_attorney.Text = ds.Tables[0].Rows[0]["Attorney"].ToString();
            txt_MC.Text = ds.Tables[0].Rows[0]["MC"].ToString();
            txtNote.Text = ds.Tables[0].Rows[0]["Note"].ToString();
            txtAlertNote.Text = ds.Tables[0].Rows[0]["AlertNote"].ToString();
            txt_Email.Text = ds.Tables[0].Rows[0]["eMail"].ToString();

            txt_DOA.Text = ds.Tables[0].Rows[0]["DOA"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[0]["DOA"]).ToString("MM/dd/yyyy") : "";
            txt_DOV.Text = ds.Tables[0].Rows[0]["DOE"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[0]["DOE"]).ToString("MM/dd/yyyy") : null;
            if (!string.IsNullOrEmpty(txt_DOV.Text))
                Session["DVLbl"] = ds.Tables[0].Rows[0]["DOE"];
            else
                Session["DVLbl"] = null;

            if (!string.IsNullOrEmpty(txt_DOA.Text))
                Session["dtDOA"] = ds.Tables[0].Rows[0]["DOA"];
            else
                Session["dtDOA"] = null;


            txt_ins_co.Text = ds.Tables[0].Rows[0]["InsCo"].ToString();
            txt_InsAdd.Text = ds.Tables[0].Rows[0]["InsAddress1"].ToString();
            txt_InsTel.Text = ds.Tables[0].Rows[0]["InsPhone"].ToString();

            txt_claim.Text = ds.Tables[0].Rows[0]["ClaimNumber"].ToString();

            txtMAProviders.Text = ds.Tables[0].Rows[0]["MA_Providers"].ToString();

            ddl_location.ClearSelection();

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Location"].ToString()))
                ddl_location.Items.FindByText(ds.Tables[0].Rows[0]["Location"].ToString()).Selected = true;

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Handedness"].ToString()))
                ddlHand.Items.FindByText(ds.Tables[0].Rows[0]["Handedness"].ToString()).Selected = true;


            Session["LocLbl"] = ds.Tables[0].Rows[0]["Location"].ToString();
            ddl_gender.ClearSelection();
            ddl_gender.Items.FindByValue(ds.Tables[0].Rows[0]["Sex"].ToString()).Selected = true;
            Session["Gender"] = ds.Tables[0].Rows[0]["Sex"].ToString();

            // ddl_casetype.ClearSelection();
            string compensation = Convert.ToString(ds.Tables[0].Rows[0]["Compensation"]);
            Session["compensation"] = Convert.ToString(ds.Tables[0].Rows[0]["Compensation"]);
            if (!string.IsNullOrEmpty(compensation))
                ddl_casetype.Items.FindByValue(compensation).Selected = true;

            txt_mname.Text = ds.Tables[0].Rows[0]["MiddleName"].ToString();
            txt_add.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
            txt_city.Text = ds.Tables[0].Rows[0]["City"].ToString();
            PatientIntime.Text = ds.Tables[0].Rows[0]["PatientIntime"].ToString();
            PatientOuttime.Text = ds.Tables[0].Rows[0]["PatientOuttime"].ToString();
            string filePath = Server.MapPath("~/Xml/USStates.xml");
            using (DataSet ds1 = new DataSet())
            {
                ds1.ReadXml(filePath);
                ddState.DataSource = ds1;
                ddState.DataTextField = "name";
                ddState.DataValueField = "name";
                ddState.DataBind();
                DataTable Store = ds1.Tables["state"];
                var result = from dm in Store.AsEnumerable()
                             where ((dm.Field<string>("name") == ds.Tables[0].Rows[0]["State"].ToString()))
                             select dm;
                if (result.Any())
                {
                    ddState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString(); ;
                }
                else
                { ddState.Items.Insert(0, new ListItem("--Select State--", "0")); }
            }

            // ddl_State.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();


            txt_zip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
            txt_policy.Text = ds.Tables[0].Rows[0]["policy_no"].ToString();
            txt_work_ph.Text = ds.Tables[0].Rows[0]["work_phone"].ToString();
            txt_mobile.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
            txt_home_ph.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txt_pharmacy_address.Text = ds.Tables[0].Rows[0]["PharmaAddress"].ToString();
            txt_pharmacy_name.Text = ds.Tables[0].Rows[0]["Pharmacy"].ToString();
            txt_adju_fax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
            txt_adju_email.Text = ds.Tables[0].Rows[0]["EmailAddress"].ToString();

            if (ds.Tables[0].Columns.Contains("Adjuster"))
            {
                string ss = ds.Tables[0].Rows[0]["Adjuster"].ToString();
                if (ss.Contains('~'))
                {
                    string[] s = ss.Split('~');
                    if (s.Length == 3)
                    {
                        txt_adjuster.Text = s[0];
                        txt_adjuster_no.Text = s[1];
                        txtAdjusterExtension.Text = s[2] == null ? "" : s[2];
                    }
                    else
                    {
                        txt_adjuster.Text = s[0];
                        txt_adjuster_no.Text = s[1];
                    }
                }
                else
                {
                    txt_adjuster.Text = ds.Tables[0].Rows[0]["Adjuster"].ToString();
                }
            }
            txt_WCBGroup.Text = ds.Tables[0].Rows[0]["WCBGroup"].ToString();



            if (ds.Tables[0].Rows[0]["DOB"] != DBNull.Value)
            {
                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString());
                txt_DOB.Text = dob.ToString("MM/dd/yyyy");
            }


            if (ds.Tables[0].Rows[0]["Vaccinated"] != DBNull.Value)
            {
                ddlVaccinated.ClearSelection();
                ddlVaccinated.Items.FindByValue(ds.Tables[0].Rows[0]["Vaccinated"].ToString()).Selected = true;

            }

            if (!string.IsNullOrEmpty(txtAlertNote.Text))
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "setAlert('" + txtAlertNote.Text + "');", true);

            getPatientID(txt_fname.Text, txt_lname.Text);



        }

    }

    protected void txt_lname_TextChanged(object sender, EventArgs e)
    {
        // string lastname = txt_lname.Text;
        //string l= hfpatientId.Value;
        //if (l != null & l!="")
        //{
        //   //bindData1(Convert.ToInt32(l));
        //    DBHelperClass db = new DBHelperClass();
        // List<string> patient = new List<string>();

        // DataSet ds = db.selectData("select distinct PatientIE_ID from tblPatientIE where Patient_ID = " + l );
        // if (ds.Tables[0].Rows.Count > 0)
        // {
        //     bindEditData(ds.Tables[0].Rows[0]["PatientIE_ID"].ToString());
        // }

        //}
        //if (!string.IsNullOrEmpty(txt_fname.Text))
        //{
        //    bindData();

        //}
        txt_fname.Focus();
    }
    protected void txt_fname_TextChanged(object sender, EventArgs e)
    {
        //if (!string.IsNullOrEmpty(txt_lname.Text))
        //{
        //    bindData();
        txt_mname.Focus();
        //}
    }

    private void bindLocation()
    {
        DataSet ds = new DataSet();

        string query = "select Location,Location_ID from tblLocations where Is_Active='True' ";

        if (Request["id"] == null && !string.IsNullOrEmpty(Session["Locations"].ToString()))
        {
            query = query + " and Location_ID in (" + Session["Locations"] + ")";
        }
        query = query + " Order By Location";

        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- Location --", "0"));

            DataSet locds = new DataSet();
            locds.ReadXml(Server.MapPath("~/LocationXML.xml"));

            ddl_location.ClearSelection();
            //ddl_location.Items.FindByValue(locds.Tables[0].Rows[0][1].ToString()).Selected = true;
            //ddl_location.Items.FindByValue("2").Selected = true;
        }
        if (Session["Location"] != null)
        {
            ddl_location.SelectedValue = Convert.ToString(Session["Location"]);
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/Compensations/Compensation"))
        {
            ddl_casetype.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        //ds = db.selectData("SELECT DISTINCT Compensation FROM tblPatientIE WHERE (Compensation <> '') OR (Compensation <> NULL) Order By Compensation");
        //  if (ds.Tables[0].Rows.Count > 0)
        //  {
        //      ddl_casetype.DataValueField = "Compensation";
        //      ddl_casetype.DataTextField = "Compensation";

        //      ddl_casetype.DataSource = ds;
        //      ddl_casetype.DataBind();


        //  }NF, WC, Lien
        //ddl_casetype.Items.Insert(0, new ListItem("-- Case Type --", "0"));
        //ddl_casetype.Items.Add(new ListItem("MM", "MM"));
        //ddl_casetype.Items.Add(new ListItem("NF", "NF"));
        //ddl_casetype.Items.Add(new ListItem("WC", "WC"));
        //ddl_casetype.Items.Add(new ListItem("Lien", "Lien"));

        string filePath = Server.MapPath("~/Xml/USStates.xml");
        using (DataSet ds1 = new DataSet())
        {
            ds1.ReadXml(filePath);
            ddState.DataSource = ds1;
            ddState.DataTextField = "name";
            ddState.DataValueField = "name";
            ddState.DataBind();
            ddState.Items.Insert(0, new ListItem("--Select State--", "0"));
        }
    }
    protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txt_DOB.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void CustomValidator2_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txt_DOA.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void CustomValidator3_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txt_DOV.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        //try
        //{
        string SP = "";
        SqlParameter[] param = null;

        if (Request["id"] != null || Session["PatientIE_ID"] != null)
        {

            param = new SqlParameter[47];
            SP = "usp_update_PatientIE";
        }
        else
        {
            param = new SqlParameter[46];
            SP = "usp_insert_PatientIE";
        }
        //   param[0] = new SqlParameter("@FirstName", node.SelectSingleNode("FirstName").InnerText);
        //param[0] = new SqlParameter("@FirstName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_fname.Text.ToLower()));
        //param[1] = new SqlParameter("@LastName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_lname.Text.ToLower()));
        //param[2] = new SqlParameter("@MiddleName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_mname.Text.ToLower()));
        param[0] = new SqlParameter("@FirstName", UppercaseFirst(txt_fname.Text));
        param[1] = new SqlParameter("@LastName", UppercaseFirst(txt_lname.Text));
        param[2] = new SqlParameter("@MiddleName", txt_mname.Text);
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
        param[8] = new SqlParameter("@State", ddState.SelectedValue.Trim());
        param[9] = new SqlParameter("@Zip", txt_zip.Text);
        param[10] = new SqlParameter("@Handedness", ddlHand.SelectedItem.Text);
        param[11] = new SqlParameter("@Createdby", "");
        param[12] = new SqlParameter("@IncCo", txt_ins_co.Text);
        param[13] = new SqlParameter("@Attorney", txt_attorney.Text);
        param[14] = new SqlParameter("@Attorney_phone", txt_attorney_ph.Text);
        param[15] = new SqlParameter("@DOV", !string.IsNullOrEmpty(txt_DOV.Text) ? txt_DOV.Text : null);

        if (string.IsNullOrWhiteSpace(txt_DOA.Text))
        {

            param[16] = new SqlParameter("@DOA", DBNull.Value);
            Session["OccurOn"] = false;
        }
        else
        {
            param[16] = new SqlParameter("@DOA", txt_DOA.Text);
            Session["OccurOn"] = true;
        }
        Session["LocLbl"] = ddl_location.SelectedItem.Text;
        param[17] = new SqlParameter("@Location", ddl_location.SelectedItem.Value);
        param[18] = new SqlParameter("@Claim", txt_claim.Text);
        param[19] = new SqlParameter("@Policy", txt_policy.Text);


        if (string.IsNullOrWhiteSpace(txt_DOB.Text))
        {
            param[20] = new SqlParameter("@AGE", DBNull.Value);
        }
        else
        {
            param[20] = new SqlParameter("@AGE", calculateAge(Convert.ToDateTime(txt_DOB.Text)));
        }


        param[21] = new SqlParameter("@SSN", txt_SSN.Text);
        param[22] = new SqlParameter("@Phone2", txt_mobile.Text);
        param[23] = new SqlParameter("@work_phone", txt_work_ph.Text);
        param[24] = new SqlParameter("@policy_no", txt_policy.Text);
        param[25] = new SqlParameter("@Compensation", ddl_casetype.SelectedItem.Value);
        param[26] = new SqlParameter("@Pharmacy", txt_pharmacy_name.Text.Trim());
        param[27] = new SqlParameter("@PharmaAddress", txt_pharmacy_address.Text.Trim());
        param[28] = new SqlParameter("@Adjuster", txt_adjuster.Text + "~" + txt_adjuster_no.Text + "~" + txtAdjusterExtension.Text);
        param[29] = new SqlParameter("@WCBGroup", txt_WCBGroup.Text);
        param[30] = new SqlParameter("@MA_Providers", txtMAProviders.Text.Trim());
        param[31] = new SqlParameter("@PatientIntime", PatientIntime.Text.Trim());
        param[32] = new SqlParameter("@PatientOuttime", PatientOuttime.Text.Trim());
        param[33] = new SqlParameter("@Adjuster_email", txt_adju_email.Text.Trim());
        param[34] = new SqlParameter("@Adjuster_fax", txt_adju_fax.Text.Trim());
        param[35] = new SqlParameter("@EmpName", txtEmpName.Text.Trim());
        param[36] = new SqlParameter("@EmpAddress", txtEmpAdd.Text.Trim());
        param[37] = new SqlParameter("@EmpPhone", txtEmpFax.Text.Trim());
        param[38] = new SqlParameter("@EmpFax", txtEmpPhone.Text.Trim());
        param[39] = new SqlParameter("@MC", txt_MC.Text);
        param[40] = new SqlParameter("@Note", txtNote.Text);
        param[41] = new SqlParameter("@Email", txt_Email.Text);
        param[42] = new SqlParameter("@InsAddress", txt_InsAdd.Text);
        param[43] = new SqlParameter("@InsTelephone", txt_InsTel.Text);
        param[44] = new SqlParameter("@Vaccinated", ddlVaccinated.SelectedItem.Value);
        param[45] = new SqlParameter("@AlertNote", txtAlertNote.Text);
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
            param[46] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"]);
        }

        int val = db.executeSP(SP, param);

        if (val > 0)
        {
            Session["DVLbl"] = txt_DOV.Text;

            Session["Gender"] = ddl_gender.SelectedItem.Value;
            if (Request["id"] == null)
            {
                lblMessage.InnerHtml = "Patient IE Created successfuly.";
                lblmess.Text = "Patient IE Created successfuly.";
                lblmess.ForeColor = System.Drawing.Color.Green;
                Session["PatientIE_ID"] = val;
                Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Create IE - Page1 " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);
                //set default values in page 1,2,3
                // DefaultValue(Session["PatientIE_ID"].ToString(), "Page1");
                DefaultValue(Session["PatientIE_ID"].ToString(), "Page2");
                //DefaultValue(Session["PatientIE_ID"].ToString(), "Page3");

                Session["FirstNameIE"] = txt_fname.Text;
                Session["LastNameIE"] = txt_lname.Text;
                Session["DVLbl"] = txt_DOV.Text;

                if (!string.IsNullOrEmpty(txt_DOA.Text))
                    Session["dtDOA"] = txt_DOA.Text;
                else
                    Session["dtDOA"] = null;

                Session["LocLbl"] = ddl_location.SelectedItem.Text;
                Session["compensation"] = ddl_casetype.SelectedItem.Text;

            }
            else
            {
                lblMessage.InnerHtml = "Patient IE Updated successfuly.";
                lblmess.Text = "Patient IE Created successfuly.";
                lblmess.ForeColor = System.Drawing.Color.Green;
                Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Update IE - Page1 " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);
            }
            getPatientID(txt_fname.Text, txt_lname.Text);
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

        if (pageHDN.Value != null || pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        else
        {
            Response.Redirect("Page2.aspx");
        }

        //}
        //catch (Exception ex)
        //{
        //    db.LogError(ex);
        //}
        //finally
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
        //    if (pageHDN.Value != null || pageHDN.Value !="")
        //    {
        //        Response.Redirect(pageHDN.Value.ToString());
        //    }
        //    else
        //    {
        //        Response.Redirect("Page2.aspx");
        //    }

        //}
        //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
        //Response.Redirect("Page2.aspx");

    }

    [WebMethod]
    public static string txt_attorney_TextChanged(string prefix)
    {

        DBHelperClass db = new DBHelperClass();
        if (string.IsNullOrEmpty(prefix) == true)
        {
            return string.Empty;

        }
        else
        {

            DataSet ds = db.selectData("select Telephone from tblAttorneys where Attorney_ID=" + prefix);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString().Trim() != "")
            {
                return ds.Tables[0].Rows[0][0].ToString();
                //ddl_casetype.Focus();
                // ddl_casetype.Focus();
            }
            else
            {
                return string.Empty;

                //txt_attorney_ph.Focus();
            }
        }

        //txt_attorney_ph.Focus();
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

    #region DefaultValue
    /// <summary>
    /// Method to insert default value in the database.
    /// </summary>
    /// <param name="id">PatientIEId to insert or update the records in the tables.</param>
    /// <param name="bodyPart">Body part in which the value has to insert.</param>
    private void DefaultValue(string id, string bodyPart = null)
    {
        try
        {
            string SP = "";
            SqlParameter[] param = null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            XmlNodeList nodeList;
            switch (bodyPart)
            {
                case "Page1":
                    {
                        #region Page1
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
                            param[46] = new SqlParameter("@DischargedOn", DBNull.Value);
                            param[47] = new SqlParameter("@AccidentDetail", "");
                            param[48] = new SqlParameter("@DoctorSeen", "");
                        }
                        #endregion
                        break;
                    }
                case "Page2":
                    {
                        #region Page2
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
                        #endregion
                        break;
                    }
                case "Page3":
                    {
                        #region Page3
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
                            param[8] = new SqlParameter("@DiagCervialBulgeDate", DBNull.Value);
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
                            param[20] = new SqlParameter("@DiagLumberBulgeDate", DBNull.Value);
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
                        #endregion
                        break;
                    }
            }
            //Set the Default value.
            db.executeSP(SP, param);
        }
        catch (Exception e)
        {
            db.LogError(e);
            throw;
        }
    }
    #endregion

    protected void def_State_Click(object sender, EventArgs e)
    {
        ddState.SelectedValue = "NJ";
    }
    protected void txt_DOA_TextChanged(object sender, EventArgs e)
    {
        txt_lname.Focus();
    }

    //protected void txt_pharmacy_name_TextChanged(object sender, EventArgs e)
    //{
    //    txt_pharmacy_address.Focus();
    //}
    protected void txt_DOV_TextChanged(object sender, EventArgs e)
    {
        txt_DOA.Focus();
    }

    private int calculateAge(DateTime bday)
    {
        int age = 0;
        age = DateTime.Now.Year - bday.Year;
        if (DateTime.Now.DayOfYear < bday.DayOfYear)
            age = age - 1;

        return age;
    }

    private void getPatientID(string fname, string lname)
    {
        string query = "select Patient_ID from [dbo].[tblPatientMaster] where FirstName='" + fname + "' and LastName='" + lname + "'";
        DataSet ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Session["PatientId"] = ds.Tables[0].Rows[0][0].ToString();
        }
    }

    //public string UppercaseFirst(string str)
    //{
    //    if (string.IsNullOrEmpty(str))
    //        return string.Empty;
    //    return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    //}

    public string UppercaseFirst(string str)
    {
        char[] array = str.ToCharArray();
        // Handle the first letter in the string.  
        if (array.Length >= 1)
        {
            if (char.IsLower(array[0]))
            {
                array[0] = char.ToUpper(array[0]);
            }
        }
        // Scan through the letters, checking for spaces.  
        // ... Uppercase the lowercase letters following spaces.  
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i - 1] == ' ')
            {
                if (char.IsLower(array[i]))
                {
                    array[i] = char.ToUpper(array[i]);
                }
            }
        }
        return new string(array);
    }


    public bool checkPatientName()
    {
        bool _result = true;
        try
        {


        }
        catch (Exception ex)
        {

        }

        return _result;
    }

}