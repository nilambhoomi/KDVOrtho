using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProcedureView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            BindProcudureDetails();
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
    protected void BodyPartDDN_SelectedIndexChanged1(object sender, EventArgs e)
    {
        BindProcudureDetails(BodyPartDDN.SelectedIndex.ToString());
    }
}