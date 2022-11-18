using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddStandards : System.Web.UI.Page
{
    //f
    public string _CurBodyPart = "";
    string _CurMode = "";
    string[] _Params;
    public string _CurIEid = "";
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private string _SKey = "";
    private bool _gridSel = true;
    private bool _fldPop = false;
    List<string> sTextlist = new List<string>();
    List<string> sColor = new List<string>();

    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["PageName"] != null)
            {
                _CurBodyPart = Session["PageName"].ToString();
                _CurIEid = Session["PatientIE_ID"].ToString();
                _SKey = "WHERE tblMacrosMaster.Heading LIKE '%" + txHeading.Text.Trim() + "%' AND BodayParts LIKE '%" + _CurBodyPart + "%'";
                bindgrid();
            }
        }
    }
   
    private void bindgrid()
    {
        DataSet ds = new DataSet();
        DataTable Standards = new DataTable();
        string SqlStr = "";
        if (!string.IsNullOrEmpty(_CurIEid))
            SqlStr = "Select tblMacrosMaster.*, dbo.STDEXISTS(" + _CurIEid + ", Macro_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblMacrosMaster " + _SKey + " Order By BodayParts,Heading";
        else
            SqlStr = "Select tblMacrosMaster.*, dbo.STDEXISTS('0', Macro_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblMacrosMaster " + _SKey + " Order By BodayParts,Heading";

        ds = db.selectData(SqlStr);

        dgvStandards.DataSource = ds;
        dgvStandards.DataBind();

    }

    protected void btnFind_Click(object sender, ImageClickEventArgs e)
    {
        _SKey = "WHERE tblProcedures.Heading LIKE '%" + txHeading.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
        bindgrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveStandards(Session["PatientIE_ID"].ToString());
        string url = "";
        string s = "window.opener.location.reload(); window.close('" + url + "', 'popup_window', 'width=900,height=500,left=100,top=100,resizable=yes');";
        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
    }
    public string SaveStandards(string ieID)
    {
        List<AddStandards_Pro> objList = new List<AddStandards_Pro>();
        AddStandards_Pro obj = new AddStandards_Pro();

        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvStandards.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    obj.Procedure_ID = dgvStandards.DataKeys[row.RowIndex].Value.ToString();
                    obj.BodyPart = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Heading = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.CCDesc = row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.PEDesc = row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.ADesc = row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.PDesc = row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.CF = row.Cells[7].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.PN = row.Cells[8].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        SaveStdUI(ieID, obj.Procedure_ID, true, obj.BodyPart, obj.Heading, obj.CCDesc, obj.PEDesc, obj.ADesc, obj.PDesc, Convert.ToBoolean(obj.CF), Convert.ToBoolean(obj.PN));
                    }
                    else
                    { SaveStdUI(ieID, obj.Procedure_ID, false, obj.BodyPart, obj.Heading, obj.CCDesc, obj.PEDesc, obj.ADesc, obj.PDesc, Convert.ToBoolean(obj.CF), Convert.ToBoolean(obj.PN)); }
                }
                
            }
            Session["StandardsList"] = objList;
        }
        catch (Exception ex)
        {

        }
        return "";
    }
    public void SaveStdUI(string ieID, string iStdID, bool StdIsChecked, string bp, string shd, string scc, string spe, string sa, string sp, bool bcf, bool bpn)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _StdID = Convert.ToInt64(iStdID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblMacros WHERE PatientIE_ID = " + ieID + " AND Macro_Master_ID = " + _StdID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && StdIsChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && StdIsChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && StdIsChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["Macro_Master_ID"] = _StdID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["BodayParts"] = bp.ToString().Trim();
            TblRow["Heading"] = shd.ToString().Trim();
            TblRow["CCDesc"] = scc.ToString().Trim();
            TblRow["PEDesc"] = spe.ToString().Trim();
            TblRow["ADesc"] = sa.ToString().Trim();
            TblRow["PDesc"] = sp.ToString().Trim();
            TblRow["CF"] = bcf;
            TblRow["PN"] = bpn;

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }
}