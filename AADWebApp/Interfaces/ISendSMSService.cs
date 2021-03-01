namespace AADWebApp.Interfaces
{
    public interface ISendSmsService
    {
        public bool SendSms(string phoneNumber, string message);
    }
}