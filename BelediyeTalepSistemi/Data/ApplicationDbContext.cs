using BelediyeTalepSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Talep> Talepler { get; set; }
        public DbSet<Mudurluk> Mudurlukler { get; set; }
        public DbSet<TalepDurumu> TalepDurumlari { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}