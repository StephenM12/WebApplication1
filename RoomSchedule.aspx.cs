//for excel package
using OfficeOpenXml;
using System;
using System.Data;

//sql connection:
using System.Data.SqlClient;

//file processing
using System.IO;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class RoomSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                //default table 
                //SqlCommand selectCommand = new SqlCommand("SELECT * FROM --------", connection);
                //SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                //DataTable table = new DataTable();
                //adapter.Fill(table);
                //GridView1.DataSource = table;
                //GridView1.DataBind();


                if (!IsPostBack)
                {
                    dropdown_Data(sender, e);


                }




            }
        }

        protected void dropdown_Data(object sender, EventArgs e)
        {



            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {

                //Dropdown datas from sql 
                SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM upload_Scheds", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                


                // Bind the data to the dropdown list
                DropDownList1.DataTextField = "Name"; // Column name to display
                DropDownList1.DataValueField = "ID"; // Column name to use as value
                DropDownList1.DataSource = reader;
                DropDownList1.DataBind();
                reader.Close();



            }
            



        }
        protected void Upload_File(object sender, EventArgs e)
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contentType = FileUpload1.PostedFile.ContentType;

            using (Stream fs = FileUpload1.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    // Open database connection
                    SqlConnection connection = dbConnection.GetConnection();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {// Perform your database operations here:
                        String query = "INSERT INTO upload_Scheds VALUES (@Name, @ContentType, @Data)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Name", fileName);
                            command.Parameters.AddWithValue("@ContentType", contentType);
                            command.Parameters.AddWithValue("@Data", bytes);

                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }

            Response.Write("File Upload successfully");
            
        }
        
        protected void Bind_Uploaded_GridView(object sender, EventArgs e)
        {
            string selected_ID = DropDownList1.SelectedValue;
            
            try
            {
                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand selectCommand = new SqlCommand("SELECT Data FROM upload_Scheds WHERE ID = @File_ID", connection);
                    selectCommand.Parameters.AddWithValue("@File_ID", selected_ID);

                    byte[] excelData = (byte[])selectCommand.ExecuteScalar();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (MemoryStream stream = new MemoryStream(excelData))
                    {
                        using (ExcelPackage package = new ExcelPackage(stream))
                        {
                            int worksheetIndex = 1; // Example index

                            if (worksheetIndex <= package.Workbook.Worksheets.Count)
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                                int startRow = 6; // Starting row index
                                int endRow = 15;
                                int startColumn = 1; // Starting column index (A)
                                int endColumn = 8; // Ending column index (H)

                                // Create a DataTable to store the extracted data
                                DataTable dataTable = new DataTable();

                                // Add columns to the DataTable based on the number of columns in the range
                                for (int col = startColumn; col <= endColumn; col++)
                                {
                                    dataTable.Columns.Add("Column " + col.ToString()); // You can customize column names here
                                }

                                // Iterate over each row in the range
                                for (int row = startRow; row <= endRow; row++)
                                {
                                    // Create a new DataRow to store the values of the current row
                                    DataRow dataRow = dataTable.NewRow();

                                    // Iterate over each column in the range
                                    for (int col = startColumn; col <= endColumn; col++)
                                    {
                                        // Get the value of the current cell
                                        object cellValue = worksheet.Cells[row, col].Value;

                                        // Add the cell value to the DataRow
                                        dataRow[col - startColumn] = cellValue != null ? cellValue.ToString() : ""; // Convert cell value to string
                                    }

                                    // Add the DataRow to the DataTable
                                    dataTable.Rows.Add(dataRow);

                                }

                                GridView1.DataSource = dataTable;
                                GridView1.DataBind();

                            }
                        }

                        connection.Close();
                    }
                }
            }
            catch
            {
                Response.Write("Failed to Show Table");
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

        protected void deployBTNclk(object sender, EventArgs e)
        {
            string courseCode = RCourseCodeTB.Text; //course code
            string sec = RSectionTB.Text; //section
            string prof = RProfTB.Text; //prof/instructor
            string room = RRoomNumberTB.Text; //room number
            string selectedCollege = RFacultyDL.SelectedItem.Text; //college value
            string selectedTimerealValue = RTimeDL.SelectedItem.Text; //Selected Time
            string selectedTime = RTimeDL.SelectedValue; //Selected Time ID
            string building = SelectBuildingDL.SelectedItem.Text;

            //calendar code:
            var dateStr = SelectDateTB.Text; //YYYY-MM-DD
            DateTime date; //attempts to parse the dateStr string into a DateTime object
            DateTime.TryParse(dateStr, out date);
            string dayOfWeekString = date.ToString("dddd");//print Monday-Sunday

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                String query = "UPDATE room_Sched SET " + dayOfWeekString + " =@room+'.'+ @courseCode+'.'+ @courseSection+'.'+@prof+'.'+@building +'.'+@selectedCollege WHERE ID = @selectedSched";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@courseCode", courseCode);
                    command.Parameters.AddWithValue("@courseSection", sec);
                    command.Parameters.AddWithValue("@prof", prof);
                    command.Parameters.AddWithValue("@room", room);
                    command.Parameters.AddWithValue("@selectedCollege", selectedCollege);
                    command.Parameters.AddWithValue("@selectedSchedvalue", selectedTimerealValue);
                    command.Parameters.AddWithValue("@selectedSched", selectedTime);
                    command.Parameters.AddWithValue("@building", building);

                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");

                    connection.Close();
                }
            }
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
        }
    }
}