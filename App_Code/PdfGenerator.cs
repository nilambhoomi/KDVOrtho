using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for PdfGenerator
/// </summary>
public class PdfGenerator
{
    SqlConnection cn;
    public PdfGenerator()
	{
	    cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ToString());	
	}
    public DataTable GetData(string qry)
    {
        try
        {
            SqlCommand cm = new SqlCommand(qry, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            DataTable dt = new DataTable();
            dt.Columns.Add("Error", typeof(string));
            dt.Rows.Add(ex.Message);
            dt.TableName = "Error";
            return  dt;
        }
    }
    public void Stamping(string SourceFile,string ColumnName, string ID ,Control ctrl )
    {

        //string pdfTemplate = openFileDialog.FileName;
        //string newFile = Application.StartupPath + "/_temp.pdf";

        
        PdfReader pdfReader = new PdfReader(SourceFile);
        AcroFields readPdfFields = pdfReader.AcroFields;
        String tabname = readPdfFields.GetField("txtTable");
        HttpContext.Current.Response.Write(readPdfFields.GetField("txtTable"));
        HttpContext.Current.Response.Write(readPdfFields.GetField("txtFile"));
        DataTable dt = GetData("select * from " + tabname + " where " + ColumnName + "=" + ID);
        string fileprefix = "1";
        try
        {
            fileprefix = dt.Rows[0][readPdfFields.GetField("txtFile")].ToString();
        }catch { }
        //HttpContext.Current.Response.Write(dt.Rows.Count);
        //HttpContext.Current.Response.Write(fileprefix);


       // PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(HttpContext.Current.Server.MapPath(fileprefix+"_"+Path.GetFileName ( SourceFile)), FileMode.Create));
        MemoryStream pdfOutput = new MemoryStream();
        PdfStamper pdfStamper = new PdfStamper(pdfReader, pdfOutput );
        AcroFields pdfFormFields = pdfStamper.AcroFields;
        pdfStamper.FormFlattening = false ;
        AcroFields ae = pdfReader.AcroFields;
        //foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in pdfReader.AcroFields.Fields)
        foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in pdfReader.AcroFields.Fields)
        {
                string textvalue = pdfFormFields.GetField(de.Key.ToString());
                string[] textpair = textvalue.Split('|');
            if (textpair.Length > 1)
            {
                try
                {
                    if (dt.Rows[0][textpair[1]] is DateTime)
                        pdfFormFields.SetField(textpair[0], DateTime.Parse(dt.Rows[0][textpair[1]].ToString()).ToString("MM/dd/yyyy"));
                    else
                        pdfFormFields.SetField(textpair[0], dt.Rows[0][textpair[1]].ToString());

                        if (textpair.Length > 2)
                        {
                            if (dt.Rows[0][textpair[1]] == null || dt.Rows[0][textpair[1]].ToString().Trim()== string.Empty)
                                pdfFormFields.SetField(textpair[0], textpair[2]);
                            else
                                pdfFormFields.SetField(textpair[0], "");
                        }
                    
                }
                catch (Exception ex)
                {
                        pdfFormFields.SetField(textpair[0], "");
                   
                }
            }
            else
            {
                if (!de.Key.ToLower().StartsWith("txtfix"))
                {
                    if (ctrl != null)
                    {
                        if (de.Key.StartsWith("@"))
                        {
                            try
                            {
                                TextBox txt = (TextBox)  ctrl.FindControl ( de.Key.Substring(1));
                                pdfFormFields.SetField(de.Key.ToString(), txt.Text );
                            }
                            catch (Exception ex)
                            {
                                pdfFormFields.SetField(textpair[0], "");
                            }
                        }
                        else
                          pdfFormFields.SetField(textpair[0], "");
                    }
                    else
                    {
                        pdfFormFields.SetField(textpair[0], "");
                    }

                }
                if (de.Key.StartsWith("#"))
                {
                    try
                    {
                        pdfFormFields.SetField(de.Key.ToString(), dt.Rows[0][de.Key.Substring(1)].ToString());
                    }
                    catch (Exception ex)
                    {
                        pdfFormFields.SetField(textpair[0], "");
                    }
                }
                else
                    pdfFormFields.SetField(textpair[0], "");

            }
            if (de.Key.ToLower().StartsWith ("imgsign"))
                {
                try
                {
                    // DataTable dts = GetData("select * from tblPatientIESign where " + ColumnName + "=" + ID);
                    string[] files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("Sign"), ID+"_*.jp*g", System.IO.SearchOption.TopDirectoryOnly);
                    if (files.Length > 0)
                    {
                        Stream inputImageStream = new FileStream(files[0], FileMode.Open, FileAccess.Read, FileShare.Read);
                        AcroFields.FieldPosition f = ae.GetFieldPositions(de.Key.ToString())[0];
                        var pdfContentByte = pdfStamper.GetOverContent(f.page);
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                        image.ScaleToFit(f.position.Width, f.position.Height + 10);

                        image.SetAbsolutePosition(f.position.Left, f.position.Bottom);
                        pdfContentByte.AddImage(image);
                    }
                //    pdfFormFields.SetFieldProperty(de.Key.ToString(), "flags", PdfFormField.FLAGS_NOVIEW, null);
                }

                catch (Exception ex) {  }
                finally { pdfFormFields.SetFieldProperty(de.Key.ToString(), "flags", PdfFormField.FLAGS_NOVIEW, null); }
                 }
               
        }
        //try
        //{
        //    AcroFields.FieldPosition f = ae.GetFieldPositions("imgsign")[0];
        //    var pdfContentByte = pdfStamper.GetOverContent(f.page);
        //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
        //    image.ScaleToFit(f.position.Width, f.position.Height + 10);
            
        //    image.SetAbsolutePosition(f.position.Left, f.position.Bottom);
        //    pdfContentByte.AddImage(image);

        //    pdfFormFields.SetFieldProperty("imgsign", "flags", PdfFormField.FLAGS_NOVIEW, null);
        //}
        //catch { }
        pdfStamper.Close();
        pdfReader.Close();
        try
        {
            var response = HttpContext.Current.Response;
            response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileprefix + "_" + Path.GetFileName(SourceFile) + "\"");
            response.ContentType = "application/pdf";
            response.BinaryWrite(pdfOutput.ToArray ());
            response.End();
        }
        catch (Exception ex)
        {
           // Logger.Error(ex);
        }
        
    }
}