<%@ Page Title="" Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Comments.aspx.cs" Inherits="Comments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <div class="container">
        <div class="row">

            <div class="col-md-10">
                <label class="control-label">Comments: </label>
                <div class="controls">
                    <asp:TextBox TextMode="MultiLine" Width="800px" Height="300px" runat="server" ID="txtComment"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-10">
                <label class="control-label">Extra Comments: </label>
                <div class="controls">
                    <asp:TextBox TextMode="MultiLine" Width="800px" Height="300px" runat="server" ID="txtCommentextra"></asp:TextBox>
                    <button type="button" id="start_button" onclick="startButton(event)">
                                <img height="25px" width="25px" src="images/mic.png" alt="start" /></button>
                            <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-10">
                <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" UseSubmitBehavior="False" />
            </div>
        </div>
    </div>
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
                $('#<%=txtCommentextra.ClientID%>').text(linebreak(final_transcript));
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

