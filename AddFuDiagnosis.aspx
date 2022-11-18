<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFuDiagnosis.aspx.cs" Inherits="AddFuDiagnosis" %>

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
 <style type="text/css">
        .Background
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .Popup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 700px;
            height: 550px;
        }
        .lbl
        {
            font-size:16px;
            font-style:italic;
            font-weight:bold;
        }
    </style>
<%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">--%>
<form runat="server" id="frm2">
    <div>
        <asp:Label runat="server" Text="Heading"></asp:Label>
        <asp:TextBox ID="txDesc" runat="server" ></asp:TextBox>
        <asp:ImageButton ID="btnFind" runat="server"  ImageUrl="~/img/25-16.png" />
        <asp:Button ID="btnSave" Text="Save and Close" runat="server" OnClick="btnSave_Click" CssClass="btn" />
    </div>
    <div>
        <asp:GridView ID="dgvDiagCodes" runat="server" AutoGenerateColumns="false" DataKeyNames="DiagCode_ID">
            <Columns>
                <asp:TemplateField HeaderText="Select">

                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>'  AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                        <%--<asp:TextBox ID="txtbodypart" runat="server" Text='<%# Eval("BodyPart") %>'></asp:TextBox>--%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblCode" runat="server" Text='<%# Eval("DiagCode") %>'></asp:Label>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PN" ItemStyle-Width="150">
                    <ItemTemplate>
                         <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PreSelect") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>


    </div>
</form>
    <script src="Scripts/jquery.js"></script>
    <script src="Scripts/jquery-ui.min.js"></script>
    <script>
        $(document).ready(function () {
            $(".date").datepicker();
        });
    </script>
<%--<script type="text/javascript">
    function RefreshParent() {
        window.opener.location.reload();
        window.close();
    }

</script>--%>
<%--</asp:Content>--%>

