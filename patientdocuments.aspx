<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="patientdocuments.aspx.cs" Inherits="patientdocuments" %>

<%@ Register Src="~/UserControl/UCDocuments.ascx" TagPrefix="uc1" TagName="UCDocuments" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" Runat="Server">
    <uc1:UCDocuments runat="server" id="UCDocuments" />
</asp:Content>

