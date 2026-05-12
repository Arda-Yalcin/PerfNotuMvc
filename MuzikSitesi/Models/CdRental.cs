using System;

namespace MuzikSitesi.Models
{
    public class CdRental
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public int CdId { get; set; }
        public Cd? Cd { get; set; }

        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        public DateTime DueDate => RentDate.AddDays(15);
    }
}
