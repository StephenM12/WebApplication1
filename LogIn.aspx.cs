using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//temporarytable
using System.Data;

//sql connection:
using System.Data.SqlClient;

using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LogInBtn_Click(object sender, EventArgs e)
        {

            //variable that will hold login values (username and password)
            string username, password;
            username = UsernameTB.Text;
            password = PasswordTB.Text;

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:

                
                String querry = "SELECT * FROM userInfo WHERE UserName= '" + username + "' AND UserPassword = '" + password + "'";

                    SqlDataAdapter sda = new SqlDataAdapter(querry, connection);

                    DataTable dtable = new DataTable();

                    sda.Fill(dtable);

                    if (dtable.Rows.Count > 0)
                    {
                        username = UsernameTB.Text;
                        password = PasswordTB.Text;

                        Response.Redirect("Home.aspx");


                    }

               
                connection.Close();
            }
            else
            {
                Console.WriteLine("Failed to connect to the database.");
            }
            
        }

        protected void CreateAccBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateAccount.aspx");
        }
    }
}