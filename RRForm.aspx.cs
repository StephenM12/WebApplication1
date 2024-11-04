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
                int userlevel = user_Identity.user_level;
                if (userlevel != 1)
                {
                    PlusIconPanel.Visible = false;
                    Panel1.Visible = false;
                    Panel2.Visible = false;
                }

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
            filler.PopulateBuildings(ddlBuildings);
            filler.PopulateRooms(SelectRoomDL);
            filler.PopulateFaculty(RFacultyDL);

            SelectRoomDL.Items.Insert(0, new ListItem("Select Room", "0"));
            SelectBuildingDL.Items.Insert(0, new ListItem("Select Building", "0"));
            ddlBuildings.Items.Insert(0, new ListItem("Select Building", "0"));

            STimeDL.Items.Insert(0, new ListItem("Select Start Time", "0"));
            ETimeDL.Items.Insert(0, new ListItem("Select End Time", "0"));
        }

        //protected void BtnExport_Click(object sender, EventArgs e)
        //{
        //    if (SelectBuildingDL.SelectedIndex > 0)
        //    {
        //        try
        //        {
        //            using (var memoryStream = new System.IO.MemoryStream())
        //            {
        //                // Retrieve data from form controls
        //                string email = this.email.Text;
        //                string courseCode = this.RCourseCodeTB.Text;
        //                string section = this.RSectionTB.Text;
        //                string instructor = this.RProfTB.Text;
        //                string faculty = this.RFacultyDL.SelectedItem.Text;
        //                string building = this.SelectBuildingDL.SelectedItem.Text;
        //                //string roomNumber = this.RRoomNumberTB.Text.ToUpper();
        //                string roomNumber = this.SelectRoomDL.SelectedItem.Text;
        //                string RRpurpose = this.RRpurpose.Text;
        //                DateTime startDate = DateTime.Parse(this.SelectDateTB.Text);
        //                DateTime endDate = DateTime.Parse(this.EndDateTB.Text);

        //                //for file
        //                string fileName = fileUpload.FileName;
        //                string contentType = fileUpload.PostedFile.ContentType;

        //                fileUpload.PostedFile.InputStream.CopyTo(memoryStream);
        //                byte[] fileData = memoryStream.ToArray();

        //                //for time
        //                string StartTime = STimeDL.SelectedItem.Text;
        //                string EndTime = ETimeDL.SelectedItem.Text;

        //                // Parse start time
        //                DateTime parsedStartTime = DateTime.ParseExact(StartTime, "h:mm tt", CultureInfo.InvariantCulture);
        //                TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

        //                // Parse end time
        //                DateTime parsedEndTime = DateTime.ParseExact(EndTime, "h:mm tt", CultureInfo.InvariantCulture);
        //                TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

        //                string insertQuery = @"INSERT INTO RoomRequest
        //                (email, Course, Section, Instructor, Faculty, PurposeoftheRoom, Building, Room, StartDate, EndDate, startTime, endTime, status, FileName, FileData, ContentType)
        //                VALUES (@Email, @Course, @Section, @Instructor, @Faculty, @PurposeoftheRoom, @Building, @Room, @StartDate, @EndDate, @StartTime, @EndTime, @Status, @FileName, @FileData, @ContentType)";

        //                using (SqlConnection connection = dbConnection.GetConnection())
        //                {
        //                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
        //                    {
        //                        command.Parameters.AddWithValue("@Email", email);
        //                        command.Parameters.AddWithValue("@Course", courseCode);
        //                        command.Parameters.AddWithValue("@Section", section);
        //                        command.Parameters.AddWithValue("@Instructor", instructor);
        //                        command.Parameters.AddWithValue("@Faculty", faculty);
        //                        command.Parameters.AddWithValue("@PurposeoftheRoom", RRpurpose);
        //                        command.Parameters.AddWithValue("@Building", building);
        //                        command.Parameters.AddWithValue("@Room", roomNumber);
        //                        command.Parameters.AddWithValue("@StartDate", startDate);
        //                        command.Parameters.AddWithValue("@EndDate", endDate);
        //                        command.Parameters.AddWithValue("@StartTime", startTimeOfDay);
        //                        command.Parameters.AddWithValue("@EndTime", endTimeOfDay);
        //                        command.Parameters.AddWithValue("@Status", "Pending");
        //                        command.Parameters.AddWithValue("@FileName", fileName);
        //                        command.Parameters.AddWithValue("@ContentType", contentType);
        //                        command.Parameters.AddWithValue("@FileData", fileData);

        //                        int rowsAffected = command.ExecuteNonQuery();
        //                        //connection.Close();

        //                        // Optionally, handle success/failure or redirect to another page
        //                        if (rowsAffected > 0)
        //                        {
        //                            // Clear TextBox controls
        //                            this.email.Text = string.Empty;
        //                            this.RCourseCodeTB.Text = string.Empty;
        //                            this.RSectionTB.Text = string.Empty;
        //                            this.RProfTB.Text = string.Empty;
        //                            //this.RRoomNumberTB.Text = string.Empty;
        //                            this.RRpurpose.Text = string.Empty;
        //                            this.SelectDateTB.Text = string.Empty;
        //                            this.EndDateTB.Text = string.Empty;
        //                            fileUpload.Attributes.Clear();

        //                            // Reset DropDownLists
        //                            this.RFacultyDL.SelectedIndex = -1;
        //                            this.SelectRoomDL.SelectedIndex = -1;
        //                            this.SelectBuildingDL.SelectedIndex = -1;
        //                            this.STimeDL.SelectedIndex = -1;
        //                            this.ETimeDL.SelectedIndex = -1;

        //                            ModalPopup.ShowMessage(this.Page, "Request Sent successfully!", "RoomRequest");
        //                        }
        //                        else
        //                        {
        //                            // Insert failed
        //                            //Response.Write("Insert failed!");
        //                            ModalPopup.ShowMessage(this.Page, "Request Sent Failed!", "RoomRequest");
        //                            // Handle failure
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exceptions
        //            //Response.Write("Error: " + ex.Message);
        //            string msg = "Error: " + ex.Message;
        //            ModalPopup.ShowMessage(this.Page, msg, "Alert!");
        //        }

        //    }
        //    else
        //    {
        //        ModalPopup.ShowMessage(this.Page, "Please select a building.", "Alert!");
        //    }
        //}

        //Method to clear fields

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            if (SelectBuildingDL.SelectedIndex != 0 &&
                    SelectRoomDL.SelectedIndex != 0 &&
                    STimeDL.SelectedIndex != 0 &&
                    ETimeDL.SelectedIndex != 0 &&
                    !string.IsNullOrEmpty(SelectDateTB.Text) &&
                    !string.IsNullOrEmpty(EndDateTB.Text) &&
                    !string.IsNullOrEmpty(RSectionTB.Text) &&
                    !string.IsNullOrEmpty(RCourseCodeTB.Text) &&
                    !string.IsNullOrEmpty(RProfTB.Text))
            {
                try
                {
                    // Retrieve and parse the dates
                    DateTime startDate = DateTime.Parse(this.SelectDateTB.Text);
                    DateTime endDate = DateTime.Parse(this.EndDateTB.Text);

                    // Validate that the end date is not earlier than the start date
                    if (endDate < startDate)
                    {
                        ModalPopup.ShowMessage(this.Page, "End date cannot be earlier than start date.", "Alert!");
                        return;
                    }

                    // Retrieve data from form controls
                    //string email = this.email.Text; // This is the email of the user sending the request
                    string courseCode = this.RCourseCodeTB.Text;
                    string section = this.RSectionTB.Text;
                    string instructor = this.RProfTB.Text;
                    string faculty = this.RFacultyDL.SelectedItem.Text;
                    string building = this.SelectBuildingDL.SelectedItem.Text;
                    string roomNumber = this.SelectRoomDL.SelectedItem.Text;
                    string RRpurpose = this.RRpurpose.Text;
                    //DateTime startDate = DateTime.Parse(this.SelectDateTB.Text);
                    //DateTime endDate = DateTime.Parse(this.EndDateTB.Text);

                    // For time
                    string StartTime = STimeDL.SelectedItem.Text;
                    string EndTime = ETimeDL.SelectedItem.Text;

                    // Parse start and end times
                    DateTime parsedStartTime = DateTime.ParseExact(StartTime, "h:mm tt", CultureInfo.InvariantCulture);
                    TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;
                    DateTime parsedEndTime = DateTime.ParseExact(EndTime, "h:mm tt", CultureInfo.InvariantCulture);
                    TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

                    // Base insert query including the RequestedByEmail column
                    string insertQuery = @"INSERT INTO RoomRequest
                        (Course, Section, Instructor, Faculty, PurposeoftheRoom, Building, Room, StartDate, EndDate, startTime, endTime, status, RequestedByEmail, Requester_Faculty";

                    string valuesClause = ") VALUES (@Course, @Section, @Instructor, @Faculty, @PurposeoftheRoom, @Building, @Room, @StartDate, @EndDate, @StartTime, @EndTime, @Status, @RequestedByEmail, @Requester_Faculty";

                    bool fileUploaded = fileUpload.HasFile;
                    byte[] fileData = null;
                    string fileName = null;
                    string contentType = null;

                    if (fileUploaded)
                    {
                        fileName = fileUpload.FileName;
                        contentType = fileUpload.PostedFile.ContentType;

                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            fileUpload.PostedFile.InputStream.CopyTo(memoryStream);
                            fileData = memoryStream.ToArray();
                        }

                        // Extend the query to include file parameters
                        insertQuery += ", FileName, FileData, ContentType";
                        valuesClause += ", @FileName, @FileData, @ContentType";
                    }

                    insertQuery += valuesClause + ")";

                    using (SqlConnection connection = dbConnection.GetConnection())
                    {
                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            //command.Parameters.AddWithValue("@Email", email);
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

                            // Add the RequestedByEmail parameter
                            command.Parameters.AddWithValue("@RequestedByEmail", user_Identity.user_Email);
                            command.Parameters.AddWithValue("@Requester_Faculty", user_Identity.user_Faculty);

                            if (fileUploaded)
                            {
                                command.Parameters.AddWithValue("@FileName", fileName);
                                command.Parameters.AddWithValue("@FileData", fileData);
                                command.Parameters.AddWithValue("@ContentType", contentType);
                            }

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Clear TextBox controls and reset DropDownLists
                                ClearFormControls();
                                ConflictCard.Visible = false;
                                ModalPopup.ShowMessage(this.Page, "Request Sent successfully!", "RoomRequest");
                            }
                            else
                            {
                                ConflictCard.Visible = false;
                                ModalPopup.ShowMessage(this.Page, "Request Sent Failed!", "RoomRequest");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error: " + ex.Message;
                    ModalPopup.ShowMessage(this.Page, msg, "Alert!");
                }
            }

            else
            {
                // Show a message indicating that all inputs must be filled
                ModalPopup.ShowMessage(this.Page, "Please fill in all required fields.", "Alert!");
            }
        }

        //protected bool CheckForAllInputs()
        //{
        //    // Ensure all inputs are filled
        //    return (SelectBuildingDL.SelectedIndex != 0 &&
        //            SelectRoomDL.SelectedIndex != 0 &&
        //            STimeDL.SelectedIndex != 0 &&
        //            ETimeDL.SelectedIndex != 0 &&
        //            !string.IsNullOrEmpty(SelectDateTB.Text) &&
        //            !string.IsNullOrEmpty(EndDateTB.Text) &&
        //            !string.IsNullOrEmpty(RSectionTB.Text) &&
        //            !string.IsNullOrEmpty(RCourseCodeTB.Text) &&
        //            !string.IsNullOrEmpty(RProfTB.Text));
        //}

        private void ClearFormControls()
        {
            // Clear TextBox controls
            //this.email.Text = string.Empty;
            this.RCourseCodeTB.Text = string.Empty;
            this.RSectionTB.Text = string.Empty;
            this.RProfTB.Text = string.Empty;
            this.RRpurpose.Text = string.Empty;
            this.SelectDateTB.Text = string.Empty;
            this.EndDateTB.Text = string.Empty;
            fileUpload.Attributes.Clear();

            // Reset DropDownLists
            this.RFacultyDL.SelectedIndex = -1;
            this.SelectRoomDL.SelectedIndex = -1;
            this.SelectBuildingDL.SelectedIndex = -1;
            this.STimeDL.SelectedIndex = -1;
            this.ETimeDL.SelectedIndex = -1;
        }

        //private void ClearFields()
        //{
        //    this.email.Text = string.Empty;
        //    this.RCourseCodeTB.Text = string.Empty;
        //    this.RSectionTB.Text = string.Empty;
        //    this.RProfTB.Text = string.Empty;
        //    this.RRpurpose.Text = string.Empty;
        //    this.SelectDateTB.Text = string.Empty;
        //    this.EndDateTB.Text = string.Empty;
        //    fileUpload.Attributes.Clear();

        //    this.RFacultyDL.SelectedIndex = -1;
        //    this.SelectRoomDL.SelectedIndex = -1;
        //    this.SelectBuildingDL.SelectedIndex = -1;
        //    this.STimeDL.SelectedIndex = -1;
        //    this.ETimeDL.SelectedIndex = -1;
        //}

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

                            // Use ScriptManager to close the modal after 2 seconds
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                                setTimeout(function() {
                                    $('#addRoomModal').modal('hide');
                                }, 2000);
                            ", true);

                            txtRoomName.Text = " ";

                            //string msg = "Room added succesfully";
                            //ModalPopup.ShowMessage(Page, msg, "Note!");
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

        //checking conlfict
        protected void CheckForAllInputs(object sender, EventArgs e)
        {
            // Ensure all inputs are filled
            if (SelectBuildingDL.SelectedIndex != 0 &&
                SelectRoomDL.SelectedIndex != 0 &&
                STimeDL.SelectedIndex != 0 &&
                ETimeDL.SelectedIndex != 0 &&
                !string.IsNullOrEmpty(SelectDateTB.Text) &&
                !string.IsNullOrEmpty(EndDateTB.Text))
            {
                // Get the selected inputs
                string roomNumber = SelectRoomDL.SelectedItem.Text;
                DateTime startDate = DateTime.Parse(SelectDateTB.Text);
                DateTime endDate = DateTime.Parse(EndDateTB.Text);

                //TimeSpan startTime = TimeSpan.Parse(STimeDL.SelectedItem.Text);
                //TimeSpan endTime = TimeSpan.Parse(ETimeDL.SelectedItem.Text);

                // Parse using DateTime.ParseExact to handle AM/PM format
                DateTime startTimeParsed = DateTime.ParseExact(STimeDL.SelectedItem.Text, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endTimeParsed = DateTime.ParseExact(ETimeDL.SelectedItem.Text, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture);

                // Extract the TimeSpan from the parsed DateTime
                TimeSpan startTime = startTimeParsed.TimeOfDay;
                TimeSpan endTime = endTimeParsed.TimeOfDay;

                // Call the conflict checking method
                bool hasConflict = CheckForConflicts(roomNumber, startDate, endDate, startTime, endTime);

                // Show or hide the conflict label
                //ConflictLabel.Visible = hasConflict;
                if (hasConflict)
                {
                    ConflictLabel.Visible = true; // Make the label visible
                    ConflictCard.Style["display"] = "block"; // Show the card
                }
                else
                {
                    ConflictLabel.Visible = false; // Hide the label
                    ConflictCard.Style["display"] = "none"; // Hide the card
                }
            }
        }

        private bool CheckForConflicts(string RoomNumber, DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
        {
            bool hasConflict = false;
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                get_ID getRoom = new get_ID();
                int roomID = getRoom.GetOrInsertRoom(connection, RoomNumber, -1);

                string query = @"
                    SELECT COUNT(*)
                    FROM Schedule
                    WHERE RoomID = @RoomID
                      AND (
                        (ScheduleDate <= @EndDate AND ScheduleDate >= @StartDate)
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
            return hasConflict;
        }
    }
}