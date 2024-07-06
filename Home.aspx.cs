using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
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
            }

        }

        private DataTable GetRoomsData()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string query = "SELECT RoomID, RoomName FROM Rooms";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                da.Fill(dt);
            }
            return dt;

        }

        //private void LoadRooms()
        //{
        //    DataTable roomsTable = GetRoomsData();
        //    roomsRepeater.DataSource = roomsTable;
        //    roomsRepeater.DataBind();

        //    foreach (RepeaterItem item in roomsRepeater.Items)
        //    {
        //        int roomId = Convert.ToInt32(DataBinder.Eval(item.DataItem, "RoomID"));
        //        string cardId = "card" + roomId;
        //        string cardTextId = "cardText" + roomId;

        //        var card = item.FindControl(cardId) as System.Web.UI.HtmlControls.HtmlGenericControl;
        //        var cardText = item.FindControl(cardTextId) as System.Web.UI.WebControls.Literal;

        //        bool isAvailable = CheckRoomAvailability(roomId);

        //        if (card != null)
        //        {
        //            card.Attributes["class"] = isAvailable ? "card text-white bg-success" : "card text-white bg-danger";
        //        }

        //        if (cardText != null)
        //        {
        //            cardText.Text = isAvailable ? "Available" : "Not Available";
        //        }
        //    }
        //}

        

        public bool CheckRoomAvailability(int roomId)
        {
            bool isAvailable = true;
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string query = "SELECT COUNT(*) FROM Schedule WHERE RoomID = @RoomID ";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@RoomID", roomId);
                //cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);

                //connection.Open();
                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    isAvailable = false;
                }
            }
            return isAvailable;
        }



    }
}