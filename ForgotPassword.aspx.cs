using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//to send email
//using System.Net.Mail;

//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class ForgotPassword1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void verify_Email_BTN(object sender, EventArgs e)
        {
            string verify_Email = EmailTB.Text;


            //verify email

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:

                string query = "SELECT COUNT(*) FROM userInfo WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    //get Email
                    command.Parameters.AddWithValue("@Email", verify_Email);

                    // Check if the email exists in the database
                    int count = (int)command.ExecuteScalar();

                   
                    if (count > 0) //if yes
                    {

                        //hold whose user logged
                        user_Identity.verify_User_email(verify_Email);

                        // Generate a PIN
                        string pin = pinGenerator.GeneratePin();
                        

                        //generate time for the pin
                        pinGenerator.Generate_expiry_Time();

                        //web credentials
                        string smtpServer = "smtp.gmail.com";
                        int smtpPort = 587;
                        string smtpUsername = "testingproject2001@gmail.com";
                        string smtpPassword = "uxws wbem dspt pdjd";


                        //email content
                        string subject = "Reset your password";
                        string body = $"Dear {user_Identity.user_FName},\n\nWe've received a request from you to reset the password for your account. To ensure the security of your account, we require you to verify your email address. Please use this verification code provided below to complete the verification process and gain access to your account.\n\nVerification Code: {pin}\n\nThank you for helping us maintain the security of your account.\n\nBest regards,\nECMS";


                        //send PIN to email
                        try
                        {

                            emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);

                            emailSender.SendEmail("testingproject2001@gmail.com", verify_Email, subject, body);


                            Response.Redirect("Verification.aspx");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());

                        }
                        finally
                        {
                            // Close the connection, whether an exception occurred or not
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                        }

                    }
                    else
                    {
                        //Response.Write("Email does not exist in the database.");
                        Label1.Text = "Email does not exist in the database.";


                    }

                    
                }

            }



            

            






        }
    }
}