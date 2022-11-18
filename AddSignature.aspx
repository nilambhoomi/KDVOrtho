<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddSignature.aspx.cs" Inherits="AddSignature" %>


<script src="Scripts/jquery-3.1.1.min.js"></script>
<script src="Scripts/SigWebTablet.js"></script>
<script src="Scripts/signScript.js"></script>
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
<input id="SignBtn" name="SignBtn" type="button" value="Sign"  onclick="javascript:onSign()"/>&nbsp;&nbsp;&nbsp;&nbsp;
<input id="button1" name="ClearBtn" type="button" value="Clear" onclick="javascript:onClear()"/>&nbsp;&nbsp;&nbsp;&nbsp

<input id="button2" name="DoneBtn" type="button" value="Done" onclick="javascript:onDone()"/>&nbsp;&nbsp;&nbsp;&nbsp

<INPUT TYPE=HIDDEN NAME="bioSigData">
<INPUT TYPE=HIDDEN NAME="sigImgData">
<BR>
<BR>
<TEXTAREA NAME="sigStringData" ROWS="20" COLS="50">SigString: </TEXTAREA>
<textarea NAME="sigImageData" runat="server" ROWS="20" COLS="50">Base64 String: </textarea>
<button id="save" runat="server" onclick="cvsa"></button>
</p>
</form>
 
<br /><br />
