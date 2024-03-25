
    function showSchedule(building) {
            // Dummy data, you need to replace this with your actual data retrieval logic
            var scheduleData = {
        "Rizal Building": [
    {room: "Room 101", time: "9:00 AM - 10:00 AM" },
    {room: "Room 102", time: "10:00 AM - 11:00 AM" },
    // Add more schedule data for Rizal Building if needed
    ],
    "Einstein Building": [
    {room: "Room A", time: "9:00 AM - 10:00 AM" },
    {room: "Room B", time: "10:00 AM - 11:00 AM" },
    // Add more schedule data for Einstein Building if needed
    ],
    "ETYCB Building": [
    {room: "Room X", time: "9:00 AM - 10:00 AM" },
    {room: "Room Y", time: "10:00 AM - 11:00 AM" },
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