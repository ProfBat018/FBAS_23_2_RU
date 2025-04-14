

using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.FluentConfigs;


public class ProductCategoryConfig: IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> entity)
    {
        entity.HasKey(pc => new { pc.CategoryRef, pc.ProductRef });
        
        entity.HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryRef)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductRef)
            .OnDelete(DeleteBehavior.Cascade);
    }
}