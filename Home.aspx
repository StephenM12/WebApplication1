<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QkE+k9pRTCMF9aXdfMqH9d5cdBOUc+ANiNHlcMz8sHs7pO7mq7CK2reA9IVNJoZy" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="./CSS/Home_Style.css" />

    <form id="form1" runat="server">

        <div class="container mx-auto">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </h1>
        </div>

        <div class="container mt-4">
            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="roomSearch" class="form-label">Search Rooms:</label>
                    <input type="text" class="form-control" id="roomSearch" placeholder="Search for a room" onkeyup="filterRooms()">
                </div>
                <div class="col-md-8">
                    <label for="buildingSelect" class="form-label">Select Building:</label>
                    <select class="form-select" id="buildingSelect" onchange="filterRooms()">
                        <option value="Rizal">Rizal Building</option>
                        <option value="Einstein">Einstein Building</option>
                        <option value="ETYCB">ETYCB Building</option>
                    </select>
                </div>
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
            </div>
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