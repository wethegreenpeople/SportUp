using SportUp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Models.ViewModels
{
    public class TeamIndexViewModel
    {
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Display(Name = "Team Sport")]
        public int TeamSportId { get; set; }

        public List<Sport> AvailableSports { get; set; }

        public List<Team> CurrentlyEnrolledTeams { get; set; }
    }
}
