using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuzikSitesi.Models
{
    public class Grup
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Kurucusu { get; set; }
        public string? Katilimcilar { get; set; }
        public List<Cd>? Cdler { get; set; }
    }
}
