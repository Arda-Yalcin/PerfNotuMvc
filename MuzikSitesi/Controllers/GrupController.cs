using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
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
            return View (gruplar);
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

            if(id==null) return NotFound();
            //Silinecek ürünü ve kategorisini buluyoruz
            var grup = _context.Gruplar.FirstOrDefault(m => m.Id==id);

            if(grup==null) return NotFound();
            return View(grup);
        }

        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult SilOnay(int id)
        {
            bool bagliAlbumVarmi= _context.Albumler.Any(u => u.GrupId == id);
            if (bagliAlbumVarmi)
            {
                TempData["HataMesaji"]="Bu Grup Silinemez! Çünkü Bu Grubun Içerisinde Hala Album Bulunmakta";
                return RedirectToAction(nameof(Sil), new{id=id});
            }
            var grup = _context.Gruplar.Find(id);

            if(grup != null)
            {
                _context.Gruplar.Remove(grup);
                _context.SaveChanges();
            }
            return RedirectToAction("Index"); //nameof ile aynı anlama geliyor
        }



        
    }
}