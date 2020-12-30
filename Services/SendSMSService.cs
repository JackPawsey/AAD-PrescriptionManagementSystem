using System;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Hosting;

namespace AADWebApp.Services
{
    public class SendSMSService
    {
        public IWebHostEnvironment WebHostEnvironment { get; }

        public SendSMSService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public void SendSMS(String PhoneNumber, String Message)
        {
            String AWSKey = "AKIA3NTV7LS6XPMG4NOK";
            String AWSSecretKey = "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";

            AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(AWSKey, AWSSecretKey, region: Amazon.RegionEndpoint.EUWest1);

            PublishRequest request = new PublishRequest
            {
                Message = Message,
                PhoneNumber = PhoneNumber
            };

            request.MessageAttributes["AWS.SNS.SMS.SenderID"] = new MessageAttributeValue { StringValue = "CCrusaders", DataType = "String" };

            try
            {
                var response = client.PublishAsync(request);

                Console.WriteLine("Message sent to " + PhoneNumber + ":");
                Console.WriteLine(Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception publishing request:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}