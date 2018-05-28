using Microsoft.EntityFrameworkCore;

namespace CETYS.Posgrado.imi359.Servicios.Models
{
    public partial class BdContext : DbContext
    {
        public virtual DbSet<Humano> Humano { get; set; }
        public virtual DbSet<Perturbacion> Perturbacion { get; set; }
        public virtual DbSet<PerturbacionValor> PerturbacionValor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;userid=eduardo;pwd=elP@ssword1;database=traficopeatonal-v2;sslmode=none;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Humano>(entity =>
            {
                entity.HasIndex(e => e.FechaIdentificacion)
                    .HasName("idx_humano_fecha_identificacion");

                entity.HasIndex(e => e.PerturbacionId)
                    .HasName("fk_humano_perturbacion_id_idx");

                entity.HasOne(d => d.Perturbacion)
                    .WithMany(p => p.Humano)
                    .HasForeignKey(d => d.PerturbacionId)
                    .HasConstraintName("fk_humano_perturbacion_id");
            });

            modelBuilder.Entity<PerturbacionValor>(entity =>
            {
                entity.HasIndex(e => e.PerturbacionId)
                    .HasName("idx_perturbacion_perturbacion_id");

                entity.HasOne(d => d.Perturbacion)
                    .WithMany(p => p.PerturbacionValor)
                    .HasForeignKey(d => d.PerturbacionId)
                    .HasConstraintName("fk_perturbacion_perturbacion_id");
            });
        }
    }
}
