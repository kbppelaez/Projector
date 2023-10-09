using System.Net.Mail;

namespace Projector.Core.Email
{
    public class EmailService : IEmailService
    {
        public EmailService() { }

        public void SendEmail(EmailContentData email)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;

            mail.From = WebsiteMailAddress();

            mail.To.Add(email.Recipient);
            mail.Subject = email.Subject;
            mail.Body = email.Content;

            SmtpClient smtp = CreateSMTPClient();
            smtp.Send(mail);
        }

        private MailAddress WebsiteMailAddress()
        {
            return new MailAddress("admin@projector.com");
        }

        private SmtpClient CreateSMTPClient()
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host = "localhost";
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;

            return smtpClient;
        }
    }
}
