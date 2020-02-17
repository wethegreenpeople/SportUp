using LinqKit;
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
            return await _context.Teams.Include(s => s.UserTeams).Where(s => s.TeamSportType == sport).ToListAsync();
        }

        // TODO: Add owner column to team table
        public async Task<Team> CreateTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<List<Team>> GetTeams(ExpressionStarter<Team> predicate)
        {
            if (predicate == null)
                return await _context.Teams
                    .Include(s => s.TeamSportType)
                    .Include(s => s.UserTeams)
                    .OrderByDescending(s => s.Id)
                    .Take(10)
                    .ToListAsync();

            return await _context.Teams
                    .Include(s => s.TeamSportType)
                    .Include(s => s.UserTeams)
                    .Where(predicate)
                    .OrderByDescending(s => s.Id)
                    .Take(10)
                    .ToListAsync();
        }
    }
}
