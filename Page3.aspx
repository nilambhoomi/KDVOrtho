<%@ Page Title="" Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Page3.aspx.cs" Inherits="Page3" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>



    <script language="javascript">


        function toggel() {


            if ($('#demo').attr('class') === 'collapse') {
                $("#demo").addClass("in");
                //$('#demo').attr('class') = 'collapse in';
            }
            else {
                $('#demo').removeClass("in");
            }

        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            MenuHighlight();
        }
        var postbackElement = null;
        function RestoreFocus(source, args) {
            document.getElementById(postbackElement.id).focus();
            enableMenu();
        }
        function SavePostbackElement(source, args) {
            postbackElement = args.get_postBackElement();
            enableMenu();
        }
        function AddRequestHandler() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(RestoreFocus);
            prm.add_beginRequest(SavePostbackElement);
            enableMenu();


        }
        function enableMenu() {

            //debugger;
            var current = location.pathname;
            var curpage = current.substr(current.lastIndexOf('/') + 1);

            $('#nav li a').each(function () {
                var $this = $(this);
                //alert(curpage);

                // if the current path is like this link, make it active
                if ($this.attr('href').indexOf(curpage) !== -1) {
                    $(this).parent('li').addClass('active');
                    //$this.addClass('active');
                }

            })

        }
    </script>
    <style>
        .labelcolor {
            color: black;
            font-weight: bold;
            font-size: 16px;
        }
    </style>
    <script type="text/javascript">
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }



        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //alert();
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
             <%--   document.getElementById('<%= btnSave.ClientID %>').click();--%>
                saveDaynamicContent();
            }
            else {
                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }


        function saveDaynamicContent() {
            debugger
            var htmlval = $("#ctl00_ContentPlaceHolder1_divtopHTML").html();
            $('#<%= hdtopHTMLContent.ClientID %>').val(htmlval);

            htmlval = $("#ctl00_ContentPlaceHolder1_divdegreeHTML").html();
            $('#<%= hddegreeHTMLContent.ClientID %>').val(htmlval);

            htmlval = $("#ctl00_ContentPlaceHolder1_divrosHTML").html();
            $('#<%= hdrosHTMLContent.ClientID %>').val(htmlval);


            document.getElementById('<%= btnSave.ClientID %>').click();
        }

        function saveall() {
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
    <asp:HiddenField runat="server" ID="hdrosHTMLContent" />
    <asp:HiddenField runat="server" ID="hdtopHTMLContent" />
    <asp:HiddenField runat="server" ID="hddegreeHTMLContent" />

    <b>Review Of Systems:</b>
    <CKEditor:CKEditorControl ID="txtROS" BasePath="ckeditor/" runat="server" Text="">
    </CKEditor:CKEditorControl>

    <div id="divrosHTML" runat="server" style="display: none"></div>
    <br />
    <div id="divtopHTML" runat="server"></div>
    <br />
    <div id="divdegreeHTML" runat="server"></div>
    <br />



    <div style="display: none">
        <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ID="btnSave" />
    </div>
    <asp:Button runat="server" ID="Button3" PostBackUrl="~/PatientIntakeList.aspx" Text="Back to List" CssClass="btn btn-default" UseSubmitBehavior="False" />



    <asp:UpdatePanel runat="server" ID="up_complains" Visible="false">
        <ContentTemplate>
            <h4 class="labelcolor">ROS</h4>
            <hr />
            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="1" ID="chk_abdominal_pain" Text="Abdominal Pain" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="2" ID="chk_blurred" Text="Blurred Vision" />
                    &nbsp;/ Double Vision
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="3" ID="chk_bowel_bladder" Text="Bowel/Bladder Incontinence" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="4" ID="chk_chest_pain" Text="Chest Pain" />
                </div>
                <div style="clear: both"></div>
            </div>
            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="5" ID="chk_diarrhea" Text="Diarrhea" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="6" ID="chk_episodic_ligth" Text="Episodic Light Headedness" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="7" ID="chk_fever" Text="Fever" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="8" ID="chk_hearing_loss" Text="Hearing Loss" />
                </div>

                <div style="clear: both"></div>
            </div>
            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="9" ID="chk_recent_wt" Text="Recent wt.loss" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="10" ID="chk_seizures" Text="Seizures" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="11" ID="chk_shortness_of_breath" Text="Shortness of Breath" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="12" ID="chk_sleep_disturbance" Text="Sleep Disturbance / Night Sweats" />
                </div>
                <div style="clear: both"></div>
            </div>
            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="13" ID="chk_jaw_pain" Text="jaw pain/clicking" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="14" ID="chk_bloodinurine" Text="blood in urine" />
                </div>
                <div style="clear: both"></div>
            </div>
            <h4 class="labelcolor">Complaints</h4>
            <hr />

            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="15" ID="chk_depression" Text="Depression " />
                </div>

                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="16" ID="chk_dizziness" Text="Dizziness" />
                </div>

                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="17" ID="chk_headaches" Text="Headaches" />
                </div>

                <%--<div class="span3">
                    <asp:CheckBox runat="server" TabIndex="18" ID="chk_jaw_pain" Text="Jaw Pain/Clicking" />
                </div>--%>
                <div style="clear: both"></div>
            </div>
            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="19" ID="chk_nausea" Text="Nausea" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="20" ID="chk_numbness_in_arm" Text="Numbness in Arm / Hand" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="21" ID="chk_numbess_in_leg" Text="Numbness in Leg" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="22" ID="chk_pain_radiating_leg" Text="Pain Radiating Down Leg" />
                </div>
                <div style="clear: both"></div>
            </div>

            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="23" ID="chk_pain_radiating_shoulder" Text="Pain Radiating Down Shoulder" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="24" ID="chk_rashes" Text="Rashes" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="25" ID="chk_anxiety" Text="Anxiety" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="26" ID="chk_tingling_in_arms" Text="Tingling in Arms" />
                </div>
                <div style="clear: both"></div>
            </div>
            <div class="form-horizontal">
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="27" ID="chk_tingling_in_legs" Text="Tingling in Legs" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="28" ID="chk_vomiting" Text="Vomiting" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="29" ID="chk_weakness_in_arm" Text="Weakness in Arm / Hand" />
                </div>
                <div class="span3">
                    <asp:CheckBox runat="server" TabIndex="30" ID="chk_weakness_in_leg" Text="Weakness in Leg" />
                </div>
                <div style="clear: both"></div>
            </div>
            <br />
            <h4>Additional Complaints <a onclick="javascript:toggel()">
                <img src="img/plus.png" /></a></h4>
            <hr />
            <asp:Panel runat="server" ID="pnlCheckbox">
                <div id="demo" class="collapse">
                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="A_Fall_History" OnCheckedChanged="A_Fall_History_CheckedChanged" AutoPostBack="true" Text=" A Fall History" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Answering_The_Door" OnCheckedChanged="Answering_The_Door_CheckedChanged" AutoPostBack="true" Text=" Answering The Door" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Appliances_Laundry_Appliances" OnCheckedChanged="Appliances_Laundry_Appliances_CheckedChanged" AutoPostBack="true" Text=" Appliances And Laundry Appliances" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Bending" OnCheckedChanged="Bending_CheckedChanged" AutoPostBack="true" Text=" Bending" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Carrying_Large_Objects" OnCheckedChanged="Carrying_Large_Objects_CheckedChanged" AutoPostBack="true" Text=" Carrying Large Objects" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Carrying" OnCheckedChanged="Carrying_CheckedChanged" AutoPostBack="true" Text=" Carrying" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Cleaning" OnCheckedChanged="Cleaning_CheckedChanged" AutoPostBack="true" Text=" Cleaning" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Cognitive_Impairment" OnCheckedChanged="Cognitive_Impairment_CheckedChanged" AutoPostBack="true" Text=" Cognitive Impairment" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Decrease_In_Sensitivity_To_Heat_Pain_Pressure" OnCheckedChanged="Decrease_In_Sensitivity_To_Heat_Pain_Pressure_CheckedChanged" AutoPostBack="true" Text=" Decrease In Sensitivity To Heat Or Pain Or Pressure" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Diminished_Sense_Of_Touch" OnCheckedChanged="Diminished_Sense_Of_Touch_CheckedChanged" AutoPostBack="true" Text=" Diminished Sense Of Touch" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Doing_Laundry" OnCheckedChanged="Doing_Laundry_CheckedChanged" AutoPostBack="true" Text=" Doing Laundry" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Driving" OnCheckedChanged="Driving_CheckedChanged" AutoPostBack="true" Text=" Driving" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Emptying_The_Mailbox" OnCheckedChanged="Emptying_The_Mailbox_CheckedChanged" AutoPostBack="true" Text=" Emptying The Mailbox" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Getting_Dressed" OnCheckedChanged="Getting_Dressed_CheckedChanged" AutoPostBack="true" Text=" Getting Dressed" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Getting_In_And_Out_Of_The_Home" OnCheckedChanged="Getting_In_And_Out_Of_The_Home_CheckedChanged" AutoPostBack="true" Text=" Getting In And Out Of The Home" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Getting_In_And_Out_Of_Bed_Chairs_Sofas" OnCheckedChanged="Getting_In_And_Out_Of_Bed_Chairs_Sofas_CheckedChanged" AutoPostBack="true" Text=" Getting In And Out Of Bed Or Chairs Or Sofas" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Hearing_Problems" OnCheckedChanged="Hearing_Problems_CheckedChanged" AutoPostBack="true" Text=" Hearing Problems" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Holding" OnCheckedChanged="Holding_CheckedChanged" AutoPostBack="true" Text=" Holding" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Household_Chores" OnCheckedChanged="Household_Chores_CheckedChanged" AutoPostBack="true" Text=" Household Chores" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Incontinence" OnCheckedChanged="Incontinence_CheckedChanged" AutoPostBack="true" Text=" Incontinence" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Kneeling" OnCheckedChanged="Kneeling_CheckedChanged" AutoPostBack="true" Text=" Kneeling" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Lack_Of_Coordination" OnCheckedChanged="Lack_Of_Coordination_CheckedChanged" AutoPostBack="true" Text=" Lack Of Coordination" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Lifting" OnCheckedChanged="Lifting_CheckedChanged" AutoPostBack="true" Text=" Lifting" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Lifting_Heavy_Or_Bulky_Objects_" OnCheckedChanged="Lifting_Heavy_Or_Bulky_Objects__CheckedChanged" AutoPostBack="true" Text=" Lifting Heavy Or Bulky Objects" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Limited_Reach" OnCheckedChanged="Limited_Reach_CheckedChanged" AutoPostBack="true" Text=" Limited Reach" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Moving_About_In_Individual_Rooms" OnCheckedChanged="Moving_About_In_Individual_Rooms_CheckedChanged" AutoPostBack="true" Text=" Moving About In Individual Rooms" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Moving_From_One_Room_To_Another" OnCheckedChanged="Moving_From_One_Room_To_Another_CheckedChanged" AutoPostBack="true" Text=" Moving From One Room To Another" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Opening_Closing_Or_Locking_Windows_And_Doors" OnCheckedChanged="Opening_Closing_Or_Locking_Windows_And_Doors_CheckedChanged" AutoPostBack="true" Text=" Opening Or Closing Or Locking Windows And Doors" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Operating_Light_Switches_Faucets_Kitchen" OnCheckedChanged="Operating_Light_Switches_Faucets_Kitchen_CheckedChanged" AutoPostBack="true" Text=" Operating Light Switches Or Faucets Or Kitchen" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Physical_Weakness" OnCheckedChanged="Physical_Weakness_CheckedChanged" AutoPostBack="true" Text=" Physical Weakness" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Playing_With_Children" OnCheckedChanged="Playing_With_Children_CheckedChanged" AutoPostBack="true" Text=" Playing With Children" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Poor_Grip" OnCheckedChanged="Poor_Grip_CheckedChanged" AutoPostBack="true" Text=" Poor Grip" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Poor_Vision" OnCheckedChanged="Poor_Vision_CheckedChanged" AutoPostBack="true" Text=" Poor Vision" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Poor_Balance_Gait" OnCheckedChanged="Poor_Balance_Gait_CheckedChanged" AutoPostBack="true" Text=" Poor Balance Or Gait" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Preparing_Meals" OnCheckedChanged="Preparing_Meals_CheckedChanged" AutoPostBack="true" Text=" Preparing Meals" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Pulling" OnCheckedChanged="Pulling_CheckedChanged" AutoPostBack="true" Text=" Pulling" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Pushing" OnCheckedChanged="Pushing_CheckedChanged" AutoPostBack="true" Text=" Pushing" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Reaching_Items_In_Closets_And_Cabinets" OnCheckedChanged="Reaching_Items_In_Closets_And_Cabinets_CheckedChanged" AutoPostBack="true" Text=" Reaching Items In Closets And Cabinets" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Reduced_Mobility" OnCheckedChanged="Reduced_Mobility_CheckedChanged" AutoPostBack="true" Text=" Reduced Mobility" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Sex_Sexual_Dysfunction" OnCheckedChanged="Sex_Sexual_Dysfunction_CheckedChanged" AutoPostBack="true" Text=" Sex (Sexual Dysfunction)" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Sitting" OnCheckedChanged="Sitting_CheckedChanged" AutoPostBack="true" Text=" Sitting" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Socializing" OnCheckedChanged="Socializing_CheckedChanged" AutoPostBack="true" Text=" Socializing" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Sports_Activities" OnCheckedChanged="Sports_Activities_CheckedChanged" AutoPostBack="true" Text=" Sports Activities" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Standing" OnCheckedChanged="Standing_CheckedChanged" AutoPostBack="true" Text=" Standing" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Stooping" OnCheckedChanged="Stooping_CheckedChanged" AutoPostBack="true" Text=" Stooping" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Twisting" OnCheckedChanged="Twisting_CheckedChanged" AutoPostBack="true" Text=" Twisting" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Use_Of_Cane_Walker_Wheelchair" OnCheckedChanged="Use_Of_Cane_Walker_Wheelchair_CheckedChanged" AutoPostBack="true" Text=" Use Of Cane Or Walker Or Wheelchair" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Using_The_Stairs" OnCheckedChanged="Using_The_Stairs_CheckedChanged" AutoPostBack="true" Text=" Using The Stairs" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Using_The_Bathtub_Or_Shower" OnCheckedChanged="Using_The_Bathtub_Or_Shower_CheckedChanged" AutoPostBack="true" Text=" Using The Bathtub Or Shower" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Using_The_Kitchen" OnCheckedChanged="Using_The_Kitchen_CheckedChanged" AutoPostBack="true" Text=" Using The Kitchen" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Using_The_Toilet" OnCheckedChanged="Using_The_Toilet_CheckedChanged" AutoPostBack="true" Text=" Using The Toilet" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Using_The_Telephone" OnCheckedChanged="Using_The_Telephone_CheckedChanged" AutoPostBack="true" Text=" Using The Telephone" />
                        </div>
                        <div style="clear: both"></div>
                    </div>

                    <div class="form-horizontal">
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Walking" OnCheckedChanged="Walking_CheckedChanged" AutoPostBack="true" Text=" Walking" />
                        </div>
                        <div class="span3">
                            <asp:CheckBox runat="server" ID="Working" OnCheckedChanged="Working_CheckedChanged" AutoPostBack="true" Text=" Working" />
                        </div>
                        <div style="clear: both"></div>
                    </div>
                    <%--<h2>patient has problems with</h2>--%>
                    <asp:TextBox runat="server" ID="txt_FreeForm" TextMode="MultiLine" Width="100%" ReadOnly="True"></asp:TextBox>
                    <br />
                    .<br />

                </div>
            </asp:Panel>

            <br />

            <div class="form-horizontal">
                <strong class="labelcolor">Degree of Disability:</strong>
                <asp:RadioButtonList ID="rblDOD" TabIndex="31" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="partial">Partial</asp:ListItem>
                    <asp:ListItem Value="25%">25%</asp:ListItem>
                    <asp:ListItem Value="50%">50%</asp:ListItem>
                    <asp:ListItem Value="75%">75%</asp:ListItem>
                    <asp:ListItem Value="100%">100%</asp:ListItem>
                    <asp:ListItem Value="none">None</asp:ListItem>
                </asp:RadioButtonList><br />
                <strong class="labelcolor">Restrictions:</strong>
                <asp:CheckBoxList ID="cblRestictions" runat="server" TabIndex="32" RepeatColumns="4">
                    <asp:ListItem Value="Bending / Twisting">Bending / Twisting</asp:ListItem>
                    <asp:ListItem Value="Climbing stairs/ladders">Climbing stairs/ladders</asp:ListItem>
                    <asp:ListItem Value="Environmental conditions">Environmental conditions</asp:ListItem>
                    <asp:ListItem Value="Kneeling">Kneeling</asp:ListItem>
                    <asp:ListItem Value="Lifting">Lifting</asp:ListItem>
                    <asp:ListItem Value="Operating heavy equipment">Operating heavy equipment</asp:ListItem>
                    <asp:ListItem Value="Operation of motor vehicles">Operation of motor vehicles</asp:ListItem>
                    <asp:ListItem Value="Personal protective equipment">Personal protective equipment</asp:ListItem>
                    <asp:ListItem Value="Sitting">Sitting</asp:ListItem>
                    <asp:ListItem Value="Standing">Standing</asp:ListItem>
                    <asp:ListItem Value="Use of public transportation">Use of public transportation</asp:ListItem>
                    <asp:ListItem Value="Use of upper extremities">Use of upper extremities</asp:ListItem>
                </asp:CheckBoxList>
                <strong class="labelcolor">Others:</strong>
                <asp:TextBox ID="txtOtherRestrictions" TabIndex="33" Width="782px" runat="server"></asp:TextBox>
                <br />
                <br />
                <strong class="labelcolor">Work Status:</strong>
                <%--  <asp:CheckBoxList ID="cblWorkStatus" runat="server" RepeatColumns="2">
                            <asp:ListItem Value="Able to go back to work">Able to go back to work                              
                         
                                 </asp:ListItem>
                            <asp:ListItem Value="Working">Working</asp:ListItem>
                            <asp:ListItem Value="Not Working">Not Working</asp:ListItem>
                            <asp:ListItem Value="Partially Working">Partially Working</asp:ListItem>
                        </asp:CheckBoxList>--%>

                <asp:Repeater ID="Repeater1" runat="server">
                    <HeaderTemplate>
                        <table border="0" cellpadding="0" cellspacing="0">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cblWorkStatus" Text='<%# Eval("WorkStatus") %>' runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCollageName" runat="server" />
                            </td>

                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <%-- <strong>Work Status Comments:</strong>
               <asp:TextBox ID="workStatusCmnts" TextMode="multiline" Columns="500" Rows="5"  runat="server"></asp:TextBox>--%>
                <div style="clear: both"></div>
            </div>
            <br />

            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>


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
                if (event.error === 'no-speech') {
                    ignore_onend = true;
                }
                if (event.error === 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend = true;
                }
                if (event.error === 'not-allowed') {
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
                if (typeof (event.results) === 'undefined') {
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
              <%--  $('#<%=txtSensory.ClientID%>').text(linebreak(final_transcript));--%>
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

