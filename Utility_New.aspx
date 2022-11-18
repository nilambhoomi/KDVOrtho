<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Utility_New.aspx.cs" Inherits="Utility_New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="Server">

    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Utility								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>


            <div class="span12">

                <div class="row">

                    <div class="row">



                        <div class="col-lg-12 inline">
                            Folder List
              <br />
                            <asp:GridView ID="gvDocument" BorderStyle="None" CssClass="table table-bordered" EmptyDataText="Sorry !! No Document found" AutoGenerateColumns="false" Width="100%" runat="server">
                                <Columns>

                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <input type="button" id="btnUpload" onclick="opensignupload('<%# Eval("Name") %>')" title="Upload" value="Upload" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                             <div runat="server" id="lblResult"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalSignupload" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; height: 570px!important">
        <div class="modal-dialog" style="background: white;">
            <div class="modal-content" style="height: 200px;">
                <div class="modal-header" style="display: inline-block; width: 100%;">
                    Upload Documents
                                       
                 <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                </div>
                <div class="modal-body">
                    <asp:FileUpload ID="fupuploadsign" AllowMultiple="true" CssClass="upload-icon" runat="server" />
                    <br />
                    <asp:Button ID="btnuploadimage" CssClass="btn btn-danger" runat="server" OnClick="btnuploadimage_Click" Text="upload" />
                </div>
                <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>


            </div>
        </div>
        x
    </div>



    <asp:HiddenField runat="server" ID="hd_name" />

    <script>

        function opensignupload(name) {
            alert(name);
            $('#ModalSignupload').modal('show');
            $("#<%= hd_name.ClientID%>").val(name);

        }

        function closeSignuploadModalPopup() {
            $('#ModalSignupload').modal('hide');
        }

    </script>
</asp:Content>

