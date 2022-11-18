<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCGeneralDocuments.ascx.cs" Inherits="UserControl_UCGeneralDocuments" %>
<script>
    <%--   $(document).ready(function () {
        $("#btnMove").click(function () {
              var filename = $(this).data('file-name');
            $("#<%=txtFileName.ClientID%>").val(filename);
            $('#myModal').modal('show');
        });
   });
 --%>
    function move(filename) {
        //  var filename = $(this).data('file-name');
        $("#<%=txtFileName.ClientID%>").val(filename);
        $('#myModal').modal('show');
    };

//    $('#my_modal').on('show.bs.modal', function(e) {
//        alert("call");
//    //get data-id attribute of the clicked element
//    var filename = $(e.relatedTarget).data('file-name');
//        alert("filename");
//    //populate the textbox
//    $(e.currentTarget).find('input[name="txtFileName"]').val(filename);
//});
</script>


<style>
    .select, .select:hover {
        background-color: gainsboro;
        text-decoration: none;
        font-size: 15px !important;
    }

    .leafNode, .leafNode:hover, .rootNode, .rootNode:hover {
        text-decoration: none;
        font-size: 15px !important;
    }

    .treeV {
        vertical-align: top;
        font-size: 15px !important;
    }

    .contextMenu {
        position: absolute;
        width: 120px;
        z-index: 99999;
        border: solid 1px #CCC;
        background: #EEE;
        padding: 0px;
        margin: 0px;
        display: none;
    }

        .contextMenu LI {
            list-style: none;
            padding: 0px;
            margin: 0px;
        }

        .contextMenu A {
            color: #333;
            text-decoration: none;
            display: block;
            line-height: 20px;
            height: 20px;
            background-position: 6px center;
            background-repeat: no-repeat;
            outline: none;
            padding: 1px 5px;
            padding-left: 28px;
        }

        .contextMenu LI.hover A {
            color: #FFF;
            background-color: #3399FF;
        }

        .contextMenu LI.disabled A {
            color: #AAA;
            cursor: default;
        }

        .contextMenu LI.hover.disabled A {
            background-color: transparent;
        }

        .contextMenu LI.separator {
            border-top: solid 1px #CCC;
        }
</style>
<script src="../js/jquery.contextMenu.js" type="text/javascript"></script>

<div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Select Folder</h4>
            </div>
            <div class="modal-body">
                <p>
                    <asp:HiddenField ID="txtFileName" runat="server" />
                    <%--                        <asp:TextBox ID="txtFileName"  Font-Names="txtFileName" runat="server"></asp:TextBox></p>    --%>
                    <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                    <br />
                    <asp:Button ID="btnTransfer" runat="server" Text="Transfer" CssClass="btn btn-success btn-sm" OnClick="btnTransfer_Click" />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>

    </div>
</div>

<%-- 22-11-2021 --%>
<div class="modal fade" id="dirModal" role="dialog">
    <div class="modal-dialog">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Rename Folder</h4>
            </div>
            <div class="modal-body">
                <p>
                    Old Folder Name :
                       <asp:TextBox ID="txtOldFolderName" runat="server" CssClass="form-control"></asp:TextBox>
                    New Folder Name :
                       <asp:TextBox ID="txtNewFolderName" runat="server" CssClass="form-control"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="Rename" CssClass="btn btn-success btn-sm" OnClick="btnRename_Click" />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>

    </div>
</div>

<%-- Over --%>


<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
<table style="width: 100%; min-height: 500px">
    <tr>
        <td style="width: 30%; vertical-align: top; border-right: 2px solid #79b0ce">
            <div style="vertical-align: top;">


                <asp:TreeView NodeStyle-CssClass="treeNode" ValidateRequestMode="Disabled"
                    RootNodeStyle-CssClass="rootNode" SelectedNodeStyle-CssClass="select"
                    LeafNodeStyle-CssClass="leafNode" runat="server" ID="treeView" OnSelectedNodeChanged="treeView_SelectedNodeChanged" Style="vertical-align: top">
                </asp:TreeView>
            </div>
        </td>
        <td style="width: 70%; vertical-align: top">

            <div style="margin-left: 50px; margin-right: 50px; vertical-align: top" runat="server" id="div_form">


                <div class="row">
                    <div class="col-sm-4">
                        <h3>Document Upload</h3>
                    </div>
                    <div class="col-sm-7">
                        <h3 runat="server" ID="lbl_name"></h3>
                    </div>

                </div>
                <div class="clearfix"></div>
                <hr />


                <div class="row">



                    <div class="col-sm-6 inline">
                        <label class="lblstyle">Select Documents</label>
                    </div>
                    <div class="col-sm-6 inline">
                        <asp:FileUpload runat="server" ID="fup" AllowMultiple="true" />
                        <i>(Max. file size 5MB.)</i>
                    </div>

                </div>



                <div>
                    <div class="row">

                        <div class="col-sm-2">
                            <label class="lblstyle">&nbsp;</label>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />

                            </div>
                        </div>

                    </div>


                </div>

            </div>

            <br />

            <div style="margin-left: 50px; margin-right: 50px">
                <br />
                <h3>
                    <asp:Label ID="lblPath" runat="server" Text=""></asp:Label></h3>
                <%-- <br />
                    <asp:ListBox ID="lstBox" runat="server" Style="height: 800px; width: 100%"></asp:ListBox>--%>
                <br />
                <asp:GridView ID="gvDocument" BorderStyle="None" CssClass="table table-bordered" EmptyDataText="Sorry !! No Document found" AutoGenerateColumns="false" Width="100%" runat="server">
                    <Columns>

                        <%-- <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="80%" />--%>
                        <asp:TemplateField>

                            <ItemTemplate>

                                <table width="100%">
                                    <tr>
                                        <td style="width: 80%">
                                            <a style="width: 80%" href='<%# "https://docs.google.com/viewer?url=https://www.paintrax.com/KDVORTHO"+Eval("PreviewPath").ToString().Replace("~","")+"&embedded=true" %>' target="_new" title="Preview">
                                                <%# Eval("Name") %>
                                            </a>
                                        </td>
                                        <td>
                                            <asp:ImageButton ImageUrl="~/images/delete.png" Height="20" Width="20" ToolTip="Remove" runat="server" ID="btnDelete" CommandArgument='<%# Eval("path") %>' OnClientClick="return confirm('Are you sure you want to delete this document ?')" Text="<i class='fas fa-trash-alt'></i> delete"
                                                OnClick="btnDelete_Click" />
                                            &nbsp;

                                <asp:ImageButton runat="server" ID="btnDownload" ToolTip="Download" CommandArgument='<%# Eval("path")+"#"+Eval("Name") %>' ImageUrl="~/images/download.png" Height="20" Width="20" CausesValidation="false" OnClick="btnDownload_Click" />
                                            &nbsp;
                                <%--  <asp:Button runat="server" ID="btnPreview" CommandArgument='<%# Eval("Document_ID") %>' CssClass="btn btn-info" Text="Preview" CausesValidation="false" OnClick="btnPreview_Click" />--%>
                                            <a style="display: none" href='<%# "https://docs.google.com/viewer?url=https://www.paintrax.com/KDVORTHO"+Eval("PreviewPath").ToString().Replace("~","")+"&embedded=true" %>' target="_new" title="Preview">
                                                <img src="images/preview.png" style="height: 18px; width: 25px; vertical-align: top!important" />
                                            </a>
                                            <a id="btnMove" data-toggle="modal" data-file-name='<%# Eval("path") %>' onclick="move('<%# Eval("path").ToString().Replace("\\","/") %>');" title="Move To">
                                                <img src="images/Move.png" style="height: 24px; width: 24px; vertical-align: top!important" />
                                            </a>
                                        </td>
                                    </tr>
                                </table>

                                <%--                                <asp:Button runat="server" ID="btnDelete" CommandArgument='<%# Eval("path") %>' CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this document ?')" Text="Delete" CausesValidation="false" OnClick="btnDelete_Click" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </td>
    </tr>
</table>
<ul id="myMenu" class="contextMenu">
    <li class="copy"><a href="#add">Rename</a></li>
</ul>

<script>
    $(document).ready(function () {

        $("input[id$='txtOldFolderName']").attr('readonly', true);
        $(".rootNode A").contextMenu({
            menu: 'myMenu'
        }, function (action, el, pos) {
            //alert(
            //	'Action: ' + action + '\n\n' +
            //	'Element text: ' + $(el).text() + '\n\n'  +$("#dirModal")
            //                );
            // alert($(el).attr("href"));
            $("input[id$='txtOldFolderName']").val($(el).text().slice(0, -6));
            $("input[id$='txtNewFolderName']").val($(el).text().slice(0, -6));
            $('#dirModal').modal('show');
        });
    });

</script>


<%-- </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvDocument" />
    </Triggers>
</asp:UpdatePanel>--%>
