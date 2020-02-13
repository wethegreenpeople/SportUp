using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Data.Models
{
    public class UserTeam
    {
        public string SportUpUserId { get; set; }
        public SportUpUser SportUpUser { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
