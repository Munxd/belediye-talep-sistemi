using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BelediyeTalepSistemi.ViewModels
{
    public class TalepCreateViewModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        public string Baslik { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Aciklama { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori alanı zorunludur.")]
        public string Kategori { get; set; } = string.Empty;

        [Required(ErrorMessage = "Müdürlük seçimi zorunludur.")]
        [Display(Name = "Müdürlük")]
        public int MudurlukId { get; set; }

        [Required(ErrorMessage = "Açık adres alanı zorunludur.")]
        public string AcikAdres { get; set; } = string.Empty;

        public string? Enlem { get; set; }

        public string? Boylam { get; set; }

        public IFormFile? Fotograf { get; set; }
    }
}