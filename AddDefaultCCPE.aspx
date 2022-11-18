<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="AddDefaultCCPE.aspx.cs" Inherits="AddDefaultCCPE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Designation Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>
            <div class="clearfix"></div>
            <br />
            <div class="row">
                <div class="col-xs-12">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Body Part</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList class="form-control" Style="width: 50%;" ID="ddlbodyPart" runat="server">
                                        <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Neck" Value="Neck"></asp:ListItem>
                                        <asp:ListItem Text="Midback" Value="Midback"></asp:ListItem>
                                        <asp:ListItem Text="Lowback" Value="Lowback"></asp:ListItem>
                                        <asp:ListItem Text="Shoulder" Value="Shoulder"></asp:ListItem>
                                        <asp:ListItem Text="Elbow" Value="Elbow"></asp:ListItem>
                                        <asp:ListItem Text="Knee" Value="Knee"></asp:ListItem>
                                        <asp:ListItem Text="Hip" Value="Hip"></asp:ListItem>
                                        <asp:ListItem Text="Wrist" Value="Wrist"></asp:ListItem>
                                        <asp:ListItem Text="Ankle" Value="Ankle"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">Position</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList class="form-control" Style="width: 50%;" ID="ddlposition" runat="server">
                                        <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                        <asp:ListItem Text="Bilateral" Value="Bilateral"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">CC</label>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCC" Rows="5" Columns="116" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">PE</label>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtPE" Rows="5" Columns="116" TextMode="MultiLine"></asp:TextBox>
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
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewDefaultCCPE.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

