using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
                int userlevel = user_Identity.user_level;
                if (userlevel != 1 )
                {
                    addBuild.Visible = false;
                    addRm.Visible = false;

                }

                dropdown_datas(sender, e);

                showRoom(sender, e);

            }
        }

        public class Room
        {
            public int RoomID { get; set; }
            public string RoomName { get; set; }
        }
        protected void dropdown_datas(object sender, EventArgs e)
        {
            DropdownFiller filler = new DropdownFiller();
            filler.PopulateBuildings(DropDownList1);

        }

        protected void BindSelectedBuild(object sender, EventArgs e)
        {
            //fetch building data to dropdown
            //int buildingId = int.Parse(DropDownList1.SelectedValue);
            showRoom(sender, e);
            


        }
        protected void showRoom(object sender, EventArgs e)
        {
            int buildingId = int.Parse(DropDownList1.SelectedValue);


            //show room
            List<Room> rooms = GetRooms(buildingId);
            RoomRepeater.DataSource = rooms;
            RoomRepeater.DataBind();


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

                        dropdown_datas(sender, e);

                        // Use ScriptManager to close the modal after 2 seconds
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                            setTimeout(function() {
                                $('#addBuildingModal').modal('hide');
                            }, 3000);
                        ", true);

                        lblSuccessMessage.Text = " ";
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

        //adding room
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

        
    }
}