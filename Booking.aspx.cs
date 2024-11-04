using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mail;
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
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                try
                {
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

                using (SqlConnection connection = dbConnection.GetConnection())
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

            using (SqlConnection connection = dbConnection.GetConnection())
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

        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Deploy" || e.CommandName == "Reject")
        //    {
        //        int rowIndex = Convert.ToInt32(e.CommandArgument);

        //        GridViewRow row = GridView1.Rows[rowIndex];
        //        int requestId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);
        //        using (SqlConnection connection = dbConnection.GetConnection())
        //        {
        //            if (e.CommandName == "Deploy")
        //            {
        //                //Access RoomNumber from TemplateField
        //                Label lblRoomName = (Label)row.FindControl("lblRoomName");
        //                string Room = lblRoomName != null ? lblRoomName.Text.ToUpper() : string.Empty;

        //                // Check room existence
        //                bool roomExists = RoomExists(connection, Room);

        //                if (!roomExists)
        //                {
        //                    txtRoomName.Text = Room;

        //                    string script = "showConfirmAddModal();";
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowModalScript", script, true);
        //                }
        //                else
        //                {
        //                    //Extract data from the row controls
        //                    string email = row.Cells[1].Text;
        //                    string course = row.Cells[2].Text;
        //                    string section = row.Cells[3].Text;
        //                    string Instructor = row.Cells[4].Text;
        //                    //string Faculty = row.Cells[5].Text;
        //                    string Building = row.Cells[6].Text;
        //                    //string Room = ((Label)row.FindControl("lblRoomName")).Text; ;
        //                    string StartDate = row.Cells[8].Text;
        //                    string EndDate = row.Cells[9].Text;
        //                    string startTime = row.Cells[10].Text;
        //                    string endTime = row.Cells[11].Text;
        //                    //string PurposeoftheRoom = row.Cells[12].Text;

        //                    get_ID getID = new get_ID();

        //                    //string insertQuery = @"
        //                    //    INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, DayID, StartTime, EndTime, StartDate, EndDate, Remarks, BuildingID)
        //                    //    VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @DayID, @StartTime, @EndTime, @StartDate, @EndDate, @Remarks, @BuildingID)";

        //                    string insertQuery = @"
        //                        INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, DayID, StartTime, EndTime, ScheduleDate, Remarks, BuildingID)
        //                        VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @DayID, @StartTime, @EndTime, @ScheduleDate, @Remarks, @BuildingID)";

        //                    try
        //                    {
        //                        // Convert necessary input values to appropriate data types
        //                        int courseID = getID.GetOrInsertCourse(connection, course);
        //                        int sectionID = getID.GetOrInsertSection(connection, section);
        //                        int instructorID = getID.GetOrInsertInstructor(connection, Instructor);
        //                        int buildingID = getID.GetOrInsertBuilding(connection, Building);
        //                        int roomID = getID.GetOrInsertRoom(connection, Room, buildingID);

        //                        //DateTime startDt = DateTime.Parse(StartDate);
        //                        //DateTime endDt = DateTime.Parse(EndDate);
        //                        DateTime startDt = DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //                        DateTime endDt = DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


        //                        TimeSpan startTm = TimeSpan.Parse(startTime);
        //                        TimeSpan endTm = TimeSpan.Parse(endTime);

        //                        // Loop through each date between startDate and endDate
        //                        for (DateTime date = startDt; date <= endDt; date = date.AddDays(1))
        //                        {
        //                            // Get the day of the week and corresponding DayID
        //                            string dayName = date.DayOfWeek.ToString().ToUpper();
        //                            int dayID = getID.GetDayID(dayName);

        //                            string remarks = "This Schedule is Requested by email: " + email;
        //                            using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
        //                            {
        //                                // Add parameters to the command
        //                                cmd.Parameters.AddWithValue("@RoomID", roomID);
        //                                cmd.Parameters.AddWithValue("@SectionID", sectionID);
        //                                cmd.Parameters.AddWithValue("@CourseID", courseID);
        //                                cmd.Parameters.AddWithValue("@InstructorID", instructorID);
        //                                cmd.Parameters.AddWithValue("@DayID", dayID);
        //                                cmd.Parameters.AddWithValue("@StartTime", startTm);
        //                                cmd.Parameters.AddWithValue("@EndTime", endTm);
        //                                //cmd.Parameters.AddWithValue("@StartDate", startDt);
        //                                //cmd.Parameters.AddWithValue("@EndDate", endDt);
        //                                cmd.Parameters.AddWithValue("@ScheduleDate", date);
        //                                cmd.Parameters.AddWithValue("@Remarks", remarks);
        //                                cmd.Parameters.AddWithValue("@BuildingID", buildingID);

        //                                // Execute the insert command
        //                                int affectedRows = cmd.ExecuteNonQuery();
        //                                if (affectedRows > 0)
        //                                {
        //                                    BindScheduleData(sender, e);

        //                                    // The insert was successful
        //                                    ModalPopup.ShowMessage_(this.Page, "Successfully Deployed", "Notification");

        //                                }
        //                                else
        //                                {
        //                                    // The insert was not successful, no rows were affected
        //                                    ModalPopup.ShowMessage_(this.Page, "Deploy not successful", "Alert!");
        //                                }
        //                            }
        //                        }


        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ModalPopup.ShowMessage_(this.Page, ex.Message, "Alert!");
        //                    }
        //                    finally
        //                    {
        //                        BindScheduleData(sender, e);
        //                    }
        //                }
        //            };
        //            string newStatus = e.CommandName == "Deploy" ? "Deployed" : "Rejected";
        //            UpdateRoomRequestStatus(requestId, newStatus, user_Identity.user_Email);
        //        }
        //    }
        //}
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deploy" || e.CommandName == "Reject")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];
                int requestId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);
                hiddenRequestId.Value = requestId.ToString();

                // Send email notification
                string requestedByEmail = row.Cells[1].Text;
                HiddenRequester_Email.Value = requestedByEmail;
                System.Diagnostics.Debug.WriteLine(requestedByEmail);

                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    if (e.CommandName == "Deploy")
                    {
                        Label lblRoomName = (Label)row.FindControl("lblRoomName");
                        string Room = lblRoomName != null ? lblRoomName.Text.ToUpper() : string.Empty;

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
                            string email = row.Cells[1].Text;
                            string course = row.Cells[2].Text;
                            string section = row.Cells[3].Text;
                            string Instructor = row.Cells[4].Text;
                            string Building = row.Cells[6].Text;
                            string StartDate = row.Cells[8].Text;
                            string EndDate = row.Cells[9].Text;
                            string startTime = row.Cells[10].Text;
                            string endTime = row.Cells[11].Text;

                            System.Diagnostics.Debug.WriteLine(Instructor);
                            get_ID getID = new get_ID();

                            string updateQuery = @"
                                UPDATE Schedule 
                                SET SectionID = @SectionID, 
                                    CourseID = @CourseID, 
                                    InstructorID = @InstructorID, 
                                    DayID = @DayID, 
                                    StartTime = @StartTime, 
                                    EndTime = @EndTime, 
                                    ScheduleDate = @ScheduleDate, 
                                    Remarks = @Remarks, 
                                    BuildingID = @BuildingID
                                WHERE RoomID = @RoomID 
                                  AND BuildingID = @BuildingID 
                                  AND StartTime = @StartTime 
                                  AND EndTime = @EndTime 
                                  AND ScheduleDate = @ScheduleDate";

                            string insertQuery = @"
                            INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, DayID, StartTime, EndTime, ScheduleDate, Remarks, BuildingID)
                            VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @DayID, @StartTime, @EndTime, @ScheduleDate, @Remarks, @BuildingID)";

                            try
                            {
                                int courseID = getID.GetOrInsertCourse(connection, course);
                                int sectionID = getID.GetOrInsertSection(connection, section);
                                int instructorID = getID.GetOrInsertInstructor(connection, Instructor);
                                int buildingID = getID.GetOrInsertBuilding(connection, Building);
                                int roomID = getID.GetOrInsertRoom(connection, Room, buildingID);

                                DateTime startDt = DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                DateTime endDt = DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                TimeSpan startTm = TimeSpan.Parse(startTime);
                                TimeSpan endTm = TimeSpan.Parse(endTime);

                                for (DateTime date = startDt; date <= endDt; date = date.AddDays(1))
                                {
                                    string dayName = date.DayOfWeek.ToString().ToUpper();
                                    int dayID = getID.GetDayID(dayName);

                                    string remarks = "This Schedule is Requested by email: " + email;

                                    if (ScheduleExists(connection, roomID, buildingID, startTm, endTm, date))
                                    {
                                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                                        {
                                            cmd.Parameters.AddWithValue("@RoomID", roomID);
                                            cmd.Parameters.AddWithValue("@SectionID", sectionID);
                                            cmd.Parameters.AddWithValue("@CourseID", courseID);
                                            cmd.Parameters.AddWithValue("@InstructorID", instructorID);
                                            cmd.Parameters.AddWithValue("@DayID", dayID);
                                            cmd.Parameters.AddWithValue("@StartTime", startTm);
                                            cmd.Parameters.AddWithValue("@EndTime", endTm);
                                            cmd.Parameters.AddWithValue("@ScheduleDate", date);
                                            cmd.Parameters.AddWithValue("@Remarks", remarks);
                                            cmd.Parameters.AddWithValue("@BuildingID", buildingID);

                                            int affectedRows = cmd.ExecuteNonQuery();
                                            if (affectedRows > 0)
                                            {
                                                ModalPopup.ShowMessage_(this.Page, "Successfully Updated", "Notification");
                                            }
                                            else
                                            {
                                                ModalPopup.ShowMessage_(this.Page, "Update not successful", "Alert!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                                        {
                                            cmd.Parameters.AddWithValue("@RoomID", roomID);
                                            cmd.Parameters.AddWithValue("@SectionID", sectionID);
                                            cmd.Parameters.AddWithValue("@CourseID", courseID);
                                            cmd.Parameters.AddWithValue("@InstructorID", instructorID);
                                            cmd.Parameters.AddWithValue("@DayID", dayID);
                                            cmd.Parameters.AddWithValue("@StartTime", startTm);
                                            cmd.Parameters.AddWithValue("@EndTime", endTm);
                                            cmd.Parameters.AddWithValue("@ScheduleDate", date);
                                            cmd.Parameters.AddWithValue("@Remarks", remarks);
                                            cmd.Parameters.AddWithValue("@BuildingID", buildingID);

                                            int affectedRows = cmd.ExecuteNonQuery();
                                            if (affectedRows > 0)
                                            {
                                                ModalPopup.ShowMessage_(this.Page, "Successfully Deployed", "Notification");
                                            }
                                            else
                                            {
                                                ModalPopup.ShowMessage_(this.Page, "Deploy not successful", "Alert!");
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ModalPopup.ShowMessage_(this.Page, ex.Message, "Alert!");
                            }
                        }

                        //string ReqStatus = e.CommandName;
                        UpdateRoomRequestStatus(requestId, "Deployed", user_Identity.user_Email);
                    }
                    if (e.CommandName == "Reject")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showRejectModal", "$('#rejectModal').modal('show');", true);
                        return; // Return early to avoid updating status until the remark is submitted

                    }
                    //string newStatus = e.CommandName == "Deploy" ? "Deployed" : "Rejected";
                    

                    // Bind the GridView after updating the status
                    BindScheduleData(sender,e);
                }
            }
        }

        protected void btnSubmitRemark_Click(object sender, EventArgs e)
        {
            int requestId = Convert.ToInt32(hiddenRequestId.Value);
            string remark = remarkText.Text;
            string requestedByEmail = HiddenRequester_Email.Value;
            System.Diagnostics.Debug.WriteLine(requestedByEmail);

            if (string.IsNullOrWhiteSpace(remark))
            {
                UpdateRoomRequestStatus(requestId, "Rejected", user_Identity.user_Email);
                SendEmailNotification(requestedByEmail, "Rejected");

            }
            else
            {
                UpdateRoomRequestStatus(requestId, "Rejected", user_Identity.user_Email);
                SendEmailNotification(requestedByEmail, "Rejected");

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "testingproject2001@gmail.com";
                string smtpPassword = "uxws wbem dspt pdjd";

                string subject = "Additional Remarks From Your Room Request Status";
                string body = $"{remark}\n\nThank you.";


                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);
                    emailSender.SendEmail("testingproject2001@gmail.com", requestedByEmail, subject, body);

                }

            }

            // Rebind the data and close the modal
            BindScheduleData(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "hideRejectModal", "$('#rejectModal').modal('hide');", true);
        }
        private void SendEmailNotification(string email, string status)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "testingproject2001@gmail.com";
            string smtpPassword = "uxws wbem dspt pdjd";

            string subject = "Update on Your Room Request Status";
            string body = $"Dear User,\n\nYour room request has been {status} by the Immediate Superior.";

            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);
                emailSender.SendEmail("testingproject2001@gmail.com", email, subject, body);



            }
        }
        private bool ScheduleExists(SqlConnection connection, int roomID, int buildingID, TimeSpan startTime, TimeSpan endTime, DateTime scheduleDate)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM Schedule 
                WHERE RoomID = @RoomID 
                  AND BuildingID = @BuildingID 
                  AND StartTime = @StartTime 
                  AND EndTime = @EndTime 
                  AND ScheduleDate = @ScheduleDate";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@RoomID", roomID);
                cmd.Parameters.AddWithValue("@BuildingID", buildingID);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                cmd.Parameters.AddWithValue("@ScheduleDate", scheduleDate);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        protected string CheckScheduleConflict(IDataItemContainer container)
        {
            var row = (GridViewRow)container;

            // Extract room ID and other values
            string RoomNumber = (string)DataBinder.Eval(row.DataItem, "Room");
            DateTime startDate = Convert.ToDateTime(DataBinder.Eval(row.DataItem, "StartDate"));
            DateTime endDate = Convert.ToDateTime(DataBinder.Eval(row.DataItem, "EndDate"));
            //DateTime scheduleDate = Convert.ToDateTime(DataBinder.Eval(row.DataItem, "ScheduleDate"));

            TimeSpan startTime = (TimeSpan)DataBinder.Eval(row.DataItem, "startTime");
            TimeSpan endTime = (TimeSpan)DataBinder.Eval(row.DataItem, "endTime");

            // Check for conflicts in the database
            bool hasConflict = CheckForConflicts(RoomNumber, startDate, endDate, startTime, endTime);

            return hasConflict ? "display:inline;" : "display:none;";
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
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@RoomID", roomID);
                    //command.Parameters.AddWithValue("@ScheduleDate", scheduleDate.Date); // Use .Date to ensure we're only using the date part
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.Parameters.AddWithValue("@EndTime", endTime);

                    // Execute the query and get the count of conflicting schedules
                    int count = (int)command.ExecuteScalar();
                    hasConflict = (count > 0); // Set hasConflict to true if conflicts exist
                }
            }

            // Return true if conflict exists, otherwise false
            return hasConflict;
        }

        private void UpdateRoomRequestStatus(int requestId, string status, string Updatedby)
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                string sql = "UPDATE RoomRequest SET status = @status, UpdatedBy = @UpdatedBy WHERE RequestID = @requestId";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@UpdatedBy", Updatedby);
                    cmd.Parameters.AddWithValue("@requestId", requestId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected string GetFileIcon(string contentType)
        {
            switch (contentType.ToLower())
            {
                case "image/jpeg":
                case "image/png":
                    return "~/images/img.png"; // Replace with your image path
                case "application/pdf":
                    return "~/images/pdf.png"; // Replace with your image path
                case "application/msword":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return "~/images/docx.png"; // Replace with your image path
                case "application/vnd.ms-excel":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "~/images/xlsx.png"; // Replace with your image path
                                                // Add more cases as needed for different file types
                default:
                    return "~/images/default-icon.png"; // Default icon for unknown types
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}