<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="GeneralDocument.aspx.cs" Inherits="GeneralDocument" %>

<%@ Register Src="~/UserControl/UCGeneralDocuments.ascx" TagPrefix="uc1" TagName="UCGeneralDocuments" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">


    <uc1:UCGeneralDocuments runat="server" ID="UCGeneralDocuments" />

</asp:Content>

