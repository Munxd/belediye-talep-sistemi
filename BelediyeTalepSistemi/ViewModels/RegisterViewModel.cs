using System.ComponentModel.DataAnnotations;

namespace BelediyeTalepSistemi.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Sifre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string SifreTekrar { get; set; } = string.Empty;
    }
}