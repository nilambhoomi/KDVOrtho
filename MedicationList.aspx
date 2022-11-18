<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="MedicationList.aspx.cs" Inherits="Procedures" %>

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
                    <small>Manage Medication							
									<i class="ace-icon fa fa-angle-double-right"></i>
                    </small>
                    <small>
                        <asp:LinkButton ID="btnaddnew" runat="server" OnClick="Add">Add New</asp:LinkButton>
                    </small>
                </h1>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvMedicationTbl" runat="server" AutoGenerateColumns="false" PageSize="25" CssClass="table table-striped table-bordered table-hover" DataKeyNames="MedicationID" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager">
                        <Columns>
                            <asp:BoundField DataField="MedicationID" HeaderText="Medication" />
                            <asp:BoundField DataField="Procedure" HeaderText="Procedure" />
                            <asp:BoundField DataField="SubProcedure" HeaderText="SubProcedure" />
                            <asp:BoundField DataField="medication" HeaderText="medication" />
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
                        <span style="font-weight:bold"> Add/Edit Medication</span>
                        </center>
                        <br />

                        <label class="control-label">Procedure</label>
                        <asp:DropDownList ID="ddlprocedureID" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="-1" runat="server" ControlToValidate="ddlprocedureID" Display="Dynamic" ErrorMessage="Please select procedure" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <br />
                        <label class="control-label">Sub Procedure</label>
                        <asp:DropDownList ID="ddlSubProcedure" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue="-1" runat="server" ControlToValidate="ddlSubProcedure" Display="Dynamic" ErrorMessage="Please select Sub procedure" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <br />
                        <label class="control-label">Medication</label>
                        <asp:TextBox ID="txtMedication" Style="width: 80%;" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMedication" Display="Dynamic" ErrorMessage="Please enter Medication" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>

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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnaddnew" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

