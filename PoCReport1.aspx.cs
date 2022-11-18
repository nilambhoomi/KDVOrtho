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




public partial class PoCReport1 : System.Web.UI.Page
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

            lnkTransfer.Attributes.Add("onclick", "javascript:return ExecuteConfirm()");
            bindLocation();
        }
        if (chkScheduled.Checked)
        {
            lnkTransfer.Attributes.Add("style", "display:block");
            lnkRescheduled.Attributes.Add("style", "display:block");
        }
        else
        {
            lnkTransfer.Attributes.Add("style", "display:none");
            lnkRescheduled.Attributes.Add("style", "display:none");
        }
        //BindProcudureList();
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

    protected void BindProcudureList()
    {
        string query = "select  tp.ProcedureDetail_ID,pm.sex,pm.LastName+', '+pm.FirstName 'Name',ie.Compensation as 'Case' ,ISNULL(CONVERT(VARCHAR(10),pm.DOB,101),'') as DOB ,ISNULL(CONVERT(VARCHAR(10),ie.DOA,101),'') as DOA ,tp.MCODE,ISnull(pm.MC,'') AS MC,pm.phone+','+pm.Phone2 as Phone,lc.location,(select ic.InsCo from tblInsCos ic where ic.InsCo_ID=ie.InsCo_ID) 'Ins Co',ie.claimnumber 'Claim Number',pm.policy_no 'Policy No',a.Attorney";
        string condition = " from  tblProceduresDetail tp  inner join tblPatientIE ie on tp.PatientIE_ID = ie.PatientIE_ID inner join tblPatientMaster pm on pm.Patient_ID=ie.Patient_ID left join dbo.tblLocations lc ON ie.Location_ID = lc.Location_ID inner join tblAttorneys a on a.Attorney_ID = ie.Attorney_ID ";
        string condition1 = null;


        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Requested,101),'') as Requested ";

        if (chkRequested.Checked)
            condition1 = " (tp.Requested>='" + txtSearchFromdate.Text + "' and tp.Requested<='" + txtSearchTodate.Text + "')";

        //if (chkRequested.Checked)
        //{
        //query += ", ISNULL(CONVERT(VARCHAR(10),tp.Requested,101),'') as Requested ";
        //condition1 = " (CONVERT(VARCHAR(10),tp.Requested,101) >= CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),tp.createddate,101) <= CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
        //}

        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Scheduled,101),'') as Scheduled ";
        if (chkScheduled.Checked)
        {
            if (!string.IsNullOrEmpty(condition1))
                condition1 += condition1 = " or (tp.Scheduled>='" + txtSearchFromdate.Text + "' and tp.Scheduled<='" + txtSearchTodate.Text + "')";
            else
                condition1 = " (tp.Scheduled>='" + txtSearchFromdate.Text + "' and tp.Scheduled<='" + txtSearchTodate.Text + "')";
        }

        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Executed,101),'') as Executed ";
        if (chkExecuted.Checked)
        {

            if (!string.IsNullOrEmpty(condition1))
                condition1 += condition1 = " or (tp.Executed>='" + txtSearchFromdate.Text + "' and tp.Executed<='" + txtSearchTodate.Text + "')";
            else
                condition1 = " (tp.Executed>='" + txtSearchFromdate.Text + "' and tp.Executed<='" + txtSearchTodate.Text + "')";
        }

        query += " ,tp.ProcedureDetail_ID ,ISnull(pm.Note,'') AS Note ";


        if (ddl_location.SelectedIndex > 0)
        {

            if (!string.IsNullOrEmpty(condition1))
                condition1 += condition1 = " and ie.Location_ID=" + ddl_location.SelectedValue;
            else
                condition1 = " ie.Location_ID=" + ddl_location.SelectedValue;
        }
        

        query += condition;
        if (!string.IsNullOrEmpty(condition1))
        {
            condition1 = condition1.Insert(0, " where ");
            query += condition1;
        }

        query = query + " order by " + ViewState["o_column"] + " "  + ViewState["c_order"];

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
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    if (dt.Columns[i].ColumnName.Equals("Requested"gvProcedureTbl))
                //    {
                //        TemplateField tfield = new TemplateField();
                //        tfield.HeaderText = "Requested";
                //        gvProcedureTbl.Columns.Add(tfield);
                //    }
                //    else if (dt.Columns[i].ColumnName.Equals("ProcedureDetail_ID"))
                //    {
                //        TemplateField tfield = new TemplateField();
                //        tfield.HeaderText = "Update";
                //        gvProcedureTbl.Columns.Add(tfield);
                //    }
                //    else
                //    {
                //        BoundField bfield = new BoundField();
                //        bfield.HeaderText = dt.Columns[i].ColumnName;
                //        bfield.DataField = dt.Columns[i].ColumnName;
                //        gvProcedureTbl.Columns.Add(bfield);
                //    }
                // }

                gvProcedureTbl.DataSource = dt;
                Session["Datatableprocedure"] = dt;
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

    private void bindLocation()
    {
        DataSet ds = new DataSet();
        DBHelperClass db = new DBHelperClass();

        string query = "select Location,Location_ID from tblLocations ";
        if (!string.IsNullOrEmpty(Session["Locations"].ToString()))
        {
            query = query + " where Location_ID in (" + Session["Locations"] + ")";
        }
        query = query + " Order By Location";

        ds = db.selectData(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_location.DataValueField = "Location_ID";
            ddl_location.DataTextField = "Location";

            ddl_location.DataSource = ds;
            ddl_location.DataBind();

            ddl_location.Items.Insert(0, new ListItem("-- All --", "0"));


        }

    }


    protected void lkExportToexcel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Datatableprocedure"];

        dt.Columns.Remove("ProcedureDetail_ID");
        dt.Columns.Remove("ProcedureDetail_ID1");
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "POCReport");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=POCReport.xlsx");
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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearchFromdate.Text = string.Empty;
        txtSearchTodate.Text = string.Empty;

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindProcudureList();
    }

    protected void gvProcedureTbl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (chkRequested.Checked)
        //        e.Row.Cells[14].Visible = true;

        //    if (chkScheduled.Checked) e.Row.Cells[15].Visible = true;
        //    if (chkExecuted.Checked) e.Row.Cells[16].Visible = true;

        //}
    }
    //protected void lnkupdaterecord_Click(object sender, EventArgs e)
    //{
    //    foreach (GridViewRow row in gvProcedureTbl.Rows)
    //    {
    //        string ProcedureDetail_ID = row.Cells[15].Text;
    //        string name = row.Cells[14].Text;
    //        string query = "update tblProceduresDetail set Requested = " + Convert.ToDateTime(name) + "  where ProcedureDetail_ID = " + ProcedureDetail_ID;
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
    //        {
    //            SqlCommand cm = new SqlCommand(query, con);
    //            con.Open();
    //            cm.ExecuteNonQuery();
    //            con.Close();
    //        }
    //    }
    //}
    protected void UpdateDetails(object sender, EventArgs e)
    {
        Button lnkUpdate = (sender as Button);
        GridViewRow row = (lnkUpdate.NamingContainer as GridViewRow);
        string ProcedureDetail_ID = lnkUpdate.CommandArgument;
        string name = row.Cells[0].Text;
        string reqdate = (row.FindControl("txtRequest") as TextBox).Text;
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Id: " + ProcedureDetail_ID + " Name: " + name + " reqdate: " + reqdate + "')", true);
        //string reqdate = row.Cells[14].Text;
        //string query = "update tblProceduresDetail set Requested = " + Convert.ToDateTime(name) + "  where ProcedureDetail_ID = " + ProcedureDetail_ID;
        //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        //{
        //    SqlCommand cm = new SqlCommand(query, con);
        //    con.Open();
        //    cm.ExecuteNonQuery();
        //    con.Close();
        //}
    }

    protected void lnkTransfer_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow grow in gvProcedureTbl.Rows)
        {
            //Searching CheckBox("chkDel") in an individual row of Grid  
            CheckBox chkExe = (CheckBox)grow.FindControl("chkExe");
            //If CheckBox is checked than delete the record with particular empid  
            if (chkExe.Checked)
            {
                HiddenField hid = grow.FindControl("hID") as HiddenField;
                HiddenField sDate = grow.FindControl("sDate") as HiddenField;
                TransferToExecute(hid.Value, sDate.Value);
            }
        }
        BindProcudureList();
    }

    public void TransferToExecute(string id, string sDate)
    {
        if (!string.IsNullOrEmpty(sDate))
        {
            DBHelperClass dBHelper = new DBHelperClass();
            string query = "update tblProceduresDetail set Executed='" + sDate + "',Scheduled=null where ProcedureDetail_ID=" + id;
            dBHelper.executeQuery(query);
        }

    }

    protected void btnReshedule_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        Reschedules(btn.CommandArgument);
    }
       
    protected void lnkRescheduled_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow grow in gvProcedureTbl.Rows)
        {
            //Searching CheckBox("chkDel") in an individual row of Grid  
            CheckBox chkExe = (CheckBox)grow.FindControl("chkExe");
            //If CheckBox is checked than delete the record with particular empid  
            if (chkExe.Checked)
            {
                HiddenField hid = grow.FindControl("hID") as HiddenField;

                Reschedules(hid.Value);
            }
        }
        BindProcudureList();
    }

    public void Reschedules(string id)
    {

        DBHelperClass dBHelper = new DBHelperClass();
        string query = "update tblProceduresDetail set Scheduled=null where ProcedureDetail_ID=" + id;
        dBHelper.executeQuery(query);
        BindProcudureList();

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


}