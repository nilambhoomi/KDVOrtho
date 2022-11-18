<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="NFDisability.aspx.cs" Inherits="NFDisability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <%-- <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
        CancelControlID="btnCancel" BackgroundCssClass="modalBackground">--%>
    </cc1:ModalPopupExtender>
    <script src="https://raw.githubusercontent.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%-- <script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
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

    <center><h3>NOTICE AND PROOF OF CLAIM FOR DISABILITY BENEFITS</h3></center>
    <div style="clear: both"></div>
    <label>Page-1</label>
    <div class="form-horizontal">
        <div class="control-group span3">
            <label class="control-label">Last Name: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_lname" AutoCompleteType="None"></asp:TextBox>
                <%--OnTextChanged="txt_lname_TextChanged" AutoPostBack="true" --%>
            </div>
        </div>
        <div class="control-group span3">
            <label class="control-label">First Name: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_fname" AutoCompleteType="None"></asp:TextBox>
                <%--OnTextChanged="txt_fname_TextChanged" AutoPostBack="true"--%>
            </div>
        </div>
        <div class="control-group span3">
            <label class="control-label">SSN: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_SSN" placeholder="xxx-xx-xxxx" TabIndex="10"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="reg1" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{2}[\s.-]\d{4}$" ControlToValidate="txt_SSN" Display="Dynamic" ErrorMessage="invalid format for SSN."></asp:RegularExpressionValidator>
            </div>
        </div>
    </div>
    <div style="clear: both"></div>
    <div class="form-horizontal">
        <div class="control-group span3">
            <label class="control-label">Address: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_add"></asp:TextBox>
            </div>
        </div>
        <div class="control-group span3">
            <label class="control-label">Mobile: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_mobile" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_mobile" Display="Dynamic" ErrorMessage="invalid format for mobile."></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="control-group span3">
            <label class="control-label">DOB: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_DOB" Width="120px"></asp:TextBox>
                <%-- <asp:RegularExpressionValidator runat="server" ID="reg11" ValidationExpression="/^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/" ErrorMessage="Enter date in MM/dd/yyyy" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_DOB"></asp:RegularExpressionValidator>--%>
            </div>
        </div>
    </div>
    <div style="clear: both"></div>
    <div class="form-horizontal">
        <div class="control-group span3">
            <label class="control-label">Claim signed on</label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_sign"></asp:TextBox>
            </div>
        </div>

    </div>
    <div style="clear: both"></div>
    <center><h3>NOTICE AND PROOF OF CLAIM FOR DISABILITY BENEFITS</h3></center>
    <label>Page-2</label>
    <div class="form-horizontal">
        <div class="control-group span3">
            <label class="control-label">Last Name: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_lname2" AutoCompleteType="None"></asp:TextBox>
                <%--OnTextChanged="txt_lname_TextChanged" AutoPostBack="true" --%>
            </div>
        </div>
        <div class="control-group span3">
            <label class="control-label">First Name: </label>
            <div class="controls">
                <asp:TextBox runat="server" ID="txt_fname2" AutoCompleteType="None"></asp:TextBox>
                <%--OnTextChanged="txt_fname_TextChanged" AutoPostBack="true"--%>
            </div>
        </div>

        <div class="control-group span3">
            <label class="control-label">Sex: </label>
            <div class="controls">
                <asp:DropDownList runat="server" ID="ddl_gender" Width="90px" TabIndex="7">
                    <asp:ListItem Value="Mr." Text="M"></asp:ListItem>
                    <asp:ListItem Value="Ms." Text="F"></asp:ListItem>

                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div style="clear: both"></div>
    <div class="form-horizontal" style="height: 250px;">
        <div class="controls">
            <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnExportPDF_Click" CssClass="btn btn-primary" UseSubmitBehavior="False" />
            <asp:Button runat="server" ID="Button1" PostBackUrl="~/FormDetails.aspx" Text="Back to List" CssClass="btn btn-default" UseSubmitBehavior="False" TabIndex="27" />
        </div>
        <div class="controls">
            <asp:Label runat="server" ID="lblmess"></asp:Label>
        </div>
    </div>

</asp:Content>

