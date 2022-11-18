<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="ProcedureList_oldcoppy.aspx.cs" Inherits="Procedures" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .pager::before {
            display: none;
        }

        .pager table {
            margin: 0 auto;
        }

            .pager table tbody tr td a,
            .pager table tbody tr td span {
                position: relative;
                float: left;
                padding: 6px 12px;
                margin-left: -1px;
                line-height: 1.42857143;
                color: #337ab7;
                text-decoration: none;
                background-color: #fff;
                border: 1px solid #ddd;
            }

            .pager table > tbody > tr > td > span {
                z-index: 3;
                color: #fff;
                cursor: default;
                background-color: #337ab7;
                border-color: #337ab7;
            }

            .pager table > tbody > tr > td:first-child > a,
            .pager table > tbody > tr > td:first-child > span {
                margin-left: 0;
                border-top-left-radius: 4px;
                border-bottom-left-radius: 4px;
            }

            .pager table > tbody > tr > td:last-child > a,
            .pager table > tbody > tr > td:last-child > span {
                border-top-right-radius: 4px;
                border-bottom-right-radius: 4px;
            }

            .pager table > tbody > tr > td > a:hover,
            .pager table > tbody > tr > td > span:hover,
            .pager table > tbody > tr > td > a:focus,
            .pager table > tbody > tr > td > span:focus {
                z-index: 2;
                color: #23527c;
                background-color: #eee;
                border-color: #ddd;
            }

        label {
            padding: 10px;
        }
    </style>
    <link href="CSS/CSS.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.blockUI.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BlockUI(elementID) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(function () {
                $("#" + elementID).block({
                    message: '<table align = "center"><tr><td>' +
                    '<img src="images/loadingAnim.gif"/></td></tr></table>',
                    css: {},
                    overlayCSS: {
                        backgroundColor: '#000000', opacity: 0.6
                    }
                });
            });
            prm.add_endRequest(function () {
                $("#" + elementID).unblock();
            });
        }
        $(document).ready(function () {

          <%--  BlockUI("<%=pnlAddEdit.ClientID %>");--%>
          <%--  BlockUI("<%=pnlAddEditMedication.ClientID %>");--%>
           <%-- BlockUI("<%=BodyPartDDN.ClientID %>");--%>
            //$.blockUI.defaults.css = {};
        });
        //function Hidepopup() {
        //    //debugger
        //    location.reload();
        //    $find("#ctl00_cpMain_popup").hide();
        //    return false;
        //}

        function checksubprocedure(subid) {
            if (subid)
            { return true; }
            else {
                alert("Please enter medication first.");
                return false;
            }
        }
        function checkSubmuscle(musid) {
            if (musid)
            { return true; }
            else {
                alert("Please enter subprocedure first");
                return false;
            }
        }

        function GetDynamicTextBox(value) {
            return '<input name = "DynamicTextBox" type="text" value = "' + value + '" />&nbsp;' +
                '<input type="button" value="Remove" onclick = "RemoveTextBox(this)" />'
        }
        function AddTextBox() {
            var div = document.createElement('DIV');
            div.innerHTML = GetDynamicTextBox("");
            document.getElementById("TextBoxContainer").appendChild(div);
        }

        function RemoveTextBox(div) {
            document.getElementById("TextBoxContainer").removeChild(div.parentNode);
        }

        //function RecreateDynamicTextboxes(Textboxvalues) {
        //    var values = eval(Textboxvalues);
        //    if (values != null) {
        //        var html = "";
        //        for (var i = 0; i < values.length; i++) {
        //            html += "<div>" + GetDynamicTextBox(values[i]) + "</div>";
        //        }
        //        document.getElementById("TextBoxContainer").innerHTML = html;
        //    }
        //}


        function GetDynamicTextBoxMuscle(value) {
            return '<input name = "DynamicTextBoxMuscle" type="text" value = "' + value + '" />&nbsp;' +
                '<input type="button" value="Remove" onclick = "RemoveTextBoxMuscle(this)" />'
        }
        function AddTextBoxMuscle() {
            var div = document.createElement('DIV');
            div.innerHTML = GetDynamicTextBoxMuscle("");
            document.getElementById("TextBoxContainerMuscle").appendChild(div);
        }

        function RemoveTextBoxMuscle(div) {
            document.getElementById("TextBoxContainerMuscle").removeChild(div.parentNode);
        }
        //function RecreateDynamicTextboxesMuscle(textboxvaluesMuscles) {
        //    var values = eval(textboxvaluesMuscles);
        //    if (values != null) {
        //        var html = "";
        //        for (var i = 0; i < values.length; i++) {
        //            html += "<div>" + GetDynamicTextBoxMuscle(values[i]) + "</div>";
        //        }
        //        document.getElementById("TextBoxContainerMuscle").innerHTML = html;
        //    }
        //}

        function divexpandcollapsemedication(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                //img.src = "minus.gif";
            } else {
                div.style.display = "none";
                //img.src = "plus.gif";
            }
        }

        function divexpandcollapseMuscle(divname) {
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                //img.src = "minus.gif";
            } else {
                div.style.display = "none";
                //img.src = "plus.gif";
            }
        }

    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js">
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <asp:HiddenField ID="hdn_ID" runat="server" />
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Manage Procedures							
									<i class="ace-icon fa fa-angle-double-right"></i>
                    </small>
                    <small>
                        <asp:LinkButton ID="btnaddnew" runat="server" OnClick="Add">Add New</asp:LinkButton>
                    </small>
                </h1>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvProcedureTbl" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="ProcedureID" OnRowDataBound="OnRowDataBound" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager">
                        <Columns>
                            <asp:BoundField DataField="ProcedureID" HeaderText="Procedure" />
                            <asp:BoundField DataField="MCODE" HeaderText="MCode" />
                            <asp:BoundField DataField="Sub_CODE" HeaderText="Sub_CODE" />
                            <asp:BoundField DataField="Heading" HeaderText="Heading" />
                            <asp:BoundField DataField="INhouseProc" HeaderText="INhouseProc" />
                            <asp:BoundField DataField="HasPosition" HeaderText="HasPosition" />
                            <asp:BoundField DataField="HasLevel" HeaderText="HasLevel" />
                            <asp:BoundField DataField="HasMuscle" HeaderText="HasMuscle" />
                            <asp:BoundField DataField="HasMedication" HeaderText="HasMedication" />
                            <asp:BoundField DataField="HasSubCode" HeaderText="HasSubCode" />
                            <asp:BoundField DataField="BodyPart" HeaderText="Bodypart" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdn_MedicationID" Value='<%# Eval("MedicationIDs") %>' runat="server" />
                                    <asp:HiddenField ID="hdn_SubProcedureID" Value='<%# Eval("SubProcedureID") %>' runat="server" />
                                    <asp:HiddenField ID="hdn_MuscleIDs" Value='<%# Eval("MusclesIDS") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit Procedure" OnClick="Edit"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkmedicationAdd" runat="server" Visible='<%# string.IsNullOrEmpty(Convert.ToString(Eval("MedicationIDs"))) %>' Text="Add-Medication" OnClick="AddMedication"></asp:LinkButton>
                                    <a href="JavaScript:divexpandcollapsemedication('divMed<%# Eval("SubProcedureID") %>');">editmedication<%# Eval("MedicationIDs") %></a>
                                    <%--Visible='<%# string.IsNullOrEmpty(Convert.ToString(Eval("SubProcedureID"))) %>'--%>
                                    <asp:LinkButton ID="lnkSubprocedureAdd" runat="server" Text="Add-SubProcedure"  OnClick="AddSubprocedure" OnClientClick='<%# "return checksubprocedure(" + (!string.IsNullOrEmpty(Convert.ToString(Eval("MedicationIds"))) ? "true" : "false") + ");" %>'></asp:LinkButton>
                                    <asp:LinkButton ID="lnkSubprocedureEdit" runat="server" Text="Edit-SubProcedure" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("SubProcedureID"))) %>' OnClick="EditSubprocedure"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkmuscleAdd" runat="server" Text="Add-Muscle" OnClick="AddMuscle" Visible='<%# string.IsNullOrEmpty(Convert.ToString(Eval("MusclesIDS"))) %>' OnClientClick='<%# "return checkSubmuscle(" + (!string.IsNullOrEmpty(Convert.ToString(Eval("SubProcedureID"))) ? "true" : "false")  + ");" %>'></asp:LinkButton>
                                    <a href="JavaScript:divexpandcollapseMuscle('divMus<%# Eval("SubProcedureID") %>');">Edit Muscle<%# Eval("MusclesIDS") %></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="100%">
                                            <div id="divMed<%# Eval("SubProcedureID") %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                <asp:GridView ID="gvMedication" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="Medication" HeaderText="Medication" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdn_procedureID" Value='<%# Eval("ProcedureID") %>' runat="server" />
                                                                <asp:HiddenField ID="hdn_SubProcedureID" Value='<%# Eval("SubProcedureID") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEditMedication" CommandArgument='<%# Eval("MedicationID") %>' runat="server" Text='<%# "Edit medication" + Eval("MedicationID") %>' OnClick="EditMedication"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="100%">
                                            <div id="divMus<%# Eval("SubProcedureID") %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                <asp:GridView ID="gvMuscle" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField DataField="Muscle" HeaderText="Muscle" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdn_procedureID" Value='<%# Eval("ProcedureID") %>' runat="server" />
                                                                <asp:HiddenField ID="hdn_SubProcedureID" Value='<%# Eval("SubProcedureID") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEditMuscle" CommandArgument='<%# Eval("MuscleID") %>' runat="server" Text="Edit" OnClick="EditMuscle"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                       <%-- <AlternatingRowStyle BackColor="#C2D69B" />--%>
                    </asp:GridView>
                    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup" Style="display: none; height: auto;">
                        <br />
                        <center>
                        <span style="font-weight:bold"> Add/Edit Procedure</span>
                        </center>
                        <br />

                        <label class="control-label">Body Parts</label>
                        <asp:DropDownList ID="ddlBID" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="-1" runat="server" ControlToValidate="ddlBID" Display="Dynamic" ErrorMessage="Please select Body Part" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <br />
                        <label class="control-label">MCODE</label>
                        <asp:TextBox ID="txtMCODE" Style="width: 80%;" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMCODE" Display="Dynamic" ErrorMessage="Please enter Mcode" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>

                        <br />
                        <label class="control-label">Heading</label>
                        <asp:TextBox ID="txtHeading" Style="width: 80%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtHeading" Display="Dynamic" ErrorMessage="Please enter Heading" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <br />
                        <asp:CheckBox ID="chkINhouseProc" runat="server" Style="padding: 5px" Text="INhouseProc " />
                        <asp:CheckBox ID="chkHasPosition" runat="server" Text="Position" />
                        <asp:CheckBox ID="chkHasLevel" runat="server" Text="Level" />
                        <br />
                        <asp:CheckBox ID="chkHasMuscle" runat="server" Text="Muscle" />
                        <asp:CheckBox ID="chkHasMedication" runat="server" Text="Medication" />
                        <asp:CheckBox ID="chkHasSubCode" runat="server" Text="SubCode" />
                        <br />
                        <div class="center">
                            <asp:Button ID="btnSave" class="btn btn-info" ValidationGroup="save" runat="server" Text="Save" OnClick="Save" />
                            <asp:Button ID="btnCancel" class="btn btn-info" runat="server" CausesValidation="false" Text="Cancel" OnClick="Cancel" />
                        </div>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                        PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>

                    <asp:Panel ID="pnlAddEditMedication" runat="server" CssClass="modalPopup" Style="display: none; height: auto;">
                        <br />
                        <center>
                        <span style="font-weight:bold"> Add Medication</span>
                        </center>
                        <input id="btnAdd" type="button" value="add More Medication" class="btn btn-info" onclick="AddTextBox()" />
                        <br />
                        <br />
                        <div id="TextBoxContainer">
                            <!--Textboxes will be added here -->
                        </div>

                        <br />
                        <asp:Button ID="btnSaveMedication" runat="server" class="btn btn-info" Text="Save" OnClick="SaveMedication" />
                        <asp:Button ID="Button2" runat="server" class="btn btn-info" CausesValidation="false" Text="Cancel" OnClick="CancelMedication" />
                    </asp:Panel>
                    <asp:LinkButton ID="lnkFakeMed" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popupMedication" runat="server" DropShadow="false"
                        PopupControlID="pnlAddEditMedication" TargetControlID="lnkFakeMed"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>


                    <asp:Panel ID="pnlAddEditSubprocedure" runat="server" CssClass="modalPopup" Style="display: none; height: auto;">
                        <br />
                        <center>
                        <span style="font-weight:bold"> Add/Edit SubProcedure</span>
                        </center>

                        <label class="control-label">Sub_CODE</label>
                        <asp:TextBox ID="txtSub_CODE" Style="width: 80%;" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSub_CODE" Display="Dynamic" ErrorMessage="Please enter Sub code" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
                        <br />
                        <label class="control-label">Medication</label>
                        <asp:TextBox ID="txtMedication" Style="width: 80%;" runat="server" ReadOnly="true"></asp:TextBox>
                        <br />
                        <label class="control-label">Heading</label>
                        <asp:TextBox ID="txtHeadingmuscle" Style="width: 80%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtHeadingmuscle" Display="Dynamic" ErrorMessage="Please enter Heading" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <label class="control-label">CCDesc</label>
                        <asp:TextBox ID="txtCCDesc" Style="width: 80%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCCDesc" Display="Dynamic" ErrorMessage="Please enter ccdesc" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <label class="control-label">PEDesc</label>
                        <asp:TextBox ID="txtPEDesc" Style="width: 80%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPEDesc" Display="Dynamic" ErrorMessage="Please enter pedesc" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <label class="control-label">ADesc</label>
                        <asp:TextBox ID="txtADesc" Style="width: 80%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtADesc" Display="Dynamic" ErrorMessage="Please enter ADesc" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <label class="control-label">PDesc</label>
                        <asp:TextBox ID="txtPDesc" Style="width: 80%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtPDesc" Display="Dynamic" ErrorMessage="Please enter PDesc" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
                        <br />
                        <asp:CheckBox ID="chkCF" runat="server" Style="padding: 5px" Text="CF " />
                        <asp:CheckBox ID="chkPN" runat="server" Text="PN" />
                        <br />
                        <asp:Button ID="btnsubprocedure" runat="server" class="btn btn-info" Text="Save" ValidationGroup="savemuscle" OnClick="SaveSubprocedure" />
                        <asp:Button ID="Button3" runat="server" class="btn btn-info" CausesValidation="false" Text="Cancel" OnClick="CancelSubprocedure" />
                    </asp:Panel>
                    <asp:LinkButton ID="fkSubprocedure" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popupSubprocedure" runat="server" DropShadow="false"
                        PopupControlID="pnlAddEditSubprocedure" TargetControlID="fkSubprocedure"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>


                    <asp:Panel ID="pnlAddEditMuscles" runat="server" CssClass="modalPopup" Style="display: none; height: auto;">
                        <br />
                        <center>
                        <span style="font-weight:bold"> Add Muscles</span>
                        </center>
                        <input id="btnAddMuscle" type="button" value="add More Muscle" class="btn btn-info" onclick="AddTextBoxMuscle()" />
                        <br />
                        <br />
                        <div id="TextBoxContainerMuscle">
                            <!--Textboxes will be added here -->
                        </div>

                        <br />
                        <asp:Button ID="Button1" runat="server" class="btn btn-info" Text="Save" OnClick="SaveMuscles" />
                        <asp:Button ID="Button4" runat="server" class="btn btn-info" CausesValidation="false" Text="Cancel" OnClick="CancelMuscle" />
                    </asp:Panel>
                    <asp:LinkButton ID="fkMuscles" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popupMuscle" runat="server" DropShadow="false"
                        PopupControlID="pnlAddEditMuscles" TargetControlID="fkMuscles"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>


                    <asp:Panel ID="EditMuscleMedication" runat="server" CssClass="modalPopup" Style="display: none; height: auto;">
                        <br />
                        <center>
                        <span style="font-weight:bold"> Edit Details</span>
                        </center>

                        <div>
                            <asp:TextBox ID="txtEditdetails" runat="server"></asp:TextBox>
                        </div>

                        <br />
                        <asp:Button ID="btnsaveeditedmuscles" runat="server" class="btn btn-info" Text="Save" OnClick="btnsaveeditedmuscles_Click" />
                        <asp:Button ID="btnsaveeditedmedication" runat="server" class="btn btn-info" Text="Save" OnClick="btnsaveeditedmedication_Click" />
                        <asp:Button ID="btncancleEditdetails" runat="server" class="btn btn-info" CausesValidation="false" Text="Cancel" OnClick="btncancleEditdetails_Click" />
                    </asp:Panel>
                    <asp:LinkButton ID="fkEditMuscleMedication" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popupEditMuscleMedication" runat="server" DropShadow="false"
                        PopupControlID="EditMuscleMedication" TargetControlID="fkEditMuscleMedication"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnaddnew" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

