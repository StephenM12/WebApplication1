<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QkE+k9pRTCMF9aXdfMqH9d5cdBOUc+ANiNHlcMz8sHs7pO7mq7CK2reA9IVNJoZy" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="./CSS/Home_Style.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />

    <%--    for modal--%>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>

    <form id="form1" runat="server">

        <br />
        <br />
        <div class="container mx-auto">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </h1>
        </div>

        <%--        this area is for card na--%>

          <div class="container">
            <div class="row">
                <asp:Repeater ID="RoomRepeater" runat="server">
                    <ItemTemplate>

                        <div class="col-md-3 mb-3 d-flex justify-content-center">
                            <div class="card text-white bg-success" style="width: 15rem;">

                                <div class="card-body">
                                    <h5 class="card-title" ><%# Eval("RoomName") %></h5>
                                    <p class="card-text" ">Available</p>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div id="add_R_B">
        <%--            <asp:Button ID="addBuild" runat="server" Text="Add BUILDING" CssClass="lower-left bg-color btn btn-primary bg-color" />--%>


            <!-- Button to ADD trigger the modal -->
            <asp:Button ID="addBuild" runat="server" Text="Add BUILDING" CssClass="lower-left bg-color btn btn-primary" OnClientClick="$('#addBuildingModal').modal('show'); return false;" />

            <!-- Modal HTML -->
            <div class="modal fade" id="addBuildingModal" tabindex="-1" role="dialog" aria-labelledby="addBuildingModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addBuildingModalLabel">Add Building</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblError" runat="server" Text="" CssClass="text-danger"></asp:Label>
                            <div class="form-group">
                                <label for="buildingName">Building Name</label>
                                <asp:TextBox ID="txtBuildingName" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnConfirmAdd" runat="server" Text="Add Building" CssClass="btn btn-primary" OnClick="btnConfirmAdd_Click" />
                        </div>
                    </div>
                </div>
            </div>



            <%--            <asp:Button ID="addRm" runat="server" Text="ADD ROOM" CssClass="lower-right bg-color btn btn-primary bg-color" />--%>

            <!-- Button to trigger the modal -->
            <asp:Button ID="addRm" runat="server" Text="ADD ROOM" CssClass="lower-right bg-color btn btn-primary" OnClientClick="$('#addRoomModal').modal('show'); return false;" />

            <!-- Modal HTML -->
            <div class="modal fade" id="addRoomModal" tabindex="-1" role="dialog" aria-labelledby="addRoomModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="addRoomModalLabel">Add Room</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblRoomError" runat="server" Text="" CssClass="text-danger"></asp:Label>
                            <div class="form-group">
                                <label for="roomName">Room Name</label>
                                <asp:TextBox ID="txtRoomName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="form-group">
                                <label for="building">Select Building</label>
                                <asp:DropDownList ID="ddlBuildings" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnConfirmAddRoom" runat="server" Text="Add Room" CssClass="btn btn-primary" OnClick="btnConfirmAddRoom_Click" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
</asp:Content>