<%@ Page Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Page2.aspx.cs" Inherits="Page2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>





<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <%--<script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>--%>
    <script src="js/jquery.ui.core.js" type="text/javascript"></script>
    <script src="js/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="js/jquery.ui.button.js" type="text/javascript"></script>
    <script src="js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="js/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="js/jquery.ui.combobox.js" type="text/javascript"></script>

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
    <style>
        button.ui-button-icon-only {
            width: 1.4em;
            height: 27px;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
    </script>
    <script>
        //function openPopup(divid) {

        //    $('#' + divid + '').modal('show');

        //}

        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //alert();
                //var currentURL = window.location.href;
                <%-- document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                document.getElementById('<%= btnSave.ClientID %>').click();--%>
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                callonsubmit();
                saveDaynamicContent();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
            callonsubmit();
            saveDaynamicContent();
           <%-- document.getElementById('<%= btnSave.ClientID %>').click();--%>
        }

        function saveDaynamicContent() {
            debugger
            var htmlval = $("#ctl00_ContentPlaceHolder1_divtopHTML").html();
            $('#<%= hdtopHTMLContent.ClientID %>').val(htmlval);

            htmlval = $("#ctl00_ContentPlaceHolder1_divsocialHTML").html();
            $('#<%= hdsocialHTMLContent.ClientID %>').val(htmlval);


            htmlval = $("#ctl00_ContentPlaceHolder1_divaccidentHTML").html();
            $('#<%= hdaccidentHTMLContent.ClientID %>').val(htmlval);

            htmlval = $("#ctl00_ContentPlaceHolder1_divhistoryHTML").html();
            $('#<%= hdhistoryHTMLContent.ClientID %>').val(htmlval);



            document.getElementById('<%= btnSave.ClientID %>').click();
        }


        function accidentdesc() {

            var compensation = '<%= Session["compensation"]%>';

            if (compensation !== null && compensation !== '') {

                if (compensation.toLowerCase() === "wc")
                    $('#txt_accident_desc').attr('value', 'presents for the evaluation of the injuries sustained in a Work Related Incident');
                else if (compensation.toLowerCase() === "mw")
                    $('#txt_accident_desc').attr('value', 'presents with complaints of pain in the');
                else if (compensation.toLowerCase() === "nf")
                    $('#txt_accident_desc').attr('value', 'presents for the evaluation of the injuries sustained in a motor vehicle accident');
                else if (compensation.toLowerCase() === "lien")
                    $('#txt_accident_desc').attr('value', 'presents for the evaluation of the injuries sustained in a motor vehicle accident');
                else if (compensation.toLowerCase() === "taxi")
                    $('#txt_accident_desc').attr('value', 'presents for the evaluation of the injuries sustained in a Work Related Incident');
            }
        }


        $(document).ready(function () {

            debugger





            //debugger;

            //window.onload = function () {
            //    if (!window.location.hash) {
            //        window.location = window.location + '#loaded';
            //        window.location.reload();
            //    }
            //}
            var radioButtons = $('#<%=rep_hospitalized.ClientID%>');
            var id = radioButtons.find('input:checked').val();
            if (id == '0' || id == '') {
                $('#<%=txt_hospital.ClientID%>').prop('disabled', true);
                $('#<%=txt_day.ClientID%>').prop('disabled', true);
                $('#<%=chk_mri.ClientID%>').prop('disabled', true);
                $('#<%=txt_mri.ClientID%>').prop('disabled', true);
                $('#<%=chk_CT.ClientID%>').prop('disabled', true);
                $('#<%=txt_CT.ClientID%>').prop('disabled', true);
                $('#<%=chk_xray.ClientID%>').prop('disabled', true);
                $('#<%=txt_x_ray.ClientID%>').prop('disabled', true);
                $('#<%=txt_prescription.ClientID%>').prop('disabled', true);
                $('#<%=txt_which_what.ClientID%>').prop('disabled', true);
            }
            else {
                $('#<%=txt_hospital.ClientID%>').prop('disabled', false);
                $('#<%=ddl_via.ClientID%>').prop('disabled', false);
                $('#<%=txt_day.ClientID%>').prop('disabled', false);
                $('#<%=chk_mri.ClientID%>').prop('disabled', false);
                $('#<%=txt_mri.ClientID%>').prop('disabled', false);
                $('#<%=chk_CT.ClientID%>').prop('disabled', false);
                $('#<%=txt_CT.ClientID%>').prop('disabled', false);
                $('#<%=chk_xray.ClientID%>').prop('disabled', false);
                $('#<%=txt_x_ray.ClientID%>').prop('disabled', false);
                $('#<%=txt_prescription.ClientID%>').prop('disabled', false);
                $('#<%=txt_which_what.ClientID%>').prop('disabled', false);
            }
            //debugger
            var rbl_seen_injury = $('#<%=rbl_seen_injury.ClientID%>');
            var rbl_seen_injury_id = radioButtons.find('input:checked').val();
            if ($("#<%= rbl_seen_injury.ClientID %> input:radio:checked").val() == '1') {
                $('#<%=txt_docname.ClientID%>').prop('disabled', false);
            }
            else {
                $('#<%=txt_docname.ClientID%>').prop('disabled', true);
            }
            $('#<%=rbl_seen_injury.ClientID%> input').change(function () {
                if ($(this).val() == '0') {
                    $('#<%=txt_docname.ClientID%>').prop('disabled', true);
                }
                else {
                    $('#<%=txt_docname.ClientID%>').prop('disabled', false);
                }
            });

            var rbl_in_past = $('#<%=rbl_in_past.ClientID%>');
            var rbl_in_past_id = radioButtons.find('input:checked').val();
            if ($("#<%= rbl_in_past.ClientID %> input:radio:checked").val() == '1') {
                $('#<%=txt_injur_past_bp.ClientID%>').prop('disabled', false);
                $('#<%=txt_injur_past_how.ClientID%>').prop('disabled', false);
            }
            else {
                $('#<%=txt_injur_past_bp.ClientID%>').prop('disabled', true);
                $('#<%=txt_injur_past_how.ClientID%>').prop('disabled', true);
            }

            $('#<%=rbl_in_past.ClientID%> input').change(function () {
                if ($(this).val() == '0') {
                    $('#<%=txt_injur_past_bp.ClientID%>').prop('disabled', true);
                    $('#<%=txt_injur_past_how.ClientID%>').prop('disabled', true);
                }
                else {
                    $('#<%=txt_injur_past_bp.ClientID%>').prop('disabled', false);
                    $('#<%=txt_injur_past_how.ClientID%>').prop('disabled', false);
                }
            });
            var rep_wenttohospital = $('#<%=rep_wenttohospital.ClientID%>');
            var rep_wenttohospital_id = radioButtons.find('input:checked').val();
            if (rep_wenttohospital_id == '0') {
                $('#<%=txt_day.ClientID%>').prop('disabled', true);
                $('#<%=txt_day.ClientID%>').prop('value', "0");
            }
            else {
                $('#<%=txt_day.ClientID%>').prop('disabled', false);
                $('#<%=txt_day.ClientID%>').select();
                $('#<%=txt_day.ClientID%>').focus();
            }
            $('#<%=rep_wenttohospital.ClientID%> input').change(function () {
                if ($(this).val() == '0') {
                    $('#<%=txt_day.ClientID%>').prop('disabled', true);
                    $('#<%=txt_day.ClientID%>').prop('value', "0");
                }
                else {
                    $('#<%=txt_day.ClientID%>').prop('disabled', false);
                    $('#<%=txt_day.ClientID%>').select();
                    $('#<%=txt_day.ClientID%>').focus();
                }
            });

            $('#<%=rep_hospitalized.ClientID%> input[type="radio"]').change(function () {
                //debugger;
                if ($(this).val() == '0') {
                    $('#<%=txt_hospital.ClientID%>').prop('disabled', true);
                    $('#<%=txt_day.ClientID%>').prop('disabled', true);
                    $('#<%=chk_mri.ClientID%>').prop('disabled', true);
                    $('#<%=txt_mri.ClientID%>').prop('disabled', true);
                    $('#<%=chk_CT.ClientID%>').prop('disabled', true);
                    $('#<%=txt_CT.ClientID%>').prop('disabled', true);
                    $('#<%=chk_xray.ClientID%>').prop('disabled', true);
                    $('#<%=txt_x_ray.ClientID%>').prop('disabled', true);
                    $('#<%=txt_prescription.ClientID%>').prop('disabled', true);
                    $('#<%=txt_which_what.ClientID%>').prop('disabled', true);
                }
                else {
                    $('#<%=txt_hospital.ClientID%>').prop('disabled', false);
                    $('#<%=ddl_via.ClientID%>').prop('disabled', false);
                    $('#<%=txt_day.ClientID%>').prop('disabled', false);
                    $('#<%=chk_mri.ClientID%>').prop('disabled', false);
                    $('#<%=txt_mri.ClientID%>').prop('disabled', false);
                    $('#<%=chk_CT.ClientID%>').prop('disabled', false);
                    $('#<%=txt_CT.ClientID%>').prop('disabled', false);
                    $('#<%=chk_xray.ClientID%>').prop('disabled', false);
                    $('#<%=txt_x_ray.ClientID%>').prop('disabled', false);
                    $('#<%=txt_prescription.ClientID%>').prop('disabled', false);
                    $('#<%=txt_which_what.ClientID%>').prop('disabled', false);
                }
            });
            //$('#ctl00_ContentPlaceHolder1_ddl_accident_desc_list').removeAttr('style');
            //$('#ctl00_ContentPlaceHolder1_ddl_accident_desc_list').addClass('form-control');
            //$('#ctl00_ContentPlaceHolder1_ddl_accident_desc_list_button').find('span').removeClass('ui-button-text');
        });






    </script>

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
    <script>
        function test(url) {

            $('#bodyui').hide();

            var $iframe = $('#neckui');
            if ($iframe.length) {
                $iframe.attr('src', url + '.aspx');
                $('#neckui').show();
                return false;
            }
            return false;
        }
    </script>

    <asp:HiddenField ID="pageHDN" runat="server" />
    <div id="bodyui" style="margin-top: 15px">

        <div style="display: none">

            <p>
                Past Medical History
                                          <%--<asp:TextBox ID="PMH" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="PMH" runat="server"></asp:TextBox>
                Past Surgical History
                                         <%--<asp:TextBox ID="PSH" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="PSH" runat="server"></asp:TextBox>
                Medication
                                       <%--<asp:TextBox ID="Medication" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="Medication" runat="server"></asp:TextBox>
                Allergies
                                          <%--<asp:TextBox ID="Allergies" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="Allergies" runat="server"></asp:TextBox>
                Family History
                            <asp:TextBox ID="FamilyHistory" runat="server"></asp:TextBox>
                <p>
                    <b class="labelcolor">Social History:</b> &nbsp;
                         <asp:CheckBox runat="server" ID="chkDeniessmoking" />
                    smoking,  
                        <asp:CheckBox runat="server" ID="chkDeniesdrinking" />
                    drinking,   
                         <asp:CheckBox runat="server" ID="chkDeniesdrugs" />
                    drugs,  
                         <asp:CheckBox runat="server" ID="chkSocialdrinking" />
                    social drinking.
                </p>
                <b class="labelcolor">Vitals:</b>
                <asp:TextBox ID="txtVitals" Width="50%" runat="server"></asp:TextBox>
            </p>
            <hr />
            <div class="col-md-3">
                <h4 class="labelcolor">Accident / Injury details</h4>

                <%--<p>
                <asp:TextBox runat="server" ID="txt_details" TextMode="MultiLine" Width="650px"></asp:TextBox>
            </p>--%>
                <p>
                    <asp:TextBox runat="server" Style="float: left;" ID="txt_details" TextMode="MultiLine" Width="650px"></asp:TextBox>
                    <button type="button" id="start_button" onclick="startButton(event)">
                        <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                    <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>

                </p>
                <br />
                <p>
                    <br />
                    <b class="labelcolor">Accident description:</b>
                    <editable:EditableDropDownList runat="server" ID="ddl_accident_desc" Width="500px">
                    </editable:EditableDropDownList>

                    <%--  <asp:ListItem Text="presents with complaints of " Value="0"></asp:ListItem>
                                    <asp:ListItem Text="presents for the evaluation of the injuries sustained in a motor vehicle accident" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="presents for the evaluation of the injuries sustained as a pedestrian struck" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="presents for the evaluation of the injuries sustained as a cyclist struck" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="presents for the evaluation of the injuries sustained in a Work Related Incident" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="presents for the evaluation of the injuries sustained slip and fall" Value="5"></asp:ListItem>--%>
                </p>
            </div>



            <p class="inline">
                Belt Restrained   
                             <editable:EditableDropDownList runat="server" ID="ddl_belt" Width="257px" CssClass="inline">
                                 <%-- <asp:ListItem Text="restrained driver" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="front seat passenger" Value="2"></asp:ListItem>
                                 <asp:ListItem Text="right rear passenger" Value="3"></asp:ListItem>
                                 <asp:ListItem Text="left rear passenger" Value="4"></asp:ListItem>--%>
                             </editable:EditableDropDownList>

                &nbsp;
                           , Vehicle was involved in: 
                           <editable:EditableDropDownList runat="server" ID="ddl_invovledin" Width="207px" CssClass="inline">
                               <%--<asp:ListItem Text="rear end" Value="1"></asp:ListItem>
                               <asp:ListItem Text="head on" Value="2"></asp:ListItem>
                               <asp:ListItem Text="driver’s side front" Value="3"></asp:ListItem>
                               <asp:ListItem Text="driver’s side rear" Value="4"></asp:ListItem>
                               <asp:ListItem Text="passenger side front" Value="5"></asp:ListItem>
                               <asp:ListItem Text="passenger side rear" Value="5"></asp:ListItem>--%>
                           </editable:EditableDropDownList>

                collision&nbsp;
            <asp:TextBox ID="txtInvolvedOther" CssClass="inline" Style="display: none" runat="server"></asp:TextBox>
            </p>
            <br />
            <p style="display: inline; margin-top: 10px">
                <b class="labelcolor">EMS :</b> &nbsp;
                             <editable:EditableDropDownList runat="server" ID="ddl_EMS" Width="150px" CssClass="inline">
                                 <%--<asp:ListItem Text="" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="arrived" Value="2"></asp:ListItem>
                                 <asp:ListItem Text="did not arrive at the scene" Value="3"></asp:ListItem>--%>
                             </editable:EditableDropDownList>

                &nbsp;
                            Hospitalized
                            <asp:RadioButtonList runat="server" ID="rep_hospitalized" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                &nbsp;Which hospital&nbsp;
                            <asp:TextBox runat="server" ID="txt_hospital" CssClass="inline">
                            </asp:TextBox>
                Went to the hospital
                            <asp:RadioButtonList runat="server" ID="rep_wenttohospital" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="same day" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                <input type="text" id="txt_day" runat="server" value="2" style="background: none; border-bottom: 1px solid; width: 10px" />
                day(s) later via 
                             <editable:EditableDropDownList runat="server" ID="ddl_via" Width="150px" CssClass="inline">
                                 <%--       <asp:ListItem Text="" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="ambulance" Value="2" Selected="True"></asp:ListItem>
                                 <asp:ListItem Text="taxi" Value="3"></asp:ListItem>--%>
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
                                <input type="text" id="txt_which_what" runat="server" visible="false" />
            </p>

            <p>
                <b class="labelcolor">Work Status :</b>&nbsp;
                            <editable:EditableDropDownList runat="server" ID="ddl_work_status" Width="250" CssClass="inline">
                                <%-- <asp:ListItem Text="Patient works as unknown" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Patient works as " Value="2"></asp:ListItem>
                                <asp:ListItem Text="Patient does not work" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Patient is a student" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Patient is retired" Value="5"></asp:ListItem>--%>
                            </editable:EditableDropDownList>

                has missed 
            <asp:TextBox ID="txtMissed" runat="server"></asp:TextBox>
                of work after the accident. 
            
            <editable:EditableDropDownList runat="server" ID="cboReturnedToWork" Width="250" CssClass="inline">
            </editable:EditableDropDownList>
            </p>
            <p>
                <asp:CheckBox runat="server" ID="chk_headinjury" Text="Head Injury" />;   
                                    <asp:CheckBox runat="server" ID="chk_loc" Text="LOC" />
                &nbsp;If LOC then for how long? 
                            <input type="text" id="txt_howlong" runat="server" />. 
                                    <asp:DropDownList runat="server" ID="ddl_howlong">
                                        <%--<asp:ListItem Text="undetermined time"></asp:ListItem>
                                        <asp:ListItem Text="seconds"></asp:ListItem>
                                        <asp:ListItem Text="minutes"></asp:ListItem>
                                        <asp:ListItem Text="hours"></asp:ListItem>
                                        <asp:ListItem Text="day(s)"></asp:ListItem>
                                        <asp:ListItem Text="hour(s)"></asp:ListItem>--%>
                                    </asp:DropDownList>

            </p>
            <p class="inline">
                Have you seen any doctor for this injury:
                            <asp:RadioButtonList runat="server" ID="rbl_seen_injury" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                If yes, name & address of the doctor<input type="text" id="txt_docname" runat="server" />. 
            </p>
            <p>
                Have you been injured in the past? 
                            <asp:RadioButtonList runat="server" ID="rbl_in_past" RepeatDirection="Horizontal" CssClass="margintop inline">
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                , If yes which body part  
                            <input type="text" runat="server" id="txt_injur_past_bp" />
                and how
                                    <input type="text" runat="server" id="txt_injur_past_how" />
                ?
            </p>

            <p>
                <asp:CheckBox ID="chkComplainingofHeadaches" runat="server" />
                The patient is complaining of headaches as a result of the accident. The headaches started after the accident and are 
                         <asp:TextBox ID="txtPersistent" runat="server"></asp:TextBox>. 
            <asp:CheckBox ID="chkHeadechesAssociated" runat="server" Text="The headaches are associated with nausea and dizziness." />
                The headaches are 
                         <asp:CheckBox ID="chkfrontal" runat="server" Text=" frontal " />
                <asp:CheckBox ID="chkLeftParietal" runat="server" Text=" left parietal  " />
                <asp:CheckBox ID="chkRightParietal" runat="server" Text=" right parietal  " />
                <asp:CheckBox ID="chkLeftTemporal" runat="server" Text=" left temporal " />
                <asp:CheckBox ID="chkRightTemporal" runat="server" Text=" right temporal " />
                <asp:CheckBox ID="chkOccipital" runat="server" Text=" occipital" />
                <asp:CheckBox ID="chkGlobal" runat="server" Text=" global." />
            </p>
            <p>
                The patient reports
            <asp:CheckBox Text=" Anxiety  " runat="server" ID="chkSevereAnxiety" />
                <asp:CheckBox ID="chkNausea" runat="server" Text=" nausea " />
                <asp:CheckBox ID="chkDizziness" runat="server" Text=" dizziness  " />
                <asp:CheckBox ID="chkVomitting" runat="server" Text=" vomiting." />
            </p>
        </div>

        <asp:HiddenField runat="server" ID="hdtopHTMLContent" />
        <asp:HiddenField runat="server" ID="hdsocialHTMLContent" />
        <asp:HiddenField runat="server" ID="hdaccidentHTMLContent" />
        <asp:HiddenField runat="server" ID="hdhistoryHTMLContent" />
        <asp:HiddenField runat="server" ID="hdhistoryHTMLValue" />

        <div id="divtopHTML" runat="server"></div>
        <div id="divsocialHTML" runat="server"></div>
        <div id="divaccidentHTML" runat="server"></div>
        <div id="divhistoryHTML" runat="server"></div>

        <br />

        <asp:UpdatePanel runat="server" ID="upmenu">
            <ContentTemplate>
                <p>
                    During the accident injuries are reported to the following body parts:
                </p>
                <p>
                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_Neck" Text=" Neck" AutoPostBack="true" OnCheckedChanged="chk_Neck_CheckedChanged" />
                    <asp:Button runat="server" ID="btnDel" Text="Delete" CommandArgument="tblbpNeck" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                    <%--<asp:CheckBox runat="server" ID="chk_Neck" Text=" Neck" AutoPostBack="true" />--%>
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_Midback" Text=" Mid-back" AutoPostBack="true" OnCheckedChanged="chk_Midback_CheckedChanged" />
                    <asp:Button runat="server" ID="Button2" Text="Delete" CommandArgument="tblbpMidback" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                    <%--<asp:CheckBox runat="server" ID="chk_Midback" Text=" Mid-back"  AutoPostBack="true" />--%>
                                    &nbsp;
                            <asp:CheckBox runat="server" ID="chk_lowback" Text=" Low-back" AutoPostBack="true" OnCheckedChanged="chk_lowback_CheckedChanged" />
                    <asp:Button runat="server" ID="Button3" Text="Delete" CommandArgument="tblbpLowback" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                    <%--<asp:CheckBox runat="server" ID="chk_lowback" Text=" Low-back" AutoPostBack="true" />--%>
                </p>
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
                            <asp:CheckBox runat="server" ID="chk_L_Shoulder" Text=" Shoulder" AutoPostBack="true" OnCheckedChanged="chk_L_Shoulder_CheckedChanged" />
                            <asp:Button runat="server" ID="Button4" Text="Delete" CommandArgument="tblbpShoulder" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                        </td>

                        <%--<asp:CheckBox runat="server" ID="chk_L_Shoulder" Text=" Shoulder" AutoPostBack="true" /></td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Keen" Text=" Knee" AutoPostBack="true" OnCheckedChanged="chk_r_Keen_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_L_Keen" Text=" Knee" AutoPostBack="true" OnCheckedChanged="chk_L_Keen_CheckedChanged" />
                            <asp:Button runat="server" ID="Button5" Text="Delete" CommandArgument="tblbpKnee" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Elbow" Text=" Elbow" AutoPostBack="true" OnCheckedChanged="chk_r_Elbow_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_Elbow" Text=" Elbow" AutoPostBack="true" OnCheckedChanged="chk_l_Elbow_CheckedChanged" />
                            <asp:Button runat="server" ID="Button6" Text="Delete" CommandArgument="tblbpElbow" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Wrist" Text=" Wrist" AutoPostBack="true" OnCheckedChanged="chk_r_Wrist_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_Wrist" Text=" Wrist" AutoPostBack="true" OnCheckedChanged="chk_l_Wrist_CheckedChanged" />
                            <asp:Button runat="server" ID="Button7" Text="Delete" CommandArgument="tblbpWrist" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_Hip" Text=" Hip" AutoPostBack="true" OnCheckedChanged="chk_r_Hip_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_Hip" Text=" Hip" AutoPostBack="true" OnCheckedChanged="chk_l_Hip_CheckedChanged" />
                            <asp:Button runat="server" ID="Button8" Text="Delete" CommandArgument="tblbpHip" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_r_ankle" Text=" Ankle" AutoPostBack="true" OnCheckedChanged="chk_r_ankle_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_l_ankle" Text=" Ankle" AutoPostBack="true" OnCheckedChanged="chk_l_ankle_CheckedChanged" />
                            <asp:Button runat="server" ID="Button9" Text="Delete" CommandArgument="tblbpAnkle" CausesValidation="false" OnClick="btnDel_Click" Visible="false"/>
                        </td>
                    </tr>
                </table>
                 <br />
                <p>
                    Other :
                </p>
                <br />
                <asp:TextBox type="text" runat="server" ID="txt_other_val" Width="100%" TextMode="MultiLine" />
                  <p>
                    Other Injuries Sustained :
                </p>
                <asp:TextBox type="text" runat="server" ID="txt_other" Width="100%" TextMode="MultiLine" />
               
                <br />
                <p>
                    ADL CAPABILITIES :
                </p>
                <asp:TextBox type="text" runat="server" ID="txt_ADL_Content" Width="100%" TextMode="MultiLine" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="display: none">
            <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ID="btnSave" />
        </div>
        <asp:Button runat="server" ID="Button1" PostBackUrl="~/PatientIntakeList.aspx" Text="Back to List" CssClass="btn btn-default" UseSubmitBehavior="False" />
    </div>

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
                $('#<%=txt_details.ClientID%>').text(linebreak(final_transcript));
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

