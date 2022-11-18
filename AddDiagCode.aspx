<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="AddDiagCode.aspx.cs" Inherits="AddDiagCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .lblstyle {
            float: right;
        }
    </style>

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">


    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>DiagCode Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Body Part</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtBodyPart" CssClass="form-control" placeholder="Location" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBodyPart" Display="Dynamic" ErrorMessage="Please enter locations." SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">Diag Code</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDiagCode" CssClass="form-control" placeholder="DiagCode" runat="server"></asp:TextBox>
                                </div>


                            </div>


                        </div>

                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Description</label>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtDescription" CssClass="form-control" placeholder="Description" TextMode="MultiLine" Rows="2" runat="server"></asp:TextBox>
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
                                    <asp:CheckBox runat="server" ID="chkPreSelect" Text=" Pre Select" />
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
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewDiagCodes.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
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

