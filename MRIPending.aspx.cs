using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MIRPending : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindLocation();
            LoadPatientIE("", 1);
        }
    }
    private void bindLocation()
    {
        DataSet ds = new DataSet();


        ds = db.selectData("select Location,Location_ID from tblLocations Order By Location");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- Location --", "0"));

            DataSet locds = new DataSet();
            locds.ReadXml(Server.MapPath("~/LocationXML.xml"));

            //if (locds.Tables[0].Rows[0][0].ToString() == System.DateTime.Now.ToString("MM-dd-yyyy"))
            //{
            ddl_location.ClearSelection();
            ddl_location.Items.FindByValue(locds.Tables[0].Rows[0][1].ToString()).Selected = true;
            //}

        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        LoadPatientIE("", 1);
        binddata(ddl_location.SelectedValue.ToString(),txt_Days.Text);
    }
    private void LoadPatientIE(string query, int pageindex)
    {
        try
        {
            int totalcount;
            DataSet dt = new DataSet();

            //dt = db.PatientMRIPending_getAll(query, pageindex, 10, out totalcount);
            //if (dt.Tables[0].Rows.Count > 0)
            //{
            //    rpview.DataSource = dt;
            //    rpview.DataBind();
            //}
            //else
            //{
            //    rpview.DataSource = null;
            //    rpview.DataBind();
            //}
            //PopulatePager(totalcount, pageindex);
            //lblcount.Text = totalcount.ToString();
        }
        catch (Exception ex)
        {
        }
    }
    private void binddata(string id,string day)
    {
        DataSet ds = new DataSet();
        ds = db.test(id,day);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gdview.DataSource = ds;
            gdview.DataBind();
            gdview.Visible = true;
        }
    }

}