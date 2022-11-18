<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCPOCSummary.ascx.cs" Inherits="UserControl_UCPOCSummary" %>

<div class="container">

    <div class="row">
        <h3>POC Summary</h3>
        <hr />
    </div>
    <div class="row">
        <div class="col-12">
            <asp:HiddenField ID="pageHDN" runat="server" />
            <div class="container">
                <div class="row">
                    <div class="table-responsive">
                        <asp:Repeater runat="server" ID="repSummery">
                            <HeaderTemplate>
                                <table class="table table-striped table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th>Date</th>
                                            <th>Body Part</th>
                                            <th>Heading</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("PDate","{0:MM/dd/yyyy}") %></td>
                                    <td><%# Eval("BodyPart") %></td>
                                    <td><%# Eval("Heading") %></td>

                                </tr>
                            </ItemTemplate>
                            <FooterTemplate></table></FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
