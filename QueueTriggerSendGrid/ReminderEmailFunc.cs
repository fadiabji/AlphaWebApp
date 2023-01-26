// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using SendEmail.Models;
using Microsoft.OData;
using System.ComponentModel;
using System.Threading.Tasks;


namespace SendEmail
{
    public class ReminderEmailFunc
    {
        [FunctionName("ReminderEmailFunc")]
        [return: SendGrid(ApiKey = "SendGridKey")]
        public SendGridMessage Run([QueueTrigger("newsqueue", Connection = "AzureWebJobsStorage")] SubscriptonsDataFM newEmail, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed subscription: {newEmail.SubscriberName}");

            SendGridMessage message = new SendGridMessage()
            {
                Subject = $"(#{newEmail.SubscriberName})! Your subscription at AlphaNews will end in five days",
                From = new EmailAddress("alpha.team23@outlook.com")
            };

            message.AddContent("text/html", $"<h2>Hello {newEmail.SubscriberName},</h2>"+
                                            $"<p>We will inform you that your subscription ({newEmail.SubscriptionTypeName}) at AlphaNews will end in five days,</p>" +
                                            $"<p>Thank you for the time you spend subscribing with us!</p>" +
                                            $"<p>If you are happy with subscription we will be happy if you resubscribe again.</p>"+
                                            $"<br><p>Best regards,AlphaNews Team</p>");
            message.AddTo(newEmail.SubscriberEmail);
            //message.AddTo("alpha.team25@outlook.com");

            message.SetFrom("alpha.team23@outlook.com");
            return message;
        }
    }
}
