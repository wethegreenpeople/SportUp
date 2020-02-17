using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportUp.Data;
using SportUp.Data.Models;
using SportUp.Managers;
using SportUp.Models.ViewModels;
using SportUp.Views.Team.ViewModels;

//  Testing CD
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

            var team = new Team()
            {
                TeamName = viewModel.TeamName,
                TeamSportType = teamSport,
                TeamPlayStyle = viewModel.PlayStyle,
                UserTeams = new List<UserTeam>
                {
                    new UserTeam()
                    {
                        SportUpUser = currentUser,
                    }
                }
            };

            await _teamManager.CreateTeamAsync(team);

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

        [HttpGet]
        public async Task<IActionResult> ViewTeam(int TeamId)
        {
            var team = await _context.Teams
                .Include(s => s.UserTeams)
                .ThenInclude(s => s.SportUpUser)
                .Include(s => s.TeamSportType)
                .SingleOrDefaultAsync(s => s.Id == TeamId);

            return View("TeamDetails", team);
        }

        [HttpGet]
        public async Task<IActionResult> BySport(int SportId)
        {
            var sport = await _sportManager.GetSportAsync(SportId);
            var teams = await _teamManager.GetTeamsBySportAsync(sport);

            return View("TeamSearchResults", new TeamSearchResultsViewModel() { Teams = teams, });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewAvailableTeams()
        {
            var predicate = PredicateBuilder.New<Team>();
            var teams = await _teamManager.GetTeams(null);

            return View("FindTeam", teams);
        }

        [HttpGet]
        public async Task<IActionResult> ViewEnrolledTeams()
        {
            var user = await _userManager.GetUserAsync(User);
            var teams = _userManager.GetEnrolledTeams(user);
            return View("JoinedTeams", teams);
        }
    }
}