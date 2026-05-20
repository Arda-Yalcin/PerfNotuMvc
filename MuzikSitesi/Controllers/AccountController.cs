using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuzikSitesi.Models;
using MuzikSitesi.Models.ViewModels;

namespace MuzikSitesi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Form bilgileriyle yeni uygulama kullanicisi hazirlanir.
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Telefon = model.Telefon,
                Adres = model.Adres,
            };

            var sonuc = await _userManager.CreateAsync(user, model.Password);
            if (sonuc.Succeeded)
            {
                // Yeni uye varsayilan Member rolune eklenip oturum acilir.
                await _userManager.AddClaimAsync(user, new Claim("TamAd", user.Ad + " " + user.Soyad));
                await _userManager.AddToRoleAsync(user, "Member");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Cd");
            }

            foreach (var hata in sonuc.Errors)
            {
                ModelState.AddModelError("", hata.Description);
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Identity e-posta ve sifreyi kontrol eder.
            var sonuc = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (sonuc.Succeeded)
            {
                return RedirectToAction("Index", "Cd");
            }

            if (sonuc.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabiniz kilitlendi.");
            }
            else
            {
                ModelState.AddModelError("", "Kullanici adi veya sifreniz hatali.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Aktif oturum kapatilir.
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Cd");
        }
    }
}
