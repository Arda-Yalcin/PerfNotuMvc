using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuzikSitesi.Data;
using MuzikSitesi.Models;

namespace MuzikSitesi.Controllers
{
    public class GrupController : Controller
    {
        private readonly AppDbContext _context;

        public GrupController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var gruplar = _context.Gruplar.ToList();
            return View(gruplar);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Ekle(Grup grup)
        {
            if (ModelState.IsValid)
            {
                _context.Gruplar.Add(grup);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grup);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var grup = _context.Gruplar.FirstOrDefault(g => g.Id == id);
            if (grup == null) return NotFound();
            return View(grup);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Grup grup)
        {
            if (ModelState.IsValid)
            {
                _context.Gruplar.Update(grup);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grup);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Sil(int? id)
        {
            if (id == null) return NotFound();
            var grup = _context.Gruplar.FirstOrDefault(m => m.Id == id);
            if (grup == null) return NotFound();
            return View(grup);
        }

        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SilOnay(int id)
        {
            var grup = _context.Gruplar.Find(id);

            if (grup != null)
            {
                _context.Gruplar.Remove(grup);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
