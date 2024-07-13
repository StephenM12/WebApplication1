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

        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                if (!IsPostBack)
                {
                    //dropdown_Data(sender, e);
                    room_dropdown_Data(sender, e);
                    edit_Roomdropdown(sender, e);

                    if (DropDownList2.Items.Count > 0)
                    {
                        DropDownList2.SelectedIndex = 0;
                        BindScheduleData(sender, e);
                    }
                }
            }
        }

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
                                    string query = @"INSERT INTO upload_SchedsTBL (FileName, ContentType, Data, Uploader, UploadDate) VALUES (@FileName, @ContentType, @FileData, @Uploader, @UploadDate)";

                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        try
                                        {
                                            command.Parameters.AddWithValue("@FileName", fileName);
                                            command.Parameters.AddWithValue("@ContentType", contentType);
                                            command.Parameters.AddWithValue("@FileData", bytes);
                                            command.Parameters.AddWithValue("@Uploader", uploader);
                                            command.Parameters.AddWithValue("@UploadDate", uploadDate);

                                            command.ExecuteNonQuery();

                                            ReadExcelData(excelStream);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle the error here
                                            Console.WriteLine("An error occurred while inserting into upload_SchedsTBL: " + ex.Message);
                                            // Optionally, you may choose to rollback any changes or perform other cleanup actions.
                                        }
                                    }
                                    //ReadExcelData(excelStream);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it accordingly
                        Console.WriteLine("An error occurred during the file upload process: " + ex.Message);
                        Response.Write("An error occurred during the file upload process. Please try again." + ex.Message);
                    }
                }
                else
                {
                    // Calendar1 does not have a selected date
                    Response.Write("Pls Select the Start and End date from the Calendars");
                }
            }
            else
            {
                // Handle the case when no file is uploaded
                Response.Write("Please upload a file.");
            }

            // Calendar1 has a selected date
        }

        public void ReadExcelData(Stream excelStream)
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
                                    string day = worksheet.Cells[5, col].Text; // Assuming row 3 contains day names

                                    // Get DayID from days_of_week table based on day name
                                    int dayID = GetDayID(day);

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

                                            get_ID dbHelper = new get_ID();
                                            var ids = dbHelper.CheckAndInsertValues(connection, currentRoom, section, courseCode, instructor);

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
                                                BuildingID = buildingID
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

                            // Perform the bulk copy
                            bulkCopy.WriteToServer(scheduleDataTable);

                            Response.Write("File uploaded successfully.");
                        }
                    }

                    //will update the contents of this:
                    room_dropdown_Data(null, EventArgs.Empty);
                    BindScheduleData(null, EventArgs.Empty);
                }
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
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        protected void DeployBTNclk(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            

            try
            {
                // Retrieve values from the form controls
                string roomNumber = RRoomNumberTB.Text;
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
                var add_Ids = add_dbHelper.CheckAndInsertValues(connection, roomNumber, section, courseCode, professor);


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
                    string dayName = date.DayOfWeek.ToString();
                    int dayID = GetDayID(dayName);

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
                room_dropdown_Data(null, EventArgs.Empty);
                BindScheduleData(null, EventArgs.Empty);

                // Close the connection
                connection.Close();
            }
        }

        private int GetDayID(string dayName)
        {
            switch (dayName)
            {
                case "Sunday":
                    return 1;

                case "Monday":
                    return 2;

                case "Tuesday":
                    return 3;

                case "Wednesday":
                    return 4;

                case "Thursday":
                    return 5;

                case "Friday":
                    return 6;

                case "Saturday":
                    return 7;

                default:
                    throw new ArgumentException("Invalid day name");
            }
        }

        protected void room_dropdown_Data(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                //Dropdown datas from sql
                SqlCommand cmd = new SqlCommand("SELECT RoomID, RoomName FROM Rooms", connection);
                SqlDataReader reader = cmd.ExecuteReader();
               

                // Bind the data to the dropdown list
                DropDownList2.DataTextField = "RoomName"; // Column name to display
                DropDownList2.DataValueField = "RoomID"; // Column name to use as value
                DropDownList2.DataSource = reader;
                DropDownList2.DataBind();

                
            }
        }
        protected void edit_Roomdropdown(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                //Dropdown datas from sql
                SqlCommand cmd = new SqlCommand("SELECT RoomID, RoomName FROM Rooms", connection);
                SqlDataReader reader = cmd.ExecuteReader();


                // Bind the data to the dropdown list
                Edit_roomDrodown.DataTextField = "RoomName"; // Column name to display
                Edit_roomDrodown.DataValueField = "RoomID"; // Column name to use as value
                Edit_roomDrodown.DataSource = reader;
                Edit_roomDrodown.DataBind();


            }
        }

        protected void BindScheduleData(object sender, EventArgs e)
        {
            string selected_ID_ROOM = DropDownList2.SelectedValue;
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

            string query = @"
            SELECT
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
            GROUP BY s.StartTime, s.EndTime
            ORDER BY s.StartTime;";

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
                    cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            BindScheduleData(sender, e);
        }

        //for edit modal:
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

        //protected void Edit_BTNclk(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Get the ScheduleID from the hidden field
        //        string scheduleID = HiddenScheduleID.Value;

        //        // Get the selected room ID
        //        string roomName = Edit_roomDrodown.SelectedValue;

        //        // Get the selected date
        //        DateTime selectedDate = DateTime.Parse(Edit_Calendar_TextBox1.Text);

        //        // Get Start time and End time
        //        string selectedStartTime = Edit_DropDownList1.SelectedItem.Text;
        //        string selectedEndTime = Edit_DropDownList2.SelectedItem.Text;

        //        DateTime parsedStartTime = DateTime.ParseExact(selectedStartTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
        //        TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

        //        DateTime parsedEndTime = DateTime.ParseExact(selectedEndTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
        //        TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;

        //        // Open database connection
        //        using (SqlConnection connection = dbConnection.GetConnection())
        //        {
        //            if (connection.State == System.Data.ConnectionState.Open)
        //            {
        //                get_ID add_dbHelper = new get_ID();
        //                int sectionID = add_dbHelper.GetOrInsertSection(connection, ESection.Text);
        //                int instructorID = add_dbHelper.GetOrInsertInstructor(connection, EProf.Text);

        //                // Update the schedule in the database
        //                string updateQuery = "UPDATE Schedule SET StartTime = @StartTime, EndTime = @EndTime, StartDate = @SelectedDate, SectionID = @SectionID, InstructorID = @InstructorID WHERE ScheduleID = @ScheduleID";

        //                using (SqlCommand command = new SqlCommand(updateQuery, connection))
        //                {
        //                    command.Parameters.AddWithValue("@ScheduleID", scheduleID);
        //                    command.Parameters.AddWithValue("@RoomID", roomName);
        //                    command.Parameters.AddWithValue("@StartTime", startTimeOfDay);
        //                    command.Parameters.AddWithValue("@EndTime", endTimeOfDay);
        //                    command.Parameters.AddWithValue("@SelectedDate", selectedDate);
        //                    command.Parameters.AddWithValue("@SectionID", sectionID);
        //                    command.Parameters.AddWithValue("@InstructorID", instructorID);

        //                    command.ExecuteNonQuery();

        //                    //if (rowsAffected > 0)
        //                    //{
        //                    //    // Update successful, add your conditional logic here
        //                    //    RCloseBtn_Click(sender, e);
        //                    //    Response.Write("Update successful");
        //                    //}
        //                    //else
        //                    //{
        //                    //    // Update failed or no rows were affected
        //                    //    Response.Write("Update failed or no rows were affected");
        //                    //}
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("Connection is not open.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("Error: " + ex.Message);
        //        // Optionally log the exception for troubleshooting
        //    }
        //}

        //protected void Edit_BTNclk(object sender, EventArgs e)
        //{
        //    // Get the new values from the fields

        //    // Get the ScheduleID from the hidden field
        //    //int scheduleID = int.Parse(HiddenScheduleID.Value);
        //    string scheduleID = HiddenScheduleID.Value;

        //    // Get the selected room ID
        //    string roomName = Edit_roomDrodown.SelectedValue;


        //    //string selectedDate = Edit_Calendar_TextBox1.Text;
        //    DateTime selectedDate = DateTime.Parse(Edit_Calendar_TextBox1.Text);


        //    //get Start time and End time
        //    string selectedStartTime = Edit_DropDownList1.SelectedItem.Text;
        //    string selectedEndTime = Edit_DropDownList2.SelectedItem.Text;


        //    DateTime parsedStartTime = DateTime.ParseExact(selectedStartTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
        //    TimeSpan startTimeOfDay = parsedStartTime.TimeOfDay;

        //    DateTime parsedEndTime = DateTime.ParseExact(selectedEndTime.Trim(), "h:mmtt", CultureInfo.InvariantCulture);
        //    TimeSpan endTimeOfDay = parsedEndTime.TimeOfDay;



        //    // Open database connection
        //    SqlConnection connection = dbConnection.GetConnection();
        //    if (connection.State == System.Data.ConnectionState.Open)
        //    {
        //        get_ID add_dbHelper = new get_ID();
        //        int sectionID = add_dbHelper.GetOrInsertSection(connection, ESection.Text);
        //        int instructorID = add_dbHelper.GetOrInsertInstructor(connection, EProf.Text);


        //        // Update the schedule in the database
        //        //string updateQuery = "UPDATE Schedule SET StartTime = @StartTime, EndTime = @EndTime, StartDate = @SelectedDate, SectionID = @SectionID, InstructorID = @InstructorID WHERE RoomID = @RoomID";
        //        string updateQuery = "UPDATE Schedule SET StartTime = @StartTime, EndTime = @EndTime, StartDate = @SelectedDate, SectionID = @SectionID, InstructorID = @InstructorID WHERE ScheduleID = @ScheduleID";

        //        using (SqlCommand command = new SqlCommand(updateQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@ScheduleID", scheduleID);
        //            command.Parameters.AddWithValue("@RoomID", roomName);
        //            command.Parameters.AddWithValue("@StartTime", startTimeOfDay);
        //            command.Parameters.AddWithValue("@EndTime", endTimeOfDay);
        //            command.Parameters.AddWithValue("@SelectedDate", selectedDate);
        //            command.Parameters.AddWithValue("@SectionID", sectionID);
        //            command.Parameters.AddWithValue("@InstructorID", instructorID);


        //            int rowsAffected = command.ExecuteNonQuery();

        //            if (rowsAffected > 0)
        //            {
        //                // Update successful, add your conditional logic here
        //                RCloseBtn_Click(sender,e);
        //                Response.Write("Update successful");

        //            }
        //            else
        //            {
        //                // Update failed or no rows were affected
        //                Response.Write("Update failed or no rows were affected");
        //            }
        //        }


        //    }

        //}

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