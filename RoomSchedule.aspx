<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoomSchedule.aspx.cs"
    Inherits="WebApplication1.RoomSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/RoomSchedule_Style.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.css">

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">

    <script src="Scripts/script.js"></script>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hiddenDayOfWeek" runat="server" />

         <!-- Loading overlay -->
        <div id="loadingOverlay" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%; background:rgba(255,255,255,0.8); z-index:9999;">
            <img src="\Images\Half circle.gif" alt="Loading..." style="position:absolute; top:50%; left:50%; transform:translate(-50%, -50%);">
        </div>

        <div>
            <div class="row">
                <!-- Button to open the modal -->
                <div class="button-container">
                    <asp:Button ID="RUploadFileBtn" runat="server" Text="Choose File" CssClass="upload-button btn btn-primary bg-color" OnClientClick="openupload(); return false;" />
                </div>
            </div>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BindScheduleData"></asp:DropDownList>
                    <br />
                    <asp:DropDownList ID="uploadSchedsDL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BindScheduleData"></asp:DropDownList>
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Calendar ID="Calendar3" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>

        <%--gridview for roomsched--%>
        <div class="d-flex justify-content-center align-items-center" style="height: 80vh;">
            <div class="schedule-container col-sm-6">
                <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
                    <ContentTemplate>

                        <%--this is for selected date--%>
                        <asp:HiddenField ID="HiddenField2" runat="server" />

                        <asp:GridView ID="GridView1" runat="server" DataKeyNames="ScheduleID" OnRowCommand="GridView1_RowCommand" CssClass="schedule-gridview" BackColor="White" BorderColor="White" BorderStyle="Ridge"
                            BorderWidth="2px" CellPadding="3" CellSpacing="1" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True"
                            EmptyDataText="No records found">
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

                                <asp:BoundField DataField="Time" HeaderText="Time" />
                                <asp:TemplateField HeaderText="Monday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMonday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Monday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tuesday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTuesday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Tuesday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Wednesday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkWednesday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Wednesday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Thursday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkThursday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Thursday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Friday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFriday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Friday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Saturday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSaturday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Saturday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sunday">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSunday" runat="server" CommandName="ShowModal" CommandArgument='<%# Eval("ScheduleID") %>' Text='<%# Eval("Sunday") %>' CssClass="no-underline" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div>
            </div>
        </div>

        <!-- Start of Modal Button-->
        <asp:Button ID="RAddSchedBtn" runat="server" Text="ADD TO SCHEDULE" CssClass="lower-right bg-color btn btn-primary bg-color" OnClientClick="openModal(); return false;" />

        <!--  Modal Button for EDIT/CANCEL SCHEDULE-->
        <asp:Button ID="REditBtn" runat="server" Text="EDIT SCHEDULE" CssClass="lower-left bg-color btn btn-primary bg-color" OnClientClick="openSched(); return false;" />

       


        <!-- Modal -->
        <div class="modal fade" id="uploadModal" tabindex="-1" aria-labelledby="uploadModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="uploadModalLabel">Upload File</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <!-- ASP.NET Controls -->
                        <div class="mb-3">
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Building:</label>
                            <asp:DropDownList ID="upload_DropDownList1" runat="server"></asp:DropDownList>
                        </div>
                        <div class="form-outline mb-4">
                            <div class="row align-items-center">

                                <!-- Start Date in Modal Button-->
                                <div class="col-sm-6">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Date:</label>
                                    <asp:TextBox ID="calendar_TB1" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>

                                <!-- End Date in Modal Button-->
                                <div class="col-sm-6">
                                    <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select End Date:</label>
                                    <asp:TextBox ID="calendar_TB2" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />


                        <div class="modal-footer">
                            <asp:Button ID="Button1" runat="server" Text="Upload file" OnClick="Upload_File" CssClass="btn btn-primary bg-color"  />
                            <asp:Button ID="RUCancelBtn" runat="server" Text="Cancel" CssClass="bg-color btn btn-primary bg-color" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="scheduleModal" tabindex="-1" role="dialog" aria-labelledby="scheduleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="scheduleModalLabel_">Schedule Details</h5>
                        <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <div class="modal-body">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                            <ContentTemplate>
                                <%--this is for scheduleID--%>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <div class="form-group">
                                    <asp:Label ID="lblBuildingID" runat="server" Text="Building Name:" AssociatedControlID="txtBuildingID"></asp:Label>
                                    <asp:TextBox ID="txtBuildingID" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblRoomID" runat="server" Text="Room Name:" AssociatedControlID="txtRoomID"></asp:Label>
                                    <asp:TextBox ID="txtRoomID" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblCourseID" runat="server" Text="Course Name:" AssociatedControlID="txtCourseID"></asp:Label>
                                    <asp:TextBox ID="txtCourseID" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblSectionID" runat="server" Text="Section Name:" AssociatedControlID="txtSectionID"></asp:Label>
                                    <asp:TextBox ID="txtSectionID" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblInstructorID" runat="server" Text="Instructor Name:" AssociatedControlID="txtInstructorID"></asp:Label>
                                    <asp:TextBox ID="txtInstructorID" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>

                                <%--<div class="form-group">
                                    <asp:Label ID="lblStartTime" runat="server" Text="Start Time:" AssociatedControlID="txtStartTime"></asp:Label>
                                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblEndTime" runat="server" Text="End Time:" AssociatedControlID="txtEndTime"></asp:Label>
                                    <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>--%>
                                <div class="form-group">
                                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate"></asp:Label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblEndDate" runat="server" Text="End Date:" AssociatedControlID="txtEndDate"></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks:" AssociatedControlID="txtRemarks"></asp:Label>
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" ReadOnly="True" />
                                </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="modal-footer">

                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                <ContentTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnUpdate_Click" Visible="false" />
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-secondary" data-bs-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Room Number:</label>
                            <%--<asp:TextBox ID="RRoomNumberTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Room Number"></asp:TextBox>--%>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="add_Dropdown_room" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

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
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Remarks : (Optional)</label>
                            <asp:TextBox ID="RRemarksTB" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Remarks"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Building:</label>
                            <asp:DropDownList ID="ADD_DropDownList1" runat="server"></asp:DropDownList>
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
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Start Time:</label>
                            <asp:DropDownList ID="RTimeStart" runat="server">
                                <asp:ListItem Text="7:00AM" Value="1"></asp:ListItem>
                                <asp:ListItem Text="8:15AM" Value="2"></asp:ListItem>
                                <asp:ListItem Text="9:30AM" Value="3"></asp:ListItem>
                                <asp:ListItem Text="10:45AM" Value="4"></asp:ListItem>
                                <asp:ListItem Text="12:00PM" Value="5"></asp:ListItem>
                                <asp:ListItem Text="1:15PM" Value="6"></asp:ListItem>
                                <asp:ListItem Text="2:30PM" Value="7"></asp:ListItem>
                                <asp:ListItem Text="3:45PM" Value="8"></asp:ListItem>
                                <asp:ListItem Text="5:00PM" Value="9"></asp:ListItem>
                                <asp:ListItem Text="6:15PM" Value="10"></asp:ListItem>
                                <asp:ListItem Text="7:30PM" Value="11"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">End Time:</label>
                            <asp:DropDownList ID="RTimeEnd" runat="server">
                                <asp:ListItem Text="8:15AM" Value="1"></asp:ListItem>
                                <asp:ListItem Text="9:30AM" Value="2"></asp:ListItem>
                                <asp:ListItem Text="10:45AM" Value="3"></asp:ListItem>
                                <asp:ListItem Text="12:00PM" Value="4"></asp:ListItem>
                                <asp:ListItem Text="1:15PM" Value="5"></asp:ListItem>
                                <asp:ListItem Text="2:30PM" Value="6"></asp:ListItem>
                                <asp:ListItem Text="3:45PM" Value="7"></asp:ListItem>
                                <asp:ListItem Text="5:00PM" Value="8"></asp:ListItem>
                                <asp:ListItem Text="6:15PM" Value="9"></asp:ListItem>
                                <asp:ListItem Text="7:30PM" Value="10"></asp:ListItem>
                                <asp:ListItem Text="8:45PM" Value="11"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <asp:Button ID="DeployBtn" runat="server" Text="Deploy" CssClass="btn btn-primary btn-block full-width bg-color fa-lg" OnClick="DeployBTNclk" />
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="RoomEdit" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-fullscreen-xxl-down">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="EditSchedule">Edit Schedule</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <!--Content of Edit Schedule / Gridview Put Here-->

                        <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Room To Edit:</label>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="Edit_roomDrodown" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="row align-items-center">
                            <div class="form-outline mb-4">
                                <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Start Time:</label>
                                <asp:DropDownList ID="Edit_DropDownList1" runat="server">
                                    <asp:ListItem Text="7:00AM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="8:15AM" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="9:30AM" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="10:45AM" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="12:00PM" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="1:15PM" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="2:30PM" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="3:45PM" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="5:00PM" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="6:15PM" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="7:30PM" Value="11"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-outline mb-4">
                                <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">End Time:</label>
                                <asp:DropDownList ID="Edit_DropDownList2" runat="server">

                                    <asp:ListItem Text="8:15AM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="9:30AM" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="10:45AM" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="12:00PM" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="1:15PM" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="2:30PM" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="3:45PM" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="5:00PM" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="6:15PM" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="7:30PM" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="8:45PM" Value="11"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Date:</label>
                            <asp:TextBox ID="Edit_Calendar_TextBox1" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Section:</label>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="ESection" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Section"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form-outline mb-4">
                            <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Professor/Instructor:</label>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="EProf" runat="server" class="form-control" Style="background-color: #ECECEC;" type="text" placeholder="Enter Professor/Instructor"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">

                        <asp:Button ID="RCloseBtn" runat="server" Text="Cancel" CssClass="bg-color btn btn-primary bg-color" OnClick="RCloseBtn_Click" />
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="HiddenScheduleID" runat="server" />
                                <asp:Button ID="MatchSchedbtn" Visible="true" runat="server" Text="Look" CssClass="bg-color btn btn-primary bg-color" OnClick="Match_Schedule" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button ID="RSaveChangesBtn" Visible="false" runat="server" Text="Save Changes" CssClass="bg-color btn btn-primary bg-color" OnClick="Edit_BTNclk" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="RSaveChangesBtn" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

        <%--js scripts--%>
        <script>

            // Show the loading GIF when form is being submitted or button clicked
            $("form").on("submit", function () {
                $("#loadingOverlay").fadeIn();
            });

            // Hide the loading GIF after the page has fully loaded
            $(window).on("load", function () {
                $("#loadingOverlay").fadeOut(); // Hide loading gif
            });



           

            function openupload() {
                $('#uploadModal').modal('show');

            }
           

            function openSched() {
                $('#RoomEdit').modal('show');
            }

            document.addEventListener("DOMContentLoaded", function () {
                // Get the upload button and input elements
                var uploadButton = document.getElementById('<%= Button1.ClientID %>');
                var startDate = document.getElementById('<%= calendar_TB1.ClientID %>');
                var endDate = document.getElementById('<%= calendar_TB2.ClientID %>');

                // Disable the upload button initially
                uploadButton.disabled = true;

                // Function to check if the dates are valid
                function validateDates() {
                    if (startDate.value && endDate.value) {
                        uploadButton.disabled = false;
                    } else {
                        uploadButton.disabled = true;
                    }
                }

                // Attach event listeners to the start and end date fields
                startDate.addEventListener('input', validateDates);
                endDate.addEventListener('input', validateDates);
            });
        </script>
    </form>
</asp:Content>