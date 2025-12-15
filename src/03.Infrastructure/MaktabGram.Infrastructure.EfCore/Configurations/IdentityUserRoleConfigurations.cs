using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaktabGram.Infrastructure.EfCore.Configurations
{
    public class IdentityUserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            var userRoles = new List<IdentityUserRole<int>>()
            {
                new IdentityUserRole<int>()
                {
                    RoleId=1,
                    UserId=1,
                },
            };
            builder.HasData(userRoles);

        }
    }
}
