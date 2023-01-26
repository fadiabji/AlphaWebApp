using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTriggerWeeklyService.Model
{
    public class WeeklyEmail
    {
        public List<Article> Articles { get; set; }
        public string  Email { get; set; }
       
    }
}
