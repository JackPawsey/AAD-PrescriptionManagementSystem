namespace AADWebApp.Interfaces
{
    public interface ISendSmsService
    {
        public void SendSms(string phoneNumber, string message);
    }
}