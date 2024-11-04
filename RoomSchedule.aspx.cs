//for excel package
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;

//sql connection:
using System.Data.SqlClient;

//for testing
using System.Diagnostics;

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
            //for specific date
            public DateTime ScheduleDate { get; set; }
            public string Remarks { get; set; }
            public object BuildingID { get; set; }
            public int fileID { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ModalPopup.RegisterModalHtml(Page);

                Dropdown_Datas(sender, e);

                //make this visible if u need to use edit button/add
                REditBtn.Visible = false;
                RAddSchedBtn.Visible = false;


                int userlevel = user_Identity.user_level;
                if (userlevel != 1 )
                {
                    btnUpdate.Visible = false;
                    btnEdit.Visible = false;
                    Button3.Visible = false;

                    REditBtn.Visible = false;
                    RUploadFileBtn.Visible = false;
                    Button2.Visible = false;
                }

                if (DropDownList2.Items.Count > 0)
                {
                    //DropDownList2.SelectedIndex = 0;
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
            if (FileUpload1.HasFile)
            {
                if (upload_DropDownList1.SelectedIndex != 0)
                {
                    try
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

                                        using (SqlConnection connection = dbConnection.GetConnection())
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

                                                    DropdownFiller filler = new DropdownFiller();
                                                    filler.PopulateSchedule(uploadSchedsDL);

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
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        string msg = "Error: " + ex.Message;
                        ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
                    }
                    finally
                    {
                        FileUpload1.Attributes.Clear();
                        upload_DropDownList1.SelectedIndex = 0;
                        calendar_TB1.Text = string.Empty;
                        calendar_TB2.Text = string.Empty;
                    }
                }
                else
                {
                    ModalPopup.ShowMessage(Page, "Pls select a building", "Alert!");
                }
            }
            else
            {
                // Handle the case when no file is uploaded
                //Response.Write("Please upload a file.");
                ModalPopup.ShowMessage_(this.Page, "Please upload a file.", "Alert!");
            }

            // Calendar1 has a selected date
        }
        //add building
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

                    Dropdown_Datas(sender, e);
                }
                catch (Exception ex)
                {
                    lblSuccessMessage.Text = "Error: " + ex.Message;
                    lblSuccessMessage.CssClass = "alert alert-danger";
                    lblSuccessMessage.Visible = true;
                }
            }
        }

        //orig
        //public void ReadExcelData(Stream excelStream, int fileID_)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = dbConnection.GetConnection())
        //        {
        //            using (ExcelPackage package = new ExcelPackage(excelStream))
        //            {
        //                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
        //                int rowCount = worksheet.Dimension.Rows;
        //                int colCount = worksheet.Dimension.Columns;

        //                string currentRoom = string.Empty;
        //                List<ScheduleRow> scheduleRows = new List<ScheduleRow>();

        //                // Track the first room inserted
        //                string firstRoomInserted = null;
        //                //track the first date
        //                string first_sdate = null;

        //                // Loop through each row in the Excel worksheet
        //                for (int row = 1; row <= rowCount; row++)
        //                {
        //                    string firstCellText = worksheet.Cells[row, 1].Text;

        //                    if (firstCellText.StartsWith("R") || firstCellText.StartsWith("E"))
        //                    {
        //                        currentRoom = firstCellText;

        //                        if (firstRoomInserted == null)
        //                        {
        //                            firstRoomInserted = currentRoom;

        //                            get_ID getRoomID = new get_ID();

        //                            int buildid = int.Parse(upload_DropDownList1.SelectedValue);
        //                            int firstRoomInserted_ID = getRoomID.GetOrInsertRoom(connection, firstRoomInserted, buildid, true);

        //                            DropdownFiller filler = new DropdownFiller();
        //                            filler.PopulateRooms(DropDownList2);

        //                            DropDownList2.SelectedValue = firstRoomInserted_ID.ToString();
        //                            uploadSchedsDL.SelectedValue = fileID_.ToString();
        //                        }
        //                    }
        //                    else if (firstCellText.Equals("Time", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        // Skip header row
        //                        continue;
        //                    }
        //                    else if (!string.IsNullOrEmpty(currentRoom) && !string.IsNullOrEmpty(firstCellText))
        //                    {
        //                        string time = worksheet.Cells[row, 1].Text;
        //                        string[] timeParts = time.Split('-');

        //                        for (int col = 2; col <= colCount; col++)
        //                        {
        //                            //hmmm try 4
        //                            string day = worksheet.Cells[5, col].Text.ToUpper(); // Assuming row 3 contains day names

        //                            // Get DayID from days_of_week table based on day name
        //                            get_ID getDay = new get_ID();
        //                            int dayID = getDay.GetDayID(day);
        //                            //int dayID = GetDayID(day);

        //                            string schedule = worksheet.Cells[row, col].Text;

        //                            if (!string.IsNullOrEmpty(schedule))
        //                            {
        //                                // Split the schedule into parts
        //                                string[] scheduleParts = schedule.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
        //                                if (scheduleParts.Length > 0)
        //                                {
        //                                    string courseCode = scheduleParts[0];
        //                                    string section = "";
        //                                    string instructor = "";

        //                                    if (scheduleParts.Length == 2 && scheduleParts[1].Contains(" "))
        //                                    {
        //                                        string[] sectionAndInstructor = scheduleParts[1].Split(' ');
        //                                        section = sectionAndInstructor[0];
        //                                        instructor = sectionAndInstructor[1];
        //                                    }
        //                                    else if (scheduleParts.Length >= 2 && !scheduleParts[1].Contains(" "))
        //                                    {
        //                                        section = scheduleParts[1];
        //                                    }
        //                                    else if (scheduleParts.Length >= 3 && !scheduleParts[2].Contains(" "))
        //                                    {
        //                                        section = scheduleParts[2];
        //                                    }
        //                                    //Response.Write($"Room: {currentRoom}, Section: {section}, CourseCode: {courseCode}, Instructor: {instructor}, Day: {dayID}, StartTime: {time.Split('-')[0]}, EndTime: {time.Split('-')[1]}, StartDate: {DateTime.Today.ToShortDateString()}, EndDate: {DateTime.Today.ToShortDateString()}<br />");
        //                                    // Create a new ScheduleRow object

        //                                    DateTime parsedTime = DateTime.ParseExact(timeParts[0].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
        //                                    TimeSpan timeOfDay = parsedTime.TimeOfDay;

        //                                    DateTime END_parsedTime = DateTime.ParseExact(timeParts[1].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
        //                                    TimeSpan END_timeOfDay = END_parsedTime.TimeOfDay;

        //                                    int buildID = int.Parse(upload_DropDownList1.SelectedValue);

        //                                    get_ID dbHelper = new get_ID();
        //                                    var ids = dbHelper.CheckAndInsertValues(connection, currentRoom, section, courseCode, instructor, buildID);

        //                                    //Get the selected date from the TextBox
        //                                    string selectedDate = calendar_TB1.Text;
        //                                    string selectedEndDate = calendar_TB2.Text;

        //                                    // Convert to DateTime directly
        //                                    DateTime Sdate = DateTime.Parse(selectedDate);
        //                                    DateTime Edate = DateTime.Parse(selectedEndDate);

        //                                    if (first_sdate == null)
        //                                    {
        //                                        first_sdate = selectedDate;
        //                                        DateTime first_sdate_ = DateTime.Parse(first_sdate);
        //                                        Calendar3.SelectedDate = first_sdate_;
        //                                    }

        //                                    //get_ID building id
        //                                    //var buildingID = dbHelper.GetOrInsertBuilding(connection, upload_DropDownList1.SelectedValue);

        //                                    ScheduleRow newRow = new ScheduleRow
        //                                    {
        //                                        RoomID = ids.roomID == -1 ? (object)DBNull.Value : ids.roomID,
        //                                        SectionID = ids.sectionID == -1 ? (object)DBNull.Value : ids.sectionID,
        //                                        CourseID = ids.courseID == -1 ? (object)DBNull.Value : ids.courseID,
        //                                        InstructorID = ids.instructorID == -1 ? (object)DBNull.Value : ids.instructorID,
        //                                        Day = dayID,
        //                                        StartTime = timeOfDay,
        //                                        EndTime = END_timeOfDay,
        //                                        StartDate = Sdate,
        //                                        EndDate = Edate,
        //                                        Remarks = null,
        //                                        BuildingID = buildID,
        //                                        fileID = fileID_
        //                                    };

        //                                    // Add the new row to the list
        //                                    scheduleRows.Add(newRow);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                // After collecting all schedule data, convert it to DataTable
        //                DataTable scheduleDataTable = ConvertToDataTable(scheduleRows);

        //                // Bulk insert into Schedule table using SqlBulkCopy
        //                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
        //                {
        //                    bulkCopy.DestinationTableName = "Schedule";

        //                    // Map DataTable columns to SQL Server table columns
        //                    bulkCopy.ColumnMappings.Add("RoomID", "RoomID");
        //                    bulkCopy.ColumnMappings.Add("SectionID", "SectionID");
        //                    bulkCopy.ColumnMappings.Add("CourseID", "CourseID");
        //                    bulkCopy.ColumnMappings.Add("InstructorID", "InstructorID");
        //                    bulkCopy.ColumnMappings.Add("Day", "DayID");
        //                    bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
        //                    bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
        //                    bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
        //                    bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
        //                    bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
        //                    bulkCopy.ColumnMappings.Add("BuildingID", "BuildingID");
        //                    bulkCopy.ColumnMappings.Add("fileID", "UploadID");

        //                    // Perform the bulk copy
        //                    bulkCopy.WriteToServer(scheduleDataTable);

        //                    firstRoomInserted = null;
        //                }
        //            }

        //            //will update the contents of this:
        //            Dropdown_Datas(null, EventArgs.Empty);
        //            BindScheduleData(null, EventArgs.Empty);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        ModalPopup.ShowMessage(Page, msg, "Alert!");
        //    }
        //    finally
        //    {
        //        FileUpload1.Attributes.Clear();

        //        upload_DropDownList1.SelectedIndex = 0;
        //        calendar_TB1.Text = string.Empty;
        //        calendar_TB2.Text = string.Empty;
        //    }
        //}

        //v2
        public void ReadExcelData(Stream excelStream, int fileID_)
        {
            try
            {
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    using (ExcelPackage package = new ExcelPackage(excelStream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        string currentRoom = string.Empty;
                        List<ScheduleRow> scheduleRows = new List<ScheduleRow>();

                        string firstRoomInserted = null;
                        string first_sdate = null;

                        // Loop through each row in the Excel worksheet
                        for (int row = 1; row <= rowCount; row++)
                        {
                            string firstCellText = worksheet.Cells[row, 1].Text;

                            if (firstCellText.StartsWith("R") || firstCellText.StartsWith("E"))
                            {
                                currentRoom = firstCellText;

                                if (firstRoomInserted == null)
                                {
                                    firstRoomInserted = currentRoom;

                                    get_ID getRoomID = new get_ID();

                                    int buildid = int.Parse(upload_DropDownList1.SelectedValue);
                                    int firstRoomInserted_ID = getRoomID.GetOrInsertRoom(connection, firstRoomInserted, buildid, true);

                                    DropdownFiller filler = new DropdownFiller();
                                    filler.PopulateRooms(DropDownList2);

                                    DropDownList2.SelectedValue = firstRoomInserted_ID.ToString();
                                    uploadSchedsDL.SelectedValue = fileID_.ToString();
                                }
                            }
                            else if (firstCellText.Equals("Time", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }
                            else if (!string.IsNullOrEmpty(currentRoom) && !string.IsNullOrEmpty(firstCellText))
                            {
                                string time = worksheet.Cells[row, 1].Text;
                                string[] timeParts = time.Split('-');

                                for (int col = 2; col <= colCount; col++)
                                {
                                    string day = worksheet.Cells[5, col].Text.ToUpper();

                                    get_ID getDay = new get_ID();
                                    int dayID = getDay.GetDayID(day);

                                    string schedule = worksheet.Cells[row, col].Text;

                                    if (!string.IsNullOrEmpty(schedule))
                                    {
                                        string[] scheduleParts = schedule.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
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

                                        DateTime parsedTime = DateTime.ParseExact(timeParts[0].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                                        TimeSpan timeOfDay = parsedTime.TimeOfDay;

                                        DateTime END_parsedTime = DateTime.ParseExact(timeParts[1].Trim(), "h:mmtt", CultureInfo.InvariantCulture);
                                        TimeSpan END_timeOfDay = END_parsedTime.TimeOfDay;

                                        int buildID = int.Parse(upload_DropDownList1.SelectedValue);

                                        get_ID dbHelper = new get_ID();
                                        var ids = dbHelper.CheckAndInsertValues(connection, currentRoom, section, courseCode, instructor, buildID);

                                        string selectedDate = calendar_TB1.Text;
                                        string selectedEndDate = calendar_TB2.Text;

                                        DateTime Sdate = DateTime.Parse(selectedDate);
                                        DateTime Edate = DateTime.Parse(selectedEndDate);

                                        if (first_sdate == null)
                                        {
                                            first_sdate = selectedDate;
                                            DateTime first_sdate_ = DateTime.Parse(first_sdate);
                                            Calendar3.SelectedDate = first_sdate_;
                                        }

                                        // Loop through each date between Sdate and Edate
                                        for (DateTime date = Sdate; date <= Edate; date = date.AddDays(1))
                                        {
                                            ScheduleRow newRow = new ScheduleRow
                                            {
                                                RoomID = ids.roomID == -1 ? (object)DBNull.Value : ids.roomID,
                                                SectionID = ids.sectionID == -1 ? (object)DBNull.Value : ids.sectionID,
                                                CourseID = ids.courseID == -1 ? (object)DBNull.Value : ids.courseID,
                                                InstructorID = ids.instructorID == -1 ? (object)DBNull.Value : ids.instructorID,
                                                Day = dayID,
                                                StartTime = timeOfDay,
                                                EndTime = END_timeOfDay,
                                                ScheduleDate = date,  // Inserting specific date here
                                                Remarks = null,
                                                BuildingID = buildID,
                                                fileID = fileID_
                                            };

                                            scheduleRows.Add(newRow);
                                        }
                                    }
                                }
                            }
                        }

                        // Convert list to DataTable and bulk insert
                        DataTable scheduleDataTable = ConvertToDataTable(scheduleRows);

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "Schedule";

                            bulkCopy.ColumnMappings.Add("RoomID", "RoomID");
                            bulkCopy.ColumnMappings.Add("SectionID", "SectionID");
                            bulkCopy.ColumnMappings.Add("CourseID", "CourseID");
                            bulkCopy.ColumnMappings.Add("InstructorID", "InstructorID");
                            bulkCopy.ColumnMappings.Add("Day", "DayID");
                            bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                            bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
                            bulkCopy.ColumnMappings.Add("ScheduleDate", "ScheduleDate"); // Changed from StartDate/EndDate to ScheduleDate
                            bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                            bulkCopy.ColumnMappings.Add("BuildingID", "BuildingID");
                            bulkCopy.ColumnMappings.Add("fileID", "UploadID");

                            bulkCopy.WriteToServer(scheduleDataTable);
                        }
                    }

                    Dropdown_Datas(null, EventArgs.Empty);
                    BindScheduleData(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                ModalPopup.ShowMessage(Page, msg, "Alert!");
            }
            finally
            {
                FileUpload1.Attributes.Clear();
                upload_DropDownList1.SelectedIndex = 0;
                calendar_TB1.Text = string.Empty;
                calendar_TB2.Text = string.Empty;
            }
        }

        //orig
        //private DataTable ConvertToDataTable(List<ScheduleRow> scheduleRows)
        //{
        //    DataTable dataTable = new DataTable();

        //    // Define the schema of the DataTable
        //    dataTable.Columns.Add("RoomID", typeof(object));
        //    dataTable.Columns.Add("SectionID", typeof(object));
        //    dataTable.Columns.Add("CourseID", typeof(object));
        //    dataTable.Columns.Add("InstructorID", typeof(object));
        //    dataTable.Columns.Add("Day", typeof(string));
        //    dataTable.Columns.Add("StartTime", typeof(TimeSpan));
        //    dataTable.Columns.Add("EndTime", typeof(TimeSpan));
        //    dataTable.Columns.Add("StartDate", typeof(DateTime));
        //    dataTable.Columns.Add("EndDate", typeof(DateTime));
        //    dataTable.Columns.Add("Remarks", typeof(string));
        //    dataTable.Columns.Add("BuildingID", typeof(object));
        //    dataTable.Columns.Add("fileID", typeof(object));

        //    // Fill the DataTable with data from List<ScheduleRow>
        //    foreach (var scheduleRow in scheduleRows)
        //    {
        //        DataRow row = dataTable.NewRow();
        //        row["RoomID"] = scheduleRow.RoomID;
        //        row["SectionID"] = scheduleRow.SectionID;
        //        row["CourseID"] = scheduleRow.CourseID;
        //        row["InstructorID"] = scheduleRow.InstructorID;
        //        row["Day"] = scheduleRow.Day;
        //        row["StartTime"] = scheduleRow.StartTime;
        //        row["EndTime"] = scheduleRow.EndTime;
        //        row["StartDate"] = scheduleRow.StartDate;
        //        row["EndDate"] = scheduleRow.EndDate;
        //        row["Remarks"] = DBNull.Value;
        //        row["BuildingID"] = scheduleRow.BuildingID;
        //        row["fileID"] = scheduleRow.fileID;
        //        dataTable.Rows.Add(row);
        //    }

        //    return dataTable;
        //}

        //changed the data that is showed based on the calendar
        //orig bind schedule
        //protected void BindScheduleData(object sender, EventArgs e)
        //{
        //    string selected_ID_ROOM = DropDownList2.SelectedValue;
        //    string selected_File = uploadSchedsDL.SelectedValue;

        //    DateTime selectedDate;

        //    if (Calendar3.SelectedDate == DateTime.MinValue)
        //    {
        //        // If no date is selected, use today's date as a default
        //        selectedDate = DateTime.Today;
        //    }
        //    else
        //    {
        //        selectedDate = Calendar3.SelectedDate;
        //    }

        //    // Calculate the day of the week for the selected date
        //    int dayOfWeek = (int)selectedDate.DayOfWeek + 1; // Adding 1 to match DayID (Sunday = 1, Monday = 2, ...)

        //    // Store the day of the week in a hidden field (optional, if needed in client-side code)
        //    hiddenDayOfWeek.Value = dayOfWeek.ToString();

        //    //orig
        //    string query = @"
        //        SELECT
        //        s.ScheduleID,  -- Include ScheduleID in the SELECT
        //        CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
        //        MAX(CASE WHEN s.DayID = 1 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Sunday],
        //        MAX(CASE WHEN s.DayID = 2 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Monday],
        //        MAX(CASE WHEN s.DayID = 3 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Tuesday],
        //        MAX(CASE WHEN s.DayID = 4 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Wednesday],
        //        MAX(CASE WHEN s.DayID = 5 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Thursday],
        //        MAX(CASE WHEN s.DayID = 6 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Friday],
        //        MAX(CASE WHEN s.DayID = 7 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName) ELSE NULL END) AS [Saturday]
        //        FROM Schedule s
        //        JOIN Sections sec ON s.SectionID = sec.SectionID
        //        JOIN Courses c ON s.CourseID = c.CourseID
        //        JOIN Instructors i ON s.InstructorID = i.InstructorID
        //        WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)
        //        AND @SelectedDate BETWEEN s.StartDate AND s.EndDate
        //        AND s.DayID = @DayOfWeek
        //        AND (@UploadID = 0 OR s.UploadID = @UploadID OR s.UploadID IS NULL)
        //        GROUP BY s.ScheduleID, s.StartTime, s.EndTime, s.Remarks
        //        ORDER BY s.StartTime;
        //    ";

        //    using (SqlConnection connection = dbConnection.GetConnection())
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);
        //            cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
        //            cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

        //            // If no file is selected or selected value is 0, ignore the file filter
        //            int selectedFileID = string.IsNullOrEmpty(selected_File) ? 0 : int.Parse(selected_File);
        //            cmd.Parameters.AddWithValue("@UploadID", selectedFileID);

        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);

        //                GridView1.DataKeyNames = new string[] { "ScheduleID" };

        //                GridView1.DataSource = dt;
        //                GridView1.DataBind();
        //            }
        //        }
        //    }
        //}
        private DataTable ConvertToDataTable(List<ScheduleRow> scheduleRows)
        {
            DataTable dataTable = new DataTable();

            // Define the schema of the DataTable
            dataTable.Columns.Add("RoomID", typeof(object));
            dataTable.Columns.Add("SectionID", typeof(object));
            dataTable.Columns.Add("CourseID", typeof(object));
            dataTable.Columns.Add("InstructorID", typeof(object));
            dataTable.Columns.Add("Day", typeof(object));  // Now using DayID instead of Day
            dataTable.Columns.Add("StartTime", typeof(TimeSpan));
            dataTable.Columns.Add("EndTime", typeof(TimeSpan));
            dataTable.Columns.Add("ScheduleDate", typeof(DateTime));  // ScheduleDate instead of StartDate/EndDate
            dataTable.Columns.Add("Remarks", typeof(string));
            dataTable.Columns.Add("BuildingID", typeof(object));
            dataTable.Columns.Add("fileID", typeof(object));  // renamed from fileID to UploadID

            // Fill the DataTable with data from List<ScheduleRow>
            foreach (var scheduleRow in scheduleRows)
            {
                DataRow row = dataTable.NewRow();
                row["RoomID"] = scheduleRow.RoomID;
                row["SectionID"] = scheduleRow.SectionID;
                row["CourseID"] = scheduleRow.CourseID;
                row["InstructorID"] = scheduleRow.InstructorID;
                row["Day"] = scheduleRow.Day;  // Assign DayID instead of Day
                row["StartTime"] = scheduleRow.StartTime;
                row["EndTime"] = scheduleRow.EndTime;
                row["ScheduleDate"] = scheduleRow.ScheduleDate;  // Assign ScheduleDate
                row["Remarks"] = string.IsNullOrEmpty(scheduleRow.Remarks) ? DBNull.Value : (object)scheduleRow.Remarks;
                row["BuildingID"] = scheduleRow.BuildingID;
                row["fileID"] = scheduleRow.fileID;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }


        protected void BindScheduleData(object sender, EventArgs e)
        {
            string selected_ID_ROOM = DropDownList2.SelectedValue;
            string selected_File = uploadSchedsDL.SelectedValue;

            DateTime selectedDate;

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

            //orig
            string query = @"
                SELECT
                    CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
                    MAX(CASE WHEN s.DayID = 1 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Sunday],
                    MAX(CASE WHEN s.DayID = 1 THEN s.ScheduleID ELSE NULL END) AS [SundayScheduleID],
                    MAX(CASE WHEN s.DayID = 2 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Monday],
                    MAX(CASE WHEN s.DayID = 2 THEN s.ScheduleID ELSE NULL END) AS [MondayScheduleID],
                    MAX(CASE WHEN s.DayID = 3 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Tuesday],
                    MAX(CASE WHEN s.DayID = 3 THEN s.ScheduleID ELSE NULL END) AS [TuesdayScheduleID],
                    MAX(CASE WHEN s.DayID = 4 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Wednesday],
                    MAX(CASE WHEN s.DayID = 4 THEN s.ScheduleID ELSE NULL END) AS [WednesdayScheduleID],
                    MAX(CASE WHEN s.DayID = 5 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Thursday],
                    MAX(CASE WHEN s.DayID = 5 THEN s.ScheduleID ELSE NULL END) AS [ThursdayScheduleID],
                    MAX(CASE WHEN s.DayID = 6 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Friday],
                    MAX(CASE WHEN s.DayID = 6 THEN s.ScheduleID ELSE NULL END) AS [FridayScheduleID],
                    MAX(CASE WHEN s.DayID = 7 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Saturday],
                    MAX(CASE WHEN s.DayID = 7 THEN s.ScheduleID ELSE NULL END) AS [SaturdayScheduleID]
                FROM Schedule s
                JOIN Sections sec ON s.SectionID = sec.SectionID
                JOIN Courses c ON s.CourseID = c.CourseID
                JOIN Instructors i ON s.InstructorID = i.InstructorID
                WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)    
                AND (@UploadID = 0 OR s.UploadID = @UploadID OR s.UploadID IS NULL)
                GROUP BY s.StartTime, s.EndTime
                ORDER BY s.StartTime;
            ";


            string query_ = @"
                SELECT
                    CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
                    MAX(CASE WHEN s.DayID = 1 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Sunday],
                    MAX(CASE WHEN s.DayID = 1 THEN s.ScheduleID ELSE NULL END) AS [SundayScheduleID],
                    MAX(CASE WHEN s.DayID = 2 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Monday],
                    MAX(CASE WHEN s.DayID = 2 THEN s.ScheduleID ELSE NULL END) AS [MondayScheduleID],
                    MAX(CASE WHEN s.DayID = 3 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Tuesday],
                    MAX(CASE WHEN s.DayID = 3 THEN s.ScheduleID ELSE NULL END) AS [TuesdayScheduleID],
                    MAX(CASE WHEN s.DayID = 4 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Wednesday],
                    MAX(CASE WHEN s.DayID = 4 THEN s.ScheduleID ELSE NULL END) AS [WednesdayScheduleID],
                    MAX(CASE WHEN s.DayID = 5 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Thursday],
                    MAX(CASE WHEN s.DayID = 5 THEN s.ScheduleID ELSE NULL END) AS [ThursdayScheduleID],
                    MAX(CASE WHEN s.DayID = 6 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Friday],
                    MAX(CASE WHEN s.DayID = 6 THEN s.ScheduleID ELSE NULL END) AS [FridayScheduleID],
                    MAX(CASE WHEN s.DayID = 7 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Saturday],
                    MAX(CASE WHEN s.DayID = 7 THEN s.ScheduleID ELSE NULL END) AS [SaturdayScheduleID]
                FROM Schedule s
                LEFT JOIN Sections sec ON s.SectionID = sec.SectionID
                LEFT JOIN Courses c ON s.CourseID = c.CourseID
                LEFT JOIN Instructors i ON s.InstructorID = i.InstructorID
                WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)    
                AND (@UploadID = 0 OR s.UploadID = @UploadID OR s.UploadID IS NULL)
                GROUP BY s.StartTime, s.EndTime
                ORDER BY s.StartTime;
            ";




            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query_, connection))
                {
                    cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);
                    //cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
                    //cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);


                    // If no file is selected or selected value is 0, ignore the file filter
                    int selectedFileID = string.IsNullOrEmpty(selected_File) ? 0 : int.Parse(selected_File);
                    cmd.Parameters.AddWithValue("@UploadID", selectedFileID);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                      

                        //GridView1.DataKeyNames = new string[] { "ScheduleID" };

                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }

        //orig
        //protected void Calendar3_SelectionChanged(object sender, EventArgs e)
        //{
        //    string selected_ID_ROOM = DropDownList2.SelectedValue;
        //    string selected_File = uploadSchedsDL.SelectedValue;
        //    DateTime selectedDate;
        //    selectedDate = Calendar3.SelectedDate;
        //    int dayOfWeek = (int)selectedDate.DayOfWeek + 1;

        //    string query = @"
        //        SELECT
        //            CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
        //            MAX(CASE WHEN s.DayID = 1 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Sunday],
        //            MAX(CASE WHEN s.DayID = 1 THEN s.ScheduleID ELSE NULL END) AS [SundayScheduleID],
        //            MAX(CASE WHEN s.DayID = 2 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Monday],
        //            MAX(CASE WHEN s.DayID = 2 THEN s.ScheduleID ELSE NULL END) AS [MondayScheduleID],
        //            MAX(CASE WHEN s.DayID = 3 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Tuesday],
        //            MAX(CASE WHEN s.DayID = 3 THEN s.ScheduleID ELSE NULL END) AS [TuesdayScheduleID],
        //            MAX(CASE WHEN s.DayID = 4 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Wednesday],
        //            MAX(CASE WHEN s.DayID = 4 THEN s.ScheduleID ELSE NULL END) AS [WednesdayScheduleID],
        //            MAX(CASE WHEN s.DayID = 5 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Thursday],
        //            MAX(CASE WHEN s.DayID = 5 THEN s.ScheduleID ELSE NULL END) AS [ThursdayScheduleID],
        //            MAX(CASE WHEN s.DayID = 6 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Friday],
        //            MAX(CASE WHEN s.DayID = 6 THEN s.ScheduleID ELSE NULL END) AS [FridayScheduleID],
        //            MAX(CASE WHEN s.DayID = 7 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Saturday],
        //            MAX(CASE WHEN s.DayID = 7 THEN s.ScheduleID ELSE NULL END) AS [SaturdayScheduleID]
        //        FROM Schedule s
        //        JOIN Sections sec ON s.SectionID = sec.SectionID
        //        JOIN Courses c ON s.CourseID = c.CourseID
        //        JOIN Instructors i ON s.InstructorID = i.InstructorID
        //        WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)    
        //        AND (@UploadID = 0 OR s.UploadID = @UploadID OR s.UploadID IS NULL)
        //        AND @SelectedDate BETWEEN s.StartDate AND s.EndDate
        //        AND s.DayID = @DayOfWeek
        //        GROUP BY s.StartTime, s.EndTime
        //        ORDER BY s.StartTime;
        //    ";




        //    using (SqlConnection connection = dbConnection.GetConnection())
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@RoomID", selected_ID_ROOM);
        //            cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
        //            cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);


        //            // If no file is selected or selected value is 0, ignore the file filter
        //            int selectedFileID = string.IsNullOrEmpty(selected_File) ? 0 : int.Parse(selected_File);
        //            cmd.Parameters.AddWithValue("@UploadID", selectedFileID);

        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);


        //                GridView1.DataSource = dt;
        //                GridView1.DataBind();
        //            }
        //        }
        //    }
        //}
        //v2
        protected void Calendar3_SelectionChanged(object sender, EventArgs e)
        {
            // Retrieve selected values from DropDownLists
            string selected_ID_ROOM = DropDownList2.SelectedValue;
            string selected_File = uploadSchedsDL.SelectedValue;

            // Get the selected date from the calendar
            DateTime selectedDate = Calendar3.SelectedDate;
            int dayOfWeek = (int)selectedDate.DayOfWeek + 1;  // Adjusting DayOfWeek to start from 1

            // orig
            string query = @"
                    SELECT
                        CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
                        MAX(CASE WHEN s.DayID = 1 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Sunday],
                        MAX(CASE WHEN s.DayID = 1 THEN s.ScheduleID ELSE NULL END) AS [SundayScheduleID],
                        MAX(CASE WHEN s.DayID = 2 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Monday],
                        MAX(CASE WHEN s.DayID = 2 THEN s.ScheduleID ELSE NULL END) AS [MondayScheduleID],
                        MAX(CASE WHEN s.DayID = 3 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Tuesday],
                        MAX(CASE WHEN s.DayID = 3 THEN s.ScheduleID ELSE NULL END) AS [TuesdayScheduleID],
                        MAX(CASE WHEN s.DayID = 4 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Wednesday],
                        MAX(CASE WHEN s.DayID = 4 THEN s.ScheduleID ELSE NULL END) AS [WednesdayScheduleID],
                        MAX(CASE WHEN s.DayID = 5 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Thursday],
                        MAX(CASE WHEN s.DayID = 5 THEN s.ScheduleID ELSE NULL END) AS [ThursdayScheduleID],
                        MAX(CASE WHEN s.DayID = 6 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Friday],
                        MAX(CASE WHEN s.DayID = 6 THEN s.ScheduleID ELSE NULL END) AS [FridayScheduleID],
                        MAX(CASE WHEN s.DayID = 7 THEN CONCAT(c.CourseCode, '-', sec.SectionName, ' ', i.InstructorName, ' ', s.Remarks) ELSE NULL END) AS [Saturday],
                        MAX(CASE WHEN s.DayID = 7 THEN s.ScheduleID ELSE NULL END) AS [SaturdayScheduleID]
                    FROM Schedule s
                    JOIN Sections sec ON s.SectionID = sec.SectionID
                    JOIN Courses c ON s.CourseID = c.CourseID
                    JOIN Instructors i ON s.InstructorID = i.InstructorID
                    WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)    
                    AND (@UploadID = 0 OR s.UploadID = @UploadID OR s.UploadID IS NULL)
                    AND @SelectedDate = s.ScheduleDate
                    AND s.DayID = @DayOfWeek
                    GROUP BY s.StartTime, s.EndTime
                    ORDER BY s.StartTime;
                ";


            string query_ = @"
                SELECT
                    CONVERT(varchar, s.StartTime, 100) + '-' + CONVERT(varchar, s.EndTime, 100) AS [Time],
                    MAX(CASE WHEN s.DayID = 1 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Sunday],
                    MAX(CASE WHEN s.DayID = 1 THEN s.ScheduleID ELSE NULL END) AS [SundayScheduleID],
                    MAX(CASE WHEN s.DayID = 2 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Monday],
                    MAX(CASE WHEN s.DayID = 2 THEN s.ScheduleID ELSE NULL END) AS [MondayScheduleID],
                    MAX(CASE WHEN s.DayID = 3 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Tuesday],
                    MAX(CASE WHEN s.DayID = 3 THEN s.ScheduleID ELSE NULL END) AS [TuesdayScheduleID],
                    MAX(CASE WHEN s.DayID = 4 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Wednesday],
                    MAX(CASE WHEN s.DayID = 4 THEN s.ScheduleID ELSE NULL END) AS [WednesdayScheduleID],
                    MAX(CASE WHEN s.DayID = 5 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Thursday],
                    MAX(CASE WHEN s.DayID = 5 THEN s.ScheduleID ELSE NULL END) AS [ThursdayScheduleID],
                    MAX(CASE WHEN s.DayID = 6 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Friday],
                    MAX(CASE WHEN s.DayID = 6 THEN s.ScheduleID ELSE NULL END) AS [FridayScheduleID],
                    MAX(CASE WHEN s.DayID = 7 THEN CONCAT(ISNULL(c.CourseCode, ' - '), '-', ISNULL(sec.SectionName, ' - '), ' ', ISNULL(i.InstructorName, ' - '), ' ', ISNULL(s.Remarks, ' - ')) ELSE NULL END) AS [Saturday],
                    MAX(CASE WHEN s.DayID = 7 THEN s.ScheduleID ELSE NULL END) AS [SaturdayScheduleID]
                FROM Schedule s
                LEFT JOIN Sections sec ON s.SectionID = sec.SectionID
                LEFT JOIN Courses c ON s.CourseID = c.CourseID
                LEFT JOIN Instructors i ON s.InstructorID = i.InstructorID
                WHERE (@RoomID IS NULL OR s.RoomID = @RoomID)    
                AND (@UploadID = 0 OR s.UploadID = @UploadID OR s.UploadID IS NULL)
                AND @SelectedDate = s.ScheduleDate
                AND s.DayID = @DayOfWeek
                GROUP BY s.StartTime, s.EndTime
                ORDER BY s.StartTime;
            ";




            using (SqlConnection connection = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query_, connection))
                {
                    // Add parameters to prevent SQL injection and ensure safe execution
                    cmd.Parameters.AddWithValue("@RoomID", string.IsNullOrEmpty(selected_ID_ROOM) ? (object)DBNull.Value : selected_ID_ROOM);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
                    cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

                    // If no file is selected or selected value is 0, ignore the file filter
                    int selectedFileID = string.IsNullOrEmpty(selected_File) ? 0 : int.Parse(selected_File);
                    cmd.Parameters.AddWithValue("@UploadID", selectedFileID);

                    try
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            // Bind the result to the GridView
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        ModalPopup.ShowMessage(Page, msg, "Alert!");
                    }
                }
            }
        }


        //updating the data upon editing from the gridview
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string scheduleID = e.CommandArgument.ToString();
                Debug.WriteLine(scheduleID);

                //string query = @"
                //        SELECT
                //            s.ScheduleID,
                //            r.RoomName,
                //            c.CourseCode,
                //            sec.SectionName,
                //            i.InstructorName,
                //            b.BuildingName,
                //            s.StartDate,
                //            s.EndDate,
                //            s.Remarks
                //        FROM Schedule s
                //        JOIN Rooms r ON s.RoomID = r.RoomID
                //        JOIN Courses c ON s.CourseID = c.CourseID
                //        JOIN Sections sec ON s.SectionID = sec.SectionID
                //        JOIN Instructors i ON s.InstructorID = i.InstructorID
                //        JOIN Buildings b ON s.BuildingID = b.BuildingID
                //        WHERE s.ScheduleID = @ScheduleID";

                string query = @"
                        SELECT
                            s.ScheduleID,
                            r.RoomName,
                            c.CourseCode,
                            sec.SectionName,
                            i.InstructorName,
                            b.BuildingName,
                            s.ScheduleDate,
                            s.Remarks
                        FROM Schedule s
                        JOIN Rooms r ON s.RoomID = r.RoomID
                        JOIN Courses c ON s.CourseID = c.CourseID
                        JOIN Sections sec ON s.SectionID = sec.SectionID
                        JOIN Instructors i ON s.InstructorID = i.InstructorID
                        JOIN Buildings b ON s.BuildingID = b.BuildingID
                        WHERE s.ScheduleID = @ScheduleID";

                string query_ = @"
                SELECT
                    s.ScheduleID,
                    r.RoomName,
                    c.CourseCode,
                    sec.SectionName,
                    i.InstructorName,
                    b.BuildingName,
                    s.ScheduleDate,
                    s.Remarks
                FROM Schedule s
                JOIN Rooms r ON s.RoomID = r.RoomID
                LEFT JOIN Courses c ON s.CourseID = c.CourseID
                LEFT JOIN Sections sec ON s.SectionID = sec.SectionID
                LEFT JOIN Instructors i ON s.InstructorID = i.InstructorID
                JOIN Buildings b ON s.BuildingID = b.BuildingID
                WHERE s.ScheduleID = @ScheduleID";


                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query_, connection))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleID", scheduleID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                HiddenField1.Value = scheduleID.ToString();

                                //txtScheduleID.Text = reader["ScheduleID"].ToString();
                                //txtRoomID.Text = reader["RoomName"].ToString();
                                txtCourseID.Text = reader["CourseCode"].ToString();
                                txtSectionID.Text = reader["SectionName"].ToString();
                                txtInstructorID.Text = reader["InstructorName"].ToString();
                                //txtBuildingID.Text = reader["BuildingName"].ToString();
                                //txtStartTime.Text = reader["StartTime"].ToString();
                                //txtEndTime.Text = reader["EndTime"].ToString();
                                //txtStartDate.Text = Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd");
                                //txtEndDate.Text = Convert.ToDateTime(reader["EndDate"]).ToString("yyyy-MM-dd");
                                //txtDate.Text = Convert.ToDateTime(reader["ScheduleDate"]).ToString("yyyy-MM-dd");
                                txtRemarks.Text = reader["Remarks"].ToString();
                            }
                        }
                    }
                }

                // Show the modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#scheduleModal').modal('show');", true);

            }


            //if (e.CommandName == "ShowModal")
            //{
            //    int scheduleID = Convert.ToInt32(e.CommandArgument);
            //    Debug.WriteLine(scheduleID);

            //    string query = @"
            //        SELECT
            //            s.ScheduleID,
            //            r.RoomName,
            //            c.CourseCode,
            //            sec.SectionName,
            //            i.InstructorName,
            //            b.BuildingName,
            //            s.StartDate,
            //            s.EndDate,
            //            s.Remarks
            //        FROM Schedule s
            //        JOIN Rooms r ON s.RoomID = r.RoomID
            //        JOIN Courses c ON s.CourseID = c.CourseID
            //        JOIN Sections sec ON s.SectionID = sec.SectionID
            //        JOIN Instructors i ON s.InstructorID = i.InstructorID
            //        JOIN Buildings b ON s.BuildingID = b.BuildingID
            //        WHERE s.ScheduleID = @ScheduleID";

            //    using (SqlConnection connection = dbConnection.GetConnection())
            //    {
            //        using (SqlCommand cmd = new SqlCommand(query, connection))
            //        {
            //            cmd.Parameters.AddWithValue("@ScheduleID", scheduleID);

            //            using (SqlDataReader reader = cmd.ExecuteReader())
            //            {
            //                if (reader.Read())
            //                {
            //                    HiddenField1.Value = scheduleID.ToString();

            //                    //txtScheduleID.Text = reader["ScheduleID"].ToString();
            //                    //txtRoomID.Text = reader["RoomName"].ToString();
            //                    txtCourseID.Text = reader["CourseCode"].ToString();
            //                    txtSectionID.Text = reader["SectionName"].ToString();
            //                    txtInstructorID.Text = reader["InstructorName"].ToString();
            //                    //txtBuildingID.Text = reader["BuildingName"].ToString();
            //                    //txtStartTime.Text = reader["StartTime"].ToString();
            //                    //txtEndTime.Text = reader["EndTime"].ToString();
            //                    txtStartDate.Text = Convert.ToDateTime(reader["StartDate"]).ToString("yyyy-MM-dd");
            //                    txtEndDate.Text = Convert.ToDateTime(reader["EndDate"]).ToString("yyyy-MM-dd");
            //                    txtRemarks.Text = reader["Remarks"].ToString();
            //                }
            //            }
            //        }
            //    }

            //    // Show the modal
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#scheduleModal').modal('show');", true);
            //}
        }

        //testing
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Example condition to check if the row should be highlighted
                // Here we check if Sunday has a schedule
                string sundayScheduleID = DataBinder.Eval(e.Row.DataItem, "SundayScheduleID").ToString();
                string mondayScheduleID = DataBinder.Eval(e.Row.DataItem, "MondayScheduleID").ToString();
                // Add other day checks as needed

                // Check the condition (you can adjust this logic as necessary)
                if (!string.IsNullOrEmpty(sundayScheduleID) ||
                    !string.IsNullOrEmpty(mondayScheduleID) /* || Add checks for other days if needed */)
                {
                    // Highlight the entire row or specific cells by adding a CSS class
                    e.Row.Attributes["class"] = "highlight-green";
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Visible = false;

            //txtBuildingID.ReadOnly = false;
            //txtRoomID.ReadOnly = false;
            txtCourseID.Enabled = true;
            txtSectionID.Enabled = true;
            txtInstructorID.Enabled = true;
            //txtStartTime.ReadOnly = false;
            //txtEndTime.ReadOnly = false;
            //txtStartDate.ReadOnly = false;
            //txtEndDate.ReadOnly = false;
            txtRemarks.Enabled = true;

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
                        cmd.Parameters.AddWithValue("@CourseID", coursecodeID);
                        cmd.Parameters.AddWithValue("@SectionID", sectionID);
                        cmd.Parameters.AddWithValue("@InstructorID", instructorID);

                        string remarksText = string.IsNullOrWhiteSpace(txtRemarks.Text) ? string.Empty : "Remarks: " + txtRemarks.Text;
                        cmd.Parameters.AddWithValue("@Remarks", remarksText);
                        cmd.Parameters.AddWithValue("@ScheduleID", HiddenField1.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            btnUpdate.Visible = false;
                            btnEdit.Visible = true;

                            txtCourseID.Enabled = false;
                            txtSectionID.Enabled = false;
                            txtInstructorID.Enabled = false;
                            txtRemarks.Enabled = false;

                            // Update successful: close modal and display success message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal", "$('#scheduleModal').modal('hide');", true);

                            // Optionally, refresh the GridView or notify the user
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", "alert('Schedule updated successfully.');", true);
                            ModalPopup.ShowMessage_(Page, "Schedule updated successfully.", "Notification");

                            // Optionally, refresh GridView
                            //GridView1.DataBind();

                            //BindScheduleData(sender, e);
                            Calendar3_SelectionChanged(sender, e);
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





        protected void btnRemove_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = dbConnection.GetConnection())
            {
                // Prepare your SQL update query
                string query = @"
                UPDATE Schedule
                SET CourseID = @CourseID,
                    SectionID = @SectionID,
                    InstructorID = @InstructorID,
                    Remarks = @Remarks
                WHERE ScheduleID = @ScheduleID"; 

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set parameters to DBNull.Value to represent NULL in the database
                    command.Parameters.AddWithValue("@CourseID", DBNull.Value);
                    command.Parameters.AddWithValue("@SectionID", DBNull.Value);
                    command.Parameters.AddWithValue("@InstructorID", DBNull.Value);
                    //command.Parameters.AddWithValue("@ScheduleDate", DBNull.Value);
                    command.Parameters.AddWithValue("@Remarks", DBNull.Value);

                    // Set the ScheduleID parameter value (this should not be NULL)
                    command.Parameters.AddWithValue("@ScheduleID", HiddenField1.Value); // Ensure HiddenField1 has the correct ScheduleID

                    try
                    {
                        // Execute the update command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Clear the textboxes for CourseID, SectionID, and InstructorID
                            txtCourseID.Text = string.Empty; // Clear CourseID
                            txtSectionID.Text = string.Empty; // Clear SectionID
                            txtInstructorID.Text = string.Empty; // Clear InstructorID

                           
                            // Set textboxes to read-only if necessary
                            txtCourseID.ReadOnly = true;
                            txtSectionID.ReadOnly = true;
                            txtInstructorID.ReadOnly = true;

                            // Show success message
                            ModalPopup.ShowMessage_(Page, "Schedule removed successfully.", "Notification");

                            // Optionally, refresh the GridView
                            //BindScheduleData(sender, e);
                            Calendar3_SelectionChanged(sender, e);
                        }
                        else
                        {
                            // Update failed: display error message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", "alert('Error: No rows were updated.');", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        string msg = "Error: " + ex.Message;
                        ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
                    }
                }
            }
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
                string buildingId = ADD_DropDownList1.SelectedValue;
                string faculty = RFacultyDL.SelectedValue;//WILL BE USED FOR BOOKING LATER
                DateTime startDate = DateTime.Parse(SelectDateTB.Text);
                DateTime endDate = DateTime.Parse(EndDateTB.Text);
                string startTime = RTimeStart.SelectedItem.Text;
                string endTime = RTimeEnd.SelectedItem.Text;

                get_ID add_dbHelper = new get_ID();

                int buildID = int.Parse(ADD_DropDownList1.SelectedValue);

                //var add_Ids = add_dbHelper.CheckAndInsertValues(connection, roomNumber, section, courseCode, professor, buildID);

                //int buildingID = add_dbHelper.GetOrInsertBuilding(connection, ADD_DropDownList1.SelectedValue);
                //int roomID = add_Ids.roomID;

                //int sectionID = add_Ids.sectionID;
                //int courseID = add_Ids.courseID;
                //int instructorID = add_Ids.instructorID;

                int sectionID = add_dbHelper.GetOrInsertSection(connection, section);
                int courseID = add_dbHelper.GetOrInsertCourse(connection, courseCode);
                int instructorID = add_dbHelper.GetOrInsertInstructor(connection, professor);

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
                        cmd.Parameters.AddWithValue("@RoomID", roomNumber);
                        cmd.Parameters.AddWithValue("@SectionID", sectionID);
                        cmd.Parameters.AddWithValue("@CourseID", courseID);
                        cmd.Parameters.AddWithValue("@InstructorID", instructorID);
                        cmd.Parameters.AddWithValue("@DayID", dayID);
                        cmd.Parameters.AddWithValue("@StartTime", startTimeOfDay);
                        cmd.Parameters.AddWithValue("@EndTime", endTimeOfDay);
                        cmd.Parameters.AddWithValue("@StartDate", date);
                        cmd.Parameters.AddWithValue("@EndDate", date);
                        cmd.Parameters.AddWithValue("@Remarks", remarks);
                        cmd.Parameters.AddWithValue("@BuildingID", buildingId);

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

            //upload_DropDownList1.SelectedIndex = 0;
            upload_DropDownList1.Items.Insert(0, new ListItem("Select Building", "0"));
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

        //for page reset
        protected void Reset_Page(object sender, EventArgs e)
        {
            string deleteQuery = @"
                DELETE FROM Schedule;
                DELETE FROM upload_SchedsTBL;
                DELETE FROM Courses;
                DELETE FROM Instructors;
                DELETE FROM Sections;
                DELETE FROM Rooms;
                DELETE FROM Buildings;
            ";

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                try
                {
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Execute the combined delete query
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, connection, transaction))
                            {
                                cmd.ExecuteNonQuery();
                            }

                            // Commit the transaction if all queries executed successfully
                            transaction.Commit();

                            Dropdown_Datas(sender, e);
                            BindScheduleData(sender, e);

                            ModalPopup.ShowMessage(Page, "page has been reset", "Notif!");
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of any errors
                            transaction.Rollback();
                            // Log the error or show a message to the user
                            Dropdown_Datas(sender, e);

                            ModalPopup.ShowMessage(Page, "An error occurred: " + ex.Message, "Alert");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle connection errors
                    ModalPopup.ShowMessage(Page, "Database connection error: " + ex.Message, "Alert");
                }
            }
        }
    }
}