﻿using Microsoft.EntityFrameworkCore;

namespace FacilityRepoEF
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
            builder.UseSerialColumns();
        }
    }
}