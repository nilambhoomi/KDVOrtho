<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="EditUser.aspx.cs" Inherits="EditUser" %>


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
                    <small>Users Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>

            <div class="">

                <div class="row">
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">First Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtFirstName" CssClass="form-control" placeholder="First Name" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="Please enter patient first name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Middle Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtMiddleName" CssClass="form-control" placeholder="Middle Name" runat="server"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Last Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLastName" CssClass="form-control" placeholder="Last Name" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Email</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator runat="server" ID="regEmail" ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
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
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAddress" CssClass="form-control" placeholder="Address" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Phone No</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPhoneNo" CssClass="form-control" placeholder="Phone No" runat="server"></asp:TextBox>
                                      
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />

                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">LoginID</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLoginID" CssClass="form-control" placeholder="LoginId" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Password</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPassowrd" TextMode="Password" CssClass="form-control" placeholder="Password" runat="server"></asp:TextBox>
                                        <%-- <asp:RegularExpressionValidator runat="server" ID="regPassword" ValidationExpression="(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{:;'?/>.<,])(?!.*\s).*$" ControlToValidate="txtPassowrd" Display="Dynamic"
                                            ErrorMessage="Password shoud be at least 8 chars with caps, low, nos & spl char."></asp:RegularExpressionValidator>--%>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />

                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Designation</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <%--  <asp:TextBox ID="txtDesignation" CssClass="form-control" placeholder="Designation" runat="server"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlDesig" DataTextField="designation" DataValueField="id" runat="server"></asp:DropDownList>
                                    </div>

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Groups</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <%--  <asp:TextBox ID="txtDesignation" CssClass="form-control" placeholder="Designation" runat="server"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlGroup" DataTextField="Name" DataValueField="Id" runat="server"></asp:DropDownList>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div style="display:none">
                            <div class="col-xs-12" >
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Locations</label>
                                    </div>
                                    <div class="col-sm-8">
                                        <input type="checkbox" id="checkAll" />
                                        Select All
    <br />
                                        <asp:CheckBoxList runat="server" ID="chkLocations" RepeatDirection="Horizontal" CssClass="chkLoc" RepeatColumns="4"></asp:CheckBoxList>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />

                            <div class="col-xs-12" >
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Page Access</label>
                                    </div>
                                    <div class="col-sm-8">
                                        <input type="checkbox" id="checkAllPage" />
                                        Select All
    <br />
                                        <asp:CheckBoxList runat="server" ID="chkPages" RepeatDirection="Horizontal" CssClass="chkPage" RepeatColumns="4">
                                            <asp:ListItem Text="Users" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Attorneys" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Diag Codes" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Ins. Co" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Medication" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Locations" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Pharmacies" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Providers" Value="8"></asp:ListItem>

                                            <asp:ListItem Text="Procedure" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="Designation" Value="10"></asp:ListItem>

                                        </asp:CheckBoxList>
                                    </div>

                                </div>


                            </div>


                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Reports</label>
                                    </div>
                                    <div class="col-sm-8">
                                        <input type="checkbox" id="checkAllReports" />
                                        Select All
    <br />
                                        <asp:CheckBoxList runat="server" ID="chkReports" RepeatDirection="Horizontal" CssClass="chkReport" RepeatColumns="4"></asp:CheckBoxList>
                                    </div>

                                </div>


                            </div>

                            <div class="clearfix"></div>
                            <br />
                             <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Role</label>
                                    </div>
                                    <div class="col-sm-8">
                                        <input type="checkbox" id="checkAllRole" />
                                        Select All
    <br />
                                        <asp:CheckBoxList runat="server" ID="chkRole" RepeatDirection="Horizontal" CssClass="chkRole" RepeatColumns="4"></asp:CheckBoxList>
                                    </div>

                                </div>


                            </div>

                            <div class="clearfix"></div>
                            <br />
                                </div>
                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">&nbsp;</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                            &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/ManageUser.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                        </div>
                                    </div>

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script src="Scripts/jquery-1.8.2.js"></script>
    <script src="Scripts/jquery-ui-1.8.24.js"></script>
    <link href="Style/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {
            $('#checkAll').click(function () {
                $('.chkLoc input:checkbox').prop('checked', this.checked);
            });

            $('#checkAllPage').click(function () {
                $('.chkPage input:checkbox').prop('checked', this.checked);
            });

            $('#checkAllReports').click(function () {
                $('.chkReport input:checkbox').prop('checked', this.checked);
            });

            $('#checkAllRole').click(function () {
                $('.chkRole input:checkbox').prop('checked', this.checked);
            });
        })

    </script>
    <%-- <script type="text/javascript">
        $(document).ready(function () {

            $("#<%=txtDesignation.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'EditUser.aspx/GetDesignations',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    val: item
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                               <%-- $("#<%=hfPatientId.ClientID %>").val(i.item.val);
                                $('#<%= txtSearch.ClientID %>').val(i.item.label);
                                $('#<%= btnSearch.ClientID %>').click();
                },
                minLength: 1
            });

        });
    </script>--%>
</asp:Content>
