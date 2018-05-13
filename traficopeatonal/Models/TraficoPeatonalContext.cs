using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CETYS.Posgrado.imi359.Models
{
    public partial class TraficoPeatonalContext : DbContext
    {
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventValueComputed> EventValueComputed { get; set; }
        public virtual DbSet<EventValueRaw> EventValueRaw { get; set; }
        public virtual DbSet<Human> Human { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;userid=eduardo;pwd=elP@ssword1;database=traficopeatonal-v1;sslmode=none;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventValueComputed>(entity =>
            {
                entity.HasIndex(e => e.EventId)
                    .HasName("idx_event_value_event_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventValueComputed)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("fk_event_value_computed_event_id");
            });

            modelBuilder.Entity<EventValueRaw>(entity =>
            {
                entity.HasIndex(e => e.EventId)
                    .HasName("idx_event_value_event_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventValueRaw)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("fk_event_value_raw_event_id");
            });

            modelBuilder.Entity<Human>(entity =>
            {
                entity.HasIndex(e => e.EventId)
                    .HasName("fk_human_event_id_idx");

                entity.HasIndex(e => e.IdentificationDate)
                    .HasName("idx_human_identification_date");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Human)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("fk_human_event_id");
            });
        }
    }
}
