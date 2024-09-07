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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deploy" || e.CommandName == "Reject")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = GridView1.Rows[rowIndex];
                int requestId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);

                if (e.CommandName == "Deploy")
                {
                    // Extract data from the row controls
                    string email = row.Cells[0].Text;
                    string course = row.Cells[1].Text;
                    string section = row.Cells[2].Text;
                    string Instructor = row.Cells[3].Text;
                    //string Faculty = row.Cells[4].Text;
                    string Building = row.Cells[5].Text;
                    string Room = row.Cells[6].Text;
                    string StartDate = row.Cells[7].Text;
                    string EndDate = row.Cells[8].Text;
                    string startTime = row.Cells[9].Text;
                    string endTime = row.Cells[10].Text;
                    //string PurposeoftheRoom = row.Cells[11].Text;

                    get_ID getID = new get_ID();

                    string insertQuery = @"
                    INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, StartTime, EndTime, StartDate, EndDate, Remarks, BuildingID)
                    VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @StartTime, @EndTime, @StartDate, @EndDate, @Remarks, @BuildingID)";

                    // Open database connection
                    SqlConnection connection = dbConnection.GetConnection();

                    try
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            // Convert necessary input values to appropriate data types
                            int courseID = getID.GetOrInsertCourse(connection, course);
                            int sectionID = getID.GetOrInsertSection(connection, section);
                            int instructorID = getID.GetOrInsertInstructor(connection, Instructor);
                            int buildingID = getID.GetOrInsertBuilding(connection, Building);
                            int roomID = getID.GetOrInsertRoom(connection, Room, true, buildingID);

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
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (you can log the error or display a message)
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
                    }
                    finally
                    {
                        // Ensure the connection is closed
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }

                        //this will send email:
                        string newStatus = e.CommandName == "Deploy" ? "Deployed" : "Rejected";
                        UpdateRoomRequestStatus(requestId, newStatus);

                        //msg to output after successfu insert
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Deployment successful!');", true);

                        //GridView1.DataBind();
                        BindScheduleData(sender, e);
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
                int roomID = getRoom.GetOrInsertRoom(connection, RoomNumber, false, -1);

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