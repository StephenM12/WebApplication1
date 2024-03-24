using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;



namespace WebApplication1.cs_files
{
    public class user_Identity
    {

        
        public static int userID { get; set; }
        public static string user_FName { get; set; }


        //get user ID with email
        public static int verify_User_email(string userEmail)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                string query = "SELECT UserID FROM  userInfo WHERE Email = @userEmail";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);

                    // Execute the query and get the result
                    int count = (int)command.ExecuteScalar();
                    connection.Close();

                    user_Identity.userID = count;
                    return count;
                }

            }
            else { return 0; }
            
        }


        public static string verify_UserName(string userName)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                string query = "SELECT FirstName FROM userInfo WHERE UserName = @userName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@userName", userName);
                    object result = command.ExecuteScalar();
                    connection.Close();

                    if (result != null)
                    {
                        user_Identity.user_FName = result.ToString();
                        return result.ToString(); // Return the first name
                    }
                    else
                    {
                        // Return null or empty string if the first name is not found
                        return string.Empty;
                    }

                }

            }
            else { return "Failed";}

                   
        }
    }
}