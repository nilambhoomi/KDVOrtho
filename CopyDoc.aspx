<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CopyDoc.aspx.cs" Inherits="CopyDoc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <p>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Clone" />
            <br />
            <br />
        
        </p>
        <div>
        </div>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
