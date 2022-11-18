<%@ Page Language="C#" AutoEventWireup="true" CodeFile="signpad.aspx.cs" Inherits="signpad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <%-- <script src="https://raw.githubusercontent.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>--%>

    <%--<script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>--%>

    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>



    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery.maskedinput.js"></script>

    <script src="Scripts/SigWebTablet.js"></script>
    <script src="Scripts/signScript.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="1" cellpadding="0"  width="500">
  <tr>
    <td height="100" width="500">
<canvas id="cnv" name="cnv" width="500" height="100"></canvas>
    </td>
  </tr>
</table>


<BR>
<canvas name="SigImg" id="SigImg" width="500" height="100"></canvas>



<form action="#" name=FORM1>
<p>
<input id="SignBtn" name="SignBtn" type="button" value="Sign"  onclick="javascript: onSign()"/>&nbsp;&nbsp;&nbsp;&nbsp;
<input id="button1" name="ClearBtn" type="button" value="Clear" onclick="javascript: onClear()"/>&nbsp;&nbsp;&nbsp;&nbsp

<input id="button2" name="DoneBtn" type="button" value="Done" onclick="javascript: onDone()"/>&nbsp;&nbsp;&nbsp;&nbsp

<INPUT TYPE=HIDDEN NAME="bioSigData">
<INPUT TYPE=HIDDEN NAME="sigImgData">
<BR>
<BR>
<TEXTAREA NAME="sigStringData" ROWS="20" COLS="50">SigString: </TEXTAREA>
<TEXTAREA NAME="sigImageData" ROWS="20" COLS="50">Base64 String: </TEXTAREA>
</p>
</form>
 
<br /><br />
    </form>
</body>
</html>
