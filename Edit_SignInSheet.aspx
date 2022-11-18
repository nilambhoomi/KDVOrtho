<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_SignInSheet.aspx.cs" Inherits="test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update PainTrax </title>
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet">
    <link href="css/style.css" rel="stylesheet">
    <link href="css/style-responsive.css" rel="stylesheet">


    <link href='https://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,800italic,400,300,600,700,800&subset=latin,cyrillic-ext,latin-ext' rel='stylesheet' type='text/css'>
    <!-- end: CSS -->
    <script src="https://raw.githubusercontent.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%-- <script src="https://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
   <%-- +'&Location='<%# HttpContext.Current.Session["location"].ToString() %>--%>
    <script type="text/javascript">
        function close_window() {
            window.opener.location ='SignInSheet.aspx?IN='<%#HttpContext.Current.Session["IN"].ToString()%>;
            window.close();
        }
    </script>


   


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

            $('[id*=txtDate1]').datepicker();
            $('[id*=txtDate2]').datepicker();
            $('[id*=txtDate3]').datepicker();

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
                            $(".dropdown-menu").css("height", "500px");
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

                $('[id*=txtDate1]').datepicker();
                $('[id*=txtDate2]').datepicker();
                $('[id*=txtDate3]').datepicker();
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
                         $(".dropdown-menu").css("height", "500px");
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

</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <div>
                Mcode&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtmcode" runat="server" ReadOnly="true">
                </asp:TextBox><br />
            </div>
            <div>
                Request&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDate1" runat="server"></asp:TextBox><br />
            </div>
            <div>
                Scheduled&nbsp;<asp:TextBox ID="txtDate2" runat="server"></asp:TextBox><br />
            </div>
            <div>
                Executed&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDate3" runat="server"></asp:TextBox><br />
                <br />
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btn_update" runat="server" OnClick="btn_update_Click" Text="Update" />
        </div>
    </form>
</body>
</html>
