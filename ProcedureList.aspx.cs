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
            BindProcudureList();
        }
    }
    protected void lnkclone_Click(object sender, EventArgs e)
    {
        LinkButton lnkclone = sender as LinkButton;

        string Procedure_ID = Convert.ToString(lnkclone.CommandArgument).Split('~')[0];
        

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("CloneProcedure", con);

            if (!string.IsNullOrEmpty(Procedure_ID))
            {
                cmd.Parameters.AddWithValue("@Procedure_ID", Procedure_ID);
            }
            
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.ExecuteReader();
            con.Close();
        }
        
        BindProcudureList();
    }

    protected void BindProcudureList(string BodyPart = null, string Mcode = null, string Heading = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_getProcedurenew", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BodyPart", BodyPart);
            cmd.Parameters.AddWithValue("@Mcode", Mcode);
            cmd.Parameters.AddWithValue("@Heading", Heading);
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            DataView dv = new DataView(dt);
            dv.Sort = "Display_Order";
            gvProcedureTbl.DataSource = dv;
            Session["Datatableprocedure"] = dv.ToTable();
            gvProcedureTbl.DataBind();
        }
    }

    protected void Edit(object sender, EventArgs e)
    {
        LinkButton btnedit = sender as LinkButton;
        Response.Redirect("AddProcedure.aspx?id=" + btnedit.CommandArgument);
    }
    protected void Clone(object sender, EventArgs e)
    {
        LinkButton btnClone = sender as LinkButton;
        Response.Redirect("AddProcedure.aspx?Cloneid=" + btnClone.CommandArgument);
    }
    protected void Add(object sender, EventArgs e)
    {
        ViewState["EditProcedure_ID"] = null;
        txtBodyParts.Text = string.Empty;
        txtMCODE.Text = string.Empty;
        txtHeading.Text = string.Empty;
        txtCCDesc.Text = string.Empty;
        txtPEDesc.Text = string.Empty;
        txtADesc.Text = string.Empty;
        txtPDesc.Text = string.Empty;

        txtHeadingS.Text = string.Empty;
        txtHeadingE.Text = string.Empty;


        txtS_CCDesc.Text = string.Empty;
        txtS_PEDesc.Text = string.Empty;
        txtS_ADesc.Text = string.Empty;
        txtS_PDesc.Text = string.Empty;



        txtE_CCDesc.Text = string.Empty;
        txtE_PEDesc.Text = string.Empty;
        txtE_ADesc.Text = string.Empty;
        txtE_PDesc.Text = string.Empty;



        txtDisplay_Order.Text = string.Empty;
        ddlposition.SelectedValue = "-1";
        txtMuscles.Text = string.Empty;
        txtMedication.Text = string.Empty;
        chkINhouseProcbit.Checked = false;
        chkHasLevel.Checked = false;
        //chkInOut.Checked = false;
        chkCF.Checked = false;
        chkPN.Checked = false;
        chkSides.Checked = false;
        txtLevelsDefault.Text = string.Empty;
        ddlSidesDefault.SelectedValue = " ";
        popup.Show();
    }

    protected void Save(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_SaveprocedureDetailsnew", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditProcedure_ID"])))
            { cmd.Parameters.AddWithValue("@procedure_ID", Convert.ToInt32(ViewState["EditProcedure_ID"])); }
            cmd.Parameters.AddWithValue("@BodyPart", txtBodyParts.Text.Trim());
            cmd.Parameters.AddWithValue("@MCODE", txtMCODE.Text.Trim());
            cmd.Parameters.AddWithValue("@Heading", txtHeading.Text.Trim());
            cmd.Parameters.AddWithValue("@CCDesc", txtCCDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@PEDesc", txtPEDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@ADesc", txtADesc.Text.Trim());
            cmd.Parameters.AddWithValue("@PDesc", txtPDesc.Text.Trim());

            cmd.Parameters.AddWithValue("@S_Heading", txtHeadingS.Text.Trim());
            cmd.Parameters.AddWithValue("@E_Heading", txtHeadingE.Text.Trim());

            cmd.Parameters.AddWithValue("@S_CCDesc", txtS_CCDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@S_PEDesc", txtS_PEDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@S_ADesc", txtS_ADesc.Text.Trim());
            cmd.Parameters.AddWithValue("@S_PDesc", txtS_PDesc.Text.Trim());

            cmd.Parameters.AddWithValue("@E_CCDesc", txtE_CCDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@E_PEDesc", txtE_PEDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@E_ADesc", txtE_ADesc.Text.Trim());
            cmd.Parameters.AddWithValue("@E_PDesc", txtE_PDesc.Text.Trim());


            cmd.Parameters.AddWithValue("@Display_Order", txtDisplay_Order.Text.Trim());
            if (ddlposition.SelectedValue.Equals("-1"))
            { cmd.Parameters.AddWithValue("@Position", ""); }
            else
            { cmd.Parameters.AddWithValue("@Position", ddlposition.SelectedValue); }

            cmd.Parameters.AddWithValue("@Muscle", txtMuscles.Text.Trim());
            cmd.Parameters.AddWithValue("@Medication", txtMedication.Text.Trim());
            cmd.Parameters.AddWithValue("@SubProcedure", txtSubProcedure.Text.Trim());

            cmd.Parameters.AddWithValue("@INhouseProcbit", chkINhouseProcbit.Checked);
            cmd.Parameters.AddWithValue("@HasLevel", chkHasLevel.Checked);
            //cmd.Parameters.AddWithValue("@INout", chkInOut.Checked);
            cmd.Parameters.AddWithValue("@CF", chkCF.Checked);
            cmd.Parameters.AddWithValue("@PN", chkPN.Checked);
            cmd.Parameters.AddWithValue("@sides", chkSides.Checked);
            cmd.Parameters.AddWithValue("@LevelsDefault", txtLevelsDefault.Text);
            cmd.Parameters.AddWithValue("@SidesDefault", ddlSidesDefault.SelectedValue);
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["EditProcedureID"] = null;
            filter();
            popup.Hide();
        }
    }
    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvProcedureTbl.PageIndex = e.NewPageIndex;
        filter();
    }
    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        ViewState["z_sortexpresion"] = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, "DESC");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, "ASC");
        }

    }

    public string SortExpression
    {
        get
        {
            if (ViewState["z_sortexpresion"] == null)
                ViewState["z_sortexpresion"] = this.gvProcedureTbl.DataKeyNames[0].ToString();
            return ViewState["z_sortexpresion"].ToString();
        }
        set
        {
            ViewState["z_sortexpresion"] = value;
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = ((DataTable)Session["Datatableprocedure"]);
        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + " " + direction;
        this.gvProcedureTbl.DataSource = dv;
        gvProcedureTbl.DataBind();
    }
    protected void Del(object sender, EventArgs e)
    {
        int status;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["DeleteprocedureID"] = row.Cells[0].Text;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("nusp_delProcedurenew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter returnParameter = cmd.Parameters.Add("RetVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.AddWithValue("@Procedure", Convert.ToString(ViewState["DeleteprocedureID"]));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                status = (int)returnParameter.Value;
            }
            ViewState["DeleteSubprocedureID"] = null;
            filter();
            if (status.Equals(5))
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Record is in use can\\'t delete...')</script>", false); }
            else if (status.Equals(2))
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Record Deleted Sucessfully...')</script>", false); }
            else
            { ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>alert('Sorry some thing went wrong please contact administrator')</script>", false); }

        }
    }
    protected void Cancel(object sender, EventArgs e)
    {
        ViewState["EditProcedure_ID"] = null;
        txtBodyParts.Text = string.Empty;
        txtMCODE.Text = string.Empty;
        txtHeading.Text = string.Empty;
        txtCCDesc.Text = string.Empty;
        txtPEDesc.Text = string.Empty;
        txtADesc.Text = string.Empty;
        txtPDesc.Text = string.Empty;

        txtS_CCDesc.Text = string.Empty;
        txtS_PEDesc.Text = string.Empty;
        txtS_ADesc.Text = string.Empty;
        txtS_PDesc.Text = string.Empty;

        txtE_CCDesc.Text = string.Empty;
        txtE_PEDesc.Text = string.Empty;
        txtE_ADesc.Text = string.Empty;
        txtE_PDesc.Text = string.Empty;


        txtDisplay_Order.Text = string.Empty;
        ddlposition.SelectedValue = "-1";
        txtMuscles.Text = string.Empty;
        txtMedication.Text = string.Empty;
        txtSubProcedure.Text = string.Empty;
        chkINhouseProcbit.Checked = false;
        chkHasLevel.Checked = false;
        //chkInOut.Checked = false;
        chkCF.Checked = false;
        chkPN.Checked = false;
        chkSides.Checked = false;
        popup.Hide();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearchBodyPart.Text = string.Empty;
        txtSearchMcode.Text = string.Empty;
        txtSearchHeading.Text = string.Empty;
        filter();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        filter();
    }
    private void filter()
    {
        BindProcudureList(txtSearchBodyPart.Text, txtSearchMcode.Text, txtSearchHeading.Text);
    }
}