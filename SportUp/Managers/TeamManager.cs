using Microsoft.EntityFrameworkCore;
using SportUp.Data;
using SportUp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Managers
{
    public class TeamManager
    {
        private readonly ApplicationDbContext _context;

        public TeamManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Team> GetTeamAsync(int teamId)
            => await _context.Teams.SingleOrDefaultAsync(s => s.Id == teamId);

        public async Task<List<Team>> GetTeamsBySportAsync(Sport sport)
        {
            return await _context.Teams.Where(s => s.TeamSportType == sport).ToListAsync();
        }

        // TODO: Add owner column to team table
        public async Task<Team> CreateTeamAsync(string teamName, SportUpUser teamOwner, Sport sport)
        {
            var team = new Team()
            {
                TeamName = teamName,
                TeamSportType = sport,
                UserTeams = new List<UserTeam>
                {
                    new UserTeam()
                    {
                        SportUpUser = teamOwner,
                    }
                }
            };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return team;
        }
    }
}
