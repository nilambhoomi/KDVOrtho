<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="PatientIntakeList.aspx.cs" Inherits="PatientIntakeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="js/SignJs/bootstrap3-typeahead.min.js"></script>

    <link rel="stylesheet" href="css/signature-pad.css" />
    <style>
        #container {
            display: block !important;
            position: relative !important;
        }

        .ui-autocomplete {
            position: absolute !important;
        }


        .rbllist {
            margin-top: 10px;
        }

            .rbllist label {
                line-height: inherit;
                vertical-align: middle;
                margin-top: 0px !important;
                margin-right: 10px;
                margin-left: 5px;
                margin-bottom: 5px !important;
            }

        /*a.btn {
            text-decoration: none;
        }

        table {
            text-align: center;
        }

        .main-container {
            min-height: 900px;
        }*/

        .pager::before {
            display: none;
        }

        .pager table {
            margin: 0 auto;
        }

            .pager table tbody tr td a,
            .pager table tbody tr td span {
                position: relative;
                float: left;
                padding: 6px 12px;
                margin-left: -1px;
                line-height: 1.42857143;
                color: #337ab7;
                text-decoration: none;
                background-color: #fff;
                border: 1px solid #ddd;
            }

            .pager table > tbody > tr > td > span {
                z-index: 3;
                color: #fff;
                cursor: default;
                background-color: #337ab7;
                border-color: #337ab7;
            }

            .pager table > tbody > tr > td:first-child > a,
            .pager table > tbody > tr > td:first-child > span {
                margin-left: 0;
                border-top-left-radius: 4px;
                border-bottom-left-radius: 4px;
            }

            .pager table > tbody > tr > td:last-child > a,
            .pager table > tbody > tr > td:last-child > span {
                border-top-right-radius: 4px;
                border-bottom-right-radius: 4px;
            }

            .pager table > tbody > tr > td > a:hover,
            .pager table > tbody > tr > td > span:hover,
            .pager table > tbody > tr > td > a:focus,
            .pager table > tbody > tr > td > span:focus {
                z-index: 2;
                color: #23527c;
                background-color: #eee;
                border-color: #ddd;
            }

        .modal {
            width: 100%;
        }

        .modal-dialog {
            width: 1000px;
            overflow-y: initial !important;
        }

        .modal-body {
            width: 1000px;
            height: 750px;
            overflow-y: auto;
        }
    </style>


    <link href="Style/jquery-ui.min.css" rel="stylesheet" />

    <script src="Scripts/jquery-ui.min.js"></script>
    <style>
        .modal-backdrop {
            z-index: -1;
        }

        .ui-datepicker {
            z-index: 3000;
        }
        /*#ProcedureDetailModal, #SignModal {
            background-color: black;
            opacity: 5.5;
        }*/
    </style>
    <style>
        #ddDate {
        }

        #ddLocation {
            width: 300px;
        }

        .title {
            padding: 5px;
            font-weight: bold;
            margin-bottom: 5px;
        }

        #tblProcedures tr td {
            width: 100px;
            margin: 5px;
        }



        .border {
            border-width: 1px;
            border-style: solid;
            border-color: gray;
        }

        .bottomborder {
            border-width: 0 0 1px 0;
            border-style: solid;
        }

        .med-button {
            width: 80px;
            margin: 4px;
        }

        .align {
            margin: 20px;
        }

        legend {
            font-size: small;
        }

        .sign {
            min-height: 125px;
            margin-top: 5px;
        }

        .topalign {
            margin: 20px 0 0 0;
        }

        .signbtns {
            padding-top: 15px;
        }

        .noborder {
            border: none;
        }

        .caption-align Caption {
            padding-left: 40px;
        }

        .large-button {
            width: 300px;
            margin: 30px;
            vertical-align: bottom;
        }


        .patientDetails {
            border-style: solid;
            min-height: 600px;
            border-width: 0 0 0 1px;
        }

        .sbtncontainer {
            vertical-align: bottom;
        }

        .boldertext {
            font-weight: bold;
        }



        .table {
            width: auto;
        }

        /*.model-header {
            padding: 0px !important;
        }*/

        .listBox {
            min-height: 136px;
            min-width: 200px;
        }

        .provider-ma {
            text-align: center;
        }

            .provider-ma input {
                width: 50px;
            }

        /*.modal {
            left: 50%;
            bottom: auto !important;
        }

        .modal-dialog {
            margin: 0 auto !important;
        }

        #ProcedureDetailModal, #SignModal {
            background-color: black;
            opacity: 5.5;
        }*/

        #content {
            padding: 0;
        }

        .leafNode {
            border-width: 0 0 0 1px;
            border-style: dotted;
            width: 150px;
            overflow: hidden;
        }

            .leafNode img {
                display: none;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

    <asp:PlaceHolder ID="PlaceHolder1" runat="server" />

    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Patient Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>


            <div class="">

                <div class="row">
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtSearch" placeholder="Search" runat="server"></asp:TextBox>
                                        <div id="container">
                                        </div>

                                    </div>

                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtFromDate" CssClass="dateonly" placeholder="From DOE" runat="server"></asp:TextBox>
                                        &nbsp;
                                          <asp:TextBox ID="txtEndDate" CssClass="dateonly" placeholder="To DOE" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" ID="ddl_location" Style="width: 200px">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2">


                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-default" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-default" Text="Refresh" OnClick="btnRefresh_Click" />
                                        <asp:HiddenField ID="hfPatientId" runat="server"></asp:HiddenField>
                                    </div>

                                    <div class="col-sm-1" style="float: right">
                                        <asp:DropDownList runat="server" ID="ddlPage" AutoPostBack="true" Style="float: right; width: 70px" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>

                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                            <%--   <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:RadioButtonList runat="server" ID="rbllisttype" CssClass="rbllist" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbllisttype_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Selected="True">  Active Patient</asp:ListItem>
                                            <asp:ListItem Value="1">  Seen Later Patient</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space"></div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvPatientDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="PatientIE_ID" OnRowDataBound="OnRowDataBound" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager" PageSize="25">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" ID="plusimg" Style="cursor: pointer" ImageUrl="img/plus.png" />
                                                    <%-- <img alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" id="plusimg" style="cursor: pointer" src="img/plus.png" />--%>
                                                    <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                        <asp:GridView ID="gvPatientFUDetails" BorderStyle="None" CssClass="table table-bordered" Width="100%" runat="server" AllowPaging="True" OnPageIndexChanging="gvPatientFUDetails_PageIndexChanging" AutoGenerateColumns="False" EmptyDataText="No Records Found" PagerStyle-CssClass="pager">
                                                            <Columns>
                                                                <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                                                <asp:BoundField DataField="MAProviders" HeaderText="MA & Providers" />
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>

                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link" ID="HyperLink1" NavigateUrl='<%# "~/EditFU.aspx?FUID="+Eval("PatientFUId") %>' Text="Edit FU"></asp:HyperLink>
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink2" Text='<%# Eval("PrintStatus").ToString() %>'></asp:HyperLink>

                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink3" Text='<%# (Eval("PrintStatusRod").ToString().Equals("Print Requested")? "" : Eval("PrintStatusRod").ToString().Equals("Printing")?"Printing Rod":Eval("PrintStatusRod").ToString().Equals("Download")? "dl RoD":Eval("PrintStatusRod").ToString())  %>'></asp:HyperLink>

                                                                        <asp:LinkButton runat="server" ID="lnkprintFU" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIEId").ToString()+","+Eval("PatientFUId").ToString()  %>' OnClick="lnkprintFU_Click">| Print</asp:LinkButton>
                                                                        <asp:LinkButton runat="server" ID="lnkDownloadFU" Text="| Download" Visible='<%# downloadVisible(Eval("PatientIEId").ToString(),Eval("PatientFUId").ToString())%>' OnClick="lnkDownloadFU_Click" CommandArgument='<%# Eval("PatientIEId").ToString()+"_"+Eval("PatientFUId").ToString()+","+Eval("firstname").ToString()+","+Eval("lastname").ToString() %>'></asp:LinkButton>
                                                                        <asp:LinkButton runat="server" ID="lnkSignFU" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientFUId").ToString()%>' OnClick="lnkSignFU_Click" Visible="false">| Sign</asp:LinkButton>

                                                                        <asp:HyperLink runat="server" ID="lnkhyper" CssClass="btn btn-link" NavigateUrl='<%# "~/patientdocuments.aspx?PID="+Eval("PatientId") %>'>| Documents</asp:HyperLink>
                                                                <%--           <asp:HyperLink runat="server" ID="lnkhyper" CssClass="btn btn-link" NavigateUrl='<%# "~/patientdocuments.aspx?PID="+Eval("Patient_ID") %>'>| Documents</asp:HyperLink>--%>

                                                                        <asp:LinkButton runat="server" ID="lnkPONReport" OnClick="lnkPONReport_Click" CommandArgument='<%# Eval("PatientIEId").ToString()+","+Eval("PatientFUId").ToString()+","+Eval("DOE","{0:MM/dd/yyyy}").ToString()+","+Eval("LastName").ToString()+","+Eval("FirstName").ToString()  %>' Text="| PON Report"></asp:LinkButton>
                                                                        <asp:LinkButton runat="server" ID="lnkFUDelete" OnClientClick="return confirm('Are you sure you want to delete this FU ?')" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientFUId").ToString()%>' OnClick="lnkDelete_FU_Click">| Delete</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                        </asp:GridView>

                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Sex" HeaderText="Title" />
                                            <asp:BoundField DataField="lastname" HeaderText="LastName" />
                                            <asp:BoundField DataField="firstname" HeaderText="FirstName" />
                                            <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="DOA" HeaderText="DOA" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Compensation" HeaderText="Case Type" />
                                            <asp:BoundField DataField="location" HeaderText="Location" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" NavigateUrl='<%# "~/Page1.aspx?id="+Eval("PatientIE_ID")+"&PID="+Eval("Patient_Id") %>' Text="Edit IE">
                                      
                                                    </asp:HyperLink>

                                                    <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlAddFU" NavigateUrl='<%# "~/AddFU.aspx?PID="+Eval("PatientIE_ID") %>' Text="| AddFU"></asp:HyperLink>

                                                    <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink2" Text='<%# Eval("PrintStatus").ToString() %>'></asp:HyperLink>

                                                    <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink4" Text='<%# Eval("PrintStatusRod").ToString().Equals("Print Requested")? "" : Eval("PrintStatusRod").ToString().Equals("Printing")?"Printing Rod":Eval("PrintStatusRod").ToString().Equals("Download")? "dl RoD":Eval("PrintStatusRod").ToString()  %>'></asp:HyperLink>

                                                    <asp:LinkButton runat="server" ID="lnkprint" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID") %>' OnClick="lnkprint_Click">| Print</asp:LinkButton>

                                                    <asp:LinkButton runat="server" ID="lnkDownloadIE" Visible='<%# downloadVisible(Eval("PatientIE_ID").ToString(), "0")%>' OnClick="lnkDownloadIE_Click" CommandArgument='<%# Eval("PatientIE_ID").ToString()+","+Eval("firstname").ToString()+","+Eval("lastname").ToString() %>'>| Download</asp:LinkButton>

                                                    <asp:LinkButton runat="server" ID="lnkSignIE" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID").ToString()+","+ Eval("Compensation").ToString()+","+ Eval("lastname").ToString()+","+ Eval("firstname").ToString()%>' OnClick="lnkSignIE_Click" Visible="false">| Sign</asp:LinkButton>

                                                    <asp:LinkButton runat="server" ID="lnkuploadsign" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID").ToString()%>' OnClick="lnkuploadsign_Click" Visible="false">| Upload Sign</asp:LinkButton>

                                                - <asp:HyperLink runat="server" ID="lnkhyper" CssClass="btn btn-link" NavigateUrl='<%# "~/patientdocuments.aspx?PID="+Eval("Patient_Id") %>'>| Documents</asp:HyperLink>
                                                  
                                                         <asp:HyperLink runat="server" CssClass="btn btn-link" ID="HyperLink5" NavigateUrl='<%# "~/Appointment.aspx?PID="+Eval("Patient_Id") %>' Text="| Appointment "></asp:HyperLink>

                                                    <asp:LinkButton runat="server" ID="lnkDelete" OnClientClick="return confirm('Are you sure you want to delete this IE ?')" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID").ToString()%>' OnClick="lnkDelete_Click">| Delete</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <PagerSettings PageButtonCount="5" />

                                        <PagerStyle CssClass="pager"></PagerStyle>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="printDocumentPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        Print Document
                                          <div style="display: inline-block; width: 80%; text-align: center;">
                                              <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                          </div>
                                    </div>
                                    <div class="modal-body">
                                        <div id="divPrint" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="modal fade" id="printRequestDocumentPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        Message
                                          <div style="display: inline-block; width: 80%; text-align: center;">
                                              <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                          </div>
                                    </div>
                                    <div class="modal-body">
                                        <p>Documents will be available for download after 5 min.</p>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="modal fade" id="RodPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        Select ROD 
                                        <div style="display: inline-block; width: 80%; text-align: center;">
                                            <asp:Button ID="btnrodsave" CssClass="btn btn-success" Style="margin-left: 15px" runat="server" OnClick="btnrodsave_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                            <asp:Literal ID="ltrprint" runat="server"></asp:Literal>
                                            <asp:Literal ID="ltrdownload" runat="server"></asp:Literal>

                                            <asp:Button ID="btnRODDelete" Text="Delete" OnClick="btnRODDelete_Click" runat="server" CssClass="btn btn-danger" />

                                        </div>
                                        <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    </div>
                                    <div class="modal-body">
                                        <asp:UpdatePanel runat="server" ID="upRod">
                                            <ContentTemplate>
                                                <div class="col-md-9 inline">

                                                    <asp:Repeater runat="server" ID="repRoD">
                                                        <HeaderTemplate>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td colspan="2"><b>ROD:</b>  </td>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <tr>
                                                                <td style="width: 20px">
                                                                    <asp:CheckBox runat="server" OnCheckedChanged="chk_CheckedChanged" ID="chk" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("isChecked")) %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="bodypart" Value='<%# Eval("bodypart") %>' runat="server" />
                                                                    <asp:HiddenField ID="isnewline" Value='<%# Eval("isnewline") %>' runat="server" />
                                                                    <%--<asp:TextBox runat="server" OnTextChanged="txtRod_TextChanged" TextMode="MultiLine" AutoPostBack="true" ID="txtRod" Text='<%# Eval("name") %>' Width='138%' /></td>--%>
                                                                    <% if (iCounter == 1 || iCounter == 14 || iCounter == 16)
                                                                        { %>
                                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtRod" Text='<%# Eval("name") %>' Columns="100" Rows="7" />
                                                                    <%}
                                                                        else
                                                                        { %>
                                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtRod1" Text='<%# Eval("name") %>' Columns="100" Rows="1" />
                                                                    <%}
                                                                        iCounter++;
                                                                    %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                    <div style="display: none">
                                                        <asp:TextBox runat="server" ID="txtrodFulldetails" Style="display: none"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdbodyparts" />
                                                        <asp:HiddenField runat="server" ID="hdnewline" />
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                                            <asp:Button ID="Button1" CssClass="btn btn-success" Style="margin-left: 15px" runat="server" OnClick="btnrodsave_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnrodieid" runat="server" />
                        <asp:HiddenField ID="hdnrodeditedfuid" runat="server" />
                        <asp:HiddenField ID="hdnrodeditedfuieid" runat="server" />
                        <asp:HiddenField ID="hfCurrentlyOpened" runat="server"></asp:HiddenField>
                        <%-- <asp:GridView ID="gvPatientDetails" Width="80%" runat="server" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging" AutoGenerateColumns="False" EmptyDataText="No Records Found" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="Sex" HeaderText="Title" />
                <asp:BoundField DataField="lastname" HeaderText="LastName" />
                <asp:BoundField DataField="firstname" HeaderText="FirstName" />
                <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DOA" HeaderText="DOA" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}"/>
                <asp:BoundField DataField="location" HeaderText="Location" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink runat="server" CssClass="btn" ID="hlAddFU" NavigateUrl='<%# "~/AddFU.aspx?PID="+Eval("PatientIE_ID") %>' Text="AddFU"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>               
            </Columns>
          
            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
          
            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
          
        </asp:GridView>--%>
                    </div>
                </div>
                <script src="Scripts/jquery-1.8.2.js"></script>
                <%--  <script src="Scripts/jquery-ui-1.8.24.js"></script>--%>

                <%--   <link href="Style/jquery-ui.css" rel="stylesheet" />--%>


                <asp:HiddenField runat="server" ID="hidBlobServer" />
                <input type="hidden" id="hidBlob" />
                <asp:HiddenField runat="server" ID="patientIEIDServer" />
                <asp:HiddenField runat="server" ID="patientFUIDServer" />
                <div class="modal fade" id="ModalSign" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; height: 670px!important">
                    <div class="modal-dialog" style="background: white;">
                        <div class="modal-content">
                            <div class="modal-header" style="display: inline-block; width: 100%;">
                                Sign
                                       
                 <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                            </div>
                            <div class="modal-body" style="width: 100%!important">
                                <div id="divSignHTML" runat="server"></div>
                                <br />
                                <div id="divedit" style="height: 400px; display: none">
                                    <img id="imgSign" />
                                </div>
                                <div id="signature-pad" class="signature-pad">
                                    <canvas runat="server" id="can" style="height: 400px"></canvas>
                                    <div class="signature-pad--actions">
                                        <div>
                                            <button type="button" class="button clear" data-action="clear">Clear</button>

                                        </div>

                                    </div>
                                </div>


                            </div>
                            <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                                <button id="btnClear" type="button" class="btn btn-danger" onclick="saveSign()">Save</button>
                                <button id="btnEdit" type="button" class="btn btn-danger" onclick="changeSign()">Edit</button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>


                        </div>
                    </div>
                    x
                </div>
                <div class="modal fade" id="ModalSignupload" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; height: 570px!important">
                    <div class="modal-dialog" style="background: white;">
                        <div class="modal-content" style="height: 200px;">
                            <div class="modal-header" style="display: inline-block; width: 100%;">
                                Upload Sign
                                       
                 <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                            </div>
                            <div class="modal-body">
                                <asp:FileUpload ID="fupuploadsign" CssClass="upload-icon" runat="server" />
                                <br />
                                <asp:Button ID="btnuploadimage" CssClass="btn btn-danger" runat="server" OnClick="btnuploadimage_Click" Text="upload" />
                            </div>
                            <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            </div>


                        </div>
                    </div>
                    x
                </div>
                <div style="display: none">
                    <asp:Button runat="server" ID="btnSaveSign" OnClick="btnSaveSign_Click" />
                </div>
                <script src="js/signature_pad.umd.js"></script>
                <script src="js/app.js"></script>
                <%--  <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>--%>
                <script src="js/jquery-ui-timepicker-addon.js"></script>
                <script>

                    function funfordefautenterkey1(btn, event) {

                        if (event.keyCode == 13) {
                            event.returnValue = false;
                            event.cancel = true;
                            btn.click();

                        }
                    }

                    function openSignModelPopup(_patientIEID, _patientFUID, _isedit, imgpath) {


                        $('#ModalSign').modal('show');
                        $("#<%= patientIEIDServer.ClientID%>").val(_patientIEID);
                        $("#<%= patientFUIDServer.ClientID%>").val(_patientFUID);


                        if (_isedit === "True") {
                            $("#imgSign").attr("src", "Sign/" + imgpath);
                            $('#divedit').show();
                            $('#signature-pad').hide();
                            $('#btnEdit').show();
                            $('#btnClear').hide();
                        }
                        else {
                            resizeCanvas();
                            $('#divedit').hide();
                            $('#signature-pad').show();
                            $('#btnEdit').hide();
                            $('#btnClear').show();
                        }
                        return false;

                    }

                    //$('#ModalSign').on('shown.bs.modal', function (e) {
                    //    alert('call me');   
                    //     resizeCanvas();
                    //})


                    function closeSignModelPopup() {
                        $('#RodPopup').modal('hide');
                    }
                    function saveSign() {

                        download();

                        $("#<%= hidBlobServer.ClientID%>").val($('#hidBlob').val());
                        document.getElementById("<%= btnSaveSign.ClientID %>").click();
                    }

                    function changeSign() {
                        //resizeCanvas();
                        $('#divedit').hide();
                        $('#signature-pad').show();
                        $('#btnEdit').hide();
                        $('#btnClear').show();
                    }
                    function setdatepicker() {
                        $('.dateonly').datepicker({
                            dateFormat: "mm/dd/yy"
                        });
                    }

                </script>
                <script>
                    function opensignupload(_patientIEID, _patientFUID, _isedit, imgpath) {

                        $('#ModalSignupload').modal('show');
                        $("#<%= patientIEIDServer.ClientID%>").val(_patientIEID);
                        $("#<%= patientFUIDServer.ClientID%>").val(_patientFUID);
                    }

                    function closeSignuploadModalPopup() {
                        $('#ModalSignupload').modal('hide');
                    }
                </script>


                <script type="text/javascript">
                    $.noConflict();

                    $('#ModalSign').on('shown.bs.modal', function () {
                        resizeCanvas();

                        $(this).find('.modal-dialog').css({
                            width: 730,
                            height: 540

                        });
                        $(this).find('.modal-body').css({

                            height: 500

                        });

                    });

                    function openModelPopup() {
                        $('#RodPopup').modal('show');
                    }

                    function openPrintPopup() {
                        $('#printDocumentPopup').modal('show');
                    }

                    function openPrintRequestPopup() {
                        $('#printRequestDocumentPopup').modal('show');
                    }

                    function closeModelPopup() {
                        //jQuery.noConflict();
                        //(function ($) {

                        $('#RodPopup').modal('hide');

                        //})(jQuery);
                    }

                    var downloadPath = '<%=ConfigurationSettings.AppSettings["downloadpath"]%>';
                    $(document).ready(function () {



                        setdatepicker();



                        if ($('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]')) {
                            $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').closest("tr").after("<tr><td></td><td colspan = '999'>" + $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').next().html() + "</td></tr>");
                            $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').attr("src", "img/minus.png");
                        }
                        $("[src*=plus]").live("click", function () {
                            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                            $(this).attr("src", "img/minus.png");
                        });

                        $("[src*=minus]").live("click", function () {
                            $(this).attr("src", "img/plus.png");
                            $(this).closest("tr").next().remove();
                        });

                        $("#<%=txtSearch.ClientID %>").autocomplete({
                            appendTo: "#container",
                            source: function (request, response) {

                                var str = request.term;

                                if (str.length < 3) {
                                    return;
                                }
                                $.ajax({
                                    url: 'Search.aspx/GetPatients',
                                    data: "{ 'prefix': '" + str + "','IsPatientPage':'false'}",
                                    dataType: "json",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        response($.map(data.d, function (item) {
                                            debugger

                                            return {
                                                label: (item.split('_')[0]),
                                                val: item.split('_')[1]
                                            }
                                        }))
                                    },
                                    error: function (response) {
                                        alert(response.responseText);
                                    },
                                    failure: function (response) {
                                        alert(response.responseText);
                                    }
                                });
                            },
                            select: function (e, i) {
                                $("#<%=hfPatientId.ClientID %>").val(i.item.val);
                                $('#<%= txtSearch.ClientID %>').val(i.item.label);
                                $('#<%= btnSearch.ClientID %>').click();
                            },
                            minLength: 1
                        });

                    });


                    $(document).on("click", ".PrintClick", function () {
                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var isdownload = 0;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "print requested") {
                            alert("You already given print request.");
                            return false;
                        }
                        else if ($(this).html().toLowerCase() == "download" || $(this).html().toLowerCase() == "downloaded") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatus',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (currentID.indexOf("lkbtnReprint") != -1) {
                                        alert("Print Request received.")
                                        location.reload();
                                    }
                                    else {
                                        $(obj).html("Print Requested");
                                    }
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownload',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    })

                    $(document).on("click", ".PrintClickRod", function () {


                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var isdownload = 0;
                        debugger;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "print requested") {
                            alert("You already given print request.");
                            return false;
                        }
                        else if ($(this).html().toLowerCase() == "dl rod" || $(this).html().toLowerCase() == "download") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatusRod',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (currentID.indexOf("lkbtnReprint") != -1) {
                                        alert("Print Request received.")
                                        location.reload();
                                    }
                                    else {
                                        $(obj).html("Print Requested");
                                    }
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownloadRod',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }


                    })




                </script>
            </div>
        </div>
    </div>



</asp:Content>

