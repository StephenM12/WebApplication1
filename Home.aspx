<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link rel="stylesheet" href="./CSS/Home_Style.css" />

    <div class="container mx-auto">
        <h1>

            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

        </h1> 

        <!-- Add the dropdown and GridView HTML code here -->
        <div class="dropdown">
            <button class="dropbtn">Select Building</button>
            <div class="dropdown-content">
                <a href="#" onclick="showSchedule('Rizal Building')">Rizal Building</a>
                <a href="#" onclick="showSchedule('Einstein Building')">Einstein Building</a>
                <a href="#" onclick="showSchedule('ETYCB Building')">ETYCB Building</a>
            </div>
        </div>

        <div class="table-responsive" id="scheduleTable">
            <!-- GridView will be populated here based on selection -->
        </div>
    </div>

    <script>
        function showSchedule(building) {
            // Dummy data, you need to replace this with your actual data retrieval logic
            var scheduleData = {
                "Rizal Building": [
                    { room: "Room 101", time: "9:00 AM - 10:00 AM" },
                    { room: "Room 102", time: "10:00 AM - 11:00 AM" },
                    // Add more schedule data for Rizal Building if needed
                ],
                "Einstein Building": [
                    { room: "Room A", time: "9:00 AM - 10:00 AM" },
                    { room: "Room B", time: "10:00 AM - 11:00 AM" },
                    // Add more schedule data for Einstein Building if needed
                ],
                "ETYCB Building": [
                    { room: "Room X", time: "9:00 AM - 10:00 AM" },
                    { room: "Room Y", time: "10:00 AM - 11:00 AM" },
                    // Add more schedule data for ETYCB Building if needed
                ]
            };

            var scheduleHtml = "<table>";
            scheduleHtml += "<tr><th>Room</th><th>Time</th></tr>";

            // Generate HTML for the selected building's schedule
            scheduleData[building].forEach(function (item) {
                scheduleHtml += "<tr><td>" + item.room + "</td><td>" + item.time + "</td></tr>";
            });

            scheduleHtml += "</table>";

            // Display the schedule in the designated div
            document.getElementById("scheduleTable").innerHTML = scheduleHtml;
        }
    </script>

</asp:Content>