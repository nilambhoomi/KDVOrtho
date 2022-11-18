using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class SchedulerNew : System.Web.UI.Page
{
    static SchDbHelper db = new SchDbHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        DataTable dt = db.GetData("select * from tblSch_Range");
        if (dt.Rows.Count > 0)
        {
            string []start = dt.Rows[0]["SchStart"].ToString().Split(new char[] { ':' });
            string []end = dt.Rows[0]["SchEnd"].ToString().Split(new char[] { ':' });
            string stime = (int.Parse(start[0]) * 60 + int.Parse(start[1])).ToString();
            string etime = (int.Parse(end[0]) * 60 + int.Parse(end[1])).ToString();
            ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", "setTime("+ stime +","+etime+");", true);
        }
        if (!this.IsPostBack)
        {
            ddlLocation.DataTextField = "Location";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataSource = db.GetData("select * from tblLocations  where Is_Active='True' Order By Location ");
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("Please Select", "0"));
            HttpContext.Current.Session["SchLocation"] = null;
            if (HttpContext.Current.Session["Location"] == null)
            {
                ddlLocation.SelectedIndex = -1;
            }
            else
            {
                HttpContext.Current.Session["SchLocation"] = Session["Location"];
                ddlLocation.SelectedValue = Convert.ToString(Session["Location"]);
            }
            

        }
    }

    [WebMethod]
    public static string getAppointments()
    {

       // return db.GetJson(@"select [date],[appointments] from View_Appointments ");
        if (HttpContext.Current.Session["SchLocation"] == null)
        {
            return db.GetJson(@"select [date],[appointments] from View_Appointments where Location_id=" + Convert.ToString(HttpContext.Current.Session["Location"]) );
        }
        else
        {
            return db.GetJson(@"select [date],[appointments] from View_Appointments where Location_id=" + Convert.ToString(HttpContext.Current.Session["SchLocation"]));
        }
    }
    [WebMethod]
    public static string getDayAppointments(string selectedDate)
    {
        SqlCommand cmd = new SqlCommand();
        // return db.GetJson(@"select [date],[appointments] from View_Appointments ");
        //if (HttpContext.Current.Session["SchLocation"] == null)
        //{
        //    cmd.Parameters.AddWithValue("@Date",selectedDate);
        //    cmd.Parameters.AddWithValue("@Location_Id", Convert.ToInt32(HttpContext.Current.Session["Location"]));
        //    return db.GetJson(cmd);
        //}
        //else
        //{
            cmd.Parameters.AddWithValue("@Date", selectedDate);
            cmd.Parameters.AddWithValue("@Location_Id", Convert.ToInt32(HttpContext.Current.Session["SchLocation"]));
            return db.GetJson(cmd);

        //}
    }

    [WebMethod]
    public static string Transfer(string fdate, string tdate)
    {

        if (HttpContext.Current.Session["SchLocation"] == null)
        {
            return db.TransferDate(fdate, tdate, Convert.ToString(HttpContext.Current.Session["Location"]));
        }
        else
        {
            return db.TransferDate(fdate, tdate, Convert.ToString(HttpContext.Current.Session["SchLocation"]));

        }

    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SchLocation"] = ddlLocation.SelectedValue;
//        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "calClicked();", true);

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "setData(JSON.parse(getdata()));calClicked();", true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "showApp(selectDate,'app');;", true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "setAppData(JSON.parse(getapp(selectDate)));", true);


    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {

    }
}