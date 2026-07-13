using BelediyeTalepSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!await context.Mudurlukler.AnyAsync())
            {
                context.Mudurlukler.AddRange(
                    new Mudurluk { MudurlukAdi = "Temizlik İşleri" },
                    new Mudurluk { MudurlukAdi = "Fen İşleri" },
                    new Mudurluk { MudurlukAdi = "Park ve Bahçeler" },
                    new Mudurluk { MudurlukAdi = "Zabıta" },
                    new Mudurluk { MudurlukAdi = "Ulaşım Hizmetleri" }
                );
            }

            if (!await context.TalepDurumlari.AnyAsync())
            {
                context.TalepDurumlari.AddRange(
                    new TalepDurumu { DurumAdi = "Yeni" },
                    new TalepDurumu { DurumAdi = "İnceleniyor" },
                    new TalepDurumu { DurumAdi = "Tamamlandı" }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}