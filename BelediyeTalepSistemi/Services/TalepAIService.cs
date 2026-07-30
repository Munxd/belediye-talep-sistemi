namespace BelediyeTalepSistemi.Services
{
    public class TalepAIService
    {
        public TalepAIResult AnalizEt(string? baslik, string? aciklama)
        {
            var metin = $"{baslik} {aciklama}".ToLower();

            var sonuc = new TalepAIResult
            {
                Kategori = "Talep",
                MudurlukAdi = "Fen İşleri",
                Aciklama = "Metin genel bir talep olarak değerlendirildi."
            };

            if (KelimeVarMi(metin, "çöp", "cop", "konteyner", "temizlik", "koku", "atık", "atik"))
            {
                sonuc.Kategori = "Şikâyet";
                sonuc.MudurlukAdi = "Temizlik İşleri";
                sonuc.Aciklama = "Metinde temizlik, çöp veya atıkla ilgili ifadeler bulundu.";
            }
            else if (KelimeVarMi(metin, "yol", "çukur", "cukur", "kaldırım", "kaldirim", "asfalt", "bozuk", "onarım", "onarim"))
            {
                sonuc.Kategori = "Şikâyet";
                sonuc.MudurlukAdi = "Fen İşleri";
                sonuc.Aciklama = "Metinde yol, asfalt, kaldırım veya onarım ile ilgili ifadeler bulundu.";
            }
            else if (KelimeVarMi(metin, "park", "bahçe", "bahce", "ağaç", "agac", "yeşil", "yesil", "çim", "cim", "oyun alanı"))
            {
                sonuc.Kategori = "Talep";
                sonuc.MudurlukAdi = "Park ve Bahçeler";
                sonuc.Aciklama = "Metinde park, bahçe veya yeşil alanla ilgili ifadeler bulundu.";
            }
            else if (KelimeVarMi(metin, "gürültü", "gurultu", "seyyar", "işgal", "isgal", "rahatsız", "rahatsiz", "denetim"))
            {
                sonuc.Kategori = "Şikâyet";
                sonuc.MudurlukAdi = "Zabıta";
                sonuc.Aciklama = "Metinde denetim, gürültü veya zabıta ile ilgili ifadeler bulundu.";
            }
            else if (KelimeVarMi(metin, "otobüs", "otobus", "durak", "trafik", "ulaşım", "ulasim", "yaya", "sinyalizasyon"))
            {
                sonuc.Kategori = "Şikâyet";
                sonuc.MudurlukAdi = "Ulaşım Hizmetleri";
                sonuc.Aciklama = "Metinde ulaşım, trafik veya durakla ilgili ifadeler bulundu.";
            }
            else if (KelimeVarMi(metin, "öneri", "oneri", "öneriyorum", "oneriyorum", "yapılabilir", "yapilabilir"))
            {
                sonuc.Kategori = "Öneri";
                sonuc.MudurlukAdi = "Fen İşleri";
                sonuc.Aciklama = "Metin öneri olarak değerlendirildi.";
            }

            return sonuc;
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
        public string Aciklama { get; set; } = string.Empty;
    }
}