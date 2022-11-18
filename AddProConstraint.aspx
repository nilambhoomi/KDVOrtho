<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="AddProConstraint.aspx.cs" Inherits="AddProConstraint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Pro Constraint Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>

            <asp:UpdatePanel runat="server" ID="upMain">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hdid" />    

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <label class="lblstyle">Select Body Part</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:DropDownList runat="server" ID="ddlbodypart" OnSelectedIndexChanged="ddlbodypart_SelectedIndexChanged" AutoPostBack="true" DataTextField="BodyPart" DataValueField="BodyPart"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlbodypart" InitialValue="0" Display="Dynamic" ErrorMessage="Please select bodypart." SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="lblstyle">Select Position</label>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:DropDownList runat="server" ID="ddlposition" OnSelectedIndexChanged="ddlposition_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>


                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <label class="lblstyle">Select Proc Req/Sched MCode</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" ID="ddlProcSchedule" DataTextField="MCODE" DataValueField="MCODE"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlProcSchedule" InitialValue="0" Display="Dynamic" ErrorMessage="Please select Proc Req/Sched MCode." SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </div>

                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <label class="lblstyle">Select Proced Executed MCode</label>
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:CheckBoxList runat="server" ID="chkProcExe" RepeatDirection="Horizontal" RepeatColumns="8" DataTextField="MCODE" DataValueField="MCODE"></asp:CheckBoxList>
                                        </div>

                                    </div>


                                </div>
                                <div class="clearfix"></div>
                                <br />

                                <div class="col-xs-12">
                                    <div class="row">

                                        <div class="col-sm-2">
                                            <label class="lblstyle">&nbsp;</label>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewProcConstraint.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                            </div>
                                        </div>

                                    </div>


                                </div>
                            </div>

                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

