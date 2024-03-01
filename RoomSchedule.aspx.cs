using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public partial class RoomSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGridView();

        }

        ///Code to connect db to gridview


        //To breakline the content

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
                SqlConnection storename = new SqlConnection("Server=tcp:bagongserver.database.windows.net,1433;Initial Catalog=bagongdb;Persist Security Info=False;User ID=Frankdb;Password=Frank12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                storename.Open();
                SqlCommand select = new SqlCommand("SELECT * FROM roomSchedtbl", storename);
                SqlDataAdapter adap = new SqlDataAdapter(select);
                DataSet ds = new DataSet();
                adap.Fill(ds);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds.Tables[0];

                    GridView1.DataBind();




                }
            }
            catch (Exception ex)
            {
                throw;

            }

        }
        protected void deployBTNclk(object sender, EventArgs e)
        {
            string courseCode = RCourseCodeTB.Text; //course code
            string sec = RSectionTB.Text; //section
            string prof = RProfTB.Text; //prof/instructor
            string bui = RBuildingTB.Text; //building
            string room = RRoomNumberTB.Text; //room number
            string selectedCollege = RFacultyDL.SelectedValue; //college value
            string selectedTime = RTimeDL.SelectedValue; //Selected Time
            var date = RCalendar1.SelectedDate;
            var dayOfWeek = date.ToString("dddd"); //Monday-Sunday



            SqlConnection storename = new SqlConnection("Server=tcp:bagongserver.database.windows.net,1433;Initial Catalog=bagongdb;Persist Security Info=False;User ID=Frankdb;Password=Frank12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            {
                String query = "UPDATE insertTesttbl Set @dayofWeek = @courseCode+'.'+ @courseSection+'.'+@prof+'.'+@building +'.'+@room+'.'+@selectedCollege WHERE ID = 12";

                using (SqlCommand command = new SqlCommand(query, storename))
                {
                    command.Parameters.AddWithValue("@courseCode", courseCode);
                    command.Parameters.AddWithValue("@courseSection", sec);
                    command.Parameters.AddWithValue("@prof", prof);
                    command.Parameters.AddWithValue("@building", bui);
                    command.Parameters.AddWithValue("@room", room);
                    command.Parameters.AddWithValue("@selectedCollege", selectedCollege);
                    command.Parameters.AddWithValue("@dayofWeek", dayOfWeek);




                    storename.Open();
                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }

            }

                



        }

        
    }
        
}
   
