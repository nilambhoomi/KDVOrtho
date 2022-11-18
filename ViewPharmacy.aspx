<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ViewPharmacy.aspx.cs" Inherits="ViewPharmacy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Manage Pharmacy						
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>
            <div class="space-12"></div>

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
                                <asp:Button runat="server" ID="btnAdd" Text="Add New" CssClass="btn btn-primary" PostBackUrl="~/AddPharmacy.aspx" />
                                <asp:HiddenField ID="hfPatientId" runat="server"></asp:HiddenField>
                            </div>

                            <div class="col-sm-6" style="float: right">
                                <asp:DropDownList runat="server" ID="ddlPage" AutoPostBack="true" Style="float: right; width: 70px" CssClass="form-control" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged">
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="50" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="100" Value="40"></asp:ListItem>
                             <%--       <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="clearfix"></div>
                    <br />
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvPharmacyDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="pager">
                                    <Columns>

                                        <asp:BoundField DataField="Pharmacy" HeaderText="Pharmacy" />
                                        <asp:BoundField DataField="City" HeaderText="City" />
                                        <asp:BoundField DataField="State" HeaderText="State" />
                                        <asp:BoundField DataField="Zip" HeaderText="Zip" />

                                        <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                                        <asp:BoundField DataField="Telephone" HeaderText="Telephone" />
                                        <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" Target="_blank" NavigateUrl='<%# "~/AddPharmacy.aspx?id="+Eval("Pharmacy_ID") %>' Text="Edit"></asp:HyperLink>
                                                <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record ?')" CausesValidation="false" OnClick="lnkDelete_Click" CommandArgument='<%# Eval("Pharmacy_ID") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

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

            <div class="space-20"></div>
        </div>
    </div>
</asp:Content>

