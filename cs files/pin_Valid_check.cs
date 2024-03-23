using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//sql connection:
using System.Data.SqlClient;
using System.Data;
using WebApplication1.cs_files;



namespace WebApplication1.cs_files
{
    public class pin_Valid_check
    {
        


        public static bool IsPinValid(string pin)
        {
            DateTime expiryTime;

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();


            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:

                
                string selectQuery = "SELECT ExpiryTime FROM ayoko_na WHERE PinCode = @PinCode_";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {

                    // Check if PIN exists and has not expired in the database
                    command.Parameters.AddWithValue("@PinCode_", pin);
                    

                    // Execute the query and retrieve the expiry time
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            expiryTime = reader.GetDateTime(0);
                        }
                        else
                        {
                            // Pin not found in the database
                            return false;
                        }
                    }

                    connection.Close();
                    
                    return DateTime.Now < expiryTime;

                }

               
            }
            else
            {
                Console.WriteLine("Database connection is not open.");
                return false; // Return false indicating failure
            }
        }

    }
}