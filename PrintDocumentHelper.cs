using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for PrintDocumentHelper
/// </summary>
public class PrintDocumentHelper
{
    public PrintDocumentHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string getDocumentString(string html)
    {
        String str = html;
        //String pattern = @"(<input\s*(.+?)\s*/>|<label>\s*(.+?)\s*</label>)";
        String pattern = @"(<input\s*(.+?)\s*>|<label.*>\s*(.+?)\s*</label>|<textarea.*>\s*(.+?)\s*</textarea>)";
        //String pattern = @"<input\s*(.+?)\s*/>";
        //String pattern = @"<label>\s*(.+?)\s*</label>";
        RegexOptions regexOptions = RegexOptions.Multiline;
        Regex regex = new Regex(pattern, regexOptions);
        StringBuilder sb = new StringBuilder();
        Dictionary<string, string> d = new Dictionary<string, string>();
        string prevtype = "", type = "";
        int chkgrp = 0;
        foreach (Match match in regex.Matches(str))
        {
            prevtype = type;
            if (match.Success)
            {
                //for (int i = 0; i < match.Groups.Count - 1; i++)
                //   {
                //       Response.Write("Match ["+i.ToString()+"]: " + match.Groups[i].Value + "<br>");
                //   }
                if (match.Groups[2].Value.Length != 0)
                {
                    String tagvalue = match.Groups[2].Value;

                    String[] attrib = tagvalue.Split(' ');
                    string id = "", value = "";
                    bool check = false;



                    foreach (string att in attrib)
                    {
                        if (att.ToLower().StartsWith("id"))
                        {
                            String[] test = att.Split('=');
                            id = test[1];
                        }
                        if (att.ToLower().StartsWith("value"))
                        {
                            String[] test = att.Split('=');
                            int startindex = tagvalue.IndexOf("value=") + 7;
                            int endindex = tagvalue.IndexOf("\"", startindex);
                            value = tagvalue.Substring(startindex, endindex - startindex);
                            //        Response.Write("value=" + value  + "<br>");
                            //        Response.Write(value + "<br>");
                        }
                        if (att.ToLower().StartsWith("checked"))
                        {
                            check = true;
                        }

                        if (att.ToLower().StartsWith("type"))
                        {
                            String[] test = att.Split('=');
                            type = test[1].ToLower();
                            Regex rx = new Regex(@"^\s*""?|""?\s*$");

                            type = rx.Replace(type, "");
                            //      Response.Write("type="+type+"<br>");
                        }
                        // d.ToString();
                    }

                    //Response.Write(prevtype);
                    if (type == "checkbox" || type == "radio")
                    {

                        Regex rx = new Regex(@"^\s*""?|""?\s*$");
                        id = rx.Replace(id, "");
                        value = rx.Replace(value, "");
                        if (value.Length > 0 && check)
                        {
                            chkgrp += 1;
                            sb.Append(value + ", ");
                        }
                    }
                    else if (type == "text")
                    {
                        //  Regex rx = new Regex(@"^\s*""?|""?\s*$");
                        // value = rx.Replace(value, "");
                        //    Response.Write(value);
                        sb.Append(value + " ");
                    }


                }
                else if (match.Groups[2].Value.Length == 0)
                {
                    if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
                    {
                        sb.Remove(sb.Length - 2, 2).Append(" ");
                        if (chkgrp > 0)
                        {
                            if (sb.ToString().LastIndexOf(",") >= 0)
                                sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                        }
                        chkgrp = 0;
                    }
                    String tagvalue = match.Groups[1].Value;
                    Regex rx = new Regex("<[^>]*>");
                    tagvalue = rx.Replace(tagvalue, "");
                    sb.Append(tagvalue);
                }
            }
        }
        if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
        {
            sb.Remove(sb.Length - 2, 2).Append(" ");
            if (chkgrp > 0)
            {
                if (sb.ToString().LastIndexOf(",") >= 0)
                    sb.Replace(",", " and", sb.ToString().LastIndexOf(","), 1);
            }
            chkgrp = 0;
        }
        return sb.Replace(" .", ". ").Replace(".", ". ").ToString();
    }

    public string getDocumentStringDenies(string html)
    {
        String str = html;
        //String pattern = @"(<input\s*(.+?)\s*/>|<label>\s*(.+?)\s*</label>)";
        String pattern = @"(<input\s*(.+?)\s*>|<label.*>\s*(.+?)\s*</label>)";
        //String pattern = @"<input\s*(.+?)\s*/>";
        //String pattern = @"<label>\s*(.+?)\s*</label>";
        RegexOptions regexOptions = RegexOptions.Multiline;
        Regex regex = new Regex(pattern, regexOptions);
        StringBuilder sb = new StringBuilder();
        Dictionary<string, string> d = new Dictionary<string, string>();
        string prevtype = "", type = "";
        int chkgrp = 0;
        foreach (Match match in regex.Matches(str))
        {
            prevtype = type;
            if (match.Success)
            {
                //for (int i = 0; i < match.Groups.Count - 1; i++)
                //   {
                //       Response.Write("Match ["+i.ToString()+"]: " + match.Groups[i].Value + "<br>");
                //   }
                if (match.Groups[2].Value.Length != 0)
                {
                    String tagvalue = match.Groups[2].Value;

                    String[] attrib = tagvalue.Split(' ');
                    string id = "", value = "";
                    bool check = false;



                    foreach (string att in attrib)
                    {
                        if (att.ToLower().StartsWith("id"))
                        {
                            String[] test = att.Split('=');
                            id = test[1];
                        }
                        if (att.ToLower().StartsWith("value"))
                        {
                            String[] test = att.Split('=');
                            int startindex = tagvalue.IndexOf("value=") + 7;
                            int endindex = tagvalue.IndexOf("\"", startindex);
                            value = tagvalue.Substring(startindex, endindex - startindex);
                            //        Response.Write("value=" + value  + "<br>");
                            //        Response.Write(value + "<br>");
                        }
                        if (att.ToLower().StartsWith("checked"))
                        {
                            check = true;
                        }

                        if (att.ToLower().StartsWith("type"))
                        {
                            String[] test = att.Split('=');
                            type = test[1].ToLower();
                            Regex rx = new Regex(@"^\s*""?|""?\s*$");

                            type = rx.Replace(type, "");
                            //      Response.Write("type="+type+"<br>");
                        }
                        // d.ToString();
                    }

                    //Response.Write(prevtype);
                    if (type == "checkbox" || type == "radio")
                    {

                        Regex rx = new Regex(@"^\s*""?|""?\s*$");
                        id = rx.Replace(id, "");
                        value = rx.Replace(value, "");
                        if (value.Length > 0 && check == false)
                        {
                            chkgrp += 1;
                            sb.Append(value + ", ");
                        }
                    }



                }
                else if (match.Groups[2].Value.Length == 0)
                {
                    if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
                    {
                        sb.Remove(sb.Length - 2, 2).Append(" ");
                        if (chkgrp > 0)
                        {
                            if (sb.ToString().LastIndexOf(",") >= 0)
                                sb.Replace(",", " and", sb.ToString().LastIndexOf(","), 1);
                        }
                        chkgrp = 0;
                    }
                    String tagvalue = match.Groups[1].Value;
                    Regex rx = new Regex("<[^>]*>");
                    tagvalue = rx.Replace(tagvalue, "");
                    sb.Append(tagvalue);
                }
            }
        }
        if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
        {
            sb.Remove(sb.Length - 2, 2).Append(" ");
            if (chkgrp > 0)
            {
                if (sb.ToString().LastIndexOf(",") >= 0)
                    sb.Replace(",", " and", sb.ToString().LastIndexOf(","), 1);
            }
            chkgrp = 0;
        }
        return sb.Replace(" .", ". ").Replace(".", ". ").ToString();
    }

    public string getDocumentStringLeftRight(string html, string bodypart = "")
    {

        String strfile = html;
        String[] str = new String[2];
        StringBuilder sb = new StringBuilder();
        int leftstart, leftend, rightstart, rightend;
        leftstart = strfile.IndexOf(@"<div id=""WrapLeft""");
        leftend = strfile.IndexOf(@"</div>", leftstart);
        str[0] = strfile.Substring(leftstart, leftend - leftstart);
        rightstart = strfile.IndexOf(@"<div id=""WrapRight""");
        rightend = strfile.IndexOf(@"</div>", rightstart);
        str[1] = strfile.Substring(rightstart, rightend - rightstart);

        //            Response.Write(str[0]);
        //          Response.Write(str[1]);
        Regex rex = new Regex(@"<div.*display:[ ]*none.*>");
        if (rex.IsMatch(str[0]))
            str[0] = "";
        if (rex.IsMatch(str[1]))
            str[1] = "";
        for (int i = 0; i < 2; i++)
        {
            //String pattern = @"(<input\s*(.+?)\s*/>|<label>\s*(.+?)\s*</label>)";
            String pattern = @"(<input\s*(.+?)\s*>|<label.*>\s*(.+?)\s*</label>|<textarea.*>\s*(.+?)\s*</textarea>)";
            //String pattern = @"<input\s*(.+?)\s*/>";
            //String pattern = @"<label>\s*(.+?)\s*</label>";
            RegexOptions regexOptions = RegexOptions.Multiline;
            Regex regex = new Regex(pattern, regexOptions);

            Dictionary<string, string> d = new Dictionary<string, string>();
            string prevtype = "", type = "";
            int chkgrp = 0;

            if (!string.IsNullOrEmpty(bodypart))
            {
                if (i == 0 && str[0] != "")
                {
                    sb.Append("<b><u>LEFT " + bodypart.ToUpper() + "</u>: </b> ");
                }
                else if (i == 1 && str[1] != "")
                {
                    sb.Append("<b><u>RIGHT " + bodypart.ToUpper() + "</u>: </b> ");
                }
            }

            foreach (Match match in regex.Matches(str[i]))
            {
                prevtype = type;
                if (match.Success)
                {

                    /*  for (int i = 0; i < match.Groups.Count - 1; i++)
                         {
                             Response.Write("Match ["+i.ToString()+"]: " + match.Groups[i].Value + "<br>");
                         }*/
                    if (match.Groups[2].Value.Length != 0)
                    {
                        String tagvalue = match.Groups[2].Value;

                        String[] attrib = tagvalue.Split(' ');
                        string id = "", value = "";
                        bool check = false;



                        foreach (string att in attrib)
                        {
                            if (att.ToLower().StartsWith("id"))
                            {
                                String[] test = att.Split('=');
                                id = test[1];
                            }
                            if (att.ToLower().StartsWith("value"))
                            {
                                String[] test = att.Split('=');
                                int startindex = tagvalue.IndexOf("value=") + 7;
                                int endindex = tagvalue.IndexOf("\"", startindex);
                                value = tagvalue.Substring(startindex, endindex - startindex);
                                //        Response.Write("value=" + value  + "<br>");
                                //        Response.Write(value + "<br>");
                            }
                            if (att.ToLower().StartsWith("checked"))
                            {
                                check = true;
                            }

                            if (att.ToLower().StartsWith("type"))
                            {
                                String[] test = att.Split('=');
                                type = test[1].ToLower();
                                Regex rx = new Regex(@"^\s*""?|""?\s*$");

                                type = rx.Replace(type, "");
                                //      Response.Write("type="+type+"<br>");
                            }
                            // d.ToString();
                        }

                        //Response.Write(prevtype);

                        if (type == "checkbox" || type == "radio")
                        {

                            Regex rx = new Regex(@"^\s*""?|""?\s*$");
                            id = rx.Replace(id, "");
                            value = rx.Replace(value, "");
                            //if (value.Length > 0 && check)
                            string strVal = (string)value;
                            if (value.Length > 0 && check && strVal.Substring(strVal.Length - 1) == ".")
                            {
                                chkgrp += 1;
                                sb.Append(value);
                            }
                            else if (value.Length > 0 && check)
                            {
                                chkgrp += 1;
                                sb.Append(value + ",");
                            }
                        }
                        else if (type == "text")
                        {
                            //  Regex rx = new Regex(@"^\s*""?|""?\s*$");
                            // value = rx.Replace(value, "");
                            //    Response.Write(value);
                            sb.Append(value + " ");
                        }


                    }
                    else if (match.Groups[2].Value.Length == 0)
                    {
                        if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
                        {
                            if (sb.ToString().Contains(','))
                            {
                                //sb.Remove(sb.Length - 2, 2).Append(" ");
                                if (chkgrp > 0)
                                {
                                    if (sb.ToString().LastIndexOf(",") >= 0)
                                        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                                }
                            }
                            chkgrp = 0;
                        }
                        String tagvalue = match.Groups[1].Value;
                        Regex rx = new Regex("<[^>]*>");
                        tagvalue = rx.Replace(tagvalue, "");
                        sb.Append(tagvalue);
                    }
                }
            }
            if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 1)
            {
              //  sb.Remove(sb.Length - 2, 2).Append(" ");
                if (chkgrp > 0)
                {
                    if (sb.ToString().LastIndexOf(",") >= 0)
                        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                }
                chkgrp = 0;
            }
            if (i == 0)
            {
                if (!string.IsNullOrEmpty(str[0]))
                    sb.Append("<br/><br/>");
            }
        }
        return sb.Replace(" .", ". ").Replace(".",". ").ToString();
    }

    public string getDocumentStringLeftRightPE(string html, string bodypart = "")
    {

        String strfile = html;
        String[] str = new String[2];
        StringBuilder sb = new StringBuilder();
        int leftstart, leftend, rightstart, rightend;
        leftstart = strfile.IndexOf(@"<div id=""WrapLeftPE""");
        leftend = strfile.IndexOf(@"</div>", leftstart);
        str[0] = strfile.Substring(leftstart, leftend - leftstart);
        rightstart = strfile.IndexOf(@"<div id=""WrapRightPE""");
        rightend = strfile.IndexOf(@"</div>", rightstart);
        str[1] = strfile.Substring(rightstart, rightend - rightstart);

        //            Response.Write(str[0]);
        //          Response.Write(str[1]);
        Regex rex = new Regex(@"<div.*display:[ ]*none.*>");
        if (rex.IsMatch(str[0]))
            str[0] = "";
        if (rex.IsMatch(str[1]))
            str[1] = "";
        for (int i = 0; i < 2; i++)
        {
            //String pattern = @"(<input\s*(.+?)\s*/>|<label>\s*(.+?)\s*</label>)";
            String pattern = @"(<input\s*(.+?)\s*>|<label.*>\s*(.+?)\s*</label>|<textarea.*>\s*(.+?)\s*</textarea>)";
            //String pattern = @"<input\s*(.+?)\s*/>";
            //String pattern = @"<label>\s*(.+?)\s*</label>";
            RegexOptions regexOptions = RegexOptions.Multiline;
            Regex regex = new Regex(pattern, regexOptions);

            Dictionary<string, string> d = new Dictionary<string, string>();
            string prevtype = "", type = "";
            int chkgrp = 0;

            if (!string.IsNullOrEmpty(bodypart))
            {
                if (i == 0 && string.IsNullOrEmpty(str[0]) == false)
                {
                    sb.Append("<b>LEFT " + bodypart.ToUpper() + " EXAMINATION: </b> ");
                }
                else if (i == 1 && string.IsNullOrEmpty(str[1]) == false)
                {
                    sb.Append("<b>RIGHT " + bodypart.ToUpper() + " EXAMINATION: </b> ");
                }
            }

            foreach (Match match in regex.Matches(str[i]))
            {
                prevtype = type;
                if (match.Success)
                {

                    /*  for (int i = 0; i < match.Groups.Count - 1; i++)
                         {
                             Response.Write("Match ["+i.ToString()+"]: " + match.Groups[i].Value + "<br>");
                         }*/
                    if (match.Groups[2].Value.Length != 0)
                    {
                        String tagvalue = match.Groups[2].Value;

                        String[] attrib = tagvalue.Split(' ');
                        string id = "", value = "";
                        bool check = false;



                        foreach (string att in attrib)
                        {
                            if (att.ToLower().StartsWith("id"))
                            {
                                String[] test = att.Split('=');
                                id = test[1];
                            }
                            if (att.ToLower().StartsWith("value"))
                            {
                                String[] test = att.Split('=');
                                int startindex = tagvalue.IndexOf("value=") + 7;
                                int endindex = tagvalue.IndexOf("\"", startindex);
                                value = tagvalue.Substring(startindex, endindex - startindex);
                                //        Response.Write("value=" + value  + "<br>");
                                //        Response.Write(value + "<br>");
                            }
                            if (att.ToLower().StartsWith("checked"))
                            {
                                check = true;
                            }

                            if (att.ToLower().StartsWith("type"))
                            {
                                String[] test = att.Split('=');
                                type = test[1].ToLower();
                                Regex rx = new Regex(@"^\s*""?|""?\s*$");

                                type = rx.Replace(type, "");
                                //      Response.Write("type="+type+"<br>");
                            }
                            // d.ToString();
                        }

                        //Response.Write(prevtype);
                        if (type == "checkbox" || type == "radio")
                        {

                            Regex rx = new Regex(@"^\s*""?|""?\s*$");
                            id = rx.Replace(id, "");
                            value = rx.Replace(value, "");
                            if (value.Length > 0 && check)
                            {
                                chkgrp += 1;
                                sb.Append(value + ", ");
                            }
                        }
                        else if (type == "text")
                        {
                            //  Regex rx = new Regex(@"^\s*""?|""?\s*$");
                            // value = rx.Replace(value, "");
                            //    Response.Write(value);
                            sb.Append(value + " ");
                        }


                    }
                    else if (match.Groups[2].Value.Length == 0)
                    {
                        if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
                        {
                            sb.Remove(sb.Length - 2, 2).Append(" ");
                            if (chkgrp > 0)
                            {
                                if (sb.ToString().LastIndexOf(",") >= 0)
                                    sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                            }
                            chkgrp = 0;
                        }
                        String tagvalue = match.Groups[1].Value;
                        Regex rx = new Regex("<[^>]*>");
                        tagvalue = rx.Replace(tagvalue, "");
                        sb.Append(tagvalue);
                    }
                }
            }
            if ((prevtype == "checkbox" || prevtype == "radio") && chkgrp > 0)
            {
                sb.Remove(sb.Length - 2, 2).Append(" ");
                if (chkgrp > 0)
                {
                    if (sb.ToString().LastIndexOf(",") >= 0)
                        sb.Replace(",", " and ", sb.ToString().LastIndexOf(","), 1);
                }
                chkgrp = 0;
            }
            if (i == 0)
            {
                if (!string.IsNullOrEmpty(str[0]))
                    sb.Append("<br/><br/>");
            }
        }
        return sb.Replace(" .", ". ").Replace(".",". ").ToString();
    }

    public string getLowbackTestString(string NameTest, string LeftTest, string RightTest, string TextVal)
    {


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
        return str;

    }

    public Dictionary<string, string> getPage1String(string strcontent)
    {
        String strfile = strcontent;
        String[] str = new String[2];
        String pattern = @"(<input\s*(.+?)\s*>|<textarea.*>\s*(.*?)\s*</textarea>)";
        RegexOptions regexOptions = RegexOptions.Multiline;
        Regex regex = new Regex(pattern, regexOptions);
        StringBuilder sb = new StringBuilder();
        Dictionary<string, string> d = new Dictionary<string, string>();


        foreach (Match match in regex.Matches(strfile))
        {
            if (match.Success)
            {
                /*                       for (int j = 0; j < match.Groups.Count - 1; j++)
                                         {
                                              Response.Write(Server.HtmlEncode ("Match [" + j.ToString() + "]: " + match.Groups[j].Value + "")+"<br>");
                                            }*/
                if (match.Groups[2].Value.Length != 0)
                {
                    String tagvalue = match.Groups[2].Value;
                    String[] attrib = tagvalue.Split(' ');
                    string id = "", value = "", type = "", chk = "false";
                    foreach (string att in attrib)
                    {

                        if (att.ToLower().StartsWith("id"))
                        {
                            String[] attele = att.Split('=');
                            id = attele[1].ToString().Trim('"');

                        }
                        if (att.ToLower().StartsWith("value"))
                        {
                            String[] test = att.Split('=');
                            int startindex = tagvalue.IndexOf("value=") + 7;
                            int endindex = tagvalue.IndexOf("\"", startindex);
                            value = tagvalue.Substring(startindex, endindex - startindex);
                        }
                        if (att.ToLower().StartsWith("type"))
                        {
                            String[] test = att.Split('=');
                            type = test[1].ToLower();
                            Regex rx = new Regex(@"^\s*""?|""?\s*$");

                            type = rx.Replace(type, "");
                            //      Response.Write("type="+type+"<br>");
                        }
                        if (type == "checkbox" || type == "radio")
                        {

                            if (att.ToLower().StartsWith("checked=\"checked\""))
                            {
                                //                                             Response.Write(string.Join (":",attrib )  + "<br>");
                                chk = "true";
                            }
                        }

                    }
                    if (id.Trim() != "")
                    {
                        if (type == "checkbox" || type == "radio")
                            d.Add(id, chk);
                        else
                            d.Add(id, value);
                    }

                }
                if (match.Groups[1].Value.StartsWith("<textarea"))
                {
                    //               Response.Write(Server.HtmlEncode( match.Groups[1].Value + "") + "<br>");
                    String tagvalue = match.Groups[1].Value;
                    String[] attrib = tagvalue.Split(' ');
                    string id = "", value = "";
                    int startpos = match.Groups[1].Value.IndexOf(">");
                    int endpos = match.Groups[1].Value.IndexOf("</textarea>");
                    value = match.Groups[1].Value.Substring(startpos + 1, endpos - startpos - 1);

                    // String pattern2 = @"<textarea.*>\s*(.+?)\s*</textarea>";
                    //String pattern2 = @"<textarea.*>(.|[\r\n])*</textarea>";
                    //RegexOptions regexOptions2 = RegexOptions.Multiline;
                    //Regex regex2 = new Regex(pattern2, regexOptions2);
                    //StringBuilder sb2 = new StringBuilder();
                    //foreach (Match match2 in regex2.Matches(match.Groups[1].Value))
                    //{
                    //    if (match2.Success)
                    //    {
                    //        value = match2.Groups[1].Value;
                    //        /*for (int j = 1; j < match.Groups.Count - 1; j++)
                    //        {
                    //            Response.Write(Server.HtmlEncode ("Match [" + j.ToString() + "]: " + match2.Groups[j].Value + "")+"<br>");
                    //        }*/
                    //    }
                    //}


                    foreach (string att in attrib)
                    {
                        //Response.Write(att.ToLower()+"<br>");
                        if (att.ToLower().StartsWith("id"))
                        {
                            String[] attele = att.Split('=');
                            id = attele[1].ToString().Trim('"');
                            //                                    Response.Write(id + "<br>");
                        }
                    }
                    if (id.Trim() != "")
                    {
                        d.Add(id, value);
                    }
                }
            }

        }
        return d;
        //foreach (string key in d.Keys)
        //{
        //    Response.Write(key + "=");
        //    Response.Write(d[key] + "<br>");
        //}
    }

  
}