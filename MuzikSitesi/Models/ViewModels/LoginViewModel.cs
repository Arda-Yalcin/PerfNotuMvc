using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace MuzikSitesi.Models.ViewModels
{
    // Giris formundan gelen kimlik bilgileri.
    public class LoginViewModel
    {
        [Required(ErrorMessage ="EPosta Gereklidir")]
        [EmailAddress(ErrorMessage ="Geçerli bir E Posta girin!")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Şifre Gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
