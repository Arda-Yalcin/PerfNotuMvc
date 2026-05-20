using System.ComponentModel.DataAnnotations;

namespace MuzikSitesi.Models.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; } = null!;

        [Required]
        public string Ad { get; set; } = null!;

        [Required]
        public string Soyad { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Telefon { get; set; } = null!;

        [Required]
        public string Adres { get; set; } = null!;
    }
}
