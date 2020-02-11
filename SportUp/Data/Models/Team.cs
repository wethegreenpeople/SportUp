using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Data.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
    }
}
