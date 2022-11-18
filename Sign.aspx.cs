using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sign : System.Web.UI.Page
{
    Byte[] bytImage = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string str = hidBlob.Value;
        if (string.IsNullOrEmpty(str) == false)
        {
            try
            {
                string blobstring = str.Split(',')[1];
                byte[] blob = Convert.FromBase64String(blobstring);

                string path = Server.MapPath("~/Sign/");
                string fname = System.DateTime.Now.Millisecond.ToString() + ".jpg";

                File.WriteAllBytes(path + "//" + fname, blob);

                lblMessage.Text = "Image Save at " + path + " " + fname;
            }
            catch (Exception ex)
            {
            }

        }

    }
}