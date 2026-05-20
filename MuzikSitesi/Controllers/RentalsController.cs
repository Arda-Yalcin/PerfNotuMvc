using System;
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
            var query = _context.CdKiralamalari
                .Include(r => r.AppUser)
                .Include(r => r.Cd)
                .ThenInclude(c => c.Grup)
                .AsQueryable();

            if (User?.IsInRole("Admin") != true)
            {
                query = query.Where(r => r.AppUserId == userId);
            }

            var rentals = query
                .OrderByDescending(r => r.RentDate)
                .ToList();

            return View(rentals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Return(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rental = _context.CdKiralamalari
                .Include(r => r.Cd)
                .FirstOrDefault(r => r.Id == id && r.AppUserId == userId);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.IsApproved && !rental.IsReturned && !rental.ReturnRequested)
            {
                rental.ReturnRequested = true;
                rental.ReturnRequestDate = DateTime.UtcNow;
                _context.SaveChanges();
                TempData["Success"] = "İade talebiniz yönetici onayına gönderildi.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult ApproveRental(int id)
        {
            var rental = _context.CdKiralamalari
                .Include(r => r.Cd)
                .FirstOrDefault(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            if (!rental.IsApproved)
            {
                if (rental.Cd == null || rental.Cd.Stock < rental.Quantity)
                {
                    TempData["Error"] = "Bu kiralama için yeterli CD stoğu yok.";
                    return RedirectToAction("Index");
                }

                rental.Cd.Stock -= rental.Quantity;
                rental.IsApproved = true;
                rental.ApprovalDate = DateTime.UtcNow;
                _context.SaveChanges();
                TempData["Success"] = "Kiralama onaylandı.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult ApproveReturn(int id)
        {
            return CompleteReturn(id, "İade onaylandı ve stok güncellendi.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DirectReturn(int id)
        {
            return CompleteReturn(id, "CD direkt iade alındı ve stok güncellendi.");
        }

        private IActionResult CompleteReturn(int id, string successMessage)
        {
            var rental = _context.CdKiralamalari
                .Include(r => r.Cd)
                .FirstOrDefault(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.IsApproved && !rental.IsReturned)
            {
                if (rental.Cd != null)
                {
                    rental.Cd.Stock += rental.Quantity;
                }

                rental.IsReturned = true;
                rental.ReturnDate = DateTime.UtcNow;
                rental.ReturnRequested = true;
                rental.ReturnRequestDate ??= DateTime.UtcNow;
                _context.SaveChanges();
                TempData["Success"] = successMessage;
            }

            return RedirectToAction("Index");
        }
    }
}
