using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;

namespace UserContextLib.Configuration
{
    public class UnserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Login)
                   .HasMaxLength(50);

            var user = new User { Id = 1, Login = "admin", Role = "Admin" };

            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.Password = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("pwd123"));
            }
            builder.HasData(user);
        }
    }
}
