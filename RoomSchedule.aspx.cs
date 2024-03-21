using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class RoomSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGridView();

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

        ///Code to connect db to gridview
        private void BindGridView()
        {

            try
            {
                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand select = new SqlCommand("SELECT * FROM savedschedules1", connection);
                    SqlDataAdapter adap = new SqlDataAdapter(select);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GridView1.DataSource = ds.Tables[0];

                        GridView1.DataBind();

                    }

                    connection.Close();
                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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

            //calendar code:
            var dateStr = SelectDateTB.Text; //YYYY-MM-DD
            DateTime date; //attempts to parse the dateStr string into a DateTime object
            DateTime.TryParse(dateStr, out date);
            string dayOfWeekString = date.ToString("dddd");//print Monday-Sunday

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:

                String query = "UPDATE savedschedules1 SET " + dayOfWeekString + " = @courseCode+'.'+ @courseSection+'.'+@prof+'.'+@building +'.'+@room+'.'+@selectedCollege WHERE ID = @selectedSched";
                using (SqlCommand command = new SqlCommand(query, connection))
                {


                    command.Parameters.AddWithValue("@courseCode", courseCode);
                    command.Parameters.AddWithValue("@courseSection", sec);
                    command.Parameters.AddWithValue("@prof", prof);
                    command.Parameters.AddWithValue("@room", room);
                    command.Parameters.AddWithValue("@selectedCollege", selectedCollege);
                    command.Parameters.AddWithValue("@selectedSchedvalue", selectedTimerealValue);
                    command.Parameters.AddWithValue("@selectedSched", selectedTime);

                    
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
   
