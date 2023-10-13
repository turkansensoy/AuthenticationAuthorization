using System.ComponentModel.DataAnnotations;

namespace AuthenticationCookie.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı giriniz.")]
        [StringLength(30, ErrorMessage = "Kullanıcı max 30 karakter olmalıdır.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre giriniz")]
        [MinLength(6, ErrorMessage = "Şifreniz min 6 karakter olmalı")]
        [MaxLength(16, ErrorMessage = "Şifreniz max 16 karakter olmalı")]
        public string Password { get; set; }
    }
}
