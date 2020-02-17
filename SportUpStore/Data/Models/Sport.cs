using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportUpManagers.Data.Models
{
    [Table("Sports")]
    public class Sport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int? MaxPlayers { get; set; }

        public ICollection<UserSport> UserSports {get;set;}
        public ICollection<Team> Teams { get; set; }
    }
}
