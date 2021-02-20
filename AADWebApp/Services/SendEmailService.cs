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

        public void SendEmail(string bodyContent, string subject, string recipientAddress)
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

            _client.SendCompleted += SendCompletedCallback;
            const string userState = "Sending...";
            _client.SendAsync(_mailMessage, userState);
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled) 
                Console.WriteLine("Send cancelled");
            else if (e.Error != null)
                Console.WriteLine("Error");
            else
                Console.WriteLine("Email sent successfully");
            
            _mailMessage.Dispose();
        }
    }
}