<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="Procedures.aspx.cs" Inherits="Procedures" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%----%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--
        a.btn {
            text-decoration: none;
        }

        table {
            text-align: center;
        }

        .main-container {
            min-height: 900px;
        }--%>
    <style>
        .pager::before {
            display: none;
        }

        .pager table {
            margin: 0 auto;
        }

            .pager table tbody tr td a,
            .pager table tbody tr td span {
                position: relative;
                float: left;
                padding: 6px 12px;
                margin-left: -1px;
                line-height: 1.42857143;
                color: #337ab7;
                text-decoration: none;
                background-color: #fff;
                border: 1px solid #ddd;
            }

            .pager table > tbody > tr > td > span {
                z-index: 3;
                color: #fff;
                cursor: default;
                background-color: #337ab7;
                border-color: #337ab7;
            }

            .pager table > tbody > tr > td:first-child > a,
            .pager table > tbody > tr > td:first-child > span {
                margin-left: 0;
                border-top-left-radius: 4px;
                border-bottom-left-radius: 4px;
            }

            .pager table > tbody > tr > td:last-child > a,
            .pager table > tbody > tr > td:last-child > span {
                border-top-right-radius: 4px;
                border-bottom-right-radius: 4px;
            }

            .pager table > tbody > tr > td > a:hover,
            .pager table > tbody > tr > td > span:hover,
            .pager table > tbody > tr > td > a:focus,
            .pager table > tbody > tr > td > span:focus {
                z-index: 2;
                color: #23527c;
                background-color: #eee;
                border-color: #ddd;
            }
    </style>
    <link href="CSS/CSS.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.blockUI.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BlockUI(elementID) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(function () {
                $("#" + elementID).block({
                    message: '<table align = "center"><tr><td>' +
             '<img src="images/loadingAnim.gif"/></td></tr></table>',
                    css: {},
                    overlayCSS: {
                        backgroundColor: '#000000', opacity: 0.6
                    }
                });
            });
            prm.add_endRequest(function () {
                $("#" + elementID).unblock();
            });
        }
        $(document).ready(function () {

            BlockUI("<%=pnlAddEdit.ClientID %>");
            BlockUI("<%=BodyPartDDN.ClientID %>");
        $.blockUI.defaults.css = {};
    });
        function Hidepopup() {
            //debugger
            location.reload();
            // $find("#ctl00_cpMain_popup").hide();
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
                    <div class="page-content">
                        <div class="page-header">
                            <h1>
                                <small>Manage Procedures							
									<i class="ace-icon fa fa-angle-double-right"></i>

                                </small>
                            </h1>
                        </div>
 <%--   <body style="margin: 0; padding: 0">

        <div class="main-container">--%>
            <%--   <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" ScriptMode = "Release" runat="server">
             </cc1:ToolkitScriptManager>--%>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>--%>


         <%--   <h2>Intial Evaluation(s) &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="TimeSheet.aspx">Sign-in Sheet</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="LogDetail.aspx">Log Detail</a>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="Templatestorepdf.aspx">Forms</a>

                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="Procedures.aspx" class="active">Procedure</a>


            </h2>--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <center> 
          <asp:DropDownList ID="BodyPartDDN" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="BodyPartDDN_SelectedIndexChanged1">
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
      
         <asp:GridView ID="gvProcedureTbl" runat="server" AutoGenerateColumns="false"    CssClass="table table-striped table-bordered table-hover" DataKeyNames="Procedure_ID"  AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1"  PagerStyle-CssClass="pager">
    <Columns>
        <asp:BoundField DataField="Procedure_ID" HeaderText="Procedure ID" />
        <asp:BoundField DataField="MCode" HeaderText="MCode" />
        <asp:BoundField DataField="DateType" HeaderText="DateType" />
        <asp:BoundField DataField="BodyPart" HeaderText="Body Part" />
        <asp:BoundField DataField="Heading" HeaderText="Heading" />
   <%--     <asp:BoundField DataField="CCDesc" HeaderText="CCDesc"/>
        <asp:BoundField DataField="PEDesc" HeaderText="PEDesc" />
        <asp:BoundField DataField="ADesc" HeaderText="ADesc" />--%>
        <asp:BoundField DataField="PDesc" HeaderText="PDesc" />
        <asp:BoundField DataField="CF" HeaderText="CF" />
        <asp:BoundField DataField="PN" HeaderText="PN" />
         <asp:TemplateField>
           <ItemTemplate>
               <%--<a onclick="alert(<%# Eval("Procedure_ID") %>)" class="btn btn-link">Edit PROC</a>--%>
               <asp:LinkButton ID="lnkEdit" runat="server" Text = "Edit" OnClick = "Edit"></asp:LinkButton>
                     <%-- <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" NavigateUrl='' Text="Edit PROC"></asp:HyperLink> |                 --%>
           </ItemTemplate>
         </asp:TemplateField>  

        </Columns>
             <AlternatingRowStyle BackColor="#C2D69B" />
             </asp:GridView>
              </center>
                    </div>


    <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup" Style="display: none">
        <asp:Label Font-Bold="true" ID="Label4" runat="server" Text="Procedure Details"></asp:Label>
        <br />
        <table align="center">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="MCode"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMCode"  runat="server"></asp:TextBox>
                </td>
            </tr>
              <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Date Type"></asp:Label>
                </td>
                <td>
                   <asp:DropDownList ID="datetypedwn" runat="server">
                       <asp:ListItem Value="R" Text="Request"></asp:ListItem>
                       <asp:ListItem Value="S" Text="Schedule"></asp:ListItem>
                       <asp:ListItem Value="E" Text="Execute"></asp:ListItem>
                   </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Heading"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtHeading" TextMode="multiline" Columns="500" Rows="5" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="PDesc"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPDesc" TextMode="multiline" Columns="500" Rows="5" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><asp:HiddenField ID="hdn_ID" runat="server" /></td>
                </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="Save" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="return Hidepopup()" />
                </td>
            </tr>


        </table>
    </asp:Panel>
                    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                    <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                        PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                        BackgroundCssClass="modalBackground">
                    </cc1:ModalPopupExtender>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvProcedureTbl" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
  <%--  </body>--%>
</div>
        </div>
</asp:Content>

