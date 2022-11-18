using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class PatientIntakeSheet : System.Web.UI.Page
{
    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["PatientIE_ID"] != null)
                bindData(Session["PatientIE_ID"].ToString());
            else
                Response.Redirect("Page1.aspx");  
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string query = "";

        if (ViewState["patientIEid"] == null)
        {
            query = "insert into dbo.tblPatientIEDetailPage1(";
            query = query + "PatientIE_ID,Sustained,Position,InvolvedIn,EMSTeam,WentTo,Via,HadXrayOf,HadCTScanOf,";
            query = query + "  PrescriptionFor,FreeForm,InjuryToHead,HadMRIOf)";
            query = query + "values(" + Session["PatientIE_ID"].ToString() + ",'" + ddl_accident_desc.Text + "','" + ddl_belt.Text + "','" + ddl_invovledin.Text + "',";
            query = query + "'" + ddl_EMS.Text + "','" + txt_hospital.Text + "','" + ddl_via.Text + "','" + txt_x_ray.Value + "',";
            query = query + "'" + txt_CT.Value + "','" + txt_prescription.Value + "','" + txt_which_what.Value + "','" + chk_headinjury.Checked + "','" + txt_mri.Value + "')";
        }
        else
        {
            query = "update  tblPatientIEDetailPage1 set ";
            query = query + "Sustained='" + ddl_accident_desc.Text + "',Position='" + ddl_belt.Text + "',";
            query = query + "InvolvedIn='" + ddl_invovledin.Text + "',EMSTeam='" + ddl_EMS.Text + "',WentTo='" + txt_hospital.Text + "',Via='" + ddl_via.Text + "',";
            query = query + "HadXrayOf='" + txt_x_ray.Value + "',HadCTScanOf='" + txt_CT.Value + "',";
            query = query + "  PrescriptionFor='" + txt_prescription.Value + "',FreeForm='" + txt_which_what.Value + "',InjuryToHead='" + chk_headinjury.Checked + "',";
            query = query + " HadMRIOf='" + txt_mri.Value + "' where PatientIE_ID=" + ViewState["patientIEid"].ToString();

        }
        int lresult = gDbhelperobj.executeQuery(query);

        if (lresult > 0)
        {
            if (ViewState["patientIEid"] == null)
            {
                query = "insert into tblInjuredBodyParts(PatientIE_ID,Neck,MidBack,LowBack,";
                query = query + "LeftShoulder,RightShoulder,LeftKnee,RightKnee,LeftElbow,RightElbow,";
                query = query + "LeftWrist,RightWrist,LeftAnkle,RightAnkle,Others)values";
                query = query + "(" + Session["PatientIE_ID"].ToString() + ",'" + chk_Neck.Checked + "','" + chk_Midback.Checked + "','" + chk_lowback.Checked + "',";
                query = query + "'" + chk_L_Shoulder.Checked + "','" + chk_r_Shoulder.Checked + "','" + chk_L_Keen.Checked + "','" + chk_r_Keen.Checked + "',";
                query = query + "'" + chk_l_Elbow.Checked + "','" + chk_r_Elbow.Checked + "','" + chk_l_Wrist.Checked + "','" + chk_r_Wrist.Checked + "',";
                query = query + "'" + chk_l_ankle.Checked + "','" + chk_r_ankle.Checked + "','" + txt_other.Value + "')";
            }
            else
            {
                query = "update tblInjuredBodyParts set Neck='" + chk_Neck.Checked + "',MidBack='" + chk_Midback.Checked + "',LowBack='" + chk_lowback.Checked + "',";
                query = query + "LeftShoulder='" + chk_L_Shoulder.Checked + "',RightShoulder='" + chk_r_Shoulder.Checked + "',LeftKnee='" + chk_L_Keen.Checked + "',RightKnee='" + chk_r_Keen.Checked + "',";
                query = query + "LeftElbow='" + chk_l_Elbow.Checked + "',RightElbow='" + chk_r_Elbow.Checked + "',";
                query = query + "LeftWrist='" + chk_l_Wrist.Checked + "',RightWrist='" + chk_r_Wrist.Checked + "',LeftAnkle='" + chk_l_ankle.Checked + "',";
                query = query + " RightAnkle='" + chk_r_ankle.Checked + "',Others='" + txt_other.Value + "' where PatientIE_ID=" + ViewState["patientIEid"].ToString();

            }
            lresult = gDbhelperobj.executeQuery(query);

            if (lresult > 0)
            {
                if (ViewState["patientIEid"] == null)
                    lblMessage.InnerHtml = "Data Save successfuly.";
                else
                    lblMessage.InnerHtml = "Data Updated successfuly.";
                lblMessage.Attributes.Add("style", "color:green");
                upMessage.Update();


            }
            else
            {
                lblMessage.InnerHtml = "Sorry,there may be same error.";
                lblMessage.Attributes.Add("style", "color:red");
            }
        }
        else
        {
            lblMessage.InnerHtml = "Sorry,there may be same error.";
            lblMessage.Attributes.Add("style", "color:red");
        }

        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);

    }

    private void bindData(string patientIEid)
    {
        string query = "select * from tblPatientIEDetailPage1 where PatientIE_ID=" + patientIEid;

        DataSet ds = gDbhelperobj.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {

            ViewState["patientIEid"] = patientIEid;

            txt_CT.Value = ds.Tables[0].Rows[0]["HadCTScanOf"].ToString();
            txt_hospital.Text = ds.Tables[0].Rows[0]["WentTo"].ToString();
            txt_mri.Value = ds.Tables[0].Rows[0]["HadMRIOf"].ToString();
            txt_prescription.Value = ds.Tables[0].Rows[0]["Persistent"].ToString();
            txt_which_what.Value = ds.Tables[0].Rows[0]["FreeForm"].ToString();
            txt_x_ray.Value = ds.Tables[0].Rows[0]["HadXrayOf"].ToString();

            ddl_accident_desc.Text = ds.Tables[0].Rows[0]["Sustained"].ToString();
            ddl_belt.Text = ds.Tables[0].Rows[0]["Position"].ToString();
            ddl_EMS.Text = ds.Tables[0].Rows[0]["EMSTeam"].ToString();
            ddl_invovledin.Text = ds.Tables[0].Rows[0]["InvolvedIn"].ToString();
            ddl_via.Text = ds.Tables[0].Rows[0]["Via"].ToString();

            if (string.IsNullOrEmpty(txt_hospital.Text))
            {
                rep_hospitalized.Items.FindByValue("0").Selected = true;
                hospitlediv.Attributes.Add("disabled", "true");
                txt_hospital.Enabled = false;
            }
            else
            {
                rep_hospitalized.Items.FindByValue("1").Selected = true;
                hospitlediv.Attributes.Add("disabled", "false");
                txt_hospital.Enabled = true;
            }

            if (string.IsNullOrEmpty(txt_docname.Value))
            {
                rbl_seen_injury.Items.FindByValue("0").Selected = true;
                txt_docname.Attributes.Add("disabled", "true");
            }
            else
            {
                rbl_seen_injury.Items.FindByValue("1").Selected = true;
                txt_docname.Attributes.Add("disabled", "false");
            }
            if (string.IsNullOrEmpty(txt_injur_past.Value))
            {
                rbl_in_past.Items.FindByValue("0").Selected = true;
                txt_injur_past.Attributes.Add("disabled", "true");
            }
            else
            {
                rbl_in_past.Items.FindByValue("1").Selected = true;
                txt_injur_past.Attributes.Add("disabled", "false");
            }
        }

        query = "select * from tblInjuredBodyParts where PatientIE_ID=" + patientIEid;

        ds = gDbhelperobj.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["LeftAnkle"] != DBNull.Value)
                chk_l_ankle.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftAnkle"].ToString());

            if (ds.Tables[0].Rows[0]["RightAnkle"] != DBNull.Value)
                chk_l_ankle.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightAnkle"].ToString());


            if (ds.Tables[0].Rows[0]["LeftShoulder"] != DBNull.Value)
                chk_L_Shoulder.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftShoulder"].ToString());

            if (ds.Tables[0].Rows[0]["RightShoulder"] != DBNull.Value)
                chk_r_Shoulder.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightShoulder"].ToString());

            if (ds.Tables[0].Rows[0]["LeftKnee"] != DBNull.Value)
                chk_L_Keen.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftKnee"].ToString());

            if (ds.Tables[0].Rows[0]["RightKnee"] != DBNull.Value)
                chk_r_Keen.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightKnee"].ToString());

            if (ds.Tables[0].Rows[0]["LeftElbow"] != DBNull.Value)
                chk_l_Elbow.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftElbow"].ToString());

            if (ds.Tables[0].Rows[0]["RightElbow"] != DBNull.Value)
                chk_r_Elbow.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightElbow"].ToString());

            if (ds.Tables[0].Rows[0]["LeftWrist"] != DBNull.Value)
                chk_l_Wrist.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftWrist"].ToString());

            if (ds.Tables[0].Rows[0]["RightWrist"] != DBNull.Value)
                chk_r_Wrist.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightWrist"].ToString());

            if (ds.Tables[0].Rows[0]["LeftHip"] != DBNull.Value)
                chk_l_Hip.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LeftHip"].ToString());

            if (ds.Tables[0].Rows[0]["RightHip"] != DBNull.Value)
                chk_r_Hip.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["RightHip"].ToString());


            if (ds.Tables[0].Rows[0]["Neck"] != DBNull.Value)
                chk_Neck.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Neck"].ToString());

            if (ds.Tables[0].Rows[0]["MidBack"] != DBNull.Value)
                chk_Midback.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["MidBack"].ToString());

            if (ds.Tables[0].Rows[0]["LowBack"] != DBNull.Value)
                chk_lowback.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["LowBack"].ToString());


            txt_other.Value = ds.Tables[0].Rows[0]["Others"].ToString();
        }
    }
}