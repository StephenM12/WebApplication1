using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
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
                    WHERE RR.status = 'pending' AND RR.Requester_Faculty = @userFaculty";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userFaculty", user_Identity.user_Faculty);
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
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = GridView1.Rows[rowIndex];
                int requestId = Convert.ToInt32(GridView1.DataKeys[rowIndex].Value);
                hiddenRequestId.Value = requestId.ToString();

               
                string requestedByEmail = row.Cells[1].Text;
                HiddenRequester_Email.Value = requestedByEmail;
                System.Diagnostics.Debug.WriteLine(requestedByEmail);

                if (e.CommandName == "Accept" )
                {
                    
                    //string newStatus = e.CommandName == "Accept" ? "Accepted" : "Rejected";
                    string ReqStatus = e.CommandName;
                    UpdateRoomRequestStatus(requestId, "Accepted", user_Identity.user_Email);
                    SendEmailNotification(requestedByEmail, ReqStatus);
                }
                if (e.CommandName == "Reject")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showRejectModal", "$('#rejectModal').modal('show');", true);
                    return; // Return early to avoid updating status until the remark is submitted
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RowCommand: {ex.Message}");
            }
            finally
            {
                // Rebind the data to refresh the GridView
                BindScheduleData(sender, e);
            }
        }

        protected void btnSubmitRemark_Click(object sender, EventArgs e)
        {
            int requestId = Convert.ToInt32(hiddenRequestId.Value);
            string remark = remarkText.Text;
            string requestedByEmail = HiddenRequester_Email.Value;
            System.Diagnostics.Debug.WriteLine(requestedByEmail);

            if (string.IsNullOrWhiteSpace(remark))
            {
                UpdateRoomRequestStatus(requestId, "Rejected", user_Identity.user_Email);
                SendEmailNotification(requestedByEmail, "Rejected");

            }
            else 
            {
                UpdateRoomRequestStatus(requestId, "Rejected", user_Identity.user_Email);
                SendEmailNotification(requestedByEmail, "Rejected");

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "testingproject2001@gmail.com";
                string smtpPassword = "uxws wbem dspt pdjd";

                string subject = "Additional Remarks From Your Room Request Status";
                string body = $"{remark}\n\nThank you.";


                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);
                    emailSender.SendEmail("testingproject2001@gmail.com", requestedByEmail, subject, body);

                }

            }
            
            // Rebind the data and close the modal
            BindScheduleData(sender, e);
            ScriptManager.RegisterStartupScript(this, GetType(), "hideRejectModal", "$('#rejectModal').modal('hide');", true);
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
            string body = $"Dear User,\n\nYour room request has been {status} by the Immediate Superior.";

            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);
                emailSender.SendEmail("testingproject2001@gmail.com", email, subject, body);


                
            }
        }

        private void UpdateRoomRequestStatus(int requestId, string status, string updatedByEmail)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                string sql = "UPDATE RoomRequest SET status = @status, DateApproved = @DateApproved, UpdatedBy = @UpdatedBy WHERE RequestID = @requestId";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@requestId", requestId);
                    cmd.Parameters.AddWithValue("@DateApproved", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedBy", updatedByEmail); 

                    cmd.ExecuteNonQuery();
                }
            }
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //download function
        //public void DownloadFile(int requestId)
        //{
        //    using (SqlConnection connection = dbConnection.GetConnection())
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT FileName, ContentType, FileData FROM RoomRequest WHERE RequestID = @FileID", connection))
        //        {
        //            cmd.Parameters.AddWithValue("@FileID", requestId);
        //            //connection.Open();

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    string fileName = reader["FileName"].ToString();
        //                    string contentType = reader["ContentType"].ToString();
        //                    byte[] fileData = (byte[])reader["FileData"];

        //                    // Send the file to the user
        //                    Response.Clear();
        //                    Response.ContentType = contentType;
        //                    Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
        //                    Response.BinaryWrite(fileData);
        //                    Response.Flush();  // Flush the response to ensure the download starts
        //                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Avoid ThreadAbortException
        //                }
        //            }
        //        }
        //    }
        //}

        protected string GetFileIcon(string contentType)
        {
            switch (contentType.ToLower())
            {
                case "image/jpeg":
                case "image/png":
                    return "~/images/img.png"; // Replace with your image path
                case "application/pdf":
                    return "~/images/pdf.png"; // Replace with your image path
                case "application/msword":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return "~/images/docx.png"; // Replace with your image path
                case "application/vnd.ms-excel":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "~/images/xlsx.png"; // Replace with your image path
                                                      // Add more cases as needed for different file types
                default:
                    return "~/images/default-icon.png"; // Default icon for unknown types
            }
        }





    }
}