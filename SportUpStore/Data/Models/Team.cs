using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportUpManagers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TeamName { get; set; }

        [Required]
        public ICollection<UserTeam> UserTeams { get; set; }
        
        [Required]
        public Sport TeamSportType { get; set; }
    }
}
