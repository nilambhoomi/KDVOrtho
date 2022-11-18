<%@ Page Title="" Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="PatientDocument.aspx.cs" Inherits="PatientDocument" %>

<%@ Register Src="~/UserControl/UCPatientDocument.ascx" TagPrefix="uc1" TagName="UCPatientDocument" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   

   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:UCPatientDocument runat="server" ID="UCPatientDocument" />

</asp:Content>

