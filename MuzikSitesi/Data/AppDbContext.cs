using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuzikSitesi.Models;

namespace MuzikSitesi.Data
{
    // Identity tablolari ile uygulama tablolarini ayni DbContext uzerinden yonetir.
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // Muzik sitesi alan tablolari.
        public DbSet<Grup> Gruplar { get; set; }
        public DbSet<Cd> Cdler { get; set; }
        public DbSet<CartItem> SepetKalemleri { get; set; }
        public DbSet<CdRental> CdKiralamalari { get; set; }
    }
}
