using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("Login.aspx");
        }
    }


    protected void btnSet_Click(object sender, EventArgs e)
    {
        SchDbHelper db = new SchDbHelper();

        db.setTime(ddlStart.SelectedValue, ddlEnd.SelectedValue);
    }
}