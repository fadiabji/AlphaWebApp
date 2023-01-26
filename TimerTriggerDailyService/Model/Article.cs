using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerNewsApp.Model
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; }

        public bool Archive { get; set; }
    }
}
