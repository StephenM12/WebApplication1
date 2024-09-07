<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RequestapprovalForm.aspx.cs" Inherits="RoomRequestForm.RequestSignatureForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/RoomSchedule_Style.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script src="Scripts/script.js"></script>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div>
        </div>
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
                        <asp:BoundField DataField="Room" HeaderText="RoomNumber" SortExpression="RoomNumber" />
                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                        <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="false" />
                        <asp:BoundField DataField="startTime" HeaderText="StartTime" SortExpression="StartTime" />
                        <asp:BoundField DataField="endTime" HeaderText="EndTime" SortExpression="EndTime" />
                        <asp:BoundField DataField="PurposeoftheRoom" HeaderText="PurposeoftheRoom" SortExpression="PurposeoftheRoom" />

                        <asp:ButtonField ButtonType="Button" CommandName="Accept" Text="Accept" />

                        <asp:ButtonField ButtonType="Button" CommandName="Reject" Text="Reject" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</asp:Content>