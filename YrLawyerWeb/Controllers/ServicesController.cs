using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resend;
using YrLawyerWeb.Models;
using YrLawyerWeb.Services;

namespace YrLawyerWeb.Controllers
{
    public class ServicesController(YrLawyerContext yrLawyerContext, IEmailService emailService) : Controller
    {
        private readonly YrLawyerContext _context = yrLawyerContext;
        private readonly IEmailService _emailService = emailService;

        public IActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        public IActionResult Order(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();

            ViewBag.Services = _context.Services.ToList();
            ViewBag.SelectedServiceId = id;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Order(int serviceId, string lastName, string firstName, string middleName, string email, string phone, DateTime visitDate)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Email == email);
            if (client == null)
            {
                client = new Client
                {
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    Email = email,
                    Phone = phone
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var cs = new ClientService
            {
                ClientId = client.Id,
                ServiceId = serviceId,
                DateRequested = visitDate
            };

            _context.ClientServices.Add(cs);
            await _context.SaveChangesAsync();

            // Простое HTML-сообщение
            var emailBody = $@"
                <html>
                    <body>
                        <h2>Подтверждение записи на услугу</h2>
                        <p>Здравствуйте, {client.FirstName} {client.LastName}!</p>
                        <p>Вы успешно записались на услугу: <strong>{_context.Services.FirstOrDefault(s => s.Id == serviceId)?.Title}</strong>.</p>
                        <p>Дата вашего посещения: <strong>{visitDate.ToShortDateString()}</strong></p>
                        <p>Благодарим вас за доверие! В случае необходимости вы можете оставить отзыв о нашей услуге, перейдя по следующей ссылке:</p>
                        <p><a href='{Url.Action("AllFeedbacks", "Services", null, Request.Scheme)}'>Оставить отзыв</a></p>
                        <br />
                        <p>С уважением, команда YrLawyer</p>
                    </body>
                </html>";

            // Отправка email
            await _emailService.Send(client.Email, "Подтверждение записи на услугу", emailBody);

            return RedirectToAction("Index");
        }

        public IActionResult AllFeedbacks()
        {
            var feedbacks = _context.Feedbacks
                .Include(f => f.Client)
                .Include(f => f.Service)
                .OrderByDescending(f => f.DateCreated)
                .ToList();

            ViewBag.Services = _context.Services.ToList();
            return View(feedbacks);
        }

        [HttpPost]
        public IActionResult AddFeedback(int serviceId, string email, int stars, string message)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Email == email);
            if (client == null) return RedirectToAction("AllFeedbacks");

            var feedback = new Feedback
            {
                ClientId = client.Id,
                ServiceId = serviceId,
                Stars = stars,
                Message = message,
                DateCreated = DateTime.Now
            };

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            return RedirectToAction("AllFeedbacks");
        }

        [HttpGet]
        public IActionResult Feedback(int id)
        {
            var service = _context.Services
                .Include(s => s.Feedbacks)
                .ThenInclude(f => f.Client)
                .FirstOrDefault(s => s.Id == id);

            if (service == null) return NotFound();

            ViewBag.Service = service;
            return View();
        }

        [HttpPost]
        public IActionResult Feedback(int serviceId, string email, int stars, string message)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Email == email);
            if (client == null) return BadRequest("Клиент с такой почтой не найден");

            var feedback = new Feedback
            {
                ClientId = client.Id,
                ServiceId = serviceId,
                Stars = stars,
                Message = message,
                DateCreated = DateTime.Now
            };

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            return RedirectToAction("Feedback", new { id = serviceId });
        }
    }
}

