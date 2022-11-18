<%@ Page Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="MIC.aspx.cs" Inherits="MIC" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


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
                  document.getElementById('<%= btnSave.ClientID %>').click();
              }
              else {
                  window.location.href = $('#ctl00_' + page).attr('href');
              }
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
                <%--             <div class="row">
                        <div class="col-md-12">--%>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">CHIEF COMPLAINT</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <label>The patient complains of neck pain that is:</label>
                        <%--<asp:Label Font-Bold="false" runat="server"  Text=""></asp:Label>--%>
                        <asp:TextBox runat="server" ID="txtPainScale" Width="40px"></asp:TextBox>
                        <label>&nbsp;/10 &nbsp;, &nbsp;with 10 being the worst and &nbsp;</label>
                        <%--<asp:Label Font-Bold="false" runat="server"  Text=""></asp:Label>--%>
                        <asp:CheckBox ID="chkContent" runat="server" Text="constant" />
                        <asp:CheckBox ID="chkIntermittent" runat="server" Text="intermittent." />
                        <asp:CheckBox ID="chkSharp" runat="server" Text="sharp" />
                        <asp:CheckBox ID="chkElectric" runat="server" Text="electric" />
                        <asp:CheckBox ID="chkShooting" runat="server" Text="shooting" />
                        <asp:CheckBox ID="chkThrobbling" runat="server" Text="throbbing" />
                        <asp:CheckBox ID="chkPulsating" runat="server" Text="pulsating" />
                        <asp:CheckBox ID="chkDull" runat="server" Text="dull" />
                        <asp:CheckBox ID="chkAchy" runat="server" Text="achy in nature." />

                        <%--<asp:Label runat="server" Shoulder	 Text=""></asp:Label><br />--%>
                        <asp:TextBox ID="txtRadiates" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtBurningTo" runat="server" Visible="false"></asp:TextBox><br />
                        <label class="control-label">The neck pain radiates to:</label>
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>
                                        <asp:ImageButton ID="btnReset" runat="server" Height="30px" Width="30px" ImageUrl="~/img/reset.png" OnClick="btnReset_Click" /></th>
                                    <th class="table_cell">
                                        <label>Shoulder </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Scapula </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Arm </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Forearm </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Hand </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Wrist</label></th>
                                    <th class="table_cell">
                                        <label>1st digit </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>2nd digit </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>3rd digit</label></th>
                                    <th class="table_cell">
                                        <label>4th digit</label></th>
                                    <th class="table_cell">
                                        <label>5th digit </label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="">Left</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderLeft1" runat="server" GroupName="RSH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaLeft1" runat="server" GroupName="RSC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmLeft1" runat="server" GroupName="RAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmLeft1" runat="server" GroupName="RFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandLeft1" runat="server" GroupName="RHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristLeft1" runat="server" GroupName="RWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitLeft1" runat="server" GroupName="R1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitLeft1" runat="server" GroupName="R2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitLeft1" runat="server" GroupName="R3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitLeft1" runat="server" GroupName="R4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitLeft1" runat="server" GroupName="R5D" /></td>
                                </tr>
                                <tr>
                                    <td>Right</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderRight1" runat="server" GroupName="RSH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaRight1" runat="server" GroupName="RSC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmRight1" runat="server" GroupName="RAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmRight1" runat="server" GroupName="RFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandRight1" runat="server" GroupName="RHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristRight1" runat="server" GroupName="RWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitRight1" runat="server" GroupName="R1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitRight1" runat="server" GroupName="R2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitRight1" runat="server" GroupName="R3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitRight1" runat="server" GroupName="R4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitRight1" runat="server" GroupName="R5D" /></td>
                                </tr>
                                <tr>
                                    <td>Bilateral</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderBilateral1" runat="server" GroupName="RSH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaBilateral1" runat="server" GroupName="RSC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmBilateral1" runat="server" GroupName="RAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmBilateral1" runat="server" GroupName="RFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandBilateral1" runat="server" GroupName="RHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristBilateral1" runat="server" GroupName="RWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitBilateral1" runat="server" GroupName="R1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitBilateral1" runat="server" GroupName="R2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitBilateral1" runat="server" GroupName="R3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitBilateral1" runat="server" GroupName="R4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitBilateral1" runat="server" GroupName="R5D" /></td>
                                </tr>
                                <tr>
                                    <td>None</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderNone1" runat="server" GroupName="RSH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaNone1" runat="server" GroupName="RSC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmNone1" runat="server" GroupName="RAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmNone1" runat="server" GroupName="RFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandNone1" runat="server" GroupName="RHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristNone1" runat="server" GroupName="RWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitNone1" runat="server" GroupName="R1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitNone1" runat="server" GroupName="R2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitNone1" runat="server" GroupName="R3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitNone1" runat="server" GroupName="R4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitNone1" runat="server" GroupName="R5D" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <label class="control-label">The neck pain is associated with</label>
                        <%--<asp:Label ID="Label2" runat="server" Style="font-weight: bold; float: left" Text="The neck pain is associated with "></asp:Label>--%>
                        <asp:CheckBox ID="chkNumbness" runat="server" Text="numbness" Checked="true" />
                        <asp:CheckBox ID="chkTingling" runat="server" Text="tingling and  " Checked="true" />
                        <asp:CheckBox ID="chkBurning" runat="server" Text="burning sensation to." /><br />
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>
                                        <asp:ImageButton ID="btnReset1" Height="30px" Width="30px" runat="server" ImageUrl="~/img/reset.png" OnClick="btnReset1_Click" /></th>
                                    <th class="table_cell">
                                        <label>Shoulder </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Scapula </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Arm </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Forearm </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Hand </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>Wrist</label></th>
                                    <th class="table_cell">
                                        <label>1st digit </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>2nd digit </label>
                                    </th>
                                    <th class="table_cell">
                                        <label>3rd digit</label></th>
                                    <th class="table_cell">
                                        <label>4th digit</label></th>
                                    <th class="table_cell">
                                        <label>5th digit </label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Left</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderLeft2" runat="server" GroupName="ASH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaLeft2" runat="server" GroupName="ASC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmLeft2" runat="server" GroupName="AAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmLeft2" runat="server" GroupName="AFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandLeft2" runat="server" GroupName="AHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristLeft2" runat="server" GroupName="AWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitLeft2" runat="server" GroupName="A1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitLeft2" runat="server" GroupName="A2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitLeft2" runat="server" GroupName="A3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitLeft2" runat="server" GroupName="A4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitLeft2" runat="server" GroupName="A5D" /></td>
                                </tr>
                                <tr>
                                    <td>Right</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderRight2" runat="server" GroupName="ASH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaRight2" runat="server" GroupName="ASC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmRight2" runat="server" GroupName="AAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmRight2" runat="server" GroupName="AFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandRight2" runat="server" GroupName="AHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristRight2" runat="server" GroupName="AWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitRight2" runat="server" GroupName="A1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitRight2" runat="server" GroupName="A2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitRight2" runat="server" GroupName="A3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitRight2" runat="server" GroupName="A4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitRight2" runat="server" GroupName="A5D" /></td>
                                </tr>
                                <tr>
                                    <td>Bilateral</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderBilateral2" runat="server" GroupName="ASH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaBilateral2" runat="server" GroupName="ASC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmBilateral2" runat="server" GroupName="AAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmBilateral2" runat="server" GroupName="AFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandBilateral2" runat="server" GroupName="AHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristBilateral2" runat="server" GroupName="AWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitBilateral2" runat="server" GroupName="A1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitBilateral2" runat="server" GroupName="A2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitBilateral2" runat="server" GroupName="A3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitBilateral2" runat="server" GroupName="A4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitBilateral2" runat="server" GroupName="A5D" /></td>
                                </tr>
                                <tr>
                                    <td>None</td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkShoulderNone2" runat="server" GroupName="ASH" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkScapulaNone2" runat="server" GroupName="ASC" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkArmNone2" runat="server" GroupName="AAR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkForearmNone2" runat="server" GroupName="AFA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkHandNone2" runat="server" GroupName="AHA" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chkWristNone2" runat="server" GroupName="AWR" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk1stDigitNone2" runat="server" GroupName="A1D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk2ndDigitNone2" runat="server" GroupName="A2D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk3rdDigitNone2" runat="server" GroupName="A3D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk4thDigitNone2" runat="server" GroupName="A4D" /></td>
                                    <td class="table_cell">
                                        <asp:RadioButton ID="chk5thDigitNone2" runat="server" GroupName="A5D" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--<div id="div_start">
          <button id="start_button" onclick="startButton(event)"><img alt="Start" id="start_img"
          src="/intl/en/chrome/assets/common/images/content/mic.gif"></button>
        </div>
        <div id="results">
          <span class="final" id="final_span"></span> <span class="interim" id=
          "interim_span"></span>
        </div>--%>
                        <asp:Label runat="server" Text="Neck pain is associated with weakness in &nbsp;" Font-Bold="False"></asp:Label>
                        <asp:TextBox ID="txtWeeknessIn" runat="server"></asp:TextBox>
                        <asp:Label runat="server" ID="lblneckpain" Text=".  Neck pain is worsened with  " Font-Bold="False"></asp:Label>
                        <asp:CheckBox ID="chkWorseSitting" runat="server" Checked="true" Text="sitting,  " />
                        <asp:CheckBox ID="chkWorseStanding" runat="server" Checked="true" Text="standing,  " />
                        <asp:CheckBox ID="chkWorseLyingDown" runat="server" Checked="true" Text="lying down,  " />
                        <asp:CheckBox ID="chkWorseMovement" runat="server" Text="movement activities,  " />
                        <asp:CheckBox ID="chkWorseSeatingtoStandingUp" runat="server" Text="going from seating to standing up,  " />
                        <asp:CheckBox ID="chkWorseWalking" runat="server" Text="walking,  " />
                        <asp:CheckBox ID="chkWorseClimbingStairs" runat="server" Text="climbing stairs,  " />
                        <asp:CheckBox ID="chkWorseDescendingStairs" runat="server" Text="descending stairs,  " />
                        <asp:CheckBox ID="chkWorseDriving" runat="server" Text="driving,  " />
                        <asp:CheckBox ID="chkWorseWorking" runat="server" Text="working,  " />
                        <asp:CheckBox ID="chkWorseBending" runat="server" Text="bending,  " />
                        <asp:CheckBox ID="chkWorseLifting" runat="server" Text="lifting,  " />
                        <asp:CheckBox ID="chkWorseTwisting" runat="server" Text="twisting " />
                        <asp:Label ID="lbland" runat="server" Text="&nbsp;and&nbsp;" />
                        <asp:TextBox ID="txtWorseOtherText" runat="server"></asp:TextBox>
                        <asp:Label ID="Label4" runat="server" Text=". Neck pain is improved with "></asp:Label>
                        <asp:CheckBox ID="chkImprovedResting" runat="server" Text="resting, " />
                        <asp:CheckBox ID="chkImprovedMedication" runat="server" Text="medication, " />
                        <asp:CheckBox ID="chkImprovedTherapy" runat="server" Text="therapy, " />
                        <asp:CheckBox ID="chkImprovedSleeping" runat="server" Text="sleeping, " />
                        <asp:CheckBox ID="chkImprovedMovement" runat="server" Text="movement. " />
                        <asp:TextBox runat="server" ID="txtFreeFormCC" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label bold">PHYSICAL EXAM:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <table>
                            <tr>
                                <th>
                                    <table style="max-width: 350px">
                                        <tr>
                                            <th>
                                                <label>Cervical spine exam</label>
                                            </th>
                                            <th>
                                                <label>ROM</label>
                                            </th>

                                            <th>
                                                <label>Normal</label>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Forward flexion</label></td>
                                            <td>
                                                <asp:TextBox ID="txtFwdFlexRight" Width="50px" runat="server"></asp:TextBox></td>
                                            <%--   <td>
                                                        <asp:TextBox ID="txtFwdFlex" runat="server" Width="25px" Text="30"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtFwdFlexNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Extension</label></td>
                                            <td>
                                                <asp:TextBox ID="txtExtensionRight" Width="50px" runat="server"></asp:TextBox></td>
                                            <%--  <td>
                                                        <asp:TextBox ID="txtExtension" Text="10" Width="25px" runat="server"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtExtensionNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </th>
                                <th>
                                    <table style="max-width: 400px">
                                        <tr>
                                            <th></th>
                                            <th>
                                                <label>Left </label>
                                            </th>
                                            <%-- <th >
                                                    </th>--%>
                                            <th>
                                                <label>Right </label>
                                            </th>
                                            <%-- <th >
                                                    </th>--%>
                                            <th>
                                                <label>Normal</label>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Rotation</label></td>
                                            <td>
                                                <asp:TextBox ID="txtRotationLeft" Width="50px" runat="server"></asp:TextBox></td>
                                            <%-- <td>
                                                        <asp:TextBox ID="txtRotationLeft" Width="25px" Text="10" runat="server"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtRotationRight" Width="50px" runat="server"></asp:TextBox></td>
                                            <%--    <td>
                                                        <asp:TextBox ID="txtRotationRight" Width="25px" runat="server" Text="10"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtRotationNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Lateral flexion</label></td>
                                            <td>
                                                <asp:TextBox ID="txtLateralFlexLeft" Width="50px" runat="server"></asp:TextBox></td>
                                            <%-- <td>
                                                        <asp:TextBox ID="txtLateralFlexLeft" Width="25px" runat="server" Text="10"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtLateralFlexRight" Width="50px" runat="server"></asp:TextBox></td>
                                            <%--    <td>
                                                        <asp:TextBox ID="txtLateralFlexRight" Width="25px" runat="server" Text="10"></asp:TextBox></td>--%>
                                            <td>
                                                <asp:TextBox ID="txtLateralFlexNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </th>
                            </tr>
                        </table>
                        <br />
                        <asp:Label Font-Bold="false" Style="float: left;" Text="The cervical spine exam reveals tenderness upon palpation at " runat="server"></asp:Label>
                        <asp:TextBox Style="float: left; height: 15px;" Width="50px" ID="txtPalpationAt" Text="C2-8" runat="server"></asp:TextBox>
                        <asp:Label Font-Bold="false" Style="float: left;" Text="&nbsp;levels&nbsp;" runat="server"></asp:Label>
                        <asp:DropDownList ID="ddlLevels" runat="server" Style="height: 30px; float: left; width: 170px">
                            <asp:ListItem Value="on the left" Text="on the left"></asp:ListItem>
                            <asp:ListItem Value="on the right" Text="on the right"></asp:ListItem>
                            <asp:ListItem Value="bilaterally" Text="bilaterally"></asp:ListItem>
                            <asp:ListItem Value="left greater than right" Text="left greater than right"></asp:ListItem>
                            <asp:ListItem Value="right greater than left" Text="right greater than left"></asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox Style="float: left; height: 15px;" Width="70px" ID="txtLevels" runat="server"></asp:TextBox>. --%>
                        <asp:Label Style="float: left;" runat="server" Text="The Spurling test is &nbsp;" Font-Bold="False"></asp:Label>
                        <asp:DropDownList Style="float: left;" ID="cboSpurlings" DataSourceID="SpurlingsXML" DataTextField="name" runat="server" Width="90px"></asp:DropDownList>
                        <asp:XmlDataSource ID="SpurlingsXML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Results/Result" />
                        <asp:Label Style="float: left;" runat="server" Text="&nbsp;. The cervical distraction test is &nbsp;" Font-Bold="False"></asp:Label>
                        <asp:DropDownList Style="float: left;" ID="cboDistraction" runat="server" DataSourceID="DistractionXML" DataTextField="name" Width="90px"></asp:DropDownList>
                        <asp:XmlDataSource ID="DistractionXML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/Results/Result" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Trigger Point:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <label>There are palpable taut bands/trigger points with referral patterns as depicted below.</label>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide1" runat="server" DataSourceID="TPSide1XML" DataTextField="name" Style="height: 30px; float: left; width: 200px"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide1XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText1" Style="float: left; margin-left: 20px;" runat="server" Text="para level C5-7, with referral patterns laterally to the region in a fan-like pattern" Width="556px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide2" Style="height: 30px; float: left; width: 200px" DataSourceID="TPSide2XML" DataTextField="name" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide2XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText2" Style="float: left; margin-left: 20px;" runat="server" Text="levator scapulae" Width="557px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide3" Style="height: 30px; float: left; width: 200px" DataSourceID="TPSide3XML" DataTextField="name" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide3XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText3" Style="float: left; margin-left: 20px;" runat="server" Text="trapezius" Width="558px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide4" Style="height: 30px; float: left; width: 200px" DataSourceID="TPSide4XML" DataTextField="name" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide4XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText4" Style="float: left; margin-left: 20px;" runat="server" Text="posterior scalenes" Width="558px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide5" Style="height: 30px; float: left; width: 200px" DataSourceID="TPSide5XML" DataTextField="name" runat="server"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide5XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText5" Style="float: left; margin-left: 20px;" runat="server" Text="sternocleidomastoid" Width="558px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide6" Style="height: 30px; float: left; width: 200px" runat="server" DataSourceID="TPSide6XML" DataTextField="name"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide6XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPText6" Style="float: left; margin-left: 20px;" runat="server" Text="splenius cervicis" Width="559px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="cboTPSide7" Style="height: 30px; float: left; width: 200px" runat="server" DataSourceID="TPSide7XML" DataTextField="name"></asp:DropDownList>
                                    <asp:XmlDataSource ID="TPSide7XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                    <asp:TextBox ID="txtTPTex7t" Style="float: left; margin-left: 20px;" runat="server" Text="splenius capitis" Width="560px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" Style="float: left;" ID="txtFreeForm" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">ASSESSMENT/DIAGNOSIS:</label>
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
                        <asp:TextBox runat="server" Style="float: left;" ID="txtFreeFormA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="basicPopup();" OnClick="AddDiag_Click" />
                        <asp:GridView ID="dgvDiagCodes" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" ReadOnly="true" runat="server" Width="400" Text='<%# Eval("Description") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">PLAN:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--  <asp:CheckBox ID="chkCervicalSpine" Style="float: left;" Text="MRI" runat="server" />
                                <asp:DropDownList ID="cboScanType" Style="float: left; height: 25px;" runat="server"></asp:DropDownList>
                                <asp:Label ID="Label7" Style="float: left;" Text=" of the cervical spine " runat="server"></asp:Label>
                                <asp:TextBox ID="txtToRuleOut" runat="server" Style="float: left; " Text="to rule out herniated nucleus pulposus/soft tissue injury " Width="299px"></asp:TextBox>--%>
                        <%--OnClick="AddStd_Click"--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormP" Style="float: left;" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddStd" Style="float: left; display: none; text-align: left;" OnClick="AddStd_Click1" runat="server"
                            ImageUrl="~/img/a1.png" Height="50px" Width="50px"
                            OnClientClick="basicPopup();" />

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:GridView ID="dgvStandards" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ProcedureDetail_ID") %>' />
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
                                <%-- <asp:TemplateField HeaderText="BodyPart" >
                                            <ItemTemplate>
                                                <label><%# Eval("BodayParts") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>--%>
                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px" TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="CC">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" runat="server" TextMode="MultiLine" Text='<%# Eval("CCDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" runat="server" TextMode="MultiLine" Text='<%# Eval("PEDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/D">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtadesc" runat="server" TextMode="MultiLine" Text='<%# Eval("ADesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdesc" runat="server" TextMode="MultiLine" Text='<%# Eval("PDesc") %>'></asp:TextBox>
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
                        <%--                   <asp:ImageButton ID="LoadDV" Style="float: left;" display="none" runat="server" OnClick="LoadDV_Click" ImageUrl="~/img/edit.gif" />--%>
                        <div style="display: none">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn blue" /></div>
                        <asp:HiddenField ID="patientID" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
   
    <script>
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
        $("#txt_day").prop('disabled', true);

        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
    </script>


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

   
    
 



 
   
                <button type="button" id="start_button" onclick="startButton(event)"><img src="images/mic.gif" alt="start" /></button>  
    <span class="final" id="final_span"></span><span class="interim" id="interim_span"></span>
           

    <script>
       
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
                $('#ctl00_ContentPlaceHolder1_txtFreeForm').text(linebreak(final_transcript));
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

    </script>
</asp:Content>
