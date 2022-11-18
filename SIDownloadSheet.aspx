<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SIDownloadSheet.aspx.cs" Inherits="SIDownloadSheet" EnableEventValidation = "false"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="width: 90%; margin: 0 auto;">
                <h2 style="text-align: center">SignIn Sheet</h2>
                <p style="text-align: center"> <asp:Button runat="server" ID="Download" Text="Download as PDF" OnClick="Download_Click" /> &nbsp; <asp:Button runat="server" ID="Button1" Text="Download as Excel" OnClick="Button1_Click"/> &nbsp; <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                     
                </p> 
                <br />
                <div> <span style="text-align: left; width: 30%"><strong>Date : </strong><asp:Label runat="server" ID="lblDate"></asp:Label></span>
                    <span style="text-align: left; width: 30%"><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Location : </strong><asp:Label runat="server" ID="lblLocation"></asp:Label></span>
                    <span style="text-align: left; width: 40%"><strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; MA Provider : </strong><asp:Label runat="server" ID="lblMAProvider"></asp:Label></span>
                </div>
             
                <br />

                <asp:GridView ID="gvSISheet" Width="100%" runat="server" AutoGenerateColumns="false" OnPreRender="gvSISheet_PreRender">
                    <EmptyDataTemplate>
                        No Records Found
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&nbsp;&nbsp;Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Signature" ItemStyle-Width="200px">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Visit<br/>IE/FU" ItemStyle-Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="lblVisit" runat="server" Text='<%# Bind("Visit") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ins<br/>(Case type)">
                            <ItemTemplate>
                                <asp:Label ID="lblCaseType" runat="server" Text='<%# Bind("CaseType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Followed <br/>up on">
                            <ItemTemplate>
                                <asp:Label ID="lblFollowedUpOn" runat="server" Text='<%# Bind("FollowedUpOn") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="In-HouseProc">
                            <ItemTemplate>
                                <asp:Label ID="lblExecuted" runat="server" Text='<%# Bind("Executed") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Procedures <br/>Requested">
                            <ItemTemplate>
                                <asp:Label ID="lblRequested" runat="server" Text='<%# Bind("Requested") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Procedures <br/>Scheduled">
                            <ItemTemplate>
                                <asp:Label ID="lblScheduled" runat="server" Text='<%# Bind("Scheduled") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Next Visit" ItemStyle-Width="80px"></asp:TemplateField>
                        <asp:TemplateField HeaderText="Next Visit" ItemStyle-Width="80px"></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
