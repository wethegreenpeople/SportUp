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
using SportUp.Managers;
using SportUp.Models.ViewModels;

namespace SportUp.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SportUpUserManager _userManager;
        private readonly SportManager _sportManager;
        private readonly TeamManager _teamManager;

        public TeamController(
            ApplicationDbContext context, 
            SportUpUserManager userManager,
            SportManager sportManager,
            TeamManager teamManager)
        {
            _context = context;
            _userManager = userManager;
            _sportManager = sportManager;
            _teamManager = teamManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allSports = await _sportManager.GetSportsAsync();

            var viewModel = new TeamIndexViewModel()
            {
                AvailableSports = allSports,
                CurrentlyEnrolledTeams = _userManager.GetEnrolledTeams(currentUser),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamIndexViewModel viewModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var teamSport = await _sportManager.GetSportAsync(viewModel.TeamSportId);
            await _teamManager.CreateTeamAsync(viewModel.TeamName, currentUser, teamSport);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> JoinTeam(int TeamId)
        {
            var teamToJoin = await _teamManager.GetTeamAsync(TeamId);
            if (teamToJoin == null)
            {
                ModelState.AddModelError("Team", "Invalid team");
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(User);

            await _userManager.AddTeamToUser(currentUser, teamToJoin);

            return RedirectToAction("Index");
        }
    }
}