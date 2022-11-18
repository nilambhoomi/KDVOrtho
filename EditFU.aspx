<%@ Page Title="" Language="C#" MasterPageFile="~/FollowUpMaster.master" AutoEventWireup="true" CodeFile="EditFU.aspx.cs" Inherits="EditFU" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />

    <script>
        function getInTime() {
            var d = new Date();
            var n = d.toLocaleString([], { hour12: true });
            $('#<%= PatientIntime.ClientID %>').val(n.replace(/,/g, ""));
        }
        function getOutTime() {
            var d = new Date();
            var n = d.toLocaleString([], { hour12: true });
            $('#<%= PatientOuttime.ClientID %>').val(n.replace(/,/g, ""));
        }

    </script>
    <style>
        .ui-button ui-widget ui-state-default ui-button-icon-only ui-corner-right ui-button-icon {
        }

        .ui-menu-item {
            /*display: none;*/
            position: absolute;
            background-color: #f9f9f9;
            min-width: 160px;
            box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
            z-index: 1;
        }

        .ui-corner-all a {
            color: black;
            /*padding: 12px 16px;*/
            text-decoration: none;
            display: block;
        }

        .labelcolor {
            color: black;
            font-weight: bold;
            font-size: 16px;
        }
    </style>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                saveDaynamicContent();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }

        function saveDaynamicContent() {
            debugger
            var htmlval = $("#ctl00_ContentPlaceHolder1_divdegreeHTML").html();
            $('#<%= hddegreeHTMLContent.ClientID %>').val(htmlval);

            htmlval = $("#ctl00_ContentPlaceHolder1_divsocialHTML").html();
            $('#<%= hdsocialHTMLContent.ClientID %>').val(htmlval);



            document.getElementById('<%= btnSave.ClientID %>').click();
        }

        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }

    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />


    <div class="col-12 alert alert-warning" id="alert_div" style="display: none">
        <p id="alert_txt"></p>
        <button type="button" class="close" data-dismiss="alert" style="top: -25px!important" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <table>
        <tr>
            <td>Patient In Time: </td>
            <td>
                <asp:TextBox ID="PatientIntime" TabIndex="28" runat="server"></asp:TextBox>
                <button type="button" class="btn btn-primary btn-small" onclick="getInTime()">Time In</button>
            </td>
            <td>Patient Out Time: </td>
            <td>
                <asp:TextBox ID="PatientOuttime" TabIndex="29" runat="server"></asp:TextBox>
                <button type="button" class="btn btn-primary btn-small" onclick="getOutTime()">Time Out</button>
            </td>
        </tr>
        <tr>
            <td>DOV:
            </td>
            <td>
                <asp:TextBox ID="txtDOV" CssClass="date" runat="server"></asp:TextBox>
            </td>
            <td>Last Name
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
            </td>
            <td>First Name
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
            </td>
            <td>MI
            </td>
            <td>
                <asp:TextBox ID="txtMI" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Location</td>
            <td>
                <asp:DropDownList ID="ddlLoaction" runat="server"></asp:DropDownList></td>
            <td>DOA</td>
            <td>
                <asp:TextBox ID="txtDOA" runat="server"></asp:TextBox></td>
            <td>Sex</td>
            <td>
                <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="Ms.">Female</asp:ListItem>
                    <asp:ListItem Value="Mr.">Male</asp:ListItem>
                </asp:RadioButtonList></td>
            <td>DOB</td>
            <td>
                <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox></td>

        </tr>
        <tr>
            <td>Home Ph</td>
            <td>
                <asp:TextBox ID="txtHomePh" runat="server"></asp:TextBox></td>
            <td>Work Ph</td>
            <td>
                <asp:TextBox ID="txtWorkPh" runat="server"></asp:TextBox></td>
            <td>Mobile</td>
            <td>
                <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox></td>
            <td>SSN</td>
            <td>
                <asp:TextBox ID="txtSSN" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Address</td>
            <td>
                <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
            <td>City</td>
            <td>
                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
            <td>State</td>
            <td>
                <asp:DropDownList ID="ddState" runat="server" DataTextField="name" DataValueField="name"></asp:DropDownList>
            </td>
            <%--<asp:XmlDataSource ID="XmlStateDataSource" runat="server" DataFile="~/Xml/USStates.xml"></asp:XmlDataSource>--%>
            <td>Zip</td>
            <td>
                <asp:TextBox ID="txtZip" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Insurance Co.</td>
            <td>
                <asp:DropDownList ID="ddlInsuranceCo" runat="server"></asp:DropDownList>
            </td>
            <td>Claim #</td>
            <td>
                <asp:TextBox ID="txtClaim" runat="server"></asp:TextBox></td>
            <td>Policy #</td>
            <td>
                <asp:TextBox ID="txtPolicy" runat="server"></asp:TextBox></td>
            <td>Case Type:</td>
            <td>
                <asp:DropDownList runat="server" ID="ddl_casetype" Width="150px" TabIndex="23">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Attorney Name</td>
            <td>
                <asp:TextBox ID="txtAttorneyName" runat="server"></asp:TextBox></td>
            <td>Attorney Ph</td>
            <td>
                <asp:TextBox ID="txtAttorneyPh" runat="server"></asp:TextBox></td>
            <td>MA & Providers</td>
            <td>
                <asp:TextBox ID="txtMAProviders" runat="server"></asp:TextBox>
                <% if (Session["pageAccess"].ToString().Contains("8"))
                    { %>
                <asp:Button ID="btneditprovider" CssClass="btn btn-primary btn-sm" runat="server" Text="Edit Provider" OnClick="btneditprovider_Click" />
                <%} %>
                <div class="col-sm-6">
                    <asp:ListBox CssClass="form-control" ID="lbSelectedMAandProviders" Visible="false" SelectionMode="Multiple" runat="server"></asp:ListBox>
                    <asp:Button ID="btnSavemaprovider" runat="server" Visible="false" CssClass="btn btn-primary btn-sm" Text="SaveMAProvider" OnClick="btnSavemaprovider_Click" />
                </div>

            </td>
            <td>Family History</td>
            <td>
                <asp:TextBox ID="FamilyHistory" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Past Medical History</td>
            <td>
                <asp:TextBox ID="PMH" runat="server"></asp:TextBox></td>
            <td>Past Surgical History</td>
            <td>
                <asp:TextBox ID="PSH" runat="server"></asp:TextBox></td>
            <td>Medication</td>
            <td>
                <asp:TextBox ID="Medication" runat="server"></asp:TextBox></td>
            <td>Allergies</td>
            <td>
                <asp:TextBox ID="Allergies" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Note</td>
            <td>
                <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td>Alert Note</td>
            <td>
                <asp:TextBox runat="server" ID="txtAlertNote" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                <p>
                    <strong>Work Condition:</strong><br />
                    <%--<asp:DropDownList ID="cboReturnToWork" runat="server"  Width="450px"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboRecevingPhyTherapy" runat="server"  Width="450px"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboFeelPainRelief" runat="server"  Width="450px"></asp:DropDownList><br />--%>

                    <editable:EditableDropDownList runat="server" ID="cboReturnToWork" Width="450px" CssClass="inline">
                    </editable:EditableDropDownList><br />
                    <editable:EditableDropDownList runat="server" ID="cboRecevingPhyTherapy" Width="450px" CssClass="inline">
                    </editable:EditableDropDownList><br />
                    <editable:EditableDropDownList runat="server" ID="cboFeelPainRelief" Width="450px" CssClass="inline">
                    </editable:EditableDropDownList><br />
                </p>
                <strong>Body Parts Affected:   </strong>
                <br />
                <%--  <asp:CheckBox ID="cbNeck" Text="Neck" runat="server" />
                <asp:CheckBox ID="cbMidBack" Text="Mid-Back" runat="server" />
                <asp:CheckBox ID="cbLowBack" Text="Low-Back" runat="server" />--%>
                <asp:CheckBox runat="server" ID="chk_Neck" Text=" Neck" AutoPostBack="true" OnCheckedChanged="chk_Neck_CheckedChanged" />
                <%--<asp:CheckBox runat="server" ID="chk_Neck" Text=" Neck" AutoPostBack="true" />--%>
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_Midback" Text=" Mid-back" AutoPostBack="true" OnCheckedChanged="chk_Midback_CheckedChanged" />
                <%--<asp:CheckBox runat="server" ID="chk_Midback" Text=" Mid-back"  AutoPostBack="true" />--%>
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_lowback" Text=" Low-back" AutoPostBack="true" OnCheckedChanged="chk_lowback_CheckedChanged" />

                <br />
                <br />
                <table>
                    <tr>
                        <td width="200px">Right </td>
                        <td width="200px">Left</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Shoulder" Text=" Shoulder" AutoPostBack="true" OnCheckedChanged="chk_r_Shoulder_CheckedChanged" />
                            <%--<asp:CheckBox runat="server" ID="chk_r_Shoulder" Text=" Shoulder" AutoPostBack="true" />--%>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_L_Shoulder" Text=" Shoulder" AutoPostBack="true" OnCheckedChanged="chk_L_Shoulder_CheckedChanged" /></td>
                        <%--<asp:CheckBox runat="server" ID="chk_L_Shoulder" Text=" Shoulder" AutoPostBack="true" /></td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Keen" Text=" Knee" AutoPostBack="true" OnCheckedChanged="chk_r_Keen_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_L_Keen" Text=" Knee" AutoPostBack="true" OnCheckedChanged="chk_L_Keen_CheckedChanged" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Elbow" Text=" Elbow" AutoPostBack="true" OnCheckedChanged="chk_r_Elbow_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_Elbow" Text=" Elbow" AutoPostBack="true" OnCheckedChanged="chk_l_Elbow_CheckedChanged" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Wrist" Text=" Wrist" AutoPostBack="true" OnCheckedChanged="chk_r_Wrist_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_Wrist" Text=" Wrist" AutoPostBack="true" OnCheckedChanged="chk_l_Wrist_CheckedChanged" /></td>
                    </tr>
                    <tr>

                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Hip" Text=" Hip" AutoPostBack="true" OnCheckedChanged="chk_r_Hip_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_Hip" Text=" Hip" AutoPostBack="true" OnCheckedChanged="chk_l_Hip_CheckedChanged" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_ankle" Text=" Ankle" AutoPostBack="true" OnCheckedChanged="chk_r_ankle_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_ankle" Text=" Ankle" AutoPostBack="true" OnCheckedChanged="chk_l_ankle_CheckedChanged" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding-left: 30px">


                <asp:HiddenField runat="server" ID="hddegreeHTMLContent" />
                <asp:HiddenField runat="server" ID="hdsocialHTMLContent" />


                <div id="divdegreeHTML" runat="server"></div>
                <div id="divsocialHTML" runat="server"></div>



                <div style="display: none">
                    <strong>Degree of Disability:</strong>
                    <asp:RadioButtonList ID="rblDOD" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="partial">Partial</asp:ListItem>
                        <asp:ListItem Value="25%">25%</asp:ListItem>
                        <asp:ListItem Value="50%">50%</asp:ListItem>
                        <asp:ListItem Value="75%">75%</asp:ListItem>
                        <asp:ListItem Value="100%">100%</asp:ListItem>
                        <asp:ListItem Value="none">None</asp:ListItem>
                    </asp:RadioButtonList><br />
                    <strong>Restrictions:</strong>
                    <asp:CheckBoxList ID="cblRestictions" runat="server" RepeatColumns="4">
                        <asp:ListItem Value="Bending / Twisting">Bending / Twisting</asp:ListItem>
                        <asp:ListItem Value="Climbing stairs/ladders">Climbing stairs/ladders</asp:ListItem>
                        <asp:ListItem Value="Environmental conditions">Environmental conditions</asp:ListItem>
                        <asp:ListItem Value="Kneeling">Kneeling</asp:ListItem>
                        <asp:ListItem Value="Lifting">Lifting</asp:ListItem>
                        <asp:ListItem Value="Operating heavy equipment">Operating heavy equipment</asp:ListItem>
                        <asp:ListItem Value="Operation of motor vehicles">Operation of motor vehicles</asp:ListItem>
                        <asp:ListItem Value="Personal protective equipment">Personal protective equipment</asp:ListItem>
                        <asp:ListItem Value="Sitting">Sitting</asp:ListItem>
                        <asp:ListItem Value="Standing">Standing</asp:ListItem>
                        <asp:ListItem Value="Use of public transportation">Use of public transportation</asp:ListItem>
                        <asp:ListItem Value="Use of upper extremities">Use of upper extremities</asp:ListItem>
                    </asp:CheckBoxList>
                    <strong>Others:</strong>
                    <asp:TextBox ID="txtOtherRestrictions" Width="750px" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <p>
                        SOCIAL HISTORY: &nbsp;
                         <asp:CheckBox runat="server" ID="chkDeniessmoking" />
                        smoking,  
                        <asp:CheckBox runat="server" ID="chkDeniesdrinking" />
                        drinking,   
                        <asp:CheckBox runat="server" ID="chkDeniesdrugs" />
                        drugs,  
                        <asp:CheckBox runat="server" ID="chkSocialdrinking" />
                        social drinking.
                    </p>
                    <br />
                    Vitals:  
                <asp:TextBox ID="txtVitals" Width="50%" runat="server"></asp:TextBox>
                    <br />
                    <strong>Work Status:</strong>
                    <%--<asp:CheckBoxList ID="cblWorkStatus" runat="server" RepeatColumns="2">
                    <asp:ListItem Value="Able to go back to work">Able to go back to work</asp:ListItem>
                    <asp:ListItem Value="Working">Working</asp:ListItem>
                    <asp:ListItem Value="Not Working">Not Working</asp:ListItem>
                    <asp:ListItem Value="Partially Working">Partially Working</asp:ListItem>
                </asp:CheckBoxList>--%>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <HeaderTemplate>
                            <table border="0" cellpadding="0" cellspacing="0">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cblWorkStatus" Text='<%# Eval("WorkStatus") %>' runat="server" />
                                    &nbsp; 
                    <div style="padding-left: 30px">
                        <asp:TextBox ID="txtCollageName" runat="server" />
                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </td>
        </tr>
        <tr>

            <td style="padding-left: 30px"><strong class="pull-left">Notes: </strong>
                <asp:TextBox runat="server" Style="float: left;" ID="txtFreeForm" TextMode="MultiLine" Width="500px" Height="100px"></asp:TextBox>
                <button type="button" id="start_button" onclick="startButton(event)">
                    <img src="images/mic.png" alt="start" width="50px" height="50px" /></button>
                <div style="display: none">
                    <span class="final" id="final_span"></span><span class="interim" id="interim_span"></span>
                </div>
            </td>
        </tr>
        <tr>
            <td></td>

            <td style="padding-left: 30px">
                <strong>Folowed Up On:</strong>
                <asp:TextBox ID="txtFollowedUpOn" CssClass="date" Width="750px" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>

    <br />

    <asp:HiddenField ID="hfPatientId" runat="server" />
    <asp:HiddenField ID="hfPatientFUId" runat="server" />
    <asp:HiddenField ID="hfPatientIEId" runat="server" />
    <div style="display: none">
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
    </div>
    <asp:Button ID="brnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
    <br />
    <br />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="js/jquery-ui-timepicker-addon.js"></script>
    <script>
        var $j = jQuery.noConflict();

        $j(document).ready(function () {
            $j(".date").datepicker();
            $j('#<%=PatientIntime.ClientID%>').datetimepicker({
                controlType: 'select',
                oneLine: true,
                timeFormat: 'hh:mm tt'
            });
            $j('#<%=PatientOuttime.ClientID%>').datetimepicker({
                controlType: 'select',
                oneLine: true,
                timeFormat: 'hh:mm tt'
            });
        });

        function setAlert(msg) {
            $('#alert_txt').text(msg);
            $('#alert_div').show();
        }
    </script>
    <script>

        var final_transcript = '';
        var recognizing = false;
        var ignore_onend;
        var start_timestamp;
        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition = new webkitSpeechRecognition();
            recognition.continuous = true;
            recognition.interimResults = true;

            recognition.onstart = function () {
                recognizing = true;
            };

            recognition.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend = true;
                }
            };

            recognition.onend = function () {
                recognizing = false;
                if (ignore_onend) {
                    return;
                }
                if (!final_transcript) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition.onresult = function (event) {
                var interim_transcript = '';
                if (typeof (event.results) == 'undefined') {
                    recognition.onend = null;
                    recognition.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript += event.results[i][0].transcript;
                    } else {
                        interim_transcript += event.results[i][0].transcript;
                    }
                }
                final_transcript = capitalize(final_transcript);
                $('#ctl00_ContentPlaceHolder1_txtFreeForm').text(linebreak(final_transcript));
                interim_span.innerHTML = linebreak(interim_transcript);

            };
        }


        var two_line = /\n\n/g;
        var one_line = /\n/g;
        function linebreak(s) {
            return s.replace(two_line, '<p></p>').replace(one_line, '<br>');
        }

        var first_char = /\S/;
        function capitalize(s) {
            return s.replace(first_char, function (m) { return m.toUpperCase(); });
        }

        function startButton(event) {
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span.innerHTML = '';
            interim_span.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }

    </script>
</asp:Content>

