using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Newtonsoft.Json;

public partial class SchedulerSetNew : System.Web.UI.Page
{
    SchDbHelper db = new SchDbHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Write("<script>parent.closemodal()</script>");

            Response.Redirect("Login.aspx");
        }
      

        if (!IsPostBack)
        {

            ddlLocation.DataTextField = "Location";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataSource = db.GetData("select * from tblLocations  where Is_Active='True' Order By Location ");
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("Please Select", "0"));

            if (Request.QueryString["appoint"] != null)
            {
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(Request.QueryString["appoint"].ToString());
                ddlLocation.SelectedValue = values["Location_ID"];
                lblFollowupDetail.Text = values["title"];
                txtNote.Text = values["AppointmentNote"];
                txtFollowedUpOn.Text = Convert.ToDateTime(values["AppointmentDate"] + " " + values["AppointmentStart"]).ToString("MM/dd/yyyy hh:mm tt").Replace('-', '/');// "03/25/2020 10:00 am";/// Request.QueryString["date"].ToString();
                txtFollowedUpOn.Focus();
            }
            else if (Request.QueryString["removeappoint"] != null)
            {

                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(Request.QueryString["removeappoint"].ToString());
                ddlLocation.SelectedValue = values["Location_ID"];
                lblFollowupDetail.Text = values["title"];
                txtNote.Text = values["AppointmentNote"];
                txtFollowedUpOn.Text = Convert.ToDateTime(values["AppointmentDate"] + " " + values["AppointmentStart"]).ToString("MM/dd/yyyy hh:mm tt").Replace('-', '/');// "03/25/2020 10:00 am";/// Request.QueryString["date"].ToString();
                txtFollowedUpOn.Focus();
                txtFollowedUpOn.Enabled = false;
                txtNote.Enabled = false;
                ddlLocation.Enabled = false;
                btnSet.Text = "Delete";
                btnSet.CssClass = "btn btn-danger";

            }
            else if (Request.QueryString["newappoint"] != null && Request.QueryString["location"] != null)
            {
                ddlLocation.SelectedValue = Request.QueryString["location"].ToString();
                divDetail.Visible = false;
                divText.Visible = true;
                txtFollowedUpOn.Enabled = false;
                txtFollowedUpOn.Text = Request.QueryString["newappoint"].ToString();
            }
        }
    }

    protected void btnSet_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["appoint"] != null)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(Request.QueryString["appoint"].ToString());

            if (ddlLocation.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Location First')", true);
                return;
            }
            CultureInfo culture = new CultureInfo("en-US");
            DateTime d = Convert.ToDateTime(txtFollowedUpOn.Text, culture);
            string AppointmentDate = d.ToString("yyyy-MM-dd HH:mm").Replace(" ", "T");
            //  if (!db.CheckDate(AppointmentDate, Convert.ToInt16(ddlLocation.SelectedValue)))
            // {
            db.UpdateDate(values["AppointmentId"], txtFollowedUpOn.Text.Trim(), ddlLocation.SelectedValue, txtNote.Text);
            //            db.UpdateDate(Request.QueryString["id"].ToString(), txtFollowedUpOn.Text.Trim(), ddlLocation.SelectedValue);
            Response.Write("<script>parent.closemodal()</script>");
            // }
            // else
            // {
            //     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Appointment already exsits')", true);
            // }

        }
        else if (Request.QueryString["newappoint"] != null && Request.QueryString["location"] != null)
        {
            if (ddlLocation.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Location First')", true);
                return;
            }
            CultureInfo culture = new CultureInfo("en-US");
            DateTime d = Convert.ToDateTime(txtFollowedUpOn.Text, culture);
            string AppointmentDate = d.ToString("yyyy-MM-dd HH:mm").Replace(" ", "T");
            //         if (!db.CheckDate(AppointmentDate, Convert.ToInt16(ddlLocation.SelectedValue)))
            //       {
            db.Insert(hfPatientId.Value, ddlLocation.SelectedValue, d.ToString("yyyy-MM-dd HH:mm"), txtNote.Text);
            Response.Write("<script>parent.closemodal()</script>");
            //     }
            //   else
            // {
            //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Appointment already exsits')", true);
            // }


        }
        if (Request.QueryString["removeappoint"] != null)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(Request.QueryString["removeappoint"].ToString());
            db.Delete(values["AppointmentId"]);
            Response.Write("<script>parent.closemodal()</script>");
        }

    }



    protected void btnNew_Click(object sender, EventArgs e)
    {
        //setAppoint.Visible = false;
        //newPatient.Visible = true;
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "$j('.dobdate').datepicker({ changeMonth: true, changeYear: true});", true);
        //txtFirstName.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int age = 0;
        ILog log = log4net.LogManager.GetLogger(typeof(SchedulerSetNew));
        DBHelperClass db = new DBHelperClass();
        try
        {
            string SP = "nusp_Insert_Patient_Sch";
            SqlParameter[] param = new SqlParameter[7];
            param[0] = new SqlParameter("@Sex", ddlSex.SelectedValue);
            param[1] = new SqlParameter("@FirstName", txtFirstName.Text);
            param[2] = new SqlParameter("@LastName", txtLastName.Text);
         
            DateTime dob = new DateTime(Int16.Parse(txtDOB.Text.Substring(6)), Int16.Parse(txtDOB.Text.Substring(0,2)), Int16.Parse(txtDOB.Text.Substring(3,2)));
            if (string.IsNullOrWhiteSpace(txtDOB.Text))
            {
                param[3] = new SqlParameter("@DOB", DBNull.Value);
            }
            else
            {
                age = System.DateTime.Now.Year - dob.Year;
                param[3] = new SqlParameter("@DOB", dob);
            }


            param[4] = new SqlParameter("@AGE", age);
            param[5] = new SqlParameter("@Phone", txtPhone.Text);
            param[6] = new SqlParameter("@Phone2", txtMobile.Text);
            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                hfPatientId.Value = val.ToString();
                txtPatientName.Text = ddlSex.SelectedValue + " " + txtFirstName.Text + " " + txtLastName.Text;
            }
        //    Response.Redirect("SchedulerSetNew.aspx?pid=");
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //setAppoint.Visible = true;
        //newPatient.Visible = false;

    }
}