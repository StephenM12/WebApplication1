<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoomSchedule.aspx.cs"
    Inherits="WebApplication1.RoomSchedule" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Style.css" />

   
    <form id="form1" runat="server">
        <%-- grid code--%>

        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1" OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="False">
        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#594B9C" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#33276A" />

        <Columns>
        <asp:BoundField DataField="Scheds" HeaderText="Schedule" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Monday" HeaderText="Monday" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Tuesday" HeaderText="Tuesday" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Wednesday" HeaderText="Wednesday" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Thursday" HeaderText="Thursday" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Friday" HeaderText="Friday" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Saturday" HeaderText="Saturday" />
        </Columns>
        <Columns>
        <asp:BoundField DataField="Sunday" HeaderText="Sunday" />
        </Columns>

        </asp:GridView>


    <!-- Button trigger modal -->
    <button type="button" class=" bg-color btn btn-primary bg-color lower-right" data-bs-toggle="modal" data-bs-target="#exampleModal" OnClick="addBtnClk">
        ADD TO SCHEDULE
    </button>


    <!-- Modal -->
    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true"> <br>
        <div class="modal-dialog modal-fullscreen">
            <div class="modal-content">

                <!-- Put ASP Controls here-->
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Add New Schedule</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <!-- Modal Body-->
                <div class="modal-body">

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Course Code:</label>
                            <asp:TextBox ID="CourseCodeTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Course Code"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Section:</label>
                            <asp:TextBox ID="SectionTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Section"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Faculty:</label>
                            <asp:TextBox ID="FacultyTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Username"></asp:TextBox>
                        </div>


                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Professor/Instructor:</label>
                            <asp:TextBox ID="ProfTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Professor/Instructor"></asp:TextBox>
                        </div>


                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Building:</label>
                            <asp:TextBox ID="BuildingTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Building"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Room Number:</label>
                            <asp:TextBox ID="RoomNumberTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Room Number"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Faculty:</label>
                            <asp:DropDownList ID="FacultyDL" runat="server">
                                <asp:ListItem Text="CCIS" Value="Value1"></asp:ListItem>
                                <asp:ListItem Text="MITL" Value="Value2"></asp:ListItem>
                                <asp:ListItem Text="CMET" Value="Value3"></asp:ListItem>
                                <asp:ListItem Text="ETYCB" Value="Value4"></asp:ListItem>
                                <asp:ListItem Text="CAS" Value="Value5"></asp:ListItem>
                                <asp:ListItem Text="SHS" Value="Value6"></asp:ListItem>
                            </asp:DropDownList>
                        </div>


                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Time:</label>
                            <asp:DropDownList ID="TimeDL" runat="server">
                                <asp:ListItem Text="7:15 AM - 8:15 AM" Value="Value1"></asp:ListItem>
                                <asp:ListItem Text="8:15 AM - 9:30 AM" Value="Value2"></asp:ListItem>
                                <asp:ListItem Text="9:30 AM - 10:45 AM" Value="Value3"></asp:ListItem>
                                <asp:ListItem Text="10:45 AM - 12:00 PM" Value="Value4"></asp:ListItem>
                                <asp:ListItem Text="12:00 PM - 1:15 PM" Value="Value5"></asp:ListItem>
                                <asp:ListItem Text="1:15 PM - 2:30 PM" Value="Value6"></asp:ListItem>
                                <asp:ListItem Text="2:30 PM - 3:45 PM" Value="Value7"></asp:ListItem>
                                <asp:ListItem Text="3:45 PM - 5:00 PM" Value="Value8"></asp:ListItem>
                                <asp:ListItem Text="5:00 PM - 6:15 PM" Value="Value9"></asp:ListItem>
                                <asp:ListItem Text="6:15 PM - 7:30 PM" Value="Value10"></asp:ListItem>
                                <asp:ListItem Text="7:30 PM - 8:45 PM" Value="Value11"></asp:ListItem>




                            </asp:DropDownList>
                        </div>

                        <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>



                </div>
                
            </div>
        </div>
    </div>


<%--</form>
   

    <form id="form1" runat="server">
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1" GridLines="None">
        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#594B9C" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#33276A" />
    </asp:GridView>
    </form>--%>
    
</asp:Content>
