using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TimerTriggerWeeklyService.Data;
using TimerTriggerWeeklyService.Model;
using TimerTriggerWeeklyService.Model.FuncModels;

namespace TimerTriggerWeeklyService.Services
{
    public class WeeklyService : IWeeklyService
    {

        private readonly FuncDbContext _db;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public WeeklyService(FuncDbContext db,
                            //ILogger logger, 
                            IConfiguration configuration,
                            ILoggerFactory loggerFactory
                            )
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<WeeklyService>();
            _configuration = configuration;
        }

        // this method will change the blobe to the size I want
        public List<Article> GetAllArticlesWithSuitableBlobe()
        {
            var allArticles = _db.Articles.ToList();
            foreach(var article in allArticles)
            {
                string imagelinkString = article.ImageLink.ToString();
                var updatedUri = imagelinkString.Replace("/news-images/", "/news-images-sm/");
                string content = article.Content.Replace("\n", "").Replace("\r", "");
                article.Content = content;
                article.ImageLink = new Uri(updatedUri);
            }
            return allArticles;
        }

      

        public List<WeeklyEmail> GetArticlesToWeeklyNewsLetter()
        {
            // Get all active subscriptions (Emails and Articles belong to this email)
            //var allActiveSubscriptions = _db.Subscriptions.Where(s => s.Active == true).Include(s => s.Categories).ToList();
            var allActiveSubscriptions = _db.Subscriptions.Where(s => s.Active == true).Include(s => s.Categories).ToList();
            var listWeeklEmail = new List<WeeklyEmail>();
            foreach (var subscription in allActiveSubscriptions)
            {
                var eachUserArticles = new List<Article>();
                //  If categories list inside the subscription have some categories 
                if (subscription.Categories.Any())
                {
                    var userId = subscription.UserId;
                    foreach (var category in subscription.Categories)
                    {
                        // Get the latest 5 articles in each category
                        eachUserArticles.AddRange(GetAllArticlesWithSuitableBlobe().Where(a => a.CategoryId == category.Id)
                                                                .OrderByDescending(x => x.DateStamp)
                                                                .Take(5).ToList());
                    }
                    var weeklyEmail = new WeeklyEmail()
                    {
                        Email = _db.Users.FirstOrDefault(u => u.Id == userId).Email,
                        Articles = eachUserArticles
                    };
                    listWeeklEmail.Add(weeklyEmail);
                }
            }
            return listWeeklEmail;
        }



    }
}
