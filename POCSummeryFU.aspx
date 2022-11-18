<%@ Page Title="" Language="C#" MasterPageFile="~/AddFollowUpMaster.master" AutoEventWireup="true" CodeFile="POCSummeryFU.aspx.cs" Inherits="POCSummeryFU" %>

<%@ Register Src="~/UserControl/UCPOCSummary.ascx" TagPrefix="uc1" TagName="UCPOCSummary" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:UCPOCSummary runat="server" ID="UCPOCSummary" />
</asp:Content>

