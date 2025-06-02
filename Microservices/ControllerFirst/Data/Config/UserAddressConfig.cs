using ControllerFirst.Data.Models;

namespace ControllerFirst.Data.Config;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Country)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Street)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.PostalCode)
            .HasMaxLength(20);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}