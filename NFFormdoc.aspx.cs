using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
//using Excel = Microsoft.Office.Interop.Excel;
//using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;
using FindAndReplace;

using System.Data;


public partial class NFFormdoc : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        string filename=Session["filename"].ToString();
        string pid = Session["newID"].ToString();
        if (Session["newID"] != null)
            bindEditData(Session["newID"].ToString());


        string inputFile = Server.MapPath("~/TemplateStore/NJ/" + Session["filename"].ToString());
       // string inputFile = Request.PhysicalApplicationPath + "\\document\\DisabilityLetterNF.docx";
        //System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Upload/"));
        Session["outfile"]="1"+Session["filename"].ToString();
        System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/document/" + Session["outfile"].ToString()));
        if (di != null)
        {
            //foreach (FileInfo file in di.GetFiles())
            //{
            //    file.Delete();
            //}
            string fileName = di.ToString();
            System.IO.File.Delete(fileName);
        }
        string outputFile = Server.MapPath("~/document/" + Session["outfile"].ToString());
        //string outputFile = Server.MapPath("DisabilityLetterNF1.docx");
        //string outputFile = Request.PhysicalApplicationPath + "\\document\\DisabilityLetterNF1.docx";

        // Copy Word document.
        File.Copy(inputFile, outputFile,false);

        // Open copied document.
        using (var flatDocument = new FlatDocument(outputFile))
        {

         
            flatDocument.FindAndReplace("[DATE]",Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy")));
            flatDocument.FindAndReplace("[NAME]", Session["fname"].ToString()+" "+Session["lname"].ToString());
            flatDocument.FindAndReplace("[DOA]", Session["DOA"].ToString());
            // Save document on Dispose.  
        }
       
        Response.Redirect("~/OpenForm.aspx");
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
                Session["fname"] = ds.Tables[0].Rows[0]["FirstName"].ToString();
                Session["lname"] = ds.Tables[0].Rows[0]["LastName"].ToString();
                Session["DOA"] = ds.Tables[0].Rows[0]["DOA"].ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }
}