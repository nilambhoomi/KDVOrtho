<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="ProcedureView.aspx.cs" Inherits="ProcedureView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" Runat="Server">
     <div class="main-container">

        <h2>Intial Evaluation(s) &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="TimeSheet.aspx">Sign-in Sheet</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="LogDetail.aspx">Log Detail</a>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="Templatestorepdf.aspx">Forms</a>


        </h2>

        <asp:DropDownList ID="BodyPartDDN" runat="server" OnSelectedIndexChanged="BodyPartDDN_SelectedIndexChanged1">
            <asp:ListItem Value="ALL">All</asp:ListItem>
            <asp:ListItem Value="Neck">Neck</asp:ListItem>
            <asp:ListItem Value="LowBack">LowBack</asp:ListItem>
            <asp:ListItem Value="MidBack">MidBack</asp:ListItem>
            <asp:ListItem Value="Shoulder">Shoulder</asp:ListItem>
            <asp:ListItem Value="Ankle">Ankle</asp:ListItem>
            <asp:ListItem Value="Elbow">Elbow</asp:ListItem>
            <asp:ListItem Value="Wrist">Wrist</asp:ListItem>
            <asp:ListItem Value="Knee">Knee</asp:ListItem>
            <asp:ListItem Value="Knee">Knee</asp:ListItem>
            <asp:ListItem Value="Hip">Hip</asp:ListItem>
        </asp:DropDownList>

         <asp:GridView ID="gvProcedureTbl" runat="server" AutoGenerateColumns="false"  Width="80%"  CssClass="Grid" DataKeyNames="Procedure_ID" AllowPaging="True"  PagerStyle-CssClass="pager">
    <Columns>
        <asp:BoundField DataField="Procedure_ID" HeaderText="Procedure ID" />
        <asp:BoundField DataField="MCode" HeaderText="MCode" />
        <asp:BoundField DataField="DateType" HeaderText="DateType" />
        <asp:BoundField DataField="BodyPart" HeaderText="Body Part" />
        <asp:BoundField DataField="Heading" HeaderText="Heading" />
        <asp:BoundField DataField="CCDesc" HeaderText="CCDesc"/>
        <asp:BoundField DataField="PEDesc" HeaderText="PEDesc" />
        <asp:BoundField DataField="ADesc" HeaderText="ADesc" />
        <asp:BoundField DataField="PDesc" HeaderText="PDesc" />
        <asp:BoundField DataField="CF" HeaderText="CF" />
        <asp:BoundField DataField="PN" HeaderText="PN" />
         <asp:TemplateField>
           <ItemTemplate>
               <a onclick="alert(<%# Eval("Procedure_ID") %>)">Edit PROC</a>
                     <%-- <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" NavigateUrl='' Text="Edit PROC"></asp:HyperLink> |                 --%>
           </ItemTemplate>
         </asp:TemplateField>  

        </Columns>
             </asp:GridView>
         </div>
</asp:Content>

