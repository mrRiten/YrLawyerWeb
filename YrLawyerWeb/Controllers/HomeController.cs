using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YrLawyerWeb.Models;

namespace YrLawyerWeb.Controllers
{
    public class HomeController(ILogger<HomeController> logger, YrLawyerContext yrLawyerContext) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly YrLawyerContext _context = yrLawyerContext;

        public IActionResult Index()
        {
            var services = _context.Services.Take(4).ToList();
            return View(services);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contacts()
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
