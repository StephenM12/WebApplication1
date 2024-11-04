using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class ViewUseraspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ModalPopup.RegisterModalHtml(Page);
                BindGridView(sender, e);

                dropdown_datas(sender, e);
            }
        }

        protected void BindGridView(object sender, EventArgs e)
        {
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    //connection.Open();
                    string query = @" SELECT * FROM userInfo";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();

                        //Get the row count
                        int rowCount = dataTable.Rows.Count;

                        // Optionally display row count in a Label
                        Label1.Text = "Users(" + rowCount.ToString() + ")";
                    }
                    else
                    {
                        GridView1.EmptyDataText = "No Data";
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the error
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowModal" || e.CommandName == "RemoveUser")
            {
                ResetModalFields();

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row = GridView1.Rows[rowIndex];

                // Retrieve the UserID from the DataKeys collection
                string userID = GridView1.DataKeys[rowIndex].Value.ToString();

                if (e.CommandName == "ShowModal")
                {
                    HiddenField1.Value = userID;
                    if (!string.IsNullOrEmpty(HiddenField1.Value))
                    {
                        //To print the id in the output in vs
                        System.Diagnostics.Debug.WriteLine("HiddenField Value: " + HiddenField1.Value);
                    }

                    string query = @"SELECT * FROM userInfo WHERE UserID = @UserID";

                    using (SqlConnection connection = dbConnection.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtUserName.Text = reader["UserName"].ToString();
                                    txtFirstName.Text = reader["FirstName"].ToString();
                                    txtLastName.Text = reader["LastName"].ToString();
                                    txtEmail.Text = reader["Email"].ToString();
                                    txtUserPassword.Text = reader["UserPassword"].ToString();
                                    ddlUserLevel.SelectedValue = reader["UserLevel"].ToString();
                                }
                            }
                        }
                    }

                    // Show the modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowEditModal", "$('#editUserModal').modal('show');", true);
                }
                else if (e.CommandName == "RemoveUser")
                {
                    // SQL query to delete the user from the userInfo table
                    string deleteQuery = @"DELETE FROM userInfo WHERE UserID = @UserID";

                    using (SqlConnection connection = dbConnection.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                        {
                            // Add the UserID parameter to the SQL command
                            cmd.Parameters.AddWithValue("@UserID", userID);

                            // Execute the DELETE command and check if any rows were affected
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                string msg = "User removed successfully.";
                                ModalPopup.ShowMessage_(Page, msg, "Notification");

                                // If successful, show a success message and rebind the GridView
                                //System.Diagnostics.Debug.WriteLine("User with ID " + userID + " has been removed successfully.");

                                //lblMessage.CssClass = "alert alert-success";
                            }
                            else
                            {
                                string msg = "Error occurred while trying to remove the user.";
                                ModalPopup.ShowMessage_(Page, msg, "Notification");
                                // If no rows affected, display an error message
                                //lblMessage.Text = "Error occurred while trying to remove the user.";
                                //lblMessage.CssClass = "alert alert-danger";
                            }
                        }
                    }

                    // Rebind the GridView to refresh the data
                    BindGridView(sender, e);
                }
            }
        }

        protected void CreateAccBtn_Click(object sender, EventArgs e)
        {
            // Ensure that the page validation passes before processing
            if (Page.IsValid)
            {
                string username = UsernameID.Text.Trim();
                string password = PasswordID.Text.Trim();
                string firstName = FirstNameID.Text.Trim();
                string lastName = LastNameID.Text.Trim();
                string email = EmailID.Text.Trim();
                int userLevel = int.Parse(userlvl.SelectedValue);
                string faculty = RFacultyDL.SelectedItem.Text;

                // Define the SQL query to insert data into the userInfo table
                string query = @"INSERT INTO userInfo (UserName, UserPassword, FirstName, LastName, Email, UserLevel, Faculty)
                     VALUES (@UserName, @UserPassword, @FirstName, @LastName, @Email, @UserLevel, @Faculty)";

                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@UserName", username);
                        command.Parameters.AddWithValue("@UserPassword", password);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@UserLevel", userLevel);
                        command.Parameters.AddWithValue("@Faculty", faculty);


                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                //    txtUserName.Text = string.Empty;
                                //    txtFirstName.Text = string.Empty;
                                //    txtLastName.Text = string.Empty;
                                //    txtEmail.Text = string.Empty;
                                //    txtUserPassword.Text = string.Empty;
                                //    ddlUserLevel.SelectedIndex = 0;

                                UsernameID.Text = string.Empty;
                                FirstNameID.Text = string.Empty;
                                LastNameID.Text = string.Empty;
                                EmailID.Text = string.Empty;
                                PasswordID.Text = string.Empty;
                                ConfirmPasswordID.Text = " ";
                                userlvl.SelectedIndex = 0;

                                // Update successful: close modal and display success message
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal", "$('#AddUser').modal('hide');", true);
                                BindGridView(sender, e);

                                ModalPopup.ShowMessage_(Page, "User account created successfully!", "Success");
                            }
                        }
                        catch (SqlException ex)
                        {
                            // Handle any errors that occur during the insert
                            // Log or show an error message
                            throw new Exception("Error inserting user data: " + ex.Message);
                        }
                    }
                }
            }
        }
        protected void btnAddFaculty_Click(object sender, EventArgs e)
        {
            string facultyCode = txtNewFacultyName.Text.Trim().ToUpper(); // Get the faculty code from the TextBox

            // Check if the faculty code is not empty
            if (string.IsNullOrEmpty(facultyCode))
            {
                // Optionally show a message to the user
                Label1facult.Text = "Faculty code cannot be empty.";
                return;
            }

            using (SqlConnection connection = dbConnection.GetConnection())
            {

                try
                {
                    //conn.Open();
                    string query = "INSERT INTO Faculty (FacultyCode) VALUES (@FacultyCode)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Use parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@FacultyCode", facultyCode);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the insertion was successful
                        if (rowsAffected > 0)
                        {
                            dropdown_datas(sender, e);

                            Label1facult.Text = facultyCode + " added successfully.";
                            Label1facult.CssClass = "alert alert-success";
                            Label1facult.Visible = true;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", @"
                            setTimeout(function() {
                                $('#addFacultyModal').modal('hide');
                            }, 2000);

                            ", true);


                            txtNewFacultyName.Text = string.Empty;
                        }
                        else
                        {
                            Label1facult.Text = "Error adding faculty.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (if needed) and display an error message
                    Label1facult.Text = "An error occurred: " + ex.Message;
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Visible = false;

            txtUserName.Enabled = true;
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtEmail.Enabled = true;
            txtUserPassword.Enabled = true;
            ddlUserLevel.Enabled = true;

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
                string password = txtUserPassword.Text;
                int userLevel = Convert.ToInt32(ddlUserLevel.SelectedValue);

                using (SqlConnection connection = dbConnection.GetConnection())
                {
                    string query = "UPDATE userInfo SET UserName = @UserName, UserPassword = @UserPassword, FirstName = @FirstName, LastName = @LastName, Email = @Email, UserLevel = @UserLevel  WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Add the parameters to the command
                        cmd.Parameters.AddWithValue("@UserID", HiddenField1.Value);
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@UserPassword", password);
                        cmd.Parameters.AddWithValue("@UserLevel", userLevel);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Update successful: close modal and display success message
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal", "$('#editUserModal').modal('hide');", true);

                            string msg = "User: " + userName + " info, updated successfully.";
                            ModalPopup.ShowMessage_(Page, msg, "Notification");

                            BindGridView(sender, e);
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

        private void ResetModalFields()
        {
            btnEdit.Visible = true;

            txtUserName.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtEmail.Enabled = false;
            txtUserPassword.Enabled = false;
            ddlUserLevel.Enabled = false;

            btnUpdate.Visible = false; // Hide the update button
        }
        protected void dropdown_datas(object sender, EventArgs e)
        {
            DropdownFiller filler = new DropdownFiller();
            filler.PopulateFaculty(RFacultyDL);
        }
    }
}