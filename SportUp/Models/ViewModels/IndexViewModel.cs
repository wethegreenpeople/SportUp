using SportUp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<Sport> AvailableSports { get; set; }
        public List<int> UserEnrolledSports { get; set; }
    }
}
