using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }

    public DbSet<UserLike> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserLike>()
        .HasKey(k=>new {k.SourceUserId, k.TargetUserId});

        builder.Entity<UserLike>()
        .HasOne(like => like.SourceUser)
        .WithMany(user => user.LikedUsers)
        .HasForeignKey(like => like.SourceUserId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
        .HasOne(like => like.TargetUser)
        .WithMany(user => user.LikedByUsers)
        .HasForeignKey(like => like.TargetUserId)
        .OnDelete(DeleteBehavior.NoAction);

    }
}
