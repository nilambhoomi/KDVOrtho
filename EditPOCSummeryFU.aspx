<%@ Page Title="" Language="C#" MasterPageFile="~/FollowUpMaster.master" AutoEventWireup="true" CodeFile="EditPOCSummeryFU.aspx.cs" Inherits="EditPOCSummeryFU" %>

<%@ Register Src="~/UserControl/UCPOCSummary.ascx" TagPrefix="uc1" TagName="UCPOCSummary" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:UCPOCSummary runat="server" ID="UCPOCSummary" />
</asp:Content>

