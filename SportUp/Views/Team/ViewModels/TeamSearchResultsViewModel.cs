using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel = SportUp.Data.Models;

namespace SportUp.Views.Team.ViewModels
{
    public class TeamSearchResultsViewModel
    {
        public List<DataModel.Team> Teams { get; set; }
    }
}
