using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendEmail.Models;
using SendGrid.Helpers.Mail;

namespace SendEmail
{
    public class SubScriptionEmail
    {
        [FunctionName("SubScriptionEmail")]
        [return: SendGrid(ApiKey = "SendGridKey")]
        public SendGridMessage Run([QueueTrigger("mailqueue", Connection = "AzureWebJobsStorage")] SubscriptionSummary newSummary, ILogger log)
        {
            if(newSummary.Name != null && newSummary.Email != null)
            {
                log.LogInformation($"C# Queue trigger function have sent Email from: {newSummary.Email}");

                SendGridMessage message = new SendGridMessage()
                {
                    Subject = $"Someone send message to us (#{newSummary.Email})!",
                    From = new EmailAddress(newSummary.Email, "azure function app")
                };

                message.AddContent("text/html", $"<h2>Hello Alpha News,</h2>" +
                                                $"<p> We have get message from contact us dialog in our website,</p>" +
                                                $"<p> Message sender Name : {newSummary.Name}</p>" +
                                                $"<p> Email: {newSummary.Email}" +
                                                $"<p> Message text: <b>{newSummary.Message}</b></p>" +
                                                $"<br><p>End of message</p>");
                message.AddTo("alpha.team25@outlook.com");
                message.SetFrom("alpha.team23@outlook.com");
                return message;
            }
            else
            {
                log.LogInformation($"C# Queue trigger function processed subscription: {newSummary.SubscriberName}");

                SendGridMessage message = new SendGridMessage()
                {
                    Subject = $"Thanks for your subscribing with us (#{newSummary.SubscriberName})!"
                };

                message.AddContent("text/html", $"<h3>Hello {newSummary.SubscriberName}!</h3> <p>Thank you for subscribing with us!" +
                    $"<br/>Your subscription {newSummary.SubscriptionTypeName} is being processed and it will cost {newSummary.SubscriptionPrice}." +
                    $"<br/>Remember your subscription will expire on {newSummary.SubscriptionExpiryDate.Date}." +
                    $"<br/><br/>Best Regards Alpha News Team</p>");
                message.AddTo(newSummary.SubscriberEmail, newSummary.SubscriberName);
                message.SetFrom("alpha.team23@outlook.com");
                return message;
            }
        }
    }

}

