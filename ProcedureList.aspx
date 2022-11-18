<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="ProcedureList.aspx.cs" Inherits="Procedures" %>

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
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"> </script>
    <link href="CSS/CSS.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.blockUI.min.js" type="text/javascript"></script>
   

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
                        <asp:LinkButton ID="btnaddnew" runat="server" PostBackUrl="~/AddProcedure.aspx">Add New</asp:LinkButton>
                    </small>
                </h1>
            </div>
            <asp:Panel ID="p" runat="server" DefaultButton="btnSearch">
                <div class="container">
                    <div class="row">
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">BodyPart</span>
                                <asp:TextBox ID="txtSearchBodyPart" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">Mcode</span>
                                <asp:TextBox ID="txtSearchMcode" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">Heading</span>
                                <asp:TextBox ID="txtSearchHeading" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <div class="input-group">
                                <asp:LinkButton ID="btnSearch" CssClass="btn success" runat="server" OnClick="btnSearch_Click" Text="Search"></asp:LinkButton>
                                <asp:LinkButton ID="btnReset" CssClass="btn btn-danger " Style="margin-left: 2px" runat="server" OnClick="btnReset_Click" Text="Reset"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </asp:Panel>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvProcedureTbl" AllowSorting="true" OnSorting="gridView_Sorting" runat="server" AutoGenerateColumns="false" PageSize="25" CssClass="table table-striped table-bordered table-hover" DataKeyNames="Procedure_ID" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager">
                        <Columns>
                            <asp:BoundField DataField="Procedure_ID" HeaderText="Procedure" />
                            <asp:TemplateField HeaderText="MCODE" SortExpression="MCODE">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("MCODE") %>'></asp:Label>
                                    <%--<asp:BoundField DataField="MCODE" HeaderText="MCODE" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Heading" SortExpression="Heading">
                                <ItemTemplate>
                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>
                                    <%--  <asp:BoundField DataField="Heading" HeaderText="Heading" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Display_Order" HeaderText="Display Order" />
                            <asp:BoundField DataField="Position" HeaderText="Position" />
                            <asp:BoundField DataField="INhouseProcbit" HeaderText="IN-House" />
                            <asp:BoundField DataField="HasLevel" HeaderText="HasLevel" />
                            <%--<asp:BoundField DataField="INout" HeaderText="INout" />--%>
                            <asp:BoundField DataField="BodyPart" HeaderText="Bodypart" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                     <asp:LinkButton ID="lnkclone" runat="server" Text="Clone" OnClick="lnkclone_Click" CommandArgument='<%# Eval("Procedure_ID") %>'></asp:LinkButton>
                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="Edit" CommandArgument='<%# Eval("Procedure_ID") %>'></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" Style="color: red" OnClientClick="return confirm('Are you sure you want delete');" Text="Remove" OnClick="Del"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#C2D69B" />
                    </asp:GridView>
                    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup" Style="display: none; width: auto; overflow: auto;">
                        <div class="container">
                            <div class="form-group row">
                                <center><h2>Add/Edit Procedure</h2></center>
                                <div class="form-inline col-lg-4">
                                    <label for="ddlposition">Position</label>
                                    <asp:DropDownList class="form-control" Style="width: 50%;" ID="ddlposition" runat="server">
                                        <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                        <asp:ListItem Text="Bilateral" Value="Bilateral"></asp:ListItem>
                                    </asp:DropDownList>

                                </div>
                                <div class="form-inline col-lg-4">
                                    <label for="txtBodyParts">Body Parts</label>
                                    <asp:TextBox ID="txtBodyParts" class="form-control" runat="server"></asp:TextBox>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBodyParts" Display="Dynamic" ErrorMessage="Please enter BodyPart" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="form-inline col-lg-4">
                                    <label for="txtMCODE">MCODE</label>
                                    <asp:TextBox ID="txtMCODE" class="form-control" runat="server"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMCODE" Display="Dynamic" ErrorMessage="Please enter Mcode" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-4">
                                    <label class="control-label">Display Order</label>
                                    <asp:TextBox ID="txtDisplay_Order" runat="server"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDisplay_Order" Display="Dynamic" ErrorMessage="Please enter Display Order" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtDisplay_Order" ErrorMessage="Please enter Number only" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>--%>
                                </div>
                                <div class="form-inline col-lg-8">
                                    <label for="txtHeading">R - Heading</label>
                                    <asp:TextBox ID="txtHeading" Width="80%" class="form-control" runat="server"></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtMuscles">Muscle</label>
                                    <asp:TextBox ID="txtMuscles" class="form-control" Style="width: 80%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label class="control-label">Medication</label>
                                    <asp:TextBox ID="txtMedication" Style="width: 80%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label class="control-label">SubProcedure</label>
                                    <asp:TextBox ID="txtSubProcedure" Style="width: 80%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="form-inline row col-lg-12">
                                    <label for="txtCCDesc">R_CCDesc</label>
                                    <asp:TextBox ID="txtCCDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtPEDesc">R_PEDesc</label>
                                    <asp:TextBox ID="txtPEDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">

                                    <label for="txtADesc">R_ADesc</label>
                                    <asp:TextBox ID="txtADesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtPDesc">R_PDesc</label>
                                    <asp:TextBox ID="txtPDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtHeadingS">S - Heading</label>
                                    <asp:TextBox ID="txtHeadingS" Width="80%" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline row col-lg-12">
                                    <label for="txtS_CCDesc">S_CCDesc</label>
                                    <asp:TextBox ID="txtS_CCDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtS_PEDesc">S_PEDesc</label>
                                    <asp:TextBox ID="txtS_PEDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">

                                    <label for="txtS_ADesc">S_ADesc</label>
                                    <asp:TextBox ID="txtS_ADesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtS_PDesc">S_PDesc</label>
                                    <asp:TextBox ID="txtS_PDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtHeadingE">E - Heading</label>
                                    <asp:TextBox ID="txtHeadingE" Width="80%" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline row col-lg-12">
                                    <label for="txtE_CCDesc">E_CCDesc</label>
                                    <asp:TextBox ID="txtE_CCDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtE_PEDesc">E_PEDesc</label>
                                    <asp:TextBox ID="txtE_PEDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">

                                    <label for="txtE_ADesc">E_ADesc</label>
                                    <asp:TextBox ID="txtE_ADesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <label for="txtE_PDesc">E_PDesc</label>
                                    <asp:TextBox ID="txtE_PDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="form-inline col-lg-12">
                                    <asp:CheckBox ID="chkINhouseProcbit" runat="server" Style="padding: 5px" Text="IN-House" />
                                    <asp:CheckBox ID="chkSides" runat="server" Text="Sides" />
                                    <asp:CheckBox ID="chkHasLevel" runat="server" Text="Level" />
                                    <asp:CheckBox ID="chkCF" runat="server" Text="CF" />
                                    <asp:CheckBox ID="chkPN" runat="server" Text="PN" />
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="form-inline col-lg-6">
                                      <label for="txtLevelsDefault">Levels Default Value.</label>
                                    <asp:TextBox ID="txtLevelsDefault" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-inline col-lg-6">
                                      <label for="ddlSidesDefault">Default Value for sides</label>
                                    <asp:DropDownList ID="ddlSidesDefault" runat="server">
                                        <asp:ListItem Text="--Select--" Value=" "></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                        <asp:ListItem Text="Bilateral" Value="Bilateral"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="form-inline col-lg-12 center">
                                    <asp:Button ID="btnSave" class="btn btn-info" ValidationGroup="save" runat="server" Text="Save" OnClick="Save" />
                                    <asp:Button ID="btnCancel" class="btn btn-info" runat="server" CausesValidation="false" Text="Cancel" OnClick="Cancel" />
                                </div>
                            </div>
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

