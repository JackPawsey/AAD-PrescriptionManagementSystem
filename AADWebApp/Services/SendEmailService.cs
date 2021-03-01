using AADWebApp.Interfaces;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AADWebApp.Services
{
    public class SendEmailService : ISendEmailService
    {
        private SmtpClient _client;
        private MailMessage _mailMessage;
        private NetworkCredential _networkCredentials;

        public bool SendEmail(string bodyContent, string subject, string recipientAddress)
        {
            const int smtpPort = 587;
            const string smtpAddress = "smtp.gmail.com";

            const string accountAddress = "cloudcrusaderssystems@gmail.com";
            const string accountPassword = "CloudCrusaders420";

            _networkCredentials = new NetworkCredential(accountAddress, accountPassword);
            _client = new SmtpClient(smtpAddress)
            {
                Port = smtpPort,
                EnableSsl = true,
                Credentials = _networkCredentials
            };

            var from = new MailAddress(accountAddress);
            var to = new MailAddress(recipientAddress);

            _mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = bodyContent,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };

            const string userState = "Sending...";

            try
            {
                _client.SendAsync(_mailMessage, userState);
                Console.WriteLine("Email sent successfully");
                return true;
            }
            catch
            {
                Console.WriteLine("Email send error");
                return false;
            }
        }
    }
}