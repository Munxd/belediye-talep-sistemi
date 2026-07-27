namespace BelediyeTalepSistemi.ViewModels
{
    public class DashboardViewModel
    {
        public int ToplamTalep { get; set; }
        public int YeniTalep { get; set; }
        public int InceleniyorTalep { get; set; }
        public int TamamlananTalep { get; set; }

        public List<DashboardChartItem> MudurlukDagilimi { get; set; } = new();
        public List<DashboardChartItem> KategoriDagilimi { get; set; } = new();
        public List<DashboardChartItem> DurumDagilimi { get; set; } = new();

        public List<DashboardMapItem> HaritaTalepleri { get; set; } = new();
    }

    public class DashboardChartItem
    {
        public string Ad { get; set; } = string.Empty;
        public int Sayi { get; set; }
    }

    public class DashboardMapItem
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public string Mudurluk { get; set; } = string.Empty;
        public string Durum { get; set; } = string.Empty;
        public string AcikAdres { get; set; } = string.Empty;
        public double Enlem { get; set; }
        public double Boylam { get; set; }
        public string? FotografYolu { get; set; }
        public string OlusturulmaTarihi { get; set; } = string.Empty;
    }
}