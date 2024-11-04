<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="./CSS/Home_Style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <!-- Loading overlay -->
        <div id="loadingOverlay" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%; background:rgba(255,255,255,0.8); z-index:9999;">
            <img src="\Images\Half circle.gif" alt="Loading..." style="position:absolute; top:50%; left:50%; transform:translate(-50%, -50%);">
        </div>
        <div class="container mx-auto mt-4">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </h1>
            <div class="col-md-3 mb-3">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <label for="building">Select Building</label>
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control" OnSelectedIndexChanged="BindSelectedBuild" AutoPostBack="True"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<label id="openModalLabel" style="cursor: pointer; color: blue;">Need to add something?</label>--%>
                <asp:Label ID="openModalLabel" runat="server" style="cursor: pointer; color: blue;" Text="Need to add something?"></asp:Label>

            </div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
                    <div class="col-md-9">
                        <div class="container">
                            <div class="row">
                               <%-- <asp:Repeater ID="RoomRepeater" runat="server">
                                    <ItemTemplate>
                                        <div class="col-md-3 mb-3 d-flex justify-content-center">
                                            <div class="card text-white bg-success" style="width: 15rem;">
                                                <div class="card-body">
                                                    <h5 class="card-title"><%# Eval("RoomName") %></h5>
                                                    <p class="card-text">Available</p>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>--%>
                                <asp:Repeater ID="RoomRepeater" runat="server">
                                    <ItemTemplate>
                                        <div class="col-md-3 mb-3 d-flex justify-content-center">
                                            <div class="card text-white <%# Eval("Availability").ToString() == "Occupied" ?  "bg-danger" : "bg-success"  %>" style="width: 15rem;">
                                                <div class="card-body">
                                                    <h5 class="card-title"><%# Eval("RoomName") %></h5>
                                                    <p class="card-text"><%# Eval("Availability") %></p>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                <ContentTemplate>
                    <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="myModalLabel">Add Items</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body">
                                    <div class="mb-3">
                                        <label for="addRoom_DropDownList" class="form-label">Rooms</label>
                                        <div class="input-group">
                                            <asp:DropDownList 
                                                ID="addRoom_DropDownList" 
                                                runat="server" 
                                                CssClass="form-select">
                                            </asp:DropDownList>
                                            <button class="btn btn-outline-secondary" type="button" data-bs-toggle="modal" data-bs-target="#addRoomModal">
                                            <i class="fas fa-plus-circle"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <label for="addBuild_DropDownList" class="form-label">Buildings</label>
                                        <div class="input-group">
                                            <asp:DropDownList 
                                                ID="addBuild_DropDownList" 
                                                runat="server" 
                                                CssClass="form-select">
                                            </asp:DropDownList>
                                            <button class="btn btn-outline-secondary" type="button" data-bs-toggle="modal" data-bs-target="#addBuildingModal">
                                            <i class="fas fa-plus-circle"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <label for="addFaculty_DropDownList" class="form-label">Faculty</label>
                                        <div class="input-group">
                                            <asp:DropDownList 
                                                ID="addFaculty_DropDownList" 
                                                runat="server" 
                                                CssClass="form-select">
                                            </asp:DropDownList>
                                            <button class="btn btn-outline-secondary" type="button" data-bs-toggle="modal" data-bs-target="#addFacultyModal">
                                            <i class="fas fa-plus-circle"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Button to ADD Room trigger the modal -->
            <%--<asp:Button ID="addRm" runat="server" Text="ADD ROOM" CssClass="btn btn-primary" OnClientClick="$('#addRoomModal').modal('show'); return false;" />--%>
            <!-- Button to ADD building trigger the modal -->
            <%--<asp:Button ID="addBuild" runat="server" Text="ADD BUILDING" CssClass="btn btn-primary" OnClientClick="$('#addBuildingModal').modal('show'); return false;" />--%>
            <!-- Modal HTML for adding room -->
            <div class="modal fade" id="addRoomModal" tabindex="-1" aria-labelledby="addRoomModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addRoomModalLabel">Add Room</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                <ContentTemplate>
                                    <div class="mb-3">
                                        <label for="txtRoomName" class="form-label">Room Name</label>
                                        <asp:TextBox ID="txtRoomName" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label for="ddlBuildings" class="form-label">Select Building</label>
                                        <asp:DropDownList ID="ddlBuildings" runat="server" CssClass="form-control" />
                                        <asp:Label ID="lblRoomError" runat="server" Text="" CssClass="text-danger"></asp:Label>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmAddRoom" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnConfirmAddRoom" runat="server" Text="Add Room" CssClass="btn btn-primary" OnClick="btnConfirmAddRoom_Click" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal HTML for adding building -->
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
                                    <asp:Label ID="lblSuccessMessage" runat="server" CssClass="alert alert-success" visible="false"></asp:Label>
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
            <!-- Add Faculty -->
            <div class="modal fade" id="addFacultyModal" tabindex="-1"  aria-labelledby="addFacultyModalLabel" aria-hidden="true">
                <!-- Modal content -->
                <div class="modal-dialog" >
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Add New Faculty</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
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
        </div>
        <script>
            $(document).ready(function () {
                // Handle click event on the label
                $('#<%= openModalLabel.ClientID %>').on('click', function () {
                    // Show your modal here
                    $('#myModal').modal('show');
                });
            });
            //$(document).ready(function () {
            //    $('#openModalLabel').on('click', function () {
            //        $('#myModal').modal('show');
            //    });
            //});

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
        </script>
    </form>
</asp:Content>