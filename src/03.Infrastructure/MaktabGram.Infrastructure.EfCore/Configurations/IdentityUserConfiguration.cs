using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MaktabGram.Infrastructure.EfCore.Configurations
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUser<int>> builder)
        {
            PasswordHasher<IdentityUser<int>> passwordHasher = new PasswordHasher<IdentityUser<int>>(); 

            var user = new IdentityUser<int>
            {
                Id = 1,
                UserName = "0937507920",
                NormalizedUserName = "09377507920",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                ConcurrencyStamp = new string(Guid.NewGuid().ToString()),
                SecurityStamp = new string(Guid.NewGuid().ToString()),
            };

            user.PasswordHash = passwordHasher.HashPassword(user, "123456");
            builder.HasData(user);
        }
    }
}