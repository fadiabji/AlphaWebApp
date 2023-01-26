using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTriggerWeeklyService.Model
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; }
      
        public string HeadLine { get; set; }
        
        public string ContentSummary { get; set; }
        
        public string Content { get; set; }
        
        public Uri ImageLink { get; set; }
        
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}