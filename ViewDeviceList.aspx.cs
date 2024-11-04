using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services.Description;


namespace WebApplication1
{
    public partial class ViewDeviceList : System.Web.UI.Page
    {
        private string connectionString = "Server=tcp:roomdata.database.windows.net,1433;Initial Catalog=roomScheduleV27_Prototype;Persist Security Info=False;User ID=FrankDB;Password=Frank12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                Load_AddNewDevice();
                Load_EditDevice();
                LoadRoomNumber();
                ValidateFormFields();
                //LoadBuildings();

            }
        }

        

        protected void ValidateFormFields()
        {
            int txtlength = txtNewEPDID.Text.Length;  // Get the length of the TextBox value
            string ddlBuilding = ddlNewBuilding.SelectedValue;  // Get the selected value of the DropDownList
            string ddlRoomNumber = ddlNewRoomNumber.SelectedValue;

            // Check if the TextBox is empty and no building is selected
            if (txtlength == 0 || ddlBuilding == "" || ddlRoomNumber == "")
            {
                btnAddSave.Enabled = false;  // Disable Save button
            }
            else if (txtlength != 0 || ddlBuilding != "" || ddlRoomNumber != "")
            {
                btnAddSave.Enabled = true;   // Enable Save button
            }
        }


        private void LoadData() // LOAD THE CARDS UPON OPENING THE WEBPAGE
        {
            string query = "SELECT EPD_ID, AssignedBuilding, AssignedRoomNumber, AssignedIPAddress, DeviceStatus FROM EPaperAssignedRoom";
            List<EPaperDetails> devices = new List<EPaperDetails>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        devices.Add(new EPaperDetails
                        {
                            EPD_ID = reader["EPD_ID"].ToString(),
                            AssignedBuilding = reader["AssignedBuilding"].ToString(),
                            AssignedRoomNumber = reader["AssignedRoomNumber"].ToString(),
                            AssignedIPAddress = reader["AssignedIPAddress"].ToString(),
                            DeviceStatus = reader["DeviceStatus"].ToString()
                        });
                    }
                }
            }

            if (devices.Count > 0)
            {
                NoDevicesLabel.Visible = false; // Hide the no devices message
                Repeater1.DataSource = devices;
                Repeater1.DataBind();

                // Register JavaScript for each device after binding the data
                foreach (var device in devices)
                {
                    string epdID = device.EPD_ID;
                    string status = device.DeviceStatus;

                    // Register the script to update the option for each device
                    ScriptManager.RegisterStartupScript(this, GetType(), "UpdateUploadOption" + epdID,
                        $"updateUploadImageOption('{epdID}', '{status}');", true);
                }
            }
            else
            {
                NoDevicesLabel.Visible = true; // Show the no devices message
                Repeater1.DataSource = null; // Ensure the repeater is empty
                Repeater1.DataBind();
            }
        }

        // POPULATE DROPDOWN FOR ADDING NEW DEVICEBUILDING
        private void Load_AddNewDevice()
        {
            string query = "SELECT buildingName FROM Buildings";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlNewBuilding.Items.Clear();
                    ddlNewBuilding.Items.Add(new ListItem("--Select Building--", ""));
                    while (reader.Read())
                    {
                        ddlNewBuilding.Items.Add(new ListItem(reader["buildingName"].ToString(), reader["buildingName"].ToString()));
                    }
                }
            }
        }
        protected void btnUploadImage_Click(object sender, EventArgs e)
        {

        }

        // FOR ADDING NEW DEVICE 
        //protected void btnAddSave_Click(object sender, EventArgs e)
        //{
        //    string epdId = txtNewEPDID.Text;
        //    string building = ddlNewBuilding.SelectedValue;
        //    string roomNumber = ddlNewRoomNumber.SelectedValue;
        //    string deviceStats = "TEXT";

        //    // Set the first three octets as fixed values "192.168.1"
        //    string firstThreeOctets = "192.168.1";

        //    // Fetch the next available fourth octet from the database
        //    int fourthOctet = GetMaxFourthOctet(firstThreeOctets);

        //    // Create the full IP address
        //    string ipAddress = $"{firstThreeOctets}.{fourthOctet}";

        //    string query = "INSERT INTO EPaperAssignedRoom (EPD_ID, AssignedBuilding, AssignedRoomNumber, AssignedIPAddress, DeviceStatus) " +
        //                   "VALUES (@EPD_ID, @AssignedBuilding, @AssignedRoomNumber, @AssignedIPAddress, @DeviceStatus)";

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@EPD_ID", epdId);
        //            cmd.Parameters.AddWithValue("@AssignedBuilding", building);
        //            cmd.Parameters.AddWithValue("@AssignedRoomNumber", roomNumber);
        //            cmd.Parameters.AddWithValue("@AssignedIPAddress", ipAddress);
        //            cmd.Parameters.AddWithValue("@DeviceStatus", deviceStats);
        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //    // Optionally clear the fields on server-side
        //    txtNewEPDID.Text = string.Empty;
        //    ddlNewBuilding.SelectedIndex = 0;  // Resetting to the first option
        //    ddlNewRoomNumber.SelectedIndex = 0; // Resetting to the first option

        //    LoadData();

        //    // Register a startup script to reset the modal
        //    ClientScript.RegisterStartupScript(this.GetType(), "resetModal", "$('#addModal').modal('hide');", true);
        //    // After saving data, redirect to the same page or another page
        //    Response.Redirect(Request.RawUrl); // Redirects to the current URL
        //}
        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            string epdId = txtNewEPDID.Text;
            string building = ddlNewBuilding.SelectedValue;
            string roomNumber = ddlNewRoomNumber.SelectedValue;
            string deviceStats = "TEXT";

            // Set the first three octets as fixed values "192.168.1"
            string firstThreeOctets = "192.168.1";

            // Fetch the next available fourth octet from the database
            int fourthOctet = GetMaxFourthOctet(firstThreeOctets);

            // Create the full IP address
            string ipAddress = $"{firstThreeOctets}.{fourthOctet}";

            // Check if EPD_ID already exists
            if (DoesEPDIdExist(epdId))
            {
                // Use SweetAlert to alert the user that the EPD_ID already exists
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "swal({title: 'Error!', text: 'The EPD ID already exists. Please use a different ID.', icon: 'error', buttons: { cancel: false, confirm: true }, });", true);
                return; // Exit the method to prevent further processing
            }

            // Generate the PHP file and get the file name
            string phpFileName = GeneratePHPFile(epdId);

            string query = "INSERT INTO EPaperAssignedRoom (EPD_ID, AssignedBuilding, AssignedRoomNumber, AssignedIPAddress, DeviceStatus, AssignedPHPFile) " +
                           "VALUES (@EPD_ID, @AssignedBuilding, @AssignedRoomNumber, @AssignedIPAddress, @DeviceStatus, @AssignedPHPFile)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EPD_ID", epdId);
                    cmd.Parameters.AddWithValue("@AssignedBuilding", building);
                    cmd.Parameters.AddWithValue("@AssignedRoomNumber", roomNumber);
                    cmd.Parameters.AddWithValue("@AssignedIPAddress", ipAddress);
                    cmd.Parameters.AddWithValue("@DeviceStatus", deviceStats);
                    cmd.Parameters.AddWithValue("@AssignedPHPFile", phpFileName);  // Save the PHP file name to the database
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Optionally clear the fields on server-side
            txtNewEPDID.Text = string.Empty;
            ddlNewBuilding.SelectedIndex = 0;  // Reset the first option
            ddlNewRoomNumber.SelectedIndex = 0; // Reset the first option

            LoadData();

            // Register a startup script to reset the modal and close it
            ClientScript.RegisterStartupScript(this.GetType(), "resetModal", "$('#addModal').modal('hide');", true);

            // After saving data, redirect to the same page 
            Response.Redirect(Request.RawUrl);
        }

        // Generate PHP file for the device upon saving and return the file name
        private string GeneratePHPFile(string epdName)
        {
            // Sanitize the label value by removing invalid file name characters and spaces
            string validValue = string.Concat(epdName.Split(Path.GetInvalidFileNameChars())).Replace(" ", "");

            // Define the path where the PHP file will be saved, using the sanitized labelValue as the file name
            string phpFilePath = $@"C:\xampp\htdocs\SqlDBConnection\{validValue}.php";  // Adjust this to your XAMPP htdocs folder

            // Define the PHP content, replacing the ID with the label's value
            string phpContent = $@"<?php
            include_once('sqlConnection.php');
            date_default_timezone_set('Asia/Manila');

            $current_time = date('H:i:s'); // Get the time today           
            $current_day = date('l'); // Returns the full name of the day (e.g., ""Monday"")

            try {{
                // Fetch the value of epaper_assignedRoom
                $epaper_query = ""SELECT AssignedRoomNumber, DeviceStatus, AssignedIPAddress, AssignedPHPFile FROM EpaperAssignedRoom WHERE EPD_ID = '{epdName}'"";
                $epaper_stmt = $conn->query($epaper_query);
                $epaper_result = $epaper_stmt->fetch(PDO::FETCH_ASSOC);
                $epaper_assignedRoom = $epaper_result['AssignedRoomNumber'];
                $epaper_status = $epaper_result['DeviceStatus'];
                $epaper_ipaddress = $epaper_result['AssignedIPAddress'];
	            $epaper_assignedphpfile = $epaper_result['AssignedPHPFile'];

                $sql_query = ""
						SELECT  Rooms.RoomName, Courses.CourseCode, Sections.SectionName, Instructors.InstructorName, Schedule.StartTime, Schedule.EndTime, 
						DATEADD(MINUTE, -1, Schedule.EndTime) AS AdjustedEndTime, days_of_week.day, Schedule.ScheduleDate  FROM Rooms
						INNER JOIN Schedule ON Rooms.RoomID = Schedule.RoomID
						INNER JOIN Courses ON Schedule.CourseID = Courses.CourseID
						INNER JOIN Sections ON Schedule.SectionID = Sections.SectionID
						INNER JOIN Instructors ON Schedule.InstructorID = Instructors.InstructorID
						INNER JOIN days_of_week ON Schedule.DayID = days_of_week.DayID
						WHERE Rooms.RoomName = :roomName
						AND days_of_week.day = :current_day
						AND :current_time BETWEEN Schedule.StartTime AND DATEADD(MINUTE, -1, Schedule.EndTime)
						AND Schedule.ScheduleDate = CAST(GETDATE() AS DATE)"";  

                // Prepare the SQL statement
                $stmt = $conn->prepare($sql_query);
                $stmt->bindParam(':roomName', $epaper_assignedRoom);
                $stmt->bindParam(':current_day', $current_day);
                $stmt->bindParam(':current_time', $current_time);

                // Execute the query
                $stmt->execute();
                $results = $stmt->fetchAll(PDO::FETCH_ASSOC);

                // Check if any of the fetched fields are empty
                $isEmpty = true;
                foreach ($results as $result) {{
                    if (!empty($result['CourseCode']) && !empty($result['SectionName'])) {{
                        $isEmpty = false;
                        break;
                    }}
                }}

                if ($isEmpty) {{
                    echo json_encode([[
                        ""RoomName"" => $epaper_assignedRoom,
                        ""DeviceStatus"" => $epaper_status,
                        ""AssignedIPAddress"" => $epaper_ipaddress,
			            ""AssignedPHPFile"" => $epaper_assignedphpfile
			
                    ]], JSON_PRETTY_PRINT);
                    exit;
                }}

                // Convert StartTime, EndTime, and AdjustedEndTime to 12-hour format, concatenate, and remove unnecessary keys
                foreach ($results as &$result) {{
                    $start_time_12hr = date(""g:i A"", strtotime($result['StartTime']));
                    $end_time_12hr = date(""g:i A"", strtotime($result['EndTime']));
                    $adjusted_end_time_12hr = date(""g:i A"", strtotime($result['AdjustedEndTime']));
        
                    // Store the original time slot and adjusted time slot
                    $result['TimeSlot'] = $start_time_12hr . "" - "" . $end_time_12hr;
                    //$result['AdjustedTimeSlot'] = $start_time_12hr . "" - "" . $adjusted_end_time_12hr;

                    // Remove unnecessary keys from the result
                    unset($result['StartTime']);
                    unset($result['EndTime']);
                    unset($result['AdjustedEndTime']);
                    unset($result['day']);
        
                    // Append the epaper_status to each result
                    $result['DeviceStatus'] = $epaper_status;
                    $result['AssignedIPAddress'] = $epaper_ipaddress;
		            $result['AssignedPHPFile'] = $epaper_assignedphpfile;
                }}

                if (count($results) > 0) {{
                    echo json_encode($results, JSON_PRETTY_PRINT);
                }} else {{
                    echo json_encode([""message"" => ""No results found""], JSON_PRETTY_PRINT);
                }}
            }} catch (PDOException $e) {{
                echo json_encode([""error"" => $e->getMessage()], JSON_PRETTY_PRINT);
            }}

            $conn = null;
            ?>";

            try
            {
                // Write the PHP content to the file
                File.WriteAllText(phpFilePath, phpContent);

                // Notify the user that the file was created
                Response.Write("PHP file generated successfully: " + phpFilePath);

                // Return only the file name (without the full path)
                return $"{validValue}.php";
            }
            catch (Exception ex)
            {
                // Handle any errors
                Response.Write("Error generating PHP file: " + ex.Message);
                return null;  // Return null if there's an error
            }
        }

        // check if the EPD_ID exists in the database
        private bool DoesEPDIdExist(string epdId)

        {
            string query = "SELECT COUNT(*) FROM EPaperAssignedRoom WHERE EPD_ID = @EPD_ID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EPD_ID", epdId);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; // Return true if EPD_ID exists
                }
            }
        }


        // get the next available fourth octet
        private int GetMaxFourthOctet(string firstThreeOctets)
        {
            int startingFourthOctet = 160; // Start from 160 if no IP addresses exist
            int nextFourthOctet = startingFourthOctet;

            string query = "SELECT MAX(CAST(PARSENAME(AssignedIPAddress, 1) AS INT)) " +
                           "FROM EPaperAssignedRoom WHERE AssignedIPAddress LIKE @firstThreeOctets + '.%'";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@firstThreeOctets", firstThreeOctets);
                    con.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        int maxFourthOctet = Convert.ToInt32(result);
                        // Increment the fourth octet by 1
                        nextFourthOctet = maxFourthOctet + 1;
                    }
                }
            }

            return nextFourthOctet;
        }



        protected void ddlAddBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedBuilding = ddlNewBuilding.SelectedValue;

            // Clear existing items in ddlNewRoomNumber
            ddlNewRoomNumber.Items.Clear();
            ddlNewRoomNumber.Items.Add(new ListItem("--Select Room--", ""));

            if (!string.IsNullOrEmpty(selectedBuilding))
            {
                
                char firstLetter = selectedBuilding.ToUpper()[0];

                
                string query = @"
                    SELECT RoomName 
                    FROM Rooms 
                    WHERE RoomName LIKE @Pattern 
                      AND RoomName NOT IN (SELECT AssignedRoomNumber FROM EPaperAssignedRoom)";


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Pattern", firstLetter + "%");

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ddlNewRoomNumber.Items.Add(new ListItem(reader["RoomName"].ToString(), reader["RoomName"].ToString()));
                        }
                    }
                }
            }
        }

        // FOR EDITING DEVICE DETAILS
        private void Load_EditDevice()
        {
            string query = "SELECT BuildingName FROM Buildings";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlEditBuilding.Items.Clear();
                    while (reader.Read())
                    {
                        ddlEditBuilding.Items.Add(new ListItem(reader["BuildingName"].ToString(), reader["BuildingName"].ToString()));

                    }
                }
            }
        }
        private void LoadRoomNumber()
        {
            string selectedBuilding = ddlEditBuilding.SelectedValue;

            ddlEditRoomNumber.Items.Clear();

            if (!string.IsNullOrEmpty(selectedBuilding))
            {
                // Get the first letter of the selected building
                char firstLetter = selectedBuilding.ToUpper()[0];

                string query = @"
                    SELECT RoomName 
                    FROM Rooms 
                    WHERE RoomName NOT IN (
                        SELECT AssignedRoomNumber 
                        FROM EPaperAssignedRoom 
                    ) 
                    AND RoomName LIKE @Pattern";


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Pattern", firstLetter + "%");

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ddlEditRoomNumber.Items.Add(new ListItem(reader["RoomName"].ToString(), reader["RoomName"].ToString()));
                        }
                    }
                }
            }
        }
        protected void ddlEditBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBuilding = ddlEditBuilding.SelectedValue;

            ddlEditRoomNumber.Items.Clear();

            if (!string.IsNullOrEmpty(selectedBuilding))
            {
                // Get the first letter of the selected building
                char firstLetter = selectedBuilding.ToUpper()[0];

                string query = @"
                    SELECT RoomName 
                    FROM Rooms 
                    WHERE RoomName NOT IN (
                        SELECT AssignedRoomNumber 
                        FROM EPaperAssignedRoom 
                    ) 
                    AND RoomName LIKE @Pattern";


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Pattern", firstLetter + "%");

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ddlEditRoomNumber.Items.Add(new ListItem(reader["RoomName"].ToString(), reader["RoomName"].ToString()));
                        }
                    }
                }
            }
        }
        protected void btnEditSave_Click(object sender, EventArgs e)
        {
            string epdId = txtEditEPDID.Text;
            string editBuilding = ddlEditBuilding.SelectedValue;
            string editRoomNumber = ddlEditRoomNumber.SelectedValue;

            string query = "UPDATE EPaperAssignedRoom SET AssignedBuilding = @AssignedBuilding, AssignedRoomNumber = @AssignedRoomNumber WHERE EPD_ID = @EPD_ID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EPD_ID", epdId);
                    cmd.Parameters.AddWithValue("@AssignedBuilding", editBuilding);
                    cmd.Parameters.AddWithValue("@AssignedRoomNumber", editRoomNumber);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadData(); // Reload data to reflect changes
        }





        // For deleting the Device
        protected void DeleteDevice(string epdId)
        {

            string query = "DELETE FROM EPaperAssignedRoom WHERE EPD_ID = @epdId"; // Adjust the table name if necessary

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@epdId", epdId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string epdIdToDelete = txtEditEPDID.Text; // Assuming you have a textbox for EPD_ID
            DeleteDevice(epdIdToDelete);
            // Optionally, refresh the data or provide user feedback
            LoadData();
        }


        //  Toggle Button for changing from Text to Image or vice versa.
        protected void ToggleButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox toggleButton = (CheckBox)sender;
            HiddenField hiddenEPDID = (HiddenField)toggleButton.NamingContainer.FindControl("HiddenEPDID");

            string epdID = hiddenEPDID.Value;
            string newStatus = toggleButton.Checked ? "IMAGE" : "TEXT";

            // Update the status in the database
            UpdateDeviceStatus(epdID, newStatus);

            // Register JavaScript to update the Upload Image option on the client side
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateUploadOption",
                $"updateUploadImageOption('{epdID}', '{newStatus}');", true);
        }


        private void UpdateDeviceStatus(string epdID, string newStatus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE EPaperAssignedRoom SET DeviceStatus = @DeviceStatus WHERE EPD_ID = @EPD_ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DeviceStatus", newStatus);
                    command.Parameters.AddWithValue("@EPD_ID", epdID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }



        public class EPaperDetails
        {
            public string EPD_ID { get; set; }
            public string AssignedBuilding { get; set; }
            public string AssignedRoomNumber { get; set; }
            public string AssignedIPAddress { get; set; }
            public string DeviceStatus { get; set; }
        }
    }
}