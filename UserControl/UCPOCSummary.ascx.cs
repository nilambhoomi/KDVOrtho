using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControl_UCPOCSummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
            bindPOC();
    }

    public void bindPOC()
    {
        try
        {
            DBHelperClass db = new DBHelperClass();


            string SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc,
 CASE 
                              WHEN p.Requested is not null 
                               THEN p.Requested
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.Scheduled
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.Executed
                              END  END END as PDate,
CASE when BodypartSide is not null and BodypartSide<>''
then BodypartSide
else BodyPart
end as BodyPart
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + Session["PatientIE_ID"].ToString() + "  and IsConsidered=0  Order By BodyPart,Heading"; ;


            DataSet dsPOC = db.selectData(SqlStr);


            if (dsPOC != null && dsPOC.Tables[0].Rows.Count > 0)
            {
                repSummery.DataSource = dsPOC;
                repSummery.DataBind();
            }
            else
            {
                repSummery.DataSource = null;
                repSummery.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
}