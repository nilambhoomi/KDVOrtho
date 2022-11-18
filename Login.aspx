<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <title>Login Page - PainTrax</title>

    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />

    <!-- bootstrap & fontawesome -->
    <link rel="stylesheet" href="../assets/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/font-awesome.css" />

    <!-- text fonts -->
    <link rel="stylesheet" href="../assets/css/ace-fonts.css" />

    <!-- ace styles -->
    <link rel="stylesheet" href="../assets/css/ace.css" />

    <!--[if lte IE 9]>
			<link rel="stylesheet" href="../assets/css/ace-part2.css" />
		<![endif]-->
    <link rel="stylesheet" href="../assets/css/ace-rtl.css" />

    <!--[if lte IE 9]>
		  <link rel="stylesheet" href="../assets/css/ace-ie.css" />
		<![endif]-->

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

    <!--[if lt IE 9]>
		<script src="../assets/js/html5shiv.js"></script>
		<script src="../assets/js/respond.js"></script>
		<![endif]-->

		 <style>
        .alert-warning {
            animation: blinker 2s linear infinite;
             font-weight:bold;
        }

        @keyframes blinker {
            50% {
                background-color: #ffdb91;
              
            }
        }
    </style>
</head>
<body class="login-layout light-login">
    <form id="form1" runat="server">
        <div class="main-container">
            <div class="main-content">
                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="position-relative">
                                <div class="center">
                                    <h1>

                                        <span class="green">e</span>
                                        <span class="grey" id="id-text2">PainTrax</span>
                                    </h1>
                                    <h4 class="blue" id="id-company-text">Admin Login</h4>
                                </div>

                                <div class="space-6"></div>
                                <div id="login-box" class="login-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header blue lighter bigger">
                                                <i class="ace-icon fa fa-coffee green"></i>
                                                <label runat="server" id="lblmess" style="color: red; display: none">Sorry , Invalid login.</label>
                                            </h4>

                                            <div class="space-6"></div>


                                            <fieldset>
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox runat="server" ID="txt_uname" class="form-control"></asp:TextBox>
                                                        <i class="ace-icon fa fa-user"></i>

                                                    </span>
                                                    <asp:RequiredFieldValidator runat="server" ID="req1" ControlToValidate="txt_uname" ErrorMessage="Please Enter User name" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                                                </label>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox runat="server" ID="txt_pass" TextMode="Password" class="form-control"></asp:TextBox>

                                                        <i class="ace-icon fa fa-lock"></i>
                                                    </span>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txt_pass" ErrorMessage="Please Enter Password" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </label>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox runat="server" ID="txtUserMasterID" placeholder="Client Id" class="form-control"></asp:TextBox>
                                                        <i class="ace-icon fa fa-user"></i>
                                                    </span>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtUserMasterID" ErrorMessage="Please Enter Client Id" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </label>
                                                <div class="space"></div>

                                                <div class="clearfix">
                                                    <label class="inline">
                                                        <asp:CheckBox ID="chkRememberMe" CssClass="ace" runat="server" />
                                                        <span class="lbl">Remember Me</span>
                                                    </label>
                                                    <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Login" class="width-35 pull-right btn btn-sm btn-primary"></asp:Button>


                                                </div>

                                                <div class="space-4"></div>
                                            </fieldset>
 <div class="alert alert-warning" style="display:none">
                                                The Website will be down on Friday, 12/24/2021, till 12 pm ET for maintainance.
                                            </div>



                                            <div class="space-6"></div>


                                        </div>
                                        <!-- /.widget-main -->

                                        <div class="toolbar clearfix">
                                            <div>
                                                <asp:Button runat="server" ID="btnChangePW" OnClick="btnChangePW_Click" Text="Change Password" CssClass="btn btn-primary" />
                                            </div>


                                        </div>
                                    </div>
                                    <!-- /.widget-body -->
                                </div>
                                <!-- /.login-box -->
                            </div>
                            <%--   <div class="navbar-fixed-top align-right">
								<br />
								&nbsp;
								<a id="btn-login-dark" href="#">Dark</a>
								&nbsp;
								<span class="blue">/</span>
								&nbsp;
								<a id="btn-login-blur" href="#">Blur</a>
								&nbsp;
								<span class="blue">/</span>
								&nbsp;
								<a id="btn-login-light" href="#">Light</a>
								&nbsp; &nbsp; &nbsp;
							</div>--%>
                        </div>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->
            </div>
            <!-- /.main-content -->
        </div>
        <!-- /.main-container -->

        <!-- start: Header -->
    </form>
</body>
</html>
