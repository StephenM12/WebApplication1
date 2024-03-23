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

            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();


            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:

                string selectQuery = "SELECT COUNT(*) FROM PinCode WHERE PinCode = @PinCode AND CONVERT(TIME, ExpiryTime) > CONVERT(TIME, GETDATE())";


                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {

                    // Check if PIN exists and has not expired in the database
                    command.Parameters.AddWithValue("@PinCode", pin);
                    int count = (int)command.ExecuteScalar();


                    //connection.Close();
                    //return true if pin IS NOT VALID
                    //return false if pin IN VALID
                    return count > 0;
                    
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