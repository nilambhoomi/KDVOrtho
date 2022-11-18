using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProcedureSurgeryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            bindLocation();
            //BindProcudureList();
        }

    }

    protected void BindProcudureList()
    {
        string query = "SELECT tp.ProcedureDetail_ID,pm.sex,pm.LastName+', '+pm.FirstName 'Name',ISnull(tp.MC_TYPE,'') AS MC,ie.Compensation as 'CaseType' ,lc.location,CASE when pm.Vaccinated = 1 THEN 'Yes' ELSE 'No' END AS Vaccinated,tp.MCODE,tp.BodyPart ";
        query += ", CASE when tp.Ins_Ver_Status=1 THEN 'YES' ELSE 'No' END as Ins_ver_status ";
        query += ", CASE when tp.MC_TYPE='Yes' then ISnull(tp.MC_Report_Status,'') ELSE 'Received' end as MC_Status ";
        query += ", CASE when ie.Compensation='WC' then ISNULL(tp.CT_Report_Status,'Received') else 'Received' end as 'Case_Status' ";
        query += ", CASE when tp.Ins_Ver_Status=1 then ISNULL(tp.Backup_Line,'Received') else 'Received' end as 'InsVerStatus'  ";
        query += ", CASE when tp.IsVaccinated=1 then ISNULL(tp.Vac_Status,'Received') else 'Received' end as 'Vac_Status'  ";
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Scheduled,101),'') as Scheduled ";
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Executed,101),'') as Executed ";
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Requested,101),'') as Requested ";
        query += " FROM  tblProceduresDetail tp  inner join tblPatientIE ie on tp.PatientIE_ID = ie.PatientIE_ID inner join tblPatientMaster pm on pm.Patient_ID=ie.Patient_ID left join dbo.tblLocations lc ON ie.Location_ID = lc.Location_ID inner join tblAttorneys a on a.Attorney_ID = ie.Attorney_ID where 1=1  ";


        if (!string.IsNullOrEmpty(txtSearchFromdate.Text) && !string.IsNullOrEmpty(txtSearchTodate.Text))
            query += " and ((tp.Scheduled BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101)) or (tp.Requested BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))) ";

        if (ddlLocation.SelectedIndex > 0)
        {
            query += " and lc.Location_ID = isnull(" + ddlLocation.SelectedValue + ",lc.Location_ID)";
        }

        query = query + " order by tp.Scheduled,tp.Requested,tp.ProcedureDetail_ID desc";

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
                    //ddlDates.ClearSelection();
                    //ddlDates.DataValueField = "Scheduled";
                    //ddlDates.DataTextField = "Scheduled";

                    //ddlDates.DataSource = dtdistinctrecord;
                    //ddlDates.DataBind();
                }


                Session["Datatableprocedure"] = dt;
                DataView dataView = dtemp.DefaultView;

                //if (!string.IsNullOrEmpty(ddlDates.SelectedValue))
                //{
                //    dataView.RowFilter = "Scheduled = '" + ddlDates.SelectedValue + "'";
                //}
                dataView.Sort = "MC";
                gvProcedureTbl.DataSource = dataView;
                Session["DatatableprocedureFiltered"] = dataView.ToTable();
                gvProcedureTbl.DataBind();

                gvProcedureTbl.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            Response.AddHeader("content-disposition", "attachment;filename=ProcedureReport.xlsx");
            // Response.AddHeader("content-disposition", "attachment;filename=ProcedureReport" + ddlDates.SelectedValue + ".xlsx");
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

            //if (!string.IsNullOrEmpty(ddlDates.SelectedValue))
            //{
            //    dataView.RowFilter = "Scheduled = '" + ddlDates.SelectedValue + "'";
            //}

            dataView.Sort = "MC";
            gvProcedureTbl.DataSource = dataView;
            Session["DatatableprocedureFiltered"] = dataView.ToTable();
            gvProcedureTbl.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProcudureList();
    }

    private void bindLocation()
    {
        DBHelperClass db = new DBHelperClass();
        DataSet ds = new DataSet();
        ds = db.selectData("select Location,Location_ID from tblLocations where is_active=1 Order By Location");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlLocation.ClearSelection();
            ddlLocation.DataValueField = "Location_ID";
            ddlLocation.DataTextField = "Location";

            ddlLocation.DataSource = ds;
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, new ListItem("-- Location --", "0"));
        }
    }

    [WebMethod]
    public static string GetNoteDetail(long ProcedureDetailID)
    {
        DBHelperClass da = new DBHelperClass();
        DataTable dt = new DataTable();
        string query = "select ProcedureDetail_ID,MC_Note,MC_Date,MC_Report_Status,MC_ReSche_Date,MC_Type,CT_AUTH_Date,CT_Report_Status,CT_Note,CT_ReSche_Date,Ins_Ver_Status,Backup_Line,Ins_Note,IsVaccinated,Vac_Status,Vac_Note from tblProceduresDetail where ProcedureDetail_ID=" + ProcedureDetailID;

        DataTable data = da.selectDatatable(query);
        string json = JsonConvert.SerializeObject(data);
        return json;
    }


    [WebMethod]
    public static string SaveMCNote(long ProcedureDetailID, string note, string mc_type, DateTime MC_Date, string MC_Report_Status, DateTime MC_ReSche_Date)
    {
        DBHelperClass da = new DBHelperClass();
        DataTable dt = new DataTable();


        string query = "update tblProceduresDetail set MC_Note='" + note + "',mc_type='" + mc_type + "',MC_Date='" + MC_Date + "',MC_Report_Status='" + MC_Report_Status + "',MC_ReSche_Date='" + MC_ReSche_Date + "' where ProcedureDetail_ID=" + ProcedureDetailID;


        da.executeQuery(query);

        string json = JsonConvert.SerializeObject(true);
        return json;
    }

    [WebMethod]
    public static string SaveCTNote(long ProcedureDetailID, string note, DateTime CT_AUTH_Date, string CT_Report_Status, DateTime CT_ReSche_Date)
    {
        DBHelperClass da = new DBHelperClass();
        DataTable dt = new DataTable();


        string query = "update tblProceduresDetail set MC_Note='" + note + "',CT_AUTH_Date='" + CT_AUTH_Date + "',CT_Report_Status='" + CT_Report_Status + "',CT_ReSche_Date='" + CT_ReSche_Date + "' where ProcedureDetail_ID=" + ProcedureDetailID;


        da.executeQuery(query);

        string json = JsonConvert.SerializeObject(true);
        return json;
    }

    [WebMethod]
    public static string SaveIVNote(long ProcedureDetailID, string note, string Backup_Line, bool Ins_Ver_Status)
    {
        DBHelperClass da = new DBHelperClass();
        DataTable dt = new DataTable();


        string query = "update tblProceduresDetail set Ins_Note='" + note + "',Ins_Ver_Status='" + Ins_Ver_Status + "',Backup_Line='" + Backup_Line + "'  where ProcedureDetail_ID=" + ProcedureDetailID;


        da.executeQuery(query);

        string json = JsonConvert.SerializeObject(true);
        return json;
    }

    [WebMethod]
    public static string SaveVCNote(long ProcedureDetailID, string note, string Vac_Status, string IsVaccinated)
    {
        DBHelperClass da = new DBHelperClass();
        DataTable dt = new DataTable();

        if (IsVaccinated == "No")
            IsVaccinated = "False";
        else
            IsVaccinated = "True";

        string query = "update tblProceduresDetail set Vac_Note='" + note + "',Vac_Status='" + Vac_Status + "',IsVaccinated='" + IsVaccinated + "'  where ProcedureDetail_ID=" + ProcedureDetailID;


        da.executeQuery(query);

        string json = JsonConvert.SerializeObject(true);
        return json;
    }
}