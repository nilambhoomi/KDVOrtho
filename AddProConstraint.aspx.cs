using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddProConstraint : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(AddProConstraint));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindBodyPart();
            bindMCODE("");
           
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        DBHelperClass db = new DBHelperClass();
        try
        {
            string query = "", strmcode = "", position = "";


            for (int i = 0; i < chkProcExe.Items.Count; i++)
            {
                if (chkProcExe.Items[i].Selected)
                {
                    strmcode = strmcode + "," + chkProcExe.Items[i].Text;

                }
            }

            if (ddlbodypart.SelectedValue.ToLower() != "neck" || ddlbodypart.SelectedValue.ToLower() != "midback" || ddlbodypart.SelectedValue.ToLower() != "lowback")
                position = ddlposition.SelectedValue;

                if (Request["id"] == null)
            {
                query = "insert into tblProcedureStatus values('" + ddlProcSchedule.SelectedValue + "','" + strmcode.TrimStart(',') + "','" + ddlbodypart.SelectedValue + "','" + position + "')";
            }
            else
            {
                query = " update tblProcedureStatus set Procedures='" + ddlProcSchedule.SelectedValue + "',PreRequest='" + strmcode.TrimStart(',') + "',BodyPart='" + ddlbodypart.SelectedValue + "',Position='" + ddlposition.SelectedValue + "' where id=" + Request["id"];
            }

            int val = db.executeQuery(query);
            if (val > 0)
                Response.Redirect("ViewProcConstraint.aspx");
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }



    private void bindBodyPart()
    {
        try
        {
            DataTable ds = new DBHelperClass().GetallBodyparts();

            ddlbodypart.DataSource = ds;
            ddlbodypart.DataBind();

            ddlbodypart.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        catch (Exception ex)
        {

        }
    }

    private void bindMCODE(string bpart, string position = "")
    {
        try
        {
            string query = "select * from tblProcedures";
            if (!string.IsNullOrEmpty(bpart) && !string.IsNullOrEmpty(position))
                query = query + " where BodyPart='" + bpart + "' and position='" + position + "' ";
            else if (!string.IsNullOrEmpty(bpart))
                query = query + " where BodyPart='" + bpart + "' ";

            DataSet ds = new DBHelperClass().selectData(query);

            ddlProcSchedule.DataSource = ds;
            ddlProcSchedule.DataBind();

            ddlProcSchedule.Items.Insert(0, new ListItem("Please Select", "0"));

            chkProcExe.DataSource = ds;
            chkProcExe.DataBind();

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbodypart_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlbodypart.SelectedValue.ToLower() == "neck" || ddlbodypart.SelectedValue.ToLower() == "midback" || ddlbodypart.SelectedValue.ToLower() == "lowback")
        {
            ddlposition.Enabled = false;
            bindMCODE(ddlbodypart.SelectedValue);
        }
        else
        {
            ddlposition.Enabled = true;
            bindMCODE(ddlbodypart.SelectedValue, ddlposition.SelectedValue);
        }
        //if (ddlbodypart.SelectedValue != "0")
        //    bindMCODE(ddlbodypart.SelectedValue);
        //else
        //    bindMCODE("");
    }

    protected void ddlposition_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindMCODE(ddlbodypart.SelectedValue, ddlposition.SelectedValue);
    }



}