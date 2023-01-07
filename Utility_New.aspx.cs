using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Utility_New : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadGrid();
    }

    public void LoadGrid()
    {
        // txtFolderName.Text = "";
        string root = Server.MapPath("~/PatientDocument");
        //string root = @"F:\Locations\" + DropDownList1.SelectedValue;
        string[] fileEntries = Directory.GetDirectories(root);

        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("Name");


        foreach (string filename in fileEntries)
        {
            string fname = filename.Split('\\').Last();
            DataRow dr = dt.NewRow();
            dr["Name"] = fname;
            dt.Rows.Add(dr);
        }

        gvDocument.DataSource = dt;
        gvDocument.DataBind();
    }

    protected void btnuploadimage_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        DataSet dataSet = new DataSet();
        string fname = "", lname = "";

        foreach (HttpPostedFile postedFile in fupuploadsign.PostedFiles)
        {
            //string[] fileName = Path.GetFileName(postedFile.FileName).Split(',');
            string fileName = Path.GetFileName(postedFile.FileName);
            DBHelperClass dBHelperClass = new DBHelperClass();

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
                    fileName = dataSet.Tables[0].Rows[0][0].ToString() + "_" + fileName;
                    string upload_folder_path = "~/PatientDocument/" + hd_name.Value;
                    string fullpath = System.IO.Path.Combine(Server.MapPath(upload_folder_path), fileName);

                    postedFile.SaveAs(fullpath);

                    sb.Append("<p>File Name : " + fileName + "  patiendId:" + fileName.Split('_')[0] + "     Status : Uploaded </p>");
                    sb.Append(Environment.NewLine);
                    Logger.Info("File Name : " + fileName + "  patiendId:" + fileName.Split('_')[0] + "     Status : Uploaded");

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
}