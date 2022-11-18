using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Globalization;

public partial class PatientDetails : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string filePath = Server.MapPath("~/Xml/USStates.xml");
            using (DataSet ds1 = new DataSet())
            {
                ds1.ReadXml(filePath);
                ddState.DataSource = ds1;
                ddState.DataTextField = "name";
                ddState.DataValueField = "name";
                ddState.DataBind();
                ddState.Items.Insert(0, new ListItem("--Select State--", "0"));
                ddState.SelectedItem.Value = "0";

                ddlPatientEmploymentState.DataSource = ds1;
                ddlPatientEmploymentState.DataTextField = "name";
                ddlPatientEmploymentState.DataValueField = "name";
                ddlPatientEmploymentState.DataBind();
                ddlPatientEmploymentState.Items.Insert(0, new ListItem("--Select State--", "0"));
                ddlPatientEmploymentState.SelectedItem.Value = "0";
            }
        }
    }
    protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txt_DOB.Text = d.ToShortDateString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string SP = "";
            SqlParameter[] param = null;

            param = new SqlParameter[25];
            SP = "usp_insert_PatientDetails";
            param[0] = new SqlParameter("@FirstName", txt_fname.Text.ToLower());
            param[1] = new SqlParameter("@LastName", txt_lname.Text.ToLower());
            param[2] = new SqlParameter("@MiddleName", txt_mname.Text.ToLower());
            param[3] = new SqlParameter("@Sex", ddl_gender.SelectedItem.Value);
            param[4] = new SqlParameter("@Address", txt_add.Text.Trim().ToLower());
            param[5] = new SqlParameter("@City", txt_city.Text.Trim());
            param[6] = new SqlParameter("@State", ddState.SelectedItem.Value);
            param[7] = new SqlParameter("@Zip", txt_zip.Text.Trim());
            param[8] = new SqlParameter("@RaceEthnicity", txtRaceEthnicity.Text.Trim());
            param[9] = new SqlParameter("@Language", txtLanguage.Text.Trim());
            param[10] = new SqlParameter("@HomePhno", txt_home_ph.Text.Trim());
            param[11] = new SqlParameter("@MobilePhno", txt_mobile.Text.Trim());
            param[12] = new SqlParameter("@eMail", txtEmail.Text.Trim());
            param[13] = new SqlParameter("@SSN", txt_SSN.Text.Trim());
            param[14] = new SqlParameter("@MaritalStatus", ddlMaratialStatus.SelectedItem.Value);
            param[15] = new SqlParameter("@PatientEmployment", txtPatientEmployment.Text.Trim());
            param[16] = new SqlParameter("@PEAddress", txt_zip.Text.Trim());
            param[17] = new SqlParameter("@PECity", txtPatientEmploymentCity.Text.Trim());
            param[18] = new SqlParameter("@PEState", ddlPatientEmploymentState.SelectedItem.Value);
            param[19] = new SqlParameter("@PEZip", txtPatientEmploymentZip.Text.Trim());
            param[20] = new SqlParameter("@RelationToPatient", txtRelation.Text.Trim());
            param[21] = new SqlParameter("@PEPhno", txtRelationPhno.Text.Trim());
            param[22] = new SqlParameter("@NextOfKinEmergency", txtNextOfKinEmergency.Text.Trim());
            param[23] = new SqlParameter("@Description", txtDescription.Text.Trim());

            if (string.IsNullOrWhiteSpace(txt_DOB.Text))
            {
                param[24] = new SqlParameter("@DOB", DBNull.Value);
            }
            else
            {
                param[24] = new SqlParameter("@DOB", Convert.ToDateTime(txt_DOB.Text));
            }

            int val = db.executeSP(SP, param);

            if (val > 0)
            {
                lblmess.Text = "Patient Details Recorded Sucessfully.";
                lblmess.ForeColor = System.Drawing.Color.Green;

                lblMessage.Attributes.Add("style", "color:green");
                upMessage.Update();
                DataSet locds = new DataSet();

            }
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
        finally
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "openPopup('mymodelmessage')", true);
            ClearControls();
        }
    }
    private void ClearControls()
    {
        try
        {

            ddlPatientEmploymentState.SelectedItem.Value = "0";
            ddState.SelectedItem.Value = "0";
            txtPatientEmploymentAddress.Text = string.Empty;
            txt_fname.Text = string.Empty;
            txt_lname.Text = string.Empty;
            txt_mname.Text = string.Empty;
            ddl_gender.SelectedItem.Value = "single"; ;
            txt_add.Text = string.Empty;
            txt_city.Text = string.Empty;
            txt_zip.Text = string.Empty;
            txtRaceEthnicity.Text = string.Empty;
            txtLanguage.Text = string.Empty;
            txt_home_ph.Text = string.Empty;
            txt_mobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txt_SSN.Text = string.Empty;
            ddlMaratialStatus.SelectedItem.Value = "0";
            txtPatientEmployment.Text = string.Empty;
            txt_zip.Text = string.Empty;
            txtPatientEmploymentCity.Text = string.Empty;
            txtPatientEmploymentZip.Text = string.Empty;
            txtRelation.Text = string.Empty;
            txtRelationPhno.Text = string.Empty;
            txtNextOfKinEmergency.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txt_DOB.Text = string.Empty;
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }


    }
}