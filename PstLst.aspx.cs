using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.IO;
using ClosedXML.Excel;
using System.Globalization;




public partial class PstLst : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["o_column"] = "FirstName";
            ViewState["c_order"] = "asc";
          //  BindProcudureList();
        }

    }
    protected void CustomValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtSearchFromdate.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void CustomValidator2_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        DateTime d;
        e.IsValid = DateTime.TryParseExact(e.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        txtSearchTodate.Text = d.ToShortDateString();
        //e.IsValid = false; 
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearchFromdate.Text = string.Empty;
        txtSearchTodate.Text = string.Empty;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProcudureList();
    }
    protected void BindProcudureList()
    {
        string query = "select ISNULL(CONVERT(VARCHAR(10),DOE,101),'') as DOE,Location,max(NoOfIE) as NoOfIE,max(NoOfFU) as NoOfFU from ( ";
        query += "select count(tblpat.PatientIE_ID) as NoOfIE,cast (0 as bigint) AS NoOfFU,tblpat.DOE as DOE, tblLoc.Location from tblpatientie tblpat inner join tblLocations tblLoc on tblpat.location_id=tblLoc.Location_ID ";

        query += " where (tblpat.doe BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101)) ";
        query += " group by tblLoc.Location,tblpat.DOE union all ";
        query += " select cast (0 as bigint) AS NoOfIE,count(tblFUPat.PatientFU_ID) as NoOfFU,tblFUPat.DOE as DOE, tblLoc.Location from tblFUPatient tblFUPat inner join tblLocations tblLoc on tblFUPat.location_id=tblLoc.Location_ID ";
        query += " where (tblFUPat.doe BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101)) ";
        query += " group by tblLoc.Location,tblFUPat.DOE ";
        query += " )t group by DOE,Location order by DOE asc ";
         
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {

            SqlCommand cm = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            con.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                gvProcedureTbl.DataSource = dt;
                Session["Datatableprocedure"] = dt;
                gvProcedureTbl.DataBind();
            }
            else
            {
                gvProcedureTbl.DataSource = null;
                gvProcedureTbl.DataBind();
            }
        }
    }


    protected void lkExportToexcel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Datatableprocedure"];

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "PstLst");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=PstLst.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }

    }

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        ViewState["z_sortexpresion"] = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, "DESC");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, "ASC");
        }

    }

    public string SortExpression
    {
        get
        {
            if (ViewState["z_sortexpresion"] == null)
                ViewState["z_sortexpresion"] = this.gvProcedureTbl.DataKeyNames[0].ToString();
            return ViewState["z_sortexpresion"].ToString();
        }
        set
        {
            ViewState["z_sortexpresion"] = value;
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = ((DataTable)Session["Datatableprocedure"]);
        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + " " + direction;
        this.gvProcedureTbl.DataSource = dv;
        gvProcedureTbl.DataBind();
    }

    protected void lnk_sorting_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        sortorder(lnk.CommandArgument);
    }

    private void sortorder(string colname)
    {
        try
        {

            if (ViewState["c_order"].ToString().ToUpper() == "ASC")
                ViewState["c_order"] = "DESC";
            else if (ViewState["c_order"].ToString().ToUpper() == "DESC")
                ViewState["c_order"] = "ASC";

            ViewState["o_column"] = colname;

            BindProcudureList();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlDates_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Datatableprocedure"];
        if (dt.Rows.Count > 0)
        {
            dt.DefaultView.Sort = "Scheduled";
            DataTable dtemp = dt.DefaultView.ToTable();

            DataView dataView = dtemp.DefaultView;
            
            gvProcedureTbl.DataSource = dataView;
            Session["DatatableprocedureFiltered"] = dataView.ToTable();
            gvProcedureTbl.DataBind();
        }
    }
}