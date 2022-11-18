using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddGroups : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["id"] != null)
            {
                string id = Request.QueryString["id"];
                BindGroupDetails(id);
                DataSet ds = db.selectData("select page_id from tblGroups  where id=" + Request["id"].ToString());

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    string pages = ds.Tables[0].Rows[0][0].ToString();

                    for (int i = 0; i < chkPages.Items.Count; i++)
                    {
                        if (pages.ToString().Contains(chkPages.Items[i].Value))
                            chkPages.Items[i].Selected = true;
                    }
                }
            }

            GetLocations();
            GetReports();
            GetRole();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "", locations = "", locations_id = "", page_id = "", pages = "", reports = "", roleid = "";


            for (int i = 0; i < chkLocations.Items.Count; i++)
            {
                if (chkLocations.Items[i].Selected)
                {
                    locations = locations + "," + chkLocations.Items[i].Text;
                    locations_id = locations_id + "," + chkLocations.Items[i].Value;
                }
            }

            for (int i = 0; i < chkPages.Items.Count; i++)
            {
                if (chkPages.Items[i].Selected)
                {
                    pages = pages + "," + chkPages.Items[i].Text;
                    page_id = page_id + "," + chkPages.Items[i].Value;
                }
            }

            for (int i = 0; i < chkReports.Items.Count; i++)
            {
                if (chkReports.Items[i].Selected)
                {
                    reports = reports + "," + chkReports.Items[i].Text;

                }
            }

            for (int i = 0; i < chkRole.Items.Count; i++)
            {
                if (chkRole.Items[i].Selected)
                {
                    roleid = roleid + "," + chkRole.Items[i].Value;

                }
            }

            if (Request["id"] != null)
            {
                query = "update tblGroups set Name='" + txtName.Text + "',";

                query = query + " page_id='" + page_id.TrimStart(',') + "',pages='" + pages.TrimStart(',') + "',";

                query = query + " locations_id='" + locations_id.TrimStart(',') + "',locations='" + locations.TrimStart(',') + "',reports='" + reports.TrimStart(',') + "',role_id='" + roleid.TrimStart(',') + "' where Id=" + Request["id"].ToString();
            }
            else
            {
                query = "insert into tblGroups(Name,locations_id,locations,page_id,pages,reports,role_id) values('" + txtName.Text + "',";
                query = query + " '" + locations_id.TrimStart(',') + "','" + locations.TrimStart(',') + "','" + page_id.TrimStart(',') + "','" + pages.TrimStart(',') + "','" + reports.TrimStart(',') + "','" + roleid.TrimStart(',') + "') ";
            }

            db.executeQuery(query);
            Response.Redirect("ViewGroups.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    public void GetLocations()
    {
        try
        {
            DataSet ds = db.selectData("select * from tblLocations order by Location ASC ");

            chkLocations.DataTextField = "Location";
            chkLocations.DataValueField = "Location_ID";

            chkLocations.DataSource = ds;
            chkLocations.DataBind();

            if (Session["chklocations"] != null)
            {

                for (int i = 0; i < chkLocations.Items.Count; i++)
                {
                    if (Session["chklocations"].ToString().Contains(chkLocations.Items[i].Value))
                        chkLocations.Items[i].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {

        }

    }

    public void GetReports()
    {
        try
        {
            DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/TemplateStore/"));
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Name");
            DataRow _dtr = null;
            foreach (DirectoryInfo directory in rootInfo.GetDirectories())
            {
                foreach (FileInfo file in directory.GetFiles())
                {

                    _dtr = dt.NewRow();
                    _dtr["Name"] = file.Name;
                    dt.Rows.Add(_dtr);
                }
            }
            //DataSet ds = db.selectData("select * from tblLocations");

            chkReports.DataTextField = "Name";
            chkReports.DataValueField = "Name";

            //Sorting the Table
            DataView dv = dt.DefaultView;
            dv.Sort = "Name asc";
            DataTable sortedtable1 = dv.ToTable();

            chkReports.DataSource = sortedtable1;
            chkReports.DataBind();

            if (Session["chkreport"] != null)
            {

                for (int i = 0; i < chkReports.Items.Count; i++)
                {
                    if (Session["chkreport"].ToString().Contains(chkReports.Items[i].Value))
                        chkReports.Items[i].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {

        }

    }

    public void GetRole()
    {
        try
        {
            DataSet ds = db.selectData("select * from tblRole");

            chkRole.DataTextField = "RoleName";
            chkRole.DataValueField = "RoleID";

            chkRole.DataSource = ds;
            chkRole.DataBind();

            if (Session["chkrole"] != null)
            {

                for (int i = 0; i < chkRole.Items.Count; i++)
                {
                    if (Session["chkrole"].ToString().Contains(chkRole.Items[i].Value))
                        chkRole.Items[i].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void BindGroupDetails(string Id = null)
    {
        DataSet ds = db.selectData("select * from tblGroups  where Id=" + Id);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();

            Session["chkreport"] = ds.Tables[0].Rows[0]["reports"].ToString();
            Session["chkrole"] = ds.Tables[0].Rows[0]["role_id"].ToString();
            Session["chklocations"] = ds.Tables[0].Rows[0]["locations_id"].ToString();
            Session["chkpages"] = ds.Tables[0].Rows[0]["page_id"].ToString();

        }

    }
}