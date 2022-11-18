using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;

public partial class pdfMap : System.Web.UI.Page
{
    DataTable dt;
    DBHelperClass db = new DBHelperClass();
   // PdfSqlHelper h = new PdfSqlHelper();

    public void loadpdflist()
    {
        ddlPdf.Items.Clear();
        string[] filePaths = Directory.GetFiles(Server.MapPath("~/MapPdf"));
        ddlPdf.Items.Add(" -- Select Pdf --");
        for (int i = 0; i < filePaths.Length; i++)
        {
            if (filePaths[i].ToLower().EndsWith("pdf"))
                ddlPdf.Items.Add(filePaths[i].Substring(filePaths[i].LastIndexOf("\\") + 1));
        }
    }
    public void loadtable()
    {
        ddlTable.DataTextField = "name";
        ddlTable.DataValueField = "name";
        //ddlTable.DataSource = db.selectData ("    SELECT name FROM sys.Tables union SELECT name FROM sys.Views union  SELECt ' -- Select Table -- '  ").Tables[0];
        ddlTable.DataSource = db.selectData("    SELECT name FROM sys.Tables where name in( SELECT table_name from  INFORMATION_SCHEMA.columns  where column_name = 'PatientIE_ID') union SELECT name FROM sys.Views where  name in( SELECT table_name from  INFORMATION_SCHEMA.columns  where column_name = 'PatientIE_ID') union   SELECt ' -- Select Table -- '  ");
        ddlTable.SelectedIndex = 0;
        ddlTable.DataBind();
    }
    public void loadpdf()
    {
        int pageno = 0;
        try
        {
            pageno = Convert.ToInt32(txtPage.Text);
        }
        catch { }
        tframe.Attributes["src"] = "Files/_temp2.pdf#page=" + pageno;
        txtSearchText.Text = "";
        txtSearchField.Text = "";
    }

    public void Stamping(string SourceFile, string TargetFile, bool flat, ListBox list, string Mode = "Read", ListBox listf = null, int Row = 0, string txtName = "", string txtvalue = "")
    {

        //string pdfTemplate = openFileDialog.FileName;
        //string newFile = Application.StartupPath + "/_temp.pdf";
        PdfReader pdfReader = null;
        PdfStamper pdfStamper = null;
        try
        {
            pdfReader = new PdfReader(SourceFile);
            pdfStamper = new PdfStamper(pdfReader, new FileStream(TargetFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;
            pdfStamper.FormFlattening = flat;
            foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in pdfReader.AcroFields.Fields)
            {
                if (Mode == "Read")
                {
                    if (de.Key.ToString().ToLower() != "txttable" && de.Key.ToString().ToLower() != "txtfile" && !de.Key.ToString().ToLower().StartsWith("txtfix"))
                    {
                        pdfFormFields.SetField(de.Key.ToString(), de.Key.ToString());
                    }
                    //   /* if (list != null && de.Key.ToString().ToLower() != "txttable" && de.Key.ToString().ToLower() != "txtfile")
                    //    {
                    //        pdfFormFields.SetField(de.Key.ToString(), de.Key.ToString());
                    //        list.Items.Add(de.Key.ToString());
                    //    }*/
                }
                if (Mode == "ReadMap")
                {

                    if (de.Key.ToString().ToLower() == "txttable")
                    {
                        lblTableName.Text = pdfFormFields.GetField(de.Key.ToString());
                    }
                    if (de.Key.ToString().ToLower() == "txtfile")
                    {
                        lblPrefix.Text = pdfFormFields.GetField(de.Key.ToString());
                    }

                    if (list != null && de.Key.ToString().ToLower() != "txttable" && de.Key.ToString().ToLower() != "txtfile" && de.Key.ToString().ToLower() != "imgsign" && !de.Key.ToString().ToLower().StartsWith("txtfix") && !de.Key.ToString().ToLower().StartsWith("@") && !de.Key.ToString().ToLower().StartsWith("#"))
                    {
                        pdfFormFields.SetField(de.Key.ToString(), pdfFormFields.GetField(de.Key.ToString()));
                        list.Items.Add(de.Key.ToString());
                    }
                }
                if (Mode == "Table")
                {
                    if (de.Key.ToString().ToLower() == txtName.ToLower())
                    {
                        pdfFormFields.SetField(de.Key.ToString(), txtvalue);
                        lblTableName.Text = pdfFormFields.GetField(de.Key.ToString());
                    }
                }
                if (Mode == "File")
                {
                    if (de.Key.ToString().ToLower() == txtName.ToLower())
                    {
                        pdfFormFields.SetField(de.Key.ToString(), txtvalue);
                        lblPrefix.Text = pdfFormFields.GetField(de.Key.ToString());
                    }
                }


                if (Mode == "Map")
                {
                    if (de.Key.ToString() == list.Text)
                    {
                        pdfFormFields.SetField(de.Key.ToString(), de.Key.ToString() + "|" + listf.Text);
                    }
                }
                if (Mode == "UnMap")
                {
                    if (de.Key.ToString() == list.Text)
                    {
                        pdfFormFields.SetField(de.Key.ToString(), de.Key.ToString());
                    }
                }
                if (Mode == "MapDefault")
                {
                    if (de.Key.ToString() == list.Text)
                    {
                        string textvalue = pdfFormFields.GetField(de.Key.ToString());
                        string[] textpair = textvalue.Split('|');
                        if (textpair.Length > 1)
                        {
                            pdfFormFields.SetField(de.Key.ToString(), textvalue + "|" + txtDefault.Text );
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Map First')", true);
                        }
                    }
                }

                /*   
                 *   if (Mode == "Gen")
                   {
                       string textvalue = pdfFormFields.GetField(de.Key.ToString());
                       string[] textpair = textvalue.Split('|');


                           if (textpair.Length > 1)
                           {
                               try
                               {
                                   pdfFormFields.SetField(textpair[0], dt.Rows[Row][textpair[1]].ToString());
                               }
                               catch (Exception ex)
                               {
                                   pdfFormFields.SetField(textpair[0], "");
                                   //hidGenError.Value  += "\n " + ex.Message;
                               }
                           }
                           else
                           {
                               pdfFormFields.SetField(textpair[0], "");
                           }



                       if (pdfFormFields.GetFieldType(de.Key.ToString()) == AcroFields.FIELD_TYPE_RADIOBUTTON)
                       {
                           try
                           {
                               if (dt.Rows[Row][de.Key.ToString()].ToString().ToLower()[0] == 'n')
                               {
                                   pdfFormFields.SetFieldProperty(de.Key.ToString(), "flags", PdfFormField.FLAGS_NOVIEW, null);
                               }
                           }
                           catch (Exception ex)
                           {
                               //hidGenError.Value += "\n " + ex.Message;

                           }
                       }
                   }*/
            }
        }
        catch(Exception ex)
        {
            lblPdf.Text = "Error !" + ex.Message;
        }
        finally
        {

            pdfStamper.Close();
            pdfReader.Close();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadtable();
            loadpdflist();

        }
    }
    protected void btnBlankPdf_Click(object sender, EventArgs e)
    {
        if (FilePdf.HasFile && FilePdf.FileName.ToLower().EndsWith(".pdf"))
        {
            try
            {
                FilePdf.SaveAs(Server.MapPath("Files/~temp.pdf"));
                Stamping(Server.MapPath("Files/~temp.pdf"), Server.MapPath("MapPdf/" + FilePdf.FileName), false, null);
                loadpdflist();
                ListText.Items.Clear();
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Pdf File Uploaded')", true);
                
            }
            catch (Exception ex)
            {
                lblPdf.Text = "Error !" + ex.Message;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Select Pdf File First')", true);
            
        }
    }


    protected void btnMap_Click(object sender, EventArgs e)
    {
        if (ListText.SelectedIndex == -1 || ListField.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Select Text and Field')", true);
            
            return;
        }
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp_2.pdf"), false, ListText, "Map", ListField);
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp2_2.pdf"), true, ListText, "Map", ListField);
        File.Delete(Server.MapPath("Files/_temp.pdf"));
        File.Move(Server.MapPath("Files/_temp_2.pdf"), Server.MapPath("Files/_temp.pdf"));
        File.Delete(Server.MapPath("Files/_temp2.pdf"));
        File.Move(Server.MapPath("Files/_temp2_2.pdf"), Server.MapPath("Files/_temp2.pdf"));
        File.Copy(Server.MapPath("Files/_temp.pdf"), Server.MapPath("MapPdf/" + ddlPdf.Text), true);
        loadpdf();
    }
    protected void btnUnMap_Click(object sender, EventArgs e)
    {
        if (ListText.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Select Text')", true);
            
            return;
        }
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp_2.pdf"), false, ListText, "UnMap", ListField);
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp2_2.pdf"), true, ListText, "UnMap", ListField);
        File.Delete(Server.MapPath("Files/_temp.pdf"));
        File.Move(Server.MapPath("Files/_temp_2.pdf"), Server.MapPath("Files/_temp.pdf"));
        File.Delete(Server.MapPath("Files/_temp2.pdf"));
        File.Move(Server.MapPath("Files/_temp2_2.pdf"), Server.MapPath("Files/_temp2.pdf"));
        File.Copy(Server.MapPath("Files/_temp.pdf"), Server.MapPath("MapPdf/" + ddlPdf.Text), true);

        loadpdf();
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {

        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp_2.pdf"), false, null);
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp2_2.pdf"), true, null);
        File.Delete(Server.MapPath("Files/_temp.pdf"));
        File.Delete(Server.MapPath("Files/_temp2.pdf"));
        File.Move(Server.MapPath("Files/_temp_2.pdf"), Server.MapPath("Files/_temp.pdf"));
        File.Move(Server.MapPath("Files/_temp2_2.pdf"), Server.MapPath("Files//_temp2.pdf"));
        File.Copy(Server.MapPath("Files/_temp.pdf"), Server.MapPath("MapPdf/" + ddlPdf.Text), true);

        loadpdf();
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        loadpdf();
    }


    protected void ddlPdf_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblPrefix.Text = "";
            lblTableName.Text = "";
            if (ddlPdf.SelectedIndex > 0)
            {

                File.Copy(Server.MapPath("MapPdf/" + ddlPdf.Text), Server.MapPath("Files/_temp_.pdf"), true);
                ListText.Items.Clear();
                File.Copy(Server.MapPath("Files/_temp_.pdf"), Server.MapPath("Files/_temp.pdf"), true);
                Stamping(Server.MapPath("Files/_temp_.pdf"), Server.MapPath("Files/_temp2.pdf"), true, ListText, "ReadMap");
                //Stamping(Server.MapPath("Files/_temp_.pdf"), Server.MapPath("Files/_temp.pdf"), false, ListText);
                //Stamping(Server.MapPath("Files/_temp_.pdf"), Server.MapPath("Files/_temp2.pdf"), true, null);
                loadpdf();
            }
            else if (ddlPdf.SelectedIndex == 0)
            {
                ListText.Items.Clear();
                tframe.Attributes["src"] = "";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error !" + ex.Message;
        }
    }
    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTable.SelectedIndex > 0)
            {
                dt = db.selectData (" select * from [" + ddlTable.Text + "]").Tables [0];
                ListField.Items.Clear();
                foreach (DataColumn col in dt.Columns)
                {
                    ListField.Items.Add(col.ColumnName);
                }
            }
            else if (ddlPdf.SelectedIndex == 0)
            {
                ListField.Items.Clear();
            }


        }
        catch (Exception ex)
        {
            lblError.Text = "Error !" + ex.Message;
        }
    }

    protected void btnTMap_Click(object sender, EventArgs e)
    {
        if (ddlTable.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Select Table')", true);            
            return;
        }
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp_2.pdf"), false, null, "Table", null, 0, "txtTable", ddlTable.Text);
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp2_2.pdf"), true, null, "Table", null, 0, "txtTable", ddlTable.Text);
        File.Delete(Server.MapPath("Files/_temp.pdf"));
        File.Move(Server.MapPath("Files/_temp_2.pdf"), Server.MapPath("Files/_temp.pdf"));
        File.Delete(Server.MapPath("Files/_temp2.pdf"));
        File.Move(Server.MapPath("Files/_temp2_2.pdf"), Server.MapPath("Files/_temp2.pdf"));
        File.Copy(Server.MapPath("Files/_temp.pdf"), Server.MapPath("MapPdf/" + ddlPdf.Text), true);
        loadpdf();
    }
    protected void btnFile_Click(object sender, EventArgs e)
    {
        if (ListField.SelectedIndex == -1)
        {
            ScriptManager.RegisterStartupScript(this,GetType(), "AlertBox", "alert('Please Select Field Name')", true);
            return;
        }
        /*if (!txtFileName.Text.EndsWith(".pdf"))
        {
            txtFileName.Text = txtFileName.Text.Trim();
            txtFileName.Text += ".pdf";
        }*/
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp_2.pdf"), false, null, "File", null, 0, "txtFile", ListField.Text);
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp2_2.pdf"), true, null, "File", null, 0, "txtFile", ListField.Text);
        File.Delete(Server.MapPath("Files/_temp.pdf"));
        File.Move(Server.MapPath("Files/_temp_2.pdf"), Server.MapPath("Files/_temp.pdf"));
        File.Delete(Server.MapPath("Files/_temp2.pdf"));
        File.Move(Server.MapPath("Files/_temp2_2.pdf"), Server.MapPath("Files/_temp2.pdf"));
        File.Copy(Server.MapPath("Files/_temp.pdf"), Server.MapPath("MapPdf/" + ddlPdf.Text), true);
        loadpdf();
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (ddlPdf.SelectedIndex > 0)
        {
            if (chkOverwrite.Checked)
            {
                File.Copy(Server.MapPath("~/MapPdf/" + ddlPdf.Text), Server.MapPath("~/TemplateStore/DownloadPdf/" + ddlPdf.Text), true);
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('File Transfered')", true);
                
            }
            else
            {
                if(File.Exists(Server.MapPath("~/TemplateStore/DownloadPdf/") + ddlPdf.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('File Already Exists')", true);                    
                }
                else
                {
                    File.Copy(Server.MapPath("~/MapPdf/" + ddlPdf.Text), Server.MapPath("~/TemplateStore/DownloadPdf/" + ddlPdf.Text));
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('File Transfered')", true);
                    
                }
            }
           // Response.ContentType = "Application/pdf";
           // Response.AppendHeader("Content-Disposition", "attachment; filename=" + ddlPdf.Text);
           // Response.TransmitFile(Server.MapPath("~/MapPdf/" + ddlPdf.Text));
           // Response.End();
        }
        else 
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Select Pdf')", true);
            return;
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (ddlPdf.SelectedIndex > 0)
        {
             if (File.Exists(Server.MapPath("~/MapPdf/") + ddlPdf.Text))
                {
                     File.Delete (Server.MapPath("~/MapPdf/" + ddlPdf.Text));
                     ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('File Deleted')", true);
                loadpdflist();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('File Not Exists')", true);
                }
           
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Select Pdf')", true);
            return;
        }
    }

    protected void btnDefault_Click(object sender, EventArgs e)
    {
        if (txtDefault.Text.Trim().Length==0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Default Value Not Empty ')", true);
            return;
        }
        if (ListText.SelectedIndex == -1 )
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AlertBox", "alert('Please Select Text ')", true);

            return;
        }
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp_2.pdf"), false, ListText, "MapDefault");
        Stamping(Server.MapPath("Files/_temp.pdf"), Server.MapPath("Files/_temp2_2.pdf"), true, ListText, "MapDefault");
        File.Delete(Server.MapPath("Files/_temp.pdf"));
        File.Move(Server.MapPath("Files/_temp_2.pdf"), Server.MapPath("Files/_temp.pdf"));
        File.Delete(Server.MapPath("Files/_temp2.pdf"));
        File.Move(Server.MapPath("Files/_temp2_2.pdf"), Server.MapPath("Files/_temp2.pdf"));
        File.Copy(Server.MapPath("Files/_temp.pdf"), Server.MapPath("MapPdf/" + ddlPdf.Text), true);
        loadpdf();

    }
}