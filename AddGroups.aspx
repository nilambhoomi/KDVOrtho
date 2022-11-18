<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="AddGroups.aspx.cs" Inherits="AddGroups" %>

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
                    <small>Group Details								
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
                                        <label class="lblstyle">Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtName" CssClass="form-control" placeholder="Name" runat="server"></asp:TextBox>
                                    </div>
                                   

                                </div>


                            </div>
                              <div class="clearfix"></div>
                            <br />
                             <div class="col-xs-12">
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

                            <div class="col-xs-12">
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
                                               <asp:ListItem Text="Groups" Value="11"></asp:ListItem>
                                               <asp:ListItem Text="Create Folder" Value="12"></asp:ListItem>
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

                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">&nbsp;</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                            &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewGroups.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
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
</asp:Content>


