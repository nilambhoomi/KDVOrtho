<%@ Page Title="" Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="DemoTest.aspx.cs" Inherits="DemoTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div runat="server" id="CF">
    </div>

    <asp:HiddenField runat="server" ID="hdId" Value="0" />
    <asp:HiddenField runat="server" ID="hdorgval" />

    <%--  <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />--%>

    <script>


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
                $("input[name='R5D'][value='" + arrayVal[10] + "']").prop('checked', true);
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
                $("input[name='A5D'][value='" + arrayVal1[10] + "']").prop('checked', true);
            }
        }

        //$(function () {
        //    $("input[name='RSH'][value='0']").prop('checked', true);
        //});

        function funSave() {


            var htmlval = $("#<%= hdorgval.ClientID %>").val();
            var orghtmlval = htmlval;


            txtPainScale = 'value="' + $("#txtPainScale").val() + '"';
            txtWeeknessIn = 'value="' + $("#txtWeeknessIn").val() + '"';
            txtWorseOtherText = 'value="' + $("#txtWorseOtherText").val() + '"';


            chkcontent = $("#chkcontent").is(':checked') ? "checked=\"checked\"" : "";
            chkintermittent = $("#chkintermittent").is(':checked') ? "checked=\"checked\"" : "";


            chksharp = $("#chksharp").is(':checked') ? "checked=\"checked\"" : "";
            chkelectric = $("#chkelectric").is(':checked') ? "checked=\"checked\"" : "";
            chkshooting = $("#chkshooting").is(':checked') ? "checked=\"checked\"" : "";
            chkthrobbling = $("#chkthrobbling").is(':checked') ? "checked=\"checked\"" : "";
            chkpulsating = $("#chkpulsating").is(':checked') ? "checked=\"checked\"" : "";
            chkdull = $("#chkdull").is(':checked') ? "checked=\"checked\"" : "";
            chkachy = $("#chkachy").is(':checked') ? "checked=\"checked\"" : "";
            chknumbness = $("#chknumbness").is(':checked') ? "checked=\"checked\"" : "";
            chktingling = $("#chktingling").is(':checked') ? "checked=\"checked\"" : "";
            chkburning = $("#chkburning").is(':checked') ? "checked=\"checked\"" : "";



            txtradiates = 'value="' + $("#txtradiates").val() + '"';
            txtburningTo = 'value="' + $("#txtburningTo").val() + '"';


            chkWorseSitting = $("#chkWorseSitting").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseStanding = $("#chkWorseStanding").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseLyingDown = $("#chkWorseLyingDown").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseMovement = $("#chkWorseMovement").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseSeatingtoStandingUp = $("#chkWorseSeatingtoStandingUp").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseWalking = $("#chkWorseWalking").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseClimbingStairs = $("#chkWorseClimbingStairs").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseDescendingStairs = $("#chkWorseDescendingStairs").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseDriving = $("#chkWorseDriving").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseWorking = $("#chkWorseWorking").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseBending = $("#chkWorseBending").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseLifting = $("#chkWorseLifting").is(':checked') ? "checked=\"checked\"" : "";
            chkWorseTwisting = $("#chkWorseTwisting").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedResting = $("#chkImprovedResting").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedMedication = $("#chkImprovedMedication").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedTherapy = $("#chkImprovedTherapy").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedSleeping = $("#chkImprovedSleeping").is(':checked') ? "checked=\"checked\"" : "";
            chkImprovedMovement = $("#chkImprovedMovement").is(':checked') ? "checked=\"checked\"" : "";




            htmlval = htmlval.replace('#txtpainScale', txtPainScale);
            htmlval = htmlval.replace('#txtWeeknessIn', txtWeeknessIn);
            htmlval = htmlval.replace('#txtWorseOtherText', txtWorseOtherText);

            htmlval = htmlval.replace('#chkcontent', chkcontent);
            htmlval = htmlval.replace('#chkintermittent', chkintermittent);
            htmlval = htmlval.replace('#chksharp', chksharp);
            htmlval = htmlval.replace('#chkelectric', chkelectric);
            htmlval = htmlval.replace('#chkshooting', chkshooting);
            htmlval = htmlval.replace('#chkthrobbling', chkthrobbling);
            htmlval = htmlval.replace('#chkpulsating', chkpulsating);
            htmlval = htmlval.replace('#chkdull', chkdull);
            htmlval = htmlval.replace('#chkachy', chkachy);

            htmlval = htmlval.replace('#txtradiates', txtradiates);
            htmlval = htmlval.replace('#txtburningTo', txtburningTo);
            htmlval = htmlval.replace('#chknumbness', chknumbness);
            htmlval = htmlval.replace('#chktingling', chktingling);
            htmlval = htmlval.replace('#chkburning', chkburning);

            htmlval = htmlval.replace('#chkWorseSitting', chkWorseSitting);
            htmlval = htmlval.replace('#chkWorseStanding', chkWorseStanding);
            htmlval = htmlval.replace('#chkWorseLyingDown', chkWorseLyingDown);
            htmlval = htmlval.replace('#chkWorseMovement', chkWorseMovement);
            htmlval = htmlval.replace('#chkWorseSeatingtoStandingUp', chkWorseSeatingtoStandingUp);
            htmlval = htmlval.replace('#chkWorseWalking', chkWorseWalking);
            htmlval = htmlval.replace('#chkWorseClimbingStairs', chkWorseClimbingStairs);
            htmlval = htmlval.replace('#chkWorseDescendingStairs', chkWorseDescendingStairs);
            htmlval = htmlval.replace('#chkWorseDriving', chkWorseDriving);
            htmlval = htmlval.replace('#chkWorseWorking', chkWorseWorking);
            htmlval = htmlval.replace('#chkWorseBending', chkWorseBending);
            htmlval = htmlval.replace('#chkWorseLifting', chkWorseLifting);
            htmlval = htmlval.replace('#chkWorseTwisting', chkWorseTwisting);
            htmlval = htmlval.replace('#chkImprovedResting', chkImprovedResting);
            htmlval = htmlval.replace('#chkImprovedMedication', chkImprovedMedication);
            htmlval = htmlval.replace('#chkImprovedTherapy', chkImprovedTherapy);
            htmlval = htmlval.replace('#chkImprovedSleeping', chkImprovedSleeping);
            htmlval = htmlval.replace('#chkImprovedMovement', chkImprovedMovement);





            var painradiation = '', painradiation1 = '';

            var radioValue = $("input[name='RSH']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='RSC']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='RAR']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='RFA']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='RHA']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='RWR']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';


            radioValue = $("input[name='R1D']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='R2D']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='R3D']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='R4D']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';

            radioValue = $("input[name='R5D']:checked").val();
            if (radioValue)
                painradiation = painradiation + ',' + radioValue;
            else
                painradiation = painradiation + ',-1';



            radioValue = $("input[name='ASH']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='ASC']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='AAR']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='AFA']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='AHA']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';


            radioValue = $("input[name='AWR']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';


            radioValue = $("input[name='A1D']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='A2D']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='A3D']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='A4D']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';

            radioValue = $("input[name='A5D']:checked").val();
            if (radioValue)
                painradiation1 = painradiation1 + ',' + radioValue;
            else
                painradiation1 = painradiation1 + ',-1';




            $.ajax({
                type: "POST",
                url: "DemoTest.aspx/funSave",
                data: "{ 'value':'" + htmlval + "','orgvalue':'" + orghtmlval + "','id':'" + $('#<%= hdId.ClientID %>').val() + "','painradiation':'" + painradiation +
                    "','painradiation1':'" + painradiation1 + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "JSON",
                success: function (output) {
                    //////debugger;
                    //$('#' + t[0].dataset.div).html('');
                    //$('#' + t[0].dataset.div).append(output.d);
                    //tableTransform($('#countbl'));
                }
            });

            return false;
        }
    </script>
</asp:Content>

