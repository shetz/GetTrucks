using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }

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

        builder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
