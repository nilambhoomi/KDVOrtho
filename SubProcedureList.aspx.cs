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
            BindSubProcudureList();
            Session["BodyPartList"] = "ALL";
            bindprocedures();
        }
    }

    protected void BindSubProcudureList(string BodyPart = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_getSubProcedure", con);

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            gvSubProcedureTbl.DataSource = dt;
            Session["Datatable"] = dt;
            gvSubProcedureTbl.DataBind();
        }
    }
    protected void Del(object sender, EventArgs e)
    {
        int status;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["DeleteSubprocedureID"] = row.Cells[0].Text;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("nusp_delSubProcedure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter returnParameter = cmd.Parameters.Add("RetVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.AddWithValue("@SubProcedure", Convert.ToString(ViewState["DeleteSubprocedureID"]));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                status = (int)returnParameter.Value;
            }
            ViewState["DeleteSubprocedureID"] = null;
            BindSubProcudureList("ALL");
            if (status.Equals(5))
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Record is in use can\\'t delete...')</script>", false); }
            else if (status.Equals(2))
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Record Deleted Sucessfully...')</script>", false); }
            else
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Sorry some thing went wrong please contact administrator')</script>", false); }

        }
    }
    protected void Edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["EditSubProcedureID"] = row.Cells[0].Text;
            if (Session["Datatable"] != null)
            {
                DataTable dt = (DataTable)(Session["Datatable"]);
                DataView dv = new DataView(dt.Copy());
                dv.RowFilter = "SubProcedureID=" + ViewState["EditSubProcedureID"];
                DataTable dt1 = dv.ToTable();

                txtSub_CODE.Text = Convert.ToString(dt1.Rows[0]["Sub_CODE"]);
                txtHeadingmuscle.Text = Convert.ToString(dt1.Rows[0]["Heading"]);
                txtCCDesc.Text = Convert.ToString(dt1.Rows[0]["CCDesc"]);
                txtPEDesc.Text = Convert.ToString(dt1.Rows[0]["PEDesc"]);
                txtADesc.Text = Convert.ToString(dt1.Rows[0]["ADesc"]);
                txtPDesc.Text = Convert.ToString(dt1.Rows[0]["PDesc"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["CF"])))
                { chkCF.Checked = Convert.ToBoolean(Convert.ToString(dt1.Rows[0]["CF"])); }
                if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["PN"])))
                { chkPN.Checked = Convert.ToBoolean(Convert.ToString(dt1.Rows[0]["PN"])); }
                if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["ProcedureMCode"])))
                {
                    ddlProcedureID.ClearSelection();
                    ddlProcedureID.Items.FindByText(Convert.ToString(Convert.ToString(dt1.Rows[0]["ProcedureMCode"]))).Selected = true;
                }
            }
            popup.Show();
        }
    }
    protected void Add(object sender, EventArgs e)
    {
        ViewState["EditSubProcedureID"] = null;
        txtSub_CODE.Text = string.Empty;
        txtHeadingmuscle.Text = string.Empty;
        txtCCDesc.Text = string.Empty;
        txtPEDesc.Text = string.Empty;
        txtADesc.Text = string.Empty;
        txtPDesc.Text = string.Empty;
        chkCF.Checked = false;
        chkPN.Checked = false;
        ddlProcedureID.ClearSelection();
        ddlProcedureID.Items.FindByText("--Select--").Selected = true;
        popup.Show();
    }

    protected void Save(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_SaveSubprocedureDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditSubProcedureID"])))
            { cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ViewState["EditSubProcedureID"])); }
            cmd.Parameters.AddWithValue("@Sub_CODE", txtSub_CODE.Text.Trim());
            cmd.Parameters.AddWithValue("@ProcedureID", Convert.ToInt32(ddlProcedureID.SelectedValue));
            cmd.Parameters.AddWithValue("@Heading", txtHeadingmuscle.Text);
            cmd.Parameters.AddWithValue("@CCDesc", txtCCDesc.Text);
            cmd.Parameters.AddWithValue("@PEDesc", txtPEDesc.Text);
            cmd.Parameters.AddWithValue("@ADesc", txtADesc.Text);
            cmd.Parameters.AddWithValue("@PDesc", txtPDesc.Text);
            cmd.Parameters.AddWithValue("@CF", Convert.ToBoolean(chkCF.Checked));
            cmd.Parameters.AddWithValue("@PN", Convert.ToBoolean(chkPN.Checked));
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["EditSubProcedureID"] = null;
            BindSubProcudureList("ALL");
            popup.Hide();
        }
    }
    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvSubProcedureTbl.PageIndex = e.NewPageIndex;
        BindSubProcudureList(Session["BodyPartList"].ToString());
    }

    private void bindprocedures()
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("select ProcedureID,MCODE from Procedures", con);
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            ddlProcedureID.DataSource = dt;
            ddlProcedureID.DataTextField = "MCODE";
            ddlProcedureID.DataValueField = "ProcedureID";
            ddlProcedureID.DataBind();
            ddlProcedureID.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }
    protected void Cancel(object sender, EventArgs e)
    {
        ViewState["EditSubProcedureID"] = null;
        txtSub_CODE.Text = string.Empty;
        txtHeadingmuscle.Text = string.Empty;
        txtCCDesc.Text = string.Empty;
        txtPEDesc.Text = string.Empty;
        txtADesc.Text = string.Empty;
        txtPDesc.Text = string.Empty;
        chkCF.Checked = false;
        chkPN.Checked = false;
        ddlProcedureID.ClearSelection();
        ddlProcedureID.Items.FindByText("--Select--").Selected = true;
        popup.Hide();
    }
}