using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IntakeSheet.Entity;
using System.Data;

public partial class GetMAProviders : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            bindLocation();
            bindMAandProviders();

            HttpCookie providersCookie = Request.Cookies["LoWEcatdsafion"];
            if (providersCookie != null && providersCookie.HasKeys)
            {
                string location = Convert.ToString(providersCookie["Location"]);
                if (!string.IsNullOrEmpty(location))
                {
                    ddLocation.SelectedIndex = ddLocation.Items.IndexOf(ddLocation.Items.FindByValue(location));
                }
                if (providersCookie["Providers"] != null)
                {
                    string allProviders = Convert.ToString(providersCookie["Providers"]);
                    string[] providers = allProviders.Split('~');
                    foreach (string provider in providers)
                    {
                        ListItem listItem = lbMAandProviders.Items.FindByText(provider.Trim());
                        if (listItem != null)
                        {
                            lbSelectedMAandProviders.Items.Add(listItem);
                            lbMAandProviders.Items.Remove(listItem);
                        }
                    }
                }
            }
        }
    }
    protected void bindLocation()
    {
        //BusinessLogic _bl = new BusinessLogic();
        //ddLocation.Items.Clear();
        //ddLocation.Items.Add(new ListItem("Please Select", "Please Select"));
        //foreach (string[] _location in _bl.getLocations())
        //{
        //    ddLocation.Items.Add(new ListItem(_location[1], _location[0]));
        //}

        DataSet ds = new DataSet();

        string query = "select Location,Location_ID from tblLocations where Is_Active='True' ";

        if (Request["id"] == null && !string.IsNullOrEmpty(Session["Locations"].ToString()))
        {
            query = query + " and Location_ID in (" + Session["Locations"] + ")";
        }
        query = query + " Order By Location";

        ds = db.selectData(query);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddLocation.DataValueField = "Location_ID";
            ddLocation.DataTextField = "Location";

            ddLocation.DataSource = ds;
            ddLocation.DataBind();

            ddLocation.Items.Insert(0, new ListItem("Please Select", "0"));
        }

        if (ddLocation.Items.Count.Equals(2))
        {
            ddLocation.SelectedIndex = 1;
        }
    }
    protected void bindMAandProviders()
    {
        BusinessLogic bl = new BusinessLogic();
        lbMAandProviders.Items.Clear();
        foreach (User user in bl.getAllProvidersAndMA())
        {
            lbMAandProviders.Items.Add(new ListItem(user.FirstName + ' ' + user.LastName, user.UserId.ToString()));
        }
    }
    protected void moveAllLeft_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbSelectedMAandProviders.Items)
        {
            lbMAandProviders.Items.Add(li);
        }
        lbSelectedMAandProviders.Items.Clear();
    }

    protected void moveLeft_Click(object sender, EventArgs e)
    {
        var result = (from ListItem item in lbSelectedMAandProviders.Items where item.Selected select item);
        if (result != null)
        {
            List<ListItem> selectedItems = result.ToList();
            foreach (ListItem item in selectedItems)
            {
                lbMAandProviders.Items.Add(item);
                lbSelectedMAandProviders.Items.Remove(item);
            }
        }

    }

    protected void moveRight_Click(object sender, EventArgs e)
    {
        var result = (from ListItem item in lbMAandProviders.Items where item.Selected select item);
        if (result != null)
        {
            List<ListItem> selectedItems = result.ToList();
            foreach (ListItem item in selectedItems)
            {
                lbSelectedMAandProviders.Items.Add(item);
                lbMAandProviders.Items.Remove(item);
            }
        }

    }

    protected void moveAllRight_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbMAandProviders.Items)
        {
            lbSelectedMAandProviders.Items.Add(li);
        }
        lbMAandProviders.Items.Clear();
    }

    protected void btnProceed_Click(object sender, EventArgs e)
    {

        HttpCookie providersCookie = new HttpCookie("LoWEcatdsafion");
        if (ddLocation.SelectedIndex > 0)
        {
            Session["Location"] = ddLocation.SelectedValue;
            providersCookie["Location"] = ddLocation.SelectedValue;
        }
        if (lbSelectedMAandProviders.Items.Count > 0)
        {
            string providers = string.Empty;
            foreach (ListItem li in lbSelectedMAandProviders.Items)
            {
                providers += (li.Text + "~ ");
            }
            Session["Providers"] = providers.Trim().TrimEnd('~');
            providersCookie["Providers"] = providers.Trim().TrimEnd('~');
        }
        if (providersCookie.HasKeys)
        {
            Response.Cookies.Add(providersCookie);
        }
        Response.Redirect("~/PatientIntakeList.aspx");
    }
}
