using MaktabGram.Domain.Core.CommentAgg.Entities;
using MaktabGram.Domain.Core.FollowerAgg.Entities;
using MaktabGram.Domain.Core.PostAgg.Entities;
using MaktabGram.Domain.Core.UserAgg.Entities;
using MaktabGram.Infrastructure.EfCore.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MaktabGram.Infrastructure.EfCore.Persistence
{
    public class AppDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Otp> Otps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }

}
