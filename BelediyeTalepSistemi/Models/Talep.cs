namespace BelediyeTalepSistemi.Models
{
    public class Talep
    {
        public int Id { get; set; }

        public string Baslik { get; set; } = string.Empty;

        public string Aciklama { get; set; } = string.Empty;

        public string Kategori { get; set; } = string.Empty;

        public string OncelikSeviyesi { get; set; } = "Orta";

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        public int ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int MudurlukId { get; set; }
        public Mudurluk? Mudurluk { get; set; }

        public int TalepDurumuId { get; set; }
        public TalepDurumu? TalepDurumu { get; set; }
    }
}