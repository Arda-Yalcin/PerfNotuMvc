using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuzikSitesi.Data;
using MuzikSitesi.Models;
using MuzikSitesi.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MuzikSitesi.Controllers
{    
    public class AlbumController : Controller
    {

        private readonly AppDbContext _context;
  
        public AlbumController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var albumler = _context.Albumler.Include(u => u.Grup).ToList();
            return View (albumler);
        }

         public IActionResult Ekle()
        {
            //   Veritabanındaki kategorileri çekip dropdown için viewbag içine atıyoruz
            //   "Id" arka planda kaydedilecek değer, "Ad" ise kullanıcıya gösterilecek metindir.

            ViewBag.GrupListesi=new SelectList(_context.Gruplar,"Id","Ad");
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Album album, IFormFile Foto)
        {
            //Model kurallarına uyuyorsa veri tabanına ekle
            if(ModelState.IsValid)
            {
                if(Foto != null)
                {
                    //Fotoğrafın uzantısını almak
                    var uzanti=Path.GetExtension(Foto.FileName);
                    var yendiAd=Guid.NewGuid()+"."+ uzanti;
                    var yol=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",yendiAd);

                    if(Foto.ContentType=="image/png"|| Foto.ContentType=="image/jpeg" ||Foto.ContentType=="image/jpg")
                    {
                        using(var stream=new FileStream(yol,FileMode.Create))
                        {
                            try
                            {
                                Foto.CopyTo(stream);
                                album.Foto=yendiAd;
                                _context.Albumler.Add(album);// fotoğrafı ekledi
                                _context.SaveChanges(); //veri tabanına kaydetti
                                return RedirectToAction("Index","Album");
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Hata="Dosya Yükleme Hatası :"+ex.Message;
                            }
                        }
                    } else { ViewBag.Hata="jpg ya da png Yükle";}

                }
            }else {ViewBag.Hata="Dosya Yükle";}
            //eğer bir hata varsa formu hatalarla birlikte tekrar göster
            ViewBag.GrupListesi=new SelectList(_context.Gruplar,"Id","Ad");
            return View(album);
        }



        public IActionResult Sil(int? id)
        {

            if(id==null) return NotFound();
            //Silinecek ürünü ve kategorisini buluyoruz
            var album = _context.Albumler.Include(u => u.Grup).FirstOrDefault(m => m.Id==id);

            if(album==null) return NotFound();
            return View(album);
        }



        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public IActionResult SilOnay(int id)
        {
            var album = _context.Albumler.Find(id);
            if(album != null)
            {
                _context.Albumler.Remove(album);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }



        public IActionResult Edit(int id)
        {
            if(id != null)
            {
                var album = _context.Albumler.FirstOrDefault(x=>x.Id==id);
                if(album != null)
                {
                    ViewBag.GrupListesi=new SelectList(_context.Gruplar,"Id","Ad");
                    return View(album);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Album album,IFormFile Foto)
        {
            if(ModelState.IsValid)
            {
                var existingAlbum = _context.Albumler.FirstOrDefault(x=>x.Id==album.Id);
                if(existingAlbum != null)
                {
                    existingAlbum.Ad = album.Ad;
                    existingAlbum.GrupId = album.GrupId;
                    
                    if(Foto != null)
                    {
                        //Foto�raf�n uzant�s�n� almak
                        var uzanti=Path.GetExtension(Foto.FileName);
                        var yendiAd=Guid.NewGuid()+"."+ uzanti;
                        var yol=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\img",yendiAd);

                        if(Foto.ContentType=="image/png"|| Foto.ContentType=="image/jpeg" ||Foto.ContentType=="image/jpg")
                        {
                            using(var stream=new FileStream(yol,FileMode.Create))
                            {
                                try
                                {
                                    Foto.CopyTo(stream);
                                    existingAlbum.Foto = yendiAd;
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.Hata="Dosya Y�kleme Hatas� :"+ex.Message;
                                    ViewBag.GrupListesi=new SelectList(_context.Gruplar,"Id","Ad");
                                    return View(album);
                                }
                            }
                        } else { 
                            ViewBag.Hata="jpg ya da png Y�kle";
                            ViewBag.GrupListesi=new SelectList(_context.Gruplar,"Id","Ad");
                            return View(album);
                        }
                    }
                    
                    _context.SaveChanges();
                    return RedirectToAction("Index","Album");
                }
            }
            ViewBag.GrupListesi=new SelectList(_context.Gruplar,"Id","Ad");
            return View(album);
        }

    }
}


