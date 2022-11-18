using System.Configuration;
using System.Data.OleDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iTextSharp.text.pdf;
public partial class batchprint : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            string FilePath = Server.MapPath(FolderPath + FileName);
            FileUpload1.SaveAs(FilePath);
            Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
        }
    }
    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls": //Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                break;
            case ".xlsx": //Excel 07
                conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                break;
        }
        conStr = String.Format(conStr, FilePath, isHDR);
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;

        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();

        //Read Data from First Sheet
        connExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        connExcel.Close();



        string query = "select * from View_PatientIE pm ";

        foreach (DataRow row in dt.Rows)
        {
            int index = dt.Rows.IndexOf(row);
            if (index < 1)
            {
                query += "Where (pm.FirstName='" + Convert.ToString(row.ItemArray[0]).Trim() + "' and pm.LastName='" + Convert.ToString(row.ItemArray[1]).Trim() + "')";
            }
            else
            {
                query += " OR (pm.FirstName='" + Convert.ToString(row.ItemArray[0]).Trim() + "' and pm.LastName='" + Convert.ToString(row.ItemArray[1]).Trim() + "')";
            }
        }




        //string query = "select * from View_PatientIE where PatientIE_ID=" + PatientIEid;
        //string query1= "select *, STUFF((select ','+MCODE from tblProceduresDetail  where PatientIE_ID = ve.PatientIE_ID FOR XML PATH ('')), 1, 1, '') 'Mcode',ve.WCBGroup from View_PatientIE ve left join tblPatientIEDetailPage2 p1 on p1.PatientIE_ID=ve.PatientIE_ID where ve.PatientIE_ID=" + PatientIEid;

        DataSet ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string foldername = "~/Bulkfileprited/" + Convert.ToString(Guid.NewGuid());



            if (!Directory.Exists(foldername))
                Directory.CreateDirectory(Server.MapPath(foldername));
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                // start fetching the records from the datatbale.


                int index = ds.Tables[0].Rows.IndexOf(row);
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["FirstName"].ToString()))
                {
                    Session["fname"] = " ";
                }
                else
                {
                    Session["fname"] = ds.Tables[0].Rows[index]["FirstName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["LastName"].ToString()))
                {
                    Session["lname"] = " ";
                }
                else
                {
                    Session["lname"] = ds.Tables[0].Rows[index]["LastName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["MiddleName"].ToString()))
                {
                    Session["mname"] = " ";
                }
                else
                {
                    Session["mname"] = ds.Tables[0].Rows[index]["MiddleName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["eMail"].ToString()))
                {
                    Session["eMail"] = " ";
                }
                else
                {
                    Session["eMail"] = ds.Tables[0].Rows[index]["eMail"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["DOA"].ToString()))
                {
                    Session["doa"] = " ";
                }
                else
                {
                    Session["doa"] = ds.Tables[0].Rows[index]["DOA"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["DOE"].ToString()))
                {
                    Session["doe"] = " ";
                }
                else
                {
                    Session["doe"] = ds.Tables[0].Rows[index]["DOE"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["SSN"].ToString()))
                {
                    Session["ssn"] = " ";
                }
                else
                {
                    Session["ssn"] = ds.Tables[0].Rows[index]["SSN"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Address1"].ToString()))
                {
                    Session["Address"] = " ";
                }
                else
                {
                    Session["Address"] = ds.Tables[0].Rows[index]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["InsAddress1"].ToString()))
                {
                    Session["InsAddress"] = " ";
                }
                else
                {
                    Session["InsAddress"] = ds.Tables[0].Rows[index]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Phone2"].ToString()))
                {
                    Session["mob"] = " ";
                }
                else
                {
                    Session["mob"] = ds.Tables[0].Rows[index]["Phone2"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["InsPhone"].ToString()))
                {
                    Session["InsPhone"] = " ";
                }
                else
                {
                    Session["InsPhone"] = ds.Tables[0].Rows[index]["InsPhone"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["City"].ToString()))
                {
                    Session["city"] = " ";
                }
                else
                {
                    Session["city"] = ds.Tables[0].Rows[index]["City"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["InsCity"].ToString()))
                {
                    Session["Inscity"] = " ";
                }
                else
                {
                    Session["Inscity"] = ds.Tables[0].Rows[index]["InsCity"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["State"].ToString()))
                {
                    Session["state"] = " ";
                }
                else
                {
                    Session["state"] = ds.Tables[0].Rows[index]["State"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["InsState"].ToString()))
                {
                    Session["Insstate"] = " ";
                }
                else
                {
                    Session["Insstate"] = ds.Tables[0].Rows[index]["InsState"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Zip"].ToString()))
                {
                    Session["zip"] = " ";
                }
                else
                {
                    Session["zip"] = ds.Tables[0].Rows[index]["Zip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["InsZip"].ToString()))
                {
                    Session["Inszip"] = " ";
                }
                else
                {
                    Session["Inszip"] = ds.Tables[0].Rows[index]["InsZip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Phone"].ToString()))
                {
                    Session["Phone"] = " ";
                }
                else
                {
                    Session["Phone"] = ds.Tables[0].Rows[index]["Phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["work_phone"].ToString()))
                {
                    Session["work_phone"] = " ";
                }
                else
                {
                    Session["work_phone"] = ds.Tables[0].Rows[index]["work_phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Sex"].ToString()))
                {
                    Session["sex"] = " ";
                }
                else
                {
                    Session["sex"] = ds.Tables[0].Rows[index]["Sex"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["InsCo"].ToString()))
                {
                    Session["InsCo"] = " ";
                }
                else
                {
                    Session["InsCo"] = ds.Tables[0].Rows[index]["InsCo"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["policy_no"].ToString()))
                {
                    Session["policy_no"] = " ";
                }
                else
                {
                    Session["policy_no"] = ds.Tables[0].Rows[index]["policy_no"].ToString();
                }


                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["ClaimNumber"].ToString()))
                {
                    Session["ClaimNumber"] = " ";
                }
                else
                {
                    Session["ClaimNumber"] = ds.Tables[0].Rows[index]["ClaimNumber"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Location"].ToString()))
                {
                    Session["LocationPdf"] = " ";
                }
                else
                {
                    Session["LocationPdf"] = ds.Tables[0].Rows[index]["Location"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Attorney"].ToString()))
                {
                    Session["Attorney"] = " ";
                }
                else
                {
                    Session["Attorney"] = ds.Tables[0].Rows[index]["Attorney"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["AttorneyAdd"].ToString()))
                {
                    Session["AttorneyAdd"] = " ";
                }
                else
                {
                    Session["AttorneyAdd"] = ds.Tables[0].Rows[index]["AttorneyAdd"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["AttorneyPhno"].ToString()))
                {
                    Session["AttorneyPhno"] = " ";
                }
                else
                {
                    Session["AttorneyPhno"] = ds.Tables[0].Rows[index]["AttorneyPhno"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Adjuster"].ToString()))
                {
                    Session["Adjuster"] = " ";
                }
                else
                {
                    Session["Adjuster"] = ds.Tables[0].Rows[index]["Adjuster"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Compensation"].ToString()))
                {
                    Session["Compensation"] = " ";
                }
                else
                {
                    Session["Compensation"] = ds.Tables[0].Rows[index]["Compensation"].ToString();
                }
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["AGE"].ToString()))
                //{
                //Session["AGE"] = " ";
                //}
                //else
                //{
                //Session["AGE"] = ds.Tables[0].Rows[index]["AGE"].ToString();
                //}

                if (ds.Tables[0].Rows[index]["DOB"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[index]["DOB"].ToString());
                    Session["dob"] = dob.ToString("MM/dd/yyyy");
                    //Session["AGE"] = CalculateAge(dob);
                }
                else
                {
                    Session["dob"] = " ";
                    Session["AGE"] = " ";
                }

                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["PMH"].ToString()))
                //{
                //    Session["PMH"] = "";
                //}
                //else
                //{
                //    Session["PMH"] = ds.Tables[0].Rows[index]["PMH"].ToString();
                //}
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["PSH"].ToString()))
                //{
                //    Session["PSH"] = "";
                //}
                //else
                //{
                //    Session["PSH"] = ds.Tables[0].Rows[index]["PSH"].ToString();
                //}
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Allergies"].ToString()))
                //{
                //    Session["Allergies"] = "";
                //}
                //else
                //{
                //    Session["Allergies"] = ds.Tables[0].Rows[index]["Allergies"].ToString();
                //}
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Medications"].ToString()))
                //{
                //    Session["Medications"] = "";
                //}
                //else
                //{
                //    Session["Medications"] = ds.Tables[0].Rows[index]["Medications"].ToString();
                //}

                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["Mcode"].ToString()))
                //{
                //    Session["Mcode"] = "";
                //}
                //else
                //{
                //    Session["Mcode"] = ds.Tables[0].Rows[index]["Mcode"].ToString();
                //}
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[index]["WCBGroup"].ToString()))
                {
                    Session["WCBGroup"] = "";
                }
                else
                {
                    Session["WCBGroup"] = ds.Tables[0].Rows[index]["WCBGroup"].ToString();
                }

                // generate files.

                var pdfPath = Path.Combine(Server.MapPath("~/TemplateStore\\BS\\Demo.pdf"));
                string fullName = string.Empty;
                var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);
                if (string.IsNullOrEmpty(txt_date.Text))
                {
                    formFieldMap["txt_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                }
                else
                {
                    formFieldMap["txt_date"] = txt_date.Text;
                }
                Label names = new Label();
                Label fpname = new Label();
                Label lpname = new Label();
                string name = "Demo.pdf";
                names.Text = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
                fpname.Text = Convert.ToString(Session["fname"]);
                lpname.Text = Convert.ToString(Session["lname"]);
                formFieldMap["txt_name"] = names.Text;
                formFieldMap["txt_eMail"] = Convert.ToString(Session["eMail"]);
                formFieldMap["txt_city"] = Convert.ToString(Session["city"]);
                formFieldMap["txt_Inscity"] = Convert.ToString(Session["Inscity"]);
                formFieldMap["txt_state"] = Convert.ToString(Session["state"]);
                formFieldMap["txt_Insstate"] = Convert.ToString(Session["Insstate"]);
                formFieldMap["txt_zip"] = Convert.ToString(Session["zip"]);
                formFieldMap["txt_Inszip"] = Convert.ToString(Session["Inszip"]);
                if (Session["Phone"] != null)
                    if (Session["Phone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_Phone2"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_Phone2"] = Session["Phone"].ToString();
                    }
                if (Session["work_phone"] != null)
                    if (Session["work_phone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_work_phone"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_work_phone"] = Session["work_phone"].ToString();
                    }
                if (Session["InsPhone"] != null)
                    if (Session["InsPhone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_InsPhone"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_InsPhone"] = Session["InsPhone"].ToString();
                    }
                if (Session["ssn"] != null)
                    if (Session["ssn"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_ssn"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_ssn"] = Session["ssn"].ToString();
                        //formFieldMap["txt_ssn"] = "";
                    }
                formFieldMap["txt_InsCo"] = Convert.ToString(Session["InsCo"]);
                formFieldMap["txt_ClaimNumber"] = Convert.ToString(Session["ClaimNumber"]);
                formFieldMap["txt_admitting_surgeon"] = "Ketan D. Vora, D.O.";
                if (!string.IsNullOrEmpty(txt_docName.Text))
                {
                    formFieldMap["txt_admitting_surgeon"] = Convert.ToString(txt_docName.Text);
                }
                formFieldMap["txt_admitting_surgeon_ppc"] = "Gurbir Johal, MD";
                formFieldMap["txt_contact_persion_at_clinic"] = "Irena Shmagin";
                formFieldMap["txt_phnodr"] = "(718)-496-9950";
                formFieldMap["txt_Referring_Physician_Phone"] = "877-774-6337";
                formFieldMap["txt_H_C_Provider_Name"] = "Ketan D. Vora, D.O.";
                formFieldMap["txt_License_State_Of"] = " New York";
                formFieldMap["txt_License_Number"] = "243182";
                formFieldMap["chk_2"] = "true";
                //formFieldMap["chk_2"] = "Checked";
                formFieldMap["txt_Referring_Clinic"] = Convert.ToString(Session["LocationPdf"]);
                formFieldMap["txt_Referring_Physician"] = "Ketan D. Vora, D.O.";
                formFieldMap["txt_Referring_Physician_ppc"] = "Gurbir Johal, MD";
                formFieldMap["txt_phnodrppc"] = "(732)-887-2004";
                formFieldMap["txt_c_fname"] = Convert.ToString(Session["fname"]);
                formFieldMap["txt_c_lname"] = Convert.ToString(Session["lname"]);
                formFieldMap["txt_fname"] = Convert.ToString(Session["fname"]);
                formFieldMap["txt_mname"] = Convert.ToString(Session["mname"]);
                formFieldMap["txt_lname"] = Convert.ToString(Session["lname"]);
                formFieldMap["txt_address"] = Convert.ToString(Session["Address"]);

                formFieldMap["txt_addressCityStateZip"] = (!string.IsNullOrEmpty(Convert.ToString(Session["Address"])) ? Convert.ToString(Session["Address"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["city"])) ? " ," + Convert.ToString(Session["city"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["state"])) ? " ," + Convert.ToString(Session["state"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["zip"])) ? " ," + Convert.ToString(Session["zip"]) : string.Empty);
                formFieldMap["txt_Insaddress"] = Convert.ToString(Session["InsAddress"]);
                //if (string.IsNullOrWhiteSpace(Session["AGE"].ToString()))
                //{
                //    formFieldMap["txt_age"] = "";
                //}
                //else
                //{
                //    formFieldMap["txt_age"] = Convert.ToString(Session["AGE"]);
                //}

                if (Session["mob"] != null)
                    if (Session["mob"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_mob"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_mob"] = Session["mob"].ToString();
                    }
                if (Session["dob"] != null)
                    if (string.IsNullOrWhiteSpace(Session["dob"].ToString()))
                    {
                    }
                    else
                    {
                        formFieldMap["txt_dob"] = Session["dob"].ToString();
                        DateTime dob;
                        if (Session["dob"] != null && DateTime.TryParseExact(Session["dob"].ToString(), "MM-dd-yyyy", null, DateTimeStyles.None, out dob))
                        {

                            formFieldMap["txtdaydob"] = Convert.ToString(dob.Day);
                            formFieldMap["txtmonthdob"] = Convert.ToString(dob.Month);
                            formFieldMap["txtyeardob"] = Convert.ToString(dob.Year);
                        }
                    }
                if (Session["Attorney"] != null)
                    if (string.IsNullOrWhiteSpace(Session["Attorney"].ToString()))
                    {
                        formFieldMap["txt_attorney"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorney"] = Session["Attorney"].ToString();
                    }
                if (Session["AttorneyPhno"] != null)
                    if (string.IsNullOrWhiteSpace(Session["AttorneyPhno"].ToString()))
                    {
                        formFieldMap["txt_attorneyPhno"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorneyPhno"] = Session["AttorneyPhno"].ToString();
                    }
                if (Session["AttorneyAdd"] != null)
                    if (string.IsNullOrWhiteSpace(Session["AttorneyAdd"].ToString()))
                    {
                        formFieldMap["txt_attorneyAdd"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorneyAdd"] = Session["AttorneyAdd"].ToString();
                    }

                if (Session["Adjuster"] != null && Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 1)
                {
                    formFieldMap["txtAdjuster"] = Convert.ToString(Session["Adjuster"]).Split('~')[0];
                    if (Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 2)
                    {
                        formFieldMap["txtAdjusterph"] = Convert.ToString(Session["Adjuster"]).Split('~')[1];
                        if (Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 3)
                        { formFieldMap["txtAdjusterext"] = Convert.ToString(Session["Adjuster"]).Split('~')[2]; }
                    }
                }
                formFieldMap["txt_policy_no"] = Convert.ToString(Session["policy_no"]);
                if (!string.IsNullOrEmpty(txt_surgery.Text))
                {
                    formFieldMap["txt_surgery"] = Convert.ToString(txt_surgery.Text);
                }

                if (!string.IsNullOrEmpty(txt_MCode_Proc.Text))
                {
                    formFieldMap["txt_MCode_Proc"] = Convert.ToString(txt_MCode_Proc.Text);
                }



                formFieldMap["txt_c_dob"] = Convert.ToString(Session["dob"]);
                formFieldMap["txt_c_name"] = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
                formFieldMap["txt_claim_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                formFieldMap["txt_claim_dateDay"] = Convert.ToString(System.DateTime.Now.ToString("dd"));
                formFieldMap["txt_claim_dateMonth"] = Convert.ToString(System.DateTime.Now.ToString("MM"));
                formFieldMap["txt_claim_dateYear"] = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
                if (Session["sex"] != null)
                    if (Session["sex"].ToString() == "Mr.")
                    {

                        formFieldMap["txt_sex"] = "Male";
                        formFieldMap["txt_male"] = "X";
                    }
                    else if (Session["sex"].ToString() == "Ms.")
                    {
                        formFieldMap["txt_sex"] = "Female";
                        formFieldMap["txt_female"] = "X";
                    }
                if (Session["ssn"] != null)
                {
                    string ssn = Session["ssn"].ToString();
                    if (string.IsNullOrWhiteSpace(ssn))
                    {
                    }
                    else
                    {
                        if (ssn.Split().Last().All(char.IsDigit))
                        {
                            string ssn1 = ssn.Replace("-", "");
                            string separated = new string(
                                                             ssn1.Select((x, i) => i > 0 && i % 1 == 0 ? new[] { ',', x } : new[] { x })
                                                                .SelectMany(x => x)
                                                                .ToArray()
                                                                 );
                            if (string.IsNullOrWhiteSpace(separated))
                            {
                            }
                            else
                            {
                                int[] a = separated.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                if (a.Count() >= 9)
                                {
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
                            }
                        }
                        else
                        {

                        }
                    }
                }
                if (Session["doa"] != null)
                    if (string.IsNullOrWhiteSpace(Session["doa"].ToString()))
                    {
                    }
                    else
                    {
                        formFieldMap["txt_doa"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy");
                        formFieldMap["txt_doaday"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("dd");
                        formFieldMap["txt_doaMonth"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM");
                        formFieldMap["txt_doaYear"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("yyyy");
                    }
                if (Session["doe"] != null)
                    if (string.IsNullOrWhiteSpace(Session["doe"].ToString()))
                    {
                    }
                    else
                    {

                        formFieldMap["txt_doe"] = Convert.ToDateTime(Session["doe"].ToString()).ToString("MM/dd/yyyy");

                        DateTime doe = Convert.ToDateTime(Session["doe"].ToString());

                        formFieldMap["txtdaydoe"] = Convert.ToString(doe.Day);
                        formFieldMap["txtmonthdoe"] = Convert.ToString(doe.Month);
                        formFieldMap["txtyeardoe"] = Convert.ToString(doe.Year);

                    }
                if (Session["Compensation"] != null)
                {
                    formFieldMap["txt_casetype"] = Convert.ToString(Session["Compensation"]);
                    if (Session["Compensation"].Equals("WC"))
                    { formFieldMap["txt_wc"] = "yes"; }
                    else
                    { formFieldMap["txt_wc"] = "No"; }
                    if (Session["Compensation"].Equals("NF"))
                    { formFieldMap["txt_NF"] = "yes"; }
                    else
                    { formFieldMap["txt_NF"] = "No"; }
                    if (Session["Compensation"].Equals("PI"))
                    { formFieldMap["txt_PI"] = "yes"; }
                    else
                    { formFieldMap["txt_PI"] = "No"; }

                    if (Session["Compensation"].Equals("Lien"))
                    { formFieldMap["txt_AL"] = "yes"; }
                    else
                    { formFieldMap["txt_AL"] = "No"; }

                    if (Session["Compensation"].Equals("MM"))
                    { formFieldMap["txt_MM"] = "yes"; }
                    else
                    { formFieldMap["txt_MM"] = "No"; }

                    if (Session["Compensation"].Equals("Taxi"))
                    { formFieldMap["txt_Taxi"] = "yes"; }
                    else
                    { formFieldMap["txt_Taxi"] = "No"; }

                    formFieldMap["txt_PC"] = "No";
                    formFieldMap["txt_SP"] = "No";

                }
                //formFieldMap["txt_PMH"] = Convert.ToString(Session["PMH"]);
                //formFieldMap["txt_PSH"] = Convert.ToString(Session["PSH"]);
                //formFieldMap["txt_Allergy"] = Convert.ToString(Session["Allergies"]);
                //formFieldMap["txt_Medications"] = Convert.ToString(Session["Medications"]);
                //formFieldMap["txt_Mcode"] = Convert.ToString(Session["Mcode"]);
                formFieldMap["txt_WCBGroup"] = Convert.ToString(Session["WCBGroup"]);
                formFieldMap["txt_1"] = Convert.ToString("2");
                formFieldMap["txt_2"] = Convert.ToString("4");
                formFieldMap["txt_3"] = Convert.ToString("3");
                formFieldMap["txt_4"] = Convert.ToString("1");
                formFieldMap["txt_5"] = Convert.ToString("8");
                formFieldMap["txt_6"] = Convert.ToString("2");
                formFieldMap["txt_7"] = Convert.ToString("3");
                formFieldMap["txt_8"] = Convert.ToString("W");
                formFieldMap["txt_npino"] = Convert.ToString("1932354818");

                if (Convert.ToString(Session["filename"]).Equals("Accelerated.pdf"))
                {
                    if (Session["Compensation"] != null)
                        if (Session["Compensation"].Equals("WC"))
                        {
                            if (Session["doe"] != null)
                                formFieldMap["txt_doeWC"] = Convert.ToDateTime(Session["doe"].ToString()).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            if (Session["doa"] != null)
                                formFieldMap["txt_doaMVA"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy");
                        }
                }

                fpname.Text = Convert.ToString(Session["fname"]);
                lpname.Text = Convert.ToString(Session["lname"]);

                var pdfContents = PDFHelper.GeneratePDF(pdfPath, formFieldMap);
                string filename = lpname.Text.Trim() + ", " + fpname.Text.Trim() + "_demo.pdf";
                File.WriteAllBytes(Path.Combine(Server.MapPath(foldername), filename), pdfContents);
               
            }
            Response.Redirect("batchprint.aspx");
        }
    }

}