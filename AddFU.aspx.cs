using IntakeSheet.BLL;
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
using System.Text.RegularExpressions;
using System.Configuration;

public partial class AddFU : System.Web.UI.Page
{
    public int PatientIE_Id;
    DBHelperClass db = new DBHelperClass();
    int count = 0;
    private bool Chk_r_Shoulder = false;
    private bool Chk_L_Shoulder = false;
    private bool Chk_Neck = false;
    private bool Chk_Midback = false;
    private bool Chk_lowback = false;
    private bool Chk_r_Keen = false;
    private bool Chk_L_Keen = false;
    private bool Chk_r_Elbow = false;
    private bool Chk_l_Elbow = false;
    private bool Chk_r_Wrist = false;
    private bool Chk_l_Wrist = false;
    private bool Chk_r_Hip = false;
    private bool Chk_l_Hip = false;
    private bool Chk_r_ankle = false;
    private bool Chk_l_ankle = false;
    private static string redirectpage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["UserId"] == null)
        //{
        //    Response.Redirect("~/Login.aspx");
        //}
        //
        //ddd
        //Session["patientFUId"] = "";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!Page.IsPostBack)
        {
            if (Request.UrlReferrer != null) { redirectpage = Convert.ToString(System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath)); }

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("WorkStatus", typeof(string)) });
            dt.Rows.Add("Able to go back to work");
            dt.Rows.Add("Working");
            dt.Rows.Add("Not Working");
            dt.Rows.Add("Partially Working");
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            bindLocations();
            bindInsuranceCo();
            bindDropdown();
            bindHTMLEdit();
            if (Request.QueryString["PID"] != null)
            {

                PatientIE_Id = Convert.ToInt32(Request.QueryString["PID"]);
                Session["PatientIE_Id2"] = PatientIE_Id;
                Session["PatientIE_ID"] = Session["PatientIE_Id2"];
                bindFUDetails(PatientIE_Id);
                hfPatientIEId.Value = PatientIE_Id.ToString();
                Session["bodyPartsList"] = null;
                AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
                master.bindData(Session["PatientIE_Id2"].ToString());
                bindPatientDetails(PatientIE_Id);

                if (!string.IsNullOrEmpty(Convert.ToString(Session["patientFUId"])) && !redirectpage.Equals("PatientIntakeList.aspx"))
                {
                    bindRestrictions(Convert.ToInt32(hfPatientIEId.Value), Convert.ToString(Session["patientFUId"]));
                }

                // Session["patientFUId"]=GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            }
            else if (Session["PatientIE_Id2"] != null)
            {

                hfPatientIEId.Value = Session["PatientIE_Id2"].ToString();
                hfPatientFUId.Value = Session["patientFUId"].ToString();
                Session["PatientIE_ID"] = Session["PatientIE_Id2"];
                bindFUDetails(Convert.ToInt32(hfPatientIEId.Value));
                AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
                master.bindData(Session["PatientIE_Id2"].ToString());
                bindPatientDetails((int)Session["PatientIE_Id2"]);

                if (!string.IsNullOrEmpty(Convert.ToString(Session["patientFUId"])) && !redirectpage.Equals("PatientIntakeList.aspx"))
                {
                    bindRestrictions(Convert.ToInt32(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]));
                }

                //bindRestrictions(Convert.ToInt32(Session["PatientIE_ID"]));
                // Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            }
            else
                PatientIE_Id = 0;
            if (Session["Location"] != null)
            {
                ddlLoaction.SelectedValue = Convert.ToString(Session["Location"]);
            }
            if (Session["Providers"] != null)
            {
                txtMAProviders.Text = Convert.ToString(Session["Providers"]);
            }

        }
        //PatientIntime.Text = DateTime.Now.ToString();
    }

    protected void bindLocations()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetLocations", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if (!string.IsNullOrEmpty(sdr["Location"].ToString()))
                    ddlLoaction.Items.Add(new ListItem(sdr["Location"].ToString(), sdr["Location_ID"].ToString()));
            }
            con.Close();
        }

        //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT DISTINCT Compensation FROM tblPatientIE WHERE (Compensation <> '') OR (Compensation <> NULL) Order By Compensation", con);
        //    cmd.CommandType = CommandType.Text;
        //    con.Open();
        //    SqlDataReader sdr = cmd.ExecuteReader();
        //    while (sdr.Read())
        //    {
        //        if (!string.IsNullOrEmpty(sdr["Compensation"].ToString()))
        //            ddlLoaction.Items.Add(new ListItem(sdr["Compensation"].ToString(), sdr["Compensation"].ToString()));
        //    }
        //    con.Close();


        //}


        //DataSet ds = db.selectData("SELECT DISTINCT Compensation FROM tblPatientIE WHERE (Compensation <> '') OR (Compensation <> NULL) Order By Compensation");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddl_casetype.DataValueField = "Compensation";
        //    ddl_casetype.DataTextField = "Compensation";

        //    ddl_casetype.DataSource = ds;
        //    ddl_casetype.DataBind();

        //    ddl_casetype.Items.Insert(0, new ListItem("-- Case Type --", "0"));
        //}
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/Compensations/Compensation"))
        {
            ddl_casetype.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
    }

    protected void bindInsuranceCo()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetInsCo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if (!string.IsNullOrEmpty(sdr["InsCo"].ToString()))
                    ddlInsuranceCo.Items.Add(new ListItem(sdr["InsCo"].ToString(), sdr["InsCo_ID"].ToString()));
            }
            con.Close();

            ddlInsuranceCo.Items.Insert(0, new ListItem("Select Insurance Co", "0"));
        }
    }

    protected void bindFUDetails(int PatientIE_Id)
    {
        BusinessLogic bl = new BusinessLogic();
        gvPatientFUDetails.DataSource = bl.GetFUDetails(PatientIE_Id);
        gvPatientFUDetails.DataBind();
    }

    protected void bindPatientDetails(int PatientIE_Id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            int Patient_Id = 0;
            SqlCommand cmd = new SqlCommand("GetFUdetails", con);
            cmd.Parameters.Add("@PatientIE_Id", PatientIE_Id);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                // if (!sdr.IsDBNull(0))
                //{
                hfPatientId.Value = sdr["Patient_ID"].ToString();
                //bindRestrictions(PatientIE_Id);
                // hfPatientFUId.Value = sdr["PatientFU_ID"].ToString();
                PMH.Text = sdr["PMH"].ToString();
                PSH.Text = sdr["PSH"].ToString();
                Medication.Text = sdr["Medications"].ToString();
                Allergies.Text = sdr["Allergies"].ToString();
                FamilyHistory.Text = sdr["FamilyHistory"].ToString();
                Session["FirstNameFU"] = txtFirstName.Text = sdr["FirstName"].ToString();
                Session["LastNameFU"] = txtLastName.Text = sdr["LastName"].ToString();
                ddlLoaction.SelectedValue = sdr["Location_ID"].ToString();
                if (!redirectpage.Equals("PatientIntakeList.aspx"))
                { txtDOV.Text = (sdr["DOE"] != DBNull.Value) ? Convert.ToDateTime(sdr["DOE"]).ToString("MM/dd/yyyy") : ""; }
                else
                { txtDOV.Text = DateTime.Now.ToString("MM/dd/yyyy"); }

                txtDOA.Text = (sdr["DOA"] != DBNull.Value) ? Convert.ToDateTime(sdr["DOA"]).ToString("MM/dd/yyyy") : "";


                if (!string.IsNullOrEmpty(txtDOA.Text))
                    Session["dtDOA"] = txtDOA.Text;
                else
                    Session["dtDOA"] = null;

                txtDOB.Text = (sdr["DOB"] != DBNull.Value) ? Convert.ToDateTime(sdr["DOB"]).ToString("MM/dd/yyyy") : "";
                rblGender.SelectedValue = sdr["Sex"].ToString();
                txtSSN.Text = sdr["SSN"].ToString();
                txtHomePh.Text = sdr["Phone"].ToString();
                txtWorkPh.Text = sdr["work_phone"].ToString();
                txtMobile.Text = sdr["Phone2"].ToString();
                txtAddress.Text = Convert.ToString(sdr["Address1"].ToString());
                txtCity.Text = sdr["City"].ToString();
                txtNote.Text = sdr["Note"].ToString();
                txtAlertNote.Text = sdr["AlertNote"].ToString();

                if (!string.IsNullOrEmpty(txtAlertNote.Text))
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "setAlert('" + txtAlertNote.Text + "');", true);


                chkDeniessmoking.Checked = !string.IsNullOrEmpty(sdr["DeniesSmoking"].ToString()) ? Convert.ToBoolean(sdr["DeniesSmoking"].ToString()) : false;
                chkDeniesdrinking.Checked = !string.IsNullOrEmpty(sdr["DeniesDrinking"].ToString()) ? Convert.ToBoolean(sdr["DeniesDrinking"].ToString()) : false;
                chkDeniesdrugs.Checked = !string.IsNullOrEmpty(sdr["DeniesDrugs"].ToString()) ? Convert.ToBoolean(sdr["DeniesDrugs"].ToString()) : false;
                chkSocialdrinking.Checked = !string.IsNullOrEmpty(sdr["DeniesSocialDrinking"].ToString()) ? Convert.ToBoolean(sdr["DeniesSocialDrinking"].ToString()) : false;
                cboReturnToWork.Text = Convert.ToString(sdr["ReturnToWork"]);
                cboRecevingPhyTherapy.Text = Convert.ToString(sdr["RecevingPhyTherapy"]);
                cboFeelPainRelief.Text = Convert.ToString(sdr["FeelPainRelief"]);
                string filePath = Server.MapPath("~/Xml/USStates.xml");
                using (DataSet ds = new DataSet())
                {
                    ds.ReadXml(filePath);
                    ddState.DataSource = ds;
                    ddState.DataTextField = "name";
                    ddState.DataValueField = "name";
                    ddState.DataBind();
                    DataTable Store = ds.Tables["state"];
                    var result = from dm in Store.AsEnumerable()
                                 where ((dm.Field<string>("name") == sdr["State"].ToString()))
                                 select dm;
                    if (result.Any())
                    {
                        ddState.SelectedValue = sdr["State"].ToString();
                    }
                    else
                    { ddState.Items.Insert(0, new ListItem("--Select State--", "0")); }
                }

                txtZip.Text = sdr["Zip"].ToString();
                txtClaim.Text = sdr["ClaimNumber"].ToString();
                txtPolicy.Text = sdr["policy_no"].ToString();
                ddl_casetype.ClearSelection();
                string compensation = Convert.ToString(sdr["Compensation"]);
                if (!string.IsNullOrEmpty(compensation))
                    ddl_casetype.Items.FindByValue(compensation).Selected = true;
                txtAttorneyName.Text = sdr["Attorney"].ToString();
                txtAttorneyPh.Text = sdr["Telephone"].ToString();
                if (!string.IsNullOrEmpty(sdr["InsCo_ID"].ToString()))
                    ddlInsuranceCo.SelectedValue = sdr["InsCo_ID"].ToString();

                Session["FirstNameIE"] = txtFirstName.Text;
                Session["LastNameIE"] = txtLastName.Text;
                Session["DVLbl"] = txtDOV.Text;
                Session["LocLbl"] = ddlLoaction.SelectedItem.Text;
                Session["compensation"] = ddl_casetype.Text;
            }
            //}
            con.Close();
            bindInjuredBodyParts(PatientIE_Id);
        }
    }

    protected void bindInjuredBodyParts(int PatientIE_Id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetInjuredBodyParts", con);
            cmd.Parameters.Add("@PatientIE_ID", PatientIE_Id);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                chk_Neck.Checked = Convert.ToBoolean(dr["Neck"]);
                chk_Midback.Checked = Convert.ToBoolean(dr["MidBack"]);
                chk_lowback.Checked = Convert.ToBoolean(dr["LowBack"]);
                chk_L_Shoulder.Checked = Convert.ToBoolean(dr["LeftShoulder"]);
                chk_r_Shoulder.Checked = Convert.ToBoolean(dr["RightShoulder"]);
                chk_L_Keen.Checked = Convert.ToBoolean(dr["LeftKnee"]);
                chk_r_Keen.Checked = Convert.ToBoolean(dr["RightKnee"]);
                chk_l_Elbow.Checked = Convert.ToBoolean(dr["LeftElbow"]);
                chk_r_Elbow.Checked = Convert.ToBoolean(dr["RightElbow"]);
                chk_l_Wrist.Checked = Convert.ToBoolean(dr["LeftWrist"]);
                chk_r_Wrist.Checked = Convert.ToBoolean(dr["RightWrist"]);
                chk_l_Hip.Checked = Convert.ToBoolean(dr["LeftHip"]);
                chk_r_Hip.Checked = Convert.ToBoolean(dr["RightHip"]);
                chk_l_ankle.Checked = Convert.ToBoolean(dr["LeftAnkle"]);
                chk_r_ankle.Checked = Convert.ToBoolean(dr["RightAnkle"]);

            }
            con.Close();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                int PatientIE_Id = 0;
                string FreeForm = GetFreeForm();
                SqlCommand cmd = new SqlCommand("nusp_SavePatientDetails", con);
                cmd.Parameters.Add("@Patient_Id", hfPatientId.Value);
                cmd.Parameters.Add("@PatientIE_ID", hfPatientIEId.Value);
                cmd.Parameters.Add("@FreeForm", FreeForm);
                if (!string.IsNullOrEmpty(hfPatientFUId.Value))
                {
                    cmd.Parameters.Add("@PatientFU_ID", hfPatientFUId.Value);
                    cmd.Parameters.Add("@isEditFU", true);
                }
                cmd.Parameters.Add("@DOE", DateTime.ParseExact(txtDOV.Text, "MM/dd/yyyy", null));
                if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
                    cmd.Parameters.Add("@DOA", DateTime.ParseExact(txtDOA.Text, "MM/dd/yyyy", null));
                if (!string.IsNullOrEmpty(txtDOB.Text.Trim()))
                    cmd.Parameters.Add("@DOB", DateTime.ParseExact(txtDOB.Text, "MM/dd/yyyy", null));
                cmd.Parameters.Add("@FirstName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower()));
                cmd.Parameters.Add("@LastName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower()));
                cmd.Parameters.Add("@Location_ID", ddlLoaction.SelectedValue);
                //if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
                //    cmd.Parameters.Add("@DOA", Convert.ToDateTime(txtDOA.Text.Trim()));
                cmd.Parameters.Add("@Sex", rblGender.SelectedValue);
                // cmd.Parameters.Add("@DOB", Convert.ToDateTime(txtDOB.Text.Trim()));
                cmd.Parameters.Add("@SSN", txtSSN.Text.Trim());
                cmd.Parameters.Add("@Phone", txtHomePh.Text.Trim());
                cmd.Parameters.Add("@work_phone", txtWorkPh.Text.Trim());
                cmd.Parameters.Add("@Phone2", txtMobile.Text.Trim());
                cmd.Parameters.Add("@Address", txtAddress.Text.Trim());
                cmd.Parameters.Add("@City", txtCity.Text.Trim());
                cmd.Parameters.Add("@Note", txtNote.Text.Trim());
                cmd.Parameters.Add("@AlertNote", txtAlertNote.Text.Trim());
                cmd.Parameters.Add("@State", ddState.SelectedValue.Trim());
                cmd.Parameters.Add("@Zip", txtZip.Text.Trim());
                cmd.Parameters.Add("@InsCo_ID", ddlInsuranceCo.SelectedValue);
                cmd.Parameters.Add("@ClaimNumber", txtClaim.Text.Trim());
                cmd.Parameters.Add("@policy_no", txtPolicy.Text.Trim());
                cmd.Parameters.Add("@Compensation", ddl_casetype.SelectedValue);
                cmd.Parameters.Add("@Attorney", txtAttorneyName.Text.Trim());
                cmd.Parameters.Add("@Telephone", txtAttorneyPh.Text.Trim());
                cmd.Parameters.Add("@MA_Providers", txtMAProviders.Text.Trim());
                cmd.Parameters.Add("@FollowedUpOn", txtFollowedUpOn.Text.Trim());
                cmd.Parameters.Add("@PMH", PMH.Text.Trim());
                cmd.Parameters.Add("@PSH", PSH.Text.Trim());
                cmd.Parameters.Add("@Medications", Medication.Text.Trim());
                cmd.Parameters.Add("@Allergies", Allergies.Text.Trim());
                cmd.Parameters.Add("@FamilyHistory", FamilyHistory.Text.Trim());
                cmd.Parameters.Add("@Neck", chk_Neck.Checked);
                cmd.Parameters.Add("@MidBack", chk_Midback.Checked);
                cmd.Parameters.Add("@LowBack", chk_lowback.Checked);
                cmd.Parameters.Add("@LShoulder", chk_L_Shoulder.Checked);
                cmd.Parameters.Add("@RShoulder", chk_r_Shoulder.Checked);
                cmd.Parameters.Add("@cbLKNee", chk_L_Keen.Checked);
                cmd.Parameters.Add("@cbRKnee", chk_r_Keen.Checked);
                cmd.Parameters.Add("@cbLElbow", chk_l_Elbow.Checked);
                cmd.Parameters.Add("@cbRElbow", chk_r_Elbow.Checked);
                cmd.Parameters.Add("@cbLWrist", chk_l_Wrist.Checked);
                cmd.Parameters.Add("@cbRWrist", chk_r_Wrist.Checked);
                cmd.Parameters.Add("@cblAnkle", chk_l_ankle.Checked);
                cmd.Parameters.Add("@cbRAnkle", chk_r_ankle.Checked);
                cmd.Parameters.Add("@cbLHip", chk_l_Hip.Checked);
                cmd.Parameters.Add("@cbRHip", chk_r_Hip.Checked);

                cmd.Parameters.Add("@DeniesSmoking", chkDeniessmoking.Checked);
                cmd.Parameters.Add("@DeniesDrinking", chkDeniesdrinking.Checked);
                cmd.Parameters.Add("@DeniesDrugs", chkDeniesdrugs.Checked);
                cmd.Parameters.Add("@DeniesSocialDrinking", chkSocialdrinking.Checked);
                cmd.Parameters.Add("@Vitals", txtVitals.Text);
                cmd.Parameters.Add("@PatientIntime", PatientIntime.Text);
                cmd.Parameters.Add("@PatientOuttime", PatientOuttime.Text);


                cmd.Parameters.Add("@ReturnToWork", cboReturnToWork.Text);
                cmd.Parameters.Add("@RecevingPhyTherapy", cboRecevingPhyTherapy.Text);
                cmd.Parameters.Add("@FeelPainRelief", cboFeelPainRelief.Text);

                SqlParameter outDirectoryId = new SqlParameter("@PatientFUIDS", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outDirectoryId);
                //if (cbNeck.Checked)
                //    cmd.Parameters.Add("@Neck", cbNeck.Checked);

                //if (cbMidBack.Checked)
                //    cmd.Parameters.Add("@MidBack", cbMidBack.Checked);

                //if (cbLowBack.Checked)
                //    cmd.Parameters.Add("@LowBack", cbLowBack.Checked);

                //if (cbRShoulder.Checked)
                //    cmd.Parameters.Add("@RShoulder", cbRShoulder.Checked);

                //if (cbLShoulder.Checked)
                //    cmd.Parameters.Add("@LShoulder", cbLShoulder.Checked);

                //if (cbRKnee.Checked)
                //    cmd.Parameters.Add("@cbRKnee", cbRKnee.Checked);

                //if (cbLKnee.Checked)
                //    cmd.Parameters.Add("@cbLKNee", cbLKnee.Checked);

                //if (cbLElbow.Checked)
                //    cmd.Parameters.Add("@cbLElbow", cbLElbow.Checked);

                //if (cbRElbow.Checked)
                //    cmd.Parameters.Add("@cbRElbow", cbRElbow.Checked);

                //if (cbLWrist.Checked)
                //    cmd.Parameters.Add("@cbLWrist", cbLWrist.Checked);

                //if (cbRWrist.Checked)
                //    cmd.Parameters.Add("@cbRWrist", cbRWrist.Checked);

                //if (cbLHip.Checked)
                //    cmd.Parameters.Add("@cbLHip", cbLHip.Checked);

                //if (cbRHip.Checked)
                //    cmd.Parameters.Add("@cbRHip", cbRHip.Checked);

                //if (cblAnkle.Checked)
                //    cmd.Parameters.Add("@cblAnkle", cblAnkle.Checked);

                //if (cbRAnkle.Checked)
                //    cmd.Parameters.Add("@cbRAnkle", cbRAnkle.Checked);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                int count = cmd.ExecuteNonQuery();







                string alertText = "Follow Up already exsists for this date. Edit the followup";
                if (count > 0)
                {
                    // hfPatientFUId.Value = count.ToString();
                    //Session["patientFUIdnewadded"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                    //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
                    //alertText = "Follow Up Added Successfully";

                    int DirectoryId = outDirectoryId.Value != null && !string.IsNullOrEmpty(Convert.ToString(outDirectoryId.Value)) ? Convert.ToInt32(outDirectoryId.Value) : 0;
                    if (DirectoryId > 0)
                    {
                        Session["patientFUId"] = Convert.ToString(DirectoryId);
                    }
                    alertText = "Follow Up Added Successfully";

                    savePage3FU(Session["patientFUId"].ToString());
                    string query = "select top 1 * from tblPage1FUHTMLContent where PateintFU_ID=" + (!string.IsNullOrEmpty(Session["patientFUId"].ToString()) ? Session["patientFUId"].ToString() : "0");
                    DataSet ds = db.selectData(query);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        query = "insert into tblPage1FUHTMLContent values(@degreeSectionHTML,@socialSectionHTML,@PatientIE_ID,@PatientFU_ID)";
                    }
                    else
                    {
                        query = "update tblPage1FUHTMLContent set degreeSectionHTML=@degreeSectionHTML,socialSectionHTML=@socialSectionHTML where PateintFU_ID=@PatientFU_ID";
                    }

                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                        command.Parameters.AddWithValue("@PatientFU_ID", Session["patientFUId"].ToString());
                        command.Parameters.AddWithValue("@degreeSectionHTML", hddegreeHTMLContent.Value);
                        command.Parameters.AddWithValue("@socialSectionHTML", hdsocialHTMLContent.Value);

                        connection.Open();
                        var results = command.ExecuteNonQuery();
                        connection.Close();
                    }

                }
                con.Close();
                if (pageHDN.Value != null)
                {
                    Response.Redirect(Convert.ToString(pageHDN.Value));
                }
                AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
                master.bindData(hfPatientIEId.Value);
                this.Page.RegisterStartupScript("Alert", "<script>javascript:alert('" + alertText + "')</script>");
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void savePage3FU(string patientFUId)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                string FreeForm = GetFreeForm();
                SqlCommand cmd = new SqlCommand("usp_saveFUPage3", con);
                cmd.Parameters.Add("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                cmd.Parameters.Add("@PatientFU_ID", patientFUId);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch (Exception ex)
        { }
    }


    protected string GetFreeForm()
    {
        string restrictions = string.Empty;
        foreach (ListItem s in cblRestictions.Items)
        {
            if (s.Selected)
            {
                restrictions += s.Value.ToLower() + ", ";
            }
        }
        string workStatus = string.Empty;
        foreach (RepeaterItem item in this.Repeater1.Items)
        {
            CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
            if (chk.Checked)
            {
                string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
                string degree = chk.Text;
                workStatus += degree.ToLower() + "-" + collageName.ToLower() + ", ";
            }
        }

        string FreeForm = string.Empty;
        if (rblDOD.SelectedIndex > -1)
        {
            FreeForm = "Degree of Disability^ " + rblDOD.SelectedValue;
        }
        if (!string.IsNullOrEmpty(restrictions))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Restrictions^ " + restrictions.TrimEnd(',');
        }
        if (!string.IsNullOrEmpty(txtOtherRestrictions.Text.Trim()))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Others^ " + txtOtherRestrictions.Text.Trim();
        }
        if (!string.IsNullOrEmpty(workStatus))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Work Status^ " + workStatus.TrimEnd(',');
        }
        if (!string.IsNullOrEmpty(txtFreeForm.Text))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += "~";
            }
            FreeForm += "Notes^ " + txtFreeForm.Text.TrimEnd(',');
        }
        return (!string.IsNullOrEmpty(FreeForm)) ? FreeForm : "None";
    }

    //protected string GetFreeForm()
    //{
    //    string restrictions = string.Empty;
    //    foreach (ListItem s in cblRestictions.Items)
    //    {
    //        if (s.Selected)
    //        {
    //            restrictions += s.Value.ToLower() + ", ";
    //        }
    //    }
    //    string workStatus = string.Empty;


    //    foreach (RepeaterItem item in this.Repeater1.Items)
    //    {
    //        CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
    //        if (chk.Checked)
    //        {
    //            string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
    //            string degree = chk.Text;
    //            workStatus += degree.ToLower() + "-" + collageName.ToLower() + ", ";

    //        }
    //    }

    //    string FreeForm = string.Empty;
    //    if (rblDOD.SelectedIndex > -1)
    //    {
    //        FreeForm = "Degree of Disability: " + rblDOD.SelectedValue;
    //    }
    //    if (!string.IsNullOrEmpty(restrictions))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Restrictions: " + restrictions.TrimEnd(',');
    //    }
    //    if (!string.IsNullOrEmpty(txtOtherRestrictions.Text.Trim()))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Others: " + txtOtherRestrictions.Text.Trim();
    //    }
    //    if (!string.IsNullOrEmpty(workStatus))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Work Status: " + workStatus.TrimEnd(',');
    //    }
    //    if (!string.IsNullOrEmpty(txtFreeForm.Text))
    //    {
    //        if (!string.IsNullOrEmpty(FreeForm))
    //        {
    //            FreeForm += ";";
    //        }
    //        FreeForm += "Notes: " + txtFreeForm.Text.TrimEnd(',');
    //    }

    //    return (!string.IsNullOrEmpty(FreeForm)) ? FreeForm : "None";
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PatientIntakeList.aspx");
    }

    protected void lbtnTimeSheet_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TimeSheet.aspx?PId=" + Request.QueryString["PId"]);
    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");

    }

    protected void gvPatientDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientFUDetails.PageIndex = e.NewPageIndex;
        bindFUDetails(Convert.ToInt32(hfPatientIEId.Value));
    }
    //#region Check box Event checked changed of body part.
    /// <summary>
    /// Event fires if check box chk_r_Shoulder checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Shoulder_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightShoulder" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Shoulder.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightShoulder";
                    d.Checked = true;
                    s.Add(d);
                    Session["RightShoulder"] = 1;
                }
                // if(s.AsEnumerable().Where(s=>s.Any=="pafe"))
                Chk_r_Shoulder = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightShoulder";
                    d.Checked = false;
                    s.Add(d);
                    Session["RightShoulder"] = 0;
                }
                Chk_r_Shoulder = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Shoulder.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "RightShoulder";
                d.Checked = true;
                s1.Add(d);
                Chk_r_Shoulder = true;
                Session["RightShoulder"] = 1;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "RightShoulder";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Shoulder = false;
                Session["RightShoulder"] = 0;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_L_Shoulder checked or unchecked in the form.
    /// </summary>
    protected void chk_L_Shoulder_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftShoulder" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_L_Shoulder.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftShoulder";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_L_Shoulder = true;
                Session["LeftShoulder"] = 1;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftShoulder");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftShoulder";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_L_Shoulder = false;
                Session["LeftShoulder"] = 0;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_L_Shoulder.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "LeftShoulder";
                d.Checked = true;
                s1.Add(d);

                Chk_L_Shoulder = true;
                Session["LeftShoulder"] = 1;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftShoulder");
                BodyParts d = new BodyParts();
                d.Parts = "LeftShoulder";
                d.Checked = false;
                s1.Add(d);

                Chk_L_Shoulder = false;
                Session["LeftShoulder"] = 0;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_Neck checked or unchecked in the form.
    /// </summary>
    protected void chk_Neck_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "Neck" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_Neck.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "Neck");
                    BodyParts d = new BodyParts();
                    d.Parts = "Neck";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_Neck = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "Neck");
                    BodyParts d = new BodyParts();
                    d.Parts = "Neck";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_Neck = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_Neck.Checked)
            {
                s1.RemoveAll(x => x.Parts == "Neck");
                BodyParts d = new BodyParts();
                d.Parts = "Neck";
                d.Checked = true;
                s1.Add(d);


                Chk_Neck = true;
            }
            else
            {

                s1.RemoveAll(x => x.Parts == "Neck");
                BodyParts d = new BodyParts();
                d.Parts = "Neck";
                d.Checked = false;
                s1.Add(d);

                Chk_Neck = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
    }
    /// <summary>
    /// Event fires if check box chk_Midback checked or unchecked in the form.
    /// </summary>
    protected void chk_Midback_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "MidBack" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_Midback.Checked)
            {

                if (count == 0)
                {
                    BodyParts d = new BodyParts();
                    d.Parts = "MidBack";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_Midback = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "MidBack");
                    BodyParts d = new BodyParts();
                    d.Parts = "MidBack";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_Midback = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_Midback.Checked)
            {
                BodyParts d = new BodyParts();
                d.Parts = "MidBack";
                d.Checked = true;
                s1.Add(d);
                Chk_Midback = true;
            }
            else
            {

                s.RemoveAll(x => x.Parts == "MidBack");
                BodyParts d = new BodyParts();
                d.Parts = "MidBack";
                d.Checked = false;
                s1.Add(d);
                Chk_Midback = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
    }
    /// <summary>
    /// Event fires if check box chk_lowback checked or unchecked in the form.
    /// </summary>
    protected void chk_lowback_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LowBack" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_lowback.Checked)
            {
                if (count == 0)
                {
                    BodyParts d = new BodyParts();
                    d.Parts = "LowBack";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_lowback = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LowBack");
                    BodyParts d = new BodyParts();
                    d.Parts = "LowBack";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_lowback = true;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_lowback.Checked)
            {
                BodyParts d = new BodyParts();
                d.Parts = "LowBack";
                d.Checked = true;
                s1.Add(d);
                Chk_lowback = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LowBack");
                BodyParts d = new BodyParts();
                d.Parts = "LowBack";
                d.Checked = false;
                s1.Add(d);
                Chk_lowback = true;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Keen checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Keen_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where (i.Parts == "RightKnee" && i.Checked == true) select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Keen.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightKnee";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Keen = true;
            }
            else
            {
                if (count > 0)
                {

                    s.RemoveAll(x => x.Parts == "RightKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightKnee";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Keen = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Keen.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightKnee");
                BodyParts d = new BodyParts();
                d.Parts = "RightKnee";
                d.Checked = true;
                s1.Add(d);

                Chk_r_Keen = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightKnee");
                BodyParts d = new BodyParts();
                d.Parts = "RightKnee";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Keen = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_L_Keen checked or unchecked in the form.
    /// </summary>
    protected void chk_L_Keen_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {

            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftKnee" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_L_Keen.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftKnee";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_L_Keen = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftKnee");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftKnee";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_L_Keen = true;
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_L_Keen.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftKnee");
                BodyParts d = new BodyParts();
                d.Parts = "LeftKnee";
                d.Checked = true;
                s1.Add(d);

                Chk_L_Keen = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftKnee");
                BodyParts d = new BodyParts();
                d.Parts = "LeftKnee";
                d.Checked = false;
                s1.Add(d);

                Chk_L_Keen = true;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Elbow checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Elbow_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightElbow" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Elbow.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightElbow";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Elbow = true;
            }
            else
            {
                if (count > 0)
                {

                    s.RemoveAll(x => x.Parts == "RightElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightElbow";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Elbow = false;
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Elbow.Checked)
            {

                s1.RemoveAll(x => x.Parts == "RightElbow");
                BodyParts d = new BodyParts();
                d.Parts = "RightElbow";
                d.Checked = true;
                s1.Add(d);

                Chk_r_Elbow = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightElbow");
                BodyParts d = new BodyParts();
                d.Parts = "RightElbow";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Elbow = false;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }

    }
    /// <summary>
    /// Event fires if check box chk_l_Elbow checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Elbow_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftElbow" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_Elbow.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftElbow";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_l_Elbow = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftElbow");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftElbow";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_Elbow = true;
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_Elbow.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftElbow");
                BodyParts d = new BodyParts();
                d.Parts = "LeftElbow";
                d.Checked = true;
                s1.Add(d);
                Chk_l_Elbow = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftElbow");
                BodyParts d = new BodyParts();
                d.Parts = "LeftElbow";
                d.Checked = false;
                s1.Add(d);

                Chk_l_Elbow = true;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Wrist checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Wrist_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightWrist" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }

            if (chk_r_Wrist.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightWrist";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Wrist = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightWrist";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Wrist = false;
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Wrist.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightWrist");
                BodyParts d = new BodyParts();
                d.Parts = "RightWrist";
                d.Checked = true;
                s1.Add(d);
                Chk_r_Wrist = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightWrist");
                BodyParts d = new BodyParts();
                d.Parts = "RightWrist";
                d.Checked = false;
                s1.Add(d);
                Chk_r_Wrist = false;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
    }
    /// <summary>
    /// Event fires if check box chk_l_Wrist checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Wrist_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftWrist" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_Wrist.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftWrist";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_l_Wrist = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftWrist");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftWrist";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_Wrist = true;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_Wrist.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftWrist");
                BodyParts d = new BodyParts();
                d.Parts = "LeftWrist";
                d.Checked = true;
                s1.Add(d);

                Chk_l_Wrist = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftWrist");
                BodyParts d = new BodyParts();
                d.Parts = "LeftWrist";
                d.Checked = false;
                s1.Add(d);

                Chk_l_Wrist = true;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Hip checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Hip_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightHip" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_Hip.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightHip";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_Hip = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightHip";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_Hip = false;
            }

            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_Hip.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightHip");
                BodyParts d = new BodyParts();
                d.Parts = "RightHip";
                d.Checked = true;
                s1.Add(d);

                Chk_r_Hip = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightHip");
                BodyParts d = new BodyParts();
                d.Parts = "RightHip";
                d.Checked = false;
                s1.Add(d);

                Chk_r_Hip = false;
            }

            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_l_Hip checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Hip_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftHip" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_Hip.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftHip";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_l_Hip = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftHip");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftHip";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_Hip = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_Hip.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftHip");
                BodyParts d = new BodyParts();
                d.Parts = "LeftHip";
                d.Checked = true;
                s1.Add(d);
                Chk_l_Hip = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftHip");
                BodyParts d = new BodyParts();
                d.Parts = "LeftHip";
                d.Checked = false;
                s1.Add(d);
                Chk_l_Hip = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_ankle checked or unchecked in the form.
    /// </summary>
    protected void chk_r_ankle_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "RightAnkle" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_r_ankle.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "RightAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightAnkle";
                    d.Checked = true;
                    s.Add(d);
                }
                Chk_r_ankle = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "RightAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "RightAnkle";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_r_ankle = false;
            }
            Session["FUbodyPartsList"] = s;

            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_r_ankle.Checked)
            {
                s1.RemoveAll(x => x.Parts == "RightAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "RightAnkle";
                d.Checked = true;
                s1.Add(d);
                Chk_r_ankle = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "RightAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "RightAnkle";
                d.Checked = false;
                s1.Add(d);

                Chk_r_ankle = false;
            }
            Session["FUbodyPartsList"] = s1;

            AddFollowUpMaster master = (AddFollowUpMaster)this.Master; master.bindData(hfPatientFUId.Value);

        }
    }
    /// <summary>
    /// Event fires if check box chk_l_ankle checked or unchecked in the form.
    /// </summary>
    protected void chk_l_ankle_CheckedChanged(object sender, EventArgs e)
    {

        SaveData();
        var s = (List<BodyParts>)Session["FUbodyPartsList"];
        if (s != null)
        {
            var cnt = (from i in s.AsEnumerable() where i.Parts == "LeftAnkle" && i.Checked == true select i).ToList();
            if (cnt == null) { count = 0; } else { count = cnt.Count(); }
            if (chk_l_ankle.Checked)
            {
                if (count == 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftAnkle";
                    d.Checked = true;
                    s.Add(d);
                }

                Chk_l_ankle = true;
            }
            else
            {
                if (count > 0)
                {
                    s.RemoveAll(x => x.Parts == "LeftAnkle");
                    BodyParts d = new BodyParts();
                    d.Parts = "LeftAnkle";
                    d.Checked = false;
                    s.Add(d);
                }
                Chk_l_ankle = false;
            }
            Session["FUbodyPartsList"] = s;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientFUId.Value);
        }
        else
        {
            List<BodyParts> s1 = new List<BodyParts>();
            if (chk_l_ankle.Checked)
            {
                s1.RemoveAll(x => x.Parts == "LeftAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "LeftAnkle";
                d.Checked = true;
                s1.Add(d);
                Chk_l_ankle = true;
            }
            else
            {
                s1.RemoveAll(x => x.Parts == "LeftAnkle");
                BodyParts d = new BodyParts();
                d.Parts = "LeftAnkle";
                d.Checked = false;
                s1.Add(d);
                Chk_l_ankle = false;
            }
            Session["FUbodyPartsList"] = s1;
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientFUId.Value);

        }
    }

    public void SaveData()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            int PatientIE_Id = 0;
            string FreeForm = GetFreeForm();
            SqlCommand cmd = new SqlCommand("nusp_SavePatientDetails", con);
            cmd.Parameters.Add("@Patient_Id", hfPatientId.Value);
            cmd.Parameters.Add("@PatientIE_ID", hfPatientIEId.Value);
            cmd.Parameters.Add("@FreeForm", FreeForm);
            if (!string.IsNullOrEmpty(hfPatientFUId.Value))
                cmd.Parameters.Add("@PatientFU_ID", hfPatientFUId.Value);
            cmd.Parameters.Add("@DOE", DateTime.ParseExact(txtDOV.Text, "MM/dd/yyyy", null));
            if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
                cmd.Parameters.Add("@DOA", DateTime.ParseExact(txtDOA.Text, "MM/dd/yyyy", null));
            if (!string.IsNullOrEmpty(txtDOB.Text.Trim()))
                cmd.Parameters.Add("@DOB", DateTime.ParseExact(txtDOB.Text, "MM/dd/yyyy", null));
            cmd.Parameters.Add("@FirstName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text.Trim().ToLower()));
            cmd.Parameters.Add("@LastName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text.Trim().ToLower()));
            cmd.Parameters.Add("@Location_ID", ddlLoaction.SelectedValue);
            //if (!string.IsNullOrEmpty(txtDOA.Text.Trim()))
            //    cmd.Parameters.Add("@DOA", Convert.ToDateTime(txtDOA.Text.Trim()));
            cmd.Parameters.Add("@Sex", rblGender.SelectedValue);
            // cmd.Parameters.Add("@DOB", Convert.ToDateTime(txtDOB.Text.Trim()));
            cmd.Parameters.Add("@SSN", txtSSN.Text.Trim());
            cmd.Parameters.Add("@Phone", txtHomePh.Text.Trim());
            cmd.Parameters.Add("@work_phone", txtWorkPh.Text.Trim());
            cmd.Parameters.Add("@Phone2", txtMobile.Text.Trim());
            cmd.Parameters.Add("@Address", txtAddress.Text.Trim());
            cmd.Parameters.Add("@City", txtCity.Text.Trim());
            cmd.Parameters.Add("@State", ddState.SelectedValue.Trim());
            cmd.Parameters.Add("@Zip", txtZip.Text.Trim());
            cmd.Parameters.Add("@InsCo_ID", ddlInsuranceCo.SelectedValue);
            cmd.Parameters.Add("@ClaimNumber", txtClaim.Text.Trim());
            cmd.Parameters.Add("@policy_no", txtPolicy.Text.Trim());
            cmd.Parameters.Add("@Compensation", ddl_casetype.SelectedValue);
            cmd.Parameters.Add("@Attorney", txtAttorneyName.Text.Trim());
            cmd.Parameters.Add("@Telephone", txtAttorneyPh.Text.Trim());
            cmd.Parameters.Add("@MA_Providers", txtMAProviders.Text.Trim());
            cmd.Parameters.Add("@FollowedUpOn", txtFollowedUpOn.Text.Trim());
            cmd.Parameters.Add("@PMH", PMH.Text.Trim());
            cmd.Parameters.Add("@PSH", PSH.Text.Trim());
            cmd.Parameters.Add("@Medications", Medication.Text.Trim());
            cmd.Parameters.Add("@Allergies", Allergies.Text.Trim());
            cmd.Parameters.Add("@FamilyHistory", FamilyHistory.Text.Trim());
            cmd.Parameters.Add("@Neck", chk_Neck.Checked);
            cmd.Parameters.Add("@MidBack", chk_Midback.Checked);
            cmd.Parameters.Add("@LowBack", chk_lowback.Checked);
            cmd.Parameters.Add("@LShoulder", chk_L_Shoulder.Checked);
            cmd.Parameters.Add("@RShoulder", chk_r_Shoulder.Checked);
            cmd.Parameters.Add("@cbLKNee", chk_L_Keen.Checked);
            cmd.Parameters.Add("@cbRKnee", chk_r_Keen.Checked);
            cmd.Parameters.Add("@cbLElbow", chk_l_Elbow.Checked);
            cmd.Parameters.Add("@cbRElbow", chk_r_Elbow.Checked);
            cmd.Parameters.Add("@cbLWrist", chk_l_Wrist.Checked);
            cmd.Parameters.Add("@cbRWrist", chk_r_Wrist.Checked);
            cmd.Parameters.Add("@cblAnkle", chk_l_ankle.Checked);
            cmd.Parameters.Add("@cbRAnkle", chk_r_ankle.Checked);
            cmd.Parameters.Add("@cbLHip", chk_l_Hip.Checked);
            cmd.Parameters.Add("@cbRHip", chk_r_Hip.Checked);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientIntime", PatientIntime.Text);
            cmd.Parameters.Add("@PatientOuttime", PatientOuttime.Text);
            cmd.Parameters.Add("@AlertNote", txtAlertNote.Text);


            cmd.Parameters.Add("@ReturnToWork", cboReturnToWork.Text);
            cmd.Parameters.Add("@RecevingPhyTherapy", cboRecevingPhyTherapy.Text);
            cmd.Parameters.Add("@FeelPainRelief", cboFeelPainRelief.Text);

            SqlParameter outDirectoryId = new SqlParameter("@PatientFUIDS", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outDirectoryId);

            con.Open();
            int count = cmd.ExecuteNonQuery();
            string alertText = "Follow Up already exsists for this date. Edit the followup";

            int DirectoryId = !String.IsNullOrEmpty(Convert.ToString(outDirectoryId.Value)) ? Convert.ToInt32(outDirectoryId.Value) : 0;
            if (DirectoryId > 0)
            {
                hfPatientFUId.Value = Convert.ToString(DirectoryId);
            }
            //if (count > 0)
            //{
            //    // hfPatientFUId.Value = count.ToString();
            //    alertText = "Follow Up Added Successfully";
            //    hfPatientFUId=
            //    //Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //    //hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //    //Session["patientFUId"] = hfPatientFUId.Value;
            //}
            //else
            //{
            //    Session["patientFUId"] = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //    hfPatientFUId.Value = GetFUID(Convert.ToInt64(hfPatientIEId.Value != "" ? hfPatientIEId.Value : "0")).ToString();
            //}

            con.Close();
            AddFollowUpMaster master = (AddFollowUpMaster)this.Master;
            master.bindData(hfPatientIEId.Value);
            //this.Page.RegisterStartupScript("Alert", "<script>javascript:alert('" + alertText + "')</script>");
        }
    }

    public int GetFUID(long PatientIE_Id)
    {
        IntakeSheet.DAL.DataAccess _dal = new IntakeSheet.DAL.DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PatientIE_Id", PatientIE_Id));
        DataTable _dt = _dal.getDataTable("GetFUID", param);
        var data = _dt.Rows[0]["PatientFU_ID"];
        return (data != DBNull.Value) ? Convert.ToInt32(data) : 0;
    }

    protected void bindRestrictions(int PatientIE_Id, string PatientFU_ID = null)
    {

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetRestrictionDetails", con);
            cmd.Parameters.Add("@PatientIE_ID", PatientIE_Id);
            cmd.Parameters.Add("@PatientFU_IDnew", PatientFU_ID);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            string freeForm = string.Empty, locationId = string.Empty, MAProviders = string.Empty;
            while (sdr.Read())
            {
                freeForm = Convert.ToString(sdr["FreeForm"]);
                locationId = Convert.ToString(sdr["Location_Id"]);
                MAProviders = Convert.ToString(sdr["MA_Providers"]);
                txtFollowedUpOn.Text = Convert.ToString(sdr["FollowedUpOn"]);
                break;
            }
            if (!string.IsNullOrEmpty(locationId))
                ddlLoaction.SelectedValue = locationId == "" ? Convert.ToString(Session["Location"]) : locationId;
            txtMAProviders.Text = MAProviders == "" ? Convert.ToString(Session["Providers"]) : MAProviders;
            if (!string.IsNullOrEmpty(freeForm))
            {
                string[] freeFormDetails = freeForm.Split('~');
                for (int i = 0; i < freeFormDetails.Length; i++)
                {
                    if (freeFormDetails[i].Length > 0)
                    {
                        string title = freeFormDetails[i].Split('^')[0].Trim();
                        if (title == "Restrictions")
                        {
                            string details = freeFormDetails[i].Split('^')[1].Trim();
                            string[] restrictionValues = details.Split(',');
                            for (int j = 0; j < restrictionValues.Length; j++)
                            {
                                for (int cblIndex = 0; cblIndex < cblRestictions.Items.Count; cblIndex++)
                                {
                                    if (cblRestictions.Items[cblIndex].Value.Replace(" ", String.Empty).ToLower() == restrictionValues[j].Replace(" ", String.Empty).ToLower())
                                    {
                                        cblRestictions.Items[cblIndex].Selected = true;
                                    }
                                }
                            }
                        }
                        else if (title == "Others")
                        {
                            txtOtherRestrictions.Text = freeFormDetails[i].Split('^')[1].Trim();
                        }
                        else if (title == "Degree of Disability")
                        {
                            rblDOD.SelectedValue = freeFormDetails[i].Split('^')[1].Trim();
                        }
                        else if (title == "Notes")
                        {
                            txtFreeForm.Text = freeFormDetails[i].Split('^')[1].Trim();
                        }
                        else if (title == "Work Status")
                        {
                            string details = freeFormDetails[i].Split('^')[1].Trim();
                            string[] workStatusValues = details.Split(',');
                            for (int j = 0; j < workStatusValues.Length; j++)
                            {
                                foreach (RepeaterItem item in this.Repeater1.Items)
                                {
                                    CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
                                    string[] v = null;
                                    List<string> myCollection = new List<string>();
                                    if (workStatusValues[j].Contains('-'))
                                    {
                                        v = workStatusValues[j].Split('-');
                                    }
                                    else
                                    {
                                        string ss = workStatusValues[j];
                                        myCollection.Add(ss);
                                        myCollection.Add("");
                                        v = myCollection.ToArray();
                                    }
                                    string og = chk.Text;
                                    og = Regex.Replace(chk.Text, @"\s+", "");
                                    string cm = v[0];
                                    cm = Regex.Replace(v[0], @"\s+", "");
                                    if (og.ToLower() == cm.ToLower())
                                    {
                                        // string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
                                        //string degree = chk.Text;
                                        // this.SaveData(degree, collageName);
                                        chk.Checked = true;
                                        (item.FindControl("txtCollageName") as TextBox).Text = v[1];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            con.Close();
        }
    }
    //protected void bindRestrictions(int PatientIE_Id)
    //{
    //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
    //    {
    //        SqlCommand cmd = new SqlCommand("nusp_GetRestrictionDetails", con);
    //        cmd.Parameters.Add("@PatientIE_ID", PatientIE_Id);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        con.Open();
    //        SqlDataReader sdr = cmd.ExecuteReader();
    //        string freeForm = string.Empty, locationId = string.Empty, MAProviders = string.Empty;
    //        while (sdr.Read())
    //        {
    //            freeForm = Convert.ToString(sdr["FreeForm"]);
    //            locationId = Convert.ToString(sdr["Location_Id"]);
    //            MAProviders = Convert.ToString(sdr["MA_Providers"]);
    //            txtFollowedUpOn.Text = Convert.ToString(sdr["FollowedUpOn"]);
    //            break;
    //        }
    //        if (!string.IsNullOrEmpty(locationId))
    //            ddlLoaction.SelectedValue = locationId == "" ? Convert.ToString(Session["Location"]) : locationId;
    //        txtMAProviders.Text = MAProviders == "" ? Convert.ToString(Session["Providers"]) : MAProviders;
    //        if (!string.IsNullOrEmpty(freeForm))
    //        {
    //            string[] freeFormDetails = freeForm.Split(';');
    //            for (int i = 0; i < freeFormDetails.Length; i++)
    //            {
    //                if (freeFormDetails[i].Length > 0)
    //                {
    //                    string title = freeFormDetails[i].Split(':')[0].Trim();
    //                    if (title == "Restrictions")
    //                    {
    //                        string details = freeFormDetails[i].Split(':')[1].Trim();
    //                        string[] restrictionValues = details.Split(',');
    //                        for (int j = 0; j < restrictionValues.Length; j++)
    //                        {
    //                            for (int cblIndex = 0; cblIndex < cblRestictions.Items.Count; cblIndex++)
    //                            {
    //                                if (cblRestictions.Items[cblIndex].Value.Replace(" ", String.Empty).ToLower() == restrictionValues[j].Replace(" ", String.Empty).ToLower())
    //                                {
    //                                    cblRestictions.Items[cblIndex].Selected = true;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else if (title == "Others")
    //                    {
    //                        txtOtherRestrictions.Text = freeFormDetails[i].Split(':')[1].Trim();
    //                    }
    //                    else if (title == "Degree of Disability")
    //                    {
    //                        rblDOD.SelectedValue = freeFormDetails[i].Split(':')[1].Trim();
    //                    }
    //                    else if (title == "Notes")
    //                    {
    //                        txtFreeForm.Text = freeFormDetails[i].Split(':')[1].Trim();
    //                    }
    //                    else if (title == "Work Status")
    //                    {
    //                        string details = freeFormDetails[i].Split(':')[1].Trim();
    //                        string[] workStatusValues = details.Split(',');
    //                        for (int j = 0; j < workStatusValues.Length; j++)
    //                        {
    //                            foreach (RepeaterItem item in this.Repeater1.Items)
    //                            {
    //                                CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
    //                                string[] v = null;
    //                                List<string> myCollection = new List<string>();
    //                                if (workStatusValues[j].Contains('-'))
    //                                {
    //                                    v = workStatusValues[j].Split('-');
    //                                }
    //                                else
    //                                {
    //                                    string ss = workStatusValues[j];
    //                                    myCollection.Add(ss);
    //                                    myCollection.Add("");
    //                                    v = myCollection.ToArray();
    //                                }
    //                                string og = chk.Text;
    //                                og = Regex.Replace(chk.Text, @"\s+", "");
    //                                string cm = v[0];
    //                                cm = Regex.Replace(v[0], @"\s+", "");
    //                                if (og.ToLower() == cm.ToLower())
    //                                {
    //                                    chk.Checked = true;
    //                                    (item.FindControl("txtCollageName") as TextBox).Text = v[1];
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        con.Close();
    //    }
    //}
    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/RTNWORKS/RTNWORK"))
        {
            cboReturnToWork.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/RECTPYS/RECTPY"))
        {
            cboRecevingPhyTherapy.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/FEELPAINRLFS/FEELPAINRLF"))
        {
            cboFeelPainRelief.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }

    }
    public void bindHtml()
    {

        string path = Server.MapPath("~/Template/Page2_degree.html");
        string body = File.ReadAllText(path);

        divdegreeHTML.InnerHtml = body;

        path = Server.MapPath("~/Template/Page1_social.html");
        body = File.ReadAllText(path);

        divsocialHTML.InnerHtml = body;
    }

    public void bindHTMLEdit()
    {
        if (!string.IsNullOrEmpty(Session["patientFUId"].ToString()))
        {
            string query = "select top 1 * from tblPage1FUHTMLContent where PateintFU_ID=" + Session["patientFUId"].ToString();
            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
            {
                bindHtml();
            }
            else
            {
                divdegreeHTML.InnerHtml = ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString();
                divsocialHTML.InnerHtml = ds.Tables[0].Rows[0]["socialSectionHTML"].ToString();
            }
        }
        else
            bindHtml();
    }
}
