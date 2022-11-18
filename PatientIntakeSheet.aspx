<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientIntakeSheet.aspx.cs" Inherits="PatientIntakeSheet" %>

<!DOCTYPE html>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

        .margintop tr {
            display: flex;
        }
    </style>


    <!-- start: Favicon -->
    <link rel="shortcut icon" href="img/favicon.ico">
    <!-- end: Favicon -->
    <script>     function openPopup(divid) {

         $('#' + divid + '').modal('show');

     }
    </script>

</head>
<body>
    <form id="form1" runat="server">


        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="js/jquery-1.6.4.min.js" />
                <asp:ScriptReference Path="js/jquery.ui.core.js" />
                <asp:ScriptReference Path="js/jquery.ui.widget.js" />
                <asp:ScriptReference Path="js/jquery.ui.button.js" />
                <asp:ScriptReference Path="js/jquery.ui.position.js" />
                <asp:ScriptReference Path="js/jquery.ui.autocomplete.js" />
                <asp:ScriptReference Path="js/jquery.ui.combobox.js" />
            </Scripts>
        </asp:ScriptManager>

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
                    <a class="brand" href="PatientIntakeList.aspx"><span>PainTrax</span></a>

                    <!-- start: Header Menu -->
                    <div class="nav-no-collapse header-nav">
                        <ul class="nav pull-right">
                            <li class="dropdown hidden-phone">
                                <a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
                                    <i class="halflings-icon white warning-sign"></i>
                                </a>

                            </li>
                            <!-- start: Notifications Dropdown -->
                            <li class="dropdown hidden-phone">
                                <a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
                                    <i class="halflings-icon white tasks"></i>
                                </a>
                                <ul class="dropdown-menu tasks">
                                    <li class="dropdown-menu-title">
                                        <span>You have 17 tasks in progress</span>
                                        <a href="#refresh"><i class="icon-repeat"></i></a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="header">
                                                <span class="title">iOS Development</span>
                                                <span class="percent"></span>
                                            </span>
                                            <div class="taskProgress progressSlim red">80</div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="header">
                                                <span class="title">Android Development</span>
                                                <span class="percent"></span>
                                            </span>
                                            <div class="taskProgress progressSlim green">47</div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="header">
                                                <span class="title">ARM Development</span>
                                                <span class="percent"></span>
                                            </span>
                                            <div class="taskProgress progressSlim yellow">32</div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="header">
                                                <span class="title">ARM Development</span>
                                                <span class="percent"></span>
                                            </span>
                                            <div class="taskProgress progressSlim greenLight">63</div>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="header">
                                                <span class="title">ARM Development</span>
                                                <span class="percent"></span>
                                            </span>
                                            <div class="taskProgress progressSlim orange">80</div>
                                        </a>
                                    </li>
                                    <li>
                                        <a class="dropdown-menu-sub-footer">View all tasks</a>
                                    </li>
                                </ul>
                            </li>
                            <!-- end: Notifications Dropdown -->
                            <!-- start: Message Dropdown -->
                            <li class="dropdown hidden-phone">
                                <a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
                                    <i class="halflings-icon white envelope"></i>
                                </a>
                                <ul class="dropdown-menu messages">
                                    <li class="dropdown-menu-title">
                                        <span>You have 9 messages</span>
                                        <a href="#refresh"><i class="icon-repeat"></i></a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="avatar">
                                                <img src="img/avatar.jpg" alt="Avatar"></span>
                                            <span class="header">
                                                <span class="from">Dennis Ji
										     </span>
                                                <span class="time">6 min
										    </span>
                                            </span>
                                            <span class="message">Lorem ipsum dolor sit amet consectetur adipiscing elit, et al commore
                                        </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="avatar">
                                                <img src="img/avatar.jpg" alt="Avatar"></span>
                                            <span class="header">
                                                <span class="from">Dennis Ji
										     </span>
                                                <span class="time">56 min
										    </span>
                                            </span>
                                            <span class="message">Lorem ipsum dolor sit amet consectetur adipiscing elit, et al commore
                                        </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="avatar">
                                                <img src="img/avatar.jpg" alt="Avatar"></span>
                                            <span class="header">
                                                <span class="from">Dennis Ji
										     </span>
                                                <span class="time">3 hours
										    </span>
                                            </span>
                                            <span class="message">Lorem ipsum dolor sit amet consectetur adipiscing elit, et al commore
                                        </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="avatar">
                                                <img src="img/avatar.jpg" alt="Avatar"></span>
                                            <span class="header">
                                                <span class="from">Dennis Ji
										     </span>
                                                <span class="time">yesterday
										    </span>
                                            </span>
                                            <span class="message">Lorem ipsum dolor sit amet consectetur adipiscing elit, et al commore
                                        </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="#">
                                            <span class="avatar">
                                                <img src="img/avatar.jpg" alt="Avatar"></span>
                                            <span class="header">
                                                <span class="from">Dennis Ji
										     </span>
                                                <span class="time">Jul 25, 2012
										    </span>
                                            </span>
                                            <span class="message">Lorem ipsum dolor sit amet consectetur adipiscing elit, et al commore
                                        </span>
                                        </a>
                                    </li>
                                    <li>
                                        <a class="dropdown-menu-sub-footer">View all messages</a>
                                    </li>
                                </ul>
                            </li>
                            <!-- end: Message Dropdown -->
                            <li>
                                <a class="btn" href="#">
                                    <i class="halflings-icon white wrench"></i>
                                </a>
                            </li>
                            <!-- start: User Dropdown -->
                            <li class="dropdown">
                                <a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
                                    <i class="halflings-icon white user"></i>Dennis Ji
							
                                <span class="caret"></span>
                                </a>
                                <ul class="dropdown-menu">
                                    <li class="dropdown-menu-title">
                                        <span>Account Settings</span>
                                    </li>
                                    <li><a href="#"><i class="halflings-icon user"></i>Profile</a></li>
                                    <li><a href="login.html"><i class="halflings-icon off"></i>Logout</a></li>
                                </ul>
                            </li>
                            <!-- end: User Dropdown -->
                        </ul>
                    </div>
                    <!-- end: Header Menu -->

                </div>
            </div>
        </div>
        <!-- start: Header -->
        <asp:UpdatePanel runat="server" ID="upMain">
            <ContentTemplate>
                <div class="container-fluid-full">
                    <div class="row-fluid">
                        <div id="content" class="span12">
                            <ul class="breadcrumb">
                                <li>
                                    <i class="icon-home"></i>
                                    <a href="Page1.aspx">Page1</a>
                                    <i class="icon-angle-right"></i>
                                </li>
                                <li id="lipage2">
                                    <i class="icon-edit"></i>
                                    <a href="PatientIntakeSheet.aspx">Page2</a>
                                    <i class="icon-angle-right"></i>
                                </li>
                                <li id="li1" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page4.aspx">Page3</a>
                                    <i class="icon-angle-right"></i>
                                </li>
                                <li id="li2" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page5.aspx">Page4</a>
                                </li>
                            </ul>
                            <div class="row-fluid">
                                <h2>Accident/Injury details</h2>
                              

                                <p>
                                    Accident description:<editable:EditableDropDownList runat="server" ID="ddl_accident_desc">
                                        <asp:ListItem Text="test" Value="0"></asp:ListItem>
                                    </editable:EditableDropDownList>
                                </p>
                                <p class="inline">
                                    Belt Restrained       
                             <editable:EditableDropDownList runat="server" ID="ddl_belt" Width="150px" CssClass="inline">
                                 <asp:ListItem Text="yes belted" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="no belted" Value="2"></asp:ListItem>

                             </editable:EditableDropDownList>&nbsp;
                           ;  Vehicle was involved in: 
                           <editable:EditableDropDownList runat="server" ID="ddl_invovledin" Width="150px" CssClass="inline">
                               <asp:ListItem Text="test" Value="1"></asp:ListItem>


                           </editable:EditableDropDownList>&nbsp;
                                </p>
                                <br />
                                <p style="display: inline; margin-top: 10px">
                                    EMS : &nbsp;
                             <editable:EditableDropDownList runat="server" ID="ddl_EMS" Width="150px" CssClass="inline">
                                 <asp:ListItem Text="test" Value="1"></asp:ListItem>
                             </editable:EditableDropDownList>&nbsp;
                            Hospitalized &nbsp;
                            <asp:RadioButtonList runat="server" ID="rep_hospitalized" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                                    Which hospital   &nbsp;
                            <asp:TextBox runat="server" ID="txt_hospital" CssClass="inline">
                                
                            </asp:TextBox>

                                </p>
                                <div id="hospitlediv" runat="server">
                                    <p>
                                        Went to the hospital same day Same day __ # of days later via 
                            <editable:EditableDropDownList runat="server" ID="ddl_via" Width="150px" CssClass="inline">
                                <asp:ListItem Text="ambulance" Value="0"></asp:ListItem>
                                <asp:ListItem Text="self" Value="1"></asp:ListItem>
                                <asp:ListItem Text="taxi" Value="2"></asp:ListItem>

                            </editable:EditableDropDownList>
                                        . At the hospital were any of the following done  
                            <asp:CheckBox runat="server" ID="chk_mri" />
                                        MRI
                            <input type="text" id="txt_mri" runat="server" />. 
                             <asp:CheckBox runat="server" ID="chk_CT" />
                                        CT
                            <input type="text" id="txt_CT" runat="server" />.
                             <asp:CheckBox runat="server" ID="chk_xray" />
                                        X-rays
                            <input type="text" id="txt_x_ray" runat="server" />.  
                            At the hospital prescription given for 
                                  <input type="text" id="txt_prescription" runat="server" />.

                            Upon impact, did you hit any part of your body? If yes, which part and on what?
                            <input type="text" id="txt_which_what" runat="server" />

                                    </p>
                                </div>
                                <p>
                                    Work Status :&nbsp;
                            <editable:EditableDropDownList runat="server" ID="ddl_work_status" Width="150" CssClass="inline">
                                <asp:ListItem Text="Test"></asp:ListItem>
                            </editable:EditableDropDownList>
                                </p>
                                <p>
                                    <asp:CheckBox runat="server" ID="chk_loc" Text="LOC" />
                                    <asp:CheckBox runat="server" ID="chk_headinjury" Text="Head Injury" />;   
                            If LOC then for how long? 
                            <input type="text" id="txt_howlong" runat="server" />.  	
                                </p>
                                <p class="inline">
                                    Have you seen any doctor for this injury:
                            
                            <asp:RadioButtonList runat="server" ID="rbl_seen_injury" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>

                                    If yes, name & address of the doctor<input type="text" id="txt_docname" runat="server" />. 
                                </p>

                                <p>
                                    Have you been injured in the past? 
                            <asp:RadioButtonList runat="server" ID="rbl_in_past" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                                    ; If yes, explain when and how  
                            <input type="text" runat="server" id="txt_injur_past" />
                                </p>
                                <p>
                                    During the accident injuries are reported to the following body parts:
                                </p>
                                <p>
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_Neck" Text=" Neck" />
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_Midback" Text=" Mid-back" />
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_lowback" Text=" Low-back" />
                                </p>
                                <table>
                                    <tr>
                                        <td width="200px">Right </td>
                                        <td width="200px">Left</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_r_Shoulder" Text=" Shoulder" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_L_Shoulder" Text=" Shoulder" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_r_Keen" Text=" Keen" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_L_Keen" Text=" Keen" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_r_Elbow" Text=" Elbow" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_l_Elbow" Text=" Elbow" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_r_Wrist" Text=" Wrist" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_l_Wrist" Text=" Wrist" /></td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_r_Hip" Text=" Hip" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_l_Hip" Text=" Hip" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_r_ankle" Text=" Ankle" />
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chk_l_ankle" Text=" Ankle" /></td>
                                    </tr>
                                </table>
                                <p>
                                    Other Injuries Sustained :
                            <input type="text" runat="server" id="txt_other" />
                                </p>
                                <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" UseSubmitBehavior="False" CssClass="btn btn-default" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script>
        $(document).ready(function () {
            $('#rbl_in_past input').change(function () {
                // The one that fires the event is always the
                // checked one; you don't need to test for this


                if ($(this).val() == '0')
                    $("#txt_injur_past").prop('disabled', true);
                else
                    $("#txt_injur_past").prop('disabled', false);
            });
        });

        $(document).ready(function () {
            $('#rep_hospitalized input').change(function () {
                // The one that fires the event is always the
                // checked one; you don't need to test for this


                if ($(this).val() == '0') {
                    $("#txt_hospital").prop('disabled', true);
                    $("#hospitlediv :input").attr('disabled', true);
                    $("#ddl_via").prop('disabled', true);
                }
                else {
                    $("#txt_hospital").prop('disabled', false);
                    $("#hospitlediv :input").attr('disabled', false);
                    $("#ddl_via").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rbl_seen_injury input').change(function () {
                // The one that fires the event is always the
                // checked one; you don't need to test for this


                if ($(this).val() == '0')
                    $("#txt_docname").prop('disabled', true);
                else
                    $("#txt_docname").prop('disabled', false);
            });
        });
    </script>
</body>
</html>
