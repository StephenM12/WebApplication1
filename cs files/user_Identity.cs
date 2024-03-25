//sql connection:
using System.Data.SqlClient;

namespace WebApplication1.cs_files
{
    public class user_Identity
    {
        public static int userID { get; set; }
        public static string user_FName { get; set; }
        public static string user_LName { get; set; }
        public static string user_Email { get; set; }

        //get user ID with email
        public static int verify_User_email(string userEmail)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                string query = "SELECT * FROM  userInfo WHERE Email = @userEmail";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);

                    // Execute the SQL command and get a data reader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop through each row in the result set
                        while (reader.Read())
                        {
                            user_Identity.userID = reader.GetInt32(0);
                            user_Identity.user_FName = reader.GetString(3);
                            user_Identity.user_LName = reader.GetString(4);
                            user_Identity.user_Email = reader.GetString(5);
                        }
                        connection.Close();
                    }

                    return 0;
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
                string query = "SELECT * FROM userInfo WHERE UserName = @userName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userName", userName);

                    // Execute the SQL command and get a data reader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop through each row in the result set
                        while (reader.Read())
                        {
                            user_Identity.userID = reader.GetInt32(0);
                            user_Identity.user_FName = reader.GetString(3);
                            user_Identity.user_LName = reader.GetString(4);
                            user_Identity.user_Email = reader.GetString(5);
                        }
                        connection.Close();
                    }

                    return "User is Verified";
                }
            }
            else { return "Failed"; }
        }
    }
}