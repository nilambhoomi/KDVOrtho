using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TemplateStore : System.Web.UI.Page
{
    public string Value { get; set; }   
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
                    ShowCheckBox=true
                    //Target = "_blank",
                    //  NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()

                };
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
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Session["PatientIE_ID"] = null;
        Response.Redirect("Page1.aspx");
    }
    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        Label message = new Label();

        foreach (TreeNode node in TreeView1.CheckedNodes)
        {
            //message.Text += node.Text;
            message.Text += node.Text;
        }      
        string name=message.Text;
        if (string.IsNullOrWhiteSpace(name))
        {
            lblMessage.Text = "Please tick the file to Download.";
            lblMessage.Visible = true;
        }
        else
        {
            Session["filename"] = message.Text;
            Session["newID"] = btn.CommandArgument;
            Response.Redirect("~/NFFormdoc.aspx");
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

   
}