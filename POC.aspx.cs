using IntakeSheet.BLL;
using IntakeSheet.DAL;
using IntakeSheet.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.Services;
using System.Collections;
using System.Web.Script.Serialization;

public partial class POC : System.Web.UI.Page
{
    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (Session["PatientIE_ID"] == null)
        {
            Session["bodyPartsList"] = null;
            Response.Redirect("Page1.aspx");

        }
        if (!IsPostBack)
        {
            bindPOC();
            if (Request.QueryString["PId"] != null)
            {
                int patientIEID = Convert.ToInt32(Request.QueryString["PId"]);
                hfPatientIE_ID.Value = patientIEID.ToString();
                PrintProcedures(patientIEID);
            }
            else if (Session["PatientIE_ID"] != null)
            {
                int patientIEID = Convert.ToInt32(Session["PatientIE_ID"]);
                hfPatientIE_ID.Value = Session["PatientIE_ID"].ToString();
                PrintProcedures(patientIEID);

            }


        }
        if (!string.IsNullOrEmpty(Convert.ToString(Session["DVLbl"])))
        {
            DateTime dts = Convert.ToDateTime(Session["DVLbl"].ToString());
            string sd = String.Format("{0:MM/dd/yyyy}", dts);
            dov.Value = sd.Replace('-', '/');
        }
    }
    public void PrintProcedures(int patientIEID)
    {
        BusinessLogic _bl = new BusinessLogic();
        DBHelperClass db = new DBHelperClass();
        PlaceHolder1.Controls.Clear();
        PlaceHolder2.Controls.Clear();
        DataTable x = new DataTable();
        DataTable dtconsider = new DataTable();
        //ltNew.Text = "<button type='button' class='btn btn-default top-right' data-toggle='modal' id='New' data-target='#ProcedureDetailModal'>New</button>";


        List<string> _injured = _bl.getInjuredParts(patientIEID).Distinct<string>().ToList<string>();
        DataTable BodypartList = db.GetallBodyparts();

        dtconsider = db.GetAllConsider(Convert.ToInt32(Session["PatientIE_ID"]));
        JavaScriptSerializer oSerializer = new JavaScriptSerializer();

        var Resultcons = (from c in dtconsider.AsEnumerable()
                          select new
                          {
                              pid = c.Field<Int64>("Procedure_ID")
                          }).ToList();

        hdnControlconsider.Value = oSerializer.Serialize(Resultcons);

        StringBuilder html1 = new StringBuilder();

        if (dtconsider != null)
        {

            if (dtconsider.Rows.Count > 0)
            {

                html1.Append("<div class='panel panel-default'>" +
                       "<div class='panel-heading'><h4 class='panel-title'>" +
                       "<a class='collapse' style='cursor: pointer;'  id='#Considertable'>Consider</a>" +
                       "</h4></div><div id='Considertablediv' class='panel-collapse collapse in' style='display:block'>" +
                       "<div class='panel-body'><table class='Proctable' border = '1'>");
                html1.Append("<thead><tr><th style='height: 35px; background-color:yellow'>BodyPart</th><th style='height: 35px; background-color:yellow'>Mcode</th><th style='height: 35px; background-color:yellow'>House</th><th style='height: 35px; background-color:yellow'>Action</th></tr></thead><tbody>");
                foreach (DataRow row in dtconsider.Rows)
                {
                    html1.Append("<tr>");
                    html1.Append("<td>" + Convert.ToString(row[1]) + "</td>");
                    html1.Append("<td>" + Convert.ToString(row[3]) + "</td>");
                    html1.Append("<td>" + Convert.ToString(row[4]) + "</td>");
                    html1.Append("<td> <input type='button' class='btn btn-warning btn-sm' style='margin-left:25px' value='X' data-toggle='tooltip' title='Delete Consider' onclick='considerPopup($(this));'data-ConID='" + row[0] + "'/> </td>");
                    html1.Append("</tr>");
                }
                html1.Append("</tbody></table></div></div></div>");
            }

        }

        PlaceHolder2.Controls.Add(new Literal { Text = html1.ToString() });

        List<string> _injuredBodyParts = new List<string>();
        Table tbl = new Table();
        // Add the table to the placeholder control
        PlaceHolder1.Controls.Add(tbl);
        // x = FlipDataTable(x);
        string potion = null;
        string iinew = string.Empty;
        foreach (var ii in _injured)
        {

            if (ii.Contains("Left"))
            {
                potion = "Left";
                iinew = ii.Substring(4, ii.Length - 4);
            }
            else if (ii.Contains("Right"))
            {
                potion = "Right";
                iinew = ii.Substring(5, ii.Length - 5);
            }
            else
            {
                potion = null;
                iinew = ii;
            }

            x = db.GetAllProcDetails(iinew, patientIEID, potion);
            if (x != null)
                if (x.Rows.Count > 0)
                {

                    // x =db.GetAllProcDetails(ii, patientIEID);
                }
                else
                {
                    x = db.GetAllProcDetail(iinew, patientIEID, potion);
                }
            // x = RemoveDuplicateRows(x, "procedureID");
            //StringBuilder html = new StringBuilder();

            if (x != null)
            {
                if (x.Rows.Count > 0)
                {
                    StringBuilder html = new StringBuilder();
                    //Table start.
                    html.Append("<div class='panel panel-default'>" +
                        "<div class='panel-heading'><h4 class='panel-title'>" +
                        "<a class='collapse' style='cursor: pointer;'  id='#" + ii + "_Colps'>" + ii + "</a>" +
                        "</h4></div><div id='" + ii + "_Colpsdiv' class='panel-collapse collapse' style='display:none'>" +
                        "<div class='panel-body'>" +
                        "<table class='Proctable'  border = '1' id='" + ii + "_tbl'>");
                    html.Append("<thead>");
                    //Building the Header row.
                    html.Append("<tr>");

                    foreach (DataColumn column in x.Columns)
                    {
                        if (column.ColumnName != "procedureID" && (column.ColumnName != "MCODE") &&
                        (column.ColumnName != "INhouseProc") && (column.ColumnName != "HasPosition") &&
                        (column.ColumnName != "HasLevel") && (column.ColumnName != "HasMuscle") &&
                        (column.ColumnName != "HasSubCode") && (column.ColumnName != "BID") && (column.ColumnName != "HasMedication") &&
                        (column.ColumnName != "PatientProceduresID")
                        && (column.ColumnName != "Medication") && (column.ColumnName != "Muscle") && (column.ColumnName != "Level")
                        && (column.ColumnName != "Req_Pos") && (column.ColumnName != "Sch_Pos") && (column.ColumnName != "FU_Pos")
                        && (column.ColumnName != "Exe_Pos") && (column.ColumnName != "SubProcedureID") && (column.ColumnName != "PatientIEID")
                        && (column.ColumnName != "PatientFuID") && (column.ColumnName != "Sides") && (column.ColumnName != "HasSides")
                        && (column.ColumnName != "Display_Order") && (column.ColumnName != "ProcedureDetailID")
                        && (column.ColumnName != "LevelsDefault") && (column.ColumnName != "SidesDefault") && (column.ColumnName != "CF") && (column.ColumnName != "SignPath"))
                        {
                            if (column.ColumnName == "bodypart")
                            {
                                html.Append("<th style='height: 35px; background-color:yellow'>");
                                html.Append(ii);
                                html.Append("</th>");
                            }
                            else
                            {

                                if (column.ColumnName != "Followup")
                                {
                                    html.Append("<th  style='height: 35px;'>");
                                    html.Append(column.ColumnName);
                                    html.Append("</th>");
                                }
                                else
                                {
                                    html.Append("<th  style='height: 35px; background-color:grey'>");
                                    html.Append(column.ColumnName);
                                    html.Append("</th>");
                                }

                            }
                        }
                    }
                    html.Append("</tr>");
                    html.Append("</thead>");
                    html.Append("<tbody>");

                    foreach (DataRow row in x.Rows)
                    {
                        html.Append("<tr>");
                        foreach (DataColumn column in x.Columns)
                        {
                            if ((column.ColumnName != "ProcedureDetail_ID") && (column.ColumnName != "Bodypart") && (column.ColumnName != "patientfu_id") && (column.ColumnName != "patientie_id") && (column.ColumnName != "InHouseProc"))
                            {
                                if (column.ColumnName == "Consider")
                                {
                                    string date1 = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[14])))
                                    { date1 = Convert.ToDateTime(row[14]).ToString("MM/dd/yyyy").Replace('-', '/'); }


                                    if (row[10] != null)
                                        if (Convert.ToInt32(row[10]) != 0)
                                        {
                                            if (row[14] != DBNull.Value)
                                            {
                                                html.Append("<td >");
                                                html.Append("<input type='button' class='btn btn-danger'   onclick='boxDisable($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "'   data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-Procedure_Detail_ID='" + row[30] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "'  data-Date='" + date1 + "' data-Position='Consider'   id='" + row[0] + "_C_" + row[1] + "_" + row[2] + "' value='Consider' />");
                                                html.Append("</td>");
                                            }
                                            else
                                            {
                                                html.Append("<td >");
                                                html.Append("<input type='button' class='btn btn-danger' onclick='boxDisable($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-Procedure_Detail_ID='" + row[30] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "'  data-Date='" + date1 + "' data-Position='Consider'   id='" + row[0] + "_C_" + row[1] + "_" + row[2] + "' value='Consider' />");
                                                html.Append("</td>");
                                            }
                                        }
                                        else
                                        {
                                            html.Append("<td >");
                                            html.Append("<input type='button' class='btn btn-danger' onclick='boxDisable($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-Procedure_Detail_ID='" + row[30] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "'  data-Date='" + date1 + "' data-Position='Consider'   id='" + row[0] + "_C_" + row[1] + "_" + row[2] + "' value='Consider' />");
                                            html.Append("</td>");
                                        }
                                }
                                else if (column.ColumnName == "Requested")
                                {

                                    string date1 = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[15])))
                                    { date1 = Convert.ToDateTime(row[15]).ToString("MM/dd/yyyy").Replace('-', '/'); }

                                    StringBuilder notify = new StringBuilder();

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[12])))
                                    { notify.AppendLine("muscle:" + row[12]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                                    { notify.AppendLine("medication:" + row[11]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[23])))
                                    { notify.AppendLine("SubCode:" + row[23]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[27])))
                                    { notify.AppendLine("side:" + row[27]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[13])))
                                    { notify.AppendLine("level:" + row[13]); }
                                    if (row[10] != null)
                                        if (Convert.ToInt32(row[10]) != 0)
                                        {
                                            html.Append("<td >");
                                            html.Append("<input type='text'  onclick='PopupNE($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "'  data-toggle='tooltip' title='" + notify + "'  data-Sides='" + row[27] + "' data-Procedure_Detail_ID='" + row[30] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "'data-ReqPos='" + row[19] + "' data-Bodyid='" + row[7] + "' data-Position='Request' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_R_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                            html.Append("</td>");
                                        }
                                        else
                                        {
                                            html.Append("<td >");
                                            html.Append("<input type='text' onclick='Popup($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "'  data-toggle='tooltip' title='" + notify + "'  data-Sides='" + row[27] + "' data-Procedure_Detail_ID='" + row[30] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Request' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_R_" + row[1] + "_" + row[2] + "' value='' />");
                                            html.Append("</td>");
                                        }
                                }
                                else if (column.ColumnName == "Scheduled")
                                {

                                    string date1 = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[16])))
                                    { date1 = Convert.ToDateTime(row[16]).ToString("MM/dd/yyyy").Replace('-', '/'); }


                                    StringBuilder notify = new StringBuilder();
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[12])))
                                    { notify.AppendLine("muscle:" + row[12]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                                    { notify.AppendLine("medication:" + row[11]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[23])))
                                    { notify.AppendLine("SubCode:" + row[23]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[27])))
                                    { notify.AppendLine("side:" + row[27]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[13])))
                                    { notify.AppendLine("level:" + row[13]); }

                                    if (row[10] != null)
                                    {
                                        if (Convert.ToInt32(row[10]) != 0)
                                        {
                                            html.Append("<td >");
                                            html.Append("<input type='text' class='ProcText' onclick='PopupNE($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[30] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "'data-ReqPos='" + row[20] + "'data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Schedule' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "' data-MCode='" + row[1] + "'   id='" + row[0] + "_S_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                            html.Append("</td>");
                                        }
                                        else
                                        {
                                            html.Append("<td >");
                                            html.Append("<input type='text' class='ProcText' onclick='Popup($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[30] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-PID='" + row[0] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Schedule' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "' data-MCode='" + row[1] + "'   id='" + row[0] + "_S_" + row[1] + "_" + row[2] + "' value='' />");
                                            html.Append("</td>");
                                        }
                                    }

                                }
                                else if (column.ColumnName == "Executed")
                                {

                                    string date1 = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[17])))
                                    { date1 = Convert.ToDateTime(row[17]).ToString("MM/dd/yyyy").Replace('-', '/'); }


                                    StringBuilder notify = new StringBuilder();
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[12])))
                                    { notify.AppendLine("muscle:" + row[12]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                                    { notify.AppendLine("medication:" + row[11]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[23])))
                                    { notify.AppendLine("SubCode:" + row[23]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[27])))
                                    { notify.AppendLine("side:" + row[27]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[13])))
                                    { notify.AppendLine("level:" + row[13]); }

                                    if (row[10] != null)
                                        if (Convert.ToInt32(row[10]) != 0)
                                        {
                                            html.Append("<td>");
                                            html.Append("<input type='text' onclick='PopupNE($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[30] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "'data-PID='" + row[0] + "'data-ReqPos='" + row[22] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Execute' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "' data-Medication='" + row[9] + "' data-SignPath='" + row[33] + "'    data-Date='" + date1 + "' data-MCode='" + row[1] + "'   class='ProcText' id='" + row[0] + "_E_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                            html.Append("</td>");
                                        }
                                        else
                                        {
                                            html.Append("<td>");
                                            html.Append("<input type='text' onclick='Popup($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[30] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-PID='" + row[0] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Execute' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "' data-Medication='" + row[9] + "'  data-Date='" + date1 + "' data-CF='" + row[34] + "' data-MCode='" + row[1] + "'     class='ProcText' id='" + row[0] + "_E_" + row[1] + "_" + row[2] + "' value='' />");
                                            html.Append("</td>");
                                        }
                                }
                                else if (column.ColumnName == "Followup")
                                {

                                    string date1 = string.Empty;
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[18])))
                                    { date1 = Convert.ToDateTime(row[18]).ToString("MM/dd/yyyy").Replace('-', '/'); }

                                    StringBuilder notify = new StringBuilder();
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[12])))
                                    { notify.AppendLine("muscle:" + row[12]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                                    { notify.AppendLine("medication:" + row[11]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[23])))
                                    { notify.AppendLine("SubCode:" + row[23]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[27])))
                                    { notify.AppendLine("side:" + row[27]); }
                                    if (!string.IsNullOrEmpty(Convert.ToString(row[13])))
                                    { notify.AppendLine("level:" + row[13]); }

                                    if (row[10] != null)
                                        if (Convert.ToInt32(row[10]) != 0)
                                        {
                                            html.Append("<td style='background:grey'>");
                                            html.Append("<input style='background:grey' type='text' onclick='PopupNE($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[30] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-PID='" + row[0] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "'data-ReqPos='" + row[21] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Follow Up' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_F_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                            html.Append("</td>");
                                        }
                                        else
                                        {
                                            html.Append("<td style='background:grey'>");
                                            html.Append("<input style='background:grey' type='text' onclick='Popup($(this));' data-LevelsDefault='" + row[31] + "' data-SidesDefault='" + row[32] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[30] + "' data-Sides='" + row[27] + "' data-HasSides ='" + row[28] + "' data-PPID='" + row[10] + "' data-PatientIEID='" + row[24] + "' data-PatientFUID='" + row[25] + "' data-PID='" + row[0] + "' data-Level='" + row[13] + "' data-Medi='" + row[11] + "' data-Musc='" + row[12] + "' data-SubPID='" + row[23] + "' data-Body='" + row[8] + "' data-Bodyid='" + row[7] + "' data-Position='Follow Up' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_F_" + row[1] + "_" + row[2] + "' value='' />");
                                            html.Append("</td>");
                                        }
                                }
                                else if (column.ColumnName == "count")
                                {
                                    html.Append("<td>");
                                    html.Append("<input type='button' class='btn btn-warning btn-sm' style='margin-left:25px' onclick='CountPopup($(this));' data-Div='" + ii + "_counttable' data-Procedure_Detail_ID='" + row[30] + "' data-PatientIEID='" + row[24] + "' data-PID='" + row[0] + "' value='" + row[26] + "'  />");
                                    html.Append("</td>");
                                }
                                else if (column.ColumnName == "MCODE")
                                {
                                    html.Append("<td style='text-align:center; background-color:#3de33d'>");
                                    html.Append(row[column.ColumnName]);
                                    html.Append("</td>");
                                }
                            }
                        }
                        html.Append("</tr>");
                    }
                    html.Append("</tbody></table><div id='" + ii + "_counttable' style='padding-left: 1%' ></div></div></div></div>");

                    //Append the HTML string to Placeholder.
                    PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

                    Page.ClientScript.RegisterStartupScript(this.GetType(), ii, "tableTransform($('#" + ii + "_tbl'));", true);

                }
                else
                {
                    StringBuilder html = new StringBuilder();
                    html.Append("<table border = '1' id='" + ii + "_tbl'>");
                    html.Append("</table>");
                    //Append the HTML string to Placeholder.
                    PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
                }
            }
        }

    }
    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();
        DataTable dt = new DataTable();
        //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
        //And add duplicate item value in arraylist.
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
            {

                duplicateList.Add(drow);
            }
            else
            {
                hTable.Add(drow[colName], string.Empty);
            }
        }

        //Removing a list of duplicate items from datatable.
        foreach (DataRow dRow in duplicateList)
            dt.Rows.Remove(dRow);

        //Datatable which contains unique records will be return as output.
        return dTable;
    }
    public static DataTable FlipDataTable(DataTable dt)
    {
        DataTable table = new DataTable();
        //Get all the rows and change into columns
        for (int i = 0; i <= dt.Rows.Count; i++)
        {
            table.Columns.Add(Convert.ToString(i));
        }
        DataRow dr;
        //get all the columns and make it as rows
        for (int j = 0; j < dt.Columns.Count; j++)
        {
            dr = table.NewRow();
            dr[0] = dt.Columns[j].ToString();
            for (int k = 1; k <= dt.Rows.Count; k++)
            {

                dr[k] = dt.Rows[k - 1][j];

            }
            table.Rows.Add(dr);
        }

        return table;
    }

    public List<Procedure> GetProceduresByBody(string _bodypart, string Position)
    {
        List<Procedure> _procedures = new List<Procedure>();
        DataAccess _dal = new DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@BodyPart", _bodypart));
        param.Add(new SqlParameter("@Position", Position));
        DataTable _dt = _dal.getDataTable("nusp_getProcedureCodesByParts", param);
        foreach (DataRow _dr in _dt.Rows)
        {
            Procedure _procedure = new Procedure();
            _procedure.ProcedureId = Convert.ToInt64(_dr["Procedure_ID"]);
            _procedure.MCode = _dr["MCode"].ToString();
            _procedure.DateType = _dr["DateType"].ToString();
            _procedure.BodyPart = _dr["BodyPart"].ToString();
            _procedure.Heading = _dr["Heading"].ToString();
            _procedure.CCDesc = _dr["CCDesc"].ToString();
            _procedure.PEDesc = _dr["PEDesc"].ToString();
            _procedure.ADesc = _dr["ADesc"].ToString();
            _procedure.PDesc = _dr["PDesc"].ToString();
            _procedure.CF = _dr["CF"] != null ? false : Convert.ToBoolean(_dr["CF"]);
            _procedure.PN = _dr["PN"] != null ? false : Convert.ToBoolean(_dr["PN"]);
            //Convert.ToBoolean(_dr["PN"]);
            _procedures.Add(_procedure);
        }

        return _procedures;
    }



    [WebMethod]
    public static string Save(long ProcedureDetailID, long ProcedureMasterID, long _patientIEID, long? _patientFUID, string SubProcedureID, string BodyPart, long ProcedureID, string Medication, string Muscle, string Level, string Position, string date, string req, int BodyPartID, int IsFromNew, int? PatientProcedureID, Int16 IsConsidered, string Side, string BlobStr = "")
    {
        POC _bl = new POC();
        string Req_Pos = ""; string Sch_Pos = ""; string Exe_Pos = ""; string FU_Pos = "";
        //DateTime Requested=new DateTime(); DateTime Scheduled = new DateTime(); DateTime Followup = new DateTime(); DateTime Consider = new DateTime(); DateTime Executed = new DateTime();
        //int BodyPartID = 0;
        string count = "", bodyPartSide = "";

        if (!string.IsNullOrEmpty(Position))
            bodyPartSide = Position + " " + BodyPart;
        else
            bodyPartSide = BodyPart;

        switch (req)
        {
            case "Request":
                DateTime Requested = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Req_Pos = Position;
                count = _bl.savePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, _patientIEID, _patientFUID, BodyPartID, ProcedureID, null, Requested, null, null, null, BodyPart, Medication, Muscle, Level, Req_Pos, Sch_Pos, Exe_Pos, FU_Pos, 0, IsFromNew, PatientProcedureID, req, IsConsidered, Side, SubProcedureID, null, bodyPartSide);
                break;
            case "Schedule":
                DateTime Scheduled = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Sch_Pos = Position;
                count = _bl.savePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, _patientIEID, _patientFUID, BodyPartID, ProcedureID, null, null, Scheduled, null, null, BodyPart, Medication, Muscle, Level, Req_Pos, Sch_Pos, Exe_Pos, FU_Pos, 0, IsFromNew, PatientProcedureID, req, IsConsidered, Side, SubProcedureID, null, bodyPartSide);
                break;
            case "Follow Up":
                DateTime Followup = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                FU_Pos = Position;
                count = _bl.savePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, _patientIEID, _patientFUID, BodyPartID, ProcedureID, null, null, null, Followup, null, BodyPart, Medication, Muscle, Level, Req_Pos, Sch_Pos, Exe_Pos, FU_Pos, 0, IsFromNew, PatientProcedureID, req, IsConsidered, Side, SubProcedureID, null, bodyPartSide);
                break;
            case "Consider":
                DateTime Consider = DateTime.Now;
                //count = _bl.savePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, _patientIEID, _patientFUID, BodyPartID, ProcedureID, NULL, null, null, null, null, BodyPart, Medication, Muscle, Level, Req_Pos, Sch_Pos, Exe_Pos, FU_Pos, 0, IsFromNew, PatientProcedureID, req, IsConsidered, Side, SubProcedureID);
                count = _bl.savePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, _patientIEID, _patientFUID, BodyPartID, ProcedureID, null, null, null, null, null, BodyPart, Medication, Muscle, Level, Req_Pos, Sch_Pos, Exe_Pos, FU_Pos, 0, IsFromNew, PatientProcedureID, req, IsConsidered, Side, SubProcedureID, null, bodyPartSide);
                break;
            case "Execute":

                string fullpath = "", fname = "";
                byte[] blob = null;
                if (string.IsNullOrEmpty(BlobStr) == false)
                {
                    try
                    {
                        string blobstring = BlobStr.Split(',')[1];
                        blob = Convert.FromBase64String(blobstring);

                        string path = HttpContext.Current.Server.MapPath("~/Sign/");
                        fname = _patientIEID.ToString() + "_" + System.DateTime.Now.Millisecond.ToString() + ".jpg";

                        fullpath = path + "//" + fname;
                    }
                    catch (Exception ex)
                    {
                    }

                }


                DateTime Executed = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Exe_Pos = Position;
                count = _bl.savePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, _patientIEID, _patientFUID, BodyPartID, ProcedureID, null, null, null, null, Executed, BodyPart, Medication, Muscle, Level, Req_Pos, Sch_Pos, Exe_Pos, FU_Pos, 0, IsFromNew, PatientProcedureID, req, IsConsidered, Side, SubProcedureID, fname,bodyPartSide);

                // File.WriteAllBytes(fullpath, blob);

                break;
        }
        // return count;
        return count;
    }


    [WebMethod]
    public static string Saveconsider(int procedureDetailID, string BodyPart, int patientIEID)
    {
        BusinessLogic _bl = new BusinessLogic();

        string status = _bl.Saveconsider(procedureDetailID, BodyPart, patientIEID);

        return status;
    }

    public string DataTableToJSONWithJSONNet(DataTable table)
    {
        string JSONString = string.Empty;
        JSONString = JsonConvert.SerializeObject(table);
        return JSONString;
    }

    [System.Web.Services.WebMethod]
    public static string GetSubcode(int PID)
    {
        DataAccess _dal = new DataAccess();
        DataTable dt = new DataTable();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PID", PID));
        dt = _dal.getDataTable("GetSubProc", param);
        string json = JsonConvert.SerializeObject(dt);
        return json;
    }
    [System.Web.Services.WebMethod]
    public static string GetProcedureCountDetail(int PID, int PatientIEID)
    {
        DataAccess _dal = new DataAccess();
        DataTable x = new DataTable();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@procedureID", PID));
        param.Add(new SqlParameter("@PatientIEID", PatientIEID));
        x = _dal.getDataTable("getProcBycountnew", param);
        string json = "";
        StringBuilder html = new StringBuilder();
        if (x.Rows.Count > 0)
        {
            // x = FlipDataTable(x);

            //Table start.
            html.Append("<table class='Proctable' width='400' border = '1' id='countbl'>");
            html.Append("<thead>");
            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in x.Columns)
            {

                if (column.ColumnName != "procedureID" && (column.ColumnName != "MCODE") &&
                (column.ColumnName != "INhouseProc") && (column.ColumnName != "Position") &&
                (column.ColumnName != "HasLevel") && (column.ColumnName != "HasMuscle") &&
                (column.ColumnName != "HasMedication") && (column.ColumnName != "PatientProceduresID")
                && (column.ColumnName != "Medication") && (column.ColumnName != "Muscle") && (column.ColumnName != "Level")
                && (column.ColumnName != "Req_Pos") && (column.ColumnName != "Sch_Pos") && (column.ColumnName != "FU_Pos")
                && (column.ColumnName != "Exe_Pos") && (column.ColumnName != "PatientIEID") && (column.ColumnName != "PatientFuID")
                && (column.ColumnName != "SubProcedureID") && (column.ColumnName != "HasSubCode")
                && (column.ColumnName != "HasSides") && (column.ColumnName != "sides") && (column.ColumnName != "ProcedureDetailID")
                && (column.ColumnName != "LevelsDefault") && (column.ColumnName != "SidesDefault"))
                {
                    if (column.ColumnName == "BodyPart")
                    {
                        html.Append("<th style='height: 35px; background-color:yellow'>");
                        html.Append("");
                        html.Append("</th>");
                    }
                    else
                    {
                        if (column.ColumnName == "Consider")
                        {
                            html.Append("<th style='display:none'>");
                            html.Append(column.ColumnName);
                            html.Append("</th>");
                        }
                        else if (column.ColumnName == "Followup")
                        {
                            html.Append("<th style='height: 35px;background:grey'>");
                            html.Append(column.ColumnName);
                            html.Append("</th>");
                        }
                        else
                        {
                            html.Append("<th  style='height: 35px;'>");
                            html.Append(column.ColumnName);
                            html.Append("</th>");
                        }

                    }
                }
                //}
            }
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");

            foreach (DataRow row in x.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in x.Columns)
                {
                    if ((column.ColumnName != "PatientProceduresID") && (column.ColumnName != "BodyPart") && (column.ColumnName != "PatientFuID") && (column.ColumnName != "PatientIEID") && (column.ColumnName != "INhouseProc"))
                    {
                        //|| column.ColumnName == "Consider" || )
                        if (column.ColumnName == "Consider")
                        {
                            if (row[8] != null)
                                if (Convert.ToInt32(row[8]) != 0)
                                {
                                    if (row[12] != DBNull.Value)
                                    {
                                        html.Append("<td style='display:none'>");
                                        html.Append("<div class='checkbox'><input type='checkbox' checked  onclick='boxDisable($(this));' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "' data-Body='" + row[6] + "' data-Date='" + row[12] + "' data-Position='Consider'   id='" + row[0] + "_C_" + row[1] + "_" + row[2] + "' value='Consider' /></div>");
                                        html.Append("</td>");
                                    }
                                    else
                                    {
                                        html.Append("<td style='display:none'>");
                                        html.Append("<div class='checkbox'><input type='checkbox' onclick='boxDisable($(this));' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "' data-Body='" + row[6] + "' data-Date='" + row[12] + "' data-Position='Consider'   id='" + row[0] + "_C_" + row[1] + "_" + row[2] + "' value='Consider' /></div>");
                                        html.Append("</td>");
                                    }
                                }
                                else
                                {
                                    html.Append("<td style='display:none'>");
                                    html.Append("<div class='checkbox'><input type='checkbox' onclick='boxDisable($(this));' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "' data-Body='" + row[6] + "' data-Date='" + row[12] + "' data-Position='Consider'   id='" + row[0] + "_C_" + row[1] + "_" + row[2] + "' value='Consider' /></div>");
                                    html.Append("</td>");
                                }
                        }
                        else if (column.ColumnName == "Requested")
                        {
                            string date1 = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(row[13])))
                            { date1 = Convert.ToDateTime(row[13]).ToString("MM/dd/yyyy").Replace('-', '/'); }


                            StringBuilder notify = new StringBuilder();
                            if (!string.IsNullOrEmpty(Convert.ToString(row[10])))
                            { notify.AppendLine("muscle:" + row[10]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[9])))
                            { notify.AppendLine("medication:" + row[9]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[24])))
                            { notify.AppendLine("SubCode:" + row[24]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[26])))
                            { notify.AppendLine("side:" + row[26]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                            { notify.AppendLine("level:" + row[11]); }

                            if (row[8] != null)
                                if (Convert.ToInt32(row[8]) != 0)
                                {
                                    html.Append("<td >");
                                    html.Append("<input type='text' data-toggle='tooltip' title='" + notify + "' onclick='PopupNE($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-PPID='" + row[8] + "' data-Procedure_Detail_ID='" + row[27] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "' data-Body='" + row[6] + "'data-ReqPos='" + row[17] + "' data-Position='Request' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_R_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                    html.Append("</td>");
                                }
                                else
                                {
                                    html.Append("<td >");
                                    html.Append("<input type='text' onclick='Popup($(this));'  data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-toggle='tooltip' title='" + notify + "'  data-PPID='" + row[8] + "'  data-Procedure_Detail_ID='" + row[27] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "' data-Body='" + row[6] + "' data-Position='Request' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_R_" + row[1] + "_" + row[2] + "' value='' />");
                                    html.Append("</td>");
                                }
                        }
                        else if (column.ColumnName == "Scheduled")
                        {

                            string date1 = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(row[14])))
                            { date1 = Convert.ToDateTime(row[14]).ToString("MM/dd/yyyy").Replace('-', '/'); }

                            StringBuilder notify = new StringBuilder();
                            if (!string.IsNullOrEmpty(Convert.ToString(row[10])))
                            { notify.AppendLine("muscle:" + row[10]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[9])))
                            { notify.AppendLine("medication:" + row[9]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[24])))
                            { notify.AppendLine("SubCode:" + row[24]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[26])))
                            { notify.AppendLine("side:" + row[26]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                            { notify.AppendLine("level:" + row[11]); }
                            if (row[8] != null)
                            {
                                if (Convert.ToInt32(row[8]) != 0)
                                {
                                    html.Append("<td >");
                                    html.Append("<input type='text' class='ProcText' data-toggle='tooltip' title='" + notify + "'  onclick='PopupNE($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "'data-ReqPos='" + row[18] + "' data-Body='" + row[6] + "' data-Position='Schedule' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'   id='" + row[0] + "_S_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                    html.Append("</td>");
                                }
                                else
                                {
                                    html.Append("<td >");
                                    html.Append("<input type='text' class='ProcText' data-toggle='tooltip' title='" + notify + "' onclick='Popup($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-PID='" + row[0] + "' data-Body='" + row[6] + "' data-Position='Schedule' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'   id='" + row[0] + "_S_" + row[1] + "_" + row[2] + "' value='' />");
                                    html.Append("</td>");
                                }
                            }

                        }
                        else if (column.ColumnName == "Executed")
                        {
                            string date1 = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(row[15])))
                            { date1 = Convert.ToDateTime(row[15]).ToString("MM/dd/yyyy").Replace('-', '/'); }

                            StringBuilder notify = new StringBuilder();
                            if (!string.IsNullOrEmpty(Convert.ToString(row[10])))
                            { notify.AppendLine("muscle:" + row[10]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[9])))
                            { notify.AppendLine("medication:" + row[9]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[24])))
                            { notify.AppendLine("SubCode:" + row[24]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[26])))
                            { notify.AppendLine("side:" + row[26]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                            { notify.AppendLine("level:" + row[11]); }

                            if (row[8] != null)
                                if (Convert.ToInt32(row[8]) != 0)
                                {
                                    html.Append("<td >");
                                    html.Append("<input type='text' onclick='PopupNE($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "'data-PID='" + row[0] + "'data-ReqPos='" + row[20] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-Body='" + row[6] + "' data-Position='Execute' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "' data-Medication='" + row[9] + "'  data-Date='" + date1 + "' data-MCode='" + row[1] + "'   class='ProcText' id='" + row[0] + "_E_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                    html.Append("</td>");
                                }
                                else
                                {
                                    html.Append("<td >");
                                    html.Append("<input type='text' onclick='Popup($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-PID='" + row[0] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-Body='" + row[6] + "' data-Position='Execute' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-Subcode='" + row[6] + "' data-InhouseProc='" + row[2] + "' data-Medication='" + row[9] + "'  data-Date='" + date1 + "' data-MCode='" + row[1] + "'   class='ProcText' id='" + row[0] + "_E_" + row[1] + "_" + row[2] + "' value='' />");
                                    html.Append("</td>");
                                }
                        }
                        else if (column.ColumnName == "Followup")
                        {
                            string date1 = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(row[16])))
                            { date1 = Convert.ToDateTime(row[16]).ToString("MM/dd/yyyy").Replace('-', '/'); }

                            StringBuilder notify = new StringBuilder();
                            if (!string.IsNullOrEmpty(Convert.ToString(row[10])))
                            { notify.AppendLine("muscle:" + row[10]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[9])))
                            { notify.AppendLine("medication:" + row[9]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[24])))
                            { notify.AppendLine("SubCode:" + row[24]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[26])))
                            { notify.AppendLine("side:" + row[26]); }
                            if (!string.IsNullOrEmpty(Convert.ToString(row[11])))
                            { notify.AppendLine("level:" + row[11]); }

                            if (row[8] != null)
                                if (Convert.ToInt32(row[8]) != 0)
                                {
                                    html.Append("<td style='background:grey'>");
                                    html.Append("<input type='text' style='background:grey' onclick='PopupNE($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-PID='" + row[0] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "'data-ReqPos='" + row[19] + "' data-Body='" + row[6] + "' data-Position='Follow Up' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_F_" + row[1] + "_" + row[2] + "' value='" + date1 + "' />");
                                    html.Append("</td>");
                                }
                                else
                                {
                                    html.Append("<td style='background:grey'>");
                                    html.Append("<input type='text' style='background:grey' onclick='Popup($(this));' data-LevelsDefault='" + row[28] + "' data-SidesDefault='" + row[29] + "' data-toggle='tooltip' title='" + notify + "' data-Procedure_Detail_ID='" + row[27] + "' data-PPID='" + row[8] + "' data-PatientIEID='" + row[21] + "' data-PatientFUID='" + row[22] + "' data-PID='" + row[0] + "' data-Level='" + row[11] + "' data-Medi='" + row[9] + "' data-Musc='" + row[10] + "' data-Body='" + row[6] + "' data-Position='Follow Up' data-HasLevel='" + row[4] + "' data-Pos='" + row[3] + "' data-Muscle='" + row[5] + "' data-InhouseProc='" + row[2] + "'  data-Medication='" + row[9] + "'  data-Date='" + date1 + "'  class='ProcText' id='" + row[0] + "_F_" + row[1] + "_" + row[2] + "' value='' />");
                                    html.Append("</td>");
                                }
                        }
                        else if (column.ColumnName == "MCODE")
                        {
                            html.Append("<td style='text-align:center; background-color:#3de33d'>");
                            html.Append(row[column.ColumnName]);
                            html.Append("</td>");
                        }
                    }
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");
            //Table end.
            html.Append("</table>");
            //Append the HTML string to Placeholder.
            //PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
            //Page.ClientScript.RegisterStartupScript(this.GetType(), ii, "tableTransform($('#" + ii + "_tbl'));", true);
        }
        return html.ToString();
    }
    [System.Web.Services.WebMethod]
    public static string GetMuscle(string Muscle)
    {
        DataTable dt = new DataTable();
        string[] lines = Muscle.Split('\n');
        dt.Columns.Add("Muscle");
        for (int i = 0; i < lines.Length; i++)
        {
            DataRow row = dt.NewRow();
            row[0] = lines[i];
            dt.Rows.Add(row);
        }
        string json = JsonConvert.SerializeObject(dt);
        return json;
    }

    [System.Web.Services.WebMethod]
    public static string GetMedicationFromDB(int Medication)
    {
        DataAccess _dal = new DataAccess();
        DataTable dt = new DataTable();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@Procedure_ID", Medication));
        param.Add(new SqlParameter("@result", SqlDbType.VarChar, -1));

        string test = _dal.getchar13string("GetMedicationnew", param);

        string[] lines = test.Split('\n');
        dt.Columns.Add("Medicaton");
        for (int i = 0; i < lines.Length; i++)
        {
            DataRow row = dt.NewRow();
            row[0] = lines[i];
            dt.Rows.Add(row);
        }
        string json = JsonConvert.SerializeObject(dt);
        return json;
    }

    [System.Web.Services.WebMethod]
    public static string GetMuscleFromDB(int Muscle)
    {
        DataAccess _dal = new DataAccess();
        DataTable dt = new DataTable();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@Procedure_ID", Muscle));
        param.Add(new SqlParameter("@result", SqlDbType.VarChar, -1));

        string test = _dal.getchar13string("GetMusclenew", param);

        string[] lines = test.Split('\n');
        dt.Columns.Add("Muscle");
        for (int i = 0; i < lines.Length; i++)
        {
            DataRow row = dt.NewRow();
            row[0] = lines[i];
            dt.Rows.Add(row);
        }
        string json = JsonConvert.SerializeObject(dt);
        return json;
    }

    [System.Web.Services.WebMethod]
    public static string DeleteConsiderFromDB(int ConID)
    {
        DataAccess _dal = new DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ConID", ConID));
        param.Add(new SqlParameter("@result", SqlDbType.VarChar, -1));

        return _dal.getchar13string("GetDeleteConsidernew", param);
    }


    [System.Web.Services.WebMethod]
    public static string GetSubCodeFromDB(int SubCode)
    {
        DataAccess _dal = new DataAccess();
        DataTable dt = new DataTable();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@Procedure_ID", SubCode));
        param.Add(new SqlParameter("@result", SqlDbType.VarChar, -1));

        string test = _dal.getchar13string("GetSubCodenew", param);

        string[] lines = test.Split('\n');
        dt.Columns.Add("SubCode");
        for (int i = 0; i < lines.Length; i++)
        {
            DataRow row = dt.NewRow();
            row[0] = lines[i];
            dt.Rows.Add(row);
        }
        string json = JsonConvert.SerializeObject(dt);
        return json;
    }

    [WebMethod]
    public static string Delete(long ProcedureDetailID, long ProcedureMasterID, string Position, string req)
    {
        BusinessLogic _bl = new BusinessLogic();
        string count = "";
        string Req_Pos = ""; string Sch_Pos = ""; string Exe_Pos = ""; string FU_Pos = "";

        switch (req)
        {
            case "Request":
                Req_Pos = Position;
                count = _bl.DeletePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, req);
                break;
            case "Schedule":
                Sch_Pos = Position;
                count = _bl.DeletePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, req);
                break;
            case "Follow Up":
                FU_Pos = Position;
                count = _bl.DeletePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, req);
                break;
            //case "Consider":
            //    count = _bl.DeletePatientProcedureDetail(ProcedureMasterID, req);
            //    break;
            case "Execute":
                Exe_Pos = Position;
                count = _bl.DeletePatientProcedureDetail(ProcedureDetailID, ProcedureMasterID, req);
                break;
        }
        // return count;
        return count;

    }

    [System.Web.Services.WebMethod]
    public static string checkStatus(string MCode, string BodyPart, int PatientIEID)
    {
        BusinessLogic _bl = new BusinessLogic();
        string precode = "";
        int status = _bl.CheckExecuteStatus(MCode, BodyPart, out precode, PatientIEID.ToString());

        return "{ \"cnt\" : \"" + status + "\",\"mcode\": \"" + precode + "\" }";
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

            string strPoc = "";
            if (dsPOC != null && dsPOC.Tables[0].Rows.Count > 0)
            {
                repSummery.DataSource = dsPOC;
                repSummery.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public string savePatientProcedureDetail(long ProcedureDetailID, long ProcedureMasterID, long _patientIEID, long? _patientFUID, long BodyPartID, long? ProcedureID, DateTime? Consider, DateTime? Requested, DateTime? Scheduled, DateTime? Followup, DateTime? Executed, string BodyPart, string Medication, string Muscle, string Level, string Req_Pos, string Sch_Pos, string Exe_Pos, string FU_Pos, int CreatedBy, int IsFromNew, int? PatientProcedureID, string req, Int16 IsConsidered, string Side, string SubCode, string SignPath = null, string BodypartSide = "")
    {

       


        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ProcedureDetailID", ProcedureDetailID));
        param.Add(new SqlParameter("@ProcedureMasterID", ProcedureMasterID));
        //param.Add(new SqlParameter("@SubProcedureID", SubProcedureID));
        //param.Add(new SqlParameter("@BodyPartID", BodyPartID));
        param.Add(new SqlParameter("@PatientIEID", _patientIEID));
        //if (_patientFUID != 0)
        param.Add(new SqlParameter("@ProcedureID", ProcedureID));
        param.Add(new SqlParameter("@PatientFuID", _patientFUID));
        param.Add(new SqlParameter("@Consider", Consider));
        param.Add(new SqlParameter("@Requested", Requested));
        param.Add(new SqlParameter("@Scheduled", Scheduled));
        param.Add(new SqlParameter("@Followup", Followup));
        param.Add(new SqlParameter("@Executed", Executed));
        param.Add(new SqlParameter("@BodyPart", BodyPart));
        param.Add(new SqlParameter("@CreatedBy", CreatedBy));
        param.Add(new SqlParameter("@Medication", Medication));
        param.Add(new SqlParameter("@Muscle", Muscle));
        param.Add(new SqlParameter("@Level", Level));
        param.Add(new SqlParameter("@Req_Pos", Req_Pos));
        param.Add(new SqlParameter("@Sch_Pos", Sch_Pos));
        param.Add(new SqlParameter("@Exe_Pos", Exe_Pos));
        param.Add(new SqlParameter("@FU_Pos", FU_Pos));
        param.Add(new SqlParameter("@IsFromNew", IsFromNew));
        param.Add(new SqlParameter("@PatientProceduresID", PatientProcedureID));
        param.Add(new SqlParameter("@Category", req));
        param.Add(new SqlParameter("@IsConsidered", IsConsidered));
        param.Add(new SqlParameter("@Side", Side));
        param.Add(new SqlParameter("@Subcode", SubCode));
        param.Add(new SqlParameter("@SignPath", SignPath));
        param.Add(new SqlParameter("@BodypartSide", BodypartSide));

        DataAccess _dal = new DataAccess();
        return _dal.putData("nusp_SavePatientProceduresDetailnew", param);

    }

}