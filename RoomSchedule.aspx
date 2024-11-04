<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoomSchedule.aspx.cs"
   Inherits="WebApplication1.RoomSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <link rel="stylesheet" href="./CSS/RoomSchedule_Style.css" />
   <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.css">
   <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.3/dist/sweetalert2.min.js"></script>
   <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
   <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
   <link href="https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.css" rel="stylesheet" />
   <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
   <script src="https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.js"></script>
   <script src="Scripts/script.js"></script>
   <form id="form1" runat="server">
      <asp:ScriptManager runat="server"></asp:ScriptManager>
      <!-- HiddenField for DayOfWeek -->
      <asp:HiddenField ID="hiddenDayOfWeek" runat="server" />
      <!-- Loading overlay -->
      <div id="loadingOverlay" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%; background:rgba(255,255,255,0.8); z-index:9999;">
         <img src="\Images\Half circle.gif" alt="Loading..." style="position:absolute; top:50%; left:50%; transform:translate(-50%, -50%);">
      </div>
      <div>
         <div class="content-wrapper">
            <div class="left-panel">
               <div>
                  <asp:UpdatePanel runat="server">
                     <ContentTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BindScheduleData" ></asp:DropDownList>
                        <br />
                        <asp:DropDownList ID="uploadSchedsDL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="BindScheduleData" ></asp:DropDownList>
                        <br />
                     </ContentTemplate>
                  </asp:UpdatePanel>
               </div>
               <br />
               <!-- FullCalendar Placeholder -->
               <div id="">
                  <asp:UpdatePanel runat="server">
                     <ContentTemplate>
                        <asp:HiddenField ID="HiddenFieldSelectedDate" runat="server" />
                     </ContentTemplate>
                  </asp:UpdatePanel>
                  <asp:Calendar ID="Calendar3" runat="server" OnSelectionChanged="Calendar3_SelectionChanged"></asp:Calendar>
               </div>
               <!-- Place the Today button on top of the three buttons -->
               <div class="buttons">
                  <!-- Button to open the modal -->
                  <div class="button-container">
                     <asp:Button ID="RUploadFileBtn" runat="server" Text="Choose File" CssClass="button" OnClientClick="openupload(); return false;" />
                     <%--modal for add sched--%>
                     <asp:Button ID="RAddSchedBtn" runat="server" Text="ADD" CssClass="button" OnClientClick="openModal(); return false;" />
                     <%--reset button--%>
                     <asp:Button ID="Button2" runat="server" Text="Reset" CssClass="button" OnClientClick="showConfirmationModal(); return false;" />
                     <!--  Modal Button for EDIT/CANCEL SCHEDULE-->
                     <asp:Button ID="REditBtn" runat="server" Text="EDIT SCHEDULE" CssClass="button" OnClientClick="openSched(); return false;" />
                  </div>
               </div>
            </div>
            <div class="right-panel">
               <asp:UpdatePanel runat="server" ID="UpdatePanelGridView">
                  <ContentTemplate>
                     <asp:HiddenField ID="hiddenToggle" runat="server" Value="false" />
                     <%--orig--%> 
                     <%-- <asp:GridView ID="GridView1" runat="server" DataKeyNames="ScheduleID" OnRowCommand="GridView1_RowCommand" CssClass="schedule-grid"  AutoGenerateColumns="false"  EmptyDataText="No records found" >   
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
                        </asp:GridView>--%>
                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"  CssClass="schedule-grid" EmptyDataText="No records found" >
                        <Columns>
                           <asp:BoundField DataField="Time" HeaderText="Time" />
                           <asp:TemplateField HeaderText="Sunday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkSunday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("SundayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Sunday") %>
                                 </asp:LinkButton>
                              </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Monday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkMonday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("MondayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Monday") %>
                                 </asp:LinkButton>
                              </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Tuesday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkTuesday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("TuesdayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Tuesday") %>
                                 </asp:LinkButton>
                              </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Wednesday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkWednesday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("WednesdayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Wednesday") %>
                                 </asp:LinkButton>
                              </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Thursday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkThursday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("ThursdayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Thursday") %>
                                 </asp:LinkButton>
                              </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Friday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkFriday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("FridayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Friday") %>
                                 </asp:LinkButton>
                              </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Saturday">
                              <ItemTemplate>
                                 <asp:LinkButton ID="lnkSaturday" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("SaturdayScheduleID") %>' CssClass="no-underline">
                                    <%# Eval("Saturday") %>
                                 </asp:LinkButton>
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
      </div>
      <!-- Upload Modal -->
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
                       <asp:UpdatePanel ID="updatePanel1" runat="server">
                                <ContentTemplate>
                     <label style="font-weight: bold;" class="form-label ms-3" for="form3Example1cg">Select Building:</label>
                      
                     <asp:DropDownList ID="upload_DropDownList1" runat="server"></asp:DropDownList>
                                    
                     <asp:Panel ID="PlusIconPanel" runat="server" Style="display: inline;">
                        <i class="fas fa-plus-circle ms-2" style="cursor: pointer;" data-bs-toggle="modal" data-bs-target="#addBuildingModal"></i>
                     </asp:Panel>
                                     </ContentTemplate>
               </asp:UpdatePanel>
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
       <!-- Add Building Modal -->
            <div class="modal fade" id="addBuildingModal" tabindex="-1" role="dialog" aria-labelledby="addBuildingModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addBuildingModalLabel">Add Building</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="updatePanelBuilding" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBuildingName" runat="server" CssClass="form-control" Placeholder="Enter building name"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblSuccessMessage" runat="server" CssClass="alert alert-success" Visible="false"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddBuilding" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddBuilding" runat="server" Text="Add Building" CssClass="btn btn-primary" OnClick="btnAddBuilding_Click" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
      <%--schedule modal--%>
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
                        <%--  <div class="form-group">
                           <asp:Label ID="lblBuildingID" runat="server" Text="Building Name:" AssociatedControlID="txtBuildingID"></asp:Label>
                           <asp:TextBox ID="txtBuildingID" runat="server" CssClass="form-control" ReadOnly="True" />
                           </div>
                           <div class="form-group">
                           <asp:Label ID="lblRoomID" runat="server" Text="Room Name:" AssociatedControlID="txtRoomID"></asp:Label>
                           <asp:TextBox ID="txtRoomID" runat="server" CssClass="form-control" ReadOnly="True" />
                           </div>--%>
                        <div class="form-group">
                           <asp:Label ID="lblCourseID" runat="server" Text="Course Name:" AssociatedControlID="txtCourseID"></asp:Label>
                           <asp:TextBox ID="txtCourseID" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                        <div class="form-group">
                           <asp:Label ID="lblSectionID" runat="server" Text="Section Name:" AssociatedControlID="txtSectionID"></asp:Label>
                           <asp:TextBox ID="txtSectionID" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                        <div class="form-group">
                           <asp:Label ID="lblInstructorID" runat="server" Text="Instructor Name:" AssociatedControlID="txtInstructorID"></asp:Label>
                           <asp:TextBox ID="txtInstructorID" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                       <%-- <div class="form-group">
                           <asp:Label ID="lbltxtDate" runat="server" Text="Schedule Date:" AssociatedControlID="txtDate"></asp:Label>
                           <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" ReadOnly="True" />
                        </div>--%>
                        <%-- <div class="form-group">
                           <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate"></asp:Label>
                           <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" ReadOnly="True" />
                           </div>
                           <div class="form-group">
                           <asp:Label ID="lblEndDate" runat="server" Text="End Date:" AssociatedControlID="txtEndDate"></asp:Label>
                           <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" ReadOnly="True" />
                           </div>--%>
                        <div class="form-group">
                           <asp:Label ID="lblRemarks" runat="server" Text="Remarks:" AssociatedControlID="txtRemarks"></asp:Label>
                           <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
               </div>
               </ContentTemplate>
               </asp:UpdatePanel>
               <div class="modal-footer">
                  <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                     <ContentTemplate>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnUpdate_Click" Visible="false" />
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                        <asp:Button ID="Button3" runat="server" Text="Remove" CssClass="btn btn-danger" OnClick="btnRemove_Click"/>
                     </ContentTemplate>
                  </asp:UpdatePanel>
                  <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-secondary" data-bs-dismiss="modal" />
               </div>
            </div>
         </div>
      </div>
      </div>
      <!-- add sched modal -->
      <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
         <br>
         <div class="modal-dialog">
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
                     <asp:DropDownList ID="add_Dropdown_room" runat="server" >
                     </asp:DropDownList>
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
                  <asp:Button ID="DeployBtn" runat="server" Text="Deploy" CssClass="btn btn-primary btn-block full-width bg-color fa-lg" OnClick="DeployBTNclk"/>
               </div>
            </div>
         </div>
      </div>
      <%--Room edit modal--%>
      <div class="modal fade" id="RoomEdit" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
         <div class="modal-dialog">
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
      <!-- Confirmation Modal -->
      <div class="modal fade" id="confirmationModal" tabindex="-1" aria-labelledby="confirmationModalLabel" aria-hidden="true">
         <div class="modal-dialog">
            <div class="modal-content">
               <div class="modal-header">
                  <h5 class="modal-title" id="confirmationModalLabel">Confirm Reset</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
               </div>
               <div class="modal-body">
                  Are you sure you want to reset the page? This action cannot be undone.
               </div>
               <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                  <asp:Button ID="btnConfirmReset" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="Reset_Page" />
               </div>
            </div>
         </div>
      </div>
      <script type="text/javascript">
         function showConfirmationModal() {
             $('#confirmationModal').modal('show');
         }
         
         // Show the loading GIF when form is being submitted or button clicked
         $("form").on("submit", function () {
             $("#loadingOverlay").fadeIn();
         });
         
         // Hide the loading GIF after the page has fully loaded
         $(window).on("load", function () {
             $("#loadingOverlay").fadeOut(); // Hide loading gif
         });
         
         
          var prm = Sys.WebForms.PageRequestManager.getInstance();
          prm.add_endRequest(function () {
              // Hide loading gif after partial postbacks
              $("#loadingOverlay").fadeOut();
          });
         
         function openupload() {
             $('#uploadModal').modal('show');
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

          function openSched() {
              $('#RoomEdit').modal('show');
          }
          function openModal() {
              $('#exampleModal').modal('show');
          }



      </script>
   </form>
</asp:Content>