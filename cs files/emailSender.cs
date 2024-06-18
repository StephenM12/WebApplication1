//for email sending
using System.Net;
using System.Net.Mail;

namespace WebApplication1.cs_files
{
    public class emailSender
    {
        private string smtpServer;
        private int smtpPort;
        private string smtpUsername;
        private string smtpPassword;

        public emailSender(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient(smtpServer);

            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            smtp.Port = smtpPort;
            smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}