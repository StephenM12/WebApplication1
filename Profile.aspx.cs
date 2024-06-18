using System;

//sql connection:
using System.Data.SqlClient;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PNameTB.Text = user_Identity.user_FName;
            PUsernameTB.Text = user_Identity.userName;
            PEmailTB.Text = user_Identity.user_Email;
        }

        protected void save_Email_Changes(object sender, EventArgs e)
        {
            string newEmail = ProfileEmail.Text;

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                string query = "UPDATE Email = @userNew_email WHERE UserID = @userID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@userNew_email", newEmail);
                        command.Parameters.AddWithValue("@userID", user_Identity.userID);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch
                    {
                        Response.Write("Unable to Change Email");
                    }
                }
            }
        }

        protected void save_password_Changes(object sender, EventArgs e)
        {

            string newPass = PNewPassTB.Text;

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:
                string query = "UPDATE UserPassword = @userNew_pass WHERE UserID = @userID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@userNew_pass", newPass);
                        command.Parameters.AddWithValue("@userID", user_Identity.userID);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch
                    {
                        Response.Write("Unable to Change password");
                    }
                }
            }


        }
    }
}