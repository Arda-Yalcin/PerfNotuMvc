using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuzikSitesi.Data;

namespace MuzikSitesi.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rentals = _context.CdKiralamalari
                .Include(r => r.Cd)
                .ThenInclude(c => c.Grup)
                .Where(r => r.AppUserId == userId)
                .OrderByDescending(r => r.RentDate)
                .ToList();

            return View(rentals);
        }
    }
}
