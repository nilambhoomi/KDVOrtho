<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="SchSettings.aspx.cs" Inherits="Default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" Runat="Server">
    <div class="container">
        <h4>Set Appointment Time Range</h4>
        <div class="col-md-4">
            Start Time <br />
            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlStart">
                <asp:ListItem Value="06:00" >6:00</asp:ListItem>
                <asp:ListItem Value="06:30" >6:30</asp:ListItem>
                <asp:ListItem Value="07:00" >7:00</asp:ListItem>
                <asp:ListItem Value="07:30" >7:30</asp:ListItem>
                <asp:ListItem Value="08:00" Selected >8:00</asp:ListItem>
                <asp:ListItem Value="08:30" >8:30</asp:ListItem>
                <asp:ListItem Value="09:00" >9:00</asp:ListItem>
                <asp:ListItem Value="09:30" >9:30</asp:ListItem>
                <asp:ListItem Value="10:00" >10:00</asp:ListItem>
                <asp:ListItem Value="18:30" >10:30</asp:ListItem>
                <asp:ListItem Value="11:00" >11:00</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            End Time <br />
              <asp:DropDownList runat="server" CssClass="form-control" ID="ddlEnd">
                <asp:ListItem Value="04:00" >4:00</asp:ListItem>
                <asp:ListItem Value="04:30" >4:30</asp:ListItem>
                <asp:ListItem Value="05:00" >5:00</asp:ListItem>
                <asp:ListItem Value="05:30" >5:30</asp:ListItem>
                <asp:ListItem Value="06:00" Selected >6:00</asp:ListItem>
                <asp:ListItem Value="06:30" >6:30</asp:ListItem>
                <asp:ListItem Value="07:00" >7:00</asp:ListItem>
                <asp:ListItem Value="07:30" >7:30</asp:ListItem>
                <asp:ListItem Value="08:00" >8:00</asp:ListItem>
                <asp:ListItem Value="08:30" >8:30</asp:ListItem>
                <asp:ListItem Value="09:00" >9:00</asp:ListItem>
                <asp:ListItem Value="09:30" >9:30</asp:ListItem>
                <asp:ListItem Value="10:00" >10:00</asp:ListItem>
            </asp:DropDownList>
        </div>
                <div class="col-md-4">


        <asp:Button ID="btnSet" runat="server" CssClass="btn btn-primary "  style="margin:12px 12px" Text="Set" OnClick="btnSet_Click"  />
                    </div>
    </div>
</asp:Content>

