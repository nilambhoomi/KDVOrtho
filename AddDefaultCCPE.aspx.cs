using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddDefaultCCPE : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(AddDefaultCCPE));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["id"] != null)
                bindData();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        DBHelperClass db = new DBHelperClass();
        try
        {
            string query = "";
          

            if (Request["id"] != null)
            {
                query = "update tblDefaultCCPE set BodyPart='" + ddlbodyPart.SelectedItem.Value + "',Position='" + ddlposition.SelectedValue + "',CC='" + txtCC.Text + "',PE='" + txtPE.Text + "' where Id=" + Request["id"];
            }
            else
            {

                query = "insert into tblDefaultCCPE values ('" + ddlbodyPart.SelectedItem.Value + "','" + ddlposition.SelectedValue + "','" + txtCC.Text + "','" + txtPE.Text + "')";
            }

           
            int val = db.executeQuery(query);

            if (val > 0)
            {
                Response.Redirect("ViewDefaultCCPE.aspx");
            }

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }

    private void bindData()
    {
        DataSet lds = null;
        int totalrecords = 0;

        using (SqlConnection gConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            try
            {
                string cnd = " where Id=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_GetDefaultCCPE_paging", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", 1);
                gComm.Parameters.AddWithValue("@cnd", cnd);

                gComm.Parameters.AddWithValue("@ordercolumn", "BodyPart");
                gComm.Parameters.AddWithValue("@sortorder", "asc");

                gComm.Parameters.AddWithValue("@PageSize", 10);
                gComm.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                gComm.Parameters["@RecordCount"].Direction = ParameterDirection.Output;

                gConn.Open();
                SqlDataAdapter lda = new SqlDataAdapter(gComm);
                lds = new DataSet();
                lda.Fill(lds);




                totalrecords = Convert.ToInt32(gComm.Parameters["@RecordCount"].Value);
                gConn.Close();

                if (totalrecords > 0)
                {
                    txtCC.Text = lds.Tables[0].Rows[0]["CC"].ToString();
                    txtPE.Text = lds.Tables[0].Rows[0]["PE"].ToString();

                    ddlposition.ClearSelection();
                    ddlposition.Items.FindByValue(lds.Tables[0].Rows[0]["Position"].ToString()).Selected = true;

                    ddlbodyPart.ClearSelection();
                    ddlbodyPart.Items.FindByValue(lds.Tables[0].Rows[0]["BodyPart"].ToString()).Selected = true;

                }
            }

            catch (Exception ex)
            {
                gConn.Close();
                log.Error(ex.Message);
            }
        }

    }



}