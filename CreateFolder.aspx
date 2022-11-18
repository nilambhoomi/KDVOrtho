<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="CreateFolder.aspx.cs" Inherits="CreateFolder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

    <div class="container">
        <div class="row">
            <h3>Create Folder</h3>
            <hr />
        </div>
        <div class="row">

            <div class="col-lg-4 inline">
                Please Select Type
                <asp:DropDownList runat="server" ID="ddl_type">
                    <asp:ListItem Value="PatientDocument" Text="Patient Documents"></asp:ListItem>
                    <asp:ListItem Value="GeneralDocuments" Text="General Documents"></asp:ListItem>
                </asp:DropDownList>

            </div>

            <div class="col-lg-4 inline">
                Please Enter Name of Folder
                <asp:TextBox ID="txtFolderName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="reqval" ControlToValidate="txtFolderName" ErrorMessage="Please Enter Folder Name" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-4 inline">
                <asp:Button ID="btnCreateFolder" Text="Create Folder" runat="server" OnClick="btnCreateFolder_Click" />
            </div>
        </div>
        <br />

        <div class="row">

            <asp:UpdatePanel runat="server" ID="upMain">
                <ContentTemplate>

                    <div class="col-lg-12 inline">
                        <div class="row">
                            <h3>Folder List</h3>
                            <hr />
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-4 inline">
                                Please Select Type
                    <asp:DropDownList runat="server" ID="ddl_search_type" OnSelectedIndexChanged="ddl_search_type_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="PatientDocument" Text="Patient Documents"></asp:ListItem>
                        <asp:ListItem Value="GeneralDocuments" Text="General Documents"></asp:ListItem>
                    </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <asp:GridView ID="gvDocument" BorderStyle="None" CssClass="table table-bordered" EmptyDataText="Sorry !! No Document found" AutoGenerateColumns="false" Width="100%" runat="server">
                                <Columns>

                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>

                                            <% if (Session["UserDesignation"].ToString().ToLower() == "administrator" || Session["UserDesignation"].ToString().ToLower() == "Supervisors")
                                                { %>
                                            <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this folder ?')" CommandArgument='<%# Eval("Name") %>' />
                                            &nbsp;
                                            <button type="button" onclick="rename('<%# Eval("Name") %>')">Rename </button>
                                            <%} %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button runat="server" ID="btndeleteall" Text="Delete All" CausesValidation="false" OnClick="btndeleteall_Click" Style="display: none" />
                    </div>
                    </div>
        <asp:HiddenField runat="server" ID="hidName" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Rename Folder</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row" style="margin: 10px">
                            <div class="col-5">
                                <asp:TextBox CssClass="form-control" runat="server" Enabled="false" ID="txt_old_name"></asp:TextBox>
                            </div>
                            <br />
                            <div class="col-5">
                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_new_name"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="req" ControlToValidate="txt_new_name" ErrorMessage="Folder Name Require." ValidationGroup="renameGRP"></asp:RequiredFieldValidator>
                            </div>
                            <br />
                            <div class="col-2">
                                <asp:Button ID="btnChange" runat="server" Text="Change" ValidationGroup="renameGRP" CssClass="btn btn-success btn-sm" OnClick="btnChange_Click" />

                            </div>
                            <asp:HiddenField runat="server" ID="hdFileName" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>



        <script>

            function deleteFun() {

                var val = confirm('This folder have files in it. Do you still want to continue ?')

                if (val) {
                    document.getElementById('<%= btndeleteall.ClientID %>').click();
                }
            }

            function rename(filename) {
                //  var filename = $(this).data('file-name');
                $("#<%=txt_old_name.ClientID%>").val(filename);
                $("#<%=hdFileName.ClientID%>").val(filename);
                $('#myModal').modal('show');
            };


        </script>
</asp:Content>

