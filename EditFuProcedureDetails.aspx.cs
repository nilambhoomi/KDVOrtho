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

public partial class EditFuProcedureDetails : System.Web.UI.Page
{
    static string prevPage = String.Empty;
    private string OTHER = "Other";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.UrlReferrer == null)
            {
                prevPage = Session["prevPage"].ToString();
            }
            else
            {
                prevPage = Request.UrlReferrer.ToString();
                Session["prevPage"] = prevPage;
            }

            hfPatientIE_ID.Value = Session["PatientIE_ID"].ToString();
            hfPatientFU_ID.Value =Session["patientFUId"].ToString();

            bindDOE();
            bindLocation();
            lblMAandProviders.Text = string.Empty;
            txtDate.Text = DOEhdn.Value;
            btnSave.Enabled = false;
            if (Session["Providers"] != null)
            {
                lblMAandProviders.Text = Convert.ToString(Session["Providers"]);
            }
            if (Session["Location"] != null)
            {
                ddLocation.SelectedValue = Convert.ToString(Session["Location"]);
            }
            //bindMAandProviders();
            if (Request.QueryString["PId"] != null)
            {
                pnlSearch.Visible = false;
                int patientIEID = Convert.ToInt32(Request.QueryString["PId"]);
                BusinessLogic bl = new BusinessLogic();
                List<PatientsByDOE_Result> PatientResult = bl.getPatientByIE(patientIEID);
              
                TreeNode tn = loadPatientsToTreeNode(PatientResult).FirstOrDefault();

                if (tn != null)
                {
                    tvPatients.Nodes.Add(tn);
                    lblDName.Text = "";
                    lblDDOA.Text = "";
                    lblPDLocation.Text = "";
                    lblDCaseType.Text = "";
                    ltNew.Text = "<button type='button' class='btn btn-default top-right' data-toggle='modal' id='New' data-target='#ProcedureDetailModal'>New</button>";

                    string[] _ids = tn.Value.Split('_');
                    TreeNodeCollection tvc = tn.ChildNodes;
                    lblDName.Text = tn.Text;
                    lblDDOA.Text = tvc[2].Text;
                    lblPDLocation.Text = tvc[1].Text;
                    lblDCaseType.Text = tvc[0].Text;
                    string doe = tvc[4].Text.Remove(0, 5);
                    DOEhdn.Value = doe;
                    txtDate.Text = doe;
                    Session["doe"] = doe;
                    ddDate.SelectedIndex = ddDate.Items.IndexOf(ddDate.Items.FindByText(doe));
                    EnableAllTreeNodes();

                    tn.SelectAction = TreeNodeSelectAction.None;
                    

                    loadProcedureDetails();
                }
                //if (ddLocation.SelectedIndex <= 0)
                //{
                //    ddLocation.Items.FindByText((PatientResult.Count() > 0) ? PatientResult[0].Location : "All").Selected = true;
                //}


                tvPatients.Nodes[0].Checked = true;
            }
        }
        else
        {
            // txtDate.Text = 
            Session["doe"] = txtDate.Text;
            //ddDate.SelectedValue;  
            //Session["doe"].ToString();
        }

        //if (ViewState["tblProcedures"] != null && pnlProcedures.Controls.Count == 0)
        //{
        //    pnlProcedures.Controls.Add((MyTable)ViewState["tblProcedures"]);
        //}
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuProcedureDetails for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }

    protected void bindDOE()
    {
        BusinessLogic _bl = new BusinessLogic();
        ddDate.Items.Clear();
        ddDate.Items.Add(new ListItem("--Please Select--", "01/01/1900"));
        foreach (DateTime _date in _bl.getDOES())
        {
            ddDate.Items.Add(new ListItem(_date.ToString("MM/dd/yyyy"), _date.ToString()));
        }

    }

    protected void bindLocation()
    {
        BusinessLogic _bl = new BusinessLogic();
        ddLocation.Items.Clear();
        ddLocation.Items.Add(new ListItem("All", "All"));
        foreach (string[] _location in _bl.getLocations())
        {
            ddLocation.Items.Add(new ListItem(_location[1], _location[0]));
        }


    }

    //protected void bindMAandProviders()
    //{
    //    BusinessLogic bl = new BusinessLogic();
    //    lbMAandProviders.Items.Clear();
    //    foreach (User user in bl.getAllProvidersAndMA())
    //    {
    //        lbMAandProviders.Items.Add(new ListItem(user.FirstName + ' ' + user.LastName, user.UserId.ToString()));
    //    }
    //}

    protected void bindBodyParts(List<string> _injuredBodyParts)
    {
        rblDateType.SelectedIndex = -1;
        ddProcedure.Items.Clear();
        ddSubProcedure.Items.Clear();
        ddBodyPart.Items.Clear();
        ddBodyPart.Items.Add(new ListItem("Please Select", ""));
       // ddBodyPart.Items.Add(new ListItem(OTHER, OTHER));
        List<string> _filters = new List<string>(){
                "Left",
                "Right"
            };
        List<Body> blist = new List<Body>();
        foreach (string s in _injuredBodyParts.Distinct<string>())
        {
            Body b = new Body();
            switch (s)
            {
                case "LeftShoulder":
                    b.Part = "Shoulder";
                    b.Position = "L";
                    break;
                case "RightShoulder":
                    b.Part = "Shoulder";
                    b.Position = "R";
                    break;
                case "LeftKnee":
                    b.Part = "Knee";
                    b.Position = "L";
                    break;
                case "RightKnee":
                    b.Part = "Knee";
                    b.Position = "R";
                    break;
                case "LeftElbow":
                    b.Part = "Elbow";
                    b.Position = "L";
                    break;
                case "RightElbow":
                    b.Part = "Elbow";
                    b.Position = "R";
                    break;
                case "LeftWrist":
                    b.Part = "Wrist";
                    b.Position = "L";
                    break;
                case "RightWrist":
                    b.Part = "Wrist";
                    b.Position = "R";
                    break;
                case "LeftHip":
                    b.Part = "Hip";
                    b.Position = "L";
                    break;
                case "RightHip":
                    b.Part = "Hip";
                    b.Position = "R";
                    break;
                case "LeftAnkle":
                    b.Part = "Ankle";
                    b.Position = "L";
                    break;
                case "RightAnkle":
                    b.Part = "Ankle";
                    b.Position = "R";
                    break;
                case "LeftArm":
                    b.Part = "Arm";
                    b.Position = "L";
                    break;
                case "RightArm":
                    b.Part = "Arm";
                    b.Position = "R";
                    break;
                case "LeftHand":
                    b.Part = "Hand";
                    b.Position = "L";
                    break;
                case "RightHand":
                    b.Part = "Hand";
                    b.Position = "R";
                    break;
                case "LeftLeg":
                    b.Part = "Leg";
                    b.Position = "L";
                    break;
                case "RightLeg":
                    b.Part = "Leg";
                    b.Position = "R";
                    break;
                case "LeftFoot":
                    b.Part = "Foot";
                    b.Position = "L";
                    break;
                case "RightFoot":
                    b.Part = "Foot";
                    b.Position = "R";
                    break;
                default:
                    b.Part = s;
                    break;
            }
            blist.Add(b);
        }
        List<Body> bodylist = new List<Body>();
        foreach (var i in blist)
        {
            var j = blist.AsEnumerable().Where(k => k.Part == i.Part).Select(k => k.Position).ToList();
            if (j.Count() > 1)
            {
                Body k = new Body();
                k.Part = i.Part;
                k.Position = "B";
                bodylist.Add(k);
            }
            else
            {
                bodylist.Add(i);
            }
        }
        // var f=  bodylist.AsEnumerable().Where(m => m.Position == "B").Select(m => m.Part).ToList();
        var distinctList = bodylist.GroupBy(x => x.Part)
                         .Select(g => g.First())
                         .ToList();
        foreach (var i in distinctList)
        {
            ddBodyPart.Items.Add(new ListItem(i.Part, i.Part + '_' + i.Position));
        }

        //
    }

    protected void bindProcedures(string _bodypart, string _datetype)
    {
        lblAlert.Text = "";
        ddSubProcedure.Items.Clear();
        BusinessLogic _bl = new BusinessLogic();
        List<Procedure> _procedures = new List<Procedure>();
        string[] tokens = _bodypart.Split('_');
        string Position = "";
        if (tokens[1] != null)
        {
            if (tokens[1] == "L") { Position = "Left"; }
            else if (tokens[1] == "R") { Position = "Right"; }
        }
        //if (_bodypart == OTHER)
        //{ _procedures = _bl.GetProceduresByBody("Other"); }
        //else
        //{

        _procedures = GetProceduresByBody(tokens[0], Position).Where(s => s.DateType == _datetype).Distinct<Procedure>(new ProcedureEqualityComparer()).ToList<Procedure>();
        //}

        ddProcedure.Items.Clear();
        ddProcedure.Items.Add(new ListItem("Please Select", "0"));
        foreach (Procedure _procedure in _procedures)
        {
            string MCodeDetails = string.Empty;
            //if (_bodypart == OTHER)
            //{ MCodeDetails = _procedure.DateType + "," + _procedure.Heading; }
            //else
            //{
            MCodeDetails = _procedure.MCode + "," + _procedure.Heading;
            //}

            ddProcedure.Items.Add(new ListItem(MCodeDetails, _procedure.ProcedureId.ToString()));
        }
    }

    protected void bindSubProcedures(long _procedureId, string MCODE, string bodypart)
    {
        lblAlert.Text = "";
        BusinessLogic _bl = new BusinessLogic();
        List<SubProcedure> _subprocedures = _bl.getSubProcedure(_procedureId).Distinct<SubProcedure>(new SubProcedureEqualityComparer()).ToList<SubProcedure>();
        List<string> _alreadyexistsprocedures = (_bl.getProcedureDetail(Convert.ToInt64(hfPatientIE_ID.Value), Convert.ToInt64(hfPatientFU_ID.Value), MCODE, bodypart).Where(s => s.DateType == rblDateType.SelectedValue).Select(s => s.SubProcedure)).Distinct<string>().ToList<string>();
        ddSubProcedure.Items.Clear();
        //if (_subprocedures.Count-_alreadyexistsprocedures.Count > 0)
        ddSubProcedure.Items.Add(new ListItem("Please Select", "0"));
        foreach (SubProcedure _subprocedure in _subprocedures)
        {
            //if (!_alreadyexistsprocedures.Contains(_subprocedure.SubProcedureName))
            ddSubProcedure.Items.Add(new ListItem(_subprocedure.SubProcedureName, _subprocedure.Id.ToString()));
        }

        btnSave.Enabled = (_subprocedures.Count <= 0);


    }

    protected void loadProcedureDetails()
    {

        //Response.Write(tvPatients.Nodes.Count);
        pnlProcedures.Controls.Clear();
        // ViewState["tblProcedures"] = null;

        //maintable
        Table _tb = new Table();
        _tb.ID = "tblProceduresTable";
        // _tb.CssClass = "text-center border";

        TableCell _tc = new TableCell();
        _tc.ID = "tcBodyPart";

        BusinessLogic _bl = new BusinessLogic();
        long _patientIEID = Convert.ToInt64(hfPatientIE_ID.Value);
        long _patientFUID = Convert.ToInt64(hfPatientFU_ID.Value);


        if (Session["UserID"] != null)
            hfUserID.Value = Session["UserID"].ToString();

        List<string> _injured = _bl.getInjuredParts(_patientIEID).Distinct<string>().ToList<string>();
        List<string> _injuredBodyParts = new List<string>();
        bindBodyParts(_injured);
        List<string> _filters = new List<string>(){
                "Left",
                "Right"
            };
        foreach (string _injuredpart in _injured)
        {
            if (_injuredpart == "Neck")
            {
                string sqlcmd;
                if (_patientFUID == 0)
                {
                    sqlcmd = "Select DiagCervialBulgeDate, Other1Date, Other2Date,Other3Date,Other4Date,Other5Date,Other6Date,Other7Date,Other1Study, Other2Study,Other3Study,Other4Study,Other5Study,Other6Study,Other7Study from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + _patientIEID;
                }
                else
                {
                    sqlcmd = "Select DiagCervialBulgeDate, Other1Date, Other2Date,Other3Date,Other4Date,Other5Date,Other6Date,Other7Date,Other1Study, Other2Study,Other3Study,Other4Study,Other5Study,Other6Study,Other7Study from tblFUPatientFUDetailPage1 WHERE PatientFU_ID = " + _patientFUID;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlcmd, con);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        lblNeckMRI.Text = (sdr["DiagCervialBulgeDate"] != DBNull.Value) ? Convert.ToDateTime(sdr["DiagCervialBulgeDate"]).ToString("MM/dd/yyyy") : lblNeckMRI.Text;
                        if (sdr["Other1Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other1Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other1Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }
                        else if (sdr["Other2Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other2Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other2Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }
                        else if (sdr["Other3Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other3Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other3Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }
                        else if (sdr["Other4Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other4Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other4Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }
                        else if (sdr["Other5Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other5Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other5Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }
                        else if (sdr["Other6Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other6Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other6Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }
                        else if (sdr["Other7Study"].ToString().Contains("UE"))
                        {
                            lblNeckUE.Text = (sdr["Other7Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other7Date"]).ToString("MM/dd/yyyy") : lblNeckUE.Text;
                        }

                    }
                }
            }
            else if (_injuredpart == "MidBack")
            {
                string sqlcmd;
                if (_patientFUID == 0)
                {
                    sqlcmd = "Select DiagThoracicBulgeDate, Other1Date, Other2Date,Other3Date,Other4Date,Other5Date,Other6Date,Other7Date,Other1Study, Other2Study,Other3Study,Other4Study,Other5Study,Other6Study,Other7Study from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + _patientIEID;
                }
                else
                {
                    sqlcmd = "Select DiagThoracicBulgeDate,Other1Date, Other2Date,Other3Date,Other4Date,Other5Date,Other6Date,Other7Date,Other1Study, Other2Study,Other3Study,Other4Study,Other5Study,Other6Study,Other7Study from tblFUPatientFUDetailPage1 WHERE PatientFU_ID = " + _patientFUID;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlcmd, con);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        lblMidBackMRI.Text = (sdr["DiagThoracicBulgeDate"] != DBNull.Value) ? Convert.ToDateTime(sdr["DiagThoracicBulgeDate"]).ToString("MM/dd/yyyy") : lblMidBackMRI.Text;
                        if (sdr["Other1Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other1Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other1Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }
                        else if (sdr["Other2Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other2Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other2Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }
                        else if (sdr["Other3Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other3Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other3Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }
                        else if (sdr["Other4Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other4Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other4Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }
                        else if (sdr["Other5Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other5Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other5Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }
                        else if (sdr["Other6Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other6Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other6Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }
                        else if (sdr["Other7Study"].ToString().Contains("LE"))
                        {
                            lblMidBackLE.Text = (sdr["Other7Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other7Date"]).ToString("MM/dd/yyyy") : lblMidBackLE.Text;
                        }

                    }
                }
            }
            else if (_injuredpart == "LowBack")
            {
                string sqlcmd;
                if (_patientFUID == 0)
                {
                    sqlcmd = "Select DiagLumberBulgeDate, Other1Date, Other2Date,Other3Date,Other4Date,Other5Date,Other6Date,Other7Date,Other1Study, Other2Study,Other3Study,Other4Study,Other5Study,Other6Study,Other7Study from tblPatientIEDetailPage3 WHERE PatientIE_ID = " + _patientIEID;
                }
                else
                {
                    sqlcmd = "Select DiagLumberBulgeDate, Other1Date, Other2Date,Other3Date,Other4Date,Other5Date,Other6Date,Other7Date,Other1Study, Other2Study,Other3Study,Other4Study,Other5Study,Other6Study,Other7Study from tblFUPatientFUDetailPage1 WHERE PatientFU_ID = " + _patientFUID;
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlcmd, con);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        lblLowBackMRI.Text = (sdr["DiagLumberBulgeDate"] != DBNull.Value) ? Convert.ToDateTime(sdr["DiagLumberBulgeDate"]).ToString("MM/dd/yyyy") : lblLowBackMRI.Text;
                        if (sdr["Other1Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other1Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other1Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }
                        else if (sdr["Other2Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other2Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other2Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }
                        else if (sdr["Other3Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other3Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other3Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }
                        else if (sdr["Other4Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other4Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other4Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }
                        else if (sdr["Other5Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other5Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other5Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }
                        else if (sdr["Other6Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other6Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other6Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }
                        else if (sdr["Other7Study"].ToString().Contains("LE"))
                        {
                            lblLowBackLE.Text = (sdr["Other7Date"] != DBNull.Value) ? Convert.ToDateTime(sdr["Other7Date"]).ToString("MM/dd/yyyy") : lblLowBackLE.Text;
                        }

                    }
                }
            }
            string _trimmed = _injuredpart;
            foreach (string _filter in _filters)
            {
                _trimmed = _trimmed.Replace(_filter, string.Empty);
            }
            if (!string.IsNullOrEmpty(_trimmed))
                _injuredBodyParts.Add(_trimmed);
        }
        _injuredBodyParts = _injuredBodyParts.Distinct<string>().ToList<string>();
        // bindBodyParts(_injuredBodyParts);

        #region Bind Injured Body Parts
        foreach (string _injuredbodypart in _injuredBodyParts)
        {
            List<Procedure> _procedures = GetProceduresByBody(_injuredbodypart, "");
            bool _istitletobebind = true;
            //Row to display a body part details completely
            //TableRow _trBodyPart = new TableRow();
            //TableCell _tcBodyPart = new TableCell();

            TableRow _trBodyPartTitle = new TableRow();
            TableCell _tcBodyPartTitle = new TableCell(); //to display bodypart name
            _tcBodyPartTitle.ID = _injuredbodypart;
            _tcBodyPartTitle.Text = _injuredbodypart;
            _tcBodyPartTitle.CssClass = "title bottomborder topalign";
            _trBodyPartTitle.Cells.Add(_tcBodyPartTitle);
            // _tb.Rows.Add(_trBodyPartTitle);


            #region Intialize colors
            Dictionary<long?, Color> _creatorcolors = new Dictionary<long?, Color>();
            List<Color> _colors = new List<Color>(){
                    Color.LightPink,
                    Color.LightSkyBlue,
                    Color.LightCoral,
                    Color.LightGreen,
                    Color.LightGray,
                    Color.LightCyan,
                    Color.LightSeaGreen,
                    Color.LightGoldenrodYellow
                };
            List<long> _allUserIDs = _bl.getAllUserIDs().Where(s => s != Convert.ToInt64(hfUserID.Value)).ToList<long>();
            for (int i = 0; i < _allUserIDs.Count() && i < 8; i++)
            {
                _creatorcolors.Add(_allUserIDs[i], _colors[i]);
            }
            _creatorcolors.Add(Convert.ToInt64(hfUserID.Value), Color.White);
            #endregion

            #region Bind Procedures
            TableRow _temp = new TableRow();
            foreach (string _proc in _procedures.Distinct<Procedure>(new ProcedureEqualityComparer()).Select(s => s.MCode).ToList<string>())
            {

                TableRow _trProcedure = new TableRow();

                TableRow _trRequested = new TableRow();
                TableRow _trExecuted = new TableRow();
                TableRow _trScheduled = new TableRow();
                Table _tbProcedure = new Table();
                TableCell _tcProcedure = new TableCell();


                //TableCell _tcs = new TableCell();//cell to display procedure MCODE
                //_tcs.Text = _procedure.MCode;
                //_tcs.ID = _injuredbodypart + _procedure.MCode;

                //TableRow _trSubProcedure = new TableRow();
                //TableCell _tcSubProcedure = new TableCell();


                //table to display subprocedures names
                //if (_proc.Contains(Session["_proc"] != null ? Session["_proc"].ToString() : ""))
                //{
                //    if (Session["_proc"] == null)
                //    {
                //        _tbProcedure.CssClass = "text-center border caption-align";
                //        _tbProcedure.ID = _injuredbodypart + "_" + _proc;
                //        _tbProcedure.Caption = _proc;
                //    }
                //    else
                //    {

                //    }
                //}
                //else
                //{

                //    _tbProcedure.CssClass = "text-center border caption-align";
                //    _tbProcedure.ID = _injuredbodypart + "_" + _proc;
                //    _tbProcedure.Caption = _proc;
                //}

                //to display the subprocedures & mcode
                TableRow _trTitles = new TableRow();
                TableCell _trTitleLabel = new TableCell();
                //_trTitleLabel.Text = _procedure.MCode;
                _trTitles.Cells.Add(_trTitleLabel);


                //row to display requested
                TableCell _tcRequestedLabel = new TableCell();
                _tcRequestedLabel.Text = "  R ";
                _tcRequestedLabel.ID = _injuredbodypart + _proc + "RequestedLabel";
                _trRequested.Cells.Add(_tcRequestedLabel);

                //row to display scheduled

                TableCell _tcScheduledLabel = new TableCell();
                _tcScheduledLabel.Text = "  S ";
                _tcScheduledLabel.ID = _injuredbodypart + _proc + "ScheduledLabel";
                _trScheduled.Cells.Add(_tcScheduledLabel);

                //row to display executed

                TableCell _tcExecutedLabel = new TableCell();
                _tcExecutedLabel.Text = "  E ";
                _tcExecutedLabel.ID = _injuredbodypart + _proc + "ExecutedLabel";
                _trExecuted.Cells.Add(_tcExecutedLabel);

                List<ProcedureDetail> _proceduredetails = _bl.getProcedureDetail(_patientIEID, _patientFUID, _proc, _injuredbodypart);

                foreach (string _subProcedure in _proceduredetails.Distinct(new subProcedureEqualityComparer()).Select(s => s.SubProcedure).ToList<string>())
                {

                    //cell for subprocedure title
                    List<ProcedureDetail> _pdBySubProcedures = _proceduredetails.Where(s => s.SubProcedure == _subProcedure).ToList<ProcedureDetail>();
                    TableCell _tcSubProcedureTitle = new TableCell();
                    long ProcedureDetailR_ID = 0;
                    long ProcedureDetailS_ID = 0;
                    long ProcedureDetailE_ID = 0;

                    ProcedureDetail _pdRequested = _pdBySubProcedures.Where(s => s.DateType == "R").FirstOrDefault<ProcedureDetail>();
                    ProcedureDetail _pdScheduled = _pdBySubProcedures.Where(s => s.DateType == "S").FirstOrDefault<ProcedureDetail>();
                    ProcedureDetail _pdExecuted = _pdBySubProcedures.Where(s => s.DateType == "E").FirstOrDefault<ProcedureDetail>();

                    ProcedureDetailR_ID = (_pdRequested != null) ? _pdRequested.ProcedureDetail_ID : ProcedureDetailR_ID;
                    ProcedureDetailS_ID = (_pdScheduled != null) ? _pdScheduled.ProcedureDetail_ID : ProcedureDetailS_ID;
                    ProcedureDetailE_ID = (_pdExecuted != null) ? _pdExecuted.ProcedureDetail_ID : ProcedureDetailE_ID;
                    long Procedure_ID = 0;
                    Procedure_ID = (_pdRequested != null) ? _pdRequested.Procedure_ID : Procedure_ID;
                    Procedure_ID = (_pdScheduled != null) ? _pdScheduled.Procedure_ID : Procedure_ID;
                    Procedure_ID = (_pdExecuted != null) ? _pdExecuted.Procedure_ID : Procedure_ID;
                    var id = _proceduredetails.Where(m => m.Procedure_ID == Procedure_ID).Select(m => m.MCODE).FirstOrDefault();
                    _tcSubProcedureTitle.Text = id;
                    if (_proc == ((_pdRequested != null ? _pdRequested.MCODE : "")))
                    {
                        //Session["_proc"] = "";
                        // if ((Session["_proc"] != null ? Session["_proc"].ToString() : "").Contains(_proc))
                        if (_proc == (Session["_proc"] != null ? Session["_proc"].ToString() : ""))
                        {
                            Session["_proc"] = (Session["_proc"] != null ? Session["_proc"].ToString() : "");
                        }
                        else
                        {
                            Session["_proc"] = _proc;
                        }
                    }
                    if (_proc == (_pdScheduled != null ? _pdScheduled.MCODE : ""))
                    {
                        // Session["_proc"] = "";
                        //if ((Session["_proc"] != null ? Session["_proc"].ToString() : "").Contains(_proc))
                        if (_proc == (Session["_proc"] != null ? Session["_proc"].ToString() : ""))
                        {
                            Session["_proc"] = (Session["_proc"] != null ? Session["_proc"].ToString() : "");
                        }
                        else
                        {
                            Session["_proc"] = _proc;
                        }
                    }
                    if (_proc == (_pdExecuted != null ? _pdExecuted.MCODE : ""))
                    {
                        //Session["_proc"] = "";
                        if (_proc == (Session["_proc"] != null ? Session["_proc"].ToString() : ""))
                        {
                            Session["_proc"] = (Session["_proc"] != null ? Session["_proc"].ToString() : "");
                        }
                        else
                        {
                            Session["_proc"] = _proc;
                        }
                    }
                    //cell for requested
                    //if (_proc.Contains((_pdRequested != null ? _pdRequested.MCODE : "")))
                    //{

                    if (_proc != "")
                    {
                        if ((Session["_proc"] != null ? Session["_proc"].ToString() : "").Contains(_proc))
                        {
                            Table Subtbl = new Table();
                            TableRow SubRow = new TableRow();
                            //TableCell SubProc = new TableCell();                           
                            //SubProc.Controls.Add(_tbProcedure);
                            ////SubRow.Cells.Add(SubProc);

                            TableCell _tcSubProcedureRequested = new TableCell();
                            TextBox _txtSubProcedureRequested = new TextBox();
                            _txtSubProcedureRequested.ID = Procedure_ID + "_" + _patientIEID + "_" + _patientFUID + "_" + _proc + "_" + "R" + "_" + _subProcedure + "_" + _injuredbodypart + "_" + ProcedureDetailR_ID;
                            _txtSubProcedureRequested.Text = (_pdRequested != null && _pdRequested.Date != null) ? ((DateTime)_pdRequested.Date).ToString("MM/dd/yyyy") : "";
                            if (_pdRequested != null && _pdRequested.CreatedBy != null)
                                _txtSubProcedureRequested.BackColor = (_creatorcolors.ContainsKey(_pdRequested.CreatedBy)) ? _creatorcolors[_pdRequested.CreatedBy] : Color.Red;
                            _txtSubProcedureRequested.CssClass = "date bottomborder text-center";
                            _txtSubProcedureRequested.Width = Unit.Pixel(90);
                            _tcSubProcedureRequested.Controls.Add(_txtSubProcedureRequested);
                            _tcSubProcedureRequested.Width = Unit.Pixel(90);
                            _trRequested.Cells.Add(_tcSubProcedureRequested);
                            //Subtbl.Rows.Add(_trRequested);
                            ////}
                            ////else
                            ////{
                            ////     TableCell _tcSubProcedureRequested = new TableCell();
                            ////    TextBox _txtSubProcedureRequested = new TextBox();
                            ////    _txtSubProcedureRequested.ID = Procedure_ID + "_" + _patientIEID + "_" + _patientFUID + "_" + _proc + "_" + "R" + "_" + _subProcedure + "_" + _injuredbodypart;
                            ////    //_txtSubProcedureRequested.Text = (_pdRequested != null && _pdRequested.Date != null) ? ((DateTime)_pdRequested.Date).ToString("MM/dd/yyyy") : "";
                            ////    if (_pdRequested != null && _pdRequested.CreatedBy != null)
                            ////        _txtSubProcedureRequested.BackColor = (_creatorcolors.ContainsKey(_pdRequested.CreatedBy)) ? _creatorcolors[_pdRequested.CreatedBy] : Color.Red;
                            ////    _txtSubProcedureRequested.CssClass = "date bottomborder text-center";
                            ////    _txtSubProcedureRequested.Width = Unit.Pixel(90);
                            ////    _tcSubProcedureRequested.Controls.Add(_txtSubProcedureRequested);
                            ////    _tcSubProcedureRequested.Width = Unit.Pixel(90);
                            ////    _trRequested.Cells.Add(_tcSubProcedureRequested);
                            ////}
                            ////if (_proc.Contains(_pdScheduled != null ?_pdScheduled.MCODE : ""))
                            ////{
                            ////    //cell for scheduled
                            TableCell _tcSubProcedureScheduled = new TableCell();
                            TextBox _txtSubProcedureScheduled = new TextBox();
                            _txtSubProcedureScheduled.ID = Procedure_ID + "_" + _patientIEID + "_" + _patientFUID + "_" + _proc + "_" + "S" + "_" + _subProcedure + "_" + _injuredbodypart + "_" + ProcedureDetailS_ID;
                            _txtSubProcedureScheduled.Text = (_pdScheduled != null && _pdScheduled.Date != null) ? ((DateTime)_pdScheduled.Date).ToString("MM/dd/yyyy") : "";
                            _txtSubProcedureScheduled.CssClass = "date bottomborder text-center";
                            _txtSubProcedureScheduled.Width = Unit.Pixel(90);
                            if (_pdScheduled != null && _pdScheduled.CreatedBy != null)
                                _txtSubProcedureScheduled.BackColor = (_creatorcolors.ContainsKey(_pdScheduled.CreatedBy)) ? _creatorcolors[_pdScheduled.CreatedBy] : Color.Red;
                            _tcSubProcedureScheduled.Controls.Add(_txtSubProcedureScheduled);
                            _trScheduled.Cells.Add(_tcSubProcedureScheduled);
                            //Subtbl.Rows.Add(_trScheduled);
                            //}
                            //else
                            //{
                            //    TableCell _tcSubProcedureScheduled = new TableCell();
                            //    TextBox _txtSubProcedureScheduled = new TextBox();
                            //    _txtSubProcedureScheduled.ID = Procedure_ID + "_" + _patientIEID + "_" + _patientFUID + "_" + _proc + "_" + "S" + "_" + _subProcedure + "_" + _injuredbodypart;
                            //    //_txtSubProcedureScheduled.Text = (_pdScheduled != null && _pdScheduled.Date != null) ? ((DateTime)_pdScheduled.Date).ToString("MM/dd/yyyy") : "";
                            //    _txtSubProcedureScheduled.CssClass = "date bottomborder text-center";
                            //    _txtSubProcedureScheduled.Width = Unit.Pixel(90);
                            //    if (_pdScheduled != null && _pdScheduled.CreatedBy != null)
                            //        _txtSubProcedureScheduled.BackColor = (_creatorcolors.ContainsKey(_pdScheduled.CreatedBy)) ? _creatorcolors[_pdScheduled.CreatedBy] : Color.Red;
                            //    _tcSubProcedureScheduled.Controls.Add(_txtSubProcedureScheduled);
                            //    _trScheduled.Cells.Add(_tcSubProcedureScheduled);
                            //}
                            //if (_proc.Contains(_pdExecuted != null ?_pdExecuted.MCODE : ""))
                            //{
                            //cell for executed
                            TableCell _tcSubProcedureExecuted = new TableCell();
                            TextBox _txtSubProcedureExecuted = new TextBox();

                            _txtSubProcedureExecuted.ID = Procedure_ID + "_" + _patientIEID + "_" + _patientFUID + "_" + _proc + "_" + "E" + "_" + _subProcedure + "_" + _injuredbodypart + "_" + ProcedureDetailE_ID;
                            _txtSubProcedureExecuted.Text = (_pdExecuted != null && _pdExecuted.Date != null) ? ((DateTime)_pdExecuted.Date).ToString("MM/dd/yyyy") : "";
                            _txtSubProcedureExecuted.Width = Unit.Pixel(90);
                            if (_pdExecuted != null && _pdExecuted.CreatedBy != null)
                                _txtSubProcedureExecuted.BackColor = (_creatorcolors.ContainsKey(_pdExecuted.CreatedBy)) ? _creatorcolors[_pdExecuted.CreatedBy] : Color.Red;
                            _txtSubProcedureExecuted.CssClass = "date bottomborder text-center";
                            _tcSubProcedureExecuted.Controls.Add(_txtSubProcedureExecuted);
                            _trExecuted.Cells.Add(_tcSubProcedureExecuted);
                            //Subtbl.Rows.Add(_trExecuted);

                        }
                    }//}
                    //else
                    //{
                    //    //cell for executed
                    //    TableCell _tcSubProcedureExecuted = new TableCell();
                    //    TextBox _txtSubProcedureExecuted = new TextBox();

                    //    _txtSubProcedureExecuted.ID = Procedure_ID + "_" + _patientIEID + "_" + _patientFUID + "_" + _proc + "_" + "E" + "_" + _subProcedure + "_" + _injuredbodypart;
                    //   // _txtSubProcedureExecuted.Text = (_pdExecuted != null && _pdExecuted.Date != null) ? ((DateTime)_pdExecuted.Date).ToString("MM/dd/yyyy") : "";
                    //    _txtSubProcedureExecuted.Width = Unit.Pixel(90);
                    //    if (_pdExecuted != null && _pdExecuted.CreatedBy != null)
                    //        _txtSubProcedureExecuted.BackColor = (_creatorcolors.ContainsKey(_pdExecuted.CreatedBy)) ? _creatorcolors[_pdExecuted.CreatedBy] : Color.Red;
                    //    _txtSubProcedureExecuted.CssClass = "date bottomborder text-center";
                    //    _tcSubProcedureExecuted.Controls.Add(_txtSubProcedureExecuted);
                    //    _trExecuted.Cells.Add(_tcSubProcedureExecuted);
                    //}
                    //add cells to their coresponding rows
                    _trTitles.Cells.Add(_tcSubProcedureTitle);




                }

                //add rows to subprocedure table


                //add subprocedure table to main table
                if (_trRequested.Cells.Count > 1 || _trScheduled.Cells.Count > 1 || _trExecuted.Cells.Count > 1)
                {
                    _tbProcedure.Rows.Add(_trTitles);
                    _tbProcedure.Rows.Add(_trRequested);
                    _tbProcedure.Rows.Add(_trScheduled);
                    _tbProcedure.Rows.Add(_trExecuted);
                    _tcProcedure.Controls.Add(_tbProcedure);
                    _temp.Cells.Add(_tcProcedure);
                    if (_istitletobebind)
                    {
                        _tb.Rows.Add(_trBodyPartTitle);
                        _istitletobebind = false;
                    }

                }
                else
                {
                    _tbProcedure.Rows.Clear();
                }
                //_trBodyPartProcedures.Cells.Add(_tcs);

                //.Controls.Add(_tbProcedure);*/
            }
            _tb.Rows.Add(_temp);

            #endregion

            // _tb.Rows.Add(_trBodyPart);
            // _tb.Rows.Add(_trBodyPartProcedures);


            #region OldCode
            //if (_proceduredetail.CreatedBy != null)
            //    if (!_creatorcolors.ContainsKey(_proceduredetail.CreatedBy))
            //    {
            //        _creatorcolors.Add(_proceduredetail.CreatedBy, _colors.ElementAt(_creatorcolors.Count));
            //    }

            //TableCell _tcRequested = new TableCell();
            //_tcRequested.ID = "Requested_" + _procedure.MCode;
            //TextBox txtDateRequested = new TextBox();
            //txtDateRequested.ID = "R_" + _procedure.ProcedureId + "_" + _patientFUID + "_" + _procedure.MCode + "_" + _injuredbodypart;
            //if (_proceduredetail.Requested.HasValue)
            //{
            //    txtDateRequested.Text = ((DateTime)_proceduredetail.Requested).ToString("MM/dd/yyyy");
            //    //txtDateRequested.BackColor = _creatorcolors[_proceduredetail.CreatedBy];
            //}
            //txtDateRequested.CssClass = "date bottomborder text-center";
            //_tcRequested.Controls.Add(txtDateRequested);
            //_trRequested.Cells.Add(_tcRequested);

            //TableCell _tcScheduled = new TableCell();
            //_tcScheduled.ID = "Scheduled_" + _procedure.MCode;
            //TextBox txtDateScheduled = new TextBox();
            //txtDateScheduled.ID = "S_" + _procedure.ProcedureId + "_" + _patientFUID + "_" + _procedure.MCode + "_" + _injuredbodypart;
            //if (_proceduredetail.Scheduled.HasValue)
            //{
            //    txtDateScheduled.Text = ((DateTime)_proceduredetail.Scheduled).ToString("MM/dd/yyyy");
            //    //txtDateScheduled.BackColor = _creatorcolors[_proceduredetail.CreatedBy];
            //}
            //txtDateScheduled.CssClass = "date bottomborder text-center";
            //_tcScheduled.Controls.Add(txtDateScheduled);
            //_trScheduled.Cells.Add(_tcScheduled);

            //TableCell _tcExecuted = new TableCell();
            //_tcExecuted.ID = "Executed_" + _procedure.MCode;
            //TextBox txtDateExecuted = new TextBox();
            //txtDateExecuted.ID = "E_" + _procedure.ProcedureId + "_" + _patientFUID + "_" + _procedure.MCode + "_" + _injuredbodypart;
            //if (_proceduredetail.Executed.HasValue)
            //{
            //    txtDateExecuted.Text = ((DateTime)_proceduredetail.Executed).ToString("MM/dd/yyyy");
            //    //txtDateExecuted.BackColor = _creatorcolors[_proceduredetail.CreatedBy];
            //}
            //txtDateExecuted.CssClass = "date noborder text-center";
            //_tcExecuted.Controls.Add(txtDateExecuted);
            //_trExecuted.Cells.Add(_tcExecuted); 
            #endregion
        }
        #endregion


        //if (_tr.Cells.Count > 1)
        //    _tb.Rows.Add(_tr);

        //if (_trRequested.Cells.Count > 1)
        //    _tb.Rows.Add(_trRequested);
        //if (_trScheduled.Cells.Count > 1)
        //    _tb.Rows.Add(_trScheduled);
        //if (_trExecuted.Cells.Count > 1)
        //    _tb.Rows.Add(_trExecuted);

        pnlProcedures.Controls.Add(_tb);

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

    protected List<TreeNode> loadPatientsToTreeNode(List<PatientsByDOE_Result> _patients, bool isFromQueryString = false)
    {
        List<TreeNode> treeNodeList = new List<TreeNode>();
        foreach (PatientsByDOE_Result _patient in _patients)
        {
            TreeNode _tnpPatient = new TreeNode();
            _tnpPatient.Text = _patient.LastName + ", " + _patient.FirstName;
            _tnpPatient.Value = _patient.PatientFUId.ToString() + "_" + _patient.PatientIEId.ToString();

            TreeNode _tnCompensation = new TreeNode();
            _tnCompensation.Text = (_patient.Compensation != null) ? _patient.Compensation : "";
            _tnpPatient.ChildNodes.Add(_tnCompensation);
            _tnCompensation.SelectAction = TreeNodeSelectAction.None;

            TreeNode _tnLocation = new TreeNode();
            //_tnLocation.Text = (((_patient.City != null) ? _patient.City : "") + "," + ((_patient.State != null) ? _patient.State : "")).TrimEnd(',');
            _tnLocation.Text = (_patient.Location != null) ? _patient.Location : "";
            _tnpPatient.ChildNodes.Add(_tnLocation);
            _tnLocation.SelectAction = TreeNodeSelectAction.None;

            TreeNode _tncDOA = new TreeNode();
            _tncDOA.Text = (_patient.DOA != null) ? ((DateTime)_patient.DOA).ToShortDateString() : "";
            _tncDOA.Value = null;
            _tnpPatient.ChildNodes.Add(_tncDOA);
            _tncDOA.SelectAction = TreeNodeSelectAction.None;

            //TreeNode _tncPatientFUId = new TreeNode();
            //_tncPatientFUId.Text = _patient.PatientFUId.ToString();
            //_tnpPatient.ChildNodes.Add(_tncPatientFUId);
            //_tncPatientFUId.SelectAction = TreeNodeSelectAction.None;

            //TreeNode _tncPatientIEId = new TreeNode();
            //_tncPatientIEId.Text = _patient.PatientIEId.ToString();
            //_tnpPatient.ChildNodes.Add(_tncPatientIEId);
            //_tncPatientIEId.SelectAction = TreeNodeSelectAction.None;

            TreeNode _tncType = new TreeNode();
            _tncType.Text = _patient.ExamType;
            _tncType.Value = null;
            _tnpPatient.ChildNodes.Add(_tncType);
            _tncType.SelectAction = TreeNodeSelectAction.None;

            TreeNode _tncDOE = new TreeNode();
            _tncDOE.Text = "DOE :" + _patient.DOE.ToShortDateString();
            _tncDOE.Value = null;
            _tnpPatient.ChildNodes.Add(_tncDOE);
            _tncDOE.SelectAction = TreeNodeSelectAction.None;
            treeNodeList.Add(_tnpPatient);
        }
        return treeNodeList;
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        BusinessLogic _bl = new BusinessLogic();
        tvPatients.Nodes.Clear();
        lblDName.Text = "";
        lblDDOA.Text = "";
        lblPDLocation.Text = "";
        lblDCaseType.Text = "";
        ltNew.Text = "";
        List<PatientsByDOE_Result> _patients = _bl.getPatientsByDOE(Convert.ToDateTime(ddDate.SelectedValue), ddLocation.SelectedValue);
        foreach (TreeNode tn in loadPatientsToTreeNode(_patients))
        {
            tvPatients.Nodes.Add(tn);
        }
    }

    protected void tvPatients_SelectedNodeChanged(object sender, EventArgs e)
    {
        lblDName.Text = "";
        lblDDOA.Text = "";
        lblPDLocation.Text = "";
        lblDCaseType.Text = "";
        ltNew.Text = "<button type='button' class='btn btn-default top-right' data-toggle='modal' id='New' data-target='#ProcedureDetailModal'>New</button>";

        string[] _ids = tvPatients.SelectedValue.Split('_');
        TreeNodeCollection tvc = tvPatients.SelectedNode.ChildNodes;
        lblDName.Text = tvPatients.SelectedNode.Text;
        lblDDOA.Text = tvc[2].Text;
        lblPDLocation.Text = tvc[1].Text;
        lblDCaseType.Text = tvc[0].Text;
        EnableAllTreeNodes();

        tvPatients.SelectedNode.SelectAction = TreeNodeSelectAction.None;
        hfPatientIE_ID.Value = _ids[1];
        hfPatientFU_ID.Value = _ids[0];

        loadProcedureDetails();
    }

    protected void EnableAllTreeNodes()
    {
        for (int i = 0; i < tvPatients.Nodes.Count; i++)
        {
            if (tvPatients.Nodes[i].Value != null)
                tvPatients.Nodes[i].SelectAction = TreeNodeSelectAction.Select;
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Login.aspx");
    }

    protected void ddBodyPart_SelectedIndexChanged(object sender, EventArgs e)
    {
        rblDateType.SelectedIndex = -1;
        //if (ddBodyPart.SelectedValue == OTHER)
        //{
        //    ddProcedure.Items.Clear();
        //    bindProcedures(ddBodyPart.SelectedValue, "");
        //    ddProcedure.Enabled = ddProcedure.Items.Count > 1;
        //    ddSubProcedure.Enabled = false;
        //    rblDateType.Enabled = false;
        //    //txtDate.Enabled = false;
        //    txtNumber.Enabled = false;
        //}
        //else
        //{
        rblDateType.Enabled = true;
        ddProcedure.Items.Clear();
        ddProcedure.Enabled = false;
        ddSubProcedure.Items.Clear();
        ddSubProcedure.Enabled = false;
        //txtDate.Enabled = false;
        txtNumber.Enabled = false;
        // }
    }

    protected void ddProcedure_SelectedIndexChanged(object sender, EventArgs e)
    {
        BusinessLogic _bl = new BusinessLogic();
        if (ddProcedure.SelectedIndex > 0)
        {
            string str = ddProcedure.SelectedItem.Text.Split(',').First();
            //ddProcedure.SelectedItem.Text
            txtNumber.Text = (_bl.GetProceduresCount(str, Convert.ToInt64(hfPatientIE_ID.Value)) + 1).ToString();
        }
        else
            txtNumber.Text = string.Empty;

        bindSubProcedures(Convert.ToInt64(ddProcedure.SelectedValue), ddProcedure.SelectedItem.Text, ddBodyPart.SelectedItem.Text);
        ddSubProcedure.Enabled = ddSubProcedure.Items.Count > 1;
        txtNumber.Enabled = false;
        // txtDate.Enabled = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string _finalString = string.Empty;
            string _mcode = string.Empty;
            if (ddSubProcedure.SelectedIndex > 0)
            {
                _finalString = ddSubProcedure.SelectedItem.Text + txtNumber.Text.Trim();
            }
            else
            {
                _finalString = txtNumber.Text.Trim();
            }
            if (!string.IsNullOrEmpty(rblDateType.Text))
            { _mcode = ddProcedure.SelectedItem.Text.Split(',').First() + "_" + rblDateType.SelectedValue + "_" + _finalString; }
            else
            { _mcode = ddProcedure.SelectedItem.Text.Split(',').First(); }

            DateTime date1 = Convert.ToDateTime(txtDate.Text);
            BusinessLogic _bl = new BusinessLogic();
            //string[] s= .Split("_");
            string[] tokens = ddBodyPart.SelectedValue.Split('_');
            string Position = "";
            if (tokens[1] != null)
            {
                if (tokens[1] == "L") { Position = "Left"; }
                else if (tokens[1] == "R") { Position = "Right"; }
            }
            string count = _bl.saveProcedureDetail(
                Convert.ToInt64(ddProcedure.SelectedValue),
                Convert.ToInt64(hfPatientIE_ID.Value),
                Convert.ToInt64(hfPatientFU_ID.Value),
                _mcode,
                tokens[0],
                // ddBodyPart.SelectedValue,
                Convert.ToDateTime(date1),
                //((txtDate.Text != "") ? (DateTime?)Convert.ToDateTime(txtDate.Text.Split('/')[1] + "/" + txtDate.Text.Split('/')[0] + "/" + txtDate.Text.Split('/')[2]) : null),
               "",//will automatically inserted from sp
                Convert.ToInt64(hfUserID.Value), 0,
                true
                );
            if (count != "0")
            {
                clear();
                lblAlert.Text = "Saved Successfully";
                lblAlert.ForeColor = Color.Green;
                loadProcedureDetails();
            }
            else
            {
                lblAlert.Text = "Failed to Save";
                lblAlert.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void clear()
    {
        ddBodyPart.SelectedIndex = 0;
        rblDateType.SelectedIndex = -1;
        ddProcedure.Items.Clear();
        ddProcedure.Enabled = false;
        ddSubProcedure.Items.Clear();
        ddSubProcedure.Enabled = false;
        // txtDate.Enabled = false;
        //txtDate.Text = string.Empty;
        txtNumber.Text = string.Empty;
        lblAlert.Text = "";
        txtDate.Text = Session["doe"].ToString();
    }

    protected void btnHidden_Click(object sender, EventArgs e)
    {
        Response.Write(hfPatientIE_ID.Value);
        Response.Write(hfPatientFU_ID.Value);
    }

    protected void rblDateType_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindProcedures(ddBodyPart.SelectedValue, rblDateType.SelectedValue);

        ddProcedure.Enabled = ddProcedure.Items.Count > 1;
        ddSubProcedure.Enabled = false;
        // txtDate.Enabled = false;
        txtNumber.Enabled = false;
    }

    protected void btnReload_Click(object sender, EventArgs e)
    {
        loadProcedureDetails();
    }

    protected void ddSubProcedure_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtNumber.Enabled = true;
        // txtDate.Enabled = true;
        btnSave.Enabled = true;
        //string sqlcmd = "Select count(ProcedureDetail_ID) from tblProceduresDetail where MCODE like '" + ddProcedure.SelectedValue + "_" + rblDateType.SelectedValue + "_" + ddSubProcedure.SelectedValue + "'";

    }

    //protected void moveAllLeft_Click(object sender, EventArgs e)
    //{
    //    foreach (ListItem li in lbSelectedMAandProviders.Items)
    //    {
    //        lbMAandProviders.Items.Add(li);
    //    }
    //    lbSelectedMAandProviders.Items.Clear();
    //}

    //protected void moveLeft_Click(object sender, EventArgs e)
    //{
    //    for (int i = 0; i < lbSelectedMAandProviders.Items.Count; i++)
    //    {
    //        ListItem li = lbSelectedMAandProviders.Items[i];
    //        if (li.Selected)
    //        {
    //            lbMAandProviders.Items.Add(li);
    //            lbSelectedMAandProviders.Items.Remove(li);
    //        }

    //    }
    //}

    //protected void moveRight_Click(object sender, EventArgs e)
    //{
    //    for (int i = 0; i < lbMAandProviders.Items.Count; i++)
    //    {
    //        ListItem li = lbMAandProviders.Items[i];
    //        if (li.Selected)
    //        {
    //            lbSelectedMAandProviders.Items.Add(li);
    //            lbMAandProviders.Items.Remove(li);
    //        }

    //    }

    //}

    //protected void moveAllRight_Click(object sender, EventArgs e)
    //{
    //    foreach (ListItem li in lbMAandProviders.Items)
    //    {
    //        lbSelectedMAandProviders.Items.Add(li);
    //    }
    //    lbMAandProviders.Items.Clear();
    //}

    protected void btnDownloadSI_Click(object sender, EventArgs e)
    {
        if (ddLocation.SelectedIndex > 0 || ddDate.SelectedIndex > 0)
        {
            long LocationId = Convert.ToInt64((ddLocation.SelectedValue == "All") ? "0" : ddLocation.SelectedValue);
            DateTime date = Convert.ToDateTime(ddDate.SelectedValue);
            Response.Redirect(string.Format("~/SIDownloadSheet.aspx?L={0}&&D={1}", LocationId, date.ToString("s", CultureInfo.InvariantCulture)));
        }
    }
    protected void lbtnPatientDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect(prevPage);
    }
}