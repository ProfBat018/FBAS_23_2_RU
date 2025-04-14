using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataLayer.Data.FluentConfigs;

public class AttributeConfig: IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> entity)
    {
        entity.HasKey(a => a.Id);
        entity.Property(a => a.Name).IsRequired().HasMaxLength(255);

        entity.HasMany(a => a.AttributeValues)
            .WithOne(av => av.ProductAttribute)
            .HasForeignKey(av => av.AttributeId);

        entity.HasMany(a => a.CategoryAttributes)
            .WithOne(ca => ca.ProductAttribute)
            .HasForeignKey(ca => ca.AttributesRef);
    }
}