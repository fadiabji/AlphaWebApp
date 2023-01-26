using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TimerNewsApp.Data;
using TimerNewsApp.Model;
using TimerNewsApp.Model.Entities;
using TimerNewsApp.Model.FuncModels;
using TimerNewsApp.Models.SpotModels;

namespace TimerNewsApp.Services
{
    internal class DailyService : IDailyService
    {
        private readonly FuncDbContext _db;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly TableServiceClient _tableServerClient;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        public DailyService(FuncDbContext db,
                            //ILogger logger, 
                            IConfiguration configuration,
                            ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<DailyService>();
            _configuration = configuration;
            _tableServerClient = new TableServiceClient(_configuration["AzureWebJobsStorage"]);
        }

        public List<SubscriptonsDataFM> GetSubscriptonsToExpire()
        {
            var dateInFiveDays = DateTime.Now.AddDays(5).Date;
            //var dateInFiveDays = DateTime.Now.Date;
            var expiringSubscriptionsData = _db.Users.Include(u => u.Subscriptions)
                                                        .Where(u => u.Subscriptions
                                                        //.Any(s => s.Active && s.ExpireAt.Date == dateInFiveDays))
                                                        .Any(s => s.ExpireAt.Date == dateInFiveDays))
                .Select(e => new SubscriptonsDataFM()
                {
                    SubscriberName = e.FirstName +" " + e.LastName,
                    SubscriberEmail = e.Email,
                    SubscriptionTypeName = e.Subscriptions.FirstOrDefault().SubscriptionType.TypeName
                }) ;
            foreach(var sub in expiringSubscriptionsData) 
            {
                _logger.LogInformation("Subscription " + sub.SubscriberName + " will Expired in 5 days");
            }
            return expiringSubscriptionsData.ToList();
        }


        public void SetSubscriptionExpired()
        {
            var result = _db.Subscriptions.Where(s => s.ExpireAt.Date < DateTime.Now.Date);
            foreach (var item in result)
            {
                item.Active = false;
                _db.Update(item);
                _logger.LogInformation("Subscription " + item.Id + " Was Expired");
            }
            _db.SaveChanges();
        }


        public void SetArticleArchive()
        {
            var thrityDaysAgo = DateTime.Now.AddDays(-30);
            //var thrityDaysAgo = DateTime.Now;

            var result = _db.Articles.Where(a => a.DateStamp < thrityDaysAgo && !a.Archive);
            foreach (var item in result)
            {
                item.Archive = true;
                _db.Update(item);
                _logger.LogInformation("Article " + item.Id + " Was Archived");
            }
            _db.SaveChanges();
        }


        public async Task AddSpotPricesDailyToTable()
        {           
                // add request for data 
                HttpClient _spotHttpClient = new HttpClient();
                var request = await _spotHttpClient.GetStringAsync("https://spotfunc.azurewebsites.net/api/SpotPriceRequest?code=vgUdbbCJSApniy7OgY2tfEJuTomMaNzZ-QWTNcMYS8h-AzFuS91H_w==");
                TodaysSpotData todaysData = JsonConvert.DeserializeObject<TodaysSpotData>(request);
                var allData = todaysData.TodaysSpotPrices.SelectMany(a => a.SpotData).ToList();

                List<AreaData> areaData = new List<AreaData>();

                var se1Data = allData.Where(d => d.AreaName == "SE1").ToList();
                var se1High = se1Data.Max(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                var se1Low = se1Data.Min(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                areaData.Add(new AreaData() { Area = "SE1", PriceHigh = se1High, PriceLow = se1Low });

                var se2Data = allData.Where(d => d.AreaName == "SE2").ToList();
                var se2High = se2Data.Max(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                var se2Low = se2Data.Min(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                areaData.Add(new AreaData() { Area = "SE2", PriceHigh = se2High, PriceLow = se2Low });


                var se3Data = allData.Where(d => d.AreaName == "SE3").ToList();
                var se3High = se3Data.Max(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                var se3Low = se3Data.Min(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                areaData.Add(new AreaData() { Area = "SE3", PriceHigh = se3High, PriceLow = se3Low });


                var se4Data = allData.Where(d => d.AreaName == "SE4").ToList();
                var se4High = se4Data.Max(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                var se4Low = se4Data.Min(p => (Convert.ToDouble(p.Price.Replace(" ", "")) / 1000));
                areaData.Add(new AreaData() { Area = "SE4", PriceHigh = se4High, PriceLow = se4Low });

                TableClient tableClient = _tableServerClient.GetTableClient(tableName: "spotprice");
                tableClient.CreateIfNotExists();

                foreach (var item in areaData)
                {
                    SpotPriceEntity newEntity = new();
                    newEntity.AreaName = item.Area;
                    newEntity.SpotPriceHigh = item.PriceHigh;
                    newEntity.SpotPriceLow = item.PriceLow;
                    newEntity.PartitionKey = item.Area;
                    newEntity.DateAndTime = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
                    //newEntity.RowKey = item.Area + newEntity.DateAndTime;
                    //newEntity.RowKey.Replace(" ", "");
                    newEntity.RowKey = new string(Enumerable.Repeat(chars, 20)
                            .Select(s => s[random.Next(s.Length)]).ToArray());
                    tableClient.AddEntity(newEntity);
                }
        }


        //public async Task AddWeatherForeCaseToTable()
        //{
        //    HttpClient httpClient = new HttpClient();
        //    var res = httpClient.GetAsync($"https://weatherapi.dreammaker-it.se/forecast?city=Linkoping&lang=en").Result;
        //    var forecast = res.Content.ReadFromJsonAsync<WeatherForecast>().Result;

        //    TableClient tableClient = _tableServerClient.GetTableClient(tableName: "Weather");
        //    tableClient.CreateIfNotExists();
        //    foreach (var item in areaData)
        //    {
        //        WeatherForecast weatherEntity = new();
        //        weatherEntity.City = forecast.City;
        //        weatherEntity.Summary = forecast.Summary;
        //        weatherEntity.TemperatureF = forecast.TemperatureF;
        //        weatherEntity.TemperatureC = forecast.TemperatureC;
        //        weatherEntity.Lang = forecast.Lang;
        //        weatherEntity.Humidity = forecast.Humidity;

        //        weatherEntity.Date = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc);
        //        //newEntity.RowKey = item.Area + newEntity.DateAndTime;
        //        //newEntity.RowKey.Replace(" ", "");
        //        weatherEntity.RowKey = new string(Enumerable.Repeat(chars, 20)
        //                .Select(s => s[random.Next(s.Length)]).ToArray());
        //        tableClient.AddEntity(weatherEntity);
        //    }
        //}
    }
}
