<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RRForm.aspx.cs" Inherits="RoomRequestForm.RRForm" EnableEventValidation="false" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
        <!-- jQuery -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <!-- Bootstrap JS -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Custom CSS -->
        <link rel="stylesheet" href="./CSS/RRForm.css" />
        <!-- Bootstrap CSS -->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />

        </head>
    <body>
        <form id="form1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <div class="container mt-3">
                <div class="card-body p-md-3 mx-md-2">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="text-center">
                                <img src="Images/MMCL.png" style="width: 130px;" alt="logo" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="header-right">
                                <h4 class="mt-1 mb-1 pb-1 text-center">Permit to Use Mapua MCL Facilities and Items | PTX</h4>
                                <h4 class="mt-1 mb-1 pb-1 text-center">For Co-curricular, Extra curricular and Rentals</h4>
                                <h4 class="mt-1 mb-1 pb-1 text-center">FORM OVPA-002A</h4>
                                <h4 class="mt-1 mb-1 pb-1 text-center">Revision No: 03</h4>
                                <h4 class="mt-1 mb-1 pb-1 text-center">Revision Date: 1/03/2014</h4>
                            </div>
                        </div>
                    </div>
                    <hr class="custom-line" />
                    <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="RSectionTB">Section:</label>
                        <asp:TextBox ID="RSectionTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Section"></asp:TextBox>
                    </div>
                    <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="RCourseCodeTB">Course Code:</label>
                        <asp:TextBox ID="RCourseCodeTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Course Code"></asp:TextBox>
                    </div>
                    <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="RProfTB">Professor/Instructor:</label>
                        <asp:TextBox ID="RProfTB" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Professor/Instructor"></asp:TextBox>
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-outline mb-3">
                                <label style="font-weight: bold;" class="form-label ms-2" for="SelectBuildingDL">
                                    Select Building:
                                    <asp:Panel ID="PlusIconPanel" runat="server" Style="display: inline;">
                                        <i class="fas fa-plus-circle ms-2" style="cursor: pointer;" data-bs-toggle="modal" data-bs-target="#addBuildingModal"></i>
                                    </asp:Panel>
                                </label>
                                <asp:DropDownList ID="SelectBuildingDL" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="CheckForAllInputs" />
                            </div>
                            <div class="form-outline mb-3">
                                <label style="font-weight: bold;" class="form-label ms-2" for="RRoomNumberTB">
                                    Room Number:
                                    <asp:Panel ID="Panel1" runat="server" Style="display: inline;">
                                        <i class="fas fa-plus-circle ms-2" style="cursor: pointer;" data-bs-toggle="modal" data-bs-target="#addRoomModal"></i>
                                    </asp:Panel>
                                </label>
                                <asp:DropDownList ID="SelectRoomDL" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="CheckForAllInputs" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="RFacultyDL">
                            Faculty:
                            <asp:Panel ID="Panel2" runat="server" Style="display: inline;">
                                <i class="fas fa-plus-circle ms-2" style="cursor: pointer;" data-bs-toggle="modal" data-bs-target="#addFacultyModal"></i>
                            </asp:Panel>
                        </label>
                        <asp:DropDownList ID="RFacultyDL" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-outline mb-3">
                                <label style="font-weight: bold;" class="form-label ms-2" for="SelectDateTB">Select Date:</label>
                                <asp:TextBox ID="SelectDateTB" runat="server" TextMode="Date" CssClass="form-control" AutoPostBack="true" OnTextChanged="CheckForAllInputs"></asp:TextBox>
                            </div>
                            <div class="form-outline mb-3">
                                <label style="font-weight: bold;" class="form-label ms-2" for="EndDateTB">Select End Date:</label>
                                <asp:TextBox ID="EndDateTB" runat="server" TextMode="Date" CssClass="form-control" AutoPostBack="true" OnTextChanged="CheckForAllInputs"></asp:TextBox>
                            </div>
                            <div class="form-outline mb-3">
                                <label style="font-weight: bold;" class="form-label ms-2" for="RTimeDL">Select StartTime:</label>
                                <asp:DropDownList ID="STimeDL" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="CheckForAllInputs">
                                    <asp:ListItem Text="7:00 AM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="8:15 AM" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="9:30 AM" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="10:45 AM" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="12:00 PM" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="1:15 PM" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="2:30 PM" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="3:45 PM" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="5:00 PM" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="6:15 PM" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="7:30 PM" Value="11"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-outline mb-3">
                                <label style="font-weight: bold;" class="form-label ms-2" for="RTimeDL">Select EndTime:</label>
                                <asp:DropDownList ID="ETimeDL" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="CheckForAllInputs">
                                    <asp:ListItem Text="8:15 AM" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="9:30 AM" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="10:45 AM" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="12:00 PM" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="1:15 PM" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="2:30 PM" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="3:45 PM" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="5:00 PM" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="6:15 PM" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="7:30 PM" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="8:45 PM" Value="11"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                    
                    <div class="card rounded border-warning mb-3" style="max-width: 30rem; display: none;" runat="server" id="ConflictCard">
                        <div class="card-header bg-warning text-white">
                            Note!
                        </div>
                        <div class="card-body">
                            <asp:Label ID="ConflictLabel" runat="server" CssClass="card-text"
                                Text="Your request may experience delays due to a schedule conflict.<br />Thank you for your understanding!"
                                Visible="false"></asp:Label>
                        </div>
                    </div>
                                </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="RRpurpose">Purpose:</label>
                        <asp:TextBox ID="RRpurpose" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Purpose:"></asp:TextBox>
                    </div>
                   <%-- <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="email">Email:</label>
                        <asp:TextBox ID="email" runat="server" class="form-control" Style="background-color: #ECECEC;" placeholder="Enter Requester's Email"></asp:TextBox>
                    </div>--%>
                    <hr class="custom-line" />
                    <div class="form-outline mb-3">
                        <label style="font-weight: bold;" class="form-label ms-2" for="fileUpload">Attach File:</label>
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control" />
                    </div>
                    <hr class="custom-line" />
                    <div class="d-flex justify-content-center">
                        <asp:Button ID="btn_Export" runat="server" Text="Submit" OnClick="BtnExport_Click" />
                        <asp:Button ID="btn_Back" runat="server" Text="Back" OnClick="Btn_Back" />
                    </div>
                </div>
            </div>
            <!-- Add Faculty -->
            <div class="modal fade" id="addFacultyModal" tabindex="-1" aria-labelledby="addFacultyModalLabel" aria-hidden="true">
                <!-- Modal content -->
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Add New Faculty</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtNewFacultyName" runat="server" CssClass="form-control" Placeholder="Enter Faculty"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label1facult" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddFaculty" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAddFaculty" runat="server" Text="Add Faculty" CssClass="btn btn-primary" OnClick="btnAddFaculty_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Add Room Modal -->
            <div class="modal fade" id="addRoomModal" tabindex="-1" role="dialog" aria-labelledby="addRoomModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addRoomModalLabel">Add Room</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label for="roomName">Room Name</label>
                                        <asp:TextBox ID="txtRoomName" runat="server" CssClass="form-control" oninput="validateRoomForm()" />
                                    </div>
                                    <div class="form-group">
                                        <label for="building">Select Building</label>
                                        <asp:DropDownList ID="ddlBuildings" runat="server" CssClass="form-control" onchange="validateRoomForm()" />
                                        <br />
                                        <asp:Label ID="lblRoomError" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                    </div>
                        </div>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConfirmAddRoom" EventName="Click" />
                        </Triggers>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <asp:Button ID="btnConfirmAddRoom" runat="server" Text="Add Room" CssClass="btn btn-primary" OnClick="btnConfirmAddRoom_Click" Enabled="true" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
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
        </form>
    </body>
</html>