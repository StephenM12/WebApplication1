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
            Response.Redirect("Home.aspx");
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try 
            {
                // Retrieve data from form controls
                string email = this.email.Text;
                string courseCode = this.RCourseCodeTB.Text;
                string section = this.RSectionTB.Text;
                string instructor = this.RProfTB.Text;
                string faculty = this.RFacultyDL.SelectedValue;
                string building = this.SelectBuildingDL.SelectedValue;
                string roomNumber = this.RRoomNumberTB.Text.ToUpper();
                DateTime startDate = DateTime.Parse(this.SelectDateTB.Text);
                DateTime endDate = DateTime.Parse(this.EndDateTB.Text);

                //for time
                string StartTime = STimeDL.SelectedItem.Text;
                string EndTime = ETimeDL.SelectedItem.Text;


                // Parse start time
                DateTime parsedStartTime = DateTime.ParseExact(StartTime, "h:mm tt", CultureInfo.InvariantCulture);
                TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

                // Parse end time
                DateTime parsedEndTime = DateTime.ParseExact(EndTime, "h:mm tt", CultureInfo.InvariantCulture);
                TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

                string insertQuery = @"INSERT INTO RoomRequest
                (email, Course, Section, Instructor, Faculty, Building, Room, StartDate, EndDate, startTime, endTime, status)
                VALUES (@Email, @Course, @Section, @Instructor, @Faculty, @Building, @Room, @StartDate, @EndDate, @StartTime, @EndTime, @Status)";

                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Course", courseCode);
                        command.Parameters.AddWithValue("@Section", section);
                        command.Parameters.AddWithValue("@Instructor", instructor);
                        command.Parameters.AddWithValue("@Faculty", faculty);
                        command.Parameters.AddWithValue("@Building", building);
                        command.Parameters.AddWithValue("@Room", roomNumber);
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
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write("Error: " + ex.Message);
            }

        }
    }
}