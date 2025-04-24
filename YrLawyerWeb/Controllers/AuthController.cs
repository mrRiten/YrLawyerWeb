using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YrLawyerWeb.Models;
using YrLawyerWeb.Services;

namespace YrLawyerWeb.Controllers
{
    public class AuthController(YrLawyerContext context, JwtService jwtService) : Controller
    {
        private readonly YrLawyerContext _context = context;
        private readonly JwtService _jwtService = jwtService;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }

            var token = _jwtService.GenerateToken(user);

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
    }
}
