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

namespace SendEmail
{
    public class WeeklyNewsLetter
    {
        [FunctionName("WeeklyNewsLetter")]
        [return: SendGrid(ApiKey = "SendGridKey",  From = "alpha.team23@outlook.com")]
        public SendGridMessage Run([QueueTrigger("weeklynewsletter", Connection = "AzureWebJobsStorage")] WeeklyEmail weeklynewsletter, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed Newsletter to: {weeklynewsletter.Email}");

            SendGridMessage message = new SendGridMessage()
            {
                Subject = $"Weekly News from AlphNews (#{weeklynewsletter.Email})!"

            };

            var stringmessage = "";
            foreach (var article in weeklynewsletter.Articles)
            {
                stringmessage += $"<img src='{article.ImageLink.AbsoluteUri}'/><br/><h3>{article.DateStamp}</h3><br/><h3>{article.HeadLine}.</h3><br/> <pre>{article.Content}.</pre><br/><br/>";
            }

            message.AddContent("text/html", $"<h3>Hello {weeklynewsletter.Email},<h3><br/><p>Dear Customer We'd like to show you notifications for the latest news and updates<p><br/>" + 
                                                                                $"{stringmessage}<br/>"+
                                                                                $"<p>We hope you enjoy news for this week.</p>"+
                                                                                $"<p>Best regards! </p><p>AlphaNews Team</p>" +
                                                                                $"<p>If you wish to unsubscribe to our newsletter, please follow the link below.</p>" +
                                                                                $"<span><a href=https://alphanews.azurewebsites.net/User/unsubscribetonewsletter>https://alphanews.azurewebsites.net/User/unsubscribetonewsletter</a></span>");


            message.AddTo(weeklynewsletter.Email);
            return message;
        }
    }

}
