﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//to send email
using System.Net.Mail;

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

                string insertQuery = "INSERT INTO pinCode (PinCode, ExpiryTime) VALUES (@PinCode, @ExpiryTime)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    //get value from textbox to db
                    command.Parameters.AddWithValue("@Email", verify_Email);

                    // Execute the query
                    int count = (int)command.ExecuteScalar();

                   

                    // Check if the email exists in the database
                    if (count > 0) //if yes
                    {

                        // Generate a PIN and 10min limit
                        string pin = pinGenerator.GeneratePin();
                        string expiryTime = pinGenerator.Generate_expiry_Time();


                        

                        SqlCommand command_ = new SqlCommand(insertQuery, connection);
                        command_.Parameters.AddWithValue("@PinCode", pin);
                        command_.Parameters.AddWithValue("@ExpiryTime", expiryTime); //still not final
                        command_.ExecuteNonQuery();

                        

                        //send PIN to email
                        try
                        {
                           
                            MailMessage mail = new MailMessage();
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                            mail.From = new MailAddress("testingproject2001@gmail.com");
                            mail.To.Add(verify_Email);
                            mail.Subject = "testing 1.1";
                            mail.Body = "Your code is: "+ pin;
                            smtp.Port = 587;
                            smtp.Credentials = new System.Net.NetworkCredential("testingproject2001@gmail.com", "uxws wbem dspt pdjd");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);



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
                        Response.Write("Email does not exist in the database.");
                        
                    }

                    
                }

            }



            

            






        }
    }
}