//sql connection:
using System.Data.SqlClient;

namespace WebApplication1.cs_files
{
    public class changePassword
    {
        

        public static bool changePass(int id, string newPass)
        {

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                string query = "UPDATE userInfo SET UserPassword = @newPassword WHERE UserID = @userID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userID", id);
                    command.Parameters.AddWithValue("@newPassword", newPass);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            else { return false; }
        }
    }
}