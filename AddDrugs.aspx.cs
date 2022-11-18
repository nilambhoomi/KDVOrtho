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
using System.Xml;
using System.IO;
using log4net;

public partial class AddDrugs : System.Web.UI.Page
{
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

    ILog log = log4net.LogManager.GetLogger(typeof(AddDrugs));

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["PageName"] != null)
            {
                _CurBodyPart = Session["PageName"].ToString();
                _CurIEid = Session["PatientIE_ID"].ToString();
                _SKey = "WHERE tblMedicines.Medicine LIKE '%" + txDesc.Text.Trim() + "%'";
                bindgrid();
            }
        }
    }

    private void bindgrid()
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";
            if (!string.IsNullOrEmpty(_CurIEid))
                SqlStr = "Select tblMedicines.*, dbo.MEDEXISTS(" + _CurIEid + ", Medicine_ID) as IsChkd FROM tblMedicines " + _SKey + " Order By Medicine";
            else
                SqlStr = "Select tblMedicines.*, dbo.MEDEXISTS('0', Medicine_ID) as IsChkd FROM tblMedicines " + _SKey + " Order By Medicine";
            ds = db.selectData(SqlStr);
            dgvMedi.DataSource = ds;
            dgvMedi.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    protected void btnFind_Click(object sender, ImageClickEventArgs e)
    {
        _SKey = "WHERE tblMedicines.Medicine LIKE '%" + txDesc.Text.Trim() + "%'";
        bindgrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveStandards(Session["PatientIE_ID"].ToString());
            string url = "";
            string s = "window.opener.location.reload(); window.close('" + url + "', 'popup_window', 'width=900,height=500,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }
    public string SaveStandards(string ieID)
    {
        List<AddDrug> objList = new List<AddDrug>();
        AddDrug obj = new AddDrug();

        string ids = string.Empty;
        try
        {
            RemoveDiagCodesDetail(Session["PatientIE_ID"].ToString());
            foreach (GridViewRow row in dgvMedi.Rows)
            {


                if (row.RowType == DataControlRowType.DataRow)
                {

                    obj.Medi_ID = dgvMedi.DataKeys[row.RowIndex].Value.ToString();
                    obj.Medical = row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        SaveMediUI(ieID, obj.Medi_ID, obj.isChecked, obj.Medical);
                    }
                    else
                    { SaveMediUI(ieID, obj.Medi_ID, obj.isChecked, obj.Medical); }
                }

            }
            Session["MedicalList"] = objList;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }
    public void SaveMediUI(string ieID, string iMediID, bool MediIsChecked, string medi)
    {
        try
        {
            string _ieMode = "";
            long _ieID = Convert.ToInt64(ieID);
            long _MediID = Convert.ToInt64(iMediID);
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * FROM tblMedicationRx WHERE PatientIE_ID = " + ieID + " AND MedicationID = " + _MediID;
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count == 0 && MediIsChecked == true)
                _ieMode = "New";
            else if (sqlTbl.Rows.Count == 0 && MediIsChecked == false)
                _ieMode = "None";
            else if (sqlTbl.Rows.Count > 0 && MediIsChecked == false)
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
                TblRow["MedicationID"] = _MediID;
                TblRow["PatientIE_ID"] = _ieID;
                TblRow["Medicine"] = medi.ToString().Trim();

                if (_ieMode == "New")
                {
                    TblRow["UpdatedBy"] = "Admin";
                    TblRow["UpdatedDateTime"] = DateTime.Now;
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
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }

    protected void RemoveDiagCodesDetail(string PatientIE_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblMedicationRx WHERE PatientIE_ID=" + PatientIE_ID;
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }
}