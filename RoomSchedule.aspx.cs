//for excel package
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;

//sql connection:
using System.Data.SqlClient;

//testing
using System.Globalization;

//file processing
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class RoomSchedule : System.Web.UI.Page
    {
        private class ScheduleRow
        {
            public object RoomID { get; set; }
            public object SectionID { get; set; }
            public object CourseID { get; set; }
            public object InstructorID { get; set; }
            public int Day { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Remarks { get; set; }
            public object BuildingID { get; set; }
            public int fileID { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ModalPopup.RegisterModalHtml(this.Page);
                Dropdown_Datas(sender, e);

                //make this visible if u need to use edit button
                REditBtn.Visible = false;

                int userlevel = user_Identity.user_level;
                if (userlevel == 2 || userlevel == 3)
                {
                    RAddSchedBtn.Visible = false;
                    REditBtn.Visible = false;
                    RUploadFileBtn.Visible = false;
                }
             
               
                upload_DropDownList1.SelectedIndex = 0;

                if (DropDownList2.Items.Count > 0)
                {
                    DropDownList2.SelectedIndex = 0;
                    BindScheduleData(sender, e);
                }
            }
        }

        //file handling
        protected void Upload_File(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contentType = FileUpload1.PostedFile.ContentType;

            string uploader = user_Identity.user_FName + user_Identity.user_LName; // Replace with actual uploader name
            DateTime uploadDate = DateTime.Now;

            //Get the selected date from the TextBox
            string selectedDate = calendar_TB1.Text;
            string selectedEndDate = calendar_TB2.Text;

            // Convert to DateTime directly
            DateTime Sdate = DateTime.Parse(selectedDate);
            DateTime Edate = DateTime.Parse(selectedEndDate);

            try
            {
                if (FileUpload1.HasFile)
                {
                    if (Sdate != DateTime.MinValue && Edate != DateTime.MinValue)
                    {
                        try
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (Stream excelStream = FileUpload1.PostedFile.InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(excelStream))
                                {
                                    byte[] bytes = br.ReadBytes((Int32)excelStream.Length);

                                    // Open database connection
                                    SqlConnection connection = dbConnection.GetConnection();
                                    if (connection.State == System.Data.ConnectionState.Open)
                                    {
                                        // Perform your database operations here:
                                        //string query = @"INSERT INTO upload_SchedsTBL (FileName, ContentType, Data, Uploader, UploadDate) VALUES (@FileName, @ContentType, @FileData, @Uploader, @UploadDate)";
                                        string query = @"INSERT INTO upload_SchedsTBL (FileName, ContentType, Data, Uploader, UploadDate) 
                                                         VALUES (@FileName, @ContentType, @FileData, @Uploader, @UploadDate);
                                                         SELECT SCOPE_IDENTITY();";  // Get the last inserted ID

                                        using (SqlCommand command = new SqlCommand(query, connection))
                                        {
                                            try
                                            {
                                                command.Parameters.AddWithValue("@FileName", fileName);
                                                command.Parameters.AddWithValue("@ContentType", contentType);
                                                command.Parameters.AddWithValue("@FileData", bytes);
                                                command.Parameters.AddWithValue("@Uploader", uploader);
                                                command.Parameters.AddWithValue("@UploadDate", uploadDate);

                                                //command.ExecuteNonQuery();

                                                // Execute the query and get the UploadID
                                                object result = command.ExecuteScalar();
                                                int uploadID = Convert.ToInt32(result);

                                                ReadExcelData(excelStream, uploadID);
                                            }
                                            catch (Exception ex)
                                            {
                                                // Handle the error here
                                                //Console.WriteLine("An error occurred while inserting into upload_SchedsTBL: " + ex.Message);

                                                string msg = "An error occurred while inserting into upload_SchedsTBL: " + ex.Message;
                                                ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it accordingly
                            //Console.WriteLine("An error occurred during the file upload process: " + ex.Message);
                           string msg = "An error occurred during the file upload process. Please try again." + ex.Message;
                            ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
                        }
                    }
                    else
                    {
                        // Calendar1 does not have a selected date
                        //Response.Write("Pls Select the Start and End date from the Calendars");

                        ModalPopup.ShowMessage_(this.Page, "Pls Select the Start and End date from the Calendars", "Alert!");
                    }
                }
                else
                {
                    // Handle the case when no file is uploaded
                    //Response.Write("Please upload a file.");
                    ModalPopup.ShowMessage_(this.Page, "Please upload a file.", "Alert!");


                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
               string msg = "Error: " + ex.Message;
               ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
            }

            // Calendar1 has a selected date
        }

        public void ReadExcelData(Stream excelStream, int fileID_)
        {
            try
            {
                // Open database connection
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        using (ExcelPackage package = new ExcelPackage(excelStream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                            int rowCount = worksheet.Dimension.Rows;
                            int colCount = worksheet.Dimension.Columns;

                            string currentRoom = string.Empty;
                            List<ScheduleRow> scheduleRows = new List<ScheduleRow>();

                            // Loop through each row in the Excel worksheet
                            for (int row = 1; row <= rowCount; row++)
                            {
                                string firstCellText = worksheet.Cells[row, 1].Text;

                                if (firstCellText.StartsWith("R") || firstCellText.StartsWith("E"))
                                {
                                    currentRoom = firstCellText; // Update current room when room header is found
                                }
                                else if (firstCellText.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Skip header row
                                    continue;
                                }
                                else if (!string.IsNullOrEmpty(currentRoom) && !string.IsNullOrEmpty(firstCellText))
                                {
                                    string time = worksheet.Cells[row, 1].Text;
                                    string[] timeParts = time.Split('-');

                                    for (int col = 2; col <= colCount; col++)
                                    {
                                        //hmmm try 4
                                        string day = worksheet.Cells[5, col].Text.ToUpper(); // Assuming row 3 contains day names

                                        // Get DayID from days_of_week table based on day name
                                        get_ID getDay = new get_ID();
                                        int dayID = getDay.GetDayID(day);
                                        //int dayID = GetDayID(day);

                                        string schedule = worksheet.Cells[row, col].Text;

                                        if (!string.IsNullOrEmpty(schedule))
                                        {
                                            // Split the schedule into parts
                                            string[] scheduleParts = schedule.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                            if (scheduleParts.Length > 0)
                                            {
                                                string courseCode = scheduleParts[0];
                                                string section = "";
                                                string instructor = "";

                                                if (scheduleParts.Length == 2 && scheduleParts[1].Contains(" "))
                                                {
                                                    string[] sectionAndInstructor = scheduleParts[1].Split(' ');
                                                    section = sectionAndInstructor[0];
                                                    instructor = sectionAndInstructor[1];
                                                }
                                                else if (scheduleParts.Length >= 2 && !scheduleParts[1].Contains(" "))
                                                {
                                                    section = scheduleParts[1];
                                                }
                                                else if (scheduleParts.Length >= 3 && !scheduleParts[2].Contains(" "))
                                                {
                                                    section = scheduleParts[2];
                                                }
                                                //Response.Write($"Room: {currentRoom}, Section: {section}, CourseCode: {courseCode}, Instructor: {instructor}, Day: {dayID}, StartTime: {time.Split('-')[0]}, EndTime: {time.Split('-')[1]}, StartDate: {DateTime.Today.ToShortDateString()}, EndDate: {DateTime.Today.ToShortDateString()}<br />");
                                                // Create a new ScheduleRow object

                                                DateTime parsedTime = DateTime.ParseExact(timeParts[0].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                                                TimeSpan timeOfDay = parsedTime.TimeOfDay;

                                                DateTime END_parsedTime = DateTime.ParseExact(timeParts[1].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                                                TimeSpan END_timeOfDay = END_parsedTime.TimeOfDay;

                                                int buildID = int.Parse(upload_DropDownList1.SelectedValue);

                                                get_ID dbHelper = new get_ID();
                                                var ids = dbHelper.CheckAndInsertValues(connection, currentRoom, section, courseCode, instructor, true, buildID);

                                                //Get the selected date from the TextBox
                                                string selectedDate = calendar_TB1.Text;
                                                string selectedEndDate = calendar_TB2.Text;

                                                // Convert to DateTime directly
                                                DateTime Sdate = DateTime.Parse(selectedDate);
                                                DateTime Edate = DateTime.Parse(selectedEndDate);

                                                //get_ID building id
                                                var buildingID = dbHelper.GetOrInsertBuilding(connection, upload_DropDownList1.SelectedValue);

                                                ScheduleRow newRow = new ScheduleRow
                                                {
                                                    RoomID = ids.roomID == -1 ? (object)DBNull.Value : ids.roomID,
                                                    SectionID = ids.sectionID == -1 ? (object)DBNull.Value : ids.sectionID,
                                                    CourseID = ids.courseID == -1 ? (object)DBNull.Value : ids.courseID,
                                                    InstructorID = ids.instructorID == -1 ? (object)DBNull.Value : ids.instructorID,
                                                    Day = dayID,
                                                    StartTime = timeOfDay,
                                                    EndTime = END_timeOfDay,
                                                    StartDate = Sdate,
                                                    EndDate = Edate,
                                                    Remarks = null,
                                                    BuildingID = buildingID,
                                                    fileID = fileID_
                                                };

                                                // Add the new row to the list
                                                scheduleRows.Add(newRow);
                                            }
                                        }
                                    }
                                }
                            }

                            // After collecting all schedule data, convert it to DataTable
                            DataTable scheduleDataTable = ConvertToDataTable(scheduleRows);

                            // Bulk insert into Schedule table using SqlBulkCopy
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                            {
                                bulkCopy.DestinationTableName = "Schedule";

                                // Map DataTable columns to SQL Server table columns
                                bulkCopy.ColumnMappings.Add("RoomID", "RoomID");
                                bulkCopy.ColumnMappings.Add("SectionID", "SectionID");
                                bulkCopy.ColumnMappings.Add("CourseID", "CourseID");
                                bulkCopy.ColumnMappings.Add("InstructorID", "InstructorID");
                                bulkCopy.ColumnMappings.Add("Day", "DayID");
                                bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                                bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
                                bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                                bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                                bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                                bulkCopy.ColumnMappings.Add("BuildingID", "BuildingID");
                                bulkCopy.ColumnMappings.Add("fileID", "UploadID");

                                // Perform the bulk copy
                                bulkCopy.WriteToServer(scheduleDataTable);

                                
                                Response.Write("File uploaded successfully.");
                            }
                        }

                        //will update the contents of this:
                        Dropdown_Datas(null, EventArgs.Empty);
                        BindScheduleData(null, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred during the file upload process. Please try again." + ex.Message);
            }
            finally
            {
                // Clear FileUpload control
                FileUpload1.Attributes.Clear();

                // Clear the TextBox inputs (start and end date)
                calendar_TB1.Text = string.Empty;
                calendar_TB2.Text = string.Empty;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoader", "hideLoading();", true);

            }
        }

        private DataTable ConvertToDataTable(List<ScheduleRow> scheduleRows)
        {
            DataTable dataTable = new DataTable();

            // Define the schema of the DataTable
            dataTable.Columns.Add("RoomID", typeof(object));
            dataTable.Columns.Add("SectionID", typeof(object));
            dataTable.Columns.Add("CourseID", typeof(object));
            dataTable.Columns.Add("InstructorID", typeof(object));
            dataTable.Columns.Add("Day", typeof(string));
            dataTable.Columns.Add("StartTime", typeof(TimeSpan));
            dataTable.Columns.Add("EndTime", typeof(TimeSpan));
            dataTable.Columns.Add("StartDate", typeof(DateTime));
            dataTable.Columns.Add("EndDate", typeof(DateTime));
            dataTable.Columns.Add("Remarks", typeof(string));
            dataTable.Columns.Add("BuildingID", typeof(object));
            dataTable.Columns.Add("fileID", typeof(object));

            // Fill the DataTable with data from List<ScheduleRow>
            foreach (var scheduleRow in scheduleRows)
            {
                DataRow row = dataTable.NewRow();
                row["RoomID"] = scheduleRow.RoomID;
                row["SectionID"] = scheduleRow.SectionID;
                row["CourseID"] = scheduleRow.CourseID;
                row["InstructorID"] = scheduleRow.InstructorID;
                row["Day"] = scheduleRow.Day;
                row["StartTime"] = scheduleRow.StartTime;
                row["EndTime"] = scheduleRow.EndTime;
                row["StartDate"] = scheduleRow.StartDate;
                row["EndDate"] = scheduleRow.EndDate;
                row["Remarks"] = DBNull.Value;
                row["BuildingID"] = scheduleRow.BuildingID;
                row["fileID"] = scheduleRow.fileID;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        //changed the data that is showed based on the calendar
        protected void BindScheduleData(object sender, EventArgs e)
        {
            string selected_ID_ROOM = DropDownList2.SelectedValue;
            string selected_File = uploadSchedsDL.SelectedValue;

            DateTime selectedDate;

            // Check if a date is selected, otherwise use today's date or a default date within the valid range
            if (Calendar3.SelectedDate == DateTime.MinValue)
            {
                // If no date is selected, use today's date as a default
                selectedDate = DateTime.Today;
            }
            else
            {
                selectedDate = Calendar3.SelectedDate;
                
            }

            // Calculate the day of the week for the selected date
            int dayOfWeek = (int)selectedDate.DayOfWeek + 1; // Adding 1 to match DayID (Sunday = 1, Monday = 2, ...)

            // Store the day of the week in a hidden field (optional, if needed in client-side code)
            hiddenDayOfWeek.Value = dayOfWeek.ToString();



            //testing
            string query = @"
                SELECT
                s.ScheduleID,  -- Include ScheduleID in the SELECT
                CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
                MAX(CASE WHEN s.DayID = 1 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Sunday],
                MAX(CASE WHEN s.DayID = 2 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Monday],
                MAX(CASE WHEN s.DayID = 3 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Tuesday],
                MAX(CASE WHEN s.DayID = 4 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Wednesday],
                MAX(CASE WHEN s.DayID = 5 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Thursday],
                MAX(CASE WHEN s.DayID = 6 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Friday],
                MAX(CASE WHEN s.DayID = 7 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Saturday]
                FROM Schedule s
                JOIN Sections sec ON s.SectionID = sec.SectionID
                JOIN Courses c ON s.CourseID = c.CourseID
                JOIN Instructors i ON s.InstructorID = i.InstructorID
                WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)
                AND @SelectedDate BETWEEN s.StartDate AND s.EndDate
                AND s.DayID = @DayOfWeek
                AND (@UploadID = 0 OR s.UploadID = @UploadID)
                GROUP BY s.ScheduleID, s.StartTime, s.EndTime  -- Group by ScheduleID, StartTime, and EndTime
                ORDER BY s.StartTime;
            ";

            //end of testing



            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
                    cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

                    // If no file is selected or selected value is 0, ignore the file filter
                    int selectedFileID = string.IsNullOrEmpty(selected_File) ? 0 : int.Parse(selected_File);
                    cmd.Parameters.AddWithValue("@UploadID", selectedFileID);


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        GridView1.DataKeyNames = new string[] { "ScheduleID" };

                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            DateTime SelectedDate;
            SelectedDate = Calendar3.SelectedDate;
            HiddenField2.Value = SelectedDate.ToString();

            BindScheduleData(sender, e);
        }

        //updating the data upon editing from the gridview
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowModal")
            {
                int scheduleID = Convert.ToInt32(e.CommandArgument);


                string query = @"
                    SELECT 
                        s.ScheduleID,
                        r.RoomName,
                        c.CourseCode,
                        sec.SectionName,
                        i.InstructorName,
                        b.BuildingName,
                        s.StartDate,
                        s.EndDate,
                        s.Remarks
                    FROM Schedule s
                    JOIN Rooms r ON s.RoomID = r.RoomID
                    JOIN Courses c ON s.CourseID = c.CourseID
                    JOIN Sections sec ON s.SectionID = sec.SectionID
                    JOIN Instructors i ON s.InstructorID = i.InstructorID
                    JOIN Buildings b ON s.BuildingID = b.BuildingID
                    WHERE s.ScheduleID = @ScheduleID";

                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleID", scheduleID);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                HiddenField1.Value = scheduleID.ToString();

                                //txtScheduleID.Text = reader["ScheduleID"].ToString();
                                txtRoomID.Text = reader["RoomName"].ToString();
                                txtCourseID.Text = reader["CourseCode"].ToString();
                                txtSectionID.Text = reader["SectionName"].ToString();
                                txtInstructorID.Text = reader["InstructorName"].ToString();
                                txtBuildingID.Text = reader["BuildingName"].ToString();
                                //txtStartTime.Text = reader["StartTime"].ToString();
                                //txtEndTime.Text = reader["EndTime"].ToString();
                                txtStartDate.Text = Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd");
                                txtEndDate.Text = Convert.ToDateTime(reader["EndDate"]).ToString("yyyy-MM-dd");
                                txtRemarks.Text = reader["Remarks"].ToString();
                            }
                        }
                    }
                }

                // Show the modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#scheduleModal').modal('show');", true);
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            
            btnEdit.Visible = false;
           
            //txtBuildingID.ReadOnly = false;
            //txtRoomID.ReadOnly = false;
            txtCourseID.ReadOnly = false;
            txtSectionID.ReadOnly = false;
            txtInstructorID.ReadOnly = false;
            //txtStartTime.ReadOnly = false;
            //txtEndTime.ReadOnly = false;
            txtStartDate.ReadOnly = false;
            txtEndDate.ReadOnly = false;
            txtRemarks.ReadOnly = false;

            btnUpdate.Visible = true;



        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Label1.Text = HiddenField1.Value;
            try
            {

                string updateQuery = @"
                UPDATE Schedule
                SET 
                    CourseID = @CourseID,
                    SectionID = @SectionID,
                    InstructorID = @InstructorID,    
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Remarks = @Remarks
                WHERE ScheduleID = @ScheduleID";

                using (SqlConnection connection = dbConnection.GetConnection())
                {

                    get_ID add_dbHelper = new get_ID();
                    int sectionID = add_dbHelper.GetOrInsertSection(connection, txtSectionID.Text);
                    int instructorID = add_dbHelper.GetOrInsertInstructor(connection, txtInstructorID.Text);
                    int coursecodeID = add_dbHelper.GetOrInsertCourse(connection, txtCourseID.Text);

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        //cmd.Parameters.AddWithValue("@RoomName", txtRoomID.Text);
                        cmd.Parameters.AddWithValue("@CourseID", coursecodeID);
                        cmd.Parameters.AddWithValue("@SectionID", sectionID);
                        cmd.Parameters.AddWithValue("@InstructorID", instructorID);
                        //cmd.Parameters.AddWithValue("@BuildingName", txtBuildingID.Text);
                        cmd.Parameters.AddWithValue("@StartDate", txtStartDate.Text);
                        cmd.Parameters.AddWithValue("@EndDate", txtEndDate.Text);
                        cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                        cmd.Parameters.AddWithValue("@ScheduleID", HiddenField1.Value);


                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Update successful: close modal and display success message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal", "$('#scheduleModal').modal('hide');", true);

                            // Optionally, refresh the GridView or notify the user
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", "alert('Schedule updated successfully.');", true);
                            ModalPopup.ShowMessage_(Page, "Schedule updated successfully.", "Notification");


                            // Optionally, refresh GridView
                            //GridView1.DataBind();

                            BindScheduleData(sender,e);
                        }
                        else
                        {
                            // Update failed: display error message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", "alert('Error: No rows were updated.');", true);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions
                string msg = "Error: " + ex.Message;
                ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
            }


            // Close the modal and refresh the GridView
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal", "$('#scheduleModal').modal('hide');", true);

            
            // Optionally, call a method to refresh the GridView
        }

        //adding Sched
        protected void DeployBTNclk(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            try
            {
                // Retrieve values from the form controls
                //string roomNumber = RRoomNumberTB.Text;
                string roomNumber = add_Dropdown_room.SelectedValue;
                string section = RSectionTB.Text;
                string courseCode = RCourseCodeTB.Text;
                string professor = RProfTB.Text;
                string remarks = RRemarksTB.Text;
                //string buildingId = ADD_DropDownList1.SelectedValue;
                string faculty = RFacultyDL.SelectedValue;//WILL BE USED FOR BOOKING LATER
                DateTime startDate = DateTime.Parse(SelectDateTB.Text);
                DateTime endDate = DateTime.Parse(EndDateTB.Text);
                string startTime = RTimeStart.SelectedItem.Text;
                string endTime = RTimeEnd.SelectedItem.Text;

                get_ID add_dbHelper = new get_ID();

                int buildID = int.Parse(ADD_DropDownList1.SelectedValue);

                var add_Ids = add_dbHelper.CheckAndInsertValues(connection, roomNumber, section, courseCode, professor, true, buildID);

                int buildingID = add_dbHelper.GetOrInsertBuilding(connection, ADD_DropDownList1.SelectedValue);
                int roomID = add_Ids.roomID;
                int sectionID = add_Ids.sectionID;
                int courseID = add_Ids.courseID;
                int instructorID = add_Ids.instructorID;

                DateTime parsedStartTime = DateTime.ParseExact(startTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

                DateTime parsedEndTime = DateTime.ParseExact(endTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

                // Loop through each date between startDate and endDate
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // Get the day of the week and corresponding DayID
                    string dayName = date.DayOfWeek.ToString().ToUpper();
                    int dayID = add_dbHelper.GetDayID(dayName);
                    //int dayID = GetDayID(dayName);

                    // Construct the SQL insert query
                    string insertQuery = @"
                    INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, DayID, StartTime, EndTime, StartDate, EndDate, Remarks, BuildingID)
                    VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @DayID, @StartTime, @EndTime, @StartDate, @EndDate, @Remarks, @BuildingID)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("@RoomID", roomID);
                        cmd.Parameters.AddWithValue("@SectionID", sectionID);
                        cmd.Parameters.AddWithValue("@CourseID", courseID);
                        cmd.Parameters.AddWithValue("@InstructorID", instructorID);
                        cmd.Parameters.AddWithValue("@DayID", dayID);
                        cmd.Parameters.AddWithValue("@StartTime", startTimeOfDay);
                        cmd.Parameters.AddWithValue("@EndTime", endTimeOfDay);
                        cmd.Parameters.AddWithValue("@StartDate", date);
                        cmd.Parameters.AddWithValue("@EndDate", date); // Assuming the same date for start and end in this context
                        cmd.Parameters.AddWithValue("@Remarks", remarks);
                        cmd.Parameters.AddWithValue("@BuildingID", buildingID);

                        // Execute the insert command
                        cmd.ExecuteNonQuery();
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
                //will update the contents of this:
                Dropdown_Datas(null, EventArgs.Empty);
                BindScheduleData(null, EventArgs.Empty);

                // Close the connection
                connection.Close();
            }
        }
        //for edit button
        protected void Edit_BTNclk(object sender, EventArgs e)
        {
            try
            {
                // Debug: Log entry to method
                Response.Write("Edit_BTNclk method triggered.<br/>");

                // Get the ScheduleID from the hidden field
                string scheduleID = HiddenScheduleID.Value;
                Response.Write("ScheduleID: " + scheduleID + "<br/>");

                // Get the selected room ID
                string roomName = Edit_roomDrodown.SelectedValue;
                Response.Write("RoomName: " + roomName + "<br/>");

                // Get the selected date
                DateTime selectedDate = DateTime.Parse(Edit_Calendar_TextBox1.Text);
                Response.Write("SelectedDate: " + selectedDate.ToString("yyyy-MM-dd") + "<br/>");

                // Get Start time and End time
                string selectedStartTime = Edit_DropDownList1.SelectedItem.Text;
                string selectedEndTime = Edit_DropDownList2.SelectedItem.Text;
                Response.Write("StartTime: " + selectedStartTime + "<br/>");
                Response.Write("EndTime: " + selectedEndTime + "<br/>");

                DateTime parsedStartTime = DateTime.ParseExact(selectedStartTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;
                DateTime parsedEndTime = DateTime.ParseExact(selectedEndTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

                // Debug: Log parsed times
                Response.Write("Parsed StartTime: " + startTimeOfDay + "<br/>");
                Response.Write("Parsed EndTime: " + endTimeOfDay + "<br/>");

                // Open database connection
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        get_ID add_dbHelper = new get_ID();
                        int sectionID = add_dbHelper.GetOrInsertSection(connection, ESection.Text);
                        int instructorID = add_dbHelper.GetOrInsertInstructor(connection, EProf.Text);

                        // Debug: Log IDs
                        Response.Write("SectionID: " + sectionID + "<br/>");
                        Response.Write("InstructorID: " + instructorID + "<br/>");

                        // Update the schedule in the database
                        string updateQuery = "UPDATE Schedule SET StartTime = @StartTime, EndTime = @EndTime, StartDate = @SelectedDate, SectionID = @SectionID, InstructorID = @InstructorID WHERE ScheduleID = @ScheduleID";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ScheduleID", scheduleID);
                            command.Parameters.AddWithValue("@RoomID", roomName);
                            command.Parameters.AddWithValue("@StartTime", startTimeOfDay);
                            command.Parameters.AddWithValue("@EndTime", endTimeOfDay);
                            command.Parameters.AddWithValue("@SelectedDate", selectedDate);
                            command.Parameters.AddWithValue("@SectionID", sectionID);
                            command.Parameters.AddWithValue("@InstructorID", instructorID);

                            int rowsAffected = command.ExecuteNonQuery();

                            // Debug: Log execution result
                            Response.Write("Rows Affected: " + rowsAffected + "<br/>");

                            if (rowsAffected > 0)
                            {
                                // Update successful
                                RCloseBtn_Click(sender, e);
                                Response.Write("Update successful");
                            }
                            else
                            {
                                // No rows were affected
                                Response.Write("No rows were affected. The record might have been deleted by another user.");
                            }
                        }
                    }
                    else
                    {
                        Response.Write("Connection is not open.");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
                // Optionally log the exception for troubleshooting
            }
        }
        protected void Match_Schedule(object sender, EventArgs e)
        {
            // Get the selected room ID
            string roomName = Edit_roomDrodown.SelectedValue;

            //string selectedDate = Edit_Calendar_TextBox1.Text;
            DateTime selectedDate = DateTime.Parse(Edit_Calendar_TextBox1.Text);
            string selectedStartTime = Edit_DropDownList1.SelectedItem.Text;
            string selectedEndTime = Edit_DropDownList2.SelectedItem.Text;

            DateTime parsedStartTime = DateTime.ParseExact(selectedStartTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
            TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

            DateTime parsedEndTime = DateTime.ParseExact(selectedEndTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
            TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                // Retrieve the existing schedule from the database
                string query = @"SELECT
                            s.ScheduleID, s.StartTime, s.EndTime, s.StartDate,
                            sec.SectionName, ins.InstructorName
                         FROM Schedule s
                         INNER JOIN Sections sec ON s.SectionID = sec.SectionID
                         INNER JOIN Instructors ins ON s.InstructorID = ins.InstructorID
                         WHERE s.RoomID = @RoomID
                           AND @SelectedDate BETWEEN s.StartDate AND s.EndDate
                           AND s.StartTime = @StartTime
                           AND s.EndTime = @EndTime";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //SqlCommand cmd = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@RoomID", roomName);
                    command.Parameters.AddWithValue("@SelectedDate", selectedDate);
                    command.Parameters.AddWithValue("@StartTime", startTimeOfDay);
                    command.Parameters.AddWithValue("@EndTime", endTimeOfDay);

                    //conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Populate fields with existing data
                        Edit_DropDownList1.SelectedValue = reader["StartTime"].ToString();
                        Edit_DropDownList2.SelectedValue = reader["EndTime"].ToString();
                        Edit_Calendar_TextBox1.Text = Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd");
                        ESection.Text = reader["SectionName"].ToString();
                        EProf.Text = reader["InstructorName"].ToString();

                        // Store ScheduleID for later use

                        int scheduleID = Convert.ToInt32(reader["ScheduleID"]);
                        HiddenScheduleID.Value = scheduleID.ToString();

                        // Show SAVE button, hide RSaveChangesBtn
                        MatchSchedbtn.Visible = false;
                        RSaveChangesBtn.Visible = true;
                    }
                    else
                    {
                        Response.Write("No record found");

                        // Hide SAVE button, show RSaveChangesBtn
                        MatchSchedbtn.Visible = true;
                        RSaveChangesBtn.Visible = false;
                    }
                }
            }
        }

        //fill all dropdown list
        protected void Dropdown_Datas(object sender, EventArgs e)
        {
            DropdownFiller filler = new DropdownFiller();

            // Populate Room dropdowns
            filler.PopulateRooms(DropDownList2);
            filler.PopulateRooms(Edit_roomDrodown);
            filler.PopulateRooms(add_Dropdown_room);

            // Populate Building dropdowns
            filler.PopulateBuildings(upload_DropDownList1);
            filler.PopulateBuildings(ADD_DropDownList1);

            //populate uploaded scheds for dropdown
            filler.PopulateSchedule(uploadSchedsDL);
            uploadSchedsDL.Items.Insert(0, new ListItem("Select Uploaded File", "0"));
        }

        //closing functions
        protected void RCloseBtn_Click(object sender, EventArgs e)
        {
            // Reset all form fields to their default states
            Edit_roomDrodown.SelectedIndex = -1;
            Edit_DropDownList1.SelectedIndex = -1;
            Edit_DropDownList2.SelectedIndex = -1;
            Edit_Calendar_TextBox1.Text = string.Empty;
            ESection.Text = string.Empty;
            EProf.Text = string.Empty;

            // Reset visibility of buttons
            MatchSchedbtn.Visible = true;
            RSaveChangesBtn.Visible = false;

            // Keep the modal open
            //ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript", "keepModalOpen();", true);
        }
       
    }
}