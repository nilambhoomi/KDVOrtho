using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateFolder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!Page.IsPostBack)
        {
            LoadGrid();

        }
    }



    protected void btnCreateFolder_Click(object sender, EventArgs e)
    {
        string path = Server.MapPath("~/" + ddl_type.SelectedItem.Value + "/") + txtFolderName.Text;


        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
            txtFolderName.Text = "";
            LoadGrid();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "sideFun", "alert('Sorry !! Directory is already exists with this name.');", true);
        }


    }
    public void LoadGrid()
    {
        // txtFolderName.Text = "";
        string root = Server.MapPath("~/" + ddl_search_type.SelectedItem.Value);
        //string root = @"F:\Locations\" + DropDownList1.SelectedValue;
        string[] fileEntries = Directory.GetDirectories(root);

        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("Name");


        foreach (string filename in fileEntries)
        {
            string fname = filename.Split('\\').Last();
            DataRow dr = dt.NewRow();
            dr["Name"] = fname;
            dt.Rows.Add(dr);
        }

        gvDocument.DataSource = dt;
        gvDocument.DataBind();
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Button btnFolder = sender as Button;
            string root = Server.MapPath("~/" + ddl_search_type.SelectedItem.Value + "/" + btnFolder.CommandArgument);
            int val = Directory.GetFiles(root).Count();

            if (val == 0)
            {
                Directory.Delete(root);
                LoadGrid();
            }
            else
            {
                hidName.Value = root;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "sideFun", "deleteFun();", true);
            }

        }
        catch (Exception)
        {

        }
    }

    protected void btndeleteall_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hidName.Value))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(hidName.Value.TrimEnd(','));


            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            Directory.Delete(hidName.Value.TrimEnd(','));
            LoadGrid();
        }
        hidName.Value = "";
    }

    protected void ddl_search_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        if (hdFileName.Value.Trim() != "" && txt_new_name.Text.Trim() != "")
        {
            Directory.Move(Server.MapPath("~/" + ddl_type.SelectedItem.Value + "/" + hdFileName.Value), Server.MapPath("~/" + ddl_type.SelectedItem.Value + "/" + txt_new_name.Text));
            LoadGrid();
        }
    }
}