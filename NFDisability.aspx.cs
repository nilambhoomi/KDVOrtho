using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NFDisability : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {

            txt_sign.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            if (Request["id"] != null)
            {
                bindEditData(Request.QueryString["id"]);
            }
            else
            {
                if (Session["PatientIE_ID"] != null)
                    bindEditData(Session["PatientIE_ID"].ToString());
            }

        }
    }
    [WebMethod]
    public static string[] getFirstName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID,FirstName from tblPatientMaster where FirstName like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                fname = ds.Tables[0].Rows[i]["FirstName"].ToString();
                patient.Add(string.Format("{0}-{1}", fname, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }

        return patient.ToArray();
    }

    [WebMethod]
    public static string[] getLastName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID,LastName from tblPatientMaster where LastName like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string fname = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                fname = ds.Tables[0].Rows[i]["LastName"].ToString();
                patient.Add(string.Format("{0}-{1}", fname, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }

        return patient.ToArray();
    }
    private void bindData()
    {

        DataSet ds = db.Patientmaster_autocomplete(txt_fname.Text, txt_lname.Text);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txt_DOB.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
        }
    }
    private void bindEditData(string PatientIEid)
    {
        try
        {
            Session["PatientIE_ID"] = PatientIEid;
            string query = "select * from View_PatientIE where PatientIE_ID=" + PatientIEid;

            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_fname.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                txt_fname2.Text = ds.Tables[0].Rows[0]["FirstName"].ToString();
                txt_lname.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                txt_lname2.Text = ds.Tables[0].Rows[0]["LastName"].ToString();
                txt_SSN.Text = ds.Tables[0].Rows[0]["SSN"].ToString();
                ddl_gender.ClearSelection();
                ddl_gender.Items.FindByValue(ds.Tables[0].Rows[0]["Sex"].ToString()).Selected = true;
                txt_add.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                txt_mobile.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                if (ds.Tables[0].Rows[0]["DOB"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString());
                    txt_DOB.Text = dob.ToString("dd/MM/yyyy");
                }


            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void txt_lname_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_fname.Text))
        {
            bindData();

        }
    }
    protected void txt_fname_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_lname.Text))
        {
            bindData();
        }
    }
   
    protected void btnExportPDF_Click(object sender, EventArgs e)
    {

        var pdfPath = Path.Combine(Server.MapPath("~/Template/DisabilityLetterNF-edit.pdf"));
        var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);



        //this is start for the disable letter.
        formFieldMap["txt_date"] = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
        formFieldMap["txt_name"] = Convert.ToString(txt_fname.Text);
        //this is end for the disable letter.
        formFieldMap["txt_fname"] = Convert.ToString(txt_fname.Text);
        formFieldMap["txt_mname"] = Convert.ToString("");
        formFieldMap["txt_lname"] = Convert.ToString(txt_lname.Text);
        formFieldMap["txt_add"] = Convert.ToString(txt_add.Text);
        formFieldMap["txt_telno"] = Convert.ToString(txt_mobile.Text);
        formFieldMap["txt_dob"] = Convert.ToString(txt_DOB.Text);
        string ssn = txt_SSN.Text;
        string ssn1 = ssn.Replace("-","");
        string separated = new string(
                                         ssn1.Select((x, i) => i > 0 && i % 1 == 0 ? new[] { ',', x } : new[] { x })
                                            .SelectMany(x => x)
                                            .ToArray()
                                             );
        if (string.IsNullOrEmpty(separated))
        {
        }
        else
        {
            int[] a = separated.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            if (a[0] == null)
            {
            }
            else
            {
                formFieldMap["1"] = Convert.ToString(a[0]);
            }
            if (a[1] == null)
            {
            }
            else
            {
                formFieldMap["2"] = Convert.ToString(a[1]);
            }
            if (a[2] == null)
            {
            }
            else
            {
                formFieldMap["3"] = Convert.ToString(a[2]);
            }
            if (a[3] == null)
            {
            }
            else
            {
                formFieldMap["4"] = Convert.ToString(a[3]);
            }
            if (a[4] == null)
            {
            }
            else
            {
                formFieldMap["5"] = Convert.ToString(a[4]);
            }
            if (a[5] == null)
            {
            }
            else
            {
                formFieldMap["6"] = Convert.ToString(a[5]);
            }
            if (a[6] == null)
            {
            }
            else
            {
                formFieldMap["7"] = Convert.ToString(a[6]);
            }
            if (a[7] == null)
            {
            }
            else
            {
                formFieldMap["8"] = Convert.ToString(a[7]);
            }
            if (a[8] == null)
            {
            }
            else
            {
                formFieldMap["9"] = Convert.ToString(a[8]);
            }
        }

        formFieldMap["txt_c_fname"] = Convert.ToString(txt_fname2.Text);
        formFieldMap["txt_c_lname"] = Convert.ToString(txt_lname2.Text);
        if (ddl_gender.SelectedValue.ToString() == "M")
        {
            formFieldMap["chk_Male"]="chk_Male.Checked";
        }
        else
        {
            formFieldMap["chk_Female"] = "chk_Female.Checked";
        }
       
        formFieldMap["txt_claimdate"] = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
        formFieldMap["txt_c_dob"] = Convert.ToString(txt_DOB.Text);
        var pdfContents = PDFHelper.GeneratePDF(pdfPath,formFieldMap);

        PDFHelper.ReturnPDF(pdfContents, "db450-" + txt_fname.Text + txt_lname.Text + Convert.ToString(System.DateTime.Now.ToString("ddMMyy"))+ "-.pdf");


    }
}