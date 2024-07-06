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
                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room1">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R101</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div> 
                </div>

               <%-- <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room2">
                    <div class="card text-white bg-danger">
                        <div class="card-body">
                            <h5 class="card-title">R102</h5>
                            <p class="card-text">Not Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room3">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R103</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room4">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R104</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room5">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R105</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room6">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R106</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room7">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R107</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room8">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R108</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room9">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R109</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room10">
                    <div class="card text-white bg-danger">
                        <div class="card-body">
                            <h5 class="card-title">R110</h5>
                            <p class="card-text">Not Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room11">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">R111</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Rizal" id="room112">
                    <div class="card text-white bg-danger">
                        <div class="card-body">
                            <h5 class="card-title">R112</h5>
                            <p class="card-text">Not Available</p>
                        </div>
                    </div>
                </div>--%>


                <!-- Repeat for remaining rooms in Rizal Building -->

                <!-- Einstein Building Rooms -->
                <div class="col-md-3 mb-3 room-card" data-building="Einstein" id="room201">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">E201</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="Einstein" id="room202">
                    <div class="card text-white bg-danger">
                        <div class="card-body">
                            <h5 class="card-title">E202</h5>
                            <p class="card-text">Not Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="Einstein" id="room203">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">E203</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="Einstein" id="room204">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">E204</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>


                <!-- Repeat for remaining rooms in Einstein Building -->

                <!-- ETYCB Building Rooms -->
                <div class="col-md-3 mb-3 room-card" data-building="ETYCB" id="room301">
                    <div class="card text-white bg-danger">
                        <div class="card-body">
                            <h5 class="card-title">ETY301</h5>
                            <p class="card-text">Not Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="ETYCB" id="room22">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">ETY302</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 mb-3 room-card" data-building="ETYCB" id="room23">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">ETY303</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 mb-3 room-card" data-building="ETYCB" id="room24">
                    <div class="card text-white bg-success">
                        <div class="card-body">
                            <h5 class="card-title">ETY304</h5>
                            <p class="card-text">Available</p>
                        </div>
                    </div>
                </div>

                <!-- Repeat for remaining rooms in ETYCB Building -->
            </div>
        <!-- Add more rooms as needed -->
   


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
