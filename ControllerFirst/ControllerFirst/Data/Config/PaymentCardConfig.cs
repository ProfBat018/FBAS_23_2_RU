using ControllerFirst.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControllerFirst.Data.Config;


public class PaymentCardConfig : IEntityTypeConfiguration<PaymentCard>
{
    public void Configure(EntityTypeBuilder<PaymentCard> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK_PaymentCard");

        entity.Property(e => e.Id)
            .HasColumnName("id");

        entity.Property(e => e.CardHolderName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("cardHolderName");

        entity.Property(e => e.EncryptedCardNumber)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("encryptedCardNumber");

        entity.Property(e => e.EncryptedExpiryDate)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("encryptedExpiryDate");

        entity.Property(e => e.EncryptedCVC)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("encryptedCvc");

        entity.Property(e => e.UserInfoId)
            .HasColumnName("userInfoId");
    }
}