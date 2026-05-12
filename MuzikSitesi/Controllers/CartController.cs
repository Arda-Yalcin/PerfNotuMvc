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

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var cartItems = _context.SepetKalemleri
                .Include(c => c.Album)
                .Include(c => c.Cd)
                .Where(c => c.AppUserId == userId)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int? albumId, int? cdId)
        {
            var userId = GetCurrentUserId();
            if (albumId == null && cdId == null)
            {
                return BadRequest();
            }

            if (albumId != null && cdId != null)
            {
                return BadRequest();
            }

            if (albumId != null)
            {
                var existingItem = _context.SepetKalemleri
                    .FirstOrDefault(c => c.AppUserId == userId && c.AlbumId == albumId);

                if (existingItem == null)
                {
                    _context.SepetKalemleri.Add(new CartItem
                    {
                        AppUserId = userId,
                        AlbumId = albumId,
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

            var existingCdItem = _context.SepetKalemleri
                .FirstOrDefault(c => c.AppUserId == userId && c.CdId == cdId);

            if (existingCdItem == null)
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
                existingCdItem.Quantity++;
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
                .Include(c => c.Album)
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
                cartItem.Quantity = quantity;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
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
