<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddLocations.aspx.cs" MasterPageFile="~/site.master" Inherits="AddLocations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .lblstyle {
            float: right;
        }
    </style>
    <script src="Scripts/jquery-1.8.2.min.js"></script>

    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

    <script>
        $(document).ready(function ($) {
            $('#<%=txtPhone.ClientID%>').mask("999-999-9999");
        })
        $(document).ready(function ($) {
            $('#<%=txtFax.ClientID%>').mask("999-999-9999");
        })
    </script>

    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Location Details								
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
                                    <label class="lblstyle">Name Of Practice</label>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtNameOfPractice" CssClass="form-control" placeholder="Name Of Practice" runat="server"></asp:TextBox>
                                    <%--    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNameOfPractice" Display="Dynamic" ErrorMessage="Please enter name of practice." SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                </div>

                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Location</label>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtLocation" CssClass="form-control" placeholder="Location" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLocation" Display="Dynamic" ErrorMessage="Please enter locations." SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>

                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Address</label>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtAddress" CssClass="form-control" placeholder="Address" runat="server"></asp:TextBox>
                                </div>

                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">City</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCity" CssClass="form-control" placeholder="City" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">State</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtState" CssClass="form-control" placeholder="State" runat="server"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Zip</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtZip" CssClass="form-control" placeholder="Zip" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">Email</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email" runat="server"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Phone</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPhone" CssClass="form-control" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">Contact Person</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtContacts" CssClass="form-control" placeholder="Contact Person" runat="server"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Fax</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtFax" CssClass="form-control" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">&nbsp;</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:CheckBox runat="server" ID="chkSetDefault" Text=" Set as Default" />
                                </div>

                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Doctor FirstName</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDFName" CssClass="form-control" placeholder="Doctor First Name" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">Doctor LastName</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDLName" CssClass="form-control" placeholder="Doctor Last Name" runat="server"></asp:TextBox>
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
                                    <asp:CheckBox runat="server" ID="chkActive" Text=" Is Active ?" />
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
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewLocations.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
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
