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
            BindMedicationList();
            Session["BodyPartList"] = "ALL";
            bindProcedure();
        }
    }

    protected void BindMedicationList(string BodyPart = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_getMuscle", con);

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            gvMedicationTbl.DataSource = dt;
            gvMedicationTbl.DataBind();
        }
    }

    protected void Edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["EditMuscleID"] = row.Cells[0].Text;
            txtMuscle.Text = row.Cells[3].Text;
            if (!string.IsNullOrEmpty(Convert.ToString(row.Cells[2].Text)) && !Convert.ToString(row.Cells[2].Text).Equals("&nbsp;"))
            {
                ddlSubProcedure.ClearSelection();
                ddlSubProcedure.Items.FindByText(Convert.ToString(row.Cells[2].Text)).Selected = true;
            }


            if (!string.IsNullOrEmpty(Convert.ToString(row.Cells[1].Text)) && !Convert.ToString(row.Cells[1].Text).Equals("&nbsp;"))
            {
                ddlprocedureID.ClearSelection();
                ddlprocedureID.Items.FindByText(Convert.ToString(row.Cells[1].Text)).Selected = true;
            }
            popup.Show();
        }
    }

    protected void Del(object sender, EventArgs e)
    {
        int status;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["DeleteMuscleID"] = row.Cells[0].Text;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("nusp_delMuscle", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter returnParameter = cmd.Parameters.Add("RetVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.AddWithValue("@Muscle", Convert.ToString(ViewState["DeleteMuscleID"]));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                status = (int)returnParameter.Value;
            }
            ViewState["DeleteMuscleID"] = null;
            BindMedicationList("ALL");
            if (status.Equals(5))
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Record is in use can\\'t delete...')</script>", false); }
            else if (status.Equals(2))
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Record Deleted Sucessfully...')</script>", false); }
            else
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Sorry some thing went wrong please contact administrator')</script>", false); }
            
        }
    }
    protected void Add(object sender, EventArgs e)
    {
        ViewState["EditMuscleID"] = null;
        txtMuscle.Text = string.Empty;
        ddlprocedureID.ClearSelection();
        ddlprocedureID.Items.FindByText("--Select--").Selected = true;
        ddlSubProcedure.ClearSelection();
        ddlSubProcedure.Items.FindByText("--Select--").Selected = true;
        popup.Show();
    }

    protected void Save(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_ModifyMuscle", con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditMuscleID"])))
            { cmd.Parameters.AddWithValue("@MuscleID", Convert.ToInt32(ViewState["EditMuscleID"])); }

            cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ddlSubProcedure.SelectedValue));

            cmd.Parameters.AddWithValue("@procedureID", Convert.ToInt32(ddlprocedureID.SelectedValue));

            cmd.Parameters.AddWithValue("@Muscle", txtMuscle.Text.Trim());

            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["EditMuscleID"] = null;
            BindMedicationList("ALL");
            popup.Hide();
        }
    }
    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvMedicationTbl.PageIndex = e.NewPageIndex;
        BindMedicationList(Session["BodyPartList"].ToString());
    }

    private void bindProcedure()
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("select ProcedureID,MCODE from procedures", con);
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            ddlprocedureID.DataSource = dt;
            ddlprocedureID.DataTextField = "MCODE";
            ddlprocedureID.DataValueField = "ProcedureID";
            ddlprocedureID.DataBind();
            ddlprocedureID.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("select SubProcedureID,Sub_Code from SubProcedures", con);
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            ddlSubProcedure.DataSource = dt;
            ddlSubProcedure.DataTextField = "Sub_Code";
            ddlSubProcedure.DataValueField = "SubProcedureID";
            ddlSubProcedure.DataBind();
            ddlSubProcedure.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }
    protected void Cancel(object sender, EventArgs e)
    {
        ViewState["EditMuscleID"] = null;
        txtMuscle.Text = string.Empty;
        ddlprocedureID.ClearSelection();
        ddlprocedureID.Items.FindByText("--Select--").Selected = true;
        ddlSubProcedure.ClearSelection();
        ddlSubProcedure.Items.FindByText("--Select--").Selected = true;
        popup.Hide();
    }
}