using System.ComponentModel.DataAnnotations;

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
    }
}