using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Procedures : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            BindBodyList();
            Session["BodyPartList"] = "ALL";
        }
    }

    protected void BindBodyList(string BodyPart = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Select * from tblbodyparts", con);
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            gvProcedureTbl.DataSource = dt;
            gvProcedureTbl.DataBind();
        }
    }

    protected void Edit(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        ViewState["EditBodyPart"] = btn.CommandArgument;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            txtBodyPart.Text = row.Cells[0].Text;
            popup.Show();
        }
    }
    protected void Add(object sender, EventArgs e)
    {
        txtBodyPart.Text = string.Empty;
        popup.Show();
    }

    protected void Save(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_SaveBodyParts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditBodyPart"])))
            { cmd.Parameters.AddWithValue("@BID", Convert.ToInt32(ViewState["EditBodyPart"])); }
            cmd.Parameters.AddWithValue("@BodyPart", txtBodyPart.Text.Trim());
            con.Open();
            int count = cmd.ExecuteNonQuery();
            ViewState["EditBodyPart"] = null;
            BindBodyList("ALL");
            popup.Hide();
        }
    }
    protected void Cancel(object sender, EventArgs e)
    {
        ViewState["EditBodyPart"] = null;
        txtBodyPart.Text = string.Empty;
        popup.Hide();
    }
    protected void gvBodyDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvProcedureTbl.PageIndex = e.NewPageIndex;
        BindBodyList(Session["BodyPartList"].ToString());
    }
}