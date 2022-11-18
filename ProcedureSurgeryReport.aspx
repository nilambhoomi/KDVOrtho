<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ProcedureSurgeryReport.aspx.cs" Inherits="ProcedureSurgeryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <link href="CSS/CSS.css" rel="stylesheet" type="text/css" />
    <style>
        .pager::before {
            display: none;
        }

        .pager table {
            margin: 0 auto;
        }

            .pager table tbody tr td a,
            .pager table tbody tr td span {
                position: relative;
                float: left;
                padding: 6px 12px;
                margin-left: -1px;
                line-height: 1.42857143;
                color: #337ab7;
                text-decoration: none;
                background-color: #fff;
                border: 1px solid #ddd;
            }

            .pager table > tbody > tr > td > span {
                z-index: 3;
                color: #fff;
                cursor: default;
                background-color: #337ab7;
                border-color: #337ab7;
            }

            .pager table > tbody > tr > td:first-child > a,
            .pager table > tbody > tr > td:first-child > span {
                margin-left: 0;
                border-top-left-radius: 4px;
                border-bottom-left-radius: 4px;
            }

            .pager table > tbody > tr > td:last-child > a,
            .pager table > tbody > tr > td:last-child > span {
                border-top-right-radius: 4px;
                border-bottom-right-radius: 4px;
            }

            .pager table > tbody > tr > td > a:hover,
            .pager table > tbody > tr > td > span:hover,
            .pager table > tbody > tr > td > a:focus,
            .pager table > tbody > tr > td > span:focus {
                z-index: 2;
                color: #23527c;
                background-color: #eee;
                border-color: #ddd;
            }

        label {
            padding: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">



    <asp:HiddenField ID="hdn_ID" runat="server" />
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Generate Report							
									<i class="ace-icon fa fa-angle-double-right"></i>
                    </small>
                    <small>
                        <%--<asp:LinkButton ID="btnaddnew" runat="server" PostBackUrl="~/AddProcedure.aspx">Add New</asp:LinkButton>--%>
                    </small>
                </h1>
            </div>
            <div class="row">


                <div class="row">
                    <div class="col-lg-3 col-md-6 col-sm-12">
                        <div class="input-group">
                            <span class="input-group-addon">From Date</span>
                            <asp:TextBox ID="txtSearchFromdate" CssClass="dtClass" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-12">
                        <div class="input-group">
                            <span class="input-group-addon">To date</span>
                            <asp:TextBox ID="txtSearchTodate" CssClass="dtClass" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-12">
                        <div class="input-group">
                            <span class="input-group-addon">Location </span>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-12">
                        <div class="input-group">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-success " Text="Search" OnClick="btnSearch_Click" />
                            <asp:LinkButton ID="lkExportToexcel" CssClass="btn btn-danger " Style="margin-left: 2px" runat="server" OnClick="lkExportToexcel_Click" Text="ExportExcel"></asp:LinkButton>

                        </div>
                    </div>
                </div>

            </div>
            <br />
            <div class="clearfix"></div>



            <asp:GridView ID="gvProcedureTbl" AllowSorting="true" OnSorting="gridView_Sorting" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Sr. #" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="MCODE" HeaderText="Procedure" />
                    <%--  <asp:BoundField DataField="BodyPart" HeaderText="Body Part" />--%>
                    <asp:BoundField DataField="location" HeaderText="Location" />

                    <asp:TemplateField HeaderText="MC" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image runat="server" ID="imgMC" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",0);" %>' ImageUrl="~/images/Status/green.PNG" Visible='<%# Eval("MC_Status").ToString() == "Received" ? true : false %>' ToolTip="Received" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="imgMCdenied" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",0);" %>' ImageUrl="~/images/Status/red.PNG" Visible='<%# Eval("MC_Status").ToString() == "Denied" ? true : false %>' ToolTip="Denied" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="imgMCPending" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",0);" %>' ImageUrl="~/images/Status/orange.PNG" Visible='<%# Eval("MC_Status").ToString() == "Pending" ? true : false %>' ToolTip="Pending" Style="height: 25px; width: 25px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CaseType" HeaderText="CaseType" ItemStyle-Width="100px" />
                    <asp:TemplateField HeaderText="NF / WC" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image runat="server" ID="CTimgMC" ImageUrl="~/images/Status/green.PNG" Visible='<%# Eval("Case_Status").ToString() == "Received" ? true : false %>' ToolTip="Received" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="CTimgMCdenied" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",1);" %>' ImageUrl="~/images/Status/red.PNG" Visible='<%# Eval("Case_Status").ToString() == "Denied" ? true : false %>' ToolTip="Denied" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="CTimgMCPending" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",1);" %>' ImageUrl="~/images/Status/orange.PNG" Visible='<%# Eval("Case_Status").ToString() == "Pending" ? true : false %>' ToolTip="Pending" Style="height: 25px; width: 25px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ins Ver" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image runat="server" ID="IVimgMC" ImageUrl="~/images/Status/green.PNG" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",2);" %>' Visible='<%# Eval("InsVerStatus").ToString() == "Received" ? true : false %>' ToolTip="Received" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="IVimgMCPending" ImageUrl="~/images/Status/orange.PNG" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",2);" %>' Visible='<%# Eval("InsVerStatus").ToString() == "Pending" ? true : false %>' ToolTip="Pending" Style="height: 25px; width: 25px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vaccined" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image runat="server" ID="VimgMC" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",3);" %>' ImageUrl="~/images/Status/green.PNG" Visible='<%# Eval("Vac_Status").ToString() == "Received" ? true : false %>' ToolTip="Received" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="VimgMCdenied" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",3);" %>' ImageUrl="~/images/Status/red.PNG" Visible='<%# Eval("Vac_Status").ToString() == "N/A" ? true : false %>' ToolTip="Denied" Style="height: 25px; width: 25px" />
                            <asp:Image runat="server" ID="VimgMCPending" onclick='<%# "ShowNotes(" +Eval("ProcedureDetail_ID") + ",3);" %>' ImageUrl="~/images/Status/orange.PNG" Visible='<%# Eval("Vac_Status").ToString() == "Pending" ? true : false %>' ToolTip="Pending" Style="height: 25px; width: 25px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>


            </asp:GridView>
        </div>

        <div class="modal fade" id="notePopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog" style="background: white">
                <div class="modal-content">
                    <div class="modal-header" style="display: inline-block; width: 100%;">
                        <h4>Note</h4>

                    </div>
                    <div class="modal-body">
                        <div id="mcDiv">

                            <label class="align boldertext" style="width: 200px">MC </label>
                            <select id="McType" onchange="showMC(this)">
                                <option value="No">No</option>
                                <option value="Yes">Yes</option>
                            </select>


                            <div id="McOpt" style="display: none">
                                <div id="McDatediv">

                                    <label id="ScDate" class="align boldertext" style="width: 200px">MC Date</label>
                                    <input type="text" id="McDate" class="date" style="width: 100px" />
                                </div>
                                <div>
                                    <label id="ObReport" class="align boldertext" style="width: 200px">Report Status</label>
                                    <select id="McStatus" style="width: 100px" onchange="showReports(this)">
                                        <option value="Pending">Pending</option>
                                        <option value="Received">Received</option>
                                        <option value="Denied">Denied</option>
                                    </select>
                                </div>
                                <div id="Obreport" style="display: none">
                                    <div>
                                        <label id="Notelbl" class="align boldertext" style="width: 200px; vertical-align: top">Note</label>

                                        <%--<input type="text"  value="" id="McNote" />--%>
                                        <textarea id="txt_mc_desc_mc" style="width: 215px; height: 100px;"></textarea>
                                    </div>

                                    <div id="ReDate">
                                        <label id="ReDateLbl" class="align boldertext" style="width: 200px">Reschedule Date</label>
                                        <input type="text" id="McReDate" class="date" style="width: 100px" />
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div id="CTDiv">

                            <div id="CtDatediv">
                                <label id="ctDate" class="align boldertext" style="width: 200px;">WC auth Date</label>
                                <input type="text" id="CtDate" class="date" style="width: 100px" />
                            </div>

                            <div>
                                <label id="AuthSub" class="align boldertext" style="width: 200px;">Auth Status</label>
                                <select id="CtStatus" name="CtStatus" style="width: 100px" onchange="showAuth(this)">
                                    <option value="Pending">Pending</option>
                                    <option value="Received">Received</option>
                                    <option value="Denied">Denied</option>
                                </select>
                            </div>
                            <div id="AuthCal" style="display: none">

                                <div>
                                    <label id="ANotelbl" class="align boldertext" style="width: 200px; vertical-align: top">Note</label>

                                    <%--<input type="text" value="" id="CtNote" />--%>
                                    <textarea id="CtNote" style="width: 215px; height: 100px"></textarea>
                                </div>

                                <div id="AuthDate">
                                    <label class="align boldertext" style="width: 200px; vertical-align: top">Reschedule Date</label>
                                    <input type="text" id="CtReDate" class="date" style="width: 100px" />
                                </div>
                            </div>

                        </div>
                        <div id="Insdiv">
                            <div>
                                <label id="InsVerLbl" class="align boldertext" style="width: 200px;">Ins Verification</label>
                                <select id="InsVerCmb" style="width: 100px" onchange="showIns(this)">
                                    <option value="true">Yes</option>
                                    <option value="false">No</option>
                                </select>
                            </div>
                            <div id="InsVer" style="display: none">
                                <label id="InsVerCon" class="align boldertext" style="width: 200px;">Backup Lien</label>
                                <select id="InsVerCof" style="width: 100px">
                                    <option value="Pending">Pending</option>
                                    <option value="Received">Received</option>
                                </select>
                                <br />
                                <div>
                                    <label id="INotelbl" class="align boldertext" style="width: 200px; vertical-align: top">Note</label>
                                    <textarea id="InsNote" style="width: 215px; height: 100px"></textarea>
                                </div>


                            </div>
                        </div>
                        <div id="Vac">
                            <div>
                                <label id="VacLbl" class="align boldertext" style="width: 200px;">Vacinated</label>
                                <select id="VacCmb" style="margin-left: 15px; width: 100px" onchange="showVac(this)">
                                    <option value="Yes">Yes</option>
                                    <option value="No">No</option>
                                </select>
                            </div>
                            <div id="VacStatus" style="display: none">
                                <label id="VacStatusLbl" class="align boldertext" style="width: 200px;">Status</label>
                                <select id="VacStatusCmb" style="margin-left: 15px; width: 100px">
                                    <option value="Pending">Pending</option>
                                    <option value="Received">Received</option>
                                    <option value="N/A">N/A</option>
                                </select>
                                <br />
                                <div>
                                    <label id="VacNotelbl" class="align boldertext" style="width: 200px; vertical-align: top;">Note</label>
                                    <textarea id="VacNote" style="width: 215px; height: 100px"></textarea>
                                </div>


                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <input type="hidden" id="hidType" />
                        <input type="hidden" id="hidId" />
                    </div>
                    <div class="modal-footer">
                        <div style="display: inline-block; width: 80%; text-align: right;">
                            <button type="button" class="btn btn-success" onclick="SaveNote()">Save Note</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
        <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
            $(function () {



                $('.dtClass').datepicker();



                $('#McDate').datepicker();
                $('#McReDate').datepicker();
                $('#CtReDate').datepicker();
                $('#CtDate').datepicker();

                $("#ctl00_cpMain_gvProcedureTbl").DataTable(
                    {
                        bLengthChange: true,
                        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                        iDisplayLength: -1,
                        bFilter: true,
                        bSort: true,
                        bPaginate: true
                    });


            });

            function ShowNotes(id, type) {

                $.ajax({
                    type: "POST",
                    url: "ProcedureSurgeryReport.aspx/GetNoteDetail",
                    data: "{'ProcedureDetailID':'" + id + "'}",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        dat = JSON.parse(data.d)[0];

                        debugger

                        if (type === 0) {
                            $('#Vac').hide();
                            $('#CTDiv').hide();
                            $('#mcDiv').show();
                            $('#Insdiv').hide();
                            $('#McType').val(dat.MC_Type);
                            if (dat.MC_Report_Status) $('#McStatus').val(dat.MC_Report_Status);
                            if (dat.MC_Date) $('#McDate').val(new Date(dat.MC_Date).toLocaleDateString("en-US"));
                            $('#txt_mc_desc_mc').val(dat.MC_Note);
                            if (dat.MC_ReSche_Date) $('#McReDate').val(new Date(dat.MC_ReSche_Date).toLocaleDateString("en-US"));
                            showMC(document.getElementById('McType'));
                            showReports(document.getElementById('McStatus'));

                        }
                        else if (type === 1) {
                            $('#CTDiv').show();
                            $('#Vac').hide();
                            $('#mcDiv').hide();
                            $('#Insdiv').hide();
                            if (dat.CT_AUTH_Date) $('#CtDate').val(new Date(dat.CT_AUTH_Date).toLocaleDateString("en-US"));
                            if (dat.CT_Report_Status) $('#CtStatus').val(dat.CT_Report_Status);
                            $('#CtNote').val(dat.CT_Note);
                            if (dat.CT_ReSche_Date) $('#CtReDate').val(new Date(dat.CT_ReSche_Date).toLocaleDateString("en-US"));
                            showAuth(document.getElementById('CtStatus'));
                        }
                        else if (type === 2) {
                            $('#CTDiv').hide();
                            $('#mcDiv').hide();
                            $('#Vac').hide();
                            $('#Insdiv').show();
                            if (dat.Ins_Ver_Status == 1)
                                $('#InsVerCmb').val('Yes');
                            else
                                $('#InsVerCmb').val('No');
                            if (dat.Backup_Line) $('#InsVerCof').val(dat.Backup_Line);
                            $('#InsNote').val(dat.Ins_Note);
                            showIns(document.getElementById('InsVerCmb'));
                        } else if (type === 3) {
                            $('#CTDiv').hide();
                            $('#mcDiv').hide();
                            $('#Vac').show();
                            $('#Insdiv').hide();
                            if (dat.IsVaccinated == 1)
                                $('#VacCmb').val('Yes');
                            else
                                $('#VacCmb').val('No');
                            $('#VacStatusCmb').val(dat.Vac_Status);
                            $('#VacNote').val(dat.Vac_Note);
                            showVac(document.getElementById('VacCmb'));
                        }

                        $("#hidType").val(type);
                        $("#hidId").val(id);

                        $('#notePopup').modal('show');


                    },
                    failure: function (response) {
                        alert("Invalid Details...")
                    }
                });


                $('#notePopup').modal('show');
                return false;
            }

            function SaveNote() {


                var type = $("#hidType").val();
                var id = $("#hidId").val();

                var method = "SaveNote";
                var data;

                if (type === '0') {
                    method = "SaveMCNote";
                    data = "{ProcedureDetailID:'" + id + "',mc_type:'" + $("#McType").val() + "',note:'" + $("#txt_mc_desc_mc").val() + "',MC_Date:'" + $("#McDate").val() + "',MC_Report_Status:'" + $("#McStatus").val() + "',MC_ReSche_Date:'" + $("#McReDate").val() + "'}";
                }
                else if (type === '1') {
                    method = "SaveCTNote";
                    data = "{ProcedureDetailID:'" + id + "',note:'" + $("#CtNote").val() + "',CT_AUTH_Date:'" + $("#CtDate").val() + "',CT_Report_Status:'" + $("#CtStatus").val() + "',CT_ReSche_Date:'" + $("#CtReDate").val() + "'}";

                }
                else if (type === '2') {
                    method = "SaveIVNote";
                    data = "{ProcedureDetailID:'" + id + "',note:'" + $("#InsNote").val() + "',Backup_Line:'" + $("#InsVerCof").val() + "',Ins_Ver_Status:'" + $("#InsVerCmb").val() + "'}";

                }
                else if (type === '3') {
                    method = "SaveVCNote";
                    data = "{ProcedureDetailID:'" + id + "',note:'" + $("#VacNote").val() + "',IsVaccinated:'" + $("#VacCmb").val() + "',Vac_Status:'" + $("#VacStatusCmb").val() + "'}";

                }
                $.ajax({
                    type: "POST",
                    url: "ProcedureSurgeryReport.aspx/" + method,
                    data: data,
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        dat = JSON.parse(data.d)[0];

                        debugger



                        alert("Notes updated successfully.")

                        $('#notePopup').modal('hide');
                        location.reload();

                    },
                    failure: function (response) {
                        alert("Invalid Details...")
                    }
                });


                $('#notePopup').modal('show');
                return false;


            }

            function showMC(obj) {
                debugger
                if (obj.value == "Yes") {
                    $("#McOpt").show();
                } else {
                    $("#McOpt").hide();
                    $("#Obreport").hide();
                }

            }

            function showReports(obj) {

                if (obj.value == "Denied") {
                    $("#Obreport").show();
                } else {
                    $("#Obreport").hide();
                }

            }

            function showAuth(obj) {


                if (obj.value == "Denied") {
                    $("#AuthCal").show();;
                } else {
                    $("#AuthCal").hide();
                }

            }

            function showIns(obj) {
                debugger
                if (obj.value == "true") {
                    $("#InsVer").show();
                } else {
                    $("#InsVer").hide();
                }

            }

            function showVac(obj) {
                debugger
                if (obj.value == "Yes") {
                    $("#VacStatus").show();
                } else {
                    $("#VacStatus").hide();
                }

            }
        </script>
</asp:Content>



