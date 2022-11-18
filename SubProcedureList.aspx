<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" ValidateRequest="false" CodeFile="SubProcedureList.aspx.cs" Inherits="Procedures" %>

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
                    <small>Manage Sub Procedures							
									<i class="ace-icon fa fa-angle-double-right"></i>
                    </small>
                    <small>
                        <asp:LinkButton ID="btnaddnew" runat="server" OnClick="Add">Add New</asp:LinkButton>
                    </small>
                </h1>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvSubProcedureTbl" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="ProcedureID" AllowPaging="True" PageSize="25" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager">
                        <Columns>
                            <asp:BoundField DataField="SubProcedureID" HeaderText="SubProcedure" />
                            <asp:BoundField DataField="ProcedureMCode" HeaderText="Pro MCode" />
                            <asp:BoundField DataField="ProcedureHeading" HeaderText="Pro Heading" />
                            <asp:BoundField DataField="ProcedureINhouseProc" HeaderText="Pro INhouseProc" />
                            <asp:BoundField DataField="ProcedureHasPosition" HeaderText="Pro HasPosition" />
                            <asp:BoundField DataField="ProcedureHasLevel" HeaderText="Pro HasLevel" />
                            <asp:BoundField DataField="ProcedureHasMuscle" HeaderText="Pro HasMuscle" />
                            <asp:BoundField DataField="ProcedureHasMedication" HeaderText="Pro HasMedication" />
                            <asp:BoundField DataField="ProcedureHasSubCode" HeaderText="Pro HasSubCode" />
                            <asp:BoundField DataField="ProcedureBodypart" HeaderText="Pro Bodypart" />
                            <asp:BoundField DataField="Sub_CODE" HeaderText="Sub_CODE" />
                            <asp:BoundField DataField="Heading" HeaderText="Heading" />
                            <asp:BoundField DataField="CCDesc" HeaderText="CCDesc" />
                            <asp:BoundField DataField="PEDesc" HeaderText="PEDesc" />
                            <asp:BoundField DataField="ADesc" HeaderText="ADesc" />
                            <asp:BoundField DataField="PDesc" HeaderText="PDesc" />
                            <asp:BoundField DataField="CF" HeaderText="CF" />
                            <asp:BoundField DataField="PN" HeaderText="PN" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" Style="color:red" OnClientClick="return confirm('Are you sure you want delete');" Text="Remove" OnClick="Del"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#C2D69B" />
                    </asp:GridView>
                    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup" Style="display: none; height: auto;">
                        <br />
                        <center>
                        <span style="font-weight:bold"> Add/Edit Sub Procedure</span>
                        </center>
                        <br />
                        <label class="control-label">Procedure</label>
                        <asp:DropDownList ID="ddlProcedureID" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="-1" runat="server" ControlToValidate="ddlProcedureID" Display="Dynamic" ErrorMessage="Please select Procedure" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <br />
                        <label class="control-label">Sub_CODE</label>
                        <asp:TextBox ID="txtSub_CODE" Style="width: 80%;" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSub_CODE" Display="Dynamic" ErrorMessage="Please enter Sub code" SetFocusOnError="True" ValidationGroup="savemuscle"></asp:RequiredFieldValidator>
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
                        <asp:CheckBox ID="chkCF" runat="server" Style="padding: 5px" Text="CF" />
                        <asp:CheckBox ID="chkPN" runat="server" Text="PN" />
                        <br />
                        <div class="center">
                            <asp:Button ID="btnSave" class="btn btn-info" ValidationGroup="save"  usesubmitbehavior="true" runat="server" Text="Save" OnClick="Save" />
                            <asp:Button ID="btnCancel" class="btn btn-info" runat="server"  usesubmitbehavior="true" CausesValidation="false" Text="Cancel" OnClick="Cancel" />
                        </div>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                        PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
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

