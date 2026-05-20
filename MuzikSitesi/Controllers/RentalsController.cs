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

        public IActionResult Index(string searchTerm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Admin tum kiralamalari, uye sadece kendi kiralamalarini gorur.
            var query = _context.CdKiralamalari
                .Include(r => r.AppUser)
                .Include(r => r.Cd)
                .ThenInclude(c => c.Grup)
                .AsQueryable();

            if (User?.IsInRole("Admin") != true)
            {
                query = query.Where(r => r.AppUserId == userId);
            }
            else if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalized = $"%{searchTerm.Trim().ToLower()}%";
                query = query.Where(r =>
                    EF.Functions.Like((r.AppUser.Ad + " " + r.AppUser.Soyad).ToLower(), normalized)
                    || EF.Functions.Like(r.AppUser.Email.ToLower(), normalized)
                    || EF.Functions.Like(r.AppUser.UserName.ToLower(), normalized));
                ViewData["SearchTerm"] = searchTerm.Trim();
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
                // Uye iade talebi acabilir; stok admin onayinda guncellenir.
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
                // Kiralama onaylanirken stok dusurulur.
                if (rental.Cd == null || rental.Cd.Stock < rental.Quantity)
                {
                    TempData["Error"] = "Bu kiralama için yeterli CD stoğu yok.";
                    return RedirectToAction("Index");
                }

                rental.Cd.Stock -= rental.Quantity;
                rental.IsApproved = true;
                rental.ApprovalDate = DateTime.UtcNow;
                rental.DueDate = rental.ApprovalDate.Value.AddDays(15);
                _context.SaveChanges();
                TempData["Success"] = "Kiralama onaylandı.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateDueDate(int id, DateTime dueDate)
        {
            var rental = _context.CdKiralamalari.FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            if (!rental.IsApproved || rental.IsReturned)
            {
                TempData["Error"] = "Sadece aktif kiralamaların son teslim tarihi değiştirilebilir.";
                return RedirectToAction("Index");
            }

            // Tarayicidan gelen tarih yerel saat kabul edilip veritabaninda UTC saklanir.
            rental.DueDate = DateTime.SpecifyKind(dueDate, DateTimeKind.Local).ToUniversalTime();
            _context.SaveChanges();
            TempData["Success"] = "Son teslim tarihi güncellendi.";

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

                // Direkt iade ve onayli iade ayni stok guncelleme akisini kullanir.
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
