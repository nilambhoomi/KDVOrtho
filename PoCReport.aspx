<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="PoCReport.aspx.cs" Inherits="PoCReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <link href="CSS/CSS.css" rel="stylesheet" type="text/css" />
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

        label {
            padding: 10px;
        }
    </style>
    <script>
        $(document).ready(function ($) {

            $('#<%=txtSearchFromdate.ClientID%>').mask("99/99/9999");
            $('#<%=txtSearchTodate.ClientID%>').mask("99/99/9999");

            $('#<%=txtSearchFromdate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
               // yearRange: "-100:+0",
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });

            $('#<%=txtSearchTodate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
               // yearRange: "-100:+0",
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });

        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <asp:HiddenField ID="hdn_ID" runat="server" />
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Generate Report							
									<i class="ace-icon fa fa-angle-double-right"></i>
                    </small>
                    <small>
                        <%--<asp:LinkButton ID="btnaddnew" runat="server" PostBackUrl="~/AddProcedure.aspx">Add New</asp:LinkButton>--%>
                    </small>
                </h1>
            </div>
            <asp:Panel ID="p" runat="server" DefaultButton="btnSearch">
                <div class="container">
                    <div class="row">
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">From Date</span>
                                <asp:TextBox ID="txtSearchFromdate" runat="server" OnServerValidate="CustomValidator1_ServerValidate"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">To date</span>
                                <asp:TextBox ID="txtSearchTodate" runat="server" OnServerValidate="CustomValidator2_ServerValidate"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">Location </span>
                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                         <div class="col-lg-3 col-md-6 col-sm-12">
                            <div class="input-group">
                                <span class="input-group-addon">Mcode Type </span>
                                <asp:DropDownList ID="ddlmcodetype" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="InHouse" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Rest" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <div class="input-group">
                                <asp:CheckBox ID="chkRequested" runat="server" Text="Requested" />
                                <asp:CheckBox ID="chkScheduled" runat="server" Text="Scheduled" />
                                <asp:CheckBox ID="chkExecuted" runat="server" Text="Executed" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="clearfix"></div>
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="input-group">
                                <asp:LinkButton ID="btnSearch" CssClass="btn success" runat="server" OnClick="btnSearch_Click" Text="Search"></asp:LinkButton>
                                <asp:LinkButton ID="btnReset" CssClass="btn btn-danger " Style="margin-left: 2px" runat="server" OnClick="btnReset_Click" Text="Reset"></asp:LinkButton>
                                <asp:LinkButton ID="lkExportToexcel" CssClass="btn btn-danger " Style="margin-left: 2px" runat="server" OnClick="lkExportToexcel_Click" Text="ExportExcel"></asp:LinkButton>
                                <asp:LinkButton ID="lnkTransfer" CssClass="btn btn-warning" Style="margin-left: 2px; display: none" runat="server" OnClick="lnkTransfer_Click" Text="Transfer to Execute"></asp:LinkButton>
                                <asp:LinkButton ID="lnkRescheduled" OnClientClick="return confirm('Are you sure you want to re-schesule this records ?')" CssClass="btn btn-info" Style="margin-left: 2px; display: none" runat="server" OnClick="lnkRescheduled_Click" Text="Reschedule"></asp:LinkButton>

                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </asp:Panel>
            <div class="clearfix"></div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <%--DataKeyNames="Procedure_ID"--%>
                    <div class="table-responsive">
                        <asp:GridView ID="gvProcedureTbl" AllowSorting="true" OnSorting="gridView_Sorting" runat="server" AutoGenerateColumns="false" PageSize="100" CssClass="table table-striped table-bordered table-hover" AllowPaging="True" PagerStyle-CssClass="pager">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkExe" />
                                        <asp:HiddenField runat="server" ID="hID" Value='<%# Eval("ProcedureDetail_ID") %>' />
                                        <asp:HiddenField runat="server" ID="sDate" Value='<%# Eval("Scheduled") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="sex" HeaderText="Sex" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                
                                <asp:BoundField DataField="Case" HeaderText="Case" />
                                <asp:BoundField DataField="DOB" HeaderText="DOB" />
                                <asp:BoundField DataField="DOA" HeaderText="DOA" />
                                <asp:BoundField DataField="MCODE" HeaderText="MCODE" />
                           
                                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                <asp:BoundField DataField="location" HeaderText="Location" SortExpression="location" />

                                <asp:BoundField HeaderText="Requested" DataField="Requested" />
                                <asp:BoundField HeaderText="Schedule" DataField="Scheduled" />
                                <asp:BoundField HeaderText="Executed" DataField="Executed" />


                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <% if (chkScheduled.Checked)
                                           {%>
                                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnReshedule" OnClientClick="return confirm('Are you sure you want to re-scheduled this record ?')" CommandArgument='<%# Eval("ProcedureDetail_ID") %>' Visible='<%# string.IsNullOrEmpty(Eval("Scheduled").ToString())?false:true %>' Text="Rescheduled" OnClick="btnReshedule_Click" />
                                        <%} %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle BackColor="#C2D69B" />
                        </asp:GridView>
                    </div>
                    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">

        function ExecuteConfirm() {
            var Ans = confirm("Are you sure,you want to transfer it to execute ?");
            if (Ans) {
                return true;
            }
            else {
                return false;
            }
        }

        $(function () {
            $("[id*=chkScheduled]").click(function () {
                if ($(this).is(":checked")) {
                    $("[id*=lnkTransfer]").show();
                    $("[id*=lnkRescheduled]").show();
                } else {
                    $("[id*=lnkTransfer]").hide();
                    $("[id*=lnkRescheduled]").hide();
                }
            });
        });
    </script>
</asp:Content>


