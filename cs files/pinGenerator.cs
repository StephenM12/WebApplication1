using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebApplication1.cs_files
{
    public class pinGenerator
    {
        
        //private static Dictionary<string, DateTime> pins = new Dictionary<string, DateTime>();

        public static string GeneratePin()
        {
            // Generate a random 4-digit PIN
            Random rand = new Random();
            string pin = rand.Next(1000, 9999).ToString("D4");


            return pin;
        }

        public static string Generate_expiry_Time()
        {

            //generate expirytime for the pin
            DateTime expiryTime = DateTime.Now.AddMinutes(10);
            //convert date time to string
            string dateString = expiryTime.ToString("hh:mm:ss ");


            return dateString;

        }

        


    }
}