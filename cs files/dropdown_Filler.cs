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
    public class faculty
    {
        public int FacultyID { get; set; }
        public string FacultyCode { get; set; }
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
    public void PopulateFaculty(DropDownList dropdown)
    {
        List<faculty> faculty_ = getFaculty();

        dropdown.DataSource = faculty_;
        dropdown.DataTextField = "FacultyCode";
        dropdown.DataValueField = "FacultyID";
        dropdown.DataBind();
    }

    private List<Room> GetRooms()
    {
        List<Room> rooms = new List<Room>();

        using (SqlConnection connection = dbConnection.GetConnection())
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
           
        }

        return rooms;
    }

    private List<Building> GetBuildings()
    {
        List<Building> buildings = new List<Building>();

        using (SqlConnection connection = dbConnection.GetConnection())
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
           
        }

        return buildings;
    }

    private List<uploadScheds> getUploadscheds()
    {
        List<uploadScheds> uploadedscheds = new List<uploadScheds>();

        using (SqlConnection connection = dbConnection.GetConnection())
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
            
        }

        return uploadedscheds;
    }
    private List<faculty> getFaculty()
    {
        List<faculty> faculty_ = new List<faculty>();

        using (SqlConnection connection = dbConnection.GetConnection())
        {
            string query = "SELECT FacultyID, FacultyCode FROM Faculty";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                faculty_.Add(new faculty
                {
                    FacultyID = Convert.ToInt32(reader["FacultyID"]),
                    FacultyCode = reader["FacultyCode"].ToString()
                });
            }

        }

        return faculty_;
    }
}


