
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Text;
using System.Web.Services;

public partial class LogDetail : System.Web.UI.Page
{
    // DataTable dt = new DataTable();
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack)
        {
            bindLogId();
            bindLocation();
            txt_start_date.Text = System.DateTime.Now.ToString("MM-dd-yyyy");
            txt_end_date.Text = System.DateTime.Now.ToString("MM-dd-yyyy");
        }

    }
    public void bindData()
    {

        DataSet ds = db.logDetail(txt_start_date.Text, txt_end_date.Text, Convert.ToString(ddl_Login_Id.SelectedItem), Convert.ToString(ddl_location.SelectedItem));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gdview.DataSource = ds;
            gdview.DataBind();
            btnExportExcel.Visible = true;
            btnExportPDF.Visible = true;
            txtSearch.Visible = true;
        }

    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        bindData();
        string temp = ddl_Login_Id.SelectedValue;
    }
    private void bindLocation()
    {

        DataSet ds = new DataSet();
        ds = db.selectData("select Location,Location_ID from tblLocations Order By Location");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";
            ddl_location.DataSource = ds;
            ddl_location.DataBind();
            ddl_location.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
            ddl_location.SelectedValue = "ALL";
        }
    }
    private void bindLogId()
    {
        DataSet ds = new DataSet();
        // ds = db.selectData("select distinct log_id,name from tblLogDetail");
        ds = db.selectData("select distinct name from tblLogMaster");



        if (ds.Tables[0].Rows.Count > 0)
        {

            ddl_Login_Id.DataValueField = "name";
            ddl_Login_Id.DataTextField = "name";
            ddl_Login_Id.DataSource = ds;
            ddl_Login_Id.DataBind();
            ddl_Login_Id.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
            ddl_Login_Id.SelectedValue = "ALL";
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        gdview.PageIndex = e.NewPageIndex;
        this.bindData();
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=LogReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //To Export all pages
            gdview.AllowPaging = false;
            this.bindData();

            gdview.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in gdview.HeaderRow.Cells)
            {
                cell.BackColor = gdview.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gdview.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gdview.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gdview.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }
            gdview.RenderControl(hw);
            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                //To Export all pages
                gdview.AllowPaging = false;
                this.bindData();

                gdview.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=LogReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }
    }

    //protected void txtSearch_TextChanged(object sender, EventArgs e)
    //{

    //}
}