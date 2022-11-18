<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Exportrecords.aspx.cs" Inherits="Page1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
 
    <br />
    <h2 style="color: #808000; font-size: x-large; font-weight: bolder;">Export & Import Records
    </h2>
    <br />
    <div>
       
        <br />
        <asp:Button ID="Button1" runat="server"
            Text="Export Excel File" OnClick="btnSave_Click" />
    </div>
    <div>
        <asp:FileUpload ID="fileUpload" runat="server" />
        <asp:Button ID="btnUpload" runat="server"
            Text="Import Excel File" OnClick="btnUpload_Click" />
    </div>

     <div>
        <asp:Label ID="lblmsg" runat="server"></asp:Label>
    </div>

</asp:Content>

