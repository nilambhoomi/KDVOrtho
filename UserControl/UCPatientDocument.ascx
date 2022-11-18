<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCPatientDocument.ascx.cs" Inherits="UserControl_UCPatientDocument" %>
<script type="text/javascript">
    function Confirmbox(e, page) {
        e.preventDefault();
        var answer = confirm('Do you want to save the data?');
        if (answer) {
            //var currentURL = window.location.href;
            <%--document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');--%>
            // document.getElementById('<%= btnSave.ClientID %>').click();
            window.location.href = $('#ctl00_' + page).attr('href');
        }
        else {
            window.location.href = $('#ctl00_' + page).attr('href');
        }
    }
    <%-- function saveall() {
        document.getElementById('<%= btnSave.ClientID %>').click();
    }--%>
</script>


<%--  <asp:UpdatePanel runat="server" ID="upmain">
        <ContentTemplate>--%>

<div class="container">

    <div class="row">
        <h3>Document Upload</h3>
        <hr />
    </div>


    <div class="row">



        <div class="col-sm-6 inline">
            <label class="lblstyle">Select Documents</label>
        </div>
        <div class="col-sm-6 inline">
            <asp:FileUpload runat="server" ID="fup" AllowMultiple="true" />
            <i>(Max. file size 5MB.)</i>
        </div>

    </div>



    <div class="col-xs-12">
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


<div class="row">
    <div class="col-12">
        <asp:HiddenField ID="pageHDN" runat="server" />
        <div class="container">
            <div class="row">
                <asp:GridView ID="gvDocument" BorderStyle="None" CssClass="table table-bordered" AutoGenerateColumns="false" Width="100%" runat="server">
                    <Columns>
                        <asp:BoundField DataField="UploadDate" HeaderText="Date" DataFormatString="{0:d}" Visible="false" />
                        <asp:BoundField DataField="DocName" HeaderText="Name" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnDelete" CommandArgument='<%# Eval("Document_ID") %>' CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this document ?')" Text="Delete" CausesValidation="false" OnClick="btnDelete_Click" />
                                 <asp:Button runat="server" ID="Button1" CommandArgument='<%# Eval("path")+"#"+Eval("DocName") %>' CssClass="btn btn-warning" Text="Download" CausesValidation="false" OnClick="btnDownload_Click" />
                                <%--  <asp:Button runat="server" ID="btnPreview" CommandArgument='<%# Eval("Document_ID") %>' CssClass="btn btn-info" Text="Preview" CausesValidation="false" OnClick="btnPreview_Click" />--%>
                                <a href='<%# "https://docs.google.com/viewer?url=https://www.paintrax.com/KDVORTHO"+Eval("path").ToString().Replace("~","")+"&embedded=true" %>' target="_new" class="btn btn-info">Preview </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>


<div class="modal fade" id="PreviewPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="background: white">
        <div class="modal-content">
            <div class="modal-header" style="display: inline-block; width: 100%;">
                Document Preview
                                       
                    <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
            </div>
            <div class="modal-body">
                <iframe width="960px" height="750px" runat="server" id="iframeDocument"></iframe>
            </div>
        </div>
    </div>
</div>
<%--   </ContentTemplate>
        <Triggers>

            <asp:PostBackTrigger ControlID="btnSave" />

        </Triggers>
    </asp:UpdatePanel>--%>
<script src="Scripts/jquery-ui-1.8.24.js"></script>
<link href="Style/jquery-ui.css" rel="stylesheet" />
<script>
    $.noConflict();
    function openPopup() {
        $('#PreviewPopup').modal('show');
    }

    function closeModelPopup() {
        //jQuery.noConflict();
        //(function ($) {

        $('#PreviewPopup').modal('hide');

        //})(jQuery);
    }
</script>
