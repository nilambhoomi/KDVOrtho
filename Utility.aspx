<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Utility.aspx.cs" MasterPageFile="~/site.master" Inherits="Utility" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

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
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div>
                                        <fieldset style="display:none">
                                            <legend>Upload Document</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>Select Folder</td>
                                                    <td>
                                                        <asp:FileUpload runat="server" ID="fup" AllowMultiple="true" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset>
                                            <legend>Upload Sign</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>Select Folder</td>
                                                    <td>
                                                        <asp:FileUpload runat="server" ID="fupsign" AllowMultiple="true" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button runat="server" ID="btnSignUpload" Text="Upload" OnClick="btnSignUpload_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset>
                                            <legend>Upload Patient Data</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>Select Folder</td>
                                                    <td>
                                                        <asp:FileUpload runat="server" ID="fup_xls" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button runat="server" ID="btnxls" Text="Import" OnClick="btnxls_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset style="display:none">
                                            <legend>Upload Document(Multiple)</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>Select Folder</td>
                                                    <td>
                                                        <asp:FileUpload runat="server" ID="fupmul" AllowMultiple="true" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button runat="server" ID="btnupload_mul" Text="Upload" OnClick="btnupload_mul_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <div runat="server" id="lblResult"></div>
                                        <%--  <asp:Literal runat="server" ID="lblResult"></asp:Literal>--%>
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
