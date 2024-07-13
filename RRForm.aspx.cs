using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using WebApplication1.cs_files;

namespace RoomRequestForm
{
    public partial class RRForm : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Page load logic if any
        }
        protected void Btn_Back(object sender, EventArgs e) 
        {
            Response.Redirect("Login.aspx");


        }
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            // Retrieve data from form controls
            string email = this.email.Text;
            string courseCode = this.RCourseCodeTB.Text;
            string section = this.RSectionTB.Text;
            string instructor = this.RProfTB.Text;
            string faculty = this.RFacultyDL.SelectedValue;
            string building = this.SelectBuildingDL.SelectedValue;
            string roomNumber = this.RRoomNumberTB.Text;
            DateTime startDate = DateTime.Parse(this.SelectDateTB.Text);
            DateTime endDate = DateTime.Parse(this.EndDateTB.Text);




            //for time

            string selectedTimeRange = this.RTimeDL.SelectedValue;
            string[] times = selectedTimeRange.Split('-');
            if (times.Length == 2)
            {
                string startTimeStr = times[0].Trim();
                string endTimeStr = times[1].Trim();

                // Parse start time
                DateTime parsedStartTime = DateTime.ParseExact(startTimeStr, "h:mm tt", CultureInfo.InvariantCulture);
                TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

                // Parse end time
                DateTime parsedEndTime = DateTime.ParseExact(endTimeStr, "h:mm tt", CultureInfo.InvariantCulture);
                TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

                string insertQuery = @"INSERT INTO RoomRequest 
                (email, CourseID, SectionID, InstructorID, Faculty, BuildingID, RoomID, StartDate, EndDate, startTime, endTime, status)
                VALUES (@Email, @CourseID, @SectionID, @InstructorID, @Faculty, @BuildingID, @RoomID, @StartDate, @EndDate, @StartTime, @EndTime, @Status)";

                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    get_ID add_dbHelper = new get_ID();
                    var add_Ids = add_dbHelper.CheckAndInsertValues(connection, roomNumber, section, courseCode, instructor);

                    int roomID = add_Ids.roomID;
                    int sectionID = add_Ids.sectionID;
                    int courseID = add_Ids.courseID;
                    int instructorID = add_Ids.instructorID;

                    //get_ID building id
                    var buildingID = add_dbHelper.GetOrInsertBuilding(connection, building);

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@CourseID", courseID);
                        command.Parameters.AddWithValue("@SectionID", sectionID);
                        command.Parameters.AddWithValue("@InstructorID", instructorID);
                        command.Parameters.AddWithValue("@Faculty", faculty);
                        command.Parameters.AddWithValue("@BuildingID", buildingID);
                        command.Parameters.AddWithValue("@RoomID", roomID);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@StartTime", startTimeOfDay);
                        command.Parameters.AddWithValue("@EndTime", endTimeOfDay);
                        command.Parameters.AddWithValue("@Status", "Pending");


                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        // Optionally, handle success/failure or redirect to another page
                        if (rowsAffected > 0)
                        {
                            // Insert successful
                            Response.Write("Insert successful!");
                            // Redirect or show success message
                        }
                        else
                        {
                            // Insert failed
                            Response.Write("Insert failed!");
                            // Handle failure
                        }
                    }
                }

            }
            //string startTime = this.RTimeDL.SelectedValue.Split('-')[0].Trim();
            //string endTime = this.RTimeDL.SelectedValue.Split('-')[1].Trim();

            //DateTime parsedStartTime = DateTime.ParseExact(startTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
            //TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

            //DateTime parsedEndTime = DateTime.ParseExact(endTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
            //TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;


            // Optional: Validate and process data as needed before inserting into the database
            // Insert into database

           
        }
    }
}