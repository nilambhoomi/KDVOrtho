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

public partial class AddAttorney : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(AddAttorney));
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
            string SP = "";
            SqlParameter[] param = null;

            if (Request["id"] != null)
            {

                param = new SqlParameter[10];
                SP = "nusp_Update_Attorney";
            }
            else
            {
                param = new SqlParameter[10];
                SP = "nusp_Insert_Attorney";
            }

            param[0] = new SqlParameter("@Attorney", txtAttorny.Text);
            param[1] = new SqlParameter("@Address1", txtAddress1.Text);

            param[2] = new SqlParameter("@Address2", txtAddress2.Text);


            param[3] = new SqlParameter("@City", txtCity.Text);
            param[4] = new SqlParameter("@State", txtState.Text);
            param[5] = new SqlParameter("@Zip", txtZip.Text);
            param[6] = new SqlParameter("@EmailAddress", txtEmail.Text);

            param[7] = new SqlParameter("@Telephone", txtPhone.Text);
            param[8] = new SqlParameter("@ContactPerson", txtContacts.Text);


            if (Request["id"] != null)
            {
                param[9] = new SqlParameter("@Attorney_ID", Request["id"]);

            }
            else
            {
                param[9] = new SqlParameter("@CreatedBy", "Admin");
            }


            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                Response.Redirect("ViewAttorney.aspx");
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
                string cnd = " where Attorney_ID=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_GetAttorney_paging", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", 1);
                gComm.Parameters.AddWithValue("@cnd", cnd);

                gComm.Parameters.AddWithValue("@ordercolumn", "Attorney");
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
                    txtAddress1.Text = lds.Tables[0].Rows[0]["Address1"].ToString();
                    txtAddress2.Text = lds.Tables[0].Rows[0]["Address2"].ToString();
                    txtCity.Text = lds.Tables[0].Rows[0]["City"].ToString();
                    txtContacts.Text = lds.Tables[0].Rows[0]["ContactPerson"].ToString();
                    txtEmail.Text = lds.Tables[0].Rows[0]["EmailAddress"].ToString();

                    txtAttorny.Text = lds.Tables[0].Rows[0]["Attorney"].ToString();
                    txtPhone.Text = lds.Tables[0].Rows[0]["Telephone"].ToString();
                    txtState.Text = lds.Tables[0].Rows[0]["State"].ToString();
                    txtZip.Text = lds.Tables[0].Rows[0]["Zip"].ToString();
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