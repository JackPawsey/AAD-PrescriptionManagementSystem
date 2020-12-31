using System;

namespace AADWebApp.Services
{
    public interface ISendEmailService
    {
        public void SendEmail(String BodyContent, String Subject, String RecipientAddress);
    }
}
