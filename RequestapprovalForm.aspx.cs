using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace RoomRequestForm
{
    public partial class RequestSignatureForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindScheduleData(sender, e);
            }
        }

        protected void BindScheduleData(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                   

                    string query = @"
                    SELECT
                        *
                    FROM RoomRequest RR
                    WHERE RR.status = 'pending'";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                    else
                    {
                        // No data found, display a message or handle accordingly
                        GridView1.EmptyDataText = "No Pending Request";
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the error
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Accept" || e.CommandName == "Reject")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = GridView1.Rows[rowIndex];
                int requestId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);

                string newStatus = e.CommandName == "Accept" ? "Accepted" : "Rejected";

                UpdateRoomRequestStatus(requestId, newStatus);


                //send email code here:
                string email = row.Cells[0].Text;
                SendEmailNotification(email, newStatus);





                BindScheduleData(sender, e);




            }
        }

        private void SendEmailNotification(string email, string status)
        {
            // Your email sending logic here
            // Example: Using SmtpClient to send an email
            //web credentials
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "testingproject2001@gmail.com";
            string smtpPassword = "uxws wbem dspt pdjd";

            string subject = "Update on Your Room Request Status";
            string body = $"Dear User,\n\nYour room request has been {status}.\n\nBest regards,\nYour Team";

            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);
                emailSender.SendEmail("testingproject2001@gmail.com", email, subject, body);


                
            }
        }

        private void UpdateRoomRequestStatus(int requestId, string status)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string sql = "UPDATE RoomRequest SET status = @status WHERE RequestID = @requestId";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@requestId", requestId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}