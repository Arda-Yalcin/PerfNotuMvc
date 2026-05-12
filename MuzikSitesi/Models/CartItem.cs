using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuzikSitesi.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public int? AlbumId { get; set; }
        public Album? Album { get; set; }

        public int? CdId { get; set; }
        public Cd? Cd { get; set; }

        public int Quantity { get; set; }
    }
}
