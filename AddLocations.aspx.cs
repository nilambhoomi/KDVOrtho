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

public partial class AddLocations : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(AddLocations));
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

                param = new SqlParameter[15];
                SP = "[nusp_update_location]";
            }
            else
            {
                param = new SqlParameter[15];
                SP = "nusp_insert_location";
            }

            param[0] = new SqlParameter("@Location", txtLocation.Text);
            param[1] = new SqlParameter("@SetAsDefault", chkSetDefault.Checked);

            param[2] = new SqlParameter("@Address", txtAddress.Text);


            param[3] = new SqlParameter("@City", txtCity.Text);
            param[4] = new SqlParameter("@State", txtState.Text);
            param[5] = new SqlParameter("@Zip", txtZip.Text);
            param[6] = new SqlParameter("@EmailAddress", txtEmail.Text);

            param[7] = new SqlParameter("@Telephone", txtPhone.Text);
            param[8] = new SqlParameter("@ContactPerson", txtContacts.Text);

            param[9] = new SqlParameter("@NameOfPractice", txtNameOfPractice.Text);
            param[10] = new SqlParameter("@Fax", txtFax.Text);
            param[11] = new SqlParameter("@Is_Active", chkActive.Checked);
            param[12] = new SqlParameter("@DrFName", txtDFName.Text);
            param[13] = new SqlParameter("@DrLName", txtDLName.Text);

            if (Request["id"] != null)
            {
                param[14] = new SqlParameter("@Location_ID", Request["id"]);

            }
            else
            {
                param[14] = new SqlParameter("@CreatedBy", "Admin");
            }


            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                Response.Redirect("ViewLocations.aspx");
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
                string cnd = " where Location_ID=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_GetLocations_paging", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", 1);
                gComm.Parameters.AddWithValue("@cnd", cnd);

                gComm.Parameters.AddWithValue("@ordercolumn", "Location");
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
                    txtAddress.Text = lds.Tables[0].Rows[0]["Address"].ToString();
                    txtCity.Text = lds.Tables[0].Rows[0]["City"].ToString();
                    txtContacts.Text = lds.Tables[0].Rows[0]["ContactPerson"].ToString();
                    txtEmail.Text = lds.Tables[0].Rows[0]["EmailAddress"].ToString();
                    txtLocation.Text = lds.Tables[0].Rows[0]["Location"].ToString();
                    txtPhone.Text = lds.Tables[0].Rows[0]["Telephone"].ToString();
                    txtState.Text = lds.Tables[0].Rows[0]["State"].ToString();
                    txtZip.Text = lds.Tables[0].Rows[0]["Zip"].ToString();
                    txtNameOfPractice.Text = lds.Tables[0].Rows[0]["NameOfPractice"].ToString();
                    txtFax.Text = lds.Tables[0].Rows[0]["Fax"].ToString();
                    txtDFName.Text = lds.Tables[0].Rows[0]["DrFName"].ToString();
                    txtDLName.Text = lds.Tables[0].Rows[0]["DrLName"].ToString();
                    chkActive.Checked = Convert.ToBoolean(lds.Tables[0].Rows[0]["Is_Active"].ToString());
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
                gConn.Close();
            }
        }

    }
}