<%@ Page Title="" Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Page4.aspx.cs" Inherits="Page4" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetGaitcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>

    <%--<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script> --%>
    <script type="text/javascript">
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
        function toggel() {


            if ($('#demo').attr('class') == 'collapse') {
                $("#demo").addClass("in");
                //$('#demo').attr('class') = 'collapse in';
            }
            else {
                $('#demo').removeClass("in");
            }

        }
    </script>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
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

            htmlval = $("#ctl00_ContentPlaceHolder1_divtopHtml").html();
            $('#<%= hdtopHTMLContent.ClientID %>').val(htmlval);

            document.getElementById('<%= btnSave.ClientID %>').click();
        }
        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
    <%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">--%>
    <%--  <div>
        <ul class="breadcrumb">
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
                <a href="Page3.aspx"><span class="label">Page3</span></a>
            </li>
            <li id="li2" runat="server" enable="false">
                <i class="icon-edit"></i>
                <a href="Page4.aspx"><span class="label label-success">Page4</span></a>
            </li>
        </ul>
        <asp:LinkButton ID="lbtnProcedureDetails" CssClass="procDetail" runat="server" OnClick="lbtnProcedureDetails_Click">Procedure Details</asp:LinkButton>
    </div>
</asp:Content>--%>
    <%--<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    --%>
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
            <div style="display: none">

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


                <div>
                    <br />
                    <strong class="labelcolor">Neurological Exam:</strong>
                    <p>
                        The patient is alert and cooperative and responding appropriately. Cranial nerves - II-XII are grossly intact except
                    <asp:TextBox ID="txtIntactExcept" TabIndex="32" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        Deep Tendon Reflexes: 
                   <table>
                       <tr>
                           <td>
                               <table style="margin-right: 50px">
                                   <thead>
                                       <tr>
                                           <td>
                                               <asp:CheckBox ID="UExchk" TabIndex="34" runat="server" Text=" Upper Extremity    " /></td>
                                           <td>Left</td>
                                           <td>Right</td>
                                       </tr>
                                   </thead>
                                   <tbody>
                                       <tr>
                                           <td>Triceps</td>
                                           <td>
                                               <asp:TextBox ID="LTricepstxt" TabIndex="35" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="RTricepstxt" TabIndex="36" Width="50" runat="server"></asp:TextBox></td>


                                       </tr>
                                       <tr>
                                           <td>Biceps</td>
                                           <td>
                                               <asp:TextBox ID="LBicepstxt" TabIndex="37" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="RBicepstxt" TabIndex="38" Width="50" runat="server"></asp:TextBox></td>
                                       </tr>
                                       <tr>
                                           <td>Brachioradialis</td>
                                           <td>
                                               <asp:TextBox ID="LBrachioradialis" TabIndex="39" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox Width="50" TabIndex="38" ID="RBrachioradialis" runat="server"></asp:TextBox></td>
                                       </tr>
                                   </tbody>
                               </table>
                           </td>
                           <td></td>
                           <td>
                               <table style="margin-top: -40px">
                                   <thead>
                                       <tr>
                                           <td>
                                               <asp:CheckBox ID="LEdtr" TabIndex="40" runat="server" Text=" Lower Extremity    " /></td>
                                           <td>Left</td>
                                           <td>Right</td>
                                       </tr>
                                   </thead>
                                   <tbody>
                                       <tr>
                                           <td>Knee</td>
                                           <td>
                                               <asp:TextBox ID="LKnee" TabIndex="41" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="RKnee" TabIndex="42" Width="50" runat="server"></asp:TextBox></td>


                                       </tr>
                                       <tr>
                                           <td>Ankle</td>
                                           <td>
                                               <asp:TextBox ID="LAnkle" TabIndex="43" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="RAnkle" TabIndex="44" Width="50" runat="server"></asp:TextBox></td>
                                       </tr>
                                       <tr>
                                           <td></td>
                                           <td>
                                               <asp:TextBox ID="TextBox1" TabIndex="45" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                           <td>
                                               <asp:TextBox ID="TextBox2" TabIndex="46" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                       </tr>
                                   </tbody>
                               </table>
                           </td>
                       </tr>
                   </table>
                    </p>
                    <hr />
                    <b class="labelcolor">Sensory:</b>  Is checked by 
                    <asp:CheckBox ID="chkPinPrick" TabIndex="47" runat="server" />
                    pinprick
                    <asp:CheckBox Text="" runat="server" ID="chkLighttouch" />
                    light touch. It is
                <asp:TextBox runat="server" TabIndex="48" Width="720px" ID="txtSensory"></asp:TextBox>
                    <button type="button" id="start_button" onclick="startButton(event)">
                        <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                    <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                    <table>
                        <tr>
                            <td>
                                <table style="margin-right: 50px; margin-top: 5px;">
                                    <thead>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="UESen_Click" runat="server" TabIndex="59" Text=" Upper Extremity    " /></td>
                                            <td>Left</td>
                                            <td>Right</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Lateral arm (C5)</td>
                                            <td>
                                                <asp:TextBox ID="TextBox9" onkeyup="RefreshUpdatePanel9();" TabIndex="60" AutoPostBack="false" OnTextChanged="TextBox9_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtUEC5Right" onkeyup="RefreshUpdatePanelC5Right();MenuHighlight();" TabIndex="61" OnTextChanged="txtUEC5Right_TextChanged" Width="100" runat="server"></asp:TextBox></td>


                                        </tr>
                                        <tr>
                                            <td>Lateral forearm, thumb, index (C6)</td>
                                            <td>
                                                <asp:TextBox ID="TextBox11" onkeyup="RefreshUpdatePanel11();MenuHighlight();" TabIndex="62" OnTextChanged="TextBox11_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox12" onkeyup="RefreshUpdatePanel12();MenuHighlight();" TabIndex="63" OnTextChanged="TextBox12_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Middle finger (C7)</td>
                                            <td>
                                                <asp:TextBox ID="TextBox13" onkeyup="RefreshUpdatePanel13();MenuHighlight();" TabIndex="64" OnTextChanged="TextBox13_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox14" onkeyup="RefreshUpdatePanel14();MenuHighlight();" TabIndex="65" OnTextChanged="TextBox14_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Medial forearm, ring, little finger (C8)</td>
                                            <td>
                                                <asp:TextBox ID="TextBox15" onkeyup="RefreshUpdatePanel15();MenuHighlight();" TabIndex="66" OnTextChanged="TextBox15_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox16" onkeyup="RefreshUpdatePanel16();MenuHighlight();" TabIndex="67" OnTextChanged="TextBox16_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Medial arm (T1)</td>
                                            <td>
                                                <asp:TextBox ID="TextBox17" onkeyup="RefreshUpdatePanel17();MenuHighlight();" TabIndex="68" OnTextChanged="TextBox17_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox18" onkeyup="RefreshUpdatePanel18();MenuHighlight();" TabIndex="69" OnTextChanged="TextBox18_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Cervical paraspinals</td>
                                            <td>
                                                <asp:TextBox ID="TextBox19" onkeyup="RefreshUpdatePanel19();MenuHighlight();" TabIndex="70" OnTextChanged="TextBox19_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox20" onkeyup="RefreshUpdatePanel20();MenuHighlight();" TabIndex="71" OnTextChanged="TextBox20_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                        </tr>
                                    </tbody>
                                </table>

                            </td>
                            <td></td>
                            <td>
                                <table style="margin-top: -20px">
                                    <thead>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="LESen_Click" TabIndex="49" runat="server" Text=" Lower Extremity    " /></td>
                                            <td>Left</td>
                                            <td>Right</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Distal medial thigh (L3)</td>
                                            <td>
                                                <asp:TextBox ID="txtDMTL3" Width="100" TabIndex="50" onkeyup="RefreshUpdatePanelL3();MenuHighlight();" OnTextChanged="txtDMTL3_TextChanged" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox4" Width="100" TabIndex="51" onkeyup="RefreshUpdatePanel4();MenuHighlight();" OnTextChanged="TextBox4_TextChanged" runat="server"></asp:TextBox></td>

                                        </tr>
                                        <tr>
                                            <td>Medial left foot (L4)</td>
                                            <td>
                                                <asp:TextBox ID="TextBox5" onkeyup="RefreshUpdatePanel5();MenuHighlight();" TabIndex="52" OnTextChanged="TextBox5_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox6" onkeyup="RefreshUpdatePanel6();MenuHighlight();" TabIndex="53" OnTextChanged="TextBox6_TextChanged" Width="100" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>Dorsum of the foot (L5)</td>
                                            <td>
                                                <asp:TextBox Width="100" onkeyup="RefreshUpdatePanel7();MenuHighlight();" TabIndex="53" OnTextChanged="TextBox7_TextChanged" ID="TextBox7" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox8" onkeyup="RefreshUpdatePanel8();MenuHighlight();" TabIndex="54" OnTextChanged="TextBox8_TextChanged" Width="100" runat="server"></asp:TextBox></td>

                                        </tr>
                                        <tr>
                                            <td>Lateral foot (S1)</td>
                                            <td>
                                                <asp:TextBox Width="100" onkeyup="RefreshUpdatePanel10();MenuHighlight();" TabIndex="55" OnTextChanged="TextBox10_TextChanged" ID="TextBox10" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox21" onkeyup="RefreshUpdatePanel21();MenuHighlight();" TabIndex="56" OnTextChanged="TextBox21_TextChanged" Width="100" runat="server"></asp:TextBox></td>

                                        </tr>
                                        <%--   <tr><td>Brachioradialis</td><td><asp:TextBox Width="25" ID="TextBox22" runat="server"></asp:TextBox></td>
                               <td><asp:TextBox ID="TextBox23" Width="25" runat="server"></asp:TextBox></td>

                           </tr>--%>
                                        <tr>
                                            <td>Lumbar paraspinals</td>
                                            <td>
                                                <asp:TextBox Width="100" onkeyup="RefreshUpdatePanel24();MenuHighlight();" TabIndex="57" OnTextChanged="TextBox24_TextChanged" ID="TextBox24" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="TextBox25" onkeyup="RefreshUpdatePanel25();MenuHighlight();" TabIndex="58" OnTextChanged="TextBox25_TextChanged" Width="100" runat="server"></asp:TextBox></td>

                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <hr />
                

                    <p>
                        <b class="labelcolor">Manual Muscle Strength Testing: </b>
                        <table>
                            <tr>
                                <td>
                                    <table style="margin-right: 50px;">
                                        <thead>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="UEmmst" TabIndex="90" runat="server" Text=" Upper Extremity    " /></td>
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
                                                    <asp:TextBox ID="TextBox30" TabIndex="91" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox31" TabIndex="92" Width="50" runat="server"></asp:TextBox></td>


                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Flexion</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox48" TabIndex="93" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox49" TabIndex="94" Width="50" runat="server"></asp:TextBox></td>


                                            </tr>
                                            <tr>
                                                <td>Elbow</td>
                                                <td>Extension</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox32" TabIndex="95" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox33" TabIndex="96" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Flexion</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox50" TabIndex="97" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox51" TabIndex="98" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Supination</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox52" TabIndex="99" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox53" TabIndex="100" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Pronation</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox54" TabIndex="101" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox55" Width="50" TabIndex="102" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Wrist</td>
                                                <td>Flexion</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox36" TabIndex="103" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox37" TabIndex="104" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Extension</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox56" TabIndex="105" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox57" Width="50" TabIndex="106" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Hand</td>
                                                <td>Grip strength</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox38" TabIndex="107" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox39" TabIndex="108" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Hand</td>
                                                <td>Finger abduction</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox58" TabIndex="109" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox59" TabIndex="110" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox34" TabIndex="111" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox35" TabIndex="112" Visible="false" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                        </tbody>
                                    </table>

                                </td>
                                <td></td>
                                <td>
                                    <table style="margin-top: -100px">
                                        <thead>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="LEmmst" TabIndex="75" runat="server" Text=" Lower Extremity    " /></td>
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
                                                    <asp:TextBox ID="TextBox22" TabIndex="76" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox23" TabIndex="77" Width="50" runat="server"></asp:TextBox></td>


                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Abduction</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox40" TabIndex="78" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox41" Width="50" TabIndex="79" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Knee</td>
                                                <td>Extension</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox26" TabIndex="80" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox27" TabIndex="81" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Flexion</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox42" TabIndex="82" Width="50" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox43" TabIndex="83" Width="50" runat="server"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Ankle</td>
                                                <td>Dorsiflexion</td>
                                                <td>
                                                    <asp:TextBox Width="50" TabIndex="84" ID="TextBox28" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox29" TabIndex="85" Width="50" runat="server"></asp:TextBox></td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Plantar flexion</td>
                                                <td>
                                                    <asp:TextBox Width="50" TabIndex="86" ID="TextBox44" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox45" TabIndex="87" Width="50" runat="server"></asp:TextBox></td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>Extensor hallucis longus</td>
                                                <td>
                                                    <asp:TextBox Width="50" TabIndex="88" ID="TextBox46" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:TextBox ID="TextBox47" TabIndex="89" Width="50" runat="server"></asp:TextBox></td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </p>
                </div>
            </div>

              
            <asp:HiddenField runat="server" ID="hdtopHTMLContent" />
            <asp:HiddenField runat="server" ID="hdHTMLContent" />

            <div runat="server" id="divtopHtml">
            </div>

            <div runat="server" id="divHtml">
            </div>



            <div style="display: none">
                <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" TabIndex="113" OnClick="btnSave_Click" ID="btnSave" UseSubmitBehavior="False" />
            </div>
            <asp:Button runat="server" ID="Button1" PostBackUrl="~/PatientIntakeList.aspx" Text="Back to List" CssClass="btn btn-default" UseSubmitBehavior="False" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function RefreshUpdatePanelL3() {
            <%= Page.ClientScript.GetPostBackEventReference(txtDMTL3, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel4() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox4, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel5() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox5, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel6() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox6, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel7() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox7, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel8() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox8, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel10() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox10, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel21() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox21, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel24() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox24, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel25() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox25, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel9() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox9, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanelC5Right() {
            <%= Page.ClientScript.GetPostBackEventReference(txtUEC5Right, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel11() {
            <%= Page.ClientScript.GetPostBackEventReference(txtDMTL3, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel12() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox12, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel13() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox13, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel14() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox14, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel15() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox15, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel16() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox16, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel17() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox17, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel18() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox18, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel19() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox19, String.Empty) %>;
            MenuHighlight();
        }
        function RefreshUpdatePanel20() {
            <%= Page.ClientScript.GetPostBackEventReference(TextBox20, String.Empty) %>;
            MenuHighlight();
        }

        //function MenuHighlight() {
        //    alert('call');
        //    $('#ctl00_page4').addClass('active');
        //}
    </script>
</asp:Content>

