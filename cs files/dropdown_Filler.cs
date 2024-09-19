using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

public class DropdownFiller
{

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
    public class uploadScheds
    {
        public int UploadID { get; set; }
        public string FileName { get; set; }
    }

    public void PopulateRooms(DropDownList dropdown)
    {
        List<Room> rooms = GetRooms();

        dropdown.DataSource = rooms;
        dropdown.DataTextField = "RoomName";
        dropdown.DataValueField = "RoomID";
        dropdown.DataBind();
    }

    public void PopulateBuildings(DropDownList dropdown)
    {
        List<Building> buildings = GetBuildings();

        dropdown.DataSource = buildings;
        dropdown.DataTextField = "BuildingName";
        dropdown.DataValueField = "BuildingID";
        dropdown.DataBind();
    }

    public void PopulateSchedule(DropDownList dropdown)
    {
        List<uploadScheds> uploadedscheds = getUploadscheds();

        dropdown.DataSource = uploadedscheds;
        dropdown.DataTextField = "FileName";
        dropdown.DataValueField = "UploadID";
        dropdown.DataBind();
    }

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
            connection.Close();
        }

        return rooms;
    }

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

            connection.Close();
        }

        return buildings;
    }

    private List<uploadScheds> getUploadscheds()
    {
        List<uploadScheds> uploadedscheds = new List<uploadScheds>();

        // Open database connection
        SqlConnection connection = dbConnection.GetConnection();
        if (connection.State == System.Data.ConnectionState.Open)
        {
            string query = "SELECT UploadID, FileName FROM upload_SchedsTBL";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                uploadedscheds.Add(new uploadScheds
                {
                    UploadID = Convert.ToInt32(reader["UploadID"]),
                    FileName = reader["FileName"].ToString()
                });
            }
            reader.Close();
            connection.Close();
        }

        return uploadedscheds;
    }
}


