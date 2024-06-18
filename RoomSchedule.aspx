<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoomSchedule.aspx.cs"
    Inherits="WebApplication1.RoomSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/RoomSchedule_Style.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script src="Scripts/script.js"></script>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div>
            <div class="row">
                <%--make this ToolTip false later--%>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Upload file" OnClick="Upload_File" />
            </div>
            <div>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Bind_Uploaded_GridView">
                </asp:DropDownList>
            </div>
            <div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="row align-items-center">
                <%--                to save the file that been uploaded--%>

                <%--<div class=" calendar-container col-3">
                    <asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged"></asp:Calendar>
                </div>--%>

                <div class="schedule-container col-sm-9">
                    <asp:GridView ID="GridView1" runat="server" CssClass="schedule-gridview" BackColor="White" BorderColor="White" BorderStyle="Ridge"
                        BorderWidth="2px" CellPadding="3" CellSpacing="1" OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        EmptyDataText="No records found">

                        <%--make this ToolTip false later--%>
                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" CssClass="schedule-gridview" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" CssClass="schedule-gridview" />
                        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#594B9C" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#33276A" />

                        <Columns>
                            <asp:BoundField DataField="Column 1" HeaderText="Time" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 2" HeaderText="Monday" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 3" HeaderText="Tuesday" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 4" HeaderText="Wednesday" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 5" HeaderText="Thursday" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 6" HeaderText="Friday" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 7" HeaderText="Saturday" />
                        </Columns>
                        <Columns>
                            <asp:BoundField DataField="Column 8" HeaderText="Sunday" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Start of Modal Button-->
        <asp:Button ID="RAddSchedBtn" runat="server" Text="ADD TO SCHEDULE" CssClass="lower-right bg-color btn btn-primary bg-color" OnClientClick="openModal(); return false;" />
        <script type="text/javascript">
            function openModal() {
                $('#exampleModal').modal('show');
            }
        </script>

        <!--  Modal Button for EDIT/CANCEL SCHEDULE-->
        <asp:Button ID="REditBtn" runat="server" Text="EDIT/CANCEL SCHEDULE" CssClass="lower-left bg-color btn btn-primary bg-color" OnClientClick="openSched(); return false;" />

        <script type="text/javascript">
            function openSched() {
                $('#RoomEdit').modal('show');
            }
        </script>

        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <br>
            <div class="modal-dialog modal-fullscreen-xxl-down">
                <div class="modal-content">

                    <!-- Put ASP Controls here-->
                    <div class="modal-header">
                        <h5 class="modal-title" style="font-weight: bold;" id="exampleModalLabel">Add New Schedule</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <!-- Modal Body-->
                    <div class="modal-body">

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Course Code:</label>
                            <asp:TextBox ID="RCourseCodeTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Course Code"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Section:</label>
                            <asp:TextBox ID="RSectionTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Section"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Professor/Instructor:</label>
                            <asp:TextBox ID="RProfTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Professor/Instructor"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Room Number:</label>
                            <asp:TextBox ID="RRoomNumberTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Room Number"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Building:</label>
                            <asp:DropDownList ID="SelectBuildingDL" runat="server">
                                <asp:ListItem Text="RIZAL" Value="Value1"></asp:ListItem>
                                <asp:ListItem Text="EINSTEIN" Value="Value2"></asp:ListItem>
                                <asp:ListItem Text="ETYCB" Value="Value3"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Faculty:</label>
                            <asp:DropDownList ID="RFacultyDL" runat="server">
                                <asp:ListItem Text="CCIS" Value="Value1"></asp:ListItem>
                                <asp:ListItem Text="MITL" Value="Value2"></asp:ListItem>
                                <asp:ListItem Text="CMET" Value="Value3"></asp:ListItem>
                                <asp:ListItem Text="ETYCB" Value="Value4"></asp:ListItem>
                                <asp:ListItem Text="CAS" Value="Value5"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Start Date in Modal Button-->
                        <div class="form-outline mb-4">
                            <div class="row align-items-center">
                                <div class="col-sm-6">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Date:</label>
                                    <asp:TextBox ID="SelectDateTB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>

                                <!-- End Date in Modal Button-->
                                <div class="col-sm-6">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select End Date:</label>
                                    <asp:TextBox ID="EndDateTB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Time:</label>
                            <asp:DropDownList ID="RTimeDL" runat="server">
                                <asp:ListItem Text="7:15 AM - 8:15 AM" Value="1"></asp:ListItem>
                                <asp:ListItem Text="8:15 AM - 9:30 AM" Value="2"></asp:ListItem>
                                <asp:ListItem Text="9:30 AM - 10:45 AM" Value="3"></asp:ListItem>
                                <asp:ListItem Text="10:45 AM - 12:00 PM" Value="4"></asp:ListItem>
                                <asp:ListItem Text="12:00 PM - 1:15 PM" Value="5"></asp:ListItem>
                                <asp:ListItem Text="1:15 PM - 2:30 PM" Value="6"></asp:ListItem>
                                <asp:ListItem Text="2:30 PM - 3:45 PM" Value="7"></asp:ListItem>
                                <asp:ListItem Text="3:45 PM - 5:00 PM" Value="8"></asp:ListItem>
                                <asp:ListItem Text="5:00 PM - 6:15 PM" Value="9"></asp:ListItem>
                                <asp:ListItem Text="6:15 PM - 7:30 PM" Value="10"></asp:ListItem>
                                <asp:ListItem Text="7:30 PM - 8:45 PM" Value="11"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <br />
                        <asp:Button ID="DeployBtn" runat="server" Text="Deploy" CssClass="btn btn-primary btn-block full-width bg-color fa-lg" OnClick="DeployBTNclk" />
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="RoomEdit" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-fullscreen">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="EditSchedule">Edit Schedule</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <!--Content of Edit Schedule / Gridview Put Here-->
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="RCloseBtn" runat="server" Text="Close" CssClass="bg-color btn btn-primary bg-color" />
                        <asp:Button ID="RSaveChangesBtn" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>