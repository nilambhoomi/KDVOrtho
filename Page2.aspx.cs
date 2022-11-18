using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Configuration;
using IntakeSheet.BLL;

public partial class Page2 : System.Web.UI.Page
{
    DBHelperClass gDbhelperobj = new DBHelperClass();
    #region DataMembers
    string patientId;
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
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["PatientIE_ID"] == null)
            {
                Session["bodyPartsList"] = null;
                Response.Redirect("Page1.aspx");

            }
            else if (Session["PatientIE_ID"] != null)
            {
                bindDropdown();
                bindHtml();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblPatientIEDetailPage1 WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                DataRow rw = ds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                if (rw != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "accidentdesc();", true);
                    string query1 = (" select Compensation FROM tblPatientIE WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                    SqlCommand cm1 = new SqlCommand(query1, cn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cm1);
                    cn.Open();
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1);
                    cn.Close();
                    string comp = ds1.Tables[0].AsEnumerable().Select(m => m.Field<string>("Compensation")).FirstOrDefault();
                    // Session["Casetype"]=comp;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Server.MapPath("~/xml/HSMData.xml"));

                    foreach (XmlNode node in doc.SelectNodes("//HSM/Sustaineds/Sustained"))
                    {
                        string casetype = node.Attributes["CaseType"].InnerText;
                        if (casetype.Contains(comp))
                        {
                            ddl_accident_desc.Text = node.Attributes["name"].InnerText;
                        }

                    }
                    // row exists
                    DefaultValue();

                }
                else
                {

                    this.patientId = Session["PatientIE_ID"].ToString();//done this change
                    bindData(this.patientId);//done this change
                    //PageMainMaster master = (PageMainMaster)this.Master;
                    //master.bindData(Session["PatientIE_ID"].ToString());
                }
            }

            //else
            //Response.Redirect("Page1.aspx");
        }

        Logger.Info(Session["uname"].ToString() + "- Visited in  Page1 for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/Sustaineds/Sustained"))
        {
            string casetype = node.Attributes["CaseType"].InnerText;
            //if (casetype.Contains(Session["Casetype"].ToString()))
            //{
            //    string ss = node.Attributes["name"].InnerText;
            //}
            ddl_accident_desc.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/WasAts/WasAt"))
        {
            ddl_belt.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/InvolvedIns/InvolvedIn"))
        {
            ddl_invovledin.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/ESMTeams/ESMTeam"))
        {
            ddl_EMS.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/Vias/Via"))
        {
            ddl_via.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/WorksAts/WorksAt"))
        {
            ddl_work_status.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/Times/Time"))
        {
            ddl_howlong.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
        foreach (XmlNode node in doc.SelectNodes("//HSM/WorkStatuss/WorkStatus"))
        {
            cboReturnedToWork.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        string SP = "";
        SqlParameter[] param = null;

        //starting the insertion in database

        string LOC = "", workat = "", DrSeen = "";

        if (chk_loc.Checked)
            LOC = txt_howlong.Value + "|" + ddl_howlong.SelectedItem.Text;
        else
            LOC = "0|undetermined time";

        if (ddl_work_status.Text == "")
            workat = "Patient works as unknown";
        else
            workat = ddl_work_status.Text;

        bool sameday = false;
        int day = 0;

        if (rep_wenttohospital.SelectedValue == "0")
        {
            sameday = true;
        }
        else
        {
            if (txt_day.Value == "0" || txt_day.Value == "")
                sameday = true;
            else
                day = Convert.ToInt16(txt_day.Value);
        }

        if (rbl_seen_injury.SelectedValue == "1")
        {
            //if (txt_docname.Value != "")
            //    DrSeen = "The patient has visited Dr " + txt_docname.Value;
            //else
            //    DrSeen = "The patient has visited Dr X since the accident.";
            DrSeen = txt_docname.Value;
        }

        DBHelperClass db = new DBHelperClass();
        //string SP = "";
        //SqlParameter[] param = null;
        param = new SqlParameter[68];
        SP = "usp_Save_IntakePage2";

        param[0] = new SqlParameter("@Sustained", ddl_accident_desc.Text);
        param[1] = new SqlParameter("@Position", ddl_belt.Text);
        param[2] = new SqlParameter("@InvolvedIn", ddl_invovledin.Text);


        param[3] = new SqlParameter("@CriteriaA", rep_hospitalized.SelectedItem.Value);
        param[4] = new SqlParameter("@EMSTeam", ddl_EMS.Text);
        param[5] = new SqlParameter("@CC2", rbl_in_past.SelectedItem.Value);
        param[6] = new SqlParameter("@WentTo", txt_hospital.Text);
        param[7] = new SqlParameter("@Via", ddl_via.Text);
        param[8] = new SqlParameter("@HadXrayOf", txt_x_ray.Value);
        param[9] = new SqlParameter("@HadCTScanOf", txt_CT.Value);
        param[10] = new SqlParameter("@PrescriptionFor", txt_prescription.Value);
        param[11] = new SqlParameter("@FreeForm", ""); //Not saving it
        param[12] = new SqlParameter("@InjuryToHead", chk_headinjury.Checked);
        param[13] = new SqlParameter("@HadMRIOf", txt_mri.Value);
        param[14] = new SqlParameter("@LossOfConsciousnessFor", LOC);
        param[15] = new SqlParameter("@DaysLater", day);
        param[16] = new SqlParameter("@SameDay", sameday);
        param[17] = new SqlParameter("@HadTreatmentFor", txt_injur_past_bp.Value);
        param[18] = new SqlParameter("@DetailInfo", txt_injur_past_how.Value);
        param[19] = new SqlParameter("@Neck", chk_Neck.Checked);
        param[20] = new SqlParameter("@MidBack", chk_Midback.Checked);
        param[21] = new SqlParameter("@LowBack", chk_lowback.Checked);
        param[22] = new SqlParameter("@LeftShoulder", chk_L_Shoulder.Checked);
        param[23] = new SqlParameter("@RightShoulder", chk_r_Shoulder.Checked);
        param[24] = new SqlParameter("@LeftKnee", chk_L_Keen.Checked);
        param[25] = new SqlParameter("@RightKnee", chk_r_Keen.Checked);
        param[26] = new SqlParameter("@LeftElbow", chk_l_Elbow.Checked);
        param[27] = new SqlParameter("@RightElbow", chk_r_Elbow.Checked);
        param[28] = new SqlParameter("@LeftWrist", chk_l_Wrist.Checked);
        param[29] = new SqlParameter("@RightWrist", chk_r_Wrist.Checked);
        param[30] = new SqlParameter("@LeftAnkle", chk_l_ankle.Checked);
        param[31] = new SqlParameter("@RightAnkle", chk_r_ankle.Checked);
        param[32] = new SqlParameter("@Others", txt_other.Text);
        param[33] = new SqlParameter("@LeftHip", chk_l_Hip.Checked);
        param[34] = new SqlParameter("@RightHip", chk_r_Hip.Checked);
        param[35] = new SqlParameter("@WorksAt", workat);
        param[36] = new SqlParameter("@AccidentDetail", txt_details.Text);
        param[37] = new SqlParameter("@DoctorSeen", DrSeen);
        param[38] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"]);
        param[39] = new SqlParameter("@PMH", PMH.Text);
        param[40] = new SqlParameter("@PSH", PSH.Text);
        param[41] = new SqlParameter("@Medications", Medication.Text);
        param[42] = new SqlParameter("@Allergies", Allergies.Text);
        //added new on 16/7/2017
        param[43] = new SqlParameter("@ComplainingHeadeaches", chkComplainingofHeadaches.Checked);
        param[44] = new SqlParameter("@Persistent", txtPersistent.Text);
        param[45] = new SqlParameter("@Frontal", chkfrontal.Checked);
        param[46] = new SqlParameter("@LeftParietal", chkLeftParietal.Checked);
        param[47] = new SqlParameter("@RightParietal", chkRightParietal.Checked);
        param[48] = new SqlParameter("@LeftTemporal", chkLeftTemporal.Checked);
        param[49] = new SqlParameter("@RightTemporal", chkRightTemporal.Checked);
        param[50] = new SqlParameter("@Occipital", chkOccipital.Checked);
        param[51] = new SqlParameter("@Global", chkGlobal.Checked);
        param[52] = new SqlParameter("@SevereAnxiety", chkSevereAnxiety.Checked);
        param[53] = new SqlParameter("@Nausea", chkNausea.Checked);
        param[54] = new SqlParameter("@Dizziness", chkDizziness.Checked);
        param[55] = new SqlParameter("@Vomiting", chkVomitting.Checked);
        param[56] = new SqlParameter("@HeadechesAssociated", chkHeadechesAssociated.Checked);
        param[57] = new SqlParameter("@FamilyHistory", FamilyHistory.Text);

        param[58] = new SqlParameter("@DeniesSmoking", chkDeniessmoking.Checked);
        param[59] = new SqlParameter("@DeniesDrinking", chkDeniesdrinking.Checked);
        param[60] = new SqlParameter("@DeniesDrugs", chkDeniesdrugs.Checked);
        param[61] = new SqlParameter("@DeniesSocialDrinking", chkSocialdrinking.Checked);
        //Added vitals on 01102017
        param[62] = new SqlParameter("@Vitals", txtVitals.Text);
        if (Session["OccurOn"] != null)
        { param[63] = new SqlParameter("@OccurOn", Convert.ToBoolean(Session["OccurOn"])); }
        else
        { param[63] = new SqlParameter("@OccurOn", false); }

        param[64] = new SqlParameter("@Missed", txtMissed.Text);
        param[65] = new SqlParameter("@ReturnToWork", cboReturnedToWork.Text);
        param[66] = new SqlParameter("@InvolvedInOther", txtInvolvedOther.Text);
        param[67] = new SqlParameter("@ADLContent", txt_ADL_Content.Text);


        //Insert values in the db.
        int val = db.executeSP(SP, param);






        string query = "select top 1 * from tblPage1HTMLContent where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        DataSet ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count == 0)
        {
            query = "insert into tblPage1HTMLContent values(@topSectionHTML,@socialSectionHTML,@accidentHTML,@PatientIE_ID,0,@historyHTML,@historyHTMLValue,@OtherValue)";
        }
        else
        {
            query = "update tblPage1HTMLContent set topSectionHTML=@topSectionHTML,accidentHTML=@accidentHTML,socialSectionHTML=@socialSectionHTML,historyHTML=@historyHTML,historyHTMLValue=@historyHTMLValue,otherValue=@OtherValue where PatientIE_ID=@PatientIE_ID";
        }

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
            command.Parameters.AddWithValue("@topSectionHTML", hdtopHTMLContent.Value);
            command.Parameters.AddWithValue("@socialSectionHTML", hdsocialHTMLContent.Value);
            command.Parameters.AddWithValue("@accidentHTML", hdaccidentHTMLContent.Value);
            command.Parameters.AddWithValue("@historyHTML", hdhistoryHTMLContent.Value);
            command.Parameters.AddWithValue("@historyHTMLValue", hdhistoryHTMLValue.Value);
            command.Parameters.AddWithValue("@OtherValue", txt_other_val.Text);

            connection.Open();
            var results = command.ExecuteNonQuery();
            connection.Close();
        }

        Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Create IE - Page2 " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        else
        {
            Response.Redirect("Page3.aspx");
        }
    }
    private void DefaultValue()
    {

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
        XmlNodeList nodeList;

        #region Page1
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage1");
        foreach (XmlNode node in nodeList)
        {
            //ddl_accident_desc.Text = node.SelectSingleNode("Sustained").InnerText;
            ddl_via.Text = node.SelectSingleNode("Via").InnerText;
            ddl_belt.Text = node.SelectSingleNode("Position").InnerText;
            ddl_invovledin.Text = node.SelectSingleNode("InvolvedIn").InnerText;
            //rep_hospitalized.SelectedItem.Value = node.SelectSingleNode("InvolvedInOther").InnerText;
            txtInvolvedOther.Text = node.SelectSingleNode("InvolvedInOther").InnerText;
            ddl_EMS.Text = node.SelectSingleNode("EMSTeam").InnerText;
            rbl_in_past.SelectedItem.Value = node.SelectSingleNode("CC2").InnerText;
            txt_hospital.Text = node.SelectSingleNode("WentTo").InnerText;

            txt_x_ray.Value = node.SelectSingleNode("HadXrayOf").InnerText;
            txt_CT.Value = node.SelectSingleNode("HadCTScanOf").InnerText;
            txt_prescription.Value = node.SelectSingleNode("PrescriptionFor").InnerText;
            chk_headinjury.Checked = Convert.ToBoolean(node.SelectSingleNode("InjuryToHead").InnerText);
            txt_mri.Value = node.SelectSingleNode("HadMRIOf").InnerText;
            txt_injur_past_bp.Value = node.SelectSingleNode("HadTreatmentFor").InnerText;
            txt_injur_past_how.Value = node.SelectSingleNode("DetailInfo").InnerText;
            // added on 17/6/2017

            txtPersistent.Text = node.SelectSingleNode("Persistent").InnerText;
            chkComplainingofHeadaches.Checked = Convert.ToBoolean(node.SelectSingleNode("ComplainingHeadeaches").InnerText);
            chkfrontal.Checked = Convert.ToBoolean(node.SelectSingleNode("Frontal").InnerText);
            chkLeftParietal.Checked = Convert.ToBoolean(node.SelectSingleNode("LeftParietal").InnerText);
            chkRightParietal.Checked = Convert.ToBoolean(node.SelectSingleNode("RightParietal").InnerText);
            chkLeftTemporal.Checked = !string.IsNullOrEmpty(node.SelectSingleNode("LeftTemporal").InnerText) ? Convert.ToBoolean(node.SelectSingleNode("LeftTemporal").InnerText) : false;
            chkRightTemporal.Checked = !string.IsNullOrEmpty(node.SelectSingleNode("RightTemporal").InnerText) ? Convert.ToBoolean(node.SelectSingleNode("RightTemporal").InnerText) : false;
            chkOccipital.Checked = Convert.ToBoolean(node.SelectSingleNode("Occipital").InnerText);
            chkGlobal.Checked = Convert.ToBoolean(node.SelectSingleNode("Global").InnerText);
            chkSevereAnxiety.Checked = Convert.ToBoolean(node.SelectSingleNode("SevereAnxiety").InnerText);
            chkNausea.Checked = Convert.ToBoolean(node.SelectSingleNode("Nausea").InnerText);
            chkDizziness.Checked = Convert.ToBoolean(node.SelectSingleNode("Dizziness").InnerText);
            chkVomitting.Checked = Convert.ToBoolean(node.SelectSingleNode("Vomiting").InnerText);




            #endregion

        }
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage2");
        foreach (XmlNode node in nodeList)
        {
            PMH.Text = node.SelectSingleNode("PMH").InnerText;
            PSH.Text = node.SelectSingleNode("PSH").InnerText;
            Medication.Text = node.SelectSingleNode("Medications").InnerText;
            Allergies.Text = node.SelectSingleNode("Allergies").InnerText;
            FamilyHistory.Text = node.SelectSingleNode("FamilyHistory").InnerText;

            chkDeniessmoking.Checked = !string.IsNullOrEmpty(node.SelectSingleNode("DeniesSmoking").InnerText) ? Convert.ToBoolean(node.SelectSingleNode("DeniesSmoking").InnerText) : false;
            chkDeniesdrinking.Checked = !string.IsNullOrEmpty(node.SelectSingleNode("DeniesDrinking").InnerText) ? Convert.ToBoolean(node.SelectSingleNode("DeniesDrinking").InnerText) : false;
            chkDeniesdrugs.Checked = !string.IsNullOrEmpty(node.SelectSingleNode("DeniesDrugs").InnerText) ? Convert.ToBoolean(node.SelectSingleNode("DeniesDrugs").InnerText) : false;
            chkSocialdrinking.Checked = !string.IsNullOrEmpty(node.SelectSingleNode("DeniesSocialDrinking").InnerText) ? Convert.ToBoolean(node.SelectSingleNode("DeniesSocialDrinking").InnerText) : false;
            txtVitals.Text = !string.IsNullOrEmpty(node.SelectSingleNode("Vitals").InnerText) ? Convert.ToString(node.SelectSingleNode("Vitals").InnerText) : string.Empty;
        }


    }
    private void bindData(string patientIEid)
    {
        string strDoctorSeen = "";
        string query = "select * from tblPatientIEDetailPage1 where PatientIE_ID=" + patientIEid;

        DataSet ds = gDbhelperobj.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {

            ViewState["patientIEid"] = patientIEid;

            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["HTMLContent"].ToString()))
            //    divHTML.InnerHtml = ds.Tables[0].Rows[0]["HTMLContent"].ToString();
            //else
            //    bindHtml();

            txt_CT.Value = ds.Tables[0].Rows[0]["HadCTScanOf"].ToString();

            if (!string.IsNullOrEmpty(txt_CT.Value))
                chk_CT.Checked = true;

            txt_hospital.Text = ds.Tables[0].Rows[0]["WentTo"].ToString();
            txt_mri.Value = ds.Tables[0].Rows[0]["HadMRIOf"].ToString();

            if (!string.IsNullOrEmpty(txt_mri.Value))
                chk_mri.Checked = true;

            txt_prescription.Value = ds.Tables[0].Rows[0]["PrescriptionFor"].ToString();
            txt_details.Text = ds.Tables[0].Rows[0]["AccidentDetail"].ToString();
            txt_which_what.Value = ds.Tables[0].Rows[0]["FreeForm"].ToString();

            txt_x_ray.Value = ds.Tables[0].Rows[0]["HadXrayOf"].ToString();

            if (!string.IsNullOrEmpty(txt_x_ray.Value))
                chk_xray.Checked = true;

            if (ds.Tables[0].Rows[0]["SameDay"].ToString() == "False")
            {
                txt_day.Disabled = false;
                txt_day.Value = ds.Tables[0].Rows[0]["DaysLater"].ToString();
                rep_wenttohospital.Items[1].Selected = true;
            }
            else
            {
                txt_day.Value = "0";
                txt_day.Disabled = true;
                rep_wenttohospital.Items[0].Selected = true;
            }

            ddl_accident_desc.Text = ds.Tables[0].Rows[0]["Sustained"].ToString();
            ddl_belt.Text = ds.Tables[0].Rows[0]["Position"].ToString();

            rep_hospitalized.ClearSelection();
            if (ds.Tables[0].Rows[0]["CriteriaA"].ToString() != "True")
                rep_hospitalized.Items.FindByValue("0").Selected = true;
            else
                rep_hospitalized.Items.FindByValue("1").Selected = true;

            ddl_EMS.Text = ds.Tables[0].Rows[0]["EMSTeam"].ToString();
            ddl_invovledin.Text = ds.Tables[0].Rows[0]["InvolvedIn"].ToString();
            txtInvolvedOther.Text = ds.Tables[0].Rows[0]["InvolvedInOther"].ToString();
            ddl_via.Text = ds.Tables[0].Rows[0]["Via"].ToString();
            chk_headinjury.Checked = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["InjuryToHead"].ToString()) ? Convert.ToBoolean(ds.Tables[0].Rows[0]["InjuryToHead"].ToString()) : false;

            strDoctorSeen = ds.Tables[0].Rows[0]["DoctorSeen"].ToString();
            if (!string.IsNullOrEmpty(strDoctorSeen))
            {
                rbl_seen_injury.ClearSelection();
                rbl_seen_injury.Items[1].Selected = true;
                //int iDr = strFreeForm.IndexOf("The patient has visited Dr") + 27;
                //string sDr = strFreeForm.Substring(iDr);
                txt_docname.Value = strDoctorSeen;
            }
            else
            {
                rbl_seen_injury.ClearSelection();
                rbl_seen_injury.Items[0].Selected = true;
                txt_docname.Value = "";
                txt_docname.Disabled = true;
            }

            if (ds.Tables[0].Rows[0]["LossOfConsciousnessFor"] == DBNull.Value || ds.Tables[0].Rows[0]["LossOfConsciousnessFor"].ToString().Contains("undetermined time") || ds.Tables[0].Rows[0]["LossOfConsciousnessFor"].ToString() == "|")
            {
                chk_loc.Checked = false;
                ddl_howlong.ClearSelection();
                txt_howlong.Value = "";
            }
            else
            {
                if (ds.Tables[0].Rows[0]["LossOfConsciousnessFor"].ToString() != "|")
                {
                    chk_loc.Checked = true;
                    ddl_howlong.ClearSelection();
                    ddl_howlong.Items.FindByText(ds.Tables[0].Rows[0]["LossOfConsciousnessFor"].ToString().Split('|')[1]).Selected = true;
                    txt_howlong.Value = ds.Tables[0].Rows[0]["LossOfConsciousnessFor"].ToString().Split('|')[0];
                }
            }

            if (ds.Tables[0].Rows[0]["CC2"] == DBNull.Value)
                rbl_in_past.Items.FindByValue("0").Selected = true;
            else
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["CC2"].ToString()) == true)
                rbl_in_past.Items.FindByValue("1").Selected = true;
            else
                rbl_in_past.Items.FindByValue("0").Selected = true;

            txt_injur_past_bp.Value = ds.Tables[0].Rows[0]["HadTreatmentFor"].ToString();
            txt_injur_past_how.Value = ds.Tables[0].Rows[0]["DetailInfo"].ToString();
            txt_ADL_Content.Text = ds.Tables[0].Rows[0]["ADLContent"].ToString();

            if (ds.Tables[0].Rows[0]["CC2"] == DBNull.Value || Convert.ToBoolean(ds.Tables[0].Rows[0]["CC2"].ToString()) == false)
            {
                txt_injur_past_bp.Disabled = true;
                txt_injur_past_how.Disabled = true;
            }
            else
            {
                txt_injur_past_bp.Disabled = false;
                txt_injur_past_how.Disabled = false;
            }

            if (ds.Tables[0].Rows[0]["CriteriaA"] == DBNull.Value || Convert.ToBoolean(ds.Tables[0].Rows[0]["CriteriaA"].ToString()) == false)
            {
                rep_hospitalized.Items.FindByValue("0").Selected = true;
                txt_CT.Disabled = true;
                chk_CT.Enabled = false;
                txt_hospital.Enabled = false;
                txt_mri.Disabled = true;
                chk_mri.Enabled = false;
                txt_prescription.Disabled = true;
                txt_which_what.Disabled = true;
                txt_x_ray.Disabled = true;
                chk_xray.Enabled = false;
                txt_day.Disabled = true;
            }
            else
            {
                rep_hospitalized.Items.FindByValue("0").Selected = false;
                txt_CT.Disabled = false;
                chk_CT.Enabled = true;
                txt_hospital.Enabled = true;
                txt_mri.Disabled = false;
                chk_mri.Enabled = true;
                txt_prescription.Disabled = false;
                txt_which_what.Disabled = false;
                txt_x_ray.Disabled = false;
                chk_xray.Enabled = true;
                txt_day.Disabled = false;
            }

            //
            txtPersistent.Text = ds.Tables[0].Rows[0]["Persistent"].ToString();
            if (ds.Tables[0].Rows[0]["ComplainingHeadeaches"] != DBNull.Value)
            { chkComplainingofHeadaches.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ComplainingHeadeaches"]); }
            if (ds.Tables[0].Rows[0]["Frontal"] != DBNull.Value)
            { chkfrontal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Frontal"]); }
            if (ds.Tables[0].Rows[0]["HeadechesAssociated"] != DBNull.Value)
            { chkHeadechesAssociated.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["HeadechesAssociated"]); }
            if (ds.Tables[0].Rows[0]["LeftParietal"] != DBNull.Value)
            { chkLeftParietal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftParietal"]); }
            if (ds.Tables[0].Rows[0]["RightParietal"] != DBNull.Value)
            { chkRightParietal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightParietal"]); }
            if (ds.Tables[0].Rows[0]["LeftTemporal"] != DBNull.Value)
            { chkLeftTemporal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftTemporal"]); }
            if (ds.Tables[0].Rows[0]["RightTemporal"] != DBNull.Value)
            { chkRightTemporal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightTemporal"]); }
            if (ds.Tables[0].Rows[0]["Occipital"] != DBNull.Value)
            { chkOccipital.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Occipital"]); }
            if (ds.Tables[0].Rows[0]["Global"] != DBNull.Value)
            { chkGlobal.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Global"]); }
            if (ds.Tables[0].Rows[0]["SevereAnxiety"] != DBNull.Value)
            { chkSevereAnxiety.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SevereAnxiety"]); }
            if (ds.Tables[0].Rows[0]["Nausea"] != DBNull.Value)
            { chkNausea.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Nausea"]); }
            if (ds.Tables[0].Rows[0]["Dizziness"] != DBNull.Value)
            { chkDizziness.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Dizziness"]); }
            if (ds.Tables[0].Rows[0]["Vomiting"] != DBNull.Value)
            { chkVomitting.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Vomiting"]); }

        }
        else
        {
            //load def values
            rep_hospitalized.Items[0].Selected = true;
            txt_hospital.Enabled = false;
            ddl_via.Enabled = false;
            rbl_in_past.Items[0].Selected = true;
            txt_injur_past_bp.Disabled = true;
            txt_injur_past_how.Disabled = true;
            rbl_seen_injury.Items[0].Selected = true;
            txt_docname.Disabled = true;
            txt_CT.Disabled = true;
            chk_CT.Enabled = false;
            txt_hospital.Enabled = false;
            txt_mri.Disabled = true;
            chk_mri.Enabled = false;
            txt_prescription.Disabled = true;
            txt_which_what.Disabled = true;
            txt_x_ray.Disabled = true;
            chk_xray.Enabled = false;
            txt_day.Disabled = true;
        }


        query = "select * from tblPage1HTMLContent where  PatientIE_ID=" + patientIEid;
        ds = gDbhelperobj.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {
            divaccidentHTML.InnerHtml = ds.Tables[0].Rows[0]["accidentHTML"].ToString();
            divsocialHTML.InnerHtml = ds.Tables[0].Rows[0]["socialSectionHTML"].ToString();
            divtopHTML.InnerHtml = ds.Tables[0].Rows[0]["topSectionHTML"].ToString();
            divhistoryHTML.InnerHtml = ds.Tables[0].Rows[0]["historyHTML"].ToString();
            txt_other_val.Text = ds.Tables[0].Rows[0]["OtherValue"].ToString();
        }
        else
            bindHtml();

        query = "select worksat,PMH,PSH,Medications,Allergies,FamilyHistory,DeniesSmoking,DeniesDrinking,DeniesDrugs,DeniesSocialDrinking,Vitals,Missed,ReturnToWork from tblPatientIEDetailPage2 where PatientIE_ID=" + patientIEid;

        ds = gDbhelperobj.selectData(query);


        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["WorksAt"] != DBNull.Value && ds.Tables[0].Rows[0]["WorksAt"].ToString() != "")
            {
                ddl_work_status.Text = ds.Tables[0].Rows[0]["WorksAt"].ToString();
                PMH.Text = ds.Tables[0].Rows[0]["PMH"].ToString();
                PSH.Text = ds.Tables[0].Rows[0]["PSH"].ToString();
                Medication.Text = ds.Tables[0].Rows[0]["Medications"].ToString();
                Allergies.Text = ds.Tables[0].Rows[0]["Allergies"].ToString();
                FamilyHistory.Text = ds.Tables[0].Rows[0]["FamilyHistory"].ToString();
                // SocialHistory.Text = ds.Tables[0].Rows[0]["SocialHistory"].ToString();
                txtVitals.Text = ds.Tables[0].Rows[0]["Vitals"].ToString();

                txtMissed.Text = ds.Tables[0].Rows[0]["Missed"].ToString();
                cboReturnedToWork.Text = ds.Tables[0].Rows[0]["ReturnToWork"].ToString();

                if (ds.Tables[0].Rows[0]["DeniesSmoking"] != DBNull.Value)
                {
                    chkDeniessmoking.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DeniesSmoking"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DeniesDrinking"] != DBNull.Value)
                {
                    chkDeniesdrinking.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DeniesDrinking"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DeniesDrugs"] != DBNull.Value)
                {
                    chkDeniesdrugs.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DeniesDrugs"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DeniesSocialDrinking"] != DBNull.Value)
                {
                    chkSocialdrinking.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DeniesSocialDrinking"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Vitals"] != DBNull.Value)
                {
                    txtVitals.Text = Convert.ToString(ds.Tables[0].Rows[0]["Vitals"].ToString());
                }
                //if (ds.Tables[0].Rows[0]["WorksAt"].ToString().Contains("as"))
                //{
                //    ddl_work_status.Text = ds.Tables[0].Rows[0]["WorksAt"].ToString().Split(' ')[3];
                //}
            }
        }

        query = "select * from tblInjuredBodyParts where PatientIE_ID=" + patientIEid;

        ds = gDbhelperobj.selectData(query);
        List<BodyParts> bodyPartsList = new List<BodyParts>();
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["LeftAnkle"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftAnkle";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftAnkle"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_l_ankle.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftAnkle"].ToString());
                Chk_l_ankle = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftAnkle"].ToString());
            }
            if (ds.Tables[0].Rows[0]["LeftElbow"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftElbow";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftElbow"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_l_Elbow.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftElbow"].ToString());
                Chk_l_Elbow = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftElbow"].ToString());
            }
            if (ds.Tables[0].Rows[0]["RightElbow"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightElbow";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_Elbow.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString());
                Chk_r_Elbow = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString());
            }
            if (ds.Tables[0].Rows[0]["RightAnkle"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightAnkle";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightAnkle"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_ankle.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightAnkle"].ToString());
                Chk_r_ankle = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightAnkle"].ToString());
            }


            if (ds.Tables[0].Rows[0]["LeftShoulder"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftShoulder";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftShoulder"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_L_Shoulder.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftShoulder"].ToString());
                Session["LeftShoulder"] = ds.Tables[0].Rows[0]["LeftShoulder"].ToString();
                Chk_L_Shoulder = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftShoulder"].ToString());
            }

            if (ds.Tables[0].Rows[0]["RightShoulder"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightShoulder";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightShoulder"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_Shoulder.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightShoulder"].ToString());
                Session["RightShoulder"] = ds.Tables[0].Rows[0]["RightShoulder"].ToString();
                Chk_r_Shoulder = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightShoulder"].ToString());
            }
            if (ds.Tables[0].Rows[0]["LeftKnee"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftKnee";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftKnee"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_L_Keen.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftKnee"].ToString());
                Chk_L_Keen = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftKnee"].ToString());
            }
            if (ds.Tables[0].Rows[0]["RightKnee"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightKnee";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightKnee"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_Keen.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightKnee"].ToString());
                Chk_r_Keen = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightKnee"].ToString());
            }
            if (ds.Tables[0].Rows[0]["LeftElbow"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftElbow";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftElbow"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_l_Elbow.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftElbow"].ToString());
                Chk_l_Elbow = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftAnkle"].ToString()); ;
            }
            if (ds.Tables[0].Rows[0]["RightElbow"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightElbow";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_Elbow.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString());
                Chk_r_Elbow = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString());
            }
            if (ds.Tables[0].Rows[0]["LeftWrist"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftWrist";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftWrist"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_l_Wrist.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftWrist"].ToString());
                Chk_l_Wrist = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftWrist"].ToString());
            }
            if (ds.Tables[0].Rows[0]["RightWrist"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightWrist";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightWrist"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_Wrist.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightWrist"].ToString());
                Chk_r_Wrist = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightWrist"].ToString());
            }
            if (ds.Tables[0].Rows[0]["LeftHip"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LeftHip";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftHip"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_l_Hip.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftHip"].ToString());
                Chk_l_Hip = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftHip"].ToString());
            }
            if (ds.Tables[0].Rows[0]["RightHip"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "RightHip";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightHip"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_r_Hip.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightHip"].ToString());
                Chk_r_Hip = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightHip"].ToString());
            }

            if (ds.Tables[0].Rows[0]["Neck"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "Neck";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Neck"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_Neck.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Neck"].ToString());
                Chk_Neck = Convert.ToBoolean(ds.Tables[0].Rows[0]["Neck"].ToString());
            }
            if (ds.Tables[0].Rows[0]["MidBack"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "MidBack";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["MidBack"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_Midback.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["MidBack"].ToString());
                Chk_Midback = Convert.ToBoolean(ds.Tables[0].Rows[0]["MidBack"].ToString());
            }
            if (ds.Tables[0].Rows[0]["LowBack"] != DBNull.Value)
            {
                BodyParts bp = new BodyParts();
                bp.Parts = "LowBack";
                bp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LowBack"].ToString()); ;
                bodyPartsList.Add(bp);
                chk_lowback.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LowBack"].ToString());
                Chk_lowback = Convert.ToBoolean(ds.Tables[0].Rows[0]["LowBack"].ToString());
            }

            Session["bodyPartsList"] = bodyPartsList;

            txt_other.Text = ds.Tables[0].Rows[0]["Others"].ToString();
        }
    }

    //#region Check box Event checked changed of body part.
    /// <summary>
    /// Event fires if check box chk_r_Shoulder checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Shoulder_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master;
            master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;

            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }

        bindADL();
    }
    /// <summary>
    /// Event fires if check box chk_L_Shoulder checked or unchecked in the form.
    /// </summary>
    protected void chk_L_Shoulder_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }
        bindADL();
    }
    /// <summary>
    /// Event fires if check box chk_Neck checked or unchecked in the form.
    /// </summary>
    protected void chk_Neck_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master;
            master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }
    }
    /// <summary>
    /// Event fires if check box chk_Midback checked or unchecked in the form.
    /// </summary>
    protected void chk_Midback_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }
    }
    /// <summary>
    /// Event fires if check box chk_lowback checked or unchecked in the form.
    /// </summary>
    protected void chk_lowback_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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
                s.Add(d);
                Chk_lowback = true;
            }
            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Keen checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Keen_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }
        bindADL();
    }
    /// <summary>
    /// Event fires if check box chk_L_Keen checked or unchecked in the form.
    /// </summary>
    protected void chk_L_Keen_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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

            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }
        bindADL();
    }
    /// <summary>
    /// Event fires if check box chk_r_Elbow checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Elbow_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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

            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }

    }
    /// <summary>
    /// Event fires if check box chk_l_Elbow checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Elbow_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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

            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Wrist checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Wrist_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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

            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }
    }
    /// <summary>
    /// Event fires if check box chk_l_Wrist checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Wrist_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_Hip checked or unchecked in the form.
    /// </summary>
    protected void chk_r_Hip_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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

            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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

            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }
    }
    /// <summary>
    /// Event fires if check box chk_l_Hip checked or unchecked in the form.
    /// </summary>
    protected void chk_l_Hip_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
        }
    }
    /// <summary>
    /// Event fires if check box chk_r_ankle checked or unchecked in the form.
    /// </summary>
    protected void chk_r_ankle_CheckedChanged(object sender, EventArgs e)
    {
        SaveData(); var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;

            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;

            PageMainMaster master = (PageMainMaster)this.Master; master.bindData(Session["PatientIE_ID"].ToString());

        }
    }
    /// <summary>
    /// Event fires if check box chk_l_ankle checked or unchecked in the form.
    /// </summary>
    protected void chk_l_ankle_CheckedChanged(object sender, EventArgs e)
    {

        SaveData();
        var s = (List<BodyParts>)Session["bodyPartsList"];
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
            Session["bodyPartsList"] = s;
            PageMainMaster master = (PageMainMaster)this.Master;
            master.bindData(Session["PatientIE_ID"].ToString());
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
            Session["bodyPartsList"] = s1;
            PageMainMaster master = (PageMainMaster)this.Master;
            master.bindData(Session["PatientIE_ID"].ToString());

        }
    }

    public void SaveData()
    {
        string SP = "";
        SqlParameter[] param = null;

        //starting the insertion in database

        string LOC = "", workat = "", DrSeen = "";

        if (chk_loc.Checked)
            LOC = txt_howlong.Value + "|" + ddl_howlong.SelectedItem.Text;
        else
            LOC = "0|undetermined time";

        if (ddl_work_status.Text == "")
            workat = "Patient works as unknown";
        else
            workat = ddl_work_status.Text;

        bool sameday = false;
        int day = 0;

        if (rep_wenttohospital.SelectedValue == "0")
        {
            sameday = true;
        }
        else
        {
            if (txt_day.Value == "0" || txt_day.Value == "")
                sameday = true;
            else
                day = Convert.ToInt16(txt_day.Value);
        }

        if (rbl_seen_injury.SelectedValue == "0")
        {
            //if (txt_docname.Value != "")
            //    DrSeen = "The patient has visited Dr " + txt_docname.Value;
            //else
            //    DrSeen = "The patient has visited Dr X since the accident.";
            DrSeen = txt_docname.Value;
        }

        DBHelperClass db = new DBHelperClass();
        //string SP = "";
        //SqlParameter[] param = null;
        param = new SqlParameter[67];
        SP = "usp_Save_IntakePage2";

        param[0] = new SqlParameter("@Sustained", ddl_accident_desc.Text);
        param[1] = new SqlParameter("@Position", ddl_belt.Text);
        param[2] = new SqlParameter("@InvolvedIn", ddl_invovledin.Text);
        param[3] = new SqlParameter("@CriteriaA", rep_hospitalized.SelectedItem.Value);
        param[4] = new SqlParameter("@EMSTeam", ddl_EMS.Text);
        param[5] = new SqlParameter("@CC2", rbl_in_past.SelectedItem.Value);
        param[6] = new SqlParameter("@WentTo", txt_hospital.Text);
        param[7] = new SqlParameter("@Via", ddl_via.Text);
        param[8] = new SqlParameter("@HadXrayOf", txt_x_ray.Value);
        param[9] = new SqlParameter("@HadCTScanOf", txt_CT.Value);
        param[10] = new SqlParameter("@PrescriptionFor", txt_prescription.Value);
        param[11] = new SqlParameter("@FreeForm", ""); //Not saving it
        param[12] = new SqlParameter("@InjuryToHead", chk_headinjury.Checked);
        param[13] = new SqlParameter("@HadMRIOf", txt_mri.Value);
        param[14] = new SqlParameter("@LossOfConsciousnessFor", LOC);
        param[15] = new SqlParameter("@DaysLater", day);
        param[16] = new SqlParameter("@SameDay", sameday);
        param[17] = new SqlParameter("@HadTreatmentFor", txt_injur_past_bp.Value);
        param[18] = new SqlParameter("@DetailInfo", txt_injur_past_how.Value);
        param[19] = new SqlParameter("@Neck", chk_Neck.Checked);
        param[20] = new SqlParameter("@MidBack", chk_Midback.Checked);
        param[21] = new SqlParameter("@LowBack", chk_lowback.Checked);
        param[22] = new SqlParameter("@LeftShoulder", chk_L_Shoulder.Checked);
        param[23] = new SqlParameter("@RightShoulder", chk_r_Shoulder.Checked);
        param[24] = new SqlParameter("@LeftKnee", chk_L_Keen.Checked);
        param[25] = new SqlParameter("@RightKnee", chk_r_Keen.Checked);
        param[26] = new SqlParameter("@LeftElbow", chk_l_Elbow.Checked);
        param[27] = new SqlParameter("@RightElbow", chk_r_Elbow.Checked);
        param[28] = new SqlParameter("@LeftWrist", chk_l_Wrist.Checked);
        param[29] = new SqlParameter("@RightWrist", chk_r_Wrist.Checked);
        param[30] = new SqlParameter("@LeftAnkle", chk_l_ankle.Checked);
        param[31] = new SqlParameter("@RightAnkle", chk_r_ankle.Checked);
        param[32] = new SqlParameter("@Others", txt_other.Text);
        param[33] = new SqlParameter("@LeftHip", chk_l_Hip.Checked);
        param[34] = new SqlParameter("@RightHip", chk_r_Hip.Checked);
        param[35] = new SqlParameter("@WorksAt", workat);
        param[36] = new SqlParameter("@AccidentDetail", txt_details.Text);
        param[37] = new SqlParameter("@DoctorSeen", DrSeen);
        param[38] = new SqlParameter("@PatientIE_ID", Session["PatientIE_ID"]);
        param[39] = new SqlParameter("@PMH", PMH.Text);
        param[40] = new SqlParameter("@PSH", PSH.Text);
        param[41] = new SqlParameter("@Medications", Medication.Text);
        param[42] = new SqlParameter("@Allergies", Allergies.Text);
        //added new on 16/7/2017
        param[43] = new SqlParameter("@ComplainingHeadeaches", chkComplainingofHeadaches.Checked);
        param[44] = new SqlParameter("@Persistent", txtPersistent.Text);
        param[45] = new SqlParameter("@Frontal", chkfrontal.Checked);
        param[46] = new SqlParameter("@LeftParietal", chkLeftParietal.Checked);
        param[47] = new SqlParameter("@RightParietal", chkRightParietal.Checked);
        param[48] = new SqlParameter("@LeftTemporal", chkLeftTemporal.Checked);
        param[49] = new SqlParameter("@RightTemporal", chkRightTemporal.Checked);
        param[50] = new SqlParameter("@Occipital", chkOccipital.Checked);
        param[51] = new SqlParameter("@Global", chkGlobal.Checked);
        param[52] = new SqlParameter("@SevereAnxiety", chkSevereAnxiety.Checked);
        param[53] = new SqlParameter("@Nausea", chkNausea.Checked);
        param[54] = new SqlParameter("@Dizziness", chkDizziness.Checked);
        param[55] = new SqlParameter("@Vomiting", chkVomitting.Checked);
        param[56] = new SqlParameter("@HeadechesAssociated", chkHeadechesAssociated.Checked);
        param[57] = new SqlParameter("@FamilyHistory", FamilyHistory.Text);

        param[58] = new SqlParameter("@DeniesSmoking", chkDeniessmoking.Checked);
        param[59] = new SqlParameter("@DeniesDrinking", chkDeniesdrinking.Checked);
        param[60] = new SqlParameter("@DeniesDrugs", chkDeniesdrugs.Checked);
        param[61] = new SqlParameter("@DeniesSocialDrinking", chkSocialdrinking.Checked);
        //Added vitals on 01102017
        param[62] = new SqlParameter("@Vitals", txtVitals.Text);
        if (Session["OccurOn"] != null)
        { param[63] = new SqlParameter("@OccurOn", Convert.ToBoolean(Session["OccurOn"])); }
        else
        { param[63] = new SqlParameter("@OccurOn", false); }

        param[64] = new SqlParameter("@Missed", txtMissed.Text);
        param[65] = new SqlParameter("@ReturnToWork", cboReturnedToWork.Text);
        param[66] = new SqlParameter("@InvolvedInOther", txtInvolvedOther.Text);

        //Insert values in the db.
        int val = db.executeSP(SP, param);

    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        string tblname = ((Button)sender).CommandArgument;
        string query = "delete from " + tblname + " where PatientIE_ID=" + Session["PatientIE_ID"].ToString();

        int val = gDbhelperobj.executeQuery(query);
    }

    public void bindHtml()
    {
        string path = Server.MapPath("~/Template/Page1_top.html");
        string body = File.ReadAllText(path);

        divtopHTML.InnerHtml = body;

        path = Server.MapPath("~/Template/Page1_social.html");
        body = File.ReadAllText(path);

        divsocialHTML.InnerHtml = body;

        path = Server.MapPath("~/Template/Page1_accident.html");
        string bodyaccident = File.ReadAllText(path);



        path = Server.MapPath("~/Template/Page1_history.html");
        body = File.ReadAllText(path);


        DataSet ds = new DBHelperClass().selectData("select Sex,Age,DOA,Handedness,Compensation from View_PatientIE where PatientIE_ID=" + Session["PatientIE_ID"].ToString());

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string sex = ds.Tables[0].Rows[0]["sex"].ToString();
            string handness = ds.Tables[0].Rows[0]["Handedness"].ToString();
            string compensation = (ds.Tables[0].Rows[0]["Compensation"].ToString() == "WC") ? "work related" : "motor vehicle";

            ViewState["gender"] = sex.ToLower() == "mr." ? "he" : "she";

            body = body.Replace("#handeness", handness);

            body = body.Replace("#gender", sex.ToLower() == "mr." ? "male" : "female");

            //bodyaccident = bodyaccident.Replace("#getlad", sex.ToLower() == "mr." ? "gentleman" : "gentlewoman");
            //body = body.Replace("#gentlady", sex.ToLower() == "mr." ? "gentleman" : "gentlewoman");
            bodyaccident = bodyaccident.Replace("#getlad", sex.ToLower() == "mr." ? "male" : "female");
            body = body.Replace("#gentlady", sex.ToLower() == "mr." ? "male" : "female");
            body = body.Replace("#sex", sex.ToLower() == "mr." ? "He" : "She");
            body = body.Replace("#lsex", sex.ToLower() == "mr." ? "he" : "she");
            body = body.Replace("#her_his", sex.ToLower() == "mr." ? "him" : "her");
            body = body.Replace("#age", ds.Tables[0].Rows[0]["Age"].ToString());
            body = body.Replace("#wctype", compensation);
            body = body.Replace("#doa", (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DOA"].ToString()) == false) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["DOA"].ToString()).ToString("MM/dd/yyyy") : "");
        }

        divhistoryHTML.InnerHtml = body;

        divaccidentHTML.InnerHtml = bodyaccident;
    }

    public void bindADL()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/XML/Default_Admin.xml"));
        XmlNodeList nodeList;

        string content = "";

        #region Page1
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage1");
        foreach (XmlNode node in nodeList)
        {
            if ((chk_L_Shoulder.Checked || chk_r_Shoulder.Checked) && (chk_r_Keen.Checked || chk_L_Keen.Checked))
                content = node.SelectSingleNode("ADLBoth").InnerText;
            else if ((chk_L_Shoulder.Checked || chk_r_Shoulder.Checked))
                content = node.SelectSingleNode("ADLShoulder").InnerText;
            else if ((chk_L_Keen.Checked || chk_r_Keen.Checked))
                content = node.SelectSingleNode("ADLKnee").InnerText;

            content = content.Replace("#gender", ViewState["gender"].ToString());

            string str = ViewState["gender"].ToString();

            str = char.ToUpper(str[0]) + str.Substring(1);

            txt_ADL_Content.Text = content.Replace("#Cgender", str).Trim();
        }
        #endregion
    }
}
