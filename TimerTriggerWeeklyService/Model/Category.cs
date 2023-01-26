using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TimerTriggerWeeklyService.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string name { get; set; }

        //public int SubscriptionId { get; set; }
        //public virtual Subscription Subscription{ get; set; }

        [JsonIgnore]
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}