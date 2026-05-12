using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MuzikSitesi.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Ad { get; set; }
        
        [Required]
        public string Soyad { get; set; }
        
        [Required]
        public string Adres { get; set; }
        
        [StringLength(11)]
        [Required]
        public string Telefon { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Şifreler Uyuşmuyor")]
        [Required,DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}