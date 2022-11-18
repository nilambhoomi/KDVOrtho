<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="PatientDetails.aspx.cs" Inherits="PatientDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PainTrax - Intakesheet</title>
    <meta name="description" content="PainTrax">
    <meta name="author" content="Unaht">
    <!-- end: Meta -->

    <!-- start: Mobile Specific -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- end: Mobile Specific -->

    <!-- start: CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-ui-1.8.24.js"></script>

    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>

    <style>
        .navbar-inverse .nav .active > a, .navbar-inverse .nav .active > a:hover, .navbar-inverse .nav .active > a:focus {
            color: #fff;
            background-color: #5aef28db;
        }

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
    <style type="text/css">
        .lbl {
            font-size: 16px;
            font-style: italic;
            font-weight: bold;
        }

        .breadcrumb {
            /* margin: -28px -28px 28px -28px; */
            line-height: 24px;
            background: #eee;
            border: 0px;
            color: #aaa;
            -webkit-box-shadow: none;
            -moz-box-shadow: none;
            box-shadow: none;
            -webkit-border-radius: 0px;
            -moz-border-radius: 0px;
            border-radius: 0px;
        }
    </style>

    <script>
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
        $(document).ready(function ($) {
            var $j = jQuery.noConflict();;

            $('#<%=txt_mobile.ClientID%>').mask("999-999-9999");
            $('#<%=txt_home_ph.ClientID%>').mask("999-999-9999");
            $('#<%=txtRelationPhno.ClientID%>').mask("999-999-9999");
            $('#<%=txt_SSN.ClientID%>').mask("999-99-9999");

            $j('#<%=txt_DOB.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $('#<%=txt_DOB.ClientID%>').mask("99/99/9999");

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                $('#<%=txt_mobile.ClientID%>').mask("999-999-9999");
                $('#<%=txt_home_ph.ClientID%>').mask("999-999-9999");
                $('#<%=txtRelationPhno.ClientID%>').mask("999-999-9999");
                $('#<%=txt_SSN.ClientID%>').mask("999-99-9999");
                $('#<%=txt_DOB.ClientID%>').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (dateText, inst) {
                        $(this).focus();
                    }
                });
                $('#<%=txt_DOB.ClientID%>').mask("99/99/9999");
            }
        });

        function Validation(e) {

            var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
            if (!(date_regex.test($('#<%=txt_DOB.ClientID%>').val()))) {
                //alert("Date of Birth Should be MM/dd/yyyy format");
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" autocomplete="off" runat="server">
        <cc1:ToolkitScriptManager ID="toolkitScriptManager1" ScriptMode="Release" runat="server">
            <Scripts>
                <asp:ScriptReference Path="js/jquery-1.6.4.min.js" />
                <asp:ScriptReference Path="js/jquery.ui.core.js" />
                <asp:ScriptReference Path="js/jquery.ui.widget.js" />
                <asp:ScriptReference Path="js/jquery.ui.button.js" />
                <asp:ScriptReference Path="js/jquery.ui.position.js" />
                <asp:ScriptReference Path="js/jquery.ui.autocomplete.js" />
                <asp:ScriptReference Path="js/jquery.ui.combobox.js" />
            </Scripts>
        </cc1:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="upMain">
            <ContentTemplate>
                <nav class="navbar navbar-inverse navbar-fixed-top">
                    <div class="navbar-inner">
                        <div class="container-fluid">
                            <a class="brand" href="PatientIntakeList.aspx"><span>ePainTrax</span></a>
                            <ul id="nav" class="nav navbar-nav">
                            </ul>
                            <div class="nav-no-collapse header-nav">
                            </div>
                        </div>
                    </div>
                </nav>

                </div>
                                </nav>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="container-fluid" style="margin-top: 100px">
            <div class="row-fluid">
                <div id="content" class="span12">

                    <div class="">
                        <style>
                            .capitalize {
                                text-transform: capitalize;
                            }

                            select::after {
                                pointer-events: none;
                            }
                        </style>
                        <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
                        <script src="Scripts/jquery-1.8.2.min.js"></script>

                        <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
                        <script src="js/jquery-mask-1.14.8.min.js"></script>
                        <script src="js/jquery.maskedinput.js"></script>
                        <script src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>
                        <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>

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
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <div class="control-group span3">
                                        <label class="control-label">Last Name: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_lname" CssClass="capitalize" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_lname" Display="Dynamic" ErrorMessage="Please enter patient details" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">First Name: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_fname" CssClass="capitalize" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_fname" Display="Dynamic" ErrorMessage="Please enter patient details" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">MI: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_mname" CssClass="capitalize" TabIndex="3"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Sex: </label>
                                        <div class="controls">
                                            <asp:DropDownList runat="server" ID="ddl_gender" Width="90px" TabIndex="4">
                                                <asp:ListItem Value="0">-- Sex --</asp:ListItem>
                                                <asp:ListItem Value="Mr." Text="M"></asp:ListItem>
                                                <asp:ListItem Value="Ms." Text="F"></asp:ListItem>
                                            </asp:DropDownList><br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddl_gender" Display="Dynamic" ErrorMessage="Please select sex" SetFocusOnError="True" ValidationGroup="save" InitialValue="0"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div style="clear: both"></div>
                                <div class="form-horizontal">
                                    <div class="control-group span3">
                                        <label class="control-label">Address: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_add" TabIndex="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">City: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_city" TabIndex="6"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">State: </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddState" TabIndex="7" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddState" Display="Dynamic" ErrorMessage="Please Select State" SetFocusOnError="True" ValidationGroup="save" InitialValue='0'></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Zip: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_zip" Width="100px" TabIndex="8"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                                <div class="form-horizontal">

                                    <div class="control-group span3">
                                        <label class="control-label">Race/Ethnicity: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtRaceEthnicity" TabIndex="9"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Language: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtLanguage" TabIndex="10"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group span3">
                                        <label class="control-label">Home Ph: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_home_ph" placeholder="xxx-xxx-xxxx" TabIndex="11"></asp:TextBox>
                                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_home_ph" Display="Dynamic" ErrorMessage="invalid format for Home Ph." ValidationGroup="save"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Mobile: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_mobile" placeholder="xxx-xxx-xxxx" TabIndex="12"></asp:TextBox>
                                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_mobile" Display="Dynamic" ErrorMessage="invalid format for mobile." ValidationGroup="save"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                </div>
                                <div style="clear: both"></div>
                                <div class="form-horizontal">
                                    <div class="control-group span3">
                                        <label class="control-label">Email: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtEmail" TabIndex="13"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Please enter Email" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="save" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>

                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">SSN: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txt_SSN" placeholder="xxx-xx-xxxx" TabIndex="14" MaxLength="9"></asp:TextBox>
                                            <%--placeholder="xxx-xx-xxxx"  ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{2}[\s.-]\d{4}$"--%>
                                            <asp:RegularExpressionValidator runat="server" ID="reg1" ForeColor="Red" ControlToValidate="txt_SSN" Display="Dynamic" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{2}[\s.-]\d{4}$" ErrorMessage="SSN should be of 9 digit" ValidationGroup="save"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">DOB: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" AutoPostBack="true" ID="txt_DOB" placeholder="MM/dd/yyyy" TabIndex="15"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_DOB" Display="Dynamic" ErrorMessage="Please enter DOB" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        </div>
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_DOB" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                                            ErrorMessage="Invalid date format." ValidationGroup="save" />
                                        <asp:CustomValidator runat="server" ControlToValidate="txt_DOB" ErrorMessage="DOB should be MM/dd/yyyy format" Display="Dynamic" SetFocusOnError="True" ValidationGroup="save" OnServerValidate="CustomValidator1_ServerValidate" />
                                        <%--<asp:RegularExpressionValidator runat="server" ID="reg11" ValidationExpression="/^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/" ErrorMessage="Please enter valid date" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_DOB"></asp:RegularExpressionValidator>--%>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Marital Status: </label>
                                        <div class="controls">
                                            <asp:DropDownList runat="server" ID="ddlMaratialStatus" Width="120px" TabIndex="16">
                                                <asp:ListItem Text="Single"></asp:ListItem>
                                                <asp:ListItem Text="Married"></asp:ListItem>
                                                <asp:ListItem Text="Divorce"></asp:ListItem>
                                                <asp:ListItem Text="Widow"></asp:ListItem>
                                                <asp:ListItem Text="Separated"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                                <hr>
                                <div style="clear: both"></div>
                                <br />
                                <div class="form-horizontal">
                                    <div class="control-group span6">
                                        <label class="control-label">Patient Employment: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtPatientEmployment" Width="100%" TabIndex="17"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="control-group span3">
                                        <label class="control-label">Address: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtPatientEmploymentAddress" TabIndex="18"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">City: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtPatientEmploymentCity" TabIndex="19"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                                <div class="form-horizontal">
                                    <div class="control-group span3">
                                        <label class="control-label">State: </label>
                                        <div class="controls">
                                            <asp:DropDownList ID="ddlPatientEmploymentState" TabIndex="20" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlPatientEmploymentState" Display="Dynamic" ErrorMessage="Please Select State" SetFocusOnError="True" ValidationGroup="save" InitialValue='0'></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Zip: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtPatientEmploymentZip" TabIndex="21"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="control-group span3">
                                        <label class="control-label">Relation to Patient : </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtRelation" TabIndex="22"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group span3">
                                        <label class="control-label">Phone: </label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtRelationPhno" TabIndex="23"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div style="clear: both"></div>
                                    <div class="form-horizontal">
                                        <div class="control-group span6">
                                            <label class="control-label">Next of Kin in Case of Emergency : </label>
                                            <div class="controls">
                                                <asp:TextBox runat="server" ID="txtNextOfKinEmergency" Width="100%" TabIndex="24"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both"></div>
                                <div class="form-horizontal">
                                    <div class="control-group span6">
                                        <label class="control-label">Description Of Illness or injury:</label>
                                        <div class="controls">
                                            <asp:TextBox runat="server" ID="txtDescription" Width="100%" TextMode="MultiLine" Height="50px" TabIndex="25"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                </div>
                                <br />
                                <div style="clear: both"></div>
                                <div class="form-horizontal" style="height: 50px;">
                                    <div class="controls">
                                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" TabIndex="31" OnClick="btnSave_Click" ValidationGroup="save" />
                                        <%--OnClientClick="Validation();"--%>
                                    </div>
                                    <div class="controls">
                                        <asp:Label runat="server" ID="lblmess"></asp:Label>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
