using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        btnCancel.Attributes.Add("onClick", "javascript:window.location.href = 'login.aspx';");
        if (!IsPostBack)
        {
            txt_uname.Text = Session["uname"].ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string query = "select Password from tblUserMaster where (LoginID=@uname or eMailID=@uname)";
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);

        SqlCommand cm = new SqlCommand(query, cn);
        cm.Parameters.AddWithValue("@uname", txt_uname.Text);


        SqlDataAdapter da = new SqlDataAdapter(cm);
        DataSet ds = new DataSet();
        da.Fill(ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["Password"].ToString() == txt_pass.Text)
            {
                //Change the password and redirect next page
                DBHelperClass db = new DBHelperClass();

                query = "UPDATE tblUserMaster SET Password = '" + txt_newPass.Text.Trim() + "' WHERE LoginID = '" + txt_uname.Text.ToString() + "'";

                int val = db.executeQuery(query);

                if (val > 0)
                {
                    Session["uname"] = txt_uname.Text;
                    lblMessage.InnerHtml = "Password has been changed successfuly.";
                    lblMessage.Attributes.Add("style", "color:green");
                    upMessage.Update();
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
                    Response.Redirect("PatientIntakeList.aspx");
                }
            }
            else
                lblmess.Attributes.Add("style", "display:block");
        }
        else
        {
            lblmess.Attributes.Add("style", "display:block");
        }
    }
}