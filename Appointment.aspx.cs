using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Appointment : System.Web.UI.Page
{
    SchDbHelper db = new SchDbHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        { Response.Redirect("Login.aspx"); }

        if (!IsPostBack)
        {
            DataTable dt = db.GetData(@"select * from tblPatientMaster where  Patient_id=" + Request.QueryString["PID"].ToString() );
            if (dt.Rows.Count > 0) 
              lblPatientDetail.Text  =  dt.Rows[0]["FirstName"].ToString()+ " " + dt.Rows[0]["LastName"].ToString();
            txtFollowedUpOn.Focus();
            Repeater1.DataSource = db.GetData("select tblAppointment.*,tblLocations.Location from tblAppointment left join tblLocations on tblAppointment.Location_ID=tblLocations.Location_ID  where  Patient_id=" + Request.QueryString["PID"].ToString() + " order by AppointmentDate desc");
            Repeater1.DataBind();
            ddlLocation.DataTextField = "Location";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataSource = db.GetData("select * from tblLocations  where Is_Active='True' Order By Location ");
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("Please Select", "0"));

            if (HttpContext.Current.Session["Location"] == null)
            {
                ddlLocation.SelectedIndex = -1;
            }
            else
            {
                ddlLocation.SelectedValue = Convert.ToString(Session["Location"]);
            }
        }

    }

    protected void btnSet_Click(object sender, EventArgs e)
    {
        if (ddlLocation.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Location First')", true);
            return;
        }
        CultureInfo culture = new CultureInfo("en-US");
        DateTime d = Convert.ToDateTime(txtFollowedUpOn.Text, culture);
        string AppointmentDate = d.ToString("yyyy-MM-dd HH:mm").Replace(" ", "T");
        if (!db.CheckDate(AppointmentDate,Convert.ToInt16( ddlLocation.SelectedValue)))
        {
            db.Insert(Request.QueryString["PID"], ddlLocation.SelectedValue ,d.ToString("yyyy-MM-dd HH:mm"),txtNote.Text );
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Appointment already exsits')", true);
        }
        Repeater1.DataSource = db.GetData("select tblAppointment.*,tblLocations.Location from tblAppointment left join tblLocations on tblAppointment.Location_ID=tblLocations.Location_ID  where  Patient_id=" + Request.QueryString["PID"].ToString() + " order by AppointmentDate desc");
        Repeater1.DataBind();
    }
    protected static string getDate(string date)
    {
       return Convert.ToDateTime (date).ToString("MM/dd/yyyy").Replace("-","/");
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        db.Delete (e.CommandArgument.ToString());
        Repeater1.DataSource = db.GetData("select tblAppointment.*,tblLocations.Location from tblAppointment left join tblLocations on tblAppointment.Location_ID=tblLocations.Location_ID  where  Patient_id=" + Request.QueryString["PID"].ToString() + " order by AppointmentDate desc");
        Repeater1.DataBind();

    }
}