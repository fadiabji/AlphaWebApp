using System;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TimerTriggerWeeklyService.Services;

namespace TimerTriggerWeeklyService
{
    public class NewsLetterFunction
    {
        private readonly ILogger _logger;
        private readonly IWeeklyService _weeklyService;
        private readonly IConfiguration _configuration;
        public NewsLetterFunction(ILoggerFactory loggerFactory, IWeeklyService weeklyService, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<NewsLetterFunction>();
            _weeklyService = weeklyService;
            _configuration = configuration;

        }

        [Function("Function1")]
        //public void Run([TimerTrigger("0 0 0 * * 6", RunOnStartup = true)] MyInfo myTimer)
        public void Run([TimerTrigger("0 0 0 * * 6")] MyInfo myTimer)

        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            var result = _weeklyService.GetArticlesToWeeklyNewsLetter();

            QueueClient queueClient = new QueueClient(_configuration["AzureWebJobsStorage"], _configuration["AzureQueueName"], new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            foreach (var item in result)
            {
                _logger.LogInformation("Sending NewsLetter to " + item.Email );
                queueClient.SendMessage(JsonConvert.SerializeObject(item));
                //queueClient.SendMessage(JsonConvert.SerializeObject(item,
                //        new JsonSerializerSettings()
                //        {
                //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //        }));
            }
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
