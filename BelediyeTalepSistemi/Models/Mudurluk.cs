namespace BelediyeTalepSistemi.Models
{
    public class Mudurluk
    {
        public int Id { get; set; }

        public string MudurlukAdi { get; set; } = string.Empty;

        public string? Aciklama { get; set; }

        public ICollection<Talep> Talepler { get; set; } = new List<Talep>();
    }
}