namespace AADWebApp.Services
{
    public interface ISendSmsService
    {
        public void SendSms(string phoneNumber, string message);
    }
}