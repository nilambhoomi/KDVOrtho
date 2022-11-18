using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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

        foreach (HttpPostedFile postedFile in fupuploadsign.PostedFiles)
        {
            //string[] fileName = Path.GetFileName(postedFile.FileName).Split(',');
            string fileName = Path.GetFileName(postedFile.FileName);
          
            try
            {


                string upload_folder_path = "~/PatientDocument/"+ hd_name.Value;
                string fullpath = System.IO.Path.Combine(Server.MapPath(upload_folder_path), fileName);

                postedFile.SaveAs(fullpath);

                sb.Append("<p>File Name : " + fileName + "  patientIEId:" + fileName.Split('_')[0] + "     Status : Uploaded </p>");
                sb.Append(Environment.NewLine);
                Logger.Info("File Name : " + fileName + "  patientIEId:" + fileName.Split('_')[0] + "     Status : Uploaded");
                

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