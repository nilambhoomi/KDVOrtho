using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Procedures : System.Web.UI.Page
{
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    BusinessLogic bl = new BusinessLogic();
    //    var PatientResult = bl.GetProceduresByBody("");

    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            BindProcudureDetails();
            Session["BodyPart"] = "ALL";
            // BodyPartDDN
        }
    }

    protected void BindProcudureDetails(string BodyPart = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_getProcedureCodesForEditByParts", con);

            if (!string.IsNullOrEmpty(BodyPart))
            {
                cmd.Parameters.AddWithValue("@BodyPart", BodyPart);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BodyPart", "ALL");
            }


            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            gvProcedureTbl.DataSource = dt;
            gvProcedureTbl.DataBind();
            //hfPatientId.Value = null;
        }
    }

    protected void BodyPartDDN_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        {
            hdn_ID.Value = row.Cells[0].Text;
            txtMCode.Text = row.Cells[1].Text;
            datetypedwn.SelectedValue = row.Cells[2].Text;
            //txtMCode.ReadOnly = true;
            txtHeading.Text = row.Cells[4].Text;
            txtPDesc.Text = row.Cells[5].Text;
           // txtCompany.Text = row.Cells[2].Text;
            popup.Show();
        }
    }

    protected void Save(object sender, EventArgs e)
    {
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            SqlCommand cmd = new SqlCommand("nusp_SaveProcedures", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Procedure_ID",Convert.ToInt64(hdn_ID.Value.Replace(",","")));
            cmd.Parameters.AddWithValue("@Headings", txtHeading.Text.Replace(",", ""));
            cmd.Parameters.AddWithValue("@MCODE", txtMCode.Text.Replace(",", "")+"_"+datetypedwn.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@PDesc", txtPDesc.Text.Replace(",", ""));
            cmd.Parameters.AddWithValue("@CreatedBy", "Admin");
            con.Open();           
            int count= cmd.ExecuteNonQuery();
            BindProcudureDetails("ALL");
            popup.Hide();
        }
    }
    protected void gvPatientDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvProcedureTbl.PageIndex = e.NewPageIndex;
        BindProcudureDetails(Session["BodyPart"].ToString());
    }

   
    protected void BodyPartDDN_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Session["BodyPart"] = BodyPartDDN.SelectedValue.ToString();
        BindProcudureDetails(BodyPartDDN.SelectedValue.ToString());
    }
}