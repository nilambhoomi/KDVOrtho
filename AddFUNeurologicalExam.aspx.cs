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

public partial class AddFUNeurologicalExam : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    DBHelperClass db = new DBHelperClass();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurFUid = "";
    public string _CurBP = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {
            if (Session["patientFUId"] == null || Session["patientFUId"] == "")
            {
                Response.Redirect("AddFu.aspx");

            }
            else
            {
                bindHTML();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblPatientIEDetailPage2 WHERE PatientIE_ID= " + Session["PatientIE_Id2"].ToString() + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                string queryFu = ("select count(*) as count1 FROM tblFUNeurologicalExam WHERE PatientFU_ID= " + Session["PatientFuId"].ToString() + "");
                SqlCommand Fucm = new SqlCommand(queryFu, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(Fucm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataSet Fuds = new DataSet();
                Fuda.Fill(Fuds);
                cn.Close();
                DataRow rw = ds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                DataRow Furw = Fuds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                if (Furw == null)
                {
                    // PopulateUI(Session["PatientIE_Id2"].ToString());
                    // DefaultValue();
                    PopulateUI(Session["PatientIE_Id2"].ToString(), Session["patientFUId"].ToString(), "Open");
                }
                else if (rw == null)
                {
                    PopulateIE(Session["PatientIE_Id2"].ToString(), "Open");
                }
                else
                {
                    DefaultValue();
                }


            }
            // DataTable dt = new DataTable();
            // dt.Columns.AddRange(new DataColumn[1] { new DataColumn("WorkStatus", typeof(string)) });
            // dt.Rows.Add("Able to go back to work");
            // dt.Rows.Add("Working");
            // dt.Rows.Add("Not Working");
            // dt.Rows.Add("Partially Working");
            //// Repeater1.DataSource = dt;
            // Repeater1.DataBind();



        }

        Logger.Info(Session["uname"].ToString() + "- Visited in AddFUNeurologicalExam for -" + Convert.ToString(Session["LastNameFU"]) + Convert.ToString(Session["FirstNameFU"]) + "-" + DateTime.Now);
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
    public string SaveUI(string ieID, string fuID, string fuMode)
    {
        string _fuMode = "";
        long _fuID = Convert.ToInt64(fuID);

        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUNeurologicalExam WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0)
            _fuMode = "New";
        else
            _fuMode = "Update";

        if (_fuMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_fuMode == "Update")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_fuMode == "Update" || _fuMode == "New")
        {
            TblRow["PatientFU_ID"] = _fuID;
            TblRow["Intactexcept"] = txtIntactExcept.Text.ToString();
            TblRow["UEdtr"] = (bool)UEdtr.Checked;
            TblRow["LEdtr"] = (bool)LEdtr.Checked;
            TblRow["DTRtricepsRight"] = txtDTRtricepsRight.Text.ToString();
            TblRow["DTRtricepsLeft"] = txtDTRtricepsLeft.Text.ToString();
            TblRow["DTRBicepsRight"] = txtDTRBicepsRight.Text.ToString();
            TblRow["DTRBicepsLeft"] = txtDTRBicepsLeft.Text.ToString();
            TblRow["DTRKneeRight"] = txtDTRKneeRight.Text.ToString();
            TblRow["DTRKneeLeft"] = txtDTRKneeLeft.Text.ToString();
            TblRow["DTRBrachioRight"] = txtDTRBrachioRight.Text.ToString();
            TblRow["DTRBrachioLeft"] = txtDTRBrachioLeft.Text.ToString();
            TblRow["DTRAnkleRight"] = txtDTRAnkleRight.Text.ToString();
            TblRow["DTRAnkleLeft"] = txtDTRAnkleLeft.Text.ToString();
            TblRow["Sensory"] = txtSensory.Text.ToString();
            TblRow["Pinprick"] = chkPinPrick.Checked;
            //    TblRow["Lighttouch"] = chkLighttouch.Checked;
            TblRow["UEsen"] = (bool)UESen.Checked;
            TblRow["LEsen"] = (bool)LESen.Checked;
            TblRow["UEC5Right"] = txtUEC5Right.Text.ToString();
            TblRow["UEC5Left"] = txtUEC5Left.Text.ToString();
            TblRow["UEC6Right"] = txtUEC6Right.Text.ToString();
            TblRow["UEC6Left"] = txtUEC6Left.Text.ToString();
            TblRow["UEC7Right"] = txtUEC7Right.Text.ToString();
            TblRow["UEC7Left"] = txtUEC7Left.Text.ToString();
            TblRow["UEC8Right"] = txtUEC8Right.Text.ToString();
            TblRow["UEC8Left"] = txtUEC8Left.Text.ToString();
            TblRow["UET1Right"] = txtUET1Right.Text.ToString();
            TblRow["UET1Left"] = txtUET1Left.Text.ToString();
            TblRow["UECervicalParaspinalsRight"] = txtUECervicalParaspinalsRight.Text.ToString();
            TblRow["UECervicalParaspinalsLeft"] = txtUECervicalParaspinalsLeft.Text.ToString();
            TblRow["LES1Left"] = txtLES1Left.Text.ToString();
            TblRow["LES1Right"] = txtLES1Right.Text.ToString();
            TblRow["LEL3Right"] = txtLEL3Right.Text.ToString();
            TblRow["LEL3Left"] = txtLEL3Left.Text.ToString();
            TblRow["LEL4Right"] = txtLEL4Right.Text.ToString();
            TblRow["LEL4Left"] = txtLEL4Left.Text.ToString();
            TblRow["LEL5Right"] = txtLEL5Right.Text.ToString();
            TblRow["LEL5Left"] = txtLEL5Left.Text.ToString();
            TblRow["LELumberParaspinalsRight"] = txtLELumberParaspinalsRight.Text.ToString();
            TblRow["LELumberParaspinalsLeft"] = txtLELumberParaspinalsLeft.Text.ToString();
            TblRow["HoffmanExam"] = cboHoffmanexam.Text.ToString();
            TblRow["Stocking"] = chkStocking.Checked;
            TblRow["Glove"] = chkGlove.Checked;
            TblRow["UEmmst"] = (bool)UEmmst.Checked;
            TblRow["LEmmst"] = (bool)LEmmst.Checked;
            TblRow["UEShoulderAbductionRight"] = txtUEShoulderAbductionRight.Text.ToString();
            TblRow["UEShoulderAbductionLeft"] = txtUEShoulderAbductionLeft.Text.ToString();
            TblRow["UEShoulderFlexionRight"] = txtUEShoulderFlexionRight.Text.ToString();
            TblRow["UEShoulderFlexionLeft"] = txtUEShoulderFlexionLeft.Text.ToString();
            TblRow["UEElbowExtensionRight"] = txtUEElbowExtensionRight.Text.ToString();
            TblRow["UEElbowExtensionLeft"] = txtUEElbowExtensionLeft.Text.ToString();
            TblRow["UEElbowFlexionRight"] = txtUEElbowFlexionRight.Text.ToString();
            TblRow["UEElbowFlexionLeft"] = txtUEElbowFlexionLeft.Text.ToString();
            TblRow["UEElbowSupinationRight"] = txtUEElbowSupinationRight.Text.ToString();
            TblRow["UEElbowSupinationLeft"] = txtUEElbowSupinationLeft.Text.ToString();
            TblRow["UEElbowPronationRight"] = txtUEElbowPronationRight.Text.ToString();
            TblRow["UEElbowPronationLeft"] = txtUEElbowPronationLeft.Text.ToString();
            TblRow["UEWristFlexionRight"] = txtUEWristFlexionRight.Text.ToString();
            TblRow["UEWristFlexionLeft"] = txtUEWristFlexionLeft.Text.ToString();
            TblRow["UEWristExtensionRight"] = txtUEWristExtensionRight.Text.ToString();
            TblRow["UEWristExtensionLeft"] = txtUEWristExtensionLeft.Text.ToString();
            TblRow["UEHandGripStrengthRight"] = txtUEHandGripStrengthRight.Text.ToString();
            TblRow["UEHandGripStrengthLeft"] = txtUEHandGripStrengthLeft.Text.ToString();
            TblRow["UEHandFingerAbductorsRight"] = txtUEHandFingerAbductorsRight.Text.ToString();
            TblRow["UEHandFingerAbductorsLeft"] = txtUEHandFingerAbductorsLeft.Text.ToString();
            TblRow["LEHipFlexionRight"] = txtLEHipFlexionRight.Text.ToString();
            TblRow["LEHipFlexionLeft"] = txtLEHipFlexionLeft.Text.ToString();
            TblRow["LEHipAbductionRight"] = txtLEHipAbductionRight.Text.ToString();
            TblRow["LEHipAbductionLeft"] = txtLEHipAbductionLeft.Text.ToString();
            TblRow["LEKneeExtensionRight"] = txtLEKneeExtensionRight.Text.ToString();
            TblRow["LEKneeExtensionLeft"] = txtLEKneeExtensionLeft.Text.ToString();
            TblRow["LEKneeFlexionRight"] = txtLEKneeFlexionRight.Text.ToString();
            TblRow["LEKneeFlexionLeft"] = txtLEKneeFlexionLeft.Text.ToString();
            TblRow["LEAnkleDorsiRight"] = txtLEAnkleDorsiRight.Text.ToString();
            TblRow["LEAnkleDorsiLeft"] = txtLEAnkleDorsiLeft.Text.ToString();
            TblRow["LEAnklePlantarRight"] = txtLEAnklePlantarRight.Text.ToString();
            TblRow["LEAnklePlantarLeft"] = txtLEAnklePlantarLeft.Text.ToString();
            TblRow["LEAnkleExtensorRight"] = txtLEAnkleExtensorRight.Text.ToString();
            TblRow["LEAnkleExtensorLeft"] = txtLEAnkleExtensorLeft.Text.ToString();
        }

        if (_fuMode == "New")
        {
            TblRow["CreatedBy"] = "Admin";
            TblRow["CreatedDate"] = DateTime.Now;
            sqlTbl.Rows.Add(TblRow);
        }

        sqlAdapt.Update(sqlTbl);
        TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

        if (_fuMode == "New")
            return "Neurological exam has been added...";
        else
            return "Neurological exam has been updated...";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveUI(Session["PatientIE_ID"].ToString(), Session["PatientFUID"].ToString(), "Update");
        saveHTML();
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }
    //    DBHelperClass db = new DBHelperClass();
    //    string query = "";

    //    query = "select top 1 * from tblPatientIEDetailPage2 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    DataSet ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count == 0)
    //    {


    //        query = "insert into tblPatientIEDetailPage2 (Seizures,ChestPain,ShortnessOfBreath,Jawpain,AbdominalPain,Fevers,Diarrhea,";
    //        query = query + "intactexcept,LEdtr,DTRtricepsRight,DTRtricepsLeft,DTRBicepsRight,DTRBicepsLeft,DTRBrachioRight,DTRBrachioLeft,UEdtr,DTRKneeLeft,DTRKneeRight,DTRAnkleLeft,DTRAnkleRight,Sensory,PinPrick,";//Lighttouch,
    //        query = query + "LEsen,LEL3Right,LEL3Left,LEL4Right,LEL4Left,LEL5Right,LEL5Left,LES1Left,LES1Right,LELumberParaspinalsRight,LELumberParaspinalsLeft,UEsen,UEC5Left,UEC5Right,UEC6Left,UEC6Right,UEC7Left,UEC7Right,UEC8Left,UEC8Right,UET1Right,UET1Left,UECervicalParaspinalsRight,UECervicalParaspinalsLeft,HoffmanExam,";
    //        query = query + "Stocking,Glove,LEmmst,LEHipFlexionRight,LEHipFlexionLeft,LEHipAbductionRight,LEHipAbductionLeft,LEKneeExtensionRight,LEKneeExtensionLeft,LEKneeFlexionRight,LEKneeFlexionLeft,LEAnkleDorsiRight,LEAnkleDorsiLeft,LEAnklePlantarRight,LEAnklePlantarLeft,LEAnkleExtensorRight,LEAnkleExtensorLeft,";
    //        query = query + "UEmmst,UEShoulderAbductionRight,UEShoulderAbductionLeft,UEShoulderFlexionRight,UEShoulderFlexionLeft,UEElbowExtensionRight,UEElbowExtensionLeft,UEElbowFlexionRight,UEElbowFlexionLeft,UEElbowSupinationRight,UEElbowSupinationLeft,UEElbowPronationRight,UEElbowPronationLeft,UEWristFlexionRight,UEWristFlexionLeft,UEWristExtensionRight,UEWristExtensionLeft,UEHandGripStrengthRight,UEHandGripStrengthLeft,UEHandFingerAbductorsRight,UEHandFingerAbductorsLeft,";
    //        query = query + "Bowel,RecentWeightloss,Episodic,Rashes,PatientIE_ID,NightSweats,DoubleVision,HearingLoss) values (";
    //       // query = query + "'" + chk_seizures.Checked + "','" + chk_chest_pain.Checked + "','" + chk_shortness_of_breath.Checked + "',";
    //        //query = query + "'" + chk_jaw_pain.Checked + "','" + chk_abdominal_pain.Checked + "','" + chk_fever.Checked + "','" + chk_diarrhea.Checked + "',";
    //        query = query + "'" + txtIntactExcept.Text + "','" + LEdtr.Checked + "','" + LTricepstxt.Text + "','" + RTricepstxt.Text + "','" + LBicepstxt.Text + "','" + RBicepstxt.Text + "','" + RBrachioradialis.Text + "','" + LBrachioradialis.Text + "','" + UExchk.Checked + "','" + LKnee.Text + "','" + RKnee.Text + "','" + LAnkle.Text + "','" + RAnkle.Text + "','" + txtSensory.Text + "','" + chkPinPrick.Checked + "','";//+ chkLighttouch.Checked + "',"
    //        query = query + "'" + LESen_Click.Checked + "','" + TextBox4.Text + "','" + txtDMTL3.Text + "','" + TextBox6.Text + "','" + TextBox5.Text + "','" + TextBox8.Text + "','" + TextBox7.Text + "','" + TextBox10.Text + "','" + TextBox21.Text + "','" + TextBox24.Text + "','" + TextBox25.Text + "','" + UESen_Click.Checked + "','" + TextBox9.Text + "','" + txtUEC5Right.Text + "','" + TextBox11.Text + "','" + TextBox12.Text + "','" + TextBox13.Text + "','" + TextBox14.Text + "','" + TextBox15.Text + "','" + TextBox16.Text + "','" + TextBox18.Text + "','" + TextBox17.Text + "','" + TextBox20.Text + "','" + TextBox19.Text + "','" + cboHoffmanexam.SelectedValue + "',";
    //        query = query + "'" + chkStocking.Checked + "','" + chkGlove.Checked + "','" + LEmmst.Checked + "','" + TextBox23.Text + "','" + TextBox22.Text + "','" + TextBox41.Text + "','" + TextBox40.Text + "','" + TextBox27.Text + "','" + TextBox26.Text + "','" + TextBox43.Text + "','" + TextBox42.Text + "','" + TextBox29.Text + "','" + TextBox28.Text + "','" + TextBox45.Text + "','" + TextBox44.Text + "','" + TextBox47.Text + "','" + TextBox46.Text + "',";
    //        query = query + "'" + UEmmst.Checked + "','" + TextBox31.Text + "','" + TextBox30.Text + "','" + TextBox49.Text + "','" + TextBox48.Text + "','" + TextBox33.Text + "','" + TextBox32.Text + "','" + TextBox51.Text + "','" + TextBox50.Text + "','" + TextBox53.Text + "','" + TextBox52.Text + "','" + TextBox55.Text + "','" + TextBox54.Text + "','" + TextBox37.Text + "','" + TextBox36.Text + "','" + TextBox57.Text + "','" + TextBox56.Text + "','" + TextBox39.Text + "','" + TextBox38.Text + "','" + TextBox59.Text + "','" + TextBox58.Text + "',";
    //     //   query = query + "'" + chk_bowel_bladder.Checked + "','" + chk_recent_wt.Checked + "',";
    //      //  query = query + "'" + chk_episodic_ligth.Checked + "','" + chk_rashes.Checked + "'," + Session["PatientIE_ID"].ToString() + "',";
    //        //query = query + "'" + chk_sleep_disturbance.Checked + "','" + chk_blurred.Checked + "','" + chk_hearing_loss.Checked + "')";
    //    }
    //    else
    //    {
    //        query = "update tblPatientIEDetailPage2 set Seizures='" + chk_seizures.Checked + "',";
    //        query = query + "ChestPain='" + chk_chest_pain.Checked + "',ShortnessOfBreath='" + chk_shortness_of_breath.Checked + "',";
    //        query = query + "Jawpain='" + chk_jaw_pain.Checked + "',AbdominalPain='" + chk_abdominal_pain.Checked + "',Fevers='" + chk_fever.Checked + "',Diarrhea='" + chk_diarrhea.Checked + "',";
    //        query = query + "Bowel='" + chk_bowel_bladder.Checked + "',RecentWeightloss='" + chk_recent_wt.Checked + "',";
    //        query = query + "Episodic='" + chk_episodic_ligth.Checked + "',Rashes='" + chk_rashes.Checked;
    //        query = query + "',NightSweats='" + chk_sleep_disturbance.Checked + "',DoubleVision='" + chk_blurred.Checked;
    //        query = query + "',HearingLoss='" + chk_hearing_loss.Checked;
    //        query = query + "',intactexcept = '" + txtIntactExcept.Text + "',LEdtr = '" + LEdtr.Checked + "',DTRtricepsRight = '" + LTricepstxt.Text + "',DTRtricepsLeft ='" + RTricepstxt.Text + "',DTRBicepsRight ='" + LBicepstxt.Text + "',DTRBicepsLeft = '" + RBicepstxt.Text + "',DTRBrachioRight ='" + RBrachioradialis.Text + "',DTRBrachioLeft = '" + LBrachioradialis.Text + "' ,UEdtr = '" + UExchk.Checked + "',DTRKneeLeft = '" + LKnee.Text + "',DTRKneeRight = '" + RKnee.Text + "',DTRAnkleLeft = '" + LAnkle.Text + "' ,DTRAnkleRight  = '" + RAnkle.Text + "',Sensory = '" + txtSensory.Text + "',PinPrick = '" + chkPinPrick.Checked + "',";//Lighttouch = '" + chkLighttouch.Checked + "',
    //        query = query + "LEsen = '" + LESen_Click.Checked + "',LEL3Right = '" + TextBox4.Text + "',LEL3Left = '" + txtDMTL3.Text + "',LEL4Right = '" + TextBox6.Text + "',LEL4Left = '" + TextBox5.Text + "',LEL5Right = '" + TextBox8.Text + "',LEL5Left = '" + TextBox7.Text + "',LES1Left = '" + TextBox10.Text + "',LES1Right = '" + TextBox21.Text + "',LELumberParaspinalsRight = '" + TextBox24.Text + "',LELumberParaspinalsLeft = '" + TextBox25.Text + "',UEsen ='" + UESen_Click.Checked + "',UEC5Left = '" + TextBox9.Text + "',UEC5Right = '" + txtUEC5Right.Text + "',UEC6Left = '" + TextBox11.Text + "',UEC6Right = '" + TextBox12.Text + "',UEC7Left = '" + TextBox13.Text + "',UEC7Right = '" + TextBox14.Text + "',UEC8Left = '" + TextBox15.Text + "',UEC8Right = '" + TextBox16.Text + "',UET1Right = '" + TextBox18.Text + "',UET1Left = '" + TextBox17.Text + "',UECervicalParaspinalsRight = '" + TextBox20.Text + "',UECervicalParaspinalsLeft = '" + TextBox19.Text + "',HoffmanExam = '" + cboHoffmanexam.SelectedValue + "',";
    //        query = query + "Stocking = '" + chkStocking.Checked + "',Glove = '" + chkGlove.Checked + "',LEmmst = '" + LEmmst.Checked + "',LEHipFlexionRight = '" + TextBox23.Text + "',LEHipFlexionLeft = '" + TextBox22.Text + "',LEHipAbductionRight = '" + TextBox41.Text + "',LEHipAbductionLeft  = '" + TextBox40.Text + "',LEKneeExtensionRight = '" + TextBox27.Text + "',LEKneeExtensionLeft = '" + TextBox26.Text + "',LEKneeFlexionRight = '" + TextBox43.Text + "',LEKneeFlexionLeft = '" + TextBox42.Text + "',LEAnkleDorsiRight = '" + TextBox29.Text + "',LEAnkleDorsiLeft = '" + TextBox28.Text + "',LEAnklePlantarRight = '" + TextBox45.Text + "',LEAnklePlantarLeft = '" + TextBox44.Text + "',LEAnkleExtensorRight = '" + TextBox47.Text + "',LEAnkleExtensorLeft = '" + TextBox46.Text + "',";
    //        query = query + "UEmmst = '" + UEmmst.Checked + "',UEShoulderAbductionRight = '" + TextBox31.Text + "',UEShoulderAbductionLeft = '" + TextBox30.Text + "',UEShoulderFlexionRight = '" + TextBox49.Text + "',UEShoulderFlexionLeft = '" + TextBox48.Text + "',UEElbowExtensionRight = '" + TextBox33.Text + "',UEElbowExtensionLeft = '" + TextBox32.Text + "',UEElbowFlexionRight = '" + TextBox51.Text + "',UEElbowFlexionLeft = '" + TextBox50.Text + "',UEElbowSupinationRight = '" + TextBox53.Text + "',UEElbowSupinationLeft = '" + TextBox52.Text + "',UEElbowPronationRight = '" + TextBox55.Text + "',UEElbowPronationLeft = '" + TextBox54.Text + "',UEWristFlexionRight = '" + TextBox37.Text + "',UEWristFlexionLeft = '" + TextBox36.Text + "',UEWristExtensionRight = '" + TextBox57.Text + "',UEWristExtensionLeft = '" + TextBox56.Text + "',UEHandGripStrengthRight = '" + TextBox39.Text + "',UEHandGripStrengthLeft = '" + TextBox38.Text + "',UEHandFingerAbductorsRight = '" + TextBox59.Text + "',UEHandFingerAbductorsLeft = '" + TextBox58.Text + "'";
    //        query = query + " Where PatientIE_ID=" + Session["PatientIE_ID"].ToString() + "";
    //    }
    //    ds.Dispose();

    //    int val = db.executeQuery(query);


    //    query = "update tblPatientIE set FreeForm='" + GetFreeForm() + "' where PatientIE_ID=" + Convert.ToString(Session["PatientIE_ID"]);

    //    db.executeQuery(query);

    //    query = "select top 1 * from tblPatientIEDetailPage1 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count == 0)
    //    {
    //        query = "Insert INTO tblPatientIEDetailPage1 (HeadechesAssociated,Nausea,Dizziness,Vomiting,SevereAnxiety,PatientIE_ID) Values ";
    //        query = query + "('" + chk_headaches.Checked + "','" + chk_nausea.Checked + "','" + chk_dizziness.Checked + "','" + chk_vomiting.Checked + "','";
    //        query = query + chk_anxiety.Checked + "'" + Session["PatientIE_ID"].ToString() + ")";
    //    }
    //    else
    //    {
    //        query = "Update tblPatientIEDetailPage1 SET HeadechesAssociated='" + chk_headaches.Checked + "',";
    //        query = query + " Nausea='" + chk_nausea.Checked + "',Dizziness='" + chk_dizziness.Checked + "',";
    //        query = query + "Vomiting='" + chk_vomiting.Checked + "',SevereAnxiety='" + chk_anxiety.Checked + "' Where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    }
    //    ds.Dispose();

    //    val = db.executeQuery(query);

    //    bool bpActive = false;
    //    if (chk_tingling_in_arms.Checked || chk_pain_radiating_shoulder.Checked || chk_numbness_in_arm.Checked || chk_weakness_in_arm.Checked)
    //        bpActive = true;
    //    else
    //        bpActive = false;

    //    if (bpActive)
    //    {
    //        query = "select Neck from tblInjuredBodyParts where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //        ds = db.selectData(query);
    //        if (ds.Tables[0].Rows.Count == 0)
    //            query = "Insert INTO tblInjuredBodyParts (PatientIE_ID, Neck) VALUES (" + Session["PatientIE_ID"].ToString() + ",'true')";
    //        else
    //            query = "Update tblInjuredBodyParts SET Neck='true' WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString();
    //        ds.Dispose();

    //        val = db.executeQuery(query);

    //        query = "select top 1 * from tblbpNeck where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //        ds = db.selectData(query);
    //        if (ds.Tables[0].Rows.Count == 0)
    //        {
    //            query = "Insert INTO tblbpNeck (Numbness,Tingling,ShoulderBilateral1,ArmBilateral2,WeeknessIn,PatientIE_ID) Values ";
    //            query = query + "('" + chk_tingling_in_arms.Checked + "','" + chk_tingling_in_arms.Checked + "','" + chk_pain_radiating_shoulder.Checked + "','" + chk_numbness_in_arm.Checked + "','";
    //            query = query + (chk_weakness_in_arm.Checked ? "arm." : "");
    //            query = query + "'," + Session["PatientIE_ID"].ToString() + ")";
    //        }
    //        else
    //        {
    //            query = "Update tblbpNeck SET Numbness='" + chk_tingling_in_arms.Checked + "',";
    //            query = query + " Tingling='" + chk_tingling_in_arms.Checked + "',ShoulderBilateral1='" + chk_pain_radiating_shoulder.Checked + "',";
    //            query = query + "ArmBilateral2='" + chk_numbness_in_arm.Checked + "',WeeknessIn='" + (chk_weakness_in_arm.Checked ? "arm." : "") + "' Where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //        }
    //        ds.Dispose();

    //        val = db.executeQuery(query);
    //    }

    //    if (chk_tingling_in_legs.Checked || chk_pain_radiating_leg.Checked || chk_numbess_in_leg.Checked || chk_weakness_in_leg.Checked)
    //        bpActive = true;
    //    else
    //        bpActive = false;

    //    if (bpActive)
    //    {
    //        query = "select LowBack from tblInjuredBodyParts where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //        ds = db.selectData(query);
    //        if (ds.Tables[0].Rows.Count == 0)
    //            query = "Insert INTO tblInjuredBodyParts (PatientIE_ID, LowBack) VALUES (" + Session["PatientIE_ID"].ToString() + ",'true')";
    //        else
    //            query = "Update tblInjuredBodyParts SET LowBack='true' WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString();
    //        ds.Dispose();

    //        val = db.executeQuery(query);

    //        query = "select top 1 * from tblbpLowBack where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //        ds = db.selectData(query);
    //        if (ds.Tables[0].Rows.Count == 0)
    //        {
    //            query = "Insert INTO tblbpLowBack (Numbness,Tingling,LegBilateral1,LegBilateral2,WeeknessIn,PatientIE_ID) Values ";
    //            query = query + "('" + chk_tingling_in_legs.Checked + "','" + chk_tingling_in_legs.Checked + "','" + chk_pain_radiating_leg.Checked + "','" + chk_numbess_in_leg.Checked + "','";
    //            query = query + (chk_weakness_in_leg.Checked ? "leg." : "");
    //            query = query + "'," + Session["PatientIE_ID"].ToString() + ")";
    //        }
    //        else
    //        {
    //            query = "Update tblbpLowBack SET Numbness='" + chk_tingling_in_legs.Checked + "',";
    //            query = query + " Tingling='" + chk_tingling_in_legs.Checked + "',LegBilateral1='" + chk_pain_radiating_leg.Checked + "',";
    //            query = query + "LegBilateral2='" + chk_numbess_in_leg.Checked + "',WeeknessIn='" + (chk_weakness_in_leg.Checked ? "arm." : "") + "' Where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //        }
    //        ds.Dispose();

    //        val = db.executeQuery(query);
    //    }

    //    lblMessage.InnerHtml = "Complain and Restictions Save Successfully.";
    //    lblMessage.Attributes.Add("style", "color:green");

    //    upMessage.Update();

    //    Response.Redirect("Page4.aspx");

    //    // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
    //}
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
            _fldPop = true;
            txtIntactExcept.Text = node.SelectSingleNode("Intactexcept") == null ? txtIntactExcept.Text.ToString().Trim() : node.SelectSingleNode("Intactexcept").InnerText;
            UEdtr.Checked = node.SelectSingleNode("UEdtr") == null ? UEdtr.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("UEdtr").InnerText);
            LEdtr.Checked = node.SelectSingleNode("LEdtr") == null ? LEdtr.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("LEdtr").InnerText);
            txtDTRtricepsRight.Text = node.SelectSingleNode("DTRtricepsRight") == null ? txtDTRtricepsRight.Text.ToString().Trim() : node.SelectSingleNode("DTRtricepsRight").InnerText;
            txtDTRtricepsLeft.Text = node.SelectSingleNode("DTRtricepsLeft") == null ? txtDTRtricepsLeft.Text.ToString().Trim() : node.SelectSingleNode("DTRtricepsLeft").InnerText;
            txtDTRBicepsRight.Text = node.SelectSingleNode("DTRBicepsRight") == null ? txtDTRBicepsRight.Text.ToString().Trim() : node.SelectSingleNode("DTRBicepsRight").InnerText;
            txtDTRBicepsLeft.Text = node.SelectSingleNode("DTRBicepsLeft") == null ? txtDTRBicepsLeft.Text.ToString().Trim() : node.SelectSingleNode("DTRBicepsLeft").InnerText;
            txtDTRKneeRight.Text = node.SelectSingleNode("DTRKneeRight") == null ? txtDTRKneeRight.Text.ToString().Trim() : node.SelectSingleNode("DTRKneeRight").InnerText;
            txtDTRKneeLeft.Text = node.SelectSingleNode("DTRKneeLeft") == null ? txtDTRKneeLeft.Text.ToString().Trim() : node.SelectSingleNode("DTRKneeLeft").InnerText;
            txtDTRBrachioRight.Text = node.SelectSingleNode("DTRBrachioRight") == null ? txtDTRBrachioRight.Text.ToString().Trim() : node.SelectSingleNode("DTRBrachioRight").InnerText;
            txtDTRBrachioLeft.Text = node.SelectSingleNode("DTRBrachioLeft") == null ? txtDTRBrachioLeft.Text.ToString().Trim() : node.SelectSingleNode("DTRBrachioLeft").InnerText;
            txtDTRAnkleRight.Text = node.SelectSingleNode("DTRAnkleRight") == null ? txtDTRAnkleRight.Text.ToString().Trim() : node.SelectSingleNode("DTRAnkleRight").InnerText;
            txtDTRAnkleLeft.Text = node.SelectSingleNode("DTRAnkleLeft") == null ? txtDTRAnkleLeft.Text.ToString().Trim() : node.SelectSingleNode("DTRAnkleLeft").InnerText;
            txtSensory.Text = node.SelectSingleNode("Sensory") == null ? txtSensory.Text.ToString().Trim() : node.SelectSingleNode("Sensory").InnerText;
            chkPinPrick.Checked = node.SelectSingleNode("Pinprick") == null ? chkPinPrick.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("Pinprick").InnerText);
            // chkLighttouch.Checked = node.SelectSingleNode("Lighttouch") == null ? chkLighttouch.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("Lighttouch").InnerText);
            UESen.Checked = node.SelectSingleNode("UEsen") == null ? UESen.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("UEsen").InnerText);
            LESen.Checked = node.SelectSingleNode("LEsen") == null ? LESen.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("LEsen").InnerText);
            txtUEC5Right.Text = node.SelectSingleNode("UEC5Right") == null ? txtUEC5Right.Text.ToString().Trim() : node.SelectSingleNode("UEC5Right").InnerText;
            txtUEC5Left.Text = node.SelectSingleNode("UEC5Left") == null ? txtUEC5Left.Text.ToString().Trim() : node.SelectSingleNode("UEC5Left").InnerText;
            txtUEC6Right.Text = node.SelectSingleNode("UEC6Right") == null ? txtUEC6Right.Text.ToString().Trim() : node.SelectSingleNode("UEC6Right").InnerText;
            txtUEC6Left.Text = node.SelectSingleNode("UEC6Left") == null ? txtUEC6Left.Text.ToString().Trim() : node.SelectSingleNode("UEC6Left").InnerText;
            txtUEC7Right.Text = node.SelectSingleNode("UEC7Right") == null ? txtUEC7Right.Text.ToString().Trim() : node.SelectSingleNode("UEC7Right").InnerText;
            txtUEC7Left.Text = node.SelectSingleNode("UEC7Left") == null ? txtUEC7Left.Text.ToString().Trim() : node.SelectSingleNode("UEC7Left").InnerText;
            txtUEC8Right.Text = node.SelectSingleNode("UEC8Right") == null ? txtUEC8Right.Text.ToString().Trim() : node.SelectSingleNode("UEC8Right").InnerText;
            txtUEC8Left.Text = node.SelectSingleNode("UEC8Left") == null ? txtUEC8Left.Text.ToString().Trim() : node.SelectSingleNode("UEC8Left").InnerText;
            txtUET1Right.Text = node.SelectSingleNode("UET1Right") == null ? txtUET1Right.Text.ToString().Trim() : node.SelectSingleNode("UET1Right").InnerText;
            txtUET1Left.Text = node.SelectSingleNode("UET1Left") == null ? txtUET1Left.Text.ToString().Trim() : node.SelectSingleNode("UET1Left").InnerText;
            txtUECervicalParaspinalsRight.Text = node.SelectSingleNode("UECervicalParaspinalsRight") == null ? txtUECervicalParaspinalsRight.Text.ToString().Trim() : node.SelectSingleNode("UECervicalParaspinalsRight").InnerText;
            txtUECervicalParaspinalsLeft.Text = node.SelectSingleNode("UECervicalParaspinalsLeft") == null ? txtUECervicalParaspinalsLeft.Text.ToString().Trim() : node.SelectSingleNode("UECervicalParaspinalsLeft").InnerText;
            txtLEL3Left.Text = node.SelectSingleNode("LEL3Left") == null ? txtLEL3Left.Text.ToString().Trim() : node.SelectSingleNode("LEL3Left").InnerText;
            txtLEL4Right.Text = node.SelectSingleNode("LEL4Right") == null ? txtLEL4Right.Text.ToString().Trim() : node.SelectSingleNode("LEL4Right").InnerText;
            txtLEL4Left.Text = node.SelectSingleNode("LEL4Left") == null ? txtLEL4Left.Text.ToString().Trim() : node.SelectSingleNode("LEL4Left").InnerText;
            txtLEL5Right.Text = node.SelectSingleNode("LEL5Right") == null ? txtLEL5Right.Text.ToString().Trim() : node.SelectSingleNode("LEL5Right").InnerText;
            txtLEL5Left.Text = node.SelectSingleNode("LEL5Left") == null ? txtLEL5Left.Text.ToString().Trim() : node.SelectSingleNode("LEL5Left").InnerText;
            txtLELumberParaspinalsRight.Text = node.SelectSingleNode("LELumberParaspinalsRight") == null ? txtLELumberParaspinalsRight.Text.ToString().Trim() : node.SelectSingleNode("LELumberParaspinalsRight").InnerText;
            txtLELumberParaspinalsLeft.Text = node.SelectSingleNode("LELumberParaspinalsLeft") == null ? txtLELumberParaspinalsLeft.Text.ToString().Trim() : node.SelectSingleNode("LELumberParaspinalsLeft").InnerText;
            cboHoffmanexam.Text = node.SelectSingleNode("HoffmanExam") == null ? cboHoffmanexam.Text.ToString().Trim() : node.SelectSingleNode("HoffmanExam").InnerText;
            chkStocking.Checked = node.SelectSingleNode("Stocking") == null ? chkStocking.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("Stocking").InnerText);
            chkGlove.Checked = node.SelectSingleNode("Glove") == null ? chkGlove.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("Glove").InnerText);
            UEmmst.Checked = node.SelectSingleNode("UEmmst") == null ? UEmmst.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("UEmmst").InnerText);
            LEmmst.Checked = node.SelectSingleNode("LEmmst") == null ? LEmmst.Checked : Safe.GetSafeBooleanD(node.SelectSingleNode("LEmmst").InnerText);
            txtUEShoulderAbductionRight.Text = node.SelectSingleNode("UEShoulderAbductionRight") == null ? txtUEShoulderAbductionRight.Text.ToString().Trim() : node.SelectSingleNode("UEShoulderAbductionRight").InnerText;
            txtUEShoulderAbductionLeft.Text = node.SelectSingleNode("UEShoulderAbductionLeft") == null ? txtUEShoulderAbductionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEShoulderAbductionLeft").InnerText;
            txtUEShoulderFlexionRight.Text = node.SelectSingleNode("UEShoulderFlexionRight") == null ? txtUEShoulderFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("UEShoulderFlexionRight").InnerText;
            txtUEShoulderFlexionLeft.Text = node.SelectSingleNode("UEShoulderFlexionLeft") == null ? txtUEShoulderFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEShoulderFlexionLeft").InnerText;
            txtUEElbowExtensionRight.Text = node.SelectSingleNode("UEElbowExtensionRight") == null ? txtUEElbowExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("UEElbowExtensionRight").InnerText;
            txtUEElbowExtensionLeft.Text = node.SelectSingleNode("UEElbowExtensionLeft") == null ? txtUEElbowExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEElbowExtensionLeft").InnerText;
            txtUEElbowFlexionRight.Text = node.SelectSingleNode("UEElbowFlexionRight") == null ? txtUEElbowFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("UEElbowFlexionRight").InnerText;
            txtUEElbowFlexionLeft.Text = node.SelectSingleNode("UEElbowFlexionLeft") == null ? txtUEElbowFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEElbowFlexionLeft").InnerText;
            txtUEElbowSupinationRight.Text = node.SelectSingleNode("UEElbowSupinationRight") == null ? txtUEElbowSupinationRight.Text.ToString().Trim() : node.SelectSingleNode("UEElbowSupinationRight").InnerText;
            txtUEElbowSupinationLeft.Text = node.SelectSingleNode("UEElbowSupinationLeft") == null ? txtUEElbowSupinationLeft.Text.ToString().Trim() : node.SelectSingleNode("UEElbowSupinationLeft").InnerText;
            txtUEElbowPronationRight.Text = node.SelectSingleNode("UEElbowPronationRight") == null ? txtUEElbowPronationRight.Text.ToString().Trim() : node.SelectSingleNode("UEElbowPronationRight").InnerText;
            txtUEElbowPronationLeft.Text = node.SelectSingleNode("UEElbowPronationLeft") == null ? txtUEElbowPronationLeft.Text.ToString().Trim() : node.SelectSingleNode("UEElbowPronationLeft").InnerText;
            txtUEWristExtensionRight.Text = node.SelectSingleNode("UEWristExtensionRight") == null ? txtUEWristExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("UEWristExtensionRight").InnerText;
            txtUEWristExtensionLeft.Text = node.SelectSingleNode("UEWristExtensionLeft") == null ? txtUEWristExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEWristExtensionLeft").InnerText;
            txtUEWristFlexionLeft.Text = node.SelectSingleNode("UEWristFlexionLeft") == null ? txtUEWristFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEWristFlexionLeft").InnerText;
            txtUEWristFlexionRight.Text = node.SelectSingleNode("UEWristFlexionRight") == null ? txtUEWristFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("UEWristFlexionRight").InnerText;
            txtUEWristExtensionRight.Text = node.SelectSingleNode("UEWristExtensionRight") == null ? txtUEWristExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("UEWristExtensionRight").InnerText;
            txtUEWristExtensionLeft.Text = node.SelectSingleNode("UEWristExtensionLeft") == null ? txtUEWristExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("UEWristExtensionLeft").InnerText;
            txtUEHandGripStrengthRight.Text = node.SelectSingleNode("UEHandGripStrengthRight") == null ? txtUEHandGripStrengthRight.Text.ToString().Trim() : node.SelectSingleNode("UEHandGripStrengthRight").InnerText;
            txtUEHandGripStrengthLeft.Text = node.SelectSingleNode("UEHandGripStrengthLeft") == null ? txtUEHandGripStrengthLeft.Text.ToString().Trim() : node.SelectSingleNode("UEHandGripStrengthLeft").InnerText;
            txtUEHandFingerAbductorsRight.Text = node.SelectSingleNode("UEHandFingerAbductorsRight") == null ? txtUEHandFingerAbductorsRight.Text.ToString().Trim() : node.SelectSingleNode("UEHandFingerAbductorsRight").InnerText;
            txtUEHandFingerAbductorsLeft.Text = node.SelectSingleNode("UEHandFingerAbductorsLeft") == null ? txtUEHandFingerAbductorsLeft.Text.ToString().Trim() : node.SelectSingleNode("UEHandFingerAbductorsLeft").InnerText;
            txtLEHipFlexionRight.Text = node.SelectSingleNode("LEHipFlexionRight") == null ? txtLEHipFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("LEHipFlexionRight").InnerText;
            txtLEHipFlexionLeft.Text = node.SelectSingleNode("LEHipFlexionLeft") == null ? txtLEHipFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEHipFlexionLeft").InnerText;
            txtLEHipAbductionRight.Text = node.SelectSingleNode("LEHipAbductionRight") == null ? txtLEHipAbductionRight.Text.ToString().Trim() : node.SelectSingleNode("LEHipAbductionRight").InnerText;
            txtLEHipAbductionLeft.Text = node.SelectSingleNode("LEHipAbductionLeft") == null ? txtLEHipAbductionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEHipAbductionLeft").InnerText;
            txtLEKneeExtensionRight.Text = node.SelectSingleNode("LEKneeExtensionRight") == null ? txtLEKneeExtensionRight.Text.ToString().Trim() : node.SelectSingleNode("LEKneeExtensionRight").InnerText;
            txtLEKneeExtensionLeft.Text = node.SelectSingleNode("LEKneeExtensionLeft") == null ? txtLEKneeExtensionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEKneeExtensionLeft").InnerText;
            txtLEKneeFlexionRight.Text = node.SelectSingleNode("LEKneeFlexionRight") == null ? txtLEKneeFlexionRight.Text.ToString().Trim() : node.SelectSingleNode("LEKneeFlexionRight").InnerText;
            txtLEKneeFlexionLeft.Text = node.SelectSingleNode("LEKneeFlexionLeft") == null ? txtLEKneeFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("LEKneeFlexionLeft").InnerText;
            txtLEAnkleDorsiRight.Text = node.SelectSingleNode("LEAnkleDorsiRight") == null ? txtLEAnkleDorsiRight.Text.ToString().Trim() : node.SelectSingleNode("LEAnkleDorsiRight").InnerText;
            txtLEAnkleDorsiLeft.Text = node.SelectSingleNode("LEAnkleDorsiLeft") == null ? txtLEAnkleDorsiLeft.Text.ToString().Trim() : node.SelectSingleNode("LEAnkleDorsiLeft").InnerText;
            txtLEAnklePlantarRight.Text = node.SelectSingleNode("LEAnklePlantarRight") == null ? txtLEAnklePlantarRight.Text.ToString().Trim() : node.SelectSingleNode("LEAnklePlantarRight").InnerText;
            txtLEAnklePlantarLeft.Text = node.SelectSingleNode("LEAnklePlantarLeft") == null ? txtLEAnklePlantarLeft.Text.ToString().Trim() : node.SelectSingleNode("LEAnklePlantarLeft").InnerText;
            txtLEAnkleExtensorRight.Text = node.SelectSingleNode("LEAnkleExtensorRight") == null ? txtLEAnkleExtensorRight.Text.ToString().Trim() : node.SelectSingleNode("LEAnkleExtensorRight").InnerText;
            txtLEAnkleExtensorLeft.Text = node.SelectSingleNode("LEAnkleExtensorLeft") == null ? txtLEAnkleExtensorLeft.Text.ToString().Trim() : node.SelectSingleNode("LEAnkleExtensorLeft").InnerText;
            txtLEL3Right.Text = node.SelectSingleNode("LEL3Right") == null ? txtLEL3Right.Text.ToString().Trim() : node.SelectSingleNode("LEL3Right").InnerText;
            txtLES1Right.Text = node.SelectSingleNode("LES1Right") == null ? txtLES1Right.Text.ToString().Trim() : node.SelectSingleNode("LES1Right").InnerText;
            txtLES1Left.Text = node.SelectSingleNode("LES1Left") == null ? txtLES1Left.Text.ToString().Trim() : node.SelectSingleNode("LES1Left").InnerText;
            _fldPop = false;
        }
        nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/IEPage1");
        //foreach (XmlNode node in nodeList)
        //{
        //    chk_vomiting.Checked = node.SelectSingleNode("Vomiting") == null ? chk_vomiting.Checked : Convert.ToBoolean(node.SelectSingleNode("Vomiting").InnerText);//page1
        //    chk_anxiety.Checked = node.SelectSingleNode("SevereAnxiety") == null ? chk_anxiety.Checked : Convert.ToBoolean(node.SelectSingleNode("SevereAnxiety").InnerText);//page1
        //    chk_nausea.Checked = node.SelectSingleNode("Nausea") == null ? chk_nausea.Checked : Convert.ToBoolean(node.SelectSingleNode("Nausea").InnerText);//page1
        //    chk_dizziness.Checked = node.SelectSingleNode("Dizziness") == null ? chk_dizziness.Checked : Convert.ToBoolean(node.SelectSingleNode("Dizziness").InnerText);//page1
        //}
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


    public void PopulateUI(string ieID, string fuID, string fuMode)
    {

        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUNeurologicalExam WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtIntactExcept.Text = TblRow["Intactexcept"].ToString().Trim();
            UEdtr.Checked = Safe.GetSafeBoolean(TblRow["UEdtr"]);
            LEdtr.Checked = Safe.GetSafeBoolean(TblRow["LEdtr"]);
            txtDTRtricepsRight.Text = TblRow["DTRtricepsRight"].ToString().Trim();
            txtDTRtricepsLeft.Text = TblRow["DTRtricepsLeft"].ToString().Trim();
            txtDTRBicepsRight.Text = TblRow["DTRBicepsRight"].ToString().Trim();
            txtDTRBicepsLeft.Text = TblRow["DTRBicepsLeft"].ToString().Trim();
            txtDTRKneeRight.Text = TblRow["DTRKneeRight"].ToString().Trim();
            txtDTRKneeLeft.Text = TblRow["DTRKneeLeft"].ToString().Trim();
            txtDTRBrachioRight.Text = TblRow["DTRBrachioRight"].ToString().Trim();
            txtDTRBrachioLeft.Text = TblRow["DTRBrachioLeft"].ToString().Trim();
            txtDTRAnkleRight.Text = TblRow["DTRAnkleRight"].ToString().Trim();
            txtDTRAnkleLeft.Text = TblRow["DTRAnkleLeft"].ToString().Trim();
            txtSensory.Text = TblRow["Sensory"].ToString().Trim();
            chkPinPrick.Checked = Safe.GetSafeBoolean(TblRow["Pinprick"]);
            // chkLighttouch.Checked = Safe.GetSafeBoolean(TblRow["Lighttouch"]);
            UESen.Checked = Safe.GetSafeBoolean(TblRow["UEsen"]);
            LESen.Checked = Safe.GetSafeBoolean(TblRow["LEsen"]);
            txtUEC5Right.Text = TblRow["UEC5Right"].ToString().Trim();
            txtUEC5Left.Text = TblRow["UEC5Left"].ToString().Trim();
            txtUEC6Right.Text = TblRow["UEC6Right"].ToString().Trim();
            txtUEC6Left.Text = TblRow["UEC6Left"].ToString().Trim();
            txtUEC7Right.Text = TblRow["UEC7Right"].ToString().Trim();
            txtUEC7Left.Text = TblRow["UEC7Left"].ToString().Trim();
            txtUEC8Right.Text = TblRow["UEC8Right"].ToString().Trim();
            txtUEC8Left.Text = TblRow["UEC8Left"].ToString().Trim();
            txtUET1Right.Text = TblRow["UET1Right"].ToString().Trim();
            txtUET1Left.Text = TblRow["UET1Left"].ToString().Trim();
            txtUECervicalParaspinalsRight.Text = TblRow["UECervicalParaspinalsRight"].ToString().Trim();
            txtUECervicalParaspinalsLeft.Text = TblRow["UECervicalParaspinalsLeft"].ToString().Trim();
            txtLEL3Left.Text = TblRow["LEL3Left"].ToString().Trim();
            txtLEL4Right.Text = TblRow["LEL4Right"].ToString().Trim();
            txtLEL4Left.Text = TblRow["LEL4Left"].ToString().Trim();
            txtLEL5Right.Text = TblRow["LEL5Right"].ToString().Trim();
            txtLEL5Left.Text = TblRow["LEL5Left"].ToString().Trim();
            txtLELumberParaspinalsRight.Text = TblRow["LELumberParaspinalsRight"].ToString().Trim();
            txtLELumberParaspinalsLeft.Text = TblRow["LELumberParaspinalsLeft"].ToString().Trim();
            cboHoffmanexam.Text = TblRow["HoffmanExam"].ToString().Trim();
            chkStocking.Checked = Safe.GetSafeBoolean(TblRow["Stocking"]);
            chkGlove.Checked = Safe.GetSafeBoolean(TblRow["Glove"]);
            UEmmst.Checked = Safe.GetSafeBoolean(TblRow["UEmmst"]);
            LEmmst.Checked = Safe.GetSafeBoolean(TblRow["LEmmst"]);
            txtUEShoulderAbductionRight.Text = TblRow["UEShoulderAbductionRight"].ToString().Trim();
            txtUEShoulderAbductionLeft.Text = TblRow["UEShoulderAbductionLeft"].ToString().Trim();
            txtUEShoulderFlexionRight.Text = TblRow["UEShoulderFlexionRight"].ToString().Trim();
            txtUEShoulderFlexionLeft.Text = TblRow["UEShoulderFlexionLeft"].ToString().Trim();
            txtUEElbowExtensionRight.Text = TblRow["UEElbowExtensionRight"].ToString().Trim();
            txtUEElbowExtensionLeft.Text = TblRow["UEElbowExtensionLeft"].ToString().Trim();
            txtUEElbowFlexionRight.Text = TblRow["UEElbowFlexionRight"].ToString().Trim();
            txtUEElbowFlexionLeft.Text = TblRow["UEElbowFlexionLeft"].ToString().Trim();
            txtUEElbowSupinationRight.Text = TblRow["UEElbowSupinationRight"].ToString().Trim();
            txtUEElbowSupinationLeft.Text = TblRow["UEElbowSupinationLeft"].ToString().Trim();
            txtUEElbowPronationRight.Text = TblRow["UEElbowPronationRight"].ToString().Trim();
            txtUEElbowPronationLeft.Text = TblRow["UEElbowPronationLeft"].ToString().Trim();
            txtUEWristExtensionRight.Text = TblRow["UEWristExtensionRight"].ToString().Trim();
            txtUEWristExtensionLeft.Text = TblRow["UEWristExtensionLeft"].ToString().Trim();
            txtUEWristFlexionLeft.Text = TblRow["UEWristFlexionLeft"].ToString().Trim();
            txtUEWristFlexionRight.Text = TblRow["UEWristFlexionRight"].ToString().Trim();
            txtUEWristExtensionRight.Text = TblRow["UEWristExtensionRight"].ToString().Trim();
            txtUEWristExtensionLeft.Text = TblRow["UEWristExtensionLeft"].ToString().Trim();
            txtUEHandGripStrengthRight.Text = TblRow["UEHandGripStrengthRight"].ToString().Trim();
            txtUEHandGripStrengthLeft.Text = TblRow["UEHandGripStrengthLeft"].ToString().Trim();
            txtUEHandFingerAbductorsRight.Text = TblRow["UEHandFingerAbductorsRight"].ToString().Trim();
            txtUEHandFingerAbductorsLeft.Text = TblRow["UEHandFingerAbductorsLeft"].ToString().Trim();
            txtLEHipFlexionRight.Text = TblRow["LEHipFlexionRight"].ToString().Trim();
            txtLEHipFlexionLeft.Text = TblRow["LEHipFlexionLeft"].ToString().Trim();
            txtLEHipAbductionRight.Text = TblRow["LEHipAbductionRight"].ToString().Trim();
            txtLEHipAbductionLeft.Text = TblRow["LEHipAbductionLeft"].ToString().Trim();
            txtLEKneeExtensionRight.Text = TblRow["LEKneeExtensionRight"].ToString().Trim();
            txtLEKneeExtensionLeft.Text = TblRow["LEKneeExtensionLeft"].ToString().Trim();
            txtLEKneeFlexionRight.Text = TblRow["LEKneeFlexionRight"].ToString().Trim();
            txtLEKneeFlexionLeft.Text = TblRow["LEKneeFlexionLeft"].ToString().Trim();
            txtLEAnkleDorsiRight.Text = TblRow["LEAnkleDorsiRight"].ToString().Trim();
            txtLEAnkleDorsiLeft.Text = TblRow["LEAnkleDorsiLeft"].ToString().Trim();
            txtLEAnklePlantarRight.Text = TblRow["LEAnklePlantarRight"].ToString().Trim();
            txtLEAnklePlantarLeft.Text = TblRow["LEAnklePlantarLeft"].ToString().Trim();
            txtLEAnkleExtensorRight.Text = TblRow["LEAnkleExtensorRight"].ToString().Trim();
            txtLEAnkleExtensorLeft.Text = TblRow["LEAnkleExtensorLeft"].ToString().Trim();
            txtLEL3Right.Text = TblRow["LEL3Right"].ToString().Trim();
            txtLES1Right.Text = TblRow["LES1Right"].ToString().Trim();
            txtLES1Left.Text = TblRow["LES1Left"].ToString().Trim();
            _fldPop = false;
        }
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }


    public void PopulateIE(string ieID, string fuMode)
    {

        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();


        SqlStr = "Select * from tblPatientIEDetailPage2 WHERE PatientIE_ID = " + ieID;

        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtIntactExcept.Text = TblRow["Intactexcept"].ToString().Trim();
            UEdtr.Checked = Safe.GetSafeBoolean(TblRow["UEdtr"]);
            LEdtr.Checked = Safe.GetSafeBoolean(TblRow["LEdtr"]);
            txtDTRtricepsRight.Text = TblRow["DTRtricepsRight"].ToString().Trim();
            txtDTRtricepsLeft.Text = TblRow["DTRtricepsLeft"].ToString().Trim();
            txtDTRBicepsRight.Text = TblRow["DTRBicepsRight"].ToString().Trim();
            txtDTRBicepsLeft.Text = TblRow["DTRBicepsLeft"].ToString().Trim();
            txtDTRKneeRight.Text = TblRow["DTRKneeRight"].ToString().Trim();
            txtDTRKneeLeft.Text = TblRow["DTRKneeLeft"].ToString().Trim();
            txtDTRBrachioRight.Text = TblRow["DTRBrachioRight"].ToString().Trim();
            txtDTRBrachioLeft.Text = TblRow["DTRBrachioLeft"].ToString().Trim();
            txtDTRAnkleRight.Text = TblRow["DTRAnkleRight"].ToString().Trim();
            txtDTRAnkleLeft.Text = TblRow["DTRAnkleLeft"].ToString().Trim();
            txtSensory.Text = TblRow["Sensory"].ToString().Trim();
            chkPinPrick.Checked = Safe.GetSafeBoolean(TblRow["Pinprick"]);
            // chkLighttouch.Checked = Safe.GetSafeBoolean(TblRow["Lighttouch"]);
            UESen.Checked = Safe.GetSafeBoolean(TblRow["UEsen"]);
            LESen.Checked = Safe.GetSafeBoolean(TblRow["LEsen"]);
            txtUEC5Right.Text = TblRow["UEC5Right"].ToString().Trim();
            txtUEC5Left.Text = TblRow["UEC5Left"].ToString().Trim();
            txtUEC6Right.Text = TblRow["UEC6Right"].ToString().Trim();
            txtUEC6Left.Text = TblRow["UEC6Left"].ToString().Trim();
            txtUEC7Right.Text = TblRow["UEC7Right"].ToString().Trim();
            txtUEC7Left.Text = TblRow["UEC7Left"].ToString().Trim();
            txtUEC8Right.Text = TblRow["UEC8Right"].ToString().Trim();
            txtUEC8Left.Text = TblRow["UEC8Left"].ToString().Trim();
            txtUET1Right.Text = TblRow["UET1Right"].ToString().Trim();
            txtUET1Left.Text = TblRow["UET1Left"].ToString().Trim();
            txtUECervicalParaspinalsRight.Text = TblRow["UECervicalParaspinalsRight"].ToString().Trim();
            txtUECervicalParaspinalsLeft.Text = TblRow["UECervicalParaspinalsLeft"].ToString().Trim();
            txtLEL3Left.Text = TblRow["LEL3Left"].ToString().Trim();
            txtLEL4Right.Text = TblRow["LEL4Right"].ToString().Trim();
            txtLEL4Left.Text = TblRow["LEL4Left"].ToString().Trim();
            txtLEL5Right.Text = TblRow["LEL5Right"].ToString().Trim();
            txtLEL5Left.Text = TblRow["LEL5Left"].ToString().Trim();
            txtLELumberParaspinalsRight.Text = TblRow["LELumberParaspinalsRight"].ToString().Trim();
            txtLELumberParaspinalsLeft.Text = TblRow["LELumberParaspinalsLeft"].ToString().Trim();
            cboHoffmanexam.Text = TblRow["HoffmanExam"].ToString().Trim();
            chkStocking.Checked = Safe.GetSafeBoolean(TblRow["Stocking"]);
            chkGlove.Checked = Safe.GetSafeBoolean(TblRow["Glove"]);
            UEmmst.Checked = Safe.GetSafeBoolean(TblRow["UEmmst"]);
            LEmmst.Checked = Safe.GetSafeBoolean(TblRow["LEmmst"]);
            txtUEShoulderAbductionRight.Text = TblRow["UEShoulderAbductionRight"].ToString().Trim();
            txtUEShoulderAbductionLeft.Text = TblRow["UEShoulderAbductionLeft"].ToString().Trim();
            txtUEShoulderFlexionRight.Text = TblRow["UEShoulderFlexionRight"].ToString().Trim();
            txtUEShoulderFlexionLeft.Text = TblRow["UEShoulderFlexionLeft"].ToString().Trim();
            txtUEElbowExtensionRight.Text = TblRow["UEElbowExtensionRight"].ToString().Trim();
            txtUEElbowExtensionLeft.Text = TblRow["UEElbowExtensionLeft"].ToString().Trim();
            txtUEElbowFlexionRight.Text = TblRow["UEElbowFlexionRight"].ToString().Trim();
            txtUEElbowFlexionLeft.Text = TblRow["UEElbowFlexionLeft"].ToString().Trim();
            txtUEElbowSupinationRight.Text = TblRow["UEElbowSupinationRight"].ToString().Trim();
            txtUEElbowSupinationLeft.Text = TblRow["UEElbowSupinationLeft"].ToString().Trim();
            txtUEElbowPronationRight.Text = TblRow["UEElbowPronationRight"].ToString().Trim();
            txtUEElbowPronationLeft.Text = TblRow["UEElbowPronationLeft"].ToString().Trim();
            txtUEWristExtensionRight.Text = TblRow["UEWristExtensionRight"].ToString().Trim();
            txtUEWristExtensionLeft.Text = TblRow["UEWristExtensionLeft"].ToString().Trim();
            txtUEWristFlexionLeft.Text = TblRow["UEWristFlexionLeft"].ToString().Trim();
            txtUEWristFlexionRight.Text = TblRow["UEWristFlexionRight"].ToString().Trim();
            txtUEWristExtensionRight.Text = TblRow["UEWristExtensionRight"].ToString().Trim();
            txtUEWristExtensionLeft.Text = TblRow["UEWristExtensionLeft"].ToString().Trim();
            txtUEHandGripStrengthRight.Text = TblRow["UEHandGripStrengthRight"].ToString().Trim();
            txtUEHandGripStrengthLeft.Text = TblRow["UEHandGripStrengthLeft"].ToString().Trim();
            txtUEHandFingerAbductorsRight.Text = TblRow["UEHandFingerAbductorsRight"].ToString().Trim();
            txtUEHandFingerAbductorsLeft.Text = TblRow["UEHandFingerAbductorsLeft"].ToString().Trim();
            txtLEHipFlexionRight.Text = TblRow["LEHipFlexionRight"].ToString().Trim();
            txtLEHipFlexionLeft.Text = TblRow["LEHipFlexionLeft"].ToString().Trim();
            txtLEHipAbductionRight.Text = TblRow["LEHipAbductionRight"].ToString().Trim();
            txtLEHipAbductionLeft.Text = TblRow["LEHipAbductionLeft"].ToString().Trim();
            txtLEKneeExtensionRight.Text = TblRow["LEKneeExtensionRight"].ToString().Trim();
            txtLEKneeExtensionLeft.Text = TblRow["LEKneeExtensionLeft"].ToString().Trim();
            txtLEKneeFlexionRight.Text = TblRow["LEKneeFlexionRight"].ToString().Trim();
            txtLEKneeFlexionLeft.Text = TblRow["LEKneeFlexionLeft"].ToString().Trim();
            txtLEAnkleDorsiRight.Text = TblRow["LEAnkleDorsiRight"].ToString().Trim();
            txtLEAnkleDorsiLeft.Text = TblRow["LEAnkleDorsiLeft"].ToString().Trim();
            txtLEAnklePlantarRight.Text = TblRow["LEAnklePlantarRight"].ToString().Trim();
            txtLEAnklePlantarLeft.Text = TblRow["LEAnklePlantarLeft"].ToString().Trim();
            txtLEAnkleExtensorRight.Text = TblRow["LEAnkleExtensorRight"].ToString().Trim();
            txtLEAnkleExtensorLeft.Text = TblRow["LEAnkleExtensorLeft"].ToString().Trim();
            txtLEL3Right.Text = TblRow["LEL3Right"].ToString().Trim();
            txtLES1Right.Text = TblRow["LES1Right"].ToString().Trim();
            txtLES1Left.Text = TblRow["LES1Left"].ToString().Trim();
            _fldPop = false;
        }
        else
            DefaultValue();

        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }

    //private void bindData()
    //{
    //    DBHelperClass db = new DBHelperClass();
    //    string query = "";

    //    query = "select top 1 * from tblPatientIEDetailPage2 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    DataSet ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        chk_seizures.Checked = ds.Tables[0].Rows[0]["Seizures"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Seizures"].ToString()) : false;
    //        chk_chest_pain.Checked = ds.Tables[0].Rows[0]["ChestPain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ChestPain"].ToString()) : false;
    //        chk_shortness_of_breath.Checked = ds.Tables[0].Rows[0]["ShortnessOfBreath"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ShortnessOfBreath"].ToString()) : false;
    //        chk_jaw_pain.Checked = ds.Tables[0].Rows[0]["Jawpain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Jawpain"].ToString()) : false;
    //        chk_abdominal_pain.Checked = ds.Tables[0].Rows[0]["AbdominalPain"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AbdominalPain"].ToString()) : false;
    //        chk_fever.Checked = ds.Tables[0].Rows[0]["Fevers"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Fevers"].ToString()) : false;
    //        chk_diarrhea.Checked = ds.Tables[0].Rows[0]["Diarrhea"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Diarrhea"].ToString()) : false;
    //        chk_bowel_bladder.Checked = ds.Tables[0].Rows[0]["Bowel"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Bowel"].ToString()) : false;
    //        chk_blurred.Checked = ds.Tables[0].Rows[0]["DoubleVision"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["DoubleVision"].ToString()) : false;
    //        chk_recent_wt.Checked = ds.Tables[0].Rows[0]["RecentWeightloss"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["RecentWeightloss"].ToString()) : false;
    //        chk_episodic_ligth.Checked = ds.Tables[0].Rows[0]["Episodic"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Episodic"].ToString()) : false;
    //        chk_rashes.Checked = ds.Tables[0].Rows[0]["Rashes"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Rashes"].ToString()) : false;
    //        chk_hearing_loss.Checked = ds.Tables[0].Rows[0]["HearingLoss"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["HearingLoss"].ToString()) : false;
    //        chk_sleep_disturbance.Checked = ds.Tables[0].Rows[0]["NightSweats"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["NightSweats"].ToString()) : false;

    //        LTricepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRtricepsLeft"]);
    //        RTricepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRtricepsRight"]);
    //        LBicepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBicepsLeft"]);
    //        RBicepstxt.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBicepsRight"]);
    //        RBrachioradialis.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBrachioRight"]);
    //        LBrachioradialis.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRBrachioLeft"]);
    //        LKnee.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRKneeLeft"]);
    //        RKnee.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRKneeRight"]);
    //        LAnkle.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRAnkleLeft"]);
    //        RAnkle.Text = Convert.ToString(ds.Tables[0].Rows[0]["DTRAnkleRight"]);
    //        UExchk.Checked = ds.Tables[0].Rows[0]["UEdtr"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEdtr"].ToString()) : false;
    //        chkPinPrick.Checked = ds.Tables[0].Rows[0]["Pinprick"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Pinprick"].ToString()) : false;
    //        //chkLighttouch.Checked = ds.Tables[0].Rows[0]["Lighttouch"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Lighttouch"].ToString()) : false;
    //        txtSensory.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sensory"]);
    //        LESen_Click.Checked = ds.Tables[0].Rows[0]["LEsen"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEsen"].ToString()) : false;
    //        TextBox4.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL3Right"]);
    //        txtDMTL3.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL3Left"]);
    //        TextBox6.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL4Right"]);
    //        TextBox5.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL4Left"]);
    //        TextBox8.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL5Right"]);
    //        TextBox7.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEL5Left"]);
    //        TextBox10.Text = Convert.ToString(ds.Tables[0].Rows[0]["LES1Left"]);
    //        TextBox21.Text = Convert.ToString(ds.Tables[0].Rows[0]["LES1Right"]);
    //        TextBox24.Text = Convert.ToString(ds.Tables[0].Rows[0]["LELumberParaspinalsRight"]);
    //        TextBox25.Text = Convert.ToString(ds.Tables[0].Rows[0]["LELumberParaspinalsLeft"]);
    //        UESen_Click.Checked = ds.Tables[0].Rows[0]["UEsen"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEsen"].ToString()) : false;
    //        TextBox9.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC5Left"]);
    //        txtUEC5Right.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC5Right"]);
    //        TextBox11.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC6Left"]);
    //        TextBox12.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC6Right"]);
    //        TextBox13.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC7Left"]);
    //        TextBox14.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC7Right"]);
    //        TextBox15.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC8Left"]);
    //        TextBox16.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEC8Right"]);
    //        TextBox18.Text = Convert.ToString(ds.Tables[0].Rows[0]["UET1Right"]);
    //        TextBox17.Text = Convert.ToString(ds.Tables[0].Rows[0]["UET1Left"]);
    //        TextBox20.Text = Convert.ToString(ds.Tables[0].Rows[0]["UECervicalParaspinalsRight"]);
    //        TextBox19.Text = Convert.ToString(ds.Tables[0].Rows[0]["UECervicalParaspinalsLeft"]);
    //        cboHoffmanexam.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["HoffmanExam"]);
    //        chkStocking.Checked = ds.Tables[0].Rows[0]["Stocking"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Stocking"].ToString()) : false;
    //        chkGlove.Checked = ds.Tables[0].Rows[0]["Glove"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Glove"].ToString()) : false;
    //        LEmmst.Checked = ds.Tables[0].Rows[0]["LEmmst"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LEmmst"].ToString()) : false;
    //        TextBox23.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipFlexionRight"]);
    //        TextBox22.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipFlexionLeft"]);
    //        TextBox41.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipAbductionRight"]);
    //        TextBox40.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEHipAbductionLeft"]);
    //        TextBox27.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeExtensionRight"]);
    //        TextBox26.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeExtensionLeft"]);
    //        TextBox43.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeFlexionRight"]);
    //        TextBox42.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEKneeFlexionLeft"]);
    //        TextBox29.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleDorsiRight"]);
    //        TextBox28.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleDorsiLeft"]);
    //        TextBox45.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnklePlantarRight"]);
    //        TextBox44.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnklePlantarLeft"]);
    //        TextBox47.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleExtensorRight"]);
    //        TextBox46.Text = Convert.ToString(ds.Tables[0].Rows[0]["LEAnkleExtensorLeft"]);
    //        UEmmst.Checked = ds.Tables[0].Rows[0]["UEmmst"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["UEmmst"].ToString()) : false;
    //        TextBox31.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderAbductionRight"]);
    //        TextBox30.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderAbductionLeft"]);
    //        TextBox49.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderFlexionRight"]);
    //        TextBox48.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEShoulderFlexionLeft"]);
    //        TextBox33.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowExtensionRight"]);
    //        TextBox32.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowExtensionLeft"]);
    //        TextBox51.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowFlexionRight"]);
    //        TextBox50.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowFlexionLeft"]);
    //        TextBox53.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowSupinationRight"]);
    //        TextBox52.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowSupinationLeft"]);
    //        TextBox55.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowPronationRight"]);
    //        TextBox54.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEElbowPronationLeft"]);
    //        TextBox37.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristFlexionRight"]);
    //        TextBox36.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristFlexionLeft"]);
    //        TextBox57.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristExtensionRight"]);
    //        TextBox56.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEWristExtensionLeft"]);
    //        TextBox39.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandGripStrengthRight"]);
    //        TextBox38.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandGripStrengthLeft"]);
    //        TextBox59.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandFingerAbductorsRight"]);
    //        TextBox58.Text = Convert.ToString(ds.Tables[0].Rows[0]["UEHandFingerAbductorsLeft"]);
    //    }

    //    query = "select FreeForm from tblPatientIE  where PatientIE_ID=" + Session["PatientIE_ID"].ToString();

    //    ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        string freeForm = Convert.ToString(ds.Tables[0].Rows[0]["FreeForm"]);
    //        //string WorkStatusComments = Convert.ToString(ds.Tables[0].Rows[0]["WorkStatusComments"]);
    //        //if (!string.IsNullOrEmpty(WorkStatusComments))
    //        //{
    //        //    workStatusCmnts.Text = WorkStatusComments.ToString();
    //        //}
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
    //                        int cblIndex = 0;
    //                        for (int j = 0; j < restrictionValues.Length; j++)
    //                        {
    //                            for (cblIndex = j; cblIndex < cblRestictions.Items.Count; cblIndex++)
    //                            {


    //                                if (cblRestictions.Items[cblIndex].Value.ToLower() == restrictionValues[j].ToLower())
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
    //                    else if (title == "Work Status")
    //                    {
    //                        string details = freeFormDetails[i].Split(':')[1].Trim();
    //                        string[] workStatusValues = details.Split(',');
    //                        for (int j = 0; j < workStatusValues.Length; j++)
    //                        {
    //                            //for (int cblIndex = 0; cblIndex < cblWorkStatus; cblIndex++)
    //                            //{
    //                            //    if (cblWorkStatus.Items[cblIndex].Value == workStatusValues[j])
    //                            //    {
    //                            //        cblWorkStatus.Items[cblIndex].Selected = true;
    //                            //    }
    //                            //}
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
    //                                    // string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
    //                                    //string degree = chk.Text;
    //                                    // this.SaveData(degree, collageName);
    //                                    chk.Checked = true;
    //                                    (item.FindControl("txtCollageName") as TextBox).Text = v[1];
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    query = "select top 1 * from tblPatientIEDetailPage1 where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        chk_headaches.Checked = ds.Tables[0].Rows[0]["HeadechesAssociated"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["HeadechesAssociated"].ToString()) : false;
    //        chk_nausea.Checked = ds.Tables[0].Rows[0]["Nausea"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Nausea"].ToString()) : false;
    //        chk_dizziness.Checked = ds.Tables[0].Rows[0]["Dizziness"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Dizziness"].ToString()) : false;
    //        chk_vomiting.Checked = ds.Tables[0].Rows[0]["Vomiting"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Vomiting"].ToString()) : false;
    //        chk_anxiety.Checked = ds.Tables[0].Rows[0]["SevereAnxiety"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["SevereAnxiety"].ToString()) : false;
    //    }

    //    query = "select top 1 * from tblbpNeck where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        chk_tingling_in_arms.Checked = ds.Tables[0].Rows[0]["Tingling"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Tingling"].ToString()) : false;
    //        chk_pain_radiating_shoulder.Checked = ds.Tables[0].Rows[0]["ShoulderBilateral1"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ShoulderBilateral1"].ToString()) : false;
    //        chk_numbness_in_arm.Checked = ds.Tables[0].Rows[0]["ArmBilateral2"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["ArmBilateral2"].ToString()) : false;
    //        if (ds.Tables[0].Rows[0]["WeeknessIn"] != DBNull.Value && ds.Tables[0].Rows[0]["WeeknessIn"].ToString() != "")
    //            chk_weakness_in_arm.Checked = true;
    //        else
    //            chk_weakness_in_arm.Checked = false;
    //    }

    //    query = "select top 1 * from tblbpLowBack where PatientIE_ID=" + Session["PatientIE_ID"].ToString();
    //    ds = db.selectData(query);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        chk_tingling_in_legs.Checked = ds.Tables[0].Rows[0]["Tingling"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Tingling"].ToString()) : false;
    //        chk_pain_radiating_leg.Checked = ds.Tables[0].Rows[0]["LegBilateral1"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LegBilateral1"].ToString()) : false;
    //        chk_numbess_in_leg.Checked = ds.Tables[0].Rows[0]["LegBilateral2"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["LegBilateral2"].ToString()) : false;
    //        if (ds.Tables[0].Rows[0]["WeeknessIn"] != DBNull.Value && ds.Tables[0].Rows[0]["WeeknessIn"].ToString() != "")
    //            chk_weakness_in_leg.Checked = true;
    //        else
    //            chk_weakness_in_leg.Checked = false;
    //    }
    //}


    //    protected string GetFreeForm()
    //    {
    //        string restrictions = string.Empty;
    //        //foreach (ListItem s in cblRestictions.Items)
    //        //{
    //        //    if (s.Selected)
    //        //    {
    //        //        restrictions += s.Value.ToLower() + ",";
    //        //    }
    //        //}
    //        //string workStatus = string.Empty;
    //        //foreach (RepeaterItem item in this.Repeater1.Items)
    //        //{
    //        //    CheckBox chk = item.FindControl("cblWorkStatus") as CheckBox;
    //        //    if (chk.Checked)
    //        //    {
    //        //        string collageName = (item.FindControl("txtCollageName") as TextBox).Text;
    //        //        string degree = chk.Text;
    //        //        workStatus += degree.ToLower() + "-" + collageName.ToLower() + ", ";
    //        //        // this.SaveData(degree, collageName);
    //        //    }
    //        //}
    //        //foreach (ListItem s in cblWorkStatus.Items)
    //        //{
    //        //    if (s.Selected)
    //        //    {
    //        //        workStatus += s.Value.ToLower() + ", ";
    //        //    }
    //        //}

    //        string FreeForm = string.Empty;
    //        if (rblDOD.SelectedIndex > -1)
    //        {
    //            FreeForm = "Degree of Disability: " + rblDOD.SelectedValue;
    //        }
    //        if (!string.IsNullOrEmpty(restrictions))
    //        {
    //            if (!string.IsNullOrEmpty(FreeForm))
    //            {
    //                FreeForm += ";";
    //            }
    //            FreeForm += "Restrictions: " + restrictions.TrimEnd(',');
    //        }
    //        if (!string.IsNullOrEmpty(txtOtherRestrictions.Text.Trim()))
    //        {
    //            if (!string.IsNullOrEmpty(FreeForm))
    //            {
    //                FreeForm += ";";
    //            }
    //            FreeForm += "Others: " + txtOtherRestrictions.Text.Trim();
    //        }
    //        if (!string.IsNullOrEmpty(workStatus))
    //        {
    //            if (!string.IsNullOrEmpty(FreeForm))
    //            {
    //                FreeForm += ";";
    //            }
    //            FreeForm += "Work Status: " + workStatus.TrimEnd(',');
    //        }

    //        return (!string.IsNullOrEmpty(FreeForm)) ? FreeForm : "None";
    //    }
    protected void txtLEL3Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLEL3Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLEL4Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLEL3Right_TextChanged1(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLEL4Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLEL5Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLEL5Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLES1Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLES1Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLELumberParaspinalsLeft_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtLELumberParaspinalsRight_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC5Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC5Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC6Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC6Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC7Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC7Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC8Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUEC8Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUET1Left_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUET1Right_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUECervicalParaspinalsLeft_TextChanged(object sender, EventArgs e)
    {
        Settextboxvalue(sender);
    }
    protected void txtUECervicalParaspinalsRight_TextChanged(object sender, EventArgs e)
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
    }


    public void saveHTML()
    {
        string query = "select * from tblPage3FUHTMLContent where PatientFU_ID=" + Session["PatientFuId"].ToString();

        DataSet ds = db.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {
            query = "update tblPage3FUHTMLContent set HTMLContent=@HTMLContent where PatientFU_ID=" + Session["PatientFuId"].ToString();
        }
        else
        {
            query = "insert into tblPage3FUHTMLContent(HTMLContent,PatientIE_ID,PatientFU_ID)values(@HTMLContent,@PatientIE_ID,@PatientFU_ID)";
        }
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@PatientIE_ID", Session["PatientIE_ID"].ToString());
            command.Parameters.AddWithValue("@PatientFU_ID", Session["patientFUId"].ToString());
            command.Parameters.AddWithValue("@HTMLContent", string.IsNullOrEmpty(hdHTMLContent.Value) ? null : hdHTMLContent.Value);

            connection.Open();
            var results = command.ExecuteNonQuery();
            connection.Close();
        }

    }

    public void bindHTML()
    {
        string query = "select * from tblPage3FUHTMLContent where PatientFU_ID=" + Session["PatientFuId"].ToString();

        DataSet ds = db.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {
            divHtml.InnerHtml = ds.Tables[0].Rows[0]["HTMLContent"].ToString();
        }
    }
}