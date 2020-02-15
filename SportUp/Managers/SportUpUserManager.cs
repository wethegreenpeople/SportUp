using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SportUp.Data;
using SportUp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Managers
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

        public async Task<SportUpUser> AddSportToUserAsync(SportUpUser user, Sport sport)
            => await AddSportToUserAsync(user, new List<Sport>() { sport });

        public async Task<SportUpUser> AddSportToUserAsync(SportUpUser user, List<Sport> sports)
        {
            var userWithSports = await _context.Users
                .Include(s => s.UserSports)
                .SingleOrDefaultAsync(s => s.Id == user.Id);

            foreach (var sport in sports)
            {
                if (userWithSports.UserSports?.Any(s => s.SportId == sport.Id) == true)
                    continue;

                userWithSports.UserSports.Add(new UserSport()
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

        public List<Sport> GetEnrolledSports(SportUpUser user)
        {
            return _context.Users
                    .Include(s => s.UserSports)
                    .SingleOrDefault(s => s.Id == user.Id)
                    .UserSports
                    .Select(s => s.Sport)
                    .ToList();
        }

        public List<Team> GetEnrolledTeams(SportUpUser user)
        {
            return _context.Users
                .Include(s => s.UserTeams)
                .ThenInclude(s => s.Team)
                .SingleOrDefault(s => s.Id == user.Id)
                .UserTeams
                .Select(s => s.Team)
                .ToList();
        }

        public async Task<SportUpUser> AddTeamToUser(SportUpUser user, Team team)
        {
            var userWithTeams = await _context.Users
               .Include(s => s.UserTeams)
               .ThenInclude(s => s.Team)
               .SingleOrDefaultAsync(s => s.Id == user.Id);

            if (userWithTeams.UserTeams?.Any(s => s.TeamId == team.Id) == true)
                return user;

            userWithTeams.UserTeams.Add(new UserTeam()
            {
                TeamId = team.Id,
                Team = team,
                SportUpUserId = user.Id,
                SportUpUser = user
            });

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
