<%@ Page Title="" Language="C#" MasterPageFile="~/AddFollowUpMaster.master" AutoEventWireup="true" CodeFile="FuShoulder.aspx.cs" Inherits="FuShoulder" %>

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
              <%--  document.getElementById('<%= btnSave.ClientID %>').click();--%>
                funSave();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
         <%--   document.getElementById('<%= btnSave.ClientID %>').click();--%>
            funSave();
        }

        function checkTP(val, pos) {

            if (val === 0)
                $('#divTP').hide();
            else
                $('#divTP').show();

            if (pos === 'left') {
                $('#trright').hide();
                $('#trbilateral').hide();
            }
            else if (pos === 'right') {
                $('#trleft').hide();
                $('#trbilateral').hide();
            }
            //else {
            //    $('#trleft').hide();
            //    $('#trright').hide();
            //}

        }


        function funSavePE() {

            var htmlval = $("#ctl00_ContentPlaceHolder1_divPE").html();
            $('#<%= hdPEvalue.ClientID %>').val(htmlval);

        }



        function funSave() {

            var htmlval = $("#ctl00_ContentPlaceHolder1_CF").html();


            $('#<%= hdCCvalue.ClientID %>').val(htmlval);

            var htmlval = $("#<%= hdorgval.ClientID %>").val();
            var orghtmlval = htmlval;

            funSavePE();


            $('#<%= hdCCvalueoriginal.ClientID %>').val(orghtmlval);


            document.getElementById('<%= btnSave.ClientID %>').click();


            chkcontentL = $("#chkcontentL").is(':checked') ? "checked=\"checked\"" : "";
            chkintermittentL = $("#chkintermittentL").is(':checked') ? "checked=\"checked\"" : "";


            chksharpL = $("#chksharpL").is(':checked') ? "checked=\"checked\"" : "";
            chkelectricL = $("#chkelectricL").is(':checked') ? "checked=\"checked\"" : "";
            chkshootingL = $("#chkshootingL").is(':checked') ? "checked=\"checked\"" : "";
            chkthrobbingL = $("#chkthrobbingL").is(':checked') ? "checked=\"checked\"" : "";
            chkpulsatingL = $("#chkpulsatingL").is(':checked') ? "checked=\"checked\"" : "";
            chkdullL = $("#chkdullL").is(':checked') ? "checked=\"checked\"" : "";
            chkachyL = $("#chkachyL").is(':checked') ? "checked=\"checked\"" : "";

            chkWorseLyingL = $("#chkWorseLyingL").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseMovementL = $("#chkWorseMovementL").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseLiftingL = $("#chkWorseLiftingL").is(':checked') ? "checked=\"checked\"" : "";

            chkWorseRotationL = $("#chkWorseRotationL").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseWorkingL = $("#chkWorseWorkingL").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseActivitiesL = $("#chkWorseActivitiesL").is(':checked') ? "checked=\"checked\"" : "";

            chkImprovedRestingL = $("#chkImprovedRestingL").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedMedicationL = $("#chkImprovedMedicationL").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedTherapyL = $("#chkImprovedTherapyL").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedSleepingL = $("#chkImprovedSleepingL").is(':checked') ? "checked=\"checked\"" : "";




            txtPainScaleL = 'value="' + $("#txtPainScaleL").val() + '"';


            chkcontentR = $("#chkcontentR").is(':checked') ? "checked=\"checked\"" : "";
            chkintermittentR = $("#chkintermittentR").is(':checked') ? "checked=\"checked\"" : "";


            chksharpR = $("#chksharpR").is(':checked') ? "checked=\"checked\"" : "";
            chkelectricR = $("#chkelectricR").is(':checked') ? "checked=\"checked\"" : "";
            chkshootingR = $("#chkshootingR").is(':checked') ? "checked=\"checked\"" : "";
            chkthrobbingR = $("#chkthrobbingR").is(':checked') ? "checked=\"checked\"" : "";
            chkpulsatingR = $("#chkpulsatingR").is(':checked') ? "checked=\"checked\"" : "";
            chkdullR = $("#chkdullR").is(':checked') ? "checked=\"checked\"" : "";
            chkachyR = $("#chkachyR").is(':checked') ? "checked=\"checked\"" : "";

            chkWorseLyingR = $("#chkWorseLyingR").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseMovementR = $("#chkWorseMovementR").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseLiftingR = $("#chkWorseLiftingR").is(':checked') ? "checked=\"checked\"" : "";

            chkWorseRotationR = $("#chkWorseRotationR").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseWorkingR = $("#chkWorseWorkingR").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseActivitiesR = $("#chkWorseActivitiesR").is(':checked') ? "checked=\"checked\"" : "";

            chkImprovedRestingR = $("#chkImprovedRestingR").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedMedicationR = $("#chkImprovedMedicationR").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedTherapyR = $("#chkImprovedTherapyR").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedSleepingR = $("#chkImprovedSleepingR").is(':checked') ? "checked=\"checked\"" : "";




            txtPainScaleR = 'value="' + $("#txtPainScaleR").val() + '"';


            htmlval = htmlval.replace('#txtPainScaleL', txtPainScaleL);
            htmlval = htmlval.replace('#txtPainScaleR', txtPainScaleR);


            htmlval = htmlval.replace('#chkcontentL', chkcontentL);
            htmlval = htmlval.replace('#chkintermittentL', chkintermittentL);
            htmlval = htmlval.replace('#chksharpL', chksharpL);
            htmlval = htmlval.replace('#chkelectricL', chkelectricL);
            htmlval = htmlval.replace('#chkshootingL', chkshootingL);
            htmlval = htmlval.replace('#chkthrobbingL', chkthrobbingL);
            htmlval = htmlval.replace('#chkpulsatingL', chkpulsatingL);
            htmlval = htmlval.replace('#chkdullL', chkdullL);
            htmlval = htmlval.replace('#chkachyL', chkachyL);
            htmlval = htmlval.replace('#chkWorseLyingL', chkWorseLyingL);
            htmlval = htmlval.replace('#chkWorseMovementL', chkWorseMovementL);
            htmlval = htmlval.replace('#chkWorseLiftingL', chkWorseLiftingL);
            htmlval = htmlval.replace('#chkWorseRotationL', chkWorseRotationL);
            htmlval = htmlval.replace('#chkWorseWorkingL', chkWorseWorkingL);
            htmlval = htmlval.replace('#chkWorseActivitiesL', chkWorseActivitiesL);
            htmlval = htmlval.replace('#chkImprovedRestingL', chkImprovedRestingL);
            htmlval = htmlval.replace('#chkImprovedMedicationL', chkImprovedMedicationL);
            htmlval = htmlval.replace('#chkImprovedTherapyL', chkImprovedTherapyL);
            htmlval = htmlval.replace('#chkImprovedSleepingL', chkImprovedSleepingL);

            htmlval = htmlval.replace('#chkcontentR', chkcontentR);
            htmlval = htmlval.replace('#chkintermittentR', chkintermittentR);
            htmlval = htmlval.replace('#chksharpR', chksharpR);
            htmlval = htmlval.replace('#chkelectricR', chkelectricR);
            htmlval = htmlval.replace('#chkshootingR', chkshootingR);
            htmlval = htmlval.replace('#chkthrobbingR', chkthrobbingR);
            htmlval = htmlval.replace('#chkpulsatingR', chkpulsatingR);
            htmlval = htmlval.replace('#chkdullR', chkdullR);
            htmlval = htmlval.replace('#chkachyR', chkachyR);
            htmlval = htmlval.replace('#chkWorseLyingR', chkWorseLyingR);
            htmlval = htmlval.replace('#chkWorseMovementR', chkWorseMovementR);
            htmlval = htmlval.replace('#chkWorseLiftingR', chkWorseLiftingR);
            htmlval = htmlval.replace('#chkWorseRotationR', chkWorseRotationR);
            htmlval = htmlval.replace('#chkWorseWorkingR', chkWorseWorkingR);
            htmlval = htmlval.replace('#chkWorseActivitiesR', chkWorseActivitiesR);
            htmlval = htmlval.replace('#chkImprovedRestingR', chkImprovedRestingR);
            htmlval = htmlval.replace('#chkImprovedMedicationR', chkImprovedMedicationR);
            htmlval = htmlval.replace('#chkImprovedTherapyR', chkImprovedTherapyR);
            htmlval = htmlval.replace('#chkImprovedSleepingR', chkImprovedSleepingR);




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

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormCC" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button1" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>

                    </div>
                </div>

                <asp:HiddenField runat="server" ID="hdId" Value="0" />
                <asp:HiddenField runat="server" ID="hdorgval" />
                <asp:HiddenField runat="server" ID="hdCCvalue" />
                <asp:HiddenField runat="server" ID="hdCCvalueoriginal" />
                <asp:HiddenField runat="server" ID="hdCCradiateval" />

                <div class="clearfix"></div>



                <div class="col-md-3">
                    <label class="control-label"><b><u>PHYSICAL EXAM:</u></b></label>
                </div>

                <div class="col-md-9" style="margin-top: 5px">

                    <asp:Repeater runat="server" ID="repROM" OnItemDataBound="repROM_ItemDataBound">
                        <HeaderTemplate>
                            <table style="width: 40%; display: none">

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
                                    <asp:TextBox ID="txtleft" runat="server" Width="50px" onkeypress="return onlyNumbers(event);" Text='<%# Eval("left") %>'></asp:TextBox></td>

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




                    <div runat="server" id="divPE">
                    </div>


                    <div class="col-md-3">
                        <label class="control-label">Notes</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeForm" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>

                    </div>


                    <asp:HiddenField runat="server" ID="hdorgvalPE" />
                    <asp:HiddenField runat="server" ID="hdPEvalue" />
                    <asp:HiddenField runat="server" ID="hdPEvalueoriginal" />
                    <asp:HiddenField runat="server" ID="hdPESides" />
                    <asp:HiddenField runat="server" ID="hdPESidesText" />


                    <asp:UpdatePanel runat="server" ID="upMedicine">
                        <ContentTemplate>

                            <div class="col-md-3">
                                <label class="control-label"><b><u>ASSESSMENT/DIAGNOSIS:</u></b></label>
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
                                    <asp:TextBox runat="server" Style="float: left;" ID="txtFreeFormA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                    <%-- <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="basicPopup();" OnClick="AddDiag_Click" />--%>
                                    <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDiag_Click" />
                                    <asp:GridView ID="dgvDiagCodes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Width="100" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hidDiagCodeID" Value='<%# Eval("Diag_Master_ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="700">
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
                                    <asp:Button runat="server" ID="btnSearch" Text="Filter" CssClass="btn btn-info" />
                                        &nbsp;
                                    <asp:Button runat="server" ID="btnDaigSave" Text="Save & Close" CssClass="btn btn-primary" OnClick="btnDaigSave_Click" />
                                    </div>
                                    <br />

                                    <div class="col-md-12">
                                        <asp:GridView ID="dgvDiagCodesPopup" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="DiagCode_ID">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">

                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' />
                                                    </ItemTemplate>
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
                $('#ctl00_lnkbtn_Shoulder').addClass('active');
            });


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
        </script>
</asp:Content>
