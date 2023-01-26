using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTriggerWeeklyService.Model.FuncModels
{
    public class CategoriesUserFunM
    {
        public List<Category> SelectedCategories { get; set; }
        public User User { get; set; }


        public CategoriesUserFunM()
        {
            SelectedCategories= new List<Category>();
        }
    }
}
