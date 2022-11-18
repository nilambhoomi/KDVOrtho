<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PageMainMaster.master" CodeFile="OthersParts.aspx.cs" Inherits="OthersParts" %>


<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="js/images/bootstrap.min.js"></script>
    <style>
        .table_cell {
            width: 100px;
        }
    </style>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                document.getElementById('<%= btnSave.ClientID %>').click();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
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
    <div class="container">
        <div class="row">
            <div class="col-lg-10" id="content">


                <asp:HiddenField runat="server" ID="hidIE" />

                <%-- <audio src="RecordingFiles/test.mp3" runat="server" id="ccAudio" controls="controls">
                </audio>--%>
                <div style="display: none!important">
                    <div id='gUMArea' style="display: none">
                        <div>
                            Record:
        <input type="radio" name="media" value="video" checked id='mediaVideo' />Video
        <input type="radio" name="media" value="audio" />audio
                        </div>
                        <button class="btn btn-default" id='gUMbtn'>Request Stream</button>
                    </div>
                    <div id='btns' style="display: none">
                        <input type="button" class="btn btn-default" id='start' onclick="startRecording('start', 'stop')" value="Start Recording" />
                        <input type="button" class="btn btn-default" id='stop' onclick="stopRecording('start', 'stop')" value="Stop Recording" />
                    </div>
                    <div style="display: none">
                        <ul class="list-unstyled" id='ul'></ul>
                    </div>
                </div>

                <input type="hidden" id="recordType" />






                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">CHIEF COMPLAINT</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <div>
                            <asp:TextBox runat="server" ID="txtOthersCC" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                            <button type="button" id="start_button" onclick="startButton(event)">
                                <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                            <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span4" style="margin-top: 5px">

                        <input type="button" class="btn btn-default" id='startCC' onclick="startRecording('startCC', 'stopCC')" value="Start Recording" />
                        &nbsp;
                                <input type="button" class="btn btn-default" id='stopCC' onclick="stopRecord('startCC', 'stopCC', 'CC')" value="Stop Recording" />
                    </div>
                    <div class="span4">

                        <audio runat="server" id="ccAudio" style="display: none" controls="controls">
                        </audio>
                    </div>

                </div>
                <br />

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">PHYSICAL EXAM</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <div>
                            <asp:TextBox runat="server" ID="txtOthersPE" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                            <button type="button" id="start_button1" onclick="startButton1(event)">
                                <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                            <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>




                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="span4" style="margin-top: 5px">

                        <input type="button" class="btn btn-default" id='startPE' onclick="startRecording('startPE', 'stopPE')" value="Start Recording" />
                        &nbsp;
                                <input type="button" class="btn btn-default" id='stopPE' onclick="stopRecord('startPE', 'stopPE', 'PE')" value="Stop Recording" />
                    </div>
                    <div class="span4">

                        <audio runat="server" id="peAudio" style="display: none" controls="controls">
                        </audio>
                    </div>

                </div>
                <br />
                <asp:UpdatePanel runat="server" ID="upMedicine">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">ASSESSMENT/DIAGNOSIS</label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-9" style="margin-top: 5px">
                                <asp:TextBox runat="server" ID="txtOthersA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                <button type="button" id="start_button2" onclick="startButton2(event)">
                                    <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                                <div style="display: none"><span class="final" id="final_span2"></span><span class="interim" id="interim_span2"></span></div>
                                <%--  <asp:ImageButton ID="AddDiag" Style="text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="basicPopup1();return false;" OnClick="AddDiag_Click" />--%>

                                <asp:ImageButton ID="AddDiag" Style="text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDiag_Click" />


                            </div>

                        </div>
                        <div class="row">
                            <div class="span4" style="margin-top: 5px">

                                <input type="button" style="display: inline" class="btn btn-default" id='startAD' onclick="startRecording('startAD', 'stopAD')" value="Start Recording" />
                                &nbsp;
                                    <input type="button" class="btn btn-default" id='stopAD' onclick="stopRecord('startAD', 'stopAD', 'AD')" value="Stop Recording" />
                            </div>
                            <div class="span4">

                                <audio runat="server" id="aAudio" style="display: none" controls="controls">
                                </audio>
                            </div>

                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label"></label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                                <asp:GridView ID="dgvDiagCodes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Width="100" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hidDiagCodeID" Value='<%# Eval("Diag_Master_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="650">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtpe" runat="server" Width="700" Text='<%# Eval("Description") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="50">
                                            <ItemTemplate>
                                                <%--    <asp:HiddenField runat="server" ID="hidDiagCodeDetailID" Value='<%# Eval("DiagCodeDetail_ID") %>' />--%>
                                                <asp:CheckBox runat="server" ID="chkRemove" Checked="true" />

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <%--  <asp:GridView ID="dgvDiagCodes" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" ReadOnly="true" runat="server" Width="400" Text='<%# Eval("Description") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>--%>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>



                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">PLAN</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                    </div>
                </div>
                <div class="row">

                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtOthersP" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button3" onclick="startButton3(event)">
                            <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span3"></span><span class="interim" id="interim_span3"></span></div>
                        <asp:ImageButton ID="AddStd" runat="server" Height="50px" Width="50px" ImageUrl="~/img/a1.png" PostBackUrl="~/AddStandards.aspx" OnClientClick="basicPopup();return false;" />

                    </div>
                </div>
                <div class="row">
                    <div class="span4" style="margin-top: 5px">
                        <input type="button" class="btn btn-default" id='startP' onclick="startRecording('startP', 'stopP')" value="Start Recording" />
                        &nbsp;
                        <input type="button" class="btn btn-default" id='stopP' onclick="stopRecord('startP', 'stopP', 'PLAN')" value="Stop Recording" />
                    </div>
                    <div class="span4">
                        <audio runat="server" id="pAudio" style="display: none" controls="controls">
                        </audio>
                    </div>

                </div>
                <br />
                <div id="divTreatment" runat="server">
                    <asp:UpdatePanel runat="server" ID="upMain">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-2 inline">
                                    <label class="control-label"></label>
                                </div>
                                <div class="col-md-9 inline">
                                    <asp:Repeater runat="server" ID="repTreatMent">
                                        <HeaderTemplate>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td colspan="2"><b>TREATMENT:</b>  </td>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="width: 20px">
                                                    <asp:CheckBox runat="server" ID="chk" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("isChecked")) %>' OnCheckedChanged="chk_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" OnTextChanged="txtTreatment_TextChanged" TextMode="MultiLine" AutoPostBack="true" ID="txtTreatment" Text='<%# Eval("name") %>' Style="width: 100%" /></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-3">
                                    <label class="control-label">TREATMENT</label>
                                </div>
                                <div class="col-md-9" style="margin-top: 5px">
                                    <div>
                                        <asp:TextBox runat="server" ID="txtTreatmentParagraph" ReadOnly="true" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label class="control-label">IMPAIRMENT RATING</label>
                                </div>
                                <div class="col-md-9" style="margin-top: 5px">
                                    <div>
                                        <asp:TextBox runat="server" ID="txtIMPRating" Width="100px"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label class="control-label">CAUSAL RELATIONSHIP</label>
                                </div>
                                <div class="col-md-9" style="margin-top: 5px">
                                    <div>
                                        <asp:TextBox runat="server" ID="txtCR" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:GridView ID="dgvStandards" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodayParts") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Heading" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CC" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("CCDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("PEDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AD" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtadesc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("ADesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PD" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdesc" Width="95" ReadOnly="true" runat="server" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row"></div>
                <div class="row" style="margin-top: 15px">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <div style="display: none">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn blue" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="MedicinePopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; width: 950px; margin-right: 20%">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    Select Diag 
                    <b id="CatHeading"></b>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMedice">
                        <ContentTemplate>

                            <div class="row" style="margin: 5px">
                                <div class="col-md-3">
                                    <asp:TextBox ID="txDesc" runat="server" Style="margin-bottom: 0px" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnSearch" Text="Filter" CssClass="btn btn-info" OnClick="btnSearch_Click" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnDaigSave" Text="Save & Close" CssClass="btn btn-primary" OnClick="btnDaigSave_Click" />
                                </div>
                                <br />

                                <div class="col-md-12">
                                    <asp:GridView ID="dgvDiagCodesPopup" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="DiagCode_ID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">

                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' AutoPostBack="true" />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                                    <%--<asp:TextBox ID="txtbodypart" runat="server" Text='<%# Eval("BodyPart") %>'></asp:TextBox>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("DiagCode") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="550">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" Width="550" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PN" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PreSelect") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <script src="https://code.jquery.com/jquery-2.2.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <script src="Scripts/adapter.js"></script>
    <script src="Scripts/recorder.js"></script>

    <script type="text/javascript">
        function basicPopup() {
            popupWindow = window.open("AddStandards.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        };
        function basicPopup1() {
            popupWindow = window.open("AddDiagnosis.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        }

        function stopRecord(startId, stopId, type) {

            var ieId = document.getElementById('<%= hidIE.ClientID%>').value;

            stopRecording(startId, stopId, type, ieId, 0)
        }
    </script>
    <script>

        $.noConflict();
        function openModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('show');

            })(jQuery);
        }

        function closeModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('hide');


            })(jQuery);
        }

        var $j = jQuery.noConflict();
        $j('#MedicinePopup').on('hidden.bs.modal', function (e) {
            $('#ctl00_lnkbtn_neck').addClass('active');
        });



        $(document).ready(function () {
            $('#rbl_in_past input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_injur_past_bp").prop('disabled', true);
                    $("#txt_injur_past_how").prop('disabled', true);
                }
                else {
                    $("#txt_injur_past_bp").prop('disabled', false);
                    $("#txt_injur_past_how").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rbl_seen_injury input').change(function () {
                if ($(this).val() == 'False') {
                    $("#txt_docname").prop('disabled', true);
                }
                else {
                    $("#txt_docname").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rep_wenttohospital input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_day").prop('disabled', true);
                    $("#txt_day").prop('value', "0");
                }
                else {
                    $("#txt_day").prop('disabled', false);
                    $("#txt_day").select();
                    $("#txt_day").focus();
                }
            });
        });

        $(document).ready(function () {
            $('#rep_hospitalized input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_hospital").prop('disabled', true);
                    $("#txt_day").prop('disabled', true);
                    $("#chk_mri").prop('disabled', true);
                    $("#txt_mri").prop('disabled', true);
                    $("#chk_CT").prop('disabled', true);
                    $("#txt_CT").prop('disabled', true);
                    $("#chk_xray").prop('disabled', true);
                    $("#txt_x_ray").prop('disabled', true);
                    $("#txt_prescription").prop('disabled', true);
                    $("#txt_which_what").prop('disabled', true);
                }
                else {
                    $("#txt_hospital").prop('disabled', false);
                    $("#ddl_via").prop('disabled', false);
                    $("#txt_day").prop('disabled', false);
                    $("#chk_mri").prop('disabled', false);
                    $("#txt_mri").prop('disabled', false);
                    $("#chk_CT").prop('disabled', false);
                    $("#txt_CT").prop('disabled', false);
                    $("#chk_xray").prop('disabled', false);
                    $("#txt_x_ray").prop('disabled', false);
                    $("#txt_prescription").prop('disabled', false);
                    $("#txt_which_what").prop('disabled', false);
                }
            });
        });
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
                $('#<%=txtOthersCC.ClientID%>').text(linebreak(final_transcript));
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
            start_timestamp = event.timeStamp;
        }

    </script>
    <script>

        var final_transcript1 = '';
        var recognizing1 = false;
        var ignore_onend1;
        var start_timestamp1;
        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition1 = new webkitSpeechRecognition();
            recognition1.continuous = true;
            recognition1.interimResults = true;

            recognition1.onstart = function () {
                recognizing1 = true;
            };

            recognition1.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend1 = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend1 = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp1 < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend1 = true;
                }
            };

            recognition1.onend = function () {
                recognizing1 = false;
                if (ignore_onend1) {
                    return;
                }
                if (!final_transcript1) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition1.onresult = function (event) {
                var interim_transcript1 = '';
                if (typeof (event.results) == 'undefined') {
                    recognition1.onend = null;
                    recognition1.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript1 += event.results[i][0].transcript;
                    } else {
                        interim_transcript1 += event.results[i][0].transcript;
                    }
                }
                final_transcript1 = capitalize(final_transcript1);
                $('#<%=txtOthersPE.ClientID%>').text(linebreak(final_transcript1));
                interim_span1.innerHTML = linebreak(interim_transcript1);

            };
        }




        function startButton1(event) {
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript1 = '';
            recognition1.lang = 'en';
            recognition1.start();
            ignore_onend1 = false;
            final_span1.innerHTML = '';
            interim_span1.innerHTML = '';
            start_timestamp1 = event.timeStamp;
        }

    </script>

    <script>

        var final_transcript2 = '';
        var recognizing2 = false;
        var ignore_onend2;
        var start_timestamp2;
        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition2 = new webkitSpeechRecognition();
            recognition2.continuous = true;
            recognition2.interimResults = true;

            recognition2.onstart = function () {
                recognizing2 = true;
            };

            recognition2.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend2 = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend2 = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp2 < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend2 = true;
                }
            };

            recognition2.onend = function () {
                recognizing2 = false;
                if (ignore_onend2) {
                    return;
                }
                if (!final_transcript2) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition2.onresult = function (event) {
                var interim_transcript2 = '';
                if (typeof (event.results) == 'undefined') {
                    recognition2.onend = null;
                    recognition2.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript2 += event.results[i][0].transcript;
                    } else {
                        interim_transcript2 += event.results[i][0].transcript;
                    }
                }
                final_transcript2 = capitalize(final_transcript2);
                $('#<%=txtOthersA.ClientID%>').text(linebreak(final_transcript2));
                interim_span2.innerHTML = linebreak(interim_transcript2);

            };
        }




        function startButton2(event) {
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript2 = '';
            recognition2.lang = 'en';
            recognition2.start();
            ignore_onend2 = false;
            final_span2.innerHTML = '';
            interim_span2.innerHTML = '';
            start_timestamp2 = event.timeStamp;
        }

    </script>

    <script>

        var final_transcript3 = '';
        var recognizing3 = false;
        var ignore_onend3;
        var start_timestamp3;
        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition3 = new webkitSpeechRecognition();
            recognition3.continuous = true;
            recognition3.interimResults = true;

            recognition3.onstart = function () {
                recognizing3 = true;
            };

            recognition3.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend3 = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend3 = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp3 < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend3 = true;
                }
            };

            recognition3.onend = function () {
                recognizing3 = false;
                if (ignore_onend3) {
                    return;
                }
                if (!final_transcript3) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition3.onresult = function (event) {
                var interim_transcript3 = '';
                if (typeof (event.results) == 'undefined') {
                    recognition3.onend = null;
                    recognition3.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript3 += event.results[i][0].transcript;
                    } else {
                        interim_transcript3 += event.results[i][0].transcript;
                    }
                }
                final_transcript3 = capitalize(final_transcript3);
                $('#<%=txtOthersP.ClientID%>').text(linebreak(final_transcript3));
                interim_span3.innerHTML = linebreak(interim_transcript3);

            };
        }




        function startButton3(event) {
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript3 = '';
            recognition3.lang = 'en';
            recognition3.start();
            ignore_onend3 = false;
            final_span3.innerHTML = '';
            interim_span3.innerHTML = '';
            start_timestamp3 = event.timeStamp;
        }

    </script>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {

            MenuHighlight();

        });
    </script>
</asp:Content>
