using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MuzikSitesi.Data;
using MuzikSitesi.Models;

namespace MuzikSitesi.Controllers
{
    public class CdController : Controller
    {
        private readonly AppDbContext _context;

        public CdController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cdler = _context.Cdler.Include(c => c.Grup).ToList();
            return View(cdler);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rent(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cd = _context.Cdler.Find(id);
            if (cd == null)
            {
                return NotFound();
            }

            if (cd.Stock <= 0)
            {
                TempData["Error"] = "Bu CD stokta yok, kiralanamaz.";
                return RedirectToAction("Index");
            }

            var existingRental = _context.CdKiralamalari.FirstOrDefault(r => r.AppUserId == userId && r.CdId == id && !r.IsReturned);
            if (existingRental == null)
            {
                _context.CdKiralamalari.Add(new CdRental
                {
                    AppUserId = userId,
                    CdId = id,
                    RentDate = DateTime.UtcNow,
                    IsReturned = false
                });
                cd.Stock--; // Stock azalt
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Ekle()
        {
            ViewBag.GrupListesi = new SelectList(_context.Gruplar, "Id", "Ad");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Ekle(Cd cd, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    var uzanti = Path.GetExtension(Foto.FileName);
                    var yendiAd = Guid.NewGuid() + "." + uzanti;
                    var yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", yendiAd);

                    if (Foto.ContentType == "image/png" || Foto.ContentType == "image/jpeg" || Foto.ContentType == "image/jpg")
                    {
                        using (var stream = new FileStream(yol, FileMode.Create))
                        {
                            try
                            {
                                Foto.CopyTo(stream);
                                cd.Foto = yendiAd;
                                _context.Cdler.Add(cd);
                                _context.SaveChanges();
                                return RedirectToAction("Index", "Cd");
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Hata = "Dosya Yükleme Hatası :" + ex.Message;
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Hata = "jpg ya da png Yükle";
                    }
                }
            }
            else
            {
                ViewBag.Hata = "Dosya Yükle";
            }

            ViewBag.GrupListesi = new SelectList(_context.Gruplar, "Id", "Ad");
            return View(cd);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Sil(int? id)
        {
            if (id == null) return NotFound();

            var cd = _context.Cdler.Include(c => c.Grup).FirstOrDefault(m => m.Id == id);
            if (cd == null) return NotFound();
            return View(cd);
        }

        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SilOnay(int id)
        {
            var cd = _context.Cdler.Find(id);
            if (cd != null)
            {
                _context.Cdler.Remove(cd);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var cd = _context.Cdler.FirstOrDefault(x => x.Id == id);
            if (cd != null)
            {
                ViewBag.GrupListesi = new SelectList(_context.Gruplar, "Id", "Ad");
                return View(cd);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Cd cd, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                var existingCd = _context.Cdler.FirstOrDefault(x => x.Id == cd.Id);
                if (existingCd != null)
                {
                    existingCd.Ad = cd.Ad;
                    existingCd.GrupId = cd.GrupId;
                    existingCd.Stock = cd.Stock;

                    if (Foto != null)
                    {
                        var uzanti = Path.GetExtension(Foto.FileName);
                        var yendiAd = Guid.NewGuid() + "." + uzanti;
                        var yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", yendiAd);

                        if (Foto.ContentType == "image/png" || Foto.ContentType == "image/jpeg" || Foto.ContentType == "image/jpg")
                        {
                            using (var stream = new FileStream(yol, FileMode.Create))
                            {
                                try
                                {
                                    Foto.CopyTo(stream);
                                    existingCd.Foto = yendiAd;
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.Hata = "Dosya Yükleme Hatası :" + ex.Message;
                                    ViewBag.GrupListesi = new SelectList(_context.Gruplar, "Id", "Ad");
                                    return View(cd);
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Hata = "jpg ya da png Yükle";
                            ViewBag.GrupListesi = new SelectList(_context.Gruplar, "Id", "Ad");
                            return View(cd);
                        }
                    }

                    _context.SaveChanges();
                    return RedirectToAction("Index", "Cd");
                }
            }

            ViewBag.GrupListesi = new SelectList(_context.Gruplar, "Id", "Ad");
            return View(cd);
        }
    }
}
