using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTriggerWeeklyService.Model
{
    public class Subscription
    {
        public int Id { get; set; }
       
        public bool Active { get; set; } = false;
   
        public string UserId { get; set; }
  
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Category> Categories { get; set; }

    }
}
