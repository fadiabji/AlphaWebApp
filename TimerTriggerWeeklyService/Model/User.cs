using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTriggerWeeklyService.Model
{
    public class User: IdentityUser
    {

        //public string Id { get; set; }
        public string FirstName { get; set; }

       
        public string LastName { get; set; }

        //public string Email { get; set; }
        public virtual ICollection<Subscription> SubscriptionList { get; set; }

    }
}
