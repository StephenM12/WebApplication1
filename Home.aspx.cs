using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userFname = user_Identity.user_FName;
            Label1.Text = "Welcome " + userFname;

            if (!IsPostBack)
            {
                //BindRooms();
                //BuildingDown_MAIN(sender, e);
                //BuildingDown_Data(sender, e);
                BindBuildings();

                int buildingId = GetSelectedBuildingId(); // Implement this method to get selected building ID
                //RoomRepository repo = new RoomRepository();
                //List<Room> rooms = repo.GetRooms(buildingId);
                List<Room> rooms = GetRooms(3);

                RoomRepeater.DataSource = rooms;
                RoomRepeater.DataBind();
            }
        }

        //public class Room
        //{
        //    public string Building { get; set; }
        //    public string RoomID { get; set; }
        //    public string RoomNumber { get; set; }
        //    public string Status { get; set; }
        //    public string CardClass { get; set; }
        //}

        public class Room
        {
            public int RoomID { get; set; }
            public string RoomName { get; set; }
        }

        private int GetSelectedBuildingId()
        {
            // Implement logic to get the selected building ID (e.g., from query string or session)
            return 1; // Placeholder value
        }

        public List<Room> GetRooms(int buildingId)
        {
            var rooms = new List<Room>();

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                var command = new SqlCommand("SELECT RoomID, RoomName FROM Rooms WHERE BuildingID = @BuildingID", connection);
                command.Parameters.AddWithValue("@BuildingID", buildingId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            RoomID = reader.GetInt32(0),
                            RoomName = reader.GetString(1)
                        });
                    }
                }
            }

            return rooms;
        }

        //adding building
        protected void btnAddBuilding_Click(object sender, EventArgs e)
        {
            string buildingName = txtBuildingName.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(buildingName))
            {
                lblSuccessMessage.Text = "Building name cannot be empty!";
                lblSuccessMessage.CssClass = "alert alert-danger";
                lblSuccessMessage.Visible = true;
                return;
            }

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                get_ID getbuildID = new get_ID();
                try
                {
                    int buildingID = getbuildID.GetOrInsertBuilding(connection, buildingName);

                    if (buildingID > 0)
                    {
                        txtBuildingName.Text = " ";
                        lblSuccessMessage.Text = buildingName + " Building inserted successfully.";
                        lblSuccessMessage.CssClass = "alert alert-success";
                        lblSuccessMessage.Visible = true;

                        // Use ScriptManager to close the modal after 2 seconds
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                            setTimeout(function() {
                                $('#addBuildingModal').modal('hide');
                            }, 3000);
                        ", true);
                    }
                    else
                    {
                        lblSuccessMessage.Text = "Failed to insert building.";
                        lblSuccessMessage.CssClass = "alert alert-danger";
                        lblSuccessMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblSuccessMessage.Text = "Error: " + ex.Message;
                    lblSuccessMessage.CssClass = "alert alert-danger";
                    lblSuccessMessage.Visible = true;
                }
            }
        }


        //for building dropdown in adding room
        private void BindBuildings()
        {
            string query = "SELECT BuildingID, BuildingName FROM Buildings";

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    ddlBuildings.DataSource = cmd.ExecuteReader();
                    ddlBuildings.DataTextField = "BuildingName";
                    ddlBuildings.DataValueField = "BuildingID";
                    ddlBuildings.DataBind();
                }
            }

            ddlBuildings.Items.Insert(0, new ListItem("Select Building", "0"));
        }

        protected void btnConfirmAddRoom_Click(object sender, EventArgs e)
        {
            string roomName = txtRoomName.Text.Trim().ToUpper();
            int buildingId = int.Parse(ddlBuildings.SelectedValue);

            if (string.IsNullOrEmpty(roomName))
            {
                lblRoomError.Text = "Room name cannot be empty!";
                return;
            }

            if (buildingId == 0)
            {
                lblRoomError.Text = "Please select a building!";
                return;
            }

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            string query = "INSERT INTO Rooms (RoomName, BuildingID) VALUES (@RoomName, @BuildingID)";

            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomName", roomName);
                    cmd.Parameters.AddWithValue("@BuildingID", buildingId);

                    try
                    {
                        cmd.ExecuteNonQuery();

                        lblRoomError.Text = ""; // Clear any previous error
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#addRoomModal').modal('hide');", true);
                    }
                    catch (Exception ex)
                    {
                        lblRoomError.Text = "Error: " + ex.Message;
                    }
                }
            }
        }

        //protected void BuildingDown_Data(object sender, EventArgs e)
        //{
        //    // Open database connection
        //    SqlConnection connection = dbConnection.GetConnection();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        //Dropdown datas from sql
        //        SqlCommand cmd = new SqlCommand("SELECT BuildingID, BuildingName FROM Buildings", connection);
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        // Bind the data to the dropdown list
        //        BuildingID.DataTextField = "BuildingName"; // Column name to display
        //        BuildingID.DataValueField = "BuildingID"; // Column name to use as value
        //        BuildingID.DataSource = reader;
        //        BuildingID.DataBind();
        //    }
        //}

        //protected void BuildingDown_MAIN(object sender, EventArgs e)
        //{
        //    // Open database connection
        //    SqlConnection connection = dbConnection.GetConnection();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        //Dropdown datas from sql
        //        SqlCommand cmd = new SqlCommand("SELECT BuildingID, BuildingName FROM Buildings", connection);
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        // Bind the data to the dropdown list
        //        buildingSelect.DataTextField = "BuildingName"; // Column name to display
        //        buildingSelect.DataValueField = "BuildingID"; // Column name to use as value
        //        buildingSelect.DataSource = reader;
        //        buildingSelect.DataBind();
        //    }
        //}

        //private void BindRooms()
        //{
        //    string query = @"
        //                SELECT
        //                    r.RoomID,
        //                    r.RoomName,
        //                    r.BuildingID,
        //                    b.BuildingName
        //                FROM
        //                    Rooms r
        //                JOIN
        //                    Buildings b ON r.BuildingID = b.BuildingID";

        //    List<Room> rooms = new List<Room>();

        //    // Open database connection
        //    SqlConnection connection = dbConnection.GetConnection();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, connection))
        //        {
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string cardClass_ = "bg-success";
        //                    string status = "Available";
        //                    //string Building = "RIZAL";

        //                    rooms.Add(new Room
        //                    {
        //                        Building = reader["BuildingName"].ToString(),
        //                        RoomID = reader["RoomID"].ToString(),
        //                        RoomNumber = reader["RoomName"].ToString(),
        //                        Status = status,
        //                        CardClass = cardClass_
        //                    });
        //                }
        //            }
        //        }

        //        RepeaterRooms.DataSource = rooms;
        //        RepeaterRooms.DataBind();
        //    }
        //}

        //protected void addRoomBTN_Click(object sender, EventArgs e)
        //{
        //    string roomtxt = txtNewroom.Text.ToUpper();
        //    string BuildingID_ = ADD_BuildDL.SelectedValue;

        //    // Open database connection
        //    SqlConnection connection = dbConnection.GetConnection();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        get_ID getBuilding = new get_ID();
        //        int buildID = getBuilding.GetOrInsertBuilding(connection, BuildingID_);

        //        string insertQuery = "INSERT INTO Rooms (RoomName, BuildingID) VALUES (@RoomName, @BuildingID)";
        //        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@RoomName", roomtxt);
        //            command.Parameters.AddWithValue("@BuildingID", buildID);
        //            int rowsAffected = command.ExecuteNonQuery();

        //            if (rowsAffected > 0)
        //            {
        //                // Insert successful, show success message
        //                BindRooms();
        //                txtNewroom.Text = string.Empty;
        //                Response.Write("Room insert successful");
        //            }
        //            else
        //            {
        //                Response.Write("Adding Room Failed");
        //            }
        //        }
        //    }
        //}

        //protected void addBuildBTN_Click(object sender, EventArgs e)
        //{
        //    string buildingTXT = Building_txtbox.Text;

        //    // Open database connection
        //    SqlConnection connection = dbConnection.GetConnection();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        get_ID add_dbHelper = new get_ID();
        //        int buildingID_ = add_dbHelper.GetOrInsertBuilding(connection, buildingTXT);
        //        if (buildingID_ != -1)
        //        {
        //            BuildingDown_Data(sender, e);
        //            Response.Write("Building insert successful");
        //        }
        //    }
        //}
    }
}