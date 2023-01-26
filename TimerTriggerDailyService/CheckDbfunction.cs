using System;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TimerNewsApp.Services;

namespace TimerNewsApp
{
    public class CheckDbfunction
    {
        private readonly ILogger _logger;
        private readonly IDailyService _dailyServiece;
        private readonly IConfiguration _configuration;

        public CheckDbfunction(ILoggerFactory loggerFactory, IDailyService dailyService, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<CheckDbfunction>();
            _dailyServiece= dailyService;
            _configuration = configuration;
        }

        [Function("Function1")]
        public void Run([TimerTrigger("0 59 23 * * *", RunOnStartup = true)] MyInfo myTimer)
        //public void Run([TimerTrigger("0 59 23 * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            var result = _dailyServiece.GetSubscriptonsToExpire();

            QueueClient queueClient = new QueueClient(_configuration["AzureWebJobsStorage"], _configuration["AzureQueueName"], new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            foreach (var item in result)
            {
                queueClient.SendMessage(JsonConvert.SerializeObject(item));
            }
            _dailyServiece.SetSubscriptionExpired();
            _dailyServiece.SetArticleArchive();
            _dailyServiece.AddSpotPricesDailyToTable();
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
