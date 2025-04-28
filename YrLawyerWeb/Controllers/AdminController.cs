using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YrLawyerWeb.Models;

namespace YrLawyerWeb.Controllers
{
    [Authorize]
    public class AdminController(YrLawyerContext context) : Controller
    {
        private readonly YrLawyerContext _context = context;

        public IActionResult Index()
        {
            return View();
        }

        // USERS
        public async Task<IActionResult> Users() =>
            View(await _context.Users.ToListAsync());

        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> EditUser(int id) =>
            View(await _context.Users.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity != null)
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Users));
        }

        // CLIENTS
        public async Task<IActionResult> Clients() =>
            View(await _context.Clients.ToListAsync());

        public IActionResult CreateClient() => View();

        [HttpPost]
        public async Task<IActionResult> CreateClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Clients));
        }

        public async Task<IActionResult> EditClient(int id) =>
            View(await _context.Clients.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> EditClient(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Clients));
        }

        public async Task<IActionResult> DeleteClient(int id)
        {
            var entity = await _context.Clients.FindAsync(id);
            if (entity != null)
            {
                _context.Clients.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Clients));
        }

        // SERVICES
        public async Task<IActionResult> Services() =>
            View(await _context.Services.ToListAsync());

        public IActionResult CreateService() => View();

        [HttpPost]
        public async Task<IActionResult> CreateService(Service service, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                service.Img = "/uploads/" + fileName;
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Services));
        }


        public async Task<IActionResult> EditService(int id) =>
            View(await _context.Services.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> EditService(Service service, IFormFile ImageFile)
        {
            var existing = await _context.Services.FindAsync(service.Id);
            if (existing == null) return NotFound();

            existing.Title = service.Title;
            existing.Description = service.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var path = Path.Combine("wwwroot/uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                existing.Img = "/uploads/" + fileName;
            }

            _context.Services.Update(existing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Services));
        }

        public async Task<IActionResult> DeleteService(int id)
        {
            var entity = await _context.Services.FindAsync(id);
            if (entity != null)
            {
                _context.Services.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Services));
        }

        // FEEDBACKS
        public async Task<IActionResult> Feedbacks() =>
            View(await _context.Feedbacks.Include(f => f.Client).Include(f => f.Service).ToListAsync());

        public IActionResult CreateFeedback() => View();

        [HttpPost]
        public async Task<IActionResult> CreateFeedback(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Feedbacks));
        }

        public async Task<IActionResult> EditFeedback(int id) =>
            View(await _context.Feedbacks.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> EditFeedback(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Feedbacks));
        }

        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var entity = await _context.Feedbacks.FindAsync(id);
            if (entity != null)
            {
                _context.Feedbacks.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Feedbacks));
        }

        // CLIENT SERVICES
        public async Task<IActionResult> ClientServices() =>
            View(await _context.ClientServices.Include(cs => cs.Client).Include(cs => cs.Service).ToListAsync());

        public IActionResult CreateClientService() => View();

        [HttpPost]
        public async Task<IActionResult> CreateClientService(ClientService cs)
        {
            _context.ClientServices.Add(cs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ClientServices));
        }

        public async Task<IActionResult> EditClientService(int id) =>
            View(await _context.ClientServices.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> EditClientService(ClientService cs)
        {
            _context.ClientServices.Update(cs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ClientServices));
        }

        public async Task<IActionResult> DeleteClientService(int id)
        {
            var entity = await _context.ClientServices.FindAsync(id);
            if (entity != null)
            {
                _context.ClientServices.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ClientServices));
        }
    }
}