<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SchedulerSetNew.aspx.cs" Inherits="SchedulerSetNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    
    <title></title>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap-theme.min.css" />
    <link rel="stylesheet" href="assets/css/font-awesome.css" />

      <%--<link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />--%>
      <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-1.8.2.js"></script>
  
    <link href="css/jquery-ui-1.8.21.custom.css" rel="stylesheet" />


    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
        <script type="text/javascript" src="js/SignJs/bootstrap3-typeahead.min.js"></script>

    <link rel="stylesheet" href="css/signature-pad.css" />
    <style>
        #container {
            display: block !important;
            position: relative !important;
        }

        .ui-autocomplete {
            position: absolute !important;
        }

    </style>


    <link href="Style/jquery-ui.min.css" rel="stylesheet" />

    <script src="Scripts/jquery-ui.min.js"></script>


    <style>
        .ui-datepicker td > a.ui-state-active {
            color: indianred !important;
            font-weight: 800;
        }
        .ui-datepicker-calendar{
            width:250px !important;
        }
    </style>
    <script>
        function newShow() {
           
            $("#setAppoint").css({ "visibility": "collapse" });
            $("#setAppoint").css({ "display": "none" });

            $("#<%=txtFirstName.ClientID %>").val('');
            $("#<%=txtLastName.ClientID %>").val('');
            $("#<%=txtDOB.ClientID %>").val('');
            $("#<%=txtPhone.ClientID %>").val('');
            $("#<%=txtMobile.ClientID %>").val('');
            $("#newPatient").css({ "visibility": "visible" });
        }
        function valid() {
         
            if (ddlSex.selectedvalue == 0) {
                alert("Please Select Gender");
                return false;
            }
            return true;
        }
    </script>

</head>
<body>

    <form id="form1" runat="server">
<%--        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                <ContentTemplate>--%>
               <div id="newPatient" style="visibility:collapse" >
                    <table style="width:100%" class="table">
                        <tr>
                            <td>First Name</td>
                            <td>Last name</td>
                        </tr>
                        <tr>
                              <td><asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" style="z-index:100" >

                                  </asp:TextBox>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="save" ControlToValidate="txtFirstName" runat="server" ErrorMessage="First Name Not Empty"></asp:RequiredFieldValidator>
                              </td>
                            <td> <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  ValidationGroup="save" ControlToValidate="txtLastName" runat="server" ErrorMessage="Last Name Not Empty"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Sex</td>
                            <td>DOB</td>
                        </tr>
                        <tr>
                              <td>    <asp:DropDownList runat="server" ID="ddlSex" Width="90px" CssClass="form-control">
                                            <asp:ListItem Value="0">-- Sex --</asp:ListItem>
                                            <asp:ListItem Value="Mr." Text="M"></asp:ListItem>
                                            <asp:ListItem Value="Ms." Text="F"></asp:ListItem>

                                        </asp:DropDownList></td>
                            <td>  <asp:TextBox ID="txtDOB" CssClass="dobdate form-control"  runat="server" ></asp:TextBox>
                                 <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDOB" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                                            ErrorMessage="Invalid date format." ValidationGroup="save" />
                            </td>
                        </tr>
                        <tr>
                            <td>Home Phone</td>
                            <td>Mobile</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox></td>
                              <td><asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" style="z-index:100" ></asp:TextBox></td>
                        </tr>

                        <tr>
                            <td></td>
                            <td style="text-align:right" ><asp:Button ID="btnSave" OnClientClick="valid()" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="save" OnClick="btnSave_Click" />
                             <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" /></td>
                        </tr>
                    </table>
                      
                   
                   

               </div>
                      <div  id="setAppoint" >
        <div class="alert-danger" style="padding:5px" runat="server" id="divDetail">
        <asp:Label ID="lblFollowupDetail" runat="server" Text="Label"  ></asp:Label>
            </div>
        <div class="alert" style="padding:5px" runat="server" id="divText" Visible="false">
         <asp:HiddenField ID="hfPatientId" runat="server" />
            Patinet Name:
         <asp:TextBox ID="txtPatientName" runat="server" Text=""  CssClass="form-control" placeholder="Type Patient Name"  ></asp:TextBox>
            
            <div id="container">
            </div>
              <input type="button"  class="btn btn-primary" value="New" onclick="newShow()" />
               <%--<asp:Button ID="btnNew" runat="server" Text="New"  Visible="true" OnClientClick="newShow()" CssClass="btn btn-primary"   />--%>
            </div>
           
         
            Note
            <asp:TextBox ID="txtNote"  CssClass="form-control" runat="server"></asp:TextBox>

        <div>
                Location :
            <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
            <br />
    <div>
        <br />
        <br />
      <asp:TextBox ID="txtFollowedUpOn" CssClass="fudate form-control"  runat="server" style="width:50%;float:right"></asp:TextBox>
        <br />
       <br />
       
    </div>
                                                                <asp:Button ID="btnSet" CssClass="btn btn-primary" style="float:right" runat="server" Text="Set" OnClick="btnSet_Click" />
                  </div>
            

     
<%--                                                   </ContentTemplate>
            </asp:UpdatePanel>--%>

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
               minTime: '06:00:00 AM',
                maxTime:'10:00:00 PM',
              //  defaultTime: '08:00 am',
                formatTime: 'hh:mm tt',
               // defaultValue: new Date()
           });

             $j('#<%=txtDOB.ClientID%>').datepicker({
                changeMonth: true,
                 changeYear: true,
                 minDate: new Date(1900, 0, 1),
               yearRange: '1900:+0'
                //onSelect: function (dateText, inst) {
                //    $(this).focus();
                //}
            });
        //});
            $("#<%=txtPatientName.ClientID %>").autocomplete({
                appendTo: "#container",
                source: function (request, response) {
                    var str = request.term;

                    if (str.length < 3) {
                        return;
                    }
                    $.ajax({
                        url: 'Search.aspx/GetPatientMaster',
                        data: "{ 'prefix': '" + str + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('_')[0],
                                    val: item.split('_')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                                $("#<%=hfPatientId.ClientID %>").val(i.item.val);
                                $('#<%= txtPatientName.ClientID %>').val(i.item.label);

                },
                minLength: 1
            });

                

   
        </script>

    </form>

</body>
</html>
