<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ManageUser.aspx.cs" Inherits="ManageUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /*a.btn {
            text-decoration: none;
        }

        table {
            text-align: center;
        }

        .main-container {
            min-height: 900px;
        }*/

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Manage Users							
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>
            <div class="space-12"></div>
            <div class="row">

                <asp:UpdatePanel runat="server" ID="upMain">
                    <ContentTemplate>
                        <div class="row">
                            <div class="alert alert-success" runat="server" id="divSuccess" style="display: none">
                                Record Deleted Successfully.
                            </div>
                            <div class="alert alert-danger" runat="server" id="divfail" style="display: none">
                                Sorry !! this record is associated with other data.
                            </div>
                        </div>

                        <div class="col-xs-12">

                            <div class="row">
                                <div class="col-sm-3" style="padding-left: 0px">
                                    <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-success" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-success" Text="Refresh" OnClick="btnRefresh_Click" />
                                    <asp:Button runat="server" ID="btnAdd" Text="Add New" CssClass="btn btn-primary" PostBackUrl="~/EditUser.aspx" />

                                </div>
                                <div class="col-sm-6" style="float: right">
                                    <asp:DropDownList runat="server" ID="ddlPage" AutoPostBack="true" Style="float: right; width: 70px" CssClass="form-control" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged">
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="50" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="100" Value="40"></asp:ListItem>
                                        <%--     <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                                <div class="clearfix"></div>
                                <br />

                                <div class="table-responsive">
                                    <asp:GridView ID="gvPatientDetails" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="User_ID" PagerStyle-CssClass="pager">
                                        <Columns>

                                            <asp:BoundField DataField="LoginID" HeaderText="Login ID" />
                                            <asp:BoundField DataField="lastname" HeaderText="Last Name" />
                                            <asp:BoundField DataField="firstname" HeaderText="First Name" />
                                            <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" />
                                            <asp:BoundField DataField="desig" HeaderText="Role" />
                                            <asp:BoundField DataField="eMailID" HeaderText="eMailID" />
                                            <asp:BoundField DataField="locations" HeaderText="Locations" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" NavigateUrl='<%# "~/EditUser.aspx?id="+Eval("User_ID") %>' Text="Edit"></asp:HyperLink>
                                                    |
                                                     <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record ?')" CausesValidation="false" OnClick="lnkDelete_Click" CommandArgument='<%# Eval("User_ID") %>'></asp:LinkButton>

                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>

                                            <%-- <asp:BoundField ItemStyle-Width="150px" DataField="ContactName" HeaderText="Contact Name" />
            <asp:BoundField ItemStyle-Width="150px" DataField="City" HeaderText="City" />--%>
                                        </Columns>
                                        <PagerSettings PageButtonCount="5" />

                                        <PagerStyle CssClass="pager"></PagerStyle>
                                    </asp:GridView>

                                    <div runat="server" id="div_page">
                                        Page
            <label runat="server" id="lbl_page_no" style="display: inline"></label>
                                        of
            <label runat="server" id="lbl_total_page" style="display: inline"></label>
                                    </div>
                                    <div>
                                        <ul class="pagination">
                                            <asp:Repeater ID="rptPager" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                            CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "active" : "" %>'
                                                            OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- #section:pages/profile.info -->
            <%--<div class="profile-user-info profile-user-info-striped">
												<div class="profile-info-row">
													<div class="profile-info-name"> Username </div>

													<div class="profile-info-value">
														<span class="editable" id="username">alexdoe</span>
													</div>
												</div>

												<div class="profile-info-row">
													<div class="profile-info-name"> Location </div>

													<div class="profile-info-value">
														<i class="fa fa-map-marker light-orange bigger-110"></i>
														<span class="editable" id="country">Netherlands</span>
														<span class="editable" id="city">Amsterdam</span>
													</div>
												</div>

												<div class="profile-info-row">
													<div class="profile-info-name"> Age </div>

													<div class="profile-info-value">
														<span class="editable" id="age">38</span>
													</div>
												</div>

												<div class="profile-info-row">
													<div class="profile-info-name"> Joined </div>

													<div class="profile-info-value">
														<span class="editable" id="signup">2010/06/20</span>
													</div>
												</div>

												<div class="profile-info-row">
													<div class="profile-info-name"> Last Online </div>

													<div class="profile-info-value">
														<span class="editable" id="login">3 hours ago</span>
													</div>
												</div>

												<div class="profile-info-row">
													<div class="profile-info-name"> About Me </div>

													<div class="profile-info-value">
														<span class="editable" id="about">Editable as WYSIWYG</span>
													</div>
												</div>
											</div>--%>

            <!-- /section:pages/profile.info -->
            <div class="space-20"></div>
        </div>
    </div>
</asp:Content>

