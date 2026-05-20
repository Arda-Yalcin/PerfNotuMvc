using System.ComponentModel.DataAnnotations;

namespace MuzikSitesi.Models.ViewModels
{
    // Adminin duzenleyebildigi kullanici profil alanlari.
    public class UserEditViewModel
    {
        public string Id { get; set; } = null!;

        [Required]
        public string Ad { get; set; } = null!;

        [Required]
        public string Soyad { get; set; } = null!;

        // E-posta sadece goruntulenir, admin tarafindan degistirilmez.
        public string? Email { get; set; }

        [Required]
        public string Telefon { get; set; } = null!;

        [Required]
        public string Adres { get; set; } = null!;
    }
}
