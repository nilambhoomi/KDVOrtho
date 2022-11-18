using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IntakeSheet.BLL;
using IntakeSheet.Entity;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class SIDownloadSheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
            if (Request.QueryString["L"] != null && Request.QueryString["D"] != null)
            {
                string MAProviders = null;
                BusinessLogic _bl = new BusinessLogic();
                string locationId = Request.QueryString["L"];
                DateTime date = DateTime.ParseExact(Request.QueryString["D"], "s", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                System.Data.DataTable dt = _bl.getSIDownload(Convert.ToInt64(locationId), date, out MAProviders);
                lblDate.Text = date.ToShortDateString();

                foreach (string[] s in _bl.getLocations())
                {
                    if (s[0] == locationId)
                    {
                        lblLocation.Text = s[1];
                        break;
                    }
                }

                lblMAProvider.Text = MAProviders;
                gvSISheet.DataSource = dt;
                gvSISheet.DataBind();
            }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    protected void gvSISheet_PreRender(object sender, EventArgs e)
    {
        var gridView = (GridView)sender;
        var header = (GridViewRow)gridView.Controls[0].Controls[0];
        if (header != null && header.Cells.Count > 10) { 
        header.Cells[9].Visible = false;
        header.Cells[10].ColumnSpan = 2;
        header.Cells[10].Text = "Next Visit";
        }
    }
    protected void Download_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=TestPage.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        this.Page.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 100f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }

   
    private void ClearControls(Control control)
    {
        for (int i = control.Controls.Count - 1; i >= 0; i--)
        {
            ClearControls(control.Controls[i]);
        }
        if (!(control is TableCell))
        {
            if (control.GetType().GetProperty("SelectedItem") != null)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                try
                {
                    literal.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control, null);
                }
                catch
                {
                }
                control.Parent.Controls.Remove(control);
            }
            else
                if (control.GetType().GetProperty("Text") != null)
                {
                    LiteralControl literal = new LiteralControl();
                    control.Parent.Controls.Add(literal);
                    literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control, null);
                    control.Parent.Controls.Remove(control);
                }
        }
        return;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TimeSheet.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            gvSISheet.AllowPaging = false;
            //this.BindGrid();

            // gvSISheet.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in gvSISheet.HeaderRow.Cells)
            {
                cell.BackColor = gvSISheet.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gvSISheet.Rows)
            {
                //row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gvSISheet.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gvSISheet.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gvSISheet.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
   
    }

    
    }
}