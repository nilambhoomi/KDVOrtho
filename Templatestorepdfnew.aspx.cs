
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Templatestorepdfnew : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            LoadPatientIE("", 1);

        }
        if (!this.IsPostBack)
        {
            DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/TemplateStore/"));
            this.PopulateTreeView(rootInfo, null);
        }
    }
    private void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
    {
        foreach (DirectoryInfo directory in dirInfo.GetDirectories())
        {
            TreeNode directoryNode = new TreeNode
            {
                Text = directory.Name,
                Value = directory.FullName
            };

            if (treeNode == null)
            {
                //If Root Node, add to TreeView.
                TreeView1.Nodes.Add(directoryNode);
            }
            else
            {
                //If Child Node, add to Parent Node.

                treeNode.ChildNodes.Add(directoryNode);

            }

            //Get all files in the Directory.
            foreach (FileInfo file in directory.GetFiles())
            {

                //Add each file as Child Node.
                TreeNode fileNode = new TreeNode
                {
                    Text = file.Name,
                    Value = file.FullName,
                    ShowCheckBox = true
                    //Target = "_blank",
                    //  NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()

                };
                //ShowCheckBox = true
                fileNode.PopulateOnDemand = true;
                // Set additional properties for the node.
                fileNode.SelectAction = TreeNodeSelectAction.Expand;


                directoryNode.ChildNodes.Add(fileNode);
            }

            PopulateTreeView(directory, directoryNode);
        }
    }
    private void LoadPatientIE(string query, int pageindex)
    {
        try
        {
            int totalcount;
            DataSet dt = new DataSet();

            dt = db.PatientIE_getAll(query, pageindex, 10, out totalcount);
            if (dt.Tables[0].Rows.Count > 0)
            {
                rpview.DataSource = dt;
                rpview.DataBind();
            }
            else
            {
                rpview.DataSource = null;
                rpview.DataBind();
            }
            PopulatePager(totalcount, pageindex);
            //lblcount.Text = totalcount.ToString();
        }
        catch (Exception ex)
        {
        }
    }
    private void PopulatePager(int recordCount, int currentPage)
    {
        List<ListItem> pages = new List<ListItem>();
        int startIndex, endIndex;
        int pagerSpan = 5;

        //Calculate the Start and End Index of pages to be displayed.
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(10));
        int pageCount = (int)Math.Ceiling(dblPageCount);

        startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
        endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        if (currentPage > pagerSpan % 2)
        {
            if (currentPage == 2)
            {
                endIndex = 5;
            }
            else
            {
                endIndex = currentPage + 2;
            }
        }
        else
        {
            endIndex = (pagerSpan - currentPage) + 1;
        }

        if (endIndex - (pagerSpan - 1) > startIndex)
        {
            startIndex = endIndex - (pagerSpan - 1);
        }

        if (endIndex > pageCount)
        {
            endIndex = pageCount;
            startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        }

        //Add the First Page Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("First", "1"));
        }

        //Add the Previous Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        }

        //Add the Next Button.
        if (currentPage < pageCount)
        {
            pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
        }

        //Add the Last Button.
        if (currentPage != pageCount)
        {
            pages.Add(new ListItem("Last", pageCount.ToString()));
        }

        if (recordCount > 0)
        {
            lbl_page_no.InnerText = currentPage.ToString();
            lbl_total_page.InnerText = pageCount.ToString();


            rptPager.DataSource = pages;
            rptPager.DataBind();
        }
        else
        {
            div_page.Style.Add("display", "none");
            rptPager.DataSource = null;
            rptPager.DataBind();
        }
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);

        string name = "";
        if (!string.IsNullOrEmpty(txt_name.Text))
        {
            name = txt_name.Text.Trim();
            LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", pageIndex);
        }
        else
            this.LoadPatientIE("", pageIndex);
    }

    protected void lnk_openIE_New_Click(object sender, EventArgs e)
    {
        string name = "";
        if (TreeView1.CheckedNodes.Count <= 0)
        {
            // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
            //lblMessage.Visible = true;

        }
        else if (TreeView1.CheckedNodes.Count >= 2)
        {
            // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
            //lblMessage.Visible = true;
        }
        else if (TreeView1.CheckedNodes.Count == 1)
        {

            foreach (TreeNode node in TreeView1.CheckedNodes)
            {
                name = node.Text;
            }
            LinkButton btn = sender as LinkButton;


            string query = " select * from tblPatientIESign where PatientIE_ID=" + btn.CommandArgument;

            DataSet ds = db.selectData(query);

            if (ds.Tables[0].Rows.Count == 0)
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "openModelPopup(" + btn.CommandArgument + ");", true);
            else
            {
                if (name == "NF packet.pdf" || name == "AOB-NY.pdf" || name == "Sx Letter.pdf" || name == "Work Letter.pdf" || name == "Pending Sx letter.pdf")
                    bindNF(btn.CommandArgument, ds.Tables[0].Rows[0]["blob_string"].ToString(), name);
                else
                    openIE(btn.CommandArgument, ds.Tables[0].Rows[0]["blob_string"].ToString());
            }

        }

    }

    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;

        //Response.Redirect("~/Templatestorepdf.aspx");  
    }

    public void openIE(string _patientIEID, string signpath)
    {
        Label parentname = new Label();
        Label childfilename = new Label();
        Label names = new Label();
        Label fpname = new Label();
        Label lpname = new Label();

        if (TreeView1.CheckedNodes.Count <= 0)
        {
            // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
            //lblMessage.Visible = true;

        }
        else if (TreeView1.CheckedNodes.Count >= 2)
        {
            // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
            //lblMessage.Visible = true;
        }
        else if (TreeView1.CheckedNodes.Count == 1)
        {

            if (TreeView1.CheckedNodes.Count > 0 && TreeView1.CheckedNodes.Count < 2)
            {

                foreach (TreeNode node in TreeView1.CheckedNodes)
                {
                    parentname.Text = node.Parent.Text;
                    childfilename.Text = node.Text;
                }
                string name = childfilename.Text;
                if (string.IsNullOrWhiteSpace(childfilename.Text))
                { }
                else
                {
                    Session["filename"] = childfilename.Text;
                }
                bindEditData(_patientIEID);

                string query = "select Proc_Name,ProcedureCode,CPTCodes from tblprocedureCodes where ProcedureCode='" + txtProcedureCode.Text.Trim() + "'";

                DataSet ds = db.selectData(query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Proc_Name"].ToString()))
                    {
                        Session["Proc_Name"] = " ";
                    }
                    else
                    {
                        Session["Proc_Name"] = ds.Tables[0].Rows[0]["Proc_Name"].ToString();
                    }
                    if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["ProcedureCode"].ToString()))
                    {
                        Session["ProcedureCode"] = " ";
                    }
                    else
                    {
                        Session["ProcedureCode"] = ds.Tables[0].Rows[0]["ProcedureCode"].ToString();
                    }
                    if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["CPTCodes"].ToString()))
                    {
                        Session["CPTCodes"] = " ";
                    }
                    else
                    {
                        Session["CPTCodes"] = ds.Tables[0].Rows[0]["CPTCodes"].ToString();
                    }
                }

                var pdfPath = Path.Combine(Server.MapPath("~/TemplateStore\\" + parentname.Text + "\\" + childfilename.Text));
                names.Text = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
                fpname.Text = Convert.ToString(Session["fname"]);
                lpname.Text = Convert.ToString(Session["lname"]);
                var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);
                formFieldMap["txt_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                formFieldMap["txt_name"] = names.Text;
                // formFieldMap["txt_name"] = @"data:image/jpg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAEgAfYDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD3+iiigDHvvCfhvU7yS8v/AA/pV3dSY3zT2UcjtgADLEZOAAPwqv8A8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQBz/APwgng//AKFTQ/8AwXQ//E0f8IJ4P/6FTQ//AAXQ/wDxNdBRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQBz/APwkOqf9CZrn/f6y/wDkij/hIdU/6EzXP+/1l/8AJFdBRQBz/wDwkOqf9CZrn/f6y/8Akij/AISHVP8AoTNc/wC/1l/8kV0FFAHP/wDCQ6p/0Jmuf9/rL/5Io/4SHVP+hM1z/v8AWX/yRXQUUAc//wAJDqn/AEJmuf8Af6y/+SKP+Eh1T/oTNc/7/WX/AMkV0FFAHP8A/CQ6p/0Jmuf9/rL/AOSKP+Eh1T/oTNc/7/WX/wAkV0FFAHP/APCQ6p/0Jmuf9/rL/wCSKP8AhIdU/wChM1z/AL/WX/yRXQUUAc//AMJDqn/Qma5/3+sv/kij/hIdU/6EzXP+/wBZf/JFdBRQBz//AAkOqf8AQma5/wB/rL/5Io/4SHVP+hM1z/v9Zf8AyRXQUUAc/wD8JDqn/Qma5/3+sv8A5Io/4SHVP+hM1z/v9Zf/ACRXQUUAc/8A8JDqn/Qma5/3+sv/AJIo/wCEh1T/AKEzXP8Av9Zf/JFdBRQBz/8AwkOqf9CZrn/f6y/+SKP+Eh1T/oTNc/7/AFl/8kV0FFAHP/8ACQ6p/wBCZrn/AH+sv/kij/hIdU/6EzXP+/1l/wDJFdBRQBz/APwkOqf9CZrn/f6y/wDkij/hIdU/6EzXP+/1l/8AJFdBRQBz/wDwkOqf9CZrn/f6y/8Akij/AISHVP8AoTNc/wC/1l/8kV0FFAHP/wDCQ6p/0Jmuf9/rL/5Io/4SHVP+hM1z/v8AWX/yRXQUUAc//wAJDqn/AEJmuf8Af6y/+SKP+Eh1T/oTNc/7/WX/AMkV0FFAHP8A/CQ6p/0Jmuf9/rL/AOSKP+Eh1T/oTNc/7/WX/wAkV0FFAHP/APCQ6p/0Jmuf9/rL/wCSKP8AhIdU/wChM1z/AL/WX/yRXQUUAc//AMJDqn/Qma5/3+sv/kitixuJbuzjnmsp7KRs5gnKF0wSOSjMvPXgnr68VYooAKKKKACiiigCnqWpwaVbrPcR3bozhALW0luGzgnlY1YgcdcY6eorL/4TLS/+fXXP/BFe/wDxmugooA5//hMtL/59dc/8EV7/APGaP+Ey0v8A59dc/wDBFe//ABmugooA5/8A4TLS/wDn11z/AMEV7/8AGaP+Ey0v/n11z/wRXv8A8ZroKKAOf/4TLS/+fXXP/BFe/wDxmj/hMtL/AOfXXP8AwRXv/wAZroKKAOf/AOEy0v8A59dc/wDBFe//ABmj/hMtL/59dc/8EV7/APGa6CigDn/+Ey0v/n11z/wRXv8A8Zo/4TLS/wDn11z/AMEV7/8AGa6CigDn/wDhMtL/AOfXXP8AwRXv/wAZo/4TLS/+fXXP/BFe/wDxmugooA5//hMtL/59dc/8EV7/APGaP+Ey0v8A59dc/wDBFe//ABmugooA5/8A4TLS/wDn11z/AMEV7/8AGaP+Ey0v/n11z/wRXv8A8ZroKKAOf/4TLS/+fXXP/BFe/wDxmj/hMtL/AOfXXP8AwRXv/wAZroKKAKem6nBqtu09vHdoiuUIurSW3bOAeFkVSRz1xjr6GirlFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFU9Sj1KW3VdLu7S2n3gs91bNOpXB4CrIhBzjnPY8c8AFyiuf+x+MP+g7of/gmm/8Akqj7H4w/6Duh/wDgmm/+SqAOgorn/sfjD/oO6H/4Jpv/AJKo+x+MP+g7of8A4Jpv/kqgDoKK5/7H4w/6Duh/+Cab/wCSqPsfjD/oO6H/AOCab/5KoA6Ciuf+x+MP+g7of/gmm/8Akqj7H4w/6Duh/wDgmm/+SqAOgorn/sfjD/oO6H/4Jpv/AJKo+x+MP+g7of8A4Jpv/kqgDoKK5/7H4w/6Duh/+Cab/wCSqPsfjD/oO6H/AOCab/5KoA6Ciuf+x+MP+g7of/gmm/8Akqj7H4w/6Duh/wDgmm/+SqAOgorn/sfjD/oO6H/4Jpv/AJKo+x+MP+g7of8A4Jpv/kqgDoKK5/7H4w/6Duh/+Cab/wCSqPsfjD/oO6H/AOCab/5KoA6Ciuf+x+MP+g7of/gmm/8Akqj7H4w/6Duh/wDgmm/+SqAOgorn/sfjD/oO6H/4Jpv/AJKo+x+MP+g7of8A4Jpv/kqgDoKK5/7H4w/6Duh/+Cab/wCSqPsfjD/oO6H/AOCab/5KoA6Ciuf+x+MP+g7of/gmm/8Akqj7H4w/6Duh/wDgmm/+SqAOgorn/sfjD/oO6H/4Jpv/AJKo+x+MP+g7of8A4Jpv/kqgDoKK5/7H4w/6Duh/+Cab/wCSqPsfjD/oO6H/AOCab/5KoA6Ciuf+x+MP+g7of/gmm/8Akqj7H4w/6Duh/wDgmm/+SqAOgorn/sfjD/oO6H/4Jpv/AJKo+x+MP+g7of8A4Jpv/kqgDoKK5/7H4w/6Duh/+Cab/wCSqPsfjD/oO6H/AOCab/5KoA6Ciuf+x+MP+g7of/gmm/8AkqpILXxUtxE1xrOjSQBwZEj0mVGZc8gMbkgHHfBx6GgDcooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKz9T0a11fyvtMt9H5Wdv2S/nts5xnPlOu7p3zjnHU1oUUAc/8A8Ibpf/P1rn/g9vf/AI9R/wAIbpf/AD9a5/4Pb3/49XQUUAc//wAIbpf/AD9a5/4Pb3/49R/whul/8/Wuf+D29/8Aj1dBRQBz/wDwhul/8/Wuf+D29/8Aj1H/AAhul/8AP1rn/g9vf/j1dBRQBz//AAhul/8AP1rn/g9vf/j1H/CG6X/z9a5/4Pb3/wCPV0FFAHP/APCG6X/z9a5/4Pb3/wCPUf8ACG6X/wA/Wuf+D29/+PV0FFAHP/8ACG6X/wA/Wuf+D29/+PUf8Ibpf/P1rn/g9vf/AI9XQUUAc/8A8Ibpf/P1rn/g9vf/AI9R/wAIbpf/AD9a5/4Pb3/49XQUUAc//wAIbpf/AD9a5/4Pb3/49R/whul/8/Wuf+D29/8Aj1dBRQBz/wDwhul/8/Wuf+D29/8Aj1H/AAhul/8AP1rn/g9vf/j1dBRQBz//AAhul/8AP1rn/g9vf/j1H/CG6X/z9a5/4Pb3/wCPV0FFAHP/APCG6X/z9a5/4Pb3/wCPUf8ACG6X/wA/Wuf+D29/+PV0FFAHP/8ACG6X/wA/Wuf+D29/+PUf8Ibpf/P1rn/g9vf/AI9XQUUAc/8A8Ibpf/P1rn/g9vf/AI9R/wAIbpf/AD9a5/4Pb3/49XQUUAc//wAIbpf/AD9a5/4Pb3/49R/whul/8/Wuf+D29/8Aj1dBRQBz/wDwhul/8/Wuf+D29/8Aj1H/AAhul/8AP1rn/g9vf/j1dBRQBz//AAhul/8AP1rn/g9vf/j1XNN0Cz0q4ae3m1J3ZChF1qVxcLjIPCyOwB464z19TWpRQAUUUUAFFFFAEc8EN1by29xFHNBKhSSORQyupGCCDwQRxisP/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAOf/wCEE8H/APQqaH/4Lof/AImj/hBPB/8A0Kmh/wDguh/+JroKKAMvTfDWg6NcNcaXomm2M7IUaS1tUiYrkHBKgHGQDj2FFalFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUVHPG01vLEk0kDuhVZYwpZCR94bgRkdeQR6g1JQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB//9k=";
                formFieldMap["txt_eMail"] = Convert.ToString(Session["eMail"]);
                formFieldMap["txt_city"] = Convert.ToString(Session["city"]);
                formFieldMap["txt_Inscity"] = Convert.ToString(Session["Inscity"]);
                formFieldMap["txt_state"] = Convert.ToString(Session["state"]);
                formFieldMap["txt_Insstate"] = Convert.ToString(Session["Insstate"]);
                formFieldMap["txt_zip"] = Convert.ToString(Session["zip"]);
                formFieldMap["txt_Inszip"] = Convert.ToString(Session["Inszip"]);

                formFieldMap["txt_ProcedureCode"] = Convert.ToString(Session["ProcedureCode"]);
                formFieldMap["txt_CPTCodes"] = Convert.ToString(Session["CPTCodes"]);
                formFieldMap["txt_Procedure_date"] = txtproc_code_date.Text;

                if (Session["Phone"] != null)
                    if (Session["Phone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_Phone2"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_Phone2"] = Session["Phone"].ToString();
                    }
                if (Session["work_phone"] != null)
                    if (Session["work_phone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_work_phone"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_work_phone"] = Session["work_phone"].ToString();
                    }
                if (Session["InsPhone"] != null)
                    if (Session["InsPhone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_InsPhone"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_InsPhone"] = Session["InsPhone"].ToString();
                    }
                if (Session["ssn"] != null)
                    if (Session["ssn"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_ssn"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_ssn"] = Session["ssn"].ToString();
                        //formFieldMap["txt_ssn"] = "";
                    }
                formFieldMap["txt_InsCo"] = Convert.ToString(Session["InsCo"]);
                formFieldMap["txt_ClaimNumber"] = Convert.ToString(Session["ClaimNumber"]);
                formFieldMap["txt_admitting_surgeon"] = "Ketan D. Vora, D.O.";
                formFieldMap["txt_admitting_surgeon_ppc"] = "Gurbir Johal, MD";
                formFieldMap["txt_contact_persion_at_clinic"] = "Eddie Mendez";
                formFieldMap["txt_phnodr"] = "(877)-774-6337";
                formFieldMap["txt_Referring_Physician_Phone"] = "877-774-6337";
                formFieldMap["txt_H_C_Provider_Name"] = "Ketan D. Vora, D.O.";
                formFieldMap["txt_License_State_Of"] = " New York";
                formFieldMap["txt_License_Number"] = "243182";
                formFieldMap["chk_2"] = "true";
                //formFieldMap["chk_2"] = "Checked";
                formFieldMap["txt_Referring_Clinic"] = Convert.ToString(Session["LocationPdf"]);
                formFieldMap["txt_Referring_Physician"] = "Ketan D. Vora, D.O.";
                formFieldMap["txt_Referring_Physician_ppc"] = "Gurbir Johal, MD";
                formFieldMap["txt_phnodrppc"] = "(732)-887-2004";
                formFieldMap["txt_c_fname"] = Convert.ToString(Session["fname"]);
                formFieldMap["txt_c_lname"] = Convert.ToString(Session["lname"]);
                formFieldMap["txt_fname"] = Convert.ToString(Session["fname"]);
                formFieldMap["txt_mname"] = Convert.ToString(Session["mname"]);
                formFieldMap["txt_lname"] = Convert.ToString(Session["lname"]);
                formFieldMap["txt_address"] = Convert.ToString(Session["Address"]);

                formFieldMap["txt_addressCityStateZip"] = (!string.IsNullOrEmpty(Convert.ToString(Session["Address"])) ? Convert.ToString(Session["Address"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["city"])) ? " ," + Convert.ToString(Session["city"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["state"])) ? " ," + Convert.ToString(Session["state"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["zip"])) ? " ," + Convert.ToString(Session["zip"]) : string.Empty);
                formFieldMap["txt_Insaddress"] = Convert.ToString(Session["InsAddress"]);
                if (string.IsNullOrWhiteSpace(Session["AGE"].ToString()))
                {
                    formFieldMap["txt_age"] = "";
                }
                else
                {
                    formFieldMap["txt_age"] = Convert.ToString(Session["AGE"]);
                }

                if (Session["mob"] != null)
                    if (Session["mob"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_mob"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_mob"] = Session["mob"].ToString();
                    }
                if (Session["dob"] != null)
                    if (string.IsNullOrWhiteSpace(Session["dob"].ToString()))
                    {
                    }
                    else
                    {
                        formFieldMap["txt_dob"] = Session["dob"].ToString();
                        DateTime dob;
                        if (Session["dob"] != null && DateTime.TryParseExact(Session["dob"].ToString(), "MM-dd-yyyy", null, DateTimeStyles.None, out dob))
                        {

                            formFieldMap["txtdaydob"] = Convert.ToString(dob.Day);
                            formFieldMap["txtmonthdob"] = Convert.ToString(dob.Month);
                            formFieldMap["txtyeardob"] = Convert.ToString(dob.Year);
                        }
                    }
                if (Session["Attorney"] != null)
                    if (string.IsNullOrWhiteSpace(Session["Attorney"].ToString()))
                    {
                        formFieldMap["txt_attorney"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorney"] = Session["Attorney"].ToString();
                    }
                if (Session["AttorneyPhno"] != null)
                    if (string.IsNullOrWhiteSpace(Session["AttorneyPhno"].ToString()))
                    {
                        formFieldMap["txt_attorneyPhno"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorneyPhno"] = Session["AttorneyPhno"].ToString();
                    }
                if (Session["AttorneyAdd"] != null)
                    if (string.IsNullOrWhiteSpace(Session["AttorneyAdd"].ToString()))
                    {
                        formFieldMap["txt_attorneyAdd"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorneyAdd"] = Session["AttorneyAdd"].ToString();
                    }

                if (Session["Adjuster"] != null && Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 1)
                {
                    formFieldMap["txtAdjuster"] = Convert.ToString(Session["Adjuster"]).Split('~')[0];
                    if (Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 2)
                    {
                        formFieldMap["txtAdjusterph"] = Convert.ToString(Session["Adjuster"]).Split('~')[1];
                        if (Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 3)
                        { formFieldMap["txtAdjusterext"] = Convert.ToString(Session["Adjuster"]).Split('~')[2]; }
                    }
                }

                formFieldMap["txt_policy_no"] = Convert.ToString(Session["policy_no"]);

                formFieldMap["txt_c_dob"] = Convert.ToString(Session["dob"]);
                formFieldMap["txt_c_name"] = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
                formFieldMap["txt_claim_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                formFieldMap["txt_claim_dateDay"] = Convert.ToString(System.DateTime.Now.ToString("dd"));
                formFieldMap["txt_claim_dateMonth"] = Convert.ToString(System.DateTime.Now.ToString("MM"));
                formFieldMap["txt_claim_dateYear"] = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
                if (Session["sex"] != null)
                    if (Session["sex"].ToString() == "Mr.")
                    {

                        formFieldMap["txt_sex"] = "Male";
                        formFieldMap["txt_male"] = "X";
                    }
                    else if (Session["sex"].ToString() == "Ms.")
                    {
                        formFieldMap["txt_sex"] = "Female";
                        formFieldMap["txt_female"] = "X";
                    }
                if (Session["ssn"] != null)
                {
                    string ssn = Session["ssn"].ToString();
                    if (string.IsNullOrWhiteSpace(ssn))
                    {
                    }
                    else
                    {
                        if (ssn.Split().Last().All(char.IsDigit))
                        {
                            string ssn1 = ssn.Replace("-", "");
                            string separated = new string(
                                                             ssn1.Select((x, i) => i > 0 && i % 1 == 0 ? new[] { ',', x } : new[] { x })
                                                                .SelectMany(x => x)
                                                                .ToArray()
                                                                 );
                            if (string.IsNullOrWhiteSpace(separated))
                            {
                            }
                            else
                            {
                                int[] a = separated.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                if (a.Count() >= 1)
                                {
                                    if (a[0] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["1"] = Convert.ToString(a[0]);
                                    }
                                }
                                if (a.Count() >= 2)
                                {
                                    if (a[1] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["2"] = Convert.ToString(a[1]);
                                    }
                                }
                                if (a.Count() >= 3)
                                {
                                    if (a[2] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["3"] = Convert.ToString(a[2]);
                                    }
                                }
                                if (a.Count() >= 4)
                                {
                                    if (a[3] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["4"] = Convert.ToString(a[3]);
                                    }
                                }
                                if (a.Count() >= 5)
                                {
                                    if (a[4] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["5"] = Convert.ToString(a[4]);
                                    }
                                }
                                if (a.Count() >= 6)
                                {
                                    if (a[5] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["6"] = Convert.ToString(a[5]);
                                    }
                                }
                                if (a.Count() >= 7)
                                {
                                    if (a[6] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["7"] = Convert.ToString(a[6]);
                                    }
                                }
                                if (a.Count() >= 8)
                                {
                                    if (a[7] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["8"] = Convert.ToString(a[7]);
                                    }
                                }
                                if (a.Count() >= 9)
                                {
                                    if (a[8] == null)
                                    {
                                    }
                                    else
                                    {
                                        formFieldMap["9"] = Convert.ToString(a[8]);
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
                if (Session["doa"] != null)
                    if (string.IsNullOrWhiteSpace(Session["doa"].ToString()))
                    {
                    }
                    else
                    {
                        formFieldMap["txt_doa"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy");
                        formFieldMap["txt_doaday"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("dd");
                        formFieldMap["txt_doaMonth"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM");
                        formFieldMap["txt_doaYear"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("yyyy");
                    }
                if (Session["doe"] != null)
                    if (string.IsNullOrWhiteSpace(Session["doe"].ToString()))
                    {
                    }
                    else
                    {

                        formFieldMap["txt_doe"] = Convert.ToDateTime(Session["doe"].ToString()).ToString("MM/dd/yyyy");

                        DateTime doe = Convert.ToDateTime(Session["doe"].ToString());

                        formFieldMap["txtdaydoe"] = Convert.ToString(doe.Day);
                        formFieldMap["txtmonthdoe"] = Convert.ToString(doe.Month);
                        formFieldMap["txtyeardoe"] = Convert.ToString(doe.Year);

                    }
                if (Session["Compensation"] != null)
                {
                    formFieldMap["txt_casetype"] = Convert.ToString(Session["Compensation"]);
                    if (Session["Compensation"].Equals("WC"))
                    { formFieldMap["txt_wc"] = "yes"; }
                    else
                    { formFieldMap["txt_wc"] = "No"; }
                    if (Session["Compensation"].Equals("NF"))
                    { formFieldMap["txt_NF"] = "yes"; }
                    else
                    { formFieldMap["txt_NF"] = "No"; }
                    if (Session["Compensation"].Equals("PI"))
                    { formFieldMap["txt_PI"] = "yes"; }
                    else
                    { formFieldMap["txt_PI"] = "No"; }

                    if (Session["Compensation"].Equals("Lien"))
                    { formFieldMap["txt_AL"] = "yes"; }
                    else
                    { formFieldMap["txt_AL"] = "No"; }

                    if (Session["Compensation"].Equals("MM"))
                    { formFieldMap["txt_MM"] = "yes"; }
                    else
                    { formFieldMap["txt_MM"] = "No"; }

                    if (Session["Compensation"].Equals("Taxi"))
                    { formFieldMap["txt_Taxi"] = "yes"; }
                    else
                    { formFieldMap["txt_Taxi"] = "No"; }

                    formFieldMap["txt_PC"] = "No";
                    formFieldMap["txt_SP"] = "No";

                }

                if (Convert.ToString(Session["filename"]).Equals("Accelerated.pdf"))
                {
                    if (Session["Compensation"] != null)
                        if (Session["Compensation"].Equals("WC"))
                        {
                            if (Session["doe"] != null)
                                formFieldMap["txt_doeWC"] = Convert.ToDateTime(Session["doe"].ToString()).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            if (Session["doa"] != null)
                                formFieldMap["txt_doaMVA"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy");
                        }
                }

                var pdfContents = PDFHelper.GeneratePDF(pdfPath, formFieldMap);


                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER);


                string filename = Convert.ToString(Session["filename"]);
                string filenamefinal = filename.Split('.').First();
                // lblMessage.Visible = false;
                //lblMessage.ImageUrl = "";

                if (filename == "Surgicore Booking Sheet.pdf")
                {

                    PDFHelper.ReturnPDF(pdfContents, lpname.Text + " " + fpname.Text + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf");

                }
                else if (filename == "PatientInformation.pdf")
                {
                    PDFHelper.ReturnPDF(pdfContents, names.Text + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf");
                }
                else
                {
                    PDFHelper.ReturnPDF(pdfContents, filenamefinal + "-" + names.Text + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf");
                }

            }
        }

    }

    [WebMethod]
    public static string[] getFirstName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID, LastName, FirstName from tblPatientMaster where FirstName like '%" + prefix + "%' OR LastName Like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["LastName"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
            name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["FirstName"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }
        return patient.ToArray();
    }

    protected void txt_name_TextChanged(object sender, EventArgs e)
    {
        string name = "";
        if (!string.IsNullOrEmpty(txt_name.Text))
        {
            name = txt_name.Text.Trim();
            LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", 1);
        }
    }

    public void bindEditData(string PatientIEid)
    {
        try
        {

            string query = "select * from View_PatientIE where PatientIE_ID=" + PatientIEid;

            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["FirstName"].ToString()))
                {
                    Session["fname"] = " ";
                }
                else
                {
                    Session["fname"] = ds.Tables[0].Rows[0]["FirstName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["LastName"].ToString()))
                {
                    Session["lname"] = " ";
                }
                else
                {
                    Session["lname"] = ds.Tables[0].Rows[0]["LastName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["MiddleName"].ToString()))
                {
                    Session["mname"] = " ";
                }
                else
                {
                    Session["mname"] = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["eMail"].ToString()))
                {
                    Session["eMail"] = " ";
                }
                else
                {
                    Session["eMail"] = ds.Tables[0].Rows[0]["eMail"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["DOA"].ToString()))
                {
                    Session["doa"] = " ";
                }
                else
                {
                    Session["doa"] = ds.Tables[0].Rows[0]["DOA"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["DOE"].ToString()))
                {
                    Session["doe"] = " ";
                }
                else
                {
                    Session["doe"] = ds.Tables[0].Rows[0]["DOE"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["SSN"].ToString()))
                {
                    Session["ssn"] = " ";
                }
                else
                {
                    Session["ssn"] = ds.Tables[0].Rows[0]["SSN"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Address1"].ToString()))
                {
                    Session["Address"] = " ";
                }
                else
                {
                    Session["Address"] = ds.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsAddress1"].ToString()))
                {
                    Session["InsAddress"] = " ";
                }
                else
                {
                    Session["InsAddress"] = ds.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Phone2"].ToString()))
                {
                    Session["mob"] = " ";
                }
                else
                {
                    Session["mob"] = ds.Tables[0].Rows[0]["Phone2"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsPhone"].ToString()))
                {
                    Session["InsPhone"] = " ";
                }
                else
                {
                    Session["InsPhone"] = ds.Tables[0].Rows[0]["InsPhone"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["City"].ToString()))
                {
                    Session["city"] = " ";
                }
                else
                {
                    Session["city"] = ds.Tables[0].Rows[0]["City"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsCity"].ToString()))
                {
                    Session["Inscity"] = " ";
                }
                else
                {
                    Session["Inscity"] = ds.Tables[0].Rows[0]["InsCity"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["State"].ToString()))
                {
                    Session["state"] = " ";
                }
                else
                {
                    Session["state"] = ds.Tables[0].Rows[0]["State"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsState"].ToString()))
                {
                    Session["Insstate"] = " ";
                }
                else
                {
                    Session["Insstate"] = ds.Tables[0].Rows[0]["InsState"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Zip"].ToString()))
                {
                    Session["zip"] = " ";
                }
                else
                {
                    Session["zip"] = ds.Tables[0].Rows[0]["Zip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsZip"].ToString()))
                {
                    Session["Inszip"] = " ";
                }
                else
                {
                    Session["Inszip"] = ds.Tables[0].Rows[0]["InsZip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Phone"].ToString()))
                {
                    Session["Phone"] = " ";
                }
                else
                {
                    Session["Phone"] = ds.Tables[0].Rows[0]["Phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["work_phone"].ToString()))
                {
                    Session["work_phone"] = " ";
                }
                else
                {
                    Session["work_phone"] = ds.Tables[0].Rows[0]["work_phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Sex"].ToString()))
                {
                    Session["sex"] = " ";
                }
                else
                {
                    Session["sex"] = ds.Tables[0].Rows[0]["Sex"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsCo"].ToString()))
                {
                    Session["InsCo"] = " ";
                }
                else
                {
                    Session["InsCo"] = ds.Tables[0].Rows[0]["InsCo"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["policy_no"].ToString()))
                {
                    Session["policy_no"] = " ";
                }
                else
                {
                    Session["policy_no"] = ds.Tables[0].Rows[0]["policy_no"].ToString();
                }


                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["ClaimNumber"].ToString()))
                {
                    Session["ClaimNumber"] = " ";
                }
                else
                {
                    Session["ClaimNumber"] = ds.Tables[0].Rows[0]["ClaimNumber"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Location"].ToString()))
                {
                    Session["LocationPdf"] = " ";
                }
                else
                {
                    Session["LocationPdf"] = ds.Tables[0].Rows[0]["Location"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Attorney"].ToString()))
                {
                    Session["Attorney"] = " ";
                }
                else
                {
                    Session["Attorney"] = ds.Tables[0].Rows[0]["Attorney"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AttorneyAdd"].ToString()))
                {
                    Session["AttorneyAdd"] = " ";
                }
                else
                {
                    Session["AttorneyAdd"] = ds.Tables[0].Rows[0]["AttorneyAdd"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AttorneyPhno"].ToString()))
                {
                    Session["AttorneyPhno"] = " ";
                }
                else
                {
                    Session["AttorneyPhno"] = ds.Tables[0].Rows[0]["AttorneyPhno"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Adjuster"].ToString()))
                {
                    Session["Adjuster"] = " ";
                }
                else
                {
                    Session["Adjuster"] = ds.Tables[0].Rows[0]["Adjuster"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Compensation"].ToString()))
                {
                    Session["Compensation"] = " ";
                }
                else
                {
                    Session["Compensation"] = ds.Tables[0].Rows[0]["Compensation"].ToString();
                }
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AGE"].ToString()))
                //{
                //    Session["AGE"] = " ";
                //}
                //else
                //{
                //    //Session["AGE"] = ds.Tables[0].Rows[0]["AGE"].ToString();
                //}

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["MA_Providers"].ToString()))
                {
                    Session["MA_Providers"] = " ";
                }
                else
                {
                    Session["MA_Providers"] = ds.Tables[0].Rows[0]["MA_Providers"].ToString();
                }

                if (ds.Tables[0].Rows[0]["DOB"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString());
                    Session["dob"] = dob.ToString("MM/dd/yyyy");
                    Session["AGE"] = CalculateAge(dob);
                }
                else
                {
                    Session["dob"] = " ";
                    Session["AGE"] = " ";
                }
            }
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
    }

    [System.Web.Services.WebMethod]
    public static void Generatepdf()
    {
        string patientID = Convert.ToString(HttpContext.Current.Session["PatientIE_ID"]);
        Templatestorepdfnew example = new Templatestorepdfnew();
        example.bindEditData(patientID);

        string names = Convert.ToString(HttpContext.Current.Session["fname"]) + " " + Convert.ToString(HttpContext.Current.Session["lname"]);
        var pdfPath = Path.Combine(HttpContext.Current.Server.MapPath("~/SUPBILL.pdf"));

        var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);

        formFieldMap["txt_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));

        formFieldMap["txt_name"] = names;



        if (HttpContext.Current.Session["dob"] != null)
        { formFieldMap["txt_dob"] = Convert.ToString(HttpContext.Current.Session["dob"]); }

        if (HttpContext.Current.Session["doe"] != null)
        { formFieldMap["txt_doe"] = Convert.ToDateTime(HttpContext.Current.Session["doe"].ToString()).ToString("MM/dd/yyyy"); }

        if (HttpContext.Current.Session["MA_Providers"] != null)
        { formFieldMap["txt_MA_Provider"] = Convert.ToString(HttpContext.Current.Session["MA_Providers"]); }



        var pdfContents = PDFHelper.GeneratePDF(pdfPath, formFieldMap);
        string filename = names + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        HttpContext.Current.Response.AppendHeader("Content-Transfer-Encoding", "binary");

        HttpContext.Current.Response.OutputStream.Write(pdfContents, 0, pdfContents.Length);
        HttpContext.Current.Response.BufferOutput = true;
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf;
        HttpContext.Current.Response.BinaryWrite(pdfContents);
        if (HttpContext.Current.Response.IsClientConnected)
        {
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }


        //var response = HttpContext.Current.Response;

        //response.AppendHeader("Content-Disposition", "inline; filename=xxx.pdf");
        //response.AppendHeader("Content-Length", pdfContents.Length.ToString());
        //response.AppendHeader("Content-Transfer-Encoding", "binary");

        //response.OutputStream.Write(pdfContents, 0, pdfContents.Length);
        //response.BufferOutput = true;
        //response.Buffer = true;

        //response.ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf;

        //response.BinaryWrite(pdfContents);

        //response.Flush();
        //response.End();
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        int age = 0;
        age = DateTime.Now.Year - dateOfBirth.Year;
        if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            age = age - 1;

        return age;
    }

    [WebMethod]
    public static string[] getProcCode(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Prc_id,Proc_Name,ProcedureCode,CPTCodes from tblprocedureCodes where ProcedureCode like '" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["ProcedureCode"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["ProcedureCode"].ToString()));
            }
        }
        return patient.ToArray();
    }

    protected void btnSaveSign_Click(object sender, EventArgs e)
    {
        byte[] blob = null;
        if (string.IsNullOrEmpty(hidBlobServer.Value) == false)
        {
            try
            {
                string blobstring = hidBlobServer.Value.Split(',')[1];
                blob = Convert.FromBase64String(blobstring);

                string path = HttpContext.Current.Server.MapPath("~/Sign/");
                string fname = patientIEIDServer.Value.ToString() + "_" + System.DateTime.Now.Millisecond.ToString() + ".jpg";

                string fullpath = path + "//" + fname;

                File.WriteAllBytes(fullpath, blob);

                string query = "insert into tblPatientIESign values(" + patientIEIDServer.Value + ",'" + fullpath + "',0,'" + hidBlobServer.Value + "')";

                DBHelperClass db = new DBHelperClass();
                db.executeQuery(query);


                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "closeModelPopup();", true);

                string name = "";
                if (TreeView1.CheckedNodes.Count <= 0)
                {
                    // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
                    //lblMessage.Visible = true;

                }
                else if (TreeView1.CheckedNodes.Count >= 2)
                {
                    // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
                    //lblMessage.Visible = true;
                }
                else if (TreeView1.CheckedNodes.Count == 1)
                {

                    foreach (TreeNode node in TreeView1.CheckedNodes)
                    {
                        name = node.Text;
                    }
                }

                if (name == "NF packet.pdf" || name == "AOB-NY.pdf" || name == "Sx Letter.pdf" || name == "Work Letter.pdf" || name== "Pending Sx letter.pdf")
                    bindNF(patientIEIDServer.Value, hidBlobServer.Value, name);
                else
                    openIE(patientIEIDServer.Value, hidBlobServer.Value);


            }
            catch (Exception ex)
            {
            }

        }
    }

    private void bindNF(string _patientIEID, string sign, string filename)
    {


        var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

        string path = Server.MapPath("~/MyFiles/" + filename);


        bindEditData(_patientIEID);

        string names = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
        string fpname = Convert.ToString(Session["fname"]);
        string lpname = Convert.ToString(Session["lname"]);




        String str = File.ReadAllText(Server.MapPath("~/Template/Forms/" + filename.Split('.')[0] + ".html"));

        str = str.Replace("#txt_date", Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")));
        str = str.Replace("#txt_name", names);
        str = str.Replace("#txt_dob", Session["dob"].ToString());
        str = str.Replace("#sign", sign);

        str = str.Replace("txt_InsCo", Convert.ToString(Session["InsCo"]));
        str = str.Replace("#txt_ClaimNumber", string.IsNullOrEmpty(Session["ClaimNumber"].ToString()) ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : Session["ClaimNumber"].ToString());


        if (Session["sex"] != null)
        {
            if (Session["sex"].ToString().ToLower() == "ms.")
                str = str.Replace("#fsign", sign);
            else
                str = str.Replace("#fsign", "");
        }
        else
        {
            str = str.Replace("#fsign", "");
        }

        str = str.Replace("#txt_InsCo", Convert.ToString(Session["InsCo"]));

        str = str.Replace("#txt_addressCityStateZip", (!string.IsNullOrEmpty(Convert.ToString(Session["Address"])) ? Convert.ToString(Session["Address"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["city"])) ? " ," + Convert.ToString(Session["city"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["state"])) ? " ," + Convert.ToString(Session["state"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["zip"])) ? " ," + Convert.ToString(Session["zip"]) : string.Empty));


        if (Session["doa"] != null)
        {
            if (string.IsNullOrWhiteSpace(Session["doa"].ToString()))
            {
            }
            else
            {
                str = str.Replace("#txt_doa", Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy"));
            }
        }

        if (Session["ssn"] != null)
            if (Session["ssn"].ToString().Split().Last().All(char.IsDigit))
            {
                str = str.Replace("#txt_ssn", "");
            }
            else
            {
                str = str.Replace("#txt_ssn", Session["ssn"].ToString());
                //formFieldMap["txt_ssn"] = "";
            }

        if (string.IsNullOrWhiteSpace(Session["Attorney"].ToString()))
        {
            str = str.Replace("#txt_attorney", "");
        }
        else
        {
            str = str.Replace("#txt_attorney", Session["Attorney"].ToString());
        }

        htmlToPdf.GeneratePdf(str, null, path);


        WebClient req = new WebClient();
        HttpResponse response = HttpContext.Current.Response;
        string filePath = path;
        response.Clear();
        response.ClearContent();
        response.ClearHeaders();
        response.Buffer = true;
        response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
        byte[] data = req.DownloadData(filePath);
        response.BinaryWrite(data);
        response.End();

    }
}