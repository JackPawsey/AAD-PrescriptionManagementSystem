using Microsoft.AspNetCore.Hosting;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AADWebApp.Services
{
    public class SendEmailService: ISendEmailService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        private NetworkCredential NetworkCreds;
        private SmtpClient Client;
        private MailMessage MailMessage;

        public SendEmailService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public void SendEmail(String BodyContent, String Subject, String RecipientAddress)
        {
            int SmtpPort = 587;
            String SmtpAddress = "smtp.gmail.com";

            String AccountAddress = "cloudcrusaderssystems@gmail.com";
            String AccountPassword = "CloudCrusaders420";

            NetworkCreds = new NetworkCredential(AccountAddress, AccountPassword);
            Client = new SmtpClient(SmtpAddress);
            Client.Port = SmtpPort;
            Client.EnableSsl = true;
            Client.Credentials = NetworkCreds;
            
            MailAddress From = new MailAddress(AccountAddress);
            MailAddress To = new MailAddress(RecipientAddress);
            MailMessage = new MailMessage(From, To);

            MailMessage.Subject = Subject;
            MailMessage.Body = BodyContent;
            MailMessage.BodyEncoding = Encoding.UTF8;
            MailMessage.IsBodyHtml = true;
            MailMessage.Priority = MailPriority.Normal;
            MailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            String Userstate = "Sending...";
            Client.SendAsync(MailMessage, Userstate);

            MailMessage.Dispose();
        }

        private void SendCompletedCallback(object Sernder, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Send cancelled");
            }
            if (e.Error != null)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Email sent successfully");
            }
        }
    }
}
