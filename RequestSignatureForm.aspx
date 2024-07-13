<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestSignatureForm.aspx.cs" Inherits="RoomRequestForm.RequestSignatureForm" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Request Signature</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:roomScheduleV6_PrototypeConnectionString2 %>" 
                ProviderName="<%$ ConnectionStrings:roomScheduleV6_PrototypeConnectionString2.ProviderName %>" 
                SelectCommand="SELECT [email], [CourseCode], [Section], [Instructor], [Faculty], [PurposeoftheRoom], [Building], [RoomNumber], [StartDate], [EndDate], [startTime], [endTime] FROM [RoomRequest]">
            </asp:SqlDataSource>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
                Height="323px" Width="973px" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
                    <asp:BoundField DataField="CourseCode" HeaderText="CourseCode" SortExpression="CourseCode" />
                    <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="Section" />
                    <asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />
                    <asp:BoundField DataField="Faculty" HeaderText="Faculty" SortExpression="Faculty" />
                    <asp:BoundField DataField="PurposeoftheRoom" HeaderText="PurposeoftheRoom" SortExpression="PurposeoftheRoom" />
                    <asp:BoundField DataField="Building" HeaderText="Building" SortExpression="Building" />
                    <asp:BoundField DataField="RoomNumber" HeaderText="RoomNumber" SortExpression="RoomNumber" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" />
                    <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                </Columns>
            </asp:GridView>
    </form>
</body>
</html>
