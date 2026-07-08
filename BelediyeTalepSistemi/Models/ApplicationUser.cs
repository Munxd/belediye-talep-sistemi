namespace BelediyeTalepSistemi.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string AdSoyad { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Sifre { get; set; } = string.Empty;

        public string Rol { get; set; } = "Vatandas";

        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        public ICollection<Talep> Talepler { get; set; } = new List<Talep>();
    }
}