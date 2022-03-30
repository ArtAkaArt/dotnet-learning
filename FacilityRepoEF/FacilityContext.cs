using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityContextLib
{

    public class FacilityContext : DbContext
    {
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Tank> Tanks { get; set; } = null!;
        public DbSet<Factory> Factories { get; set; } = null!;


        public FacilityContext(DbContextOptions<FacilityContext> options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            new UnitEntityTypeConfiguration().Configure(builder.Entity<Unit>());
            new TankEntityTypeConfiguration().Configure(builder.Entity<Tank>());
            new FactoryEntityTypeConfiguration().Configure(builder.Entity<Factory>());

            builder.UseSerialColumns();
        }
    }
    public class UnitEntityTypeConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(u => u.Name)
                   .HasMaxLength(50);
            builder.Property(u => u.Description)
                   .HasMaxLength(50);
        }
    }
    public class TankEntityTypeConfiguration : IEntityTypeConfiguration<Tank>
    {
        public void Configure(EntityTypeBuilder<Tank> builder)
        {
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(u => u.Name)
                   .HasMaxLength(50);
            builder.Property(u => u.Description)
                   .HasMaxLength(50);
        }
    }
    public class FactoryEntityTypeConfiguration : IEntityTypeConfiguration<Factory>
    {
        public void Configure(EntityTypeBuilder<Factory> builder)
        {
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(u => u.Name)
                   .HasMaxLength(50);
            builder.Property(u => u.Description)
                   .HasMaxLength(50);
        }
    }
}