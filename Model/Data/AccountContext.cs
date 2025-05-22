using EasyTrackerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EasyTrackerAPI.Data;
public class AccountContext : IdentityDbContext<IdentityUser>
{
    public AccountContext(DbContextOptions<AccountContext> options)
        : base(options) { }
    public DbSet<UserAdditionalInfo> UserAdditionalInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserAdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                 .WithMany()
                 .HasForeignKey(e => e.UserId);
        });
        builder.Entity<IdentityUser>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });
    }

    
}