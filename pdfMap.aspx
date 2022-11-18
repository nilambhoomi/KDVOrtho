<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="pdfMap.aspx.cs" Inherits="pdfMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script>
function myFunction(input,list) {
    var input, filter, ul, li, a, i, txtValue;
       //input = document.getElementById("<%=txtSearchText.ClientID%>");
       //alert("test");
       filter = input.value.toUpperCase();
       var select = null;
       if(list=="text")
           select = document.getElementById("<%=ListText.ClientID%>");
       if (list== "field")
           select = document.getElementById("<%=ListField.ClientID%>");
       
       if (select) {
        
           opt = select.getElementsByTagName("option");
           for (i = 0; i < opt.length; i++) {
               //alert(opt[i].value);
               if (opt[i].value.toUpperCase().indexOf(filter) > -1) {
                   opt[i].style.display = "";
               } else {
                   opt[i].style.display = "none";
               }
           }
       }
}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" Runat="Server">
     <div style="float:left;margin-left:10px">
        Prefix :<asp:Label ID="lblPrefix" runat="server" Width="300" Text=""></asp:Label>
        Table Name :<asp:Label ID="lblTableName" runat="server" Text=""></asp:Label><br />
        
       <iframe runat="server" id="tframe" style="width:600px;height:600px"   ></iframe>
    </div>
        <div style="float:left;margin:0px 30px">
            <asp:FileUpload ID="FilePdf" Width="250" runat="server" style="float:left" />
            <asp:Button ID="btnBlankPdf" runat="server" Text="Upload Pdf"  OnClick="btnBlankPdf_Click"  />
            <br />
           <asp:Label ID="lblPdf" runat="server" Text="" ></asp:Label>
            <br />
            
            <div style="display:inline-table">
              <div style="float:left;width:230px" >
                 
                <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                  <ContentTemplate>--%>
                  <br />
                      
                      <asp:Label ID="Label3" runat="server" Text="Pdf"></asp:Label><br />
                      
                      <asp:DropDownList ID="ddlPdf" runat="server" Width="230" AutoPostBack="true" OnSelectedIndexChanged="ddlPdf_SelectedIndexChanged" ></asp:DropDownList><br /><br />
                      <asp:Label ID="Label1" runat="server" Text="Texts"></asp:Label><br />
                      <asp:TextBox ID="txtSearchText" runat="server" Width="230" onkeyup="myFunction(this,'text')" placeholder="Search for Textboxes.." ></asp:TextBox>
                      <asp:ListBox ID="ListText" CssClass="ListText" runat="server" Width="230" Height="300"></asp:ListBox>
               <%--  </ContentTemplate>
                 </asp:UpdatePanel>--%>

            </div>
            <div style="float:left;width:100px;text-align:center" >
                   <br  /><br />
                   <asp:Button ID="btnTMap" runat="server" Text="Map Table" OnClick="btnTMap_Click" /><br /><br />
                  <br /><br /><br /><br /><br />
               

                <asp:Button ID="btnMap" runat="server" Text="Map " OnClick="btnMap_Click" /><br /><br />
                  <asp:Button ID="btnUnMap" runat="server" Text="UnMap" OnClick="btnUnMap_Click"  /><br /><br />                  
                  <asp:Button ID="btnClear" runat="server" Text="Clear Map" OnClick="btnClear_Click" /><br /><br />
            </div>

            <div style="float:left;width:230px" >
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers  >
                        <asp:PostBackTrigger ControlID="txtSearchField" />

                    </Triggers>
                    <ContentTemplate>
                         <asp:Label ID="Label5" runat="server" Text="[Contains Field PatientIE_ID]"></asp:Label> <br />
                         <asp:Label ID="Label4" runat="server" Text="Tables and Views"></asp:Label> <br />
                         <asp:DropDownList ID="ddlTable" runat="server" Width="230" AutoPostBack="true" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged" ></asp:DropDownList><br /><br />
                         <asp:Label ID="Label2" runat="server" Text="Fields"></asp:Label><br />
                         <asp:TextBox ID="txtSearchField" runat="server" width="230"  onkeyup="myFunction(this,'field')" placeholder="Search for Fields.." ></asp:TextBox>

                         <asp:ListBox ID="ListField" runat="server" Width="230" Height="300"></asp:ListBox>
                        </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                </div> 
          
<%--            <div>
            <asp:Button ID="btnPdfSelect" runat="server" Text="Select Pdf" style="width:200px" OnClick="btnPdfSelect_Click"  />
            <asp:Button ID="btnTableSelect" runat="server" Text="Select Table" style="width:210px" OnClick="btnTableSelect_Click"  />
                </div>
            <br />--%>
            
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="IndianRed"></asp:Label>
            <br />            
            <br />
            <div style="">
            <asp:TextBox ID="txtPage" runat="server" Width="30"></asp:TextBox>
            <asp:Button ID="btnPage" runat="server" Text="Set Page" OnClick="btnPage_Click" /> 
                &nbsp;&nbsp;&nbsp;
            <%--<asp:TextBox ID="txtFileName" runat="server" ></asp:TextBox>--%>
            <asp:Button ID="btnFile" runat="server" Text="Set File Name Prefix" OnClick="btnFile_Click" /> 
            <asp:TextBox ID="txtDefault" runat="server" Width="230" placeholder="Default Value for Field" ></asp:TextBox>
            <asp:Button ID="btnDefault" runat="server" Text="Set Default" OnClick="btnDefault_Click" />
            <br />
                <br />
             <asp:Button ID="btnDownload" runat="server" Text="Transfer Pdf" OnClick="btnDownload_Click" /> 
                <asp:CheckBox ID="chkOverwrite" runat="server" Text="OverWrite" />

                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnRemove" runat="server" Text="Remove Pdf" OnClick="btnRemove_Click"  /> 
           <br />
                <br />
            <asp:Label ID="lblError" runat="server" Text="" ForeColor="IndianRed" ></asp:Label>
            </div>
            
            
   
         </div>
</asp:Content>

