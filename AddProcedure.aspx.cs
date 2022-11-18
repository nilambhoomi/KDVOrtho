using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddProcedure : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["id"] != null)
                bindData();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("nusp_SaveprocedureDetailsnew", con);
                cmd.CommandType = CommandType.StoredProcedure;


                if (Request["id"] != null)
                {
                    cmd.Parameters.AddWithValue("@procedure_ID", Convert.ToInt32(Request["id"]));
                }
                cmd.Parameters.AddWithValue("@BodyPart", txtBodyParts.Text.Trim());
                cmd.Parameters.AddWithValue("@MCODE", txtMCODE.Text.Trim());
                cmd.Parameters.AddWithValue("@Heading", txtHeading.Text.Trim());
                cmd.Parameters.AddWithValue("@CCDesc", txtCCDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@PEDesc", txtPEDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@ADesc", txtADesc.Text.Trim());
                cmd.Parameters.AddWithValue("@PDesc", txtPDesc.Text.Trim());

                cmd.Parameters.AddWithValue("@S_Heading", txtHeadingS.Text.Trim());
                cmd.Parameters.AddWithValue("@E_Heading", txtHeadingE.Text.Trim());

                cmd.Parameters.AddWithValue("@S_CCDesc", txtS_CCDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@S_PEDesc", txtS_PEDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@S_ADesc", txtS_ADesc.Text.Trim());
                cmd.Parameters.AddWithValue("@S_PDesc", txtS_PDesc.Text.Trim());

                cmd.Parameters.AddWithValue("@E_CCDesc", txtE_CCDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@E_PEDesc", txtE_PEDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@E_ADesc", txtE_ADesc.Text.Trim());
                cmd.Parameters.AddWithValue("@E_PDesc", txtE_PDesc.Text.Trim());


                cmd.Parameters.AddWithValue("@Display_Order", txtDisplay_Order.Text.Trim());
                if (ddlposition.SelectedValue.Equals("-1"))
                { cmd.Parameters.AddWithValue("@Position", ""); }
                else
                { cmd.Parameters.AddWithValue("@Position", ddlposition.SelectedValue); }

                cmd.Parameters.AddWithValue("@Muscle", txtMuscles.Text.Trim());
                cmd.Parameters.AddWithValue("@Medication", txtMedication.Text.Trim());
                cmd.Parameters.AddWithValue("@SubProcedure", txtSubProcedure.Text.Trim());

                cmd.Parameters.AddWithValue("@INhouseProcbit", chkINhouseProcbit.Checked);
                cmd.Parameters.AddWithValue("@HasLevel", chkHasLevel.Checked);
                //cmd.Parameters.AddWithValue("@INout", chkInOut.Checked);
                cmd.Parameters.AddWithValue("@CF", chkCF.Checked);
                cmd.Parameters.AddWithValue("@PN", chkPN.Checked);
                cmd.Parameters.AddWithValue("@sides", chkSides.Checked);
                cmd.Parameters.AddWithValue("@LevelsDefault", txtLevelsDefault.Text);
                cmd.Parameters.AddWithValue("@SidesDefault", ddlSidesDefault.SelectedValue);
                con.Open();
                int count = cmd.ExecuteNonQuery();
                con.Close();
                Response.Redirect("ProcedureList.aspx");
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void bindData()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("nusp_getProcedurenew", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BodyPart", "");
            cmd.Parameters.AddWithValue("@Mcode", "");
            cmd.Parameters.AddWithValue("@Heading", "");
            con.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            con.Close();
            DataView dv = new DataView(dt);
            dv.Sort = "Display_Order";


            dv.RowFilter = "Procedure_ID=" + Request["id"];
            DataTable dt1 = dv.ToTable();
            txtBodyParts.Text = Convert.ToString(dt1.Rows[0]["BodyPart"]);
            txtMCODE.Text = Convert.ToString(dt1.Rows[0]["MCODE"]);
            txtHeading.Text = Convert.ToString(dt1.Rows[0]["Heading"]);
            txtCCDesc.Text = Convert.ToString(dt1.Rows[0]["CCDesc"]);
            txtPEDesc.Text = Convert.ToString(dt1.Rows[0]["PEDesc"]);
            txtADesc.Text = Convert.ToString(dt1.Rows[0]["ADesc"]);
            txtPDesc.Text = Convert.ToString(dt1.Rows[0]["PDesc"]);

            txtHeadingS.Text = Convert.ToString(dt1.Rows[0]["S_Heading"]);
            txtHeadingE.Text = Convert.ToString(dt1.Rows[0]["E_Heading"]);

            txtS_CCDesc.Text = Convert.ToString(dt1.Rows[0]["S_CCDesc"]);
            txtS_PEDesc.Text = Convert.ToString(dt1.Rows[0]["S_PEDesc"]);
            txtS_ADesc.Text = Convert.ToString(dt1.Rows[0]["S_ADesc"]);
            txtS_PDesc.Text = Convert.ToString(dt1.Rows[0]["S_PDesc"]);

            txtE_CCDesc.Text = Convert.ToString(dt1.Rows[0]["E_CCDesc"]);
            txtE_PEDesc.Text = Convert.ToString(dt1.Rows[0]["E_PEDesc"]);
            txtE_ADesc.Text = Convert.ToString(dt1.Rows[0]["E_ADesc"]);
            txtE_PDesc.Text = Convert.ToString(dt1.Rows[0]["E_PDesc"]);


            txtDisplay_Order.Text = Convert.ToString(dt1.Rows[0]["Display_Order"]);
            txtMuscles.Text = Convert.ToString(dt1.Rows[0]["HasMuscle"]);
            txtMedication.Text = Convert.ToString(dt1.Rows[0]["HasMedication"]);
            txtSubProcedure.Text = Convert.ToString(dt1.Rows[0]["HasSubProcedure"]);
            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["INhouseProcbit"])))
            {
                chkINhouseProcbit.Checked = Convert.ToBoolean(dt1.Rows[0]["INhouseProcbit"]);
            }
            else { chkINhouseProcbit.Checked = false; }
            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["HasLevel"])))
            {
                chkHasLevel.Checked = Convert.ToBoolean(dt1.Rows[0]["HasLevel"]);
            }
            else { chkHasLevel.Checked = false; }
            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["INhouseProcbit"])))
            {
                chkCF.Checked = Convert.ToBoolean(dt1.Rows[0]["CF"]);
            }
            else { chkCF.Checked = false; }
            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["PN"])))
            {
                chkPN.Checked = Convert.ToBoolean(dt1.Rows[0]["PN"]);
            }
            else { chkPN.Checked = false; }
            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["sides"])))
            {
                chkSides.Checked = Convert.ToBoolean(dt1.Rows[0]["sides"]);
            }
            else { chkSides.Checked = false; }
            //chkInOut.Checked = Convert.ToBoolean(dt1.Rows[0]["INout"]);
            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["Position"])))
            {
                ddlposition.ClearSelection();
                ddlposition.Items.FindByValue(Convert.ToString(dt1.Rows[0]["Position"])).Selected = true;
            }
            else { ddlposition.SelectedValue = "-1"; }

            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["LevelsDefault"])))
            {
                txtLevelsDefault.Text = Convert.ToString(dt1.Rows[0]["LevelsDefault"]);
            }


            if (!string.IsNullOrEmpty(Convert.ToString(dt1.Rows[0]["SidesDefault"])))
            {
                ddlSidesDefault.SelectedValue = Convert.ToString(dt1.Rows[0]["SidesDefault"]);
            }
            else { ddlSidesDefault.SelectedValue = Convert.ToString(dt1.Rows[0]["SidesDefault"]); }
        }
    }


}