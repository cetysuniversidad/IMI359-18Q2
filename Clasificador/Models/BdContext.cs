using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CETYS.Posgrado.imi359.Clasificador.Models
{
    public partial class BdContext : DbContext
    {
        private string _conexionInfo;

        public virtual DbSet<Humano> Humano { get; set; }
        public virtual DbSet<Perturbacion> Perturbacion { get; set; }
        public virtual DbSet<PerturbacionValor> PerturbacionValor { get; set; }
        public virtual DbSet<PerturbacionSoloLectura> PerturbacionLista { get; set; }

        public BdContext()
        {
            var useProdDbText = Environment.GetEnvironmentVariable("PROD_DB") ?? "False";
            var settings = useProdDbText.Equals("True") ? "appsettings.json" : "appsettings.Development.json";

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settings, true, true)
                .Build();

            _conexionInfo = config.GetConnectionString("Connection_String");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_conexionInfo);
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
