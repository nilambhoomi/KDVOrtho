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
using System.Data.OleDb;

public partial class Page1 : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindEditData();

        }
    }
    private void bindEditData()
    {
        try
        {

            string query = "select PatientIE_ID,FirstName,LastName,Compensation,InsCo,Adjuster,WCBGroup,ClaimNumber,policy_no,DOA,DOB from View_PatientIE";
            DataSet ds = db.selectData(query);
            ViewState["DataTable"] = ds.Tables[0];
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["DataTable"];

        //calling create Excel File Method
        CreateExcelFile(dt);

    }
    public void CreateExcelFile(DataTable Excel)
    {

        //Clears all content output from the buffer stream.
        Response.ClearContent();
        //Adds HTTP header to the output stream
        Response.AddHeader("content-disposition", string.Format("attachment; filename=Exportedrecords.xls"));

        // Gets or sets the HTTP MIME type of the output stream
        Response.ContentType = "application/vnd.ms-excel";
        string space = "";

        foreach (DataColumn dcolumn in Excel.Columns)
        {

            Response.Write(space + dcolumn.ColumnName);
            space = "\t";
        }
        Response.Write("\n");
        int countcolumn;
        foreach (DataRow dr in Excel.Rows)
        {
            space = "";
            for (countcolumn = 0; countcolumn < Excel.Columns.Count; countcolumn++)
            {

                Response.Write(space + dr[countcolumn].ToString());
                space = "\t";

            }

            Response.Write("\n");


        }
        Response.End();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {

        if (fileUpload.HasFile)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Importfile/"));

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            string path = string.Concat(Server.MapPath("~/Importfile/" + fileUpload.FileName));
            fileUpload.SaveAs(path);
            Microsoft.Office.Interop.Excel.Application appExcel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Range range;
            Microsoft.Office.Interop.Excel._Worksheet worksheet;

            appExcel = new Microsoft.Office.Interop.Excel.Application();
            workbook = appExcel.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets[1];
            range = worksheet.UsedRange;

            int rowCount = range.Rows.Count;
            int colCount = range.Columns.Count;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.TableName = "patinet";
            dt.Columns.Add("PatientIE_ID", typeof(int));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Compensation",typeof(string));
            dt.Columns.Add("InsCo",typeof(string));
            dt.Columns.Add("Adjuster", typeof(string));
            dt.Columns.Add("WCBGroup", typeof(string));
            dt.Columns.Add("ClaimNumber", typeof(string));
            dt.Columns.Add("policy_no", typeof(string));
            dt.Columns.Add("DOA", typeof(string));
            dt.Columns.Add("DOB", typeof(string));
            for (int Rnum = 1; Rnum <= rowCount; Rnum++)
            {
                DataRow dr = dt.NewRow();
                //Reading Each Column value From sheet to datatable Colunms                  
                for (int Cnum = 1; Cnum <= colCount; Cnum++)
                {
                    //dr[Cnum - 1] = (range.Cells[Rnum, Cnum]).value.ToString();
                    dr[Cnum - 1] = Convert.ToString((range.Cells[Rnum, Cnum] as Microsoft.Office.Interop.Excel.Range).Value2);
                }
                dt.Rows.Add(dr); // adding Row into DataTable
                dt.AcceptChanges();
            }

            workbook.Close(true);
            appExcel.Quit();

            string constr = @"Data Source=MOULICK-PC\SQLEXPRESS;Initial Catalog=dbPainTraxXUAT;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_patient_Export"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@patient", dt);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                   
                    lblmsg.Text = "Sucessufully Imported";
                }
            }
           

        }
    }
}