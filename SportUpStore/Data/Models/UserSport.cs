using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUpManagers.Data.Models
{
    public class UserSport
    {
        public string SportUpUserId { get; set; }
        public SportUpUser SportUpUser { get; set; }

        public int SportId { get; set; }
        public Sport Sport { get; set; }

    }
}
