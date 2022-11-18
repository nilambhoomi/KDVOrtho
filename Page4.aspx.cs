using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class Page4 : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();

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
                PopulateUIDefaults();
                bindData();

            }

        }
        Logger.Info(Session["uname"].ToString() + "- Visited in Page3 for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {


            DBHelperClass db = new DBHelperClass();
            string query = "";

            //query = "update  tblPatientIEDetailPage1 set FreeForm='" + txt_FreeForm.Text + "' where PatientIE_ID=" + Session["PatientIE_ID"].ToString();


            //int val = db.executeQuery(query);

            //if (val > 0)
            //{


            query = "select top 1 * from tblPatientIEDetailPage2 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
            {


                query = "insert into tblPatientIEDetailPage2 (";
                query = query + "intactexcept,LEdtr,DTRtricepsRight,DTRtricepsLeft,DTRBicepsRight,DTRBicepsLeft,DTRBrachioRight,DTRBrachioLeft,UEdtr,DTRKneeLeft,DTRKneeRight,DTRAnkleLeft,DTRAnkleRight,Sensory,PinPrick,Lighttouch,";
                query = query + "LEsen,LEL3Right,LEL3Left,LEL4Right,LEL4Left,LEL5Right,LEL5Left,LES1Left,LES1Right,LELumberParaspinalsLeft,LELumberParaspinalsRight,UEsen,UEC5Left,UEC5Right,UEC6Left,UEC6Right,UEC7Left,UEC7Right,UEC8Left,UEC8Right,UET1Right,UET1Left,UECervicalParaspinalsRight,UECervicalParaspinalsLeft,";
                query = query + "LEmmst,LEHipFlexionRight,LEHipFlexionLeft,LEHipAbductionRight,LEHipAbductionLeft,LEKneeExtensionRight,LEKneeExtensionLeft,LEKneeFlexionRight,LEKneeFlexionLeft,LEAnkleDorsiRight,LEAnkleDorsiLeft,LEAnklePlantarRight,LEAnklePlantarLeft,LEAnkleExtensorRight,LEAnkleExtensorLeft,";
                query = query + "UEmmst,UEShoulderAbductionRight,UEShoulderAbductionLeft,UEShoulderFlexionRight,UEShoulderFlexionLeft,UEElbowExtensionRight,UEElbowExtensionLeft,UEElbowFlexionRight,UEElbowFlexionLeft,UEElbowSupinationRight,UEElbowSupinationLeft,UEElbowPronationRight,UEElbowPronationLeft,UEWristFlexionRight,UEWristFlexionLeft,UEWristExtensionRight,UEWristExtensionLeft,UEHandGripStrengthRight,UEHandGripStrengthLeft,UEHandFingerAbductorsRight,UEHandFingerAbductorsLeft";
                query = query + ") values (";

                query = query + "'" + txtIntactExcept.Text + "','" + LEdtr.Checked + "','" + RTricepstxt.Text + "','" + LTricepstxt.Text + "','" + RBicepstxt.Text + "','" + LBicepstxt.Text + "','" + RBrachioradialis.Text + "','" + LBrachioradialis.Text + "','" + UExchk.Checked + "','" + LKnee.Text + "','" + RKnee.Text + "','" + LAnkle.Text + "','" + RAnkle.Text + "','" + txtSensory.Text.Replace("'", "''") + "','" + chkPinPrick.Checked + "','" + chkLighttouch.Checked + "',";
                query = query + "'" + LESen_Click.Checked + "','" + TextBox4.Text + "','" + txtDMTL3.Text + "','" + TextBox6.Text + "','" + TextBox5.Text + "','" + TextBox8.Text + "','" + TextBox7.Text + "','" + TextBox10.Text + "','" + TextBox21.Text + "','" + TextBox24.Text + "','" + TextBox25.Text + "','" + UESen_Click.Checked + "','" + TextBox9.Text + "','" + txtUEC5Right.Text + "','" + TextBox11.Text + "','" + TextBox12.Text + "','" + TextBox13.Text + "','" + TextBox14.Text + "','" + TextBox15.Text + "','" + TextBox16.Text + "','" + TextBox18.Text + "','" + TextBox17.Text + "','" + TextBox20.Text + "','" + TextBox19.Text + "',";
                query = query + "'" + LEmmst.Checked + "','" + TextBox23.Text + "','" + TextBox22.Text + "','" + TextBox41.Text + "','" + TextBox40.Text + "','" + TextBox27.Text + "','" + TextBox26.Text + "','" + TextBox43.Text + "','" + TextBox42.Text + "','" + TextBox29.Text + "','" + TextBox28.Text + "','" + TextBox45.Text + "','" + TextBox44.Text + "','" + TextBox47.Text + "','" + TextBox46.Text + "',";
                query = query + "'" + UEmmst.Checked + "','" + TextBox31.Text + "','" + TextBox30.Text + "','" + TextBox49.Text + "','" + TextBox48.Text + "','" + TextBox33.Text + "','" + TextBox32.Text + "','" + TextBox51.Text + "','" + TextBox50.Text + "','" + TextBox53.Text + "','" + TextBox52.Text + "','" + TextBox55.Text + "','" + TextBox54.Text + "','" + TextBox37.Text + "','" + TextBox36.Text + "','" + TextBox57.Text + "','" + TextBox56.Text + "','" + TextBox39.Text + "','" + TextBox38.Text + "','" + TextBox59.Text + "','" + TextBox58.Text + "')";

            }
            else
            {
                query = "update tblPatientIEDetailPage2 set ";

                query = query + "intactexcept='" + txtIntactExcept.Text + "',LEdtr = '" + LEdtr.Checked + "',DTRtricepsRight = '" + RTricepstxt.Text + "',DTRtricepsLeft ='" + LTricepstxt.Text + "',DTRBicepsRight ='" + RBicepstxt.Text + "',DTRBicepsLeft = '" + LBicepstxt.Text + "',DTRBrachioRight ='" + RBrachioradialis.Text + "',DTRBrachioLeft = '" + LBrachioradialis.Text + "' ,UEdtr = '" + UExchk.Checked + "',DTRKneeLeft = '" + LKnee.Text + "',DTRKneeRight = '" + RKnee.Text + "',DTRAnkleLeft = '" + LAnkle.Text + "' ,DTRAnkleRight  = '" + RAnkle.Text + "',Sensory = '" + txtSensory.Text.Replace("'", "''") + "',PinPrick = '" + chkPinPrick.Checked + "',Lighttouch = '" + chkLighttouch.Checked + "',";
                query = query + "LEsen = '" + LESen_Click.Checked + "',LEL3Right = '" + TextBox4.Text + "',LEL3Left = '" + txtDMTL3.Text + "',LEL4Right = '" + TextBox6.Text + "',LEL4Left = '" + TextBox5.Text + "',LEL5Right = '" + TextBox8.Text + "',LEL5Left = '" + TextBox7.Text + "',LES1Left = '" + TextBox10.Text + "',LES1Right = '" + TextBox21.Text + "',LELumberParaspinalsLeft = '" + TextBox24.Text + "',LELumberParaspinalsRight = '" + TextBox25.Text + "',UEsen ='" + UESen_Click.Checked + "',UEC5Left = '" + TextBox9.Text + "',UEC5Right = '" + txtUEC5Right.Text + "',UEC6Left = '" + TextBox11.Text + "',UEC6Right = '" + TextBox12.Text + "',UEC7Left = '" + TextBox13.Text + "',UEC7Right = '" + TextBox14.Text + "',UEC8Left = '" + TextBox15.Text + "',UEC8Right = '" + TextBox16.Text + "',UET1Right = '" + TextBox18.Text + "',UET1Left = '" + TextBox17.Text + "',UECervicalParaspinalsRight = '" + TextBox20.Text + "',UECervicalParaspinalsLeft = '" + TextBox19.Text + "',";
                query = query + "LEmmst = '" + LEmmst.Checked + "',LEHipFlexionRight = '" + TextBox23.Text + "',LEHipFlexionLeft = '" + TextBox22.Text + "',LEHipAbductionRight = '" + TextBox41.Text + "',LEHipAbductionLeft  = '" + TextBox40.Text + "',LEKneeExtensionRight = '" + TextBox27.Text + "',LEKneeExtensionLeft = '" + TextBox26.Text + "',LEKneeFlexionRight = '" + TextBox43.Text + "',LEKneeFlexionLeft = '" + TextBox42.Text + "',LEAnkleDorsiRight = '" + TextBox29.Text + "',LEAnkleDorsiLeft = '" + TextBox28.Text + "',LEAnklePlantarRight = '" + TextBox45.Text + "',LEAnklePlantarLeft = '" + TextBox44.Text + "',LEAnkleExtensorRight = '" + TextBox47.Text + "',LEAnkleExtensorLeft = '" + TextBox46.Text + "',";
                query = query + "UEmmst = '" + UEmmst.Checked + "',UEShoulderAbductionRight = '" + TextBox31.Text + "',UEShoulderAbductionLeft = '" + TextBox30.Text + "',UEShoulderFlexionRight = '" + TextBox49.Text + "',UEShoulderFlexionLeft = '" + TextBox48.Text + "',UEElbowExtensionRight = '" + TextBox33.Text + "',UEElbowExtensionLeft = '" + TextBox32.Text + "',UEElbowFlexionRight = '" + TextBox51.Text + "',UEElbowFlexionLeft = '" + TextBox50.Text + "',UEElbowSupinationRight = '" + TextBox53.Text + "',UEElbowSupinationLeft = '" + TextBox52.Text + "',UEElbowPronationRight = '" + TextBox55.Text + "',UEElbowPronationLeft = '" + TextBox54.Text + "',UEWristFlexionRight = '" + TextBox37.Text + "',UEWristFlexionLeft = '" + TextBox36.Text + "',UEWristExtensionRight = '" + TextBox57.Text + "',UEWristExtensionLeft = '" + TextBox56.Text + "',UEHandGripStrengthRight = '" + TextBox39.Text + "',UEHandGripStrengthLeft = '" + TextBox38.Text + "',UEHandFingerAbductorsRight = '" + TextBox59.Text + "',UEHandFingerAbductorsLeft = '" + TextBox58.Text + "'";
                query = query + " Where PatientIE_ID=" + Session["PatientIE_ID"].ToString() + "";
            }
            ds.Dispose();

            int val = db.executeQuery(query);

            query = "select top 1 * from tblPage3HTMLContent where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
            ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count == 0)
            {
                query = "insert into tblPage3HTMLContent(PatientIE_ID,HTMLContent,topSectionHTML)values(@PatientIE_ID,@HTMLContent,@topSectionHTML)";
            }
            else
            {
                query = "update tblPage3HTMLContent set HTMLContent=@HTMLContent,topSectionHTML=@topSectionHTML where PatientIE_ID=@PatientIE_ID";
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
                command.Parameters.AddWithValue("@HTMLContent", hdHTMLContent.Value);
                command.Parameters.AddWithValue("@topSectionHTML", hdtopHTMLContent.Value);


                connection.Open();
                var results = command.ExecuteNonQuery();
                connection.Close();
            }


            if (val > 0)
            {
                try
                {


                    long _ieID = Convert.ToInt64(Session["PatientIE_ID"].ToString());
                    string _ieMode = "";
                    string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
                    string SqlStr = "";
                    oSQLConn.ConnectionString = sProvider;
                    oSQLConn.Open();
                    SqlStr = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + _ieID;
                    SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
                    DataTable sqlTbl = new DataTable();
                    sqlAdapt.Fill(sqlTbl);
                    DataRow TblRow;

                    if (sqlTbl.Rows.Count == 0)
                        _ieMode = "New";
                    else if (sqlTbl.Rows.Count > 0)
                        _ieMode = "Update";

                    if (_ieMode == "New")
                        TblRow = sqlTbl.NewRow();
                    else if (_ieMode == "Update")
                    {
                        TblRow = sqlTbl.Rows[0];
                        TblRow.AcceptChanges();
                    }
                    else
                        TblRow = null;

                    if (_ieMode == "Update" || _ieMode == "New")
                    {
                        TblRow["PatientIE_ID"] = _ieID;
                        TblRow["GAIT"] = cboGAIT.Text.ToString();
                        TblRow["Ambulates"] = cboAmbulates.Text.ToString();
                        TblRow["Footslap"] = chkFootslap.Checked;
                        TblRow["Kneehyperextension"] = chkKneehyperextension.Checked;
                        TblRow["Unabletohealwalk"] = chkUnabletohealwalk.Checked;
                        TblRow["Unabletotoewalk"] = chkUnabletotoewalk.Checked;
                        TblRow["Other"] = txtOther.Text.ToString();

                        if (_ieMode == "New")
                        {
                            TblRow["CreatedBy"] = "Admin";
                            TblRow["CreatedDate"] = DateTime.Now;
                            sqlTbl.Rows.Add(TblRow);
                        }
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

                    oSQLConn.Close();
                }
            }


            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
            Logger.Info(Session["UserId"].ToString() + "--" + Session["uname"].ToString().Trim() + "-- Create IE - Page4 " + Session["PatientIE_ID"].ToString() + "--" + DateTime.Now);
            if (pageHDN.Value != null && pageHDN.Value != "")
            {
                Response.Redirect(pageHDN.Value.ToString());
            }
            else
            {
                Response.Redirect("Page5.aspx");
            }

        }
        catch (Exception ex)
        {


        }

    }


    protected void lbtnProcedureDetails_Click(object sender, EventArgs e)
    {
        if (Session["PatientIE_ID"] != null)
        {
            Response.Redirect("~/TimeSheet.aspx?PId=" + Convert.ToString(Session["PatientIE_ID"]));
        }
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

    private void bindData()
    {
        DBHelperClass db = new DBHelperClass();
        string query = "";

        query = "select top 1 * from tblPatientIEDetailPage2 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        DataSet ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //chk_seizures.Checked = ds.Tables[0].Rows[0]["Seizures"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Seizures"].ToString()) : false;
            //chk_chest_pain.Checked = ds.Tables[0].Rows[0]["ChestPain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ChestPain"].ToString()) : false;
            //chk_shortness_of_breath.Checked = ds.Tables[0].Rows[0]["ShortnessOfBreath"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ShortnessOfBreath"].ToString()) : false;
            //chk_jaw_pain.Checked = ds.Tables[0].Rows[0]["Jawpain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Jawpain"].ToString()) : false;
            //chk_abdominal_pain.Checked = ds.Tables[0].Rows[0]["AbdominalPain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AbdominalPain"].ToString()) : false;
            //chk_fever.Checked = ds.Tables[0].Rows[0]["Fevers"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Fevers"].ToString()) : false;
            //chk_diarrhea.Checked = ds.Tables[0].Rows[0]["Diarrhea"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Diarrhea"].ToString()) : false;
            //chk_bowel_bladder.Checked = ds.Tables[0].Rows[0]["Bowel"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Bowel"].ToString()) : false;
            //chk_blurred.Checked = ds.Tables[0].Rows[0]["DoubleVision"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["DoubleVision"].ToString()) : false;
            //chk_recent_wt.Checked = ds.Tables[0].Rows[0]["RecentWeightloss"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["RecentWeightloss"].ToString()) : false;
            //chk_episodic_ligth.Checked = ds.Tables[0].Rows[0]["Episodic"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Episodic"].ToString()) : false;
            //chk_rashes.Checked = ds.Tables[0].Rows[0]["Rashes"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Rashes"].ToString()) : false;
            //chk_hearing_loss.Checked = ds.Tables[0].Rows[0]["HearingLoss"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["HearingLoss"].ToString()) : false;
            //chk_sleep_disturbance.Checked = ds.Tables[0].Rows[0]["NightSweats"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["NightSweats"].ToString()) : false;
            //chk_depression.Checked = ds.Tables[0].Rows[0]["Depression"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Depression"].ToString()) : false;

            //chk_bloodinurine.Checked = ds.Tables[0].Rows[0]["dloodinurine"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["dloodinurine"].ToString()) : false;

            LTricepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRtricepsLeft"]);
            RTricepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRtricepsRight"]);
            LBicepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBicepsLeft"]);
            RBicepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBicepsRight"]);
            RBrachioradialis.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBrachioRight"]);
            LBrachioradialis.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBrachioLeft"]);
            LKnee.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRKneeLeft"]);
            RKnee.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRKneeRight"]);
            LAnkle.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRAnkleLeft"]);
            RAnkle.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRAnkleRight"]);
            UExchk.Checked = ds.Tables[0].Rows[0]["UEdtr"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEdtr"].ToString()) : false;
            chkPinPrick.Checked = ds.Tables[0].Rows[0]["Pinprick"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Pinprick"].ToString()) : false;
            chkLighttouch.Checked = ds.Tables[0].Rows[0]["Lighttouch"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Lighttouch"].ToString()) : false;
            txtSensory.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sensory"]);
            LESen_Click.Checked = ds.Tables[0].Rows[0]["LEsen"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEsen"].ToString()) : false;
            TextBox4.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL3Right"]);
            txtDMTL3.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL3Left"]);
            TextBox6.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL4Right"]);
            TextBox5.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL4Left"]);
            TextBox8.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL5Right"]);
            TextBox7.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL5Left"]);
            TextBox10.Text = Convert.ToString(ds.Tables[0].Rows[0]["LES1Left"]);
            TextBox21.Text = Convert.ToString(ds.Tables[0].Rows[0]["LES1Right"]);
            TextBox25.Text = Convert.ToString(ds.Tables[0].Rows[0]["LELumberParaspinalsRight"]);
            TextBox24.Text = Convert.ToString(ds.Tables[0].Rows[0]["LELumberParaspinalsLeft"]);
            UESen_Click.Checked = ds.Tables[0].Rows[0]["UEsen"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEsen"].ToString()) : false;
            TextBox9.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC5Left"]);
            txtUEC5Right.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC5Right"]);
            TextBox11.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC6Left"]);
            TextBox12.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC6Right"]);
            TextBox13.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC7Left"]);
            TextBox14.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC7Right"]);
            TextBox15.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC8Left"]);
            TextBox16.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC8Right"]);
            TextBox18.Text = Convert.ToString(ds.Tables[0].Rows[0]["UET1Right"]);
            TextBox17.Text = Convert.ToString(ds.Tables[0].Rows[0]["UET1Left"]);
            TextBox20.Text = Convert.ToString(ds.Tables[0].Rows[0]["UECervicalParaspinalsRight"]);
            TextBox19.Text = Convert.ToString(ds.Tables[0].Rows[0]["UECervicalParaspinalsLeft"]);
            //  cboHoffmanexam.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["HoffmanExam"]);
            // chkStocking.Checked = ds.Tables[0].Rows[0]["Stocking"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Stocking"].ToString()) : false;
            //chkGlove.Checked = ds.Tables[0].Rows[0]["Glove"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Glove"].ToString()) : false;
            LEmmst.Checked = ds.Tables[0].Rows[0]["LEmmst"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEmmst"].ToString()) : false;
            TextBox23.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipFlexionRight"]);
            TextBox22.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipFlexionLeft"]);
            TextBox41.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipAbductionRight"]);
            TextBox40.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipAbductionLeft"]);
            TextBox27.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeExtensionRight"]);
            TextBox26.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeExtensionLeft"]);
            TextBox43.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeFlexionRight"]);
            TextBox42.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeFlexionLeft"]);
            TextBox29.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleDorsiRight"]);
            TextBox28.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleDorsiLeft"]);
            TextBox45.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnklePlantarRight"]);
            TextBox44.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnklePlantarLeft"]);
            TextBox47.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleExtensorRight"]);
            TextBox46.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleExtensorLeft"]);
            UEmmst.Checked = ds.Tables[0].Rows[0]["UEmmst"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEmmst"].ToString()) : false;
            TextBox31.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderAbductionRight"]);
            TextBox30.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderAbductionLeft"]);
            TextBox49.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderFlexionRight"]);
            TextBox48.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderFlexionLeft"]);
            TextBox33.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowExtensionRight"]);
            TextBox32.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowExtensionLeft"]);
            TextBox51.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowFlexionRight"]);
            TextBox50.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowFlexionLeft"]);
            TextBox53.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowSupinationRight"]);
            TextBox52.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowSupinationLeft"]);
            TextBox55.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowPronationRight"]);
            TextBox54.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowPronationLeft"]);
            TextBox37.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristFlexionRight"]);
            TextBox36.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristFlexionLeft"]);
            TextBox57.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristExtensionRight"]);
            TextBox56.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristExtensionLeft"]);
            TextBox39.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandGripStrengthRight"]);
            TextBox38.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandGripStrengthLeft"]);
            TextBox59.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandFingerAbductorsRight"]);
            TextBox58.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandFingerAbductorsLeft"]);
            txtIntactExcept.Text = Convert.ToString(ds.Tables[0].Rows[0]["intactexcept"]);
            LEdtr.Checked = ds.Tables[0].Rows[0]["LEdtr"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEdtr"].ToString()) : false;
        }
        else
        {
            PopulateUIDefaults();
        }

        query = "select * from tblPage3HTMLContent where  PatientIE_ID=" + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {
            divHtml.InnerHtml = ds.Tables[0].Rows[0]["HTMLContent"].ToString();
            divtopHtml.InnerHtml = ds.Tables[0].Rows[0]["topSectionHTML"].ToString();
        }
        else
            bindHtml();

        query = "Select * from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString();
        ds = db.selectData(query);


        if (ds.Tables[0].Rows.Count > 0)
        {

            cboGAIT.Text = ds.Tables[0].Rows[0]["GAIT"].ToString().Trim();
            cboAmbulates.Text = ds.Tables[0].Rows[0]["Ambulates"].ToString().Trim();
            chkFootslap.Checked = CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Footslap"].ToString());
            chkKneehyperextension.Checked = CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Kneehyperextension"].ToString());
            chkUnabletohealwalk.Checked = CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Unabletohealwalk"].ToString());
            chkUnabletotoewalk.Checked = CommonConvert.ToBoolean(ds.Tables[0].Rows[0]["Unabletotoewalk"].ToString());
            txtOther.Text = ds.Tables[0].Rows[0]["Other"].ToString().Trim();
        }
    }

    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        //filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        //if (File.Exists(Server.MapPath(filename)))
        //{ xmlDoc.Load(Server.MapPath(filename)); }
        //else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));

        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage3");
        foreach (XmlNode node in nodeList)
        {

            if (cboGAIT.Text == "") cboGAIT.Text = node.SelectSingleNode("GAIT") == null ? cboGAIT.Text.ToString().Trim() : node.SelectSingleNode("GAIT").InnerText;
            if (cboAmbulates.Text == "") cboAmbulates.Text = node.SelectSingleNode("Ambulates") == null ? cboAmbulates.Text.ToString().Trim() : node.SelectSingleNode("Ambulates").InnerText;
            chkFootslap.Checked = node.SelectSingleNode("Footslap") == null ? chkFootslap.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Footslap").InnerText);
            chkKneehyperextension.Checked = node.SelectSingleNode("Kneehyperextension") == null ? chkKneehyperextension.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Kneehyperextension").InnerText);
            chkUnabletohealwalk.Checked = node.SelectSingleNode("Unabletohealwalk") == null ? chkUnabletohealwalk.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Unabletohealwalk").InnerText);
            chkUnabletotoewalk.Checked = node.SelectSingleNode("Unabletotoewalk") == null ? chkUnabletotoewalk.Checked : CommonConvert.ToBoolean(node.SelectSingleNode("Unabletotoewalk").InnerText);
            if (txtOther.Text == "") txtOther.Text = node.SelectSingleNode("Other") == null ? txtOther.Text.ToString().Trim() : node.SelectSingleNode("Other").InnerText;

        }

        xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage2");

        foreach (XmlNode node in nodeList)
        {
            txtIntactExcept.Text = node.SelectSingleNode("intactexcept").InnerText;
            LEdtr.Checked = node.SelectSingleNode("LEdtr") == null ? LEdtr.Checked : Convert.ToBoolean(node.SelectSingleNode("LEdtr").InnerText);

            //  chk_depression.Checked = node.SelectSingleNode("Depression") == null ? chk_depression.Checked : Convert.ToBoolean(node.SelectSingleNode("Depression").InnerText);

            LTricepstxt.Text = node.SelectSingleNode("DTRtricepsLeft").InnerText;
            RTricepstxt.Text = node.SelectSingleNode("DTRtricepsRight").InnerText;
            LBicepstxt.Text = node.SelectSingleNode("DTRBicepsLeft").InnerText;
            RBicepstxt.Text = node.SelectSingleNode("DTRBicepsRight").InnerText;
            RBrachioradialis.Text = node.SelectSingleNode("DTRBrachioRight").InnerText;
            LBrachioradialis.Text = node.SelectSingleNode("DTRBrachioLeft").InnerText;
            LKnee.Text = node.SelectSingleNode("DTRKneeLeft").InnerText;
            RKnee.Text = node.SelectSingleNode("DTRKneeRight").InnerText;
            LAnkle.Text = node.SelectSingleNode("DTRAnkleLeft").InnerText;
            RAnkle.Text = node.SelectSingleNode("DTRAnkleRight").InnerText;

            UExchk.Checked = node.SelectSingleNode("UEdtr") == null ? UExchk.Checked : Convert.ToBoolean(node.SelectSingleNode("UEdtr").InnerText);

            chkPinPrick.Checked = node.SelectSingleNode("Pinprick") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("Pinprick").InnerText);
            chkLighttouch.Checked = node.SelectSingleNode("Lighttouch") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("Lighttouch").InnerText);
            txtSensory.Text = node.SelectSingleNode("Sensory").InnerText;
            LESen_Click.Checked = node.SelectSingleNode("LEsen") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("LEsen").InnerText);
            TextBox4.Text = node.SelectSingleNode("LEL3Right").InnerText;
            txtDMTL3.Text = node.SelectSingleNode("LEL3Left").InnerText;
            TextBox6.Text = node.SelectSingleNode("LEL4Right").InnerText;
            TextBox5.Text = node.SelectSingleNode("LEL4Left").InnerText;
            TextBox8.Text = node.SelectSingleNode("LEL5Right").InnerText;
            TextBox7.Text = node.SelectSingleNode("LEL5Left").InnerText;
            TextBox10.Text = node.SelectSingleNode("LES1Left").InnerText;
            TextBox21.Text = node.SelectSingleNode("LES1Right").InnerText;
            TextBox25.Text = node.SelectSingleNode("LELumberParaspinalsRight").InnerText;
            TextBox24.Text = node.SelectSingleNode("LELumberParaspinalsLeft").InnerText;

            UESen_Click.Checked = node.SelectSingleNode("UEsen") == null ? chkPinPrick.Checked : Convert.ToBoolean(node.SelectSingleNode("UEsen").InnerText);
            TextBox9.Text = node.SelectSingleNode("UEC5Left").InnerText;
            txtUEC5Right.Text = node.SelectSingleNode("UEC5Right").InnerText;

            TextBox11.Text = node.SelectSingleNode("UEC6Left").InnerText;
            TextBox12.Text = node.SelectSingleNode("UEC6Right").InnerText;

            TextBox13.Text = node.SelectSingleNode("UEC7Left").InnerText;
            TextBox14.Text = node.SelectSingleNode("UEC7Right").InnerText;

            TextBox15.Text = node.SelectSingleNode("UEC8Left").InnerText;
            TextBox16.Text = node.SelectSingleNode("UEC8Right").InnerText;

            TextBox18.Text = node.SelectSingleNode("UET1Right").InnerText;
            TextBox17.Text = node.SelectSingleNode("UET1Left").InnerText;
            TextBox20.Text = node.SelectSingleNode("UECervicalParaspinalsRight").InnerText;
            TextBox19.Text = node.SelectSingleNode("UECervicalParaspinalsLeft").InnerText;

            //cboHoffmanexam.SelectedValue = node.SelectSingleNode("HoffmanExam").InnerText;
            //chkStocking.Checked = node.SelectSingleNode("Stocking") == null ? chkStocking.Checked : Convert.ToBoolean(node.SelectSingleNode("Stocking").InnerText);
            //chkGlove.Checked = node.SelectSingleNode("Glove") == null ? chkGlove.Checked : Convert.ToBoolean(node.SelectSingleNode("Glove").InnerText);

            LEmmst.Checked = node.SelectSingleNode("LEmmst") == null ? LEmmst.Checked : Convert.ToBoolean(node.SelectSingleNode("LEmmst").InnerText);
            TextBox23.Text = node.SelectSingleNode("LEHipFlexionRight").InnerText;
            TextBox22.Text = node.SelectSingleNode("LEHipFlexionLeft").InnerText;
            TextBox41.Text = node.SelectSingleNode("LEHipAbductionRight").InnerText;
            TextBox40.Text = node.SelectSingleNode("LEHipAbductionLeft").InnerText;
            TextBox27.Text = node.SelectSingleNode("LEKneeExtensionRight").InnerText;
            TextBox26.Text = node.SelectSingleNode("LEKneeExtensionLeft").InnerText;

            TextBox43.Text = node.SelectSingleNode("LEKneeFlexionRight").InnerText;
            TextBox42.Text = node.SelectSingleNode("LEKneeFlexionLeft").InnerText;
            TextBox29.Text = node.SelectSingleNode("LEAnkleDorsiRight").InnerText;
            TextBox28.Text = node.SelectSingleNode("LEAnkleDorsiLeft").InnerText;

            TextBox45.Text = node.SelectSingleNode("LEAnklePlantarRight").InnerText;
            TextBox44.Text = node.SelectSingleNode("LEAnklePlantarLeft").InnerText;

            TextBox47.Text = node.SelectSingleNode("LEAnkleExtensorRight").InnerText;
            TextBox46.Text = node.SelectSingleNode("LEAnkleExtensorLeft").InnerText;

            UEmmst.Checked = node.SelectSingleNode("UEmmst") == null ? UEmmst.Checked : Convert.ToBoolean(node.SelectSingleNode("UEmmst").InnerText);

            TextBox31.Text = node.SelectSingleNode("UEShoulderAbductionRight").InnerText;
            TextBox30.Text = node.SelectSingleNode("UEShoulderAbductionLeft").InnerText;

            TextBox49.Text = node.SelectSingleNode("UEShoulderFlexionRight").InnerText;
            TextBox48.Text = node.SelectSingleNode("UEShoulderFlexionLeft").InnerText;

            TextBox33.Text = node.SelectSingleNode("UEElbowExtensionRight").InnerText;
            TextBox32.Text = node.SelectSingleNode("UEElbowExtensionLeft").InnerText;

            TextBox51.Text = node.SelectSingleNode("UEElbowFlexionRight").InnerText;
            TextBox50.Text = node.SelectSingleNode("UEElbowFlexionLeft").InnerText;

            TextBox53.Text = node.SelectSingleNode("UEElbowSupinationRight").InnerText;
            TextBox52.Text = node.SelectSingleNode("UEElbowSupinationLeft").InnerText;

            TextBox55.Text = node.SelectSingleNode("UEElbowPronationRight").InnerText;
            TextBox54.Text = node.SelectSingleNode("UEElbowPronationLeft").InnerText;

            TextBox37.Text = node.SelectSingleNode("UEWristFlexionRight").InnerText;
            TextBox36.Text = node.SelectSingleNode("UEWristFlexionLeft").InnerText;
            TextBox57.Text = node.SelectSingleNode("UEWristExtensionRight").InnerText;
            TextBox56.Text = node.SelectSingleNode("UEWristExtensionLeft").InnerText;
            TextBox39.Text = node.SelectSingleNode("UEHandGripStrengthRight").InnerText;
            TextBox38.Text = node.SelectSingleNode("UEHandGripStrengthLeft").InnerText;
            TextBox59.Text = node.SelectSingleNode("UEHandFingerAbductorsRight").InnerText;
            TextBox58.Text = node.SelectSingleNode("UEHandFingerAbductorsLeft").InnerText;

        }
    }

    public void bindHtml()
    {
        string path = Server.MapPath("~/Template/Page3.html");
        string body = File.ReadAllText(path);

        divHtml.InnerHtml = body;

        path = Server.MapPath("~/Template/Page3_top.html");
        body = File.ReadAllText(path);

        divtopHtml.InnerHtml = body;

    }

}
