using Microsoft.EntityFrameworkCore;

namespace FacilityRepoEF
{

    public class FacilityContext : DbContext
    {
        public DbSet<Unit> units { get; set; } = null!;
        public DbSet<Tank> tanks { get; set; } = null!;
        public DbSet<Factory> factories { get; set; } = null!;


        public FacilityContext(DbContextOptions<FacilityContext> options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseSerialColumns();
        }
    }
}