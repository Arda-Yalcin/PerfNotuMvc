using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuzikSitesi.Models
{
    public class Cd
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Foto { get; set; }

        public int GrupId { get; set; }
        public Grup? Grup { get; set; }
        public int Stock { get; set; } = 0;
    }
}
