//sql connection:
using System.Data.SqlClient;

namespace WebApplication1.cs_files
{
    public class createAcc_Validation
    {
        public static bool isEmail_Valid(string userEmail)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            // Check if the email already exists
            string queryCheckEmail = "SELECT COUNT(*) FROM userInfo WHERE Email = @userEmail";
            using (SqlCommand commandCheckEmail = new SqlCommand(queryCheckEmail, connection))
            {
                commandCheckEmail.Parameters.AddWithValue("@userEmail", userEmail);

                int emailCount = (int)commandCheckEmail.ExecuteScalar();
                if (emailCount > 0)
                {
                    return false;
                }
                connection.Close();
            }
            return true;
        }

        public static bool isPass_Valid(string userPass)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            // Check if the username already exists
            string queryCheckUsername = "SELECT COUNT(*) FROM userInfo WHERE UserName = @userName";
            using (SqlCommand commandCheckUsername = new SqlCommand(queryCheckUsername, connection))
            {
                commandCheckUsername.Parameters.AddWithValue("@userName", userPass);
                int usernameCount = (int)commandCheckUsername.ExecuteScalar();
                if (usernameCount > 0)
                {
                    return false;
                }
                connection.Close();
            }

            return true;
        }
    }
}