using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportUpManagers.Data.Models;
using SportUpManagers.Data;

namespace SportUpManagers
{
    public class SportUpUserManager : UserManager<SportUpUser>
    {
        private readonly ApplicationDbContext _context;

        public SportUpUserManager(
            IUserStore<SportUpUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<SportUpUser> passwordHasher, 
            IEnumerable<IUserValidator<SportUpUser>> userValidators, 
            IEnumerable<IPasswordValidator<SportUpUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services,
            ILogger<UserManager<SportUpUser>> logger,
            ApplicationDbContext context)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _context = context;
        }

        public async Task<SportUpUser> AddSportToUser(SportUpUser user, Sport sport) 
            => await AddSportToUser(user, new List<Sport>() { sport });

        public async Task<SportUpUser> AddSportToUser(SportUpUser user, List<Sport> sports)
        {
            foreach (var sport in sports)
            {
                if (user.UserSports.Any(s => s.SportId == sport.Id))
                    continue;

                user.UserSports.Add(new UserSport()
                {
                    SportId = sport.Id,
                    Sport = sport,
                    SportUpUserId = user.Id,
                    SportUpUser = user
                });
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
