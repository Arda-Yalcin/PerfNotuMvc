using System;

namespace MuzikSitesi.Models
{
    // CD kiralama, onay ve iade durumlarini saklar.
    public class CdRental
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = null!;
        public AppUser? AppUser { get; set; }

        public int CdId { get; set; }
        public Cd? Cd { get; set; }

        public int Quantity { get; set; } = 1;
        public DateTime RentDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool ReturnRequested { get; set; }
        public DateTime? ReturnRequestDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        // Admin tarafindan degistirilebilen son teslim tarihi.
        public DateTime? DueDate { get; set; }
    }
}
