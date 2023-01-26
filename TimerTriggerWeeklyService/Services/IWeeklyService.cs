using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimerTriggerWeeklyService.Model;

namespace TimerTriggerWeeklyService.Services
{
    public interface IWeeklyService
    {
        List<WeeklyEmail> GetArticlesToWeeklyNewsLetter();
    }
}
