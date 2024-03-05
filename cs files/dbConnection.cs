﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;//for sql connection

namespace WebApplication1.cs_files
{
    public static class dbConnection
    {
        private static readonly string connectionString = "Server=tcp:bagongserver.database.windows.net,1433;Initial Catalog=bagongdb;Persist Security Info=False;User ID=Frankdb;Password=Frank12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to the database: " + ex.Message);
                // You may want to handle exceptions differently based on your requirements
            }
            return connection;
        }
    }
}