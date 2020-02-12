using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportUp.Data;
using SportUp.Data.Models;
using SportUp.Models;
using SportUp.Models.ViewModels;

namespace SportUp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<SportUpUser> _userManager;
        private readonly SignInManager<SportUpUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger, 
            ApplicationDbContext context, 
            UserManager<SportUpUser> userManager,
            SignInManager<SportUpUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var allSports = _context.Sports.ToList();
            var viewModel = new IndexViewModel()
            {
                AvailableSports = allSports,
            };

            if (_signInManager.IsSignedIn(User))
            {
                var enrolledSports = _context.Users
                    .Include(s => s.UserSports)
                    .SingleOrDefault(s => s.Id == _userManager.GetUserId(User))
                    .UserSports
                    .Select(s => s.Sport)
                    .ToList();

                viewModel.SportsUserIsEnrolledIn = enrolledSports;
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddSport(IndexViewModel viewModel)
        {
            var selectedSports = _context.Sports.Where(s => viewModel.SportsToEnrollUserIn.Any(us => us == s.Id)).ToList();
            var userIdentity = _context.Users.Include(s => s.UserSports).SingleOrDefault(s => s.Id == _userManager.GetUserId(this.User));
            foreach (var item in selectedSports)
            {
                if (userIdentity.UserSports.Any(s => s.SportId == item.Id))
                    continue;
                userIdentity.UserSports.Add(new UserSport()
                {
                    SportId = item.Id,
                    Sport = item,
                    SportUpUserId = userIdentity.Id,
                    SportUpUser = userIdentity
                });
            }
            _context.Users.Update(userIdentity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
