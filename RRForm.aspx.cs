using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace RoomRequestForm
{
    public partial class RRForm : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropdown_datas(sender, e);
            }
        }

        protected void Btn_Back(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void dropdown_datas(object sender, EventArgs e)
        {
            DropdownFiller filler = new DropdownFiller();
            filler.PopulateBuildings(SelectBuildingDL);

            SelectBuildingDL.Items.Insert(0, new ListItem("Select Building", "0"));
        }

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            if (SelectBuildingDL.SelectedIndex > 0)
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
                    string RRpurpose = this.RRpurpose.Text;
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
                    (email, Course, Section, Instructor, Faculty, PurposeoftheRoom, Building, Room, StartDate, EndDate, startTime, endTime, status)
                    VALUES (@Email, @Course, @Section, @Instructor, @Faculty, @PurposeoftheRoom, @Building, @Room, @StartDate, @EndDate, @StartTime, @EndTime, @Status)";

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
                            command.Parameters.AddWithValue("@PurposeoftheRoom", RRpurpose);
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
                                //Response.Write("Insert successful!");
                                // Redirect or show success message
                                ModalPopup.ShowMessage(this.Page, "Request Sent successfully!", "RoomRequest");
                            }
                            else
                            {
                                // Insert failed
                                //Response.Write("Insert failed!");
                                ModalPopup.ShowMessage(this.Page, "Request Sent Failed!", "RoomRequest");
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
                finally
                {
                    // Clear TextBox controls
                    this.email.Text = string.Empty;
                    this.RCourseCodeTB.Text = string.Empty;
                    this.RSectionTB.Text = string.Empty;
                    this.RProfTB.Text = string.Empty;
                    this.RRoomNumberTB.Text = string.Empty;
                    this.RRpurpose.Text = string.Empty;
                    this.SelectDateTB.Text = string.Empty;
                    this.EndDateTB.Text = string.Empty;

                    // Reset DropDownLists
                    this.RFacultyDL.SelectedIndex = -1;
                    this.SelectBuildingDL.SelectedIndex = -1;
                    this.STimeDL.SelectedIndex = -1;
                    this.ETimeDL.SelectedIndex = -1;
                }

            }
            else
            {
                
                ModalPopup.ShowMessage(this.Page, "Please select a building.", "Alert!");

            }

        }
    }
}