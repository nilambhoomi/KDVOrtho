<%@ Page Title="" Language="C#" MasterPageFile="~/FollowUpMaster.master" AutoEventWireup="true" CodeFile="PatientDocumentEditFU.aspx.cs" Inherits="PatientDocumentEditFU" %>

<%@ Register Src="~/UserControl/UCPatientDocument.ascx" TagPrefix="uc1" TagName="UCPatientDocument" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:UCPatientDocument runat="server" ID="UCPatientDocument" />
</asp:Content>

