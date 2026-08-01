using System.Globalization;

namespace BelediyeTalepSistemi.Services
{
    public class TalepAIService
    {
        public TalepAIResult AnalizEt(string? baslik, string? aciklama)
        {
            var metin = $"{baslik} {aciklama}".ToLower(new CultureInfo("tr-TR"));

            var mudurlukPuanlari = new Dictionary<string, int>
            {
                { "Temizlik İşleri", 0 },
                { "Fen İşleri", 0 },
                { "Park ve Bahçeler", 0 },
                { "Zabıta", 0 },
                { "Ulaşım Hizmetleri", 0 }
            };

            var eslesenKelimeler = new List<string>();

            PuanEkle(metin, mudurlukPuanlari, "Temizlik İşleri", 30, eslesenKelimeler,
                "çöp", "cop", "konteyner", "temizlik", "koku", "atık", "atik", "çöp kutusu");

            PuanEkle(metin, mudurlukPuanlari, "Fen İşleri", 30, eslesenKelimeler,
                "yol", "çukur", "cukur", "kaldırım", "kaldirim", "asfalt", "bozuk", "onarım", "onarim", "su baskını", "su baskini");

            PuanEkle(metin, mudurlukPuanlari, "Park ve Bahçeler", 30, eslesenKelimeler,
                "park", "bahçe", "bahce", "ağaç", "agac", "yeşil", "yesil", "çim", "cim", "oyun alanı");

            PuanEkle(metin, mudurlukPuanlari, "Zabıta", 30, eslesenKelimeler,
                "gürültü", "gurultu", "seyyar", "işgal", "isgal", "rahatsız", "rahatsiz", "denetim", "kaçak");

            PuanEkle(metin, mudurlukPuanlari, "Ulaşım Hizmetleri", 30, eslesenKelimeler,
                "otobüs", "otobus", "durak", "trafik", "ulaşım", "ulasim", "yaya", "sinyalizasyon", "trafik ışığı", "trafik isigi");

            var enYuksekMudurluk = mudurlukPuanlari
                .OrderByDescending(x => x.Value)
                .First();

            string mudurlukAdi = enYuksekMudurluk.Value > 0
                ? enYuksekMudurluk.Key
                : "Fen İşleri";

            string kategori = KategoriBelirle(metin);
            string oncelik = OncelikBelirle(metin);

            int guvenOrani = GuvenOraniHesapla(enYuksekMudurluk.Value, oncelik);

            string aciklamaMetni;

            if (eslesenKelimeler.Any())
            {
                aciklamaMetni = $"Metinde {string.Join(", ", eslesenKelimeler.Distinct())} ifadeleri bulundu. Bu nedenle talep {mudurlukAdi} müdürlüğüne yönlendirilebilir.";
            }
            else
            {
                aciklamaMetni = "Metinde belirgin bir anahtar kelime bulunamadı. Bu nedenle varsayılan müdürlük önerisi yapıldı.";
            }

            return new TalepAIResult
            {
                Kategori = kategori,
                MudurlukAdi = mudurlukAdi,
                OncelikSeviyesi = oncelik,
                GuvenOrani = guvenOrani,
                Aciklama = aciklamaMetni
            };
        }

        private void PuanEkle(
            string metin,
            Dictionary<string, int> puanlar,
            string mudurlukAdi,
            int puan,
            List<string> eslesenKelimeler,
            params string[] kelimeler)
        {
            foreach (var kelime in kelimeler)
            {
                if (metin.Contains(kelime))
                {
                    puanlar[mudurlukAdi] += puan;
                    eslesenKelimeler.Add(kelime);
                }
            }
        }

        private string KategoriBelirle(string metin)
        {
            if (KelimeVarMi(metin, "öneri", "oneri", "öneriyorum", "oneriyorum", "yapılabilir", "yapilabilir"))
            {
                return "Öneri";
            }

            if (KelimeVarMi(metin, "şikayet", "sikayet", "rahatsız", "rahatsiz", "bozuk", "koku", "alınmıyor", "alinmiyor", "tehlike"))
            {
                return "Şikâyet";
            }

            return "Talep";
        }

        private string OncelikBelirle(string metin)
        {
            if (KelimeVarMi(metin,
                "acil", "tehlike", "tehlikeli", "kaza", "yangın", "yangin",
                "su baskını", "su baskini", "çökme", "cokme", "yıkılma", "yikilma",
                "yaralanma", "araçlar geçemiyor", "araclar gecemiyor"))
            {
                return "Yüksek";
            }

            if (KelimeVarMi(metin, "öneri", "oneri", "istek", "talep ediyorum", "yapılabilir", "yapilabilir"))
            {
                return "Düşük";
            }

            return "Orta";
        }

        private int GuvenOraniHesapla(int mudurlukPuani, string oncelik)
        {
            int guven = 40 + mudurlukPuani;

            if (oncelik == "Yüksek")
            {
                guven += 10;
            }

            if (guven > 95)
            {
                guven = 95;
            }

            return guven;
        }

        private bool KelimeVarMi(string metin, params string[] kelimeler)
        {
            return kelimeler.Any(kelime => metin.Contains(kelime));
        }
    }

    public class TalepAIResult
    {
        public string Kategori { get; set; } = string.Empty;
        public string MudurlukAdi { get; set; } = string.Empty;
        public string OncelikSeviyesi { get; set; } = "Orta";
        public int GuvenOrani { get; set; }
        public string Aciklama { get; set; } = string.Empty;
    }
}