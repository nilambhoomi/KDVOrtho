<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="AddPatientMaster.aspx.cs" Inherits="AddPatientMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .lblstyle {
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">


    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery-ui-timepicker-addon.js"></script>
    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>


    <script>

        $(document).ready(function ($) {
            var $j = jQuery.noConflict();


            $j('#<%=txtDOB.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true
                //onSelect: function (dateText, inst) {
                //    $(this).focus();
                //}
            });

            $j('#<%=txtDOB.ClientID%>').mask("99/99/9999");

            $j('#<%=txtPhone1.ClientID%>').mask("999-999-9999");
            $j('#<%=txtPhone2.ClientID%>').mask("999-999-9999");

        });

    </script>
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Patients Details								
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
                                        <label class="lblstyle">First Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtFirstName" CssClass="form-control" placeholder="First Name" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="Please enter patient first name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Middle Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtMiddleName" CssClass="form-control" placeholder="Middle Name" runat="server"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Last Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLastName" CssClass="form-control" placeholder="Last Name" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">SEX</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList runat="server" ID="ddlSex" Width="90px" CssClass="form-control">
                                            <asp:ListItem Value="0">-- Sex --</asp:ListItem>
                                            <asp:ListItem Value="Mr." Text="M"></asp:ListItem>
                                            <asp:ListItem Value="Ms." Text="F"></asp:ListItem>

                                        </asp:DropDownList>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">DOB</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtDOB" CssClass="form-control" placeholder="DOB" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDOB" Display="Dynamic" ErrorMessage="Please enter DOB" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDOB" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                                            ErrorMessage="Invalid date format." ValidationGroup="save" />
                                        <asp:CustomValidator runat="server" ControlToValidate="txtDOB" ErrorMessage="DOB should be MM/dd/yyyy format" Display="Dynamic" SetFocusOnError="True" ClientValidationFunction="CustomValidator1_ServerValidate" />
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Handedness</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:DropDownList CssClass="form-control" ID="ddlHandedness" runat="server">
                                            <asp:ListItem Value="right-handed">right-handed</asp:ListItem>
                                            <asp:ListItem Value="left-handed">left-handed</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>

                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Email</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Work Phone</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtWorkPhone" CssClass="form-control" placeholder="Work Phone" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Address 1</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAdd1" CssClass="form-control" placeholder="Address 1" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Address 2</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAdd2" CssClass="form-control" placeholder="Address 2" runat="server"></asp:TextBox>
                                    </div>


                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">City</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtCity" CssClass="form-control" placeholder="City" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">State</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtState" CssClass="form-control" placeholder="State" runat="server"></asp:TextBox>
                                    </div>


                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Zip</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtZip" CssClass="form-control" placeholder="Zip" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Home Phone</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPhone1" CssClass="form-control" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                    </div>



                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Mobile #</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPhone2" CssClass="form-control" placeholder="xxx-xxx-xxxx" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-2">
                                        <label class="lblstyle">Account No</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtAccountNo" CssClass="form-control" placeholder="Account No" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Policy No</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPolicyNo" CssClass="form-control" placeholder="Policy No" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">SSN</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSSN" CssClass="form-control" placeholder="SSN" runat="server"></asp:TextBox>
                                    </div>


                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Image</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:FileUpload runat="server" ID="fupImg" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">&nbsp;</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                            &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/ViewPatientMaster.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                        </div>
                                    </div>

                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:GridView ID="gvPatientDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="PatientIE_ID" OnRowDataBound="gvPatientDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" ID="plusimg" Style="cursor: pointer" ImageUrl="img/plus.png" />
                                        <asp:Panel ID="pnlOrders" runat="server" Style="display:none    ">
                                            <asp:GridView ID="gvPatientFUDetails" BorderStyle="None" CssClass="table table-bordered" Width="100%" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                                    <asp:BoundField DataField="Location" HeaderText="Location" />
                                                    <asp:BoundField DataField="MAProviders" HeaderText="MA & Providers" />

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



                            </Columns>
                            <PagerSettings PageButtonCount="5" />

                            <PagerStyle CssClass="pager"></PagerStyle>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnrodieid" runat="server" />
            <asp:HiddenField ID="hdnrodeditedfuid" runat="server" />
            <asp:HiddenField ID="hdnrodeditedfuieid" runat="server" />
            <asp:HiddenField ID="hfCurrentlyOpened" runat="server"></asp:HiddenField>

        </div>
    </div>
    <script>
        $(document).ready(function () {

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
        })
    </script>

</asp:Content>

