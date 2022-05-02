using Microsoft.EntityFrameworkCore;
using WebApiFutbolistasSeg.Entidades;

namespace WebApiFutbolistasSeg
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FutbolistaEquipo>().HasKey(ft => new { ft.FutbolistaId, ft.EquipoId });
        }
        public DbSet<Futbolista> Futbolistas { get; set; }

        public DbSet<Equipo> Equipos { get; set; }

        public DbSet<Ligas> Ligas { get; set; }

        public DbSet<FutbolistaEquipo> FutbolistaEquipo { get; set; }
    }
}
