using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EBookkeepingAuth.Services
{
    public class EmailSender : IEmailSender
    {

        // Our private configuration variables
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;
        private string apiKey;
        private string userEmail;

        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSSL, string userName, string password, string apiKey, string userEmail)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.password = password;
            this.apiKey = apiKey;
            this.userEmail = userEmail;
        }

        // Use our configuration to send the email by using SmtpClient
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(apiKey);           
            var msg = new SendGridMessage();
            msg.HtmlContent = htmlMessage;
            msg.AddTo(email);
            msg.Subject = subject;
            msg.From =new EmailAddress(userEmail,userName);
            return client.SendEmailAsync(msg);
        }

      
    }

}
