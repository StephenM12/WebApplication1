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
                ModalPopup.RegisterModalHtml(this.Page);

                int userlevel = user_Identity.user_level;
                if (userlevel != 1)
                {
                    openModalLabel.Visible = false;
                    //addBuild.Visible = false;
                    //addRm.Visible = false;
                }

                dropdown_datas(sender, e);

                showRoom(sender, e);
            }
        }

        //for room showing/ upon dropdown select
        public class Room
        {
            public int RoomID { get; set; }
            public string RoomName { get; set; }
            public string Availability { get; set; }
        }

        protected void showRoom(object sender, EventArgs e)
        {
            int buildingId;

            if (!string.IsNullOrEmpty(DropDownList1.SelectedValue) && DropDownList1.SelectedValue != "0")
            {
                buildingId = int.Parse(DropDownList1.SelectedValue);
            }
            else
            {
                buildingId = 0;
            }

            //show room
            List<Room> rooms = GetRooms(buildingId);
            RoomRepeater.DataSource = rooms;
            RoomRepeater.DataBind();
        }



        
        public List<Room> GetRooms(int buildingId)
        {
            List<Room> rooms = new List<Room>();
            DateTime currentTime = DateTime.Now;
            //string currentTimeString = currentTime.ToString("h:mmtt");
            //TimeSpan currentTimeSpan = currentTime.TimeOfDay;

            TimeZoneInfo philippineTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"); // UTC+8
            DateTime currentTime_ = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, philippineTimeZone);
            TimeSpan currentTimeSpan = currentTime_.TimeOfDay;


            using (SqlConnection connection = dbConnection.GetConnection())
            {

                string query = @"
                    SELECT r.RoomID, r.RoomName, 
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM Schedule s 
                            WHERE s.RoomID = r.RoomID 
                            
                            AND s.ScheduleDate = CAST(GETDATE() AS DATE) 
                            AND @CurrentTime BETWEEN s.StartTime AND s.EndTime
                        ) 
                        THEN 'Occupied' 
                        ELSE 'Available' 
                    END AS Availability 
                    FROM Rooms r 
                    WHERE (r.BuildingID = @BuildingID OR @BuildingID = 0 )";

                //AND s.BuildingID = @BuildingID 
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BuildingID", buildingId);
                    command.Parameters.AddWithValue("@CurrentTime", currentTimeSpan); 

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Room room = new Room
                            {
                                RoomID = reader.GetInt32(0),
                                RoomName = reader.GetString(1),
                                Availability = reader.GetString(2) // Availability based on current time


                            };
                            rooms.Add(room);

                        }
                    }
                }
            }

            return rooms;
        }


        //orig
        //public List<Room> GetRooms(int buildingId)
        //{
        //    var rooms = new List<Room>();

        //    // Open database connection
        //    using (SqlConnection connection = dbConnection.GetConnection())
        //    {
        //        if (connection.State == System.Data.ConnectionState.Open)
        //        {
        //            // Determine the query based on buildingId
        //            string query = "SELECT RoomID, RoomName FROM Rooms";
        //            if (buildingId != 0)
        //            {
        //                query += " WHERE BuildingID = @BuildingID";
        //            }

        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                if (buildingId != 0)
        //                {
        //                    command.Parameters.AddWithValue("@BuildingID", buildingId);
        //                }

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        rooms.Add(new Room
        //                        {
        //                            RoomID = reader.GetInt32(0),
        //                            RoomName = reader.GetString(1)
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return rooms;
        //}



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

            using (SqlConnection connection = dbConnection.GetConnection())
            {

                try
                {
                    get_ID getbuildID = new get_ID();
                    int buildingID = getbuildID.GetOrInsertBuilding(connection, buildingName, true);

                    if (buildingID > 0)
                    {
                        txtBuildingName.Text = " ";
                        lblSuccessMessage.Text = buildingName + " Building inserted successfully.";
                        lblSuccessMessage.CssClass = "alert alert-success";
                        lblSuccessMessage.Visible = true;

                        

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                            setTimeout(function() {
                                $('#addBuildingModal').modal('hide');
                            }, 2000);

                        ", true);
                    }
                    else
                    {
                        lblSuccessMessage.Text = "Failed to insert building.";
                        lblSuccessMessage.CssClass = "alert alert-danger";
                        lblSuccessMessage.Visible = true;
                    }

                    dropdown_datas(sender, e);
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

            string query = "INSERT INTO Rooms (RoomName, BuildingID) VALUES (@RoomName, @BuildingID)";

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomName", roomName);
                    cmd.Parameters.AddWithValue("@BuildingID", buildingId);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            //in showing modal, make this the basis on how to show also refer from the trigger in the button

                            ddlBuildings.SelectedIndex = 0;

                            lblRoomError.Text = roomName + " Room Added Succesfully.";
                            lblRoomError.CssClass = "alert alert-success";
                            lblRoomError.Visible = true;

                            showRoom(sender, e);

                            // Use ScriptManager to close the modal after 2 seconds
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                                setTimeout(function() {
                                    $('#addRoomModal').modal('hide');
                                }, 2000);
                            ", true);

                            txtRoomName.Text = " ";



                            //string msgtxtbox = "Room Added Succesfully";
                            //ModalPopup.ShowMessage_(this.Page, msgtxtbox, "Note!");
                        }
                    }
                    catch (Exception ex)
                    {
                        //lblRoomError.Text = "Error: " + ex.Message;
                        string msg = "Error: " + ex.Message;
                        ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
                    }
                }
            }
        }

        //adding faculty
        protected void btnAddFaculty_Click(object sender, EventArgs e)
        {
            string facultyCode = txtNewFacultyName.Text.Trim().ToUpper(); // Get the faculty code from the TextBox

            // Check if the faculty code is not empty
            if (string.IsNullOrEmpty(facultyCode))
            {
                // Optionally show a message to the user
                Label1facult.Text = "Faculty code cannot be empty.";
                return;
            }

            using (SqlConnection connection = dbConnection.GetConnection())
            {

                try
                {
                    //conn.Open();
                    string query = "INSERT INTO Faculty (FacultyCode) VALUES (@FacultyCode)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Use parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@FacultyCode", facultyCode);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the insertion was successful
                        if (rowsAffected > 0)
                        {

                            Label1facult.Text = facultyCode + " added successfully.";
                            Label1facult.CssClass = "alert alert-success";
                            Label1facult.Visible = true;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                            setTimeout(function() {
                                $('#addFacultyModal').modal('hide');
                            }, 2000);

                            ", true);


                            txtNewFacultyName.Text = string.Empty;
                        }
                        else
                        {
                            Label1facult.Text = "Error adding faculty.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (if needed) and display an error message
                    Label1facult.Text = "An error occurred: " + ex.Message;
                }
            }
        }

        protected void BindSelectedBuild(object sender, EventArgs e)
        {
            showRoom(sender, e);
        }

        protected void dropdown_datas(object sender, EventArgs e)
        {
            DropdownFiller filler = new DropdownFiller();

            filler.PopulateBuildings(DropDownList1);
            filler.PopulateBuildings(ddlBuildings);

            //for add modal
            filler.PopulateBuildings(addBuild_DropDownList);
            filler.PopulateRooms(addRoom_DropDownList);
            filler.PopulateFaculty(addFaculty_DropDownList);

            DropDownList1.Items.Insert(0, new ListItem("Select Building", "0"));
            ddlBuildings.Items.Insert(0, new ListItem("Select Building", "0"));
        }
    }
}