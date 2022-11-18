<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PainTrax - Intakesheet</title>
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
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,800italic,400,300,600,700,800&subset=latin,cyrillic-ext,latin-ext' rel='stylesheet' type='text/css'>
    <!-- end: CSS -->

    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%-- <script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>

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

        .margintop tr {
            display: flex;
        }
    </style>


    <!-- start: Favicon -->

    <!-- end: Favicon -->
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>


    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script type="text/javascript">
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager runat="server" ID="scpmgr"></asp:ScriptManager>

        <div id="mymodelmessage" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Message</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server" ID="upMessage" UpdateMode="Conditional">
                            <ContentTemplate>
                                <label runat="server" id="lblMessage"></label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

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
                    <!-- end: Header Menu -->
                </div>
            </div>
        </div>
        <!-- start: Header -->
        <div class="container-fluid-full span8" style="margin-top: 8%; margin-left: 20%">
            <div class="row-fluid">
                <div id="content" class="span12">
                    <h1>Change Password</h1>
                    <hr />
                    <div class="form-horizontal">
                        <div class="control-group">
                            <label class="control-label">User Name: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_uname" ReadOnly="true" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <div style="clear: both"></div>

                        <div class="control-group">
                            <label class="control-label">Old Password: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_pass" TextMode="Password" Width="90%"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txt_pass" ErrorMessage="*" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div style="clear: both"></div>

                        <div class="control-group">
                            <label class="control-label">New Password: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_newPass" TextMode="Password" Width="90%"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txt_newPass" ErrorMessage="*" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="Regex2" runat="server" ControlToValidate="txt_newPass"
                                    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"
                                    ErrorMessage="Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character" ForeColor="Red" />
                                 <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                 ControlToValidate="txt_newPass"
                                 CssClass="ValidationError"
                                 ControlToCompare="txt_pass"
                                 ErrorMessage="Can't be the same old password" 
                                 ToolTip="Password must be different" Operator="NotEqual" />
                            </div>
                        </div>
                        <div style="clear: both"></div>

                        <div class="control-group">
                            <label class="control-label">Confirm Password: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_connewPass" TextMode="Password" Width="90%"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txt_connewPass" ErrorMessage="*" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                 <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                 ControlToValidate="txt_connewPass"
                                 CssClass="ValidationError"
                                 ControlToCompare="txt_newPass"
                                 ErrorMessage="No Match" 
                                 ToolTip="Password must be the same" />
                            </div>
                        </div>
                        <div style="clear: both"></div>

                        <div class="control-group">
                            <div class="controls">
                                <label runat="server" id="lblmess" style="color: red; display: none">Sorry, Incorrect old password</label>
                            </div>
                        </div>

                        <div class="control-group">
                            <div class="controls">
                                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-primary" />&nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
