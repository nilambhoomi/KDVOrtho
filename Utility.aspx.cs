using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class Utility : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(Utility));
    StringBuilder sb = new StringBuilder();
    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        Logger.Info("Welcome");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DBHelperClass dBHelperClass = new DBHelperClass();
        DataSet dataSet = new DataSet();
        string fname = "", lname = "";


        foreach (HttpPostedFile postedFile in fup.PostedFiles)
        {
            //string[] fileName = Path.GetFileName(postedFile.FileName).Split(',');
            string fileName = Path.GetFileName(postedFile.FileName);
            string msg = "";
            try
            {
                Regex re = new Regex(@"\d+");
                Match m = re.Match(fileName);
                if (m.Success)
                {

                    string file = fileName.Substring(0, m.Index);
                    string[] str = file.Split(',');
                    // lblResults.Text = string.Format("RegEx found " + m.Value + " at position " + m.Index.ToString() + " character in string is " + file + " fname: " + str[1] + ",LastName:" + str[0]);
                    lname = str[0];

                    if (str[1].Contains("_"))
                    {
                        fname = str[1].Split('_')[0];
                    }
                    else
                    {
                        fname = str[1];
                    }
                }
                else
                {

                    string[] str = fileName.Split('_');
                    fname = str[0].Split(',')[1];
                    lname = str[0].Split(',')[0];

                }

                string[] strfname = fname.TrimStart().Split('_');

                fname = strfname[0];


                dataSet = dBHelperClass.selectData("select Patient_ID from tblPatientMaster where LastName='" + lname.Trim().TrimStart() + "' and FirstName='" + fname.Trim().TrimStart() + "'");

                
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 1)
                {

                    sb.Append("<p style='color:red'> File Name : " + fileName + "     Status : Not Uploaded becuase this patient have mutiple records in system.</p>");
                    sb.Append(Environment.NewLine);
                    Logger.Info("File Name : " + fileName + "     Status : Not Uploaded");


                }
                else if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {

                    string patientid = dataSet.Tables[0].Rows[0][0].ToString();

                    string upload_folder_path = "~/PatientDocument/" + patientid;

                    if (!Directory.Exists(upload_folder_path))
                        Directory.CreateDirectory(Server.MapPath(upload_folder_path));

                    postedFile.SaveAs(System.IO.Path.Combine(Server.MapPath(upload_folder_path), fileName));

                    sb.Append("<p>File Name : " + fileName + "  patientid:" + patientid + "     Status : Uploaded </p>");
                    sb.Append(Environment.NewLine);
                    Logger.Info("File Name : " + fileName + "  patientid:" + patientid + "     Status : Uploaded");

                    //string filename = System.DateTime.Now.Millisecond.ToString() + "_" + fileName;
                    if (checkName(fileName, patientid) == false)
                    {
                        string filename = fileName;
                        string query = "insert into tblPatientDocument values('" + filename + "','" + System.DateTime.Now.ToString() + "','" + upload_folder_path + "/" + filename + "'," + patientid + ",null)";

                        dBHelperClass.executeQuery(query);

                    }

                }
                else
                {
                    sb.Append("<p style='color:red'> File Name : " + fileName + "     Status : Not Uploaded</p>");
                    sb.Append(Environment.NewLine);
                    Logger.Info("File Name : " + fileName + "     Status : Not Uploaded");
                }

            }
            catch (Exception ex)
            {
                sb.Append("<p style='color:red'>File Name : " + fileName + "     Status : Not Uploaded </p>");
                Logger.Error("File Name : " + fileName + "       Status : Not Uploaded \n");
            }
            lblResult.InnerHtml = sb.ToString();
        }

    }

    protected void btnSignUpload_Click(object sender, EventArgs e)
    {

        foreach (HttpPostedFile postedFile in fupsign.PostedFiles)
        {
            //string[] fileName = Path.GetFileName(postedFile.FileName).Split(',');
            string fileName = Path.GetFileName(postedFile.FileName);
            string msg = "";
            try
            {


                string upload_folder_path = "~/Sign/";
                string fullpath = System.IO.Path.Combine(Server.MapPath(upload_folder_path), fileName);

                postedFile.SaveAs(fullpath);

                sb.Append("<p>File Name : " + fileName + "  patientIEId:" + fileName.Split('_')[0] + "     Status : Uploaded </p>");
                sb.Append(Environment.NewLine);
                Logger.Info("File Name : " + fileName + "  patientIEId:" + fileName.Split('_')[0] + "     Status : Uploaded");
                string query = "";
                if (!string.IsNullOrEmpty(fileName.Split('_')[0]))
                {
                    query = "delete from tblPatientIESign where PatientIE_ID=" + fileName.Split('_')[0];


                    db.executeQuery(query);
                    query = "insert into tblPatientIESign values(" + fileName.Split('_')[0] + ",'" + fullpath + "',null,null,getdate())";
                    db.executeQuery(query);
                }

            }
            catch (Exception ex)
            {
                sb.Append("<p style='color:red'>File Name : " + fileName + "     Status : Not Uploaded </p>");
                Logger.Error("File Name : " + fileName + "       Status : Not Uploaded \n");
            }

        }
        lblResult.InnerHtml = sb.ToString();
    }

    private bool checkName(string fname, string pid)
    {
        bool _flag = true;
        try
        {
            DataSet dt = db.selectData("Select * from tblPatientDocument where DocName='" + fname + "' and PatientID=" + pid);

            if (dt != null && dt.Tables[0].Rows.Count > 0)
                _flag = true;
            else
                _flag = false;

        }
        catch (Exception ex)
        {
            _flag = true;
        }
        return _flag;
    }

    protected void btnxls_Click(object sender, EventArgs e)
    {
        string connString = "";
        string strFileType = Path.GetExtension(fup_xls.FileName).ToLower();
        // string path = fup_xls.PostedFile.FileName;
        string path = Server.MapPath("~/Importfile/" + fup_xls.PostedFile.FileName);
        fup_xls.SaveAs(path);
        ////Connection String to Excel Workbook
        if (strFileType.Trim() == ".xls")
        {
            connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"", path);
        }
        else if (strFileType.Trim() == ".xlsx")
        {
            connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"", path);

        }
        // connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"",path);
        string query = "SELECT * FROM [Sheet1$]";
        OleDbConnection conn = new OleDbConnection(connString);
        if (conn.State == ConnectionState.Closed)
            conn.Open();
        OleDbCommand cmd = new OleDbCommand(query, conn);
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        string pname = "";
        try
        {

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                sb = new StringBuilder();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Last Name"].ToString() == "" && ds.Tables[0].Rows[i]["ID"].ToString() == "")
                    { }
                    else
                    {
                        pname = ds.Tables[0].Rows[i]["Last Name"].ToString() + " " + ds.Tables[0].Rows[i]["First Name"].ToString();
                        savePatientData(ds.Tables[0].Rows[i]);

                        sb.Append("<p><b>Patient Name :</b> " + pname + "  <b>patientIEId :</b>" + ds.Tables[0].Rows[i]["ID"].ToString() + "     <b>Status :</b> Import </p>");
                    }

                }
            }
        }
        catch (Exception ex)
        {
            sb.Append("<p><b>Patient Name :</b> " + pname + "     <b>Status :</b> Not-Import <b>Reason :</b> " + ex.Message + " </p>");
            da.Dispose();
            conn.Close();
            conn.Dispose();
        }


        da.Dispose();
        conn.Close();
        conn.Dispose();

        lblResult.InnerHtml = sb.ToString();
    }

    private void savePatientData(DataRow dataRow)
    {
        DBHelperClass helperClass = new DBHelperClass();

        SqlParameter[] param = null;
        string sp = "";

        if (dataRow["ID"].ToString() == "")
        {
            param = new SqlParameter[27];
            sp = "nusp_import_patientdata";
        }
        else
        {
            param = new SqlParameter[28];
            sp = "nusp_import_update_patientdata";
        }

        param[0] = new SqlParameter("@location", dataRow["Location"].ToString());
        param[1] = new SqlParameter("@DOS", dataRow["DOS"].ToString());
        param[2] = new SqlParameter("@DOA", dataRow["DOA"].ToString());
        param[3] = new SqlParameter("@lastname", dataRow["Last Name"].ToString());
        param[4] = new SqlParameter("@firstname", dataRow["First Name"].ToString());
        param[5] = new SqlParameter("@middlename", dataRow["ML"].ToString());
        param[6] = new SqlParameter("@sex", dataRow["Sex"].ToString());
        param[7] = new SqlParameter("@DOB", dataRow["DOB"].ToString());
        param[8] = new SqlParameter("@SSN", dataRow["SSN"].ToString());
        param[9] = new SqlParameter("@home_ph", dataRow["Home Ph"].ToString());
        param[10] = new SqlParameter("@work_ph", dataRow["Work Ph"].ToString());
        param[11] = new SqlParameter("@mobile", dataRow["Mobile"].ToString());
        param[12] = new SqlParameter("@address", dataRow["Address"].ToString());
        param[13] = new SqlParameter("@city", dataRow["City"].ToString());
        param[14] = new SqlParameter("@state", dataRow["State"].ToString());
        param[15] = new SqlParameter("@zip", dataRow["Zip"].ToString());
        param[16] = new SqlParameter("@ins_co", dataRow["Insurance_Co"].ToString());
        param[17] = new SqlParameter("@claim", dataRow["Claim #"].ToString());
        param[18] = new SqlParameter("@policy_no", dataRow["Policy #"].ToString());
        param[19] = new SqlParameter("@attorney_name", dataRow["Attorney Name"].ToString());
        param[20] = new SqlParameter("@attorney_ph", dataRow["Attorney Ph"].ToString());
        param[21] = new SqlParameter("@case_type", dataRow["Case Type"].ToString());
        param[22] = new SqlParameter("@wcb_group", dataRow["WCB Group"].ToString());
        param[23] = new SqlParameter("@ma_provider", dataRow["MA & Providers"].ToString());
        param[24] = new SqlParameter("@adjuster", dataRow["Adjuster"].ToString());
        param[25] = new SqlParameter("@adjuster_ph", dataRow["Adjuster Ph"].ToString());
        param[26] = new SqlParameter("@ext", dataRow["Ext"].ToString());

        if (dataRow["ID"].ToString() != "")
            param[27] = new SqlParameter("@PIE_ID", dataRow["ID"].ToString());


        int val = helperClass.executeSP(sp, param);
    }

    protected void btnupload_mul_Click(object sender, EventArgs e)
    {
        DBHelperClass dBHelperClass = new DBHelperClass();
        DataSet dataSet = new DataSet();
        string fname = "", lname = "", doa = "";


        foreach (HttpPostedFile postedFile in fupmul.PostedFiles)
        {
            //string[] fileName = Path.GetFileName(postedFile.FileName).Split(',');
            string fileName = Path.GetFileName(postedFile.FileName);
            string msg = "";
            try
            {
                Regex re = new Regex(@"\d+");
                Match m = re.Match(fileName);
                if (m.Success)
                {

                    string file = fileName.Substring(0, m.Index);
                    string[] str = file.Split(',');
                    // lblResults.Text = string.Format("RegEx found " + m.Value + " at position " + m.Index.ToString() + " character in string is " + file + " fname: " + str[1] + ",LastName:" + str[0]);
                    lname = str[0];

                    if (str[1].Contains("_"))
                    {
                        fname = str[1].Split('_')[0];
                    }
                    else
                    {
                        fname = str[1];
                    }

                    file = fileName;
                    str = file.Split('_');
                    if (str.Length > 2)
                    {
                        doa = str[str.Length - 1].ToLower().Split('.')[0];
                    }
                }
                else
                {

                    string[] str = fileName.Split('_');
                    fname = str[0].Split(',')[1];
                    lname = str[0].Split(',')[0];

                }

                doa = CommonConvert.DateformatDOA(doa);
                doa = CommonConvert.DateFormat(doa);

                string[] strfname = fname.TrimStart().Split(' ');
                fname = strfname[0];
                dataSet = dBHelperClass.selectData("select Patient_ID from tblPatientMaster where LastName='" + lname.Trim().TrimStart() + "' and FirstName='" + fname.Trim().TrimStart() + "'");

                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {

                    string patientid = dataSet.Tables[0].Rows[0][0].ToString();

                    string upload_folder_path = "~/PatientDocument/" + patientid;

                    if (!Directory.Exists(upload_folder_path))
                        Directory.CreateDirectory(Server.MapPath(upload_folder_path));

                    postedFile.SaveAs(System.IO.Path.Combine(Server.MapPath(upload_folder_path), fileName));

                    sb.Append("<p>File Name : " + fileName + "  patientid:" + patientid + "     Status : Uploaded </p>");
                    sb.Append(Environment.NewLine);
                    Logger.Info("File Name : " + fileName + "  patientid:" + patientid + "     Status : Uploaded");

                    //string filename = System.DateTime.Now.Millisecond.ToString() + "_" + fileName;
                    if (checkName(fileName, patientid) == false)
                    {
                        string filename = fileName;
                        string query = "insert into tblPatientDocument values('" + filename + "','" + System.DateTime.Now.ToString() + "','" + upload_folder_path + "/" + filename + "'," + patientid + ",'" + doa + "')";

                        dBHelperClass.executeQuery(query);

                    }

                }
                else
                {
                    sb.Append("<p style='color:red'> File Name : " + fileName + "     Status : Not Uploaded</p>");
                    sb.Append(Environment.NewLine);
                    Logger.Info("File Name : " + fileName + "     Status : Not Uploaded");
                }

            }
            catch (Exception ex)
            {
                sb.Append("<p style='color:red'>File Name : " + fileName + "     Status : Not Uploaded </p>");
                Logger.Error("File Name : " + fileName + "       Status : Not Uploaded \n");
            }
            lblResult.InnerHtml = sb.ToString();
        }

    }
}