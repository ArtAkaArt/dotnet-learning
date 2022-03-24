using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FacilityRepoEF
{
    public partial class FacilitiesContext : DbContext
    {
        public FacilitiesContext()
        {
        }

        public FacilitiesContext(DbContextOptions<FacilitiesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Factory> Factories { get; set; } = null!;
        public virtual DbSet<Tank> Tanks { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Database=Facilities;Port=5432;User Id=artakaart;Password=123123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Factory>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Name, e.Description })
                    .HasName("Factories_pkey");

                entity.ToTable("factories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<Tank>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Name, e.Volume, e.Maxvolume, e.Unitid })
                    .HasName("Tanks_pkey");

                entity.ToTable("tanks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.Property(e => e.Maxvolume).HasColumnName("maxvolume");

                entity.Property(e => e.Unitid).HasColumnName("unitid");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Name, e.Factoryid })
                    .HasName("Units_pkey");

                entity.ToTable("units");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Factoryid).HasColumnName("factoryid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
