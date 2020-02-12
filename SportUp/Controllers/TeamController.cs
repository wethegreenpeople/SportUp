using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportUp.Data;
using SportUp.Data.Models;
using SportUp.Models.ViewModels;

namespace SportUp.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<SportUpUser> _userManager;

        public TeamController(ApplicationDbContext context, UserManager<SportUpUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allSports = await _context.Sports.ToListAsync();
            var enrolledTeams = await _context.Teams
                .Include(s => s.UserTeams)
                .Where(s => s.UserTeams.Any(s => s.SportUpUserId == currentUser.Id))
                .ToListAsync();
            var viewModel = new TeamIndexViewModel()
            {
                AvailableSports = allSports,
                CurrentlyEnrolledTeams = enrolledTeams,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamIndexViewModel viewModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var selectedSport = await _context.Sports.SingleOrDefaultAsync(s => s.Id == viewModel.TeamSportId);

            var team = new Team()
            {
                TeamName = viewModel.TeamName,
                TeamSportType = selectedSport,
                UserTeams = new List<UserTeam>
                {
                    new UserTeam()
                    {
                        SportUpUser = currentUser,
                    }
                }
            };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}