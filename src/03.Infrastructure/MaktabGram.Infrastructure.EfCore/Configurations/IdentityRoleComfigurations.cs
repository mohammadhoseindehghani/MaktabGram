using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MaktabGram.Infrastructure.EfCore.Configurations
{
    public class IdentityRoleComfigurations : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            var roels = new List<IdentityRole<int>>()
            {
                new IdentityRole<int>()
                {
                    Id=1,
                    Name="Admin",
                    NormalizedName="ADMIN",
                    ConcurrencyStamp=new string (Guid.NewGuid().ToString()),
                },
                new IdentityRole<int>()
                {
                    Id=2,
                    Name="User",
                    NormalizedName="USER",
                    ConcurrencyStamp=new string (Guid.NewGuid().ToString()),
                },
            };

            builder.HasData(roels);
        }
    }
}
