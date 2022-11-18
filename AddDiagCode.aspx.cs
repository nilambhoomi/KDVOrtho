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

public partial class AddDiagCode : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(AddDiagCode));
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

                param = new SqlParameter[5];
                SP = "[nusp_Update_DiagCodes]";
            }
            else
            {
                param = new SqlParameter[5];
                SP = "nusp_Insert_DiagCodes";
            }

            param[0] = new SqlParameter("@BodyPart", txtBodyPart.Text);
            param[1] = new SqlParameter("@DiagCode", txtDiagCode.Text);
            param[2] = new SqlParameter("@Description", txtDescription.Text);
            param[3] = new SqlParameter("@PreSelect", chkPreSelect.Checked);



            if (Request["id"] != null)
            {
                param[4] = new SqlParameter("@DiagCode_ID", Request["id"]);

            }
            else
            {
                param[4] = new SqlParameter("@CreatedBy", "Admin");
            }


            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                Response.Redirect("ViewDiagCodes.aspx");
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
                string cnd = " where DiagCode_ID=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_GetDiagCodes_paging", gConn);

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
                    txtBodyPart.Text = lds.Tables[0].Rows[0]["BodyPart"].ToString();
                    txtDescription.Text = lds.Tables[0].Rows[0]["Description"].ToString();
                    txtDiagCode.Text = lds.Tables[0].Rows[0]["DiagCode"].ToString();
                    chkPreSelect.Checked = string.IsNullOrEmpty(lds.Tables[0].Rows[0]["PreSelect"].ToString()) ? false : Convert.ToBoolean(lds.Tables[0].Rows[0]["PreSelect"].ToString());  
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