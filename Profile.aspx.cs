using System;

//sql connection:
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ModalPopup.RegisterModalHtml(Page);

                PNameTB.Text = user_Identity.user_FName;
                PUsernameTB.Text = user_Identity.userName;
                PEmailTB.Text = user_Identity.user_Email;
                

                txtUserName.Text = user_Identity.userName;
                txtFirstName.Text = user_Identity.user_FName;
                txtLastName.Text = user_Identity.user_LName;
                txtEmail.Text = user_Identity.user_Email;
            }

           

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(user_Identity.userID);

            btnEdit.Visible = false;

            txtUserName.Enabled = true;
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtEmail.Enabled = true;

            btnUpdate.Visible = true;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtUserName.Text;
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                string email = txtEmail.Text;
                //string password = txtUserPassword.Text;
                //int userLevel = Convert.ToInt32(ddlUserLevel.SelectedValue);

                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    string query = "UPDATE userInfo SET UserName = @UserName, FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Add the parameters to the command
                        cmd.Parameters.AddWithValue("@UserID", user_Identity.userID);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        //cmd.Parameters.AddWithValue("@UserPassword", password);
                        //cmd.Parameters.AddWithValue("@UserLevel", userLevel);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Update successful: close modal and display success message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal", "$('#editUserModal').modal('hide');", true);

                            string msg = "User: " + userName + " info, updated successfully.";
                            ModalPopup.ShowMessage_(Page, msg, "Notification");

                            
                        }
                        else
                        {
                            // Update failed: display error message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", "alert('Error: No rows were updated.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                string msg = "Error: " + ex.Message;
                ModalPopup.ShowMessage_(this.Page, msg, "Alert!");
            }
        }

        protected void SavePasswordBtn_Click(object sender, EventArgs e)
        {
            string currentPassword = CurrentPasswordTB.Text;
            string newPassword = NewPasswordTB.Text;
            string confirmPassword = ConfirmPasswordTB.Text;

            // Check if the new password and confirm password match
            if (newPassword != confirmPassword)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('New password and confirm password do not match.');", true);
                ModalPopup.ShowMessage(this.Page, "New password and confirm password do not match.", "Alert!");
                return;
            }

            // Get the username from the current user session or context
            string userName = user_Identity.userName;

            // Check if the current password entered is correct
            if (!IsCurrentPasswordValid(userName, currentPassword))
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Current password is incorrect.');", true);
                ModalPopup.ShowMessage(this.Page, "Current password is incorrect.", "Alert!");
                return;
            }

            // Update the password in the database
            bool updateSuccessful = UpdatePassword(userName, newPassword);

            if (updateSuccessful)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Password updated successfully.');", true);
                ModalPopup.ShowMessage(this.Page, "Password updated successfully.", "Notification!");
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('An error occurred while updating the password.');", true);
                ModalPopup.ShowMessage(this.Page, "An error occurred while updating the password.", "Alert!");
            }
        }

        // Method to validate current password
        private bool IsCurrentPasswordValid(string userName, string currentPassword)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;


            using (SqlConnection connection = dbConnection.GetConnection())
            {
                string query = "SELECT UserPassword FROM userInfo WHERE UserName = @UserName";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", userName);

                //conn.Open();
                object result = cmd.ExecuteScalar();
                //conn.Close();

                return result != null && result.ToString() == currentPassword;
            }
        }

        // Method to update password in the database
        private bool UpdatePassword(string userName, string newPassword)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;

            using (SqlConnection connection = dbConnection.GetConnection())
            {
                string query = "UPDATE userInfo SET UserPassword = @NewPassword WHERE UserName = @UserName";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                cmd.Parameters.AddWithValue("@UserName", userName);

                //conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                //conn.Close();

                return rowsAffected > 0;
            }
        }


        private void ResetModalFields()
        {
            btnEdit.Visible = true;

            txtUserName.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtEmail.Enabled = false;
            //txtUserPassword.Enabled = false;
            //ddlUserLevel.Enabled = false;

            btnUpdate.Visible = false; // Hide the update button
        }
    }
}