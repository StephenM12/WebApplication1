using System;

using WebApplication1.cs_files;

namespace WebApplication1
{
    public partial class Verification : System.Web.UI.Page
    {
        public static DateTime timeNow { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            DateTime currentTime = DateTime.Now;
            Verification.timeNow = currentTime;

            myParagraph.InnerText = "We've sent a verification code to " + user_Identity.user_Email;


        }
        protected void resend_Code(object sender, EventArgs e)
        {
            
            if (DateTime.Now >= Verification.timeNow)
            {
                // Generate a PIN
                string pin = pinGenerator.GeneratePin();

                //generate time for the pin
                pinGenerator.Generate_expiry_Time();

                //web credentials
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "testingproject2001@gmail.com";
                string smtpPassword = "uxws wbem dspt pdjd";

                //email content
                string subject = "Reset your password";
                string body = $"Dear {user_Identity.user_FName},\n\nWe've received a request from you to reset the password for your account. To ensure the security of your account, we require you to verify your email address. Please use this verification code provided below to complete the verification process and gain access to your account.\n\nVerification Code: {pin}\n\nThank you for helping us maintain the security of your account.\n\nBest regards,\nECMS";

                //send PIN to email
                try
                {
                    emailSender emailSender = new emailSender(smtpServer, smtpPort, smtpUsername, smtpPassword);

                    emailSender.SendEmail("testingproject2001@gmail.com", user_Identity.user_Email, subject, body);
                    DateTime currentTime_ = DateTime.Now.AddMinutes(1);
                    Verification.timeNow = currentTime_;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Response.Write("Pls wait for a minute before u can resend code again");
            }
        }

        protected void VOTPBtn_Click(object sender, EventArgs e)
        {
            // Concatenate the values of the four textboxes into a single string
            String concatenatedString = TextBox1.Text + TextBox2.Text + TextBox3.Text + TextBox4.Text;

            bool isPincorrect = pin_Valid_check.IsPinValid(concatenatedString);

            if (isPincorrect == true) { Response.Redirect("ResetPassword.aspx"); }
            else { Response.Write("Invalid"); }
        }
    }
}