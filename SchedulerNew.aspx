<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="SchedulerNew.aspx.cs" Inherits="SchedulerNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" Runat="Server">
    <script src="js/evtCalendar.js"></script>
    <script>
        var selectDate;

        var daystart =480;
        var dayend = 480;
        function setTime(start, end) {
            debugger
            daystart = start;
            dayend = end;
        }
        function dateClicked(sDate) {
            var cal = document.getElementById("cal");
            var app = document.getElementById("app");
            cal.style.visibility = "collapse";
            app.style.visibility = "visible";
           
            showApp(sDate, "app");
            setAppData(JSON.parse(getapp(sDate)));
            selectDate = sDate;
            //alert('Link ' + text+ ' clicked');
        }
        function calClicked() {
            var cal = document.getElementById("cal");
            var app = document.getElementById("app");
            app.style.visibility = "collapse";
            cal.style.visibility = "visible";
            
        }
        function getdata() {
            var apps = null;
            $.ajax({
                type: "POST",
                url: '<%=ResolveUrl("~/SchedulerNew.aspx/getAppointments") %>',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data, status) {
                    apps = data.d;
            //        alert(apps)
                },
                failure: function (data) {
                    alert("Fail");
                },
                error: function (data) {
                    alert("Error");
                }
            });
            return apps;
        }
        function getapp(selectedDate) {
            var apps = null;
            $.ajax({
                type: "POST",
                url: '<%=ResolveUrl("~/SchedulerNew.aspx/getDayAppointments") %>',
                data: "{ 'selectedDate': '" + selectedDate + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data, status) {
                    apps = data.d;
                    //alert(apps)
                },
                failure: function (data) {
                    alert("Fail");
                },
                error: function (data) {
                    alert("Error");
                }
            });
            return apps;
        }
        function TransferDate() {
            console.log(document.getElementById("txtFDate").value);
            $.ajax({
                url: '<%=ResolveUrl("~/SchedulerNew.aspx/Transfer") %>',
                data: "{ 'fdate': '" + document.getElementById("txtFDate").value + "' , 'tdate': '" + document.getElementById("txtTDate").value+ "'}",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                 //calendar.removeAllEvents();
                //calendar.addEventSource(JSON.parse(getdata()));                 
                calClicked();
                setData(JSON.parse(getdata()));
                alert(data.d);
<%--                $(<%=txtFdate.ClientID %>).val('');
                $(<%=txtTdate.ClientID %>).val('')--%>
            },
            error: function (data, status, jqXHR) { alert("Error " + jqXHR); }
        });
        }

        document.addEventListener('DOMContentLoaded', function () {
            showCalendar('cal');
            setData(JSON.parse(getdata()));
        });
        function closemodal() {
            showApp(selectDate, "app");
            setAppData(JSON.parse(getapp(selectDate)));
            setData(JSON.parse(getdata()));
            $('#ModalTime').modal('hide');
        }

        calChange = function (appoint) {
            //document.getElementById('target').src = "SchedulerSet.aspx?id=" + id + "&date=" + date + "&title=" + title;
            document.getElementById('target').src = "SchedulerSetNew.aspx?appoint=" + JSON.stringify(appoint);
            setTimeout(function () {
                var iframe = document.getElementById("target");
                iframe.contentWindow.document.form1.txtFollowedUpOn.focus();
            }, 300);
            $('#ModalTime').modal('show');
        }

        calRemove = function (appoint) {
            //document.getElementById('target').src = "SchedulerSet.aspx?id=" + id + "&date=" + date + "&title=" + title;
          //  document.getElementById('target').src = "SchedulerSetNew.aspx?appoint=" + JSON.stringify(appoint);
            document.getElementById('target').src = "SchedulerSetNew.aspx?removeappoint=" + JSON.stringify(appoint);
            setTimeout(function () {
                var iframe = document.getElementById("target");
                iframe.contentWindow.document.form1.txtFollowedUpOn.focus();
            }, 300);
            $('#ModalTime').modal('show');
        }
            
        calAdd = function (appoint,seldate) {
          //  alert(appoint);
  //          alert( <%=ddlLocation.ClientID%>.value);
            //document.getElementById('target').src = "SchedulerSet.aspx?id=" + id + "&date=" + date + "&title=" + title;
           document.getElementById('target').src = "SchedulerSetNew.aspx?newappoint=" +  seldate.substr(4,2)+"/"+seldate.substr(6,2)+"/"+seldate.substr(0,4) +" "+appoint  + "&location="+<%=ddlLocation.ClientID%>.value;
            setTimeout(function () {
                var iframe = document.getElementById("target");
                iframe.contentWindow.document.form1.txtFollowedUpOn.focus();
            }, 300);
            $('#ModalTime').modal('show');
        }


    </script>
    <link href="css/calendar.css" rel="stylesheet" />

    <div class="modal fade" id="ModalTime" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; height: 680px!important">
        <div class="modal-dialog" style="background: white;">
            <div class="modal-content">
                <div class="modal-header" style="display: inline-block; width: 100%;">
                    Appointment
                 <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                </div>
                <div class="modal-body" style="height: 500px!important" >
                    <iframe id="target" style="width: 100%; height: 100%" frameborder="0"></iframe>
                </div>
                <%--<div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>
        </div>
    </div>
    <div class="container" style="text-align: center; position: relative;margin-top:30px">
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="col-md-5" style="vertical-align:middle">
            <div class="col-md-3" >Location :</div>
            <div class="col-md-9">
            <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"  ></asp:DropDownList>
            </div> 
        </div>
         
        
            </ContentTemplate>
         
    </asp:UpdatePanel>
   
      <div class="col-md-7">
            From
            <input type="text" id="txtFDate" class="fudate" />
            <%--<asp:TextBox ID="txtFdate" Type="Date" CssClass="fudate" runat="server"></asp:TextBox>--%>
            To
            <input type="text" id="txtTDate" class="fudate" />
            <%--<asp:TextBox ID="txtTdate" CssClass="fudate" runat="server"></asp:TextBox>--%>

            <input id="btnTransfer" type="button" value="Transfer" onclick="TransferDate();" />

        </div>
    </div>
      <div id="cal" style="width:80%;margin:auto"></div>  
      <div id="app" style="visibility:collapse" class="container">
       
      </div>
     <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="js/jquery-ui-timepicker-addon.js"></script>
        <script>


            var $j = jQuery.noConflict();

            //$j(document).ready(function () {           
            $j(".fudate").datepicker({
                controlType: 'select',
                oneLine: true,
                timeFormat: 'hh:mm tt',
                showPeriod: true,
                stepMinute: 30,
                //  defaultTime: '08:00 am',
                formatTime: 'hh:mm tt',
                // defaultValue: new Date()
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });

            
            //   $('#ui-datepicker-div').removeAttr('style');
            //$('p').css("color","blue");

            //});

            //$("#ModalTime").on("hide.bs.modal", function () {

            //    setData(JSON.parse(getdata()));
            //    setAppData(JSON.parse(getapp(selectDate)));
            //});

        </script>
       
</asp:Content>

