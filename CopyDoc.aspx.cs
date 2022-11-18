using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CopyDoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        CloneDirectory("PatientDocument_S", "PatientDocument");

    }

    private void CloneDirectory(string src, string target)
    {
       DirectoryInfo rootDirectoryInfo = new DirectoryInfo(Server.MapPath( src));
        DirectoryInfo targetDirectoryInfo = new DirectoryInfo(Server.MapPath(target));
        foreach (DirectoryInfo dir in rootDirectoryInfo.GetDirectories())
        {
            if (!Directory.Exists(targetDirectoryInfo.FullName + "\\" + dir.Name))
                Directory.CreateDirectory(targetDirectoryInfo.FullName + "\\" + dir.Name);

            Files(dir.FullName,targetDirectoryInfo.FullName+"\\"+dir.Name ) ;
        }
        foreach (FileInfo file in rootDirectoryInfo.GetFiles())
        {
            if (File.Exists(targetDirectoryInfo.FullName + "\\" + file.Name))
                Label1.Text += file.FullName + " : Exists <br>";
            else
            {
                File.Copy(file.FullName, targetDirectoryInfo.FullName + "\\" + file.Name);
                Label1.Text += file.FullName + " : Copy <br>";
            }
        }

    }
    private void Files(string dir, string targetdir)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
        DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetdir);
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        {
            if (!Directory.Exists(targetDirectoryInfo.FullName + "\\" + directory.Name))
                Directory.CreateDirectory(targetDirectoryInfo.FullName + "\\" + directory.Name);

            Files(directory.FullName, targetDirectoryInfo.FullName + "\\" + directory.Name);

        }
        foreach (FileInfo file in directoryInfo.GetFiles())
        {
            if (File.Exists(targetDirectoryInfo.FullName + "\\" + file.Name))
                Label1.Text += file.FullName + " : Exists <br>";
            else
            {
                File.Copy(file.FullName, targetDirectoryInfo.FullName + "\\" + file.Name);
                Label1.Text += file.FullName + " : Copy <br>";
            }

        }

    }
}