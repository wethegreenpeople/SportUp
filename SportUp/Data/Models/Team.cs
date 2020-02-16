using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Data.Models
{
    public enum TeamPlayStyle
    {
        Casual,
        Hardcore,
        League,
    }

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

        [Required]
        public TeamPlayStyle TeamPlayStyle { get; set; }
    }
}
