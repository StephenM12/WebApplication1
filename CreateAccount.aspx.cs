using System;

//sql connection:
using System.Data.SqlClient;
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

            string userLEVEL = userlvl.SelectedValue;
            int userLevelInt = int.Parse(userLEVEL);


            bool emailCheck = createAcc_Validation.isEmail_Valid(userEmail);
            bool userNCheck = createAcc_Validation.isEmail_Valid(userName);

            if (emailCheck==true&&userNCheck==true)
            {

                // Open database connection
                SqlConnection connection = dbConnection.GetConnection();

                if (connection.State == System.Data.ConnectionState.Open)
                {// Perform your database operations here:
                    String query = "INSERT INTO userInfo (UserName, UserPassword, FirstName, LastName, Email, UserLevel) VALUES (@userName, @userPass, @userFName, @userLName, @userEmail, @UserLevel); ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@userName", userName);
                            command.Parameters.AddWithValue("@userFName", userFName);
                            command.Parameters.AddWithValue("@userLName", userLName);
                            command.Parameters.AddWithValue("@userEmail", userEmail);
                            command.Parameters.AddWithValue("@userPass", userPass);
                            command.Parameters.AddWithValue("@UserLevel", userLevelInt);

                            command.ExecuteNonQuery();
                            connection.Close();

                        }
                        catch (SqlException ex)
                        {
                            Response.Write("Pls enter a different Email/ Username");
                        }
                        
                    }
                }

                Response.Write("successfully account created");
                Response.Redirect("LogIn.aspx");



            }

            if (emailCheck == false){ Response.Write("Email already exists. Please use a different email."); }
            if (userNCheck == false){ Response.Write("Username already exists. Please choose a different username."); }
            else { Response.Write("Error!"); }


        }
    }
}