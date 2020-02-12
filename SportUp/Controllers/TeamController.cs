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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(TeamIndexViewModel viewModel)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            viewModel.Team.UserTeams = new List<UserTeam>
            {
                new UserTeam()
                {
                    SportUpUser = currentUser,
                }
            };
            await _context.Teams.AddAsync(viewModel.Team);
            await _context.SaveChangesAsync();

            return View("Index");
        }
    }
}