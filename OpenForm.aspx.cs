using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OpenForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        iframe.Src ="~/document/"+Session["outfile"].ToString();
        
        //string outputFile = Server.MapPath("~/document/DisabilityLetterNF1.docx");
        //System.Diagnostics.Process.Start(outputFile);
    }
   
}