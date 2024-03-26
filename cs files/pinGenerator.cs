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
    public class pinGenerator
    {
        
        public static string GeneratePin()
        {
            // Generate a random 4-digit PIN
            Random rand = new Random();
            string pin = rand.Next(1000, 9999).ToString("D4");


            return pin;
        }


        public static DateTime Generate_expiry_Time()
        {
            
            // Open database connection
            SqlConnection connection = dbConnection.GetConnection();

            // Generate a PIN and 10min limit
            string pin = pinGenerator.GeneratePin();

            // Generate expiry time for the pin
            DateTime expiryTime = DateTime.Now.AddMinutes(10);


            if (connection.State == System.Data.ConnectionState.Open)
            {// Perform your database operations here:

                

                string insertQuery = "INSERT INTO pinCodes (PinCode, ExpiryTime) VALUES (@PinCode, @ExpiryTime)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@PinCode", pin);
                    command.Parameters.AddWithValue("@ExpiryTime", expiryTime);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
             

            return expiryTime;
        }




    }
}