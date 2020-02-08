using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Data.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxPlayers { get; set; }

        public ICollection<UserSport> UserSports {get;set;}
    }
}
