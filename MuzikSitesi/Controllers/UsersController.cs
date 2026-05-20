using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MuzikSitesi.Models;
using MuzikSitesi.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MuzikSitesi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // Admin tum kullanicilari listeler.
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            // Duzenleme formu secilen kullanicinin mevcut bilgileriyle acilir.
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new UserEditViewModel
            {
                Id = user.Id,
                Ad = user.Ad,
                Soyad = user.Soyad,
                Email = user.Email,
                Telefon = user.Telefon,
                Adres = user.Adres
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // E-posta admin tarafindan degistirilemez; form sadece mevcut degeri gosterir.
            model.Email = user.Email;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            user.Ad = model.Ad;
            user.Soyad = model.Soyad;
            user.Telefon = model.Telefon;
            user.PhoneNumber = model.Telefon;
            user.Adres = model.Adres;

            // Identity kullanici kaydi guncellenir.
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Navbar'da gorunen ad soyad claim bilgisini guncel tut.
                var claims = await _userManager.GetClaimsAsync(user);
                var fullNameClaims = claims.Where(c => c.Type == "TamAd").ToList();
                foreach (var claim in fullNameClaims)
                {
                    await _userManager.RemoveClaimAsync(user, claim);
                }
                await _userManager.AddClaimAsync(user, new Claim("TamAd", $"{user.Ad} {user.Soyad}"));

                TempData["Success"] = "Kullanıcı bilgileri güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
