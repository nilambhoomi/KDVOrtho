<%@ Page Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Lowback.aspx.cs" Inherits="Lowback" %>


<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="js/images/bootstrap.min.js"></script>


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
    </style>
    <style>
        .ui-button ui-widget ui-state-default ui-button-icon-only ui-corner-right ui-button-icon {
        }

        .ui-menu-item {
            /*display: none;*/
            position: absolute;
            background-color: #f9f9f9;
            min-width: 160px;
            box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
            z-index: 1;
        }

        .ui-corner-all a {
            color: black;
            /*padding: 12px 16px;*/
            text-decoration: none;
            display: block;
        }
    </style>
    <style>
        button.ui-button-icon-only {
            width: 1.4em;
            height: 27px;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
               <%-- document.getElementById('<%= btnSave.ClientID %>').click();--%>
                funSave();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
            <%--document.getElementById('<%= btnSave.ClientID %>').click();--%>
            funSave();
        }

        function checkTP(val, isEdit) {

            if (val === 0)
                $('#divTP').hide();
            else
                $('#divTP').show();

            if (isEdit === 0) {

                $.get("<%= ResolveUrl("~/Template/Default_Admin.xml")%>", function (res) {


                    var defaults = $(res).find("Defaults");
                    var sides = $(defaults).find("LowBack");


                    debugger;


                    if (sides[0].getElementsByTagName('TPSide1')[0].textContent.trim() !== '')
                        $("#ddlTPSide1 option:contains(" + sides[0].getElementsByTagName('TPSide1')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);

                    $('#txtTPText1').val(sides[0].getElementsByTagName('TPText1')[0].innerHTML);

                    if (sides[0].getElementsByTagName('TPSide2')[0].textContent.trim() !== '')
                        $("#ddlTPSide2 option:contains(" + sides[0].getElementsByTagName('TPSide2')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);

                    $('#txtTPText2').val(sides[0].getElementsByTagName('TPText2')[0].innerHTML);

                    if (sides[0].getElementsByTagName('TPSide3')[0].textContent.trim() !== '')
                        $("#ddlTPSide3 option:contains(" + sides[0].getElementsByTagName('TPSide3')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);

                    $('#txtTPText3').val(sides[0].getElementsByTagName('TPText3')[0].innerHTML);
                                 

                    if (sides[0].getElementsByTagName('TPSide4')[0].textContent.trim() !== '')
                        $("#ddlTPSide4 option:contains(" + sides[0].getElementsByTagName('TPSide4')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);

                    $('#txtTPText4').val(sides[0].getElementsByTagName('TPText4')[0].innerHTML);

                });
            }
        }



        function bindSideval(sideStr, sideStr1) {

            if (sideStr !== '')
                $("#ddlTPSide1 option:contains(" + sideStr.trim() + ")").attr("selected", true);

            $('#txtTPText1').val(sideStr1);

        }

        function funSavePE() {
            var htmlval = $("#ctl00_ContentPlaceHolder1_divPE").html();

            txtTPText1 = $("#txtTPText1").val();
            ddlTPSide1 = $("#ddlTPSide1").val();

            var selectedLeft = '', selectedRight = '';
            var selectedHeader = '';
            $('#trLeft input[type=checkbox]').each(function () {
                if ($(this).is(":checked")) {
                    selectedLeft = selectedLeft + "," + 1;
                } else
                    selectedLeft = selectedLeft + "," + 0;
            });

            $('#trRight input[type=checkbox]').each(function () {
                if ($(this).is(":checked")) {
                    selectedRight = selectedRight + "," + 1;
                } else
                    selectedRight = selectedRight + "," + 0;
            });

            $('#trHeader td').each(function () {
                debugger
                selectedHeader = selectedHeader + "," + $(this).html();
            });

            var txtVal = $('#txtLegRaisedExamLeft').val() + ',' + $('#txtLegRaisedExamRight').val();


            var sidestr = ddlTPSide1;
            var sidestrText = txtTPText1;

            $('#<%= hdNameTest.ClientID %>').val(selectedHeader.trim(','));
            $('#<%= hdLeftTest.ClientID %>').val(selectedLeft.trim(','));
            $('#<%= hdRightTest.ClientID %>').val(selectedRight.trim(','));
            $('#<%= hdTextVal.ClientID %>').val(txtVal);


            $('#<%= hdPESides.ClientID %>').val(sidestr);
            $('#<%= hdPESidesText.ClientID %>').val(sidestrText);

            $('#<%= hdPEvalue.ClientID %>').val(htmlval);
        }

        function bindRadiobuttonValues(valstr, valstr1) {



            if (valstr !== '') {
                var arrayVal = valstr.split(',');

                $("input[name='RSH'][value='" + arrayVal[0] + "']").prop('checked', true);
                $("input[name='RSC'][value='" + arrayVal[1] + "']").prop('checked', true);
                $("input[name='RAR'][value='" + arrayVal[2] + "']").prop('checked', true);
                $("input[name='RFA'][value='" + arrayVal[3] + "']").prop('checked', true);
                $("input[name='RHA'][value='" + arrayVal[4] + "']").prop('checked', true);
                $("input[name='RWR'][value='" + arrayVal[5] + "']").prop('checked', true);
                $("input[name='R1D'][value='" + arrayVal[6] + "']").prop('checked', true);
                $("input[name='R2D'][value='" + arrayVal[7] + "']").prop('checked', true);
                $("input[name='R3D'][value='" + arrayVal[8] + "']").prop('checked', true);
                $("input[name='R4D'][value='" + arrayVal[9] + "']").prop('checked', true);

            }

            if (valstr1 !== '') {

                var arrayVal1 = valstr1.split(',');

                $("input[name='ASH'][value='" + arrayVal1[0] + "']").prop('checked', true);
                $("input[name='ASC'][value='" + arrayVal1[1] + "']").prop('checked', true);
                $("input[name='AAR'][value='" + arrayVal1[2] + "']").prop('checked', true);
                $("input[name='AFA'][value='" + arrayVal1[3] + "']").prop('checked', true);
                $("input[name='AHA'][value='" + arrayVal1[4] + "']").prop('checked', true);
                $("input[name='AWR'][value='" + arrayVal1[5] + "']").prop('checked', true);
                $("input[name='A1D'][value='" + arrayVal1[6] + "']").prop('checked', true);
                $("input[name='A2D'][value='" + arrayVal1[7] + "']").prop('checked', true);
                $("input[name='A3D'][value='" + arrayVal1[8] + "']").prop('checked', true);
                $("input[name='A4D'][value='" + arrayVal1[9] + "']").prop('checked', true);

            }
        }


        function funSave() {


            var htmlval = $("#ctl00_ContentPlaceHolder1_CF").html();


            $('#<%= hdCCvalue.ClientID %>').val(htmlval);

            funSavePE();

            document.getElementById('<%= btnSave.ClientID %>').click();

        }
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
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



                <div runat="server" id="CF">
                </div>


                <asp:HiddenField runat="server" ID="hdCCvalue" />
                <asp:HiddenField runat="server" ID="hdNameTest" />
                <asp:HiddenField runat="server" ID="hdLeftTest" />
                <asp:HiddenField runat="server" ID="hdRightTest" />
                <asp:HiddenField runat="server" ID="hdTextVal" />

                <%-- <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">

                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:TextBox runat="server" ID="txtFreeFormCC" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                            <button type="button" id="start_button1" onclick="startButton1(event)">
                                <img src="images/mic.gif" alt="start" /></button>
                            <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>
                        </div>
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label bold"><b><u>PHYSICAL EXAM:</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Repeater runat="server" ID="repROMCervical">
                                        <HeaderTemplate>
                                            <table style="width: 100%;">

                                                <tr>

                                                    <td style="">lumber spine exam
                                                    </td>

                                                    <td style="">ROM
                                                    </td>

                                                    <td style="">Normal
                                                    </td>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:Label runat="server" ID="lblname" Text='<%# Eval("name") %>'></asp:Label></td>
                                                <%--<td>
                                                <asp:TextBox ID="txtLEFlexionLeftWas" Text="30" Width="50px" runat="server"></asp:TextBox></td>--%>
                                                <td>
                                                    <asp:TextBox ID="txtrom" runat="server" Width="50px" onkeypress="return onlyNumbers(event);" Text='<%# Eval("rom") %>'></asp:TextBox></td>
                                                <%-- <td>
                                                <asp:TextBox ID="txtLEFlexionRightWas" Width="50px" runat="server"></asp:TextBox></td>--%>

                                                <td>
                                                    <asp:TextBox ID="txtnormal" ReadOnly="true" Text='<%# Eval("normal") %>' Width="50px" runat="server"></asp:TextBox></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td>
                                    <asp:Repeater runat="server" ID="repROM">
                                        <HeaderTemplate>
                                            <table style="width: 100%;">


                                                <tr>
                                                    <td></td>
                                                    <td style="">Left
                                                    </td>
                                                    <%--<td style="">IS
                                            </td>--%>
                                                    <td style="">Right
                                                    </td>
                                                    <%--<td style="">IS
                                            </td>--%>
                                                    <td style="">Normal
                                                    </td>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="text-align: left;">
                                                    <asp:Label runat="server" ID="lblname" Text='<%# Eval("name") %>'></asp:Label></td>
                                                <%--<td>
                                                <asp:TextBox ID="txtLEFlexionLeftWas" Text="30" Width="50px" runat="server"></asp:TextBox></td>--%>
                                                <td>
                                                    <asp:TextBox ID="txtleft" runat="server" Width="50px" onkeypress="return onlyNumbers(event);" Text='<%# Eval("left") %>'></asp:TextBox></td>
                                                <%-- <td>
                                                <asp:TextBox ID="txtLEFlexionRightWas" Width="50px" runat="server"></asp:TextBox></td>--%>
                                                <td>
                                                    <asp:TextBox ID="txtright" Width="50px" Text='<%# Eval("right") %>' onkeypress="return onlyNumbers(event);" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="txtnormal" ReadOnly="true" Text='<%# Eval("normal") %>' Width="50px" runat="server"></asp:TextBox></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div runat="server" id="divPE">
                </div>


                <asp:HiddenField runat="server" ID="hdPEvalue" />
                <asp:HiddenField runat="server" ID="hdPESides" />
                <asp:HiddenField runat="server" ID="hdPESidesText" />

                <%--  <div class="table-responsive">
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <td></td>
                                        <td colspan="1">Strainght leg raise exam</td>
                                        <td colspan="1">Braggard's test</td>
                                        <td style="">Kernig's sign</td>
                                        <td style="">Brudzinski's test</td>
                                        <td style="">Sacroiliac compression</td>
                                        <td style="">Sacral notch tenderness</td>
                                        <td style="">Ober's test causing pain at the SI joint</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td style="">Left</td>
                                        <td colspan="1">
                                            <asp:CheckBox ID="chkLegRaisedExamLeft" runat="server" />
                                            <asp:TextBox ID="txtLegRaisedExamLeft" Style="height: 15px;" Width="80px" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="1">
                                            <asp:CheckBox ID="chkBraggardLeft" runat="server" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkKernigLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkBrudzinskiLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacroiliacLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacralNotchLeft" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkOberLeft" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="">Right</td>
                                        <td>
                                            <asp:CheckBox ID="chkLegRaisedExamRight" runat="server" />
                                            <asp:TextBox ID="txtchkLegRaisedExamRight" Style="height: 15px;" Width="80px" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkBraggardRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkKernigRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkBrudzinskiRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacroiliacRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacralNotchRight" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkOberRight" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="">Bilateral</td>
                                        <td>
                                            <asp:CheckBox ID="chkLegRaisedExamBilateral" runat="server" />
                                            <asp:TextBox ID="txtLegRaisedExamBilateral" Style="height: 15px;" Width="80px" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkBraggardBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkKernigBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkBrudzinskiBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacroiliacBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkSacralNotchBilateral" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="chkOberBilateral" runat="server" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>--%>



                <%--   <div class="row">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <label class="control-label">The lumbar spine exam reveals tenderness upon palpation at &nbsp;</label>
                        <asp:TextBox ID="txtPalpationAt" Style="height: 15px;" Text="L1-S1" Width="50px" runat="server"></asp:TextBox>
                        <label class="control-label">levels</label>
                      
                        <editable:EditableDropDownList runat="server" ID="cboLevels" Width="257px" CssClass="inline">
                        </editable:EditableDropDownList>
                        <label class="control-label" style="display: none">with muscle spasm present.</label><br />
                        <br />

                      

                    </div>
                </div>


                <div class="row" runat="server" id="divTP">
                    <div class="col-md-3">
                        <label class="control-label">Trigger Point:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide1" DataSourceID="TPSide1XML" DataTextField="name" runat="server" Style="height: 30px; width: 200px"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide1XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText1" Style="margin-left: 20px;" runat="server" Text="para spinal level L3-S1 with referral patterns laterally to the region in a fan-like pattern" Width="556px"></asp:TextBox>
                                </td>
                            </tr>

                        </table>
                    </div>
                </div>--%>

                <%-- <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" Style="" ID="txtFreeForm" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                    </div>
                </div>--%>
                <asp:UpdatePanel runat="server" ID="upMedicine">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label"><b><u>ASSESSMENT/DIAGNOSIS:</u></b></label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Notes:</label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                                <asp:TextBox runat="server" Style="" ID="txtFreeFormA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                <asp:ImageButton ID="AddDiag" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDiag_Click" />

                                <input type="button" value="Default" style="float: right left; align-self: baseline; text-align: start;" onclick="saveDefault()" />
                                <asp:GridView ID="dgvDiagCodes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Width="100" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hidDiagCodeID" Value='<%# Eval("Diag_Master_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="650">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtpe" runat="server" Width="700" Text='<%# Eval("Description") %>'></asp:TextBox>
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
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>PLAN:</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%-- <asp:CheckBox ID="chkCervicalSpine" Style="" Text="MRI" runat="server" />--%>
                        <%-- <asp:DropDownList ID="cboScanType" Style=" height: 25px;" runat="server"></asp:DropDownList>
                                <asp:Label ID="Label7" Style="" Text=" of the cervical spine " runat="server"></asp:Label>
                                <asp:TextBox ID="txtToRuleOut" runat="server" Style="" Text="to rule out herniated nucleus pulposus/soft tissue injury " Width="299px"></asp:TextBox>--%>
                        <%--OnClick="AddStd_Click"--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormP" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddStd" Style="display: none;" runat="server" Height="50px" Width="50px" ImageUrl="~/img/a1.png" PostBackUrl="~/AddStandards.aspx" OnClientClick="basicPopup();return false;" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:GridView ID="dgvStandards" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px" TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PDesc" ItemStyle-Width="600">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPDesc" runat="server" CssClass="form-control" Width="600px" TextMode="MultiLine" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="IsChkd">

                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" value='<%# Convert.ToBoolean(Eval("IsChkd")) %>' AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="MCODE" ItemStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="mcode" runat="server" Text='<%# Eval("MCODE") %>'></asp:Label>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%-- <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ProcedureDetail_ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>
                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px" TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>
                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px" TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="CC" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("CCDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("PEDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AD" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtadesc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("ADesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PD" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdesc" Width="95" ReadOnly="true" runat="server" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- <asp:TemplateField HeaderText="PN" ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                <%--<asp:TemplateField HeaderText="IsChkd">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox4" Enabled="false" runat="server" value='<%# Eval("PN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row"></div>
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
                    Select Diag 
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
                                    <asp:GridView ID="dgvDiagCodesPopup" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="DiagCode_ID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">

                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' AutoPostBack="true" />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
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
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="550">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" Width="550" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
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

        var $j = jQuery.noConflict();
        $j('#MedicinePopup').on('hidden.bs.modal', function (e) {
            $('#ctl00_lnkbtn_lowback').addClass('active');
        });

        //function basicPopup() {
        //    popupWindow = window.open("AddStandards.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        //};
        //function basicPopup1() {
        //    popupWindow = window.open("AddDiagnosis.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        //}
        function OnSuccess(response) {
            //debugger;
            popupWindow = window.open("AddStandards.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        }
        function OnSuccess_q(response) {
            popupWindow = window.open("AddDiagnosis.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');

        }
        function basicPopup() {
            document.forms[0].target = "_blank";
        };
    </script>
    <script>
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
    <script>
        var controlname = null;
        var final_transcript = '';
        var recognizing = false;
        var ignore_onend;
        var start_timestamp;

        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition = new webkitSpeechRecognition();
            recognition.continuous = true;
            recognition.interimResults = true;

            recognition.onstart = function () {
                recognizing = true;
            };

            recognition.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend = true;
                }
            };

            recognition.onend = function () {
                recognizing = false;
                if (ignore_onend) {
                    return;
                }
                if (!final_transcript) {
                    //showInfo('info_start');
                    return;
                }
                if (!final_transcript1) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition.onresult = function (event) {
                var interim_transcript = '';
                if (typeof (event.results) == 'undefined') {
                    recognition.onend = null;
                    recognition.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript += event.results[i][0].transcript;
                    } else {
                        interim_transcript += event.results[i][0].transcript;
                    }
                }
                final_transcript = capitalize(final_transcript);
                //finalrecord = linebreak(final_transcript);
                //$('#ctl00_ContentPlaceHolder1_txtFreeForm').text(linebreak(final_transcript));
                $(controlname).text(linebreak(final_transcript));
                interim_span.innerHTML = linebreak(interim_transcript);
            };
        }



        var two_line = /\n\n/g;
        var one_line = /\n/g;
        function linebreak(s) {
            return s.replace(two_line, '<p></p>').replace(one_line, '<br>');
        }

        var first_char = /\S/;
        function capitalize(s) {
            return s.replace(first_char, function (m) { return m.toUpperCase(); });
        }

        function startButton(event) {
            controlname = "#ctl00_ContentPlaceHolder1_txtFreeForm";
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span.innerHTML = '';
            interim_span.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }

        function startButton1(event) {
            controlname = "#ctl00_ContentPlaceHolder1_txtFreeFormCC";
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span1.innerHTML = '';
            interim_span1.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }
        function saveDefault() {
            document.getElementById('<%= btnDaigSave.ClientID %>').click();
        }
    </script>
</asp:Content>
