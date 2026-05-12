using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MuzikSitesi.Models
{
    public class AppUser:IdentityUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
    }
}