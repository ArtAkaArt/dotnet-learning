using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;


namespace UserContextLib
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
            var user = new User {Id = 1, Login = "admin", Role = "Admin"};

            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.Password = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("pwd123"));
            }
            builder.Entity<User>().HasData(user);
        }
    }
        public class UnserEntityTypeConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.Property(u => u.Login)
                       .HasMaxLength(50);
            }
        }
}