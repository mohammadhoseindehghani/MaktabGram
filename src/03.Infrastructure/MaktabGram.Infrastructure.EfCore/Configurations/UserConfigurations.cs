using MaktabGram.Domain.Core.UserAgg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace MaktabGram.Infrastructure.EfCore.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(p => p.Id);

            builder.HasOne(u => u.IdentityUser)
                .WithOne()
                .HasForeignKey<User>(u => u.IdentityUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.Username)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(p => p.PasswordHash)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.OwnsOne(u => u.Mobile, mobile =>
            {
                mobile.Property(m => m.Value)
                .HasColumnName("Mobile")
                .HasMaxLength(11)
                .IsRequired(true);
            });



            builder.HasOne(u=>u.Profile)
                .WithOne(up=>up.User)
                .HasForeignKey<UserProfile>(up=>up.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.HasMany(u=>u.Comments)
                .WithOne(c=>c.User)
                .HasForeignKey(c=>c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u=>u.Followers)
                .WithOne(f=>f.FollowedUser)
                .HasForeignKey(u=>u.FollowedId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Followings)
                .WithOne(f => f.FollowerUser)
                .HasForeignKey(u => u.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u=>u.PostLikes)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u=>u.CommentLikes)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u=>u.SavedPosts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.TaggedPosts)
                .WithOne(c => c.TaggedUser)
                .HasForeignKey(c => c.TaggedUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}


