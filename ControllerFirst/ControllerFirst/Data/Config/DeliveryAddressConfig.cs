using ControllerFirst.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControllerFirst.Data.Config;

public class DeliveryAddressConfig : IEntityTypeConfiguration<DeliveryAddress>
{
    public void Configure(EntityTypeBuilder<DeliveryAddress> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK_DeliveryAddress");

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("fullName");

        entity.Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .HasColumnName("phoneNumber");

        entity.Property(e => e.AddressLine1)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("addressLine1");

        entity.Property(e => e.AddressLine2)
            .HasMaxLength(200)
            .HasColumnName("addressLine2");

        entity.Property(e => e.City)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("city");

        entity.Property(e => e.State)
            .HasMaxLength(100)
            .HasColumnName("state");

        entity.Property(e => e.ZipCode)
            .HasMaxLength(20)
            .HasColumnName("zipCode");

        entity.Property(e => e.Country)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("country");

        entity.Property(e => e.UserInfoId)
            .HasColumnName("userInfoId");
    }
}