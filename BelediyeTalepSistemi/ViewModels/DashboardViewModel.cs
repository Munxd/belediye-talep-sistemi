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
    }

    public class DashboardChartItem
    {
        public string Ad { get; set; } = string.Empty;
        public int Sayi { get; set; }
    }
}