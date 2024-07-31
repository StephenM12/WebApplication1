using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

public class DropdownFiller
{
   

    // Method to populate rooms dropdown
    public void PopulateRooms(DropDownList dropdown)
    {
        List<Room> rooms = GetRooms();

        dropdown.DataSource = rooms;
        dropdown.DataTextField = "RoomName";
        dropdown.DataValueField = "RoomID";
        dropdown.DataBind();
    }

    // Method to populate buildings dropdown
    public void PopulateBuildings(DropDownList dropdown)
    {
        List<Building> buildings = GetBuildings();

        dropdown.DataSource = buildings;
        dropdown.DataTextField = "BuildingName";
        dropdown.DataValueField = "BuildingID";
        dropdown.DataBind();
    }

    // Method to get rooms from the database
    private List<Room> GetRooms()
    {
        List<Room> rooms = new List<Room>();

        // Open database connection
        SqlConnection connection = dbConnection.GetConnection();
        if (connection.State == System.Data.ConnectionState.Open)
        {
            
            string query = "SELECT RoomID, RoomName FROM Rooms";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                rooms.Add(new Room
                {
                    RoomID = Convert.ToInt32(reader["RoomID"]),
                    RoomName = reader["RoomName"].ToString()
                });
            }
            reader.Close();
        }

        return rooms;
    }

    // Method to get buildings from the database
    private List<Building> GetBuildings()
    {
        List<Building> buildings = new List<Building>();

        // Open database connection
        SqlConnection connection = dbConnection.GetConnection();
        if (connection.State == System.Data.ConnectionState.Open)
        {
            string query = "SELECT BuildingID, BuildingName FROM Buildings";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                buildings.Add(new Building
                {
                    BuildingID = Convert.ToInt32(reader["BuildingID"]),
                    BuildingName = reader["BuildingName"].ToString()
                });
            }
            reader.Close();
        }

        return buildings;
    }
}

// Example classes for Room and Building
public class Room
{
    public int RoomID { get; set; }
    public string RoomName { get; set; }
}

public class Building
{
    public int BuildingID { get; set; }
    public string BuildingName { get; set; }
}
