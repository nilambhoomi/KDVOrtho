using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic;
/// <summary>
/// Summary description for Safe
/// </summary>
public class Safe
{
    public static bool IsValidDateTimeTest(string dateTime)
    {
        string[] formats = { "MM/dd/yyyy" };
        DateTime parsedDateTime;
        return DateTime.TryParseExact(dateTime, formats, System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None, out parsedDateTime);
    }

    public static DateTime? GetSafeDate1(string dateTime)
    {
        var usDtfi = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        return Convert.ToDateTime(dateTime, usDtfi);
    }

    public static DateTime? GetSafeDate(string dateTime)
    {
        var usDtfi = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;

        if (IsValidDateTimeTest(dateTime))
            return Convert.ToDateTime(dateTime, usDtfi);
        else
            return null;
    }
    public static string GetSafeDateString(object dateTime)
    {
        try
        {
            if (dateTime != null)
                return Convert.ToDateTime(dateTime).ToString("MM/dd/yyyy");
            else
                return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
    public static string GetSafeDateValueString(DateTime? dateTime)
    {
        if (dateTime != null)
            return Convert.ToDateTime(dateTime).ToString("MM/dd/yyyy");
        else
            return string.Empty;
    }

    public static Int32 GetSafeInteger(object value)
    {
        if (value != null && DBNull.Value != value)
        {
            if (value.ToString() != string.Empty)
                return Convert.ToInt32(value);
            else
                return 0;
        }
        else
            return 0;
    }
    public static long GetSafeLong(object value)
    {
        if (value != null && DBNull.Value != value)
        {
            if (value.ToString() != string.Empty)
                return Convert.ToInt64(value);
            else
                return 0;
        }
        else
            return 0;
    }

    public static Boolean GetSafeBoolean(object value)
    {
        if (value != null && DBNull.Value != value)
        {
            if (Convert.ToBoolean(value))
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public static Boolean GetSafeBooleanD(string value)
    {
        if (value == "0" || value == "false" || value == "False")
            return false;
        else
            return true;
    }

    public static bool IsNumeric(object value)
    {
        int test;
        return int.TryParse((string)value, out test);
    }
    public static bool IsNumericAndNonZero(object value)
    {
        int test;
        if (int.TryParse((string)value, out test))
        {
            return Convert.ToInt32(value) > 0 ? true : false;
        }
        else
            return false;
    }
}