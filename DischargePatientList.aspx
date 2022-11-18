<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="DischargePatientList.aspx.cs" Inherits="DischargePatientList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
     <asp:PlaceHolder ID="PlaceHolder1" runat="server" />

    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Discharge Patient Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>


            <div class="">

                <div class="row">
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-success" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-success" Text="Refresh" OnClick="btnRefresh_Click" />
                                        <asp:HiddenField ID="hfPatientId" runat="server"></asp:HiddenField>
                                    </div>
                                  
                                    <div class="col-sm-2" style="float: right">
                                        <asp:DropDownList runat="server" ID="ddlPage" AutoPostBack="true" Style="float: right; width: 70px" CssClass="form-control" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="40"></asp:ListItem>
                                            <%--   <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space"></div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvPatientDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="PatientIE_ID" OnRowDataBound="OnRowDataBound" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" ID="plusimg" Style="cursor: pointer" ImageUrl="img/plus.png" />
                                                    <%-- <img alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" id="plusimg" style="cursor: pointer" src="img/plus.png" />--%>
                                                    <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                        <asp:GridView ID="gvPatientFUDetails" BorderStyle="None" CssClass="table table-bordered" Width="100%" runat="server" AllowPaging="True" OnPageIndexChanging="gvPatientFUDetails_PageIndexChanging" AutoGenerateColumns="False" EmptyDataText="No Records Found" PagerStyle-CssClass="pager">
                                                            <Columns>
                                                                <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                                                <asp:BoundField DataField="MAProviders" HeaderText="MA & Providers" />
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>

                                                                        <%--<asp:HyperLink runat="server" CssClass="btn btn-info" ID="hlAddFU" NavigateUrl='<%# "~/TimeSheet.aspx?PId="+Eval("PatientIEId")+"&FID="+Eval("PatientFUId")  %>' Text="Procedure Details"></asp:HyperLink>--%>
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-info" ID="HyperLink1" NavigateUrl='<%# "~/EditFU.aspx?FUID="+Eval("PatientFUId") %>' Text="Edit"></asp:HyperLink>
                                                                        <%-- |
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="lkbtnReprint" Text="Print"></asp:HyperLink>--%>
                                                                        | 
																<%--<asp:HyperLink runat="server" CssClass="btn btn-info" ID="HyperLink3" NavigateUrl='<%# "~/EditFU.aspx?FUID="+Eval("PatientFUId") %>' Text="Edit"></asp:HyperLink>--%>
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink2" Text='<%# Eval("PrintStatus").ToString() %>'></asp:HyperLink>
                                                                        
                                                                         <asp:LinkButton runat="server"  ID="lnkfurod" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientFUId")+ "-" +Eval("PatientIEId") %>' Visible="false" OnClick="lnkfurod_Click">RoD</asp:LinkButton>

                                                                        <%-- <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink3" Text="Print ROD"></asp:HyperLink>--%>

                                                                        <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink4" Text='DownloadRoD'></asp:HyperLink>--%>
                                                                        <asp:HyperLink runat="server" Visible="false" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink3" Text='<%# Eval("PrintStatusRod").ToString().Equals("Print Requested")? "" : Eval("PrintStatusRod").ToString().Equals("Printing")?"Printing Rod":Eval("PrintStatusRod").ToString().Equals("Download")? "dl RoD":Eval("PrintStatusRod").ToString()  %>'></asp:HyperLink>
                                                                        

                                                                          <asp:LinkButton runat="server" ID="lnkprintFU" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIEId").ToString()+","+Eval("PatientFUId").ToString()  %>' OnClick="lnkprintFU_Click">Print</asp:LinkButton>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                        </asp:GridView>
                                                        <%--<asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="OrderId" HeaderText="Order Id" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="OrderDate" HeaderText="Date" />
                        </Columns>
                    </asp:GridView>--%>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Sex" HeaderText="Title" />
                                            <asp:BoundField DataField="lastname" HeaderText="LastName" />
                                            <asp:BoundField DataField="firstname" HeaderText="FirstName" />
                                            <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="DOA" HeaderText="DOA" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Compensation" HeaderText="Case Type" />
                                            <asp:BoundField DataField="location" HeaderText="Location" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" NavigateUrl='<%# "~/Page1.aspx?id="+Eval("PatientIE_ID") %>' Text="Edit IE">
                                      
                                                    </asp:HyperLink>
                                                    
              <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlAddFU" NavigateUrl='<%# "~/AddFU.aspx?PID="+Eval("PatientIE_ID") %>'  Visible="false" Text="AddFU"></asp:HyperLink>
                                                    <%--  | 
              <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="lkbtnReprint" Text="Print"></asp:HyperLink>--%>
                                                   
              <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink2" Visible="false" Text='<%# Eval("PrintStatus").ToString() %>'></asp:HyperLink>
                                                   
                                                    
                                                    <asp:LinkButton runat="server" ID="lnkierod" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID") %>' Visible="false" OnClick="lnkierod_Click">RoD</asp:LinkButton>

                                                    <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink5" Text="Print RoD"></asp:HyperLink>--%>

                                                    <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink6" Text='DownloadRoD'></asp:HyperLink>--%>
                                                    <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' Visible="false" data-FUIE="IE" ID="HyperLink4" Text='<%# Eval("PrintStatusRod").ToString().Equals("Print Requested")? "" : Eval("PrintStatusRod").ToString().Equals("Printing")?"Printing Rod":Eval("PrintStatusRod").ToString().Equals("Download")? "dl RoD":Eval("PrintStatusRod").ToString()  %>'></asp:HyperLink>
                                                    |
                                                                          <asp:LinkButton runat="server" ID="lnkprint" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID") %>' OnClick="lnkprint_Click">Print</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- <asp:BoundField ItemStyle-Width="150px" DataField="ContactName" HeaderText="Contact Name" />
            <asp:BoundField ItemStyle-Width="150px" DataField="City" HeaderText="City" />--%>
                                        </Columns>
                                        <PagerSettings PageButtonCount="5" />

                                        <PagerStyle CssClass="pager"></PagerStyle>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="printDocumentPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        Print Document
                                          <div style="display: inline-block; width: 80%; text-align: center;">
                                              <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                          </div>
                                    </div>
                                    <div class="modal-body">
                                        <div id="divPrint" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal fade" id="RodPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        Select ROD 
                                        <div style="display: inline-block; width: 80%; text-align: center;">
                                            <asp:Button ID="btnrodsave" CssClass="btn btn-success" Style="margin-left: 15px" runat="server" OnClick="btnrodsave_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                            <asp:Literal ID="ltrprint" runat="server"></asp:Literal>
                                            <asp:Literal ID="ltrdownload" runat="server"></asp:Literal>

                                            <asp:Button ID="btnRODDelete" Text="Delete" OnClick="btnRODDelete_Click" runat="server" CssClass="btn btn-danger" />

                                        </div>
                                        <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    </div>
                                    <div class="modal-body">
                                        <asp:UpdatePanel runat="server" ID="upRod">
                                            <ContentTemplate>
                                                <div class="col-md-9 inline">

                                                    <asp:Repeater runat="server" ID="repRoD">
                                                        <HeaderTemplate>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td colspan="2"><b>ROD:</b>  </td>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <tr>
                                                                <td style="width: 20px">
                                                                    <asp:CheckBox runat="server" OnCheckedChanged="chk_CheckedChanged" ID="chk" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("isChecked")) %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="bodypart" Value='<%# Eval("bodypart") %>' runat="server" />
                                                                    <asp:HiddenField ID="isnewline" Value='<%# Eval("isnewline") %>' runat="server" />
                                                                    <%--<asp:TextBox runat="server" OnTextChanged="txtRod_TextChanged" TextMode="MultiLine" AutoPostBack="true" ID="txtRod" Text='<%# Eval("name") %>' Width='138%' /></td>--%>
                                                                    <% if (iCounter == 1 || iCounter == 14 || iCounter == 16)
                                                                        { %>
                                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtRod" Text='<%# Eval("name") %>' Columns="100" Rows="7" />
                                                                    <%}
                                                                        else
                                                                        { %>
                                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtRod1" Text='<%# Eval("name") %>' Columns="100" Rows="1" />
                                                                    <%}
                                                                        iCounter++;
                                                                    %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                    <div style="display: none">
                                                        <asp:TextBox runat="server" ID="txtrodFulldetails" Style="display: none"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdbodyparts" />
                                                        <asp:HiddenField runat="server" ID="hdnewline" />
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                                            <asp:Button ID="Button1" CssClass="btn btn-success" Style="margin-left: 15px" runat="server" OnClick="btnrodsave_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnrodieid" runat="server" />
                        <asp:HiddenField ID="hdnrodeditedfuid" runat="server" />
                        <asp:HiddenField ID="hdnrodeditedfuieid" runat="server" />
                        <asp:HiddenField ID="hfCurrentlyOpened" runat="server"></asp:HiddenField>
                        <%-- <asp:GridView ID="gvPatientDetails" Width="80%" runat="server" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging" AutoGenerateColumns="False" EmptyDataText="No Records Found" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="Sex" HeaderText="Title" />
                <asp:BoundField DataField="lastname" HeaderText="LastName" />
                <asp:BoundField DataField="firstname" HeaderText="FirstName" />
                <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DOA" HeaderText="DOA" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}"/>
                <asp:BoundField DataField="location" HeaderText="Location" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink runat="server" CssClass="btn" ID="hlAddFU" NavigateUrl='<%# "~/AddFU.aspx?PID="+Eval("PatientIE_ID") %>' Text="AddFU"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>               
            </Columns>
          
            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
          
            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
          
        </asp:GridView>--%>
                    </div>
                </div>
                <script src="Scripts/jquery-1.8.2.js"></script>
                <script src="Scripts/jquery-ui-1.8.24.js"></script>
                <link href="Style/jquery-ui.css" rel="stylesheet" />
                <script type="text/javascript">
                    $.noConflict();
                    function openModelPopup() {
                        $('#RodPopup').modal('show');
                    }

                    function openPrintPopup() {
                        $('#printDocumentPopup').modal('show');
                    }

                    function closeModelPopup() {
                        //jQuery.noConflict();
                        //(function ($) {

                        $('#RodPopup').modal('hide');

                        //})(jQuery);
                    }

                    var downloadPath = '<%=ConfigurationSettings.AppSettings["downloadpath"]%>';
                    $(document).ready(function () {
                        if ($('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]')) {
                            $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').closest("tr").after("<tr><td></td><td colspan = '999'>" + $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').next().html() + "</td></tr>");
                            $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').attr("src", "img/minus.png");
                        }
                        $("[src*=plus]").live("click", function () {
                            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                            $(this).attr("src", "img/minus.png");
                        });

                        $("[src*=minus]").live("click", function () {
                            $(this).attr("src", "img/plus.png");
                            $(this).closest("tr").next().remove();
                        });

                        <%--$("#<%=txtSearch.ClientID %>").autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                    url: 'Search.aspx/GetPatients',
                                    data: "{ 'prefix': '" + request.term + "'}",
                                    dataType: "json",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        response($.map(data.d, function (item) {
                                            return {
                                                label: item.split('_')[0],
                                                val: item.split('_')[1]
                                            }
                                        }))
                                    },
                                    error: function (response) {
                                        alert(response.responseText);
                                    },
                                    failure: function (response) {
                                        alert(response.responseText);
                                    }
                                });
                            },
                            select: function (e, i) {
                                $("#<%=hfPatientId.ClientID %>").val(i.item.val);
                                $('#<%= txtSearch.ClientID %>').val(i.item.label);
                                $('#<%= btnSearch.ClientID %>').click();
                            },
                            minLength: 1
                        });--%>
                    });


                    $(document).on("click", ".PrintClick", function () {
                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var isdownload = 0;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "print requested") {
                            alert("You already given print request.");
                            return false;
                        }
                        else if ($(this).html().toLowerCase() == "download" || $(this).html().toLowerCase() == "downloaded") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatus',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (currentID.indexOf("lkbtnReprint") != -1) {
                                        alert("Print Request received.")
                                        location.reload();
                                    }
                                    else {
                                        $(obj).html("Print Requested");
                                    }
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownload',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    })

                    $(document).on("click", ".PrintClickRod", function () {


                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var isdownload = 0;
                        debugger;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "print requested") {
                            alert("You already given print request.");
                            return false;
                        }
                        else if ($(this).html().toLowerCase() == "dl rod" || $(this).html().toLowerCase() == "download") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatusRod',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (currentID.indexOf("lkbtnReprint") != -1) {
                                        alert("Print Request received.")
                                        location.reload();
                                    }
                                    else {
                                        $(obj).html("Print Requested");
                                    }
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownloadRod',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    })

                </script>
            </div>
        </div>
    </div>
</asp:Content>

