using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Data.Models
{
    public class SportUpUser : IdentityUser
    {
        [PersonalData]
        public string Location { get; set; }
        public ICollection<UserSport> UserSports { get; set; }
    }
}
