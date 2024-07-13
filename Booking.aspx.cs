using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using WebApplication1.cs_files;


namespace WebApplication1
{
    public partial class Booking : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateGridView();
            }
        }
        private void PopulateGridView()
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    //connection.Open();
                    string query = @"
                    SELECT 
                        RR.RequestID, 
                        RR.email, 
                        C.CourseCode, 
                        S.SectionName, 
                        I.InstructorName, 
                        RR.Faculty, 
                        RR.PurposeoftheRoom, 
                        B.BuildingName, 
                        R.RoomName, 
                        RR.StartDate, 
                        RR.EndDate, 
                        RR.startTime, 
                        RR.endTime, 
                        RR.pdfData, 
                        RR.DateSigned, 
                        RR.status
                    FROM RoomRequest RR
                    LEFT JOIN Courses C ON RR.CourseID = C.CourseID
                    LEFT JOIN Sections S ON RR.SectionID = S.SectionID
                    LEFT JOIN Instructors I ON RR.InstructorID = I.InstructorID
                    LEFT JOIN Buildings B ON RR.BuildingID = B.BuildingID
                    LEFT JOIN Rooms R ON RR.RoomID = R.RoomID";


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
                        GridView1.EmptyDataText = "No data available";
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the error
                    ErrorMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                string id = e.CommandArgument.ToString();
                UpdateStatus(id, "APPROVED");
            }
            else if (e.CommandName == "Reject")
            {
                string id = e.CommandArgument.ToString();
                UpdateStatus(id, "REJECTED");
            }
            else if (e.CommandName == "ViewPDF")
            {
                string id = e.CommandArgument.ToString();
                DisplayPDF(id);
            }

            // Rebind the GridView after processing the command
            PopulateGridView();
        }

        protected void btnUploadPDF_Click(object sender, EventArgs e)
        {
            if (FileUploadPDF.HasFile)
            {
                try
                {
                    byte[] pdfData = FileUploadPDF.FileBytes;

                    // Open database connection
                    SqlConnection connection = dbConnection.GetConnection();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        //connection.Open();
                        string query = "INSERT INTO Booking (PDFData, BookDate) VALUES (@PdfData, GETDATE())";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@PdfData", pdfData);

                        command.ExecuteNonQuery();
                    }

                    // After uploading, refresh the GridView
                    PopulateGridView();
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = "Error uploading PDF: " + ex.Message;
                }
            }
            else
            {
                ErrorMessage.Text = "Please select a PDF file to upload.";
            }
        }

        private void DisplayPDF(string id)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    //connection.Open();
                    string query = "SELECT PDFData FROM Booking WHERE BookingID = @RequestId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RequestId", id);

                    byte[] pdfData = (byte[])command.ExecuteScalar();

                    if (pdfData != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", pdfData.Length.ToString());
                        Response.BinaryWrite(pdfData);
                        Response.End();
                    }
                    else
                    {
                        ErrorMessage.Text = "Error: PDF data not found for the specified ID.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        private void UpdateStatus(string requestID, string status)
        {
            string query = "UPDATE RequestTBL SET Status = @Status WHERE RequestID = @RequestID";

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        //connection.Open();
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@RequestID", requestID);

                        cmd.ExecuteNonQuery();

                        // Send email based on status
                        if (status == "APPROVED")
                        {
                            SendApprovalEmail(requestID); // Send approval email
                        }
                        else if (status == "REJECTED")
                        {
                            SendRejectionEmail(requestID); // Send rejection email
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.Text = "Error updating status: " + ex.Message;
                    }
                }
            }
        }

        private void SendApprovalEmail(string requestID)
        {
            string recipientEmail = "jmandaya54@gmail.com"; // Change to recipient's email address
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "testingproject2001@gmail.com";
            string smtpPassword = "uxws wbem dspt pdjd";

            string subject = "Your request has been approved";
            string body = $"Your request with ID {requestID} has been approved.";

            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage(smtpUsername, recipientEmail, subject, body);
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Error sending approval email: " + ex.Message;
            }
        }

        private void SendRejectionEmail(string requestID)
        {
            string recipientEmail = "jmandaya54@gmail.com"; // Change to recipient's email address
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "testingproject2001@gmail.com";
            string smtpPassword = "uxws wbem dspt pdjd";

            string subject = "Your request has been rejected";
            string body = $"Your request with ID {requestID} has been rejected.";

            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage(smtpUsername, recipientEmail, subject, body);
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Error sending rejection email: " + ex.Message;
            }
        }
    }
}