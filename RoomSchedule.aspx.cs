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
using System.Threading.Tasks;
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
        }

        private (int roomID, int sectionID, int courseID, int instructorID) CheckAndInsertValues(SqlConnection connection, string room, string section, string course, string instructor)
        {
            int roomID = GetOrInsertRoom(connection, room);
            int sectionID = GetOrInsertSection(connection, section);
            int courseID = GetOrInsertCourse(connection, course);
            int instructorID = GetOrInsertInstructor(connection, instructor);

            return (roomID, sectionID, courseID, instructorID);
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
                                    string query = @"INSERT INTO upload_SchedsTBL (FileName, ContentType, Data, Uploader, Upload_Date) VALUES (@FileName, @ContentType, @FileData, @Uploader, @UploadDate)";

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
                                    int dayID = GetDayID(day, connection);

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
                                            var ids = CheckAndInsertValues(connection, currentRoom, section, courseCode, instructor);

                                            //// Parse the string using ParseExact with custom format specifier
                                            //DateTime parsedTime = DateTime.ParseExact(timeParts[0], "h:mmtt", CultureInfo.InvariantCulture);
                                            //TimeSpan timeOfDay = parsedTime.TimeOfDay;

                                            //// Parse the string using ParseExact with custom format specifier
                                            //DateTime END_parsedTime = DateTime.ParseExact(timeParts[1], "h:mmtt", CultureInfo.InvariantCulture);
                                            //TimeSpan END_timeOfDay = END_parsedTime.TimeOfDay;

                                            DateTime parsedTime = DateTime.ParseExact(timeParts[0].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                                            TimeSpan timeOfDay = parsedTime.TimeOfDay;

                                            DateTime END_parsedTime = DateTime.ParseExact(timeParts[1].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                                            TimeSpan END_timeOfDay = END_parsedTime.TimeOfDay;

                                            ScheduleRow newRow = new ScheduleRow
                                            {
                                                RoomID = ids.roomID == -1 ? (object)DBNull.Value : ids.roomID,
                                                SectionID = ids.sectionID == -1 ? (object)DBNull.Value : ids.sectionID,
                                                CourseID = ids.courseID == -1 ? (object)DBNull.Value : ids.courseID,
                                                InstructorID = ids.instructorID == -1 ? (object)DBNull.Value : ids.instructorID,
                                                Day = dayID,
                                                //StartTime = TimeSpan.Parse(time.Split('-')[0].Replace("AM", "").Replace("PM", "").Trim()),
                                                //EndTime = TimeSpan.Parse(time.Split('-')[1].Replace("AM", "").Replace("PM", "").Trim()),

                                                StartTime = timeOfDay,
                                                EndTime = END_timeOfDay,

                                                StartDate = Calendar1.SelectedDate,
                                                EndDate = Calendar2.SelectedDate
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

                            // Perform the bulk copy
                            bulkCopy.WriteToServer(scheduleDataTable);
                        }
                    }
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
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }


        protected void DeployBTNclk(object sender, EventArgs e)
        {
            string courseCode = RCourseCodeTB.Text; //course code
            //string sec = RSectionTB.Text; //section
            string prof = RProfTB.Text; //prof/instructor
            string room = RRoomNumberTB.Text; //room number
            string selectedCollege = RFacultyDL.SelectedItem.Text; //college value
            string selectedTimerealValue = RTimeDL.SelectedItem.Text; //Selected Time
            string selectedTime = RTimeDL.SelectedValue; //Selected Time ID
            string building = SelectBuildingDL.SelectedItem.Text;

            ////calendar code:
            var dateStr = SelectDateTB.Text; //YYYY-MM-DD
            DateTime date; //attempts to parse the dateStr string into a DateTime object
            DateTime.TryParse(dateStr, out date);
            string dayOfWeekString = date.ToString("dddd");//print Monday-Sunday


        }

        private int GetOrInsertRoom(SqlConnection connection, string room)
        {
            // Example logic for retrieving or inserting a room
            string query = "SELECT RoomID FROM Rooms WHERE RoomName = @RoomName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomName", room);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Rooms (RoomName) OUTPUT INSERTED.RoomID VALUES (@RoomName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@RoomName", room);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetOrInsertSection(SqlConnection connection, string section)
        {
            // Example logic for retrieving or inserting a section
            string query = "SELECT SectionID FROM Sections WHERE SectionName = @SectionName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SectionName", section);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Sections (SectionName) OUTPUT INSERTED.SectionID VALUES (@SectionName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@SectionName", section);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetOrInsertCourse(SqlConnection connection, string course)
        {
            // Example logic for retrieving or inserting a course
            string query = "SELECT CourseID FROM Courses WHERE CourseCode = @CourseCode";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseCode", course);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Courses (CourseCode) OUTPUT INSERTED.CourseID VALUES (@CourseCode)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@CourseCode", course);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetOrInsertInstructor(SqlConnection connection, string instructor)
        {
            // Example logic for retrieving or inserting an instructor
            string query = "SELECT InstructorID FROM Instructors WHERE InstructorName = @InstructorName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InstructorName", instructor);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Instructors (InstructorName) OUTPUT INSERTED.InstructorID VALUES (@InstructorName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@InstructorName", instructor);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetDayID(string dayName, SqlConnection connection)
        {
            int dayID = 0;
            string query = "SELECT day_id FROM days_of_week WHERE day = @DayName;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DayName", dayName);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    dayID = Convert.ToInt32(result);
                }
                // Handle case where dayName is not found in days_of_week table
            }

            return dayID;
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

        protected void Bind_Uploaded_GridView(object sender, EventArgs e)
        {
            //string selected_ID = DropDownList1.SelectedValue;

            //try
            //{
            //    // Open database connection
            //    SqlConnection connection = dbConnection.GetConnection();

            //    if (connection.State == System.Data.ConnectionState.Open)
            //    {
            //        SqlCommand selectCommand = new SqlCommand("SELECT Data FROM scheduleTBL WHERE ID = @File_ID", connection);
            //        selectCommand.Parameters.AddWithValue("@File_ID", selected_ID);

            //        byte[] excelData = (byte[])selectCommand.ExecuteScalar();
            //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //        using (MemoryStream stream = new MemoryStream(excelData))
            //        {
            //            using (ExcelPackage package = new ExcelPackage(stream))
            //            {
            //                int worksheetIndex = 1; // Example index

            //                if (worksheetIndex <= package.Workbook.Worksheets.Count)
            //                {
            //                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            //                    int startRow = 6; // Starting row index
            //                    int endRow = 15;
            //                    int startColumn = 1; // Starting column index (A)
            //                    int endColumn = 8; // Ending column index (H)

            //                    // Create a DataTable to store the extracted data
            //                    DataTable dataTable = new DataTable();

            //                    // Add columns to the DataTable based on the number of columns in the range
            //                    for (int col = startColumn; col <= endColumn; col++)
            //                    {
            //                        dataTable.Columns.Add("Column " + col.ToString()); // You can customize column names here
            //                    }

            //                    // Iterate over each row in the range
            //                    for (int row = startRow; row <= endRow; row++)
            //                    {
            //                        // Create a new DataRow to store the values of the current row
            //                        DataRow dataRow = dataTable.NewRow();

            //                        // Iterate over each column in the range
            //                        for (int col = startColumn; col <= endColumn; col++)
            //                        {
            //                            // Get the value of the current cell
            //                            object cellValue = worksheet.Cells[row, col].Value;

            //                            // Add the cell value to the DataRow
            //                            dataRow[col - startColumn] = cellValue != null ? cellValue.ToString() : ""; // Convert cell value to string
            //                        }

            //                        // Add the DataRow to the DataTable
            //                        dataTable.Rows.Add(dataRow);
            //                    }

            //                    GridView1.DataSource = dataTable;
            //                    GridView1.DataBind();
            //                }
            //            }

            //            connection.Close();
            //        }
            //    }
            //}
            //catch
            //{
            //    Response.Write("Failed to Show Table");
            //}
        }

        protected void BindScheduleData(/*int? roomId*/object sender, EventArgs e)
        {
            string selected_ID_ROOM = DropDownList2.SelectedValue;

            string query = @"
                SELECT
                CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
                MAX(CASE WHEN s.DayID = 1 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Monday],
                MAX(CASE WHEN s.DayID = 2 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Tuesday],
                MAX(CASE WHEN s.DayID = 3 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Wednesday],
                MAX(CASE WHEN s.DayID = 4 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Thursday],
                MAX(CASE WHEN s.DayID = 5 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Friday],
                MAX(CASE WHEN s.DayID = 6 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Saturday],
                MAX(CASE WHEN s.DayID = 7 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Sunday]
                FROM Schedule s
                JOIN Sections sec ON s.SectionID = sec.SectionID
                JOIN Courses c ON s.CourseID = c.CourseID
                JOIN Instructors i ON s.InstructorID = i.InstructorID
                WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)
                GROUP BY s.StartTime, s.EndTime
                ORDER BY s.StartTime; ";

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    //cmd.Parameters.AddWithValue("@RoomID", (object)roomId ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);

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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Replace(".", "</br>");
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Replace(".", "</br>");
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace(".", "</br>");
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.Replace(".", "</br>");
                e.Row.Cells[5].Text = e.Row.Cells[5].Text.Replace(".", "</br>");
                e.Row.Cells[6].Text = e.Row.Cells[6].Text.Replace(".", "</br>");
                e.Row.Cells[7].Text = e.Row.Cells[7].Text.Replace(".", "</br>");
            }
        }

        


        

        

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
        }
    }
}