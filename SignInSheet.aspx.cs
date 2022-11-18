using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SignInSheet : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["IN"] == null)
            {

                bindLocation();
                txt_Date.Text = System.DateTime.Now.ToString("dd-MM-yyyy");

            }
            else
            {
                txt_Date.Text = Convert.ToDateTime(Session["date"].ToString()).ToString("MM/dd/yyyy");
                bindLocation();
                ddl_location.SelectedValue = Session["location"].ToString();
                string date = Session["date"].ToString();
                string LOCATIONID = Session["location"].ToString();
                LoadPatientIE(date, Convert.ToInt16(LOCATIONID));
                ddl_select_SelectedIndexChanged(Session["sender"], e);

            }
        }




    }

    public void LoadPatientIE(string date, int LId)
    {
        try
        {
            //  int totalcount;
            DataSet dt = new DataSet();

            //dt = db.selectData(query);
            dt = db.PatientSP(date, LId);

            DataRow dr = null;
            if (dt.Tables[0].Rows.Count > 0)
            {

                if (dt.Tables[0].Rows.Count > 0)
                {
                    rpview.DataSource = dt;
                    rpview.DataBind();

                }
            }
            else
            {
                rpview.DataSource = null;
                rpview.DataBind();
            }
            //PopulatePager(totalcount, pageindex);
            //lblcount.Text = totalcount.ToString();
            //  BindRepeater();
        }
        catch (Exception ex)
        {
        }
    }

    //private void BindRepeater()
    //{
    //    throw new NotImplementedException();
    //}
    public void PopulatePager(int recordCount, int currentPage)
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

        //  string name = "";
        //  if (!string.IsNullOrEmpty(txt_name.Text))
        //{
        //    //name = txt_name.Text.Trim();
        //   // LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", pageIndex);
        //}
        //else
        //    this.LoadPatientIE("", pageIndex);
    }
    private void bindLocation()
    {
        DataSet ds = new DataSet();
        ds = db.selectData("select Location,Location_ID from tblLocations Order By Location");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- Location --", "0"));

            DataSet locds = new DataSet();
            locds.ReadXml(Server.MapPath("~/LocationXML.xml"));


            ddl_location.ClearSelection();
            ddl_location.Items.FindByValue(locds.Tables[0].Rows[0][1].ToString()).Selected = true;



        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        string temp = "where  =" + txt_Date.Text + "and location" + ddl_location.SelectedValue;
        Session["locationstring"] = Convert.ToString(ddl_location.SelectedItem);
        if (ddl_location.SelectedIndex == 0)
        {
            lbl_msg.InnerText = "Please Select location and Date";
        }
        else
        {
            Session["date"] = txt_Date.Text;
            Session["location"] = ddl_location.SelectedValue;
            LoadPatientIE(txt_Date.Text, Convert.ToInt16(ddl_location.SelectedValue));
            string LogName = Session["uname"].ToString();
            string LogLocation;
            if (string.IsNullOrWhiteSpace(Convert.ToString(Session["locationstring"])))
            {
                LogLocation = "";
            }
            else
            {
                LogLocation = Session["locationstring"].ToString();
            }
            string LogIp = "";
            string LogDescription = "";
            string LogIntime = null;
            string LogOutTime = Convert.ToString(System.DateTime.Now);
            string log_id = Session["log"].ToString();
            db.logDetailtbl(Convert.ToInt32(Session["log"].ToString()), "signIn Sheet", Convert.ToString(System.DateTime.Now));
            db.logDetail(LogName, LogLocation, LogIp, LogDescription, LogIntime, LogOutTime, log_id);
        }
    }
    protected void btnneck_click(object sender)
    {
        string temp = "where  =" + txt_Date.Text + "and location" + ddl_location.SelectedValue;
        if (ddl_location.SelectedIndex == 0)
        {
            lbl_msg.InnerText = "Please Select location and Date";
        }
        else
        {
            Session["date"] = txt_Date.Text;
            Session["location"] = ddl_location.SelectedValue;
            LoadPatientIE(txt_Date.Text, Convert.ToInt16(ddl_location.SelectedValue));


        }
    }
    protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string ide;
            ide = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;

            if (string.IsNullOrWhiteSpace(ide))
            {
                ide = (e.Item.FindControl("hfCustomerId1") as HiddenField).Value;
            }
            Session["ide"] = ide;
            DataTable ds = new DataTable();
            DataTable ds1 = new DataTable();
            Panel panel1 = e.Item.FindControl("Panel1") as Panel;
            DropDownList ddl_select = e.Item.FindControl("ddl_select") as DropDownList;
            ListItemCollection collection = new ListItemCollection();

            collection.Add(new ListItem("Select"));

            ds1 = db.GetGrid(Convert.ToInt16(ide));
            if (ds1.Rows[0]["neck"].ToString() == "neck")
            {
                try
                {
                    string name = "neck";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvneck = e.Item.FindControl("gvneck") as GridView;
                    gvneck.BorderWidth = 2;
                    gvneck.DataSource = ds;
                    gvneck.Font.Size = 8;
                    gvneck.Visible = false;
                    gvneck.DataBind();
                    collection.Add(new ListItem("Neck"));
                    bindedit(gvneck);
                    ds.Clear();
                }
                catch (Exception ex)
                { }
            }
            if (ds1.Rows[0]["MidBack"].ToString() == "MidBack")
            {
                try
                {
                    string name = "MidBack";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvmidback = e.Item.FindControl("gvmidback") as GridView;
                    gvmidback.BorderWidth = 2;
                    gvmidback.DataSource = ds;
                    gvmidback.Font.Size = 8;
                    gvmidback.Visible = false;
                    gvmidback.DataBind();
                    collection.Add(new ListItem("Midback"));
                    bindedit(gvmidback);
                    ds.Clear();
                }
                catch (Exception ex)
                {
                }
            }
            if (ds1.Rows[0]["LowBack"].ToString() == "LowBack")
            {
                try
                {
                    string name = "LowBack";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvlowback = e.Item.FindControl("gvlowback") as GridView;
                    gvlowback.BorderWidth = 2;
                    gvlowback.DataSource = ds;
                    gvlowback.Font.Size = 8;
                    gvlowback.Visible = false;
                    gvlowback.DataBind();
                    collection.Add(new ListItem("LowBack"));
                    bindedit(gvlowback);
                    ds.Clear();
                }
                catch (Exception ex)
                { }
            }
            if (ds1.Rows[0]["Shoulder"].ToString() == "Shoulder")
            {
                try
                {
                    string name = "Shoulder";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvshoulder = e.Item.FindControl("gvshoulder") as GridView;
                    gvshoulder.BorderWidth = 2;
                    gvshoulder.DataSource = ds;
                    gvshoulder.Font.Size = 8;
                    gvshoulder.Visible = false;
                    gvshoulder.DataBind();
                    collection.Add(new ListItem("Shoulder"));
                    bindedit(gvshoulder);
                    ds.Clear();
                }
                catch (Exception ex)
                { }
            }
            if (ds1.Rows[0]["knee"].ToString() == "knee")
            {
                try
                {
                    string name = "knee";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvknee = e.Item.FindControl("gvknee") as GridView;
                    gvknee.BorderWidth = 2;
                    gvknee.DataSource = ds;
                    gvknee.Font.Size = 8;
                    gvknee.Visible = false;
                    gvknee.DataBind();
                    collection.Add(new ListItem("knee"));
                    bindedit(gvknee);
                    ds.Clear();
                }
                catch (Exception ex)
                { }

            }
            if (ds1.Rows[0]["elbow"].ToString() == "elbow")
            {
                try
                {
                    string name = "elbow";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvelbow = e.Item.FindControl("gvelbow") as GridView;
                    gvelbow.BorderWidth = 2;
                    gvelbow.DataSource = ds;
                    gvelbow.Font.Size = 8;
                    gvelbow.DataBind();
                    gvelbow.Visible = false;
                    collection.Add(new ListItem("elbow"));
                    bindedit(gvelbow);
                    ds.Clear();
                }
                catch (Exception ex)
                { }

            }
            if (ds1.Rows[0]["wrist"].ToString() == "wrist")
            {
                try
                {
                    string name = "wrist";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvwrist = e.Item.FindControl("gvwrist") as GridView;
                    gvwrist.BorderWidth = 2;
                    gvwrist.DataSource = ds;
                    gvwrist.Font.Size = 8;
                    gvwrist.Visible = false;
                    gvwrist.DataBind();
                    collection.Add(new ListItem("wrist"));
                    bindedit(gvwrist);
                    ds.Clear();
                }
                catch (Exception ex)
                { }

            }
            if (ds1.Rows[0]["hip"].ToString() == "hip")
            {
                try
                {
                    string name = "hip";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvhip = e.Item.FindControl("gvhip") as GridView;
                    gvhip.BorderWidth = 2;
                    gvhip.DataSource = ds;
                    gvhip.Font.Size = 8;
                    gvhip.Visible = false;
                    gvhip.DataBind();
                    collection.Add(new ListItem("hip"));
                    bindedit(gvhip);
                    ds.Clear();
                }
                catch (Exception ex)
                { }

            }
            if (ds1.Rows[0]["ankle"].ToString() == "ankle")
            {
                try
                {
                    string name = "ankle";
                    string id = Convert.ToString(ide);
                    ds = db.GetGriddata(name);
                    GridView gvankle = e.Item.FindControl("gvankle") as GridView;
                    gvankle.BorderWidth = 2;
                    gvankle.DataSource = ds;
                    gvankle.Font.Size = 8;
                    gvankle.Visible = false;
                    gvankle.DataBind();
                    collection.Add(new ListItem("ankle"));
                    bindedit(gvankle);
                    ds.Clear();
                }
                catch (Exception ex)
                { }
            }
            ddl_select.DataSource = collection;
            ddl_select.DataBind();
        }
    }
    protected void ddl_select_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["sender"] = sender;
        Session["e"] = e;
        var btn = (DropDownList)sender;
        var item = (RepeaterItem)btn.NamingContainer;
        var ddl = (DropDownList)item.FindControl("ddl_select");
        var gvneck = (GridView)item.FindControl("gvneck");
        var gvmidback = (GridView)item.FindControl("gvmidback");
        var gvlowback = (GridView)item.FindControl("gvlowback");
        var gvshoulder = (GridView)item.FindControl("gvshoulder");
        var gvknee = (GridView)item.FindControl("gvknee");
        var gvelbow = (GridView)item.FindControl("gvelbow");
        var gvwrist = (GridView)item.FindControl("gvwrist");
        var gvhip = (GridView)item.FindControl("gvhip");
        var gvankle = (GridView)item.FindControl("gvankle");
        //   string temp = Session["case"].ToString();

        //if (Session["case"].ToString() != ddl.SelectedValue)
        //{

        Session["case"] = ddl.SelectedValue;

        //}
        string caseSwitch = Session["case"].ToString();

        switch (caseSwitch)
        {
            case "Select":
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "Neck":
                gvneck.Visible = true;
                bindedit(gvneck);
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "Midback":
                gvmidback.Visible = true;
                bindedit(gvmidback);
                gvneck.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "LowBack":
                gvlowback.Visible = true;
                bindedit(gvlowback);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "Shoulder":
                gvshoulder.Visible = true;
                bindedit(gvshoulder);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "knee":
                gvknee.Visible = true;
                bindedit(gvknee);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "elbow":
                gvelbow.Visible = true;
                bindedit(gvelbow);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "wrist":
                gvelbow.Visible = true;
                bindedit(gvelbow);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvhip.Visible = false;
                gvankle.Visible = false;
                break;
            case "hip":
                gvhip.Visible = true;
                bindedit(gvhip);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvankle.Visible = false;
                break;
            case "ankle":
                gvankle.Visible = true;
                bindedit(gvankle);
                gvneck.Visible = false;
                gvmidback.Visible = false;
                gvlowback.Visible = false;
                gvshoulder.Visible = false;
                gvknee.Visible = false;
                gvelbow.Visible = false;
                gvwrist.Visible = false;
                gvhip.Visible = false;
                break;
            default:

                break;
        }



    }
    protected void bindedit(GridView grview)
    {

        foreach (GridViewRow gr in grview.Rows)
        {
            if (string.IsNullOrWhiteSpace(gr.Cells[1].Text) || gr.Cells[1].Text == "&nbsp;")
            {

            }
            else
            {
                gr.Cells[1].Text = Convert.ToDateTime(gr.Cells[1].Text).ToString("MM/dd/yyyy");
            }
            if (string.IsNullOrWhiteSpace(gr.Cells[2].Text) || gr.Cells[2].Text == "&nbsp;")
            {

            }
            else
            {
                gr.Cells[2].Text = Convert.ToDateTime(gr.Cells[2].Text).ToString("MM/dd/yyyy");
            }
            if (string.IsNullOrWhiteSpace(gr.Cells[3].Text) || gr.Cells[3].Text == "&nbsp;")
            {

            }
            else
            {
                gr.Cells[3].Text = Convert.ToDateTime(gr.Cells[3].Text).ToString("MM/dd/yyyy");
            }
            ImageButton ib = new ImageButton();
            if (string.IsNullOrWhiteSpace(gr.Cells[4].Text) || gr.Cells[4].Text == "&nbsp;")
            {
                string ide = Session["ide"].ToString();
                ib.ImageUrl = "~/img/GridEdit.png";
                string url = "Edit_SignInSheet.aspx?id=" + ide + "&MCODE=" + gr.Cells[0].Text;
                ib.Attributes.Add("OnClick", String.Format("popupCenter('" + url + "', 'Update',450,450);", this.ID));
            }
            else
            {
                ib.ImageUrl = "~/img/GridEdit.png";
                string url = "Edit_SignInSheet.aspx?id=" + gr.Cells[4].Text + "&MCODE=" + gr.Cells[0].Text;
                ib.Attributes.Add("OnClick", String.Format("popupCenter('" + url + "', 'Update',450,450);", this.ID));
            }
            gr.Cells[4].Controls.Add(ib);
        }

    }
}






