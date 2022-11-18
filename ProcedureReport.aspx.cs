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




public partial class ProcedureReport : System.Web.UI.Page
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
            BindProcudureList();
        }

    }
    protected void BindProcudureList()
    {
        string query = "SELECT pm.sex,pm.LastName+', '+pm.FirstName 'Name',ISnull(pm.MC,'') AS MC,ie.Compensation as 'CaseType' ,lc.location,CASE when pm.Vaccinated = 1 THEN 'Yes' ELSE 'No' END AS Vaccinated,tp.MCODE ";
        string condition = " FROM  tblProceduresDetail tp  inner join tblPatientIE ie on tp.PatientIE_ID = ie.PatientIE_ID inner join tblPatientMaster pm on pm.Patient_ID=ie.Patient_ID left join dbo.tblLocations lc ON ie.Location_ID = lc.Location_ID inner join tblAttorneys a on a.Attorney_ID = ie.Attorney_ID ";
        string condition1 = null;
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Scheduled,101),'') as Scheduled ";
        condition1 += condition1 = " (tp.Scheduled>='" + DateTime.Now.Date + "')";

        query += condition;
        if (!string.IsNullOrEmpty(condition1))
        {
            condition1 = condition1.Insert(0, " where ");
            query += condition1;
        }

        query = query + " order by " + ViewState["o_column"] + " " + ViewState["c_order"];

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))//connString_V3
        {

            SqlCommand cm = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            con.Open();
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dt.DefaultView.Sort = "Scheduled";
                DataTable dtemp = dt.DefaultView.ToTable();
                DataTable dtdistinctrecord = dtemp.DefaultView.ToTable(true, "Scheduled");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDates.ClearSelection();
                    ddlDates.DataValueField = "Scheduled";
                    ddlDates.DataTextField = "Scheduled";

                    ddlDates.DataSource = dtdistinctrecord;
                    ddlDates.DataBind();
                }


                Session["Datatableprocedure"] = dt;
                DataView dataView = dtemp.DefaultView;

                if (!string.IsNullOrEmpty(ddlDates.SelectedValue))
                {
                    dataView.RowFilter = "Scheduled = '" + ddlDates.SelectedValue + "'";
                }
                dataView.Sort = "MC";
                gvProcedureTbl.DataSource = dataView;
                Session["DatatableprocedureFiltered"] = dataView.ToTable();
                gvProcedureTbl.DataBind();
            }
            else
            {
                gvProcedureTbl.DataSource = null;
                Session["Datatableprocedure"] = null;
                gvProcedureTbl.DataBind();
            }
        }
    }


    protected void lkExportToexcel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["DatatableprocedureFiltered"];

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "ProcedureReport");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=ProcedureReport" + ddlDates.SelectedValue+".xlsx");
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

            if (!string.IsNullOrEmpty(ddlDates.SelectedValue))
            {
                dataView.RowFilter = "Scheduled = '" + ddlDates.SelectedValue + "'";
            }

            dataView.Sort = "MC";
            gvProcedureTbl.DataSource = dataView;
            Session["DatatableprocedureFiltered"] = dataView.ToTable();
            gvProcedureTbl.DataBind();
        }
    }
}