<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="TestPage" %>

<%--<%@ Register Assembly="WebSignature" Namespace="RealSignature" TagPrefix="ASP" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-1.9.1.js"></script>
    <link rel="stylesheet" href="css/signature-pad.css" />
    <script type="text/javascript">
        function chekcVal(chk, val) {
            debugger
            var str = document.getElementById("hidval").value;

            if (chk.checked)
                str = str + "," + val;
            else
                str = str.replace("," + val, "");

            document.getElementById("hidval").value = str;

        }

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-39365077-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>

</head>
<body>
    <form runat="server" id="form1">




        <div class="col-md-3">
            <label class="control-label labelcolor">GAIT:</label>
        </div>
        <div class="col-md-9" style="margin-top: 5px">


            <section class="dropdown">
                <input type="text" id="txtGAIT" onchange="txtMe(this)" value="Antalgic">
                <select onchange="this.previousElementSibling.value = this.value; this.previousElementSibling.focus();selectVal('txtGAIT', this.value)" id="cboGAIT">
                    <option value=""></option>
                    <option value="Normal">Normal</option>
                    <option value="Antalgic">Antalgic</option>
                    <option value="Nonantalgic">Nonantalgic</option>
                </select>
            </section>


            <section class="dropdown">
                <input type="text" id="txtAmbulates" onchange="txtMe(this)" value="Ambulates with a cane">
                <select onchange="this.previousElementSibling.value = this.value; this.previousElementSibling.focus();selectVal('txtAmbulates', this.value)" id="cboAmbulates">
                    <option value=""></option>
                    <option value="Ambulates with a cane">Ambulates with a cane</option>
                    <option value="Ambulates with a straight access cane">Ambulates with a straight access cane</option>
                    <option value="Ambulates with a quad cane">Ambulates with a quad cane</option>
                    <option value="Ambulates with a walker">Ambulates with a walker</option>
                    <option value="Ambulates with a rolling walker">Ambulates with a rolling walker</option>
                    <option value="Ambulates with a crutch">Ambulates with a crutch</option>
                    <option value="Ambulates with crutches">Ambulates with crutches</option>
                    <option value="Ambulates with a wheelchair">Ambulates with a wheelchair</option>
                </select>
            </section>



            <input type="checkbox" id="chkFootslap" value="Foot slap/drop" onclick="checkMe(this)" checked="checked">
            Foot slap/drop
   
            <input type="checkbox" id="chkKneehyperextension" value="knee hyperextension" onclick="checkMe(this)" checked="checked">
            knee hyperextension
   
            <input type="checkbox" id="chkUnabletohealwalk" value="unable to heel walk" onclick="checkMe(this)" checked="checked">
            unable to heel walk
   
            <input type="checkbox" id="chkUnabletotoewalk" value="unable to toe walk" onclick="checkMe(this)" checked="checked">
            unable to toe walk
   
            <label class="control-label">and </label>
            <input type="text" id="txtOther" onchange="txtMe(this)" style="width: 350px" value="This is a test patient.">
        </div>





        <div id="signature-pad" class="signature-pad" style="display: none">

            <table style="width: 200px">
                <tr>
                    <td>
                        <canvas runat="server" id="can" style="height: 200px"></canvas>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="signature-pad--actions">
                            <div>
                                <button type="button" class="button clear" data-action="clear">Clear</button>
                                <button type="button" class="button" data-action="change-color">Change color</button>
                                <button type="button" class="button" data-action="undo">Undo</button>

                            </div>
                            <div>
                                <button type="button" class="button save" data-action="save-png">Save as PNG</button>
                                <button type="button" class="button save" data-action="save-jpg">Save as JPG</button>
                                <button type="button" class="button save" data-action="save-svg">Save as SVG</button>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img id="imgDemo" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>

            <div class="signature-pad--body">
            </div>
            <br />
            <div class="signature-pad--footer">
                <div class="description">Sign above</div>


            </div>
        </div>
        <asp:Button runat="server" ID="btnLoadcheckbox" Text="loadcheckbox" OnClick="btnLoadcheckbox_Click" />
        <asp:PlaceHolder ID="placeHolder" runat="server"></asp:PlaceHolder>
        <asp:Button runat="server" ID="btnTest" Text="Test" OnClick="btnTest_Click" />
        <asp:HiddenField runat="server" ID="hidval" />

        <div class="table-responsive">
            <table class="table table-bordered">
                <thead>
                    <tr id="trHeader">
                        <td></td>
                        <td colspan="1">Strainght leg raise test</td>
                        <td colspan="1">Braggard's test</td>
                        <td style="">Kernig's sign</td>
                        <td style="">Brudzinski's test</td>
                        <td style="">Sacroiliac compression</td>
                        <td style="">Sacral notch tenderness</td>
                        <td style="">Ober's test causing pain at the SI joint</td>
                    </tr>
                </thead>
                <tbody>
                    <tr id="trLeft">
                        <td style="">Left</td>
                        <td colspan="1">
                            <input type="checkbox" id="chkLegRaisedExamLeft" onclick="checkMe(this)" />
                            <input type="text" id="txtLegRaisedExamLeft" onchange="txtMe(this)" style="height: 15px; width: 50px" />
                        </td>
                        <td colspan="1">
                            <input type="checkbox" id="chkBraggardLeft" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkKernigLeft" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkBrudzinskiLeft" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkSacroiliacLeft" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkSacralNotchLeft" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkOberLeft" onclick="checkMe(this)" />
                        </td>
                    </tr>
                    <tr>
                        <td style="">Right</td>
                        <td>
                            <input type="checkbox" id="chkLegRaisedExamRight" onclick="checkMe(this)" />
                            <input type="text" id="txtLegRaisedExamRight" onchange="txtMe(this)" style="height: 15px; width: 50px" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkBraggardRight" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkKernigRight" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkBrudzinskiRight" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkSacroiliacRight" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkSacralNotchRight" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkOberRight" onclick="checkMe(this)" />
                        </td>
                    </tr>
                    <tr>
                        <td style="">Bilateral</td>
                        <td>
                            <input type="checkbox" id="chkLegRaisedExamBilateral" onclick="checkMe(this)" />
                            <input type="text" id="txtLegRaisedExamBilateral" onchange="txtMe(this)" style="height: 15px; width: 50px" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkBraggardBilateral" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkKernigBilateral" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkBrudzinskiBilateral" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkSacroiliacBilateral" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkSacralNotchBilateral" onclick="checkMe(this)" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkOberBilateral" onclick="checkMe(this)" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <button onclick="return getValues()">Demo</button>
    </form>


</body>
<script src="js/signature_pad.umd.js"></script>
<script src="js/app.js"></script>
<script>
    function checkMe(chk) {

        if ($(chk).prop("checked")) {
            if ($(chk).attr("type") === "radio")
                $("[name=" + $(chk).attr("name") + "]").removeAttr('checked');
            $(chk).attr('checked', 'checked');
        }
        else {
            $(chk).removeAttr('checked');
        }
    }
    function txtMe(txt) {
        var val = $(txt).val();
        $(txt).attr('value', val);
    }

    function selectVal(txtid, val) {
        $("#" + txtid).attr('value', val);
    }

    function getValues() {
        var selected = '';
        var selectedVal = '';
        $('#trLeft input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected = selected + "," + 1;
            } else
                selected = selected + "," + 0;
        });

        $('#trHeader td').each(function () {
            debugger
            selectedVal = selectedVal + "," + $(this).html();
        });

        alert(selectedVal);
        return false;
    }
</script>
</html>
