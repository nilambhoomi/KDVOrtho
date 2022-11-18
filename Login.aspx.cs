using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IntakeSheet.BLL;

public partial class Login : System.Web.UI.Page
{

    public string UserMasterId = string.Empty;
    string UserId = string.Empty;
    string password = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        UserMasterId = Request.Form["UserMasterId"];
        UserId = Request.Form["UserId"];
        password = Request.Form["password"];

        if (!string.IsNullOrEmpty(UserMasterId) && !string.IsNullOrEmpty(UserMasterId) && !string.IsNullOrEmpty(UserMasterId))
        {

            txtUserMasterID.Text = UserMasterId;
            txt_uname.Text = UserId;
            txt_pass.Text = password;

            Button btn1 = new Button();
            btnLogin_Click(btn1, e);
        }

        //Session.Abandon(); c
        //if (!IsPostBack)
        //{
        //    if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
        //    {
        //        txt_uname.Text = Request.Cookies["UserName"].Value;
        //        txt_pass.Attributes["value"] = Request.Cookies["Password"].Value;
        //        chkRememberMe.Checked = true;
        //    }
        //}

    }

   
  protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (chkRememberMe.Checked)
        {
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
        }
        else
        {
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
        }
       
        //string query = "select Password from tblUserMaster where (LoginID=@uname or eMailID=@uname)";
        string query = " select LoginID,Password,Designation,desig_id,groupid from tblUserMaster where (LoginID=@uname or eMailID=@uname) and UserMasterId='" + txtUserMasterID.Text + "'";
        //SqlConnection cn = new SqlConnection("server=OWNER-PC\\SQLEXPRESS;uid=sa;pwd=Annie123;Initial Catalog=dbPainTraxX3");
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
                try
                {
                    Session["uname"] = txt_uname.Text;
                    DBHelperClass db = new DBHelperClass();
                    string LogName = Session["uname"].ToString();
                    string _result = new BusinessLogic().login(txt_uname.Text.Trim(), txt_pass.Text.Trim());
                    if (_result != null)
                    {
                        Session["UserId"] = _result;
                        Session["UserDesignation"] = ds.Tables[0].Rows[0]["Designation"].ToString();
                        Session["UserDesigId"] = ds.Tables[0].Rows[0]["desig_id"].ToString();
                        DataSet dbGroup = db.selectData("Select * from tblgroups where id=" + ds.Tables[0].Rows[0]["groupid"].ToString());

                        if (dbGroup != null && dbGroup.Tables[0].Rows.Count > 0)
                        {
                            Session["Locations"] = dbGroup.Tables[0].Rows[0]["locations_id"].ToString();
                            Session["pageAccess"] = dbGroup.Tables[0].Rows[0]["page_id"].ToString();
                            Session["roles"] = dbGroup.Tables[0].Rows[0]["role_id"].ToString();
                            Session["reportAccess"] = dbGroup.Tables[0].Rows[0]["reports"].ToString();

                            if (chkRememberMe.Checked)
                            {
                                Response.Cookies["UserName"].Value = txt_uname.Text.Trim();
                                Response.Cookies["Password"].Value = txt_pass.Text.Trim();
                                Response.Cookies["UserMasterId"].Value = txtUserMasterID.Text.Trim();
                            }
                        }
                        else
                        {
                            Session["Locations"] = "";
                            Session["pageAccess"] = "";
                            Session["roles"] = "";
                            Session["reportAccess"] = "";
                        }
                    }

                    string LogLocation = "";
                    string LogIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
                    string LogDescription = "LoginPage Entry";
                    string LogIntime = Convert.ToString(System.DateTime.Now);
                    string LogOutTime = null;
                    string log_id = null;
                    db.logDetail(LogName, LogLocation, LogIp, LogDescription, LogIntime, LogOutTime, log_id);
                    Session["log"] = Convert.ToInt32(HttpContext.Current.Session["log_id"].ToString());
                    db.logDetailtbl(Convert.ToInt32(Session["log"].ToString()), "LogIn", Convert.ToString(System.DateTime.Now));
                    Response.Cookies["UserName"].Value = txt_uname.Text.Trim();
                    Response.Cookies["Password"].Value = txt_pass.Text.Trim();
                    Logger.Info(Session["UserId"].ToString() + '-' + Session["uname"].ToString().Trim() + "- Logged in at -" + DateTime.Now + " with Ip address -" + LogIp);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex);
                }
                Response.Redirect("GetMAProviders.aspx");
            }
            else
                lblmess.Attributes.Add("style", "display:block");
        }
        else
        {
            lblmess.Attributes.Add("style", "display:block");
            Logger.Info("Login Failed" + '-' + txt_uname.Text.Trim());
        }

        
    }

    protected void btnChangePW_Click(object sender, EventArgs e)
    {
        string query = "select Password from tblUserMaster where (LoginID=@uname or eMailID=@uname)";
        //SqlConnection cn = new SqlConnection("server=OWNER-PC\\SQLEXPRESS;uid=sa;pwd=Annie123;Initial Catalog=dbPainTraxX3");
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
                Session["uname"] = txt_uname.Text;
                Response.Redirect("ChangePassword.aspx");
            }
            else
                lblmess.Attributes.Add("style", "display:block");
        }
        else
        {
            lblmess.Attributes.Add("style", "display:block");
        }
    }
    //private string GetUserIP()
    //{
    //    return Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
    //}
}