<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FollowUpMaster.master" CodeFile="EditFuPoc.aspx.cs" Inherits="EditFUPoc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>

    <link rel="stylesheet" href="css/signature-pad.css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/Multiselect.css" rel="stylesheet" />
    <%--  <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>--%>
    <script src="js/images/bootstrap.min.js"></script>
    <%--<script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>--%>

    <script src="js/multiselect.js"></script>
    <style>
        .well {
            background-color: #3de33ddb;
            position: fixed;
            width: 96.5%;
            margin-top: -55px;
        }

        .navbar-inverse .nav .active > a, .navbar-inverse .nav .active > a:hover, .navbar-inverse .nav .active > a:focus {
            color: #fff;
            background-color: #3de33ddb;
        }

        .panel-default > .panel-heading {
            color: #333;
            background-color: #f5f5f5;
            border-color: #ddd;
        }

        .panel-body {
            padding: 15px;
            overflow-x: auto;
            white-space: nowrap;
        }

        .panel-default > .panel-heading + .panel-collapse > .panel-body {
            border-top-color: #ddd;
        }

        .panel-group .panel-heading + .panel-collapse > .list-group, .panel-group .panel-heading + .panel-collapse > .panel-body {
            border-top: 1px solid #ddd;
        }

        .panel-title > .small, .panel-title > .small > a, .panel-title > a, .panel-title > small, .panel-title > small > a {
            color: inherit;
        }


        a {
            color: #337ab7;
            text-decoration: none;
        }

        a {
            background-color: transparent;
        }

        .radio input[type="radio"], .checkbox input[type="checkbox"] {
            float: left;
            margin-left: 13px;
        }

        .ProcText {
            width: 120px;
        }

        input[type=checkbox] {
            -ms-transform: scale(1.5); /* IE */
            -moz-transform: scale(1.5); /* FF */
            -webkit-transform: scale(1.5); /* Safari and Chrome */
            -o-transform: scale(1.5); /* Opera */
            margin-left: 13px;
            /*padding: 10px;*/
        }

        .Proctable {
            table-layout: fixed;
            min-width: 1300px;
            word-wrap: break-word;
        }
    </style>

     <div class="panel panel-default">
        <div class="panel-heading">
            <h4 class="panel-title"><a class="collapse" style="cursor: pointer;" id="#Summarytable">Summary</a></h4>
        </div>
        <div id="Summarytablediv" class="panel-collapse collapse" style="display: block">
            <div class="panel-body">
                <div class="table-responsive">
                    <asp:Repeater runat="server" ID="repSummery">
                        <HeaderTemplate>
                            <table class="table table-striped table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th>Date</th>
                                        <th>Body Part</th>
                                        <th>Heading</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("PDate","{0:MM/dd/yyyy}") %></td>
                                <td><%# Eval("BodyPart") %></td>
                                <td><%# Eval("Heading") %></td>

                            </tr>
                        </ItemTemplate>
                        <FooterTemplate></table></FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div>
        <asp:HiddenField ID="hdnControlconsider" runat="server" />
        <!--Grid Container-->
        <div class="panel-group" id="Consider">
            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
        </div>
        <!--Grid Container-->
        <div class="panel-group" id="accordion">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </div>
        <%-- <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>--%>
    </div>

    <div class="modal fade" id="SelectPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <b id="CatHeading"></b>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                </div>
                <div class="modal-body">
                    <form id="MedForm" class="">

                        <%--<div class="col-lg-3">
                            <label id="SubCode_lbl" class="align boldertext">Sub Code</label>
                            <select id="SubCode" style="margin-left: 20px" onchange="SubcodeOnchange();">
                                <option value="">--Select--</option>
                            </select>
                        </div>--%>
                        <div class="col-lg-3">
                            <label id="Positionlbl" class="align boldertext">Position</label>
                            <%-- <select id="PositionValue" style="margin-left: 32px">
                                <option value="">--Select--</option>
                                <option value="L">Left</option>
                                <option value="R">Right</option>
                                <option value="B">Bilateral</option>--%>
                            <%-- <option value="B12B">B12B</option>
                                <option value="B12L">B12L</option>
                                <option value="B12R">B12R</option>--%>
                            <%--</select>--%>
                            <input type="text" style="margin-left: 30px" id="PositionValue" readonly="true" />

                        </div>
                        <div class="col-lg-3">
                            <label id="Levellbl" class="align boldertext">Level</label>
                            <input type="text" style="margin-left: 45px" value="" id="Level" />
                        </div>

                        <div class="col-lg-3">
                            <label id="Sidelbl" class="align boldertext">Side</label>
                            <%--<input type="text" style="margin-left: 45px" id="Side" />--%>
                            <select id="Side" style="margin-left: 50px">
                                <option value=" ">--Select--</option>
                                <option value="Left">Left</option>
                                <option value="Right">Right</option>
                                <option value="Bilateral">Bilateral</option>
                            </select>
                        </div>

                        <div class="col-lg-3">
                            <label id="Musclelbl" class="align boldertext">Muscle</label>
                            <select id="Muscle" class="multiselect form-control ddlpackage" multiple="multiple" style="margin-left: 35px">
                                <option value="">--Select--</option>
                            </select>
                        </div>
                        <div class="col-lg-3">
                            <label id="Medicationlbl" class="align boldertext">Medication</label>
                            <%--<select id="Medication" style="margin-left: 13px;">--%>
                            <select id="Medication" class="multiselect form-control ddlpackage" multiple="multiple" style="margin-left: 13px">
                                <option value="">--Select--</option>
                            </select>
                        </div>
                        <div class="col-lg-3">
                            <label id="SubProcedurelbl" class="align boldertext">SubProcedure</label>
                            <select id="SubProcedure" style="margin-left: 13px;">
                                <option value="">--Select--</option>
                            </select>
                        </div>
                        <div class="col-lg-3">
                            <label id="Date" class="align boldertext">Date</label>
                            <input type="text" id="date" class="date" style="margin-left: 52px" />
                        </div>
                        <div class="col-lg-3">
                            <label id="lblSign" class="align boldertext">Sign</label>

                            <div id="signature-pad" class="signature-pad">
                                <canvas id="canvSign"></canvas>
                                <div class="signature-pad--actions">
                                    <div>
                                        <button type="button" class="button clear" data-action="clear">Clear</button>
                                        <%-- <button type="button" class="button" data-action="change-color">Change color</button>
                                <button type="button" class="button" data-action="undo">Undo</button>--%>
                                    </div>
                                    <div>
                                        <input type="hidden" id="hidBlob" />
                                        <%--   <button type="button" class="button save" id="btnsignsave" data-action="save-png">Save Sign</button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3" id="signEdit">
                            <label id="lblSignEdit" class="align boldertext">Sign</label>

                            <img id="imgSignEdit" />
                            <input type="hidden" id="hidSign" />
                            <button id="btnClear" onclick="return clearSign()">Clear</button>
                        </div>
                    </form>
                </div>
                <div class="modal-footer" style="margin-right: 18px">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" onclick="save()" class="btn btn-primary">Save</button>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="dov" />
        <input type="hidden" id="HasSides" />
        <input type="hidden" id="ProcedureDetailId" />
        <input type="hidden" id="ProcedureId" />
        <input type="hidden" id="Hasposition" />
        <input type="hidden" id="HasbodyPart" />
        <input type="hidden" id="Haslevel" />
        <input type="hidden" id="Hasmuscle" />
        <input type="hidden" id="HasMedication" />
        <input type="hidden" id="HasSubcode" />
        <input type="hidden" id="inhouseproc" />
        <input type="hidden" id="PPID" />
        <input type="hidden" runat="server" id="BodyPartID" />
        <input type="hidden" runat="server" id="BodyPartval" />
        <input type="hidden" runat="server" id="positionVal" />
        <input type="hidden" id="SubCodeVal" />
        <input type="hidden" id="LevelVal" />
        <input type="hidden" id="SideVal" />
        <input type="hidden" id="MuscleVal" />
        <input type="hidden" id="Position" />
        <input type="hidden" id="MedicationVal" />
        <input type="hidden" id="DateVal" />
        <input type="hidden" id="NewProc" />
        <asp:HiddenField ID="hfPatientIE_ID" runat="server" />
        <asp:HiddenField ID="hfPatientFU_ID" runat="server" />
        <input type="hidden" id="hfLevelsDefault" />
        <input type="hidden" id="hfSidesDefault" />

    </div>
    <%--<h2>Neck</h2>--%>
    <div id="counttable"></div>
    <div class="modal fade" id="EditPopuprec" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                </div>
                <div class="modal-body">
                    <p>Do you want to edit or create procedure ?</p>
                </div>
                <div class="modal-footer" style="margin-right: 18px">
                    <button type="button" onclick="CreateProc()" class="btn btn-default" data-dismiss="modal">Create New Procedure</button>
                    <button type="button" onclick="EditProc()" class="btn btn-primary">Edit Procedure</button>
                </div>
            </div>
        </div>
    </div>
    <%--    <p><a href="#">Do it.</a></p>--%>
    <script>
        //alert(t[0].dataset.pid)
        //alert(t[0].dataset.body)
        //alert(t[0].dataset.position)
        //alert(t[0].dataset.level)
        //alert(t[0].dataset.pos)
        //alert(t[0].dataset.muscle)
        //alert(t[0].dataset.subcode)
        //alert(t[0].dataset.inhouseproc)
        //alert(t[0].dataset.muscle)
        function tableTransform(objTable) {
            debugger
            objTable.each(function () {
                var $this = $(this);
                var newrows = [];
                $this.find("tbody tr, thead tr").each(function () {
                    var i = 0;
                    $(this).find("td, th").each(function () {
                        i++;
                        if (newrows[i] === undefined) {
                            newrows[i] = $("<tr></tr>");
                        }
                        newrows[i].append($(this));
                    });
                });
                $this.find("tr").remove();
                $.each(newrows, function () {
                    $this.append(this);
                });
            });
            // switch old th to td
            //objTable.find('th').wrapInner('<td />').contents().unwrap();
            ////move first tr into thead
            //var thead = objTable.find("thead");
            //var thRows = objTable.find("tr:first");
            //var copy = thRows.clone(true).appendTo("thead");
            //thRows.remove();
            ////switch td in thead into th
            //objTable.find('thead tr td').wrapInner('<th />').contents().unwrap();
            ////add tr back into tfoot
            //objTable.find('tfoot').append("<tr></tr>");
            ////add tds into tfoot
            //objTable.find('tbody tr:first td').each(function () {
            //    objTable.find('tfoot tr').append("<td>&nbsp;</td>");
            //});
            return false;
        }

    </script>
    <script type="text/javascript">
        function toggleDiv(divId) {
            $("#" + divId).toggle();
        }




        function save() {
            debugger;

            download();
            //var SubCode = $('#SubCode').val();
            //if (SubCode == null) {
            //    SubCode = 0;
            //}

            var PositionValue = $('#PositionValue').val();
            var blobstr = $('#hidBlob').val();
            var Muscle = $('#Muscle').val();
            var Medication = $('#Medication').val();
            var date = $('#date').val();
            var procDetailId = $('#ProcedureDetailId').val();
            //if (procDetailId == undefined)
            //{ procDetailId = 0 }
            var procId = $('#ProcedureId').val();
            var IeId = $("#<%=hfPatientIE_ID.ClientID%>").val();
            var FUId = $("#<%=hfPatientFU_ID.ClientID%>").val();
            var BPart = $("#<%=BodyPartID.ClientID%>").val();
            var MuscleStr = "";
            var MedicationStr = "";
            var IsFromNew = "";
            if (Muscle != null) {
                if (Muscle.length > 0) {
                    for (var i = 0; i < Muscle.length; i++) {
                        MuscleStr += Muscle[i] + '~';
                    }
                }
            }

            if (Medication != null) {
                if (Medication.length > 0) {
                    for (var i = 0; i < Medication.length; i++) {
                        MedicationStr += Medication[i] + '~';
                    }
                }
            }

            //}
            //if ($('#PPID').val() != "0") {
            //    IsFromNew = 0;
            //} else {
            //    IsFromNew = 1;
            //}
            if ($('#NewProc').val() == '1') {
                var procid = 0;
                IsFromNew = 1;
                var ajaxdata = "{ ProcedureDetailID:'" + procDetailId +
                    "',ProcedureMasterID:'" + procId +
                    "',_patientIEID:'" + $("#<%=hfPatientIE_ID.ClientID%>").val() +
                    "',_patientFUID:'" + $("#<%=hfPatientFU_ID.ClientID%>").val() +
                    "',SubProcedureID:'" + $('#SubProcedure').val() +
                    "',BodyPart:'" + BPart +
                    "',ProcedureID:'" + procId +
                    "',Medication:'" + MedicationStr +
                    "',Muscle:'" + MuscleStr +
                    "',Level:'" + $('#Level').val() +
                    "',Position:'" + $('#PositionValue').val() +
                    "',date:'" + $('#date').val() +
                    "',req:'" + $("#<%=positionVal.ClientID%>").val() +
                    "',BodyPartID:'" + 0 +
                    "',IsFromNew:'" + IsFromNew +
                    "',PatientProcedureID:'" + procid +
                    "',IsConsidered:'" + 0 +
                    "',Side:'" + $('#Side').val() +
                    "',BlobStr:'" + blobstr +
                    "'}";
            } else {
                IsFromNew = 0;
                var ajaxdata = "{ ProcedureDetailID:'" + procDetailId +
                    "',ProcedureMasterID:'" + procId +
                    "',_patientIEID:'" + $("#<%=hfPatientIE_ID.ClientID%>").val() +
                    "',_patientFUID:'" + $("#<%=hfPatientFU_ID.ClientID%>").val() +
                    "',SubProcedureID:'" + $('#SubProcedure').val() +
                    "',BodyPart:'" + BPart +
                    "',ProcedureID:'" + procId +
                    "',Medication:'" + MedicationStr +
                    "',Muscle:'" + MuscleStr +
                    "',Level:'" + $('#Level').val() +
                    "',Position:'" + $('#PositionValue').val() +
                    "',date:'" + $('#date').val() +
                    "',req:'" + $("#<%=positionVal.ClientID%>").val() +
                    "',BodyPartID:'" + 0 +
                    "',IsFromNew:'" + IsFromNew +
                    "',PatientProcedureID: '" + $('#PPID').val() +
                    "',IsConsidered:'" + 0 +
                    "',Side:'" + $('#Side').val() +
                    "',BlobStr:'" + blobstr +
                    "'}";
            }
            //if ($('#PPID').val() != "0")
            //{


            //}
            //else {

            //}

            $.ajax({
                type: "POST",
                url: "EditFUPoc.aspx/Save",
                data: ajaxdata,
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //alert("Procedure Saved sucessfully");
                    $('#SelectPopup').modal('hide');
                    location.reload();

                    //$('.reload').click();
                    //$('#LocSelectPopup').modal('hide');
                    //console.log('reached');
                },
                failure: function (response) {
                    alert("Invalid Details...")
                }
            });


        }
        $('.date').keyup(function () {
            if (!this.value) {
                $('.date').blur();
                $('.date').datepicker("hide")
                if (confirm('Do u want to delete?')) {
                    var ProcedureDetailIDfordelete = $('#ProcedureDetailId').val();
                    if (ProcedureDetailIDfordelete == undefined) { ProcedureDetailIDfordelete = 0 }
                    var ajaxdatadel = "{ ProcedureDetailID:'" + ProcedureDetailIDfordelete +
                        "', ProcedureMasterID:'" + $('#ProcedureId').val() +
                        "',Position:'" + $('#PositionValue').val() +
                        "',req:'" + $("#<%=positionVal.ClientID%>").val() +
                        "'}";
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: "POC.aspx/Delete",
                        data: ajaxdatadel,
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function () {
                            $('#SelectPopup').modal('hide');
                            location.reload();
                        },
                        failure: function (response) {
                            alert("Invalid Details...")
                        }
                    });
                }
            }

        });
        $(document).ready(function () {

            $(window).load(function () {
                $("#" + localStorage.getItem("lastdisplayGridvalue")).prop("style", "height:auto;display:block");
                if (localStorage.getItem("lastdisplayGridconsider") == "none") { $("#Considertablediv").prop("style", "height:0px;display:none"); }
            });

            //$(window).bind('onload', function () {
            //    //$('#accordion').find("div[id*='div']").each(function (e) {
            //    //    if (localStorage.getItem("lastdisplayGridvalue") == this.id) {
            //    //        $("#" + localStorage.getItem("lastdisplayGridvalue")).prop("style", "height:auto;display:block");
            //    //    }
            //    //});
            //});

            $(window).bind('beforeunload', function () {
                debugger;
                $('#accordion').find("div[id*='div']").each(function (e) {
                    if ($("#" + this.id).css("display") == "block") { localStorage.setItem("lastdisplayGridvalue", this.id); }
                });
                if ($("#Considertablediv").css("display") == "none") { localStorage.setItem("lastdisplayGridconsider", "none"); }
                else { localStorage.setItem("lastdisplayGridconsider", "block"); }
            });

            $('.collapse').click(function () {
                var test = this.id;
                if ($(test + "div").css("display") == "none") {
                    $(test + "div").prop("style", "height:auto;display:block");
                }
                else {
                    $(test + "div").prop("style", "height:0px;display:none")
                }
            });
            $('#date').val($('#ctl00_ContentPlaceHolder1_dov').val());
            $('#date').text($('#ctl00_ContentPlaceHolder1_dov').val());
            $('#date').html($('#ctl00_ContentPlaceHolder1_dov').val());
            $('#date').datepicker();
            // $('#Medication').multiselect({ includeSelectAllOption: true});
            setdatepicker();
            loadPanel();
            if ($('#lblName').val() != "") {
                $('#New').css("visiblity", "visible");
            }
            else {
                $('#New').css("visiblity", "hidden");
            }

            $('#txtDate').text($('#DOEhdn').val());
        });
        $('#SelectPopup').on('shown.bs.modal', function (e) {
            resizeCanvas();
        })

        $('#EditPopuprec').on('shown.bs.modal', function (e) {
            resizeCanvas();
        })



        function clearSign() {

            $('#signEdit').hide();
            $('#signature-pad').show();
            $('#lblSign').show();

            $('#canvSign').height(200);

            resizeCanvas();

            return false;
        }
        function saveClick() {
            //$.blockUI({ message: "<h1>Remote call in progress...</h1>" });


            // unblock when remote call returns 



        }
        function setdatepicker() {
            $('.dateonly').datepicker({
                dateFormat: "mm/dd/yy"
            });
            $('.date').datepicker({
                dateFormat: "mm/dd/yy",

            });
        }
        function loadPanel() {
         <%--   $("#<%=btnReload.ClientID%>").click(function () {
                $('#<%=lblAlert.ClientID%>').html('');
                $('#modalclose').click();

            });--%>
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    setdatepicker();
                    loadPanel();
                }
            });
        }
        function CountPopup(t) {

            $('#ProcedureId').val(t[0].dataset.pid);
            //$('#PPID').val(t[0].dataset.PatientIEID);
            $.ajax({
                type: "POST",
                url: "EditFUPoc.aspx/GetProcedureCountDetail",
                data: "{ 'PID':' " + t[0].dataset.pid +
                    "',PatientIEID:'" + $("#<%=hfPatientIE_ID.ClientID%>").val() +
                    "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                success: function (output) {
                    debugger;
                    $('#' + t[0].dataset.div).html('');
                    $('#' + t[0].dataset.div).append(output.d);
                    tableTransform($('#countbl'));

                }
            });
        }
        function considerPopup(t) {
            debugger;
            var conID = t[0].dataset.conid;
            if (confirm('Are you sure you want to Delete this Consider?')) {
                $.ajax({
                    type: "POST",
                    url: "EditFUPoc.aspx/DeleteConsiderFromDB",
                    data: "{'ConID':'" + conID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    success: function (output) {
                        debugger;
                        if (output.d > 0) { location.reload(); }
                    }
                });
            }

        }
        function boxDisable(t) {
            debugger;
            $('#ProcedureDetailId').val(t[0].dataset.procedure_detail_id);
            $('#ProcedureId').val(t[0].dataset.pid);
            $('#HasMedication').val(t[0].dataset.medication);
            $('#Hasmuscle').val(t[0].dataset.muscle);
            $('#Haslevel').val(t[0].dataset.haslevel);
            $('#HasSides').val(t[0].dataset.hasSides);
            $("#<%=positionVal.ClientID%>").val(t[0].dataset.position);
            $("#<%=BodyPartID.ClientID%>").val(t[0].dataset.body);
            $("#<%=BodyPartval.ClientID%>").val(t[0].dataset.bodyid);
            $('#PPID').val(t[0].dataset.ppid);
            $('#HasSubcode').val(t[0].dataset.subcode);
            $('#SubCodeVal').val(t[0].dataset.subpid);
            // var SubCode = $('#SubCodeVal').val();
            //if (SubCode == null) {
            //    SubCode = '';
            //}
            var PositionValue = $('#PositionValue').val();
            var Muscle = $('#Muscle').val();
            var Medication = $('#Medication').val();
            var date = $('#date').val();
            var procDetailId = $('#ProcedureDetailId').val();
            var procId = $('#ProcedureId').val();
            var IeId = $("#<%=hfPatientIE_ID.ClientID%>").val();
            var FUId = 0;
            var BPart = $("#<%=BodyPartID.ClientID%>").val();
            var MuscleStr = "";
            var IsConsidered = "";
            if ($('#PPID').val() != "0") {
                $('#NewProc').val('0');
            }
            else {
                $('#NewProc').val('1');
            }
            //if (t.is(':checked')) {
            //if (confirm('Are you sure you want to consider this procedure?')) {
            if ($('#NewProc').val() == '1') {
                var procid = 0;
                IsFromNew = 1;
                IsConsidered = '1';
                var ajaxdata = "{ ProcedureDetailID:'" + procDetailId +
                    "', ProcedureMasterID:'" + procId +
                    "',_patientIEID:'" + $("#<%=hfPatientIE_ID.ClientID%>").val() +
                    "',_patientFUID:'" + $("#<%=hfPatientFU_ID.ClientID%>").val() +
                    "',SubProcedureID:'" + $('#SubProcedure').val() +
                    "',BodyPart:'" + BPart +
                    "',ProcedureID:'" + procId +
                    "',Medication:'" + $('#Medication').val() +
                    "',Muscle:'" + MuscleStr +
                    "',Level:'" + $('#Level').val() +
                    "',Position:'" + $('#PositionValue').val() +
                    "',date:'" + $('#date').val() +
                    "',req:'" + $("#<%=positionVal.ClientID%>").val() +
                    "',BodyPartID:'" + 0 +
                    "',IsFromNew:'" + IsFromNew +
                    "',PatientProcedureID:'" + $('#PPID').val() +
                    "',IsConsidered:'" + IsConsidered +
                    "',Side:'" + $('#Side').val() +
                    "'}";

            } else {
                IsConsidered = '1';
                IsFromNew = 0;
                var ajaxdata = "{ ProcedureDetailID:'" + procDetailId +
                    "', ProcedureMasterID:'" + procId +
                    "',_patientIEID:'" + $("#<%=hfPatientIE_ID.ClientID%>").val() +
                    "',_patientFUID:'" + $("#<%=hfPatientFU_ID.ClientID%>").val() +
                    "',SubProcedureID:'" + $('#SubProcedure').val() +
                    "',BodyPart:'" + BPart +
                    "',ProcedureID:'" + procId +
                    "',Medication:'" + $('#Medication').val() +
                    "',Muscle:'" + MuscleStr +
                    "',Level:'" + $('#Level').val() +
                    "',Position:'" + $('#PositionValue').val() +
                    "',date:'" + $('#date').val() +
                    "',req:'" + $("#<%=positionVal.ClientID%>").val() +
                    "',BodyPartID:'" + 0 +
                    "',IsFromNew:'" + IsFromNew +
                    "',PatientProcedureID: '" + $('#PPID').val() +
                    "',IsConsidered:'" + IsConsidered +
                    "',Side:'" + $('#Side').val() +
                    "'}";

            }
            debugger;
            var considerdata = "{procedureDetailID:'" + procId + "',BodyPart:'" + BPart + "',patientIEID:'" + $("#<%=hfPatientIE_ID.ClientID%>").val() + "'}";

            var already = "";
            var oTable = JSON.parse(document.getElementById('<%= hdnControlconsider.ClientID %>').value);
            debugger;
            $(oTable).each(function (index, val) {
                if (val.pid == procId) {
                    already = "true";
                    alert("Already exists");
                }
            })
            if (already != "true") {

                $.ajax({
                    type: "POST",
                    url: "EditFUPoc.aspx/Saveconsider",
                    data: considerdata,
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //alert("Considered saved sucessfully");
                        //$('#SelectPopup').modal('hide');
                        //location.reload();
                    },
                    failure: function (response) {
                        alert("Invalid Details...")
                    }
                });
                //}
                //else {

                //}


                $.ajax({
                    type: "POST",
                    url: "EditFUPoc.aspx/Save",
                    data: ajaxdata,
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //alert("Procedure Saved sucessfully");
                        $('#SelectPopup').modal('hide');
                        location.reload();
                    },
                    failure: function (response) {
                        alert("Invalid Details...")
                    }
                });
            }
        }

        function Popup(t) {
            debugger;

              var retVal = true;
            if (t[0].dataset.position == "Schedule") {

                var mcode = t[0].dataset.mcode;
                var idID = document.getElementById('<%= hfPatientIE_ID.ClientID %>').value;
                var BPart = t[0].dataset.body;

                var checkdata = "{MCode:'" + mcode + "',BodyPart:'" + BPart + "',PatientIEID:'" + idID + "'}";

                $.ajax({
                    type: "POST",
                    url: "POC.aspx/checkStatus",
                     async: false,
                    data: checkdata,
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    success: function (output) {
                        var data = JSON.parse(output.d);

                        var cnt = data.cnt;

                        if (cnt == 0) {
                            retVal = confirm('Patient Require to Execute ' + data.mcode + ' befor Exeute for ' + mcode + '. you still like to continue ?');

                        }
                    }
                });
               
            }


            if (retVal) {
                $('#NewProc').val('1')
                $('#ProcedureDetailId').val(t[0].dataset.procedure_detail_id);
                $('#ProcedureId').val(t[0].dataset.pid);
                $('#HasMedication').val(t[0].dataset.medication);
                $('#Hasmuscle').val(t[0].dataset.muscle);
                $('#Haslevel').val(t[0].dataset.haslevel);
                $('#HasSides').val(t[0].dataset.hassides);
                $("#<%=positionVal.ClientID%>").val(t[0].dataset.position);
                $("#<%=BodyPartID.ClientID%>").val(t[0].dataset.body);
                $("#<%=BodyPartval.ClientID%>").val(t[0].dataset.bodyid);
                $('#PPID').val(t[0].dataset.ppid);
                $('#HasSubcode').val(t[0].dataset.subcode);
                $('#SubCodeVal').val(t[0].dataset.subpid);
                $('#CatHeading').text(t[0].dataset.position);
                $('#SubCode').html('');
                $('#Level').html('');
                $('#Muscle').html('');
                $('#Medication').html('');
                $('#Level').html('');
                $('#date').html($('#ctl00_ContentPlaceHolder1_dov').val());
                $('#PositionValue').val(t[0].dataset.pos);
                $('#PositionValue').show();
                $('#Positionlbl').show();

                $('#signEdit').hide();



                if (t[0].dataset.position == 'Execute' && t[0].dataset.cf == 'True') {
                    $('#lblSign').show();
                    $('#signature-pad').show();
                }
                else {
                    $('#lblSign').hide();
                    $('#signature-pad').hide();
                }



                if (t[0].dataset.subcode == "True") {
                    $.ajax({
                        type: "POST",
                        url: "EditFUPoc.aspx/GetSubCodeFromDB",
                        data: "{'SubCode':'" + t[0].dataset.pid + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (output) {
                            debugger;
                            $('#SubProcedure').html('');
                            var json = JSON.parse(output.d);
                            $.each(json, function (index, value) {
                                $('#SubProcedure').append($('<option>').text(value.SubCode).attr('value', value.SubCode));
                            });
                        }
                    });
                    $('#SubProcedure').show();
                    $('#SubProcedurelbl').show();
                }

                if (t[0].dataset.muscle == "True") {

                    $.ajax({
                        type: "POST",
                        url: "EditFUPoc.aspx/GetMuscleFromDB",
                        data: "{'Muscle':'" + t[0].dataset.pid + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (output) {
                            debugger;
                            $('#Muscle').html('');
                            var json = JSON.parse(output.d);
                            $.each(json, function (index, value) {
                                $('#Muscle').append($('<option>').text(value.Muscle).attr('value', value.Muscle));
                            });
                        }
                    });
                    $('#Muscle').show();
                    $('#Musclelbl').show();
                }

                if (t[0].dataset.medication == "True") {

                    $.ajax({
                        type: "POST",
                        url: "EditFUPoc.aspx/GetMedicationFromDB",
                        data: "{'Medication':'" + t[0].dataset.pid + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (output) {
                            debugger;
                            $('#Medication').html('');
                            var json = JSON.parse(output.d);
                            $.each(json, function (index, value) {
                                $('#Medication').append($('<option>').text(value.Medicaton).attr('value', value.Medicaton));
                            });
                            $('#Medication').val($('#MedicationVal').val());
                        }
                    });

                    $('#Medication').show();
                    $('#Medicationlbl').show();
                }

                if (t[0].dataset.haslevel != "True") {
                    $('#Level').hide();
                    $('#Levellbl').hide();
                }
                else {
                    $('#Level').val(t[0].dataset.levelsdefault);
                    $('#Level').show();
                    $('#Levellbl').show();
                }
                if (t[0].dataset.hassides != "True") {
                    $('#Side').hide();
                    $('#Sidelbl').hide();
                }
                else {
                    $('#Side').val(t[0].dataset.sidesdefault);
                    $('#Side').show();
                    $('#Sidelbl').show();
                }
                if (t[0].dataset.medication != "True") {
                    $('#Medication').hide();
                    $('#Medicationlbl').hide();
                }
                else {
                    $('#Medication').show();
                    $('#Medicationlbl').show();
                }
                if (t[0].dataset.muscle != "True") {
                    $('#Muscle').hide();
                    $('#Musclelbl').hide();
                }
                else {

                    $('#Muscle').show();
                    $('#Musclelbl').show();
                }
                if (t[0].dataset.subcode != "True") {
                    $('#SubProcedure').hide();
                    $('#SubProcedurelbl').hide();
                }
                else {

                    $('#SubProcedure').show();
                    $('#SubProcedurelbl').show();
                }

                $('#SelectPopup').modal('show');
            }
        }

        function PopupNE(t) {
            debugger
            $('#ProcedureDetailId').val(t[0].dataset.procedure_detail_id);
            $('#hidSign').val(t[0].dataset.signpath);
            $('#ProcedureId').val(t[0].dataset.pid);
            $('#HasMedication').val(t[0].dataset.medication);
            $('#Hasmuscle').val(t[0].dataset.muscle);
            $("#<%=positionVal.ClientID%>").val(t[0].dataset.position);
            $("#<%=BodyPartID.ClientID%>").val(t[0].dataset.body);
            $("#<%=BodyPartval.ClientID%>").val(t[0].dataset.bodyid);

            $('#SubCodeVal').val(t[0].dataset.subpid);
            $('#PPID').val(t[0].dataset.ppid);
            $('#HasSubcode').val(t[0].dataset.subcode);
            $('#PPID').val(t[0].dataset.ppid);
            $('#Hasposition').val(t[0].dataset.pos);

            $('#PositionValue').val(t[0].dataset.pos);
            $('#CatHeading').val(t[0].dataset.position);
            $('#Haslevel').val(t[0].dataset.haslevel);
            $('#HasSides').val(t[0].dataset.hassides);
            $('#MedicationVal').val(t[0].dataset.medi);
            $('#MuscleVal').val(t[0].dataset.musc);
            $('#DateVal').val(t[0].dataset.date);
            $('#LevelVal').val(t[0].dataset.level);
            $('#SideVal').val(t[0].dataset.sides);
            $('#hfLevelsDefault').val(t[0].dataset.levelsdefault);
            $('#hfSidesDefault').val(t[0].dataset.sidesdefault);

            $('#EditPopuprec').modal('show');
        }
        function EditProc() {
            debugger;
            $('#NewProc').val('0');
            $('#EditPopuprec').modal('hide');
            $('#SubCode').html('');
            $('#Level').html('');
            $('#Muscle').html('');
            $('#Medication').html('');

            if ($("#<%=positionVal.ClientID%>").val() == 'Execute') {
                $('#lblSign').hide();
                $('#signature-pad').hide();
                $('#signEdit').show();


                $('#imgSignEdit').attr('src', 'Sign/' + $('#hidSign').val());
            }

            $('#CatHeading').text($("#<%=positionVal.ClientID%>").val());

            if ($('#PPID').val() != "0") {

                $('#PositionVal').val($("#<%=positionVal.ClientID%>").val());
                $('#PositionValue').show($('#Hasposition').val());
                $('#Positionlbl').show();


                if ($('#Haslevel').val() != "True") {
                    $('#Level').hide();
                    $('#Levellbl').hide();
                }
                else {
                    $('#Level').val($('#LevelVal').val());
                    $('#Level').show();
                    $('#Levellbl').show();
                }

                if ($('#HasSides').val() != "True") {
                    $('#Side').hide();
                    $('#Sidelbl').hide();
                }
                else {
                    $('#Side').val($('#SideVal').val());
                    $('#Side').show();
                    $('#Sidelbl').show();
                }

                if ($('#HasSubcode').val() != "True") {
                    $('#SubProcedure').hide();
                    $('#SubProcedurelbl').hide();
                }
                else {
                    $.ajax({
                        type: "POST",
                        //url: "EditFUPoc.aspx/GetSubCodeFromDB",
                        //data: "{'SubCode':'" + t[0].dataset.pid + "'}",
                        url: "EditFUPoc.aspx/GetSubCodeFromDB",
                        data: "{'SubCode':'" + $('#ProcedureId').val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (output) {
                            debugger;
                            $('#SubProcedure').html('');
                            var json = JSON.parse(output.d);
                            $.each(json, function (index, value) {
                                debugger;
                                $('#SubProcedure').append($('<option>').text(value.SubCode).attr('value', value.SubCode));
                            });
                            var str = $('#SubCodeVal').val();
                            if (str != "")
                                $("#SubProcedure").find("option[value=" + str + "]").prop("selected", "selected");
                        }
                    });

                    $('#SubProcedure').show();
                    $('#SubProcedurelbl').show();
                }

                if ($('#HasMedication').val() != "True") {
                    $('#Medication').hide();
                    $('#Medicationlbl').hide();
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "EditFUPoc.aspx/GetMedicationFromDB",
                        data: "{'Medication':'" + $('#ProcedureId').val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (output) {
                            debugger;
                            $('#Medication').html('');
                            var json = JSON.parse(output.d);
                            $.each(json, function (index, value) {
                                debugger;
                                $('#Medication').append($('<option>').text(value.Medicaton).attr('value', value.Medicaton));
                            });
                            var str = $('#MedicationVal').val();
                            if (str != "")
                                if (str.indexOf('~') != -1) {
                                    var selectedOptions = str.split("~");
                                    for (var i in selectedOptions) {
                                        $("#Medication option").filter('[value="' + selectedOptions[i] + '"]').prop('selected', true);
                                    }
                                }
                        }
                    });

                    $('#Medication').show();
                    $('#Medicationlbl').show();
                }
                if ($('#Hasmuscle').val() != "True") {
                    $('#Muscle').hide();
                    $('#Musclelbl').hide();
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "EditFUPoc.aspx/GetMuscleFromDB",
                        data: "{'Muscle':'" + $('#ProcedureId').val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "JSON",
                        success: function (output) {
                            debugger;
                            $('#Muscle').html('');
                            var json = JSON.parse(output.d);
                            $.each(json, function (index, value) {
                                $('#Muscle').append($('<option>').text(value.Muscle).attr('value', value.Muscle));
                            });
                            var str = $('#MuscleVal').val();
                            if (str != "")
                                if (str.indexOf('~') != -1) {
                                    var selectedOptions = str.split("~");
                                    for (var i in selectedOptions) {
                                        $("#Muscle option").filter('[value="' + selectedOptions[i] + '"]').prop('selected', true);
                                    }
                                }
                        }
                    });
                    $('#Muscle').show();
                    $('#Musclelbl').show();
                }
            }
            $('#date').val($('#DateVal').val());
            $('#SelectPopup').modal('show');

        }

        function CreateProc() {
            debugger
            $('#NewProc').val('1')
            $('#NewProc').val('1');
            $('#SubCode').html('');
            $('#Level').html('');
            $('#Muscle').html('');
            $('#Medication').html('');
            $('#Level').val('');
            $('#date').html($('#ctl00_ContentPlaceHolder1_dov').val());

            $('#CatHeading').text($("#<%=positionVal.ClientID%>").val());

            $('#PositionValue').show($('#Hasposition').val());
            $('#PositionVal').show();
            $('#Positionlbl').show();

            $('#signEdit').hide();



            if ($("#<%=positionVal.ClientID%>").val() == 'Execute') {
                $('#lblSign').show();
                $('#signature-pad').show();
            }
            else {
                $('#lblSign').hide();
                $('#signature-pad').hide();
            }


            if ($('#Haslevel').val() != "True") {
                $('#Level').hide();
                $('#Levellbl').hide();
            }
            else {
                $('#Level').val($('#hfLevelsDefault').val());
                $('#Level').show();
                $('#Levellbl').show();
            }

            if ($('#HasSides').val() != "True") {
                $('#Side').hide();
                $('#Sidelbl').hide();
            }
            else {
                $('#Side').val($('#hfSidesDefault').val());
                $('#Side').show();
                $('#Sidelbl').show();
            }


            if ($('#HasSubcode').val() != "True") {
                $('#SubProcedure').hide();
                $('#SubProcedurelbl').hide();
            }
            else {
                $.ajax({
                    type: "POST",
                    //url: "EditFUPoc.aspx/GetSubCodeFromDB",
                    //data: "{'SubCode':'" + t[0].dataset.pid + "'}",
                    url: "EditFUPoc.aspx/GetSubCodeFromDB",
                    data: "{'SubCode':'" + $('#ProcedureId').val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    success: function (output) {
                        debugger;
                        $('#SubProcedure').html('');
                        var json = JSON.parse(output.d);
                        $.each(json, function (index, value) {
                            $('#SubProcedure').append($('<option>').text(value.SubCode).attr('value', value.SubCode));
                        });
                        $('#SubProcedure').val($('#SubCodeVal').val());
                    }
                });

                $('#SubProcedure').show();
                $('#SubProcedurelbl').show();
            }



            if ($('#HasMedication').val() != "True") {
                $('#Medication').hide();
                $('#Medicationlbl').hide();
            }
            else {

                $.ajax({
                    type: "POST",
                    url: "EditFUPoc.aspx/GetMedicationFromDB",
                    data: "{'Medication':'" + $('#ProcedureId').val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    success: function (output) {
                        debugger;
                        $('#Medication').html('');
                        var json = JSON.parse(output.d);
                        $.each(json, function (index, value) {
                            $('#Medication').append($('<option>').text(value.Medicaton).attr('value', value.Medicaton));
                        });
                        $('#Medication').val($('#MedicationVal').val());
                    }
                });


                $('#Medication').show();
                $('#Medicationlbl').show();
            }
            if ($('#Hasmuscle').val() != "True") {
                $('#Muscle').hide();
                $('#Musclelbl').hide();
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "EditFUPoc.aspx/GetMuscleFromDB",
                    data: "{'Muscle':'" + $('#ProcedureId').val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "JSON",
                    success: function (output) {
                        debugger;
                        $('#Muscle').html('');
                        var json = JSON.parse(output.d);
                        $.each(json, function (index, value) {
                            $('#Muscle').append($('<option>').text(value.Muscle).attr('value', value.Muscle));
                        });
                    }
                });
                $('#Muscle').show();
                $('#Musclelbl').show();
            }
            $('#SelectPopup').modal('show');
        }



    </script>
    <style>
        .panel {
            margin-bottom: 20px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 px 1px r 0 bo - 1px rgba 0, 0, .pane - pa ding 15px .pan l-hading

        {
            px 15px;
            b rder olid transparent;
            o px;
            border-to-right-radius: 3 x .pane r down-toggle color: i he
        }


        ma
        mar
        :

        font-size: 6 x;
        : inherit;
        }

        nel-title > s al .panel-tit e > .sma .pane - itle,
        -title > small,
        . > small > a {
            color: inheit;
            .panel footer

        {
            padding: 10 x 1px;
            nd-color: #f5f5f5;
            x solid #ddd;
            bord r 3px;
            bo m-left r dius: 3px;
        }

        st-group,
        > .lis - roup {
            margin-bo
        }

        .pa e > .list-grup .list-group-i e
        anel > .panel colap
        list-group-ite {
            1px 0 border-adius: 0;
        }

        nel > l st-group:first- h ld .list-group-item:firt-child,
        .panel >
        .list-grou :f
        roup-item:first-child {
            r-top: 0;
            bo r border-top-right-rdius: 3px;
        }

        panel list-group:last c ild .list-group-item:lst-child,
        .pan l > .list-group la
        up-item:last-child {
            m: 0;
            bor er-b d b rder-bottomleft-radius: 3px;
            .panel-heading + li l em:first-ch l

        {
            dth: 0;
            . .pane - ooter

        {
            bord th: 0;
            .pa e > .panel-collaps .tabl, el > .table, l sive > . able

        {
            margn-bottom
        }

        .panel pse > t ble caption,
        panel > .
        .pan l > .
        .table capti n {
            15px;
            padding-left: 1 p;
        }

        panel table-responsive:f r
        first-child,
        .p
        t-child {
            bo r border-top-right-radius: 3px;
        }

        .panel > .table-responsi table: i st-child > tbody:first-child r:first-child,
        .panel > .table-responsi table: i st-child > thead:f r t-child > tr:firs - hild,
        table: i st-child > tbody:f r t-child > tr:firs - hild,
        :first-child > thead:fi st-c {
            b rder 3 borde - op-right-radius: 3px;
        }

        .panel > .table-responsve:first-child tbody f rst-child > tr:first-child td f rst-child,
        .panel > .table-responsve:first-child tbody f rst-child > tr:first-child th f rst-child,
        .panel > .table-responsve:first-child thead f rst-child > tr:first-child td f rst-child,
        .panel > .table-responsve:first-child thead f rst-child > tr:fir t child th:first-ch l,
        .panel tbody f rst-child > tr:fir t child td:first-ch l,
        .panel tbody f rst-child > tr:fir t child th:first-ch l,
        .panel thead f rst-child > tr:fir t child td:first-ch l,
        .panel >
        d:first-child > tr:firs -chi

        rder-t p left-radius: 3px;
        }

        .panel > .table-responsve:first-child > tbod : irst-child > tr:first-child t : ast-child,
        .panel > .table-responsve:first-child > tbod : irst-child > tr:first-child t : ast-child,
        .panel > .table-responsve:first-child > thea : irst-child > tr:first-child t : ast-child,
        .panel > .table-responsve:first-child > thea : irst-child > tr:fi s -child th:last-ch l,
        .panel > tbod : irst-child > tr:fi s -child td:last-ch l,
        .panel > tbod : irst-child > tr:fi s -child th:last-ch l,
        .panel > thea : irst-child > tr:fi s -child td:last-ch l,
        .pane
        ad:first-child > tr:firs -chi

        b r er-top-right-radius: 3px;
        }

        .pane .table-responsiv :
        e:last-child,
        pane
        ild {
            bor er-b d b rder-bottom-left-radius: 3px
        }

        .pa e > .table-resp > .tab e last-child > tbody:last-chil tr:last-child,
        .pa e > .table-resp > .tab e last-child > tfoo : ast-child > tr:l s -child,
        > .tab e last-child > tbod : ast-child > tr:l s -child,
        ble:last-child > tfoot:last chil
        border- otto
        x

        order- o tom-left-radius: 3px;
        }

        .pa e > .table-resonsive:last-chi > tbo y last-child > tr:last-child t : irst-child,
        .pa e > .table-resonsive:last-chi > tbo y last-child > tr:last-child t : irst-child,
        .pa e > .table-resonsive:last-chi > tfo t last-child > tr:last-child t : irst-child,
        .pa e > .table-resonsive:last-chi > tfo t last-child > tr:l s -child th:first- h ld,
        .pan > tbo y last-child > tr:l s -child td:first- h ld,
        .pan > tbo y last-child > tr:l s -child th:first- h ld,
        .pan > tfo t last-child > tr:l s -child td:first- h ld,
        .pa e
        oot:last-child > tr:last-c ild

        r-bott m left-radius: 3px;
        }

        .pa e > .table-resonsive:last-ch d > tb d :last-child > tr:last-child d last-child,
        .pa e > .table-resonsive:last-ch d > tb d :last-child > tr:last-child h last-child,
        .pa e > .table-resonsive:last-ch d > tf o :last-child > tr:last-child d last-child,
        .pa e > .table-resonsive:last-ch d > tf o :last-child > tr: a t-child th:last- h ld,
        .pa d > tb d :last-child > tr: a t-child td:last- h ld,
        .pa d > tb d :last-child > tr: a t-child th:last- h ld,
        .pa d > tf o :last-child > tr: a t-child td:last- h ld,
        .p n
        foot:last-child > tr:last-c ild

        bord r bottom-righ - adius:
        }

        .panel > .pa ble,
        .panel > .pa ble-re p nsive,
        .panel > .t b

        .pnel > .tabl

        o der-top: 1px soli ddd;
        .pa e > .ta l > tbody:first-ch l > tr:first-chld t
        anel > .tab e
        c

        rst-ch l th {
            der-to: 0;
        }

        .panel >
        ed,
        >

        ve > . a le-bordered {
            b r er: 0;
            .p n l > .table-bord r d > t o y r > td:first-ch .p n l > .table-bord r d > t o y r > th:first-ch .p n l > .table-bord r d > t o t r > td:first-ch .p n l > .table-bord r d > t o t r > th:first-ch .p n l > .table-bord r d > t e d r > td:first-ch .p n l > .table-border d > thead > tr > h first c il, .p respon i e > .table-border d > tbody > tr > d first c il, .p respon i e > .table-border d > tbody > tr > h first c il, .p respon i e > .table-border d > tfoot > tr > d first c il, .p respon i e > .table-border d > tfoot > tr > h first c il, .p respon i e > .table-border d > thead > tr > d first c il, . a nsive > .tab e- a t-chil borde - ef : 0;
            . a el > .table-bor e ed > b dy > tr > td:last-c . a el > .table-bor e ed > b dy > tr > th:last-c . a el > .table-bor e ed > f ot > tr > td:last-c . a el > .table-bor e ed > f ot > tr > th:last-c . a el > .table-bor e ed > h ad > tr > td:last-c . a el > .table-borde e > thead > tr > t :last c il, . -respo s ve > .table-borde e > tbody > tr > t :last c il, . -respo s ve > .table-borde e > tbody > tr > t :last c il, . -respo s ve > .table-borde e > tfoot > tr > t :last c il, . -respo s ve > .table-borde e > tfoot > tr > t :last c il, . -respo s ve > .table-borde e > thead > tr > t :last c il, p onsive > .tab e- a -child

        {
            order r ght: 0;
            .p n l > .table-bord r d > t o y > tr:first-c i d > .p n l > .table-bord r d > t o y > tr:first-c i d > .p n l > .table-bord r d > t e d > tr:first-c i d > .p n l > .table-border d > thead > tr:fi s -chil th, .p respon i e > .table-border d > tbody > tr:fi s -chil td, .p respon i e > .table-border d > tbody > tr:fi s -chil th, .p respon i e > .table-border d > thead > tr:fi s -chil td, . a nsive > .table bo > th b rder- o tom: 0;
            . a el > .table-bor e ed > b dy > tr:last- h ld . a el > .table-bor e ed > b dy > tr:last- h ld . a el > .table-bor e ed > f ot > tr:last- h ld . a el > .table-borde e > tfoot > tr:l s -chil th, . -respo s ve > .table-borde e > tbody > tr:l s -chil td, . -respo s ve > .table-borde e > tbody > tr:l s -chil th, . -respo s ve > .table-borde e > tfoot > tr:l s -chil td, p onsive > .tabl -b t hild > t

        {
            : 0;
            .panel . o mar i bo der: anel-group

        {
            : 20px;
            panel-group .p nel;
            o der-ra i
        }

        .



        margi -
        }


        l

        g {
            border b ttom: 0;
        }

        l-group .panl-heading + .p n l-collapse > .l s -group,
        up .panel-h adig + .anel-
        b

        brder-top: 1px s
        }

        p

        oter {
            bo d r-top: 0;
        }

        group .panel-f ote + .pnel-c
        y

        bo d
        solid #ddd;

        nel-default {
            bord r
        }

        efault > .panel-h ading {
            olor: #333;
            g;
            border-color ddd;
        }

        > .panel-heading + .pa
        n

        bor e -top-color: #dd;
        .pan
        ading .badge {
            : b ckground-colo : #333;
        }

        fault > .panel-foote + .p > {
            tom-color: #d d;
            -primary

        {
            border- o
        }

        rimary > .panel-h ading {
            olor: #fff;
            u order-color: # 3 ab7;
        }

        > .panel-heading + .panel
        -

        border t p-color: #337a7;
        .pan
        ading .badge {
            :;
            background-color: #fff;
        }
    </style>
    <script src="js/signature_pad.umd.js"></script>
    <script src="js/app.js"></script>
</asp:Content>
