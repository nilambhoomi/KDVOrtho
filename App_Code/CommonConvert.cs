using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommonConvert
/// </summary>
public class CommonConvert
{
    public CommonConvert()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool ToBoolean(dynamic variable)
    {
        try
        {
            // if (Convert.IsDBNull(variable) ||variable == "") return variable = false;
            if (variable != null || variable != "")
            { return Convert.ToBoolean(variable); }
            else
            { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static string DateFormat(string d)
    {
        string str = "";
        if (!string.IsNullOrEmpty(d))
        {
            str = Convert.ToDateTime(d).ToString("MM/dd/yyyy");
        }
        return str;
    }

    public static string FullDateFormat(string d)
    {
        string str = "";
        if (!string.IsNullOrEmpty(d))
        {
            str = Convert.ToDateTime(d).ToString("MMMM dd, yyyy");
        }
        return str;
    }

    public static string DateFormatPrint(string d)
    {
        string str = "";
        if (!string.IsNullOrEmpty(d))
        {
            str = Convert.ToDateTime(d).ToString("MMddyyyy");
        }
        return str;
    }

    public static string UppercaseFirst(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }

    public static string DateformatDOA(string d)
    {
        string str = "";
        if (!string.IsNullOrEmpty(d))
        {
            str = d.Insert(2, "/");
            str = str.Insert(5, "/");
           // str = str.Insert(9, "/");


        }
        return str;
    }
}