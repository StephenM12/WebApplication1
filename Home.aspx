<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QkE+k9pRTCMF9aXdfMqH9d5cdBOUc+ANiNHlcMz8sHs7pO7mq7CK2reA9IVNJoZy" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="./CSS/Home_Style.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    
    

     
    <form id="form1" runat="server">

        <br />
        <br />
        <div class="container mx-auto">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </h1>
        </div>

        <div class="container mt-4">
            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="roomSearch" class="form-label"><strong>Search Rooms:</strong></label>
                    <input type="text" class="form-control" id="roomSearch" placeholder="Search for a room" onkeyup="filterRooms()">
                </div>
                <div class="col-md-8">
                    <label for="buildingSelect" class="form-label"><strong>Select Building:</strong></label>
                    <select class="form-select" id="buildingSelect" onchange="filterRooms()">
                        <option value="RIZAL">Rizal Building</option>
                        <option value="EINSTEIN">Einstein Building</option>
                        <option value="ETYCB">ETYCB Building</option>
                    </select>
                </div>

               <%-- <div class="card-container">
                    <div class="card">
                        <div class="card-header">
                            <h2>Add New Item</h2>
                        </div>
                        <div class="card-body">
                            <asp:Label ID="lblMessage" runat="server" Text="Room Name:" CssClass="message-label"></asp:Label>
                            <asp:TextBox ID="txtNewroom" runat="server" CssClass="form-control" Placeholder="Enter new item"></asp:TextBox>
                            <asp:DropDownList ID="ADD_BuildDL" runat="server">
                                <asp:ListItem Text="RIZAL" Value="RIZAL"></asp:ListItem>
                                <asp:ListItem Text="EINSTEIN" Value="EINSTEIN"></asp:ListItem>
                                <asp:ListItem Text="ETYCB" Value="ETYCB"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="card-footer">
                            <asp:Button ID="btnAddroom" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="addRoomBTN_Click" />
                        </div>
                    </div>
                </div>--%>

            </div>

            <%--original room table--%>
            <div id="roomCards" class="row">
                <!-- Rizal Building Rooms -->
                <asp:Repeater ID="RepeaterRooms" runat="server">
                    <ItemTemplate>
                        <div class="col-md-3 mb-3 room-card" data-building='<%# Eval("Building") %>' id='<%# Eval("RoomID") %>'>
                            <div class="card text-white <%# Eval("CardClass") %>">
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("RoomNumber") %></h5>
                                    <p class="card-text"><%# Eval("Status") %></p>
                                </div>
                            </div>
                        </div>

                        
                    </ItemTemplate>
                </asp:Repeater>

                 <div class="col-md-3 mb-3 room-card"'>
                            <div class="card text-white ">
                                <div class="card-body">
                                    <h5 class="card-title">"Not Enough Room?"</h5>
                                    <p class="card-text"></p>
                                    <asp:Button ID="Button1" runat="server" Text="Button" />
                                </div>
                            </div>
                        </div>

            </div>
        </div>
        <div id="add_R_B">
            <asp:Button ID="addBuild" runat="server" Text="Add BUILDING" CssClass="lower-left bg-color btn btn-primary bg-color"  />

            <asp:Button ID="addRm" runat="server" Text="ADD ROOM" CssClass="lower-right bg-color btn btn-primary bg-color"  />

        </div>
        

        <asp:PlaceHolder runat="server">
            <script src="Scripts/home.js"></script>
        </asp:PlaceHolder>
    </form>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            filterRooms();
        });

        function filterRooms() {
            var selectedBuilding = document.getElementById("buildingSelect").value;
            var searchQuery = document.getElementById("roomSearch").value.toLowerCase();
            var rooms = document.getElementsByClassName("room-card");

            for (var i = 0; i < rooms.length; i++) {
                var room = rooms[i];
                var building = room.getAttribute("data-building");
                var roomTitle = room.getElementsByClassName("card-title")[0].innerText.toLowerCase();

                if (building === selectedBuilding && roomTitle.includes(searchQuery)) {
                    room.style.display = "block";
                } else {
                    room.style.display = "none";
                }
            }
        }
    </script>
</asp:Content>
