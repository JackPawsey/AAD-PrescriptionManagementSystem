namespace AADWebApp.Interfaces
{
    public interface ISendEmailService
    {
        public bool SendEmail(string bodyContent, string subject, string recipientAddress);
    }
}