using System;
using AADWebApp.Interfaces;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace AADWebApp.Services
{
    public class SendSmsService : ISendSmsService
    {
        public void SendSms(string phoneNumber, string message)
        {
            const string awsKey = "AKIA3NTV7LS6XPMG4NOK";
            const string awsSecretKey = "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";

            var client = new AmazonSimpleNotificationServiceClient(awsKey, awsSecretKey, RegionEndpoint.EUWest1);

            var request = new PublishRequest
            {
                Message = message,
                PhoneNumber = phoneNumber,
                MessageAttributes =
                {
                    ["AWS.SNS.SMS.SenderID"] = new MessageAttributeValue
                    {
                        StringValue = "CCrusaders",
                        DataType = "String"
                    }
                }
            };

            try
            {
                var response = client.PublishAsync(request);

                Console.WriteLine("Message sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception publishing request:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}