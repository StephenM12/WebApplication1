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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                if (!IsPostBack)
                {
                    dropdown_Data(sender, e);
                    room_dropdown_Data(sender, e);

                    BindScheduleData(sender, e);
                }
            }
        }

        protected void Upload_File(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contentType = FileUpload1.PostedFile.ContentType;

            string uploader = user_Identity.user_FName + user_Identity.user_LName; // Replace with actual uploader name
            DateTime uploadDate = DateTime.Now;

            if (FileUpload1.HasFile)
            {
                if (Calendar1.SelectedDate != DateTime.MinValue && Calendar2.SelectedDate != DateTime.MinValue)
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
                                    get_ID getDay = new get_ID();
                                    int dayID = getDay.GetDayID(day);

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

                                            ScheduleRow newRow = new ScheduleRow
                                            {
                                                RoomID = ids.roomID == -1 ? (object)DBNull.Value : ids.roomID,
                                                SectionID = ids.sectionID == -1 ? (object)DBNull.Value : ids.sectionID,
                                                CourseID = ids.courseID == -1 ? (object)DBNull.Value : ids.courseID,
                                                InstructorID = ids.instructorID == -1 ? (object)DBNull.Value : ids.instructorID,
                                                Day = dayID,
                                                StartTime = timeOfDay,
                                                EndTime = END_timeOfDay,
                                                StartDate = Calendar1.SelectedDate,
                                                EndDate = Calendar2.SelectedDate,
                                                Remarks = null // Set remarks to null
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
            dataTable.Columns.Add("StartTime", typeof(TimeSpan)); // Adjusted to TimeSpan
            dataTable.Columns.Add("EndTime", typeof(TimeSpan)); // Adjusted to TimeSpan
            dataTable.Columns.Add("StartDate", typeof(DateTime));
            dataTable.Columns.Add("EndDate", typeof(DateTime));
            dataTable.Columns.Add("Remarks", typeof(string));

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
                int buildingId = int.Parse(SelectBuildingDL.SelectedValue);
                string faculty = RFacultyDL.SelectedValue;//WILL BE USED FOR BOOKING LATER
                DateTime startDate = DateTime.Parse(SelectDateTB.Text);
                DateTime endDate = DateTime.Parse(EndDateTB.Text);
                string startTime = RTimeStart.SelectedItem.Text;
                string endTime = RTimeEnd.SelectedItem.Text;

                get_ID add_dbHelper = new get_ID();
                var add_Ids = add_dbHelper.CheckAndInsertValues(connection, roomNumber, section, courseCode, professor);

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

                    get_ID getDay_ = new get_ID();
                    int dayID = getDay_.GetDayID(dayName);

                    // Construct the SQL insert query
                    string insertQuery = @"
                    INSERT INTO Schedule (RoomID, SectionID, CourseID, InstructorID, DayID, StartTime, EndTime, StartDate, EndDate, Remarks)
                    VALUES (@RoomID, @SectionID, @CourseID, @InstructorID, @DayID, @StartTime, @EndTime, @StartDate, @EndDate, @Remarks)";

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

        protected void dropdown_Data(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                //Dropdown datas from sql
                SqlCommand cmd = new SqlCommand("SELECT UploadID, FileName FROM upload_SchedsTBL", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                // Bind the data to the dropdown list
                DropDownList1.DataTextField = "FileName"; // Column name to display
                DropDownList1.DataValueField = "UploadID"; // Column name to use as value
                DropDownList1.DataSource = reader;
                DropDownList1.DataBind();
                reader.Close();
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
                reader.Close();
            }
        }

        //protected void Bind_Uploaded_GridView(object sender, EventArgs e)
        //{
        //    //string selected_ID = DropDownList1.SelectedValue;

        //    //try
        //    //{
        //    //    // Open database connection
        //    //    SqlConnection connection = dbConnection.GetConnection();

        //    //    if (connection.State == System.Data.ConnectionState.Open)
        //    //    {
        //    //        SqlCommand selectCommand = new SqlCommand("SELECT Data FROM scheduleTBL WHERE ID = @File_ID", connection);
        //    //        selectCommand.Parameters.AddWithValue("@File_ID", selected_ID);

        //    //        byte[] excelData = (byte[])selectCommand.ExecuteScalar();
        //    //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    //        using (MemoryStream stream = new MemoryStream(excelData))
        //    //        {
        //    //            using (ExcelPackage package = new ExcelPackage(stream))
        //    //            {
        //    //                int worksheetIndex = 1; // Example index

        //    //                if (worksheetIndex <= package.Workbook.Worksheets.Count)
        //    //                {
        //    //                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

        //    //                    int startRow = 6; // Starting row index
        //    //                    int endRow = 15;
        //    //                    int startColumn = 1; // Starting column index (A)
        //    //                    int endColumn = 8; // Ending column index (H)

        //    //                    // Create a DataTable to store the extracted data
        //    //                    DataTable dataTable = new DataTable();

        //    //                    // Add columns to the DataTable based on the number of columns in the range
        //    //                    for (int col = startColumn; col <= endColumn; col++)
        //    //                    {
        //    //                        dataTable.Columns.Add("Column " + col.ToString()); // You can customize column names here
        //    //                    }

        //    //                    // Iterate over each row in the range
        //    //                    for (int row = startRow; row <= endRow; row++)
        //    //                    {
        //    //                        // Create a new DataRow to store the values of the current row
        //    //                        DataRow dataRow = dataTable.NewRow();

        //    //                        // Iterate over each column in the range
        //    //                        for (int col = startColumn; col <= endColumn; col++)
        //    //                        {
        //    //                            // Get the value of the current cell
        //    //                            object cellValue = worksheet.Cells[row, col].Value;

        //    //                            // Add the cell value to the DataRow
        //    //                            dataRow[col - startColumn] = cellValue != null ? cellValue.ToString() : ""; // Convert cell value to string
        //    //                        }

        //    //                        // Add the DataRow to the DataTable
        //    //                        dataTable.Rows.Add(dataRow);
        //    //                    }

        //    //                    GridView1.DataSource = dataTable;
        //    //                    GridView1.DataBind();
        //    //                }
        //    //            }

        //    //            connection.Close();
        //    //        }
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //    Response.Write("Failed to Show Table");
        //    //}
        //}

        protected void BindScheduleData(object sender, EventArgs e)
        {
            string selected_ID_ROOM = DropDownList2.SelectedValue;
            DateTime selectedDate = Calendar1.SelectedDate;

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
                GROUP BY s.StartTime, s.EndTime
                ORDER BY s.StartTime;";

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);

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
    }
}