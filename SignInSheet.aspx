<%@ Page Title="PainTrax-SignInSheet" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="SignInSheet.aspx.cs" Inherits="SignInSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
        CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <script src="https://raw.githubusercontent.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%-- <script src="https://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery.maskedinput.js"></script>


    <%--this is starting for the popup--%>
    <%-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>--%>

    <script>
        function popupCenter(url, title, w, h) {
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            return window.open(url, title, 'toolbar=no,fullscreen=no,border=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

    </script>





    <%--this is Ending  for the popup--%>



    <script type="text/javascript">
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }


        $(document).ready(function () {

            $('[id*=txt_mobile]').mask("999-999-9999")
            $('[id*=txt_home_ph]').mask("999-999-9999")
            $('[id*=txt_work_ph]').mask("999-999-9999")

            $('[id*=txt_attorney_ph]').mask("999-999-9999")
            $('[id*=txt_SSN]').mask("999-99-9999")

            $('[id*=txt_DOB]').mask("99/99/9999")

            $('[id*=txt_Date]').datepicker();

            $('[id*=txt_DOA]').datepicker();

            $('[id*=txt_fname]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getFirstName") %>',
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

            $('[id*=txt_lname]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getLastName") %>',
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

            $('[id*=txt_ins_co]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getInsComp") %>',
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
                    $('[id*=hfinccmp]').val(map[item].id);
                    return item;
                }
            });

            $('[id*=txt_pharmacy]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
    , source: function (request, response) {
        $.ajax({
            url: '<%=ResolveUrl("~/Page1.aspx/getPharmacy") %>',
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
                    $('[id*=hpharmacy]').val(map[item].id);
                    return item;
                }
            });

            $('[id*=txt_attorney]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
           , source: function (request, response) {
               $.ajax({
                   url: '<%=ResolveUrl("~/Page1.aspx/getAttorney") %>',
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
                    $('[id*=hattorney]').val(map[item].id);
                    return item;
                }
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                $('[id*=txt_mobile]').mask("999-999-9999")
                $('[id*=txt_home_ph]').mask("999-999-9999")
                $('[id*=txt_work_ph]').mask("999-999-9999")

                $('[id*=txt_attorney_ph]').mask("999-999-9999")
                $('[id*=txt_SSN]').mask("999-99-9999")

                $('[id*=txt_DOB]').mask("99/99/9999")

                $('[id*=txt_Date]').datepicker();
                //$('[id*=txt_DOB]').datepicker();
                $('[id*=txt_DOA]').datepicker();

                $('[id*=txt_fname]').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                 , source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/Page1.aspx/getFirstName") %>',
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

             $('[id*=txt_lname]').typeahead({
                 hint: true,
                 highlight: true,
                 minLength: 1
                 , source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/Page1.aspx/getLastName") %>',
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


             $('[id*=txt_ins_co]').typeahead({
                 hint: true,
                 highlight: true,
                 minLength: 1
             , source: function (request, response) {
                 $.ajax({
                     url: '<%=ResolveUrl("~/Page1.aspx/getInsComp") %>',
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
                     $('[id*=hfinccmp]').val(map[item].id);
                     return item;
                 }
             });

         $('[id*=txt_pharmacy]').typeahead({
             hint: true,
             highlight: true,
             minLength: 1
         , source: function (request, response) {
             $.ajax({
                 url: '<%=ResolveUrl("~/Page1.aspx/getPharmacy") %>',
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
                 $('[id*=hpharmacy]').val(map[item].id);
                 return item;
             }
         });

     $('[id*=txt_attorney]').typeahead({
         hint: true,
         highlight: true,
         minLength: 1
, source: function (request, response) {
    $.ajax({
        url: '<%=ResolveUrl("~/Page1.aspx/getAttorney") %>',
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
             $('[id*=hattorney]').val(map[item].id);
             return item;
         }
     });
}
        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">


    <div class="form-horizontal">
        <div class="control-group span3">
            <label class="control-label">Date*</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_Date"></asp:TextBox>

            </div>
        </div>
        <div class="control-group span3">
            <label class="control-label">Location*</label>
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
            <label class="control-label" runat="server" id="lbl_msg"></label>

        </div>
        <div class="control-group span3">
            <asp:Button ID="btn_search" runat="server" OnClick="btn_search_Click" Text="Search" />
        </div>
    </div>
    <div style="clear: both">
    </div>

    <div class="table-responsive">
        <asp:Repeater ID="rpview" runat="server" OnItemDataBound="OnItemDataBound">
            <HeaderTemplate>
                <table class="table table-striped table-bordered" style="width: 100%">
                    <thead>
                        <tr>
                            <th style="width: 10%">Name</th>
                            <th style="width: 10%">Signature</th>
                            <th style="width: 70%">Procedure</th>
                            <%--  <th>Edit</th>--%>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("LastName")+" "+Eval("FirstName")  %><br />
                        <%# Eval("DOA","{0:dd-MM-yyyy}") %><br />
                        <%# Eval("Compensation") %><br />
                        <%# Eval("tablename") %>
                    </td>
                    <td></td>
                    <td>
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:DropDownList runat="server" ID="ddl_select" Width="190px" OnTextChanged="ddl_select_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:GridView ID="gvneck" runat="server"></asp:GridView>
                            <%-- <asp:Button ID="btnclick" Text="test1" runat="server" OnClick="btnclick_Click" />--%>
                            <asp:GridView ID="gvmidback" runat="server"></asp:GridView>
                            <asp:GridView ID="gvlowback" runat="server"></asp:GridView>
                            <asp:GridView ID="gvshoulder" runat="server"></asp:GridView>
                            <asp:GridView ID="gvknee" runat="server"></asp:GridView>
                            <asp:GridView ID="gvelbow" runat="server"></asp:GridView>
                            <asp:GridView ID="gvwrist" runat="server"></asp:GridView>
                            <asp:GridView ID="gvhip" runat="server"></asp:GridView>
                            <asp:GridView ID="gvankle" runat="server"></asp:GridView>
                        </asp:Panel>
                        <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# Eval("PatientIE_ID") %>' />
                        <asp:HiddenField ID="hfCustomerId1" runat="server" Value='<%# Eval("PatientFU_ID") %>' />
                    </td>
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

</asp:Content>

