<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/FollowUpMaster.master" CodeFile="FUNeurologicalExam.aspx.cs" Inherits="FUNeurologicalExam" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>





<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%--  <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>--%>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                  <%--  document.getElementById('<%= btnSave.ClientID %>').click();--%>
                saveDaynamicContent();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }

        function saveDaynamicContent() {
            debugger
            var htmlval = $("#ctl00_ContentPlaceHolder1_divHtml").html();
            $('#<%= hdHTMLContent.ClientID %>').val(htmlval);

             document.getElementById('<%= btnSave.ClientID %>').click();
        }
        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
    <script type="text/javascript">


        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
    </script>
    <script type="text/javascript">
        function RefreshUpdatePanelLEL3Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLEL3Left, String.Empty) %>;
        }
        function RefreshUpdatePanelLEL3Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLEL3Right, String.Empty) %>;
        }
        function RefreshUpdatePanelLEL4Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLEL4Left, String.Empty) %>;
        }
        function RefreshUpdatePanelLEL4Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLEL4Right, String.Empty) %>;
        }
        function RefreshUpdatePanelLEL5Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLEL5Left, String.Empty) %>;
        }
        function RefreshUpdatePanelLEL5Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLEL5Right, String.Empty) %>;
        }
        function RefreshUpdatePanelLES1Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLES1Left, String.Empty) %>;
        }
        function RefreshUpdatePanelLES1Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLES1Right, String.Empty) %>;
        }
        function RefreshUpdatePanelLELumberParaspinalsLeft() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLELumberParaspinalsLeft , String.Empty) %>;
        }
        function RefreshUpdatePanelLELumberParaspinalsRight() {
            <%= Page.ClientScript.GetPostBackEventReference(txtLELumberParaspinalsRight, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC5Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC5Left, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC5Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC5Right, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC6Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC6Left, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC6Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC6Right, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC7Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC7Left, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC7Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC7Right, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC8Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC8Left, String.Empty) %>;
        }
        function RefreshUpdatePanelUEC8Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC8Right, String.Empty) %>;
        }
        function RefreshUpdatePanelUET1Left() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUET1Left, String.Empty) %>;
        }
        function RefreshUpdatePanelUET1Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUET1Right, String.Empty) %>;
        }
        function RefreshUpdatePanelUECervicalParaspinalsLeft() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUECervicalParaspinalsLeft, String.Empty) %>;
        }
        function RefreshUpdatePanelUECervicalParaspinalsRight() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUECervicalParaspinalsRight, String.Empty) %>;
        };
    </script>

    <%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">--%>
    <div>
        <%--<ul class="breadcrumb">
            <li>
                <i class="icon-home"></i>
                <a href="Page1.aspx"><span class="label">Page1</span></a>
            </li>
            <li id="lipage2">
                <i class="icon-edit"></i>
                <a href="Page2.aspx"><span class="label">Page2</span></a>
            </li>
            <li id="li1" runat="server" enable="false">
                <i class="icon-edit"></i>
                <a href="Page3.aspx"><span class="label label-success">Page3</span></a>
            </li>
            <li id="li2" runat="server" enable="false">
                <i class="icon-edit"></i>
                <a href="Page4.aspx"><span class="label">Page4</span></a>
            </li>
        </ul>--%>
    </div>
    <%--</asp:Content>--%>
    <%--<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">--%>
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

    <asp:UpdatePanel runat="server" ID="up_complains">
        <ContentTemplate>

            <strong>Neurological Exam:</strong>

            <asp:HiddenField runat="server" ID="hdHTMLContent" />

            <div runat="server" id="divHtml">
            </div>


            <div style="display: none">
                <p>
                    The patient is alert and cooperative and responding appropriately. Cranial nerves - II-XII are grossly intact except
                    <asp:TextBox ID="txtIntactExcept" runat="server"></asp:TextBox>
                </p>
                <p>
                    Deep Tendon Reflexes: 
                   <table>
                       <tr>
                           <td>
                               <table style="margin-right: 50px;">
                                   <thead>
                                       <tr>
                                           <td>
                                               <asp:CheckBox ID="UEdtr" runat="server" Text=" Upper Extremity    " /></td>
                                           <td>Left</td>
                                           <td>Right</td>
                                       </tr>
                                   </thead>
                                   <tbody>
                                       <tr>
                                           <td>Triceps</td>
                                           <td>
                                               <asp:TextBox ID="txtDTRtricepsLeft" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="txtDTRtricepsRight" Width="50" runat="server"></asp:TextBox></td>


                                       </tr>
                                       <tr>
                                           <td>Biceps</td>
                                           <td>
                                               <asp:TextBox ID="txtDTRBicepsLeft" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="txtDTRBicepsRight" Width="50" runat="server"></asp:TextBox></td>
                                       </tr>
                                       <tr>
                                           <td>Brachioradialis</td>
                                           <td>
                                               <asp:TextBox ID="txtDTRBrachioLeft" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox Width="50" ID="txtDTRBrachioRight" runat="server"></asp:TextBox></td>

                                       </tr>
                                   </tbody>
                               </table>
                           </td>
                           <td></td>
                           <td>
                               <table style="margin-top: -40px; margin-right: 50px;">
                                   <thead>
                                       <tr>
                                           <td>
                                               <asp:CheckBox ID="LEdtr" runat="server" Text=" Lower Extremity    " /></td>
                                           <td>Left</td>
                                           <td>Right</td>
                                       </tr>
                                   </thead>
                                   <tbody>
                                       <tr>
                                           <td>Knee</td>
                                           <td>
                                               <asp:TextBox ID="txtDTRKneeLeft" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="txtDTRKneeRight" Width="50" runat="server"></asp:TextBox></td>


                                       </tr>
                                       <tr>
                                           <td>Ankle</td>
                                           <td>
                                               <asp:TextBox ID="txtDTRAnkleLeft" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="txtDTRAnkleRight" Width="50" runat="server"></asp:TextBox></td>
                                       </tr>
                                       <%--      <tr>
                                           <td></td>
                                           <td>
                                               <asp:TextBox ID="TextBox1" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="TextBox2" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                       </tr>--%>
                                   </tbody>
                               </table>
                           </td>
                       </tr>
                   </table>

                </p>
                <p>
                    <hr />
                    Sensory:  Is checked by 
                    <asp:CheckBox ID="chkPinPrick" runat="server" />
                    light touch. It is
                   <%-- <asp:CheckBox Text="" runat="server" ID="chkLighttouch" />--%>
                    <asp:TextBox runat="server" ID="txtSensory" Width="620px"></asp:TextBox>
                    <button type="button" id="start_button" onclick="startButton(event)">
                        <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                    <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                    <table>
                        <tr>
                            <td>
                                <table style="margin-right: 50px;">
                                    <thead>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="UESen" runat="server" Text=" Upper Extremity    " /></td>
                                            <td>Left</td>
                                            <td>Right</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Lateral arm (C5)</td>
                                            <td>
                                                <asp:TextBox ID="txtUEC5Left" Width="100" onkeyup="RefreshUpdatePanelUEC5Left();" AutoPostBack="true" OnTextChanged="txtUEC5Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUEC5Right" Width="100" onkeyup="RefreshUpdatePanelUEC5Right();" AutoPostBack="true" OnTextChanged="txtUEC5Right_TextChanged" runat="server"></asp:TextBox></td>


                                        </tr>
                                        <tr>
                                            <td>Lateral forearm, thumb, index (C6)</td>
                                            <td>
                                                <asp:TextBox ID="txtUEC6Left" Width="100" onkeyup="RefreshUpdatePanelUEC6Left();" AutoPostBack="true" OnTextChanged="txtUEC6Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUEC6Right" Width="100" onkeyup="RefreshUpdatePanelUEC6Right();" AutoPostBack="true" OnTextChanged="txtUEC6Right_TextChanged" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Middle finger (C7)</td>
                                            <td>
                                                <asp:TextBox ID="txtUEC7Left" Width="100" onkeyup="RefreshUpdatePanelUEC7Left();" AutoPostBack="true" OnTextChanged="txtUEC7Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUEC7Right" Width="100" onkeyup="RefreshUpdatePanelUEC7Right();" AutoPostBack="true" OnTextChanged="txtUEC7Right_TextChanged" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Medial forearm, ring, little finger (C8)</td>
                                            <td>
                                                <asp:TextBox ID="txtUEC8Left" Width="100" onkeyup="RefreshUpdatePanelUEC8Left();" AutoPostBack="true" OnTextChanged="txtUEC8Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUEC8Right" Width="100" onkeyup="RefreshUpdatePanelUEC8Right();" AutoPostBack="true" OnTextChanged="txtUEC8Right_TextChanged" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Medial arm (T1)</td>
                                            <td>
                                                <asp:TextBox ID="txtUET1Left" Width="100" onkeyup="RefreshUpdatePanelUET1Left();" AutoPostBack="true" OnTextChanged="txtUET1Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUET1Right" Width="100" onkeyup="RefreshUpdatePanelUET1Right();" AutoPostBack="true" OnTextChanged="txtUET1Right_TextChanged" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Cervical paraspinals</td>
                                            <td>
                                                <asp:TextBox ID="txtUECervicalParaspinalsLeft" Width="100" onkeyup="RefreshUpdatePanelUECervicalParaspinalsLeft();" AutoPostBack="true" OnTextChanged="txtUECervicalParaspinalsLeft_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUECervicalParaspinalsRight" Width="100" onkeyup="RefreshUpdatePanelUECervicalParaspinalsRight();" AutoPostBack="true" OnTextChanged="txtUECervicalParaspinalsRight_TextChanged" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </tbody>
                                </table>

                            </td>
                            <td></td>
                            <td>
                                <table style="margin-top: -40px; margin-right: 50px;">
                                    <thead>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="LESen" runat="server" Text=" Lower Extremity    " /></td>
                                            <td>Left</td>
                                            <td>Right</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Distal medial thigh (L3)</td>
                                            <td>
                                                <asp:TextBox ID="txtLEL3Left" Width="100" onkeyup="RefreshUpdatePanelLEL3Left();" AutoPostBack="true" OnTextChanged="txtLEL3Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtLEL3Right" Width="100" onkeyup="RefreshUpdatePanelLEL3Right();" AutoPostBack="true" OnTextChanged="txtLEL3Right_TextChanged1" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Medial left foot (L4)</td>
                                            <td>

                                                <asp:TextBox ID="txtLEL4Left" Width="100" onkeyup="RefreshUpdatePanelLEL4Left();" AutoPostBack="true" OnTextChanged="txtLEL4Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtLEL4Right" Width="100" onkeyup="RefreshUpdatePanelLEL4Right();" AutoPostBack="true" OnTextChanged="txtLEL4Right_TextChanged" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Dorsum of the foot (L5)</td>
                                            <td>
                                                <asp:TextBox ID="txtLEL5Left" Width="100" onkeyup="RefreshUpdatePanelLEL5Left();" AutoPostBack="true" OnTextChanged="txtLEL5Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtLEL5Right" Width="100" onkeyup="RefreshUpdatePanelLEL5Right();" AutoPostBack="true" OnTextChanged="txtLEL5Right_TextChanged" runat="server"></asp:TextBox></td>

                                        </tr>
                                        <tr>
                                            <td>Lateral foot (S1)</td>
                                            <td>
                                                <asp:TextBox ID="txtLES1Left" Width="100" onkeyup="RefreshUpdatePanelLES1Left();" AutoPostBack="true" OnTextChanged="txtLES1Left_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtLES1Right" Width="100" onkeyup="RefreshUpdatePanelLES1Right();" AutoPostBack="true" OnTextChanged="txtLES1Right_TextChanged" runat="server"></asp:TextBox></td>

                                        </tr>
                                        <%--   <tr><td>Brachioradialis</td><td><asp:TextBox Width="25" ID="TextBox22" runat="server"></asp:TextBox></td>
                               <td><asp:TextBox ID="TextBox23" Width="25" runat="server"></asp:TextBox></td>

                           </tr>--%>
                                        <tr>
                                            <td>Lumbar paraspinals</td>
                                            <td>
                                                <asp:TextBox ID="txtLELumberParaspinalsLeft" Width="100" onkeyup="RefreshUpdatePanelLELumberParaspinalsLeft();" AutoPostBack="true" OnTextChanged="txtLELumberParaspinalsLeft_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtLELumberParaspinalsRight" Width="100" onkeyup="RefreshUpdatePanelLELumberParaspinalsRight();" AutoPostBack="true" OnTextChanged="txtLELumberParaspinalsRight_TextChanged" runat="server"></asp:TextBox></td>

                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </p>
                <p>
                    Hoffman's exam:
                    <asp:DropDownList ID="cboHoffmanexam" runat="server">
                        <asp:ListItem Value=" "> </asp:ListItem>
                        <asp:ListItem Value="Positive">Positive</asp:ListItem>
                        <asp:ListItem Value="Negative">Negative</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkStocking" runat="server" Text=" stocking pattern  " />
                    <asp:CheckBox ID="chkGlove" runat="server" Text=" glove pattern." />
                </p>
                <p>
                    <hr />
                    Manual Muscle Strength Testing: 
                      <table>
                          <tr>
                              <td>
                                  <table style="margin-right: 50px;">
                                      <thead>
                                          <tr>
                                              <td>
                                                  <asp:CheckBox ID="UEmmst" runat="server" Text=" Upper Extremity    " /></td>
                                              <td></td>
                                              <td>Left</td>
                                              <td>Right</td>
                                          </tr>
                                      </thead>
                                      <tbody>
                                          <tr>
                                              <td>Shoulder</td>
                                              <td>Abduction</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEShoulderAbductionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEShoulderAbductionRight" Width="50" runat="server"></asp:TextBox></td>


                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Flexion</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEShoulderFlexionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEShoulderFlexionRight" Width="50" runat="server"></asp:TextBox></td>


                                          </tr>
                                          <tr>
                                              <td>Elbow</td>
                                              <td>Extension</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowExtensionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowExtensionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Flexion</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowFlexionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowFlexionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Supination</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowSupinationLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowSupinationRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Pronation</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowPronationLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEElbowPronationRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td>Wrist</td>
                                              <td>Flexion</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEWristFlexionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEWristFlexionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Extension</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEWristExtensionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEWristExtensionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td>Hand</td>
                                              <td>Grip strength</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEHandGripStrengthLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEHandGripStrengthRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td>Hand</td>
                                              <td>Finger abduction</td>
                                              <td>
                                                  <asp:TextBox ID="txtUEHandFingerAbductorsLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtUEHandFingerAbductorsRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <%--     <tr>
                                              <td></td>
                                              <td>
                                                  <asp:TextBox ID="TextBox34" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="TextBox35" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>--%>
                                      </tbody>
                                  </table>

                              </td>
                              <td></td>
                              <td>
                                  <table style="margin-top: -120px">
                                      <thead>
                                          <tr>
                                              <td>
                                                  <asp:CheckBox ID="LEmmst" runat="server" Text=" Lower Extremity    " /></td>
                                              <td></td>
                                              <td>Left</td>
                                              <td>Right</td>
                                          </tr>
                                      </thead>
                                      <tbody style="padding: 5px">
                                          <tr>
                                              <td>Hip</td>
                                              <td>Flexion</td>
                                              <td>
                                                  <asp:TextBox ID="txtLEHipFlexionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEHipFlexionRight" Width="50" runat="server"></asp:TextBox></td>


                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Abduction</td>
                                              <td>
                                                  <asp:TextBox ID="txtLEHipAbductionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEHipAbductionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td>Knee</td>
                                              <td>Extension</td>
                                              <td>
                                                  <asp:TextBox ID="txtLEKneeExtensionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEKneeExtensionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Flexion</td>
                                              <td>
                                                  <asp:TextBox ID="txtLEKneeFlexionLeft" Width="50" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEKneeFlexionRight" Width="50" runat="server"></asp:TextBox></td>
                                          </tr>
                                          <tr>
                                              <td>Ankle</td>
                                              <td>Dorsiflexion</td>
                                              <td>
                                                  <asp:TextBox Width="50" ID="txtLEAnkleDorsiLeft" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEAnkleDorsiRight" Width="50" runat="server"></asp:TextBox></td>

                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Plantar flexion</td>
                                              <td>
                                                  <asp:TextBox Width="50" ID="txtLEAnklePlantarLeft" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEAnklePlantarRight" Width="50" runat="server"></asp:TextBox></td>

                                          </tr>
                                          <tr>
                                              <td></td>
                                              <td>Extensor hallucis longus</td>
                                              <td>
                                                  <asp:TextBox Width="50" ID="txtLEAnkleExtensorLeft" runat="server"></asp:TextBox></td>
                                              <td>
                                                  <asp:TextBox ID="txtLEAnkleExtensorRight" Width="50" runat="server"></asp:TextBox></td>

                                          </tr>
                                      </tbody>
                                  </table>
                              </td>
                          </tr>
                      </table>

                </p>
                <%-- <strong>Work Status Comments:</strong>
               <asp:TextBox ID="workStatusCmnts" TextMode="multiline" Columns="500" Rows="5"  runat="server"></asp:TextBox>--%>
                <div style="clear: both"></div>
            </div>
            <br />
            <%--<asp:Button runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ID="btnSave" UseSubmitBehavior="False" />
            <asp:Button runat="server" ID="Button1" PostBackUrl="~/PatientIntakeList.aspx" Text="Back to List" CssClass="btn btn-default" UseSubmitBehavior="False" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ID="btnSave" UseSubmitBehavior="False" />
    </div>
    <asp:Button runat="server" ID="Button1" PostBackUrl="~/PatientIntakeList.aspx" Text="Back to List" CssClass="btn btn-default" UseSubmitBehavior="False" />
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
                $('#<%=txtSensory.ClientID%>').text(linebreak(final_transcript));
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
            start_timestamp = event.timeStamp;
        }

    </script>
</asp:Content>

