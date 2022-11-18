using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class test : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        string mcode = Request.QueryString[0];
        Session["IN"] = 1;
        if (mcode != null)
        {
            if (!IsPostBack)
            {
                bind();
            }
        }
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditSignInSheet for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrWhiteSpace(txtDate1.Text))
        {
            txtDate1.Text = null;
        }

        db.Insertupdate(Session["pid"].ToString(), Session["PatientIE_ID"].ToString(), Session["MCODE"].ToString(), txtDate1.Text, txtDate2.Text, txtDate3.Text, Session["BodyPart"].ToString());
        db.logDetailtbl(Convert.ToInt32(HttpContext.Current.Session["log_id"].ToString()), "Data Updated", Convert.ToString(System.DateTime.Now));
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "none", "<script>close_window();</script>", false);
       
    }
    protected void bind()
    {
        Session["MCODE"] = Request.QueryString["MCODE"].ToString();
        Session["pid"] = Request.QueryString["id"].ToString();
        string pid = "'" + Session["pid"].ToString() + "'";

        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        ds = db.selectData("select * from tblProceduresDetail where ProcedureDetail_ID = " + pid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["PatientIE_ID"] = ds.Tables[0].Rows[0]["PatientIE_ID"].ToString();
            Session["BodyPart"] = ds.Tables[0].Rows[0]["BodyPart"].ToString();
            if (ds.Tables[0].Rows[0]["MCODE"].ToString() == null)
            {
                txtmcode.Text = string.Empty;
            }
            else
            {
                txtmcode.Text = ds.Tables[0].Rows[0]["MCODE"].ToString();
            }
            if (ds.Tables[0].Rows[0]["Requested"].ToString() == string.Empty)
            {
                txtDate1.Text = string.Empty;
            }
            else
            {
                txtDate1.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Requested"].ToString()).ToString("MM/dd/yyyy");
            }

            if (ds.Tables[0].Rows[0]["Scheduled"].ToString() == string.Empty)
            {

                txtDate2.Text = string.Empty;
            }
            else
            {
                txtDate2.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Scheduled"].ToString()).ToString("MM/dd/yyyy");
            }
            if (ds.Tables[0].Rows[0]["Executed"].ToString() == string.Empty)
            {
                txtDate3.Text = string.Empty;
            }
            else
            {
                txtDate3.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Executed"].ToString()).ToString("MM/dd/yyyy");
            }
        }
        else
        {
            ds2 = db.selectData("select * from tblProcedures where MCODE='" + Session["MCODE"].ToString() + "'");
            Session["PatientIE_ID"] = Session["pid"].ToString();
            Session["pid"] = ds2.Tables[0].Rows[0]["Procedure_ID"].ToString();
            Session["BodyPart"] = ds2.Tables[0].Rows[0]["BodyPart"].ToString();
            txtmcode.Text = Session["MCODE"].ToString();
            txtDate1.Text = string.Empty;
            txtDate2.Text = string.Empty;
            txtDate3.Text = string.Empty;
        }
    }
}