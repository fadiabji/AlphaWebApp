using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTriggerWeeklyService.Model
{
    public class SubscriptionType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string Description { get; set; }
        public int Period { get; set; }
        [Required]
        public decimal Price { get; set; }
        public virtual ICollection<Subscription> Supscriptions { get; set; }
    }
}
