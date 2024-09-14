using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Booking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ModalPopup.RegisterModalHtml(this.Page);



                //building dropdown in adding room modal
                BindBuildings();

                

                BindScheduleData(sender, e);
            }
        }

        protected void BindScheduleData(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    //connection.Open();
                    string query = @"
                    SELECT
                        *
                    FROM RoomRequest RR
                    WHERE RR.status = 'Accepted'";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                    else
                    {
                        // No data found, display a message or handle accordingly
                        GridView1.EmptyDataText = "No Pending Request";
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the error
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        public bool RoomExists(SqlConnection connection, string room)
        {
            string query = "SELECT COUNT(1) FROM Rooms WHERE RoomName = @RoomName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomName", room);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        protected void btnConfirmAddRoom_Click(object sender, EventArgs e)
        {
            if (ddlBuildings.SelectedIndex > 0)
            {
                // Proceed with adding the room
                string roomName = txtRoomName.Text;
                int buildingId = int.Parse(ddlBuildings.SelectedValue);

                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    try
                    {
                        get_ID addROOM = new get_ID();
                        int RoomID = addROOM.GetOrInsertRoom(connection, roomName, buildingId, true);
                        if (RoomID > 0)
                        {
                            lblRoom.Text = " Room inserted successfully.";
                            lblRoom.CssClass = "alert alert-success";
                            lblRoom.Visible = true;

                            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "setTimeout(function() { $('#addRoomModal').modal('hide'); }, 2000);", true);

                            
                            string msgtxtbox = "You can now deploy request with room: " + roomName;
                            ModalPopup.ShowMessage_(this.Page, msgtxtbox, "Note!");
                        }
                        else
                        {
                            lblRoom.Text = "Failed to insert Room.";
                            lblRoom.CssClass = "alert alert-danger";
                            lblRoom.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblRoom.Text = "Error: " + ex.Message;
                        lblRoom.CssClass = "alert alert-danger";
                        lblRoom.Visible = true;
                    }
                }
            }
            else
            {
                // Optionally, show an error message if no building is selected
                lblRoom.Text = "Please select a building.";
                lblRoom.CssClass = "alert alert-danger";
                lblRoom.Visible = true;
            }
        }

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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deploy" || e.CommandName == "Reject")
            {
                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();

                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = GridView1.Rows[rowIndex];
                int requestId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);

                if (e.CommandName == "Deploy")
                {
                    //Access RoomNumber from TemplateField
                    Label lblRoomName = (Label)row.FindControl("lblRoomName");
                    string Room = lblRoomName != null ? lblRoomName.Text.ToUpper() : string.Empty;

                    // Check room existence
                    bool roomExists = RoomExists(connection, Room);

                    if (!roomExists)
                    {
                        txtRoomName.Text = Room;

                        string script = "showConfirmAddModal();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", script, true);
                    }
                    else
                    {
                        // Extract data from the row controls
                        string email = row.Cells[0].Text;
                        string course = row.Cells[1].Text;
                        string section = row.Cells[2].Text;
                        string Instructor = row.Cells[3].Text;
                        //string Faculty = row.Cells[4].Text;
                        string Building = row.Cells[5].Text;
                        //string Room = row.Cells[6].Text.ToUpper();
                        string StartDate = row.Cells[7].Text;
                        string EndDate = row.Cells[8].Text;
                        string startTime = row.Cells[9].Text;
                        string endTime = row.Cells[10].Text;
                        //string PurposeoftheRoom = row.Cells[11].Text;

                        get_ID getID = new get_ID();

                        string insertQuery = @"
                        INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, DayID, StartTime, EndTime, StartDate, EndDate, Remarks, BuildingID)
                        VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @DayID, @StartTime, @EndTime, @StartDate, @EndDate, @Remarks, @BuildingID)";

                        try
                        {
                            if (connection.State == System.Data.ConnectionState.Open)
                            {
                                // Convert necessary input values to appropriate data types
                                int courseID = getID.GetOrInsertCourse(connection, course);
                                int sectionID = getID.GetOrInsertSection(connection, section);
                                int instructorID = getID.GetOrInsertInstructor(connection, Instructor);
                                int buildingID = getID.GetOrInsertBuilding(connection, Building);
                                int roomID = getID.GetOrInsertRoom(connection, Room, buildingID);

                                DateTime startDt = DateTime.Parse(StartDate);
                                DateTime endDt = DateTime.Parse(EndDate);
                                TimeSpan startTm = TimeSpan.Parse(startTime);
                                TimeSpan endTm = TimeSpan.Parse(endTime);

                                // Loop through each date between startDate and endDate
                                for (DateTime date = startDt; date <= endDt; date = date.AddDays(1))
                                {
                                    // Get the day of the week and corresponding DayID
                                    string dayName = date.DayOfWeek.ToString().ToUpper();
                                    int dayID = getID.GetDayID(dayName);

                                    string remarks = "This Schedule is Requested by email: " + email;
                                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                                    {
                                        // Add parameters to the command
                                        cmd.Parameters.AddWithValue("@RoomID", roomID);
                                        cmd.Parameters.AddWithValue("@SectionID", sectionID);
                                        cmd.Parameters.AddWithValue("@CourseID", courseID);
                                        cmd.Parameters.AddWithValue("@InstructorID", instructorID);
                                        cmd.Parameters.AddWithValue("@DayID", dayID);
                                        cmd.Parameters.AddWithValue("@StartTime", startTm);
                                        cmd.Parameters.AddWithValue("@EndTime", endTm);
                                        cmd.Parameters.AddWithValue("@StartDate", startDt);
                                        cmd.Parameters.AddWithValue("@EndDate", endDt);
                                        cmd.Parameters.AddWithValue("@Remarks", remarks);
                                        cmd.Parameters.AddWithValue("@BuildingID", buildingID);

                                        // Execute the insert command
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                //this will send email:
                                string newStatus = e.CommandName == "Deploy" ? "Deployed" : "Rejected";
                                UpdateRoomRequestStatus(requestId, newStatus);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle exception (you can log the error or display a message)
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
                        }
                        finally
                        {
                            //GridView1.DataBind();
                            BindScheduleData(sender, e);
                        }
                    }
                };
            }
        }

        protected string CheckScheduleConflict(IDataItemContainer container)
        {
            var row = (GridViewRow)container;

            // Extract room ID and other values
            string RoomNumber = (string)DataBinder.Eval(row.DataItem, "Room");
            DateTime startDate = Convert.ToDateTime(DataBinder.Eval(row.DataItem, "StartDate"));
            DateTime endDate = Convert.ToDateTime(DataBinder.Eval(row.DataItem, "EndDate"));
            TimeSpan startTime = (TimeSpan)DataBinder.Eval(row.DataItem, "startTime");
            TimeSpan endTime = (TimeSpan)DataBinder.Eval(row.DataItem, "endTime");

            // Check for conflicts in the database
            bool hasConflict = CheckForConflicts(RoomNumber, startDate, endDate, startTime, endTime);

            return hasConflict ? "display:inline;" : "display:none;";
        }

        private bool CheckForConflicts(string RoomNumber, DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
        {
            // Your database logic to check for conflicts
            // Open database connection
            bool hasConflict = false;

            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                get_ID getRoom = new get_ID();
                int roomID = getRoom.GetOrInsertRoom(connection, RoomNumber, -1);

                // Query to check for conflicts
                string query = @"
                    SELECT COUNT(*)
                    FROM Schedule
                    WHERE RoomID = @RoomID
                      AND (
                        (StartDate <= @EndDate AND EndDate >= @StartDate)
                        AND
                        (StartTime < @EndTime AND EndTime > @StartTime)
                      )";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomID", roomID);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.Parameters.AddWithValue("@EndTime", endTime);

                    int count = (int)command.ExecuteScalar();
                    hasConflict = (count > 0);
                }
            }
            // Return true if conflict exists, otherwise false
            return hasConflict;
        }

        private void UpdateRoomRequestStatus(int requestId, string status)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string sql = "UPDATE RoomRequest SET status = @status WHERE RequestID = @requestId";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@requestId", requestId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}