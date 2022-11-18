using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestPage : System.Web.UI.Page
{



    protected void Page_PreInit(object sender, EventArgs e)
    {


    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //string path = Server.MapPath("~/Template/Demo.txt");
        //string body = File.ReadAllText(path);

        //string temp = new PrintDocumentHelper().getDocumentStringDenies(body);

        PdfStampInExistingFile("Hello world");

        // setPDF();


    }


    protected void btnTest_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hidval.Value))
            Response.Write(hidval.Value.TrimStart(','));
    }

    protected void btnLoadcheckbox_Click(object sender, EventArgs e)
    {
        hidval.Value = "";
        CheckBoxList cbList = new CheckBoxList();
        cbList.ID = "chkList";

        for (int i = 0; i < 10; i++)
        {
            cbList.Items.Add(new ListItem("Checkbox " + i.ToString(), i.ToString()));
            cbList.Items[i].Attributes.Add("onclick", "chekcVal(this," + i.ToString() + ")");
            //cbList.Items[i].Attributes.Add("text", i.ToString());
        }

        placeHolder.Controls.Add(cbList);
    }

    public void demoFunction()
    {
        string NameTest = ",,Strainght leg raise,Braggard's test,Kernig's sign,Brudzinski's ,Sacroiliac compression,Sacral notch tenderness,Ober's test causing pain at the SI joint",
            LeftTest = ",1,1,1,0,1,0,1", RightTest = ",1,1,1,1,1,1,1", TextVal = ",23";

        NameTest = NameTest.TrimStart(',');
        string[] NameTestVal = NameTest.TrimStart(',').Split(',');
        string[] LeftTestVal = LeftTest.TrimStart(',').Split(',');
        string[] RightTestVal = RightTest.TrimStart(',').Split(',');
        string[] TextValVal = TextVal.Split(',');

        string str = "", leftval = "", rightval = "";
        for (int i = 0; i < NameTestVal.Length; i++)
        {

            if (i == 0)
            {
                if (!string.IsNullOrEmpty(TextValVal[0]))
                {
                    leftval = " at " + TextValVal[0] + " degrees ";
                }
                if (!string.IsNullOrEmpty(TextValVal[1]))
                {
                    rightval = " at " + TextValVal[1] + " degrees ";
                }
            }
            else
            {
                leftval = ""; rightval = "";
            }

            if (LeftTestVal[i] == "1")
            {
                str = str + "," + NameTestVal[i] + " is positive on left " + leftval;
            }
            if (RightTestVal[i] == "1")
            {
                if (LeftTestVal[i] == "1")
                    str = str + " and on the right" + rightval;
                else
                    str = str + "," + NameTestVal[i] + " is positive on right" + rightval;
            }
        }
        Response.Write(str);

    }


    public void setPDF()
    {
        string imgpath = Server.MapPath("~/Sign/21.jpg");
        string pdfpath = Server.MapPath("~/TemplateStore/Forms/Nf packet.pdf");
        string pdfpathourput = Server.MapPath("~/TemplateStore/Forms/Demo.pdf");

        using (Stream inputPdfStream = new FileStream(pdfpath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream inputImageStream = new FileStream(imgpath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream outputPdfStream = new FileStream(pdfpathourput, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);

            int val = reader.NumberOfPages;

            var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

            var pdfContentByte = stamper.GetOverContent(1);

            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);

            image.SetAbsolutePosition(759f, 459f);

            pdfContentByte.AddImage(image);
            stamper.Close();
        }
    }

    private void PdfStampInExistingFile(string text)
    {
        // string sourceFilePath = @"C:\Users\anand\Desktop\Test.pdf";

        string sourceFilePath = Server.MapPath("~/TemplateStore/Forms/Nf packet.pdf");
        byte[] bytes = File.ReadAllBytes(sourceFilePath);
        Bitmap bitmap = new Bitmap(200, 30, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        graphics.DrawString(text, new System.Drawing.Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), new PointF(0.4F, 2.4F));
        bitmap.Save(Server.MapPath("~/Image.jpg"), ImageFormat.Jpeg);
        bitmap.Dispose();
        var img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Sign/21.jpg"));
        img.SetAbsolutePosition(200, 400);
        PdfContentByte waterMark;
        using (MemoryStream stream = new MemoryStream())
        {
            PdfReader reader = new PdfReader(bytes);
            PdfStamper stamper = new PdfStamper(reader, stream);

            int pages = reader.NumberOfPages;
            for (int i = 1; i <= pages; i++)
            {
                waterMark = stamper.GetUnderContent(i);
                waterMark.AddImage(img);
            }

            bytes = stream.ToArray();
        }
        //File.Delete(Server.MapPath("~/Image.jpg"));
        File.WriteAllBytes(sourceFilePath, bytes);
    }
}