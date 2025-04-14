using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataLayer.Data.FluentConfigs;


public class AttributeValueConfig: IEntityTypeConfiguration<AttributeValue>
{
    public void Configure(EntityTypeBuilder<AttributeValue> entity)
    {
        entity.HasKey(av => new { av.AttributeId, av.Value });
        entity.Property(av => av.Value).IsRequired().HasMaxLength(255);
        
        entity.HasOne(av => av.ProductAttribute)
            .WithMany(a => a.AttributeValues)
            .HasForeignKey(av => av.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(av => av.Category)
            .WithMany(c => c.AttributeValues)
            .HasForeignKey(av => av.CategoryRef)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
