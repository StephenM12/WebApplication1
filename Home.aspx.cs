using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userFname = user_Identity.user_FName;
            Label1.Text = "Welcome " + userFname;

            if (!IsPostBack)
            {
                //LoadRooms();
                BindRooms();
            }
        }

        public class Room
        {
            public string Building { get; set; }
            public string RoomID { get; set; }
            public string RoomNumber { get; set; }
            public string Status { get; set; }
            public string CardClass { get; set; }
        }

        //private void BindRooms()
        //{
        //    var rooms = new List<Room>
        //    {
        //        new Room { Building = "Rizal", RoomID = "room1", RoomNumber = "R101", Status = "Available", CardClass = "bg-success" },
        //        new Room { Building = "Rizal", RoomID = "room2", RoomNumber = "R102", Status = "Occupied", CardClass = "bg-danger" },
        //        // Add more rooms as needed
        //    };

        //    RepeaterRooms.DataSource = rooms;
        //    RepeaterRooms.DataBind();
        //}

        private void BindRooms()
        {
            string query = "SELECT RoomID, RoomName FROM Rooms";

            List<Room> rooms = new List<Room>();

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //string status = reader["Status"].ToString();
                            //string cardClass = status == "Available" ? "bg-success" : "bg-danger";
                            string cardClass = "bg-success";
                            string status = "Available";
                            string Building = "Rizal";

                            rooms.Add(new Room
                            {
                                Building = Building,
                                RoomID = reader["RoomID"].ToString(),
                                RoomNumber = reader["RoomName"].ToString(),
                                Status = status,
                                CardClass = cardClass

                            });
                        }
                    }
                }

                RepeaterRooms.DataSource = rooms;
                RepeaterRooms.DataBind();

            }

            
        }

       
    }
}