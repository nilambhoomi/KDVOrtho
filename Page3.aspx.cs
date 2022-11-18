using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;
using System.IO;

public partial class Page3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {


            if (Session["PatientIE_ID"] == null)
            {
                Response.Redirect("Page1.aspx");

            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("WorkStatus", typeof(string)) });
                dt.Rows.Add("Able to go back to work");
                dt.Rows.Add("Working");
                dt.Rows.Add("Not Working");
                dt.Rows.Add("Partially Working");
                Repeater1.DataSource = dt;
                Repeater1.DataBind();

                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select ISFirst FROM tblPatientIEDetailPage2 WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ISFirst"] != DBNull.Value ? !Convert.ToBoolean(ds.Tables[0].Rows[0]["ISFirst"].ToString()) : true)
                { DefaultValue(); }
                else { bindData(); }

                ds = db.selectData("select FreeForm from tblPatientIEDetailPage1 where PatientIE_ID=" + Session["PatientIE_ID"].ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_FreeForm.Text = ds.Tables[0].Rows[0][0].ToString();
                    bindChkValues(txt_FreeForm.Text);
                }


                query = "select top 1 * from tblPage2HTMLContent where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
                ds = db.selectData(query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bindData();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["rosSectionNewHTML"].ToString()))
                    {
                        txtROS.Text = ds.Tables[0].Rows[0]["rosSectionNewHTML"].ToString();
                    }
                    else
                    {
                        txtROS.Text = "Temperature taken at the time of exam was ___ degrees Fahrenheit.<br/><b>General:</b> No fever, chills, night sweats, weight gain, or weight loss. ";
                        //"               <b>HEENT:</b> No double vision, eye pain, eye redness, decreased hearing, earache, ear ringing, nosebleeds, sore throat or hoarseness. " +
                        //"<b>Endocrine:</b> No cold intolerance, appetite changes or hair changes. " +
                        //"               <b>Skin:</b> Clear, no rashes or lesions " +
                        //"<b>Neuro:</b> No headaches, dizziness, vertigo or tremor. " +
                        //"               <b>Respiratory:</b> No wheezing, coughing, shortness of breath or difficulty breathing. " +
                        //"<b>Cardiovascular:</b> No chest pain, murmurs, irregular heart rate or hypertension. " +
                        //"               <b>GI:</b> No nausea, vomiting, diarrhea, constipation, jaundice or changes in bowel habits. " +
                        //  "<b>GU:</b> No blood in urine, painful urination, loss of bladder control or urinary retention. " +
                        //  "<b>Hematology:</b> No active bleeding, bruising, anemia or blood clotting disorders. " +
                        //"               <b>Psychiatric:</b> No anxiety, change in sleep pattern, depression or suicidal thoughts.";


                    }
                }
                else
                {
                    bindHtml();
                    txtROS.Text = "Temperature taken at the time of exam was ___ degrees Fahrenheit.<br/><b>General:</b> No fever, chills, night sweats, weight gain, or weight loss.";
               //"               <b>HEENT:</b> No double vision, eye pain, eye redness, decreased hearing, earache, ear ringing, nosebleeds, sore throat or hoarseness.<br/><br/>" +
               //"<b>Endocrine:</b> No cold intolerance, appetite changes or hair changes. <br/><br/>" +
               //"               <b>Skin:</b> Clear, no rashes or lesions<br/><br/>" +
               //"<b>Neuro:</b> No headaches, dizziness, vertigo or tremor.<br/><br/>" +
               //"               <b>Respiratory:</b> No wheezing, coughing, shortness of breath or difficulty breathing.<br/><br/>" +
               //"<b>Cardiovascular:</b> No chest pain, murmurs, irregular heart rate or hypertension.<br/><br/>" +
               //"               <b>GI:</b> No nausea, vomiting, diarrhea, constipation, jaundice or changes in bowel habits.<br/><br/>" +
               //  "<b>GU:</b> No blood in urine, painful urination, loss of bladder control or urinary retention.<br/><br/>" +
               //  "<b>Hematology:</b> No active bleeding, bruising, anemia or blood clotting disorders.<br/><br/>" +
               //"               <b>Psychiatric:</b> No anxiety, change in sleep pattern, depression or suicidal thoughts.<br/><br/>";


                }
            }
            bindPage1values();
        }



        Logger.Info(Session["uname"].ToString() + "- Visited in  Page2 for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";

        query = "select top 1 * from tblPage2HTMLContent where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        DataSet ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count == 0)
        {


            //query = "insert into tblPatientIEDetailPage2 (Seizures,ChestPain,ShortnessOfBreath,Jawpain,AbdominalPain,Fevers,Diarrhea,";
            //query = query + "Bowel,RecentWeightloss,Episodic,Rashes,PatientIE_ID,NightSweats,DoubleVision,HearingLoss,Depression,ISFirst,dloodinurine,HTMLContent) values (";
            //query = query + "'" + chk_seizures.Checked + "','" + chk_chest_pain.Checked + "','" + chk_shortness_of_breath.Checked + "',";
            //query = query + "'" + chk_jaw_pain.Checked + "','" + chk_abdominal_pain.Checked + "','" + chk_fever.Checked + "','" + chk_diarrhea.Checked + "',";
            //query = query + "'" + chk_bowel_bladder.Checked + "','" + chk_recent_wt.Checked + "',";
            //query = query + "'" + chk_episodic_ligth.Checked + "','" + chk_rashes.Checked + "','" + Session["PatientIE_ID"].ToString() + "',";
            //query = query + "'" + chk_sleep_disturbance.Checked + "','" + chk_blurred.Checked + "','" + chk_hearing_loss.Checked + "','" + chk_depression.Checked + "',1,'" + chk_bloodinurine.Checked + "','" + hdHTMLContent.Value + "' )";
            query = "insert into tblPage2HTMLContent(PatientIE_ID,topSectionHTML,degreeSectionHTML,rosSectionHTML,rosSectionNewHTML)values(@PatientIE_ID,@topSectionHTML,@degreeSectionHTML,@rosSectionHTML,@rosSectionNewHTML)";
        }
        else
        {
            //query = "update tblPatientIEDetailPage2 set Seizures='" + chk_seizures.Checked + "',";
            //query = query + "ChestPain='" + chk_chest_pain.Checked + "',ShortnessOfBreath='" + chk_shortness_of_breath.Checked + "',";
            //query = query + "Jawpain='" + chk_jaw_pain.Checked + "',AbdominalPain='" + chk_abdominal_pain.Checked + "',Fevers='" + chk_fever.Checked + "',Diarrhea='" + chk_diarrhea.Checked + "',";
            //query = query + "Bowel='" + chk_bowel_bladder.Checked + "',RecentWeightloss='" + chk_recent_wt.Checked + "',";
            //query = query + "Episodic='" + chk_episodic_ligth.Checked + "',Rashes='" + chk_rashes.Checked;
            //query = query + "',NightSweats='" + chk_sleep_disturbance.Checked + "',DoubleVision='" + chk_blurred.Checked;
            //query = query + "',HearingLoss='" + chk_hearing_loss.Checked + "',Depression = '" + chk_depression.Checked + "',ISFirst=1 , dloodinurine = '" + chk_bloodinurine.Checked + "',HTMLContent='" + hdHTMLContent.Value + "',";
            //query = query + " Where PatientIE_ID=" + Session["PatientIE_ID"].ToString() + "";
            query = "update tblPage2HTMLContent set topSectionHTML=@topSectionHTML,degreeSectionHTML=@degreeSectionHTML,rosSectionHTML=@rosSectionHTML,rosSectionNewHTML=@rosSectionNewHTML where PatientIE_ID=@PatientIE_ID";
        }


        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
            command.Parameters.AddWithValue("@topSectionHTML", hdtopHTMLContent.Value);
            command.Parameters.AddWithValue("@degreeSectionHTML", hddegreeHTMLContent.Value);
            command.Parameters.AddWithValue("@rosSectionHTML", hdrosHTMLContent.Value);
            command.Parameters.AddWithValue("@rosSectionNewHTML", txtROS.Text);

            connection.Open();
            var results = command.ExecuteNonQuery();
            connection.Close();
        }


        //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        //using (SqlCommand command = new SqlCommand(query, connection))
        //{
        //    command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
        //    command.Parameters.AddWithValue("@HTMLContent", hdHTMLContent.Value);
        //    command.Parameters.AddWithValue("@DisabilityContent", hdSection1.Value);



        //    connection.Open();
        //    var results = command.ExecuteNonQuery();
        //    connection.Close();
        //}



        ds.Dispose();


        query = "update tblPatientIE set FreeForm='" + GetFreeForm() + "' where PatientIE_ID=" + Convert.ToString(Session["PatientIE_ID"]);

        db.executeQuery(query);

        query = "select top 1 * from tblPatientIEDetailPage1 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count == 0)
        {
            query = "Insert INTO tblPatientIEDetailPage1 (HeadechesAssociated,Nausea,Dizziness,Vomiting,SevereAnxiety,PatientIE_ID) Values ";
            query = query + "('" + chk_headaches.Checked + "','" + chk_nausea.Checked + "','" + chk_dizziness.Checked + "','" + chk_vomiting.Checked + "','";
            query = query + chk_anxiety.Checked + "'," + Session["PatientIE_ID"].ToString() + ")";
        }
        else
        {
            query = "Update tblPatientIEDetailPage1 SET HeadechesAssociated='" + chk_headaches.Checked + "',";
            query = query + " Nausea='" + chk_nausea.Checked + "',Dizziness='" + chk_dizziness.Checked + "',";
            query = query + "Vomiting='" + chk_vomiting.Checked + "',SevereAnxiety='" + chk_anxiety.Checked + "' Where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        }
        ds.Dispose();

        int val = db.executeQuery(query);

        bool bpActive = false;
        if (chk_tingling_in_arms.Checked || chk_pain_radiating_shoulder.Checked || chk_numbness_in_arm.Checked || chk_weakness_in_arm.Checked)
            bpActive = true;
        else
            bpActive = false;

        if (bpActive)
        {
            query = "select Neck from tblInjuredBodyParts where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
                query = "Insert INTO tblInjuredBodyParts (PatientIE_ID, Neck) VALUES (" + Session["PatientIE_ID"].ToString() + ",'true')";
            else
                query = "Update tblInjuredBodyParts SET Neck='true' WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString();
            ds.Dispose();

            val = db.executeQuery(query);

            query = "select top 1 * from tblbpNeck where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
            {
                query = "Insert INTO tblbpNeck (Numbness,Tingling,ShoulderBilateral1,ArmBilateral2,WeeknessIn,PatientIE_ID) Values ";
                query = query + "('" + chk_tingling_in_arms.Checked + "','" + chk_tingling_in_arms.Checked + "','" + chk_pain_radiating_shoulder.Checked + "','" + chk_numbness_in_arm.Checked + "','";
                query = query + (chk_weakness_in_arm.Checked ? "arm." : "");
                query = query + "'," + Session["PatientIE_ID"].ToString() + ")";
            }
            else
            {
                query = "Update tblbpNeck SET Numbness='" + chk_tingling_in_arms.Checked + "',";
                query = query + " Tingling='" + chk_tingling_in_arms.Checked + "',ShoulderBilateral1='" + chk_pain_radiating_shoulder.Checked + "',";
                query = query + "ArmBilateral2='" + chk_numbness_in_arm.Checked + "',WeeknessIn='" + (chk_weakness_in_arm.Checked ? "arm." : "") + "' Where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            }
            ds.Dispose();

            val = db.executeQuery(query);
        }

        if (chk_tingling_in_legs.Checked || chk_pain_radiating_leg.Checked || chk_numbess_in_leg.Checked || chk_weakness_in_leg.Checked)
            bpActive = true;
        else
            bpActive = false;

        if (bpActive)
        {
            query = "select LowBack from tblInjuredBodyParts where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
                query = "Insert INTO tblInjuredBodyParts (PatientIE_ID, LowBack) VALUES (" + Session["PatientIE_ID"].ToString() + ",'true')";
            else
                query = "Update tblInjuredBodyParts SET LowBack='true' WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString();
            ds.Dispose();

            val = db.executeQuery(query);

            query = "select top 1 * from tblbpLowBack where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
            {
                query = "Insert INTO tblbpLowBack (Numbness,Tingling,LegBilateral1,LegBilateral2,WeeknessIn,PatientIE_ID) Values ";
                query = query + "('" + chk_tingling_in_legs.Checked + "','" + chk_tingling_in_legs.Checked + "','" + chk_pain_radiating_leg.Checked + "','" + chk_numbess_in_leg.Checked + "','";
                query = query + (chk_weakness_in_leg.Checked ? "leg." : "");
                query = query + "'," + Session["PatientIE_ID"].ToString() + ")";
            }
            else
            {
                query = "Update tblbpLowBack SET Numbness='" + chk_tingling_in_legs.Checked + "',";
                query = query + " Tingling='" + chk_tingling_in_legs.Checked + "',LegBilateral1='" + chk_pain_radiating_leg.Checked + "',";
                query = query + "LegBilateral2='" + chk_numbess_in_leg.Checked + "',WeeknessIn='" + (chk_weakness_in_leg.Checked ? "arm." : "") + "' Where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            }
            ds.Dispose();

            val = db.executeQuery(query);
        }

        lblMessage.InnerHtml = "Complain and Restictions Save Successfully.";
        lblMessage.Attributes.Add("style", "color:green");

        upMessage.Update();



        query = "update  tblPatientIEDetailPage1 set FreeForm='" + txt_FreeForm.Text + "' where PatientIE_ID=" + Session["PatientIE_ID"].ToString();


        val = db.executeQuery(query);





        Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Create IE - Page3 " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        else
        {
            Response.Redirect("Page4.aspx");
        }
        // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
    }
    //string id, string bodyPart = null
    private void DefaultValue()
    {

        string SP = "";
        SqlParameter[] param = null;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
        XmlNodeList nodeList;
        #region Page3
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage2");
        foreach (XmlNode node in nodeList)
        {
            chk_abdominal_pain.Checked = node.SelectSingleNode("AbdominalPain") == null ? chk_abdominal_pain.Checked : Convert.ToBoolean(node.SelectSingleNode("AbdominalPain").InnerText);//page2
            chk_blurred.Checked = node.SelectSingleNode("DoubleVision") == null ? chk_blurred.Checked : Convert.ToBoolean(node.SelectSingleNode("DoubleVision").InnerText);//page2
            chk_bowel_bladder.Checked = node.SelectSingleNode("Bowel") == null ? chk_bowel_bladder.Checked : Convert.ToBoolean(node.SelectSingleNode("Bowel").InnerText);//page2
            chk_chest_pain.Checked = node.SelectSingleNode("ChestPain") == null ? chk_chest_pain.Checked : Convert.ToBoolean(node.SelectSingleNode("ChestPain").InnerText);//page2

            chk_diarrhea.Checked = node.SelectSingleNode("Diarrhea") == null ? chk_diarrhea.Checked : Convert.ToBoolean(node.SelectSingleNode("Diarrhea").InnerText);//page2
            chk_episodic_ligth.Checked = node.SelectSingleNode("Episodic") == null ? chk_episodic_ligth.Checked : Convert.ToBoolean(node.SelectSingleNode("Episodic").InnerText);//page2
            chk_fever.Checked = node.SelectSingleNode("Fevers") == null ? chk_fever.Checked : Convert.ToBoolean(node.SelectSingleNode("Fevers").InnerText);//page2
            chk_hearing_loss.Checked = node.SelectSingleNode("HearingLoss") == null ? chk_hearing_loss.Checked : Convert.ToBoolean(node.SelectSingleNode("HearingLoss").InnerText);//page2
            chk_jaw_pain.Checked = node.SelectSingleNode("Jawpain") == null ? chk_jaw_pain.Checked : Convert.ToBoolean(node.SelectSingleNode("Jawpain").InnerText);//page2
            chk_rashes.Checked = node.SelectSingleNode("Rashes") == null ? chk_rashes.Checked : Convert.ToBoolean(node.SelectSingleNode("Rashes").InnerText);//page2
            chk_recent_wt.Checked = node.SelectSingleNode("RecentWeightloss") == null ? chk_recent_wt.Checked : Convert.ToBoolean(node.SelectSingleNode("RecentWeightloss").InnerText);//page2
            chk_seizures.Checked = node.SelectSingleNode("Seizures") == null ? chk_seizures.Checked : Convert.ToBoolean(node.SelectSingleNode("Seizures").InnerText);//page2
            chk_shortness_of_breath.Checked = node.SelectSingleNode("ShortnessOfBreath") == null ? chk_shortness_of_breath.Checked : Convert.ToBoolean(node.SelectSingleNode("ShortnessOfBreath").InnerText);//page2
            chk_sleep_disturbance.Checked = node.SelectSingleNode("NightSweats") == null ? chk_sleep_disturbance.Checked : Convert.ToBoolean(node.SelectSingleNode("NightSweats").InnerText);//page2

            //txtIntactExcept.Text = node.SelectSingleNode("intactexcept").InnerText;
            //LEdtr.Checked = node.SelectSingleNode("LEdtr") == null ? LEdtr.Checked : Convert.ToBoolean(node.SelectSingleNode("LEdtr").InnerText);

            //chk_depression.Checked = node.SelectSingleNode("Depression") == null ? chk_depression.Checked : Convert.ToBoolean(node.SelectSingleNode("Depression").InnerText);

            //LTricepstxt.Text = node.SelectSingleNode("DTRtricepsLeft").InnerText;
            //RTricepstxt.Text = node.SelectSingleNode("DTRtricepsRight").InnerText;
            //LBicepstxt.Text = node.SelectSingleNode("DTRBicepsLeft").InnerText;
            //RBicepstxt.Text = node.SelectSingleNode("DTRBicepsRight").InnerText;
            //RBrachioradialis.Text = node.SelectSingleNode("DTRBrachioRight").InnerText;
            //LBrachioradialis.Text = node.SelectSingleNode("DTRBrachioLeft").InnerText;
            //LKnee.Text = node.SelectSingleNode("DTRKneeLeft").InnerText;
            //RKnee.Text = node.SelectSingleNode("DTRKneeRight").InnerText;
            //LAnkle.Text = node.SelectSingleNode("DTRAnkleLeft").InnerText;
            //RAnkle.Text = node.SelectSingleNode("DTRAnkleRight").InnerText;

            //UExchk.Checked = node.SelectSingleNode("UEdtr") == null ? UExchk.Checked : Convert.ToBoolean(node.SelectSingleNode("UEdtr").InnerText);

            //chkPinPrick.Checked = node.SelectSingleNode("Pinprick") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("Pinprick").InnerText);
            //chkLighttouch.Checked = node.SelectSingleNode("Lighttouch") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("Lighttouch").InnerText);
            //txtSensory.Text = node.SelectSingleNode("Sensory").InnerText;
            //LESen_Click.Checked = node.SelectSingleNode("LEsen") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("LEsen").InnerText);
            //TextBox4.Text = node.SelectSingleNode("LEL3Right").InnerText;
            //txtDMTL3.Text = node.SelectSingleNode("LEL3Left").InnerText;
            //TextBox6.Text = node.SelectSingleNode("LEL4Right").InnerText;
            //TextBox5.Text = node.SelectSingleNode("LEL4Left").InnerText;
            //TextBox8.Text = node.SelectSingleNode("LEL5Right").InnerText;
            //TextBox7.Text = node.SelectSingleNode("LEL5Left").InnerText;
            //TextBox10.Text = node.SelectSingleNode("LES1Left").InnerText;
            //TextBox21.Text = node.SelectSingleNode("LES1Right").InnerText;
            //TextBox25.Text = node.SelectSingleNode("LELumberParaspinalsRight").InnerText;
            //TextBox24.Text = node.SelectSingleNode("LELumberParaspinalsLeft").InnerText;

            //UESen_Click.Checked = node.SelectSingleNode("UEsen") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("UEsen").InnerText);
            //TextBox9.Text = node.SelectSingleNode("UEC5Left").InnerText;
            //txtUEC5Right.Text = node.SelectSingleNode("UEC5Right").InnerText;

            //TextBox11.Text = node.SelectSingleNode("UEC6Left").InnerText;
            //TextBox12.Text = node.SelectSingleNode("UEC6Right").InnerText;

            //TextBox13.Text = node.SelectSingleNode("UEC7Left").InnerText;
            //TextBox14.Text = node.SelectSingleNode("UEC7Right").InnerText;

            //TextBox15.Text = node.SelectSingleNode("UEC8Left").InnerText;
            //TextBox16.Text = node.SelectSingleNode("UEC8Right").InnerText;

            //TextBox18.Text = node.SelectSingleNode("UET1Right").InnerText;
            //TextBox17.Text = node.SelectSingleNode("UET1Left").InnerText;
            //TextBox20.Text = node.SelectSingleNode("UECervicalParaspinalsRight").InnerText;
            //TextBox19.Text = node.SelectSingleNode("UECervicalParaspinalsLeft").InnerText;

            //cboHoffmanexam.SelectedValue = node.SelectSingleNode("HoffmanExam").InnerText;
            //chkStocking.Checked = node.SelectSingleNode("Stocking") == null ? chkStocking.Checked : Convert.ToBoolean(node.SelectSingleNode("Stocking").InnerText);
            //chkGlove.Checked = node.SelectSingleNode("Glove") == null ? chkGlove.Checked : Convert.ToBoolean(node.SelectSingleNode("Glove").InnerText);

            //LEmmst.Checked = node.SelectSingleNode("LEmmst") == null ? LEmmst.Checked : Convert.ToBoolean(node.SelectSingleNode("LEmmst").InnerText);
            //TextBox23.Text = node.SelectSingleNode("LEHipFlexionRight").InnerText;
            //TextBox22.Text = node.SelectSingleNode("LEHipFlexionLeft").InnerText;
            //TextBox41.Text = node.SelectSingleNode("LEHipAbductionRight").InnerText;
            //TextBox40.Text = node.SelectSingleNode("LEHipAbductionLeft").InnerText;
            //TextBox27.Text = node.SelectSingleNode("LEKneeExtensionRight").InnerText;
            //TextBox26.Text = node.SelectSingleNode("LEKneeExtensionLeft").InnerText;

            //TextBox43.Text = node.SelectSingleNode("LEKneeFlexionRight").InnerText;
            //TextBox42.Text = node.SelectSingleNode("LEKneeFlexionLeft").InnerText;
            //TextBox29.Text = node.SelectSingleNode("LEAnkleDorsiRight").InnerText;
            //TextBox28.Text = node.SelectSingleNode("LEAnkleDorsiLeft").InnerText;

            //TextBox45.Text = node.SelectSingleNode("LEAnklePlantarRight").InnerText;
            //TextBox44.Text = node.SelectSingleNode("LEAnklePlantarLeft").InnerText;

            //TextBox47.Text = node.SelectSingleNode("LEAnkleExtensorRight").InnerText;
            //TextBox46.Text = node.SelectSingleNode("LEAnkleExtensorLeft").InnerText;

            //UEmmst.Checked = node.SelectSingleNode("UEmmst") == null ? UEmmst.Checked : Convert.ToBoolean(node.SelectSingleNode("UEmmst").InnerText);

            //TextBox31.Text = node.SelectSingleNode("UEShoulderAbductionRight").InnerText;
            //TextBox30.Text = node.SelectSingleNode("UEShoulderAbductionLeft").InnerText;

            //TextBox49.Text = node.SelectSingleNode("UEShoulderFlexionRight").InnerText;
            //TextBox48.Text = node.SelectSingleNode("UEShoulderFlexionLeft").InnerText;

            //TextBox33.Text = node.SelectSingleNode("UEElbowExtensionRight").InnerText;
            //TextBox32.Text = node.SelectSingleNode("UEElbowExtensionLeft").InnerText;

            //TextBox51.Text = node.SelectSingleNode("UEElbowFlexionRight").InnerText;
            //TextBox50.Text = node.SelectSingleNode("UEElbowFlexionLeft").InnerText;

            //TextBox53.Text = node.SelectSingleNode("UEElbowSupinationRight").InnerText;
            //TextBox52.Text = node.SelectSingleNode("UEElbowSupinationLeft").InnerText;

            //TextBox55.Text = node.SelectSingleNode("UEElbowPronationRight").InnerText;
            //TextBox54.Text = node.SelectSingleNode("UEElbowPronationLeft").InnerText;

            //TextBox37.Text = node.SelectSingleNode("UEWristFlexionRight").InnerText;
            //TextBox36.Text = node.SelectSingleNode("UEWristFlexionLeft").InnerText;
            //TextBox57.Text = node.SelectSingleNode("UEWristExtensionRight").InnerText;
            //TextBox56.Text = node.SelectSingleNode("UEWristExtensionLeft").InnerText;
            //TextBox39.Text = node.SelectSingleNode("UEHandGripStrengthRight").InnerText;
            //TextBox38.Text = node.SelectSingleNode("UEHandGripStrengthLeft").InnerText;
            //TextBox59.Text = node.SelectSingleNode("UEHandFingerAbductorsRight").InnerText;
            //TextBox58.Text = node.SelectSingleNode("UEHandFingerAbductorsLeft").InnerText;




            // chk_headaches.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_numbness_in_arm.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_numbess_in_leg.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_pain_radiating_leg.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_pain_radiating_shoulder.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_tingling_in_arms.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_tingling_in_legs.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_weakness_in_arm.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
            //chk_weakness_in_leg.Checked = Convert.ToBoolean(node.SelectSingleNode("").InnerText);
        }
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage1");
        foreach (XmlNode node in nodeList)
        {
            chk_vomiting.Checked = node.SelectSingleNode("Vomiting") == null ? chk_vomiting.Checked : Convert.ToBoolean(node.SelectSingleNode("Vomiting").InnerText);//page1
            chk_anxiety.Checked = node.SelectSingleNode("SevereAnxiety") == null ? chk_anxiety.Checked : Convert.ToBoolean(node.SelectSingleNode("SevereAnxiety").InnerText);//page1
            chk_nausea.Checked = node.SelectSingleNode("Nausea") == null ? chk_nausea.Checked : Convert.ToBoolean(node.SelectSingleNode("Nausea").InnerText);//page1
            chk_dizziness.Checked = node.SelectSingleNode("Dizziness") == null ? chk_dizziness.Checked : Convert.ToBoolean(node.SelectSingleNode("Dizziness").InnerText);//page1
        }
        #endregion
        //#region Page2
        //nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage3");
        //foreach (XmlNode node in nodeList)
        //{
        //param[1] = new SqlParameter("@Seizures", node.SelectSingleNode("Seizures").InnerText);
        //param[2] = new SqlParameter("@ChestPain", node.SelectSingleNode("ChestPain").InnerText);
        //param[3] = new SqlParameter("@ShortnessOfBreath", node.SelectSingleNode("ShortnessOfBreath").InnerText);
        //param[4] = new SqlParameter("@Jawpain", node.SelectSingleNode("Jawpain").InnerText);
        //param[5] = new SqlParameter("@AbdominalPain", node.SelectSingleNode("AbdominalPain").InnerText);
        //param[6] = new SqlParameter("@Fevers", node.SelectSingleNode("Fevers").InnerText);
        //param[7] = new SqlParameter("@NightSweats", node.SelectSingleNode("NightSweats").InnerText);
        //param[8] = new SqlParameter("@Diarrhea", node.SelectSingleNode("Diarrhea").InnerText);
        //param[9] = new SqlParameter("@DloodInUrine", node.SelectSingleNode("DloodInUrine").InnerText);
        //param[10] = new SqlParameter("@Bowel", node.SelectSingleNode("Bowel").InnerText);
        //param[11] = new SqlParameter("@DoubleVision", node.SelectSingleNode("DoubleVision").InnerText);
        //param[12] = new SqlParameter("@HearingLoss", node.SelectSingleNode("HearingLoss").InnerText);
        //param[13] = new SqlParameter("@RecentWeightloss", node.SelectSingleNode("RecentWeightloss").InnerText);
        //param[14] = new SqlParameter("@Episodic", node.SelectSingleNode("Episodic").InnerText);
        //param[15] = new SqlParameter("@Rashes", node.SelectSingleNode("Rashes").InnerText);
        //param[16] = new SqlParameter("@PMH", node.SelectSingleNode("PMH").InnerText);
        //param[17] = new SqlParameter("@PSH", node.SelectSingleNode("PSH").InnerText);
        //param[18] = new SqlParameter("@Medications", node.SelectSingleNode("Medications").InnerText);
        //param[19] = new SqlParameter("@Allergies", node.SelectSingleNode("Allergies").InnerText);
        //param[20] = new SqlParameter("@DeniesSmoking", node.SelectSingleNode("DeniesSmoking").InnerText);
        //param[21] = new SqlParameter("@DeniesDrinking", node.SelectSingleNode("DeniesDrinking").InnerText);
        //param[22] = new SqlParameter("@DeniesDrugs", node.SelectSingleNode("DeniesDrugs").InnerText);
        //param[23] = new SqlParameter("@DeniesSocialDrinking", node.SelectSingleNode("DeniesSocialDrinking").InnerText);
        //param[24] = new SqlParameter("@WorksAt", node.SelectSingleNode("WorksAt").InnerText);
        //param[25] = new SqlParameter("@Missed", node.SelectSingleNode("Missed").InnerText);
        //param[26] = new SqlParameter("@ReturnToWork", node.SelectSingleNode("ReturnToWork").InnerText);
        //param[27] = new SqlParameter("@intactexcept", node.SelectSingleNode("intactexcept").InnerText);
        //param[28] = new SqlParameter("@DTRtricepsRight", node.SelectSingleNode("DTRtricepsRight").InnerText);
        //param[29] = new SqlParameter("@DTRtricepsLeft", node.SelectSingleNode("DTRtricepsLeft").InnerText);
        //param[30] = new SqlParameter("@DTRBicepsRight", node.SelectSingleNode("DTRBicepsRight").InnerText);
        //param[31] = new SqlParameter("@DTRBicepsLeft", node.SelectSingleNode("DTRBicepsLeft").InnerText);
        //param[32] = new SqlParameter("@DTRKneeRight", node.SelectSingleNode("DTRKneeRight").InnerText);
        //param[33] = new SqlParameter("@DTRKneeLeft", node.SelectSingleNode("DTRKneeLeft").InnerText);
        //param[34] = new SqlParameter("@DTRBrachioRight", node.SelectSingleNode("DTRBrachioRight").InnerText);
        //param[35] = new SqlParameter("@DTRBrachioLeft", node.SelectSingleNode("DTRBrachioLeft").InnerText);
        //param[36] = new SqlParameter("@Sensory", node.SelectSingleNode("Sensory").InnerText);
        //param[37] = new SqlParameter("@Pinprick", node.SelectSingleNode("Pinprick").InnerText);
        //param[38] = new SqlParameter("@Lighttouch", node.SelectSingleNode("Lighttouch").InnerText);
        //param[39] = new SqlParameter("@UEC5Right", node.SelectSingleNode("UEC5Right").InnerText);
        //param[40] = new SqlParameter("@UEC5Left", node.SelectSingleNode("UEC5Left").InnerText);
        //param[41] = new SqlParameter("@UEC6Right", node.SelectSingleNode("UEC6Right").InnerText);
        //param[42] = new SqlParameter("@UEC6Left", node.SelectSingleNode("UEC6Left").InnerText);
        //param[43] = new SqlParameter("@UEC7Right", node.SelectSingleNode("UEC7Right").InnerText);
        //param[44] = new SqlParameter("@UEC7Left", node.SelectSingleNode("UEC7Left").InnerText);
        //param[45] = new SqlParameter("@UEC8Right", node.SelectSingleNode("UEC8Right").InnerText);
        //param[46] = new SqlParameter("@UEC8Left", node.SelectSingleNode("UEC8Left").InnerText);
        //param[47] = new SqlParameter("@UET1Right", node.SelectSingleNode("UET1Right").InnerText);
        //param[48] = new SqlParameter("@UET1Left", node.SelectSingleNode("UET1Left").InnerText);
        //param[49] = new SqlParameter("@UECervicalParaspinalsRight", node.SelectSingleNode("UECervicalParaspinalsRight").InnerText);
        //param[50] = new SqlParameter("@UECervicalParaspinalsLeft", node.SelectSingleNode("UECervicalParaspinalsLeft").InnerText);
        //param[51] = new SqlParameter("@LEL3Right", node.SelectSingleNode("LEL3Right").InnerText);
        //param[52] = new SqlParameter("@LEL3Left", node.SelectSingleNode("LEL3Left").InnerText);
        //param[53] = new SqlParameter("@LEL4Right", node.SelectSingleNode("LEL4Right").InnerText);
        //param[54] = new SqlParameter("@LEL4Left", node.SelectSingleNode("LEL4Left").InnerText);
        //param[55] = new SqlParameter("@LEL5Right", node.SelectSingleNode("LEL5Right").InnerText);
        //param[56] = new SqlParameter("@LEL5Left", node.SelectSingleNode("LEL5Left").InnerText);
        //param[57] = new SqlParameter("@LELumberParaspinalsRight", node.SelectSingleNode("LELumberParaspinalsRight").InnerText);
        //param[58] = new SqlParameter("@LELumberParaspinalsLeft", node.SelectSingleNode("LELumberParaspinalsLeft").InnerText);
        //param[59] = new SqlParameter("@HoffmanExam", node.SelectSingleNode("HoffmanExam").InnerText);
        //param[60] = new SqlParameter("@Stocking", node.SelectSingleNode("Stocking").InnerText);
        //param[61] = new SqlParameter("@Glove", node.SelectSingleNode("Glove").InnerText);
        //param[62] = new SqlParameter("@UEShoulderAbductionRight", node.SelectSingleNode("UEShoulderAbductionRight").InnerText);
        //param[63] = new SqlParameter("@UEShoulderAbductionLeft", node.SelectSingleNode("UEShoulderAbductionLeft").InnerText);
        //param[64] = new SqlParameter("@UEShoulderFlexionRight", node.SelectSingleNode("UEShoulderFlexionRight").InnerText);
        //param[65] = new SqlParameter("@UEShoulderFlexionLeft", node.SelectSingleNode("UEShoulderFlexionLeft").InnerText);
        //param[66] = new SqlParameter("@UEElbowExtensionRight", node.SelectSingleNode("UEElbowExtensionRight").InnerText);
        //param[67] = new SqlParameter("@UEElbowExtensionLeft", node.SelectSingleNode("UEElbowExtensionLeft").InnerText);
        //param[68] = new SqlParameter("@UEElbowFlexionRight", node.SelectSingleNode("UEElbowFlexionRight").InnerText);
        //param[69] = new SqlParameter("@UEElbowFlexionLeft", node.SelectSingleNode("UEElbowFlexionLeft").InnerText);
        //param[70] = new SqlParameter("@UEElbowSupinationRight", node.SelectSingleNode("UEElbowSupinationRight").InnerText);
        //param[71] = new SqlParameter("@UEElbowSupinationLeft", node.SelectSingleNode("UEElbowSupinationLeft").InnerText);
        //param[72] = new SqlParameter("@UEElbowPronationRight", node.SelectSingleNode("UEElbowPronationRight").InnerText);
        //param[73] = new SqlParameter("@UEElbowPronationLeft", node.SelectSingleNode("UEElbowPronationLeft").InnerText);
        //param[74] = new SqlParameter("@UEWristFlexionRight", node.SelectSingleNode("UEWristFlexionRight").InnerText);
        //param[75] = new SqlParameter("@UEWristFlexionLeft", node.SelectSingleNode("UEWristFlexionLeft").InnerText);
        //param[76] = new SqlParameter("@UEWristExtensionRight", node.SelectSingleNode("UEWristExtensionRight").InnerText);
        //param[77] = new SqlParameter("@UEWristExtensionLeft", node.SelectSingleNode("UEWristExtensionLeft").InnerText);
        //param[78] = new SqlParameter("@UEHandGripStrengthRight", node.SelectSingleNode("UEHandGripStrengthRight").InnerText);
        //param[79] = new SqlParameter("@UEHandGripStrengthLeft", node.SelectSingleNode("UEHandGripStrengthLeft").InnerText);
        //param[80] = new SqlParameter("@UEHandFingerAbductorsRight", node.SelectSingleNode("UEHandFingerAbductorsRight").InnerText);
        //param[81] = new SqlParameter("@UEHandFingerAbductorsLeft", node.SelectSingleNode("UEHandFingerAbductorsLeft").InnerText);
        //param[82] = new SqlParameter("@LEHipFlexionRight", node.SelectSingleNode("LEHipFlexionRight").InnerText);
        //param[83] = new SqlParameter("@LEHipFlexionLeft", node.SelectSingleNode("LEHipFlexionLeft").InnerText);
        //param[84] = new SqlParameter("@LEHipAbductionRight", node.SelectSingleNode("LEHipAbductionRight").InnerText);
        //param[85] = new SqlParameter("@LEHipAbductionLeft", node.SelectSingleNode("LEHipAbductionLeft").InnerText);
        //param[86] = new SqlParameter("@LEKneeExtensionRight", node.SelectSingleNode("LEKneeExtensionRight").InnerText);
        //param[87] = new SqlParameter("@LEKneeExtensionLeft", node.SelectSingleNode("LEKneeExtensionLeft").InnerText);
        //param[88] = new SqlParameter("@LEKneeFlexionRight", node.SelectSingleNode("LEKneeFlexionRight").InnerText);
        //param[89] = new SqlParameter("@LEKneeFlexionLeft", node.SelectSingleNode("LEKneeFlexionLeft").InnerText);
        //param[90] = new SqlParameter("@LEAnkleDorsiRight", node.SelectSingleNode("LEAnkleDorsiRight").InnerText);
        //param[91] = new SqlParameter("@LEAnkleDorsiLeft", node.SelectSingleNode("LEAnkleDorsiLeft").InnerText);
        //param[92] = new SqlParameter("@LEAnklePlantarRight", node.SelectSingleNode("LEAnklePlantarRight").InnerText);
        //param[93] = new SqlParameter("@LEAnklePlantarLeft", node.SelectSingleNode("LEAnklePlantarLeft").InnerText);
        //param[94] = new SqlParameter("@LEAnkleExtensorRight", node.SelectSingleNode("LEAnkleExtensorRight").InnerText);
        //param[95] = new SqlParameter("@LEAnkleExtensorLeft", node.SelectSingleNode("LEAnkleExtensorLeft").InnerText);
        //param[96] = new SqlParameter("@CreatedBy", "Default");
        //param[97] = new SqlParameter("@CreatedDate", Convert.ToDateTime(DateTime.Now));
        //param[98] = new SqlParameter("@LES1Left", node.SelectSingleNode("LES1Left").InnerText);
        //param[99] = new SqlParameter("@LES1Right", node.SelectSingleNode("LES1Right").InnerText);
        //param[100] = new SqlParameter("@DTRAnkleRight", node.SelectSingleNode("DTRAnkleRight").InnerText);
        //param[101] = new SqlParameter("@DTRAnkleLeft", node.SelectSingleNode("DTRAnkleLeft").InnerText);
        //param[102] = new SqlParameter("@UEdtr", node.SelectSingleNode("UEdtr").InnerText);
        //param[103] = new SqlParameter("@LEdtr", node.SelectSingleNode("LEdtr").InnerText);
        //param[104] = new SqlParameter("@UEsen", node.SelectSingleNode("UEsen").InnerText);
        //param[105] = new SqlParameter("@LEsen", node.SelectSingleNode("LEsen").InnerText);
        //param[106] = new SqlParameter("@UEmmst", node.SelectSingleNode("UEmmst").InnerText);
        //param[107] = new SqlParameter("@LEmmst", node.SelectSingleNode("LEmmst").InnerText);
        //}
        //#endregion


    }
    private void bindData()
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";

        query = "select top 1 * from tblPatientIEDetailPage2 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        DataSet ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {


            chk_seizures.Checked = ds.Tables[0].Rows[0]["Seizures"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Seizures"].ToString()) : false;
            chk_chest_pain.Checked = ds.Tables[0].Rows[0]["ChestPain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ChestPain"].ToString()) : false;
            chk_shortness_of_breath.Checked = ds.Tables[0].Rows[0]["ShortnessOfBreath"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ShortnessOfBreath"].ToString()) : false;
            chk_jaw_pain.Checked = ds.Tables[0].Rows[0]["Jawpain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Jawpain"].ToString()) : false;
            chk_abdominal_pain.Checked = ds.Tables[0].Rows[0]["AbdominalPain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AbdominalPain"].ToString()) : false;
            chk_fever.Checked = ds.Tables[0].Rows[0]["Fevers"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Fevers"].ToString()) : false;
            chk_diarrhea.Checked = ds.Tables[0].Rows[0]["Diarrhea"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Diarrhea"].ToString()) : false;
            chk_bowel_bladder.Checked = ds.Tables[0].Rows[0]["Bowel"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Bowel"].ToString()) : false;
            chk_blurred.Checked = ds.Tables[0].Rows[0]["DoubleVision"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["DoubleVision"].ToString()) : false;
            chk_recent_wt.Checked = ds.Tables[0].Rows[0]["RecentWeightloss"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["RecentWeightloss"].ToString()) : false;
            chk_episodic_ligth.Checked = ds.Tables[0].Rows[0]["Episodic"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Episodic"].ToString()) : false;
            chk_rashes.Checked = ds.Tables[0].Rows[0]["Rashes"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Rashes"].ToString()) : false;
            chk_hearing_loss.Checked = ds.Tables[0].Rows[0]["HearingLoss"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["HearingLoss"].ToString()) : false;
            chk_sleep_disturbance.Checked = ds.Tables[0].Rows[0]["NightSweats"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["NightSweats"].ToString()) : false;
            chk_depression.Checked = ds.Tables[0].Rows[0]["Depression"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Depression"].ToString()) : false;

            chk_bloodinurine.Checked = ds.Tables[0].Rows[0]["dloodinurine"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["dloodinurine"].ToString()) : false;

            //LTricepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRtricepsLeft"]);
            //RTricepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRtricepsRight"]);
            //LBicepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBicepsLeft"]);
            //RBicepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBicepsRight"]);
            //RBrachioradialis.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBrachioRight"]);
            //LBrachioradialis.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBrachioLeft"]);
            //LKnee.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRKneeLeft"]);
            //RKnee.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRKneeRight"]);
            //LAnkle.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRAnkleLeft"]);
            //RAnkle.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRAnkleRight"]);
            //UExchk.Checked = ds.Tables[0].Rows[0]["UEdtr"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEdtr"].ToString()) : false;
            //chkPinPrick.Checked = ds.Tables[0].Rows[0]["Pinprick"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Pinprick"].ToString()) : false;
            //chkLighttouch.Checked = ds.Tables[0].Rows[0]["Lighttouch"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Lighttouch"].ToString()) : false;
            //txtSensory.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sensory"]);
            //LESen_Click.Checked = ds.Tables[0].Rows[0]["LEsen"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEsen"].ToString()) : false;
            //TextBox4.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL3Right"]);
            //txtDMTL3.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL3Left"]);
            //TextBox6.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL4Right"]);
            //TextBox5.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL4Left"]);
            //TextBox8.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL5Right"]);
            //TextBox7.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL5Left"]);
            //TextBox10.Text = Convert.ToString(ds.Tables[0].Rows[0]["LES1Left"]);
            //TextBox21.Text = Convert.ToString(ds.Tables[0].Rows[0]["LES1Right"]);
            //TextBox25.Text = Convert.ToString(ds.Tables[0].Rows[0]["LELumberParaspinalsRight"]);
            //TextBox24.Text = Convert.ToString(ds.Tables[0].Rows[0]["LELumberParaspinalsLeft"]);
            //UESen_Click.Checked = ds.Tables[0].Rows[0]["UEsen"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEsen"].ToString()) : false;
            //TextBox9.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC5Left"]);
            //txtUEC5Right.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC5Right"]);
            //TextBox11.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC6Left"]);
            //TextBox12.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC6Right"]);
            //TextBox13.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC7Left"]);
            //TextBox14.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC7Right"]);
            //TextBox15.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC8Left"]);
            //TextBox16.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC8Right"]);
            //TextBox18.Text = Convert.ToString(ds.Tables[0].Rows[0]["UET1Right"]);
            //TextBox17.Text = Convert.ToString(ds.Tables[0].Rows[0]["UET1Left"]);
            //TextBox20.Text = Convert.ToString(ds.Tables[0].Rows[0]["UECervicalParaspinalsRight"]);
            //TextBox19.Text = Convert.ToString(ds.Tables[0].Rows[0]["UECervicalParaspinalsLeft"]);
            //cboHoffmanexam.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["HoffmanExam"]);
            //chkStocking.Checked = ds.Tables[0].Rows[0]["Stocking"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Stocking"].ToString()) : false;
            //chkGlove.Checked = ds.Tables[0].Rows[0]["Glove"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Glove"].ToString()) : false;
            //LEmmst.Checked = ds.Tables[0].Rows[0]["LEmmst"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEmmst"].ToString()) : false;
            //TextBox23.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipFlexionRight"]);
            //TextBox22.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipFlexionLeft"]);
            //TextBox41.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipAbductionRight"]);
            //TextBox40.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipAbductionLeft"]);
            //TextBox27.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeExtensionRight"]);
            //TextBox26.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeExtensionLeft"]);
            //TextBox43.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeFlexionRight"]);
            //TextBox42.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeFlexionLeft"]);
            //TextBox29.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleDorsiRight"]);
            //TextBox28.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleDorsiLeft"]);
            //TextBox45.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnklePlantarRight"]);
            //TextBox44.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnklePlantarLeft"]);
            //TextBox47.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleExtensorRight"]);
            //TextBox46.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleExtensorLeft"]);
            //UEmmst.Checked = ds.Tables[0].Rows[0]["UEmmst"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEmmst"].ToString()) : false;
            //TextBox31.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderAbductionRight"]);
            //TextBox30.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderAbductionLeft"]);
            //TextBox49.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderFlexionRight"]);
            //TextBox48.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderFlexionLeft"]);
            //TextBox33.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowExtensionRight"]);
            //TextBox32.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowExtensionLeft"]);
            //TextBox51.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowFlexionRight"]);
            //TextBox50.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowFlexionLeft"]);
            //TextBox53.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowSupinationRight"]);
            //TextBox52.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowSupinationLeft"]);
            //TextBox55.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowPronationRight"]);
            //TextBox54.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowPronationLeft"]);
            //TextBox37.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristFlexionRight"]);
            //TextBox36.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristFlexionLeft"]);
            //TextBox57.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristExtensionRight"]);
            //TextBox56.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristExtensionLeft"]);
            //TextBox39.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandGripStrengthRight"]);
            //TextBox38.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandGripStrengthLeft"]);
            //TextBox59.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandFingerAbductorsRight"]);
            //TextBox58.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandFingerAbductorsLeft"]);
            //txtIntactExcept.Text = Convert.ToString(ds.Tables[0].Rows[0]["intactexcept"]);
            //LEdtr.Checked = ds.Tables[0].Rows[0]["LEdtr"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEdtr"].ToString()) : false;
        }


        query = "select * from tblPage2HTMLContent where  PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {

            divdegreeHTML.InnerHtml = ds.Tables[0].Rows[0]["degreeSectionHTML"].ToString();
            divtopHTML.InnerHtml = ds.Tables[0].Rows[0]["topSectionHTML"].ToString();
            divrosHTML.InnerHtml = ds.Tables[0].Rows[0]["rosSectionHTML"].ToString();
        }
        else
            bindHtml();

        query = "select FreeForm from tblPatientIE  where PatientIE_ID=" + Session["PatientIE_ID"].ToString();

        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string freeForm = Convert.ToString(ds.Tables[0].Rows[0]["FreeForm"]);
            //string WorkStatusComments = Convert.ToString(ds.Tables[0].Rows[0]["WorkStatusComments"]);
            //if (!string.IsNullOrEmpty(WorkStatusComments))
            //{
            //    workStatusCmnts.Text = WorkStatusComments.ToString();
            //}
            if (!string.IsNullOrEmpty(freeForm))
            {
                string[] freeFormDetails = freeForm.Split(';');
                for (int i = 0; i < freeFormDetails.Length; i++)
                {
                    if (freeFormDetails[i].Length > 0)
                    {
                        string title = freeFormDetails[i].Split(':')[0].Trim();
                        if (title == "Restrictions")
                        {
                            string details = freeFormDetails[i].Split(':')[1].Trim();
                            string[] restrictionValues = details.Split(',');
                            int cblIndex = 0;
                            for (int j = 0; j < restrictionValues.Length; j++)
                            {
                                for (cblIndex = j; cblIndex < cblRestictions.Items.Count; cblIndex++)
                                {


                                    if (cblRestictions.Items[cblIndex].Value.ToLower() == restrictionValues[j].ToLower())
                                    {
                                        cblRestictions.Items[cblIndex].Selected = true;
                                    }
                                }
                            }
                        }
                        else if (title == "Others")
                        {
                            txtOtherRestrictions.Text = freeFormDetails[i].Split(':')[1].Trim();
                        }
                        else if (title == "Degree of Disability")
                        {
                            rblDOD.SelectedValue = freeFormDetails[i].Split(':')[1].Trim();
                        }
                        else if (title == "Work Status")
                        {
                            string details = freeFormDetails[i].Split(':')[1].Trim();
                            string[] workStatusValues = details.Split(',');
                            for (int j = 0; j < workStatusValues.Length; j++)
                            {
                                //for (int cblIndex = 0; cblIndex < cblWorkStatus; cblIndex++)
                                //{
                                //    if (cblWorkStatus.Items[cblIndex].Value == workStatusValues[j])
                                //    {
                                //        cblWorkStatus.Items[cblIndex].Selected = true;
                                //    }
                                //}
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
        }

        query = "select top 1 * from tblPatientIEDetailPage1 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            chk_headaches.Checked = ds.Tables[0].Rows[0]["HeadechesAssociated"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["HeadechesAssociated"].ToString()) : false;
            chk_nausea.Checked = ds.Tables[0].Rows[0]["Nausea"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Nausea"].ToString()) : false;
            chk_dizziness.Checked = ds.Tables[0].Rows[0]["Dizziness"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Dizziness"].ToString()) : false;
            chk_vomiting.Checked = ds.Tables[0].Rows[0]["Vomiting"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Vomiting"].ToString()) : false;
            chk_anxiety.Checked = ds.Tables[0].Rows[0]["SevereAnxiety"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["SevereAnxiety"].ToString()) : false;
        }

        query = "select top 1 * from tblbpNeck where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            chk_tingling_in_arms.Checked = ds.Tables[0].Rows[0]["Tingling"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Tingling"].ToString()) : false;
            chk_pain_radiating_shoulder.Checked = ds.Tables[0].Rows[0]["ShoulderBilateral1"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ShoulderBilateral1"].ToString()) : false;
            chk_numbness_in_arm.Checked = ds.Tables[0].Rows[0]["ArmBilateral2"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ArmBilateral2"].ToString()) : false;
            if (ds.Tables[0].Rows[0]["WeeknessIn"] != DBNull.Value && ds.Tables[0].Rows[0]["WeeknessIn"].ToString() != "")
                chk_weakness_in_arm.Checked = true;
            else
                chk_weakness_in_arm.Checked = false;
        }

        query = "select top 1 * from tblbpLowBack where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            chk_tingling_in_legs.Checked = ds.Tables[0].Rows[0]["Tingling"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Tingling"].ToString()) : false;
            chk_pain_radiating_leg.Checked = ds.Tables[0].Rows[0]["LegBilateral1"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LegBilateral1"].ToString()) : false;
            chk_numbess_in_leg.Checked = ds.Tables[0].Rows[0]["LegBilateral2"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LegBilateral2"].ToString()) : false;
            if (ds.Tables[0].Rows[0]["WeeknessIn"] != DBNull.Value && ds.Tables[0].Rows[0]["WeeknessIn"].ToString() != "")
                chk_weakness_in_leg.Checked = true;
            else
                chk_weakness_in_leg.Checked = false;
        }
    }


    protected string GetFreeForm()
    {
        string restrictions = string.Empty;
        foreach (ListItem s in cblRestictions.Items)
        {
            if (s.Selected)
            {
                restrictions += s.Value.ToLower() + ",";
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
                // this.SaveData(degree, collageName);
            }
        }
        //foreach (ListItem s in cblWorkStatus.Items)
        //{
        //    if (s.Selected)
        //    {
        //        workStatus += s.Value.ToLower() + ", ";
        //    }
        //}

        string FreeForm = string.Empty;
        if (rblDOD.SelectedIndex > -1)
        {
            FreeForm = "Degree of Disability: " + rblDOD.SelectedValue;
        }
        if (!string.IsNullOrEmpty(restrictions))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += ";";
            }
            FreeForm += "Restrictions: " + restrictions.TrimEnd(',');
        }
        if (!string.IsNullOrEmpty(txtOtherRestrictions.Text.Trim()))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += ";";
            }
            FreeForm += "Others: " + txtOtherRestrictions.Text.Trim();
        }
        if (!string.IsNullOrEmpty(workStatus))
        {
            if (!string.IsNullOrEmpty(FreeForm))
            {
                FreeForm += ";";
            }
            FreeForm += "Work Status: " + workStatus.TrimEnd(',');
        }

        return (!string.IsNullOrEmpty(FreeForm)) ? FreeForm : "None";
    }

    protected void txtDMTL3_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "MenuHighlight();", true);
    }
    protected void TextBox4_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "MenuHighlight();", true);
    }
    protected void TextBox5_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "MenuHighlight();", true);
    }
    protected void TextBox6_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox7_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox8_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox10_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox21_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox24_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox25_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox9_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC5Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox11_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox12_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox13_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox14_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox15_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox16_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox17_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox18_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox19_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void TextBox20_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }

    private void Settextboxvalue(object sender)
    {
        TextBox txtcurrent = (TextBox)(sender);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Xml/HSMData.xml"));
        XmlNodeList nodeList;
        nodeList = xmlDoc.DocumentElement.SelectNodes("/HSM/NeurologicalExams");
        foreach (XmlNode node in nodeList)
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlNode nodevalue = node.SelectSingleNode("NeurologicalExam");
            if (nodevalue.Attributes["ikey"].Value.ToString().Equals(txtcurrent.Text))
            {
                txtcurrent.Text = nodevalue.Attributes["name"].Value.ToString();
            }
        }
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "MenuHighlight();", true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { enableMenu(); });", true);
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "enableMenu()", true);
    }

    private void bindPage1values()
    {
        DBHelperClass db = new DBHelperClass();
        string query = "select top 1 HeadechesAssociated,Nausea,Dizziness,Vomiting,SevereAnxiety from tblPatientIEDetailPage1 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        DataSet ds = db.selectData(query);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            chk_headaches.Checked = ds.Tables[0].Rows[0]["HeadechesAssociated"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["HeadechesAssociated"].ToString()) : false;
            chk_nausea.Checked = ds.Tables[0].Rows[0]["Nausea"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Nausea"].ToString()) : false;
            chk_dizziness.Checked = ds.Tables[0].Rows[0]["Dizziness"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Dizziness"].ToString()) : false;
            chk_vomiting.Checked = ds.Tables[0].Rows[0]["Vomiting"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Vomiting"].ToString()) : false;
            chk_anxiety.Checked = ds.Tables[0].Rows[0]["SevereAnxiety"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["SevereAnxiety"].ToString()) : false;
        }

    }

    private void bindChkValues(string strVal)
    {
        if (!string.IsNullOrEmpty(strVal))
        {
            foreach (Control child in pnlCheckbox.Controls)
            {
                if (child is CheckBox)
                {
                    CheckBox chk = child as CheckBox;
                    if (chk.Text.Contains(','))
                    {
                        chk.Text = chk.Text.Replace(',', ' ');
                    }
                    if (strVal.Contains(chk.Text.TrimStart().ToLower()))
                    {
                        chk.Checked = true;
                    }
                }
            }
        }
    }

    protected void A_Fall_History_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Answering_The_Door_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Appliances_Laundry_Appliances_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Bending_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Carrying_Large_Objects_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Carrying_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Cleaning_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Cognitive_Impairment_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Decrease_In_Sensitivity_To_Heat_Pain_Pressure_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Diminished_Sense_Of_Touch_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Doing_Laundry_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Driving_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Emptying_The_Mailbox_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Getting_Dressed_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Getting_In_And_Out_Of_The_Home_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Getting_In_And_Out_Of_Bed_Chairs_Sofas_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Hearing_Problems_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Holding_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Household_Chores_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Incontinence_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Kneeling_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Lack_Of_Coordination_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Lifting_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Lifting_Heavy_Or_Bulky_Objects__CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Limited_Reach_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Moving_About_In_Individual_Rooms_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Moving_From_One_Room_To_Another_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Opening_Closing_Or_Locking_Windows_And_Doors_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Operating_Light_Switches_Faucets_Kitchen_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Physical_Weakness_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Playing_With_Children_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Poor_Grip_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Poor_Vision_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Poor_Balance_Gait_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Preparing_Meals_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Pulling_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Pushing_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Reaching_Items_In_Closets_And_Cabinets_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Reduced_Mobility_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Sex_Sexual_Dysfunction_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Sitting_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Socializing_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Sports_Activities_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Standing_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Stooping_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Twisting_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Use_Of_Cane_Walker_Wheelchair_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Using_The_Stairs_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Using_The_Bathtub_Or_Shower_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Using_The_Kitchen_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Using_The_Toilet_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Using_The_Telephone_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Walking_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }

    protected void Working_CheckedChanged(object sender, EventArgs e)
    {
        constructstring();
    }
    protected void lbtnProcedureDetails_Click(object sender, EventArgs e)
    {
        if (Session["PatientIE_ID"] != null)
        {
            Response.Redirect("~/TimeSheet.aspx?PId=" + Convert.ToString(Session["PatientIE_ID"]));
        }
    }
    private void constructstring()
    {
        string text = string.Empty;
        text = "The patient complains of difficulty with,";

        if (A_Fall_History.Checked)
        { text += A_Fall_History.Text + ","; }
        if (Answering_The_Door.Checked)
        { text += Answering_The_Door.Text + ","; }
        if (Appliances_Laundry_Appliances.Checked)
        { text += Appliances_Laundry_Appliances.Text + ","; }
        if (Bending.Checked)
        { text += Bending.Text + ","; }
        if (Carrying_Large_Objects.Checked)
        { text += Carrying_Large_Objects.Text + ","; }
        if (Carrying.Checked)
        { text += Carrying.Text + ","; }
        if (Cleaning.Checked)
        { text += Cleaning.Text + ","; }
        if (Cognitive_Impairment.Checked)
        { text += Cognitive_Impairment.Text + ","; }
        if (Decrease_In_Sensitivity_To_Heat_Pain_Pressure.Checked)
        { text += Decrease_In_Sensitivity_To_Heat_Pain_Pressure.Text + ","; }
        if (Diminished_Sense_Of_Touch.Checked)
        { text += Diminished_Sense_Of_Touch.Text + ","; }
        if (Doing_Laundry.Checked)
        { text += Doing_Laundry.Text + ","; }
        if (Driving.Checked)
        { text += Driving.Text + ","; }
        if (Emptying_The_Mailbox.Checked)
        { text += Emptying_The_Mailbox.Text + ","; }
        if (Getting_Dressed.Checked)
        { text += Getting_Dressed.Text + ","; }
        if (Getting_In_And_Out_Of_The_Home.Checked)
        { text += Getting_In_And_Out_Of_The_Home.Text + ","; }
        if (Getting_In_And_Out_Of_Bed_Chairs_Sofas.Checked)
        { text += Getting_In_And_Out_Of_Bed_Chairs_Sofas.Text + ","; }
        if (Hearing_Problems.Checked)
        { text += Hearing_Problems.Text + ","; }
        if (Holding.Checked)
        { text += Holding.Text + ","; }
        if (Household_Chores.Checked)
        { text += Household_Chores.Text + ","; }
        if (Incontinence.Checked)
        { text += Incontinence.Text + ","; }
        if (Kneeling.Checked)
        { text += Kneeling.Text + ","; }
        if (Lack_Of_Coordination.Checked)
        { text += Lack_Of_Coordination.Text + ","; }
        if (Lifting.Checked)
        { text += Lifting.Text + ","; }
        if (Lifting_Heavy_Or_Bulky_Objects_.Checked)
        { text += Lifting_Heavy_Or_Bulky_Objects_.Text + ","; }
        if (Limited_Reach.Checked)
        { text += Limited_Reach.Text + ","; }
        if (Moving_About_In_Individual_Rooms.Checked)
        { text += Moving_About_In_Individual_Rooms.Text + ","; }
        if (Moving_From_One_Room_To_Another.Checked)
        { text += Moving_From_One_Room_To_Another.Text + ","; }
        if (Opening_Closing_Or_Locking_Windows_And_Doors.Checked)
        { text += Opening_Closing_Or_Locking_Windows_And_Doors.Text + ","; }
        if (Operating_Light_Switches_Faucets_Kitchen.Checked)
        { text += Operating_Light_Switches_Faucets_Kitchen.Text + ","; }
        if (Physical_Weakness.Checked)
        { text += Physical_Weakness.Text + ","; }
        if (Playing_With_Children.Checked)
        { text += Playing_With_Children.Text + ","; }
        if (Poor_Grip.Checked)
        { text += Poor_Grip.Text + ","; }
        if (Poor_Vision.Checked)
        { text += Poor_Vision.Text + ","; }
        if (Poor_Balance_Gait.Checked)
        { text += Poor_Balance_Gait.Text + ","; }
        if (Preparing_Meals.Checked)
        { text += Preparing_Meals.Text + ","; }
        if (Pulling.Checked)
        { text += Pulling.Text + ","; }
        if (Pushing.Checked)
        { text += Pushing.Text + ","; }
        if (Reaching_Items_In_Closets_And_Cabinets.Checked)
        { text += Reaching_Items_In_Closets_And_Cabinets.Text + ","; }
        if (Reduced_Mobility.Checked)
        { text += Reduced_Mobility.Text + ","; }
        if (Sex_Sexual_Dysfunction.Checked)
        { text += Sex_Sexual_Dysfunction.Text + ","; }
        if (Sitting.Checked)
        { text += Sitting.Text + ","; }
        if (Socializing.Checked)
        { text += Socializing.Text + ","; }
        if (Sports_Activities.Checked)
        { text += Sports_Activities.Text + ","; }
        if (Standing.Checked)
        { text += Standing.Text + ","; }
        if (Stooping.Checked)
        { text += Stooping.Text + ","; }
        if (Twisting.Checked)
        { text += Twisting.Text + ","; }
        if (Use_Of_Cane_Walker_Wheelchair.Checked)
        { text += Use_Of_Cane_Walker_Wheelchair.Text + ","; }
        if (Using_The_Stairs.Checked)
        { text += Using_The_Stairs.Text + ","; }
        if (Using_The_Bathtub_Or_Shower.Checked)
        { text += Using_The_Bathtub_Or_Shower.Text + ","; }
        if (Using_The_Kitchen.Checked)
        { text += Using_The_Kitchen.Text + ","; }
        if (Using_The_Toilet.Checked)
        { text += Using_The_Toilet.Text + ","; }
        if (Using_The_Telephone.Checked)
        { text += Using_The_Telephone.Text; }
        if (Walking.Checked)
        { text += Walking.Text + ","; }
        if (Working.Checked)
        { text += Working.Text + ","; }

        string result = text.ToLower().Replace("the patient complains of difficulty with,", "The patient complains of difficulty with");
        txt_FreeForm.Text = result.TrimEnd(',');
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "MenuHighlight();toggel();", true);
    }

    public void bindHtml()
    {
        string path = Server.MapPath("~/Template/Page2_top.html");
        string body = File.ReadAllText(path);

        divtopHTML.InnerHtml = body;

        path = Server.MapPath("~/Template/Page2_degree.html");
        body = File.ReadAllText(path);
        divdegreeHTML.InnerHtml = body;

        path = Server.MapPath("~/Template/Page2_ros.html");
        body = File.ReadAllText(path);
        divrosHTML.InnerHtml = body;

    }

}