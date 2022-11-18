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
public partial class AddFuDiagnosis : System.Web.UI.Page
{
    public string _CurBodyPart = "";
    public string _CurIEid = "";
    public string _CurFUid = "";
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private string _SKey = "";
    List<string> sTextlist = new List<string>();
    List<string> sColor = new List<string>();

    DBHelperClass db = new DBHelperClass();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["PageName"] != null)
            {
                if (Session["patientFUId"] != null && Session["patientFUId"] != "" && Session["PatientIE_Id"] != null && Session["PatientIE_Id"] != "")
                {
                    _CurBodyPart = Session["PageName"].ToString();
                    _CurIEid = Session["PatientIE_Id"].ToString();
                    _CurFUid = Session["patientFUId"].ToString();
                    _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
                    bindgrid();
                }
            }

        }

        Logger.Info(Session["uname"].ToString() + "- Visited in AddFuDiagnosis for -" + Convert.ToString(Session["LastNameFU"]) + Convert.ToString(Session["FirstNameFU"]) + "-" + DateTime.Now);
    }
    private void bindgrid()
    {
        DataSet ds = new DataSet();
        DataTable Standards = new DataTable();
        string SqlStr = "";
        if (_CurIEid != "")
            SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS(" + _CurIEid + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
        else
            SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
        ds = db.selectData(SqlStr);

        dgvDiagCodes.DataSource = ds;
        dgvDiagCodes.DataBind();

    }

    protected void btnFind_Click(object sender, ImageClickEventArgs e)
    {
        // _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + cboBodyPart.Text.Trim() + "%'";
        bindgrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        SaveStandards(Session["PatientIE_ID"].ToString());
        string url = "";
        string s = " window.opener.location.reload(); window.close('" + url + "', 'popup_window', 'width=900,height=500,left=100,top=100,resizable=yes');";
        ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
    }
    public string SaveStandards(string ieID)
    {
        List<Adddiagnosis> objList = new List<Adddiagnosis>();
        Adddiagnosis obj = new Adddiagnosis();
        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvDiagCodes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    obj.DiagCode_ID = dgvDiagCodes.DataKeys[row.RowIndex].Value.ToString();
                    obj.BodyPart = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.DiagCode = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Description = row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.PN = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        ids += obj.DiagCode_ID + ",";
                        SaveStdUI(ieID, _CurFUid, obj.DiagCode_ID, true, obj.BodyPart, obj.Description, obj.DiagCode);
                    }
                    else
                    { SaveStdUI(ieID, _CurFUid, obj.DiagCode_ID, false, obj.BodyPart, obj.Description, obj.DiagCode); }
                    objList.Add(obj);
                }
            }
            Session["DiagnosisList"] = objList;
        }
        catch (Exception ex)
        {

        }
        return "";
    }
    public void SaveStdUI(string ieID, string fuID, string iDiagID, bool DiagIsChecked, string bp, string dcd, string dc)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _DiagID = Convert.ToInt64(iDiagID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID + "AND PatientFu_ID=" + Session["patientFUId"].ToString();
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && DiagIsChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && DiagIsChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && DiagIsChecked == false)
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
            TblRow["Diag_Master_ID"] = _DiagID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["PatientFu_ID"] = Session["patientFUId"].ToString();
            TblRow["BodyPart"] = bp.ToString().Trim();
            TblRow["DiagCode"] = dc.ToString().Trim();
            TblRow["Description"] = dcd.ToString().Trim();

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