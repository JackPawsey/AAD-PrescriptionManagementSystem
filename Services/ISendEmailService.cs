namespace AADWebApp.Services
{
    public interface ISendEmailService
    {
        public void SendEmail(string bodyContent, string subject, string recipientAddress);
    }
}