using System;

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
            if (!IsPostBack)
            {
                try
                {
                    DbInitializer.EnsureTablesAndTriggersExist();

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    ModalPopup.ShowMessage(Page, msg, "Alert!");
                }
            }
        }

        protected void LogInBtn_Click(object sender, EventArgs e)
        {
            //variable that will hold login values (username and password)
            string username, password;
            username = UsernameTB.Text;
            password = PasswordTB.Text;

            try
            {
                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    // Perform your database operations here:
                    String querry = "SELECT * FROM userInfo WHERE UserName= '" + username + "' AND UserPassword = '" + password + "'";

                    SqlDataAdapter sda = new SqlDataAdapter(querry, connection);

                    DataTable dtable = new DataTable();

                    sda.Fill(dtable);

                    //means user is authenticated
                    if (dtable.Rows.Count > 0)
                    {
                        user_Identity.verify_UserName(username, connection);

                        username = UsernameTB.Text;
                        password = PasswordTB.Text;

                        Response.Redirect("Home.aspx");
                    }
                    else
                    {
                        ModalPopup.ShowMessage(Page, "Invalid Username or Password", "Alert!");
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                ModalPopup.ShowMessage(Page, msg, "Alert!");
            }
           
            
        }

        protected void CreateAccBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateAccount.aspx");
        }

        protected void RRBtn_Click(object sender, EventArgs e)
        {
            // Redirect to another page
            Response.Redirect("~/RRForm.aspx");
        }

    }
}