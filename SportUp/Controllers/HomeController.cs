using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<SportUpUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var allSports = _context.Sports.ToList();
            var viewModel = new IndexViewModel()
            {
                AvailableSports = allSports,
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddSport(IndexViewModel viewModel)
        {
            var selectedSports = _context.Sports.Where(s => viewModel.UserEnrolledSports.Any(us => us == s.Id)).ToList();
            var userIdentity = _context.Users.SingleOrDefault(s => s.Id == _userManager.GetUserId(this.User));
            var userSports = new List<UserSport>();
            foreach (var item in selectedSports)
            {
                userSports.Add(new UserSport()
                {
                    SportId = item.Id,
                    Sport = item,
                    SportUpUserId = userIdentity.Id,
                    SportUpUser = userIdentity
                });
            }
            userIdentity.UserSports = userSports;
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
