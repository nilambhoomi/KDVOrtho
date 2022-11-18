<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="batchprint.aspx.cs" Inherits="batchprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <br />
    <br />
    <br />
    <br />
    <div class="main-content-inner">
        <div class="page-content">
            <div class="margin-20">

                <div class="padding-20">
                    &nbsp;
            <asp:TextBox runat="server" placeholder="Print Date" ID="txt_date"></asp:TextBox>
                    &nbsp;
            <asp:TextBox runat="server" placeholder="Surgery Date" ID="txt_surgery"></asp:TextBox>
                    &nbsp;
            <asp:TextBox runat="server" placeholder="Doctor's Name" ID="txt_docName"></asp:TextBox>
                    &nbsp;
            <asp:TextBox runat="server" placeholder="Mcode" ID="txt_MCode_Proc"></asp:TextBox>
                </div>
                <div class="clearfix"></div>
                <br />
                <br />
                <div class="col-lg-6">
                    <asp:Label ID="Label1" CssClass="label-danger" runat="server" Text="Has Header ?"></asp:Label>
                    <asp:RadioButtonList ID="rbHDR" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="Yes" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:RadioButtonList>
                    <br />
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <br />
                    <br />
                    <asp:Button ID="btnUpload" CssClass="btn btn-primary" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                </div>
                <div class="col-lg-6">
                </div>


                <%-- <br />--%>
                <%--<div class="col-lg-12">
            <asp:GridView ID="GridView1" CssClass="table table-responsive" runat="server" OnPageIndexChanging="PageIndexChanging" AllowPaging="true">
        </asp:GridView>
        </div>--%>
                <%--<div class="col-lg-12">
            <asp:Button ID="btnprintbulkfile" runat="server" OnClick="btnprintbulkfile_Click" Text="PrintFiles" />
        </div>--%>
            </div>
        </div>
    </div>
</asp:Content>

