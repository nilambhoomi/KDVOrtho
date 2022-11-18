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

public partial class AddMedicine : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(AddMedicine));
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

                param = new SqlParameter[2];
                SP = "[nusp_Update_Medicine]";
            }
            else
            {
                param = new SqlParameter[2];
                SP = "nusp_Insert_Medicine";
            }

            param[0] = new SqlParameter("@Medicine", txtMedicine.Text);



            if (Request["id"] != null)
            {
                param[1] = new SqlParameter("@Medicine_ID", Request["id"]);


            }
            else
            {
                param[1] = new SqlParameter("@CreatedBy", "Admin");
            }


            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                Response.Redirect("ViewMedicine.aspx");
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
                string cnd = " where Medicine_ID=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_Medicine_paging", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", 1);
                gComm.Parameters.AddWithValue("@cnd", cnd);

                gComm.Parameters.AddWithValue("@ordercolumn", "Medicine");
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
                    txtMedicine.Text = lds.Tables[0].Rows[0]["Medicine"].ToString();
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