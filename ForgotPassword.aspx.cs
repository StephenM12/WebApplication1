using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace WebApplication1
{
    public partial class ForgotPassword1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void verify_Email_BTN(object sender, EventArgs e)
        {
            string verify_Email = EmailTB.Text;
            //Response.Redirect("Verification.aspx");
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("testingproject2001@gmail.com");
                mail.To.Add(verify_Email);
                mail.Subject = "testing 1.0";
                mail.Body = "this is for testing ";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("testingproject2001@gmail.com", "uxws wbem dspt pdjd");
                smtp.EnableSsl = true;
                smtp.Send(mail);


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            


        }
    }
}