using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
/// <summary>
/// Summary description for xmlData
/// </summary>
public class xmlData
{
    
    public static string XMLPath
    {
        get
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory.ToLower().Replace("bin", "").Replace("debug", "");
            path = System.IO.Path.Combine(path, "xml");
            path = System.IO.Path.Combine(path, "HSMData.xml");
            return path;
        }
    }
    public static string DefaultXMLPath(string sUser)
    {
        var path = System.AppDomain.CurrentDomain.BaseDirectory.ToLower().Replace("bin", "").Replace("debug", "");
        path = System.IO.Path.Combine(path, "xml");
        path = System.IO.Path.Combine(path, "Default_" + sUser + ".xml");
        return path;
    }
    public static XmlDocument DataSource
    {
        get
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlData.XMLPath);
                return xDoc;
            }
            catch (Exception ex)
            {
                //GeneralHelper.Information(ex.Message);
                return null;
            }
        }
    }
}