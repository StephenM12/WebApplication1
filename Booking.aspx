﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="WebApplication1.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>

        <asp:ScriptManager runat="server"></asp:ScriptManager>

        


       <%-- <div>
        </div>--%>
        <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                    Height="323px" Width="973px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand" DataKeyNames="RequestID">
                    <Columns>
                        <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
                        <asp:BoundField DataField="Course" HeaderText="CourseCode" SortExpression="CourseCode" />
                        <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="Section" />
                        <asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />
                        <asp:BoundField DataField="Faculty" HeaderText="Faculty" SortExpression="Faculty" />
                        <asp:BoundField DataField="Building" HeaderText="Building" SortExpression="Building" />
                        <asp:TemplateField HeaderText="RoomNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomName" runat="server" Text='<%# Eval("Room") %>'></asp:Label>
                                <i class="fa-solid fa-triangle-exclamation" style='<%# CheckScheduleConflict(Container) %>'></i>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                        <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                        <asp:BoundField DataField="startTime" HeaderText="StartTime" SortExpression="StartTime" />
                        <asp:BoundField DataField="endTime" HeaderText="EndTime" SortExpression="EndTime" />
                        <asp:BoundField DataField="PurposeoftheRoom" HeaderText="PurposeoftheRoom" SortExpression="PurposeoftheRoom" />

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnDeploy" runat="server" Text="Deploy" CommandName="Deploy" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-primary" />
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnReject" runat="server" Text="Reject" CommandName="Reject" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-secondary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>



            </ContentTemplate>
        </asp:UpdatePanel>


    </form>
</asp:Content>