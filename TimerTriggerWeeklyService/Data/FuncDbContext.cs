using TimerTriggerWeeklyService.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace TimerTriggerWeeklyService.Data
{
    public class FuncDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;

        public FuncDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["DefaultConnection"]);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Category> Categories { get; set; }

    }
}


