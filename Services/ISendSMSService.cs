using System;

namespace AADWebApp.Services
{
    public interface ISendSMSService
    {
        public void SendSMS(String PhoneNumber, String Message);
    }
}