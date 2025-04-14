

using DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data.FluentConfigs;


public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.HasKey(p => p.Id);
        entity.Property(p => p.ProductName).IsRequired().HasMaxLength(255);
        entity.Property(p => p.Description);
        entity.Property(p => p.ImagePath);
        entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
    }
}