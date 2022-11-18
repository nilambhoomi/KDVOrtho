<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="Templatestorepdfnew.aspx.cs" Inherits="Templatestorepdfnew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://raw.githubusercontent.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script type="text/javascript" src='js/SignJs/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='js/SignJs/bootstrap.min.js'></script>
    <script type="text/javascript" src="js/SignJs/bootstrap3-typeahead.min.js"></script>

    <%-- <script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="js/SignJs/jquery-ui.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <link rel="stylesheet" href="css/signature-pad.css">
    <script type="text/javascript">
        //function openPopup(divid) {

        //    $('#' + divid + '').modal('show');

        //}

        $(document).ready(function () {
            $("Div[id*='ctl00_cpMain_TreeView1n']").attr("style", "display:none")
            $('[id*=txt_name]').typeahead({
                hint: true,
                highlight: true, minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Templatestorepdf.aspx/getFirstName") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    $('[id*=hfpatientId]').val(map[item].id);
                    return item;
                }
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                $('[id*=txt_name]').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Templatestorepdf.aspx/getFirstName") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        $('[id*=hfpatientId]').val(map[item].id);
                        return item;
                    }
                });
            }
        });


    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("Div[id*='ctl00_cpMain_TreeView1n']").attr("style", "display:none")
            $('[id*=txtProcedureCode]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Templatestorepdf.aspx/getProcCode") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    $('[id*=hfpatientId]').val(map[item].id);
                    return item;
                }
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                $('[id*=txtProcedureCode]').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Templatestorepdf.aspx/getProcCode") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        $('[id*=hfpatientId]').val(map[item].id);
                        return item;
                    }
                });
            }
        });


    </script>

    <style>
        .pagination {
            height: 36px;
            margin: 18px 0;
            color: #6c58bF;
        }

            .pagination ul {
                display: inline-block;
                *display: inline;
                /* IE7 inline-block hack */
                *zoom: 1;
                margin-left: 0;
                color: #ffffff;
                margin-bottom: 0;
                -webkit-border-radius: 3px;
                -moz-border-radius: 3px;
                border-radius: 3px;
                -webkit-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
                -moz-box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
                box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
            }

            .pagination li {
                display: inline;
                color: #6c58bF;
            }

            .pagination a {
                float: left;
                padding: 0 14px;
                line-height: 34px;
                color: #6c58bF;
                text-decoration: none;
                border: 1px solid #ddd;
                border-left-width: 0;
            }

                .pagination a:hover,
                .pagination .active a {
                    background-color: #6c58bF;
                    color: #ffffff;
                }

                .pagination a:focus {
                    background-color: #6c58bF;
                    color: #ffffff;
                }


            .pagination .active a {
                color: #ffffff;
                cursor: default;
            }

            .pagination .disabled span,
            .pagination .disabled a,
            .pagination .disabled a:hover {
                color: #999999;
                background-color: transparent;
                cursor: default;
            }

            .pagination li:first-child a {
                border-left-width: 1px;
                -webkit-border-radius: 3px 0 0 3px;
                -moz-border-radius: 3px 0 0 3px;
                border-radius: 3px 0 0 3px;
            }

            .pagination li:last-child a {
                -webkit-border-radius: 0 3px 3px 0;
                -moz-border-radius: 0 3px 3px 0;
                border-radius: 0 3px 3px 0;
            }

        .pagination-centered {
            text-align: center;
        }

        .pagination-right {
            text-align: right;
        }

        .pager {
            margin-left: 0;
            margin-bottom: 18px;
            list-style: none;
            text-align: center;
            color: #6c58bF;
            *zoom: 1;
        }

            .pager:before,
            .pager:after {
                display: table;
                content: "";
            }

            .pager:after {
                clear: both;
            }

            .pager li {
                display: inline;
                color: #6c58bF;
            }

            .pager a {
                display: inline-block;
                padding: 5px 14px;
                color: #6c58bF;
                background-color: #fff;
                border: 1px solid #ddd;
                -webkit-border-radius: 15px;
                -moz-border-radius: 15px;
                border-radius: 15px;
            }

                .pager a:hover {
                    text-decoration: none;
                    background-color: #f5f5f5;
                }

            .pager .next a {
                float: right;
            }

            .pager .previous a {
                float: left;
            }

            .pager .disabled a,
            .pager .disabled a:hover {
                color: #999999;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Manage Report				
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>
            <div class="span6">
                <p>Note: Please select aleast one report.</p>
                <%--ShowCheckBoxes="Leaf" --%>
                <%--NodeIndent="15"--%>
                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Custom"
                    ExpandDepth="1">
                    <HoverNodeStyle Font-Underline="false" ForeColor="#6666AA" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                        NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                    <ParentNodeStyle Font-Bold="False" />

                    <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="true" HorizontalPadding="0px"
                        VerticalPadding="0px" />
                </asp:TreeView>

            </div>
            <div>

                <div class="span6">
                    <%--ShowCheckBoxes="Leaf" --%>
                    <asp:TreeView ID="TreeView2" runat="server" ImageSet="Custom"
                        NodeIndent="15" ExpandDepth="0">
                        <%-- <HoverNodeStyle Font-Underline="false" ForeColor="#6666AA" />
                <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                    NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                <ParentNodeStyle Font-Bold="False" />

                <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="true" HorizontalPadding="0px"
                    VerticalPadding="0px" />--%>
                    </asp:TreeView>

                </div>
                <asp:Label ID="Message"
                    runat="server" />
            </div>
            <div class="clearfix"></div>
            Search By Name:
    <asp:TextBox runat="server" ID="txt_name" OnTextChanged="txt_name_TextChanged" AutoPostBack="true"></asp:TextBox>
            &nbsp;
    <button class="btn" type="button">Search</button>
            <div class="row">
                <div class="col-lg-12">
                    <br />
                </div>
                <div class="col-lg-12">
                    <span class="form-group">The following box can used to print the Procedure Details in the form.</span>
                </div>
                <div class="col-lg-12">
                    <br />
                </div>
                <div class="col-lg-3">
                    <div class="form-group">
                        Procedure date:
                    <asp:TextBox runat="server" ID="txtproc_code_date"></asp:TextBox>
                    </div>
                </div>
                <div class="col-lg-3">
                    <div class="form-group">
                        Procedure Code:
                    <asp:TextBox runat="server" ID="txtProcedureCode" widht="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
            <%--<asp:Label runat="server" ID="lblMessage" Visible="false"></asp:Label>
    <img src="img/Select only one.gif" height="8%" width="10%" />--%>
            <%--<asp:Image ID="lblMessage" ImageUrl="~/img/select one.gif" runat="server"   height="9%" width="12%"  />--%>
            <%--ImageUrl="~/img/select one.gif"--%>

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
                                    <th>Download</th>
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
                            <th>
                                <asp:LinkButton runat="server" ID="lnk_openIE" CommandArgument='<%# Eval("PatientIE_ID")  %>' OnClick="lnk_openIE_New_Click" CssClass="btn-link" Text="VIEW"></asp:LinkButton>

                                <%--<asp:LinkButton runat="server" ID="LinkButton1" CommandArgument='<%# Eval("PatientIE_ID")  %>' OnClick="lnk_openIE_Click" CssClass="btn-link" Text="Edit"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LinkButton2" CommandArgument='<%# Eval("PatientIE_ID")  %>' OnClick="lnk_openIE_Click" CssClass="btn-link" Text="Edit"></asp:LinkButton>--%>
                            </th>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
    </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>


            <div class="col-md-12" runat="server" id="div_page">
                Page
            <label runat="server" id="lbl_page_no" style="display: inline"></label>
                of
            <label runat="server" id="lbl_total_page" style="display: inline"></label>
            </div>
            <div class="col-md-12">

                <ul class="pagination">
                    <asp:Repeater ID="rptPager" runat="server">
                        <ItemTemplate>
                            <li>
                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                    CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                    OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>

                    </asp:Repeater>
                </ul>
            </div>
        </div>
    </div>

    <div class="modal fade" id="RodPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; height: 750px;" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="background: white; height: 500px">
            <div class="modal-content">
                <div class="modal-header" style="display: inline-block; width: 100%;">
                    Select ROD 
                                        
                                        <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                </div>
                <div class="modal-body">

                    <div id="signature-pad" class="signature-pad" style="height: 500px">
                        <div class="signature-pad--body" style="height: 500px">
                            <canvas></canvas>
                        </div>
                        <div class="signature-pad--footer">
                            <div class="description">Sign above</div>

                            <div class="signature-pad--actions">
                                <div>
                                    <button type="button" class="button clear" data-action="clear">Clear</button>
                                    <%--  <button type="button" class="button" data-action="change-color">Change color</button>
                                    <button type="button" class="button" data-action="undo">Undo</button>--%>
                                </div>
                                <%-- <div>
                                    <button type="button" class="button save" data-action="save-png">Save as PNG</button>
                                    <button type="button" class="button save" data-action="save-jpg">Save as JPG</button>
                                    <button type="button" class="button save" data-action="save-svg">Save as SVG</button>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                    <button id="btnClear" type="button" class="btn btn-danger" onclick="saveSign()">Save</button>

                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>


            </div>
        </div>
    </div>
    <div style="display: none">
        <asp:Button runat="server" ID="btnSaveSign" OnClick="btnSaveSign_Click" />
    </div>
    <input type="hidden" id="hidBlob" />
    <input type="hidden" id="patientIEID" />
    <asp:HiddenField runat="server" ID="patientIEIDServer" />
    <asp:HiddenField runat="server" ID="hidBlobServer" />

    <script src="Scripts/jquery-1.8.2.js"></script>
    <script src="Scripts/jquery-ui-1.8.24.js"></script>
    <link href="Style/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">



        $.noConflict();
        function openModelPopup(_patientIEID) {


            $('#RodPopup').modal('show');
            $("#<%= patientIEIDServer.ClientID%>").val(_patientIEID);
            resizeCanvas();
        }

        function closeModelPopup() {
            $('#RodPopup').modal('hide');
        }
        function saveSign() {
            download();

            $("#<%= hidBlobServer.ClientID%>").val($('#hidBlob').val());



            document.getElementById("<%= btnSaveSign.ClientID %>").click();


            <%--var ajaxdata = "{_patientIEID:'"+ $('#patientIEID').val() +"',hidblob:'"+ $('#hidBlob').val() +"'}";

             $.ajax({
                type: "POST",
                url: "<%=  ResolveUrl("~/Templatestorepdf.aspx/saveSignature") %>",
                data: ajaxdata,
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) {
                    
                },
                failure: function (response) {
                    alert("Invalid Details...");
                }
            });--%>

        }
    </script>
    <script src="js/signature_pad.umd_org.js"></script>
    <script src="js/app_org.js"></script>

</asp:Content>

