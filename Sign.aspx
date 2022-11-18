<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sign.aspx.cs" Inherits="Sign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link rel="stylesheet" href="css/signature-pad.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <table style="width: 400px">
            <tr>
                <td>
                    <div id="signature-pad" class="signature-pad">
                        <canvas runat="server" id="can" style="height: 200px"></canvas>
                        <div class="signature-pad--actions">
                            <div>
                                <button type="button" class="button clear" data-action="clear">Clear</button>
                                <%-- <button type="button" class="button" data-action="change-color">Change color</button>
                                <button type="button" class="button" data-action="undo">Undo</button>--%>
                            </div>
                            <div>
                                <button type="button" class="button save" id="btnsignsave" data-action="save-png">Save Sign</button>

                            </div>
                        </div>
                    </div>

                </td>
            </tr>
          
            <tr>
                <td>
                    <input type="hidden" id="hidBlob" runat="server" />
                    <asp:Button runat="server" ID="btnSave"  Text="Save" OnClick="btnSave_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblMessage"></asp:Label>
                </td>
            </tr>
        </table>

        <%--   </div>--%>
    </form>
</body>
<script src="js/signature_pad.umd.js"></script>
<script src="js/app.js"></script>
</html>
