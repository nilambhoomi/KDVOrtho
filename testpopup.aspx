<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="testpopup.aspx.cs" Inherits="testpopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" Runat="Server">
    <script type="text/javascript">
        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }
</script>

    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" OnClientClick="return ShowModalPopup()" />
<asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
<cc1:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="mpe" runat="server"
    PopupControlID="pnlPopup" TargetControlID="lnkDummy" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
    <div class="header">
        Modal Popup
    </div>
    <div class="body">
        This is a Modal Popup.
        <br />
        <asp:Button ID="btnHide" runat="server" Text="Hide Modal Popup" OnClientClick="return HideModalPopup()" />
    </div>
</asp:Panel>
</asp:Content>

