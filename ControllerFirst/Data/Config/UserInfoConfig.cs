using ControllerFirst.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControllerFirst.Data.Config;

public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK_UserInfo");

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName("userId");

        entity.HasOne(e => e.User)
            .WithOne() // предполагаем, что связь 1:1
            .HasForeignKey<UserInfo>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Addresses)
            .WithOne(a => a.UserInfo)
            .HasForeignKey(a => a.UserInfoId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(e => e.Cards)
            .WithOne(c => c.UserInfo)
            .HasForeignKey(c => c.UserInfoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}