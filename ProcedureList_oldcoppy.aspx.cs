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
            Session["BodyPartList"] = "ALL";
            bindbodyparts();
        }
    }

    protected void BindProcudureList(string BodyPart = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_getProcedure", con);

            cmd.CommandType = CommandType.StoredProcedure;
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
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hdn_ID.Value = row.Cells[0].Text;
            txtMCODE.Text = row.Cells[1].Text;
            txtHeading.Text = row.Cells[3].Text;
            chkINhouseProc.Checked = Convert.ToBoolean(row.Cells[4].Text);
            chkHasPosition.Checked = Convert.ToBoolean(row.Cells[5].Text);
            chkHasLevel.Checked = Convert.ToBoolean(row.Cells[6].Text);
            chkHasMuscle.Checked = Convert.ToBoolean(row.Cells[7].Text);
            chkHasMedication.Checked = Convert.ToBoolean(row.Cells[8].Text);
            chkHasSubCode.Checked = Convert.ToBoolean(row.Cells[9].Text);
            if (!string.IsNullOrEmpty(Convert.ToString(row.Cells[10].Text)))
            {
                ddlBID.ClearSelection();
                ddlBID.Items.FindByText(Convert.ToString(row.Cells[10].Text)).Selected = true;
            }
            popup.Show();
        }
    }
    protected void Add(object sender, EventArgs e)
    {
        txtHeading.Text = string.Empty;
        txtMCODE.Text = string.Empty;
        chkINhouseProc.Checked = false;
        chkHasPosition.Checked = false;
        chkHasLevel.Checked = false;
        chkHasMuscle.Checked = false;
        chkHasMedication.Checked = false;
        chkHasSubCode.Checked = false;
        ddlBID.ClearSelection();
        ddlBID.Items.FindByText("--Select--").Selected = true;
        popup.Show();
    }

    protected void Save(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_SaveprocedureDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(hdn_ID.Value)))
            { cmd.Parameters.AddWithValue("@procedureID", Convert.ToInt32(hdn_ID.Value)); }
            cmd.Parameters.AddWithValue("@MCODE", txtMCODE.Text.Trim());
            cmd.Parameters.AddWithValue("@Heading", txtHeading.Text.Trim());
            cmd.Parameters.AddWithValue("@INhouseProc", Convert.ToBoolean(chkINhouseProc.Checked));
            cmd.Parameters.AddWithValue("@HasPosition", Convert.ToBoolean(chkHasPosition.Checked));
            cmd.Parameters.AddWithValue("@HasLevel", Convert.ToBoolean(chkHasLevel.Checked));
            cmd.Parameters.AddWithValue("@HasMuscle", Convert.ToBoolean(chkHasMuscle.Checked));
            cmd.Parameters.AddWithValue("@HasMedication", Convert.ToBoolean(chkHasMedication.Checked));
            cmd.Parameters.AddWithValue("@HasSubCode", Convert.ToBoolean(chkHasSubCode.Checked));
            cmd.Parameters.AddWithValue("@BID", Convert.ToInt32(ddlBID.SelectedValue));
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            BindProcudureList("ALL");
            popup.Hide();
        }
    }
    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvProcedureTbl.PageIndex = e.NewPageIndex;
        BindProcudureList(Session["BodyPartList"].ToString());
    }

    private void bindbodyparts()
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("select * from tblbodyparts", con);
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            ddlBID.DataSource = dt;
            ddlBID.DataTextField = "BodyPart";
            ddlBID.DataValueField = "BID";
            ddlBID.DataBind();
            ddlBID.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }
    protected void Cancel(object sender, EventArgs e)
    {
        popup.Hide();
    }

    protected void AddMedication(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["ProcedureID"] = row.Cells[0].Text;
            HiddenField hdnmedicationids = (HiddenField)row.FindControl("hdn_MedicationID");
            HiddenField hdn_SubProcedureID = (HiddenField)row.FindControl("hdn_SubProcedureID");
            HiddenField hdn_MuscleIDs = (HiddenField)row.FindControl("hdn_MuscleIDs");
            ViewState["MedicationIds"] = hdnmedicationids.Value;
            ViewState["SubProcedureID"] = hdn_SubProcedureID.Value;
            ViewState["MusclesIDs"] = hdn_MuscleIDs.Value;
            popupMedication.Show();
        }
    }
    protected string Values;
    protected void SaveMedication(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string[] textboxValues = Request.Form.GetValues("DynamicTextBox");
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        this.Values = serializer.Serialize(textboxValues);
        int count = 0;
        foreach (string textboxValue in textboxValues)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("nusp_ModifyMedication", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["ProcedureID"])))
                { cmd.Parameters.AddWithValue("@procedureID", Convert.ToInt32(ViewState["ProcedureID"])); }
                cmd.Parameters.AddWithValue("@Medication", textboxValue);
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["SubProcedureID"])))
                { cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ViewState["SubProcedureID"])); }
                con.Open();
                count = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        ViewState["MedicationIds"] = null;
        BindProcudureList("ALL");
    }

    protected void EditMedication(object sender, EventArgs e)
    {
        LinkButton lkedit = (LinkButton)(sender);
        ViewState["EditmedicationDetail_MedicationID"] = lkedit.CommandArgument;

        string GetDynamicTextBox = string.Empty;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //ViewState["ProcedureID"] = row.Cells[0].Text;
            HiddenField hdn_procedureID = (HiddenField)row.FindControl("hdn_procedureID");
            ViewState["EditmedicationDetail_procedureID"] = hdn_procedureID.Value;
            HiddenField hdn_SubProcedureID = (HiddenField)row.FindControl("hdn_SubProcedureID");
            ViewState["EditmedicationDetail_SubProcedureID"] = hdn_SubProcedureID.Value;
            txtEditdetails.Text = row.Cells[0].Text;
        }
        //string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {

        //        string query = "SELECT STUFF((SELECT ',' + convert(varchar, s.Medication) FROM tblmedication s";
        //        query += " where(procedureID = " + Convert.ToInt32(ViewState["ProcedureID"]);
        //        if (!string.IsNullOrEmpty(Convert.ToString(ViewState["SubProcedureID"])))
        //        { query += " AND(ISNULL(SubProcedureID, 0) = ISNULL(" + Convert.ToInt32(ViewState["SubProcedureID"]) + ", 0)))"; }
        //        else
        //        { query += " AND(ISNULL(SubProcedureID, 0) = ISNULL( NULL , 0)))"; }

        //        query += " FOR XML PATH('')),1, 1, '') AS Medication";

        //        SqlCommand cmd = new SqlCommand(query, con);
        //        con.Open();
        //        DataTable dt = new DataTable();
        //        dt.Load(cmd.ExecuteReader());
        //        con.Close();

        //        string[] textboxValues = Convert.ToString(dt.Rows[0]["Medication"]).Split(',');
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        GetDynamicTextBox = serializer.Serialize(textboxValues);
        //    }
        //}
        btnsaveeditedmuscles.Visible = false;
        btnsaveeditedmedication.Visible = true;
        popupEditMuscleMedication.Show();
        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>RecreateDynamicTextboxes('" + GetDynamicTextBox + "');</script>", false);
    }

    protected void CancelMedication(object sender, EventArgs e)
    {
        ViewState["ProcedureID"] = null;
        popupMedication.Hide();
    }

    protected void AddSubprocedure(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["EditsubprocedureId"] = null;
            ViewState["newProcedureID"] = row.Cells[0].Text;
            HiddenField hdnmedicationids = (HiddenField)row.FindControl("hdn_MedicationID");
            txtMedication.Text = hdnmedicationids.Value;
            txtSub_CODE.Text = string.Empty;
            txtHeadingmuscle.Text = string.Empty;
            txtCCDesc.Text = string.Empty;
            txtPEDesc.Text = string.Empty;
            txtADesc.Text = string.Empty;
            txtPDesc.Text = string.Empty;
            chkCF.Checked = false;
            chkPN.Checked = false;
            popupSubprocedure.Show();
        }
    }
    protected void SaveSubprocedure(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {

            SqlCommand cmd = new SqlCommand("nusp_SaveSubprocedureDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditsubprocedureId"])))
            { cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ViewState["EditsubprocedureId"])); }
            cmd.Parameters.AddWithValue("@Sub_CODE", txtSub_CODE.Text.Trim());
            cmd.Parameters.AddWithValue("@ProcedureID", Convert.ToInt32(ViewState["newProcedureID"]));
            cmd.Parameters.AddWithValue("@Medication", txtMedication.Text);
            cmd.Parameters.AddWithValue("@Heading", txtHeadingmuscle.Text);
            cmd.Parameters.AddWithValue("@CCDesc", txtCCDesc.Text);
            cmd.Parameters.AddWithValue("@PEDesc", txtCCDesc.Text);
            cmd.Parameters.AddWithValue("@ADesc", txtCCDesc.Text);
            cmd.Parameters.AddWithValue("@PDesc", txtCCDesc.Text);
            cmd.Parameters.AddWithValue("@CF", Convert.ToBoolean(chkCF.Checked));
            cmd.Parameters.AddWithValue("@PN", Convert.ToBoolean(chkPN.Checked));
            con.Open();
            var count = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["EditsubprocedureId"] = null;
            BindProcudureList("ALL");
            popupSubprocedure.Hide();
        }
    }
    protected void EditSubprocedure(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            HiddenField hdn_SubProcedureID = (HiddenField)row.FindControl("hdn_SubProcedureID");
            ViewState["EditsubprocedureId"] = hdn_SubProcedureID.Value;

            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select * from SubProcedures where SubProcedureID=" + Convert.ToInt32(ViewState["EditsubprocedureId"]);

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                con.Close();
                ViewState["newProcedureID"] = Convert.ToString(dt.Rows[0]["ProcedureID"]);
                txtSub_CODE.Text = Convert.ToString(dt.Rows[0]["Sub_CODE"]);
                txtMedication.Text = Convert.ToString(dt.Rows[0]["Medication"]);
                txtHeadingmuscle.Text = Convert.ToString(dt.Rows[0]["Heading"]);
                txtCCDesc.Text = Convert.ToString(dt.Rows[0]["CCDesc"]);
                txtPEDesc.Text = Convert.ToString(dt.Rows[0]["PEDesc"]);
                txtADesc.Text = Convert.ToString(dt.Rows[0]["ADesc"]);
                txtPDesc.Text = Convert.ToString(dt.Rows[0]["PDesc"]);
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["CF"])))
                { chkCF.Checked = Convert.ToBoolean(dt.Rows[0]["CF"]); }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["PN"])))
                { chkPN.Checked = Convert.ToBoolean(dt.Rows[0]["PN"]); }
            }
            popupSubprocedure.Show();
        }
    }

    protected void CancelSubprocedure(object sender, EventArgs e)
    { popupSubprocedure.Hide(); }
    protected void AddMuscle(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            ViewState["ProcedureID"] = row.Cells[0].Text;
            HiddenField hdnmedicationids = (HiddenField)row.FindControl("hdn_MedicationID");
            HiddenField hdn_SubProcedureID = (HiddenField)row.FindControl("hdn_SubProcedureID");
            HiddenField hdn_MuscleIDs = (HiddenField)row.FindControl("hdn_MuscleIDs");
            ViewState["MedicationIds"] = hdnmedicationids.Value;
            ViewState["SubProcedureID"] = hdn_SubProcedureID.Value;
            ViewState["MusclesIDs"] = hdn_MuscleIDs.Value;

            popupMuscle.Show();
        }
    }
    protected string ValuesMuscle;
    protected void SaveMuscles(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string[] textboxValues = Request.Form.GetValues("DynamicTextBoxMuscle");
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        this.ValuesMuscle = serializer.Serialize(textboxValues);
        int count = 0;
        foreach (string textboxValue in textboxValues)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("nusp_ModifyMuscle", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["ProcedureID"])))
                { cmd.Parameters.AddWithValue("@procedureID", Convert.ToInt32(ViewState["ProcedureID"])); }
                cmd.Parameters.AddWithValue("@Muscle", textboxValue);
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["SubProcedureID"])))
                { cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ViewState["SubProcedureID"])); }
                con.Open();
                count = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        ViewState["MusclesIDs"] = null;
        BindProcudureList("ALL");
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["EditprocedureId"] = gvProcedureTbl.DataKeys[e.Row.RowIndex].Value.ToString();
            HiddenField hdnmedicationids = (HiddenField)e.Row.FindControl("hdn_MedicationID");
            HiddenField hdn_SubProcedureID = (HiddenField)e.Row.FindControl("hdn_SubProcedureID");
            HiddenField hdn_MuscleIDs = (HiddenField)e.Row.FindControl("hdn_MuscleIDs");
            ViewState["MedicationIds"] = hdnmedicationids.Value;
            ViewState["EditSubprocedureId"] = hdn_SubProcedureID.Value;
            ViewState["MusclesIDs"] = hdn_MuscleIDs.Value;
            GridView gv = (GridView)e.Row.FindControl("gvMedication");
            GridView gv1 = (GridView)e.Row.FindControl("gvMuscle");
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select * from tblMedication";
                query += " where procedureID = " + Convert.ToInt32(ViewState["EditprocedureId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditSubprocedureId"])))
                { query += " AND ISNULL(SubProcedureID, 0) = ISNULL(" + Convert.ToInt32(ViewState["EditSubprocedureId"]) + ", 0)"; }
                else
                { query += " AND ISNULL(SubProcedureID, 0)= ISNULL( NULL , 0)"; }

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                con.Close();
                gv.DataSource = dt;
                gv.DataBind();


                string queryMuscle = "select * from tblMuscles";
                queryMuscle += " where procedureID = " + Convert.ToInt32(ViewState["EditprocedureId"]);
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditSubprocedureId"])))
                { queryMuscle += " AND ISNULL(SubProcedureID, 0) = ISNULL(" + Convert.ToInt32(ViewState["EditSubprocedureId"]) + ", 0)"; }
                else
                { queryMuscle += " AND ISNULL(SubProcedureID, 0)= ISNULL( NULL , 0)"; }

                SqlCommand cmd1 = new SqlCommand(queryMuscle, con);
                con.Open();
                DataTable dt1 = new DataTable();
                dt1.Load(cmd1.ExecuteReader());
                con.Close();
                gv1.DataSource = dt1;
                gv1.DataBind();
            }
        }
    }

    protected void EditMuscle(object sender, EventArgs e)
    {
        LinkButton lkedit = (LinkButton)(sender);
        ViewState["EditMuscleDetail_MuscleID"] = lkedit.CommandArgument;

        string GetDynamicTextBox = string.Empty;
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            //ViewState["ProcedureID"] = row.Cells[0].Text;
            HiddenField hdn_procedureID = (HiddenField)row.FindControl("hdn_procedureID");
            ViewState["EditmuscleDetail_procedureID"] = hdn_procedureID.Value;
            HiddenField hdn_SubProcedureID = (HiddenField)row.FindControl("hdn_SubProcedureID");
            ViewState["EditmuscleDetail_SubProcedureID"] = hdn_SubProcedureID.Value;
            txtEditdetails.Text = row.Cells[0].Text;
        }
        btnsaveeditedmuscles.Visible = true;
        btnsaveeditedmedication.Visible = false;
        popupEditMuscleMedication.Show();
    }

    protected void CancelMuscle(object sender, EventArgs e)
    { popupMuscle.Hide(); }

    protected void btncancleEditdetails_Click(object sender, EventArgs e)
    {
        ViewState["EditmuscleDetail_procedureID"] = null;
        ViewState["EditMuscleDetail_MuscleID"] = null;
        ViewState["EditmuscleDetail_SubProcedureID"] = null;
        ViewState["EditmedicationDetail_procedureID"] = null;
        ViewState["EditmedicationDetail_MedicationID"] = null;
        ViewState["EditmedicationDetail_SubProcedureID"] = null;
        txtEditdetails.Text = string.Empty;
        txtEditdetails.Text = string.Empty;
        popupEditMuscleMedication.Hide();
    }

    protected void btnsaveeditedmuscles_Click(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_ModifyMuscle", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditmuscleDetail_procedureID"])))
            { cmd.Parameters.AddWithValue("@procedureID", Convert.ToInt32(ViewState["EditmuscleDetail_procedureID"])); }
            cmd.Parameters.AddWithValue("@Muscle", txtEditdetails.Text);
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditMuscleDetail_MuscleID"])))
            {
                cmd.Parameters.AddWithValue("@MuscleID", Convert.ToInt32(ViewState["EditMuscleDetail_MuscleID"]));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditmuscleDetail_SubProcedureID"])))
            { cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ViewState["EditmuscleDetail_SubProcedureID"])); }
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["EditmuscleDetail_procedureID"] = null;
            ViewState["EditMuscleDetail_MuscleID"] = null;
            ViewState["EditmuscleDetail_SubProcedureID"] = null;
            txtEditdetails.Text = string.Empty;
            popupEditMuscleMedication.Hide();
        }
    }

    protected void btnsaveeditedmedication_Click(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_ModifyMedication", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditmedicationDetail_procedureID"])))
            { cmd.Parameters.AddWithValue("@procedureID", Convert.ToInt32(ViewState["EditmedicationDetail_procedureID"])); }
            cmd.Parameters.AddWithValue("@Medication", txtEditdetails.Text);
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditmedicationDetail_MedicationID"])))
            {
                cmd.Parameters.AddWithValue("@MedicationID", Convert.ToInt32(ViewState["EditmedicationDetail_MedicationID"]));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditmedicationDetail_SubProcedureID"])))
            { cmd.Parameters.AddWithValue("@SubProcedureID", Convert.ToInt32(ViewState["EditmedicationDetail_SubProcedureID"])); }
            con.Open();
            int count = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["EditmedicationDetail_procedureID"] = null;
            ViewState["EditmedicationDetail_MedicationID"] = null;
            ViewState["EditmedicationDetail_SubProcedureID"] = null;
            txtEditdetails.Text = string.Empty;
            popupEditMuscleMedication.Hide();
        }
    }
}