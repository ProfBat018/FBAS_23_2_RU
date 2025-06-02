using ControllerFirst.Data.Models;

namespace ControllerFirst.Data.Config;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserCardConfig : IEntityTypeConfiguration<UserCard>
{
    public void Configure(EntityTypeBuilder<UserCard> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CardHolder)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.CardNumberEncrypted)
            .IsRequired();

        builder.Property(c => c.CVVEncrypted)
            .IsRequired();

        builder.Property(c => c.ExpirationMonth)
            .IsRequired();

        builder.Property(c => c.ExpirationYear)
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}