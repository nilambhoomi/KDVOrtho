using IntakeSheet.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FollowUpMaster : System.Web.UI.MasterPage
{
    //c
    DBHelperClass gDbhelperobj = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        lnkbtn_neck.Visible = false;
        lnkbtn_Midback.Visible = false;
        lnkbtn_lowback.Visible = false;
        lnkbtn_Shoulder.Visible = false;
        lnkbtn_Keen.Visible = false;
        lnkbtn_Elbow.Visible = false;
        lnkbtn_Wrist.Visible = false;
        lnkbtn_ankle.Visible = false;
        lnkbtn_Hip.Visible = false;
        Session["RightShoulder"] = null;
        Session["LeftShoulder"] = null;
        if (Session["patientFUId"] == null)
        {
            List<Body> b = new List<Body>(); Session["FUFUbodyPartsList"] = b;
        }

        if (Session["FirstNameFUEdit"] != null && Session["LastNameFUEdit"] != null)
        {
            NameLbl.Text = Session["FirstNameFUEdit"].ToString() + ' ' + Session["LastNameFUEdit"].ToString();
        }
        if (Session["DVLbl"] != null)
        {
            DVLbl.Text = Session["DVLbl"].ToString();
        }
        if (Session["LocLbl"] != null)
        {
            locLbl.Text = Session["LocLbl"].ToString();
        }
        if (Session["compensation"] != null)
        {
            CTLbl.Text = Session["compensation"].ToString();
        }
        if (Session["FUFUbodyPartsList"] != null)
        {
            var f = (List<Body>)Session["FUFUbodyPartsList"];

            if (f != null)
                foreach (var i in (List<Body>)Session["FUFUbodyPartsList"])
                {
                    switch (i.Part)
                    {
                        case "Ankle":
                            lnkbtn_ankle.Visible = true;
                            if (i.Position == "B") { Ankle_a.HRef = "EditFuAnkle.aspx?P=B"; } else if (i.Position == "L") { Ankle_a.HRef = "EditFuAnkle.aspx?P=L"; } else if (i.Position == "R") { Ankle_a.HRef = "EditFuAnkle.aspx?P=R"; }
                            break;
                        case "Shoulder":
                            lnkbtn_Shoulder.Visible = true;
                            if (i.Position == "B") { Shoulder_a.HRef = "EditFuShoulder.aspx?P=B"; } else if (i.Position == "L") { Shoulder_a.HRef = "EditFuShoulder.aspx?P=L"; } else if (i.Position == "R") { Shoulder_a.HRef = "EditFuShoulder.aspx?P=R"; }
                            break;

                        case "Knee":
                            lnkbtn_Keen.Visible = true;
                            if (i.Position == "B") { knee_a.HRef = "EditFuKnee.aspx?P=B"; } else if (i.Position == "L") { knee_a.HRef = "EditFuKnee.aspx?P=L"; } else if (i.Position == "R") { knee_a.HRef = "EditFuKnee.aspx?P=R"; }
                            break;

                        case "Wrist":
                            lnkbtn_Wrist.Visible = true;
                            if (i.Position == "B") { Wrist_a.HRef = "EditFuWrist.aspx?P=B"; } else if (i.Position == "L") { Wrist_a.HRef = "EditFuWrist.aspx?P=L"; } else if (i.Position == "R") { Wrist_a.HRef = "EditFuWrist.aspx?P=R"; }
                            break;

                        case "Hip":
                            lnkbtn_Hip.Visible = true;
                            if (i.Position == "B") { Hip_a.HRef = "EditFuHip.aspx?P=B"; } else if (i.Position == "L") { Hip_a.HRef = "EditFuHip.aspx?P=L"; } else if (i.Position == "R") { Hip_a.HRef = "EditFuHip.aspx?P=R"; }
                            break;

                        case "Neck":
                            lnkbtn_neck.Visible = true;
                            Neck_a.HRef = "EditFuNeck.aspx";
                            break;

                        case "MidBack":
                            lnkbtn_Midback.Visible = true;
                            Midback_a.HRef = "EditFuMidback.aspx";
                            break;

                        case "LowBack":
                            lnkbtn_lowback.Visible = true;
                            Lowback_a.HRef = "EditFuLowback.aspx";
                            break;

                        case "Elbow":
                            lnkbtn_Elbow.Visible = true;
                            if (i.Position == "B") { Elbow_a.HRef = "EditFuElbow.aspx?P=B"; } else if (i.Position == "L") { Elbow_a.HRef = "EditFuElbow.aspx?P=L"; } else if (i.Position == "R") { Elbow_a.HRef = "EditFuElbow.aspx?P=R"; }
                            break;

                    }
                }
        }
        //lbtnProcedureDetails.HRef = "EditFuProcedureDetails.aspx?PId=" + Convert.ToString(Session["PatientIE_ID"]);
    }

    protected void bindBodyParts(List<string> _injuredBodyParts)
    {
        //rblDateType.SelectedIndex = -1;
        //ddProcedure.Items.Clear();
        //ddSubProcedure.Items.Clear();
        //ddBodyPart.Items.Clear();
        //ddBodyPart.Items.Add(new ListItem("Please Select", ""));
        //ddBodyPart.Items.Add(new ListItem(OTHER, OTHER));
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
        Session["FUFUbodyPartsList"] = distinctList;
        //foreach (var i in distinctList)
        //{
        //    ddBodyPart.Items.Add(new ListItem(i.Part, i.Part + '_' + i.Position));
        //}
        foreach (var i in (List<Body>)Session["FUFUbodyPartsList"])
        {
            switch (i.Part)
            {
                case "Ankle":
                    lnkbtn_ankle.Visible = true;
                    if (i.Position == "B") { Ankle_a.HRef = "EditFuAnkle.aspx?P=B"; } else if (i.Position == "L") { Ankle_a.HRef = "EditFuAnkle.aspx?P=L"; } else if (i.Position == "R") { Ankle_a.HRef = "EditFuAnkle.aspx?P=R"; }
                    break;
                case "Shoulder":
                    lnkbtn_Shoulder.Visible = true;
                    if (i.Position == "B") { Shoulder_a.HRef = "EditFuShoulder.aspx?P=B"; } else if (i.Position == "L") { Shoulder_a.HRef = "EditFuShoulder.aspx?P=L"; } else if (i.Position == "R") { Shoulder_a.HRef = "EditFuShoulder.aspx?P=R"; }
                    break;

                case "Knee":
                    lnkbtn_Keen.Visible = true;
                    if (i.Position == "B") { knee_a.HRef = "EditFuKnee.aspx?P=B"; } else if (i.Position == "L") { knee_a.HRef = "EditFuKnee.aspx?P=L"; } else if (i.Position == "R") { knee_a.HRef = "EditFuKnee.aspx?P=R"; }
                    break;

                case "Wrist":
                    lnkbtn_Wrist.Visible = true;
                    if (i.Position == "B") { Wrist_a.HRef = "EditFuWrist.aspx?P=B"; } else if (i.Position == "L") { Wrist_a.HRef = "EditFuWrist.aspx?P=L"; } else if (i.Position == "R") { Wrist_a.HRef = "EditFuWrist.aspx?P=R"; }
                    break;

                case "Hip":
                    lnkbtn_Hip.Visible = true;
                    if (i.Position == "B") { Hip_a.HRef = "EditFuHip.aspx?P=B"; } else if (i.Position == "L") { Hip_a.HRef = "EditFuHip.aspx?P=L"; } else if (i.Position == "R") { Hip_a.HRef = "EditFuHip.aspx?P=R"; }
                    break;

                case "Neck":
                    lnkbtn_neck.Visible = true;
                    Neck_a.HRef = "EditFuNeck.aspx";
                    break;

                case "MidBack":
                    lnkbtn_Midback.Visible = true;
                    Midback_a.HRef = "EditFuMidback.aspx";
                    break;

                case "LowBack":
                    lnkbtn_lowback.Visible = true;
                    Lowback_a.HRef = "EditFuLowback.aspx";
                    break;

                case "Elbow":
                    lnkbtn_Elbow.Visible = true;
                    if (i.Position == "B") { Elbow_a.HRef = "EditFuElbow.aspx?P=B"; } else if (i.Position == "L") { Elbow_a.HRef = "EditFuElbow.aspx?P=L"; } else if (i.Position == "R") { Elbow_a.HRef = "EditFuElbow.aspx?P=R"; }
                    break;

            }
        }

        
    }

    //protected void lbtnProcedureDetails_Click(object sender, EventArgs e)
    //{
    //    if (Session["PatientIE_ID"] != null)
    //    {
    //        Response.Redirect("~/EditFuProcedureDetails.aspx?PId=" + Convert.ToString(Session["PatientIE_ID"]));
    //    }

    //}
    public void bindData(string PatientFU_ID)
    {
        //IntakeSheet.BLL.BusinessLogic _bl = new IntakeSheet.BLL.BusinessLogic();
        if(PatientFU_ID != "")
        {
        List<string> _injured = getFUInjuredParts(Convert.ToInt64(PatientFU_ID));
        bindBodyParts(_injured);
        }
    }
    public List<string> getFUInjuredParts(Int64 PatientFU_ID)
    {
        DataAccess _dal = new DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PatientFU_ID", PatientFU_ID));
        DataTable _dt = _dal.getDataTable("nusp_GetFUInjuredBodyParts", param);
        IntakeSheet.Entity.BodyParts _bodyparts = new IntakeSheet.Entity.BodyParts();
        DataRow _newdr = _dt.NewRow();
        foreach (DataRow dr in _dt.Rows)
        {
            _bodyparts.Neck = (Convert.ToBoolean(dr["Neck"])) ? true : _bodyparts.Neck;
            _bodyparts.MidBack = Convert.ToBoolean(dr["MidBack"]) ? true : _bodyparts.MidBack;
            _bodyparts.LowBack = Convert.ToBoolean(dr["LowBack"]) ? true : _bodyparts.LowBack;
            _bodyparts.LeftShoulder = Convert.ToBoolean(dr["LeftShoulder"]) ? true : _bodyparts.LeftShoulder;
            _bodyparts.RightShoulder = Convert.ToBoolean(dr["RightShoulder"]) ? true : _bodyparts.RightShoulder;
            _bodyparts.LeftKnee = Convert.ToBoolean(dr["LeftKnee"]) ? true : _bodyparts.LeftKnee;
            _bodyparts.RightKnee = Convert.ToBoolean(dr["RightKnee"]) ? true : _bodyparts.RightKnee;
            _bodyparts.LeftElbow = Convert.ToBoolean(dr["LeftElbow"]) ? true : _bodyparts.LeftElbow;
            _bodyparts.RightElbow = Convert.ToBoolean(dr["RightElbow"]) ? true : _bodyparts.RightElbow;
            _bodyparts.LeftWrist = Convert.ToBoolean(dr["LeftWrist"]) ? true : _bodyparts.LeftWrist;
            _bodyparts.RightWrist = Convert.ToBoolean(dr["RightWrist"]) ? true : _bodyparts.RightWrist;
            _bodyparts.LeftHip = Convert.ToBoolean(dr["LeftHip"]) ? true : _bodyparts.LeftHip;
            _bodyparts.RightHip = Convert.ToBoolean(dr["RightHip"]) ? true : _bodyparts.RightHip;
            _bodyparts.LeftAnkle = Convert.ToBoolean(dr["LeftAnkle"]) ? true : _bodyparts.LeftAnkle;
            _bodyparts.RightAnkle = Convert.ToBoolean(dr["RightAnkle"]) ? true : _bodyparts.RightAnkle;
            _bodyparts.LeftArm = Convert.ToBoolean(dr["LeftArm"]) ? true : _bodyparts.LeftArm;
            _bodyparts.RightArm = Convert.ToBoolean(dr["RightArm"]) ? true : _bodyparts.RightArm;
            _bodyparts.LeftHand = Convert.ToBoolean(dr["LeftHand"]) ? true : _bodyparts.LeftHand;
            _bodyparts.RightHand = Convert.ToBoolean(dr["RightHand"]) ? true : _bodyparts.RightHand;
            _bodyparts.LeftLeg = Convert.ToBoolean(dr["LeftLeg"]) ? true : _bodyparts.LeftLeg;
            _bodyparts.RightLeg = Convert.ToBoolean(dr["RightLeg"]) ? true : _bodyparts.RightLeg;
            _bodyparts.LeftFoot = Convert.ToBoolean(dr["LeftFoot"]) ? true : _bodyparts.LeftFoot;
            _bodyparts.RightFoot = Convert.ToBoolean(dr["RightFoot"]) ? true : _bodyparts.RightFoot;
            _bodyparts.Face = Convert.ToBoolean(dr["Face"]) ? true : _bodyparts.Face;
            _bodyparts.Ribs = Convert.ToBoolean(dr["Ribs"]) ? true : _bodyparts.Ribs;
            _bodyparts.Chest = Convert.ToBoolean(dr["Chest"]) ? true : _bodyparts.Chest;
            _bodyparts.Abdomen = Convert.ToBoolean(dr["Abdomen"]) ? true : _bodyparts.Abdomen;
            _bodyparts.Pelvis = Convert.ToBoolean(dr["Pelvis"]) ? true : _bodyparts.Pelvis;
            _bodyparts.Other = Convert.ToBoolean(dr["Others"]) ? true : _bodyparts.Other;
        }
        _dt.Rows.Clear();
        if (_bodyparts != null)
        {
            _newdr["Neck"] = _bodyparts.Neck;
            _newdr["MidBack"] = _bodyparts.MidBack;
            _newdr["LowBack"] = _bodyparts.LowBack;
            _newdr["LeftShoulder"] = _bodyparts.LeftShoulder;
            _newdr["RightShoulder"] = _bodyparts.RightShoulder;
            _newdr["LeftKnee"] = _bodyparts.LeftKnee;
            _newdr["RightKnee"] = _bodyparts.RightKnee;
            _newdr["LeftElbow"] = _bodyparts.LeftElbow;
            _newdr["RightElbow"] = _bodyparts.RightElbow;
            _newdr["LeftWrist"] = _bodyparts.LeftWrist;
            _newdr["RightWrist"] = _bodyparts.RightWrist;
            _newdr["LeftHip"] = _bodyparts.LeftHip;
            _newdr["RightHip"] = _bodyparts.RightHip;
            _newdr["LeftAnkle"] = _bodyparts.LeftAnkle;
            _newdr["RightAnkle"] = _bodyparts.RightAnkle;
            _newdr["LeftArm"] = _bodyparts.LeftArm;
            _newdr["RightArm"] = _bodyparts.RightArm;
            _newdr["LeftHand"] = _bodyparts.LeftHand;
            _newdr["RightHand"] = _bodyparts.RightHand;
            _newdr["LeftLeg"] = _bodyparts.LeftLeg;
            _newdr["RightLeg"] = _bodyparts.RightLeg;
            _newdr["LeftFoot"] = _bodyparts.LeftFoot;
            _newdr["RightFoot"] = _bodyparts.RightFoot;
            _newdr["Face"] = _bodyparts.Face;
            _newdr["Ribs"] = _bodyparts.Ribs;
            _newdr["Chest"] = _bodyparts.Chest;
            _newdr["Abdomen"] = _bodyparts.Abdomen;
            _newdr["Pelvis"] = _bodyparts.Pelvis;
            _newdr["Others"] = _bodyparts.Other;

        }
        _dt.Rows.Add(_newdr);
        List<string> _injuredParts = new List<string>();
        foreach (DataRow dr in _dt.Rows)
        {
            foreach (DataColumn dc in _dt.Columns)
            {
                if (Convert.ToBoolean(dr[dc]))
                {
                    _injuredParts.Add(dc.ColumnName);
                }
            }
        }


        return _injuredParts;

    }
    public List<string> getFUeInjuredParts(Int64 PatientIE_ID)
    {
        DataAccess _dal = new DataAccess();
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PatientIE_ID", PatientIE_ID));
        DataTable _dt = _dal.getDataTable("nusp_GetFUeInjuredBodyParts", param);
        IntakeSheet.Entity.BodyParts _bodyparts = new IntakeSheet.Entity.BodyParts();
        DataRow _newdr = _dt.NewRow();
        foreach (DataRow dr in _dt.Rows)
        {
            _bodyparts.Neck = (Convert.ToBoolean(dr["Neck"])) ? true : _bodyparts.Neck;
            _bodyparts.MidBack = Convert.ToBoolean(dr["MidBack"]) ? true : _bodyparts.MidBack;
            _bodyparts.LowBack = Convert.ToBoolean(dr["LowBack"]) ? true : _bodyparts.LowBack;
            _bodyparts.LeftShoulder = Convert.ToBoolean(dr["LeftShoulder"]) ? true : _bodyparts.LeftShoulder;
            _bodyparts.RightShoulder = Convert.ToBoolean(dr["RightShoulder"]) ? true : _bodyparts.RightShoulder;
            _bodyparts.LeftKnee = Convert.ToBoolean(dr["LeftKnee"]) ? true : _bodyparts.LeftKnee;
            _bodyparts.RightKnee = Convert.ToBoolean(dr["RightKnee"]) ? true : _bodyparts.RightKnee;
            _bodyparts.LeftElbow = Convert.ToBoolean(dr["LeftElbow"]) ? true : _bodyparts.LeftElbow;
            _bodyparts.RightElbow = Convert.ToBoolean(dr["RightElbow"]) ? true : _bodyparts.RightElbow;
            _bodyparts.LeftWrist = Convert.ToBoolean(dr["LeftWrist"]) ? true : _bodyparts.LeftWrist;
            _bodyparts.RightWrist = Convert.ToBoolean(dr["RightWrist"]) ? true : _bodyparts.RightWrist;
            _bodyparts.LeftHip = Convert.ToBoolean(dr["LeftHip"]) ? true : _bodyparts.LeftHip;
            _bodyparts.RightHip = Convert.ToBoolean(dr["RightHip"]) ? true : _bodyparts.RightHip;
            _bodyparts.LeftAnkle = Convert.ToBoolean(dr["LeftAnkle"]) ? true : _bodyparts.LeftAnkle;
            _bodyparts.RightAnkle = Convert.ToBoolean(dr["RightAnkle"]) ? true : _bodyparts.RightAnkle;
            _bodyparts.LeftArm = Convert.ToBoolean(dr["LeftArm"]) ? true : _bodyparts.LeftArm;
            _bodyparts.RightArm = Convert.ToBoolean(dr["RightArm"]) ? true : _bodyparts.RightArm;
            _bodyparts.LeftHand = Convert.ToBoolean(dr["LeftHand"]) ? true : _bodyparts.LeftHand;
            _bodyparts.RightHand = Convert.ToBoolean(dr["RightHand"]) ? true : _bodyparts.RightHand;
            _bodyparts.LeftLeg = Convert.ToBoolean(dr["LeftLeg"]) ? true : _bodyparts.LeftLeg;
            _bodyparts.RightLeg = Convert.ToBoolean(dr["RightLeg"]) ? true : _bodyparts.RightLeg;
            _bodyparts.LeftFoot = Convert.ToBoolean(dr["LeftFoot"]) ? true : _bodyparts.LeftFoot;
            _bodyparts.RightFoot = Convert.ToBoolean(dr["RightFoot"]) ? true : _bodyparts.RightFoot;
            _bodyparts.Face = Convert.ToBoolean(dr["Face"]) ? true : _bodyparts.Face;
            _bodyparts.Ribs = Convert.ToBoolean(dr["Ribs"]) ? true : _bodyparts.Ribs;
            _bodyparts.Chest = Convert.ToBoolean(dr["Chest"]) ? true : _bodyparts.Chest;
            _bodyparts.Abdomen = Convert.ToBoolean(dr["Abdomen"]) ? true : _bodyparts.Abdomen;
            _bodyparts.Pelvis = Convert.ToBoolean(dr["Pelvis"]) ? true : _bodyparts.Pelvis;
            _bodyparts.Other = Convert.ToBoolean(dr["Others"]) ? true : _bodyparts.Other;
        }
        _dt.Rows.Clear();
        if (_bodyparts != null)
        {
            _newdr["Neck"] = _bodyparts.Neck;
            _newdr["MidBack"] = _bodyparts.MidBack;
            _newdr["LowBack"] = _bodyparts.LowBack;
            _newdr["LeftShoulder"] = _bodyparts.LeftShoulder;
            _newdr["RightShoulder"] = _bodyparts.RightShoulder;
            _newdr["LeftKnee"] = _bodyparts.LeftKnee;
            _newdr["RightKnee"] = _bodyparts.RightKnee;
            _newdr["LeftElbow"] = _bodyparts.LeftElbow;
            _newdr["RightElbow"] = _bodyparts.RightElbow;
            _newdr["LeftWrist"] = _bodyparts.LeftWrist;
            _newdr["RightWrist"] = _bodyparts.RightWrist;
            _newdr["LeftHip"] = _bodyparts.LeftHip;
            _newdr["RightHip"] = _bodyparts.RightHip;
            _newdr["LeftAnkle"] = _bodyparts.LeftAnkle;
            _newdr["RightAnkle"] = _bodyparts.RightAnkle;
            _newdr["LeftArm"] = _bodyparts.LeftArm;
            _newdr["RightArm"] = _bodyparts.RightArm;
            _newdr["LeftHand"] = _bodyparts.LeftHand;
            _newdr["RightHand"] = _bodyparts.RightHand;
            _newdr["LeftLeg"] = _bodyparts.LeftLeg;
            _newdr["RightLeg"] = _bodyparts.RightLeg;
            _newdr["LeftFoot"] = _bodyparts.LeftFoot;
            _newdr["RightFoot"] = _bodyparts.RightFoot;
            _newdr["Face"] = _bodyparts.Face;
            _newdr["Ribs"] = _bodyparts.Ribs;
            _newdr["Chest"] = _bodyparts.Chest;
            _newdr["Abdomen"] = _bodyparts.Abdomen;
            _newdr["Pelvis"] = _bodyparts.Pelvis;
            _newdr["Others"] = _bodyparts.Other;

        }
        _dt.Rows.Add(_newdr);
        List<string> _injuredParts = new List<string>();
        foreach (DataRow dr in _dt.Rows)
        {
            foreach (DataColumn dc in _dt.Columns)
            {
                if (Convert.ToBoolean(dr[dc]))
                {
                    _injuredParts.Add(dc.ColumnName);
                }
            }
        }


        return _injuredParts;

    }
}
