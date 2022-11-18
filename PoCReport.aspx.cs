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




public partial class PoCReport : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
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
        //string query = "select tp.ProcedureDetail_ID,pm.sex,pm.LastName+', '+pm.FirstName 'Name',ie.Compensation as 'Case',ISNULL(CONVERT(VARCHAR(10),pm.DOB,101),'') as DOB ,ISNULL(CONVERT(VARCHAR(10),ie.DOA,101),'') as DOA ,tp.MCODE,ISnull(pm.MC,'') AS MC,pm.phone+','+pm.Phone2 as Phone,lc.location,pm.policy_no,ie.ClaimNumber,(Select ins.InsCo from tblInsCos ins where ins.InsCo_ID= ie.InsCo_ID) 'Insurance'";
        //query += ",stuff((SELECT ', ' + cast(a.mcode as varchar(max)) FROM tblConsider c INNER JOIN tblprocedures a on C.procedureDetailID=a.Procedure_ID and c.PatientIE_ID = tp.patientie_id FOR XML PATH('')),1,1,'')  'Consider',case when Vaccinated = 1 then 'YES' when Vaccinated = 0 then 'NO' else 'NO' end as Vaccinated " ;
        //string condition = " from  tblProceduresDetail tp inner join tblProcedures pp on pp.MCODE = tp.MCODE and tp.BodyPart= pp.BodyPart inner join tblPatientIE ie on tp.PatientIE_ID = ie.PatientIE_ID inner join tblPatientMaster pm on pm.Patient_ID=ie.Patient_ID left join dbo.tblLocations lc ON ie.Location_ID = lc.Location_ID";


        string query = "select tp.ProcedureDetail_ID,pm.sex,pm.LastName+', '+pm.FirstName 'Name',ie.Compensation as 'Case',ie.MA_Providers as ProviderName,";
        query += "ISNULL((select top 1 ISNULL(CONVERT(VARCHAR(10),A.DOE,101),'') from tblFUPatient A where A.PatientIE_ID=ie.PatientIE_ID order by A.DOE DESC),ISNULL(CONVERT(VARCHAR(10),ie.DOE,101),'')) DOE,";
        query += "ISNULL(CONVERT(VARCHAR(10),pm.DOB,101),'') as DOB ,ISNULL(CONVERT(VARCHAR(10),ie.DOA,101),'') as DOA ,tp.MCODE,ISnull(pm.MC,'') AS MC,pm.phone+','+pm.Phone2 as Phone,lc.location,pm.policy_no,ie.ClaimNumber,(Select ins.InsCo from tblInsCos ins where ins.InsCo_ID= ie.InsCo_ID) 'Insurance'";
        query += ",stuff((SELECT ', ' + cast(a.mcode as varchar(max)) FROM tblConsider c INNER JOIN tblprocedures a on C.procedureDetailID=a.Procedure_ID and c.PatientIE_ID = tp.patientie_id FOR XML PATH('')),1,1,'')  'Consider',case when Vaccinated = 1 then 'YES' when Vaccinated = 0 then 'NO' else 'NO' end as Vaccinated ";
        string condition = " from  tblProceduresDetail tp inner join tblProcedures pp on pp.MCODE = tp.MCODE and tp.BodyPart= pp.BodyPart inner join tblPatientIE ie on tp.PatientIE_ID = ie.PatientIE_ID  inner join tblPatientMaster pm on pm.Patient_ID=ie.Patient_ID left join dbo.tblLocations lc ON ie.Location_ID = lc.Location_ID";

        string condition1 = null;
        string condition2 = null;
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Requested,101),'') as Requested ";

        if (chkRequested.Checked)
        {
            // query += ", ISNULL(CONVERT(VARCHAR(10),tp.Requested,101),'') as Requested ";
            condition1 = " (tp.Requested BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
        }
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Scheduled,101),'') as Scheduled ";
        if (chkScheduled.Checked)
        {
            //query += ", ISNULL(CONVERT(VARCHAR(10),tp.Scheduled,101),'') as Scheduled ";
            if (!string.IsNullOrEmpty(condition1))
            {
                condition1 += condition1 = " OR ( tp.Scheduled BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }
            else
            {

                condition1 = " (tp.Scheduled BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }

        }
        query += ", ISNULL(CONVERT(VARCHAR(10),tp.Executed,101),'') as Executed ";
        if (chkExecuted.Checked)
        {
            //query += ", ISNULL(CONVERT(VARCHAR(10),tp.Executed,101),'') as Executed ";
            if (!string.IsNullOrEmpty(condition1))
            {
                condition1 += condition1 = " OR (tp.Executed BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }
            else
            {

                condition1 = " (tp.Executed BETWEEN CONVERT(VARCHAR(10),'" + txtSearchFromdate.Text + "',101) and CONVERT(VARCHAR(10),'" + txtSearchTodate.Text + "',101))";
            }
        }
        // query += " , ISnull(pm.Note,'') AS Note ";
        query += " ,ISnull(pm.Note,'') AS Note ";
        query += condition;


        if (!string.IsNullOrEmpty(condition1))
        {
            condition1 = condition1.Insert(0, " where ");
            query += condition1;
        }

        if (Convert.ToInt32(ddlLocation.SelectedValue) > 0)
        {
            query += " and lc.Location_ID = isnull(" + ddlLocation.SelectedValue + ",lc.Location_ID)";
        }

        if (ddlmcodetype.SelectedValue.Equals("1"))
        {
            query += " and pp.INhouseProcbit = 1 ";
        }
        else if (ddlmcodetype.SelectedValue.Equals("2"))
        {
            query += " and pp.Other =1 ";
        }
        else if (ddlmcodetype.SelectedValue.Equals("3"))
        {
            query += " and pp.INhouseProcbit <> 1 and  ISNULL(pp.Other,0) <> 1  ";
        }
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

        dt.Columns.Remove("ProcedureDetail_ID");

        SettingModel settingModal = db.getSettingValues();

        if (settingModal.displaySex == false)
            dt.Columns.Remove("Sex");

        if (settingModal.displayFName == false)
            dt.Columns.Remove("FirstName");

        if (settingModal.displayLName == false)
            dt.Columns.Remove("LastName");

        if (settingModal.displayNote == false)
            dt.Columns.Remove("Note");

        if (settingModal.displayCase == false)
            dt.Columns.Remove("Case");

        if (settingModal.displayDOB == false)
            dt.Columns.Remove("DOB");

        if (settingModal.displayDOA == false)
            dt.Columns.Remove("DOA");

        if (settingModal.displayMCode == false)
            dt.Columns.Remove("MCode");

        if (settingModal.displayPhone == false)
            dt.Columns.Remove("Phone");

        if (settingModal.displayLocation == false)
            dt.Columns.Remove("Location");

        if (settingModal.displayPolicy_No == false)
            dt.Columns.Remove("Policy No");

        if (settingModal.displayClaim_No == false)
            dt.Columns.Remove("Claim Number");

        if (settingModal.displayInsurance == false)
            dt.Columns.Remove("Insurance");

        if (settingModal.displayMC == false)
            dt.Columns.Remove("MC");

        //if (settingModal.displayConsider == false)
        //    dt.Columns.Remove("Consider");

        if (settingModal.displayVaccinated == false)
            dt.Columns.Remove("Vaccinated");

        if (settingModal.displayRequested == false)
            dt.Columns.Remove("Requested");

        if (settingModal.displayScheduled == false)
            dt.Columns.Remove("Scheduled");

        if (settingModal.displayExecuted == false)
            dt.Columns.Remove("Executed");

        if (settingModal.displayAttorny == false)
            dt.Columns.Remove("Attorney");

        if (settingModal.displayDOE == false)
            dt.Columns.Remove("DOE");

        if (settingModal.displayProvider == false)
            dt.Columns.Remove("ProviderName");

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


    private void bindLocation()
    {
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

}