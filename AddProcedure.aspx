<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="AddProcedure.aspx.cs" Inherits="AddProcedure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .lblstyle {
            float: right;
        }
    </style>

    <script src="Scripts/jquery-1.8.2.min.js"></script>

    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">



    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Procedure Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Position</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList class="form-control" Style="width: 50%;" ID="ddlposition" runat="server">
                                        <asp:ListItem Text="select" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                        <asp:ListItem Text="Bilateral" Value="Bilateral"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">BodyPart</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtBodyParts" CssClass="form-control" placeholder="BodyPart" runat="server"></asp:TextBox>
                                </div>
                            </div>


                        </div>

                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Display Order</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDisplay_Order" CssClass="form-control" placeholder="Display Order" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">R - Heading</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtHeading" CssClass="form-control" placeholder="R - Heading" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Muscle</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtMuscles" class="form-control" Style="width: 80%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">Medication</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtMedication" Style="width: 80%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">SubProcedure</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtSubProcedure" Style="width: 80%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">R_CCDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCCDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">R_PEDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPEDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">R_ADesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtADesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">R_PDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">S - Heading</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtHeadingS" Width="80%" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">S_CCDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtS_CCDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">S_PEDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtS_PEDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">S_ADesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtS_ADesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">S_PDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtS_PDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">E - Heading</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtHeadingE" Width="80%" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">E_CCDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtE_CCDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">E_PEDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtE_PEDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <label class="lblstyle">E_ADesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtE_ADesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>


                        </div>
                        <div class="clearfix"></div>
                        <br />

                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">E_PDesc</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtE_PDesc" Width="80%" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>

                                <div class="col-sm-2">
                                    <label class="lblstyle">Levels Default Value.</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtLevelsDefault" runat="server" Width="80%" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-inline col-lg-12">
                                    </div>
                                </div>
                            </div>


                        </div>



                        <div class="clearfix"></div>
                        <br />

                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">Default Value for sides</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlSidesDefault" runat="server">
                                        <asp:ListItem Text="--Select--" Value=" "></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                        <asp:ListItem Text="Right" Value="Right"></asp:ListItem>
                                        <asp:ListItem Text="Bilateral" Value="Bilateral"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-sm-2">
                                    <label class="lblstyle">MCODE</label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtMCODE" class="form-control" runat="server"></asp:TextBox>
                                </div>



                            </div>
                        </div>


                        <div class="clearfix"></div>
                        <br />

                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                </div>
                                <div class="col-sm-5">
                                    <asp:CheckBox ID="chkINhouseProcbit" runat="server" Style="padding: 5px" Text="IN-House" />
                                    <asp:CheckBox ID="chkSides" runat="server" Text="Sides" />
                                    <asp:CheckBox ID="chkHasLevel" runat="server" Text="Level" />
                                    <asp:CheckBox ID="chkCF" runat="server" Text="CF" />
                                    <asp:CheckBox ID="chkPN" runat="server" Text="PN" />
                                </div>

                            </div>
                        </div>

                        <div class="clearfix"></div>
                        <br />



                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-sm-2">
                                    <label class="lblstyle">&nbsp;</label>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnBack" PostBackUrl="~/ViewAttorney.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
