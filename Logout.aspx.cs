using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        DBHelperClass db = new DBHelperClass();
        if (Session["log"] != null)
        {
            db.logDetailtbl(Convert.ToInt32(Session["log"].ToString()), "LogOut", Convert.ToString(System.DateTime.Now));
            Logger.Info(Session["UserId"].ToString() +Session["uname"].ToString() + "- Logged OUT at" + DateTime.Now);

        }
        Session.Abandon();

        Response.Redirect("Login.aspx");
    }
}