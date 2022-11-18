<%@ Page Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Page5.aspx.cs" Inherits="Page5" %>


<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>

    <script src="js/images/bootstrap.min.js"></script>

    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery-ui-timepicker-addon.js"></script>



    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
    <%--  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.37/css/bootstrap-datetimepicker.min.css" />--%>
    <%--   
    <link rel="stylesheet" href="assets/css/datepicker.css" />--%>
    <%--<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>--%>
    <%----%>
    <%--<link href="assets/css/jquery-ui.css" rel="stylesheet" />--%>
    <%--<script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <%--<script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>--%>
    <%--<script src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>--%>
    <%--<script src="http://code.jquery.com/ui/1.10.2/jquery---%>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                document.getElementById('<%= btnSave.ClientID %>').click();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
    <script>
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
        $(function () {
            if (!$.fn.bootstrapDP && $.fn.datepicker && $.fn.datepicker.noConflict) {
                var datepicker = $.fn.datepicker.noConflict();
                $.fn.bootstrapDP = datepicker;
            }

        });
        $(document).ready(function ($) {
            var $j = jQuery.noConflict();;


            //$(function () {
            //    var $j = jQuery.noConflict();;
            //    if (!$.fn.bootstrapDP && $.fn.datepicker && $.fn.datepicker.noConflict) {
            //        var datepicker = $.fn.datepicker.noConflict();
            //        $.fn.bootstrapDP = datepicker;
            //    }
            $('#<%=dtpDiagCervialBulgeDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpDiagThoracicBulgeDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpDiagLumberBulgeDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpDiagLeftShoulderDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpDiagRightShoulderDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpDiagLeftKneeDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpDiagRightKneeDate.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther1Date.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther2Date.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther3Date.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther4Date.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther5Date.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther6Date.ClientID%>').mask("99/99/9999");
            $('#<%=dtpOther7Date.ClientID%>').mask("99/99/9999");
            $('#<%=txtFollowUpInDate.ClientID%>').mask("99/99/9999");
            $j('#<%=txtFollowUpInDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            <%--$j('#<%=dtpDiagThoracicBulgeDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpDiagLumberBulgeDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpDiagLeftShoulderDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpDiagRightShoulderDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpDiagLeftKneeDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });

            $j('#<%=dtpDiagRightKneeDate.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
          
            $j('#<%=dtpOther1Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpOther2Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpOther3Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpOther4Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpOther5Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpOther6Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=dtpOther7Date.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            })--%>;
        });

    </script>

    <style>
        /*.table{
    display:table;
    width:100%;
    table-layout:fixed;
}*/
        .table_cell {
            /*display:table-cell;*/
            width: 100px;
            /*border:solid black 1px;*/
        }

        .labelcolor {
            color: black;
            font-weight: bold;
            font-size: 16px;
        }
    </style>
    <div id="mymodelmessage" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMessage" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label runat="server" id="lblMessage"></label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- start: Header -->
    <%--    <asp:UpdatePanel runat="server" ID="upMain">
            <ContentTemplate>--%>
    <div class="container">
        <div class="row">
            <div class="col-lg-10" id="content">

                <%--  <div class="row" style="display:none">
                    <div class="col-md-3">
                        <label class="control-label labelcolor">GAIT:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:DropDownList Style="" DataSourceID="cboGAITDS" Width="180px" DataTextField="name" ID="cboGAIT" runat="server"></asp:DropDownList>
                        <asp:XmlDataSource ID="cboGAITDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/GAITs/GAIT" />
                        <asp:DropDownList Style="" Width="280px" DataSourceID="cboAmbulatesDS" DataTextField="name" ID="cboAmbulates" runat="server"></asp:DropDownList>
                        <asp:XmlDataSource ID="cboAmbulatesDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Ambulatess/Ambulates" />
                        <asp:CheckBox ID="chkFootslap" runat="server" Text="Foot slap/drop " />
                        <asp:CheckBox ID="chkKneehyperextension" runat="server" Text="knee hyperextension  " />
                        <asp:CheckBox ID="chkUnabletohealwalk" runat="server" Text="unable to heel walk  " />
                        <asp:CheckBox ID="chkUnabletotoewalk" runat="server" Text="unable to toe walk  " />
                        <label class="control-label">and </label>
                        <asp:TextBox ID="txtOther" Width="350px" runat="server"></asp:TextBox>.
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label labelcolor">DIAGNOSTIC STUDIES:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" Width="80px" ID="dtpDiagCervialBulgeDate" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Width="130px" Style="" DataSourceID="cboDiagCervialBulgeStudyDS" DataTextField="name" ID="cboDiagCervialBulgeStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagCervialBulgeStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>

                                    <asp:CheckBox ID="chkDiagCervialBulge" runat="server" Visible="false" />
                                    <%--<label class="control-label">Cervical bulge</label>--%>
                                    <asp:TextBox Width="350px" placeholder="Cervical bulge" ID="txtDiagCervialBulgeText" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <label class="control-label">HNP</label>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="350px" ID="txtDiagCervialBulgeHNP1" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="350px" TextMode="MultiLine" ID="txtDiagCervialBulgeHNP2" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" Width="80px" ID="dtpDiagThoracicBulgeDate" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboDiagThoracicBulgeStudyDS" DataTextField="name" ID="cboDiagThoracicBulgeStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagThoracicBulgeStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>

                                    <asp:CheckBox ID="chkDiagThoracicBulge" runat="server" Visible="false" />
                                    <%-- <label class="control-label">Thoracic bulge</label>--%>
                                    <asp:TextBox Width="350px" placeholder="Thoracic bulge" ID="txtDiagThoracicBulgeText" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <label class="control-label">HNP</label>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="350px" ID="txtDiagThoracicBulgeHNP1" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="350px" TextMode="MultiLine" ID="txtDiagThoracicBulgeHNP2" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpDiagLumberBulgeDate" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboDiagLumberBulgeStudyDS" DataTextField="name" ID="cboDiagLumberBulgeStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagLumberBulgeStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDiagLumberBulge" runat="server" Visible="false" />
                                    <%--<label class="control-label">Lumbar bulge</label>--%>
                                    <asp:TextBox Width="350px" placeholder="Lumbar bulge" ID="txtDiagLumberBulgeText" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <label class="control-label">HNP</label>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="350px" ID="txtDiagLumberBulgeHNP1" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox Width="350px" TextMode="MultiLine" ID="txtDiagLumberBulgeHNP2" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpDiagLeftShoulderDate" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboDiagLeftShoulderStudyDS" DataTextField="name" ID="cboDiagLeftShoulderStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagLeftShoulderStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDiagLeftShoulder" runat="server" Visible="false" />
                                    <%--<label class="control-label">Left Shoulder</label>--%>
                                    <asp:TextBox Width="350px" TextMode="MultiLine" placeholder="Left Shoulder" ID="txtDiagLeftShoulderText" runat="server"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpDiagRightShoulderDate" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboDiagRightShoulderStudyDS" DataTextField="name" ID="cboDiagRightShoulderStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagRightShoulderStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDiagRightShoulder" runat="server" Visible="false" />
                                    <%--<label class="control-label">Right Shoulder</label>--%>
                                    <asp:TextBox Width="350px" TextMode="MultiLine" placeholder="Right Shoulder" ID="txtDiagRightShoulderText" runat="server"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpDiagLeftKneeDate" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboDiagLeftKneeStudyDS" DataTextField="name" ID="cboDiagLeftKneeStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagLeftKneeStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDiagLeftKnee" runat="server" Visible="false" />
                                    <%--<label class="control-label">Left knee</label>--%>
                                    <asp:TextBox Width="350px" TextMode="MultiLine" placeholder="Left knee" ID="txtDiagLeftKneeText" runat="server"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpDiagRightKneeDate" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboDiagRightKneeStudyDS" DataTextField="name" ID="cboDiagRightKneeStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagRightKneeStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDiagRightKnee" runat="server" Visible="false" />
                                    <%--<label class="control-label">Right knee</label>--%>
                                    <asp:TextBox Width="350px" TextMode="MultiLine" placeholder="Right knee" ID="txtDiagRightKneeText" runat="server"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <%-- <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpDiagBrainDate"  width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" DataSourceID="cboDiagBrainStudyDS" DataTextField="name" ID="cboDiagBrainStudy" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboDiagBrainStudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Scans/Scan" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDiagBrain" Text="Brain" runat="server" Visible="false" />
                                    <asp:TextBox placeholder="Brain" ID="txtDiagBrainText" runat="server"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label labelcolor">OTHER:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther1Date" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther1StudyDS" DataTextField="name" ID="cboOther1Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther1StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther1Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther2Date" AutoPostBack="true" Width="80px" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther2StudyDS" DataTextField="name" ID="cboOther2Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther2StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther2Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther3Date" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther3StudyDS" DataTextField="name" ID="cboOther3Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther3StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther3Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther4Date" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther4StudyDS" DataTextField="name" ID="cboOther4Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther4StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther4Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther5Date" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther5StudyDS" DataTextField="name" ID="cboOther5Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther5StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther5Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther6Date" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther6StudyDS" DataTextField="name" ID="cboOther6Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther6StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther6Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="dtpOther7Date" Width="80px" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" Width="130px" DataSourceID="cboOther7StudyDS" DataTextField="name" ID="cboOther7Study" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="cboOther7StudyDS" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox Width="900px" TextMode="MultiLine" ID="txtOther7Text" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <asp:TextBox runat="server" ID="TextBox13" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList Style="" DataSourceID="cboOther1StudyDS" DataTextField="name" ID="DropDownList7" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="XmlDataSource7" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/ScanOthers/ScanOther" />
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upMedicine" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label labelcolor">Other Medicine(s):</label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                                <asp:TextBox runat="server" Style="" ID="txtOtherMedicine" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                <asp:ImageButton ID="AddDrug" Style="text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDrug_Click" />

                                <asp:GridView ID="dgvMedi" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                    <Columns>
                                        <%-- <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Medicine" ItemStyle-Width="450">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("MedicationID") %>' />
                                                <asp:TextBox ID="txtpe" runat="server" CssClass="form-control" Width="450px" TextMode="MultiLine" Text='<%# Eval("Medicine") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="50">
                                            <ItemTemplate>
                                                <%--    <asp:HiddenField runat="server" ID="hidDiagCodeDetailID" Value='<%# Eval("DiagCodeDetail_ID") %>' />--%>
                                                <asp:CheckBox runat="server" ID="chkRemove" Checked="true" />

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div style="display: none">

                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label labelcolor">PROCEDURE:</label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:CheckBox ID="chkProcedures" runat="server" Checked="true" Text="If the patient continues to have tender palpable taut bands/trigger points" />
                            <label class="control-label">with referral patterns as noted in the future on examination,</label>
                            <label class="control-label">I will consider doing trigger point injections.</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label labelcolor">CARE:</label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:CheckBox ID="chkAcupuncture" runat="server" Checked="true" Text="Acupuncture " />
                            <asp:CheckBox ID="chkChiropratic" runat="server" Checked="true" Text="chiropractic " />
                            <asp:CheckBox ID="chkPhysicalTherapy" runat="server" Checked="true" Text="physical therapy " />
                            <asp:CheckBox ID="chkAvoidHeavyLifting" runat="server" Checked="true" Text="avoid heavy lifting " />
                            <asp:CheckBox ID="chkCarrying" runat="server" Checked="true" Text="carrying " />
                            <asp:CheckBox ID="chkExcessiveBend" runat="server" Checked="true" Text="excessive bending " />
                            <asp:CheckBox ID="chkProlongedSitStand" runat="server" Checked="true" Text="prolonged sitting or standing  " />
                            <label class="control-label">and </label>
                            <asp:TextBox ID="txtCareOther" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label labelcolor">GOALS:</label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <label class="control-label">To increase range of motion, strength, flexibility, to decrease pain and to improve body biomechanics and activities of daily living and improve the functional status.</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label labelcolor">PRECAUTIONS :</label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <label class="control-label">Universal  </label>
                            <asp:CheckBox ID="chkCardiac" Text="cardiac  " runat="server" />
                            <asp:CheckBox ID="chkWeightBearing" Text="weight bearing precaution and " runat="server" />
                            <asp:TextBox ID="txtPrecautions" Width="300" runat="server" />.
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:CheckBox ID="chkEducationProvided" Text="Patient education provided via  " runat="server" />
                            <asp:CheckBox ID="chkViaPhysician" Text="physician  " runat="server" />
                            <asp:CheckBox ID="chkViaPrintedMaterial" Text="printed material  " runat="server" />
                            <asp:CheckBox ID="chkViaWebsite" Text="online website references  " runat="server" />
                            <asp:CheckBox ID="chkIsViaVideo" Text="video about  " runat="server" />
                            <asp:TextBox ID="txtViaVideo" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label labelcolor">CONSULTATION :</label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:CheckBox ID="chkConsultNeuro" Text="Neurologist " runat="server" />
                            <asp:CheckBox ID="chkConsultOrtho" Text="orthopedist  " runat="server" />
                            <asp:CheckBox ID="chkConsultPsych" Text="psychiatrist  " runat="server" />
                            <asp:CheckBox ID="chkConsultPodiatrist" Text="podiatrist and " runat="server" />
                            <asp:TextBox ID="txtConsultOther" runat="server" />.
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label labelcolor">FOLLOW UP:</label><asp:TextBox ID="txtFollowUpIn" runat="server" />
                        &nbsp;
                        <asp:TextBox ID="txtFollowUpInDate" runat="server" placeholder="MM/dd/yyyy" />
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--<label class="control-label">weeks. </label>
                        <label class="control-label">It is my opinion that the injuries that </label>
                        <asp:TextBox ID="txtFullName1" runat="server" />
                        <label class="control-label">sustained to the </label>
                        <asp:TextBox ID="txtInjureBodyParts" runat="server" />
                        <label class="control-label">are causally related to </label>
                        <asp:TextBox ID="cboAccident" runat="server" />
                        <label class="control-label">that occurred on </label>
                        <asp:TextBox ID="txtOccuredOn" runat="server" />
                        <label class="control-label">as described by the patient.</label>--%>
                    </div>
                </div>
                <div class="row" style="margin-top: 15px">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--<asp:ImageButton ID="LoadDV" Style="" runat="server" OnClick="LoadDV_Click" ImageUrl="~/img/" />--%>
                        <div style="display: none">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn blue" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="MedicinePopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; width: 950px; margin-right: 20%">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    Select Medicine
                    <b id="CatHeading"></b>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMedice">
                        <ContentTemplate>

                            <div class="row" style="margin: 5px">
                                <div class="col-md-3">
                                    <asp:TextBox ID="txDesc" runat="server" Style="margin-bottom: 0px" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnSearch" Text="Filter" CssClass="btn btn-info" OnClick="btnSearch_Click" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnDaigSave" Text="Save & Close" CssClass="btn btn-primary" OnClick="btnDaigSave_Click" />
                                </div>
                                <br />

                                <div class="col-md-12">
                                    <asp:GridView ID="dgvMedicenPopup" runat="server" AutoGenerateColumns="false" DataKeyNames="Medicine_ID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Medicine_ID" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Medicine_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Medicine" ItemStyle-Width="1200">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMediDesc" Style="width: 1200px" runat="server" Text='<%# Eval("Medicine") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



    <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>
    <script type="text/javascript">
        //function basicPopup() {
        //    popupWindow = window.open("AddDrugs.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        //};
        function OnSuccess(response) {
            //debugger;
            popupWindow = window.open("AddDrugs.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        }
        function basicPopup() {
            saveall();
            document.forms[0].target = "_blank";
        };
    </script>
    <script>
        $.noConflict();

        function openModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('show');

            })(jQuery);
        }

        function closeModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('hide');

            })(jQuery);
        }


        $(document).ready(function () {
            $('#rbl_in_past input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_injur_past_bp").prop('disabled', true);
                    $("#txt_injur_past_how").prop('disabled', true);
                }
                else {
                    $("#txt_injur_past_bp").prop('disabled', false);
                    $("#txt_injur_past_how").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rbl_seen_injury input').change(function () {
                if ($(this).val() == 'False') {
                    $("#txt_docname").prop('disabled', true);
                }
                else {
                    $("#txt_docname").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rep_wenttohospital input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_day").prop('disabled', true);
                    $("#txt_day").prop('value', "0");
                }
                else {
                    $("#txt_day").prop('disabled', false);
                    $("#txt_day").select();
                    $("#txt_day").focus();
                }
            });
        });

        $(document).ready(function () {
            $('#rep_hospitalized input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_hospital").prop('disabled', true);
                    $("#txt_day").prop('disabled', true);
                    $("#chk_mri").prop('disabled', true);
                    $("#txt_mri").prop('disabled', true);
                    $("#chk_CT").prop('disabled', true);
                    $("#txt_CT").prop('disabled', true);
                    $("#chk_xray").prop('disabled', true);
                    $("#txt_x_ray").prop('disabled', true);
                    $("#txt_prescription").prop('disabled', true);
                    $("#txt_which_what").prop('disabled', true);
                }
                else {
                    $("#txt_hospital").prop('disabled', false);
                    $("#ddl_via").prop('disabled', false);
                    $("#txt_day").prop('disabled', false);
                    $("#chk_mri").prop('disabled', false);
                    $("#txt_mri").prop('disabled', false);
                    $("#chk_CT").prop('disabled', false);
                    $("#txt_CT").prop('disabled', false);
                    $("#chk_xray").prop('disabled', false);
                    $("#txt_x_ray").prop('disabled', false);
                    $("#txt_prescription").prop('disabled', false);
                    $("#txt_which_what").prop('disabled', false);
                }
            });
        });
    </script>
</asp:Content>
