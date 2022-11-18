<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogDetail.aspx.cs" Inherits="LogDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PainTrax LogDetail</title>
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="-1">
    <meta name="description" content="Bootstrap Metro Dashboard">
    <meta name="author" content="Dennis Ji">
    <meta name="keyword" content="Metro, Metro UI, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina">
    <!-- end: Meta -->

    <!-- start: Mobile Specific -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- end: Mobile Specific -->

    <!-- start: CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet">
    <link href="css/style.css" rel="stylesheet">
    <link href="css/style-responsive.css" rel="stylesheet">


    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,800italic,400,300,600,700,800&subset=latin,cyrillic-ext,latin-ext' rel='stylesheet' type='text/css'>
    <!-- end: CSS -->

    <style>
        .inline {
            display: inline;
        }

            .inline label {
                vertical-align: central;
            }

        checkbox + label {
            /*Style for checkbox normal*/
            width: 16px;
            height: 16px;
        }
    </style>


    <!-- start: Favicon -->
    <link rel="shortcut icon" href="img/favicon.ico">
    <!-- end: Favicon -->

    <script src="https://raw.githubusercontent.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%-- <script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script type="text/javascript">
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
        $(document).ready(function () {

            $('[id*=txt_start_date]').mask("99/99/9999")

            $('[id*=txt_start_date]').datepicker();
            $('[id*=txt_end_date]').mask("99/99/9999")

            $('[id*=txt_end_date]').datepicker();
        });
    </script>
    <script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById(strGV);
            var rowData;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">

        <asp:ScriptManager runat="server" ID="scpMGR"></asp:ScriptManager>
        <!-- start: Header -->
        <div class="navbar">
            <div class="navbar-inner">
                <div class="container-fluid">
                    <a class="btn btn-navbar" data-toggle="collapse" data-target=".top-nav.nav-collapse,.sidebar-nav.nav-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </a>
                    <a class="brand" href="PatientIntakeList.aspx"><span>PainTrax - Patient Intakesheet</span></a>

                    <div class="nav-no-collapse header-nav">
                        <ul class="nav pull-right">
                            <li><a class="brand" href="Logout.aspx"><span>Logout</span></a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!-- start: Header -->
        <div class="container-fluid-full">
            <div class="row-fluid">
                <div id="content" class="span12">

                    <div style="display: inline">
                        Select Date From
                        <asp:TextBox ID="txt_start_date" runat="server"></asp:TextBox>&nbsp;&nbsp;To Date&nbsp;&nbsp;
                        <asp:TextBox ID="txt_end_date" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        Select Location  
                        &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddl_location" runat="server"></asp:DropDownList>
                    </div>
                    <div>
                        Select Login ID
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddl_Login_Id" runat="server"></asp:DropDownList>
                    </div>
                    <div>
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" AutoPostBack="True" />
                        <br />
                        <br />
                    </div>
                    <div style="display: inline-block">
                        <div style="display: block">
                            <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" onkeyup="Search_Gridview(this, 'gdview')" Visible="false" Style="float: right;" placeholder="Look For"></asp:TextBox>
                        </div>
                        <div style="display: inline-block">
                            <asp:GridView ID="gdview" runat="server"
                                AutoGenerateColumns="false" Font-Names="Arial"
                                Font-Size="11pt" AlternatingRowStyle-BackColor="#C2D69B"
                                HeaderStyle-BackColor="green" AllowPaging="True"
                                OnPageIndexChanging="OnPaging" PageSize="10" AllowSorting="True" Width="100%">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="LogIn_Time" HeaderText="LogIn_Time" />
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="Name" HeaderText="Name" />
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="Location" HeaderText="Location" />
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="Ip_Address" HeaderText="Ip_Address" />
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="LogOut_Time" HeaderText="LogOut_Time" />
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="Description_Detail" HeaderText="Description" />
                                    <asp:BoundField ItemStyle-Width="14.28%" DataField="Log_Time" HeaderText="Log_Time" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="display: block">
                            <asp:Button ID="btnExportPDF" runat="server" Text="ExportToPDF" OnClick="btnExportPDF_Click" Visible="False" Style="float: Right;" />&nbsp;
                            <asp:Button ID="btnExportExcel" runat="server" Text="ExportToExcel" OnClick="btnExportExcel_Click" Visible="False" Style="float: Right;" />
                        </div>
                    </div>
                </div>
                <div class="row-fluid" style="height: auto">
                </div>
            </div>
        </div>


    </form>
</body>
</html>
