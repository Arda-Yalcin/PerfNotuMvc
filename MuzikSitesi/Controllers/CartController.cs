using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuzikSitesi.Data;
using MuzikSitesi.Models;

namespace MuzikSitesi.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cartItems = _context.SepetKalemleri
                .Include(c => c.Cd)
                .Where(c => c.AppUserId == userId)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int? cdId)
        {
            var userId = GetCurrentUserId();
            if (userId == null || cdId == null)
            {
                return BadRequest();
            }

            var cd = _context.Cdler.Find(cdId.Value);
            if (cd == null)
            {
                return NotFound();
            }

            var existingItem = _context.SepetKalemleri
                .FirstOrDefault(c => c.AppUserId == userId && c.CdId == cdId);
            var currentQuantity = existingItem?.Quantity ?? 0;

            if (cd.Stock <= currentQuantity)
            {
                TempData["Error"] = "Bu CD için yeterli stok yok.";
                return RedirectToAction("Index", "Cd");
            }

            if (existingItem == null)
            {
                _context.SepetKalemleri.Add(new CartItem
                {
                    AppUserId = userId,
                    CdId = cdId,
                    Quantity = 1
                });
            }
            else
            {
                existingItem.Quantity++;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var userId = GetCurrentUserId();
            var cartItem = _context.SepetKalemleri
                .Include(c => c.Cd)
                .FirstOrDefault(c => c.Id == id && c.AppUserId == userId);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity < 1)
            {
                _context.SepetKalemleri.Remove(cartItem);
            }
            else
            {
                var stock = cartItem.Cd?.Stock ?? 0;
                if (quantity > stock)
                {
                    TempData["Error"] = "Seçilen miktar stoktan fazla olamaz.";
                    return RedirectToAction("Index");
                }

                cartItem.Quantity = quantity;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RentCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return BadRequest();
            }

            var cartItems = _context.SepetKalemleri
                .Include(c => c.Cd)
                .Where(c => c.AppUserId == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Sepetinizde kiralanacak CD yok.";
                return RedirectToAction("Index");
            }

            foreach (var item in cartItems)
            {
                var stock = item.Cd?.Stock ?? 0;
                if (item.Quantity > stock)
                {
                    var productName = item.Cd?.Ad ?? "CD";
                    TempData["Error"] = $"{productName} için yeterli stok yok.";
                    return RedirectToAction("Index");
                }
            }

            foreach (var item in cartItems)
            {
                _context.CdKiralamalari.Add(new CdRental
                {
                    AppUserId = userId,
                    CdId = item.CdId!.Value,
                    Quantity = item.Quantity,
                    RentDate = DateTime.UtcNow,
                    IsApproved = false,
                    ReturnRequested = false,
                    IsReturned = false
                });
            }

            _context.SepetKalemleri.RemoveRange(cartItems);
            _context.SaveChanges();

            TempData["Success"] = "Kiralama talebiniz yönetici onayına gönderildi.";
            return RedirectToAction("Index", "Rentals");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var userId = GetCurrentUserId();
            var cartItem = _context.SepetKalemleri.FirstOrDefault(c => c.Id == id && c.AppUserId == userId);
            if (cartItem != null)
            {
                _context.SepetKalemleri.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
