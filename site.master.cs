using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        { Response.Redirect("Login.aspx");}
      
        //if (Convert.ToString(Session["UserDesignation"]).Equals("Administrator"))
        //{
        //    amaster.Visible = true;
        //}
        //else
        //{ amaster.Visible = false; }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Session["PatientIE_ID"] = null;
        Response.Redirect("Page1.aspx");


    }
}
