using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

// IdentityDbContext<AppUser, AppRole, int,
// IdentityUserClaim<int>, AppUserRole, ident>(options) 




public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
    IdentityUserToken<int>>(options)
{
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }
   public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId });

        builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
            .HasOne(s => s.TargetUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);
    }
}

















// public class DataContext1(DbContextOptions options) : DbContext
// {
//    // public DbSet<AppUser> Users { get; set; }
//     public DbSet<UserLike> Likes { get; set; }
//     public DbSet<Message> Messages { get; set; }

//     protected override void OnModelCreating(ModelBuilder builder)
//     {
//         base.OnModelCreating(builder);
//         builder.Entity<UserLike>()
//         .HasKey(k=>new {k.SourceUserId, k.TargetUserId});

//         builder.Entity<UserLike>()
//         .HasOne(like => like.SourceUser)
//         .WithMany(user => user.LikedUsers)
//         .HasForeignKey(like => like.SourceUserId)
//         .OnDelete(DeleteBehavior.Cascade);

//         builder.Entity<UserLike>()
//         .HasOne(like => like.TargetUser)
//         .WithMany(user => user.LikedByUsers)
//         .HasForeignKey(like => like.TargetUserId)
//         .OnDelete(DeleteBehavior.NoAction);

//         builder.Entity<Message>()
//             .HasOne(x => x.Recipient)
//             .WithMany(x => x.MessagesReceived)
//             .OnDelete(DeleteBehavior.Restrict);

//         builder.Entity<Message>()
//             .HasOne(x => x.Sender)
//             .WithMany(x => x.MessagesSent)
//             .OnDelete(DeleteBehavior.Restrict);

//     }
// }
