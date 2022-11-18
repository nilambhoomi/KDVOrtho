<%@ Page Language="C#" AutoEventWireup="true" CodeFile="POCSummery.aspx.cs" MasterPageFile="~/PageMainMaster.master" Inherits="POCSummery" %>

<%@ Register Src="~/UserControl/UCPOCSummary.ascx" TagPrefix="uc1" TagName="UCPOCSummary" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <uc1:UCPOCSummary runat="server" ID="UCPOCSummary" />

</asp:Content>
