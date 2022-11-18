<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MRIPending.aspx.cs" Inherits="MIRPending" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MRI's Pending List</title>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="form-horizontal">
        <div class="control-group span3">
            <label class="control-label">Location: </label>
            <div class="controls">
                <asp:DropDownList runat="server" ID="ddl_location" Width="190px">
                    <asp:ListItem Text="BaySide" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Bronx-Davidson Ave" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Corona" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Forest Hills" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>    
        <div class="control-group span3">
            <label class="control-label">Day(s): </label>
            <div class="controls">
                <asp:TextBox value="30" runat="server" ID="txt_Days" Width="31px"></asp:TextBox>
            </div>
        </div>
        <div class="controls">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-primary" UseSubmitBehavior="true" />
        </div>
        </div>
        <div class="table-responsive">
        <asp:Repeater ID="rpview" runat="server">
            <HeaderTemplate>
                <table class="table table-striped table-bordered" style="width: 100%">
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Patient Name</th>
                            <th>DOA</th>
                            <th>DOE</th>
                            <th>IE/FU</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("SEX")  %></td>
                    <td><%# Eval("LastName")+" "+Eval("FirstName")  %></td>
                    <td><%# Eval("DOA","{0:MM-dd-yyyy}") %></td>
                    <td><%# Eval("DOE","{0:MM-dd-yyyy}") %></td>
                    <td>IE/FU</td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
    </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    </div>
      <div>
          <asp:GridView ID="gdview" runat="server"></asp:GridView>
      </div>
    </form>
</body>
</html>
