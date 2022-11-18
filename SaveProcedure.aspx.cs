using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SaveProcedure : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Response.Redirect("~/Login.aspx");
        }
    }

    [WebMethod]
    public static string Save(string procedureId, string patientIEId, string patientFUId, string createdBy, string mcode, string bodypart, string date, string pDesc, string ProcedureDetail_ID)
    {
        BusinessLogic _bl = new BusinessLogic();
        //string datestring = date.Split('/')[1] + "/" + date.Split('/')[0] + "/" + date.Split('/')[2];// for india
        string datestring = date;
        string count = _bl.saveProcedureDetail(Convert.ToInt64(procedureId), Convert.ToInt64(patientIEId), Convert.ToInt64(patientFUId), mcode, bodypart, DateTime.ParseExact(datestring, "MM/dd/yyyy", null), pDesc, Convert.ToInt64(createdBy),Convert.ToInt64(ProcedureDetail_ID));
        return count;

    }

    [WebMethod]
    public static string Delete(string procedureId, string patientIEId, string patientFUId, string mcode, string bodypart, string ProcedureDetail_ID)
    {
        BusinessLogic _bl = new BusinessLogic();
        //string datestring = date.Split('/')[1] + "/" + date.Split('/')[0] + "/" + date.Split('/')[2];// for india
        string count = _bl.deleteProcedureDetail(Convert.ToInt64(procedureId), Convert.ToInt64(patientIEId), Convert.ToInt64(patientFUId), mcode, bodypart, Convert.ToInt64(ProcedureDetail_ID));
        return count;
    }
}