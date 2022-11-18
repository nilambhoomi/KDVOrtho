<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Appointment.aspx.cs" Inherits="Appointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
    <link href="css/jquery-ui-1.8.21.custom.css" rel="stylesheet" />

    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <style>
        .ui-datepicker td > a.ui-state-active {
            color: indianred !important;
            font-weight: 800;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

    <div class="container-fluid" style="margin-top: 50px">
        <div style="padding:10px">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/SchedulerNew.aspx" CssClass="btn btn-primary" >Scheduler</asp:HyperLink>
            
        </div>
        <div class="col-md-4">
            <div>
                Location :
            <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
            <br />
            <div style="background-color:lavender;padding:10px">
                <strong>Patient Name: </strong>
                <asp:Label ID="lblPatientDetail" runat="server" Text=""></asp:Label>
            </div>
            <br />
            <asp:TextBox ID="txtFollowedUpOn" CssClass="fudate form-control" runat="server"></asp:TextBox>
            <br />
            <label>Note</label>
            <asp:TextBox ID="txtNote" CssClass="form-control" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnSet" CssClass="btn btn-primary" runat="server" Text="Set" OnClick="btnSet_Click" />
            <br />
            <br />
        </div>
        <div class="col-md-8">
            <div><strong>Previous Appoinments</strong></div>
            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-6" >
                       <div style="background-color: lavender; margin:5px;display:flex">                        
                        <div class="col-md-10">
                            <div style="padding: 15px; text-align: center">
                                <%# getDate(Eval("AppointmentDate").ToString())%>
                                <%# Eval("AppointmentStart") %>
                                <%# Eval("Location") %>
                                <br />
                                <%# Eval("AppointmentNote") %>
                            </div>
                        </div>
                        <div class="col-md-2" style="padding:5px;background-color: lavender; ">
                            <asp:Button ID="Button1" runat="server" Text="✖" height="24" 
                                style="margin-top:10px" CssClass="" 
                                CommandArgument='<%# Eval("AppointmentId")%>'
                                CommandName="Delete" 
                                OnClientClick="return confirm('Are you sure you want delete');"
                                />
                        </div>
                      </div>

                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="js/jquery-ui-timepicker-addon.js"></script>
    <script>


        var $j = jQuery.noConflict();

        //$j(document).ready(function () {           
        $j(".fudate").datetimepicker({
            controlType: 'select',
            oneLine: true,
            timeFormat: 'hh:mm tt',
            showPeriod: true,
            stepMinute: 30,
            //  defaultTime: '08:00 am',
            formatTime: 'hh:mm tt',
            // defaultValue: new Date()
        });
        //});



    </script>


</asp:Content>

