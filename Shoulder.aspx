<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PageMainMaster.master" CodeFile="Shoulder.aspx.cs" Inherits="Shoulder" %>


<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                <%--document.getElementById('<%= btnSave.ClientID %>').click();--%>
                funSave();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
           <%-- document.getElementById('<%= btnSave.ClientID %>').click();--%>
            funSave();
        }


        function checkTP(val, pos, isEdit) {

            var position;


            if (val === 0)
                $('#divTP').hide();
            else
                $('#divTP').show();


            if (pos === 'left') {
                $('#trright').hide();
                $('#trbilateral').hide();
                position = 'left';
            }
            else if (pos === 'right') {
                $('#trleft').hide();
                $('#trbilateral').hide();
                position = 'right';
            }
            else {
                position = 'bilateral';
            }
            //else {
            //    $('#trleft').hide();
            //    $('#trright').hide();
            //}


            if (isEdit === 0) {



                $.get("<%= ResolveUrl("~/Template/Default_Admin.xml")%>", function (res) {

                    debugger

                    var defaults = $(res).find("Defaults");
                    var sidepos = $(defaults).find("Shoulder");
                    var sides = $(sidepos).find(position);

                    //$("#ddlTPSide1").val(position);
                    //$("#ddlTPSide2").val(position);
                    //$("#ddlTPSide3").val(position);
                    //$("#ddlTPSide4").val(position);
                    //$("#ddlTPSide5").val(position);
                    //$("#ddlTPSide6").val(position);
                    //$("#ddlTPSide7").val(position);
                    //$("#ddlTPSide8").val(position);

                    debugger;


                    if (sides[0].getElementsByTagName('TPSide1')[0].textContent.trim() !== '')
                        $("#ddlTPSide1 option:contains(" + sides[0].getElementsByTagName('TPSide1')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide2')[0].textContent.trim() !== '')
                        $("#ddlTPSide2 option:contains(" + sides[0].getElementsByTagName('TPSide2')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide3')[0].textContent.trim() !== '')
                        $("#ddlTPSide3 option:contains(" + sides[0].getElementsByTagName('TPSide3')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide4')[0].textContent.trim() !== '')
                        $("#ddlTPSide4 option:contains(" + sides[0].getElementsByTagName('TPSide4')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide5')[0].textContent.trim() !== '')
                        $("#ddlTPSide5 option:contains(" + sides[0].getElementsByTagName('TPSide5')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide6')[0].textContent.trim() !== '')
                        $("#ddlTPSide6 option:contains(" + sides[0].getElementsByTagName('TPSide6')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide7')[0].textContent.trim() !== '')
                        $("#ddlTPSide7 option:contains(" + sides[0].getElementsByTagName('TPSide7')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);
                    if (sides[0].getElementsByTagName('TPSide8')[0].textContent.trim() !== '')
                        $("#ddlTPSide8 option:contains(" + sides[0].getElementsByTagName('TPSide8')[0].textContent.trim().toLowerCase() + ")").attr("selected", true);


                    $('#txtTPText1').val(sides[0].getElementsByTagName('TPText1')[0].innerHTML);
                    $('#txtTPText2').val(sides[0].getElementsByTagName('TPText2')[0].innerHTML);
                    $('#txtTPText3').val(sides[0].getElementsByTagName('TPText3')[0].innerHTML);
                    $('#txtTPText4').val(sides[0].getElementsByTagName('TPText4')[0].innerHTML);
                    $('#txtTPText5').val(sides[0].getElementsByTagName('TPText5')[0].innerHTML);
                    $('#txtTPText6').val(sides[0].getElementsByTagName('TPText6')[0].innerHTML);
                    $('#txtTPText7').val(sides[0].getElementsByTagName('TPText7')[0].innerHTML);
                    $('#txtTPText8').val(sides[0].getElementsByTagName('TPText8')[0].innerHTML);

                });
            }
        }


        function funSavePE() {

            var htmlval = $("#ctl00_ContentPlaceHolder1_divPE").html();
            $('#<%= hdPEvalue.ClientID %>').val(htmlval);

            txtTPText1 = $("#txtTPText1").val();
            txtTPText2 = $("#txtTPText2").val();
            txtTPText3 = $("#txtTPText3").val();
            txtTPText4 = $("#txtTPText4").val();
            txtTPText5 = $("#txtTPText5").val();
            txtTPText6 = $("#txtTPText6").val();
            txtTPText7 = $("#txtTPText7").val();
            txtTPText8 = $("#txtTPText8").val();


            ddlTPSide1 = $("#ddlTPSide1").val();
            ddlTPSide2 = $("#ddlTPSide2").val();
            ddlTPSide3 = $("#ddlTPSide3").val();
            ddlTPSide4 = $("#ddlTPSide4").val();
            ddlTPSide5 = $("#ddlTPSide5").val();
            ddlTPSide6 = $("#ddlTPSide6").val();
            ddlTPSide7 = $("#ddlTPSide7").val();
            ddlTPSide8 = $("#ddlTPSide8").val();

            var sidestr = ddlTPSide1 + "," + ddlTPSide2 + "," + ddlTPSide3 + "," + ddlTPSide4 + "," + ddlTPSide5 + "," + ddlTPSide6 + "," + ddlTPSide7 + "," + ddlTPSide8;
            var sidestrText = txtTPText1 + "," + txtTPText2 + "," + txtTPText3 + "," + txtTPText4 + "," + txtTPText5 + "," + txtTPText6 + "," + txtTPText7 + "," + txtTPText8;


            $('#<%= hdPESides.ClientID %>').val(sidestr);
            $('#<%= hdPESidesText.ClientID %>').val(sidestrText);

        }

        function bindSidesVal(valStr, vatstr1) {


            var val = valStr.split(',');

            if (val[0] !== '')
                $("#ddlTPSide1 option:contains(" + val[0] + ")").attr("selected", true);
            if (val[1] !== '')
                $("#ddlTPSide2 option:contains(" + val[1] + ")").attr("selected", true);
            if (val[2] !== '')
                $("#ddlTPSide3 option:contains(" + val[2] + ")").attr("selected", true);
            if (val[3] !== '')
                $("#ddlTPSide4 option:contains(" + val[3] + ")").attr("selected", true);
            if (val[4] !== '')
                $("#ddlTPSide5 option:contains(" + val[4] + ")").attr("selected", true);
            if (val[5] !== '')
                $("#ddlTPSide6 option:contains(" + val[5] + ")").attr("selected", true);
            if (val[6] !== '')
                $("#ddlTPSide7 option:contains(" + val[6] + ")").attr("selected", true);
            if (val[7] !== '')
                $("#ddlTPSide8 option:contains(" + val[7] + ")").attr("selected", true);

            val = vatstr1.split(',');

            $('#txtTPText1').val(val[0]);
            $('#txtTPText2').val(val[1]);
            $('#txtTPText3').val(val[2]);
            $('#txtTPText4').val(val[3]);
            $('#txtTPText5').val(val[4]);
            $('#txtTPText6').val(val[5]);
            $('#txtTPText7').val(val[6]);
            $('#txtTPText8').val(val[7]);
        }

        function funSave() {

            debugger
            var htmlval = $("#ctl00_ContentPlaceHolder1_CF").html();


            $('#<%= hdCCvalue.ClientID %>').val(htmlval);

            funSavePE();

            document.getElementById('<%= btnSave.ClientID %>').click();


        }

        function displaySite() {

            var _p = GetParameterValues("P");


            if (_p === "B") {

                $('#WrapLeft').show();
                $('#WrapRight').show();

                $('#WrapLeftPE').show();
                $('#WrapRightPE').show();
            }
            else if (_p === "L") {
                $('#WrapLeft').show();
                $('#WrapRight').hide();

                $('#WrapLeftPE').show();
                $('#WrapRightPE').hide();
            }
            else if (_p === "R") {
                $('#WrapLeft').hide();
                $('#WrapRight').show();

                $('#WrapLeftPE').hide();
                $('#WrapRightPE').show();
            }

        }

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }

        $(document).ready(function ($) {
            displaySite();
        });

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
                <%--    <ul class="breadcrumb">
                                <li>
                                    <i class="icon-home"></i>
                                    <a href="Page1.aspx"><span class="label">Page1</span></a>
                                </li>
                                <li id="lipage2">
                                    <i class="icon-edit"></i>
                                    <a href="Page2.aspx"><span class="label label-success">Page2</span></a>
                                </li>
                                <li id="li1" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page3.aspx"><span class="label">Page3</span></a>
                                </li>
                                <li id="li2" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page4.aspx"><span class="label">Page4</span></a>
                                </li>
                            </ul>--%>

                <div runat="server" id="CF">
                </div>

                 <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormCC" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button1" onclick="startButton1(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>
                    </div>
                </div>

                <asp:HiddenField runat="server" ID="hdCCvalue" />



                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label bold"><b><u>PHYSICAL EXAM:</u></b></label>
                    </div>

                    <div class="col-md-9" style="margin-top: 5px">

                        <asp:Repeater runat="server" ID="repROM" OnItemDataBound="repROM_ItemDataBound" >
                            <HeaderTemplate>
                                <table style="width: 40%; border: none; display: none">

                                    <thead>
                                        <tr>
                                            <td style="text-align: left;">ROM
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td style=""></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td></td>
                                        <td style="">Left
                                        </td>

                                        <td style="">Right
                                        </td>

                                        <td style="">Normal
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label runat="server" ID="lblname" Text='<%# Eval("name") %>'></asp:Label></td>

                                    <td>
                                        <asp:TextBox ID="txtleft" runat="server" Width="50px" Text='<%# Eval("left") %>'></asp:TextBox></td>

                                    <td>
                                        <asp:TextBox ID="txtright" Width="50px" Text='<%# Eval("right") %>' runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtnormal" ReadOnly="true" Text='<%# Eval("normal") %>' Width="50px" runat="server"></asp:TextBox></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>


                    </div>



                    <asp:HiddenField runat="server" ID="hdPEvalue" />
                    <asp:HiddenField runat="server" ID="hdPESides" />
                    <asp:HiddenField runat="server" ID="hdPESidesText" />



                </div>

                <div runat="server" id="divPE">
                </div>

                  <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeForm" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                    </div>
                </div>


                <asp:UpdatePanel runat="server" ID="upMedicine">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label"><b><u>ASSESSMENT/DIAGNOSIS:</b></u></label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                                <%--<asp:CheckBox ID="chkSprainStrain" Style="float: left;" runat="server" Text="Cervical muscle sprain/strain." Checked="true" /><br />
                                <asp:CheckBox ID="chkHerniation" Style="float: left; margin-left: -18.5%" runat="server" Text="Possible cervical disc herniation." Checked="true" /><br />--%>
                                <%-- <asp:CheckBox ID="chkSyndrome" runat="server"  Text="Possible cervical radiculopathy vs. plexopathy vs. entrapment syndrome." Checked="true" />
                                --%>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label">Notes:</label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
                                <asp:TextBox runat="server" ID="txtFreeFormA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                <asp:ImageButton ID="AddDiag" Style="text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDiag_Click" />
                                <input type="button" value="Default" style="float: right left; align-self: baseline; text-align: start;" onclick="saveDefault()" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label"></label>
                            </div>
                            <div class="col-md-9" style="margin-top: 5px">
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
                        <%--  <asp:CheckBox ID="chkCervicalSpine" Style="" Text="MRI" runat="server" />
                                <asp:DropDownList ID="cboScanType" Style=" height: 25px;" runat="server"></asp:DropDownList>
                                <asp:Label ID="Label7" Style="" Text=" of the cervical spine " runat="server"></asp:Label>
                                <asp:TextBox ID="txtToRuleOut" runat="server" Style=" " Text="to rule out herniated nucleus pulposus/soft tissue injury " Width="299px"></asp:TextBox>--%>
                        <%--OnClick="AddStd_Click"--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormP" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddStd" Style="display: none" runat="server" Height="50px" Width="50px" ImageUrl="~/img/a1.png" PostBackUrl="~/AddStandards.aspx" OnClientClick="basicPopup();return false;" />
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
                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ProcedureDetail_ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--<asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
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
                        <%--<asp:ImageButton ID="LoadDV" Style="" runat="server" OnClick="LoadDV_Click" ImageUrl="~/img/edit.gif" />--%>
                        <div style="display: none">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn blue" OnClick="btnSave_Click" />
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
                                    <asp:Button runat="server" ID="btnDaigSave" Text="Save & Close" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnDaigSave_Click" />
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
            $('#ctl00_lnkbtn_Shoulder').addClass('active');
        });

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
