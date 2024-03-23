using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class create_account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CreateAccBtn_Click(object sender, EventArgs e)
        {

            string userName = UsernameID.Text;
            string userFName = FirstNameID.Text;
            string userLName = LastNameID.Text;
            string userEmail = EmailID.Text;
            string userPass = PasswordID.Text;

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:


                String query = "INSERT INTO userInfo (UserName, UserPassword, FirstName, LastName, Email) VALUES (@userName, @userPass, @userFName, @userLName, @userEmail); ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    try
                    {
                        command.Parameters.AddWithValue("@userName", userName);
                        command.Parameters.AddWithValue("@userFName", userFName);
                        command.Parameters.AddWithValue("@userLName", userLName);
                        command.Parameters.AddWithValue("@userEmail", userEmail);
                        command.Parameters.AddWithValue("@userPass", userPass);

                        command.ExecuteNonQuery();
                        connection.Close();

                    }
                    catch (SqlException ex) 
                    {


                        Response.Write("Pls enter a different Email/ Username");
                    }

                    



                    

                }


                }
                //Response.Redirect("LogIn.aspx");
        }

    }
}