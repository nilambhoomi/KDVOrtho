using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Drawing;
using System.Data.SqlClient;

public partial class WebCam_baseimg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //called page form json for creating imgBase64 in image

        StreamReader reader = new StreamReader(Request.InputStream);
        String Data = Server.UrlDecode(reader.ReadToEnd());
        reader.Close();

        DateTime nm = DateTime.Now;
        string date = nm.ToString("yyyymmddMMss");
        //used date for creating Unique image name

        Session["capturedImageURL"] = Server.MapPath("~/Userimages/") + date + ".jpeg";

        Session["Imagename"] = date + ".jpeg";
        // We can use name of image where ever we required that why we are storing in Session

        drawimg(Data.Replace("imgBase64=data:image/png;base64,", String.Empty), Session["capturedImageURL"].ToString());
        // it is method 
        // passing base64 string and string filename to Draw Image.
        if (!string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        {
            insertImagePath(Convert.ToInt32(Session["UserID"]), "~/Userimages/" + Session["Imagename"].ToString());
        }

        // it is method 
        //inserting image in to database 

    }

    public void drawimg(string base64, string filename)  // Drawing image from Base64 string.
    {
        using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                byte[] data = Convert.FromBase64String(base64);
                bw.Write(data);
                bw.Close();
            }
        }
    }

    public void insertImagePath(int UserID, string Propicpath) // use for imserting image when it is created.
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);

        SqlCommand cmd = new SqlCommand("UpdateProPic", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UserID", UserID);
        cmd.Parameters.AddWithValue("@Propic", Propicpath);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

}
