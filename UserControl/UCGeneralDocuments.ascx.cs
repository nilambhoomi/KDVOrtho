using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_UCGeneralDocuments : System.Web.UI.UserControl
{
    private DBHelperClass db = new DBHelperClass();
    private List<TreeNode> CurrentNodeMatches = new List<TreeNode>();

    private int LastNodeIndex = 0;

    private string LastSearchText;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Request["PID"] != null)
            //{
            //    Session["PatientId"] = Request["PID"].ToString();
            //    bindPatientDetails(Request["PID"].ToString());
            //}
            DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/GeneralDocuments/"));
            PopulateTreeView(rootInfo, null);
            PopulateDropBox(rootInfo);
            //ListDirectory(treeView, Server.MapPath("PatientDocument"));
            //ListDirectory(treeView, Server.MapPath("~/PatientDocument/"));
        }
    }


    //private void bindPatientDetails(string PID)
    //{
    //    try
    //    {
    //        string query = "select * from tblPatientMaster where Patient_ID=" + PID;

    //        DataSet data = db.selectData(query);

    //        if (data != null && data.Tables[0].Rows.Count > 0)
    //        {

    //            lbl_name.InnerText = data.Tables[0].Rows[0]["LastName"].ToString() + ", " + data.Tables[0].Rows[0]["FirstName"].ToString() + " - " + Convert.ToDateTime(data.Tables[0].Rows[0]["DOB"].ToString()).ToString("MM/dd/yyyy");
    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    private void PopulateDropBox(DirectoryInfo dirInfo)
    {
        foreach (DirectoryInfo directory in dirInfo.GetDirectories())
        {
            DropDownList1.Items.Add(directory.Name);
        }
    }
    private void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
    {
        foreach (DirectoryInfo directory in dirInfo.GetDirectories())
        {


            TreeNode directoryNode = new TreeNode
            {
                Text = directory.Name + " " + getFileCount(directory.FullName),
                Value = directory.FullName,
                ImageUrl = "~/images/noextend.jpg"
            };

            if (treeNode == null)
            {
                //If Root Node, add to TreeView.
                treeView.Nodes.Add(directoryNode);
            }
            else
            {
                //If Child Node, add to Parent Node.
                treeNode.ChildNodes.Add(directoryNode);
            }

            //string uname = Session["PatientId"].ToString() + "_";
            string uname = "";
            //Get all files in the Directory.
            foreach (FileInfo file in directory.GetFiles(uname + "*"))
            {
                //Add each file as Child Node.
                TreeNode fileNode = new TreeNode
                {
                    Text = file.Name.Substring(uname.Length),
                    Value = file.FullName,
                    Target = "_blank",
                    // NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString(),
                    NavigateUrl = "https://docs.google.com/viewer?url=https://www.paintrax.com/UKSPPC/GeneralDocuments/" + directory.Name.Split('(')[0].Trim() + "/" + file.Name + "&embedded=true",

                    ImageUrl = "~/images/doc.jpg"


                };
                directoryNode.ChildNodes.Add(fileNode);
            }

            PopulateTreeView(directory, directoryNode);
        }

        treeView.CollapseAll();
    }
    //private void ListDirectory(TreeView treeView, string path)
    //{
    //    treeView.Nodes.Clear();
    //    DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);

    //    treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));

    //}

    //private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
    //{
    //    TreeNode directoryNode = new TreeNode(directoryInfo.Name + getFileCount(directoryInfo.FullName))
    //    {
    //        SelectAction = TreeNodeSelectAction.Select
    //    };
    //    foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
    //    {
    //        directoryNode.ChildNodes.Add(CreateDirectoryNode(directory));

    //    }

    //    return directoryNode;
    //}

    protected void treeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        // lstBox.Items.Clear();
        // DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(treeView.SelectedNode.ValuePath));
        string p = Regex.Replace(treeView.SelectedNode.ValuePath, @" \[.+?\]", "");
        //string path = Server.MapPath(p);
        string path = treeView.SelectedNode.Value;
        string nodeName = treeView.SelectedNode.Text;

        getFileList(path, nodeName);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ImageButton btnDel = sender as ImageButton;
        string fileName = btnDel.CommandArgument;
        File.Delete(fileName);

        string dir = Path.GetDirectoryName(fileName);

        getFileList(dir, ViewState["nodeName"].ToString());

        clearTree();

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        ImageButton btn = sender as ImageButton;
        string path = btn.CommandArgument.Split('#')[0];
        string fileName = btn.CommandArgument.Split('#')[1];

        WebClient req = new WebClient();
        HttpResponse response = HttpContext.Current.Response;
        string filePath = path;
        response.Clear();
        response.ClearContent();
        response.ClearHeaders();
        response.Buffer = true;
        response.AddHeader("Content-Disposition", @"attachment;filename=""" + fileName + @"""");
        byte[] data = req.DownloadData(filePath);
        response.BinaryWrite(data);
        response.End();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (fup.HasFiles)
            {
                string upload_folder_path = ViewState["path"].ToString();


                if (!Directory.Exists(upload_folder_path))
                {
                    Directory.CreateDirectory(Server.MapPath(upload_folder_path));
                }

                foreach (HttpPostedFile uploadedFile in fup.PostedFiles)
                {
                    string filename = uploadedFile.FileName;
                    uploadedFile.SaveAs(System.IO.Path.Combine((upload_folder_path), filename));
                    // listofuploadedfiles.Text += String.Format("{0}<br />", uploadedFile.FileName);
                }

                //  DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(upload_folder_path));
                string path = upload_folder_path;
                getFileList(path);
                clearTree();
            }

        }
        catch (Exception)
        {

        }
    }

    private void getFileList(string path, string nodeName = "")
    {
        int cnt = 0;
        lblPath.Text = "Selected Folder : " + nodeName.Split('(')[0];
        ViewState["path"] = path;
        ViewState["nodeName"] = nodeName;

        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("Name");
        dt.Columns.Add("Path");
        dt.Columns.Add("PreviewPath");

        // string uname = Session["PatientId"].ToString() + "_";
        string uname = "";

        //System.Collections.Generic.IEnumerable<string> files = Directory.GetFiles(path);
        System.Collections.Generic.IEnumerable<string> files = Directory.GetFiles(path, uname + "*");

        foreach (string file in files)
        {
            string filename = file.Substring(file.LastIndexOf("\\") + 1);

            DataRow dr = dt.NewRow();
            dr["Name"] = filename.Substring(uname.Length);
            dr["Path"] = Regex.Replace(path, @" \[.+?\]", "") + "/" + filename;
            dr["PreviewPath"] = "/GeneralDocuments/" + nodeName.Split('(')[0].Trim() + "/" + filename;
            dt.Rows.Add(dr);
            //lstBox.Items.Add(file.Name);
            cnt++;
        }

        gvDocument.DataSource = dt;
        gvDocument.DataBind();
    }

    private static string getFileCount(string path)
    {
        int cnt = 0;

        string uname = "";

        //System.Collections.Generic.IEnumerable<string> files = Directory.GetFiles(path);
        string[] files = Directory.GetFiles(path, uname + "*");

        cnt = files.Length;
        return " (" + cnt.ToString("00") + ")";
    }

    private void clearTree()
    {
        // Get the 'myTreeNodeCollection' from the 'myTreeViewBase' TreeView.
        TreeNodeCollection myTreeNodeCollection = treeView.Nodes;
        // Create an array of 'TreeNodes'.
        TreeNode[] myTreeNodeArray = new TreeNode[treeView.Nodes.Count];
        // Copy the tree nodes to the 'myTreeNodeArray' array.
        treeView.Nodes.CopyTo(myTreeNodeArray, 0);
        // Remove all the tree nodes from the 'myTreeViewBase' TreeView.
        treeView.Nodes.Clear();

        DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/GeneralDocuments/"));
        PopulateTreeView(rootInfo, null);
    }



    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        string DestFile = Server.MapPath("~/GeneralDocuments/") + DropDownList1.Text;
        File.Move(txtFileName.Value, DestFile + "/" + Path.GetFileName(txtFileName.Value));
        DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/GeneralDocuments/"));
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#myModal').modal('hide');", true);
        treeView.Nodes.Clear();
        PopulateTreeView(rootInfo, null);
        //    UpdatePanel1.Update();
    }
    protected void btnRename_Click(object sender, EventArgs e)
    {

        if (Directory.Exists(Server.MapPath("GeneralDocuments/" + txtOldFolderName.Text)) && txtNewFolderName.Text.Trim() != "")
        {
            Directory.Move(Server.MapPath("GeneralDocuments/" + txtOldFolderName.Text), Server.MapPath("GeneralDocuments/" + txtNewFolderName.Text));
        }
        DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/GeneralDocuments/"));

        treeView.Nodes.Clear();
        PopulateTreeView(rootInfo, null);
        //  Page.Response.Redirect("PatientDocument.aspx");
    }
}