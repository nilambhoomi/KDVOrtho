using IntakeSheet.BLL;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddPatientMaster : System.Web.UI.Page
{

    ILog log = log4net.LogManager.GetLogger(typeof(AddPatientMaster));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request["id"] != null)
            {
                bindData();
                BindPatientIEDetails(Request["id"].ToString());
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int age = 0;
        DBHelperClass db = new DBHelperClass();
        try
        {
            string SP = "";
            SqlParameter[] param = null;

            if (Request["id"] != null)
            {

                param = new SqlParameter[21];
                SP = "nusp_Update_Patient";
            }
            else
            {
                param = new SqlParameter[22];
                SP = "nusp_Insert_Patient";
            }

            param[0] = new SqlParameter("@Sex", ddlSex.SelectedValue);
            param[1] = new SqlParameter("@FirstName", txtFirstName.Text);
            param[2] = new SqlParameter("@LastName", txtLastName.Text);
            param[3] = new SqlParameter("@MiddleName", txtMiddleName.Text);


            if (string.IsNullOrWhiteSpace(txtDOB.Text))
            {
                param[4] = new SqlParameter("@DOB", DBNull.Value);
            }
            else
            {
                age = System.DateTime.Now.Year - Convert.ToDateTime(txtDOB.Text).Year;
                param[4] = new SqlParameter("@DOB", Convert.ToDateTime(txtDOB.Text));
            }


            param[5] = new SqlParameter("@AGE", age);
            param[6] = new SqlParameter("@Handedness", ddlHandedness.SelectedItem.Text);
            param[7] = new SqlParameter("@Phone", txtPhone1.Text);
            param[8] = new SqlParameter("@Photo", fupImg.FileName);

            param[9] = new SqlParameter("@AccountNumber", txtAccountNo.Text);
            param[10] = new SqlParameter("@Address1", txtAdd1.Text);
            param[11] = new SqlParameter("@Address2", txtAdd2.Text);
            param[12] = new SqlParameter("@City", txtCity.Text);
            param[13] = new SqlParameter("@State", txtState.Text);
            param[14] = new SqlParameter("@Zip", txtZip.Text);
            param[15] = new SqlParameter("@Phone2", txtPhone2.Text);
            param[16] = new SqlParameter("@eMail", txtEmail.Text);
            param[17] = new SqlParameter("@SSN", txtSSN.Text);
            param[18] = new SqlParameter("@policy_no", txtPolicyNo.Text);
            param[19] = new SqlParameter("@work_phone", txtWorkPhone.Text);

            if (Request["id"] == null)
            {
                param[20] = new SqlParameter("@UpdateFlag", "New");
                param[21] = new SqlParameter("@CreatedBy", "");
            }
            else
            {
                param[20] = new SqlParameter("@Patient_ID", Request["id"]);
            }

            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                BindPatientIEDetails(Request["id"]);
                Response.Redirect("ViewPatientMaster.aspx");
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
                string cnd = " where Patient_ID=" + Request["id"];
                SqlCommand gComm = new SqlCommand("nusp_GetPatientMaster", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", 1);
                gComm.Parameters.AddWithValue("@cnd", cnd);

                gComm.Parameters.AddWithValue("@ordercolumn", "FirstName");
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
                    txtAccountNo.Text = lds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    txtAdd1.Text = lds.Tables[0].Rows[0]["Address1"].ToString();
                    txtAdd2.Text = lds.Tables[0].Rows[0]["Address2"].ToString();
                    txtCity.Text = lds.Tables[0].Rows[0]["City"].ToString();

                    txtDOB.Text = string.IsNullOrEmpty(lds.Tables[0].Rows[0]["DOB"].ToString()) ? "" : Convert.ToDateTime(lds.Tables[0].Rows[0]["DOB"].ToString()).ToString("MM/dd/yyyy");
                    txtEmail.Text = lds.Tables[0].Rows[0]["eMail"].ToString();
                    txtFirstName.Text = lds.Tables[0].Rows[0]["FirstName"].ToString();
                    txtLastName.Text = lds.Tables[0].Rows[0]["LastName"].ToString();
                    txtMiddleName.Text = lds.Tables[0].Rows[0]["MiddleName"].ToString();
                    txtPhone1.Text = lds.Tables[0].Rows[0]["Phone"].ToString();
                    txtPhone2.Text = lds.Tables[0].Rows[0]["Phone2"].ToString();
                    txtPolicyNo.Text = lds.Tables[0].Rows[0]["policy_no"].ToString();
                    txtSSN.Text = lds.Tables[0].Rows[0]["SSN"].ToString();
                    txtState.Text = lds.Tables[0].Rows[0]["State"].ToString();
                    txtWorkPhone.Text = lds.Tables[0].Rows[0]["work_phone"].ToString();
                    txtZip.Text = lds.Tables[0].Rows[0]["Zip"].ToString();

                    if (!string.IsNullOrEmpty(lds.Tables[0].Rows[0]["Handedness"].ToString()))
                    {
                        ddlHandedness.ClearSelection();
                        ddlHandedness.Items.FindByText(lds.Tables[0].Rows[0]["Handedness"].ToString()).Selected = true;
                    }
                    ddlSex.ClearSelection();
                    ddlSex.Items.FindByValue(lds.Tables[0].Rows[0]["Sex"].ToString()).Selected = true;

                    ViewState["img"] = lds.Tables[0].Rows[0]["Photo"].ToString();

                }
            }

            catch (Exception ex)
            {
                gConn.Close();
                log.Error(ex.Message);
            }
        }

    }

    protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtDOB.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }

    protected void BindPatientIEDetails(string patientId = null, string searchText = null)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_GetPatientIEDetails", con);


            cmd.Parameters.AddWithValue("@Patient_Id", patientId);
            cmd.Parameters.AddWithValue("@SearchText", null);
            cmd.Parameters.AddWithValue("@LocationId", null);
            cmd.Parameters.AddWithValue("@SDate", null);
            cmd.Parameters.AddWithValue("@EDate", null);



            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            string _query = "";
            DataRow row;


            try
            {
                dt = dt.Select(_query).CopyToDataTable();
                DataView dv = dt.DefaultView;
                dv.Sort = "LastTestDate desc";
                dt = dv.ToTable();
            }
            catch (Exception ex)
            {
                dt = null;
            }


            con.Close();
            Session["iedata"] = dt;

            gvPatientDetails.DataSource = dt;
            gvPatientDetails.DataBind();

        }
    }

    protected void gvPatientDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string patientIEId = gvPatientDetails.DataKeys[e.Row.RowIndex].Value.ToString();
            BusinessLogic bl = new BusinessLogic();
            GridView gvPatientFUDetails = e.Row.FindControl("gvPatientFUDetails") as GridView;

            System.Web.UI.WebControls.Image img = e.Row.FindControl("plusimg") as System.Web.UI.WebControls.Image;



            gvPatientFUDetails.ToolTip = patientIEId;
            gvPatientFUDetails.DataSource = bl.GetFUDetails(Convert.ToInt32(patientIEId));
            gvPatientFUDetails.DataBind();

            if (gvPatientFUDetails.Rows.Count == 0)
                img.Attributes.Add("style", "display:none");
            else
                img.Attributes.Add("style", "display:block");
        }
    }
}