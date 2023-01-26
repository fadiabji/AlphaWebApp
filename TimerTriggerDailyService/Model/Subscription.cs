using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TimerNewsApp.Model
{
    public class Subscription
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime ExpireAt { get; set; }
        public User User { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

    }
}
