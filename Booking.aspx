<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="WebApplication1.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" HtmlEncode="false">
        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F7F7F7" />
        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
        <SortedDescendingCellStyle BackColor="#E5E5E5" />
        <SortedDescendingHeaderStyle BackColor="#242121" />

        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Professor/ Instructor " />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Faculty " />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Date" />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Time" />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Course Code" />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Section" />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Image <br /> File of Permit" />
        </Columns>
        <Columns>
                <asp:BoundField  DataField=" " HeaderText="Action" />
        </Columns>
    </asp:GridView>



    </form>
    
</asp:Content>
